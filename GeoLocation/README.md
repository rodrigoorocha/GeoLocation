# GeoLocation API

Este projeto fornece uma API para buscar recursos dentro de um raio especificado a partir de um ponto.

## Pré-requisitos

1. Instale o [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).
2. Instale o [Docker](https://www.docker.com/).

## Executando a Aplicação Localmente

1. Clone o repositório:
   
2. Restaure as dependências:
   
3. Execute a aplicação:
   
4. Acesse a API:
   - Swagger UI: [https://localhost:5001/swagger](https://localhost:5001/swagger)
   - Endpoints disponíveis:
     - `GET /api/Feasibility/radius-search?latitude=<LAT>&longitude=<LON>&radius=<RAIO>`

## Executando a Aplicação com Docker

1. Construa a imagem Docker:
   
2. Execute o container Docker:
   
3. Acesse a API:
   - Swagger UI: [http://localhost:8080/swagger](http://localhost:8080/swagger)
   - Endpoints disponíveis:
     - `GET /api/Feasibility/radius-search?latitude=<LAT>&longitude=<LON>&radius=<RAIO>`

## Exemplos de Requisições

### Requisição bem-sucedida

### Resposta

### Nenhum recurso encontrado

## Solução de Problemas

- **Porta em uso**: Certifique-se de que as portas 5000/5001 (local) ou 8080 (Docker) não estão sendo usadas por outro processo.
- **Erro ao rodar o Docker**: Verifique se o daemon do Docker está em execução.
- **Erro ao rodar o .NET**: Certifique-se de que o SDK .NET 9 está instalado corretamente.
- **Configurações**: Confira o arquivo `appsettings.json` para ajustar as configurações.