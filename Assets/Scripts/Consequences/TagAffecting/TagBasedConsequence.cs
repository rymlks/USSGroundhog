namespace Consequences.TagAffecting
{
    public abstract class TagBasedConsequence : AbstractConsequence
    {
        public string TagToDetect = "";
        public string[] AdditionalTagsToDetect;
        public bool acceptTagInParent = false;

        void Start()
        {
            if (TagToDetect == "" && AdditionalTagsToDetect.Length == 0)
            {
                this.enabled = false;
            }
        }
    }
}