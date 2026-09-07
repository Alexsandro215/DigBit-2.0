using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigBit.conexion

{
    internal class Datos_User
    {
            public static String snumeroIdentificador;
            public static String snumero;
            public static String getUser()
            {
                return snumeroIdentificador;
            }
            public static void SetUser(string matricula)
            {
                snumeroIdentificador = matricula;
            }

        public static String getcodigo() {
            return snumero;
        }

        public static void Setcodigo(string codigo) {
            snumero = codigo;
        }
    }
}
