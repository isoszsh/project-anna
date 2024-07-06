using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyTutorial : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform player;
    public Transform currentTarget;

    private int index = 0;
    private float movementSpeed = 2f;    // Hareket h�z�
    private float rotationSpeed = 15f;    // D�n�� h�z�
    private float minDistanceToPlayer = 2f; // Player'a olan min mesafe
    private float maxDistanceFromLastFarthest = 1f; // En son en uzakta oldu�u konumdan maksimum uzakl�k
    private float maxRandomHeightChange = 1.75f; // Rastgele y�kseklik de�i�imi limiti

    private Vector3 lastFarthestPosition; // Player'dan en son uzak oldu�u konum

    // Start is called before the first frame update
    void Start()
    {
        SetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(currentTarget.position, transform.position) > 0.3f)
        {
            if (Vector3.Distance(player.position, transform.position) < minDistanceToPlayer)
            {
                Vector3 targetDirection = (currentTarget.position - transform.position).normalized;

                // Rastgele sa�a veya sola y�nelim ekle
                float randomHorizontalOffset = Random.Range(-0.3f, 0.3f);
                targetDirection += transform.right * randomHorizontalOffset;

                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.position += transform.forward * movementSpeed * Time.deltaTime;

                // Hedefe do�ru ilerlerken en son en uzakta oldu�u konumu s�f�rla
                lastFarthestPosition = transform.position;
            }
            else
            {
                // Player'dan uzaksa en son en uzakta oldu�u konumu kaydet ve etraf�nda rastgele u�
                Vector3 randomDirection = Random.insideUnitSphere.normalized;
                float randomHeightChange = Random.Range(-maxRandomHeightChange, maxRandomHeightChange);
                float clampedHeightChange = Mathf.Clamp(randomHeightChange, 0.1f, .5f); // Y�kseklik aral���
                Vector3 targetPosition = lastFarthestPosition + randomDirection * 2f + Vector3.up * clampedHeightChange; // U�ma mesafesi ve s�n�rl� y�kseklik de�i�imi
                Vector3 targetDirection = (targetPosition - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.position += transform.forward * movementSpeed * Time.deltaTime;

            }
        }
        else
        {
            // Hedefe ula��ld�, bir sonraki hedefi belirle
            index++;
            SetTarget();
        }
    }

    private void SetTarget()
    {
        if (index < waypoints.Length)
        {
            currentTarget = waypoints[index];
        }
    }
}
