using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB;
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    public partial class frmPmpaIpdPassInfo : Form
    {
        public frmPmpaIpdPassInfo()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            ComFunc CF = new ComFunc();

            this.Load                   += new EventHandler(eFormLoad);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnPrint.Click         += new EventHandler(eBtnClick);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnSave.Click          += new EventHandler(eBtnClick);
            this.txtPano.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.SS1.CellClick          += new CellClickEventHandler(eSpdClick);
            this.SS3.CellDoubleClick    += new CellClickEventHandler(eSpdDbClick);
            this.dtpInDate.ValueChanged += new EventHandler(CF.eDtpFormatSet);
            
            CF = null;
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            clsSpread CS = new clsSpread();

            if (e.ColumnHeader == true)
            {
                CS.setSpdSort(SS1, e.Column, true);
                return;
            }
        }

        void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }
            
            dtpInDate.Text = SS3.ActiveSheet.Cells[e.Row, 1].Text.Trim();
            txtIpdNo.Text = SS3.ActiveSheet.Cells[e.Row, 2].Text.Trim();
            txtRemark.Text = SS3.ActiveSheet.Cells[e.Row, 3].Text.Trim();

            btnSave.Enabled = true;
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtPano)
            {
                if (e.KeyChar == (char)13)
                {
                    txtPano.Text = string.Format("{0:D8}", Convert.ToInt32(txtPano.Text));
                    eDisplay(clsDB.DbCon);
                }
            }
        }

        void eDisplay(PsmhDb pDbCon)
        {
            int i = 0;
            int nREAD = 0;
            string strRemark = string.Empty;
            string strMisuDtl = string.Empty;

            DataTable Dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " SELECT DEPTCODE, TO_CHAR(INDATE, 'YYYY-MM-DD') INDATE,     \r\n";
            SQL += "        IPDNO, PASS_INFO, SNAME                             \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                \r\n";
            SQL += "  WHERE Pano = '" + txtPano.Text + "'                       \r\n";
            SQL += "    AND GBSTS != '9'                                        \r\n";
            SQL += "    AND ARTICLE IS NOT NULL                                 \r\n";
            SQL += "    AND PASS_DATE IS NULL                                   \r\n";
            SQL += "  ORDER BY Indate Desc                                          ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            nREAD = Dt.Rows.Count;
            SS3.ActiveSheet.RowCount = nREAD;

            if (nREAD == 0)
            {
                ComFunc.MsgBox("출입증 대여기록이 없습니다.", "확 인");
                txtPano.Focus();
                Dt.Dispose();
                Dt = null;
                return;
            }
            else
            {
                txtRemark.Text = Dt.Rows[0]["PASS_INFO"].ToString().Trim();
                lblSname.Text = Dt.Rows[0]["SNAME"].ToString().Trim();

                for (i = 0; i < nREAD; i++)
                {
                    SS3.ActiveSheet.Cells[i, 0].Text = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    SS3.ActiveSheet.Cells[i, 1].Text = Dt.Rows[i]["INDATE"].ToString().Trim();
                    SS3.ActiveSheet.Cells[i, 2].Text = Dt.Rows[i]["IPDNO"].ToString().Trim();
                    SS3.ActiveSheet.Cells[i, 3].Text = Dt.Rows[i]["PASS_INFO"].ToString().Trim();
                }
            }

            Dt.Dispose();
            Dt = null;
        }
        
        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc CF = new ComFunc();

            ComFunc.ReadSysDate(clsDB.DbCon);

            Screen_Clear();

            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
            SS1_Sheet1.Rows.Count = 0;

            SS3.ActiveSheet.ClearRange(0, 0, SS3_Sheet1.Rows.Count, SS3_Sheet1.ColumnCount, false);
            SS3_Sheet1.Rows.Count = 0;

            dtpFDate.Text = CF.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -30);
            dtpTDate.Text = clsPublic.GstrSysDate;

            CF = null;
        }

        void Screen_Clear()
        {
            ComFunc CF = new ComFunc();

            txtPano.Text = "";
            lblSname.Text = "";
            txtIpdNo.Text = "";
            dtpInDate.Text = "";
            CF.dtpClear(dtpInDate);
            txtRemark.Text = "";
            btnSave.Enabled = false;

            CF = null;
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
                eSearch(clsDB.DbCon);
            }
            else if (sender == btnPrint)
            {
                ePrint();
            }
        }

        void eSearch(PsmhDb pDbCon)
        {
            int i = 0;
            int nREAD = 0;
            string strRemark = string.Empty;
            string strMisuDtl = string.Empty;

            DataTable Dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            
            try
            {
                ComFunc CF = new ComFunc();
                

                SQL = "";
                SQL += " SELECT PANO, TO_CHAR(INDATE, 'YYYY-MM-DD') INDATE, SNAME,  \r\n";
                SQL += "        JOBSABUN, PASS_DATE, PASS_SABUN, IPDNO, PASS_INFO   \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER                \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                SQL += "    AND GBSTS != '9'                                        \r\n";
                SQL += "    AND ((ARTICLE ='Y') or  (PASS_DATE is not null ))       \r\n";
                SQL += "    AND Indate BETWEEN TO_DATE('" + dtpFDate.Text + "','yyyy-mm-dd')         ";
                SQL += "                   AND TO_DATE('" + dtpTDate.Text + "','yyyy-mm-dd') +1  \r\n";
                SQL += "  ORDER BY Indate Desc                                          ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;
                SS1.ActiveSheet.RowCount = nREAD;

                if (nREAD == 0)
                {
                    ComFunc.MsgBox("출입증 대여기록이 없습니다.", "확 인");
                    txtPano.Focus();
                    Dt.Dispose();
                    Dt = null;
                    return;
                }
                else
                {
                    for (i = 0; i < nREAD; i++)
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 1].Text = Dt.Rows[i]["INDATE"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 2].Text = CF.Read_SabunName(pDbCon, Dt.Rows[i]["JOBSABUN"].ToString().Trim());
                        SS1.ActiveSheet.Cells[i, 3].Text = Dt.Rows[i]["PASS_INFO"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 4].Text = Dt.Rows[i]["PASS_DATE"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 5].Text = CF.Read_SabunName(pDbCon, Dt.Rows[i]["PASS_SABUN"].ToString().Trim());
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

            ComFunc.ReadSysDate(clsDB.DbCon);

            strTitle = "출입증 관리 명단";
            strSubTitle = "출력일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(1) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 35, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, true, true, false, false, false, false, false, (float)0.9);

            SPR.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        void eSave(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (txtIpdNo.Text.Trim() == "")
            {
                ComFunc.MsgBox("대상자료를 선택하세요.", "확인");
                return;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(pDbCon);

                SQL = "";
                SQL += " UPDATE ADMIN.IPD_NEW_MASTER                  ";
                SQL += "    SET PASS_DATE=TRUNC(SYSDATE)                    ";
                SQL += "       ,PASS_SABUN = " + clsType.User.IdNumber + "  ";
                SQL += "       ,ARTICLE= 'N'                                ";
                SQL += " WHERE IPDNO='" + txtIpdNo.Text + "'                ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);

                Cursor.Current = Cursors.Default;

                ComFunc.MsgBox("반납 처리 완료");

                Screen_Clear();

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return; 
            }
        }
    }
}
