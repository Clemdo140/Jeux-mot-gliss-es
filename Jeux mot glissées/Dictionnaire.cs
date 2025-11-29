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
              
                if (listeMots != null)//on vérifie qu"elle n'est pas nulle
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
     
            string pivot = liste[fin];
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
        private void LireDictionnaire()
        {
            try//méthode try catch pour éviter une nouvelle fois les erreurs et afficher message d'erreur si l'instruction dangereuse ne fonctionne pas p14 cmo4
            {
               
                using (StreamReader sr = new StreamReader(NOM_FICHIER_DICO)) // ouvre le fichier en mode lecture bufferisée
                {
                    string ligne;

                
                    for (int i = 0; i < 26; i++)    //boucle pour lire les 26 lignes du fichier (une par lettre)
                    {
                        ligne = sr.ReadLine(); //lit la ligne

                      
                        if (ligne!=null)
                        {
                           
                            var mots = ligne.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)//on sépare la chaine de caractere en un tableau de mots, avec l'espace comme séparateur et StringSplitOptions.RemoveEmptyEntries permet d'enlever les chaines vides pour garder que les mots valides
                                            .Select(m => m.Trim().ToUpper())//on enleve les espaces et on met en majuscules
                                            .ToList();//on le convertit en liste car Split convertit forcément en tableau, dcp on reconvertit en liste après avoir nettoyé

                            Motsparlettre[i].AddRange(mots); //on stocka des mots dans la bonne sous-liste 
                        }
                    }
                } // 'using' ferme automatiquement le StreamReader ici, même en cas d'erreur.
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Erreur lors de la lecture du dictionnaire '{NOM_FICHIER_DICO}' : {ex.Message}");//en cas d'erreur
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
