using MySql.Data.MySqlClient;

public class Imc
{
    public static string conexaoBanco =
        "server=localhost;database=IMC;user=root;password=Senac2026;";

    private string? nome;
    private decimal peso;
    private decimal altura;

    // Id gerado pelo banco (AUTO_INCREMENT)
    public int Id { get; private set; }

    public string? Nome
    {
        get
        {
            return nome;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value) ||
                value.Any(c => !char.IsLetter(c)))
            {
                Console.WriteLine("O nome deve conter apenas letras, sem espaços ou caracteres especiais.");
                return;
            }

            nome = value;
        }
    }

    public decimal Peso
    {
        get
        {
            return peso;
        }
        set
        {
            if (value <= 0)
            {
                Console.WriteLine("O peso deve ser maior que zero.");
                return;
            }

            peso = value;
        }
    }

    public decimal Altura
    {
        get
        {
            return altura;
        }
        set
        {
            if (value <= 0)
            {
                Console.WriteLine("A altura deve ser maior que zero.");
                return;
            }

            altura = value;
        }
    }

    // ===================== ENTRADA DE DADOS =====================

    // Se "atual" for informado, o usuário pode apertar Enter para manter o valor.
    private static string LerNome(string? atual = null)
    {
        while (true)
        {
            if (atual == null)
                Console.Write("Digite seu nome: ");
            else
                Console.Write($"Novo nome [{atual}] (Enter para manter): ");

            string? entrada = Console.ReadLine();

            if (atual != null && string.IsNullOrWhiteSpace(entrada))
                return atual;

            if (!string.IsNullOrWhiteSpace(entrada) &&
                entrada.All(char.IsLetter))
                return entrada;

            Console.WriteLine("Nome inválido! Digite apenas letras, sem espaços.");
        }
    }

    private static decimal LerDecimal(string rotulo, string unidade, decimal? atual = null)
    {
        while (true)
        {
            if (atual == null)
                Console.Write($"Digite seu {rotulo} ({unidade}): ");
            else
                Console.Write($"Novo {rotulo} ({unidade}) [{atual:F2}] (Enter para manter): ");

            string? entrada = Console.ReadLine();

            if (atual != null && string.IsNullOrWhiteSpace(entrada))
                return atual.Value;

            if (decimal.TryParse(entrada, out decimal valor) && valor > 0)
                return valor;

            Console.WriteLine($"{rotulo} inválido! Digite um número maior que zero.");
        }
    }

    public static Imc CadastrarPessoa()
    {
        Imc pessoa = new Imc();

        pessoa.Nome = LerNome();
        pessoa.Peso = LerDecimal("peso", "kg");
        pessoa.Altura = LerDecimal("altura", "m");

        return pessoa;
    }

    // Pede novos valores para uma pessoa já existente
    public void EditarDados()
    {
        Nome = LerNome(Nome);
        Peso = LerDecimal("peso", "kg", Peso);
        Altura = LerDecimal("altura", "m", Altura);
    }

    // ===================== CÁLCULOS =====================

    public decimal CalcularIMC()
    {
        return Peso / (Altura * Altura);
    }

    public string ClassificacaoIMC()
    {
        decimal imc = CalcularIMC();

        if (imc < 18.5m)
            return "Abaixo do peso";

        if (imc < 25m)
            return "Peso normal";

        if (imc < 30m)
            return "Sobrepeso";

        if (imc < 35m)
            return "Obesidade grau I";

        if (imc < 40m)
            return "Obesidade grau II";

        return "Obesidade grau III";
    }

    // ===================== BANCO DE DADOS =====================

    // CREATE
    public void SalvarNoBanco()
    {
        string sql = @"
            INSERT INTO Pessoa (nome, peso, altura)
            VALUES (@nome, @peso, @altura);
        ";

        using MySqlConnection conn = new MySqlConnection(conexaoBanco);
        conn.Open();

        using MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@nome", Nome);
        cmd.Parameters.AddWithValue("@peso", Peso);
        cmd.Parameters.AddWithValue("@altura", Altura);

        cmd.ExecuteNonQuery();

        Id = (int)cmd.LastInsertedId;

        Console.WriteLine($"Pessoa cadastrada no banco de dados! (ID: {Id})");
    }

    // READ (todos)
    public static List<Imc> ListarPessoas()
    {
        List<Imc> pessoas = new List<Imc>();

        string sql = "SELECT id, nome, peso, altura FROM Pessoa ORDER BY id;";

        using MySqlConnection conn = new MySqlConnection(conexaoBanco);
        conn.Open();

        using MySqlCommand cmd = new MySqlCommand(sql, conn);
        using MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
            pessoas.Add(CriarDoReader(reader));

        return pessoas;
    }

    // READ (um)
    public static Imc? BuscarPorId(int id)
    {
        string sql = "SELECT id, nome, peso, altura FROM Pessoa WHERE id = @id;";

        using MySqlConnection conn = new MySqlConnection(conexaoBanco);
        conn.Open();

        using MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        using MySqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
            return CriarDoReader(reader);

        return null;
    }

    // UPDATE
    public bool AtualizarNoBanco()
    {
        string sql = @"
            UPDATE Pessoa
            SET nome = @nome, peso = @peso, altura = @altura
            WHERE id = @id;
        ";

        using MySqlConnection conn = new MySqlConnection(conexaoBanco);
        conn.Open();

        using MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@nome", Nome);
        cmd.Parameters.AddWithValue("@peso", Peso);
        cmd.Parameters.AddWithValue("@altura", Altura);
        cmd.Parameters.AddWithValue("@id", Id);

        return cmd.ExecuteNonQuery() > 0;
    }

    // DELETE
    public static bool DeletarDoBanco(int id)
    {
        string sql = "DELETE FROM Pessoa WHERE id = @id;";

        using MySqlConnection conn = new MySqlConnection(conexaoBanco);
        conn.Open();

        using MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        return cmd.ExecuteNonQuery() > 0;
    }

    // Monta o objeto direto nos campos, sem passar pela validação dos setters
    // (os dados que já estão no banco são considerados válidos)
    private static Imc CriarDoReader(MySqlDataReader reader)
    {
        return new Imc
        {
            Id = reader.GetInt32("id"),
            nome = reader.GetString("nome"),
            peso = reader.GetDecimal("peso"),
            altura = reader.GetDecimal("altura")
        };
    }

    // ===================== SAÍDA =====================

    public void MostrarResultado()
    {
        decimal imc = CalcularIMC();

        Console.WriteLine();
        Console.WriteLine("===== RESULTADO =====");
        Console.WriteLine($"Nome: {Nome}");
        Console.WriteLine($"Peso: {Peso:F2} kg");
        Console.WriteLine($"Altura: {Altura:F2} m");
        Console.WriteLine($"IMC: {imc:F2}");
        Console.WriteLine($"Classificação: {ClassificacaoIMC()}");
        Console.WriteLine("=====================");
    }
}
