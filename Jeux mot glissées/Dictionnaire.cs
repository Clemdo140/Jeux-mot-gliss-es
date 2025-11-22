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
        private const string NOM_FICHIER_DICO = "MotsFrancais.txt";
        private List<List<string>> motsparlettre; //Liste des 26 lettres et chaque lettre contient une liste contenant tous les mots qui commencent par cette lettre
        private string langue = "Français"; 

        public List<List<string>> Motsparlettre
        {
            get { return motsparlettre; }
            private set { motsparlettre = value;}
        }
    
        
        public string Langue
        {
            get { return langue; }
            private set {langue = value; }
        }
        public Dictionnaire()
        {
            this.Langue = "Français";
            Motsparlettre = new List<List<string>>();
            // Initialisation des 26 sous-listes (une par lettre)
            for (int i = 0; i < 26; i++)
            {
                Motsparlettre.Add(new List<string>());
            }

            LireDictionnaire();
            Tri_QuickSort(); // Appel au tri imposé après le chargement
        }

        public void Tri_QuickSort()//utilise Quicksort
        {
        }

        public void QuickSort()//utilise Partition
        {
        }
        private int Partition()
        {

        }
        private void LireDictionnaire()
        {

        }
        public bool RechDichoRecursif(string mot)//utilise  RechercherMotRecursifAide
        {
            return false;
        }

        private bool RechercherMotRecursifAide(List<string> liste, string mot, int debut, int fin)
        {

        }
        public string toString()//Afficher l'ensemble des mots du dictionnaire
        {
            int totalMots = 0;

            foreach (var listeMots in Motsparlettre)//on calcul le nombre total de mots
            {
                if (listeMots != null)
                {
                    totalMots += listeMots.Count;//ajoute le nombre de mot qui commencent par la lettre de la boucle
                }
            }

            string description = "--- Dictionnaire " + this.Langue + " ---";
            description += "\nNombre total de mots : " + totalMots.ToString();

            return description;
        }
        
    }
}
