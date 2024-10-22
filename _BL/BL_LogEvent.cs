using _DA;
using _EN;
using _EN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _BL
{
    public class BL_LogEvent
    {
        private const string DefaultApplicationName = "Application";
        private string logName;

        public bool _errorConexion = false;
        public bool _valError = false;
        public string _error = string.Empty;
        public string _errorStoreProcedure = string.Empty;
        public string SServer { get; set; }
        public EN_Proxy Proxy { get; set; }

        public Task<EN_Response> SaveLogDB(decimal NUMPEDIDO, string ACCION, string MODULO, string NOMBRESP, DateTime FECHAINICIO,
        DateTime FECHAFIN, decimal TIEMPOS, string OBSERVACIONES, string REQUESTBODY, string Creado = "")
        {
            DA_LogEvent oData = new DA_LogEvent();

            oData.SServer = SServer;
            oData.Proxy = Proxy;
            var response = oData.SaveLogDB(NUMPEDIDO, ACCION, MODULO, NOMBRESP, FECHAINICIO, FECHAFIN, TIEMPOS, OBSERVACIONES, REQUESTBODY, Creado);

            requestErrors(oData);
            return response;
        }

        private void requestErrors(DA_LogEvent oData)
        {
            _error = oData._error;
            _errorConexion = oData._errorConexion;
            _valError = oData._valError;
            _errorStoreProcedure = oData._errorStoreProcedure;
        }
    }
}
