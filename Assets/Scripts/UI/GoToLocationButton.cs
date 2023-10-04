using System.Collections;
using System.Collections.Generic;
using Managers;
using StaticUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GoToLocationButton : MonoBehaviour
{
    public string locationName;
    public string levelName;
    public Color oneHundredPercentCompletionColor = Color.yellow;
    public Color minimumCompletionColor = Color.green;
    public Color availableColor = new Color(80, 207, 255);

    // Start is called before the first frame update
    void Start()
    {
        Color levelButtonColor = ProgressManager.instance.areAllOptionalObjectivesCompleted(levelName)
            ?
            oneHundredPercentCompletionColor
            : ProgressManager.instance.isLevelCompleted(levelName)
                ? minimumCompletionColor
                : availableColor;
        this.GetComponent<Button>().colors =
            UnityUtil.getBlockWithHighlightColorChanged(this.GetComponent<Button>().colors, levelButtonColor);
    }
}