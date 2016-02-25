using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationLib
{
    public class Payment
    {
        public Payment(double amortization, double interest, DateTime date, Level level)
        {
            Amortization = amortization;
            Interest = interest;
            Date = date;
            Level = level;
        }

        public Level Level { get; }

        public DateTime Date { get; }

        public double Total { get { return Amortization + Interest; } }

        public double Amortization { get; }

        public double Interest { get; }
    }
}
