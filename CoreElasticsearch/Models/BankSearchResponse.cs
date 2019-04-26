using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreElasticsearch.Models
{
    public class BankSearchResponse
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public IEnumerable<Bank> Banks { get; set; }
    }
}
