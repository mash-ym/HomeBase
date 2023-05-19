using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace HomeBase
{   

    public class RequestInfo
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ReferrerId { get; set; }
        public int BuildingInfoId { get; set; }
        public string RequestContent { get; set; }
        public DateTime EstimateDeadline { get; set; }
        public bool OnSiteSurvey { get; set; }
        public byte[] PhotoData { get; set; }
    }

    public class RequestInfoRepository
    {
        private string connectionString;

        public RequestInfoRepository(SQLiteConnection connection)
        {
            connectionString = connection.ConnectionString;
        }

        public void CreateRequestInfo(RequestInfo requestInfo)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO RequestInfo (id, customer_id, referrer_id, 
                            building_info_id, request_content, estimate_deadline, on_site_survey, photo_data)
                            VALUES (@RequestInfoId, @CustomerId, @ReferrerId, @BuildingInfoId, 
                            @RequestContent, @EstimateDeadline, @OnSiteSurvey, @PhotoData)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", requestInfo.Id);
                    command.Parameters.AddWithValue("@CustomerId", requestInfo.CustomerId);
                    command.Parameters.AddWithValue("@ReferrerId", requestInfo.ReferrerId);
                    command.Parameters.AddWithValue("@BuildingInfoId", requestInfo.BuildingInfoId);
                    command.Parameters.AddWithValue("@RequestContent", requestInfo.RequestContent);
                    command.Parameters.AddWithValue("@EstimateDeadline", requestInfo.EstimateDeadline);
                    command.Parameters.AddWithValue("@OnSiteSurvey", requestInfo.OnSiteSurvey);
                    command.Parameters.AddWithValue("@PhotoData", requestInfo.PhotoData);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Implement other CRUD operations as needed: Read, Update, Delete
        public void Insert(RequestInfo requestInfo)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO RequestInfo (customerId, referrerId, buildingInfoId, requestContent, estimateDeadline, onSiteSurvey, photoData) " +
                                              "VALUES (@customerId, @referrerId, @buildingInfoId, @requestContent, @estimateDeadline, @onSiteSurvey, @photoData)";
                        command.Parameters.AddWithValue("@CustomerId", requestInfo.CustomerId);
                        command.Parameters.AddWithValue("@ReferrerId", requestInfo.ReferrerId);
                        command.Parameters.AddWithValue("@BuildingInfoId", requestInfo.BuildingInfoId);
                        command.Parameters.AddWithValue("@RequestContent", requestInfo.RequestContent);
                        command.Parameters.AddWithValue("@EstimateDeadline", requestInfo.EstimateDeadline);
                        command.Parameters.AddWithValue("@OnSiteSurvey", requestInfo.OnSiteSurvey);
                        command.Parameters.AddWithValue("@PhotoData", requestInfo.PhotoData);

                        command.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException ex)
                {
                    // Handle SQLite-specific exceptions
                    Console.WriteLine("SQLiteException: " + ex.Message);
                    // Additional error handling code...

                    // Optionally, rethrow the exception or throw a custom exception
                    throw;
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    Console.WriteLine("Exception: " + ex.Message);
                    // Additional error handling code...

                    // Optionally, rethrow the exception or throw a custom exception
                    throw;
                }
            }
        }
        public RequestInfo GetById(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM RequestInfo WHERE id = @id";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var requestInfo = new RequestInfo
                            {
                                Id = reader.GetInt32(0),
                                CustomerId = reader.GetInt32(1),
                                ReferrerId = (int)(reader.IsDBNull(2) ? null : (int?)reader.GetInt32(2)),
                                BuildingInfoId = reader.GetInt32(3),
                                RequestContent = reader.GetString(4),
                                EstimateDeadline = reader.GetDateTime(5),
                                OnSiteSurvey = reader.GetBoolean(6),
                                PhotoData = reader.IsDBNull(7) ? null : (byte[])reader.GetValue(7)
                            };

                            return requestInfo;
                        }

                        return null;
                    }
                }
            }
        }

        public void Update(RequestInfo requestInfo)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE RequestInfo SET customer_id = @customerId, referrer_id = @referrerId, building_info_id = @buildingInfoId, " +
                                          "request_content = @requestContent, estimate_deadline = @estimateDeadline, on_site_survey = @onSiteSurvey, " +
                                          "photo_data = @photoData WHERE id = @id";
                    command.Parameters.AddWithValue("@customerId", requestInfo.CustomerId);
                    command.Parameters.AddWithValue("@referrerId", requestInfo.ReferrerId);
                    command.Parameters.AddWithValue("@buildingInfoId", requestInfo.BuildingInfoId);
                    command.Parameters.AddWithValue("@requestContent", requestInfo.RequestContent);
                    command.Parameters.AddWithValue("@estimateDeadline", requestInfo.EstimateDeadline);
                    command.Parameters.AddWithValue("@onSiteSurvey", requestInfo.OnSiteSurvey);
                    command.Parameters.AddWithValue("@photoData", requestInfo.PhotoData);
                    command.Parameters.AddWithValue("@id", requestInfo.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(long id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var commandText = "DELETE FROM subcontractor WHERE id = @Id";

                using (var command = new SQLiteCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<RequestInfo> GetAll()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM RequestInfo";

                    using (var reader = command.ExecuteReader())
                    {
                        List<RequestInfo> requestInfos = new List<RequestInfo>();

                        while (reader.Read())
                        {
                            var requestInfo = new RequestInfo
                            {
                                Id = reader.GetInt32(0),
                                CustomerId = reader.GetInt32(1),
                                ReferrerId = reader.GetInt32(2),
                                BuildingInfoId = reader.GetInt32(3),
                                RequestContent = reader.GetString(4),
                                EstimateDeadline = reader.GetDateTime(5),
                                OnSiteSurvey = reader.GetBoolean(6),
                                PhotoData = reader.IsDBNull(7) ? null : (byte[])reader.GetValue(7)
                            };

                            requestInfos.Add(requestInfo);
                        }
                        return requestInfos;
                    }
                }
            }
        }
    }
}