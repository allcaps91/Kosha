using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-02-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\nurse\nrtong\nrtong15.frm >> frmWardInwonPrint.cs 폼이름 재정의" />

    public partial class frmNurseJobPrint : Form
    {
        string[,,] nDATA = new string[34, 21, 37];
        int nColCount = 0;
        int[,] nTotal = new int[5, 37];

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();

        public frmNurseJobPrint()
        {
            InitializeComponent();
        }

        private void Clear()
        {
            int i = 0;
            int j = 0;
            int k = 0;

            SS1_Sheet1.Cells[1, 2, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].Text = "";

            for (i = 2; i <= 3; i++)
            {
                for (j = 1; j <= 14; j++)
                {
                    for (k = 1; k <= 30; k++)
                    {
                        nDATA[i, j, k] = "";
                    }
                }

            }

            for (i = 1; i <= 4; i++)
            {
                nTotal[i, j] = 0;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnPrint.Enabled = false;
            dtpdate.Select();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                strTitle = "간호사 & 간호조무사 근무 현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            int J = 0;
            string strYYMM = "";
            int nDept = 0;
            //int nRow = 0;
            int nCount = 0;
            string strCODE = "";
            string strJik = "";
            int nTotal1 = 0;
            int nTotal2 = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Clear();


            Cursor.Current = Cursors.WaitCursor;

            strYYMM = VB.Left(dtpdate.Value.ToString("yyyy-MM-dd"), 4) + VB.Mid(dtpdate.Value.ToString("yyyy-MM-dd"), 6, 2);
            nCount = (int)VB.Val(VB.Right(dtpdate.Value.ToString("yyyy-MM-dd"), 2));

            try
            {
                SQL = "";
                SQL = " SELECT WARDCODE, JIKCODE, SCHEDULE";
                SQL = SQL + ComNum.VBLF + " FROM NUR_SCHEDULE1";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM ='" + strYYMM + "'";
                SQL = SQL + ComNum.VBLF + "   AND WARDCODE IN ('ER','SICU','MICU','2W','DR','3A','3B','3C','4A','5W','6W','7W','8W','HU')";//     '~j
                SQL = SQL + ComNum.VBLF + " ORDER BY WARDCODE, JIKCODE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i]["WARDCODE"].ToString().Trim())
                    {
                        case "ER":
                            nDept = 1;
                            break;
                        case "SICU":
                            nDept = 2;
                            break;
                        case "MICU":
                            nDept = 3;
                            break;
                        case "2W":
                        case "NP":
                            nDept = 4;
                            break;
                        case "DR":
                            nDept = 5;
                            break;
                        case "3A":
                            nDept = 6;
                            break;
                        case "3B":
                            nDept = 7;
                            break;
                        case "3C":
                            nDept = 8;
                            break;
                        case "4W":
                        case "4A":
                            nDept = 9;
                            break;
                        case "5W":
                            nDept = 10;
                            break;
                        case "6W":
                            nDept = 11;
                            break;
                        case "7W":
                            nDept = 12;
                            break;
                        case "8W":
                            nDept = 13;
                            break;
                        case "HU":
                            nDept = 14;
                            break;
                    }

                    strCODE = VB.Mid(dt.Rows[i]["Schedule"].ToString().Trim(), (nCount * 4) - 3, 4);
                    strJik = dt.Rows[i]["JikCode"].ToString().Trim();

                    if (VB.Len(strCODE) != 0)
                    {
                        for (J = 1; J <= nColCount; J++)
                        {
                            if (nDATA[1, 1, J] == strCODE)
                            {
                                if (strJik == "04" || strJik == "31" || strJik == "32" || strJik == "33" || strJik == "34" || strJik == "38" || strJik == "61" || strJik == "64")
                                {
                                    nDATA[2, nDept, J] = (VB.Val(nDATA[2, nDept, J]) + 1).ToString();  //'간호사
                                    nTotal[1, nDept] = nTotal[1, nDept] + 1;
                                    nTotal[3, J] = nTotal[3, J] + 1;
                                }
                                else
                                {
                                    nDATA[3, nDept, J] = (VB.Val(nDATA[3, nDept, J]) + 1).ToString();  //'간호조무사
                                    nTotal[2, nDept] = nTotal[2, nDept] + 1;
                                    nTotal[4, J] = nTotal[4, J] + 1;
                                }
                                break;
                            }
                        }
                    }
                }

                //DISPLAY

                for (i = 1; i < 14; i++)
                {
                    for (J = 1; J <= nColCount; J++)
                    {
                        SS1_Sheet1.Cells[(i * 2) - 1, (J + 2)].Text = nDATA[2, i, J].ToString();
                        SS1_Sheet1.Cells[(i * 2), (J + 2)].Text = nDATA[3, i, J].ToString();
                    }

                    SS1_Sheet1.Cells[(i * 2) - 1, (nColCount + 3) - 1].Text = nTotal[1, i].ToString();
                    SS1_Sheet1.Cells[(i * 2), (nColCount + 3) - 1].Text = nTotal[2, i].ToString();
                }
                nTotal1 = 0;
                nTotal2 = 0;

                for (i = 1; i <= nColCount; i++)
                {
                    SS1_Sheet1.Cells[29, nColCount + 2].Text = nTotal1.ToString();
                    SS1_Sheet1.Cells[30, nColCount + 2].Text = nTotal2.ToString();
                }

                dt.Dispose();
                dt = null;

                btnPrint.Enabled = true;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void frmNurseJobPrint_Load(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            dtpdate.Value = Convert.ToDateTime(strDTP);

            try
            {
                SQL = "";
                SQL = " SELECT CODE,NAME FROM NUR_CODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN= '4'";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                SS1_Sheet1.ColumnCount = dt.Rows.Count + 3;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS1_Sheet1.Cells[i, (i + 3) - 1].Text = dt.Rows[i]["code"].ToString().Trim();
                    nDATA[1, 1, (i + 1)] = dt.Rows[i]["Code"].ToString().Trim();
                }

                SS1_Sheet1.Cells[0, nColCount + 2].Text = "합 계";

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}
