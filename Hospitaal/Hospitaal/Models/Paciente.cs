namespace Hospitaal.Models
{
    public class Paciente
    {

        public int idPaciente {  get; set; }
        public int IdPaciente { get; internal set; }
        public string NombrePac {  get; set; }
        public string ApellidoPac {  get; set; }

        public string RunPac {  get; set; }
        public string Nacionalidad { get; set; }
        public string Visa {  get; set; }
        public string Genero { get; set; }
        public string SintomasPac {  get; set; }
        public int Medico_idMedico {  get; set; }
        public object RutPac { get; internal set; }
        public object IdMedico { get; internal set; }
    }
}
