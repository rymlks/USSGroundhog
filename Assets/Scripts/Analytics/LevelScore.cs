using System.Collections.Generic;
using Player.Death;

namespace Analytics
{
    public class LevelScore
    {
        public LevelScore(int par, float time, Dictionary<float, DeathCharacteristics> deaths, HashSet<string> gatheredItems)
        {
            parDeaths = par;
            elapsedTime = time;
            deathsByTime = new Dictionary<float, DeathCharacteristics>(deaths);
            performancePercentage = (par * 100f) / deaths.Count;
            performanceComments = generatePerformanceComments(performancePercentage);
            gatheredPermanentItems = gatheredItems;
        }

        public Dictionary<float, DeathCharacteristics> deathsByTime { get; }
        public int parDeaths { get; }
        public float elapsedTime { get; }
        public float performancePercentage { get; }
        
        public string performanceComments { get; }
        
        public HashSet<string> gatheredPermanentItems { get; }

        protected static string generatePerformanceComments(float percentage)
        {
            return percentage <= 25 ? "Opportunity for Improvement" :
                percentage <= 50 ? "Satisfactory" :
                percentage <= 75 ? "Model Employee" :
                percentage < 101 ? "Management Material" :
                "Bonjour, Monsieur Entrepreneur";
        }
    }
}