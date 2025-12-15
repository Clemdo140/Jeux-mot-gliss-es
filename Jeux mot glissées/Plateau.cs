using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
                sb.Append((i + 1).ToString());
                if (i < 9)
                {
                    sb.Append($"{"|",2}");
                }
                else
                {
                    sb.Append("|");
                }

                for (int j = 0; j < nbcolonnes; j++)
                {

                    sb.Append(char.IsLetter(Matrice[i, j]) ? Matrice[i, j] + " " : "  ");// Affiche la lettre suivie d'un espace, ou deux espaces si la case est vide, Condotion ? sivrai : sifaux
                }
                sb.AppendLine();
            }
            return sb.ToString();
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
                    File.WriteAllLines(nomfile, lignes);
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

            List<char> lettresPool = new List<char>();
            int totalCases = nblignes * nbcolonnes;

            foreach (var kvp in lettrescontraintes)//parcours la règle de chaque lettre
            {

                for (int i = 0; i < kvp.Value.max; i++)//la pondération, ajoute la lettre en son nombre maximum de fois
                {
                    lettresPool.Add(kvp.Key);
                }
            }

            if (lettresPool.Count == 0)//gerer l'erreur
            {
                return;
            }


            for (int i = 0; i < nblignes; i++)
            {
                for (int j = 0; j < nbcolonnes; j++)
                {
                    if (lettresPool.Count > 0)
                    {

                        int index = r.Next(lettresPool.Count);//choix d'un index aléatoire dans le pool restant


                        Matrice[i, j] = lettresPool[index];  // Remplissage de la matrice avec la lettre choisie


                        lettresPool.RemoveAt(index); //Retirer la lettre du pool après utilisation
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
                this.nblignes = lignes.Length;
                if (nblignes > 0)
                {
                    this.nbcolonnes = lignes[0].Split(',').Length;
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
                            Matrice[i, j] = cellule[0];
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
                    return null;

                string motUpper = mot.ToUpper();
                char premiereLettre = motUpper[0];



                int ligneBase = nblignes - 1; // La recherche commence systématiquement depuis la base de la matrice

                for (int j = 0; j < nbcolonnes; j++)//Boucle de départ, elle cherche la première lettre du mot donc le début du chemin
                {

                    if (char.IsLetter(Matrice[ligneBase, j]) && Matrice[ligneBase, j] == premiereLettre)// Vérifie que la case contient une lettre valide
                    {
                        List<(int, int)> chemin = new List<(int, int)> { (ligneBase, j) };

                        if (Recherche_Recursive(motUpper, 1, ligneBase, j, chemin))
                            return chemin;
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

            char lettreCible = mot[indexMot];

            // Déplacements possibles:
            // HAUT:         -1, 0
            // GAUCHE:        0, -1
            // DROITE:        0, +1
            // DIAGONALE HG: -1, -1
            // DIAGONALE HD: -1, +1
            // BAS :         +1, 0 
            int[] dLig = { -1, 0, 0, -1, -1, 1 };
            int[] dCol = { 0, -1, 1, -1, 1, 0 };

            for (int k = 0; k < dLig.Length; k++)
            {
                int nLig = ligCourante + dLig[k];
                int nCol = colCourante + dCol[k];


                if (nLig >= 0 && nLig < nblignes && nCol >= 0 && nCol < nbcolonnes)//vérifie si on est dans les limites du plateau
                {

                    if (!chemin.Contains((nLig, nCol)) && Matrice[nLig, nCol] == lettreCible) //vérifie si la case est déjà dans le chemin et si la lettre correspond à la cible
                    {
                      
                        if (!chemin.Contains((nLig, nCol)) &&             // La case n'est pas déjà visitée
                        char.IsLetter(Matrice[nLig, nCol]) &&        // La case contient une lettre
                         Matrice[nLig, nCol] == lettreCible)          // La lettre correspond à celle recherchée
                        {
                            chemin.Add((nLig, nCol));

                            if (Recherche_Recursive(mot, indexMot + 1, nLig, nCol, chemin))
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
        /// Met à jour le plateau après la suppression d'un mot trouvé.
        /// </summary>
        /// <param name="cheminMot"></param>
        public void Maj_Plateau(List<(int ligne, int colonne)> cheminMot)//permet de vider les cases du mot trouvé et de faire glisser les lettres des colonnes en question vers le bas
        {
            if (cheminMot == null || cheminMot.Count == 0)
                return;


            foreach (var coord in cheminMot) //Vide les cases du mot trouvé
            {
                Matrice[coord.ligne, coord.colonne] = ' '; // Marque la case comme vide
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
