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
            Console.WriteLine($"O seu nome é: {nome}");
            return nome;
        }
        set
        {
            if(string.IsNullOrWhiteSpace(value) || value.Any(c => !char.IsLetter(c)))
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
            Console.WriteLine($"O seu peso é: {peso}");
            return peso;
        }
        set
        {
            if(value <= 0)
            {
                Console.WriteLine("O peso deve ser maior que zero");
                return;
            }
            peso = value;
        }
    }

    public decimal Altura
    {
        get
        {
            Console.WriteLine($"A sua altura é: {altura}");
            return altura;
        }
        set
        {
            if(value <= 0)
            {
                Console.WriteLine("A altura deve ser maior que zero");
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

        Console.WriteLine("Nome inválido! Digite apenas letras, sem espaços.");
    }

    while (true)
    {
        Console.Write("Digite seu peso: ");
        string? entrada = Console.ReadLine();

        if (decimal.TryParse(entrada, out decimal peso) && peso > 0)
        {
            pessoa.Peso = peso;
            break;
        }

        Console.WriteLine("Peso inválido, Digite apenas um número maior que zero.");
    }

    return pessoa;
}
        
    }

