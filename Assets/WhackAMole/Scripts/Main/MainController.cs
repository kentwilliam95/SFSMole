using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhackAMole
{
    public class MainController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SceneController.Instance.LoadScene(SceneController.SceneType.MainMenu, null);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SceneController.Instance.LoadScene(SceneController.SceneType.Game, null);
            }
        }
    }
}
