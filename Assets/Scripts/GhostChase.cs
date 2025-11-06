using UnityEngine;
using UnityEngine.AI;
using System.Collections; // Cần thêm thư viện này cho Coroutine

public class GhostChase : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;

    [Tooltip("Tần suất cập nhật đường đi (tính bằng giây). Giá trị nhỏ hơn sẽ phản ứng nhanh hơn.")]
    public float updateRate = 0.25f; // Cập nhật 4 lần/giây

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // **QUAN TRỌNG:** Đặt mức ưu tiên ngẫu nhiên
        // Ma có số nhỏ hơn sẽ "nhường đường" cho ma có số lớn hơn
        navMeshAgent.avoidancePriority = Random.Range(1, 100);

        // Bắt đầu vòng lặp đuổi theo
        StartCoroutine(ChasePlayer());
    }

    IEnumerator ChasePlayer()
    {
        // Vòng lặp này chạy mãi mãi
        while (true)
        {
            if (player != null && navMeshAgent.isActiveAndEnabled)
            {
                // Chỉ đặt đích đến sau mỗi 'updateRate' giây
                navMeshAgent.SetDestination(player.position);
            }

            // Chờ 'updateRate' giây rồi mới lặp lại
            yield return new WaitForSeconds(updateRate);
        }
    }

    // Chúng ta không cần hàm Update() nữa
    // void Update()
    // {
    // }
}