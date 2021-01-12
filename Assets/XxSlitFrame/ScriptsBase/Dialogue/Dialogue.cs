using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XAnimator.Base;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.View;

namespace XxSlitFrame.ScriptsBase.Dialogue
{
    public class Dialogue : BaseWindow
    {
        private GameObject _retractPanel;
        private Image _retractBackground;
        private Button _retract;
        private ScrollRect _retractPanelScrollView;
        private GameObject _content;
        private Button _open;
        private Button _skip;
        private Button _next;
        private Text _skipTime;
        private GameObject _doctor;
        private Image _doctorHead;
        private GameObject _doctorBigDialogueBackground;
        private Text _doctorBigContent;
        private GameObject _doctorSecondaryDialogueBackground;
        private Text _doctorSecondaryContent;
        private GameObject _doctorSmallDialogueBackground;
        private Text _doctorSmallContent;
        private GameObject _patient;
        private GameObject _patientHead;
        private GameObject _patientBigDialogueBackground;
        private Text _patientBigContent;
        private GameObject _patientSecondaryDialogueBackground;
        private Text _patientSecondaryContent;
        private GameObject _patientSmallDialogueBackground;
        private Text _patientSmallContent;
        [SerializeField] [Header("对话数据")] public DialogueData dialogueData;

        [Header("对话分段")] [SerializeField] public Dictionary<int, List<DialogDataInfo>> tipsDataListDic;

        /// <summary>
        /// 当前播放的片段
        /// </summary>
        private int _currentPlayParagraphIndex;

        private int _dialogueTimeTask;
        private int _dialogueSurplusTimeTask;
        private float _dialogueTime;

        public override void Init()
        {
            //对话分段
            DialogueSubsection();
            //对话事件
            ShowObj(_skipTime.gameObject);
            HideObj(_next.gameObject);
        }

        /// <summary>
        /// 对话分段
        /// </summary>
        private void DialogueSubsection()
        {
            tipsDataListDic = new Dictionary<int, List<DialogDataInfo>>();
            for (int i = 0; i < dialogueData.dataInfos.Count; i++)
            {
                //包含当前分段,添加内容
                if (!tipsDataListDic.ContainsKey(i))
                {
                    tipsDataListDic.Add(i, dialogueData.dataInfos[i].dialogDataInfos);
                }
            }
        }

        /// <summary>
        /// 播放对话内容
        /// </summary>
        private void PlayDialogueContent(DialogDataInfo dialogDataInfo)
        {
            switch (dialogDataInfo.role)
            {
                case Role.Doctor:
                    ShowObj(_doctor);
                    HideObj(_patient);
                    switch (dialogDataInfo.length)
                    {
                        case Length.EShort:
                            ShowObj(_doctorSmallDialogueBackground);
                            HideObj(_doctorSecondaryDialogueBackground);
                            HideObj(_doctorBigDialogueBackground);
                            _doctorSmallContent.text = dialogDataInfo.dialogContent;
                            break;
                        case Length.EIn:
                            HideObj(_doctorSmallDialogueBackground);
                            ShowObj(_doctorSecondaryDialogueBackground);
                            HideObj(_doctorBigDialogueBackground);
                            _doctorSecondaryContent.text = dialogDataInfo.dialogContent;

                            break;
                        case Length.ELong:
                            HideObj(_doctorSmallDialogueBackground);
                            HideObj(_doctorSecondaryDialogueBackground);
                            ShowObj(_doctorBigDialogueBackground);
                            _doctorBigContent.text = dialogDataInfo.dialogContent;

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case Role.Patient:
                    HideObj(_doctor);
                    ShowObj(_patient);
                    switch (dialogDataInfo.length)
                    {
                        case Length.EShort:
                            ShowObj(_patientSmallDialogueBackground);
                            HideObj(_patientSecondaryDialogueBackground);
                            HideObj(_patientBigDialogueBackground);
                            _patientSmallContent.text = dialogDataInfo.dialogContent;
                            break;
                        case Length.EIn:
                            HideObj(_patientSmallDialogueBackground);
                            ShowObj(_patientSecondaryDialogueBackground);
                            HideObj(_patientBigDialogueBackground);
                            _patientSecondaryContent.text = dialogDataInfo.dialogContent;

                            break;
                        case Length.ELong:
                            HideObj(_patientSmallDialogueBackground);
                            HideObj(_patientSecondaryDialogueBackground);
                            ShowObj(_patientBigDialogueBackground);
                            _patientBigContent.text = dialogDataInfo.dialogContent;

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            AnimatorControllerManager.Instance.PlayAnim(dialogDataInfo.animType);
            listenerSvc.ExecuteEvent(dialogDataInfo.dialogClipEvent);
            audioSvc.PlayTipAndDialogAudio(dialogDataInfo.dialogueAudioClip);
        }

        /// <summary>
        /// 播放对话片段
        /// </summary>
        /// <param name="paragraph"></param>
        private void PlayDialogueParagraph(int paragraph)
        {
            //一次

            DialogueTimeTask(paragraph, tipsDataListDic[paragraph], 0);
            _currentPlayParagraphIndex = paragraph;
        }

        /// <summary>
        /// 对话循环任务
        /// </summary>
        /// <param name="paragraph"></param>
        /// <param name="currentDialogueDataList"></param>
        /// <param name="dialogueIndex"></param>
        private void DialogueTimeTask(int paragraph, List<DialogDataInfo> currentDialogueDataList, int dialogueIndex)
        {
            PlayDialogueContent(currentDialogueDataList[dialogueIndex]);
            _dialogueTimeTask = AddTimeTask(() =>
            {
                if (dialogueIndex < currentDialogueDataList.Count - 1)
                {
                    dialogueIndex++;
                    DialogueTimeTask(paragraph, currentDialogueDataList, dialogueIndex);
                }
                else
                {
                    if (persistentDataSvc.autoPlay)
                    {
                        listenerSvc.ExecuteEvent(dialogueData.dataInfos[_currentPlayParagraphIndex].endEvent);
                        _currentPlayParagraphIndex = -1;
                    }
                    else
                    {
                        HideObj(_skip);
                        ShowObj(_next);
                    }
                }
            }, "播放对话内容", currentDialogueDataList[dialogueIndex].dialogueAudioClip.length);
            _dialogueTime = currentDialogueDataList[dialogueIndex].dialogueAudioClip.length;
            _skipTime.text = ((int) _dialogueTime).ToString(CultureInfo.InvariantCulture) + "<size=35>s</size>";
            DeleteTimeTask(_dialogueSurplusTimeTask);
            _dialogueSurplusTimeTask = AddTimeTask(() =>
            {
                _dialogueTime -= 1;
                _skipTime.text = ((int) _dialogueTime).ToString(CultureInfo.InvariantCulture) + "<size=35>s</size>";
                if (_dialogueTime <= 0)
                {
                    DeleteTimeTask(_dialogueSurplusTimeTask);
                }
            }, "对话任务剩余时间", 1, 0);
        }


        protected override void InitView()
        {
            BindUi(ref _retractPanel, "RetractPanel");
            BindUi(ref _retractBackground, "RetractPanel/RetractBackground");
            BindUi(ref _retract, "RetractPanel/Retract");
            BindUi(ref _retractPanelScrollView, "RetractPanel/RetractPanelScrollView");
            BindUi(ref _content, "RetractPanel/RetractPanelScrollView/Viewport/Content");
            BindUi(ref _open, "Open");
            BindUi(ref _skip, "Skip");
            BindUi(ref _next, "Next");
            BindUi(ref _skipTime, "Skip/SkipTime");
            BindUi(ref _doctor, "Doctor");
            BindUi(ref _doctorHead, "Doctor/DoctorHead");
            BindUi(ref _doctorBigDialogueBackground, "Doctor/DoctorBigDialogueBackground");
            BindUi(ref _doctorBigContent, "Doctor/DoctorBigDialogueBackground/DoctorBigContent");
            BindUi(ref _doctorSecondaryDialogueBackground, "Doctor/DoctorSecondaryDialogueBackground");
            BindUi(ref _doctorSecondaryContent, "Doctor/DoctorSecondaryDialogueBackground/DoctorSecondaryContent");
            BindUi(ref _doctorSmallDialogueBackground, "Doctor/DoctorSmallDialogueBackground");
            BindUi(ref _doctorSmallContent, "Doctor/DoctorSmallDialogueBackground/DoctorSmallContent");
            BindUi(ref _patient, "Patient");
            BindUi(ref _patientHead, "Patient/PatientHead");
            BindUi(ref _patientBigDialogueBackground, "Patient/PatientBigDialogueBackground");
            BindUi(ref _patientBigContent, "Patient/PatientBigDialogueBackground/PatientBigContent");
            BindUi(ref _patientSecondaryDialogueBackground, "Patient/PatientSecondaryDialogueBackground");
            BindUi(ref _patientSecondaryContent, "Patient/PatientSecondaryDialogueBackground/PatientSecondaryContent");
            BindUi(ref _patientSmallDialogueBackground, "Patient/PatientSmallDialogueBackground");
            BindUi(ref _patientSmallContent, "Patient/PatientSmallDialogueBackground/PatientSmallContent");
        }

        protected override void InitListener()
        {
            BindListener(_retract, EventTriggerType.PointerClick, OnRetract);
            BindListener(_open, EventTriggerType.PointerClick, OnOpen);
            BindListener(_skip, EventTriggerType.PointerClick, OnSkip);
            BindListener(_skip, EventTriggerType.PointerEnter, OnSkipEnter);
            BindListener(_skip, EventTriggerType.PointerExit, OnSkipExit);
            BindListener(_next, EventTriggerType.PointerClick, OnSkip);
            listenerSvc.AddListenerEvent<int>(ListenerEventType.PlayDialogueParagraph, PlayDialogueParagraph);
        }


        private void OnSkipExit(BaseEventData arg0)
        {
            ShowObj(_skipTime.gameObject);
        }

        private void OnSkipEnter(BaseEventData arg0)
        {
            HideObj(_skipTime.gameObject);
        }

        private void OnRetract(BaseEventData targetObj)
        {
        }

        private void OnOpen(BaseEventData targetObj)
        {
        }

        private void OnSkip(BaseEventData targetObj)
        {
            DeleteTimeTask(_dialogueTimeTask);
            audioSvc.StopTipAndDialogAudio();
            HideView();
            audioSvc.PlayEffectAudio(AudioData.AudioType.ENextStep);
            listenerSvc.ExecuteEvent(dialogueData.dataInfos[_currentPlayParagraphIndex].endEvent);
        }
    }
}