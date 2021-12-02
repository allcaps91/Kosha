using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmWardInwonPrint2New.cs
    /// Description     : 병동별근무형태
    /// Author          : 박창욱
    /// Create Date     : 2018-02-05
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrtong\nrtong23New.frm(FrmWardInwonPrint2New.frm) >> frmWardInwonPrint2New.cs 폼이름 재정의" />	
    public partial class frmWardInwonPrint2New : Form
    {
        int nColCount = 0;
        int[,] nDATA2 = new int[5, 41];
        double[] nTotal1 = new double[4];
        double[,] nTotal2 = new double[3, 41];
        string[,] nDATA = new string[5, 41];

        public frmWardInwonPrint2New()
        {
            InitializeComponent();
        }

        void Screen_Clear()
        {
            ssView_Sheet1.Cells[1, 2, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            ssView.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cboYYMM.Focus();
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

            strTitle = "각병동 간호사, 간호조무사 근무인원 현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("통 계 월 : " + cboYYMM.Text + "분", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("출 력 자 : " + clsType.User.JobMan, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.80f);

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

            int j = 0;
            int nRow = 0;
            int nRow2 = 0;
            int nCol = 0;
            int nRNTot = 0;
            int nNATot = 0;
            string strYYMM = "";

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2);

            for (i = 2; i < 4; i++)
            {
                for (j = 1; j <= nDATA.GetUpperBound(1); j++)
                {
                    nDATA[i, j] = "";
                }
                nTotal1[1] = 0;
                nTotal1[i] = 0;
            }

            for (i = 1; i < 4; i++)
            {
                for (j = 1; j < nDATA2.GetUpperBound(1); j++)
                {
                    nDATA2[i, j] = 0;
                }
            }

            for (i = 1; i < 3; i++)
            {
                for (j = 1; j < nTotal2.GetUpperBound(1); j++)
                {
                    nTotal2[i, j] = 0;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //근무형태
                SQL = "";
                SQL = "SELECT WARDCODE, CODE, QTY1, QTY2 ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_TONG3";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM = '" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND WARDCODE IN ('NR','ND','ER','HD','CSR','OR','OPD','GAN','HU','32','53','55','63','65','70','73','75','80','DS') ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    #region SEARCH_WARD

                    switch (dt.Rows[i]["WardCode"].ToString().Trim().ToUpper())
                    {
                        case "NR":
                        case "ND":
                            nRow = 2;
                            nRow2 = 1;
                            break;
                        case "ER":
                            nRow = 4;
                            nRow2 = 2;
                            break;
                        case "HD":
                            nRow = 6;
                            nRow2 = 3;
                            break;
                        case "CSR":
                            nRow = 8;
                            nRow2 = 4;
                            break;
                        case "OR":
                            nRow = 10;
                            nRow2 = 5;
                            break;
                        case "OPD":
                            nRow = 12;
                            nRow2 = 6;
                            break;
                        case "GAN":
                            nRow = 14;
                            nRow2 = 7;
                            break;
                        case "DS":
                            nRow = 16;
                            nRow2 = 8;
                            break;
                        case "32":
                            nRow = 18;
                            nRow2 = 9;
                            break;
                        case "53":
                            nRow = 20;
                            nRow2 = 10;
                            break;
                        case "55":
                            nRow = 22;
                            nRow2 = 11;
                            break;
                        case "63":
                            nRow = 24;
                            nRow2 = 12;
                            break;
                        case "65":
                            nRow = 26;
                            nRow2 = 13;
                            break;
                        case "70":
                            nRow = 28;
                            nRow2 = 14;
                            break;
                        case "73":
                            nRow = 30;
                            nRow2 = 15;
                            break;
                        case "75":
                            nRow = 32;
                            nRow2 = 16;
                            break;
                        case "80":
                            nRow = 34;
                            nRow2 = 17;
                            break;
                    }

                    #endregion

                    for (j = 1; j <= nDATA.GetUpperBound(1); j++)
                    {
                        if (nDATA[1, j] == dt.Rows[i]["Code"].ToString().Trim().ToUpper())
                        {
                            nCol = j + 2;
                            break;
                        }
                    }

                    ssView_Sheet1.Cells[nRow - 1, nCol - 1].Text = dt.Rows[i]["Qty1"].ToString().Trim();
                    nDATA[2, nCol - 2] = (VB.Val(nDATA[2, nCol - 2]) + VB.Val(dt.Rows[i]["Qty1"].ToString().Trim())).ToString();
                    nTotal2[1, nRow2] += VB.Val(dt.Rows[i]["Qty1"].ToString().Trim());

                    ssView_Sheet1.Cells[nRow, nCol - 1].Text = dt.Rows[i]["Qty2"].ToString().Trim();
                    nDATA[3, nCol - 2] = (VB.Val(nDATA[3, nCol - 2]) + VB.Val(dt.Rows[i]["Qty2"].ToString().Trim())).ToString();
                    nTotal2[2, nRow2] += VB.Val(dt.Rows[i]["Qty2"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                for (i = 1; i <= nColCount; i++)
                {
                    ssView_Sheet1.Cells[35, i + 1].Text = nDATA[2, i];
                    nRNTot += (int)VB.Val(nDATA[2, i]);
                    ssView_Sheet1.Cells[36, i + 1].Text = nDATA[3, i];
                    nNATot += (int)VB.Val(nDATA[3, i]);
                }

                for (i = 1; i < 18; i++)
                {
                    ssView_Sheet1.Cells[(i * 2) - 1, nColCount + 2].Text = nTotal2[1, i].ToString();
                    ssView_Sheet1.Cells[i * 2, nColCount + 2].Text = nTotal2[2, i].ToString();
                }

                ssView_Sheet1.Cells[35, nColCount + 2].Text = nRNTot.ToString();
                ssView_Sheet1.Cells[36, nColCount + 2].Text = nNATot.ToString();

                btnPrint.Enabled = true;
                btnCancel.Enabled = true;
                ssView.Enabled = true;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboYYMM_Enter(object sender, EventArgs e)
        {
            Screen_Clear();
            btnSearch.Enabled = true;
            btnPrint.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
        }

        private void frmWardInwonPrint2New_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nYY = 0;
            int nMM = 0;
            string strYYMM = "";
            string strSysDate = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            nYY = (int)VB.Val(VB.Left(strSysDate, 4));
            nMM = (int)VB.Val(VB.Mid(strSysDate, 6, 2));
            strYYMM = nYY.ToString("0000") + nMM.ToString("00");

            cboYYMM.Items.Clear();

            for (i = 1; i < 25; i++)
            {
                cboYYMM.Items.Add(VB.Left(strYYMM, 4) + "년" + VB.Right(strYYMM, 2) + "월");
                strYYMM = clsVbfunc.DateYYMMAdd(strYYMM, -1);
                if (strYYMM == "199712")
                {
                    break;
                }
            }

            cboYYMM.SelectedIndex = 1;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT CODE,NAME";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_CODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN= '4'";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

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

                nColCount = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nColCount += 1;
                }

                ssView_Sheet1.ColumnCount = nColCount + 3;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[0, i + 2].Text = dt.Rows[i]["Code"].ToString().Trim();
                    nDATA[1, i + 1] = dt.Rows[i]["Code"].ToString().Trim();
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
    }
}
