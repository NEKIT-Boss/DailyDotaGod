using DailyDotaGod.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;

namespace DailyDotaGod.ViewModels
{
    class MatchViewModel : NotificationBase<Match>
    {
        private bool _isLive;
        public bool IsLive
        {
            get
            {
                return _isLive;
            }

            set
            {
                SetProperty(ref _isLive, value);
            }
        }

        private TeamViewModel _radiantTeam;
        public TeamViewModel RadiantTeam
        {
            get
            {
                return _radiantTeam;
            }

            set
            {
                SetProperty(ref _radiantTeam, value);
            }
        }

        private TeamViewModel _direTeam;
        public TeamViewModel DireTeam
        {
            get
            {
                return _direTeam;
            }

            set
            {
                SetProperty(ref _direTeam, value);
            }
        }

        public string StartTime
        {
            get
            {
                return This.StartTime.ToString("hh : mm");
            }
        }

        public string BestOf
        {
            get
            {
                return $"BO{This.BestOf}";
            }
        }

        public async Task ScheduleEvent()
        {
            var matchAppointment = new Appointment();
            matchAppointment.Subject = $"{RadiantTeam} vs {DireTeam} скоро!";
            matchAppointment.Details = "DailyDotaGod проследит за тем, чтобы ты не пропустил ни одного важного матча!";

            matchAppointment.Location = "На ламповом Твиче";
            matchAppointment.StartTime = This.StartTime;
            matchAppointment.Reminder = TimeSpan.FromMinutes(15);
            matchAppointment.Duration = TimeSpan.FromHours(3);

            string appointmentId = await AppointmentManager.ShowAddAppointmentAsync(
                                   matchAppointment, new Rect( new Point(0, 0), new Size(400, 400) ), Windows.UI.Popups.Placement.Default);
        }

        public MatchViewModel(Match thing) : base(thing)
        {
            IsLive = This.StartTime <= DateTime.Now;
            RadiantTeam = new TeamViewModel(This.Team1);
            DireTeam = new TeamViewModel(This.Team2);
        }
    }
}
