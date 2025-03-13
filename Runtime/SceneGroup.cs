using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;

namespace ringo.SceneSystem
{
    [CreateAssetMenu(fileName = "SceneGroup", menuName = "SceneManagement/SceneGroup")]
    public class SceneGroup : ScriptableObject
    {
        public List<SceneReference> Scenes = new();
        public SceneReference ActiveScene;
    }
}