using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmNrstdEduManageSet.cs
    /// Description     : 교육관리 권한설정화면
    /// Author          : 안정수
    /// Create Date     : 2018-01-29
    /// Update History  : 
    /// 
    /// </summary>
    /// <history>  
    /// 기존 Frm교육관리기초2.frm(Frm교육관리기초2) 폼 frmNrstdEduManageSet.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm교육관리기초2.frm(Frm교육관리기초2) >> frmNrstdEduManageSet.cs 폼이름 재정의" />
    public partial class frmNrstdEduManageSet : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        int intRowAffected = 0; //변경된 Row 받는 변수

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;
        DataTable dt1 = null;

        #endregion


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

        #endregion

        public frmNrstdEduManageSet(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmNrstdEduManageSet()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnDel.Click += new EventHandler(eBtnClick);
            this.btnNew.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);

            this.txtSabun.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtSeqNo.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtSName.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            //this.eControl.LostFocus += new EventHandler(eControl_LostFocus);
            //this.eControl.GotFocus += new EventHandler(eControl_GotFocus);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                ComFunc.ReadSysDate(clsDB.DbCon);

                ssList.ActiveSheet.Columns[4].Visible = false;
                SCREEN_CLEAR();
                eGetData();
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

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eSaveData();
            }

            else if (sender == this.btnDel)
            {
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eDelData();
            }

            else if (sender == this.btnNew)
            {
                btnNew_Click();
            }

            else if (sender == this.btnCancel)
            {
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                btnCancel_Click();
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtSeqNo || sender == this.txtSName)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }

            else if (sender == this.txtSabun)
            {
                if (e.KeyChar == 13)
                {
                    string strSabun = "";

                    if (String.Compare(txtSabun.Text, "99999") <= 0)
                    {
                        strSabun = ComFunc.SetAutoZero(txtSabun.Text, 5);
                    }

                    else
                    {
                        strSabun = ComFunc.SetAutoZero(txtSabun.Text, 6);
                    }

                    //인사마스타에서 부서코드를 읽음
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  a.Sabun,a.KorName,a.Buse,b.Name BuName,c.Name JikName ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST a, " + ComNum.DB_PMPA + "BAS_BUSE b, " + ComNum.DB_ERP + "INSA_CODE c";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND a.Sabun='" + strSabun + "'";
                    SQL += ComNum.VBLF + "      AND a.Buse=b.BuCode(+)";
                    SQL += ComNum.VBLF + "      AND a.Jik=c.Code(+)";
                    SQL += ComNum.VBLF + "      AND c.Gubun='2'";   //직책

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("인사마스타에 등록이 않됨!!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        txtSName.Text = dt1.Rows[0]["KorName"].ToString().Trim();
                        txtSts.Text = dt1.Rows[0]["Buse"].ToString().Trim();
                        txtSts2.Text = dt1.Rows[0]["BuName"].ToString().Trim() + " " + dt1.Rows[0]["JikName"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eGetData()
        {
            int i = 0;
            string strSabun = "";

            CS.Spread_All_Clear(ssList);


            SQL = "";
            SQL = "SELECT A.GUBUN , A.JONG, A.CODE, A.NAME, A.REMARK, A.ROWID,          ";
            SQL = SQL + ComNum.VBLF + "  B.SABUN,B.KORNAME,B.BUSE,C.NAME AS BUNAME, D.NAME AS JIKNAME          ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.NUR_EDU_CODE A          ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_ADM.INSA_MST B          ";
            SQL = SQL + ComNum.VBLF + "    ON A.REMARK = TRIM(B.SABUN)          ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.BAS_BUSE C          ";
            SQL = SQL + ComNum.VBLF + "    ON B.BUSE = C.BUCODE          ";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_ADM.INSA_CODE D          ";
            SQL = SQL + ComNum.VBLF + "    ON B.JIK = D.CODE          ";
            SQL = SQL + ComNum.VBLF + "WHERE A.GUBUN = '1'          ";
            SQL = SQL + ComNum.VBLF + "    AND D.GUBUN = '2'          ";
            SQL = SQL + ComNum.VBLF + "    AND  B.JAEGU = '0'       ";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.ENTDATE          ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ssList.ActiveSheet.Rows.Count = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssList.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["CODE"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    if (String.Compare(dt.Rows[i]["REMARK"].ToString().Trim(), "99999") <= 0)
                    {
                        strSabun = ComFunc.SetAutoZero(dt.Rows[i]["REMARK"].ToString().Trim(), 5);
                    }

                    else
                    {
                        strSabun = ComFunc.SetAutoZero(dt.Rows[i]["REMARK"].ToString().Trim(), 6);
                    }

                    ssList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["BUNAME"].ToString().Trim() + " " + dt.Rows[i]["JIKNAME"].ToString().Trim();
                }

                ssList.ActiveSheet.Rows[0, ssList.ActiveSheet.Rows.Count - 1].Height = 20;
            }

            dt.Dispose();
            dt = null;
        }

        void eSaveData()
        {
            //int i = 0;
            string strSEQNO = "";
            string strSName = "";
            string strSabun = "";
            //string strRemark = "";
            //string strJong = "";
            string strROWID = "";

            strSEQNO = txtSeqNo.Text.Trim();
            strSName = txtSName.Text.Trim();
            strSabun = txtSabun.Text.Trim();
            strROWID = txtROWID.Text.Trim();

            //Gubun , code, name -성명, SDATE-사번, ENTDATE, Remark-참고사항

            clsDB.setBeginTran(clsDB.DbCon);

            if (strROWID == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "NUR_Edu_CODE";
                SQL += ComNum.VBLF + "(Gubun,Code,Name,ENTDATE,Remark )";
                SQL += ComNum.VBLF + "VALUES (";
                SQL += ComNum.VBLF + "    '1',";
                SQL += ComNum.VBLF + "  '" + strSEQNO + "',";
                SQL += ComNum.VBLF + "  '" + strSName + "',";
                SQL += ComNum.VBLF + "   SYSDATE,";
                SQL += ComNum.VBLF + "  '" + strSabun + "'";
                SQL += ComNum.VBLF + ")";
            }

            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "NUR_Edu_CODE";
                SQL += ComNum.VBLF + "SET ";
                SQL += ComNum.VBLF + "  Gubun= '1'";
                SQL += ComNum.VBLF + "  , Code= '" + strSEQNO + "'";
                SQL += ComNum.VBLF + "  , Name= '" + strSName + "'";
                SQL += ComNum.VBLF + "  , Remark= '" + strSabun + "'";
                SQL += ComNum.VBLF + "WHERE ROWID='" + strROWID + "'";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
            ComFunc.MsgBox("저장하였습니다.");

            SCREEN_CLEAR();
            eGetData();
        }

        void btnNew_Click()
        {
            string strSEQNO = "";

            SCREEN_CLEAR();

            strSEQNO = "";

            btnSave.Enabled = true;
            btnDel.Enabled = true;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  MAX(Code) MaxSeq ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
            SQL += ComNum.VBLF + "WHERE Gubun ='1'";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                strSEQNO = ComFunc.SetAutoZero((VB.Val(dt.Rows[0]["MaxSeq"].ToString().Trim()) + 1).ToString(), 3);
            }

            dt.Dispose();
            dt = null;

            txtSeqNo.Text = strSEQNO;
            txtSabun.Focus();
        }

        void eDelData()
        {
            //int i = 0;

            string strROWID = "";
            strROWID = txtROWID.Text.Trim();

            if (MessageBox.Show("정말로 삭제를 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = "";
            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "NUR_Edu_CODE";
            SQL += ComNum.VBLF + "WHERE ROWID='" + strROWID + "'";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
            ComFunc.MsgBox("삭제하였습니다.");

            SCREEN_CLEAR();
            eGetData();
        }

        void btnCancel_Click()
        {
            SCREEN_CLEAR();
        }

        void SCREEN_CLEAR()
        {
            ComFunc.SetAllControlClear(panel1);
            btnSave.Enabled = false;
            btnDel.Enabled = false;
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //int i = 0;
            string strSabun = "";

            SCREEN_CLEAR();

            txtROWID.Text = ssList.ActiveSheet.Cells[e.Row, 4].Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  GUBUN,JONG,CODE,NAME,STIME,MAN,PLACE,JUMSU,ENTDATE,REMARK ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_CODE";
            SQL += ComNum.VBLF + "WHERE ROWID ='" + txtROWID.Text + "'";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                txtSeqNo.Text = dt.Rows[0]["CODE"].ToString().Trim();
                txtSName.Text = dt.Rows[0]["NAME"].ToString().Trim();
                txtSabun.Text = dt.Rows[0]["REMARK"].ToString().Trim();

                if (String.Compare(txtSabun.Text, "99999") <= 0)
                {
                    strSabun = ComFunc.SetAutoZero(txtSabun.Text, 5);
                }

                else
                {
                    strSabun = ComFunc.SetAutoZero(txtSabun.Text, 6);
                }

                //인사마스타에서 부서코드를 읽음
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  a.Sabun,a.KorName,a.Buse,b.Name BuName,c.Name JikName ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST a, " + ComNum.DB_PMPA + "BAS_BUSE b, " + ComNum.DB_ERP + "INSA_CODE c";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND a.Sabun='" + strSabun + "'";
                SQL += ComNum.VBLF + "      AND a.Buse=b.BuCode(+)";
                SQL += ComNum.VBLF + "      AND a.Jik=c.Code(+)";
                SQL += ComNum.VBLF + "      AND c.Gubun='2'";   //직책

                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("인사마스타에 등록이 않됨!!");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    txtSName.Text = dt1.Rows[0]["KorName"].ToString().Trim();
                    txtSts.Text = dt1.Rows[0]["Buse"].ToString().Trim();
                    txtSts2.Text = dt1.Rows[0]["BuName"].ToString().Trim() + " " + dt1.Rows[0]["JikName"].ToString().Trim();
                }

                dt1.Dispose();
                dt1 = null;
            }

            dt.Dispose();
            dt = null;
        }
    }
}
