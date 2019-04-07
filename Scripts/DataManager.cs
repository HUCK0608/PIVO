using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class DataManager
{
    /// <summary>BinaryFormmater</summary>
    private static BinaryFormatter _binaryFormatter = new BinaryFormatter();

    /// <summary>폴더 위치</summary>
    private static string _directoryPath = Application.persistentDataPath + "/Datas/";

    /// <summary>데이터 저장</summary>
    public static void SaveData<T>(T data, string fileName)
    {
        try
        {
            FileStream file = new FileStream(_directoryPath + fileName, FileMode.OpenOrCreate);

            _binaryFormatter.Serialize(file, data);
            file.Close();
        }
        catch(DirectoryNotFoundException)
        {
            CreateDataDirectory();

            SaveData<T>(data, fileName);
        }
    }

    /// <summary>데이터 불러오기</summary>
    public static T LoadData<T>(string fileName)
    {
        T data = default(T);

        try
        {
            FileStream file = new FileStream(Application.persistentDataPath + "/Datas/" + fileName, FileMode.Open);

            data = (T)_binaryFormatter.Deserialize(file);
            file.Close();
        }
        catch(DirectoryNotFoundException)
        {
            CreateDataDirectory();

            data = LoadData<T>(fileName);
        }
        catch(FileNotFoundException)
        {
#if UNITY_EDITOR
            Debug.Log(fileName + "파일이 존재하지 않습니다.");
#endif
        }

        return data;
    }

    /// <summary>Datas폴더 생성</summary>
    private static void CreateDataDirectory()
    {
#if UNITY_EDITOR
        Debug.Log("폴더가 존재하지 않아 새로 생성합니다.\n" +
                  "경로 : " + _directoryPath);
#endif

        DirectoryInfo directoryInfo = new DirectoryInfo(_directoryPath);

        directoryInfo.Create();
    }
}
