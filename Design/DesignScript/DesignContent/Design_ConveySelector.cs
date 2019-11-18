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

    public override void DesignChange2D()
    {
        base.DesignChange2D();
        ChangeWorld();
    }

    public override void DesignChange3D()
    {
        base.DesignChange3D();
        ChangeWorld();
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
        else if (WorldManager.CurrentWorldState == EWorldState.View2D && IsCanChange2D)
            SetConveyRay(false);        
           
    }

    void ChangeWorld()
    {
        Power = false;

        if (CurMeshState == ConveySelectorState.Straight)
            transform.Find("Root3D").GetComponent<MeshFilter>().mesh = GetComponent<Design_ConveySelectorMesh>().StraightOFF;
        else if (CurMeshState == ConveySelectorState.Corner)
            transform.Find("Root3D").GetComponent<MeshFilter>().mesh = GetComponent<Design_ConveySelectorMesh>().CornerOFF;
    }

    void SetConveyRay(bool Is3D)
    {
        float CurObjectRotX = transform.Find("Root3D").rotation.x;

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
