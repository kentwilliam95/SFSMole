using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhackAMole
{
    public class GameController : MonoBehaviour
    {
        private void Start()
        {
            PanelFade.Instance.Hide();
        }
    }
}
