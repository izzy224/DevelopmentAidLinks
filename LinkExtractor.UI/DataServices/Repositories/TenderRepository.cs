using LinkExtractor.DAL;
using LinkExtractor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Repositories
{
    public class TenderRepository : GenericRepository<Tender, LinkExtractorDbContext>, ITenderRepository
    {
        public TenderRepository(LinkExtractorDbContext context) : base(context)
        {
        }

        public async Task AddListAsync(IList<Tender> tenders)
        {
            foreach(var tender in tenders)
            {
                await Context.Tenders.AddAsync(tender);
            }
        }
        public async Task<bool> HasUrlAsync(string url)
        {
            return await Context.Tenders.AnyAsync(x => x.Url == url);
        }
    }
}
