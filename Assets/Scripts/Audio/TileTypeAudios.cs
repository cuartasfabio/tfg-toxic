using System.Collections.Generic;
using Gameplay.Tiles;

namespace Audio
{
	public static class TileTypeAudios
	{
		/// <summary>
		/// Relates every TileType with it's SFX.
		/// </summary>
		private static Dictionary<TileType, AudioId> _audioForType = new Dictionary<TileType, AudioId>()
		{
			{TileType.Campsite,AudioId.SFX_TileCampsite},
			{TileType.Carnivores,AudioId.SFX_TileCarnivores},
			{TileType.Explorer,AudioId.SFX_TileExplorer},
			{TileType.Forest,AudioId.SFX_TileForest},
			{TileType.Herbivores,AudioId.SFX_TileHerbivores},
			{TileType.Lake,AudioId.SFX_TileLake},
			{TileType.Wastes,AudioId.SFX_Click1}, // todo meter sfx
			{TileType.Mountain,AudioId.SFX_Click1}, // todo meter sfx
			{TileType.Purifier,AudioId.SFX_Click1}, // todo meter sfx
			{TileType.RadioTower,AudioId.SFX_Click1}, // todo meter sfx
			{TileType.Farm,AudioId.SFX_Click1}, // todo meter sfx
			{TileType.Meadow,AudioId.SFX_Click1}, // todo meter sfx
			{TileType.Swamp,AudioId.SFX_Click1}, // todo meter sfx
			{TileType.Lumberjack,AudioId.SFX_Click1}, // todo meter sfx
			{TileType.Dunes,AudioId.SFX_Click1} // todo meter sfx
		};

		/// <summary>
		/// Returns the AudioId for a given TileType.
		/// </summary>
		/// <param name="type"></param>
		/// <returns>The AudioId.</returns>
		public static AudioId GetAudioType(TileType type)
		{
			return _audioForType[type];
		}
	}
}