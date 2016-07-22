using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Aplicacao
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1 - Entrada de dados
            Console.WriteLine("Informe o nome do cliente:");
            var NomeCliente = Console.ReadLine();
            Console.WriteLine("Informe o email do cliente:");
            var Email = Console.ReadLine();

            //string ConnectionString = @"Data Source=(localDB)\Projects;Initial Catalog=CADASTRO;Integrated Security=true";
            //string ConnectionString = @"Server=(localDB)\Projects;Database=CADASTRO;Integrated Security=true";
            string ConnectionString = getConnectionStringFromConfig();
            Console.WriteLine("String de conexão: " + ConnectionString);
            SqlConnection con = new SqlConnection(ConnectionString);

            // 2 - Gravacao de dados
            //var SQL = "insert into CLIENTES(NomeCliente,Email) values (@NomeCliente,@Email);";
            var SQL = "InsertCliente";
            var cmd = new SqlCommand(SQL, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NomeCliente", NomeCliente);
            cmd.Parameters.AddWithValue("@Email", Email);

            var sql2 = "delete CLIENTES where IdCliente=3";
            var cmd2 = con.CreateCommand();
            cmd2.CommandText = sql2;

            con.Open();

            // Busca clientes - utilizando DataReader
            //var sql3 = "select * from CLIENTES;";
            var sql3 = "SelectClientesProdutos";
            var cmd3 = new SqlCommand(sql3, con);
            cmd3.CommandType = CommandType.StoredProcedure;
            var dr = cmd3.ExecuteReader();
            Console.WriteLine("============================================================");
            Console.WriteLine("Clientes Existentes na tabela antes de executar comandos");
            while (dr.Read())
            {
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Nome Cliente: " + dr[1].ToString());
                Console.WriteLine("Email: " + dr[2].ToString());
            }
            Console.WriteLine("============================================================");
            dr.NextResult();
            Console.WriteLine("============================================================");
            Console.WriteLine("Produtos Existentes na tabela");
            while (dr.Read())
            {
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Nome Produto: " + dr[1].ToString());
            }
            Console.WriteLine("============================================================");
            dr.Close();
            // Fim da busca de clientes

            var trans = con.BeginTransaction();
            try
            {
                cmd.Transaction = trans;
                cmd2.Transaction = trans;
                Console.WriteLine("Conexão com o banco de dados efetuada com sucesso...");
                Console.WriteLine("Estado da conexão: " + con.State);
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();

                // Efetiva as alteracoes no BD
                trans.Commit();
                Console.WriteLine("Comandos executados com sucesso.");
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.WriteLine("Erro ao executar transacao");
                Console.WriteLine(e.GetType());
                Console.WriteLine(e.Message);
            }
            finally
            {
                con.Close();
                Console.WriteLine("Estado da conexão: " + con.State);
                Console.ReadKey();
            }
        }

        private static string getConnectionStringFromConfig()
        {
            return ConfigurationManager.ConnectionStrings["CADASTRO"].ConnectionString;
        }

        private static string getConnectionStringFromBuilder()
        {
            SqlConnectionStringBuilder ConBuilder = new SqlConnectionStringBuilder();
            ConBuilder.DataSource = @"(localDB)\Projects";
            ConBuilder.InitialCatalog = "CADASTRO";
            ConBuilder.IntegratedSecurity = true;
            string ConnectionString = ConBuilder.ConnectionString;
            return ConnectionString;
        }
    }
}
