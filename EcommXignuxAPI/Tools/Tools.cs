using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.DirectoryServices;
using _BL;
using _EN;
using NLog.LayoutRenderers.Wrappers;

namespace EventHubLastMilleApi
{
    public class Tools
    {
        public static String GetMD5Hash(String input)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            String hash = s.ToString();
            return hash;
        }

        public static EN_Employee AuthenticateUser(EN_Employee login, IConfiguration configuration)
        {
            string conectionstring = configuration.GetConnectionString("BloggingDatabase");
            EN_Employee oEDataEmployee = new EN_Employee();
            BL_Login oBLogin = new BL_Login();

            oEDataEmployee = oBLogin.getDataUser(conectionstring, login.userName);

            if (oEDataEmployee != null)
            {
                if (login.userName.Equals(oEDataEmployee.userName))
                {

                    login.passEncrypt = Tools.GetMD5Hash(login.passEncrypt);

                    if (oEDataEmployee.passEncrypt.Equals(login.passEncrypt))
                    {
                        List<EN_EmployeeRol> oEEmployeeRoles = new List<EN_EmployeeRol>();
                        oEEmployeeRoles = oBLogin.getDataUserRoles(conectionstring, login.userName);

                        if (oEEmployeeRoles.Count > 0)
                        {
                            oEDataEmployee.roles = oEEmployeeRoles;
                        }
                    }
                    else
                    {
                        oEDataEmployee = null;
                    }
                }
            }
            return oEDataEmployee;
        }

        public static string GenerateJSONWebToken(EN_Employee userInfo, IConfiguration configuration)
        {
            int timeOutToken = 0;
            var encodetoken = string.Empty;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt").GetSection("Key").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userInfo.userName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            if (userInfo.roles.Count > 0)
            {
                timeOutToken = 60;

                //if (userInfo.roles.Count < 1)
                //{
                //    if (userInfo.roles[0].roleName.Equals("Operator"))
                //    {
                //        //timeOutToken = 480;
                //        timeOutToken = 60;
                //    }
                //    else if (userInfo.roles[0].roleName.Equals("Modify"))
                //    {
                //        //                        timeOutToken = 30;
                //        timeOutToken = 30;
                //    }
                //    else if (userInfo.roles[0].roleName.Equals("Admin"))
                //    {
                //        timeOutToken = 60;
                //    }
                //    else
                //    {
                //        timeOutToken = 0;
                //    }
                //}
                //else
                //{
                //    if (userInfo.roles.Any(item => item.roleName.Equals("Operator")))
                //    {
                //        //timeOutToken = 30;
                //        timeOutToken = 30;
                //    }
                //    else if (userInfo.roles.Any(item => item.roleName.Equals("Modify")))
                //    {
                //        //                         timeOutToken = 480;
                //        timeOutToken = 60;
                //    }
                //    else if (userInfo.roles[0].roleName.Equals("Admin"))
                //    {
                //        timeOutToken = 60;
                //    }
                //    else
                //    {
                //        timeOutToken = 0;
                //    }
                //}
            }

            if (!timeOutToken.Equals(0))
            {
                var token = new JwtSecurityToken(
                    issuer: configuration.GetSection("Jwt").GetSection("Issuer").Value,
                    audience: configuration.GetSection("Jwt").GetSection("Issuer").Value,
                    claims,
                    expires: DateTime.Now.AddMinutes(timeOutToken),
                    signingCredentials: credentials
                 );

                encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            }

            return encodetoken;
        }


        public static EN_Employee userHardCore()
        {
            EN_Employee Employee = new EN_Employee
            {
                employeeId = 1,
                employeeNumber = 1,
                email = "hfloresj@benavides.com.mx",
                name = "Hector Manuel Flores Jaimes",
                passEncrypt = "1d7c91c395aa76dd27b3191ca96b730f",
                userName = "hfloresj"
            };

            List<EN_EmployeeRol> oEEmployeeRoles = new List<EN_EmployeeRol>();

            EN_EmployeeRol er = new EN_EmployeeRol
            {
                roleName = "SuperAdmin"
            };

            oEEmployeeRoles.Add(er);
            Employee.roles = oEEmployeeRoles;

            return Employee;
        }

        public static bool  AuthenticateUserAD(string userName, string password)
        {
            bool result = false;

            try
            {

                DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath(), userName, password);
                DirectorySearcher dsearch = new DirectorySearcher(de);
                dsearch.Filter = string.Concat("sAMAccountName", userName);

                SearchResult results = null;

                results = dsearch.FindOne();

                if (results != null) {
                    result = true;              
                }

            }
            catch (Exception ex) {           
            
            }

            return result;
        }

        private static string GetCurrentDomainPath()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");
            //LDAP://na.miempresa.com
            return String.Concat("LDAP://", de.Properties["defaultNamingContext"][0].ToString());
        }
    }
}
