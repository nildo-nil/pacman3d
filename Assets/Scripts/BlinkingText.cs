using UnityEngine;
using TMPro;

//đảm bảo rằng GameObject nào được gắn script này bắt buộc phải có component TextMeshProUGUI
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
        //Độ trong suốt (Alpha) chỉ nhận giá trị từ 0 đến 1 (0 là tàng hình, 1 là rõ nhất).
        float alpha = (Mathf.Sin(Time.time * speed) + 1) / 2;
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, alpha);
    }
}
