using System;
using System.Collections.Generic;
using System.Reflection;
using MbsUnity.Runtime.Common;
using UI.Menus;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.MenuSystem
{
    [Serializable]
    public class MenuManager : SingletonBehaviour<MenuManager>
    {
        [Header("Sounds")] 
        [SerializeField] public AudioClip ClipMove;
        [SerializeField] public AudioClip ClipSelect;
        [SerializeField] public AudioClip ClipCancel;


        [Header("Menu prefabs")] 
        [SerializeField] public MainMenu MainMenu;
        [SerializeField] public CardListMenu CardListMenu;
        [SerializeField] public RunConfigMenu runConfigMenu;
        [SerializeField] public OptionsMenu OptionsMenu;
        [SerializeField] public PauseMenu PauseMenu;
        [SerializeField] public GameEndMenu GameEndMenu;
        [SerializeField] public CreditsMenu CreditsMenu;


        private Stack<Menu> menuStack = new Stack<Menu>();
        private AudioSource _audioSource;

        public override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }


        public void PlayAudioSelect()
        {
            _audioSource.Stop();
            _audioSource.clip = ClipSelect;
            // TODO _audioSource.MbsPlayMenu();
        }
        
        public void PlayAudioMove()
        {
            _audioSource.Stop();
            _audioSource.clip = ClipMove;
            // TODO _audioSource.MbsPlayMenu(.7f);
        }
        
        public void PlayAudioCancel()
        {
            _audioSource.Stop();
            _audioSource.clip = ClipCancel;
            // TODO _audioSource.MbsPlayMenu(.7f);
        }
        
        public void OpenMenu(Menu instance)
        {
            // De-activate top menu
            if (menuStack.Count > 0)
            {
                if (instance.DisableMenusUnderneath)
                {
                    foreach (var menu in menuStack)
                    {
                        menu.gameObject.SetActive(false);

                        if (menu.DisableMenusUnderneath)
                            break;
                    }
                }

                var topCanvas = instance.GetComponent<Canvas>();
                var previousCanvas = menuStack.Peek().GetComponent<Canvas>();
                topCanvas.sortingOrder = previousCanvas.sortingOrder + 1;
            }

            menuStack.Push(instance);
        }


        public T CreateInstance<T>() where T : Menu
        {
            T prefab = GetPrefab<T>();
            return Instantiate(prefab, transform);
        }

        private T GetPrefab<T>() where T : Menu
        {
            // Get prefab dynamically, based on public fields set from Unity
            // You can use private fields with SerializeField attribute too
            var fields = this.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var field in fields)
            {
                var prefab = field.GetValue(this) as T;
                if (prefab != null)
                {
                    return prefab;
                }
            }

            throw new MissingReferenceException("Prefab not found for type " + typeof(T));
        }


        public void CloseMenu(Menu menu)
        {
            if (menuStack.Count == 0)
            {
                Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", menu.GetType());
                return;
            }

            if (menuStack.Peek() != menu)
            {
                Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", menu.GetType());
                return;
            }

            CloseTopMenu();
        }

        public void CloseTopMenu()
        {
            var instance = menuStack.Pop();
            

            if (instance.DestroyWhenClosed)
                Destroy(instance.gameObject);
            else instance.gameObject.SetActive(false);

            // Re-activate top menu
            // If a re-activated menu is an overlay we need to activate the menu under it
            foreach (var menu in menuStack)
            {
                menu.gameObject.SetActive(true);
                menu.OnReactivate();

                if (menu.DisableMenusUnderneath)
                    break;
            }
        }

        public void Clear()
        {
            while (this.menuStack.Count > 0)
            {
                Menu instance = this.menuStack.Pop();
                Destroy(instance.gameObject);
            }
        }
        
        
        
        private T GetComponentInParent<T>(GameObject go)
        {
            Transform trans = go.transform;
            T comp;
            
            while (trans != null)
            {
                if (trans.TryGetComponent(out comp))
                    return comp;
                trans = trans.parent;
            }
            return default;
        }


        /// <summary>
        /// Unity interface navigation overrides
        /// </summary>
        private void Update()
        {
            if (EventSystem.current == null)
                return;
            
            GameObject selectedGo = EventSystem.current.currentSelectedGameObject;
            //if (selectedGo == null)
            //    return;

            if (menuStack.Count <= 0)
                return;

#if UNITY_SWITCH || UNITY_XBOXONE
            // if (MenuManager_NavigationConsoles.Update(selectedGo))
            //     return;   
#else
            if (MenuManager_NavigationPc.Update(selectedGo))
                return;
#endif
            MenuManager_BtnAccept.Update(selectedGo);
            MenuManager_BtnCancel.Update(selectedGo, menuStack);
        }
    }
}