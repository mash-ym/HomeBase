using System;
using System.Data.SQLite;

namespace HomeBase
{   

    public class RequestInfo
    {
        public int RequestInfoId { get; set; }
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

        public RequestInfoRepository(string dbPath)
        {
            connectionString = $"Data Source={dbPath};";
        }

        public void CreateRequestInfo(RequestInfo requestInfo)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO RequestInfo (request_info_id, customer_id, referrer_id, 
                            building_info_id, request_content, estimate_deadline, on_site_survey, photo_data)
                            VALUES (@RequestInfoId, @CustomerId, @ReferrerId, @BuildingInfoId, 
                            @RequestContent, @EstimateDeadline, @OnSiteSurvey, @PhotoData)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RequestInfoId", requestInfo.RequestInfoId);
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
    }

}
