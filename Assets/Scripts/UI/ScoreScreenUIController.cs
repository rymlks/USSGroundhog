using System;
using System.Collections.Generic;
using System.Linq;
using Analytics;
using Managers;
using Player.Death;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ScoreScreenUIController : TextUIController
    {
        protected override void EnableUI()
        {
            base.EnableUI();
            this.setUIValues(LevelScoreManager.instance.getLevelScore());
        }

        protected override void DisableUI()
        {
            base.DisableUI();
            SceneManager.LoadScene("Location Menu");
        }

        private void setUIValues(LevelScore levelScore)
        {
            this.textMesh.text = SceneManager.GetActiveScene().name +
                                 " satisfactorily completed!  Congratulations!\r\n" +
                                 "Minimum Viable Sacrifices (MVS) for the Good of the Company: " +
                                 levelScore.parDeaths + "\r\n" +
                                 "Your Sacrifices for the Good of the Company: " + levelScore.deathsByTime.Count +
                                 "\r\n" +
                                 "Company Time Spent: " + levelScore.elapsedTime + "\r\n\r\n" +
                                 "Overall Performance Rating: " + levelScore.performancePercentage + "%\r\n" +
                                 "Performance Comments: " + levelScore.performanceComments + "\r\n" +
                                 "Items Gathered: " + getItemsGatheredString(levelScore) +
                                 "Deaths in Detail: \r\n" + getDeathDetails(levelScore);

        }

        protected static string getDeathDetails(LevelScore levelScore)
        {
            Dictionary<string, int> deathCountsByCause = new Dictionary<string, int>();
            foreach (DeathCharacteristics deathCharacteristics in levelScore.deathsByTime.Values)
            {
                if (deathCountsByCause.ContainsKey(deathCharacteristics.getReason()))
                {
                    deathCountsByCause[deathCharacteristics.getReason()] =
                        deathCountsByCause[deathCharacteristics.getReason()] + 1;
                }
                else
                {
                    deathCountsByCause.Add(deathCharacteristics.getReason(), 1);
                }
            }

            return String.Concat(deathCountsByCause.Select(deathCount => deathCount.Value + "    " + deathCount.Key + "\r\n"));
        }

        protected static string getItemsGatheredString(LevelScore levelScore)
        {
            return String.Concat(levelScore.gatheredPermanentItems.Select(item => item + "\r\n"));
        }
    }
}
