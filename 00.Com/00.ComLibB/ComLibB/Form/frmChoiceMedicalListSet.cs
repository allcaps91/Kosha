using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmChoiceMedicalListSet : Form
    {
        /// Class Name      : ComLibB.dll
        /// File Name       : frmChoiceMedicalListSet.cs
        /// Description     : 선택 진료비 항목별 Set
        /// Author          : 김효성
        /// Create Date     : 2017-06-22
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// VB\basic\busanid\BuCode53.frm => frmChoiceMedicalListSet.cs 으로 변경함
        /// </history>
        /// <seealso> 
        /// VB\basic\bucode\BuCode53.frm(FrmSelectSet)
        /// </seealso>
        /// <vbp>
        /// default : VB\basic\bucode\bucode.vbp
        /// </vbp>
        string GstrSabun = "";

        public frmChoiceMedicalListSet ()
        {
            InitializeComponent ();
        }
        public frmChoiceMedicalListSet (string strSabun)
        {
            InitializeComponent ();

            GstrSabun = strSabun;
        }

        private void ClearssViewOne ()   //CLEAR_SS2()
        {
            ssViewOne_Sheet1.Cells [0 , 0 , 0 , ssViewOne_Sheet1.ColumnCount - 1].Text = "";
        }

        private void frmChoiceMedicalListSet_Load (object sender , EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            int i = 0;
            int intCol = 0;
            int intREAD = 0;
            string [] strDept = null;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            clsVbfunc.SetComboDept (clsDB.DbCon, cboChois, "" , 0);
            ComFunc.ReadSysDate(clsDB.DbCon);

            //ssChk_Sheet1.RowCount = 0;
            //ssChk_Sheet1.ColumnCount = 0;
            ssChk_Sheet1.Cells [0 , 0].Text = clsPublic.GstrSysDate;

            SQL = "SELECT DEPTCODE FROM BAS_CLINICDEPT ";
            SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

            SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

            intREAD = dt.Rows.Count;

            strDept = new string [dt.Rows.Count];
            intCol = 1;

            ssChk_Sheet1.ColumnCount = intREAD + 1;
            FarPoint.Win.Spread.CellType.CheckBoxCellType CellChk = new FarPoint.Win.Spread.CellType.CheckBoxCellType ();
            ssChk_Sheet1.Columns [1 , ssChk_Sheet1.ColumnCount - 1].CellType = CellChk;
            ssChk_Sheet1.Columns [1 , ssChk_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssChk_Sheet1.Columns [1 , ssChk_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            for (i = 0; i < intREAD; i++)
            {
                ssChk_Sheet1.Columns [intCol].Width = 30;
                ssChk_Sheet1.ColumnHeader.Cells [0 , intCol].Text = dt.Rows [i] ["DeptCode"].ToString ().Trim ();

                SQL = " SELECT ALL_USE FROM " + ComNum.DB_PMPA + "BAS_SELECT_DEPT ";
                SQL = SQL + ComNum.VBLF + "  Where DeptCode = '" + dt.Rows [i] ["DeptCode"].ToString ().Trim () + "' ";

                SqlErr = clsDB.GetDataTable (ref dt1 , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt1.Rows.Count > 0)
                {
                    if (dt1.Rows [0] ["ALL_USE"].ToString ().Trim () == "N")
                    {
                        ssChk_Sheet1.Cells [0 , intCol].Value = true;
                        ssChk_Sheet1.Cells [0 , intCol].BackColor = Color.FromArgb (240 , 240 , 240);
                    }
                    else
                    {
                        ssChk_Sheet1.Cells [0 , intCol].BackColor = Color.FromArgb (255 , 255 , 255);
                    }
                }
                dt1.Dispose ();
                dt1 = null;

                intCol = intCol + 1;

                strDept [i] = dt.Rows [i] ["DeptCode"].ToString ().Trim ();
            }
            dt.Dispose ();
            dt = null;

            ssViewOne_Sheet1.Cells [0 , 1].Text = "";
            FarPoint.Win.Spread.CellType.ComboBoxCellType CellCbo = new FarPoint.Win.Spread.CellType.ComboBoxCellType ();
            ssViewOne_Sheet1.Cells [0 , 1].CellType = CellCbo;
            ssViewOne_Sheet1.Cells [0 , 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssViewOne_Sheet1.Cells [0 , 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            clsSpread.gSpreadComboDataSetEx1 (ssViewOne , 0 , 1 , 0 , 1 , strDept , false);
            ssViewOne_Sheet1.Cells [0 , 1].Border = new FarPoint.Win.LineBorder (Color.Black , 1 , true , true , true , true);
        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }

        private bool Delete ()
        {
            bool rtnVal = false;
            string strROWID = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ComQuery.IsJobAuth(this , "D", clsDB.DbCon) == false) return rtnVal; //rtnVal;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                ssViewOne_Sheet1.RowCount = 0;
                ssViewOne_Sheet1.ColumnCount = 18;
                strROWID = ssViewOne_Sheet1.Cells [0 , 18].Text;

                if (strROWID == "")
                {
                    return rtnVal;
                }

                SQL = " INSERT INTO " + ComNum.DB_PMPA + "BAS_SELECT_SET_HIS(JDATE, OSET0,OSET1,OSET2, OSET3, OSET4, OSET5, OSET6, OSET7,ISET0,ISET1,ISET2, ISET3, ISET4, ISET5, ISET6, ISET7,  ENTDATE, SABUN, GBN ,DEPTCODE, CDATE )  ";
                SQL = SQL + ComNum.VBLF + " SELECT JDATE, OSET0,OSET1,OSET2, OSET3, OSET4, OSET5, OSET6, OSET7,ISET0,ISET1,ISET2, ISET3, ISET4, ISET5, ISET6, ISET7,  SYSDATE , '" + GstrSabun + "' ,'D', DEPTCODE, SYSDATE   FROM " + ComNum.DB_PMPA + "BAS_SELECT_SET ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                SQL = "DELETE KOSMOS_PMPA.BAS_SELECT_SET WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                clsDB.setCommitTran (clsDB.DbCon);
                ComFunc.MsgBox ("삭제 하였습니다.");
                Cursor.Current = Cursors.Default;
                btnDelete.Enabled = false;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnDelete_Click (object sender , EventArgs e)  //CmdDelete_Click()
        {
            if (Delete () == true)
            {
                ClearssViewOne ();
                Search ();
            }
        }

        private bool Search ()
        {
            bool rtnVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return rtnVal;//권한 확인
            ClearssViewOne ();

            try
            {
                SQL = " SELECT JDATE, DEPTCODE, OSET0, OSET1, OSET2, OSET3, OSET4, OSET5, OSET6, OSET7, ISET0, ISET1, ISET2, ISET3, ISET4, ISET5, ISET6, ISET7, DrCode, ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_SELECT_SET ";

                if (VB.Left (cboChois.Text , 2) != "**") SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = '" + VB.Left (cboChois.Text , 2) + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY JDATE DESC  ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                ssViewTwo_Sheet1.RowCount = 0;
                ssViewTwo_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssViewTwo_Sheet1.Cells [i , 0].Text = dt.Rows [i] ["JDATE"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 1].Text = dt.Rows [i] ["DeptCode"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 2].Text = dt.Rows [i] ["ISET0"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["ISET1"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 4].Text = dt.Rows [i] ["ISET2"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 5].Text = dt.Rows [i] ["ISET3"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 6].Text = dt.Rows [i] ["ISET4"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 7].Text = dt.Rows [i] ["ISET5"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 8].Text = dt.Rows [i] ["ISET6"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 9].Text = dt.Rows [i] ["ISET7"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 10].Text = dt.Rows [i] ["OSET0"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 11].Text = dt.Rows [i] ["OSET1"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 12].Text = dt.Rows [i] ["OSET2"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 13].Text = dt.Rows [i] ["OSET3"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 14].Text = dt.Rows [i] ["OSET4"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 15].Text = dt.Rows [i] ["OSET5"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 16].Text = dt.Rows [i] ["OSET6"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 17].Text = dt.Rows [i] ["OSET7"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 18].Text = dt.Rows [i] ["ROWID"].ToString ().Trim ();
                    ssViewTwo_Sheet1.Cells [i , 19].Text = dt.Rows [i] ["DrCode"].ToString ().Trim ();
                }
                ssViewTwo_Sheet1.SetRowHeight (i , Convert.ToInt32 (ssViewTwo_Sheet1.GetPreferredRowHeight (i)) + 15);

                dt.Dispose ();
                dt = null;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                rtnVal = false;
                return rtnVal;
            }

        }

        private void btnSearch_Click (object sender , EventArgs e)  //CmdView_Click
        {
            Search ();
        }

        private bool ChkSave ()
        {
            int i = 0;
            string strDept = "";
            string strUse = "";
            string strJDate = "";
            string strOK = "";
            string strROWID = "";
            string SqlErr = ""; //에러문 받는 변수
            bool rtnVal = false;
            string SQL = "";
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            if (ComQuery.IsJobAuth(this , "C", clsDB.DbCon) == false) return rtnVal; //rtnVal;

            clsDB.setBeginTran(clsDB.DbCon);
            

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                strJDate = ssChk_Sheet1.Cells [0 , 0].Text;

                for (i = 2; i < ssChk_Sheet1.RowCount; i++)
                {
                    strDept = ssChk_Sheet1.Cells [0 , i].Text.Trim();
                    strUse = ssChk_Sheet1.Cells [0 , i].Text == "1" ? "N" : "Y".Trim();

                    strOK = "";
                    strROWID = "";

                    SQL = " SELECT ROWID From BAS_SELECT_DEPT ";
                    SQL = SQL + ComNum.VBLF + " Where JDate =TO_DATE('" + strJDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDept + "' ";

                    SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                    if (dt.Rows.Count > 0) strROWID = dt.Rows [0] ["ROWID"].ToString ().Trim ();

                    dt.Dispose ();
                    dt = null;

                    if (strROWID == "")
                    {
                        SQL = " INSERT INTO BAS_SELECT_DEPT (JDATE,DEPTCODE,ALL_USE,ENTDATE,SABUN )";
                        SQL = SQL + ComNum.VBLF + " Values (TO_DATE('" + strJDate + "','YYYY-MM-DD'),'" + strDept + "','";
                        SQL = SQL + ComNum.VBLF + strUse + "',SYSDATE," + GstrSabun + ") ";

                        SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);
                    }
                    else
                    {
                        SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_SELECT_DEPT SET ";
                        SQL = SQL + ComNum.VBLF + " JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD') ,";
                        SQL = SQL + ComNum.VBLF + " ALL_USE = '" + strUse + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);
                    }
                    SQL = " INSERT INTO BAS_SELECT_DEPT_HIS (JDATE,DEPTCODE,ALL_USE,ENTDATE,SABUN )";
                    SQL = SQL + ComNum.VBLF + " Values (TO_DATE('" + strJDate + "','YYYY-MM-DD'),'" + strDept + "','";
                    SQL = SQL + ComNum.VBLF + strUse + "',SYSDATE," + GstrSabun + ") ";

                    SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);
                }

                clsDB.setCommitTran (clsDB.DbCon);
                ComFunc.MsgBox ("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                btnDelete.Enabled = false;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnChkSave_Click (object sender , EventArgs e) //CmdSave2_Click
        {
            if (ChkSave () == true)
            {
                Search ();
            }
        }

        private bool OneSave ()
        {
            string strROWID = "";
            string strJDate = "";
            string strDeptCode = "";
            string strISet0 = "";
            string strISet1 = "";
            string strISet2 = "";
            string strISet3 = "";
            string strISet4 = "";
            string strISet5 = "";
            string strISet6 = "";
            string strISet7 = "";
            string strOSet0 = "";
            string strOSet1 = "";
            string strOSet2 = "";
            string strOSet3 = "";
            string strOSet4 = "";
            string strOSet5 = "";
            string strOSet6 = "";
            string strOSet7 = "";
            string strDrCode = "";
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ComQuery.IsJobAuth(this , "C", clsDB.DbCon) == false) return rtnVal; //rtnVal;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                strJDate = ssViewOne_Sheet1.Cells [0 , 0].Text;
                strDeptCode = ssViewOne_Sheet1.Cells [0 , 1].Text;
                strISet0 = ssViewOne_Sheet1.Cells [0 , 2].Text;
                strISet1 = ssViewOne_Sheet1.Cells [0 , 3].Text;
                strISet2 = ssViewOne_Sheet1.Cells [0 , 4].Text;
                strISet3 = ssViewOne_Sheet1.Cells [0 , 5].Text;
                strISet4 = ssViewOne_Sheet1.Cells [0 , 6].Text;
                strISet5 = ssViewOne_Sheet1.Cells [0 , 7].Text;
                strISet6 = ssViewOne_Sheet1.Cells [0 , 8].Text;
                strISet7 = ssViewOne_Sheet1.Cells [0 , 9].Text;
                strOSet0 = ssViewOne_Sheet1.Cells [0 , 10].Text;
                strOSet1 = ssViewOne_Sheet1.Cells [0 , 11].Text;
                strOSet2 = ssViewOne_Sheet1.Cells [0 , 12].Text;
                strOSet3 = ssViewOne_Sheet1.Cells [0 , 13].Text;
                strOSet4 = ssViewOne_Sheet1.Cells [0 , 14].Text;
                strOSet5 = ssViewOne_Sheet1.Cells [0 , 15].Text;
                strOSet6 = ssViewOne_Sheet1.Cells [0 , 16].Text;
                strOSet7 = ssViewOne_Sheet1.Cells [0 , 17].Text;
                strROWID = ssViewOne_Sheet1.Cells [0 , 18].Text;
                strDrCode = VB.Pstr (ssViewOne_Sheet1.Cells [0 , 19].Text , "." , 2);

                if (strJDate == "")
                {
                    ComFunc.MsgBox ("적용일자가 공란입니다." , "확인");
                    return false;
                }

                if (strROWID == "")
                {
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "BAS_SELECT_SET_HIS(JDATE, OSET0,OSET1,OSET2, OSET3, OSET4, OSET5, OSET6, OSET7,ISET0,ISET1,ISET2, ISET3, ISET4, ISET5, ISET6, ISET7,  ENTDATE, SABUN, GBN ,DEPTCODE, CDATE, DrCode )  ";
                    SQL = SQL + ComNum.VBLF + " SELECT JDATE, OSET0,OSET1,OSET2, OSET3, OSET4, OSET5, OSET6, OSET7,ISET0,ISET1,ISET2, ISET3, ISET4, ISET5, ISET6, ISET7,  SYSDATE , '" + GstrSabun + "' ,'I', DEPTCODE, SYSDATE,DrCode  FROM KOSMOS_PMPA.BAS_SELECT_SET ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "BAS_SELECT_SET(JDATE, ISET0, ISET1,ISET2, ISET3, ISET4, ISET5, ISET6, ISET7, OSET0, OSET1,OSET2, OSET3, OSET4, OSET5, OSET6, OSET7,  ENTDATE, SABUN, DEPTCODE, DrCode )  VALUES (";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strJDate + "','YYYY-MM-DD') ,";
                    SQL = SQL + ComNum.VBLF + "  '" + strISet0 + "', '" + strISet1 + "' , '" + strISet2 + "' , '" + strISet3 + "' , '" + strISet4 + "','" + strISet5 + "','" + strISet6 + "', '" + strISet7 + "', ";
                    SQL = SQL + ComNum.VBLF + "  '" + strOSet0 + "', '" + strOSet1 + "' , '" + strOSet2 + "' , '" + strOSet3 + "' , '" + strOSet4 + "','" + strOSet5 + "','" + strOSet6 + "', '" + strOSet7 + "' ";
                    SQL = SQL + ComNum.VBLF + ", SYSDATE, '" + GstrSabun + "' ,'" + strDeptCode + "','" + strDrCode + "' ) ";

                    SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);
                }
                else
                {
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "BAS_SELECT_SET_HIS(JDATE, OSET0,OSET1,OSET2, OSET3, OSET4, OSET5, OSET6, OSET7,ISET0,ISET1,ISET2, ISET3, ISET4, ISET5, ISET6, ISET7,  ENTDATE, SABUN, GBN ,DEPTCODE, CDATE,DrCode )  ";
                    SQL = SQL + ComNum.VBLF + " SELECT JDATE, OSET0,OSET1,OSET2, OSET3, OSET4, OSET5, OSET6, OSET7,ISET0,ISET1,ISET2, ISET3, ISET4, ISET5, ISET6, ISET7,  SYSDATE , '" + GstrSabun + "' ,'U', DEPTCODE , SYSDATE, DrCode FROM " + ComNum.DB_PMPA + "BAS_SELECT_SET ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                    SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_SELECT_SET SET ";
                    SQL = SQL + " JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD') ,";
                    SQL = SQL + " ISET0 = '" + strISet0 + "' ,";
                    SQL = SQL + " ISET1 = '" + strISet1 + "' ,";
                    SQL = SQL + " ISET2 = '" + strISet2 + "' ,";
                    SQL = SQL + " ISET3 = '" + strISet3 + "' ,";
                    SQL = SQL + " ISET4 = '" + strISet4 + "' ,";
                    SQL = SQL + " ISET5 = '" + strISet5 + "' ,";
                    SQL = SQL + " ISET6 = '" + strISet6 + "' ,";
                    SQL = SQL + " ISET7 = '" + strISet7 + "' ,";
                    SQL = SQL + " OSET0 = '" + strOSet0 + "' ,";
                    SQL = SQL + " OSET1 = '" + strOSet1 + "' ,";
                    SQL = SQL + " OSET2 = '" + strOSet2 + "' ,";
                    SQL = SQL + " OSET3 = '" + strOSet3 + "' ,";
                    SQL = SQL + " OSET4 = '" + strOSet4 + "' ,";
                    SQL = SQL + " OSET5 = '" + strOSet5 + "' ,";
                    SQL = SQL + " OSET6 = '" + strOSet6 + "' ,";
                    SQL = SQL + " OSET7 = '" + strOSet7 + "' ,";
                    SQL = SQL + " DrCode = '" + strDrCode + "', ";
                    SQL = SQL + "  DEPTCODE = '" + strDeptCode + "' ";
                    SQL = SQL + " WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);
                }
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran (clsDB.DbCon);
                    ComFunc.MsgBox (SqlErr);
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran (clsDB.DbCon);
                ComFunc.MsgBox ("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                btnDelete.Enabled = false;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnSSViewOneSave_Click (object sender , EventArgs e)   //cmdSave_Click
        {
            if (OneSave () == true)
            {
                Search ();
            }
        }

        private void btnPrint_Click (object sender , EventArgs e)///CmdPrint_Click()
        {
            if (ComQuery.IsJobAuth(this , "P", clsDB.DbCon) == false) return;//권한 확인      

            ssViewTwo_Sheet1.PrintInfo.Header = "/fn\"굴림체\" /fz\"18\"" + "/c" + "선택진료비 항목별 SET LIST" + "/n/n";
            ssViewTwo_Sheet1.PrintInfo.Header = ssViewTwo_Sheet1.PrintInfo.Header + "/fn\"굴림체\" /fz\"11\"" + "/l";
            ssViewTwo_Sheet1.PrintInfo.Header = ssViewTwo_Sheet1.PrintInfo.Header + "◆인쇄일자: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            ssViewTwo_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssViewTwo_Sheet1.PrintInfo.Margin.Top = 10;
            ssViewTwo_Sheet1.PrintInfo.Margin.Bottom = 10;
            ssViewTwo_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssViewTwo_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssViewTwo_Sheet1.PrintInfo.ShowBorder = true;
            ssViewTwo_Sheet1.PrintInfo.ShowGrid = true;
            ssViewTwo_Sheet1.PrintInfo.ShowColor = true;
            ssViewTwo_Sheet1.PrintInfo.ShowShadows = true;
            ssViewTwo_Sheet1.PrintInfo.UseMax = false;
            ssViewTwo_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssViewTwo_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssViewTwo.PrintSheet (0);
        }   //CmdPrint_Click

        private void ssViewTwo_CellDoubleClick (object sender , FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            if (e.ColumnHeader == true || e.RowHeader == true) return;

            for (i = 0; i < 20; i++)
            {
                if (i == 20) i = i;
                ssViewOne_Sheet1.Cells [0 , i].Text = ssViewTwo_Sheet1.Cells [e.Row , i].Text;
            }

        }

        private void ssViewOne_EditModeOff (object sender , EventArgs e)
        {
            int i = 0;
            int intRow = ssViewOne_Sheet1.ActiveRowIndex;
            int intCol = ssViewOne_Sheet1.ActiveColumnIndex;
            string strDept = "";
            string [] strDr = null;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            try
            {

                if (intCol != 2) return;

                strDept = ssViewOne_Sheet1.Cells [0 , 1].Text;
                strDept = "";

                SQL = " SELECT DrCode,DrName FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DRDEPT1 ='" + strDept + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TOUR ='N' ";
                SQL = SQL + ComNum.VBLF + "   AND SUBSTR(DRCODE,3,2) <> '99' ";
                SQL = SQL + ComNum.VBLF + " ORDER By DrNAME ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    strDr = new string [dt.Rows.Count];


                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDr [i] = dt.Rows [i] ["DrName"].ToString ().Trim () + "." + dt.Rows [i] ["DrCode"].ToString ().Trim ();
                    }
                }
                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    return;
                }

                dt.Dispose ();
                dt = null;

                clsSpread.gSpreadComboDataSetEx1 (ssViewOne , intRow , 1 , intRow , 1 , strDr , false);
                ssViewTwo_Sheet1.Cells [intRow , 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssViewTwo_Sheet1.Cells [intRow , 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                ssViewOne_Sheet1.Cells [0 , 1].Border = new FarPoint.Win.LineBorder (Color.Black , 1 , true , true , true , true);
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }

        }
    }
}
