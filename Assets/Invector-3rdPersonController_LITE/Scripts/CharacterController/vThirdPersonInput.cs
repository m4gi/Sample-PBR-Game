using Photon.Pun;
using UnityEngine;

namespace Invector.vCharacterController
{
    public class vThirdPersonInput : MonoBehaviour
    {
        #region Variables       

        [Header("Controller Input")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;

        [Header("Camera Input")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        [HideInInspector] public vThirdPersonController cc;
        [HideInInspector] public vThirdPersonCamera tpCamera;
        [HideInInspector] public Camera cameraMain;

        PhotonView PV;

        //Car
        //protected CarController carController;
        protected SkinnedMeshRenderer Renderer;
        #endregion

        protected virtual void Start()
        {
            PV = GetComponent<PhotonView>();
            if (PV.IsMine)
            {
                Renderer = GetComponentInChildren<SkinnedMeshRenderer>();
                InitilizeController();
                InitializeTpCamera();
            }
            else
            {
                return;
            }
        }

        protected virtual void FixedUpdate()
        {
            if (PV.IsMine)
            {
                cc.UpdateMotor();               // updates the ThirdPersonMotor methods
                cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
                cc.ControlRotationType();       // handle the controller rotation type
            }
        }

        protected virtual void Update()
        {
            if (PV.IsMine)
            {
                InputHandle();                  // update the input methods
                cc.UpdateAnimator();            // updates the Animator Parameters
            }
        }

        public virtual void OnAnimatorMove()
        {
            cc.ControlAnimatorRootMotion(); // handle root motion animations 
        }

        #region Basic Locomotion Inputs

        protected virtual void InitilizeController()
        {
            cc = GetComponent<vThirdPersonController>();
            if (cc != null)
                cc.Init();
        }

        protected virtual void InitializeTpCamera()
        {
            if (tpCamera == null)
            {
                tpCamera = GetComponentInChildren<vThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }
        }

        protected virtual void InputHandle()
        {

            MoveInput();
            CameraInput();
            SprintInput();
            StrafeInput();
            JumpInput();
        }

        public virtual void MoveInput()
        {
            cc.input.x = Input.GetAxis(horizontalInput);
            cc.input.z = Input.GetAxis(verticallInput);
        }

        protected virtual void CameraInput()
        {
            if (!cameraMain)
            {
                if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
                else
                {
                    cameraMain = Camera.main;
                    cc.rotateTarget = cameraMain.transform;
                }
            }

            if (cameraMain)
            {
                cc.UpdateMoveDirection(cameraMain.transform);
            }

            if (tpCamera == null)
                return;

            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);

            tpCamera.RotateCamera(X, Y);
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
                cc.Strafe();
        }

        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput))
                cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        /// <summary>
        /// Conditions to trigger the Jump animation & behavior
        /// </summary>
        /// <returns></returns>
        protected virtual bool JumpConditions()
        {
            return cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping && !cc.stopMove;
        }

        /// <summary>
        /// Input to trigger the Jump 
        /// </summary>
        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput) && JumpConditions())
                cc.Jump();
        }

        //protected virtual void CarUpdate()
        //{
        //    if (carController == null)
        //    {
        //        if (Input.GetKeyDown(KeyCode.E))
        //        {
        //            print("Press E");
        //            var colliders = Physics.OverlapSphere(transform.position, 2f);
        //            foreach (var collider in colliders)
        //            {
        //                var car = collider.GetComponent<CarController>();
        //                if (car != null)
        //                {
        //                    carController = car;
        //                    Renderer.gameObject.SetActive(false);
        //                }
        //            }
        //        }

        //    }
        //    else
        //    {
        //        carController.Move(Input.GetAxis(horizontalInput), Input.GetAxis(verticallInput), Input.GetAxis(verticallInput), 0);
        //        transform.position = carController.transform.position;
        //    }
        //}

        #endregion       
    }
}