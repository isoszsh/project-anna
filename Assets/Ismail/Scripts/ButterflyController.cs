using UnityEngine;

public class ButterflyController : MonoBehaviour
{
    public Transform player;  // Oyuncunun transform referansý
    public Transform restPoint;  // Dinlenme noktasýnýn transform referansý
    public Transform sneakRestPoint;
    public float moveSpeed = 2f;  // Kelebeðin hareket hýzý
    public float flyRange = 5f;  // Kelebeðin oyuncudan uzaklaþabileceði maksimum mesafe
    public float changeTargetInterval = 2f;  // Hedef pozisyonu deðiþtirme aralýðý
    public float rotationSpeed = 5f;  // Dönüþ hýzý
    public float smoothing = 0.1f;  // Hareketlerin yumuþaklýðý için smoothing deðeri
    public float restDuration = 3f;  // Dinlenme süresi
    public float minRestInterval = 5f;  // Minimum dinlenme aralýðý
    public float maxRestInterval = 10f;  // Maksimum dinlenme aralýðý
    public string animationName = "Butterfly_Fly";  // Uçuþ animasyonunun adý

    private Vector3 targetPosition;
    public float timer;
    public float restTimer;
    public bool isResting;
    public bool isMovingToPoint;
    public bool sneaking;

    public Animation anim;
    public PlayerController playerController;// Animasyon bileþeni

    void Start()
    {


        PlayAnimation(animationName, 1.5f);

        SetNewTargetPosition();
        SetNextRestTimer();
    }

    void Update()
    {
        if(playerController.sneaking)
        {
            sneaking = true;
        }
        else
        {
            sneaking = false;
        }


        if (isResting)
        {
            if(sneaking)
            {
                HandleSneakResting();
            }
            else
            {
                HandleResting();
            }
            
        }
        else if (isMovingToPoint)
        {
            MoveTowardsPoint();
        }
        else
        {
            HandleFlying();
        }
    }

    void StartResting()
    {
        isResting = true;
        isMovingToPoint = true;
        targetPosition = restPoint.position;
        restTimer = restDuration;
        PlayAnimation(animationName, 0.025f);
    }

    void StartSneakResting()
    {
        isResting = true;
        isMovingToPoint = true;
        targetPosition = sneakRestPoint.position;
        restTimer = restDuration;
        PlayAnimation(animationName, 0.025f);
    }

    void EndResting()
    {
        isResting = false;
        isMovingToPoint = false;
        restTimer = -1f;
        SetNewTargetPosition();
        SetNextRestTimer();
        PlayAnimation(animationName, 1.5f);
    }

    void HandleResting()
    {
        // Dinlenme süresi boyunca kelebek sabit kalýr
        transform.position = restPoint.position;
        transform.rotation = restPoint.rotation;

        // Dinlenme süresi dolduðunda tekrar uçuþ moduna geç
        restTimer -= Time.deltaTime;
        if (restTimer <= 0f)
        {
            EndResting();
        }
    }

    void HandleSneakResting()
    {

        if (Vector3.Distance(transform.position, sneakRestPoint.position) > 0.1f)
        {
            MoveTowardsTarget(sneakRestPoint.position);
            isMovingToPoint = true;
            isResting = false;
            PlayAnimation(animationName, 1.5f);
            return;
        }
        // Dinlenme süresi boyunca kelebek sabit kalýr
        transform.position = sneakRestPoint.position;
        transform.rotation = sneakRestPoint.rotation;
        restTimer = -1f;
        if (!sneaking)
        {

            EndResting();
            restTimer = -1f;
        }

    }

    void MoveTowardsPoint()
    {
        if(sneaking)
        {
            MoveTowardsTarget(sneakRestPoint.position);

            if (Vector3.Distance(transform.position, sneakRestPoint.position) < 0.1f)
            {
                isMovingToPoint = false;
                isResting = true;
                PlayAnimation(animationName, 0.025f);
                StartSneakResting();
            }
        }
        else
        {
            MoveTowardsTarget(restPoint.position);

            if (Vector3.Distance(transform.position, restPoint.position) < 0.1f)
            {
                isMovingToPoint = false;
                isResting = true;
                restTimer = restDuration;
                PlayAnimation(animationName, 0.025f);
                StartResting();
            }
        }
        
    }

    void HandleFlying()
    {
        timer += Time.deltaTime;

        if (timer > changeTargetInterval)
        {
            SetNewTargetPosition();
            timer = 0;
        }

        MoveTowardsTarget(targetPosition);

        if (sneaking)
        {
            isMovingToPoint = true;
            targetPosition = sneakRestPoint.position;
            PlayAnimation(animationName, 1.5f);
        }
        else if (restTimer <= 0f)
        {
            isMovingToPoint = true;
            targetPosition = restPoint.position;
            PlayAnimation(animationName, 1.5f);
        }

        restTimer -= Time.deltaTime;
    }

    void SetNewTargetPosition()
    {
        if (!isResting)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-flyRange, flyRange),
                Random.Range(0.6f, 1.2f),
                Random.Range(-flyRange, flyRange)
            );

            targetPosition = player.position + randomOffset;
        }
        else
        {
            targetPosition = restPoint.position;
        }
    }

    void MoveTowardsTarget(Vector3 target)
    {
        if (isMovingToPoint || isResting || sneaking)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, 0.5f *  Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime * moveSpeed);
        }
       
        LookTowardsTarget(target);
    }

    void LookTowardsTarget(Vector3 target)
    {
        Vector3 direction = target - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            float heightDifference = target.y - transform.position.y;
            float tiltAngle = Mathf.Atan2(heightDifference, direction.magnitude) * Mathf.Rad2Deg;
            Quaternion targetTilt = Quaternion.Euler(tiltAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetTilt, rotationSpeed * Time.deltaTime);
        }
    }

    void SetNextRestTimer()
    {
        restTimer = Random.Range(minRestInterval, maxRestInterval);
    }

    void PlayAnimation(string animationName, float speed)
    {
        if (anim != null && anim[animationName] != null)
        {
            anim[animationName].speed = speed;
            anim.Play(animationName);
        }
    }
}
