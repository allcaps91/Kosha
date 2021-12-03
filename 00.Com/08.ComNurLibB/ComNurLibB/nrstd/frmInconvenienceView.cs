using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmInconvenienceView.cs
    /// Description     : 불편발생리스트관리
    /// Author          : 박창욱
    /// Create Date     : 2018-01-31
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm불편상황리스트.frm(Frm불편상황리스트.frm) >> frmInconvenienceView.cs 폼이름 재정의" />	
    public partial class frmInconvenienceView : Form
    {
        string GsWard = "";

        public frmInconvenienceView()
        {
            InitializeComponent();
        }

        public frmInconvenienceView(string sWard)
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

            strTitle = "불편상황리스트";

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

            string strToDate = "";
            string strNextDate = "";
            string strWARD = "";

            ssView_Sheet1.RowCount = 0;

            strToDate = dtpDate.Value.ToString("yyyy-MM-dd");
            strNextDate = dtpDate1.Value.AddDays(1).ToString("yyyy-MM-dd");

            strWARD = cboWard.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(BDate,'YYYY-MM-DD') BDate,TO_CHAR(JEPDATE,'YYYY-MM-DD') JepDate, ROWID, ";
                SQL = SQL + ComNum.VBLF + " SName,Age,Sex,WardCode,RoomCode,Sabun,JikJong ,DtlBun ";
                SQL = SQL + ComNum.VBLF + " From " + ComNum.DB_PMPA + "NUR_STD_INCONV ";
                SQL = SQL + ComNum.VBLF + " Where BDate >= TO_DATE('" + strToDate + "','YYYY-MM-DD')   ";
                SQL = SQL + ComNum.VBLF + "  AND  BDate <= TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                if (strWARD != "전체" && strWARD != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND WardCode = '" + strWARD + "' ";
                }

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
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["JepDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["WardCode"].ToString().Trim() + " ( " + dt.Rows[i]["RoomCode"].ToString().Trim() + " 호 )";
                    ssView_Sheet1.Cells[i, 5].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["Sabun"].ToString().Trim());
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["JikJong"].ToString().Trim();

                    switch (dt.Rows[i]["DtlBun"].ToString().Trim())
                    {
                        case "1":
                            ssView_Sheet1.Cells[i, 7].Text = "Hard Ware";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[i, 7].Text = "System Ware";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[i, 7].Text = "Human Ware";
                            break;
                    }

                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
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

        private void frmInconvenienceView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (GsWard == "")
            {
                GsWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");
            }

            Set_cboWard();

            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-10);
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

                dt.Dispose();
                dt = null;

                cboWard.SelectedIndex = -1;

                cboWard.SelectedIndex = cboWard.Items.IndexOf(GsWard);
                //cboWard.Enabled = false;

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
