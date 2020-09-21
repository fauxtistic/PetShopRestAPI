using PetShop.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.ApplicationService
{
    public interface IPetService
    {
        public Pet ValidatePet(Pet petToValidate, bool validateId);
        public Pet CreatePet(Pet petToCreate);
        public List<Pet> GetAllPets();
        public List<Pet> GetAllPets(Filter filter);
        public Pet GetPetById(int id);
        public Pet EditPet(Pet petToEdit);
        public Pet DeletePet(int id);
    }
}
