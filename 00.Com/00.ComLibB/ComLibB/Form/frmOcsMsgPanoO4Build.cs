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
    /// Class Name      : ComLibB
    /// File Name       : frmOcsMsgPanoO4Build.cs
    /// Description     :치매약제 메세지 처리 - [외래] 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-08
    /// Update History  : try-catch문 수정
    /// <history>       
    /// D:\타병원\PSMHH\basic\busuga\BuSuga65.frm(FrmOcsMsgPano_O4_Build) => frmOcsMsgPanoO4Build.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\busuga\BuSuga65.frm(FrmOcsMsgPano_O4_Build)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\busuga\busuga.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmOcsMsgPanoO4Build : Form
    {
        ComFunc CF = new ComFunc();
        string GstrHelpCode = "";
        public frmOcsMsgPanoO4Build()
        {
            InitializeComponent();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmOcsMsgPanoO4Build_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            optGB1.Checked = true;
            chkAll.Checked = true;

            SetCombo();
        }

        void SetCombo()
        {
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

        void btnMsg_Click(object sender, EventArgs e)
        {
            int i = 0;

            string strPano = "";
            string strSname = "";
            string strResult_A = "";
            string strResult_B = "";
            string strExamDate_A = "";
            string strExamDate_B = "";

            string strMsg = "";

            for(i = 0; i < ssMsg_Sheet1.RowCount; i++)
            {
                if(ssMsg_Sheet1.Cells[i, 0].Text == "True")
                {
                    strPano = ssMsg_Sheet1.Cells[i, 2].Text;
                    strSname = ssMsg_Sheet1.Cells[i, 3].Text;

                    strExamDate_A = VB.Replace(ssMsg_Sheet1.Cells[i, 6].Text, "-", "/");

                    strMsg = "";
                    strMsg = "          <<< 치매 검사처방 관련 정보 >>>              " + ComNum.VBLF;
                    strMsg += "-----------------------------------------------   " + ComNum.VBLF;
                    strMsg += "  최근 처방일자 : " + strExamDate_A + ComNum.VBLF;
                    strMsg += "-----------------------------------------------   " + ComNum.VBLF;
                    
                    strMsg += " ◈ 치매검사 오더 부탁드립니다.   " + ComNum.VBLF;

                    BAS_OCSMEMO_O2(strPano, strSname);

                }
            }
            
            ComFunc.MsgBox("등록된 메세지를 확인해보세요.");

        }

        void BAS_OCSMEMO_O2(string ArgPano, string ArgSName)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            string strData = "";            
            string strROWID = "";
            int nWRTNO = 0;

            strData = VB.Replace(lblInfo.Text, "'", "`");

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  ROWID";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_O2";
                SQL += ComNum.VBLF + "WHERE PANO = '" + ArgPano + "'";
                SQL += ComNum.VBLF + "  AND DDATE IS NULL ";
                SQL += ComNum.VBLF + "ORDER BY SDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                strROWID = "";

                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["RowID"].ToString().Trim();
                }

                SqlErr = "";
                dt.Dispose();
                dt = null;

                if (strROWID == "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  MAX(WRTNO) MWRTNO ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_O2";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    nWRTNO = Convert.ToInt16(dt.Rows[0]["MWRTNO"].ToString().Trim()) + 1;

                    SqlErr = "";
                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO ";
                    SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_OCSMEMO_O2 ";
                    SQL += ComNum.VBLF + "(  PANO, SNAME, MEMO,  SDATE, DDATE, WRTNO, GBJOB )";
                    SQL += ComNum.VBLF + "VALUES ( ";
                    SQL += ComNum.VBLF + "'" + ArgPano + "', ";
                    SQL += ComNum.VBLF + "'" + ArgSName + "', ";
                    SQL += ComNum.VBLF + "'', ";
                    SQL += ComNum.VBLF + "trunc(sysdate), ";
                    SQL += ComNum.VBLF + "'' ,";
                    SQL += ComNum.VBLF + "'" + nWRTNO + "', ";
                    SQL += ComNum.VBLF + "'5' ";
                    SQL += ComNum.VBLF + " )";
                    SqlErr = clsDB.ExecuteLongQuery(SQL, "Memo" , ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SqlErr = "";
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  ROWID ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_O2";
                    SQL += ComNum.VBLF + "WHERE PANO = '" + ArgPano + "'  ";
                    SQL += ComNum.VBLF + "  AND WRTNO = '" + nWRTNO + "'  ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE ";
                    SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_OCSMEMO_O2 ";
                    SQL += ComNum.VBLF + "SET ";
                    SQL += ComNum.VBLF + "MEMO = '' ";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";
                    SqlErr = clsDB.ExecuteLongQuery(SQL, "Memo", ref intRowAffected, clsDB.DbCon);
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
                 ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                dt.Dispose();
                dt = null;
            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }


        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strHead1 = "/c/f1" + "치 매 약 제 관 련 메세지 대상자" + "/n";

            strFont2 = "/fn\"굴림체\" /fz\"10\" /fs2";
            strHead2 = "/l/f2" + "인쇄일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss");
            strHead2 = strHead2 + "/r/f2" + "PAGE : /p";

            ssMsg_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;

            ssMsg_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;

            ssMsg_Sheet1.PrintInfo.Margin.Top = 50;
            ssMsg_Sheet1.PrintInfo.Margin.Bottom = 2000;
            ssMsg_Sheet1.PrintInfo.Margin.Left = 0;
            ssMsg_Sheet1.PrintInfo.Margin.Right = 0;

            ssMsg_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssMsg_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;

            ssMsg_Sheet1.PrintInfo.ShowBorder = true;
            ssMsg_Sheet1.PrintInfo.ShowColor = false;
            ssMsg_Sheet1.PrintInfo.ShowGrid = false;
            ssMsg_Sheet1.PrintInfo.ShowShadows = true;
            ssMsg_Sheet1.PrintInfo.UseMax = false;
            ssMsg_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssMsg_Sheet1.PrintInfo.Preview = true;
            ssMsg.PrintSheet(0);
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            GetData();
        }

        void GetData()
        {
            if(optGB1.Checked == true)
            {
                if (MessageBox.Show("해당작업은 조회 시간이 오래걸립니다. 작업을 계속하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            OPD_OCS_MSG_Process(dtpFDate.Text, dtpTDate.Text, VB.Replace(cboDay.SelectedItem.ToString(), "일전", ""));
        }

        /// <summary>
        /// 외래 OCS 메세지 처리
        /// </summary>
        /// <param name="ArgFdate"></param>
        /// <param name="ArgTdate"></param>
        /// <param name="ArgDay"></param>
        void OPD_OCS_MSG_Process(string ArgFdate, string ArgTdate, string ArgDay)
        {
            int i = 0;
            int j = 0;

            string strOK_A = "";
            string strOK_B = "";
            int nREAD = 0;

            string strExamDate = "";
            string strTDate = "";

            int nRow = 0;
            string strExamDate_A = "";
            string strExamResult_A = "";

            string strExamDate_B = "";
            string strExamResult_B = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            strTDate = CF.DATE_ADD(clsDB.DbCon, ArgTdate, 1);
            strExamDate = CF.DATE_ADD(clsDB.DbCon, DateTime.Now.ToString("yyyy-MM-dd"), Convert.ToInt16(ArgDay) * -1);

            //간장용제 환자
            try
            {
                if (optGB0.Checked == true)  //당일 예약환자기준
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  TRUNC(DATE3) ACTDATE, A.PANO ,  A.SNAME,  A.DRCODE, C.DRNAME, A.DEPTCODE ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW A, " + ComNum.DB_PMPA + "OPD_SLIP B, " + ComNum.DB_PMPA + "BAS_DOCTOR C";
                    SQL += ComNum.VBLF + "WHERE A.DATE3 >= TO_DATE('" + ArgFdate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  AND A.DATE3 < TO_DATE('" + strTDate + "','YYYY-MM-DD') "; //시간 분까지 데이타발생
                    SQL += ComNum.VBLF + "  AND A.PANO = B.PANO ";
                    SQL += ComNum.VBLF + "  AND A.DATE1 = B.ACTDATE";
                    SQL += ComNum.VBLF + "  AND A.DEPTCODE = B.DEPTCODE ";
                    SQL += ComNum.VBLF + "  AND A.DRCODE = C.DRCODE ";
                    SQL += ComNum.VBLF + "  AND B.SUNEXT IN ( SELECT SUNEXT FROM BAS_SUN WHERE GBDEMENTIA ='Y' ) ";
                    SQL += ComNum.VBLF + "GROUP BY TRUNC(DATE3), A.PANO, A.SNAME,  A.DRCODE, C.DRNAME , A.DEPTCODE ";
                    SQL += ComNum.VBLF + "HAVING SUM(B.QTY * B.NAL) > 0 ";
                    SQL += ComNum.VBLF + "ORDER BY 4 ";
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  TRUNC(B.ACTDATE) ACTDATE, A.PANO ,  A.SNAME,  A.DRCODE, C.DRNAME, A.DEPTCODE ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "OPD_SLIP B, " + ComNum.DB_PMPA + "BAS_DOCTOR C";
                    SQL += ComNum.VBLF + "WHERE B.ACTDATE >= TO_DATE('" + ArgFdate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  AND B.ACTDATE <= TO_DATE('" + ArgTdate + "','YYYY-MM-DD') "; //시간 분까지 데이타발생
                    SQL += ComNum.VBLF + "  AND A.PANO = B.PANO ";
                    SQL += ComNum.VBLF + "  AND A.ACTDATE = B.ACTDATE";
                    SQL += ComNum.VBLF + "  AND A.DEPTCODE = B.DEPTCODE ";
                    SQL += ComNum.VBLF + "  AND A.DRCODE = C.DRCODE ";
                    SQL += ComNum.VBLF + "  AND B.SUNEXT IN ( SELECT SUNEXT FROM BAS_SUN WHERE GBDEMENTIA ='Y' ) ";
                    SQL += ComNum.VBLF + "GROUP BY TRUNC(B.ACTDATE), A.PANO, A.SNAME,  A.DRCODE, C.DRNAME , A.DEPTCODE";
                    SQL += ComNum.VBLF + "HAVING SUM(B.QTY * B.NAL) > 0 ";
                    SQL += ComNum.VBLF + "ORDER BY 4 ";
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssMsg_Sheet1.RowCount = dt.Rows.Count;
                SqlErr = "";
                for (i = 0; i < ssMsg_Sheet1.RowCount; i++)
                {
                    ssMsg_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ACTDate"].ToString().Trim() + "";
                    ssMsg_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Pano"].ToString().Trim() + "";
                    ssMsg_Sheet1.Cells[i, 3].Text = dt.Rows[i]["sName"].ToString().Trim() + "";
                    ssMsg_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim() + "";
                    ssMsg_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DrName"].ToString().Trim() + "";

                    SQL = "";
                    SQL += ComNum.VBLF + "CREATE OR REPLACE VIEW VIEW_SLIP_F6216 AS";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  TO_CHAR(BDATE,'YYYY-MM-DD') BDATE";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP";
                    SQL += ComNum.VBLF + "WHERE BDATE >=TO_DATE('" + strExamDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "  AND PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "" + "' ";
                    SQL += ComNum.VBLF + "  AND SUCODE IN ( 'F6216', 'F6216P', 'FY686' )";
                    SQL += ComNum.VBLF + "GROUP BY BDATE";
                    SQL += ComNum.VBLF + "HAVING SUM(QTY * NAL) > 0 ";
                    SQL += ComNum.VBLF + "UNION ALL";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "TO_CHAR(BDATE,'YYYY-MM-DD') BDATE";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP";
                    SQL += ComNum.VBLF + "WHERE BDATE >=TO_DATE('" + strExamDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  AND PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "" + "' ";
                    SQL += ComNum.VBLF + "  AND SUCODE IN ( 'F6216', 'F6216P','FY686' )";
                    SQL += ComNum.VBLF + "GROUP BY BDATE";
                    SQL += ComNum.VBLF + "HAVING SUM(QTY * NAL) > 0";
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    dt.Dispose();
                    dt = null;
                    SqlErr = "";

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  MAX(BDATE) BDATE";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "VIEW_SLIP_F6216";
                    SQL += ComNum.VBLF + "ORDER BY BDATE ASC";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ssMsg_Sheet1.Cells[i, 6].Text = dt.Rows[i]["BDate"].ToString().Trim() + "";
                    }

                    dt.Dispose();
                    dt = null;
                }

                dt2.Dispose();
                dt2 = null;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void ssMsg_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GstrHelpCode = ssMsg_Sheet1.Cells[e.Row, 2].Text;
        }

        private void ssMsg_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            if(e.Column != 2)
            {
                return;
            }
            string SName = CF.Read_Patient(clsDB.DbCon, ssMsg_Sheet1.Cells[e.Row, 2].Text, "2");
            ssMsg_Sheet1.Cells[e.Row, 3].Text = SName;
        }
    }
}
