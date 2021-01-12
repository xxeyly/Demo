using System;
using System.Collections.Generic;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData
{
    [CreateAssetMenu(fileName = "AssessmentData", menuName = "配置文件/考核数据", order = 1)]
    [Serializable]
    public class AssessmentData : ScriptableObject
    {
        /// <summary>
        /// 本次操作记录的id 如果是练习就是练习的id,如果是自测就是自测的id，如果是考核就是考核的id
        /// </summary>
        public string pid;

        /// <summary>
        /// 创建时间
        /// </summary>
        public string createTime;

        /// <summary>
        /// 更新时间
        /// </summary>
        public string updateTime;

        /// <summary>
        /// 本次操作用时
        /// </summary>
        public string useTime;

        /// <summary>
        /// 本次得分
        /// </summary>
        public string score;

        /// <summary>
        /// 状态
        /// </summary>
        public string status;

        /// <summary>
        /// 总分数
        /// </summary>
        public string zfs;

        /// <summary>
        /// 及格分数
        /// </summary>
        public string jgfs;

        /// <summary>
        /// 用户考核提交数据
        /// </summary>
        public List<TopicInfoData> list;

        /// <summary>
        /// 保存分数
        /// </summary>
        public void SaveScore(int number, float score)
        {
            list[number].score = score.ToString();
        }

        /// <summary>
        /// 保存是否操作正确
        /// </summary>
        /// <param name="number"></param>
        /// <param name="isSuccess"></param>
        public void SaveIsSuccess(int number, bool isSuccess)
        {
            if (isSuccess)
            {
                list[number].isSuccess = "1";
            }
            else
            {
                list[number].isSuccess = "0";
            }
        }

        /// <summary>
        /// 保存操作次数
        /// </summary>
        /// <param name="number"></param>
        /// <param name="isOperation"></param>
        public void SaveOperationCount(int number, bool isOperation)
        {
            if (isOperation)
            {
                list[number].opCount = "1";
            }
            else
            {
                list[number].opCount = "0";
            }
        }


        /// <summary>
        /// 计算总分
        /// </summary>
        public void CalculationTotalScore()
        {
            float tempTotalScore = 0f;
            for (int i = 0; i < list.Count; i++)
            {
                tempTotalScore += int.Parse(list[i].score);
            }

            score = tempTotalScore.ToString();
        }
    }

    /// <summary>
    /// 存储数据小类
    /// </summary>
    [Serializable]
    public class TopicInfoData
    {
        public string number; //题号或实验步骤，在本次实验或考试中不能重复------------------------------------------------需要面板填写
        public string title; //题目或实验步骤名称-------------------------------------------------------------------------需要面板填写
        public string op; //选项  "A:选项说明;B:选项说明"-----------------------------------------------------------------需要面板填写
        public string isSuccess = "1"; //结果是否正确,0不正确，1正确              非必填项 
        public string zqda; //题目的正确选项或正确操作步骤，多个直接用分号分隔  ”A:选项说明；B:选项说明….”-------------需要面板填写
        public string useTime = "0"; //本次操作的用时,整数？ = "0"             必填项 直接填0  数据上传时自动填0
        public string opCount = "0"; //操作次数，整数？            非必填项     做对或做错都填1次或真实次数
        public string unit = "秒"; //单位  ”秒”--------------------------------------------------------------------------------默认填秒
        public string score = "0"; //本次操作的得分？             必填项
        public string otherData = "0"; //其他数据
        public List<string> opList = new List<string>();
        private bool showOpList = true;
        public string type = "1"; //1.试题、2.操作步骤--------------------------------------------------------------------------需要面板填写
        public string time = "0"; //提交时间
        public string fs; //题目或步骤的分数------------------------------------------------------------------------------需要面板填写

        public bool ShowOpList()
        {
            return showOpList;
        }

        public void SwitchShowOpList()
        {
            showOpList = !showOpList;
        }

        /// <summary>
        /// 选项列表
        /// </summary>
        /// <returns></returns>
        public List<string> OpList()
        {
            return opList;
        }
        
        /// <summary>
        /// 正确选项列表
        /// </summary>
        public List<bool> optionList = new List<bool>();

        /// <summary>
        /// 正确选项列表
        /// </summary>
        /// <returns></returns>
        public List<bool> OptionList()
        {
            return optionList;
        }
    }
}