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

        protected virtual bool tagIsRelevant(GameObject other, string tagInQuestion)
        {
            return (tagInQuestion == "" ||
                    other.CompareTag(tagInQuestion)) ||
                   (this.acceptTagInParent || alwaysAcceptInParent.Contains(tagInQuestion)) &&
                   TagAppearsInParent(other.gameObject, tagInQuestion);
        }
        protected virtual bool tagIsRelevant(Transform other, string tagInQuestion)
        {
            return (tagInQuestion == "" ||
                    other.CompareTag(tagInQuestion)) ||
                   (this.acceptTagInParent || alwaysAcceptInParent.Contains(tagInQuestion)) &&
                   TagAppearsInParent(other.gameObject, tagInQuestion);
        }

        protected virtual bool transformIsRelevant(Transform other)
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

        protected virtual bool isRelevantObject(GameObject other)
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