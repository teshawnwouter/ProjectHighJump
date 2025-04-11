using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending1 : MonoBehaviour
{
    public void Return()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
