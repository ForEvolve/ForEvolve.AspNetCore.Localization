using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForEvolve.AspNetCore.Localization
{
    public interface ISupportedCulturesRepository
    {
        Task<IEnumerable<CultureInfo>> ReadAllAsync();
        Task<CultureInfo> ReadOneAsync(string name);
        Task<CultureInfo> FindDefaultAsync();
    }

    public class SupportedCulturesRepository : ISupportedCulturesRepository
    {
        public readonly IEnumerable<CultureInfo> _cultures;
        public SupportedCulturesRepository(IEnumerable<CultureInfo> cultures)
        {
            _cultures = cultures ?? throw new ArgumentNullException(nameof(cultures));
        }

        public Task<CultureInfo> FindDefaultAsync()
        {
            return Task.FromResult(_cultures.First());
        }

        public Task<IEnumerable<CultureInfo>> ReadAllAsync()
        {
            return Task.FromResult(_cultures);
        }

        public Task<CultureInfo> ReadOneAsync(string name)
        {
            return Task.FromResult(_cultures.FirstOrDefault(x => x.Name == name));
        }
    }
}
