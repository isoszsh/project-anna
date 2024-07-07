using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CreatureController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public Color[] colors;
    public Transform playerTransform; // Player'ýn Transform'u

    private float fleeDistance = 1f; // Player'dan kaçýþ mesafesi
    private float minDistanceToDestination = 0.1f; // Hedefe ulaþýldýðýný kabul edeceðimiz minimum mesafe

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
        // Hedefe ulaþýldýðýnda veya hedef yoksa yeni hedef belirle
        if (!agent.pathPending && agent.remainingDistance < minDistanceToDestination)
        {
            SetDestination();
        }

        // Player'dan belirli bir mesafe yaklaþýldýðýnda kaçýþ fonksiyonu çaðrýlýr
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) < fleeDistance)
        {
            agent.speed = 2;
            animator.speed = 2; // Animator oynatma hýzýný da artýr
            FleeFromPlayer();
        }
        else
        {
            agent.speed = 1;
            animator.speed = 1; // Normal hýzda animasyon oynat
        }
    }

    private void SetDestination()
    {
        // NavMesh içinde rastgele bir hedef belirle
        Vector3 randomPoint = RandomNavmeshLocation(1f); // 1 metre yarýçapýnda rastgele bir nokta al

        if (randomPoint != Vector3.zero)
        {
            agent.SetDestination(randomPoint);
        }
    }

    private void FleeFromPlayer()
    {
        // Player'dan kaçmak için ters yönde bir hedef belirle
        Vector3 directionToPlayer = transform.position - playerTransform.position;
        Vector3 fleePosition = transform.position + directionToPlayer.normalized * fleeDistance;

        // NavMesh içindeki en yakýn noktaya kaçýþ pozisyonunu belirle
        Vector3 validFleePosition = GetClosestNavmeshPoint(fleePosition);
        agent.SetDestination(validFleePosition);
    }

    // NavMesh içinde rastgele bir nokta al
    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

    // NavMesh içinde verilen noktaya en yakýn geçerli noktayý bul
    private Vector3 GetClosestNavmeshPoint(Vector3 position)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return transform.position; // Geçerli bir nokta bulunamazsa mevcut pozisyonu döndür
        }
    }
}
