using Microsoft.VisualBasic.CompilerServices;
using PetShop.Core.DomainService;
using PetShop.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PetShop.Core.ApplicationService.Impl
{
    public class PetService : IPetService
    {
        private IPetRepository _petRepository;
        private IOwnerRepository _ownerRepository;
        private IPetTypeRepository _petTypeRepository;

        public PetService(IPetRepository petRepository, IOwnerRepository ownerRepository, IPetTypeRepository petTypeRepository)
        {
            _petRepository = petRepository;
            _ownerRepository = ownerRepository;
            _petTypeRepository = petTypeRepository;
        }

        //Validates new pet objects
        public Pet ValidatePet(Pet petToValidate, bool validateId)
        {
            string errorMessage = "";

            if (validateId)
            {
                if (petToValidate.PetId < 1)
                {
                    errorMessage += "Pet ID must be a positive integer greater than zero\n";
                }               
            }

            if (petToValidate.PreviousOwner == null || petToValidate.PreviousOwner.OwnerId < 1)
            {
                errorMessage += "Pet must have a previous owner and owner ID must be a positive integer greater than zero\n";
            }
            else
            {
                var foundOwner = _ownerRepository.ReadOwnerById(petToValidate.PreviousOwner.OwnerId);
                if (foundOwner == null)
                {
                    errorMessage += "No owner found with owner ID. Previous owner must be an existing owner\n";
                }
                else
                {
                    petToValidate.PreviousOwner = foundOwner;
                }
            }
            
            if (petToValidate.Type == null || petToValidate.Type.PetTypeId < 1)
            {
                errorMessage += "Pet must have a type and pet type ID must be a positive integer greater than zero\n";
            }
            else
            {
                var foundPetType = _petTypeRepository.ReadPetTypeById(petToValidate.Type.PetTypeId);
                if (foundPetType == null)
                {
                    errorMessage += "No pet type found with pet type ID. Type must be an existing pet type\n";
                }
                else
                {
                    petToValidate.Type = foundPetType;
                }
            }            

            if (string.IsNullOrEmpty(petToValidate.Name) || petToValidate.Name.Length < 2)
            {
                errorMessage += "Name of pet must be at least two characters\n";
            }
            if (petToValidate.BirthDate == null || petToValidate.BirthDate == DateTime.MinValue || petToValidate.BirthDate > DateTime.Now)
            {
                errorMessage += "Pet must have a birth date which cannot be in the future\n";
            }
            //solddate is optional - maybe initialize as equal to birthdate instead of going to default value
            if (petToValidate.SoldDate != null && petToValidate.SoldDate != DateTime.MinValue && petToValidate.SoldDate < petToValidate.BirthDate)
            {
                errorMessage += "Last selling date of the pet cannot predate it's birth\n";
            }
            if (string.IsNullOrEmpty(petToValidate.Color) || petToValidate.Color.Length < 3)
            {
                errorMessage += "Name of color must be at least three characters\n";
            }           
            if (petToValidate.Price < 0)
            {
                errorMessage += "Price of pet cannot be negative\n";
            }
            if (errorMessage.Length > 0)
            {
                throw new ArgumentException(errorMessage);
            }
            return petToValidate;
        }
        public Pet CreatePet(Pet petToCreate)
        {
            var validatedPet = ValidatePet(petToCreate, false);
            return _petRepository.CreatePet(validatedPet);
        }

        public List<Pet> GetAllPets()
        {
            return _petRepository.ReadAllPets().ToList();
        }

        public List<Pet> GetAllPets(Filter filter)
        {                                    
            if (filter.Direction != "asc" && filter.Direction != "desc")
            {
                throw new InvalidDataException("Direction must be asc or desc\n");
            }

            IEnumerable<Pet> filteredPets = new List<Pet>();

            switch (filter.Field.ToLower())
            {
                case "petid":
                    int id;
                    try
                    {
                        id = int.Parse(filter.Term);
                    }
                    catch (FormatException)
                    {
                        throw new InvalidDataException("Search term for pet ID must be an integer\n");
                    }
                    filteredPets = _petRepository.ReadAllPets().Where(pet => pet.PetId == id);
                    filteredPets = (filter.Direction == "asc") ? filteredPets.OrderBy(pet => pet.PetId) : filteredPets.OrderByDescending(pet => pet.PetId);
                    break;
                case "name": //checks if pet name contains search term
                    filteredPets = _petRepository.ReadAllPets().Where(pet => pet.Name.ToLower().Contains(filter.Term.ToLower()));
                    filteredPets = (filter.Direction == "asc") ? filteredPets.OrderBy(pet => pet.Name) : filteredPets.OrderByDescending(pet => pet.Name);
                    break;
                case "type":
                    filteredPets = _petRepository.ReadAllPets().Where(pet => pet.Type.Species.ToLower().Equals(filter.Term.ToLower()));
                    filteredPets = (filter.Direction == "asc") ? filteredPets.OrderBy(pet => pet.PetId) : filteredPets.OrderByDescending(pet => pet.PetId);
                    break;
                case "birthdate": //maybe check for year instead
                    DateTime birthDate;
                    try
                    {
                        birthDate = DateTime.Parse(filter.Term);
                    }
                    catch (FormatException)
                    {
                        throw new InvalidDataException("Search term for birth date must be date in format dd-mm-yyyy\n");
                    }
                    filteredPets = _petRepository.ReadAllPets().Where(pet => pet.BirthDate.Equals(birthDate));
                    filteredPets = (filter.Direction == "asc") ? filteredPets.OrderBy(pet => pet.PetId) : filteredPets.OrderByDescending(pet => pet.PetId);
                    break;
                case "solddate":
                    DateTime soldDate;
                    try
                    {
                        soldDate = DateTime.Parse(filter.Term);
                    }
                    catch (FormatException)
                    {
                        throw new InvalidDataException("Search term for sold date must be date in format dd-mm-yyyy\n");
                    }
                    filteredPets = _petRepository.ReadAllPets().Where(pet => pet.SoldDate.Equals(soldDate));
                    filteredPets = (filter.Direction == "asc") ? filteredPets.OrderBy(pet => pet.PetId) : filteredPets.OrderByDescending(pet => pet.PetId);
                    break;
                case "color":
                    filteredPets = _petRepository.ReadAllPets().Where(pet => pet.Color.ToLower().Equals(filter.Term.ToLower()));
                    filteredPets = (filter.Direction == "asc") ? filteredPets.OrderBy(pet => pet.PetId) : filteredPets.OrderByDescending(pet => pet.PetId);
                    break;
                case "previousowner": //check if previous owner last name contains search term
                    filteredPets = _petRepository.ReadAllPets().Where(pet => pet.PreviousOwner.LastName.ToLower().Contains(filter.Term.ToLower()));
                    filteredPets = (filter.Direction == "asc") ? filteredPets.OrderBy(pet => pet.PreviousOwner.LastName) : filteredPets.OrderByDescending(pet => pet.PreviousOwner.LastName);
                    break;
                case "price":
                    double price;
                    try
                    {
                        price = double.Parse(filter.Term);
                    }
                    catch (FormatException)
                    {
                        throw new InvalidDataException("Search term for price must be a number\n");
                    }
                    filteredPets = _petRepository.ReadAllPets().Where(pet => pet.Price == price);
                    filteredPets = (filter.Direction == "asc") ? filteredPets.OrderBy(pet => pet.PetId) : filteredPets.OrderByDescending(pet => pet.PetId);
                    break;
                default:
                    throw new InvalidDataException("Field must correspond to pet property\n");
                    
            }

            return filteredPets.ToList();
        }
        
        public Pet GetPetById(int id)
        {
            if (id < 1)
            {
                throw new InvalidDataException("ID cannot be lower than 1");
            }
            return _petRepository.ReadPetById(id);
        }

        //should be changed
        public Pet EditPet(Pet petToEdite)
        {
            var validatedPet = ValidatePet(petToEdite, true);
            return _petRepository.EditPet(validatedPet);
        }

        public Pet DeletePet(int id)
        {
            if (id < 1)
            {
                throw new InvalidDataException("ID cannot be lower than 1");
            }
            return _petRepository.DeletePet(id);
        }

    }
}
