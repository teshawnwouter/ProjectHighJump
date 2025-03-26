using UnityEngine;

public class PlatformChild : MonoBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Attach(GameObject platform)
    {
        gameObject.transform.SetParent(platform.transform,true);

    }

    public void Detach(GameObject Plarform) 
    {
        gameObject.transform.SetParent(null, true);

    }

}
