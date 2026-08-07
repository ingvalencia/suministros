using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Net.Mail;


    class clss_Funciones
    {
        public string CompletaCadena(string cadena, int tamaño, string caracter, char lado)
        {
            if (lado == 'D') // Complemento a la Derecha
            {
                if (cadena.Length < tamaño)
                {
                    return CompletaCadena(cadena + caracter, tamaño, caracter, lado);
                }
                else
                {
                    return cadena;
                }
            }
            else if (lado == 'I') // Complemento a la Izquierda
            {
                if (cadena.Length < tamaño)
                {
                    return CompletaCadena(caracter + cadena, tamaño, caracter, lado);
                }
                else
                {
                    return cadena;
                }
            }
            else
            {
                return cadena;
            }
        }       
    }
