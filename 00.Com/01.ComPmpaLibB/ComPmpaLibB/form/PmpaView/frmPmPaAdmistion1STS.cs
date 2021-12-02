using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    public partial class frmPmPaAdmistion1STS : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmPaAdmistion1STS.cs
        /// Description     : 조합용 수진자 현황(지역코드기준)
        /// Author          : 김효성
        /// Create Date     : 2017-09-13
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// </history>
        /// <seealso cref= psmh\IPD\ilrepb\ilrepb.vbp\FrmAdmistion1(ILREPB19.FRM)  >> frmPmPaAdmistion1STS.cs 폼이름 재정의" />	

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmPaAdmistion1STS()
        {
            InitializeComponent();
        }

        private void frmPmPaAdmistion1STS_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            cboJohap.Items.Add("포항시남구");
            cboJohap.Items.Add("포항시북구");
            cboJohap.Items.Add("영덕군");
            cboJohap.Items.Add("울진군");
            cboJohap.Items.Add("경주시");
            cboJohap.Items.Add("영천시");
            cboJohap.SelectedIndex = 0;
        }

        private void Screen_Clear()
        {
            ssView_Sheet1.RowCount = 50;
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";

            btnView.Enabled = true;
            btnPrint.Enabled = true;
            btnExit.Enabled = true;
            btncancel.Enabled = true;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            int i = 0;
            int nRow = 0;
            string strTdate = "";
            string strPANO = "";
            string strKiho = "";
            string StrGkiho = "";
            string strGwange = "";
            string StrPname = "";
            string strDiagnosys = "";
            string strJumin = "";
            string StrTel = "";
            string StrDeptNameK = "";
            string strInDate = "";
            DataTable dt = null;
            DataTable dtfn = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            strTdate = Convert.ToDateTime(dtpTdate.Text).AddDays(1).ToString("yyyy-MM-dd");


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT * FROM " + ComNum.DB_PMPA + "BBASCD";  //BAS_BASCD


                //'자료를 SELECT
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.RoomCode,B.Pano,A.Sname,B.Bi,B.DeptCode,A.AmSet1,                    ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(B.InDate,'YYYY-MM-DD') InDate,                       ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(B.OutDate,'YYYY-MM-DD') OutDate                      ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, " + ComNum.DB_PMPA + "IPD_TRANS B ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                     ";
                SQL = SQL + ComNum.VBLF + "    AND B.Bi IN ('11','12','13')                                     ";
                if (chkBun.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND B.OUTDATE IS NULL ";
                }

                SQL = SQL + ComNum.VBLF + "    AND B.GBIPD NOT IN ('9','D') ";
                SQL = SQL + ComNum.VBLF + "    AND ((A.AmSet6 = '*' AND B.Amt50 > 0) or (A.AmSet6 <> '*' ))         ";// & vbLf '환자구분변경
                SQL = SQL + ComNum.VBLF + "    AND A.AmSet4 <> '3'                                              ";//& vbLf '정상애기
                SQL = SQL + ComNum.VBLF + "    AND B.Pano <> '81000004'                                         "; //& vbLf
                SQL = SQL + ComNum.VBLF + "    AND B.InDate >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')    ";// & vbLf
                SQL = SQL + ComNum.VBLF + "    AND B.InDate <  TO_DATE('" + strTdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND A.IPDNO = B.IPDNO ";
                SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY 1,2                                                     ";// & vbLf

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                nRow = 0;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strPANO = dt.Rows[i]["Pano"].ToString().Trim();
                    //진료과 명칭
                    strInDate = dt.Rows[i]["Indate"].ToString().Trim();
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT DeptNameK                                              ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT                                         ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "    AND DeptCode='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "' ";

                    SqlErr = clsDB.GetDataTable(ref dtfn, SQL, clsDB.DbCon);

                    StrDeptNameK = "";

                    if (dtfn.Rows.Count > 0)
                    {
                        StrDeptNameK = dtfn.Rows[0]["DeptNameK"].ToString().Trim();
                    }

                    dtfn.Dispose();
                    dtfn = null;

                    //   '증번호등을 READ

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT Kiho,GKiho,PName,Gwange                        ";
                    SQL = SQL + ComNum.VBLF + "   FROM BAS_MIH                                        ";
                    SQL = SQL + ComNum.VBLF + "  WHERE Pano='" + strPANO + "'                         ";
                    SQL = SQL + ComNum.VBLF + "    AND Bi = '" + dt.Rows[i]["Bi"].ToString() + "'   ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY TransDate DESC                              ";

                    SqlErr = clsDB.GetDataTable(ref dtfn, SQL, clsDB.DbCon);

                    if (dtfn.Rows.Count > 0)
                    {
                        strKiho = dtfn.Rows[0]["Kiho"].ToString().Trim();
                        StrGkiho = dtfn.Rows[0]["GKiho"].ToString().Trim();
                        if (StrGkiho == "")
                        {
                            StrGkiho = strKiho;
                        }
                        StrPname = dtfn.Rows[0]["Pname"].ToString().Trim();
                        strGwange = dtfn.Rows[0]["Gwange"].ToString().Trim();
                    }
                    else
                    {
                        strKiho = "";
                        StrGkiho = "";
                        StrPname = "";
                        strGwange = "";
                    }
                    dtfn.Dispose();
                    dtfn = null;

                    //'주민번호를 READ

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT Jumin1, Jumin2, Tel    ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT            ";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "    AND Pano='" + strPANO + "' ";

                    SqlErr = clsDB.GetDataTable(ref dtfn, SQL, clsDB.DbCon);

                    if (dtfn.Rows.Count > 0)
                    {
                        strJumin = dtfn.Rows[0]["Jumin1"].ToString().Trim() + "-" + dtfn.Rows[0]["Jumin2"].ToString().Trim();
                        StrTel = dtfn.Rows[0]["Tel"].ToString().Trim();
                    }
                    else
                    {
                        strJumin = "";
                    }
                    dtfn.Dispose();
                    dtfn = null;

                    //'상병명을 READ(간호업무)

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT Diagnosys FROM NUR_JINDAN                          ";
                    SQL = SQL + ComNum.VBLF + "  WHERE Pano='" + strPANO + "'                             ";
                    SQL = SQL + ComNum.VBLF + "    AND INDATE = TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY InDate DESC                                     ";

                    SqlErr = clsDB.GetDataTable(ref dtfn, SQL, clsDB.DbCon);

                    if (dtfn.Rows.Count > 0)
                    {
                        strDiagnosys = dtfn.Rows[0]["Diagnosys"].ToString().Trim();
                    }
                    else
                    {
                        strDiagnosys = "";
                    }
                    dtfn.Dispose();
                    dtfn = null;

                    nRow = nRow + 1;

                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = StrPname;

                    switch (strGwange)
                    {
                        case "1":
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = "본인";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = "부모";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = "자";
                            break;
                        case "4":
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = "처";
                            break;
                        default:
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = "기타";
                            break;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = strJumin;
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = strDiagnosys;
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = StrDeptNameK;
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = StrTel;
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = nRow;

                btncancel.Enabled = true;
                btnPrint.Enabled = true;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "입 원   수 진 자  현 황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력기간 : : " + dtpFDate.Text + " ~ " + dtpTdate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("요양기관 : 포항성모병원 ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("조 합 명 : 전 체 " + VB.Now().ToString() + "   " + "출력일시: " + strDTP + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btncansel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
