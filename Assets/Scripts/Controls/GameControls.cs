using System;
using UnityEngine;

namespace Controls
{
    public static class GameControls
    {
        private static bool _controlsEnabled = true;


        private static bool CheckEnableKey(Predicate<KeyCode> control, KeyCode keyCode)
        {
            return control(keyCode) && _controlsEnabled;
        }
        
        private static bool CheckEnableMouse(Predicate<int> control, int mouseBtn)
        {
            return control(mouseBtn) && _controlsEnabled;
        }
        
        
        
        public static bool IsUiCancelPressed(out int i)
        {
            i = 0;
            return CheckEnableKey(Input.GetKeyDown,KeyCode.Escape);
            // return Input.GetKeyDown(KeyCode.Escape);
        }

        public static bool IsUiAcceptPressed(out int i)
        {
            i = 0;
            return CheckEnableKey(Input.GetKeyDown,KeyCode.Return);
            // return Input.GetKeyDown(KeyCode.Return);
        }
        
        public static bool IsUiUpPressed(out int i)
        {
            i = 0;
            return CheckEnableKey(Input.GetKeyDown,KeyCode.W);
            // return Input.GetKeyDown(KeyCode.W);
        }
        
        public static bool IsUiLeftPressed(out int i)
        {
            i = 0;
            return CheckEnableKey(Input.GetKeyDown,KeyCode.A);
            // return Input.GetKeyDown(KeyCode.A);
        }
        
        public static bool IsUiDownPressed(out int i)
        {
            i = 0;
            return CheckEnableKey(Input.GetKeyDown,KeyCode.S);
            // return Input.GetKeyDown(KeyCode.S);
        }
        
        public static bool IsUiRightPressed(out int i)
        {
            i = 0;
            return CheckEnableKey(Input.GetKeyDown,KeyCode.D);
            // return Input.GetKeyDown(KeyCode.D);
        }

        public static bool IsUiMouseLeftButtonPressed(out int i)
        {
            i = 0;
            return CheckEnableMouse(Input.GetMouseButtonDown,0);
            // return Input.GetMouseButtonDown(0);
        }
        
        public static bool IsUiMouseRightButtonPressed(out int i)
        {
            i = 0;
            return CheckEnableMouse(Input.GetMouseButtonDown,1);
            // return Input.GetMouseButtonDown(1);
        }

        public static float GetUiZoomValue(out int i)
        {
            i = 0;
            return Input.GetAxis("Mouse ScrollWheel");
        }
        
        public static bool IsUiRotateLeftPressed(out int i)
        {
            i = 0;
            return CheckEnableKey(Input.GetKeyDown,KeyCode.Q);
            // return Input.GetKeyDown(KeyCode.Q);
        }
        
        public static bool IsUiRotateRightPressed(out int i)
        {
            i = 0;
            return CheckEnableKey(Input.GetKeyDown,KeyCode.E);
            // return Input.GetKeyDown(KeyCode.E);
        }

        public static bool IsPauseResumePressed(out int i)
        {
            i = 0;
            return CheckEnableKey(Input.GetKeyDown,KeyCode.Escape);
            // return Input.GetKeyDown(KeyCode.Escape);
        }

        public static bool AreControlsEnabled()
        {
            return _controlsEnabled;
        }
        
        public static void EnableControls(bool b)
        {
            _controlsEnabled = b;
        }
    }
}