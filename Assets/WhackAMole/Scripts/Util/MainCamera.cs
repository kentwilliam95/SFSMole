using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhackAMole
{
    public class MainCamera : MonoBehaviour, IInitialize
    {
        public static MainCamera Instance { get; private set; }
        [SerializeField] public Camera Cam { get; private set; }

        public void Initialize()
        {
            Instance = this;

            if (Cam == null)
            {
                Cam = GetComponent<Camera>();
            }
        }
    }
}