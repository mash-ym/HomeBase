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
        private readonly DBManager _dbManager;
        

        public ScheduleRepository(DBManager dbManager)
        {
            _dbManager = dbManager;
            
        }

        public void InsertSchedule(Schedule schedule)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "INSERT INTO Schedule (SiteName, SiteDuration, GroupName, StartDate, EndDate, WorkHours, SubcontractorId)" +
                                          "VALUES (@SiteName, @SiteDuration, @GroupName, @StartDate, @EndDate, @WorkHours, @SubcontractorId)";
                    command.Parameters.AddWithValue("@SiteName", schedule.SiteName);
                    command.Parameters.AddWithValue("@SiteDuration", schedule.SiteDuration);
                    command.Parameters.AddWithValue("@GroupName", schedule.GroupName);
                    command.Parameters.AddWithValue("@StartDate", schedule.StartDate);
                    command.Parameters.AddWithValue("@EndDate", schedule.EndDate);
                    command.Parameters.AddWithValue("@WorkHours", schedule.WorkHours);
                    command.Parameters.AddWithValue("@SubcontractorId", schedule.SubcontractorId);

                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ErrorHandler.ShowErrorMessage("データの挿入エラー", ex);
                }
            }
        }

        public void UpdateSchedule(Schedule schedule)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "UPDATE Schedule SET SiteName = @SiteName, SiteDuration = @SiteDuration, GroupName = @GroupName, " +
                                          "StartDate = @StartDate, EndDate = @EndDate, WorkHours = @WorkHours, SubcontractorId = @SubcontractorId " +
                                          "WHERE Id = @Id";
                    command.Parameters.AddWithValue("@SiteName", schedule.SiteName);
                    command.Parameters.AddWithValue("@SiteDuration", schedule.SiteDuration);
                    command.Parameters.AddWithValue("@GroupName", schedule.GroupName);
                    command.Parameters.AddWithValue("@StartDate", schedule.StartDate);
                    command.Parameters.AddWithValue("@EndDate", schedule.EndDate);
                    command.Parameters.AddWithValue("@WorkHours", schedule.WorkHours);
                    command.Parameters.AddWithValue("@SubcontractorId", schedule.SubcontractorId);
                    command.Parameters.AddWithValue("@Id", schedule.Id);

                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ErrorHandler.ShowErrorMessage("データの更新エラー", ex);
                }
            }
        }

        public void DeleteSchedule(int scheduleId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "DELETE FROM Schedule WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", scheduleId);

                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ErrorHandler.ShowErrorMessage("データの削除エラー", ex);
                }
            }
        }

        public List<Schedule> GetAllSchedules()
        {
            List<Schedule> schedules = new List<Schedule>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Schedule";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Schedule schedule = new Schedule
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            SiteName = Convert.ToString(reader["SiteName"]),
                            SiteDuration = Convert.ToString(reader["SiteDuration"]),
                            GroupName = Convert.ToString(reader["GroupName"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            EndDate = Convert.ToDateTime(reader["EndDate"]),
                            WorkHours = Convert.ToDecimal(reader["WorkHours"]),
                            SubcontractorId = Convert.ToInt32(reader["SubcontractorId"])
                        };

                        schedules.Add(schedule);
                    }
                }
            }

            return schedules;
        }

        public Schedule GetScheduleById(int scheduleId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Schedule WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", scheduleId);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Schedule schedule = new Schedule
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            SiteName = Convert.ToString(reader["SiteName"]),
                            SiteDuration = Convert.ToString(reader["SiteDuration"]),
                            GroupName = Convert.ToString(reader["GroupName"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            EndDate = Convert.ToDateTime(reader["EndDate"]),
                            WorkHours = Convert.ToDecimal(reader["WorkHours"]),
                            SubcontractorId = Convert.ToInt32(reader["SubcontractorId"])
                        };

                        return schedule;
                    }
                }
            }

            return null;
        }

        public List<Schedule> SearchSchedules(string keyword)
        {
            List<Schedule> schedules = new List<Schedule>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Schedule WHERE SiteName LIKE @Keyword OR GroupName LIKE @Keyword";
                command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Schedule schedule = new Schedule
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            SiteName = Convert.ToString(reader["SiteName"]),
                            SiteDuration = Convert.ToString(reader["SiteDuration"]),
                            GroupName = Convert.ToString(reader["GroupName"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            EndDate = Convert.ToDateTime(reader["EndDate"]),
                            WorkHours = Convert.ToDecimal(reader["WorkHours"]),
                            SubcontractorId = Convert.ToInt32(reader["SubcontractorId"])
                        };

                        schedules.Add(schedule);
                    }
                }
            }

            return schedules;
        }
    }

}