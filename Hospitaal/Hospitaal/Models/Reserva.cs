namespace Hospitaal.Models
{
    public class Reserva
    {
        public int idReserva {  get; set; }
        public int IdReserva { get; internal set; }
        public string Especialidad { get; set; }
        public DateTime DiaReserva { get; set; }
        public int Paciente_idPaciente { get; set; }
        public object IdPaciente { get; internal set; }
    }
}
