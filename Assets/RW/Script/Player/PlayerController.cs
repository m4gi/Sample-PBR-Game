using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.game.pbr
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        public GameObject player;

        private Vector3 offset;
        private Vector3 newtrans;




        [SerializeField]
        float rotationSpeed = 3;

        [SerializeField]
        bool rotationCam = true;


        Rigidbody rb;
        PhotonView photonView;
        [SerializeField]
        private float speed;

        void Awake()
        {
            photonView = GetComponent<PhotonView>();
            rb = GetComponent<Rigidbody>();
        }


        void Update()
        {
            Move();
        }

        void Start()
        {
            
            offset = transform.position - player.transform.position;
            offset.x = transform.position.x - player.transform.position.x;
            offset.z = transform.position.z - player.transform.position.z;
            newtrans = transform.position;
            //not taking y as we wont update y position. 

        }

        void LateUpdate()
        {
            Look();   
        }


        void Move()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Vector3 moveBy = transform.right * x + transform.forward * z;
            rb.MovePosition(transform.position + moveBy.normalized * speed * Time.deltaTime);
        }

        void Look()
        {
            newtrans.x = player.transform.position.x + offset.x;
            newtrans.z = player.transform.position.z + offset.z;
            transform.position = newtrans;

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

