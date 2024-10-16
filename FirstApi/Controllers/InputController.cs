using DataAccessLayer.DbContext;
using DataAccessLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApplication9.Controllers
{
    public class InputController : ApiController
    {
        private const int BATCH_SIZE = 5;
        private static readonly HttpClient Client = new HttpClient();
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            var data = db.Inputs.Where(i => i.HesaplandiMi == false).ToList();
            if (!data.Any())
            {
                return BadRequest("Hesaplanmamış input bulunamadı.");
            }

            var apiEndpoints = db.Apis.Select(api => api.Url).ToList();
            if (!apiEndpoints.Any())
            {
                return BadRequest("API endpoint bulunamadı.");
            }

            var batchList = new List<List<Inputs>>();
            for (int i = 0; i < data.Count; i += BATCH_SIZE)
            {
                batchList.Add(data.Skip(i).Take(BATCH_SIZE).ToList());
            }
            await SendToAPIs(batchList, apiEndpoints);

            return Ok();
        }
        private async Task SendToAPIs(List<List<Inputs>> batchList, List<string> apiEndpoints)
        {
            var tasks = batchList.Select((batch, index) =>
            {
                var apiEndpoint = apiEndpoints[index % apiEndpoints.Count];
                return SendToAPI(batch, apiEndpoint);
            });

            await Task.WhenAll(tasks);
        }

        private async Task<HttpResponseMessage> SendToAPI(List<Inputs> inputs, string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonConvert.SerializeObject(inputs), Encoding.UTF8, "application/json")
            };

            return await Client.SendAsync(requestMessage);
        }
    }
}
