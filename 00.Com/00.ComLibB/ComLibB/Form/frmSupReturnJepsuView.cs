using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    public partial class frmSupReturnJepsuView : Form
    {
        //이벤트를 전달할 경우
        public delegate void GetPatientInfo(string strPano, string strName, string strRDate, string strRemark, string RowId, string strDate);
        public event GetPatientInfo rGetPatientInfo;

        //폼이 Close 될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmSupReturnJepsuView()
        {
            InitializeComponent();
        }

        private void frmSupReturnJepsuView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등    

            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpDate.Text = clsPublic.GstrSysDate;
            dtpDateFrom.Text = clsPublic.GstrSysDate;

            SCREEN_CLEAR();

            //'기록실,고객지원과가 아니면 삭제불가
            if (VB.Left(clsType.User.BuseCode, 4) != "0442" && VB.Left(clsType.User.BuseCode, 4) != "0783" &&
                VB.Left(clsType.User.BuseCode, 4) != "0772" && VB.Left(clsType.User.BuseCode, 4) != "0774")
            {
                if(clsType.User.Sabun != "19610" && clsType.User.Sabun != "34009" )
                {
                    btnDelete.Visible = false;
                }
            }

            if (clsType.User.Sabun == "21403")
            {
                btnDelete.Visible = true; 
            }

            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept, "1", 1);
            cboDoct.Items.Clear();
            cboDoct.Items.Add("****.전체");
            cboDoct.SelectedIndex = 0;

            //optSort0.Checked = true;
            //optJep0.Checked = true;

        }

        private void SCREEN_CLEAR()
        {
            ss1_Sheet1.RowCount = 0;
            
            btnDelete.Enabled = false;
            btnCancel.Enabled = true;
            btnExit.Enabled = true;
            ss1.Enabled = false;


        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            btnSearchClick();
            ss1.Enabled = true;
            if (optJep0.Checked == true)
            {
                btnDelete.Enabled = true;
            }
            btnCancel.Enabled = true;
        }

        private void btnSearchClick()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string StrRDate = "";
            string StrRDateFrom = "";

            ss1_Sheet1.Rows.Count = 0;
            StrRDate = dtpDate.Text;
            StrRDateFrom = dtpDateFrom.Text;


            string strDept = "";
            string strDrcode = "";

            txtPano.Text = txtPano.Text.Length > 0 ? VB.Val(txtPano.Text).ToString("00000000") : "";

            strDept = VB.Left(cboDept.Text.Trim(), 2);
            strDrcode = VB.Left(cboDoct.Text.Trim(), 4);

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT RDATE, RTIME, PANO, SNAME, DEPTCODE, ";
                SQL = SQL + ComNum.VBLF + "        DRCODE, ENTSABUN, NAME, ROWID1, ";
                SQL = SQL + ComNum.VBLF + "        ENTDATE, GBINTERNET, GBDRUG, DEPTNAMEK, ";
                SQL = SQL + ComNum.VBLF + "        DRNAME, GBCHK, ETCJIN, GUBUN, BIRTH ";
                if (optJep1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + ", DELDATE, DELSABUN ";
                }

                SQL = SQL + ComNum.VBLF + " FROM  ( ";

                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(A.RDATE, 'YYYY-MM-DD') RDATE, a.Rtime,a.Pano,a.Sname,a.DeptCode,a.DrCode,a.EntSabun,";
                SQL = SQL + ComNum.VBLF + "       b.Name,a.ROWID ROWID1 ,TO_CHAR(a.EntDate,'YYYY-MM-DD HH24:MI') EntDate, ";
                SQL = SQL + ComNum.VBLF + "       GBINTERNET, GBDRUG, c.DeptNameK, d.DRNAME, a.GbChk, A.ETCJIN, DECODE(ENTSABUN, '999999', '모바일예약','전화예약') GUBUN,  ";
                SQL = SQL + ComNum.VBLF + "  (SELECT JUMIN1 FROM ADMIN.BAS_PATIENT SUB WHERE PANO = A.PANO AND ROWNUM = 1) BIRTH ";
                if (optJep0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  FROM OPD_TELRESV     a, ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ,TO_CHAR(a.DELDATE,'YYYY-MM-DD HH24:MI') DELDATE, DELSABUN ";
                    SQL = SQL + ComNum.VBLF + "  FROM OPD_TELRESV_Del     a, ";
                }
                SQL = SQL + ComNum.VBLF + "       BAS_PASS        b, ";
                SQL = SQL + ComNum.VBLF + "       BAS_ClinicDept  c,";
                SQL = SQL + ComNum.VBLF + "       BAS_DOCTOR      d";
                if (chkAllResv.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE a.RDATE >= TO_DATE('" + StrRDate + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   WHERE a.RDATE = TO_DATE('" + StrRDate + "','YYYY-MM-DD') ";
                }
                SQL = SQL + ComNum.VBLF + "   AND a.EntSabun = b.IdNumber(+) ";
                SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE = C.DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "   AND A.DRCODE  = D.DRCODE ";
                if (chkPosco.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.Gubun = '02' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND (A.Gubun = '01' OR A.Gubun = '' OR A.Gubun IS NULL ) ";
                }
                if (chkEtcJin.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.ETCJIN = '1'";
                }
                SQL = SQL + ComNum.VBLF + "   AND ( a.GbFlag ='N' OR a.GbFlag IS NULL ) ";
                SQL = SQL + ComNum.VBLF + "   AND b.ProgramID = ' ' ";

                SQL = SQL + ComNum.VBLF + "  UNION ALL ";

                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(A.DATE3,'YYYY-MM-DD') RDATE, TO_CHAR(A.DATE3,'HH24:MI') RTIME, A.PANO, A.SNAME, A.DEPTCODE, A.DRCODE, A.PART, ";
                SQL = SQL + ComNum.VBLF + "    B.NAME, A.ROWID ROWID1, TO_CHAR(DATE1, 'YYYY-MM-DD') ENTDATE,  ";
                SQL = SQL + ComNum.VBLF + "    'N' GBINTERNET, 'N' GBDRUG, C.DEPTNAMEK, D.DRNAME, '' GBCHK, '0' ETCJIN, 'OCS예약' GUBUN, ";
                SQL = SQL + ComNum.VBLF + "  (SELECT JUMIN1 FROM ADMIN.BAS_PATIENT SUB WHERE PANO = A.PANO AND ROWNUM = 1) BIRTH ";
                if (optJep0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ,TO_CHAR(a.RETDATE,'YYYY-MM-DD HH24:MI') DELDATE, RETPART ";
                }
                SQL = SQL + ComNum.VBLF + "    FROM OPD_RESERVED_NEW A, ";
                SQL = SQL + ComNum.VBLF + "         BAS_PASS B,  ";
                SQL = SQL + ComNum.VBLF + "         BAS_CLINICDEPT C, ";
                SQL = SQL + ComNum.VBLF + "         BAS_DOCTOR D ";
                if (chkAllResv.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "    WHERE DATE3 >= TO_DATE('" + StrRDate + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    WHERE DATE3 >= TO_DATE('" + StrRDate + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                    SQL = SQL + ComNum.VBLF + "      AND DATE3 <= TO_DATE('" + StrRDate + " 23:59', 'YYYY-MM-DD HH24:MI') ";
                }
                SQL = SQL + ComNum.VBLF + "      AND A.PART = B.IDNUMBER(+) ";
                SQL = SQL + ComNum.VBLF + "      AND b.ProgramID = ' ' ";
                SQL = SQL + ComNum.VBLF + "      AND A.DEPTCODE = C.DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "      AND A.DRCODE = D.DRCODE ";
                if (optJep0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "      AND RETDATE IS NULL ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "      AND RETDATE IS NOT NULL ";
                }

                SQL = SQL + ComNum.VBLF + " ) WHERE 1 = 1 ";

                

                if (rbGubun2.Checked == true || chkPosco.Checked == true || chkEtcJin.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND GUBUN = '전화예약'  ";
                }
                else if (rbGubun3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND GUBUN = 'OCS예약'  ";
                }

                if (txtName.Text.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + "      AND Sname Like '%" + txtName.Text.Trim() + "%'";
                }

                if (txtPano.Text.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + "      AND Pano = '" + txtPano.Text.Trim() + "'";
                }

                if (rbtnPart0.Checked == true)
                {
                }
                else
                {
                    if (optJep0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "      AND ENTSABUN IN ( ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "      AND DELSABUN IN ( ";
                    }

                    if (rbtnPart1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "          SELECT SABUN ";
                        SQL = SQL + ComNum.VBLF + "            FROM ADMIN.INSA_MST ";
                        SQL = SQL + ComNum.VBLF + "           WHERE BUSE IN (SELECT BUCODE FROM ADMIN.BAS_BUSE WHERE NAME LIKE '%의료정보%') ";
                    }
                    else if (rbtnPart2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "          SELECT SABUN ";
                        SQL = SQL + ComNum.VBLF + "            FROM ADMIN.INSA_MST ";
                        SQL = SQL + ComNum.VBLF + "           WHERE BUSE IN (SELECT MATCH_CODE FROM ADMIN.NUR_CODE WHERE GUBUN = '2')";
                    }
                    else if (rbtnPart3.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "          SELECT SABUN ";
                        SQL = SQL + ComNum.VBLF + "            FROM ADMIN.INSA_MST ";
                        SQL = SQL + ComNum.VBLF + "           WHERE BUSE IN (SELECT BUCODE FROM ADMIN.BAS_BUSE WHERE NAME LIKE '%원무%') ";
                    }
                    else if (rbtnPart4.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "          '999999' ";
                    }

                    SQL = SQL + ComNum.VBLF + "   ) ";
                }
                if (strDept != "**")
                {
                    SQL = SQL + ComNum.VBLF + "      AND DEPTCODE = '" + strDept + "'";
                }
                if (strDrcode != "****")
                {
                    SQL = SQL + ComNum.VBLF + "      AND DRCODE = '" + strDrcode + "'";
                }

                if (optSort0.Checked == true)
                {
                    //SQL = SQL + ComNum.VBLF + " ORDER BY Trim(Sname) ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY BIRTH ASC ";
                }
                if (optSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY Pano  ";
                }
                if (optSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY DeptCode ASC, DrCode, RTime";
                }
                if (optSort3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY EntSabun, Pano, RTime ";
                }

                

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.RowCount = dt.Rows.Count;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i, 0].Text = "";
                        ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        if (dt.Rows[i]["GbChk"].ToString().Trim() == "Y")
                        {
                            ss1_Sheet1.Cells[i, 2].BackColor = Color.FromArgb(255, 192, 255);
                        }
                        else
                        {
                            ss1_Sheet1.Cells[i, 2].BackColor = Color.FromArgb(255, 255, 255);
                        }

                        ss1_Sheet1.Cells[i, 3].Text = GetBirth(ss1_Sheet1.Cells[i, 1].Text.Trim());
                        ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrNAME"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["RDATE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["RTime"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Name"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["entdate"].ToString().Trim();
                        if (optJep1.Checked == true)
                        {
                            if (dt.Rows[i]["DELSABUN"].ToString().Trim() == "999999")
                            {
                                ss1_Sheet1.Cells[i, 10].Text = "모바일삭제";
                            }
                            else
                            { 
                                ss1_Sheet1.Cells[i, 10].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["DELSABUN"].ToString().Trim());
                            }
                            ss1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["DELDATE"].ToString().Trim();
                        }
                        if (dt.Rows[i]["GBINTERNET"].ToString().Trim() == "Y")
                        {
                            ss1_Sheet1.Cells[i, 12].Text = "인터넷접수 ";
                        }
                        if (dt.Rows[i]["GBDRUG"].ToString().Trim() == "Y")
                        {
                            ss1_Sheet1.Cells[i, 12].Text = "약접수";
                        }
                        if (dt.Rows[i]["ETCJIN"].ToString().Trim() == "1")
                        {
                            ss1_Sheet1.Cells[i, 12].Text += "선별진료";
                        }
                        ss1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["ROWID1"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 14].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 15].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ss1_Sheet1.Cells[i, 16].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        if (ss1_Sheet1.Cells[i, 16].Text.Trim() == "OCS예약")
                        {
                            ss1_Sheet1.Cells[i, 2].BackColor = Color.FromArgb(255, 204, 204);
                            ss1_Sheet1.Cells[i, 16].BackColor = Color.FromArgb(255, 204, 204);
                        }
                        else
                        {
                            ss1_Sheet1.Cells[i, 2].BackColor = Color.FromArgb(255, 255, 255);
                            ss1_Sheet1.Cells[i, 16].BackColor = Color.FromArgb(255, 255, 255);

                        }
                    }
                }
                else
                {
                    ComFunc.MsgBox("자료가 1건도 없습니다.");
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
        }

        string GetBirth(string strPano)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strVal = "";

            try
            {
                SQL = "SELECT JUMIN1 FROM BAS_PATIENT WHERE PANO = '" + strPano + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return strVal;
                }

                strVal = dt.Rows[0]["JUMIN1"].ToString().Trim();

                dt.Dispose();
                dt = null;
                return strVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strVal;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            btnSearchClick();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인
            btnDeleteClick();
            SCREEN_CLEAR();
            optJep1.Checked = true;
        }

        private bool btnDeleteClick()
        {
            bool rtnVal = false;
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;

            string strROWID    = "";
            string strPano     = "";
            string strDept     = "";
            string strDate = "";
            string strDateFrom = "";
            string strGUBUN = "";
            string strRDate = "";


            ComFunc.ReadSysDate(clsDB.DbCon);
            strDate = dtpDate.Text;
            strDateFrom = dtpDateFrom.Text;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                for (i = 0; i < ss1_Sheet1.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(ss1_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strPano = ss1_Sheet1.Cells[i, 1].Text;
                        strDept = ss1_Sheet1.Cells[i, 4].Text;
                        strROWID = ss1_Sheet1.Cells[i, 13].Text;

                        strGUBUN = ss1_Sheet1.Cells[i, 16].Text;
                        strRDate = ss1_Sheet1.Cells[i, 6].Text;

                        if (Check_Order(strPano, strDept, strRDate) == true)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("진료완료 환자입니다. 삭제 불가능합니다." + ComNum.VBLF + "등록번호: " + strPano + "  진료과: " + strRDate);
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if (strGUBUN == "OCS예약")
                        {

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE ADMIN.OPD_RESERVED_NEW SET  ";
                            SQL = SQL + ComNum.VBLF + " RETDATE = SYSDATE, ";
                            SQL = SQL + ComNum.VBLF + " RETAMT = 0, ";
                            SQL = SQL + ComNum.VBLF + " RETPART = '" + clsType.User.Sabun + "'";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND GBRES = '1' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                        else
                        {

                            //'2014-10-14 삭제내역 복사
                            SQL = " INSERT INTO OPD_TELRESV_DEL ( ";
                            SQL = SQL + ComNum.VBLF + "       RDATE,RTIME,PANO,SNAME,DEPTCODE,DRCODE,ENTDATE,ENTSABUN,GBINTERNET,   ";
                            SQL = SQL + ComNum.VBLF + "       GBDrug,GbChojin,GBCHK,GBFLAG,GBSPC,GWACHOJAE,GKIHO,DELDATE,DELSABUN,  ";
                            SQL = SQL + ComNum.VBLF + "       Gubun,P_Exam,P_Remark, ETCJIN)                                                ";
                            SQL = SQL + ComNum.VBLF + "SELECT RDATE,RTIME,PANO,SNAME,DEPTCODE,DRCODE,ENTDATE,ENTSABUN,GBINTERNET,   ";
                            SQL = SQL + ComNum.VBLF + "       GBDrug,GbChojin,GBCHK,GBFLAG,GBSPC,GWACHOJAE,GKIHO,SYSDATE,'" + clsType.User.Sabun + "', ";
                            SQL = SQL + ComNum.VBLF + "       Gubun,P_Exam,P_Remark, ETCJIN ";
                            SQL = SQL + ComNum.VBLF + " From OPD_TELRESV ";
                            SQL = SQL + ComNum.VBLF + "Where Rowid = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            SQL = "SELECT DEPTCODE FROM OPD_TELRESV WHERE ROWID = '" + strROWID + "' ";
                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                return rtnVal;
                            }
                            if (dt.Rows.Count > 0)
                            {
                                strDept = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                            }
                            dt.Dispose();
                            dt = null;


                            SQL = "DELETE OPD_TELRESV WHERE ROWID = '" + strROWID + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }

                        if (clsPublic.GstrSysDate == strRDate)
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " INSERT INTO OPD_MASTER_DEL (ACTDATE,PANO,DEPTCODE,BI,SNAME,SEX,AGE,JICODE,DRCODE,RESERVED,CHOJAE,";
                            SQL = SQL + ComNum.VBLF + " GBGAMEK,GBSPC,JIN,SINGU,BOHUN,CHANGE,SHEET,REP,PART,JTIME,STIME,FEE1,FEE2,FEE3,FEE31,";
                            SQL = SQL + ComNum.VBLF + " FEE5,FEE51,FEE7,AMT1,AMT2,AMT3,AMT4,AMT5,AMT6,AMT7,GELCODE,OCSJIN,BDATE,BUNUP,BONRATE,";
                            SQL = SQL + ComNum.VBLF + " TEAGBE,DELDATE,DELGB,DELSABUN,DELPART,CARDSEQNO,VCODE,ERPATIENT) ";
                            SQL = SQL + ComNum.VBLF + " SELECT ACTDATE,PANO,DEPTCODE,BI,SNAME,SEX,AGE,JICODE,DRCODE,RESERVED,CHOJAE,";
                            SQL = SQL + ComNum.VBLF + " GBGAMEK,GBSPC,JIN,SINGU,BOHUN,CHANGE,SHEET,REP,PART,JTIME,STIME,FEE1,FEE2,FEE3,FEE31,";
                            SQL = SQL + ComNum.VBLF + " FEE5,FEE51,FEE7,AMT1,AMT2,AMT3,AMT4,AMT5,AMT6,AMT7,GELCODE,OCSJIN,BDATE,BUNUP,BONRATE,";
                            SQL = SQL + ComNum.VBLF + " '',SYSDATE,'1','" + clsType.User.Sabun + "','','',VCODE,ERPATIENT ";
                            SQL = SQL + ComNum.VBLF + " FROM OPD_MASTER ";
                            SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE  = '" + strDept + "' ";
                            if (strGUBUN == "OCS예약")
                            {
                                SQL = SQL + ComNum.VBLF + "   AND RESERVED = '1' ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "   AND JIN  = 'E' ";
                            }
                            SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TRUNC(SYSDATE) ";


                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "DELETE OPD_MASTER WHERE PANO  = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE  = '" + strDept + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TRUNC(SYSDATE) ";
                            if (strGUBUN == "OCS예약")
                            {
                                SQL = SQL + ComNum.VBLF + "   AND RESERVED = '1' ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "   AND JIN  = 'E' ";
                            }
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }


                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE ADMIN.EMR_TREATT SET DELDATE = '" + strRDate.Replace("-", "") + "' ";
                            SQL = SQL + ComNum.VBLF + "  WHERE PATID = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "    AND INDATE = '" + strRDate + "' ";
                            SQL = SQL + ComNum.VBLF + "    AND CLINCODE = '" + strDept + "' ";
                            SQL = SQL + ComNum.VBLF + "    AND CLASS = 'O'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                        
                    }
                }
                
                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private Boolean Check_Order(string argPANO, string argDEPT, string argChkDate)
        {
            //해당 환자의 당일 처방/수납 유무에 따라 삭제 가능 여부 체크
            // return 값이 true 면 처방/수납이 존재하는 것임.   삭제 불가
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            
            Boolean bRtn = false;

            //체크일자와 오늘날짜와는 상관없이 삭제일의 수납여부를 무조건 읽도록 변경 ('2021-03-09 김현욱)
            //if (clsPublic.GstrSysDate != argChkDate)
            //{
            //    return false;
            //    //오늘과 다른 날짜의 접수를 취소하면 예외처리/
            //}

            SQL = "";//2019-01-17 오더는 제외
            //SQL = " SELECT PTNO FROM ADMIN.OCS_OORDER ";
            //SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPANO + "' ";
            //SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + argDEPT + "' ";
            //SQL = SQL + ComNum.VBLF + "   AND BDATE = TRUNC(SYSDATE) ";
            //SQL = SQL + ComNum.VBLF + " UNION ALL ";
            SQL = SQL + ComNum.VBLF + " SELECT PANO FROM ADMIN.OPD_SLIP ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPANO + "' ";
            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + argDEPT + "' ";
            //SQL = SQL + ComNum.VBLF + "   AND BDATE = TRUNC(SYSDATE) ";
            SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + argChkDate + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                bRtn = true;
            }
            if(dt.Rows.Count > 0)
            {
                bRtn = true;
            }

            dt.Dispose();
            dt = null;
            return bRtn;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인
            
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"바탕체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            // strHead1 = "/n/n/f1/C " + dtpDateFrom.Text+ " ~ " + dtpDate.Text + " 전화접수 " + (optJep0.Checked ? "" : "삭제 ") + "List" + " /n/n/n";

            if (rbGubun2.Checked == true)
            {
                strHead1 = "/n/n/f1/C " + dtpDate.Text + " 전화예약 " + (optJep0.Checked ? "" : "삭제 ") + "List" + " /n/n/n";
            }
            else if (rbGubun3.Checked == true)
            {
                strHead1 = "/n/n/f1/C " + dtpDate.Text + " OCS예약 " + (optJep0.Checked ? "" : "삭제 ") + "List" + " /n/n/n";
            }
            else
            {
                strHead1 = "/n/n/f1/C " + dtpDate.Text + " 예약 " + (optJep0.Checked ? "" : "삭제 ") + "List" + " /n/n/n";
            }
            //strHead2 = "/l/f2" + "조회기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + "~" + dtpTDate.Value.ToString("yyyy-MM-dd") + VB.Space(10) + "인쇄일자 : " + SysDate;

            ss1_Sheet1.Columns[0].Visible = false;
            //ss1_Sheet1.Columns[8, 9].Visible = optJep1.Checked;

            ss1_Sheet1.PrintInfo.Orientation = optJep0.Checked ? FarPoint.Win.Spread.PrintOrientation.Portrait : FarPoint.Win.Spread.PrintOrientation.Landscape;
            ss1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ss1_Sheet1.PrintInfo.Margin.Left = 15;
            ss1_Sheet1.PrintInfo.Margin.Right = 0;
            ss1_Sheet1.PrintInfo.Margin.Top = 25;
            ss1_Sheet1.PrintInfo.Margin.Bottom = 30;
            ss1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ss1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ss1_Sheet1.PrintInfo.ShowBorder = true;
            ss1_Sheet1.PrintInfo.ShowColor = false;
            ss1_Sheet1.PrintInfo.ShowGrid = true;
            ss1_Sheet1.PrintInfo.ShowShadows = false;
            ss1_Sheet1.PrintInfo.UseMax = false;
            ss1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ss1_Sheet1.PrintInfo.Preview = true;
            ss1.PrintSheet(0);

            ComFunc.Delay(100);

            Cursor.Current = Cursors.Default;

            ss1_Sheet1.Columns[0].Visible = true;
            //ss1_Sheet1.Columns[8, 9].Visible = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void optJep0_CheckedChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            btnSearchClick();
            ss1.Enabled = true;
            if (optJep0.Checked == true)
            {
                btnDelete.Enabled = true;
                ss1_Sheet1.Columns[9, 11].Visible = false;
            }
            else
            {
                ss1_Sheet1.Columns[9, 11].Visible = true;
            }
            btnCancel.Enabled = true;
        }

        private void ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ss1_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ss1, e.Column);
                return;
            }
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (VB.Left(clsType.User.BuseCode, 4) != "0442" && VB.Left(clsType.User.BuseCode, 4) != "0783" &&
            VB.Left(clsType.User.BuseCode, 4) != "0772" && VB.Left(clsType.User.BuseCode, 4) != "0774")
            {
                return;
            }

            string strPano = ss1_Sheet1.Cells[e.Row, 1].Text;
            string strName = ss1_Sheet1.Cells[e.Row, 2].Text;
            string strRDate = ss1_Sheet1.Cells[e.Row, 5].Text;
            string strRemark = ss1_Sheet1.Cells[e.Row, 11].Text;
            string RowId = ss1_Sheet1.Cells[e.Row, 12].Text;
            string strDate = dtpDate.Text;

            rGetPatientInfo(strPano, strName, strRDate, strRemark, RowId, strDate);
            this.Close();
        }

        private void ss1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (Convert.ToBoolean(ss1_Sheet1.Cells[e.Row, 0].Value) == true)
            {
                ss1_Sheet1.Rows[e.Row].ForeColor = Color.Red;
            }
            else
            {
                ss1_Sheet1.Rows[e.Row].ForeColor = Color.Black;
            }
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            txtName.ImeMode = ImeMode.Hangul;
        }

        private void optSort0_Click(object sender, EventArgs e)
        {
            rbGubun1.Checked = true;
            chkAllResv.Checked = true;
            txtPano.Text = "";
        }

        private void optSort1_Click(object sender, EventArgs e)
        {
            rbGubun1.Checked = true;
            chkAllResv.Checked = true;
            txtName.Text = "";
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (txtName.Text != "" || optSort0.Checked == true)
            {
                chkAllResv.Checked = true;
            }
        }

        private void txtPano_TextChanged(object sender, EventArgs e)
        {
            if (txtPano.Text != "" || optSort1.Checked == true)
            {
                chkAllResv.Checked = true;
            }
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VB.Left(cboDept.Text.Trim(), 2) == "**")
            {
                cboDoct.Items.Clear();
                cboDoct.Items.Add("****.전체");
            }
            else
            {
                clsVbfunc.SetDrCodeCombo(clsDB.DbCon, cboDoct, VB.Left(cboDept.Text.Trim(), 2), "", 1, "");
            }

            cboDoct.SelectedIndex = 0;
        }
    }
}
