using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;

namespace FundooNotes.IntegrationTests
{
    public class NotesApiTests
    {
        private WebApplicationFactory<Program> _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        [Test]
        public async Task Get_Notes_Unauthorized_Without_Token()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/notes");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}