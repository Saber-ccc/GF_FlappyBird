using GameFramework.Event;
using GameFramework.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace CC
{
    /// <summary>
    /// 分数界面
    /// </summary>
    public class ScoreForm : UGuiForm
    {
        public Text mScoreText;
        private int m_Score = 0;
        private float m_ScoreTimer = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            //订阅事件
            GameEntry.Event.Subscribe(BirdDeadEventArgs.EventId, OnBirdDead);
            GameEntry.Event.Subscribe(AddScoreEventArgs.EventId, OnAddScore);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            m_ScoreTimer += elapseSeconds;
            if (m_ScoreTimer >= 2f)
            {
                m_ScoreTimer = 0;
                m_Score += 1;
                if(GameEntry.Localization.Language.Equals(Language.ChineseSimplified))
                    mScoreText.text = "总分：" + m_Score;
                else
                    mScoreText.text = "Score:" + m_Score;
            }
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            //取消订阅
            GameEntry.Event.Unsubscribe(BirdDeadEventArgs.EventId, OnBirdDead);
            GameEntry.Event.Unsubscribe(AddScoreEventArgs.EventId, OnAddScore);
        }

        protected override void OnPause()
        {
            base.OnPause();
            //清空数据
            m_Score = 0;
            m_ScoreTimer = 0;
            if (GameEntry.Localization.Language.Equals(Language.ChineseSimplified))
                mScoreText.text = "总分：" + m_Score;
            else
                mScoreText.text = "Score:" + m_Score;
        }

        private void OnBirdDead(object sender, GameEventArgs e)
        {
            //往数据节点里存积分数据
            GameEntry.DataNode.GetOrAddNode("Score").SetData<VarInt>(m_Score);
            //打开结束界面
            GameEntry.UI.OpenUIForm(UIFormId.GameOverForm);
            
        }

        private void OnAddScore(object sender, GameEventArgs e)
        {
            AddScoreEventArgs ase = (AddScoreEventArgs)e;
            m_Score += ase.AddCount;
            if (GameEntry.Localization.Language.Equals(Language.ChineseSimplified))
                mScoreText.text = "总分：" + m_Score;
            else
                mScoreText.text = "Score:" + m_Score;
        }
    }
}
