using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WhackAMole
{
    public class PanelLoading : MonoBehaviour,IInitialize
    {
        public static PanelLoading Instance { get; private set; }

        private Coroutine _loadingCoroutine;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectLoadingImage;
        [SerializeField] private TextMeshProUGUI _text;
        public void Initialize()
        {
            Instance = this;
            Show();
        }

        public void Update()
        {
            if (_canvasGroup.alpha <= 0) { return; }
            _rectLoadingImage.transform.Rotate(Vector3.forward, Time.deltaTime * 180f);    
        }

        public void Show(string message = "Loading...", float delay = 0.25f)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;

            //if (_loadingCoroutine != null)
            //{
            //    StopCoroutine(_loadingCoroutine);
            //}

            //_loadingCoroutine = StartCoroutine(Delay(delay, () => 
            //{
            //    Hide();
            //}));

            _text.SetText(message);
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }

        private IEnumerator Delay(float delay, Action onComplete)
        {
            yield return new WaitForSeconds(delay);
            onComplete?.Invoke();
        }
    }
}
