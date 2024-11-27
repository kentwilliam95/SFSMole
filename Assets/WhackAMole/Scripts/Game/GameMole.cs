using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace WhackAMole.Game
{
    public class GameMole : MonoBehaviour
    {
        private float _countdown;
        private Sequence _seqShow;
        [SerializeField] private Transform _body;
        [SerializeField] private AudioClip[] _audioclipShowUp;
        [SerializeField] private AudioClip[] _audioClipOuch;
        
        public bool IsShowing { get; private set; }

        public Vector3 TopPos => _body.transform.position + Vector3.up;
        private void Start()
        {
            _seqShow = DOTween.Sequence();
            _seqShow.SetAutoKill(false);

            _seqShow.Insert(0f, _body.DOLocalMove(Vector3.up, 0.3f).SetEase(Ease.OutBack));
            _seqShow.Pause();
        }

        private void Update()
        {
            if (!IsShowing)
            {
                return;
            }

            _countdown -= Time.deltaTime;
            if (_countdown <= 0)
            {
                Hide();
            }
        }

        public void Show(float howLong)
        {
            int length = _audioClipOuch.Length;
            int index = Random.Range(0, length); 
            AudioController.Instance.PlaySFX(_audioclipShowUp[index]);
            
            IsShowing = true;
            _countdown = howLong;

            _seqShow.timeScale = 1f;
            _seqShow.Restart();
        }

        public void Hit(bool withSound = true)
        {
            Hide(withSound);
        }

        private void Hide(bool withSound = true)
        {
            if (withSound)
            {
                int length = _audioClipOuch.Length;
                int index = Random.Range(0, length); 
                AudioController.Instance.PlaySFX(_audioClipOuch[index]);   
            }
            
            IsShowing = false;
            _seqShow.timeScale = 2f;
            _seqShow.SmoothRewind();
        }
    }
}