using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly string overpassApiEndpoint = "http://overpass-api.de/api/interpreter?data=";

        public async Task<IActionResult> Index()
        {
            try
            {
                var schoolsQuery = @"
            [out:json];
            (
                area['ISO3166-1'='MK'][admin_level=2];
                node['amenity'='school'](area);
            );
            out body;>;
        ";
                var schoolsResponse = await QueryOverpassApi(schoolsQuery);
                var parsedSchools = JsonConvert.DeserializeObject<OverpassApiSchoolResponse>(schoolsResponse);

                ViewData["Schools"] = parsedSchools?.Elements;

                return View("~/Views/Map/Map.cshtml");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        public IActionResult About()
        {
            return View();
        }


        private async Task<string> QueryOverpassApi(string query)
        {
            using (HttpClient client = new HttpClient())
            {
                string fullQuery = overpassApiEndpoint + query;
                HttpResponseMessage response = await client.GetAsync(fullQuery);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine($"Error: HTTP status code {response.StatusCode}");
                    // Log error details
                    return null;
                }
            }
        }

        public class OverpassApiSchoolResponse
        {
            //public string Version { get; set; }
            //public string Generator { get; set; }
            [JsonPropertyName("elements")]
            public List<School> Elements { get; set; }
        }

    }
}
