using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace HomeBase
{

    public class BuildingInfo
    {
        public int Id { get; set; } // 建物ID
        public string BuildingName { get; set; } // 建物名
        public string RoomNumber { get; set; } // 部屋番号
        public string Structure { get; set; } // 構造
        public string Address { get; set; } // 住所
        public double Area { get; set; } // 面積
        public string ProjectHistory { get; set; } // プロジェクト履歴
        public byte[] DrawingPdf { get; set; } // 図面のPDFデータ
        public int ProjectId { get; set; } // プロジェクトID
        public Project Project { get; set; } // プロジェクトとの関連
        public Estimate Estimate { get; set; } // 見積もりとの関連
        public List<SpecificationDocument> SpecificationDocuments { get; set; } // 仕様書との関連
        public List<Drawing> Drawings { get; set; } // 図面との関連
        public List<RepairHistory> RepairHistories { get; set; } // 修繕履歴との関連
    }

    public class BuildingInfoRepository
    {
        private readonly DBManager _dbManager;

        public BuildingInfoRepository(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public void InsertBuildingInfo(BuildingInfo buildingInfo)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = @"
                INSERT INTO BuildingInfo (BuildingName, RoomNumber, Structure, Address, Area, ProjectHistory, DrawingPdf, ProjectId)
                VALUES (@BuildingName, @RoomNumber, @Structure, @Address, @Area, @ProjectHistory, @DrawingPdf, @ProjectId)
            ";

                    command.Parameters.AddWithValue("@BuildingName", buildingInfo.BuildingName);
                    command.Parameters.AddWithValue("@RoomNumber", buildingInfo.RoomNumber);
                    command.Parameters.AddWithValue("@Structure", buildingInfo.Structure);
                    command.Parameters.AddWithValue("@Address", buildingInfo.Address);
                    command.Parameters.AddWithValue("@Area", buildingInfo.Area);
                    command.Parameters.AddWithValue("@ProjectHistory", buildingInfo.ProjectHistory);
                    command.Parameters.AddWithValue("@DrawingPdf", buildingInfo.DrawingPdf);
                    command.Parameters.AddWithValue("@ProjectId", buildingInfo.ProjectId);

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

        public List<BuildingInfo> GetAllBuildingInfo()
        {
            List<BuildingInfo> buildingInfos = new List<BuildingInfo>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM BuildingInfo";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BuildingInfo buildingInfo = new BuildingInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            BuildingName = reader["BuildingName"].ToString(),
                            RoomNumber = reader["RoomNumber"].ToString(),
                            Structure = reader["Structure"].ToString(),
                            Address = reader["Address"].ToString(),
                            Area = Convert.ToDouble(reader["Area"]),
                            ProjectHistory = reader["ProjectHistory"].ToString(),
                            DrawingPdf = (byte[])reader["DrawingPdf"],
                            ProjectId = Convert.ToInt32(reader["ProjectId"])
                        };

                        buildingInfos.Add(buildingInfo);
                    }
                }
            }

            return buildingInfos;
        }

        public BuildingInfo GetBuildingInfoById(int buildingId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM BuildingInfo WHERE Id = @BuildingId";
                command.Parameters.AddWithValue("@BuildingId", buildingId);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        BuildingInfo buildingInfo = new BuildingInfo
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            BuildingName = reader["BuildingName"].ToString(),
                            RoomNumber = reader["RoomNumber"].ToString(),
                            Structure = reader["Structure"].ToString(),
                            Address = reader["Address"].ToString(),
                            Area = Convert.ToDouble(reader["Area"]),
                            ProjectHistory = reader["ProjectHistory"].ToString(),
                            DrawingPdf = (byte[])reader["DrawingPdf"],
                            ProjectId = Convert.ToInt32(reader["ProjectId"])
                        };

                        return buildingInfo;
                    }
                }
            }

            return null;
        }

        public void UpdateBuildingInfo(BuildingInfo buildingInfo)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "UPDATE BuildingInfo SET BuildingName = @BuildingName, RoomNumber = @RoomNumber, Structure = @Structure, Address = @Address, Area = @Area, ProjectHistory = @ProjectHistory, DrawingPdf = @DrawingPdf, ProjectId = @ProjectId WHERE Id = @BuildingId";
                    command.Parameters.AddWithValue("@BuildingId", buildingInfo.Id);
                    command.Parameters.AddWithValue("@BuildingName", buildingInfo.BuildingName);
                    command.Parameters.AddWithValue("@RoomNumber", buildingInfo.RoomNumber);
                    command.Parameters.AddWithValue("@Structure", buildingInfo.Structure);
                    command.Parameters.AddWithValue("@Address", buildingInfo.Address);
                    command.Parameters.AddWithValue("@Area", buildingInfo.Area);
                    command.Parameters.AddWithValue("@ProjectHistory", buildingInfo.ProjectHistory);
                    command.Parameters.AddWithValue("@DrawingPdf", buildingInfo.DrawingPdf);
                    command.Parameters.AddWithValue("@ProjectId", buildingInfo.ProjectId);

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

        public void DeleteBuildingInfo(int buildingId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                try
                {
                    command.CommandText = @"DELETE FROM BuildingInfo WHERE Id = @BuildingId";
                    command.Parameters.AddWithValue("@BuildingId", buildingId);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ErrorHandler.ShowErrorMessage("データ削除エラー", ex);
                }
            }
        }
        public List<BuildingInfo> SearchBuildingInfo(string keyword)
        {
            List<BuildingInfo> results = new List<BuildingInfo>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM BuildingInfo WHERE BuildingName LIKE @Keyword OR RoomNumber LIKE @Keyword OR Structure LIKE @Keyword OR Address LIKE @Keyword";
                command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BuildingInfo buildingInfo = new BuildingInfo();
                        buildingInfo.Id = Convert.ToInt32(reader["Id"]);
                        buildingInfo.BuildingName = reader["BuildingName"].ToString();
                        buildingInfo.RoomNumber = reader["RoomNumber"].ToString();
                        buildingInfo.Structure = reader["Structure"].ToString();
                        buildingInfo.Address = reader["Address"].ToString();
                        buildingInfo.Area = Convert.ToDouble(reader["Area"]);
                        buildingInfo.ProjectHistory = reader["ProjectHistory"].ToString();
                        buildingInfo.DrawingPdf = (byte[])reader["DrawingPdf"];
                        buildingInfo.ProjectId = Convert.ToInt32(reader["ProjectId"]);

                        results.Add(buildingInfo);
                    }
                }
            }

            return results;
        }

    }
}


