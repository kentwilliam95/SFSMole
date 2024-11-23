using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace WhackAMole
{
    public class PanelFade : MonoBehaviour
    {
        public static PanelFade Instance;

        [SerializeField] private CanvasGroup _canvasGroup;
        private Tween _tweenfade;

        private void Awake()
        {
            Instance = this;
        }

        public void Show(bool force = false, Action onComplete = null)
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            if (_tweenfade != null)
            {
                _tweenfade.Kill();
            }

            if (force)
            {
                _canvasGroup.alpha = 1f;
                onComplete?.Invoke();
                return;
            }

            _tweenfade = DOVirtual.Float(0f, 1f, 0.25f, (value) =>
            {
                _canvasGroup.alpha = value;
            }).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }

        public void Hide(bool force = false, Action onComplete = null)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            if (_tweenfade != null)
            {
                _tweenfade.Kill();
            }

            if (force)
            {
                _canvasGroup.alpha = 0f;
                onComplete?.Invoke();
                return;
            }

            _tweenfade = DOVirtual.Float(1f, 0f, 0.25f, (value) =>
            {
                _canvasGroup.alpha = value;
            }).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
    }
}
