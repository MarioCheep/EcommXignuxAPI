using _EN;
using System.Data;

namespace _DA
{
    public class DA_LogEvent
    {
        public bool _errorConexion = false;
        public bool _valError = false;
        public string _error = string.Empty;
        public string _errorStoreProcedure = string.Empty;
        //public static LogEventService LogEvntSvc = new LogEventService();
        public string SServer { get; set; }
        public EN_Proxy Proxy { get; set; }

        public DA_LogEvent()
        {
        }

        public Task<EN_Response> SaveLogDB(decimal NUMPEDIDO, string ACCION, string MODULO, string NOMBRESP, DateTime FECHAINICIO,
        DateTime FECHAFIN, decimal TIEMPOS, string OBSERVACIONES, string REQUESTBODY, string Creado = "")
        {
            initialiceVars();
            SServer = SServer.TrimEnd('\r', '\n');
            //SServer = Crypt.Desencriptar(SServer);

            var L = new EN_Response();
            //OracleParameter[] parametros = {
            //    new OracleParameter(":P_NUMPEDIDO",NUMPEDIDO),
            //    new OracleParameter(":P_ACCION",ACCION),
            //    new OracleParameter(":P_MODULO",MODULO),
            //    new OracleParameter(":P_NOMBRESP",NOMBRESP),
            //    new OracleParameter(":P_FECHAINICIO",FECHAINICIO),
            //    new OracleParameter(":P_FECHAFIN",FECHAFIN),
            //    new OracleParameter(":P_TIEMPOS",TIEMPOS),
            //    new OracleParameter(":P_FECHACREACION",DateTime.Now),
            //    new OracleParameter(":P_CREADOPOR",string.Format("{0}", string.IsNullOrEmpty(Creado) ? string.Empty : Creado)),
            //    new OracleParameter(":P_OBSERVACIONES",string.Format("SADLOG {0}", string.IsNullOrEmpty(OBSERVACIONES) ? string.Empty : OBSERVACIONES)),
            //    new OracleParameter(":P_REQUESTBODY",REQUESTBODY),

            //    new OracleParameter (":T_CURSOR",OracleDbType.RefCursor,ParameterDirection.Output)
            //};

            try
            {
                DataTable dt = null; //OracleDb.ExecuteNonQuery(SServer, CommandType.StoredProcedure, "SPLOGPROCESOSAD", parametros);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        L = new EN_Response
                        {
                            //errorNumber = !DBNull.Value.Equals(dr["V_ERR"]) ? (String)(dr["V_ERR"]) : String.Empty,
                            //errorMessage = !DBNull.Value.Equals(dr["V_MSJ"]) ? (String)(dr["V_MSJ"]) : String.Empty
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                L = new EN_Response
                {
                    errorMessage = ex.ToString()
                };

                //LogEvntSvc.LogErrorAsync(ex.ToString(), EventLogEntryType.Error);
            }
            //return L;
            return Task.FromResult(L);
        }
        private Task initialiceVars()
        {
            //Task.Yield(); // Esta línea permite que la función sea asincrónica sin bloquear el hilo actual.

            _error = string.Empty;
            _errorConexion = false;
            _valError = false;
            _errorStoreProcedure = string.Empty;

            return Task.CompletedTask;
        }
    }
}
