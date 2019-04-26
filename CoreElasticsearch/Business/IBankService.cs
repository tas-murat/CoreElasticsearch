using CoreElasticsearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreElasticsearch.Business
{
    interface IBankService
    {
        Task CreateIndexAsync(string indexName);
        Task<bool> IndexAsync(List<Bank> banks, string langCode);
       // Task<ProductSearchResponse> SearchAsync(string keyword, string langCode);
    }
}
