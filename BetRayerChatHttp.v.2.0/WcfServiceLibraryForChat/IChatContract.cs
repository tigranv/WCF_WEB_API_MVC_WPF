using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibraryForChat
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IChatContract
    {
        [OperationContract]
        void AddSend(Message message);
        [OperationContract]
        Task<List<Message>> GetAllMessages();
        [OperationContract]
        Task<bool> LoginAsync(User user);
        [OperationContract]
        Task<bool> RegisterAsync(User user);
        [OperationContract]
        void SendMail(User user);
        [OperationContract]
        List<User> GetAllUsersAsync();
        [OperationContract]
        
        void RemoveFromList(User users);

        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "WcfServiceLibraryForChat.ContractType".
    [DataContract]
    public class Message
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        
    }

    [DataContract]
    public class User
    {
        bool boolValue = true;
        string stringValue = "Hello ";

       
    }
}
