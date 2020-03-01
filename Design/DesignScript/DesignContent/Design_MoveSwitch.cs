using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_MoveSwitch : MonoBehaviour
{
    bool SwitchBool = false;

    public List<GameObject> MovingActor = new List<GameObject>();
    public bool RepeatSwitch;
    
    public float TargetValue;
    public float PushDownSpeed;

    [SerializeField]
    private SoundRandomPlayer_SFX _buttonSoundRandomPlayer = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && MovingActor[0] && !SwitchBool)
        {
            foreach (var V in MovingActor)
            {
                V.GetComponent<Design_MovingActor>().OnMovingActor();
            }

            if (!RepeatSwitch)
            {
                SwitchBool = true;
                _buttonSoundRandomPlayer.Play();
                StartCoroutine(PushDownInnerMesh());
            }
        }
    }

    IEnumerator PushDownInnerMesh()
    {
        while (true)
        {
            Transform InnerMesh = transform.Find("InnerMesh");

            if (InnerMesh.localPosition.y > -TargetValue)
            {
                InnerMesh.position -= new Vector3(0, PushDownSpeed, 0);
                yield return new WaitForFixedUpdate();
            }
            else
            {
                InnerMesh.localPosition = new Vector3(InnerMesh.localPosition.x, -TargetValue, InnerMesh.localPosition.z);
                break;
            }
        }
    }
}
