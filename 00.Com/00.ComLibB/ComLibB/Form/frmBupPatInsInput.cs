using ComBase;
using ComDbB;
using ComLibB;
using Oracle.DataAccess.Client;
using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;


namespace ComLibB
{
    public partial class frmBupPatInsInput : Form, MainFormMessage
    {
        
        #region //MainFormMessage
        //string mPara1 = "";
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
        #endregion //MainFormMessage

        string fstrDefaultPano = "";
        string fstrDefaultDeptcode = "";
        string fstrDefaultDrcode = "";

        #region 환자기본정보 변수
        string strPANO = "";
        string strSNAME = "";
        string strBALDATE = "";
        string strDEPTCODE = "";
        string strDRCODE = "";
        string strDRNAME = "";
        string strDRBUNHO1 = "";
        string strDRBUNHO2 = "";
        string strBINO = "";
        string strTEL = "";
        string strPHONE = "";
        string strILLNAME = "";
        string strILLCODE = "";
        string strROWID = "";
        string strDrSabun = "";
        #endregion

        public frmBupPatInsInput()
        {
            InitializeComponent();
            setEvent();
        }


        public frmBupPatInsInput(string strPano, string strDeptcode, string strDrcode)
        {
            InitializeComponent();
            fstrDefaultPano = strPano;
            fstrDefaultDeptcode = strDeptcode;
            fstrDefaultDrcode = strDrcode;
            setEvent();
        }

        public frmBupPatInsInput(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            setEvent();
        }


        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnClose.Click += new EventHandler(eBtnClick);

            this.btn1New.Click += new EventHandler(eBtnClick);
            this.btn1Save.Click += new EventHandler(eBtnClick);
            this.btn1Prt.Click += new EventHandler(eBtnClick);
            this.btn1Del.Click += new EventHandler(eBtnClick);

            this.btn2New.Click += new EventHandler(eBtnClick);
            this.btn2Save.Click += new EventHandler(eBtnClick);
            this.btn2Prt.Click += new EventHandler(eBtnClick);
            this.btn2Del.Click += new EventHandler(eBtnClick);

            this.btn3New.Click += new EventHandler(eBtnClick);
            this.btn3Save.Click += new EventHandler(eBtnClick);
            this.btn3Prt.Click += new EventHandler(eBtnClick);
            this.btn3Del.Click += new EventHandler(eBtnClick);

            this.btn4New.Click += new EventHandler(eBtnClick);
            this.btn4Save.Click += new EventHandler(eBtnClick);
            this.btn4Prt.Click += new EventHandler(eBtnClick);
            this.btn4Del.Click += new EventHandler(eBtnClick);

            this.txtPano.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtPano.LostFocus += new EventHandler(eControl_LostFocus);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadCellClick);

            this.cboDEPTCODE.SelectedIndexChanged += new EventHandler(eComboBoxChange);
            this.cboDRCODE.SelectedIndexChanged += new EventHandler(eComboBoxChange);


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
                //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                ComFunc.ReadSysDate(clsDB.DbCon);

                ComFunc cf = new ComFunc();
                dtpSdate.Text = cf.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -200);
                dtpEdate.Text = cf.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, 0);

                cboGubun.Items.Clear();
                cboGubun.Items.Add("**.전체");
                cboGubun.Items.Add("01.보험환자용");
                cboGubun.Items.Add("02.급여,차상위환자용");
                cboGubun.Items.Add("03.연속혈당측정용 전극용");
                cboGubun.Items.Add("04.연속혈당측정기,인슐린주입기");
                cboGubun.SelectedIndex = 0;

                clsVbfunc.SetComboDept(clsDB.DbCon, cboDEPTCODE, "1", 1);
                clsVbfunc.SetDrCodeCombo(clsDB.DbCon, cboDRCODE, VB.Left(cboDEPTCODE.Text, 2), "1", 1, "");

                cboDEPTCODE.SelectedIndex = 0;
                cboDRCODE.SelectedIndex = 0;

                readList();

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

        void eBtnClick(object sender, EventArgs e)
        {

            if (sender == this.btn1New || sender == this.btn2New || sender == this.btn3New || sender == this.btn4New)
            {
                clearScreen();
                readHeader(txtPano.Text.Trim());
            }
            else if (sender == this.btn1Save || sender == this.btn2Save || sender == this.btn3Save || sender == this.btn4Save)
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                if (sender == this.btn1Save)
                {
                    if(strROWID != "") delete01(strROWID);
                    insert01();
                }
                else if (sender == this.btn2Save)
                {
                    if (strROWID != "") delete02(strROWID);
                    insert02();
                }
                else if (sender == this.btn3Save)
                {
                    if (strROWID != "") delete03(strROWID);
                    insert03();
                }
                else if (sender == this.btn4Save)
                {
                    if (strROWID != "") delete04(strROWID);
                    insert04();
                }

                readList();
            }
            else if (sender == this.btn1Del || sender == this.btn2Del || sender == this.btn3Del || sender == this.btn4Del)
            {
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                if (strROWID == "")
                {
                    return;
                }
                if (sender == this.btn1Del)
                {
                    delete01(strROWID);
                }
                else if (sender == this.btn2Del)
                {
                    delete02(strROWID);
                }
                else if (sender == this.btn3Del)
                {
                    delete03(strROWID);
                }
                else if (sender == this.btn4Del)
                {
                    delete04(strROWID);
                }

                readList();
            }
            else if (sender == this.btn1Prt || sender == this.btn2Prt || sender == this.btn3Prt || sender == this.btn4Prt)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                if (strROWID == "")
                {
                    return;
                }
                if (sender == this.btn1Prt)
                {
                    print01();
                }
                else if(sender == this.btn2Prt)
                {
                    print02();
                }
                else if (sender == this.btn3Prt)
                {
                    print03();
                }
                else if (sender == this.btn4Prt)
                {
                    print04();
                }
            }

            else if (sender == this.btnSearch)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                readList();
            }

            else if (sender == this.btnClose)
            {
                this.Close();
                return;
            }

        }

        void eSpreadCellClick(object sender, CellClickEventArgs e)
        {
            string strGUBUN = "";

            strROWID = ssList_Sheet1.Cells[e.Row, 5].Text.Trim();
            strGUBUN = ssList_Sheet1.Cells[e.Row, 4].Text.Trim();

            readData(strGUBUN, strROWID);

        }


        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPano)
            {
                txtPano.Text = txtPano.Text.Trim();
                if (txtPano.Text == "")
                {
                    return;
                }

                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

                ComFunc cf = new ComFunc();

                txtSname.Text = cf.Read_Patient(clsDB.DbCon, txtPano.Text, "2");
            }
        }

        void eComboBoxChange(object sender, EventArgs e)
        {
            if (sender == this.cboDEPTCODE)
            {
                clsVbfunc.SetDrCodeCombo(clsDB.DbCon, cboDRCODE, VB.Left(cboDEPTCODE.Text, 2), "1", 1, "");
                cboDRCODE.SelectedIndex = 0;
            }
            else if (sender == this.cboDRCODE)
            {
                txtDRBUNHO1.Text = clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(cboDRCODE.Text, ".")[0]);

                txtDRBUNHO2.Text = GetInsaCertNo(clsDB.DbCon, VB.Split(cboDRCODE.Text, ".")[0], VB.Left(cboDEPTCODE.Text, 2));
            }

        }

        private void readList()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            clearScreen();

            ssList_Sheet1.RowCount = 0;

            string SQL1 = "";

            SQL1 = "";
            SQL1 += ComNum.VBLF + "  WHERE BALDATE >= TO_DATE('" + dtpSdate.Text + "', 'YYYY-MM-DD') ";
            SQL1 += ComNum.VBLF + "    AND BALDATE <= TO_DATE('" + dtpEdate.Text + "', 'YYYY-MM-DD') ";
            if (txtPano.Text.Trim() != "")
            {
                SQL1 += ComNum.VBLF + "    AND PANO = '" + txtPano.Text.Trim() + "' ";
            }

            ComFunc cf = new ComFunc();

            try
            {
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BALDATE,'YYYY-MM-DD') BALDATE, PANO, SNAME, '01' GBN, '보험환자용' GBNK, ROWID ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_PRES_INSULIN01 ";
                SQL += SQL1;
                SQL += ComNum.VBLF + "  UNION ALL ";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BALDATE,'YYYY-MM-DD') BALDATE, PANO, SNAME, '02' GBN, '급여,차상위환자' GBNK, ROWID ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_PRES_INSULIN02 ";
                SQL += SQL1;
                SQL += ComNum.VBLF + "  UNION ALL ";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BALDATE,'YYYY-MM-DD') BALDATE, PANO, SNAME, '03' GBN, '연속혈당측정용,전극용' GBNK, ROWID ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_PRES_INSULIN03 ";
                SQL += SQL1;
                SQL += ComNum.VBLF + "  UNION ALL ";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BALDATE,'YYYY-MM-DD') BALDATE, PANO, SNAME, '04' GBN, '연속혈당측정기,인슐린자동주입기' GBNK, ROWID ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_PRES_INSULIN04 ";
                SQL += SQL1;
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BALDATE"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["GBNK"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GBN"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }


        }

        private void readData(string strGbn, string strRowid)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            clearScreen01();
            clearScreen02();
            clearScreen03();
            clearScreen04();

            if (strGbn != "01" && strGbn != "02" && strGbn != "03" && strGbn != "04")
            {
                ComFunc.MsgBox("선택한 처방전 구분이 잘못되었습니다.", "확인");
                return;
            }

            if (strRowid == "" )
            {
                ComFunc.MsgBox("처방전 읽기 오류입니다.", "확인");
                return;
            }

            if (strGbn == "01")
            {
                select01(strRowid);
                superTabControl1.SelectedTabIndex = 0;

            }
            else if (strGbn == "02")
            {
                select02(strRowid);
                superTabControl1.SelectedTabIndex = 1;
            }
            else if (strGbn == "03")
            {
                select03(strRowid);
                superTabControl1.SelectedTabIndex = 2;
            }
            else if (strGbn == "04")
            {
                select04(strRowid);
                superTabControl1.SelectedTabIndex = 3;
            }
        }

        private void readHeader(string strPANO)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ComFunc cf = new ComFunc();

            if (strPANO == "")
            {
                ComFunc.MsgBox("환자번호가 공란입니다.", "환자번호 확인");
                return;
            }

            try
            {
                SQL = " SELECT GKIHO, JUMIN1, JUMIN2, JUMIN3, SNAME, TEL, HPHONE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + strPANO + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    txtPano.Text = strPANO;
                    txtBINO.Text = dt.Rows[0]["GKIHO"].ToString().Trim();
                    txtSname.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    txtNAME.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    txtJUMIN.Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    txtTEL.Text = dt.Rows[0]["TEL"].ToString().Trim();
                    txtPHONE.Text = dt.Rows[0]["HPHONE"].ToString().Trim();
                    //cboDEPTCODE.SelectedIndex = 0;
                    //txtDRBUNHO1.Text = "";
                    //txtILLNAME.Text = "";
                    //txtILLCODE.Text = "";
                    //cboDRCODE.SelectedIndex = 0;
                    //txtDRBUNHO2.Text = "";

                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void readJumin(string strPANO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ComFunc cf = new ComFunc();

            if (strPANO == "")
            {
                ComFunc.MsgBox("환자번호가 공란입니다.", "환자번호 확인");
                return;
            }

            try
            {
                SQL = " SELECT GKIHO, JUMIN1, JUMIN2, JUMIN3, SNAME, TEL, HPHONE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + strPANO + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    txtJUMIN.Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private bool getHeader()
        {

            strPANO = txtPano.Text.Trim();
            strSNAME = txtNAME.Text.Trim();
            strBALDATE = dtpBALDATE.Text.Trim();
            strDEPTCODE = VB.Split(cboDEPTCODE.Text, ".")[0];
            strDRCODE = VB.Split(cboDRCODE.Text, ".")[0];
            strDRNAME = VB.Split(cboDRCODE.Text, ".")[1];
            strDRBUNHO1 = txtDRBUNHO1.Text.Trim();
            strDRBUNHO2 = txtDRBUNHO2.Text.Trim();
            strBINO = txtBINO.Text.Trim();
            strTEL = txtTEL.Text.Trim();
            strPHONE = txtPHONE.Text.Trim();
            strILLNAME = txtILLNAME.Text.Trim();
            strILLCODE = txtILLCODE.Text.Trim();

            return true;
        }

        private void setHeader(DataTable dt1)
        {

            ComFunc cf = new ComFunc();

            string strPANO = dt1.Rows[0]["PANO"].ToString().Trim();
            txtPano.Text = strPANO;
            txtSname.Text = dt1.Rows[0]["SNAME"].ToString().Trim();
            txtBINO.Text = dt1.Rows[0]["BINO"].ToString().Trim();
            readJumin(strPANO);
            txtNAME.Text = dt1.Rows[0]["SNAME"].ToString().Trim();
            dtpBALDATE.Text = dt1.Rows[0]["BALDATE"].ToString().Trim();
            txtTEL.Text = dt1.Rows[0]["TEL"].ToString().Trim();
            txtPHONE.Text = dt1.Rows[0]["PHONE"].ToString().Trim();
            cboDEPTCODE.Text = dt1.Rows[0]["DEPTCODE"].ToString().Trim() + "." + readDeptNameK(dt1.Rows[0]["DEPTCODE"].ToString().Trim());
            txtILLNAME.Text = dt1.Rows[0]["ILLNAME"].ToString().Trim();
            txtILLCODE.Text = dt1.Rows[0]["ILLCODE"].ToString().Trim();
            cboDRCODE.Text = dt1.Rows[0]["DRCODE"].ToString().Trim() + "." + dt1.Rows[0]["DRNAME"].ToString().Trim();
            txtDRBUNHO1.Text = dt1.Rows[0]["DRBUNHO1"].ToString().Trim();
            txtDRBUNHO2.Text = dt1.Rows[0]["DRBUNHO2"].ToString().Trim();
            

            dt1.Dispose();
            dt1 = null;
        }

        private void clearScreen()
        {
            int i = 0;
     
            ComFunc.ReadSysDate(clsDB.DbCon);

            ComFunc cf = new ComFunc();
            dtpBALDATE.Text = clsPublic.GstrSysDate;

            if (fstrDefaultPano != "")
            {
                readHeader(fstrDefaultPano);
                for (i = 0; i < cboDEPTCODE.Items.Count; i++)
                {
                    cboDEPTCODE.SelectedIndex = i;
                    if (fstrDefaultDeptcode == VB.Left(cboDEPTCODE.Text, 2))
                    {
                        break;
                    }
                }

                for (i = 0; i < cboDRCODE.Items.Count; i++)
                {
                    cboDRCODE.SelectedIndex = i;
                    if (fstrDefaultDrcode == VB.Left(cboDRCODE.Text, 4))
                    {
                        break;
                    }
                }
                
            }
            else
            {
                cboDEPTCODE.SelectedIndex = 0;
                txtDRBUNHO1.Text = "";
                cboDRCODE.SelectedIndex = 0;
                txtDRBUNHO2.Text = "";
                txtBINO.Text = "";
                txtTEL.Text = "";
                txtPHONE.Text = "";
                txtILLNAME.Text = "";
                txtILLCODE.Text = "";
            }

            strROWID = "";

            clearScreen01();
            clearScreen02();
            clearScreen03();
            clearScreen04();
        }

        private void clearScreen01()
        {

            chk1_1.Checked = false;
            chk1_2.Checked = false;
            chk1_2_1.Checked = false;
            chk1_2_2.Checked = false;
            chk1_2_3.Checked = false;
            chk1_2_4.Checked = false;
            chk1_2_5.Checked = false;
            chk1_3.Checked = false;
            chk1_3_1.Checked = false;
            chk1_3_2.Checked = false;
            txt1_3_3.Text = "";
            chk1_4_1.Checked = false;
            chk1_4_2.Checked = false;
            chk1_4_3.Checked = false;
            chk1_4_4.Checked = false;
            chk1_4_5.Checked = false;
            txt1_5.Text = "";
            txt1_6_1.Text = "";
            txt1_6_2.Text = "";
            spreadClear01();
        }

        private void clearScreen02()
        {
            chk2_1_1.Checked = false;
            chk2_1_2.Checked = false;
            chk2_2_1.Checked = false;
            chk2_2_2.Checked = false;
            chk2_2_3.Checked = false;
            chk2_2_4.Checked = false;
            chk2_2_5.Checked = false;
            chk2_2_6.Checked = false;
            chk2_3_1.Checked = false;
            chk2_3_2.Checked = false;
            chk2_3_3.Checked = false;
            chk2_3_4.Checked = false;
            chk2_3_5.Checked = false;
            chk2_3_6.Checked = false;
            chk2_4_1.Checked = false;
            chk2_4_2.Checked = false;
            chk2_4_3.Checked = false;
            chk2_4_4.Checked = false;
            chk2_4_5.Checked = false;
            chk2_4_6.Checked = false;
            chk2_4_7.Checked = false;
            chk2_4_8.Checked = false;
            chk2_5_1.Checked = false;
            chk2_5_2.Checked = false;
            chk2_5_3.Checked = false;
            txt2_5_4.Text = "";
            chk2_6_1.Checked = false;
            chk2_6_2.Checked = false;
            chk2_6_3.Checked = false;
            chk2_6_4.Checked = false;
            txt2_7_1.Text = "";
            txt2_8_1.Text = "";
            txt2_8_2.Text = "";
            spreadClear02();
        }

        private void clearScreen03()
        {
            chk3_1_1.Checked = false;
            dtp3_1_2.Text = "";
            dtp3_1_3.Text = "";
            txt3_1_4.Text = "";
            txt3_1_5.Text = "";
            chk3_2_1.Checked = false;
            txt3_2_2.Text = "";
            chk3_3_1.Checked = false;
            txt3_3_2.Text = "";
            txt3_3_3.Text = "";
            chk3_4_1.Checked = false;
            dtp3_4_2.Text = "";
            txt3_4_3.Text = "";
            txt3_5_1.Text = "";
            txt3_5_2.Text = "";
            txt3_5_3.Text = "";
            txt3_5_4.Text = "";
            spreadClear03();
        }

        private void clearScreen04()
        {
            chk4_1_1.Checked = false;
            chk4_2_1.Checked = false;
            chk4_2_2.Checked = false;
            chk4_2_3.Checked = false;
            chk4_2_4.Checked = false;
            chk4_2_5.Checked = false;
            chk4_2_6.Checked = false;
            chk4_3_1.Checked = false;
            chk4_3_2.Checked = false;
            txt4_3_3.Text = "";
            txt4_3_4.Text = "";
            txt4_3_5.Text = "";
            spreadClear04();
        }

        private bool checkData(string strGbn)
        {
            return true;
        }

        private void select01(string strRowid)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strRowid == "")
            {
                ComFunc.MsgBox("조회할 내역을 선택하여 주십시요.", "선택 내역없음");
                return;
            }

            try
            {
                SQL = " SELECT * ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_PRES_INSULIN01 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    setHeader(dt);

                    chk1_1.Checked = (dt.Rows[0]["chk1_1"].ToString().Trim() == "1" ? true : false);
                    chk1_2.Checked = (dt.Rows[0]["chk1_2"].ToString().Trim() == "1" ? true : false);
                    chk1_2_1.Checked = (dt.Rows[0]["chk1_2_1"].ToString().Trim() == "1" ? true : false);
                    chk1_2_2.Checked = (dt.Rows[0]["chk1_2_2"].ToString().Trim() == "1" ? true : false);
                    chk1_2_3.Checked = (dt.Rows[0]["chk1_2_3"].ToString().Trim() == "1" ? true : false);
                    chk1_2_4.Checked = (dt.Rows[0]["chk1_2_4"].ToString().Trim() == "1" ? true : false);
                    chk1_2_5.Checked = (dt.Rows[0]["chk1_2_5"].ToString().Trim() == "1" ? true : false);
                    chk1_3.Checked = (dt.Rows[0]["chk1_3"].ToString().Trim() == "1" ? true : false);
                    chk1_3_1.Checked = (dt.Rows[0]["chk1_3_1"].ToString().Trim() == "1" ? true : false);
                    chk1_3_2.Checked = (dt.Rows[0]["chk1_3_2"].ToString().Trim() == "1" ? true : false);
                    txt1_3_3.Text = dt.Rows[0]["txt1_3_3"].ToString().Trim();
                    chk1_4_1.Checked = (dt.Rows[0]["chk1_4_1"].ToString().Trim() == "1" ? true : false);
                    chk1_4_2.Checked = (dt.Rows[0]["chk1_4_2"].ToString().Trim() == "1" ? true : false);
                    chk1_4_3.Checked = (dt.Rows[0]["chk1_4_3"].ToString().Trim() == "1" ? true : false);
                    chk1_4_4.Checked = (dt.Rows[0]["chk1_4_4"].ToString().Trim() == "1" ? true : false);
                    chk1_4_5.Checked = (dt.Rows[0]["chk1_4_5"].ToString().Trim() == "1" ? true : false);
                    txt1_5.Text = dt.Rows[0]["txt1_5"].ToString().Trim();
                    txt1_6_1.Text = dt.Rows[0]["txt1_6_1"].ToString().Trim();
                    txt1_6_2.Text = dt.Rows[0]["txt1_6_2"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void delete01(string strRowid)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (strRowid == "")
            {
                ComFunc.MsgBox("삭제할 내역을 선택하여 주십시요.", "선택 내역없음");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " DELETE KOSMOS_OCS.OCS_PRES_INSULIN01 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "'";    
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }


        }

        private void insert01()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            #region 입력값 변수
            string strchk1_1 = (chk1_1.Checked == true ? "1" : "0");
            string strchk1_2 = (chk1_1.Checked == true ? "1" : "0");
            string strchk1_2_1 = (chk1_2_1.Checked == true ? "1" : "0");
            string strchk1_2_2 = (chk1_2_2.Checked == true ? "1" : "0");
            string strchk1_2_3 = (chk1_2_3.Checked == true ? "1" : "0");
            string strchk1_2_4 = (chk1_2_4.Checked == true ? "1" : "0");
            string strchk1_2_5 = (chk1_2_5.Checked == true ? "1" : "0");
            string strchk1_3 = (chk1_3.Checked == true ? "1" : "0");
            string strchk1_3_1 = (chk1_3_1.Checked == true ? "1" : "0");
            string strchk1_3_2 = (chk1_3_2.Checked == true ? "1" : "0");
            string strtxt1_3_3 = txt1_3_3.Text.Trim();
            string strchk1_4_1 = (chk1_4_1.Checked == true ? "1" : "0");
            string strchk1_4_2 = (chk1_4_2.Checked == true ? "1" : "0");
            string strchk1_4_3 = (chk1_4_3.Checked == true ? "1" : "0");
            string strchk1_4_4 = (chk1_4_4.Checked == true ? "1" : "0");
            string strchk1_4_5 = (chk1_4_5.Checked == true ? "1" : "0");
            string strtxt1_5 = txt1_5.Text.Trim();
            string strtxt1_6_1 = txt1_6_1.Text.Trim();
            string strtxt1_6_2 = txt1_6_2.Text.Trim();
            #endregion

            if (checkData("01") != true)
            {
                ComFunc.MsgBox("입력값 오류입니다. 확인하시기 바랍니다", "값 오류");
                return;
            }


            if (getHeader() != true)
            {
                ComFunc.MsgBox("환자 기본정보 입력 오류입니다. 확인하시기 바랍니다", "환자 기본정보 오류");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " INSERT INTO KOSMOS_OCS.OCS_PRES_INSULIN01 ( ";
                SQL += ComNum.VBLF + " PANO, SNAME, BALDATE, DEPTCODE, ";
                SQL += ComNum.VBLF + " DRCODE, DRNAME, DRBUNHO1, DRBUNHO2, ";
                SQL += ComNum.VBLF + " BINO, TEL, PHONE, ILLNAME, ";
                SQL += ComNum.VBLF + " ILLCODE, WDATE, WSABUN, ";
                SQL += ComNum.VBLF + " chk1_1, chk1_2, chk1_2_1, chk1_2_2, ";
                SQL += ComNum.VBLF + " chk1_2_3, chk1_2_4, chk1_2_5, chk1_3, ";
                SQL += ComNum.VBLF + " chk1_3_1, chk1_3_2, txt1_3_3, chk1_4_1, ";
                SQL += ComNum.VBLF + " chk1_4_2, chk1_4_3, chk1_4_4, chk1_4_5, ";
                SQL += ComNum.VBLF + " txt1_5, txt1_6_1, txt1_6_2 ";
                SQL += ComNum.VBLF + " ) VALUES( ";
                SQL += ComNum.VBLF + "'" +  strPANO + "','" + strSNAME + "',TO_DATE('" + strBALDATE + "','YYYY-MM-DD'),'" + strDEPTCODE + "',";
                SQL += ComNum.VBLF + "'" + strDRCODE + "','" + strDRNAME + "','" + strDRBUNHO1 + "','" + strDRBUNHO2 + "',";
                SQL += ComNum.VBLF + "'" + strBINO + "','" + strTEL + "','" + strPHONE + "','" + strILLNAME + "',";
                SQL += ComNum.VBLF + "'" + strILLCODE + "',SYSDATE, " + clsType.User.Sabun + ",";
                SQL += ComNum.VBLF + "'" + strchk1_1 + "','" + strchk1_2 + "','" + strchk1_2_1 + "','" + strchk1_2_2 + "',";
                SQL += ComNum.VBLF + "'" + strchk1_2_3 + "','" + strchk1_2_4 + "','" + strchk1_2_5 + "','" + strchk1_3 + "',";
                SQL += ComNum.VBLF + "'" + strchk1_3_1 + "','" + strchk1_3_2 + "','" + strtxt1_3_3 + "','" + strchk1_4_1 + "',";
                SQL += ComNum.VBLF + "'" + strchk1_4_2 + "','" + strchk1_4_3 + "','" + strchk1_4_4 + "','" + strchk1_4_5 + "',";
                SQL += ComNum.VBLF + "'" + strtxt1_5 + "','" + strtxt1_6_1 + "','" + strtxt1_6_2 + "')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                if (ComFunc.MsgBoxQ("저장되었습니다. 인쇄하시겠습니까?", "인쇄", MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    print01();
                }

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void print01()
        {

            spreadDataSet01();

            ssDiabetes2_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDiabetes2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDiabetes2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes2_Sheet1.PrintInfo.Margin.Top = 60;
            ssDiabetes2_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDiabetes2_Sheet1.PrintInfo.ShowColor = false;
            ssDiabetes2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDiabetes2_Sheet1.PrintInfo.ShowBorder = false;
            ssDiabetes2_Sheet1.PrintInfo.ShowGrid = true;
            ssDiabetes2_Sheet1.PrintInfo.ShowShadows = false;
            ssDiabetes2_Sheet1.PrintInfo.UseMax = true;
            ssDiabetes2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDiabetes2_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDiabetes2_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDiabetes2_Sheet1.PrintInfo.Preview = false;
            ssDiabetes2.PrintSheet(0);

        }

        private void select02(string strRowid)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strRowid == "")
            {
                ComFunc.MsgBox("조회할 내역을 선택하여 주십시요.", "선택 내역없음");
                return;
            }

            try
            {
                SQL = " SELECT * ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_PRES_INSULIN02 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    setHeader(dt);
                    
                    chk2_1_1.Checked = (dt.Rows[0]["chk2_1_1"].ToString().Trim() == "1" ? true : false);
                    chk2_1_2.Checked = (dt.Rows[0]["chk2_1_2"].ToString().Trim() == "1" ? true : false);
                    chk2_2_1.Checked = (dt.Rows[0]["chk2_2_1"].ToString().Trim() == "1" ? true : false);
                    chk2_2_2.Checked = (dt.Rows[0]["chk2_2_2"].ToString().Trim() == "1" ? true : false);
                    chk2_2_3.Checked = (dt.Rows[0]["chk2_2_3"].ToString().Trim() == "1" ? true : false);
                    chk2_2_4.Checked = (dt.Rows[0]["chk2_2_4"].ToString().Trim() == "1" ? true : false);
                    chk2_2_5.Checked = (dt.Rows[0]["chk2_2_5"].ToString().Trim() == "1" ? true : false);
                    chk2_2_6.Checked = (dt.Rows[0]["chk2_2_6"].ToString().Trim() == "1" ? true : false);
                    chk2_3_1.Checked = (dt.Rows[0]["chk2_3_1"].ToString().Trim() == "1" ? true : false);
                    chk2_3_2.Checked = (dt.Rows[0]["chk2_3_2"].ToString().Trim() == "1" ? true : false);
                    chk2_3_3.Checked = (dt.Rows[0]["chk2_3_3"].ToString().Trim() == "1" ? true : false);
                    chk2_3_4.Checked = (dt.Rows[0]["chk2_3_4"].ToString().Trim() == "1" ? true : false);
                    chk2_3_5.Checked = (dt.Rows[0]["chk2_3_5"].ToString().Trim() == "1" ? true : false);
                    chk2_3_6.Checked = (dt.Rows[0]["chk2_3_6"].ToString().Trim() == "1" ? true : false);
                    chk2_4_1.Checked = (dt.Rows[0]["chk2_4_1"].ToString().Trim() == "1" ? true : false);
                    chk2_4_2.Checked = (dt.Rows[0]["chk2_4_2"].ToString().Trim() == "1" ? true : false);
                    chk2_4_3.Checked = (dt.Rows[0]["chk2_4_3"].ToString().Trim() == "1" ? true : false);
                    chk2_4_4.Checked = (dt.Rows[0]["chk2_4_4"].ToString().Trim() == "1" ? true : false);
                    chk2_4_5.Checked = (dt.Rows[0]["chk2_4_5"].ToString().Trim() == "1" ? true : false);
                    chk2_4_6.Checked = (dt.Rows[0]["chk2_4_6"].ToString().Trim() == "1" ? true : false);
                    chk2_4_7.Checked = (dt.Rows[0]["chk2_4_7"].ToString().Trim() == "1" ? true : false);
                    chk2_4_8.Checked = (dt.Rows[0]["chk2_4_8"].ToString().Trim() == "1" ? true : false);
                    chk2_5_1.Checked = (dt.Rows[0]["chk2_5_1"].ToString().Trim() == "1" ? true : false);
                    chk2_5_2.Checked = (dt.Rows[0]["chk2_5_2"].ToString().Trim() == "1" ? true : false);
                    chk2_5_3.Checked = (dt.Rows[0]["chk2_5_3"].ToString().Trim() == "1" ? true : false);
                    txt2_5_4.Text = dt.Rows[0]["txt2_5_4"].ToString().Trim();
                    chk2_6_1.Checked = (dt.Rows[0]["chk2_6_1"].ToString().Trim() == "1" ? true : false);
                    chk2_6_2.Checked = (dt.Rows[0]["chk2_6_2"].ToString().Trim() == "1" ? true : false);
                    chk2_6_3.Checked = (dt.Rows[0]["chk2_6_3"].ToString().Trim() == "1" ? true : false);
                    chk2_6_4.Checked = (dt.Rows[0]["chk2_6_4"].ToString().Trim() == "1" ? true : false);
                    txt2_7_1.Text = dt.Rows[0]["txt2_7_1"].ToString().Trim();
                    txt2_8_1.Text = dt.Rows[0]["txt2_8_1"].ToString().Trim();
                    txt2_8_2.Text = dt.Rows[0]["txt2_8_2"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void delete02(string strRowid)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (strRowid == "")
            {
                ComFunc.MsgBox("삭제할 내역을 선택하여 주십시요.", "선택 내역없음");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " DELETE KOSMOS_OCS.OCS_PRES_INSULIN02 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }


        }

        private void insert02()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            #region 입력값 변수
            string strchk2_1_1 = (chk2_1_1.Checked == true ? "1" : "0");
            string strchk2_1_2 = (chk2_1_1.Checked == true ? "1" : "0");
            string strchk2_2_1 = (chk2_2_1.Checked == true ? "1" : "0");
            string strchk2_2_2 = (chk2_2_2.Checked == true ? "1" : "0");
            string strchk2_2_3 = (chk2_2_3.Checked == true ? "1" : "0");
            string strchk2_2_4 = (chk2_2_4.Checked == true ? "1" : "0");
            string strchk2_2_5 = (chk2_2_5.Checked == true ? "1" : "0");
            string strchk2_2_6 = (chk2_2_6.Checked == true ? "1" : "0");
            string strchk2_3_1 = (chk2_3_1.Checked == true ? "1" : "0");
            string strchk2_3_2 = (chk2_3_2.Checked == true ? "1" : "0");
            string strchk2_3_3 = (chk2_3_3.Checked == true ? "1" : "0");
            string strchk2_3_4 = (chk2_3_4.Checked == true ? "1" : "0");
            string strchk2_3_5 = (chk2_3_5.Checked == true ? "1" : "0");
            string strchk2_3_6 = (chk2_3_6.Checked == true ? "1" : "0");
            string strchk2_4_1 = (chk2_4_1.Checked == true ? "1" : "0");
            string strchk2_4_2 = (chk2_4_2.Checked == true ? "1" : "0");
            string strchk2_4_3 = (chk2_4_3.Checked == true ? "1" : "0");
            string strchk2_4_4 = (chk2_4_4.Checked == true ? "1" : "0");
            string strchk2_4_5 = (chk2_4_5.Checked == true ? "1" : "0");
            string strchk2_4_6 = (chk2_4_6.Checked == true ? "1" : "0");
            string strchk2_4_7 = (chk2_4_7.Checked == true ? "1" : "0");
            string strchk2_4_8 = (chk2_4_8.Checked == true ? "1" : "0");
            string strchk2_5_1 = (chk2_5_1.Checked == true ? "1" : "0");
            string strchk2_5_2 = (chk2_5_2.Checked == true ? "1" : "0");
            string strchk2_5_3 = (chk2_5_3.Checked == true ? "1" : "0");
            string strtxt2_5_4 = txt2_5_4.Text.Trim();
            string strchk2_6_1 = (chk2_6_1.Checked == true ? "1" : "0");
            string strchk2_6_2 = (chk2_6_2.Checked == true ? "1" : "0");
            string strchk2_6_3 = (chk2_6_3.Checked == true ? "1" : "0");
            string strchk2_6_4 = (chk2_6_4.Checked == true ? "1" : "0");
            string strtxt2_7_1 = txt2_7_1.Text.Trim();
            string strtxt2_8_1 = txt2_8_1.Text.Trim();
            string strtxt2_8_2 = txt2_8_2.Text.Trim();


            #endregion

            if (checkData("02") != true)
            {
                ComFunc.MsgBox("입력값 오류입니다. 확인하시기 바랍니다", "값 오류");
                return;
            }

            if (getHeader() != true)
            {
                ComFunc.MsgBox("환자 기본정보 입력 오류입니다. 확인하시기 바랍니다", "환자 기본정보 오류");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " INSERT INTO KOSMOS_OCS.OCS_PRES_INSULIN02 ( ";
                SQL += ComNum.VBLF + " PANO, SNAME, BALDATE, DEPTCODE, ";
                SQL += ComNum.VBLF + " DRCODE, DRNAME, DRBUNHO1, DRBUNHO2, ";
                SQL += ComNum.VBLF + " BINO, TEL, PHONE, ILLNAME, ";
                SQL += ComNum.VBLF + " ILLCODE, WDATE, WSABUN, ";
                SQL += ComNum.VBLF + " chk2_1_1,chk2_1_2,chk2_2_1,chk2_2_2, ";
                SQL += ComNum.VBLF + " chk2_2_3, chk2_2_4, chk2_2_5, chk2_2_6, ";
                SQL += ComNum.VBLF + " chk2_3_1, chk2_3_2, chk2_3_3, chk2_3_4, ";
                SQL += ComNum.VBLF + " chk2_3_5, chk2_3_6, chk2_4_1, chk2_4_2, ";
                SQL += ComNum.VBLF + " chk2_4_3, chk2_4_4, chk2_4_5, chk2_4_6, ";
                SQL += ComNum.VBLF + " chk2_4_7, chk2_4_8, chk2_5_1, chk2_5_2, ";
                SQL += ComNum.VBLF + " chk2_5_3, txt2_5_4, chk2_6_1, chk2_6_2, ";
                SQL += ComNum.VBLF + " chk2_6_3, chk2_6_4, txt2_7_1, txt2_8_1, ";
                SQL += ComNum.VBLF + " txt2_8_2 ";
                SQL += ComNum.VBLF + " ) VALUES( ";
                SQL += ComNum.VBLF + "'" + strPANO + "','" + strSNAME + "',TO_DATE('" + strBALDATE + "','YYYY-MM-DD'),'" + strDEPTCODE + "',";
                SQL += ComNum.VBLF + "'" + strDRCODE + "','" + strDRNAME + "','" + strDRBUNHO1 + "','" + strDRBUNHO2 + "',";
                SQL += ComNum.VBLF + "'" + strBINO + "','" + strTEL + "','" + strPHONE + "','" + strILLNAME + "',";
                SQL += ComNum.VBLF + "'" + strILLCODE + "',SYSDATE, " + clsType.User.Sabun + ",";
                SQL += ComNum.VBLF + "'" + strchk2_1_1 + "','" + strchk2_1_2 + "','" + strchk2_2_1 + "','" + strchk2_2_2 + "',";
                SQL += ComNum.VBLF + "'" + strchk2_2_3 + "','" + strchk2_2_4 + "','" + strchk2_2_5 + "','" + strchk2_2_6 + "',";
                SQL += ComNum.VBLF + "'" + strchk2_3_1 + "','" + strchk2_3_2 + "','" + strchk2_3_3 + "','" + strchk2_3_4 + "',";
                SQL += ComNum.VBLF + "'" + strchk2_3_5 + "','" + strchk2_3_6 + "','" + strchk2_4_1 + "','" + strchk2_4_2 + "',";
                SQL += ComNum.VBLF + "'" + strchk2_4_3 + "','" + strchk2_4_4 + "','" + strchk2_4_5 + "','" + strchk2_4_6 + "',";
                SQL += ComNum.VBLF + "'" + strchk2_4_7 + "','" + strchk2_4_8 + "','" + strchk2_5_1 + "','" + strchk2_5_2 + "',";
                SQL += ComNum.VBLF + "'" + strchk2_5_3 + "','" + strtxt2_5_4 + "','" + strchk2_6_1 + "','" + strchk2_6_2 + "',";
                SQL += ComNum.VBLF + "'" + strchk2_6_3 + "','" + strchk2_6_4 + "','" + strtxt2_7_1 + "','" + strtxt2_8_1 + "',";
                SQL += ComNum.VBLF + "'" + strtxt2_8_2 + "')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                if (ComFunc.MsgBoxQ("저장되었습니다. 인쇄하시겠습니까?", "인쇄", MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    print02();
                }

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void print02()
        {
            spreadDataSet02();

            ssDiabetes3_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDiabetes3_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDiabetes3_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes3_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes3_Sheet1.PrintInfo.Margin.Top = 60;
            ssDiabetes3_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDiabetes3_Sheet1.PrintInfo.ShowColor = false;
            ssDiabetes3_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDiabetes3_Sheet1.PrintInfo.ShowBorder = false;
            ssDiabetes3_Sheet1.PrintInfo.ShowGrid = true;
            ssDiabetes3_Sheet1.PrintInfo.ShowShadows = false;
            ssDiabetes3_Sheet1.PrintInfo.UseMax = true;
            ssDiabetes3_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDiabetes3_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDiabetes3_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDiabetes3_Sheet1.PrintInfo.Preview = false;
            ssDiabetes3.PrintSheet(0);

        }

        private void select03(string strRowid)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strRowid == "")
            {
                ComFunc.MsgBox("조회할 내역을 선택하여 주십시요.", "선택 내역없음");
                return;
            }

            try
            {
                SQL = " SELECT * ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_PRES_INSULIN03 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    setHeader(dt);

                    chk3_1_1.Checked = (dt.Rows[0]["chk3_1_1"].ToString().Trim() == "1" ? true : false);
                    dtp3_1_2.Text = dt.Rows[0]["dtp3_1_2"].ToString().Trim();
                    dtp3_1_3.Text = dt.Rows[0]["dtp3_1_3"].ToString().Trim();
                    txt3_1_4.Text = dt.Rows[0]["txt3_1_4"].ToString().Trim();
                    txt3_1_5.Text = dt.Rows[0]["txt3_1_5"].ToString().Trim();
                    chk3_2_1.Checked = (dt.Rows[0]["chk3_2_1"].ToString().Trim() == "1" ? true : false);
                    txt3_2_2.Text = dt.Rows[0]["txt3_2_2"].ToString().Trim();
                    chk3_3_1.Checked = (dt.Rows[0]["chk3_3_1"].ToString().Trim() == "1" ? true : false);
                    txt3_3_2.Text = dt.Rows[0]["txt3_3_2"].ToString().Trim();
                    txt3_3_3.Text = dt.Rows[0]["txt3_3_3"].ToString().Trim();
                    chk3_4_1.Checked = (dt.Rows[0]["chk3_4_1"].ToString().Trim() == "1" ? true : false);
                    dtp3_4_2.Text = dt.Rows[0]["dtp3_4_2"].ToString().Trim();
                    txt3_4_3.Text = dt.Rows[0]["txt3_4_3"].ToString().Trim();
                    txt3_5_1.Text = dt.Rows[0]["txt3_5_1"].ToString().Trim();
                    txt3_5_2.Text = dt.Rows[0]["txt3_5_2"].ToString().Trim();
                    txt3_5_3.Text = dt.Rows[0]["txt3_5_3"].ToString().Trim();
                    txt3_5_4.Text = dt.Rows[0]["txt3_5_4"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void delete03(string strRowid)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (strRowid == "")
            {
                ComFunc.MsgBox("삭제할 내역을 선택하여 주십시요.", "선택 내역없음");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " DELETE KOSMOS_OCS.OCS_PRES_INSULIN03 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }


        }

        private void insert03()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            #region 입력값 변수
            string strchk3_1_1 = (chk3_1_1.Checked == true ? "1" : "0");
            string strdtp3_1_2 = dtp3_1_2.Text.Trim();
            string strdtp3_1_3 = dtp3_1_3.Text.Trim();
            string strtxt3_1_4 = txt3_1_4.Text.Trim();
            string strtxt3_1_5 = txt3_1_5.Text.Trim();
            string strchk3_2_1 = (chk3_2_1.Checked == true ? "1" : "0");
            string strtxt3_2_2 = txt3_2_2.Text.Trim();
            string strchk3_3_1 = (chk3_3_1.Checked == true ? "1" : "0");
            string strtxt3_3_2 = txt3_3_2.Text.Trim();
            string strtxt3_3_3 = txt3_3_3.Text.Trim();
            string strchk3_4_1 = (chk3_4_1.Checked == true ? "1" : "0");
            string strdtp3_4_2 = dtp3_4_2.Text.Trim();
            string strtxt3_4_3 = txt3_4_3.Text.Trim();
            string strtxt3_5_1 = txt3_5_1.Text.Trim();
            string strtxt3_5_2 = txt3_5_2.Text.Trim();
            string strtxt3_5_3 = txt3_5_3.Text.Trim();
            string strtxt3_5_4 = txt3_5_4.Text.Trim();
            #endregion

            if (checkData("03") != true)
            {
                ComFunc.MsgBox("입력값 오류입니다. 확인하시기 바랍니다", "값 오류");
                return;
            }

            if (getHeader() != true)
            {
                ComFunc.MsgBox("환자 기본정보 입력 오류입니다. 확인하시기 바랍니다", "환자 기본정보 오류");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " INSERT INTO KOSMOS_OCS.OCS_PRES_INSULIN03 ( ";
                SQL += ComNum.VBLF + " PANO, SNAME, BALDATE, DEPTCODE, ";
                SQL += ComNum.VBLF + " DRCODE, DRNAME, DRBUNHO1, DRBUNHO2, ";
                SQL += ComNum.VBLF + " BINO, TEL, PHONE, ILLNAME, ";
                SQL += ComNum.VBLF + " ILLCODE, WDATE, WSABUN, ";
                SQL += ComNum.VBLF + " chk3_1_1, dtp3_1_2, dtp3_1_3, txt3_1_4, ";
                SQL += ComNum.VBLF + " txt3_1_5, chk3_2_1, txt3_2_2, chk3_3_1, ";
                SQL += ComNum.VBLF + " txt3_3_2, txt3_3_3, chk3_4_1, dtp3_4_2, ";
                SQL += ComNum.VBLF + " txt3_4_3, txt3_5_1, txt3_5_2, txt3_5_3, ";
                SQL += ComNum.VBLF + " txt3_5_4 ";
                SQL += ComNum.VBLF + " ) VALUES( ";
                SQL += ComNum.VBLF + "'" + strPANO + "','" + strSNAME + "',TO_DATE('" + strBALDATE + "','YYYY-MM-DD'),'" + strDEPTCODE + "',";
                SQL += ComNum.VBLF + "'" + strDRCODE + "','" + strDRNAME + "','" + strDRBUNHO1 + "','" + strDRBUNHO2 + "',";
                SQL += ComNum.VBLF + "'" + strBINO + "','" + strTEL + "','" + strPHONE + "','" + strILLNAME + "',";
                SQL += ComNum.VBLF + "'" + strILLCODE + "',SYSDATE, " + clsType.User.Sabun + ",";
                SQL += ComNum.VBLF + "'" + strchk3_1_1 + "','" + strdtp3_1_2 + "','" + strdtp3_1_3 + "','" + strtxt3_1_4 + "',";
                SQL += ComNum.VBLF + "'" + strtxt3_1_5 + "','" + strchk3_2_1 + "','" + strtxt3_2_2 + "','" + strchk3_3_1 + "',";
                SQL += ComNum.VBLF + "'" + strtxt3_3_2 + "','" + strtxt3_3_3 + "','" + strchk3_4_1 + "','" + strdtp3_4_2 + "',";
                SQL += ComNum.VBLF + "'" + strtxt3_4_3 + "','" + strtxt3_5_1 + "','" + strtxt3_5_2 + "','" + strtxt3_5_3 + "',";
                SQL += ComNum.VBLF + "'" + strtxt3_5_4 + "')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                if (ComFunc.MsgBoxQ("저장되었습니다. 인쇄하시겠습니까?", "인쇄", MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    print03();
                }

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void print03()
        {
            spreadDataSet03();

            ssDiabetes4_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDiabetes4_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDiabetes4_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes4_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes4_Sheet1.PrintInfo.Margin.Top = 60;
            ssDiabetes4_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDiabetes4_Sheet1.PrintInfo.ShowColor = false;
            ssDiabetes4_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDiabetes4_Sheet1.PrintInfo.ShowBorder = false;
            ssDiabetes4_Sheet1.PrintInfo.ShowGrid = true;
            ssDiabetes4_Sheet1.PrintInfo.ShowShadows = false;
            ssDiabetes4_Sheet1.PrintInfo.UseMax = true;
            ssDiabetes4_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDiabetes4_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDiabetes4_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDiabetes4_Sheet1.PrintInfo.Preview = false;
            ssDiabetes4.PrintSheet(0);
        }

        private void select04(string strRowid)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strRowid == "")
            {
                ComFunc.MsgBox("조회할 내역을 선택하여 주십시요.", "선택 내역없음");
                return;
            }

            try
            {
                SQL = " SELECT * ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_PRES_INSULIN04 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    setHeader(dt);

                    chk4_1_1.Checked = (dt.Rows[0]["chk4_1_1"].ToString().Trim() == "1" ? true : false);
                    chk4_2_1.Checked = (dt.Rows[0]["chk4_2_1"].ToString().Trim() == "1" ? true : false);
                    chk4_2_2.Checked = (dt.Rows[0]["chk4_2_2"].ToString().Trim() == "1" ? true : false);
                    chk4_2_3.Checked = (dt.Rows[0]["chk4_2_3"].ToString().Trim() == "1" ? true : false);
                    chk4_2_4.Checked = (dt.Rows[0]["chk4_2_4"].ToString().Trim() == "1" ? true : false);
                    chk4_2_5.Checked = (dt.Rows[0]["chk4_2_5"].ToString().Trim() == "1" ? true : false);
                    chk4_2_6.Checked = (dt.Rows[0]["chk4_2_6"].ToString().Trim() == "1" ? true : false);
                    chk4_3_1.Checked = (dt.Rows[0]["chk4_3_1"].ToString().Trim() == "1" ? true : false);
                    chk4_3_2.Checked = (dt.Rows[0]["chk4_3_2"].ToString().Trim() == "1" ? true : false);
                    txt4_3_3.Text = dt.Rows[0]["txt4_3_3"].ToString().Trim();
                    txt4_3_4.Text = dt.Rows[0]["txt4_3_4"].ToString().Trim();
                    txt4_3_5.Text = dt.Rows[0]["txt4_3_5"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void delete04(string strRowid)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (strRowid == "")
            {
                ComFunc.MsgBox("삭제할 내역을 선택하여 주십시요.", "선택 내역없음");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " DELETE KOSMOS_OCS.OCS_PRES_INSULIN04 ";
                SQL += ComNum.VBLF + " WHERE ROWID = '" + strRowid + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }


        }

        private void insert04()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            #region 입력값 변수

            string strchk4_1_1 = (chk4_1_1.Checked == true ? "1" : "0");
            string strchk4_2_1 = (chk4_2_1.Checked == true ? "1" : "0");
            string strchk4_2_2 = (chk4_2_2.Checked == true ? "1" : "0");
            string strchk4_2_3 = (chk4_2_3.Checked == true ? "1" : "0");
            string strchk4_2_4 = (chk4_2_4.Checked == true ? "1" : "0");
            string strchk4_2_5 = (chk4_2_5.Checked == true ? "1" : "0");
            string strchk4_2_6 = (chk4_2_6.Checked == true ? "1" : "0");
            string strchk4_3_1 = (chk4_3_1.Checked == true ? "1" : "0");
            string strchk4_3_2 = (chk4_3_2.Checked == true ? "1" : "0");
            string strtxt4_3_3 = txt4_3_3.Text.Trim();
            string strtxt4_3_4 = txt4_3_3.Text.Trim();
            string strtxt4_3_5 = txt4_3_3.Text.Trim();
            #endregion

            if (checkData("04") != true)
            {
                ComFunc.MsgBox("입력값 오류입니다. 확인하시기 바랍니다", "값 오류");
                return;
            }

            if (getHeader() != true)
            {
                ComFunc.MsgBox("환자 기본정보 입력 오류입니다. 확인하시기 바랍니다", "환자 기본정보 오류");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                SQL = " INSERT INTO KOSMOS_OCS.OCS_PRES_INSULIN04 ( ";
                SQL += ComNum.VBLF + " PANO, SNAME, BALDATE, DEPTCODE, ";
                SQL += ComNum.VBLF + " DRCODE, DRNAME, DRBUNHO1, DRBUNHO2, ";
                SQL += ComNum.VBLF + " BINO, TEL, PHONE, ILLNAME, ";
                SQL += ComNum.VBLF + " ILLCODE, WDATE, WSABUN, ";
                SQL += ComNum.VBLF + " chk4_1_1, chk4_2_1, chk4_2_2, chk4_2_3, ";
                SQL += ComNum.VBLF + " chk4_2_4, chk4_2_5, chk4_2_6, chk4_3_1, ";
                SQL += ComNum.VBLF + " chk4_3_2, txt4_3_3, txt4_3_4, txt4_3_5 ";
                SQL += ComNum.VBLF + " ) VALUES( ";
                SQL += ComNum.VBLF + "'" + strPANO + "','" + strSNAME + "',TO_DATE('" + strBALDATE + "','YYYY-MM-DD'),'" + strDEPTCODE + "',";
                SQL += ComNum.VBLF + "'" + strDRCODE + "','" + strDRNAME + "','" + strDRBUNHO1 + "','" + strDRBUNHO2 + "',";
                SQL += ComNum.VBLF + "'" + strBINO + "','" + strTEL + "','" + strPHONE + "','" + strILLNAME + "',";
                SQL += ComNum.VBLF + "'" + strILLCODE + "',SYSDATE, " + clsType.User.Sabun + ",";
                SQL += ComNum.VBLF + "'" + strchk4_1_1 + "','" + strchk4_2_1 + "','" + strchk4_2_2 + "','" + strchk4_2_3 + "',";
                SQL += ComNum.VBLF + "'" + strchk4_2_4 + "','" + strchk4_2_5 + "','" + strchk4_2_6 + "','" + strchk4_3_1 + "',";
                SQL += ComNum.VBLF + "'" + strchk4_3_2 + "','" + strtxt4_3_3 + "','" + strtxt4_3_4 + "','" + strtxt4_3_5 + "')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                if (ComFunc.MsgBoxQ("저장되었습니다. 인쇄하시겠습니까?", "인쇄", MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    print04();
                }

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        private void print04()
        {
            spreadDataSet04();

            ssDiabetes5_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDiabetes5_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDiabetes5_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes5_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes5_Sheet1.PrintInfo.Margin.Top = 60;
            ssDiabetes5_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDiabetes5_Sheet1.PrintInfo.ShowColor = false;
            ssDiabetes5_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDiabetes5_Sheet1.PrintInfo.ShowBorder = false;
            ssDiabetes5_Sheet1.PrintInfo.ShowGrid = true;
            ssDiabetes5_Sheet1.PrintInfo.ShowShadows = false;
            ssDiabetes5_Sheet1.PrintInfo.UseMax = true;
            ssDiabetes5_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDiabetes5_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDiabetes5_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDiabetes5_Sheet1.PrintInfo.Preview = false;
            ssDiabetes5.PrintSheet(0);
        }

        private string readPatientJuso(string strPANO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strJUSO = "";

            try
            {
                SQL = " SELECT JUSO_1 || ' ' || JUSO_2 || ' ' || JUSO_3 JUSO ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.VIEW_PATIENT_JUSO ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + strPANO + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return "";
                }
                if (dt.Rows.Count > 0)
                {
                    strJUSO = dt.Rows[0]["JUSO"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;

                return strJUSO;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return "";
            }

            
        }

        private string readDeptNameK(string strDEPT)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strDEPTNAMEK = "";

            try
            {
                SQL = " SELECT DEPTNAMEK FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL += ComNum.VBLF + " WHERE DEPTCODE = '" + strDEPT + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return "";
                }
                if (dt.Rows.Count > 0)
                {
                    strDEPTNAMEK = dt.Rows[0]["DEPTNAMEK"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;

                return strDEPTNAMEK;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return "";
            }


            return "";
        }

        private string GetInsaCertNo(PsmhDb pDbCon, string strDrCode, string strDeptCode)
        {
            string rtnVal = "";
            string strDEPTK = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            switch (strDeptCode)
            {
                case "MD":
                case "MG":
                case "MC":
                case "MP":
                case "ME":
                case "MO":
                case "MN":
                case "MI":
                case "MR":
                    strDEPTK = "내과";
                    break;
                default:

                    SQL = " SELECT REPLACE(DEPTNAMEK, ' ', '') DEPTK ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                    SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = '" + strDeptCode + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        dt.Dispose();
                        dt = null;
                        return "";
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strDEPTK = dt.Rows[0]["DEPTK"].ToString();
                    }

                    dt.Dispose();
                    dt = null;

                    break;
            }

            if (strDEPTK == "")
            { return ""; }
            #region 이전 전문의 번호 가져오기
            //SQL = " SELECT BUNHO FROM KOSMOS_ADM.INSA_MSTL ";
            //SQL = SQL + ComNum.VBLF + " WHERE NAME LIKE '%전문의%' ";
            //SQL = SQL + ComNum.VBLF + "  AND NAME LIKE '%" + strDEPTK + "%'  ";
            //SQL = SQL + ComNum.VBLF + "  AND GIKWAN LIKE '%보건%' ";
            //SQL = SQL + ComNum.VBLF + "  AND GUBUN = '1' ";
            //SQL = SQL + ComNum.VBLF + "  AND SABUN IN ";
            //SQL = SQL + ComNum.VBLF + "                  ( SELECT SABUN FROM KOSMOS_OCS.OCS_DOCTOR ";
            //SQL = SQL + ComNum.VBLF + "                     WHERE DRCODE = '" + strDrCode + "' ";
            //SQL = SQL + ComNum.VBLF + "                       AND GBOUT = 'N') ";
            //SQL = SQL + ComNum.VBLF + " ORDER BY CHIDATE ASC "; 
            #endregion
            //2020-02-17 내과는 세부전문의번호 먼저 가져오고 없으면 그냥 전문의 가져오기
            SQL = "";
            if (strDEPTK == "내과")
            {
                SQL += ComNum.VBLF + " SELECT NAME, BUNHO, '1' GUBUN, CHIDATE FROM KOSMOS_ADM.INSA_MSTL ";
                SQL += ComNum.VBLF + " WHERE NAME LIKE '%전문의%' ";
                SQL += ComNum.VBLF + "   AND NAME LIKE '%분과%' ";
                SQL += ComNum.VBLF + "   AND GUBUN = '1' ";
                SQL += ComNum.VBLF + "   AND SABUN IN ";
                SQL += ComNum.VBLF + "                   (SELECT SABUN FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL += ComNum.VBLF + "                     WHERE DRCODE = '" + strDrCode + "' ";
                SQL += ComNum.VBLF + "                       AND GBOUT = 'N') ";
                SQL += ComNum.VBLF + " UNION ALL ";
            }
            SQL += ComNum.VBLF + "  SELECT NAME, BUNHO, '2' GUBUN, CHIDATE FROM KOSMOS_ADM.INSA_MSTL ";
            SQL += ComNum.VBLF + "  WHERE NAME LIKE '%전문의%' ";
            SQL = SQL + ComNum.VBLF + "  AND NAME LIKE '%" + strDEPTK + "%'  ";
            SQL += ComNum.VBLF + "    AND GIKWAN LIKE '%보건%' ";
            SQL += ComNum.VBLF + "    AND GUBUN = '1' ";
            SQL += ComNum.VBLF + "    AND SABUN IN ";
            SQL += ComNum.VBLF + "                   (SELECT SABUN FROM KOSMOS_OCS.OCS_DOCTOR ";
            SQL += ComNum.VBLF + "                     WHERE DRCODE = '" + strDrCode + "' ";
            SQL += ComNum.VBLF + "                       AND GBOUT = 'N') ";
            SQL += ComNum.VBLF + "  ORDER BY GUBUN ASC, CHIDATE ASC ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                dt.Dispose();
                dt = null;
                return "";
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["BUNHO"].ToString();
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        private string readDrSabun(string strDrCode)
        {
            string rtnVal = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL += ComNum.VBLF + "  SELECT SABUN FROM KOSMOS_OCS.OCS_DOCTOR  ";
            SQL += ComNum.VBLF + "  WHERE DRCODE = '" + strDrCode + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                dt.Dispose();
                dt = null;
                return "";
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["SABUN"].ToString();
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        private string convYYMMDD(string strDate)
        {
            string strYY = "";
            string strMM = "";
            string strDD = "";

            strYY = VB.Left(strDate, 4);
            strMM = VB.Mid(strDate, 6, 2);
            if (VB.Left(strMM, 1) == "0") strMM = VB.Right(strMM, 1);
            if (VB.Left(strDD, 1) == "0") strMM = VB.Right(strDD, 1);

            return strYY + "년 " + strMM + "월 " + strDD + "일";
        }

        Image SIGNATUREFILE_DBToFile(string strSabun)
        {
            Image rtnVAL = null;

            if (string.IsNullOrEmpty(strSabun)) return rtnVAL;

            string SQL = "";
            IDataReader reader = null;
            OracleCommand cmd = null;

            try
            {
                SQL = "";
                SQL = SQL + "\r\n" + "SELECT SABUN, SIGNATURE ";
                SQL = SQL + "\r\n" + "FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                SQL = SQL + "\r\n" + "WHERE TRIM(DRCODE) = '" + strSabun + "'";

                cmd = clsDB.DbCon.Con.CreateCommand();
                cmd.InitialLONGFetchSize = -1;
                cmd.CommandText = SQL;
                cmd.CommandTimeout = 30;
                reader = cmd.ExecuteReader();

                cmd.Dispose();
                cmd = null;

                if (reader == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                }

                while (reader.Read())
                {
                    byte[] byteArray = (byte[])reader.GetValue(1);
                    MemoryStream memStream = new MemoryStream(byteArray);
                    rtnVAL = Image.FromStream(memStream);
                }
                return rtnVAL;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVAL;
            }
        }

        private void spreadClear01()
        {
            ssDiabetes2_Sheet1.Cells[4, 1].Text = "[  ] 재발급";       //재발급
            ssDiabetes2_Sheet1.Cells[5, 3].Text = "";               //건강보험증번호
            ssDiabetes2_Sheet1.Cells[5, 8].Text = "";               //주민번호
            ssDiabetes2_Sheet1.Cells[6, 3].Text = "";               //성명
            ssDiabetes2_Sheet1.Cells[6, 9].Text = "";               //자택번호
            ssDiabetes2_Sheet1.Cells[7, 9].Text = "";               //휴대번호

            ssDiabetes2_Sheet1.Cells[9, 2].Text = "";               //진료과목
            ssDiabetes2_Sheet1.Cells[9, 5].Text = "";               //상병명
            ssDiabetes2_Sheet1.Cells[9, 9].Text = "";               //상병코드

            ssDiabetes2_Sheet1.Cells[13, 1].Text = "[  ] 제 1형 당뇨병";
            ssDiabetes2_Sheet1.Cells[13, 3].Text = "";

            ssDiabetes2_Sheet1.Cells[14, 1].Text = "[  ] 제 2형 당뇨병";
            ssDiabetes2_Sheet1.Cells[14, 3].Text = "[  ] 만 19세 미만 :     [  ] 인슐린 투여     [  ] 인슐린 미투여";
            ssDiabetes2_Sheet1.Cells[15, 3].Text = "[  ] 만 19세 이상 :     [  ] 인슐린 투여";

            ssDiabetes2_Sheet1.Cells[16, 1].Text = "[  ] 임신 중 당뇨병";
            ssDiabetes2_Sheet1.Cells[16, 3].Text = "[  ] 인슐린 투여";
            ssDiabetes2_Sheet1.Cells[17, 3].Text = "[  ] 인슐린 미투여";
            ssDiabetes2_Sheet1.Cells[18, 3].Text = "※ 참고 : 분만 예정일(                             )";

            ssDiabetes2_Sheet1.Cells[20, 3].Text = "[    ]혈당측정검사지          [    ]채혈침      [    ]인슐린주사기      [    ]인슐린주사바늘";
            ssDiabetes2_Sheet1.Cells[20, 3].Text += ComNum.VBLF;
            ssDiabetes2_Sheet1.Cells[20, 3].Text += ComNum.VBLF + "[    ]인슐린펌프용 주사기   [    ]인슐린펌프용 주사바늘";

            ssDiabetes2_Sheet1.Cells[21, 3].Text = "";
            ssDiabetes2_Sheet1.Cells[22, 3].Text = " 평균  [         ] 회 검사/일";
            ssDiabetes2_Sheet1.Cells[23, 3].Text = " 평균  [         ] 회 검사/일";

            ssDiabetes2_Sheet1.Cells[27, 4].Text = "";               //발급일자
            ssDiabetes2_Sheet1.Cells[29, 4].Text = "";               //담당의사(면허번호)
            ssDiabetes2_Sheet1.Cells[30, 4].Text = "";               //전문과목(전문의번호)
        }

        private void spreadDataSet01()
        {
            
            ssDiabetes2_Sheet1.Cells[4, 1].Text = "[" + (chk1_1.Checked == true ? " V " : "  ") + "] 재발급";
            ssDiabetes2_Sheet1.Cells[5, 3].Text = txtBINO.Text.Trim();       //보장기관명
            ssDiabetes2_Sheet1.Cells[5, 8].Text = txtJUMIN.Text.Trim();      //주민등록번호
            ssDiabetes2_Sheet1.Cells[6, 3].Text = txtNAME.Text.Trim();       //성명
            ssDiabetes2_Sheet1.Cells[6, 9].Text = txtTEL.Text.Trim();       //자택
            ssDiabetes2_Sheet1.Cells[7, 9].Text = txtPHONE.Text.Trim();       //전화번호

            ssDiabetes2_Sheet1.Cells[9, 2].Text = VB.Split(cboDEPTCODE.Text, ".")[1];      //진료과목
            ssDiabetes2_Sheet1.Cells[9, 5].Text = txtILLNAME.Text.Trim();      //상병명
            ssDiabetes2_Sheet1.Cells[9, 9].Text = txtILLCODE.Text.Trim();     //상병코드


            ssDiabetes2_Sheet1.Cells[13, 1].Text = "[" + (chk1_1.Checked == true ? "V " : "  ") + "] 제 1형 당뇨병";
            ssDiabetes2_Sheet1.Cells[13, 3].Text = "";

            ssDiabetes2_Sheet1.Cells[14, 1].Text = "[" + (chk1_2.Checked == true ? "V " : "  ") + "] 제 2형 당뇨병";
            ssDiabetes2_Sheet1.Cells[14, 3].Text = "[" + (chk1_2_1.Checked == true ? "V " : "  ") + "] 만 19세 미만 :     [" + (chk1_1.Checked == true ? "V " : "  ") + "] 인슐린 투여     [" + (chk1_1.Checked == true ? "V " : "  ") + "] 인슐린 미투여";
            ssDiabetes2_Sheet1.Cells[15, 3].Text = "[" + (chk1_2_4.Checked == true ? "V " : "  ") + "] 만 19세 이상 :     [" + (chk1_1.Checked == true ? "V " : "  ") + "] 인슐린 투여";

            ssDiabetes2_Sheet1.Cells[16, 1].Text = "[" + (chk1_3.Checked == true ? "V " : "  ") + "] 임신 중 당뇨병";
            ssDiabetes2_Sheet1.Cells[16, 3].Text = "[" + (chk1_3_1.Checked == true ? "V " : "  ") + "] 인슐린 투여";
            ssDiabetes2_Sheet1.Cells[17, 3].Text = "[" + (chk1_3_2.Checked == true ? "V " : "  ") + "] 인슐린 미투여";
            ssDiabetes2_Sheet1.Cells[18, 3].Text = "※ 참고 : 분만 예정일( " + txt1_3_3.Text.Trim() + " )";

            ssDiabetes2_Sheet1.Cells[20, 3].Text = "[" + (chk1_4_1.Checked == true ? " V  " : "    ") + "]혈당측정검사지          ["
                                                       + (chk1_4_2.Checked == true ? " V  " : "    ") + "]채혈침      ["
                                                       + (chk1_4_3.Checked == true ? " V  " : "    ") + "]인슐린주사기";
                                                       
            ssDiabetes2_Sheet1.Cells[20, 3].Text += ComNum.VBLF;
            ssDiabetes2_Sheet1.Cells[20, 3].Text += ComNum.VBLF + "[" + (chk1_4_4.Checked == true ? " V  " : "    ") + "]인슐린펌프용 주사기   [" 
                                                                      + (chk1_4_5.Checked == true ? " V  " : "    ") + "]인슐린펌프용 주사바늘";

            ssDiabetes2_Sheet1.Cells[21, 3].Text = "";
            ssDiabetes2_Sheet1.Cells[22, 3].Text = " 평균  [  " + txt1_6_1.Text.Trim() + "  ] 회 검사/일";
            ssDiabetes2_Sheet1.Cells[23, 3].Text = " 평균  [  " + txt1_6_2.Text.Trim() + "  ] 회 검사/일";

            ssDiabetes2_Sheet1.Cells[27, 4].Text = convYYMMDD(dtpBALDATE.Text.Trim());      //발급일자
            ssDiabetes2_Sheet1.Cells[29, 4].Text = VB.Split(cboDRCODE.Text, ".")[1] + "(" + txtDRBUNHO1.Text.Trim() + ")";      //의사성명(면허번호)
            ssDiabetes2_Sheet1.Cells[30, 4].Text = VB.Split(cboDEPTCODE.Text, ".")[1] + "(" + txtDRBUNHO2.Text.Trim() + ")";      //전문과목(전문의자격번호)

            string strFile = "";

            strDrSabun = readDrSabun(VB.Split(cboDRCODE.Text, ".")[0]);

            if (strDrSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssDiabetes2_Sheet1.Cells[29, 9].CellType = imgCellType;
                ssDiabetes2_Sheet1.Cells[29, 9].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssDiabetes2_Sheet1.Cells[29, 9].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(strDrSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(strDrSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssDiabetes2_Sheet1.Cells[29, 9].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssDiabetes2_Sheet1.Cells[29, 9].CellType = textCellType;
                ssDiabetes2_Sheet1.Cells[29, 9].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssDiabetes2_Sheet1.Cells[29, 9].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssDiabetes2_Sheet1.Cells[29, 9].Text = "(서명 또는 인)";
            }



        }

        private void spreadClear02()
        {
            ssDiabetes3_Sheet1.Cells[3, 0].Value = false;

            ssDiabetes3_Sheet1.Cells[6, 3].Text = "";
            ssDiabetes3_Sheet1.Cells[8, 3].Text = "";
            ssDiabetes3_Sheet1.Cells[9, 3].Text = "";
            ssDiabetes3_Sheet1.Cells[10, 3].Text = "";
            
            ssDiabetes3_Sheet1.Cells[8, 13].Text = "";
            ssDiabetes3_Sheet1.Cells[9, 13].Text = "";

            ssDiabetes3_Sheet1.Cells[11, 1].Text = "";
            ssDiabetes3_Sheet1.Cells[11, 8].Text = "";
            ssDiabetes3_Sheet1.Cells[11, 15].Text = "";

            ssDiabetes3_Sheet1.Cells[17, 2].Value = false;
            ssDiabetes3_Sheet1.Cells[17, 9].Value = false;

            ssDiabetes3_Sheet1.Cells[20, 3].Value = false;
            ssDiabetes3_Sheet1.Cells[21, 3].Value = false;
            ssDiabetes3_Sheet1.Cells[22, 3].Value = false;
            ssDiabetes3_Sheet1.Cells[23, 3].Value = false;
            ssDiabetes3_Sheet1.Cells[24, 3].Value = false;

            ssDiabetes3_Sheet1.Cells[27, 3].Value = false;

            ssDiabetes3_Sheet1.Cells[29, 3].Value = false;
            ssDiabetes3_Sheet1.Cells[30, 3].Value = false;
            ssDiabetes3_Sheet1.Cells[32, 3].Value = false;
            ssDiabetes3_Sheet1.Cells[33, 3].Value = false;

            ssDiabetes3_Sheet1.Cells[34, 3].Value = false;
            ssDiabetes3_Sheet1.Cells[35, 3].Value = false;

            ssDiabetes3_Sheet1.Cells[39, 0].Value = false;
            ssDiabetes3_Sheet1.Cells[40, 0].Value = false;

            ssDiabetes3_Sheet1.Cells[40, 4].Value = false;
            ssDiabetes3_Sheet1.Cells[41, 4].Value = false;

            ssDiabetes3_Sheet1.Cells[40, 9].Value = false;
            ssDiabetes3_Sheet1.Cells[41, 9].Value = false;

            ssDiabetes3_Sheet1.Cells[40, 13].Value = false;
            ssDiabetes3_Sheet1.Cells[41, 13].Value = false;

            ssDiabetes3_Sheet1.Cells[43, 0].Value = false;

            ssDiabetes3_Sheet1.Cells[42, 5].Value = false;
            ssDiabetes3_Sheet1.Cells[43, 5].Value = false;

            ssDiabetes3_Sheet1.Cells[44, 9].Text = "";

            ssDiabetes3_Sheet1.Cells[48, 4].Value = false;
            ssDiabetes3_Sheet1.Cells[48, 9].Value = false;
            ssDiabetes3_Sheet1.Cells[48, 12].Value = false;
            ssDiabetes3_Sheet1.Cells[48, 16].Value = false;

            ssDiabetes3_Sheet1.Cells[49, 4].Text = "";
            ssDiabetes3_Sheet1.Cells[49, 9].Text = "";

            ssDiabetes3_Sheet1.Cells[50, 6].Text = "";
            ssDiabetes3_Sheet1.Cells[50, 6].Text = "";

            ssDiabetes3_Sheet1.Cells[54, 6].Text = "";
            ssDiabetes3_Sheet1.Cells[51, 6].Text = "";

            ssDiabetes3_Sheet1.Cells[54, 6].Text = "";

            ssDiabetes3_Sheet1.Cells[55, 0].Text = "";

            ssDiabetes3_Sheet1.Cells[57, 6].Text = "";
            ssDiabetes3_Sheet1.Cells[57, 10].Text = "";

            ssDiabetes3_Sheet1.Cells[58, 6].Text = "";
            ssDiabetes3_Sheet1.Cells[58, 10].Text = "";

        }


        private void spreadDataSet02()
        {
            ssDiabetes3_Sheet1.Cells[3, 0].Value = chkRePrt.Checked;

             ssDiabetes3_Sheet1.Cells[6, 3].Text = txtBINO.Text.Trim();       //보장기관명
            ssDiabetes3_Sheet1.Cells[8, 3].Text = txtNAME.Text.Trim();       //성명
            ssDiabetes3_Sheet1.Cells[9, 3].Text = txtTEL.Text.Trim();       //자택
            ssDiabetes3_Sheet1.Cells[10, 3].Text = readPatientJuso(strPANO);        //환자주소

            ssDiabetes3_Sheet1.Cells[8, 13].Text = txtJUMIN.Text.Trim();      //주민등록번호
            ssDiabetes3_Sheet1.Cells[9, 13].Text = txtPHONE.Text.Trim();       //전화번호

            ssDiabetes3_Sheet1.Cells[11, 1].Text = VB.Split(cboDEPTCODE.Text, ".")[1];      //진료과목
            ssDiabetes3_Sheet1.Cells[11, 8].Text = txtILLNAME.Text.Trim();      //상병명
            ssDiabetes3_Sheet1.Cells[11, 15].Text = txtILLCODE.Text.Trim();     //상병코드

            ssDiabetes3_Sheet1.Cells[17, 2].Value = chk2_1_1.Checked;
            ssDiabetes3_Sheet1.Cells[17, 9].Value = chk2_1_2.Checked;

            ssDiabetes3_Sheet1.Cells[20, 3].Value = chk2_2_1.Checked;
            ssDiabetes3_Sheet1.Cells[21, 3].Value = chk2_2_2.Checked;
            ssDiabetes3_Sheet1.Cells[22, 3].Value = chk2_2_3.Checked;
            ssDiabetes3_Sheet1.Cells[23, 3].Value = chk2_2_4.Checked;
            ssDiabetes3_Sheet1.Cells[24, 3].Value = chk2_2_5.Checked;

            ssDiabetes3_Sheet1.Cells[27, 3].Value = chk2_2_6.Checked;

            ssDiabetes3_Sheet1.Cells[29, 3].Value = chk2_3_1.Checked;
            ssDiabetes3_Sheet1.Cells[30, 3].Value = chk2_3_2.Checked;
            ssDiabetes3_Sheet1.Cells[32, 3].Value = chk2_3_3.Checked;
            ssDiabetes3_Sheet1.Cells[33, 3].Value = chk2_3_4.Checked;

            ssDiabetes3_Sheet1.Cells[34, 3].Value = chk2_3_5.Checked;
            ssDiabetes3_Sheet1.Cells[35, 3].Value = chk2_3_6.Checked;

            ssDiabetes3_Sheet1.Cells[39, 0].Value = chk2_4_1.Checked;
            ssDiabetes3_Sheet1.Cells[40, 0].Value = chk2_4_2.Checked;

            ssDiabetes3_Sheet1.Cells[40, 4].Value = chk2_4_3.Checked;
            ssDiabetes3_Sheet1.Cells[41, 4].Value = chk2_4_6.Checked;

            ssDiabetes3_Sheet1.Cells[40, 9].Value = chk2_4_4.Checked;
            ssDiabetes3_Sheet1.Cells[41, 9].Value = chk2_4_7.Checked;

            ssDiabetes3_Sheet1.Cells[40, 13].Value = chk2_4_5.Checked;
            ssDiabetes3_Sheet1.Cells[41, 13].Value = chk2_4_8.Checked;

            ssDiabetes3_Sheet1.Cells[43, 0].Value = chk2_5_1.Checked;

            ssDiabetes3_Sheet1.Cells[42, 5].Value = chk2_5_2.Checked;
            ssDiabetes3_Sheet1.Cells[43, 5].Value = chk2_5_3.Checked;

            ssDiabetes3_Sheet1.Cells[44, 9].Text = txt2_5_4.Text.Trim();

            ssDiabetes3_Sheet1.Cells[48, 4].Value = chk2_6_1.Checked;
            ssDiabetes3_Sheet1.Cells[48, 9].Value = chk2_6_2.Checked;
            ssDiabetes3_Sheet1.Cells[48, 12].Value = chk2_6_3.Checked;
            ssDiabetes3_Sheet1.Cells[48, 16].Value = chk2_6_4.Checked;

            ssDiabetes3_Sheet1.Cells[49, 4].Text = txt2_7_1.Text.Trim();
            
            ssDiabetes3_Sheet1.Cells[50, 6].Text = txt2_8_1.Text.Trim();
            ssDiabetes3_Sheet1.Cells[51, 6].Text = txt2_8_2.Text.Trim();

            ssDiabetes3_Sheet1.Cells[54, 6].Text = "7";

            ssDiabetes3_Sheet1.Cells[55, 0].Text = convYYMMDD(dtpBALDATE.Text.Trim());

            ssDiabetes3_Sheet1.Cells[57, 6].Text = VB.Split(cboDRCODE.Text, ".")[1];      //의사성명
            ssDiabetes3_Sheet1.Cells[57, 10].Text = txtDRBUNHO1.Text.Trim();     //면허번호

            ssDiabetes3_Sheet1.Cells[58, 6].Text = VB.Split(cboDEPTCODE.Text, ".")[1];      //전문과목
            ssDiabetes3_Sheet1.Cells[58, 10].Text = txtDRBUNHO2.Text.Trim();     //전문의자격번호

            string strFile = "";

            strDrSabun = readDrSabun(VB.Split(cboDRCODE.Text, ".")[0]);

            if (strDrSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssDiabetes3_Sheet1.Cells[57, 13].CellType = imgCellType;
                ssDiabetes3_Sheet1.Cells[57, 13].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssDiabetes3_Sheet1.Cells[57, 13].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(strDrSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(strDrSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssDiabetes3_Sheet1.Cells[57, 13].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssDiabetes3_Sheet1.Cells[57, 13].CellType = textCellType;
                ssDiabetes3_Sheet1.Cells[57, 13].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssDiabetes3_Sheet1.Cells[57, 13].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssDiabetes3_Sheet1.Cells[57, 13].Text = "(서명 또는 인)";
            }
        }

        private void spreadClear03()
        {
            ssDiabetes4_Sheet1.Cells[7, 1].Text = " ① [     ] 재발급";

            ssDiabetes4_Sheet1.Cells[8, 4].Text = "";
            ssDiabetes4_Sheet1.Cells[8, 7].Text = "";
            ssDiabetes4_Sheet1.Cells[9, 3].Text = "";
            ssDiabetes4_Sheet1.Cells[9, 7].Text = "";
            ssDiabetes4_Sheet1.Cells[10, 7].Text = "";

            ssDiabetes4_Sheet1.Cells[14, 2].Text = "";
            ssDiabetes4_Sheet1.Cells[14, 5].Text = "";
            ssDiabetes4_Sheet1.Cells[14, 8].Text = "";

            ssDiabetes4_Sheet1.Cells[21, 3].Text = "[   ] 연속혈당측정 시작일 (          ) ~ 종료일 (          ),";
            ssDiabetes4_Sheet1.Cells[22, 3].Text = "      기간 동안 착용일수 (   ) 일 또는 착용비율 (   )%";
            ssDiabetes4_Sheet1.Cells[23, 3].Text = "[   ] 당 평균값 (      )mg/dl";
            ssDiabetes4_Sheet1.Cells[24, 3].Text = "[   ] 변동계수 (      )% 혹은 표준편차 (       )mg/dl";
            ssDiabetes4_Sheet1.Cells[25, 3].Text = "[   ] 당화혈색소 검사내역 : 시행일 (        ), 검사수치 (     )%";

            ssDiabetes4_Sheet1.Cells[29, 1].Text = "제조 또는 수입업소 (                ), 제품명 (                  )";
            ssDiabetes4_Sheet1.Cells[30, 3].Text = "";
            ssDiabetes4_Sheet1.Cells[31, 3].Text = "";

            ssDiabetes4_Sheet1.Cells[35, 1].Text = "";
            ssDiabetes4_Sheet1.Cells[37, 4].Text = "";
            ssDiabetes4_Sheet1.Cells[38, 4].Text = "";
        }


        private void spreadDataSet03()
        {
            if (chkRePrt.Checked == true)
            {
                ssDiabetes4_Sheet1.Cells[7, 1].Text = " ① [  V  ] 재발급";
            }
            else
            {
                ssDiabetes4_Sheet1.Cells[7, 1].Text = " ① [     ] 재발급";
            }

            ssDiabetes4_Sheet1.Cells[8, 4].Text = txtBINO.Text.Trim();       //보장기관명
            ssDiabetes4_Sheet1.Cells[8, 7].Text = txtJUMIN.Text.Trim();      //주민등록번호
            ssDiabetes4_Sheet1.Cells[9, 3].Text = txtNAME.Text.Trim();       //성명
            ssDiabetes4_Sheet1.Cells[9, 7].Text = txtTEL.Text.Trim();       //자택
            ssDiabetes4_Sheet1.Cells[10, 7].Text = txtPHONE.Text.Trim();       //전화번호

            ssDiabetes4_Sheet1.Cells[14, 2].Text = VB.Split(cboDEPTCODE.Text, ".")[1];      //진료과목
            ssDiabetes4_Sheet1.Cells[14, 5].Text = txtILLNAME.Text.Trim();      //상병명
            ssDiabetes4_Sheet1.Cells[14, 8].Text = txtILLCODE.Text.Trim();     //상병코드

            ssDiabetes4_Sheet1.Cells[21, 3].Text = "[   ] 연속혈당측정 시작일 (          ) ~ 종료일 (          ),";
            ssDiabetes4_Sheet1.Cells[22, 3].Text = "      기간 동안 착용일수 (   ) 일 또는 착용비율 (   )%";
            ssDiabetes4_Sheet1.Cells[23, 3].Text = "[   ] 당 평균값 (      )mg/dl";
            ssDiabetes4_Sheet1.Cells[24, 3].Text = "[   ] 변동계수 (      )% 혹은 표준편차 (       )mg/dl";
            ssDiabetes4_Sheet1.Cells[25, 3].Text = "[   ] 당화혈색소 검사내역 : 시행일 (        ), 검사수치 (     )%";

            ssDiabetes4_Sheet1.Cells[29, 1].Text = "제조 또는 수입업소 ( " + txt3_5_1.Text.Trim() + " ), 제품명 ( " + txt3_5_2.Text.Trim() + " )";
            ssDiabetes4_Sheet1.Cells[30, 3].Text = txt3_5_3.Text.Trim();
            ssDiabetes4_Sheet1.Cells[31, 3].Text = txt3_5_4.Text.Trim();

            ssDiabetes4_Sheet1.Cells[35, 1].Text = convYYMMDD(dtpBALDATE.Text.Trim());      //발급일자
            ssDiabetes4_Sheet1.Cells[37, 4].Text = VB.Split(cboDRCODE.Text, ".")[1] + "(" + txtDRBUNHO1.Text.Trim() + ")";      //의사성명(면허번호)
            ssDiabetes4_Sheet1.Cells[38, 4].Text = VB.Split(cboDEPTCODE.Text, ".")[1] + "(" + txtDRBUNHO2.Text.Trim() + ")";      //전문과목(전문의자격번호)

            string strFile = "";

            strDrSabun = readDrSabun(VB.Split(cboDRCODE.Text, ".")[0]);

            if (strDrSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssDiabetes4_Sheet1.Cells[37, 5].CellType = imgCellType;
                ssDiabetes4_Sheet1.Cells[37, 5].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssDiabetes4_Sheet1.Cells[37, 5].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(strDrSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(strDrSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssDiabetes4_Sheet1.Cells[37, 5].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssDiabetes4_Sheet1.Cells[37, 5].CellType = textCellType;
                ssDiabetes4_Sheet1.Cells[37, 5].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssDiabetes4_Sheet1.Cells[37, 5].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssDiabetes4_Sheet1.Cells[37, 5].Text = "(서명 또는 인)";
            }


        }

        private void spreadClear04()
        {
            ssDiabetes5_Sheet1.Cells[3, 0].Value = false;   //재발급
            ssDiabetes5_Sheet1.Cells[6, 3].Text = "";       //보장기관명

            ssDiabetes5_Sheet1.Cells[8, 3].Text = "";       //성명
            ssDiabetes5_Sheet1.Cells[9, 3].Text = "";       //전화번호
            ssDiabetes5_Sheet1.Cells[10, 3].Text = "";      //주소

            ssDiabetes5_Sheet1.Cells[8, 13].Text = "";      //주민등록번호
            ssDiabetes5_Sheet1.Cells[9, 13].Text = "";      //휴대전화번호

            ssDiabetes5_Sheet1.Cells[11, 1].Text = "";      //진료과목
            ssDiabetes5_Sheet1.Cells[11, 8].Text = "";      //상병명
            ssDiabetes5_Sheet1.Cells[11, 15].Text = "";     //상병코드

            ssDiabetes5_Sheet1.Cells[17, 2].Value = false;      

            ssDiabetes5_Sheet1.Cells[20, 3].Value = false;
            ssDiabetes5_Sheet1.Cells[21, 3].Value = false;
            ssDiabetes5_Sheet1.Cells[22, 3].Value = false;
            ssDiabetes5_Sheet1.Cells[23, 3].Value = false;
            ssDiabetes5_Sheet1.Cells[24, 3].Value = false;

            ssDiabetes5_Sheet1.Cells[27, 3].Value = false;

            ssDiabetes5_Sheet1.Cells[48, 4].Value = false;
            ssDiabetes5_Sheet1.Cells[48, 9].Value = false;

            ssDiabetes5_Sheet1.Cells[49, 4].Text = "[                      ]";      //처방기간
            ssDiabetes5_Sheet1.Cells[49, 13].Text = "[                      ]";     //처방개수

            ssDiabetes5_Sheet1.Cells[50, 4].Text = "";      //다음처방일

            ssDiabetes5_Sheet1.Cells[55, 0].Text = "";      //발급일자

            ssDiabetes5_Sheet1.Cells[57, 6].Text = "";      //의사성명
            ssDiabetes5_Sheet1.Cells[57, 10].Text = "";     //면허번호

            ssDiabetes5_Sheet1.Cells[58, 6].Text = "";      //전문과목
            ssDiabetes5_Sheet1.Cells[58, 10].Text = "";     //전문의자격번호
        }


        private void spreadDataSet04()
        {
            ssDiabetes5_Sheet1.Cells[3, 0].Value = chkRePrt.Checked;   //재발급
            ssDiabetes5_Sheet1.Cells[6, 3].Text = txtBINO.Text.Trim() ;       //보장기관명

            ssDiabetes5_Sheet1.Cells[8, 3].Text = txtNAME.Text.Trim();       //성명
            ssDiabetes5_Sheet1.Cells[9, 3].Text = txtPHONE.Text.Trim();       //전화번호
            ssDiabetes5_Sheet1.Cells[10, 3].Text = readPatientJuso(strPANO);      //주소

            ssDiabetes5_Sheet1.Cells[8, 13].Text = txtJUMIN.Text.Trim();      //주민등록번호
            ssDiabetes5_Sheet1.Cells[9, 13].Text = txtPHONE.Text.Trim();      //휴대전화번호

            ssDiabetes5_Sheet1.Cells[11, 1].Text = VB.Split(cboDEPTCODE.Text, ".")[1];      //진료과목
            ssDiabetes5_Sheet1.Cells[11, 8].Text = txtILLNAME.Text.Trim();      //상병명
            ssDiabetes5_Sheet1.Cells[11, 15].Text = txtILLCODE.Text.Trim();     //상병코드

            ssDiabetes5_Sheet1.Cells[17, 2].Value = chk4_1_1.Checked;

            ssDiabetes5_Sheet1.Cells[20, 3].Value = chk4_1_1.Checked;
            ssDiabetes5_Sheet1.Cells[21, 3].Value = chk4_2_1.Checked;
            ssDiabetes5_Sheet1.Cells[22, 3].Value = chk4_2_3.Checked;
            ssDiabetes5_Sheet1.Cells[23, 3].Value = chk4_2_4.Checked;
            ssDiabetes5_Sheet1.Cells[24, 3].Value = chk4_2_5.Checked;

            ssDiabetes5_Sheet1.Cells[27, 3].Value = chk4_2_6.Checked;

            ssDiabetes5_Sheet1.Cells[48, 4].Value = chk4_3_1.Checked;
            ssDiabetes5_Sheet1.Cells[48, 9].Value = chk4_3_2.Checked;

            ssDiabetes5_Sheet1.Cells[49, 4].Text = "[     " + txt4_3_3.Text.Trim() + "     ]";      //처방기간
            ssDiabetes5_Sheet1.Cells[49, 13].Text = "[     " + txt4_3_3.Text.Trim() + "     ]";     //처방개수

            ssDiabetes5_Sheet1.Cells[50, 4].Text = txt4_3_5.Text.Trim();      //다음처방일

            ssDiabetes5_Sheet1.Cells[55, 0].Text = convYYMMDD(dtpBALDATE.Text.Trim());      //발급일자

            ssDiabetes5_Sheet1.Cells[57, 6].Text = VB.Split(cboDRCODE.Text, ".")[1];      //의사성명
            ssDiabetes5_Sheet1.Cells[57, 10].Text = txtDRBUNHO1.Text.Trim();     //면허번호

            ssDiabetes5_Sheet1.Cells[58, 6].Text = VB.Split(cboDEPTCODE.Text, ".")[1];      //전문과목
            ssDiabetes5_Sheet1.Cells[58, 10].Text = txtDRBUNHO2.Text.Trim();     //전문의자격번호

            string strFile = "";

            strDrSabun = readDrSabun(VB.Split(cboDRCODE.Text, ".")[0]);

            if (strDrSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssDiabetes5_Sheet1.Cells[57, 13].CellType = imgCellType;
                ssDiabetes5_Sheet1.Cells[57, 13].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssDiabetes5_Sheet1.Cells[57, 13].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(strDrSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(strDrSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssDiabetes5_Sheet1.Cells[57, 13].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssDiabetes5_Sheet1.Cells[57, 13].CellType = textCellType;
                ssDiabetes5_Sheet1.Cells[57, 13].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssDiabetes5_Sheet1.Cells[57, 13].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssDiabetes5_Sheet1.Cells[57, 13].Text = "(서명 또는 인)";
            }
        }
    }
}
