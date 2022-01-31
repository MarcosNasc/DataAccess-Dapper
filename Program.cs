using Dapper;
using DataAccessDapper.Models;
using Microsoft.Data.SqlClient;

namespace DataAccessDapper
{
    class Program
    {
        const string connectionString = "Server=localhost;Database=crud;Integrated Security=True;TrustServerCertificate=True";
        public static void Main(string[] args)
        {

            Menu();
        }

        public static void Menu()
        {
            Console.WriteLine("Gerenciar Contatos");
            Console.WriteLine("[C] Criar novo Contato");
            Console.WriteLine("[R] Listar todos Contatos");
            Console.WriteLine("[U] Atualizar Contato");
            Console.WriteLine("[D] Deletar Contato");
            Console.WriteLine("[E] Sair");

            var option = Console.ReadLine().ToUpper();
            int id;
            switch (option)
            {
                case "C":
                    Create();
                    break;
                case "R":
                    Read();
                    break;
                case "U":
                    Console.WriteLine("Informe o ID do Contato");
                    id = int.Parse(Console.ReadLine());
                    var contact = UpdateModel();
                    Update(id, contact);
                    break;
                case "D":
                    Console.WriteLine("Informe o ID do Contato");
                    id = int.Parse(Console.ReadLine());
                    Delete(id);
                    break;
                case "E":
                    Environment.Exit(0);
                    break;
            }
        }

        public static void Create()
        {

            var contact = new Contact();

            Console.WriteLine("Cadastro de Contatos");
            Console.WriteLine("Informe o Nome do Contato: ");
            contact.Name = Console.ReadLine();
            Console.WriteLine("Infome o Email do Contato ");
            contact.Email = Console.ReadLine();
            Console.WriteLine("Informe o Telefone do Contato");
            contact.Phone = Console.ReadLine();
            Console.WriteLine("Informe a data de nascimento do Contato");
            var birthDateString = Console.ReadLine();
            contact.Birthdate = DateTime.Parse(birthDateString);
            contact.CreateDate = DateTime.Now;

            Save(contact);
        }

        public static void Save(Contact contact)
        {
            string insert = @"INSERT INTO Contact (
                                        Name,
                                        Email, 
                                        Phone,
                                        Birthdate,
                                        CreateDate
                                    )
                                     VALUES(
                                         @Name,
                                         @Email,
                                         @Phone,
                                         @Birthdate,
                                         @CreateDate
                                    )";

            using (var connection = new SqlConnection(connectionString))
            {
                var rows = connection.Execute(insert, new
                {
                    contact.Name,
                    contact.Email,
                    contact.Phone,
                    contact.Birthdate,
                    contact.CreateDate
                });

                RetornaLinhas(rows, "inserida");
            }

        }

        public static void Read()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                IEnumerable<Contact> contacts = connection.Query<Contact>("SELECT * FROM Contact");
                int rows = 0;
                foreach (var contact in contacts)
                {
                    Console.WriteLine(contact.Id);
                    Console.WriteLine(contact.Name);
                    Console.WriteLine(contact.Email);
                    Console.WriteLine(contact.Phone);
                    Console.WriteLine(contact.Birthdate);
                    Console.WriteLine(contact.CreateDate);
                    rows++;
                }

                RetornaLinhas(rows, "retornada");
            }
        }

        public static void Update(int id, Contact contact)
        {
            string update = @" UPDATE Contact
                                SET Name = @Name,
                                    Email = @Email,
                                    Phone = @Phone
                                WHERE Id = @Id
                             ";

            using (var connection = new SqlConnection(connectionString))
            {
                var rows = connection.Execute(update, new
                {
                    contact.Name,
                    contact.Email,
                    contact.Phone,
                    id,
                });

                RetornaLinhas(rows, "atualizada");
            }

        }

        public static Contact UpdateModel()
        {
            var contact = new Contact();

            Console.WriteLine("Atualizar dados do Contato");
            Console.WriteLine("Informe o Nome do Contato: ");
            contact.Name = Console.ReadLine();
            Console.WriteLine("Infome o Email do Contato ");
            contact.Email = Console.ReadLine();
            Console.WriteLine("Informe o Telefone do Contato");
            contact.Phone = Console.ReadLine();

            return contact;
        }
        public static void Delete(int id)
        {
            string delete = "DELETE FROM Contact WHERE Id = @id";
            using (var connection = new SqlConnection(connectionString))
            {
                var rows = connection.Execute(delete, new
                {
                    id
                });

                RetornaLinhas(rows, "delatada");
            }

        }

        public static void RetornaLinhas(int qtdLinhas, string acao)
        {
            if (qtdLinhas == 1)
            {
                Console.WriteLine($"{qtdLinhas} linha {acao}");
            }
            else
            {
                Console.WriteLine($"{qtdLinhas} linhas {acao}s");
            }

        }
    }
}