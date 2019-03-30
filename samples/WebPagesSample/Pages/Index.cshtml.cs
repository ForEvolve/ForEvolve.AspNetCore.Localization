using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebPagesSample.Pages
{
    public class IndexModel : PageModel
    {
        [Compare(nameof(Compare2))]
        public string Compare1 { get; set; } = "Some value";
        public string Compare2 { get; set; } = "Some non-matching value";
        [CreditCard]
        public string CreditCard { get; set; } = "Not a credit card";
        [EmailAddress]
        public string EmailAddress { get; set; } = "not an email address";
        [FileExtensions]
        public string FileExtensions { get; set; } = "toto.unknown";
        [MaxLength(5)]
        public string MaxLength { get; set; } = "ABCDEF";
        [MinLength(5)]
        public string MinLength { get; set; } = "ABC";
        [Phone]
        public string Phone { get; set; }
        [Range(10, 20)]
        public int Range { get; set; } = 30;
        [RegularExpression("[a-z]")]
        public string RegularExpression { get; set; } = "1";
        [Required]
        public string Required { get; set; }
        [StringLength(5)]
        public string StringLength { get; set; } = "ABCDEF";
        [StringLength(5, MinimumLength = 3)]
        public string StringLengthIncludingMinimum { get; set; } = "AB";
        [Url]
        public string Url { get; set; } = "Not an URL";

        public void OnGet()
        {

        }
    }
}
