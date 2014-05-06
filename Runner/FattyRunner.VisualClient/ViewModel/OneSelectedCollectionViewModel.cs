using System.Collections.Generic;

using Caliburn.Micro;

namespace FattyRunner.VisualClient.ViewModel {
    public class OneSelectedCollectionViewModel<T> : Conductor<T>.Collection.OneActive where T: class {
        public OneSelectedCollectionViewModel(IEnumerable<T> items) {
            this.Items.AddRange(items);
        }

        public void Reset(IEnumerable<T> items) {
            this.Items.AddRange(items);
            this.Refresh();
        }
    }
}