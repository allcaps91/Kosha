using ComBase;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPanPanjengHelp.cs
/// Description     : 1차판정 D1,D2 Help 
/// Author          : 이상훈
/// Create Date     : 2019-11-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm판정help.frm(Frm판정help)" />

namespace ComHpcLibB
{
    public partial class frmHcPanPanjengHelp : Form
    {
        HicCodeService hicCodeService = null;

        public delegate void SetGstrValue(string GstrValue, string GstrName);
        public event SetGstrValue rSetGstrValue;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrFlag;
        string FstrGubun;

        public frmHcPanPanjengHelp(string strGubun)
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
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            SS1_Sheet1.Columns.Get(3).Visible = false;
            SS1_Sheet1.ColumnHeader.Rows.Get(-1).Height = 24;
            FstrFlag = "";
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
                int nCnt = 0;

                clsPublic.GstrRetName = "";

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nCnt += 1;
                        if (nCnt > 6)
                        {
                            MessageBox.Show("코드 7개이상 선택불가", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        clsPublic.GstrRetValue += SS1.ActiveSheet.Cells[i, 1].Text.Trim() + ",";
                        clsPublic.GstrRetName += SS1.ActiveSheet.Cells[i, 2].Text.Trim() + "/";
                    }
                }

                rSetGstrValue(clsPublic.GstrRetValue, clsPublic.GstrRetName);

                this.Close();
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (e.Column == 0) return;

            if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "True")
            {
                SS1.ActiveSheet.Cells[e.Row, 0].Text = "";                   
            }
            else
            {
                SS1.ActiveSheet.Cells[e.Row, 0].Text = "True";
            }
        }

        void fn_Screen_Display()
        {
            int nRead = 0;

            sp.Spread_All_Clear(SS1);

            //if (clsPublic.GstrRetValue == "53")
            if (FstrGubun == "53")
            {
                this.Text = "2차판정 추가검사 코드 Help";
                FstrFlag = "OK";
            }

            //기존에 등록된 자료를 찾음
            List<HIC_CODE> list = hicCodeService.GetCodeNamebyGubun(FstrGubun);

            nRead = list.Count;
            SS1.ActiveSheet.RowCount = nRead;
            SS1_Sheet1.ColumnHeader.Rows.Get(-1).Height = 24;
            for (int i = 0; i < nRead; i++)
            {
                SS1.ActiveSheet.Cells[i, 0].Text = "";
                SS1.ActiveSheet.Cells[i, 1].Text = list[i].CODE.Trim();
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].NAME.Trim();
            }
            clsPublic.GstrRetValue = "";
            
            if (VB.L(clsPublic.GstrRetName, ",") > 1)
            {
                for (int i = 0; i < VB.L(clsPublic.GstrRetName, ","); i++)
                {
                    for (int j = 0; j < SS1.ActiveSheet.RowCount; j++)
                    {
                        if (SS1.ActiveSheet.Cells[j, 1].Text == VB.Pstr(clsPublic.GstrRetName, ",", i))
                        {
                            SS1.ActiveSheet.Cells[j, 0].Text = "True";
                        }
                    }
                }
            }
        }
    }
}
