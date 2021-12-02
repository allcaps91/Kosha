using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPanSpcExam.cs
/// Description     : 특검 소견코드별 관련검사항목 설정
/// Author          : 이상훈
/// Create Date     : 2019-12-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSpcExam.frm(HcPan93)" />

namespace ComHpcLibB
{
    public partial class frmHcPanSpcExam : Form
    {
        HicExcodeService hicExcodeService = null;
        HicSpcSogenexamExcodeService hicSpcSogenexamExcodeService = null;
        HicSpcSogenexamService hicSpcSogenexamService = null;
        HicSpcScodeService hicSpcScodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrCode = "";
        string FstrGbSuga = "";
        string FstrROWID = "";

        public frmHcPanSpcExam()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            hicExcodeService = new HicExcodeService();
            hicSpcSogenexamExcodeService = new HicSpcSogenexamExcodeService();
            hicSpcSogenexamService = new HicSpcSogenexamService();
            hicSpcScodeService = new HicSpcScodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
        }
        
        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            string strData = "";

            SS1_Sheet1.Rows.Get(-1).Height = 20F;
            SS2_Sheet1.Rows.Get(-1).Height = 20F;
            SS3_Sheet1.Rows.Get(-1).Height = 20F;

            //SS2_Sheet1.Columns.Get(6).Visible = false;  //ROWID
            //SS2_Sheet1.Columns.Get(7).Visible = false;  //변경

            txtView.Text = "";

            cboPart2.Items.Clear();
            cboPart2.Items.Add("0.전체항목");
            cboPart2.Items.Add("1.신체계측");
            cboPart2.Items.Add("2.검사실검사");
            cboPart2.Items.Add("3.방사선검사");
            cboPart2.Items.Add("4.기타검사");
            cboPart2.Items.Add("5.액팅코드");
            cboPart2.Items.Add("9.금액코드");
            cboPart2.SelectedIndex = 0;

            cboSogen.Items.Clear();
            cboSogen.Items.Add("");
            cboSogen.Items.Add("01.청력");
            cboSogen.Items.Add("02.호흡기");
            cboSogen.Items.Add("03.CO");
            cboSogen.Items.Add("04.고혈압");
            cboSogen.Items.Add("05.당뇨");
            cboSogen.Items.Add("06.이상지질");
            cboSogen.Items.Add("07.신장");
            cboSogen.Items.Add("08.흉부");
            cboSogen.Items.Add("09.비만");
            cboSogen.Items.Add("10.호흡기질환");
            cboSogen.Items.Add("11.혈액질환");
            cboSogen.Items.Add("12.심질환");
            cboSogen.Items.Add("13.간장");
            cboSogen.Items.Add("14.공복혈당");
            cboSogen.Items.Add("15.입파구");
            cboSogen.Items.Add("16.기타");
            cboSogen.SelectedIndex = 0;

            fn_Screen_Clear();
            fn_SogenCode_Display();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                int nRead = 0;
                int nRow = 0;
                string strGubun = "";
                string strView = ""; 

                strGubun = VB.Left(cboPart2.Text, 1);
                txtView.Text = txtView.Text.Trim();
                strView = txtView.Text;

                //검사항목을 SELECT
                List<HIC_EXCODE> list = hicExcodeService.GetCodeHNamebyPartHName(strGubun, strView);

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = list[i].CODE.Trim();
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].HNAME.Trim();
                }
            }
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
                SS3.Focus();
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strExCode = "";
                int nRow = 0;

                strExCode = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();

                //자료가 있는지 Check
                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    if (strExCode == SS2.ActiveSheet.Cells[i, 2].Text.Trim())
                    {
                        MessageBox.Show(i + "번줄에 이미 " + strExCode + " 코드가 있습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //검사명을 READ
                HIC_EXCODE list = hicExcodeService.GetHNameYNamebyCode(strExCode);

                if (list.IsNullOrEmpty())
                {
                    MessageBox.Show("검사코드가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                SS2.ActiveSheet.RowCount = SS2.ActiveSheet.NonEmptyRowCount + 200;
                nRow = SS2.ActiveSheet.NonEmptyRowCount;
                SS2.ActiveSheet.Cells[nRow, 0].Text = "";
                SS2.ActiveSheet.Cells[nRow, 1].Text = strExCode;
                SS2.ActiveSheet.Cells[nRow, 2].Text = list.HNAME.Trim();
                SS2.ActiveSheet.Cells[nRow, 3].Text = list.YNAME.Trim();
            }
            else if (sender == SS3)
            {
                fn_Screen_Clear();
                FstrCode = SS3.ActiveSheet.Cells[e.Row, 0].Text.Trim();

                txtCode.Text = FstrCode;
                txtName.Text = hb.READ_SpcSCode_Name(FstrCode); //소견
                fn_Screen_Display();
            }
        }

        void fn_Screen_Clear()
        {
            txtCode.Text = "";
            txtName.Text = "";
            lblMsg.Text = "";

            sp.Spread_All_Clear(SS2);   //묶음항목

            FstrCode = "";
            FstrROWID = "";
            FstrGbSuga = "";
            SS1.Enabled = false;
            grpData.Enabled = false;
            pnlMain.Enabled = false;

            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            cboSogen.SelectedIndex = 0;
        }

        void fn_SogenCode_Display()
        {
            //판정소견코드를 Display
            List<HIC_SPC_SCODE> list = hicSpcScodeService.GetCodeNameby();

            SS3.ActiveSheet.RowCount = list.Count;
            for (int i = 0; i < SS3.ActiveSheet.RowCount; i++)
            {
                SS3.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                SS3.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
            }
        }

        void fn_Screen_Display()
        {
            string strGbSuga = "";
            string strSuDate = "";
            int nRead = 0;

            //검사 묶음코드 상세내역
            List<HIC_SPC_SOGENEXAM_EXCODE> list = hicSpcSogenexamExcodeService.GetItembySogenCode(FstrCode);

            nRead = list.Count;
            SS2.ActiveSheet.RowCount = nRead;

            for (int i = 0; i < nRead; i++)
            {
                SS2.ActiveSheet.Cells[i, 0].Text = "";
                SS2.ActiveSheet.Cells[i, 1].Text = list[i].EXCODE.Trim();
                SS2.ActiveSheet.Cells[i, 2].Text = list[i].HNAME.Trim();
                SS2.ActiveSheet.Cells[i, 3].Text = list[i].YNAME.Trim();
            }

            HIC_SPC_SOGENEXAM list2 = hicSpcSogenexamService.GetHangbySogenCode(FstrCode);

            if (!list2.IsNullOrEmpty())
            {
                cboSogen.SelectedIndex = int.Parse(list2.HANG);
            }

            SS2.Enabled = true;
            SS1.Enabled = true;
            grpData.Enabled = true;

            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }
    }
}
