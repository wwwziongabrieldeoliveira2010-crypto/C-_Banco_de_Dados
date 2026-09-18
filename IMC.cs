using MySql.Data.MySqlClient;

public class Imc
{
    public static string conexaoBanco =
        "server=localhost;database=IMC;user=root;password=Senac2026;";

    private string? nome;
    private decimal peso;
    private decimal altura;

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

    public static Imc CadastrarPessoa()
    {
        Imc pessoa = new Imc();

        
        while (true)
        {
            Console.Write("Digite seu nome: ");
            string? nome = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(nome) &&
                nome.All(char.IsLetter))
            {
                pessoa.Nome = nome;
                break;
            }

            Console.WriteLine(
                "Nome inválido! Digite apenas letras, sem espaços."
            );
        }

        
        while (true)
        {
            Console.Write("Digite seu peso (kg): ");
            string? entrada = Console.ReadLine();

            if (decimal.TryParse(entrada, out decimal peso) &&
                peso > 0)
            {
                pessoa.Peso = peso;
                break;
            }

            Console.WriteLine(
                "Peso inválido! Digite um número maior que zero."
            );
        }

        
        while (true)
        {
            Console.Write("Digite sua altura (m): ");
            string? entrada = Console.ReadLine();

            if (decimal.TryParse(entrada, out decimal altura) &&
                altura > 0)
            {
                pessoa.Altura = altura;
                break;
            }

            Console.WriteLine(
                "Altura inválida! Digite um número maior que zero."
            );
        }

        return pessoa;
    }

    
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

        public void SalvarNoBanco()
    {
        string sql = @"
            INSERT INTO Pessoa (nome, peso, altura)
            VALUES (@nome, @peso, @altura);
        ";

        using MySqlConnection conn =
            new MySqlConnection(conexaoBanco);

        conn.Open();

        using MySqlCommand cmd =
            new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@nome", Nome);
        cmd.Parameters.AddWithValue("@peso", Peso);
        cmd.Parameters.AddWithValue("@altura", Altura);

        cmd.ExecuteNonQuery();

        Console.WriteLine("Pessoa cadastrada no banco de dados!");
    }

    
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