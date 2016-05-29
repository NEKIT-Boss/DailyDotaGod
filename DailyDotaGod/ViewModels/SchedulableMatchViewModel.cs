using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyDotaGod.Data;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using DailyDotaGod.Models;

namespace DailyDotaGod.ViewModels
{
    class SchedulableMatchViewModel : MatchViewModel
    {
        private bool? _isScheduled = false;

        public bool? IsScheduled
        {
            get
            {
                return _isScheduled;
            }

            set
            {
                SetProperty(ref _isScheduled, value);
            }
        }

        private TimeSpan _timeLeft;
        public TimeSpan TimeLeft
        {
            get
            {
                return _timeLeft;
            }

            set
            {
                SetProperty(ref _timeLeft, value);
                TimeLeftReadable = $"{TimeLeft.Hours} : {TimeLeft.Minutes}";
            }
        }

        private string _timeLeftReadable;
        public string TimeLeftReadable
        {
            get
            {
                return _timeLeftReadable;
            }

            set
            {
                SetProperty(ref _timeLeftReadable, value);
            }
        }

        private string AppointmentId { get; set; } = string.Empty;

        private void ReSchedule(bool isScheduled)
        {
            IsScheduled = null;
            IsScheduled = isScheduled;
        }


        public SchedulableMatchViewModel(Match thing) : base(thing)
        {
            using (var context = new StorageContext())
            {
                ScheduledMatch scheduled = context.ScheduledMatches.FirstOrDefault(x => x.Match.Id == This.Id);
                if (scheduled != default(ScheduledMatch))
                {
                    ReSchedule(true);
                    AppointmentId = scheduled.AppointmentId;
                }

                TimeLeft = This.StartTime - DateTime.Now;
            }
        }

        public async Task ScheduleEvent()
        {
            if (IsScheduled.Value)
            {
                bool removed = await AppointmentManager.ShowRemoveAppointmentAsync(
                     AppointmentId, new Rect(new Point(0, 0), new Size(200, 200)), Windows.UI.Popups.Placement.Default);

                if (removed)
                {
                    ReSchedule(false);
                    await StorageManager.Instance.RemoveScheduledMatch(AppointmentId);
                    AppointmentId = string.Empty;
                }
                else
                {
                    ReSchedule(true);
                }
            }

            else
            {
                var matchAppointment = new Appointment();
                matchAppointment.Subject = $"{RadiantTeam} vs {DireTeam} скоро!";
                matchAppointment.Details = "DailyDotaGod проследит за тем, чтобы ты не пропустил ни одного важного матча!";

                matchAppointment.Location = "На ламповом Твиче";
                matchAppointment.StartTime = This.StartTime;
                matchAppointment.Reminder = TimeSpan.FromMinutes(15);
                matchAppointment.Duration = TimeSpan.FromHours(3);

                string appointmentId = await AppointmentManager.ShowAddAppointmentAsync(
                                       matchAppointment, new Rect(new Point(0, 0), new Size(200, 200)), Windows.UI.Popups.Placement.Default);

                if (appointmentId != string.Empty)
                {
                    ReSchedule(true);
                    AppointmentId = appointmentId;
                    await StorageManager.Instance.StoreScheduledMatch(This, AppointmentId);
                }
                else
                {
                    ReSchedule(false);
                }
            }
        }
    }
}
