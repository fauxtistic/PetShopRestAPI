using PetShop.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.DomainService
{
    public interface IOwnerRepository
    {
        public Owner CreateOwner(Owner ownerToCreate);
        public IEnumerable<Owner> ReadAllOwners();
        public Owner ReadOwnerById(int id);
        public Owner EditOwner(Owner editedOwner);
        public Owner DeleteOwner(int id);
    }
}
