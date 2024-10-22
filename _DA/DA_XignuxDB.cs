using _EN;
using _EN.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace _DA
{
    public class DA_XignuxDB
    {
        public bool _errorConexion = false;
        public bool _valError = false;
        public string _error = string.Empty;
        public string _errorStoreProcedure = string.Empty;
        private const string connString = "\"Data Source=.;Initial Catalog=PRMAC;User Id=PayRollsa;Password=PayRoll20*\" providerName=\"System.Data.SqlClient\"";

        private int _ProductId { get; set; }

        public List<EN_Response> GetProductStockDB(EN_ProductStock entidad)
        {
            initialiceVars();

            List<EN_Response> GetStock = new List<EN_Response>();
            GetStock = GetStockData(entidad);

            var L = new List<EN_Response>();
            //OracleParameter[] parametros = {
            //    new OracleParameter(":P_TIPOPEDIDO", 2),
            //    new OracleParameter(":P_IDPEDIDOESTATUS", entidad.Estatus),
            //    new OracleParameter(":P_IDPEDIDO", entidad.Id_Transaccion),
            //    new OracleParameter(":P_IDESTATUSOBS", _IdEstatusOBS),
            //    new OracleParameter(":P_COMENTARIOS", OracleDbType.Varchar2, OracleString.Null, ParameterDirection.InputOutput),
            //    new OracleParameter(":P_FECHASTATUS", OracleDate.GetSysDate()),
            //    new OracleParameter(":P_ORIGENESTATUS", 2),
            //    new OracleParameter(":P_CIA", _Cia),
            //    new OracleParameter(":P_SUC", _Suc),
            //    new OracleParameter(":P_CAJAWS", 1),
            //    new OracleParameter(":P_IDEMPLEADOWS", 1),
            //    new OracleParameter(":P_FECHAMODSUCWS", OracleDate.GetSysDate()),
            //    new OracleParameter(":P_HORAMODSUCWS", OracleDate.GetSysDate()),
            //    new OracleParameter(":P_NOMBREREPARTIDORWS", entidad.Nombre_Operador),
            //    new OracleParameter(":P_IDREPARTIDORWS", OracleDecimal.Zero),
            //    new OracleParameter(":P_IDTICKETWS", OracleDecimal.Zero),
            //    new OracleParameter(":P_DATOSEXRASWS", entidad.Live_Status),
            //    new OracleParameter(":P_IDUSUARIOCREATE", "Admin"),
            //    new OracleParameter(":P_FECHACREATE",OracleDate.GetSysDate()),
            //    new OracleParameter(":P_IDESTATUSENTREGADO", OracleDecimal.Null),
            //    new OracleParameter(":P_IDUSUARIOCANCELACION",OracleString.Null),
            //    new OracleParameter(":T_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)
            //};


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

        public List<EN_Response> GetProductStockDB(EN_ProductStock entidad, bool bDapper)
        {
            initialiceVars();

            List<EN_Response> GetStock = new List<EN_Response>();
            //GetStock = GetStockData(entidad);

            var L = new List<EN_Response>();

            try
            {

                var sql = "SELECT Count(IdProduct) FROM Products";
                //var products = new List<Product>();
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                           
                        }
                    }
                }

                DataTable dt = null;

                if (dt.Rows.Count > 0)
                {
                    //toDo Something
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
        private void initialiceVars()
        {
            _error = string.Empty;
            _errorConexion = false;
            _valError = false;
            _errorStoreProcedure = string.Empty;
        }

        private List<EN_Response> GetStockData(EN_ProductStock entidad)
        {
            initialiceVars();

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
