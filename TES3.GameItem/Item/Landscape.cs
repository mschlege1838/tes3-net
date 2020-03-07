using System.Collections.Generic;
using System.IO;

using TES3.Core;
using TES3.Records;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("LAND")]
    public class Landscape : TES3GameItem
    {
        const int HEIGHT_MAP_FLAG = 0x01;
        const int COLOR_MAP_FLAG = 0x02;
        const int TEXTURE_FLAG = 0x04;
        const int CS_GENERATED_FLAG = 0x08;

        TES3VertexNormal[,] normalVectorMapping;
        sbyte[,] slopeMapping;
        byte[,] worldMapPalette;
        TES3VertexColor[,] colorMapping;

        public Landscape(LandscapeGridKey grid) : base(grid)
        {

        }

        public Landscape(Record record) : base(record)
        {
            
        }

        public override string RecordName => "LAND";

        [IdField]
        public LandscapeGridKey Grid
        {
            get => (LandscapeGridKey) Id;
            set => Id = value;
        }


        public bool CSGenerated
        {
            get;
            set;
        } = true;

        public float BaseHeight
        {
            get;
            set;
        }

        public LandTextureMap TextureMap
        {
            get;
            set;
        }

        public TES3VertexNormal[,] NormalVectorMapping
        {
            get => normalVectorMapping;
            set => normalVectorMapping = Validation.ValidateSquare(value, VertexNormalSubRecord.MAPPING_SIDE_LENGTH, "value", "Normal Vector Mapping", true);
        }

        public sbyte[,] SlopeMapping
        {
            get => slopeMapping;
            set => slopeMapping = Validation.ValidateSquare(value, HeightMapSubRecord.MAPPING_SIDE_LENGTH, "value", "Slope Mapping", true);
        }

        public byte[,] WorldMapPalette
        {
            get => worldMapPalette;
            set => worldMapPalette = Validation.ValidateSquare(value, WorldMapPaletteSubRecord.PALETTE_SIDE_LENGTH, "value", "Palette", true);
        }

        public TES3VertexColor[,] ColorMapping
        {
            get => colorMapping;
            set => colorMapping = Validation.ValidateSquare(value, VertexColorSubRecord.COLOR_MAPPING_SIDE_LENGTH, "value", "Color Mapping", true);
        }

        public bool HasHeightMapData
        {
            get => NormalVectorMapping != null || SlopeMapping != null || WorldMapPalette != null;
        }

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            CheckState();

            subRecords.Add(new GridSubRecord("INTV", Grid.GridX, Grid.GridY));

            var flags = 0;
            if (HasHeightMapData)
            {
                flags |= HEIGHT_MAP_FLAG;
            }
            if (ColorMapping != null)
            {
                flags |= COLOR_MAP_FLAG;
            }
            if (TextureMap != null)
            {
                flags |= TEXTURE_FLAG;
            }
            if (CSGenerated)
            {
                flags |= CS_GENERATED_FLAG;
            }
            subRecords.Add(new IntSubRecord("DATA", flags));

        }

        protected override void UpdateRequired(Record record)
        {
            CheckState();

            var gridSubRecord = record.GetSubRecord<GridSubRecord>("INTV");
            gridSubRecord.GridX = Grid.GridX;
            gridSubRecord.GridY = Grid.GridY;

            var dataSubRecord = record.GetSubRecord<IntSubRecord>("DATA");
            FlagSet(dataSubRecord, HasHeightMapData, HEIGHT_MAP_FLAG, IntFlagSet, IntFlagClear);
            FlagSet(dataSubRecord, ColorMapping != null, COLOR_MAP_FLAG, IntFlagSet, IntFlagClear);
            FlagSet(dataSubRecord, TextureMap != null, TEXTURE_FLAG, IntFlagSet, IntFlagClear);
            FlagSet(dataSubRecord, CSGenerated, CS_GENERATED_FLAG, IntFlagSet, IntFlagClear);

        }

        protected override void UpdateOptional(Record record)
        {
            if (HasHeightMapData)
            {
                ProcessOptional(record, "VNML", true, () => new VertexNormalSubRecord("VNML", NormalVectorMapping),
                    (sr) => sr.NormalVectorMapping = NormalVectorMapping);

                ProcessOptional(record, "VHGT", true, () => new HeightMapSubRecord("VHGT", BaseHeight, SlopeMapping, new byte[3]),
                    (sr) =>
                    {
                        sr.BaseHeight = BaseHeight;
                        sr.SlopeMapping = SlopeMapping;
                    }
                );

                ProcessOptional(record, "WNAM", true, () => new WorldMapPaletteSubRecord("WNAM", WorldMapPalette),
                    (sr) => sr.Palette = WorldMapPalette);
            }

            ProcessOptional(record, "VTEX", TextureMap != null, () => new LandTextureSubRecord("VTEX", TextureMap.ProcessCopy()),
                (sr) => sr.Data = TextureMap.ProcessCopy());
            ProcessOptional(record, "VCLR", ColorMapping != null, () => new VertexColorSubRecord("VCLR", ColorMapping),
                (sr) => sr.ColorMapping = ColorMapping);
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            var gridSubRecord = record.GetSubRecord<GridSubRecord>("INTV");
            Id = new LandscapeGridKey(gridSubRecord.GridX, gridSubRecord.GridY);

            CSGenerated = HasFlagSet(record.GetSubRecord<IntSubRecord>("DATA").Data, CS_GENERATED_FLAG);


            // Optional
            NormalVectorMapping = record.TryGetSubRecord<VertexNormalSubRecord>("VNML")?.NormalVectorMapping;

            var heightMapSubRecord = record.TryGetSubRecord<HeightMapSubRecord>("VHGT");
            if (heightMapSubRecord != null)
            {
                BaseHeight = heightMapSubRecord.BaseHeight;
                SlopeMapping = heightMapSubRecord.SlopeMapping;
            }

            WorldMapPalette = record.TryGetSubRecord<WorldMapPaletteSubRecord>("WNAM")?.Palette;
            TextureMap = record.TryGetSubRecord<LandTextureSubRecord>("VTEX")?.Data;
            ColorMapping = record.TryGetSubRecord<VertexColorSubRecord>("VCLR")?.ColorMapping;
        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "INTV");
            validator.CheckRequired(record, "DATA");
        }

        public override TES3GameItem Copy()
        {
            var result = new Landscape(Grid)
            {
                CSGenerated = CSGenerated,
                BaseHeight = BaseHeight,
                TextureMap = TextureMap.Copy()
            };

            var normalVectorMapping = new TES3VertexNormal[VertexNormalSubRecord.MAPPING_SIDE_LENGTH, VertexNormalSubRecord.MAPPING_SIDE_LENGTH];
            CollectionUtils.CopySquare(NormalVectorMapping, normalVectorMapping, VertexNormalSubRecord.MAPPING_SIDE_LENGTH);
            result.NormalVectorMapping = normalVectorMapping;
            
            var slopeMapping = new sbyte[HeightMapSubRecord.MAPPING_SIDE_LENGTH, HeightMapSubRecord.MAPPING_SIDE_LENGTH];
            CollectionUtils.CopySquare(SlopeMapping, slopeMapping, HeightMapSubRecord.MAPPING_SIDE_LENGTH);
            result.SlopeMapping = slopeMapping;

            var worldMapPalette = new byte[WorldMapPaletteSubRecord.PALETTE_SIDE_LENGTH, WorldMapPaletteSubRecord.PALETTE_SIDE_LENGTH];
            CollectionUtils.CopySquare(WorldMapPalette, worldMapPalette, WorldMapPaletteSubRecord.PALETTE_SIDE_LENGTH);
            result.WorldMapPalette = worldMapPalette;

            var colorMapping = new TES3VertexColor[VertexColorSubRecord.COLOR_MAPPING_SIDE_LENGTH, VertexColorSubRecord.COLOR_MAPPING_SIDE_LENGTH];
            CollectionUtils.CopySquare(ColorMapping, colorMapping, VertexColorSubRecord.COLOR_MAPPING_SIDE_LENGTH);
            result.ColorMapping = colorMapping;

            return result;
        }

        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            
            writer.WriteLine($"CS Generated: {CSGenerated}");

            if (NormalVectorMapping != null)
            {
                writer.WriteLine("Normal Vectors");
                writer.IncIndent();
                PrintUtils.PrintSquare(NormalVectorMapping, writer);
                writer.DecIndent();
            }

            if (SlopeMapping != null)
            {
                writer.WriteLine("Height Map Data");

                writer.IncIndent();
                writer.WriteLine($"Base Height: {BaseHeight}");

                writer.WriteLine("Slope Map");
                writer.IncIndent();
                PrintUtils.PrintSquare(SlopeMapping, writer);
                writer.DecIndent();

                writer.DecIndent();
            }

            if (WorldMapPalette != null)
            {
                writer.WriteLine("World Map Palette");
                writer.IncIndent();
                PrintUtils.PrintSquare(WorldMapPalette, writer);
                writer.DecIndent();
            }

            if (TextureMap != null)
            {
                writer.WriteLine("Texture Map Data");

                writer.IncIndent();
                writer.WriteLine($"Swizzled: {TextureMap.Swizzled}");

                writer.WriteLine("Texture Mapping");
                writer.IncIndent();
                var table = Table.Of(LandTextureMap.TEXTURE_MAPPING_SIDE_LENGTH);
                for (var i = 0; i < LandTextureMap.TEXTURE_MAPPING_SIDE_LENGTH; ++i)
                {
                    var row = new short[LandTextureMap.TEXTURE_MAPPING_SIDE_LENGTH];
                    for (var j = 0; j < LandTextureMap.TEXTURE_MAPPING_SIDE_LENGTH; ++j)
                    {
                        row[j] = TextureMap.GetTextureIndex(i, j);
                    }
                    table.AddRow(row);
                }
                table.Print(writer);
                writer.DecIndent();

                writer.DecIndent();
            }

            if (ColorMapping != null)
            {
                writer.WriteLine("Color Mapping");
                writer.IncIndent();
                PrintUtils.PrintSquare(ColorMapping, writer);
                writer.DecIndent();
            }

            writer.DecIndent();
        }

        public override string ToString()
        {
            return $"Landscape ({Grid.GridX}, {Grid.GridY})";
        }

        void CheckState()
        {
            if (HasHeightMapData && (NormalVectorMapping == null || SlopeMapping == null || WorldMapPalette == null))
            {
                throw new TES3ValidationException("If any of NormalVectorMapping, SlopeMapping, or WorldMapPalette are set, they must all be set.");
            }
        }

    }
}
