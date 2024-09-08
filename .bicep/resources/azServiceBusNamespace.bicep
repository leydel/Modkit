param name string
param location string

resource azServiceBusNamespace 'Microsoft.ServiceBus/namespaces@2022-01-01-preview' = {
  name: name
  location: location
  sku: {
    name: 'Standard'
  }
  properties: {}
}

output name string = azServiceBusNamespace.name
