using UnifyPetStoreApplication.Services.Interfaces;

class Program
{
    #region Properties

    private readonly IPetStoreService petStoreService;

    string apiEndpointUrl = "https://petstore.swagger.io/v2/pet/findByStatus?status=available";

    #endregion

    #region Constructor
    public Program(IPetStoreService petStoreService)
    {
        this.petStoreService = petStoreService;
    }

    #endregion

    static async Task Main()
    {
        using HttpClient httpClient = new HttpClient();
        var petStoreService = new PetStoreService(httpClient);

        var program = new Program(petStoreService);
        await program.Run();
    }


    public async Task Run()
    {
        try
        {
            var groupedPets = await petStoreService.GetGroupedPets(apiEndpointUrl);

            if (groupedPets.Count > 0)
            {
                foreach (var categoryGroup in groupedPets)
                {
                    Console.WriteLine($"Category {categoryGroup.Key}:");

                    foreach (var pet in categoryGroup.Value)
                    {
                        Console.WriteLine($"Pet name: {pet.Name}");
                    }

                    Console.WriteLine("");
                }
            }
            else
            {
                Console.WriteLine("No available pets were found");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}