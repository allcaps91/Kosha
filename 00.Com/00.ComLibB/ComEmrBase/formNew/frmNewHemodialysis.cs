using ComBase;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// 신규 혈액투석 기록지
    /// </summary>
    public partial class frmNewHemodialysis : Form, EmrChartForm
    {
        #region // 폼에 사용하는 변수를 코딩하는 부분
        //private frmEmrInitList frmEmrInitListEvent;
        #endregion

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        FormEmrMessage mEmrCallForm = null;
        EmrPatient pAcp = null;

        public string mstrFormNo = "";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public string mstrEmrNo = "0";
        public string mstrMode = "W";

        /// <summary>
        /// 간호기록
        /// </summary>
        frmNursingRecordOld fEmrNursingRecordOld = null;

        /// <summary>
        /// 서식지 폼
        /// </summary>
        frmEmrChartNew fEmrChartNew = null;


        /// <summary>
        //  플로우차트 폼
        /// </summary>
        frmEmrChartFlowOld fEmrChartFlowOld = null;


        /// <summary>
        /// 임상관찰 기록지
        /// </summary>
        frmEmrVitalSign fEmrVitalSign = null;
        #endregion

        #region //외부에서 이벤트 받아서 처리 클리어, 저장, 삭제, 프린터
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
        #endregion

        #region //EmrChartForm
        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = 0;
            //isReciveOrderSave = true;
            //dblEmrNo = pSaveData(strFlag);
            //isReciveOrderSave = false;
            return dblEmrNo;
        }

        public bool DelDataMsg()
        {
            bool rtnVal = false;
            //rtnVal = pDelData();
            return rtnVal;
        }

        public void ClearFormMsg()
        {
            mstrEmrNo = "0";
            //pClearForm();
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

        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {

        }

        public int PrintFormMsg(string strPRINTFLAG)
        {
            return 0;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //rtnVal = clsFormPrint.PrintToTifFileLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        #endregion

        #region //FormEmrMessage
        public void MsgSave(string strSaveFlag)
        {

        }
        public void MsgDelete()
        {
           

        }
        public void MsgClear()
        {
            //ComFunc.MsgBoxEx(this, "MsgClear");
        }
        public void MsgPrint()
        {

        }
        #endregion


        //==========================================================================================//
        //=============================== 아래부터 코딩을 하면 됨 =====================================//
        //=========================================================================================//

        #region // 기록지 클리어, 저장, 삭제, 프린터
        /// <summary>
        /// 화면 정리
        /// </summary>
        public void pClearForm()
        {
            mstrEmrNo = "0";
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
        }

        #endregion

        #region 생성자
        public frmNewHemodialysis()
        {
            InitializeComponent();
        }

        public frmNewHemodialysis(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            InitializeComponent();
        }

        #endregion //생성자

        /// <summary>
        /// 서브폼 클리어
        /// </summary>
        public void SubFormClear()
        {
            if (fEmrChartNew != null)
            {
                fEmrChartNew.Dispose();
                fEmrChartNew = null;
            }

            if (fEmrNursingRecordOld != null)
            {
                fEmrNursingRecordOld.Dispose();
                fEmrNursingRecordOld = null;
            }

            if (fEmrVitalSign != null)
            {
                fEmrVitalSign.Dispose();
                fEmrVitalSign = null;
            }

            if (fEmrChartFlowOld != null)
            {
                fEmrChartFlowOld.Dispose();
                fEmrChartFlowOld = null;
            }
        }

        private void frmNewHemodialysis_Load(object sender, EventArgs e)
        {
            #region 간호기록
            EmrForm pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, "965", clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, 965).ToString());
            fEmrNursingRecordOld = new frmNursingRecordOld(pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), pAcp, "0", "W");
            #endregion

            #region 혈액투석
            fEmrChartNew = new frmEmrChartNew("1577", clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, 1577).ToString(), pAcp, "0", "W", mEmrCallForm);
            #endregion

            #region 인공신장실 V/S
            fEmrVitalSign = new frmEmrVitalSign("2201", clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, 2201).ToString(), pAcp, "0", "W", mEmrCallForm);
            #endregion

            #region 플로우(Cath)
            fEmrChartFlowOld = new frmEmrChartFlowOld("2139", clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, 2139).ToString(), pAcp, "0", "W", mEmrCallForm);
            #endregion

            TabFormAdd(fEmrChartNew, tabPage1);
            TabFormAdd(fEmrNursingRecordOld, tabPage2);
            TabFormAdd(fEmrVitalSign, tabPage3);
            TabFormAdd(fEmrChartFlowOld, tabPage4);

            fEmrChartNew.Width = Width;

            tabControl1.SelectedIndex = clsEmrPublic.HemodialysisTabIdx;
        }

        private void TabFormAdd(Form frm, TabPage tabPage)
        {
            frm.TopLevel = false;
            tabPage.Controls.Add(frm);
            //fEmrChartNew.Parent = panEmrWriteMain;
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.Show();
        }

        private void frmNewHemodialysis_Resize(object sender, EventArgs e)
        {
            if (fEmrChartNew != null)
            {
                fEmrChartNew.Width = Width;
                fEmrChartNew.Height = Height;
            }

            if (fEmrNursingRecordOld != null)
            {
                fEmrNursingRecordOld.Width = Width;
                fEmrNursingRecordOld.Height = Height;
            }

            if (fEmrVitalSign != null)
            {
                fEmrVitalSign.Width = Width;
                fEmrVitalSign.Height = Height;
            }

            if (fEmrChartFlowOld != null)
            {
                fEmrChartFlowOld.Width = Width;
                fEmrChartFlowOld.Height = Height;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsEmrPublic.HemodialysisTabIdx = tabControl1.SelectedIndex;
        }

        private void frmNewHemodialysis_FormClosed(object sender, FormClosedEventArgs e)
        {
            SubFormClear();
        }
    }
}
