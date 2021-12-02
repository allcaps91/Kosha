using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewPateintAddress.cs
    /// Description     : 환자 주소록 조회 폼
    /// Author          : 안정수
    /// Create Date     : 2017-10-16
    /// Update History  : 
    /// <history>       
    /// d:\psmh\OPD\oiguide\FrmAddress.frm(FrmAddress) => frmPmpaViewPateintAddress.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oiguide\FrmAddress.frm(FrmAddress)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewPateintAddress : Form
    {
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        int nOpt = 0;
        int nRow = 0;
        public frmPmpaViewPateintAddress()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);
            this.btnDel.Click += new EventHandler(eBtnEvent);

            this.txtStic.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtStic)
            {
                if (e.KeyChar == 13 && txtStic.Text.Trim() != "")
                {
                    eGetData();
                }
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            optSoptA.Checked = true;
            txtStic.Focus();
        }

        void eBtnEvent(object sender, EventArgs e)
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
                eSave();
            }

            else if (sender == this.btnDel)
            {
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eDelete();
            }
        }

        void Clear_Screen()
        {
            int i = 0;
            int j = 0;

            txtMemo.Text = "";
            txtStic.Focus();

            CS.Spread_All_Clear(ssList);

            for (i = 0; i < ssList_Sheet1.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = "0";
            }

            nRow = 0;
        }

        void eGetData()
        {
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Clear_Screen();

            if (chkaddress.Checked == true && txtStic.Text == "")
            {
                ComFunc.MsgBox("마스터 주소 조회시는 조건을 입력하십시요.");
                return;
            }

            else if(txtStic.Text == "")
            {
                ComFunc.MsgBox("조회 할 글자 또는 숫자를 입력하세요!");
                return;
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  a.Pano, a.Sname, a.Jumin1, a.Jumin2, a.Deptcode, a.Lastdate,";

            if (chkaddress.Checked == false)
            {
                SQL += ComNum.VBLF + "  a.Tel, a.Hphone, a.Juso";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT a";
                SQL += ComNum.VBLF + "Where 1=1";
            }
            else if (chkaddress.Checked == true)
            {
                SQL += ComNum.VBLF + "  b.Tel, b.Hphone, b.Juso, b.Memo ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT a, " + ComNum.DB_PMPA + "ETC_Address b";
                SQL += ComNum.VBLF + "WHERE a.Pano(+) = b.Pano";
            }
            if (txtStic.Text != "")
            {
                switch (nOpt)
                {
                    case 0:
                        SQL += ComNum.VBLF + "  And a.Sname = '" + txtStic.Text + "'";
                        break;

                    case 1:
                        SQL += ComNum.VBLF + "  And (a.Jumin1 = '" + VB.Left(txtStic.Text, 6) + "' " +
                                      " or a.Jumin2 = '" + VB.Right(txtStic.Text, 7) + "')";
                        break;
                }
            }
            SQL += ComNum.VBLF + "ORDER BY a.Sname , Substr(a.Jumin1, 1, 2) DESC";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (chkaddress.Checked == true && dt.Rows.Count <= 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                else if (chkaddress.Checked == false && dt.Rows.Count <= 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다. 기존 자료에서 찾아보시기 바랍니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 60)
                    {
                        if (MessageBox.Show("조회건수가 60건이 넘습니다. 계속하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        SQL += ComNum.VBLF + "a.Pano, a.Sname, a.Jumin1, a.Jumin2, a.Deptcode, a.Lastdate, ";
                        SQL += ComNum.VBLF + "b.Tel, b.Hphone, b.Juso, b.Memo";

                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = " " + dt.Rows[i]["Sname"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Jumin1"].ToString().Trim() + "-" + dt.Rows[i]["Jumin2"].ToString().Trim();
                        //"YY-MM-DD"
                        ssList_Sheet1.Cells[i, 6].Text = " " + dt.Rows[i]["DeptCode"].ToString().Trim() + " " + VB.Mid(dt.Rows[i]["LastDate"].ToString().Trim(), 3, 2) + VB.Mid(dt.Rows[i]["LastDate"].ToString().Trim(), 5, 6) + " ";
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Tel"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Hphone"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Juso"].ToString().Trim();

                        if (chkaddress.Checked == true)
                        {
                            ssList_Sheet1.Cells[i, 8].Text = " " + dt.Rows[i]["Memo"].ToString().Trim();
                            if (ssList_Sheet1.Cells[i, 8].Text != "")
                            {
                                ssList_Sheet1.Cells[i, 2].Text = "*";
                            }
                            ssList_Sheet1.Cells[i, 0].ForeColor = Color.LightGray;
                        }
                        else
                        {
                            ssList_Sheet1.Cells[i, 0].ForeColor = Color.White;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        void eDelete()
        {
            string strTemp = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int intRowAffected = 0; //변경된 Row 받는 변수
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            for (i = 0; i < ssList_Sheet1.Rows.Count; i++)
            {
                if (ssList_Sheet1.Cells[i, 0].Text == "True")
                {
                    strTemp = ssList_Sheet1.Cells[i, 1].Text;

                    #region ssList_Select(GoSub)
                    SQL = "";
                    SQL += ComNum.VBLF + "Select";
                    SQL += ComNum.VBLF + "  Pano";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_ADDRESS";
                    SQL += ComNum.VBLF + "Where 1=1";
                    SQL += ComNum.VBLF + "      AND PANO = '" + strTemp + "' ";



                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        ComFunc.MsgBox("해당 주소는 환자 마스터 자료입니다.");
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion

                    #region ssList_Delete(GoSub)

                    SQL = "";
                    SQL += ComNum.VBLF + "Delete FROM " + ComNum.DB_PMPA + "ETC_ADDRESS";
                    SQL += ComNum.VBLF + "Where PANO = '" + strTemp + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    try
                    {
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
                        Cursor.Current = Cursors.Default;

                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    #endregion
                }
            }
        }

        void eSave()
        {
            int i = 0;
            int j = 0;

            string strPANO = "";
            string strTel = "";
            string strHphone = "";
            string strJuso = "";
            string strMemo = "";
            string strChange = "";
            string strError = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            ssList_Sheet1.Cells[nRow, 8].Text = txtMemo.Text;
            ssList_Sheet1.Cells[nRow, 10].Text = "Y";

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            strError = "";

            for (i = 0; i < ssList_Sheet1.Rows.Count; i++)
            {
                strPANO = ssList_Sheet1.Cells[i, 1].Text;
                strTel = ssList_Sheet1.Cells[i, 4].Text;
                strHphone = ssList_Sheet1.Cells[i, 5].Text;
                strJuso = ssList_Sheet1.Cells[i, 7].Text;
                strMemo = ssList_Sheet1.Cells[i, 8].Text;
                strChange = ssList_Sheet1.Cells[i, 10].Text;

                if (strChange == "Y")
                {
                    #region SELECT_RTN(GoSub)

                    SQL = "";
                    SQL += ComNum.VBLF + "Select";
                    SQL += ComNum.VBLF + "  PANO";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_ADDRESS";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND PANO = '" + strPANO + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    j = dt.Rows.Count;

                    dt.Dispose();
                    dt = null;

                    #endregion

                    if (j > 0)
                    {
                        #region UPDATE_RTN(GoSub)

                        SQL = "";
                        SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "ETC_Address " + "SET";
                        SQL += ComNum.VBLF + "Tel ='" + strTel + "',";
                        SQL += ComNum.VBLF + "Hphone ='" + strHphone + "',";
                        SQL += ComNum.VBLF + "Juso ='" + strJuso + "', ";
                        SQL += ComNum.VBLF + "Memo ='" + strMemo + "'";
                        SQL += ComNum.VBLF + "WHERE Pano = '" + strPANO + "'";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }


                        #endregion
                    }
                    else
                    {
                        #region INSERT_RTN(GoSub)

                        SQL = "";
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "ETC_Address";
                        SQL += ComNum.VBLF + "(PANO,TEL,HPHONE, JUSO, MEMO)";
                        SQL += ComNum.VBLF + "VALUES(";
                        SQL += ComNum.VBLF + "  '" + strPANO + "',";
                        SQL += ComNum.VBLF + "  '" + strTel + "', ";
                        SQL += ComNum.VBLF + "  '" + strHphone + "',";
                        SQL += ComNum.VBLF + "  '" + strJuso + "',";
                        SQL += ComNum.VBLF + "  '" + strMemo + "'";
                        SQL += ComNum.VBLF + ")";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        #endregion
                    }
                }

                if (strError != "")
                {
                    break;
                }
            }

            if (strError == "")
            {
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("자료가 등록되었습니다.");
                Clear_Screen();
                eGetData();
            }
            else
            {
                ComFunc.MsgBox("등록중 오류가 발생하여 RollBack 합니다.");
                clsDB.setRollbackTran(clsDB.DbCon);
                Clear_Screen();
                eGetData();
            }
        }

        void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssList_Sheet1.Cells[nRow, 8].Text = txtMemo.Text;

            nRow = e.Row;

            txtMemo.Text = ssList_Sheet1.Cells[e.Row, 8].Text;
        }

        void ssList_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            ssList_Sheet1.Cells[e.Row, 10].Text = "Y";
        }
    }
}
