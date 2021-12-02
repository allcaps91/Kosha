using ComBase;
using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB;
using FarPoint.Win.Spread;
using System.ComponentModel;
using System.Drawing.Printing;

namespace ComPmpaLibB
{
    public partial class frmMisuAdd : Form
    {

        string strChkflag = string.Empty;
        string strFirst = string.Empty;
        string strName = string.Empty;
        string strMstROWID = string.Empty;
        int nEditCol = 0;
        double nAmt1 = 0;
        double nAmt2 = 0;
        double nAmt3 = 0;
        int nRow = 0;
        clsPmpaMisu CPM = new clsPmpaMisu();
        clsSpread cSpd = new clsSpread();
        PsmhDb pDbCon;
        string GstrPANO = string.Empty;    

        public frmMisuAdd()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmMisuAdd(string strPano )
        {
            InitializeComponent();
            SetEvent();
            GstrPANO = strPano;
        }

        void SetEvent()
        {
            ComFunc CF = new ComFunc();

            this.Load += new EventHandler(eFormLoad);
            this.CmdExit.Click += new EventHandler(eBtnClick);
            this.CmdCancel.Click += new EventHandler(eBtnClick);
            this.CmdOk.Click += new EventHandler(eBtnClick);
            this.CmdSMS.Click += new EventHandler(eBtnClick);
            this.CmdPrint.Click += new EventHandler(eBtnClick);
            
            this.TxtPano.KeyPress += new KeyPressEventHandler(eKeyPress);
        //    this.ss2.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);
            //  this.dtpInDate.ValueChanged += new EventHandler(CF.eDtpFormatSet);

              this.TxtPano.LostFocus += new EventHandler(eCtl_LostFocus);

            CF = null;
        }

        private void eCtl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.TxtPano)
            {
                if (TxtPano.Text == "") { return;  }
                TxtPano.Text = string.Format("{0:D8}", Convert.ToInt32(TxtPano.Text));
                if (strFirst == "OK")
                {
                    Master_Id_Display(clsDB.DbCon);
                }

                CmdOk.Enabled = true;
                CmdCancel.Enabled = true;
                CmdSMS.Enabled = true;

            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == TxtPano)
            {
                if (e.KeyChar == (char)13)
                {
                    TxtPano.Text = string.Format("{0:D8}", Convert.ToInt32(TxtPano.Text));
                    Master_Id_Display(clsDB.DbCon);
                    CmdCancel.Enabled = true;
                }
            }
        }
        private void Send_SMS()
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                clsPublic.GstrRetValue = "";

                SQL = "";
                SQL += ComNum.VBLF + " SELECT HPhone,Tel,Sname From " + ComNum.DB_PMPA + "Bas_Patient ";
                SQL += ComNum.VBLF + "  Where Pano = '" + TxtPano.Text.Trim() + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    clsPublic.GstrRetValue = Dt.Rows[0]["HPhone"].ToString().Trim();
                    if (clsPublic.GstrRetValue == "")
                        clsPublic.GstrRetValue = Dt.Rows[0]["Tel"].ToString().Trim();
                }

                clsPublic.GstrRetValue += "^^" + Dt.Rows[0]["SNAME"].ToString().Trim();

                Dt.Dispose();
                Dt = null;

                //이미 폼이 떠있는지 확인한다.
                foreach (Form frmFindform in Application.OpenForms)
                {
                    if (frmFindform.GetType() == typeof(frmSMS_Misu))
                    {
                        frmFindform.Activate();
                        frmFindform.BringToFront();
                        return;
                    }
                }

                frmSMS_Misu frm = new frmSMS_Misu(1,VB.Left(clsPublic.GstrRetValue,13));
                frm.ShowDialog();

                clsPublic.GstrRetValue = "";
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

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            clsSpread CS = new clsSpread();
            //Screen_Clear();
            if (e.ColumnHeader == true)
            {
                CS.setSpdSort(ss2, e.Column, true);
                return;
            }
        }

        void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            string strPano = string.Empty, strSname = string.Empty,
            strInDate = string.Empty, nIPDNO = string.Empty, strOutDate = string.Empty,
            nTRSNo = string.Empty, strJumin = string.Empty, strRemark = string.Empty;

            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }


            //FstrPano = ss2.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            //strSname = ss2.ActiveSheet.Cells[e.Row, 1].Text.Trim();
            //strInDate = ss2.ActiveSheet.Cells[e.Row, 2].Text.Trim();
            //strOutDate = ss2.ActiveSheet.Cells[e.Row, 3].Text.Trim();
            //FnIPDNO = ss2.ActiveSheet.Cells[e.Row, 16].Text.Trim();
            //FnTRSNO = Convert.ToInt64(ss2.ActiveSheet.Cells[e.Row, 17].Text.Trim().ToString());

            //P_Info.Text = "";
            //P_Info.Text = FstrPano + " " + strSname + " " + strInDate;
        }


        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc CF = new ComFunc();

            ComFunc.ReadSysDate(clsDB.DbCon);

         
            Display_Clear();

            CF = null;

            PanelMsg.Text = "";
           
            nRow = 1;

            ssOrder.ActiveSheet.ClearRange(0, 0, ssOrder_Sheet1.Rows.Count, ssOrder_Sheet1.ColumnCount, false);
            ssOrder_Sheet1.Rows.Count = 0;

            TxtPano.Text = GstrPANO;

            if (TxtPano.Text != "")
            {
                KeyPressEventArgs k = new KeyPressEventArgs((char)13);

                eKeyPress(this.TxtPano, k);
            }
            else
            {
                TxtPano.Select();
            }
        }

        void Display_Clear()
        {
            int i = 0;
            ComFunc CF = new ComFunc();

            TxtPano.Text = "";
            for (i = 0; i < ss2.ActiveSheet.RowCount; i++)
            { 
                ss2.ActiveSheet.Cells[i, 1].Text = "";
                ss2.ActiveSheet.Cells[i, 3].Text = "";
                ss2.ActiveSheet.Cells[i, 5].Text = "";
      
            }
            ssOrder.ActiveSheet.ClearRange(0, 0, ssOrder_Sheet1.Rows.Count, ssOrder_Sheet1.ColumnCount, false);
            ssOrder_Sheet1.Rows.Count = 0;

            CmdOk.Enabled = false;
            CmdCancel.Enabled = false;
            CmdSMS.Enabled = false;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == CmdExit)
            {
                this.Close();
                return;
            }
            else if (sender == CmdSMS)
            {
                    Send_SMS();
            }
            //   else if (sender == btnSave)
            //   {
            //       eSave(clsDB.DbCon);
            //   }
            else if (sender == CmdOk)
            {
                eSave(clsDB.DbCon);
            }
            else if (sender == CmdPrint)
            {
                ePrint();
            }
            else if (sender == CmdCancel)
            {
                Display_Clear();
                TxtPano.Focus();

            }
        }
       
     
        public string READ_MISU_MAGAM_DATE(PsmhDb pDbCon)
        {
            DataTable DtFunc = null;
            string rtnVal = string.Empty;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

        
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT NAME";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND Gubun = 'ACC_전표입력가능시작일자' AND Code='02'  ";
                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                }

                if (DtFunc.Rows.Count > 0)
                    rtnVal = DtFunc.Rows[0]["NAME"].ToString().Trim();
                else
                    return rtnVal;

                DtFunc.Dispose();
                DtFunc = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return rtnVal;
        }

        
        void Master_Id_Display(PsmhDb pDbCon)
        {
            int i = 0;
            int j = 0;
           
            int nREAD = 0; 
            int nCnt = 0;
            long nHisAmt = 0;
            long nIDNO = 0;
            
            string strMisuDtl = string.Empty, strDate = string.Empty,
            strRemark = string.Empty, strMDATE = string.Empty;
            DataTable Dt = null;
            DataTable Dt2 = null;
            
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            ssOrder.ActiveSheet.ClearRange(0, 0, ssOrder_Sheet1.Rows.Count, ssOrder_Sheet1.ColumnCount, false);
            ssOrder_Sheet1.Rows.Count = 0;

            strMDATE = READ_MISU_MAGAM_DATE(pDbCon);

            try
            {

                SQL = "";
                SQL += " SELECT   Sname,Jumin1,Jumin2,Jumin3,Sex,Tel,  \r\n";
                SQL += "        Bi,Gwange,Pname,Kiho,TO_CHAR(LastDate,'yyyy-mm-dd') LastDate   \r\n";
                SQL += "   FROM  " + ComNum.DB_PMPA + "BAS_PATIENT A                \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                SQL += "    AND Pano = '" + TxtPano.Text.Trim() + "'        ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                nREAD = Dt.Rows.Count;
                if (nREAD == 0)
                {
                    ComFunc.MsgBox("등록번호가 오류입니다. 다시 입력하세요.", "확 인");
                    TxtPano.Focus();
                    Dt.Dispose();
                    Dt = null;
                    return;
                }
                strName = Dt.Rows[i]["Sname"].ToString().Trim();

                ss2.ActiveSheet.Cells[0, 1].Text = strName;
                ss2.ActiveSheet.Cells[1, 1].Text = Dt.Rows[i]["Jumin1"].ToString().Trim() + clsAES.DeAES(Dt.Rows[i]["Jumin3"].ToString().Trim());
                ss2.ActiveSheet.Cells[2, 1].Text = Dt.Rows[i]["Sex"].ToString().Trim();
                ss2.ActiveSheet.Cells[3, 1].Text = Dt.Rows[i]["Tel"].ToString().Trim();
                ss2.ActiveSheet.Cells[0, 3].Text = Dt.Rows[i]["Bi"].ToString().Trim();
                ss2.ActiveSheet.Cells[1, 3].Text = Dt.Rows[i]["Gwange"].ToString().Trim();
                ss2.ActiveSheet.Cells[2, 3].Text = Dt.Rows[i]["Pname"].ToString().Trim();
                ss2.ActiveSheet.Cells[3, 3].Text = Dt.Rows[i]["Kiho"].ToString().Trim();
                ss2.ActiveSheet.Cells[3, 5].Text = Dt.Rows[i]["LastDate"].ToString().Trim();

                Dt.Dispose();
                Dt = null;

                //---------------------( 미수발생 마스타 Select )---------------------*

                SQL = "";
                SQL += " SELECT  ROWID,Pano,MAmt,IAmt,JAmt  \r\n";
                SQL += "   FROM  " + ComNum.DB_PMPA + "MISU_GAINMST A                \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                SQL += "    AND Pano = '" + TxtPano.Text.Trim() + "'        ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                nREAD = Dt.Rows.Count;
                if (nREAD == 0)
                {
                    nAmt1 = 0;
                    nAmt2 = 0;
                    nAmt3 = 0;

                }
                else
                {
                    strMstROWID = "";

                    nAmt1 = Convert.ToInt64(Dt.Rows[i]["MAmt"]);
                    nAmt2 = Convert.ToInt64(Dt.Rows[i]["IAmt"]);
                    nAmt3 = nAmt1 - nAmt2;
                    strChkflag = "OK";
                    strMstROWID = Dt.Rows[i]["ROWID"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                ss2.ActiveSheet.Cells[0, 5].Text = VB.Format(nAmt1, "###,###,##0") + VB.Space(5);// string.Format("{0:#,##0}", nAmt1);
                ss2.ActiveSheet.Cells[1, 5].Text = VB.Format(nAmt2, "###,###,##0") + VB.Space(5);// string.Format("{0:#,##0}", nAmt2);
                ss2.ActiveSheet.Cells[2, 5].Text = VB.Format(nAmt3, "###,###,##0") + VB.Space(5);// string.Format("{0:#,##0}", nAmt3);

                // ----------------(미수내역 History 조회 )-------------------------*

                SQL = "";
                SQL += " SELECT  To_Char(Bdate,'YYYY-MM-DD') Bdate, Gubun1, Gubun2,  Amt,  \r\n";
                SQL += "   Remark,Idno,MisuDtl,ROWID,FLAG,SABUN,POBUN,                     \r\n";
                SQL += "   TO_CHAR(EntTime,'YYYY-MM-DD HH24:MI') EntTime, GRADE,GRADE2     \r\n";
                SQL += "   FROM  " + ComNum.DB_PMPA + "MISU_GAINSLIP A                    \r\n";
                SQL += "  WHERE 1 = 1                                                     \r\n";
                SQL += "    AND Pano = '" + TxtPano.Text.Trim() + "'                      \r\n";
                SQL += "  ORDER BY Bdate,Gubun1                                           \r\n";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                nREAD = Dt.Rows.Count;
                ssOrder.ActiveSheet.RowCount = 0;
                ssOrder.ActiveSheet.RowCount = nREAD + 5;

                if (nREAD == 0)
                {

                    Dt.Dispose();
                    Dt = null;
                    return;
                }
                else
                {

                    for (i = 0; i < nREAD; i++)
                    {
                        nRow += 1;
                        if (ssOrder_Sheet1.Rows.Count < nRow)
                        {
                            ssOrder_Sheet1.Rows.Count = nRow;
                        }

                        nCnt = 0;
                        ssOrder.ActiveSheet.Cells[i, 1].Text = Dt.Rows[i]["Bdate"].ToString().Trim();

                        ssOrder.ActiveSheet.Cells[i, 2].Text = Dt.Rows[i]["Gubun1"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 3].Text = VB.Format( Convert.ToInt32(Dt.Rows[i]["Amt"]), "############0"); // string.Format("{0:###0}", Convert.ToInt64(Dt.Rows[i]["Amt"]));
                        ssOrder.ActiveSheet.Cells[i, 4].Text = Dt.Rows[i]["Gubun2"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 5].Text = Dt.Rows[i]["Remark"].ToString().Trim();
                        strRemark = Dt.Rows[i]["Remark"].ToString().Trim();

                        for (j = 0; j < strRemark.Length; j++)
                        {
                            if (VB.Mid(strRemark, j, 1) == VB.Chr(13).ToString())
                            {
                                nCnt += 1;
                            }
                        }
                        if (nCnt == 0)
                            nCnt = 1;


                        cSpd.SetfpsRowHeight(ssOrder, ssOrder_Sheet1.Cells[nRow - 1, 1].Row.Height * nCnt);
                        strMisuDtl = ComFunc.LeftH(Dt.Rows[i]["MisuDtl"].ToString().Trim() + VB.Space(30), 30);
                        ssOrder.ActiveSheet.Cells[i, 6].Text = ComFunc.LeftH(strMisuDtl, 1);
                        ssOrder.ActiveSheet.Cells[i, 7].Text = ComFunc.MidH(strMisuDtl, 2,2);
                        ssOrder.ActiveSheet.Cells[i, 8].Text = ComFunc.MidH(strMisuDtl, 4, 2);
                        ssOrder.ActiveSheet.Cells[i, 9].Text = Dt.Rows[i]["GRADE2"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 10].Text =VB.Format( VB.Val(ComFunc.MidH(strMisuDtl, 6, 9)), "############");
                        ssOrder.ActiveSheet.Cells[i, 11].Text = ComFunc.MidH(strMisuDtl, 15, 8).Trim();
                        ssOrder.ActiveSheet.Cells[i, 12].Text = VB.Right(strMisuDtl,  8).Trim();
                        ssOrder.ActiveSheet.Cells[i, 13].Text = Dt.Rows[i]["FLAG"].ToString().Trim();
                        nIDNO = Convert.ToInt64(Dt.Rows[i]["Idno"].ToString());
                      

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT Name FROM " + ComNum.DB_PMPA + "BAS_PASS  ";
                        SQL += ComNum.VBLF + " WHERE IDnumber = " + nIDNO + " ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (Dt2.Rows.Count > 0)
                        {
                            ssOrder.ActiveSheet.Cells[i, 14].Text = Dt2.Rows[0]["Name"].ToString().Trim();
                        }
                        Dt2.Dispose();
                        Dt2 = null;

                        ssOrder.ActiveSheet.Cells[i, 15].Text = Dt.Rows[i]["EntTime"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 16].Text = Dt.Rows[i]["ROWID"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 17].Text = "";
                        ssOrder.ActiveSheet.Cells[i, 18].Text = Dt.Rows[i]["SABUN"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT SABUN, KORNAME FROM " + ComNum.DB_ERP + "INSA_MST ";
                        SQL += ComNum.VBLF + " WHERE SABUN  = '" + VB.Format(VB.Val(Dt.Rows[i]["SABUN"].ToString().Trim()), "####0") + "' ";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (Dt2.Rows.Count > 0)
                        {
                            ssOrder.ActiveSheet.Cells[i, 19].Text = Dt2.Rows[0]["KORNAME"].ToString().Trim() ;
                        }
                        Dt2.Dispose();
                        Dt2 = null;

                        if (Dt.Rows[i]["POBUN"].ToString().Trim() == "1")
                        {
                            ssOrder.ActiveSheet.Cells[i, 20].Text = "포스코";
                        }
                        else if (Dt.Rows[i]["POBUN"].ToString().Trim() == "2")
                        {
                            ssOrder.ActiveSheet.Cells[i, 20].Text = "계산서";
                        }
                        if (Dt.Rows[i]["POBUN"].ToString().Trim() == "1")
                        {
                            ssOrder.ActiveSheet.Cells[i, 20].Text = "포스코";
                        }
                        
                        if (string.Compare(Dt.Rows[i]["Bdate"].ToString().Trim(), clsPublic.GstrSysDate) < 0)
                        {
                            ssOrder_Sheet1.Cells[i, 0, i, 10].Locked = true;
                            ssOrder_Sheet1.Cells[i, 5, i, 5].Locked = false;
                            ssOrder_Sheet1.Cells[i, 6, i, 6].Locked = false;
                            ssOrder_Sheet1.Cells[i, 7, i, 7].Locked = false;
                            ssOrder_Sheet1.Cells[i, 8, i, 8].Locked = false;
                            ssOrder_Sheet1.Cells[i, 9, i, 9].Locked = false;
                            ssOrder_Sheet1.Cells[i, 13, i, 13].Locked = false;
                           
                        }
                        
                        
                        

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
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "개인미수내역";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
       //     strHeader += CS.setSpdPrint_String("등록기간 : ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
       //     strHeader += CS.setSpdPrint_String("증빙서류 : ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.9f);

            CS.setSpdPrint(ssOrder, PrePrint, setMargin, setOption, strHeader, strFooter);

        }
        string READ_GubunName(string argGrade)
        {
            string rtnVal = "";

            switch (argGrade)
            {
                case "1":
                    rtnVal = "미수발생";
                    break;
                case "2":
                    rtnVal = "미수입금";
                    break;
                case "3":
                    rtnVal = "미수반송";
                    break;
                case "4":
                    rtnVal = "미수감액";
                    break;
                case "5":
                    rtnVal = "미수삭감";
                    break;
                case "9":
                    rtnVal = "참고사항";
                    break;
               
                default:
                    rtnVal = "";
                    break;
            }

            return rtnVal;
        }
        string READ_BuseName(string argGrade)
        {
            string rtnVal = "";

            switch (argGrade)
            {
                case "1":
                    rtnVal = "외래수납";
                    break;
                case "2":
                    rtnVal = "응급실";
                    break;
                case "3":
                    rtnVal = "입원수납";
                    break;
                case "4":
                    rtnVal = "심사팀";
                    break;
                case "5":
                    rtnVal = "원무팀";
                    break;
              
                default:
                    rtnVal = "";
                    break;
            }

            return rtnVal;
        }
        string READ_PerMisuGye(string argGrade)
        {
            string rtnVal = "";

            switch (VB.Val(argGrade).ToString("00"))
            {
                case "01":
                    rtnVal = "가퇴원미수";
                    break;
                case "02":
                    rtnVal = "업무착오미수";
                    break;
                case "03":
                    rtnVal = "탈원미수";
                    break;
                case "04":
                    rtnVal = "지불각서";
                    break;
                case "05":
                    rtnVal = "응급미수";
                    break;
                case "06":
                    rtnVal = "외래미수";
                    break;
                case "07":
                    rtnVal = "심사청구미수";
                    break;
                case "08":
                    rtnVal = "책임보험";
                    break;
                case "09":
                    rtnVal = "퇴원";
                    break;
                case "10":
                    rtnVal = "기타";
                    break;
                case "11":
                    rtnVal = "기관청구미수";
                    break;
                case "12":
                    rtnVal = "입원정밀";
                    break;
                case "13":
                    rtnVal = "필수접종국가지원";
                    break;
                case "14":
                    rtnVal = "회사접종";
                    break;
                case "15":
                    rtnVal = "금연처방";
                    break;
                

                default:
                    rtnVal = "";
                    break;
            }

            return rtnVal;
        }
        string READ_PerMisuGrade_New(string argGrade)
        {
            string rtnVal = "";

            switch (VB.Val(argGrade).ToString("00"))
            {
                case "01":
                    rtnVal = "채권사의뢰검토";
                    break;
                case "02":
                    rtnVal = "집행불능";
                    break;
                case "03":
                    rtnVal = "문제환자";
                    break;
                case "04":
                    rtnVal = "대불접수";
                    break;
                case "05":
                    rtnVal = "대불불능";
                    break;
                case "06":
                    rtnVal = "대장관리";
                    break;
                case "07":
                    rtnVal = "소액미수";
                    break;
                case "08":
                    rtnVal = "의료분쟁";
                    break;
                default:
                    rtnVal = "";
                    break;
            }

            return rtnVal;
        }
       

        void eSave(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;
            int nREAD = 0;
            Double nAmt = 0;
            Double nMAmt = 0;
            Double nIAmt = 0;
            string strROWID = string.Empty, strMDATE = string.Empty, strPobun = string.Empty,
            strSabun = string.Empty, strFlag = string.Empty,
            strRemark = string.Empty, strGubun2 = string.Empty, strBDate = string.Empty, strJob = string.Empty, strMisuDtl = string.Empty,
            strGrade = string.Empty, strMisuGye = string.Empty, strDeptCode = string.Empty, strIO = string.Empty, strBuse = string.Empty,
            strGbn = string.Empty, strDate = string.Empty, strChk = string.Empty, strDel = string.Empty;
            DataTable Dt = null;
            strMDATE = READ_MISU_MAGAM_DATE(pDbCon);

            ComFunc.ReadSysDate(clsDB.DbCon);

            for (i = 0; i < ssOrder.ActiveSheet.RowCount; i++)
            {
                strDel = ssOrder.ActiveSheet.Cells[i, 0].Text;
                strDate = ssOrder.ActiveSheet.Cells[i, 1].Text;
                strGbn = ssOrder.ActiveSheet.Cells[i, 2].Text;
                nAmt = VB.Val(ssOrder.ActiveSheet.Cells[i, 3].Text);
                strBuse = ssOrder.ActiveSheet.Cells[i, 4].Text;
                strSabun = ssOrder.ActiveSheet.Cells[i, 18].Text;
                strROWID = ssOrder.ActiveSheet.Cells[i, 16].Text;
                if (strDel != "True" && nAmt != 0)
                {
                    if (VB.Len(strDate) != 10)
                    {
                        ComFunc.MsgBox("일자를 정확히 입력하세요.");
                        return;
                    }
                    if (strBuse == "")
                    {
                        ComFunc.MsgBox("부서구분이 공란입니다.");
                        return;
                    }
                    if (string.Compare(strDate, strMDATE) <= 0 && strROWID =="")
                    {
                        ComFunc.MsgBox("등록하려는 일자 중 미수마감일보다 빠른 일자가 있습니다 미수등록이 불가능합니다.");
                        return;
                    }
                    if (strGbn == "1")
                    {
                        strIO = ssOrder.ActiveSheet.Cells[i, 6].Text;
                        strDeptCode = ssOrder.ActiveSheet.Cells[i, 7].Text;
                        strMisuGye = ssOrder.ActiveSheet.Cells[i, 8].Text;
                        strGrade = ssOrder.ActiveSheet.Cells[i, 9].Text;
                        if (strIO != "I" && strIO != "O")
                        {
                            ComFunc.MsgBox("외래,입원 구분이 오류");
                            return;
                        }
                        if (strDeptCode == "" && string.Compare(strDate, "1999-01-01") >= 0)
                        {
                            ComFunc.MsgBox("진료과가 공란입니다.");
                            return;
                        }
                        if (strMisuGye == "")
                        {
                            ComFunc.MsgBox("미수계정이 공란입니다");
                            return;
                        }
                        if (CPM.READ_PerMisuGye(strMisuGye) == "구분없음")
                        {
                            ComFunc.MsgBox("미수계정이 오류입니다.");
                            return;//READ_PerMisuGrade_New(dt1.Rows[0]["GRADE2"].ToString().Trim());
                        }
                        if (READ_PerMisuGrade_New(strGrade) == "구분없음")
                        {
                            ComFunc.MsgBox("미수등급이 오류입니다.");
                            return;//READ_PerMisuGrade_New(dt1.Rows[0]["GRADE2"].ToString().Trim());
                        }
                    }
                    else
                    {
                        strIO = ssOrder.ActiveSheet.Cells[i, 6].Text;
                        strDeptCode = ssOrder.ActiveSheet.Cells[i, 7].Text;
                        strMisuGye = ssOrder.ActiveSheet.Cells[i, 8].Text;
                        if (strIO != "I" && strIO != "O")
                        {
                            ComFunc.MsgBox("외래,입원 구분이 오류");
                            return;
                        }
                        if (strDeptCode == "" && string.Compare(strDate, "1999-01-01") >= 0)
                        {
                            ComFunc.MsgBox("진료과가 공란입니다.");
                            return;
                        }
                        if (strMisuGye == "")
                        {
                            ComFunc.MsgBox("미수계정이 공란입니다");
                            return;
                        }

                    }
                }
            }


            nMAmt = 0; nIAmt = 0;
            CmdOk.Enabled = false;
            CmdCancel.Enabled = false;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {

                for (i = 0; i < ssOrder.ActiveSheet.RowCount; i++)

                {
                    strDel = ssOrder.ActiveSheet.Cells[i, 0].Text;
                    strDate = ssOrder.ActiveSheet.Cells[i, 1].Text;
                    strGbn = ssOrder.ActiveSheet.Cells[i, 2].Text;
                    nAmt = VB.Val(ssOrder.ActiveSheet.Cells[i, 3].Text);
                    strROWID = ssOrder.ActiveSheet.Cells[i, 16].Text;
                    strChk = ssOrder.ActiveSheet.Cells[i, 17].Text;
                    strSabun = ssOrder.ActiveSheet.Cells[i, 18].Text;
                    strPobun = ssOrder.ActiveSheet.Cells[i, 20].Text;
                    if (strPobun == "포스코")
                    {
                        strPobun = "1";
                    }
                    else if (strPobun == "계산서")
                    {
                        strPobun = "2";
                    }
                    else
                    {
                        strPobun = "";
                    }

                    if (strROWID != "" && strDel == "True")
                    {
                        if (string.Compare(strDate, strMDATE) <= 0)
                        {
                            ComFunc.MsgBox("등록하려는 일자 중 미수마감일보다 빠른 일자가 있습니다 미수등록이 불가능합니다.");
                        }
                        else
                        {
                            strJob = "D";

                            //CmdOK_History_Insert
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "MISU_HISTORY ";
                            SQL += ComNum.VBLF + "      ( WRTNO,UpDateTime,UpdateTable,UpdateJob,UpdateSabun, ";
                            SQL += ComNum.VBLF + "        MisuID,Bdate,Class,Gubun,Amt,Remark,EntDate,EntPart,GRADE,GRADE2 )";
                            SQL += ComNum.VBLF + "        select '77777'                                                ";
                            SQL += ComNum.VBLF + "          ,SYSDATE                                                              ";
                            SQL += ComNum.VBLF + "          ,'" + "S" + "'                              ";
                            SQL += ComNum.VBLF + "          ,'" + strJob + "'                             ";
                            SQL += ComNum.VBLF + "          ,'" + clsType.User.IdNumber + "'         ";
                            SQL += ComNum.VBLF + "          ,Pano, Bdate,'08',Gubun1 || Gubun2,Amt,MisuDtl, EntTime,IDno, GRADE ,GRADE2                   ";
                            SQL += ComNum.VBLF + "           from " + ComNum.DB_PMPA + "MISU_GAINSLIP                                 ";
                            SQL += ComNum.VBLF + "      WHERE ROWID = '" + strROWID + "'                  ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("작업중 문제가 발생했습니다");
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            //CmdOK_Delete_Rtn
                            SQL = "";
                            SQL += ComNum.VBLF + " DELETE " + ComNum.DB_PMPA + "MISU_GAINSLIP ";
                            SQL += ComNum.VBLF + "      WHERE ROWID = '" + strROWID + "'                  ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("작업중 문제가 발생했습니다");
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                        }
                    }
                    else if (strROWID != "" && strChk == "Y")
                    {
                        strJob = "M";

                        //CmdOK_History_Insert
                        SQL = "";
                        SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "MISU_HISTORY ";
                        SQL += ComNum.VBLF + "      ( WRTNO,UpDateTime,UpdateTable,UpdateJob,UpdateSabun, ";
                        SQL += ComNum.VBLF + "        MisuID,Bdate,Class,Gubun,Amt,Remark,EntDate,EntPart,GRADE,GRADE2 )";
                        SQL += ComNum.VBLF + "        select '77777'                                                ";
                        SQL += ComNum.VBLF + "          ,SYSDATE                                                              ";
                        SQL += ComNum.VBLF + "          ,'" + "S" + "'                              ";
                        SQL += ComNum.VBLF + "          ,'" + strJob + "'                             ";
                        SQL += ComNum.VBLF + "          ,'" + clsType.User.IdNumber + "'         ";
                        SQL += ComNum.VBLF + "          ,Pano, Bdate,'08',Gubun1 || Gubun2,Amt,MisuDtl, EntTime,IDno, GRADE ,GRADE2                   ";
                        SQL += ComNum.VBLF + "           from " + ComNum.DB_PMPA + "MISU_GAINSLIP                                 ";
                        SQL += ComNum.VBLF + "      WHERE ROWID = '" + strROWID + "'                  ";


                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("작업중 문제가 발생했습니다");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        //Glue_Set_Rtn


                        strBDate = ssOrder.ActiveSheet.Cells[i, 1].Text;
                        strGubun2 = ssOrder.ActiveSheet.Cells[i, 4].Text;
                        strRemark = ssOrder.ActiveSheet.Cells[i, 5].Text;
                        strMisuDtl = "";
                        strMisuDtl += ComFunc.LeftH(ssOrder.ActiveSheet.Cells[i, 6].Text + VB.Space(1), 1);
                        strMisuDtl += ComFunc.LeftH(ssOrder.ActiveSheet.Cells[i, 7].Text + VB.Space(2), 2);
                        strMisuDtl += VB.Format(VB.Val(ssOrder.ActiveSheet.Cells[i, 8].Text), "00");
                        strGrade = VB.Format(VB.Val(ssOrder.ActiveSheet.Cells[i, 9].Text), "00");
                        if (strGrade == "00") { strGrade = ""; }
                        strMisuDtl += ComFunc.LeftH(VB.Format(VB.Val(ssOrder.ActiveSheet.Cells[i, 10].Text), "000000000") + VB.Space(9), 9);
                        strMisuDtl += ComFunc.LeftH(ssOrder.ActiveSheet.Cells[i, 11].Text.Trim() + VB.Space(8), 8);
                        strMisuDtl += ComFunc.LeftH(ssOrder.ActiveSheet.Cells[i, 12].Text.Trim() + VB.Space(8), 8);
                        if (ssOrder.ActiveSheet.Cells[i, 13].Text == "*")
                        {
                            strFlag = "*";
                        }
                        if (ssOrder.ActiveSheet.Cells[i, 13].Text != "*")
                        {
                            strFlag = "0";
                        }

                        //CmdOK_Update_Rtn
                        SQL = "";
                        SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "MISU_GAINSLIP  ";
                        SQL += ComNum.VBLF + "    SET  Bdate   = TO_DATE('" + strBDate + "','YYYY-MM-DD')      ";
                        SQL += ComNum.VBLF + "        ,Gubun1 = '" + strGbn + "'    ";
                        SQL += ComNum.VBLF + "        ,Gubun2 = '" + strGubun2 + "'    ";
                        SQL += ComNum.VBLF + "        ,Amt  = " + nAmt + "     ";
                        SQL += ComNum.VBLF + "        ,Remark   = '" + strRemark  + "'      ";
                        SQL += ComNum.VBLF + "        ,MisuDtl   = '" + strMisuDtl  + "'       ";
                        SQL += ComNum.VBLF + "        ,FLAG  = '" + strFlag  + "'       ";
                        SQL += ComNum.VBLF + "        ,GRADE2  = '" + strGrade + "'      ";
                        SQL += ComNum.VBLF + "        ,SABUN  = '" + strSabun + "'      ";
                        SQL += ComNum.VBLF + "        ,POBUN  = '" + strPobun + "'      ";

                        SQL += ComNum.VBLF + "      WHERE ROWID = '" + strROWID + "'                  ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("작업중 문제가 발생했습니다");
                            Cursor.Current = Cursors.Default;
                            return;
                        }


                       
                    }
                    else if (strROWID == "" && strChk == "Y")
                    {
                        if (string.Compare(strDate, strMDATE) <= 0)
                        {
                            ComFunc.MsgBox("등록하려는 일자 중 미수마감일보다 빠른 일자가 있습니다 미수등록이 불가능합니다.");
                        }
                        else
                        {
                            //Glue_Set_Rtn
                            strBDate = ssOrder.ActiveSheet.Cells[i, 1].Text;
                            strGubun2 = ssOrder.ActiveSheet.Cells[i, 4].Text;
                            strRemark = ssOrder.ActiveSheet.Cells[i, 5].Text;
                            strMisuDtl = "";
                            strMisuDtl += ComFunc.LeftH(ssOrder.ActiveSheet.Cells[i, 6].Text + VB.Space(1), 1);
                            strMisuDtl += ComFunc.LeftH(ssOrder.ActiveSheet.Cells[i, 7].Text + VB.Space(2), 2);
                            strMisuDtl += VB.Format(VB.Val(ssOrder.ActiveSheet.Cells[i, 8].Text), "00");
                            strGrade    = VB.Format(VB.Val(ssOrder.ActiveSheet.Cells[i, 9].Text), "00");
                            if (strGrade == "00") { strGrade = ""; } 
                            strMisuDtl += ComFunc.LeftH(VB.Format(VB.Val(ssOrder.ActiveSheet.Cells[i, 10].Text), "00000000") + VB.Space(9), 9);
                            strMisuDtl += ComFunc.LeftH(ssOrder.ActiveSheet.Cells[i, 11].Text.Trim() + VB.Space(8), 8);
                            strMisuDtl += ComFunc.LeftH(ssOrder.ActiveSheet.Cells[i, 12].Text.Trim() + VB.Space(8), 8);
                            if (ssOrder.ActiveSheet.Cells[i, 13].Text == "*")
                            {
                                strFlag = "*";
                            }
                            if (ssOrder.ActiveSheet.Cells[i, 13].Text != "*")
                            {
                                strFlag = "0";
                            }

                            //CmdOK_Insert_Rtn
                            SQL = "";
                            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "MISU_GAINSLIP ";
                            SQL += ComNum.VBLF + "      ( Pano,Gubun1,Gubun2,Bdate,Amt,Remark,MisuDtl,IDno, ";
                            SQL += ComNum.VBLF + "        Flag,Part,EntTime,GRADE2,SABUN,POBUN) ";
                            SQL += ComNum.VBLF + " VALUES ";
                            SQL += ComNum.VBLF + "       ('" + VB.Format(VB.Val(TxtPano.Text), "00000000") + "','" + strGbn + "','" + strGubun2 + "', "  ;
                            SQL += ComNum.VBLF + "          TO_DATE('" + strBDate + "','YYYY-MM-DD'),'" + nAmt + "',                           ";
                            SQL += ComNum.VBLF + "          '" + strRemark + "','" + strMisuDtl + "','" + clsType.User.IdNumber + "','" + strFlag + "',   ";
                            SQL += ComNum.VBLF + "          '" + clsType.User.IdNumber + "',SYSDATE,'" + strGrade + "', '" + strSabun + "',  ";
                            SQL += ComNum.VBLF + "          '" + strPobun + "' )         ";
                         
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("작업중 문제가 발생했습니다");
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                        }
                    }
                    if (strDel != "True" && nAmt != 0)
                    {
                        switch (strGbn)
                        {
                            case "1":
                                nMAmt = nMAmt + nAmt;
                                break;
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                                nIAmt = nIAmt + nAmt;
                                break;
                            default:
                                break;
                        }

                    }

                }
                //CmdOK_Mst_Update
                if (strMstROWID =="")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "MISU_GAINMST ";
                    SQL += ComNum.VBLF + "      ( PANO,MAMT,IAMT,JAMT ) ";
                    SQL += ComNum.VBLF + "      VALUES ";
                    SQL += ComNum.VBLF + "      ('" + VB.Format(VB.Val(TxtPano.Text), "00000000") + "'," + nMAmt + ",";
                    SQL += ComNum.VBLF + "      " + nIAmt + " ," + (nMAmt - nIAmt) + " )         "; 

                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "MISU_GAINMST SET ";
                    SQL += ComNum.VBLF + "        MAmt   = " + nMAmt + ",       ";
                    SQL += ComNum.VBLF + "        IAmt   = " + nIAmt + ",       ";
                    SQL += ComNum.VBLF + "        JAmt   = " + (nMAmt - nIAmt) + "       ";
                   

                    SQL += ComNum.VBLF + "      WHERE ROWID = '" + strMstROWID + "'                  ";
                }
               
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("작업중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                Display_Clear();
                TxtPano.Focus();

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

        private void ssOrder_CellClick(object sender, CellClickEventArgs e)
        {

        }

        private void ssOrder_LeaveCell(object sender, LeaveCellEventArgs e)
        {
            

            int Row = ssOrder_Sheet1.ActiveRowIndex;
            int Column = ssOrder_Sheet1.ActiveColumnIndex  ;
            int NewCol = ssOrder_Sheet1.ActiveColumnIndex  + 1;

            if (NewCol == 1) { return; }

            string strData = ssOrder_Sheet1.Cells[Row, Column ].Text.Trim();

            switch (Column)
            {
                case 2://품목코드
                    PanelMsg.Text = READ_GubunName(strData);
                    if (PanelMsg.Text == "")
                    {
                        ComFunc.MsgBox("1.미수발생 2.미수입금 3.반송 4.감액, 5.삭감 9.참고사항 입니다.");
                    }
                    else
                    {
                        PanelMsg.Text = "1.미수발생 2.미수입금 3.반송 4.감액, 5.삭감 9.참고사항 ";
                    }
                    break;

                case 4://급여/비급여 설정
                    PanelMsg.Text = READ_BuseName(strData);
                    if (PanelMsg.Text == "")
                    {
                        ComFunc.MsgBox("부서코드가 오류입니다");
                    }
                    else
                    {
                        PanelMsg.Text = "1.외래 2.응급실 3.입원 4.심사팀 5.원무팀 ";
                    }
                    break;


                case 6://수량

                    if (strData != "O" && strData != "I")
                    {
                        ComFunc.MsgBox("< O.외래 I.입원 입니다. >-");
                    }
                    else
                    {
                        PanelMsg.Text = "< O.외래 I.입원 입니다. >-";
                    }
                    break;

                case 8://수량

                    if (strData != "")
                    {
                        PanelMsg.Text = READ_PerMisuGye(strData);
                        if (PanelMsg.Text == "")
                        {
                            ComFunc.MsgBox("미수구분이 오류입니다");
                        }

                        else
                        {
                            PanelMsg.Text = "1.가퇴원 2.업무착오 3.탈원 4.지불각서 5.응급 6.외래 7.심사청구 8.책임보험, 9.퇴원,10.기타, 11.기관청구, 12.입원정밀, 13.필수접종국가지원 14.회사접종 15.금연처방";
                        }
                    }

                    break;
                case 12://수량

                    if (strData != "0" && strData != "*" && strData != "")
                    {
                        ComFunc.MsgBox("입금 완료 구분이 오류 (*,0,' ')");
                    }
                    else
                    {
                        PanelMsg.Text = "-<  '*':입금완료     '0':미완료  >-";
                    }
                    break;

            }

            switch (NewCol)
            {
                case 2://품목코드
                        PanelMsg.Text = "1.미수발생 2.미수입금 3.반송 4.감액, 5.삭감 9.참고사항 ";
                    break;

                case 4://급여/비급여 설정
                        PanelMsg.Text = "1.외래 2.응급실 3.입원 4.심사팀 5.원무팀 ";
                    break;


                case 6://수량

                        PanelMsg.Text = "< O.외래 I.입원 입니다. >-";
                    break;

                case 8://수량

                            PanelMsg.Text = "1.가퇴원 2.업무착오 3.탈원 4.지불각서 5.응급 6.외래 7.심사청구 8.책임보험, 9.퇴원,10.기타, 11.기관청구, 12.입원정밀, 13.필수접종국가지원 14.회사접종 15.금연처방";
                 
                    break;
                case 9://수량

                    PanelMsg.Text = "1.채권사의뢰검토 2.집행불능 3.문제환자 4.대불접수 5.대불불능 6.대장관리 7.소액미수 8.의료분쟁";

                    break;
                case 12://수량

                        PanelMsg.Text = "-<  '*':입금완료     '0':미완료  >-";
                    break;

            }

        }

        private void ssOrder_EditModeOff(object sender, EventArgs e)
        {
           
        }

        private void ssOrder_Change(object sender, ChangeEventArgs e)
        {
            String strData = "";

            int Row = ssOrder_Sheet1.ActiveRowIndex;
            int Column = ssOrder_Sheet1.ActiveColumnIndex;
            int NewCol = ssOrder_Sheet1.ActiveColumnIndex + 1;

            if (Column == 0 && Column > 12 ) { return; }
            if (Column == 1)
            {
                strData = ssOrder_Sheet1.Cells[Row, 1].Text.Trim();
                if (VB.Len(strData) != 10) { return; }

                if (string.Compare(strData, clsPublic.GstrSysDate) > 0)
                {
                    ComFunc.MsgBox("작업일자가 오류입니다.");
                    return;
                }
            }
            else if (Column == 18)
            {
                DataTable DtFunc = null;
                string rtnVal = string.Empty;
                string SQL = string.Empty;
                string SqlErr = string.Empty;


                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT KORNAME";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_ERP + "INSA_MST ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1";
                    SQL += ComNum.VBLF + "    AND SABUN = '" + clsType.User.IdNumber + "'  ";
                    SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    }

                    if (DtFunc.Rows.Count > 0)
                        ssOrder_Sheet1.Cells[Row, 19].Text = DtFunc.Rows[0]["KORNAME"].ToString().Trim();
                    else
                        ssOrder_Sheet1.Cells[Row, 19].Text = "" ;

                    DtFunc.Dispose();
                    DtFunc = null;

                    
                }

                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                }

              

            }
            ssOrder_Sheet1.Cells[Row, 17].Text = "Y";

        }

        private void ssOrder_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            int Row = ssOrder_Sheet1.ActiveRowIndex;
            int Column = ssOrder_Sheet1.ActiveColumnIndex;
            int NewCol = ssOrder_Sheet1.ActiveColumnIndex + 1;

            if (Column != 13) { return; }
            if (ssOrder_Sheet1.Cells[Row, 13].Text == ""|| ssOrder_Sheet1.Cells[Row, 13].Text == "0")
            {
                ssOrder_Sheet1.Cells[Row, 13].Text = "*";
            }
            else
            {
                ssOrder_Sheet1.Cells[Row, 13].Text = "0";
            }
            ssOrder_Sheet1.Cells[Row, 17].Text = "Y";
        }
    }
    

}
