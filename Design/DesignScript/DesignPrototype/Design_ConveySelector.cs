using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_ConveySelector : Design_Convey
{
    private ConveySelectorState CurMeshState;
    public override void BeginPlay()
    {
        base.BeginPlay();

        CurMeshState = GetComponent<Design_ConveySelectorMesh>().ObjectMeshSelect;
    }


    public override void PushConveyPower()
    {
        base.PushConveyPower();

        Power = true;

        if (CurMeshState == ConveySelectorState.Straight)
            transform.Find("Root3D").GetComponent<MeshFilter>().mesh = GetComponent<Design_ConveySelectorMesh>().StraightON;
        else if (CurMeshState == ConveySelectorState.Corner)
            transform.Find("Root3D").GetComponent<MeshFilter>().mesh = GetComponent<Design_ConveySelectorMesh>().CornerON;

        if (WorldManager.CurrentWorldState == EWorldState.View3D)
            SetConveyRay(true);
        else if (WorldManager.CurrentWorldState == EWorldState.View2D && bShow)
            SetConveyRay(false);        
           
    }

    public override void ChangeWorld(EWorldState CurState)
    {
        base.ChangeWorld(CurState);

        Power = false;

        if (CurMeshState == ConveySelectorState.Straight)
            transform.Find("Root3D").GetComponent<MeshFilter>().mesh = GetComponent<Design_ConveySelectorMesh>().StraightOFF;
        else if (CurMeshState == ConveySelectorState.Corner)
            transform.Find("Root3D").GetComponent<MeshFilter>().mesh = GetComponent<Design_ConveySelectorMesh>().CornerOFF;
    }

    void SetConveyRay(bool Is3D)
    {
        float CurObjectRotX = transform.Find("Root3D").rotation.x;
        Debug.Log(CurObjectRotX);

        if (CurObjectRotX == -0.5f)
            CurObjectRotX *= transform.Find("Root3D").forward.y;
        else if (CurObjectRotX < -0.5f)
            CurObjectRotX = 1f;

        if (CurMeshState == ConveySelectorState.Straight)
        {
            if (CurObjectRotX == 0f)
            {
                ConveyPower(Is3D, EConveyDirection.Left);
                ConveyPower(Is3D, EConveyDirection.Right);
            }
            else if (CurObjectRotX == 0.5f)
            {
                ConveyPower(Is3D, EConveyDirection.Up);
                ConveyPower(Is3D, EConveyDirection.Down);
            }
            else if (CurObjectRotX == 1f)
            {
                ConveyPower(Is3D, EConveyDirection.Left);
                ConveyPower(Is3D, EConveyDirection.Right);
            }
            else if (CurObjectRotX == -0.5f)
            {
                ConveyPower(Is3D, EConveyDirection.Up);
                ConveyPower(Is3D, EConveyDirection.Down);
            }
        }
        else if (CurMeshState == ConveySelectorState.Corner)
        {
            if (CurObjectRotX == 0)
            {
                ConveyPower(Is3D, EConveyDirection.Left);
                ConveyPower(Is3D, EConveyDirection.Down);
            }
            else if (CurObjectRotX == 0.5f)
            {
                Debug.Log("일단 제대로 여기가 불리는지부터");
                ConveyPower(Is3D, EConveyDirection.Right);
                ConveyPower(Is3D, EConveyDirection.Down);
            }
            else if (CurObjectRotX == 1f)
            {
                ConveyPower(Is3D, EConveyDirection.Up);
                ConveyPower(Is3D, EConveyDirection.Right);
            }
            else if (CurObjectRotX == -0.5f)
            {
                ConveyPower(Is3D, EConveyDirection.Up);
                ConveyPower(Is3D, EConveyDirection.Left);
            }
        }
    }

}
