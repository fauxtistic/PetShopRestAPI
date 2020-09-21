using PetShop.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.ApplicationService
{
    public interface IPetTypeService
    {
        public PetType ValidatePetType(PetType petTypeToValidate, bool validateId);
        public PetType CreatePetType(PetType petTypeToCreate);
        public List<PetType> GetAllPetTypes();
        public List<PetType> GetAllPetTypes(Filter filter);
        public PetType GetPetTypeById(int id);
        public PetType EditPetType(PetType petTypeToEdit);
        public PetType DeletePetType(int id);
    }
}
