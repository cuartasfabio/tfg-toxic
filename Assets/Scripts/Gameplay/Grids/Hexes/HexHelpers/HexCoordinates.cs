using UnityEngine;

namespace Gameplay.Grids.Hexes.HexHelpers
{
    [System.Serializable]
    public struct HexCoordinates
    {
        [SerializeField] private int x;
        [SerializeField] private int z;
        
        public HexCoordinates(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public static HexCoordinates FromOffsetCoordinates(int x, int z)
        {
            return new HexCoordinates(x - z/2, z);
        }

        public int X
        {
            get
            {
                return x;
            }
        }
        public int Z
        {
            get
            {
                return z;
            }
        }
        public int Y
        {
            get
            {
                return -X - Z;
            }
        }

        public override string ToString()
        {
            return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
        }

        public string ToStringOnSeparateLines()
        {
            return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
        }
        
        
        /* Transforma HexCoordinates en coordenadas de world space */
        public static Vector3 ToPosition(HexCoordinates coords)
        {
            Vector3 position;
            position.x = HexMetrics.OuterRadius * (Mathf.Sqrt(3) * coords.X + (Mathf.Sqrt(3)/2) * coords.Z);
            position.y = 0f;
            position.z = HexMetrics.OuterRadius * ((3f / 2) * coords.Z);
            return position;
        }
        
        /* Tramsforma coordenadas world space en HexCoordinates */
        public static HexCoordinates ToCoordinates(Vector3 position)
        {
            float x = position.x / (HexMetrics.InnerRadius * 2f);
            float y = -x;
            
            float offset = position.z / (HexMetrics.OuterRadius * 3f);
            x -= offset;
            y -= offset;

            int iX = Mathf.RoundToInt(x);
            int iY = Mathf.RoundToInt(y);
            int iZ = Mathf.RoundToInt(-x -y);

            if (iX + iY + iZ != 0) {
                float dX = Mathf.Abs(x - iX);
                float dY = Mathf.Abs(y - iY);
                float dZ = Mathf.Abs(-x -y - iZ);

                if (dX > dY && dX > dZ) {
                    iX = -iY - iZ;
                }
                else if (dZ > dY) {
                    iZ = -iX - iY;
                }
            }
            return new HexCoordinates(iX, iZ);
        }

    }
}