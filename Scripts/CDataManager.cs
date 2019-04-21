using UnityEngine;
using System.IO;
using System.Xml;

public static class CDataManager
{
    /// <summary>XmlDocument</summary>
    private static XmlDocument _xmlDocument = null;
    /// <summary>폴더 위치</summary>
    private static string _directoryPath = Application.persistentDataPath + "/";

    /// <summary>
    /// Xml 초기화
    /// </summary>
    /// <param name="fileName">파일 이름</param>
    /// <param name="fileMode">파일 모드(OpenOrCreate만 사용(Default = FileMode.Open)</param>
    public static void InitXmlDocument(string fileName, FileMode fileMode = FileMode.Open)
    {
        XmlDocument xmlDocument = null;

        FileInfo fileInfo = new FileInfo(_directoryPath + fileName + ".xml");

        // 파일이 없을 경우
        if (!fileInfo.Exists)
        {

#if UNITY_EDITOR
            Debug.Log("Xml 파일이 존재하지 않습니다.\n" +
                      "경로 : " + _directoryPath + fileName);
#endif

            // 파일모드가 OpenOrCreate일 경우 파일을 생성
            if (fileMode.Equals(FileMode.OpenOrCreate))
            {
                xmlDocument = new XmlDocument();
                xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));

#if UNITY_EDITOR
                Debug.Log("Xml 파일구문을 생성합니다");
#endif

            }
        }
        // 파일이 있을 경우
        else
        {
            xmlDocument = new XmlDocument();
            xmlDocument.Load(fileInfo.FullName);
        }

        _xmlDocument = xmlDocument;
    }

    /// <summary>
    /// 데이터를 씀
    /// </summary>
    /// <param name="nodePath">노드 경로</param>
    /// <param name="elementsName">속성들의 이름</param>
    /// <param name="datas">데이터들</param>
    public static void WritingData(string nodePath, string[] elementsName, string[] datas)
    {
        // 노드 경로를 '/'단위로 쪼갬
        string[] nodesName = nodePath.Split('/');

        // 첫 번째 노드를 가져옴(없을 경우 생성)
        XmlNode node = _xmlDocument.GetNode(_xmlDocument, nodesName[0], FileMode.OpenOrCreate);
        // nodePath를 따라 노드를 찾아 들어감(없을 경우 생성)
        for (int i = 1; i < nodesName.Length; i++)
            node = node.GetNode(_xmlDocument, nodesName[i], FileMode.OpenOrCreate);

        // 데이터 쓰기
        int dataCount = datas.Length;
        for (int i = 0; i < dataCount; i++)
            node.SetElement(_xmlDocument, elementsName[i], datas[i]);
    }

    /// <summary>
    /// 데이터 불러오기
    /// </summary>
    /// <param name="nodePath">노드 경로</param>
    /// <param name="elementsName">속성들의 이름</param>
    /// <returns></returns>
    public static string[] LoadData(string nodePath, string[] elementsName)
    {
        string[] datas = null;

        if(_xmlDocument != null)
        {
            // 노드 경로를 '/'단위로 쪼갬
            string[] nodesName = nodePath.Split('/');

            // 첫 번째 노드를 가져옴
            XmlNode node = _xmlDocument.GetNode(_xmlDocument, nodesName[0]);
            // nodePath를 따라 노드를 찾아 들어감
            for (int i = 1; i < nodesName.Length && node != null; i++)
                node = node.GetNode(_xmlDocument, nodesName[i]);

            // 최종 노드가 존재할 경우에 진행
            if (node != null)
            {
                // 데이터 배열 크기 초기화
                datas = new string[elementsName.Length];

                for (int i = 0; i < elementsName.Length; i++)
                {
                    // 속성 노드를 가져옴
                    XmlNode elementNode = node.GetNode(_xmlDocument, elementsName[i]);

                    // 속성 노드가 존재할 경우 데이터 저장
                    if (elementNode != null)
                        datas[i] = elementNode.InnerText;
                }
            }
        }

        return datas;
    }

    /// <summary>
    /// 파일로 저장
    /// </summary>
    /// <param name="fileName">파일 이름</param>
    public static void SaveFile(string fileName)
    {
        _xmlDocument.Save(_directoryPath + fileName + ".xml");

        _xmlDocument = null;
    }

    /// <summary>
    /// 노드 반환
    /// </summary>
    /// <param name="currentNode">현재 노드</param>
    /// <param name="xmlDocument">XmlDocument</param>
    /// <param name="nodeName">노드 이름</param>
    /// <param name="fileMode">파일 모드(OpenOrCreate만 사용, Default = FileMode.Open)</param>
    private static XmlNode GetNode(this XmlNode currentNode, XmlDocument xmlDocument, string nodeName, FileMode fileMode = FileMode.Open)
    {
        XmlNode node = currentNode.SelectSingleNode(nodeName);

        if (node == null)
        {

#if UNITY_EDITOR
            Debug.Log("노드가 존재하지 않습니다.\n" +
                      "노드 이름 : " + nodeName);
#endif

            // 파일모드가 OpenOrCreate일 경우 노드를 생성
            if(fileMode.Equals(FileMode.OpenOrCreate))
            {
                node = xmlDocument.CreateNode(XmlNodeType.Element, nodeName, string.Empty);
                currentNode.AppendChild(node);

#if UNITY_EDITOR
                Debug.Log("노드를 생성합니다.\n" +
                          "노드 이름 : " + nodeName);
#endif

            }
        }

        return node;
    }

    /// <summary>
    /// 속성 설정
    /// </summary>
    /// <param name="currentNode">현재 노드</param>
    /// <param name="xmlDocument">XmlDocument</param>
    /// <param name="elementName">속성 이름</param>
    /// <param name="data">저장할 데이터</param>
    private static void SetElement(this XmlNode currentNode, XmlDocument xmlDocument, string elementName, string data)
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
