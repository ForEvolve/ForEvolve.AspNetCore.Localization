using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebPagesSample.Pages
{
    // IgnoreAntiforgeryToken allows tests to post here without worrying about anti-forgery
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public ValidationModel Model { get; set; } = new ValidationModel();

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            var form = Request.Form;
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return Redirect("/?success=true");
        }

        public class ValidationModel
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
            public string Phone { get; set; } = "not a phone";
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
            public string Website { get; set; } = "Not an URL";
        }
    }
}
