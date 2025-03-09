using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ringo.SceneSystem.Editor
{
    public class SceneGroupPlayer : EditorWindow
    {
        private SceneGroup _sceneGroupToPlay;
        
        [MenuItem("Testing/Scene Group Player")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<SceneGroupPlayer>("Scene Group Player");
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
        
            SceneGroupField();
            
            PlaySceneGroupButton();
        
            EditorGUILayout.EndVertical();
        }
        
        private void SceneGroupField()
        {
            EditorGUILayout.BeginHorizontal();
            
            _sceneGroupToPlay = (SceneGroup) EditorGUILayout.ObjectField("Scene Group", _sceneGroupToPlay, typeof(SceneGroup), false);
            EditorPrefs.SetString("SceneGroupToPlay", AssetDatabase.GetAssetPath(_sceneGroupToPlay));
            
            EditorGUILayout.EndHorizontal();
        }

        private void PlaySceneGroupButton()
        {
            if (Application.isPlaying)
                return;
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Play Scene Group"))
                PlaySceneGroup();
            
            EditorGUILayout.EndHorizontal();
        }

        private void PlaySceneGroup()
        {
            if (_sceneGroupToPlay == null)
            {
                Debug.LogWarning("Scene Group is null");
                return;
            }
            
            EditorPrefs.SetBool("DoOverrideScene", true);
            EditorApplication.EnterPlaymode();
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod()
        {
            if (!ShouldOverrideScene())
                return;

            SetShouldOverrideScene(false);
            
            var sceneGroupPath = EditorPrefs.GetString("SceneGroupToPlay", string.Empty);
            var sceneGroupToPlay = default(SceneGroup);
            
            if (!string.IsNullOrEmpty(sceneGroupPath))
            {
                sceneGroupToPlay = AssetDatabase.LoadAssetAtPath<SceneGroup>(sceneGroupPath);
            }
            
            if (sceneGroupToPlay == null)
            {
                Debug.LogWarning("Scene Group is null");
                return;
            }
            
            LoadSceneGroup(sceneGroupToPlay);
        }

        private static async void LoadSceneGroup(SceneGroup sceneGroup)
        {
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneGroup.Scenes[0].Scene.Name);
            
            await SceneManager.LoadSceneGroup(sceneGroup);
        }

        private static bool ShouldOverrideScene()
        {
            return EditorPrefs.GetBool("DoOverrideScene", false);
        }
        
        private static void SetShouldOverrideScene(bool value)
        {
            EditorPrefs.SetBool("DoOverrideScene", value);
        }
    }
}