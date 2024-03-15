using FootballExerciseApp.Models;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FootballExerciseApp.Services
{
    public class FootballDataService : IFootballDataService
    {
        private List<FootballMatch>? footballMatches;

        public async Task<FootballMatch?> GetMatch(int id) {
            if (footballMatches == null)
            {
                await FetchData();
            }
            return footballMatches?.FirstOrDefault(match => match.Id == id);
        }

        public async Task<List<FootballMatch>> GetMatches()
        {
            if (footballMatches == null)
            {
                await FetchData();
            }
            return footballMatches ?? new List<FootballMatch>();
        }

        private async Task FetchData()
        {
            /// <summary>
            /// Fetches football match data from blob storage or a local file (matches.json) if it exists
            /// </summary>

            // Attempt reading local file first
            string path = Path.Combine(Directory.GetCurrentDirectory(), "matches.json");

            if (!File.Exists(path))
            {
                // TODO: Make this configurable?
                string dataUrl = "https://functionapp2018071101324.blob.core.windows.net/data/matches_latest.json";

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        using (HttpResponseMessage response = await client.GetAsync(dataUrl))
                        {
                            using (HttpContent content = response.Content)
                            {
                                string matchData = await content.ReadAsStringAsync();
                                if (matchData != null)
                                {
                                    // Make sure we save the file first for easier usage in the future
                                    // TODO: Dynamic update/refresh of file?
                                    File.WriteAllText(path, matchData);

                                    footballMatches = JsonConvert.DeserializeObject<List<FootballMatch>>(matchData) ?? new();
                                }
                                else
                                {
                                    throw new Exception("Unable to deserialize match data");
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
            else
            {
                string matchJson = File.ReadAllText(path);
                footballMatches = JsonConvert.DeserializeObject<List<FootballMatch>>(matchJson) ?? new();
            }
        }
    }
}
