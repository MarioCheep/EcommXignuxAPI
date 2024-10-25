using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EN.Entities
{
    public class EN_ApplyCouponResponse
    {
        public string ValidationInformation { get; set; }
        public int DiscountPercentage { get; set; }
        public string ProductName { get; set; }
    }
}
