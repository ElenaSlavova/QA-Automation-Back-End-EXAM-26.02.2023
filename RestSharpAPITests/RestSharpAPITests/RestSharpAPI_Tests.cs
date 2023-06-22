using RestSharp;
using System.Net;
using System.Numerics;
using System.Text.Json;
using System.Xml.Linq;

namespace RestSharpAPITests
{
    public class RestSharpAPI_Tests
    {
        private RestClient client;
        private const string baseUrl = "https://contactbook.slavova1.repl.co/api";

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient(baseUrl);
        }

        [Test]
        public void Test_Get_CheckFirstAndLastNamesOfContact()
        {
            var request = new RestRequest("contacts", Method.Get);
            var response = this.client.Execute(request);

            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            Assert.That(contacts[0].firstName, Is.EqualTo("Steve"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Jobs"));
        }


        [Test]
        public void Test_Get_FindContactByKeyword()
        {
            var request = new RestRequest("contacts/search/albert", Method.Get);
            var response = this.client.Execute(request);

            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            Assert.That(contacts[0].firstName, Is.EqualTo("Albert"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Einstein"));
        }

        [Test]
        public void Test_GetSearchByKeyword_InvalidResult()
        {
            var request = new RestRequest("contacts/search/missing" + DateTime.Now.Ticks, Method.Get);
            var response = this.client.Execute(request);

            var contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);
            Assert.That(contacts, Is.Empty);
            Assert.That(response.Content, Is.EqualTo("[]"));
        }

        [Test]
        public void Test_TryToCreateNewContactWithInvalidData()
        {

            var request = new RestRequest("contacts", Method.Post);
            var response = this.client.Execute(request);

            var body = new
            {
                email = "marie67@gmail.com",
                phone = "+1 800 200 300"
            };
            request.AddBody(body);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"First name cannot be empty!\"}"));

        }
        [Test]
        public void Test_TryToCreateNewContactWithValidData()
        {
            var request = new RestRequest("contacts", Method.Post);

            var body = new
            {
                firstName = "Marie" + DateTime.Now.Ticks,
                lastName = "Curie" + DateTime.Now.Ticks,
                email = "marie67@gmail.com",
                phone = "+1 800 200 300",
                comments = "Old friend"
            };
            request.AddBody(body);
            var response = this.client.Execute(request);

            var contactObject = JsonSerializer.Deserialize<ContactObject>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(contactObject.msg, Is.EqualTo("Contact added."));
            Assert.That(contactObject.contact.id, Is.GreaterThan(0));
            Assert.That(contactObject.contact.firstName, Is.EqualTo(body.firstName));
            Assert.That(contactObject.contact.lastName, Is.EqualTo(body.lastName));
            Assert.That(contactObject.contact.email, Is.EqualTo(body.email));
            Assert.That(contactObject.contact.phone, Is.EqualTo(body.phone));
            Assert.That(contactObject.contact.comments, Is.EqualTo(body.comments));

        }

    }
}