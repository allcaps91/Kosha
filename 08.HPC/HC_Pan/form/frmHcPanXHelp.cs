using ComBase;
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
/// File Name       : frmHcPanXHelp.cs
/// Description     : 방사선종사자 판정 Help 
/// Author          : 이상훈
/// Create Date     : 2019-11-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HcPan104.frm(FrmXhelp1)" />

namespace HC_Pan
{
    public partial class frmHcPanXHelp : Form
    {
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuService hicJepsuService = null;
        HicPatientService hicPatientService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicJepsuLtdResBohum1Service hicJepsuLtdResBohum1Service = null;
        HicCodeService hicCodeService = null;

        public delegate void SetGstrValue(string GstrValue);
        public event SetGstrValue rSetGstrValue;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrGubun;
        string FstrJong;

        public frmHcPanXHelp(string strGubun)
        {
            InitializeComponent();
            FstrGubun = strGubun;
            SetEvent();
        }

        void SetEvent()
        {
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuService = new HicJepsuService();
            hicPatientService = new HicPatientService();
            hicResultExCodeService = new HicResultExCodeService();
            hicJepsuLtdResBohum1Service = new HicJepsuLtdResBohum1Service();
            hicCodeService = new HicCodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnMenuOK.Click += new EventHandler(eBtnClick);
            this.btnMenuSave.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS1.Change += new ChangeEventHandler(eSpdChange);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            SS1_Sheet1.Columns.Get(4).Visible = false;
            SS1_Sheet1.Columns.Get(5).Visible = false;
            SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 24;
            txtJochi.Text = "";
            FstrJong = clsPublic.GstrRetValue;
            FstrJong = FstrGubun;
            clsPublic.GstrRetValue = "";
            fn_Screen_Display();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnMenuOK)
            {
                rSetGstrValue(txtJochi.Text);
                this.Close();
            }
            else if (sender == btnMenuSave)
            {
                long nCode = 0;
                string strDel = "";
                string strName = "";
                string strGCode = "";
                string strROWID = "";
                string strChange = "";
                long nMaxNo = 0;
                int result = 0;

                //신규등록을 위해 최대번호를 읽음
                nMaxNo = long.Parse(hicCodeService.GetMaxCodebyGubun(FstrJong));

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
                            result = hicCodeService.Insert("70", string.Format("{0:00000}", nMaxNo), strName, "", strGCode);
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
                }

                clsDB.setCommitTran(clsDB.DbCon);
                fn_Screen_Display();
            }
        }

        void fn_Screen_Display()
        {
            int nRead = 0;

            sp.Spread_All_Clear(SS1);
            SS1.ActiveSheet.RowCount = 30;

            //기존에 등록된 자료를 찾음
            List<HIC_CODE> list = hicCodeService.GetItembyGubun(FstrJong);

            nRead = list.Count;
            SS1.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SS1.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].GCODE;
                SS1.ActiveSheet.Cells[i, 4].Text = list[i].ROWID;
                SS1.ActiveSheet.Cells[i, 5].Text = "";
            }
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            string strChk = "";
            string strJochi = "";
            string strRemark = "";

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                strJochi = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                if (!strJochi.IsNullOrEmpty())
                {
                    strRemark += strJochi + ",";
                }
            }

            if (!strRemark.IsNullOrEmpty())
            {
                strRemark = VB.Left(strRemark, strRemark.Length - 1);
                txtJochi.Text = strRemark;
            }
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (sender == SS1)
            {
                SS1.ActiveSheet.Cells[e.Row, 5].Text = "Y";
                btnMenuSave.Enabled = true;
            }
        }
    }
}
