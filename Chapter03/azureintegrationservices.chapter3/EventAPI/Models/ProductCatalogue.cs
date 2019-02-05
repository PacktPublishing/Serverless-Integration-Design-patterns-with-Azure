using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EventAPI.Models
{
    public class ProductCatalogue
    {
        [Required(ErrorMessage ="Product Id is requred")]
        public long Id { get; set; }
        [Required(ErrorMessage = "Product Name is requred")]
        public string Name { get; set; }
        public string productCategory { get; set; }
        public string productPrice { get; set; }
        public string productDescription { get; set; }
    }
}
