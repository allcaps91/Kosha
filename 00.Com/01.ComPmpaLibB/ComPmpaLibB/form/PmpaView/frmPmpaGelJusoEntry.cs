using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComLibB;

namespace ComPmpaLibB
{
    public partial class frmPmpaGelJusoEntry : Form
    {
        private string FstrGbn = string.Empty;
        private string FstrCode = string.Empty;
        private string FstrSangho = string.Empty;
        private string FstrTel = string.Empty;
        private string FstrDamdang = string.Empty;
        private string FstrMail = string.Empty;
        private string FstrJuso1 = string.Empty;
        private string FstrJuso2 = string.Empty;
        private string FstrJuso3 = string.Empty;
        private string FstrROWID = string.Empty;
        private string FstrZip = string.Empty;

        clsPrint clsPrt = new clsPrint();

        private string PrintName1 = "봉투인쇄";
        private string PrintName2 = "봉투인쇄136";

        public frmPmpaGelJusoEntry()
        {
            InitializeComponent();
        }

        private void frmPmpaGelJusoEntry_Load(object sender, EventArgs e)
        {
            cboGbn.Items.Clear();
            cboGbn.Items.Add("01.산재관련 거래처");
            cboGbn.Items.Add("02.자보관련 거래처");
            cboGbn.Items.Add("03.의료보험 조합");
            cboGbn.Items.Add("04.종합병원");
            cboGbn.Items.Add("05.보건소");
            cboGbn.Items.Add("06.협력병원");
            cboGbn.Items.Add("99.기타");
            cboGbn.SelectedIndex = 0;

            SS1.Enabled = false;

            Screen_Clear();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0, nREAD = 0;

            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            //btnSearch.Enabled = false;

            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
            SS1_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Code,Sangho,Tel,Damdang,Mail,Juso1,Juso2,Juso3,ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_GelJuso ";
            SQL += ComNum.VBLF + "  WHERE Gubun = '" + VB.Left(cboGbn.Text, 2) + "' ";
            if (txtSangho.Text.Trim() != string.Empty)
            {
                SQL += ComNum.VBLF + "  AND Sangho LIKE '" + txtSangho.Text.Trim() + "%' ";
            }
            if (rdoGubun1.Checked) { SQL += ComNum.VBLF + "ORDER BY Code "; }
            if (rdoGubun2.Checked) { SQL += ComNum.VBLF + "ORDER BY Sangho"; }
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            nREAD = Dt.Rows.Count;

            SS1_Sheet1.Rows.Count = nREAD + 30;

            if (nREAD > 0)
            {
                for (i = 0; i < nREAD; i++)
                {
                    SS1_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["Code"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["Sangho"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["Tel"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["Damdang"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["Mail"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 6].Text = READ_MAIL_Name(Dt.Rows[i]["Mail"].ToString().Trim());
                    SS1_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["Juso1"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["Juso2"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["Juso3"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }

            Dt.Dispose();
            Dt = null;

            btnSave.Enabled = true;
            SS1.Enabled = true;

        }

        string READ_MAIL_Name(string Code)
        {
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";
            DataTable dt = null;

            SQL = "SELECT MAILJUSO FROM " + ComNum.DB_PMPA + "BAS_MAILNEW ";
            SQL = SQL + ComNum.VBLF + "WHERE MAILCODE ='" + Code.Trim() + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["MAILJUSO"].ToString().Trim();
            }
            else
            {
                rtnVal = "";
            }
            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Screen_Clear()
        {
            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
            SS1_Sheet1.Rows.Count = 0;
            btnSave.Enabled = false;
            btnPrint.Enabled = false;
            tabCntrl.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0;
            string strDel = string.Empty;
            string strChk = string.Empty;

            btnSave.Enabled = false;


            clsDB.setBeginTran(clsDB.DbCon);

            FstrGbn = VB.Left(cboGbn.Text, 2);

            for (i = 0; i < SS1_Sheet1.Rows.Count; i++)
            {
                strDel = SS1_Sheet1.Cells[i, 0].Text.Trim();
                FstrCode = SS1_Sheet1.Cells[i, 1].Text.Trim();
                FstrSangho = SS1_Sheet1.Cells[i, 2].Text.Trim();
                FstrROWID = SS1_Sheet1.Cells[i, 10].Text.Trim();
                strChk = SS1_Sheet1.Cells[i, 11].Text.Trim();

                if (strDel == "1" && FstrROWID != string.Empty)
                {
                    Data_Delete(clsDB.DbCon);
                }
                else if (strChk == "Y")
                {
                    if (FstrROWID == string.Empty)
                    {
                        Data_Insert(clsDB.DbCon);
                    }
                    else
                    {
                        Data_Update(clsDB.DbCon);
                    }
                }

            }

            clsDB.setCommitTran(clsDB.DbCon);

            Screen_Clear();
        }

        private void Data_Insert(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            if (FstrCode == string.Empty || FstrSangho == string.Empty) { return; }

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_GELJUSO (";
            SQL += ComNum.VBLF + "        Gubun,Code,Sangho,Tel,Damdang,Mail,Juso1,Juso2,Juso3 ) ";
            SQL += ComNum.VBLF + "VALUES ('" + FstrGbn + "', '" + FstrCode + "','" + FstrSangho + "', ";
            SQL += ComNum.VBLF + "        '" + FstrTel + "','" + FstrDamdang + "','" + FstrMail + "', ";
            SQL += ComNum.VBLF + "        '" + FstrJuso1 + "','" + FstrJuso2 + "','" + FstrJuso3 + "') ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;
            }

        }

        private void Data_Update(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            SQL = "";
            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + " ETC_GELJUSO ";
            SQL += ComNum.VBLF + "    SET Code      = '" + FstrCode + "',";
            SQL += ComNum.VBLF + "        Sangho    = '" + FstrSangho + "', ";
            SQL += ComNum.VBLF + "        Tel       = '" + FstrTel + "',";
            SQL += ComNum.VBLF + "        Damdang   = '" + FstrDamdang + "',";
            SQL += ComNum.VBLF + "        Mail      = '" + FstrMail + "', ";
            SQL += ComNum.VBLF + "        Juso1     = '" + FstrJuso1 + "',";
            SQL += ComNum.VBLF + "        Juso2     = '" + FstrJuso2 + "',";
            SQL += ComNum.VBLF + "        Juso3     = '" + FstrJuso3 + "' ";
            SQL += ComNum.VBLF + "  WHERE ROWID     = '" + FstrROWID + "' ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;
            }

        }

        private void Data_Delete(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            SQL = "";
            SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "ETC_GELJUSO ";
            SQL += ComNum.VBLF + "  WHERE ROWID = '" + FstrROWID + "' ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;
            }
        }

        private void SS1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            clsSpread clsSp = new clsSpread();

            if (btnSave.Enabled)
            {
                if ((bool)SS1_Sheet1.Cells[e.Row, 0].Value)
                {
                    clsSp.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, Color.FromArgb(255, 0, 0));
                }
                else
                {
                    clsSp.setSpdForeColor(SS1, e.Row, 0, e.Row, SS1_Sheet1.ColumnCount - 1, Color.FromArgb(0, 0, 0));
                }
            }
        }

        void eZip_Value(string GstrValue)
        {
            FstrZip = GstrValue;
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 5)
            {
                FstrZip = "";

                frmSearchRoadAdd frm = new frmSearchRoadAdd();
                frm.rSetGstrValue += new frmSearchRoadAdd.SetGstrValue(eZip_Value);
                frm.ShowDialog();

                if (FstrZip != "")
                {
                    SS1.ActiveSheet.Cells[e.Row, 5].Text = VB.Left(VB.Pstr(FstrZip, "|", 1), 5);
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string PrtName = string.Empty;

            //프린터 장비 조회
            if (rdoGu1.Checked)
            {
                if (clsPrt.isLabBarCodePrinter(this.PrintName1) == false) { return; }
            }
            else if (rdoGu2.Checked)
            {
                if (clsPrt.isLabBarCodePrinter(this.PrintName2) == false) { return; }
            }

            try
            {
                PrtName = VB.IIf(rdoGu1.Checked, this.PrintName1, this.PrintName2).ToString();

                PrintDocument RcpPrint = new PrintDocument();
                PrintController printController = new StandardPrintController();
                RcpPrint.PrintController = printController;  //기본인쇄창 없애기

                PageSettings ps = new PageSettings();
                ps.PrinterSettings.PrinterName = PrtName;
                ps.Margins = new Margins(10, 10, 10, 10);
                ps.Landscape = false;
                RcpPrint.DefaultPageSettings = ps;
                RcpPrint.PrinterSettings.PrinterName = PrtName;
                RcpPrint.PrintPage += new PrintPageEventHandler(RcpPrint_PrintPage);

                RcpPrint.Print();

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message.ToString());
            }
        }

        private void RcpPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            //TODO : 실제 인쇄 136 프린터 적용하여 테스트 후 구현하기

            //clsPrint.printString(e, "입  원", 0 + intX, 10 + intY, new Font("굴림체", 18, FontStyle.Bold), Brushes.Black);
            //clsPrint.printString(e, "의뢰서", 0 + intX, 30 + intY, new Font("굴림체", 18, FontStyle.Bold), Brushes.Black);
            //    nSu = Val(TxtSu.Text)

            //    For i = 0 To(SS1.MaxRows - 1)
            //        SS1.Row = i + 1
            //        SS1.Col = 1:  strChk = SS1.Text
            //        SS1.Col = 1:  SS1.Text = ""


            //        strOK = "NO"
            //        If OptPrt(0).Value = True Then strOK = "OK"
            //        If OptPrt(1).Value = True And strChk = "1" Then strOK = "OK"
            //        If OptPrt(2).Value = True And strChk <> "1" Then strOK = "OK"



            //        If strOK = "OK" Then
            //            If OptGu(0).Value = True Then
            //                strBuse = "원무과"
            //                strName = Trim(TxtName.Text)                         '담당자성명
            //                strDamdang = Trim(TxtDamDang.Text)                   '담당명
            //                strTel = "(054) " & Trim(TxtTel.Text)                           '담당자전화번호
            //                SS1.Col = 6:  strMail = Trim(SS1.Text)               '우편번호
            //                SS1.Col = 7:  strJuso1 = Trim(SS1.Text) & " "        '우편번호의 주소
            //                SS1.Col = 8:  strJuso1 = strJuso1 & Trim(SS1.Text)   '주소(동,번지)
            //                SS1.Col = 9:  strJuso2 = Trim(SS1.Text)              '2번째줄의 주소
            //                SS1.Col = 10: strJuso3 = Trim(SS1.Text)              '3번째줄의 주소
            //                For j = 1 To nSu
            //''''                    Call Text_Print("굴림체", 12, "", 850, 1550, "원무과")
            //''''                    Call Text_Print("굴림체", 12, "", 850, 2750, strDamdang)
            //''''                    Call Text_Print("굴림체", 12, "", 1160, 1550, strName)
            //''''                    Call Text_Print("굴림체", 12, "", 1160, 2750, strTel)

            //                    Call Text_Print("굴림체", 12, "", 600, 1550, "----------------")
            //                    Call Text_Print("굴림체", 12, "", 1160, 1550, "원무과")
            //                    Call Text_Print("굴림체", 12, "", 1160, 2750, strDamdang)
            //                    Call Text_Print("굴림체", 12, "", 1470, 1550, strName)
            //                    Call Text_Print("굴림체", 12, "", 1470, 2750, strTel)



            //                    Call Text_Print("굴림체", 15, "", 2500, 5800, strJuso1)     '주소
            //                    Call Text_Print("굴림체", 13, "", 3000, 6500, strJuso2)
            //                    Call Text_Print("굴림체", 12, "", 3500, 8050, strJuso3)

            //                    Call Text_Print("굴림체", 18, "B", 4000, 8600, Left(strMail, 1) & "  " & Mid(strMail, 2, 1) & "  " & Mid(strMail, 3, 1))
            //                    Call Text_Print("굴림체", 18, "B", 4000, 10590, Mid(strMail, 4, 1) & "  " & Mid(strMail, 5, 1) & "  " & Mid(strMail, 6, 1))
            //                    Printer.EndDoc
            //                Next j
            //            Else
            //                strBuse = "원무과"
            //                strName = Trim(TxtName.Text)                         '담당자성명
            //                strDamdang = Trim(TxtDamDang.Text)                   '담당명
            //                strTel = "(054) " & Trim(TxtTel.Text)                           '담당자전화번호
            //                SS1.Col = 6:  strMail = Trim(SS1.Text)               '우편번호
            //                SS1.Col = 7:  strJuso1 = Trim(SS1.Text) & " "        '우편번호의 주소
            //                SS1.Col = 8:  strJuso1 = strJuso1 & Trim(SS1.Text)   '주소(동,번지)
            //                SS1.Col = 9:  strJuso2 = Trim(SS1.Text)              '2번째줄의 주소
            //                SS1.Col = 10: strJuso3 = Trim(SS1.Text)              '3번째줄의 주소


            //                For j = 1 To nSu
            //                    Call Text_Print("굴림체", 15, "", 2460, 3160, "원무과")
            //                    Call Text_Print("굴림체", 15, "", 2460, 5510, strDamdang)
            //                    Call Text_Print("굴림체", 15, "", 3000, 3160, strName)
            //                    Call Text_Print("굴림체", 15, "", 3000, 5510, strTel)


            //                    Call Text_Print("굴림체", 20, "", 10000, 7800, strJuso1)     '주소
            //                    Call Text_Print("굴림체", 17, "", 10900, 10500, strJuso2)
            //                    Call Text_Print("굴림체", 15, "", 11815, 12500, strJuso3)

            //                    Call Text_Print("굴림체", 22, "B", 12730, 13650, Left(strMail, 1) & "  " & Mid(strMail, 2, 1) & "  " & Mid(strMail, 3, 1))
            //                    Call Text_Print("굴림체", 22, "B", 12730, 16190, Mid(strMail, 4, 1) & "  " & Mid(strMail, 5, 1) & "  " & Mid(strMail, 6, 1))
            //                    Printer.EndDoc
            //                Next j
            //            End If
            //        End If
            //   Next i
        }

        private void tabCntrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCntrl.SelectedIndex == 1)
            {
                btnPrint.Enabled = true;
            }
            else
            {
                btnPrint.Enabled = false;
            }
        }
    }
}
