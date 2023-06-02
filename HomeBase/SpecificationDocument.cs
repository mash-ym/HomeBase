using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBase
{
    public class SpecificationDocument
    {
        public string ItemName { get; set; }
        public string Specification { get; set; }
        public string ConstructionRoom { get; set; }
        public string ConstructionSection { get; set; }
        public int DetailId { get; set; }
        public DateTime ConstructionDate { get; set; }
        public int ServiceLife { get; set; }

        public SpecificationDocument(string itemName, string specification, string constructionRoom,
            string constructionSection, int detailId, DateTime constructionDate, int serviceLife)
        {
            ItemName = itemName;
            Specification = specification;
            ConstructionRoom = constructionRoom;
            ConstructionSection = constructionSection;
            DetailId = detailId;
            ConstructionDate = constructionDate;
            ServiceLife = serviceLife;
        }
    }

}
