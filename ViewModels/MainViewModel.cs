using System;

using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp2.Helpers;
using WpfApp2.Models;
using WpfApp2.ViewModels;
using WpfApp2.Views;
using System.Windows.Navigation;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;

namespace WpfApp2.ViewModels
{
    class MainViewModel : ObservableObject
    //class MainViewModel : ObservableRecipient, IRecipient<Models.Message>

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
        private int _sendCount;
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

        //MVVM01
        public ICommand ExecCommand { get;}

        //MVVM03
        public IRelayCommand CalculateCommand { get; }

        //MVVM03
        public IRelayCommand SendCommand { get; }
        public IRelayCommand SendCommand2 { get; }


        private string _sendMessage;
        public string SendMessage
        {
            get => _sendMessage;
            set
            {
                SetProperty(ref _sendMessage, value);
                SendCommand.NotifyCanExecuteChanged();
            }
        }

        private string _receiveMessage;
        public string ReceiveMessage
        {
            get => _receiveMessage;
            set => SetProperty(ref _receiveMessage, value);
        }

        private string _receiveMessage2;
        public string ReceiveMessage2
        {
            get => _receiveMessage2;
            set => SetProperty(ref _receiveMessage2, value);
        }
        //MVVM06

        private bool _isBusy;
        /// <summary>
        /// Processing flag
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private int _progressValue;
        public int ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }

        private string _progressText;
        public string ProgressText
        {
            get => _progressText;
            set => SetProperty(ref _progressText, value);
        }

        private CancellationTokenSource _cancelTokenSrc;
        private HeavyWorkModel _model;

        public IAsyncRelayCommand ExecuteCommand { get; }
        public IRelayCommand CancelCommand { get; }

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

            //WeakReferenceMessenger.Default.Register<string>(this, OnReceive);
            //WeakReferenceMessenger.Default.Register<Models.Message>(this, OnReceive2);

            SendCommand = new RelayCommand(
                execute: () =>
                {
                    Console.WriteLine("SendCommand_execute");   
                    WeakReferenceMessenger.Default.Send(SendMessage);
                },
                canExecute: () =>
                {
                    Console.WriteLine("SendCommand_canExecute");
                    var a = !string.IsNullOrEmpty(SendMessage);
                    Console.WriteLine(a);
                    return !string.IsNullOrEmpty(SendMessage);

                    // if the return value is false, this button could not be executed.
                });
            //this.IsActive = true;

            _sendCount = 0;
            SendCommand2 = new RelayCommand(() =>
            {

                _sendCount++;
                WeakReferenceMessenger.Default.Send(new Models.Message { Num = _sendCount, Str = DateTime.Now.ToString()});
                // you could send the message directly.
            });
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
            IsBusy = false;

            ExecuteCommand = new AsyncRelayCommand(OnExecuteAsync, CanExecute);

            CancelCommand = new RelayCommand(
                () =>
                {
                    _cancelTokenSrc?.Cancel();
                    UpdateCommandStatus();
                },
                () => IsBusy);
        }

        private async Task ExecAsync()
        {
            IsFree = false;
            Status = "Processing...";

            await Task.Delay(2000);

            IsFree = true;
            Status = "Complete";
        }

        private void OnReceive(object recipient, string message)
        {
            ReceiveMessage = $"Recevied:{message}";
            Console.WriteLine("SendCommand_ReceiveMessage");
            //<TextBlock Margin="10" Text="{Binding ReceiveMessage}" />
        }
        private void OnReceive2(object recipient, Models.Message message)
        {
            ReceiveMessage2 = $"Num:{message.Num}, Str:{message.Str}";
            Console.WriteLine("SendCommand_ReceiveMessage");
            //<TextBlock Margin="10" Text="{Binding ReceiveMessage}" />
        }

        //this method is created by IRecipient<Models.Message>
        public void Receive(Models.Message message)
        {
            ReceiveMessage2 = $"Num={message.Num}, Str={message.Str}";
        }

        private async Task OnExecuteAsync()
        {
            IsBusy = true;
            UpdateCommandStatus();

            _model = new HeavyWorkModel();

            var p = new Progress<ProgressInfo>();
            p.ProgressChanged += (sender, e) =>
            {
                ProgressValue = e.ProgressValue;
                ProgressText = e.ProgressText;
            };

            _cancelTokenSrc = new CancellationTokenSource();
            await _model.HeavyWorkAsync(p, _cancelTokenSrc.Token);

            IsBusy = false;
            UpdateCommandStatus();
        }

        private bool CanExecute()
        {
            return !IsBusy;
        }

        private void UpdateCommandStatus()
        {
            ExecuteCommand.NotifyCanExecuteChanged();
            CancelCommand.NotifyCanExecuteChanged();
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
