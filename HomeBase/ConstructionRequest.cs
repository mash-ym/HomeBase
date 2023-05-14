using System;
using System.Data.SQLite;

namespace HomeBase
{
    public class ConstructionRequest
    {
        public int RequestId { get; set; }
        public string EstimateId { get; set; }
        public string ItemName { get; set; }
        public double CostQuantity { get; set; }
        public string CostUnit { get; set; }
        public double CostUnitPrice { get; set; }
        public double WorkHours { get; set; }
        public DateTime StartDate { get; set; }
        public int SubcontractorId { get; set; }
        public string SiteContact { get; set; }
        public string SalesContact { get; set; }
        public byte[] DrawingPdf { get; set; }
    }

    public class ConstructionRequestRepository
    {
        private string connectionString;

        public ConstructionRequestRepository(string dbPath)
        {
            connectionString = $"Data Source={dbPath};";
        }

        public void CreateConstructionRequest(ConstructionRequest constructionRequest)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO ConstructionRequest (request_id, estimate_id, item_name, cost_quantity, cost_unit, 
                            cost_unit_price, work_hours, start_date, subcontractor_id, site_contact, sales_contact, drawing_pdf)
                            VALUES (@RequestId, @EstimateId, @ItemName, @CostQuantity, @CostUnit, @CostUnitPrice, 
                            @WorkHours, @StartDate, @SubcontractorId, @SiteContact, @SalesContact, @DrawingPdf)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RequestId", constructionRequest.RequestId);
                    command.Parameters.AddWithValue("@EstimateId", constructionRequest.EstimateId);
                    command.Parameters.AddWithValue("@ItemName", constructionRequest.ItemName);
                    command.Parameters.AddWithValue("@CostQuantity", constructionRequest.CostQuantity);
                    command.Parameters.AddWithValue("@CostUnit", constructionRequest.CostUnit);
                    command.Parameters.AddWithValue("@CostUnitPrice", constructionRequest.CostUnitPrice);
                    command.Parameters.AddWithValue("@WorkHours", constructionRequest.WorkHours);
                    command.Parameters.AddWithValue("@StartDate", constructionRequest.StartDate);
                    command.Parameters.AddWithValue("@SubcontractorId", constructionRequest.SubcontractorId);
                    command.Parameters.AddWithValue("@SiteContact", constructionRequest.SiteContact);
                    command.Parameters.AddWithValue("@SalesContact", constructionRequest.SalesContact);
                    command.Parameters.AddWithValue("@DrawingPdf", constructionRequest.DrawingPdf);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Implement other CRUD operations as needed: Read, Update, Delete
    }

}
