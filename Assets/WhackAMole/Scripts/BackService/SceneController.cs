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

        private Coroutine _coroutineLoad;
        private AsyncOperation _loadOp;
        private AsyncOperation _UnloadOp;
        private Action _onComplete;
        [SerializeField] private int _indexSceneLoaded = int.MinValue;
        private int _indexNextScene;

        public enum SceneType
        {
            MainMenu = 1,
            Game = 2,
        }

        private void Awake()
        {
            Instance = this;
        }

        public void LoadScene(SceneType scene, Action onComplete)
        {
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
            while (loadOP.progress < 1)
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
            while (loadOP.progress < 1)
            {
                yield return null;
            }
            _coroutineLoad = null;
            _indexSceneLoaded = int.MinValue;
            onComplete?.Invoke();
        }


        //private void Update()
        //{
        //    LoadOperationUpdate();
        //    UnloadOperationUpdate();
        //}

        ////change it to coroutine
        //private void LoadOperationUpdate()
        //{
        //    if (_loadOp == null) { return; }
        //    if (_loadOp.progress >= 1)
        //    {
        //        _indexSceneLoaded = _indexNextScene;

        //        _onComplete?.Invoke();
        //        _onComplete = null;
        //        _loadOp = null;
        //    }
        //}

        //private void UnloadOperationUpdate()
        //{
        //    if (_UnloadOp == null) { return; }
        //    if (_UnloadOp.progress >= 1)
        //    {
        //        _onComplete?.Invoke();
        //        _onComplete = null;
        //        _UnloadOp = null;
        //    }
        //}
    }
}