using System;
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

        public EstimateRepository(string dbPath)
        {
            connectionString = $"Data Source={dbPath};";
        }

        public void CreateEstimate(Estimate estimate)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Estimate (estimate_id, site_name, site_address, 
                            created_at, request_info_id, customer_info_id, building_info_id, 
                            issue_date, creator_id, total_amount, due_date, change_history, 
                            delivery_location, drawing_pdf)
                            VALUES (@EstimateId, @SiteName, @SiteAddress, @CreatedAt, 
                            @RequestInfoId, @CustomerInfoId, @BuildingInfoId, @IssueDate, 
                            @CreatorId, @TotalAmount, @DueDate, @ChangeHistory, 
                            @DeliveryLocation, @DrawingPdf)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EstimateId", estimate.EstimateId);
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
                    command.Parameters.AddWithValue("@DrawingPdf", estimate.DrawingPdf);

                    command.ExecuteNonQuery();
                }
            }
        }
        private string GenerateUniqueEstimationNumber()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM estimation";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    int rowCount = Convert.ToInt32(command.ExecuteScalar());

                    // Generate a unique 5-digit estimation number based on the row count
                    string estimationNumber = (rowCount + 1).ToString("D5");
                    return estimationNumber;
                }
            }
        }
        // Read (Get) an estimate by its ID
        public Estimate GetEstimateById(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM estimation WHERE estimate_id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapToEstimate(reader);
                        }
                    }
                }
            }

            return null; // Return null if estimate with the given ID is not found
        }

        // Update an existing estimate
        public void UpdateEstimate(Estimate estimate)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE estimation SET site_name = @siteName, site_address = @siteAddress, " +
                               "created_at = @createdAt, request_info_id = @requestInfoId, customer_info_id = @customerInfoId, " +
                               "building_info_id = @buildingInfoId, issue_date = @issueDate, creator_id = @creatorId, " +
                               "total_amount = @totalAmount, due_date = @dueDate, change_history = @changeHistory, " +
                               "delivery_location = @deliveryLocation, drawing_pdf = @drawingPdf " +
                               "WHERE estimate_id = @id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@siteName", estimate.SiteName);
                    command.Parameters.AddWithValue("@siteAddress", estimate.SiteAddress);
                    command.Parameters.AddWithValue("@createdAt", estimate.CreatedAt);
                    command.Parameters.AddWithValue("@requestInfoId", estimate.RequestInfoId);
                    command.Parameters.AddWithValue("@customerInfoId", estimate.CustomerInfoId);
                    command.Parameters.AddWithValue("@buildingInfoId", estimate.BuildingInfoId);
                    command.Parameters.AddWithValue("@issueDate", estimate.IssueDate);
                    command.Parameters.AddWithValue("@creatorId", estimate.CreatorId);
                    command.Parameters.AddWithValue("@totalAmount", estimate.TotalAmount);
                    command.Parameters.AddWithValue("@dueDate", estimate.DueDate);
                    command.Parameters.AddWithValue("@changeHistory", estimate.ChangeHistory);
                    command.Parameters.AddWithValue("@deliveryLocation", estimate.DeliveryLocation);
                    command.Parameters.AddWithValue("@drawingPdf", estimate.DrawingPdf);
                    command.Parameters.AddWithValue("@id", estimate.EstimateId);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Delete an estimate by its ID
        public void DeleteEstimate(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM estimation WHERE estimate_id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Helper method to map a SQLiteDataReader to an Estimate object
        private Estimate MapToEstimate(SQLiteDataReader reader)
        {
            Estimate estimate = new Estimate();
            estimate.EstimateId = Convert.ToInt32(reader["estimate_id"]);
            estimate.SiteName = reader["site_name"].ToString();
            estimate.SiteAddress = reader["site_address"].ToString();
            estimate.CreatedAt = Convert.ToDateTime(reader["created_at"]);
            estimate.RequestInfoId = Convert.ToInt32(reader["request_info_id"]);
            estimate.CustomerInfoId = Convert.ToInt32(reader["customer_info_id"]);
            estimate.BuildingInfoId = Convert.ToInt32(reader["building_info_id"]);
            estimate.IssueDate = Convert.ToDateTime(reader["issue_date"]);
            estimate.CreatorId = Convert.ToInt32(reader["creator_id"]);
            estimate.TotalAmount = Convert.ToDecimal(reader["total_amount"]);
            estimate.DueDate = Convert.ToDateTime(reader["due_date"]);
            estimate.ChangeHistory = reader["change_history"].ToString();
            estimate.DeliveryLocation = reader["delivery_location"].ToString();
            estimate.DrawingPdf = (byte[])reader["drawing_pdf"];

            return estimate;
        }
    }
}
