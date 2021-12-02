using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComPmpaLibB
{
    public partial class frmPmpaJiByung : Form
    {
        private string FstrROWID = string.Empty;
        private string FstrPano = string.Empty;
        private string FstrInDate = string.Empty;
        private string FstrJumin = string.Empty;
        private string FstrSName = string.Empty;
        private string FstrGamek = string.Empty;
        private string FstrBohun = string.Empty;
        private string FstrGelCode = string.Empty;

        long FnIPDNO = 0;
        long FnTRSNO = 0;

        ComFunc CF      = new ComFunc();
        clsSpread cSpd  = new clsSpread();

        public frmPmpaJiByung()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaJiByung(string ArgBi, string ArgDept)
        {
            InitializeComponent();
            setEvent();

            switch (ArgBi)
            {
                case "11":
                case "12":
                case "13":
                    rdoBi6.Checked = true;
                    break;
                case "31":
                case "33":
                    rdoBi2.Checked = true;
                    break;
                case "51":
                    rdoBi7.Checked = true;
                    break;
                case "52":
                case "55":
                    rdoBi1.Checked = true;
                    break;
                default:
                    break;
            }

            if (ArgDept == "OG")
            {
                rdoBi3.Checked = true;
            }
        }

        private void setEvent()
        {
            this.btnNew.Click           += new EventHandler(eBtnClick);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnCancel.Click        += new EventHandler(eBtnClick);
            this.btnDelete.Click        += new EventHandler(eBtnClick);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnNhic.Click          += new EventHandler(eBtnClick);
            this.rdoBi1.CheckedChanged  += new EventHandler(eBtnClick);
            this.rdoBi2.CheckedChanged  += new EventHandler(eBtnClick);
            this.rdoBi3.CheckedChanged  += new EventHandler(eBtnClick);
            this.rdoBi4.CheckedChanged  += new EventHandler(eBtnClick);
            this.rdoBi5.CheckedChanged  += new EventHandler(eBtnClick);
            this.rdoBi6.CheckedChanged  += new EventHandler(eBtnClick);
            this.rdoBi7.CheckedChanged  += new EventHandler(eBtnClick);

            #region Focus Events
            this.dtpSDate.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.dtpEDate.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.cboDept.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.cboDr.KeyPress         += new KeyPressEventHandler(eKeyPress);
            this.cboGbSpc.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.cboBi.KeyPress         += new KeyPressEventHandler(eKeyPress);
            this.txtPname.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.cboGwange.KeyPress     += new KeyPressEventHandler(eKeyPress);
            this.cboGbGamek.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.txtGelCode.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.txtKiho.KeyPress       += new KeyPressEventHandler(eKeyPress);
            this.cboBohun.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.txtGkiho1.KeyPress     += new KeyPressEventHandler(eKeyPress);
            this.txtGkiho4.KeyPress     += new KeyPressEventHandler(eKeyPress);
            this.txtFromTrans.KeyPress  += new KeyPressEventHandler(eKeyPress);
            this.txtEramt.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.txtJupboNo.KeyPress    += new KeyPressEventHandler(eKeyPress);
            this.txtBDate.KeyPress      += new KeyPressEventHandler(eKeyPress);
            this.txtStartDate.KeyPress  += new KeyPressEventHandler(eKeyPress);
            this.txtEndDate.KeyPress    += new KeyPressEventHandler(eKeyPress);
            #endregion

            #region DateTimePicker Null 처리
            this.dtpSDate.ValueChanged  += new EventHandler(eDtpCheck);
            this.dtpEDate.ValueChanged  += new EventHandler(eDtpCheck);
            //this.dtpSDate.ValueChanged  += new EventHandler(CF.eDtpFormatSet);
            //this.dtpEDate.ValueChanged  += new EventHandler(CF.eDtpFormatSet);
            #endregion

            #region SelStart 이벤트 처리
            this.txtPname.Click         += new EventHandler(eSelStart);
            this.txtGelCode.Click       += new EventHandler(eSelStart);
            this.txtKiho.Click          += new EventHandler(eSelStart);
            this.txtGkiho1.Click        += new EventHandler(eSelStart);
            this.txtGkiho4.Click        += new EventHandler(eSelStart);
            this.txtFromTrans.Click     += new EventHandler(eSelStart);
            this.txtEramt.Click         += new EventHandler(eSelStart);
            this.txtJupboNo.Click       += new EventHandler(eSelStart);
            #endregion

            cSpd.setSpdSort(SSList, -1,  true);
        }

        void eDtpCheck(object sender, EventArgs e)
        {
            if (sender == dtpSDate)
            {
                if (dtpSDate.Checked == true)
                {
                    dtpSDate.Format = DateTimePickerFormat.Short;
                }
                else
                {
                                    
                    dtpSDate.Format = DateTimePickerFormat.Custom;
                    dtpSDate.CustomFormat = " ";
                }
            }
            else if (sender == dtpEDate)
            {
                if (dtpEDate.Checked == true)
                {
                    dtpEDate.Format = DateTimePickerFormat.Short;
                }
                else
                {
                    dtpEDate.Format = DateTimePickerFormat.Custom;
                    dtpEDate.CustomFormat = " ";
                }
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnNew)              //신규
            {
                SetNewData(clsDB.DbCon);
            }
            else if (sender == this.btnExit)        //닫기
            {
                this.Close();
                return;
            }
            else if (sender == this.btnCancel)      //취소
            {
                Screen_Clear();
            }
            else if (sender == this.btnDelete)      //삭제
            {
                eDeleteData();
            }
            else if (sender == this.btnSearch)      //명단조회
            {
                Display_SSList();
            }
            else if (sender == this.rdoBi1 || sender == this.rdoBi2 || sender == this.rdoBi3 || sender == this.rdoBi4 || sender == this.rdoBi5 || sender == this.rdoBi6 || sender == this.rdoBi7)
            {
                Display_SSList();
            }
            else if (sender == this.btnNhic)
            {
                Nhic_View();
            }
        }

        void SetNewData(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            Screen_Clear();

            FstrROWID = "";
            FnTRSNO = 0;

            dtpSDate.Checked = true;
            dtpEDate.Checked = true;

            dtpSDate.Text = FstrInDate;
            dtpEDate.Text = clsPublic.GstrSysDate;

            if (FstrGamek != "") { cboGbGamek.Text = FstrGamek; }
            if (FstrBohun != "") { cboBohun.Text = FstrBohun; }
            if (FstrGelCode != "")
            {
                txtGelCode.Text = FstrGelCode;

                SQL = "";
                SQL += " SELECT MiaName,DelDate FROM " + ComNum.DB_PMPA + "BAS_MIA ";
                SQL += " WHERE MiaCode = '" + txtGelCode.Text.ToUpper() + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                
                if (Dt.Rows.Count == 0)
                {
                    lblGelCode.Text = "계약처 코드 Err";
                }
                else
                {
                    lblGelCode.Text = Dt.Rows[0]["MiaName"].ToString().Trim();

                    if (Dt.Rows[0]["DelDate"].ToString().Trim() != "")
                        ComFunc.MsgBox(Dt.Rows[0]["DelDate"].ToString().Trim() + "일 삭제처리된 계약처코드입니다.", "확인");
                }

                Dt.Dispose();
                Dt = null;
                
            }

            panJob.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = false;
            dtpSDate.Focus();
        }

        void eSelStart(object sender, EventArgs e)
        {
            TextBox tP = sender as TextBox;
            tP.SelectionStart = 0;
            tP.SelectionLength = tP.Text.Length;
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.dtpSDate && e.KeyChar == (Char)13) { dtpEDate.Focus(); }
            if (sender == this.dtpEDate && e.KeyChar == (Char)13) { cboDept.Focus(); }
            if (sender == this.cboDept && e.KeyChar == (Char)13) { cboDr.Focus(); }
            if (sender == this.cboDr && e.KeyChar == (Char)13) { cboBi.Focus(); }
            //if (sender == this.cboGbSpc && e.KeyChar == (Char)13) { txt_Spc_BDate.Focus(); }
            //if (sender == this.txt_Spc_BDate && e.KeyChar == (Char)13) { cboBi.Focus(); }
            if (sender == this.cboBi && e.KeyChar == (Char)13) { txtPname.Focus(); }
            if (sender == this.txtPname && e.KeyChar == (Char)13) { cboGwange.Focus(); }
            if (sender == this.cboGwange && e.KeyChar == (Char)13) { cboGbGamek.Focus(); }
            if (sender == this.cboGbGamek && e.KeyChar == (Char)13) { txtGelCode.Focus(); }
            if (sender == this.txtGelCode && e.KeyChar == (Char)13) { txtKiho.Focus(); }
            if (sender == this.txtKiho && e.KeyChar == (Char)13) { cboBohun.Focus(); }
            if (sender == this.cboBohun && e.KeyChar == (Char)13) { txtGkiho1.Focus(); }
            if (sender == this.txtGkiho1 && e.KeyChar == (Char)13) { txtGkiho4.Focus(); }
            if (sender == this.txtGkiho4 && e.KeyChar == (Char)13) { txtFromTrans.Focus(); }
            if (sender == this.txtFromTrans && e.KeyChar == (Char)13) { txtEramt.Focus(); }
            if (sender == this.txtEramt && e.KeyChar == (Char)13) { txtJupboNo.Focus(); }
            if (sender == this.txtJupboNo && e.KeyChar == (Char)13) { txtBDate.Focus(); }
            if (sender == this.txtBDate && e.KeyChar == (Char)13) { txtStartDate.Focus(); }
            if (sender == this.txtStartDate && e.KeyChar == (Char)13) { txtEndDate.Focus(); }
            if (sender == this.txtEndDate && e.KeyChar == (Char)13) { btnSave.Focus(); }
        }

        private void frmPmpaJiByung_Load(object sender, EventArgs e)
        {
            clsVbfunc.SetComboDept(clsDB.DbCon,cboDept, "2", 1);
            CF.Combo_BCode_SET(clsDB.DbCon, cboBi, "BAS_환자종류", true, 2);
            CF.Combo_BCode_SET(clsDB.DbCon, cboGbGamek, "BAS_감액코드명", true, 2);
            CF.Combo_BCode_SET(clsDB.DbCon, cboGwange, "BAS_피보험자관계", true, 2);
            CF.Combo_BCode_SET(clsDB.DbCon, cboBohun, "IPD_보훈구분", true, 2);

            cboGbSpc.Items.Clear();
            cboGbSpc.Items.Add("0.비선택진료"); 
            cboGbSpc.Items.Add("1.선택진료");
            cboGbSpc.Text = "";

            SSList.ActiveSheet.ClearRange(0, 0, SSList_Sheet1.Rows.Count, SSList_Sheet1.ColumnCount, false);
            
            Screen_Clear();

            Display_SSList();
        }

        void Screen_Clear()
        {
            
            SS2.ActiveSheet.ClearRange(0, 0, SS2_Sheet1.Rows.Count, SS2_Sheet1.ColumnCount, false);

            dtpSDate.Checked = false;
            dtpSDate.Format = DateTimePickerFormat.Custom;
            dtpSDate.CustomFormat = " ";

            dtpEDate.Checked = false;
            dtpEDate.Format = DateTimePickerFormat.Custom;
            dtpEDate.CustomFormat = " ";
            
            Control[] con = ComFunc.GetAllControls(this);

            if (con != null)
            {
                foreach (Control ctl in con)
                {
                    if (ctl is TextBox)
                    {
                        ((TextBox)ctl).Text = "";
                    }

                    if (ctl is ComboBox)
                    {
                        ((ComboBox)ctl).Text = "";
                    }

                    if (ctl is CheckBox)
                    {
                        ((CheckBox)ctl).Checked = false;
                    }
                }
            }

            lblPatInfo.Text = "";
            lblBi.Text = "";
            lblGwange.Text = "";
            lblGbGamek.Text = "";
            lblGelCode.Text = "";
            lblKiho.Text = "";
            lblBohun.Text = "";

            panJob.Enabled = false;
            btnNew.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;

            clsLockCheck.GstrLockPtno = "";
        }
        
        private void Display_SSList()
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            int i = 0, nRead = 0, nRow = 0;

            try
            {
                SSList.ActiveSheet.ClearRange(0, 0, SSList_Sheet1.Rows.Count, SSList_Sheet1.ColumnCount, false);
                SSList.ActiveSheet.RowCount = 0;

                //대상 환자를 SELECT MASTER => TRANS 단위로 변경작업함 2005-09-07 윤
                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.Pano,A.SName,A.Age,A.Sex,B.DeptCode,A.RoomCode,A.IPDNO,TO_CHAR(b.InDate,'YYYY-MM-DD') InDate,";
                SQL += ComNum.VBLF + "        C.JUMIN1,C.JUMIN2,c.JUMIN3, B.TRSNO, B.GbIPD,b.GBGAMEK,b.Bohun, b.GBSTS, A.WARDCODE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_TRANS B, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT C ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND B.ActDate IS NULL ";
                if (txtPano.Text.Trim() != "")
                {
                    SQL += ComNum.VBLF + " AND a.Pano ='" + txtPano.Text.Trim() + "' ";
                }
                else
                { 
                    if (rdoBi1.Checked)
                    { 
                        SQL += ComNum.VBLF + " AND B.Bi IN ('52','55') "; //자보
                    }
                    else if (rdoBi2.Checked)
                    { 
                        SQL += ComNum.VBLF + " AND B.Bi IN ('31','32','33') "; //산재
                    }
                    else if (rdoBi3.Checked)
                    { 
                        SQL += ComNum.VBLF + " AND B.DeptCode='OG' ";
                        SQL += ComNum.VBLF + " AND B.AmSet4='1' "; //산부인과
                    }
                    else if (rdoBi4.Checked)
                    { 
                        SQL += ComNum.VBLF + " AND B.AmSet4='3' "; //신생아
                    }
                    else if (rdoBi5.Checked)
                    { 
                        SQL += ComNum.VBLF + " AND B.Bi IN ('21','22') ";
                        SQL += ComNum.VBLF + " AND B.DeptCode='NP' ";
                    }
                    else if (rdoBi6.Checked)
                    {
                        SQL += ComNum.VBLF + " AND B.Bi IN ('11','12','13') ";  //보험
                    } 
                    else if (rdoBi7.Checked)
                    {
                        SQL += ComNum.VBLF + " AND (B.BI NOT IN ('52','55','31','32','33','11','12','13','22') OR B.BI = '22' AND B.DEPTCODE <> 'NP') ";
                        SQL += ComNum.VBLF + " AND B.AMSET4 NOT IN ('1','3') ";
                    }
                }
                SQL += ComNum.VBLF + " AND A.PANO = C.PANO(+) ";
                SQL += ComNum.VBLF + " AND B.GBIPD <> 'D' ";
                SQL += ComNum.VBLF + " AND A.IPDNO = B.IPDNO ";
                SQL += ComNum.VBLF + "ORDER BY A.SName,A.Pano ";
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
                        if(nRow > SSList_Sheet1.Rows.Count)
                        {
                            SSList_Sheet1.Rows.Count = nRow;
                        }

                        if (Dt.Rows[i]["GBSTS"].ToString().Trim() == "5" || Dt.Rows[i]["GBSTS"].ToString().Trim() == "6" || Dt.Rows[i]["GBSTS"].ToString().Trim() == "7")
                        {
                            SSList_Sheet1.Cells[nRow - 1, 0].BackColor = Color.FromArgb(255, 200, 200);
                        }

                        SSList_Sheet1.Cells[nRow - 1, 0].Text = VB.Format(VB.Val(Dt.Rows[i]["Pano"].ToString().Trim()),"00000000");
                        SSList_Sheet1.Cells[nRow - 1, 1].Text = Dt.Rows[i]["SName"].ToString().Trim();
                        SSList_Sheet1.Cells[nRow - 1, 2].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                        SSList_Sheet1.Cells[nRow - 1, 3].Text = Dt.Rows[i]["Age"].ToString().Trim() + "/" + Dt.Rows[i]["Sex"].ToString().Trim();
                        SSList_Sheet1.Cells[nRow - 1, 4].Text = Dt.Rows[i]["WARDCODE"].ToString().Trim();
                        SSList_Sheet1.Cells[nRow - 1, 5].Text = Dt.Rows[i]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(Dt.Rows[i]["JUMIN3"].ToString().Trim());
                        SSList_Sheet1.Cells[nRow - 1, 6].Text = Dt.Rows[i]["IPDNO"].ToString().Trim();
                        SSList_Sheet1.Cells[nRow - 1, 7].Text = Dt.Rows[i]["TRSNO"].ToString().Trim();
                        if (Dt.Rows[i]["GBIPD"].ToString().Trim() == "9")
                        {
                            SSList_Sheet1.Cells[nRow - 1, 8].Text = "지병";
                        }

                        SSList_Sheet1.Cells[nRow - 1, 9].Text = Dt.Rows[i]["indate"].ToString().Trim();
                    }
                }
                else
                {
                    Dt.Dispose();
                    Dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
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
        
        private void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Display_SSList();
            }
        }

        private void SSList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            string strList = string.Empty;
            int i = 0, nRow = 0, nRead = 0;

            try
            {
                FstrROWID = ""; FstrGamek = "";   FstrBohun = ""; FstrGelCode = "";

                if (e.Row < 0 || e.Column < 0)
                {
                    return;
                }

                FstrPano = SSList_Sheet1.Cells[e.Row, 0].Text.Trim();
                FstrSName = SSList_Sheet1.Cells[e.Row, 1].Text.Trim();
                FstrJumin = SSList_Sheet1.Cells[e.Row, 5].Text.Trim();
                FnIPDNO = Convert.ToInt64(SSList_Sheet1.Cells[e.Row, 6].Text);
                FnTRSNO = Convert.ToInt64(SSList_Sheet1.Cells[e.Row, 7].Text);

                SS2.ActiveSheet.ClearRange(0, 0, SS2_Sheet1.Rows.Count, SS2_Sheet1.ColumnCount, false);

                Screen_Clear();


                //환자 자격사항을 표시
                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.Pano,A.SName,A.Bi,A.Sex,A.Age,A.DeptCode,A.GBGAMEK,A.GelCode,";
                SQL += ComNum.VBLF + "        A.Bohun,TO_CHAR(B.InDate,'YYYY-MM-DD') InDate ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_TRANS B ";
                SQL += ComNum.VBLF + "  WHERE A.IPDNO=" + FnIPDNO + " ";
                SQL += ComNum.VBLF + "    AND B.TRSNO  = " + FnTRSNO + " ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    FstrInDate = Dt.Rows[0]["InDate"].ToString().Trim();

                    strList = "";
                    strList += Dt.Rows[0]["Pano"].ToString().Trim() + " ";
                    strList += Dt.Rows[0]["SName"].ToString().Trim() + " " + " ( ";
                    strList += Dt.Rows[0]["Age"].ToString().Trim() + "/";
                    strList += Dt.Rows[0]["Sex"].ToString().Trim() + " " + ")  ";
                    strList += Dt.Rows[0]["DeptCode"].ToString().Trim() + "  ";
                    strList += " 입원일: " + FstrInDate;
                    strList += " 종류: " + CF.Read_Bcode_Name(clsDB.DbCon, "BAS_환자종류", Dt.Rows[0]["Bi"].ToString().Trim());

                    FstrGamek = Dt.Rows[0]["GBGAMEK"].ToString().Trim();
                    FstrBohun = Dt.Rows[0]["Bohun"].ToString().Trim();
                    FstrGelCode = Dt.Rows[0]["GelCode"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                lblPatInfo.Text = strList;

                //입원환자 보험자격 내역
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TRSNO,GbIpd,TO_CHAR(InDate,'YYYY-MM-DD') InDate, GBGAMEK, Bohun, ";
                SQL += ComNum.VBLF + "        TO_CHAR(OutDate,'YYYY-MM-DD') OutDate,Bi,DeptCode,Kiho ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL += ComNum.VBLF + "  WHERE IPDNO=" + FnIPDNO + " ";
                SQL += ComNum.VBLF + "    AND GbIPD = '9'  ";      //지병만 표시함
                SQL += ComNum.VBLF + "  ORDER BY InDate,GbIPD ";
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
                    nRow = 0;

                    for (i = 0; i < nRead; i++)
                    {
                        nRow += 1;
                        if (nRow > SS2_Sheet1.Rows.Count)
                        {
                            SS2_Sheet1.Rows.Count = nRow;
                        }

                        SS2_Sheet1.Cells[nRow - 1, 0].Text = Dt.Rows[i]["InDate"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 1].Text = Dt.Rows[i]["Bi"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 2].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 3].Text = VB.IIf(Dt.Rows[i]["GbIPD"].ToString().Trim() == "9", "지병","").ToString();
                        SS2_Sheet1.Cells[nRow - 1, 4].Text = Dt.Rows[i]["Kiho"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 5].Text = CF.Read_MiaName(clsDB.DbCon, Dt.Rows[i]["Kiho"].ToString().Trim(), false);
                        SS2_Sheet1.Cells[nRow - 1, 6].Text = Dt.Rows[i]["OutDate"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 7].Text = Dt.Rows[i]["GBGAMEK"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 8].Text = Dt.Rows[i]["Bohun"].ToString().Trim();
                        SS2_Sheet1.Cells[nRow - 1, 9].Text = Dt.Rows[i]["TRSNO"].ToString().Trim();
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

        private void SS2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            DataTable Dt = null;
            DataTable Dt2 = null;
            string SQL = "";
            string SqlErr = "";

            string strDeptCode = string.Empty;
            string strDrcode = string.Empty;            
            string strRemark = string.Empty;
            long nAmt50 = 0;
            int i = 0;
            int Inx;

            try
            {
                if (e.Row < 0 || e.Column < 0)
                {
                    return;
                }

                FnTRSNO = Convert.ToInt64(SS2_Sheet1.Cells[e.Row, 9].Text);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT Bi,GbGamek,DeptCode,DrCode,PName,Kiho,GKiho,Gwange,Bohun,GelCode,";
                SQL += ComNum.VBLF + "        FromTrans,ErAmt,JupboNo,Remark,Amt50,";
                SQL += ComNum.VBLF + "        TO_CHAR(InDate,'YYYY-MM-DD') InDate, ";
                SQL += ComNum.VBLF + "        TO_CHAR(OutDate,'YYYY-MM-DD') OutDate ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL += ComNum.VBLF + "  WHERE TRSNO = " + FnTRSNO + " ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return;
                }
                else
                {
                    panBo1.Visible = true;
                    panBo3.Visible = true;
                    panBo4.Visible = true;
                    switch (cboBi.Text)
                    {
                        case "32":
                            panBo3.Visible = false;
                            panBo4.Visible = false;
                            panBo1.Visible = false;
                            break;
                        case "31":
                            panBo3.Visible= true;
                            panBo4.Visible = false;
                            panBo1.Visible = false;
                            break;
                        case "52":
                            panBo4.Visible = true;
                            panBo3.Visible = false;
                            panBo1.Visible = false;
                            break;
                        default:
                            panBo4.Visible = false;
                            panBo3.Visible = false;
                            panBo1.Visible = true;
                            break;
                    }

                    dtpSDate.Format = DateTimePickerFormat.Short;
                    dtpEDate.Format = DateTimePickerFormat.Short;
                    dtpSDate.Text = Dt.Rows[0]["InDate"].ToString().Trim();
                    dtpEDate.Text = Dt.Rows[0]["OutDate"].ToString().Trim(); 
                    
                    cboBi.Text = Dt.Rows[0]["Bi"].ToString().Trim(); 
                    strDeptCode = Dt.Rows[0]["DeptCode"].ToString().Trim(); 
                    strDrcode = Dt.Rows[0]["DrCode"].ToString().Trim(); 
                    strRemark = Dt.Rows[0]["Remark"].ToString().Trim();
                    nAmt50 = Convert.ToInt64(Dt.Rows[0]["Amt50"].ToString());

                    lblBi.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_환자종류", cboBi.Text);
                    cboGbGamek.Text = Dt.Rows[0]["GbGameK"].ToString().Trim();
                    lblGbGamek.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_감액코드명", cboGbGamek.Text);
                    txtGelCode.Text = Dt.Rows[0]["GelCode"].ToString().Trim();

                    if (txtGelCode.Text != "")
                    {
                        SQL = " SELECT MiaName,DelDate FROM BAS_MIA WHERE MiaCode = '" + txtGelCode.Text.ToUpper() + "'";
                        SqlErr = clsDB.GetDataTable(ref Dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (Dt2.Rows.Count == 0)
                        {
                            lblGelCode.Text = "계약처 코드 Err";
                        }
                        else
                        { 
                            lblGelCode.Text = Dt2.Rows[0]["MiaName"].ToString().Trim();
                            if (Dt2.Rows[0]["DelDate"].ToString().Trim() != "")
                            {
                                ComFunc.MsgBox(Dt2.Rows[0]["DelDate"].ToString().Trim() + "일 삭제처리된 계약처코드입니다!", "확인");
                            }
                        }
                        Dt2.Dispose();
                        Dt2 = null;
                    }
                    
                    //for (i = 0; i < cboDept.Items.Count; i++)
                    //{
                    //    if (cboDept.Items[i].ToString() == strDeptCode)
                    //    {
                    //        cboDept.SelectedIndex = i;
                    //        break;
                    //    }
                    //}

                    Inx = cboDept.FindString(strDeptCode);
                    cboDept.SelectedIndex = Inx;
                    
                    //for (i = 0; i < cboDr.Items.Count; i++)
                    //{
                    //    if (cboDr.Items[i].ToString() == strDrcode)
                    //    {
                    //        cboDr.SelectedIndex = i;
                    //        break;
                    //    }
                    //}

                    Inx = cboDr.FindString(strDrcode);
                    cboDr.SelectedIndex = Inx;

                    txtPname.Text = Dt.Rows[0]["Pname"].ToString().Trim(); 
                    txtKiho.Text = Dt.Rows[0]["Kiho"].ToString().Trim();
                    lblKiho.Text = CF.Read_MiaName(clsDB.DbCon,txtKiho.Text, false);
                    //txtGkiho2.Text = Dt.Rows[0]["GKiho"].ToString().Trim(); 
                    txtGkiho4.Text = Dt.Rows[0]["GKiho"].ToString().Trim(); 
                    txtGkiho1.Text = Dt.Rows[0]["GKiho"].ToString().Trim();    
                    cboGwange.Text = Dt.Rows[0]["Gwange"].ToString().Trim();
                    lblGwange.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_피보험자관계", cboGwange.Text);
                    cboBohun.Text = Dt.Rows[0]["Bohun"].ToString().Trim();
                    lblBohun.Text = CF.Read_Bcode_Name(clsDB.DbCon,"IPD_보훈구분", cboBohun.Text);
                    
                    txtBDate.Text = VB.Left(txtGkiho1.Text, 2) + "/" + VB.Mid(txtGkiho1.Text, 3, 2) + "/" + VB.Mid(txtGkiho1.Text, 5, 2);
                    txtStartDate.Text = VB.Mid(txtGkiho1.Text, 7, 2) + "/" + VB.Mid(txtGkiho1.Text, 9, 2) + "/" + VB.Mid(txtGkiho1.Text, 11, 2);
                    txtEndDate.Text = VB.Mid(txtGkiho1.Text, 13, 2) + "/" + VB.Mid(txtGkiho1.Text, 15, 2) + "/" + VB.Mid(txtGkiho1.Text, 17, 2);
                    
                    txtFromTrans.Text = Dt.Rows[0]["FromTrans"].ToString().Trim();
                    txtEramt.Text = Dt.Rows[0]["ErAmt"].ToString().Trim(); 
                    txtJupboNo.Text = Dt.Rows[0]["JupboNo"].ToString().Trim();

                    chkBunman.Checked = false;
                    chkBohonp.Checked = false;

                    if (VB.Left(strRemark, 2) == "NP") { chkBohonp.Checked = true; }
                    if (VB.Left(strRemark, 2) == "OG") { chkBunman.Checked = true; }
                    
                    panJob.Enabled = true;
                    btnNew.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnDelete.Enabled = false;
                    if (nAmt50 == 0) { btnDelete.Enabled = true; }
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

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDept.Text.Trim().Length < 1)
                return;

            Set_DoctCode(cboDept.Text, cboDr);
        }

        private void Set_DoctCode(string strDepts, ComboBox cboDr)
        {
            int i = 0;

            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strDept = string.Empty;

            strDept = VB.Left(strDepts, 2);

            cboDr.Items.Clear();
            cboDr.Items.Add(" ");

            if (strDept.Equals("전체") || strDept.Trim().Equals("")) { return; }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT DrCode, DrName                       ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR     ";
                SQL += ComNum.VBLF + "  WHERE (DrDept1 = '" + strDept + "'         ";
                SQL += ComNum.VBLF + "     OR DrDept2 = '" + strDept + "')         ";
                SQL += ComNum.VBLF + "    AND TOUR != 'Y'                          ";
                SQL += ComNum.VBLF + "  ORDER BY DrCode                            ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    cboDr.Items.Add(Dt.Rows[i]["DrCode"].ToString().Trim() + "." + Dt.Rows[i]["DrName"].ToString().Trim());
                }
                cboDr.SelectedIndex = 0;

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                return;
            }
        }

        private void cboBi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBi.Text.Trim().Length < 2)
            {
                return;
            }

            lblBi.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_환자종류", cboBi.Text.Trim());
        }

        private void cboGwange_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblGwange.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_피보험자관계", cboGwange.Text.Trim());
        }

        private void cboGbGamek_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGbGamek.Text.Trim().Length < 2)
                return;

            lblGbGamek.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_감액코드명", cboGbGamek.Text.Trim());
        }

        private void txtGelCode_TextChanged(object sender, EventArgs e)
        {
            Setting_GelCode(txtGelCode, lblGelCode);
            
            if (string.Compare(txtGelCode.Text, "H000") >= 0 && string.Compare(txtGelCode.Text, "H999") <= 0)
            {
                cboGbGamek.Text = "55";
                lblGbGamek.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_감액코드명", cboGbGamek.Text);
            }
            else if (cboGbGamek.Text == "55")
            {
                cboGbGamek.Text = "00";
                lblGbGamek.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_감액코드명", cboGbGamek.Text);
            }
        }

        private void Setting_GelCode(TextBox txtCode, Label lblName)
        {
            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (txtCode.Text.Trim() == "")
            {
                lblName.Text = "";
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT MiaName,DelDate FROM " + ComNum.DB_PMPA + "BAS_MIA ";
                SQL += ComNum.VBLF + "  WHERE MiaCode = '" + txtCode.Text.Trim().ToUpper() + "'";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    lblName.Text = "계약처 코드 Err";
                }
                else
                {
                    lblName.Text = Dt.Rows[0]["MiaName"].ToString().Trim();
                    if (Dt.Rows[0]["DelDate"].ToString().Trim() != "")
                    {
                        ComFunc.MsgBox(Dt.Rows[0]["DelDate"].ToString().Trim() + "일 삭제처리된 계약처코드입니다..!!", "확인");
                    }
                }

                Dt.Dispose();
                Dt = null;

            }
        }

        private void txtKiho_TextChanged(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();

            string SQL = "";
            string SqlErr = "";

            if (string.Compare(txtKiho.Text, "H001") >= 0 && string.Compare(txtKiho.Text, "H999") <= 0 && txtKiho.Text != "H020")
            {
                cboGbGamek.Text = "55";
            }
            else if (cboGbGamek.Text == "55")
            {
                cboGbGamek.Text = "00";
            }

            Setting_Kiho(txtKiho, lblKiho, txtKiho.Text);

            lblKiho.Text = "";
            
            if (string.Compare(cboBi.Text, "50") > 0)
                return;

            if (cboBi.Text.Trim() == "31" && (txtKiho.Text == "5000" || txtKiho.Text == "50001"))
                return;

            if (cboBi.Text.Trim() == "33")
                return;

            string strCheck = cboBi.Text.Trim();

            switch (strCheck)
            {
                case "41":
                    strCheck = "11";
                    break;
                case "42":
                    strCheck = "12";
                    break;
                case "43":
                    strCheck = "13";
                    break;
                default:
                    if (string.Compare(strCheck, "20") > 0 && string.Compare(strCheck, "30") < 0)
                        strCheck = "20";
                    break;
            }
            
            //직원 존속 감액이 우선 
            if (cboGbGamek.Text.Trim() != "23")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT MiaCode,MiaName                       ";
                SQL += ComNum.VBLF + "   FROM BAS_MIA                               ";
                SQL += ComNum.VBLF + "  WHERE KIHO = '" + txtKiho.Text.Trim() + "'  ";
                SQL += ComNum.VBLF + "    AND MIADETAIL = '99'                      ";
                SQL += ComNum.VBLF + "    AND (DelDate is Null OR DELDATE = '')     ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    if (VB.Left(Dt.Rows[0]["MIACODE"].ToString().Trim(), 1) == "H")
                    {
                        cboGbGamek.Text = "55";
                        txtGelCode.Text = Dt.Rows[0]["MiaCode"].ToString().Trim();
                        lblGelCode.Text = Dt.Rows[0]["MiaName"].ToString().Trim();
                    }
                }

                Dt.Dispose();
                Dt = null;

            }
        }

        public void Setting_Kiho(TextBox txtCode, Label lblName, string strKiho)
        {
            string strDeldate = string.Empty;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            lblName.Text = "";
            
            SQL = "";
            SQL += ComNum.VBLF + " SELECT MiaCode,MiaName,MiaClass,MiaDetail,    ";
            SQL += ComNum.VBLF + "        TO_CHAR(DelDate,'YYYY-MM-DD') DelDate  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA          ";
            SQL += ComNum.VBLF + "  WHERE MiaCode = '" + strKiho + "'            ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                lblName.Text = "-< ERROR >-";
            }
            else
            {
                strDeldate = Dt.Rows[0]["DelDate"].ToString().Trim();
                if (strDeldate != "" && string.Compare(strDeldate, clsPublic.GstrActDate) <= 0)
                {
                    lblName.Text = "-< 삭 제 >-";
                    return;
                }
                lblName.Text = Dt.Rows[0]["MiaName"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

        }

        private void cboBohun_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblBohun.Text = CF.Read_Bcode_Name(clsDB.DbCon,"IPD_보훈구분", cboBohun.Text.Trim());
        }

        void Nhic_View()
        {
            string strJagek = string.Empty;
            string strKiho = string.Empty, strGKiho = string.Empty;
            string strPname = string.Empty, strMCode = string.Empty;
            string strDept = string.Empty;

            strDept = VB.Left(cboDept.Text, 2).Trim();

            if (strDept.Equals(""))
            {
                strDept = "MD";
            }
            FstrSName = VB.ExtractString(FstrSName, "1");
            frmPmpaCheckNhic frmPmpaCheckNhicX = new frmPmpaCheckNhic(FstrPano, strDept, FstrSName, VB.Left(FstrJumin, 6), VB.Right(FstrJumin, 7), ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"), "");

            frmPmpaCheckNhicX.ShowDialog();

            if (clsPublic.GstrHelpCode != "")
            {
                if (VB.Pstr(clsPublic.GstrHelpCode, ";", 1) != "")
                {
                    strJagek = VB.Pstr(clsPublic.GstrHelpCode, ";", 2);
                    strKiho = VB.Pstr(clsPublic.GstrHelpCode, ";", 4);
                    strGKiho = VB.Pstr(clsPublic.GstrHelpCode, ";", 5);
                    strPname = VB.Pstr(clsPublic.GstrHelpCode, ";", 1);
                    strMCode = VB.Pstr(clsPublic.GstrHelpCode, ";", 8).Trim();
                    if (VB.Pstr(clsPublic.GstrHelpCode, ";", 9).Trim() == "Y" && cboBi.Text != "21" && cboBi.Text != "22")
                    {
                        strMCode = "F000";
                    }
                    
                    if (VB.Left(strJagek, 1) == "7")
                    {
                        cboBi.Text = "21";
                        txtKiho.Text = strKiho;
                        txtGkiho1.Text = "";
                        Setting_Kiho(txtKiho, lblKiho, strKiho);
                    }
                    else if (VB.Left(strJagek, 1) == "8")
                    {
                        cboBi.Text = "22";
                        txtKiho.Text = strKiho;
                        txtGkiho1.Text = "";
                        Setting_Kiho(txtKiho, lblKiho, strKiho);
                    }
                    else if (VB.Left(strJagek, 1) == "1" || VB.Left(strJagek, 1) == "2")
                    {
                        cboBi.Text = "13";
                        txtKiho.Text = "00000000000";
                        txtGkiho1.Text = VB.Left(strGKiho, 1) + "-" + VB.Mid(strGKiho, 2, 20).Trim();
                        lblKiho.Text = "공단(지역)";
                    }
                    else if (VB.Left(strJagek, 1) == "4" || VB.Left(strJagek, 1) == "5" || VB.Left(strJagek, 1) == "6")
                    {
                        if (VB.Left(strGKiho, 1) == "5" || VB.Left(strGKiho, 1) == "6")
                        {
                            cboBi.Text = "11";
                            txtKiho.Text = strKiho;
                            txtGkiho1.Text = VB.Left(strGKiho, 1) + "-" + VB.Mid(strGKiho, 2, 20).Trim();
                            Setting_Kiho(txtKiho, lblKiho, strKiho);
                        }
                        else
                        {
                            cboBi.Text = "12";
                            txtKiho.Text = strKiho;
                            txtGkiho1.Text = VB.Left(strGKiho, 1) + "-" + VB.Mid(strGKiho, 2, 20).Trim();
                            Setting_Kiho(txtKiho, lblKiho, strKiho);
                        }
                    }

                    if (FstrSName == strPname.Trim())
                    {
                        cboGwange.Text = "1";
                    }
                    else
                    {
                        cboGwange.Text = "5";
                    }

                    if (strMCode == "F000")
                    {
                        cboBohun.Text = "3";    //차상위 F자격이면 장애구분 3 세팅
                    }

                    lblGwange.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_피보험자관계", cboGwange.Text.Trim());

                    //자격상실 여부
                    string strData = VB.Pstr(clsPublic.GstrHelpCode, ";", 6).Trim();

                    if (strData != "")
                    {
                        ComFunc.MsgBox(strData + "일자로 자격상실", "확인");
                        return;
                    }

                    strData = VB.Pstr(clsPublic.GstrHelpCode, ";", 10).Trim();
                    if (strData == "Y")
                        ComFunc.MsgBox("해외출국자 입니다. 본인을 확인하세요", "확인");
                }
            }

            clsPublic.GstrHelpCode = "";
        }
        
        void eDeleteData()
        {
            DataTable Dt = null;
           
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (FnTRSNO == 0)
            {
                return;
            }

            clsIument cIU = null;

            SQL = " SELECT nvl(SUM(AMT1+AMT2),0) AMTSUM FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP WHERE TRSNO =" + FnTRSNO + " ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToInt64(Dt.Rows[0]["AMTSUM"].ToString()) != 0)
                {
                    ComFunc.MsgBox("삭제하려는 자격에 금액이 발생되었습니다..삭제불가", "확인");
                    Dt.Dispose();
                    Dt = null;
                    return;
                }
            }
            else
            {
                return;
            }
            Dt.Dispose();
            Dt = null;


            SQL = " SELECT nvl(SUM(AMT1+AMT2),0) AMTSUM FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP WHERE  ACTDATE < TRUNC(SYSDATE)   AND TRSNO =" + FnTRSNO + " ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToInt64(Dt.Rows[0]["AMTSUM"].ToString()) != 0)
                {
                    ComFunc.MsgBox("지병자격 삭제할려는 오늘이전 금액이 발생되었습니다..삭제불가", "확인");
                    Dt.Dispose();
                    Dt = null;
                    return;
                }
            }
            else
            {
                return;
            }
            Dt.Dispose();
            Dt = null;



            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "DELETE FROM " + ComNum.DB_PMPA + "IPD_TRANS WHERE TRSNO = " + FnTRSNO + " ";
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
                Cursor.Current = Cursors.Default;

                cIU = new clsIument();
                cIU.Ipd_Trans_Update(clsDB.DbCon, FnIPDNO);
                Screen_Clear();
                Display_SSList();
                FstrGamek = ""; FstrBohun = "";
                SSList.Focus();

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

        private void btnGelCode_Click(object sender, EventArgs e)
        {
            FrmCodehelp frm = new FrmCodehelp();
            frm.rSetHelpName += new FrmCodehelp.SetHelpName(GetValue_Help);
            frm.ShowDialog();

            txtGelCode.Text = clsPublic.GstrHelpCode;
            clsPublic.GstrHelpCode = "";
            Setting_GelCode(txtGelCode, lblGelCode);
            
            if (string.Compare(txtGelCode.Text, "H000") >= 0 && string.Compare(txtGelCode.Text, "H999") <= 0)
            {
                cboGbGamek.Text = "55";
                lblGbGamek.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_감액코드명", cboGbGamek.Text);
            }
            else if (cboGbGamek.Text == "55")
            {
                cboGbGamek.Text = "00";
                lblGbGamek.Text = CF.Read_Bcode_Name(clsDB.DbCon,"BAS_감액코드명", cboGbGamek.Text);
            }
        }
        void GetValue_Help(string strHelp)
        {
            clsPublic.GstrHelpCode = strHelp;
        }

        private void btnKiho_Click(object sender, EventArgs e)
        {
            FrmCodehelp frm = new FrmCodehelp();
            frm.rSetHelpName += new FrmCodehelp.SetHelpName(GetValue_Help);
            frm.ShowDialog();

            txtGelCode.Text = clsPublic.GstrHelpCode;
            clsPublic.GstrHelpCode = "";
            Setting_GelCode(txtGelCode, lblGelCode);
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int nBonRate = 0, nGisulRate = 0, nCNT = 0;
            string strSDate = string.Empty;
            string strDeptCode = string.Empty;
            string strDrcode = string.Empty;
            string strBi = string.Empty;
            string strFromtrans = string.Empty;
            string strErAmt = string.Empty;
            string strJupboNo = string.Empty;
            string strSanDate = string.Empty;
            string strKiho = string.Empty;
            string strGKiho = string.Empty;
            string strRemark = string.Empty;
            string strAmtSet5 = string.Empty;
            string strEdate = string.Empty;
            string strRoutDate = string.Empty;      //지병 퇴원등록시간
            string strGbSpc = string.Empty;

             
            clsPmpaFunc cPF = new clsPmpaFunc();
            clsIument cIU = new clsIument();

            try
            {
                #region Data Set
                strDeptCode = VB.Left(cboDept.Text.Trim(), 2);
                strDrcode = VB.Left(cboDr.Text.Trim(), 4);
                strBi = cboBi.Text.Trim();
                if (dtpEDate.Checked == true)
                {
                    strEdate = dtpEDate.Text.Trim();
                    strRoutDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
                }
              
               

                //if (txt_Spc_BDate.Text == "" && cboGbSpc.Text.Trim() == "1" && strDeptCode != "PD" && strDeptCode != "NP")
                //{
                //    clsPublic.GstrMsgList = "지병등록작업 - 선택신청서가 없는 환자입니다.";
                //    clsPublic.GstrMsgList += ComNum.VBLF + ComNum.VBLF + "신청서 없이 작업하시겠습니까??(예외일경우만 작업하십시오)";
                //    if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                //        return;
                //}

                strGbSpc = cboGbSpc.Text.Trim();
                if (strGbSpc == "")
                    strGbSpc = "0";

                if (strEdate == "")
                {
                    clsPublic.GstrMsgList = "지병등록시 퇴원일자가 공란입니다..이대로 등록하시겠습니까?";
                    if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                    {
                        strEdate = clsPublic.GstrSysDate;
                        strRoutDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
                    }
                    strRoutDate = "";
                }

                //보험자격을 Setting
                strKiho = txtKiho.Text.Trim();

                switch (cboBi.Text.Trim())
                {
                    case "32":
                        //strGKiho = txtGkiho2.Text.Trim();
                        break;
                    case "31":
                        strSanDate = VB.Left(txtBDate.Text, 2) + VB.Mid(txtBDate.Text, 4, 2) + VB.Mid(txtBDate.Text, 7, 2);
                        strSanDate += VB.Left(txtStartDate.Text, 2) + VB.Mid(txtStartDate.Text, 4, 2) + VB.Mid(txtStartDate.Text, 7, 2);
                        strSanDate += VB.Left(txtEndDate.Text, 2) + VB.Mid(txtEndDate.Text, 4, 2) + VB.Mid(txtEndDate.Text, 7, 2);
                        strGKiho = strSanDate;
                        break;
                    case "52":
                    case "55": strGKiho = txtGkiho4.Text.Trim(); break;
                    default: strGKiho = txtGkiho1.Text.Trim(); break;
                }

                if (cboBi.Text == "52" || cboBi.Text == "55")
                {
                    strFromtrans = txtFromTrans.Text.Trim();
                    strErAmt = txtEramt.Text.Trim();
                    strJupboNo = txtJupboNo.Text.Trim();
                }
                else
                {
                    strFromtrans = "";
                    strErAmt = "0";
                    strJupboNo = "";
                }
                #endregion

                #region Data_Check
                strRemark = "";
                if (chkBohonp.Checked) { strRemark = "NP 보호정액"; }
                if (chkBunman.Checked) { strRemark = "OG 기왕증"; }
                if (strDrcode == "") { ComFunc.MsgBox("의사코드가 공란입니다.", "오류"); return; }
                if (strBi == "") { ComFunc.MsgBox("환자종류가 공란입니다.", "오류"); return; }

                //의사코드 오류를 점검
                SQL = "";
                SQL += ComNum.VBLF + " SELECT DrDept1 FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL += ComNum.VBLF + "  WHERE DrCode='" + strDrcode + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    ComFunc.MsgBox("해당 의사코드가 등록 않됨.", "의사오류");
                    cboDr.Focus();
                    return;
                }
                else
                {
                    if (Dt.Rows[0]["DrDept1"].ToString().Trim() != strDeptCode)
                    {
                        Dt.Dispose();
                        Dt = null;
                        ComFunc.MsgBox("해당과의 의사코드가 아님.", "의사오류");
                        cboDept.Focus();
                        return;
                    }

                    if (VB.Right(strDrcode, 2) == "99")
                    {
                        //2010-08-26 심사과 풀어주기 요청
                        if (ComFunc.MsgBoxQ("진료과장을 선택안하고 작업을 진행하시겠습니까", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                        {
                            Dt.Dispose();
                            Dt = null;
                            return;
                        }
                    }
                }

                Dt.Dispose();
                Dt = null;

                if (clsLockCheck.GstrLockPtno != "")
                {
                    clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                    clsLockCheck.GstrLockPtno = "";
                }

                //동일한 시작일에 지병등록이 있는지 Check
                if (FnTRSNO == 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT COUNT(*) CNT FROM IPD_TRANS ";
                    SQL += ComNum.VBLF + " WHERE Pano='" + FstrPano + "' ";
                    SQL += ComNum.VBLF + "   AND InDate=TO_DATE('" + dtpSDate.Text + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND GbIPD='9' "; //지병;
                    SQL += ComNum.VBLF + "   AND DeptCode ='" + cboDept.Text.Trim() + "' ";  //2013-02-06 지병등록시 과체크 하여 입원일자동일 허용함
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (Dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt16(Dt.Rows[0]["CNT"].ToString()) > 0)
                        {
                            Dt.Dispose();
                            Dt = null;
                            ComFunc.MsgBox("동일날짜에 지병이 2건 등록됨.", "오류");
                            return;
                        }
                    }
                    Dt.Dispose();
                    Dt = null;
                }
                #endregion

                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(clsDB.DbCon);

                nBonRate = cPF.READ_IpdBon_Rate_CHK(clsDB.DbCon, strBi, FstrInDate);  //본인부담율
                nGisulRate = cPF.READ_Gisul_Rate(clsDB.DbCon, strBi, FstrInDate);     //기술료 가산

                //퇴원종류 확인
                SQL = "";
                SQL += ComNum.VBLF + " SELECT AMSET5 FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL += ComNum.VBLF + "  WHERE TRSNO  = " + FnTRSNO + " ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    strAmtSet5 = Dt.Rows[0]["AMSET5"].ToString().Trim(); 
                }
                Dt.Dispose();
                Dt = null;

                //2011-04-22 지병건 퇴원걸릴경우 퇴원종류 1:완쾌세팅
                if (strEdate != "")
                    strAmtSet5 = "1";

                #region IPD_TRANS에 UPDATE
                if (FnTRSNO == 0)
                { 
                    FnTRSNO = cPF.GET_NEXT_TRSNO(clsDB.DbCon);   //IPD_TRANS 의 고유번호

                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "IPD_TRANS (";
                    SQL += ComNum.VBLF + " TRSNO,IPDNO,PANO,GBIPD,INDATE,OUTDATE,DEPTCODE,DRCODE,ILSU,BI,KIHO,";
                    SQL += ComNum.VBLF + " GKIHO,PNAME,GWANGE,BONRATE,GISULRATE,GBGAMEK,GelCode,BOHUN,AMSET1,AMSET2,AMSET3,AMSET4,";
                    SQL += ComNum.VBLF + " AMSET5,AMSETB,FROMTRANS,ERAMT,REMARK,JUPBONO,GBDRG,DRGWRTNO,SANGAMT,DTGAMEK,";
                    SQL += ComNum.VBLF + " OGPDBUN,AMT01,AMT02,AMT03,AMT04,AMT05,AMT06,AMT07,AMT08,AMT09,AMT10,AMT11,";
                    SQL += ComNum.VBLF + " AMT12,AMT13,AMT14,AMT15,AMT16,AMT17,AMT18,AMT19,AMT20,AMT21,AMT22,AMT23,AMT24,AMT25,";
                    SQL += ComNum.VBLF + " AMT26,AMT27,AMT28,AMT29,AMT30,AMT31,AMT32,AMT33,AMT34,AMT35,AMT36,AMT37,AMT38,AMT39,";
                    SQL += ComNum.VBLF + " AMT40,AMT41,AMT42,AMT43,AMT44,AMT45,AMT46,AMT47,AMT48,AMT49,AMT50,AMT51,AMT52,AMT53,";
                    SQL += ComNum.VBLF + " AMT54,AMT55,AMT56,AMT57,AMT58,AMT59,AMT60,Amt64,ENTDATE,ENTSABUN,GBSTS,ROutDate,GbSPC) ";
                    SQL += ComNum.VBLF + " VALUES ( ";
                    SQL += ComNum.VBLF + " " + FnTRSNO + "," + FnIPDNO + ",'" + FstrPano + "','9',";
                    SQL += ComNum.VBLF + " TO_DATE('" + dtpSDate.Text + "','YYYY-MM-DD'), TO_DATE('" + strEdate + "','YYYY-MM-DD'), '" + strDeptCode + "',";
                    SQL += ComNum.VBLF + " '" + strDrcode + "',0,'" + strBi + "','" + strKiho + "','" + strGKiho + "',";
                    SQL += ComNum.VBLF + " '" + txtPname.Text.Trim() + "','" + cboGwange.Text.Trim() + "',";
                    SQL += ComNum.VBLF + nBonRate + "," + nGisulRate + ",'" + cboGbGamek.Text.Trim() + "','" + txtGelCode.Text.Trim() + "',";
                    SQL += ComNum.VBLF + " '" + cboBohun.Text.Trim() + "','0','0',' ',' ','" + strAmtSet5 + "',' ',";
                    SQL += ComNum.VBLF + " '" + strFromtrans + "','" + strErAmt + "','" + strRemark + "', ";
                    SQL += ComNum.VBLF + " '" + strJupboNo + "','0',0,0,0,' ', ";
                    SQL += ComNum.VBLF + " 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0, ";
                    SQL += ComNum.VBLF + " 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0, ";
                    if (strEdate == "")
                    {
                        //2011-02-18
                        SQL = SQL + "SYSDATE," + clsType.User.IdNumber + ",'0',TO_DATE('" + strRoutDate + "','YYYY-MM-DD HH24:MI'),'" + strGbSpc + "' ) ";
                    }
                    else
                    {
                        SQL = SQL + "SYSDATE," + clsType.User.IdNumber + ",'3',TO_DATE('" + strRoutDate + "','YYYY-MM-DD HH24:MI'),'" + strGbSpc + "' ) ";
                    }
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS ";
                    SQL += ComNum.VBLF + "    SET InDate=TO_DATE('" + dtpSDate.Text + "','YYYY-MM-DD'),";
                    SQL += ComNum.VBLF + "        OutDate=TO_DATE('" + strEdate + "','YYYY-MM-DD'),";
                    SQL += ComNum.VBLF + "        DeptCode='" + strDeptCode + "',";
                    SQL += ComNum.VBLF + "        DrCode='" + strDrcode + "',";
                    SQL += ComNum.VBLF + "        Bi='" + strBi + "',";
                    SQL += ComNum.VBLF + "        Kiho='" + strKiho + "',";
                    SQL += ComNum.VBLF + "        GKiho='" + strGKiho + "',";
                    SQL += ComNum.VBLF + "        PName='" + txtPname.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "        Gwange='" + cboGwange.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "        BonRate=" + nBonRate + ",";
                    SQL += ComNum.VBLF + "        GisulRate=" + nGisulRate + ",";
                    SQL += ComNum.VBLF + "        GbGamek='" + cboGbGamek.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "        GelCode='" + txtGelCode.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "        Bohun='" + cboBohun.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "        FromTrans = '" + strFromtrans + "',";
                    SQL += ComNum.VBLF + "        ErAmt = '" + strErAmt + "',";
                    SQL += ComNum.VBLF + "        Remark = '" + strRemark + "',";
                    SQL += ComNum.VBLF + "        JupboNo='" + strJupboNo + "',";
                    SQL += ComNum.VBLF + "        EntDate=SYSDATE,EntSabun=" + clsType.User.IdNumber + " ";
                    SQL += ComNum.VBLF + " WHERE TRSNO=" + FnTRSNO + " ";
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
                #endregion

                nCNT = 0;

                //IPD_MASTER에 TRANS건수 및 최종 TRSNO를 SET
                SQL = "";
                SQL += ComNum.VBLF + " SELECT COUNT(*) CNT FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL += ComNum.VBLF + "  WHERE IPDNO = " + FnIPDNO + " ";
                SQL += ComNum.VBLF + "    AND GbIPD IN ('1','9') ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (Dt.Rows.Count > 0)
                {
                    nCNT = Convert.ToInt16(Dt.Rows[0]["CNT"].ToString());
                }

                Dt.Dispose();
                Dt = null;

                #region IPD_NEW_MASTER UpDate
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + "    SET TRSCNT=" + nCNT + " ";
                SQL += ComNum.VBLF + "  WHERE IPDNO=" + FnIPDNO + " ";
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

                clsDB.setCommitTran(clsDB.DbCon);

                cIU.Ipd_Trans_Update(clsDB.DbCon, FnIPDNO);

                Cursor.Current = Cursors.Default;

                Update_Bas_Mih();  //보험내역

                Screen_Clear();
                Display_SSList();
                FstrGamek = "";
                FstrBohun = "";

                SSList.Focus();
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                if (clsDB.DbCon.Trs != null)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void cboDr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDept.Text != "" && cboDr.Text != "")
            {
                if (ComFunc.READ_SELECT_DOCTOR_CHK(clsDB.DbCon, "", cboDr.Text.Trim()) == "OK")
                {
                    txt_Spc_BDate.BackColor = Color.LightGoldenrodYellow;
                    txt_Spc_BDate.Text = ComFunc.READ_PANO_SELECT_MST_BDATE(clsDB.DbCon,FstrPano, "I", VB.Left(cboDr.Text, 4), VB.Left(dtpSDate.Text, 10));
                    cboGbSpc.Text = "1";
                }
                else
                { 
                    txt_Spc_BDate.BackColor = Color.White;
                    txt_Spc_BDate.Text = "";
                    cboGbSpc.Text = "0";
                }
            }
        }

        private void Update_Bas_Mih()//
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            
            string strMihPname = string.Empty;
            string strMihKiho = string.Empty;
            string strMihGKiho = string.Empty;
            string strMihGwange = string.Empty;
            string strInsKiho = string.Empty;
            string strInsGkiho = string.Empty;
            string strRowid = string.Empty;

            Cursor.Current = Cursors.WaitCursor;

             
            

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                //*------( 등록번호,종류,일자 같은것 Select )---------*
                strInsKiho = txtKiho.Text.Trim();
                strInsGkiho = txtGkiho1.Text.Trim();
                string strSDate = dtpSDate.Value.ToString("yyyy-MM-dd");

                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(TransDate,'yyyy-mm-dd') TransDate,Rowid      ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIH                        ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + FstrPano + "'                            ";
                SQL += ComNum.VBLF + "    AND Bi = '" + cboBi.Text + "'                            ";
                SQL += ComNum.VBLF + "    AND TransDate = TO_DATE('" + strSDate + "','YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다" + ComNum.VBLF + SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (Dt.Rows.Count == 1)         //같은것이 있으면 Update
                {
                    strRowid = Dt.Rows[0]["ROWID"].ToString().Trim();

                    if (Bas_Mih_Update(clsDB.DbCon, txtPname.Text.Trim(), cboGwange.Text.Trim(), strInsGkiho, strInsKiho, strRowid) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                   
                }

                Dt.Dispose();
                Dt = null;


                //*------( 등록번호,종류가 같은것 Select )---------*
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Kiho,GKiho,Gwange, Pname, RowId,          ";
                SQL += ComNum.VBLF + "        TO_CHAR(TransDate,'yyyy-mm-dd') TransDate ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIH             ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + FstrPano + "'                 ";
                SQL += ComNum.VBLF + "    AND Bi = '" + cboBi.Text + "'                 ";
                SQL += ComNum.VBLF + "  ORDER BY TransDate Desc                         ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다" + ComNum.VBLF + SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
                if (Dt.Rows.Count == 0)         //같은 환자종류의 자료가 없으면 Insert후 종료함
                {
                    if (Bas_Mih_Insert(clsDB.DbCon, cboBi.Text.Trim(), dtpSDate.Text, txtPname.Text.Trim(), cboGwange.Text.Trim(), strInsKiho, strInsGkiho) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                   
                }
                else
                {
                    strRowid = Dt.Rows[0]["RowId"].ToString().Trim();
                    strMihKiho = Dt.Rows[0]["Kiho"].ToString().Trim();
                    strMihGKiho = Dt.Rows[0]["GKiho"].ToString().Trim(); 
                    strMihGwange = Dt.Rows[0]["Gwange"].ToString().Trim(); 
                    strMihPname = Dt.Rows[0]["Pname"].ToString().Trim(); 
                }

                Dt.Dispose();
                Dt = null;

                //*---( 보험100%,일반,계약처,미확인은 최종분의 내역만 관리함 )---------*
                string strBi = cboBi.Text.Trim();

                if (strBi.Equals("41") || strBi.Equals("42") || strBi.Equals("43") || strBi.Equals("51") || strBi.Equals("55"))
                {
                    if (Bas_Mih_Update(clsDB.DbCon, txtPname.Text.Trim(), cboGwange.Text.Trim(), strInsGkiho, strInsKiho, strRowid) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    
                }

                //*-----( 동일한 내역의 보험사항이 있는지 Check )---------------*
                if (strInsKiho != strMihKiho)   // 조합기호가 틀리면 Insert
                {
                    if (Bas_Mih_Insert(clsDB.DbCon, cboBi.Text.Trim(), dtpSDate.Text, txtPname.Text.Trim(), cboGwange.Text.Trim(), strInsKiho, strInsGkiho) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                
                }
                
                if (cboBi.Text.Trim() == "31" || cboBi.Text.Trim() == "52")    //산재,자보 증번호 관리안함
                {
                    if (Bas_Mih_Update(clsDB.DbCon, txtPname.Text.Trim(), cboGwange.Text.Trim(), strInsGkiho, strInsKiho, strRowid) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    
                }

                if (strInsGkiho != strMihGKiho)     //보험,보호환자는 증번호가 틀리면 Insert함
                {
                    if (Bas_Mih_Insert(clsDB.DbCon, cboBi.Text.Trim(), dtpSDate.Text, txtPname.Text.Trim(), cboGwange.Text.Trim(), strInsKiho, strInsGkiho) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                  
                }

                if (Bas_Mih_Update(clsDB.DbCon, txtPname.Text.Trim(), cboGwange.Text.Trim(), strInsGkiho, strInsKiho, strRowid) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                clsDB.setCommitTran(clsDB.DbCon);         
                       
                Cursor.Current = Cursors.Default;

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private bool Bas_Mih_Update(PsmhDb pDbCon, string strPname, string strGwange, string strInsGkiho, string strInsKiho, string strRowid)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_MIH     ";
                SQL += ComNum.VBLF + "    SET Pname = '" + strPname + "',       ";  
                SQL += ComNum.VBLF + "        Gwange = '" + strGwange + "',     ";
                SQL += ComNum.VBLF + "        GKiho = '" + strInsGkiho + "',    ";
                SQL += ComNum.VBLF + "        Kiho = '" + strInsKiho + "'       ";
                SQL += ComNum.VBLF + "  WHERE Rowid = '" + strRowid + "'        ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private bool Bas_Mih_Insert(PsmhDb pDbCon, string strBi, string strSDate, string strPname, string strGwange, string strInsKiho, string strInsGkiho)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            
            if (strBi.Trim() == "")
            {
                ComFunc.MsgBox("보험유형이 공란임", "알림");
                return false;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_MIH            ";
                SQL += ComNum.VBLF + "        (Pano,Bi,TransDate,Pname,Gwange,Kiho,Gkiho)   ";
                SQL += ComNum.VBLF + " VALUES                                               ";
                SQL += ComNum.VBLF + "        ('" + FstrPano + "', '" + strBi + "',         ";
                SQL += ComNum.VBLF + "        TO_DATE('" + strSDate + "','YYYY-MM-DD'),     ";
                SQL += ComNum.VBLF + "        '" + strPname + "','" + strGwange + "',       ";
                SQL += ComNum.VBLF + "        '" + strInsKiho + "','" + strInsGkiho + "' )  ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void panList_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
