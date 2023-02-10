using NUnit.Framework;
using System;

namespace MortgageCalculatorTest
{
    [TestFixture]
    public class Tests
    {
        [TestCase("John Doe", 100000, 4, 15, 739.69)]
        [TestCase("Jane,Doe", 3200, 5, 4, 73.69)]
        public void TestValidInput(string customer, double loan, double interest, int years, double expectedResult)
        {
            var customerTest = new MortgageCalculator.Customer
            {
                Name = customer,
                TotalLoan = loan,
                InterestRate = interest,
                TotalYears = years,
            };

            var result = customerTest.CalculateMonthlyPayment();
            Assert.AreEqual(expectedResult, result, 0.01);
        }
    }
}