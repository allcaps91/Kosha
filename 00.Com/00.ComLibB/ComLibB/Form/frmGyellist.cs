using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;
using ComLibB;
using ComBase.Mvc.Utils;
using DevComponents.DotNetBar;
namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmGyellist
    /// Description     : 결핵관리 서식지 리스트 폼
    /// Author          : 김욱동
    /// Create Date     : 2021-09-29
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    public partial class frmGyellist : Form, MainFormMessage
    {
        string GstrSysDate = "";
        string GstrPANO = "";
        string GstrIllcode = "";
        string strTabTag = "0";
        #region MainFormMessage InterFace

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

        #endregion

        public frmGyellist(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }

        public frmGyellist()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }

        private void frmGyellist_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComFunc CF = new ComFunc();
            clsSpread CS = new clsSpread();

            GstrSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            dtpFDate.Value = Convert.ToDateTime(CF.DATE_ADD(clsDB.DbCon, GstrSysDate, -7));
            dtpTDate.Value = Convert.ToDateTime(GstrSysDate);

            CS.setSpdFilter(ssView, 1, AutoFilterMode.EnhancedContextMenu, true);
            CS.setSpdFilter(ssView, 2, AutoFilterMode.EnhancedContextMenu, true);
            CS.setSpdFilter(ssView, 3, AutoFilterMode.EnhancedContextMenu, true);
            CS.setSpdFilter(ssView, 4, AutoFilterMode.EnhancedContextMenu, true);
            CS.setSpdFilter(ssView, 5, AutoFilterMode.EnhancedContextMenu, true);
            CS.setSpdFilter(ssView, 6, AutoFilterMode.EnhancedContextMenu, true);
            CS.setSpdFilter(ssView, 7, AutoFilterMode.EnhancedContextMenu, true);
            CS.setSpdFilter(ssView, 8, AutoFilterMode.EnhancedContextMenu, true);
            CF = null;
            CS = null;

            //탭버튼클릭 이벤트
            this.tab1.Click += new EventHandler(eTabClick);
            this.tab2.Click += new EventHandler(eTabClick);
            this.tab3.Click += new EventHandler(eTabClick);
            this.tab4.Click += new EventHandler(eTabClick);

            GetData(strTabTag);
        }

        void frmInfect2_FormClosed(object sender, FormClosedEventArgs e)
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData(strTabTag);
            txtPano.Text = "";
            txtSName.Text = "";
        }
        void eTabClick(object sender, EventArgs e)
        {
            SuperTabItem ss = (SuperTabItem)sender;

            strTabTag = ss.Tag.ToString();

            //조회
            GetData(strTabTag);
        }
        private void GetData(string TabTag)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT A.ACTDATE, A.ENTDATE, A.PANO, A.SNAME, A.EDUCATOR, A.ILLCODE, A.GUNGU, A.M2_DISREG9,(SELECT SEX FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PANO) AS SEX, KOSMOS_OCS.FC_GET_AGE2(A.PANO,TRUNC(SYSDATE)) AS AGE, A.ROWID ";
                if(TabTag == "0")
                {
                    SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_PMPA + "NUR_STD_INFECT9 A";
                }
                else if(TabTag == "1")
                {
                    SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_PMPA + "NUR_STD_INFECT10 A";
                }
                else if (TabTag == "2")
                {
                    SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_PMPA + "NUR_STD_INFECT11 A";
                }
                else if (TabTag == "3")
                {
                    SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_PMPA + "NUR_STD_INFECT12 A";
                }
                SQL = SQL + ComNum.VBLF + "WHERE ACTDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND ACTDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "' ,'YYYY-MM-DD') ";
                if(string.IsNullOrEmpty(txtPano.Text.Trim()) == false )
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.PANO = '" + txtPano.Text.Trim() + "' ";
                }
                if (string.IsNullOrEmpty(txtSName.Text.Trim()) == false)
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.SNAME = '" + txtSName.Text.Trim() + "' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY ENTDATE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장 
                    return;
                }

                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = clsVbfunc.GetOCSDrDeptcodeSabun(clsDB.DbCon, dt.Rows[0]["EDUCATOR"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 6].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["EDUCATOR"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 7].Text = VB.Left(dt.Rows[i]["M2_DISREG9"].ToString().Trim(),1) == "*" ? "" : dt.Rows[i]["M2_DISREG9"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();

                    }
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

                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
        
        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }
            
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);//정렬
                return;
            }

            GstrPANO = ssView_Sheet1.Cells[e.Row, 2].Text;
            GstrIllcode = ssView_Sheet1.Cells[e.Row, 5].Text;

         
        }
 
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            ssView_Sheet1.Columns[0].Visible = false;

            strTitle = "결핵환자 관리목록 ";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("등록기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
            CS = null;

            ssView_Sheet1.Columns[0].Visible = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void DeleteData()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            string strROWID = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            if (ComFunc.MsgBoxQ("정말로 삭제하시겠습니까? 삭제한 보고서는 복구 불가능합니다.", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strROWID = ssView_Sheet1.Cells[i, 20].Text;

                    if (Convert.ToBoolean(ssView_Sheet1.Cells[i, 0].Value) == true)
                    {
                        SQL = "";
                        SQL = "DELETE " + ComNum.DB_PMPA + "NUR_STD_INFECT9  WHERE ROWID = '" + strROWID + "'";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");                
                Cursor.Current = Cursors.Default;
                GetData(strTabTag);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string READ_PatientName(string ArgPano)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT SNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO='" + ArgPano + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["SNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return strVal;
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
                return strVal;
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
     
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
          
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            //string strDate = "";

            GstrPANO = ssView_Sheet1.Cells[e.Row, 1].Text;
            GstrIllcode = ssView_Sheet1.Cells[e.Row, 9].Text;
            //strDate = ssView_Sheet1.Cells[e.Row, 1].Text.Trim();

            if (strTabTag == "0")
            {
                frmGyelEdu frmGyelEduX = new frmGyelEdu(GstrPANO, GstrIllcode);
                frmGyelEduX.StartPosition = FormStartPosition.CenterParent;
                frmGyelEduX.ShowDialog();
                frmGyelEduX = null;
                GetData(strTabTag);
            }
            if (strTabTag == "1")
            {
                frmGyelEdu2 frmGyelEdu2X = new frmGyelEdu2(GstrPANO, GstrIllcode);
                frmGyelEdu2X.StartPosition = FormStartPosition.CenterParent;
                frmGyelEdu2X.ShowDialog();
                frmGyelEdu2X = null;
                GetData(strTabTag);
            }
            if (strTabTag == "2")
            {
                frmGyelGeneral frmGyelGeneralX = new frmGyelGeneral(GstrPANO, GstrIllcode);
                frmGyelGeneralX.StartPosition = FormStartPosition.CenterParent;
                frmGyelGeneralX.ShowDialog();
                frmGyelGeneralX = null;
                GetData(strTabTag);
            }
            if (strTabTag == "3")
            {
                frmGyelGeneral2 frmGyelGeneral2X = new frmGyelGeneral2(GstrPANO, GstrIllcode);
                frmGyelGeneral2X.StartPosition = FormStartPosition.CenterParent;
                frmGyelGeneral2X.ShowDialog();
                frmGyelGeneral2X = null;
                GetData(strTabTag);
            }

        }

    
   
     
    }
}
