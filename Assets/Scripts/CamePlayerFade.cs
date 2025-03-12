using UnityEngine;

public class CamePlayerFade : MonoBehaviour
{
    private Material fadeMaterial;
    public GameObject player;
    [SerializeField] private float maxRot, transparancyPerDeg;
    void Start()
    {
        //fadeMaterial = player.GetComponent<Material>();
        transparancyPerDeg = 1 / maxRot;
    }

    private void Update()
    {
        fadeMaterial.color = new Vector4(fadeMaterial.color.r, fadeMaterial.color.g, fadeMaterial.color.b, 1 - transparancyPerDeg * Mathf.Abs(transform.eulerAngles.x));
    }
}
