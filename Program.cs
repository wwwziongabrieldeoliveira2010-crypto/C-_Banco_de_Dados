Banco.CriarBanco();
Banco.CriarTabelaPessoa();

Imc pessoa = Imc.CadastrarPessoa();

pessoa.SalvarNoBanco();

pessoa.MostrarResultado();