using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Mimo.api;
using Mimo.Common.DataModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Mimo.Tests
{
    public class MimoIntegrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public MimoIntegrationTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("someWrongName")]

        public async Task GetAchievementsTest_Fail(string userGuid)
        {
            var response = await _client.GetAsync($"/Achievements/{userGuid}");
            var responseStatus = response.StatusCode;
            Assert.True((int)responseStatus >= 400);

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.True(!string.IsNullOrEmpty(responseString));
        }

        [Theory]
        [InlineData("user1")]

        public async Task GetAchievementsTest_Passt(string userGuid)
        {
            var response = await _client.GetAsync($"/Achievements/{userGuid}");
            var responseStatus = response.StatusCode;
            Assert.Equal(HttpStatusCode.OK, responseStatus);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<List<LessonProgressModel>>(responseString);
            Assert.True(responseModel != null);
        }
    }
}
