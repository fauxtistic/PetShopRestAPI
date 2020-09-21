using PetShop.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.DomainService
{
    public interface IPetTypeRepository
    {
        public PetType CreatePetType(PetType petTypeToCreate);
        public IEnumerable<PetType> ReadAllPetTypes();
        public PetType ReadPetTypeById(int id);
        public PetType EditPetType(PetType editedType);
        public PetType DeletePetType(int id);
    }
}
