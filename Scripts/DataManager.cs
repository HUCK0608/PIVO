using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

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


    /// <summary>
    /// 데이터 저장
    /// </summary>
    /// <param name="fileName">파일 이름</param>
    /// <param name="nodePath">노드 경로</param>
    /// <param name="elementName">속성 이름</param>
    /// <param name="dataName">데이터 이름</param>
    /// <param name="data">데이터</param>
    public static void SaveData(string fileName, string nodePath, string elementName, string data)
    {
        GetXmlDocument(fileName);

        string[] nodesName = nodePath.Split('/');

        XmlNode node = xmlDocument.GetNode(nodesName[0]);
        for(int i = 1; i < nodesName.Length; i++)
            node = node.GetNode(nodesName[i]);

        node.SetElement(elementName, data);

        xmlDocument.Save("./Assets/Resources/Datas/" + fileName + ".xml");
    }

    /// <summary>
    /// 데이터 불러오기
    /// </summary>
    /// <param name="fileName">파일 이름</param>
    /// <param name="nodePath">노드 경로</param>
    /// <param name="elementName">속성 이름</param>
    /// <returns></returns>
    public static string LoadData(string fileName, string nodePath, string elementName)
    {
        GetXmlDocument(fileName);

        string[] nodesName = nodePath.Split('/');

        XmlNode node = xmlDocument.GetNode(nodesName[0]);
        for (int i = 1; i < nodesName.Length; i++)
            node = node.GetNode(nodesName[i]);

        string data = node.InnerText;

        return data;
    }

    /// <summary>
    /// XML 초기화
    /// </summary>
    /// <param name="fileName">파일 이름</param>
    private static XmlDocument GetXmlDocument(string fileName)
    {
        XmlDocument xmlDocument = new XmlDocument();

        TextAsset textAsset = (TextAsset)Resources.Load("Datas/" + fileName);

        if (textAsset == null)
        {
#if UNITY_EDITOR
            Debug.Log("XML 파일이 존재하지 않습니다.\n" +
                      "경로 : " + _directoryPath + fileName);
#endif
            return xmlDocument;
        }

        xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(textAsset.text);

        return xmlDocument;
    }

    /// <summary>
    /// 노드를 반환
    /// </summary>
    /// <param name="currentNode">현재 노드</param>
    /// <param name="nodeName">노드 이름</param>
    /// <returns></returns>
    private static XmlNode GetNode(this XmlNode currentNode, string nodeName)
    {
        XmlNode node = currentNode.SelectSingleNode(nodeName);

        if (node == null)
        {
            node = xmlDocument.CreateNode(XmlNodeType.Element, nodeName, string.Empty);
            currentNode.AppendChild(node);

#if UNITY_EDITOR
            Debug.Log("노드가 존재하지 않아 노드를 생성합니다.\n" +
                      "생성된 노드 : " + nodeName);
#endif
        }

        return node;
    }

    /// <summary>
    /// 속성 설정
    /// </summary>
    /// <param name="currentNode">현재 노드</param>
    /// <param name="elementName">속성 이름</param>
    /// <param name="data">저장할 데이터</param>
    private static void SetElement(this XmlNode currentNode, string elementName, string data)
    {
        XmlNode elementNode = currentNode.SelectSingleNode(elementName);

        // 저장된 데이터가 없을 경우 생성
        if(elementNode == null)
        {
            XmlElement element = xmlDocument.CreateElement(elementName);
            element.InnerText = data;
            currentNode.AppendChild(element);

#if UNITY_EDITOR
            Debug.Log("데이터가 존재하지 않아 속성을 생성합니다.\n" +
                      "생성된 속성 : " + elementName);
#endif
        }
        // 저장된 데이터가 있을 경우 수정
        else
        {
            elementNode.InnerText = data;
        }
    }
}
