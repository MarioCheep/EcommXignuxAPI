using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _EN;
using _BL;
using static _EN.EN_ControlErrors;

namespace EventHubLastMilleApi
{
    public static class ControlErrors
    {
        public static EN_Errors setVarError(BL_Orders oBLogic)
        {
            EN_Errors oEErrors = new EN_Errors()
            {
                valError = oBLogic._valError,
                errorConexion = oBLogic._errorConexion,
                error = oBLogic._error,
                errorStoreProcedure = oBLogic._errorStoreProcedure
            };

            return oEErrors;
        }
    }
}
