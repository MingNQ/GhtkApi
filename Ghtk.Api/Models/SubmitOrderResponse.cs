using System.Text.Json.Serialization;

namespace Ghtk.Api.Models
{
    public class SubmitOrderResponse : ApiResult
    {
        [JsonPropertyName("order")]
        public SubmitOrderResponseOrder Order { get; set; } = default!;
    }

    public class SubmitOrderResponseOrder
    {
        [JsonPropertyName("partner_id")]
        public string PartnerId { get; set; } = default!;

        [JsonPropertyName("label")]
        public string Label { get; set; } = default!;

        [JsonPropertyName("area")]
        public int Area { get; set; }

        [JsonPropertyName("fee")]
        public double Fee { get; set; }

        [JsonPropertyName("insurance_fee")]
        public double InsuranceFee { get; set; }

        [JsonPropertyName("tracking_id")]
        public string TrackingId { get; set; } = default!;

        [JsonPropertyName("estimated_pick_time")]
        public string EstimatedPickTime { get; set; } = default!;

        [JsonPropertyName("estimated_deliver_time")]
        public string EstimatedDeliverTime { get; set; } = default!;

        [JsonPropertyName("products")]
        public OrderProduct[] Products { get; set; } = [];

        [JsonPropertyName("status_id")]
        public long StatusId { get; set; }
    }
}
