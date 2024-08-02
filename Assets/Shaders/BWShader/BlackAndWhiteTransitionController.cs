using UnityEngine;

public class BlackAndWhiteTransitionController : MonoBehaviour
{
    public Material transitionMaterial;
    public float transitionDuration = 2.0f;
    private bool isTransitioningToBlackAndWhite = false;
    private bool isTransitioningToOriginal = false;

    void Start()
    {
        // Oyun baþladýðýnda geçiþ ilerlemesini 1 olarak ayarla (renkli)
        transitionMaterial.SetFloat("_TransitionProgress", 1f);
    }

    public void OpenEyes()
    {
        isTransitioningToBlackAndWhite = true;
        isTransitioningToOriginal = false;
    }

    public void CloseEyes()
    {
        isTransitioningToBlackAndWhite = false;
        isTransitioningToOriginal = true;
    }

    void Update()
    {
        if (isTransitioningToBlackAndWhite)
        {
            float transitionProgress = transitionMaterial.GetFloat("_TransitionProgress");
            transitionProgress += Time.deltaTime / transitionDuration;
            transitionMaterial.SetFloat("_TransitionProgress", Mathf.Clamp01(transitionProgress));

            if (transitionProgress >= 1f)
            {
                isTransitioningToBlackAndWhite = false;
            }
        }

        if (isTransitioningToOriginal)
        {
            float transitionProgress = transitionMaterial.GetFloat("_TransitionProgress");
            transitionProgress -= Time.deltaTime / transitionDuration;
            transitionMaterial.SetFloat("_TransitionProgress", Mathf.Clamp01(transitionProgress));

            if (transitionProgress <= 0f)
            {
                isTransitioningToOriginal = false;
            }
        }
    }
}
