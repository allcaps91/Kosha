using ComBase; //기본 클래스
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBasePictureView : Form, EmrChartForm
    {
        //bool misUserTemplet = false;
        public string mstrMACRONO = "";
        /// <summary>
        /// 차트작성 0, 기록지등록에서 호출 1,
        /// </summary>
        //int mCallFormGb = 0;  //
        bool isReciveOrderSave = false;

        string EmrUrlMain = "http://192.168.100.33:8090/Emr/MtsEmrSite.mts";
        string gEmrUrl = "http://192.168.100.33:8090/Emr";
        string EmrUrlImage = "http://192.168.100.33:8090/Emr/progressImageEditor.mts?formNo=1232";
        string EmrUrlPatSend = "http://192.168.100.33:8090/Emr/emrxmlInfo.mts?";

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //Form mCallForm = null;
        FormEmrMessage mEmrCallForm = null;
        EmrPatient pAcp = null;
        EmrForm pForm = null;
        FormXml[] mFormXml = null;

        //FormXml[] mFormXmlInit = null;
        public string mstrFormNo = "1232";
        public string mstrUpdateNo = "1";
        public string mstrFormText = "";
        public string mstrEmrNo = "0";  //961 131641  //963 735603
        public string mstrEmrImgNo = "0"; 
        public string mstrMode = "W";
        #endregion

        #region // TopMenu관련 선언
        private usFormTopMenu usFormTopMenuEvent;
        private usTimeSet usTimeSetEvent;
        public ComboBox mMaskBox = null;
        #endregion

        #region //EmrChartForm
        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = 0;

            dblEmrNo = VB.Val(mstrEmrNo);
            return dblEmrNo;
        }

        public bool DelDataMsg()
        {
            bool rtnVal = false;
            rtnVal = pDelData();
            return rtnVal;
        }

        public void ClearFormMsg()
        {
            mstrEmrNo = "0";
            pClearForm();
        }

        public void SetUserFormMsg(double dblMACRONO)
        {
            pSetUserForm(dblMACRONO);
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            bool rtnVal = false;
            rtnVal = pSaveUserForm(dblMACRONO);
            return rtnVal;
        }

        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {

        }

        public int PrintFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            if (strPRINTFLAG == "N")
            {
                frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
                frmEmrPrintOptionX.ShowDialog();
            }

            if (clsFormPrint.mstrPRINTFLAG == "-1")
            {
                return rtnVal;
            }

            //if (clsEmrQuery.SaveEmrXmlPrnYnForm(mstrEmrNo, "0") == false)
            //{
            //    return rtnVal;
            //}

            rtnVal = clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, mstrEmrNo, panChart, "C");
            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            rtnVal = clsFormPrint.PrintToTifFileLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, mstrEmrNo, panChart, "C");
            return rtnVal;
        }


        #endregion

        #region // TopMenu관련 이벤트 처리 함수

        private void SetTopMenuLoad()
        {
            //----TopMenu관련 이벤트 생성 및 선언
            usFormTopMenuEvent = new usFormTopMenu();
            //usBtnShow(usFormTopMenuEvent, "mbtnSave");
            usFormTopMenuEvent.rSetTimeCheckShow += new usFormTopMenu.SetTimeCheckShow(usFormTopMenuEvent_SetTimeCheckShow);
            usFormTopMenuEvent.rSetSave += new usFormTopMenu.SetSave(usFormTopMenuEvent_SetSave);
            usFormTopMenuEvent.rSetDel += new usFormTopMenu.SetDel(usFormTopMenuEvent_SetDel);
            usFormTopMenuEvent.rSetClear += new usFormTopMenu.SetClear(usFormTopMenuEvent_SetClear);
            usFormTopMenuEvent.rSetPrint += new usFormTopMenu.SetPrint(usFormTopMenuEvent_SetPrint);
            usFormTopMenuEvent.rEventClosed += new usFormTopMenu.EventClosed(usFormTopMenuEvent_EventClosed);

            usFormTopMenuEvent.dtMedFrDate.ValueChanged += new EventHandler(dtMedFrDate_ValueChanged);

            this.Controls.Add(usFormTopMenuEvent);
            usFormTopMenuEvent.Parent = this.panTopMenu;
            usFormTopMenuEvent.Dock = DockStyle.Fill;
            //--------------------------
        }

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
            SaveData("0", true);
        }

        private void usFormTopMenuEvent_SetSaveTemp(string strFrDate, string strFrTime)
        {
            SaveData("0", false);
        }

        private void usFormTopMenuEvent_SetDel(string strFrDate, string strFrTime)
        {
            pDelData();
        }

        private void usFormTopMenuEvent_SetClear()
        {
            mstrEmrNo = "0";
            pClearForm();
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);

            //mstrXmlInit = clsXML.SaveDataToXml(this, true, panChart, false);
        }

        private void usFormTopMenuEvent_SetPrint()
        {
            pPrintForm();
        }

        private void usFormTopMenuEvent_EventClosed()
        {
            //아무것도 하지 않는다.
        }

        private void dtMedFrDate_ValueChanged(object sender, EventArgs e)
        {
            //필요시만 코딩함
        }

        #endregion
      
        #region //Private Function 기록지 클리어, 저장, 삭제, 프린터

        /// <summary>
        /// 기록지 정보를 세팅한다
        /// </summary>
        private void SetFormInfo()
        {
            pForm = null;
            pForm = clsEmrChart.ClearEmrForm();
            pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, mstrFormNo, mstrUpdateNo);
        }

        /// <summary>
        /// 기록지 신규 작성을 위해 클리어
        /// </summary>
        public void pClearForm()
        {
            //모든 컨트롤을 초기화 한다.
            ComFunc.SetAllControlClearEx(this);
            //시간 세팅을 한다.
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));

            usFormTopMenuEvent.txtMedFrTime.Text = ComFunc.FormatStrToDate(VB.Mid(strCurDateTime, 9, 4), "M");
            usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            usFormTopMenuEvent.txtMedFrTime.Enabled = true;

            pClearFormExcept();

        }

        /// <summary>
        /// 클리어하고 폼별로 별요한것 기본 세팅
        /// </summary>
        private void pClearFormExcept()
        {
            //Control[] controls = ComFunc.GetAllControls(panChart);

            //foreach (Control ctl in controls)
            //{
            //    if (ctl is PictureBox)
            //    {
            //        if (((PictureBox)ctl).Image != null)
            //            ((PictureBox)ctl).Image.Dispose();
            //    }
            //}
        }

        private void pSetUserForm(double dblMACRONO)
        {
            clsXML.LoadDataUserChartRow(clsDB.DbCon, this, dblMACRONO.ToString(), false);
        }


        // <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private double pSaveEmrData()
        {
            double dblEmrNo = 0;
            string strChartDate = VB.Format(usFormTopMenuEvent.dtMedFrDate.Value, "yyyyMMdd");
            string strChartTime = VB.Replace(usFormTopMenuEvent.txtMedFrTime.Text, ":", "");
            //string strXML = "";

            if (pForm.FmOLDGB == 1)
            {
                clsEmrType.EmrXmlImage[] pEmrXmlImage = new clsEmrType.EmrXmlImage[0];
                //strXML = clsXML.SaveDataToXmlOld(this, false, panChart, ref pEmrXmlImage, mEmrXmlImageInit);

                //dblEmrNo = pSaveOldEmrData(strChartDate, strChartTime, strXML, pEmrXmlImage, mEmrXmlImageInit);

            }
            else
            {
                dblEmrNo = pSaveNewEmrData(strChartDate, strChartTime);
            }

            //전자인증 : 옮김

            return dblEmrNo;
        }

        private double pSaveNewEmrData(string strChartDate, string strChartTime)
        {
            double dblEmrNo = 0;

            string strSAVEGB = "1";
            string strSAVECERT = "1";
            dblEmrNo = clsEmrQuery.SaveChartMst(clsDB.DbCon, pAcp, this, false, this,
                                                                mstrFormNo, mstrUpdateNo, mstrEmrNo, strChartDate, strChartTime,
                                                                clsType.User.IdNumber, clsType.User.IdNumber, strSAVEGB, strSAVECERT, "0", "");
            return dblEmrNo;
        }

        /// <summary>
        /// 저장 : 이전서식
        /// </summary>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strXML"></param>
        /// <param name="pEmrXmlImage"></param>
        /// <returns></returns>
        private double pSaveOldEmrData(string strChartDate, string strChartTime, string strXML, clsEmrType.EmrXmlImage[] pEmrXmlImage, clsEmrType.EmrXmlImageInit[] pEmrXmlImageInit)
        {
            double dblEmrNo = 0;
            dblEmrNo = clsXMLOld.gSaveEmrXml(clsDB.DbCon, pAcp, mstrFormNo, mstrUpdateNo, mstrEmrNo, strChartDate, strChartTime, strXML, pEmrXmlImage, pEmrXmlImageInit, "");
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
                if (pForm.FmOLDGB == 1)
                {
                    if (clsEmrQuery.IsChangeAuthOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false)
                    {
                        return false;
                    }
                    if (clsXML.gDeleteEmrXmlOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber, mstrFormNo) == true)
                    {
                        ComFunc.MsgBoxEx(this, "삭제하였습니다.");
                        mstrEmrNo = "0";
                        if (isReciveOrderSave == false)
                        {
                            //처방저장시에는 다시 조회 하지 않는다
                            pClearForm();
                            if (mEmrCallForm != null)
                            {
                                mEmrCallForm.MsgDelete();
                            }
                        }
                    }
                }
                else
                {
                    if (clsEmrQuery.IsChangeAuth(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false)
                    {
                        return false;
                    }
                    if (clsXML.gDeleteEmrXml(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == true)
                    {
                        mstrEmrNo = "0";
                        if (isReciveOrderSave == false)
                        {
                            //처방저장시에는 다시 조회 하지 않는다
                            pClearForm();
                            if (mEmrCallForm != null)
                            {
                                mEmrCallForm.MsgDelete();
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 기록지를 출력한다.
        /// </summary>
        public void pPrintForm()
        {
            frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
            frmEmrPrintOptionX.ShowDialog();

            if (clsFormPrint.mstrPRINTFLAG == "-1")
            {
                return;
            }

            //if (clsEmrQuery.SaveEmrXmlPrnYnForm(mstrEmrNo, "0") == false)
            //{
            //    return;
            //}

            //clsFormPrint.PrintFormLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
        }

        /// <summary>
        /// 사용자 템플릿 저장
        /// </summary>
        /// <returns></returns>
        public bool pSaveUserForm(double dblMACRONO)
        {
            //TODO
            bool rtnVal = false;
            rtnVal = clsEmrQuery.SaveDataAEMRUSERCHARTFORMROW(clsDB.DbCon, this, false, this, dblMACRONO);
            return rtnVal;
        }

        #endregion //기록지 클리어, 저장, 삭제, 프린터

        #region //Public Function 외부에서 이벤트 받아서 처리 클리어, 저장, 삭제, 프린터


        /// <summary>
        /// 환자 받아서 기록지를 초기화 한다.
        /// </summary>
        public void gPatientinfoRecive(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;

            //폼을 클리어하고 기록지 작성 정보등을 갱신한다.
            pClearForm();
        }

        /// <summary>
        /// 폼이 로드할때 초기 세팅을 한다
        /// </summary>
        public void pInitForm()
        {
            SetFormInfo();

            SetTopMenuLoad();
        }

       
        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double SaveData(string strFlag, bool blnCertYn)
        {
            double dblEmrNo = 0;

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (pForm.FmOLDGB == 1)
                {
                    if (clsEmrQuery.IsChangeAuthOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false) return 0;
                }
                else
                {
                    if (clsEmrQuery.IsChangeAuth(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false) return 0;
                }
            }
         

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (MessageBox.Show(new Form() { TopMost = true }, "기존 내용을 변경하시겠습니까?", "EMR", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                {
                    return dblEmrNo;
                }
            }

            dblEmrNo = pSaveEmrData();

            if (dblEmrNo == 0)
            {
                MessageBox.Show(new Form() { TopMost = true }, "저장중 에러가 발생했습니다.");
            }
            else
            {
                MessageBox.Show(new Form() { TopMost = true }, "저장하였습니다.");
                mstrEmrNo = Convert.ToString(dblEmrNo);

                if (dblEmrNo != 0)
                {
                    //////전자인증 : 신규서식지만
                    //if (blnCertYn == true)
                    //{
                    //    if (pForm.FmOLDGB != 1)
                    //    {
                    //        string strXML = clsXML.SaveDataToXml(this, true, panChart, false);
                    //        bool blnCert = clsEmrQuery.SaveDataAEMRCHARTCERTY(this, false, this, dblEmrNo, null);
                    //        if (blnCert == false)
                    //        {
                    //            MessageBox.Show(new Form() { TopMost = true }, "인증중 에러가 발생했습니다." + ComNum.VBLF + "추후 인증을 실시해 주시기 바랍니다.");
                    //        }
                    //    }
                    //}
                }

                if (isReciveOrderSave == false)
                {
                    //처방저장시에는 다시 조회 하지 않는다
                    if (mEmrCallForm != null)
                    {
                        strFlag = "0";
                        mEmrCallForm.MsgSave(strFlag);
                    }
                }
            }
            return dblEmrNo;
        }

        /// <summary>
        /// 사용자 템플릿 뷰어용
        /// </summary>
        public void pInitFormTemplet()
        {
            SetFormInfo();

            SetTopMenuLoad();

            mFormXml = FormDesignQuery.GetDataFormXml(mstrFormNo, mstrUpdateNo);
            if (mFormXml == null)
            {
                return;
            }

            if (mFormXml != null)
            {
                for (int i = 0; i < mFormXml.Length; i++)
                {
                    if (mFormXml[i].strCONTROLPARENT == "Form1")
                    {
                        mFormXml[i].strCONTROLPARENT = "panChart";
                    }

                    if (mFormXml[i].strCONTROTYPE == "System.Windows.Forms.Panel")
                    {
                        mFormXml[i].strCONTROTYPE = "mtsPanel15.mPanel";
                    }
                }

                FormLoadControl.LoadControl(this, mFormXml, "panChart");
            }

            //SetControlEvents();
        }

        /// <summary>
        /// 폼에 이전내역을 세팅한다
        /// </summary>
        public void LoadForm()
        {
            pClearForm();
            GetImageNo(mstrEmrNo);
            //mstrXmlInit = clsXML.SaveDataToXml(this, true, panChart, false);

            //pMultiTextHeigh();
        }

        /// <summary>
        /// 사용자 템플릿 내용을 세팅한다
        /// </summary>
        public void LoadFormTemplet()
        {
            pClearForm();
            //pMultiTextHeigh();
        }

        #endregion

        #region //생성자
        public frmEmrBasePictureView()
        {
            InitializeComponent();
            pInitForm();
        }

        public frmEmrBasePictureView(string strEmrNo, EmrPatient po, FormEmrMessage pEmrCallForm)
        {
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mEmrCallForm = pEmrCallForm;
            InitializeComponent();
            pInitForm();
            LoadForm();
        }
        #endregion //생성자

        private void frmEmrBasePictureView_Load(object sender, EventArgs e)
        {
            this.Width = 700;

            //clsEmrChart.SetChartHead(clsDB.DbCon, usFormTopMenuEvent, pAcp);

            //usFormTopMenuEvent.mbtnClear.Visible = true;
            //usFormTopMenuEvent.mbtnPrint.Visible = true;
            //usFormTopMenuEvent.mbtnSave.Visible = true;
            //usFormTopMenuEvent.mbtnSaveTemp.Visible = true;

            panTopMenu.Visible = false;  

            WebLogin();

            wbImg.Navigate(EmrUrlMain);
            DateTime dtp = DateTime.Now;

            while (wbImg.ReadyState != WebBrowserReadyState.Complete)
            {
                if ((DateTime.Now - dtp).TotalMilliseconds >= 60000)
                {
                    Log.Debug("webImage, {} ", "1분경과 : " + "webImage, {}", EmrUrlMain);
                    break;
                }
                Application.DoEvents();
            }
            Application.DoEvents();


            SetPatInfoImg();
            //wbImg
        }

        #region 웹 관련
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (clsType.User.DrCode == "")
            {
                ComFunc.MsgBoxEx(this, "의사만 작성이 가능합니다.");
                return;
            }
            if (ComFunc.CheckTime(txtChartTime.Text.Trim()) == false)
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return;
            }

            if (SaveDataImageWeb() == false)
            {
                return;
            }
            
            if (VB.Val(mstrEmrImgNo) == 0)
            {
                SaveDataImage(VB.Val(mstrEmrImgNo));
            }

            mstrEmrImgNo = "";
        }

        private bool SaveDataImageWeb()
        {
            try
            {
                string strChartTime = txtChartTime.Text.Replace(":", "");
                if (strChartTime.Length < 6)
                {
                    strChartTime += "00";
                }
                wbImg.Document.GetElementById("chartDate").SetAttribute("value", dtpChartDate.Value.ToString("yyyyMMdd"));
                wbImg.Document.GetElementById("chartTime").SetAttribute("value", strChartTime);

                string strURL = "javascript:doSave()";
                wbImg.Navigate(strURL);
                DateTime dtp = DateTime.Now;

                ComFunc.Delay(1000);


                return true;
            }
            catch
            {
                return false;
            }
        }


        private bool SaveDataImage(double pEmrNo)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strEmrNo = string.Empty;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);

                SQL = " SELECT EMRNO FROM ADMIN.EMRXMLIMAGES";
                SQL = SQL + ComNum.VBLF + " WHERE WRITEDATE = '" + strCurDate + "'";
                SQL = SQL + ComNum.VBLF + " AND USEID = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + " AND PTNO = '" + pAcp.ptNo + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY WRITEDATE DESC, WRITETIME DESC";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                strEmrNo = dt.Rows[0]["EMRNO"].ToString().Trim();
                dt.Dispose();
                dt = null;

                SQL = " INSERT INTO ADMIN.EMRXMLMST";
                SQL = SQL + ComNum.VBLF + " SELECT EMRNO, PTNO, '1', FORMNO, USEID,";
                SQL = SQL + ComNum.VBLF + " CHARTDATE, CHARTTIME, INOUTCLS, MEDFRDATE,";
                SQL = SQL + ComNum.VBLF + " MedFrTime , MedEndDate, MedEndTime, MedDeptCd, MedDrCd, writeDate, writeTime";
                SQL = SQL + ComNum.VBLF + "  From ADMIN.EMRXML";
                SQL = SQL + ComNum.VBLF + " Where EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// EMRNO => IMAGEEMRNO
        /// </summary>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        private void GetImageNo(string strEmrNo)
        {
            OracleDataReader reader = null;

            try
            {
                string SQL = "SELECT CHARTDATE, CHARTTIME, EMRIMAGENO";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXMLIMAGES";
                SQL += ComNum.VBLF + "WHERE EMRNO = " + VB.Val(strEmrNo);

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(reader.GetValue(0).ToString().Trim(), "D"));
                    txtChartTime.Text = ComFunc.FormatStrToDate(reader.GetValue(1).ToString().Trim(), "M");
                    mstrEmrImgNo = reader.GetValue(2).ToString().Trim();
                }

                reader.Dispose();

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }


        private void SetPatInfoImg()
        {
            if (pAcp == null)
            {
                Log.Warn("AcpEmr is null");
                return;
            }

            string strURL = "";

            string strMedEndDate = "";
            string strMedEndTime = "";

            try
            {
                if (pAcp.inOutCls == "I")
                {
                    strMedEndDate = pAcp.medEndDate;
                    strMedEndTime = pAcp.medEndTime;
                }


                //http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=03983614&acpNo=0&inOutCls=O&medFrDate=20170803&medFrTime=114500&medEndDate=&medEndTime=&medDeptCd=MN&medDeptName=&medDrCd=0503&gubun=3&formNo=
                //http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=03983614&acpNo=0&inOutCls=O&medFrDate=20170803&medFrTime=114500&medEndDate=&medEndTime=&medDeptCd=MN&medDeptName=&medDrCd=0503&gubun=3&formNo=
                strURL = EmrUrlPatSend +
                       "ptNo=" + pAcp.ptNo +
                       "&acpNo=" + pAcp.acpNo +
                       "&inOutCls=" + pAcp.inOutCls +
                       "&medFrDate=" + pAcp.medFrDate +
                       "&medFrTime=" + pAcp.medFrTime +
                       "&medEndDate=" + strMedEndDate +
                       "&medEndTime=" + strMedEndTime +
                       "&medDeptCd=" + pAcp.medDeptCd +
                       "&medDeptName=" + "" +
                       "&medDrCd=" + pAcp.medDrCd +
                       "&gubun=" + "3" +
                       "&formNo=";
                //strURL = "http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=02487371&acpNo=0&inOutCls=O&medFrDate=20180226&medFrTime=141800&medEndDate=&medEndTime=&medDeptCd=GS&medDeptName=&medDrCd=2121&gubun=3&formNo=";
                wbImg.Navigate(strURL); //한장씩 볼 경우
                DateTime dtp = DateTime.Now;

                while (wbImg.ReadyState != WebBrowserReadyState.Complete)
                {
                    if ((DateTime.Now - dtp).TotalMilliseconds >= 60000)
                    {
                        Log.Debug("webImage, {}", "1분경과 : " + EmrUrlMain);
                        break;
                    }
                    Application.DoEvents();
                }

                //---------------------------------------------
                // 2019-01-22 테스트로 변경함
                //---------------------------------------------
                //for (int intWeb = 0; intWeb < 40000; intWeb++)
                //{
                //    Application.DoEvents();
                //}
                ComFunc.Delay(1000);
                //---------------------------------------------
                wbImg.Navigate(EmrUrlImage + "&emrImageNo=" + mstrEmrImgNo + "&emrNo=" + mstrEmrNo);
                dtp = DateTime.Now;

                while (wbImg.ReadyState != WebBrowserReadyState.Complete)
                {
                    if ((DateTime.Now - dtp).TotalMilliseconds >= 60000)
                    {
                        Log.Debug("webImage, {}", "1분경과 : " + strURL);
                        break;
                    }
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                //ComFunc.MsgBoxEx(this, ex.Message);
                //clsDB.SaveSqlErrLog(ex.Message, "", clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void WebLogin()
        {

            //clsType.User.Passhash256 = "5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028";

            string strURL = "";
            string strUseId = clsType.User.IdNumber;
            string strPw = clsType.User.Passhash256;

            //webImage.Navigate("http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=&acpNo=&inOutCls=&medFrDate=&medFrTime=&medEndDate=&medEndTime=&medDeptCd=&medDeptName=&medDrCd=&gubun=3&formNo=");
            //while (webImage.IsBusy == true)
            //{
            //    Application.DoEvents();
            //}

            //webEMR.Navigate("http://192.168.100.33:8090/Emr/emrxmlInfo.mts?ptNo=&acpNo=&inOutCls=&medFrDate=&medFrTime=&medEndDate=&medEndTime=&medDeptCd=&medDeptName=&medDrCd=&gubun=3&formNo=");
            //while (webImage.IsBusy == true)
            //{
            //    Application.DoEvents();
            //}

            try
            {
                //---------------------------------------------
                // 2019-01-22 테스트로 변경함
                //---------------------------------------------
                //for (int intWeb = 0; intWeb < 40000; intWeb++)
                //{
                //    Application.DoEvents();
                //}
                //ComFunc.Delay(1000);
                //---------------------------------------------


                ////http://192.168.100.33:8090/Emr/doLogin.mts?useId=15200&password=5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028&loginType=vb
                //webImage.Navigate("http://192.168.100.33:8090/Emr/doLogin.mts?useId=15200&password=5857b0bd2c33cea36a79dba3e1f17e225e0dad5976b53a7f4b790e9694bcd028&loginType=vb");

                strURL = gEmrUrl + "/doLogin.mts?useId=" + strUseId + "&password=" + strPw + "&loginType=vb";
                wbImg.Navigate(strURL);
                DateTime dtp = DateTime.Now;

                while (wbImg.ReadyState != WebBrowserReadyState.Complete)
                {
                    if ((DateTime.Now - dtp).TotalMilliseconds >= 60000)
                    {
                        Log.Debug("webImage, {}", "1분경과 : " + strURL);
                        break;
                    }
                    Application.DoEvents();
                }

                //---------------------------------------------
                // 2019-01-22 테스트로 변경함
                //---------------------------------------------
                //for (int intWeb = 0; intWeb < 40000; intWeb++)
                //{
                //    Application.DoEvents();
                //}
                //ComFunc.Delay(1000);
                //---------------------------------------------
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                ComFunc.MsgBoxEx(this, ex.Message);
                //clsDB.SaveSqlErrLog(ex.Message, "", clsDB.DbCon); //에러로그 저장
                return;
            }
        }
        #endregion

        private void MbtnDelete_Click(object sender, EventArgs e)
        {
            if (VB.Val(mstrEmrNo) == 0)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("기존내용을 삭제하시겠습니까?", "PSMH", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            double dblEmrNo = VB.Val(mstrEmrNo);

            if (DeleteDate(dblEmrNo) == true)
            {
                if (mEmrCallForm != null)
                {
                    mEmrCallForm.MsgDelete();
                }
                else
                {
                    this.Close();
                }
            }
        }


        private bool DeleteDate(double dblEmrNo)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strChartUseId = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "SELECT ";
                SQL = SQL + ComNum.VBLF + "                A.EMRNO, A.USEID";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRXML A";
                SQL = SQL + ComNum.VBLF + "    WHERE A.EMRNO = " + dblEmrNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                strChartUseId = dt.Rows[0]["USEID"].ToString().Trim();
                dt.Dispose();
                dt = null;

                if (clsType.User.IdNumber != strChartUseId)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                strCurDateTime = strCurDateTime.Replace("-", "").Replace(":", "");

                double dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "ADMIN.EMRXMLHISNO");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO ADMIN.EMRXMLHISTORY";
                SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "', '" + clsType.User.IdNumber + "',CERTNO";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXMLMST";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + dblEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "삭제하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }
    }
}
