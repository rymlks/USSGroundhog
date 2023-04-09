using TMPro;
using UnityEngine;

public class KeyStatusUIController : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    private float lastAffectedTime = -1;
    public Color alertColor = Color.green;

    void Start()
    {
        if (textMesh == null)
        {
            this.textMesh = this.GetComponent<TextMeshProUGUI>();
        }
    }

    bool affectedNow()
    {
        float timeSinceLastAffected = Time.time - this.lastAffectedTime;
        return timeSinceLastAffected > 0 && timeSinceLastAffected < 1;
    }

    void LateUpdate()
    {
        if (this.affectedNow())
        {
            Debug.Log("Setting alertColor to " + alertColor);
            this.textMesh.color = alertColor;
        }
        else
        {
            if (this.textMesh)
                this.textMesh.color = Color.clear;
        }
    }

    public void showStatusNextFrame()
    {
        this.lastAffectedTime = Time.time;
    }
}