using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeux_mot_glissées
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Instanciation de l'objet Plateau (appel du constructeur par défaut)
            Plateau plateauCourant = new Plateau();

            // 2. Appel de la méthode toString() pour obtenir l'affichage
            string affichage = plateauCourant.toString();

            // 3. Affichage sur la console
            Console.WriteLine(affichage);

            Console.ReadKey();
        }

    }
    }

