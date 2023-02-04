using System.Collections.Generic;
using Backend.Persistence;
using Gameplay.Levels;
using MbsUnity.Runtime.Common;
using MbsUnity.Runtime.Common.DTO;
using Scenes;
using Settings;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif


public class GameManager : SingletonBehaviour<GameManager>
{
    [SerializeField] private GameObject _cutscenePrefab;
    [SerializeField] private Canvas _cutsceneCanvas;

    [SerializeField] private List<GameModeData> _gameModeConfigs;

    public RunManager RunManager { get; private set; }
    
    public SettingsManager SettingsManager { get; private set; }
    
    public PlayerStats PlayerStats { get; private set; }

    // todo public LevelStats LevelStats (in GameSceneManager?)
    
    public PersistentStorage PersistentStorage;

    public const string GameVersion = "UNIVERSIDAD DE OVIEDO - TRABAJO FIN DE GRADO - TOXIC FRONTIER - FABIO CUARTAS PUENTE"; //"usability-test-build";

    
    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;

        SettingsManager = new SettingsManager();
        PlayerStats = new PlayerStats();

        // ====== Boot Up Backend ===================
        PersistentStorage = new PersistentStorage();
        
        PersistentStorage.Load(SettingsManager, PersistentStorage.SaveType.SETTINGS);
        SettingsManager.RefreshCurrentSettings();
        
        PersistentStorage.Load(PlayerStats, PersistentStorage.SaveType.PLAYERSTATS);
        
        TryLoadRun();
        // todo PersistentStorage.LoadStats
        

        // ====== Start Game ========================
        if (SceneManager.GetActiveScene().name == "LoadScene")
        {
            // run the cutscene
            // Instantiate(_cutscenePrefab, _cutsceneCanvas.transform).GetComponent<Cutscene>().Play();
            SceneManager.LoadScene("MainMenuScene");
        }
        if (SceneManager.GetActiveScene().name == "GameScene")
            SceneManager.LoadScene("RunSetUpScene");
        
        // cargar audio y más stuff
    }

    public void CreateRun(GameMode gameMode)
    {
        PersistentStorage.DeleteSavedRun();
        RunManager = new RunManager(_gameModeConfigs[(int)gameMode]);
        // SaveRun();
    }

    public void ClearRun()
    {
        RunManager = null;
        PersistentStorage.DeleteSavedRun();
        
    }

    public void TryLoadRun() // every time game starts, if theres a run save file, load it
    {
        if (PersistentStorage.Exists(PersistentStorage.SaveType.RUN))
        {
            RunManager = new RunManager();
            PersistentStorage.Load(RunManager, PersistentStorage.SaveType.RUN);
        }
    }

    public bool IsThereASavedRun()
    {
        return PersistentStorage.Exists(PersistentStorage.SaveType.RUN);
    }
    
    // ---------------------------

    public void SaveSettings()
    {
        PersistentStorage.Save(SettingsManager, PersistentStorage.SaveType.SETTINGS);
    }
    
    public void SavePlayerStats()
    {
        PersistentStorage.Save(PlayerStats, PersistentStorage.SaveType.PLAYERSTATS);
    }

    public void SaveRun()
    {
        PersistentStorage.Save(RunManager, PersistentStorage.SaveType.RUN);
        PersistentStorage.Save(RunManager.LevelManager, PersistentStorage.SaveType.LEVEL);
    }
    
    
    
    public GameModeData GetGameModeConfig(GameMode gm)
    {
        return _gameModeConfigs[(int) gm];
    }
    
    /*
     * 
     */
 
#if UNITY_EDITOR
    [MenuItem("Edit/Play from Load Scene %9")]
    public static void PlayFromPreLaunchScene()
    {
        if ( EditorApplication.isPlaying == true )
        {
            EditorApplication.isPlaying = false;
            return;
        }
     
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/LoadScene.unity");
        EditorApplication.isPlaying = true;
    }
#endif

    public override void OnDestroy()
    {
        base.OnDestroy();
        DataTransferBus.Dispose();
    }
    
}