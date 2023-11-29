using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospitaal.Models;
using MySqlConnector;

namespace Hospitaal.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MedicoService _medicoService;

        public MedicosController(IConfiguration configuration, MedicoService medicoService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _medicoService = medicoService ?? throw new ArgumentNullException(nameof(medicoService));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Medico>> Get()
        {
            try
            {
                List<Medico> medicos = _medicoService.GetMedicos();
                return Ok(medicos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult PostMedicos([FromBody] Medico medico)
        {
            try
            {
                _medicoService.CrearMedico(medico);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _medicoService.EliminarMedico(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Medico medico)
        {
            try
            {
                _medicoService.EditarMedico(id, medico);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }

    public class MedicoService
    {
        private readonly IConfiguration _configuration;

        public MedicoService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public List<Medico> GetMedicos()
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection");
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM Medico";
            using MySqlCommand command = new MySqlCommand(query, connection);
            using MySqlDataReader dataReader = command.ExecuteReader();

            List<Medico> medicos = new List<Medico>();

            while (dataReader.Read())
            {
                Medico medico = MapToMedico(dataReader);
                medicos.Add(medico);
            }

            return medicos;
        }

        public void CrearMedico(Medico medico)
        {
            // Implementar la lógica para crear un nuevo médico en la base de datos
        }

        public void EliminarMedico(int id)
        {
            // Implementar la lógica para eliminar un médico en la base de datos
        }

        public void EditarMedico(int id, Medico medico)
        {
            // Implementar la lógica para editar un médico en la base de datos
        }

        private static Medico MapToMedico(MySqlDataReader dataReader)
        {
            return new Medico
            {
                idMedico = Convert.ToInt32(dataReader["idMedico"]),
                NombreMed = dataReader["NombreMed"].ToString(),
                ApellidoMed = dataReader["ApellidoMed"].ToString(),
                RunMed = dataReader["RunMed"].ToString(),
                EunaCom = dataReader["EunaCom"].ToString(),
                NacionalidadMed = dataReader["NacionalidadMed"].ToString(),
                Especialidad = dataReader["Especialidad"].ToString(),
                TarifaHr = Convert.ToInt32(dataReader["TarifaHr"]),
                // Agregar más campos según sea necesario
            };
        }



    }
}

























