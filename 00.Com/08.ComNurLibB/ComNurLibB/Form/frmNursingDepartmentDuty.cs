using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedIpdNr
    /// File Name       : frmNursingDepartmentDuty.cs
    /// Description     : 인공신장실 당직 스케쥴 조회 및 인쇄
    /// Author          : 이현종
    /// Create Date     : 2018-05-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrward\Frm간호부당직표.frm(Frm간호부당직표.frm) >> frmNursingDepartmentDuty.cs 폼이름 재정의" />
    public partial class frmNursingDepartmentDuty : Form
    {
        public frmNursingDepartmentDuty()
        {
            InitializeComponent();
        }

        private void frmNursingDepartmentDuty_Load(object sender, EventArgs e)
        {

            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ComFunc.ReadSysDate(clsDB.DbCon);
            DateTime DT = Convert.ToDateTime(clsPublic.GstrSysDate).AddMonths(+3);

            cboYYMM.Items.Clear();
            for (int i = 1; i < 25; i++)
            {
                cboYYMM.Items.Add(DT.Year + "-" + ComFunc.SetAutoZero(DT.Month.ToString(), 2) + "월");
                DT = DT.AddMonths(-1);
            }

            cboBuse.Items.Clear();
            cboBuse.Items.Add("1.인공신장실");
            cboBuse.SelectedIndex = 0;

            cboYYMM.SelectedIndex = 2;

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            Read_Data(cboYYMM.Text, "HD");
        }

        void CLEAR_SCREEN()
        {
            int i = 0;
            for (i = 1; i < 27; i = i + 5)
            {
                ss1_Sheet1.Cells[i, 6].ForeColor = System.Drawing.Color.FromArgb(0, 0, 0);
                ss1_Sheet1.Cells[i, 6].BackColor = System.Drawing.Color.FromArgb(217, 232, 255);
            }

            ss1_Sheet1.Cells[26, 0, 27, 6].Text = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            Read_Data(cboYYMM.Text, "HD");
        }

        void YYMM_DISPLAYER()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            DateTime DT = Convert.ToDateTime(VB.Left(cboYYMM.Text, 7) + "-01");
            string strDate = DT.ToShortDateString();
            int nDD = DateTime.DaysInMonth(DT.Year, DT.Month);

            int j = 1;
            int nCol = 0;
            string strWeek = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                for (int i = 1; i < nDD + 1; i++)
                {
                    SQL = "Select TO_CHAR(TO_DATE('" + VB.Left(strDate, 8) + ComFunc.SetAutoZero(i.ToString(), 2) + "', 'YYYY-MM-DD'),'DY') cWeek ";
                    SQL += ComNum.VBLF + " FROM DUAL ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strWeek = dt.Rows[0]["cWeek"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    switch (strWeek)
                    {
                        case "MON":
                        case "월":
                            nCol = 0;          //  '월
                            break;
                        case "TUE":
                        case "화":
                            nCol = 1;          // '화
                            break;
                        case "WED":
                        case "수":
                            nCol = 2;          // '수
                            break;
                        case "THU":
                        case "목":
                            nCol = 3;          // '목
                            break;
                        case "FRI":
                        case "금":
                            nCol = 4;          //  '금"
                            break;
                        case "SAT":
                        case "토":
                            nCol = 5;          // '토
                            break;
                        case "SUN":
                        case "일":
                            nCol = 6;          // '일
                            break;
                    }

                    ss1_Sheet1.Cells[j, nCol].Text = i.ToString();
                    if (nCol == 6) j = j + 5;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void Read_Data(string arg, string arg2)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;
            int j = 0;
            int k = 0;

            ss1_Sheet1.Cells[0, 0, ss1_Sheet1.RowCount - 1, 6].Text = "";
            CLEAR_SCREEN();
            YYMM_DISPLAYER();

            string strSDATE = VB.Left(arg, 7) + "-01";
            DateTime DT = Convert.ToDateTime(strSDATE);
            string strEDATE = VB.Left(arg, 7) + "-" + DateTime.DaysInMonth(DT.Year, DT.Month);

            Cursor.Current = Cursors.WaitCursor;
            try
            {

                SQL = " SELECT ACTDATE, B.KORNAME";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_SCHEDULE2 A, KOSMOS_ADM.INSA_MST B";
                SQL += ComNum.VBLF + " WHERE ACTDATE >= TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + strEDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND A.HD = B.SABUN";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
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
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    for(j = 0; j < 7; j++)
                    {
                        for(k = 1; k < 28; k = k + 5)
                        {
                            if (ComFunc.SetAutoZero(ss1_Sheet1.Cells[k, j].Text, 2) == VB.Right(ComFunc.FormatStrToDateTime(dt.Rows[i]["ACTDATE"].ToString().Trim(), "D"), 2)) 
                            {
                                ss1_Sheet1.Cells[k + 1, j].Text = dt.Rows[i]["KORNAME"].ToString().Trim();
                                ss1_Sheet1.Cells[k + 2, j].Text = " ";
                                ss1_Sheet1.Cells[k + 3, j].Text = " ";
                                ss1_Sheet1.Cells[k + 4, j].Text = " ";
                                
                            }
                        }
                    }
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            SET_PRINT();
        }

        void SET_PRINT()
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

            strTitle = cboYYMM.Text + " " + VB.Mid(cboBuse.Text, 3, cboBuse.Text.Length);
 
            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(0, 0, 50, 20, 0, 0);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, false, true, false, true, false);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboYYMM_SelectedIndexChanged(object sender, EventArgs e)
        {
            Read_Data(cboYYMM.Text, "HD");
        }
    }
}
