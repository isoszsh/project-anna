using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class ShowcaseManager : MonoBehaviour
{
    public InputActionAsset inputActionAsset; // Input Asset'inizi burada baðlayýn
    private InputAction jumpAction;
    public string levelName;
    private void OnEnable()
    {
        // Input Action Asset'ten "Jump" aksiyonunu al
        var gameplayActionMap = inputActionAsset.FindActionMap("Player"); // "Gameplay" yerine Input Asset'teki aksiyon haritanýzýn adýný yazýn
        if (gameplayActionMap != null)
        {
            jumpAction = gameplayActionMap.FindAction("Jump"); // "Jump" aksiyonunun adýný kullanýn
            if (jumpAction != null)
            {
                jumpAction.Enable();
                jumpAction.performed += OnJumpPerformed;
            }
        }
    }

    private void OnDisable()
    {
        if (jumpAction != null)
        {
            jumpAction.Disable();
            jumpAction.performed -= OnJumpPerformed;
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        // Asenkron bir þekilde "prepare_game" sahnesine geçiþ yap
        StartCoroutine(LoadPrepareGameSceneAsync());
    }

    IEnumerator LoadPrepareGameSceneAsync()
    {
        // Sahneyi asenkron yükler
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);

        // Yükleme tamamlanana kadar bekle
        while (!asyncLoad.isDone)
        {
            yield return null; // Bir sonraki frame'e geç
        }
    }
}
