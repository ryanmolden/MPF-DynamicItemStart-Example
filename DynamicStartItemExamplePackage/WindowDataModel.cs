using System.ComponentModel;
using System;

namespace ExampleInc.DynamicStartItemExamplePackage
{
    public class WindowDataModel : INotifyPropertyChanged
    {
        #region Private Fields

        private int numberOfItems = 5;

        #endregion

        #region Public Properties

        public int NumberOfItems
        {
            get 
            {
                return this.numberOfItems;
            }
            set
            {
                this.numberOfItems = value;
                NotifyPropertyChanged("NumberOfItems");
            }
        }

        #endregion

        #region Private Helper Methods

        private void NotifyPropertyChanged(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }

            if (propertyName.Length == 0)
            {
                throw new ArgumentException("NotifyPropertyChanged requires a non-empty string as the property name.");
            }

            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
