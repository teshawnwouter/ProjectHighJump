using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [Header("Pause variables")]
    public bool isPaused = false;
    [SerializeField] private GameObject pauseScreen;

    [Header("refreances")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GrappleHook grappleHook;
    [SerializeField] private Swinging swinging;
    [SerializeField] private WallRun wallRun;

    
    private void Update()
    {
        if (isPaused)
        {
            Paused();
        }
        else
        {
            UnPause();
        }
    }

    public void Paused()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        wallRun.enabled = false;
        playerMovement.enabled = false;
        grappleHook.enabled = false;
        swinging.enabled = false;
        pauseScreen.SetActive(true);
    }

    public void UnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        Time.timeScale = 1f;
        wallRun.enabled = true;
        playerMovement.enabled = true;
        grappleHook.enabled = true;
        swinging.enabled = true;
        pauseScreen.SetActive(false);
    }

    public void removePause()
    {
        isPaused = false;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isPaused)
            {
                isPaused = false;
            }
            else
            {
                isPaused = true;
            }
        }

    }
}
