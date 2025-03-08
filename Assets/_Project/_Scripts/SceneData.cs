using System;
using Eflatun.SceneReference;

namespace ringo.SceneSystem
{
    [Serializable]
    public struct SceneData
    {
        public SceneReference Scene;

        public bool SetActive;
        public bool ReloadIfActive;
    }
}