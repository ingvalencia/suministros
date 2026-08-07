using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Suministro
{
    class clss_Static
    {
        public static string TeclaPulsada;
        public static DateTime FechaTeclaPulsada;
        public static string TeclaFinal;

        public clss_Static() { }

        public string ObtieneTeclaPulsada()
        {
            return TeclaPulsada;
        }

        public void ColocaTeclaPulsada(string cad)
        {
            TeclaPulsada = cad;
        }

        public DateTime ObtieneFechaTeclaPulsada()
        {
            return FechaTeclaPulsada;
        }

        public void ColocaFechaTeclaPulsada(DateTime cad)
        {
            FechaTeclaPulsada = cad;
        }

        public string ObtieneTeclaFinal()
        {
            return TeclaFinal;
        }

        public void ColocaTeclaFinal(string cad)
        {
            TeclaFinal = cad;
        }
    }
}
