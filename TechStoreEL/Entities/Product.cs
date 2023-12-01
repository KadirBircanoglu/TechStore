using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreEL.Entities
{
    public class Product:BaseNumeric<long>
    {
        public string SerialNumber { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set;}
        public  bool IsDeleted { get; set; }
    }
}
