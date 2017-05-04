﻿using System;
using System.ServiceModel;

namespace WCF_WPF_ServiceTCP
{
    [ServiceContract]
    public interface IMessageCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnMessageAdded(string message, DateTime timestamp);
    }

    [ServiceContract(CallbackContract = typeof(IMessageCallback), SessionMode = SessionMode.Required)]
    public interface IMessage
    {
        [OperationContract(IsOneWay = true)]
        void AddMessage(string message, string sender);
        [OperationContract]
        bool Subscribe();
        [OperationContract]
        bool Unsubscribe(); 
    }
}