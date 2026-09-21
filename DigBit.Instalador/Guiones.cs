using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text;

namespace DigBit.Instalador
{
    /// <summary>
    /// Todo lo que este instalador hace por debajo lo hacen los guiones de
    /// deploy\, que llevan meses de uso y de correcciones. Aqui NO se
    /// reimplementa nada: se construye la linea de comandos y se ejecuta.
    ///
    /// Es a proposito. Si el instalador hiciera su propia version de crear la
    /// cuenta o de escribir el registro, habria dos caminos que mantener y uno
    /// de los dos se quedaria atras.
    /// </summary>
    internal static class Guiones
    {
        /// <summary>
        /// Carpeta que contiene deploy\. Se busca junto al ejecutable y un nivel
        /// por encima, para que valga tanto en la raiz del paquete como en
        /// bin\Release mientras se desarrolla.
        /// </summary>
        public static string RaizPaquete()
        {
            return RaizPaqueteDesde(AppDomain.CurrentDomain.BaseDirectory);
        }

        /// <summary>
        /// La busqueda de verdad, con el punto de partida como parametro para
        /// poder comprobarla sin ejecutar el instalador entero.
        /// </summary>
        public static string RaizPaqueteDesde(string aqui)
        {
            for (int subir = 0; subir <= 4; subir++)
            {
                string candidata = aqui;
                for (int i = 0; i < subir; i++)
                {
                    candidata = Path.GetFullPath(Path.Combine(candidata, ".."));
                }

                if (File.Exists(Path.Combine(candidata, "deploy", "configurar_equipo.ps1")))
                {
                    return candidata;
                }
            }

            return null;
        }

        public static string Guion(string raiz, string nombre)
        {
            return Path.Combine(raiz, "deploy", nombre);
        }

        /// <summary>Numeros de serie de las memorias USB conectadas.</summary>
        public static List<string> MemoriasUsb()
        {
            List<string> series = new List<string>();
            try
            {
                using (ManagementObjectSearcher buscador = new ManagementObjectSearcher(
                    "SELECT Model, SerialNumber FROM Win32_DiskDrive WHERE InterfaceType='USB'"))
                using (ManagementObjectCollection discos = buscador.Get())
                {
                    foreach (ManagementBaseObject disco in discos)
                    {
                        using (disco)
                        {
                            object serie = disco["SerialNumber"];
                            object modelo = disco["Model"];
                            if (serie != null && !string.IsNullOrWhiteSpace(serie.ToString()))
                            {
                                series.Add((modelo == null ? "" : modelo + "  ") + serie.ToString().Trim());
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Sin WMI no se puede autorizar ninguna memoria; la casilla se
                // quedara deshabilitada y ya esta.
            }

            return series;
        }

        /// <summary>
        /// Envuelve un valor para pasarlo a PowerShell entre comillas simples,
        /// duplicando las que lleve dentro. Sin esto, una contrasena con un
        /// apostrofo rompe la linea entera o, peor, ejecuta otra cosa.
        /// </summary>
        public static string Comillas(string valor)
        {
            return "'" + (valor ?? string.Empty).Replace("'", "''") + "'";
        }
    }
}
