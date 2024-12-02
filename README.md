# NailApp

## Requisitos

### Ambiente de Desenvolvimento
- **IDE**: Visual Studio 2022 ou superior.
- **Banco de Dados**: SQL Server 2019 ou superior.
- **Framework**: .NET 7.0 (ou conforme especificado no projeto).
- **Ferramentas**: Entity Framework Core (via NuGet).

## Configuração do Projeto

1. **Clone este repositório**
   ```bash
   git clone https://github.com/nathaliab03/NailApp.git
2. **Abra o projeto**
- Abra a solução NailApp.sln no Visual Studio.
3. **Configure o Banco de Dados**
- No arquivo **appsettings.json**, atualize a string de conexão para apontar para o seu servidor SQL.
4. **Restaure os Pacotes NuGet**
- No Visual Studio, use o Gerenciador de Pacotes NuGet ou execute:
  ```bash
  dotnet restore
5. **Crie o Banco de Dados**
- Aplique as migrations para criar o banco de dados:
  ```bash
  dotnet ef database update
6. **Execute a Aplicação**
- Inicie a aplicação com:
   ```bash
  dotnet run
## Funcionalidades
Gerenciamento de agendas e estilistas para salões de unha.