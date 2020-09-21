using PetShop.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.DomainService
{
    public interface IPetRepository
    {
        public Pet CreatePet(Pet pet);
        public IEnumerable<Pet> ReadAllPets();
        public Pet ReadPetById(int id);
        public Pet EditPet(Pet editedPet);
        public Pet DeletePet(int id);
    }
}
