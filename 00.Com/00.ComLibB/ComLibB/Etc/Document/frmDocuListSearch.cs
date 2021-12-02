using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmDocuListSearch : Form, MainFormMessage
    {
        #region //MainFormMessage
        public string mPara1 = "";
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
            mPara1 = strPara;
        }
        #endregion //MainFormMessage

        public frmDocuListSearch()
        {
            InitializeComponent();
        }


        public frmDocuListSearch(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmDocuListSearch(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }


        private void frmDocuListSearch_Load(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strBuCode = string.Empty;
            string strBuCodeS = string.Empty;

            SsView.ActiveSheet.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            SsView.ActiveSheet.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;

            ComFunc CF = new ComFunc();

            DtpToDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            DtpFrDate.Value = Convert.ToDateTime(DtpToDate.Value.ToString("yyyy-01-01"));

            CboBuseName.Items.Clear();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT BUSE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_MST";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + VB.Val(clsType.User.Sabun).ToString("00000") + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strBuCode = dt.Rows[0]["BUSE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (clsVbfunc.BAS_BUSE_TREE_UP(clsDB.DbCon, strBuCode) == "044510" || clsVbfunc.BAS_BUSE_TREE_UP(clsDB.DbCon, strBuCode) == "044520")
            {
                strBuCode = "044501";
            }

            strBuCodeS = clsVbfunc.READ_BAS_BUSE(clsDB.DbCon, clsVbfunc.BAS_BUSE_TREE_UP(clsDB.DbCon, strBuCode));
            strBuCodeS = string.Concat(strBuCodeS, VB.Space(50), ".", clsVbfunc.BAS_BUSE_TREE_UP(clsDB.DbCon, strBuCode));

            CboBuseName.Items.Add(strBuCodeS);
            CboBuseName.SelectedIndex = 0;

            if (VB.Right(CboBuseName.Text, 6) == "055201")
            {
                CboBuseName.Items.Add(string.Concat("병리과", VB.Space(50), ".", "055201"));
            }
            else if (VB.Right(CboBuseName.Text, 6) == "077201")
            {
                CboBuseName.Items.Add(string.Concat("어린이집", VB.Space(50), ".", "101730"));
            }

            if (clsType.User.Sabun == "30224")
            {
                CboBuseName.Items.Clear();
                CboBuseName.Items.Add(string.Concat("사회봉사팀", VB.Space(50), ".", "101770"));
                CboBuseName.SelectedIndex = 0;
            }

            CboBuseName.SelectedIndex = 0;
            GetData();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수
            string strBuseName = string.Empty;

            ComFunc CF = new ComFunc();

            strBuseName = VB.Right(CboBuseName.Text, 6);

            SsView_Sheet1.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT SEQNO, WORKDAY, DOCUNO, PLACENAME, DOCUNAME, BUSE";

                if (OptSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU";
                    SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '0'";
                }
                else if (OptSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.INSA_DOCU1";
                    SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '1'";
                }

                SQL = SQL + ComNum.VBLF + "   AND WORKDAY BETWEEN TO_DATE('" + DtpFrDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') AND TO_DATE('" + DtpToDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "   AND BUSE IN (" + clsVbfunc.BAS_BUSE_TREE(clsDB.DbCon, strBuseName) + ")";
                SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
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

                SsView_Sheet1.RowCount = dt.Rows.Count;
                SsView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SsView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                    SsView_Sheet1.Cells[i, 1].Text = Convert.ToDateTime(dt.Rows[i]["WORKDAY"].ToString().Trim()).ToString("yyyy-MM-dd");
                    SsView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DOCUNO"].ToString().Trim();
                    SsView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PLACENAME"].ToString().Trim();
                    SsView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DOCUNAME"].ToString().Trim();
                    SsView_Sheet1.Cells[i, 5].Text = CF.Read_BuseName(clsDB.DbCon, dt.Rows[i]["BUSE"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";
            string strSort = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            if (OptSort0.Checked == true)
            {
                strSort = "접수공문 리스트";
            }
            else if (OptSort1.Checked == true)
            {
                strSort = "발송공문 리스트";
            }

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C" + strSort + "/n/n/n/n";
            strHead2 = "/l/f2" + "출력일자 : " + clsPublic.GstrSysDate;

            SsView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;  //가로
            SsView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            SsView_Sheet1.PrintInfo.Margin.Left = 20;
            SsView_Sheet1.PrintInfo.Margin.Right = 0;
            SsView_Sheet1.PrintInfo.Margin.Top = 35;
            SsView_Sheet1.PrintInfo.Margin.Bottom = 30;
            SsView_Sheet1.PrintInfo.ZoomFactor = 0.9F;
            SsView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            SsView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            SsView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            SsView_Sheet1.PrintInfo.ShowBorder = true;
            SsView_Sheet1.PrintInfo.ShowColor = false;
            SsView_Sheet1.PrintInfo.ShowGrid = true;
            SsView_Sheet1.PrintInfo.ShowShadows = false;
            SsView_Sheet1.PrintInfo.UseMax = false;
            SsView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            SsView.PrintSheet(0);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDocuListSearch_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmDocuListSearch_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }
    }
}
