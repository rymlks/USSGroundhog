using System;
using System.Collections.Generic;
using System.Linq;
using Analytics;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ScoreScreenUIController : TextUIController
    {
        protected override void EnableUI()
        {
            base.EnableUI();
            this.setUIValues(ScoreManager.instance.getLevelScore());
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
                                 "Items Gathered: " + getItemsGatheredString(levelScore);

        }

        protected static string getItemsGatheredString(LevelScore levelScore)
        {
            return String.Concat(levelScore.gatheredPermanentItems.Select(item => item + "\r\n"));
        }
    }
}
