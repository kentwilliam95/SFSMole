using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WhackAMole
{
    public class UILogin : MonoBehaviour
    {
        [SerializeField] private Button _buttonGuestLogin;
        [SerializeField] private Button _buttonLogin;

        [SerializeField] private TMP_InputField _textUsername;
        [SerializeField] private TMP_InputField _textPassword;
        
        private void Start()
        {
            _textUsername.text = PlayerPrefs.GetString(SFSController.USERNAMEKEY);
            _buttonGuestLogin.onClick.AddListener(ButtonGuestLogin_OnClicked);     
        }

        private void ButtonGuestLogin_OnClicked()
        {
            SFSController.Instance.Login(_textUsername.text, _textPassword.text, () => 
            {
                Debug.Log("Login Success!");
            });
        }
    }
}
