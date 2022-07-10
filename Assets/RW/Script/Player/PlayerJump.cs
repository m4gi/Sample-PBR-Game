using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.game.pbr
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerJump : MonoBehaviour
    {

        //public Vector3 jump;

        public bool isGrounded;
        Rigidbody rb;

        [SerializeField]
        float jumpForce;

        PhotonView photonView;
        void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnCollisionStay(Collision collision)
        {
            isGrounded = true;
        }



        void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }

}