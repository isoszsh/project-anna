using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class ShowcaseManager : MonoBehaviour
{
    public InputActionAsset inputActionAsset; // Input Asset'inizi burada ba�lay�n
    private InputAction jumpAction;
    public string levelName;
    private void OnEnable()
    {
        // Input Action Asset'ten "Jump" aksiyonunu al
        var gameplayActionMap = inputActionAsset.FindActionMap("Player"); // "Gameplay" yerine Input Asset'teki aksiyon haritan�z�n ad�n� yaz�n
        if (gameplayActionMap != null)
        {
            jumpAction = gameplayActionMap.FindAction("Jump"); // "Jump" aksiyonunun ad�n� kullan�n
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
        // Asenkron bir �ekilde "prepare_game" sahnesine ge�i� yap
        StartCoroutine(LoadPrepareGameSceneAsync());
    }

    IEnumerator LoadPrepareGameSceneAsync()
    {
        // Sahneyi asenkron y�kler
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);

        // Y�kleme tamamlanana kadar bekle
        while (!asyncLoad.isDone)
        {
            yield return null; // Bir sonraki frame'e ge�
        }
    }
}
