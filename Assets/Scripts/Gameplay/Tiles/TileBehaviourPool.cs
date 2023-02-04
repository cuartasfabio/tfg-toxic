using System;
using System.Collections.Generic;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.TileCreations;
using Gameplay.TileCreations.TileScriptObjs;
using Gameplay.Tiles.TileBehaviours;
using Gameplay.Tiles.TileEntities;
using UnityEngine;

namespace Gameplay.Tiles
{
    /// <summary>
    /// Pool class to instantiate TileBehaviours.
    /// </summary>
    public class TileBehaviourPool : MonoBehaviour
    {
        [Serializable]
        public class TileConfig
        {
            [SerializeField] public TileBehaviour Behaviour;
            [SerializeField] public TileData data;
        }
        
        [SerializeField] private TileConfig _ground;
        [SerializeField] private TileConfig _ruins;
        [SerializeField] private TileConfig _crystals;
        [SerializeField] private TileConfig _lake;
        [SerializeField] private TileConfig _forest;
        [SerializeField] private TileConfig _meadow;
        [SerializeField] private TileConfig _herbivores;
        [SerializeField] private TileConfig _carnivores;
        [SerializeField] private TileConfig _campsite;
        [SerializeField] private TileConfig _explorer;
        [SerializeField] private TileConfig _wastes;
        [SerializeField] private TileConfig _frontier;
        [SerializeField] private TileConfig _deepForest;
        [SerializeField] private TileConfig _village;
        [SerializeField] private TileConfig _city;
        [SerializeField] private TileConfig _mountain;
        [SerializeField] private TileConfig _monolith;
        [SerializeField] private TileConfig _mutants;
        [SerializeField] private TileConfig _purifier;
        [SerializeField] private TileConfig _radioTower;
        [SerializeField] private TileConfig _factory;
        [SerializeField] private TileConfig _farmland;
        [SerializeField] private TileConfig _farm;
        [SerializeField] private TileConfig _lumberjack;
        [SerializeField] private TileConfig _oldCabin;
        [SerializeField] private TileConfig _swamp;
        [SerializeField] private TileConfig _warpedWoods;
        [SerializeField] private TileConfig _dunes;
        [SerializeField] private TileConfig _fossils;
        [SerializeField] private TileConfig _oasis;
        [SerializeField] private TileConfig _shardMine;
        [SerializeField] private TileConfig _strandedShip;
        
        private Dictionary<TileType,TileConfig> _tileConfigs;
        private Dictionary<TileType, Func<ITileEntity>> _tileEntities;

        // [Space] [SerializeField] private Material _matPropPreview;
        // public Material MatPropPreview => _matPropPreview;

        private void Awake()
        {
            _tileConfigs = new Dictionary<TileType, TileConfig>()
            {
                {TileType.Ground, _ground},
                {TileType.Ruins, _ruins},
                {TileType.Crystals, _crystals},
                {TileType.Lake, _lake},
                {TileType.Forest, _forest},
                {TileType.Meadow, _meadow},
                {TileType.Herbivores, _herbivores},
                {TileType.Carnivores, _carnivores},
                {TileType.Campsite, _campsite},
                {TileType.Explorer, _explorer},
                {TileType.Wastes, _wastes},
                {TileType.Frontier, _frontier},
                {TileType.DeepForest, _deepForest},
                {TileType.Village, _village},
                {TileType.City, _city},
                {TileType.Mountain, _mountain},
                {TileType.Monolith, _monolith},
                {TileType.Mutants, _mutants},
                {TileType.Purifier, _purifier},
                {TileType.RadioTower, _radioTower},
                {TileType.Factory, _factory},
                {TileType.Farmland, _farmland},
                {TileType.Farm, _farm},
                {TileType.Lumberjack, _lumberjack},
                {TileType.OldCabin, _oldCabin},
                {TileType.Swamp, _swamp},
                {TileType.WarpedWoods, _warpedWoods},
                {TileType.Dunes, _dunes},
                {TileType.Fossils, _fossils},
                {TileType.Oasis, _oasis},
                {TileType.ShardMine, _shardMine},
                {TileType.StrandedShip, _strandedShip}
            };
            
            _tileEntities = new Dictionary<TileType, Func<ITileEntity>>()
            {
                {TileType.Ground, () => new GroundEntity()},
                {TileType.Ruins, () => new RuinsEntity()},
                {TileType.Crystals, () => new CrystalsEntity()},
                {TileType.Lake, () => new LakeEntity()},
                {TileType.Forest, () => new ForestEntity()},
                {TileType.Meadow, () => new MeadowEntity()},
                {TileType.Herbivores, () => new HerbivoresEntity()},
                {TileType.Carnivores, () => new CarnivoresEntity()},
                {TileType.Campsite, () => new CampsiteEntity()},
                {TileType.Explorer, () => new ExplorerEntity()},
                {TileType.Wastes, () => new WastesEntity()},
                {TileType.Frontier, () => new FrontierEntity()},
                {TileType.DeepForest, () => new DeepForestEntity()}, 
                {TileType.Village, () => new VillageEntity()}, 
                {TileType.City, () => new CityEntity()},
                {TileType.Mountain, () => new MountainEntity()},
                {TileType.Monolith, () => new MonolithEntity()},
                {TileType.Mutants, () => new MutantsEntity()},
                {TileType.Purifier, () => new PurifierEntity()},
                {TileType.RadioTower, () => new RadioTowerEntity()},
                {TileType.Factory, () => new FactoryEntity()},
                {TileType.Farmland, () => new FarmlandEntity()},
                {TileType.Farm, () => new FarmEntity()},
                {TileType.Lumberjack, () => new LumberjackEntity()},
                {TileType.OldCabin, () => new OldCabinEntity()},
                {TileType.Swamp, () => new SwampEntity()},
                {TileType.WarpedWoods, () => new WarpedWoodsEntity()},
                {TileType.Dunes, () => new DunesEntity()},
                {TileType.Fossils, () => new FossilsEntity()},
                {TileType.Oasis, () => new OasisEntity()},
                {TileType.ShardMine, () => new ShardMineEntity()},
                {TileType.StrandedShip, () => new StrandedShipEntity()}
            };
            
        }
    
        private TileBehaviour InitTile(TileType tileID, TileBehaviour tilePrefab, Tile tile)
        {
            ITileEntity entity = _tileEntities[tileID].Invoke();

            TileBehaviour behaviour = tilePrefab.GetComponent<TileBehaviour>();
            behaviour.Init(entity, tile);

            return behaviour;
        }
    
        public TileBehaviour GetNewTile(TileType tileID)
        {
            TileBehaviour tileObj = Instantiate(_tileConfigs[tileID].Behaviour, Vector3.zero, Quaternion.identity);

            Tile tile = tileObj.gameObject.GetComponentInChildren<Tile>();
            tile.Init(_tileConfigs[tileID].data);
        
            return InitTile(tileID, tileObj, tile);
        }
    
        public TileBehaviour GetNewTile(TileType tileID, Vector3 newPos)
        {
            TileBehaviour tileObj = Instantiate(_tileConfigs[tileID].Behaviour, newPos, Quaternion.identity);
            
            Tile tile = tileObj.gameObject.GetComponentInChildren<Tile>();
            tile.Init(_tileConfigs[tileID].data);
        
            return InitTile(tileID, tileObj, tile);
        }
        
        public TileBehaviour GetNewTile(TileType tileID, HexCoordinates newCoords)
        {
            Vector3 newPos = HexCoordinates.ToPosition(newCoords);
            
            TileBehaviour tileObj = Instantiate(_tileConfigs[tileID].Behaviour, newPos, Quaternion.identity);
            
            Tile tile = tileObj.gameObject.GetComponentInChildren<Tile>();
            tile.Init(_tileConfigs[tileID].data);
        
            return InitTile(tileID, tileObj, tile);
        }
        
    }
}
