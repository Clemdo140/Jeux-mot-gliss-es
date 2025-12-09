using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Jeux_mot_glissées;
using System.Security.Cryptography;

namespace Jeux_mot_glissées
{
    internal class Program
    {
        static void Main(string[] args)
        {
         
            Console.OutputEncoding = Encoding.UTF8; // Pour afficher les caractères spéciaux (le 🏆 dans la classe jeu)
            Console.Title = "Jeu des MOTS GLISSÉS ";//renommer la console
            Console.WriteLine("\n=============================");
            Console.Write("=== ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("JEU DES MOTS GLISSÉS");
            Console.ResetColor();
            Console.WriteLine(" ===");
            Console.WriteLine("=============================");

            Jeu jeu = new Jeu(); //Initialisation des composants centraux du jeu (Jeu, Dictionnaire, Joueurs, Temps)
            jeu.CreerJoueurs();
            jeu.ConfigurerTemps();

            int lignes = 8;//par défaut
            int colonnes = 8;

            bool sortir = false;

            
            while (!sortir) //La boucle se répète tant que l'utilisateur n'a pas choisi l'option Sortir-> énoncé
            {
                AfficherMenu();

                // Saisie sécurisée : on attend une option valide (1, 2, ou 3)
                if (!int.TryParse(Console.ReadLine(), out int choix) || choix < 1 || choix > 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Choix invalide :  Veuillez entrer 1, 2 ou 3.");
                    Console.ResetColor();
                    continue; // Recommence la boucle du menu
                }

                Plateau plateauACharger = null;
                const int DIMENSION_MIN = 5;
                const int DIMENSION_MAX = 26;
                switch (choix)
                {
                    case 1:   // Option 1 : Jouer à partir d'un fichier

                        
                        Console.Write("\nEntrez le nom du fichier du plateau (ex: Test1.csv) : ");
                        string nomFichier = Console.ReadLine()?.Trim();

                       // if (string.IsNullOrWhiteSpace(nomFichier))
                       // {
                        //    Console.WriteLine("Nom de fichier vide. Retour au menu.");
                         //   continue;
                        //}


                        plateauACharger = new Plateau(nomFichier); // Le constructeur Plateau gère l'appel à ToRead et le try/catch de FileNotFound

                        
                        if (plateauACharger.Matrice != null && plateauACharger.NbLignes > 0)// On vérifie que le plateau est bien chargé (sinon il est régénéré aléatoirement)
                        {
                            jeu.DemarrerPartie(plateauACharger);
                        }
                        break;

                    case 2:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n--- Définition de la taille du Plateau aléatoire ---");
                        Console.ResetColor();
                        lignes = SaisirDimension("lignes", 8, DIMENSION_MIN, DIMENSION_MAX);//8 par défaut, et on fixe 5 en min et 26 en max
                        colonnes = SaisirDimension("colonnes", 8, DIMENSION_MIN, DIMENSION_MAX);

                        Console.WriteLine("\nLancement de la génération d'un plateau aléatoire...");// Option 2 : Jouer à partir d'un plateau généré aléatoirement

                        plateauACharger = new Plateau(lignes, colonnes);// Le constructeur Plateau() principal gère la génération avec lignes et colonnes choisies par l'utilisateur
                        jeu.DemarrerPartie(plateauACharger);
                        break;

                    case 3:
   
                        sortir = true;// Option 3 : Sortir
                        break;
                }
            }

            Console.WriteLine("\nMerci d'avoir joué au jeu des MOTS GLISSÉS. Au revoir !");
            Console.ReadKey();
        }

        static void AfficherMenu()
        {
            Console.WriteLine("\n==============================");
            Console.Write("=== ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("MENU DES MOTS GLISSÉS");
            Console.ResetColor();
            Console.WriteLine(" ===");
            Console.WriteLine("==============================");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("1. ");
            Console.ResetColor();
            Console.WriteLine("Jouer à partir d'un fichier");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("2. ");
            Console.ResetColor();
            Console.WriteLine("Jouer à partir d'un plateau généré aléatoirement");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("3. ");
            Console.ResetColor();
            Console.WriteLine("Sortir");
            Console.Write("Votre choix : ");
        }

        static int SaisirDimension(string nomDimension, int defaut, int min, int max)
        {
            int valeur = defaut;//8 ici
            bool saisieValide = false;

            while (!saisieValide)
            {
                Console.Write($"Entrez le nombre de {nomDimension} (Défaut {defaut}, Min {min}, Max {max}) : ");
                string input = Console.ReadLine();

                
                if (string.IsNullOrWhiteSpace(input))// Valeur par défaut
                {
                    
                    saisieValide = true;
                }
              
                else if (int.TryParse(input, out valeur) && valeur >= min && valeur <= max) //Validation du nombre et des bornes
                {
                    saisieValide = true;
                }
                
                else// erreur
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"❌ Erreur: Veuillez entrer un nombre entier entre {min} et {max}");
                    Console.ResetColor();
                }
            }
            return valeur;
        }

        

    }

    }
    

