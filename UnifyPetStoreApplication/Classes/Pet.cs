using UnifyPetStoreApplication.Shared;

namespace UnifyPetStoreApplication.Classes
{
    public class Pet
    {
        public long Id { get; set; }

        public Category? Category { get; set; }

        public string Name { get; set; }

        public List<string>? PhotoURLs { get; set; }

        public List<Tag>? Tags{ get; set; }

        public PetStatus Status{ get; set; }
        
    }
}
