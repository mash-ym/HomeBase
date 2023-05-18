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
        public int DetailId { get; set; }        // ID (detail_id) [Primary Key]
        public int EstimateId { get; set; }      // 見積番号 (estimate_id) [Foreign Key]
        public string ItemName { get; set; }     // 項目名 (item_name)
        public decimal CostQuantity { get; set; }     // 原価数量 (cost_quantity)
        public string CostUnit { get; set; }     // 原価単位 (cost_unit)
        public decimal CostUnitPrice { get; set; }    // 原価単価 (cost_unit_price)
        public decimal EstimateQuantity { get; set; }   // 見積数量 (estimate_quantity)
        public string EstimateUnit { get; set; }   // 見積単位 (estimate_unit)
        public decimal EstimateUnitPrice { get; set; }  // 見積単価 (estimate_unit_price)
        public string Specification { get; set; }  // 仕様 (specification)
        public decimal MarkupRate { get; set; }    // 掛率 (markup_rate)
        public string Remarks { get; set; }        // 備考 (remarks)
        public decimal WorkHours { get; set; }     // 工数 (work_hours)
        public int SubcontractorId { get; set; }   // 下請け業者ID (subcontractor_id)
        public int GroupNumber { get; set; }       // グループ番号 (group_number)
        public string GroupName { get; set; }      // グループ名 (group_name)
        public string Dependency { get; set; }     // 依存関係 (dependency)

        public EstimateDetail(int detailId, int estimateId, string itemName, int costQuantity, string costUnit, decimal costUnitPrice,
                          int estimateQuantity, string estimateUnit, decimal estimateUnitPrice, string specification,
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
            
        }

        public void CreateEstimateDetail(EstimateDetail estimateDetail)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO EstimateDetail (detail_id, estimate_id, item_name, cost_quantity, cost_unit, 
                            cost_unit_price, estimate_quantity, estimate_unit, estimate_unit_price, specification, 
                            markup_rate, remarks, work_hours, subcontractor_id, group_number, group_name, dependency)
                            VALUES (@DetailId, @EstimateId, @ItemName, @CostQuantity, @CostUnit, @CostUnitPrice, 
                            @EstimateQuantity, @EstimateUnit, @EstimateUnitPrice, @Specification, @MarkupRate, 
                            @Remarks, @WorkHours, @SubcontractorId, @GroupNumber, @GroupName, @Dependency)";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
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

                    command.ExecuteNonQuery();
                }
            }
        }

        // Implement other CRUD operations as needed: Read, Update, Delete
        public List<EstimateDetail> GetEstimateDetailsByEstimateID(int estimateID)
        {
            var estimateDetails = new List<EstimateDetail>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT * FROM estimation_detail WHERE estimate_id = @estimate_id";
                    command.Parameters.AddWithValue("@estimate_id", estimateID);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var detailID = reader.GetInt32(0);
                            var itemName = reader.GetString(2);
                            var costQuantity = reader.GetInt32(3);
                            var costUnit = reader.GetString(4);
                            var costUnitPrice = reader.GetDecimal(5);
                            var estimateQuantity = reader.GetInt32(6);
                            var estimateUnit = reader.GetString(7);
                            var estimateUnitPrice = reader.GetDecimal(8);
                            var specification = reader.GetString(9);
                            var markupRate = reader.GetDecimal(10);
                            var remarks = reader.GetString(11);
                            var workHours = reader.GetDecimal(12);
                            var subcontractorID = reader.GetInt32(13);
                            var groupNumber = reader.GetInt32(14);
                            var groupName = reader.GetString(15);
                            var dependency = reader.GetString(16);
                            var estimateDetail = new EstimateDetail(detailID, estimateID, itemName, costQuantity, costUnit, costUnitPrice,
                        estimateQuantity, estimateUnit, estimateUnitPrice, specification, markupRate, remarks, workHours,
                        subcontractorID, groupNumber, groupName, dependency);

                            estimateDetails.Add(estimateDetail);
                        }
                    }
                }
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

        public void DeleteEstimateDetail(int detailID)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM estimation_detail WHERE detail_id = @detail_id";
                    command.Parameters.AddWithValue("@detail_id", detailID);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
