using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewNotJinList.cs
    /// Description     : 당일접수후 미진료 리스트 
    /// Author          : 안정수
    /// Create Date     : 2017-09-08
    /// Update History  : 2017-11-03
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm당일접수미진료리스트.frm(Frm당일접수미진료리스트) => frmPmpaViewNotJinList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm당일접수미진료리스트.frm(Frm당일접수미진료리스트)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewNotJinList : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        public frmPmpaViewNotJinList()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnChk.Click += new EventHandler(eBtnEvent);
            this.btnChk3.Click += new EventHandler(eBtnEvent);
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

                btnView_Click();

            }

            else if (sender == this.btnCommand1)
            {
                btnCommand1_Click();

            }

            else if (sender == this.btnChk)
            {
                btnChk_Click();
            }

            else if (sender == this.btnChk3)
            {
                btnChk3_Click();
            }
        }

        void btnCommand1_Click()
        {
            string strDate = "";

            ssList1_Sheet1.Rows.Count = 0;


            strDate = dtpDate.Text;

            btnView_Click();

            strDate = Convert.ToDateTime(strDate).AddDays(1).ToShortDateString();
            dtpDate.Text = strDate;

            if (String.Compare(strDate, "2010-01-24") > 0)
            {
                return;
            }
        }

        void btnView_Click()
        {
            string strDate = "";
            int i = 0;
            int nRead = 0;

            string strPano = "";
            string strDept = "";
            string strDrCode = "";
            string strSname = "";
            string strMCode = "";
            string strJin = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            DataTable dt3 = null;

            Cursor.Current = Cursors.WaitCursor;

            strDate = dtpDate.Text;
            ssList1_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                    ";
            SQL += ComNum.VBLF + "  PANO,SNAME,DEPTCODE,DRCODE,MCode,Part,Jin,Amt7          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND JIN NOT IN ('2','4','7','Q','C','D')            ";
            SQL += ComNum.VBLF + "      AND REP <> '#'                                      ";
            SQL += ComNum.VBLF + "      AND RESERVED = '0'                                  ";
            SQL += ComNum.VBLF + "      AND PANO  <> '81000004'                             ";
            SQL += ComNum.VBLF + "      AND DEPTCODE NOT IN ('HR','ER','TO')                ";
            SQL += ComNum.VBLF + "ORDER BY DEPTCODE                                         ";

            
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

                for (i = 0; i < nRead; i++)
                {                  
                    strPano = dt.Rows[i]["PANO"].ToString().Trim();
                    strDept = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    strDrCode = dt.Rows[i]["DRCODE"].ToString().Trim();
                    strSname = dt.Rows[i]["SNAME"].ToString().Trim();
                    strMCode = dt.Rows[i]["MCode"].ToString().Trim();
                    strJin = dt.Rows[i]["Jin"].ToString().Trim();  //2013-07-03

                    //2013-07-03 E전화예약상태 H는 접수비발생
                    if (strJin == "H")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                    ";
                        SQL += ComNum.VBLF + "  COUNT(*) CNT                                            ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER                      ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
                        SQL += ComNum.VBLF + "      AND PTNO = '" + strPano + "'                        ";
                        SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "      AND DEPTCODE = '" + strDept + "'                    ";
                        SQL += ComNum.VBLF + "      AND SUCODE = '$$33'                                 ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                                
                            return;
                        }

                        if (dt1.Rows.Count == 0)
                        {
                            dt1.Dispose();
                            dt1 = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            return;
                        }


                        if (Convert.ToInt32(dt1.Rows[0]["CNT"].ToString().Trim()) >= 1)
                        {
                            ssList1_Sheet1.Rows.Count += 1;

                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 0].Text = strPano;
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 1].Text = strSname;
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 2].Text = strDept;
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 3].Text = CF.READ_DrName(clsDB.DbCon, strDrCode);
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 4].Text = "전화-접수비체크환자 $$33 오더발생";
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 5].Text = dt.Rows[i]["Part"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }                        


                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                    ";
                    SQL += ComNum.VBLF + "  COUNT(*) CNT                                            ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP                       ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
                    SQL += ComNum.VBLF + "      AND PANO = '" + strPano + "'                        ";
                    SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "      AND DEPTCODE = '" + strDept + "'                    ";


                    SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt3.Rows.Count == 0)
                    {
                        dt3.Dispose();
                        dt3 = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        return;
                    }


                    if (Convert.ToInt32(dt3.Rows[0]["CNT"].ToString().Trim()) == 0)
                    {
                        //2015-11-25 $$35 진료후 진단서발급
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                    ";
                        SQL += ComNum.VBLF + "  COUNT(*) CNT                                            ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER                      ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
                        SQL += ComNum.VBLF + "      AND PTNO = '" + strPano + "'                        ";
                        SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "      AND DEPTCODE = '" + strDept + "'                    ";
                        SQL += ComNum.VBLF + "      AND SUCODE = '$$35'                                 ";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count == 0)
                        {
                            dt1.Dispose();
                            dt1 = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            return;
                        }


                        if (Convert.ToInt32(dt1.Rows[0]["CNT"].ToString().Trim()) >= 1)
                        {
                            ssList1_Sheet1.Rows.Count += 1;

                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 0].Text = strPano;
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 1].Text = strSname;
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 2].Text = strDept;
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 3].Text = CF.READ_DrName(clsDB.DbCon, strDrCode);
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 4].Text = "$$35";
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 5].Text = dt.Rows[i]["Part"].ToString().Trim();
                        }


                        dt1.Dispose();
                        dt1 = null;

                        //$$13:진료만받으신분, $$11:예약후진료오시지않음 $$12:결과확인후 경과관찰 $$20:진단서 발급
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                    ";
                        SQL += ComNum.VBLF + "  COUNT(*) CNT                                            ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER                      ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
                        SQL += ComNum.VBLF + "      AND PTNO = '" + strPano + "'                        ";
                        SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "      AND DEPTCODE = '" + strDept + "'                    ";
                        SQL += ComNum.VBLF + "      AND SUCODE = '$$21'                                 ";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count == 0)
                        {
                            dt1.Dispose();
                            dt1 = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            return;
                        }

                        if (Convert.ToInt32(dt1.Rows[0]["CNT"].ToString().Trim()) >= 1)
                        {
                            ssList1_Sheet1.Rows.Count += 1;

                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 0].Text = strPano;
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 1].Text = strSname;
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 2].Text = strDept;
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 3].Text = CF.READ_DrName(clsDB.DbCon, strDrCode);
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 4].Text = "$$21   접수비: " + VB.Val(dt.Rows[i]["AMT7"].ToString().Trim() + "원");
                            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 5].Text = dt.Rows[i]["Part"].ToString().Trim();
                        }

                        else
                        {
                            //차상위 E,F 환자 $$13 진료만 봄 환자 체크 2010-01-25
                            if (strMCode == "E000" || strMCode == "F000")
                            {
                                //dt1.Dispose();
                                //dt1 = null;

                                SQL = "";
                                SQL += ComNum.VBLF + "SELECT                                                    ";
                                SQL += ComNum.VBLF + "  COUNT(*) CNT                                            ";
                                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER                      ";
                                SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
                                SQL += ComNum.VBLF + "      AND PTNO = '" + strPano + "'                        ";
                                SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                                SQL += ComNum.VBLF + "      AND DEPTCODE = '" + strDept + "'                    ";
                                SQL += ComNum.VBLF + "      AND SUCODE IN ('$$13','$$12')                       ";  //2013-06-17
                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                                if (dt1.Rows.Count == 0)
                                {
                                    dt1.Dispose();
                                    dt1 = null;
                                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                                    return;
                                }

                                if (Convert.ToInt32(dt1.Rows[0]["CNT"].ToString().Trim()) >= 1)
                                {
                                    ssList1_Sheet1.Rows.Count += 1;

                                    ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 0].Text = strPano;
                                    ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 1].Text = strSname;
                                    ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 2].Text = strDept;
                                    ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 3].Text = CF.READ_DrName(clsDB.DbCon, strDrCode);
                                    ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 4].Text = "$$13,$$12 - E,F 진료만 볼경우";
                                    ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 5].Text = dt.Rows[i]["Part"].ToString().Trim();
                                }
                                dt1.Dispose();
                                dt1 = null;
                            }

                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT                                                    ";
                            SQL += ComNum.VBLF + "  COUNT(*) CNT                                            ";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER                      ";
                            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
                            SQL += ComNum.VBLF + "      AND PTNO = '" + strPano + "'                        ";
                            SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "      AND DEPTCODE = '" + strDept + "'                    ";

                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                            if (dt2.Rows.Count == 0)
                            {
                                dt2.Dispose();
                                dt2 = null;
                                ComFunc.MsgBox("해당 DATA가 없습니다.");
                                return;
                            }

                            if (Convert.ToInt32(VB.Val(dt2.Rows[0]["CNT"].ToString().Trim())) == 0)
                            {
                                ssList1_Sheet1.Rows.Count += 1;

                                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 0].Text = strPano;
                                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 1].Text = strSname;
                                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 2].Text = strDept;
                                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 3].Text = CF.READ_DrName(clsDB.DbCon, strDrCode);
                                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 4].Text = "오더 내역이 없습니다.";
                                ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 5].Text = dt.Rows[i]["Part"].ToString().Trim();
                            }

                            dt2.Dispose();
                            dt2 = null;
                        }                            
                    }

                    dt3.Dispose();
                    dt3 = null;
                }

                dt.Dispose();
                dt = null;

                ssList1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            }

            Cursor.Current = Cursors.Default;
            ComFunc.MsgBox("조회가 완료 되었습니다.");
        }

        void btnChk_Click()
        {
            int i = 0;
            string strDate = "";
            int nRead = 0;
            string strPano = "";
            string strDept = "";

            int OAmt = 0;
            int CAmt = 0;            

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            strDate = dtpDate.Text;

            CS.Spread_All_Clear(ssList2);

            Cursor.Current = Cursors.WaitCursor;

            //현금영수 30만 이상 의무사항 점$검
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                    ";
            SQL += ComNum.VBLF + "  PANO,DEPTCODE,SUM(AMT1+AMT2) AMT                        ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP                       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "      AND ActDate =TO_DATE('" + strDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND BUN ='99'                                       ";
            SQL += ComNum.VBLF + "GROUP BY PANO,DEPTCODE                                    ";
            SQL += ComNum.VBLF + "HAVING SUM(AMT1 + AMT2) >= 300000                         ";
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

                    for (i = 0; i < nRead; i++)
                    {
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strDept = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        OAmt = Convert.ToInt32(dt.Rows[i]["AMT"].ToString().Trim());

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                        ";
                        SQL += ComNum.VBLF + "  SUM(DECODE(TranHeader,'2',TradeAmt * -1, TradeAmt)) CardAmt ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV                        ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                        SQL += ComNum.VBLF + "      AND ActDate =TO_DATE('" + strDate + "','YYYY-MM-DD')    ";
                        SQL += ComNum.VBLF + "      AND PANO ='" + strPano + "'                             ";
                        SQL += ComNum.VBLF + "      AND DEPTCODE IN ('" + strDept + "', 'II')               ";  //II과 포함함
                        SQL += ComNum.VBLF + "      AND GUBUN  IN ('1','2','3')                             ";  //현금, 카드 포함
                        SQL += ComNum.VBLF + "      AND PTGUBUN IN ('1','3')                                ";  //코세스카드만
                        SQL += ComNum.VBLF + "      AND GBIO ='O'                                           ";  //외래만

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            CAmt = Convert.ToInt32(VB.Val(dt1.Rows[0]["CardAMT"].ToString().Trim()));

                            if (OAmt > CAmt)
                            {
                                ssList2_Sheet1.Rows.Count += 1;

                                ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 0].Text = strPano;
                                ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 1].Text = strDept;
                                ssList2_Sheet1.Cells[ssList2_Sheet1.Rows.Count - 1, 2].Text = "※외래발생금:" + OAmt + "  ※카드+현금승인:" + CAmt;
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        ssList2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
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
            Cursor.Current = Cursors.Default;
        }

        void btnChk3_Click()
        {
            int i = 0;
            string strDate = "";
            int nRead = 0;
            string strPano = "";
            string strSname = "";
            string strDept = "";

            int nRow = 0;

            int CardAmt = 0;

            Cursor.Current = Cursors.WaitCursor;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            strDate = dtpDate.Text.Trim();
            CS.Spread_All_Clear(ssList3);

            //카드결제기준
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                    ";
            SQL += ComNum.VBLF + "  Pano,SName,DeptCode,Gubun,TradeAmt                      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV                    ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "      AND ACTDATE =TO_DATE('" + strDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND PTGUBUN  IN ('1','3')                           "; //코세스카드만
            SQL += ComNum.VBLF + "      AND Gubun ='1'                                      ";
            SQL += ComNum.VBLF + "ORDER BY Pano                                             ";

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
                ssList3_Sheet1.Rows.Count = nRead;

                for (i = 0; i < nRead; i++)
                {
                    strPano = dt.Rows[i]["PANO"].ToString().Trim();
                    strSname = dt.Rows[i]["SName"].ToString().Trim();
                    strDept = dt.Rows[i]["DeptCode"].ToString().Trim();

                    CardAmt = Convert.ToInt32(dt.Rows[i]["TradeAmt"].ToString().Trim());

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                            ";
                    SQL += ComNum.VBLF + "  SUM(DECODE(TranHeader,'2', TradeAmt * -1, TradeAmt )) CardAmt   ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "CARD_APPROV                            ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
                    SQL += ComNum.VBLF + "      AND ACTDATE =TO_DATE('" + strDate + "','YYYY-MM-DD')        ";
                    SQL += ComNum.VBLF + "      AND PANO ='" + strPano + "'                                 ";
                    SQL += ComNum.VBLF + "      AND TradeAmt =" + CardAmt + "                               ";
                    SQL += ComNum.VBLF + "      AND DeptCode ='" + strDept + "'                             ";
                    SQL += ComNum.VBLF + "      AND GUBUN  = '2'                                            ";
                    SQL += ComNum.VBLF + "      AND PTGUBUN IN ('1','3')                                    "; //코세스 카드만
                    SQL += ComNum.VBLF + "HAVING SUM(DECODE(TranHeader,'2', TradeAmt * -1, TradeAmt ))  > 0 ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {                         
                        ssList3_Sheet1.Cells[nRow, 0].Text = strPano;
                        ssList3_Sheet1.Cells[nRow, 1].Text = strSname;
                        ssList3_Sheet1.Cells[nRow, 2].Text = strDept + "과 ※카드와 현금승인 동일금액존재:" + CardAmt;                                                        
                        nRow++;                            
                    }

                    dt1.Dispose();
                    dt1 = null;

                    ssList3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                }
            }
            dt.Dispose();
            dt = null;

            ssList3_Sheet1.Rows.Count = nRow;

            Cursor.Current = Cursors.Default;
        }
    }
}

