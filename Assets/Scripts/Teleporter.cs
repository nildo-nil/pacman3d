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

            Invoke(nameof(ResetTeleportFlag), 0.5f);
        }
    }
    private void ResetTeleportFlag()
    {
        isTeleporting = false;
    }
}