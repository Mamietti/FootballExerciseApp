using FootballExerciseApp.Models;

namespace FootballExerciseApp.Services
{
    public interface IFootballDataService
    {
        public Task<List<FootballMatch>> GetMatches();
        public Task<FootballMatch?> GetMatch(int id);
    }
}
