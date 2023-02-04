using System.Collections.Generic;
using Controls;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.MenuSystem
{
    public static class MenuManager_BtnCancel
    {
        public static void Update(GameObject selectedGo, Stack<Menu> menuStack)
        {
            if (GameControls.IsUiCancelPressed(out int playerCancel))
            {
                if (selectedGo != null)
                {
                    TMP_Dropdown dropdown = selectedGo.GetComponentInParent<TMP_Dropdown>();
                    if (dropdown != null && dropdown.IsExpanded)
                    {
                        BaseEventData data = new BaseEventData(EventSystem.current);
                        dropdown.OnCancel(data);
                        return;
                    }
                }

                menuStack.Peek().OnBackPressed();
            }
        }
    }
}