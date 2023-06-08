using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBase
{
    public class Drawing
    {
        public int Id { get; set; }
        public string DrawingName { get; set; } //図面の名称
        public string Description { get; set; } //図面の説明
        public byte[] DrawingPdf { get; set; } //図面のPDFデータ
        public DateTime CreatedDate { get; set; } // 図面の作成日
        public int BuildingInfoId { get; set; } // 建物情報の外部キー
        public BuildingInfo BuildingInfo { get; set; }
    }

    public class DrawingRepository
    {
        private readonly DBManager _dbManager;

        public DrawingRepository(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public void InsertDrawing(Drawing drawing)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "INSERT INTO Drawing (DrawingName, Description, DrawingPdf, CreatedDate, BuildingInfoId) " +
                                          "VALUES (@DrawingName, @Description, @DrawingPdf, @CreatedDate, @BuildingInfoId)";
                    command.Parameters.AddWithValue("@DrawingName", drawing.DrawingName);
                    command.Parameters.AddWithValue("@Description", drawing.Description);
                    command.Parameters.AddWithValue("@DrawingPdf", drawing.DrawingPdf);
                    command.Parameters.AddWithValue("@CreatedDate", drawing.CreatedDate);
                    command.Parameters.AddWithValue("@BuildingInfoId", drawing.BuildingInfoId);

                    command.ExecuteNonQuery();

                    // 挿入したデータのIDを取得
                    command.CommandText = "SELECT last_insert_rowid();";
                    int drawingId = Convert.ToInt32(command.ExecuteScalar());

                    // DrawingオブジェクトにIDを設定
                    drawing.Id = drawingId;

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Failed to insert Drawing.", ex);
                }
            }
        }

        public void UpdateDrawing(Drawing drawing)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    command.CommandText = "UPDATE Drawing SET DrawingName = @DrawingName, Description = @Description, " +
                                          "DrawingPdf = @DrawingPdf, CreatedDate = @CreatedDate, BuildingInfoId = @BuildingInfoId " +
                                          "WHERE Id = @Id";
                    command.Parameters.AddWithValue("@DrawingName", drawing.DrawingName);
                    command.Parameters.AddWithValue("@Description", drawing.Description);
                    command.Parameters.AddWithValue("@DrawingPdf", drawing.DrawingPdf);
                    command.Parameters.AddWithValue("@CreatedDate", drawing.CreatedDate);
                    command.Parameters.AddWithValue("@BuildingInfoId", drawing.BuildingInfoId);
                    command.Parameters.AddWithValue("@Id", drawing.Id);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Failed to update Drawing. Drawing not found.");
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Failed to update Drawing.", ex);
                }
            }
        }

        public void DeleteDrawing(int drawingId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    // 削除する図面情報を取得
                    Drawing drawing = GetDrawingById(drawingId);

                    if (drawing != null)
                    {
                        // 図面情報を削除
                        command.CommandText = "DELETE FROM Drawing WHERE Id = @Id";
                        command.Parameters.AddWithValue("@Id", drawingId);
                        command.ExecuteNonQuery();

                        // 関連する建物情報の図面リストからも削除
                        BuildingInfo buildingInfo = drawing.BuildingInfo;
                        if (buildingInfo != null && buildingInfo.Drawings != null)
                        {
                            buildingInfo.Drawings.RemoveAll(d => d.Id == drawingId);
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Failed to delete Drawing.", ex);
                }
            }
        }

        public List<Drawing> GetAllDrawings()
        {
            List<Drawing> drawings = new List<Drawing>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Drawing";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Drawing drawing = new Drawing
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            DrawingName = Convert.ToString(reader["DrawingName"]),
                            Description = Convert.ToString(reader["Description"]),
                            DrawingPdf = (byte[])reader["DrawingPdf"],
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"])
                        };

                        drawings.Add(drawing);
                    }
                }
            }

            return drawings;
        }

        public Drawing GetDrawingById(int drawingId)
        {
            Drawing drawing = null;

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Drawing WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", drawingId);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        drawing = new Drawing
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            DrawingName = Convert.ToString(reader["DrawingName"]),
                            Description = Convert.ToString(reader["Description"]),
                            DrawingPdf = (byte[])reader["DrawingPdf"],
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"])
                        };
                    }
                }
            }

            return drawing;
        }

        public List<Drawing> SearchDrawing(string keyword)
        {
            List<Drawing> drawings = new List<Drawing>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Drawing WHERE DrawingName LIKE '%' || @Keyword || '%' OR Description LIKE '%' || @Keyword || '%'";
                command.Parameters.AddWithValue("@Keyword", keyword);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Drawing drawing = new Drawing
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            DrawingName = Convert.ToString(reader["DrawingName"]),
                            Description = Convert.ToString(reader["Description"]),
                            DrawingPdf = (byte[])reader["DrawingPdf"],
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            BuildingInfoId = Convert.ToInt32(reader["BuildingInfoId"])
                        };

                        drawings.Add(drawing);
                    }
                }
            }

            return drawings;
        }

    }
}

