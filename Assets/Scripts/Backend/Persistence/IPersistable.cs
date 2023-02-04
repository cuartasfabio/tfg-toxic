namespace Backend.Persistence
{
	public interface IPersistable
	{
		/// <summary>
		/// Saves the IPersistable data. Called only by PersistentStorage.
		/// </summary>
		/// <param name="writer"></param>
		public void Save(GameDataWriter writer);

		/// <summary>
		/// Loads the IPersistable data. Called only by PersistentStorage.
		/// </summary>
		/// <param name="reader"></param>
		public void Load(GameDataReader reader);
	}
}