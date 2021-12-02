using ComBase;
using ComLibB;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using ComBase.Controls;
using System.IO;

/// <summary>
/// Class Name      : ComHpcLibB\Hc_School
/// File Name       : frmHcSchoolCommonInput.cs
/// Description     : 판정내용입력
/// Author          : 이상훈
/// Create Date     : 2020-02-03
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmUseWardSet2.frm(HcSchool51)" />

namespace ComHpcLibB
{
    public partial class frmHcSchoolCommonInput : Form
    {
        HicResultwardService hicResultwardService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FsGubun;

        public delegate void Spread_DoubleClick(string strGubun, string strWardName);
        public event Spread_DoubleClick ssPanjengDblClick;

        public frmHcSchoolCommonInput(string sGubun)
        {
            InitializeComponent();
            FsGubun = sGubun;
            SetEvent();
        }

        void SetEvent()
        {
            hicResultwardService = new HicResultwardService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnMenuSelect.Click += new EventHandler(eBtnClick);
            this.SS1.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int Inx = 0;
            string strCODE = "";
            string strCName = "";
            string strCOldName = "";
            int nRead = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            //기존의 내용을 Clear
            sp.Spread_All_Clear(SS1);

            SS1_Sheet1.Columns.Get(3).Visible = false;
            SS1_Sheet1.Columns.Get(4).Visible = false;

            List<HIC_RESULTWARD> list = hicResultwardService.GetItembyGubun(string.Format("{0:00}", FsGubun.To<int>()));

            nRead = list.Count;
            SS1.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].WARDNAME;
            }

            for (int i = 0; i < nRead; i++)
            {
                Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 2);
                SS1.ActiveSheet.Rows[i].Height = size.Height;
            }

            clsPublic.GstrRetValue = "";
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnMenuSelect)
            {
                string strChk = "";
                string strValue = "";
                int k = 0;

                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = SS1.ActiveSheet.Cells[i, 1].Text;
                    if (strChk == "True")
                    {
                        strValue += SS1.ActiveSheet.Cells[i, 2].Text + "\r\n";
                        k++;
                    }
                }

                if (k > 0)
                {
                    ssPanjengDblClick(FsGubun, strValue);
                }

                this.Close();
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (e.Column == 2 && e.RowHeader == false)
            {
                ssPanjengDblClick(FsGubun, SS1.ActiveSheet.Cells[e.Row, 2].Text + "\r\n");
                this.Close();
            }
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column != 1)
            {
                return;
            }

            if (SS1.ActiveSheet.Cells[e.Row, e.Column].Text == "True")
            {
                SS1.ActiveSheet.Cells[e.Row, e.Column].BackColor = Color.FromArgb(128, 128, 255);
            }
            else
            {
                SS1.ActiveSheet.Cells[e.Row, e.Column].ForeColor = Color.FromArgb(255, 255, 255);
            }
        }
    }
}
