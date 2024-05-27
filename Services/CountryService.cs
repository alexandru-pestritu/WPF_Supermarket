using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WPF_Supermarket.Services
{
    public class CountryService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<string>> GetAllCountriesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("https://countriesnow.space/api/v0.1/countries/capital");
            response.EnsureSuccessStatusCode();

            var countriesJson = await response.Content.ReadAsStringAsync();
            var countryResponse = JsonConvert.DeserializeObject<CountryResponse>(countriesJson);

            List<string> countryNames = new List<string>();
            foreach (var country in countryResponse.Data)
            {
                countryNames.Add(country.Name);
            }

            countryNames.Sort();
            return countryNames;
        }
    }

    public class CountryResponse
    {
        public bool Error { get; set; }
        public string Msg { get; set; }
        public List<Country> Data { get; set; }
    }

    public class Country
    {
        public string Name { get; set; }
    }
}
