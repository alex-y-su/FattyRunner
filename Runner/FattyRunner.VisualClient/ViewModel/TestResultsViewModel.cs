using System;
using System.Collections.Generic;
using System.Linq;

using Caliburn.Micro;

using FattyRunner.Engine;

namespace FattyRunner.VisualClient.ViewModel {
    public class TestResultsViewModel : Screen {
        private readonly Func<TimeMeasure, TimeMeasureViewModel> _measureFactory;
        private readonly TestResult _result;

        public TestResultsViewModel(TestResult result, Func<TimeMeasure, TimeMeasureViewModel> measureFactory) {
            this._result = result;
            this._measureFactory = measureFactory;
        }

        public IEnumerable<TimeMeasureViewModel> Timings {
            get {
                return this._result.Timings.OrderBy(x => x.IterationCount)
                    .Select(this._measureFactory);
            }
        }

        public double OpsLast {
            get {
                var lst = this._result.Timings.Last();
                return GetOps(lst);
            }
        }

        public double OpsAvg {
            get {
                var tms = this._result.Timings.Where(x => x.Time > 0)
                    .Select(GetOps)
                    .ToArray();

                if (tms.Length == 0)
                    return double.PositiveInfinity;

                return tms.Average();
            }
        }

        private static double GetOps(TimeMeasure tm) {
            return ((tm.IterationCount / (double)tm.Time) * 100D);
        }
    }
}