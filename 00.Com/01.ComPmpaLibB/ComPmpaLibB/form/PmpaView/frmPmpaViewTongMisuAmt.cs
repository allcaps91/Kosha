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
    /// File Name       : frmPmpaViewTongMisuAmt.cs
    /// Description     : 월별미수금총괄표
    /// Author          : 박창욱
    /// Create Date     : 2017-10-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs61.frm(FrmTongMisuAmt.frm) >> frmPmpaViewTongMisuAmt.cs 폼이름 재정의" />	
    public partial class frmPmpaViewTongMisuAmt : Form
    {
        double[,] FnAmt = new double[23, 9];
        ComFunc cf = new ComFunc();
        string GstrYYMM = "";
        string GstrMenu = "";
        string GstrSMenu = "";


        public frmPmpaViewTongMisuAmt()
        {
            InitializeComponent();
        }

        private void frmPmpaViewTongMisuAmt_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            btnSTS.Enabled = false;
            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 36, "", "1");
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

        void SS_SET()
        {
            //건강보험 + 의료급여
            if (chkOpt0.Checked == true)
            {
                ssView_Sheet1.Rows[5].Visible = true;
                ssView_Sheet1.Rows[6].Visible = true;
                ssView_Sheet1.Rows[0].Visible = true;
                ssView_Sheet1.Rows[1].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[5].Visible = false;
                ssView_Sheet1.Rows[6].Visible = false;
                ssView_Sheet1.Rows[0].Visible = false;
                ssView_Sheet1.Rows[1].Visible = false;
            }

            //산재 + 자보
            if (chkOpt1.Checked == true)
            {
                ssView_Sheet1.Rows[2].Visible = true;
                ssView_Sheet1.Rows[3].Visible = true;
                ssView_Sheet1.Rows[7].Visible = true;
                ssView_Sheet1.Rows[8].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[2].Visible = false;
                ssView_Sheet1.Rows[3].Visible = false;
                ssView_Sheet1.Rows[7].Visible = false;
                ssView_Sheet1.Rows[8].Visible = false;
            }

            //CLEAR
            ssView_Sheet1.Cells[0, 0, 9, 0].Text = "";

            if (chkOpt0.Checked == true && chkOpt1.Checked == true)
            {
                ssView_Sheet1.Cells[1, 0].Text = "외";
                ssView_Sheet1.Cells[3, 0].Text = "래";
                ssView_Sheet1.Cells[6, 0].Text = "입";
                ssView_Sheet1.Cells[8, 0].Text = "원";
            }
            else if (chkOpt1.Checked == true)
            {
                ssView_Sheet1.Cells[2, 0].Text = "외";
                ssView_Sheet1.Cells[3, 0].Text = "래";
                ssView_Sheet1.Cells[7, 0].Text = "입";
                ssView_Sheet1.Cells[8, 0].Text = "원";
            }
            else if (chkOpt0.Checked == true)
            {
                ssView_Sheet1.Cells[0, 0].Text = "외";
                ssView_Sheet1.Cells[1, 0].Text = "래";
                ssView_Sheet1.Cells[5, 0].Text = "입";
                ssView_Sheet1.Cells[6, 0].Text = "원";
            }

            //기타미수
            if (chkOpt2.Checked == true)
            {
                ssView_Sheet1.Rows[11].Visible = true;
                ssView_Sheet1.Rows[12].Visible = true;
                ssView_Sheet1.Rows[13].Visible = true;
                ssView_Sheet1.Rows[14].Visible = true;
                ssView_Sheet1.Rows[15].Visible = true;
                ssView_Sheet1.Rows[16].Visible = true;
                ssView_Sheet1.Rows[17].Visible = true;
                ssView_Sheet1.Rows[18].Visible = true;
                ssView_Sheet1.Rows[19].Visible = true;
                ssView_Sheet1.Rows[20].Visible = true;
                ssView_Sheet1.Rows[21].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[11].Visible = false;
                ssView_Sheet1.Rows[12].Visible = false;
                ssView_Sheet1.Rows[13].Visible = false;
                ssView_Sheet1.Rows[14].Visible = false;
                ssView_Sheet1.Rows[15].Visible = false;
                ssView_Sheet1.Rows[16].Visible = false;
                ssView_Sheet1.Rows[17].Visible = false;
                ssView_Sheet1.Rows[18].Visible = false;
                ssView_Sheet1.Rows[19].Visible = false;
                ssView_Sheet1.Rows[20].Visible = false;
                ssView_Sheet1.Rows[21].Visible = false;
            }

            if (chkOpt0.Checked == false && chkOpt1.Checked == false)
            {
                ssView_Sheet1.Rows[4].Visible = false;
                ssView_Sheet1.Rows[9].Visible = false;
                ssView_Sheet1.Rows[10].Visible = false;
            }
            else
            {
                ssView_Sheet1.Rows[4].Visible = true;
                ssView_Sheet1.Rows[9].Visible = true;
                ssView_Sheet1.Rows[10].Visible = true;
            }

            ssView_Sheet1.Cells[0, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
        }

        private void chkOpt_CheckedChanged(object sender, EventArgs e)
        {
            SS_SET();
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

            int i = 0;
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;//2021-08-02 심경순 선생님 요청으로 미리보기 제외

            clsSpread CS = new clsSpread(); //
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                ssPrint_Sheet1.Cells[i + 6, 0].Text = ssView_Sheet1.Cells[i, 0].Text;
                ssPrint_Sheet1.Cells[i + 6, 1].Text = ssView_Sheet1.Cells[i, 1].Text;
                ssPrint_Sheet1.Cells[i + 6, 2].Text = ssView_Sheet1.Cells[i, 2].Text;
                ssPrint_Sheet1.Cells[i + 6, 3].Text = ssView_Sheet1.Cells[i, 3].Text;
                ssPrint_Sheet1.Cells[i + 6, 4].Text = ssView_Sheet1.Cells[i, 4].Text;
                ssPrint_Sheet1.Cells[i + 6, 5].Text = ssView_Sheet1.Cells[i, 5].Text;
                ssPrint_Sheet1.Cells[i + 6, 6].Text = ssView_Sheet1.Cells[i, 6].Text;
                ssPrint_Sheet1.Cells[i + 6, 7].Text = ssView_Sheet1.Cells[i, 7].Text;
                ssPrint_Sheet1.Cells[i + 6, 9].Text = ssView_Sheet1.Cells[i, 8].Text;
                ssPrint_Sheet1.Cells[i + 6, 11].Text = ssView_Sheet1.Cells[i, 9].Text;
            }

            //건강보험 + 의료급여
            if (chkOpt0.Checked == true)
            {
                ssPrint_Sheet1.Rows[11].Visible = true;
                ssPrint_Sheet1.Rows[12].Visible = true;
                ssPrint_Sheet1.Rows[6].Visible = true;
                ssPrint_Sheet1.Rows[7].Visible = true;
            }
            else
            {
                ssPrint_Sheet1.Rows[11].Visible = false;
                ssPrint_Sheet1.Rows[12].Visible = false;
                ssPrint_Sheet1.Rows[6].Visible = false;
                ssPrint_Sheet1.Rows[7].Visible = false;
            }

            //산재 + 자보
            if (chkOpt1.Checked == true)
            {
                ssPrint_Sheet1.Rows[8].Visible = true;
                ssPrint_Sheet1.Rows[9].Visible = true;
                ssPrint_Sheet1.Rows[13].Visible = true;
                ssPrint_Sheet1.Rows[14].Visible = true;
            }
            else
            {
                ssPrint_Sheet1.Rows[8].Visible = false;
                ssPrint_Sheet1.Rows[9].Visible = false;
                ssPrint_Sheet1.Rows[13].Visible = false;
                ssPrint_Sheet1.Rows[14].Visible = false;
            }

            if (chkOpt0.Checked == true && chkOpt1.Checked == true)
            {
                ssPrint_Sheet1.Cells[7, 0].Text = "외";
                ssPrint_Sheet1.Cells[9, 0].Text = "래";
                ssPrint_Sheet1.Cells[12, 0].Text = "입";
                ssPrint_Sheet1.Cells[14, 0].Text = "원";
            }
            else if (chkOpt1.Checked == true)
            {
                ssPrint_Sheet1.Cells[8, 0].Text = "외";
                ssPrint_Sheet1.Cells[9, 0].Text = "래";
                ssPrint_Sheet1.Cells[13, 0].Text = "입";
                ssPrint_Sheet1.Cells[14, 0].Text = "원";
            }
            else if (chkOpt0.Checked == true)
            {
                ssPrint_Sheet1.Cells[6, 0].Text = "외";
                ssPrint_Sheet1.Cells[7, 0].Text = "래";
                ssPrint_Sheet1.Cells[11, 0].Text = "입";
                ssPrint_Sheet1.Cells[12, 0].Text = "원";
            }

            //기타미수
            if (chkOpt2.Checked == true)
            {
                ssPrint_Sheet1.Rows[17].Visible = true;
                ssPrint_Sheet1.Rows[18].Visible = true;
                ssPrint_Sheet1.Rows[19].Visible = true;
                ssPrint_Sheet1.Rows[20].Visible = true;
                ssPrint_Sheet1.Rows[21].Visible = true;
                ssPrint_Sheet1.Rows[22].Visible = true;
                ssPrint_Sheet1.Rows[23].Visible = true;
                ssPrint_Sheet1.Rows[24].Visible = true;
                ssPrint_Sheet1.Rows[25].Visible = true;
                ssPrint_Sheet1.Rows[26].Visible = true;
                ssPrint_Sheet1.Rows[27].Visible = true;
            }
            else
            {
                ssPrint_Sheet1.Rows[17].Visible = false;
                ssPrint_Sheet1.Rows[18].Visible = false;
                ssPrint_Sheet1.Rows[19].Visible = false;
                ssPrint_Sheet1.Rows[20].Visible = false;
                ssPrint_Sheet1.Rows[21].Visible = false;
                ssPrint_Sheet1.Rows[22].Visible = false;
                ssPrint_Sheet1.Rows[23].Visible = false;
                ssPrint_Sheet1.Rows[24].Visible = false;
                ssPrint_Sheet1.Rows[25].Visible = false;
                ssPrint_Sheet1.Rows[26].Visible = false;
                ssPrint_Sheet1.Rows[27].Visible = false;
            }

            if (chkOpt0.Checked == false && chkOpt1.Checked == false)
            {
                ssPrint_Sheet1.Rows[10].Visible = false;
                ssPrint_Sheet1.Rows[15].Visible = false;
                ssPrint_Sheet1.Rows[16].Visible = false;
            }
            else
            {
                ssPrint_Sheet1.Rows[10].Visible = true;
                ssPrint_Sheet1.Rows[15].Visible = true;
                ssPrint_Sheet1.Rows[16].Visible = true;
            }

            strTitle = "월별 미수금 총괄표";

            ssPrint_Sheet1.Cells[2, 0].Text = "작 업 월: " + cboYYMM.Text;
            ssPrint_Sheet1.Cells[3, 0].Text = "출력일자: " + VB.Now().ToString();

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, false, false, false, false, false);

            CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
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

            int j = 0;
            int K = 0;
            string strYYMM = "";
            string strFDate = "";
            string strTdate = "";
            string strBiGbn = "";


            for (i = 0; i < 23; i++)
            {
                for (j = 0; j < 9; j++)
                {
                    FnAmt[i, j] = 0;
                }
            }

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTdate = cf.READ_LASTDAY(clsDB.DbCon, strFDate);

            if (string.Compare(strYYMM, "200701") >= 0)
            {
                ssView_Sheet1.Columns[6].Visible = true;

                //⑥당월절사삭감
                ssView_Sheet1.ColumnHeader.Cells[0, 7].Text = "⑥당월반송액";
                ssView_Sheet1.ColumnHeader.Cells[0, 8].Text = "⑦기타입금액";
                ssView_Sheet1.ColumnHeader.Cells[0, 9].Text = "⑧월말잔액";
            }
            else
            {
                ssView_Sheet1.Columns[6].Visible = false;

                //⑥당월절사삭감
                ssView_Sheet1.ColumnHeader.Cells[0, 7].Text = "⑥당월반송액";
                ssView_Sheet1.ColumnHeader.Cells[0, 8].Text = "⑦기타입금액";
                ssView_Sheet1.ColumnHeader.Cells[0, 9].Text = "⑧월말잔액";
            }

            //jjy(2003 - 01 - 13) '통계 remark 등록 공용변수
            GstrYYMM = strYYMM;
            GstrMenu = "4";
            GstrSMenu = "2";

            try
            {
                #region Misu_Monthly_ADD

                //월별 미수번호별 통계를 ADD
                //** 미수종류(Class) **
                //01.공단 02.직장 03.지역 04.보호 05.산재 07.자보
                strBiGbn = "''";
                if (chkOpt0.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'01','02','03','04'";
                }
                if (chkOpt1.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'05','07'";
                }
                if (chkOpt2.Checked == true)
                {
                    strBiGbn = strBiGbn + ",'11','08','13','09','16','17','18' ";
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Class, IpdOpd, SUM(IwolAmt) IwolAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(MisuAmt) MisuAmt, SUM(IpgumAmt) IpgumAmt, SUM(SakAmt) SakAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(BanAmt) BanAmt, SUM(EtcAmt) EtcAmt, SUM(JanAmt) JanAmt,";
                SQL = SQL + ComNum.VBLF + "       sum(SakAmt2) SakAmt2 ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_MONTHLY ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strYYMM + "' ";
                if (strBiGbn != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND CLASS IN (" + strBiGbn + " ) ";
                }
                SQL = SQL + ComNum.VBLF + "GROUP BY Class,IpdOpd ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Class,IpdOpd ";

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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i]["Class"].ToString().Trim())
                    {
                        case "01":
                        case "02":
                        case "03":
                            j = 1;
                            break;//의료보험
                        case "04":
                            j = 2;
                            break; //의료급여
                        case "05":
                            j = 3;
                            break; //산재
                        case "07":
                            j = 4;
                            break; //자보
                        case "11":
                            j = 13;
                            break; //보훈청
                        case "08":
                            j = 14;
                            break; //계약처
                        case "13":
                            j = 15;
                            break; //심신장애
                        case "09":
                            j = 16;
                            break; //헌혈미수
                        case "16":
                            j = 18;
                            break; //장기요양소견서
                        case "17":
                            j = 19;
                            break; //가정간호소견
                        case "18":
                            j = 20;
                            break; //치매검사
                        default:
                            j = 17;
                            break; //기타미수
                    }

                    if (j < 5)
                    {
                        if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                        {
                            j += 5;
                        }
                    }

                    FnAmt[j, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim()); //전월이월
                    FnAmt[j, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim()); //당월미수
                    FnAmt[j, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim()); //입금액
                    FnAmt[j, 4] += VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim()); //삭감액
                    FnAmt[j, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim()); //절사삭감
                    FnAmt[j, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim()); //반송액
                    FnAmt[j, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim()); //기타입금
                    FnAmt[j, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim()); //월말잔액

                    if (j <= 4)
                    {
                        K = 5;
                    }
                    else if (j <= 9)
                    {
                        K = 10;
                    }
                    else
                    {
                        K = 21;
                    }

                    //소계에 ADD
                    FnAmt[K, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim()); //전월이월
                    FnAmt[K, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim()); //당월미수
                    FnAmt[K, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim()); //입금액
                    FnAmt[K, 4] += VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim()); //삭감액
                    FnAmt[K, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim()); //절사삭감
                    FnAmt[K, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim()); //반송액
                    FnAmt[K, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim()); //기타입금
                    FnAmt[K, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim()); //월말잔액

                    //기관미수계
                    if (j <= 9)
                    {
                        K = 11;
                        FnAmt[K, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim()); //전월이월
                        FnAmt[K, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim()); //당월미수
                        FnAmt[K, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim()); //입금액
                        FnAmt[K, 4] += VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim()); //삭감액
                        FnAmt[K, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim()); //절사삭감
                        FnAmt[K, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim()); //반송액
                        FnAmt[K, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim()); //기타입금
                        FnAmt[K, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim()); //월말잔액
                    }

                    //미수합계에 ADD
                    K = 22;
                    FnAmt[K, 1] += VB.Val(dt.Rows[i]["IwolAmt"].ToString().Trim()); //전월이월
                    FnAmt[K, 2] += VB.Val(dt.Rows[i]["MisuAmt"].ToString().Trim()); //당월미수
                    FnAmt[K, 3] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim()); //입금액
                    FnAmt[K, 4] += VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim()); //삭감액
                    FnAmt[K, 5] += VB.Val(dt.Rows[i]["SakAmt2"].ToString().Trim()); //절사삭감
                    FnAmt[K, 6] += VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim()); //반송액
                    FnAmt[K, 7] += VB.Val(dt.Rows[i]["EtcAmt"].ToString().Trim()); //기타입금
                    FnAmt[K, 8] += VB.Val(dt.Rows[i]["JanAmt"].ToString().Trim()); //월말잔액
                }
                dt.Dispose();
                dt = null;

                #endregion

                #region GainMisu_Monthly_ADD

                //월별 개인미수 통계를 ADD
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SUM(IwolAmt) IwolAmt, SUM(MonMAmt) MisuAmt, SUM(MonIAmt) IpgumAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(MONSAMT) SakAmt, SUM(MONBAMT) BanAmt, SUM(MONGAMT) EtcAmt,";
                SQL = SQL + ComNum.VBLF + "       SUM(JanAmt) JanAmt";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_GAINTONG ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strYYMM + "' ";

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

                j = 12; //개인미수
                FnAmt[j, 1] += VB.Val(dt.Rows[0]["IwolAmt"].ToString().Trim()); //전월이월
                FnAmt[j, 2] += VB.Val(dt.Rows[0]["MisuAmt"].ToString().Trim()); //당월미수
                FnAmt[j, 3] += VB.Val(dt.Rows[0]["IpgumAmt"].ToString().Trim()); //입금액
                FnAmt[j, 4] += VB.Val(dt.Rows[0]["SakAmt"].ToString().Trim()); //삭감액
                FnAmt[j, 6] += VB.Val(dt.Rows[0]["BanAmt"].ToString().Trim()); //반송액
                FnAmt[j, 7] += VB.Val(dt.Rows[0]["EtcAmt"].ToString().Trim()); //기타입금
                FnAmt[j, 8] += VB.Val(dt.Rows[0]["JanAmt"].ToString().Trim()); //월말잔액


                //소계에 ADD
                K = 21;
                FnAmt[K, 1] += VB.Val(dt.Rows[0]["IwolAmt"].ToString().Trim()); //전월이월
                FnAmt[K, 2] += VB.Val(dt.Rows[0]["MisuAmt"].ToString().Trim()); //당월미수
                FnAmt[K, 3] += VB.Val(dt.Rows[0]["IpgumAmt"].ToString().Trim()); //입금액
                FnAmt[K, 4] += VB.Val(dt.Rows[0]["SakAmt"].ToString().Trim()); //삭감액
                FnAmt[K, 6] += VB.Val(dt.Rows[0]["BanAmt"].ToString().Trim()); //반송액
                FnAmt[K, 7] += VB.Val(dt.Rows[0]["EtcAmt"].ToString().Trim()); //기타입금
                FnAmt[K, 8] += VB.Val(dt.Rows[0]["JanAmt"].ToString().Trim()); //월말잔액


                //미수합계에 ADD
                K = 22;
                FnAmt[K, 1] += VB.Val(dt.Rows[0]["IwolAmt"].ToString().Trim()); //전월이월
                FnAmt[K, 2] += VB.Val(dt.Rows[0]["MisuAmt"].ToString().Trim()); //당월미수
                FnAmt[K, 3] += VB.Val(dt.Rows[0]["IpgumAmt"].ToString().Trim()); //입금액
                FnAmt[K, 4] += VB.Val(dt.Rows[0]["SakAmt"].ToString().Trim()); //삭감액
                FnAmt[K, 6] += VB.Val(dt.Rows[0]["BanAmt"].ToString().Trim()); //반송액
                FnAmt[K, 7] += VB.Val(dt.Rows[0]["EtcAmt"].ToString().Trim()); //기타입금
                FnAmt[K, 8] += VB.Val(dt.Rows[0]["JanAmt"].ToString().Trim()); //월말잔액

                dt.Dispose();
                dt = null;

                #endregion


                //내용을 Sheet에 Display
                for (i = 1; i < 23; i++)
                {
                    for (j = 1; j < 9; j++)
                    {
                        ssView_Sheet1.Cells[i - 1, j + 1].Text = FnAmt[i, j].ToString("###,###,###,##0 ");
                    }
                }
                btnSTS.Enabled = true;



            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strYYMM = "";
            string strFDate = "";
            string strTdate = "";
            string strMSG = "";

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 7, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTdate = cf.READ_LASTDAY(clsDB.DbCon, strFDate);

            strMSG = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PANO , SUM(MONIAMT) AMT";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_GAINTONG  ";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + " GROUP BY PANO ";
                SQL = SQL + ComNum.VBLF + "HAVING SUM(MONIAMT) <> 0  ";
                SQL = SQL + ComNum.VBLF + " MINUS ";
                SQL = SQL + ComNum.VBLF + "SELECT PANO, SUM(AMT) AMT";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE BDATE >=TO_DATE('" + strFDate + "' ,'yyyy-mm-dd') ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE <=TO_DATE('" + strTdate + "' ,'yyyy-mm-dd') ";
                SQL = SQL + ComNum.VBLF + "   AND GUBUN1 ='2' ";
                SQL = SQL + ComNum.VBLF + " GROUP BY PANO   ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strMSG += ComNum.VBLF + dt.Rows[i]["Pano"].ToString().Trim() + " 개인 미수 입금액 차이 발생" + ComNum.VBLF;
                }
                dt.Dispose();
                dt = null;


                if (strMSG != "")
                {
                    ComFunc.MsgBox(strMSG);
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSTS_Click(object sender, EventArgs e)
        {
            frmPmpaViewBalRemark frm = new frmPmpaViewBalRemark(GstrYYMM, GstrMenu, GstrSMenu);
            frm.Show();
        }
    }
}
