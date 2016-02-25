using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculationLib;
using OxyPlot;
using OxyPlot.Axes;

namespace FinanceCalculator.ViewModel
{
    public class PaymentsViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private OxyPlot.PlotModel plotModel;
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { plotModel = value; OnPropertyChanged("PlotModel"); }
        }

        public PaymentsViewModel()
        {
            //default values:
            Level1 = 0.7;
            AmortizationPerYearLevel1 = 0.02;
            Level2 = 0.5;
            AmortizationPerYearLevel2 = 0.01;
            Interest = 0.02;
            LoanSize = 1000000;
            OwnCapital = 150000;
            InterestDeduction = 0.3;
            PlotModel = new PlotModel();
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, StringFormat= "yyyy-MM-dd" });
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            
        }

        public IList<DataPoint> Points { get; private set; }

        // inputs
        private double level1, amortizationPerYearLevel1, level2, amortizationPerYearLevel2, interest, loanSize, ownCapital, interestDeduction;

        public double Level1
        {
            get { return level1; }
            set
            {
                level1 = value;
                OnPropertyChanged("Level1");
                ClearOutputAndRecalculate();
            }
        }

        public double InterestDeduction
        {
            get { return interestDeduction; }
            set
            {
                interestDeduction = value;
                OnPropertyChanged("InterestDeduction");
                ClearOutputAndRecalculate();
            }
        }

        public double AmortizationPerYearLevel1
        {
            get { return amortizationPerYearLevel1; }
            set
            {
                amortizationPerYearLevel1 = value;
                OnPropertyChanged("AmortizationPerYearLevel1");
                ClearOutputAndRecalculate();
            }
        }

        public double Level2
        {
            get { return level2; }
            set
            {
                level2 = value;
                OnPropertyChanged("Level2");
                ClearOutputAndRecalculate();
            }
        }

        public double AmortizationPerYearLevel2
        {
            get { return amortizationPerYearLevel2; }
            set
            {
                amortizationPerYearLevel2 = value;
                OnPropertyChanged("AmortizationPerYearLevel2");
                ClearOutputAndRecalculate();
            }
        }

        public double Interest
        {
            get { return interest; }
            set
            {
                interest = value;
                OnPropertyChanged("Interest");
                ClearOutputAndRecalculate();
            }
        }

        public double LoanSize
        {
            get { return loanSize; }
            set
            {
                loanSize = value;
                OnPropertyChanged("LoanSize");
                ClearOutputAndRecalculate();
            }
        }

        public double OwnCapital
        {
            get { return ownCapital; }
            set
            {
                ownCapital = value;
                OnPropertyChanged("OwnCapital");
                ClearOutputAndRecalculate();
            }
        }

        // outputs
        private Payment currentFirstLevel, currentSecondLevel;
        private double total1, interestCost1, amortCost1, total2, interestCost2, amortCost2;
        private DateTime period1, period2;

        private void ClearOutputAndRecalculate()
        {
            currentFirstLevel = null;
            currentSecondLevel = null;
            CalculateValues();
        }

        private void CalculateValues()
        {
            var dateToPayments = LoanCalculator.CalculateAndReturnFirstAndSecondLevels(Level1, AmortizationPerYearLevel1, Level2,
                AmortizationPerYearLevel2, Interest, LoanSize, OwnCapital, InterestDeduction);

            var allPayments = LoanCalculator.Calculate(Level1, AmortizationPerYearLevel1, Level2,
                AmortizationPerYearLevel2, Interest, LoanSize, OwnCapital, InterestDeduction);

            Points = allPayments.Where(x => x.Value.Level == Level.Level1 || x.Value.Level == Level.Level2).Select(x => new DataPoint(DateTimeAxis.ToDouble(x.Key), x.Value.Total)).ToList();

            if (dateToPayments.Count >= 2)
            {
                currentFirstLevel = dateToPayments.Values.OrderBy(x => x.Date).First();
                currentSecondLevel = dateToPayments.Values.OrderBy(x => x.Date).Last();
                period1 = currentFirstLevel.Date;
                total1 = currentFirstLevel.Total;
                interestCost1 = currentFirstLevel.Interest;
                amortCost1 = currentFirstLevel.Amortization;

                period2 = currentSecondLevel.Date;
                total2 = currentSecondLevel.Total;
                interestCost2 = currentSecondLevel.Interest;
                amortCost2 = currentSecondLevel.Amortization;
            }
            else if (dateToPayments.Count == 1)
            {
                currentFirstLevel = dateToPayments.Values.OrderBy(x => x.Date).First();
                period1 = currentFirstLevel.Date;
                total1 = currentFirstLevel.Total;
                interestCost1 = currentFirstLevel.Interest;
                amortCost1 = currentFirstLevel.Amortization;

                period2 = DateTime.MinValue;
                total2 = 0;
                interestCost2 = 0;
                amortCost2 = 0;
            }
            else
            {
                period1 = DateTime.MinValue;
                total1 = 0;
                interestCost1 = 0;
                amortCost1 = 0;

                period2 = DateTime.MinValue;
                total2 = 0;
                interestCost2 = 0;
                amortCost2 = 0;
            }

            OnPropertyChanged("Period1");
            OnPropertyChanged("Total1");
            OnPropertyChanged("InterestCost1");
            OnPropertyChanged("AmortCost1");
            OnPropertyChanged("Period2");
            OnPropertyChanged("Total2");
            OnPropertyChanged("InterestCost2");
            OnPropertyChanged("AmortCost2");
            OnPropertyChanged("Points");
        }

        // Period 1 results

        public DateTime Period1
        {
            get
            {
                return period1;
            }
        }

        public double Total1
        {
            get
            {
                return total1;
            }
        }

        public double InterestCost1
        {
            get
            {
                return interestCost1;
            }
        }

        public double AmortCost1
        {
            get
            {
                return amortCost1;
            }
        }

        // Period 2 results

        public DateTime Period2
        {
            get
            {
                return period2;
            }
        }

        public double Total2
        {
            get
            {
                return total2;
            }
        }

        public double InterestCost2
        {
            get
            {
                return interestCost2;
            }
        }

        public double AmortCost2
        {
            get
            {
                return amortCost2;
            }
        }

        #region IDataErrorInfo members
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                string validationResult = null;
                switch (columnName)
                {
                    case "Level1":
                        validationResult = ValidateLevel1();
                        break;
                    case "Level2":
                        validationResult = ValidateLevel2();
                        break;
                    case "InterestDeduction":
                        validationResult = ValidatePositive(InterestDeduction);
                        break;
                    case "Interest":
                        validationResult = ValidatePositive(Interest);
                        break;
                    case "AmortizationPerYearLevel1":
                        validationResult = ValidatePositive(AmortizationPerYearLevel1);
                        break;
                    case "AmortizationPerYearLevel2":
                        validationResult = ValidatePositive(AmortizationPerYearLevel2);
                        break;
                    case "OwnCapital":
                        validationResult = ValidatePositive(OwnCapital);
                        break;
                    case "LoanSize":
                        validationResult = ValidatePositive(LoanSize);
                        break;
                    
                    default:
                        throw new ApplicationException("Unknown Property being validated on Product.");
                }
                return validationResult;
            }
        }

        private string ValidateLevel1()
        {
            if (Level1 <= Level2)
            {
                return "Level 1 must be larger than level 2";
            }

            if (Level1 > 1)
            {
                return "Level 1 is too large";
            }

            if (Level1 < 0)
            {
                return "Level 1 is too small";
            }

            return String.Empty;
        }

        private string ValidateLevel2()
        {
            if (Level1 <= Level2)
            {
                return "Level 1 must be larger than level 2";
            }

            if (Level2 > 1)
            {
                return "Level 1 is too large";
            }

            if (Level2 < 0)
            {
                return "Level 1 is too small";
            }

            return String.Empty;
        }

        private string ValidatePositive(double value)
        {
            if (value < 0)
            {
                return "value must can't be negative";
            }

            return String.Empty;
        }

       

        #endregion

        #region INotifyPropertyChanged interface implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
