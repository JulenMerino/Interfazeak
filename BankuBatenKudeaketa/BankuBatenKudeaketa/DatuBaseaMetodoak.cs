using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BankuBatenKudeaketa
{
    class DatuBaseaMetodoak
    {
        private readonly string connectionString = "Server=localhost;Port=3306;Database=bankubatenkudeaketa;User Id=root;Password=mysql;";

        // Método para conectar a la base de datos y confirmar la conexión
        public async Task<string> ConectarBaseDatosAsync()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string database = connection.Database;
                    string server = connection.DataSource;

                    return $"Konektatuta ondorengo datubasera: '{database}' ondorengo zerbitzarian '{server}'";
                }
                catch (Exception ex)
                {
                    return $"Errorea konektatzean: {ex.Message}";
                }
            }
        }

        // Método que devuelve el número de clientes en la tabla 'bezeroak'
        public int LortuBezeroKopurua()
        {
            int clienteCount = 0;
            string query = "SELECT COUNT(*) FROM bezeroak;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();
                    clienteCount = Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return clienteCount;
        }

        public async Task<(string NAN, string Nombre)> ObtenerClientePorLineaAsync(int linea)
        {
            string query = "SELECT NAN, Izena FROM bezeroak LIMIT 1 OFFSET @linea";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@linea", linea - 1); // Ajustar para que la línea sea 0-indexada

                try
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string nan = reader["NAN"].ToString();
                            string nombre = reader["Izena"].ToString();
                            return (nan, nombre);
                        }
                        else
                        {
                            return (null, null);  // Si no se encuentra el cliente en esa línea
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return (null, null);
                }
            }
        }

        public async Task<bool> InsertarClienteAsync(string nan, string nombre)
        {
            string query = "INSERT INTO bezeroak (NAN, Izena) VALUES (@nan, @nombre);";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nan", nan);
                command.Parameters.AddWithValue("@nombre", nombre);

                try
                {
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();  // Ejecuta el comando INSERT
                    return true;  // Si todo va bien, devolver verdadero
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al insertar cliente: " + ex.Message);
                    return false;  // Si ocurre algún error, devolver falso
                }
            }
        }
        // Método para eliminar un cliente de la base de datos
        public async Task<bool> EliminarClienteAsync(string nan, string nombre)
        {
            string query = "DELETE FROM bezeroak WHERE NAN = @nan AND Izena = @nombre;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nan", nan);
                command.Parameters.AddWithValue("@nombre", nombre);

                try
                {
                    await connection.OpenAsync();
                    int filasAfectadas = await command.ExecuteNonQueryAsync();  // Ejecuta el comando DELETE

                    // Si se eliminó al menos una fila, la eliminación fue exitosa
                    return filasAfectadas > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al eliminar cliente: " + ex.Message);
                    return false;  // Si ocurre algún error, devolver falso
                }
            }
        }

        public async Task<bool> ActualizarClienteAsync(string nan, string nombre)
        {
            string query = "UPDATE bezeroak SET Izena = @nombre WHERE NAN = @nan;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nan", nan);
                command.Parameters.AddWithValue("@nombre", nombre);

                try
                {
                    await connection.OpenAsync();
                    int filasAfectadas = await command.ExecuteNonQueryAsync();  // Ejecuta el comando UPDATE

                    // Si se actualizó al menos una fila, la actualización fue exitosa
                    return filasAfectadas > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al actualizar cliente: " + ex.Message);
                    return false;  // Si ocurre algún error, devolver falso
                }
            }
        }

        public async Task<List<string>> ObtenerTodosLosNANAsync()
        {
            string query = "SELECT NAN FROM bezeroak;";  // Aquí obtenemos solo el NAN

            List<string> nanList = new List<string>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    await connection.OpenAsync();  // Asegurarse de abrir la conexión de forma asincrónica
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())  // Leer los registros
                        {
                            string nan = reader["NAN"].ToString();  // Obtener el NAN
                            nanList.Add(nan);  // Añadir el NAN a la lista
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener los NAN: " + ex.Message);
                }
            }

            return nanList;  // Retornar la lista de NANs
        }

        public async Task<(string NAN, string Nombre)> ObtenerClientePorNANAsync(string nan)
        {
            string query = "SELECT NAN, Izena FROM bezeroak WHERE NAN = @nan LIMIT 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nan", nan);

                try
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string nombre = reader["Izena"].ToString();
                            return (nan, nombre);  // Retorna el NAN y el nombre
                        }
                        else
                        {
                            return (null, null);  // Si no se encuentra el cliente
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener cliente por NAN: " + ex.Message);
                    return (null, null);  // Si ocurre algún error, devuelve null
                }
            }
        }

        public async Task<List<string>> ObtenerCuentasPorNANAsync(string nan)
        {
            string query = "SELECT Deskripzioa FROM gordailuak WHERE Bezeroak_NAN = @nan;";  // Ajusta la consulta según la estructura de tu base de datos

            List<string> cuentasList = new List<string>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nan", nan);

                try
                {
                    await connection.OpenAsync();  // Asegurarse de abrir la conexión de forma asincrónica
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())  // Leer los registros
                        {
                            string descripcionCuenta = reader["Deskripzioa"].ToString();  // Obtener la descripción de la cuenta
                            cuentasList.Add(descripcionCuenta);  // Añadir la cuenta a la lista
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener cuentas: " + ex.Message);
                }
            }

            return cuentasList;  // Retornar la lista de cuentas
        }

        public async Task<List<string>> ObtenerPrestamoPorNANAsync(string nan)
        {
            string query = "SELECT Deskripzioa FROM maileguak WHERE Bezeroak_NAN = @nan;";  // Ajusta la consulta según la estructura de tu base de datos

            List<string> prestamoList = new List<string>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nan", nan);

                try
                {
                    await connection.OpenAsync();  // Asegurarse de abrir la conexión de forma asincrónica
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())  // Leer los registros
                        {
                            string descripcionCuenta = reader["Deskripzioa"].ToString();  // Obtener la descripción de la cuenta
                            prestamoList.Add(descripcionCuenta);  // Añadir la cuenta a la lista
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener cuentas: " + ex.Message);
                }
            }

            return prestamoList;  // Retornar la lista de cuentas
        }



    }
}
