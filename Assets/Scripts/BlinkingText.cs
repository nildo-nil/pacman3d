using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BlinkingText : MonoBehaviour
{
    public float speed = 3f; 
    private TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float alpha = (Mathf.Sin(Time.time * speed) + 1) / 2;
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, alpha);
    }
}
