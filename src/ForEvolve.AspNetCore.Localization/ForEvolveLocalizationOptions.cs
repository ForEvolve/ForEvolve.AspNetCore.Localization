using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEvolve.AspNetCore.Localization
{
    public class ForEvolveLocalizationOptions : LocalizationOptions
    {
        public ForEvolveMvcLocalizationOptions MvcOptions { get; set; }

        //public IList<CultureInfo> SupportedCultures { get; set; }
        //public CultureInfo DefaultCulture { get; set; }
        public RequestLocalizationOptions RequestLocalizationOptions { get; set; }
    }
}
