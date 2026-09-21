# Sistema de Cadastro e Cálculo de IMC

Aplicação de console em **C#** que calcula o Índice de Massa Corporal (IMC) de pessoas e guarda os dados em um banco **MySQL**. Com ela é possível **cadastrar, listar, atualizar e deletar** pessoas (CRUD completo).

## Integrantes

- Emanoel 
- Samuel
- Zion

## Funcionalidades

- **Cadastrar** uma pessoa (nome, peso e altura) com validação dos dados digitados
- **Listar** todas as pessoas cadastradas, com IMC e classificação calculados na hora
- **Atualizar** os dados de uma pessoa pelo ID (Enter mantém o valor atual)
- **Deletar** uma pessoa pelo ID, com confirmação antes de apagar

### Classificação do IMC

| IMC            | Classificação     |
|----------------|-------------------|
| Abaixo de 18,5 | Abaixo do peso    |
| 18,5 a 24,9    | Peso normal       |
| 25,0 a 29,9    | Sobrepeso         |
| 30,0 a 34,9    | Obesidade grau I  |
| 35,0 a 39,9    | Obesidade grau II |
| 40,0 ou mais   | Obesidade grau III|

## Banco de dados utilizado

**MySQL** (banco `IMC`, tabela `Pessoa`).

## Biblioteca/driver utilizado

**MySql.Data** (MySQL Connector/NET), pacote NuGet oficial da Oracle para conectar aplicações .NET ao MySQL.

## Pré-requisitos

- [.NET SDK 6.0 ou superior](https://dotnet.microsoft.com/download)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) instalado e em execução

## Como instalar as dependências

Na pasta do projeto (onde fica o arquivo `.csproj`), execute:

```bash
dotnet add package MySql.Data
```

Se o pacote já estiver listado no `.csproj`, basta restaurar as dependências:

```bash
dotnet restore
```

## Como configurar o banco

1. Certifique-se de que o MySQL está rodando.
2. Execute o script `tabela_pessoa.sql` no MySQL (pelo MySQL Workbench, pelo terminal ou outro cliente). Ele cria o banco e a tabela:

```sql
CREATE DATABASE IF NOT EXISTS IMC;
USE IMC;

CREATE TABLE IF NOT EXISTS Pessoa (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    peso DECIMAL(5,2) NOT NULL,
    altura DECIMAL(3,2) NOT NULL
);
```

> Se a tabela `Pessoa` já existia sem a coluna `id`, rode:
> `ALTER TABLE Pessoa ADD COLUMN id INT AUTO_INCREMENT PRIMARY KEY FIRST;`

3. No arquivo `Imc.cs`, ajuste a connection string com os dados do seu MySQL:

```csharp
public static string conexaoBanco =
    "server=localhost;database=IMC;user=root;password=SUA_SENHA;";
```

## Como executar o projeto

Na pasta do projeto, execute:

```bash
dotnet run
```

O menu será exibido no terminal:

```
========== MENU ==========
1 - Cadastrar pessoa
2 - Listar pessoas
3 - Atualizar pessoa
4 - Deletar pessoa
0 - Sair
==========================
```

## Estrutura do projeto

| Arquivo             | Função                                                                 |
|---------------------|------------------------------------------------------------------------|
| `Program.cs`        | Menu interativo e chamadas das operações                               |
| `Imc.cs`            | Classe com validações, cálculo do IMC e acesso ao banco (CRUD)         |
| `tabela_pessoa.sql` | Script para criar o banco e a tabela                                   |

## Como funciona a conexão com o banco

A conexão é feita pelo driver **MySql.Data** e segue sempre os mesmos passos em cada operação:

1. **Connection string:** o texto em `conexaoBanco` informa o servidor, o banco, o usuário e a senha.
2. **`MySqlConnection`:** é criada com a connection string e aberta com `conn.Open()`.
3. **`MySqlCommand`:** recebe o comando SQL (`INSERT`, `SELECT`, `UPDATE` ou `DELETE`) e a conexão que será usada.
4. **Parâmetros:** os valores digitados pelo usuário entram por parâmetros (`@nome`, `@peso`, `@altura`, `@id`) em vez de serem concatenados no SQL. Isso evita **SQL Injection**.
5. **Execução:**
   - `ExecuteNonQuery()` para comandos que alteram dados (`INSERT`, `UPDATE`, `DELETE`); retorna quantas linhas foram afetadas.
   - `ExecuteReader()` para consultas (`SELECT`); o `MySqlDataReader` percorre o resultado linha por linha, e cada linha vira um objeto `Imc`.
6. **Fechamento:** a conexão, o comando e o leitor são declarados com `using`, então são fechados automaticamente ao fim de cada método, mesmo que ocorra algum erro.

O IMC e a classificação **não são salvos no banco**: apenas nome, peso e altura ficam gravados, e o IMC é recalculado sempre que necessário.
