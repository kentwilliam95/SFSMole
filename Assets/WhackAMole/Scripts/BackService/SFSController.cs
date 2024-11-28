using PlasticGui.WorkspaceWindow.PendingChanges;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Logging;
using Sfs2X.Requests;
using Sfs2X.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhackAMole
{
    public class SFSController : MonoBehaviour, IInitialize
    {
        public const string USERNAMEKEY = "Username";
        public static SFSController Instance { get; private set; }
        
        private Action _onLoginSuccess;
        private SmartFox _sfs;
        public SmartFox Client => _sfs;

        [Header("Config")]
        [Header("SFS2X connection settings")]

        [Tooltip("IP address or domain name of the SmartFoxServer instance; if encryption is enabled, a domain name must be entered")]
        public string host = "127.0.0.1";

        [Tooltip("TCP listening port of the SmartFoxServer instance, used for TCP socket connection in all builds except WebGL")]
        public int tcpPort = 9933;

        [Tooltip("HTTP listening port of the SmartFoxServer instance, used for WebSocket (WS) connections in WebGL build")]
        public int httpPort = 8080;

        [Tooltip("HTTPS listening port of the SmartFoxServer instance, used for WebSocket Secure (WSS) connections in WebGL build and connection encryption in all other builds")]
        public int httpsPort = 8443;

        [Tooltip("Use SmartFoxServer's HTTP tunneling (BlueBox) if TCP socket connection can't be established; not available in WebGL builds")]
        public bool useHttpTunnel = false;

        [Tooltip("Enable SmartFoxServer protocol encryption; 'host' must be a domain name and an SSL certificate must have been deployed")]
        public bool encrypt = false;

        [Tooltip("Name of the SmartFoxServer Zone to join")]
        public string zone = "BasicExamples";

        [Tooltip("Display SmartFoxServer client debug messages")]
        public bool debug = false;

        [Tooltip("Client-side SmartFoxServer logging level")]
        public LogLevel logLevel = LogLevel.INFO;

        public Action OnConnected;
        public Action OnDisconnected;

        public void Initialize()
        {
            Instance = this;
            Application.runInBackground = true;
#if !UNITY_WEBGL
            _sfs = new SmartFox();
#else
            _sfs = new SmartFox(encrypt ? UseWebSocket.WSS_BIN : UseWebSocket.WS_BIN);
#endif
            _sfs.EnableLagMonitor(true);
            _sfs.AddEventListener(SFSEvent.CONNECTION, SFS_OnConnect);
            _sfs.AddEventListener(SFSEvent.CONNECTION_LOST, SFS_OnLostConnection);
            _sfs.AddEventListener(SFSEvent.CRYPTO_INIT, SFS_OnCryptoInitialized);
            _sfs.AddEventListener(SFSEvent.LOGIN_ERROR, SFS_OnLoginError);
            _sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, SFS_OnExtensionResponse);
         

            _sfs.Logger.EnableConsoleTrace = true;
            _sfs.Logger.LoggingLevel = logLevel;

            Connect();
        }

        public void Connect()
        {
            ConfigData config = new ConfigData();
            config.Zone = zone;
            config.Host = host;
            config.Port = tcpPort;

            config.HttpPort = httpPort;
            config.HttpsPort = httpsPort;

            config.BlueBox.IsActive = useHttpTunnel;
            config.BlueBox.UseHttps = encrypt;

#if UNITY_WEBGL
		cfg.Port = encrypt ? httpsPort : httpPort;
#endif
            Debug.Log("Connecting");
            _sfs.Connect(config);
        }

        private void Update()
        {
            // Process the SmartFox events queue
            if (_sfs != null)
                _sfs.ProcessEvents();
        }

        public void Login(string username, string password, Action onLoginSuccess)
        {
            Debug.Log("Performing login...");
            _sfs.Send(new LoginRequest(username));
            _onLoginSuccess = onLoginSuccess;

            PlayerPrefs.SetString(USERNAMEKEY, username);
        }

        private void OnApplicationQuit()
        {
            if (_sfs != null && _sfs.IsConnected)
                _sfs.Disconnect();
        }

        #region sfs callbacks
        private void SFS_OnConnect(BaseEvent evt)
        {
            Debug.Log($"[SFS] Connect!");
            if ((bool)evt.Params["success"])
            {
                Debug.Log("Connection established successfully");
                Debug.Log("SFS2X API version: " + _sfs.Version);
                Debug.Log("Connection mode is: " + _sfs.ConnectionMode);

                OnConnected?.Invoke();
                if (encrypt)
                {
                    Debug.Log("Initializing encryption...");
                    _sfs.InitCrypto();
                }
            }
            else
            {
                Debug.Log("Connection failed; is the server running at all?");
            }
        }

        private void SFS_OnLostConnection(BaseEvent evt)
        {
            _sfs.RemoveEventListener(SFSEvent.CONNECTION, SFS_OnConnect);
            _sfs.RemoveEventListener(SFSEvent.CONNECTION_LOST, SFS_OnLostConnection);
            _sfs.RemoveEventListener(SFSEvent.CRYPTO_INIT, SFS_OnCryptoInitialized);
            _sfs.RemoveEventListener(SFSEvent.LOGIN_ERROR, SFS_OnLoginError);
            _sfs.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, SFS_OnExtensionResponse);

            _sfs = null;


            string reason = (string)evt.Params["reason"];
            Debug.Log("Connection to SmartFoxServer lost; reason is: " + reason);

            if (reason != ClientDisconnectionReason.MANUAL)
            {
                // Show error message
                string connLostMsg = "An unexpected disconnection occurred; ";

                if (reason == ClientDisconnectionReason.IDLE)
                    connLostMsg += "you have been idle for too much time";
                else if (reason == ClientDisconnectionReason.KICK)
                    connLostMsg += "you have been kicked";
                else if (reason == ClientDisconnectionReason.BAN)
                    connLostMsg += "you have been banned";
                else
                    connLostMsg += "reason is unknown.";

                Debug.Log(connLostMsg);
            }

            OnDisconnected?.Invoke();
        }

        private void SFS_OnCryptoInitialized(BaseEvent evt)
        {
            if ((bool)evt.Params["success"])
            {
                Debug.Log("Encryption initialized successfully");
            }
            else
            {
                Debug.Log("Encryption initialization failed: " + (string)evt.Params["errorMessage"]);
                _sfs.Disconnect();
            }
        }

        private void SFS_OnLoginError(BaseEvent evt)
        {
            Debug.Log($"[SFS] Login Error: {(string)evt.Params["errorMessage"]}");

            // Disconnect
            // NOTE: this causes a CONNECTION_LOST event with reason "manual", which in turn removes all SFS listeners
            _sfs.Disconnect();
        }

        private void SFS_OnExtensionResponse(BaseEvent evt)
        {
            //var cmd = evt.Params["cmd"];
            //Debug.Log(cmd);

            //switch (cmd)
            //{
               
            //}
        }
        #endregion
    }
}
