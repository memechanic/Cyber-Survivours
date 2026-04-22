using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [Serializable]
    public class SaveData
    {
        public int coins;
        public List<int> unlockedCharacters = new() { 0 };
    }

    public SaveData data;

    private string path;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            path = Application.persistentDataPath + "/save.dat";
            data = Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        byte[] encrypted = SaveEncryption.Encrypt(json);
        File.WriteAllBytes(path, encrypted);
    }

    public void ResetSave()
    {
        data = new SaveData();
        Save();
    }

    public SaveData Load()
    {
        if (File.Exists(path))
        {
            try
            {
                byte[] fileData = File.ReadAllBytes(path);
                string json = SaveEncryption.Decrypt(fileData);
                return JsonUtility.FromJson<SaveData>(json);
            }
            catch
            {
                return new SaveData();
            }
        }

        return new SaveData();
    }

    void OnApplicationQuit()
    {
        Save();
    }

    private static class SaveEncryption
    {
        private static readonly string password = "kapibara_is_the_best";

        private static byte[] GetKey()
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static byte[] Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = GetKey();
                aes.GenerateIV();

                byte[] iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    ms.Write(iv, 0, iv.Length);

                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    return ms.ToArray();
                }
            }
        }

        public static string Decrypt(byte[] data)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = GetKey();

                byte[] iv = new byte[16];
                Buffer.BlockCopy(data, 0, iv, 0, 16);

                aes.IV = iv;

                byte[] encrypted = new byte[data.Length - 16];
                Buffer.BlockCopy(data, 16, encrypted, 0, encrypted.Length);

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream(encrypted))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
