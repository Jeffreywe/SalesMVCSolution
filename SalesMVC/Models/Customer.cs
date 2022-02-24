using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesMVC.Models {
    public class Customer {

        public int Id { get; set; }
        [Required, StringLength(30)]
        public string Name { get; set; }
        public bool Active { get; set; } = true;
        [Column(TypeName = "decimal(11,2)")]
        public decimal Sales { get; set; } = 0;
        [Required, StringLength(2)]
        public string StateCode { get; set; } = "OH";

    }
}
