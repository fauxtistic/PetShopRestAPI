using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.Entity
{
    public class Owner
    {
        public int OwnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return $"Id: {OwnerId}" +
                   $"\tName: {FirstName} {LastName}" +
                   $"\tAddress: {Address}" +
                   $"\tPhone number: {PhoneNumber}" +
                   $"\tSold date: {Email}";                   
        }
    }
}
