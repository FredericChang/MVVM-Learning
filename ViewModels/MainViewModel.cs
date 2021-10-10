using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp2.Helpers;
using WpfApp2.Models;
using WpfApp2.ViewModels;
using WpfApp2.Views;
using System.Windows.Navigation;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp2.ViewModels
{
    class MainViewModel : ObservableObject

    {
        private User user;

        //public string FirstName
        //{
        //    get { return user.FirstName; }
        //    set
        //    {
        //        if (user.FirstName != value)
        //        {
        //            user.FirstName = value;
        //            OnPropertyChange("FirstName");
        //            // If the first name has changed, the FullName property needs to be udpated as well.

        //        }
        //    }
        //}

        //public string LastName
        //{
        //    get { return user.LastName; }
        //    set
        //    {
        //        if (user.LastName != value)
        //        {
        //            user.LastName = value;
        //            OnPropertyChange("LastName");
        //            // If the first name has changed, the FullName property needs to be udpated as well.

        //        }
        //    }
        //}

        //// This property is an example of how model properties can be presented differently to the View.
        //// In this case, we transform the birth date to the user's age, which is read only.
        //public int Age
        //{
        //    get
        //    {
        //        DateTime today = DateTime.Today;
        //        int age = today.Year - user.BirthDate.Year;
        //        if (user.BirthDate > today.AddYears(-age)) age--;
        //        return age;
        //    }
        //}

        //// This property is just for display purposes and is a composition of existing data.
        //public string FullName
        //{
        //    get { return FirstName + " " + LastName; }
        //}

        private string _value1;
        public string Value1
        {
            get => _value1;
            set
            {
                SetProperty(ref _value1, value);
                CalculateCommand?.NotifyCanExecuteChanged();
            }
        }

        private string _value2;
        public string Value2
        {
            get => _value2;
            set
            {
                SetProperty(ref _value2, value);
                CalculateCommand?.NotifyCanExecuteChanged();
            }
        }

        private string _result;
        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        private bool _isFree;
        public bool IsFree
        {
            get => _isFree;
            set => SetProperty(ref _isFree, value);
        }

        private string _status;
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
        
        public ICommand ExecCommand { get;}
        public IRelayCommand CalculateCommand { get; }


        public MainViewModel()
        {
            user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Now.AddYears(-30)
            };

            IsFree = true;
            Status = "";
            ExecCommand = new AsyncRelayCommand(ExecAsync);



            CalculateCommand = new RelayCommand(
                execute: () =>
                {
                    try
                    {
                        Result = $"{Value1} + {Value2} = {int.Parse(Value1) + int.Parse(Value2)}";
                    }
                    catch
                    {
                        Result = "Error!";
                    }
                },
                canExecute: () =>
                {
                    return !string.IsNullOrEmpty(Value1) && !string.IsNullOrEmpty(Value2);
                });
        }

        private async Task ExecAsync()
        {
            IsFree = false;
            Status = "Processing...";

            await Task.Delay(2000);

            IsFree = true;
            Status = "Complete";
        }
    }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void OnPropertyChange(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }

        //}


        //public ICommand LoginCommand => new DelegateCommand(obj =>
        //{
        //    if(user.FirstName == "John" && user.LastName == "Doe")
        //    {
        //        Console.WriteLine("ddd");


        //        //TablePage editWindow = new TablePage();
        //        //editWindow.ShowDialog();
        //    }

        //});

    
}
