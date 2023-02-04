using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.MenuSystem
{
    public static class MenuManager_NavigationPc
    {
        private static void MoveSlider(Slider slider, MoveDirection dir, Vector2 moveVec)
        {
            AxisEventData data = new AxisEventData(EventSystem.current);
            data.moveDir = dir;
            data.moveVector = moveVec;
            slider.OnMove(data);
        }
        
        
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedGo"></param>
        /// <returns>Whether MenuManager should return</returns>
        public static bool Update(GameObject selectedGo)
        {
#if NOT_DONE_YET
            if (GameControls.IsUiDPadTopPressed())
            {
                Selectable selectable = selectedGo.GetComponent<Selectable>();
                Selectable up = selectable.FindSelectableOnUp();
                if (up != null)
                {
                    EventSystem.current.SetSelectedGameObject(up.gameObject);
                    return true;
                }
            }
            if (GameControls.IsUiDPadBottomPressed())
            {
                Selectable selectable = selectedGo.GetComponent<Selectable>();
                Selectable down = selectable.FindSelectableOnDown();
                if (down != null)
                {
                    EventSystem.current.SetSelectedGameObject(down.gameObject);
                    return true;
                }
            }
            if (GameControls.IsUiDPadRightPressed())
            {
                Selectable selectable = selectedGo.GetComponent<Selectable>();
                
                if (selectable is Slider slider)
                {
                    MoveSlider(slider, MoveDirection.Right, Vector2.right);
                    return true;
                }
                Selectable right = selectable.FindSelectableOnRight();
                if (right != null)
                {
                    EventSystem.current.SetSelectedGameObject(right.gameObject);
                    return true;
                }
            }
            if (GameControls.IsUiDPadLeftPressed())
            {
                Selectable selectable = selectedGo.GetComponent<Selectable>();
                
                if (selectable is Slider slider)
                {
                    MoveSlider(slider, MoveDirection.Left, Vector2.left);
                    return true;
                }
                Selectable left = selectable.FindSelectableOnLeft();
                if (left != null)
                {
                    EventSystem.current.SetSelectedGameObject(left.gameObject);
                    return true;
                }
            }
#endif

            return false;
        }
    }
}