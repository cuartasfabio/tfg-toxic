using System.IO;
using Gameplay.Grids.Hexes.HexHelpers;
using UnityEngine;

namespace Backend.Persistence
{
	public class GameDataReader
	{
		private BinaryReader _reader;

		public GameDataReader(BinaryReader reader)
		{
			_reader = reader;
		}

		public float ReadFloat()
		{
			return _reader.ReadSingle();
		}

		public int ReadInt()
		{
			return _reader.ReadInt32();
		}
		
		public bool ReadBool()
		{
			return _reader.ReadBoolean();
		}

		public Vector3 ReadVector3()
		{
			Vector3 value;
			value.x = _reader.ReadSingle();
			value.y = _reader.ReadSingle();
			value.z = _reader.ReadSingle();
			return value;
		}
		
		public Quaternion ReadQuaternion()
		{
			Quaternion value;
			value.x = _reader.ReadSingle();
			value.y = _reader.ReadSingle();
			value.z = _reader.ReadSingle();
			value.w = _reader.ReadSingle();
			return value;
		}

		public HexCoordinates ReadHexCoords()
		{
			int x = _reader.ReadInt32();
			int z = _reader.ReadInt32();
			return new HexCoordinates(x, z);
		}
		
	}
}