using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComPmpaLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;



/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPrintBar.cs
/// Description     : 바코드중 출력 로직/// Author          : 김경동
/// Create Date     : 2020-07-01
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "" />
namespace ComHpcLibB
{
    public partial class frmHcPrintBar : Form
    {
        HIC_JEPSU nHJ = null;
        clsHcFunc cHF = null;
        ComFunc cf = null;
        clsHaBase hb = null;

        List<GROUPCODE_EXAM_DISPLAY> Gxd = null;

        FpSpread ssPrint;
        HicSunapdtlGroupcodeService hicSunapdtlGroupcodeService = null;
        HicJepsuService hicJepsuService = null;

        string strRetValue1 = ""; //VB에서 FstrRetValue
        string strList = "";
        string strBiMan = "";
        int nCount = 0;

        //출장검진출력용 변수(2021-03-02)
        string FstrJuso1 = "";
        string FstrJuso2 = "";
        string FstrJuso3 = "";
        string FstrGbNaksang = "";
        string FstrExamList = "";
        string FstrExamList1 = "";
        string FstrExamList2 = "";
        string FstrPrtGubun = "";
        string FstrSecond = "";
        string FstrJobName = "";
        string FstrUCodes = "";
        string FstrExamName = "";
        string FstrUAName = "";

        public frmHcPrintBar()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        /// <summary>
        /// 수검자 정보 Display
        /// </summary>
        /// <param name="argPtno"></param>
        /// <param name="argYear"></param>

        /// <summary>
        /// 건진접수증
        /// </summary>
        /// <param name="aHJ">HIC_JEPSU</param>
        /// <param name="arglist">UA4종 검사여부 체크</param>
        /// <param name="argValue1">검사항목</param>
        /// <param name="argBiMan">56종(학생검진) 비만여부</param>
        /// <param name="argCount">검사항목 갯수</param>
        public frmHcPrintBar(HIC_JEPSU aHJ, List<GROUPCODE_EXAM_DISPLAY> arglist, string argValue1, string argBiMan, int argCount)
        {
            InitializeComponent();
            SetControl();
            SetEvent();
            nHJ = aHJ;
            Gxd = arglist;
            nCount = argCount;

            strRetValue1 = argValue1;
            strBiMan = argBiMan;
        }

        private void SetControl()
        {
            nHJ = new HIC_JEPSU();
            cHF = new clsHcFunc();
            cf = new ComFunc();
            hb = new clsHaBase();

            hicSunapdtlGroupcodeService = new HicSunapdtlGroupcodeService();
            hicJepsuService = new HicJepsuService();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            this.Hide();

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread SPR = new clsSpread();
            clsPrint CP = new clsPrint();

            //건진접수증
            long nWrtno = 0;
            long nCNT = 0;
            long nUACnt = 0;
            long nLen = 0;
            string[] strUACode = new string[6];
            string strJuso = "";
            string strJuso1 = "";
            string strJuso2 = "";
            string strJuso3 = "";
            string strJobName = "";
            string strUCodes = "";
            string strExamName = "";
            string strTalk = "";
            string strPrint = "";
            string strGbNaksang = "";
            string strSecond = "";

            //strExamList는 가셔야할곳 1번째줄, 2번쨰줄, 3번쨰줄
            string strExamList = "";
            string strExamList1 = "";
            string strExamList2 = "";
            //C#추가
            string strUAName = "";
            string strPrintName = "";

            //'성명 성별 나이 종류 주민 날짜 회사코드
            //GstrRetValue = Trim(TxtSName.Text) & "^^" & ComboSex.Text & "^^" & TxtAge.Text & "^^" & ComboJong.Text & "^^" & _
            //TxtJumin1.Text & "-" & Left(TxtJumin2.Text, 1) & "******" & "^^" & GstrSysDate & "^^" & TxtLtdCode.Text & "^^" & TxtXrayNo.Text & "^^"
            strJuso = "";
            strJuso = cf.TextBox_2_MultiLine("■주소:" + VB.Trim(nHJ.JUSO1) + " " + VB.Trim(nHJ.JUSO2), 34);
            strJuso1 = VB.Trim(VB.Pstr(strJuso, "{{@}}", 1));
            strJuso2 = VB.Trim(VB.Pstr(strJuso, "{{@}}", 2));
            strJuso3 = VB.Trim(VB.Pstr(strJuso, "{{@}}", 3));

            List<HIC_SUNAPDTL_GROUPCODE> list = hicSunapdtlGroupcodeService.GetExamNamebyWrtNo(nHJ.WRTNO);

            for (int i = 0; i < list.Count; i++)
            {
                if (strExamName == "")
                {
                    strExamName = VB.Trim(list[i].EXAMNAME);
                }
                else
                {
                    strExamName = strExamName + "," + VB.Trim(list[i].EXAMNAME);
                }
            }

            if (strExamName != "") { nCNT = nCNT + 1; }

            strGbNaksang = "";
            if (cHF.GET_Naksang_Flag(nHJ.AGE, nHJ.JEPDATE, nHJ.PTNO) == "Y")
            {
                strGbNaksang = "★낙상주의★";
                FstrGbNaksang = strGbNaksang;
            }
            //바코드 라인(접수번호)
            SSBar.ActiveSheet.Cells[2, 1].Text = nHJ.WRTNO.ToString();

            //이름(성별/나이)
            SSBar.ActiveSheet.Cells[2, 5].Text = nHJ.SNAME + "(" + nHJ.SEX + "/" + nHJ.AGE.ToString() + ")";
            //주민번호
            SSBar.ActiveSheet.Cells[3, 5].Text = VB.Left(nHJ.JUMINNO, 6) + "-" + VB.Mid(nHJ.JUMINNO, 7, 1) + "******";



            //검사항목표시
            SSBar.ActiveSheet.Cells[4, 1].Text = "가셔야할곳☞  " + VB.Trim(VB.Pstr(strRetValue1, "^^", 1));

            strExamList = ""; strExamList1 = ""; strExamList2 = "";
            FstrExamList = ""; FstrExamList1 = ""; FstrExamList2 = "";

            for (int i = 2; i <= nCount; i++)
            {
                if (nLen < 2500)
                {
                    strExamList = strExamList + VB.Trim(VB.Pstr(strRetValue1, "^^", i));
                    nLen = nLen + (((VB.Len(VB.Trim(VB.Pstr(strRetValue1, "^^", i))) / 2) * 200) + 100);
                }
                else if (nLen >= 2500 && nLen < 5000)
                {
                    strExamList1 = strExamList1 + VB.Trim(VB.Pstr(strRetValue1, "^^", i));
                    nLen = nLen + (((VB.Len(VB.Trim(VB.Pstr(strRetValue1, "^^", i))) / 2) * 200) + 100);
                }
                else if (nLen >= 5000)
                {
                    strExamList2 = strExamList2 + VB.Trim(VB.Pstr(strRetValue1, "^^", i));
                    nLen = nLen + (((VB.Len(VB.Trim(VB.Pstr(strRetValue1, "^^", i))) / 2) * 200) + 100);
                }
            }

            FstrExamList = strExamList;
            FstrExamList1 = strExamList1;
            FstrExamList2 = strExamList2;
            SSBar.ActiveSheet.Cells[5, 1].Text = strExamList;
            SSBar.ActiveSheet.Cells[6, 1].Text = strExamList1;
            SSBar.ActiveSheet.Cells[7, 1].Text = strExamList2;

            //UA4종 검사여부 체크
            for (int i = 0; i < Gxd.Count; i++)
            {
                if (Gxd[i].EXCODE.Trim() == "A111" || Gxd[i].EXCODE.Trim() == "A112" || Gxd[i].EXCODE.Trim() == "A113" || Gxd[i].EXCODE.Trim() == "A114" || Gxd[i].EXCODE.Trim() == "LU38")
                {
                    nUACnt = nUACnt + 1;
                }
                if (Gxd[i].EXCODE.Trim() == "LU38") { strUACode[1] = "UA9종"; }
                if (Gxd[i].EXCODE.Trim() == "LU54") { strUACode[1] = "UA10종"; }             //LU54 한개만 있으면 UA10종
                if (Gxd[i].EXCODE.Trim() == "A236") { strUACode[2] = "코티닌"; }             //코티닌 검사
                if (Gxd[i].EXCODE.Trim() == "TZ46") { strUACode[2] = "코티닌"; }             //코티닌 검사
                if (Gxd[i].EXCODE.Trim() == "E923") { strUACode[3] = "TBPE"; }               //TBPE 검사
                if (Gxd[i].EXCODE.Trim() == "E922") { strUACode[4] = "마약"; }               //마약검사
            }

            if (nUACnt == 4) { strUACode[0] = "UA4종"; }

            strSecond = "";
            HIC_JEPSU item = hicJepsuService.GetAutoJepByWrtno(nWrtno);
            if (!item.IsNullOrEmpty())
            {
                if (item.AUTOJEP == "Y")
                {
                    strSecond = "★★★(2차)";
                    FstrSecond = strSecond;
                }
            }

            if (!nHJ.WEBPRINTREQ.IsNullOrEmpty()) { strTalk = "OK"; }
            if (!nHJ.GBCHK3.IsNullOrEmpty()) { strPrint = "OK"; }

            //가셔야할곳 부터 표시
            SSBar.ActiveSheet.Cells[8, 1].Text = "검사 완료후 접수창구에 본 증을";
            SSBar.ActiveSheet.Cells[9, 1].Text = "제출하셔야 처리가 완료됩니다.";
            if (strTalk == "OK")
            {
                //SSBar.ActiveSheet.Cells[10, 1].Text = "★알림톡결과지    ☏054-260-8188";
                SSBar.ActiveSheet.Cells[10, 1].Text = "★결과지: 알림톡";
                FstrPrtGubun = "★결과지: 알림톡";
            }
            else if (strPrint == "OK")
            {
                SSBar.ActiveSheet.Cells[10, 1].Text = "★결과지: 방문수령";
                FstrPrtGubun = "★결과지: 방문수령";
                //SSBar.ActiveSheet.Cells[10, 1].Text = "                 ☏054-260-8188";
            }
            else if (nHJ.GBJUSO == "Y")
            {
                SSBar.ActiveSheet.Cells[10, 1].Text = "★결과지: 우편발송(집)";
                FstrPrtGubun = "★결과지: 우편발송(집)";
            }
            else if (nHJ.GBJUSO == "N")
            {
                SSBar.ActiveSheet.Cells[10, 1].Text = "★결과지: 우편발송(회사)";
                FstrPrtGubun = "★결과지: 우편발송(회사)";
            }
            else if (nHJ.GBJUSO == "E")
            {
                SSBar.ActiveSheet.Cells[10, 1].Text = "★결과지: 별도발송";
                FstrPrtGubun = "★결과지: 별도발송";
            }
            else
            {
                SSBar.ActiveSheet.Cells[10, 1].Text = "";
                FstrPrtGubun = "";
            }

            

            strUAName = "";
            for (int i = 0; i <= 5; i++)
            {
                if (strUACode[i] != "")
                {
                    nCNT = nCNT + 1;
                    strUAName = strUAName + " " + strUACode[i];
                }
            }
            if (strUAName == "")
            {
                SSBar.ActiveSheet.Cells[11, 1].Text = strExamName;
                FstrExamName = strExamName;
            }
            else
            {
                SSBar.ActiveSheet.Cells[11, 1].Text = strUAName;
                FstrUAName = strUAName;
            }

            //공백
            SSBar.ActiveSheet.Cells[12, 1].Text = "";
            SSBar.ActiveSheet.Cells[13, 1].Text = "";

            //출장검진 흉부촬영번호 표시
            if (nHJ.GBCHUL == "Y")
            {
                SSBar.ActiveSheet.Cells[14, 1].Text = "접수번호: " + nHJ.WRTNO + "   " + nHJ.XRAYNO;
            }
            else
            {
                SSBar.ActiveSheet.Cells[14, 1].Text = "접수번호: " + nHJ.WRTNO + "   " + strGbNaksang;
            }

            SSBar.ActiveSheet.Cells[15, 1].Text = "등록번호: " + nHJ.PTNO + "   " + strSecond;

            //2019-02-18 접수자 표시 의뢰서 작업 (최종작업자 표시)
            if (nHJ.PTNO != "")
            {
                HIC_JEPSU item2 = hicJepsuService.GetItemByPanoJepdateGjjong(nHJ.PTNO, nHJ.JEPDATE, nHJ.GJJONG);
                strJobName = clsHelpDesk.READ_INSA_NAME(clsDB.DbCon, item2.JOBSABUN.ToString());
                strUCodes = item2.UCODES;

                FstrJobName = strJobName;
                FstrUCodes = strUCodes;
            }

            if (strJobName != "")
            {
                SSBar.ActiveSheet.Cells[15, 1].Text = "등록번호: " + nHJ.PTNO + "   " + strSecond + "접수자: " + strJobName;
            }

            if (nHJ.LTDCODE.ToString() != "")
            {
                SSBar.ActiveSheet.Cells[16, 1].Text = cf.Read_Ltd_Name(clsDB.DbCon, nHJ.LTDCODE.ToString());
            }

            if (VB.Left(nHJ.GJJONG, 2) == "56" && strBiMan == "비만")
            {
                SSBar.ActiveSheet.Cells[17, 1].Text = "★★★(B)";
            }

            if (nHJ.GJJONG == "11" && !strUCodes.IsNullOrEmpty())
            {
                SSBar.ActiveSheet.Cells[18, 1].Text = "11. 일반+특수" + " " + nHJ.JEPDATE;
            }
            else
            {
                SSBar.ActiveSheet.Cells[18, 1].Text = nHJ.GJJONG + ". " + hb.READ_GjJong_Name(nHJ.GJJONG) + "  " + nHJ.JEPDATE;
            }

            //주소
            SSBar.ActiveSheet.Cells[19, 1].Text = strJuso1;
            SSBar.ActiveSheet.Cells[20, 1].Text = strJuso2;
            SSBar.ActiveSheet.Cells[21, 1].Text = strJuso3;

            FstrJuso1 = strJuso1;
            FstrJuso2 = strJuso2;
            FstrJuso3 = strJuso3;



            //2021-03-02
            if (nHJ.GBCHUL== "Y")
            {
                Print_Chul();
            }
            else
            {
                strPrintName = CP.getPmpaBarCodePrinter("신용카드"); //Default :신용카드(접수창구용 설정이름)
                setMargin = new clsSpread.SpdPrint_Margin(0, 0, 20, 40, 0, 0);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, false, false);
                ssPrint = SSBar;
                SPR.setSpdPrint(ssPrint, false, setMargin, setOption, "", "", strPrintName);

                ComFunc.Delay(1500);

                ssPrint.Dispose();
                ssPrint = null;
            }

            this.Close();
        }

        public void Print_Chul()
        {
 
            clsPrint CP = new clsPrint();
            PrintDocument pd;

            if (CP.isPmpaBarCodePrinter("신용카드") == false)
            {
                ComFunc.MsgBox("지정된 프린터를 찾을수 없습니다.", "신용카드");
                return;
            }

            pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = CP.getPmpaBarCodePrinter("신용카드");
            pd.PrinterSettings.DefaultPageSettings.PaperSize = new PaperSize("POSSIZE", 50, 30);

            pd.PrintPage += new PrintPageEventHandler(Chul_Print);
            pd.Print();    //프린트
        }

        public  void Chul_Print(object sender, PrintPageEventArgs ev)
        {

            int nX = 0;
            int nY = 0;
            int nCY = 17;
            Image WrtnoBarCode = null;
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();

            WrtnoBarCode = b.Encode(BarcodeLib.TYPE.CODE128, nHJ.WRTNO.ToString(), Color.Black, Color.White, 250, 45);


            ev.Graphics.DrawString(VB.Space(15) + "접 수 증", new Font("맑은 고딕", 15f, FontStyle.Bold), Brushes.Black, nX + 5, nY + (nCY * 1), new StringFormat());
            ev.Graphics.DrawString(VB.Space(30) + nHJ.SNAME + "(" + nHJ.SEX + "/" + nHJ.AGE + ")", new Font("맑은 고딕", 10f, FontStyle.Bold), Brushes.Black, nX + 5, nY + (nCY * 3), new StringFormat());
            ev.Graphics.DrawImage(WrtnoBarCode, 5, 50, 150, 30);


            //ev.Graphics.DrawString(" 접수증", new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 4), new StringFormat());
            ev.Graphics.DrawString(VB.Space(5) +nHJ.WRTNO + VB.Space(15) + VB.Left(nHJ.JUMINNO, 6) + "-" + VB.Mid(nHJ.JUMINNO, 7, 1) + "******" , new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 5), new StringFormat());

            //구분선
            //ev.Graphics.DrawString("--------------------------------------------------", new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 6), new StringFormat());
            ev.Graphics.DrawString("――――――――――――――――――――――", new Font("맑은 고딕", 10f, FontStyle.Bold), Brushes.Black, nX + 5, nY + (nCY * 6), new StringFormat());
            //가셔야할곳
            ev.Graphics.DrawString(" 가셔야할곳☞  " + VB.Trim(VB.Pstr(strRetValue1, "^^", 1)), new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 7), new StringFormat());
            ev.Graphics.DrawString( FstrExamList, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 8), new StringFormat());
            ev.Graphics.DrawString( FstrExamList1, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 9), new StringFormat());
            ev.Graphics.DrawString( FstrExamList2, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 10), new StringFormat());

            //구분선
            ev.Graphics.DrawString("――――――――――――――――――――――", new Font("맑은 고딕", 10f, FontStyle.Bold), Brushes.Black, nX + 5, nY + (nCY * 11), new StringFormat());
            ev.Graphics.DrawString(" 검사 완료후 접수창구에 본 증을 ", new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 12), new StringFormat());
            ev.Graphics.DrawString(" 제출하셔야 처리가 완료됩니다.", new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 13), new StringFormat());
            ev.Graphics.DrawString( FstrPrtGubun, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 14), new StringFormat());
            //구분선
            ev.Graphics.DrawString("――――――――――――――――――――――", new Font("맑은 고딕", 10f, FontStyle.Bold), Brushes.Black, nX + 5, nY + (nCY * 15), new StringFormat());


            //UANAME
            if (FstrUAName =="")
            {
                ev.Graphics.DrawString(FstrExamName, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 16), new StringFormat());
            }
            else
            {
                ev.Graphics.DrawString(FstrUAName, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 16), new StringFormat());
            }
            
            ev.Graphics.DrawString("", new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 17), new StringFormat());
            ev.Graphics.DrawString("", new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 18), new StringFormat());
            //구분선
            ev.Graphics.DrawString("――――――――――――――――――――――", new Font("맑은 고딕", 10f, FontStyle.Bold), Brushes.Black, nX + 5, nY + (nCY * 19), new StringFormat());

            //흉부촬영번호 표시
            ev.Graphics.DrawString(" 접수번호:" + nHJ.WRTNO + "  " + nHJ.XRAYNO, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 20), new StringFormat());

            if (FstrJobName == "")
            {
                ev.Graphics.DrawString(" 등록번호: " + nHJ.PTNO + "  " + FstrSecond, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 21), new StringFormat());
            }
            else
            {
                ev.Graphics.DrawString(" 등록번호: " + nHJ.PTNO + "  " + FstrSecond + "접수자: " + FstrJobName, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 21), new StringFormat());
            }
            
            if (nHJ.LTDCODE.ToString() != "")
            {
                ev.Graphics.DrawString(" "+cf.Read_Ltd_Name(clsDB.DbCon, nHJ.LTDCODE.ToString()) , new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 22), new StringFormat());
            }

            //학생검진비만표시 제외
            if (nHJ.GJJONG == "11" && !FstrUCodes.IsNullOrEmpty())
            {
                ev.Graphics.DrawString(" 11. 일반+특수" + " " + nHJ.JEPDATE, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 24), new StringFormat());
            }
            else
            {
                ev.Graphics.DrawString(" "+nHJ.GJJONG + ". " + hb.READ_GjJong_Name(nHJ.GJJONG) + "  " + nHJ.JEPDATE, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 25), new StringFormat());
            }

            ev.Graphics.DrawString(" "+FstrJuso1, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 26), new StringFormat());
            ev.Graphics.DrawString(" "+FstrJuso2, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 27), new StringFormat());
            ev.Graphics.DrawString(" "+FstrJuso3, new Font("맑은 고딕", 10f), Brushes.Black, nX + 5, nY + (nCY * 28), new StringFormat());
        }
    }
}
