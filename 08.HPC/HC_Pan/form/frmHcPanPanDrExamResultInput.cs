using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanPanDrExamResultInput.cs
/// Description     : 폐활량 및 야간작업 결과 입력
/// Author          : 이상훈
/// Create Date     : 2019-12-27
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm판정의사검사결과입력.frm(Frm판정의사검사결과입력)" />

namespace HC_Pan
{
    public partial class frmHcPanPanDrExamResultInput : Form
    {
        HicResultService hicResultService = null;
        HicJepsuService hicJepsuService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicRescodeService hicRescodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrJepDate;
        string FstrPano;
        long FnWRTNO;
        int FnRow;
        long FnClickRow;    // Help를 Click한 Row
        string FstrSex;

        public frmHcPanPanDrExamResultInput(long nWrtNo)
        {
            InitializeComponent();
            FnWRTNO = nWrtNo;
            SetEvent();
        }

        void SetEvent()
        {
            hicResultService = new HicResultService();
            hicJepsuService = new HicJepsuService();
            hicResultExCodeService = new HicResultExCodeService();
            hicRescodeService = new HicRescodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS2.CellClick += new CellClickEventHandler(eSpdClick);
            this.List1.SelectedIndexChanged += new EventHandler(eListSelectedIndexChanged);
            this.SS2.Change += new ChangeEventHandler(eSpdChange);
        }

        private void eListSelectedIndexChanged(object sender, EventArgs e)
        {
            string strGubun = "";
            string strCODE = "";

            strCODE = VB.Pstr(List1.Text, ".", 1).Trim();

            SS2.ActiveSheet.Cells[0, 2].Text = strCODE;
            strGubun = SS2.ActiveSheet.Cells[0, 7].Text.Trim();
            SS2.ActiveSheet.Cells[0, 4].Text = hb.READ_ResultName(strGubun, strCODE);
            SS2.ActiveSheet.Cells[0, 8].Text = "Y"; //변경여부

        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            this.ssPatInfo_Sheet1.Columns.Get(6).Visible = false;

            this.SS2_Sheet1.Columns.Get(7).Visible = false;
            this.SS2_Sheet1.Columns.Get(8).Visible = false;
            this.SS2_Sheet1.Columns.Get(9).Visible = false;
            this.SS2_Sheet1.Columns.Get(10).Visible = false;

            List1.Items.Clear();
            sp.Spread_All_Clear(SS2);

            FnClickRow = 0;

            if (FnWRTNO > 0)
            {
                fn_Screen_Display();
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
                string strResult = "";
                string strCODE = "";
                string strROWID = "";
                string strPanjeng = "";
                string strNewPan = "";
                string strChange = "";
                string strResCode = "";
                int nResult = 0;
                string strJepDate = "";
                int result = 0;

                strJepDate = hm.GET_HIC_JepsuDate(FnWRTNO);

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    strCODE = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                    strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                    strPanjeng = SS2.ActiveSheet.Cells[i, 5].Text.Trim();
                    strChange = SS2.ActiveSheet.Cells[i, 8].Text.Trim();
                    strROWID = SS2.ActiveSheet.Cells[i, 9].Text.Trim();
                    strResCode = SS2.ActiveSheet.Cells[i, 7].Text.Trim();
                    strNewPan = hm.ExCode_Result_Panjeng(strCODE, strResult, FstrSex, strJepDate, "");

                    if (strChange == "Y" || strPanjeng != strNewPan)
                    {
                        //결과를 저장

                        HIC_RESULT item = new HIC_RESULT();

                        item.RESULT = strResult;
                        item.PANJENG = strNewPan;
                        item.RESCODE = strResCode;
                        item.RID = strROWID;

                        result = hicResultService.UpdatebyRowId(item);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show(i + "번줄 검사결과를 등록중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                string strResCode = "";
                string strData = "";

                strResCode = SS2.ActiveSheet.Cells[e.Row, 7].Text.Trim();

                FnClickRow = e.Row;
                List1.Items.Clear();

                //자료를 READ
                List<HIC_RESCODE> list = hicRescodeService.GetCodeNamebyGubun(strResCode);

                for (int i = 0; i < list.Count; i++)
                {
                    List1.Items.Add(list[i].CODE.Trim() + "." + list[i].NAME.Trim());
                }
            }
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (sender == SS2)
            {
                SS2.ActiveSheet.Cells[e.Row, 8].Text = "Y";
            }
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            int nRow = 0;
            string strNextDay = "";
            string strCODE = "";
            string strRemark = "";
            string strJong = "";
            string strXrayno = "";
            string strSex = "";
            string strPart = "";
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strGbHelp = "";
            string strData = "";
            string strJumin = "";
            string strJepDate = "";
            string strGbRead = "";
            long nResultDr = 0;
            long nHeaPano = 0;

            //Screen_Injek_display       '인적사항을 Display
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            strJepDate = list.JEPDATE;
            strSex = list.SEX;

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PTNO;
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE.ToString();
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.ToString()); //회사명
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = strJepDate;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);  //건진유형

            //Screen_Exam_Items_display  '검사항목을 Display
            List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoInExCode(FnWRTNO, clsHcVariable.G37_DOCT_ENTCODE);

            nREAD = list2.Count;
            SS2.ActiveSheet.RowCount = nREAD;
            nRow = 0;
            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (nRow > SS2.ActiveSheet.RowCount)
                    {
                        SS2.ActiveSheet.RowCount = nRow;
                    }

                    strExCode = list2[i].EXCODE.Trim();
                    strResult = list2[i].RESULT.Trim();
                    strResCode = list2[i].RESCODE;
                    strResultType = list2[i].RESULTTYPE.Trim();
                    strGbCodeUse = list2[i].GBCODEUSE.Trim();

                    SS2.ActiveSheet.Cells[nRow - 1, 0].Text = list2[i].EXCODE.Trim();
                    SS2.ActiveSheet.Cells[nRow - 1, 1].Text = list2[i].HNAME.Trim();
                    SS2.ActiveSheet.Cells[nRow - 1, 2].Text = strResult;
                    //A103(비만도)는 자동계산(입력금지)
                    if (strGbCodeUse == "N" || strExCode == "A103")
                    {
                        if (strExCode != "A151" && strExCode != "TH01" && strExCode != "TH02")
                        {
                            SS2.ActiveSheet.Cells[nRow - 1, 3].Locked = true;
                            SS2.ActiveSheet.Cells[nRow - 1, 3].Text = "";
                        }
                    }

                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            SS2.ActiveSheet.Cells[nRow - 1, 4].Text = hb.READ_ResultName(strResCode, strResult);
                        }
                    }

                    strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list2[i].MIN_M, list2[i].MAX_M, list2[i].MIN_F, list2[i].MAX_F);
                    SS2.ActiveSheet.Cells[nRow - 1, 6].Text = strNomal;
                    SS2.ActiveSheet.Cells[nRow - 1, 7].Text = strResCode;
                    if (list2[i].EXCODE.Trim() == "A151")
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, 7].Text = "007";
                    }
                    if (list2[i].EXCODE.Trim() == "TH01" || list2[i].EXCODE.Trim() == "TH02")
                    {
                        SS2.ActiveSheet.Cells[nRow - 1, 7].Text = "022";
                    }
                    SS2.ActiveSheet.Cells[nRow - 1, 8].Text = "";
                    SS2.ActiveSheet.Cells[nRow - 1, 9].Text = list2[i].RID.Trim();
                    SS2.ActiveSheet.Cells[nRow - 1, 10].Text = list2[i].RESULTTYPE.Trim();
                }
            }
            SS2.ActiveSheet.RowCount = nRow;

            if (SS2.ActiveSheet.RowCount > 0)
            {
                strResCode = SS2.ActiveSheet.Cells[0, 7].Text.Trim();
            }
            else
            {
                strResCode = "";
            }
            FnClickRow = 0;

            List1.Items.Clear();

            //자료를 READ
            List<HIC_RESCODE> list3 = hicRescodeService.GetCodeNamebyBindGubun(strResCode);

            nREAD = list3.Count;
            
            for (int i = 0; i < nREAD; i++)
            {
                List1.Items.Add(list3[i].CODE.Trim() + "." + list3[i].NAME.Trim());
            }

            FstrPano = ssPatInfo.ActiveSheet.Cells[0, 0].Text;
            FstrJepDate = ssPatInfo.ActiveSheet.Cells[0, 4].Text;
        }
    }
}
