using System;
using System.Collections.Generic;
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

            return posiblesE.Max();
        }
 
    }
}