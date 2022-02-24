using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesMVC.Models {
    public class Orderline {

        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Product { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(11,2)"), DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(11,2)"), DataType(DataType.Currency)]
        public decimal LineTotal { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
