using ComBase;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBaseChartWrite : Form, MainFormMessage, FormEmrMessage
    {
        #region //폼에서 사용하는 변수

        EmrPatient AcpEmr = null;
        EmrForm fWrite = null;

        /// <summary>
        /// 등록번호
        /// </summary>
        string mPTNO = string.Empty;

        /// <summary>
        /// 정신과 열람가능 여부
        /// </summary>
        //bool mViewNpChart = false;

        #endregion //폼에서 사용하는 변수

        #region //서브폼 선언부

        /// <summary>
        /// 내원내역
        /// </summary>
        frmEmrBaseAcpListWrite fEmrBaseAcpListWrite;  //내원내역

        Form ActiveFormWrite = null;
        EmrChartForm ActiveFormViewChart = null;

        #endregion

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
        public FormEmrMessage mEmrCallForm = null;
        public void MsgSave(string strSaveFlag)
        {
            //if (IsOrderSave == true)
            //{
            //    IsOrderSave = false;
            //    return;
            //}
            //IsOrderSave = false;
            ////----상용기록
            ////----전체기록
            //if (mstrEMRVIEWNEW == "1")
            //{
            //    if (fEmrChartHisUserDrNew != null)
            //    {
            //        fEmrChartHisUserDrNew.RvClearForm();
            //        fEmrChartHisUserDrNew.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //    if (fEmrChartHisAllDrNew != null)
            //    {
            //        fEmrChartHisAllDrNew.RvClearForm();
            //        fEmrChartHisAllDrNew.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //}
            //else
            //{
            //    if (fEmrChartHisUserDr != null)
            //    {
            //        fEmrChartHisUserDr.RvClearForm();
            //        fEmrChartHisUserDr.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //    if (fEmrChartHisAllDr != null)
            //    {
            //        fEmrChartHisAllDr.RvClearForm();
            //        fEmrChartHisAllDr.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //}

        }
        public void MsgDelete()
        {
            //if (IsOrderSave == true)
            //{
            //    IsOrderSave = false;
            //    return;
            //}
            //IsOrderSave = false;
            ////----상용기록
            ////----전체기록
            //if (mstrEMRVIEWNEW == "1")
            //{
            //    if (fEmrChartHisUserDrNew != null)
            //    {
            //        fEmrChartHisUserDrNew.RvClearForm();
            //        fEmrChartHisUserDrNew.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //    if (fEmrChartHisAllDrNew != null)
            //    {
            //        fEmrChartHisAllDrNew.RvClearForm();
            //        fEmrChartHisAllDrNew.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //}
            //else
            //{
            //    if (fEmrChartHisUserDr != null)
            //    {
            //        fEmrChartHisUserDr.RvClearForm();
            //        fEmrChartHisUserDr.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //    if (fEmrChartHisAllDr != null)
            //    {
            //        fEmrChartHisAllDr.RvClearForm();
            //        fEmrChartHisAllDr.RvPatInfo(AcpEmr);
            //        Application.DoEvents();
            //    }
            //}

        }
        public void MsgClear()
        {
            ComFunc.MsgBoxEx(this, "MsgClear");
        }
        public void MsgPrint()
        {

        }
        #endregion

        #region //이벤트 전달
      
        //폼이 Close될 경우
        //public delegate void EventClosed();
        //public event EventClosed rEventClosed;

        public void ClearForm()
        {
            fWrite = null;
            mPTNO = string.Empty;

            ClearChartRecInfo();

            if (fEmrBaseAcpListWrite != null)
            {
                fEmrBaseAcpListWrite.ClearForm();
            }

            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
                ActiveFormViewChart = null;
            }
        }

        public void GetJupHis(string pPTNO)
        {
            mPTNO = pPTNO;

            if (mPTNO.Trim() != "")
            {
                if (fEmrBaseAcpListWrite == null)
                {
                    LoadSubForm();
                }
                fEmrBaseAcpListWrite.GetJupHis(mPTNO);
            }
        }



        public void SetPTNO(string pPTNO)
        {
            mPTNO = pPTNO;
        }

        #endregion

        #region //공통함수

        private void ClearChartRecInfo()
        {
            lblInfo.Text = string.Empty;
            lblPtName.Text = string.Empty;
            lblViewFORMNAME.Text = string.Empty;
        }

        private void SetChartRecInfo(EmrPatient tAcp)
        {
            //lblDept.Text = tAcp.medDeptCd;
            mbtnMacro1.Left = lblViewFORMNAME.Left + lblViewFORMNAME.Width;
            lblInfo.Left = lblViewFORMNAME.Left + lblViewFORMNAME.Width;
            lblInfo.Text = string.Format("이름: {0}  주민번호: {1} - {2} ({3}/{4})  ", tAcp.ptName, tAcp.ssno1, tAcp.ssno2, tAcp.sex, tAcp.age);
            lblInfo.Text += string.Format("{0} {1} {2} {3} ", tAcp.inOutCls.Equals("O") ? "외래" : "입원", tAcp.medDeptKorName, clsVbfunc.GetOCSDrCodeDrName(clsDB.DbCon, tAcp.medDrCd),  VB.Val(tAcp.medFrDate).ToString("0000-00-00"));

        }

        private void SubFormToControl(Form frm, Control pControl)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.Text = string.Empty;
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            //frm.Dock = DockStyle.Fill;
            frm.Show();

        }

        private void LoadSubForm()
        {
            fEmrBaseAcpListWrite = new frmEmrBaseAcpListWrite();
            //fEmrBaseAcpListWrite.rEventClosed += new frmEmrBaseAcpListWrite.EventClosed(frmEmrBaseAcpList_EventClosed);
            fEmrBaseAcpListWrite.rSetWriteForm += new frmEmrBaseAcpListWrite.SetWriteForm(frmEmrBaseAcpListWrite_SetWriteForm);
            fEmrBaseAcpListWrite.rViewPatInfo += new frmEmrBaseAcpListWrite.ViewPatInfo(frmEmrBaseAcpListWrite_rViewPatInfo);
            if (fEmrBaseAcpListWrite != null)
            {
                SubFormToControl(fEmrBaseAcpListWrite, panViewEmrAcp);
            }
        }

        private void frmEmrBaseAcpListWrite_rViewPatInfo(EmrPatient tAcp)
        {
            if(tAcp == null)
            {
                ComFunc.MsgBoxEx(this, "접수내역을 찾을수 없습니다.");
                return;
            }
            AcpEmr = clsEmrChart.ClearPatient();
            AcpEmr = tAcp;

            //fEmrBaseAcpListWrite.SetPatInfo(AcpEmr);

            ClearChartRecInfo();
            SetChartRecInfo(AcpEmr);

            if (ActiveFormWrite != null)
            {
                ViewChart(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString());
            }
        }

        private void frmEmrBaseAcpList_EventClosed()
        {
            fEmrBaseAcpListWrite.Dispose();
            fEmrBaseAcpListWrite = null;
        }

        private void frmEmrBaseAcpListWrite_SetWriteForm(EmrForm aWrite)
        {
            fWrite = aWrite;
            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Close();
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
            }

            if (AcpEmr == null)
            {
                ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                return;
            }

            ViewChart(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString());
        }


        private void ResizeSubForm()
        {
            try
            {
                if (ActiveFormWrite == null) return;

                if (ActiveFormWrite.Name == "frmScanImageViewNew")
                {
                    ActiveFormWrite.WindowState = FormWindowState.Normal;
                    ActiveFormWrite.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    if (fWrite.FmALIGNGB == 1)   //Left
                    {
                        panOption.Visible = false;
                        ActiveFormWrite.Height = panEmrViewMain.Height - 20;
                    }
                    else if (fWrite.FmALIGNGB == 2)  //Top
                    {
                        panOption.Visible = false;
                        ActiveFormWrite.Width = panEmrViewMain.Width - 20;
                    }
                    else  //None
                    {
                        ActiveFormWrite.Dock = DockStyle.None;
                        ActiveFormWrite.Dock = DockStyle.Fill;
                    }
                }
            }
            catch { }
        }

        private void ViewChart(string strFormNo, string strUpdateNo)
        {
            mbtnMacro1.Visible = false;
            try
            {

                if (ActiveFormWrite != null)
                {
                    ActiveFormWrite.Close();
                    ActiveFormWrite.Dispose();
                    ActiveFormWrite = null;
                    ActiveFormViewChart = null;
                }

                ClearChartRecInfo();

                fWrite = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, strFormNo, strUpdateNo);

                if (fWrite == null)
                {
                    ComFunc.MsgBoxEx(this, "등록된 기록지 폼이 없습니다.");
                    fWrite = clsEmrChart.ClearEmrForm();
                    return;
                }

                string NewRecord = clsEmrQuery.NewArgreeRecord(clsDB.DbCon);

                if (fWrite.FmFORMNO == 963)
                {
                    fWrite.FmPROGFORMNAME = fWrite.FmOLDGB == 1 ? "frmEmrForm_Progress_New" : "frmEmrForm_Progress_New2";
                    fWrite.FmFORMTYPE = "4";
                }
                else if (fWrite.FmOLDGB == 1 && fWrite.FmFORMNO == 1232)
                {
                    fWrite.FmPROGFORMNAME = "frmEmrBaseProgressImage";
                    fWrite.FmFORMTYPE = "4";
                }

                //1050~1055(동의서, 설문지, 안내문, 신청서, 정보정정관련자료, 개인정보관련동의서) 
                //1066(환자교육)
                //1068(설명문)
                if (fWrite.FmGRPFORMNO >= 1050 && fWrite.FmGRPFORMNO <= 1055 || fWrite.FmGRPFORMNO == 1066 || fWrite.FmGRPFORMNO == 1068|| fWrite.FmGRPFORMNO == 1078 || fWrite.FmFORMNO == 2148 || fWrite.FmGRPFORMNO == 1081)
                {
                    //전자동의서
                    if (fWrite.FmFORMTYPE == "2")
                    {
                        EasManager easManager = EasManager.Instance;

                        //     panSplitV.Left = 50;
                      //  int xx = panSplitV.SplitPosition;
                    //    panSplitV.SplitPosition = 50;

                        ActiveFormWrite = easManager.GetEasFormViewer(panSplitV);
                        easManager.Write(fWrite, AcpEmr);

                        easManager.ShowTabletMoniror();
                        /*
                                                frmEasTabletViewer form = new frmEasTabletViewer(easManager, easManager.GetEasFormViewer().GetBound());
                                                form.Write(fWrite, AcpEmr);
                                                form.Show();
                                                form.BringToFront();
                                                */
                    }
                    else
                    {
                        if (NewRecord.Equals("N"))
                        {
                            ActiveFormWrite = new frmEmrBaseEmrChartOld(this, AcpEmr, "NEW", "", "", strFormNo, fWrite.FmFORMNAME, "0");
                            ((frmEmrBaseEmrChartOld)ActiveFormWrite).rSaveOrDelete += frmEmrBaseEmrChartOld_SaveOrDelete;
                        }
                        else
                        {
                            ActiveFormWrite = new frmEmrChartNew(strFormNo, strUpdateNo, AcpEmr, "0", "W", this);
                        }

                        //((frmEmrBaseEmrChartOld)ActiveFormWrite).rEventClosed += frmEmrBaseEmrChartOld_EventClosed;
                    }
                }
                else if (fWrite.FmFORMTYPE == "4") //개발자가 만든것
                {
                    //clsFormMap.EmrFormMapping("MHENRINS", strNameSpace, frmFORM.FmPROGFORMNAME, strFormNo, strUpdateNo, pEmrPatient, strEmrNo, "W");
                    #region 19-07-05 추가 PrtSeq, 작성일자 생성자로 넘겨주기 위해서 추가된 생성자.
                    ActiveFormWrite = clsEmrFormMap.EmrFormMappingEx(fWrite.FmPROGFORMNAME, fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, "0", "W", "", this);
                    #endregion
                    //해당 생성자 없을경우 아래로 
                    if (ActiveFormWrite == null)
                    {
                        ActiveFormWrite = clsEmrFormMap.EmrFormMappingEx(fWrite.FmPROGFORMNAME, fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, "0", "W", this);

                    }

                }
                else if (fWrite.FmFORMTYPE == "3")
                {
                    ActiveFormWrite = new frmEmrChartFlowOld(strFormNo, strUpdateNo, AcpEmr, "0", "W", this);

                }
                else
                {
                    ActiveFormWrite = new frmEmrChartNew(strFormNo, strUpdateNo, AcpEmr, "0", "W", this);
                    //물리치료실 일때만 이전내역.
                    if (clsType.User.BuseCode.Equals("055307"))
                    {
                        mbtnMacro1.Visible = true;
                    }
                }

                lblViewFORMNAME.Text = fWrite.FmFORMNAME;

                if (fWrite.FmALIGNGB == 1)   //Left
                {
                    panOption.Visible = false;
                    ActiveFormWrite.Height = panEmrViewMain.Height - 20;
                }
                else if (fWrite.FmALIGNGB == 2)  //Top
                {
                    panOption.Visible = false;
                    ActiveFormWrite.Width = panEmrViewMain.Width - 20;
                }
                else  //None
                {
                    ActiveFormWrite.Dock = DockStyle.None;
                    ActiveFormWrite.Dock = DockStyle.Fill;
                }

                //응급실의 경우는 입원 기록을 작성을 할 수 있도록 : 추후 입원시에 넘기도록 한다.
                //(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
                if (NewRecord.Equals("N"))
                {
                    #region 이전 동의서
                    if (fWrite.FmGRPFORMNO >= 1050 && fWrite.FmGRPFORMNO <= 1055 || fWrite.FmGRPFORMNO == 1066 || fWrite.FmGRPFORMNO == 1068 || fWrite.FmFORMNO == 2148)
                    {
                    }
                    else
                    {
                        ActiveFormViewChart = (EmrChartForm)ActiveFormWrite;
                    }
                    #endregion
                }
                else
                {
                    ActiveFormViewChart = (EmrChartForm)ActiveFormWrite;
                }

                ActiveFormWrite.TopLevel = false;
                this.Controls.Add(ActiveFormWrite);
                ActiveFormWrite.Parent = panEmrViewMain;
                ActiveFormWrite.Text = ActiveFormWrite.Text;
                ActiveFormWrite.ControlBox = false;
                ActiveFormWrite.FormBorderStyle = FormBorderStyle.None;
                ActiveFormWrite.Top = 0;
                ActiveFormWrite.Left = 0;

                ActiveFormWrite.Show();

                SetChartRecInfo(AcpEmr);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void frmEmrBaseEmrChartOld_SaveOrDelete()
        {
            ActiveFormWrite.Dispose();
            ActiveFormWrite = null;
            //GetChartHis();
        }

        private void frmEmrBaseEmrChartOld_EventClosed()
        {
            ActiveFormWrite.Dispose();
            ActiveFormWrite = null;
        }

        #endregion

        #region //생성자 및 폼 이벤트
        public frmEmrBaseChartWrite()
        {
            InitializeComponent();
        }

        public frmEmrBaseChartWrite(string strPtno)
        {
            InitializeComponent();
            mPTNO = strPtno;
        }

        private void FrmEmrBaseChartWrite_Load(object sender, EventArgs e)
        {
           
            LoadSubForm();

            panSideBarLeft.Visible = false;
        }

        private void frmEmrBaseChartWrite_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }
        #endregion //생성자 및 폼 이벤트


        #region //컨트롤 이벤트
        private void mbtnMacro1_Click(object sender, EventArgs e)
        {
            if (ActiveFormWrite == null)
            {
                return;
            }

            string strFormNo = fWrite.FmFORMNO.ToString();
            //물리치료 일때
            if (fWrite.FmGRPFORMNO == 1031 && fWrite.FmFORMNAME.IndexOf("재평가") != -1)
            {
                strFormNo += ", " + FormPatInfoFunc.Set_FormPatInfo_PTGetFormNo(clsDB.DbCon, fWrite.FmFORMNAME);
            }

            using (frmEmrChartHisList fEmrChartHisList = new frmEmrChartHisList(ActiveFormWrite, AcpEmr.ptNo, AcpEmr.acpNo, strFormNo, fWrite.FmFORMNAME, ""))
            {
                fEmrChartHisList.StartPosition = FormStartPosition.CenterScreen;
                fEmrChartHisList.ShowDialog(this);
            }
        }

        private void btnSideBarLeft_Click(object sender, EventArgs e)
        {
            if (panAcp.Visible == false) return;
            panAcp.Visible = false;
            panSideBarLeft.Visible = true;
            panSplitV.Visible = false;
        }

        private void lblSideBarLeft_Click(object sender, EventArgs e)
        {
            panAcp.Visible = true;
            panSideBarLeft.Visible = false;
            panSplitV.Visible = true;
        }

        private void panSplitV_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ResizeForm();
        }

        private void ResizeForm()
        {
            try
            {

                if (fEmrBaseAcpListWrite != null)
                {
                    fEmrBaseAcpListWrite.WindowState = FormWindowState.Normal;
                    fEmrBaseAcpListWrite.Height = panViewEmrAcp.Height;
                    fEmrBaseAcpListWrite.Width = panViewEmrAcp.Width;
                }
                Application.DoEvents();
                if (ActiveFormWrite != null)
                {
                    ActiveFormWrite.WindowState = FormWindowState.Normal;
                    ActiveFormWrite.Height = panEmrViewMain.Height;
                    ActiveFormWrite.Width = panEmrViewMain.Width;
                }
                Application.DoEvents();
            }
            catch
            {

            }
        }
        #endregion //컨트롤 이벤트

        private void frmEmrBaseChartWrite_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrBaseAcpListWrite != null)
            {
                fEmrBaseAcpListWrite.Dispose();
                fEmrBaseAcpListWrite = null;
            }

            if (ActiveFormWrite != null)
            {
                ActiveFormWrite.Dispose();
                ActiveFormWrite = null;
            }
        }
    }
}
