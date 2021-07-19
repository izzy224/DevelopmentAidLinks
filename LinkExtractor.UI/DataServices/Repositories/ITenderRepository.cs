using LinkExtractor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Repositories
{
    public interface ITenderRepository : IGenericRepository<Tender>
    {
        Task AddListAsync(IList<Tender> tenders);
        Task<bool> HasUrlAsync(string url);
    }
}
