using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase;

namespace ComLibB
{
    public partial class frmOcsCpPopUpMsg : Form
    {
        //모니터 사이즈, 폼 위치
        private int mintTop = 0;
        private int mintLeft = 0;
        private int mintMonitor = 0;
        private int[] mintWidth = null;
        private int[] mintHeight = null;

        string mstrCpDept = "";

        /// <summary>
        /// 모니터
        /// </summary>
        private void GetMonitorInfo()
        {
            Screen[] screens = Screen.AllScreens;

            mintMonitor = screens.Length;
            mintWidth = new int[mintMonitor];
            mintHeight = new int[mintMonitor];

            int i = 0;
            foreach (Screen screen in screens)
            {
                mintWidth[i] = screen.Bounds.Width;
                mintHeight[i] = screen.Bounds.Height;
                i = i + 1;
            }
        }

        public frmOcsCpPopUpMsg()
        {
            InitializeComponent();
        }

        public frmOcsCpPopUpMsg(double pCPNO)
        {
            InitializeComponent();
        }

        private void frmOcsCpPopUpMsg_Load(object sender, EventArgs e)
        {
            ClearForm();
            CenterPanel();
            GetSetCpPopUp();
        }

        private void CenterPanel()
        {
            try
            {
                if (this.VerticalScroll.Value != 0)
                {
                    this.VerticalScroll.Value = 0;
                }
                panCP.Left = this.Width / 2 - panCP.Width / 2;
                panCP.Top = this.Height / 2 - panCP.Height / 2;
            }
            catch
            {

            }
        }

        private void frmOcsCpPopUpMsg_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.VerticalScroll.Value != 0)
                {
                    this.VerticalScroll.Value = 0;
                }
                panCP.Left = this.Width / 2 - panCP.Width / 2;
                panCP.Top = this.Height / 2 - panCP.Height / 2;
            }
            catch
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lblCP.ForeColor == Color.Red)
            {
                lblCP.ForeColor = Color.Blue;
            }
            else
            {
                lblCP.ForeColor = Color.Red;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            this.Close();
        }

        bool SaveData()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO  KOSMOS_OCS.OCS_CP_COMP";
                SQL = SQL + ComNum.VBLF + "(";
                SQL = SQL + ComNum.VBLF + " CPNO, COMIP, IDNUMBER, COMPDATE, COMPTIME";
                SQL = SQL + ComNum.VBLF + ")";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "     R.CPNO, ";    //CPNO
                SQL = SQL + ComNum.VBLF + "     '" + clsCompuInfo.gstrCOMIP + "', ";    //COMIP
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "', ";    //IDNUMBER
                SQL = SQL + ComNum.VBLF + "     '" + VB.Left(strCurDateTime, 8) + "', ";    //COMPDATE
                SQL = SQL + ComNum.VBLF + "     '" + VB.Right(strCurDateTime, 6) + "' ";    //COMPTIME
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_CP_RECORD R";
                SQL = SQL + ComNum.VBLF + "WHERE NOT EXISTS (SELECT 1 FROM KOSMOS_OCS.OCS_CP_COMP C";
                SQL = SQL + ComNum.VBLF + "                     WHERE  C.CPNO = R.CPNO ";
                SQL = SQL + ComNum.VBLF + "                         AND  C.COMIP = '" + clsCompuInfo.gstrCOMIP + "')";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("해당환자를 확인 바랍니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void ClearForm()
        {
            lblCpName0.Text = "";
            lblPtno0.Text = "";
            lblPtname0.Text = "";
            lblSex0.Text = "";
            lblAge0.Text = "";
            lblDrugName0.Text = "";

            lblCpName1.Text = "";
            lblPtno1.Text = "";
            lblPtname1.Text = "";
            lblSex1.Text = "";
            lblAge1.Text = "";
            lblDrugName1.Text = "";

            panInfo0.Visible = false;
            panInfo1.Visible = false;
        }
        
        private void GetSetCpPopUp()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            PsmhDb DbCon2 = null;  //기본 연결 객체
            DbCon2 = clsDB.DBConnect();

            try
            {
                mstrCpDept = "";

                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    VALUEV, VALUEN ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_PCCONFIG ";
                SQL = SQL + ComNum.VBLF + "WHERE IPADDRESS = '" + clsCompuInfo.gstrCOMIP + "' ";
                SQL = SQL + ComNum.VBLF + "    AND GUBUN = '기타PC설정' ";
                SQL = SQL + ComNum.VBLF + "    AND CODE = 'CP 메세지 팦업' ";
                SQL = SQL + ComNum.VBLF + "    AND VALUEN = '1' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, DbCon2);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, DbCon2); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    mstrCpDept = dt.Rows[0]["VALUEV"].ToString().Trim();
                }
                
                dt.Dispose();
                dt = null;

                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    R.CPNO, R.PTNO, R.PTNAME, R.GBIO,  ";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(R.BDATE, 'YYYY-MM-DD') AS BDATE ,  ";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(N.OUTTIME, 'YYYY-MM-DD HH24:MI') AS OUTTIME , ";
                SQL = SQL + ComNum.VBLF + "    N.SEX, N.AGE, N.DRNAME, N.HODEPT1, N.HODRNAME1,  ";
                SQL = SQL + ComNum.VBLF + "    R.DEPTCODE, R.BI, R.CPCODE, R.AGE, R.SEX,  ";
                SQL = SQL + ComNum.VBLF + "    R.STARTSABUN, U.USERNAME,  ";
                SQL = SQL + ComNum.VBLF + "    B.BASNAME  ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_CP_RECORD R  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.NUR_ER_PATIENT N  ";
                SQL = SQL + ComNum.VBLF + "     ON R.PTNO = N.PANO  ";
                SQL = SQL + ComNum.VBLF + "     AND R.BDATE = N.JDATE  ";
                SQL = SQL + ComNum.VBLF + "     AND R.INTIME = N.INTIME  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_BASCD B  ";
                SQL = SQL + ComNum.VBLF + "    ON R.CPCODE = B.BASCD  ";
                SQL = SQL + ComNum.VBLF + "    AND B.GRPCDB = 'CP관리'  ";
                SQL = SQL + ComNum.VBLF + "    AND B.GRPCD = 'CP코드관리'  ";
                SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_USER U ";
                SQL = SQL + ComNum.VBLF + "    ON R.STARTSABUN = U.IDNUMBER  ";
                SQL = SQL + ComNum.VBLF + "WHERE NOT EXISTS (SELECT 1 FROM KOSMOS_OCS.OCS_CP_COMP C ";
                SQL = SQL + ComNum.VBLF + "                     WHERE  C.CPNO = R.CPNO  ";
                SQL = SQL + ComNum.VBLF + "                         AND  C.COMIP = '" + clsCompuInfo.gstrCOMIP + "')";
                if (mstrCpDept == "PM") //약국은 두가지만 전달
                {
                    SQL = SQL + ComNum.VBLF + "     AND R.CPCODE IN ('CPCODE0001','CPCODE0002')";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, DbCon2);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    clsDB.DisDBConnect(DbCon2);
                    DbCon2 = null;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0 )
                        {
                            lblCpName0.Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                            lblPtno0.Text = dt.Rows[i]["PTNO"].ToString().Trim();
                            lblPtname0.Text = dt.Rows[i]["PTNAME"].ToString().Trim();
                            lblSex0.Text = dt.Rows[i]["SEX"].ToString().Trim();
                            lblAge0.Text = dt.Rows[i]["AGE"].ToString().Trim();
                            if (dt.Rows[i]["CPCODE"].ToString().Trim() == "CPCODE0001")
                            {
                                lblDrugName0.Text = "";
                            }
                            else if (dt.Rows[i]["CPCODE"].ToString().Trim() == "CPCODE0002")
                            {
                                lblDrugName0.Text = "";
                            }
                            
                            panInfo0.Visible = true;
                        }
                        else if (i == 1)
                        {
                            lblCpName1.Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                            lblPtno1.Text = dt.Rows[i]["PTNO"].ToString().Trim();
                            lblPtname1.Text = dt.Rows[i]["PTNAME"].ToString().Trim();
                            lblSex1.Text = dt.Rows[i]["SEX"].ToString().Trim();
                            lblAge1.Text = dt.Rows[i]["AGE"].ToString().Trim();
                            if (dt.Rows[i]["CPCODE"].ToString().Trim() == "CPCODE0001")
                            {
                                lblDrugName1.Text = "";
                            }
                            else if (dt.Rows[i]["CPCODE"].ToString().Trim() == "CPCODE0002")
                            {
                                lblDrugName1.Text = "";
                            }
                            panInfo1.Visible = true;
                        }
                    }
                }
                dt.Dispose();
                dt = null;
                clsDB.DisDBConnect(DbCon2);
                DbCon2 = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.DisDBConnect(DbCon2);
                DbCon2 = null;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }
    }
}
