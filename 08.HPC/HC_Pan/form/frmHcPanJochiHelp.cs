using ComBase;
using ComLibB;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanJochiHelp.cs
/// Description     : 조치 상용구 관리
/// Author          : 이상훈
/// Create Date     : 2019-11-25
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmJochiHelp.frm(HcPan96)" />

namespace HC_Pan
{
    public partial class frmHcPanJochiHelp : Form
    {
        HicCodeService hicCodeService = null;

        clsSpread sp = new clsSpread();

        string FstrRemark;

        public delegate void Spread_DoubleClick(string strRemark);
        public event Spread_DoubleClick SpreadDoubleClick;

        public frmHcPanJochiHelp()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicCodeService = new HicCodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnMenuOK.Click += new EventHandler(eBtnClick);
            this.btnMenuSave.Click += new EventHandler(eBtnClick);
            this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS1.Change += new ChangeEventHandler(eSpdChacnge);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            SS1_Sheet1.Columns.Get(4).Visible = false;
            SS1_Sheet1.Columns.Get(5).Visible = false;
            SS1_Sheet1.Rows.Get(-1).Height = 20;
            txtJochi.Text = "";
            fn_Screen_Display();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnMenuOK)
            {
                SpreadDoubleClick(FstrRemark);
                this.Close();
            }
            else if (sender == btnMenuSave)
            {
                int nCode = 0;
                string strDel = "";
                string strName = "";
                string strGCode = "";
                string strROWID = "";
                string strChange = "";
                long nMaxNo = 0;
                
                //신규등록을 위해 최대번호를 읽음
                nMaxNo = long.Parse(hicCodeService.GetMaxCodebyGubun("27"));

                clsDB.setBeginTran(clsDB.DbCon);
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strName = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                    strGCode = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                    strDel = SS1.ActiveSheet.Cells[i, 3].Text.Trim();
                    strROWID = SS1.ActiveSheet.Cells[i, 4].Text.Trim();
                    strChange = SS1.ActiveSheet.Cells[i, 5].Text.Trim();
                    if (strDel == "True")
                    {
                        strDel = "Y";
                    }
                    else
                    {
                        strDel = "";
                    }

                    if (strROWID.IsNullOrEmpty())
                    {
                        if (!strName.IsNullOrEmpty() && strDel.IsNullOrEmpty())
                        {
                            nMaxNo += 1;
                            result = hicCodeService.Insert("27", nMaxNo.ToString(), strName, "", strGCode);
                        }
                    }
                    else
                    {
                        if (strDel == "Y")
                        {
                            result = hicCodeService.Delete(strROWID);
                        }
                        else if (strChange == "Y")
                        {
                            result = hicCodeService.Update(strName, strGCode, strROWID);
                        }
                    }

                    if (result < 0)
                    {
                        MessageBox.Show("자료 등록 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                fn_Screen_Display();
            }
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            string strChk = "";
            string strJochi = "";

            if (sender == SS1)
            {
                FstrRemark = "";
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    if (strChk == "True")
                    {
                        strJochi = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                        if (!strJochi.IsNullOrEmpty())
                        {
                            FstrRemark += strJochi + "/";
                        }
                    }
                }

                if (!FstrRemark.IsNullOrEmpty())
                {
                    FstrRemark = VB.Left(FstrRemark, FstrRemark.Length - 1);
                }
            }
        }

        void eSpdChacnge(object sender, ChangeEventArgs e)
        {
            if (sender == SS1)
            {
                SS1.ActiveSheet.Cells[e.Row, 5].Text = "True";
                btnMenuSave.Enabled = true;
            }
        }

        void fn_Screen_Display()
        {
            int nRead = 0;

            sp.Spread_All_Clear(SS1);

            //기존에 등록된 자료를 찾음
            List<HIC_CODE> list = hicCodeService.GetItembyGubun("27");

            nRead = list.Count;
            SS1.ActiveSheet.RowCount = nRead + 20;
            if (nRead > 40)
            {
                SS1_Sheet1.Rows.Get(-1).Height = 20;
            }

            for (int i = 0; i < nRead; i++)
            {
                SS1.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].GCODE;
                SS1.ActiveSheet.Cells[i, 4].Text = list[i].ROWID;
                SS1.ActiveSheet.Cells[i, 5].Text = "";
            }

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 1);
                if (SS1.ActiveSheet.Cells[i, 1].Text.Trim() != "")
                {
                    SS1.ActiveSheet.Rows[i].Height = size.Height;
                }
                else
                {
                    SS1.ActiveSheet.Rows[i].Height = 20;
                }
            }
        }
    }
}
