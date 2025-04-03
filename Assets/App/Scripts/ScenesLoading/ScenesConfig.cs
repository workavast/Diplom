using System.Collections.Generic;

namespace App.ScenesLoading
{
    public static class ScenesConfig
    {
        public const int MainMenuSceneIndex = 2;
        public const int GameplaySceneIndex = 3;
        public const int LobbySceneIndex = 4;
        public const int Coop = 5;
        
        public static readonly Dictionary<int, string> SceneNamesByIndexes = new()
        {
            { 0, "BootstrapScene" },
            { 1, "ScenesLoadingScene" },
            { MainMenuSceneIndex, "MainMenuScene" },
            { GameplaySceneIndex, "GameplayScene" },
            { LobbySceneIndex, "Lobby" },
            { Coop, "Coop"}
        };
        
        public static string NameByIndex(int sceneIndex)
        {
            if (!SceneNamesByIndexes.ContainsKey(sceneIndex))
                return default;

            return SceneNamesByIndexes[sceneIndex];
        }
    }
}