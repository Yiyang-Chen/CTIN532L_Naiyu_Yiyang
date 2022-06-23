using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


/// <summary>
/// streaming Ҫ����build��Ķ����� persistent ������̨�����ϵ�һЩ�ļ� temporary ��ʱcache DataPath editor��runtime���᲻һ��
/// </summary>
public enum DataPath { STREAMING, PERSISTENT, TEMPORARY, DEFAULT };


public class DataMgr : SingletonManager<DataMgr>
{
    
    /// <summary>
    /// ����modeѡ��·��
    /// </summary>
    private string pathMode(DataPath mode)
    {
        string _path = "";

        switch (mode)
        {
            case DataPath.STREAMING:
                _path += Application.streamingAssetsPath;
                break;
            case DataPath.PERSISTENT:
                _path += Application.persistentDataPath;
                break;
            case DataPath.TEMPORARY:
                _path += Application.temporaryCachePath;
                break;
            default:
                _path += Application.dataPath;
                break;
        }
        return _path;
    }

    /// <summary>
    /// �洢�ļ�
    /// </summary>
    /// <typeparam name="T">data��class</typeparam>
    /// <param name="saveData">Ҫ�����data�����class��ֻ������string,bool,int,float</param>
    /// <param name="mode">DataPath.xx</param>
    /// <param name="path">string,"/xx/xx.xx"�����Ҫָ��һ��flie</param>
    public void Save<T>(T saveData, DataPath mode, string path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string finalPath = pathMode(mode) + path;
        //��using��ֹfile����
        using (FileStream stream = new FileStream(finalPath, FileMode.Create))
        {
            formatter.Serialize(stream, saveData);
            stream.Close();
        }
    }

    /// <summary>
    /// �����ļ�
    /// </summary>
    /// <typeparam name="T">data��class</typeparam>
    /// <param name="mode">DataPath.xx</param>
    /// <param name="path">string,"/xx/xx.xx"�����Ҫָ��һ��flie</param>
    /// <returns>����data�����û��ȡ��������null</returns>
    public T Load<T>(DataPath mode, string path) where T : class
    {
        T data = null;

        string finalPath = pathMode(mode) + path;
        Debug.Log(finalPath);

        if (File.Exists(finalPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            
            using (FileStream stream = new FileStream(finalPath, FileMode.Open))
            {
                data = formatter.Deserialize(stream) as T;
                stream.Close();
            }
        }

        return data;
    }

    /// <summary>
    /// ɾ��ĳ��������ļ�
    /// </summary>
    /// <param name="mode">DataPath.xx</param>
    /// <param name="path"></param>
    public void Delete(DataPath mode, string path)
    {
        string _path = pathMode(mode);

        _path += path;

        if (File.Exists(_path))
        {
            File.Delete(_path);
        }
    }
}
