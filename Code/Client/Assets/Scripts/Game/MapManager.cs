using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using Cinemachine;

public class MapManager
{
    public string MapName { get; set; }
    public Player MyPlayer { get; set; }
    public List<Player> PlayersInMap { get; set; }

    public void Init()
    {
        SceneManager.sceneLoaded += SpawnPlayers;
    }

    public void Update()
    {

    }

    public void SpawnPlayers(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == MapName)
        {
            
            foreach (Player p in PlayersInMap)
            {
                if (p.PlayerId == MyPlayer.PlayerId)
                    continue;

                Vector3 spawnPosition = new Vector3(p.PlayerState.PosX, p.PlayerState.PosY, 0);
                GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Characters/Player");
                GameObject go = GameObject.Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
                go.name = $"Player_{p.PlayerId}";
                PlayerController pc = go.GetComponent<PlayerController>();
                pc.PlayerState = p.PlayerState;
            }

            {
                Vector3 spawnPosition = new Vector3(MyPlayer.PlayerState.PosX, MyPlayer.PlayerState.PosY, 0);
                GameObject myPlayerPrefab = Resources.Load<GameObject>("Prefabs/Characters/MyPlayer");
                GameObject go = GameObject.Instantiate(myPlayerPrefab, spawnPosition, Quaternion.identity);
                go.name = $"MyPlayer_{MyPlayer.PlayerId}";
                MyPlayerController mpc = go.GetComponent<MyPlayerController>();
                mpc.PlayerState = MyPlayer.PlayerState;

                CinemachineVirtualCamera vm = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
                vm.Follow = go.transform;
                vm.LookAt = go.transform;
            }

            SceneManager.sceneLoaded -= SpawnPlayers;
        }

    }
}