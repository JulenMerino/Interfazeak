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

        public async Task<decimal?> ObtenerSaldoPorDescripcionAsync(string nan, string descripcion)
        {
            string query = "SELECT Saldo FROM gordailuak WHERE Bezeroak_NAN = @PkNan AND  Deskripzioa = @descripcion;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@PkNan", nan);
                command.Parameters.AddWithValue("@descripcion", descripcion);

                try
                {
                    await connection.OpenAsync();
                    var result = await command.ExecuteScalarAsync();

                    // Convertir el resultado a decimal o null si no se encuentra
                    return result != null ? Convert.ToDecimal(result) : (decimal?)null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener el importe: " + ex.Message);
                    return null;
                }
            }
        }


        public async Task<bool> EliminarMaileguaPorDescripcionAsync(string descripcion)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = "DELETE FROM Maileguak WHERE Deskripzioa = @descripcion";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@descripcion", descripcion);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0; // Si se eliminó al menos una fila, devolvemos true
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Manejo de errores de MySQL
                Console.WriteLine("Error de MySQL: " + ex.Message);
                return false;
            }
        }


        public async Task<List<string>> ObtenerListaMaileguakAsync()
        {
            List<string> listaMaileguak = new List<string>();
            string query = "SELECT Deskripzioa FROM maileguak;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    await connection.OpenAsync();
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            listaMaileguak.Add(reader.GetString(0)); // Suponiendo que la columna Deskribapena es la primera
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener los datos: " + ex.Message);
                }
            }

            return listaMaileguak;
        }









        public async Task<bool> ModificarImportePorDescripcionAsync(string descripcion, decimal nuevoImporte)
        {
            string query = "UPDATE deposituak SET Importe = @importe WHERE Deskribapena = @descripcion;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@importe", nuevoImporte);
                command.Parameters.AddWithValue("@descripcion", descripcion);

                try
                {
                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al modificar el importe: " + ex.Message);
                    return false;
                }
            }
        }

        public async Task<bool> EliminarDepositoPorDescripcionAsync(string descripcion)
        {
            string query = "DELETE FROM deposituak WHERE Deskribapena = @descripcion;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@descripcion", descripcion);

                try
                {
                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al eliminar el depósito: " + ex.Message);
                    return false;
                }
            }
        }

        public async Task<(string Descripcion, decimal Importe, int Plazo, DateTime Fecha)> ObtenerPrestamoPorNanYDescripcionAsync(string pkNan, string descripcion)
        {
            string query = @"SELECT Deskripzioa, Kantitatea, EpeHilabete, HasieraData FROM Maileguak WHERE Bezeroak_NAN = @PkNan AND Deskripzioa = @Descripcion";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@PkNan", pkNan);
                command.Parameters.AddWithValue("@Descripcion", descripcion);

                try
                {
                    await connection.OpenAsync();
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Verificar si los valores no son DBNull antes de asignarlos
                            string descripcionResult = reader["Deskripzioa"] != DBNull.Value ? reader["Deskripzioa"].ToString() : null;
                            decimal importeResult = reader["Kantitatea"] != DBNull.Value ? reader.GetDecimal("Kantitatea") : 0.0m;
                            int plazoResult = reader["EpeHilabete"] != DBNull.Value ? reader.GetInt32("EpeHilabete") : 0;
                            DateTime fechaResult = reader["HasieraData"] != DBNull.Value ? reader.GetDateTime("HasieraData") : DateTime.MinValue;

                            return (descripcionResult, importeResult, plazoResult, fechaResult);
                        }
                        else
                        {
                            return (null, 0, 0, DateTime.MinValue); // No se encontró un préstamo
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener el préstamo: " + ex.Message);
                    return (null, 0, 0, DateTime.MinValue);
                }
            }
        }




    }
}
