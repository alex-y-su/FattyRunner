using System.Globalization;

using Azon.Helpers.Extensions;

using Caliburn.Micro;

using FattyRunner.Engine;

namespace FattyRunner.VisualClient.ViewModel {
    public class TimeMeasureViewModel : Screen {
        private readonly TimeMeasure _time;

        public TimeMeasureViewModel(TimeMeasure time) {
            this._time = time;
        }

        public string Iterations {
            get {
                return this._time.IterationCount
                                 .ToString(CultureInfo.CurrentUICulture);
            }
        }

        public double Time {
            get {
                var s = (this._time.Time / (double)100);
                return s;
            }
        }
    }
}