using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.game.pbr
{
    public class PlayerCamera : MonoBehaviour
    {
        public GameObject player;
        private Vector3 offset;


        [SerializeField]
        float rotationSpeed = 3;

        [SerializeField]
        bool rotationCam = true;

        PhotonView photonView;
        void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }
        void Start()
        {
            offset = transform.position - player.transform.position;
        }


        private void LateUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            transform.position = player.transform.position + offset;

            if (rotationCam)
            {
                Quaternion turnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X")
                * rotationSpeed, Vector3.up);
                offset = turnAngle * offset;
            }

            transform.LookAt(player.transform);

        }
    }

}