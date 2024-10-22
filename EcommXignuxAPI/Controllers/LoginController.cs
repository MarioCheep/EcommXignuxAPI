using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using _EN;
using _BL;
using Newtonsoft.Json;


namespace EventHubLastMilleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class loginController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public loginController(IConfiguration config)
        {
            this.configuration = config;
        }

        [HttpGet]
        public IActionResult Login(string userName, string pass)
        {
            EN_Employee Employee = new EN_Employee();

            EN_Employee login = new EN_Employee();
            login.userName = userName;
            login.passEncrypt = pass;

            IActionResult response = Unauthorized();

            //EN_Employee Employee = Tools.AuthenticateUser(login, configuration);  //Autentificacion por Uusario en base de datos
            Employee = Tools.userHardCore(); // Datos de usuario Hardcore

            //Autentificacion por usuario de Active Directory LDAP
            //bool EmployeeValidAd = Tools.AuthenticateUserAD(userName, pass);
            
            //if (EmployeeValidAd)
            //{
            //    Employee = login;
            //}
            //===================================================================



            if (Employee != null)
            {
                if (Employee.name != null)
                {
                    Employee.token = Tools.GenerateJSONWebToken(Employee, configuration);

                    //response = Ok(new { oEEmployee = Employee });
                    var json = JsonConvert.SerializeObject(Employee, Formatting.None);
                    return Ok(json);

                }
            }

            return response;
        }
    }
}