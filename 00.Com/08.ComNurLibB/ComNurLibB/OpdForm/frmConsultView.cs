using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmConsultView.cs
    /// Description     : Consult조회
    /// Author          : 박창욱
    /// Create Date     : 2017-12-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\emr\emrprt\FrmConsultView.frm(FrmConsultView.frm) >> frmConsultView.cs 폼이름 재정의" />
    public partial class frmConsultView : Form
    {
        ComFunc CF = null;
        private ContextMenu PopupMenu = null;
        private string FstrROWID = "";
        private string[] sResultRemark1 = new string[101];
        private int nPageTot = 0;

        private string strPANO = "";
        private string strSname = "";
        private string strBDATE = "";
        private string strFrDATE = "";
        private string strToDATE = "";
        private string strGubun = "";

        private string strSex = "";
        private string strJumin = "";
        private string strBi = "";
        private string strRoom = "";
        private string strDeptCode = "";
        private string strDrname = "";
        private string strTel = "";
        private string strIPDNO = "";
        private string strWard = "";
        private string strAge = "";
        private string strToDr = "";
        private string strToDept = "";

        private string strGbSTS = "";

        private bool bSimsa = false;

        public frmConsultView()
        {
            InitializeComponent();
        }

        public frmConsultView(string argPano, string argDate)
        {
            InitializeComponent();
            strPANO = argPano;
            strFrDATE = argDate;
            strToDATE = argDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argPano"></param>
        /// <param name="argFrDate"></param>
        /// <param name="argToDate"></param>
        /// <param name="argGubun">전체:0, 미처리:1, 완료:2</param>
        public frmConsultView(string argPano, string argFrDate, string argToDate, string argGubun)
        {
            InitializeComponent();
            strPANO = argPano;
            strFrDATE = argFrDate;
            strToDATE = argToDate;
            strGubun = argGubun;
        }

        /// <summary>
        /// 심사팀용
        /// </summary>
        /// <param name="argPano"></param>
        /// <param name="argFrDate"></param>
        /// <param name="argToDate"></param>
        /// <param name="argGubun"></param>
        /// <param name="bSimsa">심사팀 여부</param>
        public frmConsultView(string argPano, string argFrDate, string argToDate, string argGubun, bool bSimsa)
        {
            InitializeComponent();
            strPANO = argPano;
            strFrDATE = argFrDate;
            strToDATE = argToDate;
            strGubun = argGubun;
            this.bSimsa = bSimsa;
        }

        void Print_String(string argString)
        {
            int j = 0;
            int k = 0;
            int nLen = 0;
            string strfirst = "";
            string strlast = "";
            string sresultremark = "";

            for (j = 0; j < 101; j++)
            {
                sResultRemark1[j] = "";
            }

            j = 0;

            nPageTot = 0;
            strlast = "";
            strfirst = argString;
            nLen = (int)ComFunc.LenH(strfirst.Trim());

            for (k = 1; k < 4001; k++)
            {
                sresultremark = VB.Mid(strfirst, k, 1);
                strlast += VB.Mid(strfirst, k, 1);

                if (sresultremark == ComNum.VBLF || strlast.Length >= 75)
                {
                    j += 1;
                    if (strlast == ComNum.VBLF)
                    {
                        sResultRemark1[j] = "";
                        strlast = "";
                    }
                    else if (strlast.Length >= 75 && VB.Left(strlast, 1) != ComNum.VBLF)
                    {
                        sResultRemark1[j] = strlast;
                        strlast = "";
                    }
                    else if (strlast.Length >= 75 && VB.Left(strlast, 1) == ComNum.VBLF)
                    {
                        sResultRemark1[j] = VB.Right(strlast, strlast.Length - 1);
                        strlast = "";
                    }
                    else if (VB.Left(strlast, 1) == ComNum.VBLF && k != 1)
                    {
                        strlast = VB.Right(strlast, strlast.Length - 1);
                        sResultRemark1[j] = VB.Left(strlast, strlast.Length - 1);
                        strlast = "";
                    }
                    else if (VB.Right(strlast, 1) == ComNum.VBLF && k != 1)
                    {
                        strlast = VB.Left(strlast, strlast.Length - 1);
                        sResultRemark1[j] = VB.Left(strlast, strlast.Length - 1);
                        strlast = "";
                    }
                    else if (k == 1)
                    {
                        sResultRemark1[j] = VB.Left(strlast, strlast.Length - 1);
                        strlast = "";
                    }
                }
                sresultremark = "";
            }

            j += 1;
            if (strlast.Length >= 1 && VB.Left(strlast, 1) == ComNum.VBLF)
            {
                sResultRemark1[j] = VB.Right(strlast, strlast.Length - 1);
                strlast = "";
            }
            else if (strlast.Length >= 1)
            {
                sResultRemark1[j] = strlast.Trim();
                strlast = "";
            }

            nPageTot = j;
        }

        void Screen_Clear()
        {
            ssView_Sheet1.RowCount = 0;
            txtPano.Text = "";
            lblSName.Text = "";
            txtRemark.Text = "";
            grbConsult.Enabled = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            int nLenT = 0;
            string strText = "";
            string l_str = "";
            string strSDATE = "";
            string strEDATE = "";

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  TO_CHAR(SDATE, 'YYYY-MM-DD HH24:MI') SDATE, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(EDATE,'YYYY-MM-DD HH24:MI') EDATE, ";
                SQL = SQL + ComNum.VBLF + "        AGE, SEX, ROOMCODE, SName, AGE, PTNO, ";
                SQL = SQL + ComNum.VBLF + "        BINPID, TODRCODE , FRDRCODE, FRREMARK , FRDEPTCODE, TOREMARK, TODEPTCODE ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_ITRANSFER ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strSDATE = dt.Rows[0]["SDATE"].ToString().Trim();
                    strEDATE = dt.Rows[0]["EDATE"].ToString().Trim();

                    ssPrint_Sheet1.Cells[3, 1].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["BInpID"].ToString().Trim());
                    ssPrint_Sheet1.Cells[4, 1].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["ToDrCode"].ToString().Trim());
                    ssPrint_Sheet1.Cells[5, 1].Text = dt.Rows[0]["Sname"].ToString().Trim() + VB.Space(1) + "(" + dt.Rows[0]["SEX"].ToString().Trim() +
                                                      "/" + dt.Rows[0]["AGE"].ToString().Trim() + ")";
                    ssPrint_Sheet1.Cells[5, 2].Text = "병록번호 : " + dt.Rows[0]["Ptno"].ToString().Trim();
                    ssPrint_Sheet1.Cells[5, 3].Text = dt.Rows[0]["RoomCode"].ToString().Trim() + "호실";

                    strText = VB.RTrim(dt.Rows[0]["FRREMARK"].ToString().Trim());
                    l_str = strText;
                    nLenT = (int)ComFunc.LenH(l_str);

                    Print_String(strText);

                    for (i = 0; i <= nPageTot; i++)
                    {
                        ssPrint_Sheet1.Cells[7, 0].Text += sResultRemark1[i];
                    }

                    ssPrint_Sheet1.Cells[16, 2].Text = strSDATE.Trim();
                    ssPrint_Sheet1.Cells[16, 3].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["FrDrCode"].ToString().Trim()) + "[" +
                                                       dt.Rows[0]["FRDEPTCODE"].ToString().Trim() + "]";

                    strText = VB.RTrim(dt.Rows[0]["TOREMARK"].ToString().Trim());
                    l_str = strText;
                    nLenT = (int)ComFunc.LenH(l_str);

                    Print_String(strText);

                    for (i = 0; i <= nPageTot; i++)
                    {
                        ssPrint_Sheet1.Cells[19, 0].Text += sResultRemark1[i];
                    }

                    ssPrint_Sheet1.Cells[36, 2].Text = strEDATE.Trim();
                    ssPrint_Sheet1.Cells[36, 3].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["ToDrCode"].ToString().Trim()) + "[" +
                                                       dt.Rows[0]["TODEPTCODE"].ToString().Trim() + "]";
                }

                dt.Dispose();
                dt = null;

                setMargin = new clsSpread.SpdPrint_Margin(0, 0, 0, 0, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, false, false, false, false, false);

                CS.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);

                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_MED + "OCS_ITRANSFER SET GbPrint ='Y'";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID  = '" + FstrROWID.Trim() + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strGBConfirm = "";
            string strGbPrint = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            if (FstrROWID == "")
            {
                ComFunc.MsgBox("작업 오류. 다시 작업하세요.");
                return;
            }

            if (ComFunc.MsgBoxQ("CONSULT 정보를 수정하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            if (rdoConfirm0.Checked == true)
            {
                strGBConfirm = "*";
                strGbPrint = "Y";
            }
            else
            {
                strGBConfirm = "";
                strGbPrint = "";
            }

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_MED + "OCS_ITRANSFER SET ";
                SQL = SQL + ComNum.VBLF + "       GBCONFIRM = '" + strGBConfirm + "',  ";
                SQL = SQL + ComNum.VBLF + "       GBPRINT = '" + strGbPrint + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }
                                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("변경 완료.");
                Cursor.Current = Cursors.Default;

                Search_Data();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
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

            Cursor.Current = Cursors.WaitCursor;
            ssView_Sheet1.RowCount = 0;
                        
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.PTNO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, FRDEPTCODE, FRDRCODE,";
                SQL = SQL + ComNum.VBLF + "       FRREMARK, TODEPTCODE, TODRCODE, TOREMARK, A.ROWID, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(SDATE, 'YYYY-MM-DD HH24:MI') SDATE,  TO_CHAR(EDATE, 'YYYY-MM-DD HH24:MI') EDATE ,";
                SQL = SQL + ComNum.VBLF + "       A.GBCONFIRM, A.GBDEL, B.SNAME, B.AGE, B.SEX, B.WARDCODE ,B.IPDNO, B.ROOMCODE, A.GBSTS ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_MED + "OCS_ITRANSFER A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B ";

                if (rdoDate0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   WHERE SDATE >= TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND SDATE <  TO_DATE('" + dtpToDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   WHERE EDATE >= TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND EDATE <  TO_DATE('" + dtpToDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                }

                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + " AND TODEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' ";
                }

                if (txtPano.Text.Trim() != "" && txtPano.Text.Length == 8)
                {
                    SQL = SQL + ComNum.VBLF + " AND PTNO = '" + txtPano.Text + "' ";
                }

                if (rdoGB2.Checked == true) //완료
                {
                    SQL = SQL + ComNum.VBLF + " AND GBCONFIRM = '*' ";
                }
                else if (rdoGB1.Checked == true) //미완료
                {
                    SQL = SQL + ComNum.VBLF + " AND ( GBCONFIRM IN (' ','','T') OR GBCONFIRM IS NULL ) ";
                }

                SQL = SQL + ComNum.VBLF + "  AND A.GBDEL <>'*'";
                SQL = SQL + ComNum.VBLF + "  AND A.PTNO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "  AND B.ACTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY  A.TODEPTCODE, A.TODRCODE";

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
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["TODEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["TODRCODE"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PtNo"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["sName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["WardCode"].ToString().Trim();

                        if (dt.Rows[i]["GBDel"].ToString().Trim() == "*")
                        {
                            ssView_Sheet1.Cells[i, 8].Text = "삭제";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["GBCONFIRM"].ToString().Trim() == "*" ? "완료" : "";
                        }

                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["GBSTS"].ToString().Trim() == "0" ? "Y" : "N";
                        ssView_Sheet1.Cells[i, 9 + 1].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["FRDRCODE"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 10 + 1].Text = dt.Rows[i]["FRDEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 11 + 1].Text = dt.Rows[i]["sDate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 12 + 1].Text = dt.Rows[i]["FRREMARK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 13 + 1].Text = dt.Rows[i]["edate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 14 + 1].Text = dt.Rows[i]["TOREMARK"].ToString().Trim();

                        ssView_Sheet1.Cells[i, 15 + 1].Text = dt.Rows[i]["GBCONFIRM"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 16 + 1].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 17 + 1].Text = dt.Rows[i]["TODRCODE"].ToString().Trim();

                        ssView_Sheet1.Cells[i, 18 + 1].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 19 + 1].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmConsultView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsSpread methodSpd = new clsSpread();
            CF = new ComFunc();

            txtPano.Text = "";
            lblSName.Text = "";
            //strBDATE = strBDATE.Replace("-", "");
            strFrDATE = strFrDATE.Replace("-", "");
            strToDATE = strToDATE.Replace("-", "");

            bSimsa = clsType.User.BuseCode == "078201";

            Control[] penel = null;
            Control obj = null;
            if (strGubun != "")
            {
                penel = this.Controls.Find("rdoGB" + strGubun, true);
                if (penel != null)
                {
                    if (penel.Length > 0)
                    {
                        obj = (RadioButton)penel[0];
                        ((RadioButton)obj).Checked = true;
                    }
                }
            }
            
            if (strPANO != "")
            {
                txtPano.Text = strPANO;
                lblSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPano.Text);
                if (strFrDATE != "")
                {
                    dtpFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(strFrDATE, "D"));
                    dtpToDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(strToDATE, "D"));
                }
                else
                {
                    dtpFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                    dtpToDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                }
                
            }
            else
            {
                dtpFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
                dtpToDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            }
            
            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept, "1", 1);

            // 필터기능 추가
            for (int i = 0; i < ssView_Sheet1.ColumnCount; i++)
            {
                methodSpd.setSpdFilter(ssView, i, AutoFilterMode.EnhancedContextMenu, true);
            }
            
            Search_Data();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoGB_CheckedChanged(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strPano = "";
            string strDrcd = "";
            string strRemark = "";

            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            

            strRemark += "▣ 환자정보 :   " + ssView_Sheet1.Cells[e.Row, 3].Text.Trim();
            strRemark += " / " + ssView_Sheet1.Cells[e.Row, 4].Text.Trim();
            strRemark += " / " + ssView_Sheet1.Cells[e.Row, 6].Text.Trim();
            strRemark += " / " + ssView_Sheet1.Cells[e.Row, 5].Text.Trim();
            strRemark += " / " + ssView_Sheet1.Cells[e.Row, 7].Text.Trim() + ComNum.VBLF;

            strRemark += "▣ 의뢰정보 :   " + ssView_Sheet1.Cells[e.Row, 10 + 1].Text.Trim();
            strRemark += " / " + ssView_Sheet1.Cells[e.Row, 9 + 1].Text.Trim() + ComNum.VBLF;
            strRemark += "▣ 의뢰일시 :   " + ssView_Sheet1.Cells[e.Row, 11 + 1].Text.Trim() + ComNum.VBLF + ComNum.VBLF;
            strRemark += "▣ 의뢰내용 ---------------------------------------------------> " +
                         ComNum.VBLF + ComNum.VBLF + ssView_Sheet1.Cells[e.Row, 12 + 1].Text.Trim() + ComNum.VBLF;

            strRemark += "▣ 진료정보 :   " + ssView_Sheet1.Cells[e.Row, 1].Text.Trim();
            strRemark += " / " + ssView_Sheet1.Cells[e.Row, 2].Text.Trim() + ComNum.VBLF;
            strRemark += "▣ 진료일시 :   " + ssView_Sheet1.Cells[e.Row, 13 + 1].Text.Trim() + ComNum.VBLF + ComNum.VBLF;
            strRemark += "▣ 회송내용 ---------------------------------------------------> " +
                         ComNum.VBLF + ComNum.VBLF + ssView_Sheet1.Cells[e.Row, 14 + 1].Text.Trim() + ComNum.VBLF;

            if (ssView_Sheet1.Cells[e.Row, 15 + 1].Text.Trim() == "*")
            {
                rdoConfirm0.Checked = true;
            }
            else
            {
                rdoConfirm1.Checked = true;
            }

            FstrROWID = ssView_Sheet1.Cells[e.Row, 16 + 1].Text.Trim();

            txtRemark.Text = strRemark;


            strPano = ssView_Sheet1.Cells[e.Row, 3].Text;
            strDrcd = ssView_Sheet1.Cells[e.Row, 17 + 1].Text;

            //EMR 뷰어                
            //clsVbEmr.EXECUTE_TextEmrView(strPano, clsType.User.Sabun);
            if (bSimsa == false)
            {
                //2018-08-02 심사팀요청으로 심사팀이 아닐때만 실행
                clsVbEmr.EXECUTE_TextEmrView(strPano, VB.Val(clsVbfunc.GetOCSDrCodeSabun(clsDB.DbCon, strDrcd)).ToString());
            }
            else
            {
                //clsVbEmr.EXECUTE_TextEmrView(strPano, clsType.User.Sabun);
            }

            grbConsult.Enabled = true;
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");

            lblSName.Text = "";
            lblSName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, txtPano.Text);
        }

        private void ssView_CellClick(object sender, CellClickEventArgs e)
        {
            if(e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            strPANO = ssView_Sheet1.Cells[e.Row, 3].Text;
            strSname = ssView_Sheet1.Cells[e.Row, 4].Text;
            strFrDATE = ssView_Sheet1.Cells[e.Row, 0].Text;
            strToDATE = ssView_Sheet1.Cells[e.Row, 0].Text;
            strSex = ssView_Sheet1.Cells[e.Row, 6].Text;
            strRoom = ssView_Sheet1.Cells[e.Row, 18 + 1].Text;
            strDeptCode = ssView_Sheet1.Cells[e.Row, 10 + 1].Text;
            strGbSTS = ssView_Sheet1.Cells[e.Row, 9].Text;
            strDrname = ssView_Sheet1.Cells[e.Row, 9 + 1].Text;
            //strTel = Trim(P(GstrHelpCode, "^^", 8))
            strIPDNO = ssView_Sheet1.Cells[e.Row, 19 + 1].Text;
            strWard = ssView_Sheet1.Cells[e.Row, 7].Text;
            strAge = ssView_Sheet1.Cells[e.Row, 5].Text;
            strToDr = ssView_Sheet1.Cells[e.Row, 2].Text;
            strToDept = ssView_Sheet1.Cells[e.Row, 1].Text;

            // 마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {                
                PopupMenu = new ContextMenu();
                ssView.ContextMenu = null;
                
                PopupMenu.Name = "ssView";                
                PopupMenu.MenuItems.Add("접수증 출력", new System.EventHandler(mnuSet_Click));

                ssView.ContextMenu = PopupMenu;
            }
        }

        private void mnuSet_Click(object sender, EventArgs e) // 이벤트 헨들러
        {
            string strPopMenuName = "";
            string strSelectMenuName = "";
            string strSelectMenuText = "";
            
            strPopMenuName = ((MenuItem)sender).Parent.Name;
            strSelectMenuName = ((MenuItem)sender).Name;
            strSelectMenuText = ((MenuItem)sender).Text;

            if (strSelectMenuText == "접수증 출력")
            {
                PRINT_CONSULTJUPSU();
            }

            ssView.ContextMenu = null;
        }

        private void PRINT_CONSULTJUPSU()
        {
            //clsSpread.SpdPrint_Margin setMargin;
            //clsSpread.SpdPrint_Option setOption;
            clsSpread SP = new clsSpread();
            clsPrint CP = new clsPrint();

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strPatSTS = "";


            string mstrPrintName = CP.getPrinter_Chk("접수증");

            if (mstrPrintName == "")
            {
                ComFunc.MsgBox("컴퓨터에 프린트 드라이버가 없습니다.", "프린터 설정오류");
                return;
            }
            
            try
            {                                
                SQL = "SELECT JUMIN1 || Jumin2 Jumin,Bi FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO ='" + strPANO + "'          ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    strJumin = dt.Rows[0]["Jumin"].ToString().Trim();
                    strBi = dt.Rows[0]["Bi"].ToString().Trim();                    
                }
                

                SQL = "SELECT a.Tel,a.Team  ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_TEAM a, KOSMOS_PMPA.NUR_TEAM_ROOMCODE b";
                SQL = SQL + ComNum.VBLF + "   Where a.WardCode = b.WardCode";
                SQL = SQL + ComNum.VBLF + "    AND a.TEAM=b.TEAM";
                SQL = SQL + ComNum.VBLF + "    AND b.RoomCode =" + VB.Val(strRoom) + " ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strTel = strWard + " " + dt.Rows[0]["Tel"].ToString().Trim();
                }
                else
                {
                    SQL = "SELECT NAME,CODE ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE ";
                    SQL = SQL + ComNum.VBLF + "   WHERE GUBUN ='ETC_병동전화' ";
                    SQL = SQL + ComNum.VBLF + "    AND TRIM(CODE) ='" + strRoom + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        strTel = dt1.Rows[0]["CODE"].ToString().Trim() + " " + dt1.Rows[0]["NAME"].ToString().Trim();
                    }
                    else
                    {
                        strTel = strWard;
                    }
                    dt1.Dispose();
                    dt1 = null;
                }                
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


            switch (strBi)
            {
                case "11":
                case "12":
                case "13":
                    strBi = "보험";
                    break;
                case "21":
                    strBi = "1종";
                    break;
                case "22":
                    strBi = "2종";
                    break;
                case "23":
                    strBi = "3종";
                    break;
                case "24":
                    strBi = "행려";
                    break;
                case "31":
                    strBi = "산재";
                    break;
                case "32":
                    strBi = "공상";
                    break;
                case "33":
                    strBi = "산재공상";
                    break;
                case "41":
                case "42":
                case "43":
                case "44":
                case "51":
                    strBi = "일반";
                    break;
                case "52":
                    strBi = "TA";
                    break;
                case "55":
                    strBi = "TA일반";
                    break;
            }

            // 타이틀
            ssJupsuPrint_Sheet1.Cells[0, 0].Text = "컨설트 접수증";
            // 의뢰과(의사)
            ssJupsuPrint_Sheet1.Cells[1, 2].Text = CF.READ_DEPTNAMEK(clsDB.DbCon, strDeptCode) + "(" + strDrname + ")";
            // 수신과(의사)
            ssJupsuPrint_Sheet1.Cells[2, 2].Text = CF.READ_DEPTNAMEK(clsDB.DbCon, strToDept) + "(" + strToDr + ")";
            // 등록번호
            ssJupsuPrint_Sheet1.Cells[3, 1].Text = strPANO + " " + strSname;
            ssJupsuPrint_Sheet1.Cells[3, 1].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssJupsuPrint_Sheet1.Cells[3, 1].VerticalAlignment = CellVerticalAlignment.Center;
            //처방일자
            ssJupsuPrint_Sheet1.Cells[4, 1].Text = strBDATE;
            ssJupsuPrint_Sheet1.Cells[4, 1].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssJupsuPrint_Sheet1.Cells[4, 1].VerticalAlignment = CellVerticalAlignment.Center;
            //성별,나이
            ssJupsuPrint_Sheet1.Cells[4, 4].Text = strSex + "/" + strAge;
            ssJupsuPrint_Sheet1.Cells[4, 4].HorizontalAlignment = CellHorizontalAlignment.Center;
            ssJupsuPrint_Sheet1.Cells[4, 4].VerticalAlignment = CellVerticalAlignment.Center;
            // 메모
            ssJupsuPrint_Sheet1.Cells[5, 0].Text = "";
            ssJupsuPrint_Sheet1.Cells[5, 0].Text += "주민번호:" + strJumin + "  ▶ 거동: " + strGbSTS + ComNum.VBLF;
            ssJupsuPrint_Sheet1.Cells[5, 0].Text += "호실:" + strRoom + " 팀번호:" + strTel + " 자격:" + strBi +  ComNum.VBLF;
            
            strPatSTS = READ_IPD_PATIENT_STS(strPANO, clsPublic.GstrSysDate, strIPDNO, strAge, strWard);
            if(strPatSTS != "")
            {
                ssJupsuPrint_Sheet1.Cells[5, 0].Text += "<병동환자정보>" + ComNum.VBLF;
                ssJupsuPrint_Sheet1.Cells[5, 0].Text += strPatSTS;
            }

            ssJupsuPrint_Sheet1.PrintInfo.Printer = mstrPrintName;
            ssJupsuPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;   //세로
            ssJupsuPrint_Sheet1.PrintInfo.Margin.Left = 5;
            ssJupsuPrint_Sheet1.PrintInfo.Margin.Right = 5;
            ssJupsuPrint_Sheet1.PrintInfo.Margin.Top = 5;
            ssJupsuPrint_Sheet1.PrintInfo.Margin.Bottom = 5;
            ssJupsuPrint_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssJupsuPrint_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssJupsuPrint_Sheet1.PrintInfo.ShowBorder = false;
            ssJupsuPrint_Sheet1.PrintInfo.ShowColor = false;
            ssJupsuPrint_Sheet1.PrintInfo.ShowGrid = true;
            ssJupsuPrint_Sheet1.PrintInfo.ShowShadows = false;
            ssJupsuPrint_Sheet1.PrintInfo.UseMax = false;
            ssJupsuPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssJupsuPrint.PrintSheet(0);
        }



        private string READ_IPD_PATIENT_STS(string argPTNO, string ArgBDate, string ArgIPDNO, string ArgAge, string ArgWardCode)
        {
            string strSTS = "";
            string strTemp = "";

            //낙상
            if (clsNurse.READ_WARNING_FALL(clsDB.DbCon, argPTNO, ArgBDate, Convert.ToDouble(ArgIPDNO), ArgAge, "") != "")
            {
                strSTS = strSTS + "낙상,";
            }

            //'욕창
            if (clsNurse.READ_WARNING_BRADEN(clsDB.DbCon, argPTNO, ArgBDate, ArgIPDNO, ArgAge, ArgWardCode) != "")
            {
                strSTS = strSTS + "욕창,";
            }


            //'통증
            if (clsNurse.READ_PAIN_RESTART(clsDB.DbCon, ArgIPDNO, argPTNO) != "")
            {
                strSTS = strSTS + "통증,";
            }

            //'사생활
            if (clsNurse.READ_SECRET(clsDB.DbCon, ArgIPDNO) != "")
            {
                strSTS = strSTS + "사생활,";
            }

            //'항암제
            if (clsNurse.READ_JUSAMIX(clsDB.DbCon, argPTNO) != "")
            {
                strSTS = strSTS + "항암제,";
            }

            //'중심정맥관
            if (clsNurse.READ_CENTRAL_CATH(clsDB.DbCon, ArgIPDNO) != "")
            {
                strSTS = strSTS + "중심정맥과,";
            }
                
            //'ADR
            if (clsNurse.READ_ADR(clsDB.DbCon, ArgIPDNO) != "")
            {
                strSTS = strSTS + "ADR,";
            }
            
            //'화재
            strTemp = clsNurse.READ_FIRE(clsDB.DbCon, ArgIPDNO);
            if (strTemp != "")
            {
                strSTS = strSTS + "화재:" + strTemp + ",";
            }

            return strSTS;
        }

    }
}
