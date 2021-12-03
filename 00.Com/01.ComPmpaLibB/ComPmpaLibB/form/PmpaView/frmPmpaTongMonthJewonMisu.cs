using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaTongMonthJewonMisu.cs
    /// Description     : 월말현재 재원미수금 총괄표
    /// Author          : 박창욱
    /// Create Date     : 2017-08-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs23.frm(FrmTongMonthJewonMisu.frm) >> frmPmpaTongMonthJewonMisu.cs 폼이름 재정의" />	
    public partial class frmPmpaTongMonthJewonMisu : Form
    {
        double[,] FnAmt = new double[10, 11]; 

        public frmPmpaTongMonthJewonMisu()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            Cursor.Current = Cursors.WaitCursor;

            //Print Head
            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fs2";
            strHead1 = "/f1" + VB.Space(30);
            strHead1 = strHead1 + "월말현재 재원미수금 총괄표";
            strHead2 = "/f2" + "작업월: " + cboYYMM.Text;
            strHead2 = strHead2 + VB.Space(10) + "인쇄일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");


            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 50;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 30;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 100;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int k = 0;
            int nRead = 0;
            string strYYMM = "";

            ssView.Enabled = true;

            //Sheet Clear
            ssView_Sheet1.Cells[0, 1, 8, ssView_Sheet1.ColumnCount - 1].Text = "";

            strYYMM = ComFunc.LeftH(cboYYMM.Text, 4) + ComFunc.MidH(cboYYMM.Text, 6, 2);

            //누적할 배열을 Clear
            for (i = 1; i < 10; i++)
            {
                for (k = 1; k < 11; k++)
                {
                    FnAmt[i, k] = 0;
                }
            }

            //자료조회
            try
            {
                SQL = "";
                SQL = "SELECT SuBi,COUNT(*) CNT,SUM(TotAmt) TotAmt,SUM(JohapAmt) JohapAmt,";
                SQL = SQL + ComNum.VBLF + " SUM(JungAmt) JungAmt,SUM(IpgumAmt) IpgumAmt,";
                SQL = SQL + ComNum.VBLF + " SUM(JohapMisu) JohapMisu,SUM(BoninAmt) BoninAmt,";
                SQL = SQL + ComNum.VBLF + " SUM(BoninMisu) BoninMisu ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALJEWON ";
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "  AND YYMM='" + strYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY SuBi ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SuBi ";

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
                    return;
                }

                nRead = dt.Rows.Count;

                for (i = 0; i < nRead; i++)
                {
                    switch (dt.Rows[i]["SuBi"].ToString().Trim())
                    {
                        case "1":   //보험
                            k = 1;
                            break;
                        case "2":   //보호
                            k = 2;
                            break;
                        case "3":   //산재
                            k = 4;
                            break;
                        case "4":   //자보
                            k = 5;
                            break;
                        case "5":   //일반
                            k = 7;
                            break;
                        default:    //기타는 보험으로
                            k = 1;
                            break;
                    }

                    //금액을 누적
                    FnAmt[k, 1] += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                    FnAmt[k, 2] += VB.Val(dt.Rows[i]["TotAmt"].ToString().Trim());
                    FnAmt[k, 3] += VB.Val(dt.Rows[i]["JohapAmt"].ToString().Trim());
                    FnAmt[k, 4] += VB.Val(dt.Rows[i]["BoninAmt"].ToString().Trim());
                    FnAmt[k, 5] += VB.Val(dt.Rows[i]["JungAmt"].ToString().Trim());
                    FnAmt[k, 6] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                    FnAmt[k, 7] += VB.Val(dt.Rows[i]["JohapMisu"].ToString().Trim());
                    FnAmt[k, 8] += VB.Val(dt.Rows[i]["BoninMisu"].ToString().Trim());
                    FnAmt[k, 9] = FnAmt[k, 7] + FnAmt[k, 8];
                    FnAmt[k, 10] = FnAmt[k, 6] + FnAmt[k, 9];

                    //소계에 ADD
                    if (k < 3)
                    {
                        FnAmt[3, 1] += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                        FnAmt[3, 2] += VB.Val(dt.Rows[i]["TotAmt"].ToString().Trim());
                        FnAmt[3, 3] += VB.Val(dt.Rows[i]["JohapAmt"].ToString().Trim());
                        FnAmt[3, 4] += VB.Val(dt.Rows[i]["BoninAmt"].ToString().Trim());
                        FnAmt[3, 5] += VB.Val(dt.Rows[i]["JungAmt"].ToString().Trim());
                        FnAmt[3, 6] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                        FnAmt[3, 7] += VB.Val(dt.Rows[i]["JohapMisu"].ToString().Trim());
                        FnAmt[3, 8] += VB.Val(dt.Rows[i]["BoninMisu"].ToString().Trim());
                        FnAmt[3, 9] = FnAmt[3, 7] + FnAmt[3, 8];
                        FnAmt[3, 10] = FnAmt[3, 6] + FnAmt[3, 9];
                    }
                    else if (k == 7)
                    {
                        FnAmt[8, 1] += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                        FnAmt[8, 2] += VB.Val(dt.Rows[i]["TotAmt"].ToString().Trim());
                        FnAmt[8, 3] += VB.Val(dt.Rows[i]["JohapAmt"].ToString().Trim());
                        FnAmt[8, 4] += VB.Val(dt.Rows[i]["BoninAmt"].ToString().Trim());
                        FnAmt[8, 5] += VB.Val(dt.Rows[i]["JungAmt"].ToString().Trim());
                        FnAmt[8, 6] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                        FnAmt[8, 7] += VB.Val(dt.Rows[i]["JohapMisu"].ToString().Trim());
                        FnAmt[8, 8] += VB.Val(dt.Rows[i]["BoninMisu"].ToString().Trim());
                        FnAmt[8, 9] = FnAmt[8, 7] + FnAmt[8, 8];
                        FnAmt[8, 10] = FnAmt[8, 6] + FnAmt[8, 9];
                    }
                    else
                    {
                        FnAmt[6, 1] += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                        FnAmt[6, 2] += VB.Val(dt.Rows[i]["TotAmt"].ToString().Trim());
                        FnAmt[6, 3] += VB.Val(dt.Rows[i]["JohapAmt"].ToString().Trim());
                        FnAmt[6, 4] += VB.Val(dt.Rows[i]["BoninAmt"].ToString().Trim());
                        FnAmt[6, 5] += VB.Val(dt.Rows[i]["JungAmt"].ToString().Trim());
                        FnAmt[6, 6] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                        FnAmt[6, 7] += VB.Val(dt.Rows[i]["JohapMisu"].ToString().Trim());
                        FnAmt[6, 8] += VB.Val(dt.Rows[i]["BoninMisu"].ToString().Trim());
                        FnAmt[6, 9] = FnAmt[6, 7] + FnAmt[6, 8];
                        FnAmt[6, 10] = FnAmt[6, 6] + FnAmt[6, 9];
                    }

                    FnAmt[9, 1] += VB.Val(dt.Rows[i]["CNT"].ToString().Trim());
                    FnAmt[9, 2] += VB.Val(dt.Rows[i]["TotAmt"].ToString().Trim());
                    FnAmt[9, 3] += VB.Val(dt.Rows[i]["JohapAmt"].ToString().Trim());
                    FnAmt[9, 4] += VB.Val(dt.Rows[i]["BoninAmt"].ToString().Trim());
                    FnAmt[9, 5] += VB.Val(dt.Rows[i]["JungAmt"].ToString().Trim());
                    FnAmt[9, 6] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                    FnAmt[9, 7] += VB.Val(dt.Rows[i]["JohapMisu"].ToString().Trim());
                    FnAmt[9, 8] += VB.Val(dt.Rows[i]["BoninMisu"].ToString().Trim());
                    FnAmt[9, 9] = FnAmt[9, 7] + FnAmt[9, 8];
                    FnAmt[9, 10] = FnAmt[9, 6] + FnAmt[9, 9];
                }

                dt.Dispose();
                dt = null;

                //인원 및 금액을 Display
                for (i = 1; i < 10; i++)
                {
                    for (k = 1; k < 11; k++)
                    {
                        ssView_Sheet1.Cells[i - 1, k].Text = FnAmt[i, k].ToString("###,###,###,##0");
                    }
                }

                btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void frmPmpaTongMonthJewonMisu_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsVbfunc.SetCboDate(clsDB.DbCon, cboYYMM, 36, "", "0");
            cboYYMM.SelectedIndex = 0;
            //재무회계팀일 경우 기간 편집 되도록 보완(2021-09-03 김현욱)
            switch (clsType.User.JobGroup)
            {
                case "JOB021002":
                case "JOB021005":
                case "JOB021004":
                    cboYYMM.DropDownStyle = ComboBoxStyle.DropDown;
                    break;
            }

            btnPrint.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog mDlg = new SaveFileDialog())
            {
                mDlg.InitialDirectory = Application.StartupPath;
                mDlg.Filter = "Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                mDlg.FilterIndex = 1;
                if (mDlg.ShowDialog() == DialogResult.OK)
                {
                    ssView.SaveExcel(mDlg.FileName,
                    FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
                }
            }
        }
    }
}
