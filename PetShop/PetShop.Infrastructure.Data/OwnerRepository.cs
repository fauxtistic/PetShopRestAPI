using PetShop.Core.DomainService;
using PetShop.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Infrastructure.Data
{
    public class OwnerRepository : IOwnerRepository
    {
        private static IEnumerable<Owner> _owners = new List<Owner>();
        private static int _counter = 1;

        public OwnerRepository()
        {            
        }

        public Owner CreateOwner(Owner ownerToCreate)
        {
            ownerToCreate.OwnerId = _counter++;
            ((List<Owner>)_owners).Add(ownerToCreate);
            return ownerToCreate;
        }

        public IEnumerable<Owner> ReadAllOwners()
        {
            return _owners;
        }

        public Owner ReadOwnerById(int id)
        {
            Owner foundOwner = null;
            foreach (var owner in _owners)
            {
                if (owner.OwnerId == id)
                {
                    foundOwner = owner;
                    break;
                }
            }
            return foundOwner;
        }

        public Owner EditOwner(Owner editedOwner)
        {
            var changedOwner = ReadOwnerById(editedOwner.OwnerId);
            if (changedOwner != null)
            {
                changedOwner.FirstName = editedOwner.FirstName;
                changedOwner.LastName = editedOwner.LastName;
                changedOwner.Address = editedOwner.Address;
                changedOwner.PhoneNumber = editedOwner.PhoneNumber;
                changedOwner.Email = editedOwner.Email;
            }            
            return changedOwner;
        }
        public Owner DeleteOwner(int id)
        {
            var ownerToDelete = ReadOwnerById(id);
            if (ownerToDelete != null)
            {
                ((List<Owner>)_owners).Remove(ownerToDelete);
            }            
            return ownerToDelete;
            
        }
    }
}
