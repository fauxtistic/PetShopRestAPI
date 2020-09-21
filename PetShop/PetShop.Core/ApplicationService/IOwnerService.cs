using PetShop.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.ApplicationService
{
    public interface IOwnerService
    {
        public Owner ValidateOwner(Owner ownerToValidate, bool validateId);
        public Owner CreateOwner(Owner ownerToCreate);
        public List<Owner> GetAllOwners();
        public List<Owner> GetAllOwners(Filter filter);
        public Owner GetOwnerById(int id);
        public Owner EditOwner(Owner ownerToEdit);
        public Owner DeleteOwner(int id);
    }
}
