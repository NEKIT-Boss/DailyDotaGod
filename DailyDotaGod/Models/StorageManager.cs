using DailyDotaGod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using DailyDotaGod.Data;
using DailyDotaGod.Models.DailyDotaProxy;
using System.Diagnostics;
using Newtonsoft.Json;

namespace DailyDotaGod.Models
{
    class StorageManager
    {
        private static readonly StorageManager _instance = new StorageManager();
        public static StorageManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private StorageManager()
        {
            
        }

        /// <summary>
        /// For now, we will try without interface
        /// Then just consider it, bc it makes sense, but I don't know
        /// Checks the team from DailyDota API, if it already is in database
        /// </summary>
        /// <param name="loadedTeam">The Json team to check</param>
        /// <returns>Whether or not team passed exists already in the storage</returns>
        public bool TeamExists(DailyDotaProxy.Team loadedTeam)
        {
            using (var context = new StorageContext())
            {
                var stored_team = context.Teams.FirstOrDefault((team) =>
                    (loadedTeam.Name == team.Name && loadedTeam.Tag == team.Tag)
                );

                return stored_team != default(Data.Team);
            }
        }

        private async Task<TeamImage> LoadTeamImage(DailyDotaProxy.Team team)
        {
            return await Task.Run(async () =>
            {
                byte[] rawData = await team.LogoUrl.DownloadRaw();

                return new TeamImage
                {
                    Data = rawData,
                };
            });
        }

        //Load Those CountryImages, actually outside teams maybe
        //During adding the teams, it is known that obly new teams, so could do some
        // Iteration find new coutry images

        /// <summary>
        /// Adds the team acquired from DailyDota API 
        /// </summary>
        /// <param name="loadedTeam">The team to add to database</param>
        /// <returns>Whether or not, the operation was successful</returns>
        public async Task<bool> StoreTeams( List<DailyDotaProxy.Team> loadedTeams )
        {
            try
            {
                using (var context = new StorageContext())
                {
                    Task<TeamImage>[] logoTasks = new Task<TeamImage>[loadedTeams.Count];
                    for (int i = 0; i < loadedTeams.Count; i++)
                    {
                        logoTasks[i] = LoadTeamImage(loadedTeams[i]);
                    }
                    await Task.WhenAll(logoTasks);

                    TeamImage[] logos = logoTasks.Select(logoTask => logoTask.Result).ToArray();
                    context.TeamImages.AddRange(logos);
                    await context.SaveChangesAsync();

                    List<Data.Team> teams = new List<Data.Team>();
                    foreach (var teamLogoTuple in loadedTeams.Zip(logos, (team, logo) => Tuple.Create(team, logo)))
                    {
                        teams.Add(new Data.Team()
                        {
                            Name = teamLogoTuple.Item1.Name,
                            Tag = teamLogoTuple.Item1.Tag,
                            Logo = teamLogoTuple.Item2,
                            CountryLogo = null
                        });
                    }
                    context.Teams.AddRange(teams);

                    await context.SaveChangesAsync();
                    Debug.WriteLine($"Finished!");
                    return true;
                }
            }

            catch
            {
                return false;
            }
        }

    }
}
