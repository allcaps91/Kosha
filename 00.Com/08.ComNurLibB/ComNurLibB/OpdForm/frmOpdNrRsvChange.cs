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
using ComLibB;

namespace ComNurLibB
{
    public partial class frmOpdNrRsvChange : Form
    {
        private string strPtno = "";
        private string FstrDept = "";
        private string FstrDrCode = "";
        private string FstrDate = "";
        private string FstrROWID = "";
        private string FstrRowid_opd = "";
        private string FstrSPC = "";
        private string FstrChangeBeforeDate = "";

        public frmOpdNrRsvChange()
        {
            InitializeComponent();
        }

        public frmOpdNrRsvChange(string strPtno)
        {
            InitializeComponent();
            this.strPtno = strPtno;
        }

        private void frmOpdNrRsvChange_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtPano.Text = strPtno;
            getRsvData();
        }

        private void getRsvData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            txtPano.Text = VB.Format(txtPano.Text, "00000000");
            if (VB.Trim(txtPano.Text) == "") return;

            ssView1_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.DEPTCODE, A.SNAME, B.DRNAME,c.GbSPC,a.CHOJAE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.DATE3,'YYYY-MM-DD') DATE3, TO_CHAR(A.DATE3,'HH24:MI') TIME3, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.DATE1,'YYYY-MM-DD') DATE1, A.DRCODE, A.ROWID, C.ROWID OPDROWID ";
                SQL = SQL + ComNum.VBLF + " FROM OPD_RESERVED_NEW A, BAS_DOCTOR B, OPD_MASTER C ";
                SQL = SQL + ComNum.VBLF + " WHERE A.DRCODE = B.DRCODE ";
                SQL = SQL + ComNum.VBLF + "   AND A.PANO  = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.DATE3 >= " + ComFunc.ConvOraToDate(Convert.ToDateTime(clsPublic.GstrSysDate), "D");
                SQL = SQL + ComNum.VBLF + "   AND A.DATE3 <= " + ComFunc.ConvOraToDate(Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(1), "D");
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = C.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND C.ACTDATE = TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE = C.DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "   AND C.RESERVED  = '1' ";
                SQL = SQL + ComNum.VBLF + "   AND TRUNC(A.TRANSDATE)  =TRUNC(SYSDATE) ";  //'당일건만
                SQL = SQL + ComNum.VBLF + "   AND a.RetDate IS NULL ";  //'2011-03-09
                SQL = SQL + ComNum.VBLF + " ORDER BY DEPTCODE ";

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
                    ComFunc.MsgBox("예약이 없습니다. 다시 확인하세요.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    txtSname.Text = dt.Rows[0]["SNAME"].ToString().Trim();

                    for (i = 0; i < dt.Rows.Count; i++)
                    {

                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DATE3"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["TIME3"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DATE1"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["OPDROWID"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["GbSPC"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["CHOJAE"].ToString().Trim();
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
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strDate = "";
            string strTime = "";
            string strChojae = "";
            
            try
            {
                strDate = ssView1_Sheet1.Cells[e.Row, 0].Text;
                strTime = ssView1_Sheet1.Cells[e.Row, 1].Text;
                FstrDept = ssView1_Sheet1.Cells[e.Row, 2].Text;
                FstrDrCode = ssView1_Sheet1.Cells[e.Row, 4].Text;
                FstrDate = ssView1_Sheet1.Cells[e.Row, 5].Text;
                FstrROWID = ssView1_Sheet1.Cells[e.Row, 6].Text;
                FstrRowid_opd = ssView1_Sheet1.Cells[e.Row, 7].Text;
                FstrSPC = ssView1_Sheet1.Cells[e.Row, 8].Text;
                strChojae = ssView1_Sheet1.Cells[e.Row, 9].Text;


                if (strChojae == "5" || strChojae == "6")
                {
                    ComFunc.MsgBox("예약당시 공휴예약대상환자 입니다. 예약변경이 할 수 없슴. 원무과로 문의");
                    return;
                }

                FstrChangeBeforeDate = strDate + " " + strTime;
                

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DRCODE FROM BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DRDEPT1 = '" + FstrDept + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TOUR = 'N' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY DRCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                cboDr.Items.Clear();
                if (dt.Rows.Count > 0)
                {                    
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDr.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim());
                        if (FstrDrCode == dt.Rows[i]["DRCODE"].ToString().Trim())
                        {
                            cboDr.SelectedIndex = i;
                        }
                    }
                }                
                dt.Dispose();
                dt = null;

                dtpDate.Value = Convert.ToDateTime(strDate);
                dtpTime.Text = strTime;
                cboDr_SelectedIndexChanged(null, null);
                cboDr.Focus();
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

        private void cboDr_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {                
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DRNAME FROM BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DRCODE  = '" + cboDr.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }                
                if (dt.Rows.Count > 0)
                {    
                    txtDrName.Text = dt.Rows[i]["DRNAME"].ToString().Trim();                    
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

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                getRsvData();
            }            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtPano.Text = "";
            cboDr.Items.Clear();            
            dtpDate.Text = clsPublic.GstrSysDate;
            dtpTime.Text = clsPublic.GstrSysTime;

            txtPano_R.Text = "";
            txtPano.Focus();


            ssSub1_Sheet1.Cells[0, 1].Text = "";
            ssSub1_Sheet1.Cells[1, 1].Text = "";            
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {                
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT GBJIN,GBJIN2 FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
                SQL = SQL + ComNum.VBLF + " WHERE SCHDATE = " + ComFunc.ConvOraToDate(dtpDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND DRCODE ='" + VB.Trim(cboDr.Text) + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssSub1_Sheet1.Cells[0, 1].Text = SCH_OPD_GUBUN(dt.Rows[0]["GBJIN"].ToString().Trim());
                    ssSub1_Sheet1.Cells[1, 1].Text = SCH_OPD_GUBUN(dt.Rows[0]["GBJIN"].ToString().Trim());
                }
                else
                {
                    ssSub1_Sheet1.Cells[0, 1].Text = "";
                    ssSub1_Sheet1.Cells[1, 1].Text = "";
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

        private string SCH_OPD_GUBUN(string argBun)
        {
            string rtnVal = "";

            switch (argBun)
            {
                case "1":
                    rtnVal = "진료";
                    break;
                case "2":
                    rtnVal = "수술";
                    break;
                case "3":
                    rtnVal = "특수검사";
                    break;
                case "4":
                    rtnVal = "진료않함";
                    break;
                case "5":
                    rtnVal = "학회";
                    break;
                case "6":
                    rtnVal = "휴가";
                    break;
                case "7":
                    rtnVal = "출장";
                    break;
                case "9":
                    rtnVal = "off(주40시간)";
                    break;
            }

            return rtnVal;
        }
        
        private void btnSearch_R_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            txtPano_R.Text = VB.Format(txtPano_R.Text, "00000000");
            if (VB.Trim(txtPano_R.Text) == "") return;

            ssView99_Sheet1.RowCount = 0;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";                
                SQL = SQL + ComNum.VBLF + "SELECT SName,Pano,DeptCode,DrCode,TO_CHAR(Date3,'YYYY-MM-DD') RDate,TO_CHAR(Date3,'HH24:MI') RTime ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + txtPano_R.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Date3 >= TO_DATE('" + clsPublic. GstrSysDate + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "   AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND RETDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Date3,DeptCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    ssView99_Sheet1.RowCount = dt.Rows.Count;
                    ssView99_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView99_Sheet1.Cells[i, 0].Text = dt.Rows[i]["RDate"].ToString().Trim();
                        ssView99_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RTime"].ToString().Trim();
                        ssView99_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssView99_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView99_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DrCode"].ToString().Trim();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            if (btnSaveClick() == true)
            {
                btnCancel.PerformClick();
            }
        }

        private bool btnSaveClick()
        {
            bool rtVal = false;
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string StrRDate   = "";
            string strRTime   = "";
            string StrDrCode  = "";
            string strPano    = "";
            string strRemark = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            if (string.Compare(clsPublic.GstrSysTime, "17:30") >= 0)
            {
                ComFunc.MsgBox("오후5시 이후부터는 작업 불가합니다..!!");
                return rtVal;
            }
            
            if (FstrChangeBeforeDate == "")
            {
                ComFunc.MsgBox("대상이 선택안되었습니다.. 다시 확인후 작업하세요..");
                return rtVal;
            }

            if (Convert.ToDateTime(clsPublic.GstrSysDate).DayOfWeek == DayOfWeek.Saturday)
            {
                if (VB.UCase(VB.Trim(FstrDept)) == "FM")
                {
                }
                else
                {
                    if (string.Compare(clsPublic.GstrSysTime, "12:30") >= 0)
                    {
                        ComFunc.MsgBox("예약부도자 예약변경 토요일은 오전 11시 50분까지 가능합니다..");
                        return rtVal;
                    }
                }
            }
            else
            {
                if (string.Compare(clsPublic.GstrSysTime, "17:30") > 0)
                {
                    ComFunc.MsgBox("예약부도자 예약변경은 오후 5시 10분까지 가능합니다.." + ComNum.VBLF + "시간 이후에 변경작업은 원무과에 연락하십시오");
                    return rtVal;
                }
            }

            if (VB.Left(VB.Trim(txtPano.Text), 1) == "9")
            {
                ComFunc.MsgBox("접수할수 없는 등록번호(9로 시작하는것)입니다..");
                return rtVal;
            }
            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'대체공휴일 체크
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_JOB";
                SQL = SQL + ComNum.VBLF + "  WHERE JobDate = " + ComFunc.ConvOraToDate(dtpDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "    AND tempholyday  = '*' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("대체공휴일입니다.. 예약변경불가!!. 원무과 문의하세요!!(예약비차액발생)");
                    return rtVal;
                }


                //'예약부도 변경시 - 선택진료 제한
                if (Convert.ToDateTime(FstrDate) >= Convert.ToDateTime("2011-06-01") && FstrSPC == "1")
                {
                    if (CHECK_CHOICE_TREAT_SAT(clsDB.DbCon, FstrDept, dtpDate.Value.ToShortDateString()) != "OK")
                    {
                        ComFunc.MsgBox("예약변경시 토요일 선택진료 가능과가 아닙니다..");
                        return rtVal;
                    }
                }

                //'DRG예약자체크 -  DRG 2016-06-08
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT pano ";
                SQL = SQL + ComNum.VBLF + " FROM IPD_reserved";
                SQL = SQL + ComNum.VBLF + "  WHERE ReDate = " + ComFunc.ConvOraToDate(dtpDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND Pano ='" + VB.Trim(txtPano.Text) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (GBCHK IS NULL OR GBCHK <> '1' ) ";
                SQL = SQL + ComNum.VBLF + "   AND GbDRG ='Y' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("당일DRG예약자 입니다.당일 접수자로 변경 못함.");
                    return rtVal;
                }
                
                //'2013-11-27
                if (VB.UCase(VB.Trim(FstrDept)) == "FM")
                {
                    ComFunc.MsgBox("가정의학과 접수는 가정의학과에서만 가능합니다..!!");
                    return rtVal;
                }
                
                StrRDate = dtpDate.Value.ToShortDateString();
                strRTime = VB.Trim(dtpTime.Text);
                
                if (VB.Mid(StrRDate, 5, 1) != "-" && VB.Mid(StrRDate, 8, 1) != "1")
                {
                    ComFunc.MsgBox("변경일자를 확인하세요.");
                    return rtVal;
                }
                
                if (dtpDate.Value <= Convert.ToDateTime(clsPublic.GstrSysDate))
                {
                    ComFunc.MsgBox("변경일자는 오늘이후날짜만 가능합니다...");
                    return rtVal;
                }
                
                if (VB.Mid(strRTime, 3, 1) != ":")
                {
                    ComFunc.MsgBox("변경시간를 확인하세요.");
                    return rtVal;
                }


                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode = '" + VB.Trim(FstrDept) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Date3 >= " + ComFunc.ConvOraToDate(dtpDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND Date3 < " + ComFunc.ConvOraToDate(dtpDate.Value.AddDays(1), "D");
                SQL = SQL + ComNum.VBLF + "   AND TRANSDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND RETDATE IS NULL ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("변경일자에 예약 되어 있습니다. 다른 일자로 변경하세요.");
                    return rtVal;
                }
                
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SUM(NAL*QTY) NAL FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode = '" + VB.Trim(FstrDept) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = " + ComFunc.ConvOraToDate(Convert.ToDateTime(clsPublic.GstrSysDate), "D");
                SQL = SQL + ComNum.VBLF + "   AND BDATE = " + ComFunc.ConvOraToDate(Convert.ToDateTime(clsPublic.GstrSysDate), "D");
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("수납한 내역이 있습니다 예약 변경 작업을 할 수 없습니다");
                    return rtVal;
                }
                
                //'일일수술센터 예약 변경 못함.
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PANO FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND INDATE >= " + ComFunc.ConvOraToDate(Convert.ToDateTime(clsPublic.GstrSysDate), "D");
                SQL = SQL + ComNum.VBLF + "   AND INDATE < " + ComFunc.ConvOraToDate(Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(1), "D");
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + VB.Trim(FstrDept) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GBSTS <> '9' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("입원환자는 예약변경 작업을 할 수 없습니다..!!!");
                    return rtVal;
                }


                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "OPD_MASTER_DEL(ACTDATE, PANO, DEPTCODE, BI, SNAME, SEX, AGE, JICODE, DRCODE,               ";
                SQL = SQL + ComNum.VBLF + " RESERVED, CHOJAE, GBGAMEK, GBSPC, JIN,MksJin, SINGU, BOHUN, CHANGE, SHEET, REP, PART, JTIME,                 " ;
                SQL = SQL + ComNum.VBLF + " STIME, FEE1, FEE2, FEE3, FEE31, FEE5, FEE51, FEE7, AMT1, AMT2, AMT3, AMT4, AMT5, AMT6,                " ;
                SQL = SQL + ComNum.VBLF + " AMT7, GELCODE, OCSJIN, BDATE, BUNUP, BONRATE, TEAGBE, DELDATE, DELGB, DELSABUN, DELPART,PNEUMONIA,MCode,LastDanAmt,JinTime)  " ;
                SQL = SQL + ComNum.VBLF + " SELECT ACTDATE, PANO, DEPTCODE, BI, SNAME, SEX, AGE, JICODE, DRCODE, RESERVED, CHOJAE, GBGAMEK,       " ;
                SQL = SQL + ComNum.VBLF + " GBSPC, JIN,MksJin, SINGU, BOHUN, CHANGE, SHEET, REP, PART, JTIME, STIME, FEE1, FEE2, FEE3, FEE31, FEE5,      " ;
                SQL = SQL + ComNum.VBLF + " FEE51, FEE7, AMT1, AMT2, AMT3, AMT4, AMT5, AMT6, AMT7, GELCODE, OCSJIN, BDATE, BUNUP, BONRATE, TEAGBE," ;
                SQL = SQL + ComNum.VBLF + "  SYSDATE, '2',  '" + clsType.User.Sabun + "','" + clsType.User.JobPart + "', PNEUMONIA,MCode,LastDanAmt,JinTime                " ;
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                                                       ";
                SQL = SQL + ComNum.VBLF + "WHERE ActDate = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')                                            " ;
                SQL = SQL + ComNum.VBLF + "  AND Pano = '" + txtPano.Text + "'                                                                    " ;
                SQL = SQL + ComNum.VBLF + "  AND DeptCode = '" + VB.Trim(FstrDept) + "'                                                              " ;
                SQL = SQL + ComNum.VBLF + "  AND BDate = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')                                                " ;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }



                //'접수삭제함.
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "   AND ROWID = '" + FstrRowid_opd + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }



                //'진료 대기순서 삭제
                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "OPD_DEPTJEPSU ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "   AND PANO  = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE  = '" + FstrDrCode + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }


                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVED_NEW SET ";
                SQL = SQL + ComNum.VBLF + "  TRANSDATE = '', TRANSAMT = 0,  ";
                SQL = SQL + ComNum.VBLF + "  DATE3 = TO_DATE('" + StrRDate + " " + strRTime + "','YYYY-MM-DD HH24:MI'), ";
                SQL = SQL + ComNum.VBLF + "  PRTSEQNO  = '0' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID  = '" + FstrROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }


                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "OPD_RESERVEDBACKUP ";
                SQL = SQL + ComNum.VBLF + "    SET DATE3 = TO_DATE('" + StrRDate + " " + strRTime + "', 'YYYY-MM-DD HH24:MI')  ";
                SQL = SQL + ComNum.VBLF + "  WHERE Pano     = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DeptCode = '" + FstrDept + "' ";
                SQL = SQL + ComNum.VBLF + "    AND Date1    = TO_DATE('" + FstrDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND DRCODE   = '" + FstrDrCode + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }


                //'TextEmr 자동 EMR Table INSERT                
                clsConAcpInfo.NEW_TextEMR_TreatInterface(VB.Trim(txtPano.Text), clsPublic.GstrSysDate, VB.Trim(FstrDept), "외래", "취소", FstrDrCode, clsDB.DbCon);


                //'OPD_SUNAP 추가함
                strRemark = "외래예약변경 " + FstrChangeBeforeDate + " -> " + StrRDate + " " + strRTime;
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP (ACTDATE, PANO, AMT, PART, SEQNO,STIME, BIGO, REMARK,DEPTCODE,BI,DELDATE) VALUES (";
                SQL = SQL + ComNum.VBLF + " TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), '" + txtPano.Text + "', ";
                SQL = SQL + ComNum.VBLF + " 0 , '" + clsType.User.Sabun + "',0, '" + clsPublic.GstrSysDate + "','" + FstrDate + "', ";
                SQL = SQL + ComNum.VBLF + " '" + strRemark + "' , '" + FstrDept + "','XX',TRUNC(SYSDATE) ) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                FstrRowid_opd = "";
                FstrROWID = "";
                FstrDrCode = "";
                FstrDept = "";                
                FstrDate = "";
                FstrChangeBeforeDate = "";
                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
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
        
        public string CHECK_CHOICE_TREAT_SAT(PsmhDb pDbCon, string ArgDept, string ArgDate)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            if (clsVbfunc.GetYoIl(ArgDate) != "토요일")
            {
                return rtnVal;
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Code ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN         = '선택진료_외래토요일' ";
            SQL += ComNum.VBLF + "    AND TRIM(Code)    = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND JDate         <= TO_DATE('" + ArgDate + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='' ) ";
            SQL += ComNum.VBLF + "  ORDER BY JDate DESC ";
            SqlErr = clsDB.GetDataTable(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }


            if (DtPf.Rows.Count > 0)
                rtnVal = "OK";

            DtPf.Dispose();
            DtPf = null;

            return rtnVal;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
