using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationLib
{
    public class LoanCalculator
    {
        public static Dictionary<DateTime, Payment> Calculate(double level1, double amortizationPerYearLevel1, double level2, 
            double amortizationPerYearLevel2, double interest, double loanSize, double ownCapital, double interestDeduction)
        {
            if (!CheckInputVariables2(level1, amortizationPerYearLevel1, level2, amortizationPerYearLevel2, interest, loanSize, ownCapital))
            {
                return new Dictionary<DateTime, Payment>();
            }

            interest = interest * (1 - interestDeduction);

            // setup parameters
            double totalCostOfAsset = loanSize + ownCapital;
            
            // variables used in loop 
            double currentLoanSize = loanSize;
            double currentLoanToValue = loanSize / (loanSize + ownCapital);

            var currentDate = GetLastDayOfCurrentMonth(DateTime.Now);

            var paymentsPerMonth = new Dictionary<DateTime, Payment>();
            Payment payment;
            while (currentLoanSize > 0 && currentLoanToValue > level2)
            {
                double amortizementPayment;
                double interestPayment;   
                if (currentLoanToValue > level1)
                {
                    amortizementPayment = (currentLoanSize + ownCapital) * amortizationPerYearLevel1 / 12;
                    interestPayment = currentLoanSize * interest / 12;
                    payment = new Payment(amortizementPayment, interestPayment, currentDate, Level.Level1);
                }
                else if (currentLoanToValue < level1 && currentLoanToValue > level2)
                {
                    amortizementPayment = (currentLoanSize + ownCapital) * amortizationPerYearLevel2 / 12;
                    interestPayment = currentLoanSize * interest / 12;
                    payment = new Payment(amortizementPayment, interestPayment, currentDate, Level.Level2);
                }
                else
                {
                    payment = new Payment(0, currentLoanSize * interest / 12, new DateTime(2100, 1, 1), Level.NoAmortization);
                    paymentsPerMonth.Add(payment.Date, payment);
                    break;
                }

                // store results
                paymentsPerMonth.Add(currentDate, payment);

                // update variables for next iteration
                currentLoanSize = currentLoanSize - amortizementPayment;
                currentDate = currentDate.AddMonths(1);
                currentLoanToValue = currentLoanSize / totalCostOfAsset;
            }

            return paymentsPerMonth;
        }

        private static DateTime GetLastDayOfCurrentMonth(DateTime date)
        {
            return new DateTime(date.Year, date.AddMonths(1).Month, 1).AddDays(-1); // last day of current month
        }
        
        public static Dictionary<DateTime, Payment> CalculateAndReturnFirstAndSecondLevels(double level1, double amortizationPerYearLevel1, double level2,
            double amortizationPerYearLevel2, double interest, double loanSize, double ownCapital, double interestDeduction)
        {
            var payments = Calculate(level1, amortizationPerYearLevel1, level2, amortizationPerYearLevel2, interest, loanSize, ownCapital, interestDeduction);
            var toReturn = new Dictionary<DateTime, Payment>();

            var level1Payments = payments.Values.OrderBy(x => x.Date).FirstOrDefault(x => x.Level == Level.Level1);
            var level2Payments = payments.Values.OrderBy(x => x.Date).FirstOrDefault(x => x.Level == Level.Level2);
            if (level1Payments != null)
            {
                toReturn.Add(level1Payments.Date, level1Payments);
            }
            else
            {
                var currentDate = new DateTime(1900, 1, 1);
                toReturn.Add(currentDate, new Payment(0, 0, currentDate, Level.Level1));
            }
            if (level2Payments != null)
            {
                toReturn.Add(level2Payments.Date, level2Payments);
            }
            else
            {
                var currentDate = GetLastDayOfCurrentMonth(DateTime.Now);
                toReturn.Add(currentDate, new Payment(0, 0, currentDate, Level.Level2));
            }

            return toReturn;
        }

        private static void CheckInputVariables(double level1, double amortizationPerYearLevel1, double level2, double amortizationPerYearLevel2,
            double interest, double loanSize, double ownCapital)
        {
            if (level1 < level2)
            {
                throw new Exception("level1 should be larger than level2");
            }

            if (level1 < 0 || level1 > 1)
            {
                throw new Exception("level1 interest isn't between 0 and 1");
            }

            if (level2 < 0 || level2 > 1)
            {
                throw new Exception("level2 interest isn't between 0 and 1");
            }

            if (amortizationPerYearLevel1 < 0 || amortizationPerYearLevel1 > 1)
            {
                throw new Exception("amortizationPerYearLevel1 interest isn't between 0 and 1");
            }

            if (amortizationPerYearLevel2 < 0 || amortizationPerYearLevel2 > 1)
            {
                throw new Exception("amortizationPerYearLevel2 interest isn't between 0 and 1");
            }

            if (interest < 0 || interest > 1)
            {
                throw new Exception("level2 interest isn't between 0 and 1");
            }

            if (loanSize < 0)
            {
                throw new Exception("loanSize interest is smaller than 0");
            }

            if (ownCapital < 0)
            {
                throw new Exception("ownCapital interest is smaller than 0");
            }
        }

        private static bool CheckInputVariables2(double level1, double amortizationPerYearLevel1, double level2, double amortizationPerYearLevel2,
            double interest, double loanSize, double ownCapital)
        {
            var okInputs = true;
            if (level1 < level2)
            {
                okInputs = false;
            }

            if (level1 < 0 || level1 > 1)
            {
                okInputs = false;
            }

            if (level2 < 0 || level2 > 1)
            {
                okInputs = false;
            }

            if (amortizationPerYearLevel1 < 0 || amortizationPerYearLevel1 > 1)
            {
                okInputs = false;
            }

            if (amortizationPerYearLevel2 < 0 || amortizationPerYearLevel2 > 1)
            {
                okInputs = false;
            }

            if (interest < 0 || interest > 1)
            {
                okInputs = false;
            }

            if (loanSize < 0)
            {
                okInputs = false;
            }

            if (ownCapital < 0)
            {
                okInputs = false;
            }

            return okInputs;
        }
    }

        public enum Level
    {
        Level1,
        Level2,
        NoAmortization
    }

}
