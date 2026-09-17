using MySql.Data.MySqlClient;

public class Banco
{
    // Conexão com o servidor MySQL
    private static string conexaoServidor =
        "server=localhost;user=root;password=Senac2026;";

    // Conexão com o banco depois que ele for criado

    public static void CriarBanco()
    {
        using MySqlConnection conn = new MySqlConnection(conexaoServidor);

        conn.Open();

        string sql = "CREATE DATABASE IF NOT EXISTS IMC";

        MySqlCommand cmd = new MySqlCommand(sql, conn);

        cmd.ExecuteNonQuery();

        Console.WriteLine("Banco criado!");
    }

    private static string conexaoBanco =
        "server=localhost;database=IMC;user=root;password=Senac2026;";

    public static void CriarTabelaPessoa()
    {
        using MySqlConnection conn = new MySqlConnection(conexaoBanco);

        conn.Open();

        string sql = @"
            CREATE TABLE IF NOT EXISTS Pessoa
            (
                id INT AUTO_INCREMENT PRIMARY KEY,
                nome VARCHAR(100) NOT NULL,
                peso DECIMAL NOT NULL,
                altura DECIMAL NOT NULL
            );
        ";

        MySqlCommand cmd = new MySqlCommand(sql, conn);

        cmd.ExecuteNonQuery();

        Console.WriteLine("Tabela Pessoa criada!");
    }
}
