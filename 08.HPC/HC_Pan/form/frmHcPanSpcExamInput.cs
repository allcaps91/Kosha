using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanSpcExamInput.cs
/// Description     : 특수검사입력
/// Author          : 이상훈
/// Create Date     : 2019-12-26
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm특수검사입력.frm(Frm특수검사입력)" />

namespace HC_Pan
{
    public partial class frmHcPanSpcExamInput : Form
    {
        HicJepsuService hicJepsuService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicResultService hicResultService = null;
        HicRescodeService hicRescodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnRowNo;       //메모리타자기 위치 저장용
        long FnClickRow;    //Help를 Click한 Row
        string FstrSex;     //성별
        string FstrJepDate;
        int FnAge;
        string FstrGjYear;
        long FnWRTNO;

        public frmHcPanSpcExamInput(long nWrtNo)
        {
            InitializeComponent();
            FnWRTNO = nWrtNo;
            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicResultExCodeService = new HicResultExCodeService();
            hicResultService = new HicResultService();
            hicRescodeService = new HicRescodeService();

            this.Load += new EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS2.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS2.Change += new ChangeEventHandler(eSpdChange);
            this.SS2.KeyDown += new KeyEventHandler(eSpdKeyDown);
            this.SS2.LeaveCell += new LeaveCellEventHandler(eSpdLeaveCell);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            SS2_Sheet1.Columns.Get(0).Visible = false;  //검사코드
            SS2_Sheet1.Columns.Get(7).Visible = false;  //결과값코드
            SS2_Sheet1.Columns.Get(8).Visible = false;  //변경
            SS2_Sheet1.Columns.Get(9).Visible = false;  //ROWID
            SS2_Sheet1.Columns.Get(10).Visible = false; //Result Type
        }

        void eFormActivated(object sender, EventArgs e)
        {
            int nRow = 0;
            int nREAD = 0;
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";

            //접수일자를 읽음
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            FstrJepDate = list.JEPDATE;
            FstrSex = list.SEX;

            //검사결과를 읽음
            List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetItembyWrtNoExCodeSpc(FnWRTNO);

            nREAD = list2.Count;
            nRow = 0;
            SS2.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list2[i].EXCODE.Trim();
                strResult = list2[i].RESULT.Trim();
                strResCode = list2[i].RESCODE.Trim();
                strResultType = list2[i].RESULTTYPE.Trim();
                strGbCodeUse = list2[i].GBCODEUSE.Trim();

                SS2.ActiveSheet.Cells[i, 0].Text = list2[i].EXCODE.Trim();
                SS2.ActiveSheet.Cells[i, 1].Text = list2[i].HNAME.Trim();
                SS2.ActiveSheet.Cells[i, 2].Text = strResult;

                //A103(비만도)는 자동계산(입력금지)
                if (strGbCodeUse == "N" || strExCode == "A103")
                {
                    if (strExCode != "A151" && strExCode != "TH01" && strExCode != "TH02")
                    {
                        SS2.ActiveSheet.Cells[i, 3].Locked = true;
                        SS2.ActiveSheet.Cells[i, 3].Text = "";
                    }
                }

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[i, 4].Text = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list2[i].MIN_M, list2[i].MAX_M, list2[i].MIN_F, list2[i].MAX_F);

                SS2.ActiveSheet.Cells[i, 6].Text = strNomal;
                SS2.ActiveSheet.Cells[i, 7].Text = strResCode;

                if (list2[i].EXCODE.Trim() == "TH01" || list2[i].EXCODE.Trim() == "TH02")
                {
                    SS2.ActiveSheet.Cells[i, 7].Text = "022";
                }
                SS2.ActiveSheet.Cells[i, 8].Text = "";
                SS2.ActiveSheet.Cells[i, 9].Text = list2[i].RID.Trim();
                SS2.ActiveSheet.Cells[i, 10].Text = list2[i].RESULTTYPE.Trim();
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

            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                if (e.Column != 1)
                {
                    return;
                }
                MessageBox.Show(SS2.ActiveSheet.Cells[e.Row, 2].Text, "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            SS2.ActiveSheet.Cells[e.Row, 8].Text = "Y";
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                string strResCode = "";
                string strData = "";

                if (e.Column != 3)
                {
                    return;
                }

                strResCode = SS2.ActiveSheet.Cells[e.Row, 7].Text.Trim();

                if (strResCode.IsNullOrEmpty())
                {
                    sp.Spread_All_Clear(ssList);
                    FnClickRow = 0;
                    return;
                }

                FnClickRow = e.Row;

                //자료를 READ
                List<HIC_RESCODE> list = hicRescodeService.GetCodeNamebyBindGubun(strResCode);

                ssList.ActiveSheet.RowCount = list.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    ssList.ActiveSheet.Cells[i, 0].Text = list[i].CODE;
                    ssList.ActiveSheet.Cells[i, 1].Text = list[i].NAME;
                }
            }
            else if (sender == ssList)
            {
                string strGubun = "";
                string strCode = "";

                if (FnClickRow == 0)
                {
                    return;
                }

                strCode = ssList.ActiveSheet.Cells[e.Row, 0].Text;

                SS2.ActiveSheet.Cells[(int)FnClickRow, 2].Text = strCode;
                strGubun = SS2.ActiveSheet.Cells[(int)FnClickRow, 7].Text;
                SS2.ActiveSheet.Cells[(int)FnClickRow, 4].Text = hb.READ_ResultName(strGubun, strCode);
                SS2.ActiveSheet.Cells[(int)FnClickRow, 8].Text = "Y";   //변경여부

            }
        }

        void eSpdKeyDown(object sender, KeyEventArgs e)
        {
            string strResult = "";
            string strResCode = "";
            string strResType = "";

            if (FnRowNo < 1 || FnRowNo > SS2.ActiveSheet.RowCount)
            {
                return;
            }

            strResult = "";
            switch (e.KeyCode)
            {
                case Keys.F4:
                    strResult = "정상";
                    break;
                case Keys.F5:
                    strResult = "과체중";
                    break;
                case Keys.F6:
                    strResult = "비만";
                    break;
                case Keys.F7:
                    strResult = "염증";
                    break;
                case Keys.F8:
                    strResult = "비정상";
                    break;
                case Keys.F9:
                    strResult = "미실시";
                    break;
                default:
                    return;
            }

            SS2.ActiveSheet.Cells[(int)FnRowNo, 7].Text.Trim();
            SS2.ActiveSheet.Cells[(int)FnRowNo, 10].Text.Trim();

            if (!strResult.IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[(int)FnRowNo, 2].Text = strResult;
                SS2.ActiveSheet.Cells[(int)FnRowNo, 8].Text = "Y";
                FnRowNo += 1;
                if (FnRowNo > SS2.ActiveSheet.RowCount)
                {
                    FnRowNo = SS2.ActiveSheet.RowCount;
                }
                SS2.ActiveSheet.SetActiveCell((int)FnRowNo, 2);
            }
        }

        void eSpdLeaveCell(object sender, LeaveCellEventArgs e)
        {
            string strGubun = "";
            string strCode = "";

            FnRowNo = e.NewRow;

            if (e.Column != 2)
            {
                return;
            }

            strCode = SS2.ActiveSheet.Cells[e.Row, 2].Text.Trim();
            if (strCode.IsNullOrEmpty())
            {
                return;
            }

            if (strCode == "미실시")
            {
                return;
            }

            strGubun = SS2.ActiveSheet.Cells[e.Row, 7].Text.Trim();
            if (strGubun.IsNullOrEmpty())
            {
                return;
            }

            if (strCode.IsNullOrEmpty())
            {
                SS2.ActiveSheet.Cells[e.Row, 4].Text = "";
            }
            else
            {
                SS2.ActiveSheet.Cells[e.Row, 4].Text = hb.READ_ResultName(strGubun, strCode);
                if (SS2.ActiveSheet.Cells[e.Row, 4].Text.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[e.Row, 2].Text = "";
                    MessageBox.Show(strCode + "가 결과코드값에 등록이 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }
    }
}
