using System;
using System.Collections.Generic;
using System.IO;

using TES3.Core;
using TES3.Records;
using TES3.GameItem.Part;
using TES3.Records.SubRecords;
using TES3.Util;

namespace TES3.GameItem.Item
{

    [TargetRecord("PGRD")]
    public class PathGrid : TES3GameItem
    {

        short granularity;

        public PathGrid(PathGridKey cellKey) : base(cellKey)
        {
            Granularity = 1024;
        }

        public PathGrid(Record record) : base(record)
        {
            
        }

        public override string RecordName => "PGRD";

        [IdField]
        public PathGridKey CellKey
        {
            get => (PathGridKey) Id;
            set => Id = value;
        }


        public short Granularity
        {
            get => granularity;
            set
            {
                switch (value)
                {
                    case 128:
                    case 256:
                    case 512:
                    case 1024:
                    case 2048:
                    case 4096:
                        granularity = value;
                        return;
                    default:
                        // TODO Warn
                        // Console.Error.WriteLine($"Bad path grid for {CellKey}: {value}; defaulting to 1024.");
                        granularity = 1024;
                        return;
                }
            }
        }

        public IList<PathGridNode> Nodes
        {
            get;
        } = new List<PathGridNode>();

        protected override void OnCreate(IList<SubRecord> subRecords)
        {
            subRecords.Add(new PathGridDataSubRecord("DATA", CellKey.Grid.GridX, CellKey.Grid.GridY, Granularity, (short)Nodes.Count)); ;
            subRecords.Add(new StringSubRecord("NAME", CellKey.Name));
        }

        protected override void UpdateRequired(Record record)
        {
            record.GetSubRecord<StringSubRecord>("NAME").Data = CellKey.Name;

            var dataSubRecord = record.GetSubRecord<PathGridDataSubRecord>("DATA");
            dataSubRecord.GridX = CellKey.Grid.GridX;
            dataSubRecord.GridY = CellKey.Grid.GridY;
            dataSubRecord.Granularity = Granularity;
            dataSubRecord.PointCount = (short) Nodes.Count;

        }

        protected override void UpdateOptional(Record record)
        {
            var points = new List<RawPathGridPoint>(Nodes.Count);
            var connectors = new List<int>();
            foreach(var node in Nodes)
            {
                foreach (var point in node.ConnectedPoints)
                {
                    var index = IndexOf(point);
                    if (index == -1)
                    {
                        throw new InvalidOperationException("Invalid connected point.");
                    }

                    connectors.Add(index);
                }

                var current = node.Point;
                points.Add(new RawPathGridPoint(current.X, current.Y, current.Z, current.Generated, (byte) node.ConnectedPoints.Count, 0));
            }
            
            if (record.ContainsSubRecord("PGRP"))
            {
                record.GetSubRecord<PathGridPointSubRecord>("PGRP").Points = points;
            }
            else if (points.Count > 0)
            {
                record.InsertSubRecordAt(record.GetAddIndex("PGRC"), new PathGridPointSubRecord("PGRP", points));
            }
            
            
            if (record.ContainsSubRecord("PGRC"))
            {
                record.GetSubRecord<PathGridConnectionSubRecord>("PGRC").Connections = connectors;
            }
            else if (connectors.Count > 0)
            {
                record.InsertSubRecordAt(record.GetAddIndex("PGRC"), new PathGridConnectionSubRecord("PGRC", connectors));
            }
            
            
        }

        protected override void DoSyncWithRecord(Record record)
        {
            // Required
            var dataSubRecord = record.GetSubRecord<PathGridDataSubRecord>("DATA");
            Id = new PathGridKey(record.GetSubRecord<StringSubRecord>("NAME").Data, new GridKey(dataSubRecord.GridX, dataSubRecord.GridY));
            Granularity = dataSubRecord.Granularity;


            // Complex
            // Points/Connectors
            Nodes.Clear();
            if (record.ContainsSubRecord("PGRP"))
            {
                var rawPoints = record.GetSubRecord<PathGridPointSubRecord>("PGRP").Points;
                var points = new List<PathGridPoint>(rawPoints.Count);
                foreach (var point in rawPoints)
                {
                    points.Add(new PathGridPoint(point.X, point.Y, point.Z, point.Generated));
                }
                if (record.ContainsSubRecord("PGRC"))
                {
                    var connectedInicies = record.GetSubRecord<PathGridConnectionSubRecord>("PGRC").Connections;
                    var connectorPos = 0;
                    for (var i = 0; i < rawPoints.Count; ++i)
                    {
                        var rawPoint = rawPoints[i];

                        var connected = new List<PathGridPoint>();
                        for (var j = 0; j < rawPoint.ConnectionCount; ++j)
                        {
                            connected.Add(points[connectedInicies[connectorPos++]]);
                        }

                        Nodes.Add(new PathGridNode(points[i], connected));
                    }
                }
            }

        }

        protected override void DoValidateRecord(Record record, Validator validator)
        {
            validator.CheckRequired(record, "DATA");
            validator.CheckRequired(record, "NAME");
        }


        public override void StreamDebug(TextWriter target)
        {
            var writer = new IndentWriter(target);

            writer.WriteLine(ToString());

            writer.IncIndent();
            writer.WriteLine($"Granularity: {Granularity}");

            writer.WriteLine("Points");
            writer.IncIndent();
            var index = -1;
            foreach (var node in Nodes)
            {
                writer.WriteLine($"{++index}: {node.Point}");
                if (node.ConnectedPoints.Count != 0)
                {
                    writer.IncIndent();
                    writer.WriteLine(string.Join(", ", node.ConnectedPoints));
                    writer.DecIndent();
                }
            }
            writer.DecIndent();

            writer.DecIndent();
        }

        public override TES3GameItem Copy()
        {
            var result = new PathGrid(CellKey)
            {
                Granularity = Granularity
            };

            CollectionUtils.Copy(Nodes, result.Nodes);
            return result;
        }

        public override string ToString()
        {
            return $"Path Grid ({CellKey.Name} ({CellKey.Grid.GridX}, {CellKey.Grid.GridY}))";
        }

        int IndexOf(PathGridPoint point)
        {
            int current = -1;
            foreach (var node in Nodes)
            {
                ++current;
                if (ReferenceEquals(point, node.Point))
                {
                    return current;
                }
            }
            return -1;
        }
    }
}
