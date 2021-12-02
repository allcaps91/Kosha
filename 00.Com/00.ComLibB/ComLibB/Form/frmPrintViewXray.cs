using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 방사선 촬영 결과지(일반)
/// Author : 김형범
/// Create Date : 2017.06.30
/// </summary>
/// <history>
/// TODO: XuAgfa.bas => Report_Print_NEW 함수 생성 필요
/// </history>
namespace ComLibB
{
    /// <summary> 방사선 촬영 결과지(일반) </summary>
    public partial class frmPrintViewXray : Form
    {
        string FstrRowid = ""; //방사선결과 xray_result_new의 rowid
        string GstrHelpCode = "";

        /// <summary> 방사선 촬영 결과지(일반) </summary>
        public frmPrintViewXray()
        {
            InitializeComponent();
        }

        public frmPrintViewXray(string strHelpCode)
        {
            InitializeComponent();

            GstrHelpCode = strHelpCode;
        }

        void frmPrintViewXray_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            rtxtResult.Text = "";
            FstrRowid = GstrHelpCode;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            FstrRowid = "AAABxkAAYAAAWkBAAH";
            rtxtResult.Text = "";

            ReportViewRichText(rtxtResult, FstrRowid);
        }

        void ReportViewRichText(RichTextBox ArgControl, string strRowid)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string strBI = "";
            string strBiName = "";
            string strJuMin = "";
            string strDeptName = "";
            string strDrname = "";
            string strResultDrName = "";
            string strXCode = "";
            string strXName = "";
            string strResult = "";
            string strXJong = "";
            string strChar = "";
            string strPRT = "";
            int intReportLine = 0;
            int intLine2Char = 0; //1줄당 인쇄할 글자수
            int intPage2Line = 0; //1Page에 인쇄할 줄의 갯수
            int intReportTotalPage = 0;
            string strIpdOpd = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ArgControl.Text = "";

            // 인쇄여부 설정이 Off이면 인쇄않함
            //if (clsPublic.GnPrtOnOff == 0)
            //{
            //    return;
            //}

            intLine2Char = 80; //1줄당 인쇄할 글자(Byte)수
            intPage2Line = 34; //1Page당 인쇄할 줄의 갯수

            try
            {
                //해당 판독번호로 판독결과를 READ
                SQL = "";
                SQL = "SELECT Pano,SName,TO_CHAR(ReadDate,'YYYY-MM-DD') ReadDate,TO_CHAR(SeekDate,'YYYY-MM-DD') SeekDate,";
                SQL = SQL + ComNum.VBLF + " XJong,SName,Sex,Age,IpdOpd,DeptCode,DrCode,WardCode,RoomCode,XDrCode1,XDrCode2,";
                SQL = SQL + ComNum.VBLF + " XDrCode3,IllCode1,IllCode2,IllCode3,XCode,XName,Result,Result1,PrtCNT,ViewCNT,WRTNO ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "XRAY_RESULTNEW ";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + FstrRowid + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                clsXuAgfa.TXD.WRTNO = Convert.ToInt32(dt.Rows[0]["WRTNO"].ToString().Trim());
                clsXuAgfa.TXD.Pano = dt.Rows[0]["Pano"].ToString().Trim();
                clsXuAgfa.TXD.sName = dt.Rows[0]["SName"].ToString().Trim();
                clsXuAgfa.TXD.Age = Convert.ToInt32(dt.Rows[0]["Age"].ToString().Trim());
                clsXuAgfa.TXD.Sex = dt.Rows[0]["Sex"].ToString().Trim();
                clsXuAgfa.TXD.DeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                clsXuAgfa.TXD.DrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                clsXuAgfa.TXD.IpdOpd = dt.Rows[0]["IpdOpd"].ToString().Trim();
                clsXuAgfa.TXD.WardCode = dt.Rows[0]["WardCode"].ToString().Trim();
                clsXuAgfa.TXD.RoomCode = dt.Rows[0]["RoomCode"].ToString().Trim();
                clsXuAgfa.TXD.SeekDate = dt.Rows[0]["SeekDate"].ToString().Trim();
                clsXuAgfa.TXD.ReadDate = dt.Rows[0]["ReadDate"].ToString().Trim();
                clsXuAgfa.TXD.XJong = dt.Rows[0]["XJong"].ToString().Trim();
                clsXuAgfa.TXD.XCode = dt.Rows[0]["XCode"].ToString().Trim();
                clsXuAgfa.TXD.Xname = dt.Rows[0]["XName"].ToString().Trim();
                clsXuAgfa.TXD.XDrSabun = Convert.ToInt32(dt.Rows[0]["XDrCode1"].ToString().Trim());
                strXCode = clsXuAgfa.TXD.XCode.Trim();
                strXName = clsXuAgfa.TXD.Xname.Trim();
                strResult = dt.Rows[0]["Result"].ToString().Trim() + dt.Rows[0]["Result1"].ToString().Trim();

                dt.Dispose();
                dt = null;

                //자료를 SELECT
                SQL = "";
                SQL = "SELECT Exid, TO_CHAR(ENTERDATE, 'YYYY-MM-DD') ENTERDATE ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "XRAY_DETAIL ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + clsXuAgfa.TXD.Pano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ExInfo = " + clsXuAgfa.TXD.WRTNO + " ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                clsXuAgfa.TXD.Gisa = "";
                clsXuAgfa.TXD.EnterDate = "";

                if (dt.Rows.Count > 0)
                {
                    clsXuAgfa.TXD.Gisa = clsXuAgfa.READ_XRAY_GISA(clsDB.DbCon, Convert.ToInt32(dt.Rows[0]["Exid"].ToString().Trim())).Trim();
                    clsXuAgfa.TXD.EnterDate = dt.Rows[0]["EnterDate"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                // 환자마스타의 자료를 READ
                SQL = "";
                SQL = "SELECT Bi,Jumin1,Jumin2 ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_Patient ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + clsXuAgfa.TXD.Pano + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                strBI = " ";
                strJuMin = " ";

                if (dt.Rows.Count == 1)
                {
                    strBI = dt.Rows[0]["Bi"].ToString().Trim();
                    strJuMin = dt.Rows[0]["Jumin1"].ToString().Trim() + "-" + dt.Rows[0]["Jumin2"].ToString().Trim();
                }

                strBiName = clsVbfunc.GetBiName(strBI);
                strResultDrName = clsVbfunc.GetPassName(clsDB.DbCon, VB.Format(clsXuAgfa.TXD.XDrSabun, "####0"));
                strResult = VB.RTrim(strResult);
                intReportLine = clsXuAgfa.String_PrintLine_COUNT(strResult, intLine2Char);
                intReportTotalPage = (intReportLine / intPage2Line);

                if (intReportLine > (intReportTotalPage * intPage2Line))
                {
                    intReportTotalPage = intReportTotalPage + 1;
                }

                //처방종류(촬영종류)
                switch (clsXuAgfa.TXD.XJong)
                {
                    case "1":
                        strXJong = "일반촬영";
                        break;
                    case "2":
                        strXJong = "특수촬영";
                        break;
                    case "3":
                        strXJong = "Sono촬영";
                        break;
                    case "4":
                        strXJong = "CT촬영";
                        break;
                    case "5":
                        strXJong = "MRI촬영";
                        break;
                    case "6":
                        strXJong = "RI촬영";
                        break;
                    case "7":
                        strXJong = "BMD촬영";
                        break;
                    default:
                        strXJong = "";
                        break;
                }

                strDeptName = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, clsXuAgfa.TXD.DeptCode);
                strDrname = clsXuAgfa.READ_DrName(clsDB.DbCon, clsXuAgfa.TXD.DrCode);
                strIpdOpd = "외래";

                if (clsXuAgfa.TXD.IpdOpd == "I")
                {
                    strIpdOpd = "입원";
                }

                Report_Head_NEW_Print(ArgControl, strDeptName, strDrname); //인적사항 인쇄

                //내용을 인쇄함
                for (i = 0; i < VB.Len(strResult); i++)
                {
                    strChar = VB.Mid(strResult, i, 1);

                    if (strChar == ComNum.VBLF || strChar == ComNum.VBLF)
                    {
                        if (strChar == ComNum.VBLF)
                        {
                            Report_Print_NEW_OneLine(ArgControl, strDeptName, strDrname, strResultDrName);
                        }
                    }
                    else if (VB.Len(strPRT) >= intLine2Char)
                    {
                        Report_Print_NEW_OneLine(ArgControl, strDeptName, strDrname, strResultDrName);
                        strPRT = strChar;
                    }
                    else
                    {
                        strPRT = strPRT + strChar;
                    }
                }

                if (strPRT != "")
                {
                    Report_Print_NEW_OneLine(ArgControl, strDeptName, strDrname, strResultDrName);
                }

                Report_Tail_NEW_Print(ArgControl, strDeptName, strDrname, strResultDrName);   //판독일자,판독의사등
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void Report_Head_NEW_Print(RichTextBox ArgControl, string strDeptName, string strDrname)
        {
            int j = 0;
            int intPrtPage = 0;
            int intReportTotalPage = 0;

            //글자체를 지정함
            //ArgControl.Font = "굴림체";
            //ArgControl.SelFontName = "굴림체";
            //ArgControl.SelFontSize = 20;

            for (j = 0; j < 1; j++)
            {
                ArgControl.Text = ArgControl.Text + ComNum.VBLF;
            }

            switch (clsXuAgfa.TXD.XJong)
            {
                case "6":
                    ArgControl.Text = ArgControl.Text + VB.Space(21) + "R I Study Report" + ComNum.VBLF;
                    break;
                case "E":
                    ArgControl.Text = ArgControl.Text + VB.Space(21) + "전기진단검사 결과지" + ComNum.VBLF;
                    break;
                default:
                    ArgControl.Text = ArgControl.Text + VB.Space(21) + "방사선 촬영 결과지" + ComNum.VBLF;
                    break;
            }

            //글자체를 지정함
            //ArgControl.Font = "굴림체"

            //Page를 인쇄
            intPrtPage = intPrtPage + 1;

            if (intReportTotalPage > 1)
            {
                ArgControl.Text = ArgControl.Text + VB.Space(74) + "▶Page: (" + intReportTotalPage + "-" + intPrtPage + ")" + ComNum.VBLF;
            }
            else
            {
                ArgControl.Text = ArgControl.Text + ComNum.VBLF;
            }

            ArgControl.Text = ArgControl.Text;

            ArgControl.Text = ArgControl.Text + VB.Space(9) + VB.String(85, "=") + ComNum.VBLF;
            ArgControl.Text = ArgControl.Text + ComNum.VBLF;

            //등록번호,종류,병동.호실,처방종류
            ArgControl.Text = ArgControl.Text + VB.Space(9) + "등록번호: " + VB.Left(clsXuAgfa.TXD.Pano + VB.Space(20), 20);
            ArgControl.Text = ArgControl.Text + "성 명: " + VB.Left(clsXuAgfa.TXD.sName + VB.Space(15), 15);
            ArgControl.Text = ArgControl.Text + "성      별: " + VB.Format(clsXuAgfa.TXD.Age, "###0") + "/" + clsXuAgfa.TXD.Sex + VB.Space(5);

            if (clsXuAgfa.TXD.IpdOpd == "I")
            {
                ArgControl.Text = ArgControl.Text + clsXuAgfa.TXD.WardCode.Trim() + "/" + clsXuAgfa.TXD.RoomCode.Trim();
            }
            else
            {
                ArgControl.Text = ArgControl.Text + "외래";
            }

            ArgControl.Text = ArgControl.Text + ComNum.VBLF;
            ArgControl.Text = ArgControl.Text + VB.Space(9) + "의 뢰 과: " + VB.Left(strDeptName + VB.Space(20), 20);
            ArgControl.Text = ArgControl.Text + "의 사: " + VB.Left(strDrname + VB.Space(15), 15);
            ArgControl.Text = ArgControl.Text + "검사요청일: " + clsXuAgfa.TXD.EnterDate;
            ArgControl.Text = ArgControl.Text + ComNum.VBLF;
            ArgControl.Text = ArgControl.Text + VB.Space(9) + VB.String(85, "=") + ComNum.VBLF;

            for (j = 0; j < 1; j++)
            {
                ArgControl.Text = ArgControl.Text + ComNum.VBLF;
            }

            //검사명칭
            ArgControl.Text = ArgControl.Text + VB.Space(9) + "검사명: " + clsXuAgfa.TXD.Xname + ComNum.VBLF;

            for (j = 0; j < 1; j++)
            {
                ArgControl.Text = ArgControl.Text + ComNum.VBLF;
            }
        }

        void Report_Print_NEW_OneLine(RichTextBox ArgControl, string strDeptName, string strDrname, string strResultDrName)
        {
            int intPrtLine = 0;
            int intPage2Line = 0;
            string strPRT = "";

            intPrtLine = intPrtLine + 1;

            if (intPrtLine > intPage2Line)
            {
                ArgControl.Text = ArgControl.Text + VB.Space(68) + "☞다음 Page에 계속" + ComNum.VBLF;
                Report_Tail_NEW_Print(ArgControl, strDeptName, strDrname, strResultDrName);  //마지막부분 인쇄
                Report_Head_NEW_Print(ArgControl, strDeptName, strDrname);  //다음장 Head를 인쇄
                intPrtLine = 1;
            }

            ArgControl.Text = ArgControl.Text + VB.Space(9) + strPRT + ComNum.VBLF;
            strPRT = "";
        }

        void Report_Tail_NEW_Print(RichTextBox ArgControl, string strDeptName, string strDrname, string strResultDrName)
        {
            //VB

            //If nPrtLine < nPage2Line - 3 Then;
            //For j = 0 To 20;
            //ArgControl.Text = ArgControl.Text & vbCrLf;
            //nPrtLine = nPrtLine + 1;
            //If nPrtLine > nPage2Line - 3 Then Exit For;
            //Next j;
            //End If;

            ArgControl.Text = ArgControl.Text + ComNum.VBLF;

            //외부판독은 판독의사 인쇄 안함
            if (clsXuAgfa.TXD.XDrSabun != 99001)
            {
                ArgControl.Text = ArgControl.Text + VB.Space(13) + "촬영일자 : " + clsXuAgfa.TXD.SeekDate + VB.Space(9) + "판독일자 : " + VB.Left(clsXuAgfa.TXD.ReadDate + VB.Space(10), 10); //판독일자
            }
            else
            {
                ArgControl.Text = ArgControl.Text + VB.Space(13) + VB.Space(50); //판독일자
            }

            switch (clsXuAgfa.TXD.XJong)
            {
                case "6":
                    ArgControl.Text = ArgControl.Text + "판독의사 : " + "Chanwoo Lee. M.D." + ComNum.VBLF;
                    ArgControl.Text = ArgControl.Text + VB.Space(9) + VB.String(85, "-") + ComNum.VBLF;
                    ArgControl.Text = ArgControl.Text + VB.Space(13) + "Pohang St. Mary's Hospital. Department of Nuclear Medicine.  Tel:054-289-4520";
                    break;
                case "E":
                    ArgControl.Text = ArgControl.Text + "판독의사 : " + "김 종 민" + ComNum.VBLF;
                    ArgControl.Text = ArgControl.Text + VB.Space(9) + VB.String(85, "-") + ComNum.VBLF;
                    ArgControl.Text = ArgControl.Text + VB.Space(9) + "Pohang St.Mary's Hospital. Physical medicine & Rehabilitation     Tel:054-289-4591";
                    break;
                default:

                    if (clsXuAgfa.TXD.XDrSabun != 99001) //외부판독은 판독의사 인쇄 안함
                    {
                        ArgControl.Text = ArgControl.Text + VB.Space(12) + "판독의사 : " + strResultDrName + ComNum.VBLF;
                    }
                    else
                    {
                        ArgControl.Text = ArgControl.Text + ComNum.VBLF;
                    }

                    ArgControl.Text = ArgControl.Text + VB.Space(9) + VB.String(85, "-") + ComNum.VBLF;
                    ArgControl.Text = ArgControl.Text + VB.Space(13) + "※포항성모병원 방사선과※    전화:054-289-4628 FAX:054-277-2072 ARS:054-289-4600";
                    break;
            }
        }

        //TODO: XuAgfa.bas => Report_Print_NEW 함수 생성 필요
        void btnPrint_Click(object sender, EventArgs e)
        {
            //Report_Print_NEW(FstrRowid);
        }
    }
}
