using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static System.Net.Mime.MediaTypeNames;

namespace Jeux_mot_glissées
{
     public class Dictionnaire
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
        /// <summary>
        /// Cette fonction sert de point de départ.
        ///Elle parcourt l'ensemble des listes de mots que l'on veut trier (foreach (var listeMots in Motsparlettre)) 
        ///et lance le tri récursif sur chacune d'elles, en s'assurant qu'elles contiennent au moins deux éléments à trier
        /// </summary>
        public void Tri_QuickSort()
        {
            foreach (var listeMots in Motsparlettre) // on applique le tri sur chaque sous-liste dans le tableau
            {
              
                if (listeMots != null && listeMots.Count > 1)//on vérifie qu"elle n'est pas nulle
                    QuickSort(listeMots, 0, listeMots.Count - 1);
            }
        }
        /// <summary>
        /// Ce tri dichotomique permet de résoudre un grand problème en le divisant récursivement en sous-problèmes plus petits, jusqu'à ce que les sous-problèmes soient simples à résoudre
        /// L'algorithme commence par sélectionner l'élément du milieu de la liste, appelé le pivot
        ///Rôle du Pivot : Servir de point de référence pour la division de la liste en deux sous-ensembles :les elements plus grand que pivot et ceux plus petits.
        ///  ///Le rôle du pivot est de garantir qu'à la fin de la phase de partitionnemen, il se retrouve à sa position définitive dans la liste triée.
        /// Une fois que le pivot est placé, on sait qu'il est exactement à sa place correcte.
        ///Il ne sera plus jamais déplacé ou inclus dans les étapes récursives suivantes, car le tri est appliqué uniquement aux sous-listes à gauche et à droite de cet index.
        ///La methode est donc de trier successivement des pivots un par un
        /// </summary>

        private void QuickSort(List<string> liste, int debut, int fin)//récursive
        {
            if (debut < fin)//Condition d'arret 
            {
                int pivotIndex = Partition(liste, debut, fin);// sépare la liste en 3 parties, à gauche les elements < pivot, le pivot au milieu et à droite les elements > pivot
                QuickSort(liste, debut, pivotIndex - 1); // Tri partie gauche
                QuickSort(liste, pivotIndex + 1, fin);  // Tri partie droite
            }
        }

        /// <summary>
        /// //choisir un pivot et réarrange les autres éléments pour que seul le pivot soit a sa place
        /// </summary>

        private int Partition(List<string> liste, int debut, int fin)
        {
           
            int milieu = debut + (fin - debut) / 2; // Calcule le milieu
            string tempo = liste[milieu]; // Échange l'élément du milieu avec l'élément de fin
            liste[milieu] = liste[fin];
            liste[fin] = tempo;

            string pivot = liste[fin];//on utilise liste[fin] comme pivot 
            int i = debut - 1;//marqueur de la fin de la zone des plus petits éléments de pivot.

            for (int j = debut; j < fin; j++)//on ne touche pas au pivot qui est a la fin
            {
                if (string.Compare(liste[j], pivot, StringComparison.OrdinalIgnoreCase) <= 0)//on compare liste[j] et pivot en ne demandant pas de tenir compte des majuscules avec StringComparison.OrdinalIgnoreCase
                {
                    i++; //On avance le marqueur de la zone des petits éléments


                    string temp = liste[i]; //Si l'élément actuel  est plus petit ou égal au pivot, c'est un plus petit élément que pivot, et il doit aller à gauche.
                    liste[i] = liste[j];
                    liste[j] = temp;
                }
            }
            string temp2 = liste[i + 1];
            liste[i + 1] = liste[fin];//on place le pivot à sa position définitive, juste apres la limmite i des petits elements
            liste[fin] = temp2;
            return i + 1; //retourne l'index du pivot
        }

        /// <summary>
        /// Permet d'obtenir un tableau de 26 listes (une pour chaque lettre de A à Z), et chaque liste contient tous les mots du dictionnaire commençant par cette lettre, 
        /// qui vont être triés par la fonction Tri_QuickSort().
        /// </summary>
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
