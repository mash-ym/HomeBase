using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace HomeBase
{
    public class EstimateDetail
    {
        public int DetailId { get; set; }
        public int EstimateId { get; set; }
        public string ItemName { get; set; }
        public decimal CostQuantity { get; set; }
        public string CostUnit { get; set; }
        public decimal CostUnitPrice { get; set; }
        public decimal EstimateQuantity { get; set; }
        public string EstimateUnit { get; set; }
        public decimal EstimateUnitPrice { get; set; }
        public string Specification { get; set; }
        public decimal MarkupRate { get; set; }
        public string Remarks { get; set; }
        public decimal WorkHours { get; set; }
        public int SubcontractorId { get; set; }
        public int GroupNumber { get; set; }
        public string GroupName { get; set; }
        public string Dependency { get; set; }

        public EstimateDetail()
        {
            // デフォルトコンストラクタ
        }

        public EstimateDetail(int detailId, int estimateId, string itemName, decimal costQuantity, string costUnit, decimal costUnitPrice,
            decimal estimateQuantity, string estimateUnit, decimal estimateUnitPrice, string specification,
            decimal markupRate, string remarks, decimal workHours, int subcontractorId,
            int groupNumber, string groupName, string dependency)
        {
            DetailId = detailId;
            EstimateId = estimateId;
            ItemName = itemName;
            CostQuantity = costQuantity;
            CostUnit = costUnit;
            CostUnitPrice = costUnitPrice;
            EstimateQuantity = estimateQuantity;
            EstimateUnit = estimateUnit;
            EstimateUnitPrice = estimateUnitPrice;
            Specification = specification;
            MarkupRate = markupRate;
            Remarks = remarks;
            WorkHours = workHours;
            SubcontractorId = subcontractorId;
            GroupNumber = groupNumber;
            GroupName = groupName;
            Dependency = dependency;
        }
    }

    public class EstimateDetailRepository
    {
        private string connectionString;

        public EstimateDetailRepository(SQLiteConnection connection)
        {
            connectionString = connection.ConnectionString;
        }

        public void AddEstimateDetail(EstimateDetail estimateDetail)
        {
            string insertQuery = @"
        INSERT INTO EstimateDetail (
            detail_id,
            estimate_id,
            item_name,
            cost_quantity,
            cost_unit,
            cost_unit_price,
            estimate_quantity,
            estimate_unit,
            estimate_unit_price,
            specification,
            markup_rate,
            remarks,
            work_hours,
            subcontractor_id,
            group_number,
            group_name,
            dependency
        ) VALUES (
            @DetailId,
            @EstimateId,
            @ItemName,
            @CostQuantity,
            @CostUnit,
            @CostUnitPrice,
            @EstimateQuantity,
            @EstimateUnit,
            @EstimateUnitPrice,
            @Specification,
            @MarkupRate,
            @Remarks,
            @WorkHours,
            @SubcontractorId,
            @GroupNumber,
            @GroupName,
            @Dependency
        );
    ";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@DetailId", estimateDetail.DetailId);
                command.Parameters.AddWithValue("@EstimateId", estimateDetail.EstimateId);
                command.Parameters.AddWithValue("@ItemName", estimateDetail.ItemName);
                command.Parameters.AddWithValue("@CostQuantity", estimateDetail.CostQuantity);
                command.Parameters.AddWithValue("@CostUnit", estimateDetail.CostUnit);
                command.Parameters.AddWithValue("@CostUnitPrice", estimateDetail.CostUnitPrice);
                command.Parameters.AddWithValue("@EstimateQuantity", estimateDetail.EstimateQuantity);
                command.Parameters.AddWithValue("@EstimateUnit", estimateDetail.EstimateUnit);
                command.Parameters.AddWithValue("@EstimateUnitPrice", estimateDetail.EstimateUnitPrice);
                command.Parameters.AddWithValue("@Specification", estimateDetail.Specification);
                command.Parameters.AddWithValue("@MarkupRate", estimateDetail.MarkupRate);
                command.Parameters.AddWithValue("@Remarks", estimateDetail.Remarks);
                command.Parameters.AddWithValue("@WorkHours", estimateDetail.WorkHours);
                command.Parameters.AddWithValue("@SubcontractorId", estimateDetail.SubcontractorId);
                command.Parameters.AddWithValue("@GroupNumber", estimateDetail.GroupNumber);
                command.Parameters.AddWithValue("@GroupName", estimateDetail.GroupName);
                command.Parameters.AddWithValue("@Dependency", estimateDetail.Dependency);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        // Implement other CRUD operations as needed: Read, Update, Delete
        public List<EstimateDetail> GetEstimateDetailsByEstimateId(int estimateId)
        {
            List<EstimateDetail> estimateDetails = new List<EstimateDetail>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM EstimateDetail WHERE estimate_id = @estimateId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@estimateId", estimateId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EstimateDetail estimateDetail = new EstimateDetail();
                            estimateDetail.DetailId = reader.GetInt32(reader.GetOrdinal("detail_id"));
                            estimateDetail.EstimateId = reader.GetInt32(reader.GetOrdinal("estimate_id"));
                            estimateDetail.ItemName = reader.GetString(reader.GetOrdinal("item_name"));
                            estimateDetail.CostQuantity = reader.GetDecimal(reader.GetOrdinal("cost_quantity"));
                            estimateDetail.CostUnit = reader.GetString(reader.GetOrdinal("cost_unit"));
                            estimateDetail.CostUnitPrice = reader.GetDecimal(reader.GetOrdinal("cost_unit_price"));
                            estimateDetail.EstimateQuantity = reader.GetDecimal(reader.GetOrdinal("estimate_quantity"));
                            estimateDetail.EstimateUnit = reader.GetString(reader.GetOrdinal("estimate_unit"));
                            estimateDetail.EstimateUnitPrice = reader.GetDecimal(reader.GetOrdinal("estimate_unit_price"));
                            estimateDetail.Specification = reader.GetString(reader.GetOrdinal("specification"));
                            estimateDetail.MarkupRate = reader.GetDecimal(reader.GetOrdinal("markup_rate"));
                            estimateDetail.Remarks = reader.GetString(reader.GetOrdinal("remarks"));
                            estimateDetail.WorkHours = reader.GetDecimal(reader.GetOrdinal("work_hours"));
                            estimateDetail.SubcontractorId = reader.GetInt32(reader.GetOrdinal("subcontractor_id"));
                            estimateDetail.GroupNumber = reader.GetInt32(reader.GetOrdinal("group_number"));
                            estimateDetail.GroupName = reader.GetString(reader.GetOrdinal("group_name"));
                            estimateDetail.Dependency = reader.GetString(reader.GetOrdinal("dependency"));

                            estimateDetails.Add(estimateDetail);
                        }
                    }
                }

                connection.Close();
            }

            return estimateDetails;
        }


        public void UpdateEstimateDetail(EstimateDetail estimateDetail)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"UPDATE estimation_detail SET item_name = @item_name, cost_quantity = @cost_quantity,
                                    cost_unit = @cost_unit, cost_unit_price = @cost_unit_price, estimate_quantity = @estimate_quantity,
                                    estimate_unit = @estimate_unit, estimate_unit_price = @estimate_unit_price,
                                    specification = @specification, markup_rate = @markup_rate, remarks = @remarks,
                                    work_hours = @work_hours, subcontractor_id = @subcontractor_id, group_number = @group_number,
                                    group_name = @group_name, dependency = @dependency
                                    WHERE detail_id = @detail_id";
                    command.Parameters.AddWithValue("@detail_id", estimateDetail.DetailId);
                    command.Parameters.AddWithValue("@item_name", estimateDetail.ItemName);
                    command.Parameters.AddWithValue("@cost_quantity", estimateDetail.CostQuantity);
                    command.Parameters.AddWithValue("@cost_unit", estimateDetail.CostUnit);
                    command.Parameters.AddWithValue("@cost_unit_price", estimateDetail.CostUnitPrice);
                    command.Parameters.AddWithValue("@estimate_quantity", estimateDetail.EstimateQuantity);
                    command.Parameters.AddWithValue("@estimate_unit", estimateDetail.EstimateUnit);
                    command.Parameters.AddWithValue("@estimate_unit_price", estimateDetail.EstimateUnitPrice);
                    command.Parameters.AddWithValue("@specification", estimateDetail.Specification);
                    command.Parameters.AddWithValue("@markup_rate", estimateDetail.MarkupRate);
                    command.Parameters.AddWithValue("@remarks", estimateDetail.Remarks);
                    command.Parameters.AddWithValue("@work_hours", estimateDetail.WorkHours);
                    command.Parameters.AddWithValue("@subcontractor_id", estimateDetail.SubcontractorId);
                    command.Parameters.AddWithValue("@group_number", estimateDetail.GroupNumber);
                    command.Parameters.AddWithValue("@group_name", estimateDetail.GroupName);
                    command.Parameters.AddWithValue("@dependency", estimateDetail.Dependency);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteEstimateDetail(int detailId)
        {
            string deleteQuery = "DELETE FROM EstimateDetail WHERE detail_id = @DetailId;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@DetailId", detailId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
