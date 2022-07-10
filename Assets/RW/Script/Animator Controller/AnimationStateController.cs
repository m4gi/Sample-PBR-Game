using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.game.pbr
{

    public class AnimationStateController : MonoBehaviour
    {
        Animator ani;
        private int isWalkingHash;
        private int isRunningHash;
        private int isAttack1Hash;
        private int isAttack2Hash;

        PhotonView photonView;
        void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }
        // Start is called before the first frame update
        void Start()
        {
            ani = GetComponent<Animator>();
            isWalkingHash = Animator.StringToHash("isWalking");
            isRunningHash = Animator.StringToHash("isRunning");
            isAttack1Hash = Animator.StringToHash("isAttack1");
            isAttack2Hash = Animator.StringToHash("isAttack2");

        }

        // Update is called once per frame
        void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            bool isRunning = ani.GetBool(isRunningHash);
            bool isWalking = ani.GetBool(isWalkingHash);
            bool isAttack1 = ani.GetBool(isAttack1Hash);
            bool isAttack2 = ani.GetBool(isAttack2Hash);
            bool forwardPressed = Input.GetKey(KeyCode.W);
            bool hehindPressed = Input.GetKey(KeyCode.S);
            bool leftPressed = Input.GetKey(KeyCode.A);
            bool rightPressed = Input.GetKey(KeyCode.D);
            bool runPressed = Input.GetKey(KeyCode.LeftShift);
            bool fire1Pressed = Input.GetMouseButton(0);
            bool fire2Pressed = Input.GetMouseButton(1);

            // if player presses W key
            if (!isWalking && (forwardPressed || hehindPressed || leftPressed || rightPressed))
            {
                //then set the isWalking boolean to be true
                ani.SetBool(isWalkingHash, true);
                
            }
            // if player not presses W key
            if (isWalking && !(forwardPressed || hehindPressed || leftPressed || rightPressed))
            {
                //then set the isWalking boolean to be false
                ani.SetBool(isWalkingHash, false);
            }
            // if player is walking and not running and presses left shift
            if (!isRunning && ((forwardPressed || hehindPressed || leftPressed || rightPressed) && runPressed))
            {
                //then set the isRunning boolean to be true
                ani.SetBool(isRunningHash, true);

            }
            // if player is running and stops running or stops walking
            if (isRunning && (!(forwardPressed || hehindPressed || leftPressed || rightPressed) || !runPressed))
            {
                //then set the isRunning boolean to be false
                ani.SetBool(isRunningHash, false);
            }
            // if player is presses mouse 1
            if (!isAttack1 && fire1Pressed)
            {
                // then set the isAttack1 boolean to be true
                ani.SetBool(isAttack1Hash, true);
            }
            // if player is not presses mouse 1
            if (isAttack1 && !fire1Pressed)
            {
                // then set the isAttack1 boolean to be false
                ani.SetBool(isAttack1Hash, false);
            }

            // if player is presses mouse 2
            if (!isAttack2 && fire2Pressed)
            {
                // then set the isAttack1 boolean to be true
                ani.SetBool(isAttack2Hash, true);
            }
            // if player is not presses mouse 2
            if (isAttack2 && !fire2Pressed)
            {
                // then set the isAttack1 boolean to be false
                ani.SetBool(isAttack2Hash, false);
            }
        }

    }

}
