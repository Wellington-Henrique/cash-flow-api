using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Register([FromBody] RequestRegisterExpenseJson request) 
        {
            try
            {
                var response = new RegisterExpenseUserCase().Execute(request);

                return Created(string.Empty, response);

            } catch (ErrorOnValidationException ex)
            {
                var error = new ResponseErrorJson(ex.Errors);
                return BadRequest(error);
            } catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "unknown error");
            }
        }
    }
}
