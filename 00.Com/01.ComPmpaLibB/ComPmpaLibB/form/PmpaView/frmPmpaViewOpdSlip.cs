using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewOpdSlip.cs
    /// Description     : 외래진료비내역
    /// Author          : 안정수
    /// Create Date     : 2017-08-14
    /// Update History  : 2017-11-30
    /// <history>       
    /// TODO : 출력 부분 실제 테스트 필요
    /// d:\psmh\OPD\oviewa\OVIEWA10.FRM(FrmOpdSlip) => frmPmpaViewOpdSlip.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA10.FRM(FrmOpdSlip)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewOpdSlip : Form
    {
        ComFunc CF = new ComFunc();
        frmSetPrintInfo frmSetPrintInfoX = new frmSetPrintInfo();

        //public delegate void SendPano(string strPano);
        //public event SendPano rSendPano;

        string mstrPano = "";
        string mstrBuseCode = "";
        string mstrJobName = "";
        //string GstrRetValue = "";

        string strFnu = "";
        string strTnu = "";
        string strBi = "";
        string strBiName = "";
        string strDEPTCODE = "";
        string strSname = "";
        string strIndate = "";
        string strOutdate = "";

        int nMenuChoice = 0;
        int nGetcount = 0;

        string mstrJobSabun = "";
        string FstrChkBi = "";

        public frmPmpaViewOpdSlip()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewOpdSlip(string GstrPANO, string PassBuse, string strJobName, string strJobSabun)
        {
            InitializeComponent();
            setEvent();
            mstrPano = GstrPANO;
            mstrBuseCode = PassBuse;
            mstrJobName = strJobName;
            mstrJobSabun = strJobSabun;
        }

        public frmPmpaViewOpdSlip(string rstrPano, string rstrFrDate, string rstrToDate, string rstrDeptCode, string rstrJindate)
        {
            InitializeComponent();
            setEvent();
            mstrPano = rstrPano;
            strDEPTCODE = rstrDeptCode;
            if (rstrJindate != "")
            {
                strIndate = VB.Left(rstrJindate,4) + "-" + VB.Mid(rstrJindate, 5,2) + "-" + VB.Right(rstrJindate, 2);
                strOutdate = VB.Left(rstrJindate, 4) + "-" + VB.Mid(rstrJindate, 5, 2) + "-" + VB.Right(rstrJindate, 2);
            }
            else if (rstrFrDate !="")
            {
                strIndate = rstrFrDate;
                strOutdate = rstrToDate;
            }



        }


        
        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnOk.Click += new EventHandler(eBtnEvent);         
            
            this.btnNext.Click += new EventHandler(eBtnEvent);

            this.optJob1.Click += new EventHandler(eBtnEvent);
            this.optJob2.Click += new EventHandler(eBtnEvent);
            this.optJob3.Click += new EventHandler(eBtnEvent);
            this.optJob4.Click += new EventHandler(eBtnEvent);
            this.optJob5.Click += new EventHandler(eBtnEvent);

            this.txtPano.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.cboBi.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.cboDept.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpFdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTdate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.cboFnu.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.cboTnu.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(sender == txtPano)
            {
                if (e.KeyChar == 13)
                {
                    //if(dtpFdate.Enabled == false)
                    if (!VB.IsNumeric(txtPano.Text))
                    {

                    }
                    else
                    {
                        txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
                        Read_Bas_Patient(txtPano.Text);
                        cboBi.Focus();
                    }

                }

                if (e.KeyChar != 13)
                {
                    return;
                }

                //등록번호 자동입력을 위한 전역 변수 2012-04-03 이주형
                //rSendPano(txtPano.Text);
            }

            else if (sender == this.cboBi)
            {
                if (e.KeyChar == 13)
                {
                    cboDept.Focus();
                }
            }

            else if (sender == this.cboDept)
            {
                if (e.KeyChar == 13)
                {
                    dtpFdate.Focus();
                }
            }

            else if (sender == this.dtpFdate)
            {
                if (e.KeyChar == 13)
                {
                    dtpTdate.Focus();
                }
            }

            else if (sender == this.dtpTdate)
            {
                if (e.KeyChar == 13)
                {
                    cboFnu.Focus();
                }
            }

            else if (sender == this.cboFnu)
            {
                if (e.KeyChar == 13)
                {
                    cboTnu.Focus();
                }
            }

            else if (sender == this.cboTnu)
            {
                if (e.KeyChar == 13)
                {
                    btnOk.Focus();
                }
            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnOk)
            {
                //                
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                btnOk_Click();
            }

            else if (sender == this.btnPrint)
            {
                //                
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                //ePrint();
                ePrint2();
            }

            else if (sender == this.btnNext)
            {
                btnNext_Click();
            }

            else if (sender == optJob1)
            {
                btnJob1_Click();
            }
            else if (sender == optJob2)
            {
                btnJob2_Click();
            }
            else if (sender == optJob3)
            {
                btnJob3_Click();
            }
            else if (sender == optJob4)
            {
                btnJob4_Click();
            }
            else if (sender == optJob5)
            {
                btnJob5_Click();
            }
        }

        void frmPmpaViewOpdSlip_Activated(object sender, EventArgs e)
        {
            Clear_Screen();
            //txtPano.Text = mstrPano;

            if (VB.Left(mstrBuseCode, 4) == "0782")
            {
                chkJin.Checked = false;
            }
        }


        void eFormActivated(object sender, EventArgs e)
        {
            if (clsPublic.GstrPANO != txtPano.Text && clsPublic.GstrPANO != "")
            {
                txtPano.Text = clsPublic.GstrPANO;

                Read_Bas_Patient(txtPano.Text);
                dtpFdate.Text = strIndate;
                dtpTdate.Text = strOutdate;
                if (strDEPTCODE !="" )cboDept.Text = strDEPTCODE;
                //ComFunc.ComboFind(cboDept, "L", 1, strDEPTCODE.ToString().Trim());
                btnOk_Click();
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  

            this.StartPosition = FormStartPosition.CenterScreen;

            lblSname.Text = "이　　름";
            chkJin.Checked = true;
            nMenuChoice = 1;
            optJob1.Checked = true;

            this.Text = "";
            Set_Combo();
            NuName_Setting();
            Clear_Screen();
            //txtPano.Focus();  2020-04-01 KHS

            //cboBi.SelectedIndex = 0;
            if (strDEPTCODE =="") cboDept.SelectedIndex = 0;


            if (mstrPano != "")
            {
                txtPano.Text = mstrPano;

                Read_Bas_Patient(txtPano.Text);
                dtpFdate.Text = strIndate;
                dtpTdate.Text = strOutdate;
                cboDept.Text = strDEPTCODE;
                //ComFunc.ComboFind(cboDept, "L", 1, strDEPTCODE.ToString().Trim());

                btnOk_Click();
            }
        }

        void Clear_Screen()
        {
            
            //ssList_Sheet1.Rows.Count = 0;

            cboFnu.SelectedIndex = 0;
            cboTnu.SelectedIndex = 0;
            cboBi.SelectedIndex = 0;
            cboDept.SelectedIndex = 0;

            //btnPrint.Enabled = false;
            //btnOk.Enabled = false;
            //ssList.Enabled = false;
            //dtpFdate.Enabled = false;
            //dtpTdate.Enabled = false;

            strFnu = "01";
            strTnu = "50";
        }

        void Set_Combo()
        {
            int i = 0;
            string strDept = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            cboFnu.Items.Add(""); cboTnu.Items.Add("");
            cboFnu.Items.Add("진찰료"); cboTnu.Items.Add("진찰료");
            cboFnu.Items.Add("입원료"); cboTnu.Items.Add("입원료");
            cboFnu.Items.Add("투약료"); cboTnu.Items.Add("투약료");
            cboFnu.Items.Add("주사료"); cboTnu.Items.Add("주사료");
            cboFnu.Items.Add("마취료"); cboTnu.Items.Add("마취료");
            cboFnu.Items.Add("PT/NS"); cboTnu.Items.Add("PT/NS");
            cboFnu.Items.Add("처치료"); cboTnu.Items.Add("처치료");
            cboFnu.Items.Add("수술료"); cboTnu.Items.Add("수술료");
            cboFnu.Items.Add("검사료"); cboTnu.Items.Add("검사료");
            cboFnu.Items.Add("방사선"); cboTnu.Items.Add("방사선");
            cboFnu.Items.Add("비급여"); cboTnu.Items.Add("비급여");
            cboFnu.Items.Add("현금계정"); cboTnu.Items.Add("현금계정");

            //환자종류
            cboBi.Items.Clear();
            cboBi.Items.Add("00.전체");
            cboBi.Items.Add("11.공단");
            cboBi.Items.Add("12.연합회");
            cboBi.Items.Add("13.지역");
            cboBi.Items.Add("21.보호1종");
            cboBi.Items.Add("22.보호2종");
            cboBi.Items.Add("23.의료부조");
            cboBi.Items.Add("24.행려환자");
            cboBi.Items.Add("31.산재");
            cboBi.Items.Add("32.공무원공상");
            cboBi.Items.Add("33.산재공상");
            cboBi.Items.Add("41.공단100%");
            cboBi.Items.Add("42.직장100%");
            cboBi.Items.Add("43.지역100%");
            cboBi.Items.Add("44.가족계획");
            cboBi.Items.Add("45.보험계약");
            cboBi.Items.Add("51.일반");
            cboBi.Items.Add("52.TA보험");
            cboBi.Items.Add("53.계약");
            cboBi.Items.Add("54.미확인");
            cboBi.Items.Add("55.TA일반");
            cboBi.SelectedIndex = 1;

            //진료과
            cboDept.Items.Clear();
            cboDept.Items.Add("00.전체");

            strDept = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "* FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
            SQL += ComNum.VBLF + "WHERE PrintRanking < 30";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDept = dt.Rows[i]["DeptCode"].ToString().Trim() + ".";
                        strDept += dt.Rows[i]["DeptNameK"].ToString().Trim();
                        cboDept.Items.Add(strDept);
                    }
                }
                cboDept.Items.Add("RD.영상의학과");
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

        }

        void NuName_Setting()
        {
            clsPmpaPb.GstrSETNus[0] = "진찰료";
            clsPmpaPb.GstrSETNus[1] = "입원실료";
            clsPmpaPb.GstrSETNus[2] = "환자관리";
            clsPmpaPb.GstrSETNus[3] = "투약료";
            clsPmpaPb.GstrSETNus[4] = "주사료";
            clsPmpaPb.GstrSETNus[5] = "마취료";
            clsPmpaPb.GstrSETNus[6] = "물리치료";
            clsPmpaPb.GstrSETNus[7] = "신경정신";
            clsPmpaPb.GstrSETNus[8] = "처치료";
            clsPmpaPb.GstrSETNus[9] = "수술분만";
            clsPmpaPb.GstrSETNus[10] = "수혈료";
            clsPmpaPb.GstrSETNus[11] = "기브스료";
            clsPmpaPb.GstrSETNus[12] = "특수검사";
            clsPmpaPb.GstrSETNus[13] = "기타검사";
            clsPmpaPb.GstrSETNus[14] = "방사선료";
            clsPmpaPb.GstrSETNus[15] = "식대료";
            clsPmpaPb.GstrSETNus[16] = "보호안치료";
            clsPmpaPb.GstrSETNus[17] = "MRI(급여)";
            clsPmpaPb.GstrSETNus[18] = "급여C/T";
            clsPmpaPb.GstrSETNus[19] = "기타급여";
            clsPmpaPb.GstrSETNus[20] = "비급기본";
            clsPmpaPb.GstrSETNus[21] = "비급투약료";
            clsPmpaPb.GstrSETNus[22] = "비급주사료";
            clsPmpaPb.GstrSETNus[23] = "비급마취료";
            clsPmpaPb.GstrSETNus[24] = "비급물리료";
            clsPmpaPb.GstrSETNus[25] = "비급신경료";
            clsPmpaPb.GstrSETNus[26] = "비급처치료";
            clsPmpaPb.GstrSETNus[27] = "비급수술료";
            clsPmpaPb.GstrSETNus[28] = "비급수혈료";
            clsPmpaPb.GstrSETNus[29] = "비급기브료";
            clsPmpaPb.GstrSETNus[30] = "비급특검료";
            clsPmpaPb.GstrSETNus[31] = "비급검사료";
            clsPmpaPb.GstrSETNus[32] = "비급방사선";
            clsPmpaPb.GstrSETNus[33] = "식대료";
            clsPmpaPb.GstrSETNus[34] = "실료차";
            clsPmpaPb.GstrSETNus[35] = "초음파";
            clsPmpaPb.GstrSETNus[36] = "C/T";
            clsPmpaPb.GstrSETNus[37] = "MRI(비급여)";
            clsPmpaPb.GstrSETNus[38] = "보조기";
            clsPmpaPb.GstrSETNus[39] = "보철료";
            clsPmpaPb.GstrSETNus[40] = "구급차사용";
            clsPmpaPb.GstrSETNus[41] = "안치료";
            clsPmpaPb.GstrSETNus[42] = "골밀도검사";
            clsPmpaPb.GstrSETNus[43] = "특진료";
            clsPmpaPb.GstrSETNus[44] = "";
            clsPmpaPb.GstrSETNus[45] = "전화료";
            clsPmpaPb.GstrSETNus[46] = "제증명료";
            clsPmpaPb.GstrSETNus[47] = "특수재료대";
            clsPmpaPb.GstrSETNus[48] = "";
            clsPmpaPb.GstrSETNus[49] = "합계금액";
            clsPmpaPb.GstrSETNus[50] = "보증금";
            clsPmpaPb.GstrSETNus[51] = "중간납";
            clsPmpaPb.GstrSETNus[52] = "조합부담금";
            clsPmpaPb.GstrSETNus[53] = "감액";
            clsPmpaPb.GstrSETNus[54] = "차인납부액";
            clsPmpaPb.GstrSETNus[55] = "개인미수";
            clsPmpaPb.GstrSETNus[56] = "퇴원금";
            clsPmpaPb.GstrSETNus[57] = "환불금";
            clsPmpaPb.GstrSETNus[58] = "";
            clsPmpaPb.GstrSETNus[59] = "전액본인부담";
            clsPmpaPb.GstrSETNus[60] = "";
            clsPmpaPb.GstrSETNus[61] = "";
            clsPmpaPb.GstrSETNus[62] = "";
            clsPmpaPb.GstrSETNus[63] = "외래영수금";
        }

        void Read_Bas_Patient(string strPano)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                ";
            SQL += ComNum.VBLF + "  Sname";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
            SQL += ComNum.VBLF + "WHERE Pano = '" + strPano + "'        ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox(txtPano.Text + " 환자마스타 없습니다.");
                    txtPano.Focus();
                }

                strSname = dt.Rows[0]["Sname"].ToString().Trim();
                lblSname.Text = strSname;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            //txtPano.Enabled = false;

            btnNext.Enabled = true;
            groupBox7.Enabled = false;
            btnExit.Enabled = true;

            dtpFdate.Enabled = true;
            dtpTdate.Enabled = true;
            cboFnu.Enabled = true;
            cboTnu.Enabled = true;
            btnOk.Enabled = true;

            dt.Dispose();
            dt = null;
        }
       
        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            Application.DoEvents();

            string strTitle = "";
            string strTitle2 = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;

            ssList.ActiveSheet.Columns[9].Visible = false;
            ssList.ActiveSheet.Columns[10].Visible = false;
            ssList.ActiveSheet.Columns[11].Visible = false;
            ssList.ActiveSheet.Columns[12].Visible = false;
            ssList.ActiveSheet.Columns[13].Visible = false;
            ssList.ActiveSheet.Columns[14].Visible = false;


            #endregion

            if (chkPrtIP.Checked == true)
            {
                strTitle = "(입원) 항 목 별  진 료 내 역";
            }

            else
            {
                strTitle = "(외래) 항 목 별  진 료 내 역";
            }

            strTitle2 = txtPano.Text + "  " + strSname + "  ";
            strTitle2 += "구분: " + cboBi.SelectedItem.ToString().Trim() + VB.Space(3) + "진료과:" + cboDept.SelectedItem.ToString().Trim();

            if (clsType.User.UserName != "")
            {
                strTitle2 += "  기간:" + dtpFdate.Text + "-" + dtpTdate.Text + "  발급자:" + clsType.User.UserName;
            }
            else
            {
                strTitle2 += "  기간:" + dtpFdate.Text + "-" + dtpTdate.Text;
            }

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strTitle2, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);

            //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter = SPR.setSpdPrint_String("- 포항성모병원 -", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter += SPR.setSpdPrint_String( "** 본원은 종합병원으로서 종별가산율은 의료급여 18%, 건강보험 25% 입니다. **", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Center, false, true);

            ssList.ActiveSheet.Columns[9].Visible = false;
            ssList.ActiveSheet.Columns[10].Visible = false;

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, true, false, false, true, false, true, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true; 
            ssList.ActiveSheet.Columns[9].Visible = true;
            ssList.ActiveSheet.Columns[10].Visible = true;
            ssList.ActiveSheet.Columns[11].Visible = true;
            ssList.ActiveSheet.Columns[12].Visible = true;
            ssList.ActiveSheet.Columns[13].Visible = true;
            ssList.ActiveSheet.Columns[14].Visible = true;
            #endregion

            btnExit.Enabled = true;
            //}
        }

        void ePrint2()
        {

            //'Host Call Routine  : 작업구분(N:누적별, I:항목별, D:일자별)
            //'                     병록번호(8자리), 환자구분(2자리),
            //'                     From Nu(2자리), To Nu(2자리)
            string strHeader = "";
            string strFooter = "";

            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strFoot = "";
            string strTitle = "";
            string strSubTitle = "";
            bool PrePrint = chkPreView.Checked;

            if (MessageBox.Show("인쇄를 하시겠습니까?", "알 림", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            if (FstrChkBi == "OK" && chkDoctor.Checked == true)
            {
                if (DialogResult.No == MessageBox.Show("건보+보호자격에 요양병원체크가 되어있습니다. 그래도 출력하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    return;
            }

            clsPublic.GstrRetValue = txtPano.Text + "^^" + "외래^^" + dtpFdate.Text + "~" + dtpTdate.Text + "^^";
            frmSetPrintInfo f = new frmSetPrintInfo();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog(this);

            //if (VB.Pstr(clsPublic.GstrRetValue, "^^", 1) != "OK") // 출력 않되게 한이유 ??@ㅓㅛㅐㅏ12ㅓㅓㅛ
            //{
            //    return;
            //}

            //nPage = Convert.ToInt32(VB.Pstr(clsPublic.GstrRetValue, "^^", 1));
            //nPage = 1;
            //clsPublic.GstrRetValue = "";

            //for (nX = 1; nX <= nPage; nX++)
            //{
                #region 시트 히든
                ssList_Sheet1.Columns[9].Visible = false;
                ssList_Sheet1.Columns[10].Visible = false;
                ssList_Sheet1.Columns[11].Visible = false;
                ssList_Sheet1.Columns[12].Visible = false;
                ssList_Sheet1.Columns[13].Visible = false;
                ssList_Sheet1.Columns[14].Visible = false;
                #endregion

                strSubTitle = txtPano.Text + " " + strSname + " ";
           // strHead2 = "/c/f2" + "등록번호: " + txtPano.Text + VB.Space(5) + " 환자성명: " + lblSname.Text + VB.Space(5) + " 진료기간: " + dtpFdate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTdate.Value.ToString("yyyy-MM-dd") + VB.Space(5) + " 진료과: " + cboDept.Text + VB.Space(5) + " 환자구분" + cboBi.Text + VB.Space(10) + " 비고" + "/f2/n";

            if (clsType.User.UserName != "")
                {
                strSubTitle += "구분: " + cboBi.SelectedItem.ToString() + "  기간: " + dtpFdate.Text + "-" + dtpTdate.Text + VB.Space(5) + " 진료과: " + cboDept.Text + "  발급자:" + clsType.User.UserName;
                }
                else
                {
                    strSubTitle += "구분: " + cboBi.SelectedItem.ToString() + "-" + dtpTdate.Text;
                }

                if (chkPrtIP.Checked == true)
                {
                    strTitle = "(입원)  항 목 별  진 료 내 역";
                }
                else
                {
                    strTitle = "(외래)  항 목 별  진 료 내 역";
                }

                strFoot = "/f/f2/c" + "- 포항성모병원 -" + "/n/n";
                strFoot += "/l/f2/c" + "** 본원은 종합병원으로서 종별가산율은 의료급여 18% 건강보험 25% 입니다. **";

                ssList_Sheet1.Columns[9].Visible = false;
                ssList_Sheet1.Columns[10].Visible = false;

                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);

                strFooter = SPR.setSpdPrint_String("- 포항성모병원 -", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Center, false, true);
                strFooter += SPR.setSpdPrint_String("** 본원은 종합병원으로서 종별가산율은 의료급여 18%, 건강보험 25% 입니다. **", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Center, false, true);

                if (chkJiwon.Checked == true)
                {
                    strFooter += SPR.setSpdPrint_String("*  진찰료에 의료질평가 지원금이 포함되어 있습니다. **", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Center, false, true);
                    strFooter += SPR.setSpdPrint_String("*  AU214  AU233  의료질평가지원금 - 의료질, 안전 3등급(외래)   2,230원 *", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Center, false, true);
                    strFooter += SPR.setSpdPrint_String("*  AU312  AU313  의료질평가지원금 - 교육수련 3등급(외래)          60원 *", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Center, false, true);
                    strFooter += SPR.setSpdPrint_String("*  AU413  2018년 9월1일부터 연구개발분야지원금 - 교육수련 3등급(외래)   70원 *", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Center, false, true);
                }

                ssList.ActiveSheet.Columns[9].Visible = false;
                ssList.ActiveSheet.Columns[10].Visible = false;

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, false, true, false, true, false);

                SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);                
            //}
       
            #region //시트 히든 해제

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;

            ssList.ActiveSheet.Columns[9].Visible = true;
            ssList.ActiveSheet.Columns[10].Visible = true;
            ssList.ActiveSheet.Columns[11].Visible = true;
            ssList.ActiveSheet.Columns[12].Visible = true;
            ssList.ActiveSheet.Columns[13].Visible = true;
            ssList.ActiveSheet.Columns[14].Visible = true;


            #endregion

            btnExit.Enabled = true;
            
        }

        void btnOk_Click()
        {
            string strDrCode = "";
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            Cursor.Current = Cursors.WaitCursor;

            strBi = VB.Left(cboBi.SelectedItem.ToString(), 2);
            strBiName = VB.Mid(cboBi.SelectedItem.ToString() + VB.Space(12), cboBi.SelectedItem.ToString().Length + 1, 12);
            //strDEPTCODE = VB.Left(cboDept.SelectedItem.ToString(), 2);
            strDEPTCODE = VB.Left(cboDept.Text, 2);
            //strDeptName = VB.Mid(cboDept.SelectedItem.ToString() + VB.Space(12), cboDept.SelectedItem.ToString().Length + 1, 12);

            if (dtpFdate.Text == "")
            {
                ComFunc.MsgBox("시작일자를 입력하세요 !!");
                return;
            }

            if (dtpTdate.Text == "")
            {
                ComFunc.MsgBox("종료일자를 입력하세요 !!");
                return;
            }

            if (String.Compare(strTnu, strFnu) < 0)
            {
                ComFunc.MsgBox("행위범위 FROM < TO Error !!");
                return;
            }
            else
            {
                btnPrint.Enabled = false;
                ssList_Sheet1.Rows.Count = 0;
                ss1_Clear();

                switch (nMenuChoice)
                {
                    case 1:
                        Read_OPD_SLIP1();
                        break;
                    case 2:
                        Read_OPD_SLIP2();
                        break;
                    case 3:
                        Read_OPD_SLIP3();
                        break;
                    case 4:
                        Read_OPD_SLIP4();
                        break;
                    case 5:
                        Read_OPD_SLIP5();
                        break;
                }

                FstrChkBi = "";

                if (chkDoctor.Checked == true)
                {
                    strDrCode = "";

                    //2014-12-29 주치의, 요양기관번호 기재
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                ";
                    SQL += ComNum.VBLF + "  DRCODE, bi                                                          ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                                 ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                             ";
                    SQL += ComNum.VBLF + "      AND Pano='" + txtPano.Text + "'                                 ";
                    SQL += ComNum.VBLF + "      AND BDATE>=TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')        ";
                    SQL += ComNum.VBLF + "      AND BDATE<=TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD')        ";

                    if (VB.Left(cboDept.SelectedItem.ToString(), 2) != "00")
                    {
                        SQL += ComNum.VBLF + "      AND DEPTCODE='" + VB.Left(cboDept.SelectedItem.ToString(), 2) + "' ";
                    }

                    try
                    {
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (VB.I(strDrCode, CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim())) < 2)
                            {
                                strDrCode += CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim()) + "(" + CF.READ_OCS_Doctor3_DrBunho(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim()) + "), ";

                                if (string.Compare(dt.Rows[i]["BI"].ToString().Trim(), "30") <= 0)
                                    FstrChkBi = "OK";
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    if (VB.Right(strDrCode, 2) == ", ")
                    {
                        strDrCode = VB.Left(strDrCode, VB.Len(strDrCode) - 2);
                    }

                    ssList_Sheet1.Rows.Count += 1;

                    ssList.Sheets[0].AddSpanCell(0, ssList_Sheet1.Rows.Count - 1, 2, 1);
                    ssList.Sheets[0].AddSpanCell(3, ssList_Sheet1.Rows.Count - 1, ssList_Sheet1.Columns.Count - 1, 1);

                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "요양기관번호: 37100068";
                    ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 3].Text = "주치의: " + strDrCode;

                }

                if(dtpFdate.Text !="" && dtpTdate.Text != "")//2020-04-01
                {
                    if (string.Compare(dtpFdate.Text, Convert.ToDateTime(dtpTdate.Text).AddDays(-1830).ToShortDateString()) <= 0)
                    {
                        ComFunc.MsgBox("영수증 재발행은 5년까지만 가능합니다.");

                        btnPrint.Enabled = false;

                        if (CF.JIN_AMT_PRINT_CHK(clsDB.DbCon, mstrJobSabun) == "OK")
                        {
                            btnPrint.Enabled = true;
                        }
                    }
                    else
                    {
                        btnPrint.Enabled = true;
                    }
                }

                //if (string.Compare(dtpFdate.Text, Convert.ToDateTime(dtpTdate.Text).AddDays(-1830).ToShortDateString()) <= 0)
                //{
                //    ComFunc.MsgBox("영수증 재발행은 5년까지만 가능합니다.");

                //    btnPrint.Enabled = false;

                //    if (CF.JIN_AMT_PRINT_CHK(clsDB.DbCon, mstrJobSabun) == "OK")
                //    {
                //        btnPrint.Enabled = true;
                //    }
                //}
                //else
                //{
                //    btnPrint.Enabled = true;
                //}
            }

            Cursor.Current = Cursors.Default;
        }

        void cboFnu_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboFnu.SelectedIndex)
            {
                case 2:
                    strFnu = "02";
                    break;
                case 3:
                    strFnu = "04";
                    break;
                case 4:
                    strFnu = "05";
                    break;
                case 5:
                    strFnu = "06";
                    break;
                case 6:
                    strFnu = "07";
                    break;
                case 7:
                    strFnu = "09";
                    break;
                case 8:
                    strFnu = "10";
                    break;
                case 9:
                    strFnu = "13";
                    break;
                case 10:
                    strFnu = "15";
                    break;
                case 11:
                    strFnu = "21";
                    break;
                case 12:
                    strFnu = "51";
                    break;
                default:
                    strFnu = "01";
                    break;
            }
        }

        void cboTnu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTnu.SelectedIndex < cboFnu.SelectedIndex)
            {
                cboTnu.SelectedIndex = cboFnu.SelectedIndex;
            }

            switch (cboTnu.SelectedIndex)
            {
                case 1:
                    strTnu = "01";
                    break;
                case 2:
                    strTnu = "03";
                    break;
                case 3:
                    strTnu = "04";
                    break;
                case 4:
                    strTnu = "05";
                    break;
                case 5:
                    strTnu = "06";
                    break;
                case 6:
                    strTnu = "08";
                    break;
                case 7:
                    strTnu = "09";
                    break;
                case 8:
                    strTnu = "12";
                    break;
                case 9:
                    strTnu = "14";
                    break;
                case 10:
                    strTnu = "20";
                    break;
                case 11:
                    strTnu = "50";
                    break;
                case 12:
                    strTnu = "99";
                    break;
                default:
                    strTnu = "50";
                    break;
            }
        }

        void btnJob1_Click()
        {
            this.Text = "(외래)  항 목 별  진 료 내 역";
            ssList_Sheet1.ColumnHeader.Cells[0, 0].Text = "행위구분";
            ssList_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
            ssList_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";
            nMenuChoice = 1;
        }

        void btnJob2_Click()
        {
            this.Text = "(외래)  일 자 별  진 료 내 역";
            ssList_Sheet1.ColumnHeader.Cells[0, 0].Text = "발생일자";
            ssList_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
            ssList_Sheet1.ColumnHeader.Cells[0, 2].Text = "한글수가";
            nMenuChoice = 2;
            btnNext.Enabled = true;
        }

        void btnJob3_Click()
        {
            this.Text = "(외래) 진료내역 (행위,일자별)";
            ssList_Sheet1.ColumnHeader.Cells[0, 0].Text = "행위구분";
            ssList_Sheet1.ColumnHeader.Cells[0, 1].Text = "발생일자";
            ssList_Sheet1.ColumnHeader.Cells[0, 2].Text = "수가코드";
            nMenuChoice = 3;
            btnNext.Enabled = true;
        }

        void btnJob4_Click()
        {
            this.Text = "(외래)  항 목 별  진 료 내 역 Ⅱ";
            ssList_Sheet1.ColumnHeader.Cells[0, 0].Text = "행위구분";
            ssList_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
            ssList_Sheet1.ColumnHeader.Cells[0, 2].Text = "한글수가";
            nMenuChoice = 4;
        }

        void btnJob5_Click()
        {
            this.Text = "(외래표준코드)  항 목 별  진 료 내 역";
            ssList_Sheet1.ColumnHeader.Cells[0, 0].Text = "행위구분";
            ssList_Sheet1.ColumnHeader.Cells[0, 1].Text = "수가코드";
            ssList_Sheet1.ColumnHeader.Cells[0, 2].Text = "표준코드";
            nMenuChoice = 5;
        }

        void btnNext_Click()
        {
            Clear_Screen();

            btnExit.Enabled = true;
            groupBox7.Enabled = true;

            txtPano.Enabled = true;
            txtPano.Text = "";
            txtPano.Focus();
            lblSname.Text = "";

            clsSpread CS = new clsSpread();
            CS.Spread_All_Clear(ssList);
        }
        
        void Read_OPD_SLIP1()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAmt1 = "";
            string strAmt2 = "";

            int nNu = 0;
            int nNuChk = 0;
            int i = 0;
            int j = 0;
            int nRowInx = 0;
            int nCnt2 = 0;
            int nInitNo = 0;

            int nAmt1 = 0;
            int nAmt2 = 0;
            int nStot1 = 0;
            int nStot2 = 0;
            int nGtot1 = 0;
            int nGtot2 = 0;
            int nNalSu = 0;
            double nAmt = 0;

            nInitNo = 66;

            #region SQL_SET_SLIP1(GoSub)
            // 2009-10-26 윤조연 추가 = 당일진찰료 포함 루틴추가

            SQL = "";
            SQL += ComNum.VBLF + "SELECT    ";
            SQL += ComNum.VBLF + "  Sucode, I.Sunext, SunameK, B.Bcode, BaseAmt, Qty,SUGBp,I.GBSUGBS,";
            SQL += ComNum.VBLF + "  GbSpc, GbNgt, GbGisul, GbSelf, GbChild, Nu,";
            SQL += ComNum.VBLF + "  SUM(Nal) Nalsu, SUM(Amt1) Amtt1, SUM(Amt2) Amtt2 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Pano = '" + txtPano.Text + "'";
            if (strBi != "00")
            {
                SQL += ComNum.VBLF + "      AND Bi   = '" + strBi + "' ";
            }
            if (strDEPTCODE != "00")
            {
                SQL += ComNum.VBLF + "      AND DeptCode = '" + strDEPTCODE + "' ";
            }
            SQL += ComNum.VBLF + "  AND Nu BETWEEN '" + strFnu + "' ";
            SQL += ComNum.VBLF + "  AND '" + strTnu + "' ";
            SQL += ComNum.VBLF + "  AND I.Sunext = B.Sunext";
            SQL += ComNum.VBLF + "  AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";  //저가약제 제외코드 2011-04-09
            SQL += ComNum.VBLF + "  AND Bdate BETWEEN TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "GROUP BY Sucode, I.Sunext, SunameK, B.Bcode, BaseAmt, Qty,SUGBp,I.GBSUGBS,";
            SQL += ComNum.VBLF + "GbSpc, GbNgt, GbGisul, GbSelf, GbChild, Nu";

            if (DateTime.Now.ToString("yyyy-MM-dd") == dtpTdate.Text && chkJin.Checked == true)  // 진찰료 포함
            {
                SQL += ComNum.VBLF + "UNION ALL";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  decode(chojae,'1','AA176','2','AA1761','3','AA276','4','AA2761','5','AA1762','6','AA2762')  Sucode, ";
                SQL += ComNum.VBLF + "  '' Sunext,'진찰료' SunameK,    ";
                SQL += ComNum.VBLF + "  decode(chojae,'1','AA176','2','AA156050','3','AA256','4','AA256010','5','AA156050','6','AA256050') Hcode,";
                SQL += ComNum.VBLF + "  AMT1 BASEAMT  , COUNT(Pano) Qty,'','', ";
                SQL += ComNum.VBLF + "  '' GbSpc, '' GbNgt,'' GbGisul, '1' GbSelf, '' GbChild, '01' Nu, ";
                SQL += ComNum.VBLF + "  1 Nalsu, SUM(Amt1) Amtt1,0 as Amtt2  ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND PANO ='" + txtPano.Text + "'  ";
                SQL += ComNum.VBLF + "      AND ACTDATE =trunc(sysdate)";
                if (strBi != "00")
                {
                    SQL += ComNum.VBLF + "      AND Bi   = '" + strBi + "' ";
                }
                if (strDEPTCODE != "00")
                {
                    SQL += ComNum.VBLF + "      AND DeptCode = '" + strDEPTCODE + "'  ";
                }
                SQL += ComNum.VBLF + "GROUP by decode(chojae,'1','AA176','2','AA1761','3','AA276','4','AA2761','5','AA1762','6','AA2762') ,2,3,";
                SQL += ComNum.VBLF + "         decode(chojae,'1','AA176','2','AA156050','3','AA256','4','AA256010','5','AA156050','6','AA256050'),";
                SQL += ComNum.VBLF + "         amt1,6,7,8,9,10,11,12    ";
                SQL += ComNum.VBLF + "ORDER BY 12, 1, 2";
            }
            else
            {
                SQL += ComNum.VBLF + "ORDER BY Nu, ";
                SQL += ComNum.VBLF + "         DECODE(Qty, .1,1,.2,2,.3,3,.4,4,.5,5,.6,6,.7,7,.8,8,.9,9,0), ";
                SQL += ComNum.VBLF + "         Sucode, I.Sunext";
            }
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            #endregion SQL_SET_SLIP1(GoSub) End

            #region SQL_MAIN_SLIP1(GoSub)
            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRowInx = 0;

            if (dt.Rows.Count == 0)
            {
                btnPrint.Enabled = true;
                ssList.Enabled = true;
                ComFunc.MsgBox("해당조건의 데이터는 없습니다.");
                dt.Dispose();
                dt = null;
                return;
            }

            nGetcount += dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nNalSu = Convert.ToInt16(dt.Rows[i]["NalSu"].ToString().Trim());
                nAmt = Convert.ToDouble(dt.Rows[i]["Amtt1"].ToString().Trim());

                if (nNalSu != 0 || nAmt != 0)
                {
                    nNu = Convert.ToInt32(dt.Rows[i]["Nu"].ToString().Trim());
                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        if (dt.Rows[i]["GBSELF"].ToString().Trim() == "2")
                        {
                            strNujuk = "전액본인부담";
                        }
                        else
                        {
                            strNujuk = clsPmpaPb.GstrSETNus[nNu - 1];
                        }
                    }

                    if (nNu != nNuChk)
                    {
                        #region SUB_TOT_SLIP1(GoSub)

                        nRowInx += 1;
                        if (ssList_Sheet1.Rows.Count < nRowInx)
                        {
                            ssList_Sheet1.Rows.Count = nRowInx;
                        }

                        ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                        strAmt1 = String.Format("{0:#,##0}", nStot1);
                        strAmt2 = String.Format("{0:#,##0}", nStot2);

                        ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "누적별계";
                        ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                        ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                        nStot1 = 0;
                        nStot2 = 0;

                        #endregion SUB_TOT_SLIP1(GoSub) End

                        nNuChk = nNu;

                        if (dt.Rows[i]["GBSELF"].ToString().Trim() == "2")
                        {
                            strNujuk = "전액본인부담";
                        }
                        else
                        {
                            strNujuk = clsPmpaPb.GstrSETNus[nNu - 1];
                        }
                    }

                    if (nNu == 6) //마취 합산분 풀어서 Display
                    {
                        #region DATA_MARCHI_MOVE_SLIP1(GoSub)
                        if (nInitNo != 66)
                        {
                            continue;
                        }

                        nInitNo = 88;
                        

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT";
                        SQL += ComNum.VBLF + "  Sucode Sucode1, I.Sunext Sunext1, SunameK SunameK1, Hcode Hcode1, BaseAmt BaseAmt1, Qty Qty1,SUGBp,I.GBSUGBS,";
                        SQL += ComNum.VBLF + "  GbSpc GbSpc1, GbNgt GbNgt1, GbGisul GbGisul1, GbSelf GbSelf1, GbChild GbChild1, Nu Nu1,";
                        SQL += ComNum.VBLF + "  Nal Nalsu1, Amt1 Amtt11, Amt2 Amtt21";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND Pano = '" + txtPano.Text + "' ";
                        if (strBi != "00")
                        {
                            SQL += ComNum.VBLF + "      AND Bi   = '" + strBi + "' ";
                        }
                        if (strDEPTCODE != "00")
                        {
                            SQL += ComNum.VBLF + "      AND DeptCode   = '" + strDEPTCODE + "' ";
                        }
                        SQL += ComNum.VBLF + "      AND Nu   = '06' ";
                        SQL += ComNum.VBLF + "      AND Bdate >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND Bdate <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND I.Sunext = B.Sunext";
                        SQL += ComNum.VBLF + "ORDER BY Nu, Sucode, I.Sunext";

                        try
                        {
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count == 0)
                            {
                                dt.Dispose();
                                dt = null;
                                ComFunc.MsgBox("return");
                                return;
                            }

                            nCnt2 = dt1.Rows.Count;

                            for (j = 0; j <= (nCnt2 - 1); j++)
                            {
                                #region DATA_MOVE_SLIP1_2(GoSub)

                                nRowInx += 1;

                                if (ssList_Sheet1.Rows.Count < nRowInx)
                                {
                                    ssList_Sheet1.Rows.Count = nRowInx;
                                }

                                if (j > 0)
                                {
                                    strNujuk = "          ";
                                }

                                nAmt1 = Convert.ToInt32(dt1.Rows[j]["Amtt11"].ToString().Trim());
                                nAmt2 = Convert.ToInt32(dt1.Rows[j]["Amtt21"].ToString().Trim());

                                nStot1 += nAmt1;
                                nStot2 += nAmt2;
                                nGtot1 += nAmt1;
                                nGtot2 += nAmt2;

                                strBaseAmt = String.Format("{0:##,###,##0}", dt1.Rows[j]["BaseAmt1"].ToString().Trim());
                                strQty = String.Format("{0:#0.0}", dt1.Rows[j]["Qty1"].ToString().Trim());
                                strNal = String.Format("{0:##0}", dt1.Rows[j]["Nalsu1"].ToString().Trim());
                                strAmt1 = String.Format("{0:###,###,##0}", nAmt1);
                                strAmt2 = String.Format("{0:###,###,##0}", nAmt2);

                                ssList_Sheet1.Cells[nRowInx - 1, 0].Text = strNujuk;
                                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = dt1.Rows[j]["Sucode1"].ToString().Trim();
                                ssList_Sheet1.Cells[nRowInx - 1, 2].Text = dt1.Rows[j]["Hcode1"].ToString().Trim();
                                ssList_Sheet1.Cells[nRowInx - 1, 3].Text = dt1.Rows[j]["SunameK1"].ToString().Trim();
                                ssList_Sheet1.Cells[nRowInx - 1, 4].Text = strBaseAmt;
                                ssList_Sheet1.Cells[nRowInx - 1, 5].Text = strQty;
                                ssList_Sheet1.Cells[nRowInx - 1, 6].Text = strNal;
                                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;

                                //2014-07-11
                                if (i <= dt1.Rows.Count)
                                {
                                    ssList_Sheet1.Cells[nRowInx - 1, 9].Text = dt1.Rows[i]["GbSpc"].ToString().Trim();
                                    ssList_Sheet1.Cells[nRowInx - 1, 10].Text = dt1.Rows[i]["GbNgt"].ToString().Trim();
                                    ssList_Sheet1.Cells[nRowInx - 1, 11].Text = dt1.Rows[i]["GbGisul"].ToString().Trim();
                                    ssList_Sheet1.Cells[nRowInx - 1, 12].Text = dt1.Rows[i]["GbSelf"].ToString().Trim();
                                }


                                ssList_Sheet1.Cells[nRowInx - 1, 13].Text = dt1.Rows[j]["SUGBp"].ToString().Trim();
                                ssList_Sheet1.Cells[nRowInx - 1, 14].Text = dt1.Rows[j]["GBSUGBS"].ToString().Trim();

                                if (CF.READ_BAS_Sun_S(clsDB.DbCon, dt1.Rows[j]["Sucode1"].ToString().Trim()) == "6" || CF.READ_BAS_Sun_S(clsDB.DbCon, dt1.Rows[j]["Sucode1"].ToString().Trim()) == "7")
                                {
                                    ssList_Sheet1.Rows[i].BackColor = Color.LightGreen;
                                }
                                else
                                {
                                    ssList_Sheet1.Rows[i].BackColor = Color.White;
                                }


                                #endregion DATA_MOVE_SLIP1_2(GoSub) End
                            }
                        }

                        catch (Exception ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        }

                        dt1.Dispose();
                        dt1 = null;
                        #endregion DATA_MARCHI_MOVE_SLIP1(GoSub) End
                    }
                    else
                    {
                        #region DATA_MOVE_SLIP1(GoSub)
                        nRowInx += 1;

                        if (ssList_Sheet1.Rows.Count < nRowInx)
                        {
                            ssList_Sheet1.Rows.Count = nRowInx;
                        }

                        nAmt1 = Convert.ToInt32(dt.Rows[i]["Amtt1"].ToString().Trim());
                        nAmt2 = Convert.ToInt32(dt.Rows[i]["Amtt2"].ToString().Trim());

                        nStot1 += nAmt1;
                        nStot2 += nAmt2;
                        nGtot1 += nAmt1;
                        nGtot2 += nAmt2;

                        strBaseAmt = String.Format("{0:##,###,##0}", dt.Rows[i]["BaseAmt"].ToString().Trim());
                        strQty = String.Format("{0:#0.0}", dt.Rows[i]["Qty"].ToString().Trim());
                        strNal = String.Format("{0:##0}", dt.Rows[i]["Nalsu"].ToString().Trim());
                        strAmt1 = String.Format("{0:###,###,##0}", nAmt1);
                        strAmt2 = String.Format("{0:###,###,##0}", nAmt2);

                        ssList_Sheet1.Cells[nRowInx - 1, 0].Text = strNujuk;
                        ssList_Sheet1.Cells[nRowInx - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 2].Text = dt.Rows[i]["Bcode"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 4].Text = strBaseAmt;
                        ssList_Sheet1.Cells[nRowInx - 1, 5].Text = strQty;
                        ssList_Sheet1.Cells[nRowInx - 1, 6].Text = strNal;
                        ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                        ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;

                        ssList_Sheet1.Cells[nRowInx - 1, 9].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 10].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 11].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 12].Text = dt.Rows[i]["GbSelf"].ToString().Trim();

                        ssList_Sheet1.Cells[nRowInx - 1, 13].Text = dt.Rows[i]["SUGBp"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 14].Text = dt.Rows[i]["GBSUGBS"].ToString().Trim();

                        if (CF.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim()) == "6" || CF.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim()) == "7")
                        {
                            ssList_Sheet1.Rows[nRowInx - 1].BackColor = Color.LightGreen;
                        }
                        else
                        {
                            ssList_Sheet1.Rows[nRowInx - 1].BackColor = Color.White;
                        }

                        #endregion DATA_MOVE_SLIP1(GoSub) End
                    }
                }
                Application.DoEvents();
                strNujuk = "";
            }
            if (nGetcount > 0)
            {
                #region SUB_TOT_SLIP1(GoSub)

                nRowInx += 1;
                if (ssList_Sheet1.Rows.Count < nRowInx)
                {
                    ssList_Sheet1.Rows.Count = nRowInx;
                }

                ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                strAmt1 = String.Format("{0:###,###,##0}", nStot1);
                strAmt2 = String.Format("{0:###,###,##0}", nStot2);

                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "누적별계";
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                nStot1 = 0;
                nStot2 = 0;

                #endregion SUB_TOT_SLIP1(GoSub) End
            }

            if (nGetcount > 0)
            {
                #region GRAND_TOT_SLIP1(GoSub)

                nRowInx += 1;
                if (ssList_Sheet1.Rows.Count < nRowInx)
                {
                    ssList_Sheet1.Rows.Count = nRowInx;
                }

                ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                strAmt1 = String.Format("{0:###,###,##0}", nGtot1);
                strAmt2 = String.Format("{0:###,###,##0}", nGtot2);

                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "전체합계";
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                nGtot1 = 0;
                nGtot2 = 0;

                #endregion GRAND_TOT_SLIP1(GoSub) End

                btnPrint.Enabled = true;

            }

            dt.Dispose();
            dt = null;
            #endregion SQL_MAIN_SLIP1(GoSub) End
        }

        void Read_OPD_SLIP2()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            string strBday = "";
            string strBdaySW = "";
            string strBdayPrint = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAmt1 = "";
            string strAmt2 = "";

            int i = 0;
            int j = 0;
            int nRowInx = 0;

            int nAmt1 = 0;
            int nAmt2 = 0;
            int nStot1 = 0;
            int nStot2 = 0;
            int nGtot1 = 0;
            int nGtot2 = 0;

            #region SQL_SET_SLIP2(GoSub)

            SQL = "";
            SQL += ComNum.VBLF + "SELECT    ";
            SQL += ComNum.VBLF + "  TO_CHAR(Bdate, 'yy-mm-dd') Bday,";
            SQL += ComNum.VBLF + "  Nu,Sucode,SunameK,Hcode Hcode1,BaseAmt,Qty,";
            // 2015-07-06 수납조 제외
            if (chkMinus.Checked == true)
            {
                SQL += ComNum.VBLF + "  GbSpc,GbNgt,GbGisul,GbSelf,GbChild,Amt1 Amtt1,Amt2 Amtt2,Nal SNal";
            }
            else
            {
                SQL += ComNum.VBLF + "  GbSpc,GbNgt,GbGisul,GbSelf,GbChild,Sum(Amt1) Amtt1,Sum(Amt2) Amtt2,Sum(Nal) SNal ";
            }

            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Pano = '" + txtPano.Text + "'";
            if (strBi != "00")
            {
                SQL += ComNum.VBLF + "      AND Bi   = '" + strBi + "' ";
            }
            if (strDEPTCODE != "00")
            {
                SQL += ComNum.VBLF + "      AND DeptCode = '" + strDEPTCODE + "' ";
            }
            SQL += ComNum.VBLF + "  AND Nu >= '" + strFnu + "'  ";
            SQL += ComNum.VBLF + "  AND Nu <= '" + strTnu + "'  ";
            SQL += ComNum.VBLF + "  AND Bdate >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND Bdate <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND I.Sunext = B.Sunext";
            SQL += ComNum.VBLF + "  AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";  //저가약제 제외코드 2011-04-09
            if (chkMinus.Checked == false)
            {
                //2015-07-06
                SQL += ComNum.VBLF + "GROUP BY Bdate,Nu,Sucode,SunameK,Hcode,BaseAmt,Qty,GbSpc,GbNgt,GbGisul,GbSelf,GbChild ";
                SQL += ComNum.VBLF + "Having Sum(Nal) <> 0";
            }

            if (DateTime.Now.ToString("yyyy-MM-dd") == dtpTdate.Text && chkJin.Checked == true) // 진찰료포함
            {
                SQL += ComNum.VBLF + "UNION ALL     ";
                SQL += ComNum.VBLF + "  SELECT  TO_CHAR(Bdate, 'yy-mm-dd') Bday,'01' Nu, ";
                SQL += ComNum.VBLF + "  decode(chojae,'1','AA176','2','AA1761','3','AA276','4','AA2761','5','AA1762','6','AA2762')  Sucode,";

                if (chkMinus.Checked == true)
                {
                    SQL += ComNum.VBLF + "  '진찰료' SunameK,'' HCode1,AMT1 BASEAMT  , 1 Qty,";
                }
                else
                {
                    SQL += ComNum.VBLF + "  '진찰료' SunameK,'' HCode1,AMT1 BASEAMT  , COUNT(Pano) Qty,";
                }
                SQL += ComNum.VBLF + "   '' GbSpc, '' GbNgt,'' GbGisul, '1' GbSelf, '' GbChild,  ";
                //2015-07-06 수납조 제외
                if (chkMinus.Checked == true)
                {
                    SQL += ComNum.VBLF + "   Amt1 Amtt1,0 as Amtt2, 1 Nal   ";
                }
                else
                {
                    SQL += ComNum.VBLF + "   SUM(Amt1) Amtt1,0 as Amtt2, 1 Nal   ";
                }

                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND PANO ='" + txtPano.Text + "'  ";
                SQL += ComNum.VBLF + "      AND ACTDATE =trunc(sysdate)";
                if (strBi != "00")
                {
                    SQL += ComNum.VBLF + "      AND Bi   = '" + strBi + "' ";
                }
                if (strDEPTCODE != "00")
                {
                    SQL += ComNum.VBLF + "      AND DeptCode = '" + strDEPTCODE + "'  ";
                }

                if (chkMinus.Checked == false)
                {
                    SQL += ComNum.VBLF + "GROUP by Bdate,2,decode(chojae,'1','AA176','2','AA1761','3','AA276','4','AA2761','5','AA1762','6','AA2762') ,4,5,AMT1,8,9,10,11,12,13,15";
                }

                SQL += ComNum.VBLF + "ORDER BY 1, 2, 3";
            }
            else
            {
                SQL += ComNum.VBLF + "ORDER BY Bdate, Nu, Sucode";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            #endregion SQL_SET_SLIP2(GoSub) End

            #region SQL_MAIN_SLIP2(GoSub)
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRowInx = 0;
            strBdaySW = "";

            nGetcount += dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strBday = dt.Rows[i]["Bday"].ToString().Trim();

                if (strBdaySW == "")
                {
                    strBdayPrint = strBday;
                    strBdaySW = strBday;
                }

                if (strBday != strBdaySW)
                {
                    #region SUB_TOT_SLIP2(GoSub)

                    nRowInx += 1;
                    if (ssList_Sheet1.Rows.Count < nRowInx)
                    {
                        ssList_Sheet1.Rows.Count = nRowInx;
                    }

                    ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;


                    strAmt1 = String.Format("{0:###,###,##0}", nStot1);
                    strAmt2 = String.Format("{0:###,###,##0}", nStot2);

                    ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "누적별계";
                    ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                    ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                    nStot1 = 0;
                    nStot2 = 0;

                    #endregion SUB_TOT_SLIP2(GoSub) End

                    strBdaySW = strBday;
                    strBdayPrint = strBday;
                }

                #region DATA_MOVE_SLIP2(GoSub)
                nRowInx += 1;

                if (ssList_Sheet1.Rows.Count < nRowInx)
                {
                    ssList_Sheet1.Rows.Count = nRowInx;
                }

                nAmt1 = Convert.ToInt32(dt.Rows[i]["Amtt1"].ToString().Trim());
                nAmt2 = Convert.ToInt32(dt.Rows[i]["Amtt2"].ToString().Trim());

                nStot1 += nAmt1;
                nStot2 += nAmt2;
                nGtot1 += nAmt1;
                nGtot2 += nAmt2;

                strBaseAmt = string.Format("{0:##,###,##0}", dt.Rows[i]["BaseAmt"].ToString().Trim());
                strQty = string.Format("{0:#0.0}", dt.Rows[i]["Qty"].ToString().Trim());
                strNal = string.Format("{0:##0}", dt.Rows[i]["SNal"].ToString().Trim());
                strAmt1 = string.Format("{0:###,###,##0}", nAmt1);
                strAmt2 = string.Format("{0:###,###,##0}", nAmt2);

                ssList_Sheet1.Cells[nRowInx - 1, 0].Text = strBdayPrint;
                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                ssList_Sheet1.Cells[nRowInx - 1, 2].Text = dt.Rows[i]["Hcode1"].ToString().Trim();
                ssList_Sheet1.Cells[nRowInx - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                ssList_Sheet1.Cells[nRowInx - 1, 4].Text = strBaseAmt;
                ssList_Sheet1.Cells[nRowInx - 1, 5].Text = strQty;
                ssList_Sheet1.Cells[nRowInx - 1, 6].Text = strNal;
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;

                #endregion DATA_MOVE_SLIP2(GoSub) End
                strBdayPrint = "";
            }

            if (nGetcount > 0)
            {
                #region SUB_TOT_SLIP2(GoSub)

                nRowInx += 1;
                if (ssList_Sheet1.Rows.Count < nRowInx)
                {
                    ssList_Sheet1.Rows.Count = nRowInx;
                }

                ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                strAmt1 = String.Format("{0:###,###,##0}", nStot1);
                strAmt2 = String.Format("{0:###,###,##0}", nStot2);

                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "일별소계";
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                nStot1 = 0;
                nStot2 = 0;

                #endregion SUB_TOT_SLIP2(GoSub) End

                #region GRAND_TOT_SLIP2(GoSub)

                nRowInx += 1;
                if (ssList_Sheet1.Rows.Count < nRowInx)
                {
                    ssList_Sheet1.Rows.Count = nRowInx;
                }

                ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                strAmt1 = String.Format("{0:###,###,##0}", nGtot1);
                strAmt2 = String.Format("{0:###,###,##0}", nGtot2);

                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "전체합계";
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                nGtot1 = 0;
                nGtot2 = 0;

                # endregion GRAND_TOT_SLIP2(GoSub) End

                btnPrint.Enabled = true;

                ssList.Enabled = true;

                #endregion SQL_MAIN_SLIP2(GoSub) End
            }
        }

        void Read_OPD_SLIP3()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAmt1 = "";
            string strAmt2 = "";
            string strCFdate = "";
            string strBDate = "";

            int nNu = 0;
            int nNuChk = 0;
            int i = 0;
            int nRowInx = 0;

            int nAmt1 = 0;
            int nAmt2 = 0;
            int nStot1 = 0;
            int nStot2 = 0;
            int nGtot1 = 0;
            int nGtot2 = 0;


            #region SQL_SET_SLIP3(GoSub)

            SQL = "";
            SQL += ComNum.VBLF + "SELECT    ";
            SQL += ComNum.VBLF + "  TO_CHAR(Bdate, 'yy-mm-dd') Bday,    ";
            SQL += ComNum.VBLF + "  Nu,Sucode,I.SuNext,SunameK,BaseAmt,Qty,Nal,";
            SQL += ComNum.VBLF + "  GbSpc,GbNgt,GbGisul,GbSelf,GbChild,Amt1,Amt2,Part";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Pano = '" + txtPano.Text + "'";
            if (strBi != "00")
            {
                SQL += ComNum.VBLF + "      AND Bi   = '" + strBi + "' ";
            }
            if (strDEPTCODE != "00")
            {
                SQL += ComNum.VBLF + "      AND DeptCode = '" + strDEPTCODE + "' ";
            }
            SQL += ComNum.VBLF + "  AND Nu >= '" + strFnu + "'  ";
            SQL += ComNum.VBLF + "  AND Nu <= '" + strTnu + "'  ";
            SQL += ComNum.VBLF + "  AND Bdate >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND Bdate <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND I.Sunext = B.Sunext";
            SQL += ComNum.VBLF + "  AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";  //저가약제 제외코드 2011-04-09

            if (DateTime.Now.ToString("yyyy-MM-dd") == dtpTdate.Text && chkJin.Checked == true) // 진찰료포함
            {
                SQL += ComNum.VBLF + "UNION ALL     ";
                SQL += ComNum.VBLF + "  SELECT  TO_CHAR(Bdate, 'yy-mm-dd') Bday,'01' Nu, ";
                SQL += ComNum.VBLF + "  decode(chojae,'1','AA176','2','AA1761','3','AA276','4','AA2761','5','AA1762','6','AA2762')  Sucode,";
                SQL += ComNum.VBLF + "  decode(chojae,'1','AA176','2','AA1761','3','AA276','4','AA2761','5','AA1762','6','AA2762')  SuNext,";
                SQL += ComNum.VBLF + "  '진찰료' SunameK,AMT1 BASEAMT  , 1 Qty, 1 Nal,    ";
                SQL += ComNum.VBLF + "  '' GbSpc, '' GbNgt,'' GbGisul, '1' GbSelf, '' GbChild,  ";
                SQL += ComNum.VBLF + "   Amt1 Amtt1,0 as Amtt2,Part ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND PANO ='" + txtPano.Text + "'  ";
                SQL += ComNum.VBLF + "      AND ACTDATE =trunc(sysdate)";
                if (strBi != "00")
                {
                    SQL += ComNum.VBLF + "      AND Bi   = '" + strBi + "' ";
                }
                if (strDEPTCODE != "00")
                {
                    SQL += ComNum.VBLF + "      AND DeptCode = '" + strDEPTCODE + "'  ";
                }

                SQL += ComNum.VBLF + "ORDER BY 3, 1, 4";
            }

            else
            {
                SQL += ComNum.VBLF + "ORDER BY Nu, Bdate, Sucode, I.Sunext";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            #endregion SQL_SET_SLIP3(GoSub) End

            #region SQL_MAIN_SLIP3(GoSub)
            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            strCFdate = "";
            nRowInx = 0;


            nGetcount += dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nNu = Convert.ToInt32(dt.Rows[i]["Nu"].ToString().Trim());


                if (nNuChk == 0)
                {
                    nNuChk = nNu;
                    strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                    strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                    if (dt.Rows[i]["GBSELF"].ToString().Trim() == "2")
                    {
                        strNujuk = "전액본인부담";
                    }
                    else
                    {
                        strNujuk = clsPmpaPb.GstrSETNus[nNu - 1];
                    }
                }

                if (nNu != nNuChk)
                {
                    #region SUB_TOT_SLIP3(GoSub)

                    nRowInx += 1;
                    if (ssList_Sheet1.Rows.Count < nRowInx)
                    {
                        ssList_Sheet1.Rows.Count = nRowInx;
                    }

                    ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                    strAmt1 = String.Format("{0:###,###,##0}", nStot1);
                    strAmt2 = String.Format("{0:###,###,##0}", nStot2);

                    ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "누적별계";
                    ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                    ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                    nStot1 = 0;
                    nStot2 = 0;

                    #endregion SUB_TOT_SLIP3(GoSub) End

                    nNuChk = nNu;
                    strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                    strBDate = dt.Rows[i]["Bday"].ToString().Trim();

                    if (dt.Rows[i]["GBSELF"].ToString().Trim() == "2")
                    {
                        strNujuk = "전액본인부담";
                    }
                    else
                    {
                        strNujuk = clsPmpaPb.GstrSETNus[nNu - 1];
                    }

                    if (strCFdate != dt.Rows[i]["Bday"].ToString().Trim())
                    {
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                    }
                }

                #region DATA_MOVE_SLIP3(GoSub)
                nRowInx += 1;

                if (ssList_Sheet1.Rows.Count < nRowInx)
                {
                    ssList_Sheet1.Rows.Count = nRowInx;
                }

                nAmt1 = Convert.ToInt32(dt.Rows[i]["Amt1"].ToString().Trim());
                nAmt2 = Convert.ToInt32(dt.Rows[i]["Amt2"].ToString().Trim());

                nStot1 += nAmt1;
                nStot2 += nAmt2;
                nGtot1 += nAmt1;
                nGtot2 += nAmt2;

                strBaseAmt = String.Format("{0:#,##0}", dt.Rows[i]["BaseAmt"].ToString().Trim());
                strQty = String.Format("{0:#0.0}", dt.Rows[i]["Qty"].ToString().Trim());
                strNal = String.Format("{0:##0}", dt.Rows[i]["Nal"].ToString().Trim());
                strAmt1 = String.Format("{0:#,##0}", nAmt1);
                strAmt2 = String.Format("{0:#,##0}", nAmt2);

                ssList_Sheet1.Cells[nRowInx - 1, 0].Text = strNujuk;
                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = strBDate;
                ssList_Sheet1.Cells[nRowInx - 1, 2].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                ssList_Sheet1.Cells[nRowInx - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                ssList_Sheet1.Cells[nRowInx - 1, 4].Text = strBaseAmt;
                ssList_Sheet1.Cells[nRowInx - 1, 5].Text = strQty;
                ssList_Sheet1.Cells[nRowInx - 1, 6].Text = strNal;
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;

                #endregion DATA_MOVE_SLIP3(GoSub) End
                strNujuk = "";
                strBDate = "";
            }

            if (nGetcount > 0)
            {
                #region SUB_TOT_SLIP3(GoSub)

                nRowInx += 1;
                if (ssList_Sheet1.Rows.Count < nRowInx)
                {
                    ssList_Sheet1.Rows.Count = nRowInx;
                }

                ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                strAmt1 = String.Format("{0:###,###,##0}", nStot1);
                strAmt2 = String.Format("{0:###,###,##0}", nStot2);

                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "누적별계";
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                nStot1 = 0;
                nStot2 = 0;

                #endregion SUB_TOT_SLIP3(GoSub) End

                #region GRAND_TOT_SLIP3(GoSub)

                nRowInx += 1;
                if (ssList_Sheet1.Rows.Count < nRowInx)
                {
                    ssList_Sheet1.Rows.Count = nRowInx;
                }

                ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                strAmt1 = String.Format("{0:#,##0}", nGtot1);
                strAmt2 = String.Format("{0:#,##0}", nGtot2);

                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "전체합계";
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                nGtot1 = 0;
                nGtot2 = 0;

                # endregion GRAND_TOT_SLIP3(GoSub) End

                btnPrint.Enabled = true;

                ssList.Enabled = true;

                #endregion SQL_MAIN_SLIP3(GoSub) End
            }
        }

        void Read_OPD_SLIP4()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAmt1 = "";
            string strAmt2 = "";

            int nNu = 0;
            int nNuChk = 0;
            int i = 0;
            int j = 0;
            int nRowInx = 0;
            int nCnt2 = 0;
            int nInitNo = 0;

            int nAmt1 = 0;
            int nAmt2 = 0;
            int nStot1 = 0;
            int nStot2 = 0;
            int nGtot1 = 0;
            int nGtot2 = 0;
            int nNalSu = 0;
            double nAmt = 0;

            nInitNo = 66;

            #region SQL_SET_SLIP1(GoSub)

            SQL = "";
            SQL += ComNum.VBLF + "SELECT    ";
            SQL += ComNum.VBLF + "  Sucode, I.Sunext, SunameK, Hcode, BaseAmt, Qty,";
            SQL += ComNum.VBLF + "  GbSpc, GbNgt, GbGisul, GbSelf, GbChild, Nu,BUN,";
            SQL += ComNum.VBLF + "  SUM(Nal) Nalsu, SUM(Amt1) Amtt1, SUM(Amt2) Amtt2";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Pano = '" + txtPano.Text + "'";
            if (strBi != "00")
            {
                SQL += ComNum.VBLF + "      AND Bi   = '" + strBi + "' ";
            }
            if (strDEPTCODE != "00")
            {
                SQL += ComNum.VBLF + "      AND DeptCode = '" + strDEPTCODE + "' ";
            }
            SQL += ComNum.VBLF + "  AND Nu BETWEEN '" + strFnu + "' ";
            SQL += ComNum.VBLF + "  AND '" + strTnu + "' ";
            SQL += ComNum.VBLF + "  AND I.Sunext = B.Sunext";
            SQL += ComNum.VBLF + "  AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";  //저가약제 제외코드 2011-04-09
            SQL += ComNum.VBLF + "  AND Bdate BETWEEN TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "GROUP BY Sucode, I.Sunext, SunameK, Hcode, BaseAmt, Qty,";
            SQL += ComNum.VBLF + "GbSpc, GbNgt, GbGisul, GbSelf, GbChild, Nu, BUN";

            if (DateTime.Now.ToString("yyyy-MM-dd") == dtpTdate.Text && chkJin.Checked == true)  // 진찰료 포함
            {
                SQL += ComNum.VBLF + "UNION ALL";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  decode(chojae,'1','AA176','2','AA1761','3','AA276','4','AA2761','5','AA1762','6','AA2762')  Sucode,";
                SQL += ComNum.VBLF + "  '' Sunext,'진찰료' SunameK,'' Hcode,AMT1 BASEAMT  , COUNT(Pano) Qty,";
                SQL += ComNum.VBLF + "  '' GbSpc, '' GbNgt,'' GbGisul, '1' GbSelf, '' GbChild, '01' Nu, '01' BUN,";
                SQL += ComNum.VBLF + "  1 Nalsu, SUM(Amt1) Amtt1,0 as Amtt2 ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND PANO ='" + txtPano.Text + "'  ";
                SQL += ComNum.VBLF + "      AND ACTDATE =trunc(sysdate)";
                if (strBi != "00")
                {
                    SQL += ComNum.VBLF + "      AND Bi   = '" + strBi + "' ";
                }
                if (strDEPTCODE != "00")
                {
                    SQL += ComNum.VBLF + "      AND DeptCode = '" + strDEPTCODE + "'  ";
                }
                SQL += ComNum.VBLF + "GROUP by decode(chojae,'1','AA176','2','AA1761','3','AA276','4','AA2761','5','AA1762','6','AA2762') ,2,3,4,amt1,6,7,8,9,10,11,12";
                SQL += ComNum.VBLF + "ORDER BY 12, 1, 2";
            }
            else
            {
                SQL += ComNum.VBLF + "ORDER BY BUN, Nu, ";
                SQL += ComNum.VBLF + "         DECODE(Qty, .1,1,.2,2,.3,3,.4,4,.5,5,.6,6,.7,7,.8,8,.9,9,0),";
                SQL += ComNum.VBLF + "         Sucode, I.Sunext";
            }
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            #endregion SQL_SET_SLIP1(GoSub) End
            ///////////////////////////////////////////////////
            #region SQL_MAIN_SLIP1(GoSub)
            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRowInx = 0;

            if (dt.Rows.Count == 0)
            {
                btnPrint.Enabled = true;
                ssList.Enabled = true;
                ComFunc.MsgBox("해당조건의 데이터는 없습니다.");
                dt.Dispose();
                dt = null;
                return;
            }

            nGetcount += dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nNalSu = Convert.ToInt16(dt.Rows[i]["NalSu"].ToString().Trim());
                nAmt = Convert.ToDouble(dt.Rows[i]["Amtt1"].ToString().Trim());

                switch (dt.Rows[i]["BUN"].ToString().Trim())
                {
                    case "01":
                    case "02":
                        nNu = 1;    //진찰료
                        break;

                    case "03":
                    case "05":
                    case "07":
                    case "09":
                        nNu = 2;    //입원실료
                        break;

                    case "04":
                    case "06":
                    case "08":
                    case "10":
                        nNu = 3;    //환자관리
                        break;

                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "15":
                        nNu = 4;
                        break;

                    case "16":
                    case "17":
                    case "18":
                    case "19":
                    case "20":
                    case "21":
                        //주사료중에 마취용 누적분류는 마취누적으로 분류
                        if (dt.Rows[i]["Nu"].ToString().Trim() == "6")
                        {
                            nNu = 6;
                        }
                        else
                        {
                            nNu = 5;
                        }
                        break;

                    case "22":
                    case "23":
                        nNu = 6;
                        break;

                    case "24":
                    case "25":
                        nNu = 7;
                        break;

                    case "26":
                    case "27":
                        nNu = 8;
                        break;

                    case "28":
                    case "29":
                    case "30":
                    case "31":
                    case "32":
                    case "33":
                        nNu = 9;
                        break;

                    case "34":
                    case "35":
                    case "36":
                        nNu = 10;
                        break;

                    case "37":
                        nNu = 11;
                        break;

                    case "38":
                    case "39":
                        nNu = 12;
                        break;

                    case "40":
                        nNu = 40;
                        break;

                    case "41":
                    case "42":
                    case "43":
                    case "44":
                    case "45":
                    case "46":
                    case "47":
                    case "48":
                    case "49":
                    case "50":
                    case "51":
                        nNu = 13;
                        break;

                    case "52":
                    case "53":
                    case "54":
                    case "55":
                    case "56":
                    case "57":
                    case "58":
                    case "59":
                    case "60":
                    case "61":
                    case "62":
                    case "63":
                    case "64":
                        nNu = 14;
                        break;

                    case "65":
                    case "66":
                    case "67":
                    case "68":
                    case "69":
                    case "70":
                        nNu = 15;
                        break;

                    case "71":
                        nNu = 36;
                        break;

                    case "72":
                        nNu = 37;
                        break;

                    case "73":
                        nNu = 38;
                        break;

                    case "74":
                        nNu = 34;
                        break;

                    case "75":
                        nNu = 47;
                        break;

                    case "76":
                        nNu = 46;
                        break;

                    case "77":
                        nNu = 35;
                        break;

                    case "78":
                        nNu = 43;
                        break;

                    case "79":
                        nNu = 41;
                        break;

                    case "80":
                        nNu = 42;
                        break;

                    case "81":
                        nNu = 44;
                        break;

                    case "82":
                        nNu = 61;
                        break;

                    case "83":
                        nNu = 62;
                        break;

                    case "84":
                        nNu = 63;
                        break;

                    default:
                        nNu = 20;
                        break;
                }
                if (nNalSu != 0 || nAmt != 0)
                {
                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strNujuk = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_누적행위명", ComFunc.SetAutoZero(nNu.ToString(), 2));
                    }

                    if (nNu != nNuChk)
                    {
                        #region SUB_TOT_SLIP1(GoSub)

                        nRowInx += 1;
                        if (ssList_Sheet1.Rows.Count < nRowInx)
                        {
                            ssList_Sheet1.Rows.Count = nRowInx;
                        }

                        ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                        strAmt1 = nStot1.ToString("###,###,##0");
                        strAmt2 = nStot2.ToString("###,###,##0");

                        ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "누적별계";
                        ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                        ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                        nStot1 = 0;
                        nStot2 = 0;

                        #endregion SUB_TOT_SLIP1(GoSub) End
                        nNuChk = nNu;
                        strNujuk = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_누적행위명", ComFunc.SetAutoZero(nNu.ToString(), 2));
                    }

                    //마취 합산분 풀어서 Display
                    if (nNu == 6)
                    {
                        #region DATA_MARCHI_MOVE_SLIP1(GoSub)

                        if (nInitNo != 66)
                        {
                            continue;
                        }

                        nInitNo = 88;

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT";
                        SQL += ComNum.VBLF + "  Sucode Sucode1, I.Sunext Sunext1, SunameK SunameK1, Hcode Hcode1, BaseAmt BaseAmt1, Qty Qty1,";
                        SQL += ComNum.VBLF + "  GbSpc GbSpc1, GbNgt GbNgt1, GbGisul GbGisul1, GbSelf GbSelf1, GbChild GbChild1, Nu Nu1,";
                        SQL += ComNum.VBLF + "  Nal Nalsu1, Amt1 Amtt11, Amt2 Amtt21";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND Pano = '" + txtPano.Text + "' ";
                        if (strBi != "00")
                        {
                            SQL += ComNum.VBLF + "      AND Bi   = '" + strBi + "' ";
                        }
                        if (strDEPTCODE != "00")
                        {
                            SQL += ComNum.VBLF + "      AND DeptCode   = '" + strDEPTCODE + "' ";
                        }
                        SQL += ComNum.VBLF + "      AND Nu   = '06' ";
                        SQL += ComNum.VBLF + "      AND Bdate >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND Bdate <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND I.Sunext = B.Sunext";
                        SQL += ComNum.VBLF + "ORDER BY Nu, Sucode, I.Sunext";

                        try
                        {
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count == 0)
                            {
                                dt1.Dispose();
                                dt1 = null;
                                return;
                            }

                            nCnt2 = dt1.Rows.Count;

                            for (j = 0; j <= (nCnt2 - 1); j++)
                            {
                                #region DATA_MOVE_SLIP1_2(GoSub)
                                nRowInx += 1;

                                if (ssList_Sheet1.Rows.Count < nRowInx)
                                {
                                    ssList_Sheet1.Rows.Count = nRowInx;
                                }

                                if (j > 0)
                                {
                                    strNujuk = "          ";
                                }

                                nAmt1 = Convert.ToInt32(dt1.Rows[j]["Amtt11"].ToString().Trim());
                                nAmt2 = Convert.ToInt32(dt1.Rows[j]["Amtt21"].ToString().Trim());

                                nStot1 += nAmt1;
                                nStot2 += nAmt2;
                                nGtot1 += nAmt1;
                                nGtot2 += nAmt2;

                                strBaseAmt = String.Format("{0:##,###,##0}", dt1.Rows[j]["BaseAmt1"].ToString().Trim());
                                strQty = String.Format("{0:#0.0}", dt1.Rows[j]["Qty1"].ToString().Trim());
                                strNal = String.Format("{0:##0}", dt1.Rows[j]["Nalsu1"].ToString().Trim());
                                strAmt1 = String.Format("{0:###,###,##0}", nAmt1);
                                strAmt2 = String.Format("{0:###,###,##0}", nAmt2);

                                ssList_Sheet1.Cells[nRowInx - 1, 0].Text = strNujuk;
                                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = dt1.Rows[j]["Sucode1"].ToString().Trim();
                                ssList_Sheet1.Cells[nRowInx - 1, 2].Text = dt1.Rows[j]["Hcode1"].ToString().Trim();
                                ssList_Sheet1.Cells[nRowInx - 1, 3].Text = dt1.Rows[j]["SunameK1"].ToString().Trim();
                                ssList_Sheet1.Cells[nRowInx - 1, 4].Text = strBaseAmt;
                                ssList_Sheet1.Cells[nRowInx - 1, 5].Text = strQty;
                                ssList_Sheet1.Cells[nRowInx - 1, 6].Text = strNal;
                                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                                #endregion DATA_MOVE_SLIP1_2(GoSub) End
                            }
                        }

                        catch (Exception ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        }

                        dt1.Dispose();
                        dt1 = null;
                        #endregion DATA_MARCHI_MOVE_SLIP1(GoSub) End
                    }
                    else
                    {
                        #region DATA_MOVE_SLIP1(GoSub)

                        nRowInx += 1;

                        if (ssList_Sheet1.Rows.Count < nRowInx)
                        {
                            ssList_Sheet1.Rows.Count = nRowInx;
                        }

                        nAmt1 = Convert.ToInt32(dt.Rows[i]["Amtt1"].ToString().Trim());
                        nAmt2 = Convert.ToInt32(dt.Rows[i]["Amtt2"].ToString().Trim());

                        nStot1 += nAmt1;
                        nStot2 += nAmt2;
                        nGtot1 += nAmt1;
                        nGtot2 += nAmt2;

                        strBaseAmt = String.Format("{0:#,##0}", dt.Rows[i]["BaseAmt"].ToString().Trim());
                        strQty = String.Format("{0:#0.0}", dt.Rows[i]["Qty"].ToString().Trim());
                        strNal = String.Format("{0:##0}", dt.Rows[i]["Nalsu"].ToString().Trim());
                        strAmt1 = String.Format("{0:#,##0}", nAmt1);
                        strAmt2 = String.Format("{0:#,##0}", nAmt2);

                        ssList_Sheet1.Cells[nRowInx - 1, 0].Text = strNujuk;
                        ssList_Sheet1.Cells[nRowInx - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 2].Text = dt.Rows[i]["Hcode"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 4].Text = strBaseAmt;
                        ssList_Sheet1.Cells[nRowInx - 1, 5].Text = strQty;
                        ssList_Sheet1.Cells[nRowInx - 1, 6].Text = strNal;
                        ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                        ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;

                        #endregion DATA_MOVE_SLIP1(GoSub) End
                    }
                }

                strNujuk = "";
            }

            if (nGetcount > 0)
            {
                #region SUB_TOT_SLIP1(GoSub)

                nRowInx += 1;
                if (ssList_Sheet1.Rows.Count < nRowInx)
                {
                    ssList_Sheet1.Rows.Count = nRowInx;
                }

                ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                strAmt1 = String.Format("{0:#,##0}", nStot1);
                strAmt2 = String.Format("{0:#,##0}", nStot2);

                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "누적별계";
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                nStot1 = 0;
                nStot2 = 0;

                #endregion SUB_TOT_SLIP1(GoSub) End
            }

            if (nGetcount > 0)
            {
                #region GRAND_TOT_SLIP1(GoSub)

                nRowInx += 1;
                if (ssList_Sheet1.Rows.Count < nRowInx)
                {
                    ssList_Sheet1.Rows.Count = nRowInx;
                }

                ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                strAmt1 = String.Format("{0:###,###,##0}", nGtot1);
                strAmt2 = String.Format("{0:###,###,##0}", nGtot2);

                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "전체합계";
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                nGtot1 = 0;
                nGtot2 = 0;

                #endregion GRAND_TOT_SLIP1(GoSub) End

                btnPrint.Enabled = true;

            }

            dt.Dispose();
            dt = null;
            #endregion SQL_MAIN_SLIP1(GoSub) End
        }

        void Read_OPD_SLIP5()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAmt1 = "";
            string strAmt2 = "";

            int nNu = 0;
            int nNuChk = 0;
            int i = 0;
            int j = 0;
            int nRowInx = 0;
            int nCnt2 = 0;
            int nInitNo = 0;
            
            int nAmt1 = 0;
            int nAmt2 = 0;
            int nStot1 = 0;
            int nStot2 = 0;
            int nGtot1 = 0;
            int nGtot2 = 0;
            int nNalSu = 0;
            double nAmt = 0;

            nInitNo = 66;

            #region SQL_SET_SLIP1(GoSub)

            // 2009-10-26 윤조연 추가 = 당일진찰료 포함 루틴추가

            SQL = "";
            SQL += ComNum.VBLF + "SELECT    ";
            SQL += ComNum.VBLF + "  Sucode, I.Sunext, SunameK, B.Bcode, BaseAmt, Qty,SUGBp,I.GBSUGBS,";
            SQL += ComNum.VBLF + "  GbSpc, GbNgt, GbGisul, GbSelf, GbChild, Nu,";
            SQL += ComNum.VBLF + "  SUM(Nal) Nalsu, SUM(Amt1) Amtt1, SUM(Amt2) Amtt2 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Pano = '" + txtPano.Text + "'";
            if (strBi != "00")
            {
                SQL += ComNum.VBLF + "      AND Bi   = '" + strBi + "' ";
            }
            if (strDEPTCODE != "00")
            {
                SQL += ComNum.VBLF + "      AND DeptCode = '" + strDEPTCODE + "' ";
            }
            SQL += ComNum.VBLF + "  AND Nu BETWEEN '" + strFnu + "' ";
            SQL += ComNum.VBLF + "  AND '" + strTnu + "' ";
            SQL += ComNum.VBLF + "  AND I.Sunext = B.Sunext";
            SQL += ComNum.VBLF + "  AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";  //저가약제 제외코드 2011-04-09
            SQL += ComNum.VBLF + "  AND Bdate BETWEEN TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "GROUP BY Sucode, I.Sunext, SunameK, B.Bcode, BaseAmt, Qty,SUGBp,I.GBSUGBS,";
            SQL += ComNum.VBLF + "GbSpc, GbNgt, GbGisul, GbSelf, GbChild, Nu";

            if (DateTime.Now.ToString("yyyy-MM-dd") == dtpTdate.Text && chkJin.Checked == true)  // 진찰료 포함
            {
                SQL += ComNum.VBLF + "UNION ALL";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  decode(chojae,'1','AA176','2','AA1761','3','AA276','4','AA2761','5','AA1762','6','AA2762')  Sucode, ";
                SQL += ComNum.VBLF + "  decode(chojae,'1','AA176','2','AA1761','3','AA276','4','AA2761','5','AA1762','6','AA2762')  SuNext, ";
                SQL += ComNum.VBLF + "  '진찰료' SunameK,  ";
                SQL += ComNum.VBLF + "  decode(chojae,'1','AA176','2','AA156050','3','AA256','4','AA256010','5','AA156050','6','AA256050') Hcode,";
                SQL += ComNum.VBLF + "  AMT1 BASEAMT  , COUNT(Pano) Qty,'','', ";
                SQL += ComNum.VBLF + "  '' GbSpc, '' GbNgt,'' GbGisul, '1' GbSelf, '' GbChild, '01' Nu, ";
                SQL += ComNum.VBLF + "  1 Nalsu, SUM(Amt1) Amtt1,0 as Amtt2  ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND PANO ='" + txtPano.Text + "'  ";
                SQL += ComNum.VBLF + "      AND ACTDATE =trunc(sysdate)";
                if (strBi != "00")
                {
                    SQL += ComNum.VBLF + "      AND Bi   = '" + strBi + "' ";
                }
                if (strDEPTCODE != "00")
                {
                    SQL += ComNum.VBLF + "      AND DeptCode = '" + strDEPTCODE + "'  ";
                }
                SQL += ComNum.VBLF + "GROUP by decode(chojae,'1','AA176','2','AA1761','3','AA276','4','AA2761','5','AA1762','6','AA2762'), ";
                SQL += ComNum.VBLF + "         decode(chojae,'1','AA176','2','AA156050','3','AA256','4','AA256010','5','AA156050','6','AA256050'),";
                SQL += ComNum.VBLF + "         2,3,4,amt1,6,7,8,9,10,11,12   ";
                SQL += ComNum.VBLF + "ORDER BY 12, 1, 2";
            }
            else
            {
                SQL += ComNum.VBLF + "ORDER BY Nu, ";
                SQL += ComNum.VBLF + "         DECODE(Qty, .1,1,.2,2,.3,3,.4,4,.5,5,.6,6,.7,7,.8,8,.9,9,0), ";
                SQL += ComNum.VBLF + "         Sucode, I.Sunext";
            }
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            #endregion SQL_SET_SLIP1(GoSub) End

            #region SQL_MAIN_SLIP1(GoSub)
            nNuChk = 0;
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRowInx = 0;

            if (dt.Rows.Count == 0)
            {
                btnPrint.Enabled = true;
                ssList.Enabled = true;
                ComFunc.MsgBox("해당조건의 데이터는 없습니다.");
                dt.Dispose();
                dt = null;
                return;
            }

            nGetcount += dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nNalSu = Convert.ToInt16(dt.Rows[i]["NalSu"].ToString().Trim());
                nAmt = Convert.ToDouble(dt.Rows[i]["Amtt1"].ToString().Trim());

                if (nNalSu != 0 || nAmt != 0)
                {
                    nNu = Convert.ToInt32(dt.Rows[i]["Nu"].ToString().Trim());
                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strNujuk = clsPmpaPb.GstrSETNus[nNu - 1];
                    }

                    if (nNu != nNuChk)
                    {
                        #region SUB_TOT_SLIP1(GoSub)

                        nRowInx += 1;
                        if (ssList_Sheet1.Rows.Count < nRowInx)
                        {
                            ssList_Sheet1.Rows.Count = nRowInx;
                        }

                        ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                        strAmt1 = String.Format("{0:###,###,##0}", nStot1);
                        strAmt2 = String.Format("{0:###,###,##0}", nStot2);

                        ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "누적별계";
                        ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                        ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                        nStot1 = 0;
                        nStot2 = 0;

                        #endregion SUB_TOT_SLIP1(GoSub) End

                        nNuChk = nNu;
                        strNujuk = clsPmpaPb.GstrSETNus[nNu - 1];

                    }

                    if (nNu == 6) //마취 합산분 풀어서 Display
                    {
                        #region DATA_MARCHI_MOVE_SLIP1(GoSub)
                        if (nInitNo != 66)
                        {
                            continue;
                        }

                        nInitNo = 88;

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT";
                        SQL += ComNum.VBLF + "  Sucode Sucode1, I.Sunext Sunext1, SunameK SunameK1, b.BCode BCode1, BaseAmt BaseAmt1, Qty Qty1,SUGBp,I.GBSUGBS,";
                        SQL += ComNum.VBLF + "  GbSpc GbSpc1, GbNgt GbNgt1, GbGisul GbGisul1, GbSelf GbSelf1, GbChild GbChild1, Nu Nu1,";
                        SQL += ComNum.VBLF + "  Nal Nalsu1, Amt1 Amtt11, Amt2 Amtt21";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP I, " + ComNum.DB_PMPA + "BAS_SUN B";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND Pano = '" + txtPano.Text + "' ";
                        if (strBi != "00")
                        {
                            SQL += ComNum.VBLF + "      AND Bi   = '" + strBi + "' ";
                        }
                        if (strDEPTCODE != "00")
                        {
                            SQL += ComNum.VBLF + "      AND DeptCode   = '" + strDEPTCODE + "' ";
                        }
                        SQL += ComNum.VBLF + "      AND Nu   = '06' ";
                        SQL += ComNum.VBLF + "      AND Bdate >= TO_DATE('" + dtpFdate.Text + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND Bdate <= TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD')";
                        SQL += ComNum.VBLF + "      AND I.Sunext = B.Sunext";
                        SQL += ComNum.VBLF + "ORDER BY Nu, Sucode, I.Sunext";

                        try
                        {
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count == 0)
                            {
                                dt1.Dispose();
                                dt1 = null;
                                return;
                            }

                            nCnt2 = dt1.Rows.Count;

                            for (j = 0; j <= (nCnt2 - 1); j++)
                            {
                                #region DATA_MOVE_SLIP1_2(GoSub)

                                nRowInx += 1;

                                if (ssList_Sheet1.Rows.Count < nRowInx)
                                {
                                    ssList_Sheet1.Rows.Count = nRowInx;
                                }

                                if (j > 0)
                                {
                                    strNujuk = "          ";
                                }

                                nAmt1 = Convert.ToInt32(dt1.Rows[j]["Amtt11"].ToString().Trim());
                                nAmt2 = Convert.ToInt32(dt1.Rows[j]["Amtt21"].ToString().Trim());

                                nStot1 += nAmt1;
                                nStot2 += nAmt2;
                                nGtot1 += nAmt1;
                                nGtot2 += nAmt2;

                                strBaseAmt = String.Format("{0:#,##0}", dt1.Rows[j]["BaseAmt1"].ToString().Trim());
                                strQty = String.Format("{0:#0.0}", dt1.Rows[j]["Qty1"].ToString().Trim());
                                strNal = String.Format("{0:##0}", dt1.Rows[j]["Nalsu1"].ToString().Trim());
                                strAmt1 = String.Format("{0:###,###,##0}", nAmt1);
                                strAmt2 = String.Format("{0:###,###,##0}", nAmt2);

                                ssList_Sheet1.Cells[nRowInx - 1, 0].Text = strNujuk;
                                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = dt1.Rows[j]["Sucode1"].ToString().Trim();
                                ssList_Sheet1.Cells[nRowInx - 1, 2].Text = dt1.Rows[j]["BCode1"].ToString().Trim();
                                ssList_Sheet1.Cells[nRowInx - 1, 3].Text = dt1.Rows[j]["SunameK1"].ToString().Trim();
                                ssList_Sheet1.Cells[nRowInx - 1, 4].Text = strBaseAmt;
                                ssList_Sheet1.Cells[nRowInx - 1, 5].Text = strQty;
                                ssList_Sheet1.Cells[nRowInx - 1, 6].Text = strNal;
                                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;

                                //2014-07-11
                                if (i <= dt1.Rows.Count)
                                {
                                    ssList_Sheet1.Cells[nRowInx - 1, 9].Text = dt1.Rows[i]["GbSpc"].ToString().Trim();
                                    ssList_Sheet1.Cells[nRowInx - 1, 10].Text = dt1.Rows[i]["GbNgt"].ToString().Trim();
                                    ssList_Sheet1.Cells[nRowInx - 1, 11].Text = dt1.Rows[i]["GbGisul"].ToString().Trim();
                                    ssList_Sheet1.Cells[nRowInx - 1, 12].Text = dt1.Rows[i]["GbSelf"].ToString().Trim();
                                }

                                ssList_Sheet1.Cells[nRowInx - 1, 13].Text = dt1.Rows[j]["SUGBp"].ToString().Trim();
                                ssList_Sheet1.Cells[nRowInx - 1, 14].Text = dt1.Rows[j]["GBSUGBS"].ToString().Trim();                                

                                if (CF.READ_BAS_Sun_S(clsDB.DbCon, dt1.Rows[j]["Sucode1"].ToString().Trim()) == "6" || CF.READ_BAS_Sun_S(clsDB.DbCon, dt1.Rows[j]["Sucode1"].ToString().Trim()) == "7")
                                {
                                    ssList_Sheet1.Rows[i].BackColor = Color.LightGreen;
                                }
                                else
                                {
                                    ssList_Sheet1.Rows[i].BackColor = Color.White;
                                }


                                #endregion DATA_MOVE_SLIP1_2(GoSub) End
                            }
                        }

                        catch (Exception ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        }

                        dt1.Dispose();
                        dt1 = null;
                        #endregion DATA_MARCHI_MOVE_SLIP1(GoSub) End 
                    }
                    else
                    {
                        #region DATA_MOVE_SLIP1(GoSub)
                        nRowInx += 1;

                        if (ssList_Sheet1.Rows.Count < nRowInx)
                        {
                            ssList_Sheet1.Rows.Count = nRowInx;
                        }

                        nAmt1 = Convert.ToInt32(dt.Rows[i]["Amtt1"].ToString().Trim());
                        nAmt2 = Convert.ToInt32(dt.Rows[i]["Amtt2"].ToString().Trim());

                        nStot1 += nAmt1;
                        nStot2 += nAmt2;
                        nGtot1 += nAmt1;
                        nGtot2 += nAmt2;

                        strBaseAmt = String.Format("{0:#,##0}", dt.Rows[i]["BaseAmt"].ToString().Trim());
                        strQty = String.Format("{0:#0.0}", dt.Rows[i]["Qty"].ToString().Trim());
                        strNal = String.Format("{0:##0}", dt.Rows[i]["Nalsu"].ToString().Trim());
                        strAmt1 = String.Format("{0:#,##0}", nAmt1);
                        strAmt2 = String.Format("{0:#,##0}", nAmt2);

                        ssList_Sheet1.Cells[nRowInx - 1, 0].Text = strNujuk;
                        ssList_Sheet1.Cells[nRowInx - 1, 1].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 2].Text = dt.Rows[i]["Bcode"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 4].Text = strBaseAmt;
                        ssList_Sheet1.Cells[nRowInx - 1, 5].Text = strQty;
                        ssList_Sheet1.Cells[nRowInx - 1, 6].Text = strNal;
                        ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                        ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;

                        // 2014-07-11
                        ssList_Sheet1.Cells[nRowInx - 1, 9].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 10].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 11].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 12].Text = dt.Rows[i]["GbSelf"].ToString().Trim();

                        ssList_Sheet1.Cells[nRowInx - 1, 13].Text = dt.Rows[i]["SUGBp"].ToString().Trim();
                        ssList_Sheet1.Cells[nRowInx - 1, 14].Text = dt.Rows[i]["GBSUGBS"].ToString().Trim();

                        if (CF.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim()) == "6" || CF.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim()) == "7")
                        {
                            ssList_Sheet1.Rows[i].BackColor = Color.LightGreen;
                        }
                        else
                        {
                            ssList_Sheet1.Rows[i].BackColor = Color.White;
                        }

                        #endregion DATA_MOVE_SLIP1(GoSub) End
                    }
                }

                strNujuk = "";
            }

            if (nGetcount > 0)
            {
                #region SUB_TOT_SLIP1(GoSub)

                nRowInx += 1;
                if (ssList_Sheet1.Rows.Count < nRowInx)
                {
                    ssList_Sheet1.Rows.Count = nRowInx;
                }

                ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                strAmt1 = String.Format("{0:###,###,##0}", nStot1);
                strAmt2 = String.Format("{0:###,###,##0}", nStot2);

                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "누적별계";
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                nStot1 = 0;
                nStot2 = 0;

                #endregion SUB_TOT_SLIP1(GoSub) End
            }

            if (nGetcount > 0)
            {
                #region GRAND_TOT_SLIP1(GoSub)

                nRowInx += 1;
                if (ssList_Sheet1.Rows.Count < nRowInx)
                {
                    ssList_Sheet1.Rows.Count = nRowInx;
                }

                ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;

                strAmt1 = String.Format("{0:###,###,##0}", nGtot1);
                strAmt2 = String.Format("{0:###,###,##0}", nGtot2);

                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "전체합계";
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                nGtot1 = 0;
                nGtot2 = 0;

                #endregion GRAND_TOT_SLIP1(GoSub) End

                btnPrint.Enabled = true;

            }

            dt.Dispose();
            dt = null;
            #endregion SQL_MAIN_SLIP1(GoSub) End
        }             

        void ss1_Clear()
        {
            nGetcount = 0;

            ssList_Sheet1.Rows.Count = 0;
            ssList_Sheet1.Rows.Count = 22;
        }

        //핫 키 설정, 2017-11-29 안정수
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!base.ProcessCmdKey(ref msg, keyData)) // 위에서 처리 안했으면
            {
                // 여기에 처리코드를 넣는다.
                if (keyData.Equals(Keys.F6))
                {
                    optJob1.Checked = true;
                    btnJob1_Click();
                    return false;
                }

                else if (keyData.Equals(Keys.F7))
                {
                    optJob2.Checked = true;
                    btnJob2_Click();
                    return false;
                }

                else if (keyData.Equals(Keys.F8))
                {
                    optJob3.Checked = true;
                    btnJob3_Click();
                    return false;
                }

                else if (keyData.Equals(Keys.F9))
                {
                    optJob4.Checked = true;
                    btnJob4_Click();
                    return false;
                }

                else if (keyData.Equals(Keys.F10))
                {
                    optJob5.Checked = true;
                    btnJob5_Click();
                    return false;
                }

                else if (keyData.Equals(Keys.Escape))
                {
                    btnNext_Click();
                    return false;
                }

                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
