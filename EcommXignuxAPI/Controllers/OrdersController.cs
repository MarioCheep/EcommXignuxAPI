using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using _EN;
using _BL;
using Newtonsoft.Json;
using Azure;
using Newtonsoft.Json.Linq;
using static Azure.Core.HttpHeader;
using Microsoft.AspNetCore.Rewrite;
using Swashbuckle.AspNetCore.Annotations;
using System.Drawing;
using _EN.Entitys;
using System.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EventHubLastMille.Controllers
{
    /// <summary>
    /// Controler Maneja todo lo relacionado con Pedidos
    /// </summary>

    [Route("api/[controller]/[action]")]
    [ApiController]
    [SwaggerSubType(typeof(EN_ChangeOrderStatus))]
    public class OrdersController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly string OracleCString;
        private readonly string AppSettings;
        private readonly EN_Proxy Proxy;

        /// <summary>
        /// Recupera la informacion de configuracion
        /// para poder acceder a ella dentro del controller 
        /// </summary>
        public OrdersController(IConfiguration config)
        {
            this.configuration = config;

            //if (bool.Parse(configuration.GetSection("AppSettings").GetSection("Prod").Value))
            //{
            this.OracleCString = configuration.GetConnectionString("OracleDbPrd");
            this.AppSettings = configuration.GetConnectionString("AppSettings");

            this.Proxy = new EN_Proxy()
            {
                strWebProxy = configuration.GetSection("AppSettings").GetSection("strWebProxy").Value,
                strPort = configuration.GetSection("AppSettings").GetSection("strPort").Value,
                strUserName = configuration.GetSection("AppSettings").GetSection("strUserName").Value,
                strPassword = configuration.GetSection("AppSettings").GetSection("strPassword").Value,
                strDominio = configuration.GetSection("AppSettings").GetSection("strDominio").Value,
            };

            //}
            //else
            //{
            //this.OracleCString = configuration.GetConnectionString("OracleDb");
            //}

        }

        /// <summary>
        /// Update Status Orders Sad Web
        /// </summary>
        /// <remarks>
        ///     Sample **request**:
        ///     
        ///         POST /api/Orders
        ///         {
        ///             "cajaTicketOriginal": "14031104004425",
        ///             "datoAdicional": "JUANPABLO|SVT",
        ///             "fechaHoraPos": "2023-08-02T00:05:52.759Z",
        ///             "idPedido": 133885,
        ///             "localCtf": 2311,
        ///             "localCtfSpecified": true,
        ///             "numEmpleado": 11016973,
        ///             "numEmpleadoSpecified": true,
        ///             "pedidoInternet": "SVT10747",
        ///             "productos": [
        ///                               {
        ///                                  "cantidad": 2,
        ///                                  "cantidadSpecified": true,
        ///                                  "motivo": "Pedido Entregado a CLIENTE",
        ///                                  "nuevoEstatus": 03,
        ///                                  "sku": "2",
        ///                                  "skuSpecified": true
        ///                               },
        ///                                {
        ///                                  "cantidad": 8,
        ///                                  "cantidadSpecified": true,
        ///                                  "motivo": "Pedido Entregado a CLIENTE",
        ///                                  "nuevoEstatus": 03,
        ///                                  "sku": "7",
        ///                                  "skuSpecified": true
        ///                               }
        ///                          ]
        ///         }
        /// </remarks>
        /// <param name="entidad"></param>
        /// <param name="entidadParameters"></param>
        /// <response code="200">Success</response>
        /// <response code="201">Create a tag in the system</response>
        /// <response code="400">Unable to create tag due to validation error</response>
        /// <response code="401">Error: Unauthorized | Token no Found or Fail</response>
        /// 
        ///[Authorize]
        [Produces(contentType: "application/json")]
        [HttpPost(Name = "ChangeOrderStatus")]
        public IActionResult ChangeOrderStatus([FromBody] EN_DataObs entidad = default, [FromQuery] EN_DataObs entidadParameters = default)
        {
            List<EN_Response> oEResponse = new List<EN_Response>();

            if (ModelState.IsValid)
            {
                BL_ChanceOrderStatus oBLogic = new BL_ChanceOrderStatus();

                if (entidad != null)
                {
                    oEResponse = oBLogic.ChangeOrderStatus(OracleCString, Proxy, entidad);
                }
                else if (entidadParameters.IdPedido.ToString() != null & entidadParameters.IdPedido != 0)
                {
                    oEResponse = oBLogic.ChangeOrderStatus(OracleCString, Proxy, entidadParameters);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

            var json = JsonConvert.SerializeObject(oEResponse, Formatting.None);

            if (oEResponse.FirstOrDefault().status.Equals(400))
            {
                return BadRequest(json);
            }

            return Created("~api/Orders/", json);
        }

        /// <summary>
        /// SET DEVOLUCION DE PEDIDO (ChangeOrderReturn)
        /// </summary>
        /// <remarks>
        ///     Sample **request**:
        ///     
        ///         POST /api/Orders
        ///         {
        ///             "cajaTicketOriginal": "14031104004425",
        ///             "datoAdicional": "JUANPABLO|SVT",
        ///             "fechaHoraPos": "2023-08-02T00:05:52.759Z",
        ///             "idPedido": 133885,
        ///             "localCtf": 2311,
        ///             "localCtfSpecified": true,
        ///             "numEmpleado": 11016973,
        ///             "numEmpleadoSpecified": true,
        ///             "pedidoInternet": "SVT10747",
        ///             "productos": [
        ///                               {
        ///                                  "cantidad": 2,
        ///                                  "cantidadSpecified": true,
        ///                                  "motivo": "Pedido Entregado a CLIENTE",
        ///                                  "nuevoEstatus": 03,
        ///                                  "sku": "1026303",
        ///                                  "skuSpecified": true
        ///                               },
        ///                                {
        ///                                  "cantidad": 8,
        ///                                  "cantidadSpecified": true,
        ///                                  "motivo": "Pedido Entregado a CLIENTE",
        ///                                  "nuevoEstatus": 03,
        ///                                  "sku": "1021317",
        ///                                  "skuSpecified": true
        ///                               }
        ///                          ]
        ///         }
        /// </remarks>
        /// <param name="entidad"></param>
        /// <param name="entidadParameters"></param>
        /// <response code="200">Success</response>
        /// <response code="201">Create a tag in the system</response>
        /// <response code="400">Unable to create tag due to validation error</response>
        /// <response code="401">Error: Unauthorized | Token no Found or Fail</response>
        /// 
        ///[Authorize]
        [Produces(contentType: "application/json")]
        [HttpPost(Name = "OrderReturn")]
        public IActionResult OrderReturn([FromBody] EN_DataObs entidad = default, [FromQuery] EN_DataObs entidadParameters = default)
        {
            EN_Response oEResponse = new EN_Response();

            if (ModelState.IsValid)
            {
                BL_ChanceOrderStatus oBLogic = new BL_ChanceOrderStatus();

                if (entidad != null)
                {
                    oEResponse = oBLogic.OrderReturn(OracleCString, Proxy, entidad);
                }
                else if (entidadParameters.IdPedido.ToString() != null & entidadParameters.IdPedido != 0)
                {
                    oEResponse = oBLogic.OrderReturn(OracleCString, Proxy, entidadParameters);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

            var json = JsonConvert.SerializeObject(oEResponse, Formatting.None);

            if (oEResponse.status.Equals(400))
            {
                return BadRequest(json);
            }

            return Created("~api/Orders/", json);
        }

        /// <summary>
        /// Sync Orders Live Status SVT
        /// </summary>
        /// <remarks>
        ///     Sample **request**:
        ///     
        ///         POST /api/Orders
        ///         {
        ///             "codigo_comercio": "SVT",
        ///             "id_transaccion": "SVT12345",
        ///             "numero_local": 2311,
        ///             "estatus": "PROCESSING",
        ///             "operador_fecha": "2023-08-02T00:05:52.759Z",
        ///             "nombre_operador": "Mensajeros Urbanos",
        ///             "guia_operador": "123456",
        ///             "live_status": "https://xandar-lsw-v3.instaleap.io/?job=37045ff1-ae90-4a3f-aba7-132e37cf7fa4&token=ecKOOonqh18MsFola9Y25sicV6cEmQ2FLgozAcLr",
        ///         }
        /// </remarks>
        /// <param name="entityReq"></param>
        /// <param name="entityReqParam"></param>
        /// <response code="200">Success</response>
        /// <response code="201">Create a tag in the system</response>
        /// <response code="400">Unable to create tag due to validation error</response>
        /// <response code="401">Error: Unauthorized | Token no Found or Fail</response>
        /// 
        ///[Authorize]
        [Produces(contentType: "application/json")]
        [HttpPost(Name = "SyncOrderLiveStatus")]
        public IActionResult SyncOrderLiveStatus([FromBody] EN_SetOrderLiveStatus entityReq = default, [FromQuery] EN_SetOrderLiveStatus entityReqParam = default)
        {
            List<EN_Response> oEResponse = new List<EN_Response>();

            if (ModelState.IsValid)
            {
                BL_SetOrderLiveStatus oBLogic = new BL_SetOrderLiveStatus();

                if (entityReq != null)
                {
                    oEResponse = oBLogic.SetOrderLiveStatus(OracleCString, Proxy, entityReq);
                }
                else
                {
                    oEResponse = oBLogic.SetOrderLiveStatus(OracleCString, Proxy, entityReqParam);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

            var json = JsonConvert.SerializeObject(oEResponse, Formatting.None);

            if (oEResponse.FirstOrDefault().status.Equals(400))
            {
                return BadRequest(json);
            }

            return Created("~api/Orders/", json);
        }
    }
}
