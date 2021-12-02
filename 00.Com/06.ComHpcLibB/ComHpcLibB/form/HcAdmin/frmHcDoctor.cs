using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB;
using FarPoint.Win.Spread;

namespace ComHpcLibB
{
    /// <summary>
    /// Class Name      : ComHpcLibB
    /// File Name       : frmHcDoctor.cs
    /// Description     : 판정의사 코드관리
    /// Author          : 김민철
    /// Create Date     : 2018-11-21
    /// Update History  : 
    /// <seealso cref="\Hic\HcCode\HcCode18.frm(FrmDoctor)"/>
    /// </summary>
    public partial class frmHcDoctor : Form
    {
        //TODO: 의사별 판정구분 세분화 작업 필요 (ex: 암, 특수, 학생검진, 진단서 등...)
        public frmHcDoctor()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            this.Load               += new EventHandler(eFormLoad);
            this.btnExit.Click      += new EventHandler(eBtnClick);
            this.btnSave.Click      += new EventHandler(eBtnClick);
            this.btnSearch.Click    += new EventHandler(eBtnClick);
            this.SS1.EditModeOff    += new EventHandler(eSpdEditModeOff);
            this.SS1.ButtonClicked  += new EditorNotifyEventHandler(eSpdBtnClick);
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column == 0)
            {
                clsSpread cSpd = new clsSpread();

                if (SS1.ActiveSheet.Cells[e.Row, 0].Text == "True")
                {
                    cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, Color.Red);
                }
                else
                {
                    if (SS1.ActiveSheet.Cells[e.Row, 12].Text == "Y")
                    {
                        cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, Color.Blue);
                    }
                    else
                    {
                        cSpd.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, Color.Black);
                    }
                }
            }
        }

        void eSpdEditModeOff(object sender, EventArgs e)
        {
            int nRow = SS1.ActiveSheet.ActiveRowIndex;

            if (SS1.ActiveSheet.Cells[nRow, 12].Text.Trim() != "Y")
            {
                clsSpread cSpd = new clsSpread();

                SS1.ActiveSheet.Cells[nRow, 12].Text = "Y";
                cSpd.setSpdForeColor(SS1, nRow, 0, nRow, SS1_Sheet1.ColumnCount - 1, Color.Blue);
                cSpd = null;

            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                eSave(clsDB.DbCon);
            }
            else if (sender == btnSearch)
            {
                clsSpread cSpd = new clsSpread();

                cSpd.Spread_All_Clear(SS1);

                cSpd = null;

                Screen_Display(chkOut.Checked);
            }
        }

        void eSave(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int nRow, i = 0;

            string strDel       = string.Empty;
            string strSaBun     = string.Empty;
            string strDrname    = string.Empty;
            string strIpsaDay   = string.Empty;
            string strReDay     = string.Empty;
            long nLicence       = 0;
            string strRoom      = string.Empty;
            string strPan       = string.Empty;
            string strHea       = string.Empty;
            string strDent      = string.Empty;
            string strDrCode    = string.Empty;
            string strROWID     = string.Empty;

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(pDbCon);

                nRow = SS1.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1;

                for (i = 0; i < nRow; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 12].Text.Trim() == "Y")
                    {
                        strDel = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                        strSaBun = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                        strDrname = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                        strIpsaDay = SS1.ActiveSheet.Cells[i, 3].Text.Trim();
                        strReDay = SS1.ActiveSheet.Cells[i, 4].Text.Trim();
                        nLicence = (long)VB.Val(SS1.ActiveSheet.Cells[i, 5].Text.Trim());
                        strRoom = SS1.ActiveSheet.Cells[i, 6].Text.Trim();
                        strPan = SS1.ActiveSheet.Cells[i, 7].Text.Trim() == "True" ? "1" : "0";
                        strHea = SS1.ActiveSheet.Cells[i, 8].Text.Trim();
                        strDent = SS1.ActiveSheet.Cells[i, 9].Text.Trim();
                        strDrCode = SS1.ActiveSheet.Cells[i, 10].Text.Trim();
                        strROWID = SS1.ActiveSheet.Cells[i, 11].Text.Trim();

                        if (strSaBun != "")
                        {
                            strSaBun = VB.Format(VB.Val(strSaBun), "00000");

                            if (strROWID == "")
                            {
                                if (strDel != "True")
                                {
                                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "HIC_DOCTOR                                                \r\n";
                                    SQL += "        (SABUN,DRNAME,IPSADAY,REDAY,LICENCE,ENTDATE,ENTSABUN,ROOM,PAN,GbDent,DrCode,GBHea)  \r\n";
                                    SQL += " VALUES                                                                                     \r\n";
                                    SQL += "        ('" + strSaBun + "','" + strDrname + "',TO_DATE('" + strIpsaDay + "','YYYY-MM-DD'), \r\n";
                                    SQL += "        TO_DATE('" + strReDay + "','YYYY-MM-DD'), " + nLicence + ",SYSDATE,                 \r\n";
                                    SQL += "        " + clsType.User.IdNumber + ",'" + strRoom + "','" + strPan + "','" + strDent + "', \r\n";
                                    SQL += "        '" + strDrCode + "','" + strHea + "')                                                   ";
                                }
                            }
                            else
                            {
                                if (strDel == "True")
                                {
                                    SQL = " DELETE " + ComNum.DB_PMPA + "HIC_DOCTOR WHERE ROWID = '" + strROWID + "' ";
                                }
                                else
                                {
                                    SQL = " UPDATE " + ComNum.DB_PMPA + "HIC_DOCTOR                         \r\n";
                                    SQL += "   SET DrName   = '" + strDrname + "',                          \r\n";
                                    SQL += "       IpsaDay  = TO_DATE('" + strIpsaDay + "','YYYY-MM-DD'),   \r\n";
                                    SQL += "       ReDay    = TO_DATE('" + strReDay + "','YYYY-MM-DD'),     \r\n";
                                    SQL += "       Licence  = " + nLicence + ",                             \r\n";
                                    SQL += "       Room     ='" + strRoom + "',                             \r\n";
                                    SQL += "       Pan      ='" + strPan + "',                              \r\n";
                                    SQL += "       GbDent   ='" + strDent + "',                             \r\n";
                                    SQL += "       DrCode   ='" + strDrCode + "',                           \r\n";
                                    SQL += "       EntDate  = SYSDATE,                                      \r\n";
                                    SQL += "       GBHEA    = '" + strHea + "',                             \r\n";
                                    SQL += "       EntSabun =" + clsType.User.IdNumber + "                  \r\n";
                                    SQL += " WHERE ROWID = '" + strROWID + "'                                   ";
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
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }
                        }
                    }
   
                }

                clsDB.setCommitTran(pDbCon);

                Cursor.Current = Cursors.Default;

                MessageBox.Show("저장 완료!","확인");

                Screen_Display(chkOut.Checked);

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.ToString());
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            clsSpread cSpd = new clsSpread();

            cSpd.Spread_All_Clear(SS1);

            cSpd = null;

            Screen_Display(chkOut.Checked);
        }

        void Screen_Display(bool ArgOut)
        {
            DataTable Dt = new DataTable();
            DataTable Dt2 = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int i = 0;
            int intRowAffected = 0;

            string strSaBun = string.Empty;
            string strIpsaDay = string.Empty;

            try
            {
                SQL = "";
                SQL += " SELECT Sabun,DrName,TO_CHAR(IpsaDay,'YYYY-MM-DD') IpsaDay,GbHea,               ";
                SQL += "        TO_CHAR(ReDay,'YYYY-MM-DD') ReDay,Licence,Room,Pan,GbDent,DrCode,ROWID  ";
                SQL += "   FROM " + ComNum.DB_PMPA + "HIC_DOCTOR                                        ";
                if (ArgOut == false)
                {
                    SQL += "  WHERE ReDay IS NULL                                                       ";
                }
                SQL += "  ORDER BY DrName                                                               ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    SS1.ActiveSheet.RowCount = Dt.Rows.Count + 5;

                    for (i = 0; i < Dt.Rows.Count; i++)
                    {
                        strSaBun = VB.Format(VB.Val(Dt.Rows[i]["Sabun"].ToString()), "00000");
                        strIpsaDay = Dt.Rows[i]["IpsaDay"].ToString().Trim();

                        //인사마스타의 입사일,퇴사일을 READ
                        SQL = " SELECT TO_CHAR(IpsaDay,'YYYY-MM-DD') IpsaDay,";
                        SQL += "       TO_CHAR(ToiDay,'YYYY-MM-DD') ReDay ";
                        SQL += "  FROM " + ComNum.DB_ERP + "INSA_MST ";
                        SQL += " WHERE Sabun='" + strSaBun + "' ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (Dt2.Rows.Count > 0)
                        {
                            clsDB.setBeginTran(clsDB.DbCon);

                            strIpsaDay = Dt2.Rows[0]["IpsaDay"].ToString().Trim();

                            SQL = " UPDATE " + ComNum.DB_PMPA + "HIC_DOCTOR                     ";
                            SQL += "   SET IpsaDay = TO_DATE('" + strIpsaDay + "','YYYY-MM-DD') ";
                            SQL += " WHERE ROWID='" + Dt.Rows[i]["ROWID"].ToString().Trim() + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                                
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            clsDB.setRollbackTran(clsDB.DbCon);
                        }

                        SS1.ActiveSheet.Cells[i, 1].Text  = Dt.Rows[i]["Sabun"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 2].Text  = Dt.Rows[i]["DrName"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 3].Text  = strIpsaDay;
                        SS1.ActiveSheet.Cells[i, 4].Text  = Dt.Rows[i]["ReDay"].ToString().Trim(); 
                        SS1.ActiveSheet.Cells[i, 5].Text  = Dt.Rows[i]["Licence"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 6].Text  = Dt.Rows[i]["Room"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 7].Text  = Dt.Rows[i]["Pan"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 8].Text  = Dt.Rows[i]["GbHea"].ToString().Trim();      //종검판정여부
                        SS1.ActiveSheet.Cells[i, 9].Text  = Dt.Rows[i]["GbDent"].ToString().Trim();   
                        SS1.ActiveSheet.Cells[i, 10].Text = Dt.Rows[i]["DrCode"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 11].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
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
    }
}
