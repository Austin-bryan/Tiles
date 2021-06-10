using UnityEngine.UI;
using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    public Text fpsText;
    float deltaTime;

    public void Update() => deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

    public void OnGUI()
    {
        int w      = Screen.width, h = Screen.height;
        var style  = new GUIStyle();
        var rect   = new Rect(0, 0, w, h * 2 / 100);
        float msec = Mathf.Ceil(deltaTime * 1000f);
        float fps  = Mathf.Ceil(1f / deltaTime);
        var text   = string.Format($"{msec} ms {fps} fps");

        style.alignment = TextAnchor.UpperCenter;
        style.fontSize  = h * 2 / 100;
        style.normal.textColor = new Color(0f, 0f, 0.5f, 1f);

        GUI.Label(rect, text, style);
    }
}
