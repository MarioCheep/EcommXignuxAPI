using _EN;
using Microsoft.Data.SqlClient;
using System.Data;

namespace _DA
{
    public class DA_Login
    {
        public bool _errorConexion = false;
        public bool _valError = false;
        public string _error = string.Empty;
        public string _errorStoreProcedure = string.Empty;

        public EN_Employee getDataUser(string SServer, string userName)
        {
            initialiceVars();

            var L = new EN_Employee();
            SqlParameter[] parametros = {
                    new SqlParameter("@correo",userName)
            };

            try
            {
                DataTable dt = null; // SQL.ExecuteNonQuery(SServer, CommandType.StoredProcedure, "GET_CatUsuarios", parametros);

                if (dt.Rows.Count > 0)
                {
                    L = new EN_Employee()
                    {
                        employeeId = Int64.Parse(dt.Rows[0]["idUsuario"].ToString() == "" ? "0" : dt.Rows[0]["idUsuario"].ToString()),
                        name = dt.Rows[0]["nombreCompleto"].ToString(),
                        passEncrypt = dt.Rows[0]["passEncrypt"].ToString(),
                        email = dt.Rows[0]["correo"].ToString(),
                        userName = dt.Rows[0]["userName"].ToString(),
                        employeeNumber = Int64.Parse(dt.Rows[0]["employeeNumber"].ToString() == "" ? "0" : dt.Rows[0]["employeeNumber"].ToString())
                    };
                }
            }
            catch (Exception ex)
            {
                L = new EN_Employee
                {
                     name = ex.ToString()
                };
            }
            return L;
        }

        public List<EN_EmployeeRol> getDataUserRoles(string SServer, string userName)
        {
            initialiceVars();

            var L = new List<EN_EmployeeRol>();
            SqlParameter[] parametros = {
                    new SqlParameter("@correo",userName)
            };

            try
            {
                DataTable dt = null; // SQL.ExecuteNonQuery(SServer, CommandType.StoredProcedure, "GET_CatUsuarioRoles", parametros);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        L.Add(new EN_EmployeeRol
                        {
                            roleName = dt.Rows[0]["roleName"].ToString(),
                            category = dt.Rows[0]["category"].ToString(),
                            groupName = dt.Rows[0]["groupName"].ToString()
                        });
                    }
                }           
            }
            catch (Exception ex)
            {
                L.Add(new EN_EmployeeRol
                {
                    roleName = ex.ToString()
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
    }
}
