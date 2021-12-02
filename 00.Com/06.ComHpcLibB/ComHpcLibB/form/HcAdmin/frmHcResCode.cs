using ComBase;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB;
using FarPoint.Win.Spread;
using System.Drawing;

namespace ComHpcLibB
{
    /// <summary>
    /// Class Name      : ComHpcLibB
    /// File Name       : frmHcResCode.cs
    /// Description     : 검사 결과값 코드등록
    /// Author          : 김민철
    /// Create Date     : 2018-11-16
    /// Update History  : 
    /// <seealso cref="\Hic\HcCode\HcCode04.frm(FrmResEntry)"/>
    /// </summary>
    public partial class frmHcResCode : Form
    {
        string FstrGubun = string.Empty;

        public frmHcResCode()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.btnCancel.Click        += new EventHandler(eBtnClick);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick    += new CellClickEventHandler(eSpdCellDblClick);
            this.SS2.EditModeOff        += new EventHandler(eSpdEditModeOff);
            this.SS2.ButtonClicked      += new EditorNotifyEventHandler(eSpdBtnClick);
        }
        
        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column == 0)
            {
                clsSpread cSpd = new clsSpread();

                if (SS2.ActiveSheet.Cells[e.Row, 0].Text == "True")
                {
                    cSpd.setSpdForeColor(SS2, e.Row, 0, e.Row, SS2_Sheet1.ColumnCount - 1, Color.Red);
                }
                else
                {
                    if (SS2.ActiveSheet.Cells[e.Row, 6].Text == "Y")
                    {
                        cSpd.setSpdForeColor(SS2, e.Row, 0, e.Row, SS2_Sheet1.ColumnCount - 1, Color.Blue);
                    }
                    else
                    {
                        cSpd.setSpdForeColor(SS2, e.Row, 0, e.Row, SS2_Sheet1.ColumnCount - 1, Color.Black);
                    }
                }
            }
        }

        void eSpdEditModeOff(object sender, EventArgs e)
        {
            int nRow = SS2.ActiveSheet.ActiveRowIndex;

            if (SS2.ActiveSheet.Cells[nRow, 6].Text.Trim() != "Y")
            {
                clsSpread cSpd = new clsSpread();

                SS2.ActiveSheet.Cells[nRow, 6].Text = "Y";
                cSpd.setSpdForeColor(SS2, nRow, 0, nRow, SS2_Sheet1.ColumnCount - 1, Color.Blue);
                cSpd = null;
                
            }
        }
        
        void eSpdCellDblClick(object sender, CellClickEventArgs e)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string strName = string.Empty;
            int i, nREAD = 0;

            try
            {
                clsSpread cSpd = new clsSpread();

                cSpd.Spread_All_Clear(SS2);

                cSpd = null;

                panListDtl.Enabled = true;

                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnExit.Enabled = false;

                FstrGubun = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                strName = SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim();

                lblCode.Text = "▶코드:" + FstrGubun + " " + strName;

                if (FstrGubun == "") { return; }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Code,Name,GbFlag,Mark,ROWID       ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_RESCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                             ";
                SQL = SQL + ComNum.VBLF + "   AND Gubun = '" + FstrGubun + "'       ";
                SQL = SQL + ComNum.VBLF + " ORDER BY Code                           ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                if (nREAD > 0)
                {
                    SS2.ActiveSheet.RowCount = nREAD + 50;

                    for (i = 0; i < nREAD; i++)
                    {
                        SS2.ActiveSheet.Cells[i, 1].Text = Dt.Rows[i]["Code"].ToString().Trim();
                        SS2.ActiveSheet.Cells[i, 2].Text = Dt.Rows[i]["Name"].ToString().Trim();
                        SS2.ActiveSheet.Cells[i, 3].Text = Dt.Rows[i]["GbFlag"].ToString().Trim();
                        SS2.ActiveSheet.Cells[i, 4].Text = Dt.Rows[i]["Mark"].ToString().Trim();
                        SS2.ActiveSheet.Cells[i, 5].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnCancel)
            {
                Screen_Clear();
            }
            else if (sender == btnSave)
            {
                eDataSave(clsDB.DbCon);
            }
        }

        void eDataSave(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int nRow, i = 0;

            string strDel = "";
            string strCode = "";
            string strName = "";
            string strROWID = "";
            string strGbFlag = "";
            string strChange = "";
            string strMark = "";
            string strNewCode = "";

            if (SS2.ActiveSheet.RowCount == 0) { return; }

            try
            {

                for (i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    strDel      = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                    strCode     = SS2.ActiveSheet.Cells[i, 1].Text.Trim();
                    strName     = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                    strGbFlag   = SS2.ActiveSheet.Cells[i, 3].Text.Trim();
                    strMark     = SS2.ActiveSheet.Cells[i, 4].Text.Trim();
                    strROWID    = SS2.ActiveSheet.Cells[i, 5].Text.Trim();
                    
                    if (strDel != "True" && strCode != "")
                    { 
                        if (strCode.Length != 2) { MessageBox.Show("코드를 반드시 2자리로 입력하세요", "오류"); return; }
                        if (strName == "") { MessageBox.Show(i + "번줄 결과값이 공란입니다.", "오류"); return; }
                        if (strMark != "" && strMark != "*") { MessageBox.Show("마크란에는 공란 및 '*' 문자만 가능함", "오류"); return; }
                    }
                }
               

                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(pDbCon);

                nRow = SS2.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1;

                for (i = 0; i < nRow; i++)
                {
                    strDel      = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                    strCode     = SS2.ActiveSheet.Cells[i, 1].Text.Trim();
                    strName     = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                    strGbFlag   = SS2.ActiveSheet.Cells[i, 3].Text.Trim();
                    strMark     = SS2.ActiveSheet.Cells[i, 4].Text.Trim();
                    strROWID    = SS2.ActiveSheet.Cells[i, 5].Text.Trim();
                    strChange   = SS2.ActiveSheet.Cells[i, 6].Text.Trim();

                    strNewCode = FstrGubun + strCode;

                    if (strROWID != "")
                    {
                        if (strDel == "True")
                        {
                            SQL = " DELETE " + ComNum.DB_PMPA + "HIC_RESCODE WHERE ROWID ='" + strROWID + "' ";
                        }
                        else if (strChange == "Y")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "HIC_RESCODE ";
                            SQL += ComNum.VBLF + "    SET Code  = '" + strCode + "',        ";
                            SQL += ComNum.VBLF + "        Name  = '" + strName + "',        ";
                            SQL += ComNum.VBLF + "        GbFlag= '" + strGbFlag + "',      ";
                            SQL += ComNum.VBLF + "        Mark  = '" + strMark + "'         ";
                            SQL += ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "'        ";
                        }
                    }
                    else
                    {
                        if (strDel != "True" && strCode != "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "HIC_RESCODE                    ";
                            SQL += ComNum.VBLF + "        (Gubun,Code,Name,GbFlag,Mark)                             ";
                            SQL += ComNum.VBLF + " VALUES (                                                         ";
                            SQL += ComNum.VBLF + "        '" + FstrGubun + "', '" + strCode + "', '" + strName + "',";
                            SQL += ComNum.VBLF + "        '" + strGbFlag + "', '" + strMark + "' )                  ";
                        }
                    }

                    if (SQL != "")
                    {
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            this.btnSave.Enabled = true;
                            this.btnCancel.Enabled = true;
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }


                clsDB.setCommitTran(pDbCon);
                
                Cursor.Current = Cursors.Default;

                Screen_Clear();
                
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {

            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            int i = 0;
            
            Screen_Clear();
            
            try
            {
                clsSpread cSpd = new clsSpread();

                cSpd.Spread_All_Clear(SS1);

                cSpd = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Code,Name                         ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "HIC_CODE    ";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1                             ";
                SQL = SQL + ComNum.VBLF + "   AND Gubun = '17'                      ";
                SQL = SQL + ComNum.VBLF + "   AND (GBDEL IS NULL OR GBDEL = '')     ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Code                            ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    SS1.ActiveSheet.RowCount = Dt.Rows.Count;

                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = Dt.Rows[i]["Code"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 1].Text = Dt.Rows[i]["Name"].ToString().Trim();
                    }
                }

                Dt.Dispose();
                Dt = null;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        void Screen_Clear()
        {
            clsSpread cSpd = new clsSpread();
            
            cSpd.Spread_All_Clear(SS2);
            lblCode.Text = "";

            cSpd = null;

            panListDtl.Enabled = false;

            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
        }
        
    }
}
