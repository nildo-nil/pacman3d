using UnityEngine;

public class UI_Wiggle : MonoBehaviour
{
    [Tooltip("Góc lắc tối đa (ví dụ: 15 độ)")]
    public float maxRotation = 15.0f;

    [Tooltip("Tốc độ lắc (số càng lớn lắc càng nhanh)")]
    public float speed = 2.0f;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        float oscillation = Mathf.Sin(Time.time * speed);
        float zRotation = oscillation * maxRotation;
        rectTransform.localEulerAngles = new Vector3(0, 0, zRotation);
    }
}
