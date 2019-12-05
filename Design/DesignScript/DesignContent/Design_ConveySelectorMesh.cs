using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ConveySelectorState { Straight, Corner }

[ExecuteInEditMode]
public class Design_ConveySelectorMesh : MonoBehaviour
{
    public Mesh StraightON;
    public Mesh StraightOFF;

    public Mesh CornerON;
    public Mesh CornerOFF;

    public ConveySelectorState ObjectMeshSelect;
    private ConveySelectorState BeforeMesh;

    public bool bRotate;
    private float CurRotationX;
    void Update()
    {
        if (Application.isPlaying == false)
        {
            ChangingMesh();
            RotateObject();
        }
    }

    void ChangingMesh()
    {
        if (BeforeMesh != ObjectMeshSelect)
        {
            Mesh TargetMesh = StraightOFF;

            if (ObjectMeshSelect == ConveySelectorState.Straight)
                TargetMesh = StraightOFF;
            else
                TargetMesh = CornerOFF;

            transform.Find("Root3D").GetComponent<MeshFilter>().mesh = TargetMesh;
            BeforeMesh = ObjectMeshSelect;
        }
    }

    void RotateObject()
    {
        if (bRotate)
        {
            //transform.Find("Root3D").Rotate(new Vector3(90, 0, 0));
            CurRotationX = CurRotationX + 90f;
            if (CurRotationX >= 360f)
                CurRotationX = 0f;

            transform.Find("Root3D").rotation = Quaternion.Euler(CurRotationX, -90f, 0);
        }
    }

}
