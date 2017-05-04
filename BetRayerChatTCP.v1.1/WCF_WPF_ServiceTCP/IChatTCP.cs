using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCF_WPF_ServiceTCP
{
    [ServiceContract]
    public interface IMessageCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnMessageAdded(string message, DateTime timestamp);
    }

    //This is a little different than the client 
    // in that we need to state the SessionMode as required or it will default to "notAllowed"
    [ServiceContract(CallbackContract = typeof(IMessageCallback), SessionMode = SessionMode.Required)]
    public interface IMessage
    {
        [OperationContract(IsOneWay = true)]
        void AddMessage(string message);
        [OperationContract]
        bool Subscribe();
        [OperationContract]
        bool Unsubscribe();

 
    }
}
