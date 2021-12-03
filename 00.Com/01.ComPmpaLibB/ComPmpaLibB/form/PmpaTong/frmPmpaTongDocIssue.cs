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
    /// File Name       : frmPmpaTongDocIssue.cs
    /// Description     : 서류발급통계
    /// Author          : 안정수
    /// Create Date     : 2017-09-11
    /// Update History  : 2017-11-04
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm서류발급통계.frm(Frm서류발급통계) => frmPmpaTongDocIssue.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm서류발급통계.frm(Frm서류발급통계)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaTongDocIssue : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        public frmPmpaTongDocIssue()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등            

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if(sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void ePrint()
        {
            string strChkOne = "";

            if(chkOne.Checked == true)
            {
                strChkOne = "(" + chkOne.Name + ")";
            }
            else
            {
                strChkOne = "";
            }

            if (MessageBox.Show("접수~서류발급 출력하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                JtoDocu_Prt(strChkOne);
            }

            if (MessageBox.Show("퇴원계산서수납~서류발급 출력하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                StoDocu_Prt(strChkOne);
            }
        }

        void JtoDocu_Prt(string ArgChkOne)
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

            if (strPrintName != "")
            {
                strTitle = "접수~서류발급 통계" + "/n";
                strSubTitle = "출력일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + ArgChkOne;


                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String(strTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 170, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                SPR.setSpdPrint(ssList1, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
            }
        }

        void StoDocu_Prt(string ArgChkOne)
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

            if (strPrintName != "")
            {
                strTitle = "퇴원계산서수납~서류발급 통계" + "/n";
                strSubTitle = "출력일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + ArgChkOne;

                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String(strTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 170, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                SPR.setSpdPrint(ssList2, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
            }
        }

        void eGetData()
        {
            if(chkOne.Checked == true)
            {
                JtoDocu_Group();
                StoDocu_Group();
            }
            else
            {
                JtoDocu();
                StoDocu();
            }
        }

        void JtoDocu_Group()
        {
            string strFDate = "";
            string strTDate = "";
            int i = 0;
            int nRead = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                        ";
            SQL += ComNum.VBLF + "  A.PANO, B.SNAME, A.ACTDATE                                                                                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "OPD_MASTER B                                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";
            SQL += ComNum.VBLF + "  AND A.PANO = B.PANO                                                                                         ";
            SQL += ComNum.VBLF + "  AND A.ACTDATE = B.ACTDATE                                                                                   ";
            SQL += ComNum.VBLF + "  AND (A.SUNEXT IN (SELECT SUCODE                                                                             ";
            SQL += ComNum.VBLF + "                      FROM ADMIN.BAS_SUT                                                                ";
            SQL += ComNum.VBLF + "                      WHERE BUN = '75' AND SUCODE LIKE 'ZA%')                                                 ";
            SQL += ComNum.VBLF + "          OR A.SUNEXT IN ('AA333A','ZA38','ZA18', 'COPY','XCDC','F08','F12', 'Y82', 'ZA67', 'Y75'))           ";
            SQL += ComNum.VBLF + "  AND A.ACTDATE BETWEEN TO_DATE('" + strFDate + "','YYYY-MM-DD') AND TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND A.ROWID NOT IN (SELECT TABLEROWID                                                                       ";
            SQL += ComNum.VBLF + "                      FROM ADMIN.OPD_SLIP_JDEL                                                          ";
            SQL += ComNum.VBLF + "                      WHERE ACTDATE BETWEEN TO_DATE('" + strFDate + "','YYYY-MM-DD')                          ";
            SQL += ComNum.VBLF + "                      AND TO_DATE('" + strTDate + "','YYYY-MM-DD'))                                           ";
            SQL += ComNum.VBLF + "  AND A.WARDCODE <> 'II'                                                                                      ";
            SQL += ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.ACTDATE                                                                           ";

            try
            {
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

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;

                    ssList1_Sheet1.Rows.Count = 0;
                    ssList1_Sheet1.Rows.Count = nRead;
                    ssList1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < nRead; i++)
                    {
                        ssList1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 2].Text = VB.Left(dt.Rows[i]["ACTDATE"].ToString().Trim(), 10);
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        void StoDocu_Group()
        {
            string strFDate = "";
            string strTDate = "";
            int i = 0;
            int nRead = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                        ";
            SQL += ComNum.VBLF + "  A.PANO, B.SNAME, A.ACTDATE                                                                                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B                                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";
            SQL += ComNum.VBLF + "  AND A.PANO = B.PANO                                                                                         ";
            SQL += ComNum.VBLF + "  AND A.ACTDATE = B.ACTDATE                                                                                   ";
            SQL += ComNum.VBLF + "  AND (A.SUNEXT IN (SELECT SUCODE                                                                             ";
            SQL += ComNum.VBLF + "                      FROM ADMIN.BAS_SUT                                                                ";
            SQL += ComNum.VBLF + "                      WHERE BUN = '75' AND SUCODE LIKE 'ZA%')                                                 ";
            SQL += ComNum.VBLF + "          OR A.SUNEXT IN ('AA333A','ZA38','ZA18', 'COPY','XCDC','F08','F12', 'Y82', 'ZA67', 'Y75'))           ";
            SQL += ComNum.VBLF + "  AND A.ACTDATE BETWEEN TO_DATE('" + strFDate + "','YYYY-MM-DD') AND TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND A.ROWID NOT IN (SELECT TABLEROWID                                                                       ";
            SQL += ComNum.VBLF + "                      FROM ADMIN.OPD_SLIP_JDEL                                                          ";
            SQL += ComNum.VBLF + "                      WHERE ACTDATE BETWEEN TO_DATE('" + strFDate + "','YYYY-MM-DD')                          ";
            SQL += ComNum.VBLF + "                      AND TO_DATE('" + strTDate + "','YYYY-MM-DD'))                                           ";
            SQL += ComNum.VBLF + "  AND A.WARDCODE = 'II'                                                                                       ";
            SQL += ComNum.VBLF + "GROUP BY A.PANO, B.SNAME, A.ACTDATE                                                                           ";

            try
            {
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

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;

                    ssList2_Sheet1.Rows.Count = 0;
                    ssList2_Sheet1.Rows.Count = nRead;
                    ssList2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < nRead; i++)
                    {
                        ssList2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList2_Sheet1.Cells[i, 2].Text = VB.Left(dt.Rows[i]["ACTDATE"].ToString().Trim(), 10);
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        void JtoDocu()
        {
            string strFDate = "";
            string strTDate = "";
            int i = 0;
            int nRead = 0;

            string strJTime = "";
            string strSTime = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                        ";
            SQL += ComNum.VBLF + "  A.PANO, B.SNAME, TO_CHAR(B.JTIME, 'YYYY-MM-DD HH24:MI') T_1, TO_CHAR(B.STIME, 'YYYY-MM-DD HH24:MI') T_2,    ";
            SQL += ComNum.VBLF + "  TO_CHAR(B.JTIME, 'HH24:MI') T_1_HM, TO_CHAR(B.STIME, 'HH24:MI') T_2_HM                                      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "OPD_MASTER B                                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                     ";
            SQL += ComNum.VBLF + "  AND A.PANO = B.PANO                                                                                         ";
            SQL += ComNum.VBLF + "  AND A.ACTDATE = B.ACTDATE                                                                                   ";
            SQL += ComNum.VBLF + "  AND (A.SUNEXT IN (SELECT SUCODE                                                                             ";
            SQL += ComNum.VBLF + "                      FROM ADMIN.BAS_SUT                                                                ";
            SQL += ComNum.VBLF + "                      WHERE BUN = '75' AND SUCODE LIKE 'ZA%')                                                 ";
            SQL += ComNum.VBLF + "          OR A.SUNEXT IN ('AA333A','ZA38','ZA18', 'COPY','XCDC','F08','F12', 'Y82', 'ZA67', 'Y75'))           ";
            SQL += ComNum.VBLF + "  AND A.ACTDATE BETWEEN TO_DATE('" + strFDate + "','YYYY-MM-DD') AND TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND A.ROWID NOT IN (SELECT TABLEROWID                                                                       ";
            SQL += ComNum.VBLF + "                      FROM ADMIN.OPD_SLIP_JDEL                                                          ";
            SQL += ComNum.VBLF + "                      WHERE ACTDATE BETWEEN TO_DATE('" + strFDate + "','YYYY-MM-DD')                          ";
            SQL += ComNum.VBLF + "                      AND TO_DATE('" + strTDate + "','YYYY-MM-DD'))                                           ";
            SQL += ComNum.VBLF + "  AND A.WARDCODE <> 'II'                                                                                      ";
            

            try
            {
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

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;

                    ssList1_Sheet1.Rows.Count = 0;
                    ssList1_Sheet1.Rows.Count = nRead;
                    ssList1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < nRead; i++)
                    {
                        ssList1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["T_1"].ToString().Trim();
                        ssList1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["T_2"].ToString().Trim();

                        strJTime = dt.Rows[i]["T_1_HM"].ToString().Trim();
                        strSTime = dt.Rows[i]["T_2_HM"].ToString().Trim();

                        if(strJTime != "" && strSTime != "")
                        {
                            if(DateTime.Compare(Convert.ToDateTime(strJTime), Convert.ToDateTime(strSTime)) < 0)
                            {
                                ssList1_Sheet1.Cells[i, 4].Text = (Convert.ToDateTime(strSTime) - Convert.ToDateTime(strJTime)).TotalMinutes.ToString();
                            }
                            else if(DateTime.Compare(Convert.ToDateTime(strJTime), Convert.ToDateTime(strSTime)) > 0)
                            {
                                ssList1_Sheet1.Cells[i, 4].Text = "-" +  (Convert.ToDateTime(strJTime) - Convert.ToDateTime(strSTime)).TotalMinutes.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        void StoDocu()
        {
            string strFDate = "";
            string strTDate = "";
            int i = 0;
            int nRead = 0;

            string strJTime = "";
            string strSTime = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                            ";
            SQL += ComNum.VBLF + "  A.PANO, B.SNAME, TO_CHAR(B.SUNAPTIME, 'YYYY-MM-DD HH24:MI') T_1, TO_CHAR(A.ENTDATE, 'YYYY-MM-DD HH24:MI') T_2,  ";
            SQL += ComNum.VBLF + "  TO_CHAR(B.SUNAPTIME, 'HH24:MI') T_1_HM, TO_CHAR(A.ENTDATE, 'HH24:MI') T_2_HM                                    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B                                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                         ";
            SQL += ComNum.VBLF + "  AND A.PANO = B.PANO                                                                                             ";
            SQL += ComNum.VBLF + "  AND A.ACTDATE = B.ACTDATE                                                                                       ";
            SQL += ComNum.VBLF + "  AND (A.SUNEXT IN (SELECT SUCODE                                                                                 ";
            SQL += ComNum.VBLF + "                      FROM ADMIN.BAS_SUT                                                                    ";
            SQL += ComNum.VBLF + "                      WHERE BUN = '75' AND SUCODE LIKE 'ZA%')                                                     ";
            SQL += ComNum.VBLF + "          OR A.SUNEXT IN ('AA333A','ZA38','ZA18', 'COPY','XCDC','F08','F12', 'Y82', 'ZA67', 'Y75'))               ";
            SQL += ComNum.VBLF + "  AND A.ACTDATE BETWEEN TO_DATE('" + strFDate + "','YYYY-MM-DD') AND TO_DATE('" + strTDate + "','YYYY-MM-DD')     ";
            SQL += ComNum.VBLF + "  AND A.ROWID NOT IN (SELECT TABLEROWID                                                                           ";
            SQL += ComNum.VBLF + "                      FROM ADMIN.OPD_SLIP_JDEL                                                              ";
            SQL += ComNum.VBLF + "                      WHERE ACTDATE BETWEEN TO_DATE('" + strFDate + "','YYYY-MM-DD')                              ";
            SQL += ComNum.VBLF + "                      AND TO_DATE('" + strTDate + "','YYYY-MM-DD'))                                               ";
            SQL += ComNum.VBLF + "  AND A.WARDCODE = 'II'                                                                                           ";


            try
            {
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

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;

                    ssList2_Sheet1.Rows.Count = 0;
                    ssList2_Sheet1.Rows.Count = nRead;
                    ssList2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < nRead; i++)
                    {
                        ssList2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["T_1"].ToString().Trim();
                        ssList2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["T_2"].ToString().Trim();

                        strJTime = dt.Rows[i]["T_1_HM"].ToString().Trim();
                        strSTime = dt.Rows[i]["T_2_HM"].ToString().Trim();

                        if (strJTime != "" && strSTime != "")
                        {
                            if (DateTime.Compare(Convert.ToDateTime(strJTime), Convert.ToDateTime(strSTime)) < 0)
                            {
                                ssList2_Sheet1.Cells[i, 4].Text = (Convert.ToDateTime(strSTime) - Convert.ToDateTime(strJTime)).TotalMinutes.ToString();
                            }
                            else if (DateTime.Compare(Convert.ToDateTime(strJTime), Convert.ToDateTime(strSTime)) > 0)
                            {
                                ssList2_Sheet1.Cells[i, 4].Text = "-" + (Convert.ToDateTime(strJTime) - Convert.ToDateTime(strSTime)).TotalMinutes.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }


    }
}
