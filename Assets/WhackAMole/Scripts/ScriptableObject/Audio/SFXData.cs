using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace WhackAMole
{
    [CreateAssetMenu(menuName = "Audio/SFX", fileName = "SFX")]
    public class SFXData : ScriptableObject
    {
        public enum ID
        {
            GameEnd,
            GameStart,
            Button,
            Win,
            Lose,
            Countdown,
            PositiveDing,
            NegativeDing
        }
        
        [System.Serializable]
        public struct Data
        {
            public ID Id; 
            public AudioClip AudioClip;
        }

        
        private Dictionary<ID, AudioClip> _dictAudio;

        public Data[] _data;
        public void Init()
        {
            _dictAudio = new Dictionary<ID, AudioClip>();
            for (int i = 0; i < _data.Length; i++)
            {
                _dictAudio.TryAdd(_data[i].Id, _data[i].AudioClip);
            }
        }

        public AudioClip GetAudio(ID id)
        {
            _dictAudio.TryGetValue(id, out AudioClip clip);
            return clip;
        }
    }
}
