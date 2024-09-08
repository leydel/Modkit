param name string
param serviceBusNamespaceName string
param duplicateDetection bool
param session bool
param deadLettering bool
param partitioning bool
param express bool

resource azServiceBusNamespace 'Microsoft.ServiceBus/namespaces@2022-01-01-preview' existing = {
  name: serviceBusNamespaceName
}

resource serviceBusQueue 'Microsoft.ServiceBus/namespaces/queues@2022-01-01-preview' = {
  parent: azServiceBusNamespace
  name: name
  properties: {
    requiresDuplicateDetection: duplicateDetection
    requiresSession: session
    deadLetteringOnMessageExpiration: deadLettering
    enablePartitioning: partitioning
    enableExpress: express
  }
}

output name string = serviceBusQueue.name
