using System;
using System.Collections.Generic;

namespace BoardTab.Models
{
    public class Equipment
    {
        public string Number { get; set; }

        public Int64 PlanCount { get; set; }

        public Int64 InStockCount { get; set; } 

        public string Remarks { get; set; }
    }
    public class EquipmentTemp : Equipment
    {
        public string Percent { get; set; }

        public Int64 UnFinished { get; set; }
        public int LAY_TABLE_INDEX { get; set; }
    }

    
    public class PlanInfo
    {
        public string CreateTime { get; set; }

        public string ModuleA { get; set; }

        public Int64 ModuleAPlanCount { get; set; }

        public Int64 ModuleAInStockCount { get; set; }

        public string ModuleACustomer { get; set; }

        public string ModuleB { get; set; }

        public Int64 ModuleBPlanCount { get; set; }

        public Int64 ModuleBInStockCount { get; set; }

        public string ModuleBCustomer { get; set; }
    }

    public class PlanInfoTemp: PlanInfo
    {
        public int LAY_TABLE_INDEX { get; set; }
    }

    public class PlanInfoPlus
    {
        public string CreateTimeStr { get; set; }
        public DateTime CreateTime { get; set; }

        public string ModuleA { get; set; }

        public Int64 ModuleAPlanCount { get; set; }

        public Int64 ModuleAInStockCount { get; set; }

        public string ModuleACustomer { get; set; }

        public string ModuleB { get; set; }

        public Int64 ModuleBPlanCount { get; set; }

        public Int64 ModuleBInStockCount { get; set; }

        public string ModuleBCustomer { get; set; }
    }
    public class PlanInfoInView
    {
        public List<string> TimeList { get; set; }
     

        public List<string> ModuleA { get; set; }

    

        public List<string> ModuleACount { get; set; }

        public List<string> ModuleACustomer { get; set; }

        public List<string> ModuleB { get; set; }

        public List<string> ModuleBCount { get; set; }
         

        public List<string> ModuleBCustomer { get; set; }

        public string Message { get; set; }
    }
    public class CountTemp
    { 
        
    }
}
