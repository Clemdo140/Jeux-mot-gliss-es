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
        }
        public int Scoretot
        {
            get { return scoretot; }
        }
        public List<int> Scoreplateau
        {
            get { return scoreplateau; }
        }

        public void Add_Mot(string mot)
        {

        }

        public string toString()
        {
            return ($"nom : {this.nom} \nmotstrouvés : {this.motstrouvés} \nscoretot : {this.scoretot} \nscoreplateau : {this.scoreplateau}");
        
       }
        public void Add_Score(int val)
        {

        }

        public bool Contient(string mot)
        {
            return false;
        }
    }
}
