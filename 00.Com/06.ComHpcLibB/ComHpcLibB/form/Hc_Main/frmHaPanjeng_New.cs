using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaPanjeng_New.cs
/// Description     : 검사코드별 약속처방 판정내용입력
/// Author          : 이상훈
/// Create Date     : 2019-10-08
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain82.frm(FrmUseWardView_new)" />

namespace ComHpcLibB
{
    public partial class frmHaPanjeng_New : Form
    {
        HeaCodeService heaCodeService = null;
        HeaResultwardService heaResultwardService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        public delegate void EventClosed(string strReturn);
        public event EventClosed rEventClosed;

        string FstrResult;
        string FstrSetOrder;

        public frmHaPanjeng_New(string strResult, string strSetOrder)
        {
            InitializeComponent();
            FstrResult = strResult;
            FstrSetOrder = strSetOrder;
            SetEvent();
        }

        void SetEvent()
        {
            heaCodeService = new HeaCodeService();
            heaResultwardService = new HeaResultwardService();

            this.Load += new EventHandler(eFormLoad);
            this.txtKeyword.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnMenuSelect.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtKeyword.IsNullOrEmpty()) { return; }

            fn_SS_Display(txtKeyword.Text.Trim());
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_SS_Display();
        }

        void fn_SS_Display(string argKeyWord = "")
        {
            int nREAD = 0;
            long nJobSabun = 0;

            //기존의 내용을 Clear
            sp.Spread_All_Clear(SS1);
            lblResult.Text = FstrResult;

            SS1_Sheet1.Rows.Get(-1).Height = 24;
            SS1_Sheet1.Columns.Get(3).Visible = false;
            SS1_Sheet1.Columns.Get(4).Visible = false;

            //판정의사 사번
            nJobSabun = heaCodeService.GetCodeByGubun("05").To<long>(0);

            //DB에서 자료를 SELECT
            List<HEA_RESULTWARD> list = heaResultwardService.GetItembySabuncodeGubun(nJobSabun.ToString(), VB.Pstr(clsHcVariable.GstrRefValue, "^^", 1), "03", VB.Pstr(clsHcVariable.GstrRefValue, "^^", 2), argKeyWord);

            nREAD = list.Count;
            SS1.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                SS1.ActiveSheet.Cells[i, 0].Text = list[i].SEQNO.ToString();
                SS1.ActiveSheet.Cells[i, 1].Text = "";
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].WARDNAME;
                SS1.ActiveSheet.Cells[i, 4].Text = list[i].GUBUN.Trim() + list[i].CODE.Trim() + list[i].SEQNO.ToString();
                SS1.ActiveSheet.Cells[i, 5].Text = list[i].STEP.To<string>("");
                switch (SS1.ActiveSheet.Cells[i, 5].Text.Trim())
                {
                    case "1":
                        SS1.ActiveSheet.Cells[i, 5].BackColor = Color.FromArgb(255, 255, 128);
                        break;
                    case "2":
                        SS1.ActiveSheet.Cells[i, 5].BackColor = Color.FromArgb(255, 255, 217);
                        break;
                    case "3":
                        SS1.ActiveSheet.Cells[i, 5].BackColor = Color.FromArgb(255, 0, 0);
                        break;
                    default:
                        SS1.ActiveSheet.Cells[i, 5].BackColor = Color.FromArgb(255, 255, 255);
                        break;
                }

                FarPoint.Win.Spread.Row row;
                row = SS1.ActiveSheet.Rows[i];
                float rowSize = row.GetPreferredHeight();
                row.Height = rowSize;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                clsHcVariable.GstrRefValue = "";
                this.Close();
                return;
            }
            else if (sender == btnMenuSelect)
            {
                string strChk = "";
                clsHcVariable.GstrRefValue = "";
                clsHcVariable.GstrRefValue1 = "";

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 1].Text;
                    if (strChk == "True")
                    {
                        SS1.ActiveSheet.Cells[i, 1].Text = "";
                        strChk = "";
                        clsHcVariable.GstrRefValue += SS1.ActiveSheet.Cells[i, 2].Text + "\r\n";
                        clsHcVariable.GstrRefValue1 += SS1.ActiveSheet.Cells[i, 4].Text + ",";
                    }
                }

                if (clsHcVariable.GstrRefValue1.Trim() == ",")
                {
                    clsHcVariable.GstrRefValue1 = "";
                }

                rEventClosed(clsHcVariable.GstrRefValue);

                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                fn_SS_Display(txtKeyword.Text.Trim());
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                clsHcVariable.GstrRefValue = SS1.ActiveSheet.Cells[e.Row, 2].Text + "\r\n";
                clsHcVariable.GstrRefValue1 = SS1.ActiveSheet.Cells[e.Row, 4].Text + ",";

                if (clsHcVariable.GstrRefValue1.Trim() == ",")
                {
                    clsHcVariable.GstrRefValue1 = "";
                }

                rEventClosed(clsHcVariable.GstrRefValue);

                this.Close();
                return;
            }
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (SS1.ActiveSheet.Cells[e.Row, e.Column].Text == "True")
            {
                SS1.ActiveSheet.Cells[e.Row, 2].BackColor = Color.FromArgb(128, 128, 255);
            }
            else
            {
                SS1.ActiveSheet.Cells[e.Row, 2].BackColor = Color.FromArgb(255, 255, 255);
            }
        }
    }
}
