using System;
using System.Collections.Generic;
using System.Text;
using FW.WPF.ViewModels.Base;
namespace FW.WPF.ViewModels
{
    public class MessageViewModel : ViewModel
    {
        private string _Message;
        public string Message
        { 
            get => _Message; 
            set
            {
                Set(ref _Message, value); 
                OnPropertyChanged("HasMessage");
            }
        }

        public bool HasMessage => !string.IsNullOrEmpty(Message);
    }
}
