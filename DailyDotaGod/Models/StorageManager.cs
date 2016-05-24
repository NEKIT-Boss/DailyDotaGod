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
using System.Collections.ObjectModel;
using Microsoft.Data.Entity;
using DailyDotaGod.ViewModels;

namespace DailyDotaGod.Models
{
    class StorageManager
    {

        #region Singletone
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
        #endregion

        public ObservableCollection<TeamViewModel> Teams { get; set; } = new ObservableCollection<TeamViewModel>();

        public async Task<bool> SyncWithStorage()
        {
            using (var context = new StorageContext())
            {
                try
                {
                    if (Teams.Count == 0)
                    {
                        var teams = await context.Teams.ToListAsync();
                        foreach (var team in teams)
                        {
                            Teams.Add(new TeamViewModel(team));
                        }
                    }

                    return true;
                }

                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return false;
                }
            }
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
                var storedTeam = context.Teams.FirstOrDefault((team) =>
                    (loadedTeam.Name == team.Name && loadedTeam.Tag == team.Tag)
                );

                return storedTeam != default(Data.Team);
            }
        }

        private bool CountryImageExists(string countryCode)
        {
            using (var context = new StorageContext())
            {
                var storedLogo = context.CountryImages.FirstOrDefault((countryImage) => 
                    countryImage.Code ==  countryCode 
                );

                return storedLogo != default(CountryImage);
            }
        }

        private async Task<TeamImage> LoadTeamImageAsync(Uri logoUri)
        {
            return await Task.Run(async () =>
            {
                byte[] rawData = await logoUri.DownloadRaw();

                return new TeamImage
                {
                    Data = rawData,
                };
            });
        }

        private async Task<CountryImage> LoadCountryImageAsync(Uri logoUri, string countryCode)
        {
            return await Task.Run(async () =>
            {
                byte[] rawData = await logoUri.DownloadRaw();

                return new CountryImage
                {
                    Data = rawData,
                    Code = countryCode
                };
            });
        }

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
                    
                    List<Task<TeamImage>> teamImagesTasks = new List<Task<TeamImage>>();
                    List<Task<CountryImage>> countryImagesTasks = new List<Task<CountryImage>>();
                    foreach (var loadedTeam in loadedTeams)
                    {
                        teamImagesTasks.Add( LoadTeamImageAsync(loadedTeam.LogoUrl) );
                        if ( (loadedTeam.CountryCode != "") && (!CountryImageExists(loadedTeam.CountryCode)) )
                        {
                            countryImagesTasks.Add( LoadCountryImageAsync(loadedTeam.CountryLogoUrl, loadedTeam.CountryCode) );
                        }
                    }
                    await Task.WhenAll(countryImagesTasks);
                    await Task.WhenAll(teamImagesTasks);

                    TeamImage[] teamImages = teamImagesTasks.Select(teamImageTask => teamImageTask.Result).ToArray();
                    context.TeamImages.AddRange(teamImages);
                    await context.SaveChangesAsync();

                    CountryImage[] countryImages = countryImagesTasks.Select(countryImagesTask => countryImagesTask.Result).ToArray();
                    context.CountryImages.AddRange(countryImages);
                    await context.SaveChangesAsync();

                    List<Data.Team> teams = new List<Data.Team>();
                    for (int teamIndex = 0; teamIndex < loadedTeams.Count; teamIndex++)
                    {
                        teams.Add(new Data.Team()
                        {
                            Name = loadedTeams[teamIndex].Name,
                            Tag = loadedTeams[teamIndex].Tag,
                            Logo = teamImages[teamIndex],
                            CountryLogo =
                                context.CountryImages.FirstOrDefault( 
                                    countryImage => countryImage.Code == loadedTeams[teamIndex].CountryCode
                                )
                        });
                    }
                    context.Teams.AddRange(teams);

                    await context.SaveChangesAsync();
                    await SyncWithStorage();
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
