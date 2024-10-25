using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EN.Entities
{
    public class EN_EstimatedDeliveryResponse : EN_Response
    {
        public string OrderCreated { get; set; }
        public string EstimatedDelivery { get; set; }
        public string MinDelivery { get; set; }
        public string MaxDelivery { get; set; }
    }
}
