targetScope = 'subscription'

@description('The ID of the user to deploy resources for')
param userId string

@description('The Azure region to deploy to')
param region string

var resourceToken = take(toLower(uniqueString(subscription().id, 'modkit', userId, region)), 7)

resource azResourceGroup 'Microsoft.Resources/resourceGroups@2023-07-01' = {
  name: 'rg-modkit-local-${userId}'
  location: region
}

module azServiceBusNamespace 'resources/azServiceBusNamespace.bicep' = {
  name: 'azServiceBusNamespace'
  scope: azResourceGroup
  params: {
    name: 'sbns-modkit-local-${userId}-${resourceToken}'
    location: region
  }
}

module azServiceBusQueue 'resources/azServiceBusQueue.bicep' = {
  name: 'azServiceBusQueue'
  scope: azResourceGroup
  params: {
    name: 'sbq-modkit-local-${userId}-${resourceToken}'
    serviceBusNamespaceName: azServiceBusNamespace.outputs.name
    duplicateDetection: true
    session: true
    deadLettering: true
    partitioning: false
    express: true
  }
}

output resourceGroupName string = azResourceGroup.name
output serviceBusNamespaceName string = azServiceBusNamespace.outputs.name
output serviceBusQueueName string = azServiceBusQueue.outputs.name
