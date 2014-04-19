using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FattyRunner.VisualClient.ViewModel {
    public class ListOfTestsViewModel: INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Text { get; set; }

        public void ClickDo() {
            this.Text = "123";
        }
    }
}
