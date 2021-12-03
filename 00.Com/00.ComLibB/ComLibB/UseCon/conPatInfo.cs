using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class conPatInfo : UserControl
    {
        //2017-09-06
        private frmAgreePrint frmAgreePrintEvent = null;
        private frmViewCsinfo frmViewCsinfoEvent = null;
        private frmAllergyAndAnti frmAllergyAndAntiEvent = null;
        private frmViewInfect frmViewInfectEvent = null;

        private string mstrDrYN = ""; //DR창 구분

        //등록번호, 이름
        private Color PtOnForeColor = Color.Blue;
        private Color PtOffForeColor = Color.Silver;
        private Color PtOnBackColor = Color.FromArgb(255, 255, 192);
        private Color PtOffBackColor = Color.White;

        //제외한 나머지
        private Color InfoOnForeColor = Color.Black;
        private Color InfoOffForeColor = Color.Silver;
        private Color InfoOnBackColor = Color.PaleGreen;
        private Color InfoOffBackColor = Color.White;

        private string GempNo = "";
        private string GgbIo = "";
        private string GbDate = "";
        private string GPtno = "";
        private string GDeptCode = "";

        frmComConPatInfo_SCH frm;

        public string pDrYN
        {
            get
            {
                return mstrDrYN;
            }
            set
            {
                mstrDrYN = value;
            }
        }

        enum enmOPD_MASTER_MAIN { BDATE, PANO, SNAME, AGE, SEX, BI, BI_NM, AUTO_CHK, SECRET, GBDRG, GBIO, DEPT, WRCODE, DRNM, DRCD, INDATE, VIP, BLACK, GAMEK, SINGA, INFECT, GBSMS, OPDATE, OPNM, ILLS, INFE_IMG, ABO, CVR, FALL, BRADEN, FIRE, PNEUMONIA, BRAIN, AMI, OP_JIPYO, CONTRAST, AST, ALLEGY, ADR, NST, HEIGHT, WEIGHT, PREGNANT, OM };

        public conPatInfo()
        {
            InitializeComponent();
        }

        private void conPatInfo_Load(object sender, EventArgs e)
        {
            this.Height = 58;

            //아이템 올 클리어
            ComFunc.SetAllControlClear(this);

            lblHelp1.BackColor = Color.White;

            SetItemClear(mstrDrYN);
        }

        /// <summary>
        /// 컨트롤에 포함된 모든 컨트롤를 반환한다. GetAllControlsUsingRecursive와 동일
        /// </summary>
        /// <param name="containerControl"></param>
        /// <returns></returns>
        public static Control[] GetAllControls(Control containerControl)
        {
            List<Control> allControls = new List<Control>();

            Queue<Control.ControlCollection> queue = new Queue<Control.ControlCollection>();
            queue.Enqueue(containerControl.Controls);

            while (queue.Count > 0)
            {
                Control.ControlCollection controls
                            = (Control.ControlCollection)queue.Dequeue();
                if (controls == null || controls.Count == 0)
                    continue;

                foreach (Control control in controls)
                {
                    allControls.Add(control);
                    queue.Enqueue(control.Controls);
                }
            }

            return allControls.ToArray();
        }

        public void SetItemClear(string DrYN = "N")
        {
            Control[] controls = GetAllControls(this);

            foreach (Control ctl in controls)
            {
                if (ctl is Label)
                {
                    if (VB.Left(ctl.Name, 7) != "lblHelp")
                    {
                        ctl.ForeColor = InfoOffForeColor;
                        ctl.BackColor = InfoOffBackColor;
                    }
                }
                else if (ctl is ComboBox)
                {
                    ((ComboBox)ctl).ForeColor = InfoOffForeColor;
                }
            }

            //mstrDrYN = DrYN;

            //if (mstrDrYN == "Y")
            //{
            //진료에서 뜰때
            //Width = 1320;

            ppnlMedPatInfo1.Location = new Point(2, 100);
            ppnlMedPatInfo1.Visible = false;
            //}
            //else
            //{
            //진료 이외에 뜰때
            //Width = 1162;

            ppnlMedPatInfo1.Location = new Point(2, 2);
            ppnlMedPatInfo1.Visible = true;
            //}

            lblPTNO1.Text = "등록번호";
            lblSNAME1.Text = "성명";
            lblAGESEX1.Text = "나이/성별";
            lblDEPT1.Text = "진료과";
            lblDRNM1.Text = "진료의";
            lblWRCODE1.Text = "병동/병실";
            lblINDATE1.Text = "내원/입원일";
            lblABO1.Text = "혈액형";
            lblHEIGHT1.Text = "신장";
            lblWEIGHT1.Text = "체중";
            lblBSA.Text = "BSA";
            lblBINM1.Text = "유형";
            lblAUTOCHK1.Text = "후불";
            lblSECRET1.Text = "사생활";
            lblGBDRG1.Text = "DRG";
            lblGBSMS1.Text = "개인정보";
            lblVIP1.Text = "VIP";
            lblFamily1.Text = "직원가족";
            lblCatholic1.Text = "가톨릭";
            lblMom1.Text = "임산부";
            lblFALL1.Text = "낙상";
            lblBRADEN1.Text = "욕창";

            lblAIR1.Text = "공기";
            lblAIR1.Visible = true;
            picAIR1.Image = Properties.Resources.I00000;
            picAIR1.Visible = false;

            lblCONTACT1.Text = "접촉";
            lblCONTACT1.Visible = true;
            picCONTACT1.Image = Properties.Resources.I00000;
            picCONTACT1.Visible = false;

            lblBLOOD1.Text = "혈액";
            lblBLOOD1.Visible = true;
            picBLOOD1.Image = Properties.Resources.I00000;
            picBLOOD1.Visible = false;

            //비말
            lblBIMAL1.Text = "비말";
            lblBIMAL1.Visible = true;
            picBIMAL1.Image = Properties.Resources.I00000;
            picBIMAL1.Visible = false;

            //보호
            lblProtect1.Text = "보호";
            lblProtect1.Visible = true;
            picProtect1.Image = Properties.Resources.보호;
            picProtect1.Visible = false;

            //해외
            lblForegin1.Text = "해외";
            lblForegin1.Visible = true;
            picForegin1.Image = Properties.Resources.해외;
            picForegin1.Visible = false;

            //lblINFECT1.Text = "격리";
            lblALLEGY1.Text = "알러지";
            lblFIRE1.Text = "화재";

            //CI
            lblCI1.Text = "질지표";
            lblNST1.Text = "영양상담";

            cboDIS1.Items.Clear();
            cboDIS1.Items.Add("진단명");            // 진단명
            cboDIS1.SelectedIndex = 0;

            cboOP1.Items.Clear();
            cboOP1.Items.Add("수술명(수술일자/POD)");         // 수술일자
            cboOP1.SelectedIndex = 0;

            //dt.Rows[0][enmOPD_MASTER_MAIN.PNEUMONIA.ToString()].ToString();      // 폐렴
            //dt.Rows[0][enmOPD_MASTER_MAIN.BRAIN.ToString()].ToString();          // 뇌졸증
            //dt.Rows[0][enmOPD_MASTER_MAIN.AMI.ToString()].ToString();            // AMI
            //dt.Rows[0][enmOPD_MASTER_MAIN.OP_JIPYO.ToString()].ToString();       // 예방항생제
            //dt.Rows[0][enmOPD_MASTER_MAIN.CONTRAST.ToString()].ToString();       // 조영제
            //dt.Rows[0][enmOPD_MASTER_MAIN.AST.ToString()].ToString();            // 항생제반응
            //dt.Rows[0][enmOPD_MASTER_MAIN.ADR.ToString()].ToString();            // ADR
            //dt.Rows[0][enmOPD_MASTER_MAIN.CVR.ToString()].ToString();            // CVR
            //dt.Rows[0][enmOPD_MASTER_MAIN.GBIO.ToString()].ToString();            // 외래/입원/응급
        }

        /// <summary>환자공통정보</summary>
        /// <param name="empNo">이용자사번</param>
        /// <param name="gbIo">진료구분(입원, 외래, 응급)</param>
        /// <param name="bDate">발생일자(조회하고자 하는 일자)</param>
        /// <param name="Ptno">환자번호</param>
        /// <param name="DeptCode">진료과</param>
        public void SetDisPlay(string empNo, string gbIo, string bDate, string Ptno, string DeptCode)
        {
            GempNo = empNo;
            GgbIo = gbIo;
            GbDate = bDate;
            GPtno = Ptno;
            GDeptCode = DeptCode;

            ComFunc.SetAllControlClear(this);
            SetItemClear(mstrDrYN);

            DataTable dt = SelDB(gbIo, bDate, Ptno, DeptCode);

            if (dt != null && dt.Rows.Count > 0)
            {
                SetSpd(dt);

                //TODO : 2017.04.04.김홍록 : 권한에 의한 화면 조절
                //SetRestriction(empNo);
            }
        }

        /// <summary>
        /// 환자공통 및 스케쥴을 동시에 불수 있는 화면
        /// </summary>
        /// <param name="empNo"></param>
        /// <param name="gbIo"></param>
        /// <param name="bDate"></param>
        /// <param name="Ptno"></param>
        /// <param name="DeptCode"></param>
        /// <param name="pPan">외부화면을 제어 하기 위한 판넬</param>
        public void SetDisPlay(string empNo, string gbIo, string bDate, string Ptno, string DeptCode, Panel pPan)
        {
            GempNo = empNo;
            GgbIo = gbIo;
            GbDate = bDate;
            GPtno = Ptno;
            GDeptCode = DeptCode;

            ComFunc.SetAllControlClear(this);
            SetItemClear(mstrDrYN);

            DataTable dt = SelDB(gbIo, bDate, Ptno, DeptCode);

            if (dt != null && dt.Rows.Count > 0)
            {
                SetSpd(dt);

                //TODO : 2017.04.04.김홍록 : 권한에 의한 화면 조절
                //SetRestriction(empNo);
            }

            this.panel1.Visible = true;
            this.panel1.Width = 1234;
            this.panel1.Height = 145;

            frm = new frmComConPatInfo_SCH(Ptno, "TEST");

            this.panel1.Controls.Clear();

            frm.TopLevel = false;
            frm.Dock = System.Windows.Forms.DockStyle.Fill;
            frm.ControlBox = false;
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            frm.Show();

            this.panel1.Controls.Add(frm);

            pPan.Height = 55 + 220;
        }

        private void SetRestriction(string empNo)
        {
            // TODO : 2017.03.31.김홍록 : 사용자에 대한 보이는 옵션 제한
        }

        private void SetSpd(DataTable dt)
        {
            SetItemClear(mstrDrYN);

            if (dt.Rows[0][enmOPD_MASTER_MAIN.PANO.ToString()].ToString().Trim() != "")
            {
                lblPTNO1.Text = dt.Rows[0][enmOPD_MASTER_MAIN.PANO.ToString()].ToString().Trim();            // 환자번호
                lblPTNO1.ForeColor = Color.DimGray; // PtOnForeColor;
                lblPTNO1.BackColor = PtOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.SNAME.ToString()].ToString().Trim() != "")
            {
                lblSNAME1.Text = dt.Rows[0][enmOPD_MASTER_MAIN.SNAME.ToString()].ToString().Trim();           // 환자명
                lblSNAME1.ForeColor = PtOnForeColor;
                lblSNAME1.BackColor = PtOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.AGE.ToString()].ToString().Trim() != "" && dt.Rows[0][enmOPD_MASTER_MAIN.SEX.ToString()].ToString().Trim() != "")
            {
                //lblAGESEX1.Text = dt.Rows[0][enmOPD_MASTER_MAIN.AGE.ToString()].ToString().Trim() + "/" + dt.Rows[0][enmOPD_MASTER_MAIN.SEX.ToString()].ToString().Trim();             // 나이/성별
                lblAGESEX1.Text = clsVbfunc.READ_AGE_GESAN_Ex1(clsDB.DbCon,GPtno) + "/" + dt.Rows[0][enmOPD_MASTER_MAIN.SEX.ToString()].ToString().Trim();             // 나이/성별
                lblAGESEX1.ForeColor = InfoOnForeColor;
                lblAGESEX1.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.DEPT.ToString()].ToString().Trim() != "")
            {
                lblDEPT1.Text = dt.Rows[0][enmOPD_MASTER_MAIN.DEPT.ToString()].ToString().Trim();            // 과
                lblDEPT1.ForeColor = InfoOnForeColor;
                lblDEPT1.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.DRNM.ToString()].ToString().Trim() != "")
            {
                lblDRNM1.Text = dt.Rows[0][enmOPD_MASTER_MAIN.DRNM.ToString()].ToString().Trim();
                lblDRNM1.Tag = dt.Rows[0][enmOPD_MASTER_MAIN.DRCD.ToString()].ToString().Trim();            // 의사
                lblDRNM1.ForeColor = InfoOnForeColor;
                lblDRNM1.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.WRCODE.ToString()].ToString().Trim() != "")
            {
                lblWRCODE1.Text = dt.Rows[0][enmOPD_MASTER_MAIN.WRCODE.ToString()].ToString();          // 병동/병실/병상
                lblWRCODE1.ForeColor = InfoOnForeColor;
                lblWRCODE1.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.INDATE.ToString()].ToString().Trim() != "")
            {
                lblINDATE1.Text = VB.Left(dt.Rows[0][enmOPD_MASTER_MAIN.INDATE.ToString()].ToString(), 10);         // 입원일
                lblINDATE1.ForeColor = InfoOnForeColor;
                lblINDATE1.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.ABO.ToString()].ToString().Trim() != "")
            {
                lblABO1.Text = dt.Rows[0][enmOPD_MASTER_MAIN.ABO.ToString()].ToString();            // 혈액형
                lblABO1.ForeColor = InfoOnForeColor;
                lblABO1.BackColor = InfoOnBackColor;

                if (VB.Right(lblABO1.Text.Trim(), 1) == "-")
                {
                    lblABO1.Text = lblABO1.Text.Replace("-", "(Rh-)");
                    lblABO1.Font = new Font(lblABO1.Font.Name, lblABO1.Font.Size, FontStyle.Bold);
                }
                else
                {
                    lblABO1.Font = new Font(lblABO1.Font.Name, lblABO1.Font.Size, FontStyle.Regular);
                }
            }

            //키/몸무게 다시 정의 2021-04-05 KMC
            if (dt.Rows[0][enmOPD_MASTER_MAIN.GBIO.ToString()].ToString().Trim() == "외래")
            {
                string strBody = ComFunc.Get_Opd_Body(lblPTNO1.Text.Trim(), lblINDATE1.Text);

                if (strBody != "")
                {
                    lblHEIGHT1.Text = VB.Pstr(strBody, "^^", 1) + "cm";         // 키
                    lblHEIGHT1.ForeColor = InfoOnForeColor;
                    lblHEIGHT1.BackColor = InfoOnBackColor;

                    lblWEIGHT1.Text = VB.Pstr(strBody, "^^", 2) + "kg";         // 체중
                    lblWEIGHT1.ForeColor = InfoOnForeColor;
                    lblWEIGHT1.BackColor = InfoOnBackColor;
                }
            }
            else
            {
                string strBody = ComFunc.Get_Ipd_Body(lblPTNO1.Text.Trim(), lblINDATE1.Text);

                if (strBody != "")
                {
                    lblHEIGHT1.Text = VB.Pstr(strBody, "^^", 1) + "cm";         // 키
                    lblHEIGHT1.ForeColor = InfoOnForeColor;
                    lblHEIGHT1.BackColor = InfoOnBackColor;

                    lblWEIGHT1.Text = VB.Pstr(strBody, "^^", 2) + "kg";         // 체중
                    lblWEIGHT1.ForeColor = InfoOnForeColor;
                    lblWEIGHT1.BackColor = InfoOnBackColor;
                }
            }

            //혈액종양내과 요청 2020-12-23 KMC
            string strBSA = ComFunc.GetBSA(lblHEIGHT1.Text.Trim(), lblWEIGHT1.Text.Trim());

            //외래스테이션에서 Vital 작성시 그 자료를 활용함
            if (strBSA != "")
            {
                lblBSA.Text = strBSA;
                lblBSA.ForeColor = InfoOnForeColor;
                lblBSA.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.BI_NM.ToString()].ToString().Trim() != "")
            {
                lblBINM1.Text = dt.Rows[0][enmOPD_MASTER_MAIN.BI_NM.ToString()].ToString();           // 보험유형
                lblBINM1.ForeColor = InfoOnForeColor;
                lblBINM1.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.AUTO_CHK.ToString()].ToString().Trim() == "Y")
            {
                lblAUTOCHK1.Text = "후불";       // 후불
                lblAUTOCHK1.ForeColor = InfoOnForeColor;
                lblAUTOCHK1.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.SECRET.ToString()].ToString().Trim() != "")
            {
                lblSECRET1.Text = "사생활";         // 사생활
                lblSECRET1.ForeColor = InfoOnForeColor;
                lblSECRET1.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.GBDRG.ToString()].ToString().Trim() != "")
            {
                lblGBDRG1.Text = dt.Rows[0][enmOPD_MASTER_MAIN.GBDRG.ToString()].ToString();          // DRG
                lblGBDRG1.ForeColor = InfoOnForeColor;
                lblGBDRG1.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.GBSMS.ToString()].ToString().Trim() != "")
            {
                switch (dt.Rows[0][enmOPD_MASTER_MAIN.GBSMS.ToString()].ToString())
                {
                    case "Y":
                        lblGBSMS1.Text = "정보동의";
                        break;
                    case "N":
                        lblGBSMS1.Text = "정보요청";
                        break;
                    case "X":
                        lblGBSMS1.Text = "정보거부";
                        break;
                }

                lblGBSMS1.ForeColor = InfoOnForeColor;
                lblGBSMS1.BackColor = InfoOnBackColor;
            }
            else
            {
                lblGBSMS1.Text = "정보요청";          // 개인정보여부
                lblGBSMS1.ForeColor = InfoOnForeColor;
                lblGBSMS1.BackColor = InfoOnBackColor;
            }

            //VIP
            if (dt.Rows[0][enmOPD_MASTER_MAIN.VIP.ToString()].ToString().Trim() != "")
            {
                lblVIP1.ForeColor = InfoOnForeColor;
                lblVIP1.BackColor = InfoOnBackColor;
            }

            //Family (직원가족에 대한 재정의 필요 : 2018.07.27) 
            if (dt.Rows[0][enmOPD_MASTER_MAIN.GAMEK.ToString()].ToString().Trim() == "병원 직원"
                || dt.Rows[0][enmOPD_MASTER_MAIN.GAMEK.ToString()].ToString().Trim() == "병원 직원의 배우자"
                || dt.Rows[0][enmOPD_MASTER_MAIN.GAMEK.ToString()].ToString().Trim() == "병원 직원의 직계존비속"
                || dt.Rows[0][enmOPD_MASTER_MAIN.GAMEK.ToString()].ToString().Trim() == "병원 직원의 친형제,자매,장인,장모,시부모"
                || dt.Rows[0][enmOPD_MASTER_MAIN.GAMEK.ToString()].ToString().Trim() == "병원 (주)영일직원 본인"
                || dt.Rows[0][enmOPD_MASTER_MAIN.GAMEK.ToString()].ToString().Trim() == "병원 (주)영일직원. 배우자,직계존비속등"
                || dt.Rows[0][enmOPD_MASTER_MAIN.GAMEK.ToString()].ToString().Trim() == "재단소속 성직자 및 수도자") //2018.07.27 이상훈 추가
            {
                lblFamily1.ForeColor = InfoOnForeColor;
                lblFamily1.BackColor = InfoOnBackColor;
            }

            //가톨릭신자
            if (dt.Rows[0][enmOPD_MASTER_MAIN.SINGA.ToString()].ToString().Trim() == "1")
            {
                lblCatholic1.ForeColor = InfoOnForeColor;
                lblCatholic1.BackColor = InfoOnBackColor;
            }

            //임산부
            if (dt.Rows[0][enmOPD_MASTER_MAIN.PREGNANT.ToString()].ToString().Trim() == "Y")
            {
                lblMom1.ForeColor = InfoOnForeColor;
                lblMom1.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.FALL.ToString()].ToString().Trim() == "Y")
            {
                lblFALL1.Text = "낙상";            // 낙상
                lblFALL1.ForeColor = InfoOnForeColor;
                lblFALL1.BackColor = InfoOnBackColor;
            }

            if (VB.Val(dt.Rows[0][enmOPD_MASTER_MAIN.BRADEN.ToString()].ToString().Trim()) > 0)
            {
                lblBRADEN1.Text = "욕창";         // 욕창
                lblBRADEN1.ForeColor = InfoOnForeColor;
                lblBRADEN1.BackColor = InfoOnBackColor;
            }

            //혈액
            if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 1, 1) == "1")
            {
                lblBLOOD1.Visible = false;
                picBLOOD1.Visible = true;
                picBLOOD1.Image = Properties.Resources.I00100;
            }

            //접촉1
            if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 2, 1) == "1")
            {
                lblCONTACT1.Visible = false;
                picCONTACT1.Visible = true;
                picCONTACT1.Image = Properties.Resources.I01000;
            }

            //접촉2(격리)
            if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 3, 1) == "1")
            {
                lblCONTACT1.Visible = false;
                picCONTACT1.Visible = true;
                picCONTACT1.Image = Properties.Resources.I00001;
            }

            //공기
            if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 4, 1) == "1")
            {
                lblAIR1.Visible = false;
                picAIR1.Visible = true;
                picAIR1.Image = Properties.Resources.I10000;
            }

            //비말
            if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 5, 1) == "1")
            {
                lblBIMAL1.Visible = false;
                picBIMAL1.Visible = true;
                picBIMAL1.Image = Properties.Resources.I00010;
            }

            //보호
            if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 6, 1) == "1")
            {
                lblProtect1.Visible = false;
                picProtect1.Visible = true;
                picProtect1.Image = Properties.Resources.보호;
            }

            //해외
            if (VB.Mid(dt.Rows[0][enmOPD_MASTER_MAIN.INFE_IMG.ToString()].ToString().Trim(), 7, 1) == "1")
            {
                lblForegin1.Visible = false;
                picForegin1.Visible = true;
                picForegin1.Image = Properties.Resources.해외;
            }

            #region //2018-09-18  사용안함 이현종
            ////옴
            //if (dt.Rows[0][enmOPD_MASTER_MAIN.OM.ToString()].ToString().Trim() == "Y")
            //{
            //    lblOM1.Text = "옴";
            //    lblOM1.ForeColor = InfoOnForeColor;
            //    lblOM1.BackColor = InfoOnBackColor;
            //}

            //격리
            //if (dt.Rows[0][enmOPD_MASTER_MAIN.INFECT.ToString()].ToString().Trim() == "Y")
            //{
            //    lblINFECT1.Text = "격리";
            //    lblINFECT1.ForeColor = InfoOnForeColor;
            //    lblINFECT1.BackColor = InfoOnBackColor;
            //}
            #endregion //2018-09-18  사용안함 이현종

            if (dt.Rows[0][enmOPD_MASTER_MAIN.ALLEGY.ToString()].ToString().Trim() == "Y")
            {
                lblALLEGY1.Text = "알러지";         // 알러지
                lblALLEGY1.ForeColor = InfoOnForeColor;
                lblALLEGY1.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.FIRE.ToString()].ToString().Trim() != "")
            {
                if (dt.Rows[0][enmOPD_MASTER_MAIN.FIRE.ToString()].ToString() == "Bed")
                { lblFIRE1.Text = "침대"; }
                else if (dt.Rows[0][enmOPD_MASTER_MAIN.FIRE.ToString()].ToString() == "W/C")
                { lblFIRE1.Text = "휠체어"; }
                else if (dt.Rows[0][enmOPD_MASTER_MAIN.FIRE.ToString()].ToString() == "Walking")
                { lblFIRE1.Text = "보행"; }

                lblFIRE1.ForeColor = InfoOnForeColor;
                lblFIRE1.BackColor = InfoOnBackColor;
            }

            //CI
            //if(dt.Rows[0]["CI"].ToString().Trim() != "")
            //{
            //    lblCI1.Text = "";
            //    lblCI2.Text = "";
            //    lblCI1.ForeColor = InfoOnForeColor;
            //    lblCI2.ForeColor = InfoOnBackColor;
            //}

            if (dt.Rows[0][enmOPD_MASTER_MAIN.NST.ToString()].ToString().Trim() != "N")
            {
                //lblNST1.Text = dt.Rows[0][enmOPD_MASTER_MAIN.NST.ToString()].ToString();            // NST
                lblNST1.ForeColor = InfoOnForeColor;
                lblNST1.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmOPD_MASTER_MAIN.ILLS.ToString()].ToString().Trim() != "")
            {
                cboDIS1.Items.Clear();
                cboDIS1.Items.Add(dt.Rows[0][enmOPD_MASTER_MAIN.ILLS.ToString()].ToString());            // 진단명
                cboDIS1.SelectedIndex = 0;

                cboDIS1.ForeColor = InfoOnForeColor;
            }

            if (clsType.User.JobGroup == "JOB013013" || clsType.User.JobGroup == "JOB013015" || clsType.User.JobGroup == "JOB013016")
            {
                SetIpdOpDate(lblPTNO1.Text.Trim(), lblINDATE1.Text.Trim());
            }
            else
            {
                if (dt.Rows[0][enmOPD_MASTER_MAIN.OPDATE.ToString()].ToString().Trim() != "")
                {
                    cboOP1.Items.Clear();
                    cboOP1.Items.Add("POD[ " +
                        VB.DateDiff("d", dt.Rows[0][enmOPD_MASTER_MAIN.OPDATE.ToString()].ToString()
                        , ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")) + " ] "
                        + dt.Rows[0][enmOPD_MASTER_MAIN.OPNM.ToString()].ToString()
                        + " (" + dt.Rows[0][enmOPD_MASTER_MAIN.OPDATE.ToString()].ToString() + ")");         // 수술일자
                    cboOP1.SelectedIndex = 0;

                    cboOP1.ForeColor = InfoOnForeColor;
                }
            }

            //dt.Rows[0][enmOPD_MASTER_MAIN.PNEUMONIA.ToString()].ToString();      // 폐렴
            //dt.Rows[0][enmOPD_MASTER_MAIN.BRAIN.ToString()].ToString();          // 뇌졸증
            //dt.Rows[0][enmOPD_MASTER_MAIN.AMI.ToString()].ToString();            // AMI
            //dt.Rows[0][enmOPD_MASTER_MAIN.OP_JIPYO.ToString()].ToString();       // 예방항생제
            //dt.Rows[0][enmOPD_MASTER_MAIN.CONTRAST.ToString()].ToString();       // 조영제
            //dt.Rows[0][enmOPD_MASTER_MAIN.AST.ToString()].ToString();            // 항생제반응
            //dt.Rows[0][enmOPD_MASTER_MAIN.ADR.ToString()].ToString();            // ADR
            //dt.Rows[0][enmOPD_MASTER_MAIN.CVR.ToString()].ToString();            // CVR
            //dt.Rows[0][enmOPD_MASTER_MAIN.GBIO.ToString()].ToString();            // 외래/입원/응급
        }

        private void SetIpdOpDate(string strPano, string strIndate)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = "SELECT 'POD[ ' || (TRUNC(SYSDATE) - OPDATE)      ";
                SQL = SQL + ComNum.VBLF + "    || ' ] ' || OPTITLE || ' (' || TO_CHAR(OPDATE, 'YYYY-MM-DD')      ";
                SQL = SQL + ComNum.VBLF + "    || ')' AS OPDATA      ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.ORAN_MASTER      ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + strPano + "'      ";
                SQL = SQL + ComNum.VBLF + "    AND OPDATE >= TO_DATE('" + strIndate + "','YYYY-MM-DD')      ";
                SQL = SQL + ComNum.VBLF + "ORDER BY OPDATE DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    cboOP1.Items.Clear();

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboOP1.Items.Add(dt.Rows[i]["OPDATA"].ToString().Trim());
                    }

                    cboOP1.SelectedIndex = 0;
                    cboOP1.ForeColor = InfoOnForeColor;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private DataTable SelDB(string gbIo, string bDate, string Ptno, string DeptCode)
        {
            string SQL = "";    //Query문
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                //SQL = "";
                //SQL = SQL + "  SELECT                                                                                                                                        \r\n";
                //SQL = SQL + "         OPD.PANO                                                                                  AS PANO     -- 01 환자번호                   \r\n";
                //SQL = SQL + "       , OPD.SNAME                                                                                 AS SNAME    -- 02 환자성명                   \r\n";
                //SQL = SQL + "       , KOSMOS_OCS.FC_GET_AGE(KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(OPD.PANO), TRUNC(SYSDATE))          AS AGE      -- 03 나이                       \r\n";
                //SQL = SQL + "       , OPD.SEX                                                                                   AS SEX      -- 04 성별                       \r\n";
                //SQL = SQL + "       , OPD.BI                                                                                    AS BI       -- 05 BI                         \r\n";
                //SQL = SQL + "       , KOSMOS_OCS.FC_BI_NM(OPD.BI)                                                               AS BI_NM    -- 06 보험유형                   \r\n";
                //SQL = SQL + "       , KOSMOS_OCS.FC_BAS_AUTO_MST_CHK(OPD.PANO, TRUNC(OPD.BDATE))                                AS AUTO_CHK -- 07 후불여부                   \r\n";
                //SQL = SQL + "       , ''                                                                                        AS SECRET   -- 08 사생활                     \r\n";
                //SQL = SQL + "       , ''                                                                                        AS GBDRG    -- 09 DRG여부                    \r\n";
                //SQL = SQL + "       , '외래'                                                                                    AS GBIO     -- 10 진료구분                   \r\n";
                //SQL = SQL + "       , OPD.DEPTCODE                                                                              AS DEPT     -- 11 진료과                     \r\n";
                //SQL = SQL + "       , ''                                                                                        AS WRCODE   -- 12 병동병실                   \r\n";
                //SQL = SQL + "       , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(OPD.DRCODE)                                               AS DRNM     -- 13 지정의                     \r\n";
                //SQL = SQL + "       , OPD.DRCODE                                                                                AS DRCD     -- 13 지정의코드                 \r\n";
                //SQL = SQL + "       , OPD.BDATE                                                                                 AS INDATE   -- 14 입원일자, 외래는 발생일자  \r\n";
                //SQL = SQL + "       , BAS.GB_VIP                                                                                AS VIP      -- 15 VIP                        \r\n";
                //SQL = SQL + "       , KOSMOS_OCS.FC_BAS_OCSMEMO_CHK(OPD.PANO, TRUNC(SYSDATE))                                   AS BLACK    -- 16 문제환자                   \r\n";
                //SQL = SQL + "       , KOSMOS_OCS.FC_BAS_GAMCODE_NM(OPD.GBGAMEK)                                                 AS GAMEK    -- 17 감액정보                   \r\n";
                //SQL = SQL + "       , BAS.RELIGION                                                                              AS SINGA    -- 18 가톨릭                     \r\n";
                //SQL = SQL + "       , ''                                                                                        AS INFECT   -- 19 보호(격리)                 \r\n";
                //SQL = SQL + "       , BAS.GBSMS                                                                                 AS GBSMS    -- 20 개인정보동의서             \r\n";
                //SQL = SQL + "       , KOSMOS_OCS.FC_ORAN_MASTER_OPDATE(OPD.PANO)                                                AS OPDATE   -- 21 수술일자                   \r\n";
                //SQL = SQL + "       , KOSMOS_OCS.FC_ORAN_MASTER_NM(OPD.PANO, KOSMOS_OCS.FC_ORAN_MASTER_OPDATE(OPD.PANO))        AS OPNM     -- 21 수술명                     \r\n ";
                //SQL = SQL + "       , KOSMOS_OCS.FC_OCS_ILLS(OPD.PANO, 'O', TRUNC(OPD.BDATE), OPD.DEPTCODE, NULL)               AS ILLS     -- 22 ILL                        \r\n";
                //SQL = SQL + "       , KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(OPD.PANO, TRUNC(OPD.BDATE))                          AS INFE_IMG -- 23 감염이미지                 \r\n";
                //SQL = SQL + "	   , KOSMOS_OCS.FC_EXAM_BLOOD_MASTER_ABO(OPD.PANO)                                             AS ABO      -- 24 ABO                        \r\n";
                //SQL = SQL + "       , ''                                                                                        AS CVR      -- 25 CVR                        \r\n";  // TODO : CVR
                //SQL = SQL + "       , KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(OPD.PANO, TRUNC(OPD.BDATE), 0, '0')                     AS FALL     -- 26 낙상                       \r\n";
                //SQL = SQL + "       , ''                                                                                        AS BRADEN   -- 27 욕창                       \r\n";
                //SQL = SQL + "       , ''                                                                                        AS FIRE     -- 28 화재                       \r\n";
                //SQL = SQL + "       , ''                                                                                        AS PNEUMONIA-- 29 폐렴                       \r\n";  // TODO : 폐렴
                //SQL = SQL + "       , ''                                                                                        AS BRAIN    -- 30 뇌졸증                     \r\n";  // TODO : 뇌졸증
                //SQL = SQL + "       , ''                                                                                        AS AMI      -- 31 AMI                        \r\n";  // TODO : AMI
                //SQL = SQL + "       , ''                                                                                        AS OP_JIPYO -- 32 수술예방항생제             \r\n";
                //SQL = SQL + "       , KOSMOS_OCS.FC_XRAY_CONTRAST_CHK(OPD.PANO)                                                 AS CONTRAST -- 33 조영제부작용환자           \r\n";
                //SQL = SQL + "       , KOSMOS_OCS.FC_NUR_AST_CHK(OPD.PANO)                                                       AS AST      -- 34 항생반응                   \r\n";
                //SQL = SQL + "       , KOSMOS_OCS.FC_ETC_ALLERGY_MST_CHK(OPD.PANO)                                               AS ALLEGY   -- 35 알러지                     \r\n";
                //SQL = SQL + "       , KOSMOS_OCS.FC_DRUG_ADR_CHK(OPD.PANO)                                                      AS ADR      -- 36 ADR                        \r\n";
                //SQL = SQL + "       , ''                                                                                        AS NST      -- 37 NST                        \r\n";
                //SQL = SQL + "       , ''                                                                                        AS HEIGHT   -- 38 키                         \r\n";  // TODO : Height
                //SQL = SQL + "       , ''                                                                                        AS WEIGHT   -- 39 몸무게                     \r\n";  // TODO : Weight
                //SQL = SQL + "       , OPD.PREGNANT                                                                              AS PREGNANT -- 40 산모여부                   \r\n";
                //SQL = SQL + "  FROM KOSMOS_PMPA.OPD_MASTER  OPD                                                                                                              \r\n";
                //SQL = SQL + "     , KOSMOS_PMPA.BAS_PATIENT BAS                                                                                                              \r\n";
                //SQL = SQL + " WHERE 1 = 1                                                                                                                                    \r\n";
                //SQL = SQL + "   AND '" + gbIo + "' IN ('O', 'E')                                                                                                             \r\n";
                //SQL = SQL + "  AND OPD.PANO     = " + ComFunc.covSqlstr(Ptno, false);
                //SQL = SQL + "   AND OPD.PANO     = BAS.PANO                                                                                                                  \r\n ";
                //SQL = SQL + "  AND OPD.BDATE    = " + ComFunc.covSqlDate(bDate, false);
                //SQL = SQL + "  AND OPD.DEPTCODE = " + ComFunc.covSqlstr(DeptCode, false);
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "    BAS.PANO                                                                                    AS PANO     -- 01 환자번호";
                SQL = SQL + ComNum.VBLF + "    , BAS.SNAME                                                                                 AS SNAME    -- 02 환자성명";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_GET_AGE(KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(BAS.PANO), TRUNC(SYSDATE))          AS AGE      -- 03 나이";
                SQL = SQL + ComNum.VBLF + "    , BAS.SEX                                                                                   AS SEX      -- 04 성별";
                SQL = SQL + ComNum.VBLF + "    , OPD.BI                                                                                    AS BI       -- 05 BI";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_BI_NM(OPD.BI)                                                               AS BI_NM    -- 06 보험유형";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_BAS_AUTO_MST_CHK(OPD.PANO, TRUNC(OPD.BDATE))                                AS AUTO_CHK -- 07 후불여부";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS SECRET   -- 08 사생활";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS GBDRG    -- 09 DRG여부";
                SQL = SQL + ComNum.VBLF + "    , '외래'                                                                                    AS GBIO     -- 10 진료구분";
                SQL = SQL + ComNum.VBLF + "    , OPD.DEPTCODE                                                                              AS DEPT     -- 11 진료과";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS WRCODE   -- 12 병동병실";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(OPD.DRCODE)                                               AS DRNM     -- 13 지정의";
                SQL = SQL + ComNum.VBLF + "    , OPD.DRCODE                                                                                AS DRCD     -- 13 지정의코드";
                SQL = SQL + ComNum.VBLF + "    , OPD.BDATE                                                                                 AS INDATE   -- 14 입원일자, 외래는 발생일자";
                SQL = SQL + ComNum.VBLF + "    , BAS.GB_VIP                                                                                AS VIP      -- 15 VIP";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_BAS_OCSMEMO_CHK(BAS.PANO, TRUNC(SYSDATE))                                   AS BLACK    -- 16 문제환자";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_BAS_GAMCODE_NM(BAS.GBGAMEK)                                                 AS GAMEK    -- 17 감액정보";
                SQL = SQL + ComNum.VBLF + "    , BAS.RELIGION                                                                              AS SINGA    -- 18 가톨릭";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS INFECT   -- 19 보호(격리)";
                SQL = SQL + ComNum.VBLF + "    , BAS.GBSMS                                                                                 AS GBSMS    -- 20 개인정보동의서";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_ORAN_MASTER_OPDATE(BAS.PANO)                                                AS OPDATE   -- 21 수술일자";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_ORAN_MASTER_NM(OPD.PANO, KOSMOS_OCS.FC_ORAN_MASTER_OPDATE(BAS.PANO))        AS OPNM     -- 21 수술명";
                if (DeptCode == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_OCS_ILLS(BAS.PANO, 'E', TRUNC(OPD.BDATE), OPD.DEPTCODE, NULL)               AS ILLS     -- 22 ILL";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_OCS_ILLS(BAS.PANO, 'O', TRUNC(OPD.BDATE), OPD.DEPTCODE, NULL)               AS ILLS     -- 22 ILL";
                }
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(BAS.PANO, TRUNC(OPD.BDATE))                          AS INFE_IMG -- 23 감염이미지";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_EXAM_BLOOD_MASTER_ABO(BAS.PANO)                                             AS ABO      -- 24 ABO";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS CVR      -- 25 CVR";
                //SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(BAS.PANO, TRUNC(OPD.BDATE))                              AS FALL     -- 26 낙상";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(BAS.PANO, TRUNC(SYSDATE))                              AS FALL     -- 26 낙상";
                SQL = SQL + ComNum.VBLF + "    , 0                                                                                        AS BRADEN   -- 27 욕창";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS FIRE     -- 28 화재";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS PNEUMONIA-- 29 폐렴";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS BRAIN    -- 30 뇌졸증";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS AMI      -- 31 AMI";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS OP_JIPYO -- 32 수술예방항생제";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_XRAY_CONTRAST_CHK(BAS.PANO)                                                 AS CONTRAST -- 33 조영제부작용환자";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_NUR_AST_CHK(BAS.PANO)                                                       AS AST      -- 34 항생반응";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_ETC_ALLERGY_MST_CHK(BAS.PANO)                                               AS ALLEGY   -- 35 알러지";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_DRUG_ADR_CHK(BAS.PANO)                                                      AS ADR      -- 36 ADR";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS NST      -- 37 NST";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS HEIGHT   -- 38 키";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS WEIGHT   -- 39 몸무게";
                SQL = SQL + ComNum.VBLF + "    , OPD.PREGNANT                                                                              AS PREGNANT -- 40 산모여부";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_EXAM_INFECT_MASTER_OM(BAS.PANO)                                             AS OM       -- 41 옴";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_PATIENT BAS";
                SQL = SQL + ComNum.VBLF + "    LEFT OUTER JOIN KOSMOS_PMPA.OPD_MASTER OPD";
                SQL = SQL + ComNum.VBLF + "        ON OPD.PANO = BAS.PANO";
                SQL = SQL + ComNum.VBLF + "            AND OPD.BDATE = " + ComFunc.covSqlDate(bDate, false);
                SQL = SQL + ComNum.VBLF + "            AND OPD.DEPTCODE = " + ComFunc.covSqlstr(DeptCode, false);
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND '" + gbIo + "' IN('O', 'E')";
                SQL = SQL + ComNum.VBLF + "    AND BAS.PANO = " + ComFunc.covSqlstr(Ptno, false);
                SQL = SQL + ComNum.VBLF + " UNION ALL                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + " SELECT                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + "        IPD.PANO                                                                                  AS PANO     -- 01 환자번호          ";
                SQL = SQL + ComNum.VBLF + "      , IPD.SNAME                                                                                 AS SNAME    -- 02 환자성명          ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_GET_AGE(KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(IPD.PANO), TRUNC(SYSDATE))          AS AGE      -- 03 나이              ";
                SQL = SQL + ComNum.VBLF + "      , IPD.SEX                                                                                   AS SEX      -- 04 성별              ";
                SQL = SQL + ComNum.VBLF + "      , IPD.BI                                                                                    AS BI       -- 05 BI                ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_BI_NM(IPD.BI)                                                               AS BI_NM    -- 06 보험유형          ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_BAS_AUTO_MST_CHK(IPD.PANO, TRUNC(IPD.INDATE))                               AS AUTO_CHK -- 07 후불여부          ";
                SQL = SQL + ComNum.VBLF + "      , IPD.SECRET                                                                                AS SECRET   -- 08 사생활            ";
                SQL = SQL + ComNum.VBLF + "      , IPD.GBDRG                                                                                 AS GBDRG    -- 09 DRG여부           ";
                SQL = SQL + ComNum.VBLF + "      , '입원'                                                                                    AS GBIO     -- 10 진료구분          ";
                SQL = SQL + ComNum.VBLF + "      , IPD.DEPTCODE                                                                              AS DEPT     -- 11 진료과            ";
                SQL = SQL + ComNum.VBLF + "      , IPD.WARDCODE || '/' || IPD.ROOMCODE || DECODE(IPD.BEDNUM, NULL, '', '/' || IPD.BEDNUM)    AS WRCODE   -- 12 병동병실          ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(IPD.DRCODE)                                               AS DRNM     -- 13 지정의            ";
                SQL = SQL + ComNum.VBLF + "      , IPD.DRCODE                                                                                AS DRCD     -- 13 지정의코드       ";
                SQL = SQL + ComNum.VBLF + "      , IPD.INDATE                                                                                AS INDATE   -- 14 입원일자          ";
                SQL = SQL + ComNum.VBLF + "      , BAS.GB_VIP                                                                                AS VIP      -- 15 VIP               ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_BAS_OCSMEMO_CHK(IPD.PANO, TRUNC(IPD.INDATE))                                AS BLACK    -- 16 문제환자          ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_BAS_GAMCODE_NM(IPD.GBGAMEK)                                                 AS GAMEK    -- 17 감액정보          ";
                SQL = SQL + ComNum.VBLF + "      , BAS.RELIGION                                                                              AS SINGA    -- 18 가톨릭            ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_IPD_TRANSFOR_INFECT_CHK(IPD.PANO, IPD.IPDNO)                                AS INFECT   -- 19 보호(격리)        ";
                SQL = SQL + ComNum.VBLF + "      , BAS.GBSMS                                                                                 AS GBSMS    -- 20 개인정보동의서    ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_ORAN_MASTER_OPDATE(IPD.PANO)                                                AS OPDATE   -- 21 수술일자          ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_ORAN_MASTER_NM(IPD.PANO, KOSMOS_OCS.FC_ORAN_MASTER_OPDATE(IPD.PANO))        AS OPNM     -- 21 수술명            ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_OCS_ILLS(IPD.PANO, 'I', TRUNC(IPD.INDATE), IPD.DEPTCODE, NULL)              AS ILLS     -- 22 ILL               ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(IPD.PANO, TRUNC(IPD.INDATE))                         AS INFE_IMG -- 23 감염이미지        ";
                SQL = SQL + ComNum.VBLF + "	  , KOSMOS_OCS.FC_EXAM_BLOOD_MASTER_ABO(IPD.PANO)                                                AS ABO      -- 24 ABO                  ";
                SQL = SQL + ComNum.VBLF + "      , ''                                                                                        AS CVR      -- 25 CVR               "; // TODO : CVR
                //SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(IPD.PANO, TRUNC(IPD.INDATE))                            AS FALL     -- 26 낙상              ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(IPD.PANO, TRUNC(SYSDATE))                            AS FALL     -- 26 낙상              ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_READ_WARNING_BRADEN(IPD.IPDNO)                                          AS BRADEN   -- 27 욕창   "; // TODO : 욕창
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_NUR_MASTER_FIRE(IPD.IPDNO)                                                  AS FIRE     -- 28 화재              ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_NUR_MASTER_pneumonia(IPD.IPDNO)                                             AS PNEUMONIA-- 29 폐렴              "; // TODO : 폐렴
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_IPD_TRANS_BRAIN(IPD.IPDNO)                                                  AS BRAIN    -- 30 뇌졸증            "; // TODO : 뇌졸증
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_NUR_MASTER_AMI(IPD.IPDNO)                                                   AS AMI      -- 31 AMI               "; // TODO : AMI
                SQL = SQL + ComNum.VBLF + "      , IPD.OP_JIPYO                                                                              AS OP_JIPYO -- 32 수술예방항생제    ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_XRAY_CONTRAST_CHK(IPD.PANO)                                                 AS CONTRAST -- 33 조영제부작용환자  ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_NUR_AST_CHK(IPD.PANO)                                                       AS AST      -- 34 항생반응          ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_ETC_ALLERGY_MST_CHK(IPD.PANO)                                               AS ALLEGY   -- 35 알러지            ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_DRUG_ADR_CHK(IPD.PANO)                                                      AS ADR      -- 36 ADR               ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_DIET_NST_PROGRESS_CHK(IPD.IPDNO)                                            AS NST      -- 37 NST               ";
                SQL = SQL + ComNum.VBLF + "      , TO_CHAR(IPD.HEIGHT)                                                                       AS HEIGHT   -- 38 키                ";
                SQL = SQL + ComNum.VBLF + "      , TO_CHAR(IPD.WEIGHT)                                                                       AS WEIGHT   -- 39 몸무게            ";
                SQL = SQL + ComNum.VBLF + "      , IPD.PREGNANT                                                                              AS PREGNANT -- 40 산모여부          ";
                SQL = SQL + ComNum.VBLF + "      , KOSMOS_OCS.FC_EXAM_INFECT_MASTER_OM(IPD.PANO)                                             AS OM       -- 41 옴";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER    IPD                                                                                               ";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_PMPA.BAS_PATIENT       BAS                                                                                               ";
                SQL = SQL + ComNum.VBLF + "WHERE 1    = 1                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "     AND '" + gbIo + "' IN ('I')                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "     AND IPD.PANO = " + ComFunc.covSqlstr(Ptno, false);
                SQL = SQL + ComNum.VBLF + "     AND IPD.PANO = BAS.PANO                                   ";
                //SQL = SQL + ComNum.VBLF + "     AND IPD.OUTDATE IS NULL                                   ";
                //SQL = SQL + ComNum.VBLF + "     AND IPD.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD')       ";
                SQL = SQL + ComNum.VBLF + "     AND ((IPD.OUTDATE IS NULL AND IPD.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "         OR (INDATE <= TO_DATE('" + bDate + " 23:59', 'YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "             AND OUTDATE >= TO_DATE('" + bDate + " 00:00', 'YYYY-MM-DD HH24:MI')))";
                //입원취소자 제외(2019-11-13)
                SQL = SQL + ComNum.VBLF + "    AND IPD.GBSTS NOT IN ('9')";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return dt;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return dt;
                }

                return dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return dt;
            }
        }

        private void picAIR_MouseUp(object sender, MouseEventArgs e)
        {
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip((Control)sender, ((Control)sender).Tag.ToString());
        }

        private void Frm_rEventClose()
        {
            frmAgreePrintEvent.Dispose();
            frmAgreePrintEvent = null;
        }

        private void lbl_MouseUp(object sender, MouseEventArgs e)
        {
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip((Control)sender, ((Control)sender).Tag.ToString());
        }

        private void cbo_MouseUp(object sender, MouseEventArgs e)
        {
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip((Control)sender, ((Control)sender).Tag.ToString());
        }

        private void ViewInfect()
        {
            if (lblPTNO1.Text.Trim() == "" || lblPTNO1.Text.Trim() == "등록번호")
            { return; }

            string strPtNo = VB.IIf(VB.IsNumeric(lblPTNO1.Text.Trim()) == true, lblPTNO1.Text.Trim(), "").ToString();

            if (frmViewInfectEvent != null)
            {
                frmViewInfectEvent.Dispose();
                frmViewInfectEvent = null;
            }

            frmViewInfectEvent = new frmViewInfect(strPtNo);
            frmViewInfectEvent.ShowDialog();

            SetDisPlay(GempNo, GgbIo, GbDate, GPtno, GDeptCode);
        }

        private void lblPTNO_DoubleClick(object sender, EventArgs e)
        {
            if (((Label)sender).Text.Trim() != "등록번호")
            {
                if (frmViewCsinfoEvent != null)
                {
                    frmViewCsinfoEvent.Dispose();
                    frmViewCsinfoEvent = null;
                }

                frmViewCsinfoEvent = new frmViewCsinfo(lblPTNO1.Text.Trim());
                frmViewCsinfoEvent.ShowDialog();

                SetDisPlay(GempNo, GgbIo, GbDate, GPtno, GDeptCode);
            }
        }

        private void lblGBSMS_DoubleClick(object sender, EventArgs e)
        {
            if (((Label)sender).Text.Trim() == "동의" || ((Label)sender).Text.Trim() == "거부")
            {
                return;
            }

            if (frmAgreePrintEvent != null)
            {
                frmAgreePrintEvent.Dispose();
                frmAgreePrintEvent = null;
            }

            frmAgreePrintEvent = new frmAgreePrint(lblPTNO1.Text.Trim(), "0", "O", lblINDATE1.Text.Replace("-", "").Trim(), "120000", "", "120000", lblDEPT1.Text.Trim(), "", lblDRNM1.Tag.ToString(), "1", "2204");
            frmAgreePrintEvent.rEventClose += Frm_rEventClose;
            frmAgreePrintEvent.ShowDialog();
        }

        private void pic_DoubleClick(object sender, EventArgs e)
        {
            ViewInfect();
        }

        private void lbl_DoubleClick(object sender, EventArgs e)
        {
            ViewInfect();
        }

        private void lblALLEGY_DoubleClick(object sender, EventArgs e)
        {
            if (lblPTNO1.Text.Trim() == "" || lblPTNO1.Text.Trim() == "등록번호")
            { return; }

            if (frmAllergyAndAntiEvent != null)
            {
                frmAllergyAndAntiEvent.Dispose();
                frmAllergyAndAntiEvent = null;
            }

            frmAllergyAndAntiEvent = new frmAllergyAndAnti(lblPTNO1.Text.Trim(), lblINDATE1.Text.Trim());
            frmAllergyAndAntiEvent.ShowDialog();

            SetDisPlay(GempNo, GgbIo, GbDate, GPtno, GDeptCode);
        }

        private void lblHelp_Click(object sender, EventArgs e)
        {
            frmPatInfoHelp frm = new frmPatInfoHelp();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }


        /// <summary>
        /// 낙상
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblFALL1_DoubleClick(object sender, EventArgs e)
        {
            string strMsg = "";
            
            if (GgbIo != "I") return;

            if (lblFALL1.BackColor == Color.PaleGreen)
            {
                strMsg = READ_DETAIL_FALL(GPtno, GbDate);
                if (strMsg != "")
                {
                    ComFunc.MsgBox(strMsg, "확인");
                }
            }
        }

        /// <summary>
        /// 욕창
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblBRADEN1_DoubleClick(object sender, EventArgs e)
        {
            string strMsg = "";

            if (GgbIo != "I") return;

            if (lblBRADEN1.BackColor == Color.PaleGreen)
            {
                strMsg = READ_DETAIL_BRADEN(GPtno, GbDate);
                if (strMsg != "")
                {
                    ComFunc.MsgBox(strMsg, "확인");
                }
            }
        }

        private void lblFIRE1_Click(object sender, EventArgs e)
        {
            string strMsg = "";

            if (GgbIo != "I") return;

            if (lblFIRE1.BackColor == Color.PaleGreen)
            {
                strMsg = READ_DETAIL_FIRE(GPtno, GbDate);
                if (strMsg != "")
                {
                    ComFunc.MsgBox(strMsg, "확인");
                }
            }
        }

        private void lblNST1_Click(object sender, EventArgs e)
        {
        }


        private string READ_DETAIL_FALL(string argPTNO, string argDATE)
        {
            // 여기꺼 바꾸면 CARE PLAN 것도 바꿔야 함.
            string ArgIPDNO = "";
            string ArgAge = "";

            string strFall = "";
            string strTOTAL = "";
            string strCAUSE = "";
            string strDrug = "";
            string strTEMP = "";
            string strTOOL = "";
            string strWARD_C = "";
            string strAGE_C = "";
            string strWARD = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT IPDNO, WARDCODE, AGE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER IPD ";
                SQL = SQL + ComNum.VBLF + " WHERE IPD.PANO = " + ComFunc.covSqlstr(argPTNO, false);
                SQL = SQL + ComNum.VBLF + "   AND ((IPD.OUTDATE IS NULL AND IPD.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "         OR (INDATE <= TO_DATE('" + argDATE + " 23:59', 'YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "        AND OUTDATE >= TO_DATE('" + argDATE + " 00:00', 'YYYY-MM-DD HH24:MI')))";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    ArgIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                    ArgAge = dt.Rows[0]["AGE"].ToString().Trim();
                    strWARD = dt.Rows[0]["WARDCODE"].ToString().Trim();

                    switch (dt.Rows[0]["WARDCODE"].ToString().Trim())
                    {
                        case "33":
                        case "35":
                            strFall = "OK";
                            strWARD_C = "중환자실 재원 환자";
                            break;
                        case "NR":
                        case "IQ":
                            strFall = "OK";
                            strWARD_C = "신생아실 재원 환자";
                            break;
                    }

                    if (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) >= 70)
                    {
                        strFall = "OK";
                        strAGE_C = "70세 이상 환자";
                    }

                    if (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) < 7)
                    {
                        strFall = "OK";
                        strAGE_C = "7세 미만 환자";
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                SQL = "";
                SQL = "  SELECT PANO, TOTAL ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_FALLMORSE_SCALE";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + " AND IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "     AND ROWID = (";
                SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                SQL = SQL + ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_FALLMORSE_SCALE";
                SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                SQL = SQL + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";
                strTOOL = "The Morse Fall Scale";

                if (VB.Val(ArgAge) < 18)
                {
                    SQL = "  SELECT PANO, TOTAL ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_FALLHUMPDUMP_SCALE";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                    SQL = SQL + ComNum.VBLF + " AND IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "     AND ROWID = (";
                    SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                    SQL = SQL + ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_FALLHUMPDUMP_SCALE";
                    SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                    SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                    SQL = SQL + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";
                    strTOOL = "The Humpty Dumpty Scale";
                }

                //신생아의 경우 도구표 사용하지 않음
                if (strWARD_C == "신생아실 재원 환자")
                {
                    strTOOL = "";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();
                    if (VB.Val(ArgAge) < 18 && VB.Val(strTOTAL) >= 12 || VB.Val(ArgAge) >= 18 && VB.Val(strTOTAL) >= 51)
                    {
                        strFall = "OK";
                    }
                }

                dt.Dispose();
                dt = null;

                strDrug = "";
                SQL = "";
                SQL = " SELECT * ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_FALL_WARNING";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND (WARNING1 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR WARNING2 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR WARNING3 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR WARNING4 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_01 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_02 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_03 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_04 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_05 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_06 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_07 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_08 = '1'";
                SQL = SQL + ComNum.VBLF + "                  OR DRUG_08_ETC <> '')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    strFall = "OK";

                    strCAUSE = "";
                    if (strAGE_C == "")
                    {
                        if (dt.Rows[0]["WARNING1"].ToString().Trim() == "1")
                        {
                            strCAUSE = strCAUSE + "70세이상 ";
                        }
                        if (dt.Rows[0]["WARNING2"].ToString().Trim() == "1")
                        {
                            strCAUSE = strCAUSE + "보행장애 ";
                        }
                        if (dt.Rows[0]["WARNING3"].ToString().Trim() == "1")
                        {
                            strCAUSE = strCAUSE + "혼미 ";
                        }
                        if (dt.Rows[0]["WARNING4"].ToString().Trim() == "1")
                        {
                            strCAUSE = strCAUSE + "어지럼증 ";
                        }
                        strDrug = "";

                        if (dt.Rows[0]["DRUG_01"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "진정제 ";
                        }
                        if (dt.Rows[0]["DRUG_02"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "수면제 ";
                        }
                        if (dt.Rows[0]["DRUG_03"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "향정신성약물 ";
                        }
                        if (dt.Rows[0]["DRUG_04"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "항우울제 ";
                        }
                        if (dt.Rows[0]["DRUG_05"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "완하제 ";
                        }
                        if (dt.Rows[0]["DRUG_06"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "이뇨제 ";
                        }
                        if (dt.Rows[0]["DRUG_07"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + "진정약물 ";
                        }
                        if (dt.Rows[0]["DRUG_08"].ToString().Trim() == "1")
                        {
                            strDrug = strDrug + dt.Rows[0]["DRUB_08_ETC"].ToString().Trim();
                        }
                    }
                }

                if (strFall == "OK")
                {
                    strTEMP = "";
                    if (strTOTAL == "")
                    {
                    }
                    else
                    {
                        if (strWARD !="33"&& strWARD != "35")
                        {
                            strTEMP = strTEMP + " => 낙상점수 : " + VB.Val(strTOTAL) + "점 ";
                            strTEMP = strTEMP + ComNum.VBLF;
                        }
                        

                    }

                    if (strWARD_C != "")
                    {
                        strTEMP = strTEMP + strWARD_C;
                        strTEMP = strTEMP + ComNum.VBLF;
                    }

                    if (strAGE_C != "")
                    {
                        strTEMP = strTEMP + strAGE_C;
                        strTEMP = strTEMP + ComNum.VBLF;
                    }

                    if (strCAUSE != "")
                    {
                        strTEMP = strTEMP + strCAUSE;
                        strTEMP = strTEMP + ComNum.VBLF;
                    }

                    if (strDrug != "")
                    {
                        strTEMP = strTEMP + "-위험약물:" + strDrug;
                        strTEMP = strTEMP + ComNum.VBLF;
                    }
                }
                return strTEMP;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
        }

        private string READ_DETAIL_BRADEN(string argPTNO, string argDATE, string ArgDate2 = "")
        {
            string ArgIPDNO = "";
            string ArgAge = "";
            string argWard = "";

            string strBraden = "";
            string strGUBUN = "";
            string strTOTAL = "";
            string strBun = "";
            string strTOOL = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //if (argPTNO == "09315922")
            //{

            //}


            //if (argPTNO == "08619351")
            //{
            //    argPTNO = argPTNO;
            //}

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT IPDNO, WARDCODE, AGE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER IPD ";
                SQL = SQL + ComNum.VBLF + " WHERE IPD.PANO = " + ComFunc.covSqlstr(argPTNO, false);
                SQL = SQL + ComNum.VBLF + "   AND ((IPD.OUTDATE IS NULL AND IPD.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "         OR (INDATE <= TO_DATE('" + argDATE + " 23:59', 'YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "        AND OUTDATE >= TO_DATE('" + argDATE + " 00:00', 'YYYY-MM-DD HH24:MI')))";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    ArgIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                    ArgAge = dt.Rows[0]["AGE"].ToString().Trim();
                    argWard = dt.Rows[0]["WARDCODE"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;



                if (argWard == "NR" || argWard == "ND" || argWard == "IQ")
                {
                    strGUBUN = "신생아";
                    //strTOOL = "신생아욕창사정 도구표";
                }
                else if (VB.Val(ArgAge) < 5)
                {
                    strGUBUN = "소아";
                    //strTOOL = "소아욕창사정 도구표";
                }
                else
                {
                    strGUBUN = "";
                    //strTOOL = "욕창사정 도구표";
                }


                if (strGUBUN == "")
                {
                    SQL = "";
                    SQL = " SELECT A.PANO, A.TOTAL, A.AGE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_SCALE A";
                    SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + argPTNO + "' ";
                    if (ArgDate2 != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                    }
                    SQL = SQL + ComNum.VBLF + "     AND A.ROWID = (";
                    SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                    SQL = SQL + ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_BRADEN_SCALE";
                    SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "       AND PANO = '" + argPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                    SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                    SQL = SQL + "  ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), A.ENTDATE) DESC ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return "";
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();

                        if (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) > 60 && VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 18 || VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) < 60 && VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 16)
                        {
                            strBraden = "OK";
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                else if (strGUBUN == "신생아")
                {
                    SQL = "";
                    SQL = "SELECT TOTAL ";
                    SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_BRADEN_SCALE_BABY ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + ArgIPDNO;
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPTNO + "' ";
                    if (ArgDate2 != "")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE >= TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     AND ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                    }
                    SQL = SQL + ComNum.VBLF + "  ORDER BY ACTDATE DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return "";
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();
                        if (VB.Val(dt.Rows[0]["Total"].ToString().Trim()) <= 20)
                        {
                            strBraden = "OK";
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                SQL = "";
                SQL = " SELECT *";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_WARNING ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ( ";
                SQL = SQL + ComNum.VBLF + "         WARD_ICU = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR GRADE_HIGH = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR PARAL = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR COMA = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR NOT_MOVE = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR DIET_FAIL = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR NEED_PROTEIN = '1' ";
                SQL = SQL + ComNum.VBLF + "      OR EDEMA = '1'";
                SQL = SQL + ComNum.VBLF + "      OR (BRADEN = '1' AND (BRADEN_OK = '0' OR BRADEN_OK = NULL))";
                SQL = SQL + ComNum.VBLF + "      )";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    strBraden = "OK";
                    strBun = "";

                    if (dt.Rows[0]["WARD_ICU"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "중환자실 ";
                    }
                    if (dt.Rows[0]["GRADE_HIGH"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "중증도 분류 3군 이상 ";
                    }
                    if (dt.Rows[0]["PARAL"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "뇌, 척추 관련 마비 ";
                    }
                    if (dt.Rows[0]["NOT_MOVE"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "부종 ";
                    }
                    if (dt.Rows[0]["DIET_FAIL"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "영양불량 ";
                    }
                    if (dt.Rows[0]["NEED_PROTEIN"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "단백질 불량 ";
                    }
                    if (dt.Rows[0]["EDEMA"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "부동 ";
                    }
                    if (dt.Rows[0]["BRADEN"].ToString().Trim() == "1")
                    {
                        strBun = strBun + "현재 욕창이 있는 환자 ";
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                string strTemp = "";
                if (strBraden == "OK")
                {
                    strTemp = "";
                    if (VB.Val(strTOTAL) > 0)
                    {
                        strTemp = strTemp + " ★ 욕창점수 : " + strTOTAL + ComNum.VBLF;
                    }

                    if (strBun != "")
                    {
                        strTemp = strTemp + strBun + ComNum.VBLF;
                    }
                }

                return strTemp;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
        }

        private string READ_DETAIL_FIRE(string argPTNO, string argDATE)
        {
            string ArgIPDNO = "";
            string ArgAge = "";
            string argWard = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strTemp = "";
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT IPDNO, WARDCODE, AGE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER IPD ";
                SQL = SQL + ComNum.VBLF + " WHERE IPD.PANO = " + ComFunc.covSqlstr(argPTNO, false);
                SQL = SQL + ComNum.VBLF + "   AND ((IPD.OUTDATE IS NULL AND IPD.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "         OR (INDATE <= TO_DATE('" + argDATE + " 23:59', 'YYYY-MM-DD HH24:MI')";
                SQL = SQL + ComNum.VBLF + "        AND OUTDATE >= TO_DATE('" + argDATE + " 00:00', 'YYYY-MM-DD HH24:MI')))";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    ArgIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                    ArgAge = dt.Rows[0]["AGE"].ToString().Trim();
                    argWard = dt.Rows[0]["WARDCODE"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT FIRE_EXIT_GUBUN ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE IPDNO = " + ArgIPDNO;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["FIRE_EXIT_GUBUN"].ToString().Trim())
                    {
                        case "1":
                            strTemp = strTemp + " 해당환자의 화재대피 유형은 1급(Bed) 환자입니다." + ComNum.VBLF;
                            //strTemp = strTemp + "   => 중환자, 거동불능환자, 당일 수술환자, 신생아" + ComNum.VBLF;
                            strTemp = strTemp + "   => 독립보행이 불가능한 환자, 신생아" + ComNum.VBLF;
                            strTemp = strTemp + "   ★ 의료진 동행하여야 합니다.";
                            break;
                        case "2":
                            strTemp = strTemp + " 해당환자의 화재대피 유형은 2급(Wheel Chair) 환자입니다." + ComNum.VBLF;
                            //strTemp = strTemp + "   => 산모, 소아, 노약환자" + ComNum.VBLF;
                            strTemp = strTemp + "   => 도움이 필요한 환자" + ComNum.VBLF;
                            strTemp = strTemp + "   ★ 간호사, 대피 요원, 보호자 동행하여야 합니다.";
                            break;
                        case "3":
                            strTemp = strTemp + " 해당환자의 화재대피 유형은 3급(Walking) 환자입니다." + ComNum.VBLF;
                            //strTemp = strTemp + "   => 일반환자, 거동이 가능한 환자" + ComNum.VBLF;
                            strTemp = strTemp + "   => 일반환자, 보행가능환자" + ComNum.VBLF;
                            strTemp = strTemp + "   ★ 보호자 도움 및 자력으로 대피하여야 합니다.";
                            break;
                    }
                }
                dt.Dispose();
                dt = null;
                return strTemp;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
        }

        private void lblNST1_DoubleClick(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strIPDNO = "";
            string strWRTNO = "";

            if (lblNST1.BackColor == Color.PaleGreen)
            {                
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    SQL = "";
                    SQL = " SELECT IPDNO, WARDCODE, AGE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER IPD ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPD.PANO = " + ComFunc.covSqlstr(GPtno, false);
                    SQL = SQL + ComNum.VBLF + "   AND ((IPD.OUTDATE IS NULL AND IPD.JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD'))";
                    SQL = SQL + ComNum.VBLF + "         OR (INDATE <= TO_DATE('" + GbDate + " 23:59', 'YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "        AND OUTDATE >= TO_DATE('" + GbDate + " 00:00', 'YYYY-MM-DD HH24:MI')))";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    strWRTNO = READ_NST_MAXWRTNO(strIPDNO);
                    READ_NST_SEl(GPtno, strIPDNO, strWRTNO);
                }
                catch (Exception ex)
                {
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }
        }

        private string READ_NST_MAXWRTNO(string ArgIPDNO)
        {
            string rtnVal = "";
             int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {                
                SQL = "SELECT MAX(WRTNO) WRTNO FROM KOSMOS_PMPA.DIET_NST_PROGRESS ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = '" + ArgIPDNO + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {                    
                    rtnVal = dt.Rows[0]["WRTNO"].ToString().Trim();                    
                }
                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private void READ_NST_SEl(string ArgPano, string ArgIPDNO, string argWRTNO)
        {
            clsPat.PATi.WRTNO = 0;
            clsPat.PATi.IPDNO = 0;
            clsPat.PATi.Pano = "";
            clsPat.PATi.sName = "";
            clsPat.PATi.Sex = "";
            clsPat.PATi.Age = "";
            clsPat.PATi.InDate = "";
            clsPat.PATi.DeptCode = "";
            clsPat.PATi.DrCode = "";
            clsPat.PATi.WardCode = "";
            clsPat.PATi.RoomCode = "";
            clsPat.PATi.RDate = "";
            clsPat.PATi.DIAGNOSIS = "";
            clsPat.PATi.BDate = "";
            clsPat.PATi.DRSABUN = "";
            clsPat.PATi.NRSABUN = "";
            clsPat.PATi.PMSABUN = "";
            clsPat.PATi.DTSABUN = "";

            clsPat.PATi.WRTNO = (double)VB.Val(argWRTNO);
            clsPat.PATi.Pano = ArgPano;
            clsPat.PATi.IPDNO = (double)VB.Val(ArgIPDNO);
            clsPat.PATi.RDate = "";
            
            frmNSTView_New frmNSTView_NewX = new frmNSTView_New("");
            frmNSTView_NewX.StartPosition = FormStartPosition.CenterParent;
            frmNSTView_NewX.ShowDialog();
        }
    }
}
