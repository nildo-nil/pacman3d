using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform destinationPoint;
    private static bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            isTeleporting = true;
            CharacterController controller = other.GetComponent<CharacterController>();

            if (controller != null)
            {
                controller.enabled = false;
                other.transform.position = destinationPoint.position;
                controller.enabled = true;
            }
            else
            {
                other.transform.position = destinationPoint.position;
            }
        //giúp Player có đủ thời gian để bước ra khỏi vùng Trigger của cổng đích trước khi hệ thống cho phép dịch chuyển lần nữa
            Invoke(nameof(ResetTeleportFlag), 0.5f);
        }
    }

    //mở khóa hệ thống (false), cho phép lần dịch chuyển tiếp theo xảy ra
    private void ResetTeleportFlag()
    {
        isTeleporting = false;
    }
}