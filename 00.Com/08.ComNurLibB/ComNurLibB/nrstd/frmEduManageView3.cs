using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmEduManageView3.cs
    /// Description     : 교육관리조회(전체및개인별조회)
    /// Author          : 박창욱
    /// Create Date     : 2018-01-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    ///
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm교육관리조회3.frm(Frm교육관리조회3.frm) >> frmEduManageView3.cs 폼이름 재정의" />	
    public partial class frmEduManageView3 : Form
    {
        string strBuName = "";
        string FstrUse_All = "";
        string strSysDate = "";
        string gsWard = "";

        public frmEduManageView3()
        {
            InitializeComponent();
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

            strTitle = "교육관리 리스트";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조회기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSeach2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nRow = 0;
            int nSum = 0;
            int nSum2 = 0;
            int nSum3 = 0;
            string strBuCode = "";
            string strBuCode1 = "";

            strBuName = "";

            for (i = 1; i < ssBuse_Sheet1.NonEmptyRowCount; i++)
            {
                if (Convert.ToBoolean(ssBuse_Sheet1.Cells[i - 1, 0].Value) == true)
                {
                    strBuCode += ssBuse_Sheet1.Cells[i - 1, 1].Text.Trim() + ",";
                    strBuName += ssBuse_Sheet1.Cells[i - 1, 2].Text.Trim() + ",";
                }
            }

            ssView.Enabled = false;
            ssView_Sheet1.RowCount = 0;

            nRow = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                for (k = 1; k < VB.L(strBuCode, ","); k++)
                {
                    strBuCode1 = VB.Pstr(strBuCode, ",", k);
                    if (VB.Val(VB.Pstr(strBuCode, ",", k)) > 40000)
                    {
                        strBuCode1 = ComFunc.LeftH(VB.Pstr(strBuCode, ",", k), 5) + "%";
                    }

                    //참석자를 Display
                    SQL = "";
                    SQL = "SELECT c.Code, a.Sabun,TO_CHAR(a.IpsaDay,'YYYY-MM-DD') IpsaDay,a.Jik,a.KorName,a.Buse,b.Name BuseName,c.Name JikName ";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "INSA_MST a,";
                    SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_BUSE b,";
                    SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_ERP + "INSA_CODE c ";
                    SQL = SQL + ComNum.VBLF + " WHERE a.Buse like '" + strBuCode1 + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND a.IpsaDay<=TO_DATE('" + strSysDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND (a.ToiDay IS NULL OR a.ToiDay>=TO_DATE('" + strSysDate + "','YYYY-MM-DD')) ";
                    SQL = SQL + ComNum.VBLF + "   AND TRUNC(a.Sabun) < 90000 ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Buse=b.BuCode(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND a.Jik=c.Code(+) ";
                    SQL = SQL + ComNum.VBLF + "   AND c.Gubun='2' "; //직책
                    SQL = SQL + ComNum.VBLF + " ORDER BY c.Code, a.Sabun, a.KorName ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ssView_Sheet1.RowCount = dt.Rows.Count + nRow + 2;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            nRow += 1;
                            ssView_Sheet1.Cells[nRow - 1, 0].Text = " " + dt.Rows[i]["BuseName"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = " " + dt.Rows[i]["JikName"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Sabun"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["KorName"].ToString().Trim().Replace(" ", "");
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["IpsaDay"].ToString().Trim();

                            switch (dt.Rows[i]["Jik"].ToString().Trim())
                            {
                                case "32":
                                    ssView_Sheet1.Cells[nRow - 1, 5].Text = "80";   //수간호사
                                    break;
                                case "33":
                                    ssView_Sheet1.Cells[nRow - 1, 5].Text = "80";   //책임간호사
                                    break;
                                case "34":
                                    ssView_Sheet1.Cells[nRow - 1, 5].Text = "60";   //간호사
                                    break;
                                case "36":
                                    ssView_Sheet1.Cells[nRow - 1, 5].Text = "40";   //책임간호조무사
                                    break;
                                case "37":
                                    ssView_Sheet1.Cells[nRow - 1, 5].Text = "40";   //간호조무사
                                    break;
                                case "59":
                                case "91":
                                    ssView_Sheet1.Cells[nRow - 1, 5].Text = "40";   //응급구조사,치료사
                                    break;
                            }

                            nSum3 += (int)VB.Val(ssView_Sheet1.Cells[nRow - 1, 5].Text.Trim());

                            ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["Buse"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["Jik"].ToString().Trim();

                            //여부를 읽음
                            SQL = "";
                            SQL = "SELECT SUM(TO_NUMBER(Jumsu)) Jumsu,COUNT(*) CNT";
                            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_EDU_MST ";
                            SQL = SQL + ComNum.VBLF + " WHERE TRIM(Sabun)=" + dt.Rows[i]["SABUN"].ToString().Trim();
                            SQL = SQL + ComNum.VBLF + "   AND SDate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND SDate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND ((GUBUN = '2' AND SIGN = '1') OR GUBUN = '1')";

                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                Cursor.Current = Cursors.Default;
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                if (VB.Val(dt1.Rows[0]["Jumsu"].ToString().Trim()) > 0)
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt1.Rows[0]["Jumsu"].ToString().Trim();
                                    nSum += (int)VB.Val(dt1.Rows[0]["Jumsu"].ToString().Trim());
                                }
                                else
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 6].Text = "0";
                                }
                                ssView_Sheet1.Cells[nRow - 1, 9].Text = dt1.Rows[0]["CNT"].ToString().Trim();
                                nSum2 += (int)VB.Val(dt1.Rows[0]["CNT"].ToString().Trim());
                            }
                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 4].Text = "합계";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 5].Text = nSum3.ToString();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 6].Text = nSum.ToString();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 9].Text = nSum2.ToString();

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = "평균";
                if (nSum > 0)
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = (nSum * 100 / nSum3).ToString("###,###.0");
                }

                ssView.Enabled = true;
                ssView.Focus();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            Search_Data();
        }

        void Search_Data()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRow = 0;
            int nCNT = 0;
            string strBuCode = "";
            string strBuse = "";
            string strBuseName = "";

            ssBuse_Sheet1.RowCount = 0;
            ssBuse_Sheet1.RowCount = 50;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT a.Buse,b.Name BuName,COUNT(*) CNT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST a, " + ComNum.DB_PMPA + "BAS_BUSE b ";
                SQL = SQL + ComNum.VBLF + "WHERE a.IpsaDay<=TO_DATE('" + strSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND (a.ToiDay IS NULL OR a.ToiDay>=TO_DATE('" + strSysDate + "','YYYY-MM-DD')) ";
                if (FstrUse_All == "OK")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.BUSE IN ( ";
                    SQL = SQL + ComNum.VBLF + "       SELECT MATCH_CODE";
                    SQL = SQL + ComNum.VBLF + "         FROM " + ComNum.DB_PMPA + "NUR_CODE";
                    SQL = SQL + ComNum.VBLF + "        WHERE GUBUN = '2'";
                    SQL = SQL + ComNum.VBLF + "          AND GBUSE = 'Y'";
                    SQL = SQL + ComNum.VBLF + "          AND MATCH_CODE IS NOT NULL)";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND a.Buse='" + gsWard + "' ";
                }
                SQL = SQL + ComNum.VBLF + "   AND a.Buse=b.BuCode(+) ";
                SQL = SQL + ComNum.VBLF + " GROUP BY a.Buse,b.Name ";
                SQL = SQL + ComNum.VBLF + " ORDER BY a.Buse ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboBuse.Items.Clear();
                cboBuse.Items.Add(" ");
                cboBuse.Items.Add("*.전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nCNT = (int)VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                    strBuse = dt.Rows[i]["Buse"].ToString().Trim();
                    strBuseName = dt.Rows[i]["BuName"].ToString().Trim();
                    cboBuse.Items.Add(strBuseName + VB.Space(50) + strBuse);

                    if (nCNT > 0)
                    {
                        nRow += 1;
                        if (nRow > ssBuse_Sheet1.RowCount)
                        {
                            ssBuse_Sheet1.RowCount = nRow;
                        }

                        strBuCode = dt.Rows[i]["Buse"].ToString().Trim();
                        ssBuse_Sheet1.Cells[nRow - 1, 1].Text = strBuse;
                        ssBuse_Sheet1.Cells[nRow - 1, 2].Text = " " + strBuseName;
                    }
                }
                dt.Dispose();
                dt = null;

                cboBuse.Items.Add("정형외과(일반).100251");
                ssBuse_Sheet1.RowCount = nRow;
                ssBuse_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssBuse.Enabled = true;

                if (FstrUse_All == "OK")
                {
                    cboBuse.SelectedIndex = 1;
                }
                else
                {
                    cboBuse.SelectedIndex = 2;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmEduManageView3_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strSabun = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            strSabun = clsType.User.Sabun;

            dtpFDate.Value = Convert.ToDateTime(VB.Left(strSysDate, 4) + "-01-01");

            ssView_Sheet1.Columns[7].Visible = false;
            ssView_Sheet1.Columns[8].Visible = false;

            ssView.Enabled = false;
            ssBuse.Enabled = false;

            FstrUse_All = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun ='1'";
                SQL = SQL + ComNum.VBLF + "   AND Remark ='" + strSabun + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrUse_All = "OK";
                }

                SQL = "";
                SQL = "SELECT a.Buse,b.Name BuName ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST a, " + ComNum.DB_PMPA + "BAS_BUSE b ";
                SQL = SQL + ComNum.VBLF + "WHERE a.IpsaDay<=TO_DATE('" + strSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND (a.ToiDay IS NULL OR a.ToiDay>=TO_DATE('" + strSysDate + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "  AND a.Buse=b.BuCode(+) ";
                SQL = SQL + ComNum.VBLF + "  AND a.Sabun='" + strSabun + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                gsWard = "";

                if (dt.Rows.Count > 0)
                {
                    gsWard = dt.Rows[0]["Buse"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

                Search_Data();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            string GstrRetValue = "";

            GstrRetValue = ssView_Sheet1.Cells[e.Row, 2].Text.Trim() + "^^" + dtpFDate.Value.ToString("yyyy-MM-dd") + "^^" + dtpTDate.Value.ToString("yyyy-MM-dd") + "^^";

            if (GstrRetValue != "")
            {
                //Frm교육관리조회5
                frmEduManageView5 frm = new frmEduManageView5(GstrRetValue);
                frm.ShowDialog();
            }
        }

        private void ssBuse_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column != 0)
            {
                return;
            }

            if (Convert.ToBoolean(ssBuse_Sheet1.Cells[e.Row, e.Column].Value) == true)
            {
                ssBuse_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(128, 128, 255);
            }
            else
            {
                ssBuse_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(255, 255, 255);
            }
        }

        private void ssBuse_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssBuse_Sheet1.RowCount == 0)
            {
                return;
            }

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nSum = 0;
            int nSum2 = 0;
            int nSum3 = 0;
            string strBuCode = "";
            string strSabun = "";
            string strBuse = "";

            strBuCode = ssBuse_Sheet1.Cells[e.Row, 1].Text.Trim();
            strBuName = ssBuse_Sheet1.Cells[e.Row, 2].Text.Trim();

            if (VB.Val(clsType.User.Sabun) <= 99999)
            {
                strSabun = VB.Val(clsType.User.Sabun).ToString("00000");
            }
            else
            {
                strSabun = VB.Val(clsType.User.Sabun).ToString("000000");
            }

            ssView.Enabled = false;
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //참석자를 Display
                SQL = "";
                SQL = "SELECT c.Code, a.Sabun,TO_CHAR(a.IpsaDay,'YYYY-MM-DD') IpsaDay,a.Jik,a.KorName,a.Buse,b.Name BuseName,c.Name JikName ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_ERP + "INSA_MST a,";
                SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_BUSE b,";
                SQL = SQL + ComNum.VBLF + "       " + ComNum.DB_ERP + "INSA_CODE c ";
                SQL = SQL + ComNum.VBLF + " WHERE a.Buse = '" + strBuCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.IpsaDay<=TO_DATE('" + strSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND (a.ToiDay IS NULL OR a.ToiDay>=TO_DATE('" + strSysDate + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "   AND a.Sabun < '90000' ";
                SQL = SQL + ComNum.VBLF + "   AND a.Buse=b.BuCode(+) ";
                SQL = SQL + ComNum.VBLF + "   AND a.Jik=c.Code(+) ";
                SQL = SQL + ComNum.VBLF + "   AND c.Gubun='2' "; //직책
                if (FstrUse_All != "OK")
                {
                    SQL = SQL + ComNum.VBLF + "   AND trunc(a.Sabun) = " + strSabun;
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY c.Code, a.Sabun, a.KorName ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count + 1;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBuse = dt.Rows[i]["Buse"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 0].Text = " " + dt.Rows[i]["BuseName"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = " " + dt.Rows[i]["JikName"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sabun"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["KorName"].ToString().Trim().Replace(" ", "");
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["IpsaDay"].ToString().Trim();

                    switch (dt.Rows[i]["Jik"].ToString().Trim())
                    {
                        case "32":
                            ssView_Sheet1.Cells[i, 5].Text = "32";   //수간호사
                            break;
                        case "33":
                            ssView_Sheet1.Cells[i, 5].Text = "32";   //책임간호사
                            break;
                        case "34":
                            ssView_Sheet1.Cells[i, 5].Text = "32";   //간호사
                            break;
                        case "53":
                        case "57":
                            ssView_Sheet1.Cells[i, 5].Text = "24";   //병리사,치위생사
                            break;
                        case "36":
                            ssView_Sheet1.Cells[i, 5].Text = "24";   //책임간호조무사
                            break;
                        case "37":
                            switch (strBuse)
                            {
                                //병동일 경우
                                case "033113":
                                case "033122":
                                case "033114":
                                case "033116":
                                case "033117":
                                case "033118":
                                case "033119":
                                case "033121":
                                case "033120":
                                case "033106":
                                case "033104":
                                case "033105":
                                case "033123":
                                case "033102":
                                case "101743":
                                case "101746":
                                case "101747":
                                case "101748":
                                case "101749":
                                case "101750":
                                case "101751":
                                    ssView_Sheet1.Cells[i, 5].Text = "24";  //간호조무사(병동)
                                    break;
                                default:
                                    ssView_Sheet1.Cells[i, 5].Text = "24";  //간호조무사
                                    break;
                            }
                            break;
                        case "91":
                            ssView_Sheet1.Cells[i, 5].Text = "24";   //치료사
                            break;
                        case "59":
                            //응급구조사 병동일 경우 60점, 외래일 경우 40점
                            switch (strBuse)
                            {
                                case "033113":
                                case "033122":
                                case "033114":
                                case "033116":
                                case "033117":
                                case "033118":
                                case "033119":
                                case "033121":
                                case "033120":
                                case "033106":
                                case "033104":
                                case "033105":
                                case "033123":
                                case "033102":
                                case "101743":
                                case "101746":
                                case "101747":
                                case "101748":
                                case "101749":
                                case "101750":
                                case "101751":
                                    ssView_Sheet1.Cells[i, 5].Text = "24";  //응급구조사
                                    break;
                                default:
                                    ssView_Sheet1.Cells[i, 5].Text = "24";  //응급구조사
                                    break;
                            }
                            break;
                        default:
                            ssView_Sheet1.Cells[i, 5].Text = "24";
                            break;
                    }
                    nSum3 += (int)VB.Val(ssView_Sheet1.Cells[i, 5].Text);

                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Buse"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Jik"].ToString().Trim();

                    //      (GUBUN)교육종류:   01.병동교육 02.감염교육 03.QI교육
                    //      04.CS교육 05.CPR교육 06.학술강좌
                    //      07.간호부주최 직무교육 08.전직원교육 09.특강(간협)
                    //      10.연수교육 11.10대질환 12.보수교육 13.기타Report
                    //      14.강사활동(교육) 15.프리셉터교육 16.Cyber 교육
                    //      17.승진자교육 18.기타"

                    //여부를 읽음
                    SQL = "";
                    SQL = " SELECT FLOOR(SUM(TO_NUMBER(EDUTIME)/60) ) AS hh , SUM(TO_NUMBER(EDUTIME)) EDUTIME ,";
                    SQL = SQL + ComNum.VBLF + " ROUND(MOD(SUM(TO_NUMBER(EDUTIME)/60), 1),1 ) *60 AS mi   , ";
                    SQL = SQL + ComNum.VBLF + " COUNT(*) CNT ";
                    SQL = SQL + ComNum.VBLF + " FROM (";
                    SQL = SQL + ComNum.VBLF + " SELECT BUCODE, SABUN, '간호부교육' GUBUN, EDUNAME, FRDATE, TODATE,translate(EDUTIME, '0123456789' ||EDUTIME , '0123456789')  EDUTIME, PLACE, JUMSU, '' GIKWAN, MAN, '' COM";
                    SQL = SQL + ComNum.VBLF + " From " + ComNum.DB_PMPA + "NUR_EDU_MST";
                    SQL = SQL + ComNum.VBLF + " WHERE (SABUN = '" + dt.Rows[i]["SABUN"].ToString().Trim() + "' OR SABUN = '" + dt.Rows[i]["SABUN"].ToString().Trim() + "')";
                    SQL = SQL + ComNum.VBLF + "   AND FRDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND FRDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " Union All";
                    SQL = SQL + ComNum.VBLF + " SELECT A.BUSE, A.SABUN, '사이버교육' GUBUN, NVL(B.EDUTITLE, GRADE ) EDUTITLE, A.SDATE, A.EDATE,to_char(to_number(translate(A.EDUTIME, '0123456789' ||A.EDUTIME , '0123456789'))*60)   EDUTIME, EDUHOMEPAGE, '' JUMSU, '' GIKWAN,  '' MAN, A.COMPLETION";
                    SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "REPORT_EDU A, " + ComNum.DB_PMPA + "REPORT_EDU_CODE B";
                    SQL = SQL + ComNum.VBLF + " WHERE A.EDUCODE = B.CODE(+)";
                    SQL = SQL + ComNum.VBLF + "   AND A.SABUN = '" + dt.Rows[i]["SABUN"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "   AND CANDATE IS NULL";
                    SQL = SQL + ComNum.VBLF + "   AND A.SDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND A.SDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " Union All";
                    SQL = SQL + ComNum.VBLF + " SELECT B.BUSE, A.SABUN, '필수교육', NAEYONG, FDATE, TDATE, translate(EDUTIME, '0123456789' ||EDUTIME , '0123456789') TIMELESS, JANGSO PLACE, PUNGGA JUMSU, GIKWAN, EDUMAN, '' COM";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MSTW A, " + ComNum.DB_ERP + "INSA_MST B";
                    SQL = SQL + ComNum.VBLF + " Where a.SABUN = b.SABUN";
                    SQL = SQL + ComNum.VBLF + "   AND (A.GUBUN = '1' OR A.GUBUN IS NULL)";
                    SQL = SQL + ComNum.VBLF + "   AND A.SABUN = '" + dt.Rows[i]["SABUN"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "   AND A.FDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND A.FDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " Union All";
                    SQL = SQL + ComNum.VBLF + " SELECT B.BUSE, A.SABUN, '특성화교육', NAEYONG, FDATE, TDATE, translate(EDUTIME, '0123456789' ||EDUTIME , '0123456789') TIMELESS, JANGSO PLACE, PUNGGA JUMSU, GIKWAN, EDUMAN, '' COM";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MSTW A, " + ComNum.DB_ERP + "INSA_MST B";
                    SQL = SQL + ComNum.VBLF + " Where a.SABUN = b.SABUN";
                    SQL = SQL + ComNum.VBLF + "   AND A.GUBUN = '2'";
                    SQL = SQL + ComNum.VBLF + "   AND A.SABUN = '" + dt.Rows[i]["SABUN"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "   AND A.FDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND A.FDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " Union All";
                    SQL = SQL + ComNum.VBLF + " SELECT B.BUSE, A.SABUN, '인증제직무교육', NAEYONG, FDATE, TDATE,  translate(EDUTIME, '0123456789' ||EDUTIME , '0123456789') TIMELESS, JANGSO PLACE, PUNGGA JUMSU, GIKWAN, EDUMAN, '' COM";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MSTW A, " + ComNum.DB_ERP + "INSA_MST B";
                    SQL = SQL + ComNum.VBLF + " Where a.SABUN = b.SABUN";
                    SQL = SQL + ComNum.VBLF + "   AND A.GUBUN = '3'";
                    SQL = SQL + ComNum.VBLF + "   AND A.SABUN = '" + dt.Rows[i]["SABUN"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "   AND A.FDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND A.FDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " Union All";
                    SQL = SQL + ComNum.VBLF + " SELECT B.BUSE, A.SABUN, '보수교육', NAEYONG, FDATE, TDATE,  translate(EDUTIME, '0123456789' ||EDUTIME , '0123456789') TIMELESS, JANGSO PLACE, PUNGGA JUMSU, GIKWAN, EDUMAN, '' COM";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MSTW A, " + ComNum.DB_ERP + "INSA_MST B";
                    SQL = SQL + ComNum.VBLF + " Where a.SABUN = b.SABUN";
                    SQL = SQL + ComNum.VBLF + "   AND A.GUBUN = '4'";
                    SQL = SQL + ComNum.VBLF + "   AND A.SABUN = '" + dt.Rows[i]["SABUN"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "   AND A.FDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "   AND A.FDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " Union All";
                    SQL = SQL + ComNum.VBLF + " SELECT B.BUSE, A.SABUN, '인사등록교육', NAEYONG, FDATE, TDATE, EDUTIME TIMELESS, JANGSO PLACE, '' JUMSU, GIKWAN, '' MAN, '' COM";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MSTF A, " + ComNum.DB_ERP + "INSA_MST B";
                    SQL = SQL + ComNum.VBLF + " Where a.SABUN = b.SABUN";
                    SQL = SQL + ComNum.VBLF + " AND A.SABUN = '" + dt.Rows[i]["SABUN"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + " AND A.FDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " AND A.FDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + " ) A, " + ComNum.DB_PMPA + "BAS_BUSE B";
                    SQL = SQL + ComNum.VBLF + " Where a.BuCode = b.BuCode";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        if (VB.Val(dt1.Rows[0]["EDUTIME"].ToString().Trim()) > 0)
                        {
                            ssView_Sheet1.Cells[i, 6].Text = dt1.Rows[0]["hh"].ToString().Trim() + "." + dt1.Rows[0]["mi"].ToString().Trim();
                            nSum += (int)VB.Val(dt1.Rows[0]["EDUTIME"].ToString().Trim());
                            ssView_Sheet1.Cells[i, 10].Text = dt1.Rows[0]["EDUTIME"].ToString().Trim();
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 6].Text = "0";
                        }
                        ssView_Sheet1.Cells[i, 9].Text = dt1.Rows[0]["CNT"].ToString().Trim();
                        nSum2 += (int)VB.Val(dt1.Rows[0]["CNT"].ToString().Trim());
                    }
                    dt1.Dispose();
                    dt1 = null;
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = "합계";
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = nSum3.ToString();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = VB.Fix(nSum / 60) + "." + (nSum % 60);
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = nSum2.ToString();

                ssView.Enabled = true;
                ssView.Focus();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
