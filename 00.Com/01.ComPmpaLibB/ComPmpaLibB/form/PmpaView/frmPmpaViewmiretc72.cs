using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaViewmiretc72 : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmpaViewmiretc72.cs
        /// Description     : 퇴원자명단조회(총진료비)
        /// Author          : 김효성
        /// Create Date     : 2017-08-23
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "psmh\mir\miretc\FrmOutPatient.frm(Frmmiretc72) >> frmPmpaViewDischargePatientList.cs 폼이름 재정의" />

        string GstrPANO = "";

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewmiretc72(string strPANO)
        {
            InitializeComponent();
            GstrPANO = strPANO;
        }
        public frmPmpaViewmiretc72()
        {
            InitializeComponent();
        }

        private void frmPmpaViewmiretc72_Load(object sender, EventArgs e)
        {

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //if ( ComQuery . isFormAuth (clsDB.DbCon, this ) == false )
            //{ this . Close ( ); return; } //폼 권한 조회
            //ComFunc . SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y" ); //폼 기본값 세팅 등

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DEPTCODE,DEPTNAMEK ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                cboDept.Items.Clear();
                cboDept.Items.Add("**.전체과");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                cboDept.SelectedIndex = 0;

                cboDrCode.Items.Clear();
                cboDrCode.Items.Add("****.전체의사");
                cboDrCode.SelectedIndex = 0;

                dtpFdate.Value = Convert.ToDateTime(strDTP);
                dtpTdate.Value = Convert.ToDateTime(strDTP);

                cboBi.Items.Clear();
                cboBi.Items.Add("**.전체");
                cboBi.Items.Add("AA.요양급여");
                cboBi.Items.Add("BB.의료급여");
                cboBi.Items.Add("11.공단");
                cboBi.Items.Add("12.연합회");
                cboBi.Items.Add("13.지역");
                cboBi.Items.Add("21.보호1종");
                cboBi.Items.Add("22.보호2종");
                cboBi.Items.Add("23.의료부조");
                cboBi.Items.Add("24.행려환자");
                cboBi.Items.Add("31.산재");
                cboBi.Items.Add("32.공무원공상");
                cboBi.Items.Add("33.산재공상");
                cboBi.Items.Add("41.공단100%");
                cboBi.Items.Add("42.직장100%");
                cboBi.Items.Add("43.지역100%");
                cboBi.Items.Add("44.가족계획");
                cboBi.Items.Add("45.보험계약");
                cboBi.Items.Add("51.일반");
                cboBi.Items.Add("52.TA보험");
                cboBi.Items.Add("53.계약");
                cboBi.Items.Add("54.미확인");
                cboBi.Items.Add("55.TA일반");
                cboBi.SelectedIndex = 0;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            double nAmt50 = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            nAmt50 = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "	SELECT  A.ACTDATE, A.OUTDATE,A.PANO ";
                SQL = SQL + ComNum.VBLF + "		,A.SNAME, A.BI, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE";
                SQL = SQL + ComNum.VBLF + "		,A.ILSU, A.DEPTCODE, C.DRNAME,";
                SQL = SQL + ComNum.VBLF + "       A.WARDCODE, A.ROOMCODE , SUM(B.AMT50) AMT50  ";
                SQL = SQL + ComNum.VBLF + "	FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "IPD_TRANS B, " + ComNum.DB_PMPA + "BAS_DOCTOR C";
                SQL = SQL + ComNum.VBLF + "	    WHERE A.IPDNO = B.IPDNO";
                SQL = SQL + ComNum.VBLF + "		AND A.ACTDATE >=TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "		AND A.ACTDATE <=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";

                switch (VB.Left(cboBi.Text, 2))
                {
                    case "**":
                        break;
                    case "AA":
                        SQL = SQL + ComNum.VBLF + "	    AND A.BI IN ( '11','12','13')  ";
                        break;
                    case "BB":
                        SQL = SQL + ComNum.VBLF + "	    AND A.BI IN ('21','22','23','24') ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "	    AND A.BI = '" + VB.Left(cboBi.Text, 2) + "' ";
                        break;
                }

                if (VB.Left(cboDept.Text, 2) != "**")
                {
                    SQL = SQL + ComNum.VBLF + "	AND a.DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "'";
                }

                if (VB.Left(cboDrCode.Text, 4) != "****")
                {
                    SQL = SQL + ComNum.VBLF + "	AND a.DRCODE = '" + VB.Left(cboDrCode.Text, 4) + "' ";
                }

                SQL = SQL + ComNum.VBLF + "		AND A.DRCODE = C.DRCODE ";
                SQL = SQL + ComNum.VBLF + "		AND B.GBIPD IN ('1','9')";
                SQL = SQL + ComNum.VBLF + "	    GROUP BY A.ACTDATE, A.OUTDATE, A.PANO, A.SNAME, A.BI, A.INDATE, A.ILSU, A.DEPTCODE, C.DRNAME, A.WARDCODE, A.ROOMCODE";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count;
                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = DateTime.Parse(dt.Rows[i]["ACTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BI"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = DateTime.Parse(dt.Rows[i]["OUTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ILSU"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 11].Text = double.Parse(dt.Rows[i]["AMT50"].ToString().Trim()).ToString("###,###,###,###,##0 ");

                    nAmt50 = nAmt50 + double.Parse(dt.Rows[i]["AMT50"].ToString().Trim());
                }

                ssView_Sheet1.Rows.Add(0, 1);

                ssView_Sheet1.Cells[0, 0].Text = " ▶ 건 수 : " + i + ComNum.VBLF + "  ▶ Total :   " + nAmt50.ToString("###,###,###,###,##0 ");
                ssView_Sheet1.RowHeader.Cells[0, 0].BackColor = Color.FromArgb(0, 0, 255);

                ssView_Sheet1.AddSpanCell(0, 0, 1, 12);

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            string strHead = "";
            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";

            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs0";
            strFont2 = "/fn\"바탕체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs0";
            strHead = "퇴원자 명단 조회 (총진료비)";

            strHead1 = "/f1" + VB.Space(15) + strHead;
            strHead2 = "/l/f2" + "작업기간 : " + dtpFdate.Value.ToString("yyyy-MM-dd") + " ==> " + dtpTdate.Value.ToString("yyyy-MM-dd") + "/n";

            strHead2 = strHead2 + "/l/f2" + "보험자격 : " + cboBi.Text + "/n";
            strHead2 = strHead2 + "/l/f2" + "진 료 과 : " + cboDept.Text + "/n";
            strHead2 = strHead2 + "/l/f2" + "진료의사 : " + cboDrCode.Text + "/n";
            strHead2 = strHead2 + "/l/f2" + "인쇄일자 : " + DateTime.Parse(strDTP).ToString("yyyy-MM-dd") + "/n";

            btnPrint.Enabled = false;

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (e.Row == 0 || e.Column == 0)
            {
                return;
            }
            GstrPANO = ssView_Sheet1.Cells[e.Row, 0].Text;

            //TODO 폼 열기
            //FrmSlipView_Ipd.Show
        }

        private void cboDept_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strDept = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            strDept = VB.Left(cboDept.Text, 2);
            cboDrCode.Items.Clear();

            if (strDept == "")
            {
                return;
            }

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DRCODE, DRNAME FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE TOUR = 'N' ";

                if (strDept != "**")
                {
                    SQL = SQL + ComNum.VBLF + "   AND DRDEPT1  = '" + strDept + "' ";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    cboDrCode.Items.Add("****.전체");
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("진료과장이 없습니다.", "확인");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDrCode.Items.Add(dt.Rows[i]["DRCODE"].ToString().Trim() + "." + dt.Rows[i]["DRNAME"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
