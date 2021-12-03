using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaTongMonthJewonMisu2.cs
    /// Description     : 월말현재 재원미수금 상세내역
    /// Author          : 박창욱
    /// Create Date     : 2017-08-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs22.frm(FrmMonthJewonMisu.frm) >> frmPmpaTongMonthJewonMisu2.cs 폼이름 재정의" />	
    public partial class frmPmpaTongMonthJewonMisu2 : Form
    {
        double[,] FnAmt = new double[4, 9];

        public frmPmpaTongMonthJewonMisu2()
        {
            InitializeComponent();
        }

        void ScreenClear()
        {
            btnSearch.Enabled = true;
            cboYYMM.Enabled = true;
            cboJong.Enabled = true;

            //Sheet Clear
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 30;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ScreenClear();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            ssView_Sheet1.Columns[11].Visible = false;  //병동
            ssView_Sheet1.Columns[14].Visible = false;  //의사
            ssView_Sheet1.Columns[16].Visible = false;  //재원일수

            Cursor.Current = Cursors.WaitCursor;

            //Print Head
            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fs2";
            strHead1 = "/f1" + VB.Space(30);
            strHead1 = strHead1 + "월말현재 재원미수금 상세내역";
            strHead2 = "/f2/n" + "작업월: " + cboYYMM.Text;
            strHead2 = strHead2 + VB.Space(10) + "인쇄일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon,"A"), "A");
            ;

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 50;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 30;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView.PrintSheet(0);

            ssView_Sheet1.Columns[11].Visible = true;  //병동
            ssView_Sheet1.Columns[14].Visible = true;  //의사
            ssView_Sheet1.Columns[16].Visible = true;  //재원일수
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
            int nRow = 0;
            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";
            string strJong = "";
            string strOldData = "";
            string strNewData = "";

            btnSearch.Enabled = false;
            cboYYMM.Enabled = false;
            cboJong.Enabled = false;
            ssView.Enabled = true;

            //Sheet Clear
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 30;

            strYYMM = ComFunc.LeftH(cboYYMM.Text, 4) + ComFunc.MidH(cboYYMM.Text, 6, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strYYMM, 4)), (int)VB.Val(VB.Right(strYYMM, 2)));
            strJong = ComFunc.LeftH(cboJong.Text, 1);

            //누적할 배열을 Clear
            for (i = 1; i < 4; i++)
            {
                for (k = 1; k < 9; k++)
                {
                    FnAmt[i, k] = 0;
                }
            }

            //자료 조회
            try
            {
                SQL = "";
                SQL = "SELECT SuBi,Bi,Pano,SName,TotAmt,JohapAmt,JungAmt,IpgumAmt,JohapMisu,BoninAmt,BoninMisu,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(InDate,'YYYY-MM-DD') InDate,Ilsu,WardCode,RoomCode,";
                SQL = SQL + ComNum.VBLF + " DeptCode,DrCode ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALJEWON ";
                SQL = SQL + ComNum.VBLF + "WHERE YYMM='" + strYYMM + "' ";

                //允(2005-11-01) 루틴변경
                if (string.Compare(strYYMM, "200508") < 0)
                {
                    SQL = SQL + ComNum.VBLF + "  AND (TotAmt <> 0 OR IPGUMAMT <>'0')  ";
                }
                if (strJong != "0")
                {
                    SQL = SQL + ComNum.VBLF + " AND SuBi='" + strJong + "' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY SuBi,Bi,Pano ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    btnSearch.Enabled = true;
                    cboYYMM.Enabled = true;
                    cboJong.Enabled = true;
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                nRow = 0;
                strOldData = "";

                for (i = 0; i < nRead; i++)
                {
                    strNewData = dt.Rows[i]["SuBi"].ToString().Trim();
                    if (strOldData != strNewData)
                    {
                        SearchSubTotal(ref nRow);
                        strOldData = strNewData;
                    }

                    //금액을 누적
                    FnAmt[1, 1] = VB.Val(dt.Rows[i]["TotAmt"].ToString().Trim());
                    FnAmt[1, 2] = VB.Val(dt.Rows[i]["JohapAmt"].ToString().Trim());
                    FnAmt[1, 3] = VB.Val(dt.Rows[i]["BoninAmt"].ToString().Trim());
                    FnAmt[1, 4] = VB.Val(dt.Rows[i]["JungAmt"].ToString().Trim());
                    FnAmt[1, 5] = VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                    FnAmt[1, 6] = VB.Val(dt.Rows[i]["JohapMisu"].ToString().Trim());
                    FnAmt[1, 7] = VB.Val(dt.Rows[i]["BoninMisu"].ToString().Trim());
                    FnAmt[1, 8] = FnAmt[1, 6] + FnAmt[1, 7];

                    //소계에 누적
                    FnAmt[2, 1] += VB.Val(dt.Rows[i]["TotAmt"].ToString().Trim());
                    FnAmt[2, 2] += VB.Val(dt.Rows[i]["JohapAmt"].ToString().Trim());
                    FnAmt[2, 3] += VB.Val(dt.Rows[i]["BoninAmt"].ToString().Trim());
                    FnAmt[2, 4] += VB.Val(dt.Rows[i]["JungAmt"].ToString().Trim());
                    FnAmt[2, 5] += VB.Val(dt.Rows[i]["IpgumAmt"].ToString().Trim());
                    FnAmt[2, 6] += VB.Val(dt.Rows[i]["JohapMisu"].ToString().Trim());
                    FnAmt[2, 7] += VB.Val(dt.Rows[i]["BoninMisu"].ToString().Trim());
                    FnAmt[2, 8] = FnAmt[2, 6] + FnAmt[2, 7];

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SName"].ToString().Trim();

                    for (k = 1; k < 9; k++)
                    {
                        ssView_Sheet1.Cells[nRow - 1, k + 2].Text = FnAmt[1, k].ToString("###,###,###,##0");
                    }

                    ssView_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 16].Text = dt.Rows[i]["Ilsu"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                SearchSubTotal(ref nRow);

                //합계
                nRow += 1;
                if (nRow > ssView_Sheet1.RowCount)
                {
                    ssView_Sheet1.RowCount = nRow;
                }
                ssView_Sheet1.Cells[nRow - 1, 2].Text = "**합계**";
                for (k = 1; k < 9; k++)
                {
                    ssView_Sheet1.Cells[nRow - 1, k + 2].Text = FnAmt[3, k].ToString("###,###,###,##0");
                }

                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                ssView.Enabled = true;
                btnSearch.Enabled = true;
                cboYYMM.Enabled = true;
                cboJong.Enabled = true;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                cboYYMM.Enabled = true;
                cboJong.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        //소계
        void SearchSubTotal(ref int nRow)
        {
            int i = 0;

            if (FnAmt[2, 1] == 0)
            {
                return;
            }

            nRow += 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }
            ssView_Sheet1.Cells[nRow - 1, 2].Text = "**소계**";
            for (i = 1; i < 9; i++)
            {
                ssView_Sheet1.Cells[nRow - 1, i + 2].Text = FnAmt[2, i].ToString("###,###,###,##0");
                FnAmt[3, i] += FnAmt[2, i];
                FnAmt[2, i] = 0;
            }

            return;
        }

        private void frmPmpaTongMonthJewonMisu2_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();   //폼 권한 조회
            //    return;
            //}
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");   //폼 기본값 세팅 등

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

            cboJong.Items.Clear();
            cboJong.Items.Add("0.전체");
            cboJong.Items.Add("1.건강보험");
            cboJong.Items.Add("2.의료급여");
            cboJong.Items.Add("3.산재");
            cboJong.Items.Add("4.자보");
            cboJong.Items.Add("5.일반");
            cboJong.SelectedIndex = 0;

            ScreenClear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
