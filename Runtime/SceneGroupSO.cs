using UnityEngine;

namespace ringo.SceneSystem
{
    [CreateAssetMenu(fileName = "SceneGroup", menuName = "SceneManagement/SceneGroup")]
    public class SceneGroupSO : ScriptableObject
    {
        public SceneGroup SceneGroup;
    }
}