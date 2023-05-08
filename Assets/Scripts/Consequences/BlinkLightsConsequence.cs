#nullable enable
using System.Collections.Generic;
using System.Linq;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class BlinkLightsConsequence : MonoBehaviour, IConsequence
    {
        private class LightBlinkData
        {
            public float MaximumIntensity;
            public float CycleTimeSeconds;
            public bool IsDescending;
            public float TimeBlinkingBegun;
        
            public LightBlinkData(float maximumIntensity, float cycleTimeSeconds, bool isDescending, float timeBlinkingBegun)
            {
                MaximumIntensity = maximumIntensity;
                CycleTimeSeconds = cycleTimeSeconds;
                IsDescending = isDescending;
                TimeBlinkingBegun = timeBlinkingBegun;
            }

            public float TimeSinceCycleStart()
            {
                return Time.time - this.TimeBlinkingBegun;
            }
        }

        public bool includeChildren = true;
        public float minimumSecondsToOff = 2;
        public float maximumSecondsToOff = 2;
        private bool _enabled = false;
        private Dictionary<Light, LightBlinkData> _lights;
    

        public void Execute(TriggerData? data)
        {
            this._lights = new Dictionary<Light, LightBlinkData>();
            (includeChildren ? GetComponentsInChildren<Light>().ToList() : GetComponents<Light>().ToList()).ForEach(childLight =>
            {
                _lights.Add(childLight, new LightBlinkData(childLight.intensity, areMinAndMaxWithinIntensityTolerance() ? minimumSecondsToOff : minimumSecondsToOff +
                    Mathf.Abs(maximumSecondsToOff - minimumSecondsToOff) * (float)new System.Random().NextDouble(), true, Time.time));
            });
            this._enabled = true;
        }

        private bool areMinAndMaxWithinIntensityTolerance()
        {
            return withinIntensityTolerance(minimumSecondsToOff, maximumSecondsToOff);
        }
        
        private bool withinIntensityTolerance(float a, float b)
        {
            return Mathf.Abs(a - b) < 0.0001f;
        }

        void Update()
        {
            if (_enabled)
            {
                foreach (var entry in _lights)
                {
                    entry.Key.intensity = determineIntensity(entry.Value);
                    if (hasReachedGoal(entry))
                    {
                        entry.Value.IsDescending = !entry.Value.IsDescending;
                        entry.Value.TimeBlinkingBegun = Time.time;
                    }
                }
            }
        }

        private bool hasReachedGoal(KeyValuePair<Light, LightBlinkData> entry)
        {
            bool hasReachedOrExceededBrightnessGoal()
            {
                return (withinIntensityTolerance(entry.Key.intensity, entry.Value.MaximumIntensity) ||  entry.Key.intensity > entry.Value.MaximumIntensity) && !entry.Value.IsDescending;
            }
            bool hasReachedOrExceededDimnessGoal()
            {
                return (withinIntensityTolerance(entry.Key.intensity, 0) ||  entry.Key.intensity < 0) && entry.Value.IsDescending;
            }

            return hasReachedOrExceededDimnessGoal() || hasReachedOrExceededBrightnessGoal();
        }

        private float determineIntensity(LightBlinkData data)
        {
            return data.MaximumIntensity * (
                data.IsDescending ? 1 - data.TimeSinceCycleStart() / 
                    data.CycleTimeSeconds
                :
                    data.TimeSinceCycleStart() / 
               data.CycleTimeSeconds);
        }
    }
}
