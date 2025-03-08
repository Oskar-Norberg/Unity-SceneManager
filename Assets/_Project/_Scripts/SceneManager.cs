using System.Collections.Generic;
using ringo.SceneSystem.Helpers;
using UnityEngine;

namespace ringo.SceneSystem
{
    public class SceneManager : Singleton<SceneManager>
    {
        [SerializeField] private SceneGroup initialSceneGroup;
        
        private SceneGroup _currentSceneGroup;

        protected override void Awake()
        {
            base.Awake();
            
            if (initialSceneGroup == null)
            {
                Debug.LogError("Initial Scene Group is not set in SceneManager");
                return;
            }
            
            LoadScene(initialSceneGroup);
        }
        
        public void LoadScene(SceneGroup sceneGroup)
        {
            UnloadScenes(sceneGroup);
            LoadSceneGroup(sceneGroup);

            _currentSceneGroup = sceneGroup;
        }

        private void UnloadScenes(SceneGroup newSceneGroup)
        {
            List<string> currentSceneNames = GetCurrentSceneNames();
            
            // loop through all old scenes, if it's not in the new scene list and not marked as ReloadIfActive, unload it

            foreach (var scene in currentSceneNames)
            {
                var sceneData = newSceneGroup.Scenes.Find(x => x.Scene.Name == scene);
                
                if (sceneData is { ReloadIfActive: true })
                {
                    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
                }
            }
        }

        private List<string> GetCurrentSceneNames()
        {
            List<string> sceneNames = new List<string>();
            int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                sceneNames.Add(UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name);
            }

            return sceneNames;
        }

        private void LoadSceneGroup(SceneGroup sceneGroup)
        {
            foreach (var sceneData in sceneGroup.Scenes)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneData.Scene.Name, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                
                if (sceneData.SetActive)
                    UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneData.Scene.Name));
            }
        }
    }
}