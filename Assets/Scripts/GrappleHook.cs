using UnityEngine;

public class GrappleHook : MonoBehaviour
{

    public Spring spring;
    private HookShot grappleShot;
    public LineRenderer lr;
    public int quality;
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve affectCurve;
    private Vector3 currentGrapplePosition;
    public Material bluese;

    private void Start()
    {
        grappleShot = GetComponent<HookShot>();
        spring = GetComponent<Spring>();
        spring.SetTarget(0);
    }

    private void Update()
    {

    }

    private void DrawRope()
    {
        if (!grappleShot.hookShooting)
        {
            currentGrapplePosition = grappleShot.grappleShootPoint.position;
            spring.Reset();
            if (lr.enabled)
            {
                lr.enabled = false;
                lr.positionCount = 0;
            }
            return;
        }
        if (lr.positionCount == 0)
        {
            lr.enabled = true;
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }
        lr.material = bluese;
        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update();

        Vector3 up = Quaternion.LookRotation((grappleShot.grapplePoint - grappleShot.grappleShootPoint.position).normalized) * Vector3.up;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grappleShot.grapplePoint, Time.deltaTime * 12f);

        for (var i = 0; i < quality + 1; i++)
        {
            var delta = i / (float)quality;
            var offset = affectCurve.Evaluate(delta) * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * waveHeight * up;

            lr.SetPosition(i, Vector3.Lerp(grappleShot.grappleShootPoint.position, currentGrapplePosition, delta) + offset);
        }
    }
}
