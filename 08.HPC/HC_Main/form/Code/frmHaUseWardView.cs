using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaUseWardView.cs
/// Description     : 판정내용입력
/// Author          : 이상훈
/// Create Date     : 2019-10-07
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain58.frm(FrmUseWardView)" />

namespace HC_Main
{
    public partial class frmHaUseWardView : Form
    {
        HeaResultwardService heaResultwardService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        string FstrCode;

        public frmHaUseWardView(string sCode)
        {
            InitializeComponent();
            FstrCode = sCode;
            SetEvent();
        }

        void SetEvent()
        {
            heaResultwardService = new HeaResultwardService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSelect.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int inx = 0;
            string strCODE  = "";
            string strCName = "";
            string strCOldName = "";
            long nREAD = 0;
            string strGubun = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            //기존의 내용을 Clear
            sp.Spread_All_Clear(SS1);

            strCODE = "";
            strCName = "";
            strCOldName = "";

            SS1_Sheet1.Rows.Get(-1).Height = 24;
            SS1_Sheet1.Columns.Get(3).Visible = false;
            SS1_Sheet1.Columns.Get(4).Visible = false;

            if (FstrCode != "")
            {
                for (int i = 0; i < FstrCode.Trim().Length; i++)
                {
                    strCODE += VB.Mid(FstrCode.Trim(), i, 1) + ","; 
                }
                strCODE = "(" + VB.Left(strCODE.Trim(), strCODE.Trim().Length - 1) + ")";
            }

            strGubun = "02";
            //DB에서 자료를 SELECT
            List<HEA_RESULTWARD> list = heaResultwardService.GetItembySabuncodeGubun(clsType.User.IdNumber, strCODE, strGubun);

            nREAD = list.Count;
            SS1.ActiveSheet.RowCount = list.Count;

            for (int i = 0; i < nREAD; i++)
            {
                strCName = hb.Read_JilJong_Name(VB.Left(list[i].CODE, 1));
                if (strCName.Trim() != strCOldName.Trim())
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = strCName;
                    strCOldName = strCName;
                }
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].WARDNAME.Trim();
                SS1.ActiveSheet.Cells[i, 4].Text = list[i].GUBUN.Trim() + list[i].CODE.Trim();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                rEventClosed();
                this.Close();
                return;
            }
            else if (sender == btnSelect)
            {
                string strChk = "";

                strChk = "";
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk =  SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                    if (strChk == "True")
                    {
                        SS1.ActiveSheet.Cells[i, 1].Text = "";
                        strChk = "";
                        SS1.ActiveSheet.Cells[i, 1].Text = "";
                    }
                }
            }
        }
    }
}
