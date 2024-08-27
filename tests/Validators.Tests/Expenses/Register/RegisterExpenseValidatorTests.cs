using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;

namespace Validators.Tests.Expenses.Register
{
    public class RegisterExpenseValidatorTests
    {
        [Fact]
        public void Success()
        {
            //Arrange
            //Criar instâncias necessárias para a acriação do teste
            var validator = new RegisterExpenseValidator();
            
            var request = new RequestRegisterExpenseJson
            {
                Amount = 100,
                Date = DateTime.Now.AddDays(-1),
                Description = "Description",
                Title = "Title",
                PaymentType = CashFlow.Communication.Enums.PaymentType.CreditCard
            };

            //Act
            //Ação, executar o método que será testado
            var result = validator.Validate(request);

            //Assert
            //Resultado do teste, verdadeiro ou falso
            Assert.True(result.IsValid);
        }
    }
}
