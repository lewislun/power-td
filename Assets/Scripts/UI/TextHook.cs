using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextHook : MonoBehaviour {

    protected TextMeshProUGUI text;

    protected void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetFloat(float value) {
        text.text = value.ToString();
    }
}
