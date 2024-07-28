using System.Collections;
using UnityEngine;

public class ChestBossStunState : ChestBossBaseState
{
    private int stunTime = 3;
    private GameObject player;
    private bool readyToLook = false;
    private LineRenderer lineRenderer;
    private float drawDuration = 2f; // Çizginin çizilme süresi

    public Gradient gradient;
    public float animationSpeed = 1.0f;

    private float animationTime = 0.0f;

    public override void Enter()
    {
        readyToLook = false;
        boss.GetComponent<Animator>().ResetTrigger("Run");
        boss.GetComponent<Animator>().SetTrigger("HitWall");

        player = boss.GetComponent<ChestBossEnemy>().player;

        // LineRenderer bileşenini başlat
        lineRenderer = boss.GetComponent<LineRenderer>();

        SetNewLineRenderer();

        // Coroutine'i başlat
        boss.StartCoroutine(StunRoutine());
    }

    public override void Perform()
    {
        if (readyToLook)
        {
            SmooteLookAtPlayer();
        }
        AnimateLineRenderer();
    }

    public override void Exit()
    {
        // Çizgiyi temizle
        boss.StartCoroutine(FadeOutLineRenderer());
    }

    private void SmooteLookAtPlayer()
    {
        Vector3 direction = player.transform.position - boss.transform.position;
        direction.y = 0; // Y ekseninde dönmeyi engellemek için
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, targetRotation, Time.deltaTime * 2);
    }

    private IEnumerator StunRoutine()
    {
        // Stun süresi boyunca bekle
        yield return new WaitForSeconds(stunTime);
        boss.GetComponent<Animator>().ResetTrigger("HitWall");
        boss.GetComponent<Animator>().SetTrigger("Stun");
        readyToLook = true;
        yield return new WaitForSeconds(2);
        readyToLook = false;
        if(bossStateMachine.stunNumber > 0){
            boss.StartCoroutine(DrawLineRenderer());
        }
        boss.GetComponent<Animator>().ResetTrigger("Stun");
        boss.GetComponent<Animator>().SetTrigger("Run");
        yield return new WaitForSeconds(2);
        // Takip state'ine geçiş yap
        bossStateMachine.GameLoop();
    }

    private IEnumerator DrawLineRenderer()
    {
        FindThePlayerTransform();

        Vector3 start = boss.transform.position;
        Vector3 end = (boss.transform.position + player.transform.position) / 2;
        float elapsedTime = 0;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);

        while (elapsedTime < drawDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / drawDuration);
            lineRenderer.SetPosition(1, Vector3.Lerp(start, end, t));
            yield return null;
        }
    }

    private IEnumerator FadeOutLineRenderer()
    {
        float elapsedTime = 0;
        Color startColor = lineRenderer.startColor;
        Color endColor = lineRenderer.endColor;

        while (elapsedTime < drawDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / drawDuration));
            startColor.a = alpha;
            endColor.a = alpha;
            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
            yield return null;
        }

        lineRenderer.positionCount = 0; // Çizgiyi tamamen temizle
    }

    private void ClearAttackPath()
    {
        if (lineRenderer != null)
        {
            boss.StartCoroutine(FadeOutLineRenderer());
        }
    }

    public void SetNewLineRenderer()
    {
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.black, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );
        
        lineRenderer.colorGradient = gradient;
    }

    void AnimateLineRenderer()
    {
        animationTime += Time.deltaTime * animationSpeed;
        if (animationTime > 1.0f)
            animationTime = 0.0f;

        lineRenderer.startColor = gradient.Evaluate(animationTime);
        lineRenderer.endColor = gradient.Evaluate((animationTime + 0.5f) % 1.0f);
    }

    public void FindThePlayerTransform()
    {
        bossStateMachine.playerTransform = boss.GetComponent<ChestBossEnemy>().player.transform.position + boss.transform.forward * 2;
        bossStateMachine.direction = (bossStateMachine.playerTransform - boss.transform.position).normalized;
    }
}
