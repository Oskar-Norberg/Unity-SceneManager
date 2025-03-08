using System.Collections.Generic;
using UnityEngine;

namespace ringo.SceneSystem
{
    [CreateAssetMenu(fileName = "SceneGroup", menuName = "SceneManagement/SceneGroup")]
    public class SceneGroup : ScriptableObject
    {
        public List<SceneData> Scenes = new();
    }
}