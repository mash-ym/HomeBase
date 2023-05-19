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

        public void CreateConstructionRequest(ConstructionRequest constructionRequest)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO ConstructionRequest (request_id, estimate_id, item_name, cost_quantity, cost_unit, 
                            cost_unit_price, work_hours, start_date, subcontractor_id, site_contact, sales_contact, drawing_pdf)
                            VALUES (@RequestId, @EstimateId, @ItemName, @CostQuantity, @CostUnit, @CostUnitPrice, 
                            @WorkHours, @StartDate, @SubcontractorId, @SiteContact, @SalesContact, @DrawingPDF)";

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
                    command.Parameters.AddWithValue("@DrawingPDF", constructionRequest.DrawingPDF);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Implement other CRUD operations as needed: Read, Update, Delete
        public void InsertConstructionRequest(ConstructionRequest constructionRequest)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO ConstructionRequest (estimate_id, item_name, cost_quantity, cost_unit, cost_unit_price, work_hours, start_date, subcontractor_id, site_contact, sales_contact, drawing_pdf)
                                        VALUES (@EstimateId, @ItemName, @CostQuantity, @CostUnit, @CostUnitPrice, @WorkHours, @StartDate, @SubcontractorId, @SiteContact, @SalesContact, @DrawingPDF)";
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
                    command.Parameters.AddWithValue("@DrawingPDF", constructionRequest.DrawingPDF);

                    command.ExecuteNonQuery();
                }
            }
        }

        public ConstructionRequest GetConstructionRequestById(int constructionRequestId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM ConstructionRequest WHERE request_id = @ConstructionRequestId";
                    command.Parameters.AddWithValue("@ConstructionRequestId", constructionRequestId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ConstructionRequest
                            {
                                RequestId = reader.GetInt32(0),
                                EstimateId = reader.GetString(1),
                                ItemName = reader.GetString(2),
                                CostQuantity = reader.GetDouble(3),
                                CostUnit = reader.GetString(4),
                                CostUnitPrice = reader.GetDouble(5),
                                WorkHours = reader.GetDouble(6),
                                StartDate = reader.GetDateTime(7),
                                SubcontractorId = reader.GetInt32(8),
                                SiteContact = reader.GetString(9),
                                SalesContact = reader.GetString(10),
                                DrawingPDF = (byte[])reader.GetValue(11)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void UpdateConstructionRequest(ConstructionRequest constructionRequest)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE ConstructionRequest SET estimate_id = @EstimateId, item_name = @ItemName, cost_quantity = @CostQuantity, cost_unit = @CostUnit,
                                        cost_unit_price = @CostUnitPrice, work_hours = @WorkHours, start_date = @StartDate, subcontractor_id = @subcontractorId, site_contact = @SiteContact, sales_contact = @SalesContact, drawing_pdf = @DrawingPDF WHERE request_id = @RequestId";
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
                    command.Parameters.AddWithValue("@DrawingPDF", constructionRequest.DrawingPDF);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteConstructionRequest(int constructionRequestID)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM ConstructionRequest WHERE request_id = @ConstructionRequestID";
                    command.Parameters.AddWithValue("@ConstructionRequestID", constructionRequestID);

                    command.ExecuteNonQuery();
                }
            }
        }
    }

}
