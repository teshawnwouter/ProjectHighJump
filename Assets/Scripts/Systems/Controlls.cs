using UnityEngine;

public class Controlls : MonoBehaviour
{
    public GameObject pause;
    public Pause pausePause;
    public void ExitControlls()
    {
        pausePause.controllScreen = false;
        this.gameObject.SetActive(false);
        pause.SetActive(true);
    }
    
}
