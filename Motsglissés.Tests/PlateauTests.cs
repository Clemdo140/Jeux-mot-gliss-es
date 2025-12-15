using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jeux_mot_glissées;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public class PlateauTests
{

    [TestMethod]
    /// <summary>
    /// Teste la recherche de mots sur le plateau, en s'assurant que les chemins valides sont correctement identifiés et que les mots invalides ne sont pas trouvés
    ///   /// <summary>
    public void Test3_Recherche_Mot_Trouve_Chemin_Valide_Depuis_Base()
    {
        //Créer un plateau 3x3
        var plateau = new Plateau(3, 3);
        plateau.Matrice[0, 1] = 'A';
        plateau.Matrice[1, 1] = 'B';
        plateau.Matrice[2, 2] = 'C'; 
        plateau.Matrice[2, 1] = 'X'; 

 
        List<(int, int)> cheminValide = (List<(int, int)>)plateau.Recherche_Mot("CBA");

        Assert.IsNotNull(cheminValide, "Le chemin valide 'CBA' n'a pas été trouvé.");
        Assert.AreEqual(3, cheminValide.Count, "Le chemin trouvé devrait contenir 3 lettres.");

        // Vérifier que le chemin commence bien par la base (2, 2)
        Assert.AreEqual((2, 2), cheminValide[0], "Le chemin doit commencer par le C à la base.");

        // ACT : Rechercher un mot qui ne commence pas par la base
        List<(int, int)> cheminInvalide = (List<(int, int)>)plateau.Recherche_Mot("AB");

        // ASSERT
        Assert.IsNull(cheminInvalide, "Un mot ne démarrant pas à la base ne devrait pas être trouvé.");
    }

    // 4. Test du Glissement du Plateau (Maj_Plateau)
    [TestMethod]
    /// <summary>
    /// Teste la mise à jour du plateau après la suppression d'un mot, en vérifiant que les lettres au-dessus glissent correctement vers le bas pour remplir les espaces vides
    /// <summary>
    public void Test4_Maj_Plateau_Glissement_Vertical_Correct()
    {
        // Créer un plateau 4x1 pour tester le glissement vertical
        var plateau = new Plateau(4, 1);
        plateau.Matrice[0, 0] = 'A';
        plateau.Matrice[1, 0] = 'B';
        plateau.Matrice[2, 0] = 'C';
        plateau.Matrice[3, 0] = 'D'; // Base

        // Définir un chemin à supprimer (ex: "CD")
        List<(int, int)> cheminMot = new List<(int, int)> { (2, 0), (3, 0) }; // C et D

        //Mettre à jour le plateau
        plateau.Maj_Plateau(cheminMot);

        // Vérifier le résultat

        // B (ancienne pos 1) doit glisser en pos 3.
        Assert.AreEqual('B', plateau.Matrice[3, 0], "La lettre 'B' aurait dû glisser en position (3, 0).");
        // A (ancienne pos 0) doit glisser en pos 2.
        Assert.AreEqual('A', plateau.Matrice[2, 0], "La lettre 'A' aurait dû glisser en position (2, 0).");

        // Les deux cases du haut doivent être vides
        Assert.AreEqual(' ', plateau.Matrice[1, 0], "La case 1 doit être vide après glissement.");
        Assert.AreEqual(' ', plateau.Matrice[0, 0], "La case 0 doit être vide après glissement.");
    }

    
}