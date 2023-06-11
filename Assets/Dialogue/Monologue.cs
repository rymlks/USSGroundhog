using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Dialogue
{
    public class Monologue : MonoBehaviour
    {
        public string monologueName;
        public float timeToDisplayEachSentence = 8f;
        public TMP_Text monologueText;
        private List<string> statements = null;
        private float startTime = float.PositiveInfinity;

        void Start()
        {
            readFromJson(monologueName);
            if (statements == null || statements == null || statements.Count < 1)
            {
                Destroy(this);
            }
            else
            {
                BeginMonologue();
            }
        }

        void Update()
        {
            if (Time.time < startTime)
            {
                return;
            }
            else if (Time.time < startTime + (this.statements.Count * this.timeToDisplayEachSentence))
            {
                displayMonologue();
            }
            else
            {
                hideMonologue();
            }
        }

        private void hideMonologue()
        {
            this.monologueText.enabled = false;
        }

        private void BeginMonologue()
        {
            this.startTime = Time.time;
            this.monologueText.enabled = true;
        }
        
        private void displayMonologue()
        {
            string toDisplay = selectStatement();
            this.monologueText.text = toDisplay;
        }

        private string selectStatement()
        {
            int statementIndex = (int)((Time.time - this.startTime) / this.timeToDisplayEachSentence);
            return statements[statementIndex];
        }

        protected void readFromJson(string dialogueFileName)
        {
            MonologueStatements statementsObject = JsonUtility.FromJson<MonologueStatements>(getJson());
            if (statementsObject == null)
            {
                Debug.Log("Failed to load monologue text for: " + this.monologueName);
                Destroy(this.gameObject);
            }
            else
            {
                this.statements = statementsObject.statements.ToList();
            }
        }

        private string getJson()
        {
            return ((TextAsset)(Resources.Load<TextAsset>(this.monologueName))).text;
        }
    }
}
