using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CommonTestUtilities.Requests;

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

            var request = RequestRegisterExpenseJsonBuilder.Build();
            //Act
            //Ação, executar o método que será testado
            var result = validator.Validate(request);

            //Assert
            //Resultado do teste, verdadeiro ou falso
            Assert.True(result.IsValid);
        }
    }
}
