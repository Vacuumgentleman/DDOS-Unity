using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    public string baseUrl = "http://localhost:5005/server";

    public event Action<int, ServerData> OnDataReceived;

    private Dictionary<int, Coroutine> positionCoroutines = new Dictionary<int, Coroutine>();

    public IEnumerator GetPlayerData(string gameId, string playerId)
    {
        string url = $"{baseUrl}/{gameId}/{playerId}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"GET Error: {webRequest.error}");
                Debug.LogError($"Response: {webRequest.downloadHandler.text}");
            }
            else
            {
                Debug.Log($"GET Success: {webRequest.downloadHandler.text}");
                var data = JsonUtility.FromJson<ServerData>(webRequest.downloadHandler.text);
                OnDataReceived?.Invoke(Convert.ToInt16(playerId), data);
            }
        }
    }

    // POST request
    public IEnumerator PostPlayerData(string gameId, string playerId, ServerData data)
    {
        string url = $"{baseUrl}/{gameId}/{playerId}";
        string jsonData = JsonUtility.ToJson(data);

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"POST Error: {webRequest.error}");
                Debug.LogError($"Response: {webRequest.downloadHandler.text}");
            }
            else
            {
                Debug.Log($"POST Success: {webRequest.downloadHandler.text}");
            }
        }
    }

    // Iniciar envío periódico (corutina saturadora)
    public void StartSendingPlayerPosition(string gameId, int playerId, Func<Vector3> getPositionFunc)
    {
        if (positionCoroutines.ContainsKey(playerId))
        {
            Debug.LogWarning($"Coroutine for Player {playerId} already running.");
            return;
        }

        Coroutine coroutine = StartCoroutine(SendPlayerPositionCoroutine(gameId, playerId, getPositionFunc));
        positionCoroutines.Add(playerId, coroutine);
    }

    // Corutina que envía 10 peticiones simultáneas cada 100 ms sin esperar respuestas (saturación)
    private IEnumerator SendPlayerPositionCoroutine(string gameId, int playerId, Func<Vector3> getPositionFunc)
    {
        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 pos = getPositionFunc();
                ServerData data = new ServerData
                {
                    posX = pos.x + i,  // pequeña variación para simular cambio
                    posY = pos.y,
                    posZ = pos.z
                };

                // fire-and-forget
                StartCoroutine(PostPlayerData(gameId, playerId.ToString(), data));
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    // Detener si es necesario
    public void StopSendingPlayerPosition(int playerId)
    {
        if (positionCoroutines.TryGetValue(playerId, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            positionCoroutines.Remove(playerId);
        }
    }
}