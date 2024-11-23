using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WhackAMole
{
    public class MainController : MonoBehaviour
    {        
        public static bool IsConnectedToServer { get; private set; }
        [SerializeField] private SFSController _serverConnection;

        private void Awake()
        {
            _serverConnection.OnConnected = SFS_OnConnectedToServer;
            var initializes = GetComponentsInChildren<IInitialize>();       
            for (int i = 0; i < initializes.Length; i++)
            {
                initializes[i].Initialize();
            }
        }

        private void Start()
        {
            PanelFade.Instance.Show(true);
            PanelLoading.Instance.Show("Connecting to server...");
            SceneController.Instance.LoadScene(SceneController.SceneType.MainMenu, null);
        }

        private void SFS_OnConnectedToServer()
        {
            IsConnectedToServer = true;
            PanelLoading.Instance.Hide();
        }

        private void SFS_OnDisconnectedFromServer()
        {
            IsConnectedToServer = false;
        }
    }
}
