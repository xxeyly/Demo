using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using XxSlitFrame.Tools.Svc;
using UnityEngine.UI;
using XxSlitFrame.Tools.ConfigData;

namespace XxSlitFrame.View
{
    public enum ViewShowType
    {
        /// <summary>
        /// 活动
        /// </summary>
        Activity,

        /// <summary>
        /// 冻结
        /// </summary>
        Frozen
    }

    public enum ShowType
    {
        //直接
        Direct,

        //渐隐
        Curve
    }

    public struct TimeTaskInfo
    {
        [Header("任务ID")] public int TimeTaskId;
        [Header("任务类型")] public TimeTaskList.TimeLoopType TimeLoopType;
        [Header("任务名称")] public string TimeTaskName;
    }

    /// <summary>
    /// 视图基类
    /// </summary>
    public abstract partial class BaseWindow : MonoBehaviour
    {
        protected GameObject Window;
        protected CanvasGroup _canvasGroup;
        protected AudioSvc audioSvc;
        protected ResSvc resSvc;
        protected ViewSvc viewSvc;
        protected SceneSvc sceneSvc;
        protected PersistentDataSvc persistentDataSvc;
        protected TimeSvc timeSvc;
        protected ListenerSvc listenerSvc;
        protected MouseSvc mouseSvc;

        [SerializeField] [Header("组件事件监听")] protected Dictionary<string, UnityAction<BaseEventData>> uiListener =
            new Dictionary<string, UnityAction<BaseEventData>>();

        [SerializeField] [Header("计时任务列表")] protected List<TimeTaskInfo> timeTaskInfoList = new List<TimeTaskInfo>();
        [Header("视图类型")] [SerializeField] protected ViewShowType ViewShowType = ViewShowType.Activity;
        [Header("显示类型")] [SerializeField] protected ShowType ShowType = ShowType.Direct;

        [Header("显示时间")] [Range(0.1f, 9)] [SerializeField]
        protected float ShowTime = 1;

        [Header("显示播放动画")] [SerializeField] protected bool isShowAnim = false;

        [Header("隐藏播放动画")] [SerializeField] protected bool isHideAnim = false;


        [HideInInspector] public Type viewType;

        /// <summary>
        /// 视图显示任务
        /// </summary>
        private int viewShowTimeTask;

        protected BaseWindow()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            viewType = InitViewType();
            // ReSharper disable once VirtualMemberCallInConstructor
            InitViewShowType();
        }


        protected virtual void Update()
        {
            if (timeTaskInfoList.Count >= 1)
            {
                foreach (TimeTaskInfo timeTaskInfo in timeTaskInfoList)
                {
                    if (!timeSvc.GetAllTimeTaskId().Contains(timeTaskInfo.TimeTaskId))
                    {
                        timeTaskInfoList.Remove(timeTaskInfo);
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// 返回当前视图的活动类型
        /// </summary>
        /// <returns></returns>
        public ViewShowType GetViewShowType()
        {
            return ViewShowType;
        }

        /// <summary>
        /// 视图初始化
        /// </summary>
        public void ViewStartInit()
        {
            Window = transform.Find("Window").gameObject;
            _canvasGroup = Window.GetComponent<CanvasGroup>();
            audioSvc = AudioSvc.Instance;
            resSvc = ResSvc.Instance;
            viewSvc = ViewSvc.Instance;
            sceneSvc = SceneSvc.Instance;
            timeSvc = TimeSvc.Instance;
            persistentDataSvc = PersistentDataSvc.Instance;
            listenerSvc = ListenerSvc.Instance;
            mouseSvc = MouseSvc.Instance;
            InitView();
            InitListener();
            OnlyOnceInit();
        }


        public abstract void Init();

        /// <summary>
        /// 初始化视图的显示类型
        /// </summary>
        protected virtual void InitViewShowType()
        {
        }

        /// <summary>
        /// UI类型设置
        /// </summary>
        protected Type InitViewType()
        {
            return GetType();
        }

        /// <summary>
        /// UI绑定
        /// </summary>
        protected abstract void InitView();

        /// <summary>
        /// 事件监听
        /// </summary>
        protected abstract void InitListener();

        /// <summary>
        /// 仅仅初始化一次
        /// </summary>
        protected virtual void OnlyOnceInit()
        {
        }

        /// <summary>
        /// 隐藏元素
        /// </summary>
        /// <param name="hideObjArray">需要隐藏的元素</param>
        protected void HideObj(params GameObject[] hideObjArray)
        {
            foreach (GameObject hideObj in hideObjArray)
            {
                hideObj.SetActive(false);
            }
        }

        /// <summary>
        /// 隐藏元素
        /// </summary>
        /// <param name="hideObjArray">需要隐藏的元素</param>
        protected void HideObj(params MaskableGraphic[] hideObjArray)
        {
            foreach (MaskableGraphic hideObj in hideObjArray)
            {
                hideObj.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 隐藏元素
        /// </summary>
        /// <param name="hideObjArray">需要隐藏的元素</param>
        protected void HideObj(params Selectable[] hideObjArray)
        {
            foreach (Selectable hideObj in hideObjArray)
            {
                hideObj.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 显示元素
        /// </summary>
        /// <param name="showObjArray">需要显示的元素</param>
        protected void ShowObj(params GameObject[] showObjArray)
        {
            foreach (GameObject showObj in showObjArray)
            {
                showObj.SetActive(true);
            }
        }

        /// <summary>
        /// 显示元素
        /// </summary>
        /// <param name="showObjArray">需要显示的元素</param>
        protected void ShowObj(params MaskableGraphic[] showObjArray)
        {
            foreach (MaskableGraphic showObj in showObjArray)
            {
                showObj.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 显示或隐藏物体
        /// </summary>
        /// <param name="display"></param>
        /// <param name="showObjArray"></param>
        protected void DisPlayObj(bool display, params GameObject[] showObjArray)
        {
            foreach (GameObject showObj in showObjArray)
            {
                showObj.SetActive(display);
            }
        }

        /// <summary>
        /// 显示或隐藏物体
        /// </summary>
        /// <param name="display"></param>
        /// <param name="showObjArray"></param>
        protected void DisPlayObj(bool display, params MaskableGraphic[] showObjArray)
        {
            foreach (MaskableGraphic showObj in showObjArray)
            {
                showObj.gameObject.SetActive(display);
            }
        }

        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="errorTips">错误提示面板</param>
        /// <param name="errorTipContent">错误提示内容文本</param>
        /// <param name="content">错误提示内容</param>
        /// <param name="action">错误提示完毕后执行事件</param>
        /// <returns></returns>
        protected int ShowErrorTip(GameObject errorTips, Text errorTipContent, string content,
            UnityAction action = null)
        {
            audioSvc.PlayEffectAudio(AudioData.AudioType.EErrorPopup);

            int errorTimeTask = timeSvc.ShowErrorTip(errorTips, errorTipContent, content, action);
            timeTaskInfoList.Add(new TimeTaskInfo()
                {TimeTaskId = errorTimeTask, TimeLoopType = TimeTaskList.TimeLoopType.Once, TimeTaskName = "错误提示"});

            return errorTimeTask;
        }

        /// <summary>
        /// 图片闪烁
        /// </summary>
        /// <param name="twinkleImage"></param>
        /// <param name="twinkleInterval"></param>
        /// <returns></returns>
        protected int ImageTwinkle(Image twinkleImage, float twinkleInterval)
        {
            int twinkleTimeTask = timeSvc.ImageTwinkle(twinkleImage, twinkleInterval);
            timeTaskInfoList.Add(new TimeTaskInfo()
                {TimeTaskId = twinkleTimeTask, TimeLoopType = TimeTaskList.TimeLoopType.Once, TimeTaskName = "图片闪烁"});
            return twinkleTimeTask;
        }

        /// <summary>
        /// 显示或隐藏物体
        /// </summary>
        /// <param name="display"></param>
        /// <param name="showObjArray"></param>
        protected void DisPlayObj(bool display, params Selectable[] showObjArray)
        {
            foreach (Selectable showObj in showObjArray)
            {
                showObj.gameObject.SetActive(display);
            }
        }

        /// <summary>
        /// 显示元素
        /// </summary>
        /// <param name="showObjArray">需要显示的元素</param>
        protected void ShowObj(params Selectable[] showObjArray)
        {
            foreach (Selectable showObj in showObjArray)
            {
                showObj.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 视图摧毁
        /// </summary>
        public void OnViewDestroy()
        {
            ViewDestroy();
        }

        /// <summary>
        /// 界面摧毁
        /// </summary>
        protected virtual void ViewDestroy()
        {
            for (int i = 0; i < timeTaskInfoList.Count; i++)
            {
                switch (timeTaskInfoList[i].TimeLoopType)
                {
                    case TimeTaskList.TimeLoopType.Once:
                        DeleteTimeTask(timeTaskInfoList[i].TimeTaskId);
                        break;
                    case TimeTaskList.TimeLoopType.Loop:
                        DeleteSwitchTask(timeTaskInfoList[i].TimeTaskId);
                        break;
                    case TimeTaskList.TimeLoopType.Immortal:
                        DeleteImmortalTimeTask(timeTaskInfoList[i].TimeTaskId);
                        break;
                }
            }

            timeTaskInfoList.Clear();
        }

        /// <summary>
        /// 隐藏视图
        /// </summary>
        protected void HideView()
        {
            viewSvc.HideView(viewType);
        }

        /// <summary>
        /// 显示视图
        /// </summary>
        protected void ShowView()
        {
            viewSvc.ShowView(viewType);
        }

        /// <summary>
        /// 获得当前视图的显示状态
        /// </summary>
        /// <returns></returns>
        public bool GetDisplay()
        {
            return Window.activeInHierarchy;
        }

        /// <summary>
        /// 更改透明度
        /// </summary>
        /// <param name="apache"></param>
        private void ChangeApache(float apache)
        {
            if (apache <= 0)
            {
                apache = 0;
            }
            else if (apache >= 1)
            {
                apache = 1;
            }

            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = apache;
            }
        }

        /// <summary>
        /// 设置当前视图关闭或者隐藏
        /// </summary>
        /// <param name="display"></param>
        public void DisPlay(bool display)
        {
            if (display)
            {
                switch (ShowType)
                {
                    case ShowType.Direct:
                        ChangeApache(1);
                        break;
                    case ShowType.Curve:

                        if (isShowAnim)
                        {
                            float apache = (float) Math.Round(_canvasGroup.alpha, 3);
                            DeleteImmortalTimeTask(viewShowTimeTask);
                            viewShowTimeTask = AddImmortalTimeTask(() =>
                            {
                                apache = (float) Math.Round(apache += 0.01f / ShowTime, 3);
                                ChangeApache(apache);
                                if (apache >= 1)
                                {
                                    DeleteImmortalTimeTask(viewShowTimeTask);
                                }
                            }, "视图显示任务", 0.01f, (int) (100 * ShowTime));
                        }
                        else
                        {
                            ChangeApache(1);
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                ShowObj(Window);
            }
            else
            {
                switch (ShowType)
                {
                    case ShowType.Direct:
                        ChangeApache(0);
                        HideObj(Window);
                        break;
                    case ShowType.Curve:
                        if (isHideAnim)
                        {
                            DeleteImmortalTimeTask(viewShowTimeTask);
                            float apache = (float) Math.Round(_canvasGroup.alpha, 3);
                            viewShowTimeTask = AddImmortalTimeTask(() =>
                            {
                                apache = (float) Math.Round(apache -= 0.01f / ShowTime, 3);
                                ChangeApache(apache);
                                if (apache <= 0)
                                {
                                    if (Window != null)
                                    {
                                        HideObj(Window);
                                    }

                                    DeleteImmortalTimeTask(viewShowTimeTask);
                                }
                            }, "视图隐藏任务", 0.01f, (int) (100 * ShowTime));
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                ViewDestroy();
            }
        }


        #region UI 绑定

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <param name="viewType">需要绑定的组件</param>
        /// <param name="path">当前组件的路径</param>
        // ReSharper disable once VirtualMemberCallInConstructor
        protected void BindUi<T>(ref T viewType, string path)
        {
            viewType = transform.Find("Window/" + path).GetComponent<T>();
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewType"></param>
        /// <param name="path"></param>
        protected void BindUi<T>(ref List<T> viewType, string path)
        {
            viewType = new List<T>(transform.Find("Window/" + path).GetComponentsInChildren<T>(true));
        }

        /// <summary>
        /// 绑定UI
        /// </summary>
        /// <param name="viewType">视图类型</param>
        /// <param name="path">路径</param>
        protected void BindUi(ref GameObject viewType, string path)
        {
            viewType = transform.Find("Window/" + path).GetComponent<Transform>().gameObject;
        }

        #endregion

        #region UI 事件绑定

        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="button"></param>
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(UnityEngine.UI.Button button, EventTriggerType eventId, UnityAction action)
        {
            button.onClick.AddListener(action);
        }


        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="selectable"></param>
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(Selectable selectable, EventTriggerType eventId, UnityAction<BaseEventData> action)
        {
            /*if (!uiListener.ContainsKey(selectable.name))
            {
                uiListener.Add(selectable.name, action);
            }*/

            EventTrigger trigger = selectable.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = selectable.gameObject.AddComponent<EventTrigger>();
            }

            if (trigger.triggers.Count == 0)
            {
                trigger.triggers = new List<EventTrigger.Entry>();
            }

            UnityAction<BaseEventData> callback = action;
            EventTrigger.Entry entry = new EventTrigger.Entry {eventID = eventId};
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="selectable"></param>
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(GameObject selectable, EventTriggerType eventId, UnityAction<BaseEventData> action)
        {
            if (!uiListener.ContainsKey(selectable.name))
            {
                uiListener.Add(selectable.name, action);
            }

            EventTrigger trigger = selectable.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = selectable.gameObject.AddComponent<EventTrigger>();
            }

            if (trigger.triggers.Count == 0)
            {
                trigger.triggers = new List<EventTrigger.Entry>();
            }

            UnityAction<BaseEventData> callback = action;
            EventTrigger.Entry entry = new EventTrigger.Entry {eventID = eventId};
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// 绑定监听事件
        /// </summary>
        /// <param name="buttonList">当前要操作的UI组件</param>
        /// <param name="eventId">要触发的事件类型</param>
        /// <param name="action">要执行的事件</param>
        protected void BindListener(List<Selectable> buttonList, EventTriggerType eventId,
            UnityAction<BaseEventData> action)
        {
            foreach (Selectable selectable in buttonList)
            {
                if (!uiListener.ContainsKey(selectable.name))
                {
                    uiListener.Add(selectable.name, action);
                }

                EventTrigger trigger = selectable.GetComponent<EventTrigger>();
                if (trigger == null)
                {
                    trigger = selectable.gameObject.AddComponent<EventTrigger>();
                }

                if (trigger.triggers.Count == 0)
                {
                    trigger.triggers = new List<EventTrigger.Entry>();
                }

                UnityAction<BaseEventData> callback = action;
                EventTrigger.Entry entry = new EventTrigger.Entry {eventID = eventId};
                entry.callback.AddListener(callback);
                trigger.triggers.Add(entry);
            }
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="selectableList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected List<Selectable> SelectableConverter<T>(List<T> selectableList) where T : Selectable
        {
            List<Selectable> returnSelectableList = new List<Selectable>();
            foreach (T selectable in selectableList)
            {
                returnSelectableList.Add(selectable);
            }

            return returnSelectableList;
        }

        /// <summary>
        /// 增加计时任务
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="taskName"></param>
        /// <param name="delay"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected int AddTimeTask(UnityAction callback, string taskName, float delay, int count = 1)
        {
            int timeTaskId = timeSvc.AddTimeTask(callback, taskName, delay, count);
            timeTaskInfoList.Add(new TimeTaskInfo()
                {TimeTaskId = timeTaskId, TimeLoopType = TimeTaskList.TimeLoopType.Once, TimeTaskName = taskName});
            return timeTaskId;
        }

        /// <summary>
        /// 增加循环计时任务
        /// </summary>
        /// <param name="callbackList"></param>
        /// <param name="taskName"></param>
        /// <param name="delay"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected int AddSwitchTask(List<UnityAction> callbackList, string taskName, float delay, int count = 1)
        {
            int timeTaskId = timeSvc.AddSwitchTask(callbackList, taskName, delay, count);
            timeTaskInfoList.Add(new TimeTaskInfo()
                {TimeTaskId = timeTaskId, TimeLoopType = TimeTaskList.TimeLoopType.Loop, TimeTaskName = taskName});
            return timeTaskId;
        }

        /// <summary>
        /// 增加不摧毁任务
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="taskName"></param>
        /// <param name="delay"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected int AddImmortalTimeTask(UnityAction callback, string taskName, float delay, int count = 1)
        {
            int timeTaskId = timeSvc.AddImmortalTimeTask(callback, taskName, delay, count);
            timeTaskInfoList.Add(new TimeTaskInfo()
                {TimeTaskId = timeTaskId, TimeLoopType = TimeTaskList.TimeLoopType.Once, TimeTaskName = taskName});
            // TimeSvc.DeleteTimeTask();
            return timeTaskId;
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        protected void DeleteTimeTask(int timeTaskId)
        {
            timeSvc.DeleteTimeTask(timeTaskId);
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        protected void DeleteSwitchTask(int timeTaskId)
        {
            timeSvc.DeleteSwitchTask(timeTaskId);
        }

        /// <summary>
        /// 删除计时任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        protected void DeleteImmortalTimeTask(int timeTaskId)
        {
            timeSvc.DeleteImmortalTimeTask(timeTaskId);
            DeleteTimeTaskById(timeTaskId);
        }

        /// <summary>
        /// 根据任务ID删除任务
        /// </summary>
        /// <param name="timeTaskId"></param>
        private void DeleteTimeTaskById(int timeTaskId)
        {
            for (int i = 0; i < timeTaskInfoList.Count; i++)
            {
                if (timeTaskInfoList[i].TimeTaskId == timeTaskId)
                {
                    timeTaskInfoList.Remove(timeTaskInfoList[i]);
                    return;
                }
            }
        }

        /// <summary>
        /// 暂停时事件
        /// </summary>
        public virtual void ViewPause()
        {
        }

        /// <summary>
        /// 继续时事件
        /// </summary>
        public virtual void ViewContinue()
        {
        }

        #endregion
    }
}