using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_LightTrigger : CWorldObject
{
    public float IntensityUpSpeed;
    public float IntensityDownSpeed;

    private Light PointLight;
    private float DefaultIntensity;
    private Coroutine TargetCoroutine;

    protected override void Awake()
    {
        PointLight = GetComponentInChildren<Light>();
        DefaultIntensity = PointLight.intensity;
        SetLight(false);
    }

    public override void Change2D()
    {
        GetComponent<BoxCollider>().enabled = false;
        SetLight(false);
    }

    public override void Change3D()
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    public override void ShowOffBlock() { }
    public override void ShowOnBlock() { }

    public void SetLight(bool bState)
    {
        if (bState)
        {
            if (TargetCoroutine != null)
                StopCoroutine(TargetCoroutine);

            TargetCoroutine = StartCoroutine(IntensityUp());
        }
        else
        {
            if (TargetCoroutine != null)
                StopCoroutine(TargetCoroutine);

            TargetCoroutine = StartCoroutine(IntensityDown());
        }
    }

    IEnumerator IntensityUp()
    {
        while (PointLight.intensity < DefaultIntensity)
        {
            if (IntensityUpSpeed > 0)
            {
                PointLight.intensity += IntensityUpSpeed;
                yield return new WaitForFixedUpdate();
            }
            else
            {
                PointLight.intensity = DefaultIntensity;
                yield return null;
            }
        }

        PointLight.intensity = DefaultIntensity;
    }

    IEnumerator IntensityDown()
    {
        while (PointLight.intensity > 0)
        {
            if (IntensityDownSpeed > 0)
            {
                PointLight.intensity -= IntensityDownSpeed;
                yield return new WaitForFixedUpdate();
            }
            else
            {
                PointLight.intensity = 0;
                yield return null;
            }
        }

        PointLight.intensity = 0;
    }

    new private void OnTriggerEnter(Collider other)
    {
        if (CPlayerManager.Instance.RootObject3D == other.gameObject)
            SetLight(true);
    }

    new private void OnTriggerExit(Collider other)
    {
        if (CPlayerManager.Instance.RootObject3D == other.gameObject)
            SetLight(false);
    }
}
