using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

namespace Business.Concrete;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public IDataResult<List<PaymentDto>> GetPayments()
    {
        var payments = _paymentRepository.GetList(null ,includeProperties: "Student");
        var mappedPayments = _mapper.Map<List<PaymentDto>>(payments);
        return new SuccessDataResult<List<PaymentDto>>(mappedPayments, "Payments retrieved successfully.");
    }

    public IDataResult<PaymentDto> GetPaymentById(int paymentId)
    {
        var payment = _paymentRepository.Get(p => p.Id == paymentId, includeProperties: "Student");
        if (payment == null)
            return new ErrorDataResult<PaymentDto>("Payment not found.");

        var mappedPayment = _mapper.Map<PaymentDto>(payment);
        return new SuccessDataResult<PaymentDto>(mappedPayment, "Payment retrieved successfully.");
    }

    public IDataResult<List<PaymentDto>> GetOverduePayments()
    {
        var overduePayments = _paymentRepository.GetList(p => !p.IsPaid && p.DueDate < DateTime.Now, includeProperties: "Student");
        var mappedPayments = _mapper.Map<List<PaymentDto>>(overduePayments);
        return new SuccessDataResult<List<PaymentDto>>(mappedPayments, "Overdue payments retrieved successfully.");
    }

    public IResult CreatePayment(PaymentCreateDto paymentCreateDto)
    {
        var payment = _mapper.Map<Payment>(paymentCreateDto);
        _paymentRepository.Add(payment);
        return new SuccessResult("Payment created successfully.");
    }

    public IResult UpdatePayment(PaymentUpdateDto paymentUpdateDto)
    {
        var payment = _paymentRepository.Get(p => p.Id == paymentUpdateDto.Id);
        if (payment == null)
            return new ErrorResult("Payment not found.");

        _mapper.Map(paymentUpdateDto, payment);
        _paymentRepository.Update(payment);
        return new SuccessResult("Payment updated successfully.");
    }

    public IResult DeletePayment(int paymentId)
    {
        var payment = _paymentRepository.Get(p => p.Id == paymentId);
        if (payment == null)
            return new ErrorResult("Payment not found.");

        _paymentRepository.Delete(payment);
        return new SuccessResult("Payment deleted successfully.");
    }

    public IResult NotifyParentForOverduePayment(int paymentId)
    {
        var payment = _paymentRepository.Get(p => p.Id == paymentId, includeProperties: "Student.Parent");
        if (payment == null)
            return new ErrorResult("Payment not found.");

        if (payment.IsPaid)
            return new ErrorResult("Payment is not overdue.");

        // Simulate notification logic
        var parent = payment.Student.Parent;
        if (parent == null)
            return new ErrorResult("Parent information not available for the student.");

        // Notify the parent (e.g., via email or SMS)
        // For demonstration purposes, return success
        return new SuccessResult($"Notification sent to {parent.FullName} for overdue payment.");
    }
}