using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeux_mot_glissées
{
    internal class Plateau
    {
        string[,] matlettres; //grille de jeu
        Dictionary<char, Tuple<int, int>> lettrescontraintes; //Stocke les données lues dans Lettres.txt

        public Plateau(string[,] matlettres, Dictionary<char, Tuple<int, int>> lettrescontraintes)
        {
            this.matlettres = matlettres;
            this.lettrescontraintes = lettrescontraintes;
        }

        public string[,] Matlettres
        {
            get { return matlettres; }
        }
        public Dictionary<char, Tuple<int, int>> Lettrescontraintes
        {
            get { return lettrescontraintes; }
        }
        public string toString()
        {
            return ($"matlettres : {this.matlettres} \nlettrescontraintes : {this.lettrescontraintes} ");
        }
        public void ToFile(string nomfile)
        {

        }
        public void ToRead(string nomfile)
        {

        }

        public object Recherche_Mot(string mot)
        {
            return null;
        }
        public void Maj_Plateau(object objet)
        {

        }
    }
}
