using System.Collections.Generic;
using Backend.Persistence;
using Gameplay.Grids.Hexes.HexHelpers;
using UnityEngine;

namespace Gameplay.TileCreations.Formations
{
    /// <summary>
    /// This class stores the lake formations and their borders.
    /// Adds new lakes to existing formations.
    /// Refreshes the sprites for the neighbour lakes in formation.
    /// </summary>
    public class FormationsRegister: MonoBehaviour, IPersistable
    {
        [SerializeField] private AdjacencySpritesData _lakeVariants;
        public AdjacencySpritesData LakeVariants => _lakeVariants;

        private List<HashSet<HexCoordinates>> _lakeFormations;
        private List<HashSet<HexCoordinates>> _lakeFormationsBorders;

        private void Awake()
        {
            _lakeFormations = new List<HashSet<HexCoordinates>>();
            _lakeFormationsBorders = new List<HashSet<HexCoordinates>>();
        }

        public void RegisterLake(HexCoordinates lakeCoords)
        {
            int lakeFormation = GetLakeFormationIndexToRegister(lakeCoords);

            if (lakeFormation < 0)
            {
                // hay que crear nueva formación
                _lakeFormations.Add(new HashSet<HexCoordinates>());
                _lakeFormationsBorders.Add(new HashSet<HexCoordinates>());
                lakeFormation = _lakeFormations.Count - 1;
            }
            
            AddToFormation(lakeFormation, lakeCoords);
        }
        
        public void RemoveLakeFromFormation(HexCoordinates toDeleteCoords)
        {
            // en que formation está?
            int lakeFormation = GetLakeFormation(toDeleteCoords);

            if (lakeFormation >= 0)
            {
                // remove lake from formations
                _lakeFormations[lakeFormation].Remove(toDeleteCoords);
                // recalculate formationBorders
                RecalculateFormationBorders(lakeFormation);
            }
        }

        private void RecalculateFormationBorders(int formation)
        {
            _lakeFormationsBorders[formation].Clear();
            foreach (HexCoordinates hexCoord in _lakeFormations[formation])
                AddLakeBordersToFormation(hexCoord, formation);
        }
        
        public (List<HexCoordinates>, int) GetCoordsOfFormations(HexCoordinates lakeCoords)
        {
            List<HexCoordinates> res = new List<HexCoordinates>();
            int numberOfForms = 0;

            for (int i = 0; i < _lakeFormationsBorders.Count; i++)
            {
                if (_lakeFormationsBorders[i].Contains(lakeCoords))
                {
                    numberOfForms++;
                    foreach (HexCoordinates hexCoord in _lakeFormations[i])
                        res.Add(hexCoord);
                }
            }

            return (res, numberOfForms);
        }

        private void AddToFormation(int lakeFormation, HexCoordinates lakeCoords)
        {
            _lakeFormations[lakeFormation].Add(lakeCoords);

            AddLakeBordersToFormation(lakeCoords, lakeFormation);
        }

        private void AddLakeBordersToFormation(HexCoordinates lakeCoords, int formation)
        {
            List<HexCoordinates> borders = HexFunctions.GetNeighborCoords(lakeCoords);

            for (int i = 0; i < borders.Count; i++)
                _lakeFormationsBorders[formation].Add(borders[i]);
        }

        private int GetLakeFormationIndexToRegister(HexCoordinates lakeCoords)
        {
            List<int> formations = new List<int>();
            
            for (int i = 0; i < _lakeFormationsBorders.Count; i++)
                if (_lakeFormationsBorders[i].Contains(lakeCoords))
                    formations.Add(i);
            
            if (formations.Count > 1)
                return JoinFormations(formations);

            if (formations.Count == 1)
                return formations[0];
            
            return -1;
        }
        
        private int JoinFormations(List<int> indexes)
        {
            // juntar las formations de una en una, todas a la primera

            for (int i = indexes.Count-1; i > 0; i--)
                JoinTwoFormations(indexes[i], indexes[0]);

            for (int i = indexes.Count-1; i > 0; i--)
            {
               // eliminar formation
               _lakeFormations.RemoveAt(indexes[i]);
               // eliminar formationBorders
               _lakeFormationsBorders.RemoveAt(indexes[i]);
            }

            return indexes[0];
        }

        private void JoinTwoFormations(int indexToJoin, int index)
        {
            // juntar contenido de lakeFormations
            foreach (HexCoordinates hexCoord in _lakeFormations[indexToJoin])
                _lakeFormations[index].Add(hexCoord);

            // juntar contenido de lakeFormationBorders
            foreach (HexCoordinates hexCoord in _lakeFormationsBorders[indexToJoin])
                _lakeFormationsBorders[index].Add(hexCoord);
        }

        private int GetLakeFormation(HexCoordinates lakeCoords)
        {
            for (int i = 0; i < _lakeFormations.Count; i++)
                if (_lakeFormations[i].Contains(lakeCoords))
                    return i;

            return -1;
        }
        
        // -------------------------------------------------------------------------------------------------------------

        public void Save(GameDataWriter writer)
        {
            // lake formations
            writer.Write(_lakeFormations.Count);
            for (int i = 0; i < _lakeFormations.Count; i++)
            {
                writer.Write(_lakeFormations[i].Count);
                foreach (var coord in _lakeFormations[i])
                {
                    writer.Write(coord);  
                }
            }
            
            // lake formations borders
            writer.Write(_lakeFormationsBorders.Count);
            for (int i = 0; i < _lakeFormationsBorders.Count; i++)
            {
                writer.Write(_lakeFormationsBorders[i].Count);
                foreach (var coord in _lakeFormationsBorders[i])
                {
                    writer.Write(coord);  
                }
            }
        }

        public void Load(GameDataReader reader)
        {
            int formNum = reader.ReadInt();
            for (int i = 0; i < formNum; i++)
            {
                _lakeFormations.Add(new HashSet<HexCoordinates>());
                int hashNum = reader.ReadInt();
                for (int j = 0; j < hashNum; j++)
                {
                    _lakeFormations[i].Add(reader.ReadHexCoords());
                }
            }
            
            int borderNum = reader.ReadInt();
            for (int i = 0; i < borderNum; i++)
            {
                _lakeFormationsBorders.Add(new HashSet<HexCoordinates>());
                int hashNum = reader.ReadInt();
                for (int j = 0; j < hashNum; j++)
                {
                    _lakeFormationsBorders[i].Add(reader.ReadHexCoords());
                }
            }
        }
    }
}