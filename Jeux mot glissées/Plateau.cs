using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Resources.ResXFileRef;

namespace Jeux_mot_glissées
{
    public class Plateau
    {
        private int nblignes;
        private int nbcolonnes;

        string CHEMIN_LETTRES = "Lettres.txt";
        char[,] matrice;//le tableau en 2D actuel
        Random r = new Random();//pour générer aléatoirement le plateau
        private Dictionary<char, (int max, int poids)> lettrescontraintes; // Dictionnaire interne pour stocker les contraintes de Lettres.txt.

        public Dictionary<char, (int max, int poids)> Lettrescontraintes
        {
            get { return lettrescontraintes; }
        }
        public int NbLignes { get { return nblignes; } }
        public int NbColonnes { get { return nbcolonnes; } }
        public Plateau() : this(8, 8) // Le constructeur par défaut appelle le nouveau constructeur avec les valeurs par défaut 8x8, comme dans le sujet du projet
        {

        }
        public Plateau(int lignes, int colonnes)//constructeur principal paour générér aléatoirement
        {

            this.nblignes = lignes;
            this.nbcolonnes = colonnes;

            Matrice = new char[nblignes, nbcolonnes];//on crée le plateau

            lettrescontraintes = ChargerContraintesLettres();//on importe les contraintes
            GenererPlateaualeatoire();
        }
        public Plateau(string nomfile) //initialise par fichier
        {
            this.nblignes = 0;
            this.nbcolonnes = 0;
            lettrescontraintes = ChargerContraintesLettres();
            ToRead(nomfile);
        }

        public char[,] Matrice
        {
            get { return matrice; }
             set { matrice = value; }
        }
        /// <summary>
        /// Affichage du plateau sous forme de chaîne de caractères
        /// </summary>
        /// <returns></returns>
        public string toString()//affichage du plateau

        {
            StringBuilder sb = new StringBuilder();//pour construire un affichage efficace
            for (int i = 0; i < nbcolonnes; i++)//ajoute le numéro de la ligne avant son contenu pour mieux se repérer
            {
                sb.Append("--");//.Append rajoute à l'affichage
            }
            sb.AppendLine("--");

            for (int i = 0; i < nblignes; i++)//ajoute le numéro de la ligne avant son contenu pour mieux se repérer
            {
                sb.Append((i + 1).ToString());   // Affiche le numéro de la ligne i+1
                if (i < 9)
                {
                    sb.Append($"{"|",2}");   // Ajoute un séparateur | avec un alignement sur 2 caractères de la ligne 1 à 9
                }
                else
                {
                    sb.Append("|"); // Ajoute un séparateur | sans alignement pour les lignes 10 et plus
                }

                for (int j = 0; j < nbcolonnes; j++)
                {

                    sb.Append(char.IsLetter(Matrice[i, j]) ? Matrice[i, j] + " " : "  ");// Affiche la lettre suivie d'un espace, ou deux espaces si la case est vide, Condotion ? sivrai : sifaux
                }
                sb.AppendLine();
            }
            return sb.ToString();  //Retourne la chaine construite
        }
        /// <summary>
        /// Sauvegarde le plateau dans un fichier au format CSV.
        /// </summary>
        /// <param name="nomfile"></param>
        public void ToFile(string nomfile)//sauvegarder l'état interne de la matrice de lettres, elle prend l'etat actuel de la matrice lettres et la convertit en format simple
        {
            {
                try // si le chemin est bon, méthode try/catch pour éviter les erreurs p14 cmo4
                {
                    string[] lignes = new string[nblignes];// on convertir un tableau 2D en 1 tableau 1D de chaines de caractères
                    for (int i = 0; i < nblignes; i++)
                    {
                        string ligne = "";
                        for (int j = 0; j < nbcolonnes; j++)
                        {
                            ligne += Matrice[i, j] + (j < nbcolonnes - 1 ? "," : ""); //Condition? valeursivrai : valeursifaux, le but étant d'ajouter une virgule sur chaque lettre sauf la dernière ligne
                        }
                        lignes[i] = ligne;
                    }
                    File.WriteAllLines(nomfile, lignes); //écrit toutes les lignes dans le fichier spécifié 
                }
                catch (Exception ex)//message affiché en cas d'erreur
                {
                    Console.WriteLine($"Erreur lors de la sauvegarde du plateau : {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Génère un plateau aléatoire en respectant les contraintes de lettres avec des lettres choisies de manière aléatoire
        /// Cette méthode assure que la répartition des lettres sur le plateau est équilibrée et conforme aux règles
        /// </summary>
        public void GenererPlateaualeatoire()//génère le plateau aléatoirement
        {

            List<char> lettresPond = new List<char>(); // Liste pour stocker les lettres disponibles en fonction des contraintes
            int totalCases = nblignes * nbcolonnes; // Nombre total de cases dans la matrice

            foreach (var kvp in lettrescontraintes)//parcours la règle de chaque lettre
            {

                for (int i = 0; i < kvp.Value.max; i++)//la pondération, ajoute la lettre en son nombre maximum de fois
                {
                    lettresPond.Add(kvp.Key); //ajoute la lettre au pool
                }
            }

            if (lettresPond.Count == 0)//gerer l'erreur. Si aucune lettre n'a été ajoutée au pool, on quitte la méthode
            {
                return;
            }


            for (int i = 0; i < nblignes; i++) //remplissage de la matrice
            {
                for (int j = 0; j < nbcolonnes; j++) 
                {
                    if (lettresPond.Count > 0) // Vérifie s'il reste des lettres dans le pool
                    {

                        int index = r.Next(lettresPond.Count);//choix d'un index aléatoire dans le pool restant


                        Matrice[i, j] = lettresPond[index];  // Remplissage de la matrice avec la lettre choisie


                        lettresPond.RemoveAt(index); //Retirer la lettre du pool après utilisation
                    }
                    else
                    {

                        Matrice[i, j] = ' ';// S'il n'y a plus de lettres disponibles dans le pool, remplir avec un espace vide 
                    }
                }
            }
        }
        /// <summary>
        /// Lit un fichier CSV pour initialiser le plateau
        /// transforme les éléments du fichier en éléments de la matrice
        /// </summary>
        /// <param name="nomfile"></param>
        public void ToRead(string nomfile) 
        {
            try//méthode try catch pour éviter une nouvelle fois les erreurs
            {
                string[] lignes = File.ReadAllLines(nomfile);//lit le contenu du fichier et le place en mémoire sous forme de tableau
                this.nblignes = lignes.Length; // le nombre de lignes du fichier devient le nombre de lignes du plateau
                if (nblignes > 0)
                {
                    this.nbcolonnes = lignes[0].Split(',').Length; // on compte combien il y a de colonnes en regardant la premiere ligne. On découpe avec split
                }

                // Redimensionnement de la Matrice
                Matrice = new char[nblignes, nbcolonnes];
                for (int i = 0; i < nblignes; i++)
                {
                    var lettres = lignes[i].Split(',').Select(s => s.Trim().ToUpper()).ToArray(); // Convertir toutes les cellules en majuscules/sans espaces une seule fois

                    for (int j = 0; j < nbcolonnes; j++)
                    {
                        string cellule = lettres[j];

                        if (string.IsNullOrEmpty(cellule))
                        {
                            Matrice[i, j] = ' '; // Si la chaîne est vide, la case est un espace
                        }
                        else
                        {
                            // Prendre le premier caractère de la cellule nettoyée
                            Matrice[i, j] = cellule[0]; // Garanti que chaque case du plateau contient une seule lettre
                        }
                    }
                }
                Console.WriteLine($"Plateau chargé depuis {nomfile}");
            }
            catch (FileNotFoundException f)//si on ne trouve pas le fichier
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ : Le fichier '{nomfile}' n'existe pas. {f.Message}");
                Console.ResetColor();
                Console.WriteLine($"Génération d'un plateau aléatoire");
                this.nblignes = 8;//par défaut
                this.nbcolonnes = 8;
                Matrice = new char[nblignes, nbcolonnes];
                GenererPlateaualeatoire();
            }
            catch (Exception ex)//si autre erreur
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erreur lors du chargement du plateau : {ex.Message}");
                Console.ResetColor();
                Console.WriteLine($"Génération d'un plateau aléatoire");
                this.nblignes = 8;//par défaut
                this.nbcolonnes = 8;
                Matrice = new char[nblignes, nbcolonnes];
                GenererPlateaualeatoire();
            }
        }
        /// <summary>
        /// Charge les contraintes des lettres depuis le fichier Lettres.txt
        /// elle les règles de génération du plateau et de scoring à partir du fichier Lettres.txt et les stocke dans une structure de données interne
        /// </summary>
        /// <returns></returns>
        private Dictionary<char, (int max, int poids)> ChargerContraintesLettres()//
        {
            var contraintes = new Dictionary<char, (int max, int poids)>();//nouveau dictionnaire sera retourné
            try//méthode try catch encore un efois pour éviter les erreurs
            {


                string[] lignes = File.ReadAllLines(CHEMIN_LETTRES);//lire toute les données du fichier

                foreach (var ligne in lignes)//parcourt chaque ligne
                {

                    if (ligne == null || ligne.Length == 0) continue;// Ignorer les lignes vides 


                    string[] parties = ligne.Split(','); // Séparation des données par virgule (format CSV)

                    if (parties.Length >= 3 && char.TryParse(parties[0].Trim().ToUpper(), out char lettre))//>=3 car lettre, poids, max et essaye de convertir la première partie de la ligne en une lettre majuscule en enlevant les espaces
                    {
                        if (int.TryParse(parties[1].Trim(), out int max) && int.TryParse(parties[2].Trim(), out int poids))
                        {

                            contraintes.Add(lettre, (max, poids));//on ajoute les contrainte de la lettre au dictionnaire
                        }
                    }
                }
            }
            catch (FileNotFoundException f)//si on ne trouve pas le fichier
            {
                Console.WriteLine($"Erreur: Le fichier des contraintes {CHEMIN_LETTRES} n'existe pas. {f.Message}");
            }
            catch (Exception ex)//en cas d'autre erreur
            {
                Console.WriteLine($"Erreur lors du traitement de {CHEMIN_LETTRES}: {ex.Message}");
            }
            return contraintes;
        }
        /// <summary>
        /// Recherche un mot dans le plateau en suivant les règles de déplacement
        /// elle etabli d'abord les conditions initiales puis recherche le mot rentré par le joueur et  cherche la premiere lettre du mot depuis la base
        /// </summary>
        /// <param name="mot"></param>
        /// <returns></returns>
        public object Recherche_Mot(string mot)
        {
            {

                if (string.IsNullOrWhiteSpace(mot) || mot.Length < 2) 
                    return null; // Mot invalide s'il est vide ou trop court. Rettourne null, cela évite de lancer une recherche inutile

                string motUpper = mot.ToUpper(); //convertit le mot en majuscules pour simplifier la recherche
                char premiereLettre = motUpper[0]; // Récupère la première lettre du mot à chercher



                int ligneBase = nblignes - 1; // Définit la ligne de départ comme la dernière ligne du plateau. Cela veut dire que la recherche commence toujours par le bas du plateau

                for (int j = 0; j < nbcolonnes; j++)//Boucle de départ, elle cherche la première lettre du mot donc le début du chemin
                {

                    if (char.IsLetter(Matrice[ligneBase, j]) && Matrice[ligneBase, j] == premiereLettre)// Vérifie que la case contient bien une lettre et que cette lettre correspond à la première lettre du mot recherché
                    {
                        List<(int, int)> chemin = new List<(int, int)> { (ligneBase, j) }; // Crée une liste de coordonnées (ligne, colonne) qui représente le chemin du mot. On ajoute la première case trouvée comme point de départ.

                        if (Recherche_Recursive(motUpper, 1, ligneBase, j, chemin)) // Appelle la méthode récursive pour chercher le reste du mot à partir de la position actuelle (ligneBase, j) et de l'index 1 (deuxième lettre du mot)
                            return chemin; // liste des coordonnées déjà trouvées
                    }
                }
                return null;
            }

        }

        /// <summary>
        ///  Permet de vérifier si la suite du chemin est valide pour le mot rentré par le joueur sur le plateau
        ///  à partir d'une position donnée, elle regarde si la lettre suivante du mot peut être trouvée dans l'une des cases voisines de déplacements autorisés
        /// </summary>
        /// <param name="mot"></param>
        /// <param name="indexMot"></param>
        /// <param name="ligCourante"></param>
        /// <param name="colCourante"></param>
        /// <param name="chemin"></param>
        /// <returns></returns>
        private bool Recherche_Recursive(string mot, int indexMot, int ligCourante, int colCourante, List<(int, int)> chemin)
        {
            if (indexMot == mot.Length)
            {
                return true; // Condition d'arrêt: tout le mot a été trouvé
            }

            char lettreCible = mot[indexMot]; // Récupère la lettre qu’on cherche à cette étape


            // Déplacements possibles:
            // HAUT:         -1, 0
            // GAUCHE:        0, -1
            // DROITE:        0, +1
            // DIAGONALE HG: -1, -1
            // DIAGONALE HD: -1, +1
            // BAS :         +1, 0 
            int[] dLig = { -1, 0, 0, -1, -1, 1 };
            int[] dCol = { 0, -1, 1, -1, 1, 0 };

            for (int k = 0; k < dLig.Length; k++) // Boucle sur toutes les directions possibles
            {
                int nLig = ligCourante + dLig[k];  // Calcule la nouvelle position ligne
                int nCol = colCourante + dCol[k];  // Calcule la nouvelle position colonne


                if (nLig >= 0 && nLig < nblignes && nCol >= 0 && nCol < nbcolonnes)//vérifie si on est dans les limites du plateau
                {

                    if (!chemin.Contains((nLig, nCol)) && Matrice[nLig, nCol] == lettreCible) //vérifie si la case est déjà dans le chemin et si la lettre correspond à la cible
                    {
                      
                        if (           

                        char.IsLetter(Matrice[nLig, nCol]) &&        // La case contient une lettre
                         Matrice[nLig, nCol] == lettreCible)          // La lettre correspond à celle recherchée
                        {
                            chemin.Add((nLig, nCol)); // Ajoute la nouvelle position au chemin si conditions vérifiées

                            if (Recherche_Recursive(mot, indexMot + 1, nLig, nCol, chemin)) // Appel récursif pour la lettre suivante du mot
                            {
                                return true; // Succès: on arrete de tester les autres directions
                            }

                            chemin.RemoveAt(chemin.Count - 1); // Échec, on annule le dernier mouvement

                        }
                    }
                }

            }
            return false; // Échec du chemin à partir de cette position
        }
        /// <summary>
        /// Met à jour le plateau après la suppression d'un mot trouvé
        /// Suppression : Elle vide les cases occupées par le mot trouvé
       /// Glissement : Elle fait tomber les lettres restantes dans chaque colonne pour combler les espaces vides .
        /// </summary>
        /// <param name="cheminMot"></param>
        public void Maj_Plateau(List<(int ligne, int colonne)> cheminMot)
        {
            if (cheminMot == null || cheminMot.Count == 0) // Si le chemin est nul ou vide rien à faire
                return;


            foreach (var coord in cheminMot) //Parcourt toutes les coordonnées du mot trouvé 
            {
                Matrice[coord.ligne, coord.colonne] = ' '; // Vide chacune de ces cases en les remplaçant par un espace
            }

            for (int j = 0; j < nbcolonnes; j++) // glissement colonne par colonne
            {

                for (int i = nblignes - 1; i >= 0; i--) // Parcourt la ligne du bas vers le haut
                {

                    if (Matrice[i, j] == ' ')// Si la case actuelle est vide
                    {

                        for (int k = i - 1; k >= 0; k--) //Chercher la première lettre non vide au-dessus (plus petit 'k')
                        {
                            if (Matrice[k, j] != ' ')
                            {
                                Matrice[i, j] = Matrice[k, j]; // on déplace la lettre vers le bas
                                Matrice[k, j] = ' '; // Vider l'ancienne position ('k')
                                break; // Passer à la ligne suivante 'i' de la colonne
                            }
                        }
                    }
                }
            }
        }
       

    }
}
