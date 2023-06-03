﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBase
{
    public class RepairHistory
    {
        public int Id { get; set; }
        public string ProjectId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int BuildingInfoId { get; set; }
        public BuildingInfo BuildingInfo { get; set; }
        public Project Project { get; set; }
    }

}
