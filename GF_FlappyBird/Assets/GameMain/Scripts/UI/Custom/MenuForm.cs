using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace CC
{
    /// <summary>
    /// 菜单界面
    /// </summary>
    public class MenuForm : UGuiForm
    {
        private ProcedureMenu m_ProcedureMenu = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_ProcedureMenu = (ProcedureMenu)userData;
            Log.Info(m_ProcedureMenu);
        }

        protected override void OnClose(object userData)
        {
            m_ProcedureMenu = null;
            base.OnClose(userData);
        }

        public void OnStartBntClick()
        {
            m_ProcedureMenu.StartGame();
        }

        public void OnSettingBntClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SettingFrom);
        }
    }
}
