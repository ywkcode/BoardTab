using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardTab.Models
{
    public class Product
    {
        public string CreateTime { get; set; }
        public string Orderer { get; set; }
        public string OrderName { get; set; }
        public string OrderNum { get; set; }
        public string ContractTime { get; set; }
        public string JichuNum { get; set; }
        public string HeGeNum { get; set; }
        public string ShaoLuTime { get; set; }
        public string ShaoLuNum { get; set; }
        public string ChuLuTime { get; set; }
        public string ChuLuAllNum { get; set; }
        public string ChuLuCPNum { get; set; }
        public string PackageNum { get; set; }
        public string SendTime { get; set; } 
        public string Reason { get; set; }
    }

    public class ProductTemp : Product
    {
        public int LAY_TABLE_INDEX { get; set; }
    }

    public class Settings
    { 
      

        public string Message { get; set; }

        public string TimeOut { get; set; }
    }
}
