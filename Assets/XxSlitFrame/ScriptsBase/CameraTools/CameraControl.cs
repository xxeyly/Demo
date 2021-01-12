using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using XxSlitFrame.Tools;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;

namespace CameraTools
{
    /// <summary>
    /// 相机控制
    /// </summary>
    public class CameraControl : StartSingleton
    {
        public static CameraControl Instance;

        private GameObject _cameraParent; //相机父物体,用来寻路
        [Header("相机是否可以移动")] public bool isControl; //相机是否可以移动
        [Header("视图限制")] public bool viewConstraint; //视图限制,某些视图不受移动限制,如对话框,提示框等

        [SerializeField] [Header("最小的相机领域")] private float cameraMinField; //最小的相机领域

        [SerializeField] [Header("最大的相机领域")] private float cameraMaxField; //最大的相机领域

        [SerializeField] [Header("移动速度")] private float moveSpeed = 1; //移动速度
        [SerializeField] [Header("旋转速度")] private float rotationSpeed = 2; //旋转速度

        [SerializeField] [Header("相机上旋转最高值")] [Range(0, 90)]
        private float downRange = 45; //相机上旋转最低值

        [SerializeField] [Header("相机下旋转最高值")] [Range(0, -30)]
        private float topRange = 30; //相机下旋转最高值

        [SerializeField] [Header("相机高度最小值")] [Range(0.5f, 2)]
        private float cameraHeightMin = 0.5f; //相机高度最小值

        [SerializeField] [Header("相机高度最大值")] [Range(2f, 5)]
        private float cameraHeightMax = 20; //相机高度最大值

        [HideInInspector] public Camera currentCamera; //当前相机
        private float _x;
        private float _y;
        [Header("当前位置数据")] public CameraPosData cameraPosData;

        public override void StartSvc()
        {
            Instance = GetComponent<CameraControl>();
            Init();
        }

        /// <summary>
        /// 获得当前位置信息
        /// </summary>
        /// <returns></returns>
        public void SetCurrentCameraPosInfo()
        {
            cameraPosData.SetCameraPosInfo(_cameraParent.transform.position, transform.localPosition, transform.localEulerAngles, currentCamera.fieldOfView);
#if UNITY_EDITOR
            EditorUtility.SetDirty(cameraPosData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
#endif
        }

        public override void Init()
        {
            TryScrollWheel();
            currentCamera = GetComponent<Camera>();
            _cameraParent = gameObject.GetComponentInParent<NavMeshAgent>().gameObject.gameObject;
            cameraMinField = 40;
            cameraMaxField = 60;
            ResetRot();
            ListenerSvc.Instance.AddListenerEvent<CameraPosType>(ListenerEventType.CameraMoveToTargetPos, ChangePosition);
        }

        void Update()
        {
            if (Instance != null)
            {
                // 如果有UI显示,则不能进行操作
                if (isControl /*&& !MouseSvc.Instance.GetMouseOverUI()*/)
                {
                    if (viewConstraint)
                    {
                        if (ViewSvc.Instance.GetCurrentActiveViewCount() != 0)
                        {
                            return;
                        }
                    }

                    TryRotate();
                    TryScrollWheel();
                    TryMove();
                    // ResetRot();
                }
            }
        }

        void TryMove()
        {
            float height;
            if (Input.GetKey(KeyCode.Q))
            {
                height = -1 * PersistentDataSvc.Instance.cameraSpeed;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                height = 1 * PersistentDataSvc.Instance.cameraSpeed;
            }
            else
            {
                height = 0;
            }

            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            if (Math.Abs(v) > 0 || Math.Abs(h) > 0)
            {
                _cameraParent.GetComponent<NavMeshAgent>()
                    .Move(transform.TransformDirection(moveSpeed * Time.deltaTime * new Vector3(h, 0, v) * PersistentDataSvc.Instance.cameraSpeed));
            }

            if (height < 0)
            {
                if (transform.localPosition.y > cameraHeightMin)
                {
                    transform.Translate(0, height * Time.deltaTime * 0.2f, 0);
                }
            }

            if (height > 0)
            {
                if (transform.localPosition.y < cameraHeightMax)
                {
                    transform.Translate(0, height * Time.deltaTime * 0.2f, 0);
                }
            }
        }


        /// <summary>
        /// 更改位置
        /// </summary>
        public void ChangePosition(CameraPosType cameraPosType)
        {
            CameraPosInfo cameraPosInfo = cameraPosData.GetCameraPosInfoByCameraPosType(cameraPosType);
            cameraPosData.currentCameraPosType = cameraPosType;
            _cameraParent.GetComponent<NavMeshAgent>().enabled = false;
            if (_cameraParent.transform.position != cameraPosInfo.navMeshAgentPos)
            {
                _cameraParent.transform.position = cameraPosInfo.navMeshAgentPos;
            }

            if (transform.localPosition != cameraPosInfo.cameraPos)
            {
                transform.localPosition = cameraPosInfo.cameraPos;
            }

            transform.localEulerAngles = cameraPosInfo.cameraRot;
            _cameraParent.GetComponent<NavMeshAgent>().enabled = true;
            currentCamera.fieldOfView = cameraPosInfo.cameraFieldView;
            ResetRot();
        }


        /// <summary>
        /// 改变相机的领域
        /// </summary>
        public void ChangeCameraField(float field)
        {
            transform.GetComponent<Camera>().fieldOfView = field;
        }


        /// <summary>
        /// 重置相机的旋转值
        /// </summary>
        private void ResetRot()
        {
            _x = transform.localEulerAngles.y;
            if (transform.localEulerAngles.x > Mathf.Abs(downRange) + 0.1f)
            {
                _y = transform.localEulerAngles.x - 360;
            }
            else
            {
                _y = transform.localEulerAngles.x;
            }
        }

        /// <summary>
        /// 尝试更改相机的领域
        /// </summary>
        void TryScrollWheel()
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (currentCamera.fieldOfView < cameraMaxField)
                    currentCamera.fieldOfView += 2 * PersistentDataSvc.Instance.cameraSpeed;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (currentCamera.fieldOfView > cameraMinField)
                    currentCamera.fieldOfView -= 2 * PersistentDataSvc.Instance.cameraSpeed;
            }
        }

        /// <summary>
        /// 尝试旋转
        /// </summary>
        void TryRotate()
        {
            if (Input.GetMouseButton(1))
            {
                _x += Input.GetAxis("Mouse X") * rotationSpeed * PersistentDataSvc.Instance.cameraSpeed;
                _y -= Input.GetAxis("Mouse Y") * rotationSpeed * PersistentDataSvc.Instance.cameraSpeed;
                _y = ClampAngle(_y, downRange, topRange);
//            Quaternion rotation1 = Quaternion.Euler(y, x, 0.0f);
                transform.localEulerAngles = new Vector3(_y, _x, 0f);
//            transform.localRotation = rotation1;
            }
        }

        /// <summary>
        /// 修正夹角
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="down"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        private float ClampAngle(float angle, float down, float top)
        {
            if (angle > 0)
            {
                if (angle < down)
                {
                    return angle;
                }

                return down;
            }

            if (angle < 0)
            {
                if (angle > top)
                {
                    return angle;
                }

                return top;
            }

            return angle;
        }

        /// <summary>
        /// 返回当前相机
        /// </summary>
        /// <returns></returns>
        public UnityEngine.Camera GetCurrentWorldCamera()
        {
            return GetComponent<UnityEngine.Camera>();
        }
    }
}