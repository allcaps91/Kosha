using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>   
    /// File Name       : frmDispSetOpdNr.cs
    /// Description     : 대기순번 기타작업
    /// Author          : 유진호
    /// Create Date     : 2017-01-18
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\Frm기타대기순번작업
    /// </history>
    /// </summary>
    public partial class frmDispSetOpdNr : Form
    {
        ComFunc CF = new ComFunc();
        ContextMenu PopupMenu = null;

        private string GstrEmrViewDoct = "";
        private string FstrROWID = "";
        private string FstrROWID2 = "";
        private string FstrInfo = "";
        private string FstrDrCode = "";
        private int FnRow = 0;


        public frmDispSetOpdNr()
        {
            InitializeComponent();
        }

        private void frmDispSetOpdNr_Load(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            SET_CboDept();
        }

        private void SET_CboDept()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                cboDept.Items.Clear();
                cboDept.Items.Add("**");

                cboDept2.Items.Clear();
                cboDept2.Items.Add("**");

                if (GstrEmrViewDoct == "")
                {
                    cboDept.SelectedIndex = 0;
                    cboDept2.SelectedIndex = 0;
                    return;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PrintRanking,DeptCode ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode IN (";
                SQL = SQL + ComNum.VBLF + "   SELECT DrDept1 FROM BAS_DOCTOR WHERE DrCode IN (" + GstrEmrViewDoct + ") ";
                SQL = SQL + ComNum.VBLF + "    GROUP BY DrDept1) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                        cboDept2.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                    }
                }
                dt.Dispose();
                dt = null;
                cboDept.SelectedIndex = 0;
                cboDept2.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit2_Click(object sender, EventArgs e)
        {
            panPOPUP.Visible = false;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return;  //권한 확인

            if (btnSaveClick() == true)
            {
                btnSearchClick();
                READ_DIPS_LIST();
            }
        }

        private bool btnSaveClick()
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strROWID = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView2_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssView2_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strROWID = ssView2_Sheet1.Cells[i, 9].Text;
                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_PMPA.OPD_DEPTJEPSU ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                        }
                    }
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

        private void btnSave2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return;  //권한 확인
            if (btnSave2Click() == true)
            {
                //Call cmdView_Click                    
                panPOPUP.Visible = false;
            }
        }

        private bool btnSave2Click()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strPANO = "";
            string StrDrCode = "";
            string strDeptCode = "";
            string strSname = "";

            string strGuBun = "";
            string strGUBUN2 = "";
            string strRTime = "";
            string strSRTime = "";


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                strGUBUN2 = "00";
                if (optJob_0.Checked == true)
                {
                    strGuBun = "9";

                    if (VB.Right(FstrDrCode, 2) == "99")
                    {
                        if (VB.Left(cboDept2.Text, 2) == "**")
                        {
                            ComFunc.MsgBox("진료과를 선택하세요!!", "진료과 학인");
                            return rtVal;
                        }

                        if (VB.Left(cboDoct2.Text, 4) == "****")
                        {
                            ComFunc.MsgBox("진료의사를 선택하세요!!", "의사 학인");
                            return rtVal;
                        }
                    }
                }
                else if (optJob_3.Checked == true)
                {
                    strGuBun = "9";
                    strGUBUN2 = "01";

                    if (VB.Right(FstrDrCode, 2) == "99")
                    {
                        if (VB.Left(cboDept2.Text, 2) == "**")
                        {
                            ComFunc.MsgBox("진료과를 선택하세요!!", "진료과 학인");
                            return rtVal;
                        }

                        if (VB.Left(cboDoct2.Text, 4) == "****")
                        {
                            ComFunc.MsgBox("진료의사를 선택하세요!!", "의사 학인");
                            return rtVal;
                        }
                    }
                }
                else if (optJob_1.Checked == true)
                {
                    strGuBun = "8";

                    if (VB.Left(cboDept2.Text, 2) == "**")
                    {
                        ComFunc.MsgBox("진료과를 선택하세요!!", "진료과 학인");
                        return rtVal;
                    }

                    if (VB.Left(cboDoct2.Text, 4) == "****")
                    {

                        ComFunc.MsgBox("진료의사를 선택하세요!!", "의사 학인");
                        return rtVal;
                    }
                }
                else if (optJob_2.Checked == true)
                {
                    strGuBun = "7";
                }

                if (FnRow > 0)
                {
                    strPANO = ssView1_Sheet1.Cells[FnRow, 0].Text;
                    strSname = ssView1_Sheet1.Cells[FnRow, 1].Text;
                    strDeptCode = ssView1_Sheet1.Cells[FnRow, 2].Text;
                    StrDrCode = ssView1_Sheet1.Cells[FnRow, 7].Text;

                    if (strGuBun == "8" || VB.Right(FstrDrCode, 2) == "99")
                    {
                        strDeptCode = VB.Left(cboDept2.Text, 2);
                        StrDrCode = VB.Left(cboDoct2.Text, 4);
                    }
                    else if (strGuBun == "7")
                    {
                        strDeptCode = "RM";
                        StrDrCode = "2598";
                    }

                    strRTime = clsPublic.GstrSysTime;
                    strSRTime = txtSeqRTime.Text;



                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.OPD_DEPTJEPSU ( ActDate,DeptCode,DrCode,Pano,SName,JepTime,Gubun,GUBUN2,RTime,Chojae,DeptJTime,Seq_RTime) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "  TRUNC(SYSDATE),'" + strDeptCode + "','" + StrDrCode + "','" + strPANO + "','" + strSname + "',";
                    SQL = SQL + ComNum.VBLF + "  SYSDATE,'" + strGuBun + "','" + strGUBUN2 + "','" + strRTime + "','3',SYSDATE,'" + strSRTime + "' ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }

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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            btnSearchClick();
        }

        private void btnSearchClick()
        {
            panPOPUP.Visible = false;
            panInfo.Text = "";
            
            if (optJob_0.Checked == true)
            {
                READ_Consult();
            }
            else if (optJob_1.Checked == true)
            {
                READ_HEA();
            }
            else if (optJob_2.Checked == true)
            {
                READ_EMG_LIST();
            }
            else if (optJob_3.Checked == true)
            {
                READ_IPD_LIST();
            }

            READ_DIPS_LIST();
        }

        private void READ_Consult()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.PTNO, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, FRDEPTCODE, FRDRCODE, FRREMARK, TODEPTCODE, TODRCODE, TOREMARK, A.ROWID, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(SDATE, 'YYYY-MM-DD HH24:MI') SDATE,  TO_CHAR(EDATE, 'YYYY-MM-DD HH24:MI') EDATE ,  A.GBCONFIRM, A.GBDEL, ";
                SQL = SQL + ComNum.VBLF + "   B.SNAME, B.AGE, B.SEX, B.WARDCODE  ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_ITRANSFER A, KOSMOS_PMPA.IPD_NEW_MASTER B ";
                SQL = SQL + ComNum.VBLF + "    WHERE SDATE >= TO_DATE('" + dtpDate.Value.AddDays(-15) + "','YYYY-MM-DD') ";
                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + " AND TODEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' ";
                    if (VB.Left(cboDoct.Text, 4) != "****") SQL = SQL + ComNum.VBLF + " AND TODrCODE = '" + VB.Left(cboDoct.Text, 4) + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND TODEPTCODE IN (SELECT DrDept1 FROM KOSMOS_PMPA.BAS_DOCTOR WHERE DrCode IN (" + GstrEmrViewDoct + ") ) ";
                }

                if (chkCon.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND  GBCONFIRM IN ('*') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND ( GBCONFIRM IN (' ','','T') OR GBCONFIRM IS NULL ) ";
                }

                SQL = SQL + ComNum.VBLF + "  AND A.GbSend =' ' ";
                SQL = SQL + ComNum.VBLF + "  AND A.GBDEL <>'*' ";
                SQL = SQL + ComNum.VBLF + "  AND A.PTNO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "  AND B.ACTDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY  A.TODEPTCODE, A.TODRCODE";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PtNo"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["sName"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["TODEPTCODE"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 3].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["TODRCODE"].ToString().Trim());
                        ssView1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["WardCode"].ToString().Trim() + "병동";
                        ssView1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["TODRCODE"].ToString().Trim();


                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT PANO ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_DEPTJEPSU ";
                        SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE =TRUNC(SYSDATE)";
                        SQL = SQL + ComNum.VBLF + "   AND PANO ='" + dt.Rows[i]["PtNo"].ToString().Trim() + "' ";
                        if (dt.Rows[i]["TODEPTCODE"].ToString().Trim() == "EN")
                        {
                            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + dt.Rows[i]["TODEPTCODE"].ToString().Trim() + "' ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "   AND DRCODE ='" + dt.Rows[i]["TODRCODE"].ToString().Trim() + "' ";
                        }
                        SQL = SQL + ComNum.VBLF + "   AND GUBUN ='9' ";
                        SQL = SQL + ComNum.VBLF + "   AND (GUBUN IS NULL OR GUBUN ='00')  ";
                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt2.Rows.Count > 0)
                        {
                            ssView1_Sheet1.Cells[i, 0].BackColor = Color.FromArgb(255, 208, 255);
                        }
                        dt2.Dispose();
                        dt2 = null;
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void READ_HEA()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT '건진' Gubun,jepDate, PANO,PTNO,SNAME,WRTNO,'HR' DEPTCODE,'7101' DRCODE,SEX,AGE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.Hic_JEPSU  ";
                SQL = SQL + ComNum.VBLF + "    WHERE jepDATE = " + ComFunc.ConvOraToDate(dtpDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "  AND GBSTS NOT IN ('D','0') ";
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + "  SELECT '종검' Gubun, sDate,PANO,PTNO,SNAME,WRTNO,'TO' DEPTCODE,'7102' DRCODE,SEX,AGE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.HEA_JEPSU  ";
                SQL = SQL + ComNum.VBLF + "    WHERE SDATE = " + ComFunc.ConvOraToDate(dtpDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "  AND GBSTS NOT IN ('D','0') ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY 1, 2,5 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PtNo"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["sName"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 3].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        ssView1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DrCode"].ToString().Trim();


                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT PANO ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_DEPTJEPSU ";
                        SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE =TRUNC(SYSDATE)";
                        SQL = SQL + ComNum.VBLF + "   AND PANO ='" + dt.Rows[i]["PtNo"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GUBUN ='8' ";
                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt2.Rows.Count > 0)
                        {
                            ssView1_Sheet1.Cells[i, 0].BackColor = Color.FromArgb(255, 208, 255);
                        }
                        dt2.Dispose();
                        dt2 = null;

                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void READ_EMG_LIST()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "  SELECT '근전도' Gubun, Pano ,SName,DeptCode,DrCode,Age,Sex,IpdOpd ";
                SQL = SQL + ComNum.VBLF + "   FROM  KOSMOS_PMPA.XRAY_DETAIL  ";
                SQL = SQL + ComNum.VBLF + "  WHERE XJong = 'E' "; //'EMG
                SQL = SQL + ComNum.VBLF + "    AND SeekDate >= " + ComFunc.ConvOraToDate(dtpDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "    AND SeekDate < " + ComFunc.ConvOraToDate(dtpDate.Value.AddDays(1), "D");
                SQL = SQL + ComNum.VBLF + "  ORDER BY SName,Pano,XJong,XCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["sName"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 3].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        ssView1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DrCode"].ToString().Trim();


                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT PANO ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_DEPTJEPSU ";
                        SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE =TRUNC(SYSDATE)";
                        SQL = SQL + ComNum.VBLF + "   AND PANO ='" + dt.Rows[i]["PtNo"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GUBUN ='7' ";
                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt2.Rows.Count > 0)
                        {
                            ssView1_Sheet1.Cells[i, 0].BackColor = Color.FromArgb(255, 208, 255);
                        }
                        dt2.Dispose();
                        dt2 = null;
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void READ_IPD_LIST()
        {
            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                ssView1_Sheet1.RowCount = 0;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PaNO, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, DeptCode, DRCODE, ROWID, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(INDATE, 'YYYY-MM-DD HH24:MI') SDATE, SNAME, AGE, SEX, WARDCODE  ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER  ";
                SQL = SQL + ComNum.VBLF + "    WHERE ActDate IS NULL ";
                SQL = SQL + ComNum.VBLF + "     AND GbSts NOT IN ('7','9') ";
                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + " AND DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' ";
                    if (VB.Left(cboDoct.Text, 4) != "****") SQL = SQL + ComNum.VBLF + " AND DrCODE = '" + VB.Left(cboDoct.Text, 4) + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " AND DEPTCODE IN (SELECT DrDept1 FROM KOSMOS_PMPA.BAS_DOCTOR WHERE DrCode IN (" + GstrEmrViewDoct + ") ) ";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY  DEPTCODE, DRCODE, SNAME ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["sName"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 3].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        ssView1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["WardCode"].ToString().Trim() + "병동";
                        ssView1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DrCode"].ToString().Trim();


                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT PANO ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_DEPTJEPSU ";
                        SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE =TRUNC(SYSDATE)";
                        SQL = SQL + ComNum.VBLF + "   AND PANO ='" + dt.Rows[i]["PtNo"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND DRCODE ='" + dt.Rows[i]["DrCode"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND GUBUN ='9' ";
                        SQL = SQL + ComNum.VBLF + "   AND GUBUN2 ='01' ";
                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt2.Rows.Count > 0)
                        {
                            ssView1_Sheet1.Cells[i, 0].BackColor = Color.FromArgb(255, 208, 255);
                        }
                        dt2.Dispose();
                        dt2 = null;
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void READ_DIPS_LIST()
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                ssView2_Sheet1.RowCount = 0;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PANO,SNAME,DEPTCODE,DRCODE,GUBUN,SEQ_RTime,ROWID,Gb_Call,";
                SQL = SQL + ComNum.VBLF + "TO_CHAR(DeptJTime,'HH24:MI') DeptJTime ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_DEPTJEPSU ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE =TRUNC(SYSDATE) ";

                if (optJob_2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND GUBUN IN ('7') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND GUBUN IN ('8','9') ";
                }

                if (VB.Left(cboDept.Text, 2) != "**") SQL = SQL + ComNum.VBLF + " AND DeptCode = '" + VB.Left(cboDept.Text, 2) + "' ";
                if (VB.Left(cboDoct.Text, 4) != "****") SQL = SQL + ComNum.VBLF + " AND DrCode = '" + VB.Left(cboDoct.Text, 4) + "' ";

                SQL = SQL + ComNum.VBLF + " ORDER BY DeptJTime, SEQ_RTime ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.RowCount = dt.Rows.Count;
                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView2_Sheet1.Cells[i, 1].Text = "";
                        ssView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["sName"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        if (dt.Rows[i]["DeptCode"].ToString().Trim() == "2598")
                        {
                            ssView2_Sheet1.Cells[i, 4].Text = "근전도";
                        }
                        else
                        {
                            ssView2_Sheet1.Cells[i, 4].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        }

                        ssView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DeptJTime"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["SEQ_RTime"].ToString().Trim();

                        ssView2_Sheet1.Cells[i, 7].Text = "";
                        if (dt.Rows[i]["Gb_Call"].ToString().Trim() == "Y")
                        {
                            ssView2_Sheet1.Cells[i, 7].Text = "부재중";
                        }

                        ssView2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strDoct = "";
            try
            {
                cboDoct.Items.Clear();
                cboDoct.Items.Add("****.전체");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DrName,DrCode FROM BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DrCode IN (" + GstrEmrViewDoct + ") ";
                if (cboDept.Text != "**")
                {
                    SQL = SQL + ComNum.VBLF + "  AND DrDept1='" + VB.Trim(cboDept.Text) + "' ";
                }

                switch (clsType.User.Sabun)
                {
                    case "28182":
                    case "16225":
                        SQL = SQL + ComNum.VBLF + "ORDER BY DRCODE ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "ORDER BY DrName ";
                        break;
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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDoct = dt.Rows[i]["DrCode"].ToString().Trim() + ".";
                        strDoct = strDoct + dt.Rows[i]["DrName"].ToString().Trim();
                        cboDoct.Items.Add(strDoct);
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboDept2_SelectedIndexChanged(object sender, EventArgs e)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strDoct = "";
            try
            {
                cboDoct2.Items.Clear();
                cboDoct2.Items.Add("****.전체");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DrName,DrCode FROM BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DrCode IN (" + GstrEmrViewDoct + ") ";
                if (cboDept2.Text != "**")
                {
                    SQL = SQL + ComNum.VBLF + "  AND DrDept1='" + VB.Trim(cboDept2.Text) + "' ";
                }

                switch (clsType.User.Sabun)
                {
                    case "28182":
                    case "16225":
                        SQL = SQL + ComNum.VBLF + "ORDER BY DRCODE ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "ORDER BY DrName ";
                        break;
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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDoct = dt.Rows[i]["DrCode"].ToString().Trim() + ".";
                        strDoct = strDoct + dt.Rows[i]["DrName"].ToString().Trim();
                        cboDoct2.Items.Add(strDoct);
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPANO = "";
            string strSname = "";

            FnRow = e.Row;
            panInfo.Text = "";

            if (e.ColumnHeader == true || e.RowHeader == true) return;

            ComFunc.ReadSysDate(clsDB.DbCon);

            strPANO = ssView1_Sheet1.Cells[e.Row, 0].Text;
            strSname = ssView1_Sheet1.Cells[e.Row, 1].Text;
            FstrDrCode = ssView1_Sheet1.Cells[e.Row, 7].Text;

            if (ComFunc.MsgBoxQ(strSname + "(" + strPANO + ")님을 진료대기로 올리겠습니까??", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                panPOPUP.Visible = false;
                return;
            }


            txtSeqRTime.Text = clsPublic.GstrSysTime;

            //'이비인후과
            if (VB.Left(FstrDrCode, 2) == "42")
            {
                cboDept2.Text = "EN";
                cboDept_SelectedIndexChanged(null, null);
            }

            panPOPUP.Visible = true;

            if (optJob_0.Checked == true)
            {
                pan_Doct.Visible = false;
                if (VB.Right(FstrDrCode, 2) == "99")
                {
                    pan_Doct.Visible = true;
                }
            }
            else if (optJob_1.Checked == true)
            {
                pan_Doct.Visible = true;
            }
            else if (optJob_2.Checked == true)
            {
                pan_Doct.Visible = false;
            }

            panInfo.Text = strSname + "(" + strPANO + ")";
        }

        private void ssView2_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string strROWID = "";
            string strSRTime = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (e.Row >= 0 && e.Column == 6)
            {
                strSRTime = ssView2_Sheet1.Cells[e.Row, 6].Text;
                strROWID = ssView2_Sheet1.Cells[e.Row, 9].Text;

                if (strROWID == "") return;
                if (VB.Len(strSRTime) != 5) return;

                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_PMPA.OPD_DEPTJEPSU SET ";
                    SQL = SQL + ComNum.VBLF + " SEQ_RTime ='" + strSRTime + "' ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(ex.Message);
                    Cursor.Current = Cursors.Default;
                    return;
                }

            }
        }

        private void ssView2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            FstrROWID2 = "";

            // 마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                PopupMenu = new ContextMenu();
                ssView2.ContextMenu = null;

                FstrROWID2 = ssView2_Sheet1.Cells[e.Row, 9].Text;
                FstrInfo = "▶ " + ssView2_Sheet1.Cells[e.Row, 1].Text + " ◀";
                FstrInfo = FstrInfo + " (" + ssView2_Sheet1.Cells[e.Row, 2].Text + ")";

                PopupMenu.Name = "ssView2";
                PopupMenu.MenuItems.Add(FstrInfo);
                PopupMenu.MenuItems.Add("정보", new System.EventHandler(mnuSet_Click));
                PopupMenu.MenuItems.Add("부재중표시", new System.EventHandler(mnuSet_Click));
                PopupMenu.MenuItems.Add("부재중해지", new System.EventHandler(mnuSet_Click));

                PopupMenu.MenuItems[0].Name = "PopupMenu_0";
                PopupMenu.MenuItems[1].Name = "PopupMenu_1";
                PopupMenu.MenuItems[2].Name = "PopupMenu_2";

                ssView2.ContextMenu = PopupMenu;
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


            if (strPopMenuName == "ssList")
            {
                if (strSelectMenuName == "PopupMenu_1")
                {
                    if (updatePopMenu1() == true)
                    {
                        READ_DIPS_LIST();
                    }
                }
                if (strSelectMenuName == "PopupMenu_2")
                {
                    if (updatePopMenu2() == true)
                    {
                        READ_DIPS_LIST();
                    }
                }
            }
        }

        private bool updatePopMenu1()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (FstrROWID2 == "") return rtVal;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인
                SQL = " UPDATE KOSMOS_PMPA.OPD_DEPTJEPSU SET ";
                SQL = SQL + ComNum.VBLF + " Gb_Call ='Y' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID2 + "' ";

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

        private bool updatePopMenu2()
        {
            bool rtVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인
                SQL = " UPDATE KOSMOS_PMPA.OPD_DEPTJEPSU SET ";
                SQL = SQL + ComNum.VBLF + " Gb_Call ='' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID2 + "' ";

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
    }
}
