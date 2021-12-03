using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewJepsuProxy.cs
    /// Description     : 대리접수자 명단(기록실)
    /// Author          : 안정수
    /// Create Date     : 2017-07-06
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\ovchrt\Frm대리접수자명단.frm(Frm대리접수자명단) => frmPmpaViewJepsuProxy.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\ovchrt\Frm대리접수자명단.frm(Frm대리접수자명단)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewJepsuProxy : Form
    {
        clsSpread CS = new clsSpread();

        public frmPmpaViewJepsuProxy()
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
            this.btnCancel.Click += new EventHandler(eBtnEvent);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등                   

            this.StartPosition = FormStartPosition.CenterScreen;
            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            optSort0.Checked = true;

            SCREEN_CLEAR();
        }

        void SCREEN_CLEAR()
        {
            CS.Spread_All_Clear(ssList);
            ssList_Sheet1.Rows.Count = 50;

            btnView.Enabled = true;
            btnCancel.Enabled = true;
            btnExit.Enabled = true;

            ssList.Enabled = false;
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
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }

            else if (sender == this.btnCancel)
            {
                SCREEN_CLEAR();
                btnView.Focus();
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (optGubun0.Checked == true)
            {
                strTitle = dtpDate.Value.ToShortDateString() + " 당일 대리접수 List";
                strHeader = SPR.setSpdPrint_String(strTitle, new Font("바탕체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 45, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, true, false);
                SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else
            {
                strTitle = dtpDate.Value.ToShortDateString() + " 당일 대리접수 삭제 List";
                strHeader = SPR.setSpdPrint_String(strTitle, new Font("바탕체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 45, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, true, false);
                SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            
        }

        void eGetData()
        {
            int i = 0;
            int nREAD = 0;

            string strRDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            CS.Spread_All_Clear(ssList);
            Cursor.Current = Cursors.WaitCursor;

            strRDate = dtpDate.Text;
            progressBar1.Value = 0;

            //if (optGubun0.Checked == true)
            //{
            //    ssList_Sheet1.Columns[9].Visible = false;
            //    ssList_Sheet1.Columns[10].Visible = false;
            //}
            //else
            //{
            //    ssList_Sheet1.Columns[9].Visible = true;
            //    ssList_Sheet1.Columns[10].Visible = true;
            //}

            try
            {
                //SQL += ComNum.VBLF + "SELECT                                                                        ";
                //SQL += ComNum.VBLF + "  a.Pano,c.SName,a.ActDate,a.Part,a.DeptCode,                                 ";
                //SQL += ComNum.VBLF + "  a.STime,c.Hphone                                                            ";
                //SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SUNAP a, " + ComNum.DB_PMPA + "BAS_PATIENT c   ";
                //SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                //SQL += ComNum.VBLF + "      AND a.Pano = c.Pano(+)                                                   ";

                if (optGubun0.Checked == true)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "  SELECT ";
                    SQL += ComNum.VBLF + "       'NEW' GUBUN, A.PANO, C.SNAME, A.RDATE AS ACTDATE, A.ENTSABUN AS PART, A.DEPTCODE,A.RTIME AS STIME, C.HPHONE ";
                    SQL += ComNum.VBLF + "    FROM ADMIN.OPD_TELRESV A ";
                    SQL += ComNum.VBLF + "  INNER JOIN ADMIN.BAS_USER B ";
                    SQL += ComNum.VBLF + "      ON A.ENTSABUN = B.SABUN ";
                    SQL += ComNum.VBLF + "     AND B.JOBGROUP IN (";
                    SQL += ComNum.VBLF + "        SELECT BASCD";
                    SQL += ComNum.VBLF + "          FROM ADMIN.BAS_BASCD";
                    SQL += ComNum.VBLF + "         WHERE GRPCDB = '권한관리'";
                    SQL += ComNum.VBLF + "           AND GRPCD  = '의료정보팀'";
                    SQL += ComNum.VBLF + "        )";
                    //SQL += ComNum.VBLF + "    AND B.JOBGROUP IN ('JOB015001', 'JOB015002', 'JOB015003') ";
                    SQL += ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.BAS_PATIENT C ";
                    SQL += ComNum.VBLF + "    ON A.PANO = C.PANO ";
                    SQL += ComNum.VBLF + "  WHERE A.RDATE = TO_DATE('" + strRDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "      AND GBFLAG = 'Y' ";
                    SQL += ComNum.VBLF + "  UNION ALL ";
                    SQL += ComNum.VBLF + "  SELECT ";
                    SQL += ComNum.VBLF + "       'NEW' GUBUN, A.PANO, C.SNAME, A.RDATE AS ACTDATE, A.ENTSABUN AS PART, A.DEPTCODE,A.RTIME AS STIME, C.HPHONE ";
                    SQL += ComNum.VBLF + "    FROM ADMIN.OPD_TELRESV_DEL A ";
                    SQL += ComNum.VBLF + "  INNER JOIN ADMIN.BAS_USER B ";
                    SQL += ComNum.VBLF + "      ON A.ENTSABUN = B.SABUN ";
                    SQL += ComNum.VBLF + "     AND B.JOBGROUP IN (";
                    SQL += ComNum.VBLF + "        SELECT BASCD";
                    SQL += ComNum.VBLF + "          FROM ADMIN.BAS_BASCD";
                    SQL += ComNum.VBLF + "         WHERE GRPCDB = '권한관리'";
                    SQL += ComNum.VBLF + "           AND GRPCD  = '의료정보팀'";
                    SQL += ComNum.VBLF + "        )";
                    //SQL += ComNum.VBLF + "    AND B.JOBGROUP IN ('JOB015001', 'JOB015002', 'JOB015003')  ";
                    SQL += ComNum.VBLF + "  INNER JOIN ADMIN.BAS_USER D ";
                    SQL += ComNum.VBLF + "      ON A.DELSABUN = D.SABUN ";
                    //SQL += ComNum.VBLF + "    AND D.JOBGROUP NOT IN('JOB015001', 'JOB015002', 'JOB015003') ";
                    SQL += ComNum.VBLF + "     AND D.JOBGROUP NOT IN (";
                    SQL += ComNum.VBLF + "        SELECT BASCD";
                    SQL += ComNum.VBLF + "          FROM ADMIN.BAS_BASCD";
                    SQL += ComNum.VBLF + "         WHERE GRPCDB = '권한관리'";
                    SQL += ComNum.VBLF + "           AND GRPCD  = '의료정보팀'";
                    SQL += ComNum.VBLF + "        )";
                    SQL += ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.BAS_PATIENT C ";
                    SQL += ComNum.VBLF + "    ON A.PANO = C.PANO ";
                    SQL += ComNum.VBLF + "  WHERE A.RDATE = TO_DATE('" + strRDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "      AND GBFLAG = 'Y' ";
                    SQL += ComNum.VBLF + "  UNION ALL ";
                    SQL += ComNum.VBLF + "  SELECT  ";
                    SQL += ComNum.VBLF + "       'OLD' GUBN, A.Pano, C.SNAME, ActDate, Part, A.DeptCode, STime, C.HPHONE ";
                    SQL += ComNum.VBLF + "    FROM ADMIN.OPD_SUNAP A ";
                    SQL += ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.BAS_PATIENT C ";
                    SQL += ComNum.VBLF + "    ON A.PANO = C.PANO ";
                    SQL += ComNum.VBLF + "  WHERE a.Actdate = TO_DATE('" + strRDate + "','YYYY-MM-DD')                ";
                    SQL += ComNum.VBLF + "    AND a.Part = '333'                                                       ";
                    SQL += ComNum.VBLF + "    AND a.Remark = '대리'                                                    ";

                    if (optSort0.Checked == true)
                    {
                        SQL += ComNum.VBLF + "ORDER BY Sname                                                          ";
                    }
                    else if (optSort1.Checked == true)
                    {
                        SQL += ComNum.VBLF + "ORDER BY Pano                                                           ";
                    }
                    else if (optSort2.Checked == true)
                    {
                        SQL += ComNum.VBLF + "ORDER BY DeptCode, Pano                                                ";
                    }
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT  ";
                    SQL = SQL + ComNum.VBLF + "     'NEW' GUBUN, A.PANO, C.SNAME, A.RDATE AS ACTDATE, A.ENTSABUN AS PART, A.DEPTCODE, ";
                    SQL = SQL + ComNum.VBLF + "     A.RTIME AS STIME, C.HPHONE, A.DELDATE, A.DELSABUN   ";
                    SQL = SQL + ComNum.VBLF + "   FROM ADMIN.OPD_TELRESV_DEL A  ";
                    SQL = SQL + ComNum.VBLF + " INNER JOIN ADMIN.BAS_USER B   ";
                    SQL = SQL + ComNum.VBLF + "    ON A.DELSABUN = B.SABUN   ";
                    SQL = SQL + ComNum.VBLF + "   AND B.JOBGROUP IN (";
                    SQL = SQL + ComNum.VBLF + "        SELECT BASCD";
                    SQL = SQL + ComNum.VBLF + "          FROM ADMIN.BAS_BASCD";
                    SQL = SQL + ComNum.VBLF + "         WHERE GRPCDB = '권한관리'";
                    SQL = SQL + ComNum.VBLF + "           AND GRPCD  = '의료정보팀'";
                    SQL = SQL + ComNum.VBLF + "        )";
                    //SQL = SQL + ComNum.VBLF + "   AND B.JOBGROUP IN ('JOB015001', 'JOB015002', 'JOB015003') ";
                    SQL = SQL + ComNum.VBLF + " LEFT OUTER JOIN ADMIN.BAS_PATIENT C  ";
                    SQL = SQL + ComNum.VBLF + "    ON A.PANO = C.PANO        ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.DELDATE >= TO_DATE('" + strRDate + "','YYYY-MM-DD')      ";
                    SQL = SQL + ComNum.VBLF + "   AND A.DELDATE < TO_DATE('" + dtpDate.Value.AddDays(1).ToShortDateString() + "', 'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND GBFLAG = 'Y'  ";

                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;
                    ssList_Sheet1.Rows.Count = nREAD;
                    ssList_Sheet1.SetRowHeight(-1, 26);

                    progressBar1.Value = VB.Fix((i + 1) / nREAD * 100);

                    for (i = 0; i < nREAD; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                                                                 ";
                        SQL += ComNum.VBLF + "  DrCode                                                                                               ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                                  ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                                                              ";
                        SQL += ComNum.VBLF + "      AND PANO ='" + dt.Rows[i]["Pano"].ToString().Trim() + "'                                         ";
                        SQL += ComNum.VBLF + "      AND ACTDATE =TO_DATE('" + VB.Left(dt.Rows[i]["ActDate"].ToString().Trim(), 10) + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND DEPTCODE ='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'                                 ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssList_Sheet1.Cells[i, 3].Text = clsVbfunc.GetOCSDrCodeDrName(clsDB.DbCon, dt1.Rows[0]["DrCode"].ToString().Trim());
                            ssList_Sheet1.Cells[i, 8].Text = dt1.Rows[0]["DrCode"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;

                        ssList_Sheet1.Cells[i, 4].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["Part"].ToString().Trim());
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["STime"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Hphone"].ToString().Trim();

                        if (optGubun1.Checked == true)
                        {
                            ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DELDATE"].ToString().Trim();
                            ssList_Sheet1.Cells[i, 10].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["DELSABUN"].ToString().Trim());
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

            progressBar1.Value = 100;
            ssList.Enabled = true;
            btnCancel.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void ssList_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssList, e.Column);
                return;
            }
        }

        private void ssList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strPANO = "";
            string strDeptCode = "";
            string strDrCode = "";

            if (optGubun1.Checked == true) return;

            ssList_Sheet1.Rows[e.Row].ForeColor = Color.OrangeRed;
            if (ComFunc.MsgBoxQ("해당 당일 대리접수를 삭제 하시겠습니까 ?") == DialogResult.No)
            {
                ssList_Sheet1.Rows[e.Row].ForeColor = Color.Black;
                return;
            }

            strPANO = ssList_Sheet1.Cells[e.Row, 0].Text;
            strDeptCode = ssList_Sheet1.Cells[e.Row, 2].Text;
            strDrCode = ssList_Sheet1.Cells[e.Row, 8].Text;

            if (strPANO == "")
            {
                ComFunc.MsgBox("등록번호가 공란입니다.", "오류");
                return;
            }

            if (strDeptCode == "")
            {
                ComFunc.MsgBox("진료과가 공란입니다.", "오류");
                return;
            }

            if (strDrCode == "")
            {
                ComFunc.MsgBox("진료과장을 선택하지 않았습니다.", "오류");
                return;
            }

            clsPublic.GstrHelpCode = "";

            frmPmpaTodayJupsuCancel frmPmpaTodayJupsuCancelX = new frmPmpaTodayJupsuCancel(strPANO, strDeptCode);
            frmPmpaTodayJupsuCancelX.StartPosition = FormStartPosition.CenterParent;
            frmPmpaTodayJupsuCancelX.ShowDialog();

            if (clsPublic.GstrHelpCode == "OK")
            {
                DeleteJupsu(strPANO, strDeptCode, strDrCode);
                optGubun1.Checked = true;
            }
            else
            {
                ssList_Sheet1.Rows[e.Row].ForeColor = Color.Black;
            }          
        }

        private bool DeleteJupsu(string strPANO, string strDeptCode, string strDrCode)
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {                
                //'2014-10-14 삭제내역 복사
                SQL = " INSERT INTO ADMIN.OPD_TELRESV_DEL ( ";
                SQL = SQL + ComNum.VBLF + "       RDATE,RTIME,PANO,SNAME,DEPTCODE,DRCODE,ENTDATE,ENTSABUN,GBINTERNET,   ";
                SQL = SQL + ComNum.VBLF + "       GBDrug,GbChojin,GBCHK,GBFLAG,GBSPC,GWACHOJAE,GKIHO,DELDATE,DELSABUN,  ";
                SQL = SQL + ComNum.VBLF + "       Gubun,P_Exam,P_Remark)                                                ";
                SQL = SQL + ComNum.VBLF + "SELECT RDATE,RTIME,PANO,SNAME,DEPTCODE,DRCODE,ENTDATE,ENTSABUN,GBINTERNET,   ";
                SQL = SQL + ComNum.VBLF + "       GBDrug,GbChojin,GBCHK,GBFLAG,GBSPC,GWACHOJAE,GKIHO,SYSDATE,'" + clsType.User.Sabun + "', ";
                SQL = SQL + ComNum.VBLF + "       Gubun,P_Exam,P_Remark ";
                SQL = SQL + ComNum.VBLF + " From ADMIN.OPD_TELRESV ";
                SQL = SQL + ComNum.VBLF + "WHERE RDATE = TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND PANO = '" + strPANO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DEPTCODE = '" + strDeptCode + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                SQL = "DELETE ADMIN.OPD_TELRESV ";
                SQL = SQL + ComNum.VBLF + "WHERE RDATE = TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "  AND PANO = '" + strPANO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DEPTCODE = '" + strDeptCode + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;

                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void optGubun0_CheckedChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            eGetData();
        }

        private void optGubun1_CheckedChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            eGetData();
        }
    }
}
