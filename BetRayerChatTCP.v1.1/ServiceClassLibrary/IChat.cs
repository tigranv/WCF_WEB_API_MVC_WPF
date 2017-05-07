using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClassLibrary
{
    [ServiceContract]
    public interface IMessageCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnMessageAdded(string message, DateTime timestamp);
        [OperationContract(IsOneWay = true)]
        void OnPrivateMessageAdded(string message, string sender, DateTime timestamp);

        [OperationContract(IsOneWay = true)]
        void SendNames(ObservableCollection<string> names);
    }

    [ServiceContract(CallbackContract = typeof(IMessageCallback), SessionMode = SessionMode.Required)]
    public interface IMessage
    {
        [OperationContract(IsOneWay = true)]
        void AddMessage(string message, string sender, bool convMode);
        [OperationContract(IsOneWay = true)]
        void AddPrivateMessage(string message, string sender, string reciver);
        [OperationContract]
        bool Subscribe(string name);
        [OperationContract]
        bool Unsubscribe(string name);
        [OperationContract(IsOneWay = true)]
        void SendOnlineUsers();
    }
}
