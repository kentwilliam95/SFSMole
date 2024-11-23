using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WhackAMole
{
    public class MainController : MonoBehaviour
    {
        private void Start()
        {
            PanelFade.Instance.Show(true);
            SceneController.Instance.LoadScene(SceneController.SceneType.MainMenu, null);
        }
    }
}
