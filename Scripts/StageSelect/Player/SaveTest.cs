using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SaveTest : MonoBehaviour
{
    private bool a = false;
    private int b = 0;
    private float c = 0f;
    private float d = 0f;

    [Serializable]
    public class Data
    {
        public bool a;
        public int b;
        public float c;
        public float d;
        public Data f;
    }

    [Serializable]
    public struct SData
    {
        public bool a;
        public int b;
        public List<float> c;

        public SData(int _a) { a = true; b = 1; c = new List<float>(); c.Add(1f); c.Add(2f); c.Add(3f); }
    }

    [Serializable]
    public class CData
    {
        public bool a;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            SaveData();
        else if (Input.GetKeyDown(KeyCode.F6))
            LoadData();

        if (Input.GetKeyDown(KeyCode.A))
            a = !a;
        else if (Input.GetKeyDown(KeyCode.B))
            b += 1;
        else if (Input.GetKeyDown(KeyCode.C))
            c += 0.5f;
    }

    private void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Data.dat");

        Data data = new Data();

        data.a = a;
        data.b = b;
        data.c = c;
        data.d = d;

        SData sdata = new SData(1);

        bf.Serialize(file, data);
        bf.Serialize(file, sdata);
        file.Close();
    }

    private void LoadData()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Data.dat", FileMode.Open);

            if (file != null && file.Length > 0)
            {
                Data data = (Data)bf.Deserialize(file);
                SData sdata = (SData)bf.Deserialize(file);
                CData cdata = (CData)bf.Deserialize(file);

                Debug.Log(cdata);

                a = data.a;
                b = data.b;
                c = data.c;
                d = data.d;

                Debug.Log("a : " + a);
                Debug.Log("b : " + b);
                Debug.Log("c : " + c);
                Debug.Log("d : " + d);
                if (data.f == null)
                    Debug.Log("a");

                Debug.Log("sa : " + sdata.a);
                Debug.Log("sb : " + sdata.b);
                Debug.Log("sc1 : " + sdata.c[0]);
                Debug.Log("sc2 : " + sdata.c[1]);
                Debug.Log("sc3 : " + sdata.c[2]);

                file.Close();
            }
        }
        catch(FileNotFoundException e)
        {
            Debug.Log("!!!!!!!!");
        }
        catch (Exception e)
        {
            Debug.Log("!!!!");
        }

    }
}
