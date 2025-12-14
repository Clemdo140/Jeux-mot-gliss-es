using Jeux_mot_glissées;
namespace Motsglissés.Tests
{
    [TestClass]
    public class DicoTest
    {
        private Dictionnaire dico;
        [TestInitialize]
        public void Setup() 
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
    }
}