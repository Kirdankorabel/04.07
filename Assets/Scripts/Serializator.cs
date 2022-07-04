using UnityEngine;

public static class Serializator
{
    public static void Serialize(string key, object data)
    {
        string jsData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsData);
        Debug.Log(2);
    }
    public static T DeSerialize<T>(string key) where T: new()
    {
        if(PlayerPrefs.HasKey(key))
        {
            string data = PlayerPrefs.GetString(key);
            Debug.Log(1);
            return JsonUtility.FromJson<T>(data);
        }
        return new T();
    }
}
