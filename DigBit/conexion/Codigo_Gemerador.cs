using System;

namespace DigBit.conexion
{
    internal class CodigoGenerador
    {
        private static readonly Random random = new Random();
        private const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /// Genera un código aleatorio de longitud especificada.
        public static string GenerarCodigoAleatorio(int longitud)
        {
            char[] codigo = new char[longitud];
            for (int i = 0; i < codigo.Length; i++)
            {
                codigo[i] = caracteres[random.Next(caracteres.Length)];
            }
            return new string(codigo);
        }
    }
}
