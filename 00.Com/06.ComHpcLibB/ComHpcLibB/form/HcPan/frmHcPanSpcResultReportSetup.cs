using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPanSpcResultReportSetup.cs
/// Description     : 특수건강진단 결과표 - 일반판정(D2) 질병코드 세팅
/// Author          : 이상훈
/// Create Date     : 2019-12-27
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm특수결과표설정1.frm(Frm특수결과표설정1)" />

namespace ComHpcLibB
{
    public partial class frmHcPanSpcResultReportSetup : Form
    {
        HicCodeService hicCodeService = null;

        HIC_CODE CodeHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrROWID;
        string FstrCode;
        string FstrName;

        public frmHcPanSpcResultReportSetup()
        {
            InitializeComponent();
            SetEvent();
            SetControll();
        }

        void SetEvent()
        {
            hicCodeService = new HicCodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnHelp.Click += new EventHandler(eBtnClick); 
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.txtCode.LostFocus += new EventHandler(eTxtLostFocus);
            this.txtCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtDBun.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

        }

        void SetControll()
        {
            CodeHelpItem = new HIC_CODE();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            SS1_Sheet1.Columns.Get(4).Visible = false;

            fn_Screen_Clear();
            eBtnClick(btnSearch, new EventArgs());
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
            }
            else if (sender == btnHelp)
            {
                clsPublic.GstrRetValue = "19";  //질병소견
                frmHcCodeHelp frm = new frmHcCodeHelp(clsPublic.GstrRetValue);
                frm.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(ePost_value_CODE);
                frm.ShowDialog();

                //if (!CodeHelpItem.IsNullOrEmpty())
                if (!FstrCode.IsNullOrEmpty())
                {
                    //txtDBun.Text = CodeHelpItem.CODE.Trim();
                    //lblDBunName.Text = CodeHelpItem.NAME.Trim();
                    txtDBun.Text = FstrCode.Trim();
                    lblDBunName.Text = FstrName.Trim();
                }
                else
                {
                    txtDBun.Text = "";
                    lblDBunName.Text = "";
                }
            }
            else if (sender == btnOK)
            {
                string strBun = "";
                int result = 0;

                if (FstrROWID == "")
                {
                    MessageBox.Show("", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                strBun = txtDBun.Text.Trim();

                clsDB.setBeginTran(clsDB.DbCon);

                result = hicCodeService.UpdateGCode2byRowId(FstrROWID, strBun);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("자료갱신 실패", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    eBtnClick(btnCancel, new EventArgs());
                    eBtnClick(btnSearch, new EventArgs());
                }
            }
            else if (sender == btnSearch)
            {
                int nRead = 0;

                List<HIC_CODE> list = hicCodeService.GetCodeNamebyGubun("34");

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].GCODE2;
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].GCODE;
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].ROWID;
                }
            }
        }

        //private void ePost_value_CODE(HIC_CODE item)
        private void ePost_value_CODE(string strCode, string strName)
        {
            //CodeHelpItem = item;
            FstrCode = strCode;
            FstrName = strName;
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                FstrROWID = "";

                if (e.ColumnHeader == true) return;
                if (e.RowHeader == true) return;

                txtCode.Text = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                txtName.Text = SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                txtDBun.Text = SS1.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                FstrROWID = SS1.ActiveSheet.Cells[e.Row, 4].Text.Trim();

                btnOK.Enabled = true;
            }
        }
    
        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtCode)
            {
                txtCode.Text = txtCode.Text.Trim();
                if (txtCode.Text.Trim() == "") return;
                fn_Screen_Clear();
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtCode)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtDBun)
            {
                if (e.KeyChar == 13)
                {
                    lblDBunName.Text = hb.READ_HIC_CODE("19", txtDBun.Text.Trim()); //공단질병분류
                    SendKeys.Send("{TAB}");
                }
            }
            else if (sender == txtName)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void fn_Screen_Clear()
        {
            FstrROWID = "";
            txtCode.Text = "";
            txtName.Text = "";
            txtDBun.Text = "";
            lblDBunName.Text = "";
            btnOK.Enabled = false;
            btnCancel.Enabled = true;
        }

        void fn_Screen_Display()
        {
            FstrROWID = "";

            //자료를 READ
            HIC_CODE list = hicCodeService.GetItembyCode("34", txtCode.Text.Trim());

            if (!list.IsNullOrEmpty())
            {
                FstrROWID = list.ROWID.Trim();
                txtName.Text = list.NAME.Trim();
                lblDBunName.Text = hb.READ_HIC_CODE("19", list.GCODE2.Trim());  //공단질병분류
            }
            else
            {
                FstrROWID = "";
            }

            btnOK.Enabled = true;
            btnCancel.Enabled = true;
        }
    }
}
