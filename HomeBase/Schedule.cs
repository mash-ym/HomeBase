using System;
using System.Collections.Generic;
using System.Data.SQLite;
namespace HomeBase
{
    
    public class Schedule
    {
        public int Id { get; set; }
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

        public ScheduleRepository(SQLiteConnection connection)
        {
            connectionString = connection.ConnectionString;
        }

        public void AddSchedule(Schedule schedule)
        {
            string insertQuery = @"
            INSERT INTO Schedule (site_name, site_duration, group_name, start_date, end_date, work_hours, subcontractor_id)
            VALUES (@SiteName, @SiteDuration, @GroupName, @StartDate, @EndDate, @WorkHours, @SubcontractorId);
            ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@SiteName", schedule.SiteName);
                command.Parameters.AddWithValue("@SiteDuration", schedule.SiteDuration);
                command.Parameters.AddWithValue("@GroupName", schedule.GroupName);
                command.Parameters.AddWithValue("@StartDate", schedule.StartDate);
                command.Parameters.AddWithValue("@EndDate", schedule.EndDate);
                command.Parameters.AddWithValue("@WorkHours", schedule.WorkHours);
                command.Parameters.AddWithValue("@SubcontractorId", schedule.SubcontractorId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Schedule> GetSchedules()
        {
            List<Schedule> schedules = new List<Schedule>();

            string selectQuery = "SELECT * FROM Schedule;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
            {
                connection.Open();

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Schedule schedule = new Schedule
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        SiteName = reader["site_name"].ToString(),
                        SiteDuration = reader["site_duration"].ToString(),
                        GroupName = reader["group_name"].ToString(),
                        StartDate = Convert.ToDateTime(reader["start_date"]),
                        EndDate = Convert.ToDateTime(reader["end_date"]),
                        WorkHours = Convert.ToDecimal(reader["work_hours"]),
                        SubcontractorId = Convert.ToInt32(reader["subcontractor_id"])
                    };

                    schedules.Add(schedule);
                }
            }

            return schedules;
        }

        public void DeleteSchedule(int id)
        {
            string deleteQuery = "DELETE FROM Schedule WHERE id = @Id;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

}