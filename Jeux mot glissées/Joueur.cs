using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeux_mot_glissées
{
    public class Joueur
    {
        private string nom;
        private List<string> motstrouvés;
        private int scoretot;
        private List<int> scoreplateau;

        public Joueur(string nom)
        {
            this.nom = nom;
            this.motstrouvés = new List<string>();
            this.scoretot = 0;
            this.scoreplateau = new List<int>();

        }
        public string Nom
        {
            get { return nom; }
        }
        public List<string> Motstrouvés
        {
            get { return motstrouvés; }
            private set { motstrouvés = value; }
        }
        public int Scoretot
        {
            get { return scoretot; }
            private set {scoretot=value; }
        }
        public List<int> Scoreplateau
        {
            get { return scoreplateau; }
            private set {scoreplateau=value; }

        }
        /// <summary>
        /// Ajoute un mot à la liste des mots trouvés par le joueur s'il n'y est pas déjà
        /// </summary>
        /// <param name="mot"></param>
        public void Add_Mot(string mot)
        {
            if (mot != null && mot.Length != 0 && !Contient(mot)) //on vérifie que le mot n'est pas vide et qu'il n'a pas déja été ajouté à la liste
            {

                Motstrouvés.Add(mot.ToUpper());//on met en majuscules pour éviter les erreurs
            }
        }
        /// <summary>
        /// Affiche les informations du joueur
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            string mots = AfficherListe(motstrouvés);//on utilise une fonction spécifique pour afficher les listes
            string scores = AfficherListe(scoreplateau);
            return ($"Nom : {Nom}\nScore total : {Scoretot}\nMots trouvés ({Motstrouvés.Count}) : {mots}\nScores par plateau : {scores}");

        }
        /// <summary>
        /// Affiche une liste générique sous forme de chaîne de caractères
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="liste"></param>
        /// <returns></returns>
        private string AfficherListe<T>(List<T> liste)//private car utilisée que dans cette classe
        {
            if (liste == null || liste.Count == 0)
            {
                return "Aucun";
            }
            string resultat = ""; 

            
            for (int i = 0; i < liste.Count; i++)//on parcours la liste
            {
                resultat += liste[i].ToString(); // Concaténation de l'élément

                
                if (i < liste.Count - 1)
                {
                    resultat += ", "; // si ce n'est pas le dernier élément, on met virgule espace
                }
            }

            return resultat;
        }
        /// <summary>
        /// Ajoute un score au score total et à l'historique des scores par plateau
        /// </summary>
        /// <param name="val"></param>
        public void Add_Score(int val)
        {
            if (val > 0)
            {
                Scoretot += val; // Augmente le score du joueur qui vient de jouer
                scoreplateau.Add(val); // Ajout à l'historique par plateau
            }
        }
        /// <summary>
        /// Vérifie si un mot a déjà été trouvé par le joueur
        /// </summary>
        /// <param name="mot"></param>
        /// <returns></returns>
        public bool Contient(string mot)
        {
            return Motstrouvés.Contains(mot.ToUpper()); //on vérifie si le mot en paramètre est dans la liste des most trouvés, on met en majuscules pour éviter les erreurs
        }
    }
}