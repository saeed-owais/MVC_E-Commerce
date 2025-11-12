using BLL.DTOs.Payment;
using BLL.Services.Payment;
using DA.Models;
using DAL.Enums;
using DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Json;


public class MockPaymentService : IMockPaymentService
{
    private readonly IUnitOfWork _uow;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _cfg;

    //private readonly string _stripeSecret;
    //private readonly string _fawryMerchant;
    //private readonly string _fawrySecret;
    //private readonly string _fawryBaseUrl;

    private readonly string _paymobApiKey;
    private readonly string _paymobIframeId;
    private readonly string _paymobIntegrationId;
    private readonly string _paymobCallbackUrl;
    private readonly string _paymobBaseUrl;

    public MockPaymentService(IUnitOfWork uow, IHttpClientFactory httpClientFactory, IConfiguration cfg)
    {
        _uow = uow;
        _httpClientFactory = httpClientFactory;
        _cfg = cfg;

       

        _paymobApiKey = cfg["Payments:Paymob:ApiKey"] ?? "";
        _paymobIframeId = cfg["Payments:Paymob:IframeId"] ?? "";
        _paymobIntegrationId = cfg["Payments:Paymob:IntegrationId"] ?? "";
        _paymobCallbackUrl = cfg["Payments:Paymob:CallbackUrl"] ?? "";
        _paymobBaseUrl = cfg["Payments:Paymob:BaseUrl"] ?? "https://accept.paymob.com/api";
    }

    public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto dto)
    {
        var method = (dto.PaymentMethod ?? "").Trim().ToLowerInvariant();

        return method switch
        {
            //"stripe" => await PayWithStripe(dto),
            //"fawry" => await PayWithFawry(dto),
            "paymob" => await PayWithPaymob(dto),
            _ => new PaymentResponseDto { IsSuccessful = false, Message = "Unsupported payment method." }
        };
    }

    // ✅ Paymob Integration
    private async Task<PaymentResponseDto> PayWithPaymob(PaymentRequestDto dto)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            // 1️⃣ Get Auth Token
            var authResp = await client.PostAsJsonAsync($"{_paymobBaseUrl}/auth/tokens", new { api_key = _paymobApiKey });
            var authJson = await authResp.Content.ReadFromJsonAsync<JsonElement>();
            var token = authJson.GetProperty("token").GetString();

            // 2️⃣ Register Order
            var orderData = new
            {
                auth_token = token,
                delivery_needed = false,
                amount_cents = (long)(dto.Amount * 100),
                currency = "EGP",
                items = new object[] { },
                merchant_order_id = dto.OrderId
            };
            var orderResp = await client.PostAsJsonAsync($"{_paymobBaseUrl}/ecommerce/orders", orderData);
            var orderJson = await orderResp.Content.ReadFromJsonAsync<JsonElement>();
            var orderId = orderJson.GetProperty("id").GetInt32();

            // 3️⃣ Request Payment Key
            var paymentKeyReq = new
            {
                auth_token = token,
                amount_cents = (long)(dto.Amount * 100),
                expiration = 3600,
                order_id = orderId,
                billing_data = new
                {
                    apartment = "NA",
                    email = "customer@example.com",
                    floor = "NA",
                    first_name = "Customer",
                    last_name = "Test",
                    phone_number = "+201000000000",
                    street = "NA",
                    city = "Cairo",
                    country = "EG",
                    state = "Cairo"
                },
                currency = "EGP",
                integration_id = _paymobIntegrationId,
                lock_order_when_paid = true
            };

            var paymentResp = await client.PostAsJsonAsync($"{_paymobBaseUrl}/acceptance/payment_keys", paymentKeyReq);
            var paymentJson = await paymentResp.Content.ReadFromJsonAsync<JsonElement>();
            var paymentKey = paymentJson.GetProperty("token").GetString();

            // 4️⃣ Generate Payment Link
            var paymentUrl = $"https://accept.paymob.com/api/acceptance/iframes/{_paymobIframeId}?payment_token={paymentKey}";

            // 5️⃣ Save payment entry in DB
            var pay = new Payment
            {
                OrderId = dto.OrderId,
                PaymentMethod = PaymentMethod.Paymob,
                TransactionId = paymentKey,
                IsSuccessful = false // نعتبرها لم تتم بعد لحين Callback
            };
            await _uow.Payments.AddAsync(pay);
            await _uow.CompleteAsync();

            return new PaymentResponseDto
            {
                IsSuccessful = true,
                TransactionId = paymentKey,
                Message = paymentUrl // نرجع الرابط نفسه عشان نوجّه العميل عليه
            };
        }
        catch (Exception ex)
        {
            return new PaymentResponseDto { IsSuccessful = false, Message = ex.Message };
        }
    }

}
