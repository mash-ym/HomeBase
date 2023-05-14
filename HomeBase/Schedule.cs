using System;
using System.Data.SQLite;
namespace HomeBase
{
    
    public class Schedule
    {
        public int ScheduleId { get; set; }
        public string SiteName { get; set; }
        public string SiteDuration { get; set; }
        public string GroupName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal WorkHours { get; set; }
        public int SubcontractorId { get; set; }
    }

    public class ScheduleRepository
    {
        private string connectionString;

        public ScheduleRepository(string dbPath)
        {
            connectionString = $"Data Source={dbPath};";
        }

        public void CreateSchedule(Schedule schedule)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Schedule (schedule_id, site_name, site_duration, group_name, 
                            start_date, end_date, work_hours, subcontractor_id)
                            VALUES (@ScheduleId, @SiteName, @SiteDuration, @GroupName, 
                            @StartDate, @EndDate, @WorkHours, @SubcontractorId)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ScheduleId", schedule.ScheduleId);
                    command.Parameters.AddWithValue("@SiteName", schedule.SiteName);
                    command.Parameters.AddWithValue("@SiteDuration", schedule.SiteDuration);
                    command.Parameters.AddWithValue("@GroupName", schedule.GroupName);
                    command.Parameters.AddWithValue("@StartDate", schedule.StartDate);
                    command.Parameters.AddWithValue("@EndDate", schedule.EndDate);
                    command.Parameters.AddWithValue("@WorkHours", schedule.WorkHours);
                    command.Parameters.AddWithValue("@SubcontractorId", schedule.SubcontractorId);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Implement other CRUD operations as needed: Read, Update, Delete
    }

}
