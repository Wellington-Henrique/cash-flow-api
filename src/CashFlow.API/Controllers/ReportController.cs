using CashFlow.Application.UseCases.Expenses.GetAll;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CashFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [HttpGet("excel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetExcel([FromServices] IGetAllExpenseUseCase useCase)
        {
            byte[] file = new byte[1];

            if(file.Length > 0)
                return File(file, MediaTypeNames.Application.Octet, "repor.xlsx");

            return NoContent();
        }
    }
}
