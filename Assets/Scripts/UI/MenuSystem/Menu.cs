using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.MenuSystem
{
    /// <summary>
    /// Non-generic menu, because MonoBehaviours cannot be generic.
    /// </summary>
    public abstract class Menu : MonoBehaviour
    {
        [Tooltip("Destroy the Game Object when menu is closed (reduces memory usage)")]
        public bool DestroyWhenClosed = true;

        [Tooltip("Disable menus that are under this one in the stack")]
        public bool DisableMenusUnderneath = true;

        public abstract void OnBackPressed();

        public virtual void OnReactivate()
        {
            // To be implemented by chinldren
        }
    }
    
    
    public abstract class Menu<T> : Menu where T : Menu<T>
    {
        public static T Instance { get; private set; }
        
        public abstract bool PlayAudioOnOpen { get; } 
        public abstract bool PlayAudioOnClose { get; } 
        
        private GameObject _lastHighlightedObject;

        protected virtual void Awake()
        {
            Instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }

        /// <summary>
        /// This function must remain protected.
        /// The best way to open a given menu is to create a static method 'Show',
        /// where we parametrize the menu if needed, and then call to Open.
        /// </summary>
        protected static void _Open()
        {
            if (Instance == null)
            {
                Instance = MenuManager.Get().CreateInstance<T>();
            }
            else
            {
                Instance.gameObject.SetActive(true);
            }

            MenuManager manager = MenuManager.Get();
           
            if (Instance.PlayAudioOnOpen) 
                manager.PlayAudioSelect();
            
            manager.OpenMenu(Instance);
            
            Instance.OnMenuOpen();
        }

        protected static void _Close()
        {
            if (Instance == null)
            {
                Debug.LogErrorFormat("Trying to close menu {0} but Instance is null", typeof(T));
                return;
            }

            MenuManager manager = MenuManager.Get();
            
            
            if (Instance.PlayAudioOnClose) 
                manager.PlayAudioCancel();
            
            manager.CloseMenu(Instance);
        }

        public override void OnBackPressed()
        {
            _Close();
        }

        protected virtual void OnMenuOpen()
        {
            // Do nothing
        }
        

        public void Highlight(GameObject obj)
        {
            _lastHighlightedObject = obj;
            EventSystem.current.SetSelectedGameObject(_lastHighlightedObject);
        }
        
        private void LateUpdate()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(_lastHighlightedObject);

            _lastHighlightedObject = EventSystem.current.currentSelectedGameObject;
        }
    }
}