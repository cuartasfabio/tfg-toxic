using System.Collections.Generic;
using Backend.Persistence;
using Gameplay.Commands.TileBehaviourCommands;
using Gameplay.Commands.TileBehaviourCommands.Formations;
using Gameplay.Grids.Hexes;
using Gameplay.Grids.Hexes.HexHelpers;
using Gameplay.Levels;
using Gameplay.Tiles;
using Gameplay.Tiles.TileBehaviours;
using UI.InGames.TileBorders;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Gameplay.Visitors.Tiles
{
    public class OnPlaceVisitor: AbstractTileVisitor
    {
        
        private UpdateScoreActionCommand _onPlaceScoreActionCommand;
        
        public new List<ITileActionCommand> GetBehaviourCommands(TileBehaviour tile)
        {
            _commands = new List<ITileActionCommand>();
            tile.Accept(this);
            // _commands.Add(_onPlaceScoreCommand);
            return _commands;
        }

        public UpdateScoreActionCommand GetScoreCommand()
        {
            return _onPlaceScoreActionCommand;
        }
        
        private void DiscoverNeighbors(HexCoordinates behaviour)
        {
            List<HexCoordinates> freeNeighbors = HexFunctions.GetNullNeighborsCoordinates(behaviour);
            for (int i = 0; i < freeNeighbors.Count; i++)
                if (Random.Range(0f, 1f) < ObjectCache.Current.GridSizeManager.GridDiscoveryRate.GetRate())
                    _commands.Add(new DiscoverTileActionCommand(freeNeighbors[i], false, 0.15f));
        }

        private void CheckForNearCrystals(UpdateScoreActionCommand scoreActionCommand, HexCoordinates behaviourCoords)
        {
            // if (scoreCommand.TotalAmount == 0)
            //     scoreCommand.AddPartialScore(10, behaviourCoords);
            
            List<HexCoordinates> crystals = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Crystals}, 1);
            if (crystals.Count > 0)
            {
                scoreActionCommand.AddPartialScore(-scoreActionCommand.TotalAmount * 2, crystals[0]);
            }
        }

        private bool CheckPlacingMatrix(TileBehaviour behaviour, HexCoordinates behaviourCoords)
        {
            if (!TileTypes.CanBePlaced(behaviour.TileEntity.GetCardId(), behaviourCoords))
            {
                ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CannotPlace);
                ObjectCache.Current.PlayerTurnManager.PlayerTurnStatePlacing.SetCanBePlaced(false);
                return false;
            }
            ObjectCache.Current.PlayerTurnManager.PlayerTurnStatePlacing.SetCanBePlaced(true);
            return true;
        }

        private void CheckNextMonolith(List<HexCoordinates> natureTilesToPlace)
        {
            LevelStats levelStats = GameManager.Get().RunManager.LevelManager.LevelStats;
            if (levelStats.WillReachMonolith(natureTilesToPlace.Count))
            {
                for (int i = 0; i < 4; i++)
                {
                    HexCoordinates? monolithCoords = ObjectCache.Current.HexGrid.Lists.GetRandomCoordOfType(TileType.Ground);
                    if (monolithCoords.HasValue && !natureTilesToPlace.Contains(monolithCoords.Value))
                    {
                        _commands.Add(new CreateAndPlaceTileActionCommand(monolithCoords.Value, TileType.Monolith, 0.1f));
                        break;
                    }
                }
            }
        }

        private void CheckNextOldCabin(List<HexCoordinates> forestsToPlace)
        {
            LevelStats levelStats = GameManager.Get().RunManager.LevelManager.LevelStats;
            if (levelStats.WillReachCabin(forestsToPlace.Count))
            {
                HexCoordinates? newCabinCoords = ObjectCache.Current.HexGrid.Lists.GetRandomCoordOfTypes(new[] {TileType.Forest, TileType.DeepForest, TileType.WarpedWoods});
                if (newCabinCoords.HasValue)
                {
                    _commands.Add(new CreateAndPlaceTileActionCommand(newCabinCoords.Value, TileType.OldCabin, 0.1f));
                }
                
            }
        }

        // ---------------------------------------------------------------------------------------
        
        
        public override void Visit(LakeBehaviour behaviour)
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);
            
            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;

            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            _onPlaceScoreActionCommand.AddPartialScore(10, behaviourCoords);
            
            List<HexCoordinates> forestsMountains = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Forest, TileType.Mountain}, 1);
            for (int i = 0; i < forestsMountains.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(5, forestsMountains[i]);
            }

            List<HexCoordinates> monoliths = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Monolith}, 3);
            for (int i = 0; i < monoliths.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(5, monoliths[i]);
            }
            
            
            // por cada Lake conectado -> score ++
            (List<HexCoordinates>, int) lakeFormation =
                ObjectCache.Current.FormationsRegister.GetCoordsOfFormations(behaviourCoords);
            for (int i = 0; i < lakeFormation.Item1.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(lakeFormation.Item2, lakeFormation.Item1[i]);
            }

            _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));
            
            _commands.Add(new AddToFormationActionCommand(behaviour, behaviourCoords, TileType.Lake, 0.1f));
            
            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            DiscoverNeighbors(behaviourCoords);
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
            
            CheckNextMonolith(new List<HexCoordinates> { behaviourCoords });
        }

        
        
        
        public override void Visit(ForestBehaviour behaviour)
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);
            
            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
            
            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            _onPlaceScoreActionCommand.AddPartialScore(10, behaviourCoords);
            
            List<HexCoordinates> mountainLakes = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Mountain, TileType.Lake}, 1);
            for (int i = 0; i < mountainLakes.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(5, mountainLakes[i]);
            }
            
            List<HexCoordinates> monoliths = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Monolith}, 3);
            for (int i = 0; i < monoliths.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(5, monoliths[i]);
            }
            
            
            if (ObjectCache.Current.HexGrid.Lists.GetTypeForCoord(behaviourCoords) == TileType.Forest)
            {
                _commands.Add(new CreateAndPlaceTileActionCommand(behaviourCoords, TileType.DeepForest, 0.5f, true));
                _onPlaceScoreActionCommand.AddPartialScore(7, behaviourCoords);
            }
            else
            {
                _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));
                
                List<HexCoordinates> forests = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                    new[] {TileType.Forest}, 1);
                for (int i = 0; i < forests.Count; i++)
                {
                    _onPlaceScoreActionCommand.AddPartialScore(7, forests[i]);
                }
                List<HexCoordinates> deepForests = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                    new[] {TileType.DeepForest}, 1);
                List<HexCoordinates> grounds = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                    new[] {TileType.Ground}, 1);

                int forestsToPlace = deepForests.Count;
                
                List<HexCoordinates> forestPlaced = new List<HexCoordinates>();
                
                for (int i = 0; i < forestsToPlace; i++)
                {
                    _onPlaceScoreActionCommand.AddPartialScore(10, deepForests[i]);
                    
                    if (grounds.Count < 1)
                        break;
                    
                    int chosenIndex = RandomTf.Rng.Next(grounds.Count);
                    _commands.Add(new CreateAndPlaceTileActionCommand(grounds[chosenIndex], TileType.Forest, 0.5f));
                    forestPlaced.Add(grounds[chosenIndex]);
                    grounds.RemoveAt(chosenIndex);
                }
                
                forestPlaced.Add(behaviourCoords);
                CheckNextMonolith(forestPlaced);
                CheckNextOldCabin(forestPlaced);
            }
            

            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            DiscoverNeighbors(behaviourCoords);
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
            
            
        }

        
        
        
        public override void Visit(HerbivoresBehaviour behaviour)
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);
            
            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
            
            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            _onPlaceScoreActionCommand.AddPartialScore(10, behaviourCoords);
            
            List<HexCoordinates> monoliths = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Monolith}, 3);
            for (int i = 0; i < monoliths.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(5, monoliths[i]);
            }
            
            List<HexCoordinates> carnivores = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Carnivores}, 1);
            for (int i = 0; i < carnivores.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(-10, carnivores[i]);
            }
            
            List<HexCoordinates> farms = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Farm}, 2);
            for (int i = 0; i < farms.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(10, farms[i]);
            }
            
            List<HexCoordinates> neighbourHerbivores = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Herbivores}, 1);
            if (neighbourHerbivores.Count > 0)
            {
                ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CannotPlace);
                ObjectCache.Current.PlayerTurnManager.PlayerTurnStatePlacing.SetCanBePlaced(false);
                return;
            }
            
            _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));

            
            List<HexCoordinates> movedHerbivores = new List<HexCoordinates>();
            List<HexCoordinates> grounds = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Ground}, 1);
            for (int i = 0; i < grounds.Count; i++)
            {
                List<HexCoordinates> herbivoresInRange2 = HexFunctions.GetTilesOfTypeInRadius(grounds[i],
                    new[] {TileType.Herbivores}, 1);
                for (int j = 0; j < herbivoresInRange2.Count; j++)
                {
                    if (movedHerbivores.Contains(herbivoresInRange2[j])) continue;
                    _commands.Add(new CreateAndPlaceTileActionCommand(herbivoresInRange2[j], TileType.Ground, 0.25f));
                    _commands.Add(new CreateAndPlaceTileActionCommand(grounds[i], TileType.Herbivores, 0.25f));
                    movedHerbivores.Add(herbivoresInRange2[j]);
                    _onPlaceScoreActionCommand.AddPartialScore(15, herbivoresInRange2[j]);
                    break;
                }
            }

            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            DiscoverNeighbors(behaviourCoords);
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
        }
        
        
        
        
        public override void Visit(CarnivoresBehaviour behaviour)
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);

            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
            
            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            // _onPlaceScoreCommand.AddPartialScore(10, behaviourCoords);
            
            // List<HexCoordinates> carns = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
            //     new[] {TileType.Carnivores}, 1);
            // if (carns.Count > 0)
            // {
            //     ObjectCache.Current.PlayerHand.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CannotPlace);
            //     ObjectCache.Current.PlayerHand.HandStatePlacing.SetCanBePlaced(false);
            //     return;
            // }
            
            List<HexCoordinates> monoliths = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Monolith}, 3);
            for (int i = 0; i < monoliths.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(15, monoliths[i]);
            }
            
            List<HexCoordinates> eatable = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                TileTypes.EatableTypes, 1);
            for (int i = 0; i < eatable.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(10,eatable[i]);
                _commands.Add(new CreateAndPlaceTileActionCommand(eatable[i], TileType.Ground,  0.5f));
            }

            if (eatable.Count == 0 && monoliths.Count == 0)
            {
                List<HexCoordinates> grounds = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                    new[] {TileType.Ground}, 1);
                for (int i = 0; i < grounds.Count; i++)
                {
                    _onPlaceScoreActionCommand.AddPartialScore(-5,grounds[i]);
                }
            }

            _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));

            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            DiscoverNeighbors(behaviourCoords);
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
        }
        
        
        
        public override void Visit(CampsiteBehaviour behaviour)
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);
            
            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
            
            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            _onPlaceScoreActionCommand.AddPartialScore(10, behaviourCoords);
            
            bool levelsUp = false;

            // con más de 1 vecino campsite o village --> evoluciona a Village
            List<HexCoordinates> campsOrVills = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Campsite, TileType.Village}, 1);
            if (campsOrVills.Count > 1)
            {
                levelsUp = true;
                _commands.Add(new CreateAndPlaceTileActionCommand(behaviourCoords, TileType.Village,  0.5f, true));
            }
            for (int i = 0; i < campsOrVills.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(5, campsOrVills[i]);
            }
            // comprobar vecinos campsite (si estos evolucionan a Village)
            List<HexCoordinates> camps = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Campsite}, 1);
            for (int i = 0; i < camps.Count; i++)
            {
                List<HexCoordinates> neighbourCampsOrVills = HexFunctions.GetTilesOfTypeInRadius(camps[i],
                    new[] {TileType.Campsite, TileType.Village}, 1);
                if (neighbourCampsOrVills.Count > 0)
                {
                    _onPlaceScoreActionCommand.AddPartialScore(5, camps[i]);
                    _commands.Add(new CreateAndPlaceTileActionCommand(camps[i], TileType.Village,  0.5f));
                }
            }
            // rodeado de village --> evoluciona a City
            List<HexCoordinates> vills = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Village}, 1);
            if (vills.Count > 5)
            {
                levelsUp = true;
                _onPlaceScoreActionCommand.AddPartialScore(10, behaviourCoords);
                _commands.Add(new CreateAndPlaceTileActionCommand(behaviourCoords, TileType.City,  0.5f, true));
                _commands.Add(new ShuffleCardIntoDeckActionCommand(TileType.Wastes, 0f, behaviourCoords));
            }
            // comprobar vecinos village (si estos evolucionan a City)
            for (int i = 0; i < vills.Count; i++)
            {
                List<HexCoordinates> neighbourVills = HexFunctions.GetTilesOfTypeInRadius(vills[i],
                    new[] {TileType.Village}, 1);
                if (neighbourVills.Count > 4)
                {
                    _onPlaceScoreActionCommand.AddPartialScore(10, vills[i]);
                    _commands.Add(new CreateAndPlaceTileActionCommand(vills[i], TileType.City,  0.5f));
                    _commands.Add(new ShuffleCardIntoDeckActionCommand(TileType.Wastes, 0f, vills[i]));
                }
            }
            if (!levelsUp)
                _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));
            
            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            DiscoverNeighbors(behaviourCoords);

            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
        }
        
        
        
        
        public override void Visit(ExplorerBehaviour behaviour)
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);
            
            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
            
            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            // _onPlaceScoreCommand.AddPartialScore(10, behaviourCoords);
            

            // limpia miasma (aplía grid) en un radio de 1 celdas
            if (ObjectCache.Current.HexGrid.Lists.GetTypeForCoord(behaviourCoords) == TileType.Frontier)
            {
                HexCoordinates? origin = HexFunctions.GetRandomOuterCellInRadius(behaviourCoords, 1);
                if (origin.HasValue)
                {
                    List<HexCoordinates> cellCoordsToDiscover = ObjectCache.Current.GridSizeManager.GetCellsToDiscover(RandomTf.Rng.Next(5,9),
                        (HexCoordinates) origin);
                    if (!cellCoordsToDiscover.Contains(behaviourCoords)) cellCoordsToDiscover.Add(behaviourCoords);
                    for (int i = 0; i < cellCoordsToDiscover.Count; i++)
                    {
                        // if (!cellCoordsToDiscover[i].Equals(behaviourCoords))
                        _commands.Add(new DiscoverTileActionCommand(cellCoordsToDiscover[i], true, 0.2f));
                        _onPlaceScoreActionCommand.AddPartialScore(2, cellCoordsToDiscover[i]);
                    }
                }
            } else if (ObjectCache.Current.HexGrid.Lists.GetTypeForCoord(behaviourCoords) == TileType.Ruins)
            {
                // todo pctg of wastes/campsite/quest
                float rand = RandomTf.Rng.Next(11)/10f;
                if (rand < 0.5)
                {
                    _commands.Add(new CardToHandActionCommand(TileType.Campsite, 1.5f, behaviourCoords));
                }
                else
                {
                    _commands.Add(new ShuffleCardIntoDeckActionCommand(TileType.Wastes, 0f, behaviourCoords));
                }
                _onPlaceScoreActionCommand.AddPartialScore(10, behaviourCoords);
            }
            
            
            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
        }




        public override void Visit(WastesBehaviour behaviour)
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);
            
            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
            
            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            // _onPlaceScoreCommand.AddPartialScore(10, behaviourCoords);

            _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));
            
            if (ObjectCache.Current.HexGrid.Lists.GetTypeForCoord(behaviourCoords) == TileType.Crystals)
            {
                _onPlaceScoreActionCommand.AddPartialScore(20, behaviourCoords);
            }
            else
            {
                _onPlaceScoreActionCommand.AddPartialScore(-10, behaviourCoords);
                CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            }
            
            
            DiscoverNeighbors(behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
            
        }

        
        
        
        public override void Visit(MeadowBehaviour behaviour)
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);
            
            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
            
            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            _onPlaceScoreActionCommand.AddPartialScore(10, behaviourCoords);
            
            List<HexCoordinates> monoliths = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Monolith}, 3);
            for (int i = 0; i < monoliths.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(5, monoliths[i]);
            }

            _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));


            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            DiscoverNeighbors(behaviourCoords);
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
            
            CheckNextMonolith(new List<HexCoordinates> { behaviourCoords });
        }
        
        
        
        
        
        public override void Visit(MountainBehaviour behaviour)
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);

            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
            
            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            _onPlaceScoreActionCommand.AddPartialScore(10, behaviourCoords);
            
            List<HexCoordinates> monoliths = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Monolith}, 3);
            for (int i = 0; i < monoliths.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(5, monoliths[i]);
            }

            List<HexCoordinates> forestsLakes = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Forest, TileType.Lake}, 1);
            for (int i = 0; i < forestsLakes.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(5, forestsLakes[i]);
            }
            
            
            List<HexCoordinates> mountains = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Mountain}, 1);
            if (mountains.Count > 1) // si hay más de 1 montaña vecina, que no sean vecinas entre ellas
            {
                if (HexFunctions.GetNeighborsInAdjacency(mountains).Count > 0) {
                    ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CannotPlace);
                    ObjectCache.Current.PlayerTurnManager.PlayerTurnStatePlacing.SetCanBePlaced(false);
                    return; // alguna es vecina de otra
                }
            }

            for (int i = 0; i < mountains.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(10, mountains[i]);
            }
            

            _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));
            
            // _commands.Add(new AddToFormationCommand(behaviour, behaviourCoords, TileType.Mountain, 0.1f)); // todo
            
            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            DiscoverNeighbors(behaviourCoords);
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
            
            CheckNextMonolith(new List<HexCoordinates> { behaviourCoords });
        }




        public override void Visit(PurifierBehaviour behaviour)
        {
             HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);

             if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
             
             _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
             // _onPlaceScoreCommand.AddPartialScore(10, behaviourCoords);

             _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));
            
            // discovers every frontier in range 2
            List<HexCoordinates> frontiers = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Frontier}, 2);
            for (int i = 0; i < frontiers.Count; i++)
            {
                _commands.Add(new DiscoverTileActionCommand(frontiers[i], false, 0.1f));
                _onPlaceScoreActionCommand.AddPartialScore(3, frontiers[i]);
            }

           
            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            // DiscoverNeighbors(behaviourCoords);
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
        }
        
        
        
        
        public override void Visit(RadioTowerBehaviour behaviour) 
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);

            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
             
            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            // _onPlaceScoreCommand.AddPartialScore(10, behaviourCoords);

            _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));
            
            // check for undiscovered terrain tiles in a radius around the tower
            List<HexCoordinates> undiscovered = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.None}, 4);

            List<HexCoordinates> undiscoveredTerrains = new List<HexCoordinates>();
            for (int i = 0; i < undiscovered.Count; i++)
            {
                if (ObjectCache.Current.TerrainBuilder.GetTerrainTileForCoords(undiscovered[i]) != TileType.Ground)
                    undiscoveredTerrains.Add(undiscovered[i]);
            }

            int numberOfTerrainsToDiscover = RandomTf.Rng.Next(1, 4);
            for (int i = 0; i < numberOfTerrainsToDiscover; i++)
            {
                if (undiscoveredTerrains.Count < 1) break;
                
                int randomTerrain = RandomTf.Rng.Next(undiscoveredTerrains.Count);
                _commands.Add(new DiscoverTileActionCommand(undiscoveredTerrains[randomTerrain], true, 0.15f));
                _onPlaceScoreActionCommand.AddPartialScore(7, undiscoveredTerrains[randomTerrain]);
                undiscoveredTerrains.RemoveAt(randomTerrain);
            }
            
            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            // DiscoverNeighbors(behaviourCoords);
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
        }
        
        
        
        
        public override void Visit(FarmBehaviour behaviour)
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);

            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
             
            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            _onPlaceScoreActionCommand.AddPartialScore(10, behaviourCoords);

            _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));
            
            List<HexCoordinates> farmlands = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Farmland}, 1);
            for (int i = 0; i < farmlands.Count; i++)
            {
                // check if farmlands aren't next to other FARM
                List<HexCoordinates> otherFarms = HexFunctions.GetTilesOfTypeInRadius(farmlands[i],
                    new[] {TileType.Farm}, 1);
                if (otherFarms.Count < 1)
                {
                    _onPlaceScoreActionCommand.AddPartialScore(5, farmlands[i]);
                }
                else
                {
                    _onPlaceScoreActionCommand.AddPartialScore(-10, otherFarms[0]);
                }
                    
            }
            
            
            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            DiscoverNeighbors(behaviourCoords);
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
        }
        
        
        
        
        public override void Visit(LumberjackBehaviour behaviour)
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);

            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
             
            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            _onPlaceScoreActionCommand.AddPartialScore(10, behaviourCoords);

            _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));
            
            List<HexCoordinates> forests = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Forest, TileType.DeepForest, TileType.WarpedWoods}, 1);
            for (int i = 0; i < forests.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(5, forests[i]);
            }

            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            DiscoverNeighbors(behaviourCoords);
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
        }




        public override void Visit(SwampBehaviour behaviour)
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);

            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
             
            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            _onPlaceScoreActionCommand.AddPartialScore(10, behaviourCoords);
            
            List<HexCoordinates> monoliths = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Monolith}, 3);
            for (int i = 0; i < monoliths.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(5, monoliths[i]);
            }

            _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));
            
            
            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            DiscoverNeighbors(behaviourCoords);
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
            
            CheckNextMonolith(new List<HexCoordinates> { behaviourCoords });
        }
        
        
        
        
        public override void Visit(DunesBehaviour behaviour)
        {
            HexCoordinates behaviourCoords = HexCoordinates.ToCoordinates(behaviour.transform.position);

            if (!CheckPlacingMatrix(behaviour, behaviourCoords))
                return;
             
            _onPlaceScoreActionCommand = new UpdateScoreActionCommand(behaviourCoords, 1.5f);
            _onPlaceScoreActionCommand.AddPartialScore(10, behaviourCoords);
            
            List<HexCoordinates> monoliths = HexFunctions.GetTilesOfTypeInRadius(behaviourCoords,
                new[] {TileType.Monolith}, 3);
            for (int i = 0; i < monoliths.Count; i++)
            {
                _onPlaceScoreActionCommand.AddPartialScore(5, monoliths[i]);
            }

            _commands.Add( new PlaceSelectedActionCommand(behaviourCoords, behaviour, 0.5f));
            
            
            CheckForNearCrystals(_onPlaceScoreActionCommand, behaviourCoords);
            _commands.Add(_onPlaceScoreActionCommand);
            DiscoverNeighbors(behaviourCoords);
            
            ObjectCache.Current.PlayerTurnManager.HoveredTileBorder.SetMat(TileBorderPool.HoverBorderType.CanPlace);
            
            CheckNextMonolith(new List<HexCoordinates> { behaviourCoords });
        }
        
    }
}