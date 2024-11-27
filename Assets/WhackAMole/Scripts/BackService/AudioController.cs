using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhackAMole
{
    public class AudioController : MonoBehaviour, IInitialize
    {
        public static AudioController Instance { get; private set; }
        [SerializeField] private AudioSource _audioSFX;
        [SerializeField] private AudioSource _audioBGM;
        [SerializeField] private SFXData _sfxData;
        public void Initialize()
        {
            Instance = this;
            _sfxData.Init();
        }

        public void PlaySFX(SFXData.ID id)
        {
            var clip = _sfxData.GetAudio(id);
            if (clip == null) { return;}
            _audioSFX.PlayOneShot(clip);
        }

        public void PlaySFX(AudioClip clip, float volume = 0.75f)
        {
            _audioSFX.PlayOneShot(clip, volume);
        }
    }
}
