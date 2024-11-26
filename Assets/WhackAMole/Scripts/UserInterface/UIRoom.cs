using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEditor;
using UnityEngine;

namespace WhackAMole
{
    public class UIRoom : UIBase
    {
        public struct UserListData
        {
            public string Username;
            public int Id;
        }
        [SerializeField] private TextMeshProUGUI _textPrefab;
        [SerializeField] private RectTransform _rtTextContainer;

        [SerializeField] private TextMeshProUGUI _textRoomSize;
        [SerializeField] private TextMeshProUGUI _textRoomName;

        public Action OnSemiOfflineGame_Clicked;
        
        public void SetupUsers(in UserListData[] data)
        {
            var count = _rtTextContainer.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                Destroy(_rtTextContainer.GetChild(i).gameObject);
            }
            
            for (int i = 0; i < data.Length; i++)
            {
                var d = data[i];
                var text = Instantiate(_textPrefab, _rtTextContainer);
                
                text.SetText($"{d.Id}\t\t{d.Username}");
                text.gameObject.SetActive(true);
            }

            var client = SFSController.Instance.Client;
            var room = client.LastJoinedRoom;
            _textRoomName.SetText($"Room {room.Name}");
            _textRoomSize.SetText($"{data.Length} / {client.LastJoinedRoom.Capacity}");
        }

        public void ButtonSemiOfflineGame_OnClicked()
        {
            OnSemiOfflineGame_Clicked?.Invoke();
        }
    }
}
