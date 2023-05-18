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
            
        }

        public void CreateSchedule(Schedule schedule)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Schedule (id, site_name, site_duration, group_name, 
                            start_date, end_date, work_hours, subcontractor_id)
                            VALUES (@Id, @SiteName, @SiteDuration, @GroupName, 
                            @StartDate, @EndDate, @WorkHours, @SubcontractorId)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", schedule.Id);
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
        public void InsertSchedule(Schedule schedule)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Schedule (SiteName, SiteDuration, GroupName, StartDate, EndDate, WorkHours, SubcontractorId) " +
                                          "VALUES (@SiteName, @SiteDuration, @GroupName, @StartDate, @EndDate, @WorkHours, @SubcontractorId)";
                    command.Parameters.AddWithValue("@SiteName", schedule.SiteName);
                    command.Parameters.AddWithValue("@SiteDuration", schedule.SiteDuration);
                    command.Parameters.AddWithValue("@GroupName", schedule.GroupName);
                    command.Parameters.AddWithValue("@StartDate", schedule.StartDate);
                    command.Parameters.AddWithValue("@EndDate", schedule.EndDate);
                    command.Parameters.AddWithValue("@WorkHours", schedule.WorkHours);
                    command.Parameters.AddWithValue("@SubcontractorID", schedule.SubcontractorId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Schedule> GetAllSchedules()
        {
            var schedules = new List<Schedule>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Schedule";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var schedule = new Schedule
                            {
                                Id = reader.GetInt32(0),
                                SiteName = reader.GetString(1),
                                SiteDuration = reader.GetString(2),
                                GroupName = reader.GetString(3),
                                StartDate = reader.GetDateTime(4),
                                EndDate = reader.GetDateTime(5),
                                WorkHours = reader.GetDecimal(6),
                                SubcontractorId = reader.GetInt32(7)
                            };

                            schedules.Add(schedule);
                        }
                    }
                }
            }

            return schedules;
        }

        public void UpdateSchedule(Schedule schedule)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Schedule SET SiteName = @SiteName, SiteDuration = @SiteDuration, " +
                                          "GroupName = @GroupName, StartDate = @StartDate, EndDate = @EndDate, " +
                                          "WorkHours = @WorkHours, SubcontractorId = @SubcontractorId WHERE Id = @Id";
                    command.Parameters.AddWithValue("@SiteName", schedule.SiteName);
                    command.Parameters.AddWithValue("@SiteDuration", schedule.SiteDuration);
                    command.Parameters.AddWithValue("@GroupName", schedule.GroupName);
                    command.Parameters.AddWithValue("@StartDate", schedule.StartDate);
                    command.Parameters.AddWithValue("@EndDate", schedule.EndDate);
                    command.Parameters.AddWithValue("@WorkHours", schedule.WorkHours);
                    command.Parameters.AddWithValue("@SubcontractorID", schedule.SubcontractorId);
                    command.Parameters.AddWithValue("@Id", schedule.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteSchedule(int Id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Schedule WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}