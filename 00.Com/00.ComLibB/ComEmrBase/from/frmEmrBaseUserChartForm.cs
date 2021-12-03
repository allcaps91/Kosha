using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBaseUserChartForm : Form
    {
        EmrForm fWrite = null;
        Form mCallForm = null;
        Form mChartForm = null;
        long mFORMNO = 0;
        long mUPDATENO = 0;
        double mMACRONO = 0;

        Form ActiveFormWrite = null;
        EmrChartForm ActiveFormWriteChart = null;

        #region //이벤트 전달
        //사용자 템플릿
        public delegate void EventSetUserChart(double dblMACRONO);
        public event EventSetUserChart rEventSetUserChart;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;
        #endregion

        public frmEmrBaseUserChartForm()
        {
            InitializeComponent();
        }

        public frmEmrBaseUserChartForm(Form CallForm, Form pChartForm, long pFORMNO, long pUPDATENO)
        {
            InitializeComponent();
            mCallForm = CallForm;
            mChartForm = pChartForm;
            mFORMNO = pFORMNO;
            mUPDATENO = pUPDATENO;
        }

        private void frmEmrBaseUserChartForm_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //}
            //ComFunc.SetFormInit(clsDB.DbCon,this, "Y", "Y", "Y");

            GetMacroList();

            if (mFORMNO != 0)
            {
                fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mFORMNO.ToString(), mUPDATENO.ToString());

                if (mChartForm != null)
                {
                    LoadForm(0);
                    if (mChartForm != null)
                    {

                    }
                }
            }
        }

        private void LoadForm(double dblMACRONO)
        {
            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;
            }
            
            ActiveFormWrite = new frmEmrChartNew(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), true, dblMACRONO.ToString());
            ActiveFormWriteChart = (EmrChartForm)ActiveFormWrite;
            ActiveFormWrite.TopLevel = false;
            this.Controls.Add(ActiveFormWrite);
            ActiveFormWrite.Parent = panEmr;
            ActiveFormWrite.Text = fWrite.FmFORMNAME;
            ActiveFormWrite.ControlBox = false;
            ActiveFormWrite.FormBorderStyle = FormBorderStyle.None;
            ActiveFormWrite.Top = 0;
            ActiveFormWrite.Left = 0;
            if (fWrite.FmALIGNGB == 1)   //Left
            {
                //panOption.Visible = false;
                ActiveFormWrite.Height = panEmr.Height - 20;
            }
            else if (fWrite.FmALIGNGB == 2)  //Top
            {
                //panOption.Visible = false;
                ActiveFormWrite.Width = panEmr.Width - 20;
            }
            else  //None
            {
                ActiveFormWrite.Dock = DockStyle.Fill;
            }
            ActiveFormWrite.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void GetMacroList()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) //권한 확인
            //{
            //    return;
            //}

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssMacro_Sheet1.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT  ";
            SQL = SQL + ComNum.VBLF + "        A.MACRONO, A.FORMNO, A.UPDATENO, A.GRPGB, A.USEGB, A.TITLE, F.FORMNAME";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRUSERCHARTFORM A";
            SQL = SQL + ComNum.VBLF + " INNER JOIN " + ComNum.DB_EMR + "AEMRFORM F";
            SQL = SQL + ComNum.VBLF + "     ON A.FORMNO = F.FORMNO";
            SQL = SQL + ComNum.VBLF + "     AND A.UPDATENO = F.UPDATENO";
            SQL = SQL + ComNum.VBLF + " WHERE A.FORMNO = " + mFORMNO;
            SQL = SQL + ComNum.VBLF + "     AND A.UPDATENO = " + mUPDATENO;
            SQL = SQL + ComNum.VBLF + "     AND A.DELYN = '0'";  //삭제 제외
            if (opdAllL.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "  AND (A.GRPGB = 'D'";
                SQL = SQL + ComNum.VBLF + "       OR ( A.GRPGB = 'U' OR A.USEGB = '" + clsType.User.IdNumber + "') )";
            }
            else if (optDeptL.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "  AND A.GRPGB = 'D'";
                SQL = SQL + ComNum.VBLF + "      AND A.USEGB = '" + clsType.User.DeptCode + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "     AND A.GRPGB = 'U'";
                SQL = SQL + ComNum.VBLF + "      AND A.USEGB = '" + clsType.User.IdNumber + "'";
            }
            
            SQL = SQL + ComNum.VBLF + " ORDER BY A.ORDERSEQ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
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

            ssMacro_Sheet1.RowCount = dt.Rows.Count;
            ssMacro_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssMacro_Sheet1.Cells[i, 0].Text = VB.Trim(dt.Rows[i]["GRPGB"].ToString());
                ssMacro_Sheet1.Cells[i, 1].Text = VB.Trim(dt.Rows[i]["TITLE"].ToString());
                ssMacro_Sheet1.Cells[i, 2].Text = VB.Trim(dt.Rows[i]["MACRONO"].ToString());
                ssMacro_Sheet1.Cells[i, 3].Text = VB.Trim(dt.Rows[i]["FORMNO"].ToString());
                ssMacro_Sheet1.Cells[i, 4].Text = VB.Trim(dt.Rows[i]["UPDATENO"].ToString());
                ssMacro_Sheet1.Cells[i, 5].Text = VB.Trim(dt.Rows[i]["USEGB"].ToString());
                ssMacro_Sheet1.Cells[i, 6].Text = VB.Trim(dt.Rows[i]["FORMNAME"].ToString());
            }

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetMacroList();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (ActiveFormWrite == null) return;

            txtTitle.Text = "";
            mMACRONO = 0;
            optUserR.Checked = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ActiveFormWrite == null) return;
            if (txtTitle.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "제목을 입력해 주십시요.");
                return;
            }

            if (SaveDate() == true)
            {
                txtTitle.Text = "";
                mMACRONO = 0;
                if (ActiveFormWrite != null)
                {
                    ActiveFormWrite.Dispose();
                    ActiveFormWrite = null;
                    ActiveFormWriteChart = null;
                }

                if (optUserR.Checked && optUserL.Checked == false)
                {
                    optUserL.Checked = true;
                }
                else if (optDeptR.Checked && optDeptL.Checked == false)
                {
                    optDeptL.Checked = true;
                }

                GetMacroList();
            }
        }

        private bool SaveDate()
        {
            if (mMACRONO != 0)
            {
                if (ComFunc.MsgBoxQ("내용 및 제목을 변경하시겠습니까?","EMR", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return false;
                }
            }
            
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            double dblMACRONO = 0;
            
            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                if (mMACRONO != 0)
                {
                    dblMACRONO = mMACRONO;

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_EMR + "AEMRUSERCHARTFORM SET ";
                    SQL = SQL + ComNum.VBLF + "     TITLE =  '" + txtTitle.Text.Replace("'","`") + "'";
                    SQL = SQL + ComNum.VBLF + "WHERE MACRONO =  " + mMACRONO;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_EMR + "AEMRUSERCHARTROW ";
                    SQL = SQL + ComNum.VBLF + "WHERE MACRONO =  " + mMACRONO;
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                else
                {
                    dblMACRONO = ComQuery.GetSequencesNoEx(clsDB.DbCon, "" + ComNum.DB_EMR + "SEQ_AEMRUSERSENTENCE_MACRONO");

                    if (dblMACRONO == 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, "시퀀스 조회중 에러가 발생했습니다.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRUSERCHARTFORM ";
                    SQL = SQL + ComNum.VBLF + "		(MACRONO, FORMNO, UPDATENO, GRPGB, USEGB, TITLE,  ";
                    SQL = SQL + ComNum.VBLF + "		ORDERSEQ, USEID, WRITEDATE, WRITETIME )           ";
                    SQL = SQL + ComNum.VBLF + "VALUES (           ";
                    SQL = SQL + ComNum.VBLF + "		" + dblMACRONO + ",";     //MACRONO, 
                    SQL = SQL + ComNum.VBLF + "		" + mFORMNO + ",";     //FORMNO, 
                    SQL = SQL + ComNum.VBLF + "		" + mUPDATENO + ",";     //UPDATENO, 
                    if (optDeptR.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "		'D',";     //GRPGB, 
                        SQL = SQL + ComNum.VBLF + "		'" + clsType.User.DeptCode + "',";     //USEGB, 
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "		'U',";     //GRPGB, 
                        SQL = SQL + ComNum.VBLF + "		'" + clsType.User.IdNumber + "',";     //USEGB, 
                    }
                    SQL = SQL + ComNum.VBLF + "		'" + txtTitle.Text.Replace("'", "`") + "',";     //TITLE, 
                    SQL = SQL + ComNum.VBLF + "		999,";     //ORDERSEQ, 
                    SQL = SQL + ComNum.VBLF + "		'" + clsType.User.IdNumber + "',";     //USEID, 
                    SQL = SQL + ComNum.VBLF + "		'" + VB.Left(strCurDateTime,8) + "',";     //WRITEDATE, 
                    SQL = SQL + ComNum.VBLF + "		'" + VB.Right(strCurDateTime, 6) + "'";		//WRITETIME
                    SQL = SQL + ComNum.VBLF + ")           ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                
                bool rtnVal = false;
                rtnVal = ActiveFormWriteChart.SaveUserFormMsg(dblMACRONO);

                if (rtnVal == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "AEMRUSERCHARTROW 저장중 에러가 발생했습니다");
                    clsDB.SaveSqlErrLog("AEMRUSERCHARTROW 저장중 에러가 발생했습니다", SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ActiveFormWrite == null) return;
            if (mMACRONO == 0)
            {
                ComFunc.MsgBoxEx(this, "선택된 항목이 없습니다.");
                return;
            }
            if (DeleteDate() == true)
            {
                txtTitle.Text = "";
                mMACRONO = 0;
                if (ActiveFormWrite != null)
                {
                    ActiveFormWrite.Dispose();
                    ActiveFormWrite = null;
                    ActiveFormWriteChart = null;
                }
                GetMacroList();
            }
        }

        private bool DeleteDate()
        {
            if (mMACRONO != 0)
            {
                if (ComFunc.MsgBoxQ("삭제 하시겠습니까?" + ComNum.VBLF + "삭제하면 되돌릴수 없습니다.", "EMR", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return false;
                }
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_EMR + "AEMRUSERCHARTFORM SET ";
                SQL = SQL + ComNum.VBLF + "     DELYN =  '1', ";
                SQL = SQL + ComNum.VBLF + "     USEID =  '" + clsType.User.IdNumber + "',";
                SQL = SQL + ComNum.VBLF + "     WRITEDATE =  '" + VB.Left(strCurDateTime,8) + "',";
                SQL = SQL + ComNum.VBLF + "     WRITETIME =  '" + VB.Right(strCurDateTime, 6) + "'";
                SQL = SQL + ComNum.VBLF + "WHERE MACRONO =  " + mMACRONO;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

        }
        private void ssMacro_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssMacro_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssMacro, e.Column);
                return;
            }
            
            double dblMACRONO = VB.Val(ssMacro_Sheet1.Cells[e.Row, 2].Text.Trim());
            string strGRPGB = ssMacro_Sheet1.Cells[e.Row, 0].Text.Trim();

            //ssMacro_Sheet1.Cells[0, 0, ssMacro_Sheet1.RowCount - 1, ssMacro_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            //ssMacro_Sheet1.Cells[e.Row, 0, e.Row, ssMacro_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            txtTitle.Text = ssMacro_Sheet1.Cells[e.Row, 1].Text.Trim();
            mMACRONO = dblMACRONO;
            if (strGRPGB == "D")
            {
                optDeptR.Checked = true;
            }
            else
            {
                optUserR.Checked = true;
            }

            LoadForm(mMACRONO);
            
        }

        private void ssMacro_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssMacro_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssMacro, e.Column);
                return;
            }

            double dblMACRONO = VB.Val(ssMacro_Sheet1.Cells[e.Row, 2].Text.Trim());

            rEventSetUserChart(dblMACRONO);
        }
    }
}
