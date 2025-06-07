using Ghtk.Api.Models;
using Ghtk.Repository;
using Ghtk.Repository.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ghtk.Api.Controllers
{
    [Route("/services/shipment")]
    [ApiController]
    public class ShipmentServiceController(IOrderRepository orderRepository, ILogger<ShipmentServiceController> logger) : ControllerBase
    {
        [HttpPost]
        [Route("order")]
        [Authorize]
        public async Task<IActionResult> SubmitOrderAsync([FromBody] SubmitOrderRequest order)
        {
            logger.LogInformation("SubmitOrder called");

            var partnerId = User.FindFirst("PartnerId")?.Value;
            if (string.IsNullOrEmpty(partnerId))
            {
                return Unauthorized();
            }

            var orderEntity = new Order()
            {
                Id = Guid.NewGuid().ToString(),
                PartnerId = partnerId,
                PickName = order.Order.PickName,
                PickAddress = order.Order.PickAddress,
                PickProvince = order.Order.PickProvince,
                PickDistrict = order.Order.PickDistrict,
                PickWard = order.Order.PickWard,
                PickTel = order.Order.PickTel,
                Tel = order.Order.Tel,
                Name = order.Order.Name,
                Address = order.Order.Address,
                Province = order.Order.Province,
                District = order.Order.District,
                Ward = order.Order.Ward,
                Hamlet = order.Order.Hamlet,
                IsFreeship = 1,
                PickDate = DateTimeOffset.Now,
                PickMoney = 1,
                Note = "note",
                Value = 1,
                Transport = "transport",
                PickOption = "pick_option",
                DeliverOption = "deliver_option",
                TrackingId = Guid.NewGuid().ToString(),
                Status = 1,

                Products = order.Products.Select(p => new Product
                {
                    Name = p.Name,
                    Quantity = p.Quantity,
                    Weight = p.Weight,
                    ProductCode = p.ProductCode,
                }).ToList()
            };

            await orderRepository.CreateOrderAsync(orderEntity);

            var response = new SubmitOrderResponse()
            {
                Success = true,
                Order = new SubmitOrderResponseOrder
                {
                    PartnerId = partnerId,
                    Label = "",
                    Area = 1,
                    Fee = 1.0,
                    InsuranceFee = 1.0,
                    TrackingId = orderEntity.TrackingId,
                    EstimatedPickTime = "2021-01-01T00:00:00Z",
                    EstimatedDeliverTime = "2021-01-01T00:00:00Z",
                    Products = order.Products.Select(p => new OrderProduct()
                    {
                        Name = p.Name,
                        Quantity = p.Quantity,
                        Weight = p.Weight,
                        ProductCode = p.ProductCode,
                    }).ToArray(),
                    StatusId = orderEntity.Status
                }
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("v2/{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderStatusAsync(string id)
        {
            logger.LogInformation("GetOrderStatus called with id {id}", id);

            var partnerId = User.FindFirst("PartnerId")?.Value;
            if (string.IsNullOrEmpty(partnerId))
            {
                return Unauthorized();
            }
            var order = await orderRepository.FindOrderAsync(id, partnerId);
            if (order == null)
            {
                return NotFound(new ApiResult()
                {
                    Success = false,
                    Message = "Tracking Id not found"
                });
            }

            var result = new GetOrderStatusResponse()
            {
                Success = true,
                Order = new GetOrderStatusOrder()
                {
                    LabelId = "Label_id",
                    PartnerId = partnerId,
                    Status = order.Status,
                    StatusText = "status_text",
                    Created = DateTimeOffset.Now,
                    Modified = DateTimeOffset.Now,
                    Message = "message",
                    PickDate = DateTimeOffset.Now,
                    DeliverDate = DateTimeOffset.Now,
                    CustomerFullname = "customer_fullname",
                    CustomerTel = "customer_tel",
                    Address = order.Address,
                    StorageDay = 1,
                    ShipMoney = 1,
                    Insurance = 1,
                    Value = order.Value
                }
            };

            return Ok(result);
        }

        [HttpPost]
        [Route("cancel/{id}")]
        [Authorize]
        public async Task<IActionResult> CancelOrderAsync(string id)
        {
            logger.LogInformation("CancelOrder called with id {id}", id);

            var partnerId = User.FindFirst("PartnerId")?.Value;
            if (string.IsNullOrEmpty(partnerId))
            {
                return Unauthorized();
            }

            var b = await orderRepository.CancelOrderAsync(id, partnerId);
            if (!b)
            {
                return NotFound(new ApiResult()
                {
                    Success = false,
                    Message = "Tracking Id not found"
                });
            }

            var result = new ApiResult()
            {
                Success = true,
                Message = "Order Canceled"
            };

            return Ok(result);
        }
    }
}
