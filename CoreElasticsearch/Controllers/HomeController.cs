using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreElasticsearch.Models;
using Microsoft.Extensions.Localization;
using CoreElasticsearch.Business;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace CoreElasticsearch.Controllers
{
    public class HomeController : Controller
    {
        private readonly BankService _bankService;
        public HomeController(BankService bankService)
        {
            _bankService = bankService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            string indexName = $"banks";
            await FeedBanksInTurkish(indexName, "tr");
            await FeedBanksInEnglish(indexName, "en");

            return View();
        }
        public async Task<IActionResult> Banks(string keyword)
        {
            var requestCultureFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            CultureInfo culture = requestCultureFeature.RequestCulture.Culture;

            BankSearchResponse productSearchResponse = await _bankService.SearchAsync(keyword, culture.TwoLetterISOLanguageName);

            return View(productSearchResponse);
        }
        private async Task FeedBanksInTurkish(string indexName, string lang)
        {
            List<Bank> banks = new List<Bank>
            {
                new Bank
                {
                    ID = 1,
                    Name = "Banka kredi analist",
                    Description = "Banka kredi analist",
                    Experience = 6
                },
                new Bank
                {
                    ID = 2,
                    Name = "Banka kredi analist",
                    Description = "Banka kredi analist",
                    Experience = 7
                },
                new Bank
                {
                    ID = 3,
                    Name = "Banka İnsan Kaynakları",
                    Description = "Banka İnsan Kaynakları",
                    Experience = 3
                },
                new Bank
                {
                    ID = 4,
                    Name = "Banka İnsan Kaynakları",
                    Description = "Banka İnsan Kaynakları",
                    Experience = 5
                }
            };

            await _bankService.CreateIndexAsync($"{indexName}_{lang}");
            await _bankService.IndexAsync(banks, lang);
        }
        private async Task FeedBanksInEnglish(string indexName, string lang)
        {
            List<Bank> banks = new List<Bank>
            {
                new Bank
                {
                    ID = 1,
                    Name = "Bank credits analyst",
                    Description = "Bank credits analyst",
                    Experience = 6
                },
                new Bank
                {
                    ID = 2,
                    Name = "Bank credits analyst",
                    Description = "Bank credits analyst",
                    Experience = 7
                },
                new Bank
                {
                    ID = 3,
                    Name = "Bank Human Resources",
                    Description = "Bank Human Resources",
                    Experience = 3
                },
                new Bank
                {
                    ID = 4,
                    Name = "Bank Human Resources",
                    Description = "Bank Human Resources",
                    Experience = 5
                }
            };

            await _bankService.CreateIndexAsync($"{indexName}_{lang}");
            await _bankService.IndexAsync(banks, lang);
        }
    }
}
