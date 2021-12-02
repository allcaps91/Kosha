using ComBase;
using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBaseProgressOcsNew : Form, MainFormMessage, FormEmrMessage
    {
        EmrForm fWrite = null;
        Form ActiveFormWrite = null;
        EmrChartForm ActiveFormWriteChart = null;
        const string NewEmrStartDate = "2020-04-22 07:00";
        #region //MainFormMessage
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

        #region //FormEmrMessage
        public ComEmrBase.FormEmrMessage mEmrCallForm = null;

        public void MsgSave(string strSaveFlag)
        {
            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;
            }

            if (strSaveFlag == "ORD")
            {

            }
            else
            {
                if (baseContinuView == null)
                    return;

                baseContinuView.SetContinuView();
            }
        }

        public void MsgDelete()
        {
            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;
            }

            if (baseContinuView == null)
            {
                return;
            }
            baseContinuView.SetContinuView();
            //baseContinuView.GetJupHis(txtPtNo.Text.Trim());
        }

        public void MsgClear()
        {
        }

        public void MsgPrint()
        {

        }

        #endregion

        #region 생성자
        public frmEmrBaseProgressOcsNew()
        {
            InitializeComponent();
        }

        public frmEmrBaseProgressOcsNew(EmrPatient emrPatient)
        {
            InitializeComponent();
            AcpEmr = emrPatient;
        }
        #endregion

        #region 서브폼 선언부
        EmrPatient AcpEmr = null;
        frmEmrBaseContinuView baseContinuView = null;

        //TODO 신규EMR
        frmEmrBaseProgressImage progressImage = null;
        frmEmrChartNew progressImage_New = null;

        frmEmrForm_Progress_New progress_New = null;
        frmEmrForm_Progress_New2 progress_New2 = null;

        frmEmrFormSearch fEmrFormSearch = null;  //폼조회

        private void SubFormToControl(Form frm, Control pControl, string DockForm, bool FitSize_H, bool FitSize_W)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.Text = "";
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            if (FitSize_H == true)
            {
                frm.Height = pControl.Height;
            }
            if (FitSize_W == true)
            {
                frm.Width = pControl.Width;
            }
            if (DockForm == "Fill")
            {
                frm.Dock = DockStyle.Fill;
            }
            frm.Show();

        }
        #endregion

        #region //Public Function

        public void FormExit()
        {
            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;
            }

            if (baseContinuView != null)
            {
                baseContinuView.Dispose();
                baseContinuView = null;
            }

            if (progressImage != null)
            {
                progressImage.Dispose();
                progressImage = null;
            }

            if (progress_New != null)
            {
                progress_New.Dispose();
                progress_New = null;
            }

            this.SuspendLayout();
        }

        /// <summary>
        /// 환자정보가 바뀔 경우 : 환자 정보를 갱신한다
        /// </summary>
        /// <param name="AcpEmr"></param>
        public void SetPatInfo(EmrPatient pAcpEmr)
        {
            using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("EmrSetting"))
            {
                //EMR 연속보기(0)/텍스트작성(1) 설정
                string strEmrGb = reg.GetValue("EmrGbnSet", string.Empty).ToString();
                (strEmrGb.Equals("1") ? rdoText : rdoView).Checked = true;
                reg.Close();
            }

            SetContinuView();

            AcpEmr = pAcpEmr;

            if (baseContinuView != null)
            {
                baseContinuView.GetContinuView(AcpEmr.ptNo, AcpEmr.inOutCls);
            }

            if (progress_New != null)
            {
                progress_New.Dispose();
                progress_New = null;
            }

            if (progressImage != null)
            {
                progressImage.Dispose();
                progressImage = null;
            }

            if (progress_New2 != null)
            {
                progress_New2.Dispose();
                progress_New2 = null;
            }

            if (progressImage_New != null)
            {
                progressImage_New.Dispose();
                progressImage_New = null;
            }

            if (Convert.ToDateTime(clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime) >= Convert.ToDateTime(NewEmrStartDate))
            {
                #region // 외래 진료과별 부분오픈
                //if (clsOrdFunction.GstrGbJob.Equals("OPD") && 
                //    (!clsType.User.DeptCode.Equals("NP") && !clsType.User.DeptCode.Equals("DM") && !clsType.User.DeptCode.Equals("OG") && 
                //     !clsType.User.DeptCode.Equals("OS") && !clsType.User.DeptCode.Equals("NS") && !clsType.User.DeptCode.Equals("CS") &&
                //     !clsType.User.DeptCode.Equals("UR") && !clsType.User.DeptCode.Equals("RM") && !clsType.User.DeptCode.Equals("NE") && !clsType.User.DeptCode.Equals("MI"))
                //    )
                //{
                //    //TODO 구EMR
                //    progress_New = new frmEmrForm_Progress_New(AcpEmr, this);
                //    progress_New.SetPatInfo(AcpEmr);
                //    if (progress_New != null)
                //    {
                //        SubFormToControl(progress_New, panText, "None", false, false);
                //    }

                //    progressImage = new frmEmrBaseProgressImage(AcpEmr, this);
                //    progressImage.SetPatInfo(AcpEmr);
                //    if (progressImage != null)
                //    {
                //        SubFormToControl(progressImage, panImg, "None", false, false);
                //    }
                //}
                //else
                //{
                //    //TODO 신규EMR
                //    progress_New2 = new frmEmrForm_Progress_New2(AcpEmr, this);
                //    progress_New2.SetPatInfo(AcpEmr);
                //    if (progress_New2 != null)
                //    {
                //        SubFormToControl(progress_New2, panText, "None", false, false);
                //    }

                //    progressImage_New = new frmEmrChartNew("1232", clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, 1232).ToString(), AcpEmr, "0", "W", mEmrCallForm);
                //    if (progressImage_New != null)
                //    {
                //        SubFormToControl(progressImage_New, panImg, "None", false, false);
                //    }
                //}
                #endregion

                //TODO 신규EMR
                progress_New2 = new frmEmrForm_Progress_New2(AcpEmr, this);
                progress_New2.SetPatInfo(AcpEmr);
                if (progress_New2 != null)
                {
                    SubFormToControl(progress_New2, panText, "None", false, false);
                }

                progressImage_New = new frmEmrChartNew("1232", clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, 1232).ToString(), AcpEmr, "0", "W", mEmrCallForm);
                if (progressImage_New != null)
                {
                    SubFormToControl(progressImage_New, panImg, "None", false, false);
                }
            }
            else
            {
                //TODO EMR
                progress_New = new frmEmrForm_Progress_New(AcpEmr, this);
                progress_New.SetPatInfo(AcpEmr);
                if (progress_New != null)
                {
                    SubFormToControl(progress_New, panText, "None", false, false);
                }

                progressImage = new frmEmrBaseProgressImage(AcpEmr, this);
                progressImage.SetPatInfo(AcpEmr);
                if (progressImage != null)
                {
                    SubFormToControl(progressImage, panImg, "None", false, false);
                }
            }

            ResizeForm();
        }

        public void ClearPatInfo()
        {
            SetContinuView();

            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;
            }

            AcpEmr = null;
            if (baseContinuView != null)
            {
                baseContinuView.ClearForm();
            }

            //TODO 신규EMR
            if (progress_New2 != null)
            {
                progress_New2.pClearForm();
            }

            if (progressImage_New != null)
            {
                progressImage_New.pClearForm();
            }

            if (progress_New != null)
            {
                progress_New.pClearForm();
            }

            if (progressImage != null)
            {
                progressImage.ClearPatInfo();
            }
        }

        public bool CheckAndSaveEmr()
        {
            bool rtnVal = true;
            string strChangeChart = string.Empty;

            #region //EMR 저장시 메시지 박스 기본 버튼(예1, 아니오0)
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button2;

            try
            {
                RegistryKey reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("EmrSetting");
                string strEmrGb = reg.GetValue("EmrSaveMsg", "0").ToString();
                reg.Close();
                reg.Dispose();

                defaultButton = strEmrGb.Equals("1") ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, "기본 메시지 박스를 불러오는중 오류가 발생했습니다." + ComNum.VBLF +
                    ex.Message);
            } 
            #endregion

            try
            {
                if (progress_New != null)
                {
                    strChangeChart = progress_New.CheckChartChangeData();
                    if (strChangeChart != "")
                    {
                        if (ComFunc.MsgBoxQEx(this, strChangeChart + ComNum.VBLF + "저장 하시겠습니까?", "저장", defaultButton) == DialogResult.Yes)
                        {
                            double pEmrNo = progress_New.SetSaveData();
                            if (pEmrNo == 0)
                            {
                                rtnVal = false;
                            }
                        }
                    }
                }
                if (progress_New2 != null)
                {
                    strChangeChart = progress_New2.CheckChartChangeData();
                    if (strChangeChart != "")
                    {
                        if (ComFunc.MsgBoxQEx(this, strChangeChart + ComNum.VBLF + "저장 하시겠습니까?", "저장", defaultButton) == DialogResult.Yes)
                        {
                            double pEmrNo = progress_New2.SetSaveData();
                            if (pEmrNo == 0)
                            {
                                rtnVal = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, "경과기록지 EMR 저장시 오류가 발생했습니다." + ComNum.VBLF +
                     ex.Message);
            }

            try
            {
                if(baseContinuView != null)
                {
                    strChangeChart = baseContinuView.CheckChartChangeData();
                    if (strChangeChart != "")
                    {
                        if (ComFunc.MsgBoxQEx(this, strChangeChart + ComNum.VBLF + "저장 하시겠습니까?", "저장", defaultButton) == DialogResult.Yes)
                        {
                            double pEmrNo = baseContinuView.SaveData();
                            if (pEmrNo == 0)
                            {
                                rtnVal = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, "연속보기 EMR 저장시 오류가 발생했습니다." + ComNum.VBLF +
                     ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// Progress 초기 및 작성 후 화면 옵션 처리 함수
        /// </summary>
        public void SetChartView()
        {
            rdoView.Checked = true;
        }
        #endregion //Public Function

        private void FrmEmrBaseProgressOcsNew_Load(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            panFormSearch.Left = 80;
            panFormSearch.Top = 3;
            panFormSearch.Width = 520;
            panFormSearch.Height = 771;
            panFormSearch.Visible = false;

            panView.Dock = DockStyle.Fill;
            panView.BringToFront();

            panImg.Dock = DockStyle.Fill;
            panText.Dock = DockStyle.Fill;

            Application.DoEvents();

            LoadSubForm();

            using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("EmrSetting"))
            {
                //EMR 연속보기(0)/텍스트작성(1) 설정
                string strEmrGb = reg.GetValue("EmrGbnSet", string.Empty).ToString();
                (strEmrGb.Equals("1") ? rdoText : rdoView).Checked = true;
                reg.Close();
            }

            ResizeForm();
        }

        void LoadSubForm()
        {
            string strEmrGb = string.Empty;
            using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("EmrSetting"))
            {
                strEmrGb = reg.GetValue("VIEWORDERVISIBLE", "1").ToString();
                rdoView.Visible = strEmrGb.Equals("1");

                if (strEmrGb.Equals("1"))
                {
                    baseContinuView = new frmEmrBaseContinuView();

                    if (baseContinuView != null)
                    {
                        SubFormToControl(baseContinuView, panView, "Fill", true, true);
                    }

                    if (AcpEmr != null)
                    {
                        baseContinuView.GetContinuView(AcpEmr.ptNo, AcpEmr.inOutCls);
                    }
                }
                else
                {
                    //삭제 후 저장 하였는데 살아있으면 폼 삭제 처리
                    if (baseContinuView != null && baseContinuView.IsDisposed == false)
                    {
                        baseContinuView.Dispose();
                        baseContinuView = null;
                    }
                }
            }

          
                        
            progress_New = new frmEmrForm_Progress_New(AcpEmr, this);
            if (progress_New != null)
            {
                SubFormToControl(progress_New, panText, "None", false, false);
            }

            progressImage = new frmEmrBaseProgressImage(AcpEmr, this);
            if (progressImage != null)
            {
                SubFormToControl(progressImage, panImg, "None", false, false);
            }
        }

        /// <summary>
        /// 21-06-30 설정 값 체크 후 폼 생성/삭제
        /// </summary>
        private void SetContinuView()
        {
            string strEmrGb = string.Empty;
            using (RegistryKey reg = Registry.LocalMachine.CreateSubKey("Software").CreateSubKey("EmrSetting"))
            {
                strEmrGb = reg.GetValue("VIEWORDERVISIBLE", "1").ToString();

                rdoView.Visible = strEmrGb.Equals("1");

                if (strEmrGb.Equals("1"))
                {
                    if (baseContinuView == null)
                    {
                        baseContinuView = new frmEmrBaseContinuView();
                        SubFormToControl(baseContinuView, panView, "Fill", true, true);
                    }
                }
                else
                {
                    //삭제 후 저장 하였는데 살아있으면 폼 삭제 처리
                    if (baseContinuView != null && baseContinuView.IsDisposed == false)
                    {
                        baseContinuView.Close();
                        baseContinuView.Dispose();
                        baseContinuView = null;
                    }
                }
            }
        }

        private void BaseContinuView_rEventClosed()
        {
            if (baseContinuView != null)
            {
                baseContinuView.Dispose();
                baseContinuView = null;
            }
        }

        /// <summary>
        /// 연속보기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RdoView_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoView.Checked == false)
                return;

            panImg.Visible = false;
            panText.Visible = false;

            panView.BringToFront();
            panView.Visible = true;
        }

        /// <summary>
        /// 경과이미지 작성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RdoImg_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoImg.Checked == false)
                return;

            panText.Visible = false;
            panView.Visible = false;

            panImg.BringToFront();
            panImg.Visible = true;
        }

        /// <summary>
        /// 경과기록지 작성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RdoText_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoText.Checked == false)
                return;

            panImg.Visible = false;
            panView.Visible = false;

            panText.BringToFront();
            panText.Visible = true;
        }

        private void BtnSearchRmk_Click(object sender, EventArgs e)
        {
            if (AcpEmr == null)
                return;

            using (frmEmrBaseSingularRemark frm = new frmEmrBaseSingularRemark(AcpEmr.ptNo))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void mbtnSheet_Click(object sender, EventArgs e)
        {
            //----전체기록
            if (fEmrFormSearch == null)
            {
                fEmrFormSearch = new frmEmrFormSearch();
                fEmrFormSearch.rSetWriteForm += new frmEmrFormSearch.SetWriteForm(frmEmrFormSearch_SetWriteForm);
                fEmrFormSearch.rEventClosed += new frmEmrFormSearch.EventClosed(frmEmrFormSearch_EventClosed);
                if (fEmrFormSearch != null)
                {
                    fEmrFormSearch.Owner = this;
                    fEmrFormSearch.TopLevel = false;
                    this.Controls.Add(fEmrFormSearch);
                    fEmrFormSearch.Parent = panFormSearch;
                    fEmrFormSearch.Text = "";
                    fEmrFormSearch.ControlBox = false;
                    fEmrFormSearch.FormBorderStyle = FormBorderStyle.None;
                    fEmrFormSearch.Top = 0;
                    fEmrFormSearch.Left = 0;
                    fEmrFormSearch.WindowState = FormWindowState.Normal;
                    fEmrFormSearch.Height = panFormSearch.Height;
                    fEmrFormSearch.Width = panFormSearch.Width;
                    //fEmrFormSearch.Dock = DockStyle.Fill;
                    fEmrFormSearch.Show();

                }
            }
            panFormSearch.BringToFront();
            panFormSearch.Visible = true;

        }

        private void frmEmrFormSearch_EventClosed()
        {
            panFormSearch.Visible = false;
            fEmrFormSearch.Close();
            fEmrFormSearch = null;
        }

        private void frmEmrFormSearch_SetWriteForm(EmrForm aWrite)
        {
            panFormSearch.Visible = false;
                       
            fWrite = aWrite;
            
            LoadChart(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString());
        }

        private void LoadChart(string strFormNo, string strUpdateNo)
        {
            if (fWrite.FmDOCFORMNAME.Trim() != "")
            {
                LoadWirteDoc(strFormNo, strUpdateNo);
            }
            else
            {
                LoadWirteForm(strFormNo, strUpdateNo);
            }
        }

        private void LoadWirteDoc(string strFormNo, string strUpdateNo)
        {
            if (VB.Val(strFormNo) != 0)
            {
                fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFormNo, strUpdateNo);

                if (fWrite == null)
                {
                    ComFunc.MsgBoxEx(this, "등록된 기록지 폼이 없습니다.");
                    fWrite = clsEmrChart.ClearEmrForm();
                    return;
                }
            }
        }

        private void LoadWirteForm(string strFormNo, string strUpdateNo)
        {
            if (VB.Val(strFormNo) != 0)
            {
                fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFormNo, strUpdateNo);

                if (fWrite == null)
                {
                    ComFunc.MsgBoxEx(this, "등록된 기록지 폼이 없습니다.");
                    fWrite = clsEmrChart.ClearEmrForm();
                    return;
                }

                #region 기록지 입원, 외래, 과 체크
                //폼에 등록한 과와 현재 
                if (fWrite.FmVISITSDEPT.Length > 0 && fWrite.FmVISITSDEPT.Equals(AcpEmr.medDeptCd) == false)
                {
                    ComFunc.MsgBoxEx(this, "현재 기록지는 " + fWrite.FmVISITSDEPT + "환자만 작성이 가능합니다.\r\n지금 챠트로 작성하시려고 하시는 환자는 '" + AcpEmr.medDeptCd + "'과입니다.\r\n해당 환자리스트에서 챠트버튼을 눌러서 작성하시거나,\r\n'EMR작성' 화면에서 맞는 내원내역을 선택후 작성해주세요.");
                    return;
                }

                //기록지 저장은 외래만 가능한데 환자 정보가 외래가 아닐경우
                if (fWrite.FmINOUTCLS == "1" && AcpEmr.inOutCls != "O")
                {
                    ComFunc.MsgBoxEx(this, "현재 기록지는 외래 혹은 응급실 환자만 작성이 가능합니다.\r\n해당 환자리스트에서 챠트버튼을 눌러서 작성하시거나,\r\n'EMR작성' 화면에서 맞는 내원내역을 선택후 작성해주세요.");
                    return;
                }

                //기록지 저장은 입원만 가능한데 환자 정보가 입원이 아닐경우
                if (fWrite.FmINOUTCLS == "2" && AcpEmr.inOutCls != "I")
                {
                    ComFunc.MsgBoxEx(this, "현재 기록지는 입원 환자만 작성이 가능합니다.\r\n입원 환자리스트에서 챠트버튼을 눌러서 작성하시거나,\r\n'EMR작성' 화면에서 맞는 내원내역을 선택후 작성해주세요.");
                    return;
                }
                #endregion


                string NewRecord = clsEmrQuery.NewArgreeRecord(clsDB.DbCon);

                string strEmrNo = "0";
                
                //string strVal = tAcp.medFrDate;
                string strVal = "";

                if (ActiveFormWrite != null)
                {
                    ActiveFormWrite.Dispose();
                    ActiveFormWrite = null;
                    ActiveFormWriteChart = null;
                }

                if (strFormNo == "963")
                {
                    fWrite.FmPROGFORMNAME = "frmEmrForm_Progress_New2";
                    fWrite.FmFORMTYPE = "4";
                }

                //1050~1055(동의서, 설문지, 안내문, 신청서, 정보정정관련자료, 개인정보관련동의서) 
                //1066(환자교육)
                //1068(설명문)
                if (NewRecord.Equals("N") && (fWrite.FmGRPFORMNO >= 1050 && fWrite.FmGRPFORMNO <= 1055 || fWrite.FmGRPFORMNO == 1066 || fWrite.FmGRPFORMNO == 1068))// || fWrite.FmFORMNO == 2148)
                {
                    ActiveFormWrite = new frmEmrBaseEmrChartOld(this, AcpEmr, "NEW", "", "", strFormNo, fWrite.FmFORMNAME, strEmrNo);
                    ((frmEmrBaseEmrChartOld)ActiveFormWrite).rSaveOrDelete += frmEmrBaseEmrChartOld_SaveOrDelete;
                    ((frmEmrBaseEmrChartOld)ActiveFormWrite).rEventClosed += frmEmrBaseEmrChartOld_EventClosed;

                }
                else if (fWrite.FmFORMTYPE == "4") //개발자가 만든것
                {
                    #region 19-07-05 추가 PrtSeq, 작성일자 생성자로 넘겨주기 위해서 추가된 생성자.
                    ActiveFormWrite = clsEmrFormMap.EmrFormMappingEx(fWrite.FmPROGFORMNAME, fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", strVal, this);
                    #endregion
                    //해당 생성자 없을경우 아래로 
                    if (ActiveFormWrite == null)
                    {
                        ActiveFormWrite = clsEmrFormMap.EmrFormMappingEx(fWrite.FmPROGFORMNAME, fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, "0", "W", this);
                    }

                }
                else if (fWrite.FmFORMTYPE == "3") //Flow
                {
                    ActiveFormWrite = new frmEmrChartFlowOld(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", "", strVal, this);
                }
                else if (fWrite.FmFORMTYPE == "2") //전자동의서
                {
                    ActiveFormWrite = new frmEmrChartNew(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                }
                else if (fWrite.FmFORMTYPE == "1") //동의서
                {
                    ActiveFormWrite = new frmEmrChartNew(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                }
                else if (fWrite.FmFORMTYPE == "0") //정형화 서식
                {
                    ActiveFormWrite = new frmEmrChartNew(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                }

                if (ActiveFormWrite == null)
                {
                    return;
                }

                ActiveFormWrite.FormClosed += ActiveFormWrite_FormClosed;
                if (NewRecord.Equals("N") && (fWrite.FmGRPFORMNO >= 1050 && fWrite.FmGRPFORMNO <= 1055 || fWrite.FmGRPFORMNO == 1066 || fWrite.FmGRPFORMNO == 1068))// || fWrite.FmFORMNO == 2148)
                {
                }
                else
                {
                    ActiveFormWriteChart = (EmrChartForm)ActiveFormWrite;
                }

                ActiveFormWrite.Owner = this;
                ActiveFormWrite.Show();
            }
        }

        private void frmEmrBaseEmrChartOld_SaveOrDelete()
        {
            if (ActiveFormWrite == null)
                return;

            ActiveFormWrite.Dispose();
            ActiveFormWrite = null;
            //GetChartHis();
        }

        private void frmEmrBaseEmrChartOld_EventClosed()
        {
            if (ActiveFormWrite == null)
                return;

            ActiveFormWrite.Dispose();
            ActiveFormWrite = null;
        }

        /// <summary>
        /// 사용자 서식 버튼으로 생성한 폼이 닫힐때.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActiveFormWrite_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender.Equals(ActiveFormWrite))
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;
            }
        }

        private void frmEmrBaseProgressOcsNew_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormWriteChart = null;
            }
        }

        private void frmEmrBaseProgressOcsNew_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }

        private void ResizeForm()
        {
            return;

            //try
            //{
            //    Application.DoEvents();
            //    if (baseContinuView != null)
            //    {
            //        baseContinuView.WindowState = FormWindowState.Normal;
            //        baseContinuView.Height = panView.Height + 80;
            //        baseContinuView.Width = panView.Width - 80;
            //    }
            //    Application.DoEvents();
            //}
            //catch
            //{

            //}
        }


    }
}
