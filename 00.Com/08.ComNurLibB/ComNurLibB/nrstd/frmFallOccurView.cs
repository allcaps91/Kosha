using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using ComLibB;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmFallOccurView.cs
    /// Description     : 낙상발생리스트
    /// Author          : 박창욱
    /// Create Date     : 2018-01-25
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm낙상발생리스트.frm(Frm낙상발생리스트.frm) >> frmFallOccurView.cs 폼이름 재정의" />	
    public partial class frmFallOccurView : Form
    {
        string GsWard = "";

        public frmFallOccurView()
        {
            InitializeComponent();
        }

        public frmFallOccurView(string sWard)
        {
            InitializeComponent();

            this.GsWard = sWard;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "낙상발생리스트";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조회기간 : " + dtpDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpDate1.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strTemp = "";

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT Pano,IPDNO,SName,Sex,Age,DIAGNOSYS,DeptCode, EntDate, EntSabun,Gubun, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(ActDate,'YYYY-MM-DD') BDate, ";
                SQL = SQL + ComNum.VBLF + "   TO_CHAR(ActDate,'YYYY-MM-DD HH24:MI') ActDate,TO_CHAR(SeekDate,'YYYY-MM-DD HH24:MI') SeekDate,   ";
                SQL = SQL + ComNum.VBLF + "   TO_CHAR(ReturnDate,'YYYY-MM-DD HH24:MI') ReturnDate,Nur_Fall6,Nur_Fall7, WARDCODE, MAX(rowid) ROWID1";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_Fall_Report ";
                if (rdoGubun1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE ReturnDate >=TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ReturnDate <=TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE ActDate >=TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ActDate <=TO_DATE('" + dtpDate1.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                }
                if (clsPublic.GstrWardCodes != "HD")
                {
                    if (cboWard.Text.Trim() != "")
                    {
                        switch (cboWard.Text.Trim())
                        {
                            case "전체":
                                SQL = SQL + ComNum.VBLF + " AND RoomCode NOT IN ('233','234') ";
                                break;
                            case "SICU":
                                SQL = SQL + ComNum.VBLF + " AND RoomCode = '233' ";
                                break;
                            case "MICU":
                                SQL = SQL + ComNum.VBLF + " AND RoomCode = '234' ";
                                break;
                            case "ND":
                                SQL = SQL + ComNum.VBLF + " AND RoomCode IN('369','358','398') ";
                                break;
                            default:
                                SQL = SQL + ComNum.VBLF + " AND WARDCODE='" + cboWard.Text.Trim() + "' ";
                                break;
                        }
                    }
                }
                SQL = SQL + ComNum.VBLF + " GROUP BY  PANO, IPDNO,       SName,       Sex,       Age,       DIAGNOSYS,       DeptCode,       EntDate,       EntSabun, ";
                SQL = SQL + ComNum.VBLF + " Gubun,       ActDate,       ActDate,       SeekDate,       ReturnDate,       Nur_Fall6,       Nur_Fall7, WARDCODE ";
                SQL = SQL + ComNum.VBLF + "ORDER BY ActDate     ";


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;
                //ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                ssView_Sheet1.SetRowHeight(-1, 28);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SeekDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ReturnDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();

                    strTemp = "";
                    if (VB.Mid(dt.Rows[i]["Nur_Fall6"].ToString().Trim(), 1, 1) == "1")
                    {
                        strTemp += "침대,";
                    }
                    if (VB.Mid(dt.Rows[i]["Nur_Fall6"].ToString().Trim(), 2, 1) == "1")
                    {
                        strTemp += "의료장비,";
                    }
                    if (VB.Mid(dt.Rows[i]["Nur_Fall6"].ToString().Trim(), 3, 1) == "1")
                    {
                        strTemp += "의자,";
                    }
                    if (VB.Mid(dt.Rows[i]["Nur_Fall6"].ToString().Trim(), 4, 1) == "1")
                    {
                        strTemp += "보행,";
                    }
                    if (VB.Mid(dt.Rows[i]["Nur_Fall6"].ToString().Trim(), 5, 1) == "1")
                    {
                        strTemp += "기타,";
                    }
                    ssView_Sheet1.Cells[i, 6].Text = strTemp;

                    strTemp = "";
                    if (VB.Mid(dt.Rows[i]["Nur_Fall7"].ToString().Trim(), 1, 1) == "1")
                    {
                        strTemp += "병실,";
                    }
                    if (VB.Mid(dt.Rows[i]["Nur_Fall7"].ToString().Trim(), 2, 1) == "1")
                    {
                        strTemp += "화장실,";
                    }
                    if (VB.Mid(dt.Rows[i]["Nur_Fall7"].ToString().Trim(), 3, 1) == "1")
                    {
                        strTemp += "샤워실,";
                    }
                    if (VB.Mid(dt.Rows[i]["Nur_Fall7"].ToString().Trim(), 4, 1) == "1")
                    {
                        strTemp += "복도,";
                    }
                    if (VB.Mid(dt.Rows[i]["Nur_Fall7"].ToString().Trim(), 5, 1) == "1")
                    {
                        strTemp += "응급실,";
                    }
                    if (VB.Mid(dt.Rows[i]["Nur_Fall7"].ToString().Trim(), 6, 1) == "1")
                    {
                        strTemp += "중환자실,";
                    }
                    if (VB.Mid(dt.Rows[i]["Nur_Fall7"].ToString().Trim(), 7, 1) == "1")
                    {
                        strTemp += "검사실,";
                    }
                    if (VB.Mid(dt.Rows[i]["Nur_Fall7"].ToString().Trim(), 8, 1) == "1")
                    {
                        strTemp += "기타,";
                    }
                    ssView_Sheet1.Cells[i, 7].Text = strTemp;

                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Ipdno"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["EntSabun"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    if (dt.Rows[i]["IPDNO"].ToString().Trim() == "0")
                    {
                        ssView_Sheet1.Cells[i, 13].Text = "OPD";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    }
                    ssView_Sheet1.Cells[i, 14].Text = dt.Rows[i]["ROWID1"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmFallOccurView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-10);

            if (GsWard == "")
            {
                GsWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");
            }

            Set_cboWard();

            ssView_Sheet1.Columns[10].Visible = false;
            //ssView_Sheet1.Columns[11].Visible = false;
            ssView_Sheet1.Columns[12].Visible = false;


            cboWard.Enabled = ComQuery.NURSE_System_Manager_Check(VB.Val(clsType.User.Sabun));

            if (clsType.User.BuseCode == "076001")
            {
                cboWard.Enabled = true;
                cboWard.SelectedIndex = 0;
                panel1.Visible = false;
            }
            else
            {
                panel1.Visible = true;
            }


        }

        void Set_cboWard()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT WardCode, WardName  ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU','NP','2W','NR','DR','IQ','ER') ";
                SQL = SQL + ComNum.VBLF + "   AND USED = 'Y'";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                cboWard.Items.Clear();
                cboWard.Items.Add("전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }

                //cboWard.Items.Add("SICU");
                //cboWard.Items.Add("MICU");

                cboWard.SelectedIndex = -1;

                if (cboWard.Items.IndexOf(GsWard) != -1)
                {
                    cboWard.SelectedIndex = cboWard.Items.IndexOf(GsWard);
                    cboWard.Enabled = false;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            frmFallReport frmFallReportX = null;
            string strGUBUN = "";
            string strWard = "";
            string strROWID = "";

            strGUBUN = ssView_Sheet1.Cells[e.Row, 10].Text.Trim();
            strWard = ssView_Sheet1.Cells[e.Row, 13].Text.Trim();
            strROWID = ssView_Sheet1.Cells[e.Row, 14].Text.Trim();

            if (strWard != "OPD")
            {
                //IPDNO 컬럼
                frmFallReportX = new frmFallReport(strWard, ssView_Sheet1.Cells[e.Row, 8].Text.Trim(), "", "", strROWID);
            }
            else
            {
                //PANO, BDATE, DEPTCODE 컬럼
                frmFallReportX = new frmFallReport("OPD", ssView_Sheet1.Cells[e.Row, 11].Text.Trim(), ssView_Sheet1.Cells[e.Row, 12].Text, ssView_Sheet1.Cells[e.Row, 5].Text);
            }

            frmFallReportX.StartPosition = FormStartPosition.CenterParent;
            frmFallReportX.ShowDialog();
            frmFallReportX.Dispose();
            frmFallReportX = null;
            btnSearch_Click(this.btnSearch, null);
            
            //this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            clsSpread CS = new clsSpread();
            CS.ExportToXLS(ssView);
            CS = null;
        }
    }
}
