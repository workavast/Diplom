using System.Collections.Generic;

namespace App.ScenesLoading
{
    public static class ScenesConfig
    {
        public const int GameplaySceneIndex = 3;
        
        public static readonly Dictionary<int, string> SceneNamesByIndexes = new()
        {
            { 0, "BootstrapScene" },
            { 1, "ScenesLoadingScene" },
            { 2, "MainMenuScene" },
            { GameplaySceneIndex, "GameplayScene" }
        };
        
        public static string NameByIndex(int sceneIndex)
        {
            if (!SceneNamesByIndexes.ContainsKey(sceneIndex))
                return default;

            return SceneNamesByIndexes[sceneIndex];
        }
    }
}