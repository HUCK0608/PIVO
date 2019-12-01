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
        SetConveyState();
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

    void SetConveyState()
    {
        float CurObjectRotY = transform.Find("Root3D").rotation.y;
        CurObjectRotY *= 10;
        CurObjectRotY = Mathf.Round(CurObjectRotY);
        CurObjectRotY *= 0.1f;
        ConveyState.Clear();

        if (CurMeshState == ConveySelectorState.Straight)
        {
            if (CurObjectRotY == 0.7f)
            {
                ConveyState.Add(EConveyDirection.Left);
                ConveyState.Add(EConveyDirection.Right);
            }
            else if (CurObjectRotY == -0.7f)
            {
                ConveyState.Add(EConveyDirection.Left);
                ConveyState.Add(EConveyDirection.Right);
            }
            else if (CurObjectRotY == 0.5f)
            {
                ConveyState.Add(EConveyDirection.Up);
                ConveyState.Add(EConveyDirection.Down);
            }
            else if (CurObjectRotY == -0.5f)
            {
                ConveyState.Add(EConveyDirection.Up);
                ConveyState.Add(EConveyDirection.Down);
            }
            else if (CurObjectRotY == 0f)
            {
                ConveyState.Add(EConveyDirection.Left);
                ConveyState.Add(EConveyDirection.Right);
            }
        }
        else if (CurMeshState == ConveySelectorState.Corner)
        {
            if (CurObjectRotY == 0.7f)
            {
                ConveyState.Add(EConveyDirection.Left);
                ConveyState.Add(EConveyDirection.Up);
            }
            else if (CurObjectRotY == -0.7f)
            {
                ConveyState.Add(EConveyDirection.Down);
                ConveyState.Add(EConveyDirection.Right);
            }
            else if (CurObjectRotY == 0.5f)
            {
                ConveyState.Add(EConveyDirection.Right);
                ConveyState.Add(EConveyDirection.Up);
            }
            else if (CurObjectRotY == -0.5f)
            {
                ConveyState.Add(EConveyDirection.Left);
                ConveyState.Add(EConveyDirection.Down);
            }
            else if (CurObjectRotY == 0f)
            {
                ConveyState.Add(EConveyDirection.Left);
                ConveyState.Add(EConveyDirection.Up);
            }
        }
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
        foreach (var Value in ConveyState)
        {
            ConveyPower(Is3D, Value);
        }
        
    }

}
