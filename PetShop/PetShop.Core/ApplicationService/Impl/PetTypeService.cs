using PetShop.Core.DomainService;
using PetShop.Core.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PetShop.Core.ApplicationService.Impl
{
    public class PetTypeService : IPetTypeService
    {
        private IPetTypeRepository _petTypeRepository;

        public PetTypeService(IPetTypeRepository petTypeRepository)
        {
            _petTypeRepository = petTypeRepository;
        }

        public PetType ValidatePetType(PetType petTypeToValidate, bool validateId)
        {
            string errorMessage = "";

            if (validateId)
            {
                if (petTypeToValidate.PetTypeId < 1)
                {
                    errorMessage += "ID must be a positive integer greater than zero\n";
                }
            }

            if (string.IsNullOrEmpty(petTypeToValidate.Species) || !new Regex(RegexConstants.FirstOrLastName).IsMatch(petTypeToValidate.Species))
            {
                errorMessage += "Species name must consist of letters a-z\n";
            }

            if (errorMessage.Length > 0)
            {
                throw new ArgumentException(errorMessage);
            }

            return petTypeToValidate;
        }

        public PetType CreatePetType(PetType petTypeToCreate)
        {
            var validatedPetType = ValidatePetType(petTypeToCreate, false);
            return _petTypeRepository.CreatePetType(validatedPetType);
        }

        public List<PetType> GetAllPetTypes()
        {
            return _petTypeRepository.ReadAllPetTypes().ToList();
        }

        public List<PetType> GetAllPetTypes(Filter filter)
        {
            if (filter.Direction != "asc" && filter.Direction != "desc")
            {
                throw new InvalidDataException("Direction must be asc or desc\n");
            }

            IEnumerable<PetType> filteredTypes = new List<PetType>();

            switch (filter.Field.ToLower())
            {
                case "pettypeid":
                    int id;
                    try
                    {
                        id = int.Parse(filter.Term);
                    }
                    catch (FormatException)
                    {
                        throw new InvalidDataException("Search term for pet type ID must be an integer\n");
                    }
                    filteredTypes = _petTypeRepository.ReadAllPetTypes().Where(type => type.PetTypeId == id);
                    filteredTypes = (filter.Direction == "asc") ? filteredTypes.OrderBy(type => type.PetTypeId) : filteredTypes.OrderByDescending(type => type.PetTypeId);
                    break;
                case "species":
                    filteredTypes = _petTypeRepository.ReadAllPetTypes().Where(type => type.Species.ToLower().Contains(filter.Term.ToLower()));
                    filteredTypes = (filter.Direction == "asc") ? filteredTypes.OrderBy(type => type.Species) : filteredTypes.OrderByDescending(type => type.Species);
                    break;
                default:
                    throw new InvalidDataException("Field must correspond to pet type property\n");
            }

            return filteredTypes.ToList();
        }

        public PetType GetPetTypeById(int id)
        {
            if (id < 1)
            {
                throw new InvalidDataException("ID cannot be lower than 1");
            }
            return _petTypeRepository.ReadPetTypeById(id);
        }

        public PetType EditPetType(PetType petTypeToEdit)
        {
            var validatedPetType = ValidatePetType(petTypeToEdit, true);
            return _petTypeRepository.EditPetType(validatedPetType);
        }

        public PetType DeletePetType(int id)
        {
            if (id < 1)
            {
                throw new InvalidDataException("ID cannot be lower than 1");
            }
            return _petTypeRepository.DeletePetType(id);
        }     

    }
}
