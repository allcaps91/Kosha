using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;
using ComLibB;

namespace ComHpcLibB
{
    public partial class conHcPatInfo : UserControl
    {
        clsHcMain cHcMain = null;
        ToolTip toolTip = null;

        //2017-09-06
        private frmAgreePrint frmAgreePrintEvent = null;
        private frmViewCsinfo frmViewCsinfoEvent = null;
        private frmAllergyAndAnti frmAllergyAndAntiEvent = null;
        private frmViewInfect frmViewInfectEvent = null;

        //등록번호, 이름
        private Color PtOnForeColor     = Color.Blue;
        private Color PtOffForeColor    = Color.Silver;
        private Color PtOnBackColor     = Color.FromArgb(255, 255, 192);
        private Color PtOffBackColor    = Color.White;

        //제외한 나머지
        private Color InfoOnForeColor   = Color.Black;
        private Color InfoOffForeColor  = Color.Silver;
        private Color InfoOnBackColor   = Color.PaleGreen;
        private Color InfoOffBackColor  = Color.White;

        private string GempNo = "";
        private string GgbIo = "";
        private string GbDate = "";
        private string GPtno = "";
        private string GDeptCode = "";

        enum enmHC_PAT_INFO { PANO, AGE, SEX, SNAME, JUMIN, JEPDATE, GJJONG, GBGJYN, LTDNAME, HEIGHT, WEIGHT, BPRESS, BPULSE, UCODENM, VIP, PRIVACY, CVR, ADR, ALLEGY, FALL, FIRE, REMARK, EXREMARK, PASTILL, INFE_IMG };
        
        public conHcPatInfo() 
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cHcMain = new clsHcMain();
            toolTip = new ToolTip();

            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;
        }

        void SetEvent()
        {
            this.Load               += new EventHandler(eFormLoad);
            this.lblUCODE.MouseUp   += new MouseEventHandler(elblMouseUp);
            this.lblGJJONG.MouseUp  += new MouseEventHandler(elblMouseUp);
            this.lblSECRET.MouseUp  += new MouseEventHandler(elblMouseUp);
            this.lblCVR.MouseUp     += new MouseEventHandler(elblMouseUp);
            this.lblADR.MouseUp     += new MouseEventHandler(elblMouseUp);
            this.lblALLERGY.MouseUp += new MouseEventHandler(elblMouseUp);
            this.lblALLERGY.DoubleClick += new EventHandler(elblDblClick);
            this.lblBIMAL.DoubleClick   += new EventHandler(elblDblClick);
            this.lblBLOOD.DoubleClick   += new EventHandler(elblDblClick);
            this.lblFOREGIN.DoubleClick += new EventHandler(elblDblClick);
            this.lblFALL.MouseUp    += new MouseEventHandler(elblMouseUp);
            this.lblBURN.MouseUp    += new MouseEventHandler(elblMouseUp);
        }

        private void elblDblClick(object sender, EventArgs e)
        {
            if (sender == lblALLERGY)
            {
                if (lblPTNO.Text.Trim() == "" || lblPTNO.Text.Trim() == "등록번호")
                { return; }

                if (frmAllergyAndAntiEvent != null)
                {
                    frmAllergyAndAntiEvent.Dispose();
                    frmAllergyAndAntiEvent = null;
                }

                frmAllergyAndAntiEvent = new frmAllergyAndAnti(lblPTNO.Text.Trim(), lblJEPDATE.Text.Trim());
                frmAllergyAndAntiEvent.ShowDialog();
            }
            else
            {
                ViewInfect();
            }
            
        }

        private void ViewInfect()
        {
            if (lblPTNO.Text.Trim() == "" || lblPTNO.Text.Trim() == "등록번호")
            { return; }

            string strPtNo = VB.IIf(VB.IsNumeric(lblPTNO.Text.Trim()) == true, lblPTNO.Text.Trim(), "").ToString();

            if (frmViewInfectEvent != null)
            {
                frmViewInfectEvent.Dispose();
                frmViewInfectEvent = null;
            }

            frmViewInfectEvent = new frmViewInfect(strPtNo);
            frmViewInfectEvent.ShowDialog();
        }

        void elblMouseUp(object sender, MouseEventArgs e)
        {
            if (sender == lblUCODE || sender == lblGJJONG)
            {
                toolTip.SetToolTip((Control)sender, ((Control)sender).Text.ToString());
            }
            else
            {
                toolTip.SetToolTip((Control)sender, ((Control)sender).Tag.ToString());
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //아이템 올 클리어
            ComFunc.SetAllControlClear(this);

            SetItemClear();
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

        public void SetItemClear()
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
            }

            lblPTNO.Text    = "등록번호";
            lblAGE.Text     = "나이";
            lblSEX.Text     = "성별";
            lblSNAME.Text   = "수검자명";
            lblBIRTH.Text   = "생년월일";

            lblJEPDATE.Text = "검진일자";
            lblGJNAME.Text  = "검진종류";
            lblGJJONG.Text  = "";
            
            lblLTDNAME.Text = "사업장명";
            lblUCODE.Text   = "유해인자";

            lblVIP.Text     = "VIP";
            lblSECRET.Text  = "개인정보";
            lblCVR.Text     = "CVR";
            lblADR.Text     = "ADR";
            lblALLERGY.Text = "알러지";
            lblFALL.Text    = "낙상";
            lblBURN.Text    = "화재";
            lblPATREMARK.Text = "환자 참고사항";

            lblBIMAL.Text   = "비말";
            lblBLOOD.Text   = "혈액";
            lblFOREGIN.Text = "해외";

            lblEXAM.Text    = "검사요령";
            lblGWA.Text     = "과거력";
            
        }

        /// <summary>환자공통정보</summary>
        /// <param name="empNo">이용자사번</param>
        /// <param name="gbIo">진료구분(입원, 외래, 응급)</param>
        /// <param name="bDate">검진일자(조회하고자 하는 일자)</param>
        /// <param name="Ptno">환자번호</param>
        /// <param name="DeptCode">진료과</param>
        public void SetDisPlay(string empNo, string gbIo, string bDate, string Ptno, string DeptCode, string GjJong)
        {
            GempNo = empNo;
            GgbIo = gbIo;
            GbDate = bDate;
            GPtno = Ptno;
            GDeptCode = DeptCode;

            ComFunc.SetAllControlClear(this);
            SetItemClear();

            DataTable dt = SelDB(gbIo, bDate, Ptno, DeptCode, GjJong);

            if (dt != null && dt.Rows.Count > 0)
            {
                SetSpd(dt);
            }
        }

        private DataTable SelDB(string gbIo, string bDate, string Ptno, string DeptCode, string GjJong)
        {
            string SQL = "";    //Query문
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PAT.PTNO                                                                                   AS PANO     -- 01 환자번호";
                //SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_GET_AGE(KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(PAT.PTNO), TRUNC(SYSDATE))          AS AGE      -- 02 나이";
                SQL = SQL + ComNum.VBLF + "    , HIC.AGE                                                                                   AS AGE      -- 02 나이";
                SQL = SQL + ComNum.VBLF + "    , HIC.SEX                                                                                   AS SEX      -- 03 성별";
                SQL = SQL + ComNum.VBLF + "    , HIC.SNAME                                                                                 AS SNAME    -- 04 환자성명";
                SQL = SQL + ComNum.VBLF + "    , SUBSTR(PAT.JUMIN, 1, 6)                                                                   AS JUMIN    -- 05 생년월일";
                SQL = SQL + ComNum.VBLF + "    , TO_CHAR(HIC.JEPDATE, 'YYYY-MM-DD')                                                        AS JEPDATE  -- 06 접수일자";
                SQL = SQL + ComNum.VBLF + "    , HIC.GJJONG || '.' || KOSMOS_PMPA.FC_HIC_GJJONG_NAME(HIC.GJJONG, HIC.UCODES)               AS GJJONG   -- 07 검진종류";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_PMPA.FC_HIC_JEPSUJONG(HIC.PTNO, TO_DATE(HIC.JEPDATE))                              AS GBGJYN   -- 08 검진여부";       // TODO : 공백처리
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_PMPA.FC_HIC_LTDNAME(HIC.LTDCODE)                                                   AS LTDNAME  -- 09 사업장명칭";     
                //SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS WEIGHT   -- 10 신장";           
                //SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS HEIGHT   -- 11 몸무게";         
                //SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS BPRESS   -- 12 혈압";           
                //SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS BPULSE   -- 13 맥박";           
                SQL = SQL + ComNum.VBLF + "    , HIC.UCODES                                                                                AS UCODENM  -- 14 유해인자명";     // TODO : 공백처리
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_PMPA.FC_BAS_VIPINFO(HIC.PTNO) || '/' || PAT.VIPREMARK                              AS VIP      -- 15 VIP ";
                SQL = SQL + ComNum.VBLF + "    , PAT.GBPRIVACY                                                                             AS PRIVACY  -- 16 개인정보 ";
                SQL = SQL + ComNum.VBLF + "      , ''                                                                                      AS CVR      -- 17 CVR ";           // TODO : CVR
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_DRUG_ADR_CHK(PAT.PTNO)                                                      AS ADR      -- 18 ADR";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_ETC_ALLERGY_MST_CHK(PAT.PTNO)                                               AS ALLEGY   -- 19 알러지";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(PAT.PTNO, TRUNC(SYSDATE))                               AS FALL     -- 20 낙상";
                //SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_NUR_MASTER_FIRE(IPD.IPDNO)                                                  AS FIRE     -- 21 화재 ";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS FIRE     -- 21 화재 ";          // TODO : 화재(내시경실 정보필요)
                SQL = SQL + ComNum.VBLF + "    , HIC.REMARK                                                                                AS REMARK   -- 22 환자참고사항 ";
                SQL = SQL + ComNum.VBLF + "    , HIC.EXAMREMARK                                                                            AS EXREMARK -- 23 검사요령 ";
                SQL = SQL + ComNum.VBLF + "    , ''                                                                                        AS PASTILL  -- 24 과거력 ";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(HIC.PTNO, TRUNC(HIC.JEPDATE))                     AS INFE_IMG -- 25 감염이미지";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.HIC_PATIENT PAT ";
                SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN KOSMOS_PMPA.HIC_JEPSU HIC ";
                SQL = SQL + ComNum.VBLF + "         ON HIC.PTNO = PAT.PTNO ";
                SQL = SQL + ComNum.VBLF + "        AND HIC.JEPDATE = " + ComFunc.covSqlDate(bDate, false);
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1 ";
                SQL = SQL + ComNum.VBLF + "   AND PAT.PTNO = " + ComFunc.covSqlstr(Ptno, false);
                SQL = SQL + ComNum.VBLF + "   AND HIC.GJJONG = '" + GjJong + "' ";
                SQL = SQL + ComNum.VBLF + " UNION ALL                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + " SELECT                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + "      PAT.PTNO                                                                                  AS PANO     -- 01 환자번호";
                //SQL = SQL + ComNum.VBLF + "     , KOSMOS_OCS.FC_GET_AGE(KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(PAT.PTNO), TRUNC(SYSDATE))         AS AGE      -- 02 나이";
                SQL = SQL + ComNum.VBLF + "     , HEA.AGE                                                                                  AS AGE      -- 02 나이";
                SQL = SQL + ComNum.VBLF + "     , HEA.SEX                                                                                  AS SEX      -- 03 성별";
                SQL = SQL + ComNum.VBLF + "     , HEA.SNAME                                                                                AS SNAME    -- 04 환자성명";
                SQL = SQL + ComNum.VBLF + "     , SUBSTR(PAT.JUMIN, 1, 6)                                                                  AS JUMIN    -- 05 생년월일";
                SQL = SQL + ComNum.VBLF + "     , TO_CHAR(HEA.SDATE, 'YYYY-MM-DD')                                                         AS JEPDATE  -- 06 접수일자";
                SQL = SQL + ComNum.VBLF + "     , HEA.GJJONG || '.' || KOSMOS_PMPA.FC_HEA_GJJONG_NAME(HEA.GJJONG)                          AS GJJONG   -- 07 검진종류";       
                SQL = SQL + ComNum.VBLF + "     , KOSMOS_PMPA.FC_HIC_JEPSUJONG(HEA.PTNO, TO_DATE(HEA.SDATE))                               AS GBGJYN   -- 08 검진여부";       // TODO : 공백처리
                SQL = SQL + ComNum.VBLF + "     , KOSMOS_PMPA.FC_HIC_LTDNAME(HEA.LTDCODE)                                                  AS LTDNAME  -- 09 사업장명칭";
                //SQL = SQL + ComNum.VBLF + "     , ''                                                                                       AS WEIGHT   -- 10 신장";          
                //SQL = SQL + ComNum.VBLF + "     , ''                                                                                       AS HEIGHT   -- 11 몸무게";        
                //SQL = SQL + ComNum.VBLF + "     , ''                                                                                       AS BPRESS   -- 12 혈압";          
                //SQL = SQL + ComNum.VBLF + "     , ''                                                                                       AS BPULSE   -- 13 맥박";          
                SQL = SQL + ComNum.VBLF + "     , ''                                                                                       AS UCODENM  -- 14 유해인자명";     // TODO : 공백처리
                SQL = SQL + ComNum.VBLF + "     , KOSMOS_PMPA.FC_BAS_VIPINFO(HEA.PTNO) || '/' || PAT.VIPREMARK                             AS VIP      -- 15 VIP ";
                SQL = SQL + ComNum.VBLF + "     , PAT.GBPRIVACY                                                                            AS PRIVACY  -- 16 개인정보 ";
                SQL = SQL + ComNum.VBLF + "       , ''                                                                                     AS CVR      -- 17 CVR ";           // TODO : CVR
                SQL = SQL + ComNum.VBLF + "     , KOSMOS_OCS.FC_DRUG_ADR_CHK(PAT.PTNO)                                                     AS ADR      -- 18 ADR";
                SQL = SQL + ComNum.VBLF + "     , KOSMOS_OCS.FC_ETC_ALLERGY_MST_CHK(PAT.PTNO)                                              AS ALLEGY   -- 19 알러지";
                SQL = SQL + ComNum.VBLF + "     , KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(PAT.PTNO, TRUNC(SYSDATE))                              AS FALL     -- 20 낙상";
                //SQL = SQL + ComNum.VBLF +  "    , KOSMOS_OCS.FC_NUR_MASTER_FIRE(IPD.IPDNO)                                                 AS FIRE     -- 21 화재 ";
                SQL = SQL + ComNum.VBLF + "     , ''                                                                                       AS FIRE     -- 21 화재 ";          // TODO : 화재(내시경실 정보필요)
                SQL = SQL + ComNum.VBLF + "     , HEA.ACTMEMO                                                                              AS REMARK   -- 22 환자참고사항 ";
                SQL = SQL + ComNum.VBLF + "     , HEA.EXAMREMARK                                                                           AS EXREMARK -- 23 검사요령 ";
                SQL = SQL + ComNum.VBLF + "     , ''                                                                                       AS PASTILL  -- 24 과거력 ";
                SQL = SQL + ComNum.VBLF + "    , KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(HEA.PTNO, TRUNC(HEA.SDATE))                       AS INFE_IMG -- 25 감염이미지";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.HEA_JEPSU    HEA                                                                                              ";
                SQL = SQL + ComNum.VBLF + "     , KOSMOS_PMPA.HIC_PATIENT  PAT                                                                                              ";
                SQL = SQL + ComNum.VBLF + " WHERE 1    = 1                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "   AND HEA.PTNO = " + ComFunc.covSqlstr(Ptno, false);
                SQL = SQL + ComNum.VBLF + "   AND HEA.PTNO = PAT.PTNO                                   ";
                SQL = SQL + ComNum.VBLF + "   AND HEA.SDATE = " + ComFunc.covSqlDate(bDate, false);
                SQL = SQL + ComNum.VBLF + "   AND HEA.GJJONG = '" + GjJong + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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

        private void SetSpd(DataTable dt)
        {
            SetItemClear();

            #region Display
            if (dt.Rows[0][enmHC_PAT_INFO.PANO.ToString()].ToString().Trim() != "")
            {
                lblPTNO.Text = dt.Rows[0][enmHC_PAT_INFO.PANO.ToString()].ToString().Trim();            // 환자번호
                lblPTNO.ForeColor = PtOnForeColor;
                lblPTNO.BackColor = PtOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.AGE.ToString()].ToString().Trim() != "")
            {
                lblAGE.Text = dt.Rows[0][enmHC_PAT_INFO.AGE.ToString()].ToString().Trim();           // 나이
                lblAGE.ForeColor = InfoOnForeColor;
                lblAGE.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.SEX.ToString()].ToString().Trim() != "")
            {
                lblSEX.Text = dt.Rows[0][enmHC_PAT_INFO.SEX.ToString()].ToString().Trim();             // 성별
                lblSEX.ForeColor = InfoOnForeColor;
                lblSEX.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.SNAME.ToString()].ToString().Trim() != "")
            {
                lblSNAME.Text = dt.Rows[0][enmHC_PAT_INFO.SNAME.ToString()].ToString().Trim();            // 성명
                lblSNAME.ForeColor = InfoOnForeColor;
                lblSNAME.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.JUMIN.ToString()].ToString().Trim() != "")                //생년월일
            {
                lblBIRTH.Text = dt.Rows[0][enmHC_PAT_INFO.JUMIN.ToString()].ToString().Trim();
                lblBIRTH.ForeColor = InfoOnForeColor;
                lblBIRTH.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.JEPDATE.ToString()].ToString().Trim() != "")
            {
                lblJEPDATE.Text = dt.Rows[0][enmHC_PAT_INFO.JEPDATE.ToString()].ToString();          // 검진일자
                lblJEPDATE.ForeColor = InfoOnForeColor;
                lblJEPDATE.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.GJJONG.ToString()].ToString().Trim() != "")
            {
                lblGJNAME.Text = VB.Left(dt.Rows[0][enmHC_PAT_INFO.GJJONG.ToString()].ToString(), 10);    // 검진종류
                lblGJNAME.ForeColor = InfoOnForeColor;
                lblGJNAME.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.GBGJYN.ToString()].ToString().Trim() != "")
            {
                lblGJJONG.Text = dt.Rows[0][enmHC_PAT_INFO.GBGJYN.ToString()].ToString();            // 검진여부 
                lblGJJONG.ForeColor = InfoOnForeColor;
                lblGJJONG.BackColor = InfoOnBackColor;
            }


            if (dt.Rows[0][enmHC_PAT_INFO.LTDNAME.ToString()].ToString().Trim() != "")
            {
                lblLTDNAME.Text = dt.Rows[0][enmHC_PAT_INFO.LTDNAME.ToString()].ToString();         // 사업장명
                lblLTDNAME.ForeColor = InfoOnForeColor;
                lblLTDNAME.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.UCODENM.ToString()].ToString().Trim() != "")
            {
                lblUCODE.Text = cHcMain.UCode_Names_Display(dt.Rows[0][enmHC_PAT_INFO.UCODENM.ToString()].ToString());
                //lblUCODE.Text = dt.Rows[0][enmHC_PAT_INFO.UCODENM.ToString()].ToString();         // 유해인자
                lblUCODE.ForeColor = InfoOnForeColor;
                lblUCODE.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.VIP.ToString()].ToString().Trim() != "" && dt.Rows[0][enmHC_PAT_INFO.VIP.ToString()].ToString().Trim() != "/")
            {
                lblVIP.Text = dt.Rows[0][enmHC_PAT_INFO.VIP.ToString()].ToString();          // VIP
                lblVIP.ForeColor = InfoOnForeColor;
                lblVIP.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.PRIVACY.ToString()].ToString().Trim() != "")
            {
                lblSECRET.Tag = dt.Rows[0][enmHC_PAT_INFO.PRIVACY.ToString()].ToString().Trim();
                lblSECRET.Text = "동의함";          // 개인정보
                lblSECRET.ForeColor = InfoOnForeColor;
                lblSECRET.BackColor = InfoOnBackColor;
            }
            else
            {
                lblSECRET.Text = "정보요청";          // 개인정보여부
                lblSECRET.ForeColor = InfoOnForeColor;
                lblSECRET.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.CVR.ToString()].ToString().Trim() != "")
            {
                //lblCVR.Text = dt.Rows[0][enmHC_PAT_INFO.CVR.ToString()].ToString();          // CVR
                lblCVR.ForeColor = InfoOnForeColor;
                lblCVR.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.ADR.ToString()].ToString().Trim() == "Y")
            {
                //lblADR.Text = dt.Rows[0][enmHC_PAT_INFO.ADR.ToString()].ToString();          // ADR
                lblADR.ForeColor = InfoOnForeColor;
                lblADR.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.ALLEGY.ToString()].ToString().Trim() == "Y")
            {
                //lblALLERGY.Text = dt.Rows[0][enmHC_PAT_INFO.ALLEGY.ToString()].ToString();          // ALLERGY
                lblALLERGY.ForeColor = InfoOnForeColor;
                lblALLERGY.BackColor = InfoOnBackColor;
            }

            if (dt.Rows[0][enmHC_PAT_INFO.FALL.ToString()].ToString().Trim() == "Y")
            {
                //lblFALL.Text = dt.Rows[0][enmHC_PAT_INFO.FALL.ToString()].ToString();           // 낙상
                lblFALL.ForeColor = InfoOnForeColor;
                lblFALL.BackColor = InfoOnBackColor;
            }

            if (VB.Val(dt.Rows[0][enmHC_PAT_INFO.FIRE.ToString()].ToString().Trim()) > 0)
            {
                //lblBURN.Text = dt.Rows[0][enmHC_PAT_INFO.FIRE.ToString()].ToString();         // 화재
                lblBURN.ForeColor = InfoOnForeColor;
                lblBURN.BackColor = InfoOnBackColor;
            }

            if (VB.Val(dt.Rows[0][enmHC_PAT_INFO.REMARK.ToString()].ToString().Trim()) > 0)
            {
                lblPATREMARK.Text = dt.Rows[0][enmHC_PAT_INFO.REMARK.ToString()].ToString();         // 참고사항
                lblPATREMARK.ForeColor = InfoOnForeColor;
                lblPATREMARK.BackColor = InfoOnBackColor;
            }

            if (VB.Val(dt.Rows[0][enmHC_PAT_INFO.EXREMARK.ToString()].ToString().Trim()) > 0)
            {
                lblEXAM.Text = dt.Rows[0][enmHC_PAT_INFO.EXREMARK.ToString()].ToString();         // 검사요령
                lblEXAM.ForeColor = InfoOnForeColor;
                lblEXAM.BackColor = InfoOnBackColor;
            }

            if (VB.Val(dt.Rows[0][enmHC_PAT_INFO.PASTILL.ToString()].ToString().Trim()) > 0)
            {
                lblGWA.Text = dt.Rows[0][enmHC_PAT_INFO.PASTILL.ToString()].ToString();         // 과거력
                lblGWA.ForeColor = InfoOnForeColor;
                lblGWA.BackColor = InfoOnBackColor;
            }
            
            //비말
            if (VB.Mid(dt.Rows[0][enmHC_PAT_INFO.INFE_IMG.ToString()].ToString().Trim(), 5, 1) == "1")
            {
                lblBIMAL.ForeColor = InfoOnForeColor;
                lblBIMAL.BackColor = InfoOnBackColor;
            }

            //혈액
            if (VB.Mid(dt.Rows[0][enmHC_PAT_INFO.INFE_IMG.ToString()].ToString().Trim(), 1, 1) == "1")
            {
                lblBLOOD.ForeColor = InfoOnForeColor;
                lblBLOOD.BackColor = InfoOnBackColor;
            }

            //해외
            if (VB.Mid(dt.Rows[0][enmHC_PAT_INFO.INFE_IMG.ToString()].ToString().Trim(), 7, 1) == "1")
            {
                lblFOREGIN.ForeColor = InfoOnForeColor;
                lblFOREGIN.BackColor = InfoOnBackColor;              
            }

            #endregion

            //if (dt.Rows[0][enmOPD_MASTER_MAIN.ALLEGY.ToString()].ToString().Trim() == "Y")
            //{
            //    lblALLEGY1.Text = "알러지";         // 알러지
            //    lblALLEGY1.ForeColor = InfoOnForeColor;
            //    lblALLEGY1.BackColor = InfoOnBackColor;
            //}

            //if (dt.Rows[0][enmOPD_MASTER_MAIN.FIRE.ToString()].ToString().Trim() != "")
            //{
            //    if (dt.Rows[0][enmOPD_MASTER_MAIN.FIRE.ToString()].ToString() == "Bed")
            //    { lblFIRE1.Text = "침대"; }
            //    else if (dt.Rows[0][enmOPD_MASTER_MAIN.FIRE.ToString()].ToString() == "W/C")
            //    { lblFIRE1.Text = "휠체어"; }
            //    else if (dt.Rows[0][enmOPD_MASTER_MAIN.FIRE.ToString()].ToString() == "Walking")
            //    { lblFIRE1.Text = "보행"; }

            //    lblFIRE1.ForeColor = InfoOnForeColor;
            //    lblFIRE1.BackColor = InfoOnBackColor;
            //}

            ////CI
            ////if(dt.Rows[0]["CI"].ToString().Trim() != "")
            ////{
            ////    lblCI1.Text = "";
            ////    lblCI2.Text = "";
            ////    lblCI1.ForeColor = InfoOnForeColor;
            ////    lblCI2.ForeColor = InfoOnBackColor;
            ////}

            //if (dt.Rows[0][enmOPD_MASTER_MAIN.NST.ToString()].ToString().Trim() != "N")
            //{
            //    //lblNST1.Text = dt.Rows[0][enmOPD_MASTER_MAIN.NST.ToString()].ToString();            // NST
            //    lblNST1.ForeColor = InfoOnForeColor;
            //    lblNST1.BackColor = InfoOnBackColor;
            //}

            //if (dt.Rows[0][enmOPD_MASTER_MAIN.ILLS.ToString()].ToString().Trim() != "")
            //{
            //    cboDIS1.Items.Clear();
            //    cboDIS1.Items.Add(dt.Rows[0][enmOPD_MASTER_MAIN.ILLS.ToString()].ToString());            // 진단명
            //    cboDIS1.SelectedIndex = 0;

            //    cboDIS1.ForeColor = InfoOnForeColor;
            //}

            //if (clsType.User.JobGroup == "JOB013013" || clsType.User.JobGroup == "JOB013015" || clsType.User.JobGroup == "JOB013016")
            //{
            //    SetIpdOpDate(lblPTNO1.Text.Trim(), lblINDATE1.Text.Trim());
            //}
            //else
            //{
            //    if (dt.Rows[0][enmOPD_MASTER_MAIN.OPDATE.ToString()].ToString().Trim() != "")
            //    {
            //        cboOP1.Items.Clear();
            //        cboOP1.Items.Add("POD[ " +
            //            VB.DateDiff("d", dt.Rows[0][enmOPD_MASTER_MAIN.OPDATE.ToString()].ToString()
            //            , ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")) + " ] "
            //            + dt.Rows[0][enmOPD_MASTER_MAIN.OPNM.ToString()].ToString()
            //            + " (" + dt.Rows[0][enmOPD_MASTER_MAIN.OPDATE.ToString()].ToString() + ")");         // 수술일자
            //        cboOP1.SelectedIndex = 0;

            //        cboOP1.ForeColor = InfoOnForeColor;
            //    }
            //}
        }
    }
}
