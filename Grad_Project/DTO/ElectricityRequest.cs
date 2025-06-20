using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grad_Project.DTOs
{
    public class ElectricityRequest
    {
        [Required(ErrorMessage = "Season is required")]
        [RegularExpression("^(summer|winter)$", ErrorMessage = "Invalid season value. Allowed: 'summer' or 'winter'")]
        public string Season { get; set; }

        [Required(ErrorMessage = "Answers are required")]
        [MinLength(1, ErrorMessage = "At least one answer is required")]
        public List<string> Answers { get; set; }

        [Range(0.1, 10000, ErrorMessage = "Amount must be between 0.1 and 10000")]
        public decimal? Amount { get; set; } 
    }
}
