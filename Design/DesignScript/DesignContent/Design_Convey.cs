using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EConveyDirection{ None, Left, Right, Up, Down }
public class Design_Convey : Design_WorldObjectController
{
    [HideInInspector]
    public bool Power;
    [HideInInspector]
    public List<EConveyDirection> ConveyState = new List<EConveyDirection>();
    public override void BeginPlay()
    {
        base.BeginPlay();

        Power = false;
    }
    public void ConveyPower(bool Is3D, EConveyDirection EConveyDir)
    {
        StartCoroutine(WaitRayChangingWorld(Is3D, EConveyDir));
    }


    IEnumerator WaitRayChangingWorld(bool Is3D, EConveyDirection EConveyDir)
    {
        yield return new WaitUntil(() => WorldManager.CurrentWorldState != EWorldState.Changing);

        Vector3 ConveyDir = Vector3.zero;
        float RayDistance = 2f;
        EConveyDirection PartnerDirecton = EConveyDirection.None;

        if (EConveyDir == EConveyDirection.Left)
        {
            PartnerDirecton = EConveyDirection.Right;
            ConveyDir = Vector3.left;
        }
        else if (EConveyDir == EConveyDirection.Right)
        {
            PartnerDirecton = EConveyDirection.Left;
            ConveyDir = Vector3.right;
        }
        else if (EConveyDir == EConveyDirection.Up)
        {
            PartnerDirecton = EConveyDirection.Down;
            ConveyDir = Vector3.up;
        }
        else if (EConveyDir == EConveyDirection.Down)
        {
            PartnerDirecton = EConveyDirection.Up;
            ConveyDir = Vector3.down;
        }


        if (Is3D)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, ConveyDir, out hit, RayDistance))
            {
                if (hit.transform.GetComponent<Design_Convey>() != null)
                {
                    if (!hit.transform.GetComponent<Design_Convey>().Power)
                    {
                        foreach (var Value in hit.transform.GetComponent<Design_Convey>().ConveyState)
                        {
                            if (Value == PartnerDirecton)
                            {
                                hit.transform.GetComponent<Design_Convey>().PushConveyPower();
                                break;
                            }
                        }
                    }
                }
                else if (hit.transform.parent.GetComponent<Design_Convey>() != null)
                {
                    if (!hit.transform.parent.GetComponent<Design_Convey>().Power)
                    {
                        foreach (var Value in hit.transform.parent.GetComponent<Design_Convey>().ConveyState)
                        {
                            if (Value == PartnerDirecton)
                            {
                                hit.transform.parent.GetComponent<Design_Convey>().PushConveyPower();
                                break;
                            }
                        }
                    }
                }

            }
        }
        else
        {
            RaycastHit2D[] hit2D = Physics2D.RaycastAll(transform.position, ConveyDir, RayDistance);
            foreach (var Value in hit2D)
            {
                if (Value.transform.parent.GetComponent<Design_Convey>() != null)
                {
                    if (!Value.transform.parent.GetComponent<Design_Convey>().Power && !Value.transform.parent.GetComponent<Design_Convey>().CheckBlockingTile())
                    {
                        foreach (var V in Value.transform.parent.GetComponent<Design_Convey>().ConveyState)
                        {
                            if (V == PartnerDirecton)
                            {
                                Value.transform.parent.GetComponent<Design_Convey>().PushConveyPower();
                                break;
                            }
                        }
                    }
                }
                else if (Value.transform.GetComponent<Design_Convey>() != null)
                {
                    if (!Value.transform.GetComponent<Design_Convey>().Power && !Value.transform.GetComponent<Design_Convey>().CheckBlockingTile())
                    {
                        foreach (var V in Value.transform.parent.GetComponent<Design_Convey>().ConveyState)
                        {
                            if (V == PartnerDirecton)
                            {
                                Value.transform.GetComponent<Design_Convey>().PushConveyPower();
                                break;
                            }
                        }
                    }
                }
            }
        }

    }

    public virtual void PushConveyPower() { }

    bool CheckBlockingTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (-Vector3.forward * 2f), -Vector3.forward, out hit))
        {
            if (hit.transform.GetComponent<CWorldObject>())
            {
                if (hit.transform.GetComponent<CWorldObject>().IsCanChange2D)
                    return true;
            }
            else if (hit.transform.parent.GetComponent<CWorldObject>())
            {
                if (hit.transform.parent.GetComponent<CWorldObject>().IsCanChange2D)
                    return true;
            }
        }
        return false;
    }

}
