using MySql.Data.MySqlClient;

Banco.CriarBanco();
Banco.CriarTabelaPessoa();
while (true)
{
    Console.WriteLine();
    Console.WriteLine("========== MENU ==========");
    Console.WriteLine("1 - Cadastrar pessoa");
    Console.WriteLine("2 - Listar pessoas");
    Console.WriteLine("3 - Atualizar pessoa");
    Console.WriteLine("4 - Deletar pessoa");
    Console.WriteLine("0 - Sair");
    Console.WriteLine("==========================");
    Console.Write("Escolha uma opção: ");

    string? opcao = Console.ReadLine();

    try
    {
        switch (opcao)
        {
            case "1":
                Cadastrar();
                break;
            case "2":
                Listar();
                break;
            case "3":
                Atualizar();
                break;
            case "4":
                Deletar();
                break;
            case "0":
                Console.WriteLine("Saindo...");
                return;
            default:
                Console.WriteLine("Opção inválida!");
                break;
        }
    }
    catch (MySqlException ex)
    {
        Console.WriteLine($"Erro no banco de dados: {ex.Message}");
    }
}

static void Cadastrar()
{
    Imc pessoa = Imc.CadastrarPessoa();
    pessoa.MostrarResultado();
    pessoa.SalvarNoBanco();
}

static void Listar()
{
    List<Imc> pessoas = Imc.ListarPessoas();

    if (pessoas.Count == 0)
    {
        Console.WriteLine("Nenhuma pessoa cadastrada.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine($"{"ID",-5}{"Nome",-20}{"Peso",8}{"Altura",9}{"IMC",8}  Classificação");
    Console.WriteLine(new string('-', 70));

    foreach (Imc p in pessoas)
    {
        Console.WriteLine(
            $"{p.Id,-5}{p.Nome,-20}{p.Peso,8:F2}{p.Altura,9:F2}{p.CalcularIMC(),8:F2}  {p.ClassificacaoIMC()}"
        );
    }
}

static void Atualizar()
{
    Listar();

    int id = LerId("Digite o ID da pessoa que deseja atualizar: ");
    Imc? pessoa = Imc.BuscarPorId(id);

    if (pessoa == null)
    {
        Console.WriteLine("Pessoa não encontrada!");
        return;
    }

    pessoa.EditarDados();

    if (pessoa.AtualizarNoBanco())
    {
        Console.WriteLine("Pessoa atualizada com sucesso!");
        pessoa.MostrarResultado();
    }
    else
    {
        Console.WriteLine("Nenhum registro foi atualizado.");
    }
}

static void Deletar()
{
    Listar();

    int id = LerId("Digite o ID da pessoa que deseja deletar: ");
    Imc? pessoa = Imc.BuscarPorId(id);

    if (pessoa == null)
    {
        Console.WriteLine("Pessoa não encontrada!");
        return;
    }

    Console.Write($"Tem certeza que deseja deletar {pessoa.Nome} (ID {pessoa.Id})? (s/n): ");
    string? confirmacao = Console.ReadLine();

    if (confirmacao?.Trim().ToLower() != "s")
    {
        Console.WriteLine("Operação cancelada.");
        return;
    }

    if (Imc.DeletarDoBanco(id))
        Console.WriteLine("Pessoa deletada com sucesso!");
    else
        Console.WriteLine("Nenhum registro foi deletado.");
}

static int LerId(string mensagem)
{
    while (true)
    {
        Console.Write(mensagem);

        if (int.TryParse(Console.ReadLine(), out int id) && id > 0)
            return id;

        Console.WriteLine("ID inválido! Digite um número inteiro maior que zero.");
    }
}
