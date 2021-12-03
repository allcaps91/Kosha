using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewJinCertificateList.cs
    /// Description     : 진료 증명서 리스트
    /// Author          : 안정수
    /// Create Date     : 2017-10-16
    /// Update History  :     
    /// <history>       
    /// d:\psmh\OPD\jengsan\Frm진료증명서리스트.frm(Frm진료증명서리스트) => frmPmpaViewJinCertificateList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\jengsan\Frm진료증명서리스트.frm(Frm진료증명서리스트)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewJinCertificateList : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        int mnJobSabun = 0;
        string FstrROWID = "";
        public frmPmpaViewJinCertificateList()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewJinCertificateList(int GnJobSabun)
        {
            InitializeComponent();
            setEvent();
            mnJobSabun = GnJobSabun;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnSendPage.Click += new EventHandler(eBtnEvent);
            this.btnCommand1.Click += new EventHandler(eBtnEvent);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            btnCommand1.Visible = false;

            optSort0.Checked = true;
            Set_Combo();
        }

        void Set_Combo()
        {
            cboGubun.Items.Clear();
            cboGubun.Items.Add("1.발급");
            cboGubun.Items.Add("2.재발급");
            cboGubun.SelectedIndex = 0;

            cboPart.Items.Clear();
            cboPart.Items.Add("**.전체");
            cboPart.Items.Add("00.진료사실증명서");
            cboPart.Items.Add("01.진단서");
            cboPart.Items.Add("02.상해진단서");
            cboPart.Items.Add("03.병사용진단서");
            cboPart.Items.Add("05.사망진단서");
            cboPart.Items.Add("08.소견서");
            cboPart.Items.Add("14.건강진단서");
            cboPart.Items.Add("18.진료의뢰서");
            cboPart.Items.Add("19.장애인증명서");
            cboPart.Items.Add("20.장애진단서");
            cboPart.Items.Add("21.출생증명서");
            cboPart.Items.Add("26.의료급여의뢰서");
            cboPart.Items.Add("27.응급환자진료의뢰서");
            cboPart.Items.Add("29.근로능력평가진단서");
            cboPart.Items.Add("31.의료급여연장신청서");
            cboPart.SelectedIndex = 0;

            txtPrtdate.Text = "";
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }

            else if (sender == this.btnCancel)
            {
                ssList_Sheet1.Rows.Count = 0;
                ssList_Sheet1.Rows.Count = 50;
            }

            else if (sender == this.btnSendPage)
            {
                btnSendPage_Click();
            }

            else if (sender == this.btnCommand1)
            {
                btnCommand1_Click();
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            strTitle = "진료사실 증명서 List";
            strSubTitle = "\r\n" + "조회일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + VB.Space(5) + "인쇄일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + "\r\n";
            strSubTitle += "업무메모 : " + txtPrtdate.Text.Trim();

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String("/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 60, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        //일반진단서 2013-01-25 이주형
        void btnCommand1_Click()
        {
            int i = 0;
            string strDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strDate = dtpDate.Text;
            ssList_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  A.SEQNO, A.PTNO, A.DEPTCODE, A.EDATE, B.SNAME, A.LSDATE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI01 A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
            SQL += ComNum.VBLF + "WHERE 1=1";

            if (optSort0.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND A.PRTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND A.LSDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }

            SQL += ComNum.VBLF + "      AND A.PTNO = B.PANO(+)";
            SQL += ComNum.VBLF + "      AND (SEQNO IS NOT NULL OR SEQNO <>'')";
            SQL += ComNum.VBLF + "ORDER BY A.SEQNO DESC";

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
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();

                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["EDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["USE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = "";
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

        void btnSendPage_Click()
        {
            string strPANO = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strRemark = "";
            string strRowid = "";
            string strBDate = "";
            int i = 0;

            strBDate = dtpDate.Text;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            if (FstrROWID == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "TONG_WONMUDAILY";
                SQL += ComNum.VBLF + "(JOBDATE, GBIO, ENTSABUN,ENTDATE,REMARK)";
                SQL += ComNum.VBLF + "VALUES (";
                SQL += ComNum.VBLF + "  TO_DATE('" + strBDate + "','YYYY-MM-DD') ,";
                SQL += ComNum.VBLF + "  'A',";
                SQL += ComNum.VBLF + "  " + mnJobSabun + ",";
                SQL += ComNum.VBLF + "  SYSDATE, '" + txtPrtdate.Text + "' )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

            }

            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "TONG_WONMUDAILY" + "SET";
                SQL += ComNum.VBLF + "ENTSABUN = " + mnJobSabun + ", ";
                SQL += ComNum.VBLF + "ENTDATE = SYSDATE ,";
                SQL += ComNum.VBLF + "REMARK = '" + txtPrtdate.Text + "'";
                SQL += ComNum.VBLF + "WHERE ROWID = '" + FstrROWID + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

            }

            clsDB.setCommitTran(clsDB.DbCon);
            Cursor.Current = Cursors.Default;

            MemoView();
        }

        //2017-06-01추가
        void MemoView()
        {
            int i = 0;
            string strDate = "";

            int nRead = 0;
            string strGubun = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strDate = dtpDate.Text;

            txtPrtdate.Text = "";

            FstrROWID = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  REMARK ,ROWID ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_WONMUDAILY";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND JOBDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND  GBIO ='A'";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return;
                //}

                if (dt.Rows.Count > 0)
                {
                    txtPrtdate.Text = dt.Rows[0]["Remark"].ToString().Trim();
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
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

        void eGetData()
        {
            switch (VB.Left(cboGubun.SelectedItem.ToString().Trim(), 1))
            {
                case "1":
                    HISTORY_PRINT();
                    break;

                case "2":
                    HISTORY_REPRINT();
                    break;
            }

            ssList.ActiveSheet.Columns[6].AllowAutoFilter = true;
            ssList.ActiveSheet.AutoFilterMode = FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu;

            MemoView();
        }

        void HISTORY_REPRINT()
        {
            int i = 0;
            string strDate = "";

            int nRead = 0;

            string strGubun = "";

            strDate = dtpDate.Text;
            ssList_Sheet1.Rows.Count = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  MCCLASS, SEQNO, PANO, 'O' IPDOPD, DEPTCODE, SINNAME, SINSAYU, BIGO,";
            SQL += ComNum.VBLF + "  TO_CHAR(SEQDATE,'YYYY-MM-DD') BDATE, REPRINT PRTDATE,ROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU_REPRINT";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND REPRINT = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            if (VB.Left(cboPart.SelectedItem.ToString(), 2) != "**")
            {
                SQL += ComNum.VBLF + "  AND MCCLASS = '" + VB.Left(cboPart.SelectedItem.ToString(), 2) + "'";
            }
            SQL += ComNum.VBLF + "ORDER BY SEQNO DESC";

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

                    ssList_Sheet1.Rows.Count = nRead;

                    for (i = 0; i < nRead; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = CF.READ_PassName(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim());
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BDATE"].ToString().Trim() + "(";

                        switch (dt.Rows[i]["MCCLASS"].ToString().Trim())
                        {
                            case "00":
                                ssList_Sheet1.Cells[i, 5].Text += "진료사실증명서";
                                break;
                            case "01":
                                ssList_Sheet1.Cells[i, 5].Text += "진단서";
                                break;
                            case "08":
                                ssList_Sheet1.Cells[i, 5].Text += "소견서";
                                break;
                            case "18":
                                ssList_Sheet1.Cells[i, 5].Text += "진료의뢰서";
                                break;
                            case "26":
                                ssList_Sheet1.Cells[i, 5].Text += "의료급여의뢰서";
                                break;
                            case "14":
                                ssList_Sheet1.Cells[i, 5].Text += "건강진단서";
                                break;
                            case "21":
                                ssList_Sheet1.Cells[i, 5].Text += "출생증명서";
                                break;
                            case "27":
                                ssList_Sheet1.Cells[i, 5].Text += "응급환자진료의뢰서";
                                break;
                            case "02":
                                ssList_Sheet1.Cells[i, 5].Text += "상해진단서";
                                break;
                            case "03":
                                ssList_Sheet1.Cells[i, 5].Text += "병무용진단서";
                                break;
                            case "05":
                                ssList_Sheet1.Cells[i, 5].Text += "사망진단서";
                                break;
                            case "19":
                                ssList_Sheet1.Cells[i, 5].Text += "장애인증명서";
                                break;
                            case "20":
                                ssList_Sheet1.Cells[i, 5].Text += "장애진단서";
                                break;
                            case "29":
                                ssList_Sheet1.Cells[i, 5].Text += "근로능력평가진단서";
                                break;
                            case "31":
                                ssList_Sheet1.Cells[i, 5].Text += "의료급여연장신청서";
                                break;
                        }

                        ssList_Sheet1.Cells[i, 5].Text += ")";

                        ssList_Sheet1.Cells[i, 6].Text = "신청자" + dt.Rows[i]["SINNAME"].ToString().Trim() + ComNum.VBLF;
                        ssList_Sheet1.Cells[i, 6].Text += "신청사유:" + dt.Rows[i]["SINSAYU"].ToString().Trim();

                        if (dt.Rows[i]["BIGO"].ToString().Trim() != "")
                        {
                            ssList_Sheet1.Cells[i, 6].Text += "비고:" + dt.Rows[i]["BIGO"].ToString().Trim();
                        }

                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        ssList_Sheet1.Rows[i].Height = 20;

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

        void HISTORY_PRINT()
        {
            int i = 0;
            string strDate = "";
            string strGubun = "";

            string SQL1 = "";
            string SQL2 = "";
            string SQL3 = "";
            string SQL4 = "";
            string SQL5 = "";
            string SQL6 = "";
            string SQL7 = "";
            string SQL8 = "";
            string SQL9 = "";
            string SQL10 = "";
            string SQL11 = "";
            string SQL12 = "";
            string SQL13 = "";
            string SQL14 = "";
            string SQL15 = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strGubun = VB.Left(cboPart.SelectedItem.ToString(), 2);

            strDate = dtpDate.Text;

            ssList_Sheet1.Rows.Count = 0;

            //========================================================================================↓(수정본)
            if (optSort0.Checked == true && String.Compare(strDate, "2011-06-29") <= 0)
            {
                ComFunc.MsgBox("2011-06-29 이전 리스트는 조회할 수 없습니다.");
                return;
            }

            #region SQL1
            SQL1 = "";
            SQL1 += ComNum.VBLF + "SELECT";
            SQL1 += ComNum.VBLF + " A.SEQNO, A.PANO, A.IPDOPD, A.DEPTCODE, A.REMARK, A.BDATE, B.SNAME, A.PRTDATE";
            SQL1 += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_WONSELU A, " + ComNum.DB_PMPA + "BAS_PATIENT B";
            SQL1 += ComNum.VBLF + "WHERE 1=1";
            if (optSort0.Checked == true)
            {
                SQL1 += ComNum.VBLF + " AND A.PRTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL1 += ComNum.VBLF + " AND A.ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }

            SQL1 += ComNum.VBLF + "     AND A.PANO = B.PANO(+) ";
            SQL1 += ComNum.VBLF + "     AND (SEQNO IS NOT NULL OR SEQNO <>'')";
            #endregion

            //========================================================================================↑(수정본)

            #region SQL2
            SQL2 = "";
            SQL2 += ComNum.VBLF + "SELECT";
            SQL2 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '진단서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE";
            SQL2 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI01 B";
            SQL2 += ComNum.VBLF + "WHERE 1=1";
            SQL2 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL2 += ComNum.VBLF + "     AND A.MCCLASS = '01'";
            if (optSort0.Checked == true)
            {
                SQL2 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL2 += ComNum.VBLF + " AND B.LSDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            SQL2 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            #region SQL3
            SQL3 = "";
            SQL3 += ComNum.VBLF + "SELECT";
            SQL3 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '소견서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE";
            SQL3 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI08 B";
            SQL3 += ComNum.VBLF + "WHERE 1=1";
            SQL3 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL3 += ComNum.VBLF + "     AND A.MCCLASS = '08'";
            if (optSort0.Checked == true)
            {
                SQL3 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL3 += ComNum.VBLF + " AND B.LSDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            SQL3 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            #region SQL4
            SQL4 = "";
            SQL4 += ComNum.VBLF + "SELECT";
            SQL4 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '진료의뢰서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE";
            SQL4 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI18 B";
            SQL4 += ComNum.VBLF + "WHERE 1=1";
            SQL4 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL4 += ComNum.VBLF + "     AND A.MCCLASS = '18'";
            if (optSort0.Checked == true)
            {
                SQL4 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL4 += ComNum.VBLF + " AND B.LSDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            SQL4 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            #region SQL5
            SQL5 = "";
            SQL5 += ComNum.VBLF + "SELECT";
            SQL5 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD, '' DEPTCODE, '의료급여의뢰서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE";
            SQL5 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI26 B";
            SQL5 += ComNum.VBLF + "WHERE 1=1";
            SQL5 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL5 += ComNum.VBLF + "     AND A.MCCLASS = '26'";
            if (optSort0.Checked == true)
            {
                SQL5 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL5 += ComNum.VBLF + " AND B.BALDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            SQL5 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            #region SQL6
            SQL6 = "";
            SQL6 += ComNum.VBLF + "SELECT";
            SQL6 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '응급환자진료의뢰서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE";
            SQL6 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI27 B";
            SQL6 += ComNum.VBLF + "WHERE 1=1";
            SQL6 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL6 += ComNum.VBLF + "     AND A.MCCLASS = '27'";
            if (optSort0.Checked == true)
            {
                SQL6 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL6 += ComNum.VBLF + " AND B.LSDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            SQL6 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            //add 상해,병사,사망,장애증명,장애진단
            #region SQL7 
            SQL7 = "";
            SQL7 += ComNum.VBLF + "SELECT";
            SQL7 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '상해진단서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE";
            SQL7 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI02 B";
            SQL7 += ComNum.VBLF + "WHERE 1=1";
            SQL7 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL7 += ComNum.VBLF + "     AND A.MCCLASS = '02'";
            if (optSort0.Checked == true)
            {
                SQL7 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL7 += ComNum.VBLF + " AND B.LSDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            SQL7 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            #region SQL8
            SQL8 = "";
            SQL8 += ComNum.VBLF + "SELECT";
            SQL8 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '병무용진단서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE";
            SQL8 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI03 B";
            SQL8 += ComNum.VBLF + "WHERE 1=1";
            SQL8 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL8 += ComNum.VBLF + "     AND A.MCCLASS = '03'";
            if (optSort0.Checked == true)
            {
                SQL8 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL8 += ComNum.VBLF + " AND B.LSDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            SQL8 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            #region SQL9
            SQL9 = "";
            SQL9 += ComNum.VBLF + "SELECT";
            SQL9 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '사망진단서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE";
            SQL9 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI05 B";
            SQL9 += ComNum.VBLF + "WHERE 1=1";
            SQL9 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL9 += ComNum.VBLF + "     AND A.MCCLASS = '05'";
            if (optSort0.Checked == true)
            {
                SQL9 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL9 += ComNum.VBLF + " AND B.LSDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            SQL9 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            #region SQL10
            SQL10 = "";
            SQL10 += ComNum.VBLF + "SELECT";
            SQL10 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD,  (SELECT DRDEPT1 FROM ADMIN.BAS_DOCTOR C WHERE B.DRCODE = C.DRCODE) DEPTCODE, '장애인증명서' REMARK, '' BDATE, B.TNAME SNAME, A.SEQDATE PRTDATE";
            SQL10 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI19 B";
            SQL10 += ComNum.VBLF + "WHERE 1=1";
            SQL10 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL10 += ComNum.VBLF + "     AND A.MCCLASS = '19'";
            if (optSort0.Checked == true)
            {
                SQL10 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL10 += ComNum.VBLF + " AND B.LSDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            SQL10 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            #region SQL11
            SQL11 = "";
            SQL11 += ComNum.VBLF + "SELECT";
            SQL11 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '장애진단서' REMARK, '' BDATE, NAME SNAME, A.SEQDATE PRTDATE";
            SQL11 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI22 B";
            SQL11 += ComNum.VBLF + "WHERE 1=1";
            SQL11 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL11 += ComNum.VBLF + "     AND A.MCCLASS = '22'";
            if (optSort0.Checked == true)
            {
                SQL11 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL11 += ComNum.VBLF + " AND B.LSDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            SQL11 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            #region SQL12
            SQL12 = "";
            SQL12 += ComNum.VBLF + "SELECT";
            SQL12 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '근로능력평가진단서' REMARK, '' BDATE, SNAME, A.SEQDATE PRTDATE";
            SQL12 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI29 B";
            SQL12 += ComNum.VBLF + "WHERE 1=1";
            SQL12 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL12 += ComNum.VBLF + "     AND A.MCCLASS = '29'";
            if (optSort0.Checked == true)
            {
                SQL12 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL12 += ComNum.VBLF + " AND B.LSDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            SQL12 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            #region SQL13
            SQL13 = "";
            SQL13 += ComNum.VBLF + "SELECT";
            SQL13 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD, B.DEPTCODE, '건강진단서' REMARK, '' BDATE, B.SNAME, A.SEQDATE PRTDATE";
            SQL13 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI14 B";
            SQL13 += ComNum.VBLF + "WHERE 1=1";
            SQL13 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL13 += ComNum.VBLF + "     AND A.MCCLASS = '14'";
            if (optSort0.Checked == true)
            {
                SQL13 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL13 += ComNum.VBLF + " AND B.LSDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            SQL13 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            #region SQL14
            SQL14 = "";
            SQL14 += ComNum.VBLF + "SELECT";
            SQL14 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD, '' DEPTCODE, '출생증명서' REMARK, '' BDATE, B.NAME SNAME, A.SEQDATE PRTDATE";
            SQL14 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI21 B";
            SQL14 += ComNum.VBLF + "WHERE 1=1";
            SQL14 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL14 += ComNum.VBLF + "     AND A.MCCLASS = '21'";
            if (optSort0.Checked == true)
            {
                SQL14 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL14 += ComNum.VBLF + " AND B.LSDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            SQL14 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            #region SQL15
            SQL15 = "";
            SQL15 += ComNum.VBLF + "SELECT";
            SQL15 += ComNum.VBLF + " A.SEQNO, B.PTNO PANO, '' IPDOPD, '' DEPTCODE, '의료급여연장신청' REMARK, '' BDATE, B.SNAME SNAME, A.SEQDATE PRTDATE";
            SQL15 += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU A, " + ComNum.DB_MED + "OCS_MCCERTIFI31 B";
            SQL15 += ComNum.VBLF + "WHERE 1=1";
            SQL15 += ComNum.VBLF + "     AND a.MCNO = b.MCNO";
            SQL15 += ComNum.VBLF + "     AND A.MCCLASS = '31'";
            if (optSort0.Checked == true)
            {
                SQL15 += ComNum.VBLF + " AND A.SEQDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
            }
            else
            {
                SQL15 += ComNum.VBLF + " AND B.BALDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            }
            SQL15 += ComNum.VBLF + "     AND SEND = 'P' ";
            #endregion


            switch (strGubun)
            {
                case "**":
                    SQL = SQL1 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL2 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL3 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL4 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL5 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL6 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL7 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL8 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL9 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL10 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL11 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL12 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL13 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL14 + ComNum.VBLF + "UNION ALL" + ComNum.VBLF +
                          SQL15;
                    break;

                case "00":
                    SQL = SQL1;
                    break;

                case "01":
                    SQL = SQL2;
                    break;

                case "08":
                    SQL = SQL3;
                    break;

                case "18":
                    SQL = SQL4;
                    break;

                case "26":
                    SQL = SQL5;
                    break;

                case "27":
                    SQL = SQL6;
                    break;

                case "02":
                    SQL = SQL7;
                    break;

                case "03":
                    SQL = SQL8;
                    break;

                case "05":
                    SQL = SQL9;
                    break;

                case "19":
                    SQL = SQL10;
                    break;

                case "20":
                    SQL = SQL11;
                    break;

                case "29":
                    SQL = SQL12;
                    break;

                case "14":
                    SQL = SQL13;
                    break;

                case "21":
                    SQL = SQL14;
                    break;

                case "31":
                    SQL = SQL15;
                    break;
            }

            SQL += ComNum.VBLF + "ORDER BY SEQNO DESC";

            //========================================================================================↓(원본)
            //    strSQL = " SELECT A.SEQNO, A.PANO, A.IPDOPD, A.DEPTCODE, A.REMARK,A.BDATE, B.SNAME "
            //    strSQL = strSQL & " FROM ETC_WONSELU A, BAS_PATIENT B"
            //    strSQL = strSQL & " WHERE A.ACTDATE = TO_DATE('" & strDate & "','YYYY - MM - DD')  "
            //    strSQL = strSQL & "   AND A.PANO = B.PANO(+) "
            //    strSQL = strSQL & "   AND (SEQNO IS NOT NULL OR SEQNO <>'') "
            //    strSQL = strSQL & " ORDER BY A.SEQNO DESC "
            //========================================================================================↑(원본)

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
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IPDOPD"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = "";

                        ssList_Sheet1.Rows[i].Height = 20;
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

        void ssList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strRowid = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;



            strRowid = ssList_Sheet1.Cells[e.Row, 7].Text;

            if (ssList_Sheet1.Cells[e.Row, 7].Text != "")
            {
                if (MessageBox.Show("재당 환자의 재증명 재발급 내용을 삭제처리 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    SQL = "";
                    SQL += ComNum.VBLF + "DELETE " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU_REPRINT";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND ROWID  = '" + strRowid + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    try
                    {

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                        ComFunc.MsgBox("삭제하였습니다.");
                        Cursor.Current = Cursors.Default;
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
                else
                {
                    return;
                }

                eGetData();
            }
        }
    }
}
