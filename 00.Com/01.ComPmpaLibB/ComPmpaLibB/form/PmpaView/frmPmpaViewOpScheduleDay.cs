using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewOpScheduleDay.cs
    /// Description     : 수술실 현황
    /// Author          : 박창욱
    /// Create Date     : 2017-09-06
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iviewa\ipdsim091.frm(FrmOpScheduleDay.frm) >> frmPmpaViewOpScheduleDay.cs 폼이름 재정의" />	
    public partial class frmPmpaViewOpScheduleDay : Form
    {

        string GstrCheckList = "";

  
        public frmPmpaViewOpScheduleDay()
        {
            InitializeComponent();
        }

        #region //MainFormMessage
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion

        public frmPmpaViewOpScheduleDay(MainFormMessage pform)
        {
            mCallForm = pform;
            InitializeComponent();
            setEvent();
        }


        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void setEvent()
        {
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPmpaViewOpScheduleDay_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strFont1 = "";
            string strHead1 = "";
            string JobDate = "";
            string PrintDate = "";
            string JobMan = "";

            PrintDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");
            JobDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            JobMan = clsType.User.JobMan;

            //Print Head
            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strHead1 = "/f1" + VB.Space(30) + "(" + JobDate + ")  수  술  실   현  황" + "/n";

            //Print Body
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1;
            ssView_Sheet1.PrintInfo.Margin.Left = 200;
            ssView_Sheet1.PrintInfo.Margin.Right = 5;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strDate = "";
            int nRead = 0;

            strDate = dtpFDate.Value.ToString("yyyy-MM-dd");

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            try
            {
                SQL = "";
                SQL = " SELECT A.PANO,A.SNAME, B.BI ,A.WARDCODE, A.ROOMCODE, A.DEPTCODE,A.AGE,A.SEX,A.OPTIMEFROM,A.OPTIMETO ";
                SQL = SQL + " ,A.DIAGNOSIS,A.OPTITLE,A.OPDOCT1,A.ANGBN, A.ANDOCT1,";
                SQL = SQL + "  B.WARDCODE IWARDCODE, B.ROOMCODE IROOMCODE ";
                SQL = SQL + " FROM " + ComNum.DB_PMPA + "ORAN_MASTER A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
                SQL = SQL + " WHERE 1 = 1";
                SQL = SQL + "   AND a.OPDATE =TO_DATE('" + strDate + "','YYYY-MM-DD')";
                if (string.Compare(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), strDate) > 0)
                {
                    SQL = SQL + "   AND A.OPBUN IN ('1','2','3','4') ";
                }
                SQL = SQL + "   AND A.PANO = B.PANO(+)";
                SQL = SQL + "   AND B.ACTDATE IS NULL ";
                SQL = SQL + " ORDER BY A.DeptCode,A.OpTimeFrom ";

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
                    ComFunc.MsgBox("해당일자에 수술예약자가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = nRead;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRead; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["IWardcode"].ToString().Trim() + "/" + dt.Rows[i]["IROOMCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["BI"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["age"].ToString().Trim() + "/" + dt.Rows[i]["sex"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["deptcode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = VB.Val(dt.Rows[i]["optimefrom"].ToString().Trim() + dt.Rows[i]["Optimeto"].ToString().Trim()).ToString("00:00-00:00");
                    ssView_Sheet1.Cells[i, 7].Text = " " + dt.Rows[i]["diagnosis"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = " " + dt.Rows[i]["optitle"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["angbn"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["Opdoct1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["andoct1"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            if (e.Column == 1)
            {
                GstrCheckList = ssView_Sheet1.Cells[e.Row, 1].Text; //등록번호
                GstrCheckList += ssView_Sheet1.Cells[e.Row, 2].Text; //환자종류
                //FrmEntryMain.Show     //TODO : 연결 필요
            }
        }
    }
}
