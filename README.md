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