using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _DA;
using _EN;

namespace _BL
{
    public class BL_Login
    {
        public bool _errorConexion = false;
        public bool _valError = false;
        public string _error = string.Empty;
        public string _errorStoreProcedure = string.Empty;

        public EN_Employee getDataUser(string SServer, string userName)
        {
            DA_Login oData = new DA_Login();
            var response = oData.getDataUser(SServer, userName);

            requestErrors(oData);
            return response;
        }

        public List<EN_EmployeeRol> getDataUserRoles(string SServer, string userName)
        {
            DA_Login oData = new DA_Login();
            var response = oData.getDataUserRoles(SServer, userName);

            requestErrors(oData);
            return response;
        }       

        private void requestErrors(DA_Login oData)
        {
            _error = oData._error;
            _errorConexion = oData._errorConexion;
            _valError = oData._valError;
            _errorStoreProcedure = oData._errorStoreProcedure;
        }
    }
}
