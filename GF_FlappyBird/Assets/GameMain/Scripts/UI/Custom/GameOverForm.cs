using GameFramework;
using GameFramework.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace CC
{
    /// <summary>
    /// 游戏结束界面
    /// </summary>
    public class GameOverForm : UGuiForm
    {
        public Text mScoreText;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            //获取分数
            int score = GameEntry.DataNode.GetNode("Score").GetData<VarInt>();
            if (GameEntry.Localization.Language.Equals(Language.ChineseSimplified))
                mScoreText.text = "你的总分：" + score;
            else
                mScoreText.text = "Score:" + score;
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            mScoreText.text = string.Empty;
        }

        public void OnRestartButtonClick()
        {
            Close(true);
            //派发重新开始游戏事件
            GameEntry.Event.Fire(this, ReferencePool.Acquire<RestartEventArgs>());
            //显示小鸟
            GameEntry.Entity.ShowBird(new BirdData(GameEntry.Entity.GenerateSerialId(), 3, 5));
        }

        public void OnReturnButtonClick()
        {
            Close(true);
            //派发返回菜单场景事件
            GameEntry.Event.Fire(this, ReferencePool.Acquire<ReturnMenuEventArgs>());
        }
    }
}
