using UnityEngine;

namespace UI.MenuSystem
{
    public static class MenuManager_BtnAccept
    {
        public static void Update(GameObject selectedGo)
        {
#if NOT_DONE_YET
            if (GameControls.IsUiAcceptPressed(out int playerAccept))
            {
                // TODO remove accept from Unity's default mapping
                if (GameControls.IsUsingGamepad(playerAccept))
                {
                    if (selectedGo.transform.TryGetComponent(out Button btn))
                    {
                        BaseEventData data = new BaseEventData(EventSystem.current);
                        btn.OnSubmit(data);
                        //btn.onClick.Invoke();
                    }
                    else if (selectedGo.transform.TryGetComponent(out Toggle toggle))
                    {
                        //toggle.isOn = !toggle.isOn;
                        BaseEventData data = new BaseEventData(EventSystem.current);
                        toggle.OnSubmit(data);
                    }
                    else if (selectedGo.transform.TryGetComponent(out MbsDropdown mbsDropdown))
                    {
                        BaseEventData data = new BaseEventData(EventSystem.current);
                        mbsDropdown.OnSubmit(data);
                    }
                    else if (selectedGo.transform.TryGetComponent(out TMP_Dropdown dropdown))
                    {
                        BaseEventData data = new BaseEventData(EventSystem.current);
                        dropdown.OnSubmit(data);
                    }
                }
            }
#endif
        }
    }
}