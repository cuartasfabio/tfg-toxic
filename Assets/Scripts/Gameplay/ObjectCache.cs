using Cameras;
using Gameplay.Cards;
using Gameplay.Commands;
using Gameplay.Grids;
using Gameplay.Grids.GridHelpers;
using Gameplay.PlayerControllers;
using Gameplay.PlayerTurn;
using Gameplay.Quests;
using Gameplay.Scores;
using Gameplay.TileCreations;
using Gameplay.TileCreations.Formations;
using Gameplay.Tiles;
using UI.InGames;
using UI.InGames.DescriptionHelp;
using UI.InGames.ScoreTexts;
using UI.InGames.TileBorders;
using UI.InGames.TileInfos;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Cache object containing references to every controller and manager on the Game Scene.
    /// </summary>
    public class ObjectCache : MonoBehaviour
    {
        public static ObjectCache Current { get; private set; }
    
        // [SerializeField] public Canvas InGameUICanvas;
        [SerializeField] public InGameUI InGameUI;
    
        [SerializeField] public CommandBuffer CommandBuffer;

        [SerializeField] public UICardHand UiCardHand;
        [SerializeField] public CardDeck CardDeck;
        [SerializeField] public CardPool CardPool;

        [SerializeField] public CameraControl CameraControl;
        [SerializeField] public RaycastManager RaycastManager;
    
        [SerializeField] public PlayerTurnManager PlayerTurnManager;
        
        [SerializeField] public TerrainBuilder TerrainBuilder;

        [SerializeField] public HexGrid HexGrid;
        [SerializeField] public GridSizeManager GridSizeManager;
    
        [SerializeField] public TileBehaviourPool TileBehaviourPool;

        [SerializeField] public FormationsRegister FormationsRegister;

        // [SerializeField] public GameSceneManager GameSceneManager;

        [SerializeField] public ScoreTextsPool ScoreTextPool;

        [SerializeField] public TileInfosPool TileInfosPool;
    
        [SerializeField] public TileBorderPool TileBorderPool;
    
        [SerializeField] public ScoreManager ScoreManager;

        [SerializeField] public UIQuestList UIQuestList;

        [SerializeField] public HexBackground HexBackground;
        
        [SerializeField] public HelpManager HelpManager;

        [Space]
    
        [SerializeField] public Camera MainCamera;
        [SerializeField] public Transform DynamicCardBehaviours;
        
        
        private void Awake()
        {
            Current = this;
        }
    
    }
}
