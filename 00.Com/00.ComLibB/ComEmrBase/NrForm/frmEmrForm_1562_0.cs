using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmEmrForm_1562_0 : Form
    {

        #region // 폼에 사용하는 변수를 코딩하는 부분
        //private frmEmrInitList frmEmrInitListEvent;
        #endregion

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;

        public string mstrFormNo = "1562";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public EmrPatient p = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "W";
        #endregion

        #region // TopMenu관련 선언
        private usFormTopMenu usFormTopMenuEvent;
        private usTimeSet usTimeSetEvent;
        public ComboBox mMaskBox = null;
        #endregion

        #region // TopMenu관련 이벤트 처리 함수

        private void usFormTopMenuEvent_SetTimeCheckShow(ComboBox mkText)
        {
            mMaskBox = mkText;
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            usTimeSetEvent = new usTimeSet();
            usTimeSetEvent.rSetTime += new usTimeSet.SetTime(usTimeSetEvent_SetTime);
            usTimeSetEvent.rEventClosed += new usTimeSet.EventClosed(usTimeSetEvent_EventClosed);
            this.Controls.Add(usTimeSetEvent);
            usTimeSetEvent.Top = mMaskBox.Top - 5;
            usTimeSetEvent.Left = mMaskBox.Left;
            usTimeSetEvent.BringToFront();
        }

        private void usTimeSetEvent_SetTime(string strText)
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            usFormTopMenuEvent.txtMedFrTime.Text = strText;
        }

        private void usTimeSetEvent_EventClosed()
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
        }

        private void usFormTopMenuEvent_SetSave(string strFrDate, string strFrTime)
        {
            double dblEmrNo = 0;
            dblEmrNo = pSaveData();
            if (dblEmrNo > 0)
            {
                this.Close();
            }
        }

        private void usFormTopMenuEvent_SetDel(string strFrDate, string strFrTime)
        {
            pDelData();
        }
        private void usFormTopMenuEvent_SetClear()
        {
            mstrEmrNo = "0";
            pClearForm();
        }
        private void usFormTopMenuEvent_SetPrint()
        {
            pPrintForm();
        }
        private void usFormTopMenuEvent_EventClosed()
        {
            //아무것도 하지 않는다.
        }

        #endregion

        #region //외부에서 이벤트 받아서 처리 클리어, 저장, 삭제, 프린터
        /// <summary>
        /// 환자 받아서 기록지를 초기화 한다.
        /// </summary>
        public void gPatientinfoRecive(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;

            //폼을 클리어하고 기록지 작성 정보등을 갱신한다.
            pClearForm();
            //기록지 정보를 세팅한다.
            pSetEmrInfo();
        }

        /// <summary>
        /// 폼이 로드할때 초기 세팅을 한다
        /// </summary>
        public void pInitForm()
        {
            //텍스트 박스에 상용구 이벤트를 세팅한다

            //----TopMenu관련 이벤트 생성 및 선언
            usFormTopMenuEvent = new usFormTopMenu();
            //usBtnShow(usFormTopMenuEvent, "mbtnSave");
            usFormTopMenuEvent.rSetTimeCheckShow += new usFormTopMenu.SetTimeCheckShow(usFormTopMenuEvent_SetTimeCheckShow);
            usFormTopMenuEvent.rSetSave += new usFormTopMenu.SetSave(usFormTopMenuEvent_SetSave);
            usFormTopMenuEvent.rSetDel += new usFormTopMenu.SetDel(usFormTopMenuEvent_SetDel);
            usFormTopMenuEvent.rSetClear += new usFormTopMenu.SetClear(usFormTopMenuEvent_SetClear);
            usFormTopMenuEvent.rSetPrint += new usFormTopMenu.SetPrint(usFormTopMenuEvent_SetPrint);
            usFormTopMenuEvent.rEventClosed += new usFormTopMenu.EventClosed(usFormTopMenuEvent_EventClosed);

            this.Controls.Add(usFormTopMenuEvent);
            usFormTopMenuEvent.Parent = this.panTopMenu;
            usFormTopMenuEvent.Dock = DockStyle.Fill;
            //--------------------------

            pClearForm();
            pSetEmrInfo();
        }

        private void pSetEmrInfo()
        {
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);
            usFormTopMenuEvent.mbtnPrint.Visible = false;

            //EMRNO가 있으면 기록 정보를 세팅을 한다.
            pLoadEmrChartInfo();

        }

        /// <summary>
        /// 폼별 특수한 초기화세팅이 필요할 경우 코딩.
        /// </summary>
        public void pInitFormSpc()
        {

        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double pSaveData()
        {
            double dblEmrNo = 0;

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (txtUSEID.Text != clsType.User.Sabun)
                {
                    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "변경할 수 없습니다.");
                    return 0;
                }
            }

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (ComFunc.MsgBoxQEx(this, "기존 내용을 변경하시겠습니까?", "EMR") == System.Windows.Forms.DialogResult.No)
                {
                    return dblEmrNo;
                }
            }

            dblEmrNo = pSaveEmrData();
            if (dblEmrNo == 0)
            {
                ComFunc.MsgBoxEx(this, "저장중 에러가 발생했습니다.");
            }
            else
            {
                txtUSEID.Clear();
                mstrEmrNo = "0";
                QueryChartList();
            }
            return dblEmrNo;
        }

        #endregion

        //==========================================================================================//
        //=============================== 아래부터 코딩을 하면 됨 ===================================//
        //=========================================================================================//

        #region // 기록지 클리어, 저장, 삭제, 프린터
        /// <summary>
        /// 화면 정리
        /// </summary>
        public void pClearForm()
        {
            //모든 컨트롤을 초기화 한다.
            ssWrite_Sheet1.RowCount = 0;
            ssWrite_Sheet1.RowCount = 1;
            ssWrite_Sheet1.SetRowHeight(-1, 24);
            //ssList_Sheet1.RowCount = 0;

            //시간 세팅을 한다.
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            usFormTopMenuEvent.txtMedFrTime.Text = ComFunc.FormatStrToDate(VB.Mid(strCurDateTime, 9, 4), "M");
            usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            usFormTopMenuEvent.txtMedFrTime.Enabled = true;
            usFormTopMenuEvent.mbtnTime.Visible = true;
            //자격에따라서 버튼을 설정을 한다.
            pClearFormExcept();
        }

        /// <summary>
        /// 폼별로 EMR 작성 내역을 화면에 보여준다.
        /// </summary>
        private void pLoadEmrChartInfo()
        {
            //mstrEmrNo = "1455366";
            if (VB.Val(mstrEmrNo) == 0)
            {
                return;
            }
            //출력한 기록지는 수정이 안되도록 막는다.
            //bool blnPRNYN = clsEmrQuery.GetPrnYnInfo(mstrEmrNo);
            //if (blnPRNYN == true)
            //{
            //    clsEmrFunc.usBtnHide(usFormTopMenuEvent);
            //}

            LoadFlowData();
            //clsXML.LoadDataXML(this, mstrEmrNo, false);
        }

        private void LoadFlowData()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            Cursor.Current = Cursors.WaitCursor;

            string strCHARTDATE = "";
            //string strCHARTA = "";
            string strCHARTTIME = "";
            string strUSEID = "";
            string strPRNYN = "";
            string strUSERFORMNO = "";

            ssWrite_Sheet1.RowCount = 0;
            ssWrite_Sheet1.RowCount = 1;
            ssWrite_Sheet1.SetRowHeight(-1, 24);

            SQL = " SELECT ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTDATE, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTTIME,    ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it1') AS COL1, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it2') AS COL2, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it3') AS COL3, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it4') AS COL4, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it5') AS COL5, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it6') AS COL6, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it7') AS COL7, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it8') AS COL8, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it9') AS COL9, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it14') AS COL14, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it10') AS COL10, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it11') AS COL11, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it12') AS COL12, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it13') AS COL13, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it121') AS COL21, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it150') AS COL150, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it274') AS COL274, ";
            SQL = SQL + ComNum.VBLF + "         A.EMRNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.ACPNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.INOUTCLS,         ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDEPTCD,        ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDRCD,          ";
            SQL = SQL + ComNum.VBLF + "         A.USEID,            ";
            SQL = SQL + ComNum.VBLF + "         A.MEDFRDATE,        ";
            SQL = SQL + ComNum.VBLF + "         a.MedFrTime         ";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.EMRXML A, KOSMOS_EMR.EMRXMLMST B ";
            SQL = SQL + ComNum.VBLF + " WHERE a.EMRNO = b.EMRNO ";
            SQL = SQL + ComNum.VBLF + "   AND B.FORMNO = 1562 ";
            SQL = SQL + ComNum.VBLF + "   AND B.PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND A.EMRNO = " + VB.Val(mstrEmrNo);

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (dt == null)
            {
                MessageBox.Show(new Form() { TopMost = true }, "조회중 문제가 발생했습니다");
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }
            strCHARTDATE = (dt.Rows[0]["CHARTDATE"].ToString() + "").Trim();
            strCHARTTIME = (dt.Rows[0]["CHARTTIME"].ToString() + "").Trim();
            strUSEID = (dt.Rows[0]["USEID"].ToString() + "").Trim();
            //strPRNYN = (dt.Rows[0]["PRNYN"].ToString() + "").Trim();
            txtDAEmrNo.Text = mstrEmrNo;
            txtCHARTDATE.Text = strCHARTDATE;
            txtCHARTTIME.Text = strCHARTTIME;
            txtUSEID.Text = strUSEID;
            strUSERFORMNO = "1562";
            
            ssWrite_Sheet1.Cells[0, 0].Text = (dt.Rows[0]["COL1"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 1].Text = (dt.Rows[0]["COL2"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 2].Text = (dt.Rows[0]["COL3"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 3].Text = (dt.Rows[0]["COL4"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 4].Text = (dt.Rows[0]["COL5"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 5].Text = (dt.Rows[0]["COL6"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 6].Text = (dt.Rows[0]["COL7"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 7].Text = (dt.Rows[0]["COL8"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 8].Text = (dt.Rows[0]["COL9"].ToString() + "").Trim();

            ssWrite_Sheet1.Cells[0, 9].Text = (dt.Rows[0]["COL14"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 10].Text = (dt.Rows[0]["COL10"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 11].Text = (dt.Rows[0]["COL11"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 12].Text = (dt.Rows[0]["COL12"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 13].Text = (dt.Rows[0]["COL13"].ToString() + "").Trim();

            ssWrite_Sheet1.Cells[0, 14].Text = (dt.Rows[0]["COL21"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 15].Text = (dt.Rows[0]["COL150"].ToString() + "").Trim();
            ssWrite_Sheet1.Cells[0, 16].Text = (dt.Rows[0]["COL274"].ToString() + "").Trim();


            dt.Dispose();
            dt = null;

            //Progress Note
            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(strCHARTDATE, "D"));
            usFormTopMenuEvent.txtMedFrTime.Text = ComFunc.FormatStrToDate(strCHARTTIME, "T");

            usFormTopMenuEvent.dtMedFrDate.Enabled = false;

            if (clsType.User.IdNumber != strUSEID)
            {
                usFormTopMenuEvent.mbtnSave.Visible = false;
                usFormTopMenuEvent.mbtnDelete.Visible = false;
            }
            else
            {
                if (strPRNYN == "1" && strUSERFORMNO != "0")
                {
                    usFormTopMenuEvent.mbtnSave.Visible = false;
                    usFormTopMenuEvent.mbtnDelete.Visible = false;
                }
                else
                {
                    usFormTopMenuEvent.mbtnSave.Visible = true;
                    usFormTopMenuEvent.mbtnDelete.Visible = true;
                }
            }

            Cursor.Current = Cursors.Default;
        }
        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private double pSaveEmrData()
        {
            double dblEmrNo = 0;
            //double dblEmrCertNo = 0;
            string strChartDate = VB.Format(usFormTopMenuEvent.dtMedFrDate.Value, "yyyyMMdd");
            string strChartTime = VB.Replace(usFormTopMenuEvent.txtMedFrTime.Text, ":", "");

            string strXML = "";
            string strXmlHead = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\r\n" + "<chart>" + "\r\n";
            string strXmlTail = "\r\n" + "</chart>";
            string strChart = "";
            
            strChart = strChart + ComNum.VBLF + "<it1 type = \"inputText\" label = \"Duty\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 0].Text.Trim() + "]]></it1>";
            strChart = strChart + ComNum.VBLF + "<it2 type = \"inputText\" label = \"구분\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 1].Text.Trim() + "]]></it2>";
            strChart = strChart + ComNum.VBLF + "<it3 type = \"inputText\" label = \"혈압(Sys)\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 2].Text.Trim() + "]]></it3>";
            strChart = strChart + ComNum.VBLF + "<it4 type = \"inputText\" label = \"혈압(Dia)\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 3].Text.Trim() + "]]></it4>";
            strChart = strChart + ComNum.VBLF + "<it5 type = \"inputText\" label = \"혈압측정위치\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 4].Text.Trim() + "]]></it5>";
            strChart = strChart + ComNum.VBLF + "<it6 type = \"inputText\" label = \"맥박\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 5].Text.Trim() + "]]></it6>";
            strChart = strChart + ComNum.VBLF + "<it7 type = \"inputText\" label = \"호흡\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 6].Text.Trim() + "]]></it7>";
            strChart = strChart + ComNum.VBLF + "<it8 type = \"inputText\" label = \"체온\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 7].Text.Trim() + "]]></it8>";
            strChart = strChart + ComNum.VBLF + "<it9 type = \"inputText\" label = \"체온측정위치\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 8].Text.Trim() + "]]></it9>";
            strChart = strChart + ComNum.VBLF + "<it14 type = \"inputText\" label = \"SpO2\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 9].Text.Trim() + "]]></it14>";
            strChart = strChart + ComNum.VBLF + "<it10 type = \"inputText\" label = \"체중\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 10].Text.Trim() + "]]></it10>";
            strChart = strChart + ComNum.VBLF + "<it11 type = \"inputText\" label = \"신장\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 11].Text.Trim() + "]]></it11>";
            strChart = strChart + ComNum.VBLF + "<it12 type = \"inputText\" label = \"배둘레\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 12].Text.Trim() + "]]></it12>";
            strChart = strChart + ComNum.VBLF + "<it13 type = \"inputText\" label = \"FHR\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 13].Text.Trim() + "]]></it13>";
            strChart = strChart + ComNum.VBLF + "<it121 type = \"inputText\" label = \"머리둘레\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 14].Text.Trim() + "]]></it121>";
            strChart = strChart + ComNum.VBLF + "<it150 type = \"inputText\" label = \"가슴둘레\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 15].Text.Trim() + "]]></it150>";
            strChart = strChart + ComNum.VBLF + "<it274 type = \"inputText\" label = \"비고\"><![CDATA[" + ssWrite_Sheet1.Cells[0, 16].Text.Trim() + "]]></it274>";
            
            strXML = strXmlHead + strChart + strXmlTail;

            //EmrPatient pTmp = null;
            //pTmp = clsEmrChart.ClearPatient();

            //if (VB.Val(mstrEmrNo) == 0)
            //{
            //    pTmp = p;
            //}
            //else
            //{
            //    pTmp = clsEmrChart.SetEmrPatInfoXml(mstrEmrNo);
            //}
            
            dblEmrNo = gSaveEmrXml(mstrFormNo, mstrUpdateNo, mstrEmrNo, strChartDate, strChartTime, strXML);

            if (dblEmrNo != 0)
            {
                //TODO 전자인증
                //if (clsType.gHosInfo.strEmrCertUseYn == "1")
                //{
                //    dblEmrCertNo = clsEmrCerti.SaveEmrCert(Convert.ToString(dblEmrNo), mstrEmrNo, strXML, strChartDate, strChartTime);
                //    if (dblEmrCertNo == 0)
                //    {
                //        MessageBox.Show(new Form() { TopMost = true }, "인증중 에러가 발생했습니다." + ComNum.VBLF + "추후 인증을 실시해 주시기 바랍니다.");
                //    }
                //}
            }
            return dblEmrNo;
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        public bool pDelData()
        {
            if (VB.Val(mstrEmrNo) == 0)
            {
                return false;
            }

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (txtUSEID.Text != clsType.User.IdNumber)
                {
                    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                    return false;
                }
            }

            if (gDeleteEmrXml(mstrEmrNo, clsType.User.IdNumber) == true)
            {
                mstrEmrNo = "0";
                pClearForm();
                QueryChartList();
            }
            return true;
        }

        /// <summary>
        /// 기록지를 출력한다.
        /// </summary>
        public void pPrintForm()
        {

        }

        /// <summary>
        /// 클리어하고 폼별로 별요한것 기본 세팅
        /// </summary>
        private void pClearFormExcept()
        {
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);
            if (clsType.User.AuAWRITE == "1")
            {
                clsEmrFunc.usBtnShow(usFormTopMenuEvent, "mbtnClear");
                clsEmrFunc.usBtnShow(usFormTopMenuEvent, "mbtnSave");
                clsEmrFunc.usBtnShow(usFormTopMenuEvent, "mbtnDelete");
            }
            // 내원일을 세팅한다.
            //clsEmrFunc.SetMedFrEndDate(clsDB.DbCon, mstrEmrNo, p, dtpFrDate, dtpEndDate);
        }

        #endregion

        public frmEmrForm_1562_0()
        {
            InitializeComponent();
        }

        public frmEmrForm_1562_0(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            InitializeComponent();
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
        }

        private void frmEmrForm_1562_0_Load(object sender, EventArgs e)
        {

            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "N"); //폼 기본값 세팅 등    

            ssList.Dock = DockStyle.Fill;
            webGRAPH.Dock = DockStyle.Fill;
            webGRAPH.Visible = false;

            ssList_Sheet1.RowCount = 0;
            //clsSpread.gSpreadEnter_NextColumn(ssWrite);
            pInitForm();
            usFormTopMenuEvent.mbtnSaveTemp.Visible = false;

            SetUserAut();
            
            string strCurDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"),"D");
            dtpEndDate.Value = Convert.ToDateTime(strCurDate);

            if (p.inOutCls == "I")
            {
                dtpFrDate.Value = VB.DateAdd("D", -7, strCurDate);
            }
            else
            {
                dtpFrDate.Value = VB.DateAdd("m", -12, strCurDate);
            }

            ssWrite_Sheet1.Cells[0, 1].Text = "정규";
            ssWrite_Sheet1.Cells[0, 4].Text = "Rt Arm";
            ssWrite_Sheet1.Cells[0, 8].Text = "고막";
            clsSpread.gSpreadEnter_NextCol(ssWrite);

            lblPatInfo.Text = p.ptName + "[" + p.ptNo + "] " + p.sex + "/" + p.age;

            QueryChartList();

            ssWrite_Sheet1.SetActiveCell(0, 2);
            ssWrite.Focus();
            Application.DoEvents();

            SendPatInfoWeb();


        }

        private void SendPatInfoWeb()
        {
            string gEmrUrl = "http://192.168.100.33:8090/";
            string EmrGraph = "Emr/emrxmlInfo.mts?"; //환자정보전달
            string strURL = gEmrUrl + EmrGraph;

            strURL = strURL +
            "ptNo=" + p.ptNo + "&" +
            "acpNo=" + p.acpNo + "&" +
            "inOutCls=" + p.inOutCls + "&" +
            "medFrDate=" + p.medFrDate + "&" +
            "medFrTime=" + p.medFrTime + "&" +
            "medEndDate=" + p.medEndDate + "&" +
            "medEndTime=" + p.medEndTime + "&" +
            "medDeptCd=" + p.medDeptCd + "&" +
            "medDeptName=" + "" + "&" +
            "medDrCd=" + p.medDrCd + "&" +
            "gubun=" + "1" + "&" +
            "formNo=" + "1562";

            webGRAPH.Navigate(strURL);
            while (webGRAPH.IsBusy == true)
            {
                Application.DoEvents();
            }
            for (int intWeb = 0; intWeb < 40000; intWeb++)
            {
                Application.DoEvents();
            }
        }

        private void SetUserAut()
        {
            if (clsType.User.AuAWRITE == "1")
            {
                panTopMenu.Visible = true;
                panChart.Visible = true;
            }
            else
            {
                panTopMenu.Visible = false;
                panChart.Visible = false;
            }
            //if (clsType.User.AuAPRINT == "1")
            //{
            //    mbtnPrint.Visible = true;
            //}
            //else
            //{
            //    mbtnPrint.Visible = true;
            //}
        }

        private void mbtnSearchAll_Click(object sender, EventArgs e)
        {
            QueryChartList();
        }

        private void QueryChartList()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            
            SQL = " SELECT ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTDATE, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTTIME,    ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it1') AS COL1, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it2') AS COL2, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it3') AS COL3, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it4') AS COL4, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it5') AS COL5, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it6') AS COL6, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it7') AS COL7, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it8') AS COL8, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it9') AS COL9, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it14') AS COL14, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it10') AS COL10, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it11') AS COL11, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it12') AS COL12, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it13') AS COL13, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it121') AS COL21, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it150') AS COL150, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it274') AS COL274, ";
            SQL = SQL + ComNum.VBLF + "         A.EMRNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.ACPNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.INOUTCLS,         ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDEPTCD,        ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDRCD,          ";
            SQL = SQL + ComNum.VBLF + "         A.USEID,            ";
            SQL = SQL + ComNum.VBLF + "         A.MEDFRDATE,        ";
            SQL = SQL + ComNum.VBLF + "         A.MedFrTime         ";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.EMRXML A, KOSMOS_EMR.EMRXMLMST B ";
            SQL = SQL + ComNum.VBLF + " WHERE a.EMRNO = b.EMRNO ";
            SQL = SQL + ComNum.VBLF + "   AND B.FORMNO = 1562 ";
            SQL = SQL + ComNum.VBLF + "   AND B.PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE >= '" + VB.Format(dtpFrDate.Value, "yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE <= '" + VB.Format(dtpEndDate.Value, "yyyyMMdd") + "'";

            if (chkDesc.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY A.CHARTDATE DESC, A.CHARTTIME DESC";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY A.CHARTDATE ASC, A.CHARTTIME ASC";
            }


            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (dt == null)
            {
                MessageBox.Show(new Form() { TopMost = true }, "조회중 문제가 발생했습니다");
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }
            ssList_Sheet1.RowCount = dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D");
                ssList_Sheet1.Cells[i, 1].Text = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M");

                ssList_Sheet1.Cells[i, 2].Text = (dt.Rows[i]["COL1"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 3].Text = (dt.Rows[i]["COL2"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["COL3"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 5].Text = (dt.Rows[i]["COL4"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 6].Text = (dt.Rows[i]["COL5"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 7].Text = (dt.Rows[i]["COL6"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 8].Text = (dt.Rows[i]["COL7"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 9].Text = (dt.Rows[i]["COL8"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 10].Text = (dt.Rows[i]["COL9"].ToString() + "").Trim();

                ssList_Sheet1.Cells[i, 11].Text = (dt.Rows[i]["COL14"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 12].Text = (dt.Rows[i]["COL10"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 13].Text = (dt.Rows[i]["COL11"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 14].Text = (dt.Rows[i]["COL12"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 15].Text = (dt.Rows[i]["COL13"].ToString() + "").Trim();

                ssList_Sheet1.Cells[i, 16].Text = (dt.Rows[i]["COL21"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 17].Text = (dt.Rows[i]["COL150"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 18].Text = (dt.Rows[i]["COL274"].ToString() + "").Trim();

                ssList_Sheet1.Cells[i, 19].Text = clsVbfunc.GetInSaName(clsDB.DbCon, (dt.Rows[i]["USEID"].ToString() + "").Trim());
                //ssList_Sheet1.Cells[i, 20].Text = (dt.Rows[i]["COL150"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 21].Text = (dt.Rows[i]["EMRNO"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 22].Text = (dt.Rows[i]["USEID"].ToString() + "").Trim();
                //ssList_Sheet1.Cells[i, 23].Text = (dt.Rows[i]["COL27"].ToString() + "").Trim();                
                ssList_Sheet1.SetRowHeight(i, 24);

                double rtnVal = VB.Val(ssList_Sheet1.Cells[i, 4].Text);
                if (rtnVal >= 140 || rtnVal < 80) // 혈압
                {
                    ssList_Sheet1.Cells[i, 4].ForeColor = Color.Red;
                }

                rtnVal = VB.Val(ssList_Sheet1.Cells[i, 5].Text);
                if (rtnVal >= 90 || rtnVal < 60) // 혈압
                {
                    ssList_Sheet1.Cells[i, 5].ForeColor = Color.Red;
                }

                rtnVal = VB.Val(ssList_Sheet1.Cells[i, 7].Text);
                if (rtnVal >= 100 || rtnVal < 60) //맥박
                {
                    ssList_Sheet1.Cells[i, 7].ForeColor = Color.Red;
                }

                rtnVal = VB.Val(ssList_Sheet1.Cells[i, 8].Text);
                if (rtnVal >= 21 || rtnVal < 12) //호흡
                {
                    ssList_Sheet1.Cells[i, 8].ForeColor = Color.Red;
                }

                rtnVal = VB.Val(ssList_Sheet1.Cells[i, 9].Text);
                if (rtnVal >= 37.5 || rtnVal < 36.5) //체온
                {
                    ssList_Sheet1.Cells[i, 9].ForeColor = Color.Red;
                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }


        private bool gDeleteEmrXml(string strEmrNo, string strUseId)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strFormNo = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = " SELECT FORMNO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (dt == null)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show(new Form() { TopMost = true }, "조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    strFormNo = (dt.Rows[0]["FORMNO"].ToString() + "").Trim();
                }
                dt.Dispose();
                dt = null;

                if (strFormNo != "")
                {
                    SQL = " SELECT FORMNO ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML_TUYAK ";
                    SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (dt == null)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show(new Form() { TopMost = true }, "조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strFormNo = (dt.Rows[0]["FORMNO"].ToString() + "").Trim();
                    }
                    dt.Dispose();
                    dt = null;
                }

                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                strCurDateTime = strCurDateTime.Replace("-", "").Replace(":", "");

                double dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "KOSMOS_EMR.EMRXMLHISNO");

                if (strFormNo == "1796")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_EMR.EMRXMLHISTORY_TUYAK";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,HISTORYWRITEDATE,HISTORYWRITETIME, ";
                    SQL = SQL + ComNum.VBLF + "      DELUSEID,CERTNO, IT1, IT2, IT3, IT4, IT5, IT6, IT7, IT8, IT9, IT10)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME, ";
                    SQL = SQL + ComNum.VBLF + "      TO_CHAR(SYSDATE,'YYYYMMDD') , TO_CHAR(SYSDATE,'HH24MMSS') ,  ";
                    SQL = SQL + ComNum.VBLF + "       '" + clsType.User.IdNumber + "',CERTNO, IT1, IT2, IT3, IT4, IT5, IT6, IT7, IT8, IT9, IT10";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML_TUYAK";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXML_TUYAK";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }



                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_EMR.EMRXMLHISTORY";
                SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS, HISTORYWRITEDATE,HISTORYWRITETIME, UPDATENO,";
                SQL = SQL + ComNum.VBLF + "      DELUSEID,CERTNO)";
                SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS, ";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "', UPDATENO, ";
                SQL = SQL + ComNum.VBLF + "       '" + clsType.User.IdNumber + "',CERTNO";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }


                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }


                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXMLMST";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(strEmrNo);
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, "삭제되었습니다.");
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }


        private double gSaveEmrXml(string mstrFormNo, string mstrUpdateNo, string mstrEmrNo, string strChartDate, string strChartTime, string strXML)
        {
            //int i = 0;
            //DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            double dblEmrNo = 0;
            double dblEmrHisNo = 0;
            //string strCONTENTS = "(SELECT CONTENTS FROM KOSMOS_EMR.EMRFORM WHERE FORMNO = 1562)";
            string strUPDATENO = "(SELECT MAX(UPDATENO) FROM KOSMOS_EMR.EMRFORM WHERE FORMNO = 1562)";


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                if (mstrEmrNo != "")
                {
                    dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "KOSMOS_EMR.EMRXMLHISNO");
                    string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                    strCurDateTime = strCurDateTime.Replace("-", "").Replace(":", "");

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_EMR.EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,HISTORYWRITEDATE,HISTORYWRITETIME, UPDATENO,EMRSIGNED,EMRXMLHASH)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,";
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                    SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "', UPDATENO, EMRSIGNED, EMRXMLHASH";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(mstrEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(mstrEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM KOSMOS_EMR.EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + VB.Val(mstrEmrNo);
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }
                }


                dblEmrNo = ComQuery.GetSequencesNo(clsDB.DbCon, "GetEmrXmlNo");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + "      (EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,EMRSIGNED,EMRXMLHASH) VALUES (";
                SQL = SQL + ComNum.VBLF + "     " + dblEmrNo + ",";
                SQL = SQL + ComNum.VBLF + "     " + VB.Val("1562") + ",";
                SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strChartDate + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strChartTime + "',";
                SQL = SQL + ComNum.VBLF + "     " + VB.Val(p.acpNo) + ",";
                SQL = SQL + ComNum.VBLF + "     '" + p.ptNo + "',";
                SQL = SQL + ComNum.VBLF + "     '" + p.inOutCls + "',";
                SQL = SQL + ComNum.VBLF + "     '" + p.medFrDate + "',";     //'strMedFrDate
                SQL = SQL + ComNum.VBLF + "     '" + p.medFrTime + "',";
                SQL = SQL + ComNum.VBLF + "     '" + p.medEndDate + "',";
                SQL = SQL + ComNum.VBLF + "     '" + p.medEndTime + "',";
                SQL = SQL + ComNum.VBLF + "     '" + p.medDeptCd + "',";
                SQL = SQL + ComNum.VBLF + "     '" + p.medDrCd + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strChartDate + "',";
                SQL = SQL + ComNum.VBLF + "     '" + strChartTime + "',";
                SQL = SQL + ComNum.VBLF + "     :1,";
                SQL = SQL + ComNum.VBLF + "     '',";
                SQL = SQL + ComNum.VBLF + "     " + strUPDATENO + ",'','')";

                clsDB.ExecuteXmlQuery(SQL, strXML, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return 0;
                }
                
                SQL = "INSERT INTO KOSMOS_EMR.EMRXMLMST (EmrNo,PtNo,GbEmr,FormNo,UseID,ChartDate,ChartTime,";
                SQL = SQL + ComNum.VBLF + " InOutCls,MedFrDate,MedFrTime,MedEndDate,MedEndTime,MedDeptCd,MedDrCd,";
                SQL = SQL + ComNum.VBLF + " WriteDate,WriteTime) ";
                SQL = SQL + ComNum.VBLF + " SELECT EmrNo,PtNo,'1',FormNo,UseID,ChartDate,ChartTime,InOutCls,";
                SQL = SQL + ComNum.VBLF + "        MedFrDate,MedFrTime,MedEndDate,MedEndTime,MedDeptCd,MedDrCd,";
                SQL = SQL + ComNum.VBLF + "        writeDate,writeTime ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_EMR.EMRXML ";
                SQL = SQL + ComNum.VBLF + "  WHERE EmrNo=" + dblEmrNo + " ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return 0;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, "저장되었습니다.");
                return dblEmrNo;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                Cursor.Current = Cursors.Default;
                return dblEmrNo;
            }
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssList, e.Column);
                return;
            }
            string strEmrNo = ssList_Sheet1.Cells[e.Row, 21].Text;
            pClearForm();
            mstrEmrNo = strEmrNo;
            LoadFlowData();
        }

        private void ssWrite_ComboCloseUp(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (ssWrite_Sheet1.RowCount == 0) return;
            if (ssWrite_Sheet1.ActiveColumnIndex == ssWrite_Sheet1.ColumnCount) return;
            ssWrite_Sheet1.SetActiveCell(ssWrite_Sheet1.ActiveRowIndex, ssWrite_Sheet1.ActiveColumnIndex + 1);
        }

        private void ssWrite_EditModeOff(object sender, EventArgs e)
        {
            //if (ssWrite_Sheet1.ActiveColumnIndex == ssWrite_Sheet1.ColumnCount) return;
            //ssWrite_Sheet1.ActiveColumnIndex = ssWrite_Sheet1.ActiveColumnIndex + 1;
        }

        private void btnSearchData_Click(object sender, EventArgs e)
        {
            ssList.Dock = DockStyle.Fill;
            webGRAPH.Dock = DockStyle.Fill;
            webGRAPH.Visible = false;
            ssList.Visible = true;
            QueryChartList();
        }

        private void btnSearchGraph_Click(object sender, EventArgs e)
        {
            ssList.Dock = DockStyle.Fill;
            webGRAPH.Dock = DockStyle.Fill;
            ssList.Visible = false;
            webGRAPH.Visible = true;

            //string EmrUrlMain = "http://192.168.100.33:8090/Emr/MtsEmrSite.mts";
            string gEmrUrl = "http://192.168.100.33:8090/";
            string EmrGraph = "Emr/vitalCheck.mts?";
            string strURL = gEmrUrl + EmrGraph;

            strURL = strURL + "ptNo=" + p.ptNo + "&" + "startDate=" + dtpFrDate.Value.ToString("yyyyMMdd") + "&" + "endDate=" + dtpEndDate.Value.ToString("yyyyMMdd");

            webGRAPH.Navigate(strURL);
            while (webGRAPH.IsBusy == true)
            {
                Application.DoEvents();
            }
            for (int intWeb = 0; intWeb < 40000; intWeb++)
            {
                Application.DoEvents();
            }
        }
    }
}
