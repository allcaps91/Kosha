using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;
using System.IO;

namespace ComSupLibB
{
    /// <summary>
    /// Class Name      : MedIpdNr
    /// File Name       : frmOpdCSRGureq.cs
    /// Description     : 공급실 물품 청구
    /// Author          : 김욱동
    /// Create Date     : 2021-05-07
    /// TODO :
    /// 3. 테스트 필요
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 Frm공급실물품청구 폼 frmOpdCSRGureq.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrinfo\Frm공급실물품청구.frm(Frm공급실물품청구) >> frmOpdCSRGureq.cs 폼이름 재정의" />
    public partial class frmOpdCSRGureq : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        //clsIpdNr CIN = new clsIpdNr();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;
        DataTable dt1 = null;

        string FstrMaster = "";
        int intRowAffected = 0; //변경된 Row 받는 변수

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

        public frmOpdCSRGureq(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmOpdCSRGureq()
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

            this.btnPrint.Click += new EventHandler(eBtnPrint);

            this.mnuExit.Click += new EventHandler(eMenu_Click);
            this.mnuSearch.Click += new EventHandler(eMenu_Click);
            this.mnuEOGas.Click += new EventHandler(eMenu_Click);
            this.mnuOcsActing.Click += new EventHandler(eMenu_Click);


            this.btnPrint.Click += new EventHandler(eBtnPrint);

            this.dtpDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            //this.eControl.LostFocus += new EventHandler(eControl_LostFocus);
            //this.eControl.GotFocus += new EventHandler(eControl_GotFocus);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //else
            //{
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                ComFunc.ReadSysDate(clsDB.DbCon);

                Set_Init();
            // }
            ssList_Sheet1.Columns[5].Visible = false;
            ssList_Sheet1.Columns[6].Visible = false;
            ssList_Sheet1.Columns[7].Visible = false;
            ssList_Sheet1.Columns[8].Visible = false;

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
                //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                //{
                //    return; //권한 확인
                //}
                eGetData();
            }

            else if (sender == this.btnSave)
            {
                //if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                //{
                //    return; //권한 확인
                //}
                eSaveData();
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                //{
                //    return; //권한 확인
                //}
                ePrint();
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.dtpDate)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eMenu_Click(object sender, EventArgs e)
        {
            if (sender == this.mnuExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.mnuSearch)
            {
               frmOpdCSRGureqSearch frm = new frmOpdCSRGureqSearch();
               frm.ShowDialog();
            }
            else if (sender == this.mnuEOGas)
            {
                frmOpdEOgasSearch frm = new frmOpdEOgasSearch();
                frm.ShowDialog();
            }
            else if (sender == this.mnuOcsActing)
            {
                frmSupCSROcsActing frm = new frmSupCSROcsActing();
                frm.ShowDialog();
            }

        }

        void ssList_Change(object sender, ChangeEventArgs e)
        {
            ssList.ActiveSheet.Cells[e.Row, 8].Text = "Y";
        }

        void Set_Init()
        {
            ComboWard_SET();

            
            ssList.ActiveSheet.Columns[5].Visible = false;  //ROWID
            ssList.ActiveSheet.Columns[6].Visible = false;  //4차
            ssList.ActiveSheet.Columns[7].Visible = false;  //5차
            ssList.ActiveSheet.Columns[8].Visible = false;  //수정

            dtpDate.Text = clsPublic.GstrSysDate;

            //TEST
            //clsPublic.GstrWardCodes = "CSR";

            Time_CHK();

            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnPrint.Enabled = false;

            FstrMaster = "";
            
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "(" + VB.Pstr(cboWard.SelectedItem.ToString().Trim(), ".", 1) + ") 멸균 및 소모물품 청구 내역";
            strSubTitle = "작업일자 : " + dtpDate.Text;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 13, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, false, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void eSaveData()
        {
            int i = 0;
            int j = 0;
            int nRead = 0;

            string strJEPCODE = "";

            int nReq4 = 0;
            int nReq5 = 0;

            int nOldReq4 = 0;
            int nOldReq5 = 0;

            string strRDate = "";
            string strBuCode = "";
            string strChange = "";
            string strEdit = "";
            string strROWID = "";

            long[] nData = new long[32];
            string strDay = "";
            long nOldQty = 0;
            long nSum = 0;

            //청구일자
            strRDate = dtpDate.Value.ToString("yyyy-MM-dd");

            //부서코드
            if (cboWard.Text.Trim() == "")
            {
                return;
            }
            else
            {
                strBuCode = VB.Pstr(cboWard.Text.Trim().Trim(), ".", 2);
            }

            //점검
            if (FstrMaster != "Y")
            {
                if (String.Compare(strRDate, clsPublic.GstrSysDate) < 0)
                {
                    ComFunc.MsgBox("당일 이전일은 저장할 수 없습니다.");
                    return;
                }
            }

            if (strBuCode == "" || strBuCode == "전체")
            {
                ComFunc.MsgBox("부서가 공란이거나 전체일 경우 저장할 수 없습니다.");
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            Cursor.Current = Cursors.WaitCursor;

            
            strDay = VB.Right(dtpDate.Text, 2);
            strEdit = "";

            try
            {
                for (i = 0; i <= ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    strJEPCODE = ssList.ActiveSheet.Cells[i, 0].Text;
                    nReq4 = Convert.ToInt32(VB.Val(ssList.ActiveSheet.Cells[i, 2].Text));
                    nReq5 = Convert.ToInt32(VB.Val(ssList.ActiveSheet.Cells[i, 3].Text));

                    strROWID = ssList.ActiveSheet.Cells[i, 5].Text;
                    nOldReq4 = Convert.ToInt32(VB.Val(ssList.ActiveSheet.Cells[i, 6].Text));
                    nOldReq5 = Convert.ToInt32(VB.Val(ssList.ActiveSheet.Cells[i, 7].Text));
                    strEdit = ssList.ActiveSheet.Cells[i, 8].Text;

                    //물품청구가 바뀌었는지 점검
                    strChange = "";
                    if (nReq4 != nOldReq4)
                    {
                        strChange = "OK";
                    }

                    if (nReq5 != nOldReq5)
                    {
                        strChange = "OK";
                    }

                    if (strEdit == "Y")
                    {
                        strChange = "OK";
                    }

                    //ROWID가 공란이면 새로 저장합니다.
                    if (strROWID == "")
                    {
                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_ERP + "CSR_REQ (RDate,BuCode,JepCode,Req1,Req2,Req3,Req4,Req5,JobSabun, EntTime) ";
                        SQL = SQL + ComNum.VBLF + "  VALUES  ( ";
                        SQL = SQL + ComNum.VBLF + " TO_DATE('" + strRDate + "','YYYY-MM-DD') ,'" + strBuCode + "' ,";
                        SQL = SQL + ComNum.VBLF + " '" + strJEPCODE + "','0','0','0', " + nReq4 + ", " + nReq5 + ",  ";
                        SQL = SQL + ComNum.VBLF + " " + clsType.User.Sabun + " , SYSDATE )  ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox(SqlErr);
                            ComFunc.MsgBox("신규등록시 오류가 발생함");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    //ROWID가 있거나 , 청구수량이 틀릴경우만 업데이트
                    else
                    {
                        if (strChange == "OK")
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_ERP + "CSR_REQ SET ";
                            SQL = SQL + ComNum.VBLF + " Rdate=TO_DATE('" + strRDate + "','YYYY-MM-DD'), ";
                            SQL = SQL + ComNum.VBLF + " BuCode = '" + strBuCode + "' , ";
                            SQL = SQL + ComNum.VBLF + " JepCode='" + strJEPCODE + "', ";
                            SQL = SQL + ComNum.VBLF + " Req4=" + nReq4 + ", ";
                            SQL = SQL + ComNum.VBLF + " Req5=" + nReq5 + ", ";                            
                            SQL = SQL + ComNum.VBLF + " JobSabun=" + clsType.User.Sabun + ", ";
                            SQL = SQL + ComNum.VBLF + " EntTime = TO_DATE('" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "','YYYY-MM-DD HH24:MI')  ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                ComFunc.MsgBox(SqlErr);
                                ComFunc.MsgBox("UPDATE시 오류가 발생함");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                }

                //===============================================================================================================


                //공급실 일보에 업데이트

                strRDate = VB.Left(dtpDate.Value.ToString("yyyy-MM-dd"), 4) + VB.Mid(dtpDate.Value.ToString("yyyy-MM-dd"), 6, 2);
                strEdit = "";
                for (i = 0; i <= ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    //변수 Clear
                    for (j = 0; j < nData.Length; j++)
                    {
                        nData[j] = 0;
                    }

                    strROWID = "";
                    strJEPCODE = ssList.ActiveSheet.Cells[i, 0].Text;

                    //기존 데이터 조회
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  YYMM,BUCODE,JEPCODE,GUBUN,DAY01,DAY02,DAY03,DAY04,DAY05,DAY06,DAY07,";
                    SQL += ComNum.VBLF + "  DAY08,DAY09,DAY10,DAY11,DAY12,DAY13,DAY14,DAY15,DAY16,DAY17,DAY18,DAY19,DAY20,DAY21,";
                    SQL += ComNum.VBLF + "  DAY22 , DAY23, DAY24, DAY25, DAY26, DAY27, DAY28, DAY29, DAY30, DAY31, DAYTOT, ROWID";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "CSR_ILBO";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND YYMM ='" + strRDate + "'";
                    SQL += ComNum.VBLF + "      AND BUCODE ='" + strBuCode + "'";
                    SQL += ComNum.VBLF + "      AND JEPCODE ='" + strJEPCODE + "'";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        nRead = dt.Rows.Count;

                        for (j = 1; j < nData.Length; j++)
                        {
                            nData[j] = Convert.ToInt64(VB.Val(dt.Rows[0]["DAY" + ComFunc.SetAutoZero(j.ToString(), 2)].ToString().Trim()));
                        }
                        strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    //변경값 READ
                    strJEPCODE = ssList.ActiveSheet.Cells[i, 0].Text;
                    nReq4 = Convert.ToInt32(VB.Val(ssList.ActiveSheet.Cells[i, 2].Text));
                    nReq5 = Convert.ToInt32(VB.Val(ssList.ActiveSheet.Cells[i, 3].Text));
                    nOldReq4 = Convert.ToInt32(VB.Val(ssList.ActiveSheet.Cells[i, 6].Text));
                    nOldReq5 = Convert.ToInt32(VB.Val(ssList.ActiveSheet.Cells[i, 7].Text));
                    strEdit = ssList.ActiveSheet.Cells[i, 8].Text;

                    nData[Convert.ToInt32(strDay)] = Convert.ToInt64(nReq4 + nReq5);
                    nOldQty = nOldReq4 + nOldReq5;

                    nSum = 0;

                    for (j = 1; j < nData.Length; j++)
                    {
                        nSum = nSum + nData[j];
                    }

                    //자료변경이 있을경우
                    if (strROWID == "")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_ERP + "CSR_ILBO";
                        SQL += ComNum.VBLF + "VALUES( ";
                        SQL += ComNum.VBLF + "  '" + strRDate + "','" + strBuCode + "',";
                        SQL += ComNum.VBLF + " '" + strJEPCODE + "', '3', ";

                        for (j = 1; j < nData.Length; j++)
                        {
                            SQL += ComNum.VBLF + nData[j] + ", ";
                        }

                        SQL += ComNum.VBLF + nSum + " ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        if (strEdit == "Y")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_ERP + "CSR_ILBO";
                            SQL += ComNum.VBLF + "SET";

                            for (j = 1; j < nData.Length; j++)
                            {
                                SQL += ComNum.VBLF + "DAY" + ComFunc.SetAutoZero(j.ToString(), 2) + "=" + nData[j] + ",";
                            }

                            SQL += ComNum.VBLF + "DAYTOT = " + nSum;
                            SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                        }
                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
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

            ComFunc.MsgBox("저장하였습니다.");
            CS.Spread_All_Clear(ssList);
            eGetData();

        }

        void eGetData()
        {
            int i = 0;
            int nRow = 0;
            int nRead = 0;
            

            string strBucode = VB.Pstr(cboWard.SelectedItem.ToString().Trim(), ".", 2);

            btnSave.Enabled = true;

            //해당일만 저장 가능
            if (FstrMaster != "Y")
            {
                if (String.Compare(dtpDate.Text, clsPublic.GstrSysDate) < 0)
                {
                    btnSave.Enabled = false;
                }
            }

            btnPrint.Enabled = true;

            CS.Spread_All_Clear(ssList);


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region 해당일조회
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  JepCode";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "CSR_REQ";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND RDate =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND BuCode ='" + strBucode + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                dt.Dispose();
                dt = null;

                #endregion

             

                #region 부서기초코드세팅된것 조회

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  b.GbCSR,a.JepCode,b.CsrName";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "CSR_BUSEJEP a, " + ComNum.DB_ERP + "ORD_JEP b";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND BuCode ='" + strBucode + "'";
                SQL += ComNum.VBLF + "      AND a.JepCode=b.JepCode(+)";
                SQL += ComNum.VBLF + "      AND b.GbCSR IS NOT NULL";
                SQL += ComNum.VBLF + "      AND b.DelDate IS NULL";
                SQL += ComNum.VBLF + "ORDER BY a.SortNo,b.GbCSR,a.JepCode";

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
                    nRead = dt.Rows.Count;
                    ssList.ActiveSheet.Rows.Count = nRead;

                    for (i = 0; i < nRead; i++)
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  BuCode, Req4, Req5, Req4 + Req5 as SReq, ROWID";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "CSR_REQ";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND BuCode ='" + strBucode + "'";
                        SQL += ComNum.VBLF + "      AND RDate =TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND JepCode ='" + dt.Rows[i]["JepCode"].ToString().Trim() + "'";

                        if (chkView.Checked == true)
                        {
                            SQL += ComNum.VBLF + "  AND ( REQ4 > 0 OR REQ5 > 0 ) ";
                        }

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        //청구물품만                   
                        if (chkView.Checked == true)
                        {
                            if (dt1.Rows.Count > 0)
                            {
                                nRow += 1;

                                ssList.ActiveSheet.Cells[nRow - 1, 0].Text = dt.Rows[i]["JepCode"].ToString().Trim();
                                ssList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["CsrName"].ToString().Trim();
                            }
                        }
                        else
                        {
                            nRow += 1;
                            ssList.ActiveSheet.Cells[nRow - 1, 0].Text = dt.Rows[i]["JepCode"].ToString().Trim();
                            ssList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["CsrName"].ToString().Trim();
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 2].Text = VB.Val(dt1.Rows[0]["Req4"].ToString().Trim()).ToString();
                            ssList.ActiveSheet.Cells[nRow - 1, 3].Text = VB.Val(dt1.Rows[0]["Req5"].ToString().Trim()).ToString();
                            ssList.ActiveSheet.Cells[nRow - 1, 4].Text = VB.Val(dt1.Rows[0]["SReq"].ToString().Trim()).ToString();

                            //기존 Req
                            ssList.ActiveSheet.Cells[nRow - 1, 5].Text = dt1.Rows[0]["ROWID"].ToString().Trim();
                            ssList.ActiveSheet.Cells[nRow - 1, 6].Text = VB.Val(dt1.Rows[0]["Req4"].ToString().Trim()).ToString();
                            ssList.ActiveSheet.Cells[nRow - 1, 7].Text = VB.Val(dt1.Rows[0]["Req5"].ToString().Trim()).ToString();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }

                dt.Dispose();
                dt = null;
                #endregion

                //시간대별 마감 체크
                Req_Enable();
                ssList.ActiveSheet.Rows.Count = nRow;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }


        }

        void Time_CHK()
        {
            DataTable dt2 = null;
            string strWeek = "";

            strWeek = ComFunc.LeftH(CF.READ_YOIL(clsDB.DbCon, dtpDate.Text), 2);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (strWeek != "일")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  HolyDay";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_JOB";
                    SQL += ComNum.VBLF + "WHERE JobDate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";

                    SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        if (dt2.Rows[0]["HolyDay"].ToString().Trim() == "*")
                        {
                            strWeek = "휴";
                        }
                    }

                    dt2.Dispose();
                    dt2 = null;
                }

                switch (strWeek)
                {
                    case "토":
                        txtSts.Text = "토요일 마감 시간 [ 4차 : 12:10  5차 : 12:30 ] 마감 시간 이후 입력불가능합니다.";
                        break;

                    default:
                        txtSts.Text = "평일 마감 시간 [ 4차 : 17:00  5차 : 17:30 ] 마감 시간 이후 입력불가능합니다.";
                        break;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        void Req_Enable()
        {
            string strWeek = "";

            //4차, 5차 UnLock
            ssList.ActiveSheet.Cells[0, 2, ssList.ActiveSheet.Rows.Count - 1, 7].Locked = false;

            #region UnLock_Cell

            ssList.ActiveSheet.Cells[0, 2, ssList.ActiveSheet.Rows.Count - 1, 7].BackColor = Color.White;

            #endregion

            if (String.Compare(dtpDate.Text, clsPublic.GstrSysDate) > 0)
            {
                return;
            }

            if (FstrMaster == "Y")
            {
                return;
            }

            strWeek = ComFunc.LeftH(CF.READ_YOIL(clsDB.DbCon, dtpDate.Text), 2);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (strWeek != "일")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  HolyDay";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_JOB";
                    SQL += ComNum.VBLF + "WHERE JobDate=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["HolyDay"].ToString().Trim() == "*")
                        {
                            strWeek = "휴";
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            

            if (strWeek == "일" || strWeek == "휴")
            {
                    ssList.ActiveSheet.Cells[0, 2, ssList.ActiveSheet.Rows.Count - 1, 3].Locked = true;
                    //Lcok Cell
                    ssList.ActiveSheet.Cells[0, 2, ssList.ActiveSheet.Rows.Count - 1, 3].BackColor = Color.Green;
            }

            else if (strWeek == "토")
            {
                if (String.Compare(clsPublic.GstrSysTime, "12:10") >= 0 && String.Compare(clsPublic.GstrSysDate, "12:30") <= 0)
                {
                    ssList.ActiveSheet.Cells[0, 2, ssList.ActiveSheet.Rows.Count - 1, 2].Locked = true;

                    //Lcok Cell
                    ssList.ActiveSheet.Cells[0, 2, ssList.ActiveSheet.Rows.Count - 1, 2].BackColor = Color.Green;
                }

                else if (String.Compare(clsPublic.GstrSysTime, "12:30") >= 0 && String.Compare(clsPublic.GstrSysDate, "12:35") <= 0)
                {
                    ssList.ActiveSheet.Cells[0, 2, ssList.ActiveSheet.Rows.Count - 1, 3].Locked = true;

                    //Lcok Cell
                    ssList.ActiveSheet.Cells[0, 2, ssList.ActiveSheet.Rows.Count - 1, 3].BackColor = Color.Green;
                }
            }

            //평일
            else
            {
                if (String.Compare(clsPublic.GstrSysTime, "17:00") >= 0 && String.Compare(clsPublic.GstrSysDate, "17:30") <= 0)
                {
                    ssList.ActiveSheet.Cells[0, 2, ssList.ActiveSheet.Rows.Count - 1, 2].Locked = true;

                    //Lcok Cell
                    ssList.ActiveSheet.Cells[0, 2, ssList.ActiveSheet.Rows.Count - 1, 2].BackColor = Color.Green;
                }

                else if (String.Compare(clsPublic.GstrSysTime, "17:30") >= 0 && String.Compare(clsPublic.GstrSysDate, "23:59") <= 0)
                {
                    ssList.ActiveSheet.Cells[0, 2, ssList.ActiveSheet.Rows.Count - 1, 3].Locked = true;

                    //Lcok Cell
                    ssList.ActiveSheet.Cells[0, 2, ssList.ActiveSheet.Rows.Count - 1, 3].BackColor = Color.Green;
                }
            }
        }

        void ComboWard_SET()
        {
            int i = 0;
            int intIdxCbo = 0;
            string Bucode = "";
            Bucode = File.Exists(@"c:\cmc\gumebuse.dat") ? File.ReadAllText(@"c:\cmc\gumebuse.dat").Split('.')[0] : clsType.User.BuseCode;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  BuCode,Name";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUSE";
                SQL += ComNum.VBLF + "WHERE ORDFLAG = 'Y' ";
                if (Bucode == "033102" || Bucode == "033103")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033102','033103') ";
                }
                else if (Bucode == "033113" || Bucode == "033125")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033113','033125') ";
                }
                else if (Bucode == "033122" || Bucode == "033126")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033122','033126') ";
                }
                else if (Bucode == "033123" || Bucode == "033104")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033123','033104') ";
                }
                else if (Bucode == "033118" || Bucode == "033108")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033118','033108') ";
                }
                else if (Bucode == "055101")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('055101','044510','044520','100570') ";
                }
                else if (Bucode == "056102")
                {
                    SQL += ComNum.VBLF + " AND BuCode in ('011101','011102','011103','011104','011105','011106','011109','011110', ";
                   SQL += ComNum.VBLF + " '011111','011112','011113','011114','011117','011116','011124','011129','033140','056102','056104','056101', ";
                   SQL += ComNum.VBLF + " '100130','100070','100090','100150','100110', '011150')            ";
                                   }
                else if (Bucode != "******")
                {
                    SQL += ComNum.VBLF + "AND BuCode = '" + Bucode + "' ";
                }
                SQL += ComNum.VBLF + "ORDER BY BuCode";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    cboWard.Items.Clear();
                    cboWard.Items.Add("전체");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["Name"].ToString().Trim() + "." + dt.Rows[i]["BuCode"].ToString().Trim());

                        if (dt.Rows[i]["BuCode"].ToString().Trim() == Bucode)
                        {
                            intIdxCbo = i + 1;
                        }


                    }
                }

                dt.Dispose();
                dt = null;

                cboWard.SelectedIndex = intIdxCbo;

                if (intIdxCbo > 0)
                {
                    cboWard.Enabled = false;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }
    }
}

