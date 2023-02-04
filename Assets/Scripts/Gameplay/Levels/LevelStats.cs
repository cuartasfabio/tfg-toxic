using Backend.Persistence;

namespace Gameplay.Levels
{
	public class LevelStats : IPersistable
	{
		public int ExploredTiles { get; set; }
		public int LastTurnScore { get; set; }
		public int CampsitesPlaced { get; set; } // todo nonsense to have this here.. 
		
		// -- todo only to test level specific quests
		public int FarmsPlaced { get; set; }
		public int MeadowsPlaced { get; set; }
		public int DunesPlaced { get; set; }
		public int ShardMinesExplored { get; set; }
		//
		
		public int NaturalTilesPlaced { get; private set; }
		public const int TilesToMonolith = 15;
		public int ForestsPlaced { get; private set; }
		public const int ForestsToCabin = 10;

		

		public void IncrementNaturalTiles()
		{
			NaturalTilesPlaced++;
			if (NaturalTilesPlaced >= TilesToMonolith)
				NaturalTilesPlaced = 0;
		}

		public bool WillReachMonolith(int tilesToPlace)
		{
			return NaturalTilesPlaced + tilesToPlace >= TilesToMonolith;
		}
		
		public void IncrementForestsPlaced()
		{
			ForestsPlaced++;
			if (ForestsPlaced >= ForestsToCabin)
				ForestsPlaced = 0;
		}

		public bool WillReachCabin(int tilesToPlace)
		{
			return ForestsPlaced + tilesToPlace >= ForestsToCabin;
		}
		
		public void Save(GameDataWriter writer)
		{
			writer.Write(ExploredTiles);
			writer.Write(LastTurnScore);
			writer.Write(CampsitesPlaced);
			writer.Write(NaturalTilesPlaced);
			writer.Write(ForestsPlaced);
			
			//
			writer.Write(FarmsPlaced);
			writer.Write(MeadowsPlaced);
			writer.Write(DunesPlaced);
			writer.Write(CampsitesPlaced);
			//
		}

		public void Load(GameDataReader reader)
		{
			ExploredTiles = reader.ReadInt();
			LastTurnScore = reader.ReadInt();
			CampsitesPlaced = reader.ReadInt();
			NaturalTilesPlaced = reader.ReadInt();
			ForestsPlaced = reader.ReadInt();
			
			//
			FarmsPlaced = reader.ReadInt();
			MeadowsPlaced = reader.ReadInt();
			DunesPlaced = reader.ReadInt();
			CampsitesPlaced = reader.ReadInt();
			//
		}
	}
}