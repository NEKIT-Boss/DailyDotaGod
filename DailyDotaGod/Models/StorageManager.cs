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
using System.Collections;

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
        public ObservableCollection<TeamViewModel> FavoriteTeams { get; set; } = new ObservableCollection<TeamViewModel>();
        //Here we define that, and I wish som two way data binding, like, no just store favorite team we need, and then sync exposed


        public async Task<bool> SyncExposed(bool syncAll = false, bool syncTeams = false, bool syncFavorites = false, bool syncLeagues = false, bool syncMatches = false)
        {
            using (var context = new StorageContext())
            {
                if (syncAll)
                {
                    syncTeams = true;
                    syncFavorites = true;
                    syncLeagues = true;
                    syncMatches = true;
                }

                try
                {
                    if (syncTeams)
                    {
                        //Think of doing that smartly, but later
                        //Task[] imageLoadingTasks = new Task[2];
                        await context.TeamImages.LoadAsync();
                        await context.CountryImages.LoadAsync();

                        //await Task.WhenAll(imageLoadingTasks);

                        var teams = await context.Teams.ToListAsync();
                        if (teams.Any())
                        {
                            foreach (var team in teams)
                            {
                                Teams.Add(new TeamViewModel(team));
                            }
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
        /// Need to think about whether or not outside method must call it
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

        //I think that ie really bad
        private async Task SyncToStorage(StorageContext context)
        {
            await context.SaveChangesAsync().ConfigureAwait(false);
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
                byte[] rawData = await logoUri.DownloadRawAsync();
                if (rawData == null)
                {
                    return null;
                }

                return new TeamImage
                {
                    Data = rawData,
                };
            }).ConfigureAwait(false);
        }

        private async Task<CountryImage> LoadCountryImageAsync(Uri logoUri, string countryCode)
        {
            return await Task.Run(async () =>
            {
                if (logoUri == null)
                {
                    return null;
                }

                byte[] rawData = await logoUri.DownloadRawAsync();

                return new CountryImage
                {
                    Data = rawData,
                    Code = countryCode
                };
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds the team acquired from DailyDota API 
        /// </summary>
        /// <param name="loadedTeam">The team to add to database</param>
        /// <returns>Whether or not, the operation was successful</returns>
        public async Task<bool> StoreTeams( IEnumerable<DailyDotaProxy.Team> loadedTeams )
        {
            try
            {
                using (var context = new StorageContext())
                {
                    //Further must move it to separate methods

                    List<Task<TeamImage>> teamImagesTasks = new List<Task<TeamImage>>();
                    List<Task<CountryImage>> countryImagesTasks = new List<Task<CountryImage>>();
                    foreach (var loadedTeam in loadedTeams)
                    {
                        teamImagesTasks.Add( LoadTeamImageAsync(loadedTeam.LogoUrl) );
                        if ( !CountryImageExists(loadedTeam.CountryCode) )
                        {
                            countryImagesTasks.Add( LoadCountryImageAsync(loadedTeam.CountryLogoUrl, loadedTeam.CountryCode) );
                        }
                    }
                    await Task.WhenAll(countryImagesTasks);
                    await Task.WhenAll(teamImagesTasks);

                    TeamImage[] teamImages = teamImagesTasks
                        .Select(teamImageTask => teamImageTask.Result)
                        .ToArray();

                    context.TeamImages.AddRange(
                        teamImages.Where( teamImage => teamImage != null)
                    );
                    //await context.SaveChangesAsync();
                    await SyncToStorage(context);

                    CountryImage[] countryImages = countryImagesTasks
                        .Select(countryImagesTask => countryImagesTask.Result)
                        .ToArray();

                    context.CountryImages.AddRange(
                        countryImages.Where ( countryImage => countryImage != null )
                    );
                    //await context.SaveChangesAsync();
                    await SyncToStorage(context);

                    List<Data.Team> teams = new List<Data.Team>();
                    for (int teamIndex = 0; teamIndex < loadedTeams.Count(); teamIndex++)
                    {
                        teams.Add(new Data.Team()
                        {
                            Name = loadedTeams.ElementAt(teamIndex).Name,
                            Tag = loadedTeams.ElementAt(teamIndex).Tag,
                            Logo = teamImages[teamIndex],
                            CountryLogo =
                                context.CountryImages.FirstOrDefault( 
                                    countryImage => countryImage.Code == loadedTeams.ElementAt(teamIndex).CountryCode
                                )
                        });
                    }
                    context.Teams.AddRange(teams);
                    //await context.SaveChangesAsync();
                    await SyncToStorage(context);

                    //await SyncExposed();
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
