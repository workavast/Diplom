using UnityEngine;

namespace App.Lobby.StartGameTimer
{
    [CreateAssetMenu(fileName = nameof(StartGameTimerConfig), menuName = Consts.AppName + "/Configs/" + nameof(StartGameTimerConfig))]
    public class StartGameTimerConfig : ScriptableObject
    {
        [field: SerializeField, Min(0)] public float NonFullTeamStartTime { get; private set; } = 60;
        [field: SerializeField, Min(0)] public float FullTeamStartTime { get; private set; } = 5;
    }
}