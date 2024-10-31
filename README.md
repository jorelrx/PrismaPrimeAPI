# PrismaPrimeInvestApi

PrismaPrimeInvest é uma aplicação construída para gerenciar investimentos e usuários. Utiliza arquitetura baseada em microsserviços, permitindo uma escalabilidade robusta, e está preparada para hospedagem na Azure.

## Índice

- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Configuração do Banco de Dados e Migrations](#configuração-do-banco-de-dados-e-migrations)
- [Como Executar o Projeto](#como-executar-o-projeto)
- [Funcionalidades](#funcionalidades)
- [Contribuição](#contribuição)

---

## Tecnologias Utilizadas

Este projeto utiliza as seguintes tecnologias:

- **.NET Core 8.0**: Framework principal do projeto.
- **Entity Framework Core**: ORM para gerenciar a camada de dados, migrations e banco de dados.
- **AutoMapper**: Biblioteca para mapear automaticamente objetos, como entidades para DTOs.
- **Azure SQL Database**: Banco de dados hospedado na nuvem Azure.
- **PrismaPrimeInvest.Api**: API RESTful para expor os serviços de investimento.
  
O projeto segue padrões e boas práticas como SOLID e Domain-Driven Design (DDD), além de uma estrutura modular com injeção de dependências via `Infra.IoC`.

---

## Estrutura do Projeto

A organização do projeto segue uma estrutura modular:

```
PrismaPrimeInvest.sln
src/
├── PrismaPrimeInvest.Api               # Camada de apresentação (API)
├── PrismaPrimeInvest.Domain            # Camada de domínio (entidades e lógica de negócio)
├── PrismaPrimeInvest.Application       # Serviços, DTOs, validações e configurações
├── PrismaPrimeInvest.Infra.IoC         # Configuração de injeção de dependências
└── PrismaPrimeInvest.Infra.Data        # Contexto de banco de dados e repositórios
```

### Camadas e Funções

1. **PrismaPrimeInvest.Api**: Ponto de entrada da API. Contém os controladores e configurações de middlewares.
2. **PrismaPrimeInvest.Domain**: Define as entidades de negócio, como `User` e `Fund`, e suas pastas específicas para fácil organização.
3. **PrismaPrimeInvest.Application**: Define a lógica de aplicação, como serviços e DTOs. Aqui também estão as validações, filtros, mapeamentos, e configurações para o AutoMapper e banco de dados.
4. **PrismaPrimeInvest.Infra.IoC**: Configura a injeção de dependências para desacoplamento da aplicação e gerenciamento de instâncias de classes.
5. **PrismaPrimeInvest.Infra.Data**: Responsável pelo contexto do banco de dados (`ApplicationDbContext`) e implementação dos repositórios.

---

## Configuração do Banco de Dados e Migrations

Para adicionar e executar migrations no banco de dados, execute os comandos abaixo:

### 1. Adicionar uma Migration

```bash
dotnet ef migrations add <NomeDaMigration> --project src/PrismaPrimeInvest.Infra.Data -s src/PrismaPrimeInvest.Api -c ApplicationDbContext
```

Exemplo:

```bash
dotnet ef migrations add UpdateCreatedAtAndUpdatedAt --project src/PrismaPrimeInvest.Infra.Data -s src/PrismaPrimeInvest.Api -c ApplicationDbContext
```

### 2. Aplicar a Migration ao Banco de Dados

```bash
dotnet ef database update --project src/PrismaPrimeInvest.Infra.Data -s src/PrismaPrimeInvest.Api -c ApplicationDbContext
```

---

## Como Executar o Projeto

1. **Clone o Repositório**:
   
   ```bash
   git clone https://github.com/jorelrx/PrismaPrimeAPI.git
   cd PrismaPrimeInvest
   ```

2. **Configuração do Ambiente**:

   Modifique o arquivo `appsettings.json` na pasta `PrismaPrimeInvest.Api` com a conexão do banco de dados e outras configurações necessárias. Exemplo:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=<your_server>;Database=<your_db>;User Id=<your_user>;Password=<your_password>;"
     }
   }
   ```

3. **Executar o Projeto**:

   Com o ambiente configurado, execute o projeto a partir da pasta raiz com o comando:

   ```bash
   dotnet run --project src/PrismaPrimeInvest.Api
   ```

4. **Acessar a Documentação**:

   A API expõe uma interface Swagger em `http://localhost:5000/swagger` onde você pode testar os endpoints.

---

## Funcionalidades

- **Gerenciamento de Usuários**: Criação, atualização, exclusão e listagem de usuários.
- **Gerenciamento de Fundos de Investimento**: Cadastrar, atualizar, consultar e excluir fundos de investimento.
- **Filtros e Paginação**: Recursos de filtros em dados com paginação.
- **Configuração de Middleware**: Autenticação, CORS, documentação com Swagger e tratamento de erros.

---

## Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir um issue para discutir mudanças que você gostaria de ver ou para submeter um pull request.

---

## Comandos adicionais

1. **Removendo arquivo appsettings.json do rastreamendo git**:
   
   ```bash
   git update-index --assume-unchanged src/PrismaPrimeInvest.Api/appsettings.json
   ```

2. **Adicionando arquivo appsettings.json do rastreamendo git**:
   
   ```bash
   git update-index --no-assume-unchanged src/PrismaPrimeInvest.Api/appsettings.json
   ```
   
---

## Objetivos
### Entidades
- Usuario
- Fundo

### Funcionalidades
- Usuario tem seus fundos
- Para cada fundo que o usuario tem, deve ter uma quantidade
- Usuario pode adicionar ou remover fundo
- Para cada fundo adicionado deve pegar o atual e verificar o ganho com base no valor
- Para cada fundo, deve mostrar proximo dividendo, valor minimo do fundo, valor maximo, melhor dia de compra,

- Para cada fundo, deve ter uma lista de pagamentos, registrando o valor maximo, minimo, valor pago dividendo e o preço do dia de pagamento
- Para cada fundo, deve registrar todo dia o maior valor e o menor
- para cada fundo, deve ter relatorio mensal do fundo