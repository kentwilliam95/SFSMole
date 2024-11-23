using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WhackAMole
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance { get; private set; }
        public enum SceneType
        {
            MainMenu = 1,
            Game = 2,
        }

        private Coroutine _coroutineLoad;
        [SerializeField] private int _indexSceneLoaded = int.MinValue;

        private void Awake()
        {
            Instance = this;
        }

        public void LoadScene(SceneType scene, Action onComplete)
        {
            Debug.Log("Load Scene");
            if (_coroutineLoad != null)
            {
                Debug.Log($"{GetType()}: Trying is trying to load next scene!");
                return;
            }

            int sceneIndex = (int)scene;
            UnloadScene(_indexSceneLoaded, () =>
            {
                LoadNextScene(sceneIndex, onComplete);
            });
        }

        private void LoadNextScene(int nextScene, Action onComplete)
        {
            _coroutineLoad = StartCoroutine(LoadNextScene_C(nextScene, onComplete)); 
        }

        private void UnloadScene(int sceneIndex, Action onComplete)
        {
            if (_indexSceneLoaded == int.MinValue)
            {
                onComplete?.Invoke();
                return;
            }
            
            _coroutineLoad = StartCoroutine(UnloadScene_C(sceneIndex, onComplete));
        }

        private IEnumerator LoadNextScene_C(int sceneIndex, Action onComplete)
        {
            var loadOP = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            loadOP.allowSceneActivation = true;
            while (loadOP.progress < 1 || !loadOP.isDone)
            {
                yield return null;
            }            
            _coroutineLoad = null;
            _indexSceneLoaded = sceneIndex;
            onComplete?.Invoke();
        }

        private IEnumerator UnloadScene_C(int sceneIndex, Action onComplete)
        {
            var loadOP = SceneManager.UnloadSceneAsync(sceneIndex);
            while (loadOP.progress < 1 || !loadOP.isDone)
            {
                yield return null;
            }
            _coroutineLoad = null;
            _indexSceneLoaded = int.MinValue;
            onComplete?.Invoke();
        }
    }
}