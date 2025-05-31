using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ringo.SceneSystem
{
    public static class SceneManager
    {
        public static SceneGroup CurrentSceneGroup => _currentSceneGroup;
        
        private static bool _isSceneLoading = false;

        public delegate void OnSceneLoadingStartedEventHandler();
        public static event OnSceneLoadingStartedEventHandler OnSceneLoadingStarted;
        
        public delegate void OnSceneLoadingFinishedEventHandler();
        public static event OnSceneLoadingFinishedEventHandler OnSceneLoadingFinished;
        
        private static SceneGroup _currentSceneGroup;
        
        public static async Task<bool> LoadSceneGroup(SceneGroupSO sceneGroupSO)
        {
            if (sceneGroupSO == null)
            {
                Debug.LogError("SceneGroupSO is null!");
                return false;
            }
            
            return await LoadSceneGroup(sceneGroupSO.SceneGroup);
        }
        
        public static async Task<bool> LoadSceneGroup(SceneGroup sceneGroup)
        {
            if (_isSceneLoading)
            {
                Debug.LogWarning("Scene group is already loading!");
                return false;
            }
            
            _currentSceneGroup = sceneGroup;
            _isSceneLoading = true;
            OnSceneLoadingStarted?.Invoke();
            
            List<SceneData> sceneDatas = new(sceneGroup.Scenes);
            
            bool leftOverScene = await UnloadScenes(sceneDatas);
            
            if (leftOverScene)
                await LoadFirstSceneSingle(sceneDatas);

            await LoadScenes(sceneGroup);
            
            _isSceneLoading = false;
            
            OnSceneLoadingFinished?.Invoke();
            return true;
        }

        /**
         * <summary>Unloads scenes not in sceneDatas</summary>
         * <returns>Whether there is any left-over scene.</returns>
         */
        private static async Task<bool> UnloadScenes(List<SceneData> sceneDatas)
        {
            List<string> currentSceneNames = GetCurrentSceneNames();

            foreach (var sceneName in currentSceneNames)
            {
                var sceneData = sceneDatas.Find(sd => sd.Scene.Name == sceneName);
                
                if (UnityEngine.SceneManagement.SceneManager.sceneCount == 1)
                    return true;

                if (sceneData.Scene == null)
                    await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
                else if (sceneData.ReloadIfActive)
                    await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
            }

            return false;
        }
        
        private static async Task LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        }

        private static async Task LoadFirstSceneSingle(List<SceneData> sceneDatas)
        {
            await LoadScene(sceneDatas[0].Scene.Name, LoadSceneMode.Single);
            sceneDatas.RemoveAt(0);
        }
        
        private static async Task LoadScenes(SceneGroup sceneGroup)
        {
            List<string> currentSceneNames = GetCurrentSceneNames();
            
            foreach (var sceneData in sceneGroup.Scenes)
            {
                bool sceneActive = currentSceneNames.Contains(sceneData.Scene.Name);
                if (sceneActive)
                    continue;
                
                await LoadScene(sceneData.Scene.Name, LoadSceneMode.Additive);

                if (sceneData.SetActive)
                {
                    Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneData.Scene.Name);
                    UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
                }
            }
        }
        
        private static List<string> GetCurrentSceneNames()
        {
            List<string> sceneNames = new List<string>();
            int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                sceneNames.Add(UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name);
            }

            return sceneNames;
        }
    }
}