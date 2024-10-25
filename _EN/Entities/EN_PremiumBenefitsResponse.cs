using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EN.Entities
{
    public class EN_PremiumBenefitsResponse : EN_Response
    {
        public int BenefitId {  get; set; }
        public string BenefitName { get; set; }
    }
}
