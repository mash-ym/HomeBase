using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace HomeBase
{   

    public class BuildingInfo
    {
        public int Id { get; set; }
        public string BuildingName { get; set; }
        public string RoomNumber { get; set; }
        public string Structure { get; set; }
        public string Address { get; set; }
        public double Area { get; set; }
        public string ProjectHistory { get; set; }
        public byte[] DrawingPdf { get; set; }
    }

    public class BuildingInfoRepository
    {
        private readonly DBManager _dbManager;
        private readonly ErrorHandler _errorHandler;

        public BuildingInfoRepository(DBManager dbManager, ErrorHandler errorHandler)
        {
            _dbManager = dbManager;
            _errorHandler = errorHandler;
        }

        public void InsertBuildingInfo(BuildingInfo buildingInfo)
        {
            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "INSERT INTO BuildingInfo (BuildingName, RoomNumber, Structure, Address, Area, ProjectHistory, DrawingPdf) " +
                                          "VALUES (@BuildingName, @RoomNumber, @Structure, @Address, @Area, @ProjectHistory, @DrawingPdf)";
                    command.Parameters.AddWithValue("@BuildingName", buildingInfo.BuildingName);
                    command.Parameters.AddWithValue("@RoomNumber", buildingInfo.RoomNumber);
                    command.Parameters.AddWithValue("@Structure", buildingInfo.Structure);
                    command.Parameters.AddWithValue("@Address", buildingInfo.Address);
                    command.Parameters.AddWithValue("@Area", buildingInfo.Area);
                    command.Parameters.AddWithValue("@ProjectHistory", buildingInfo.ProjectHistory);
                    command.Parameters.AddWithValue("@DrawingPdf", buildingInfo.DrawingPdf);

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
            List<BuildingInfo> buildingInfoList = new List<BuildingInfo>();

            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                try
                {
                    command.CommandText = "SELECT * FROM BuildingInfo";

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BuildingInfo buildingInfo = new BuildingInfo
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                BuildingName = Convert.ToString(reader["BuildingName"]),
                                RoomNumber = Convert.ToString(reader["RoomNumber"]),
                                Structure = Convert.ToString(reader["Structure"]),
                                Address = Convert.ToString(reader["Address"]),
                                Area = Convert.ToDouble(reader["Area"]),
                                ProjectHistory = Convert.ToString(reader["ProjectHistory"]),
                                DrawingPdf = (byte[])reader["DrawingPdf"]
                            };

                            buildingInfoList.Add(buildingInfo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.ShowErrorMessage("データ読込エラー", ex);
                }
            }

            return buildingInfoList;
        }
        public BuildingInfo GetBuildingInfoById(int buildingId)
        {
            BuildingInfo buildingInfo = null;

            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                try
                {
                    command.CommandText = "SELECT * FROM BuildingInfo WHERE Id = @BuildingId";
                    command.Parameters.AddWithValue("@BuildingId", buildingId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            buildingInfo = new BuildingInfo
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                BuildingName = Convert.ToString(reader["BuildingName"]),
                                RoomNumber = Convert.ToString(reader["RoomNumber"]),
                                Structure = Convert.ToString(reader["Structure"]),
                                Address = Convert.ToString(reader["Address"]),
                                Area = Convert.ToDouble(reader["Area"]),
                                ProjectHistory = Convert.ToString(reader["ProjectHistory"]),
                                DrawingPdf = (byte[])reader["DrawingPdf"]
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.ShowErrorMessage("データエラー", ex);
                }
            }

            return buildingInfo;
        }
        public void UpdateBuildingInfo(BuildingInfo buildingInfo)
        {
            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                try
                {
                    command.CommandText = @"UPDATE BuildingInfo SET
                                    BuildingName = @BuildingName,
                                    RoomNumber = @RoomNumber,
                                    Structure = @Structure,
                                    Address = @Address,
                                    Area = @Area,
                                    ProjectHistory = @ProjectHistory,
                                    DrawingPdf = @DrawingPdf
                                    WHERE Id = @BuildingId";

                    command.Parameters.AddWithValue("@BuildingName", buildingInfo.BuildingName);
                    command.Parameters.AddWithValue("@RoomNumber", buildingInfo.RoomNumber);
                    command.Parameters.AddWithValue("@Structure", buildingInfo.Structure);
                    command.Parameters.AddWithValue("@Address", buildingInfo.Address);
                    command.Parameters.AddWithValue("@Area", buildingInfo.Area);
                    command.Parameters.AddWithValue("@ProjectHistory", buildingInfo.ProjectHistory);
                    command.Parameters.AddWithValue("@DrawingPdf", buildingInfo.DrawingPdf);
                    command.Parameters.AddWithValue("@BuildingId", buildingInfo.Id);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ErrorHandler.ShowErrorMessage("データ上書きエラー", ex);
                }
            }
        }
        public void DeleteBuildingInfo(int buildingId)
        {
            using (SQLiteConnection connection = _dbManager.GetConnection())
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
            List<BuildingInfo> buildingInfos = new List<BuildingInfo>();

            using (SQLiteConnection connection = _dbManager.GetConnection())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                try
                {
                    command.CommandText = "SELECT * FROM BuildingInfo WHERE id = @Keyword OR building_name LIKE @Keyword OR room_number LIKE @Keyword OR structure LIKE @Keyword OR address LIKE @Keyword OR project_history LIKE @Keyword";
                    command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BuildingInfo buildingInfo = new BuildingInfo
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                BuildingName = Convert.ToString(reader["building_name"]),
                                RoomNumber = Convert.ToString(reader["room_number"]),
                                Structure = Convert.ToString(reader["structure"]),
                                Address = Convert.ToString(reader["address"]),
                                Area = Convert.ToDouble(reader["area"]),
                                ProjectHistory = Convert.ToString(reader["project_history"]),
                                DrawingPdf = (byte[])reader["drawing_pdf"]
                            };

                            buildingInfos.Add(buildingInfo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.ShowErrorMessage("検索エラー", ex);
                }
            }

            return buildingInfos;
        }
    }
}


