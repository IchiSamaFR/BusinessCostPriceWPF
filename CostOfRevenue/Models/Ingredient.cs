using CostOfRevenue.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostOfRevenue.Models
{
    public class Ingredient
    {
        public string Id;
        public string Name;
        public Enums.Unit Unit;
        public float Price;
        public DateTime Date;
        public float Quantity;
    }
}
