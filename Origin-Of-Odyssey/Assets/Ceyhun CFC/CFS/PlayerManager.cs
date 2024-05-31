using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Proyecto26;

public class PlayerManager : MonoBehaviourPunCallbacks
{
	PhotonView PV;
	GameManager _GameManager;
	string PlayerID;
	string Nickname;
	string[] Deck;

	void Start()
	{
		PV = GetComponent<PhotonView>();
		_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		if (PV.IsMine)
		{
			string PlayerDeckTotal = "";

			for (int i = 0; i < AuthManager.playerDeckArray.Length; i++)
			{
			 	PlayerDeckTotal += AuthManager.playerDeckArray[i] + ",";
			}

			PlayerID = PhotonNetwork.LocalPlayer.ActorNumber.ToString();
			Nickname = AuthManager.userName;
			Deck = AuthManager.playerDeckArray;


			PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);



			_GameManager.SendData(PlayerID, Nickname, Deck);

		
		}

	}

}