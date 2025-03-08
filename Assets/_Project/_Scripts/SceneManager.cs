using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ringo.SceneSystem
{
    public static class SceneManager
    {
        public static async void LoadSceneGroup(SceneGroup sceneGroup)
        {
            // TODO: It seems the first loaded scene is not unloaded. Fix.
            await UnloadScenes(sceneGroup);
            await LoadScenesInGroup(sceneGroup);
        }

        private static async Task UnloadScenes(SceneGroup newSceneGroup)
        {
            List<string> currentSceneNames = GetCurrentSceneNames();
            
            // loop through all old scenes, if it's not in the new scene list and not marked as ReloadIfActive, unload it
            foreach (var scene in currentSceneNames)
            {
                var sceneData = newSceneGroup.Scenes.Find(x => x.Scene.Name == scene);
                
                if (sceneData.Scene == null || sceneData.ReloadIfActive)
                    await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
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

        private static async Task LoadScenesInGroup(SceneGroup sceneGroup)
        {
            foreach (var sceneData in sceneGroup.Scenes)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneData.Scene.Name).isLoaded)
                    continue;
                
                await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneData.Scene.Name, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                
                if (sceneData.SetActive)
                    UnityEngine.SceneManagement.SceneManager.SetActiveScene(sceneData.Scene.LoadedScene);
            }
        }
    }
}