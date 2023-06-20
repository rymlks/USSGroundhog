#nullable enable
using Triggers;
using UnityEngine.SceneManagement;

namespace Consequences
{
    public class LoadSceneConsequence : AbstractConsequence
    {
        public string sceneName;
        public override void Execute(TriggerData? data)
        {
            if (sceneName != null)
            {
                SceneManager.LoadScene(this.sceneName);
            }
        }
    }
}
