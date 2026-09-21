using System;

namespace DigBit.conexion
{
    public enum EstadoCodigo
    {
        AunNoEmpieza,
        Vigente,
        Expirado,
        YaRegistrado
    }

    /// <summary>
    /// Ventana de validez de un codigo de acceso: la sesion de clase (materia,
    /// grupo, laboratorio) para la que el profesor lo genero. Todos los instantes
    /// vienen del reloj del servidor MySQL, no del equipo.
    /// </summary>
    public class VentanaCodigo
    {
        public int IdCodigo { get; set; }
        public string Codigo { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public DateTime AhoraServidor { get; set; }

        /// <summary>DateTime.Now de este equipo en el instante en que se leyo AhoraServidor.</summary>
        public DateTime LeidoEnRelojLocal { get; set; }

        public EstadoCodigo Estado { get; set; }

        // Descripcion de la sesion (fase 6): lo rellena Consultas.ObtenerVentanaSesion.
        public int IdLaboratorio { get; set; }
        public string Laboratorio { get; set; }
        public int IdGrupo { get; set; }
        public string Grupo { get; set; }
        public string Materia { get; set; }
        public string Profesor { get; set; }

        /// <summary>Desfase entre el reloj del servidor y el de este equipo (servidor - local).</summary>
        public TimeSpan Desfase
        {
            get { return AhoraServidor - LeidoEnRelojLocal; }
        }

        /// <summary>
        /// El fin de la ventana expresado en el reloj de este equipo. Es un valor
        /// estable: se puede guardar (Datos_User.VentanaActual) y consultar mucho
        /// despues sin que se desplace. Es lo que usan los temporizadores locales.
        /// </summary>
        public DateTime FinEnRelojLocal
        {
            get { return Fin - Desfase; }
        }

        /// <summary>Tiempo que le queda al codigo AHORA (negativo si ya expiro).</summary>
        public TimeSpan Restante
        {
            get { return FinEnRelojLocal - DateTime.Now; }
        }
    }
}
