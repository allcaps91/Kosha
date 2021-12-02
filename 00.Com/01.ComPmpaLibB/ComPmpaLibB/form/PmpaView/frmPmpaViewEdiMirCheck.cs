using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewEdiMirCheck.cs
    /// Description     : EDI접수증과 미수발생액 점검
    /// Author          : 박창욱
    /// Create Date     : 2017-10-24
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs36.frm(FrmEdiMirCheck.frm) >> frmPmpaViewEdiMirCheck.cs 폼이름 재정의" />	
    public partial class frmPmpaViewEdiMirCheck : Form
    {
        double[,] FnAmt = new double[14, 10];
        string GstrYYMM = "";
        string GstrMenu = "";
        string GstrSMenu = "";


        public frmPmpaViewEdiMirCheck()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "EDI접수증과 미수발생액 점검";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("진료월: " + cboYYMM.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 120, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nRead = 0;
            int nBiNo = 0;
            double nAmt = 0;
            string strYYMM = "";
            string strFDate = "";
            string strTdate = "";
            string strIpdOpd = "";
            string strBi = "";

            string strJDate1 = "";
            string strJDate2 = "";

            clsPmpaMisu cpm = new clsPmpaMisu();

            //누적할 배열을 Clear
            for (i = 0; i < 14; i++)
            {
                for (k = 0; k < 10; k++)
                {
                    FnAmt[i, k] = 0;
                }
            }

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTdate = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strYYMM, 4)), Convert.ToInt32(VB.Right(strYYMM, 2)));

            GstrYYMM = strYYMM;
            GstrMenu = "3";
            GstrSMenu = "7";

            try
            {
                //퇴원자 조합부담금
                #region Slip_ADD

                //외래,입원 당월퇴원 조합부담 발생액을 TONG_SLIP에서 READ
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT BiGbn, IpdOpd, SUM(Amt33) Amt";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALDAILY ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "GROUP BY BiGbn,IpdOpd ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    nBiNo = (int)VB.Val(dt.Rows[i]["BiGbn"].ToString().Trim());
                    strIpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    if (nAmt != 0)
                    {
                        switch (nBiNo)
                        {
                            case 1:
                                k = 1;
                                break;
                            case 2:
                                k = 2;
                                break;
                            case 3:
                                k = 4;
                                break;
                            case 4:
                                k = 5;
                                break;
                            default:
                                k = 1;
                                break;
                        }
                        if (strIpdOpd == "I")
                        {
                            k += 6;
                        }
                        FnAmt[k, 1] += nAmt;
                    }
                }
                dt.Dispose();
                dt = null;
                #endregion

                //중간청구 조합부담금
                #region JungganMir_ADD

                strJDate1 = Convert.ToDateTime(strTdate).AddDays(1).ToString("yyyy-MM-dd");
                strJDate2 = clsVbfunc.LastDay(Convert.ToInt32(VB.Left(strJDate1, 4)), Convert.ToInt32(VB.Mid(strJDate1, 6, 2)));

                //입원 당월 중간청구 Build 조합부담 발생액을 READ
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Bi,DeptCode,SUM(BuildJAmt) Amt ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MIR_IPDID ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND BuildDate>=TO_DATE('" + strJDate1 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND BuildDate<=TO_DATE('" + strJDate2 + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND FLAG = '1' "; //청구자
                SQL = SQL + ComNum.VBLF + "GROUP BY Bi,DeptCode ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    nBiNo = cpm.READ_Bi_SuipTong(dt.Rows[i]["Bi"].ToString().Trim(), strFDate);
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    if (nAmt != 0)
                    {
                        switch (nBiNo)
                        {
                            case 1:
                                k = 1;
                                break;
                            case 2:
                                k = 2;
                                break;
                            case 3:
                                k = 4;
                                break;
                            case 4:
                                k = 5;
                                break;
                            default:
                                k = 1;
                                break;
                        }
                        if (k == 2 && dt.Rows[i]["DeptCode"].ToString().Trim() == "NP")
                        {
                            k = 3;
                        }
                        k += 6;
                        FnAmt[k, 6] += nAmt;    //중간청구
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion

                //응급실6시간이상, NP낮병동
                #region Em6TimveOver_ADD

                //응급실6시간이상,NP낮병동 조합부담액 ADD
                SQL = "";
                SQL = SQL + "SELECT SuBi, DeptCode, SUM(Johap) Amt ";
                SQL = SQL + " FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
                SQL = SQL + "WHERE 1 = 1";
                SQL = SQL + "  AND ActDate>=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL = SQL + "  AND ActDate<=TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + "  AND Gubun='3' "; //응급6시간초과
                SQL = SQL + "GROUP BY SuBi,DeptCode ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    nBiNo = (int)VB.Val(dt.Rows[i]["SuBi"].ToString().Trim());
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    if (nAmt != 0)
                    {
                        switch (nBiNo)
                        {
                            case 1:
                                k = 1;
                                break;
                            case 2:
                                k = 2;
                                break;
                            case 3:
                                k = 4;
                                break;
                            case 4:
                                k = 5;
                                break;
                            default:
                                k = 1;
                                break;
                        }
                        if (k == 2 && dt.Rows[i]["DeptCode"].ToString().Trim() == "NP")
                        {
                            k = 3;
                        }

                        if (nAmt != 0)
                        {
                            FnAmt[k, 1] -= nAmt;    //외래에 (-)
                            k += 6;
                            FnAmt[k, 6] += nAmt;    //입원에 (+)
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion

                //당월분 실청구액
                #region SilMir_ADD

                //미수관리에서 청구미수액을 ADD
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Class,IpdOpd,TongGbn,Bun,SUM(Amt2) Amt ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND MirYYMM = '" + strYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Class <= '07' ";
                SQL = SQL + ComNum.VBLF + "   AND TongGbn IN ('1','2') ";
                SQL = SQL + ComNum.VBLF + " GROUP BY Class,IpdOpd,TongGbn,Bun ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    strBi = dt.Rows[i]["Class"].ToString().Trim();
                    strIpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    if (nAmt != 0)
                    {
                        switch (strBi)
                        {
                            case "01":
                            case "02":
                            case "03":
                                k = 1;  //보험
                                break;
                            case "04":
                                k = 2;  //보호
                                break;
                            case "05":
                                k = 4;  //산재
                                break;
                            case "07":
                                k = 5;  //자보
                                break;
                            default:
                                k = 1;  //기타는 보험으로
                                break;
                        }
                        //의료보호 정신과 서면청구
                        if (strBi == "04")
                        {
                            if (dt.Rows[i]["Bun"].ToString().Trim() == "07" || dt.Rows[i]["Bun"].ToString().Trim() == "14")
                            {
                                k = 3;
                            }
                        }
                        if (strIpdOpd == "I")
                        {
                            k += 6;
                        }

                        if (dt.Rows[i]["TongGbn"].ToString().Trim() == "1")
                        {
                            FnAmt[k, 3] += nAmt;
                        }
                        else
                        {
                            FnAmt[k, 8] += nAmt;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion

                //당월분 월말, 중간청구 EDI접수액
                #region EdiJepsu_ADD

                //보험 당월 진료분 EDI접수액 합산(재청구,추가청구는 제외)
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Johap, IpdOpd ,Week,"; //SUM(SamJAmt+SamJangAmt+SamDaebul+SAMUAMT100) Amt "
                SQL = SQL + ComNum.VBLF + "       MIRGBN, SUM(SamJAmt+SamJangAmt+SamDaebul+SAMGAMT+ SAMGAMT_TUBER +SAMUAMT100) AMT";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "EDI_JEPSU ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strYYMM + "' "; //진료월
                SQL = SQL + ComNum.VBLF + "   AND BanDate IS NULL ";          //반송분은 제외
                SQL = SQL + ComNum.VBLF + "   AND MirGbn IN ('0','4') ";      //추가청구,재청구는 제외
                SQL = SQL + ComNum.VBLF + " GROUP BY Johap,IpdOpd,Week,MIRGBN ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    strBi = dt.Rows[i]["Johap"].ToString().Trim();
                    strIpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    if (nAmt != 0)
                    {
                        switch (strBi)
                        {
                            case "1":
                            case "2":
                                k = 1;  //보험
                                break;
                            case "5":
                                if (dt.Rows[i]["MIRGBN"].ToString().Trim() == "0")
                                {
                                    k = 2;  //보호
                                }
                                else
                                {
                                    k = 3;  //NP정액
                                }
                                break;
                            default:
                                k = 1;  //기타는 보험으로
                                break;
                        }
                        if (strIpdOpd == "I")
                        {
                            k += 6;
                        }
                        if (dt.Rows[i]["Week"].ToString().Trim() == "7")    //중간청구
                        {
                            FnAmt[k, 7] += nAmt;
                        }
                        else
                        {
                            FnAmt[k, 2] += nAmt;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                //산재 당월 진료분 EDI 접수액 합산(재청구, 추가청구 제외)
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT IpdOpd,Week,SUM(JepAmt) Amt ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "EDI_SANJEPSU ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND YYMM = '" + strYYMM + "' "; //진료월
                SQL = SQL + ComNum.VBLF + "   AND BanDate IS NULL ";          //반송분은 제외
                SQL = SQL + ComNum.VBLF + "   AND MirGbn = '0' ";             //추가청구,재청구는 제외
                SQL = SQL + ComNum.VBLF + " GROUP BY IpdOpd,Week ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                for (i = 0; i < nRead; i++)
                {
                    nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                    strIpdOpd = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    if (nAmt != 0)
                    {
                        k = 4;
                        if (strIpdOpd == "I")
                        {
                            k += 6;
                        }
                        if (dt.Rows[i]["Week"].ToString().Trim() == "7")
                        {
                            FnAmt[k, 7] += nAmt;
                        }
                        else
                        {
                            FnAmt[k, 2] += nAmt;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion


                //외래, 입원소계 및 합계를 구함
                for (i = 1; i < 10; i++)
                {
                    //외래소계
                    FnAmt[6, i] = FnAmt[1, i] + FnAmt[2, i] + FnAmt[3, i] + FnAmt[4, i] + FnAmt[5, i];

                    //입원소계
                    FnAmt[12, i] = FnAmt[7, i] + FnAmt[8, i] + FnAmt[9, i] + FnAmt[10, i] + FnAmt[11, i];

                    //합계
                    FnAmt[13, i] = FnAmt[6, i] + FnAmt[12, i];
                }

                //내용을 Sheet에 Display
                for (i = 1; i < 14; i++)
                {
                    //월말차이
                    FnAmt[i, 4] = FnAmt[i, 3] - FnAmt[i, 2];

                    //중간차이
                    FnAmt[i, 9] = FnAmt[i, 8] - FnAmt[i, 7];

                    for (k = 1; k < 10; k++)
                    {
                        ssView_Sheet1.Cells[i - 1, k + 1].Text = FnAmt[i, k].ToString("###,###,###,##0");
                    }
                }


                btnTong.Enabled = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboYYMM_SelectedIndexChanged(object sender, EventArgs e)
        {
            ssView_Sheet1.Cells[0, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            btnTong.Enabled = false;
        }

        private void btnTong_Click(object sender, EventArgs e)
        {
            frmPmpaViewBalRemark frm = new frmPmpaViewBalRemark(GstrYYMM, GstrMenu, GstrSMenu);
            frm.Show();
        }

        private void frmPmpaViewEdiMirCheck_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "1");
            
            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

        }
    }
}
