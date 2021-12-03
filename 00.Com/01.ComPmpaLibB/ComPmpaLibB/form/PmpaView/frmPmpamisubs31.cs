using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpamisubs31.cs
    /// Description     : 청구예정과 실청구액 비교 작업
    /// Author          : 이정현
    /// Create Date     : 2017-11-14
    /// <history> 
    /// 청구예정과 실청구액 비교 작업
    /// </history>
    /// <seealso>
    /// PSMH\misu\misubs\FrmMirCheck.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\misu\misubs\misubs.vbp
    /// </vbp>
    /// </summary>
    public partial class frmPmpamisubs31 : Form
    {
        private string GstrYYMM = "";       //작업년월
        private string GstrFDate = "";      //시작일자
        private string GstrTDate = "";      //종료일자
        private string GstrJong = "";       //작업시(보험종류구분)

        private clsPmpaFunc CF = new clsPmpaFunc();

        public frmPmpamisubs31()
        {
            InitializeComponent();
        }

        private void frmPmpamisubs31_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            lblMsg.Text = "";

            cboJong.Items.Clear();
            cboJong.Items.Add("0.전체");
            cboJong.Items.Add("1.건강보험");
            cboJong.Items.Add("2.의료급여");
            cboJong.Items.Add("3.산재");
            cboJong.Items.Add("4.자보");
            cboJong.SelectedIndex = 0;

            rdoIO0.Checked = true;

            progressBar1.Value = 0;
        }

        private void rdoIO_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoIO0.Checked == true)
            {
                btnSimsa.Enabled = true;
            }
            else if (rdoIO1.Checked == true)
            {
                btnSimsa.Enabled = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            GstrYYMM = dtpYYMM.Value.ToString("yyyyMM");
            GstrFDate = dtpYYMM.Value.ToString("yyyy-MM-01");
            GstrTDate = dtpYYMM.Value.ToString("yyyy-MM-") + DateTime.DaysInMonth(dtpYYMM.Value.Year, dtpYYMM.Value.Month).ToString("00");
            GstrJong = VB.Left(cboJong.Text, 1);

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //입원
                if (rdoIO0.Checked == true)
                {
                    //퇴원자 실청구액 UPDATE 퇴원건별(misu_baltewon 기준)
                    //if (Tewon_Mir_Check(clsDB.DbCon) == false)
                    //{
                    //    //clsDB.setRollbackTran(clsDB.DbCon);
                    //    //Cursor.Current = Cursors.Default;
                    //    //return;
                    //}

                    ////퇴원자 실청구액 UPDATE 개인별 환자종류별 (misu_baltewon 기준)  
                    //if (Tewon_Mir_Check2(clsDB.DbCon) == false)
                    //{
                    //    //clsDB.setRollbackTran(clsDB.DbCon);
                    //    //Cursor.Current = Cursors.Default;
                    //    //return;
                    //}

                    ////중간청구 실청구액 UPDATE
                    //if (Junggan_Mir_Check(clsDB.DbCon) == false)
                    //{
                    //    //clsDB.setRollbackTran(clsDB.DbCon);
                    //    //Cursor.Current = Cursors.Default;
                    //    //return;
                    //}
                    Tewon_Mir_Check(clsDB.DbCon);
                    Tewon_Mir_Check2(clsDB.DbCon);
                    Junggan_Mir_Check(clsDB.DbCon);
                }
                //외래
                else
                {
                    if (VB.Val(GstrYYMM) >= 201410)
                    {
                        //외래 수납 월별 누적으로변경 (진료과별 누적제외)
                        if (OPD_MIR_CHECK_201410(clsDB.DbCon) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else if (VB.Val(GstrYYMM) >= 201404)
                    {
                        //외래 수납 월별 누적으로변경
                        if (OPD_MIR_CHECK_201404(clsDB.DbCon) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else if (VB.Val(GstrYYMM) >= 201302)
                    {
                        //외래 수납 산재로직 변경
                        if (OPD_MIR_CHECK_201302(clsDB.DbCon) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else if (VB.Val(GstrYYMM) >= 201201)
                    {
                        //외래 수납
                        if (OPD_MIR_CHECK_201201(clsDB.DbCon) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        //외래 수납
                        if (OPD_MIR_CHECK(clsDB.DbCon) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("통계형성 완료");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (VB.Left(cboJong.Text, 1) != "1" && VB.Left(cboJong.Text, 1) != "2")
            {
                ComFunc.MsgBox("건강보험, 의료급여만 작업가능합니다.");
                return;
            }

            GstrYYMM = dtpYYMM.Value.ToString("yyyyMM");
            GstrFDate = dtpYYMM.Value.ToString("yyyy-MM-01");
            GstrTDate = dtpYYMM.Value.ToString("yyyy-MM-") + DateTime.DaysInMonth(dtpYYMM.Value.Year, dtpYYMM.Value.Month).ToString("00");
            GstrJong = VB.Left(cboJong.Text, 1);

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //입원
                if (rdoIO0.Checked == true)
                {
                    //퇴원자 실청구액 UPDATE 개인별 환자종류별 (misu_baltewon 기준)
                    if (Tewon_Mir_check2_Remark(clsDB.DbCon) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                //외래
                else
                {
                    if (OPD_MIR_CHECK_REMARK(clsDB.DbCon) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    //if (OPD_MIR_자동점검_REMARK(clsDB.DbCon) == false)
                    //{
                    //    clsDB.setRollbackTran(clsDB.DbCon);
                    //    Cursor.Current = Cursors.Default;
                    //    return;
                    //}
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("통계형성 완료");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSimsa_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (rdoIO0.Checked == false) { return; }

            GstrYYMM = dtpYYMM.Value.ToString("yyyyMM");
            GstrFDate = dtpYYMM.Value.ToString("yyyy-MM-01");
            GstrTDate = dtpYYMM.Value.ToString("yyyy-MM-") + DateTime.DaysInMonth(dtpYYMM.Value.Year, dtpYYMM.Value.Month).ToString("00");
            GstrJong = VB.Left(cboJong.Text, 1);

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;

            string strROWID = "";

            double dblSimAmt = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                #region Tewon_SIMSA_Check   퇴원건별 심사조정액 BUILD

                lblMsg.Text = "★ 퇴원자 심사조정액 BUILD  ★";
                progressBar1.Value = 0;

                //당월의 퇴원환자 명단을 SELECT
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(ActDate,'YYYY-MM-DD') AS ActDate, Pano, Bi,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(InDate,'YYYY-MM-DD') AS InDate, TO_CHAR(OutDate,'YYYY-MM-DD') AS OutDate,";
                SQL = SQL + ComNum.VBLF + "     DeptCode, TotAmt, Junggan, Johap, ROWID, IPDNO, TRSNO  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
                SQL = SQL + ComNum.VBLF + "     WHERE ActDate >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND ActDate <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND Gubun IN ('1','3') ";      //퇴원자,응급실6시간이상+NP낮병동

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "          AND SUBI = '1' "; } //보험
                else if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '2' "; } //보호
                else if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '3' "; } //산재
                else if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '4' "; } //자보

                SQL = SQL + ComNum.VBLF + "ORDER BY Bi, Pano, ActDate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                progressBar1.Maximum = dt.Rows.Count - 1;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    progressBar1.Value = i;

                    strROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                    #region GoSub SimSa_Jojung  재원심사조정액 build

                    dblSimAmt = 0;

                    if (Convert.ToDateTime(GstrFDate) <= Convert.ToDateTime("2012-07-01"))
                    {
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     SUM(a.AMT1 + a.AMT2) AS AMT";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP A, ";
                        SQL = SQL + ComNum.VBLF + "         (SELECT";
                        SQL = SQL + ComNum.VBLF + "             TO_NUMBER(RTRIM(SABUN),'999999') AS SABUN, KORNAME, BUSE";
                        SQL = SQL + ComNum.VBLF + "         FROM " + ComNum.DB_ERP + "INSA_MST ) D ,";
                        SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_BUSE E";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.TRSNO = '" + dt.Rows[i]["TRSNO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.PART  = D.SABUN(+)";
                        SQL = SQL + ComNum.VBLF + "         AND A.PART NOT IN ( '!','#','$','+','-','?','\','^','*','**','@','!!'  )";
                        SQL = SQL + ComNum.VBLF + "         AND D.BUSE = E.BUCODE(+)";
                        SQL = SQL + ComNum.VBLF + "         AND D.BUSE NOT IN ('100570','033102','033103')";
                    }
                    else
                    {
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     SUM(a.AMT1 + a.AMT2) AS AMT";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a";
                        SQL = SQL + ComNum.VBLF + "     WHERE a.TRSNO = '" + dt.Rows[i]["TRSNO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND a.PART IN";
                        SQL = SQL + ComNum.VBLF + "                 (SELECT";
                        SQL = SQL + ComNum.VBLF + "                     SABUN ";
                        SQL = SQL + ComNum.VBLF + "                 FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_PMPA + "BAS_BUSE B  ";
                        SQL = SQL + ComNum.VBLF + "                     WHERE A.BUSE  = B.BUCODE ";
                        SQL = SQL + ComNum.VBLF + "                         AND B.NAME LIKE '%심사%')";
                    }

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        dblSimAmt = VB.Val(dt1.Rows[0]["AMT"].ToString().Trim());
                    }

                    dt1.Dispose();
                    dt1 = null;

                    //DB에 UPDATE
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALTEWON";
                    SQL = SQL + ComNum.VBLF + "     SET";
                    SQL = SQL + ComNum.VBLF + "         SIMAMT = '" + dblSimAmt + "'";
                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("MISU_BALDAILY에 심사조정 자료를 등록중 오류발생");
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    #endregion
                }

                dt.Dispose();
                dt = null;
                
                #endregion

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("통계형성 완료");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        //퇴원건별
        private bool Tewon_Mir_Check(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            int k = 0;

            string strPano = "";
            string strBi = "";
            string strInDate = "";
            string strOutDate = "";
            string strTInDate = "";
            string strTOutDate = "";
            string strDeptCode = "";
            string strROWID = "";
            string strBohoNP = "";
            string strGubun = "";
            string strJepDate = "";
            string strJepno = "";
            string strVCode = "";
            string strMisuFrom = "";
            string strMisuTo = "";

            double dblTotAmt = 0;
            double dblSlipJAmt = 0;
            double dblJepJAmt = 0;
            double dblWRTNO = 0;

            //보호NP등 서면청구 접수일자를 작업월의 익월1일로 SET
            string strSlipMirDate = Convert.ToDateTime(GstrTDate).AddDays(1).ToString("yyyy-MM-dd");

            lblMsg.Text = "★ 퇴원자 실청구액 UPDATE ★";
            progressBar1.Value = 0;

            try
            {
                //당월의 퇴원환자 명단을 SELECT
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.ActDate,'YYYY-MM-DD') AS ActDate, A.Pano, A.Bi,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.InDate,'YYYY-MM-DD') AS InDate, TO_CHAR(A.OutDate,'YYYY-MM-DD') AS OutDate,";
                SQL = SQL + ComNum.VBLF + "     A.DeptCode, A.TotAmt, A.Junggan, A.Johap, A.ROWID, A.IPDNO, A.TRSNO, a.Gubun,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(B.INDATE,'YYYY-MM-DD') AS TINDATE, TO_CHAR(B.OUTDATE,'YYYY-MM-DD') AS TOUTDATE, ";
                SQL = SQL + ComNum.VBLF + "     B.VCODE, B.OGPDBUN, B.OGPDBUNDTL";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALTEWON A, " + ComNum.DB_PMPA + "IPD_TRANS B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.ActDate >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.ActDate <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.Gubun IN ('1','3') ";    //퇴원자,응급실6시간이상+NP낮병동
                SQL = SQL + ComNum.VBLF + "         AND A.JepDate IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND (A.WRTNO IS NULL OR A.WRTNO = 0) ";
                SQL = SQL + ComNum.VBLF + "         AND Johap > 0 ";
                
                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "          AND A.SUBI = '1' "; }     //보험
                else if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "          AND A.SUBI = '2' "; }     //보호
                else if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "          AND A.SUBI = '3' "; }     //산재
                else if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "          AND A.SUBI = '4' "; }     //자보

                SQL = SQL + ComNum.VBLF + "         AND A.IPDNO = B.IPDNO(+)";
                SQL = SQL + ComNum.VBLF + "         AND A.TRSNO = B.TRSNO(+)";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.Bi, A.Pano, A.ActDate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                progressBar1.Maximum = dt.Rows.Count - 1;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    progressBar1.Value = i;

                    strPano = dt.Rows[i]["PANO"].ToString().Trim();
                    strBi = dt.Rows[i]["BI"].ToString().Trim();
                    strDeptCode = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    strGubun = dt.Rows[i]["GUBUN"].ToString().Trim();
                    strInDate = dt.Rows[i]["INDATE"].ToString().Trim();
                    strOutDate = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    strTInDate = dt.Rows[i]["TINDATE"].ToString().Trim();
                    strTOutDate = dt.Rows[i]["TOUTDATE"].ToString().Trim();
                    strROWID = dt.Rows[i]["ROWID"].ToString().Trim();

                    dblTotAmt = VB.Val(dt.Rows[i]["TOTAMT"].ToString().Trim());
                    dblSlipJAmt = VB.Val(dt.Rows[i]["JOHAP"].ToString().Trim());

                    strVCode = dt.Rows[i]["VCODE"].ToString().Trim();

                    //환자종류를 기준 청구자료를 READ
                    switch (strBi)
                    {
                        //산재
                        case "31":
                            #region GoSub Tewon_Mir_SAN     산재 실청구액을 READ

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     WRTNO, a.EdiTAmt, b.JepNo, TO_CHAR(b.JepDate,'YYYY-MM-DD') AS JepDate ";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID a, " + ComNum.DB_PMPA + "EDI_SANJEPSU b ";
                            SQL = SQL + ComNum.VBLF + "     WHERE a.Pano='" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND a.IpdOpd = 'I' ";
                            SQL = SQL + ComNum.VBLF + "         AND a.YYMM >= '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND a.FrDate >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "         AND a.ToDate <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "         AND a.EdiMirNo > '0' ";    //EDI청구분
                            SQL = SQL + ComNum.VBLF + "         AND A.GBNEDI = '0' ";      //원청구
                            SQL = SQL + ComNum.VBLF + "         AND a.EdiMirNo = b.MirNo(+) ";
                            SQL = SQL + ComNum.VBLF + "         AND b.Week <> '7' ";       //중간청구는 제외

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 1)
                            {
                                //청구내역을 UPDATE
                                dblWRTNO = VB.Val(dt1.Rows[0]["WRTNO"].ToString().Trim());
                                strJepDate = dt1.Rows[0]["JEPDATE"].ToString().Trim();
                                strJepno = dt1.Rows[0]["JEPNO"].ToString().Trim();
                                dblJepJAmt = VB.Val(dt1.Rows[0]["EDITAMT"].ToString().Trim());

                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALTEWON";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JepDate = TO_DATE('" + strJepDate + "', 'YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + "         JepNo = '" + strJepno + "', ";
                                SQL = SQL + ComNum.VBLF + "         JepJAmt = " + dblJepJAmt + ", ";
                                SQL = SQL + ComNum.VBLF + "         WRTNO = " + dblWRTNO;
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("MISU_BALDAILY에 자료를 등록중 오류발생");
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                            #endregion
                            break;
                        //자보
                        case "52":
                            #region GoSub Tewon_Mir_TA      자보 실청구액을 READ

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     Amt2, TO_CHAR(BDate,'YYYY-MM-DD') AS BDate,";
                            SQL = SQL + ComNum.VBLF + "     TO_CHAR(FromDate,'YYYY-MM-DD') AS FromDate,";
                            SQL = SQL + ComNum.VBLF + "     TO_CHAR(ToDate,'YYYY-MM-DD') AS ToDate ";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                            SQL = SQL + ComNum.VBLF + "     WHERE MisuID = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND IpdOpd = 'I' ";
                            SQL = SQL + ComNum.VBLF + "         AND FromDate >= TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "         AND ToDate <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "         AND MirYYMM >= '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND Class = '07' ";
                            SQL = SQL + ComNum.VBLF + "         AND Amt2 <> 0 ";
                            SQL = SQL + ComNum.VBLF + "ORDER BY BDate ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 1)
                            {
                                strJepDate = dt1.Rows[0]["BDATE"].ToString().Trim();
                                strJepno = "";
                                dblJepJAmt = VB.Val(dt1.Rows[0]["AMT2"].ToString().Trim());
                                strMisuFrom = dt1.Rows[0]["FROMDATE"].ToString().Trim();
                                strMisuTo = dt1.Rows[0]["TODATE"].ToString().Trim();

                                dt1.Dispose();
                                dt1 = null;

                                dblWRTNO = 0;

                                //청구번호를 READ
                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     WRTNO";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_TAID ";
                                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND IpdOpd = 'I' ";
                                SQL = SQL + ComNum.VBLF + "         AND YYMM >= '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND FrDate >= TO_DATE('" + strMisuFrom + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "         AND ToDate <= TO_DATE('" + strMisuTo + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "         AND UpCnt1 <> '9' ";   //보류자 제외

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    dblWRTNO = VB.Val(dt1.Rows[0]["WRTNO"].ToString().Trim());
                                }

                                dt1.Dispose();
                                dt1 = null;

                                //DB에 UPDATE
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALTEWON";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JepDate = TO_DATE('" + strJepDate + "','YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + "         JepNo = '" + strJepno + "', ";
                                SQL = SQL + ComNum.VBLF + "         JepJAmt = " + dblJepJAmt + ", ";
                                SQL = SQL + ComNum.VBLF + "         WRTNO = " + dblWRTNO;
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("MISU_BALDAILY에 자료를 등록중 오류발생");
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                            }
                            #endregion
                            break;
                        //보험,보호 청구서별
                        default:
                            #region GoSub Tewon_Mir_Bohum       보험,보호 실청구액을 READ

                            strBohoNP = "";

                            //보호np정액은 200201부터 edi청구함
                            //보호정신과 서면청구는 청구서를 기준함
                            if (VB.Val(GstrYYMM) < 200201 && strDeptCode == "NP" && (VB.Val(strBi) >= 21 && VB.Val(strBi) <= 29))
                            {
                                strBohoNP = "YES";

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     WRTNO, TO_CHAR(OutDate,'YYYY-MM-DD') AS OutDate, JinDate1, EdiJAmt, JAmt, BoAmt, edigamt, NVL(EDIUAMT100,'0') AS EDIUAMT100 ";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID ";
                                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND YYMM >= '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND IpdOpd = 'I' ";
                                SQL = SQL + ComNum.VBLF + "         AND Bi = '" + strBi + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND UpCnt1 <> '9' ";
                                SQL = SQL + ComNum.VBLF + "         AND JinDate1 >= '" + strInDate.Replace("-", "") + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND JinDate1 <= '" + strOutDate.Replace("-", "") + "' ";
                                SQL = SQL + ComNum.VBLF + "ORDER BY OutDate DESC ";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     WRTNO, TO_CHAR(a.OutDate,'YYYY-MM-DD') AS OutDate, a.JinDate1, a.EdiJAmt, a.JAmt, a.EdiBoAmt, A.EDIGAMT, NVL(A.EDIUAMT100,'0') AS EDIUAMT100, ";
                                SQL = SQL + ComNum.VBLF + "     b.JepNo, TO_CHAR(b.JepDate,'YYYY-MM-DD') AS JepDate, GBMIR ";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID a, " + ComNum.DB_PMPA + "EDI_JEPSU b ";
                                SQL = SQL + ComNum.VBLF + "     WHERE a.Pano = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND a.IpdOpd = 'I' ";

                                if (strGubun == "1") { SQL = SQL + ComNum.VBLF + "          AND a.YYMM >= '" + GstrYYMM + "' "; }

                                SQL = SQL + ComNum.VBLF + "         AND a.Bi = '" + strBi + "' ";   //환자종류가 같은것

                                //2016-07-11
                                if (strGubun == "3")
                                {
                                    SQL = SQL + ComNum.VBLF + "         AND (b.MirGbn IS NULL OR b.MirGbn = '0')  ";
                                }

                                if (strTInDate != "")
                                {
                                    SQL = SQL + ComNum.VBLF + "         and a.IODATE = '" + strTInDate.Replace("-", "") + strTOutDate.Replace("-", "") + "' ";

                                    if (strVCode == "")
                                    {
                                        SQL = SQL + ComNum.VBLF + "         AND A.VCODE IS NULL";
                                    }
                                    else
                                    {
                                        SQL = SQL + ComNum.VBLF + "         AND a.VCODE = '" + strVCode + "' ";
                                    }
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "         AND a.JinDate1 >= '" + strInDate.Replace("-", "") + "' ";

                                    if (strOutDate.Trim() != "") { SQL = SQL + ComNum.VBLF + "          AND a.JinDate1<='" + strOutDate.Replace("-", "") + "' "; }
                                }

                                SQL = SQL + ComNum.VBLF + "         AND a.EdiMirNo > '0' ";     //EDI청구분
                                SQL = SQL + ComNum.VBLF + "         AND a.EdiMirNo=b.MirNo(+) ";

                                if (strDeptCode != "EM" && strDeptCode != "ER")
                                {
                                    SQL = SQL + ComNum.VBLF + "         AND b.Week <> '7' ";    //중간청구는 제외
                                    SQL = SQL + ComNum.VBLF + "         AND b.MirGbn ='0' ";
                                }

                                //BOHOJONG    CHAR(1)     청구종류(0.일반 1.재청구 2.추가 3,분리청구 4.서면청구,8.약제상한)
                                SQL = SQL + ComNum.VBLF + "         AND A.BOHOJONG NOT IN ('1')";
                                SQL = SQL + ComNum.VBLF + "ORDER BY a.JinDate1 DESC ";
                            }

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                for (k = 0; k < dt1.Rows.Count; k++)
                                {
                                    //청구내역을 UPDATE
                                    if (strBohoNP == "YES")
                                    {
                                        dblWRTNO = VB.Val(dt1.Rows[k]["WRTNO"].ToString().Trim());
                                        strJepDate = strSlipMirDate;        //서면청구 익월1일자
                                        strJepno = "1";
                                        dblJepJAmt = VB.Val(dt1.Rows[k]["JAMT"].ToString().Trim()) + VB.Val(dt1.Rows[k]["BOAMT"].ToString().Trim()) + VB.Val(dt1.Rows[k]["EDIGAMT"].ToString().Trim()) + VB.Val(dt1.Rows[k]["EDIUAMT100"].ToString().Trim());
                                    }
                                    else
                                    {
                                        dblWRTNO = VB.Val(dt1.Rows[k]["WRTNO"].ToString().Trim());
                                        strJepDate = dt1.Rows[k]["JEPDATE"].ToString().Trim();
                                        strJepno = dt1.Rows[k]["JEPNO"].ToString().Trim();
                                        dblJepJAmt = VB.Val(dt1.Rows[k]["EDIJAMT"].ToString().Trim()) + VB.Val(dt1.Rows[k]["EDIBOAMT"].ToString().Trim()) + VB.Val(dt1.Rows[k]["EDIGAMT"].ToString().Trim()) + VB.Val(dt1.Rows[k]["EDIUAMT100"].ToString().Trim());
                                    }
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;

                            //해당 청구번호가 이미 있으면 UPDATE 안함
                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     COUNT(*) AS CNT";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALTEWON ";
                            SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + dblWRTNO;

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                if (VB.Val(dt1.Rows[0]["CNT"].ToString().Trim()) > 0)
                                {
                                    dblWRTNO = 0;
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;

                            if (dblWRTNO > 0 && strJepDate.Trim() != "")
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALTEWON";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JepDate = TO_DATE('" + strJepDate + "', 'YYYY-MM-DD'),";
                                SQL = SQL + ComNum.VBLF + "         JepNo = '" + strJepno + "', ";
                                SQL = SQL + ComNum.VBLF + "         JepJAmt = " + dblJepJAmt + ", ";
                                SQL = SQL + ComNum.VBLF + "         WRTNO = " + dblWRTNO;
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("MISU_BALDAILY에 자료를 등록중 오류발생");
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                            }

                            #endregion
                            break;
                    }
                }

                dt.Dispose();
                dt = null;
                
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        //병록번호별 환자종류별
        private bool Tewon_Mir_Check2(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            
            lblMsg.Text = "★ 퇴원자 실청구액 UPDATE ★";
            progressBar1.Value = 0;

            try
            {
                #region GoSub BASE_BUILD 기본자료 형성

                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                SQL = SQL + ComNum.VBLF + "     SET";
                SQL = SQL + ComNum.VBLF + "         TOTAMT = 0, ";
                SQL = SQL + ComNum.VBLF + "         JUNGGAN = 0, ";
                SQL = SQL + ComNum.VBLF + "         JOHAP = 0, ";
                SQL = SQL + ComNum.VBLF + "         HALIN = 0, ";
                SQL = SQL + ComNum.VBLF + "         BOJUNG = 0, ";
                SQL = SQL + ComNum.VBLF + "         ETCMISU = 0, ";
                SQL = SQL + ComNum.VBLF + "         SUNAP = 0, ";
                SQL = SQL + ComNum.VBLF + "         DANSU = 0, ";
                SQL = SQL + ComNum.VBLF + "         JEPJAMT = 0";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM = '" + GstrYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "     AND Gubun IN ('1', '3')";
                SQL = SQL + ComNum.VBLF + "     AND (Chk IS NULL OR Chk <> 'Y')";    //2016-11-07  빌드 clear 제외

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "      AND SUBI = '1' "; }  //보험
                else if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '2' "; }  //보호
                else if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '3' "; }  //산재
                else if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '4' "; }  //자보

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     a.Gubun, a.PANO, B.SNAME, a.BI, a.SUBI, SUM(a.TOTAMT) AS TOTAMT, SUM(a.JUNGGAN) AS JUNGGAN, SUM(a.JOHAP) JOHAP,  SUM(a.HALIN) AS HALIN, ";
                SQL = SQL + ComNum.VBLF + "     SUM(a.BOJUNG) AS BOJUNG, SUM(a.ETCMISU) AS ETCMISU, ";
                SQL = SQL + ComNum.VBLF + "     SUM(a.SUNAP) AS SUNAP, SUM(a.DANSU) AS DANSU, SUM(a.JEPJAMT) AS JEPJAMT    ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALTEWON a, " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
                SQL = SQL + ComNum.VBLF + "     WHERE ACTDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND ACTDATE <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND SUBI ='1' "; } //보험
                if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND SUBI ='2' "; } //보호
                if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "         AND SUBI ='3' "; } //산재
                if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "         AND SUBI ='4' "; } //자보

                SQL = SQL + ComNum.VBLF + "         AND a.Gubun IN ('1', '3')";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+)";
                SQL = SQL + ComNum.VBLF + "GROUP BY a.Gubun, a.PANO, B.SNAME, a.BI, a.SUBI";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     ROWID";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                        SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "         AND YYMM = '" + GstrYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'I' ";
                        SQL = SQL + ComNum.VBLF + "         AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND GUBUN = '" + dt.Rows[i]["GUBUN"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count == 0)
                        {
                            SQL = "";
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, SUBI, IPDOPD, TOTAMT, JUNGGAN, JOHAP, HALIN, BOJUNG, ETCMISU, SUNAP, DANSU, Gubun) ";
                            SQL = SQL + ComNum.VBLF + "VALUES";
                            SQL = SQL + ComNum.VBLF + "     (";
                            SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["PANO"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["BI"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SUBI"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         'I', ";
                            SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["TOTAMT"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["JUNGGAN"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["JOHAP"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["HALIN"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["BOJUNG"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["ETCMISU"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SUNAP"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["DANSU"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["Gubun"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "     )";
                        }
                        else
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         TOTAMT = '" + dt.Rows[i]["TOTAMT"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         JUNGGAN = '" + dt.Rows[i]["JUNGGAN"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         JOHAP = '" + dt.Rows[i]["JOHAP"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         HALIN = '" + dt.Rows[i]["HALIN"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         BOJUNG = '" + dt.Rows[i]["BOJUNG"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         ETCMISU = '" + dt.Rows[i]["ETCMISU"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         SUNAP = '" + dt.Rows[i]["SUNAP"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         DANSU = '" + dt.Rows[i]["DANSU"].ToString().Trim() + "', ";
                            SQL = SQL + ComNum.VBLF + "         SNAME = '" + dt.Rows[i]["SNAME"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                if (GstrJong == "0" || GstrJong == "1" || GstrJong == "2")
                {
                    #region GoSub Tewon_PANO_Mir_Bohum2 보험,보호 청구서별

                    progressBar1.Value = 0;

                    //미수기준으로 변경 하였습니다.
                    SQL = "";
                    SQL = "CREATE OR REPLACE VIEW VIEW_MISU_BALCHACK_PANO AS";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SELECT";
                    SQL = SQL + ComNum.VBLF + "             PANO, SNAME, BI, JOHAP, SUM(EdiJAmt + EDIBOAMT + EDIGAMT + NVL(EDIUAMT100,'0')) AS EDIJAMT , GBMIR ";
                    SQL = SQL + ComNum.VBLF + "         FROM " + ComNum.DB_PMPA + "MIR_INSID  ";
                    SQL = SQL + ComNum.VBLF + "             WHERE EDIMIRNO IN";
                    SQL = SQL + ComNum.VBLF + "                         (SELECT";
                    SQL = SQL + ComNum.VBLF + "                             B.MIRNO ";
                    SQL = SQL + ComNum.VBLF + "                         FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "EDI_JEPSU B";
                    SQL = SQL + ComNum.VBLF + "                             WHERE A.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "                                 AND A.TONGGBN = '1'";
                    SQL = SQL + ComNum.VBLF + "                                 AND A.IpdOpd = 'I'";

                    if (GstrJong == "1")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND A.CLASS IN ('01', '02', '03') ";  //보험
                    }
                    else if (GstrJong == "2")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND A.CLASS = '04' ";    //보호
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND A.CLASS <= '04' ";   //보험, 보호
                    }

                    SQL = SQL + ComNum.VBLF + "                                 AND A.MISUID = B.JEPNO";
                    SQL = SQL + ComNum.VBLF + "                                 AND B.JEPDATE >= TO_DATE('" + GstrYYMM + "01','YYYYMMDD') ";
                    SQL = SQL + ComNum.VBLF + "                         GROUP BY B.MIRNO)";
                    SQL = SQL + ComNum.VBLF + "         GROUP BY PANO, SNAME, BI, JOHAP, GBMIR ";
                    SQL = SQL + ComNum.VBLF + "         UNION ALL ";
                    SQL = SQL + ComNum.VBLF + "         SELECT";
                    SQL = SQL + ComNum.VBLF + "             PANO, SNAME, BI, JOHAP, SUM(EdiJAmt + EDIBOAMT+ EDIGAMT + NVL(EDIUAMT100,'0')) AS EDIJAMT , GBMIR ";
                    SQL = SQL + ComNum.VBLF + "         FROM " + ComNum.DB_PMPA + "MIR_INSID  ";
                    SQL = SQL + ComNum.VBLF + "             WHERE EDIMIRNO IN";
                    SQL = SQL + ComNum.VBLF + "                         (SELECT";
                    SQL = SQL + ComNum.VBLF + "                             B.MIRNO";
                    SQL = SQL + ComNum.VBLF + "                         FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "EDI_JEPSU B";
                    SQL = SQL + ComNum.VBLF + "                             WHERE A.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "                                 AND A.TONGGBN = '2'";
                    SQL = SQL + ComNum.VBLF + "                                 AND A.IpdOpd = 'I'";

                    if (GstrJong == "1")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND A.CLASS IN ('01', '02', '03') "; //보험
                    }
                    else if (GstrJong == "2")
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND A.CLASS = '04' ";   //보호
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                                 AND A.CLASS <= '04'  "; //보험, 보호
                    }

                    SQL = SQL + ComNum.VBLF + "                                 AND A.MISUID = B.JEPNO";
                    SQL = SQL + ComNum.VBLF + "                                 AND B.JEPDATE >= TO_DATE('" + GstrYYMM + "01','YYYYMMDD') ";
                    SQL = SQL + ComNum.VBLF + "                         GROUP BY B.MIRNO)";
                    SQL = SQL + ComNum.VBLF + "                 AND GBMIR = '3'";    //응급실 6시간 이상
                    SQL = SQL + ComNum.VBLF + "         GROUP BY PANO, SNAME, BI, JOHAP, GBMIR ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PANO, B.SNAME, A.BI, A.JOHAP, A.GBMIR, SUM(A.EDIJAMT) AS EDIJAMT";
                    SQL = SQL + ComNum.VBLF + "FROM VIEW_MISU_BALCHACK_PANO A, " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.GBMIR IN ('0','1','3') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+)";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.BI, A.JOHAP, A.GBMIR ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";

                            if (dt.Rows[i]["GBMIR"].ToString().Trim() == "3")
                            {
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '3'";
                            }
                            else if (dt.Rows[i]["GBMIR"].ToString().Trim() == "2")
                            {
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '2'";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '1'";
                            }

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["PANO"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["BI"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         'I', ";

                                //보호
                                if (dt.Rows[i]["JOHAP"].ToString().Trim() == "5")
                                {
                                    SQL = SQL + ComNum.VBLF + "         '2', ";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "         '1', ";
                                }

                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["EDIJAMT"].ToString().Trim() + "', ";

                                if (dt.Rows[i]["GBMIR"].ToString().Trim() == "3")
                                {
                                    SQL = SQL + ComNum.VBLF + "         '3'";
                                }
                                else if (dt.Rows[i]["GBMIR"].ToString().Trim() == "2")
                                {
                                    SQL = SQL + ComNum.VBLF + "         '2'";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "         '1'";
                                }

                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dt.Rows[i]["EDIJAMT"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;


                    Tewon_Mir_check2_Remark(pDbCon);

                    #endregion
                }

                if (GstrJong == "0" || GstrJong == "3")
                {
                    if (VB.Val(GstrYYMM) >= 201302)
                    {
                        #region GoSub Tewon_PANO_Mir_SAN3 산재

                        progressBar1.Value = 0;

                        //EDI 청구분
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, SUM(a.EdiTAmt) AS EDITAMT ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID a, " + ComNum.DB_PMPA + "MISU_IDMST B ";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = B.MISUID ";
                        SQL = SQL + ComNum.VBLF + "         AND A.FRDATE = B.FROMDATE ";
                        SQL = SQL + ComNum.VBLF + "         AND A.TODATE = B.TODATE ";
                        SQL = SQL + ComNum.VBLF + "         AND B.MIRYYMM = '" + GstrYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND B.TONGGBN IN ('1') ";
                        SQL = SQL + ComNum.VBLF + "         AND B.IpdOpd = 'I'";
                        SQL = SQL + ComNum.VBLF + "         AND B.CLASS IN ('05') ";  //산재
                        SQL = SQL + ComNum.VBLF + "         AND a.IpdOpd = 'I' ";
                        SQL = SQL + ComNum.VBLF + "         AND a.EdiMirNo > '0' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.GBNEDI = '0' "; //원청구
                        SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, A.SNAME";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            progressBar1.Maximum = dt.Rows.Count - 1;

                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                progressBar1.Value = i;

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     ROWID";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '1'";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count == 0)
                                {
                                    SQL = "";
                                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                    SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                    SQL = SQL + ComNum.VBLF + "VALUES";
                                    SQL = SQL + ComNum.VBLF + "     (";
                                    SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["PANO"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '31', 'I', '3', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["EDITAMT"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '1'";
                                    SQL = SQL + ComNum.VBLF + "     )";
                                }
                                else
                                {
                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dt.Rows[i]["EDITAMT"].ToString().Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                                }

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                        }

                        dt.Dispose();
                        dt = null;

                        progressBar1.Value = 0;

                        //서면청구분
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, SUM(A.JAmt) AS EDITAMT ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID a ";
                        SQL = SQL + ComNum.VBLF + "     WHERE a.IpdOpd = 'I' ";
                        SQL = SQL + ComNum.VBLF + "         AND a.YYMM = '" + GstrYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND a.EdiGbn = '3' "; //2003년8월 이전
                        SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, A.SNAME";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            progressBar1.Maximum = dt.Rows.Count - 1;

                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     ROWID";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '1'";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count == 0)
                                {
                                    SQL = "";
                                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                    SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                    SQL = SQL + ComNum.VBLF + "VALUES";
                                    SQL = SQL + ComNum.VBLF + "     (";
                                    SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["PANO"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '31', 'I', '3', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + VB.Fix(Convert.ToInt32(VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim())) / 10) * 10 + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '1'";
                                    SQL = SQL + ComNum.VBLF + "     )";
                                }
                                else
                                {
                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + VB.Fix(Convert.ToInt32(VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim())) / 10) * 10 + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                                }

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                        }

                        dt.Dispose();
                        dt = null;

                        #endregion
                    }
                    else
                    {
                        #region GoSub Tewon_PANO_Mir_SAN2 산재

                        progressBar1.Value = 0;

                        //EDI 청구분
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, SUM(a.EdiTAmt) AS EDITAMT ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID a, " + ComNum.DB_PMPA + "EDI_SANJEPSU b ";
                        SQL = SQL + ComNum.VBLF + "     WHERE a.IpdOpd = 'I' ";
                        SQL = SQL + ComNum.VBLF + "         AND a.YYMM = '" + GstrYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND a.EdiMirNo > '0' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.GBNEDI = '0' "; //원청구
                        SQL = SQL + ComNum.VBLF + "         AND a.EdiMirNo = b.MirNo(+) ";
                        SQL = SQL + ComNum.VBLF + "         AND b.Week <> '7' "; //중간청구는 제외
                        SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, A.SNAME";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            progressBar1.Maximum = dt.Rows.Count - 1;

                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                progressBar1.Value = i;

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     ROWID";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '1'";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count == 0)
                                {
                                    SQL = "";
                                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                    SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                    SQL = SQL + ComNum.VBLF + "VALUES";
                                    SQL = SQL + ComNum.VBLF + "     (";
                                    SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["PANO"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '31', 'I', '3', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["EDITAMT"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '1'";
                                    SQL = SQL + ComNum.VBLF + "     )";
                                }
                                else
                                {
                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dt.Rows[i]["EDITAMT"].ToString().Trim() + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                                }

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                        }

                        dt.Dispose();
                        dt = null;

                        progressBar1.Value = 0;

                        //서면청구분
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, SUM(A.JAmt) AS EDITAMT ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID a ";
                        SQL = SQL + ComNum.VBLF + "     WHERE a.IpdOpd = 'I' ";
                        SQL = SQL + ComNum.VBLF + "         AND a.YYMM = '" + GstrYYMM + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND a.EdiGbn = '3' "; //2003년8월 이전
                        SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, A.SNAME";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            progressBar1.Maximum = dt.Rows.Count - 1;

                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                progressBar1.Value = i;

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     ROWID";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '1'";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count == 0)
                                {
                                    SQL = "";
                                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                    SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                    SQL = SQL + ComNum.VBLF + "VALUES";
                                    SQL = SQL + ComNum.VBLF + "     (";
                                    SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["PANO"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '31', 'I', '3', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + VB.Fix(Convert.ToInt32(VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim())) / 10) * 10 + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '1' ";
                                    SQL = SQL + ComNum.VBLF + "     )";
                                }
                                else
                                {
                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + VB.Fix(Convert.ToInt32(VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim())) / 10) * 10 + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                                }

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                        }

                        dt.Dispose();
                        dt = null;

                        #endregion
                    }
                }

                if (GstrJong == "0" || GstrJong == "4")
                {
                    #region GoSub Tewon_PANO_Mir_TA2 자보

                    progressBar1.Value = 0;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.MISUID, B.SNAME, SUM(A.AMT2) AS AMT2 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'I'";
                    SQL = SQL + ComNum.VBLF + "         AND A.CLASS = '07' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.AMT2 <> '0' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.TONGGBN = '1' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.MISUID = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.MISUID, B.SNAME";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["MISUID"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '52' ";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '1'";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["MISUID"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '52', 'I', '4', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["AMT2"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '1' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dt.Rows[i]["AMT2"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                }
                
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool Junggan_Mir_Check(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";
            //string strJong = "";
            string strPano = "";
            string strSname = "";
            string strBi = "";
            string strSuBi = "";
            string strInDate = "";
            string strStartDate = "";
            string strOutDate = "";
            string strDeptCode = "";
            string strROWID = "";
            //string strBohoNP = "";
            string strSlipMirDate = "";
            string strJepDate = "";
            string strJepno = "";
            string strBonRate = "";
            string strRateGasan = "";
            string strMisuFrom = "";
            string strMisuTo = "";
            
            double dblTotAmt = 0;
            double dblSlipJAmt = 0;
            double dblJepTAmt = 0;
            double dblJepJAmt = 0;
            double dblWRTNO = 0;
            double dblIPDNO = 0;
            double dblTRSNO = 0;
            double dblCTAmt1 = 0;
            double dblCTAmt2 = 0;

            double dblBuildTAmt = 0;    //중간청구 대상 총진료비
            double dblBuildJAmt = 0;    //중간청구 대상 조합부담액
            
            strYYMM = dtpYYMM.Value.ToString("yyyyMM");
            strFDate = dtpYYMM.Value.ToString("yyyy-MM-01");
            strTDate = dtpYYMM.Value.ToString("yyyy-MM-") + DateTime.DaysInMonth(dtpYYMM.Value.Year, dtpYYMM.Value.Month).ToString("00");

            //보호NP등 서면청구 접수일자를 작업월의 익월1일로 SET
            strSlipMirDate = Convert.ToDateTime(strTDate).AddDays(1).ToString("yyyy-MM-dd");
            
            try
            {
                #region Call Junggan_Subi_Update(strYYMM) 'SuBi NULL UPDATE

                //통계 환자종류 오류자 SELECT
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     ROWID, Bi";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_IPDID ";
                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + strYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "         AND Flag = '1' "; //청구자
                SQL = SQL + ComNum.VBLF + "         AND SuBi IS NULL ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Bi, Pano ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strBi = dt.Rows[i]["BI"].ToString().Trim();
                        strROWID = dt.Rows[i]["ROWID"].ToString().Trim();
                        strSuBi = READ_Bi_SuipTong(VB.Val(strBi), VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01");

                        //자료를 DB에 UPDATE
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "MIR_IPDID";
                        SQL = SQL + ComNum.VBLF + "     SET";
                        SQL = SQL + ComNum.VBLF + "         SuBi = '" + strSuBi + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("MIR_IPDID에 환자종류(SuBi) 변경중 오류발생");
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                //당월분 오류자료만 SELECT
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     YYMM, Pano, Bi, SuBi, SName, DeptCode, TO_CHAR(Indate,'YYYY-MM-DD') AS InDate,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(StartDate,'YYYY-MM-DD') AS StartDate,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(EndDate,'YYYY-MM-DD') AS EndDate,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(JepDate,'YYYY-MM-DD') AS JepDate,";
                SQL = SQL + ComNum.VBLF + "     BuildTAmt, BuildJAmt, JepNo, JepTamt, JepJAmt, WRTNO, ROWID, AO220, ";
                SQL = SQL + ComNum.VBLF + "     IPDNO, TRSNO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_IPDID ";
                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "         AND Flag = '1' "; //청구자

                //보험
                if (GstrJong == "1")
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBI = '1' ";
                }

                //보호
                if (GstrJong == "2")
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBI = '2' ";
                }

                //산재
                if (GstrJong == "3")
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBI = '3' ";

                    if (GstrYYMM == "201310")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND JEPDATE IS NULL ";
                    }
                }

                //자보
                if (GstrJong == "4")
                {
                    SQL = SQL + ComNum.VBLF + "         AND SUBI = '4' ";

                    if (GstrYYMM == "201310")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND JEPDATE IS NULL ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY SuBi, Pano ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i;

                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strBi = dt.Rows[i]["BI"].ToString().Trim();
                        strSname = dt.Rows[i]["SNAME"].ToString().Trim();
                        strSuBi = READ_Bi_SuipTong(VB.Val(dt.Rows[i]["BI"].ToString().Trim()), VB.Left(GstrYYMM, 4) + "-" + VB.Right(GstrYYMM, 2) + "-01");
                        strDeptCode = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        strInDate = dt.Rows[i]["INDATE"].ToString().Trim();
                        strStartDate = dt.Rows[i]["STARTDATE"].ToString().Trim();
                        strOutDate = dt.Rows[i]["ENDDATE"].ToString().Trim();
                        strJepDate = dt.Rows[i]["JEPDATE"].ToString().Trim();

                        dblWRTNO = VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim());
                        dblTotAmt = VB.Val(dt.Rows[i]["BUILDTAMT"].ToString().Trim());      //중간청구대상 총진료비
                        dblSlipJAmt = VB.Val(dt.Rows[i]["BUILDJAMT"].ToString().Trim());    //중간청구대상 조합부담액
                        dblJepTAmt = VB.Val(dt.Rows[i]["JEPTAMT"].ToString().Trim());       //중간청구 실청구 조합부담액
                        dblJepJAmt = VB.Val(dt.Rows[i]["JEPJAMT"].ToString().Trim());       //중간청구 실청구 조합부담액
                        strJepno = dt.Rows[i]["JEPNO"].ToString().Trim();                   //중간청구 EDI 접수번호
                        strROWID = dt.Rows[i]["ROWID"].ToString().Trim();
                        dblIPDNO = VB.Val(dt.Rows[i]["IPDNO"].ToString().Trim());           //2005년08월부터 사용
                        dblTRSNO = VB.Val(dt.Rows[i]["TRSNO"].ToString().Trim());           //2005년08월부터 사용

                        //중간청구 Build시 조합부담액이 누락되었으면
                        if (dblSlipJAmt == 0)
                        {
                            #region Call Junggan_Amt_Gesan(strPano, strBi, strStartDate, strOutDate, strYYMM)

                            dblBuildJAmt = 0;
                            dblBuildTAmt = 0;

                            dblCTAmt1 = 0;
                            dblCTAmt2 = 0;

                            strBonRate = CF.READ_BonRate(pDbCon, "IPD", strBi, strOutDate, strYYMM);
                            strRateGasan = CF.READ_RateGasan(pDbCon, strBi, strOutDate).ToString();

                            //재원처방에서 중간청구액을 READ
                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     SUM(Amt1) AS Amt, SUM(DECODE(Nu,'19',Amt1,0)) AS CTAmt";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                            SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND Bi = '" + strBi + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BDate >= TO_DATE('" + strStartDate + "', 'YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "         AND BDate <= TO_DATE('" + strOutDate + "', 'YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "         AND YYMM = '" + VB.Right(strYYMM, 4) + "' ";

                            if (strBi == "52") //자보
                            {
                                SQL = SQL + ComNum.VBLF + "         AND (Nu < '21' OR (Nu > '35' AND Nu < '41')) ";
                            }
                            else                 //산재,보험,보호
                            {
                                SQL = SQL + ComNum.VBLF + "         AND  GbSelf = '0'  ";
                                SQL = SQL + ComNum.VBLF + "         AND  Nu     < '21' ";
                            }

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                dblBuildTAmt = VB.Val(dt1.Rows[0]["AMT"].ToString().Trim());

                                //CT금액을 SET
                                if (VB.Val(strBi) <= 20)
                                {
                                    if (Convert.ToDateTime(strOutDate) < Convert.ToDateTime("2001-07-01"))
                                    {
                                        dblCTAmt1 = VB.Val(dt1.Rows[0]["CTAMT"].ToString().Trim());
                                    }
                                    else
                                    {
                                        dblCTAmt2 = VB.Val(dt1.Rows[0]["CTAMT"].ToString().Trim());
                                    }
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;

                            if (dblBuildTAmt != 0)
                            {
                                #region GoSub Junggan_Amt_Gesan_SUB 총진료비,청구액 계산

                                //중간청구액을 계산
                                if (strBi == "31" || strBi == "52" || strBi == "21")
                                {
                                    dblBuildJAmt = dblBuildTAmt;
                                }
                                //보험
                                else if (VB.Val(strBi) <= 20)
                                {
                                    dblBuildJAmt = VB.Fix((int)((dblBuildTAmt - dblCTAmt1 - dblCTAmt2) * (100 - VB.Val(strBonRate)) / 100));
                                    dblBuildJAmt = dblBuildJAmt + (dblCTAmt1 * 0.45);
                                    dblBuildJAmt = dblBuildJAmt + (dblCTAmt2 * 0.55);
                                }
                                else
                                {
                                    dblBuildJAmt = VB.Fix((int)(dblBuildTAmt * (100 - VB.Val(strBonRate)) / 100));
                                }

                                #endregion
                            }
                            else
                            {
                                //퇴원처방에서 중간청구액을 READ
                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     SUM(Amt1) AS Amt,SUM(DECODE(Nu,'19',Amt1,0)) AS CTAmt ";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND Bi = '" + strBi + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND BDate >= TO_DATE('" + strStartDate + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "         AND BDate <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "         AND YYMM = '" + VB.Right(strYYMM, 4) + "' ";

                                if (strBi == "52")  //자보
                                {
                                    SQL = SQL + ComNum.VBLF + "         AND (Nu < '21' OR (Nu > '35' AND Nu < '41')) ";
                                }
                                else                //산재,보험,보호
                                {
                                    SQL = SQL + ComNum.VBLF + "         AND GbSelf = '0'  ";
                                    SQL = SQL + ComNum.VBLF + "         AND Nu     < '21' ";
                                }

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    dblBuildTAmt = VB.Val(dt1.Rows[0]["AMT"].ToString().Trim());

                                    //CT금액을 SET
                                    if (VB.Val(strBi) <= 20)
                                    {
                                        if (Convert.ToDateTime(strOutDate) < Convert.ToDateTime("2001-07-01"))
                                        {
                                            dblCTAmt1 = VB.Val(dt1.Rows[0]["CTAMT"].ToString().Trim());
                                        }
                                        else
                                        {
                                            dblCTAmt2 = VB.Val(dt1.Rows[0]["CTAMT"].ToString().Trim());
                                        }
                                    }
                                }

                                dt1.Dispose();
                                dt1 = null;

                                if (dblBuildTAmt != 0)
                                {
                                    #region GoSub Junggan_Amt_Gesan_SUB 총진료비,청구액 계산

                                    //중간청구액을 계산
                                    if (strBi == "31" || strBi == "52" || strBi == "21")
                                    {
                                        dblBuildJAmt = dblBuildTAmt;
                                    }
                                    //보험
                                    else if (VB.Val(strBi) <= 20)
                                    {
                                        dblBuildJAmt = VB.Fix((int)((dblBuildTAmt - dblCTAmt1 - dblCTAmt2) * (100 - VB.Val(strBonRate)) / 100));
                                        dblBuildJAmt = dblBuildJAmt + (dblCTAmt1 * 0.45);
                                        dblBuildJAmt = dblBuildJAmt + (dblCTAmt2 * 0.55);
                                    }
                                    else
                                    {
                                        dblBuildJAmt = VB.Fix((int)(dblBuildTAmt * (100 - VB.Val(strBonRate)) / 100));
                                    }

                                    #endregion
                                }
                            }

                            #endregion

                            dblTotAmt = dblBuildJAmt;
                            dblSlipJAmt = dblBuildJAmt;
                        }

                        if (strSuBi == "1" || strSuBi == "2")
                        {
                            #region GoSub Junggan_Mir_Bohum 보험,보호 실청구액을 READ

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     a.WRTNO, TO_CHAR(a.OutDate,'YYYY-MM-DD') AS OutDate, a.JinDate1, a.EdiJAmt, a.JAmt, a.EdiTAmt, a.TAmt, a.EdiBoamt, a.EdiGamt, a.EdiUamt100, ";
                            SQL = SQL + ComNum.VBLF + "     b.JepNo, TO_CHAR(b.JepDate,'YYYY-MM-DD') AS JepDate ";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID a, " + ComNum.DB_PMPA + "EDI_JEPSU b ";
                            SQL = SQL + ComNum.VBLF + "     WHERE a.Pano = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND a.IpdOpd = 'I' ";
                            SQL = SQL + ComNum.VBLF + "         AND a.Bi = '" + strBi + "' "; //환자종류가 같은것
                            SQL = SQL + ComNum.VBLF + "         AND a.YYMM = '" + strYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND a.EdiMirNo > '0' "; //EDI청구분
                            SQL = SQL + ComNum.VBLF + "         AND a.EdiMirNo = b.MirNo(+) ";
                            SQL = SQL + ComNum.VBLF + "         AND b.Week = '7' "; //중간청구만
                            SQL = SQL + ComNum.VBLF + "ORDER BY a.JinDate1 DESC ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 1)
                            {
                                //청구내역을 UPDATE
                                dblWRTNO = VB.Val(dt1.Rows[0]["WRTNO"].ToString().Trim());
                                strJepDate = dt1.Rows[0]["JEPDATE"].ToString().Trim();
                                strJepno = dt1.Rows[0]["JEPNO"].ToString().Trim();
                                dblJepTAmt = VB.Val(dt1.Rows[0]["EDITAMT"].ToString().Trim());
                                dblJepJAmt = VB.Val(dt1.Rows[0]["EDIJAMT"].ToString().Trim()) + VB.Val(dt1.Rows[0]["EDIBOAMT"].ToString().Trim()) + VB.Val(dt1.Rows[0]["EDIGAMT"].ToString().Trim()) + VB.Val(dt1.Rows[0]["EDIUAMT100"].ToString().Trim());
                            }

                            dt1.Dispose();
                            dt1 = null;

                            #endregion
                        }
                        else if (strSuBi == "3")
                        {
                            #region GoSub Junggan_Mir_SAN 산재 실청구액을 READ

                            dblWRTNO = 0;
                            strJepDate = "";
                            strJepno = "";
                            dblJepTAmt = 0;
                            dblJepJAmt = 0;

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     WRTNO, a.EdiTAmt, b.JepNo, TO_CHAR(b.JepDate,'YYYY-MM-DD') AS JepDate ";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID a, " + ComNum.DB_PMPA + "EDI_SANJEPSU b ";
                            SQL = SQL + ComNum.VBLF + "     WHERE a.Pano = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND a.IpdOpd = 'I' ";
                            SQL = SQL + ComNum.VBLF + "         AND a.YYMM = '" + strYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND a.EdiMirNo > '0' "; //EDI청구분
                            SQL = SQL + ComNum.VBLF + "         AND A.GBNEDI = '0' "; //원청구
                            SQL = SQL + ComNum.VBLF + "         AND a.EdiMirNo = b.MirNo ";
                            SQL = SQL + ComNum.VBLF + "         AND b.Week = '7' "; //중간청구만

                            if (VB.Val(strYYMM) >= 200508)
                            {
                                SQL = SQL + ComNum.VBLF + "         AND A.IPDNO = " + dblIPDNO + " ";
                                SQL = SQL + ComNum.VBLF + "         AND A.TRSNO = " + dblTRSNO + " ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "         AND A.FRDATE = TO_DATE('" + strStartDate + "','YYYY-MM-DD')";
                            }

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 1)
                            {
                                //청구내역을 UPDATE
                                dblWRTNO = VB.Val(dt1.Rows[0]["WRTNO"].ToString().Trim());
                                strJepDate = dt1.Rows[0]["JEPDATE"].ToString().Trim();
                                strJepno = dt1.Rows[0]["JEPNO"].ToString().Trim();
                                dblJepTAmt = VB.Val(dt1.Rows[0]["EDITAMT"].ToString().Trim());
                                dblJepJAmt = VB.Val(dt1.Rows[0]["EDITAMT"].ToString().Trim());
                            }

                            dt1.Dispose();
                            dt1 = null;

                            #endregion
                        }
                        else if (strSuBi == "4")
                        {
                            #region GoSub Junggan_Mir_TA 자보 실청구액을 READ

                            strJepDate = "";
                            strJepno = "";
                            dblJepJAmt = 0;
                            dblJepJAmt = 0;
                            strMisuFrom = "";
                            strMisuTo = "";

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     Amt2, TO_CHAR(BDate,'YYYY-MM-DD') AS BDate,";
                            SQL = SQL + ComNum.VBLF + "     TO_CHAR(FromDate,'YYYY-MM-DD') AS FromDate,";
                            SQL = SQL + ComNum.VBLF + "     TO_CHAR(ToDate,'YYYY-MM-DD') AS ToDate ";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                            SQL = SQL + ComNum.VBLF + "     WHERE MisuID = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND IpdOpd = 'I' ";
                            SQL = SQL + ComNum.VBLF + "         AND MirYYMM = '" + strYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND Class = '07' ";
                            SQL = SQL + ComNum.VBLF + "         AND TONGGBN = '2'"; //중간청구
                            SQL = SQL + ComNum.VBLF + "         AND (FROMDATE = TO_DATE('" + strStartDate + "','YYYY-MM-DD') OR JDATE = TO_DATE('" + strStartDate + "','YYYY-MM-DD'))";

                            if (VB.Val(strYYMM) == 200310)
                            {
                                SQL = SQL + ComNum.VBLF + "         AND FROMDATE >= TO_DATE('2003-10-08','YYYY-MM-DD')";
                            }

                            SQL = SQL + ComNum.VBLF + "ORDER BY BDate ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 1)
                            {
                                strJepDate = dt1.Rows[0]["BDATE"].ToString().Trim();
                                strJepno = "";
                                dblJepTAmt = VB.Val(dt1.Rows[0]["AMT2"].ToString().Trim());
                                dblJepJAmt = VB.Val(dt1.Rows[0]["AMT2"].ToString().Trim());
                                strMisuFrom = dt1.Rows[0]["FROMDATE"].ToString().Trim();
                                strMisuTo = dt1.Rows[0]["TODATE"].ToString().Trim();

                                dt1.Dispose();
                                dt1 = null;

                                dblWRTNO = 0;

                                //청구번호를 READ
                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     WRTNO";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_TAID ";
                                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND IpdOpd = 'I' ";
                                SQL = SQL + ComNum.VBLF + "         AND YYMM = '" + strYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND UpCnt1 <> '9' "; //보류자 제외

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count == 1)
                                {
                                    dblWRTNO = VB.Val(dt1.Rows[0]["WRTNO"].ToString().Trim());
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;

                            #endregion
                        }

                        if (GstrJong == "2" && strPano == "07715077" && strYYMM == "201102") { } //재형성 않되도록 처리
                        else
                        {
                            //자료를 DB에 UPDATE
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "MIR_IPDID";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         WRTNO = " + dblWRTNO + ", ";
                            SQL = SQL + ComNum.VBLF + "         JepNo = '" + strJepno + "', ";
                            SQL = SQL + ComNum.VBLF + "         BuildTAmt = " + dblTotAmt + ", ";
                            SQL = SQL + ComNum.VBLF + "         BuildJAmt = " + dblSlipJAmt + ", ";
                            SQL = SQL + ComNum.VBLF + "         JepTAmt = " + dblJepTAmt + ", ";
                            SQL = SQL + ComNum.VBLF + "         JepJAmt = " + dblJepJAmt + ", ";
                            SQL = SQL + ComNum.VBLF + "         JepDate = TO_DATE('" + strJepDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("MIR_IPDID에 중간청구 자료를 변경중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string READ_Bi_SuipTong(double dblBi, string strJodDate)
        {
            string rtnVal = "0";

            if (Convert.ToDateTime(strJodDate) >= Convert.ToDateTime("2003-11-03"))
            {
                if ((dblBi >= 11 && dblBi <= 13) || dblBi == 32 || (dblBi >= 41 && dblBi <= 44))
                {
                    rtnVal = "1";   //보험
                }
                else if (dblBi >= 21 && dblBi <= 24)
                {
                    rtnVal = "2";   //보호
                }
                else if (dblBi == 31 || dblBi == 33)
                {
                    rtnVal = "3";   //산재
                }
                else if (dblBi == 52)
                {
                    rtnVal = "4";   //자보
                }
                else
                {
                    rtnVal = "5";   //일반
                }
            }
            else
            {
                if ((dblBi >= 11 && dblBi <= 13) || (dblBi >= 41 && dblBi <= 44))
                {
                    rtnVal = "1";   //보험
                }
                else if (dblBi >= 21 && dblBi <= 24)
                {
                    rtnVal = "2";   //보호
                }
                else if (dblBi >= 31 && dblBi <= 33)
                {
                    rtnVal = "3";   //산재
                }
                else if (dblBi == 52)
                {
                    rtnVal = "4";   //자보
                }
                else
                {
                    rtnVal = "5";   //일반
                }
            }

            return rtnVal;
        }

        private bool OPD_MIR_CHECK_201410(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strTime1 = "";
            string strTime2 = "";
            string strPano = "";
            string strRemark = "";

            double dblTAmt = 0;
            double dblJAmt = 0;

            if ((GstrJong == "1" || GstrJong == "2") && GstrYYMM == "201402") { return rtnVal; }

            //04811623 4868 OPD_SLIP과 마감 금액 차이로 강제 MISU_BALCHECK_PANO 테이블 조정
            if ((GstrJong == "1" || GstrJong == "2") && GstrYYMM == "201409") { return rtnVal; }

            //04811623 4868 OPD_SLIP과 마감 금액 차이로 강제 MISU_BALCHECK_PANO 테이블 조정
            if ((GstrJong == "1" || GstrJong == "2") && GstrYYMM == "201502") { return rtnVal; }

            //08224396    박덕희 05958414    최석달 misu_baltewon 중복 마감 금액 차이로 강제 MISU_BALCHECK_PANO 테이블 조정
            if ((GstrJong == "1" || GstrJong == "2") && GstrYYMM == "201503") { return rtnVal; }

            //2015-04-21  02688173    박춘자  21  2   2015-04-20  2015-04-20  1   ER  251050    최석달 misu_baltewon 중복 마감 금액 차이로 강제 MISU_BALCHECK_PANO 테이블 조정
            if ((GstrJong == "1" || GstrJong == "2") && GstrYYMM == "201504") { return rtnVal; }

            //Select * from MISU_BALCHECK_PANO  where PANO='06417366'
            //SELECT * FROM ADMIN.MISU_IDMST WHERE MISUID = '06417366'
            //SELECT * FROM ADMIN.MIR_SANID WHERE WRTNO IN ( '6024183','6024182')
            //청구건 중복인식 문제로 재빌드 안되게 막음 MISU_BALCHECK_PANO 조정
            if (GstrJong == "3" && GstrYYMM == "201612") { return rtnVal; }

            //SELECT A.*, A.ROWID FROM ADMIN.MISU_BALCHECK_PANO A
            //WHERE PANO = '09982178'
            //AND YYMM = '201705'
            //청구건 중복인식 문제로 재빌드 안되게 막음 MISU_BALCHECK_PANO 조정
            if (GstrJong == "3" && GstrYYMM == "201705") { return rtnVal; }

            //OPD_SLIP과 마감 금액 차이로 강제 MISU_BALCHECK_PANO 테이블 조정
            if ((GstrJong == "1" || GstrJong == "2") && GstrYYMM == "201702") { return rtnVal; }

            lblMsg.Text = "★ 외래 실청구액 CHECK ★";

            strTime1 = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":");

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                SQL = SQL + ComNum.VBLF + "     SET";
                SQL = SQL + ComNum.VBLF + "         TOTAMT = 0, ";
                SQL = SQL + ComNum.VBLF + "         JUNGGAN = 0, ";
                SQL = SQL + ComNum.VBLF + "         JOHAP = 0, ";
                SQL = SQL + ComNum.VBLF + "         HALIN = 0, ";
                SQL = SQL + ComNum.VBLF + "         BOJUNG = 0, ";
                SQL = SQL + ComNum.VBLF + "         ETCMISU = 0, ";
                SQL = SQL + ComNum.VBLF + "         SUNAP = 0, ";
                SQL = SQL + ComNum.VBLF + "         DANSU = 0, ";
                SQL = SQL + ComNum.VBLF + "         JEPJAMT = 0";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM = '" + GstrYYMM + "'";
                SQL = SQL + ComNum.VBLF + "     AND Gubun = '4' ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "     AND SUBI ='1' "; } //보험
                else if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "     AND SUBI ='2' "; } //보호
                else if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "     AND SUBI ='3' "; } //산재
                else if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "     AND SUBI ='4' "; } //자보

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                #region GoSub OPD_DATA_RTN_OLD

                progressBar1.Value = 0;

                //201201 부터 외래 건강보험/의료급여 일자별 청구 개시
                if (GstrJong == "1" || GstrJong == "2")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PANO, B.SNAME, A.BI, ";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '96', Amt1 + Amt2, 0)) AS Bun96,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '98', Amt1 + Amt2, 0)) AS Bun98,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '99', Amt1 + Amt2, 0)) AS Bun99 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.ACTDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.ACTDATE <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.GBSLIP NOT IN ('Z','Q','E') OR A.GBSLIP IS NULL)";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('92', '96', '98', '99')";

                    if (GstrJong == "1")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.BI IN ('11', '12', '13', '51', '32', '42', '43')"; //允2005-11-21(43종 조합부담금 발생건수발생(2005/03/07)
                    }

                    if (GstrJong == "2")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.BI IN ('20', '21', '22', '23', '24') "; //보호
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.BI ";
                }
                else if (GstrJong == "3" || GstrJong == "4")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     '' AS BDATE, A.PANO, B.SNAME, A.BI, '' AS DEPTCODE, ";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '96', Amt1 + Amt2, 0)) AS Bun96,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '98', Amt1 + Amt2, 0)) AS Bun98,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '99', Amt1 + Amt2, 0)) AS Bun99 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.ACTDATE >= TO_DATE('" + GstrFDate + "', 'YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "         AND A.ACTDATE <= TO_DATE('" + GstrTDate + "', 'YYYY-MM-DD')";

                    if ((GstrJong == "4" || GstrJong == "3") && (GstrFDate == "2015-02-01" || GstrFDate == "2015-10-01" || Convert.ToDateTime(GstrFDate) >= Convert.ToDateTime("2016-01-01")))
                    {
                        //SQL = SQL + ComNum.VBLF + "         AND (A.GBSLIP NOT IN ('Z','Q','E') OR A.GBSLIP IS NULL)" '2월자보만 제외 응급실 6시간 퇴원미수에 누적 않되었음.
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         AND (A.GBSLIP NOT IN ('Z','Q','E') OR A.GBSLIP IS NULL) ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('92', '96', '98', '99')";

                    if (GstrJong == "3")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.BI in ('31', '33') "; //산재-임시 33종 오류용
                    }

                    if (GstrJong == "4")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.BI ='52' "; //자보
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.BI ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i;

                        if (VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) != 0)
                        {
                            dblTAmt = VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim());

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, TOTAMT, JOHAP, HALIN, SUNAP, JEPJAMT, GUBUN, BDATE, DEPTCODE)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["Pano"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SName"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["Bi"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";

                                switch (dt.Rows[i]["Bi"].ToString().Trim())
                                {
                                    case "20":
                                    case "21":
                                    case "22":
                                    case "23":
                                    case "24":
                                        SQL = SQL + ComNum.VBLF + "         '2', "; //보호
                                        break;
                                    case "31":
                                        SQL = SQL + ComNum.VBLF + "         '3', "; //산재
                                        break;
                                    case "52":
                                        SQL = SQL + ComNum.VBLF + "         '4', "; //자보
                                        break;
                                    default:
                                        SQL = SQL + ComNum.VBLF + "         '1', "; //보험
                                        break;
                                }

                                SQL = SQL + ComNum.VBLF + "         '" + dblTAmt + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '0', ";
                                SQL = SQL + ComNum.VBLF + "         '4', ";
                                SQL = SQL + ComNum.VBLF + "         '', ";
                                SQL = SQL + ComNum.VBLF + "         '' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         TOTAMT = '" + dblTAmt + "', ";
                                SQL = SQL + ComNum.VBLF + "         JOHAP = '" + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         HALIN = '" + VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + "', ";

                                switch (dt.Rows[i]["Bi"].ToString().Trim())
                                {
                                    case "20":
                                    case "21":
                                    case "22":
                                    case "23":
                                    case "24":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '2', ";  //보호
                                        break;
                                    case "31":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '3', ";   //산재
                                        break;
                                    case "52":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '4', ";  //자보
                                        break;
                                    default:
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '1', ";  //보험
                                        break;
                                }

                                SQL = SQL + ComNum.VBLF + "         SUNAP = '" + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim()) + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                if (GstrJong == "0" || GstrJong == "1" || GstrJong == "2")
                {
                    #region GoSub OPD_MIR_INSID

                    progressBar1.Value = 0;

                    //청구내역
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PANO, B.SNAME, A.BI, A.JOHAP, SUM(A.EDITAMT) AS EDITAMT , ";
                    SQL = SQL + ComNum.VBLF + "     SUM(A.EDIJAMT + A.EDIBOAMT + A.EDIGAMT + nvl(a.EDIUAMT100,0)) AS EDIJAMT, SUM(A.EDIBAMT) AS EDIBAMT ";  //2016-06-03
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.EDIMIRNO IN";
                    SQL = SQL + ComNum.VBLF + "                 (SELECT";
                    SQL = SQL + ComNum.VBLF + "                     B.MIRNO ";
                    SQL = SQL + ComNum.VBLF + "                 FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "EDI_JEPSU B";
                    SQL = SQL + ComNum.VBLF + "                     WHERE A.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "                         AND A.TONGGBN IN ('1', '2')";
                    SQL = SQL + ComNum.VBLF + "                         AND A.IpdOpd = 'O' ";

                    if (GstrJong == "1")
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND A.CLASS IN ('01', '02', '03')"; //보험
                    }
                    else if (GstrJong == "2")
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND A.CLASS = '04' ";              //보호
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND A.CLASS <= '04'  ";          //보험, 보호
                    }

                    SQL = SQL + ComNum.VBLF + "                         AND A.MISUID = B.JEPNO";

                    if (GstrYYMM != "201104")
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND SUBSTR(B.YYMM, 1, 4) = '" + VB.Left(GstrYYMM, 4) + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "                 GROUP BY B.MIRNO)";
                    SQL = SQL + ComNum.VBLF + "         AND A.EdiMirNo > '0' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+)";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.BI, A.JOHAP";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";
                            SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN, BDATE, DEPTCODE, WRTNO)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["Pano"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SName"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["Bi"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";

                                if (dt.Rows[i]["JOHAP"].ToString().Trim() == "5")   //보호
                                {
                                    SQL = SQL + ComNum.VBLF + "         '2', ";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "         '1', ";
                                }

                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["EDIJAMT"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '4', ";
                                SQL = SQL + ComNum.VBLF + "         '', ";
                                SQL = SQL + ComNum.VBLF + "         '', ";
                                SQL = SQL + ComNum.VBLF + "         '' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + VB.Val(dt.Rows[i]["EDIJAMT"].ToString().Trim()) + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    OPD_MIR_CHECK_REMARK(pDbCon);

                    #endregion
                }

                if (GstrJong == "0" || GstrJong == "3")
                {
                    #region GoSub OPD_MIR_SANID

                    progressBar1.Value = 0;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     PANO, SNAME, EDIGBN, SUM(EDITAMT) AS EDITAMT, SUM(JAMT) AS JAMT ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID";
                    SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                    SQL = SQL + ComNum.VBLF + "         AND UPCNT1 <> '9' ";
                    SQL = SQL + ComNum.VBLF + "         AND GBNEDI = '0' "; //원청구 允(2005-11-23) 재설정
                    SQL = SQL + ComNum.VBLF + "         AND (MIRGBN = '1' OR MIRGBN IS NULL)";
                    SQL = SQL + ComNum.VBLF + "GROUP BY PANO, SNAME, EDIGBN ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        rtnVal = true;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strPano = dt.Rows[0]["PANO"].ToString().Trim(); //변수 설정

                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            if(strPano != dt.Rows[i]["PANO"].ToString().Trim())
                            {
                                #region GoSub OPD_MIR_SANID_INSERT

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     ROWID";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count == 0)
                                {
                                    SQL = "";
                                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                    SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                    SQL = SQL + ComNum.VBLF + "VALUES";
                                    SQL = SQL + ComNum.VBLF + "     (";
                                    SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "',";
                                    SQL = SQL + ComNum.VBLF + "         '31', 'O', '3', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dblJAmt + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '4' ";
                                    SQL = SQL + ComNum.VBLF + "     )";
                                }
                                else
                                {
                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dblJAmt + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                                }

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }

                                dt1.Dispose();
                                dt1 = null;

                                #endregion

                                dblJAmt = 0;
                                strPano = dt.Rows[i]["PANO"].ToString().Trim();
                            }

                            if (dt.Rows[i]["EdiGbn"].ToString().Trim() == "3")
                            {
                                dblJAmt = dblJAmt + VB.Fix((int)(VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()) / 10)) * 10;
                            }
                            else
                            {
                                if (VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim()) == 0)
                                {
                                    dblJAmt = dblJAmt + VB.Fix((int)(VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()) / 10)) * 10;
                                }
                                else
                                {
                                    dblJAmt = dblJAmt + VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim());
                                }
                            }
                        }
                    }

                    #region GoSub OPD_MIR_SANID_INSERT

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     ROWID";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                    SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                    SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt1.Rows.Count == 0)
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                        SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "',";
                        SQL = SQL + ComNum.VBLF + "         '31', 'O', '3', ";
                        SQL = SQL + ComNum.VBLF + "         '" + dblJAmt + "', ";
                        SQL = SQL + ComNum.VBLF + "         '4' ";
                        SQL = SQL + ComNum.VBLF + "     )";
                    }
                    else
                    {
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                        SQL = SQL + ComNum.VBLF + "     SET";
                        SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dblJAmt + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }

                    dt1.Dispose();
                    dt1 = null;

                    #endregion

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                if (GstrJong == "0" || GstrJong == "4")
                {
                    #region GoSub OPD_MIR_TAID

                    progressBar1.Value = 0;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.MISUID, B.SNAME, SUM(A.AMT2) AS AMT2 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND A.CLASS = '07' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.AMT2 <> '0' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.TONGGBN = '1' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.MISUID = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.MISUID, B.SNAME  ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["MISUID"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '52' ";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["MISUID"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '52', 'O', '4', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["AMT2"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '4' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dt.Rows[i]["AMT2"].ToString().Trim() + "' "; 
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                }


                #region GoSub MISU_BALCHECK_PANO_REMARK MISU_BALCHECK_PANO 청구금액 0 인경우 REMARK사항 표시

                progressBar1.Value = 0;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, BI, ROWID, TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '1' "; }  //보험
                if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '2' "; }  //보호
                if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '3' "; }  //산재
                if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '4' "; }  //자보

                SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                SQL = SQL + ComNum.VBLF + "         AND REMARK IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i;

                        strRemark = "";

                        //참고내역read
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     WRTNO, REMARK";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSDTL ";
                        SQL = SQL + ComNum.VBLF + "     WHERE WRTNO IN";
                        SQL = SQL + ComNum.VBLF + "             (SELECT";
                        SQL = SQL + ComNum.VBLF + "                 WRTNO";
                        SQL = SQL + ComNum.VBLF + "             FROM " + ComNum.DB_PMPA + "MIR_INSID ";
                        SQL = SQL + ComNum.VBLF + "                 WHERE (BYYMM = '" + GstrYYMM + "' OR YYMM = '" + GstrYYMM + "')"; //2008/10/14 심사과 김준수샘 요청
                        SQL = SQL + ComNum.VBLF + "                     AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "                     AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "                     AND IPDOPD = 'O' ";

                        if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "                     AND JOHAP <> '5' "; }
                        if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "                     AND JOHAP = '5' "; }

                        SQL = SQL + ComNum.VBLF + "                     AND UPCNT1 = '9')";
                        SQL = SQL + ComNum.VBLF + "         AND SUNEXT <> '########' ";
                        SQL = SQL + ComNum.VBLF + "         AND REMARK IS NOT NULL";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["REMARK"].ToString().Trim() != "")
                            {
                                strRemark = dt1.Rows[0]["REMARK"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        //업무의뢰서 요청으로 2006-08-28 尹
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE, 'YYYY-MM-DD') AS BDATE, REMARK";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_HOANBUL ";
                        SQL = SQL + ComNum.VBLF + "     WHERE BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "', 'YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["REMARK"].ToString().Trim() != "")
                            {
                                strRemark = strRemark + dt1.Rows[0]["BDATE"].ToString().Trim() + ":" + dt1.Rows[0]["REMARK"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.ORDERCODE AS ORDERCODE, TO_CHAR(A.BDATE, 'YYYY-MM-DD') AS BDATE, B.ORDERNAMES AS ORDERNAMES";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_MED + "OCS_ORDERCODE B ";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "', 'YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.PTNO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "         AND A.BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE LIKE '$$%' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE = B.ORDERCODE ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["ORDERNAMES"].ToString().Trim() != "")
                            {
                                strRemark = strRemark + dt1.Rows[0]["BDATE"].ToString().Trim() + ":" + dt1.Rows[0]["ORDERNAMES"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strRemark != "")
                        {
                            //2021-02-23 변환에러로 수정작업
                            if (Encoding.Default.GetBytes(strRemark).Length > 100)
                            {
                                byte[] strByte = Encoding.Default.GetBytes(strRemark);

                                strRemark = Encoding.Default.GetString(strByte, 0, 100);
                            }

                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                strTime2 = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":");

                ComFunc.MsgBox(strTime1 + " : " + strTime2);
                
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool OPD_MIR_CHECK_201404(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            int k = 0;

            string strTime1 = "";
            string strTime2 = "";
            string strPano = "";
            string strBi = "";
            string strRemark = "";

            double dblTAmt = 0;
            double dblJAmt = 0;

            if ((GstrJong == "1" || GstrJong == "2") && GstrYYMM == "201402") { return rtnVal; }

            //04811623 4868 OPD_SLIP과 마감 금액 차이로 강제 MISU_BALCHECK_PANO 테이블 조정
            if ((GstrJong == "1" || GstrJong == "2") && GstrYYMM == "201409") { return rtnVal; }

            lblMsg.Text = "★ 외래 실청구액 CHECK ★";
            
            strTime1 = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":");

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                SQL = SQL + ComNum.VBLF + "     SET";
                SQL = SQL + ComNum.VBLF + "         TOTAMT = 0, ";
                SQL = SQL + ComNum.VBLF + "         JUNGGAN = 0, ";
                SQL = SQL + ComNum.VBLF + "         JOHAP = 0, ";
                SQL = SQL + ComNum.VBLF + "         HALIN = 0, ";
                SQL = SQL + ComNum.VBLF + "         BOJUNG = 0, ";
                SQL = SQL + ComNum.VBLF + "         ETCMISU = 0, ";
                SQL = SQL + ComNum.VBLF + "         SUNAP = 0, ";
                SQL = SQL + ComNum.VBLF + "         DANSU = 0, ";
                SQL = SQL + ComNum.VBLF + "         JEPJAMT = 0";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM ='" + GstrYYMM + "'";
                SQL = SQL + ComNum.VBLF + "     AND Gubun ='4' ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '1' "; } //보험
                else if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '2' "; } //보호
                else if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '3' "; } //산재
                else if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '4' "; } //자보

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                #region GoSub OPD_DATA_RTN_OLD

                progressBar1.Value = 0;

                //201201 부터 외래 건강보험/의료급여 일자별 청구 개시
                if (GstrJong == "1" || GstrJong == "2")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PANO, B.SNAME, A.BI, A.DEPTCODE, ";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '96', Amt1 + Amt2, 0)) AS Bun96, ";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '98', Amt1 + Amt2, 0)) AS Bun98, ";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '99', Amt1 + Amt2, 0)) AS Bun99 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.ACTDATE >= TO_DATE('" + GstrFDate + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.ACTDATE <= TO_DATE('" + GstrTDate + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.GBSLIP NOT IN ('Z','Q','E') OR A.GBSLIP IS NULL)";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('92', '96', '98', '99') ";

                    if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND A.BI IN ('11','12','13','51','32','42','43')"; } //允2005-11-21(43종 조합부담금 발생건수발생(2005/03/07)
                    if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND A.BI IN ('20','21','22','23','24') "; } //보호

                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.BI, A.DEPTCODE  ";
                }
                else if (GstrJong == "3" || GstrJong == "4")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     '' AS BDATE, A.PANO, B.SNAME, A.BI, '' AS DEPTCODE, ";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '96', Amt1 + Amt2, 0)) AS Bun96,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '98', Amt1 + Amt2, 0)) AS Bun98,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '99', Amt1 + Amt2, 0)) AS Bun99 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.ACTDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.ACTDATE <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.GBSLIP NOT IN ('Z','Q','E') OR A.GBSLIP IS NULL)";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('92', '96', '98', '99') ";

                    if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "         AND A.BI in ('31', '33') "; } //산재-임시 33종 오류용
                    if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "         AND A.BI = '52' "; } //자보

                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.BI ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i;

                        if (VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) != 0)
                        {
                            dblTAmt = VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim());

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM='" + GstrYYMM + "'  ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO ='" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI ='" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN ='4'";

                            if (dt.Rows[i]["DEPTCODE"].ToString().Trim() != "")
                            {
                                SQL = SQL + ComNum.VBLF + "         AND DEPTCODE = '" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, TOTAMT, JOHAP, HALIN, SUNAP, JEPJAMT, GUBUN, BDATE, DEPTCODE)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["Pano"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SName"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["Bi"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";

                                switch (dt.Rows[i]["Bi"].ToString().Trim())
                                {
                                    case "20":
                                    case "21":
                                    case "22":
                                    case "23":
                                    case "24":
                                        SQL = SQL + ComNum.VBLF + "         '2', "; //보호
                                        break;
                                    case "31":
                                        SQL = SQL + ComNum.VBLF + "         '3',";             //산재
                                        break;
                                    case "52":
                                        SQL = SQL + ComNum.VBLF + "         '4',";             //자보
                                        break;
                                    default:
                                        SQL = SQL + ComNum.VBLF + "         '1', ";            //보험
                                        break;
                                }

                                SQL = SQL + ComNum.VBLF + "         '" + dblTAmt + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '0', ";
                                SQL = SQL + ComNum.VBLF + "         '4', ";
                                SQL = SQL + ComNum.VBLF + "         '', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         TOTAMT = '" + dblTAmt + "', ";
                                SQL = SQL + ComNum.VBLF + "         JOHAP = '" + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         HALIN = '" + VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + "', ";

                                switch (dt.Rows[i]["Bi"].ToString().Trim())
                                {
                                    case "20":
                                    case "21":
                                    case "22":
                                    case "23":
                                    case "24":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '2', "; //보호
                                        break;
                                    case "31":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '3',";             //산재
                                        break;
                                    case "52":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '4',";             //자보
                                        break;
                                    default:
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '1', ";            //보험
                                        break;
                                }

                                SQL = SQL + ComNum.VBLF + "         SUNAP  = '" + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim()) + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                if (GstrJong == "0" || GstrJong == "1" || GstrJong == "2")
                {
                    #region GoSub OPD_MIR_INSID

                    progressBar1.Value = 0;

                    //청구내역
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PANO, B.SNAME, A.BI, A.JOHAP, SUM(A.EDITAMT) AS EDITAMT, A.DEPTCODE1, ";
                    SQL = SQL + ComNum.VBLF + "     SUM(A.EDIJAMT + A.EDIBOAMT + A.EDIGAMT) AS EDIJAMT, SUM(A.EDIBAMT) AS EDIBAMT ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID A, " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.EDIMIRNO IN";
                    SQL = SQL + ComNum.VBLF + "                 (SELECT";
                    SQL = SQL + ComNum.VBLF + "                     B.MIRNO ";
                    SQL = SQL + ComNum.VBLF + "                 FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "EDI_JEPSU B";
                    SQL = SQL + ComNum.VBLF + "                     WHERE A.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "                         AND A.TONGGBN IN ('1', '2') ";
                    SQL = SQL + ComNum.VBLF + "                         AND A.IpdOpd = 'O'";

                    if (GstrJong == "1")
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND A.CLASS IN ('01', '02', '03') "; //보험
                    }
                    else if (GstrJong == "2")
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND A.CLASS = '04' ";              //보호
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND A.CLASS <= '04' ";          //보험, 보호
                    }

                    SQL = SQL + ComNum.VBLF + "                         AND A.MISUID = B.JEPNO";

                    if (GstrYYMM != "201104")
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND SUBSTR(B.YYMM, 1, 4) = '" + VB.Left(GstrYYMM, 4) + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "                 GROUP BY B.MIRNO)";
                    SQL = SQL + ComNum.VBLF + "         AND A.EdiMirNo > '0'  ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+)";
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.PANO, B.SNAME, A.BI, A.JOHAP, A.DEPTCODE1  ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";
                            SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                            SQL = SQL + ComNum.VBLF + "         AND DEPTCODE = '" + dt.Rows[i]["DEPTCODE1"].ToString().Trim() + "' ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN, BDATE, DEPTCODE, WRTNO)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["PANO"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SANAME"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["BI"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";

                                if (dt.Rows[i]["JOHAP"].ToString().Trim() == "5")   //보호
                                {
                                    SQL = SQL + ComNum.VBLF + "         '2', ";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "         '1', ";
                                }

                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["EDIJAMT"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '4', ";
                                SQL = SQL + ComNum.VBLF + "         '', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["DEPTCODE1"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + VB.Val(dt.Rows[i]["EDIJAMT"].ToString().Trim()) + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    progressBar1.Value = 0;

                    //청구 보류내역 참고사항 READ
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.BI, A.JOHAP, A.DEPTCODE1, A.JINDATE1, A.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID A";

                    if (VB.Val(GstrYYMM) <= 200212)
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE A.YYMM = '" + GstrYYMM + "'";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE (A.BYYMM = '" + GstrYYMM + "' OR  A.YYMM = '" + GstrYYMM + "')"; //2008/10/14 심사과 김준수샘 요청
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND A.UPCNT1 = '9' ";

                    if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND A.JOHAP <> '5'"; }
                    if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND A.JOHAP = '5'"; }

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            strPano = dt.Rows[i]["PANO"].ToString().Trim();
                            strBi = dt.Rows[i]["BI"].ToString().Trim();

                            //참고내역read
                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     WRTNO, REMARK";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSDTL ";
                            SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = '" + dt.Rows[i]["WRTNO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND SUNEXT <> '########' ";
                            SQL = SQL + ComNum.VBLF + "         AND REMARK IS NOT  NULL  ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["REMARK"].ToString().Trim() != "")
                                {
                                    strRemark = dt1.Rows[0]["REMARK"].ToString().Trim();
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;

                            if (strRemark != "")
                            {
                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     ROWID";
                                SQL = SQL + ComNum.VBLF + "FROM MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     WHERE  YYMM = '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "'";
                                SQL = SQL + ComNum.VBLF + "         AND BI = '" + strBi + "'";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";
                                SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                                SQL = SQL + ComNum.VBLF + "         AND BDATE = TO_DATE('" + dt.Rows[i]["JINDATE1"].ToString().Trim() + "', 'YYYYMMDD') ";

                                switch (strBi)
                                {
                                    case "11":
                                    case "12":
                                    case "13":
                                    case "21":
                                    case "22":
                                        break;
                                    default:
                                        SQL = SQL + ComNum.VBLF + "         AND (REMARK IS NULL OR RTRIM(REMARK) = '') ";
                                        break;
                                }

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    //2021-02-23 변환에러로 수정작업
                                    if (Encoding.Default.GetBytes(strRemark).Length > 100)
                                    {
                                        byte[] strByte = Encoding.Default.GetBytes(strRemark);

                                        strRemark = Encoding.Default.GetString(strByte, 0, 100);
                                    }

                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                        return rtnVal;
                                    }
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                if (GstrJong == "0" || GstrJong == "3")
                {
                    #region GoSub OPD_MIR_SANID

                    progressBar1.Value = 0;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PANO, A.EDIGBN, SUM(A.EDITAMT) AS EDITAMT, SUM(A.JAMT) AS JAMT ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID A, " + ComNum.DB_PMPA + "MISU_IDMST B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = B.MISUID ";
                    SQL = SQL + ComNum.VBLF + "         AND A.FRDATE = B.FROMDATE ";
                    SQL = SQL + ComNum.VBLF + "         AND A.TODATE = B.TODATE ";
                    SQL = SQL + ComNum.VBLF + "         AND B.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND B.TONGGBN IN ('1','2') ";
                    SQL = SQL + ComNum.VBLF + "         AND B.IpdOpd = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND B.CLASS IN ('05') ";  //산재
                    SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND A.UPCNT1 <> '9' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBNEDI = '0'"; //원청구 允(2005-11-23) 재설정
                    SQL = SQL + ComNum.VBLF + "         AND ( A.MIRGBN = '1' OR A.MIRGBN IS NULL)";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, A.EDIGBN ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;
                        strPano = dt.Rows[0]["PANO"].ToString().Trim();

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            if (strPano != dt.Rows[i]["PANO"].ToString().Trim())
                            {
                                #region GoSub OPD_MIR_SANID_INSERT

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     ROWID";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count == 0)
                                {
                                    SQL = "";
                                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                    SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                    SQL = SQL + ComNum.VBLF + "VALUES";
                                    SQL = SQL + ComNum.VBLF + "     (";
                                    SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '31', ";
                                    SQL = SQL + ComNum.VBLF + "         'O', ";
                                    SQL = SQL + ComNum.VBLF + "         '3', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dblJAmt + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '4' ";
                                    SQL = SQL + ComNum.VBLF + "     )";
                                }
                                else
                                {
                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dblJAmt + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                                }

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }

                                dt1.Dispose();
                                dt1 = null;

                                #endregion

                                dblJAmt = 0;
                                strPano = dt.Rows[i]["PANO"].ToString().Trim();
                            }

                            if (dt.Rows[i]["EDIGBN"].ToString().Trim() == "3")
                            {
                                dblJAmt = dblJAmt = VB.Fix((int)(VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()) / 10)) * 10;
                            }
                            else
                            {
                                if (VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim()) == 0)
                                {
                                    dblJAmt = dblJAmt = VB.Fix((int)(VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()) / 10)) * 10;
                                }
                                else
                                {
                                    dblJAmt = dblJAmt + VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim());
                                }
                            }
                        }
                    }

                    #region GoSub OPD_MIR_SANID_INSERT

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     ROWID";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                    SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                    SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt1.Rows.Count == 0)
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                        SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + "         '31', ";
                        SQL = SQL + ComNum.VBLF + "         'O', ";
                        SQL = SQL + ComNum.VBLF + "         '3', ";
                        SQL = SQL + ComNum.VBLF + "         '" + dblJAmt + "', ";
                        SQL = SQL + ComNum.VBLF + "         '4' ";
                        SQL = SQL + ComNum.VBLF + "     )";
                    }
                    else
                    {
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                        SQL = SQL + ComNum.VBLF + "     SET";
                        SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dblJAmt + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }

                    dt1.Dispose();
                    dt1 = null;

                    #endregion

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                if (GstrJong == "0" || GstrJong == "4")
                {
                    #region GoSub OPD_MIR_TAID

                    progressBar1.Value = 0;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.MISUID, B.SNAME, SUM(A.AMT2) AS AMT2 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND A.CLASS = '07' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.AMT2 <> '0' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.TONGGBN = '1' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.MISUID = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.MISUID, B.SNAME  ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["MISUID"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '52' ";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["MisuID"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '52', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";
                                SQL = SQL + ComNum.VBLF + "         '4', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["AMT2"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '4' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dt.Rows[i]["AMT2"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                #region GoSub MISU_BALCHECK_PANO_REMARK MISU_BALCHECK_PANO 청구금액 0 인경우 REMARK사항 표시

                progressBar1.Value = 0;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, BI, ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                SQL = SQL + ComNum.VBLF + "     WHERE YYMM ='" + GstrYYMM + "' ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '1' "; } //보험
                else if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '2' "; } //보호
                else if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '3' "; } //산재
                else if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '4' "; } //자보

                SQL = SQL + ComNum.VBLF + "         AND IPDOPD='O' ";
                SQL = SQL + ComNum.VBLF + "         AND JEPJAMT ='0' ";
                SQL = SQL + ComNum.VBLF + "         AND REMARK IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i;

                        //업무의뢰서 요청으로 2006-08-28 尹
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE, REMARK";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_HOANBUL ";
                        SQL = SQL + ComNum.VBLF + "     WHERE BDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND BDATE <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                strRemark = strRemark + dt1.Rows[k]["BDATE"].ToString().Trim() + ":" + dt1.Rows[k]["REMARK"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.ORDERCODE AS ORDERCODE, TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, B.ORDERNAMES AS ORDERNAMES";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_MED + "OCS_ORDERCODE B ";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.BDATE <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.PTNO  = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "         AND A.BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE LIKE '$$%' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE = B.ORDERCODE ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                strRemark = strRemark + dt1.Rows[k]["BDATE"].ToString().Trim() + ":" + dt1.Rows[k]["ORDERNAMES"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strRemark != "")
                        {
                            //2021-02-23 변환에러로 수정작업
                            if (Encoding.Default.GetBytes(strRemark).Length > 100)
                            {
                                byte[] strByte = Encoding.Default.GetBytes(strRemark);

                                strRemark = Encoding.Default.GetString(strByte, 0, 100);
                            }

                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                strTime2 = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":");

                ComFunc.MsgBox(strTime1 + " : " + strTime2);
                
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool OPD_MIR_CHECK_201302(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            int k = 0;

            string strTime1 = "";
            string strTime2 = "";
            string strPano = "";
            string strBi = "";
            string strRemark = "";

            double dblTAmt = 0;
            double dblJAmt = 0;

            if ((GstrJong == "1" || GstrJong == "2") && GstrYYMM == "201402") { return rtnVal; }

            lblMsg.Text = "★ 외래 실청구액 CHECK ★";

            strTime1 = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":");

            try
            {
                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                SQL = SQL + ComNum.VBLF + "     SET";
                SQL = SQL + ComNum.VBLF + "         TOTAMT = 0, ";
                SQL = SQL + ComNum.VBLF + "         JUNGGAN = 0, ";
                SQL = SQL + ComNum.VBLF + "         JOHAP = 0, ";
                SQL = SQL + ComNum.VBLF + "         HALIN = 0, ";
                SQL = SQL + ComNum.VBLF + "         BOJUNG = 0, ";
                SQL = SQL + ComNum.VBLF + "         ETCMISU = 0, ";
                SQL = SQL + ComNum.VBLF + "         SUNAP = 0, ";
                SQL = SQL + ComNum.VBLF + "         DANSU = 0, ";
                SQL = SQL + ComNum.VBLF + "         JEPJAMT = 0";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM ='" + GstrYYMM + "'";
                SQL = SQL + ComNum.VBLF + "     AND Gubun ='4' ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '1' "; } //보험
                if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '2' "; } //보호
                if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '3' "; } //산재
                if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '4' "; } //자보

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                #region GoSub OPD_DATA_RTN_OLD

                progressBar1.Value = 0;

                //201201 부터 외래 건강보험/의료급여 일자별 청구 개시
                if (GstrJong == "1" || GstrJong == "2")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.BDATE, A.PANO, B.SNAME, A.BI, A.DEPTCODE, ";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '96', Amt1 + Amt2, 0)) AS Bun96,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '98', Amt1 + Amt2, 0)) AS Bun98,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '99', Amt1 + Amt2, 0)) AS Bun99 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.ACTDATE >= TO_DATE('" + GstrFDate + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.ACTDATE <= TO_DATE('" + GstrTDate + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.GBSLIP NOT IN ('Z', 'Q', 'E') OR A.GBSLIP IS NULL)";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('92', '96', '98', '99') ";

                    if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND A.BI IN ('11', '12', '13', '51', '32', '42', '43')"; } //允2005-11-21(43종 조합부담금 발생건수발생(2005/03/07)
                    if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND A.BI IN ('20', '21', '22', '23', '24') "; } //보호

                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY BDATE, A.PANO, B.SNAME, A.BI, A.DEPTCODE  ";
                }
                else if (GstrJong == "3" || GstrJong == "4")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     '' AS BDATE, A.PANO, B.SNAME, A.BI, '' AS DEPTCODE, ";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun,'96',Amt1+Amt2,0)) AS Bun96,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun,'98',Amt1+Amt2,0)) AS Bun98,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun,'99',Amt1+Amt2,0)) AS Bun99 ";
                    SQL = SQL + ComNum.VBLF + "FROM OPD_SLIP A, BAS_PATIENT B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.ACTDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.ACTDATE <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.GBSLIP NOT IN ('Z', 'Q', 'E') OR A.GBSLIP IS NULL)";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('92', '96', '98', '99') ";

                    if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "         AND A.BI in ('31', '33') "; } //산재-임시 33종 오류용
                    if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "         AND A.BI = '52' "; } //자보

                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.BI ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i;

                        if (VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) != 0)
                        {
                            dblTAmt = VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim());

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "'  ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                            if (dt.Rows[i]["BDATE"].ToString().Trim() != "")
                            {
                                SQL = SQL + ComNum.VBLF + "         AND BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "', 'YYYY-MM-DD') ";
                            }

                            if (dt.Rows[i]["DEPTCODE"].ToString().Trim() != "")
                            {
                                SQL = SQL + ComNum.VBLF + "         AND DEPTCODE = '" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, TOTAMT, JOHAP, HALIN, SUNAP, JEPJAMT, GUBUN, BDATE, DEPTCODE)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["PANO"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["BI"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";

                                switch (dt.Rows[i]["BI"].ToString().Trim())
                                {
                                    case "20":
                                    case "21":
                                    case "22":
                                    case "23":
                                    case "24":
                                        SQL = SQL + ComNum.VBLF + "         '2', "; //보호
                                        break;
                                    case "31":
                                        SQL = SQL + ComNum.VBLF + "         '3', ";             //산재
                                        break;
                                    case "52":
                                        SQL = SQL + ComNum.VBLF + "         '4', ";             //자보
                                        break;
                                    default:
                                        SQL = SQL + ComNum.VBLF + "         '1', ";            //보험
                                        break;
                                }

                                SQL = SQL + ComNum.VBLF + "         '" + dblTAmt + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '0', ";
                                SQL = SQL + ComNum.VBLF + "         '4', ";
                                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "', 'YYYY-MM-DD'), ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET ";
                                SQL = SQL + ComNum.VBLF + "         TOTAMT = '" + dblTAmt + "', ";
                                SQL = SQL + ComNum.VBLF + "         JOHAP  = '" + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         HALIN  = '" + VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + "', ";

                                switch (dt.Rows[i]["BI"].ToString().Trim())
                                {
                                    case "20":
                                    case "21":
                                    case "22":
                                    case "23":
                                    case "24":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '2', "; //보호
                                        break;
                                    case "31":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '3', "; //산재
                                        break;
                                    case "52":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '4', "; //자보
                                        break;
                                    default:
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '1', "; //보험
                                        break;
                                }

                                SQL = SQL + ComNum.VBLF + "         SUNAP  = '" + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim()) + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                if (GstrJong == "0" || GstrJong == "1" || GstrJong == "2")
                {
                    #region GoSub OPD_MIR_INSID

                    progressBar1.Value = 0;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.WRTNO,  A.PANO, B.SNAME, A.BI, A.JOHAP, A.EDITAMT, A.DEPTCODE1, A.realdept, A.JINDATE1, ";
                    SQL = SQL + ComNum.VBLF + "     (A.EDIJAMT + A.EDIBOAMT + A.EDIGAMT) AS EDIJAMT, A.EDIBAMT ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID A, " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.EDIMIRNO IN";
                    SQL = SQL + ComNum.VBLF + "                     (SELECT";
                    SQL = SQL + ComNum.VBLF + "                         B.MIRNO ";
                    SQL = SQL + ComNum.VBLF + "                     FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "EDI_JEPSU B";
                    SQL = SQL + ComNum.VBLF + "                         WHERE A.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "                             AND A.TONGGBN IN ('1', '2') ";
                    SQL = SQL + ComNum.VBLF + "                             AND A.IpdOpd = 'O'";

                    if (GstrJong == "1")
                    {
                        SQL = SQL + ComNum.VBLF + "                             AND A.CLASS IN ('01', '02', '03') "; //보험
                    }
                    else if (GstrJong == "2")
                    {
                        SQL = SQL + ComNum.VBLF + "                             AND A.CLASS = '04' ";              //보호
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                             AND A.CLASS <= '04'  ";          //보험, 보호
                    }

                    SQL = SQL + ComNum.VBLF + "                             AND A.MISUID = B.JEPNO";

                    if (GstrYYMM != "201104")
                    {
                        SQL = SQL + ComNum.VBLF + "                             AND SUBSTR(B.YYMM, 1, 4) = '" + VB.Left(GstrYYMM, 4) + "' ";
                    }
                        
                    SQL = SQL + ComNum.VBLF + "                     GROUP BY B.MIRNO)";
                    SQL = SQL + ComNum.VBLF + "         AND A.EDIMIRNO > '0'  ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+)";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";
                            SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                            SQL = SQL + ComNum.VBLF + "         AND BDATE = TO_DATE('" + dt.Rows[i]["JINDATE1"].ToString().Trim() + "', 'YYYYMMDD')";

                            if (GstrYYMM == "201311" && dt.Rows[i]["PANO"].ToString().Trim() == "06333239")
                            {
                                SQL = SQL + ComNum.VBLF + "         AND DEPTCODE = '" + dt.Rows[i]["REALDEPT"].ToString().Trim() + "' ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "         AND DEPTCODE = '" + dt.Rows[i]["DEPTCODE1"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN, BDATE, DEPTCODE, WRTNO)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["Pano"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SName"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["Bi"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";

                                if (dt.Rows[i]["JOHAP"].ToString().Trim() == "5")   //보호
                                {
                                    SQL = SQL + ComNum.VBLF + "         '2', ";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "         '1', ";
                                }

                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["EDIJAMT"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '4', ";
                                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dt.Rows[i]["JINDATE1"].ToString().Trim() + "', 'YYYYMMDD'), ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["DEPTCODE1"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["WRTNO"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + VB.Val(dt.Rows[i]["EDIJAMT"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         WRTNO = '" + dt.Rows[i]["WRTNO"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    progressBar1.Value = 0;

                    //청구 보류내역 참고사항 READ
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.BI, A.JOHAP, A.DEPTCODE1, A.JINDATE1, A.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID A";

                    if (VB.Val(GstrYYMM) <= 200212)
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE A.YYMM = '" + GstrYYMM + "' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE (A.BYYMM = '" + GstrYYMM + "' OR A.YYMM = '" + GstrYYMM + "')"; //2008/10/14 심사과 김준수샘 요청
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND A.UPCNT1 = '9' ";

                    if (GstrJong == "1")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.JOHAP <>'5'";
                    }

                    if (GstrJong == "2")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.JOHAP ='5'";
                    }

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            strPano = dt.Rows[i]["PANO"].ToString().Trim();
                            strBi = dt.Rows[i]["BI"].ToString().Trim();
                            strRemark = "";

                            //참고내역read
                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     WRTNO, REMARK";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSDTL ";
                            SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = '" + dt.Rows[i]["WRTNO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND SUNEXT <> '########' ";
                            SQL = SQL + ComNum.VBLF + "         AND REMARK IS NOT  NULL  ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["REMARK"].ToString().Trim() != "")
                                {
                                    strRemark = dt1.Rows[0]["REMARK"].ToString().Trim();
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;

                            if (strRemark != "")
                            {
                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     ROWID";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "'";
                                SQL = SQL + ComNum.VBLF + "         AND BI = '" + strBi + "'";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";
                                SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                                SQL = SQL + ComNum.VBLF + "         AND BDATE = TO_DATE('" + dt.Rows[i]["JINDATE1"].ToString().Trim() + "', 'YYYYMMDD')";

                                switch (strBi)
                                {
                                    case "11":
                                    case "12":
                                    case "13":
                                    case "21":
                                    case "22":
                                        break;
                                    default:
                                        SQL = SQL + ComNum.VBLF + "         AND (REMARK IS NULL OR RTRIM(REMARK) = '')";
                                        break;
                                }

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    //2021-02-23 변환에러로 수정작업
                                    if (Encoding.Default.GetBytes(strRemark).Length > 100)
                                    {
                                        byte[] strByte = Encoding.Default.GetBytes(strRemark);

                                        strRemark = Encoding.Default.GetString(strByte, 0, 100);
                                    }

                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                        return rtnVal;
                                    }
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                if (GstrJong == "0" || GstrJong == "3")
                {
                    #region GoSub OPD_MIR_SANID

                    progressBar1.Value = 0;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PANO, A.EDIGBN, SUM(A.EDITAMT) AS EDITAMT, SUM(A.JAMT) AS JAMT ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID A, " + ComNum.DB_PMPA + "MISU_IDMST B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = B.MISUID ";
                    SQL = SQL + ComNum.VBLF + "         AND A.FRDATE = B.FROMDATE ";
                    SQL = SQL + ComNum.VBLF + "         AND A.TODATE = B.TODATE ";
                    SQL = SQL + ComNum.VBLF + "         AND B.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND B.TONGGBN IN ('1', '2') ";
                    SQL = SQL + ComNum.VBLF + "         AND B.IpdOpd = 'O' ";
                    SQL = SQL + ComNum.VBLF + "         AND B.CLASS IN ('05') "; //산재
                    SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND A.UPCNT1 <> '9' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBNEDI = '0'"; //원청구 允(2005-11-23) 재설정
                    SQL = SQL + ComNum.VBLF + "         AND ( A.MIRGBN = '1' OR A.MIRGBN IS  NULL)";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, A.EDIGBN ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        strPano = dt.Rows[0]["PANO"].ToString().Trim();

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            if (strPano != dt.Rows[i]["PANO"].ToString().Trim())
                            {
                                #region GoSub OPD_MIR_SANID_INSERT

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     ROWID";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count == 0)
                                {
                                    SQL = "";
                                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                    SQL = SQL + ComNum.VBLF + "VALUES";
                                    SQL = SQL + ComNum.VBLF + "     (";
                                    SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '31', ";
                                    SQL = SQL + ComNum.VBLF + "         'O', ";
                                    SQL = SQL + ComNum.VBLF + "         '3', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dblJAmt + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '4' ";
                                    SQL = SQL + ComNum.VBLF + "     )";
                                }
                                else
                                {
                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dblJAmt + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                                }

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }

                                dt1.Dispose();
                                dt1 = null;

                                #endregion

                                dblJAmt = 0;
                                strPano = dt.Rows[i]["PANO"].ToString().Trim();
                            }

                            if (dt.Rows[i]["EDIGBN"].ToString().Trim() == "3")
                            {
                                dblJAmt = dblJAmt + VB.Fix((int)(VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()) / 10)) * 10;
                            }
                            else
                            {
                                if (VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim()) == 0)
                                {
                                    dblJAmt = dblJAmt + VB.Fix((int)(VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()) / 10)) * 10;
                                }
                                else
                                {
                                    dblJAmt = dblJAmt + VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim());
                                }
                            }
                        }
                    }

                    #region GoSub OPD_MIR_SANID_INSERT

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     ROWID";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                    SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                    SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt1.Rows.Count == 0)
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                        SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + "         '31', ";
                        SQL = SQL + ComNum.VBLF + "         'O', ";
                        SQL = SQL + ComNum.VBLF + "         '3', ";
                        SQL = SQL + ComNum.VBLF + "         '" + dblJAmt + "', ";
                        SQL = SQL + ComNum.VBLF + "         '4' ";
                        SQL = SQL + ComNum.VBLF + "     )";
                    }
                    else
                    {
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                        SQL = SQL + ComNum.VBLF + "     SET";
                        SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dblJAmt + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }

                    dt1.Dispose();
                    dt1 = null;

                    #endregion

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                if (GstrJong == "0" || GstrJong == "4")
                {
                    #region GoSub OPD_MIR_TAID

                    progressBar1.Value = 0;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.MISUID, B.SNAME, SUM(A.AMT2) AS AMT2 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND A.CLASS = '07' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.AMT2 <> '0' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.TONGGBN = '1' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.MISUID = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.MISUID, B.SNAME  ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["misuid"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '52' ";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["MisuID"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '52', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";
                                SQL = SQL + ComNum.VBLF + "         '4', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["AMT2"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '4' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dt.Rows[i]["AMT2"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                }


                #region GoSub MISU_BALCHECK_PANO_REMARK MISU_BALCHECK_PANO 청구금액 0 인경우 REMARK사항 표시

                progressBar1.Value = 0;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, BI, ROWID, TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '1' "; } //보험
                else if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '2' "; } //보호
                else if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '3' "; } //산재
                else if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '4' "; } //자보

                SQL = SQL + ComNum.VBLF + "         AND IPDOPD='O' ";
                SQL = SQL + ComNum.VBLF + "         AND JEPJAMT ='0' ";
                SQL = SQL + ComNum.VBLF + "         AND REMARK IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strRemark = "";

                        //참고내역read
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     WRTNO, REMARK";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSDTL ";
                        SQL = SQL + ComNum.VBLF + "     WHERE WRTNO IN";
                        SQL = SQL + ComNum.VBLF + "             (SELECT";
                        SQL = SQL + ComNum.VBLF + "                 WRTNO";
                        SQL = SQL + ComNum.VBLF + "             FROM " + ComNum.DB_PMPA + "MIR_INSID ";
                        SQL = SQL + ComNum.VBLF + "                 WHERE (BYYMM = '" + GstrYYMM + "' OR YYMM = '" + GstrYYMM + "' ) "; //2008/10/14 심사과 김준수샘 요청
                        SQL = SQL + ComNum.VBLF + "                     AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "                     AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "                     AND IPDOPD = 'O'";

                        if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "                     AND JOHAP <> '5'"; }
                        if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "                     AND JOHAP = '5' "; }

                        SQL = SQL + ComNum.VBLF + "                     AND UPCNT1 = '9')";
                        SQL = SQL + ComNum.VBLF + "         AND SUNEXT <>'########' ";
                        SQL = SQL + ComNum.VBLF + "         AND REMARK IS NOT  NULL  ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["REMARK"].ToString().Trim() != "")
                            {
                                strRemark = dt1.Rows[0]["REMARK"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        //업무의뢰서 요청으로 2006-08-28 尹
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE, 'YYYY-MM-DD') AS BDATE, REMARK";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_HOANBUL ";
                        SQL = SQL + ComNum.VBLF + "     WHERE BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                strRemark = strRemark + dt1.Rows[k]["BDATE"].ToString().Trim() + ":" + dt1.Rows[k]["REMARK"].ToString().Trim() + VB.Space(4);
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.ORDERCODE AS ORDERCODE, TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, B.ORDERNAMES AS ORDERNAMES   ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_MED + "OCS_ORDERCODE B ";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "' ,'YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.PTNO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "         AND A.BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE LIKE '$$%' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE = B.ORDERCODE ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                strRemark = strRemark + dt1.Rows[k]["BDATE"].ToString().Trim() + ":" + dt1.Rows[k]["ORDERNAMES"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strRemark != "")
                        {
                            //2021-02-23 변환에러로 수정작업
                            if (Encoding.Default.GetBytes(strRemark).Length > 100)
                            {
                                byte[] strByte = Encoding.Default.GetBytes(strRemark);

                                strRemark = Encoding.Default.GetString(strByte, 0, 100);
                            }

                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                strTime2 = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":");

                ComFunc.MsgBox(strTime1 + " : " + strTime2);
                
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool OPD_MIR_CHECK_201201(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            int k = 0;

            string strTime1 = "";
            string strTime2 = "";
            string strPano = "";
            string strBi = "";
            string strRemark = "";

            double dblTAmt = 0;
            double dblJAmt = 0;

            lblMsg.Text = "★ 외래 실청구액 CHECK ★";

            try
            {
                strTime1 = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":");

                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                SQL = SQL + ComNum.VBLF + "     SET";
                SQL = SQL + ComNum.VBLF + "         TOTAMT = 0, ";
                SQL = SQL + ComNum.VBLF + "         JUNGGAN = 0, ";
                SQL = SQL + ComNum.VBLF + "         JOHAP = 0, ";
                SQL = SQL + ComNum.VBLF + "         HALIN = 0, ";
                SQL = SQL + ComNum.VBLF + "         BOJUNG = 0, ";
                SQL = SQL + ComNum.VBLF + "         ETCMISU = 0, ";
                SQL = SQL + ComNum.VBLF + "         SUNAP = 0, ";
                SQL = SQL + ComNum.VBLF + "         DANSU = 0, ";
                SQL = SQL + ComNum.VBLF + "         JEPJAMT = 0";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM = '" + GstrYYMM + "'";
                SQL = SQL + ComNum.VBLF + "     AND Gubun = '4' ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '1' "; } //보험
                else if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '2' "; } //보호
                else if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '3' "; } //산재
                else if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '4' "; } //자보

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                #region GoSub OPD_DATA_RTN_OLD

                progressBar1.Value = 0;

                //201201 부터 외래 건강보험/의료급여 일자별 청구 개시
                if (GstrJong == "1" || GstrJong == "2")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.BDATE, A.PANO, B.SNAME, A.BI, A.DEPTCODE, ";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '96', Amt1 + Amt2, 0)) AS Bun96,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '98', Amt1 + Amt2, 0)) AS Bun98,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '99', Amt1 + Amt2, 0)) AS Bun99 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.ACTDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.ACTDATE <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.GBSLIP NOT IN ('Z', 'Q', 'E') OR A.GBSLIP IS NULL)";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('92', '96', '98', '99') ";

                    if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND A.BI IN ('11', '12', '13', '51', '32', '42', '43')"; } //允2005-11-21(43종 조합부담금 발생건수발생(2005/03/07)
                    if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND A.BI IN ('20', '21', '22', '23', '24') "; } //보호

                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY BDATE, A.PANO, B.SNAME, A.BI, A.DEPTCODE  ";
                }
                else if (GstrJong == "3" || GstrJong == "4")
                {
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     '' AS BDATE, A.PANO, B.SNAME, A.BI, '' AS DEPTCODE,   ";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '96', Amt1 + Amt2, 0)) AS Bun96,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '98', Amt1 + Amt2, 0)) AS Bun98,";
                    SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '99', Amt1 + Amt2, 0)) AS Bun99 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.ACTDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.ACTDATE <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.GBSLIP NOT IN ('Z','Q','E') OR A.GBSLIP IS NULL)";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('92','96','98','99') ";

                    if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "         AND A.BI in ('31', '33') "; } //산재-임시 33종 오류용
                    if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "         AND A.BI = '52' "; } //자보

                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.BI ";
                }
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i;

                        if (VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) != 0)
                        {
                            dblTAmt = VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim());

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "'  ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                            if (dt.Rows[i]["BDATE"].ToString().Trim() != "")
                            {
                                SQL = SQL + ComNum.VBLF + "    AND BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                            }

                            if (dt.Rows[i]["DEPTCODE"].ToString().Trim() != "")
                            {
                                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, TOTAMT, JOHAP, HALIN, SUNAP, JEPJAMT, GUBUN, BDATE, DEPTCODE)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["Pano"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SName"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["Bi"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";

                                switch (dt.Rows[i]["Bi"].ToString().Trim())
                                {
                                    case "20":
                                    case "21":
                                    case "22":
                                    case "23":
                                    case "24":
                                        SQL = SQL + ComNum.VBLF + "         '2', "; //보호
                                        break;
                                    case "31":
                                        SQL = SQL + ComNum.VBLF + "         '3', ";             //산재
                                        break;
                                    case "52":
                                        SQL = SQL + ComNum.VBLF + "         '4', ";             //자보
                                        break;
                                    default:
                                        SQL = SQL + ComNum.VBLF + "         '1', ";            //보험
                                        break;
                                }

                                SQL = SQL + ComNum.VBLF + "         '" + dblTAmt + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '0', ";
                                SQL = SQL + ComNum.VBLF + "         '4', ";
                                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dt.Rows[i]["BDate"].ToString().Trim() + "' ,'YYYY-MM-DD'), ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET ";
                                SQL = SQL + ComNum.VBLF + "         TOTAMT = '" + dblTAmt + "', ";
                                SQL = SQL + ComNum.VBLF + "         JOHAP  = '" + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         HALIN  = '" + VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + "', ";

                                switch (dt.Rows[i]["Bi"].ToString().Trim())
                                {
                                    case "20":
                                    case "21":
                                    case "22":
                                    case "23":
                                    case "24":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '2', "; //보호
                                        break;
                                    case "31":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '3', ";             //산재
                                        break;
                                    case "52":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '4', ";             //자보
                                        break;
                                    default:
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '1', ";            //보험
                                        break;
                                }

                                SQL = SQL + ComNum.VBLF + "         SUNAP = '" + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim()) + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                if (GstrJong == "0" || GstrJong == "1" || GstrJong == "2")
                {
                    #region GoSub OPD_MIR_INSID

                    progressBar1.Value = 0;

                    //청구내역
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.WRTNO, A.PANO, B.SNAME, A.BI, A.JOHAP, A.EDITAMT, A.DEPTCODE1, A.JINDATE1, ";
                    SQL = SQL + ComNum.VBLF + "     (A.EDIJAMT + A.EDIBOAMT + A.EDIGAMT) AS EDIJAMT, A.EDIBAMT";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID A, " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.EDIMIRNO IN";
                    SQL = SQL + ComNum.VBLF + "                 (SELECT";
                    SQL = SQL + ComNum.VBLF + "                     B.MIRNO ";
                    SQL = SQL + ComNum.VBLF + "                 FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "EDI_JEPSU B";
                    SQL = SQL + ComNum.VBLF + "                     WHERE A.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "                         AND A.TONGGBN IN ('1', '2') ";
                    SQL = SQL + ComNum.VBLF + "                         AND A.IpdOpd = 'O'";

                    if (GstrJong == "1")
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND A.CLASS IN ('01', '02', '03') "; //보험
                    }
                    else if (GstrJong == "2")
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND A.CLASS = '04' ";              //보호
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND A.CLASS <= '04'  ";          //보험, 보호
                    }

                    SQL = SQL + ComNum.VBLF + "                         AND A.MISUID = B.JEPNO";

                    if (GstrYYMM != "201104")
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND SUBSTR(B.YYMM, 1, 4) = '" + VB.Left(GstrYYMM, 4) + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "                 GROUP BY B.MIRNO)";
                    SQL = SQL + ComNum.VBLF + "  AND A.PANO = B.PANO(+)";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";
                            SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                            SQL = SQL + ComNum.VBLF + "         AND BDATE = TO_DATE('" + dt.Rows[i]["JINDATE1"].ToString().Trim() + "','YYYYMMDD' ) ";
                            SQL = SQL + ComNum.VBLF + "         AND DEPTCODE = '" + dt.Rows[i]["DEPTCODE1"].ToString().Trim() + "' ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN, BDATE, DEPTCODE, WRTNO)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["Pano"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SName"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["Bi"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";

                                if (dt.Rows[i]["JOHAP"].ToString().Trim() == "5")   //보호
                                {
                                    SQL = SQL + ComNum.VBLF + "         '2', ";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "         '1', ";
                                }

                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["EDIJAMT"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '4', ";
                                SQL = SQL + ComNum.VBLF + "         TO_DATE('" + dt.Rows[i]["JINDATE1"].ToString().Trim() + "', 'YYYYMMDD'), ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["DEPTCODE1"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["WRTNO"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + VB.Val(dt.Rows[i]["EDIJAMT"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         WRTNO = '" + dt.Rows[i]["WRTNO"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    progressBar1.Value = 0;

                    //청구 보류내역 참고사항 READ
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.BI, A.JOHAP, A.DEPTCODE1, A.JINDATE1, A.WRTNO ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID A";

                    if (VB.Val(GstrYYMM) <= 200212)
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE A.YYMM = '" + GstrYYMM + "' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE (A.BYYMM = '" + GstrYYMM + "' OR  A.YYMM = '" + GstrYYMM + "' ) "; //2008/10/14 심사과 김준수샘 요청
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.UPCNT1 = '9' ";

                    if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND A.JOHAP <> '5'"; }
                    if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND A.JOHAP = '5'"; }

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            strPano = dt.Rows[i]["PANO"].ToString().Trim();
                            strBi = dt.Rows[i]["BI"].ToString().Trim();

                            //참고내역read
                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     WRTNO, REMARK";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSdt1L ";
                            SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = '" + dt.Rows[i]["WRTNO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND SUNEXT <> '########' ";
                            SQL = SQL + ComNum.VBLF + "         AND REMARK IS NOT NULL";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["REMARK"].ToString().Trim() != "")
                                {
                                    strRemark = dt1.Rows[0]["REMARK"].ToString().Trim();
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;

                            if (strRemark != "")
                            {
                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     ROWID";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "'";
                                SQL = SQL + ComNum.VBLF + "         AND BI = '" + strBi + "'";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";
                                SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                                SQL = SQL + ComNum.VBLF + "         AND BDATE = TO_DATE('" + dt.Rows[i]["JINDATE1"].ToString().Trim() + "', 'YYYYMMDD')";
                                SQL = SQL + ComNum.VBLF + "         AND (REMARK IS NULL OR RTRIM(REMARK) = '')";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    //2021-02-23 변환에러로 수정작업
                                    if (Encoding.Default.GetBytes(strRemark).Length > 100)
                                    {
                                        byte[] strByte = Encoding.Default.GetBytes(strRemark);

                                        strRemark = Encoding.Default.GetString(strByte, 0, 100);
                                    }

                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                        return rtnVal;
                                    }
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                if (GstrJong == "0" || GstrJong == "3")
                {
                    #region GoSub OPD_MIR_SANID

                    progressBar1.Value = 0;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     PANO, EDIGBN, SUM(EDITAMT) AS EDITAMT, SUM(JAMT) AS JAMT";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID";
                    SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND UPCNT1 <> '9' ";
                    SQL = SQL + ComNum.VBLF + "         AND GBNEDI = '0'"; //원청구 允(2005-11-23) 재설정
                    SQL = SQL + ComNum.VBLF + "         AND (MIRGBN = '1' OR MIRGBN IS NULL)";
                    SQL = SQL + ComNum.VBLF + "GROUP BY PANO, EDIGBN ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        strPano = dt.Rows[0]["PANO"].ToString().Trim();
                        dblJAmt = 0;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (strPano != dt.Rows[i]["PANO"].ToString().Trim())
                            {
                                #region OPD_MIR_SANID_INSERT

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     ROWID";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count == 0)
                                {
                                    SQL = "";
                                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                    SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                    SQL = SQL + ComNum.VBLF + "VALUES";
                                    SQL = SQL + ComNum.VBLF + "     (";
                                    SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '31', ";
                                    SQL = SQL + ComNum.VBLF + "         'O', ";
                                    SQL = SQL + ComNum.VBLF + "         '3', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dblJAmt + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '4' ";
                                    SQL = SQL + ComNum.VBLF + "     )";
                                }
                                else
                                {
                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dblJAmt + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                                }

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }

                                dt1.Dispose();
                                dt1 = null;

                                #endregion

                                dblJAmt = 0;
                                strPano = dt.Rows[i]["PANO"].ToString().Trim();
                            }

                            if (dt.Rows[i]["EDIGBN"].ToString().Trim() == "3")
                            {
                                dblJAmt = dblJAmt + VB.Fix((int)(VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()) / 10)) * 10;
                            }
                            else
                            {
                                if (VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim()) == 0)
                                {
                                    dblJAmt = dblJAmt + VB.Fix((int)(VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()) / 10)) * 10;
                                }
                                else
                                {
                                    dblJAmt = dblJAmt + VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim());
                                }
                            }
                        }
                    }

                    #region OPD_MIR_SANID_INSERT

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     ROWID";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                    SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                    SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt1.Rows.Count == 0)
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                        SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + "         '31', ";
                        SQL = SQL + ComNum.VBLF + "         'O', ";
                        SQL = SQL + ComNum.VBLF + "         '3', ";
                        SQL = SQL + ComNum.VBLF + "         '" + dblJAmt + "', ";
                        SQL = SQL + ComNum.VBLF + "         '4' ";
                        SQL = SQL + ComNum.VBLF + "     )";
                    }
                    else
                    {
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                        SQL = SQL + ComNum.VBLF + "     SET";
                        SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dblJAmt + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }

                    dt1.Dispose();
                    dt1 = null;

                    #endregion

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                if (GstrJong == "0" || GstrJong == "4")
                {
                    #region GoSub OPD_MIR_TAID

                    progressBar1.Value = 0;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.MISUID, B.SNAME, SUM(A.AMT2) AS AMT2 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + "MISU_IDMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND A.CLASS = '07' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.AMT2 <> '0' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.TONGGBN = '1' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.MISUID = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.MISUID, B.SNAME  ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["misuid"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '52' ";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["MisuID"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '52', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";
                                SQL = SQL + ComNum.VBLF + "         '4', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["AMT2"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '4' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dt.Rows[i]["AMT2"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                #region GoSub MISU_BALCHECK_PANO_REMARK MISU_BALCHECK_PANO 청구금액 0 인경우 REMARK사항 표시

                progressBar1.Value = 0;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, BI, ROWID, TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '1' "; } //보험
                if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '2' "; } //보호
                if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '3' "; } //산재
                if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '4' "; } //자보

                SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                SQL = SQL + ComNum.VBLF + "         AND JEPJAMT = '0' ";

                switch (GstrJong)
                {
                    case "1":
                    case "2":   //보험  '보호
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "         AND REMARK IS NULL ";
                        break;
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i;
                        strRemark = "";

                        //업무의뢰서 요청으로 2006-08-28 尹
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE, REMARK";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_HOANBUL ";
                        SQL = SQL + ComNum.VBLF + "     WHERE BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                strRemark = strRemark + dt1.Rows[k]["BDATE"].ToString().Trim() + ":" + dt1.Rows[k]["REMARK"].ToString().Trim() + VB.Space(4);
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.ORDERCODE AS ORDERCODE, TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, B.ORDERNAMES AS ORDERNAMES ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_MED + "OCS_ORDERCODE B ";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "' ,'YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.PTNO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "         AND A.BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE LIKE '$$%' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE = B.ORDERCODE ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                strRemark = strRemark + dt1.Rows[k]["BDATE"].ToString().Trim() + ":" + dt1.Rows[k]["ORDERNAMES"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strRemark != "")
                        {
                            //2021-02-23 변환에러로 수정작업
                            if (Encoding.Default.GetBytes(strRemark).Length > 100)
                            {
                                byte[] strByte = Encoding.Default.GetBytes(strRemark);

                                strRemark = Encoding.Default.GetString(strByte, 0, 100);
                            }

                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt.Rows[i]["ROWID"].ToString().Trim() + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                strTime2 = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":");

                ComFunc.MsgBox(strTime1 + " : " + strTime2);
                
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool OPD_MIR_CHECK(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            int k = 0;

            string strTime1 = "";
            string strTime2 = "";
            string strPano = "";
            string strBi = "";
            string strRemark = "";

            double dblTAmt = 0;
            double dblJAmt = 0;

            lblMsg.Text = "★ 외래 실청구액 CHECK ★";

            try
            {
                strTime1 = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":");

                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                SQL = SQL + ComNum.VBLF + "     SET";
                SQL = SQL + ComNum.VBLF + "         TOTAMT = 0, ";
                SQL = SQL + ComNum.VBLF + "         JUNGGAN = 0, ";
                SQL = SQL + ComNum.VBLF + "         JOHAP = 0, ";
                SQL = SQL + ComNum.VBLF + "         HALIN = 0, ";
                SQL = SQL + ComNum.VBLF + "         BOJUNG = 0, ";
                SQL = SQL + ComNum.VBLF + "         ETCMISU = 0, ";
                SQL = SQL + ComNum.VBLF + "         SUNAP = 0, ";
                SQL = SQL + ComNum.VBLF + "         DANSU = 0, ";
                SQL = SQL + ComNum.VBLF + "         JEPJAMT = 0";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM = '" + GstrYYMM + "'";
                SQL = SQL + ComNum.VBLF + "     AND Gubun = '4' ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '1' "; } //보험
                else if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '2' "; } //보호
                else if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '3' "; } //산재
                else if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "     AND SUBI = '4' "; } //자보

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                #region GoSub OPD_DATA_RTN_OLD

                progressBar1.Value = 0;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, B.SNAME, A.BI, ";
                SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '96', Amt1 + Amt2, 0)) AS Bun96,";
                SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '98', Amt1 + Amt2, 0)) AS Bun98,";
                SQL = SQL + ComNum.VBLF + "     SUM(DECODE(A.Bun, '99', Amt1 + Amt2, 0)) AS Bun99 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.ACTDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.ACTDATE <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND (A.GBSLIP NOT IN ('Z', 'Q', 'E') OR A.GBSLIP IS NULL)";
                SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('92', '96', '98', '99') ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND A.BI IN ('11', '12', '13', '51', '32', '42', '43')"; } //允2005-11-21(43종 조합부담금 발생건수발생(2005/03/07)
                if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND A.BI IN ('20', '21', '22', '23', '24') "; } //보호
                if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "         AND A.BI in ('31', '33') "; } //산재-임시 33종 오류용
                if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "         AND A.BI = '52' "; } //자보

                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.BI ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i;

                        if (VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) != 0)
                        {
                            dblTAmt = VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim());

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, TOTAMT, JOHAP, HALIN, SUNAP, JEPJAMT, GUBUN)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["PANO"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["BI"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";

                                switch (dt.Rows[i]["BI"].ToString().Trim())
                                {
                                    case "20":
                                    case "21":
                                    case "22":
                                    case "23":
                                    case "24":
                                        SQL = SQL + ComNum.VBLF + "         '2', "; //보호
                                        break;
                                    case "31":
                                        SQL = SQL + ComNum.VBLF + "         '3', ";             //산재
                                        break;
                                    case "52":
                                        SQL = SQL + ComNum.VBLF + "         '4', ";             //자보
                                        break;
                                    default:
                                        SQL = SQL + ComNum.VBLF + "         '1', ";            //보험
                                        break;
                                }

                                SQL = SQL + ComNum.VBLF + "         '" + dblTAmt + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         '0', ";
                                SQL = SQL + ComNum.VBLF + "         '4' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET ";
                                SQL = SQL + ComNum.VBLF + "         TOTAMT = '" + dblTAmt + "', ";
                                SQL = SQL + ComNum.VBLF + "         JOHAP = '" + VB.Val(dt.Rows[i]["BUN98"].ToString().Trim()) + "', ";
                                SQL = SQL + ComNum.VBLF + "         HALIN = '" + VB.Val(dt.Rows[i]["BUN96"].ToString().Trim()) + "', ";

                                switch (dt.Rows[i]["BI"].ToString().Trim())
                                {
                                    case "20":
                                    case "21":
                                    case "22":
                                    case "23":
                                    case "24":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '2', "; //보호
                                        break;
                                    case "31":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '3', ";             //산재
                                        break;
                                    case "52":
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '4', ";             //자보
                                        break;
                                    default:
                                        SQL = SQL + ComNum.VBLF + "         SUBI = '1', ";            //보험
                                        break;
                                }

                                SQL = SQL + ComNum.VBLF + "         SUNAP  = '" + VB.Val(dt.Rows[i]["BUN99"].ToString().Trim()) + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                if (GstrJong == "0" || GstrJong == "1" || GstrJong == "2")
                {
                    #region GoSub OPD_MIR_INSID

                    progressBar1.Value = 0;

                    //청구내역
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PANO, B.SNAME, A.BI, A.JOHAP, SUM(A.EDITAMT) AS EDITAMT, ";
                    SQL = SQL + ComNum.VBLF + "     SUM(A.EDIJAMT + A.EDIBOAMT + A.EDIGAMT) AS EDIJAMT, SUM(A.EDIBAMT) AS EDIBAMT ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID A, " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.EDIMIRNO IN";
                    SQL = SQL + ComNum.VBLF + "                 (SELECT";
                    SQL = SQL + ComNum.VBLF + "                     B.MIRNO ";
                    SQL = SQL + ComNum.VBLF + "                 FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "EDI_JEPSU B";
                    SQL = SQL + ComNum.VBLF + "                     WHERE A.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "                         AND A.TONGGBN IN ('1', '2') ";
                    SQL = SQL + ComNum.VBLF + "                         AND A.IpdOpd = 'O'";

                    if (GstrJong == "1")
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND A.CLASS IN ('01', '02', '03') "; //보험
                    }
                    else if (GstrJong == "2")
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND A.CLASS = '04' ";              //보호
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND A.CLASS <= '04'  ";          //보험, 보호
                    }

                    SQL = SQL + ComNum.VBLF + "                         AND A.MISUID = B.JEPNO";

                    if (GstrYYMM != "201104")
                    {
                        SQL = SQL + ComNum.VBLF + "                         AND SUBSTR(B.YYMM, 1, 4) = '" + VB.Left(GstrYYMM, 4) + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "                 GROUP BY B.MIRNO)";
                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO(+)";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.BI, A.JOHAP";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";
                            SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["PANO"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["BI"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";

                                if (dt.Rows[i]["JOHAP"].ToString().Trim() == "5") //보호
                                {
                                    SQL = SQL + ComNum.VBLF + "         '2', ";
                                }
                                else
                                {
                                    SQL = SQL + ComNum.VBLF + "         '1', ";
                                }

                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["EDIJAMT"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '4' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dt.Rows[i]["EDIJAMT"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    progressBar1.Value = 0;

                    //청구 보류내역 참고사항 READ
                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.BI, A.JOHAP ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID A";

                    if (VB.Val(GstrYYMM) <= 200212)
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE A.YYMM = '" + GstrYYMM + "' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     WHERE (A.BYYMM = '" + GstrYYMM + "' OR A.YYMM = '" + GstrYYMM + "' ) "; //2008/10/14 심사과 김준수샘 요청
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND A.UPCNT1 = '9' ";

                    if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND A.JOHAP <> '5'"; }
                    if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND A.JOHAP = '5'"; }

                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, A.SNAME, A.BI, A.JOHAP ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            strPano = dt.Rows[i]["PANO"].ToString().Trim();
                            strBi = dt.Rows[i]["BI"].ToString().Trim();

                            //참고내역read
                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     WRTNO, REMARK";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSDTL ";
                            SQL = SQL + ComNum.VBLF + "     WHERE WRTNO IN";
                            SQL = SQL + ComNum.VBLF + "             (SELECT";
                            SQL = SQL + ComNum.VBLF + "                 WRTNO";
                            SQL = SQL + ComNum.VBLF + "             FROM " + ComNum.DB_PMPA + "MIR_INSID ";

                            if (VB.Val(GstrYYMM) <= 200212)
                            {
                                SQL = SQL + ComNum.VBLF + "                 WHERE YYMM = '" + GstrYYMM + "'";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "                 WHERE (BYYMM = '" + GstrYYMM + "' OR YYMM = '" + GstrYYMM + "' ) "; //2008/10/14 심사과 김준수샘 요청
                            }

                            SQL = SQL + ComNum.VBLF + "                   AND PANO = '" + strPano + "'";
                            SQL = SQL + ComNum.VBLF + "                   AND BI = '" + strBi + "'";
                            SQL = SQL + ComNum.VBLF + "                   AND IPDOPD = 'O'";

                            if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "                   AND JOHAP <> '5' "; }
                            if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "                   AND JOHAP = '5' "; }

                            SQL = SQL + ComNum.VBLF + "                   AND UPCNT1 = '9')";
                            SQL = SQL + ComNum.VBLF + "         AND SUNEXT <> '########' ";
                            SQL = SQL + ComNum.VBLF + "         AND REMARK IS NOT  NULL  ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["REMARK"].ToString().Trim() != "")
                                {
                                    strRemark = dt1.Rows[0]["REMARK"].ToString().Trim();
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;

                            if (strRemark != "")
                            {
                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     ROWID";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     WHERE  YYMM = '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "'";
                                SQL = SQL + ComNum.VBLF + "         AND BI = '" + strBi + "'";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";
                                SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                                SQL = SQL + ComNum.VBLF + "         AND (REMARK IS NULL OR RTRIM(REMARK) = '') ";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    //2021-02-23 변환에러로 수정작업
                                    if (Encoding.Default.GetBytes(strRemark).Length > 100)
                                    {
                                        byte[] strByte = Encoding.Default.GetBytes(strRemark);

                                        strRemark = Encoding.Default.GetString(strByte, 0, 100);
                                    }

                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";

                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                        ComFunc.MsgBox(SqlErr);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                        return rtnVal;
                                    }
                                }

                                dt1.Dispose();
                                dt1 = null;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                if (GstrJong == "0" || GstrJong == "3")
                {
                    #region GoSub OPD_MIR_SANID

                    progressBar1.Value = 0;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     PANO, EDIGBN, SUM(EDITAMT) AS EDITAMT, SUM(JAMT) AS JAMT ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_SANID";
                    SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "'";
                    SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND UPCNT1 <> '9' ";
                    SQL = SQL + ComNum.VBLF + "         AND GBNEDI = '0'"; //원청구 允(2005-11-23) 재설정
                    SQL = SQL + ComNum.VBLF + "         AND ( MIRGBN = '1' OR MIRGBN IS NULL)";
                    SQL = SQL + ComNum.VBLF + "GROUP BY PANO, EDIGBN ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        strPano = dt.Rows[0]["PANO"].ToString().Trim();
                        dblJAmt = 0;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            if (strPano != dt.Rows[i]["PANO"].ToString().Trim())
                            {
                                #region GoSub OPD_MIR_SANID_INSERT

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     ROWID";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                                SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                                if (dt1.Rows.Count == 0)
                                {
                                    SQL = "";
                                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                    SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                    SQL = SQL + ComNum.VBLF + "VALUES";
                                    SQL = SQL + ComNum.VBLF + "     (";
                                    SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '31', ";
                                    SQL = SQL + ComNum.VBLF + "         'O', ";
                                    SQL = SQL + ComNum.VBLF + "         '3', ";
                                    SQL = SQL + ComNum.VBLF + "         '" + dblJAmt + "', ";
                                    SQL = SQL + ComNum.VBLF + "         '4' ";
                                    SQL = SQL + ComNum.VBLF + "     )";
                                }
                                else
                                {
                                    SQL = "";
                                    SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                    SQL = SQL + ComNum.VBLF + "     SET";
                                    SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dblJAmt + "' ";
                                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                                }

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }

                                dt1.Dispose();
                                dt1 = null;

                                #endregion

                                dblJAmt = 0;
                                strPano = dt.Rows[i]["PANO"].ToString().Trim();
                            }

                            if (dt.Rows[i]["EDIGBN"].ToString().Trim() == "3")
                            {
                                dblJAmt = dblJAmt + VB.Fix((int)(VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()) / 10)) * 10;
                            }
                            else
                            {
                                if (VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim()) == 0)
                                {
                                    dblJAmt = dblJAmt + VB.Fix((int)(VB.Val(dt.Rows[i]["JAMT"].ToString().Trim()) / 10)) * 10;
                                }
                                else
                                {
                                    dblJAmt = dblJAmt + VB.Val(dt.Rows[i]["EDITAMT"].ToString().Trim());
                                }
                            }
                        }
                    }

                    #region GoSub OPD_MIR_SANID_INSERT

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     ROWID";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                    SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND BI = '31' ";
                    SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt1.Rows.Count == 0)
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                        SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                        SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + "         '31', ";
                        SQL = SQL + ComNum.VBLF + "         'O', ";
                        SQL = SQL + ComNum.VBLF + "         '3', ";
                        SQL = SQL + ComNum.VBLF + "         '" + dblJAmt + "', ";
                        SQL = SQL + ComNum.VBLF + "         '4' ";
                        SQL = SQL + ComNum.VBLF + "     )";
                    }
                    else
                    {
                        SQL = "";
                        SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                        SQL = SQL + ComNum.VBLF + "     SET";
                        SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dblJAmt + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }

                    dt1.Dispose();
                    dt1 = null;

                    #endregion

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                if (GstrJong == "0" || GstrJong == "4")
                {
                    #region GoSub OPD_MIR_TAID

                    progressBar1.Value = 0;

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.MISUID, B.SNAME, SUM(A.AMT2) AS AMT2 ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_IDMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.MIRYYMM = '" + GstrYYMM + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O'";
                    SQL = SQL + ComNum.VBLF + "         AND A.CLASS = '07' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.AMT2 <> '0' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.TONGGBN = '1' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.MISUID = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.MISUID, B.SNAME";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        progressBar1.Maximum = dt.Rows.Count - 1;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            progressBar1.Value = i;

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["misuid"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '52' ";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4'";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                                SQL = SQL + ComNum.VBLF + "     (YYMM, PANO, SNAME, BI, IPDOPD, SUBI, JEPJAMT, GUBUN)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         '" + GstrYYMM + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["MisuID"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["SNAME"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '52', ";
                                SQL = SQL + ComNum.VBLF + "         'O', ";
                                SQL = SQL + ComNum.VBLF + "         '4', ";
                                SQL = SQL + ComNum.VBLF + "         '" + dt.Rows[i]["AMT2"].ToString().Trim() + "', ";
                                SQL = SQL + ComNum.VBLF + "         '4' ";
                                SQL = SQL + ComNum.VBLF + "     )";
                            }
                            else
                            {
                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         JEPJAMT = '" + dt.Rows[i]["AMT2"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";
                            }

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                #region GoSub MISU_BALCHECK_PANO_REMARK MISU_BALCHECK_PANO 청구금액 0 인경우 REMARK사항 표시

                progressBar1.Value = 0;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PANO, BI, ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '1' "; } //보험
                if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '2' "; } //보호
                if (GstrJong == "3") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '3' "; } //산재
                if (GstrJong == "4") { SQL = SQL + ComNum.VBLF + "         AND SUBI = '4' "; } //자보

                SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                SQL = SQL + ComNum.VBLF + "         AND JEPJAMT = '0' ";
                SQL = SQL + ComNum.VBLF + "         AND REMARK IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i;
                        strRemark = "";

                        //업무의뢰서 요청으로 2006-08-28 尹
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE, 'YYYY-MM-DD') AS BDATE, REMARK";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_HOANBUL ";
                        SQL = SQL + ComNum.VBLF + "     WHERE BDATE >= TO_DATE('" + GstrFDate + "', 'YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND BDATE <= TO_DATE('" + GstrTDate + "', 'YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                strRemark = strRemark + dt1.Rows[k]["BDATE"].ToString().Trim() + ":" + dt1.Rows[k]["REMARK"].ToString().Trim() + VB.Space(4);
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.ORDERCODE AS ORDERCODE, TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, B.ORDERNAMES AS ORDERNAMES";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_MED + "OCS_ORDERCODE B ";
                        SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE >= TO_DATE('" + GstrFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.BDATE <= TO_DATE('" + GstrTDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "         AND A.PTNO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "         AND A.BI = '" + dt.Rows[i]["BI"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE LIKE '$$%' ";
                        SQL = SQL + ComNum.VBLF + "         AND A.ORDERCODE = B.ORDERCODE ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (k = 0; k < dt1.Rows.Count; k++)
                            {
                                strRemark = strRemark + dt1.Rows[k]["BDATE"].ToString().Trim() + ":" + dt1.Rows[k]["ORDERNAMES"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strRemark != "")
                        {
                            //2021-02-23 변환에러로 수정작업
                            if (Encoding.Default.GetBytes(strRemark).Length > 100)
                            {
                                byte[] strByte = Encoding.Default.GetBytes(strRemark);

                                strRemark = Encoding.Default.GetString(strByte, 0, 100);
                            }

                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                strTime2 = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(pDbCon, "T"), "M", ":");

                ComFunc.MsgBox(strTime1 + " : " + strTime2);
                
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool Tewon_Mir_check2_Remark(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strPano = "";
            string strBi = "";
            string strRemark = "";

            progressBar1.Value = 0;

            try
            {
                //청구 보류내역 참고사항 READ
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.BI, A.JOHAP ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID A";

                if (VB.Val(GstrYYMM) <= 200212)
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE A.YYMM = '" + GstrYYMM + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE (A.BYYMM = '" + GstrYYMM + "' OR A.YYMM = '" + GstrYYMM + "' ) "; //2008/10/14 심사과 김준수샘 요청
                }

                SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'I'";
                SQL = SQL + ComNum.VBLF + "         AND A.UPCNT1 = '9' ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND A.JOHAP <> '5'"; }
                if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND A.JOHAP = '5'"; }

                SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, A.SNAME, A.BI, A.JOHAP ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i;

                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strBi = dt.Rows[i]["BI"].ToString().Trim();
                        strRemark = "";

                        //참고내역read
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     WRTNO, REMARK";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSDTL ";
                        SQL = SQL + ComNum.VBLF + "     WHERE WRTNO IN";
                        SQL = SQL + ComNum.VBLF + "                 (SELECT";
                        SQL = SQL + ComNum.VBLF + "                     WRTNO";
                        SQL = SQL + ComNum.VBLF + "                 FROM " + ComNum.DB_PMPA + "MIR_INSID ";

                        if (VB.Val(GstrYYMM) <= 200212)
                        {
                            SQL = SQL + ComNum.VBLF + "                     WHERE YYMM = '" + GstrYYMM + "'";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "                     WHERE (BYYMM = '" + GstrYYMM + "' OR YYMM = '" + GstrYYMM + "' ) "; //2008/10/14 심사과 김준수샘 요청
                        }

                        SQL = SQL + ComNum.VBLF + "                         AND PANO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "                         AND BI = '" + strBi + "' ";
                        SQL = SQL + ComNum.VBLF + "                         AND IPDOPD = 'I' ";

                        if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "                         AND JOHAP <> '5' "; }
                        if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "                         AND JOHAP = '5' "; }

                        SQL = SQL + ComNum.VBLF + "                         AND UPCNT1 = '9')";
                        SQL = SQL + ComNum.VBLF + "         AND SUNEXT <> '########' ";
                        SQL = SQL + ComNum.VBLF + "         AND REMARK IS NOT NULL ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["REMARK"].ToString().Trim() != "")
                            {
                                strRemark = dt1.Rows[0]["REMARK"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strRemark != "")
                        {
                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID, REMARK";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                            SQL = SQL + ComNum.VBLF + "     WHERE YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "'";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '" + strBi + "'";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '1' "; //퇴원
                            SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'I' ";
                            SQL = SQL + ComNum.VBLF + "         AND (REMARK IS NULL OR RTRIM(REMARK) = '')";
                            
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                //2021-02-23 변환에러로 수정작업
                                if(Encoding.Default.GetBytes(strRemark).Length > 100)
                                {
                                    byte[] strByte = Encoding.Default.GetBytes(strRemark);

                                    strRemark = Encoding.Default.GetString(strByte, 0, 100);
                                }

                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool OPD_MIR_CHECK_REMARK(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strPano = "";
            string strBi = "";
            string strRemark = "";

            progressBar1.Value = 0;

            try
            {
                //청구 보류내역 참고사항 READ
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.BI, A.JOHAP, A.DEPTCODE1, A.JINDATE1, A.WRTNO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID A";

                if (VB.Val(GstrYYMM) <= 200212)
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE A.YYMM = '" + GstrYYMM + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE ( A.BYYMM = '" + GstrYYMM + "' OR A.YYMM = '" + GstrYYMM + "' ) "; //2008/10/14 심사과 김준수샘 요청
                }
                    
                SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O'";
                //SQL = SQL + ComNum.VBLF + "         AND A.WRTNO IN ('4659976')";
                SQL = SQL + ComNum.VBLF + "         AND A.UPCNT1 = '9' ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND A.JOHAP <> '5'"; }
                if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND A.JOHAP = '5'"; }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i;

                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strBi = dt.Rows[i]["BI"].ToString().Trim();
                        strRemark = "";

                        //참고내역read
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     WRTNO, REMARK";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSDTL ";
                        SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = '" + dt.Rows[i]["WRTNO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND SUNEXT <> '########' ";
                        SQL = SQL + ComNum.VBLF + "         AND REMARK IS NOT NULL  ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["REMARK"].ToString().Trim() != "")
                            {
                                strRemark = dt1.Rows[0]["REMARK"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strRemark != "")
                        {
                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                            SQL = SQL + ComNum.VBLF + "     WHERE  YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "'";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '" + strBi + "'";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";
                            SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                            SQL = SQL + ComNum.VBLF + "         AND (REMARK IS NULL OR RTRIM(REMARK) = '')";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                //2021-02-23 변환에러로 수정작업
                                if (Encoding.Default.GetBytes(strRemark).Length > 100)
                                {
                                    byte[] strByte = Encoding.Default.GetBytes(strRemark);

                                    strRemark = Encoding.Default.GetString(strByte, 0, 100);
                                }

                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool OPD_MIR_자동점검_REMARK(PsmhDb pDbCon)
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strPano = "";
            string strBi = "";
            string strRemark = "";

            progressBar1.Value = 0;

            try
            {
                //청구 보류내역 참고사항 READ
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.BI, A.JOHAP, A.DEPTCODE1, A.JINDATE1, A.WRTNO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSID A";

                if (VB.Val(GstrYYMM) <= 200212)
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE A.YYMM = '" + GstrYYMM + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE ( A.BYYMM = '" + GstrYYMM + "' OR A.YYMM = '" + GstrYYMM + "' ) "; //2008/10/14 심사과 김준수샘 요청
                }

                SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O'";
                //SQL = SQL + ComNum.VBLF + "         AND A.WRTNO IN ('4659976')";
                SQL = SQL + ComNum.VBLF + "         AND A.UPCNT1 = '9' ";

                if (GstrJong == "1") { SQL = SQL + ComNum.VBLF + "         AND A.JOHAP <> '5'"; }
                if (GstrJong == "2") { SQL = SQL + ComNum.VBLF + "         AND A.JOHAP = '5'"; }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    progressBar1.Maximum = dt.Rows.Count - 1;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        progressBar1.Value = i;

                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strBi = dt.Rows[i]["BI"].ToString().Trim();
                        strRemark = "(자동점검)";

                        //참고내역read
                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     WRTNO, REMARK";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MIR_INSDTL ";
                        SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = '" + dt.Rows[i]["WRTNO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND SUNEXT <> '########' ";
                        SQL = SQL + ComNum.VBLF + "         AND REMARK IS NOT NULL  ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["REMARK"].ToString().Trim() != "")
                            {
                                strRemark = dt1.Rows[0]["REMARK"].ToString().Trim();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (strRemark != "")
                        {
                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO ";
                            SQL = SQL + ComNum.VBLF + "     WHERE  YYMM = '" + GstrYYMM + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "'";
                            SQL = SQL + ComNum.VBLF + "         AND BI = '" + strBi + "'";
                            SQL = SQL + ComNum.VBLF + "         AND GUBUN = '4' ";
                            SQL = SQL + ComNum.VBLF + "         AND IPDOPD = 'O' ";
                            SQL = SQL + ComNum.VBLF + "         AND (REMARK IS NULL OR RTRIM(REMARK) = '')";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return rtnVal;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                //2021-02-23 변환에러로 수정작업
                                if (Encoding.Default.GetBytes(strRemark).Length > 100)
                                {
                                    byte[] strByte = Encoding.Default.GetBytes(strRemark);

                                    strRemark = Encoding.Default.GetString(strByte, 0, 100);
                                }

                                SQL = "";
                                SQL = "UPDATE " + ComNum.DB_PMPA + "MISU_BALCHECK_PANO";
                                SQL = SQL + ComNum.VBLF + "     SET";
                                SQL = SQL + ComNum.VBLF + "         REMARK = '" + strRemark + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + dt1.Rows[0]["ROWID"].ToString().Trim() + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("OPD_MIR_CHECK 자료를 등록중 오류발생");
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public static string ByteSubstring(string Data, int StartIndex, int ByteLength)
        {
            String str = "";

            byte[] TEMP = Encoding.Default.GetBytes(Data);

            str = Encoding.UTF8.GetString(TEMP, StartIndex, ByteLength);

            return str;
        }

    }
}
