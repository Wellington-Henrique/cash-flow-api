﻿using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
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

            } catch (Exception ex)
            {
                var error = new ResponseErrorJson(ex.Message);
                return BadRequest(error);
            } catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "unknown error");
            }
        }
    }
}
