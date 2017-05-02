using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceAssembly;
using System.ServiceModel;

namespace ConsoleHosting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SERVER";

            ServiceHost host = new ServiceHost(typeof(ChatService));

            //---------------------------------------------------------------------------------------
            // Привязка и ее свойства.
            NetTcpBinding binding = new NetTcpBinding();

            // Время ожидания закрытия соединения.
            binding.CloseTimeout = TimeSpan.MaxValue;

            // Способ сравнения имен узлов при разборе URI.
            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;

            // Число каналов, готовых к обслуживанию запросов. 
            // Последующие запросы на соединение становятся в очередь.
            binding.ListenBacklog = 10;

            // Размер пула буферов для транспортного протокола.
            binding.MaxBufferPoolSize = 524888;

            // Число входящих или исходящих соединений.
            // Входящие и исходящие соединения считаются порознь.
            binding.MaxConnections = 10;

            // Размер одного входящего сообщения.
            binding.MaxReceivedMessageSize = 65536;

            // Имя привязки.
            binding.Name = "MyBinding";

            // Время ожидания завершения операции открытия соединения.
            binding.OpenTimeout = TimeSpan.FromMinutes(2f);

            // Разрешение/Запрет обобщения портов слушателями служб. По умолчанию - false.
            binding.PortSharingEnabled = true;

            // Время ожидания завершения операции приема.
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
