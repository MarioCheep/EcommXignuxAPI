using _BL;
using _EN;
using _EN.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventHubLastMille.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ECommXignuxController : ControllerBase
    {
        //// GET: api/<ValuesController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        /// <summary>
        /// Get Products Stock
        /// </summary>
        /// <remarks>
        ///     Sample **request**:
        ///     
        ///         Get /api/Stock
        ///         {
        ///             "productId": "123",
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
        [HttpGet(Name = "GetProductsStock")]
        public IActionResult GetProductsStock([FromBody] EN_ProductStock entityReq = default, [FromQuery] EN_ProductStock entityReqParam = default)
        {
            List<EN_Response> oEResponse = new List<EN_Response>();

            if (ModelState.IsValid)
            {
                BL_ECommXignux oBLogic = new BL_ECommXignux();

                if (entityReq != null)
                {
                    oEResponse = oBLogic.GetProductStock(entityReq);
                }
                else
                {
                    oEResponse = oBLogic.GetProductStock(entityReqParam);
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

            return Created("~api/ECommXignux/", json);
        }


        //// GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<ValuesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
