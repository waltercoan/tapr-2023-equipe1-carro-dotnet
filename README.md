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

### Falha de conexão com o CosmosDB devido bloqueio na rede da UNIVILLE
- Alunos que utilizarem seus notebooks pessoais conectados a rede UNIVILLE devem alterar o arquivo RepositoryDBContext.cs para modificar o método de conexão da aplicação com o CosmosDB
- [CosmosDB Gateway Connection](https://learn.microsoft.com/en-us/azure/cosmos-db/dedicated-gateway)
```
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseCosmos(
        accountEndpoint: _configuration["CosmosDBURL"],
        tokenCredential: new DefaultAzureCredential(),
        databaseName: _configuration["CosmosDBDBName"],
        options => { options.ConnectionMode(ConnectionMode.Gateway); });
}
```

## CRUD API REST
### Verbo GET
- Objetivo: Retornar uma lista de objetos ou um objeto específico a partir da chave

#### ICarroService.cs
- Criar os métodos na interface do serviço
```
public interface ICarroService
{
    Task<List<Carro>> GetAllAsync();
    Task<Carro> GetByIdAsync(string id);
}
```
#### CarroService.cs
- Implementar a lógica de consulta na classe concreta do serviço
```
public async Task<List<Carro>> GetAllAsync()
{
    var listaCarros = await _dbContext.Carros.ToListAsync();
    return listaCarros;
}

public async Task<Carro> GetByIdAsync(string id)
{
    var carro = await _dbContext.Carros.Where(c => c.id.Equals(new Guid(id))).FirstOrDefaultAsync();
    return carro;
    
}
```
#### teste.rest
- Implementação do teste do verbo GET
```
### Buscar todos os carros
GET http://localhost:5202/api/v1/carro

### Buscar carro pelo ID
GET http://localhost:5202/api/v1/carro/3f840c63-130c-436b-8543-97ab14caf16f
```
### Verbo POST
- Objetivo: Inserir uma nova instância da entidade no banco de dados
#### ICarroService.cs
- Criar o método saveNew na interface de serviço
```
public interface ICarroService
{
    Task<List<Carro>> GetAllAsync();
    Task<Carro> GetByIdAsync(string id);
    Task<Carro> saveNewAsync(Carro carro);
}
```
#### CarroService.cs
- Implementar a lógica para salvar a nova entidade no banco, o campo ID é alterado para null para garantir que o método será utilizado apenas para incluir novos registros
```
public async Task<Carro> saveNewAsync(Carro carro)
{
    carro.id = Guid.Empty;
    await _dbContext.Carros.AddAsync(carro);
    await _dbContext.SaveChangesAsync();

    return carro;
}
```
#### CarroController.cs
- Implememntar no controlador o metodo para inserir o novo carro no sistema.
```
[HttpPost()]
public async Task<IResult> InsertNew(Carro carro){      
    if(carro == null){
        return Results.BadRequest();
    }
    await _service.saveNewAsync(carro);

    return Results.Ok(carro);
}
```
#### teste.rest
- Implementação do teste do verbo POST
```
### Inserir um novo Carro
POST http://localhost:5202/api/v1/carro
Content-Type: application/json

{
  "placa": "MAS1334"
}
```
### Verbo PUT
- Objetivo: Alterar os dados de uma determinada instância da entidade

#### ICarroService.cs
- Criar o método update na interface de serviço
```
public interface ICarroService
{
    Task<List<Carro>> GetAllAsync();
    Task<Carro> GetByIdAsync(string id);
    Task<Carro> saveNewAsync(Carro carro);
    Task<Carro> updateAsync(String id, Carro carro);
}
```
#### CarroService.cs
- Implementar a lógica para realizar o update da entidade no banco
```
public async Task<Carro> updateAsync(string id, Carro carro)
{
    var carroAntigo = await _dbContext.Carros.Where(c => c.id.Equals(new Guid(id))).FirstOrDefaultAsync();        
    if (carroAntigo != null){
        //Atualizar cada atributo do objeto antigo 
        carroAntigo.modelo = carro.modelo;
        await _dbContext.SaveChangesAsync();
    }
    return carroAntigo;
}
```
#### CarroController.cs
- Implememntar no controlador o metodo para realizar o update do registro
```
[HttpPut("{id}")]
public async Task<IResult> Update(string id, Carro carro){      
    if(carro == null || id.Equals(String.Empty))
    {
        return Results.BadRequest();
    }

    carro = await _service.updateAsync(id, carro);

    if(carro == null)
    { 
        return Results.NotFound();
    }

    return Results.Ok(carro);
}
```
#### teste.rest
```
### Atualizar o  Carro
PUT http://localhost:5202/api/v1/carro/be122a6f-885d-4b2d-a3e1-63aa0485c8bb
Content-Type: application/json

{
  "placa": "MAS1334",
  "modelo": "Fiat UNO"
}
```
### Verbo DELETE
- Objetivo: Remover uma instância da entidade

#### ICarroService.cs
- Criar o método delete na interface de serviço
```
public interface ICarroService
{
    Task<List<Carro>> GetAllAsync();
    Task<Carro> GetByIdAsync(string id);
    Task<Carro> saveNewAsync(Carro carro);
    Task<Carro> updateAsync(String id, Carro carro);
    Task<Carro> DeleteAsync(String id);
}
```
#### CarroService.cs
- Implementar a lógica para realizar a exclusão da entidade no banco
```
public async Task<Carro> DeleteAsync(string id)
{
    var carroAntigo = await _dbContext.Carros.Where(c => c.id.Equals(new Guid(id))).FirstOrDefaultAsync();
    if (carroAntigo != null){
            _dbContext.Remove(carroAntigo);
        await _dbContext.SaveChangesAsync();
    }
    return carroAntigo;
}
```
#### CarroController.cs
- Implememntar no controlador o metodo para realizar a exclusão do registro
```
[HttpDelete("{id}")]
public async Task<IResult> Delete(string id){      
    if(id.Equals(String.Empty))
    {
        return Results.BadRequest();
    }

    var carro = await _service.DeleteAsync(id);

    if(carro == null)
    { 
        return Results.NotFound();
    }

    return Results.Ok(carro);
}
```

#### teste.rest
- Implementação do teste do verbo DELETE
```
### Remover o Carro
DELETE  http://localhost:5202/api/v1/carro/be122a6f-885d-4b2d-a3e1-63aa0485c8bb
Content-Type: application/json
```