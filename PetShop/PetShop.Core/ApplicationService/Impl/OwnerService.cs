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
    public class OwnerService : IOwnerService
    {
        private IOwnerRepository _ownerRepository;

        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }
        public Owner ValidateOwner(Owner ownerToValidate, bool validateId)
        {
            string errorMessage = "";

            if (validateId)
            {
                if (ownerToValidate.OwnerId < 1)
                {
                    errorMessage += "ID must be a positive integer greater than zero\n";
                }                
            }

            if (string.IsNullOrEmpty(ownerToValidate.FirstName) || !new Regex(RegexConstants.FirstOrLastName).IsMatch(ownerToValidate.FirstName))
            {
                errorMessage += "First name must consist of letters a-z\n";
            }
            if (string.IsNullOrEmpty(ownerToValidate.LastName) || !new Regex(RegexConstants.FirstOrLastName).IsMatch(ownerToValidate.LastName))
            {
                errorMessage += "Last name must consist of letters a-z\n";
            }
            if (string.IsNullOrEmpty(ownerToValidate.Address) || !new Regex(RegexConstants.RegexAddress).IsMatch(ownerToValidate.Address))
            {
                errorMessage += "Address must be in format street, number, with additions allowed after\n";
            }
            if (string.IsNullOrEmpty(ownerToValidate.PhoneNumber) || !new Regex(RegexConstants.RegexPhoneNumber).IsMatch(ownerToValidate.PhoneNumber))
            {
                errorMessage += "Phone number is not in valid format\n";
            }
            if (string.IsNullOrEmpty(ownerToValidate.Email) || !new Regex(RegexConstants.RegexEmail).IsMatch(ownerToValidate.Email))
            {
                errorMessage += "Email is not in valid format\n";
            }

            if (errorMessage.Length > 0)
            {
                throw new ArgumentException(errorMessage);
            }

            return ownerToValidate;
        }

        public Owner CreateOwner(Owner ownerToCreate)
        {
            var validatedOwner = ValidateOwner(ownerToCreate, false);
            return _ownerRepository.CreateOwner(validatedOwner);
        }

        public List<Owner> GetAllOwners()
        {
            return _ownerRepository.ReadAllOwners().ToList();
        }

        public List<Owner> GetAllOwners(Filter filter)
        {
            if (filter.Direction != "asc" && filter.Direction != "desc")
            {
                throw new InvalidDataException("Direction must be asc or desc\n");
            }

            IEnumerable<Owner> filteredOwners = new List<Owner>();

            switch (filter.Field.ToLower())
            {
                case "ownerid":
                    int id;
                    try
                    {
                        id = int.Parse(filter.Term);
                    }
                    catch (FormatException)
                    {
                        throw new InvalidDataException("Search term for owner ID must be an integer\n");
                    }
                    filteredOwners = _ownerRepository.ReadAllOwners().Where(owner => owner.OwnerId == id);
                    filteredOwners = (filter.Direction == "asc") ? filteredOwners.OrderBy(owner => owner.OwnerId) : filteredOwners.OrderByDescending(owner => owner.OwnerId);
                    break;
                    //all below allow partial search
                case "firstname":
                    filteredOwners = _ownerRepository.ReadAllOwners().Where(owner => owner.FirstName.ToLower().Contains(filter.Term.ToLower()));
                    filteredOwners = (filter.Direction == "asc") ? filteredOwners.OrderBy(owner => owner.FirstName) : filteredOwners.OrderByDescending(owner => owner.FirstName);
                    break;
                case "lastname":
                    filteredOwners = _ownerRepository.ReadAllOwners().Where(owner => owner.LastName.ToLower().Contains(filter.Term.ToLower()));
                    filteredOwners = (filter.Direction == "asc") ? filteredOwners.OrderBy(owner => owner.LastName) : filteredOwners.OrderByDescending(owner => owner.LastName);
                    break;
                case "address":
                    filteredOwners = _ownerRepository.ReadAllOwners().Where(owner => owner.Address.ToLower().Contains(filter.Term.ToLower()));
                    filteredOwners = (filter.Direction == "asc") ? filteredOwners.OrderBy(owner => owner.Address) : filteredOwners.OrderByDescending(owner => owner.Address);
                    break;                
                case "phonenumber":
                    filteredOwners = _ownerRepository.ReadAllOwners().Where(owner => owner.PhoneNumber.ToLower().Contains(filter.Term.ToLower()));
                    filteredOwners = (filter.Direction == "asc") ? filteredOwners.OrderBy(owner => owner.PhoneNumber) : filteredOwners.OrderByDescending(owner => owner.PhoneNumber);
                    break;
                case "email":
                    filteredOwners = _ownerRepository.ReadAllOwners().Where(owner => owner.Email.ToLower().Contains(filter.Term.ToLower()));
                    filteredOwners = (filter.Direction == "asc") ? filteredOwners.OrderBy(owner => owner.Email) : filteredOwners.OrderByDescending(owner => owner.Email);
                    break;
                default:
                    throw new InvalidDataException("Field must correspond to owner property\n");
            }

            return filteredOwners.ToList();
        }

        public Owner GetOwnerById(int id)
        {
            if (id < 1)
            {
                throw new InvalidDataException("ID cannot be lower than 1");
            }
            return _ownerRepository.ReadOwnerById(id);
        }
        public Owner EditOwner(Owner ownerToEdit) //stopgap
        {
            var validatedOwner = ValidateOwner(ownerToEdit, true);
            return _ownerRepository.EditOwner(validatedOwner);
        }

        public Owner DeleteOwner(int id)
        {
            if (id < 1)
            {
                throw new InvalidDataException("ID cannot be lower than 1");
            }
            return _ownerRepository.DeleteOwner(id);
        }
    }
}
