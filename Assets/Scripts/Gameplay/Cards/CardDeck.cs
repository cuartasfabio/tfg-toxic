using System.Collections.Generic;
using Backend.Persistence;
using Gameplay.Tiles;
using UnityEngine;
using Utils;

namespace Gameplay.Cards
{
	public class CardDeck : MonoBehaviour, IPersistable
    {
        /// <summary>
        /// The data for building the current level deck.
        /// </summary>
        private CardDeckData _deckData;
        /// <summary>
        /// Shuffled card list to draw from.
        /// </summary>
        private List<TileType> _shuffledDeck;
        /// <summary>
        /// The LAST index in the shuffled deck where we picked a card.
        /// </summary>
        private int _lastPickIndex;

        /// <summary>
        /// Cards inserted in the deck during the current game.
        /// Inserted cards are permanent so every time the deck refills, every card in _insertions
        /// replaces a random card.
        /// </summary>
        private List<TileType> _insertions;

        public void Init(CardDeckData deckData)
        {
            _deckData = deckData;
            _insertions = new List<TileType>();
            FillUpDeck();
        }
        
        /// <summary>
        /// Empties the shuffled card list and populates it with
        /// new cards picked from the pools of CardDeckData.
        /// </summary>
        private void FillUpDeck()
        {
            _lastPickIndex = -1;
            _shuffledDeck = new List<TileType>();
            
            int normalAppearances = _deckData.PoolSize / _deckData.NormalTier.Length;
            
            // adds normalTier cards
            for (int i = 0; i < _deckData.NormalTier.Length; i++)
            {
                for (int j = 0; j < normalAppearances; j++)
                {
                    _shuffledDeck.Add(_deckData.NormalTier[i]);
                }
            }
            
            // adds specialTiers cards
            for (int i = 0; i < _deckData.SpecialTiers.Length; i++)
            {
                // calculates the number of specialTier cards to add based on (normalAppearances/normalPerSpecial)
                int specialAppearances = (int) Mathf.Round(normalAppearances / _deckData.SpecialTiers[i].NormalPerSpecial);
                AddSpecialTiers(i, specialAppearances);
            }
            
            // shuffles the populated list
            _shuffledDeck.KFYShuffle();
            
            
            // only after shuffling, every inserted card replaced one in the deck (starting from index=0)
            int range = Mathf.Min(_insertions.Count, _shuffledDeck.Count);
            for (int i = 0; i < range; i++)
            {
                _shuffledDeck[i] = _insertions[i];
            }
            
            // if insertions been made, shuffles again to distribute insertions
            if (range > 0) _shuffledDeck.KFYShuffle();
        }

        private void AddSpecialTiers(int index, int specialAppearances)
        {
            for (int j = 0; j < _deckData.SpecialTiers[index].CardTypes.Length; j++)
            {
                for (int k = 0; k < specialAppearances; k++)
                {
                    _shuffledDeck.Add(_deckData.SpecialTiers[index].CardTypes[j]);
                }
            }
        }

        public TileType DrawRandomCardType()
        {
            // if we drawed the last card of the shuffled list, refill it.
            if (_lastPickIndex >= _shuffledDeck.Count - 1)
                FillUpDeck();
            
            _lastPickIndex++;
            
            return _shuffledDeck[_lastPickIndex]; // todo index out of bounds exc. ?
        }

        /// <summary>
        /// Method called when a new insertion in the deck is made during the game.
        /// Shuffles the inserted card in, and stores it in _insertions.
        /// </summary>
        /// <param name="cardType">The TileType to shuffle in.</param>
        public void ShuffleIntoDeck(TileType cardType)
        {
            int start = _lastPickIndex + 1 >= _shuffledDeck.Count ? _shuffledDeck.Count - 1 : _lastPickIndex + 1;

            int randPos = _lastPickIndex;
            bool sameType = true;
            while (sameType)
            {
                randPos = RandomTf.Rng.Next(start,_shuffledDeck.Count);
                sameType = _shuffledDeck[randPos] == cardType;
            }
            
            _insertions.Add(cardType);
            _shuffledDeck[randPos] = cardType;
        }
        
        // -------------------------------------------------------------------------------------------------------------

        public void Save(GameDataWriter writer)
        {
			writer.Write(_lastPickIndex);
            
            writer.Write(_shuffledDeck.Count);
            for (int i = 0; i < _shuffledDeck.Count; i++)
            {
                writer.Write((int) _shuffledDeck[i]);
            }
            
            writer.Write(_insertions.Count);
            for (int i = 0; i < _insertions.Count; i++)
            {
                writer.Write((int) _insertions[i]);
            }
        }

        public void Load(GameDataReader reader)
        {
            _lastPickIndex = reader.ReadInt();
            
            _shuffledDeck = new List<TileType>();
            int cardsCount = reader.ReadInt();
            for (int i = 0; i < cardsCount; i++)
            {
                _shuffledDeck.Add((TileType) reader.ReadInt());
            }
            
            _insertions = new List<TileType>();
            int insertsCount = reader.ReadInt();
            for (int i = 0; i < insertsCount; i++)
            {
                _insertions.Add((TileType) reader.ReadInt());
            }
        }
	}
}