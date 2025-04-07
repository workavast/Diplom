using System.Collections.Generic;

namespace App.ScenesLoading
{
    public static class ScenesConfig
    {
        public const int BootstrapSceneIndex = 0;
        public const int ScenesLoadingSceneIndex = 1;
        public const int MainMenuSceneIndex = 2;
        public const int GameplaySceneIndex = 3;
        public const int LobbySceneIndex = 4;
        public const int CoopSceneIndex = 5;
        public const int TrainSceneIndex = 6;

        public static readonly Dictionary<int, string> SceneNamesByIndexes = new()
        {
            { BootstrapSceneIndex, "BootstrapScene" },
            { ScenesLoadingSceneIndex, "ScenesLoadingScene" },
            { MainMenuSceneIndex, "MainMenuScene" },
            { GameplaySceneIndex, "GameplayScene" },
            { LobbySceneIndex, "Lobby" },
            { CoopSceneIndex, "Coop" },
            { TrainSceneIndex, "Train" }
        };

        public static string NameByIndex(int sceneIndex)
        {
            if (!SceneNamesByIndexes.ContainsKey(sceneIndex))
                return default;

            return SceneNamesByIndexes[sceneIndex];
        }
        
        public static int GetIndex(this SceneType scene) 
            => (int)scene;
    }

    public enum SceneType : int
    {
        BootstrapScene = ScenesConfig.BootstrapSceneIndex,
        ScenesLoadingScene = ScenesConfig.ScenesLoadingSceneIndex,
        MainMenuScene = ScenesConfig.MainMenuSceneIndex,
        GameplayScene = ScenesConfig.GameplaySceneIndex,
        Lobby = ScenesConfig.LobbySceneIndex,
        Coop = ScenesConfig.CoopSceneIndex,
        Train = ScenesConfig.TrainSceneIndex
    }
}