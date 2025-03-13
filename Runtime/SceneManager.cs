using System.Collections.Generic;
using System.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ringo.SceneSystem
{
    public static class SceneManager
    {
        private static bool _isSceneLoading = false;

        public delegate void OnSceneLoadingStartedEventHandler();

        public static event OnSceneLoadingStartedEventHandler OnSceneLoadingStarted;

        public delegate void OnSceneLoadingFinishedEventHandler();

        public static event OnSceneLoadingFinishedEventHandler OnSceneLoadingFinished;

        public static async Task<bool> LoadSceneGroup(SceneGroup sceneGroup)
        {
            if (_isSceneLoading)
            {
                Debug.LogWarning("Scene group is already loading!");
                return false;
            }

            _isSceneLoading = true;
            OnSceneLoadingStarted?.Invoke();

            await LoadScenes(sceneGroup);

            _isSceneLoading = false;

            OnSceneLoadingFinished?.Invoke();
            return true;
        }

        private static async Task LoadScenes(SceneGroup sceneGroup)
        {
            Queue<SceneReference> sceneQueue = new();
            Queue<AsyncOperation> asyncOperationQueue = new();

            foreach (var sceneReference in sceneGroup.Scenes)
            {
                sceneQueue.Enqueue(sceneReference);
            }

            // Load Scenes and disable scene activation
            bool firstScene = true;
            while (sceneQueue.Count > 0)
            {
                SceneReference sceneReference = sceneQueue.Dequeue();

                LoadSceneMode mode = firstScene ? LoadSceneMode.Single : LoadSceneMode.Additive;
                AsyncOperation operation = await LoadScene(sceneReference, mode);

                operation.allowSceneActivation = false;
                asyncOperationQueue.Enqueue(operation);

                firstScene = false;
            }

            // Enable scene activation
            while (asyncOperationQueue.Count > 0)
            {
                asyncOperationQueue.Dequeue().allowSceneActivation = true;
            }
        }

        private static async Task<AsyncOperation> LoadScene(SceneReference sceneReference, LoadSceneMode loadSceneMode)
        {
            AsyncOperation operation =
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneReference.Name, loadSceneMode);

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            return operation;
        }
    }
}