using System.Collections.Generic;
using System.Linq;
using StaticUtils;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using static StaticUtils.UnityUtil;

namespace Triggers
{
    public abstract class TagBasedTrigger : AbstractTrigger
    {
        public string CustomTag = "";
        public string[] AdditionalTagsToDetect;
        public bool acceptTagInParent = false;
        public string[] alwaysAcceptInParent = {"Player", "Corpse"};

        protected bool tagIsRelevant(Collider other, string tagInQuestion)
        {
            return (tagInQuestion == "" ||
                    other.CompareTag(tagInQuestion)) ||
                   (this.acceptTagInParent || alwaysAcceptInParent.Contains(tagInQuestion)) &&
                   TagAppearsInParent(other.gameObject, tagInQuestion);
        }

        protected bool intersectingRelevantObject(Collider other)
        {
            List<string> allTags = this.AdditionalTagsToDetect.ToList();
            allTags.Add(CustomTag);
            foreach (var relevantTag in allTags)
            {
                if (tagIsRelevant(other, relevantTag))
                {
                    return true;
                }
            }
            return false;
        }
    }
}