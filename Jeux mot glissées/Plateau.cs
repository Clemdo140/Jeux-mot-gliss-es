using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeux_mot_glissées
{
    internal class Plateau
    {
        int nblignes = 8;
        int nbcolonnes = 8;
        string CHEMIN_LETTRES = "Lettres.txt";
        char[,] matrice;//le tableau en 2D actuel
        Random r = new Random();//pour générer aléatoirement le plateau
        private Dictionary<char, (int max, int poids)> lettrescontraintes; // Dictionnaire interne pour stocker les contraintes de Lettres.txt.

        public Plateau()   //Initialise le plateau par génération automatique et aléatoire à partir de Lettres.txt.
        {
            Matrice = new char[nblignes, nbcolonnes];
            lettrescontraintes = ChargerContraintesLettres();
            GenererPlateaualeatoire();
        }
        public Plateau(string nomfile) : this()  //Initialise par fichier
        {
            ToRead(nomfile);
        }

        public char[,] Matrice
        {
            get { return matrice; }
            private set {; }
        }
   
        public string toString()
        {
            return ($"matlettres : {this.matrice} \nlettrescontraintes : {this.lettrescontraintes} ");
        }
        public void ToFile(string nomfile)
        {

        }
        public void GenererPlateaualeatoire()//génère le plateau aléatoirement
        {
            List<char> lettresPool = new List<char>();

            foreach (var kvp in lettrescontraintes)//parcours la règle de chaque lettre
            {
                for (int i = 0; i < kvp.Value.max; i++)//la pondération, ajoute la lettre en son nombre maximum de fois
                {
                    lettresPool.Add(kvp.Key);
                }
            }

            if (lettresPool.Count == 0) { //si le nombre d'élément de la liste est nul, erreur
            return;
            }

            for (int i = 0; i < nblignes; i++)
            {
                for (int j = 0; j < nbcolonnes; j++)
                {
                    int index = r.Next(lettresPool.Count); //choix d'un index aléatoire
                    Matrice[i, j] = lettresPool[index];  //remplissage aléatoire de la matrice
                }
            }
        }
        public void ToRead(string nomfile)
        {

        }
        private Dictionary<char, (int max, int poids)> ChargerContraintesLettres()
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
