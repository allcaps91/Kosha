using ComBase;
using ComDbB;
using FarPoint.Win.Spread;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace ComEmrBase
{
    public partial class frmNursingRecordOld : Form, EmrChartForm
    {
        //string mOption;
        string mMACROGB;
        int mlngMACROINDEX;
        string mInOutGb;

        #region // 상용구 관련 모듈
        private Control mControl = null;
        private frmEmrMacrowordProg frmMacrowordProgEvent;
        //Form mCallForm = null;
        #endregion

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;
        public string mstrFormNo = "965";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "간호기록지";
        public EmrPatient p = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "W";

        public string FstrWardCode = "";
        public string FstrRoomCode = "";

        public string mstrUserGb = "";
        EmrForm pForm = null;
        #endregion

        #region // TopMenu관련 선언
        private usFormTopMenu usFormTopMenuEvent;
        private usTimeSet usTimeSetEvent;
        public ComboBox mMaskBox = null;
        #endregion

        #region // 폼에 사용하는 변수를 코딩하는 부분
        //int nImage;
        //int nSelectedImage;
        int nImageSaved;
        int nSelectedImageSaved;

        string mstrXML = "";

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
            pSaveData();

            clsEmrFunc.NowEmrCert(clsDB.DbCon, p.medFrDate, p.ptNo);
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

        #region //EmrChartForm
        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }
        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = 0;
            //dblEmrNo = pSaveData(strFlag);
            return dblEmrNo;
        }

        public bool DelDataMsg()
        {
            return pDelData();
        }

        public void ClearFormMsg()
        {
            mstrEmrNo = "0";
            pClearForm();
        }
        public void SetUserFormMsg(double dblMACRONO)
        {
            //TODO
            //pSetUserForm(dblMACRONO);
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            bool rtnVal = false;
            return rtnVal;
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

            if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            {
                return rtnVal;
            }

            rtnVal = clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = clsFormPrint.PrintToTifFileLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        #endregion //EmrChartForm

        #region //외부에서 이벤트 받아서 처리 클리어, 저장, 삭제, 프린터
        /// <summary>
        /// 환자 받아서 기록지를 초기화 한다.
        /// </summary>
        public void gPatientinfoRecive(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            //폼을 클리어하고 기록지 작성 정보등을 갱신한다.
            p = po;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;

            pClearForm();
            
            QueryChartList();

            READ_NOT_CONVERT_BST(p.ptNo, p.medFrDate, ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10));
            READ_WARNING_FALL1(p.ptNo, ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10), p.age);
            READ_WARNING_BRADEN1(p.ptNo, ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10));
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
            usFormTopMenuEvent.dtMedFrDate.ValueChanged += new EventHandler(dtMedFrDate_ValueChanged);
            this.Controls.Add(usFormTopMenuEvent);
            usFormTopMenuEvent.Parent = this.panTopMenu;
            usFormTopMenuEvent.Dock = DockStyle.Fill;
            //--------------------------

            pClearForm();
            pSetEmrInfo();
        }

        /// <summary>
        /// 작성일자 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtMedFrDate_ValueChanged(object sender, EventArgs e)
        {
            string strdtpDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
            string MsgDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyy-MM-dd");
            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            if (p.medEndDate != "")
            {
                if ((VB.Val(strdtpDate) > VB.Val(p.medEndDate)))
                {
                    ComFunc.MsgBoxEx(this, "재원 기간을 넘어서는 작성 할수 없습니다.");
                    usFormTopMenuEvent.dtMedFrDate.Value = DateTime.ParseExact(p.medEndDate, "yyyyMMdd", null);
                }
            }
            else
            {
                if ((VB.Val(strdtpDate) > VB.Val(strCurDate)))
                {
                    ComFunc.MsgBoxEx(this, "재원 기간을 넘어서는 작성 할수 없습니다.");
                    usFormTopMenuEvent.dtMedFrDate.Value = DateTime.ParseExact(strCurDate, "yyyyMMdd", null);
                }
            }
        }

        private void pSetEmrInfo()
        {
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);
            usFormTopMenuEvent.mbtnPrint.Visible = false;

            //기록지 정보를 세팅한다.
            SetFormInfo();
            //EMRNO가 있으면 기록 정보를 세팅을 한다.
            pLoadEmrChartInfo();
        }

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

            //if (VB.Val(mstrEmrNo) != 0)
            //{
            //    //if (clsEmrQuery.IsChangeAuth(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false) return 0;            
            //}

            #region 작성일자 팝업 알림창
            string strdtpDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
            string MsgDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyy-MM-dd");
            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            if (p.inOutCls == "O")
            {
                if (p.medFrDate != usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd") && p.medDeptCd.Equals("ER") == false)
                {
                    if (ComFunc.MsgBoxQEx(this, "작성일자가 외래 진료일이 아닙니다 계속 작성하시겠습니까?") == DialogResult.No)
                    {
                        return VB.Val(mstrEmrNo);
                    }
                }
            }
            else
            {
                DateTime dtpChartDate = DateTime.ParseExact(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd") + " " + VB.Left(usFormTopMenuEvent.txtMedFrTime.Text.Replace(":", ""), 4), "yyyyMMdd HHmm", null);
                DateTime dtpSysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

                if (dtpChartDate > dtpSysDate)
                {
                    ComFunc.MsgBoxEx(this, "현재 시간 이후로 작성할 수 없습니다.");
                    return VB.Val(mstrEmrNo);
                }

                //if (p.medEndDate != "")
                //{
                //    if ((VB.Val(p.medFrDate) > VB.Val(strdtpDate)) || (VB.Val(p.medEndDate) < VB.Val(strdtpDate)))
                //    {
                //        //if (ComFunc.MsgBoxQEx(this, "작성일자가 재원기간을 벗어났습니다.\r\n현재 지정하신 작성일자는 '" + MsgDate + "' 입니다 정말 이날짜로 계속 작성하시겠습니까?") == DialogResult.No)
                //        //{
                //        //    return VB.Val(mstrEmrNo);
                //        //}

                //        ComFunc.MsgBoxEx(this, "작성일자가 재원기간을 벗어났습니다.\r\n현재 지정하신 작성일자는 '" + MsgDate + "' 입니다");
                //        return VB.Val(mstrEmrNo);
                //    }
                //}
                //else
                //{
                //    if ((VB.Val(p.medFrDate) > VB.Val(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"))) || (VB.Val(strCurDate) < VB.Val(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"))))
                //    {
                //        ComFunc.MsgBoxEx(this, "작성일자가 재원기간을 벗어났습니다.\r\n현재 지정하신 작성일자는 '" + MsgDate + "' 입니다");
                //        return VB.Val(mstrEmrNo);
                //    }
                //}
            }
            #endregion

            string strDate = VB.Format(usFormTopMenuEvent.dtMedFrDate.Value, "yyyy-MM-dd") + " " + VB.Left(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2) + ":" + VB.Right(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2);
            if (!VB.IsDate(strDate))
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return VB.Val(mstrEmrNo);
            }

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (ComFunc.MsgBoxQEx(this, "기존 내용을 변경하시겠습니까?", "EMR", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return VB.Val(mstrEmrNo);
                }
            }

            dblEmrNo = pSaveEmrData();
            if (dblEmrNo == 0)
            {
                ComFunc.MsgBox("저장중 에러가 발생했습니다.");
            }
            else
            {
                //ComFunc.MsgBox("저장하였습니다.");
                //mstrEmrNo = Convert.ToString(dblEmrNo);
                //pSetEmrInfo();
                mstrEmrNo = "0";
                pClearForm();
                //GetJinDanStsInfo("1");
                QueryChartList();
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
                if (clsEmrQuery.IsChangeAuthOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false)
                {
                    return false;
                }

                if (clsEmrQuery.READ_CHART_APPLY(this, mstrEmrNo))
                    return false;

                if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                    return false;

                if (ComFunc.MsgBoxQ("기존내용을 삭제하시겠습니까?") == DialogResult.No)
                {
                    return false;
                }
            }

            clsDB.setBeginTran(clsDB.DbCon);

            if (tabNr.SelectedIndex == 0)
            {
                if (txtEmrNoD.Text.Trim() != "")
                {
                    if (DeleteNurRecord(txtEmrNoD.Text) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return false;
                    }
                }
                if (txtEmrNoA.Text.Trim() != "")
                {
                    if (DeleteNurRecord(txtEmrNoA.Text) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return false;
                    }
                }
            }
            else if (tabNr.SelectedIndex == 1)
            {
                if (txtEmrNoR.Text.Trim() != "")
                {
                    if (DeleteNurRecord(txtEmrNoR.Text) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return false;
                    }
                }
            }
            else if (tabNr.SelectedIndex == 2)
            {
                if (txtEmrNoN.Text.Trim() != "")
                {
                    if (DeleteNurRecord(txtEmrNoN.Text) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return false;
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);

            mstrEmrNo = "0";
            pClearForm();

            pSetEmrInfo();
            //GetJinDanStsInfo("1");
            QueryChartList();

            return true;
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
            //ComFunc.SetAllControlClearEx(this);
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

            //사본발급 출력여부
            usFormTopMenuEvent.lblPrntYn.Visible = clsEmrQuery.READ_PRTLOG2(mstrEmrNo);
            usFormTopMenuEvent.mbtnSaveTemp.Visible = false;

            //출력한 기록지는 수정이 안되도록 막는다.
            //bool blnPRNYN = clsEmrQuery.GetPrnYnInfo(mstrEmrNo);
            //if (blnPRNYN == true)
            //{
            //    clsEmrFunc.usBtnHide(usFormTopMenuEvent);
            //}

            //clsXML.LoadDataXML(this, mstrEmrNo, false);
        }

        private void LoadNrRecord(int iRow, int iCol)
        {
            ssData_Sheet1.RowCount = 0;
            ssAction_Sheet1.RowCount = 0;
            ssResponse_Sheet1.RowCount = 0;
            ssNara_Sheet1.RowCount = 0;

            txtAction.Text = "";
            txtData.Text = "";
            txtResult.Text = "";
            txtNarration.Text = "";

            txtEmrNoD.Text = "";
            txtEmrNoA.Text = "";
            txtEmrNoN.Text = "";
            txtEmrNoR.Text = "";

            txtCHARTDATE.Text = "";
            txtCHARTTIME.Text = "";
            txtUSEID.Text = "";
            mstrUserGb = "";

            tabNr.SelectedIndex = 0;
            
            txtCHARTDATE.Text = ssList_Sheet1.Cells[iRow, 1].Text.Replace("-","");
            txtCHARTTIME.Text = ssList_Sheet1.Cells[iRow, 2].Text.Replace(":", "");
            txtUSEID.Text = ssList_Sheet1.Cells[iRow, 8].Text;

            switch (ssList_Sheet1.Cells[iRow, 6].Text)
            {
                case "D":
                    txtProbDA.Text = ssList_Sheet1.Cells[iRow, 5].Text;
                    txtEmrNoD.Text = ssList_Sheet1.Cells[iRow, 10].Text;
                    txtData.Text = ssList_Sheet1.Cells[iRow, 7].Text.Replace("\n","\r\n");
                    txtWardDA.Text = ssList_Sheet1.Cells[iRow, 3].Text;
                    txtRoomDA.Text = ssList_Sheet1.Cells[iRow, 4].Text;
                    GetDaUpdate(txtCHARTDATE.Text, txtCHARTTIME.Text, "A");
                    tabNr.SelectedIndex = 0;
                    break;
                case "A":
                    txtProbDA.Text = ssList_Sheet1.Cells[iRow, 5].Text;
                    txtEmrNoA.Text = ssList_Sheet1.Cells[iRow, 10].Text;
                    txtAction.Text = ssList_Sheet1.Cells[iRow, 7].Text.Replace("\n", "\r\n");
                    txtWardDA.Text = ssList_Sheet1.Cells[iRow, 3].Text;
                    txtRoomDA.Text = ssList_Sheet1.Cells[iRow, 4].Text;
                    GetDaUpdate(txtCHARTDATE.Text, txtCHARTTIME.Text, "D");
                    tabNr.SelectedIndex = 0;
                    break;
                case "R":
                    txtProbR.Text = ssList_Sheet1.Cells[iRow, 5].Text;
                    txtEmrNoR.Text = ssList_Sheet1.Cells[iRow, 10].Text;
                    txtResult.Text = ssList_Sheet1.Cells[iRow, 7].Text.Replace("\n", "\r\n");
                    txtWardR.Text = ssList_Sheet1.Cells[iRow, 3].Text;
                    txtRoomR.Text = ssList_Sheet1.Cells[iRow, 4].Text;
                    tabNr.SelectedIndex = 1;
                    break;
                default:
                    txtProbN.Text = ssList_Sheet1.Cells[iRow, 5].Text;
                    txtEmrNoN.Text = ssList_Sheet1.Cells[iRow, 10].Text;
                    txtNarration.Text = ssList_Sheet1.Cells[iRow, 7].Text.Replace("\n", "\r\n");
                    txtWardN.Text = ssList_Sheet1.Cells[iRow, 3].Text;
                    txtRoomN.Text = ssList_Sheet1.Cells[iRow, 4].Text;
                    tabNr.SelectedIndex = 2;
                    break;
            }

            
            //SSTab1.SelectedIndex = 1;

            if (txtCHARTDATE.Text.Trim() != "")
            {
                usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(txtCHARTDATE.Text.Trim(), "D"));
                usFormTopMenuEvent.txtMedFrTime.Text = ComFunc.FormatStrToDate(txtCHARTTIME.Text.Trim(), "T");
                usFormTopMenuEvent.dtMedFrDate.Enabled = false;
                //SSTab1.Enabled = false;
            }
        }

        private void GetDaUpdate(string strDate, string strTime, string strGb)
        {
            string strDate1 = "";
            string strTime1 = "";

            for (int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                strDate1 = ssList_Sheet1.Cells[i, 1].Text.Replace("-", "");
                strTime1 = ssList_Sheet1.Cells[i, 2].Text.Replace(":", "");

                if (strDate1 == strDate)
                {
                    if (ssList_Sheet1.Cells[i, 6].Text == strGb && strTime1 == strTime)
                    {
                        if (strGb == "D")
                        {
                            txtProbDA.Text = ssList_Sheet1.Cells[i, 5].Text;
                            txtEmrNoD.Text = ssList_Sheet1.Cells[i, 10].Text;
                            txtData.Text = ssList_Sheet1.Cells[i, 7].Text.Replace("\n", "\r\n");
                        }
                        else if (strGb == "A")
                        {
                            txtProbDA.Text = ssList_Sheet1.Cells[i, 5].Text;
                            txtEmrNoA.Text = ssList_Sheet1.Cells[i, 10].Text;
                            txtAction.Text = ssList_Sheet1.Cells[i, 7].Text.Replace("\n", "\r\n");
                        }
                    }
                }
            }
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
            string strChartTime = VB.Replace(usFormTopMenuEvent.txtMedFrTime.Text, ":", "", 1, -1);
            if (strChartTime.Length < 6)
            {
                strChartTime = strChartTime + "00";
            }

            mstrXML = "";




            //dblEmrNo = pSaveNrRecord(strChartDate, strChartTime);


            if (tabNr.SelectedIndex == 0)
            {
                FstrWardCode = txtWardDA.Text.Trim();
                FstrRoomCode = txtRoomDA.Text.Trim();

                if (txtData.Text != "")
                {
                    dblEmrNo = SaveProgress(txtData.Text.Replace("\r\n", "\n"), "Data", txtProbDA.Text, txtEmrNoD.Text.Trim());
                    if (dblEmrNo == 0)
                    {
                        return dblEmrNo;
                    }
                }

                if (txtAction.Text != "")
                {
                    dblEmrNo = SaveProgress(txtAction.Text.Replace("\r\n", "\n"), "Action", txtProbDA.Text, txtEmrNoA.Text.Trim());
                    if (dblEmrNo == 0)
                    {
                        return dblEmrNo;
                    }
                }
            }
            else if (tabNr.SelectedIndex == 1)
            {
                FstrWardCode = txtWardR.Text.Trim();
                FstrRoomCode = txtRoomR.Text.Trim();

                if (txtResult.Text != "")
                {
                    dblEmrNo = SaveProgress(txtResult.Text.Replace("\r\n", "\n"), "Result", txtProbR.Text, txtEmrNoR.Text.Trim());
                    if (dblEmrNo == 0)
                    {
                        return dblEmrNo;
                    }
                }
            }
            else if (tabNr.SelectedIndex == 2)
            {
                FstrWardCode = txtWardN.Text.Trim();
                FstrRoomCode = txtRoomN.Text.Trim();

                if (txtNarration.Text != "")
                {
                    //2019-11-26 나레이션 저장시 간호진단 제외
                    //dblEmrNo = SaveProgress(txtNarration.Text.Replace("\r\n", "\n"), "", txtProbN.Text, txtEmrNoN.Text.Trim());
                    dblEmrNo = SaveProgress(txtNarration.Text.Replace("\r\n", "\n"), "", "", txtEmrNoN.Text.Trim());
                    if (dblEmrNo == 0)
                    {
                        return dblEmrNo;
                    }
                }
            }


            if (dblEmrNo != 0)
            {
                //TODO 전자인증
                //if (clsType.gHosInfo.strEmrCertUseYn == "1")
                //{
                //    dblEmrCertNo = clsEmrCerti.SaveEmrCert(Convert.ToString(dblEmrNo), mstrEmrNo, mstrXML, strChartDate, strChartTime);
                //    if (dblEmrCertNo == 0)
                //    {
                //        ComFunc.MsgBox("인증중 에러가 발생했습니다." + ComNum.VBLF + "추후 인증을 실시해 주시기 바랍니다.");
                //    }
                //}
            }
            return dblEmrNo;
         }

        private double SaveProgress(string strProg, string strGb, string strProb, string strEmrNo)
        {
            //Dim adoRs As ADODB.Recordset
            //Dim lngRow As Long
            //Dim lngCol As Long
            //Dim strSql As String
            //Dim blnNew As Boolean
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strHead;
            string strChartX1;
            string strChartX2;
            string strXML;
            //string strXMLCert;

            double dblEmrNo = 0;
            //double dblFormNo = 0;
            //string strUseId;
            string strChartDate;
            string strChartTime;
            //string strAcpNo;
            //string strPtNo;
            string strInOutCls;
            //string strMedFrDate;
            //string strMedFrTime;
            //string strMedEndDate;
            //string strMedEndTime;
            //string strMedDeptCd;
            //string strMedDrCd;
            //string strCHARTXML;
            string strCONTENTS;
            //string strUPDATENO;

            string strPROGRESS;



            strPROGRESS = VB.Replace(VB.Trim(strProg), "'", "`");

            if (strPROGRESS == "") return dblEmrNo;


            //'2019-05-16
            if (clsEmrQueryPohangS.CHECK_NUR_SCHEDULE(clsDB.DbCon) == true)
            {
                ComFunc.MsgBox("근무시간이 아닙니다. 챠트 작성이 불가능합니다.");
                return dblEmrNo;
            }

            //'2019-05-16
            if (VB.Val(strEmrNo) != 0)
            {
                //if (txtUSEID.Text.Trim() != clsType.User.IdNumber)
                //{
                //    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "변경할 수 없습니다.");
                //    return VB.Val(strEmrNo);
                //}

                if (clsEmrQuery.READ_CHART_APPLY(this, strEmrNo))
                    return VB.Val(strEmrNo);

                if (clsEmrQuery.READ_PRTLOG(this, strEmrNo))
                    return VB.Val(strEmrNo);

                if (VB.Val(strEmrNo) != 0)
                {
                    //if (ComFunc.MsgBoxQ("기존내용을 변경하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                    //{
                    //    return dblEmrNo;
                    //}
                }
            }

            strChartDate = VB.Format(usFormTopMenuEvent.dtMedFrDate.Value, "yyyyMMdd");
            strChartTime = VB.Replace(usFormTopMenuEvent.txtMedFrTime.Text, ":", "");
            strInOutCls = mInOutGb;

            if (strChartTime.Length < 6)
            {
                strChartTime = ComFunc.RPAD(strChartTime, 8 - strChartTime.Length, "0");
            }

            

            string strDate = VB.Format(usFormTopMenuEvent.dtMedFrDate.Value, "yyyy-MM-dd") + " " + VB.Left(strChartTime, 2) + ":" + VB.Right(strChartTime, 2);
            if (!VB.IsDate(strDate))
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return 0;
            }

            if (p.medDeptCd == "HD" && FstrWardCode == "" && FstrRoomCode == "") strInOutCls = "O";

            // 차트백업 및 삭제
            if (VB.Val(strEmrNo) != 0)
            {
                dblEmrNo = VB.Val(strEmrNo);
                if (DeleteNurRecord(strEmrNo) == false)
                {
                    //GoTo ErrS
                }
            }


            if (pForm.FmOLDGB == 1)
            {
                strHead = ssHead_Sheet1.Cells[0, 0].Text;
                strChartX1 = ssHead_Sheet1.Cells[1, 0].Text;
                strChartX2 = ssHead_Sheet1.Cells[1, 1].Text;

                strCONTENTS = "(SELECT CONTENTS FROM ADMIN.EMRFORM WHERE FORMNO = " + mstrFormNo + ")";
                //strUPDATENO = "0";

                dblEmrNo = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GetEmrXmlNo");

                string strTAGHEAD = "";
                string strTAGTAIL = "";

                strXML = strHead + strChartX1;

                try
                {                
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT A.FORMNO, A.FORMNAME1 FORMNAME, ";
                    SQL = SQL + ComNum.VBLF + "        B.ITEMNO, B.ITEMNAME, B.ITEMTYPE, B.ITEMHALIGN, B.ITEMVALIGN, ";
                    SQL = SQL + ComNum.VBLF + "        B.ITEMHEIGHT, B.ITEMWIDTH, B.MULTILINE,  ";
                    SQL = SQL + ComNum.VBLF + "        B.USEMACRO, B.CONTROLID, B.ITEMRMK, B.TAGHEAD, B.TAGTAIL ";
                    SQL = SQL + ComNum.VBLF + "   FROM ADMIN.EMRFORM A INNER JOIN ADMIN.EMROPTFORM B ";
                    SQL = SQL + ComNum.VBLF + "     ON A.FORMNO = B.FORMNO ";
                    SQL = SQL + ComNum.VBLF + "  WHERE A.FORMNO = 965 ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY B.ITEMNO ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return 0;
                    }
                
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            strTAGHEAD = dt.Rows[i]["TAGHEAD"].ToString().Trim();
                            strTAGTAIL = dt.Rows[i]["TAGTAIL"].ToString().Trim();

                            switch (dt.Rows[i]["ITEMNAME"].ToString().Trim())
                            {
                                case "병동":
                                    strXML = strXML + strTAGHEAD + FstrWardCode + strTAGTAIL;
                                    break;
                                case "호실":
                                strXML = strXML + strTAGHEAD + FstrRoomCode + strTAGTAIL;
                                    break;
                                case "간호문제":
                                strXML = strXML + strTAGHEAD + strProb + strTAGTAIL;
                                    break;
                                case "구분":
                                strXML = strXML + strTAGHEAD + strGb + strTAGTAIL;
                                    break;
                                case "간호기록":
                                    strXML = strXML + strTAGHEAD + strPROGRESS + strTAGTAIL + strChartX2;
                                    break;
                            }
                        }
                    }
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(ex.Message);
                }

                // 이전서식
                if (SaveEmrXmlData(clsDB.DbCon, p, clsType.User.IdNumber, mstrFormNo, mstrUpdateNo, dblEmrNo, strChartDate.Trim(), strChartTime.Trim(), strXML) == 0)
                {
                    return 0;
                }

                try
                {
                    SQL = " INSERT INTO ADMIN.NUR_NURSE_RECODE( ";
                    SQL = SQL + ComNum.VBLF + " EMRNO, PTNO, USEID, CHARTDATE,";
                    SQL = SQL + ComNum.VBLF + " CHARTTIME, MEDFRDATE, MEDDEPTCD, MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + " WRITEDATE, WRITETIME, WARDCODE, ROOMCODE,";
                    SQL = SQL + ComNum.VBLF + " PROBLEM , GUBUN, NURSERECODE, PROBLEMCODE) VALUES (";
                    SQL = SQL + ComNum.VBLF + "  " + dblEmrNo + ",";
                    SQL = SQL + ComNum.VBLF + " '" + p.ptNo + "',";
                    SQL = SQL + ComNum.VBLF + " '" + clsType.User.IdNumber + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strChartDate + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strChartTime + "',";
                    SQL = SQL + ComNum.VBLF + " '" + p.medFrDate + "',";
                    SQL = SQL + ComNum.VBLF + " '" + p.medDeptCd + "',";
                    SQL = SQL + ComNum.VBLF + " '" + p.medDrCd + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "',";
                    SQL = SQL + ComNum.VBLF + " '" + ComQuery.CurrentDateTime(clsDB.DbCon, "T") + "',";
                    SQL = SQL + ComNum.VBLF + " '" + VB.Trim(FstrWardCode) + "',";
                    SQL = SQL + ComNum.VBLF + " '" + VB.Trim(FstrRoomCode) + "',";
                    SQL = SQL + ComNum.VBLF + " '" + VB.Trim(strProb) + "',";
                    SQL = SQL + ComNum.VBLF + " '" + VB.Trim(strGb) + "',";
                    SQL = SQL + ComNum.VBLF + " '" + VB.Trim(strPROGRESS) + "',";
                    SQL = SQL + ComNum.VBLF + " '" + VB.Trim(clsEmrPublic.GstrNurCodeDAR) + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    return 0;
                }
            }
            else
            {
                // 신규서식
                string strSAVEGB = "1";
                string strSAVECERT = "1";

                dblEmrNo = clsEmrQuery.SaveNurseRecord(clsDB.DbCon, p, mstrFormNo, mstrUpdateNo, strEmrNo, strChartDate, strChartTime,
                                                       clsType.User.IdNumber, clsType.User.IdNumber, strSAVEGB, strSAVECERT, "0", "",
                                                       strProb, strGb, strPROGRESS, VB.Trim(clsEmrPublic.GstrNurCodeDAR), FstrWardCode, FstrRoomCode);
            }

            return dblEmrNo;
        }

        private int SaveEmrXmlData(PsmhDb pDbCon, EmrPatient po, string strUseId, string strFormNo, string strUpdateNo, double strEmrNoNew, string strChartDate, string strChartTime, string strXml)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string writeDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string writeTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");


            if (clsEmrQuery.OldSaveEmrXmlData(strEmrNoNew.ToString(), strFormNo, clsType.User.IdNumber, strChartDate, strChartTime, po.acpNo, po.ptNo, po.inOutCls, po.medFrDate, po.medFrTime, po.medEndDate, po.medFrTime, po.medDeptCd, po.medDrCd, strXml, strUpdateNo) == false)
            {
                return 0;
            }


            //'미비 등록                
            SQL = "";
            SQL = SQL + ComNum.VBLF + " UPDATE ADMIN.EMRMIBI SET ";
            SQL = SQL + ComNum.VBLF + "     WRITEDATE = '" + writeDate + "', ";
            SQL = SQL + ComNum.VBLF + "     WRITETIME = '" + writeTime + "'";
            SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + po.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE = '" + po.medFrDate + "' ";
            SQL = SQL + ComNum.VBLF + "   AND MEDDRCD = '" + strUseId + "' ";
            SQL = SQL + ComNum.VBLF + "   AND MIBICLS = '1' ";
            SQL = SQL + ComNum.VBLF + "   AND MIBIGRP = 'D' ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return 0;
            }

            return 1;
        }

        private double pSaveNrRecord(string strChartDate, string strChartTime)
        {
            string strNrRecore = "";
            string strXML = "";
            string strXmlHead = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\r\n" + "<chart>" + "\r\n";
            string strXmlTail = "\r\n" + "</chart>";

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            bool bloNarrationCheck = false;
            bool bloJindan = false;
            string strStatusNo = "";
            string strUserGb = "";
            double dblEmrNoOld = 0;
            double dblEmrNoNew = 0;

            bool blnD = false;
            bool blnA = false;
            bool blnR = false;
            //string checkStatus = "0";
            //string strJindanno = "";
            //string strJINTITLE = "";
            //bool newYnPart = true; 
            string strMacrono = "";
            string strContent = "";
            string strType = "";

            if (txtUSEID.Text.Trim() != "")
            {
                if (txtUSEID.Text.Trim() != clsType.User.IdNumber)
                {
                    ComFunc.MsgBox("수정할 권한이 없습니다");
                    return 0;
                }
            }

            if (txtNarration.Text.Trim() != "")
            {
                //if ((txtNarration.Text.Trim() != "") || (txtJindanNo.Text.Trim() == "9999999999999"))
                //{
                //    bloNarrationCheck = true;
                //}
            }
            //
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                EmrPatient pTmp = null;
                pTmp = clsEmrChart.ClearPatient();

                if (VB.Val(mstrEmrNo) == 0)
                {
                    pTmp = p;
                }
                else
                {
                    pTmp = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, mstrEmrNo);
                }

                if (bloNarrationCheck == false)
                {
                    //진행중인 간호진단이 있는지 확인
                    //dt = getPatJindan(pTmp, txtJindanNo.Text.Trim());

                    //if (dt != null)
                    //{
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        bloJindan = true;
                    //        strStatusNo = dt.Rows[0]["STATUSNO"].ToString().Trim();
                    //    }
                    //    dt.Dispose();
                    //    dt = null;
                    //}

                    if (bloJindan == false)
                    {
                        if (tabNr.SelectedIndex == 0 || tabNr.SelectedIndex == 1)
                        {
                            for (i = 0; i < ssData_Sheet1.RowCount; i++)
                            {
                                if (Convert.ToBoolean(ssData_Sheet1.Cells[i, 0].Value) == true)
                                {
                                    strUserGb = ssData_Sheet1.Cells[i, 4].Text.Trim();
                                }
                            }
                            for (i = 0; i < ssAction_Sheet1.RowCount; i++)
                            {
                                if (Convert.ToBoolean(ssAction_Sheet1.Cells[i, 0].Value) == true)
                                {
                                    strUserGb = ssAction_Sheet1.Cells[i, 4].Text.Trim();
                                }
                            }
                            for (i = 0; i < ssResponse_Sheet1.RowCount; i++)
                            {
                                if (Convert.ToBoolean(ssResponse_Sheet1.Cells[i, 0].Value) == true)
                                {
                                    strUserGb = ssResponse_Sheet1.Cells[i, 4].Text.Trim();
                                }
                            }
                        }
                        SQL = "SELECT mhemr.emrnrjindanstatus_seq.nextval as STATUSNO FROM DUAL ";
                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return 0;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            return 0;
                        }
                        strStatusNo = dt.Rows[0]["STATUSNO"].ToString().Trim();
                        dt.Dispose();
                        dt = null;

                        //SQL = "";
                        //SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRNRJINDANSTATUS (PTNO,STATUSNO,JINDANNO,MEDFRDATE,MEDENDDATE,MEDDEPTCD,MEDDRCD,INOUTCLS,STATUS,STARTDATE,ENDDATE,USEID,DELCLS,WRITEDATE,WRITETIME, USERGB)";
                        //SQL = SQL + ComNum.VBLF + " VALUES('" + pTmp.ptNo + "', ";
                        //SQL = SQL + ComNum.VBLF + "'" + strStatusNo + "', ";
                        //SQL = SQL + ComNum.VBLF + "" + VB.Val(txtJindanNo.Text.Trim()) + ", ";
                        //SQL = SQL + ComNum.VBLF + "'" + pTmp.medFrDate + "', ";
                        //SQL = SQL + ComNum.VBLF + "'" + pTmp.medEndDate + "', ";
                        //SQL = SQL + ComNum.VBLF + "'" + pTmp.medDeptCd + "', ";
                        //SQL = SQL + ComNum.VBLF + "'" + pTmp.medDrCd + "', ";
                        //SQL = SQL + ComNum.VBLF + "'" + pTmp.inOutCls + "', ";
                        //SQL = SQL + ComNum.VBLF + "'1', ";
                        //SQL = SQL + ComNum.VBLF + "'" + VB.Left(strCurDateTime, 8) + "', ";
                        //SQL = SQL + ComNum.VBLF + "'', ";
                        //SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "', ";
                        //SQL = SQL + ComNum.VBLF + " '0',";
                        //SQL = SQL + ComNum.VBLF + " '" + VB.Left(strCurDateTime, 8) + "',";
                        //SQL = SQL + ComNum.VBLF + " '" + VB.Right(strCurDateTime, 6) + "',";
                        //SQL = SQL + ComNum.VBLF + " '" + strUserGb + "')";
                        //SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        //if (SqlErr != "")
                        //{
                        //    clsDB.setRollbackTran(clsDB.DbCon);
                        //    ComFunc.MsgBox(SqlErr);
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        //    Cursor.Current = Cursors.Default;
                        //    return 0;
                        //}
                    }
                }

                if (tabNr.SelectedIndex == 0 || tabNr.SelectedIndex == 1)
                {
                    //DATA/ACTION 리절트가  동시저장이 될수도 있고 데이타 액션만 저장될수도 있음
                    dblEmrNoOld = VB.Val(mstrEmrNo);
                    dblEmrNoNew = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GetEmrXmlNo");

                    if (dblEmrNoOld != 0)
                    {
                        //'기존 DAR 차팅이 있는지 확인하고 기존 내용을 DELCLS = '0'으로 한다
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT TYPE ";
                        SQL = SQL + ComNum.VBLF + "    FROM   " + ComNum.DB_EMR + "EMRNURSRECORD";
                        SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + dblEmrNoOld;
                        SQL = SQL + ComNum.VBLF + "  GROUP BY TYPE";
                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return 0;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["TYPE"].ToString().Trim() == "D")
                                {
                                    blnD = true;
                                }
                                else if (dt.Rows[i]["TYPE"].ToString().Trim() == "A")
                                {
                                    blnA = true;
                                }
                                else if (dt.Rows[i]["TYPE"].ToString().Trim() == "R")
                                {
                                    blnR = true;
                                }
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }


                    if (dblEmrNoOld != 0)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE   " + ComNum.DB_EMR + "EMRNURSRECORD SET DELCLS = '0'";
                        SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + dblEmrNoOld;
                        SQL = SQL + ComNum.VBLF + "      AND TYPE IN ('D','A','R')";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return 0;
                        }
                    }

                    //DATA
                    if (ssData_Sheet1.RowCount > 0)
                    {
                        strType = "D";
                        if (blnD == true)
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE   " + ComNum.DB_EMR + "EMRNURSRECORD SET DELCLS = '0'";
                            SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + dblEmrNoOld;
                            SQL = SQL + ComNum.VBLF + "      AND TYPE = '" + strType + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return 0;
                            }
                        }
                        for (i = 0; i < ssData_Sheet1.RowCount; i++)
                        {
                            strUserGb = ssData_Sheet1.Cells[i, 4].Text.Trim();
                            if (Convert.ToBoolean(ssData_Sheet1.Cells[i, 0].Value) == true && ssData_Sheet1.Cells[i, 1].Text.Trim() != "")
                            {
                                //checkStatus = "1";
                                //strJindanno = txtJindanNo.Text.Trim();
                                //strJINTITLE = txtNrJindan.Text.Trim();

                                strMacrono = ssData_Sheet1.Cells[i, 3].Text.Trim();
                                strContent = ssData_Sheet1.Cells[i, 1].Text.Trim();
                                //if (ssData_Sheet1.Cells[i, 5].Text.Trim() == "")
                                //{
                                //    newYnPart = true;
                                //}
                                //else
                                //{
                                //    newYnPart = false;
                                //}
                                //newYnPart = true;
                                int intDspSeq = 0;
                                if (ssData_Sheet1.Cells[i, 2].Text.Trim() == "" || ssData_Sheet1.Cells[i, 2].Text.Trim() == "999")
                                {
                                    intDspSeq = i;
                                }
                                else
                                {
                                    intDspSeq = Convert.ToInt32(VB.Val(ssData_Sheet1.Cells[i, 2].Text.Trim()));
                                }

                                //if (saveEmrNurseRecord(dblEmrNoNew, strType, strContent, txtJindanNo.Text.Trim(), strMacrono, newYnPart, checkStatus, strUserGb, strStatusNo, intDspSeq) == false)
                                //{
                                //    clsDB.setRollbackTran(clsDB.DbCon);
                                //    ComFunc.MsgBox(SqlErr);
                                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                //    Cursor.Current = Cursors.Default;
                                //    return 0;
                                //}
                            }
                        }
                    }
                    strNrRecore = strNrRecore + MakeSpdXml(ssData);

                    //ACTION
                    if (ssAction_Sheet1.RowCount > 0)
                    {
                        strType = "A";
                        if (blnA == true)
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE   " + ComNum.DB_EMR + "EMRNURSRECORD SET DELCLS = '0'";
                            SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + dblEmrNoOld;
                            SQL = SQL + ComNum.VBLF + "      AND TYPE = '" + strType + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return 0;
                            }
                        }
                        for (i = 0; i < ssAction_Sheet1.RowCount; i++)
                        {
                            strUserGb = ssAction_Sheet1.Cells[i, 4].Text.Trim();
                            if (Convert.ToBoolean(ssAction_Sheet1.Cells[i, 0].Value) == true && ssAction_Sheet1.Cells[i, 1].Text.Trim() != "")
                            {
                                //checkStatus = "1";
                                //strJindanno = txtJindanNo.Text.Trim();
                                //strJINTITLE = txtNrJindan.Text.Trim();

                                strMacrono = ssAction_Sheet1.Cells[i, 3].Text.Trim();
                                strContent = ssAction_Sheet1.Cells[i, 1].Text.Trim();

                                //newYnPart = true;
                                int intDspSeq = 0;
                                if (ssAction_Sheet1.Cells[i, 2].Text.Trim() == "" || ssAction_Sheet1.Cells[i, 2].Text.Trim() == "999")
                                {
                                    intDspSeq = i;
                                }
                                else
                                {
                                    intDspSeq = Convert.ToInt32(VB.Val(ssAction_Sheet1.Cells[i, 2].Text.Trim()));
                                }
                                //if (saveEmrNurseRecord(dblEmrNoNew, strType, strContent, txtJindanNo.Text.Trim(), strMacrono, newYnPart, checkStatus, strUserGb, strStatusNo, intDspSeq) == false)
                                //{
                                //    clsDB.setRollbackTran(clsDB.DbCon);
                                //    ComFunc.MsgBox(SqlErr);
                                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                //    Cursor.Current = Cursors.Default;
                                //    return 0;
                                //}
                            }
                        }
                    }
                    strNrRecore = strNrRecore + MakeSpdXml(ssAction);

                    //Response
                    if (ssResponse_Sheet1.RowCount > 0)
                    {
                        strType = "R";
                        if (blnR == true)
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE   " + ComNum.DB_EMR + "EMRNURSRECORD SET DELCLS = '0'";
                            SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + dblEmrNoOld;
                            SQL = SQL + ComNum.VBLF + "      AND TYPE = '" + strType + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return 0;
                            }
                        }
                        for (i = 0; i < ssResponse_Sheet1.RowCount; i++)
                        {
                            strUserGb = ssResponse_Sheet1.Cells[i, 4].Text.Trim();
                            if (Convert.ToBoolean(ssResponse_Sheet1.Cells[i, 0].Value) == true && ssResponse_Sheet1.Cells[i, 1].Text.Trim() != "")
                            {
                                //checkStatus = "1";
                                //strJindanno = txtJindanNo.Text.Trim();
                                //strJINTITLE = txtNrJindan.Text.Trim();

                                strMacrono = ssResponse_Sheet1.Cells[i, 3].Text.Trim();
                                strContent = ssResponse_Sheet1.Cells[i, 1].Text.Trim();

                                //newYnPart = true;
                                int intDspSeq = 0;
                                if (ssResponse_Sheet1.Cells[i, 2].Text.Trim() == "" || ssResponse_Sheet1.Cells[i, 2].Text.Trim() == "999")
                                {
                                    intDspSeq = i;
                                }
                                else
                                {
                                    intDspSeq = Convert.ToInt32(VB.Val(ssResponse_Sheet1.Cells[i, 2].Text.Trim()));
                                }
                                //if (saveEmrNurseRecord(dblEmrNoNew, strType, strContent, txtJindanNo.Text.Trim(), strMacrono, newYnPart, checkStatus, strUserGb, strStatusNo, intDspSeq) == false)
                                //{
                                //    clsDB.setRollbackTran(clsDB.DbCon);
                                //    ComFunc.MsgBox(SqlErr);
                                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                //    Cursor.Current = Cursors.Default;
                                //    return 0;
                                //}
                            }
                        }
                    }
                    strNrRecore = strNrRecore + MakeSpdXml(ssResponse);

                    strXML = strXmlHead + strNrRecore + strXmlTail;
                    if (clsXMLOld.gSaveEmrXmlEx(clsDB.DbCon, pTmp, mstrFormNo, mstrUpdateNo, dblEmrNoNew, dblEmrNoOld, strChartDate, strChartTime, strXML) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }
                }//(tabNr.SelectedIndex == 0 || tabNr.SelectedIndex == 1)

                if (tabNr.SelectedIndex == 2)
                {
                    strType = "N";
                    strUserGb = clsType.User.IdNumber;
                    dblEmrNoOld = VB.Val(mstrEmrNo);
                    dblEmrNoNew = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GetEmrXmlNo");

                    if (dblEmrNoOld != 0)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE   " + ComNum.DB_EMR + "EMRNURSRECORD SET DELCLS = '0'";
                        SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + dblEmrNoOld;
                        SQL = SQL + ComNum.VBLF + "      AND TYPE = '" + strType + "'";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return 0;
                        }
                    }

                    //checkStatus = "1";
                    //newYnPart = true;

                    //if (saveEmrNurseRecord(dblEmrNoNew, strType, VB.Replace(txtNarration.Text.Trim(), "'", "`"), txtJindanNo.Text.Trim(), "0", newYnPart, checkStatus, strUserGb, strStatusNo, 1) == false)
                    //{
                    //    clsDB.setRollbackTran(clsDB.DbCon);
                    //    ComFunc.MsgBox(SqlErr);
                    //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    //    Cursor.Current = Cursors.Default;
                    //    return 0;
                    //}
                    strNrRecore = strNrRecore + "<txtNarration Type=\"TextBox\" Index=\"\"><![CDATA[" + VB.Replace(txtNarration.Text.Trim(), "'", "`") + "]]></txtNarration>";

                    strXML = strXmlHead + strNrRecore + strXmlTail;
                    if (clsXMLOld.gSaveEmrXmlEx(clsDB.DbCon, pTmp, mstrFormNo, mstrUpdateNo, dblEmrNoNew, dblEmrNoOld, strChartDate, strChartTime, strXML) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                mstrXML = strXML;
                return dblEmrNoNew;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return 0;
            }
        }

        private string MakeSpdXml(FarPoint.Win.Spread.FpSpread ssSpd)
        {
            int i = 0;
            int j = 0;
            string strXML = "";
            string strConName = ssSpd.Name;
            string strConIndex = "";

            strXML = strXML + "<" + strConName + " Type=\"fpSpreadSheet\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + Convert.ToString(ssSpd.ActiveSheet.RowCount) + "_" + Convert.ToString(ssSpd.ActiveSheet.ColumnCount) + "]]></" + strConName + ">";
            for (i = 0; i < ssSpd.ActiveSheet.RowCount; i++)
            {
                for (j = 0; j < ssSpd.ActiveSheet.ColumnCount; j++)
                {
                    strXML = strXML + clsXMLOld.SaveSpreadData(ssSpd, i, j);
                }
            }
            return strXML;
        }

        //private bool saveEmrNurseRecord(double dblEmrNo, string strType, string strContent, string strJindanNo, string strMacrono, bool newYnPart, string checkStatus, string strUserGb, string strStatusNo, int intDspSeq)
        //{
            
        //}

        private DataTable getPatJindan(EmrPatient pPara, string JindanNo)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT B.STATUSNO, B.STATUS, B.STARTDATE, B.ENDDATE, B.JINDANNO, B.USERGB, A.GRPFORMNAME, B.USEID, D.USENAME";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRNRDIAGNOSISGROUP A ";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "EMRNRJINDANSTATUS B ";
            SQL = SQL + ComNum.VBLF + "                                ON A.GRPFORMNO = B.JINDANNO ";
            SQL = SQL + ComNum.VBLF + "                                AND A.USERGB = B.USERGB ";
            SQL = SQL + ComNum.VBLF + "                             INNER JOIN " + ComNum.DB_EMR + "VIEWEMRUSER D ";
            SQL = SQL + ComNum.VBLF + "                                ON B.USEID = D.USEID  ";
            SQL = SQL + ComNum.VBLF + "  WHERE B.PTNO = '" + pPara.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.MEDFRDATE = '" + pPara.medFrDate + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.INOUTCLS ='" + pPara.inOutCls + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.DELCLS = '0' ";
            SQL = SQL + ComNum.VBLF + "  GROUP BY  B.STATUSNO, B.STATUS, B.STARTDATE, B.ENDDATE, B.JINDANNO, B.USERGB, A.GRPFORMNAME, B.USEID, D.USENAME";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return null;
            }
            return dt;
        }
        

        private bool DeleteNurRecord(string strEmrNo)
        {
            if (IsOldChartFormByEmrNo(strEmrNo) == true)            
            {
                // 이전서식
                if (DeleteEmrXmlData(clsDB.DbCon, strEmrNo, clsType.User.IdNumber) != 1)
                {
                    return false;
                }

                if (pDelNrRecord(strEmrNo) == false)
                {
                    return false;
                }
            }
            else
            {                
                // 신규서식
                if (DeleteEmrData(clsDB.DbCon, strEmrNo) == false)
                {
                    return false;
                }
            }

            return true;
        }

        private int DeleteEmrXmlData(PsmhDb pDbCon, string strEmrNo, string strSabun)
        {
            //bool rtnVal = false;
            //DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double dblEmrHisNo = 0;
            //double dblEmrNoNew = 0;
            //string strFormNo = "";

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

                //SQL = " SELECT FORMNO ";
                //SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXMLMST ";
                //SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;
                //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                //if (SqlErr != "")
                //{
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //    return 0;
                //}
                //if (dt.Rows.Count > 0)
                //{
                //    strFormNo = dt.Rows[0]["FORMNO"].ToString().Trim();
                //}
                //dt.Dispose();
                //dt = null;


                //if (strFormNo == "")
                //{
                //    SQL = " SELECT FORMNO ";
                //    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML_TUYAK ";
                //    SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;
                //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                //    if (SqlErr != "")
                //    {
                //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //        return 0;
                //    }
                //    if (dt.Rows.Count > 0)
                //    {
                //        strFormNo = "1796";
                //    }
                //    dt.Dispose();
                //    dt = null;
                //}


                //if (strFormNo == "1796")
                //{
                //    SQL = "";
                //    SQL = SQL + ComNum.VBLF + " INSERT INTO ADMIN.EMRXMLHISTORY_TUYAK";
                //    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                //    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                //    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,HISTORYWRITEDATE,HISTORYWRITETIME, ";
                //    SQL = SQL + ComNum.VBLF + "      DELUSEID,CERTNO, IT1, IT2, IT3, IT4, IT5, IT6, IT7, IT8, IT9, IT10)";
                //    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                //    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                //    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                //    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME, ";
                //    SQL = SQL + ComNum.VBLF + "      TO_CHAR(SYSDATE,'YYYYMMDD') , TO_CHAR(SYSDATE,'HH24MMSS') ,  ";
                //    SQL = SQL + ComNum.VBLF + "       '" + clsType.User.IdNumber + "',CERTNO, IT1, IT2, IT3, IT4, IT5, IT6, IT7, IT8, IT9, IT10";
                //    SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRXML_TUYAK";
                //    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEmrNo;
                //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                //    if (SqlErr != "")
                //    {
                //        ComFunc.MsgBox(SqlErr);
                //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                //        return 0;
                //    }


                //    SQL = "";
                //    SQL = SQL + ComNum.VBLF + " DELETE FROM ADMIN.EMRXML_TUYAK";
                //    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEmrNo;
                //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                //    if (SqlErr != "")
                //    {
                //        ComFunc.MsgBox(SqlErr);
                //        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                //        return 0;
                //    }
                //}


                dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY";
                SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,UPDATENO, CONTENTS, HISTORYWRITEDATE,HISTORYWRITETIME)";
                SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML, UPDATENO, CONTENTS, ";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "'";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return 0;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return 0;
                }

                
                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXMLMST";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + strEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return 0;
                }
                                
                return 1;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return 0;
            }
        }

        private bool DeleteEmrData(PsmhDb pDbCon, string strEmrNo)
        {
            bool rtnVal = false;
            //string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            double dblEmrHisNo = 0; //무저건 발생한다
            //double dblEmrNoNew = 0;

            if (strEmrNo == "0")
            {
                return rtnVal;
            }

            string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            //clsDB.setBeginTran(pDbCon);

            try
            {
                // EMRNOHIS 시퀀스 생성                
                dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(pDbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                // AEMRCHARTMST 백업 및 삭제
                SqlErr = clsEmrQuery.SaveChartMastHis(pDbCon, strEmrNo, dblEmrHisNo, strCurDate, strCurTime, "C", "", clsType.User.IdNumber);
                if (SqlErr != "OK")
                {
                    //clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //// AEMRNURSRECORD 백업 및 삭제
                //SqlErr = clsEmrQuery.SaveNurNurseRecordHis(pDbCon, strEmrNo);
                //if (SqlErr != "OK")
                //{
                //    //clsDB.setRollbackTran(pDbCon);
                //    ComFunc.MsgBox(SqlErr);
                //    Cursor.Current = Cursors.Default;
                //    return rtnVal;
                //}

                //clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                //clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private bool pDelNrRecord(string strEmrNo)
        {
            //DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {                
                SQL = " DELETE ADMIN.NUR_NURSE_RECODE ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;
                
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
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
            //else
            //{
            //    panWrite.Visible = false;
            //    panList.Top = 0;
            //    panList.Height = panChart.Height - 20;
            //}

            //mstrEmrNo = "0";

            // 내원일을 세팅한다.
            //clsEmrFunc.SetMedFrEndDate(clsDB.DbCon, mstrEmrNo, p, dtpFrDate, dtpEndDate);


            //txtJindanNo.Text = "";
            //txtNrJindan.Text = "";

            txtEmrNoA.Text = "";
            txtEmrNoD.Text = "";
            txtEmrNoN.Text = "";
            txtEmrNoR.Text = "";

            txtData.Text = "";
            txtAction.Text = "";
            txtResult.Text = "";
            txtNarration.Text = "";

            FstrWardCode = p.ward;
            FstrRoomCode = p.room;
            txtWardDA.Text = p.ward; 
            txtRoomDA.Text = p.room;
            txtWardR.Text = p.ward;
            txtRoomR.Text = p.room;
            txtWardN.Text = p.ward;
            txtRoomN.Text = p.room;
                 
            //임시 55/557 병동은 DR로 표기 요청 (간호부고경자 2021-11-11)
            if (p.ward =="55" && p.room =="557")
            {
                txtWardDA.Text = "DR";
                txtWardR.Text = "DR";
                txtWardN.Text = "DR";
            }
            //ssData_Sheet1.RowCount = 0;
            //ssAction_Sheet1.RowCount = 0;
            //ssResponse_Sheet1.RowCount = 0;
            //ssNara_Sheet1.RowCount = 0;

            cboBuse.Items.Clear();
            cboBuse.Items.Add("혈관조영실" + VB.Space(50) + "100570");
            cboBuse.Items.Add("인공신장실" + VB.Space(50) + "033108");

            //tabNr.SelectedTab = tabNr_TabPage0;

            mstrUserGb = "";
        }

        #endregion

        private void frmMacrowordProgEvent_SetMacro(string strCtlName, string strMacrono, string strMacro, string strCtlNameIdx)
        {
            string strConIndex = "";
            strConIndex = clsXML.IsArryCon(mControl);

            frmMacrowordProgEvent.Close();
            frmMacrowordProgEvent = null;

            //strMacro = VB.Replace(strMacro, "\n", "\r\n", 1, -1);

            //if (optAdd.Checked == true)
            //{
            //    mControl.Text = mControl.Text + " " + strMacro;
            //}
            //else
            //{
            //    mControl.Text = strMacro;
            //}
        }

        private void frmMacrowordProgEvent_EventClosed()
        {
            frmMacrowordProgEvent.Close();
            frmMacrowordProgEvent = null;
        }

        public frmNursingRecordOld()
        {
            InitializeComponent();
        }

        public frmNursingRecordOld(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
        }

        public frmNursingRecordOld(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
        }

        private void frmNursingRecordOld_Load(object sender, EventArgs e)
        {
            //trvJindan.Dock = DockStyle.Fill;
            //panItem.Dock = DockStyle.Fill;
            mInOutGb = "I";

            //nImage = 0;
            //nSelectedImage = 1;

            nImageSaved = 2;
            nSelectedImageSaved = 3;

            trvMACROGrp.ImageList = this.ImageList2;
            //ssItem_Sheet1.RowCount = 0;
            ssList_Sheet1.RowCount = 0;
            //ssStatus_Sheet1.RowCount = 0;

            //panItem.Visible = false;
            pComboSet();
            pInitForm();

   

            SetUserAut();
            //if (VB.Val(mstrEmrNo) > 0)
            //{
            //    // 내원일을 세팅한다.
            //    //clsEmrFunc.SetMedFrEndDate(clsDB.DbCon, mstrEmrNo, p, dtpFrDate, dtpEndDate);

                
            //}
            string gUserGrade = SET_GRADE();

            dtpFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-3);
            dtpEndDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            QueryChartList();

            optDept.Checked = true;

            //optIng.Checked = true;
            //GetJinDanStsInfo("1");

            //optNanda.Checked = true;
            //tabNr.SelectedIndex = 0;
        }

        private string READ_NOT_CONVERT_BST(string argPTNO, string argINDATE, string argDate)
        {
            //공용으로 변경 예정
            string strOK = "";
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                SQL = "  SELECT MIN(SUBSTR(MEASURE_DT, 1, 8)) EXAMDATE  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.EXAM_INTERFACE_BST ";
                SQL = SQL + ComNum.VBLF + " WHERE PATIENT_ID LIKE '" + argPTNO + "%' ";
                SQL = SQL + ComNum.VBLF + "   AND MEASURE_DT >= '" + argINDATE.Replace("-", "").Trim() + "000000' ";
                SQL = SQL + ComNum.VBLF + "   AND MEASURE_DT <= '" + argDate.Replace("-","").Trim() + "999999' ";
                SQL = SQL + ComNum.VBLF + "   AND EMR IS NULL ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strOK = dt.Rows[0]["EXAMDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strOK != "")
                {
                    MessageBox.Show("★ 검사일 : " + strOK + ComNum.VBLF + " 저장하지 않은 BST 인터페이스 결과가 있습니다. 확인하시기 바랍니다.", "확인");

                    using (frmBSTInterface frm = new frmBSTInterface())
                    {
                        frm.StartPosition = FormStartPosition.CenterParent;
                        frm.ShowDialog(this);
                    }
                }

                Cursor.Current = Cursors.Default;
                return rtnVal;
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
                return rtnVal;
            }
        }

        private string READ_WARNING_FALL1(string argPTNO, string argDate, string argAge)
        {
            string strFall = "";
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "  SELECT PANO, TOTAL, AGE ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_FALLMORSE_SCALE";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND TOTAL >= 51";
                SQL = SQL + ComNum.VBLF + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                if (VB.Val(argAge) < 13)
                {
                    SQL = "  SELECT PANO, TOTAL ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_FALLHUMPDUMP_SCALE";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND TOTAL >= 12";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    strFall = "OK";
                }

                dt.Dispose();
                dt = null;

                if (strFall == "")
                {
                    if (VB.Val(argAge) < 13)
                    {
                        SQL = " SELECT  EXAM1, EXAM2, EXAM3, EXAM4 ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_FALLHUMPDUMP_CAUSE";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY ENTDATE DESC ";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            if(dt.Rows[0]["EXAM1"].ToString().Trim().Equals("1") ||
                               dt.Rows[0]["EXAM2"].ToString().Trim().Equals("1") ||
                               dt.Rows[0]["EXAM3"].ToString().Trim().Equals("1") ||
                               dt.Rows[0]["EXAM4"].ToString().Trim().Equals("1"))
                            {
                                strFall = "OK";
                            }
                        }

                        dt.Dispose();
                        dt = null;
                    }
                    else
                    {
                        SQL = " SELECT CAUSE1, CAUSE2, CAUSE3, CAUSE4,";
                        SQL = SQL + ComNum.VBLF + " CAUSE5, CAUSE6, CAUSE7, CAUSE8, ";
                        SQL = SQL + ComNum.VBLF + " CAUSE9, CAUSEETC, CAUSE10, CAUSE11, ";
                        SQL = SQL + ComNum.VBLF + " EXAM1, EXAM2, EXAM3, EXAM4 ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_FALLMORSE_CAUSE";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY ENTDATE DESC ";

                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            if (
                                dt.Rows[0]["CAUSE1"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["CAUSE2"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["CAUSE3"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["CAUSE4"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["CAUSE5"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["CAUSE6"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["CAUSE7"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["CAUSE8"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["CAUSE9"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["CAUSE10"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["CAUSE11"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["EXAM1"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["EXAM2"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["EXAM3"].ToString().Trim().Equals("1") ||
                                dt.Rows[0]["EXAM4"].ToString().Trim().Equals("1") ||
                                string.IsNullOrWhiteSpace(dt.Rows[0]["CAUSEETC"].ToString().Trim()) == false)
                            {
                                strFall = "OK";
                            }
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }

                if (strFall == "OK")
                {
                    using (frmMSG_FALL frm = new frmMSG_FALL())
                    {
                        frm.StartPosition = FormStartPosition.CenterParent;
                        frm.ShowDialog(this);
                    }
                }

                Cursor.Current = Cursors.Default;
                return rtnVal;
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
                return rtnVal;
            }
        }

        private string READ_WARNING_BRADEN1(string argPTNO, string argDate)
        {
            string strBraden = "";
            string rtnVal = "";

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (VB.Val(p.age) < 5)
                {
                    SQL = " SELECT A.PANO, A.TOTAL, B.AGE ";
                    SQL += ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_SCALE_CHILD A, ADMIN.IPD_NEW_MASTER B";
                    SQL += ComNum.VBLF + " WHERE A.PANO = '" + argPTNO + "'";
                    SQL += ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "     AND A.TOTAL <= 16";
                    SQL += ComNum.VBLF + "     AND A.IPDNO = B.IPDNO ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strBraden = "OK";
                    }

                    dt.Dispose();
                    dt = null;

                }
                else
                {
                    SQL = " SELECT A.PANO, A.TOTAL, B.AGE ";
                    SQL += ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_SCALE A, ADMIN.IPD_NEW_MASTER B";
                    SQL += ComNum.VBLF + " WHERE A.PANO = '" + argPTNO + "'";
                    SQL += ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "     AND A.TOTAL <= 18";
                    SQL += ComNum.VBLF + "     AND A.IPDNO = B.IPDNO ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        double Age = VB.Val(dt.Rows[0]["AGE"].ToString().Trim());
                        double TOTAL = VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim());

                        if (Age >= 60 && TOTAL <= 18 ||
                            Age < 60 && TOTAL <= 16)
                        {
                            strBraden = "OK";
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (string.IsNullOrWhiteSpace(strBraden))
                {
                    SQL = " SELECT BUN1, BUN2, BUN3, BUN4, BUN5, BUN6, BUN7, YOKGBN ";
                    SQL += ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_SCALE_DETAIL ";
                    SQL += ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";
                    SQL += ComNum.VBLF + " AND ActDate >= TO_DATE('" + argDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  ORDER BY ActDate DESC ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (
                            dt.Rows[0]["BUN1"].ToString().Trim().Equals("1") ||
                            dt.Rows[0]["BUN2"].ToString().Trim().Equals("1") ||
                            dt.Rows[0]["BUN3"].ToString().Trim().Equals("1") ||
                            dt.Rows[0]["BUN4"].ToString().Trim().Equals("1") ||
                            dt.Rows[0]["BUN5"].ToString().Trim().Equals("1") ||
                            dt.Rows[0]["BUN6"].ToString().Trim().Equals("1") ||
                            dt.Rows[0]["BUN7"].ToString().Trim().Equals("1") ||
                            dt.Rows[0]["YOKGBN"].ToString().Trim().Equals("1"))
                        {
                            strBraden = "OK";
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }

            
                if (strBraden.Equals("OK"))
                {
                    using(frmMSG_BEDSORE frm = new frmMSG_BEDSORE())
                    {
                        frm.StartPosition = FormStartPosition.CenterParent;
                        frm.ShowDialog(this);
                    }                
                }

                Cursor.Current = Cursors.Default;
                return rtnVal;
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
                return rtnVal;
            }
        }


        string SET_GRADE()
        {
            string rtnVal = string.Empty;
            switch (clsType.User.BuseCode)
            {
                case "078200":
                case "078201":
                case "077502":
                case "077405":
                    rtnVal = "SIMSA";
                    break;
                case "044201":
                case "044200":
                    rtnVal = "WRITE";
                    break;

            }

            if (clsType.User.IdNumber == "14472")
            {
                rtnVal = "SIMSA";
            }

            return rtnVal;

        }

        private void pComboSet()
        {
            int i = 0;
            DataTable dt = null;
            //string SQL = "";
            //string SqlErr = "";
            //string mstrWard = "";

            //SQL = " SELECT NAME WARDCODE, MATCH_CODE";
            //SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
            //SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
            //SQL = SQL + ComNum.VBLF + "   AND SUBUSE = '1'";
            //SQL = SQL + ComNum.VBLF + "   AND MATCH_CODE = '" + clsType.User.BuseCode + "'";
            //SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

            //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            //if (SqlErr != "")
            //{
            //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //    return;
            //}
            //if (dt.Rows.Count > 0)
            //{
            //    mstrWard = dt.Rows[0]["WARDCODE"].ToString().Trim();
            //}
            //dt.Dispose();
            //dt = null;

            cboWard.Items.Clear();
            //-->병동콤보 세팅
            cboWard.Items.Add("전  체" + VB.Space(50) + "0");

            dt = clsEmrQueryPohangS.READ_WARD_LIST(clsDB.DbCon);

            if (dt == null)
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count != 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add((dt.Rows[i]["WARDNAME"].ToString() + "").Trim() + VB.Space(50) + (dt.Rows[i]["WARDCD"].ToString() + "").Trim());
                }
            }
            dt.Dispose();
            dt = null;
            cboWard.SelectedIndex = 0;

            for (i = 0; i < cboWard.Items.Count; i++)
            {
                if ((VB.Right(cboWard.Items[i].ToString(), 10)).Trim() == clsType.User.BuseCode)
                {
                    cboWard.SelectedIndex = i;
                    break;
                }
            }
        }

        private void SetUserAut()
        {
            //if (clsType.User.AuAWRITE == "1")
            //{
            //    panWrite.Visible = true;
            //    panChart.Top = 37;
            //}
            //else
            //{
            //    panWrite.Visible = false;
            //    panChart.Top = 0;
            //}
            //if (clsType.User.AuAPRINTIN == "1")
            //{
            //    mbtnPrint.Visible = true;
            //}
            //else
            //{
            //    mbtnPrint.Visible = true;
            //}

            panWrite.Visible = true;
            panChart.Top = 37;
        }

        private void mbtnSearchAll_Click(object sender, EventArgs e)
        {
            QueryChartList();
        }

        private void QueryChartList()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssList_Sheet1.RowCount = 0;
            
            Cursor.Current = Cursors.WaitCursor;
            READ_NUR_PROBLEM(p.ptNo, p.medFrDate);

            //if(pForm.FmOLDGB == 1)
            //{ 
            //    // 이전서식
            //    SQL = " SELECT A.EMRNO, ";
            //    SQL = SQL + ComNum.VBLF + "     A.CHARTDATE, A.CHARTTIME,   ";
            //    SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ta3') AS TA3, ";
            //    SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ta4') AS TA4, ";
            //    SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ta1') AS TA1, ";
            //    SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//it1') AS IT1, ";
            //    SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ta2') AS TA2, ";
            //    SQL = SQL + ComNum.VBLF + "     A.MEDDEPTCD, A.MEDDRCD, A.USEID,U.NAME USENAME, A.ACPNO, A.INOUTCLS, A.EMRNO, ";
            //    SQL = SQL + ComNum.VBLF + "     A.MEDFRDATE, A.MEDFRTIME,  B.FORMNO, B.FORMNAME1 FORMNAME, B.USERFORMNO ";            
            //    SQL = SQL + ComNum.VBLF + "FROM ADMIN.EMRXML A INNER JOIN ADMIN.EMRFORM B ";
            //    SQL = SQL + ComNum.VBLF + "      ON A.FORMNO = B.FORMNO ";
            //    SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.EMR_USERT U";
            //    SQL = SQL + ComNum.VBLF + "      ON A.USEID = U.USERID ";

            //    if (cboBuse.Text != "")
            //    {
            //        SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.EMR_USERT U2 ";
            //        SQL = SQL + ComNum.VBLF + "      ON A.USEID = U2.USERID";
            //        SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.BAS_BUSE E ";
            //        SQL = SQL + ComNum.VBLF + "      ON E.BUCODE = U2.BUSECODE ";
            //        SQL = SQL + ComNum.VBLF + "    AND E.BUCODE = " + VB.Right(cboBuse.Text, 6);
            //    }

            //    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO IN (";
            //    SQL = SQL + ComNum.VBLF + "    SELECT EMRNO FROM ADMIN.EMRXMLMST ";
            //    SQL = SQL + ComNum.VBLF + "     WHERE FORMNO = " + mstrFormNo;
            //    SQL = SQL + ComNum.VBLF + "      AND PTNO = '" + p.ptNo + "' ";
            //    SQL = SQL + ComNum.VBLF + "      AND CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ";
            //    SQL = SQL + ComNum.VBLF + "      AND CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "') ";
            //    SQL = SQL + ComNum.VBLF + "  ORDER BY A.CHARTDATE DESC, TRIM(A.CHARTTIME) DESC, A.EMRNO ASC ";

            //    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            //    if (SqlErr != "")
            //    {
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //        Cursor.Current = Cursors.Default;
            //        return;
            //    }
            //    if (dt.Rows.Count == 0)
            //    {
            //        dt.Dispose();
            //        dt = null;
            //        Cursor.Current = Cursors.Default;
            //        return;
            //    }

            //    string strChartDate = "";
            //    string strChartTime = "";

            //    ssList_Sheet1.RowCount = dt.Rows.Count;

            //    for (i = 0; i < dt.Rows.Count; i++)
            //    {
            //        strChartDate = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D");
            //        strChartTime = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M");

            //        ssList_Sheet1.Cells[i, 1].Text = strChartDate;
            //        ssList_Sheet1.Cells[i, 2].Text = strChartTime;
            //        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["TA3"].ToString().Trim();
            //        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["TA4"].ToString().Trim();
            //        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["TA1"].ToString().Trim();
            //        ssList_Sheet1.Cells[i, 6].Text = VB.Left(dt.Rows[i]["IT1"].ToString().Trim(), 1);
            //        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["TA2"].ToString().Trim();
            //        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["USENAME"].ToString().Trim();
            //        ssList_Sheet1.Cells[i, 10].Text = (dt.Rows[i]["EMRNO"].ToString() + "").Trim();
            //        ssList_Sheet1.SetRowHeight(i, Convert.ToInt32(ssList_Sheet1.GetPreferredRowHeight(i)) + 16);            
            //    }
            //    dt.Dispose();
            //    dt = null;
            //    Cursor.Current = Cursors.Default;
            //}

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "     FORMNO, UPDATENO, FORMNAME, EMRNO, CHARTDATE, CHARTTIME, ";
            SQL = SQL + ComNum.VBLF + "     T.WARDCODE, ROOMCODE, PROBLEM, GUBUN, NURSERECODE, ";
            SQL = SQL + ComNum.VBLF + "     MEDDEPTCD, MEDDRCD, USEID, USENAME, ACPNO, INOUTCLS, ";
            SQL = SQL + ComNum.VBLF + "     MEDFRDATE, MEDFRTIME ";
            SQL = SQL + ComNum.VBLF + "FROM ";
            SQL = SQL + ComNum.VBLF + "( ";
            SQL = SQL + ComNum.VBLF + "SELECT B.FORMNO, B.UPDATENO, B.FORMNAME, A.EMRNO, ";
            SQL = SQL + ComNum.VBLF + "     A.CHARTDATE, A.CHARTTIME, ";
            SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ta3') AS WARDCODE, ";
            SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ta4') AS ROOMCODE, ";
            SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ta1') AS PROBLEM, ";
            SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//it1') AS GUBUN, ";
            SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ta2') AS NURSERECODE, ";
            SQL = SQL + ComNum.VBLF + "     A.MEDDEPTCD, A.MEDDRCD, A.USEID, U.NAME USENAME, A.ACPNO, A.INOUTCLS, ";
            SQL = SQL + ComNum.VBLF + "     A.MEDFRDATE, A.MEDFRTIME ";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.EMRXML A INNER JOIN ADMIN.EMRFORM B ";
            SQL = SQL + ComNum.VBLF + "      ON A.FORMNO = B.FORMNO ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.EMR_USERT U ";
            SQL = SQL + ComNum.VBLF + "      ON A.USEID = U.USERID ";
            SQL = SQL + ComNum.VBLF + "  WHERE EMRNO IN( ";
            SQL = SQL + ComNum.VBLF + "    SELECT EMRNO FROM ADMIN.EMRXMLMST ";
            SQL = SQL + ComNum.VBLF + "     WHERE FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "      AND PTNO = '" + p.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "      AND CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "      AND CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "') ";
            SQL = SQL + ComNum.VBLF + "UNION ALL ";
            SQL = SQL + ComNum.VBLF + "  SELECT B.FORMNO, B.UPDATENO, B.FORMNAME, A.EMRNO, ";
            SQL = SQL + ComNum.VBLF + "     A.CHARTDATE, A.CHARTTIME, ";
            SQL = SQL + ComNum.VBLF + "     N.WARDCODE, ";
            SQL = SQL + ComNum.VBLF + "     N.ROOMCODE, ";
            SQL = SQL + ComNum.VBLF + "     N.PROBLEM, ";
            SQL = SQL + ComNum.VBLF + "     N.TYPE AS GUBUN, ";
            SQL = SQL + ComNum.VBLF + "     N.NRRECODE AS NURSERECODE, ";
            SQL = SQL + ComNum.VBLF + "     A.MEDDEPTCD, A.MEDDRCD, A.CHARTUSEID USEID, U.NAME USENAME, A.ACPNO, A.INOUTCLS, ";
            SQL = SQL + ComNum.VBLF + "     A.MEDFRDATE, A.MEDFRTIME ";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.AEMRCHARTMST A INNER JOIN ADMIN.AEMRFORM B ";
            SQL = SQL + ComNum.VBLF + "      ON A.FORMNO = B.FORMNO ";
            SQL = SQL + ComNum.VBLF + "     AND A.UPDATENO = B.UPDATENO ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.EMR_USERT U ";
            SQL = SQL + ComNum.VBLF + "      ON A.CHARTUSEID = U.USERID ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.AEMRNURSRECORD N ";
            SQL = SQL + ComNum.VBLF + "      ON A.EMRNO = N.EMRNO ";
            SQL = SQL + ComNum.VBLF + "     AND A.EMRNOHIS = N.EMRNOHIS ";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "  AND A.PTNO = '" + p.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "' ";
            SQL = SQL + ComNum.VBLF + ") T ";
            if (cboBuse.Text != "")
            {
                SQL = SQL + ComNum.VBLF + "    INNER JOIN ADMIN.EMR_USERT U2 ";
                SQL = SQL + ComNum.VBLF + "      ON T.USEID = U2.USERID";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.BAS_BUSE E ";
                SQL = SQL + ComNum.VBLF + "      ON E.BUCODE = U2.BUSECODE ";
                SQL = SQL + ComNum.VBLF + "    AND E.BUCODE = " + VB.Right(cboBuse.Text, 6);
            }
            SQL = SQL + ComNum.VBLF + "ORDER BY CHARTDATE DESC, TRIM(CHARTTIME) DESC, EMRNO ASC ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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

            string strChartDate = "";
            string strChartTime = "";

            ssList_Sheet1.RowCount = dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strChartDate = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D");
                strChartTime = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M");

                ssList_Sheet1.Cells[i, 1].Text = strChartDate;
                ssList_Sheet1.Cells[i, 2].Text = strChartTime;
                ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["PROBLEM"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = VB.Left(dt.Rows[i]["GUBUN"].ToString().Trim(), 1);
                ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["NURSERECODE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                ssList_Sheet1.Cells[i, 10].Text = (dt.Rows[i]["EMRNO"].ToString() + "").Trim();
                ssList_Sheet1.SetRowHeight(i, Convert.ToInt32(ssList_Sheet1.GetPreferredRowHeight(i)) + 16);
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void SetUserXmlValue(FarPoint.Win.Spread.FpSpread ctl, XmlNode childNode, int iRow)
        {
            string strName = "";
            //string strIndex = "";
            string strValue = "";
            //string strType = "";

            int iCol = 0;
                       

            strName = childNode.Name.ToString();            
            strValue = (childNode.InnerText.ToString() + "").ToString();

            switch (strName)
            {
                case "ta3":         //병동
                    iCol = 3;                    
                    break;
                case "ta4":         //호실
                    iCol = 4;
                    break;
                case "ta1":         //간호문제
                    iCol = 5;
                    break;
                case "it1":         //구분
                    iCol = 6;
                    break;
                case "ta2":         //간호기록
                    iCol = 7;
                    break;
            }

            if (strName == "it1")
            {
                ctl.ActiveSheet.Cells[iRow, iCol].Text = VB.Left(strValue, 1);
            }
            else
            {
                ctl.ActiveSheet.Cells[iRow, iCol].Text = strValue;
            }            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //panItem.Visible = false;
        }

        private void mbtnNrJindan_Click(object sender, EventArgs e)
        {
            //TODO
            //frmNursingRecordJindan frmNursingRecordJindanX = new frmNursingRecordJindan();
            //frmNursingRecordJindanX.ShowDialog();
        }
        
        private void trvJindan_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            System.Windows.Forms.TreeNode Node;

            Node = e.Node;
            if (Node.GetNodeCount(false) > 0)
            {
                return;
            }
                        
            ssData_Sheet1.RowCount = 0;
            ssAction_Sheet1.RowCount = 0;
            ssResponse_Sheet1.RowCount = 0;
            ssNara_Sheet1.RowCount = 0;

            GetJindanInfo(Convert.ToInt32(VB.Val(e.Node.Name)), e.Node.Text.Trim());
        }

        private void GetJindanInfo(int intVal, string strName)
        {
            string strUserGb = "";            
            mstrUserGb = "";

            GetNrMacroList("D", intVal, strName, strUserGb, ssData_Sheet1, "");
            GetNrMacroList("A", intVal, strName, strUserGb, ssAction_Sheet1, "");
            GetNrMacroList("R", intVal, strName, strUserGb, ssResponse_Sheet1, "");
            GetNrMacroList("", intVal, strName, strUserGb, ssNara_Sheet1, "");
        }

        private void GetNrMacroList(string strOption, int intJinDanNo, string strGrpName, string strUserGb, FarPoint.Win.Spread.SheetView spd, string strCopy)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            mstrUserGb = strUserGb;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "  SELECT MACRONO, JINDANNO, TYPE, CONTENT, DISPSEQ, USERGB, USEID, USECLS";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRNRMACRO";
            SQL = SQL + ComNum.VBLF + "  WHERE JINDANNO = " + intJinDanNo;
            SQL = SQL + ComNum.VBLF + "  AND TYPE = '" + strOption + "'";
            SQL = SQL + ComNum.VBLF + "  AND USERGB = '" + strUserGb + "'";
            SQL = SQL + ComNum.VBLF + "  ORDER BY DISPSEQ ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
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

            spd.RowCount = dt.Rows.Count;
            spd.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                spd.Cells[i, 1].Text = dt.Rows[i]["CONTENT"].ToString().Trim();
                spd.Cells[i, 2].Text = dt.Rows[i]["DISPSEQ"].ToString().Trim();                
                spd.Cells[i, 3].Text = dt.Rows[i]["MACRONO"].ToString().Trim();                
                spd.Cells[i, 4].Text = dt.Rows[i]["USERGB"].ToString().Trim();
                if (dt.Rows[i]["USECLS"].ToString().Trim() == "0")
                {
                    spd.Cells[i, 0, i, spd.ColumnCount - 1].BackColor = Color.LightGray;
                }
                else
                {
                    spd.Cells[i, 0].Value = false;
                }
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }
        
        private void mbtnDelete_Click(object sender, EventArgs e)
        {
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            int i = 0;
            //DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ssList_Sheet1.RowCount == 0)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("정말 삭제하시겠습니까?", "간호진단", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value) == true)
                    {
                        if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 12].Value) == false)
                        {
                            string strEmrNo = ssList_Sheet1.Cells[i, 10].Text.Trim();
                            string strMacrono = ssList_Sheet1.Cells[i, 8].Text.Trim();

                            SQL = " UPDATE " + ComNum.DB_EMR + "EMRNURSRECORD SET DELCLS = '0'";
                            SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + VB.Val(strEmrNo);
                            SQL = SQL + ComNum.VBLF + "  AND MACRONO = " + VB.Val(strMacrono);
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("삭제 하였습니다.");

                QueryChartList();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void cmdInsData_Click(object sender, EventArgs e)
        {
            string strUserGb = "";
            if (ssData_Sheet1.RowCount <= 0)
            {
                strUserGb = mstrUserGb;
            }
            else
            {
                strUserGb = ssData_Sheet1.Cells[ssData_Sheet1.RowCount - 1, 4].Text.Trim();
            }
            ssData_Sheet1.RowCount = ssData_Sheet1.RowCount + 1;
            ssData_Sheet1.Cells[ssData_Sheet1.RowCount - 1, 0].Value = true;
            ssData_Sheet1.Cells[ssData_Sheet1.RowCount - 1, 4].Text = strUserGb;
        }

        private void cmdInsAction_Click(object sender, EventArgs e)
        {
            string strUserGb = "";
            if (ssAction_Sheet1.RowCount <= 0)
            {
                strUserGb = mstrUserGb;
            }
            else
            {
                strUserGb = ssAction_Sheet1.Cells[ssAction_Sheet1.RowCount - 1, 4].Text.Trim();
            }
            ssAction_Sheet1.RowCount = ssAction_Sheet1.RowCount + 1;
            ssAction_Sheet1.Cells[ssAction_Sheet1.RowCount - 1, 0].Value = true;
            ssAction_Sheet1.Cells[ssAction_Sheet1.RowCount - 1, 4].Text = strUserGb;
        }

        private void cmdInsResponse_Click(object sender, EventArgs e)
        {
            string strUserGb = "";
            if (ssResponse_Sheet1.RowCount <= 0)
            {
                strUserGb = mstrUserGb;
            }
            else
            {
                strUserGb = ssResponse_Sheet1.Cells[ssResponse_Sheet1.RowCount - 1, 4].Text.Trim();
            }
            ssResponse_Sheet1.RowCount = ssResponse_Sheet1.RowCount + 1;
            ssResponse_Sheet1.Cells[ssResponse_Sheet1.RowCount - 1, 0].Value = true;
            ssResponse_Sheet1.Cells[ssResponse_Sheet1.RowCount - 1, 4].Text = strUserGb;
        }

        private void cmdDelData_Click(object sender, EventArgs e)
        {
            if (ssData_Sheet1.RowCount == 0)
            {
                return;
            }
            ssData_Sheet1.Rows[ssData_Sheet1.ActiveRowIndex].Remove();
        }

        private void cmdDelAction_Click(object sender, EventArgs e)
        {
            if (ssAction_Sheet1.RowCount == 0)
            {
                return;
            }
            ssAction_Sheet1.Rows[ssAction_Sheet1.ActiveRowIndex].Remove();
        }

        private void cmdDelResponse_Click(object sender, EventArgs e)
        {
            if (ssResponse_Sheet1.RowCount == 0)
            {
                return;
            }
            ssResponse_Sheet1.Rows[ssResponse_Sheet1.ActiveRowIndex].Remove();
        }

        private void btnNarration_Click(object sender, EventArgs e)
        {
            string strConIndex = "";
            mControl = txtNarration;
            strConIndex = clsXML.IsArryCon(mControl);

            if (frmMacrowordProgEvent == null)
            {
                frmMacrowordProgEvent = new frmEmrMacrowordProg(mControl.Name, strConIndex, mstrFormNo, "200", mstrFormText);
                frmMacrowordProgEvent.rSetMacro += new frmEmrMacrowordProg.SetMacro(frmMacrowordProgEvent_SetMacro);
                frmMacrowordProgEvent.rEventClosed += new frmEmrMacrowordProg.EventClosed(frmMacrowordProgEvent_EventClosed);
            }
            frmMacrowordProgEvent.ShowDialog();
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }
            string strEmrNo = ssList_Sheet1.Cells[e.Row, 10].Text.Trim();
            mstrEmrNo = strEmrNo;

            pLoadEmrChartInfo();
            bool btnVisible = false;

            if (IsOldChartFormByEmrNo(mstrEmrNo) == true)
            {
                btnVisible = clsEmrQuery.IsChangeAuthOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber);
            }
            else
            {
                btnVisible = clsEmrQuery.IsChangeAuth(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber);
            }
            
            usFormTopMenuEvent.mbtnSave.Visible = btnVisible;
            //usFormTopMenuEvent.mbtnSaveTemp.Visible = btnVisible;
            usFormTopMenuEvent.mbtnDelete.Visible = btnVisible;

            LoadNrRecord(e.Row, e.Column);
        }

        private void CheckSpd(FarPoint.Win.Spread.SheetView spd, bool chkVal)
        {
            int i = 0;
            for (i = 0; i < spd.RowCount; i++)
            {
                spd.Cells[i, 0].Value = chkVal;
            }
        }
        
        private void mbtnPrint_Click(object sender, EventArgs e)
        {
            //mbtnPrint.Enabled = false;
            //ssList_Sheet1.Columns[0].Visible = false;
            //ssList_Sheet1.Columns[1].Width = 86;
            //ssList_Sheet1.Columns[12].Visible = false;
            //clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
            //                             ssList, "P", 30, 20, 50, 50, false, FarPoint.Win.Spread.PrintOrientation.Portrait, "ROW", -1, 10, 0);
            //ssList_Sheet1.Columns[0].Visible = true;
            //ssList_Sheet1.Columns[1].Width = 70;
            //ssList_Sheet1.Columns[12].Visible = true;
            //mbtnPrint.Enabled = true;
        }

        private void LoadTable()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            try
            {                
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.MACROGB, A.MACROINDEX, A.MACROKEY, A.MACROPARENT, A.MACRONAME,";
                SQL = SQL + ComNum.VBLF + "      (SELECT MAX(O.MACROINDEX) FROM ADMIN.EMRMACROETCDTL O";
                SQL = SQL + ComNum.VBLF + "              WHERE O.MACROGB = A.MACROGB AND O.MACROINDEX = A.MACROINDEX) AS DTLYN";
                SQL = SQL + ComNum.VBLF + "          FROM ADMIN.EMRMACROETC A";
                SQL = SQL + ComNum.VBLF + "         WHERE A.MACROGB = '"+ mMACROGB + "'";                
                SQL = SQL + ComNum.VBLF + "         ORDER BY A.MACROPARENTV, A.SYSDSPINDEX";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    System.Windows.Forms.TreeNode[] oNodex;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["MACROPARENT"].ToString().Trim() == "0_")
                        {
                            trvMACROGrp.Nodes.Add((dt.Rows[i]["MACROKEY"].ToString() + "").Trim(), (dt.Rows[i]["MACRONAME"].ToString() + "").Trim(), nImageSaved, nSelectedImageSaved);
                        }
                        else
                        {
                            oNodex = trvMACROGrp.Nodes.Find((dt.Rows[i]["MACROPARENT"].ToString() + "").Trim(), true);
                            if (oNodex.Length > 0)
                            {
                                oNodex[0].Nodes.Add((dt.Rows[i]["MACROKEY"].ToString() + "").Trim(), (dt.Rows[i]["MACRONAME"].ToString() + "").Trim());
                            }
                        }
                    }                    
                }
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void optDept_CheckedChanged(object sender, EventArgs e)
        {
            if (optDept.Checked == true)
            {
                panWard.Visible = true;

                //mOption = "D";
                if (cboWard.Items.Count == 0)
                {
                    mMACROGB = clsType.User.DeptCode;
                }
                else
                {
                    mMACROGB = VB.Trim(VB.Right(cboWard.Text, 10));
                }

                trvMACROGrp.Nodes.Clear();
                LoadTable();
            }
        }

        private void optUse_CheckedChanged(object sender, EventArgs e)
        {
            if (optUse.Checked == true)
            {
                panWard.Visible = false;

                //mOption = "U";
                switch (clsType.User.IdNumber)
                {
                    case "14472":
                    case "16047":
                    case "22901":
                    case "28727":
                    case "28754":
                        mMACROGB = "21987";
                        break;
                    case "15317":
                    case "13662":
                        mMACROGB = "15317";
                        break;
                    default:
                        mMACROGB = clsType.User.IdNumber;
                        break;
                }

                trvMACROGrp.Nodes.Clear();
                LoadTable();
            }
        }

        private void trvMACROGrp_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {            
            ssData_Sheet1.RowCount = 0;
            ssAction_Sheet1.RowCount = 0;
            ssResponse_Sheet1.RowCount = 0;
            ssNara_Sheet1.RowCount = 0;

            mlngMACROINDEX = Convert.ToInt32(VB.Val(e.Node.Name.Split('_')[1]));

            txtProbDA.Text = e.Node.Text.Trim();
            txtProbR.Text = e.Node.Text.Trim();
            txtProbN.Text = e.Node.Text.Trim();

            GetDtlInfo("DATA", ssData_Sheet1);
            GetDtlInfo("ACTION", ssAction_Sheet1);
            GetDtlInfo("RESULT", ssResponse_Sheet1);
            GetDtlInfo("NARA", ssNara_Sheet1);

        }

        private void GetDtlInfo(string strOption, FarPoint.Win.Spread.SheetView spd)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            
            spd.RowCount = 0;
            
            try
            {                
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ";
                SQL = SQL + ComNum.VBLF + "  A.MACROCD, ";
                SQL = SQL + ComNum.VBLF + "  A.MACROSEQ, A.MACROTEXT, A.MACRODSP";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.EMRMACROETCDTL A";
                SQL = SQL + ComNum.VBLF + "          WHERE A.MACROGB = '" + mMACROGB + "'";
                SQL = SQL + ComNum.VBLF + "          AND A.MACROINDEX = " + mlngMACROINDEX;
                SQL = SQL + ComNum.VBLF + "          AND A.MACROCD = '"+ strOption + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    spd.RowCount = dt.Rows.Count;
                    spd.SetRowHeight(-1, 20);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        spd.Cells[i, 0].Value = true;
                        spd.Cells[i, 1].Text = dt.Rows[i]["MACROTEXT"].ToString().Trim();
                        spd.Cells[i, 2].Text = dt.Rows[i]["MACRODSP"].ToString().Trim();
                        spd.Cells[i, 3].Text = dt.Rows[i]["MACROCD"].ToString().Trim();
                        spd.Cells[i, 4].Text = dt.Rows[i]["MACROSEQ"].ToString().Trim();
                        //spd.SetRowHeight(i, Convert.ToInt32(spd.GetPreferredRowHeight(i)) + 10);
                    }
                }                
                dt.Dispose();
                dt = null;                
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnMcrReg_Click(object sender, EventArgs e)
        {
            GvClear();

            using (frmNursingRecordJindanOld frm = new frmNursingRecordJindanOld())
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void btnNurCodeDAR_Click(object sender, EventArgs e)
        {
            pClearForm();

            using (frmEmrNurJinNew frm = new frmEmrNurJinNew(p.ward, p))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }

            if (clsEmrPublic.GstrNurCodeDAR != "")
            {
                txtProbDA.Text = ReadNurCodeDAR("3", clsEmrPublic.GstrNurCodeDAR);
                txtProbR.Text = txtProbDA.Text;
                txtProbN.Text = txtProbDA.Text;
            }
            
            ssData_Sheet1.RowCount = 0;
            if (clsEmrPublic.GstrNurData.Count > 0)
            {
                foreach (string s in clsEmrPublic.GstrNurData)
                {
                    ssData_Sheet1.RowCount = ssData_Sheet1.RowCount + 1;
                    ssData_Sheet1.SetRowHeight(-1, 20);

                    ssData_Sheet1.Cells[ssData_Sheet1.RowCount - 1, 0].Value = true;
                    ssData_Sheet1.Cells[ssData_Sheet1.RowCount - 1, 1].Text = s;
                }
            }

            ssAction_Sheet1.RowCount = 0;
            if (clsEmrPublic.GstrNurAction.Count > 0)
            {
                foreach (string s in clsEmrPublic.GstrNurAction)
                {
                    ssAction_Sheet1.RowCount = ssAction_Sheet1.RowCount + 1;
                    ssAction_Sheet1.SetRowHeight(-1, 20);

                    ssAction_Sheet1.Cells[ssAction_Sheet1.RowCount - 1, 0].Value = true;
                    ssAction_Sheet1.Cells[ssAction_Sheet1.RowCount - 1, 1].Text = s;
                }
            }

            ssResponse_Sheet1.RowCount = 0;
            if (clsEmrPublic.GstrNurResult1 != "")
            {
                ssResponse_Sheet1.RowCount = ssResponse_Sheet1.RowCount + 1;
                ssResponse_Sheet1.SetRowHeight(-1, 20);

                ssResponse_Sheet1.Cells[ssResponse_Sheet1.RowCount - 1, 0].Value = true;
                ssResponse_Sheet1.Cells[ssResponse_Sheet1.RowCount - 1, 1].Text = clsEmrPublic.GstrNurResult1;
            }

            READ_NUR_PROBLEM(p.ptNo, p.medFrDate);

            tabNr.SelectedIndex = 0;
        }


        private void READ_NUR_PROBLEM(string strPtno, string strMedFrDate)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssNurProblem_Sheet1.RowCount = 2;
            ssNurProblem_Sheet1.ColumnCount = 0;

            ssNurProblem_Sheet1.Rows[0].Label = "문제";
            ssNurProblem_Sheet1.Rows[1].Label = "목표";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT A.RANKING, A.SEQNO, B.NURPROBLEM, A.GOAL, TO_CHAR(A.SDATE,'YY/MM/DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.EDATE,'YY/MM/DD') EDATE, A.BIGO, A.ROWID, '1' GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.EMR_CADEX_NURPROBLEM A, ADMIN.EMR_CADEX_NURPROBLEM_CODE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.SEQNO = B.SEQNO";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.MEDFRDATE = '" + strMedFrDate + "' ";
                if (chkNurProblemDel.Checked == true) SQL = SQL + ComNum.VBLF + "   AND A.EDATE IS NULL";

                SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT RANKING, 0, PROBLEM, GOAL, TO_CHAR(SDATE,'YY/MM/DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(EDATE,'YY/MM/DD') EDATE, '' BIGO, A.ROWID, '2' GUBUN";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.NUR_CARE_GOAL A";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO = '" + strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.INDATE = TO_DATE('" + strMedFrDate + "','YYYY-MM-DD')  ";
                if (chkNurProblemDel.Checked == true) SQL = SQL + ComNum.VBLF + "   AND EDATE IS NULL";

                SQL = SQL + ComNum.VBLF + " ORDER BY RANKING ASC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {                    
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssNurProblem_Sheet1.ColumnCount += 1;
                        ssNurProblem_Sheet1.Cells[0, ssNurProblem_Sheet1.ColumnCount - 1].Text = "#" + dt.Rows[i]["RANKING"].ToString().Trim() + "." + dt.Rows[i]["NURPROBLEM"].ToString().Trim();
                        ssNurProblem_Sheet1.Cells[0, ssNurProblem_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                        ssNurProblem_Sheet1.Cells[0, ssNurProblem_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 210);
                        ssNurProblem_Sheet1.Columns[ssNurProblem_Sheet1.ColumnCount - 1].Width = ssNurProblem_Sheet1.Columns[ssNurProblem_Sheet1.ColumnCount - 1].GetPreferredWidth() + 4;

                        ssNurProblem_Sheet1.ColumnCount += 1;
                        ssNurProblem_Sheet1.Cells[0, ssNurProblem_Sheet1.ColumnCount - 1].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                        ssNurProblem_Sheet1.Columns[ssNurProblem_Sheet1.ColumnCount - 1].Width = ssNurProblem_Sheet1.Columns[ssNurProblem_Sheet1.ColumnCount - 1].GetPreferredWidth() + 4;

                        ssNurProblem_Sheet1.ColumnCount += 1;
                        ssNurProblem_Sheet1.Cells[0, ssNurProblem_Sheet1.ColumnCount - 1].Text = dt.Rows[i]["EDATE"].ToString().Trim();
                        ssNurProblem_Sheet1.Columns[ssNurProblem_Sheet1.ColumnCount - 1].Width = ssNurProblem_Sheet1.Columns[ssNurProblem_Sheet1.ColumnCount - 1].GetPreferredWidth() + 4;

                        ssNurProblem_Sheet1.ColumnCount += 1;
                        ssNurProblem_Sheet1.Columns[ssNurProblem_Sheet1.ColumnCount - 1].Visible = false;
                        ssNurProblem_Sheet1.Cells[0, ssNurProblem_Sheet1.ColumnCount - 1].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        ssNurProblem_Sheet1.Cells[1, ssNurProblem_Sheet1.ColumnCount - 4].Text = dt.Rows[i]["GOAL"].ToString().Trim();
                        ssNurProblem_Sheet1.Cells[1, ssNurProblem_Sheet1.ColumnCount - 4].ForeColor = Color.FromArgb(0, 0, 0);
                        ssNurProblem_Sheet1.Cells[1, ssNurProblem_Sheet1.ColumnCount - 4].BackColor = Color.FromArgb(255, 225, 210);
                        ssNurProblem_Sheet1.Columns[ssNurProblem_Sheet1.ColumnCount - 4].Width = ssNurProblem_Sheet1.Columns[ssNurProblem_Sheet1.ColumnCount - 3].GetPreferredWidth() + 4;

                        ssNurProblem_Sheet1.Cells[1, ssNurProblem_Sheet1.ColumnCount - 1].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssNurProblem_Sheet1.AddSpanCell(1, ssNurProblem_Sheet1.ColumnCount - 4, 1, 4);

                        if (ssNurProblem_Sheet1.Columns[i * 4].GetPreferredWidth() > ssNurProblem_Sheet1.Columns[ssNurProblem_Sheet1.ColumnCount - 1].GetPreferredWidth())
                        {
                            ssNurProblem_Sheet1.Columns[i * 4].Width = ssNurProblem_Sheet1.Columns[i * 4].GetPreferredWidth() + 5;
                        }
                        else
                        {
                            ssNurProblem_Sheet1.Columns[i * 4].Width = ssNurProblem_Sheet1.Columns[ssNurProblem_Sheet1.ColumnCount - 1].GetPreferredWidth() + 5;
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private string ReadNurCodeDAR(string strGubun, string strCode)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                
                SQL = " SELECT NAME ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.NUR_CODE_DAR ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '" + strGubun + "' ";
                SQL = SQL + ComNum.VBLF + "    AND CODE = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                }
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private void GvClear()
        {
            clsEmrPublic.GstrNurCodeDAR = "";
            clsEmrPublic.GstrNurData.Clear();
            clsEmrPublic.GstrNurAction.Clear();
            clsEmrPublic.GstrNurResult.Clear();
        }

        private void btnAplayDA_Click(object sender, EventArgs e)
        {
            string strTemp = "";

            strTemp = "";
            for (int i = 0; i < ssData_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssData_Sheet1.Cells[i, 0].Value) == true)
                {
                    strTemp = strTemp + ssData_Sheet1.Cells[i, 1].Text + ComNum.VBLF;
                }
            }
            if (strTemp != "")
            {
                strTemp = VB.Left(strTemp, strTemp.Length - 2);
                txtData.Text = strTemp;
            }

            strTemp = "";
            for (int i = 0; i < ssAction_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssAction_Sheet1.Cells[i, 0].Value) == true)
                {
                    strTemp = strTemp + ssAction_Sheet1.Cells[i, 1].Text + ComNum.VBLF;
                }
            }
            if (strTemp != "")
            {
                strTemp = VB.Left(strTemp, strTemp.Length - 2);
                txtAction.Text = strTemp;
            }
        }

        private void btnAplayR_Click(object sender, EventArgs e)
        {
            string strTemp = "";

            strTemp = "";
            for (int i = 0; i < ssResponse_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssResponse_Sheet1.Cells[i, 0].Value) == true)
                {
                    strTemp = strTemp + ssResponse_Sheet1.Cells[i, 1].Text + ComNum.VBLF;
                }
            }
            if (strTemp != "")
            {
                strTemp = VB.Left(strTemp, strTemp.Length - 2);
                txtResult.Text = strTemp;
            }
        }

        private void btnAplayN_Click(object sender, EventArgs e)
        {
            string strTemp = "";

            strTemp = "";
            for (int i = 0; i < ssNara_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssNara_Sheet1.Cells[i, 0].Value) == true)
                {
                    strTemp = strTemp + ssNara_Sheet1.Cells[i, 1].Text + ComNum.VBLF;
                }
            }
            if (strTemp != "")
            {
                strTemp = VB.Left(strTemp, strTemp.Length - 2);
                txtNarration.Text = strTemp;
            }
        }

        private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (optDept.Checked == true)
            {
                panWard.Visible = true;

                //mOption = "D";
                if (cboWard.Items.Count == 0)
                {
                    mMACROGB = clsType.User.DeptCode;
                }
                else
                {
                    mMACROGB = VB.Trim(VB.Right(cboWard.Text, 10));
                }

                trvMACROGrp.Nodes.Clear();
                LoadTable();
            }
        }

        private void ssNurProblem_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strGUBUN = "";
            string strROWID = "";

            if (e.Row == 0 && (e.Column + 2) % 4 == 0)
            {
                strROWID = ssNurProblem.ActiveSheet.Cells[0, e.Column + 1].Text;
                strGUBUN = ssNurProblem.ActiveSheet.Cells[1, e.Column + 1].Text;

                if (strGUBUN == "2")
                {
                    if (ComFunc.MsgBoxQ("오늘날짜로 종료일을 입력합니다. 계속하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        try
                        {

                            SQL = " UPDATE ADMIN.NUR_CARE_GOAL SET ";
                            SQL = SQL + ComNum.VBLF + " EDATE = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            READ_NUR_PROBLEM(p.ptNo, p.medFrDate);
                        }
                        catch (Exception ex)
                        {
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;                            
                            return;                        
                        }
                    }
                }
                else
                {
                    ComFunc.MsgBox("카덱스에서 등록한 내용입니다. 카덱스에서 작업하시기 바랍니다.");
                }

                return;
            }


            if (VB.Left(ssNurProblem.ActiveSheet.Cells[e.Row, e.Column].Text, 1) == "#")
            {
                strROWID = ssNurProblem.ActiveSheet.Cells[0, e.Column + 3].Text;
                strGUBUN = ssNurProblem.ActiveSheet.Cells[1, e.Column + 3].Text;

                if (strGUBUN == "2")
                {
                    if (ComFunc.MsgBoxQ("해당 간호문제를 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        try
                        {
                            SQL = " DELETE ADMIN.NUR_CARE_GOAL ";
                            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            READ_NUR_PROBLEM(p.ptNo, p.medFrDate);
                        }
                        catch (Exception ex)
                        {
                            clsDB.SaveSqlErrLog(ex.Message, "", clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                else
                {
                    ComFunc.MsgBox("카덱스에서 등록한 내용입니다. 카덱스에서 작업하시기 바랍니다.");
                }
            }
        }

        private void ssData_CellClick(object sender, CellClickEventArgs e)
        {
            
        }

        private void ssAction_CellClick(object sender, CellClickEventArgs e)
        {
            
        }

        private void chkDATA_CheckedChanged(object sender, EventArgs e)
        {
            if (ssData_Sheet1.RowCount == 0) return;

            if (chkDATA.Checked == true)
            {
                ssData_Sheet1.Cells[0, 0, ssData_Sheet1.RowCount - 1, 0].Value = true;
            }
            else
            {
                ssData_Sheet1.Cells[0, 0, ssData_Sheet1.RowCount - 1, 0].Value = false;
            }
        }

        private void chkACTION_CheckedChanged(object sender, EventArgs e)
        {
            if (ssAction_Sheet1.RowCount == 0) return;

            if (chkACTION.Checked == true)
            {
                ssAction_Sheet1.Cells[0, 0, ssAction_Sheet1.RowCount - 1, 0].Value = true;
            }
            else
            {
                ssAction_Sheet1.Cells[0, 0, ssAction_Sheet1.RowCount - 1, 0].Value = false;
            }
        }

        private void chkRESPONSE_CheckedChanged(object sender, EventArgs e)
        {
            if (ssResponse_Sheet1.RowCount == 0) return;

            if (chkRESPONSE.Checked == true)
            {
                ssResponse_Sheet1.Cells[0, 0, ssResponse_Sheet1.RowCount - 1, 0].Value = true;
            }
            else
            {
                ssResponse_Sheet1.Cells[0, 0, ssResponse_Sheet1.RowCount - 1, 0].Value = false;
            }
        }

        private void chkNARRATION_CheckedChanged(object sender, EventArgs e)
        {
            if (ssNara_Sheet1.RowCount == 0) return;

            if (chkNARRATION.Checked == true)
            {
                ssNara_Sheet1.Cells[0, 0, ssNara_Sheet1.RowCount - 1, 0].Value = true;
            }
            else
            {
                ssNara_Sheet1.Cells[0, 0, ssNara_Sheet1.RowCount - 1, 0].Value = false;
            }
        }

        private void chkNurProblemDel_CheckedChanged(object sender, EventArgs e)
        {
            READ_NUR_PROBLEM(p.ptNo, p.medFrDate);
        }

        private void ssList_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (ssList_Sheet1.RowCount == 0)
                    return;


                //string strName = ssList_Sheet1.Cells[e.Row, 18].Text.Trim();
                string strGUBUN = ssList_Sheet1.Cells[e.Row, 6].Text.Trim();
                string strEmrNo = ssList_Sheet1.Cells[e.Row, 10].Text.Trim();
                string GstrHelpCode = string.Empty;

                #region 쿼리
                OracleDataReader reader = null;
                string SQL = " SELECT PROBLEMCODE FROM ADMIN.NUR_NURSE_RECODE";
                SQL += ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if(reader.HasRows && reader.Read())
                {
                    GstrHelpCode = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                #endregion


                if (strGUBUN.Equals("D") || strGUBUN.Equals("A") && string.IsNullOrWhiteSpace(GstrHelpCode) == false)
                {

                    using (frmEmrNurJinResult frm = new frmEmrNurJinResult(GstrHelpCode))
                    {
                        frm.StartPosition = FormStartPosition.CenterParent;
                        frm.ShowDialog(this);
                    }

                    ssResponse_Sheet1.RowCount = 0;
                    if (clsEmrPublic.GstrNurResult.Count > 0)
                    {
                        for(int i = 0; i < clsEmrPublic.GstrNurResult.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(clsEmrPublic.GstrNurResult[i].Trim()) == false)
                            {
                                ssResponse_Sheet1.RowCount += 1;
                                ssResponse_Sheet1.Cells[ssResponse_Sheet1.RowCount - 1, 1].Text = clsEmrPublic.GstrNurResult[i].Trim();
                                ssResponse_Sheet1.Cells[ssResponse_Sheet1.RowCount - 1, 0].Text = "True";
                                ssResponse_Sheet1.Rows[ssResponse_Sheet1.RowCount - 1].Height = ssResponse_Sheet1.Rows[ssResponse_Sheet1.RowCount - 1].GetPreferredHeight() + 5;
                            }
                        }

                        if (clsEmrPublic.GstrNurCodeDAR.Length > 0)
                        {
                            txtProbR.Text = ReadNurCodeDAR("3", clsEmrPublic.GstrNurCodeDAR);
                        }

                        tabNr.SelectedIndex = 1;
                    }

                }
            }
        }

        private bool IsOldChartFormByEmrNo(string strEmrNo)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT '1' AS OLDGB                    ";
                SQL += ComNum.VBLF + "  FROM ADMIN.EMRXML A        ";
                SQL += ComNum.VBLF + " WHERE EMRNO = '"+ strEmrNo + "'  ";
                SQL += ComNum.VBLF + " UNION ALL                        ";
                SQL += ComNum.VBLF + "SELECT B.OLDGB                    ";
                SQL += ComNum.VBLF + "  FROM ADMIN.AEMRCHARTMST A  ";
                SQL += ComNum.VBLF + " INNER JOIN ADMIN.AEMRFORM B ";
                SQL += ComNum.VBLF + "    ON A.FORMNO = B.FORMNO        ";
                SQL += ComNum.VBLF + "   AND A.UPDATENO = B.UPDATENO    ";
                SQL += ComNum.VBLF + " WHERE EMRNO = '" + strEmrNo + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["OLDGB"].ToString().Trim() == "1")
                    {
                        rtnVal = true;
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVal;
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
                return rtnVal;
            }
        }
    }
}
