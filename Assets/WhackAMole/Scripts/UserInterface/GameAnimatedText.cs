using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace WhackAMole
{
    public class GameAnimatedText : MonoBehaviour
    {
        private Sequence _seq;
        [SerializeField] private TextMeshProUGUI _text;
        private void OnDestroy()
        {
            _seq?.Kill();
        }

        void Start()
        {
            _seq = DOTween.Sequence();
            _seq.SetAutoKill(false);

            _seq.Insert(0f,_text.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
            _seq.Insert(0f, _text.DOFade(1f, 0.25f).From(0f));
            
            _seq.Insert(0.75f,_text.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBack));
            _seq.Insert(0.75f, _text.DOFade(0f, 0.25f).From(1f));
            _seq.Pause();
        }

        public void ShowText(int value)
        {
            _text.SetText(value.ToString());
            _seq.Restart();
        }

        public void ShowText(string value)
        {
            _text.SetText(value);
            _seq.Restart();
        }
    }
}
