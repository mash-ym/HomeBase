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
        public byte[] DrawingPDF { get; set; }
    }

    public class ConstructionRequestRepository
    {
        private string connectionString;

        public ConstructionRequestRepository(SQLiteConnection connection)
        {
            connectionString = connection.ConnectionString;
        }

        public void AddConstructionRequest(ConstructionRequest constructionRequest)
        {
            string insertQuery = @"
            INSERT INTO ConstructionRequest (
                estimate_id, item_name, cost_quantity, cost_unit, cost_unit_price,
                work_hours, start_date, subcontractor_id, site_contact, sales_contact, drawing_pdf
            ) VALUES (
                @EstimateId, @ItemName, @CostQuantity, @CostUnit, @CostUnitPrice,
                @WorkHours, @StartDate, @SubcontractorId, @SiteContact, @SalesContact, @DrawingPdf
            );
        ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
            {
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
                command.Parameters.AddWithValue("@DrawingPdf", constructionRequest.DrawingPDF);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public ConstructionRequest GetConstructionRequest(int requestId)
        {
            string selectQuery = "SELECT * FROM ConstructionRequest WHERE request_id = @RequestId;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@RequestId", requestId);

                connection.Open();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ConstructionRequest
                        {
                            RequestId = Convert.ToInt32(reader["request_id"]),
                            EstimateId = Convert.ToString(reader["estimate_id"]),
                            ItemName = Convert.ToString(reader["item_name"]),
                            CostQuantity = Convert.ToDouble(reader["cost_quantity"]),
                            CostUnit = Convert.ToString(reader["cost_unit"]),
                            CostUnitPrice = Convert.ToDouble(reader["cost_unit_price"]),
                            WorkHours = Convert.ToDouble(reader["work_hours"]),
                            StartDate = Convert.ToDateTime(reader["start_date"]),
                            SubcontractorId = Convert.ToInt32(reader["subcontractor_id"]),
                            SiteContact = Convert.ToString(reader["site_contact"]),
                            SalesContact = Convert.ToString(reader["sales_contact"]),
                            DrawingPDF = (byte[])reader["drawing_pdf"]
                        };
                    }
                }
            }

            return null;
        }

        public void UpdateConstructionRequest(ConstructionRequest constructionRequest)
        {
            string updateQuery = @"
            UPDATE ConstructionRequest SET
                estimate_id = @EstimateId,
                item_name = @ItemName,
                cost_quantity = @CostQuantity,
                cost_unit = @CostUnit,
                cost_unit_price = @CostUnitPrice,
                work_hours = @WorkHours,
                start_date = @StartDate,
                subcontractor_id = @SubcontractorId,
                site_contact = @SiteContact,
                sales_contact = @SalesContact,
                drawing_pdf = @DrawingPdf
            WHERE request_id = @RequestId;
        ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
            {
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
                command.Parameters.AddWithValue("@DrawingPdf", constructionRequest.DrawingPDF);
                command.Parameters.AddWithValue("@RequestId", constructionRequest.RequestId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteConstructionRequest(int requestId)
        {
            string deleteQuery = "DELETE FROM ConstructionRequest WHERE request_id = @RequestId;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@RequestId", requestId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
