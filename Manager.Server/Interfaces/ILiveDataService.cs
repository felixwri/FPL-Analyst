using Manager.Server.Models;

namespace Manager.Server.Interfaces
{
    public interface ILiveDataService
    {
        Task<Dictionary<int, PlayerData>> GetLivePlayerData();
        Task<ManagerPicks> GetPicks(Team team);
    }
}