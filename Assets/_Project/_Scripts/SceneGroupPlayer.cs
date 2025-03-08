using UnityEditor;
using UnityEngine;


namespace ringo.SceneSystem.Editor
{
    public class SceneGroupPlayer : EditorWindow
    {
        private static SceneGroup _sceneGroupToPlay;
        
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
            
            EditorApplication.EnterPlaymode();
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod()
        {
            var sceneGroupPath = EditorPrefs.GetString("SceneGroupToPlay", string.Empty);
            
            if (!string.IsNullOrEmpty(sceneGroupPath))
            {
                _sceneGroupToPlay = AssetDatabase.LoadAssetAtPath<SceneGroup>(sceneGroupPath);
            }
            
            if (_sceneGroupToPlay == null)
            {
                Debug.LogWarning("Scene Group is null");
                return;
            }

            SceneManager.LoadSceneGroup(_sceneGroupToPlay);
        }
    }
}