using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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
            
            List<SceneData> sceneDatas = new(sceneGroup.Scenes);
            
            bool leftOverScene = await UnloadScenes(sceneDatas);
            await LoadScenesInGroup(sceneDatas);
            
            if (leftOverScene)
                await UnloadLeftOverScenes(sceneDatas);

            _isSceneLoading = false;
            
            OnSceneLoadingFinished?.Invoke();
            return true;
        }

        /**
         * <summary>Unloads scenes not in sceneData list.</summary>
         * <returns>Returns whether there were any left-over scenes, because unity always needs to have at least one scene loaded at all times.</returns>
         */
        private static async Task<bool> UnloadScenes(List<SceneData> sceneDatas)
        {
            List<string> currentSceneNames = GetCurrentSceneNames();
            
            // loop through all old scenes, if it's not in the new scene list and not marked as ReloadIfActive, unload it
            foreach (var scene in currentSceneNames)
            {
                var sceneData = sceneDatas.Find(x => x.Scene.Name == scene);

                if (UnityEngine.SceneManagement.SceneManager.sceneCount == 1)
                    return true;
                
                if (sceneData.Scene == null || sceneData.ReloadIfActive)
                    await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
            }

            return false;
        }

        private static async Task LoadScenesInGroup(List<SceneData> sceneDatas)
        {
            // Load all scenes from SceneGroup
            foreach (var sceneData in sceneDatas)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneData.Scene.Name).isLoaded)
                    continue;
                
                await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneData.Scene.Name, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                
                if (sceneData.SetActive)
                    UnityEngine.SceneManagement.SceneManager.SetActiveScene(sceneData.Scene.LoadedScene);
            }
        }

        private static async Task UnloadLeftOverScenes(List<SceneData> sceneDatas)
        {
            // Remove if there is any scene that shouldn't be loaded.
            foreach (var sceneName in GetCurrentSceneNames())
            {
                if (sceneDatas.Find(x => x.Scene.Name == sceneName).Scene == null)
                    await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
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