namespace Backend.Persistence
{
	public class PlayerStats : IPersistable
	{
		// This class storages the persistant data of the player.
		
		public int GenericQuestsCompleted { get; set; }
		public int World1QuestsCompleted { get; set; }
		public int World2QuestsCompleted { get; set; }
		public int World3QuestsCompleted { get; set; }

		public int ModeRunHighScore { get; private set; }
		public int ModeEndlessHighScore { get; private set; }


		/// <summary>
		/// Checks if the new score surpasses the stored one. If so, updates it.
		/// </summary>
		/// <param name="score">The new final score.</param>
		public void SetRunHighScore(int score)
		{
			ModeRunHighScore = score > ModeRunHighScore ? score : ModeRunHighScore;
		}
		
		/// <summary>
		/// Checks if the new score surpasses the stored one. If so, updates it.
		/// </summary>
		/// <param name="score">The new final score.</param>
		public void SetEndlessHighScore(int score)
		{
			ModeEndlessHighScore = score > ModeEndlessHighScore ? score : ModeEndlessHighScore;
		}
		
		
		// -------------------------------------------------------------------------------------------------------------
		
		public void Save(GameDataWriter writer)
		{
			writer.Write(GenericQuestsCompleted);
			writer.Write(World1QuestsCompleted);
			writer.Write(World2QuestsCompleted);
			writer.Write(World3QuestsCompleted);
			
			writer.Write(ModeRunHighScore);
			writer.Write(ModeEndlessHighScore);
		}

		public void Load(GameDataReader reader)
		{
			GenericQuestsCompleted = reader.ReadInt();
			World1QuestsCompleted = reader.ReadInt();
			World2QuestsCompleted = reader.ReadInt();
			World3QuestsCompleted = reader.ReadInt();

			ModeRunHighScore = reader.ReadInt();
			ModeEndlessHighScore = reader.ReadInt();
		}
	}
}