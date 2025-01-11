using Core.Utilities.Results;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

namespace Business.Abstract;

public interface IPaymentService
{
    IDataResult<List<PaymentDto>> GetPayments();
    IDataResult<PaymentDto> GetPaymentById(int paymentId);
    IDataResult<List<PaymentDto>> GetOverduePayments();
    IResult CreatePayment(PaymentCreateDto paymentCreateDto);
    IResult UpdatePayment(PaymentUpdateDto paymentUpdateDto);
    IResult DeletePayment(int paymentId);
    IResult NotifyParentForOverduePayment(int paymentId);
}