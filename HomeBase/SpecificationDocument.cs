using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
