using Hospitaal.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Hospitaal.Controllers
{
    public class PacienteController
    {

        [Route("api/[controller]")]
        [ApiController]
       public class PacienteService
{
    private readonly IConfiguration _configuration;

    public PacienteService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public List<Paciente> GetPacientes()
    {
        string connectionString = _configuration.GetConnectionString("MySqlConnection");
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string query = "SELECT * FROM Paciente";
        using MySqlCommand command = new MySqlCommand(query, connection);
        using MySqlDataReader dataReader = command.ExecuteReader();

        List<Paciente> pacientes = new List<Paciente>();

        while (dataReader.Read())
        {
            Paciente paciente = MapToPaciente(dataReader);
            pacientes.Add(paciente);
        }

        return pacientes;
    }

    public void CrearPaciente(Paciente paciente)
    {
        string connectionString = _configuration.GetConnectionString("MySqlConnection");

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string query = "INSERT INTO Paciente (NombrePac, ApellidoPac, RutPac, Nacionalidad, Visa, Genero, SintomasPac, IdMedico) VALUES (@NombrePac, @ApellidoPac, @RutPac, @Nacionalidad, @Visa, @Genero, @SintomasPac, @IdMedico)";

        using MySqlCommand command = new MySqlCommand(query, connection);
        SetPacienteParameters(command, paciente);

        command.ExecuteNonQuery();
    }

    public void EliminarPaciente(int id)
    {
        string connectionString = _configuration.GetConnectionString("MySqlConnection");

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string query = "DELETE FROM Paciente WHERE IdPaciente = @id";

        using MySqlCommand command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        command.ExecuteNonQuery();
    }

    public void EditarPaciente(int id, Paciente paciente)
    {
        string connectionString = _configuration.GetConnectionString("MySqlConnection");

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string query = "UPDATE Paciente SET NombrePac = @NombrePac, ApellidoPac = @ApellidoPac, RutPac = @RutPac, Nacionalidad = @Nacionalidad, Visa = @Visa, Genero = @Genero, SintomasPac = @SintomasPac, IdMedico = @IdMedico WHERE IdPaciente = @id";

        using MySqlCommand command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        SetPacienteParameters(command, paciente);

        command.ExecuteNonQuery();
    }

    private static Paciente MapToPaciente(MySqlDataReader dataReader)
    {
        return new Paciente
        {
            IdPaciente = Convert.ToInt32(dataReader["IdPaciente"]),
            NombrePac = dataReader["NombrePac"].ToString(),
            ApellidoPac = dataReader["ApellidoPac"].ToString(),
            RutPac = dataReader["RutPac"].ToString(),
            Nacionalidad = dataReader["Nacionalidad"].ToString(),
            Visa = dataReader["Visa"].ToString(),
            Genero = dataReader["Genero"].ToString(),
            SintomasPac = dataReader["SintomasPac"].ToString(),
            IdMedico = Convert.ToInt32(dataReader["IdMedico"])
        };
    }

    private static void SetPacienteParameters(MySqlCommand command, Paciente paciente)
    {
        command.Parameters.AddWithValue("@NombrePac", paciente.NombrePac);
        command.Parameters.AddWithValue("@ApellidoPac", paciente.ApellidoPac);
        command.Parameters.AddWithValue("@RutPac", paciente.RutPac);
        command.Parameters.AddWithValue("@Nacionalidad", paciente.Nacionalidad);
        command.Parameters.AddWithValue("@Visa", paciente.Visa);
        command.Parameters.AddWithValue("@Genero", paciente.Genero);
        command.Parameters.AddWithValue("@SintomasPac", paciente.SintomasPac);
        command.Parameters.AddWithValue("@IdMedico", paciente.IdMedico);
    }
}
    }
}
