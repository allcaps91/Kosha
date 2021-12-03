using ComBase;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaUseWard_New.cs
/// Description     : 약속처방 등록 및 조회
/// Author          : 이상훈
/// Create Date     : 2019-10-07
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain81.frm(FrmUseWard_new)" />

namespace ComHpcLibB
{
    public partial class frmHaUseWard_New : Form
    {
        HicCodeService hicCodeService = null;
        HeaCodeService heaCodeService = null;
        HeaResultwardService heaResultwardService = null;
        HicExcodeService hicExcodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        long FnJobSabun;

        public frmHaUseWard_New()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicCodeService = new HicCodeService();
            heaCodeService = new HeaCodeService();
            heaResultwardService = new HeaResultwardService();
            hicExcodeService = new HicExcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnFind.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS1.EditModeOff += new EventHandler(eSpdEditOff);
        }

        private void eSpdEditOff(object sender, EventArgs e)
        {
            int nRow = SS1.ActiveSheet.ActiveRowIndex;

            FarPoint.Win.Spread.Row row;
            row = SS1.ActiveSheet.Rows[nRow];
            float rowSize = row.GetPreferredHeight();
            row.Height = rowSize;
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nRow = 0;

            cboPart2.Items.Clear();
            cboPart2.Items.Add(" ");
            cboPart2.Items.Add("1.신체계측");
            cboPart2.Items.Add("2.검사실검사");
            cboPart2.Items.Add("3.방사선검사");
            cboPart2.Items.Add("4.기타검사");
            cboPart2.Items.Add("9.금액코드");

            cboStep.Items.Clear();
            cboStep.Items.Add("전체");
            cboStep.Items.Add("1");
            cboStep.Items.Add("2");
            cboStep.Items.Add("3");
            cboStep.SelectedIndex = 0;

            lblSts.Text = "";
            lblSts1.Text = "";

            SS1_Sheet1.Columns.Get(4).Visible = false;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnFind)
            {
                int nRead = 0;
                int nRow = 0;
                string strPart = "";
                string strName = "";

                strPart = VB.Left(cboPart2.Text, 1);
                strName = txtName.Text.Trim();

                //자료를 SELECT
                List<HIC_EXCODE> list = hicExcodeService.GetCodeHNamebyPartHname(strPart, strName);

                nRead = list.Count;
                SS2.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < nRead; i++)
                {
                    SS2.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                    SS2.ActiveSheet.Cells[i, 1].Text = list[i].HNAME;
                }
            }
            else if (sender == btnSearch)
            {
                sp.Spread_All_Clear(SS1);
                fn_Screen_Display();
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

                strTitle = "종검판정 코드";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnSave)
            {
                int nSeqNo = 0;
                string strCODE = "";
                string strWard = "";
                string strROWID = "";
                string strChk = "";
                string strStep = "";
                int result = 0;

                strCODE = lblSts.Text.Trim();
                strStep = cboStep.Text.Trim();

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strChk = "";
                    strWard = "";
                    strROWID = "";

                    strChk = SS1.ActiveSheet.Cells[i, 0].Text;
                    if (strChk == "True")
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = "";
                    }
                    if (SS1.ActiveSheet.Cells[i, 1].Text.Trim() == "")
                    {
                        nSeqNo = 0;
                    }
                    else
                    {
                        nSeqNo = int.Parse(SS1.ActiveSheet.Cells[i, 1].Text.Trim());
                    }
                    strWard = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                    strStep = SS1.ActiveSheet.Cells[i, 3].Text.Trim();
                    strROWID = SS1.ActiveSheet.Cells[i, 4].Text.Trim();
                    strWard = strWard.Replace("'", "`");

                    if (strChk == "True")   //코드삭제
                    {
                        if (strROWID != "")
                        {
                            result = heaResultwardService.DeletebyRowId(strROWID);
                        }
                    }
                    else
                    {
                        if (strCODE.Trim() != "")
                        {
                            if (strROWID.Trim() == "" && strWard != "")
                            {
                                //result = heaResultwardService.InsertItem(clsType.User.IdNumber, nSeqNo, strCODE.Trim(), strStep, "03", strWard);
                                result = heaResultwardService.InsertItem(FnJobSabun.ToString(), nSeqNo, strCODE.Trim(), strStep, "03", strWard);
                            }
                            else if (strROWID != "")
                            {
                                result = heaResultwardService.UpdateWardNameStepSeqNobyRowId(strROWID, strWard, strStep, nSeqNo);
                            }
                        }
                    }

                    if (result < 0)
                    {
                        MessageBox.Show("코드 저장 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                sp.Spread_All_Clear(SS1);
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                lblSts.Text = SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                lblSts1.Text = SS2.ActiveSheet.Cells[e.Row, 1].Text.Trim();

                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void fn_Screen_Display()
        {
            int nRow = 0;
            int nRead = 0;
            string strStep = "";
            string strGubun = "";

            SS1_Sheet1.Columns.Get(4).Visible = false;
            if (lblSts.Text.Trim()  == "")
            {
                MessageBox.Show("먼저 코드를 선택하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //판정의사 사번
            FnJobSabun = heaCodeService.GetCodebyGubun("05");

            if (cboStep.Text.Trim() != "전체")
            {
                strStep = cboStep.Text.Trim();
            }

            strGubun = "03";

            //기존의 자료를 읽음
            List<HEA_RESULTWARD> list = heaResultwardService.GetItembySabunCodeStep(FnJobSabun, lblSts.Text, strGubun, strStep);
            SS1.ActiveSheet.RowCount = list.Count+5;
            if (nRead > 50)
            {
                SS1.ActiveSheet.RowCount = nRead;
            }

            for (int i = 0; i < list.Count; i++)
            {
                SS1.ActiveSheet.Cells[i, 1].Text = list[i].SEQNO.ToString();
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].WARDNAME;
                SS1.ActiveSheet.Cells[i, 3].Text = list[i].STEP;
                SS1.ActiveSheet.Cells[i, 4].Text = list[i].ROWID;

                FarPoint.Win.Spread.Row row;
                row = SS1.ActiveSheet.Rows[i];
                float rowSize = row.GetPreferredHeight();
                row.Height = rowSize;
            }
        }
    }
}
