using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EN.Entities
{
    public class EN_CalculateShippingResponse
    {
        public double ShippingCost { get; set; }
        public double IncrementByProduct { get; set; }
        public double IncrementByDistance { get; set; }

    }
}
