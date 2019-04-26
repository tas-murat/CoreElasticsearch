using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreElasticsearch.Models;
using Nest;

namespace CoreElasticsearch.Business
{
    public class BankService : IBankService
    {
        ElasticClient elasticClient;
        public BankService(ConnectionSettings connectionSettings)
        {
            elasticClient = new ElasticClient(connectionSettings);
        }
        public async Task CreateIndexAsync(string indexName)
        {
            var createIndexDescriptor = new CreateIndexDescriptor(indexName.ToLowerInvariant())
                                        .Mappings(m => m.Map<Bank>(p => p.AutoMap()));

            await elasticClient.CreateIndexAsync(createIndexDescriptor);
        }

        public async Task<bool> IndexAsync(List<Bank> banks, string langCode)
        {
            string indexName = $"products_{langCode}";

            IBulkResponse response = await elasticClient.IndexManyAsync(banks, indexName);

            return response.IsValid;
        }
        public async Task<BankSearchResponse> SearchAsync(string keyword, string langCode)
        {
            BankSearchResponse bankSearchResponse = new BankSearchResponse();
            string indexName = $"products_{langCode}";

            ISearchResponse<Bank> searchResponse = await elasticClient.SearchAsync<Bank>(x => x
                .Index(indexName)
                .Query(q =>
                            q.MultiMatch(mp => mp
                                        .Query(keyword)
                                        .Fields(f => f.Fields(f1 => f1.Name, f2 => f2.Description)))
                ));

            if (searchResponse.IsValid && searchResponse.Documents != null)
            {
                bankSearchResponse.Total = (int)searchResponse.Total;
                bankSearchResponse.Banks = searchResponse.Documents;
            }

            return bankSearchResponse;
        }
    }
}
