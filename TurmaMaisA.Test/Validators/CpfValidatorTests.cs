using TurmaMaisA.Utils.Validators;

namespace TurmaMaisA.Test.Validators
{
    public class CpfValidatorTests
    {

        [Theory(DisplayName = "Validate Should Return True For Valid CPFs")]
        [InlineData("037.870.100-25")] // Exemplo de CPF válido
        [InlineData("03787010025")]    // Exemplo de CPF válido sem formatação
        public void Validate_WithValidCpf_ShouldReturnTrue(string validCpf)
        {
            // Act
            var result = CpfValidator.Validate(validCpf);

            // Assert
            Assert.True(result);
        }

        [Theory(DisplayName = "Validate Should Return False For Invalid CPFs")]
        [InlineData("000.000.000-00")]       // Dígitos repetidos
        [InlineData("111.111.111-11")]       // Dígitos repetidos
        [InlineData("123.456.789-00")]       // Dígitos verificadores incorretos
        [InlineData("123.456.789")]          // Incompleto
        [InlineData("abc.def.ghi-jk")]       // Com letras
        [InlineData(null)]                   // Nulo
        [InlineData("")]                     // Vazio
        [InlineData("   ")]                  // Espaços em branco
        public void Validate_WithInvalidCpf_ShouldReturnFalse(string invalidCpf)
        {
            // Act
            var result = CpfValidator.Validate(invalidCpf);

            // Assert
            Assert.False(result);
        }
    }
}
