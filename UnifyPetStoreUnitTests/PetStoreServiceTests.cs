using Moq;
using System.Net;
using UnifyPetStoreApplication.Classes;
using Moq.Protected;

namespace UnifyPetStoreApplicationUnitTests
{
    [TestFixture]
    public class PetStoreServiceTests
    {
        private HttpClient CreateHttpClient(HttpStatusCode statusCode, string content)
        {
            var handler = new Mock<HttpMessageHandler>();
            handler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content),
                });

            return new HttpClient(handler.Object);
        }

        [Test]
        public async Task GetGroupedPets_SuccessfulApiRequest_ReturnsGroupedPets()
        {
            // Arrange
            string responseContent = @"[
            {
                ""id"": 1,
                ""name"": ""Pet1"",
                ""category"": {
                    ""id"": 101,
                    ""name"": ""CategoryA""
                }
            },
            {
                ""id"": 2,
                ""name"": ""Pet2"",
                ""category"": {
                    ""id"": 102,
                    ""name"": ""CategoryB""
                }
            }
            ]";

            var httpClient = CreateHttpClient(HttpStatusCode.OK, responseContent);
            var service = new PetStoreService(httpClient);
            string apiEndpointUrl = "https://petstore.swagger.io/v2/pet/findByStatus?status=available";

            // Act
            var groupedPets = await service.GetGroupedPets(apiEndpointUrl);

            // Assert
            Assert.IsNotNull(groupedPets);
            Assert.IsNotEmpty(groupedPets);
        }

        [Test]
        public async Task GetGroupedPets_ApiRequestFails_ReturnsEmptyDictionary()
        {
            // Arrange
            var httpClient = CreateHttpClient(HttpStatusCode.BadRequest, "");
            var service = new PetStoreService(httpClient);
            string apiEndpointUrl = "https://petstore.swagger.io/v2/pet/findByStatus?status=available";

            // Act
            var groupedPets = await service.GetGroupedPets(apiEndpointUrl);

            // Assert
            Assert.IsNotNull(groupedPets);
            Assert.IsEmpty(groupedPets);
        }

        [Test]
        public void GroupPets_PetsInList_ReturnsGroupedPets()
        {
            // Arrange
            var service = new PetStoreService(new HttpClient());
            var pets = new List<Pet>
            {
                new Pet { Name = "Dog", Category = new Category { Name = "Mammal" } },
                new Pet { Name = "Cat", Category = new Category { Name = "Mammal" } },
                new Pet { Name = "Parrot" },
            };

            // Act
            var groupedPets = service.GroupPets(pets);

            // Assert
            Assert.IsNotNull(groupedPets);
            Assert.IsNotEmpty(groupedPets);

            // Check if the grouping is correct
            Assert.IsTrue(groupedPets.ContainsKey("Mammal"));
            Assert.IsTrue(groupedPets.ContainsKey("not available"));
            Assert.AreEqual(2, groupedPets["Mammal"].Count);
            Assert.AreEqual(1, groupedPets["not available"].Count);
        }
    }
}
