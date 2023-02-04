using Audio;
using Backend.Localization;
using Backend.Persistence;
using Controls;
using Gameplay;
using Gameplay.Levels;
using UI.Menus;
using UnityEngine;

namespace Scenes
{
       public class GameSceneManager : MonoBehaviour
       {
              public static GameSceneManager Current { get; private set; } //todo change to SingletonBehaviour
       
              public bool IsPaused => _isPaused;
              private bool _isPaused;
              private float _pauseCountdown;
              
              //-----------------------------------------------------
              private RunManager _runManager;
              //-----------------------------------------------------
              
              [Space] 
              [SerializeField] private Sprite _loseSprite;
              [SerializeField] private Sprite _winSprite;

              private void Awake()
              {
                     Current = this;
                     _pauseCountdown = -1;
              }

              /// <summary>
              /// Initializes the scene with the data from the next level (if there's a saved level, then loads it).
              /// </summary>
              private void Start()
              {
                     _runManager = GameManager.Get().RunManager;
                     
                     if (_runManager.CurrentLevelIndex >= 0)
                     {
                            LoadLevelFromSave();
                     }
                     else
                     {
                            LoadNextLevel();
                     }
              }
              
              private void LoadLevelFromSave()
              {
                     _runManager.LoadSavedLevel();
                     StartCoroutine(ObjectCache.Current.InGameUI.Open());
              }

              private void LoadNextLevel()
              {
                     StartCoroutine(_runManager.LoadNextLevel());
                     StartCoroutine(ObjectCache.Current.InGameUI.Open());
              }
              

              private void Update()
              {
                     if (_isPaused)
                            return;
              
                     // ======= Handle Pauses ========================================

                     if (_pauseCountdown > 0)
                     {
                            _pauseCountdown -= Time.deltaTime;
                     }
                     else
                     {
                            if (GameControls.IsPauseResumePressed(out int playerPause))
                            {
                                   Pause();
                                   AudioController.Get().PlaySfx(AudioId.SFX_Pause);
                                   StartCoroutine(ObjectCache.Current.InGameUI.Hide(PauseMenu.Show));
                            }
                     }
              }

              // public void OpenPause(Action doLast)
              // {
              //        StartCoroutine(ObjectCache.Current.InGameUI.HideInGameUI(doLast));
              // }
       
              public void Pause()
              {
                     _isPaused = true;
                     _pauseCountdown = 0.2f;
                     // ocultar la in-game ui
                     // ObjectCache.Current.InGameUI.gameObject.SetActive(false);
                     // desactivar movimiento de camara
                     ObjectCache.Current.CameraControl.enabled = false;
                     // desactivar cursor
                     ObjectCache.Current.PlayerTurnManager.enabled = false;
                     // y si está colocando un item? soltarlo
                     ObjectCache.Current.PlayerTurnManager.CancelPlacing();
              }
       
              // public void ClosePause(Action doLast)
              // {
              //        StartCoroutine(ObjectCache.Current.InGameUI.ShowInGameUI(doLast));
              // }

              public void Resume()
              {
                     _isPaused = false;
                     // mostrar in-game ui
                     // ObjectCache.Current.InGameUI.gameObject.SetActive(true);
                     // reactivar camera movement
                     ObjectCache.Current.CameraControl.enabled = true;
                     // reactivar cursor
                     ObjectCache.Current.PlayerTurnManager.enabled = true;
              }

              

              public void TriggerGameOver(string reasonText = "")
              {
                     AudioController.Get().StopSoundtrack(PlaylistId.PL_ST_WORLD1, true);
                     AudioController.Get().StopAmbience(PlaylistId.PL_AM_WORLD1,true);      // todo refact.
                     AudioController.Get().PlaySfx(AudioId.SFX_GameOver, true, 1f);
                     GameEndMenu.Config(reasonText, _loseSprite);
                     Pause();
                     StartCoroutine(ObjectCache.Current.InGameUI.Hide(GameEndMenu.Show));
              }


              public void GoToNextWorld()
              {
                     if (_runManager.IsThereANextLevel())
                     {
                            StartCoroutine(ObjectCache.Current.InGameUI.Close(LoadNextLevel));
                     }
                     else
                     {
                            TriggerWinMenu(StringBank.GetStringRaw("WIN_MESSAGE"));
                     }
              }
              
              private void TriggerWinMenu(string reasonText = "")
              {
                     AudioController.Get().StopSoundtrack(PlaylistId.PL_ST_WORLD1, true);
                     AudioController.Get().StopAmbience(PlaylistId.PL_AM_WORLD1,true);      // todo refact.
                     AudioController.Get().PlaySfx(AudioId.SFX_GameOver, true, 1f);
                     GameEndMenu.Config(reasonText, _winSprite);
                     Pause();
                     StartCoroutine(ObjectCache.Current.InGameUI.Hide(GameEndMenu.Show));
              }
       }
}