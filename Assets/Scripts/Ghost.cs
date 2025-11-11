using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Ghost : MonoBehaviour
{
    [Header("=== SETUP ===")]
    public Transform player;
    public GhostType ghostType = GhostType.Red;

    [Header("=== SETTINGS ===")]
    public float updateRate = 0.3f;
    public float flankDistance = 3.5f;
    public float predictTime = 1.5f;

    private NavMeshAgent navMeshAgent;

    public enum GhostType { Red, Pink, Blue, Orange }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // ✅ KIỂM TRA AN TOÀN
        if (navMeshAgent == null)
        {
            Debug.LogError($"❌ THIẾU NavMeshAgent trên {gameObject.name}!", this);
            return;
        }
        if (player == null)
        {
            Debug.LogError($"❌ THIẾU PLAYER trên {gameObject.name}!", this);
            return;
        }

        // ✅ SETTINGS TỐI ƯU - BẬT LẠI DEFAULT
        navMeshAgent.avoidancePriority = Random.Range(1, 100);
        navMeshAgent.updatePosition = true;  // ✅ BẬT LẠI - Để NavMesh tự move
        navMeshAgent.updateRotation = true;
        navMeshAgent.speed = 4f;             // ✅ Đảm bảo speed > 0

        Debug.Log($"✅ {gameObject.name} ({ghostType}) READY! Speed: {navMeshAgent.speed}");

        StartCoroutine(AIBehavior());
    }

    IEnumerator AIBehavior()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        while (true)
        {
            if (player != null && navMeshAgent.isActiveAndEnabled)
            {
                Vector3 targetPos = CalculateTargetPosition();

                // ✅ LUÔN CẬP NHẬT - KHÔNG KIỂM TRA KHOẢNG CÁCH (gây đứng yên)
                navMeshAgent.SetDestination(targetPos);

                // Debug Console
                Debug.Log($"{gameObject.name} → Target: {targetPos}, Distance: {Vector3.Distance(transform.position, targetPos):F1}");
            }
            yield return wait;
        }
    }

    Vector3 CalculateTargetPosition()
    {
        switch (ghostType)
        {
            case GhostType.Red:
                return player.position;

            case GhostType.Pink:
                return player.position - player.right * flankDistance;

            case GhostType.Blue:
                return player.position + player.right * flankDistance;

            case GhostType.Orange:
                // Predict đơn giản hơn
                return player.position + player.forward * predictTime;

            default:
                return player.position;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Vector3 target = CalculateTargetPosition();
            Gizmos.color = ghostType switch
            {
                GhostType.Red => Color.red,
                GhostType.Pink => Color.magenta,
                GhostType.Blue => Color.cyan,
                GhostType.Orange => Color.yellow,
                _ => Color.white
            };
            Gizmos.DrawSphere(target, 0.5f);
            Gizmos.DrawLine(transform.position, target);
        }
    }
}
