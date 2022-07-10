using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.game.pbr
{
    

    public class PlayerMovement : MonoBehaviour
    {
        Rigidbody rb;
        PhotonView photonView;
        [SerializeField] 
        private float speed;
     
        void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Vector3 moveBy = transform.right * x + transform.forward * z;
            rb.MovePosition(transform.position + moveBy.normalized * speed * Time.deltaTime);
        }
    }
}