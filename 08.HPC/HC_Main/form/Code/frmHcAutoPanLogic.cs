using ComBase;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcAutoPanLogic.cs
/// Description     : 가판정문구 생성 조건 설정  =>>> 사용무(2020.09.25 16:12) 김경동 확인
/// Author          : 이상훈
/// Create Date     : 2019-09-05
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmAutoPanLogic.frm(FrmAutoPanLogic)" />

namespace HC_Main
{
    public partial class frmHcAutoPanLogic : Form
    {
        HeaAutopanService heaAutopanService = null;

        frmHcAutoPanExCode FrmHcAutoPanExCode = null;
        HIC_EXCODE HicExCodeItem = null;

        clsSpread sp = new clsSpread();

        public frmHcAutoPanLogic()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            heaAutopanService = new HeaAutopanService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnNew.Click += new EventHandler(eBtnClick);
            this.btnFind.Click += new EventHandler(eBtnClick);
            this.btnAdd1.Click += new EventHandler(eBtnClick);
            this.btnAdd2.Click += new EventHandler(eBtnClick);
            this.btnAdd3.Click += new EventHandler(eBtnClick);
            this.btnDel1.Click += new EventHandler(eBtnClick);
            this.btnDel2.Click += new EventHandler(eBtnClick);
            this.btnDel3.Click += new EventHandler(eBtnClick);
            this.btnAuto1.Click += new EventHandler(eBtnClick);
            this.btnAuto2.Click += new EventHandler(eBtnClick);
            this.btnDel3.Click += new EventHandler(eBtnClick);
            this.btnSave1.Click += new EventHandler(eBtnClick);
            this.btnSave2.Click += new EventHandler(eBtnClick);
            this.btnSave3.Click += new EventHandler(eBtnClick);
            this.btnInsert.Click += new EventHandler(eBtnClick);
            this.btnExamDel.Click += new EventHandler(eBtnClick);
            this.btnExamSave.Click += new EventHandler(eBtnClick);
            this.SS1.CellClick += new CellClickEventHandler(eSpdClick);

            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSSeqNo.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSExam.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSAnd.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSOr.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSCalc.CellDoubleClick += new CellClickEventHandler(eSpdDClick);

            this.SSSeqNo.Change += new ChangeEventHandler(eSpdChange);
            this.SSExam.Change += new ChangeEventHandler(eSpdChange);
            this.SSAnd.Change += new ChangeEventHandler(eSpdChange);
            this.SSOr.Change += new ChangeEventHandler(eSpdChange);
            this.SSCalc.Change += new ChangeEventHandler(eSpdChange);

            this.txtSearch.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {

        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                fn_Screen_Clear();
            }
            else if (sender == btnFind)
            {
                fn_ViewAutoPan(txtSearch.Text.Trim());
            }
            else if (sender == btnSave)
            {
                fn_Save("1");
                fn_Save("2");
                fn_Save("3");
                MessageBox.Show("저장 완료!", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);

                fn_Screen_Clear("");
            }
            else if (sender == btnAdd1)
            {
                if (txtSeqNo.Text.Trim() == "")
                {
                    MessageBox.Show("신규 조건생성 버튼을 클릭한 후 작성하시기 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                FrmHcAutoPanExCode = new frmHcAutoPanExCode();
                FrmHcAutoPanExCode.rSetGstrValue += new frmHcAutoPanExCode.SetGstrValue(ExCode_value);
                FrmHcAutoPanExCode.ShowDialog();
                FrmHcAutoPanExCode.rSetGstrValue -= new frmHcAutoPanExCode.SetGstrValue(ExCode_value);

                //if (!HicExCodeItem.IsNullOrEmpty())
                //{
                //    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                //}
                //else
                //{
                //    txtLtdCode.Text = "";
                //}


            }
            else if (sender == btnAdd2)
            {

            }
            else if (sender == btnAdd3)
            {

            }
        }

        void ExCode_value(HIC_EXCODE item)
        {
            HicExCodeItem = item;
        }

        void fn_Save(string sGubun)
        {
            switch (sGubun)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                default:
                    break;
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.ColumnHeader == true)
                {
                    sp.setSpdSort(SS1, e.Column, true);
                }
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtSearch)
            {
                if (e.KeyChar == (char)13)
                {
                    btnSearch.Focus();
                    fn_ViewAutoPan("");
                }
            }
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (sender == SS1)
            {
                SS1.ActiveSheet.Cells[e.Row, 3].Text = "";
            }
        }

        void fn_Screen_Clear(string strTemp = "")
        {
            if (strTemp == "")
            {
                txtSearch.Text = "";
                txtWrtNo.Text = "";
                txtSyntax.Text = "";
                txtSeqNo.Text = "";
            }

            sp.Spread_All_Clear(SSExam);
            sp.Spread_All_Clear(SSAnd);
            sp.Spread_All_Clear(SSOr);
            sp.Spread_All_Clear(SSCalc);

            txtJepNo.Text = "";
            txtTest.Text = "";
        }

        void fn_ViewAutoPan(string strTemp = "")
        {
            int nRead = 0;

            SS1.ActiveSheet.RowCount = 0;

            List<HEA_AUTOPAN> list = heaAutopanService.GetItembyText(strTemp);

            nRead = list.Count;
            if (nRead > 0)
            {
                SS1.ActiveSheet.RowCount = nRead;
                SS1.DataSource = list;
            }
        }
    }
}
