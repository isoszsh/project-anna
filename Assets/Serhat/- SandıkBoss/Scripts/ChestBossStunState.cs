using System.Collections;
using UnityEngine;

public class ChestBossStunState : ChestBossBaseState
{
    private int stunTime = 2;
    private GameObject player;
    private bool readyToLook = false;
    private LineRenderer lineRenderer;
    private float drawDuration = 2f; // Çizginin çizilme süresi

    public Gradient gradient;
    public float animationSpeed = 1.0f;

    private float animationTime = 0.0f;
    private float lineHeightOffset = 0.5f; // Çizginin yerden yüksekte oluşması için offset

    public override void Enter()
    {
        Debug.Log("Stun State");
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
        if (bossStateMachine.stunNumber != 3)
        {
            yield return new WaitForSeconds(stunTime);
        }
        boss.GetComponent<Animator>().ResetTrigger("HitWall");
        boss.GetComponent<Animator>().SetTrigger("Stun");
        if(bossStateMachine.stunNumber == 0)
        {
            player = boss.allSpawnPointParentObject;
        }
        readyToLook = true;
        yield return new WaitForSeconds(1.5f);
        readyToLook = false;
        if (bossStateMachine.stunNumber > 0)
        {
            boss.StartCoroutine(DrawLineRenderer());
            yield return new WaitForSeconds(2);
        }        
        boss.GetComponent<Animator>().ResetTrigger("Stun");
        boss.GetComponent<Animator>().SetTrigger("Run");
        // Takip state'ine geçiş yap
        bossStateMachine.GameLoop();
    }

    private IEnumerator DrawLineRenderer()
    {
        FindThePlayerTransform();

        Vector3 start = boss.transform.position;
        start.y += lineHeightOffset; // Başlangıç pozisyonunu yükselt
        Vector3 direction = (bossStateMachine.playerTransform - boss.transform.position).normalized;
        Vector3 end = bossStateMachine.playerTransform - direction * 4;
        end.y += lineHeightOffset; // Bitiş pozisyonunu yükselt
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

        while (elapsedTime < drawDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / drawDuration));
            Color startColor = lineRenderer.startColor;
            Color endColor = lineRenderer.endColor;
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
        lineRenderer.endWidth = 0.1f; // Uç noktasının küçülmesi için

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

        float alpha = Mathf.Sin(animationTime * Mathf.PI * 2) * 0.5f + 0.5f; // Nefes alır gibi alpha değeri değişir

        Gradient gradient = lineRenderer.colorGradient;
        GradientColorKey[] colorKeys = gradient.colorKeys;
        GradientAlphaKey[] alphaKeys = gradient.alphaKeys;

        for (int i = 0; i < alphaKeys.Length; i++)
        {
            alphaKeys[i].alpha = alpha;
        }

        gradient.SetKeys(colorKeys, alphaKeys);
        lineRenderer.colorGradient = gradient;
    }

    public void FindThePlayerTransform()
    {
        Vector3 direction = (player.transform.position - boss.transform.position).normalized;
        direction.y = 0; // Y eksenindeki farkı sıfırlayarak yatay düzlemde tut
        Vector3 end = player.transform.position + direction * 5;
        end.y += lineHeightOffset; // Player'ın y eksenine ayarla ve yükselt

        bossStateMachine.playerTransform = end;
        Vector3 newDirection = (end - boss.transform.position).normalized;
        bossStateMachine.direction = newDirection;
    }
}
