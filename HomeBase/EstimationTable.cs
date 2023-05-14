using System;
using System.Data.SQLite;

namespace HomeBase
{
    public class EstimationTable
    {
        private SQLiteConnection connection;

        public EstimationTable(string connectionString)
        {
            connection = new SQLiteConnection(connectionString);
        }

        public void CreateTable()
        {
            connection.Open();

            string sql = @"CREATE TABLE IF NOT EXISTS estimation (
                            id INTEGER PRIMARY KEY,
                            estimation_number TEXT UNIQUE,
                            site_name TEXT,
                            site_address TEXT,
                            created_at DATETIME,
                            request_info_id INTEGER,
                            customer_info_id INTEGER,
                            building_info_id INTEGER,
                            issued_date DATE,
                            creator_id INTEGER,
                            total_amount REAL,
                            deadline DATE,
                            revision_history TEXT,
                            delivery_location TEXT,
                            drawing_pdf BLOB
                        )";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        public void InsertEstimation(string estimationNumber, string siteName, string siteAddress, DateTime createdAt, int requestInfoId, int customerInfoId, int buildingInfoId, DateTime issuedDate, int creatorId, double totalAmount, DateTime deadline, string revisionHistory, string deliveryLocation, byte[] drawingPdf)
        {
            connection.Open();

            string sql = @"INSERT INTO estimation (
                            estimation_number,
                            site_name,
                            site_address,
                            created_at,
                            request_info_id,
                            customer_info_id,
                            building_info_id,
                            issued_date,
                            creator_id,
                            total_amount,
                            deadline,
                            revision_history,
                            delivery_location,
                            drawing_pdf
                        ) VALUES (
                            @EstimationNumber,
                            @SiteName,
                            @SiteAddress,
                            @CreatedAt,
                            @RequestInfoId,
                            @CustomerInfoId,
                            @BuildingInfoId,
                            @IssuedDate,
                            @CreatorId,
                            @TotalAmount,
                            @Deadline,
                            @RevisionHistory,
                            @DeliveryLocation,
                            @DrawingPdf
                        )";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@EstimationNumber", estimationNumber);
                command.Parameters.AddWithValue("@SiteName", siteName);
                command.Parameters.AddWithValue("@SiteAddress", siteAddress);
                command.Parameters.AddWithValue("@CreatedAt", createdAt);
                command.Parameters.AddWithValue("@RequestInfoId", requestInfoId);
                command.Parameters.AddWithValue("@CustomerInfoId", customerInfoId);
                command.Parameters.AddWithValue("@BuildingInfoId", buildingInfoId);
                command.Parameters.AddWithValue("@IssuedDate", issuedDate);
                command.Parameters.AddWithValue("@CreatorId", creatorId);
                command.Parameters.AddWithValue("@TotalAmount", totalAmount);
                command.Parameters.AddWithValue("@Deadline", deadline);
                command.Parameters.AddWithValue("@RevisionHistory", revisionHistory);
                command.Parameters.AddWithValue("@DeliveryLocation", deliveryLocation);
                command.Parameters.AddWithValue("@DrawingPdf", drawingPdf);

                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

}
