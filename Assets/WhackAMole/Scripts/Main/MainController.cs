using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WhackAMole
{
    public class MainController : MonoBehaviour
    {        

        private void Awake()
        {
            var initializes = GetComponentsInChildren<IInitialize>();       
            for (int i = 0; i < initializes.Length; i++)
            {
                initializes[i].Initialize();
            }
        }

        private void Start()
        {
            PanelFade.Instance.Show(true);
            SceneController.Instance.LoadScene(SceneController.SceneType.MainMenu, null);
        }
    }
}
