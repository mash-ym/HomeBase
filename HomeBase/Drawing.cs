using System;
using System.Collections.Generic;
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

}
