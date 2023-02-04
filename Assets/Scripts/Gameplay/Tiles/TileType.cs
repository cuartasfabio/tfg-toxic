using Gameplay.Grids.Hexes.HexHelpers;

namespace Gameplay.Tiles
{
    public enum TileType : ushort
    {
        None = 0,
        
        Ground, 
        Ruins, 
        Crystals, 
        Lake, 
        Forest, 
        Meadow,
        Herbivores,
        Carnivores,
        Campsite,
        Explorer,
        Wastes,
        Frontier,
        DeepForest,
        Village,
        City,
        Mountain,
        Monolith,
        Mutants,
        Purifier,
        RadioTower,
        Factory,
        Farmland,
        Farm,
        Lumberjack,
        OldCabin,
        Swamp,
        WarpedWoods,
        Dunes,
        Fossils,
        Oasis,
        ShardMine,
        StrandedShip,
        
        Any
    }
    
    public static class TileTypes
    {
        public static int TileTypeCount => (int) TileType.Any - 1;

        /// <summary>
        /// Every index of this List is a TileType. The list inside each index contains the TileTypes where it can be placed.
        /// </summary>
        public static readonly TileType[][] PlacingRules =
        {
            new TileType[] {}, // NONE
            new TileType[] {}, // GROUND
            new TileType[] {}, // RUINS
            new TileType[] {}, // CRYSTALS
            new[] {TileType.Ground}, // LAKE
            new[] {TileType.Ground, TileType.Forest}, // FOREST
            new[] {TileType.Ground}, // MEADOW
            new[] {TileType.Ground}, // HERBIVORES
            new[] {TileType.Ground}, // CARNIVORES
            new[] {TileType.Ground, TileType.Forest, TileType.Mountain, TileType.Herbivores, TileType.Carnivores, TileType.DeepForest}, // CAMPSITE
            new[] {TileType.Frontier, TileType.Ruins}, // EXPLORER
            new[] {TileType.Ground, TileType.Crystals, TileType.Lake, TileType.Forest, TileType.Herbivores, TileType.Carnivores, TileType.Mountain, TileType.DeepForest}, // WASTES
            new TileType[] {}, // FRONTIER
            new TileType[] {}, // DEEPFOREST
            new TileType[] {}, // VILLAGE
            new TileType[] {}, // CITY
            new[] {TileType.Ground}, // MOUNTAIN
            new TileType[] {}, // MONOLITH
            new TileType[] {}, // MUTANTS
            new[] {TileType.Ground}, // PURIFIER
            new[] {TileType.Ground}, // RADIOTOWER
            new TileType[] {}, // FACTORY
            new TileType[] {}, // FARMLAND
            new[] {TileType.Ground}, // FARM
            new[] {TileType.Ground}, // LUMBERJACK
            new TileType[] {}, // OLDCABIN
            new[] {TileType.Ground}, // SWAMP
            new TileType[] {}, // WARPED WOODS
            new[] {TileType.Ground}, // DUNES
            new TileType[] {}, // FOSSILS
            new TileType[] {}, // OASIS
            new TileType[] {}, // SHARDMINE
            new TileType[] {} // STRANDEDSHIP
        };

        public static bool CanBePlaced(TileType toPlace, HexCoordinates coords)
        {
            int typeBeh = (int)toPlace;
            
            TileType typeCell = ObjectCache.Current.HexGrid.Lists.GetTypeForCoord(coords);
            
            for (int i = 0; i < PlacingRules[typeBeh].Length; i++)
            {
                if (PlacingRules[typeBeh][i] == typeCell)
                    return true;
            }

            return false;
        }
        
        public static readonly TileType[] EatableTypes =
        {
            TileType.Herbivores, TileType.Carnivores, TileType.Campsite, TileType.Village, TileType.Farm, TileType.Lumberjack
        };
    }
}