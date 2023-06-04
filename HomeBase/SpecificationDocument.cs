using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace HomeBase
{
    public class SpecificationDocument
    {
        public int DocumentId { get; set; } // 仕様書のID
        public string DocumentName { get; set; } // 仕様書の名前
        public string DocumentType { get; set; } // 仕様書の種類
        public int ProjectId { get; set; } // プロジェクトのID
        public string ItemName { get; set; } // 項目名
        public string Specification { get; set; } // 仕様
        public string ConstructionRoom { get; set; } // 部屋名
        public string ConstructionSection { get; set; } // 部位名
        public int DetailId { get; set; } // 詳細ID
        public DateTime ConstructionDate { get; set; } // 工事日
        public int ServiceLife { get; set; } // 使用寿命

        public SpecificationDocument(int documentId, string documentName, string documentType, int projectId,
            string itemName, string specification, string constructionRoom, string constructionSection,
            int detailId, DateTime constructionDate, int serviceLife)
        {
            DocumentId = documentId;
            DocumentName = documentName;
            DocumentType = documentType;
            ProjectId = projectId;
            ItemName = itemName;
            Specification = specification;
            ConstructionRoom = constructionRoom;
            ConstructionSection = constructionSection;
            DetailId = detailId;
            ConstructionDate = constructionDate;
            ServiceLife = serviceLife;
        }
    }

    public class SpecificationDocumentRepository
    {
        private readonly DBManager _dbManager;

        public SpecificationDocumentRepository(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        public void InsertSpecificationDocument(SpecificationDocument document, int estimateId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    // SpecificationDocument テーブルへの挿入クエリの作成
                    command.CommandText = @"
                        INSERT INTO SpecificationDocument (DocumentName, DocumentType, ProjectId, ItemName,
                                                          Specification, ConstructionRoom, ConstructionSection,
                                                          DetailId, ConstructionDate, ServiceLife)
                        VALUES (@DocumentName, @DocumentType, @ProjectId, @ItemName,
                                @Specification, @ConstructionRoom, @ConstructionSection,
                                @DetailId, @ConstructionDate, @ServiceLife)";

                    // パラメータの設定
                    command.Parameters.AddWithValue("@DocumentName", document.DocumentName);
                    command.Parameters.AddWithValue("@DocumentType", document.DocumentType);
                    command.Parameters.AddWithValue("@ProjectId", document.ProjectId);
                    command.Parameters.AddWithValue("@ItemName", document.ItemName);
                    command.Parameters.AddWithValue("@Specification", document.Specification);
                    command.Parameters.AddWithValue("@ConstructionRoom", document.ConstructionRoom);
                    command.Parameters.AddWithValue("@ConstructionSection", document.ConstructionSection);
                    command.Parameters.AddWithValue("@DetailId", document.DetailId);
                    command.Parameters.AddWithValue("@ConstructionDate", document.ConstructionDate);
                    command.Parameters.AddWithValue("@ServiceLife", document.ServiceLife);

                    // クエリの実行
                    command.ExecuteNonQuery();

                    // トランザクションのコミット
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // トランザクションのロールバック
                    transaction.Rollback();
                    ErrorHandler.ShowErrorMessage("データの挿入エラー", ex);
                }
            }
        }

        public void UpdateSpecificationDocument(SpecificationDocument document)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    // SpecificationDocument テーブルの更新クエリの作成
                    command.CommandText = @"
                        UPDATE SpecificationDocument
                        SET DocumentName = @DocumentName,
                            DocumentType = @DocumentType,
                            ProjectId = @ProjectId,
                            ItemName = @ItemName,
                            Specification = @Specification,
                            ConstructionRoom = @ConstructionRoom,
                            ConstructionSection = @ConstructionSection,
                            DetailId = @DetailId,
                            ConstructionDate = @ConstructionDate,
                            ServiceLife = @ServiceLife
                        WHERE DocumentId = @DocumentId";

                    // パラメータの設定
                    command.Parameters.AddWithValue("@DocumentName", document.DocumentName);
                    command.Parameters.AddWithValue("@DocumentType", document.DocumentType);
                    command.Parameters.AddWithValue("@ProjectId", document.ProjectId);
                    command.Parameters.AddWithValue("@ItemName", document.ItemName);
                    command.Parameters.AddWithValue("@Specification", document.Specification);
                    command.Parameters.AddWithValue("@ConstructionRoom", document.ConstructionRoom);
                    command.Parameters.AddWithValue("@ConstructionSection", document.ConstructionSection);
                    command.Parameters.AddWithValue("@DetailId", document.DetailId);
                    command.Parameters.AddWithValue("@ConstructionDate", document.ConstructionDate);
                    command.Parameters.AddWithValue("@ServiceLife", document.ServiceLife);
                    command.Parameters.AddWithValue("@DocumentId", document.DocumentId);

                    // クエリの実行
                    command.ExecuteNonQuery();

                    // トランザクションのコミット
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // トランザクションのロールバック
                    transaction.Rollback();
                    ErrorHandler.ShowErrorMessage("データの更新エラー", ex);
                }
            }
        }

        public void DeleteSpecificationDocument(int documentId)
        {
            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            using (SQLiteTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    // SpecificationDocument テーブルからの削除クエリの作成
                    command.CommandText = @"
                DELETE FROM SpecificationDocument
                WHERE DocumentId = @DocumentId";

                    // パラメータの設定
                    command.Parameters.AddWithValue("@DocumentId", documentId);

                    // クエリの実行
                    command.ExecuteNonQuery();

                    // トランザクションのコミット
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // トランザクションのロールバック
                    transaction.Rollback();
                    ErrorHandler.ShowErrorMessage("データの削除エラー", ex);
                }
            }
        }

        public List<SpecificationDocument> GetAllSpecificationDocuments()
        {
            List<SpecificationDocument> documents = new List<SpecificationDocument>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                // SpecificationDocument テーブルからのデータ取得クエリの作成
                command.CommandText = @"
                    SELECT DocumentId, DocumentName, DocumentType, ProjectId, ItemName, Specification,
                           ConstructionRoom, ConstructionSection, DetailId, ConstructionDate, ServiceLife
                    FROM SpecificationDocument";

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // 仕様書のデータを読み取り、SpecificationDocument オブジェクトに変換してリストに追加
                        int documentId = reader.GetInt32(0);
                        string documentName = reader.GetString(1);
                        string documentType = reader.GetString(2);
                        int projectId = reader.GetInt32(3);
                        string itemName = reader.GetString(4);
                        string specification = reader.GetString(5);
                        string constructionRoom = reader.GetString(6);
                        string constructionSection = reader.GetString(7);
                        int detailId = reader.GetInt32(8);
                        DateTime constructionDate = reader.GetDateTime(9);
                        int serviceLife = reader.GetInt32(10);

                        SpecificationDocument document = new SpecificationDocument(documentId, documentName, documentType,
                            projectId, itemName, specification, constructionRoom, constructionSection, detailId,
                            constructionDate, serviceLife);

                        documents.Add(document);
                    }
                }
            }

            return documents;
        }

        public SpecificationDocument GetSpecificationDocumentById(int documentId)
        {
            SpecificationDocument document = null;

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                // SpecificationDocument テーブルから指定されたIDのデータを取得するクエリの作成
                command.CommandText = @"
                    SELECT DocumentId, DocumentName, DocumentType, ProjectId, ItemName, Specification,
                           ConstructionRoom, ConstructionSection, DetailId, ConstructionDate, ServiceLife
                    FROM SpecificationDocument
                    WHERE DocumentId = @DocumentId";

                command.Parameters.AddWithValue("@DocumentId", documentId);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // 仕様書のデータを読み取り、SpecificationDocument オブジェクトに変換
                        string documentName = reader.GetString(1);
                        string documentType = reader.GetString(2);
                        int projectId = reader.GetInt32(3);
                        string itemName = reader.GetString(4);
                        string specification = reader.GetString(5);
                        string constructionRoom = reader.GetString(6);
                        string constructionSection = reader.GetString(7);
                        int detailId = reader.GetInt32(8);
                        DateTime constructionDate = reader.GetDateTime(9);
                        int serviceLife = reader.GetInt32(10);

                        document = new SpecificationDocument(documentId, documentName, documentType,
                            projectId, itemName, specification, constructionRoom, constructionSection, detailId,
                            constructionDate, serviceLife);
                    }
                }
            }

            return document;
        }

        public List<SpecificationDocument> SearchSpecificationDocument(string keyword)
        {
            List<SpecificationDocument> documents = new List<SpecificationDocument>();

            using (SQLiteConnection connection = _dbManager.Connection)
            using (SQLiteCommand command = connection.CreateCommand())
            {
                // SpecificationDocument テーブルからキーワードに基づいてデータを検索するクエリの作成
                command.CommandText = @"
            SELECT DocumentId, DocumentName, DocumentType, ProjectId, ItemName, Specification,
                   ConstructionRoom, ConstructionSection, DetailId, ConstructionDate, ServiceLife
            FROM SpecificationDocument
            WHERE DocumentName LIKE '%' || @Keyword || '%' OR ItemName LIKE '%' || @Keyword || '%'";

                command.Parameters.AddWithValue("@Keyword", keyword);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // 検索結果の仕様書データを読み取り、SpecificationDocument オブジェクトに変換
                        int documentId = reader.GetInt32(0);
                        string documentName = reader.GetString(1);
                        string documentType = reader.GetString(2);
                        int projectId = reader.GetInt32(3);
                        string itemName = reader.GetString(4);
                        string specification = reader.GetString(5);
                        string constructionRoom = reader.GetString(6);
                        string constructionSection = reader.GetString(7);
                        int detailId = reader.GetInt32(8);
                        DateTime constructionDate = reader.GetDateTime(9);
                        int serviceLife = reader.GetInt32(10);

                        SpecificationDocument document = new SpecificationDocument(documentId, documentName, documentType,
                            projectId, itemName, specification, constructionRoom, constructionSection, detailId,
                            constructionDate, serviceLife);

                        documents.Add(document);
                    }
                }
            }

            return documents;
        }


    }

}
