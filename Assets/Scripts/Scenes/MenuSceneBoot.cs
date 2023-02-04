using System.Collections;
using Audio;
using UI.Menus;
using UnityEngine;

namespace Scenes
{
    public class MenuSceneBoot : MonoBehaviour
    {
        private void Start()
        {
            MainMenu.Show();
            AudioController.Get().PlaySoundtrack(PlaylistId.PL_ST_MENU,false,1f);
            AudioController.Get().PlayAmbience(PlaylistId.PL_AM_MENU,false,1f);
        }
        
    }
}