using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEvolve.AspNetCore.Localization
{
    public class SupportedCulturesCollection : ISupportedCulturesCollection
    {
        private readonly ImmutableList<CultureInfo> _supportedCultures = new[]
        {
            new CultureInfo("en"),
            new CultureInfo("fr"),
            new CultureInfo("he"),
            new CultureInfo("pt"),
            new CultureInfo("pt-BR"),
            new CultureInfo("es"),
            new CultureInfo("no"),
            new CultureInfo("nb"),
            new CultureInfo("zh"),
            new CultureInfo("zh-Hant"),
            new CultureInfo("zh-TW"),
            new CultureInfo("pl"),
        }.ToImmutableList();

        public CultureInfo this[int index] => ((IReadOnlyList<CultureInfo>)_supportedCultures)[index];

        public int Count => ((IReadOnlyCollection<CultureInfo>)_supportedCultures).Count;

        public IEnumerator<CultureInfo> GetEnumerator()
        {
            return ((IEnumerable<CultureInfo>)_supportedCultures).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_supportedCultures).GetEnumerator();
        }
    }
}
