using Hospitaal.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Hospitaal.Controllers
{
    public class ReservaController
    {
        [Route("api/[controller]")]
        [ApiController]
        public class ReservasController : ControllerBase
        {
            private readonly ReservaService _reservaService;

            public ReservasController(ReservaService reservaService)
            {
                _reservaService = reservaService ?? throw new ArgumentNullException(nameof(reservaService));
            }

            [HttpGet]
            public IActionResult Get()
            {
                try
                {
                    var reservas = _reservaService.GetReservas();
                    return Ok(reservas);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error interno del servidor: {ex.Message}");
                }
            }

            [HttpPost]
            public IActionResult Post([FromBody] Reserva reserva)
            {
                try
                {
                    _reservaService.CrearReserva(reserva);
                    return Ok("Reserva creada exitosamente.");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error al crear la reserva: {ex.Message}");
                }
            }

            [HttpDelete("{id}")]
            public IActionResult Delete(int id)
            {
                try
                {
                    var result = _reservaService.EliminarReserva(id);
                    if (result.Success)
                    {
                        return Ok(result.Message);
                    }
                    else
                    {
                        return BadRequest(result.Message);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error al eliminar la reserva: {ex.Message}");
                }
            }

            [HttpPut("{id}")]
            public IActionResult Put(int id, [FromBody] Reserva reserva)
            {
                try
                {
                    var result = _reservaService.EditarReserva(id, reserva);
                    if (result.Success)
                    {
                        return Ok(result.Message);
                    }
                    else
                    {
                        return BadRequest(result.Message);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error al actualizar la reserva: {ex.Message}");
                }
            }
        }

        public class ReservaService
        {
            private readonly IConfiguration _configuration;

            public ReservaService(IConfiguration configuration)
            {
                _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            }

            public List<Reserva> GetReservas()
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Reserva";
                using MySqlCommand command = new MySqlCommand(query, connection);
                using MySqlDataReader dataReader = command.ExecuteReader();

                List<Reserva> reservas = new List<Reserva>();

                while (dataReader.Read())
                {
                    Reserva reserva = MapToReserva(dataReader);
                    reservas.Add(reserva);
                }

                return reservas;
            }

            public void CrearReserva(Reserva reserva)
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = "INSERT INTO Reserva (Especialidad, DiaReserva, IdPaciente) VALUES (@Especialidad, @DiaReserva, @IdPaciente)";

                using MySqlCommand command = new MySqlCommand(query, connection);
                SetReservaParameters(command, reserva);

                command.ExecuteNonQuery();
            }

            public OperationResult EliminarReserva(int id)
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Reserva WHERE IdReserva = @id";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return new OperationResult(true, "Reserva eliminada correctamente.");
                }
                else
                {
                    return new OperationResult(false, "No se encontró la reserva con el ID especificado.");
                }
            }

            public OperationResult EditarReserva(int id, Reserva reserva)
            {
                string connectionString = _configuration.GetConnectionString("MySqlConnection");

                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = "UPDATE Reserva SET Especialidad = @Especialidad, DiaReserva = @DiaReserva, IdPaciente = @IdPaciente WHERE IdReserva = @id";

                using MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                SetReservaParameters(command, reserva);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return new OperationResult(true, "Reserva actualizada correctamente.");
                }
                else
                {
                    return new OperationResult(false, "No se encontró la reserva con el ID especificado.");
                }
            }

            private static Reserva MapToReserva(MySqlDataReader dataReader)
            {
                return new Reserva
                {
                    IdReserva = Convert.ToInt32(dataReader["IdReserva"]),
                    Especialidad = dataReader["Especialidad"].ToString(),
                    DiaReserva = Convert.ToDateTime(dataReader["DiaReserva"]),
                    IdPaciente = Convert.ToInt32(dataReader["IdPaciente"])
                };
            }

            private static void SetReservaParameters(MySqlCommand command, Reserva reserva)
            {
                command.Parameters.AddWithValue("@Especialidad", reserva.Especialidad);
                command.Parameters.AddWithValue("@DiaReserva", reserva.DiaReserva);
                command.Parameters.AddWithValue("@IdPaciente", reserva.IdPaciente);
            }
        }

        public class OperationResult
        {
            public bool Success { get; }
            public string Message { get; }

            public OperationResult(bool success, string message)
            {
                Success = success;
                Message = message;
            }
        }
    }
}
