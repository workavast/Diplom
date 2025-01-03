using System.Collections.Generic;

namespace App
{
    public static class ScenesData
    {
        public static readonly Dictionary<int, string> SceneNamesByIndexes = new()
        {
            { 0, "MainMenu" },
            { 1, "Gameplay" }
        };
        
        public static string NameByIndex(int sceneIndex)
        {
            if (!SceneNamesByIndexes.ContainsKey(sceneIndex))
                return default;

            return SceneNamesByIndexes[sceneIndex];
        }
    }
}