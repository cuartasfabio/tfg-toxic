using System.IO;
using UnityEngine;

namespace Backend.Persistence
{
	public class PersistentStorage
	{
		public const int SaveVersion = 1;
		public int LastReadVersion { get; private set; }
		
		private readonly string[] _saveFilePaths;

		public enum SaveType
		{
			RUN,
			SETTINGS,
			PLAYERSTATS,
			LEVEL
		}

		public PersistentStorage()
		{
			_saveFilePaths = new[]
			{
				Path.Combine(Application.persistentDataPath, "tf-runsave"),
				Path.Combine(Application.persistentDataPath, "tf-settings"),
				Path.Combine(Application.persistentDataPath, "tf-stats"),
				Path.Combine(Application.persistentDataPath, "tf-levelsave")
			};
		}
		
		public void Save(IPersistable persistable, SaveType saveType)
		{
			using (BinaryWriter writer = new BinaryWriter(File.Open(_saveFilePaths[(int)saveType], FileMode.Create)))
			{
				writer.Write(SaveVersion);
				persistable.Save(new GameDataWriter(writer));
			}
		}
		
		public void Load(IPersistable persistable, SaveType saveType)
		{
			try
			{
				using (BinaryReader reader = new BinaryReader(File.Open(_saveFilePaths[(int)saveType], FileMode.Open)))
				{
					
					var gdr = new GameDataReader(reader);
					LastReadVersion = gdr.ReadInt();
					persistable.Load(gdr);
				}
				
			}
			catch (FileNotFoundException e)
			{
				Debug.LogWarning(e.Message);
				Save(persistable, saveType);
			}
		}

		public bool Exists(SaveType st)
		{
			return File.Exists(_saveFilePaths[(int) st]);
		}

		public void DeleteSavedRun()
		{
			if (Exists(SaveType.LEVEL))
				File.Delete(_saveFilePaths[(int) SaveType.LEVEL]);
			if (Exists(SaveType.RUN))
				File.Delete(_saveFilePaths[(int) SaveType.RUN]);
		}
		
	}
}