using PetShop.Core.DomainService;
using PetShop.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Infrastructure.Data
{
    public class PetRepository : IPetRepository
    {
        private static IEnumerable<Pet> _pets = new List<Pet>();
        private static int _counter = 1;

        public PetRepository()
        {                        
        }

        public Pet CreatePet(Pet pet)
        {
            pet.PetId = _counter++;
            ((List<Pet>)_pets).Add(pet);
            return pet;
        }

        public IEnumerable<Pet> ReadAllPets()
        {
            return _pets;
        }

        public Pet ReadPetById(int id)
        {
            Pet foundPet = null;
            foreach (var pet in _pets)
            {
                if (pet.PetId == id)
                {
                    foundPet = pet;
                    break;
                }
            }
            return foundPet;
        }

        //should be changed
        public Pet EditPet(Pet editedPet)
        {
            Pet petToChange = ReadPetById(editedPet.PetId);
            if (petToChange != null)
            {
                petToChange.Name = editedPet.Name;
                petToChange.Type = editedPet.Type;
                petToChange.BirthDate = editedPet.BirthDate;
                petToChange.SoldDate = editedPet.SoldDate;
                petToChange.Color = editedPet.Color;
                petToChange.PreviousOwner = editedPet.PreviousOwner;
                petToChange.Price = editedPet.Price;
            }            
            return petToChange;
        }

        public Pet DeletePet(int id)
        {
            var petToDelete = ReadPetById(id);
            if (petToDelete != null)
            {
                ((List<Pet>)_pets).Remove(petToDelete);
            }
            return petToDelete;
        }

        
    }
}
