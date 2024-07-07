using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CreatureController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public Color[] colors;
    public Transform playerTransform; // Player'�n Transform'u

    private float fleeDistance = 1f; // Player'dan ka��� mesafesi
    private float minDistanceToDestination = 0.1f; // Hedefe ula��ld���n� kabul edece�imiz minimum mesafe

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;

        float randomscale = UnityEngine.Random.Range(.2f, 1f);
        this.transform.localScale = new Vector3(randomscale, randomscale, randomscale);
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        renderer.materials[1].color = colors[UnityEngine.Random.Range(0, colors.Length)];
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(EnableAnimator());
    }

    IEnumerator EnableAnimator()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, .5f));
        animator.enabled = true;
        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {
        // Hedefe ula��ld���nda veya hedef yoksa yeni hedef belirle
        if (!agent.pathPending && agent.remainingDistance < minDistanceToDestination)
        {
            SetDestination();
        }

        // Player'dan belirli bir mesafe yakla��ld���nda ka��� fonksiyonu �a�r�l�r
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) < fleeDistance)
        {
            agent.speed = 2;
            animator.speed = 2; // Animator oynatma h�z�n� da art�r
            FleeFromPlayer();
        }
        else
        {
            agent.speed = 1;
            animator.speed = 1; // Normal h�zda animasyon oynat
        }
    }

    private void SetDestination()
    {
        // NavMesh i�inde rastgele bir hedef belirle
        Vector3 randomPoint = RandomNavmeshLocation(1f); // 1 metre yar��ap�nda rastgele bir nokta al

        if (randomPoint != Vector3.zero)
        {
            agent.SetDestination(randomPoint);
        }
    }

    private void FleeFromPlayer()
    {
        // Player'dan ka�mak i�in ters y�nde bir hedef belirle
        Vector3 directionToPlayer = transform.position - playerTransform.position;
        Vector3 fleePosition = transform.position + directionToPlayer.normalized * fleeDistance;

        // NavMesh i�indeki en yak�n noktaya ka��� pozisyonunu belirle
        Vector3 validFleePosition = GetClosestNavmeshPoint(fleePosition);
        agent.SetDestination(validFleePosition);
    }

    // NavMesh i�inde rastgele bir nokta al
    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

    // NavMesh i�inde verilen noktaya en yak�n ge�erli noktay� bul
    private Vector3 GetClosestNavmeshPoint(Vector3 position)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return transform.position; // Ge�erli bir nokta bulunamazsa mevcut pozisyonu d�nd�r
        }
    }
}
