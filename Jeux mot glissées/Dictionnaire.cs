using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Jeux_mot_glissées
{
    internal class Dictionnaire
    {
        // Définition des attributs privés de la classe dictionnaire

        private List<string> mots; 
        private string langue = "Français"; //Langue du dictionnaire, par défaut en "Français"

        // Constructeur de la classe dictionnaire pour initialiser les attributs
        public Dictionnaire(string MotsFrancais.txt )
        {
            this.mots = mots;
            this.langue = langue;
        }
        public List<string> Mots
        {
            get { return this.mots; }
        }




       



















        public string Langue
        {
            get { return this.langue; }
        }




























        public string toString()
        {
            return ($"mots : {this.mots} \nlangue : {this.langue} ");
        }
        public bool RechDichoRecursif(string mot)
        {
            return false;
        }
        public void Tri_XXX()
        {

        }
    }
}
