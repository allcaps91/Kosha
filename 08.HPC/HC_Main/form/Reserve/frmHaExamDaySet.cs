using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaExamDaySet.cs
/// Description     : 선택검사 예약
/// Author          : 김민철
/// Create Date     : 2020-09-02
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm기타검사예약.frm(Frm선택검사예약)" />

namespace HC_Main
{ 
    public partial class frmHaExamDaySet : Form
    {
        private string FstrSName = "";
        private long FnPano = 0;
        private string FstrSDate = "";
        private List<long> lstLtdCodes = new List<long>();
        private List<GROUPCODE_EXAM_DISPLAY> lstGED = new List<GROUPCODE_EXAM_DISPLAY>();
        private string FstrSTime = "";
        private bool bNEW = false;

        clsSpread cSpd = null;
        HeaResvExamService heaResvExamService = null;
        HeaCodeService heaCodeService = null;
        HeaResvLtdService heaResvLtdService = null;
        HeaResvSetService heaResvSetService = null;
        public frmHaExamDaySet()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHaExamDaySet(string argSName, long argPano, string argSDate, List<long> lstCodes, string argSTime, List<GROUPCODE_EXAM_DISPLAY> hEARESULT)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            this.FstrSName = argSName;
            this.FnPano = argPano;
            this.FstrSDate = argSDate;
            this.lstLtdCodes = lstCodes;
            this.FstrSTime = argSTime;
            this.lstGED = hEARESULT;
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            heaResvExamService = new HeaResvExamService();
            heaCodeService = new HeaCodeService();
            heaResvLtdService = new HeaResvLtdService();
            heaResvSetService = new HeaResvSetService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSet.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.SS2.EditModeOff += new EventHandler(eSpdEditOff);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                SS2.ActiveSheet.Cells[i, 0].Text = "";
                SS2.ActiveSheet.Cells[i, 1].Text = "";
                SS2.ActiveSheet.Cells[i, 2].Text = "";
                SS2.ActiveSheet.Cells[i, 3].Text = "";
                SS2.ActiveSheet.Cells[i, 4].Text = "";
            }

            Screen_Display();

            //btnExit.Enabled = clsHcVariable.GbClose;

        }

        private void eSpdEditOff(object sender, EventArgs e)
        {
            int nRow = SS2.ActiveSheet.ActiveRowIndex;
            int nCol = SS2.ActiveSheet.ActiveColumnIndex;

            if (nCol == 2)
            {
                if (SS2.ActiveSheet.Cells[nRow, nCol].Text != FstrSDate)
                {
                    SS2.ActiveSheet.Cells[nRow, nCol].BackColor = Color.LightPink;
                }
                else
                {
                    SS2.ActiveSheet.Cells[nRow, nCol].BackColor = Color.White;
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                if (bNEW)
                {
                    //신규 검사가 있으면 무조건 저장
                    if (ComFunc.MsgBoxQ("내용저장 후 종료하시겠습니까? ", "", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        Data_Save();
                        return;
                    }
                    else
                    {
                        this.Close();
                        return;
                    }
                    
                }
                else
                {
                    this.Close();
                    return;
                }
            }
            else if (sender == btnSet)
            {
                Set_Spread_SDateTime();
            }
            else if (sender == btnSave)
            {
                Data_Save();
            }
        }

        private void Data_Save()
        {
            string strCODE = "", strExName = "", strRDate = "", strGb = "", strROWID = "";
            string strTime = "", strAmPm = "";
            
            HEA_RESV_EXAM nHRE = null;

            clsHcVariable.LSTHaRESEXAM.Clear();

            try
            {
                if (!Error_Check(SS2)) { return; }

                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    strCODE = SS2.ActiveSheet.Cells[i, 0].Text;
                    strExName = SS2.ActiveSheet.Cells[i, 1].Text;
                    strRDate = SS2.ActiveSheet.Cells[i, 2].Text;
                    strTime = SS2.ActiveSheet.Cells[i, 3].Text;
                    strRDate = strRDate + " " + strTime;
                    strGb = VB.Format(VB.Val(SS2.ActiveSheet.Cells[i, 4].Text), "00");

                    strAmPm = string.Compare(strTime, "12:00") >= 0 ? "P" : "A";

                    strROWID = heaResvExamService.Read_Hes_Resv_Exam(FstrSDate, FnPano, strCODE);
                    
                    if (strROWID.IsNullOrEmpty())
                    {
                        nHRE = new HEA_RESV_EXAM
                        {
                            RTIME = strRDate,
                            PANO = FnPano,
                            SNAME = FstrSName,
                            GBEXAM = strGb,
                            EXAMNAME = strExName,
                            SDATE = FstrSDate,
                            ENTSABUN = clsType.User.IdNumber.To<long>(0),
                            EXCODE = strCODE,
                            AMPM = strAmPm,
                            RowStatus = RowStatus.Insert
                        };
                    }
                    else
                    {
                        nHRE = new HEA_RESV_EXAM
                        {
                            RTIME = strRDate,
                            PANO = FnPano,
                            SNAME = FstrSName,
                            GBEXAM = strGb,
                            SDATE = FstrSDate,
                            ENTSABUN = clsType.User.IdNumber.To<long>(0),
                            EXCODE = strCODE,
                            AMPM = strAmPm,
                            RID = strROWID,
                            RowStatus = RowStatus.Update
                        };
                    }

                    clsHcVariable.LSTHaRESEXAM.Add(nHRE);
                }

                this.Close();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private bool Error_Check(FpSpread sS2)
        {
            string strExName = string.Empty;
            string strRDate = string.Empty;
            string strTime = string.Empty;
            string strGb = string.Empty;

            long nTotal = 0, nGaCnt = 0, nCNT = 0;

            try
            {
                for (int i = 0; i < sS2.ActiveSheet.RowCount; i++)
                {
                    strExName = sS2.ActiveSheet.Cells[i, 1].Text;
                    strRDate = sS2.ActiveSheet.Cells[i, 2].Text;
                    strTime = sS2.ActiveSheet.Cells[i, 3].Text;
                    strGb = VB.Format(VB.Val(sS2.ActiveSheet.Cells[i, 4].Text), "00");

                    if (strRDate == "" || strTime == "")
                    {
                        MessageBox.Show("검사일자 및 시각 입력누락!!", "입력확인");
                        return false;
                    }

                    if (string.Compare(strRDate, FstrSDate) < 0)
                    {
                        MessageBox.Show("검사일자가 수검예정일 이전입니다.", "입력확인");
                        return false;
                    }

                    if (strGb == "")
                    {
                        MessageBox.Show(strExName + " 검사가 기초코드(13)에 등록되지 않았습니다.", "입력확인");
                        return false;
                    }

                    nTotal = 0; nGaCnt = 0; nCNT = 0;

                    //해당일자 선택검사 총원(오전, 오후)
                    if (clsHcVariable.GbGaResvLtd)
                    {
                        List<HEA_RESV_LTD> lstHRL = heaResvLtdService.GetListAmPmInwonBySDateGubun(strRDate, strGb);

                        if (lstHRL.Count > 0)
                        {
                            for (int j = 0; j < lstHRL.Count; j++)
                            {
                                if (string.Compare(strTime, "12:00") < 0)
                                {
                                    nTotal += lstHRL[j].AMINWON;        //정원(오전)
                                }
                                else
                                {
                                    nTotal += lstHRL[j].PMINWON;        //정원(오후)
                                }
                            }
                        }

                        //해당일자 선택검사 기존 예약인원
                        string strTDate = Convert.ToDateTime(strRDate).AddDays(1).ToShortDateString();
                        string strAMPM = string.Compare(strTime, "12:00") < 0 ? "A" : "P";

                        nCNT = heaResvExamService.GetExistCountbyPanoGbExam(strRDate, strTDate, FnPano, strGb, strAMPM, lstLtdCodes);

                    }
                    else
                    {
                        List<HEA_RESV_SET> lstHRS = heaResvSetService.GetListBySDateGubun(strRDate, strGb);

                        if (lstHRS.Count > 0)
                        {
                            for (int j = 0; j < lstHRS.Count; j++)
                            {
                                if (lstHRS[j].GBRESV == "1")            //예약
                                {
                                    if (string.Compare(strTime, "12:00") < 0)
                                    {
                                        nTotal += lstHRS[j].AMINWON;        //정원(오전)
                                    }
                                    else
                                    {
                                        nTotal += lstHRS[j].PMINWON;        //정원(오후)
                                    }
                                }
                                else                                   //가예약
                                {
                                    if (string.Compare(strTime, "12:00") < 0)
                                    {
                                        nGaCnt += lstHRS[j].GAINWONAM;        //가예약(오전)
                                    }
                                    else
                                    {
                                        nGaCnt += lstHRS[j].GAINWONPM;        //가예약(오후)
                                    }
                                }
                            }
                        }

                        //해당일자 선택검사 기존 예약인원
                        string strTDate = Convert.ToDateTime(strRDate).AddDays(1).ToShortDateString();
                        string strAMPM = string.Compare(strTime, "12:00") < 0 ? "A" : "P";

                        nCNT = heaResvExamService.GetExistCountbyPanoGbExam(strRDate, strTDate, FnPano, strGb, strAMPM);

                        //회사별 가예약
                        HEA_RESV_LTD iHRL = heaResvLtdService.GetSumAmPmJanByGubun(strRDate, strGb);
                        if (!iHRL.IsNullOrEmpty())
                        {
                            if (string.Compare(strTime, "12:00") < 0)
                            {
                                nCNT += iHRL.AMJAN;     //정원(오전)
                            }
                            else
                            {
                                nCNT += iHRL.PMJAN;     //정원(오후)
                            }
                        }
                    }

                    if (nTotal <= (nGaCnt + nCNT))
                    {
                        clsPublic.GstrMsgList = "                     ※ 예약정원 초과 ※" + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += "☞선택검사명: " + strExName + ComNum.VBLF;
                        clsPublic.GstrMsgList += "☞검사일자: " + strRDate + ComNum.VBLF;
                        clsPublic.GstrMsgList += "================================" + ComNum.VBLF;
                        clsPublic.GstrMsgList += "         총원: " + nTotal + "명 중," + ComNum.VBLF;
                        clsPublic.GstrMsgList += "      가예약: " + nGaCnt + "명," + ComNum.VBLF;
                        clsPublic.GstrMsgList += "   예약인원: " + nCNT + "명 " + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += "그래도 접수 하시겠습니까?";

                        if (MessageBox.Show(clsPublic.GstrMsgList, "확인사항", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void Set_Spread_SDateTime()
        {
            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                if (SS2.ActiveSheet.Cells[i, 2].Text == "") { SS2.ActiveSheet.Cells[i, 2].Text = FstrSDate; }
                if (SS2.ActiveSheet.Cells[i, 3].Text == "") { SS2.ActiveSheet.Cells[i, 3].Text = FstrSTime; }
            }
        }

        private void Screen_Display()
        {
            int nCNT = 0;
            string strOK = "";
            string strRTime = "";

            if (lstGED.IsNullOrEmpty() || lstGED.Count == 0)
            {
                this.Hide();
                return;
            }

            cSpd.Spread_Clear_Simple(SS2);

            for (int i = 0; i < clsHcVariable.LSTHaRESEXAM.Count; i++)
            {
                nCNT += 1;
                if (SS2.ActiveSheet.RowCount < nCNT) { SS2.ActiveSheet.RowCount = nCNT; }

                SS2.ActiveSheet.Cells[nCNT - 1, 0].Text = clsHcVariable.LSTHaRESEXAM[i].EXCODE.To<string>("");
                SS2.ActiveSheet.Cells[nCNT - 1, 1].Text = clsHcVariable.LSTHaRESEXAM[i].EXAMNAME.To<string>("");

                strRTime = heaResvExamService.GetRTimeByPanoExCodeSDate(FnPano, clsHcVariable.LSTHaRESEXAM[i].EXCODE, FstrSDate, "HH24:MI");
                if (strRTime.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[nCNT - 1, 2].Text ="";
                    SS2.ActiveSheet.Cells[nCNT - 1, 3].Text = "";
                }
                else
                {
                    SS2.ActiveSheet.Cells[nCNT - 1, 2].Text = clsHcVariable.LSTHaRESEXAM[i].RDATE.To<string>("");
                    SS2.ActiveSheet.Cells[nCNT - 1, 3].Text = clsHcVariable.LSTHaRESEXAM[i].STIME.To<string>("");
                }
                
                SS2.ActiveSheet.Cells[nCNT - 1, 4].Text = clsHcVariable.LSTHaRESEXAM[i].GBEXAM.To<string>("");

                if (SS2.ActiveSheet.Cells[nCNT - 1, 2].Text != "" && SS2.ActiveSheet.Cells[nCNT - 1, 2].Text != FstrSDate)
                {
                    SS2.ActiveSheet.Cells[nCNT - 1, 2].BackColor = Color.FromArgb(255, 192, 255);
                }
                else
                {
                    SS2.ActiveSheet.Cells[nCNT - 1, 2].BackColor = Color.White;
                }
            }


            nCNT = SS2.ActiveSheet.RowCount;

            for (int i = 0; i < lstGED.Count; i++)
            {
                if (lstGED[i].EXNAME == "관상동맥CT(조영제포함)")
                {
                    i = i;
                }

                if (lstGED[i].RowStatus != RowStatus.Delete && lstGED[i].ETCEXAM == "Y")
                {
                    if (SS2.ActiveSheet.RowCount == 0)
                    {
                        Set_Spread_Display(ref nCNT, i);
                    }
                    else
                    {
                        strOK = "";

                        for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                        {
                            if (SS2.ActiveSheet.Cells[j, 0].Text == lstGED[i].EXCODE.To<string>("").Trim())
                            {
                                strOK = "OK";
                                break;
                            }
                        }

                        if (strOK == "")
                        {
                            Set_Spread_Display(ref nCNT, i);
                        }
                    }
                }
            }

            this.ActiveControl = btnSave;
        }

        private void Set_Spread_Display(ref int nCNT, int i)
        {
            string strRTime = string.Empty;
            string strGbn2 = string.Empty;

            nCNT += 1;
            if (SS2.ActiveSheet.RowCount < nCNT) { SS2.ActiveSheet.RowCount = nCNT; }

            SS2.ActiveSheet.Cells[nCNT - 1, 0].Text = lstGED[i].EXCODE;
            SS2.ActiveSheet.Cells[nCNT - 1, 1].Text = lstGED[i].EXNAME;

            strRTime = heaResvExamService.GetRTimeByPanoExCodeSDate(FnPano, lstGED[i].EXCODE, FstrSDate, "HH24:MI");

            if (strRTime.To<string>("").Trim() == "")
            {
                SS2.ActiveSheet.Cells[nCNT - 1, 2].Text = FstrSDate;
                SS2.ActiveSheet.Cells[nCNT - 1, 3].Text = FstrSTime;
                bNEW = true;    //신규검사가 있으면 표시
            }
            else
            {
                SS2.ActiveSheet.Cells[nCNT - 1, 2].Text = VB.Left(strRTime, 10);
                SS2.ActiveSheet.Cells[nCNT - 1, 3].Text = VB.Right(strRTime, 5);
            }

            strGbn2 = heaCodeService.GetGubun2ByGubunCode("13", lstGED[i].EXCODE);

            SS2.ActiveSheet.Cells[nCNT - 1, 4].Text = strGbn2;

            if (SS2.ActiveSheet.Cells[nCNT - 1, 2].Text != "" && SS2.ActiveSheet.Cells[nCNT - 1, 2].Text != FstrSDate)
            {
                SS2.ActiveSheet.Cells[nCNT - 1, 2].BackColor = Color.FromArgb(255, 192, 255);
            }
            else
            {
                SS2.ActiveSheet.Cells[nCNT - 1, 2].BackColor = Color.White;
            }
        }
    }
}
