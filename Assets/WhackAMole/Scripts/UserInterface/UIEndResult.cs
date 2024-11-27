using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WhackAMole
{
    public class UIEndResult : UIBase
    {
        [SerializeField] private TextMeshProUGUI _text;
        public Action OnButtonClick;
        
        public void Show(String text)
        {
            _text.SetText(text);
            base.Show();
        }

        public void Button_OnClicked()
        {
            OnButtonClick?.Invoke();    
        }
        
    }
}
