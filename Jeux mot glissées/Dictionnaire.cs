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
        private string langue ; 

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
            Tri_QuickSort(); // tri du dictionnaire
           
        }

        public void Tri_QuickSort()
        {
            foreach (var listeMots in Motsparlettre) // on applique le tri sur chaque sous-liste dans le tableau
            {
              
                if (listeMots != null && listeMots.Count > 1)//on vérifie qu"elle n'est pas nulle
                    QuickSort(listeMots, 0, listeMots.Count - 1);
            }
        }

        private void QuickSort(List<string> liste, int debut, int fin)//récursive
        {
            if (debut < fin)//Condition d'arret 
            {
                int pivotIndex = Partition(liste, debut, fin);// sépare la liste en 3 parties, à gauche les elements < pivot, le pivot au milieu et à droite les elements > pivot
                QuickSort(liste, debut, pivotIndex - 1); // Tri partie gauche
                QuickSort(liste, pivotIndex + 1, fin);  // Tri partie droite
            }
        }
        private int Partition(List<string> liste, int debut, int fin)//choisir un pivot et réarrange un segment de liste pour que le pivot se retrouve à sa place définitive.
        {
           
            int milieu = debut + (fin - debut) / 2; // Calcule le milieu
            string tempo = liste[milieu]; // Échange l'élément du milieu avec l'élément de fin
            liste[milieu] = liste[fin];
            liste[fin] = tempo;

            string pivot = liste[fin];//on utilise liste[fin] comme pivot 
            int i = (debut - 1);//pour indiquer la position du prochain élément

            for (int j = debut; j < fin; j++)
            {
                if (string.Compare(liste[j], pivot, StringComparison.OrdinalIgnoreCase) <= 0)//on compare liste[j] et pivot en ne demandant pas detenir compte des majuscules avec StringComparison.OrdinalIgnoreCase
                {
                    i++;
                
                    string temp = liste[i];
                    liste[i] = liste[j];
                    liste[j] = temp;
                }
            }
            string temp2 = liste[i + 1];
            liste[i + 1] = liste[fin];//on place le pivot à sa position définitive
            liste[fin] = temp2;
            return i + 1; //retourne l'index du pivot
        }
        // Dans Dictionnaire.cs

        private void LireDictionnaire()
        {
            try
            {
               
                string[] lignes = File.ReadAllLines(NOM_FICHIER_DICO);//lire toutes les lignes du fichier

                foreach (var ligne in lignes)
                {
                    if (string.IsNullOrWhiteSpace(ligne)) continue;

                   
                    var tokens = ligne.Split(null as char[], StringSplitOptions.RemoveEmptyEntries)
                                      .Select(m => m.Trim().ToUpper())
                                      .ToList();

                    if (tokens.Count == 0) continue;

                    foreach (string token in tokens)
                    {
                        
                        if (token.Length > 0 && token.All(char.IsLetter))// Vérifie que le token est composé UNIQUEMENT de lettres (ignore les nombres d'index)
                        {
                            char premiereLettre = token[0];

                            
                            if (premiereLettre >= 'A' && premiereLettre <= 'Z')//Distribution dans la bonne sous-liste (A, B, C...)
                            {
                                
                                int index = premiereLettre - 'A';// 'A' est à l'index 0, 'B' à 1, etc.
                                Motsparlettre[index].Add(token);
                            }
                        }
                    }
                }

            
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture du dictionnaire '{NOM_FICHIER_DICO}' : {ex.Message}");
            }
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
        public bool RechDichoRecursif(string mot)//Recherche dicotomique du mot dans le dictiionnaire
        {
            if (mot == null)
            {
                return false;
            }

            string motMaj = mot.ToUpper();
            char premiereLettre = motMaj[0];

            if (premiereLettre < 'A' || premiereLettre > 'Z') return false;//utilise l'index de la lettre 

           
            int index = premiereLettre - 'A'; // Détermine l'index de la liste cible 
            List<string> listeCible = Motsparlettre[index];

            // Lance la recherche dichotomique récursive sur la sous-liste triée.
            return RechercherMotRecursifAide(listeCible, motMaj, 0, listeCible.Count - 1);//on se restreint à rechercher les mots qui commencent par la même lettre
        }

        private bool RechercherMotRecursifAide(List<string> liste, string mot, int debut, int fin)
        {
            if (debut > fin) return false; // Condition d'arrêt : l'élément n'existe pas

            int milieu = debut + (fin - debut) / 2;
            int comparaison = string.Compare(mot, liste[milieu], StringComparison.OrdinalIgnoreCase);

            if (comparaison == 0)
            {
                return true; // Mot trouvé
            }
            else if (comparaison < 0)
            {
                return RechercherMotRecursifAide(liste, mot, debut, milieu - 1); // Recherche à gauche
            }
            else
            {
                return RechercherMotRecursifAide(liste, mot, milieu + 1, fin); // Recherche à droite
            }
        }

    }
}
