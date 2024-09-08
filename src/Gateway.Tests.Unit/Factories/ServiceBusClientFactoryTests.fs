namespace Modkit.Gateway.Factories

open Azure.Messaging.ServiceBus
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type ServiceBusClientFactoryTests () =
    [<TestMethod>]
    member _.CreateClient_CreatesServiceBusClient () =
        // Arrange
        let serviceBusClientFactory: IServiceBusClientFactory = ServiceBusClientFactory()
        let connectionString = "SERVICE_BUS_CONNECTION_STRING"

        // Act
        let res = serviceBusClientFactory.CreateClient connectionString

        // Assert
        Assert.IsInstanceOfType<ServiceBusClient> res
        Assert.AreEqual(ServiceBusTransportType.AmqpWebSockets, res.TransportType)
