using System;
using System.Data.SQLite;

namespace HomeBase
{   

    public class BuildingInfo
    {
        public int BuildingInfoId { get; set; }
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

        public BuildingInfoRepository(string dbPath)
        {
            connectionString = $"Data Source={dbPath};";
        }

        public void CreateBuildingInfo(BuildingInfo buildingInfo)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO BuildingInfo (building_info_id, building_name, room_number, 
                            structure, address, area, project_history, drawing_pdf)
                            VALUES (@BuildingInfoId, @BuildingName, @RoomNumber, @Structure, 
                            @Address, @Area, @ProjectHistory, @DrawingPdf)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BuildingInfoId", buildingInfo.BuildingInfoId);
                    command.Parameters.AddWithValue("@BuildingName", buildingInfo.BuildingName);
                    command.Parameters.AddWithValue("@RoomNumber", buildingInfo.RoomNumber);
                    command.Parameters.AddWithValue("@Structure", buildingInfo.Structure);
                    command.Parameters.AddWithValue("@Address", buildingInfo.Address);
                    command.Parameters.AddWithValue("@Area", buildingInfo.Area);
                    command.Parameters.AddWithValue("@ProjectHistory", buildingInfo.ProjectHistory);
                    command.Parameters.AddWithValue("@DrawingPdf", buildingInfo.DrawingPdf);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Implement other CRUD operations as needed: Read, Update, Delete
    }

}
