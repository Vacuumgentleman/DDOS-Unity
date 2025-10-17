using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ApiClient api;
    [SerializeField] private List<PlayerController> players;
    public string gameId;

    private void Start()
    {
        api.OnDataReceived += OnDataReceived;

        // Iniciar envío automático para todos los jugadores
        for (int i = 0; i < players.Count; i++)
        {
            int playerId = i;
            PlayerController player = players[playerId];

            api.StartSendingPlayerPosition(
                gameId,
                playerId,
                player.GetPosition
            );
        }
    }

    public void GetPlayerData(int playerId)
    {
        StartCoroutine(api.GetPlayerData(gameId, playerId.ToString()));
    }

    public void OnDataReceived(int playerId, ServerData data)
    {
        Vector3 position = new Vector3(data.posX, data.posY, data.posZ);
        players[playerId].MovePlayer(position);
    }

    public void SendPlayerPosition(int playerId)
    {
        Vector3 position = players[playerId].GetPosition();
        ServerData data = new ServerData
        {
            posX = position.x,
            posY = position.y,
            posZ = position.z
        };
        StartCoroutine(api.PostPlayerData(gameId, playerId.ToString(), data));
    }
}