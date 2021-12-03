using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmOcsMsgPanoHyperlipidemia
    /// File Name : frmOcsMsgPanoHyperlipidemia.cs
    /// Title or Description : 고지혈증 메시지 처리 - [ 외래 ]
    /// Author : 박창욱
    /// Create Date : 2017-06-08
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    /// </summary>  
    /// <history>  
    /// VB\BuSuga59.frm(FrmOcsMsgPano_O2_Build) -> frmOcsMsgPanoHyperlipidemia.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\busuga\BuSuga59.frm(FrmFrmOcsMsgPano_O2_Build)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\busuga\\busuga.vbp
    /// </vbp>
    public partial class frmOcsMsgPanoHyperlipidemia : Form
    {
        private string gstrHelpCode = "";

        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        public delegate void EventClose();
        public event EventClose rEventClose;

        public frmOcsMsgPanoHyperlipidemia()
        {
            InitializeComponent();
        }

        private void BAS_OCSMEMO_02(string ArgPano, string ArgSName)
        {
            string strData = "";
            string strROWID = "";
            int nWRTNO = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            strData = VB.Replace(txtInfo.Rtf, "'", "`");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL = " SELECT ROWID FROM ADMIN.BAS_OCSMEMO_O2";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                strROWID = "";

                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["RowID"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                if (strROWID == "")
                {
                    SQL = "";
                    SQL = " SELECT MAX(WRTNO) MWRTNO FROM ADMIN.BAS_OCSMEMO_O2 ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    nWRTNO = Convert.ToInt32(dt.Rows[0]["MWRTNO"].ToString().Trim()) + 1;
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = " INSERT INTO ADMIN.BAS_OCSMEMO_O2 (  PANO, SNAME, MEMO, SDATE, DDATE, WRTNO, GBJOB ) VALUES (";
                    SQL = SQL + ComNum.VBLF + " '" + ArgPano + "', '" + ArgSName + "', :1, trunc(sysdate), '' , '" + nWRTNO + "', ";
                    SQL = SQL + ComNum.VBLF + "    '2'  ";
                    SQL = SQL + ComNum.VBLF + " ) ";
                    SqlErr = clsDB.ExecuteLongQuery(SQL, strData, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE ADMIN.BAS_OCSMEMO_O2 SET MEMO = :1 ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                    SqlErr = clsDB.ExecuteLongQuery(SQL, strData, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        //외래 OCS 메시지 처리
        private void OPD_OCS_MSG_Process(string ArgFdate, string ArgTdate, string ArgDay)
        {
            int i = 0;
            int j = 0;
            string strOK_A = "";
            string strOK_B = "";
            int nRead = 0;
            string strExamDate = "";
            string strTDate = "";
            int nRow = 0;
            string strExamDate_A = "";
            string strExamResult_A = "";
            string strExamDate_B = "";
            string strExamResult_B = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt2 = null;


            strTDate = Convert.ToDateTime(ArgTdate).AddDays(1).ToString("yyyy-MM-dd");
            strExamDate = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(VB.Val(ArgDay) * -1).ToString("yyyy-MM-dd");

            try
            {
                if (rdoReservation.Checked == true)
                {
                    SQL = "";
                    SQL = "  SELECT  TRUNC(DATE3) ACTDATE, A.PANO ,  A.SNAME,  A.DRCODE, C.DRNAME, A.DEPTCODE";
                    SQL = SQL + ComNum.VBLF + "     FROM OPD_RESERVED_NEW A, OPD_SLIP B , BAS_DOCTOR C";
                    SQL = SQL + ComNum.VBLF + "   WHERE A.DATE3 >= TO_DATE('" + ArgFdate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND A.DATE3 < TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND A.PANO = B.PANO";
                    SQL = SQL + ComNum.VBLF + "     AND A.DATE1 = B.ACTDATE ";
                    SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE = B.DEPTCODE";
                    SQL = SQL + ComNum.VBLF + "     AND A.DRCODE = C.DRCODE";
                    SQL = SQL + ComNum.VBLF + "     AND B.SUNEXT IN ( SELECT SUNEXT FROM ADMIN.BAS_SUN WHERE GBGOJI ='Y' ) ";
                    SQL = SQL + ComNum.VBLF + "   GROUP BY TRUNC(DATE3), A.PANO, A.SNAME,  A.DRCODE, C.DRNAME , A.DEPTCODE";
                    SQL = SQL + ComNum.VBLF + "  HAVING SUM(B.QTY * B.NAL) > 0";
                    SQL = SQL + ComNum.VBLF + "   ORDER BY 4";
                }
                else
                {
                    SQL = "";
                    SQL = "  SELECT  TRUNC(B.ACTDATE) ACTDATE, A.PANO ,  A.SNAME,  A.DRCODE, C.DRNAME, A.DEPTCODE";
                    SQL = SQL + ComNum.VBLF + "     FROM OPD_MASTER A, OPD_SLIP B , BAS_DOCTOR C";
                    SQL = SQL + ComNum.VBLF + "   WHERE B.ACTDATE >= TO_DATE('" + ArgFdate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND B.ACTDATE <= TO_DATE('" + ArgTdate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "     AND A.PANO = B.PANO";
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = B.ACTDATE ";
                    SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE = B.DEPTCODE";
                    SQL = SQL + ComNum.VBLF + "     AND A.DRCODE = C.DRCODE";
                    SQL = SQL + ComNum.VBLF + "     AND B.SUNEXT IN ( SELECT SUNEXT FROM ADMIN.BAS_SUN WHERE GBGOJI ='Y' ) ";
                    SQL = SQL + ComNum.VBLF + "   GROUP BY TRUNC(B.ACTDATE), A.PANO, A.SNAME,  A.DRCODE, C.DRNAME , A.DEPTCODE";
                    SQL = SQL + ComNum.VBLF + "   HAVING SUM(B.QTY * B.NAL) > 0";
                    SQL = SQL + ComNum.VBLF + "   ORDER BY 4";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다.");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                nRead = dt.Rows.Count;

                ssView_Sheet1.RowCount = 0;
                nRow = 0;

                for (i = 0; i < nRead; i++)
                {
                    SQL = "";
                    SQL = "   SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE , B.MASTERCODE, B.RESULT ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.EXAM_SPECMST A, ADMIN.EXAM_RESULTC B";
                    SQL = SQL + ComNum.VBLF + " WHERE A.BDATE >=TO_DATE('" + strExamDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND B.MASTERCODE IN ( 'CR40','CR39')";
                    SQL = SQL + ComNum.VBLF + "   AND A.SPECNO = B.SPECNO";
                    SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY BDATE ASC ";
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다.");
                        return;
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        strOK_A = "NO";
                        strOK_B = "NO";
                        strExamDate_A = "";
                        strExamResult_A = "";
                        strExamDate_B = "";
                        strExamResult_B = "";

                        for (j = 0; j < dt2.Rows.Count; j++)
                        {
                            if (dt2.Rows[j]["MASTERCODE"].ToString().Trim() == "CR40")
                            {
                                if (Convert.ToInt32(dt2.Rows[j]["result"].ToString().Trim()) < 130 ||
                                   Convert.ToInt32(dt2.Rows[j]["result"].ToString().Trim()) > 230)
                                {
                                    strOK_A = "OK";
                                }
                                else
                                {
                                    strOK_A = "NO";
                                }
                                strExamDate_A = dt2.Rows[j]["BDate"].ToString().Trim();
                                strExamResult_A = dt2.Rows[j]["result"].ToString().Trim();
                            }
                            else if (dt2.Rows[j]["MASTERCODE"].ToString().Trim() == "CR39")
                            {
                                if (Convert.ToInt32(dt2.Rows[j]["result"].ToString().Trim()) < 34 ||
                                   Convert.ToInt32(dt2.Rows[j]["result"].ToString().Trim()) > 143)
                                {
                                    strOK_B = "OK";
                                }
                                else
                                {
                                    strOK_B = "NO";
                                }
                                strExamDate_B = dt2.Rows[j]["BDate"].ToString().Trim();
                                strExamResult_B = dt2.Rows[j]["result"].ToString().Trim();
                            }
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;

                    if (chkAll.Checked == true)
                    {
                        nRow += 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = Convert.ToDateTime(dt.Rows[i]["ACTDate"].ToString().Trim()).ToString("yyyy-MM-dd");
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["sName"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = strExamDate_A;
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = strExamResult_A;
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = strExamDate_B;
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = strExamResult_B;
                    }
                    else
                    {
                        if (strOK_A == "NO" || strOK_B == "NO")  //ocs MESSAGE 처리
                        {
                            nRow += 1;
                            if (nRow > ssView_Sheet1.RowCount)
                            {
                                ssView_Sheet1.RowCount = nRow;
                            }
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = Convert.ToDateTime(dt.Rows[i]["ACTDate"].ToString().Trim()).ToString("yyyy-MM-dd");
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["sName"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DrName"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 6].Text = strExamDate_A;
                            ssView_Sheet1.Cells[nRow - 1, 7].Text = strExamResult_A;
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = strExamDate_B;
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = strExamResult_B;
                        }
                    }

                    if (strOK_A == "NO" || strOK_B == "NO")
                    {
                        ssView_Sheet1.Columns[-1].ForeColor = Color.FromArgb(255, 0, 0);
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (rdoTreatment.Checked == true)
            {
                if (ComFunc.MsgBoxQ("해당 작업은 조회 시간이 오래걸립니다. 작업을 계속 하시겠습니까?", "확인") == DialogResult.No)
                {
                    return;
                }
            }
            Cursor.Current = Cursors.WaitCursor;
            OPD_OCS_MSG_Process(dtpFDate.Text, dtpTDate.Text, VB.Replace(cboDay.Text, "일전", ""));
            Cursor.Current = Cursors.Default;
        }

        private void btnMsg_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strPano = "";
            string strSname = "";
            string strResult_A = "";
            string strResult_B = "";
            string strExamDate_A = "";
            string strExamDate_B = "";

            string strMsg = "";

            for (i = 1; i < ssView_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssView_Sheet1.Cells[i - 1, 0].Value) == true)
                {
                    strPano = ssView_Sheet1.Cells[i - 1, 2].Text;
                    strSname = ssView_Sheet1.Cells[i - 1, 3].Text;
                    strExamDate_A = VB.Replace(ssView_Sheet1.Cells[i - 1, 6].Text, "-", "/");
                    strResult_A = ssView_Sheet1.Cells[i - 1, 7].Text;
                    strExamDate_B = VB.Replace(ssView_Sheet1.Cells[i - 1, 8].Text, "-", "/");
                    strResult_B = ssView_Sheet1.Cells[i - 1, 9].Text;

                    strMsg = "";
                    strMsg = "          <<< 고지혈증 관련 정보 >>>              " + ComNum.VBLF;
                    strMsg = strMsg + "-----------------------------------------------   " + ComNum.VBLF;
                    strMsg = strMsg + "  검사종류            검사일자     결  과  " + ComNum.VBLF;
                    strMsg = strMsg + "-----------------------------------------------   " + ComNum.VBLF;
                    strMsg = strMsg + " T. Cholesterol     " + strExamDate_A + VB.Space(5) + strResult_A + ComNum.VBLF;
                    strMsg = strMsg + " [참고치:130 ~ 230 ]                              " + ComNum.VBLF;
                    strMsg = strMsg + "-----------------------------------------------   " + ComNum.VBLF;
                    strMsg = strMsg + " Triglyceride       " + strExamDate_B + VB.Space(5) + strResult_B + ComNum.VBLF;
                    strMsg = strMsg + " [참고치: 34 ~ 143 ]                             " + ComNum.VBLF;
                    strMsg = strMsg + "-----------------------------------------------  " + ComNum.VBLF + ComNum.VBLF;
                    strMsg = strMsg + " ◈ 고지혈제 오더 전송시 본인 부담 100% 로   " + ComNum.VBLF;
                    strMsg = strMsg + "    부탁드립니다.   " + ComNum.VBLF;
                    txtInfo.Text = strMsg;

                    BAS_OCSMEMO_02(strPano, strSname);
                }
            }
            ComFunc.MsgBox("등록된 메세지를 확인해보세요.");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            Cursor.Current = Cursors.WaitCursor;

            //Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fs2";
            strHead1 = "/c/f1" + "고 지 혈 증 메세지 대상자" + "/n";
            strHead2 = "/l/f2" + "인쇄일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            strHead2 = strHead2 + "/r/f2" + "PAGE : /p";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 5;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);

            Cursor.Current = Cursors.Default;
        }

        private void frmOcsMsgPanoHyperlipidemia_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = clsPublic.GstrSysDate;
            dtpTDate.Text = clsPublic.GstrSysDate;

            cboDay.Items.Clear();
            cboDay.Items.Add("30일전");
            cboDay.Items.Add("60일전");
            cboDay.Items.Add("90일전");
            cboDay.Items.Add("120일전");
            cboDay.Items.Add("150일전");
            cboDay.Items.Add("200일전");
            cboDay.Items.Add("300일전");
            cboDay.Items.Add("400일전");
            cboDay.SelectedIndex = 6;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClose();
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            //FrmTongGoji.Show 1
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) { return; }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            if (e.Column == 0 || e.Row == 0)
            {
                return;
            }
            gstrHelpCode = ssView_Sheet1.Cells[e.Row, 2].ToString().Trim();
            rSetHelpCode(gstrHelpCode);

            rEventClose();
        }

    }
}
