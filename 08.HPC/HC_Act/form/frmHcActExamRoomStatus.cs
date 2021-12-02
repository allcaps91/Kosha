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
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmHcActExamRoomStatus.cs
/// Description     : 일반검진/종합검진 대기현황
/// Author          : 이상훈
/// Create Date     : 2020-10-18
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "신규" />

namespace HC_Act
{
    public partial class frmHcActExamRoomStatus : Form
    {
        ActingCheckService actingCheckService = null;
        HeaResultService heaResultService = null;
        WaitCheckService waitCheckService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HicResultActiveService hicResultActiveService = null;
        HicSangdamWaitService hicSangdamWaitService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        string FGstrHcPart; //"1" : 종합검진 / "2" : 일반검진
        string strPart = "";

        public frmHcActExamRoomStatus()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcActExamRoomStatus(string GstrHcPart)
        {
            InitializeComponent();
            FGstrHcPart = GstrHcPart;
            SetEvent();
        }

        void SetEvent()
        {
            actingCheckService = new ActingCheckService();
            heaResultService = new HeaResultService();
            waitCheckService = new WaitCheckService();
            heaSangdamWaitService = new HeaSangdamWaitService();
            hicResultActiveService = new HicResultActiveService();
            hicSangdamWaitService = new HicSangdamWaitService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.rdoGubun1.Click += new EventHandler(eRdoClick);
            this.rdoGubun2.Click += new EventHandler(eRdoClick);
            this.ssChk.CellClick += new CellClickEventHandler(eSpdClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            if (FGstrHcPart == "1")
            {
                rdoGubun1.Checked = true;   //일반검진
                ssChk_Hic.Visible = true;
                ssChk.Visible = false;
                ssChk_Hic.BringToFront();
                pnlHicGubun.Visible = true;
            }
            else
            {
                rdoGubun2.Checked = true;   //종합검진
                ssChk_Hic.Visible = false;
                ssChk.Visible = true;
                ssChk.BringToFront();
                pnlHicGubun.Visible = false;
            }

            eBtnClick(btnSearch, new EventArgs());
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
                string strGubun = "";

                if (rdoGubun1.Checked == true)
                {
                    strGubun = "1";
                }
                else if (rdoGubun2.Checked == true)
                {
                    strGubun = "2";
                }
                fn_ACTING_CHECK(strGubun, clsPublic.GstrSysDate);
            }
        }

        void eRdoClick(object sender, EventArgs e)
         {
            if (sender == rdoGubun1)
            {
                if (rdoGubun1.Checked == true)
                {
                    pnlHicGubun.Visible = true;
                    ssChk_Hic.Visible = true;
                    ssChk.Visible = false;
                    ssChk_Hic.BringToFront();
                }
                else
                {
                    pnlHicGubun.Visible = false;
                    ssChk_Hic.Visible = false;
                    ssChk.Visible = true;
                    ssChk.BringToFront();
                }
            }
            else if (sender == rdoGubun2)
            {
                if (rdoGubun2.Checked == true)
                {
                    pnlHicGubun.Visible = false;
                    ssChk_Hic.Visible = false;
                    ssChk.Visible = true;
                    ssChk.BringToFront();                    
                }
                else
                {
                    pnlHicGubun.Visible = true;
                    ssChk_Hic.Visible = true;
                    ssChk.Visible = false;
                    ssChk_Hic.BringToFront();
                }
            }

            eBtnClick(btnSearch, new EventArgs());
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssChk)
            {
                if (ssChk.ActiveSheet.NonEmptyRowCount == 0) return;

                if (e.Row < 0 || e.ColumnHeader == true) return;

                if (e.Column != 4) return;

                strPart = ssChk.ActiveSheet.Cells[e.Row, 6].Text.Trim();

                if (strPart.IsNullOrEmpty()) return;

                for (int i = 0; i < ssChk.ActiveSheet.RowCount; i++)
                {
                    ssChk.ActiveSheet.Cells[i, 5].Text = "";
                }

                //해당 검사실 대기명단
                if (rdoGubun1.Checked == true)  //일검
                {
                    List<HIC_SANGDAM_WAIT> Wait_List = hicSangdamWaitService.Read_Sangdam_Wait_List(strPart, "");

                    for (int i = 0; i < Wait_List.Count; i++)
                    {
                        ssChk.ActiveSheet.Cells[i, 5].Text = Wait_List[i].SNAME;
                        ssChk.ActiveSheet.Cells[i, 8].Text = Wait_List[i].WRTNO.ToString();
                    }
                }
                else if (rdoGubun2.Checked == true) //종검
                {
                    List<HEA_SANGDAM_WAIT> Wait_List = heaSangdamWaitService.Read_Sangdam_Wait_List(strPart, "");

                    for (int i = 0; i < Wait_List.Count; i++)
                    {
                        ssChk.ActiveSheet.Cells[i, 5].Text = Wait_List[i].SNAME;
                        ssChk.ActiveSheet.Cells[i, 8].Text = Wait_List[i].WRTNO.ToString();
                    }
                }

            }
        }

        void fn_ACTING_CHECK(string argGubun, string argJepDate)
        {
            int nRead = 0;
            int nRead2 = 0;
            int nRow = 0;
            int nRow1 = 0;
            int nRow2 = 0;

            string strPart = "";
            string strJong = "";
            string strOldHaRoom = "";
            string strOldHcRoom = "";
            string strChk1 = "";    //일검

            bool bColor = false;
            string strExams = "";
            string strTemp = "";
            //string strGbWait = "";
            List<string> strGbWait = new List<string>();
            string strOK = "";
            string strExName = "";
            bool boolSort = false;
            string strGB = "";

            int nREAD = 0;

            sp.Spread_All_Clear(ssChk);
            ssChk.ActiveSheet.RowCount = 20;

            strPart = argJepDate.Left(1);

            if (argGubun == "1")        //일반검진
            {
                if (rdoJepsuGubun2.Checked == true)
                {
                    strGB = "1";
                }
                else if (rdoJepsuGubun3.Checked == true)
                {
                    strGB = "2";
                }
                else
                {
                    strGB = "";
                }

                List<ACTING_CHECK> list = actingCheckService.ACTING_CHECK_HIC(0, argJepDate, 0, "", "");

                nREAD = list.Count;
                sp.SetfpsRowHeight(ssChk, 32);
                ssChk_Hic.ActiveSheet.RowCount = nREAD;
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        nRow1 += 1;
                        nRow += 1;

                        ssChk_Hic.ActiveSheet.Cells[i, 0].Text = list[i].NAME;
                        ssChk_Hic.ActiveSheet.Cells[i, 3].Text = list[i].ENTPART;

                        Application.DoEvents();

                        //청력(특수) 및 폐기능검사 대기인원수 표시
                        strGbWait.Clear();
                        if (list[i].NAME == "특수청력")
                        {
                            strGbWait.Add("12");
                            strGbWait.Add("13");
                        }
                        if (list[i].NAME == "폐활량")
                        {
                            strGbWait.Add("06");
                            strGbWait.Add("07");
                        }
                        //if (list[i].NAME == "자궁암검사")
                        if (list[i].NAME == "자궁경부암")
                        {
                            strGbWait.Add("14");
                        }
                        if (list[i].NAME == "구강상담")
                        {
                            strGbWait.Add("08");
                            strGbWait.Add("09");
                        }


                        if (i == 0)
                        {
                            if (!list[i].HCROOM.IsNullOrEmpty())
                            {
                                ssChk.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].HCROOM;
                            }
                            strOldHaRoom = list[i].HCROOM;
                            nRow2 = nRow;
                        }

                        if (!list[i].HCROOM.IsNullOrEmpty())
                        {
                            ssChk.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].HCROOM;
                        }

                        if (strOldHcRoom != list[i].HCROOM)
                        {
                            strOldHcRoom = list[i].HCROOM;
                            nRow2 = nRow;
                            if (bColor == true)
                            {
                                bColor = false;
                            }
                            else
                            {
                                bColor = true;
                            }
                        }

                        //상태점검
                        ssChk_Hic.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].NAME;
                        if (bColor == true)
                        {
                            ufn_Line_Color(ssChk_Hic, nRow - 1);
                        }
                        //상태점검
                        List<HIC_RESULT_ACTIVE> Actlist = hicResultActiveService.Read_Active_Hic(0, 0, argJepDate, strJong, list[i].ENTPART);
                        if (Actlist.Count > 0)
                        {
                            ssChk_Hic.ActiveSheet.Cells[nRow - 1, 1].Text = "미검";
                            ssChk_Hic.ActiveSheet.Cells[nRow - 1, 1].ForeColor = Color.FromArgb(255, 0, 0);
                            ssChk_Hic.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.FromArgb(255, 255, 255);
                        }
                        else
                        {
                            ssChk_Hic.ActiveSheet.Cells[nRow - 1, 1].Text = "완료";
                            ssChk_Hic.ActiveSheet.Cells[nRow - 1, 1].ForeColor = Color.FromArgb(0, 0, 0);
                            ssChk_Hic.ActiveSheet.Cells[nRow - 1, 1].BackColor = Color.FromArgb(255, 255, 255);
                        }

                        ssChk_Hic.ActiveSheet.Cells[nRow - 1, 1].Text = "";                        

                        //대기점검
                        List<WAIT_CHECK> Waitlist = waitCheckService.Read_Wait_Hic(argJepDate, list[i].ENTPART, strGB);

                        nRead2 = Waitlist.Count;

                        if (nRead2 == 0)
                        {
                            ssChk_Hic.ActiveSheet.Cells[nRow - 1, 2].Text = "0";
                        }
                        else
                        {
                            ssChk_Hic.ActiveSheet.Cells[nRow - 1, 2].Text = nRead2.To<string>();
                        }

                        //청력(특수) 및 폐기능검사 대기인원수를 표시함
                        if (strGbWait.Count > 0 && rdoJepsuGubun2.Checked == true)
                        {
                            ssChk_Hic.ActiveSheet.Cells[nRow - 1, 2].Text += "/" + waitCheckService.Read_Exam_Wait_Hic(strGbWait).To<string>();
                        }
                    }
                }
            }
            else if (argGubun == "2")   //종합검진
            {
                List<ACTING_CHECK> list = actingCheckService.ACTING_CHECKbyWrtNOGubun11(0, argJepDate);

                nRead = list.Count;
                sp.SetfpsRowHeight(ssChk, 32);
                ssChk.ActiveSheet.RowCount = nRead;
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        nRow1 += 1;
                        nRow += 1;

                        if (i == 0)
                        {
                            ssChk.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].HAROOM;
                            strOldHaRoom = list[i].HAROOM;
                            nRow2 = nRow;
                        }

                        ssChk.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].HAROOM;
                        if (strOldHaRoom != list[i].HAROOM)
                        {
                            strOldHaRoom = list[i].HAROOM;
                            nRow2 = nRow;
                            if (bColor == true)
                            {
                                bColor = false;
                            }
                            else
                            {
                                bColor = true;
                            }
                        }

                        ssChk.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].NAME;
                        if (bColor == true)
                        {
                            ufn_Line_Color(ssChk, nRow - 1);
                        }
                        //상태점검
                        List<HEA_RESULT> Rsltlist = heaResultService.Read_Result(0, list[i].HEAPART);

                        if (Rsltlist.Count == 0)
                        {
                            List<HEA_RESULT> Actlist2 = heaResultService.Read_Active(0, list[i].HEAPART);
                            if (Actlist2.Count > 0)
                            {
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].Text = "미검";
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].ForeColor = Color.FromArgb(255, 0, 0);
                            }
                            else
                            {
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].Text = "완료";
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].ForeColor = Color.FromArgb(0, 0, 0);
                            }
                        }
                        else
                        {
                            List<HEA_RESULT> Rsltlist2 = heaResultService.Read_Result(0, list[i].HEAPART);

                            if (Rsltlist2.Count > 0)
                            {
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].Text = "미검";
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].ForeColor = Color.FromArgb(255, 0, 0);
                            }
                            else
                            {
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].Text = "완료";
                                ssChk.ActiveSheet.Cells[nRow - 1, 1].ForeColor = Color.FromArgb(0, 0, 0);
                            }
                        }
                        ssChk.ActiveSheet.Cells[nRow - 1, 1].Text = "";

                        //대기점검
                        List<WAIT_CHECK> Waitlist = waitCheckService.Read_Wait(argJepDate, list[i].HEAPART);

                        nRead2 = Waitlist.Count;

                        if (nRead2 == 0)
                        {
                            ssChk.ActiveSheet.Cells[nRow - 1, 2].Text = "0";
                        }
                        else
                        {
                            ssChk.ActiveSheet.Cells[nRow - 1, 2].Text = nRead2.To<string>();
                        }

                        //검사실 대기인원
                        HEA_SANGDAM_WAIT ExamWaitlist = waitCheckService.Read_Exam_Wait(argJepDate, list[i].HAROOM);

                        if (!ExamWaitlist.IsNullOrEmpty())
                        {
                            ssChk.ActiveSheet.Cells[nRow - 1, 3].Text = ExamWaitlist.CNT.To<string>();

                            if (list[i].HAROOM == "99")
                            {
                                ssChk.ActiveSheet.Cells[nRow - 1, 3].Text = "";
                            }
                        }
                        ssChk.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].HEAPART;

                        if (i < list.Count - 1)
                        {
                            if (strOldHaRoom != list[i + 1].HAROOM)
                            {
                                if (nRow1 > 0)
                                {
                                    ssChk_Sheet1.AddSpanCell(nRow2 - 1, 3, nRow1, 1);
                                    ssChk_Sheet1.AddSpanCell(nRow2 - 1, 4, nRow1, 1);
                                    ssChk_Sheet1.AddSpanCell(nRow2 - 1, 6, nRow1, 1);
                                }
                                nRow1 = 0;
                            }
                        }
                    }

                    ////현 검사실 대기명단
                    //List<HEA_SANGDAM_WAIT> NowExamWaitlist = heaSangdamWaitService.Read_Now_Wait("");

                    //if (NowExamWaitlist.Count > 0)
                    //{
                    //    for (int i = 0; i < NowExamWaitlist.Count; i++)
                    //    {
                    //        ssChk.ActiveSheet.Cells[i, 5].Text = NowExamWaitlist[i].SNAME;
                    //        ssChk.ActiveSheet.Cells[i, 8].Text = NowExamWaitlist[i].WRTNO.To<string>();
                    //    }
                    //}
                }
            }
        }

        void ufn_Line_Color(FpSpread Spd, int argRow)
        {
            Spd.ActiveSheet.Cells[argRow, 0].BackColor = Color.FromArgb(255, 255, 128);
            Spd.ActiveSheet.Cells[argRow, 1].BackColor = Color.FromArgb(255, 255, 128);
            Spd.ActiveSheet.Cells[argRow, 2].BackColor = Color.FromArgb(255, 255, 128);
            Spd.ActiveSheet.Cells[argRow, 3].BackColor = Color.FromArgb(255, 255, 128);
        }
    }
}
