using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PetShop.Core.Entity
{
    public class Pet
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public PetType Type { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime SoldDate { get; set; }
        public string Color { get; set; }
        public Owner PreviousOwner { get; set; }
        public double Price { get; set; }

        public override string ToString()
        {
            return $"Id: {PetId}" +
                   $"\tName: {Name}" +
                   $"\tType: {Type}" +
                   $"\tBirth date: {BirthDate.ToShortDateString()}" +
                   $"\tSold date: {SoldDate.ToShortDateString()}" +
                   $"\tColor: {Color}" +
                   $"\tPrevious owner: {PreviousOwner.FirstName} {PreviousOwner.LastName} (Id: {PreviousOwner.OwnerId})" +
                   $"\tPrice {Price}";
        }
    }
}
