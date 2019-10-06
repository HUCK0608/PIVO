using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_IntroTimeLineCircleMovement : MonoBehaviour
{
    float TimeCounter = 0;
    public float Speed;
    public float Width;
    public float Height;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimeCounter += Time.deltaTime;

        float x = Mathf.Cos(TimeCounter) * Width;
        float y = Mathf.Sin(TimeCounter) * Height;
        float z = 0;

        transform.position = new Vector3(x, y, z);

        
    }
}
