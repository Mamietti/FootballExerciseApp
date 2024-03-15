namespace FootballExerciseApp.Models
{
    public class FootballMatch
    {
        public int Id { get; set; }
        public DateTime MatchDate { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public FootballTeam? HomeTeam { get; set; }
        public FootballTeam? AwayTeam { get; set; }

    }
}
