using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Jeux_mot_glissées;

namespace Jeux_mot_glissées
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; // Pour afficher les caractères spéciaux (le 🏆 dans la classe jeu)
            Console.Title = "Jeu des MOTS GLISSÉS - ESILV";//renommer la console

           
            Jeu jeu = new Jeu(); //Initialisation des composants centraux du jeu (Jeu, Dictionnaire, Joueurs, Temps)
            jeu.CreerJoueurs();
            jeu.ConfigurerTemps();

            bool sortir = false;

            
            while (!sortir) //La boucle se répète tant que l'utilisateur n'a pas choisi l'option Sortir-> énoncé
            {
                AfficherMenu();

                // Saisie sécurisée : on attend une option valide (1, 2, ou 3)
                if (!int.TryParse(Console.ReadLine(), out int choix) || choix < 1 || choix > 3)
                {
                    Console.WriteLine("❌ Choix invalide. Veuillez entrer 1, 2 ou 3.");
                    continue; // Recommence la boucle du menu
                }

                Plateau plateauACharger = null;

                switch (choix)
                {
                    case 1:   // Option 1 : Jouer à partir d'un fichier

                        Console.Write("\nEntrez le nom du fichier du plateau (ex: Test1.csv) : ");
                        string nomFichier = Console.ReadLine()?.Trim();

                        if (string.IsNullOrWhiteSpace(nomFichier))
                        {
                            Console.WriteLine("Nom de fichier vide. Retour au menu.");
                            continue;
                        }

                       
                        plateauACharger = new Plateau(nomFichier); // Le constructeur Plateau gère l'appel à ToRead et le try/catch de FileNotFound

                        
                        if (plateauACharger.Matrice != null)// On vérifie que le plateau est bien chargé (sinon il est régénéré aléatoirement)
                        {
                            jeu.DemarrerPartie(plateauACharger);
                        }
                        break;

                    case 2:
                 
                        Console.WriteLine("\nLancement de la génération d'un plateau aléatoire...");// Option 2 : Jouer à partir d'un plateau généré aléatoirement

                        plateauACharger = new Plateau();// Le constructeur Plateau() par défaut gère la génération
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
            Console.WriteLine("\n=================================");
            Console.WriteLine("=== MENU DES MOTS GLISSÉS ===");
            Console.WriteLine("=================================");
            Console.WriteLine("1. Jouer à partir d'un fichier");
            Console.WriteLine("2. Jouer à partir d'un plateau généré aléatoirement");
            Console.WriteLine("3. Sortir");
            Console.Write("Votre choix : ");
        }
       
    }

    }
    

