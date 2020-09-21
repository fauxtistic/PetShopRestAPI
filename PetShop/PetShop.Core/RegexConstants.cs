using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core
{
    public static class RegexConstants
    {
        public const string FirstOrLastName = "[a-zA-Z]+";
        //public const string FirstOrLastName= "^[a - zA - Z] + (([\\'\\,\\.\\-][a-zA-Z])?[a-zA-Z]*)*$"; //cant get this to work
        public const string RegexAddress = "^((.){1,}(\\d){1,}(.){0,})$";
        public const string RegexPhoneNumber = "^([\\+][0-9]{1,3}([ \\.\\-])?)?([\\(]{1}[0-9]{3}[\\)])?([0-9A-Z \\.\\-]{1,32})((x|ext|extension)?[0-9]{1,4}?)$";
        public const string RegexEmail = "[\\w-]+@([\\w-]+\\.)+[\\w-]+";
    }
}
