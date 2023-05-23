using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace HomeBase
{
    public class Estimate
    {
        public int EstimateId { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RequestInfoId { get; set; }
        public int CustomerInfoId { get; set; }
        public int BuildingInfoId { get; set; }
        public DateTime IssueDate { get; set; }
        public int CreatorId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime DueDate { get; set; }
        public string ChangeHistory { get; set; }
        public string DeliveryLocation { get; set; }
        public byte[] DrawingPdf { get; set; }
    }

    public class EstimateRepository
    {
        private string connectionString;

        public EstimateRepository(SQLiteConnection connection)
        {
            connectionString = connection.ConnectionString;
        }

        public void AddEstimate(Estimate estimate)
        {
            string insertQuery = @"
            INSERT INTO Estimate (site_name, site_address, created_at, request_info_id, customer_info_id, building_info_id, issue_date, creator_id, total_amount, due_date, change_history, delivery_location)
            VALUES (@SiteName, @SiteAddress, @CreatedAt, @RequestInfoId, @CustomerInfoId, @BuildingInfoId, @IssueDate, @CreatorId, @TotalAmount, @DueDate, @ChangeHistory, @DeliveryLocation);
        ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@SiteName", estimate.SiteName);
                command.Parameters.AddWithValue("@SiteAddress", estimate.SiteAddress);
                command.Parameters.AddWithValue("@CreatedAt", estimate.CreatedAt);
                command.Parameters.AddWithValue("@RequestInfoId", estimate.RequestInfoId);
                command.Parameters.AddWithValue("@CustomerInfoId", estimate.CustomerInfoId);
                command.Parameters.AddWithValue("@BuildingInfoId", estimate.BuildingInfoId);
                command.Parameters.AddWithValue("@IssueDate", estimate.IssueDate);
                command.Parameters.AddWithValue("@CreatorId", estimate.CreatorId);
                command.Parameters.AddWithValue("@TotalAmount", estimate.TotalAmount);
                command.Parameters.AddWithValue("@DueDate", estimate.DueDate);
                command.Parameters.AddWithValue("@ChangeHistory", estimate.ChangeHistory);
                command.Parameters.AddWithValue("@DeliveryLocation", estimate.DeliveryLocation);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Estimate> GetAllEstimates()
        {
            string selectQuery = "SELECT * FROM Estimate;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
            {
                connection.Open();

                List<Estimate> estimateList = new List<Estimate>();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Estimate estimate = new Estimate()
                        {
                            EstimateId = reader.GetInt32(0),
                            SiteName = reader.GetString(1),
                            SiteAddress = reader.GetString(2),
                            CreatedAt = reader.GetDateTime(3),
                            RequestInfoId = reader.GetInt32(4),
                            CustomerInfoId = reader.GetInt32(5),
                            BuildingInfoId = reader.GetInt32(6),
                            IssueDate = reader.GetDateTime(7),
                            CreatorId = reader.GetInt32(8),
                            TotalAmount = reader.GetDecimal(9),
                            DueDate = reader.GetDateTime(10),
                            ChangeHistory = reader.GetString(11),
                            DeliveryLocation = reader.GetString(12)
                        };

                        estimateList.Add(estimate);
                    }
                }

                return estimateList;
            }
        }

        public void UpdateEstimate(Estimate estimate)
        {
            string updateQuery = @"
                UPDATE Estimate SET
                site_name = @SiteName,
                site_address = @SiteAddress,
                created_at = @CreatedAt,
                request_info_id = @RequestInfoId,
                customer_info_id = @CustomerInfoId,
                building_info_id = @BuildingInfoId,
                issue_date = @IssueDate,
                creator_id = @CreatorId,
                total_amount = @TotalAmount,
                due_date = @DueDate,
                change_history = @ChangeHistory,
                delivery_location = @DeliveryLocation
                WHERE estimate_id = @EstimateId;
            ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@SiteName", estimate.SiteName);
                command.Parameters.AddWithValue("@SiteAddress", estimate.SiteAddress);
                command.Parameters.AddWithValue("@CreatedAt", estimate.CreatedAt);
                command.Parameters.AddWithValue("@RequestInfoId", estimate.RequestInfoId);
                command.Parameters.AddWithValue("@CustomerInfoId", estimate.CustomerInfoId);
                command.Parameters.AddWithValue("@BuildingInfoId", estimate.BuildingInfoId);
                command.Parameters.AddWithValue("@IssueDate", estimate.IssueDate);
                command.Parameters.AddWithValue("@CreatorId", estimate.CreatorId);
                command.Parameters.AddWithValue("@TotalAmount", estimate.TotalAmount);
                command.Parameters.AddWithValue("@DueDate", estimate.DueDate);
                command.Parameters.AddWithValue("@ChangeHistory", estimate.ChangeHistory);
                command.Parameters.AddWithValue("@DeliveryLocation", estimate.DeliveryLocation);
                command.Parameters.AddWithValue("@EstimateId", estimate.EstimateId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteEstimate(int estimateId)
        {
            string deleteQuery = "DELETE FROM Estimate WHERE estimate_id = @Id;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@Id", estimateId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}

