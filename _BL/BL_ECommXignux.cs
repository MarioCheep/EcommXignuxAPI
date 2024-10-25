using _DA;
using _EN;
using _EN.Entities;
using Newtonsoft.Json;
using System.Net;

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

        public EN_Response GetProductStock(int productId)
        {

            DA_XignuxDB oData = new DA_XignuxDB();
            EN_Response response = new EN_Response();
            EN_Response responseValidate = new EN_Response();

            if (ValidateData_GetStock(productId, ref responseValidate))
            {

                //var myRequest = JsonConvert.SerializeObject(entidad);
                //oBLogEvent.SaveLogDB(Decimal.Parse(entidad.Id_Transaccion.Replace("Xignux", "")), "GetStock", LogDBModule, "RequestData", DateTime.Now, DateTime.Now, 0, myRequest, string.Empty);

                response = oData.GetProductStockDB(productId, true);
            }
            else
            {
                response = responseValidate;
            }

            requestErrors(oData);
            var myResponse = JsonConvert.SerializeObject(response);
            //oBLogEvent.SaveLogDB(Decimal.Parse(entidad.Id_Transaccion.Replace("Xignux", "")), "GetStock", LogDBModule, "ResponseData", DateTime.Now, DateTime.Now, 0, myResponse, string.Empty);

            return response;
        }

        public List<EN_ApplyCouponResponse> CouponApplicable(EN_ApplyCoupon entidad)
        {

            DA_XignuxDB oData = new DA_XignuxDB();
            List<EN_ApplyCouponResponse> olstENResponse = new List<EN_ApplyCouponResponse>();
            EN_Response responseValidate = new EN_Response();

            if (ValidateData_Coupon(entidad, ref responseValidate))
            {

                var myRequest = JsonConvert.SerializeObject(entidad);
                //oBLogEvent.SaveLogDB(Decimal.Parse(entidad.Id_Transaccion.Replace("Xignux", "")), "GetStock", LogDBModule, "RequestData", DateTime.Now, DateTime.Now, 0, myRequest, string.Empty);

                olstENResponse = oData.GetCouponApplicableDB(entidad);

            }
            else
            {
                olstENResponse.Add(new EN_ApplyCouponResponse
                {
                    DiscountPercentage = 0,
                    ValidationInformation = responseValidate.errorMessage
                });
            }

            requestErrors(oData);
            var myResponse = JsonConvert.SerializeObject(olstENResponse);
            //oBLogEvent.SaveLogDB(Decimal.Parse(entidad.Id_Transaccion.Replace("Xignux", "")), "GetStock", LogDBModule, "ResponseData", DateTime.Now, DateTime.Now, 0, myResponse, string.Empty);

            return olstENResponse;
        }

        public List<EN_PremiumBenefitsResponse> GetPremiumBenefits(int clientId)
        {

            DA_XignuxDB oData = new DA_XignuxDB();
            List<EN_PremiumBenefitsResponse> lstResponse = new List<EN_PremiumBenefitsResponse>();
            EN_PremiumBenefitsResponse responseValidate = new EN_PremiumBenefitsResponse();


            lstResponse = oData.GetPremiumBenefitsDB(clientId);

            if (!ValidateData_GetBenefistByClient(clientId, ref responseValidate))
                lstResponse.Add(responseValidate);
            else
                lstResponse.Add(new EN_PremiumBenefitsResponse { status = 200, });


            requestErrors(oData);
            var myResponse = JsonConvert.SerializeObject(lstResponse);
            //oBLogEvent.SaveLogDB(Decimal.Parse(entidad.Id_Transaccion.Replace("Xignux", "")), "GetStock", LogDBModule, "ResponseData", DateTime.Now, DateTime.Now, 0, myResponse, string.Empty);

            return lstResponse;
        }

        public double CalculateShipping(EN_CalculateShipping entidad)
        {

            DA_XignuxDB oData = new DA_XignuxDB();
            EN_Response response = new EN_Response();
            EN_Response responseValidate = new EN_Response();

            try
            {
                //if (ValidateData_GetStock(productId, ref responseValidate))
                //{

                //var myRequest = JsonConvert.SerializeObject(entidad);
                //oBLogEvent.SaveLogDB(Decimal.Parse(entidad.Id_Transaccion.Replace("Xignux", "")), "GetStock", LogDBModule, "RequestData", DateTime.Now, DateTime.Now, 0, myRequest, string.Empty);

                return oData.CalculateShippingDB(entidad);
                //}
                //else
                //{
                //    response = responseValidate;
                //}

                //requestErrors(oData);
                //var myResponse = JsonConvert.SerializeObject(response);
                //oBLogEvent.SaveLogDB(Decimal.Parse(entidad.Id_Transaccion.Replace("Xignux", "")), "GetStock", LogDBModule, "ResponseData", DateTime.Now, DateTime.Now, 0, myResponse, string.Empty);
            }
            catch (Exception ex)
            {
                //ToDO Implement Log Action
                return -2.0;
            }
        }
            
        private bool ValidateData_GetStock(int productId, ref EN_Response response)
        {
            if (productId != 0)
            {
                if (productId == 1612)
                {
                    EN_Response RaiseError = new EN_Response();
                    RaiseError.status = 0;
                    RaiseError.errorMessage = "productId inválido";
                    response = RaiseError;

                    return false;
                }
            }
            else
            {
                response.errorMessage = "Se requiere un productId mayor a 0.";
                response.status = 400;
                return false;
            }
            return true;
        }

        private bool ValidateData_Coupon(EN_ApplyCoupon entidad, ref EN_Response response)
        {
            if (entidad != null)
            {
                if (entidad.couponCode.Equals("CUP1612"))
                {
                    EN_Response RaiseError = new EN_Response();
                    RaiseError.status = 0;
                    RaiseError.errorMessage = "Cupón inválido";
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

        private bool ValidateData_GetBenefistByClient(int clientId, ref EN_PremiumBenefitsResponse response)
        {
            if (clientId == 0)
            {
                response.errorMessage = "Cliente inválido.";
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
