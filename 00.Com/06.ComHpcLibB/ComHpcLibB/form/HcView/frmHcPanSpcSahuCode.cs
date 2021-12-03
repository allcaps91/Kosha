using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanSpcSahuCode.cs
/// Description     : 특수검진 사후관리 Help
/// Author          : 이상훈
/// Create Date     : 2019-11-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSpcSahuCode.frm(HcSahu)" />

namespace ComHpcLibB
{
    public partial class frmHcPanSpcSahuCode : Form
    {
        HicCodeService hicCodeService = null;

        public delegate void SetGstrValue(string GstrValue, string GstrName);
        public event SetGstrValue rSetGstrValue;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrGubun;
        int nRead;

        public frmHcPanSpcSahuCode()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcPanSpcSahuCode(string strGubun)
        {
            InitializeComponent();
            FstrGubun = strGubun;
            SetEvent();
        }

        void SetEvent()
        {
            hicCodeService = new HicCodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnMenuOK.Click += new EventHandler(eBtnClick); 
            this.SS1.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            List<HIC_CODE> list = hicCodeService.GetCodeNamebyGubun("32");

            nRead = list.Count;
            SS1.ActiveSheet.RowCount = nRead;
            SS1_Sheet1.ColumnHeader.Rows.Get(-1).Height = 24;
            for (int i = 0; i < nRead; i++)
            {
                SS1.ActiveSheet.Cells[i, 0].Text = "";
                SS1.ActiveSheet.Cells[i, 1].Text = list[i].CODE.Trim();
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].NAME.Trim();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                clsPublic.GstrRetValue = "";
                this.Close();
                return;
            }
            else if (sender == btnMenuOK)
            {
                string strName = "";

                clsPublic.GstrRetValue = "";
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        SS1.ActiveSheet.Cells[i, 1].Text = clsPublic.GstrRetValue += SS1.ActiveSheet.Cells[i, 1].Text + ",";
                    }
                }

                if (clsPublic.GstrRetValue.IsNullOrEmpty())
                {
                    MessageBox.Show("사후관리를 1건도 선택하지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (clsPublic.GstrRetValue.Length > 6)
                {
                    MessageBox.Show("사후관리는 최대 3개까지만 선택이 가능함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                clsPublic.GstrRetValue = VB.Left(clsPublic.GstrRetValue, clsPublic.GstrRetValue.Length - 1);
                strName = hm.Sahu_Names_Display(clsPublic.GstrRetValue);

                rSetGstrValue(clsPublic.GstrRetValue, strName);
                this.Close();
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (e.Column != 1)
            {
                return;
            }

            if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "True")
            {
                SS1.ActiveSheet.Cells[e.Row, 0].Text = "";
                return;
            }
            SS1.ActiveSheet.Cells[e.Row, 0].Text = "True";
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            clsPublic.GstrRetValue = VB.Left(SS1.ActiveSheet.Cells[e.Row, 1].Text + VB.Space(10), 10);
            this.Close();
        }
    }
}
