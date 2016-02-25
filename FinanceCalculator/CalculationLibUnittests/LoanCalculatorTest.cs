using CalculationLib;
using NUnit.Framework;
using System;
using System.Linq;

namespace CalculationLibUnittests
{
    [TestFixture]
    public class LoanCalculatorTest
    {
        [Test]
        public void TestLevel1()
        {
            // ARRANGE
            double level1 = 0.7;
            double amortizatoinPerYearLevel1 = 0.02;
            double level2 = 0.5;
            double amortizationPerYearLevel2 = 0.01;
            double interest = 0.01;
            double loanSize = 1000000;
            double ownCapital = 10000;

            // DO
            var result = LoanCalculator.Calculate(level1, amortizatoinPerYearLevel1, level2, amortizationPerYearLevel2,
                interest, loanSize, ownCapital, 0);

            var interestPayment = interest* loanSize / 12;
            var amortization = amortizatoinPerYearLevel1 * (loanSize + ownCapital) / 12;
            var total = interestPayment + amortization;
            // ASSERT
            var firstLevel1Result = result.OrderBy(x => x.Key).First().Value;

            Assert.AreEqual(interestPayment, firstLevel1Result.Interest, 0.000001);
            Assert.AreEqual(amortization, firstLevel1Result.Amortization, 0.000001);
            Assert.AreEqual(total, firstLevel1Result.Total, 0.000001);
        }

        [Test]
        public void TestLevel1_using_CalculateAndReturnFirstAndSecondLevels()
        {
            // ARRANGE
            double level1 = 0.7;
            double amortizatoinPerYearLevel1 = 0.02;
            double level2 = 0.5;
            double amortizationPerYearLevel2 = 0.01;
            double interest = 0.01;
            double loanSize = 1000000;
            double ownCapital = 10000;

            // DO
            var result = LoanCalculator.CalculateAndReturnFirstAndSecondLevels(level1, amortizatoinPerYearLevel1, level2, amortizationPerYearLevel2,
                interest, loanSize, ownCapital, 0);

            var interestPayment = interest * loanSize / 12;
            var amortization = amortizatoinPerYearLevel1 * (loanSize + ownCapital) / 12;
            var total = interestPayment + amortization;
            // ASSERT
            var firstLevel1Result = result.OrderBy(x => x.Key).First().Value;

            Assert.AreEqual(interestPayment, firstLevel1Result.Interest, 0.000001);
            Assert.AreEqual(amortization, firstLevel1Result.Amortization, 0.000001);
            Assert.AreEqual(total, firstLevel1Result.Total, 0.000001);
        }

        [Test]
        public void TestLevel2()
        {
            // ARRANGE
            double level1 = 0.7;
            double amortizatoinPerYearLevel1 = 0.02;
            double level2 = 0.5;
            double amortizationPerYearLevel2 = 0.01;
            double interest = 0.01;
            double loanSize = 1000000;
            double ownCapital = 0.6 * loanSize;

            // DO
            var result = LoanCalculator.Calculate(level1, amortizatoinPerYearLevel1, level2, amortizationPerYearLevel2,
                interest, loanSize, ownCapital, 0);

            var interestPayment = interest * loanSize / 12;
            var amortization = amortizationPerYearLevel2 * (loanSize + ownCapital) / 12;
            var total = interestPayment + amortization;
            // ASSERT
            var firstLevel2Result = result.OrderBy(x => x.Key).Where(x => x.Value.Level == Level.Level2).First().Value;

            Assert.AreEqual(interestPayment, firstLevel2Result.Interest, 0.000001);
            Assert.AreEqual(amortization, firstLevel2Result.Amortization, 0.000001);
            Assert.AreEqual(total, firstLevel2Result.Total, 0.000001);
        }

        [Test]
        public void TestLevel2_using_CalculateAndReturnFirstAndSecondLevels()
        {
            // ARRANGE
            double level1 = 0.7;
            double amortizatoinPerYearLevel1 = 0.02;
            double level2 = 0.5;
            double amortizationPerYearLevel2 = 0.01;
            double interest = 0.01;
            double loanSize = 1000000;
            double ownCapital = 0.6 * loanSize;

            // DO
            var result = LoanCalculator.CalculateAndReturnFirstAndSecondLevels(level1, amortizatoinPerYearLevel1, level2, amortizationPerYearLevel2,
                interest, loanSize, ownCapital, 0);

            var interestPayment = interest * loanSize / 12;
            var amortization = amortizationPerYearLevel2 * (loanSize + ownCapital) / 12;
            var total = interestPayment + amortization;
            // ASSERT
            var firstLevel2Result = result.OrderBy(x => x.Key).Where(x => x.Value.Level == Level.Level2).First().Value;

            Assert.AreEqual(interestPayment, firstLevel2Result.Interest, 0.000001);
            Assert.AreEqual(amortization, firstLevel2Result.Amortization, 0.000001);
            Assert.AreEqual(total, firstLevel2Result.Total, 0.000001);
        }
    }
}
