using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnifyPetStoreApplication.Classes;
using UnifyPetStoreApplication.Services.Interfaces;

public class PetStoreService : IPetStoreService
{
    private readonly HttpClient httpClient;

    public PetStoreService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Dictionary<string, List<Pet>>> GetGroupedPets(string apiEndpointUrl)
    {
        Dictionary<string, List<Pet>> groupedPets = new Dictionary<string, List<Pet>>();
        List<Pet> petsFromAPI = new List<Pet>();

        // Getting API response
        HttpResponseMessage response = await httpClient.GetAsync(apiEndpointUrl);

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();

            //Basic Sanitation of data
            responseContent = SanitizeResponseContent(responseContent);

            try
            {
                petsFromAPI = JsonConvert.DeserializeObject<List<Pet>>(responseContent);

            }
            catch (JsonException ex)
            {
                Console.WriteLine("Error deserializing JSON: " + ex.Message);
                throw;
            }
        }
        else
        {
            Console.WriteLine("API request failed with status code: " + response.StatusCode);
        }

        //If no errors have been logged, group pets by category and return it.
        if (petsFromAPI != null && petsFromAPI.Count > 0)
        {
            groupedPets = GroupPets(petsFromAPI);
        }

        return groupedPets;
    }

    public Dictionary<string, List<Pet>> GroupPets(List<Pet> pets)
    {
        var groupedPets = new Dictionary<string, List<Pet>>();

        groupedPets = pets
            .OrderByDescending(pet => pet.Name) // Order pets by name within each group
            .GroupBy(pet => pet.Category?.Name)
            .OrderBy(group => group.Key)
            .ToDictionary(group => group.Key ?? "not available", group => group.ToList());

        return groupedPets;
    }

    private string SanitizeResponseContent(string content)
    {
        //regular expression to remove curly braces with alphanumeric content
        string pattern = @"\{[a-zA-Z0-9]*\}";
        string sanitizedContent = Regex.Replace(content, pattern, string.Empty);

        return sanitizedContent;
    }

}
