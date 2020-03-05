
using System;

using TES3.Util;

namespace TES3.Core
{
    public class LandTextureMap : ICopyable<LandTextureMap>
    {
        public const int TEXTURE_MAPPING_SIDE_LENGTH = 16;
        public const int LOGICAL_NONE_TEX_INDEX = -1;

        readonly short[,] textureIndexMapping;
        readonly Swizzler swizzler;

        public LandTextureMap(short[,] textureIndexMapping, bool swizzled)
        {
            this.textureIndexMapping = Validation.ValidateSquare(textureIndexMapping, TEXTURE_MAPPING_SIDE_LENGTH, "textureIndexMapping", "Texture Index Mapping");
            Swizzled = swizzled;
            swizzler = new Swizzler(textureIndexMapping);
        }

        public short this[int x, int y]
        {
            get => textureIndexMapping[x, y];
            set => textureIndexMapping[x, y] = value;
        }

        public bool Swizzled
        {
            get;
            private set;
        }

        public short GetTextureIndex(int x, int y)
        {
            return (short) (textureIndexMapping[x, y] - 1);
        }

        public void SetTextureIndex(int x, int y, short texIndex)
        {
            if (texIndex < LOGICAL_NONE_TEX_INDEX)
            {
                throw new ArgumentOutOfRangeException("texIndex", texIndex, $"Texture Index cannot be less than {LOGICAL_NONE_TEX_INDEX}: {texIndex}");
            }
            textureIndexMapping[x, y] = (short) (texIndex + 1);
        }

        public void UnSwizzle()
        {
            if (!Swizzled)
            {
                return;
            }

            swizzler.Unswizzle();
            Swizzled = false;
        }

        public void Swizzle()
        {
            if (Swizzled)
            {
                return;
            }

            swizzler.Swizzle();
            Swizzled = true;
        }

        public LandTextureMap ProcessCopy()
        {
            return this;
        }

        public LandTextureMap Copy()
        {
            var textureIndexMapping = new short[TEXTURE_MAPPING_SIDE_LENGTH, TEXTURE_MAPPING_SIDE_LENGTH];
            for (var i = 0; i < TEXTURE_MAPPING_SIDE_LENGTH; ++i)
            {
                for (var j = 0; j < TEXTURE_MAPPING_SIDE_LENGTH; ++j)
                {
                    textureIndexMapping[i, j] = this.textureIndexMapping[i, j];
                }
            }

            return new LandTextureMap(textureIndexMapping, Swizzled);
        }


        class Swizzler
        {
            readonly short[,] data;
            readonly short[] buf = new short[8];

            internal Swizzler(short[,] data)
            {
                this.data = data;
            }

            internal void Unswizzle()
            {
                Process(false, 0, 0, 15, 0, 3, 12, 12, 12);
                Process(false, 1, 0, 15, 4, 2, 12, 12, 8);
                Process(false, 4, 0, 11, 0, 7, 12, 8, 12);
                Process(false, 5, 0, 11, 4, 6, 12, 8, 8);
            }

            internal void Swizzle()
            {
                Process(true, 0, 0, 15, 0, 3, 12, 12, 12);
                Process(true, 1, 0, 15, 4, 2, 12, 12, 8);
                Process(true, 4, 0, 11, 0, 7, 12, 8, 12);
                Process(true, 5, 0, 11, 4, 6, 12, 8, 8);
            }

            void Process(bool swizzle, int r1, int c1, int r2, int c2, int r3, int c3, int r4, int c4)
            {
                for (var i = 0; i < 4; ++i)
                {
                    if (swizzle)
                    {
                        Buf(r4, c4, 0);
                        Copy(r1, c1, r4, c4);

                        Buf(r3, c3, 4);
                        BufCopy(0, r3, c3);

                        Buf(r2, c2, 0);
                        BufCopy(4, r2, c2);

                        BufCopy(0, r1, c1);

                        c1 += 4;
                        r2 -= 1;
                        c3 -= 4;
                        r4 += 1;
                    }
                    else
                    {
                        Buf(r2, c2, 0);
                        Copy(r1, c1, r2, c2);

                        Buf(r3, c3, 4);
                        BufCopy(0, r3, c3);

                        Buf(r4, c4, 0);
                        BufCopy(4, r4, c4);

                        BufCopy(0, r1, c1);

                        c1 += 4;
                        r2 -= 1;
                        c3 -= 4;
                        r4 += 1;
                    }
                }
            }

            void Buf(int row, int col, int bufTarget)
            {
                for (var i = 0; i < 4; ++i)
                {
                    buf[bufTarget++] = data[row, col++];
                }
            }

            void Copy(int srcRow, int srcCol, int targetRow, int targetCol)
            {
                for (var i = 0; i < 4; ++i)
                {
                    data[targetRow, targetCol++] = data[srcRow, srcCol++];
                }
            }

            void BufCopy(int bufSrc, int targetRow, int targetCol)
            {
                for (var i = 0; i < 4; ++i)
                {
                    data[targetRow, targetCol++] = buf[bufSrc++];
                }
            }

        }
    }
}
