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
    class StorageManager : NotificationBase
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

        private bool _updateNotifier;
        public bool UpdateNotifier
        {
            get
            {
                return _updateNotifier;
            }

            set
            {
                SetProperty(ref _updateNotifier, value);
            }
        }

        private void Notify()
        {
            UpdateNotifier = !UpdateNotifier;
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

        internal bool MatchExists(DailyDotaProxy.Match match)
        {
            using (var context = new StorageContext())
            {
                var storedMatch = context.Matches.FirstOrDefault( (x) =>
                    x.StartTime == match.StartTime
                        && x.Team1.Name == match.Team1.Name
                        && x.Team2.Name == match.Team2.Name
                    );

                return storedMatch != default(Data.Match);
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
                byte[] rawData = null;
                TeamImage image = null;

                try
                {
                    rawData = await logoUri.DownloadRawAsync();
                }

                catch (NullReferenceException)
                {
                    Debug.WriteLine($"Logo is null, but everything is fine!");
                }

                catch (Exception ex)
                {
                    Debug.WriteLine($"{ex.Message} something went wrong while loading");
                }

                finally
                {
                    image = (rawData != null) ? new TeamImage { Data = rawData } : null;
                }

                return image;
            }).ConfigureAwait(false);
        }

        private async Task<CountryImage> LoadCountryImageAsync(Uri logoUri, string countryCode)
        {
            return await Task.Run(async () =>
            {
                byte[] rawData = null;
                CountryImage image = null;

                try
                {
                    rawData = await logoUri.DownloadRawAsync();
                }

                catch (NullReferenceException)
                {
                    Debug.WriteLine($"Logo is null, but everything is fine!");
                }

                catch (Exception ex)
                {
                    Debug.WriteLine($"{ex.Message} something went wrong while loading");
                }

                finally
                {
                    image = (rawData != null) ? new CountryImage { Code = countryCode, Data = rawData } : null;
                }

                return image;
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds the team acquired from DailyDota API 
        /// </summary>
        /// <param name="loadedTeam">The team to add to database</param>
        /// <returns>Whether or not, the operation was successful</returns>
        public async Task<bool> StoreTeams( IEnumerable<DailyDotaProxy.Team> newTeams )
        {
            try
            {
                using (var context = new StorageContext())
                {
                    List<Task<TeamImage>> teamImagesTasks = new List<Task<TeamImage>>();
                    List<Task<CountryImage>> countryImagesTasks = new List<Task<CountryImage>>();
                    foreach (var newTeam in newTeams)
                    {
                        teamImagesTasks.Add( LoadTeamImageAsync(newTeam.LogoUrl) );
                        if ( !CountryImageExists(newTeam.CountryCode) )
                        {
                            countryImagesTasks.Add( LoadCountryImageAsync(newTeam.CountryLogoUrl, newTeam.CountryCode) );
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
                    await context.SaveChangesAsync();

                    CountryImage[] countryImages = countryImagesTasks
                        .Select(countryImagesTask => countryImagesTask.Result)
                        .ToArray();

                    context.CountryImages.AddRange(
                        countryImages.Where ( countryImage => countryImage != null )
                    );
                    await context.SaveChangesAsync();

                    List<Data.Team> teams = new List<Data.Team>();
                    for (int teamIndex = 0; teamIndex < newTeams.Count(); teamIndex++)
                    {
                        teams.Add(new Data.Team()
                        {
                            Name = newTeams.ElementAt(teamIndex).Name,
                            Tag = newTeams.ElementAt(teamIndex).Tag,
                            Logo = teamImages[teamIndex],
                            CountryLogo =
                                context.CountryImages.FirstOrDefault( 
                                    countryImage => countryImage.Code == newTeams.ElementAt(teamIndex).CountryCode
                                )
                        });
                    }
                    context.Teams.AddRange(teams);
                    await context.SaveChangesAsync();
                    Notify();

                    return true;
                }
            }

            catch
            {
                return false;
            }
        }

        public async Task StoreMatches(IEnumerable<DailyDotaProxy.Match> loadedMatches)
        {
            using (var context = new StorageContext())
            {
                foreach( var loadedMatch in loadedMatches)
                {
                    context.Matches.Add(new Data.Match
                    {
                        BestOf = loadedMatch.BestOf,
                        League = null,
                        LiveStatus = loadedMatch.LiveStatus,
                        StartTime = loadedMatch.StartTime,
                        Team1 = await context.Teams.FirstAsync( x => x.Name == loadedMatch.Team1.Name ),
                        Team2 = await context.Teams.FirstAsync( x => x.Name == loadedMatch.Team2.Name ),
                    });
                }

                await context.SaveChangesAsync();
                Notify();
            }
        }

        public async Task<bool> StoreScheduledMatch(Data.Match match, string appointmentId) 
        {
            using (var context = new StorageContext())
            {
                try
                {
                    ScheduledMatch scheduled = new ScheduledMatch()
                    {
                        Match = match,
                        AppointmentId = appointmentId
                    };

                    context.ScheduledMatches.Add(scheduled);
                    await context.SaveChangesAsync();
                    return true;
                }

                catch
                {
                    return false;
                }
            }
        }

        public async Task<bool> RemoveScheduledMatch(string appointmentId)
        {
            using (var context = new StorageContext())
            {
                try
                {
                    context.ScheduledMatches.Remove(await context.ScheduledMatches.FirstAsync(x => x.AppointmentId == appointmentId));
                    await context.SaveChangesAsync();
                    return true;
                }

                catch
                {
                    return false;
                }
            }
        }

        public async Task<bool> RemoveFavoriteTeam(Data.Team team)
        {
            using (var context = new StorageContext())
            {
                try
                {
                    context.FavoriteTeams.Remove(await context.FavoriteTeams.FirstAsync(x => x.Team.Id == team.Id));
                    await context.SaveChangesAsync();
                    return true;
                }

                catch
                {
                    return false;
                }
            }
        }

    }
}
