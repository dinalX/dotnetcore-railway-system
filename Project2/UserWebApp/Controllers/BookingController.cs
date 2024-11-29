using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using UserWebApp.Models;

namespace UserWebApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly HttpClient _httpClient;

        public BookingController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:44303/api/")
            };
        }

        // GET: Booking/Search
        public async Task<ActionResult> Search()
        {
            var model = new SearchViewModel
            {
                Stations = await GetStationsAsync()
            };
            return View(model);
        }

        // POST: Booking/Search
        [HttpPost]
        public async Task<ActionResult> Search(SearchViewModel model)
        {
            // Fetch available trains based on search criteria
            if (ModelState.IsValid)
            {
                model.AvailableTrains = await GetAvailableTrainsAsync(model.TravelDate, model.StartStationId, model.DestinationStationId);
            }

            // Always repopulate stations for the dropdown list
            model.Stations = await GetStationsAsync();

            return View(model);
        }

        // Method to fetch stations from the API
        private async Task<List<Station>> GetStationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("search/stations?stationName=");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Station>>(data);
                }
                else
                {
                    ModelState.AddModelError("", "Error fetching stations.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error fetching stations: {ex.Message}");
            }
            return new List<Station>(); // Return empty list on failure
        }

        // Method to fetch available trains based on search criteria
        private async Task<List<Train>> GetAvailableTrainsAsync(DateTime travelDate, int startStationId, int destinationStationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"search/trains?travelDate={travelDate:yyyy-MM-dd}&startStationId={startStationId}&destinationStationId={destinationStationId}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Train>>(data);
                }
                else
                {
                    ModelState.AddModelError("", "Error fetching available trains.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error fetching available trains: {ex.Message}");
            }
            return new List<Train>(); // Return empty list on failure
        }
    }
}
