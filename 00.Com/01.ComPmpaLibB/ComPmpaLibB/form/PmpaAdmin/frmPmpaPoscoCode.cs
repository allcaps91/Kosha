using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaPoscoCode : Form
    {
        clsSpread cSpd = new clsSpread();
        clsComPmpaSpd cPmpaSpd = new clsComPmpaSpd();
        ComFunc CF = new ComFunc();
        clsPmpaFunc cPF = new clsPmpaFunc();
        clsPmpaMisu cPM = new clsPmpaMisu();
        Card CARD = new Card();
        clsPmpaPb cPb = new clsPmpaPb();

        public frmPmpaPoscoCode()
        {
            InitializeComponent();
            SetEvent();
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            cSpd.Spread_All_Clear(SS_Posco);
            ComboGubun_SET(clsDB.DbCon, cboGubun, "00000", true, 1, "");

        }
        private void ComboGubun_SET(PsmhDb pDbCon, ComboBox ArgCombobox, string argGubun, bool ArgClear, int ArgTYPE, string ArgNULL = "")
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if (ArgClear == true)
            {
                ArgCombobox.Items.Clear();
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                               ";
            SQL = SQL + ComNum.VBLF + "     Code,Name                                  ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_BCODE                 ";
            SQL = SQL + ComNum.VBLF + "     WHERE 1=1                                       ";
            SQL = SQL + ComNum.VBLF + "   AND Gubun = '" + argGubun + "'                    ";
            SQL = SQL + ComNum.VBLF + "   AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
            SQL = SQL + ComNum.VBLF + "ORDER BY Sort,Code                                   ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (ArgNULL != "N")
            {
                ArgCombobox.Items.Add("00000.목록");
            }

            if (dt.Rows.Count > 0)
            {

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (ArgTYPE == 1)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                    }

                    else if (ArgTYPE == 2)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Code"].ToString().Trim());
                    }

                    else if (ArgTYPE == 3)
                    {
                        ArgCombobox.Items.Add(dt.Rows[i]["Name"].ToString().Trim());
                    }
                }
            }
            else
            {
                ComFunc.MsgBox("조회중 문제가 발생하였습니다.");
                return;
            }
            dt.Dispose();
            dt = null;
            ArgCombobox.SelectedIndex = 0;
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);

            this.CmdExit.Click += new EventHandler(eBtnClick);
            this.CmdView.Click += new EventHandler(eBtnClick);
            this.CmdPrint.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.CmdSave.Click += new EventHandler(eBtnClick);
        }

        private void Posco_Save()
        {
            int i = 0;
            bool bDel = false;
            string strGubun = "";
            string strCode = "";
            string strName = "";
            string strJDate = "";
            string strRowid = "";
            string strChange = "";
            long nAmt = 0;
            string strName2 = "";
            string strDeldate = "";
            string strPart = "";
            
            int nCol = 0;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

           
            //string SQL = "";
      

            strGubun = VB.Trim(VB.Pstr(cboGubun.Text, ".", 1));

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                for (i = 0; i < SS_Posco_Sheet1.Rows.Count; i++)
                {
                    bDel = Convert.ToBoolean(SS_Posco_Sheet1.Cells[i, 0].Value);
                    strCode = SS_Posco_Sheet1.Cells[i, 1].Text;
                    strName = SS_Posco_Sheet1.Cells[i, 2].Text;
                    strName2 = SS_Posco_Sheet1.Cells[i, 3].Text;
                    nAmt = Convert.ToInt32(VB.Val(SS_Posco_Sheet1.Cells[i, 4].Text));
                    strJDate = SS_Posco_Sheet1.Cells[i, 5].Text;
                    strDeldate = SS_Posco_Sheet1.Cells[i, 6].Text;
                    strPart = SS_Posco_Sheet1.Cells[i, 7].Text;
                    strChange = SS_Posco_Sheet1.Cells[i, 8].Text;
                    strRowid = SS_Posco_Sheet1.Cells[i, 9].Text;
                    nCol = Convert.ToInt32(VB.Val(SS_Posco_Sheet1.Cells[i, 10].Text));

                    if (bDel)
                    {
                        if (strRowid != "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "ETC_BCODE ";
                            SQL += ComNum.VBLF + "  WHERE ROWID ='" + strRowid + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return ;
                            }
                        }
                       
                   
                    }
                    else if (strChange == "Y")
                    {
                        if (strRowid == "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_BCODE (Gubun,Code,Name,JDate,AMT,Part,Name2,DelDate,EntSabun,EntDate,SCol ) ";
                            SQL += ComNum.VBLF + " VALUES ('" + strGubun + "','" + strCode + "','" + strName + "', TO_DATE('" + strJDate + "', 'YYYY-MM-DD')" + "," + nAmt + ", ";
                            SQL += ComNum.VBLF + " '" + strPart + "','" + strName2 + "', TO_DATE('" + strDeldate + "', 'YYYY-MM-DD'),'" + clsType.User.IdNumber + "',sysdate, " + nCol + ") ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                        }
                        else
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_BCODE ";
                            SQL += ComNum.VBLF + "    SET Code ='" + strCode + "', ";
                            SQL += ComNum.VBLF + "        Name ='" + strName + "', ";
                            SQL += ComNum.VBLF + "        Name2 ='" + strName2 + "', ";
                            SQL += ComNum.VBLF + "        AMT =" + nAmt + ", ";
                            SQL += ComNum.VBLF + "        SCol =" + nCol + ", ";
                            SQL += ComNum.VBLF + "        JDate = TO_DATE('" + strJDate + "','YYYY-MM-DD'), ";
                            SQL += ComNum.VBLF + "        Part ='" + strPart + "', ";
                            SQL += ComNum.VBLF + "        DelDate = TO_DATE('" + strDeldate + "','YYYY-MM-DD'), ";
                            SQL += ComNum.VBLF + "        EntSabun ='" + clsType.User.IdNumber + "', ";
                            SQL += ComNum.VBLF + "        EntDate = sysdate ";
                            SQL += ComNum.VBLF + "  WHERE ROWID ='" + strRowid + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                        }
                    }
                   

                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;


                cSpd.Spread_All_Clear(SS_Posco);

                return;

            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }
        private void Screen_Display()
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";
            int i = 0 ,nRead = 0, nRow = 0;
            string strGubun = "";

            cSpd.Spread_All_Clear(SS_Posco);
            strGubun = VB.Trim(VB.Pstr(cboGubun.Text, ".", 1));

            SS_Posco_Sheet1.Rows.Count = 0;
            try
            {
                

                SQL = "";
                SQL += ComNum.VBLF + " SELECT Code,Name,Name2,AMT,TO_CHAR(JDate,'YYYY-MM-DD') JDate,TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,Part,SCol,ROWID ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_BCODE " ;
                SQL += ComNum.VBLF + "  WHERE Gubun='" + strGubun + "' ";
                if (chkSel.Checked !=true)
                {
                    SQL += ComNum.VBLF + "    AND  DelDate IS NULL " ;
                }
                SQL += ComNum.VBLF + "  ORDER BY sort,Code  ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                nRead = Dt.Rows.Count;

                if (nRead > 0)
                {
                    for (i = 0; i < nRead; i++)
                    {
                        nRow += 1;
                        if (SS_Posco_Sheet1.Rows.Count < nRow)
                        {
                            SS_Posco_Sheet1.Rows.Count = nRow +10;
                        }

                        SS_Posco_Sheet1.Cells[nRow - 1, 0].Text = "";
                        SS_Posco_Sheet1.Cells[nRow - 1, 1].Text = Dt.Rows[i]["Code"].ToString().Trim();
                        SS_Posco_Sheet1.Cells[nRow - 1, 2].Text = Dt.Rows[i]["Name"].ToString().Trim();
                        SS_Posco_Sheet1.Cells[nRow - 1, 3].Text = Dt.Rows[i]["Name2"].ToString().Trim();
                        SS_Posco_Sheet1.Cells[nRow - 1, 4].Text = Dt.Rows[i]["AMT"].ToString().Trim();
                        if (strGubun != "00000")
                        {
                            SS_Posco_Sheet1.Cells[nRow - 1, 5].Text = Dt.Rows[i]["JDate"].ToString().Trim();
                            SS_Posco_Sheet1.Cells[nRow - 1, 6].Text = Dt.Rows[i]["DelDate"].ToString().Trim();
                        }
                        SS_Posco_Sheet1.Cells[nRow - 1, 7].Text = Dt.Rows[i]["Part"].ToString().Trim();
                        SS_Posco_Sheet1.Cells[nRow - 1, 8].Text = Dt.Rows[i]["Part"].ToString().Trim();
                        SS_Posco_Sheet1.Cells[nRow - 1, 9].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                        SS_Posco_Sheet1.Cells[nRow - 1, 10].Text = Dt.Rows[i]["SCOL"].ToString().Trim();

                    }
                }

                Dt.Dispose();
                Dt = null;
               

            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.CmdExit)             //닫기
            {
                this.Close();
            }
            else if (sender == this.CmdView)      //조회
            {
                Screen_Display();
            }
           
            else if (sender == this.CmdSave)       //포스코항목 저장
            {
                Posco_Save();
            }
            else if (sender == this.btnCancel)       //포스코항목 저장
            {
                cSpd.Spread_All_Clear(SS_Posco);
            }

        }

        private void CmdPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = cboGubun.Text + "자료사전";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
          
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(SS_Posco, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void SS_Posco_Change(object sender, ChangeEventArgs e)
        {
            String strData = "";

            int Row = SS_Posco_Sheet1.ActiveRowIndex;
            int Column = SS_Posco_Sheet1.ActiveColumnIndex;
            int NewCol = SS_Posco_Sheet1.ActiveColumnIndex + 1;

            if (Column == 0 && Column > 8) { return; }
            SS_Posco_Sheet1.Cells[Row, 8].Text = "Y";

        }
    }
}
