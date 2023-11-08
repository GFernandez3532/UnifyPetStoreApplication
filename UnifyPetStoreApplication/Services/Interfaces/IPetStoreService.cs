using UnifyPetStoreApplication.Classes;

namespace UnifyPetStoreApplication.Services.Interfaces
{
    public interface IPetStoreService
    {
        Task<Dictionary<string, List<Pet>>> GetGroupedPets(string apiEndpointUrl);
    }
}