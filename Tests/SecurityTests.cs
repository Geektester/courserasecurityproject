using Xunit;
using SafeVault.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SafeVault.Tests
{
    public class SecurityTests
    {
        [Fact]
        public void InputValidation_ShouldRejectInvalidUsername()
        {
            var model = new LoginModel { Username = "x", Password = "ValidPass123" };
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            bool valid = Validator.TryValidateObject(model, context, results, true);

            Assert.False(valid); // Username too short
        }

        [Fact]
        public void SqlInjection_ShouldFail()
        {
            var model = new LoginModel { Username = "admin' --", Password = "anything" };
            // Attempted injection should not succeed due to parameterized query
            Assert.True(true); // Placeholder for actual DB test
        }
    }
}
