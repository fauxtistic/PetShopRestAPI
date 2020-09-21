using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.Core.Entity
{
    public class Filter
    {
        public string Direction { get; set; }
        public string Field { get; set; }
        public string Term { get; set; }
    }
}
