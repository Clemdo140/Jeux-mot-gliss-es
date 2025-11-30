using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jeux_mot_glissées
{
    internal class Jeu
    {

        public Dictionnaire dictionnaire;
        public Plateau plateauCourant;
        private List<Joueur> joueurs;

        private TimeSpan DureePartieDefaut = TimeSpan.FromMinutes(2); // Durée totale par défaut de 2 minutes pour une partie
        private TimeSpan TempsParTourDefaut = TimeSpan.FromSeconds(30); // Délai par tour par défaut de 30 secondes pour une partie

        private TimeSpan DureePartie;
        private TimeSpan TempsParTour;
        private DateTime heureDebutPartie;

        public Dictionnaire Dico 
        { get; private set; }
        public Plateau PlateauCourant
        { get; private set; }
        public List<Joueur> Joueurs 
        { get; private set; }
        public Jeu()
        {
            
            Dico = new Dictionnaire();
            Joueurs = new List<Joueur>();
            DureePartie = DureePartieDefaut;
            TempsParTour = TempsParTourDefaut;
        }
        public void CreerJoueurs()
        {
            Console.WriteLine("\n--- Création des joueurs ---");
            string nomJ1;
            string nomJ2;

           
            do//on crée le joueur 1
            {
                Console.Write("Entrez le nom du Joueur 1 : ");
                nomJ1 = Console.ReadLine();
            } while (nomJ1 == null);

            do//on crée le joueur 2
            {
                Console.Write("Entrez le nom du Joueur 2 : ");
                nomJ2 = Console.ReadLine();
            } while (nomJ2==null || nomJ1.Equals(nomJ2, StringComparison.OrdinalIgnoreCase));// on vérifie que ce soit pas les mêmes

            Joueurs.Add(new Joueur(nomJ1));
            Joueurs.Add(new Joueur(nomJ2));
            Console.WriteLine($"Joueurs actuels : {nomJ1} et {nomJ2}");
        }

        public void ConfigurerTemps()
        {
            Console.WriteLine("\n--- Configuration du Temps (Laisser vide pour utiliser la valeur par défaut) ---");

            // Configurer la durée totale de la partie
            Console.Write($"Durée totale (en minutes, défaut {DureePartieDefaut.TotalMinutes} min) : ");//on précise la valeur par défaut qu'on avait prévu
            if (double.TryParse(Console.ReadLine(), out double minPartie) && minPartie > 0)// on vérifie que le nombres de minutes est une valeure cohérente
            {
                DureePartie = TimeSpan.FromMinutes(minPartie);
            }

            // Configurer le temps par tour
            Console.Write($"Temps max par tour (en secondes, défaut {TempsParTourDefaut.TotalSeconds} sec) : ");
            if (int.TryParse(Console.ReadLine(), out int secTour) && secTour > 0)
            {
                TempsParTour = TimeSpan.FromSeconds(secTour);
            }

            Console.WriteLine($"Voici les règles de temps : {DureePartie.TotalMinutes} min | Tour max : {TempsParTour.TotalSeconds} sec.");
        }
        public void DemarrerPartie(Plateau plateauInitial)
        {
            PlateauCourant = plateauInitial;
            this.heureDebutPartie = DateTime.Now;
            int indexJoueur = 0;

            Console.WriteLine("\n*** DÉBUT DE LA PARTIE ! ***");
            Console.WriteLine(PlateauCourant.toString());

            while ((DateTime.Now - heureDebutPartie) < DureePartie && PlateauCourant.Matrice.Cast<char>().Any(char.IsLetter))// La boucle continue tant que le temps n'est pas écoulé OU qu'il reste des lettres
            {
                Joueur joueurActuel = Joueurs[indexJoueur % Joueurs.Count];
                TimeSpan tempsRestantPartie = DureePartie - (DateTime.Now - this.heureDebutPartie);

                Console.WriteLine($"\n=========================================");
                Console.WriteLine($"TOUR DE JOUER : {joueurActuel.Nom} | Reste : {tempsRestantPartie.TotalSeconds:F0} sec.");
                Console.WriteLine("=========================================");

                if (JouerTour(joueurActuel))
                {
                    break; // Sort immédiatement de la boucle while de la partie
                }

                // Alternance du joueur
                indexJoueur++;
                Thread.Sleep(500); // Petite pause avant le tour suivant pour faciliter l'afffciahge, 500 millisecondes
            }

            AfficherResultatsFinaux();
        }
        private static string LireMotAvecTimeout(TimeSpan dureeMax)
        {
            DateTime debutLecture = DateTime.Now;
            StringBuilder motSaisi = new StringBuilder();

            // Boucle tant qu'on a du temps et que le mot n'est pas "entré"
            while ((DateTime.Now - debutLecture) < dureeMax)
            {
                // 1. Vérifier s'il y a une touche disponible (non-bloquant)
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true); // Lire la touche sans l'afficher immédiatement

                    if (key.Key == ConsoleKey.Enter)
                    {
                        // Entrée complète, afficher le mot tapé sur la console
                        Console.WriteLine();
                        return motSaisi.ToString();
                    }
                    else if (key.Key == ConsoleKey.Backspace && motSaisi.Length > 0)
                    {
                        // Gérer l'effacement
                        motSaisi.Remove(motSaisi.Length - 1, 1);
                        Console.Write("\b \b"); // Effacer un caractère de la console
                    }
                    else if (char.IsLetterOrDigit(key.KeyChar) || key.KeyChar == ' ')
                    {
                        // Ajouter le caractère au mot
                        motSaisi.Append(key.KeyChar);
                        Console.Write(key.KeyChar);

                    }
                }
                Thread.Sleep(50); // Mettre une petite pause pour éviter de monopoliser le CPU
            }

            // Le temps est écoulé sans appui sur Entrée
            Console.WriteLine(); // Aller à la ligne
            return null;
        }
        private bool JouerTour(Joueur joueur)
        {
            DateTime heureDebutTour = DateTime.Now;

            while ((DateTime.Now - heureDebutTour) < TempsParTour)
            {
                TimeSpan tempsRestantTour = TempsParTour;

                TimeSpan tempsRestantGlobal = DureePartie - (DateTime.Now - this.heureDebutPartie); // Calcul du temps restant global 

                Console.Write($"Saisissez votre mot ou (passer) --- Reste {tempsRestantGlobal.TotalSeconds:F1}s : ");

                string motSaisi = LireMotAvecTimeout(tempsRestantTour);
                if ((DateTime.Now - this.heureDebutPartie) >= DureePartie)
                {
                    Console.WriteLine("\nFIN DE PARTIE ! Le temps total est écoulé. Le mot saisi n'est pas pris en compte.");
                    return true; // Arrêt immédiat du tour
                }

                if (string.IsNullOrWhiteSpace(motSaisi) || motSaisi.Equals("passer", StringComparison.OrdinalIgnoreCase))// si rien n'es rentrer ou que le joueur a dit qu'il voulait passer
                {
                    Console.WriteLine("Passage au joueur suivant.");
                    return false;
                }

               
                if (joueur.Contient(motSaisi))//si le mot a déja été trouvé
                {
                    Console.WriteLine("Erreur : Ce mot a déjà été trouvé !");
                    continue;//le joueur peut saisir un nouveau mot
                }

                
                object chemin = PlateauCourant.Recherche_Mot(motSaisi);//on regarde si le mot est compatible avec le plateau

                if (chemin == null)
                {
                    Console.WriteLine("Erreur : Mot non compatible avec le plateau ou trop court");
                    continue;
                }

                if (!Dico.RechDichoRecursif(motSaisi))//si le mot n'est pas validé dans le dictionnaire
                {
                    Console.WriteLine("Erreur : Mot non trouvé dans le dictionnaire français.");
                    continue;
                }


                Console.WriteLine($"\n[MOT VALIDÉ : {motSaisi.ToUpper()}]");//Le mot a passé tous les tests

                int scoreObtenu = CalculerScore(motSaisi);// Calcul du score basé sur la longueur et le poids
                joueur.Add_Score(scoreObtenu);
                joueur.Add_Mot(motSaisi);

                PlateauCourant.Maj_Plateau((List<(int, int)>)chemin);// applique la mécanique de glissement des lettres et affiche le nouveau plateau

                Console.WriteLine("Plateau mis à jour :");
                Console.WriteLine(PlateauCourant.toString());
                return false;
            }

            Console.WriteLine("Temps imparti pour le tour écoulé.");//si il n'as pas trouvé de mot valide avat la fin du while
            return false;
        }

        private int CalculerScore(string mot)
        {


            int score = mot.Length * mot.Length; // Longueur du mot au carré pour calculer le poids du mot

            foreach (char c in mot.ToUpper())// Accès aux contraintes du plateau pour obtenir les poids
            {

                if (PlateauCourant.Lettrescontraintes.TryGetValue(c, out var contraintes))//on essaye de récupérer les contraintes pour chaque lettre dans Lettres.txt
                {
                    score += contraintes.poids;
                }
            }

            Console.WriteLine($"+ {score} points pour ce mot.");
            return score;//le score est la longueur au carré du mot plus la somme des poids des lettres
        }

        private void AfficherResultatsFinaux()
        {
            Console.WriteLine("\n\n*** FIN DE LA PARTIE ! ***");
            Console.WriteLine("--- SCORES FINAUX ---");

            var classement = Joueurs.OrderByDescending(j => j.Scoretot).ToList();//variable qui stock le classement des joueurs en fonction de leurs classements

            foreach (var joueur in classement)
            {
                Console.WriteLine(joueur.toString());//donne le recap de la performance et les infos du joueur
            }

            if (classement.Count > 0)//on vérifie qu'il y a au moins 1 joueur dans le classement avant de l'afficher
            {
                Console.WriteLine($"\n🏆 Vainqueur : {classement.First().Nom} avec {classement.First().Scoretot} points !");
            }
        }


    }
}
