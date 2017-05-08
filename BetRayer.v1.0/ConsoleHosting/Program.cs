using System;
using ServiceAssembly;
using System.ServiceModel;

namespace ConsoleHosting
{
    class Program
    {
        static void Main(string[] args)
        {
            // all bindings and their properties
            ServiceHost host = new ServiceHost(typeof(ChatService));

            //---------------------------------------------------------------------------------------
            NetTcpBinding binding = new NetTcpBinding();

            binding.CloseTimeout = TimeSpan.MaxValue;

            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;

            binding.ListenBacklog = 10;

            binding.MaxBufferPoolSize = 524888;

            binding.MaxConnections = 10;

            binding.MaxReceivedMessageSize = 65536;

            binding.Name = "MyBinding";

            binding.OpenTimeout = TimeSpan.FromMinutes(2f);

            binding.PortSharingEnabled = true;

            binding.ReceiveTimeout = TimeSpan.MaxValue;


            //---------------------------------------------------------------------------------------

            host.AddServiceEndpoint(typeof(IChat), binding, "net.tcp://localhost:7998");
            host.Open();
            Console.WriteLine("Server is opened");

            Console.ReadKey();

            host.Close();
        }
    }
}
