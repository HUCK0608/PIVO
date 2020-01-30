using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
public enum ThemeType{Grass, Snow}

[ExecuteInEditMode]
public class Design_MaterialChange : MonoBehaviour
{
    public Material ChangeMaterial;
    private Material BeforeMaterial;

    public bool SaveCurMat;
    public bool RotPlus;
    public bool RotMinus;
    public bool RotZero;
    public bool RotZ;
    public ThemeType Theme;
    public bool UseSideRandom;
    public bool Use2DTexture;
    public bool bOffShadowOption;
    public bool bOnShadowOption;

    private bool ChangeRot;
    

    void Update()
    {
        if (Application.isPlaying == false)
        {
            ChangeMaterialFunc();
            ChangeRot3D();
            ChangeRot3DZero();
            SideRandom();
            RotateZ();
            SaveCurMaterial();
            OffShadowOption();
            OnShadowOption();

        }
    }


    void ChangeMaterialFunc()
    {
        if (BeforeMaterial != ChangeMaterial)
        {
            transform.Find("Root3D").GetComponent<MeshRenderer>().material = ChangeMaterial;
            BeforeMaterial = ChangeMaterial;
        }
    }

    void SaveCurMaterial()
    {
        if (SaveCurMat)
            ChangeMaterial = transform.Find("Root3D").GetComponent<MeshRenderer>().sharedMaterial;
    }


    void ChangeRot3D()
    {
        if (RotMinus || RotPlus)
            ChangeRot = true;

        if (ChangeRot)
        {
            float RotValue = 0;

            if (RotPlus)
                RotValue = 90;
            else if (RotMinus)
                RotValue = -90;

            transform.Find("Root3D").transform.Rotate(new Vector3(0, RotValue, 0));
            
            ChangeRot = false;
           
        }
    }


    void ChangeRot3DZero()
    {

        if (RotZero)
        {
            transform.Find("Root3D").transform.rotation = Quaternion.Euler (0, 0, 0);
        }
    }

    //빌드할때 문제있어서 주석침
    void SideRandom() 
    {
        if (UseSideRandom)
        {
            string AssetPath = "Assets/Art/Tile/Texture/";
            string ThemeString = "Snow/Materials/Tile_Snow_";
            string RandomTile = "0";
            string TileFormat = ".mat";



            int RandomNum = Random.Range(0, 9);

            if (RandomNum == 0)
                RandomTile = 5.ToString();
            else if (RandomNum == 1)
                RandomTile = 7.ToString();
            else if (RandomNum == 2)
                RandomTile = 10.ToString();
            else if (RandomNum == 3)
                RandomTile = 11.ToString();
            else if (RandomNum == 4)
                RandomTile = 12.ToString();
            else if (RandomNum == 5)
                RandomTile = 18.ToString();
            else if (RandomNum == 6)
                RandomTile = 19.ToString();
            else if (RandomNum == 7)
                RandomTile = 22.ToString();
            else if (RandomNum == 8)
                RandomTile = 23.ToString();
            else if (RandomNum == 9)
                RandomTile = 25.ToString();



            if (Theme == ThemeType.Grass)
                ThemeString = "Grass/Materials/Tile_Grass_";
            else
                ThemeString = "Snow/Materials/Tile_Snow_";


            ChangeMaterial = (Material)AssetDatabase.LoadAssetAtPath(AssetPath + ThemeString + RandomTile + TileFormat, typeof(Material));
            ChangeMaterialFunc();
        }
    }

    void RotateZ()
    {
        if (RotZ)
        {
            transform.Find("Root3D").transform.Rotate(new Vector3(0, 0, 180));
        }
    }
    
    //타일도 투디텍스쳐 쓰고싶은데 잠김...
    void IsUse2DTexture()
    {

        if (Use2DTexture)
        {
            //transform.Find("Root3D").gameObject.GetComponent<CSingleObject2D3D>().IsUse2DTexture = true;
        }
    }

    void OffShadowOption()
    {
        if (bOffShadowOption)
        {
            transform.Find("Root3D").GetComponent<MeshRenderer>().receiveShadows = false;
            transform.Find("Root3D").GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        }
    }

    void OnShadowOption()
    {
        if (bOnShadowOption)
        {
            transform.Find("Root3D").GetComponent<MeshRenderer>().receiveShadows = true;
            transform.Find("Root3D").GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        }
    }
}

#endif