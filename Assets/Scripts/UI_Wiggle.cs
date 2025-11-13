using UnityEngine;

public class UI_Wiggle : MonoBehaviour
{
    [Tooltip("Góc lắc tối đa (ví dụ: 15 độ)")]
    public float maxRotation = 15.0f;

    [Tooltip("Tốc độ lắc (số càng lớn lắc càng nhanh)")]
    public float speed = 2.0f;

    //chứa thông tin về vị trí, kích thước và góc xoay của UI
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        //tạo ra giá trị dao động từ -1 đến 1 dựa trên hàm sin của thời gian hiện tại nhân với tốc độ
        float oscillation = Mathf.Sin(Time.time * speed);
        //tính toán góc quay z dựa trên giá trị dao động và góc lắc tối đa
        float zRotation = oscillation * maxRotation;
        //cập nhật góc quay của RectTransform
        rectTransform.localEulerAngles = new Vector3(0, 0, zRotation);
    }
}
