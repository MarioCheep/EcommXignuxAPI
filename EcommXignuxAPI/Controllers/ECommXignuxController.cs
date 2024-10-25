using _BL;
using _EN;
using _EN.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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

        /// <summary>
        /// Get Products Stock
        /// </summary>
        /// <remarks>
        ///     Sample **request**:
        ///         Get /api/Stock/{productId}
        /// </remarks>
        /// <response code="200">Success</response>
        /// <response code="201">Create a tag in the system</response>
        /// <response code="400">Unable to create tag due to validation error</response>
        /// <response code="401">Error: Unauthorized | Token no Found or Fail</response>
        /// 
        ///[Authorize]
        [HttpGet("Stock/{productId}")]
        public bool Stock(int productId)
        {
            bool bResult = false;
            EN_Response oEResponse = new EN_Response();

            if (ModelState.IsValid)
            {
                BL_ECommXignux oBLogic = new BL_ECommXignux();

                oEResponse = oBLogic.GetProductStock(productId);

                bResult = (oEResponse.status == 0 ? false : true);
            }
            else
                oEResponse.errorMessage = BadRequest(ModelState).ToString();

            return bResult;
        }

        /// <summary>
        /// Apply Coupon
        /// </summary>
        /// <remarks>
        ///     Sample **request**:
        ///     
        ///         Post /api/apply-coupon
        ///         {
        ///             "couponCode": "XN20241026",
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
        [HttpPost("apply-coupon")]
        public IActionResult applyCoupon([FromBody] EN_ApplyCoupon entityReq = default, [FromQuery] EN_ApplyCoupon entityReqParam = default)
        {
            List<EN_ApplyCouponResponse> oEResponse = new List<EN_ApplyCouponResponse>();

            if (ModelState.IsValid)
            {
                BL_ECommXignux oBLogic = new BL_ECommXignux();

                if (entityReq != null)
                {
                    oEResponse = oBLogic.CouponApplicable(entityReq);
                }
                else
                {
                    oEResponse = oBLogic.CouponApplicable(entityReqParam);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

            var json = JsonConvert.SerializeObject(oEResponse, Formatting.None);

            return Created("~api/ECommXignux/apply-coupon", json);
        }



        /// <summary>
        /// Premium Benefits
        /// </summary>
        /// <remarks>
        ///     Sample **request**:
        ///         Get /api/premium-benefits/{clientId}
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
        [HttpGet("premium-benefits/{clientId}")]
        public IActionResult GetPremiumBenefits(int clientId)
        {
            List<EN_PremiumBenefitsResponse> oEResponse = new List<EN_PremiumBenefitsResponse>();

            if (ModelState.IsValid)
            {
                BL_ECommXignux oBLogic = new BL_ECommXignux();

                oEResponse = oBLogic.GetPremiumBenefits(clientId);
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

            return Created("~api/ECommXignux/premium-benefits", json);
        }

        /// <summary>
        /// Calculate Shipping
        /// </summary>
        /// <remarks>
        ///     Sample **request**:
        ///         Post /api/calculate-shipping/
        ///          {
        ///             "ClientId": "1",
        ///             "ProductId": "1",
        ///         }
        /// </remarks>
        /// <response code="200">Success</response>
        /// <response code="201">Create a tag in the system</response>
        /// <response code="400">Unable to create tag due to validation error</response>
        /// <response code="401">Error: Unauthorized | Token no Found or Fail</response>
        /// 
        ///[Authorize]
        [Produces(contentType: "application/json")]
        [HttpPost("calculate-shipping/")]
        public double CalculateShipping([FromBody] EN_CalculateShipping entityReq = default, [FromQuery] EN_CalculateShipping entityReqParam = default)
        {

            if (ModelState.IsValid)
            {
                BL_ECommXignux oBLogic = new BL_ECommXignux();
                EN_Response GetStockResponse = new EN_Response();
                bool bStockAvailable = false;
                double dblShippingCost = 0;

                EN_CalculateShipping CalculateShippingRequest = new EN_CalculateShipping();

                if (entityReq != null)
                    CalculateShippingRequest = entityReq;
                else
                    CalculateShippingRequest = entityReqParam;

                GetStockResponse = oBLogic.GetProductStock(CalculateShippingRequest.ProductId);

                bStockAvailable = (GetStockResponse.status == 0 ? false : true);

                //Validate Stock
                if (bStockAvailable)
                    dblShippingCost = oBLogic.CalculateShipping(CalculateShippingRequest);

                return dblShippingCost;
            }
            else
            {
                //ToDO Implement Log Action
                return -4;
            }
        }

        /// <summary>
        /// Estimated Delivery
        /// </summary>
        /// <remarks>
        ///     Sample **request**:
        ///     
        ///         Get /api/estimated-delivery
        ///         {
        ///             "OrderId": "1",
        ///             "ProductId": "1",
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
        [HttpGet("estimated-delivery")]
        public IActionResult estimatedDelivery([FromBody] EN_EstimatedDelivery entityReq = default, [FromQuery] EN_EstimatedDelivery entityReqParam = default)
        {
            EN_EstimatedDeliveryResponse oEResponse = new EN_EstimatedDeliveryResponse();

            if (ModelState.IsValid)
            {
                BL_ECommXignux oBLogic = new BL_ECommXignux();

                if (entityReq != null)
                {
                    oEResponse = oBLogic.GetEstimatedDelivery(entityReq);
                }
                else
                {
                    oEResponse = oBLogic.GetEstimatedDelivery(entityReqParam);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

            var json = JsonConvert.SerializeObject(oEResponse, Formatting.None);

            return Created("~api/ECommXignux/estimated-delivery", json);
        }


        //// POST api/<ValuesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
