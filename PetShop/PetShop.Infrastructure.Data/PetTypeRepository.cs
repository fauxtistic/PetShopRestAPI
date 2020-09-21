using PetShop.Core.DomainService;
using PetShop.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Infrastructure.Data
{
    public class PetTypeRepository : IPetTypeRepository
    {
        private static IEnumerable<PetType> _petTypes = new List<PetType>();
        private static int _counter = 1;

        public PetTypeRepository()
        {           
        }

        public PetType CreatePetType(PetType petTypeToCreate)
        {
            petTypeToCreate.PetTypeId = _counter++;
            ((List<PetType>)_petTypes).Add(petTypeToCreate);
            return petTypeToCreate;

        }

        public IEnumerable<PetType> ReadAllPetTypes()
        {
            return _petTypes;
        }

        public PetType ReadPetTypeById(int id)
        {
            PetType foundPetType = null;
            foreach (var type in _petTypes)
            {
                if (type.PetTypeId == id)
                {
                    foundPetType = type;
                    break;
                }
            }
            return foundPetType;
        }

        public PetType EditPetType(PetType editedType)
        {
            var changedPetType = ReadPetTypeById(editedType.PetTypeId);
            if (changedPetType != null)
            {
                changedPetType.Species = editedType.Species;
            }            
            return changedPetType;
        }

        public PetType DeletePetType(int id)
        {
            var petTypeToDelete = ReadPetTypeById(id);
            if (petTypeToDelete != null)
            {
                ((List<PetType>)_petTypes).Remove(petTypeToDelete);
            }
            return petTypeToDelete;
        }
        
    }
}
