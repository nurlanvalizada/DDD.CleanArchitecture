using AppDomain.ValueObjects;
using Xunit;

namespace UnitTests
{
    public class AddressTests
    {
        [Fact]
        public void Addresses_With_Same_Properties_Should_Be_Equal()
        {
            // Arrange
            var address1 = new Address("123 Main St", "City", "State", "Country", "12345");
            var address2 = new Address("123 Main St", "City", "State", "Country", "12345");

            //Assert
            Assert.Equal(address1, address2);
        }

        [Fact]
        public void Addresses_With_Different_Properties_Should_Not_Be_Equal()
        {
            // Arrange
            var address1 = new Address("123 Main St", "City", "State", "Country", "12345");
            var address2 = new Address("456 Elm St", "City", "State", "Country", "54321");

            // Assert
            Assert.NotEqual(address1, address2);
        }
    }
}