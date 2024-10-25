using _EN;
using _EN.Entities;
using Azure;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;

namespace _DA
{
    public class DA_XignuxDB
    {
        public bool _errorConexion = false;
        public bool _valError = false;
        public string _error = string.Empty;
        public string _errorStoreProcedure = string.Empty;

        private int _ProductId { get; set; }

        private string connString {  get; set; }


        //Constructor
        public DA_XignuxDB()
        {
            try
            {
                connString = GetConnString();

            }
            catch (Exception ex)
            {
                //toDo Register in Log DataBase
            }
        }

        public List<EN_Response> GetProductStockDB(EN_ApplyCoupon entidad)
        {
            initializeVars();

            List<EN_Response> GetStock = new List<EN_Response>();
            //GetStock = GetStockData(entidad);

            var L = new List<EN_Response>();

            try
            {
                //DataTable dt = OracleDb.ExecuteNonQuery(SServer, CommandType.StoredProcedure, "SPCAMBIAESTATUS_APILIVE", parametros);
                DataTable dt = null; // OracleDb.ExecuteNonQuery(SServer, CommandType.StoredProcedure, "SPCAMBIAESTATUS_API", parametros);


                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        L.Add(new EN_Response
                        {
                            id = !DBNull.Value.Equals(dr["IDPEDIDOESTATUS"]) ? Int64.Parse(dr["IDPEDIDOESTATUS"].ToString()) : 0,
                            errorNumber = dr["V_ERR"].ToString(),
                            errorMessage = dr["V_MSJ"].ToString(),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                L.Add(new EN_Response
                {
                    errorMessage = ex.ToString()
                });
            }
            return L;
        }

        public EN_Response GetProductStockDB(int productId, bool bDapper)
        {
            initializeVars();

            EN_Response oResponse = new EN_Response();
            try
            {
                //var sql = "SELECT IdUsuario FROM Usuarios";
                ////var products = new List<Product>();
                //using (var connection = new SqlConnection(connString))
                //{
                //    connection.Open();
                //    using (var command = new SqlCommand(sql, connection))
                //    {
                //        using (var reader = command.ExecuteReader())
                //        {
                //            while (reader.Read())
                //            {
                //                //Console.WriteLine(String.Format("{0}, {1}", reader[0], reader[1]));
                //                string result = String.Format("{0}", reader[0]);
                //            }

                //            // Call Close when done reading.
                //            reader.Close();
                //        }
                //    }
                //}

                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@productId", productId);
                    parameters.Add("@RESULT", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                    var RS = connection.Execute("GETProductStock", parameters, null, null, commandType: CommandType.StoredProcedure);
                    oResponse.status = parameters.Get<int>("@RESULT");
                }
            }
            catch (Exception ex)
            {
                oResponse.errorMessage = ex.ToString();
            }
            return oResponse;
        }

        public List<EN_ApplyCouponResponse> GetCouponApplicableDB(EN_ApplyCoupon entidad)
        {
            initializeVars();

            EN_ApplyCouponResponse oENResponse = new EN_ApplyCouponResponse();

            List<EN_ApplyCouponResponse> olstENResponse = new List<EN_ApplyCouponResponse>();


            try
            {
                using (var connection = new SqlConnection(connString))
                {
                    //Set up DynamicParameters object to pass parameters  
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("CouponCode", entidad.couponCode);

                    //Execute stored procedure and map the returned result to a Customer object  
                    var QueryResult = connection.Query<EN_ApplyCouponResponse>("GETCouponDiscount", parameters, commandType: CommandType.StoredProcedure);

                    olstENResponse = (List<EN_ApplyCouponResponse>)QueryResult;

                    //oENResponse = olstENResponse[0];

                }
            }
            catch (Exception ex)
            {
                //ToDO Add LOGDB Exception 
            }
            return olstENResponse;
        }

        public List<EN_PremiumBenefitsResponse> GetPremiumBenefitsDB(int clientId)
        {

            List<EN_PremiumBenefitsResponse> olstENResponse = new List<EN_PremiumBenefitsResponse>();
            initializeVars();

            try
            {
                using (var connection = new SqlConnection(connString))
                {
                    //Set up DynamicParameters object to pass parameters  
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ClientId", clientId);

                    //Execute stored procedure and map the returned result to a Customer object  
                    var QueryResult = connection.Query<EN_PremiumBenefitsResponse>("GETPremiumBenefits", parameters, commandType: CommandType.StoredProcedure);

                    olstENResponse = (List<EN_PremiumBenefitsResponse>)QueryResult;
                }
            }
            catch (Exception ex)
            {
                olstENResponse.Add(new EN_PremiumBenefitsResponse
                {
                    errorMessage = ex.ToString()
                });
            }

            return olstENResponse;
        }

        public double CalculateShippingDB(EN_CalculateShipping entidad)
        {
            List<EN_CalculateShippingResponse> olstENResponse = new List<EN_CalculateShippingResponse>();
            EN_CalculateShippingResponse oENResponse = new EN_CalculateShippingResponse();

            initializeVars();

            try
            {
                using (var connection = new SqlConnection(connString))
                {
                    //Set up DynamicParameters object to pass parameters  
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("ClientId", entidad.ClientId);
                    parameters.Add("ProductId", entidad.ProductId);

                    //Execute stored procedure and map the returned result to a Customer object  
                    var QueryResult = connection.Query<EN_CalculateShippingResponse>("CalculateShipping", parameters, commandType: CommandType.StoredProcedure);

                    olstENResponse = (List<EN_CalculateShippingResponse>)QueryResult;

                    oENResponse = olstENResponse[0];

                    return oENResponse.ShippingCost;
                }
            }
            catch (Exception ex)
            {
                //ToDO Implement Log Action
                return -1.0;
            }
        }

        private void initializeVars()
        {
            _error = string.Empty;
            _errorConexion = false;
            _valError = false;
            _errorStoreProcedure = string.Empty;
        }

        private string GetConnString()
        {
            string KeyString = string.Empty;

            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Xignux\\");
                if (key != null)
                {

                    KeyString = key.GetValue("connString").ToString();
                    //if (KeyString == null)
                    //{
                    //   ToDo //Add to Log DataBase
                    //} 
                }
                else
                    throw new Exception("Llave de conexión no encontrada");

                return KeyString;
            }
            catch (Exception ex)
            {
                //toDo Add Log Event
                return ex.Message.ToString();
            }
        }

        private List<EN_Response> GetStockData(EN_ApplyCoupon entidad)
        {
            initializeVars();

            var L = new List<EN_Response>();

            try
            {
                DataTable dt = null;

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        _ProductId = 200;

                        L.Add(new EN_Response
                        {
                            status = 1
                        });
                    }
                }
                else
                    L.Add(new EN_Response
                    {
                        status = 0
                    });
            }
            catch (Exception ex)
            {
                L.Add(new EN_Response
                {
                    errorMessage = ex.ToString()
                });
            }
            return L;
        }
    }
}
