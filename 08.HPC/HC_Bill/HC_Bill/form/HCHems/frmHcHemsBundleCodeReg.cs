using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Bill
/// File Name       : frmHcHemsBundleCodeReg.cs
/// Description     : 묶음코드명 등록
/// Author          : 이상훈
/// Create Date     : 2021-02-08
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHems_코드1.frm(FrmHems_코드1)" />

namespace HC_Bill
{
    public partial class frmHcHemsBundleCodeReg : Form
    {
        HicGroupcodeService hicGroupcodeService = null;

        frmHcCodeHelp FrmHcCodeHelp = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsHcBill chb = new clsHcBill();
        clsHcType hct = new clsHcType();
        clsPrint CP = new clsPrint();

        string FstrROWID;
        string FstrCode;
        string FstrName;

        public frmHcHemsBundleCodeReg()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicGroupcodeService = new HicGroupcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);

            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);

            this.btnUHelp.Click += new EventHandler(eBtnClick);
            this.btnReExHelp.Click += new EventHandler(eBtnClick);

            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);

            this.txtCode.KeyPress += new KeyPressEventHandler(eKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strData = "";

            sp.Spread_All_Clear(SS1);
            SS1_Sheet1.Rows[-1].Height = 20;

            ComFunc.ReadSysDate(clsDB.DbCon);

            cboHang.Items.Clear();
            cboHang.Items.Add("A.1차검진");
            cboHang.Items.Add("B.2차검진");
            cboHang.Items.Add("C.특수검진");
            cboHang.Items.Add("D.일반+특수");
            cboHang.Items.Add("E.채용신검");
            cboHang.Items.Add("F.암검진");
            cboHang.Items.Add("G.기타검진");
            cboHang.Items.Add("H.공통선택");
            cboHang.Items.Add("I.특수선택");
            cboHang.Items.Add("M.유해물질");
            cboHang.SelectedIndex = 0;

            //적용수가
            cboSuga.Items.Clear();
            cboSuga.Items.Add("1.보험80%");
            cboSuga.Items.Add("2.보험100%");
            cboSuga.Items.Add("3.보험125%");
            cboSuga.Items.Add("4.일,특수차액");
            cboSuga.Items.Add("5.임의수가");
            cboSuga.SelectedIndex = 0;

            cboJong.Items.Clear();
            hb.ComboJong_AddItem(cboJong);
            cboJong.Items.Add("90.공통선택");
            cboJong.Items.Add("91.취급물질별");
            cboJong.Items.Add("92.특수선택");

            cboJong1.Items.Clear();
            cboJong1.Items.Add("**.전체");
            hb.ComboJong_AddItem(cboJong1);
            cboJong1.Items.Add("90.공통선택");
            cboJong1.Items.Add("91.취급물질별");
            cboJong1.Items.Add("92.특수선택");
            cboJong1.SelectedIndex = 0;

            //부담율
            cboBuRate.Items.Clear();
            cboBuRate.Items.Add(" ");
            cboBuRate.Items.Add("01.조합100%");
            cboBuRate.Items.Add("02.회사100%");
            cboBuRate.Items.Add("03.본인100%");
            cboBuRate.Items.Add("04.조합,본인50%");
            cboBuRate.Items.Add("05.조합,회사50%");
            cboBuRate.Items.Add("06.회사,본인50%");
            cboBuRate.SelectedIndex = 0;

            fn_Screen_Clear();
            fn_Code_Sheet_Set();
        }

        void fn_Screen_Clear()
        {
            txtCode.Text = "";   txtDelDate.Text = "";
            txtName.Text = "";   txtSDate.Text = "";
            cboJong.SelectedIndex = -1;
            txtUCode.Text = "";
            txtReExam.Text = "";
            txtUCode.Text = "";
            txtReExam.Text = "";
            cboHang.SelectedIndex = -1;
            cboSuga.SelectedIndex = -1;
            cboBuRate.Text = "";
            chkSelect.Checked = false;
            chkDental.Checked = false;
            txtYName.Text = "";
            FstrROWID = "";
            FstrCode = "";

            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;

            pnlCan.Enabled = true;
            for (int i = 0; i <= 5; i++)
            {
                CheckBox chkAm = (Controls.Find("chkAm" + (i).To<string>(), true)[0] as CheckBox);
                chkAm.Checked = false;
            }

            txtExamName.Text = "";

            for (int i = 0; i <= 5; i++)
            {
                CheckBox chkPrint = (Controls.Find("chkPrint" + (i).To<string>(), true)[0] as CheckBox);
                chkPrint.Checked = false;
            }

            txtRemark.Text = "";
        }

        void fn_Code_Sheet_Set()
        {
            int nRow = 0;
            int nRead = 0;
            string strJong = "";

            strJong = VB.Left(cboJong1.Text, 2);

            //묶음코드명을 Display
            List<HIC_GROUPCODE> list = hicGroupcodeService.GetItembyJong(strJong);

            nRead = list.Count;
            SS1.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                SS1.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                SS1.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].JONG;
                SS1.ActiveSheet.Cells[i, 3].Text = list[i].HANG;
                SS1.ActiveSheet.Cells[i, 4].Text = list[i].GBSELECT;
                SS1.ActiveSheet.Cells[i, 5].Text = list[i].GBSUGA;
                SS1.ActiveSheet.Cells[i, 6].Text = list[i].UCODE;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                string strHang = "";
                string strJong = "";
                string strSelect = "";
                string strDental = "";
                string strGbSuga = "";
                string strBurate = "";
                string strChkAm = "";
                string strChkPrt = "";
                string strGbModify = "";

                int result = 0;

                //GoSub CmdOK_Data_Check  '자료에 오류가 있는지 Check
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("금액코드명이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (cboSuga.Text.Trim() == "")
                {
                    MessageBox.Show("기본적용 수가구분이 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (cboJong.Text.Trim() == "")
                {
                    MessageBox.Show("건진종류가 공란입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                Cursor.Current = Cursors.WaitCursor;

                clsDB.setCommitTran(clsDB.DbCon);

                strJong = VB.Left(cboJong.Text, 2);
                strHang = VB.Left(cboHang.Text, 1);
                strGbSuga = VB.Left(cboSuga.Text, 1);
                strBurate = VB.Left(cboBuRate.Text, 2);
                strSelect = "N";

                if (chkSelect.Checked == true)
                {
                    strSelect = "Y";
                }
                strDental = "N";
                if (chkDental.Checked == true)
                {
                    strDental = "Y";
                }

                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkAm = (Controls.Find("chkAm" + (i).To<string>(), true)[0] as CheckBox);
                    if (chkAm.Checked == true)
                    {
                        strChkAm += "1" + ",";
                    }
                    else
                    {
                        strChkAm += "0" + ",";
                    }

                    CheckBox chkPrint = (Controls.Find("chkPrint" + (i).To<string>(), true)[0] as CheckBox);
                    if (chkPrint.Checked == true)
                    {
                        strChkPrt += "1" + ",";
                    }
                    else
                    {
                        strChkPrt += "0" + ",";
                    }
                }

                HIC_GROUPCODE item = new HIC_GROUPCODE();

                item.CODE = FstrCode;
                item.HANG = strHang;
                item.NAME = txtName.Text.Trim();
                item.JONG = strJong;
                item.GBSELECT = strSelect;
                item.GBSUGA = strGbSuga;
                if (txtUCode.Text.Trim() != "")
                {
                    item.UCODE = VB.Pstr(txtUCode.Text.Trim(), ".", 1);
                }
                else
                {
                    item.UCODE = "";
                }
                item.YNAME = txtYName.Text.Trim();                
                item.SDATE = txtSDate.Text.To<DateTime>();
                item.DELDATE = txtDelDate.Text.To<DateTime>();
                item.ENTSABUN = clsType.User.IdNumber.To<long>();
                item.GBAM = strChkAm;
                item.GBSELF = strBurate;
                item.REMARK = txtRemark.Text.Trim();
                item.GBDENT = strDental;
                item.EXAMNAME = txtExamName.Text.Trim();
                item.GBPRINT = strChkPrt;
                if (txtReExam.Text.Trim() != "")
                {
                    item.REEXAM = VB.Pstr(txtReExam.Text.Trim(), ".", 1);
                }
                else
                {
                    item.REEXAM = "";
                }
                item.GBSUNAP = "";
                item.GBNOTADDPAN = "";
                item.GBSANGDAM = "";
                item.GBGUBUN1 = ""; 

                if (FstrROWID == "")
                {
                    result = hicGroupcodeService.Insert(item);
                    strGbModify = "신규등록 중";
                }
                else
                {
                    result = hicGroupcodeService.UpDate(item);
                    strGbModify = "변경중";
                }

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("묶음코드 " + strGbModify + " 오류 발생!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                Cursor.Current = Cursors.Default;

                fn_Screen_Clear();
                fn_Code_Sheet_Set();
                txtCode.Focus();
            }
            else if (sender == btnDelete)
            {
                int result = 0;

                if (MessageBox.Show("정말로 삭제를 하시겠습니까?", "선택", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                result = hicGroupcodeService.DeletebyRowId(FstrROWID);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("검사 금액 코드 삭제시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                fn_Screen_Clear();
                fn_Code_Sheet_Set();
                txtCode.Focus();
            }
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
                txtCode.Focus();
            }
            else if (sender == btnSearch)
            {
                fn_Code_Sheet_Set();
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "묶 음 코 드 집";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("검진종류: " + cboJong1.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += sp.setSpdPrint_String("출력일시: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);
                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnUHelp)
            {
                FrmHcCodeHelp = new frmHcCodeHelp("51");
                FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(eCode_value);
                FrmHcCodeHelp.ShowDialog();

                if (!FstrCode.IsNullOrEmpty())
                {
                    txtUCode.Text = FstrCode.Trim() + "." + FstrName.Trim();
                }
                else
                {
                    txtUCode.Text = "";
                }
                FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(eCode_value);
            }
            else if (sender == btnReExHelp) //사용무
            {

            }
        }

        void eCode_value(string strCode, string strName)
        {
            FstrCode = strCode;
            FstrName = strName;
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                txtCode.Text = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                fn_Screen_Display();
                txtName.Focus();
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtCode)
            {
                if (txtCode.Text.Trim() == "")
                {
                    return;
                }
                else
                {
                    txtCode.Text = txtCode.Text.Trim();
                }

                if (txtCode.Text.Length >= 0)
                {
                    fn_Screen_Display();
                }
            }
        }

        void fn_Screen_Display()
        {
            FstrCode = txtCode.Text.Trim();
            FstrROWID = "";
            cboHang.SelectedIndex = -1;
            for (int i = 0; i < cboHang.Items.Count; i++)
            {
                cboHang.SelectedIndex = i;
                if (VB.Left(cboHang.Text, 1) == VB.Left(FstrCode, 1))
                {
                    cboHang.SelectedIndex = i;
                    break;
                }
            }

            //자료를 READ
            HIC_GROUPCODE list = hicGroupcodeService.GetItemByCode(FstrCode);

            if (!list.IsNullOrEmpty())
            {
                FstrROWID = list.RID;
                txtDelDate.Text = list.DELDATE.To<string>();
                txtSDate.Text = list.SDATE.To<string>();
                txtName.Text = list.NAME;
                txtYName.Text = list.YNAME;

                cboJong.SelectedIndex = -1;
                if (!list.JONG.IsNullOrEmpty())
                {
                    cboJong.SelectedIndex = cboJong.FindString(list.JONG);
                }

                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkAm = (Controls.Find("chkAm" + (i).To<string>(), true)[0] as CheckBox);
                    chkAm.Checked = false;

                    CheckBox chkPrint = (Controls.Find("chkPrint" + (i).To<string>(), true)[0] as CheckBox);
                    chkPrint.Checked = false;
                }

                cboHang.SelectedIndex = -1;
                if (!list.HANG.IsNullOrEmpty())
                {
                    cboHang.SelectedIndex = cboHang.FindString(list.HANG);
                }

                chkSelect.Checked = false;
                if (list.GBSELECT == "Y")
                {
                    chkSelect.Checked = true;
                }

                chkDental.Checked = false;
                if (list.GBDENT == "Y")
                {
                    chkDental.Checked = true;
                }

                txtUCode.Text = list.UCODE;
                txtUCode.Text = hb.READ_HIC_CODE("09", list.UCODE);
                txtReExam.Text = list.REEXAM;
                //if (txtReExam.Text.Trim() != "")
                //{

                //}
                cboSuga.SelectedIndex = -1;
                if (!list.GBSUGA.IsNullOrEmpty())
                {
                    cboSuga.SelectedIndex = cboSuga.FindString(list.GBSUGA);
                }

                if (!list.GBSELF.IsNullOrEmpty())
                {
                    cboBuRate.SelectedIndex = list.GBSELF.To<int>();
                }

                //암항목
                if (!list.GBAM.IsNullOrEmpty())
                {
                    for (int i = 0; i < VB.L(list.GBAM, ","); i++)
                    {
                        CheckBox chkAm = (Controls.Find("chkAm" + (i).To<string>(), true)[0] as CheckBox);
                        if (VB.Pstr(list.GBAM, ",", i + 1) == "1")
                        {                            
                            chkAm.Checked = true;
                        }
                    }
                }

                //검사명표기
                txtExamName.Text = list.EXAMNAME;

                //접수증출력구분
                if (!list.GBPRINT.IsNullOrEmpty())
                {
                    for (int i = 0; i < VB.L(list.GBPRINT, ","); i++)
                    {
                        CheckBox chkPrint = (Controls.Find("chkPrint" + (i).To<string>(), true)[0] as CheckBox);
                        if (VB.Pstr(list.GBPRINT, ",", i + 1) == "1")
                        {   
                            chkPrint.Checked = true;
                        }
                    }
                }

                //검사안내
                txtRemark.Text = list.REMARK;
            }

            btnSave.Enabled = true;
            btnDelete.Enabled = true;
            if (FstrROWID.IsNullOrEmpty())
            {
                btnDelete.Enabled = false;
            }
            btnCancel.Enabled = true;
            btnExit.Enabled = false;
            txtName.Focus();
        }
    }
}
