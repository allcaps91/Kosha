using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewSakBanCheck.cs
    /// Description     : 월별 삭감, 반송액 점검표
    /// Author          : 박창욱
    /// Create Date     : 2017-08-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs42.frm(FrmSakBanCheck.frm) >> frmPmpaViewSakBanCheck.cs 폼이름 재정의" />	
    public partial class frmPmpaViewSakBanCheck : Form
    {
        double[,] FnAmt = new double[14, 10];

        public frmPmpaViewSakBanCheck()
        {
            InitializeComponent();
        }

        void SS_SET()
        {
            ssView_Sheet1.Rows[2].Visible = false;
            ssView_Sheet1.Rows[8].Visible = false;

            if (chkOpt00.Checked == true)   //건강보험 + 의료급여
            {
                ssView_Sheet1.Rows[0].Visible = true;
                ssView_Sheet1.Rows[1].Visible = true;
                ssView_Sheet1.Rows[2].Visible = true;
                ssView_Sheet1.Rows[6].Visible = true;
                ssView_Sheet1.Rows[7].Visible = true;
                ssView_Sheet1.Rows[8].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[0].Visible = false;
                ssView_Sheet1.Rows[1].Visible = false;
                ssView_Sheet1.Rows[2].Visible = false;
                ssView_Sheet1.Rows[6].Visible = false;
                ssView_Sheet1.Rows[7].Visible = false;
                ssView_Sheet1.Rows[8].Visible = false;
            }

            if (chkOpt01.Checked == true)   //산재 + 자보
            {
                ssView_Sheet1.Rows[3].Visible = true;
                ssView_Sheet1.Rows[4].Visible = true;
                ssView_Sheet1.Rows[9].Visible = true;
                ssView_Sheet1.Rows[10].Visible = true;
            }
            else
            {
                ssView_Sheet1.Rows[3].Visible = false;
                ssView_Sheet1.Rows[4].Visible = false;
                ssView_Sheet1.Rows[9].Visible = false;
                ssView_Sheet1.Rows[10].Visible = false;
            }

            //CLEAR
            ssView_Sheet1.Cells[0, 0, 9, 0].Text = "";

            if (chkOpt00.Checked == true && chkOpt01.Checked == true)
            {
                ssView_Sheet1.Cells[1, 0].Text = "외";
                ssView_Sheet1.Cells[4, 0].Text = "래";
                ssView_Sheet1.Cells[7, 0].Text = "입";
                ssView_Sheet1.Cells[10, 0].Text = "원";
            }
            else if (chkOpt01.Checked == true)
            {
                ssView_Sheet1.Cells[3, 0].Text = "외";
                ssView_Sheet1.Cells[4, 0].Text = "래";
                ssView_Sheet1.Cells[9, 0].Text = "입";
                ssView_Sheet1.Cells[10, 0].Text = "원";
            }
            else if (chkOpt00.Checked == true)
            {
                ssView_Sheet1.Cells[0, 0].Text = "외";
                ssView_Sheet1.Cells[2, 0].Text = "래";
                ssView_Sheet1.Cells[6, 0].Text = "입";
                ssView_Sheet1.Cells[8, 0].Text = "원";
            }

            if (chkOpt00.Checked == false && chkOpt01.Checked == false)
            {
                ssView_Sheet1.Rows[5].Visible = false;
                ssView_Sheet1.Rows[11].Visible = false;
                ssView_Sheet1.Rows[12].Visible = false;
            }
            else
            {
                ssView_Sheet1.Rows[5].Visible = true;
                ssView_Sheet1.Rows[11].Visible = true;
                ssView_Sheet1.Rows[12].Visible = true;
            }

            //CLEAR
            ssView_Sheet1.Cells[0, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
        }

        private void chkOpt00_CheckedChanged(object sender, EventArgs e)
        {
            SS_SET();
        }

        private void chkOpt01_CheckedChanged(object sender, EventArgs e)
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
                return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            Cursor.Current = Cursors.WaitCursor;

            //Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fs2";
            strHead1 = "/f1" + VB.Space(30);
            strHead1 = strHead1 + "월별 삭감,반송액 점검표";
            if (rdoJob00.Checked == true)
            {
                strHead2 = "/f2/n" + "진료월: " + cboYYMM.Text;
            }
            else
            {
                strHead2 = "/f2/n" + "통보월: " + cboYYMM.Text;
            }
            strHead2 += VB.Space(10) + "인쇄일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");

            //Print Body
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 50;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            int j = 0;
            int nBiNo = 0;
            double nAmt = 0;
            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";
            string strBiGbn = "";

           // if (rdoJob00.Checked == true)
           // {
           //     ComFunc.MsgBox("프로그램을 준비중입니다.");
           //     return;
           // }

            //누적할 배열을 Clear
            for (i = 0; i < 14; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    FnAmt[i, j] = 0;
                }
            }

            strYYMM = ComFunc.LeftH(cboYYMM.Text, 4) + ComFunc.MidH(cboYYMM.Text, 6, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strYYMM, 4)), (int)VB.Val(VB.Right(strYYMM, 2)));

            View_EdiBanAmt_ADD(strYYMM, ref nBiNo, ref nAmt);   //EDI반송금액 ADD
            View_EdiReMir_ADD(strYYMM, ref nAmt);    //EDI 재청구 접수액 ADD
            View_EdiSakAmt_ADD(strYYMM, ref nAmt);   //EDI 삭감액 ADD

            View_MisuBanSak_ADD(strYYMM, ref strBiGbn, ref nAmt);  //미수관리 삭감, 반송액 ADD
            View_MisuReMir_ADD(strYYMM, ref strBiGbn, ref nAmt);   //미수관리 재청구액 ADD

            //내용을 Sheet에 display
            for (i = 1; i < 14; i++)
            {
                FnAmt[i, 3] = FnAmt[i, 2] - FnAmt[i, 1];    //반송차액
                FnAmt[i, 6] = FnAmt[i, 5] - FnAmt[i, 4];    //재청구차액
                FnAmt[i, 9] = FnAmt[i, 8] - FnAmt[i, 7];    //삭감차액

                for (j = 1; j < 10; j++)
                {
                    ssView_Sheet1.Cells[i - 1, j + 1].Text = FnAmt[i, j].ToString("###,###,###,##0");
                }
            }


        }

        //EDI반송금액 ADD
        void View_EdiBanAmt_ADD(string strYYMM, ref int nBiNo, ref double nAmt)
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (chkOpt00.Checked == true)
                {
                    //보험
                    SQL = "";
                    SQL = "SELECT b.Bi,b.IpdOpd,SUM(b.EdiJAmt) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "EDI_RESULT3 a, " + ComNum.DB_PMPA + "MIR_INSID b ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "  AND a.YYMM = '" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND a.JCode >= '01' ";
                    SQL = SQL + ComNum.VBLF + "  AND a.JCode <= '99' ";
                    SQL = SQL + ComNum.VBLF + "  AND a.JCODE NOT IN('80','82','84','86')"; //심사중
                    SQL = SQL + ComNum.VBLF + "  AND a.WRTNO = b.WRTNO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY b.Bi,b.IpdOpd ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY b.Bi,b.IpdOpd ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nBiNo = Read_Bi_SuipTong(dt.Rows[i]["Bi"].ToString().Trim(), VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01");
                        nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                        if (nAmt != 0)
                        {
                            switch (nBiNo)
                            {
                                case 1:
                                    j = 1;  //보험
                                    break;
                                case 2:
                                    j = 2;  //보호
                                    break;
                                default:
                                    j = 1;  //기타는 보험으로
                                    break;
                            }
                            if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                            {
                                j += 6;
                            }
                            FnAmt[j, 1] += nAmt;
                            if (j <= 5)
                            {
                                FnAmt[6, 1] += nAmt;
                            }
                            else
                            {
                                FnAmt[12, 1] += nAmt;
                            }
                            FnAmt[13, 1] += nAmt;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }

                if (chkOpt01.Checked == true)
                {
                    //산재
                    SQL = "";
                    SQL = "SELECT IpdOpd,SUM(JAMT) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "EDI_SANRESULT3 ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "  AND YYMM='" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND JCode>='01' AND JCode<='98' ";
                    SQL = SQL + ComNum.VBLF + "  AND JCODE NOT IN('80','82','84','86')"; //심사중
                    SQL = SQL + ComNum.VBLF + "GROUP BY IpdOpd ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                        if (nAmt != 0)
                        {
                            j = 4;
                            if(dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                            {
                                j += 6;
                            }
                            FnAmt[j, 1] += nAmt;
                            if (j <= 5)
                            {
                                FnAmt[6, 1] += nAmt;
                            }
                            else
                            {
                                FnAmt[12, 1] += nAmt;
                            }
                            FnAmt[13, 1] += nAmt;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //EDI 재청구 접수액을 READ
        void View_EdiReMir_ADD(string strYYMM, ref double nAmt)
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (chkOpt00.Checked == true)
                {
                    //보험
                    SQL = "";
                    SQL = "SELECT Johap,IpdOpd,SUM(JepAmt) Amt ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "EDI_JEPSU ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "  AND YYMM = '" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND MirGbn='1' "; //보완(재)청구
                    SQL = SQL + ComNum.VBLF + "GROUP BY Johap,IpdOpd ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY Johap,IpdOpd ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                        if (nAmt != 0)
                        {
                            switch (dt.Rows[i]["Johap"].ToString().Trim())
                            {
                                case "5":
                                    j = 2;  //보호
                                    break;
                                default:
                                    j = 1;  //기타는 보험으로
                                    break;
                            }
                            if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                            {
                                j += 6;
                            }
                            FnAmt[j, 4] += nAmt;
                            if (j <= 5)
                            {
                                FnAmt[6, 4] += nAmt;
                            }
                            else
                            {
                                FnAmt[12, 4] += nAmt;
                            }
                            FnAmt[13, 4] += nAmt;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }

                if (chkOpt01.Checked == true)
                {
                    //산재
                    SQL = "";
                    SQL = "SELECT IpdOpd,SUM(JepAmt) Amt ";
                    SQL = SQL + " FROM " + ComNum.DB_PMPA + "EDI_SANJEPSU ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "  AND YYMM='" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND MirGbn='1' "; //보완(재)청구
                    SQL = SQL + ComNum.VBLF + "GROUP BY IpdOpd ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                        if (nAmt != 0)
                        {
                            j = 4;
                            if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                            {
                                j += 6;
                            }
                            FnAmt[j, 4] += nAmt;
                            if (j <= 5)
                            {
                                FnAmt[6, 4] += nAmt;
                            }
                            else
                            {
                                FnAmt[12, 4] += nAmt;
                            }
                            FnAmt[13, 4] += nAmt;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //EDI 삭감액 ADD
        void View_EdiSakAmt_ADD(string strYYMM, ref double nAmt)
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (chkOpt00.Checked == true)
                {
                    //보험
                    SQL = "";
                    SQL = "SELECT Johap,IpdOpd,SUM(SakAmt1+SakAmt2) Amt ";
                    SQL = SQL + " FROM " + ComNum.DB_PMPA + "EDI_RESULT2 ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "  AND YYMM = '" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND (SakAmt1 <> 0 OR SakAmt2 <> 0) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY Johap,IpdOpd ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY Johap,IpdOpd ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                        if (nAmt != 0)
                        {
                            switch (dt.Rows[i]["Johap"].ToString().Trim())
                            {
                                case "5":
                                    j = 2;  //보호
                                    break;
                                default:
                                    j = 1;  //기타는 보험으로
                                    break;
                            }
                            if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                            {
                                j += 6;
                            }
                            FnAmt[j, 7] += nAmt;
                            if (j <= 5)
                            {
                                FnAmt[6, 7] += nAmt;
                            }
                            else
                            {
                                FnAmt[12, 7] += nAmt;
                            }
                            FnAmt[13, 7] += nAmt;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }

                if (chkOpt01.Checked == true)
                {
                    //산재
                    SQL = "";
                    SQL = "SELECT IpdOpd,SUM(SakAmt) Amt ";
                    SQL = SQL + " FROM " + ComNum.DB_PMPA + "EDI_SANRESULT2 ";
                    SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "  AND YYMM = '" + strYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND SakAmt <> 0 ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY IpdOpd ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY IpdOpd ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                        if (nAmt != 0)
                        {
                            j = 4;
                            if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                            {
                                j += 6;
                            }
                            FnAmt[j, 7] += nAmt;
                            if (j <= 5)
                            {
                                FnAmt[6, 7] += nAmt;
                            }
                            else
                            {
                                FnAmt[12, 7] += nAmt;
                            }
                            FnAmt[13, 7] += nAmt;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //미수관리 삭감, 반송액 ADD
        void View_MisuBanSak_ADD(string strYYMM, ref string strBiGbn, ref double nAmt)
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strBiGbn = "''";
            if (chkOpt00.Checked == true)
            {
                strBiGbn += ",'01','02','03','04'";
            }
            if (chkOpt01.Checked == true)
            {
                strBiGbn += ",'05','07'";
            }

            try
            {
                //미수관리 반송액을 ADD
                SQL = "";
                SQL = "SELECT a.Class,a.IpdOpd,SUM(DECODE(b.Gubun,'31',b.Amt,'35',b.Amt,0)) SakAmt, ";
                SQL = SQL + ComNum.VBLF + "      SUM(DECODE(b.Gubun,'32',b.Amt,0)) BanAmt  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_IDMST a, " + ComNum.DB_PMPA + "MISU_SLIP b ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND a.MirYYMM = '" + strYYMM + "' ";
                if (strBiGbn != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND a.CLASS IN (" + strBiGbn + " ) ";
                }
                SQL = SQL + ComNum.VBLF + "  AND a.WRTNO=b.WRTNO(+) ";
                SQL = SQL + ComNum.VBLF + "  AND b.Gubun IN ('31','32','35') ";
                SQL = SQL + ComNum.VBLF + "GROUP BY a.Class,a.IpdOpd ";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.Class,a.IpdOpd ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i]["Class"].ToString().Trim())
                    {
                        case "01":
                        case "02":
                        case "03":
                            j = 1;  //보험
                            break;
                        case "04":
                            j = 2;  //보호
                            break;
                        case "05":
                            j = 4;  //산재
                            break;
                        case "07":
                            j = 5;  //자보
                            break;
                        default:
                            j = 1;  //기타는 보험으로
                            break;
                    }
                    if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                    {
                        j += 6;
                    }

                    //반송액 누적
                    nAmt = VB.Val(dt.Rows[i]["BanAmt"].ToString().Trim());
                    FnAmt[j, 2] += nAmt;
                    if (j <= 5)
                    {
                        FnAmt[6, 2] += nAmt;
                    }
                    else
                    {
                        FnAmt[12, 2] += nAmt;
                    }
                    FnAmt[13, 2] += nAmt;

                    //삭감액 누적
                    nAmt = VB.Val(dt.Rows[i]["SakAmt"].ToString().Trim());
                    FnAmt[j, 8] += nAmt;
                    if (j <= 5)
                    {
                        FnAmt[6, 8] += nAmt;
                    }
                    else
                    {
                        FnAmt[12, 8] += nAmt;
                    }
                    FnAmt[13, 8] += nAmt;   
                }
                dt.Dispose();
                dt = null;


            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //미수관리 재청구액 ADD
        //미수관리 삭감, 반송액 ADD
        void View_MisuReMir_ADD(string strYYMM, ref string strBiGbn, ref double nAmt)
        {
            int i = 0;
            int j = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strBiGbn = "''";
            if (chkOpt00.Checked == true)
            {
                strBiGbn += ",'01','02','03','04'";
            }
            if (chkOpt01.Checked == true)
            {
                strBiGbn += ",'05','07'";
            }

            try
            {
                SQL = "";
                SQL = "SELECT Class,IpdOpd,SUM(Amt2) Amt ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND MirYYMM = '" + strYYMM + "' ";
                if (strBiGbn != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND CLASS IN (" + strBiGbn + " ) ";
                }
                SQL = SQL + ComNum.VBLF + "  AND TongGbn='3' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY Class,IpdOpd ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Class,IpdOpd ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (nAmt != 0)
                    {
                        switch (dt.Rows[i]["Class"].ToString().Trim())
                        {
                            case "01":
                            case "02":
                            case "03":
                                j = 1;  //보험
                                break;
                            case "04":
                                j = 2;  //보호
                                break;
                            case "05":
                                j = 4;  //산재
                                break;
                            case "07":
                                j = 5;  //자보
                                break;
                            default:
                                j = 1;  //기타는 보험으로
                                break;
                        }
                        if (dt.Rows[i]["IpdOpd"].ToString().Trim() == "I")
                        {
                            j += 6;
                        }

                        //재청구액을 ADD
                        nAmt = VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                        FnAmt[j, 5] += nAmt;
                        if (j <= 5)
                        {
                            FnAmt[6, 5] += nAmt;
                        }
                        else
                        {
                            FnAmt[12, 5] += nAmt;
                        }
                        FnAmt[13, 5] += nAmt;
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //환자종류로 수입통계에서 사용하는 환자구분으로 변환
        int Read_Bi_SuipTong(string argBi, string argJobDate)
        {
            int rtnVar = 0;

            if (Convert.ToDateTime(argJobDate) >= Convert.ToDateTime("2003-11-03"))
            {
                switch (argBi)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "32":
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                        rtnVar = 1; //보험
                        break;
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                        rtnVar = 2; //보호
                        break;
                    case "31":
                    case "33":
                        rtnVar = 3; //산재
                        break;
                    case "52":
                        rtnVar = 4; //자보
                        break;
                    default:
                        rtnVar = 5; //일반
                        break;
                }
            }
            else
            {
                switch (argBi)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                        rtnVar = 1; //보험
                        break;
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                        rtnVar = 2; //보호
                        break;
                    case "31":
                    case "32":
                    case "33":
                        rtnVar = 3; //산재
                        break;
                    case "52":
                        rtnVar = 4; //자보
                        break;
                    default:
                        rtnVar = 5; //일반
                        break;
                }
            }

            return rtnVar;
        }

        private void cboYYMM_SelectedIndexChanged(object sender, EventArgs e)
        {
            ssView_Sheet1.Cells[0, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
        }

        private void frmPmpaViewSakBanCheck_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 12, "", "0");

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

        private void rdoJob00_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoJob00.Checked == true)
            {
                grbYYMM.Text = "진료월";
            }
            else
            {
                grbYYMM.Text = "통보(작업)월";
            }
        }
    }
}
