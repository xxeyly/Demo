using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XxSlitFrame.Tools.Svc.BaseSvc;
using Random = UnityEngine.Random;

// ReSharper disable Unity.InefficientPropertyAccess

namespace XxSlitFrame.Tools.Svc
{
    /// <summary>
    /// 计时系统
    /// </summary>
    public class TimeSvc : SvcBase
    {
        public static TimeSvc Instance;

        private List<TimeTask> _taskTimeList;
        private List<TimeTask> _taskTimeImmortalList;
        private List<SwitchTask> _taskSwitchList;
        private List<int> _tidTimeList;
        private List<int> _tidTimeImmortalList;
        private List<int> _tidSwitchList;
        private bool _clear;
        public List<string> currentTime;
        public DayOfWeek currentWeek;

        [HideInInspector] [Header("所有计时任务")] [SerializeField]
        public List<TimeTaskList> timeTaskList;

        public override void StartSvc()
        {
            Instance = GetComponent<TimeSvc>();
        }

        public override void InitSvc()
        {
            _taskTimeList = new List<TimeTask>();
            _taskSwitchList = new List<SwitchTask>();
            _taskTimeImmortalList = new List<TimeTask>();
            _tidTimeList = new List<int>();
            _tidSwitchList = new List<int>();
            _tidTimeImmortalList = new List<int>();
            currentTime = new List<string>() {"", "", "", "", "", ""};
            UpdateCurrentSystemTime();
        }

        /// <summary>
        /// 返回当前系统时间
        /// </summary>
        /// <returns></returns>
        public void UpdateCurrentSystemTime()
        {
            currentTime[0] = DateTime.Now.Year.ToString();
            if (DateTime.Now.Month < 10)
            {
                currentTime[1] = "0" + DateTime.Now.Month;
            }
            else
            {
                currentTime[1] = DateTime.Now.Month.ToString();
            }

            if (DateTime.Now.Day < 10)
            {
                currentTime[2] = "0" + DateTime.Now.Day;
            }
            else
            {
                currentTime[2] = DateTime.Now.Day.ToString();
            }

            if (DateTime.Now.Hour < 10)
            {
                currentTime[3] = "0" + DateTime.Now.Hour;
            }
            else
            {
                currentTime[3] = DateTime.Now.Hour.ToString();
            }

            if (DateTime.Now.Minute < 10)
            {
                currentTime[4] = "0" + DateTime.Now.Minute;
            }
            else
            {
                currentTime[4] = DateTime.Now.Minute.ToString();
            }

            if (DateTime.Now.Second < 10)
            {
                currentTime[5] = "0" + DateTime.Now.Second;
            }
            else
            {
                currentTime[5] = DateTime.Now.Second.ToString();
            }

            currentWeek = DateTime.Now.DayOfWeek;
        }

        /// <summary>
        /// 获得所有计时任务的ID
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllTimeTaskId()
        {
            List<int> timeTaskIdList = new List<int>();
            foreach (TimeTaskList taskList in timeTaskList)
            {
                timeTaskIdList.Add(taskList.tid);
            }

            return timeTaskIdList;
        }

        #region 定时任务

        /// <summary>
        /// 增加定时任务
        /// </summary>
        public int AddTimeTask(UnityAction callback, string taskName, float delay, int count = 1)
        {
            float destTime = Time.time + delay;
            int tid = GetTimeTaskTid();
            TimeTask timeTask = new TimeTask
            {
                Tid = tid, DestTime = destTime, TaskName = taskName, Callback = callback, Count = count, Delay = delay
            };
            _taskTimeList.Add(timeTask);
            timeTaskList.Add(new TimeTaskList
            {
                tid = timeTask.Tid,
                tidName = timeTask.TaskName,
                loopType = TimeTaskList.TimeLoopType.Once
            });
            return tid;
        }

        /// <summary>
        /// 增加不摧毁定时任务
        /// </summary>
        public int AddImmortalTimeTask(UnityAction callback, string taskName, float delay, int count = 1)
        {
            float destTime = Time.time + delay;
            int tid = GetImmortalTaskTid();
            TimeTask timeTask = new TimeTask
            {
                Tid = tid, DestTime = destTime, TaskName = taskName, Callback = callback, Count = count, Delay = delay
            };
            _taskTimeImmortalList.Add(timeTask);
            timeTaskList.Add(new TimeTaskList
            {
                tid = timeTask.Tid,
                tidName = timeTask.TaskName,
                loopType = TimeTaskList.TimeLoopType.Immortal
            });
            return tid;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="tid"></param>
        public bool DeleteTimeTask(int tid)
        {
            bool exist = false;
            for (int i = 0; i < _taskTimeList.Count; i++)
            {
                if (_taskTimeList[i].Tid == tid)
                {
                    if (_tidTimeList.Contains(tid))
                    {
                        _tidTimeList.Remove(tid);
                    }

                    if (_taskTimeList.Contains(_taskTimeList[i]))
                    {
                        _taskTimeList.Remove(_taskTimeList[i]);
                    }

                    exist = true;
                    break;
                }
            }

            foreach (TimeTaskList taskList in timeTaskList)
            {
                if (taskList.tid == tid)
                {
                    timeTaskList.Remove(taskList);
                    break;
                }
            }

            return exist;
        }

        /// <summary>
        /// 删除不死任务
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public bool DeleteImmortalTimeTask(int tid)
        {
            bool exist = false;
            for (int i = 0; i < _taskTimeImmortalList.Count; i++)
            {
                if (_taskTimeImmortalList[i].Tid == tid)
                {
                    if (_tidTimeList.Contains(tid))
                    {
                        _tidTimeList.Remove(tid);
                    }

                    if (_taskTimeImmortalList.Contains(_taskTimeImmortalList[i]))
                    {
                        _taskTimeImmortalList.Remove(_taskTimeImmortalList[i]);
                    }

                    exist = true;
                    break;
                }
            }

            foreach (TimeTaskList taskList in timeTaskList)
            {
                if (taskList.tid == tid)
                {
                    timeTaskList.Remove(taskList);
                    break;
                }
            }

            return exist;
        }

        public void DeleteImmortalTimeTask()
        {
            _clear = true;
            for (int i = 0; i < timeTaskList.Count; i++)
            {
                if (timeTaskList[i].loopType == TimeTaskList.TimeLoopType.Immortal)
                {
                    timeTaskList.Remove(timeTaskList[i]);
                }
            }

            _taskTimeImmortalList.Clear();
            _clear = false;
        }


        /// <summary>
        /// 删除任务
        /// </summary>
        public bool DeleteTimeTask()
        {
            _clear = true;
            for (int i = 0; i < timeTaskList.Count; i++)
            {
                if (timeTaskList[i].loopType == TimeTaskList.TimeLoopType.Once)
                {
                    timeTaskList.Remove(timeTaskList[i]);
                }
            }

            _taskTimeList.Clear();
            _clear = false;
            return false;
        }

        /// <summary>
        /// 删除所有计时任务,不死任务除外
        /// </summary>
        private void DeleteAllTimeTask()
        {
            DeleteTimeTask();
            DeleteSwitchTask();
        }

        /// <summary>
        /// 生成计时唯一ID
        /// </summary>
        private int GetTimeTaskTid()
        {
            int tid = Random.Range(0, Int32.MaxValue);
            if (!_tidTimeList.Contains(tid))
            {
                _tidTimeList.Add(tid);
                return tid;
            }
            else
            {
                return GetTimeTaskTid();
            }
        }

        #endregion

        #region 切换类型定时任务

        /// <summary>
        /// 增加定时任务
        /// </summary>
        public int AddSwitchTask(List<UnityAction> callbackList, string taskName, float delay, int count = 1)
        {
            float destTime = Time.time + delay;
            int tid = GetSwitchTaskTid();
            SwitchTask switchTask = new SwitchTask
            {
                Tid = tid,
                DestTime = destTime,
                TaskName = taskName,
                CallbackList = callbackList,
                Count = count,
                Delay = delay
            };
            _taskSwitchList.Add(switchTask);
            timeTaskList.Add(new TimeTaskList
            {
                tid = switchTask.Tid,
                tidName = switchTask.TaskName,
                loopType = TimeTaskList.TimeLoopType.Loop
            });
            return tid;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="tid"></param>
        public bool DeleteSwitchTask(int tid)
        {
            bool exist = false;
            for (int i = 0; i < _taskSwitchList.Count; i++)
            {
                if (_taskSwitchList[i].Tid == tid)
                {
                    if (_tidSwitchList.Contains(tid))
                    {
                        _tidSwitchList.Remove(tid);
                    }

                    if (_taskSwitchList.Contains(_taskSwitchList[i]))
                    {
                        _taskSwitchList.Remove(_taskSwitchList[i]);
                    }

                    exist = true;
                    break;
                }
            }

            foreach (TimeTaskList taskList in timeTaskList)
            {
                if (taskList.tid == tid)
                {
                    timeTaskList.Remove(taskList);
                    break;
                }
            }

            return exist;
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        public bool DeleteSwitchTask()
        {
            bool exist = false;

            for (int i = 0; i < timeTaskList.Count; i++)
            {
                if (timeTaskList[i].loopType == TimeTaskList.TimeLoopType.Loop)
                {
                    timeTaskList.Remove(timeTaskList[i]);
                }
            }

            _taskSwitchList.Clear();
            return exist;
        }

        /// <summary>
        /// 生成计时唯一ID
        /// </summary>
        private int GetImmortalTaskTid()
        {
            int tid = Random.Range(0, Int32.MaxValue);
            if (!_tidTimeImmortalList.Contains(tid))
            {
                _tidTimeImmortalList.Add(tid);
                return tid;
            }
            else
            {
                return GetImmortalTaskTid();
            }
        }

        /// <summary>
        /// 生成计时唯一ID
        /// </summary>
        private int GetSwitchTaskTid()
        {
            int tid = Random.Range(0, Int32.MaxValue);
            if (!_tidSwitchList.Contains(tid))
            {
                _tidSwitchList.Add(tid);
                return tid;
            }
            else
            {
                return GetSwitchTaskTid();
            }
        }

        #endregion

        private void FixedUpdate()
        {
            UpdateCurrentSystemTime();
            if (!_clear)
            {
                if (_taskTimeList != null)
                {
                    for (int i = 0; i < _taskTimeList.Count; i++)
                    {
                        TimeTask timeTask = _taskTimeList[i];
                        if (timeTask.DestTime > Time.time)
                        {
                        }
                        else
                        {
                            if (timeTask.Count == 1)
                            {
                                int timeTaskTid = timeTask.Tid;
                                _taskTimeList[i].Callback.Invoke();
                                if (_tidTimeList.Contains(timeTaskTid))
                                {
                                    _tidTimeList.Remove(timeTaskTid);
                                }

                                foreach (TimeTaskList taskList in timeTaskList)
                                {
                                    if (taskList.tid == timeTaskTid)
                                    {
                                        timeTaskList.Remove(taskList);
                                        break;
                                    }
                                }

                                if (_taskTimeList.Contains(timeTask))
                                {
                                    _taskTimeList.Remove(timeTask);
                                }
                            }
                            else
                            {
                                //不是无限循环
                                if (timeTask.Count != 0)
                                {
                                    timeTask.Count -= 1;
                                    timeTask.DestTime += timeTask.Delay;
                                    _taskTimeList[i].Callback.Invoke();
                                }
                                else
                                {
                                    timeTask.DestTime += timeTask.Delay;
                                    _taskTimeList[i].Callback.Invoke();
                                }
                            }
                        }
                    }
                }

                if (_taskTimeImmortalList != null)
                {
                    for (int i = 0; i < _taskTimeImmortalList.Count; i++)
                    {
                        TimeTask timeTask = _taskTimeImmortalList[i];
                        if (timeTask.DestTime > Time.time)
                        {
                        }
                        else
                        {
                            if (timeTask.Count == 1)
                            {
                                int timeTaskTid = timeTask.Tid;
                                _taskTimeImmortalList[i].Callback.Invoke();
                                if (_tidTimeList.Contains(timeTaskTid))
                                {
                                    _tidTimeList.Remove(timeTaskTid);
                                }

                                foreach (TimeTaskList taskList in timeTaskList)
                                {
                                    if (taskList.tid == timeTaskTid)
                                    {
                                        timeTaskList.Remove(taskList);
                                        break;
                                    }
                                }

                                if (_taskTimeImmortalList.Contains(timeTask))
                                {
                                    _taskTimeImmortalList.Remove(timeTask);
                                }
                            }
                            else
                            {
                                //不是无限循环
                                if (timeTask.Count != 0)
                                {
                                    timeTask.Count -= 1;
                                    timeTask.DestTime += timeTask.Delay;
                                    _taskTimeImmortalList[i].Callback.Invoke();
                                }
                                else
                                {
                                    timeTask.DestTime += timeTask.Delay;
                                    _taskTimeImmortalList[i].Callback.Invoke();
                                }
                            }
                        }
                    }
                }

                if (_taskSwitchList != null)
                {
                    for (int i = 0; i < _taskSwitchList.Count; i++)
                    {
                        SwitchTask switchTask = _taskSwitchList[i];
                        if (switchTask.DestTime > Time.time)
                        {
                        }
                        else
                        {
                            switchTask.CallbackList[switchTask.CurrentTaskNumber].Invoke();
                            if (switchTask.CurrentTaskNumber == switchTask.CallbackList.Count - 1)
                            {
                                switchTask.CurrentTaskNumber = 0;
                                if (switchTask.Count == 0)
                                {
                                }
                                else if (switchTask.Count == 1)
                                {
                                    int timeTaskTid = switchTask.Tid;

                                    if (_tidSwitchList.Contains(timeTaskTid))
                                    {
                                        _tidSwitchList.Remove(timeTaskTid);
                                    }

                                    foreach (TimeTaskList taskList in timeTaskList)
                                    {
                                        if (taskList.tid == timeTaskTid)
                                        {
                                            timeTaskList.Remove(taskList);
                                            break;
                                        }
                                    }

                                    if (_taskSwitchList.Contains(switchTask))
                                    {
                                        _taskSwitchList.Remove(switchTask);
                                    }
                                }
                                else
                                {
                                    switchTask.Count -= 1;
                                }
                            }
                            else
                            {
                                switchTask.CurrentTaskNumber++;
                            }

                            switchTask.DestTime += switchTask.Delay;
                        }
                    }
                }
            }
        }

        private List<int> _timeList;

        /// <summary>
        /// 秒数转换时间
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public List<int> TimeConversion(int duration)
        {
            _timeList = new List<int>();
            if (duration <= 9)
            {
                _timeList.Add(0);
                _timeList.Add(0);
                _timeList.Add(0);
                _timeList.Add(duration);
            }
            else if (duration >= 10 && duration <= 59)
            {
                int tenSecond = duration / 10;
                int second = duration - tenSecond * 10;
                _timeList.Add(0);
                _timeList.Add(0);
                _timeList.Add(tenSecond);
                _timeList.Add(second);
            }
            else if (duration >= 60 && duration <= 599)
            {
                int minute = duration / 60;
                int tenSecond = (duration - minute * 60) / 10;
                int second = duration - (tenSecond * 10 + minute * 60);
                _timeList.Add(0);
                _timeList.Add(minute);
                _timeList.Add(tenSecond);
                _timeList.Add(second);
            }
            else if (duration >= 600 && duration <= 3599)
            {
                int tenMinute = duration / 600;
                int minute = (duration - tenMinute * 600) / 60;
                int tenSecond = (duration - tenMinute * 600 - minute * 60) / 10;
                int second = duration - tenMinute * 600 - tenSecond * 10 - minute * 60;
                _timeList.Add(tenMinute);
                _timeList.Add(minute);
                _timeList.Add(tenSecond);
                _timeList.Add(second);
            }

            return _timeList;
        }


        /// <summary>
        /// 时间转换秒数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int TimeConversion(List<int> time)
        {
            return time[0] * 600 + time[1] * 60 + time[2] * 10 + time[3];
        }

        /// <summary>
        /// 图片闪烁
        /// </summary>
        /// <param name="twinkleImage"></param>
        /// <param name="twinkleInterval"></param>
        /// <returns></returns>
        public int ImageTwinkle(Image twinkleImage, float twinkleInterval)
        {
            int twinkleTimeTask = 0;
            float apache = 1f;
            bool _enhance = true;
            twinkleTimeTask = Instance.AddTimeTask(() =>
            {
                if (_enhance)
                {
                    apache -= twinkleInterval;
                    if (apache <= 0.2)
                    {
                        _enhance = false;
                    }
                }
                else
                {
                    apache += twinkleInterval;
                    if (apache >= 1)
                    {
                        _enhance = true;
                    }
                }

                twinkleImage.color =
                    new Color(twinkleImage.color.r, twinkleImage.color.g, twinkleImage.color.b, apache);
            }, "提示", twinkleInterval, 0);
            return twinkleTimeTask;
        }

        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="errorTips">错误提示面板</param>
        /// <param name="errorTipContent">错误提示内容文本</param>
        /// <param name="content">错误提示内容</param>
        /// <param name="action">错误提示完毕后执行事件</param>
        /// <returns></returns>
        public int ShowErrorTip(GameObject errorTips, Text errorTipContent, string content, UnityAction action = null)
        {
            int errorTipsTimeTask = 0;
            errorTips.SetActive(true);
            errorTipContent.text = content;
            action?.Invoke();
            errorTipsTimeTask = AddTimeTask(() => { errorTips.SetActive(false); }, "错误提示内容",
                General.General.ViewErrorTime);
            return errorTipsTimeTask;
        }
    }

    /// <summary>
    /// 定时任务数据类
    /// </summary>
    public class TimeTask
    {
        public int Tid; //任务ID
        public string TaskName; //任务名称
        public float DestTime; //执行时间
        public UnityAction Callback; // 执行的逻辑
        public int Count; //执行次数
        public float Delay; //执行间隔
    }

    public class SwitchTask
    {
        public int Tid; //任务ID
        public string TaskName; //任务名称
        public float DestTime; //执行时间
        public List<UnityAction> CallbackList; // 执行的逻辑
        public int CurrentTaskNumber; //当前执行的任务索引
        public int Count; //执行次数
        public float Delay; //执行间隔
    }

    [Serializable]
    public struct TimeTaskList
    {
        public enum TimeLoopType
        {
            [Header("一次")] Once,
            [Header("循环")] Loop,
            [Header("不死")] Immortal,
        }

        [Header("任务ID")] public int tid;
        [Header("任务名称")] public string tidName;
        [Header("任务类型")] public TimeLoopType loopType;
    }
}