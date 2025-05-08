using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class EncryptedJSONSaveSystem
{
    private static readonly string fileName = "save.dat";
    private static readonly string key = "yjd7HnM90!xpQw54"; // Clave de ejemplo, se puede mover a configuración

    public static void Save(GameData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data);
            byte[] encrypted = Encrypt(json, key);
            File.WriteAllBytes(GetPath(), encrypted);
            Debug.Log("Game saved with encryption.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving encrypted data: " + e.Message);
        }
    }

    public static GameData Load()
    {
        try
        {
            if (!File.Exists(GetPath())) return null;

            byte[] encrypted = File.ReadAllBytes(GetPath());
            string json = Decrypt(encrypted, key);
            return JsonUtility.FromJson<GameData>(json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading encrypted data: " + e.Message);
            return null;
        }
    }

    public static void Delete()
    {
        if (File.Exists(GetPath()))
        {
            File.Delete(GetPath());
            Debug.Log("Encrypted save deleted.");
        }
    }

    private static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    private static byte[] Encrypt(string data, string secretKey)
    {
        using (Aes aes = Aes.Create())
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
            using (SHA256 sha256 = SHA256.Create())
                keyBytes = sha256.ComputeHash(keyBytes);

            aes.Key = keyBytes;
            aes.GenerateIV();

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(aes.IV, 0, aes.IV.Length);
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                    sw.Write(data);

                return ms.ToArray();
            }
        }
    }

    private static string Decrypt(byte[] data, string secretKey)
    {
        using (Aes aes = Aes.Create())
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
            using (SHA256 sha256 = SHA256.Create())
                keyBytes = sha256.ComputeHash(keyBytes);

            aes.Key = keyBytes;

            byte[] iv = new byte[aes.BlockSize / 8];
            Array.Copy(data, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data, iv.Length, data.Length - iv.Length);
                ms.Position = 0;
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                    return sr.ReadToEnd();
            }
        }
    }

    public static bool HasSaveData()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        return File.Exists(path);
    }

    public static void DeleteSaveData()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Archivo de guardado eliminado.");
        }
        else
        {
            Debug.Log("No se encontró archivo de guardado para eliminar.");
        }
    }



}

