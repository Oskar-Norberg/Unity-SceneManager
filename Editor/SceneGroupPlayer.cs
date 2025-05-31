using UnityEditor;
using UnityEngine;

namespace ringo.SceneSystem.Editor
{
    public class SceneGroupPlayer : EditorWindow
    {
        private SceneGroupSO _sceneGroupToPlay;
        
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
            
            _sceneGroupToPlay = (SceneGroupSO) EditorGUILayout.ObjectField("Scene Group", _sceneGroupToPlay, typeof(SceneGroupSO), false);
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
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod()
        {
            if (!ShouldOverrideScene())
                return;

            SetShouldOverrideScene(false);
            
            var sceneGroupPath = EditorPrefs.GetString("SceneGroupToPlay", string.Empty);
            var sceneGroupToPlay = default(SceneGroupSO);
            
            if (!string.IsNullOrEmpty(sceneGroupPath))
            {
                sceneGroupToPlay = AssetDatabase.LoadAssetAtPath<SceneGroupSO>(sceneGroupPath);
            }
            
            if (sceneGroupToPlay == null)
            {
                Debug.LogWarning("Scene Group is null");
                return;
            }
            
            LoadSceneGroup(sceneGroupToPlay);
        }

        private static async void LoadSceneGroup(SceneGroupSO sceneGroup)
        {
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