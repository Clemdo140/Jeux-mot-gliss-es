using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeux_mot_glissées
{
    internal class Jeu
    {

        private Dictionnaire dictionnaire;
        private Plateau plateauCourant;
        private List<Joueur> joueurs;

        // B. Paramètres de jeu (à configurer au lancement)
        private TimeSpan tempsParTour;  // Durée maximum allouée à chaque joueur par tour (TimeSpan)
        private TimeSpan tempsPartieTotal; // Durée totale de la partie (TimeSpan)

        // Chemin du fichier, déclaré comme constante
        private string CHEMIN_DICTIONNAIRE = "MotsFrancais.txt";

        public Jeu(TimeSpan tempsTour, TimeSpan tempsTotal)
        {
            // Initialisation des objets nécessaires (Contrôle Centralisé)

            // 1. Initialisation du dictionnaire (déjà discutée)
            // L'appel au constructeur lance la lecture du fichier et le tri.
            this.dictionnaire = new Dictionnaire(CHEMIN_DICTIONNAIRE);

            // 2. Initialisation de la collection de joueurs (List<T>)
            this.joueurs = new List<Joueur>();

            // 3. Initialisation des paramètres de temps
            this.tempsParTour = tempsTour;
            this.tempsPartieTotal = tempsTotal;

            // Note : Le plateau n'est PAS initialisé ici, car son type (aléatoire ou fichier)
            // est déterminé plus tard par le menu.
        }




    }
}
