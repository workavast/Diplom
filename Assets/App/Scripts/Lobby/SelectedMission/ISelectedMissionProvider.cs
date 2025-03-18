using App.Missions;

namespace App.Lobby.SelectedMission
{
    public interface ISelectedMissionProvider
    {
        public int ActiveMissionIndex { get; }

        public MissionConfig GetMission(int activeMissionIndex);
    }
}