using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refactoring.ApiConsumer.Models;
using Refactoring.LegacyService;
using Refactoring.LegacyService.Interfaces;
using RestSharp;
using System;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Refactoring.JuniorApi.Tests
{
    public class CandidatesControllerTests : IClassFixture<JuniorApiWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private JuniorApiWebApplicationFactory<Startup> _fixure;

        public CandidatesControllerTests(JuniorApiWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _fixure = factory;
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async void TestPostWithInvalidAge()
        {
            CandidateRequest request = new CandidateRequest
            {
                Firstname="Subhro",
                Surname = "Mondal",
                DateOfBirth = DateTime.Now,
                PositionId = 100,
                EmailAddress = "subhro.mondal@company.com"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/candidates", stringContent);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);

            JObject result = JObject.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal(
                "Age validation failed. Candidate must be greater than 18 years.", 
                result.SelectToken("errors.DateOfBirth")[0].Value<string>());
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async void TestPostWithInvalidEmailAddress()
        {
            CandidateRequest request = new CandidateRequest
            {
                Firstname = "Subhro",
                Surname = "Mondal",
                DateOfBirth = DateTime.Now.AddYears(-19),
                PositionId = 100,
                EmailAddress = "subhro.mondal"
            };
            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/candidates", stringContent);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);

            JObject result = JObject.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal(
                "The EmailAddress field is not a valid e-mail address.",
                result.SelectToken("errors.EmailAddress")[0].Value<string>());
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async void TestPostWithRequiredPositionId()
        {
            CandidateRequest request = new CandidateRequest
            {
                Firstname = "Subhro",
                Surname = "Mondal",
                DateOfBirth = DateTime.Now.AddYears(-19),
                EmailAddress = "subhro@company.com"
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/candidates", stringContent);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);

            JObject result = JObject.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal(
                "The field PositionId must be between 1 and 2147483647.",
                result.SelectToken("errors.PositionId")[0].Value<string>());
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async void TestPostWithRequiredMinFirstName()
        {
            CandidateRequest request = new CandidateRequest
            {
                Firstname = "",
                Surname = "Mondal",
                DateOfBirth = DateTime.Now.AddYears(-19),
                EmailAddress = "subhro@company.com"
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/candidates", stringContent);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);

            JObject result = JObject.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal(
                "The Firstname field is required.",
                result.SelectToken("errors.Firstname")[0].Value<string>());
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async void TestPostWithValidCandidateRequest()
        {
            CandidateRequest request = new CandidateRequest
            {
                Firstname = "Subhro",
                Surname = "Mondal",
                PositionId = 10,
                DateOfBirth = DateTime.Now.AddYears(-19),
                EmailAddress = "subhro@company.com"
            };

            _fixure.CandidateCreditSrv.Setup(x => x.GetCredit(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>())).Returns(600);

            _fixure.CandidateDataAccessSrv.Setup(x => x.AddCandidate(
                It.IsAny<Candidate>())).Verifiable();

            _fixure.PositionRepoSrv.Setup(x => x.GetById(
                It.IsAny<int>())).Returns(new Position()
                {
                    Name = Position.PositionType_SecuritySpecialist
                });

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/candidates", stringContent);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Accepted);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async void TestPostWithInvalidCreditRequest()
        {
            CandidateRequest request = new CandidateRequest
            {
                Firstname = "Subhro",
                Surname = "Mondal",
                PositionId = 10,
                DateOfBirth = DateTime.Now.AddYears(-19),
                EmailAddress = "subhro@company.com"
            };

            _fixure.CandidateCreditSrv.Setup(x => x.GetCredit(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>())).Returns(100);

            _fixure.CandidateDataAccessSrv.Setup(x => x.AddCandidate(
                It.IsAny<Candidate>())).Verifiable();

            _fixure.PositionRepoSrv.Setup(x => x.GetById(
                It.IsAny<int>())).Returns(new Position()
                {
                    Name = Position.PositionType_FeatureDeveloper
                });

            var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/candidates", stringContent);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Equal(
                "\"Credit must be greater than or equal to 500\"",
                await response.Content.ReadAsStringAsync());
        }

        [Trait("Category", "Integ")]
        [Fact(Skip = "Require server deployment")]
        public async void TestPostWithValidCandidate()
        {
            CandidateRequest candidate = new CandidateRequest
            {
                Firstname = "Subhro",
                Surname = "Mondal",
                PositionId = 10,
                DateOfBirth = DateTime.Now.AddYears(-19),
                EmailAddress = "subhro@company.com"
            };

            //// this is an integration test which requires actual deployment of 
            ///services of web server or in memory server e.g. TestServer (e.g. Microsoft.AspNetCore.TestHost)
            RestClient client = new RestClient("http://juniorapi.company.com");
            RestRequest request = new RestRequest("api/candidates");
            request.AddParameter("request", candidate, "text/plain", ParameterType.RequestBody);
            var response = await client.ExecuteAsync(request, Method.POST);
            Assert.True(response.IsSuccessful);
        }
    }
}