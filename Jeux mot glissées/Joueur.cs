using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeux_mot_glissées
{
    internal class Joueur
    {
        string nom;
        List<string> motstrouvés;
        int scoretot;
        List<int> scoreplateau;

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
            private set {; }
        }
        public int Scoretot
        {
            get { return scoretot; }
            private set {; }
        }
        public List<int> Scoreplateau
        {
            get { return scoreplateau; }
            private set {; }

        }

        public void Add_Mot(string mot)
        {
            if (mot != null && mot.Length != 0 && !Contient(mot)) //on vérifie que le mot n'est pas vide et qu'il n'a pas déja été ajouté à la liste
            {

                Motstrouvés.Add(mot.ToUpper());//on met en majuscules pour éviter les erreurs
            }
        }

        public string toString()
        {
            return ($"nom : {this.nom} \nmotstrouvés : {this.motstrouvés} \nscoretot : {this.scoretot} \nscoreplateau : {this.scoreplateau}");

        }
        public void Add_Score(int val)
        {
            if (val > 0)
            {
                Scoretot += val; // Augmente le score du joueur qui vient de jouer
            }
        }

        public bool Contient(string mot)
        {
            return Motstrouvés.Contains(mot.ToUpper()); //on vérifie si le mot en paramètre est dans la liste des most trouvés, on met en majuscules pour éviter les erreurs
        }
    }
}