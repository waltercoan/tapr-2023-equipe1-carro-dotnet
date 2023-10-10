# tapr-2023-equipe1-carro-dotnet

## Autenticação no AZURE
[DOC](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-linux?pivots=apt)

```
az login -u walter.s@univille.br
az login --use-device-code
az ad signed-in-user show
```

## Extensões do VSCode
[C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
[Rest Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)

## Criação do projeto
```
dotnet new webapi
dotnet dev-certs https --trust
```

## Executar o projeto
```
dotnet watch --launch-profile https
```

## Dependências
```
dotnet add package Azure.Identity
dotnet add package Microsoft.EntityFrameworkCore.Cosmos
```

## CosmosDB
- [Introdução](https://learn.microsoft.com/en-us/azure/cosmos-db/introduction)
- [Databases, containers, and items](https://learn.microsoft.com/en-us/azure/cosmos-db/resource-model)


### Configuração RBAC de permissão
```
az cosmosdb sql role assignment create --account-name COSMOSDBACCOUNT --resource-group GRUPODERECURSO --role-assignment-id 00000000-0000-0000-0000-000000000002 --role-definition-name "Cosmos DB Built-in Data Contributor" --scope "/" --principal-id GUIDUSUARIOAD
```

