﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_app.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author {  get; set; }
        [Required,Display(Name ="List Price"),Range(1,1000)]
        public int ListPrice { get; set; }

        [Required,Display(Name ="Price for 1-50"),Range(1,1000)]
        public int Price { get; set; }
        
        [Required,Display(Name ="Price for 50+"),Range(1,1000)]
        public int Price50 { get; set; }
        [Required,Display(Name ="Price for 100+"),Range(1,1000)]
        public int Price100 { get; set; }
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }


        [ValidateNever]
        public List<ProductImage>? ProductImages { get; set; }     
        [ValidateNever]
        public string ImgUrl { get; set; }
    }

}
