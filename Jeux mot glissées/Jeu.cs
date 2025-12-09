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
        { get; set; }
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
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n--- Création des joueurs ---");
            Console.ResetColor();

            int nbJoueurs = 0;

            // On demande combien de joueurs vont participer tant que le nombre de joueurs rentré est inférieur à 2
            while (nbJoueurs < 2)
            {
                Console.Write("Combien de joueurs participent (minimum 2) ? ");
                string entree = Console.ReadLine();

                // On vérifie que c'est bien un nombre et qu'il est supérieur ou égal à 2
                if (!int.TryParse(entree, out nbJoueurs) || nbJoueurs < 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ : Veuillez entrer un nombre entier valide supérieur ou égal à 2.");
                    Console.ResetColor();
                }
            }

            // Boucle qui sert à créer chaque joueur
            for (int i = 0; i < nbJoueurs; i++)
            {
                string nom = null;
                bool nomValide = false;

                while (nomValide == false)
                {
                    // Note : J'ai mis des parenthèses autour de (i + 1) pour faire l'addition correctement
                    Console.Write("Entrez le nom du Joueur " + (i + 1) + " : ");
                    nom = Console.ReadLine();

                    // 1. Vérification que le nom ne soit pas vide
                    if (string.IsNullOrWhiteSpace(nom))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Le nom ne peut pas être vide");
                        Console.ResetColor();
                        continue; // On recommence la boucle while immédiatement
                    }

                    // 2. Vérification des doublons 
                    bool existeDeja = false;

                    // On parcourt la liste des joueurs déjà créés
                    foreach (Joueur j in Joueurs)
                    {
                        // Si le nom existe déjà (en ignorant les majuscules/minuscules)
                        if (j.Nom.ToUpper() == nom.ToUpper())
                        {
                            existeDeja = true;
                            break; // On arrête de chercher, on a trouvé un doublon
                        }
                    }

                    if (existeDeja)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Ce nom est déjà pris. Veuillez en choisir un autre.");
                        Console.ResetColor();
                        continue; // On recommence la boucle while immédiatement
                    }

                    // Si on arrive ici, le nom est valide
                    nomValide = true;
                }

                // Ajout du joueur à la liste
                Joueurs.Add(new Joueur(nom));
            }

            // Affichage des joueurs créés via une boucle foreach
            Console.WriteLine("\nLa partie est bien configurée avec " + nbJoueurs + " joueurs :");
            foreach (Joueur joueur in Joueurs)
            {
                Console.WriteLine("-> " + joueur.Nom);
            }
        }


        public void ConfigurerTemps()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n--- Configuration du Temps (Laisser vide pour utiliser la valeur par défaut) ---");
            Console.ResetColor();

            // --- Configuration de la Durée Totale de la Partie (Minutes) ---

            double minPartie = 0;
            bool saisieValideMin = false;

            while (!saisieValideMin)
            {
                Console.Write($"Durée totale (en minutes, défaut {DureePartieDefaut.TotalMinutes} min) : ");
                string input = Console.ReadLine();

                
                if (string.IsNullOrWhiteSpace(input))// si la saisie est vide
                {
                    minPartie = DureePartieDefaut.TotalMinutes;
                    saisieValideMin = true;
                }
               
                else if (double.TryParse(input, out minPartie) && minPartie > 0) //on essaye de lire et on test si >0
                {
                    saisieValideMin = true;
                }
                
                else// saisi invalide
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Erreur: Veuillez entrer un nombre positif valide (doit être supérieur à 0).");
                    Console.ResetColor();
                }
            }

            // Affectation directe, car minPartie est GARANTI d'être > 0 ici.
            DureePartie = TimeSpan.FromMinutes(minPartie);

            // --- Configuration du Temps par Tour (Secondes) ---

            int secTour = 0;
            bool saisieValideSec = false;

            while (!saisieValideSec)
            {
                Console.Write($"Temps max par tour (en secondes, défaut {TempsParTourDefaut.TotalSeconds} sec) : ");
                string input = Console.ReadLine();

               
                if (string.IsNullOrWhiteSpace(input))//saisi vide
                {
                    secTour = (int)TempsParTourDefaut.TotalSeconds;
                    saisieValideSec = true;
                }
                
                else if (int.TryParse(input, out secTour) && secTour > 0) //Tentative de lecture
                {
                    saisieValideSec = true;
                }
                
                else//saisi invalide
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Erreur: Veuillez entrer un nombre entier positif valide (doit être supérieur à 0).");
                    Console.ResetColor();
                }
            }

            // Affectation directe, car secTour est GARANTI d'être > 0 ici.
            TempsParTour = TimeSpan.FromSeconds(secTour);

            Console.WriteLine($"\nVoici les règles de temps : {DureePartie.TotalMinutes} min | Tour max : {TempsParTour.TotalSeconds} sec.");
        }
        public void DemarrerPartie(Plateau plateauInitial)
        {
            PlateauCourant = plateauInitial;
            this.heureDebutPartie = DateTime.Now;
            int indexJoueur = 0;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n*** DÉBUT DE LA PARTIE ! ***");
            Console.ResetColor();
            Console.WriteLine("\n");
            Console.WriteLine(PlateauCourant.toString());

            while ((DateTime.Now - heureDebutPartie) < DureePartie && PlateauCourant.Matrice.Cast<char>().Any(char.IsLetter))// La boucle continue tant que le temps n'est pas écoulé OU qu'il reste des lettres
            {
                Joueur joueurActuel = Joueurs[indexJoueur % Joueurs.Count];
                TimeSpan tempsRestantPartie = DureePartie - (DateTime.Now - this.heureDebutPartie);

                Console.WriteLine($"\n===========================================================");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"TOUR DU JOUEUR : {joueurActuel.Nom} | Reste : {tempsRestantPartie.Minutes:D2} min et {tempsRestantPartie.Seconds:D2} sec : ");
                Console.ResetColor();
                Console.WriteLine("===========================================================");

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
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"Saisissez votre mot ou (passer) --- Reste {tempsRestantTour.Minutes:D2}min et {tempsRestantTour.Seconds:D2}sec : ");
                Console.ResetColor();

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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\n*** FIN DE LA PARTIE ! ***");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("--- SCORES FINAUX ---");
            Console.ResetColor();   

            var classement = Joueurs.OrderByDescending(j => j.Scoretot).ToList();//variable qui stock le classement des joueurs en fonction de leurs classements

            foreach (var joueur in classement)
            {
                Console.WriteLine(joueur.toString());//donne le recap de la performance et les infos du joueur
                Console.WriteLine();
            }

            if (classement.Count > 0)//on vérifie qu'il y a au moins 1 joueur dans le classement avant de l'afficher
            {
                Console.WriteLine($"\n🏆 Vainqueur : {classement.First().Nom} avec {classement.First().Scoretot} points !");
            }
        }


    }
}
