using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Lab2Cifrados.Models
{
    public class RSA
    {

        public bool EsPrimo(int n)
        {
            bool esPrimo;
            int cantDivExactas = 0;

            for (int i = 1; i < (n + 1); i++)
            {
                if ((n % i) == 0)
                {
                    cantDivExactas++;
                }
            }

            if (cantDivExactas == 2)
            {
                esPrimo = true;
            }
            else
            {
                esPrimo = false;
            }

            return esPrimo;
        }

        public int MCD(int a, int b)
        {
            var nMax = Math.Max(a, b);
            var nMin = Math.Min(a, b);

            if (nMin == 0)
            {
                return nMax;
            }

            else
            {
                return MCD(nMin, nMax % nMin);
            }

        }

        public int CalcularE(int n, int phiN)
        {
            var hastaPhiN = new List<int>();
            var posiblesE = new List<int>();
            var e = 0;
            var mitadDeLista = 0;

            for (int i = 2; i < phiN; i++)
            {
                hastaPhiN.Add(i);
            }

            foreach (var item in hastaPhiN)
            {
                if ((MCD(item, n) == 1) && (MCD(item, phiN) == 1))
                {
                    posiblesE.Add(item);
                }
            }

            mitadDeLista = posiblesE.Count() / 2;
            e = posiblesE.ElementAt<int>(mitadDeLista);
            return e;
        }

        public int CalcularD(int e, int phiN)
        {

            e = e % phiN;
            for (int d = 1; d < phiN; d++)
            {
                if (((e * d) % phiN) == 1)
                {
                    return d;
                }
            }

            return 1;
        }

        public string[] ObtenerKeys(string Ruta)
        {
            string[] key = new string[2];
            var cadena = string.Empty;
            using (var stream = new FileStream(Ruta, FileMode.Open))
            {
                using (var Lector = new StreamReader(stream))
                {
                    while (!Lector.EndOfStream)
                    {
                        cadena = Lector.ReadLine();
                    }
                }
            }
            key = cadena.Split(',');
            return key;
        }

        public int CifYDescifNumero(int NCifrar, int llave, int n)
        {
            /*ulong potencia = Convert.ToUInt64(Math.Pow(Convert.ToUInt64(NCifrar), Convert.ToUInt64(llave)));
            var cifrado = Convert.ToInt32(potencia % Convert.ToUInt64(n));*/
            int valor = 1;
            for (int i = 0; i < llave; i++)
            {
                valor = valor * NCifrar % n;
            }
            return valor;
        }
        
    }
}