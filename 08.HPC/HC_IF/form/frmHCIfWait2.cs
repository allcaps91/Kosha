using ComBase;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using ComBase.Controls;

/// <summary>
/// Class Name      : HC_IF
/// File Name       : frmHCIfWait2.cs
/// Description     : 대기순번(2번)
/// Author          : 이상훈
/// Create Date     : 2020-03-03
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm대기순번2.frm(Frm대기순번2)" />

namespace HC_IF
{
    public partial class frmHCIfWait2 : Form
    {
        HicBcodeService hicBcodeService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HeaJepsuService heaJepsuService = null;
        HicSangdamWaitService hicSangdamWaitService = null;
        HicJepsuService hicJepsuService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        int FnHrefTime = 0;
        int FnDoctNo = 0;
        int FnDispCNT = 0;
        long FnTimer1_CNT = 0;
        long FnTimerGuide = 0;
        string FstrFlag = "";
        string FstrPart = "";
        string FstrRoom = "";
        int FnPage = 0;

        string GstrProgram = "";
        string GstrCenterGbn = "";

        public frmHCIfWait2()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            hicBcodeService = new HicBcodeService();
            heaSangdamWaitService = new HeaSangdamWaitService();
            heaJepsuService = new HeaJepsuService();
            hicJepsuService = new HicJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.timer1.Tick += new EventHandler(eTimerTick);
            this.timerGuide.Tick += new EventHandler(eTimerTick);
            this.pnlTimeBar.DoubleClick += new EventHandler(ePnlDblClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strRec = "";
            string strLog = "";
            string strGubun = "";
            string strIpAddress = "':";

            //fn_PreFormCheck();
            clsCompuInfo.SetComputerInfo();
            ComFunc.ReadSysDate(clsDB.DbCon);

            FnTimerGuide = 0;
            FnPage = 0;
            clsPublic.GstrPassProgramID = "";
            strRec = "";
            strGubun = "BAS_표시장비검사실설정";
            strIpAddress = clsCompuInfo.gstrCOMIP + ":2";

            lblGuide.Parent = pictureBox1;
            lblGuide.BackColor = Color.Transparent;
            lblGuide.BringToFront();

            //lblDoct0.Parent = pictureBox3;
            //lblDoct0.BackColor = Color.Transparent;
            //lblDoct0.BringToFront();

            //lblDoct1.Parent = pictureBox3;
            //lblDoct1.BackColor = Color.Transparent;
            //lblDoct1.BringToFront();

            strRec = hicBcodeService.GetCodeNamebyGubunCode(strGubun, strIpAddress);

            if (string.IsNullOrEmpty(strRec))
            {
                MessageBox.Show("대기순번 설정값이 없이 실행이 불가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            GstrProgram = "종합검진";
            GstrCenterGbn = "2";
            if (VB.Pstr(strRec, "{}", 1) == "일반")
            {
                GstrProgram = "일반건진";
                GstrCenterGbn = "1";
            }

            //프린트 세팅
            //string sPrintName1 = "센터영수증";
            //string sPrintName2 = "카드영수증";
            //string strPrintName1 = string.Empty;
            //string strPrintName2 = string.Empty;

            //using (clsPrint CP = new clsPrint())
            //{
            //    strPrintName1 = CP.getPrinter_Chk(sPrintName1.ToUpper());
            //    strPrintName2 = CP.getPrinter_Chk(sPrintName2.ToUpper());
            //}

            //if (strPrintName1 == "")
            //{
            //    //MessageBox.Show("프린터 설정 오류입니다. 전산정보팀에 연락바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            fn_Screen_Clear();

            FstrPart = "";
            pnlTimeBar.Text = "";
            lblRoom.Text = "";
            lblRoomName.Text = "";

            pnlTimeBar.Text = VB.Left(clsPublic.GstrSysDate, 4) + "년 " + VB.Mid(clsPublic.GstrSysDate, 6, 2) + "월 " + VB.Right(clsPublic.GstrSysDate, 2) + "일  " + VB.Left(clsPublic.GstrSysTime, 2) + "시 " + VB.Right(clsPublic.GstrSysTime, 2) + "분";
            lblCall.Visible = false;

            //형식:종검/건진{}방번호{}방명칭{}검사파트{}
            FstrRoom = VB.Pstr(strRec, "{}", 2);
            lblRoom.Text = FstrRoom;
            lblRoomName.Text = FstrRoom;
            lblRoomName.Text = VB.Pstr(strRec, "{}", 3);
            lblRoomName.Text = lblRoomName.Text.Replace(System.Environment.NewLine, "\r\n");
            FstrPart = VB.Pstr(strRec, "{}", 4);

            lblDoct0.Visible = false;
            lblDoct1.Visible = false;
            if (!VB.Pstr(strRec, "{}", 5).IsNullOrEmpty())
            {
                lblDoct0.Visible = true;
                lblDoct1.Visible = true;
                lblDoct0.Text = VB.Pstr(strRec, "{}", 5);
                lblDoct1.Text = VB.Pstr(strRec, "{}", 6);
            }

            if (GstrProgram == "종합검진")
            {
                if (lblRoomName.Text == "폐기능검사2") lblRoom.Text = "7";
                if (lblRoomName.Text == "안과검사실") lblRoom.Text = "8";
                if (lblRoomName.Text == "청력검사실") lblRoom.Text = "9";
                fn_Screen_Display_Hea();
            }
            else
            {
                fn_Screen_Display_Hic();
            }
            timer1.Enabled = true;

            //PC자동종료 제거
            //SQL = " SELECT  ";
            //SQL = SQL + ComNum.VBLF + "    B.GRPCD, B.BASCD, B.REMARK, B.REMARK1, B.VFLAG1, B.DISPSEQ ,  ";
            //SQL = SQL + ComNum.VBLF + "    P.VALUEV, P.VALUEN, P.DELGB, P.ROWID";
            //SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_PMPA + "BAS_BASCD B ";
            //SQL = SQL + ComNum.VBLF + "INNER JOIN  " + ComNum.DB_PMPA + "BAS_PCCONFIG P ";
            //SQL = SQL + ComNum.VBLF + "     ON B.GRPCD = P.GUBUN  ";
            //SQL = SQL + ComNum.VBLF + "     AND B.BASCD = P.CODE  ";
            //SQL = SQL + ComNum.VBLF + "     AND P.IPADDRESS =  '" + clsCompuInfo.gstrCOMIP + "'";
            //SQL = SQL + ComNum.VBLF + "WHERE B.GRPCDB = '프로그램PC세팅'  ";
            //SQL = SQL + ComNum.VBLF + "  AND B.GRPCD = '기타PC설정'  ";
            //SQL = SQL + ComNum.VBLF + "  AND B.BASCD = '" + clsCompuInfo.gstrCOMIP + "'  ";
            //SQL = SQL + ComNum.VBLF + "  AND B.APLFRDATE <= '" + strAPLDATE + "'";
            //SQL = SQL + ComNum.VBLF + "  AND B.APLENDDATE >= '" + strAPLDATE + "'";
            //SQL = SQL + ComNum.VBLF + "  AND B.USECLS = '1'";
            //에 등록
            clsType.CompEnv.NotAutoLogOut = true;
        }

        void eTimerTick(object sender, EventArgs e)
        {
            if (sender == timer1)
            {
                fn_Screen_Clear();
                if (GstrProgram == "종합검진")
                {
                    fn_Screen_Display_Hea();
                }
                else
                {
                    fn_Screen_Display_Hic();
                }
                //this.Show();
                //Application.DoEvents();
            }            
            else if (sender == timerGuide)
            {
                FnTimerGuide += 1;
                if (FnTimerGuide >= 5)
                {
                    FnTimerGuide = 1;
                }

                switch (FnTimerGuide)
                {
                    case 1:
                        lblGuide.Text = "고객님을 정성껏 모시겠습니다.";
                        break;
                    case 2:
                        lblGuide.Text = "정확한 수검자 확인을 위하여";
                        break;
                    case 3:
                        lblGuide.Text = "성함 및 주민번호를 질문 드리고 있습니다.";
                        break;
                    case 4:
                        lblGuide.Text = "협조하여 주시면 고맙겠습니다.";
                        break;
                    default:
                        break;
                }
            }
        }

        void ePnlDblClick(object sender, EventArgs e)
        {
            if (sender == pnlTimeBar)
            {
                this.Close();
                Application.Exit();
            }
        }

        /// <summary>
        /// 종합건진 대기모니터
        /// </summary>
        void fn_Screen_Display_Hea()
        {
            int nRow = 0;
            int nREAD = 0;
            string strOK = "";
            int nStart = 0;
            int nEnd = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            pnlTimeBar.Text = VB.Left(clsPublic.GstrSysDate, 4) + "년 " + VB.Mid(clsPublic.GstrSysDate, 6, 2) + "월 " + VB.Right(clsPublic.GstrSysDate, 2) + "일  " + VB.Left(clsPublic.GstrSysTime, 2) + "시 " + VB.Right(clsPublic.GstrSysTime, 2) + "분";

            List<HEA_SANGDAM_WAIT> list = heaSangdamWaitService.GetItembyGubunEntTime(string.Format("{0:00}", FstrRoom), clsPublic.GstrSysDate, DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToShortDateString());

            nREAD = list.Count;            
            for (int i = 0; i < nREAD; i++)
            {
                if (nRow < 11)
                {
                    //내시경실은 수면내시경 대상자는 노란색으로 표시                    
                    if (FstrRoom == "9")
                    {
                        strOK = "OK";
                    }
                    else
                    {
                        //삭제되지 않은 명단만 표시
                        strOK = "";
                        if (heaJepsuService.GetCountbyWrtNo(list[i].WRTNO) > 0)
                        {
                            strOK = "OK";
                        }
                    }

                    if (strOK == "OK" && nRow <= 10)
                    {
                        ssList.ActiveSheet.Cells[i, 0].Text = (i + 1).ToString();
                        ssList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                        ssList.ActiveSheet.Cells[i, 2].Text = list[i].ENT;
                        ssList.ActiveSheet.Cells[i, 3].Text = list[i].WRTNO.ToString();
                        if (FstrRoom == "9")
                        {
                            ssList.ActiveSheet.Cells[i, 2].Text = "";
                        }
                        else
                        {
                            ssList.ActiveSheet.Cells[i, 2].Text = list[i].ENT;
                        }

                        if (!list[i].CALL.IsNullOrEmpty())
                        {
                            ssList.ActiveSheet.Cells[i, 2].Text = "진료중";
                        }
                        else
                        {
                            ssList.ActiveSheet.Cells[i, 2].Text = "검사중";
                        }
                    }

                    if (FstrRoom == "9")    //내시경실
                    {
                        ssList.ActiveSheet.Cells[i, 3].Text = "";
                        if (list[i].GBENDO == "Y")   //수면내시경
                        {
                            ssList.ActiveSheet.Cells[i, 1].ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFF0000")); ;
                        }
                        else
                        {
                            ssList.ActiveSheet.Cells[i, 1].ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&H0&")); ;
                        }
                    }
                    nRow += 1;
                }
            }
        }

        /// <summary>
        /// 일반건진 대기모니터
        /// </summary>
        void fn_Screen_Display_Hic()
        {
            int nRow = 0;
            int nREAD = 0;
            string strList = "";
            string strOK = "";
            int result = 0;
            string strSName = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            pnlTimeBar.Text = VB.Left(clsPublic.GstrSysDate, 4) + "년 " + VB.Mid(clsPublic.GstrSysDate, 6, 2) + "월 " + VB.Right(clsPublic.GstrSysDate, 2) + "일  " + VB.Left(clsPublic.GstrSysTime, 2) + "시 " + VB.Right(clsPublic.GstrSysTime, 2) + "분";
            if (lblCall.Visible == true)
            {
                lblCall.Visible = false;
            }

            strSName = "{수검자호출}";

            int nCnt = hicSangdamWaitService.GetCountbyGubunSName(FstrRoom, strSName);

            //수검자 Call
            if (nCnt > 0)
            {
                lblCall.Visible = true;

                clsDB.setBeginTran(clsDB.DbCon);

                result = hicSangdamWaitService.DeletebyGubunSName(FstrRoom, strSName);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                Application.DoEvents();
            }

            List<HIC_SANGDAM_WAIT> list = hicSangdamWaitService.GetItembyGubunEntTime(string.Format("{0:00}", FstrRoom), clsPublic.GstrSysDate, DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToShortDateString());

            nREAD = list.Count;
            strList = ",";
            for (int i = 0; i < nREAD; i++)
            {
                strOK = "OK";
                if (VB.InStr(strList, "," + list[i].PANO + ",") > 0)
                {
                    strOK = "";
                }
                strList += list[i].PANO + ",";
                if (nRow >= 11)
                {
                    strOK = "";
                }

                if (strOK == "OK")
                {
                    //삭제되지 않은 명단만 표시
                    if (hicJepsuService.GetCntbyWrtNo(list[i].WRTNO) > 0)
                    {
                        ssList.ActiveSheet.Cells[i, 0].Text = (i + 1).ToString();
                        ssList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                        ssList.ActiveSheet.Cells[i, 2].Text = list[i].ENT;
                        ssList.ActiveSheet.Cells[i, 3].Text = list[i].WRTNO.ToString();

                        if (!list[i].CALL.IsNullOrEmpty())
                        {
                            if (string.Compare(FstrRoom, "15") >= 0 && string.Compare(FstrRoom, "19") <= 0)
                            {
                                ssList.ActiveSheet.Cells[i, 2].Text = "진료중";
                            }
                        }
                        else if (FstrRoom == "8" || FstrRoom == "9")
                        {
                            ssList.ActiveSheet.Cells[i, 2].Text = "상담중";
                        }
                        else
                        {
                            ssList.ActiveSheet.Cells[i, 2].Text = "검사중";
                        }

                        nRow += 1;
                    }
                }
            }
        }

        void fn_Screen_Clear()
        {
            sp.Spread_All_Clear(ssList);

            ssList.ActiveSheet.RowCount = 11;
            ssList_Sheet1.Rows[-1].Height = 74;
        }
    }
}
