using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
namespace com.game.pbr
{
    public class Launcher : MonoBehaviourPunCallbacks
    {

        #region Comment
        /*
                #region Public Fields

                [SerializeField]
                private GameObject controlPanel;

                [SerializeField]
                private Text feedbackText;

                [SerializeField]
                private byte maxPlayersPerRoom = 4;

                private bool isConnecting;
                private string gameVersion = "1";

                [Space(10)]
                [Header("Custom Variables")]
                public InputField playerNameField;
                public InputField roomNameField;

                [Space(5)]
                public Text playerStatus;
                public Text connectionStatus;

                [Space(5)]
                public GameObject roomJoinUI;
                public GameObject buttonLoadArena;
                public GameObject buttonJoinRoom;


                string playerName = "";
                string roomName = "";
                #endregion




                #region MonoBehaviour Callbacks

                private void Awake()
                {
                    PhotonNetwork.AutomaticallySyncScene = true;
                }


                private void Start()
                {
                    PlayerPrefs.DeleteAll();

                    Debug.Log("Connecting to Photon");

                    roomJoinUI.SetActive(false);
                    buttonLoadArena.SetActive(false);

                    ConnectToPhoton();
                }

                #endregion

                #region Helper Methods

                public void SetPlayerName(string name)
                {
                    playerName = name;
                }

                public void SetRoomName(string name)
                {
                    roomName = name;
                }

                #endregion

                #region Photon Method

                private void ConnectToPhoton()
                {
                    connectionStatus.text = "Connecting...";
                    PhotonNetwork.GameVersion = gameVersion;
                    PhotonNetwork.ConnectUsingSettings();
                }

                public void JoinRoom()
                {
                    if (PhotonNetwork.IsConnected)
                    {
                        PhotonNetwork.LocalPlayer.NickName = playerName; //1
                        Debug.Log("PhotonNetwork.IsConnected! | Trying to Create/Join Room " + roomNameField.text);
                        RoomOptions roomOptions = new RoomOptions(); //2
                        roomOptions.MaxPlayers = maxPlayersPerRoom;
                        TypedLobby typedLobby = new TypedLobby(roomName, LobbyType.Default); //3
                        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby); //4
                    }
                }


                public override void OnConnected()
                {

                    base.OnConnected();

                    connectionStatus.text = "Connected to Photon!";
                    connectionStatus.color = Color.green;
                    roomJoinUI.SetActive(true);
                    buttonLoadArena.SetActive(false);
                }

                public override void OnDisconnected(DisconnectCause cause)
                {

                    isConnecting = false;
                    controlPanel.SetActive(true);
                    Debug.LogError("Disconnected. Please check your Internet connection.");
                }

                public override void OnJoinedRoom()
                {

                    if (PhotonNetwork.IsMasterClient)
                    {
                        buttonLoadArena.SetActive(true);
                        buttonJoinRoom.SetActive(false);
                        playerStatus.text = "Your are Lobby Leader";
                    }
                    else
                    {
                        playerStatus.text = "Connected to Lobby";
                    }
                }
                #endregion

                #region Public Method

                public void LoadArena()
                {

                    if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
                    {
                        PhotonNetwork.LoadLevel("");
                    }
                    else
                    {
                        playerStatus.text = "Minimum 2 Players required to Load Arena!";
                    }
                }

                #endregion

                */


        #endregion

        #region Private Serializable Fiedlds
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 15;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;
        #endregion



        #region Private Fields

        private string gameVersion = "1";
        private bool isConnecting;
        #endregion



        #region MonoBehavior CallBacks


        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }
        #endregion



        #region Public Method

        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }

        }

        #endregion


        #region MonoBehaviourPunCallBacks CallBacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN/Launcher: OnConnectedToMaster() was called by PUN");
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }

        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            isConnecting = false;
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("Loading... the Game");


                // #Critical
                // Load the Room Level
                PhotonNetwork.LoadLevel("Game");
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayersPerRoom;
            PhotonNetwork.CreateRoom(null, roomOptions);
        }


        #endregion
    }

}