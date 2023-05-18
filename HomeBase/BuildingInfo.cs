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
        }

        public void CreateBuildingInfo(BuildingInfo buildingInfo)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO BuildingInfo (id, building_name, room_number, 
                            structure, address, area, project_history, drawing_pdf)
                            VALUES (@BuildingInfoId, @BuildingName, @RoomNumber, @Structure, 
                            @Address, @Area, @ProjectHistory, @DrawingPdf)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", buildingInfo.Id);
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
        public void Insert(BuildingInfo buildingInfo)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                    INSERT INTO BuildingInfo (BuildingName, RoomNumber, Structure, Address, Area, ProjectHistory, DrawingPdf)
                    VALUES (@BuildingName, @RoomNumber, @Structure, @Address, @Area, @ProjectHistory, @DrawingPdf)";

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

        public void Update(BuildingInfo buildingInfo)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"
                    UPDATE BuildingInfo
                    SET BuildingName = @BuildingName,
                        RoomNumber = @RoomNumber,
                        Structure = @Structure,
                        Address = @Address,
                        Area = @Area,
                        ProjectHistory = @ProjectHistory,
                        DrawingPdf = @DrawingPdf
                    WHERE Id = @Id";

                    command.Parameters.AddWithValue("@BuildingName", buildingInfo.BuildingName);
                    command.Parameters.AddWithValue("@RoomNumber", buildingInfo.RoomNumber);
                    command.Parameters.AddWithValue("@Structure", buildingInfo.Structure);
                    command.Parameters.AddWithValue("@Address", buildingInfo.Address);
                    command.Parameters.AddWithValue("@Area", buildingInfo.Area);
                    command.Parameters.AddWithValue("@ProjectHistory", buildingInfo.ProjectHistory);
                    command.Parameters.AddWithValue("@DrawingPdf", buildingInfo.DrawingPdf);
                    command.Parameters.AddWithValue("@Id", buildingInfo.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int buildingInfoId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM BuildingInfo WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", buildingInfoId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public BuildingInfo GetById(int buildingInfoId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT * FROM BuildingInfo WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", buildingInfoId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new BuildingInfo
                            {
                                Id = reader.GetInt32(0),
                                BuildingName = reader.GetString(1),
                                RoomNumber = reader.GetString(2),
                                Structure = reader.GetString(3),
                                Address = reader.GetString(4),
                                Area = reader.GetDouble(5),
                                ProjectHistory = reader.GetString(6),
                                DrawingPdf = (byte[])reader.GetValue(7)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public List<BuildingInfo> GetAll()
        {
            var buildingInfos = new List<BuildingInfo>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT * FROM BuildingInfo";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var buildingInfo = new BuildingInfo
                            {
                                Id = reader.GetInt32(0),
                                BuildingName = reader.GetString(1),
                                RoomNumber = reader.GetString(2),
                                Structure = reader.GetString(3),
                                Address = reader.GetString(4),
                                Area = reader.GetDouble(5),
                                ProjectHistory = reader.GetString(6),
                                DrawingPdf = (byte[])reader.GetValue(7)
                            };

                            buildingInfos.Add(buildingInfo);
                        }
                    }
                }
            }

            return buildingInfos;
        }
    }

}


