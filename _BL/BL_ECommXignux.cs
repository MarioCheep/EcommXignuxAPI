using _DA;
using _EN;
using _EN.Entities;
using Newtonsoft.Json;

namespace _BL
{
    public class BL_ECommXignux
    {
        readonly BL_LogEvent oBLogEvent = new BL_LogEvent();
        public bool _errorConexion = false;
        public bool _valError = false;
        public string _error = string.Empty;
        public string _errorStoreProcedure = string.Empty;
        private const string LogDBModule = "BL_ECommXignux";

        public List<EN_Response> GetProductStock(EN_ProductStock entidad)
        {

            DA_XignuxDB oData = new DA_XignuxDB();
            List<EN_Response> response = new List<EN_Response>();
            EN_Response responseValidate = new EN_Response();

            if (ValidateData_GetStock(entidad, ref responseValidate))
            {

                var myRequest = JsonConvert.SerializeObject(entidad);
                //oBLogEvent.SaveLogDB(Decimal.Parse(entidad.Id_Transaccion.Replace("Xignux", "")), "GetStock", LogDBModule, "RequestData", DateTime.Now, DateTime.Now, 0, myRequest, string.Empty);


                response =  oData.GetProductStockDB(entidad, true);
            }
            else
            {
                response.Add(responseValidate);
            }

            requestErrors(oData);
            var myResponse = JsonConvert.SerializeObject(response);
            //oBLogEvent.SaveLogDB(Decimal.Parse(entidad.Id_Transaccion.Replace("Xignux", "")), "GetStock", LogDBModule, "ResponseData", DateTime.Now, DateTime.Now, 0, myResponse, string.Empty);

            return response;
        }

        private bool ValidateData_GetStock(EN_ProductStock entidad, ref EN_Response response)
        {
            if (entidad != null)
            {
                if (entidad.ProductId.Equals(1612))
                {
                    EN_Response RaiseError = new EN_Response();
                    RaiseError.status = 0;
                    RaiseError.errorMessage = "Producto inválido";
                    response = RaiseError;

                    return false;
                }
            }
            else
            {
                response.errorMessage = "Los datos propocionados nos son validos o estan vacios.";
                response.status = 400;
                return false;
            }
            return true;
        }
            private void requestErrors(DA_XignuxDB oData)
        {
            _error = oData._error;
            _errorConexion = oData._errorConexion;
            _valError = oData._valError;
            _errorStoreProcedure = oData._errorStoreProcedure;
        }
    }
}
