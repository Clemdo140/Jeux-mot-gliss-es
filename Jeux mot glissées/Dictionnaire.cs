using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeux_mot_glissées
{
    internal class Dictionnaire
    {
        List<List<string>> mots;
        string langue;

        public Dictionnaire(List<List<string>> mots, string langue)
        {
            this.mots = mots;
            this.langue = langue;
        }
        public List<List<string>> Mots
        {
            get { return mots; }
        }
        public string Langue
        {
            get { return langue; }
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
