using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace _EN
{
    public class EN_Response
    {
        public int status { get; set; }
        public string errorLine { get; set; }
        public string errorNumber { get; set; }
        public string errorMessage { get; set; }
        public Int64 id { get; set; }
    }

    public class EN_ErrorDesciptionDb : EN_Language, IErrorDb
    {
        public string errorDescriptionBd { get; set; }
    }

    public class EN_Language : ILanguage
    {
        public int language { get; set; }
    }

    public class EN_AddValidateOrderReturn: EN_Response
    {
        public Int64 artsInDev { get; set; }
        public Int64 cantTotalPedido { get; set; }
        public Int64 cantTotalDev { get; set; }
        public int esAbf { get; set; }
    }

    public class EN_ComplementResponse:EN_Response
    {
        public int esCancelacion { get; set; }
    }
}
