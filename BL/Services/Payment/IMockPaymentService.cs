using BLL.DTOs.Payment;


namespace BLL.Services.Payment
{
    public interface IMockPaymentService
    {
        public Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto dto);

    }
}
