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

        public void AddRequestInfo(RequestInfo requestInfo)
        {
            string insertQuery = @"
            INSERT INTO RequestInfo (customer_id, referrer_id, building_info_id, request_content, estimate_deadline, on_site_survey, photo_data)
            VALUES (@CustomerId, @ReferrerId, @BuildingInfoId, @RequestContent, @EstimateDeadline, @OnSiteSurvey, @PhotoData);
            ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@CustomerId", requestInfo.CustomerId);
                command.Parameters.AddWithValue("@ReferrerId", requestInfo.ReferrerId);
                command.Parameters.AddWithValue("@BuildingInfoId", requestInfo.BuildingInfoId);
                command.Parameters.AddWithValue("@RequestContent", requestInfo.RequestContent);
                command.Parameters.AddWithValue("@EstimateDeadline", requestInfo.EstimateDeadline);
                command.Parameters.AddWithValue("@OnSiteSurvey", requestInfo.OnSiteSurvey);
                command.Parameters.AddWithValue("@PhotoData", requestInfo.PhotoData);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<RequestInfo> GetRequestInfos()
        {
            List<RequestInfo> requestInfos = new List<RequestInfo>();

            string selectQuery = "SELECT * FROM RequestInfo;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
            {
                connection.Open();

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    RequestInfo requestInfo = new RequestInfo
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        CustomerId = Convert.ToInt32(reader["customer_id"]),
                        ReferrerId = Convert.ToInt32(reader["referrer_id"]),
                        BuildingInfoId = Convert.ToInt32(reader["building_info_id"]),
                        RequestContent = reader["request_content"].ToString(),
                        EstimateDeadline = Convert.ToDateTime(reader["estimate_deadline"]),
                        OnSiteSurvey = Convert.ToBoolean(reader["on_site_survey"]),
                        PhotoData = (byte[])reader["photo_data"]
                    };

                    requestInfos.Add(requestInfo);
                }
            }

            return requestInfos;
        }

        public void DeleteRequestInfo(int id)
        {
            string deleteQuery = "DELETE FROM RequestInfo WHERE id = @Id;";

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