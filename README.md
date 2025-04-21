# GeoLocation API

Este projeto fornece uma API para buscar recursos dentro de um raio especificado a partir de um ponto.

## Pr�-requisitos

1. Instale o [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).
2. Instale o [Docker](https://www.docker.com/).

## Executando a Aplica��o Localmente

1. Clone o reposit�rio:
   
2. Restaure as depend�ncias:
   
3. Execute a aplica��o:
   
4. Acesse a API:
   - Swagger UI: [https://localhost:5001/swagger](https://localhost:5001/swagger)
   - Endpoints dispon�veis:
     - `GET /api/Feasibility/radius-search?latitude=<LAT>&longitude=<LON>&radius=<RAIO>`

## Executando a Aplica��o com Docker

1. Construa a imagem Docker:
   
2. Execute o container Docker:
   
3. Acesse a API:
   - Swagger UI: [http://localhost:8080/swagger](http://localhost:8080/swagger)
   - Endpoints dispon�veis:
     - `GET /api/Feasibility/radius-search?latitude=<LAT>&longitude=<LON>&radius=<RAIO>`

## Exemplos de Requisi��es

### Requisi��o bem-sucedida

### Resposta

### Nenhum recurso encontrado

## Solu��o de Problemas

- **Porta em uso**: Certifique-se de que as portas 5000/5001 (local) ou 8080 (Docker) n�o est�o sendo usadas por outro processo.
- **Erro ao rodar o Docker**: Verifique se o daemon do Docker est� em execu��o.
- **Erro ao rodar o .NET**: Certifique-se de que o SDK .NET 9 est� instalado corretamente.
- **Configura��es**: Confira o arquivo `appsettings.json` para ajustar as configura��es.
