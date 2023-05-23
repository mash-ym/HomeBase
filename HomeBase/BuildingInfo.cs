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
        private string connectionString;

        public BuildingInfoRepository(SQLiteConnection connection)
        {
            connectionString = connection.ConnectionString;
        }

        public void AddBuildingInfo(BuildingInfo buildingInfo)
        {
            string insertQuery = @"
            INSERT INTO BuildingInfo (building_name, room_number, structure, address, area, project_history, drawing_pdf)
            VALUES (@BuildingName, @RoomNumber, @Structure, @Address, @Area, @ProjectHistory, @DrawingPdf);
        ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@BuildingName", buildingInfo.BuildingName);
                command.Parameters.AddWithValue("@RoomNumber", buildingInfo.RoomNumber);
                command.Parameters.AddWithValue("@Structure", buildingInfo.Structure);
                command.Parameters.AddWithValue("@Address", buildingInfo.Address);
                command.Parameters.AddWithValue("@Area", buildingInfo.Area);
                command.Parameters.AddWithValue("@ProjectHistory", buildingInfo.ProjectHistory);
                command.Parameters.AddWithValue("@DrawingPdf", buildingInfo.DrawingPdf);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<BuildingInfo> GetAllBuildingInfo()
        {
            string selectQuery = "SELECT * FROM BuildingInfo;";

            List<BuildingInfo> buildingInfos = new List<BuildingInfo>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
            {
                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BuildingInfo buildingInfo = new BuildingInfo()
                        {
                            Id = Convert.ToInt32(reader["building_info_id"]),
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

            return buildingInfos;
        }

        public BuildingInfo GetBuildingInfoById(int buildingId)
        {
            string selectQuery = "SELECT * FROM BuildingInfo WHERE building_info_id = @BuildingId;";
            BuildingInfo buildingInfo = null;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@BuildingId", buildingId);

                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        buildingInfo = new BuildingInfo()
                        {
                            Id = Convert.ToInt32(reader["building_info_id"]),
                            BuildingName = Convert.ToString(reader["building_name"]),
                            RoomNumber = Convert.ToString(reader["room_number"]),
                            Structure = Convert.ToString(reader["structure"]),
                            Address = Convert.ToString(reader["address"]),
                            Area = Convert.ToDouble(reader["area"]),
                            ProjectHistory = Convert.ToString(reader["project_history"]),
                            DrawingPdf = (byte[])reader["drawing_pdf"]
                        };
                    }
                }
            }

            return buildingInfo;
        }

        public void UpdateBuildingInfo(BuildingInfo buildingInfo)
        {
            string updateQuery = @"
            UPDATE BuildingInfo
            SET building_name = @BuildingName, room_number = @RoomNumber, structure = @Structure,
                address = @Address, area = @Area, project_history = @ProjectHistory, drawing_pdf = @DrawingPdf
                        WHERE building_info_id = @BuildingId;
        ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@BuildingName", buildingInfo.BuildingName);
                command.Parameters.AddWithValue("@RoomNumber", buildingInfo.RoomNumber);
                command.Parameters.AddWithValue("@Structure", buildingInfo.Structure);
                command.Parameters.AddWithValue("@Address", buildingInfo.Address);
                command.Parameters.AddWithValue("@Area", buildingInfo.Area);
                command.Parameters.AddWithValue("@ProjectHistory", buildingInfo.ProjectHistory);
                command.Parameters.AddWithValue("@DrawingPdf", buildingInfo.DrawingPdf);
                command.Parameters.AddWithValue("@BuildingId", buildingInfo.Id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteBuildingInfo(int buildingId)
        {
            string deleteQuery = "DELETE FROM BuildingInfo WHERE building_info_id = @BuildingId;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@BuildingId", buildingId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}


