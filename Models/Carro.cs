namespace tapr_2023_equipe1_carro_dotnet.Models;

public class Carro
{
    public Guid id {get;set;}
    public string placa {get;set;}
    public Modelo modelo {get;set;}
    public string ClienteId { get; set; }
}