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

        string sql = "CREATE DATABASE IF NOT EXISTS ec";

        MySqlCommand cmd = new MySqlCommand(sql, conn);

        cmd.ExecuteNonQuery();

        Console.WriteLine("Banco criado!");
    }

    private static string conexaoBanco =
        "server=localhost;database=ec;user=root;password=Senac2026;";

    public static void CriarTabelaAluno()
    {
        using MySqlConnection conn = new MySqlConnection(conexaoBanco);

        conn.Open();

        string sql = @"
            CREATE TABLE IF NOT EXISTS aluno
            (
                id INT AUTO_INCREMENT PRIMARY KEY,
                nome VARCHAR(100) NOT NULL,
                idade INT NOT NULL,
                cpf VARCHAR(14) NOT NULL
            );
        ";

        MySqlCommand cmd = new MySqlCommand(sql, conn);

        cmd.ExecuteNonQuery();

        Console.WriteLine("Tabela aluno criada!");
    }
}
