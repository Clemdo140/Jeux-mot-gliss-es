using Jeux_mot_glissées;
namespace Motsglissés.Tests
{
    [TestClass]
    public class DicoTest
    {
        private Dictionnaire dico;
        [TestInitialize]
        public void Setup() //evite de recréer un dico a chaque fois
        {
         
            dico = new Dictionnaire();
        }
        [TestMethod]
        public void Test1_Tri_QuickSort_Est_Trie_Correctement()
        {
            // On choisit une sous-liste à vérifier (ici, on prend les mots en 'C', index 2)
            int indexC = 'C' - 'A';
            List<string> motsC = dico.Motsparlettre[indexC];
            Assert.IsTrue(motsC.Count > 10, "La sous-liste 'C' est trop petite pour une vérification significative.");//on vérifie qu'il y a assez de mots à tester

            for (int i = 0; i < motsC.Count - 1; i++)
            {
                int comparaison = string.Compare(motsC[i], motsC[i + 1], System.StringComparison.OrdinalIgnoreCase);

                Assert.IsTrue(comparaison <= 0,
                    $"La liste n'est pas triée correctement. Erreur de séquence : '{motsC[i]}' vient après '{motsC[i + 1]}'");
            }
        }
        [TestMethod]
        public void Test2_RechDichoRecursif_Trouve_Mots_Valides()
        {
            // on définit des mots à tester
            string motValide1 = "JEU";
            string motValide2 = "DICTIONNAIRE";
            string motInvalide = "XYZ_JAMAISTROUVE_XYZ";

            // Test 2.1 : Trouve un mot valide
            Assert.IsTrue(dico.RechDichoRecursif(motValide1), $"Devrait trouver le mot '{motValide1}' dans le dictionnaire.");
            Assert.IsTrue(dico.RechDichoRecursif(motValide2), $"Devrait trouver le mot '{motValide2}' dans le dictionnaire.");

            // Test 2.2 : Ne trouve pas un mot invalide
            Assert.IsFalse(dico.RechDichoRecursif(motInvalide), $"Ne devrait PAS trouver le mot '{motInvalide}'.");

            // Test 2.3 : Teste la recherche avec une casse différente
            Assert.IsTrue(dico.RechDichoRecursif("jeu"), "Devrait trouver le mot même s'il est en minuscules.");
        }
    }
}