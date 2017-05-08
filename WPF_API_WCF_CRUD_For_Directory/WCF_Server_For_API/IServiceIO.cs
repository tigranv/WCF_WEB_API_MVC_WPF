using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WCF_Server_For_API
{
    [ServiceContract]
    public interface IServiceIO
    {
        [OperationContract]
        IEnumerable<string> GetAllFiles(string dirName);


        [OperationContract]
        string Get(string dirName, string fileName);


        [OperationContract]
        void Put(string dirName, string fileName, string value);

        [OperationContract]
        void Post(string dirName, string fileName, string value);

        [OperationContract]
        void Delete(string dirName, string fileName);
        
    }

    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
