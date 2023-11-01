#nullable enable
using System;
using System.Collections.Generic;
using Managers;
using Triggers;
using UnityEngine;
using System.Collections;

namespace Consequences
{
    public class PushConsequence : AbstractConsequence
    {
        public Vector3 pushStrength;

        protected List<GameObject> _objectsAlreadyPushedThisFrame;

        public void Start()
        {
            _objectsAlreadyPushedThisFrame = new List<GameObject>();
            //StartCoroutine(ObjectClearer());
        }
        
        public override void Execute(TriggerData? data)
        {
            if (_objectsAlreadyPushedThisFrame.Contains(data.triggeringObject))
            {
                return;
            }
            Rigidbody rb = data.triggeringObject.GetComponentInChildren<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(pushStrength);
                _objectsAlreadyPushedThisFrame.Add(data.triggeringObject);
            }
        }
        
        private IEnumerator ObjectClearer()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                _objectsAlreadyPushedThisFrame.Clear();
            }
        }

        public void FixedUpdate()
        {
            _objectsAlreadyPushedThisFrame.Clear();
        }
    }
}