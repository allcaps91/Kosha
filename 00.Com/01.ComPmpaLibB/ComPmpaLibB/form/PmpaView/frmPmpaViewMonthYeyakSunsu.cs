using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMonthYeyakSunsu.cs
    /// Description     : 월말현재 예약선수금 상세내역
    /// Author          : 박창욱
    /// Create Date     : 2017-08-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\misu\misubs\misubs25.frm(FrmMonthYeyakSunsu.frm) >> frmPmpaViewMonthYeyakSunsu.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMonthYeyakSunsu : Form
    {
        double[] FnAmt = new double[4];

        public frmPmpaViewMonthYeyakSunsu()
        {
            InitializeComponent();
        }

        void ScreenClear()
        {
            btnSearch.Enabled = true;
            cboYYMM.Enabled = true;
            cboJong.Enabled = true;

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
            {
                return; //권한 확인
            }

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            Cursor.Current = Cursors.WaitCursor;

            //Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fs2";
            strHead1 = "/f1" + VB.Space(15);
            if (rdoGubun0.Checked == true)
            {
                strHead1 += "월말현재 예약선수금 상세내역(진찰)";
            }
            else if (rdoGubun1.Checked == true)
            {
                strHead1 += "월말현재 예약선수금 상세내역(검사)";
            }

            strHead2 = "/f2/n" + "작업월: " + cboYYMM.Text;
            strHead2 += VB.Space(10) + "인쇄일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");


            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 50;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 30;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);

            Cursor.Current = Cursors.Default;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int nRead = 0;
            int nRow = 0;
            string strYYMM = "";
            string strFDate = "";
            string strTDate = "";
            string strJong = "";
            string strOldData = "";
            string strNewData = "";

            ssView_Sheet1.RowCount = 0;

            btnSearch.Enabled = false;
            cboYYMM.Enabled = false;
            cboJong.Enabled = false;
            ssView.Enabled = true;

            Cursor.Current = Cursors.WaitCursor;

            ssView_Sheet1.RowCount = 30;

            strYYMM = ComFunc.LeftH(cboYYMM.Text, 4) + ComFunc.MidH(cboYYMM.Text, 6, 2);
            strFDate = VB.Left(strYYMM, 4) + "-" + VB.Right(strYYMM, 2) + "-01";
            strTDate = clsVbfunc.LastDay((int)VB.Val(VB.Left(strYYMM, 4)), (int)VB.Val(VB.Right(strYYMM, 2)));
            strJong = ComFunc.LeftH(cboJong.Text, 1);

            //누적할 배열을 Clear
            for (i = 1; i < 4; i++)
            {
                FnAmt[i] = 0;
            }


            //자료조회
            try
            {
                SQL = "";
                SQL = "SELECT SuBi,Bi,Pano,SName,DeptCode,DrCode,Amt7,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(Date3,'YYYY-MM-DD HH24:MI') Date3,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(Date1,'YYYY-MM-DD') Date1 ";
                if (rdoGubun0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALYEYAK ";
                }
                else if (rdoGubun1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "MISU_BALYEYAK_EXAM ";
                }
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "  AND YYMM='" + strYYMM + "' ";
                SQL = SQL + ComNum.VBLF + "  AND Amt7 <> 0 ";
                if (strJong != "0")
                {
                    SQL = SQL + ComNum.VBLF + " AND SuBi='" + strJong + "' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY SuBi,Bi,Date3,Pano ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);   //에러로그 저장
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
                    FnAmt[1] = VB.Val(dt.Rows[i]["Amt7"].ToString().Trim());
                    //소계에 누적
                    FnAmt[2] += VB.Val(dt.Rows[i]["Amt7"].ToString().Trim());

                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Left(dt.Rows[i]["Date3"].ToString().Trim(), 10);
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = VB.Right(dt.Rows[i]["Date3"].ToString().Trim(), 5);
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = FnAmt[1].ToString("###,###,###,##0 ");
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["Date1"].ToString().Trim();

                    //2013-03-12
                    if (VB.Left(dt.Rows[i]["Date3"].ToString().Trim(), 10) != "" && Convert.ToDateTime(VB.Left(dt.Rows[i]["Date3"].ToString().Trim(), 10)) <= Convert.ToDateTime(strTDate))
                    {
                        ssView_Sheet1.Rows[nRow - 1].BackColor = Color.FromArgb(255, 255, 0);
                    }
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
                ssView_Sheet1.Cells[nRow - 1, 7].Text = FnAmt[3].ToString("###,###,###,##0 ");


                ssView_Sheet1.RowCount = nRow;
                ssView.Enabled = true;
                btnSearch.Enabled = true;
                cboYYMM.Enabled = true;
                cboJong.Enabled = true;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                cboYYMM.Enabled = true;
                cboJong.Enabled = true;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);   //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void SearchSubTotal(ref int nRow)
        {
            if (FnAmt[2] == 0)
            {
                return;
            }

            nRow += 1;
            if (nRow > ssView_Sheet1.RowCount)
            {
                ssView_Sheet1.RowCount = nRow;
            }
            ssView_Sheet1.Cells[nRow - 1, 2].Text = "**소계**";
            ssView_Sheet1.Cells[nRow - 1, 7].Text = FnAmt[2].ToString("###,###,###,##0");

            FnAmt[3] += FnAmt[2];
            FnAmt[2] = 0;

            return;
        }

        private void frmPmpaViewMonthYeyakSunsu_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

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
