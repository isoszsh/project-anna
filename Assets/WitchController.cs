using UnityEngine;
using System.Collections;

public class WitchController : MonoBehaviour
{
    public GameObject potionPrefab;
    public GameObject gasCloudPrefab;
    public GameObject minionPrefab;
    public GameObject poisonDartPrefab;
    public GameObject stinkBombPrefab;
    public GameObject plantPrefab;
    public GameObject targetMarkerPrefab;
    public Transform cauldron;
    public Transform[] plantSpawnPoints;
    public float attackInterval = 2.0f;
    public float gasCloudDuration = 5.0f;
    public float stinkBombDelay = 3.0f;
    public float targetMarkerDuration = 2.0f;
    public float potionArcHeight = 5.0f;

    private int attackCount = 0;  // Total attacks counter
    private int plantCount = 0;   // Plant counter
    private bool isAttacking = false;
    private Transform player;

    public GameObject stunAnimationStars;
    public GameObject flowersVases;

    public bool isAttackingNow ;

    public GameObject music;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        stunAnimationStars.SetActive(false);
        isAttackingNow = false;
        // GameManager.Instance.playerController.lockControls = true;
        // StartCoroutine(AttackRoutine()); // Bunu �a��rmadan �nce ba�ka bir �enumerator olu�tur ve cutscene yap
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    public void StartEveryThink()
    {
        isAttackingNow = true;
        StartCoroutine(AttackRoutine());
    }

    public void StopEveryThink()
    {
        isAttackingNow = false;
        music.SetActive(false);
    }

    IEnumerator AttackRoutine()
    {
        while (isAttackingNow)
        {
            yield return new WaitForSeconds(attackInterval);

            int attackChoice = Random.Range(0, 6);
            switch (attackChoice)
            {
                case 0:
                    StartCoroutine(PotionSplash());
                    break;
                case 1:
                    StartCoroutine(GasCloud());
                    break;
                case 2:
                    StartCoroutine(SummonMinions());
                    break;
                case 3:
                    StartCoroutine(PoisonDart());
                    break;
                case 4:
                    StartCoroutine(StinkBomb());
                    break;
                case 5:
                    StartCoroutine(CircleAttack());
                    break;
            }

            attackCount++;
            if (attackCount % 10 == 0) // After every 10 attacks
            {
                stunAnimationStars.SetActive(true);
                flowersVases.GetComponent<Animator>().SetTrigger("Up");
                if (plantCount < 5) // If there are less than 5 plants
                {
                    SpawnPlant();
                    yield return new WaitForSeconds(15.0f); // Wait for 15 seconds
                    stunAnimationStars.SetActive(false);
                    flowersVases.GetComponent<Animator>().SetTrigger("Down");
                }
                else
                {
                    yield return new WaitForSeconds(15.0f); // Wait for 15 seconds if max plants are spawned
                }
            }
            isAttacking = false;
        }
    }

    IEnumerator PotionSplash()
    {
        isAttacking = true;
        Vector3 targetPosition = player.position;
        GameObject marker = Instantiate(targetMarkerPrefab, targetPosition, Quaternion.identity);
        yield return new WaitForSeconds(targetMarkerDuration);

        GameObject potion = Instantiate(potionPrefab, new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
        Vector3 startPos = potion.transform.position;
        Vector3 endPos = targetPosition;
        float duration = 1.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            potion.transform.position = Vector3.Lerp(startPos, endPos, t) + Vector3.up * Mathf.Sin(t * Mathf.PI) * potionArcHeight;
            elapsed += Time.deltaTime;
            yield return null;
        }

        potion.transform.position = endPos;
        Destroy(marker);
        isAttacking = false;
    }

    IEnumerator GasCloud()
    {
        isAttacking = true;
        Vector3 targetPosition = player.position;
        GameObject marker = Instantiate(targetMarkerPrefab, targetPosition, Quaternion.identity);
        yield return new WaitForSeconds(targetMarkerDuration);

        GameObject gasCloud = Instantiate(gasCloudPrefab, targetPosition, Quaternion.identity);
        Destroy(gasCloud, gasCloudDuration);
        Destroy(marker);
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }

    IEnumerator SummonMinions()
    {
        isAttacking = true;
        Transform m1 = Instantiate(minionPrefab, new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z) + Vector3.left * 2, Quaternion.identity).transform;
        m1.rotation = Quaternion.Euler(0, 180, 0);
        Transform m2 = Instantiate(minionPrefab, new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z) + Vector3.right * 2, Quaternion.identity).transform;
        m2.rotation = Quaternion.Euler(0, 180, 0);

        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }

    IEnumerator PoisonDart()
    {
        isAttacking = true;
        Vector3 targetPosition = player.position;
        GameObject marker = Instantiate(targetMarkerPrefab, targetPosition, Quaternion.identity);
        yield return new WaitForSeconds(targetMarkerDuration);

        GameObject dart = Instantiate(poisonDartPrefab, new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
        dart.GetComponent<Rigidbody>().AddForce((targetPosition - transform.position).normalized * 15f, ForceMode.VelocityChange);
        Destroy(marker);
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }

    IEnumerator StinkBomb()
    {
        isAttacking = true;
        Vector3 targetPosition = player.position;
        GameObject marker = Instantiate(targetMarkerPrefab, targetPosition, Quaternion.identity);
        yield return new WaitForSeconds(targetMarkerDuration);

        GameObject stinkBomb = Instantiate(stinkBombPrefab, new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
        stinkBomb.GetComponent<Rigidbody>().AddForce((targetPosition - transform.position).normalized * 5f, ForceMode.VelocityChange);
        Destroy(marker);
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }

    IEnumerator CircleAttack()
    {
        isAttacking = true;
        int numberOfObjects = 10; // Number of objects in the circle
        float radius = 3.0f; // Radius of the circle
        float expansionSpeed = 10.0f; // Expansion speed
        float spawnInterval = 0.2f; // Interval for spawning objects

        Vector3 centerPosition = new Vector3(cauldron.position.x, cauldron.position.y + .5f, cauldron.position.z);

        // Calculate positions around the circle for spawning objects
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfObjects;
            Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius + centerPosition;
            GameObject obj = Instantiate(potionPrefab, spawnPosition, Quaternion.identity);

            StartCoroutine(ExpandObject(obj, expansionSpeed));
            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }

    IEnumerator ExpandObject(GameObject obj, float speed)
    {
        while (true)
        {
            if (obj != null)
            {
                obj.transform.position += (obj.transform.position - cauldron.position).normalized * speed * Time.deltaTime;
                yield return null;
            }
        }
    }

    void SpawnPlant()
    {
        if (plantCount < 5)
        {
            int spawnIndex = Random.Range(0, plantSpawnPoints.Length);
            Instantiate(plantPrefab, plantSpawnPoints[spawnIndex].position, Quaternion.identity);
            plantCount++;
        }
    }
}
