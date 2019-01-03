using Microsoft.Azure.EventHubs;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SenndJsonToAzure
{
    class Program
    {
        private static EventHubClient eventHubClient;
        private const string EventHubConnectionString = "EntityPath=mytesting.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=4b4wUQny9H83mZ0NIh1VChWqIiEmIWt/yKFcsr3+oEM=";
        private const string EventHubName = "mytesting";

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }
        static async Task MainAsync(string[] args)
        {
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
            // Endpoint=sb://namespace_DNS_Name;EntityPath=
            {
                EntityPath = "myeventhub",
                Endpoint = new Uri("sb://testingevent.servicebus.windows.net/"),
                SasKeyName = "iothubroutes_mytesting",
                SasKey = "oEQ4T5iB/tsFwbXrYc4nAxUJujXaHvqEcGBLSTV4WRo="
                //Endpoint=sb://testingevent.servicebus.windows.net/;SharedAccessKeyName=iothubroutes_mytesting;SharedAccessKey=oEQ4T5iB/tsFwbXrYc4nAxUJujXaHvqEcGBLSTV4WRo=;EntityPath=myeventhub
                // Endpoint = new Uri("Endpoint=sb://mytesting.azure-devices.net;EntityPath=mytesting;SharedAccessKeyName=iothubowner;SharedAccessKey=4b4wUQny9H83mZ0NIh1VChWqIiEmIWt/yKFcsr3+oEM=")
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            await SendMessagesToEventHub(10);

            await eventHubClient.CloseAsync();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }

        private static async Task SendMessagesToEventHub(int numMessagesToSend)
        {
            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    var message = $"Message {i}";
                    Console.WriteLine($"Sending message: {message}");
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(10);
            }

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }
}
