using Ghtk.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ghtk.Api.Controllers
{
    [Route("/services/shipment")]
    [ApiController]
    public class ShipmentServiceController(ILogger<ShipmentServiceController> logger) : ControllerBase
    {
        [HttpPost]
        [Route("order")]
        [Authorize]
        public IActionResult SubmitOrder([FromBody] SubmitOrderRequest order)
        {
            logger.LogInformation("SubmitOrder called");

            var response = new SubmitOrderResponse()
            {
                Success = true,
                Order = new SubmitOrderResponseOrder
                {
                    PartnerId = "",
                    Label = "",
                    Area = 1,
                    Fee = 1.0,
                    InsuranceFee = 1.0,
                    TrackingId = 1,
                    EstimatedPickTime = "2021-01-01T00:00:00Z",
                    EstimatedDeliverTime = "2021-01-01T00:00:00Z",
                    Products = [],
                    StatusId = 1
                }
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("v2/{id}")]
        [Authorize]
        public IActionResult GetOrderStatus(string id)
        {
            logger.LogInformation("GetOrderStatus called with id {id}", id);

            var result = new GetOrderStatusResponse()
            {
                Success = true,
                Order = new Order()
                {
                    LabelId = "label_id",
                    PartnerId = "partner_id",
                    Status = 1,
                    StatusText = "status_text",
                    Created = DateTimeOffset.Now,
                    Modified = DateTimeOffset.Now,
                    Message = "message",
                    PickDate = DateTimeOffset.Now,
                    DeliverDate = DateTimeOffset.Now,
                    CustomerFullname = "customer_fullname",
                    CustomerTel = "customer_tel",
                    Address = "address",
                    StorageDay = 1,
                    ShipMoney = 1,
                    Insurance = 1,
                    Value = 1
                }
            };

            return Ok(result);
        }

        [HttpPost]
        [Route("cancel/{id}")]
        [Authorize]
        public IActionResult CancelOrder(string id)
        {
            logger.LogInformation("CancelOrder called with id {id}", id);

            var result = new ApiResult()
            {
                Success = true,
                Message = "Order Canceled"
            };

            return Ok(result);
        }
    }
}
