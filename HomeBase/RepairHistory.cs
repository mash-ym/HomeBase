using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBase
{
    public class RepairHistory
    {
        public int Id { get; set; }
        public string ProjectId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int BuildingInfoId { get; set; }
        public BuildingInfo BuildingInfo { get; set; }
        public Project Project { get; set; }
    }

    public class RepairHistoryRepository
    {
        private readonly DBManager _dbManager;

        public RepairHistoryRepository(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public void InsertRepairHistory(RepairHistory repairHistory)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "INSERT INTO RepairHistory (ProjectId, Description, Date, BuildingInfoId) " +
                                          "VALUES (@ProjectId, @Description, @Date, @BuildingInfoId)";
                    command.Parameters.AddWithValue("@ProjectId", repairHistory.ProjectId);
                    command.Parameters.AddWithValue("@Description", repairHistory.Description);
                    command.Parameters.AddWithValue("@Date", repairHistory.Date);
                    command.Parameters.AddWithValue("@BuildingInfoId", repairHistory.BuildingInfoId);

                    command.ExecuteNonQuery();

                    // 挿入したデータのIDを取得
                    command.CommandText = "SELECT last_insert_rowid();";
                    int repairHistoryId = Convert.ToInt32(command.ExecuteScalar());

                    // RepairHistoryオブジェクトにIDを設定
                    repairHistory.Id = repairHistoryId;

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Failed to insert RepairHistory.", ex);
                }
            }
        }

        public RepairHistory GetRepairHistoryById(int repairHistoryId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
            SELECT RH.Id, RH.ProjectId, RH.Description, RH.Date, RH.BuildingInfoId,
                   P.Id AS Project_Id, P.ProjectName, P.StartDate, P.EndDate, P.Status
            FROM RepairHistory RH
            JOIN Projects P ON RH.ProjectId = P.Id
            WHERE RH.Id = @RepairHistoryId;
        ";
                command.Parameters.AddWithValue("@RepairHistoryId", repairHistoryId);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        RepairHistory repairHistory = new RepairHistory
                        {
                            Id = reader.GetInt32(0),
                            ProjectId = reader.GetString(1),
                            Description = reader.GetString(2),
                            Date = reader.GetDateTime(3),
                            BuildingInfoId = reader.GetInt32(4),
                            Project = new Project
                            {
                                ProjectId = reader.GetInt32(5),
                                ProjectName = reader.GetString(6),
                                StartDate = reader.GetDateTime(7),
                                EndDate = reader.GetDateTime(8),
                                Status = reader.GetString(9)
                            }
                        };

                        return repairHistory;
                    }
                }
            }

            return null;
        }

        public void UpdateRepairHistory(RepairHistory repairHistory)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "UPDATE RepairHistory " +
                                          "SET ProjectId = @ProjectId, Description = @Description, Date = @Date, BuildingInfoId = @BuildingInfoId " +
                                          "WHERE Id = @RepairHistoryId";
                    command.Parameters.AddWithValue("@ProjectId", repairHistory.ProjectId);
                    command.Parameters.AddWithValue("@Description", repairHistory.Description);
                    command.Parameters.AddWithValue("@Date", repairHistory.Date);
                    command.Parameters.AddWithValue("@BuildingInfoId", repairHistory.BuildingInfoId);
                    command.Parameters.AddWithValue("@RepairHistoryId", repairHistory.Id);

                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Failed to update RepairHistory.", ex);
                }
            }
        }

        public List<RepairHistory> GetAllRepairHistorys()
        {
            List<RepairHistory> repairHistories = new List<RepairHistory>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM RepairHistory";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RepairHistory repairHistory = new RepairHistory
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ProjectId = Convert.ToString(reader["ProjectId"]),
                            Description = Convert.ToString(reader["Description"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"])
                        };

                        repairHistories.Add(repairHistory);
                    }
                }
            }

            return repairHistories;
        }

        public List<RepairHistory> SearchRepairHistory(string keyword)
        {
            List<RepairHistory> searchResults = new List<RepairHistory>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM RepairHistory WHERE ProjectId LIKE @Keyword OR Description LIKE @Keyword";
                command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RepairHistory repairHistory = new RepairHistory
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ProjectId = Convert.ToString(reader["ProjectId"]),
                            Description = Convert.ToString(reader["Description"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"])
                        };

                        searchResults.Add(repairHistory);
                    }
                }
            }

            return searchResults;
        }

        public void DeleteRepairHistory(int repairHistoryId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    // 削除する修理履歴のIDを指定して削除クエリを実行
                    command.CommandText = "DELETE FROM RepairHistory WHERE Id = @RepairHistoryId";
                    command.Parameters.AddWithValue("@RepairHistoryId", repairHistoryId);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Failed to delete RepairHistory.", ex);
                }
            }
        }



    }
}
