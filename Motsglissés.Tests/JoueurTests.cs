using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jeux_mot_glissées;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public class JoueurTests
{

    [TestMethod]
    /// <summary>
    ///  Teste l'ajout de mots et de scores dans la classe Joueur, y compris la gestion des doublons et la mise à jour correcte du score total
    /// <summary>
    public void Test5_Joueur_Gestion_Mot_Et_Score()
    {
        Joueur joueur = new Joueur("Testeur");

        //Ajouter un mot et un score au hazard
        joueur.Add_Mot("MAISON");
        joueur.Add_Score(15); 

        //Vérifier l'ajout
        Assert.AreEqual(1, joueur.Motstrouvés.Count, "Le nombre de mots trouvés doit être 1.");
        Assert.AreEqual("MAISON", joueur.Motstrouvés.First(), "Le mot n'a pas été ajouté ou mis en majuscule.");
        Assert.AreEqual(15, joueur.Scoretot, "Le score total doit être 15.");
        Assert.AreEqual(1, joueur.Scoreplateau.Count, "L'historique des scores doit avoir 1 entrée.");

        //Tenter d'ajouter le même mot (casse différente)
        joueur.Add_Mot("maison");

        //Vérifier la détection du doublon
        Assert.AreEqual(1, joueur.Motstrouvés.Count, "Le mot ne doit pas être ajouté une seconde fois (doublon).");
        Assert.IsTrue(joueur.Contient("maison"), "La vérification 'Contient' doit ignorer la casse.");

        //Ajouter un autre mot et score
        joueur.Add_Mot("VOITURE");
        joueur.Add_Score(25);

        //Vérifier l'incrémentation
        Assert.AreEqual(2, joueur.Motstrouvés.Count, "Le nombre de mots trouvés doit être 2.");
        Assert.AreEqual(40, joueur.Scoretot, "Le score total doit être 15 + 25 = 40.");
    }

}