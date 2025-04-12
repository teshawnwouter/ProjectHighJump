using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("EndScreen");
        }
    }
}
