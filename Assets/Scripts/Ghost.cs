using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Ghost : MonoBehaviour
{
    [Header("=== SETUP ===")]
    public Transform player;
    public GhostType ghostType = GhostType.Red;
    private Vector3 initialPosition;
    public GhostState currentState = GhostState.Chase;

    [Header("=== SETTINGS ===")]
    public float updateRate = 0.3f;
    public float flankDistance = 3.5f;
    public float predictTime = 1.5f;
    public float respawnDelay = 2.5f;

    [Header("=== SPEED SETTINGS ===")]
    public float chaseSpeed = 4.0f;
    public float frightenedSpeed = 2.0f;
    public float eatenSpeed = 8.0f;

    private NavMeshAgent navMeshAgent;

    public enum GhostType { Red, Pink, Blue, Orange }
    public enum GhostState { Chase, Frightened, Eaten, LeavingHome, DelayingHome }

    public static bool IsGloballyFrightened = false;
    public static float FrightenedDuration = 0f;

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

        // ➡️ LƯU VỊ TRÍ GỐC NGAY TRONG START()
        initialPosition = transform.position;

        // ✅ SETTINGS TỐI ƯU - BẬT LẠI DEFAULT
        navMeshAgent.avoidancePriority = Random.Range(1, 100);
        navMeshAgent.updatePosition = true;  // ✅ BẬT LẠI - Để NavMesh tự move
        navMeshAgent.updateRotation = true;
        navMeshAgent.speed = 4f;             // ✅ Đảm bảo speed > 0

        Debug.Log($"✅ {gameObject.name} ({ghostType}) READY! Speed: {navMeshAgent.speed}");
        SetState(GhostState.Chase);
        if (ghostType == GhostType.Red)
        {
            StartCoroutine(FrightenedTimerRoutine());
        }
    }
    public void StartChasing()
    {
        // Bắt đầu Coroutine có độ trễ 1 frame
        StartCoroutine(DelayedAIStart());
    }

    IEnumerator DelayedAIStart()
    {
        // CHỜ 1 FRAME: Điều này cho phép Unity kích hoạt đầy đủ NavMesh Agent
        yield return null;

        // Bắt đầu AIBehavior
        StartCoroutine(AIBehavior());
    }

    IEnumerator AIBehavior()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        while (true)
        {
            if (player != null && navMeshAgent.isActiveAndEnabled && navMeshAgent.isOnNavMesh)
            {
                Vector3 targetPos = CalculateTargetPosition();

                // Logic Eaten: Mục tiêu là Ghost House
                if (currentState == GhostState.Eaten)
                {
                    targetPos = initialPosition;

                    // KIỂM TRA ĐIỀU KIỆN ĐÃ VỀ NHÀ
                    if (Vector3.Distance(transform.position, initialPosition) < 1.0f)
                    {
                        SetState(GhostState.DelayingHome);
                        StartCoroutine(DelayLeavingHome());
                        continue;
                    }
                }

                // Logic LeavingHome: Mục tiêu vẫn là Ghost House nhưng tốc độ Chase
                else if (currentState == GhostState.LeavingHome)
                {
                    targetPos = initialPosition;
                }

                navMeshAgent.SetDestination(targetPos);
            }
            yield return wait;
        }

    }

    // Coroutine Quản lý thời gian Frightened (Chỉ cần chạy trên 1 Ghost)
    IEnumerator FrightenedTimerRoutine()
    {
        while (true)
        {
            if (IsGloballyFrightened)
            {
                FrightenedDuration -= updateRate;
                if (FrightenedDuration <= 0)
                {
                    SetAllGhostsState(GhostState.Chase);
                }
            }
            yield return new WaitForSeconds(updateRate);
        }
    }

    Vector3 CalculateTargetPosition()
    {
        // Logic Frightened: Chọn mục tiêu ngẫu nhiên gần đó
        if (currentState == GhostState.Frightened)
        {
            Vector3 randomPos = transform.position + Random.insideUnitSphere * 5f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 5f, NavMesh.AllAreas))
            {
                return hit.position;
            }
            return transform.position;
        }

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

    // Hàm gọi khi ăn Power Pellet
    public void SetAllFrightened(float duration)
    {
        IsGloballyFrightened = true;
        FrightenedDuration = duration;
        SetAllGhostsState(GhostState.Frightened);
    }

    // Hàm được gọi từ PlayerController khi va chạm
    public void SetState(GhostState newState)
    {
        if (currentState == GhostState.Eaten && newState != GhostState.LeavingHome && newState != GhostState.Chase && newState != GhostState.Frightened)
        {
            return; // Không đổi trạng thái khi đang bay về nhà
        }

        currentState = newState;
        UpdateAgentSettings();
    }

    // Hàm tiện ích để cập nhật tất cả Ghost
    void SetAllGhostsState(GhostState state)
    {
        Ghost[] allGhosts = FindObjectsByType<Ghost>(FindObjectsSortMode.None);
        foreach (var ghost in allGhosts)
        {
            if (ghost.currentState != GhostState.Eaten)
            {
                ghost.SetState(state);
            }
        }
        if (state != GhostState.Frightened) IsGloballyFrightened = false;
    }

    void UpdateAgentSettings()
    {
        switch (currentState)
        {
            case GhostState.Chase:
            case GhostState.LeavingHome:
                navMeshAgent.speed = chaseSpeed;
                // Cập nhật màu/model (Màu bình thường)
                break;
            case GhostState.Frightened:
                navMeshAgent.speed = frightenedSpeed;
                // Cập nhật màu/model (Màu Xanh!)
                break;
            case GhostState.Eaten:
                navMeshAgent.speed = eatenSpeed;
                // Cập nhật màu/model (Mắt)
                break;
        }
    }

    // Coroutine xử lý Ghost Respawn
    IEnumerator DelayLeavingHome()
    {
        // Vô hiệu hóa Agent để nó không di chuyển trong khi chờ
        navMeshAgent.enabled = false;

        SetState(GhostState.LeavingHome);

        yield return new WaitForSeconds(respawnDelay); // Chờ 2.5 giây

        navMeshAgent.enabled = true;

        // Trở lại Frightened nếu timer vẫn còn, nếu không thì Chase
        if (IsGloballyFrightened)
        {
            SetState(GhostState.Frightened);
        }
        else
        {
            SetState(GhostState.Chase);
        }
    }
}
