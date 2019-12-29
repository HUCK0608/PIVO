using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections.Generic;

public enum EXmlDocumentNames { None, GrassStageDatas, SnowStageDatas, Setting }

public static class CDataManager
{
    /// <summary>파일이 저장되는 폴더 위치</summary>
    private static string _fileDirectoryPath = Application.persistentDataPath + "/";
    public static string FileDirectoryPath { get { return _fileDirectoryPath; } }

    /// <summary>XmlDocument 모음</summary>
    private static Dictionary<EXmlDocumentNames, XmlDocument> _xmlDocuments = new Dictionary<EXmlDocumentNames, XmlDocument>();

    /// <summary>현재 xml 문서 이름</summary>
    private static EXmlDocumentNames _currentXmlDocumentName = EXmlDocumentNames.None;

    private static bool _isSaveData = true;
    /// <summary>데이터를 저장하는지 여부</summary>
    public static bool IsSaveData { set { _isSaveData = value; } }

    /// <summary>
    /// 데이터 파일이 존재하는지 여부
    /// </summary>
    /// <param name="file">파일 이름</param>
    /// <returns></returns>
    public static bool IsHaveGameData()
    {
        FileInfo fileInfo = new FileInfo(_fileDirectoryPath + EXmlDocumentNames.GrassStageDatas.ToString("G") + ".xml");

        return fileInfo.Exists;
    }

    /// <summary>
    /// xml 문서 반환
    /// </summary>
    /// <param name="file">반환 받을 파일</param>
    /// <param name="fileMode">파일이 없을 경우 Open이면 null을, OpenOrCreate이면 새로운 파일을 생성해서 반환</param>
    /// <returns></returns>
    private static XmlDocument GetXmlDocument(EXmlDocumentNames file, FileMode fileMode = FileMode.Open)
    {
        // 저장되어 있는 xml 문서가 있다면 반환
        if (_xmlDocuments.ContainsKey(file))
        {
            _currentXmlDocumentName = file;

            return _xmlDocuments[file];
        }

        FileInfo fileInfo = new FileInfo(_fileDirectoryPath + file.ToString("G") + ".xml");

        // 해당 file이 있을 경우
        if (fileInfo.Exists)
        {
            // 아닌 경우 해당 문서를 가져오고 리스트에 저장
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileInfo.FullName);
            _xmlDocuments.Add(file, xmlDocument);

            return xmlDocument;
        }

        // 해당 file이 없고,
        // fileMode가 OpenOrCreate이면 새로운 파일 생성 후 반환
        if (fileMode.Equals(FileMode.OpenOrCreate))
        {
            // 새로운 파일 생성
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));
            _xmlDocuments.Add(file, xmlDocument);
            _currentXmlDocumentName = file;

            return xmlDocument;
        }
        // fileMode가 OpenOrCreate가 아니면 null을 반환
        else
            return null;
    }

    /// <summary>
    /// 노드 반환
    /// </summary>
    /// <param name="currentNode">현재 노드</param>
    /// <param name="nodeName">노드 이름</param>
    /// <param name="fileMode">노드가 없을 경우 Open이면 null을, OpenOrCreate이면 새로운 노드를 생성해서 반환</param>
    /// <returns></returns>
    private static XmlNode GetNode(this XmlNode currentNode, string nodeName, FileMode fileMode = FileMode.Open)
    {
        // 반환할 노드를 가져옴
        XmlNode node = currentNode.SelectSingleNode(nodeName);

        // 해당 노드가 존재하지 않고 fileMode가 OpenOrCreate이면 새로운 노드 생성 후 연결
        if(node == null && fileMode.Equals(FileMode.OpenOrCreate))
        {
            if (currentNode.OwnerDocument == null)
                node = (currentNode as XmlDocument).CreateNode(XmlNodeType.Element, nodeName, string.Empty);
            else
                node = currentNode.OwnerDocument.CreateNode(XmlNodeType.Element, nodeName, string.Empty);
            currentNode.AppendChild(node);
        }

        return node;
    }

    /// <summary>
    /// 데이터 쓰기
    /// </summary>
    /// <param name="file">xml 파일</param>
    /// <param name="nodePath">노드 경로</param>
    /// <param name="elementsName">속성들의 이름</param>
    /// <param name="datas">데이터들</param>
    public static void WritingDatas(EXmlDocumentNames file, string nodePath, string[] elementsName, string[] datas, bool isCompulsion = false)
    {
        if (!_isSaveData && !isCompulsion)
            return;

        // xml 파일 열기
        XmlDocument xmlDocument = GetXmlDocument(file, FileMode.OpenOrCreate);

        // 노드 경로를 '/' 단위로 쪼갬
        string[] nodesName = nodePath.Split('/');

        // nodePath를 따라 노드를 찾아 들어감(없을 경우 생성)
        XmlNode currentNode = xmlDocument.GetNode(nodesName[0], FileMode.OpenOrCreate);
        for (int i = 1; i < nodesName.Length; i++)
            currentNode = currentNode.GetNode(nodesName[i], FileMode.OpenOrCreate);

        // 속성 개수만큼 해당 속성의 데이터를 수정(없을 경우 생성)
        for (int i = 0; i < elementsName.Length; i++)
        {
            XmlNode elementNode = currentNode.GetNode(elementsName[i], FileMode.OpenOrCreate);
            elementNode.InnerText = datas[i];
        }
    }

    /// <summary>
    /// 데이터 읽기
    /// </summary>
    /// <param name="file">xml 파일</param>
    /// <param name="nodePath">노드 경로</param>
    /// <param name="elementsName">속성들의 이름</param>
    /// <returns></returns>
    public static string[] ReadDatas(EXmlDocumentNames file, string nodePath, string[] elementsName)
    {
        // xml 문서 가져오기
        XmlDocument xmlDocument = GetXmlDocument(file);

        // xml 문서가 없을 경우 null을 반환
        if (xmlDocument == null)
            return null;

        // 노드 경로를 '/' 단위로 쪼갬
        string[] nodesName = nodePath.Split('/');

        // nodePath를 따라 노드를 찾아 들어감
        XmlNode currentNode = xmlDocument.GetNode(nodesName[0]);
        for (int i = 1; i < nodesName.Length && currentNode != null; i++)
            currentNode = currentNode.GetNode(nodesName[i]);

        // 최종 노드가 없을 경우 null을 반환
        if (currentNode == null)
            return null;

        // 반환할 데이터 배열 초기화
        string[] datas = new string[elementsName.Length];

        // 속성들이 있을경우 데이터를 불러옴
        for(int i = 0; i < elementsName.Length; i++)
        {
            XmlNode elementNode = currentNode.GetNode(elementsName[i]);

            if (elementNode != null)
                datas[i] = elementNode.InnerText;
        }

        return datas;
    }

    /// <summary>
    /// 현재 열려있는 xml 문서를 저장
    /// <param name="isCompulsion">강제 저장 여부</param>
    /// </summary>
    public static void SaveCurrentXmlDocument(bool isCompulsion = false)
    {
        if (!_isSaveData && !isCompulsion)
            return;

        if (_currentXmlDocumentName.Equals(EXmlDocumentNames.None))
            return;

        _xmlDocuments[_currentXmlDocumentName].Save(_fileDirectoryPath + _currentXmlDocumentName.ToString("G") + ".xml");
    }
}
