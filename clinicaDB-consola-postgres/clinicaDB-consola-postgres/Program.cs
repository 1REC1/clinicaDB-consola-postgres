using Npgsql;  // Importa Npgsql para PostgreSQL

namespace ClinicaApp
{
    class Program
    {
        private static string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=yourpassword;Database=clinicaDB;";

        static void Main(string[] args)
        {
            if (Login(out string role))
            {
                ShowMenu(role);
            }
        }

        static bool Login(out string role)
        {
            Console.WriteLine("=== Login ===");
            Console.Write("Usuario: ");
            string username = Console.ReadLine();
            Console.Write("Contraseña: ");
            string password = Console.ReadLine();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT nombre, contrasena, rol FROM usuarios WHERE nombre = @username AND contrasena = @password";
                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                connection.Open();
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    role = reader["rol"].ToString();
                    connection.Close();
                    Console.WriteLine("Login exitoso.\n");
                    return true;
                }
                else
                {
                    connection.Close();
                    Console.WriteLine("Usuario o contraseña incorrectos.\n");
                    role = null;
                    return false;
                }
            }
        }

        static void ShowMenu(string role)
        {
            while (true)
            {
                Console.WriteLine("=== Menú Principal ===");

                if (role == "admin")
                {
                    Console.WriteLine("1. Gestión de Pacientes");
                    Console.WriteLine("2. Gestión de Médicos");
                    Console.WriteLine("3. Salir");
                }
                else if (role == "médicos")
                {
                    Console.WriteLine("1. Gestión de Pacientes");
                    Console.WriteLine("2. Listar Médicos");
                    Console.WriteLine("3. Salir");
                }
                else if (role == "pacientes")
                {
                    Console.WriteLine("1. Listar Pacientes");
                    Console.WriteLine("3. Salir");
                }
                else
                {
                    Console.WriteLine("No tienes permisos para acceder a este menú.\n");
                    return;
                }

                Console.Write("Seleccione una opción: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        if (role == "pacientes")
                        {
                            ListPacientes();
                        }
                        else if (role == "médicos" || role == "admin")
                        {
                            ManagePacientes();
                        }
                        break;
                    case "2":
                        if (role == "médicos" || role == "admin")
                        {
                            ManageMedicos();
                        }
                        else if (role == "pacientes")
                        {
                            Console.WriteLine("No tienes permisos para gestionar médicos.\n");
                        }
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Opción no válida.\n");
                        break;
                }
            }
        }

        static void ManagePacientes()
        {
            while (true)
            {
                Console.WriteLine("=== Gestión de Pacientes ===");
                Console.WriteLine("1. Crear Paciente");
                Console.WriteLine("2. Listar Pacientes");
                Console.WriteLine("3. Volver");
                Console.Write("Seleccione una opción: ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        CreatePaciente();
                        break;
                    case "2":
                        ListPacientes();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Opción no válida.\n");
                        break;
                }
            }
        }

        static void CreatePaciente()
        {
            Console.WriteLine("=== Crear Paciente ===");
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                string query = "INSERT INTO pacientes (Nombre, Fecha_Creacion) VALUES (@nombre, @fecha)";
                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombre", nombre);
                command.Parameters.AddWithValue("@fecha", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                Console.WriteLine("Paciente creado exitosamente.\n");
            }
        }

        static void ListPacientes()
        {
            Console.WriteLine("=== Lista de Pacientes ===");

            using (var connection = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT Id, Nombre, Fecha_Creacion FROM pacientes";
                var command = new NpgsqlCommand(query, connection);

                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["Id"]}, Nombre: {reader["Nombre"]}, Fecha de Creación: {reader["Fecha_Creacion"]}");
                }

                reader.Close();
                connection.Close();
            }

            Console.WriteLine();
        }

        static void ManageMedicos()
        {
            while (true)
            {
                Console.WriteLine("=== Gestión de Médicos ===");
                Console.WriteLine("1. Crear Médico");
                Console.WriteLine("2. Listar Médicos");
                Console.WriteLine("3. Volver");
                Console.Write("Seleccione una opción: ");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        CreateMedico();
                        break;
                    case "2":
                        ListMedicos();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Opción no válida.\n");
                        break;
                }
            }
        }

        static void CreateMedico()
        {
            Console.WriteLine("=== Crear Médico ===");
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                string query = "INSERT INTO medicos (Nombre, Fecha_Creacion) VALUES (@nombre, @fecha)";
                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombre", nombre);
                command.Parameters.AddWithValue("@fecha", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                Console.WriteLine("Médico creado exitosamente.\n");
            }
        }

        static void ListMedicos()
        {
            Console.WriteLine("=== Lista de Médicos ===");

            using (var connection = new NpgsqlConnection(connectionString))
            {
                string query = "SELECT Id, Nombre, Fecha_Creacion FROM medicos";
                var command = new NpgsqlCommand(query, connection);

                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["Id"]}, Nombre: {reader["Nombre"]}, Fecha de Creación: {reader["Fecha_Creacion"]}");
                }

                reader.Close();
                connection.Close();
            }

            Console.WriteLine();
        }
    }
}
