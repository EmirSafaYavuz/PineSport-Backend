using Business.Abstract;
using Entities.Dtos;
using Entities.Dtos.Register;
using Entities.Dtos.Update;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // GET /api/payments
        [HttpGet]
        public IActionResult GetPayments()
        {
            var result = _paymentService.GetPayments();
            return GetResponseOnlyResultData(result);
        }

        // POST /api/payments
        [HttpPost]
        public IActionResult CreatePayment([FromBody] PaymentCreateDto paymentCreateDto)
        {
            var result = _paymentService.CreatePayment(paymentCreateDto);
            return result.Success 
                ? Created(result.Message, "Payment created successfully") 
                : BadRequest(result.Message, result.Message);
        }

        // GET /api/payments/{id}
        [HttpGet("{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var result = _paymentService.GetPaymentById(id);
            return GetResponseOnlyResultData(result);
        }

        // PUT /api/payments/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePayment(int id, [FromBody] PaymentUpdateDto paymentUpdateDto)
        {
            paymentUpdateDto.Id = id; // Ensure payment ID is passed to the service
            var result = _paymentService.UpdatePayment(paymentUpdateDto);
            return GetResponseOnlyResultMessage(result);
        }

        // DELETE /api/payments/{id}
        [HttpDelete("{id}")]
        public IActionResult DeletePayment(int id)
        {
            var result = _paymentService.DeletePayment(id);
            return GetResponseOnlyResult(result);
        }

        // GET /api/payments/overdue
        [HttpGet("overdue")]
        public IActionResult GetOverduePayments()
        {
            var result = _paymentService.GetOverduePayments();
            return GetResponseOnlyResultData(result);
        }

        // POST /api/payments/{id}/notify
        [HttpPost("{id}/notify")]
        public IActionResult NotifyParentForOverduePayment(int id)
        {
            var result = _paymentService.NotifyParentForOverduePayment(id);
            return GetResponseOnlyResult(result);
        }
    }
}