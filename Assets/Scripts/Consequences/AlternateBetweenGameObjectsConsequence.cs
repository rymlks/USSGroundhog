using System.Collections.Generic;
using System.Linq;
using StaticUtils;
using UnityEngine;

namespace Consequences
{
    public class AlternateBetweenGameObjectsConsequence : AbstractInterruptibleConsequence
    {
        public float alternationSeconds;
        protected float elapsedSinceLastAlternation = 0f;
        public GameObject[] toAlternateBetween;
        protected List<GameObject> _toAlternateBetween;
        protected int _index = 0;

        void Start()
        {
            if (this.toAlternateBetween == null || this.toAlternateBetween.Length < 1)
            {
                _toAlternateBetween = UnityUtil.GetImmediateChildGameObjects(this.gameObject);
            }
            else
            {
                _toAlternateBetween = toAlternateBetween.ToList();
            }
        }

        void Update()
        {
            if (this.started)
            {
                if (this.elapsedSinceLastAlternation >= this.alternationSeconds)
                {
                    this.alternate(_index + 1);
                }
                this.elapsedSinceLastAlternation += Time.deltaTime;
            }
        }

        protected void alternate(int toIndex)
        {
            if (toIndex >= this._toAlternateBetween.Count)
            {
                toIndex = 0;
            }
            for (int i = 0; i < _toAlternateBetween.Count; i++)
            {
                _toAlternateBetween[i].SetActive(i == toIndex);
            }

            _index = toIndex;
            this.elapsedSinceLastAlternation -= this.alternationSeconds;
        } 
    }
}
