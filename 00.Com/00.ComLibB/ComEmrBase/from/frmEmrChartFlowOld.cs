using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    public partial class frmEmrChartFlowOld : Form, EmrChartForm
    {

        #region // TopMenu관련 선언
        private usFormTopMenu usFormTopMenuEvent;
        private usTimeSet usTimeSetEvent;
        public ComboBox mMaskBox = null;
        #endregion

        #region //폼에서 사용하는 변수

        int TopPanelHeight = 34;

        /// <summary>
        /// 차트작성 0, 기록지등록에서 호출 1,
        /// </summary>
        int mCallFormGb = 0;

        /// <summary>
        /// 해드를 보이게 할지 여부
        /// </summary>
        string mFLOWGB = "COL"; 
        int mFLOWITEMCNT = 0;
        int mFLOWHEADCNT = 0;
        int mFLOWINPUTSIZE = 0;

        //int mRow = 0;
        //int mCol = 0;
        FormFlowSheet[] mFormFlowSheet = null;
        FormFlowSheetHead[,] mFormFlowSheetHead = null;


        /// <summary>
        /// 차트복사에서 열었는지
        /// </summary>
        bool bCopy = false;

        ///// <summary>
        ///// 인터페이스 폼
        ///// </summary>
        Form fEmrInterface = null;

        #endregion //폼에서 사용하는 변수

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //Form mCallForm = null;
        FormEmrMessage mEmrCallForm = null;
        EmrPatient pAcp = null;
        EmrForm pForm = null;

        Font bFont = null;

        bool mLodaing = false;

        /// <summary>
        /// 기록지 번호
        /// </summary>
        public string mstrFormNo = "2185";
        /// <summary>
        /// 기록지 업데이트 번호
        /// </summary>
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        /// <summary>
        /// EMRNO
        /// </summary>
        public string mstrEmrNo = "42694720";  //961 131641  //963 735603
        /// <summary>
        /// W: 작성, V: 조회
        /// </summary>
        public string mstrMode = "W";

        /// <summary>
        /// 차트복사에서 사용
        /// </summary>
        public string mstrDeptCode = string.Empty;

        /// <summary>
        /// 차트복사에서는 PrtSeq를 위해서, 그외에는 strInOutCls 구분 위해서.
        /// </summary>
        public string mstrVal = string.Empty;

        /// <summary>
        /// 조회 위한 함수
        /// </summary>
        string strInOutCls = string.Empty;
        /// <summary>
        /// 입퇴원일자 안에 작성된 갯수
        /// </summary>
        string strEmrCnt = string.Empty;

        /// <summary>
        /// 의료정보팀용 경과기록지
        /// </summary>
        frmEmrChartNew fEmrChartNew = null;

        /// <summary>
        /// 피하주사순서
        /// </summary>
        frmEmrInsulinImg frmEmrInsulinImgX = null;

        /// <summary>
        /// 의료정보팀 변경이력
        /// </summary>
        frmEmrNewHisView frmEmrNewHisViewX = null;

        /// <summary>
        /// 간호부 참고 이미지
        /// </summary>
        Form frmImgRmk = null;

        /// <summary>
        /// 날짜 음영 
        /// </summary>
        string strOldDate = string.Empty;

        /// <summary>
        /// 날짜 색상
        /// </summary>
        Color dblColorInfo;

        /// <summary>
        /// VB => txtEmrUseId => 작성자 아이디
        /// </summary>
        string mEmrUseId = string.Empty;

        /// <summary>
        /// 의료정보팀 권한 작성용 플래그
        /// </summary>
        string strSaveFlag = string.Empty;

        #endregion

        #region // TopMenu관련 이벤트 처리 함수

        private void SetTopMenuLoad()
        {
            //----TopMenu관련 이벤트 생성 및 선언
            usFormTopMenuEvent = new usFormTopMenu();
            //usBtnShow(usFormTopMenuEvent, "mbtnSave");
            usFormTopMenuEvent.rSetTimeCheckShow += new usFormTopMenu.SetTimeCheckShow(usFormTopMenuEvent_SetTimeCheckShow);
            usFormTopMenuEvent.rSetSave += new usFormTopMenu.SetSave(usFormTopMenuEvent_SetSave);
            usFormTopMenuEvent.rSetSaveTemp += new usFormTopMenu.SetSaveTemp(usFormTopMenuEvent_SetSaveTemp);
            usFormTopMenuEvent.rSetDel += new usFormTopMenu.SetDel(usFormTopMenuEvent_SetDel);
            usFormTopMenuEvent.rSetClear += new usFormTopMenu.SetClear(usFormTopMenuEvent_SetClear);
            usFormTopMenuEvent.rSetPrint += new usFormTopMenu.SetPrint(usFormTopMenuEvent_SetPrint);
            usFormTopMenuEvent.rSetPrintNull += new usFormTopMenu.SetPrintNull(usFormTopMenuEvent_SetPrintNull);
            usFormTopMenuEvent.rEventClosed += new usFormTopMenu.EventClosed(usFormTopMenuEvent_EventClosed);

            //usFormTopMenuEvent.dtMedFrDate.ValueChanged += new EventHandler(dtMedFrDate_ValueChanged);

            this.Controls.Add(usFormTopMenuEvent);
            usFormTopMenuEvent.Parent = this.panTopMenu;
            usFormTopMenuEvent.Dock = DockStyle.Fill;
            //--------------------------

            usFormTopMenuEvent.mbtnSaveTemp.Visible = false; //플로우시트는 임시저장 없음
        }

        /// <summary>
        /// 시간 패널 보이도록
        /// </summary>
        /// <param name="mkText"></param>
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

        /// <summary>
        /// 시간 세팅
        /// </summary>
        /// <param name="strText"></param>
        private void usTimeSetEvent_SetTime(string strText)
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            usFormTopMenuEvent.txtMedFrTime.Text = strText;
        }

        /// <summary>
        /// 시간 패널 닫기
        /// </summary>
        private void usTimeSetEvent_EventClosed()
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
        }

        /// <summary>
        /// 인증저장 버튼 클릭 이벤트
        /// </summary>
        /// <param name="strFrDate"></param>
        /// <param name="strFrTime"></param>
        private void usFormTopMenuEvent_SetSave(string strFrDate, string strFrTime)
        {
            pSaveData();

            #region 오늘 작성된 내역 전자인증 다시
            if (pAcp != null)
            {
                clsEmrFunc.NowEmrCert(clsDB.DbCon, pAcp.medFrDate, pAcp.ptNo);
            }
            #endregion

            if (pForm.FmFORMNO == 2641)
            {
                mEmrCallForm.MsgSave("ORD");
            }
        }

        /// <summary>
        /// 임시저장 버튼 클릭 이벤트
        /// </summary>
        /// <param name="strFrDate"></param>
        /// <param name="strFrTime"></param>
        private void usFormTopMenuEvent_SetSaveTemp(string strFrDate, string strFrTime)
        {
            pSaveData();
        }

        /// <summary>
        /// 삭제 버튼 클릭
        /// </summary>
        /// <param name="strFrDate"></param>
        /// <param name="strFrTime"></param>
        private void usFormTopMenuEvent_SetDel(string strFrDate, string strFrTime)
        {
            pDelData();
            if (pForm.FmFORMNO == 2641)
            {
                mEmrCallForm.MsgDelete();
            }
        }

        /// <summary>
        /// 차트 클리어
        /// </summary>
        private void usFormTopMenuEvent_SetClear()
        {
            DateTime dtpSys = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            usFormTopMenuEvent.txtMedFrTime.Enabled = true;
            usFormTopMenuEvent.dtMedFrDate.Value = dtpSys;
            usFormTopMenuEvent.txtMedFrTime.Text = dtpSys.ToString("HH:mm");
            mstrEmrNo = "0";
            mEmrUseId = string.Empty;
            ssWrite_Sheet1.Cells[0, 0, ssWrite_Sheet1.RowCount - 1, ssWrite_Sheet1.ColumnCount - 1].Text = "";
        }

        /// <summary>
        /// 출력
        /// </summary>
        private void usFormTopMenuEvent_SetPrint()
        {
            pPrintForm();
        }

        /// <summary>
        /// 출력버튼(빈서식지) 클릭 이벤트
        /// </summary>
        private void usFormTopMenuEvent_SetPrintNull()
        {
            //pClearForm();
        }

        /// <summary>
        /// 상단 버튼 컨트롤 Unload
        /// </summary>
        private void usFormTopMenuEvent_EventClosed()
        {
            //아무것도 하지 않는다.
        }

        /// <summary>
        /// 차트일자 변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtMedFrDate_ValueChanged(object sender, EventArgs e)
        {
            if (mstrMode.Equals("W") == false || mLodaing == false || pAcp.medDeptCd.Equals("ER"))
                return;


            string strdtpDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            if (pAcp.medEndDate != "")
            {
                if (VB.Val(strdtpDate) > (VB.Val(pAcp.medEndDate)))
                {
                    ComFunc.MsgBoxEx(this, "재원 기간을 넘어서는 작성 할수 없습니다.");
                    usFormTopMenuEvent.dtMedFrDate.Value = DateTime.ParseExact(pAcp.medEndDate, "yyyyMMdd", null);
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

        #endregion

        #region //EmrChartForm
        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = 0;
            //isRecivebtnSaveOrderave = true;
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
            return pPrintForm();
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //rtnVal = clsFormPrint.PrintToTifFileLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        #endregion

        #region //생성자
        public frmEmrChartFlowOld()
        {
            InitializeComponent();
        }

        public frmEmrChartFlowOld(string strFormNo, string strUpdateNo, string strEmrNo, int pCallFormGb)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mCallFormGb = pCallFormGb;

            pInitFormTemplet();
        }

        public frmEmrChartFlowOld(string pMake, int pItemCnt, int pHeadCnt, FormFlowSheet[] pFormFlowSheet, FormFlowSheetHead[,] pFormFlowSheetHead)
        {
            InitializeComponent();
            mFLOWGB = pMake;
            mFLOWITEMCNT = pItemCnt;
            mFLOWHEADCNT = pHeadCnt;
            mFormFlowSheet = pFormFlowSheet;
            mFormFlowSheetHead = pFormFlowSheetHead;

            pInitFormTemplet();
        }

        public frmEmrChartFlowOld(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mstrEmrNo = strEmrNo;
            InitializeComponent();

            pInitFormTemplet();
        }

        public frmEmrChartFlowOld(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            mstrEmrNo = strEmrNo;
            InitializeComponent();

            pInitFormTemplet();
        }

        /// <summary>
        /// 차트복사 => 작성일자 조회 위해서 
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="po"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strMode"></param>
        /// <param name="pEmrCallForm"></param>
        public frmEmrChartFlowOld(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, string strDeptCode, string strPrtSeq, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            mstrEmrNo = strEmrNo;
            mstrDeptCode = strDeptCode;
            mstrVal = strPrtSeq;
            InitializeComponent();

            pInitFormTemplet();
        }

        private void frmEmrChartFlowOld_Load(object sender, EventArgs e)
        {
            clsEmrChart.SetChartHead(clsDB.DbCon, usFormTopMenuEvent, pAcp);
            btnWebRmk.Visible = clsType.User.IsNurse.Equals("OK") && mstrMode.Equals("W");
            btnRmk2.Visible = mstrMode.Equals("W") && pForm.FmFORMNO == 1573; 

            //물리치료실 일때만 이전내역.
            if (clsType.User.BuseCode.Equals("055307"))
            {
                btnAutoText.Visible = true;
            }

            bFont = new Font("굴림체", 9f);

            bCopy = mEmrCallForm != null && ((Form)mEmrCallForm).Name.Equals("frmEmrJobChartCopy");

            btnUpDown.Visible = false;

            if (mCallFormGb != 1)
            {
                SetUserAut();
            }

            if (mstrMode.Equals("W"))
            {
                usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                usFormTopMenuEvent.txtMedFrTime.Text = usFormTopMenuEvent.dtMedFrDate.Value.ToString("HH:mm");
                btnSearchData.Visible = mstrFormNo.Equals("2201");
                btnInterface.Visible = pForm.FmFORMNO == 3537;
            }

            chkAsc.Checked = clsEmrPublic.bOrderAsc;

            //19 - 09 - 24 의료정보팀 요청으로 작성기간이 입퇴원 기간안이 아니고 그 전이나 그 이후 일경우 체크 위해서
            if (mstrVal.IndexOf("|") != -1)
            {
                strEmrCnt = mstrVal.Substring(mstrVal.IndexOf("|") + 1);
                mstrVal = mstrVal.Substring(0, mstrVal.IndexOf("|"));
            }

            if (pAcp != null && pAcp.inOutCls == "I")
            {
                if ((mEmrCallForm != null && (((Form)mEmrCallForm).Name.Equals("frmEmrLibViewerNr") == false)))
                {
                    dtpFrDate.Value  = Convert.ToDateTime(clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.IdNumber,
                        DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd")));
                    dtpEndDate.Value = Convert.ToDateTime(pAcp.medEndDate.Length == 0 ? ComQuery.CurrentDateTime(clsDB.DbCon, "S") : ComFunc.FormatStrToDate(pAcp.medEndDate, "D"));
                }
                else
                {
                    dtpFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-3);
                    dtpEndDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                }
            }
            else
            {
                if (pAcp != null)
                {
                    dtpFrDate.Value = Convert.ToDateTime(clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.IdNumber,
                        DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd")));

                    #region 투약기록지이면서 외래일 경우엔 투약기록지가 이틀 후까지 조회 가능하게(ER)
                    if ((pForm.FmFORMNO == 1796 || pForm.FmFORMNO == 2049 ||
                         pForm.FmFORMNO == 2137 || pForm.FmFORMNO == 1567 ||
                         pForm.FmFORMNO == 1965) && mstrVal.Equals("O") || pAcp.medDeptCd.Equals("ER"))
                    {
                        dtpEndDate.Value = dtpFrDate.Value.AddDays(1);
                        //dtpEndDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                    }
                    else
                    {
                        dtpEndDate.Value = dtpFrDate.Value;
                        dtpFrDate.Value = pForm.FmGRPFORMNO == 1031 && clsType.User.BuseCode.Equals("055307") ? dtpFrDate.Value.AddDays(-14) : dtpFrDate.Value;
                    }
                    #endregion
                }
            }

            #region 투약기록지 코드검색
            btnSearchCode.Visible = pForm.FmFORMNO == 1796;
            #endregion

            #region 변경내역 버튼
            btnSearchHis.Visible = clsType.User.AuAMANAGE.Equals("1");
            if (btnSearchHis.Visible)
            {
                btnSearchHis.BringToFront();
                btnWebRmk.Visible = false;
            }
            btnSearchHis.Left += pForm.FmFORMNO == 1796 ? btnSearchHis.Width : 0;
            #endregion

            if (clsType.User.BuseCode.Equals("044201") && pForm.FmFORMNO == 965 && pForm.FmOLDGB == 0)
            {
                lblIcuCount.Left = lblCount.Left + lblCount.Width;
                btnSearchAll.Left = lblIcuCount.Width + lblIcuCount.Left + 3;
                btnHis.Left = btnSearchAll.Width + btnSearchAll.Left;
                btnPrint.Left = btnHis.Width + btnHis.Left;
                btnSearchHis.Left = btnPrint.Width + btnPrint.Left;
                lblServerDate.Left = btnSearchHis.Width + btnSearchHis.Left;
            }


            for (int i = 0; i < ssView.ActiveSheet.Columns.Count; i++)
            {
                ssView.ActiveSheet.Columns.Get(i).AllowAutoFilter = true;
            }

            ssView.ActiveSheet.AutoFilterMode = AutoFilterMode.EnhancedContextMenu;
            ssView.ActiveSheet.AutoSortEnhancedContextMenu = true;

            #region //19-12-02 심사팀,의료정보팀 필터기능 추가
            if (clsType.User.BuseCode.Equals("078201"))
            {
                //ICU 기본간호활동2
                if (mstrFormNo.Equals("1976"))
                {
                    btnUpDown.Visible = true;
                    ssView_Sheet1.FrozenRowCount = 3;
                }

                if (pForm.FmFORMNO == 1725 || pForm.FmFORMNO == 1573)
                {
                    btnUpDown.Visible = true;
                }

                //인공호흡기 관련 폐렴예방 bundle          CRRT기록(지속적 신 대체 기록)      중심정맥관 관리기록
                // if (mstrFormNo.Equals("2639") || mstrFormNo.Equals("2239") || mstrFormNo.Equals("2227"))
                if (mFLOWGB.Equals("COL"))
                {
                    ssView_Sheet1.FrozenColumnCount = 3;
                }
                else if (mFLOWGB.Equals("ROW"))
                {
                    ssView_Sheet1.FrozenRowCount = 3;
                }
            }
            #endregion

            ssView.ScrollTipPolicy = ScrollTipPolicy.Both;
            mLodaing = true;

            #region 상처, 욕창 구분1~3 항목만 조회 할 수 있게.
            if (mstrMode.Equals("W") && pForm.FmOLDGB != 1 && (pForm.FmFORMNO == 1725 || pForm.FmFORMNO == 1573))
            {
                string strSearch = pForm.FmFORMNO == 1573 ? "위치" : "구분";
                panCbo.Visible = true;
                btnSaveOrder.Visible = true;
                btnAdd.Visible = true;
                btnRemove.Visible = true;


                foreach (FormFlowSheet flowSheet in mFormFlowSheet.Where(d => d.ItemCode.IndexOf("_") != -1).ToList())
                {
                    if (flowSheet.ItemCode.IndexOf("_1") == -1)
                    {
                        ssWrite_Sheet1.Rows[flowSheet.ItemNumber].Visible = false;
                        ssWrite_Sheet1.Cells[flowSheet.ItemNumber, 0].Text = "";
                    }
                }

                SetFlowViewLastData();
                SetFlowViewGbnGroup();

                if (pForm.FmFORMNO == 1573)
                {
                    GetDcur();
                }

                if (mFLOWGB == "ROW") //세로방식(아래로 작성)
                {
                    panWriteTop.Visible = true;
                    panWriteTop.Height = TopPanelHeight + 48;

                    panCbo.Left = 0;
                    panCbo.Visible = true;

                    if (panCbo.Visible)
                    {
                        string ItemCd = pForm.FmFORMNO == 1725 ? "상처간호" : "욕창간호";
                        panCbo.Top = 50;
                        btnAdd.Top = 52;
                        btnRemove.Top = 52;
                        btnSaveOrder.Top = 52;
                        btnRmk2.Top = 20;
                        btnWebRmk.Top = 20;

                        if (ssChk2.Visible == false)
                        {
                            btnSearchGBN.Left = ssChk1.Width;
                        }
                        else
                        {
                            btnSearchGBN.Left = ssChk2.Left + 30;
                        }

                        btnAdd.Left = panCbo.Width;
                        btnRemove.Left = panCbo.Left + btnAdd.Left + btnRemove.Width;
                        btnSaveOrder.Left = btnRemove.Left + btnRemove.Width + 5;
                        btnSaveOrder.BackColor = clsEmrQuery.ChartOrder_Exists(this, pAcp, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyy-MM-dd"), ItemCd, ItemCd) ? Color.Pink : Color.Gainsboro;

                        btnRmk2.Left = 0;
                        btnWebRmk.Left = btnRmk2.Left + 55;
                    }
                    else
                    {
                        btnAdd.Top = 50;
                        btnAdd.Left = btnAdd.Width + 1;

                        btnRemove.Top = 50;
                        btnRemove.Left = btnAdd.Left + btnRemove.Width + 1;
                    }
                }
            }
            #endregion

        

            #region TDM에서 들여ㅓ왔을경우
            if (Owner != null && Owner.Name.Equals("frmSupDrstTDMReturn"))
            {
                FarPoint.Win.Spread.FilterItemValue test = new FarPoint.Win.Spread.FilterItemValue("W-VAN1");
                FarPoint.Win.Spread.FilterItemValue test1 = new FarPoint.Win.Spread.FilterItemValue("W-VAN05");
                FarPoint.Win.Spread.MultiValuesFilterItem multifilter = new FarPoint.Win.Spread.MultiValuesFilterItem(new FarPoint.Win.Spread.FilterItemValue[] { test, test1 });
                FarPoint.Win.Spread.IRowFilter rowFilter = new FarPoint.Win.Spread.HideRowFilter(ssView.ActiveSheet);
                FarPoint.Win.Spread.FilterColumnDefinition fd = new FarPoint.Win.Spread.FilterColumnDefinition(3, FarPoint.Win.Spread.FilterListBehavior.Custom);


                fd.Filters.Add(multifilter);
                rowFilter.ColumnDefinitions.Add(fd);
                ssView.ActiveSheet.RowFilter = rowFilter;
                ssView.ActiveSheet.AutoFilterColumn(3, multifilter.DisplayName, 0);
                ssView.ActiveSheet.AutoFilterMode = FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu;
            }
            #endregion

            SetScoreVisible();
            SetDateVisible();
            SetDateVisible3();

            //자동조회
            GetSearchData();
        }

        #endregion //생성자

        #region //차트조회

        DataTable FlowData = null;
        //int DataTotalCount = 0;
        string ChartEmrNo = string.Empty;
        bool ErCoordinator = false;

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            GetSearchData();
        }

        void GetSearchData()
        {
            if (pAcp == null)
                return;

            if (FlowData != null)
            {
                FlowData.Dispose();
                FlowData = null;
            }

            if (pForm.FmFORMNO == 3552)
            {
                MO_ROW_HEADER_SET();
            }

            SetFlowViewGbnGroup();

            bool mViewNpChart = clsEmrQueryOld.ViewNPChart(clsType.User.Sabun);

            if (mstrMode.Equals("H"))
            {
                #region 수정 내역
                FlowData = clsEmrQuery.QueryHisSpdList(clsDB.DbCon, pAcp, pForm, mstrEmrNo, chkAsc.Checked);
                #endregion
            }
            else
            {

                #region 차트복사, 일반조회
                if (VB.IsNumeric(mstrVal) == false)
                {
                    //clsEmrQuery.QuerySpdList(clsDB.DbCon, pAcp, mstrFormNo, mstrUpdateNo, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                    //        ssView,
                    //        mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet, chkDesc.Checked);

                    FlowData = clsEmrQuery.QuerySpdList(clsDB.DbCon, pAcp, pForm,
                        dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                        mstrVal, mViewNpChart, chkAsc.Checked);
                }
                else
                {
                    //clsEmrQuery.QuerySpdList2(clsDB.DbCon, pAcp, mstrFormNo, mstrEmrNo, mstrUpdateNo, mstrPrtSeq,
                    //                        ssView,
                    //                        mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet, chkDesc.Checked);

                    FlowData = clsEmrQuery.QuerySpdList2(clsDB.DbCon, pAcp, pForm, mstrDeptCode, mstrEmrNo, mstrVal, mViewNpChart, chkAsc.Checked);
                }
                #endregion
            }

            if (FlowData == null)
            {
                ComFunc.MsgBoxEx(this, "조회중 오류가 발생했습니다.");
                return;
            }

            int intEmrCnt = (from DataRow dr in FlowData.Rows.AsParallel() group dr by dr["EMRNO"]).Count();
            int intDateCnt = (from DataRow dr in FlowData.Rows.AsParallel() group dr by dr["CHARTDATE"]).Count();
            lblCount.Text = string.Format("일수:{0}({1})", intDateCnt, intEmrCnt);

            //19-09-24 의료정보팀 요청(간호기록만 장수 다를경우 색깔 빨간색으로)
            if (clsType.User.BuseCode.Equals("044201") && mstrFormNo.Equals("965") && intEmrCnt != VB.Val(strEmrCnt))
            {
                lblCount.ForeColor = Color.Red;
            }

            #region 신규 간호 기록지 ICU 갯수 분리
            if (pForm.FmFORMNO == 965 && pForm.FmOLDGB == 0 && clsType.User.AuAMANAGE.Equals("1"))
            {
                intEmrCnt = FlowData.AsEnumerable().Where(d => d["WARDCODE"].ToString().Equals("33") || d["WARDCODE"].ToString().Equals("35")).Count();
                intDateCnt = FlowData.AsEnumerable().Where(d => d["WARDCODE"].ToString().Equals("33") || d["WARDCODE"].ToString().Equals("35")).GroupBy(d => d["CHARTDATE"]).Count();
                lblIcuCount.Text = string.Format("ICU 일수:{0}({1})", intDateCnt, intEmrCnt);
            }
            #endregion

            #region 상처,욕창 차팅 된 항목만 보이게
            if (pForm.FmOLDGB == 0 && (pForm.FmFORMNO == 1573 || pForm.FmFORMNO == 1725))
            {
                int ItemCnt = FlowData.AsEnumerable().Where(d => 
                d.Field<string>("ITEMCD").ToString().IndexOf("_5") != -1 && d.Field<string>("ITEMVALUE") != null && !string.IsNullOrWhiteSpace(d.Field<string>("ITEMVALUE"))).Count();
                if (ItemCnt == 0)
                {
                    //intMaxIdx = 4;
                    foreach (FormFlowSheet formFlow in mFormFlowSheet.Where(d => d.ItemCode.IndexOf("_5") != -1 ).ToList())
                    {
                        if (mFLOWGB.Equals("COL"))
                        {
                            ssView_Sheet1.Columns[formFlow.ItemNumber + 2].Visible = false;
                        }
                        else
                        {
                            ssView_Sheet1.Rows[formFlow.ItemNumber + 2].Visible = false;
                        }

                    }
                }

                ItemCnt = FlowData.AsEnumerable().Where(d => 
                d.Field<string>("ITEMCD").ToString().IndexOf("_4") != -1 && d.Field<string>("ITEMVALUE") != null && !string.IsNullOrWhiteSpace(d.Field<string>("ITEMVALUE"))).Count();
                if (ItemCnt == 0)
                {
                    //intMaxIdx = 3;
                    foreach (FormFlowSheet formFlow in mFormFlowSheet.Where(d => d.ItemCode.IndexOf("_4") != -1).ToList())
                    {
                        if (mFLOWGB.Equals("COL"))
                        {
                            ssView_Sheet1.Columns[formFlow.ItemNumber + 2].Visible = false;
                        }
                        else
                        {
                            ssView_Sheet1.Rows[formFlow.ItemNumber + 2].Visible = false;
                        }

                    }
                }
                ItemCnt = FlowData.AsEnumerable().Where(d => 
                d.Field<string>("ITEMCD").ToString().IndexOf("_3") != -1 && d.Field<string>("ITEMVALUE") != null && !string.IsNullOrWhiteSpace(d.Field<string>("ITEMVALUE"))).Count();
                if (ItemCnt == 0)
                {
                    //intMaxIdx = 2;
                    foreach (FormFlowSheet formFlow in mFormFlowSheet.Where(d => d.ItemCode.IndexOf("_3") != -1).ToList())
                    {
                        if (mFLOWGB.Equals("COL"))
                        {
                            ssView_Sheet1.Columns[formFlow.ItemNumber + 2].Visible = false;
                        }
                        else
                        {
                            ssView_Sheet1.Rows[formFlow.ItemNumber + 2].Visible = false;
                        }

                    }
                }

                ItemCnt = FlowData.AsEnumerable().Where(d => 
                d.Field<string>("ITEMCD").ToString().IndexOf("_2") != -1 && d.Field<string>("ITEMVALUE") != null && !string.IsNullOrWhiteSpace(d.Field<string>("ITEMVALUE"))).Count();
                if (ItemCnt == 0)
                {
                    //intMaxIdx = 1;
                    foreach (FormFlowSheet formFlow in mFormFlowSheet.Where(d => d.ItemCode.IndexOf("_2") != -1).ToList())
                    {
                        if (mFLOWGB.Equals("COL"))
                        {
                            ssView_Sheet1.Columns[formFlow.ItemNumber + 2].Visible = false;
                        }
                        else
                        {
                            ssView_Sheet1.Rows[formFlow.ItemNumber + 2].Visible = false;
                        }

                    }

                }
            }
            #endregion

            ChartEmrNo = string.Empty;
            if (mFLOWGB.Equals("ROW"))
            {
                ssView.ActiveSheet.ColumnCount = 0;
                if (bCopy)
                {
                    ssView.ActiveSheet.Rows[ssView.ActiveSheet.RowCount - 3].Visible = false;
                }
            }
            else
            {
                ssView.ActiveSheet.RowCount = 0;
                if (bCopy)
                {
                    ssView.ActiveSheet.Columns[ssView.ActiveSheet.ColumnCount - 3].Visible = false;
                }
            }

            //간호기록
            //임상관찰, 기본간호활동
            if  (pForm.FmOLDGB != 1 &&
                    ((pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 1575 || pForm.FmFORMNO == 2135 ||
                     pForm.FmFORMNO == 1935 || pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 ||
                     pForm.FmFORMNO == 2201) ||
                    (pForm.FmFORMNO == 965 || pForm.FmFORMNO == 2137 || pForm.FmFORMNO == 2049))
                )
            {
                SetFlowViewDataBindNew();
            }
            else
            {
                SetFlowViewDataBind();
            }
        }

        /// <summary>
        /// 스프레드 로우 Top변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SsView_TopChange(object sender, TopChangeEventArgs e)
        {
            if (e.NewTop >= (ssView.ActiveSheet.RowCount - 20))
            {
                if (pForm.FmOLDGB != 1 &&
                    (pForm.FmFORMNO == 965 || pForm.FmFORMNO == 2137 || pForm.FmFORMNO == 2049 ||
                    pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 1575 || pForm.FmFORMNO == 2135 || 
                    pForm.FmFORMNO == 1935 || pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 || 
                    pForm.FmFORMNO == 2201))
                {
                }
                else
                {
                    SetFlowViewDataBind();
                }
            }
            else if (e.NewTop <= ssView.ActiveSheet.RowCount - 20)
            {
                if (e.NewTop + 30 >= ssView.ActiveSheet.RowCount)
                {
                    if (pForm.FmOLDGB != 1 &&
                                     (pForm.FmFORMNO == 965 || pForm.FmFORMNO == 2137 || pForm.FmFORMNO == 2049 ||
                                      pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 1575 || pForm.FmFORMNO == 2135 ||
                                      pForm.FmFORMNO == 1935 || pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 ||
                                      pForm.FmFORMNO == 2201))
                    {
                    }
                    else
                    {
                        SetFlowViewDataBind();
                    }
                }
            }

            //if (e.NewTop > e.OldTop)
            //{
            //    ssView.ShowRow(0, e.OldTop + 1, VerticalPosition.Top);
            //}
        }

        /// <summary>
        /// 스프레드 칼럼 Left변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssView_LeftChange(object sender, LeftChangeEventArgs e)
        {
            if (e.NewLeft >= (ssView.ActiveSheet.ColumnCount - 20))
            {
                if (pForm.FmOLDGB != 1 &&
                    (pForm.FmFORMNO == 965 || pForm.FmFORMNO == 2137 || pForm.FmFORMNO == 2049 ||
                    pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 1575 || pForm.FmFORMNO == 2135 ||
                    pForm.FmFORMNO == 1935 || pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 ||
                    pForm.FmFORMNO == 2201))
                {
                }
                else
                {
                    SetFlowViewDataBind();
                }
            }
            else if (e.NewLeft <= ssView.ActiveSheet.ColumnCount - 20)
            {
                if (e.NewLeft + 30 >= ssView.ActiveSheet.ColumnCount)
                {
                    if (pForm.FmOLDGB != 1 &&
                                     (pForm.FmFORMNO == 965 || pForm.FmFORMNO == 2137 || pForm.FmFORMNO == 2049 ||
                                      pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 1575 || pForm.FmFORMNO == 2135 ||
                                      pForm.FmFORMNO == 1935 || pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 ||
                                      pForm.FmFORMNO == 2201))
                    {
                    }
                    else
                    {
                        SetFlowViewDataBind();
                    }
                }
            }
        }

        /// <summary>
        /// 낙상, 욕창 고위험군, 재평가 요인 표시
        /// </summary>
        private void SetScoreVisible()
        {
            if (mstrMode.Equals("W") == false)
                return;

            #region 욕창,낙상 고위험군 및 점수 표시
            if (pForm.FmOLDGB != 1 && (pForm.FmFORMNO == 2596 || pForm.FmFORMNO == 2500 || pForm.FmFORMNO == 2476))
            {
                List<FormFlowSheet> formFlowSheets = mFormFlowSheet.ToList();
                List<string> lstItemCode = new List<string>();
                List<string> lstItemVal = new List<string>();

                if (pForm.FmFORMNO == 2596)
                {
                    lstItemCode.Add("I0000033477"); //욕창고위험군
                    lstItemCode.Add("I0000037396"); //욕창 비정기적 재평가
                    lstItemVal.Add(FormPatInfoFunc.READ_DETAIL_BRADEN(clsDB.DbCon, pAcp));
                    lstItemVal.Add(FormPatInfoFunc.READ_DETAIL_EVAL_BRADEN_NEW(clsDB.DbCon, pAcp));
                }
                else if (pForm.FmFORMNO == 2500 || pForm.FmFORMNO == 2476) // 신생아, 낙상
                {
                    lstItemCode.Add("I0000033476"); //욕창고위험군
                    lstItemCode.Add("I0000037318"); //욕창 비정기적 재평가
                    lstItemVal.Add(FormPatInfoFunc.READ_DETAIL_FALL(clsDB.DbCon, pAcp));
                    lstItemVal.Add(FormPatInfoFunc.READ_DETAIL_EVAL_FALL_NEW(clsDB.DbCon, pAcp));
                }

                for (int i = 0; i < lstItemCode.Count; i++)
                {
                    FormFlowSheet formFlow = formFlowSheets.Where(c => c.ItemCode.Equals(lstItemCode[i])).FirstOrDefault();
                    if (formFlow == null)
                        continue;

                    int index = formFlow.ItemNumber;

                    if (mFLOWGB.Equals("ROW"))
                    {
                        ssWrite_Sheet1.Cells[index, 0].Text = lstItemVal[i];
                        if (lstItemVal[i].Length > 5)
                        {
                            ssWrite_Sheet1.Rows[index].Height = ssWrite_Sheet1.Rows[index].GetPreferredHeight() + 16;
                        }
                    }
                    else
                    {
                        ssWrite_Sheet1.Cells[0, index].Text = lstItemVal[i];
                        if (lstItemVal[i].Length > 5)
                        {
                            ssWrite_Sheet1.Columns[index].Width = ssWrite_Sheet1.Columns[index].GetPreferredWidth() + 16;
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 인공호흡기 관련폐렴 Bundle 연동 (2639)
        /// </summary>
        private void SetDateVisible3()
        {
            if (mstrMode.Equals("W") == false)
                return;


            #region 유치도뇨관 Bundle 유지일 연동
            if (pForm.FmOLDGB != 1 && pForm.FmFORMNO == 2639)
            {
                string strInsertDay = string.Empty;
                string strMaintaintDay = string.Empty;
                string strSize = string.Empty;

                FormPatInfoFunc.Set_FormPatInfo_PneumoniaUseDay(clsDB.DbCon, pAcp, ref strInsertDay, ref strMaintaintDay, ref strSize);

                if (string.IsNullOrWhiteSpace(strMaintaintDay))
                {
                    //lstItemVal.Add(ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>().ToString("yyyy-MM-dd"));
                    //lstItemVal.Add("1일");
                    //lstItemVal.Add(strSize);
                    return;
                }

                List<FormFlowSheet> formFlowSheets = mFormFlowSheet.ToList();
                List<string> lstItemCode = new List<string>();
                List<string> lstItemVal = new List<string>();

                DateTime dtpInsert = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                if (DateTime.TryParseExact(strInsertDay, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out dtpInsert) == false)
                {
                    dtpInsert = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                }

                lstItemCode.Add("I0000037660"); //폐렴Bundle - 삽입일
                lstItemCode.Add("I0000037743"); //폐렴Bundle - 유지일
                lstItemCode.Add("I0000037661"); //폐렴Bundle - 사이즈(Fr)
                lstItemVal.Add(dtpInsert.ToString("yyyy-MM-dd"));
                lstItemVal.Add(strMaintaintDay + "일");
                lstItemVal.Add(strSize);

                for (int i = 0; i < lstItemCode.Count; i++)
                {
                    FormFlowSheet formFlow = formFlowSheets.Where(c => c.ItemCode.Equals(lstItemCode[i])).FirstOrDefault();
                    int index = formFlow.ItemNumber;

                    if (mFLOWGB.Equals("ROW"))
                    {
                        ssWrite_Sheet1.Cells[index, 0].Text = lstItemVal[i];
                        if (lstItemVal[i].Length > 5)
                        {
                            ssWrite_Sheet1.Rows[index].Height = ssWrite_Sheet1.Rows[index].GetPreferredHeight() + 16;
                        }
                    }
                    else
                    {
                        ssWrite_Sheet1.Cells[0, index].Text = lstItemVal[i];
                        if (lstItemVal[i].Length > 5)
                        {
                            ssWrite_Sheet1.Columns[index].Width = ssWrite_Sheet1.Columns[index].GetPreferredWidth() + 16;
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 유치도뇨관 Bundle 연동 (2641)
        /// </summary>
        private void SetDateVisible()
        {
            if (mstrMode.Equals("W") == false)
                return;


            #region 유치도뇨관 Bundle 유지일 연동
            if (pForm.FmOLDGB != 1 && pForm.FmFORMNO == 2641)
            {
                string strInsertDay = string.Empty;
                string strMaintaintDay = string.Empty;
                string strSize = string.Empty;

                FormPatInfoFunc.Set_FormPatInfo_VentilatorUseDay(clsDB.DbCon, pAcp, ref strInsertDay, ref strMaintaintDay, ref strSize);

                if (string.IsNullOrWhiteSpace(strMaintaintDay))
                {
                    //ComFunc.MsgBoxEx(this, "이미 제거된 도뇨관 입니다\r\n다시 확인해주세요.");
                    return;
                }

                List<FormFlowSheet> formFlowSheets = mFormFlowSheet.ToList();
                List<string> lstItemCode = new List<string>();
                List<string> lstItemVal = new List<string>();

                DateTime dtpInsert = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                if (DateTime.TryParseExact(strInsertDay, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out dtpInsert) == false)
                {
                    dtpInsert = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                }

                lstItemCode.Add("I0000037645"); //유치도뇨관 - 삽입일
                lstItemCode.Add("I0000037742"); //유치도뇨관 - 유지일
                lstItemCode.Add("I0000038003"); //유치도뇨관 - 사이즈(Fr)
                lstItemVal.Add(dtpInsert.ToString("yyyy-MM-dd"));
                lstItemVal.Add(strMaintaintDay + "일");
                lstItemVal.Add(strSize);

                for (int i = 0; i < lstItemCode.Count; i++)
                {
                    FormFlowSheet formFlow = formFlowSheets.Where(c => c.ItemCode.Equals(lstItemCode[i])).FirstOrDefault();
                    int index = formFlow.ItemNumber;

                    if (mFLOWGB.Equals("ROW"))
                    {
                        ssWrite_Sheet1.Cells[index, 0].Text = lstItemVal[i];
                        if (lstItemVal[i].Length > 5)
                        {
                            ssWrite_Sheet1.Rows[index].Height = ssWrite_Sheet1.Rows[index].GetPreferredHeight() + 16;
                        }
                    }
                    else
                    {
                        ssWrite_Sheet1.Cells[0, index].Text = lstItemVal[i];
                        if (lstItemVal[i].Length > 5)
                        {
                            ssWrite_Sheet1.Columns[index].Width = ssWrite_Sheet1.Columns[index].GetPreferredWidth() + 16;
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 상처, 욕창 내원 기간안에 (상처, 욕창)구분 값 가져오기
        /// </summary>
        private void SetFlowViewGbnGroup()
        {
            if (mstrMode.Equals("W") == false)
                return;

            if (pForm.FmOLDGB == 1)
                return;

            if (pForm.FmFORMNO != 1573 && pForm.FmFORMNO != 1725)
                return;

            DataTable dt = clsEmrQuery.QueryCboGbnList(clsDB.DbCon, pAcp, pForm);

            if (dt == null)
            {
                return;
            }

            btnAdd.Visible = true;
            btnRemove.Visible = true;

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            ssChk1_Sheet1.RowCount = 0;
            ssChk1_Sheet1.RowCount = dt.Rows.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssChk1_Sheet1.Cells[i, 0].Text = (dt.Rows[i][0].ToString());
            }
            dt.Dispose();


            dt = clsEmrQuery.QueryCboGbnList(clsDB.DbCon, pAcp, pForm, true);
            if (dt == null)
            {
                return;
            }

            ssChk2.Visible = pForm.FmFORMNO == 1725;
            if(ssChk2.Visible == false)
            {
                btnSearchGBN.Left = ssChk1.Width + 10;
                panCbo.Width = ssChk1.Width + btnSearchGBN.Width + 10;
            }
            ssChk2_Sheet1.RowCount = 0;
            ssChk2_Sheet1.RowCount = dt.Rows.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssChk2_Sheet1.Cells[i, 0].Text = (dt.Rows[i][0].ToString());
            }
            dt.Dispose();
        }

        /// <summary>
        /// 상처, 욕창 최근에 작성한 끌려오게 (데이터만)
        /// </summary>
        private void SetFlowViewLastData()
        {
            if (mstrMode.Equals("W") == false)
                return;

            if (pForm.FmOLDGB == 1)
                return;

            if (pForm.FmFORMNO != 1573 && pForm.FmFORMNO != 1725)
                return;

            DataTable dt = clsEmrQuery.QuerySpdLastList(clsDB.DbCon, pAcp, pForm);
            if (dt == null)
            {                
                return;
            }

            btnAdd.Visible = true;
            btnRemove.Visible = true;

            SheetView sheet = ssView.ActiveSheet;
            List<FormFlowSheet> formFlowSheets = mFormFlowSheet.ToList();

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                for (int i = 0; i < ssWrite_Sheet1.RowCount; i++)
                {
                    if (ssWrite_Sheet1.Rows[i].Visible == false)
                    {
                        ssWrite_Sheet1.Cells[i, 0].Text = "";
                    }
                }

                FormFlowSheet formFlow = formFlowSheets.Where(c => c.ItemCode.Equals("I0000037213")).FirstOrDefault();

                if (pForm.FmFORMNO == 1573 && formFlow.ItemCode.IndexOf("I0000037213") != -1)
                {
                    ComboSelChange(formFlow.ItemNumber);
                }
                return;
            }


            foreach (DataRow row in dt.Rows)
            {
                string strItem = row["ITEMVALUE"].ToString().Trim();
                FormFlowSheet formFlow = formFlowSheets.Where(c => c.ItemCode.Equals(row["ITEMCD"].ToString().Trim())).FirstOrDefault();

                if (formFlow == null)
                    continue;

                int index = formFlow.ItemNumber;

                if (formFlow.CellType.Equals("CheckBoxCellType"))
                {
                    strItem = strItem.Equals("1").ToString();
                }

                if (mFLOWGB.Equals("ROW"))
                {
                    ssWrite_Sheet1.Cells[index, 0].Text = strItem;
                    if (ssWrite_Sheet1.Rows[index].Visible == false && !string.IsNullOrWhiteSpace(strItem))
                    {
                        ssWrite_Sheet1.Rows[index].Visible = true;
                    }
                }
                else
                {
                    ssWrite_Sheet1.Cells[0, index].Text = strItem;
                }

                if (pForm.FmFORMNO == 1573 && formFlow.ItemCode.IndexOf("I0000037213") != -1)
                {
                    ComboSelChange(index);
                }
            }

            dt.Dispose();

            for (int i = 0; i < ssWrite_Sheet1.RowCount; i++)
            {
                if (ssWrite_Sheet1.Rows[i].Visible == false)
                {
                    ssWrite_Sheet1.Cells[i, 0].Text = "";
                }
            }
        }

        private void SetFlowViewDataBindNew()
        {
            if (FlowData == null)
            {
                return;
            }

            if (pForm.FmOLDGB == 1)
                return;

            SheetView sheet = ssView.ActiveSheet;

            try
            {
                foreach (DataRow row in FlowData.Rows)
                {
                    int colCount = 0;
                    int rowCount = 0;
                    string chartDate = VB.Val(row[mstrMode.Equals("H") ? "WRITEDATE" : "CHARTDATE"].ToString().Trim()).ToString("0000-00-00");

                    sheet.RowCount += 1;
                    if ((int)sheet.Rows[sheet.RowCount - 1].Height < 22)
                    {
                        sheet.SetRowHeight(sheet.RowCount - 1, 22);
                    }
                    else
                    {
                        sheet.SetRowHeight(sheet.RowCount - 1, (int)sheet.Rows[sheet.RowCount - 1].Height);
                    }

                    colCount = sheet.ColumnCount;
                    rowCount = sheet.RowCount;

                    clsSpread.SetTypeAndValue(sheet, rowCount - 1, 0, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, chartDate, false);

                    #region 의료정보팀 서버시간 
                    sheet.Cells[rowCount - 1, 0].Tag = row["WRITEDATE"].ToString();
                    sheet.Cells[rowCount - 1, 1].Tag = row["WRITETIME"].ToString();
                    #endregion

                    //clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                    clsSpread.SetTypeAndValue(sheet, rowCount - 1, 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((row[mstrMode.Equals("H") ? "WRITETIME" : "CHARTTIME"].ToString() + "").Trim(), "M"), false);

                    #region 임상관찰
                    if (pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 2135 || pForm.FmFORMNO == 1935 ||
                        pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 || pForm.FmFORMNO == 2201)
                    {
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 2, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["BGRPNAME"].ToString().Trim(), true);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 3, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_L, row["GRPNAME"].ToString().Trim(), true);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 4, "TextCellType", !clsType.User.BuseCode.Equals("078201"), clsSpread.VAlign_C, clsSpread.HAlign_L, row["ITEMNAME"].ToString().Trim(), true);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 5, "TextCellType", !clsType.User.BuseCode.Equals("078201"), clsSpread.VAlign_C, clsSpread.HAlign_C, row["ITEMVALUE"].ToString().Trim(), true);

                    }
                    #endregion

                    #region 기본 간호활동
                    else if (pForm.FmFORMNO == 1575 && pForm.FmOLDGB != 1)
                    {
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 2, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["BGRPNAME"].ToString().Trim(), true);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 3, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_L, row["GRPNAME"].ToString().Trim(), true);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 4, "TextCellType", !clsType.User.BuseCode.Equals("078201"), clsSpread.VAlign_C, clsSpread.HAlign_L, row["ITEMVALUE"].ToString().Trim(), true);
                    }
                    #endregion

                    #region 간호기록
                    else if (pForm.FmFORMNO == 965)//병동
                    {
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 2, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["WARDCODE"].ToString().Trim(), true);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 3, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["ROOMCODE"].ToString().Trim(), true);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 4, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["PROBLEM"].ToString().Trim(), true);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 5, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["TYPE"].ToString().Trim(), true);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 6, "TextCellType", !clsType.User.BuseCode.Equals("078201"), clsSpread.VAlign_C, clsSpread.HAlign_L, row["ITEMVALUE"].ToString().Trim(), true);
                    }
                    else if (pForm.FmFORMNO == 2137)//회복실
                    {
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 2, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["WARDCODE"].ToString().Trim(), true);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 3, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["ROOMCODE"].ToString().Trim(), true);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 4, "TextCellType", !clsType.User.BuseCode.Equals("078201"), clsSpread.VAlign_C, clsSpread.HAlign_L, row["ITEMVALUE"].ToString().Trim(), true);
                    }
                    else if (pForm.FmFORMNO == 2049)//ER
                    {
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 2, "TextCellType", !clsType.User.BuseCode.Equals("078201"), clsSpread.VAlign_C, clsSpread.HAlign_L, row["ITEMVALUE"].ToString().Trim(), true);
                    }
                    #endregion

                    clsSpread.SetTypeAndValue(sheet, rowCount - 1, colCount - 4, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["USENAME"].ToString().Trim(), false);
                    clsSpread.SetTypeAndValue(sheet, rowCount - 1, colCount - 3, "TextCellType", true, clsSpread.VAlign_T, clsSpread.HAlign_C, row["PRNTYN"].ToString().Trim(), false);
                    clsSpread.SetTypeAndValue(sheet, rowCount - 1, colCount - 2, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["EMRNO"].ToString().Trim(), false);
                    clsSpread.SetTypeAndValue(sheet, rowCount - 1, colCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["CHARTUSEID"].ToString().Trim(), false);

                    //2020-08-11 늘림...
                    //sheet.Rows[sheet.RowCount - 1].Height = (int)sheet.Rows[sheet.RowCount - 1].GetPreferredHeight() + 15;
                    sheet.Rows[sheet.RowCount - 1].Height = (int)sheet.Rows[sheet.RowCount - 1].GetPreferredHeight() + 30;

                    if (clsType.User.BuseCode.Equals("044201") && clsType.User.AuAMANAGE.Equals("1") && bCopy == false ||
                      clsType.User.BuseCode.Equals("078201") || ErCoordinator == true)
                    {
                        if (!strOldDate.Equals(row["CHARTDATE"].ToString()))
                        {
                            dblColorInfo = dblColorInfo == Color.White ? ComNum.SPSELCOLOR : Color.White;
                            strOldDate = row["CHARTDATE"].ToString();
                        }

                        if (mFLOWGB.Equals("ROW"))
                        {
                            sheet.Columns[sheet.ColumnCount - 1].BackColor = dblColorInfo;
                        }
                        else
                        {
                            sheet.Rows[sheet.RowCount - 1].BackColor = dblColorInfo;
                        }
                    }
                }
            }
            catch 
            {

            }
        }

        private void SetFlowViewDataBind(string PrintFlag = "")
        {
            if (FlowData == null)
            {
                return;
            }

            SheetView sheet = ssView.ActiveSheet;
            int count = 0;

            try
            {
                int height = 0;
                int width = 0;

                foreach (DataRow row in FlowData.Rows)
                {  
                    if (!ChartEmrNo.Equals(row["EMRNO"].ToString()))
                    {
                        if (mFLOWGB.Equals("ROW"))
                        {
                            sheet.ColumnCount += 1;
                            if ((int)sheet.Columns[sheet.ColumnCount - 1].Width < mFLOWINPUTSIZE)
                            {
                                sheet.SetColumnWidth(sheet.ColumnCount - 1, mFLOWINPUTSIZE);
                            }
                            else
                            {
                                sheet.SetColumnWidth(sheet.ColumnCount - 1, (int)sheet.Columns[sheet.ColumnCount - 1].Width);
                            }

                            width += (int)sheet.Columns[sheet.ColumnCount - 1].Width;
                        }
                        else
                        {
                            sheet.RowCount += 1;
                            if ((int)sheet.Rows[sheet.RowCount - 1].Height < 22)
                            {
                                sheet.SetRowHeight(sheet.RowCount - 1, 22);
                            }
                            else
                            {
                                sheet.SetRowHeight(sheet.RowCount - 1, (int)sheet.Rows[sheet.RowCount - 1].Height);
                            }

                            height += (int)sheet.Rows[sheet.RowCount - 1].Height;
                        }
                    }

                    if (clsType.User.BuseCode.Equals("044201") && clsType.User.AuAMANAGE.Equals("1") && bCopy == false ||
                        clsType.User.BuseCode.Equals("078201") || ErCoordinator == true)
                    {
                        if (!strOldDate.Equals(row["CHARTDATE"].ToString()))
                        {
                            dblColorInfo = dblColorInfo == Color.White ? ComNum.SPSELCOLOR : Color.White;
                            strOldDate = row["CHARTDATE"].ToString();
                        }

                        if (mFLOWGB.Equals("ROW"))
                        {
                            sheet.Columns[sheet.ColumnCount - 1].BackColor = dblColorInfo;
                        }
                        else
                        {
                            sheet.Rows[sheet.RowCount - 1].BackColor = dblColorInfo;
                        }
                    }

                    int colCount = sheet.ColumnCount;
                    int rowCount = sheet.RowCount;

                    ChartEmrNo = row["EMRNO"].ToString();
                    string chartDate = VB.Val(row[mstrMode.Equals("H") ? "WRITEDATE" : "CHARTDATE"].ToString().Trim()).ToString("0000-00-00");

                    if (mFLOWGB.Equals("ROW"))
                    {
                        //Console.WriteLine(row["ITEMCD"].ToString());

                        #region 의료정보팀 서버시간 
                        sheet.Columns[colCount - 1].Tag = string.Format("{0} \r\n{1}", row["WRITEDATE"].ToString(), VB.Val(row["WRITETIME"].ToString()).ToString("00:00:00"));
                        #endregion

                        clsSpread.SetTypeAndValue(sheet, 0, colCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, chartDate, false);
                        clsSpread.SetTypeAndValue(sheet, 1, colCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((row[mstrMode.Equals("H") ? "WRITETIME" : "CHARTTIME"].ToString()).Trim(), "M"), false);
                        //clsSpread.SetTypeAndValue(sheet, 2, colCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((row["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                        for (int j = 0; j < mFormFlowSheet.Length; j++) //이부분 수정 
                        {
  

                            if (row["ITEMCD"].ToString().Trim().Equals(mFormFlowSheet[j].ItemCode.Trim())) //이부분 수정 
                            {
                                //이부분 수정 4 + j, ssList.ActiveSheet.ColumnCount - 1
                                clsSpread.SetTypeAndValue(sheet, 2 + j, colCount - 1, clsXML.GetType("TEXT"), !clsType.User.BuseCode.Equals("078201"),
                                                sheet.Rows[2 + j].VerticalAlignment,
                                                sheet.Rows[2 + j].HorizontalAlignment,
                                                row["ITEMVALUE"].ToString().Trim(),
                                                true);

                                Size ColSize = new Size((int) sheet.Columns[colCount - 1].Width, (int) sheet.Rows[2 + j] .Height);
                                Size TextSize = TextRenderer.MeasureText(row["ITEMVALUE"].ToString().Trim(), ssView.Font, ColSize, TextFormatFlags.WordEllipsis | TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl);
                                if (TextSize.Height > ColSize.Height)
                                {
                                    sheet.Rows[2 + j].Height += (TextSize.Height - ColSize.Height) + 16;
                                
                                }
                            }
                        }

                        clsSpread.SetTypeAndValue(sheet, rowCount - 4, colCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["USENAME"].ToString().Trim(), true);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 3, colCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["PRNTYN"].ToString().Trim(), false);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 2, colCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["EMRNO"].ToString().Trim(), false);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, colCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["CHARTUSEID"].ToString().Trim(), false);
                    }
                    else
                    {
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 0, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, chartDate, false);

                        #region 의료정보팀 서버시간 
                        sheet.Cells[rowCount - 1, 0].Tag = row["WRITEDATE"].ToString();
                        sheet.Cells[rowCount - 1, 1].Tag = row["WRITETIME"].ToString();
                        #endregion

                        //clsSpread.SetTypeAndValue(ssList.ActiveSheet, ssList.ActiveSheet.RowCount - 1, 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, clsEmrFunc.DutyGet(ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M")), false);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, ComFunc.FormatStrToDate((row["CHARTTIME"].ToString() + "").Trim(), "M"), false);
                        for (int j = 0; j < mFormFlowSheet.Length; j++) //이부분 수정 
                        {
                            #region 투약기록지 색상표시
                            if (pForm.FmFORMNO == 1796 && bCopy == false)

                            {
                                switch (row["BUN"].ToString().Trim())
                                {
                                    case "11":
                                    case "12":
                                        sheet.Rows[rowCount - 1].ForeColor = Color.FromArgb(240, 50, 50);
                                        break;
                                    case "20":
                                        sheet.Rows[rowCount - 1].ForeColor = Color.FromArgb(50, 50, 240);
                                        break;
                                    default:
                                        sheet.Rows[rowCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                                        break;
                                }
                            }
                            #endregion

                            if (row["ITEMCD"].ToString().Trim().Equals(mFormFlowSheet[j].ItemCode.Trim())) //이부분 수정 
                            {
                                //경과 이미지
                                if (row["FORMNO"].ToString().Trim().Equals("1232"))
                                {
                                    sheet.Cells[rowCount - 1, 2 + j].CellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                                    sheet.Cells[rowCount - 1, 2 + j].VerticalAlignment = sheet.Columns[2 + j].VerticalAlignment;
                                    sheet.Cells[rowCount - 1, 2 + j].HorizontalAlignment = CellHorizontalAlignment.Center;
                                    sheet.Cells[rowCount - 1, 2 + j].Value = GetEmrImg(row["IMAGEVALUE"]);
                                    sheet.Rows[sheet.RowCount - 1].Height = 352;
                                }
                                //경과기록지
                                else if (row["FORMNO"].ToString().Equals("963"))
                                {
                                    if (pForm.FmUPDATENO == 1)
                                    {
                                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 2 + j, clsXML.GetTypeEx(row["ITEMTYPE"].ToString().Trim()), true,
                                            sheet.Columns[2 + j].VerticalAlignment,
                                            sheet.Columns[2 + j].HorizontalAlignment,
                                            GetContent(row["ITEMVALUE"].ToString().Trim()),
                                            true);
                                    }
                                    else
                                    {

                                        sheet.Cells[rowCount - 1, 2].Tag = row["GBN"].ToString();
                                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, 2 + j, clsXML.GetTypeEx(row["ITEMTYPE"].ToString().Trim()), true,
                                                               sheet.Columns[2 + j].VerticalAlignment,
                                                               sheet.Columns[2 + j].HorizontalAlignment,
                                                                row["GBN"].ToString().Trim().Equals("NEW") ? row["ITEMVALUE"].ToString().Trim() : GetContent(row["ITEMVALUE"].ToString().Trim()),
                                                               true);
                                    }
                                }
                                //그외 기록지들
                                else
                                {
                                    clsSpread.SetTypeAndValue(sheet, rowCount - 1, 2 + j, clsXML.GetTypeEx(row["ITEMTYPE"].ToString().Trim()), true,
                                                                sheet.Columns[2 + j].VerticalAlignment,
                                                                sheet.Columns[2 + j].HorizontalAlignment,
                                                                row["ITEMVALUE"].ToString().Trim(),
                                                                true);

                                    #region 활력측정 수치 벗어나면 빨간색 표시
                                    if (row["FORMNO"].ToString().Trim().Equals("1562"))
                                    {
                                        double rtnVal = VB.Val(row["ITEMVALUE"].ToString().Trim());
                                        switch (j)
                                        {
                                            case 2: //혈압
                                                if (rtnVal >= 140 || rtnVal < 80)
                                                {
                                                    sheet.Cells[rowCount - 1, 2 + j].ForeColor = Color.Red;
                                                }
                                                break;
                                            case 3: //혈압
                                                if (rtnVal >= 90 || rtnVal < 60)
                                                {
                                                    sheet.Cells[rowCount - 1, 2 + j].ForeColor = Color.Red;
                                                }
                                                break;
                                            case 5: //맥박
                                                if (rtnVal >= 100 || rtnVal < 60)
                                                {
                                                    sheet.Cells[rowCount - 1, 2 + j].ForeColor = Color.Red;
                                                }
                                                break;
                                            case 6: //호흡
                                                if (rtnVal >= 21 || rtnVal < 12)
                                                {
                                                    sheet.Cells[rowCount - 1, 2 + j].ForeColor = Color.Red;
                                                }
                                                break;
                                            case 7: //체온
                                                if (rtnVal >= 37.5 || rtnVal < 36.5)
                                                {
                                                    sheet.Cells[rowCount - 1, 2 + j].ForeColor = Color.Red;
                                                }
                                                break;
                                        }
                                    }
                                    #endregion
                                }

                                sheet.Rows[sheet.RowCount - 1].Height =
                                (int)sheet.Rows[sheet.RowCount - 1].GetPreferredHeight() + 16;
                            }
                        }

                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, colCount - 4, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["USENAME"].ToString().Trim(), true);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, colCount - 3, "TextCellType", true, clsSpread.VAlign_T, clsSpread.HAlign_C, row["PRNTYN"].ToString().Trim(), false);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, colCount - 2, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["EMRNO"].ToString().Trim(), false);
                        clsSpread.SetTypeAndValue(sheet, rowCount - 1, colCount - 1, "TextCellType", true, clsSpread.VAlign_C, clsSpread.HAlign_C, row["CHARTUSEID"].ToString().Trim(), false);

                        Console.WriteLine(sheet.Cells[rowCount - 1, colCount - 3].Text.Trim());
                    }

                    count++;
                    if (Owner == null || Owner != null && Owner.Name.Equals("frmSupDrstTDMReturn") == false)
                    {
                        if (bCopy == false && string.IsNullOrEmpty(PrintFlag) && count >= 50 && ((mFLOWGB.Equals("COL") && height >= 858) ||
                        bCopy == false && (mFLOWGB.Equals("ROW") && width >= 2360)))
                        {
                            break;
                        }
                    }
                }


                //심사팀 복사 가능하게
                if (clsType.User.BuseCode.Equals("078201") && sheet.ColumnCount > 0 && sheet.RowCount > 0)
                {
                    sheet.Cells[0, 0, sheet.RowCount - 1, sheet.ColumnCount - 1].Locked = false;
                }

                for (int i = (count - 1); i >=0; i--)
                {
                    //Console.WriteLine(i);
                    FlowData.Rows.RemoveAt(i);
                }
               
            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        private string GetContent(string strXml)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(strXml);
            return xml.InnerText;
        }

        /// <summary>
        /// EMR 이미지 가져오기
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private Image GetEmrImg(object bytes)
        {
            Image rtnVal = null;


            if (bytes == DBNull.Value)
            {
                return rtnVal;
            }

            if (pForm.FmOLDGB != 1)
            {
                bytes = Convert.FromBase64String(bytes.ToString());
            }

            byte[] byteArray = (byte[]) bytes;
            using (MemoryStream memStream = new MemoryStream(byteArray))
            {
                rtnVal = Image.FromStream(memStream);
            }
            return rtnVal;
        }

        private void btnHis_Click(object sender, EventArgs e)
        {
            if (mstrMode.Equals("W") == false || ssView_Sheet1.RowCount == 0)
                return;

            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            usFormTopMenuEvent.txtMedFrTime.Text = usFormTopMenuEvent.dtMedFrDate.Value.ToString("HH:mm");
            usFormTopMenuEvent.dtMedFrDate.Enabled = true;

            mEmrUseId = string.Empty;
            mstrEmrNo = string.Empty;

            ssWrite_Sheet1.Cells[0, 0, ssWrite_Sheet1.RowCount - 1, ssWrite_Sheet1.ColumnCount - 1].Text = "";

            if (mFLOWGB.Equals("COL"))
            {
                for (int i = 0; i < mFormFlowSheet.Length; i++)
                {
                    ssWrite_Sheet1.Cells[0, i].Text = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, i + 2].Text.Trim();
                }
            }
            else
            {
                for (int i = 0; i < mFormFlowSheet.Length; i++)
                {
                    ssWrite_Sheet1.Cells[i, 0].Text = ssView_Sheet1.Cells[i + 2, ssView_Sheet1.ActiveColumnIndex].Text.Trim();
                    ssWrite_Sheet1.Rows[i].Visible = true;
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (pForm.FmFORMNO == 963)
            {
                using(frmEmrProgressView frm = new frmEmrProgressView(this, pAcp, pForm, dtpFrDate, dtpEndDate))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    //frm.ShowDialog(this);
                    frm.Show();
                    frm.Hide();
                    frm.pPrintForm();
                }
                return;
            }
            pPrintForm();
            //if(string.IsNullOrEmpty(mstrPrtSeq))
            //{
            //}
            //else
            //{
            //    PrintFormMsg("0");
            //}
        }
        #endregion //차트조회

        #region //Private Function

        private void GetDcur()
        {
            OracleDataReader dataReader = null;

            string SQL = " SELECT Total ";
            SQL = SQL + ComNum.VBLF + " FROM ADMIN.NUR_BRADEN_SCALE B ";
            SQL = SQL + ComNum.VBLF + "   INNER JOIN ADMIN.IPD_NEW_MASTER M";
            SQL = SQL + ComNum.VBLF + "      ON B.IPDNO = M.IPDNO";
            SQL = SQL + ComNum.VBLF + " AND M.PANO = '" + pAcp.ptNo + "'";
            SQL = SQL + ComNum.VBLF + " AND B.ACTDATE = TO_DATE('" + ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10) + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + " AND M.Amset4 <> '3'";
            SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000'";
            SQL = SQL + ComNum.VBLF + " AND M.Pano <> '81000004'";

            string sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, sqlErr);
                return;
            }

            if (dataReader.HasRows && dataReader.Read())
            {
                ssWrite_Sheet1.Cells[0, 0].Text = dataReader.GetValue(0).ToString().Trim() == "0" ? "" : dataReader.GetValue(0).ToString().Trim();
            }
            dataReader.Dispose();
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
        /// 사용자 템플릿 뷰어용
        /// </summary>
        public void pInitFormTemplet()
        {
            SetFormInfo();

            SetTopMenuLoad();

            if (mstrFormNo != "0")
            {
                FormDesignQuery.GetSetDate_AEMRFLOWXML(mstrFormNo, mstrUpdateNo, ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFLOWINPUTSIZE, ref mFormFlowSheet, ref mFormFlowSheetHead);
                mFLOWGB = pForm.FmFLOWGB;
            }

            if (mFormFlowSheetHead != null)
            {
                SetWriteSpd();
            }

            //조회 일경우
            if (mstrMode.Equals("H") || mstrMode.Equals("V") || mstrMode.Equals("P"))
            {
                spcChart.Panel1Collapsed = true;
                spcChart.Panel1.Visible = false;
                if(bCopy)
                {
                    btnHis.Visible = false;
                }
                return;
            }
        }

        /// <summary>
        /// 권한별 환경을 설정을 한다(작성화면 안보이게 등)
        /// </summary>
        private void SetUserAut()
        {
            if (clsType.User.AuAWRITE == "1")
            {
                panTopMenu.Visible = true;
                panWrite.Visible = true;
            }
            else
            {
                panTopMenu.Visible = false;
                panWrite.Visible = false;
            }

            panTopMenu.Visible = mstrMode.Equals("W");
            panWrite.Visible = mstrMode.Equals("W");

            if (clsType.User.AuAPRINTIN == "1")
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
            }

            usFormTopMenuEvent.mbtnClear.Visible = true;
            usFormTopMenuEvent.mbtnPrintNull.Visible = true;
            usFormTopMenuEvent.mbtnSave.Visible = true;
            usFormTopMenuEvent.mbtnSaveTemp.Visible = false; //플로우시트는 임시저장 없음
        }

        /// <summary>
        /// 스프래드 클리어
        /// </summary>
        private void pClearForm()
        {
            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            usFormTopMenuEvent.txtMedFrTime.Text = usFormTopMenuEvent.dtMedFrDate.Value.ToString("HH:mm");

            ssWrite_Sheet1.RowCount = 0;
            ssWrite_Sheet1.ColumnCount = 0;
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.ColumnCount = 0;
        }

        /// <summary>
        /// 작성 스프래드 세팅
        /// </summary>
        private void SetWriteSpd()
        {
            pClearForm();

            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            ssView_Sheet1.DefaultStyle.Border = complexBorder2;
            ssView_Sheet1.SheetCornerStyle.Border = complexBorder2;
            ssView_Sheet1.ColumnHeader.DefaultStyle.Border = complexBorder2;
            ssView_Sheet1.RowHeader.DefaultStyle.Border = complexBorder2;

            ssWrite_Sheet1.SheetCornerStyle.Border = complexBorder2;
            ssWrite_Sheet1.DefaultStyle.Border = complexBorder2;
            ssWrite_Sheet1.ColumnHeader.DefaultStyle.Border = complexBorder2;
            ssWrite_Sheet1.RowHeader.DefaultStyle.Border = complexBorder2;


            ssView.BorderStyle = BorderStyle.FixedSingle;

            if (mFLOWGB == "ROW") //세로방식(아래로 작성)
            {
                panWriteTop.Visible = true;
                panWriteTop.Height = TopPanelHeight + 48;

                panCbo.Left = 0;

                if (mstrMode.Equals("W") && pForm.FmOLDGB != 1 && (pForm.FmFORMNO == 1725 || pForm.FmFORMNO == 1573))
                {
                    panCbo.Visible = true;
                }

                #region 혈종기록지 버튼 위치 이동
                if (pForm.FmFORMNO == 3552) //세로방식(아래로 작성)
                {
                    btnGetExam.Visible = true;
                    btnGetExam.Top = btnWebRmk.Top;
                    btnGetExam.Left = btnWebRmk.Left - btnGetExam.Width - 10;
                }
                #endregion

                btnRmk2.Visible = mstrMode.Equals("W") && pForm.FmFORMNO == 1573;

                if (panCbo.Visible)
                {
                    string ItemCd = pForm.FmFORMNO == 1725 ? "상처간호" : "욕창간호";
                    if (clsEmrQuery.ChartOrder_Exists(this, pAcp, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyy-MM-dd"), ItemCd, ItemCd))
                    {
                        btnSaveOrder.BackColor = Color.Pink;
                        btnSaveOrder.Text = "당일처방내역";
                    }
                    else
                    {
                        btnSaveOrder.Text = "처방전송";
                        btnSaveOrder.BackColor = Color.Gainsboro;
                    }

                    panCbo.Top  = 50;
                    btnAdd.Top  = 54;
                    btnRemove.Top = 54;
                    btnSaveOrder.Top = 54;
                    btnRmk2.Top = 20;
                    btnWebRmk.Top = 20;

                    if (ssChk2.Visible == false)
                    {
                        btnSearchGBN.Left = ssChk1.Width;
                    }
                    else
                    {
                        btnSearchGBN.Left = ssChk2.Left + 30;
                    }

                    btnAdd.Left = panCbo.Width;
                    btnRemove.Left = panCbo.Left + btnAdd.Left + btnRemove.Width;
                    btnSaveOrder.Left = btnRemove.Left + btnRemove.Width + 5;
                    btnSaveOrder.BackColor = clsEmrQuery.ChartOrder_Exists(this, pAcp, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyy-MM-dd"), ItemCd, ItemCd) ? Color.Pink : Color.Gainsboro;
                    btnRmk2.Left = 0;
                    btnWebRmk.Left = btnRmk2.Left + 55;
                }
                else
                {
                    btnAdd.Top = 50;
                    btnAdd.Left = btnAdd.Width + 1;

                    btnRemove.Top = 50;
                    btnRemove.Left = btnAdd.Left + btnRemove.Width + 1;
                }

                spcChart.Orientation = Orientation.Vertical;

                ssWrite_Sheet1.RowCount = mFLOWITEMCNT;
                ssWrite_Sheet1.RowHeader.ColumnCount = mFLOWHEADCNT;
                ssWrite_Sheet1.ColumnCount = 1;
                ssWrite_Sheet1.ColumnHeader.Rows[0].Visible = false;

                DesignFunc.SetInitSpd(ssWrite, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet);
                DesignFunc.SetHead(ssWrite_Sheet1, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheetHead);

                ssView_Sheet1.RowCount = mFLOWITEMCNT + clsEmrNum.FLOWVIWADD;
                ssView_Sheet1.RowHeader.ColumnCount = mFLOWHEADCNT;
                ssView_Sheet1.ColumnCount = 1;
                ssView_Sheet1.ColumnHeader.Rows[0].Visible = false;
                ssWrite_Sheet1.Columns[0].Width = mFLOWINPUTSIZE == 0 ? 30 : mFLOWINPUTSIZE;

                DesignFunc.SetInitSpd(ssView, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet, "V");
                DesignFunc.SetHead(ssView_Sheet1, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheetHead, "V");
                Application.DoEvents();
                ssView_Sheet1.Rows[0, ssView_Sheet1.RowCount - 1].Locked = false;


                //spcChart.Panel1 사이즈 설정
                int WriteWidth = 0;
                WriteWidth += mFormFlowSheet[0].Width;
                for (int i = 0; i < mFLOWHEADCNT; i++)
                {
                    WriteWidth += mFormFlowSheetHead[0, i].Width;
                }
                spcChart.SplitterDistance = WriteWidth + mFLOWHEADCNT;
                //spcChart.Panel1.Width = 10 + WriteWidth;
            }
            else //가로방식(옆으로 작성)
            {
                panWriteTop.Visible = false;
                panWriteTop.Width = TopPanelHeight;

                spcChart.Orientation = Orientation.Horizontal;
                ssWrite_Sheet1.ColumnCount = mFLOWITEMCNT;
                ssWrite_Sheet1.ColumnHeader.RowCount = mFLOWHEADCNT;
                ssWrite_Sheet1.RowCount = 1;
                DesignFunc.SetInitSpd(ssWrite, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet);
                DesignFunc.SetHead(ssWrite_Sheet1, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheetHead);

                ssView_Sheet1.ColumnCount = mFLOWITEMCNT + clsEmrNum.FLOWVIWADD;
                ssView_Sheet1.ColumnHeader.RowCount = mFLOWHEADCNT;
                ssView_Sheet1.RowCount = 1;
                DesignFunc.SetInitSpd(ssView, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet, "V");
                DesignFunc.SetHead(ssView_Sheet1, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheetHead, "V");

                ssView_Sheet1.Columns[0, ssView_Sheet1.ColumnCount - 1].Locked = false;
                ssWrite_Sheet1.Rows[0].Height = mFLOWINPUTSIZE == 0 ? 30 : mFLOWINPUTSIZE;

                //spcChart.Panel1 사이즈 설정
            }

            clsSpread.gSpreadEnter_NextCol(ssWrite);

            //V/S
            if (pForm.FmFORMNO ==  1571 || pForm.FmFORMNO ==  2201 || pForm.FmFORMNO == 2136)
            {
                DefaultValue();
            }
            else if (pForm.FmFORMNO == 1572)
            {
                if (mFLOWGB.Equals("ROW"))
                {
                    ssWrite_Sheet1.Cells[0, 0, ssWrite_Sheet1.RowCount - 1, 0].Text = "";
                }
                else
                {
                    ssWrite_Sheet1.Cells[0, 0, 0, ssWrite_Sheet1.ColumnCount - 1].Text = "";
                }

                DesignFunc.DefaultValue(ssWrite_Sheet1, mFLOWGB, mFormFlowSheet, "ta6", "Unit");
                DesignFunc.DefaultValue(ssWrite_Sheet1, mFLOWGB, mFormFlowSheet, "ta7", "SC");
                DesignFunc.DefaultValue(ssWrite_Sheet1, mFLOWGB, mFormFlowSheet, "I0000035481", "Unit");
                DesignFunc.DefaultValue(ssWrite_Sheet1, mFLOWGB, mFormFlowSheet, "I0000035482", "SC");
            }
            else if (pForm.FmFORMNO == 1969)
            {
                if (mFLOWGB.Equals("ROW"))
                {
                    ssWrite_Sheet1.Cells[0, 0, ssWrite_Sheet1.RowCount - 1, 0].Text = "";
                }
                else
                {
                    ssWrite_Sheet1.Cells[0, 0, 0, ssWrite_Sheet1.ColumnCount - 1].Text = "";
                }
            }
            //else if (pForm.FmFORMNO == 3552)
            //{
            //    MO_ROW_HEADER_SET();
            //}
        }

        #region MO 로우 칼럼헤더 오더 명칭 반영
        private void MO_ROW_HEADER_SET()
        {
            List<Dictionary<string, object>> dt = FormPatInfoQuery.Query_MO_ORDER_NAME_LIST(pAcp);

            if (dt == null)
                return;

            if (dt.Count == 0)
            {
                if (mstrFormNo != "0")
                {
                    FormDesignQuery.GetSetDate_AEMRFLOWXML(mstrFormNo, mstrUpdateNo, ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFLOWINPUTSIZE, ref mFormFlowSheet, ref mFormFlowSheetHead);
                    mFLOWGB = pForm.FmFLOWGB;
                }

                if (mFormFlowSheetHead != null)
                {
                    SetWriteSpd();
                }
                return;
            }

            for (int i = 0; i < dt.Count; i++)
            {
                ssWrite_Sheet1.RowHeader.Cells[dt[i]["SEQNO"].To<int>(0), 1].Text = dt[i]["ORDERNAME"].To<string>();
                ssView_Sheet1.RowHeader.Cells[dt[i]["SEQNO"].To<int>(0) + 2, 1].Text = dt[i]["ORDERNAME"].To<string>();
            }
        }
        #endregion

        #region 기본값 V/S
        /// <summary>
        /// 생성후 기본값 설정
        /// </summary>
        void DefaultValue()
        {
            if (pAcp == null)
                return;

            Cursor.Current = Cursors.WaitCursor;

            string SQL = string.Empty;
            DataTable dt = null;
            OracleDataReader reader = null;

            string strVal1 = string.Empty;
            string strVal2 = string.Empty;

            if (pForm.FmOLDGB == 1 && (pForm.FmFORMNO == 1562 || pForm.FmFORMNO ==  2201))
            #region  mstrFormNo.Equals("1562") || mstrFormNo.Equals("2201")
            {
                SQL = " SELECT VAL1, VAL2 FROM ADMIN.EMR_SETUP_01 ";
                SQL += ComNum.VBLF + " WHERE BUSE = '" + clsType.User.BuseCode + "'";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        strVal1 = dt.Rows[0]["VAL1"].ToString().Trim();
                        strVal2 = dt.Rows[0]["VAL2"].ToString().Trim();
                    }

                    dt.Dispose();

                    int intCol = -1;
                    List<string> strTEMP1 = new List<string>();
                    int index = -1;

                    switch (mstrFormNo)
                    {
                        case "1562":
                            intCol = 17;
                            break;
                        case "2201":
                            intCol = 3;
                            break;
                    }

                    #region 쿼리

                    SQL = " SELECT RT_A, LT_A, RT_L, LT_L ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.NUR_VITAL_REGION ";
                    SQL = SQL + ComNum.VBLF + "    WHERE PANO = '" + pAcp.ptNo + "'";

                    string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (SqlErr.Length > 0)
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }


                    ListBox strTEMP1_T = new ListBox();
                    strTEMP1.Clear();

                    if (reader.HasRows && reader.Read())
                    {
                        if (reader.GetValue(0).ToString().Trim().Equals("0"))
                        {
                            strTEMP1.Add("Rt Arm");
                        }
                        if (reader.GetValue(1).ToString().Trim().Equals("0"))
                        {
                            strTEMP1.Add("Lt Arm");
                        }
                        if (reader.GetValue(2).ToString().Trim().Equals("0"))
                        {
                            strTEMP1.Add("Rt Leg");
                        }
                        if (reader.GetValue(3).ToString().Trim().Equals("0"))
                        {
                            strTEMP1.Add("Lt Leg");
                        }

                        strTEMP1_T.Items.Clear();
                        strTEMP1_T.Items.AddRange(strTEMP1.ToArray());

                        if (strVal1.Equals("Rt Arm") && strTEMP1.IndexOf("Rt Arm") == -1)
                        {
                        }
                        else if (strVal1.Equals("Lt Arm") && strTEMP1.IndexOf("Lt Arm") == -1)
                        {
                        }
                        else if (strVal1.Equals("Rt Leg") && strTEMP1.IndexOf("Rt Leg") == -1)
                        {
                        }
                        else if (strVal1.Equals("Lt Leg") && strTEMP1.IndexOf("Lt Leg") == -1)
                        {
                        }
                        else
                        {
                            index = strTEMP1_T.Items.IndexOf(strVal1);
                            if (strVal1.Length > 0 && index != -1)
                            {
                                strTEMP1_T.Items.RemoveAt(index);
                                strTEMP1_T.Items.Insert(0, strVal1);
                            }
                        }

                        ssWrite_Sheet1.Cells[0, intCol].CellType = null;
                        ComboBoxCellType TypeCombo = new ComboBoxCellType();
                        TypeCombo.ListControl = strTEMP1_T;
                        TypeCombo.Editable = true;
                        ssWrite_Sheet1.Cells[0, intCol].CellType = TypeCombo;
                        ssWrite_Sheet1.Cells[0, intCol].Text = strTEMP1_T.Items[0].ToString();
                    }
                    else
                    {
                        ListBox strTEMP2_T = new ListBox();
                        strTEMP2_T.Items.Add("Rt Arm");
                        strTEMP2_T.Items.Add("Lt Arm");
                        strTEMP2_T.Items.Add("Rt Leg");
                        strTEMP2_T.Items.Add("Lt Leg");

                        if (strVal1.Length > 0)
                        {
                            index = strTEMP2_T.Items.IndexOf(strVal1);
                            if (index != -1)
                            {
                                strTEMP2_T.Items.RemoveAt(index);
                                strTEMP2_T.Items.Insert(0, strVal1);
                            }
                        }

                        ComboBoxCellType TypeCombo3 = new ComboBoxCellType();
                        ssWrite_Sheet1.Cells[0, intCol].CellType = null;
                        TypeCombo3.ListControl = strTEMP2_T;
                        TypeCombo3.Editable = true;
                        ssWrite_Sheet1.Cells[0, intCol].CellType = TypeCombo3;
                        ssWrite_Sheet1.Cells[0, intCol].Text = strTEMP2_T.Items[0].ToString();
                    }

                    reader.Dispose();


                    switch (mstrFormNo)
                    {
                        case "1562":
                            intCol = 21;
                            break;
                        case "2201":
                            intCol = 7;
                            break;
                    }

                    ListBox strTEMP3_T = new ListBox();
                    strTEMP3_T.Items.Clear();
                    strTEMP3_T.Items.Add("고막");
                    strTEMP3_T.Items.Add("Axilla");
                    strTEMP3_T.Items.Add("Oral");
                    strTEMP3_T.Items.Add("Rectal");

                    index = strTEMP3_T.Items.IndexOf(strVal2);
                    if (strVal2.Length > 0 && index != -1)
                    {
                        strTEMP3_T.Items.RemoveAt(index);
                        strTEMP3_T.Items.Insert(0, strVal2);
                    }

                    ComboBoxCellType TypeCombo4 = new ComboBoxCellType();
                    ssWrite_Sheet1.Cells[0, intCol].CellType = null;
                    TypeCombo4.ListControl = strTEMP3_T;
                    TypeCombo4.Editable = true;
                    ssWrite_Sheet1.Cells[0, intCol].CellType = TypeCombo4;
                    ssWrite_Sheet1.Cells[0, intCol].Text = strTEMP3_T.Items[0].ToString();
                    #endregion

                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, ex.Message);
                    return;
                }
                #endregion
            }

            #region 1571(복막투석)
            if (pForm.FmFORMNO ==  1571 && pForm.FmOLDGB == 1)
            {
                NumberCellType numberCell = new NumberCellType();
                numberCell.DecimalPlaces = 0;
                ssWrite_Sheet1.Cells[0, 6].CellType = numberCell;
                ssWrite_Sheet1.Cells[0, 9].CellType = numberCell;
                ssWrite_Sheet1.Cells[0, 10].CellType = numberCell;
                ssWrite_Sheet1.Cells[0, 10].Formula = "G1-J1";
            }
            #endregion

            #region 2136(회복실 섭취배설)
            if (mstrFormNo.Equals("2136"))
            {
                ssWrite_Sheet1.RowCount = 3;
                ssWrite_Sheet1.Cells[0, 0].Text = "Intake";
                ssWrite_Sheet1.Cells[1, 0].Text = "Output";
                ssWrite_Sheet1.Cells[2, 0].Text = "Medication";

                List<string> strTEMP = new List<string>();
                List<string> strTEMP2 = new List<string>();
                List<string> strTEMP3 = new List<string>();

                #region 쿼리
                SQL = "SELECT GUBUN, CODE ";
                SQL += ComNum.VBLF + " FROM ADMIN.BAS_BCODE ";
                SQL += ComNum.VBLF + " WHERE GUBUN IN ('OP_Intake','OP_Output','OP_Medication') ";
                SQL += ComNum.VBLF + " ORDER BY GUBUN ";

                try
                {


                    string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                        ComFunc.MsgBox(sqlErr);
                        return;
                    }

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            switch (reader.GetValue(0).ToString().Trim())
                            {
                                case "OP_Intake":
                                    strTEMP.Add(reader.GetValue(1).ToString().Trim());
                                    break;
                                case "OP_Output":
                                    strTEMP2.Add(reader.GetValue(1).ToString().Trim());
                                    break;
                                case "OP_Medication":
                                    strTEMP3.Add(reader.GetValue(1).ToString().Trim());
                                    break;
                            }

                        }
                    }
                    reader.Dispose();

                }
                catch (Exception ex)
                {
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                }

                #endregion

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 1; j < ssWrite_Sheet1.ColumnCount - 1; j++)
                    {
                        ComboBoxCellType boxCellType = new ComboBoxCellType();
                        ListBox listBox = new ListBox();
                        if (i == 0)
                        {
                            listBox.Items.AddRange(strTEMP.ToArray());
                        }
                        else if (i == 1)
                        {
                            listBox.Items.AddRange(strTEMP2.ToArray());
                        }
                        else
                        {
                            listBox.Items.AddRange(strTEMP3.ToArray());
                        }
                        boxCellType.Editable = true;
                        boxCellType.ListControl = listBox;
                        boxCellType.MaxDrop = 10;
                        boxCellType.MaxLength = 150;
                        boxCellType.ListAlignment = FarPoint.Win.ListAlignment.Left;
                        ssWrite_Sheet1.Cells[i, j].CellType = boxCellType;
                        ssWrite_Sheet1.Cells[i, j].HorizontalAlignment = CellHorizontalAlignment.Left;
                        ssWrite_Sheet1.Cells[i, j].VerticalAlignment = CellVerticalAlignment.Center;
                    }
                }
            }
            #endregion
        }

        #endregion

        /// <summary>
        /// 스프래드에 출력여부 표시하거나 숨기기
        /// </summary>
        private void SetPrintVisible()
        {
            //int i = 0;
            //if (mFLOWGB == "ROW")
            //{
            //    for (i = 1; i < mFLOWHEADCNT; i++)
            //    {
            //        ssView_Sheet1.Columns[i].Visible = true;
            //    }
            //}
            //else
            //{
            //    for (i = 1; i < mFLOWHEADCNT; i++)
            //    {
            //        ssView_Sheet1.Rows[i].Visible = true;
            //    }
            //}


        }

        /// <summary>
        /// 작성내역을 세팅한다
        /// </summary>
        private void pSetEmrInfo()
        {
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);

            //EMRNO가 있으면 기록 정보를 세팅을 한다.
            //TODO
            //LoadEmrChartInfo();

        }

        /// <summary>
        /// 사용자 템플릿 작성내역을 불러온다
        /// </summary>
        private void pSetEmrInfoTemplet()
        {
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            //clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);

            //TODO
            //clsXML.LoadDataUserChartRow(clsDB.DbCon, this, mstrMACRONO, false);
        }

        /// <summary>
        /// 출력
        /// </summary>
        private int pPrintForm()
        {
            string strMode = "A";
            int rtnVal = 0;

            if (FlowData.Rows.Count > 0)
            {
                if (pForm.FmOLDGB != 1 &&
                              (pForm.FmFORMNO == 965 || pForm.FmFORMNO == 2137 || pForm.FmFORMNO == 2049 ||
                              pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 1575 || pForm.FmFORMNO == 2135 ||
                              pForm.FmFORMNO == 1935 || pForm.FmFORMNO == 2431 || pForm.FmFORMNO == 1969 || 
                              pForm.FmFORMNO == 2201))
                {
                }
                else
                {
                    SetFlowViewDataBind("P");
                }
            }

            string strPreView = "P";
            FpSpread prtSpd = ssView;
            SheetView sheetView = null;

            if (VB.IsNumeric(mstrVal))
            {
                strMode = "P";
                clsFormPrint.mstrPRINTFLAG = "0"; //원외출력으로 변경
            }
            else
            {
                strPreView = clsType.User.BuseCode.Equals("078201")  ? "P" : "V";                
            }

            btnPrint.Enabled = false;

            List<int> lstVal = new List<int>();

            #region 출력전 배경색 로우 저장

            if (mFLOWGB.Equals("ROW"))
            {
                for (int i = 0; i < ssView_Sheet1.ColumnCount; i++)
                {
                    if(ssView_Sheet1.Columns[i].BackColor == ComNum.SPSELCOLOR)
                    {
                        lstVal.Add(i);
                    }
                }

                ssView_Sheet1.Columns[0, ssView_Sheet1.ColumnCount - 1].BackColor = Color.White;
                ssView_Sheet1.Columns[0, ssView_Sheet1.ColumnCount - 1].ForeColor = Color.Black;
            }
            else
            {
                for (int i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (ssView_Sheet1.Rows[i].BackColor == ComNum.SPSELCOLOR)
                    {
                        lstVal.Add(i);
                    }
                }
                ssView_Sheet1.Rows[0, ssView_Sheet1.RowCount - 1].BackColor = Color.White;
                ssView_Sheet1.Rows[0, ssView_Sheet1.RowCount - 1].ForeColor = Color.Black;
            }
            #endregion

            if (mFLOWGB.Equals("ROW"))
            {
                ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 3].Visible = false;
            }
            else
            {
                ssView_Sheet1.Columns[ssView_Sheet1.ColumnCount - 3].Visible = false;
            }

            SetPrintVisible();

            #region 필터
            IRowFilter rf = ssView.ActiveSheet.RowFilter;
            bool FilterPrint = false;

            if ((mEmrCallForm != null && ((Form) mEmrCallForm).Name.Equals("frmEmrJobChartCopy") == false) &&   rf != null && rf.ColumnDefinitions != null)
            {
                foreach (FilterColumnDefinition fcd in rf.ColumnDefinitions)
                {
                    FilterItemCollection filters = fcd.Filters;
                    IFilterItem iFilterItem = fcd.Filters.Cast<IFilterItem>().Where(d => d.DisplayName.Equals("Default Filter") == false).FirstOrDefault();

                    if (iFilterItem is MultiValuesFilterItem)
                    {
                        MultiValuesFilterItem mvFilterItem = iFilterItem as MultiValuesFilterItem;
                        if (mvFilterItem != null)
                        {
                            FilterPrint = true;
                            break;
                        }
                    }
                }
            }

            if (strPreView.Equals("V") && FilterPrint)
            {

                sheetView = CopySheet(ssView_Sheet1);
                prtSpd = new FpSpread();
                prtSpd.Sheets.Clear();
                prtSpd.Sheets.Add(sheetView);
                if (mFLOWGB.Equals("ROW"))
                {
                    sheetView.ColumnCount = 0;
                }
                else
                {
                    sheetView.RowCount = 0;
                }

                List<int> lstRemoveRow = new List<int>();
                for (int i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (ssView_Sheet1.RowFilter.IsRowFilteredOut(i) == false)
                    {
                        if (mFLOWGB.Equals("ROW"))
                        {
                            sheetView.ColumnCount += 1;
                            for (int j = 0; j < ssView_Sheet1.RowCount; j++)
                            {
                                sheetView.Rows[j].Height = ssView_Sheet1.Rows[i].Height;
                                sheetView.Cells[j, sheetView.ColumnCount - 1].Text = ssView_Sheet1.Cells[i, j].Text.Trim();
                                sheetView.Columns[sheetView.ColumnCount - 1].Width = ssView_Sheet1.Columns[j].Width;
                            }
                        }
                        else
                        {
                            sheetView.Rows.Count += 1;
                            sheetView.Rows[sheetView.Rows.Count - 1].Height = ssView_Sheet1.Rows[i].Height;
                            for (int j = 0; j < ssView_Sheet1.ColumnCount; j++)
                            {
                                sheetView.Cells[sheetView.RowCount - 1, j].Text = ssView_Sheet1.Cells[i, j].Text.Trim();
                            }
                        }
                    }
                }
            }
            #endregion

            pAcp.age = ComFunc.AgeCalcX1(pAcp.ssno1 + pAcp.ssno2, pAcp.medFrDate);
            if (pForm.FmPRINTTYPE == 2)
            {
                //rtnVal = clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                //                         ssView, strPreView, 30, 20, 30, 20, false, PrintOrientation.Landscape, mFLOWGB, -1, ssView_Sheet1.RowCount - 3, mFLOWHEADCNT, strMode);
                rtnVal = clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                         prtSpd, strPreView, 30, 20, 30, 20, false, PrintOrientation.Landscape, mFLOWGB, -1, ssView_Sheet1.RowCount - 3, mFLOWHEADCNT, strMode);

            }
            else
            {
                //rtnVal = clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                //                         ssView, strPreView, 30, 20, 30, 20, false, PrintOrientation.Portrait, mFLOWGB, -1, ssView_Sheet1.ColumnCount - 3, mFLOWHEADCNT, strMode);
                rtnVal = clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                          prtSpd, strPreView, 30, 20, 30, 20, false, PrintOrientation.Portrait, mFLOWGB, -1, ssView_Sheet1.ColumnCount - 3, mFLOWHEADCNT, strMode);
            }

            //ComFunc.Delay(1000);

            #region 출력후 배경색 되돌리기
            if (mFLOWGB.Equals("ROW"))
            {
                for (int i = 0; i < lstVal.Count; i++)
                {
                    ssView_Sheet1.Columns[lstVal[i]].BackColor = ComNum.SPSELCOLOR;
                }
            }
            else
            {
                for (int i = 0; i < lstVal.Count; i++)
                {
                    ssView_Sheet1.Rows[lstVal[i]].BackColor = ComNum.SPSELCOLOR;
                }
            }
            #endregion

            btnPrint.Enabled = true;

            if (mFLOWGB.Equals("ROW"))
            {
                ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 3].Visible = true;
            }
            else
            {
                ssView_Sheet1.Columns[ssView_Sheet1.ColumnCount - 3].Visible = true;
            }

            if (sheetView != null)
            {
                sheetView.Dispose();
                prtSpd.Dispose();
            }

            return rtnVal;
        }

        private SheetView CopySheet(SheetView sheet)
        {
            SheetView newSheet = null;

            if (sheet != null)
            {
                newSheet = (SheetView)FarPoint.Win.Serializer.LoadObjectXml(ssView_Sheet1.GetType(), FarPoint.Win.Serializer.GetObjectXml(sheet, "CopySheet"), "CopySheet");
            }

            return newSheet;
        }

        #endregion //Private Function

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
            //intMaxIdx = 1;

            SetFormInfo();
            if (mstrFormNo != "0")
            {
                FormDesignQuery.GetSetDate_AEMRFLOWXML(mstrFormNo, mstrUpdateNo, ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFLOWINPUTSIZE, ref mFormFlowSheet, ref mFormFlowSheetHead);
                mFLOWGB = pForm.FmFLOWGB;
            }

            if (mFormFlowSheetHead != null)
            {
                SetWriteSpd();
            }

            //기록지 정보를 세팅한다.
            if (mstrMode.Equals("W"))
            {
                usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                usFormTopMenuEvent.txtMedFrTime.Text = usFormTopMenuEvent.dtMedFrDate.Value.ToString("HH:mm");
                btnSearchData.Visible = pForm.FmFORMNO == 2201;

                if (pForm.FmFORMNO == 1573 || pForm.FmFORMNO == 1725)
                {
                    foreach (FormFlowSheet flowSheet in mFormFlowSheet.Where(d => d.ItemCode.IndexOf("_") != -1).ToList())
                    {
                        if (flowSheet.ItemCode.IndexOf("_1") == -1)
                        {
                            ssWrite_Sheet1.Rows[flowSheet.ItemNumber].Visible = false;
                            ssWrite_Sheet1.Cells[flowSheet.ItemNumber, 0].Text = "";
                        }
                    }

                    btnSearchGBN.PerformClick();

                    SetFlowViewLastData();
                    SetFlowViewGbnGroup();

                    if (pForm.FmFORMNO == 1573)
                    {
                        GetDcur();
                    }
                }
            }

            SetScoreVisible();
            SetDateVisible();
            SetDateVisible3();
            GetSearchData();
        }

        /// <summary>
        /// 폼이 로드할때 초기 세팅을 한다
        /// </summary>
        public void pInitForm()
        {
            //SetFormInfo(); //TODO

            SetTopMenuLoad();
            
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double pSaveData()
        {
            double dblEmrNo = 0;

            if (pAcp.ptNo.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "환자를 선택해 주십시오.");
                return VB.Val(mstrEmrNo);
            }

            if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                return VB.Val(mstrEmrNo);

            if (usFormTopMenuEvent.txtMedFrTime.Text.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "시간을 설정해 주십시오.");
                return VB.Val(mstrEmrNo);
            }

            string strDate = string.Format("{0} {1}:{2}",
                usFormTopMenuEvent.dtMedFrDate.Value.ToShortDateString(),
                VB.Left(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2),
                VB.Right(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2));

            if (!VB.IsDate(strDate))
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return VB.Val(mstrEmrNo);
            }

            #region 작성일자 팝업 알림창
            string strdtpDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
            string MsgDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyy-MM-dd");
            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            if (pAcp.inOutCls == "O")
            {
                if (pAcp.medFrDate != usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd") && pAcp.medDeptCd.Equals("ER") == false)
                {
                    if (ComFunc.MsgBoxQEx(this, "작성일자가 외래 진료일이 아닙니다 계속 작성하시겠습니까?") == DialogResult.No)
                    {
                        return VB.Val(mstrEmrNo);
                    }
                }
            }
            else
            {
                if (pAcp.medEndDate != "")
                {
                    if ((VB.Val(pAcp.medFrDate) > VB.Val(strdtpDate)) || (VB.Val(pAcp.medEndDate) < VB.Val(strdtpDate)))
                    {
                        if (ComFunc.MsgBoxQEx(this, "작성일자가 재원기간을 벗어났습니다.\r\n현재 지정하신 작성일자는 '" + MsgDate + "' 입니다 정말 이 날짜로 계속 작성하시겠습니까?") == DialogResult.No)
                        {
                            return VB.Val(mstrEmrNo);
                        }
                    }
                }
                else
                {
                    if ((VB.Val(pAcp.medFrDate) > VB.Val(strdtpDate)) || (VB.Val(strCurDate) < VB.Val(strdtpDate)))
                    {
                        if (ComFunc.MsgBoxQEx(this, "작성일자가 재원기간을 벗어났습니다.\r\n현재 지정하신 작성일자는 '" + MsgDate + "' 입니다 정말 이 날짜로 계속 작성하시겠습니까?") == DialogResult.No)
                        {
                            return VB.Val(mstrEmrNo);
                        }
                    }
                }

                DateTime dtpChartDate = DateTime.ParseExact(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd") + " " + VB.Left(usFormTopMenuEvent.txtMedFrTime.Text.Replace(":", ""), 4), "yyyyMMdd HHmm", null);
                DateTime dtpSysDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

                if (dtpChartDate > dtpSysDate)
                {
                    ComFunc.MsgBoxEx(this, "현재 시간 이후로 작성할 수 없습니다.");
                    return VB.Val(mstrEmrNo);
                }
            }
            #endregion

            bool blnSave = false;

            if (pForm.FmOLDGB == 1)
            {
                blnSave = SaveFlowDataOld();
            }
            else
            {
                #region 물리치료 경과 기록지 ㅊ체크
                if (pForm.FmGRPFORMNO == 1031 && pForm.FmFORMNAME.IndexOf("경과기록지") != -1 && mFormFlowSheet.Where(d => d.ItemCode.Equals("I0000032877")).FirstOrDefault() != null)
                {
                    string ChartTimeText = ssWrite_Sheet1.Cells[0, mFormFlowSheet.Where(d => d.ItemCode.Equals("I0000032877")).First().ItemNumber].Text.Trim();
                    string rtnVal = clsEmrQuery.PT_Time_Duplicate(clsDB.DbCon, pAcp, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"), ChartTimeText, mstrEmrNo);
                    if (rtnVal.NotEmpty())
                    {
                        ComFunc.MsgBoxEx(this, rtnVal +  "기록지가 치료시간 범위안에 작성되어 있습니다.");
                        return 0;
                    }
                }
                #endregion

                blnSave = SaveFlowDataNew();
            }

            if (blnSave == true)
            {
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                ssWrite_Sheet1.Cells[0, mstrFormNo.Equals("2136") ? 1 : 0, ssWrite_Sheet1.RowCount - 1, ssWrite_Sheet1.ColumnCount - 1].Text = "";
                usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                usFormTopMenuEvent.txtMedFrTime.Text = usFormTopMenuEvent.dtMedFrDate.Value.ToString("HH:mm");
                mstrEmrNo = string.Empty;
                mEmrUseId = string.Empty;
                GetSearchData();
            }

            return dblEmrNo;
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        private bool pDelData()
        {
            bool rtnVal = false;
            double dEmrNo = VB.Val(mstrEmrNo);

            if (dEmrNo == 0)
                return rtnVal;

            if (dEmrNo != 0)
            {
                if (mEmrUseId.Trim() != clsType.User.IdNumber)
                {
                    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                    return rtnVal;
                }
            }

            Cursor.Current = Cursors.WaitCursor;
            if (pForm.FmOLDGB == 1)
            {
                rtnVal = clsEmrQuery.DeleteEmrXmlData(mstrEmrNo);
            }
            else
            {
                string strCHARTDATE = "";
                string strCHARTTIME = "";

                SpecialFormHis(pAcp, mstrFormNo, mstrEmrNo, ref strCHARTDATE, ref strCHARTTIME);

                rtnVal = clsXML.gDeleteEmrXml(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber);

                if (strCHARTDATE != "")
                {
                    SpecialFormDelete(pAcp, mstrFormNo, mstrEmrNo, strCHARTDATE, strCHARTTIME);
                }
                
            }
            Cursor.Current = Cursors.Default;

            if (rtnVal)
            {
                #region 혈종기록지 로우헤더에 오더명칭 매칭 관련 로직
                if (pForm.FmFORMNO == 3552)
                {
                    clsEmrQuery.MO_ORDER_NAME_DEL(pAcp);
                }
                #endregion

                usFormTopMenuEvent_SetClear();
                GetSearchData();
            }

            return rtnVal;
        }

        /// <summary>
        /// 이전 기록지 저장 루틴
        /// </summary>
        /// <returns></returns>
        bool SaveFlowDataOld()
        {
            bool rtnVal = false;
            try
            {
                if (pAcp.inOutCls == "" || pAcp.ptNo == "" || pAcp.medDeptCd == "" ||
                   pAcp.medDrCd == "" || pAcp.medFrDate == "")
                {
                    ComFunc.MsgBoxEx(this, "환자 정보가 정확하지 않습니다." + ComNum.VBLF + "확인 후 다시 시도 하십시오.");
                    return rtnVal;
                }

                if (VB.Val(mstrEmrNo) != 0)
                {
                    if (ComFunc.MsgBoxQ("기존내용을 변경하시겠습니까?") == DialogResult.No)
                    {
                        return rtnVal;
                    }

                    if (clsType.User.IdNumber != mEmrUseId)
                    {
                        ComFunc.MsgBoxEx(this, "작성된 사용자가 다릅니다. 변경할 수 없습니다.");
                        return rtnVal;
                    }

                    if (clsEmrQuery.READ_CHART_APPLY(this, mstrEmrNo))
                        return rtnVal;

                    if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                        return rtnVal;
                }

                StringBuilder XMLBuilder = new StringBuilder();
                XMLBuilder.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                XMLBuilder.AppendLine("<chart>");

                string strCONTENTS = "(SELECT CONTENTS FROM ADMIN.EMRFORM WHERE FORMNO = " + VB.Val(mstrFormNo) + ")";
                string strUPDATENO = clsEmrQuery.GetMaxUpdateNo(clsDB.DbCon, VB.Val(mstrFormNo)).ToString();

                Cursor.Current = Cursors.WaitCursor;
                if (VB.Val(mstrEmrNo) != 0)
                {
                    if (clsEmrQuery.DeleteEmrXmlData(mstrEmrNo) == false)
                    {
                        return rtnVal;
                    }
                }

                string strChartDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
                string strChartTime = usFormTopMenuEvent.txtMedFrTime.Text.Replace(":", "").Trim();
                string strInOutCls = pAcp.inOutCls;

                double dblEmrNo = 0;
                if (mstrFormNo.Equals("2136") == false)
                {
                    dblEmrNo = ComQuery.GetSequencesNo(clsDB.DbCon, "GetEmrXmlNo");
                    //칼럼
                    for (int i = 0; i < mFormFlowSheet.Length; i++)
                    {
                        if(mFormFlowSheet[i].CellType == "CheckBoxCellType")
                        {
                            XMLBuilder.AppendLine("<" + mFormFlowSheet[i].ItemCode + " type=\"inputText\" label=\"" + mFormFlowSheet[i].ItemName
                            + "\"><![CDATA[" +
                            (mFLOWGB.Equals("COL") ? (ssWrite_Sheet1.Cells[0, i].Text.Trim().Equals("True") ? "1" : "0") : (ssWrite_Sheet1.Cells[i, 0].Text.Trim().Equals("True") ? "1" : "0"))
                            + "]]><" + "/" + mFormFlowSheet[i].ItemCode + ">");
                        }
                        else
                        {
                            XMLBuilder.AppendLine("<" + mFormFlowSheet[i].ItemCode + " type=\"inputText\" label=\"" + mFormFlowSheet[i].ItemName
                            + "\"><![CDATA[" +
                            (mFLOWGB.Equals("COL") ? ssWrite_Sheet1.Cells[0, i].Text.Trim().Replace("'", "`") : ssWrite_Sheet1.Cells[i, 0].Text.Trim().Replace("'", "`"))
                            + "]]><" + "/" + mFormFlowSheet[i].ItemCode + ">");
                        }
                        
                    }

                    XMLBuilder.AppendLine("</chart>");
                    if (clsEmrQuery.SaveEmrXmlData(dblEmrNo.ToString(), mstrFormNo, strChartDate, strChartTime,
                        pAcp.acpNo, pAcp.ptNo, strInOutCls, pAcp.medFrDate, pAcp.medFrTime,
                        pAcp.medEndDate, pAcp.medEndTime, pAcp.medDeptCd, pAcp.medDrCd,
                        XMLBuilder.ToString().Trim(), strUPDATENO))
                    {
                        mstrEmrNo = dblEmrNo.ToString();
                    }
                }
                else
                {
                    for(int Row = 0; Row < 3; Row++)
                    {
                        dblEmrNo = ComQuery.GetSequencesNo(clsDB.DbCon, "GetEmrXmlNo");
                        XMLBuilder.Clear();
                        XMLBuilder.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                        XMLBuilder.AppendLine("<chart>");
                        //칼럼
                        for (int i = 0; i < mFormFlowSheet.Length; i++)
                        {
                            XMLBuilder.AppendLine("<" + mFormFlowSheet[i].ItemCode + " type=\"inputText\" label=\"" + mFormFlowSheet[i].ItemName
                                + "\"><![CDATA[" + (ssWrite_Sheet1.Cells[Row, i].Text.Trim().Replace("'", "`")) + "]]><" + "/" + mFormFlowSheet[i].ItemCode + ">");
                        }

                        XMLBuilder.AppendLine("</chart>");
                        if (clsEmrQuery.SaveEmrXmlData(dblEmrNo.ToString(), mstrFormNo, strChartDate, strChartTime,
                            pAcp.acpNo, pAcp.ptNo, strInOutCls, pAcp.medFrDate, pAcp.medFrTime,
                            pAcp.medEndDate, pAcp.medEndTime, pAcp.medDeptCd, pAcp.medDrCd,
                            XMLBuilder.ToString().Trim(), strUPDATENO))
                        {
                            mstrEmrNo = dblEmrNo.ToString();
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        bool SaveFlowDataNew()
        {
            bool rtnVal = false;

            if (pAcp.inOutCls == "" || pAcp.ptNo == "" || pAcp.medDeptCd == "" ||
                   pAcp.medDrCd == "" || pAcp.medFrDate == "")
            {
                ComFunc.MsgBoxEx(this, "환자 정보가 정확하지 않습니다." + ComNum.VBLF + "확인 후 다시 시도 하십시오.");
                return rtnVal;
            }

            double dblEmrNo = 0;
            string strSAVEGB = "1";
            string strSAVECERT = "1";  //if (blnCertYn == true)
            string strChartDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
            string strChartTime = usFormTopMenuEvent.txtMedFrTime.Text.Replace(":", "").Trim();


            if (VB.Val(mstrEmrNo) != 0)
            {
                if (ComFunc.MsgBoxQ("기존내용을 변경하시겠습니까?") == DialogResult.No)
                {
                    return rtnVal;
                }

                if (clsType.User.IdNumber != mEmrUseId)
                {
                    ComFunc.MsgBoxEx(this, "작성된 사용자가 다릅니다. 변경할 수 없습니다.");
                    return rtnVal;
                }

                if (clsEmrQuery.READ_CHART_APPLY(this, mstrEmrNo))
                    return rtnVal;

                if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                    return rtnVal;

                string strCHARTDATE_T = "";
                string strCHARTTIME_T = "";

                SpecialFormHis(pAcp, mstrFormNo, mstrEmrNo, ref strCHARTDATE_T, ref strCHARTTIME_T);

                if (VB.Left(strChartTime, 4) != VB.Left(strCHARTTIME_T, 4))
                {
                    if (strCHARTDATE_T != "")
                    {
                        SpecialFormDelete(pAcp, mstrFormNo, mstrEmrNo, strCHARTDATE_T, strCHARTTIME_T);
                    }
                }
            }

            #region 상처, 욕창 저장 하드코딩
            if (pForm.FmOLDGB == 0 && (pForm.FmFORMNO == 1573 || pForm.FmFORMNO == 1725))
            {
                SheetView sheetView = (SheetView) FarPoint.Win.Serializer.LoadObjectXml(ssWrite_Sheet1.GetType(), FarPoint.Win.Serializer.GetObjectXml(ssWrite_Sheet1, "CopySheet"), "CopySheet");
                ssWrite2.Sheets.Clear();
                ssWrite2.Sheets.Add(sheetView);

                double iCount = ssWrite_Sheet1.RowCount / mFormFlowSheet.Length;
                for(int i = 0; i < iCount; i++)
                {
                    dblEmrNo = clsEmrQuery.SaveChartMst(clsDB.DbCon, pAcp, this, false, this,
                                                                  mstrFormNo, mstrUpdateNo, mstrEmrNo, strChartDate, strChartTime,
                                                                  clsType.User.IdNumber, clsType.User.IdNumber, strSAVEGB, strSAVECERT, "0", strSaveFlag, ssWrite2, pForm.FmFLOWGB);

                    ssWrite2.ActiveSheet.Rows.Remove(0, mFormFlowSheet.Length);
                }

                ssWrite2.Sheets.Clear();
            }
            #endregion
            else
            {
                dblEmrNo = clsEmrQuery.SaveChartMst(clsDB.DbCon, pAcp, this, false, this,
                                                              mstrFormNo, mstrUpdateNo, mstrEmrNo, strChartDate, strChartTime,
                                                              clsType.User.IdNumber, clsType.User.IdNumber, strSAVEGB, strSAVECERT, "0", strSaveFlag, ssWrite, pForm.FmFLOWGB);

            }


            if (dblEmrNo > 0)
            {
                #region 혈종기록지 로우헤더에 오더명칭 매칭 관련 로직
                if (pForm.FmFORMNO == 3552)
                {
                    clsEmrQuery.MO_ORDER_NAME_SAVE(pAcp, ssWrite);
                }
                #endregion

                rtnVal = true;
                mstrEmrNo = dblEmrNo.ToString();
            }

            SpecialFormSave(strChartDate, strChartTime);

            return rtnVal;
        }

        /// <summary>
        /// 연동된 서식지의 데이타를 지운다
        /// </summary>
        /// <param name="strEmrNo"></param>
        private void SpecialFormDelete(EmrPatient AcpEmr, string strFormNo, string strEmrNo, string strCHARTDATE, string strCHARTTIME)
        {
            bool isSpecialForm = false;

            switch (strFormNo)
            {
                case "1571": //복막투석 연동
                    isSpecialForm = true;
                    break;
                default:
                    break;
            }

            if (isSpecialForm == false)
            {
                return;
            }

            if (strFormNo == "1571")
            {
                Delete_1571_3150(AcpEmr, strCHARTDATE, strCHARTTIME);
            }
        }

        /// <summary>
        /// 기존 연동되는 기록지에 내용이 있는지 확인한다
        /// </summary>
        /// <param name="AcpEmr"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strCHARTDATE"></param>
        /// <param name="strCHARTTIME"></param>
        private void SpecialFormHis(EmrPatient AcpEmr, string strFormNo, string strEmrNo, ref string strCHARTDATE, ref string strCHARTTIME)
        {
            bool isSpecialForm = false;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            switch (strFormNo)
            {
                case "1571": //복막투석 연동
                    isSpecialForm = true;
                    break;
                default:
                    break;
            }

            if (isSpecialForm == false)
            {
                return;
            }

            if (strFormNo == "1571")
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    CHARTDATE, CHARTTIME";
                SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo + " ";

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
                    return;
                }

                strCHARTDATE = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                strCHARTTIME = dt.Rows[0]["CHARTTIME"].ToString().Trim();

                dt.Dispose();
                dt = null;
            }
        }

        /// <summary>
        /// 복막투석 => 임상관찰 기록
        /// </summary>
        /// <param name="strEmrNo"></param>
        private void Delete_1571_3150(EmrPatient AcpEmr, string strCHARTDATE, string strCHARTTIME)
        {
            //DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string ITEMCD = "I0000013209";
            string strDestFormNo = "3150";
            //string strCHARTDATE = "";
            //string strCHARTTIME = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //SQL = "";
                //SQL = SQL + ComNum.VBLF + "SELECT ";
                //SQL = SQL + ComNum.VBLF + "    CHARTDATE, CHARTTIME";
                //SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTMST";
                //SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strEmrNo + " ";

                //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                //if (SqlErr != "")
                //{
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //    return;
                //}

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    return;
                //}

                //strCHARTDATE = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                //strCHARTTIME = dt.Rows[0]["CHARTTIME"].ToString().Trim();

                //dt.Dispose();
                //dt = null;

                SQL = " ";
                SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_EMR + "AEMRCHARTROW SET";
                SQL = SQL + ComNum.VBLF + "    ITEMVALUE = '' ";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = (";
                SQL = SQL + ComNum.VBLF + "             SELECT MAX(EMRNO)";
                SQL = SQL + ComNum.VBLF + "             FROM  " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL = SQL + ComNum.VBLF + "             WHERE ACPNO = " + AcpEmr.acpNo;
                SQL = SQL + ComNum.VBLF + "                 AND CHARTDATE = '" + strCHARTDATE + "'";
                SQL = SQL + ComNum.VBLF + "                 AND CHARTTIME = '" + strCHARTTIME + "'";
                SQL = SQL + ComNum.VBLF + "                 AND FORMNO = " + strDestFormNo;
                SQL = SQL + ComNum.VBLF + "             )";
                SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + ITEMCD + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("임상관찰 데이타를 삭제중 오류가 발생하였습니다.");
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

        /// <summary>
        /// 특수하게 타 기록지와 연동할 경우
        /// </summary>
        private void SpecialFormSave(string strChartDate, string strChartTime)
        {
            bool isSpecialForm = false;

            switch(mstrFormNo)
            {
                case "1571": //복막투석 연동
                    isSpecialForm =true; 
                    break;
                default:
                    break;
            }

            if (isSpecialForm == false)
            {
                return;
            }

            if (mstrFormNo == "1571") 
            {
                Save_1571_3150(strChartDate, strChartTime);
            }
        }

        /// <summary>
        /// 복목투석기록지 => 임상관찰
        /// </summary>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <returns></returns>
        private bool Save_1571_3150(string strChartDate, string strChartTime)
        {
            string ITEMCD = "I0000013209";
            string ITEMVALUE = "";
            string strDestFormNo = "3150";

            #region //Flow Sheet 정보를 가지고 온다
            //EmrForm pForm = null;
            //pForm = clsEmrChart.ClearEmrForm();
            //pForm = clsEmrChart.SerEmrFormInfo(pDbCon, strFormNo, strUpdateNo);

            string pFLOWGB = "";
            int intFLOWITEMCNT = 0;
            int intFLOWHEADCNT = 0;
            int intFLOWINPUTSIZE = 0;
            FormFlowSheet[] pFormFlowSheet = null;
            FormFlowSheetHead[,] pFormFlowSheetHead = null;
            FormDesignQuery.GetSetDate_AEMRFLOWXML(pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), ref pFLOWGB, ref intFLOWITEMCNT, ref intFLOWHEADCNT, ref intFLOWINPUTSIZE, ref pFormFlowSheet, ref pFormFlowSheetHead);
            #endregion //Flow Sheet 정보를 가지고 온다

            for (int i = 0; i < pFormFlowSheet.Length; i++)
            {
                if(ITEMCD == pFormFlowSheet[i].ItemCode)
                {
                    if (pFLOWGB == "COL")
                    {
                        ITEMVALUE = ssWrite.ActiveSheet.Cells[0, i].Text.Trim();
                    }
                    else
                    {
                        ITEMVALUE = ssWrite.ActiveSheet.Cells[i, 0].Text.Trim();
                    }
                    break;
                }
            }

            if (ITEMVALUE == "")
            {
                return true;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);

                #region 재원환자의 경우 당일 기본 시간을 만들어 줘야 한다
                if (clsEmrQueryEtc.SetSaveDefaultVitalTime(clsDB.DbCon, pAcp.acpNo, pAcp.ward, strChartDate, strDestFormNo) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                }
                #endregion

                //해당일자에 아이템이 있는지 확인한다
                if (clsEmrQueryEtc.GetSetTodayItem(clsDB.DbCon, pAcp, strChartDate, strDestFormNo) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                //해당일자에 차팅된 아이템이 있는지 확인한다
                string strChartItems = "I0000013209"; //제수량

                if (clsEmrQueryEtc.GetSetTodayChartedItem(clsDB.DbCon, pAcp, strChartItems, strChartDate, strDestFormNo, strCurDate, strCurTime, "IIO") == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                //0. AEMRBVITALTIME 저장을 한다
                if (clsEmrQueryEtc.SaveAEMRBVITALTIME(clsDB.DbCon, pAcp, strChartDate, strChartTime, strDestFormNo, strCurDate, strCurTime) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                //0. AEMRCHRATMST, AEMRCHARTROW 저장을 한다
                if (SaveAEMRCHRATMSTandAEMRCHARTROW(pAcp, strChartDate, strChartTime, strDestFormNo, strCurDate, strCurTime, ITEMVALUE) == 0)
                {
                    //전자인증을 한다
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 임상관찰 기록지를 실제 저장하는 부분 : AEMRCHRATMST, AEMRCHARTROW
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="AcpEmr"></param>
        /// <param name="strChartDate"></param>
        /// <param name="strChartTime"></param>
        /// <param name="strCurDate"></param>
        /// <param name="strCurTime"></param>
        /// <returns></returns>
        private double SaveAEMRCHRATMSTandAEMRCHARTROW(EmrPatient AcpEmr, string strChartDate, string strChartTime, string strFormNo, string strCurDate, string strCurTime, string pITEMVALUE)
        {
            double rtnVal = 0;

            DataTable dt = null;
            int i = 0;
            int j = 0;
            string SQL = "";
            string SqlErr = "";

            string strUpdateNo = "1";

            double dblEmrHisNo = 0;
            double dblEmrNoNew = 0;

            string strCHARTUSEID = clsType.User.IdNumber;
            string strCOMPUSEID = clsType.User.IdNumber;
            string strSaveFlag = "SAVE";
            string strSAVEGB = "1";
            string strSAVECERT = "1"; // 0:임시저장, 1:인증저장
            string strFORMGB = "0";

            try
            {

                #region //차팅이 된 아이템을 변수에 담는다.
                string[] arryEMRNO_T = null;
                string[] arryITEMCD_T = null;
                string[] arryITEMNO_T = null;
                string[] arryITEMINDEX_T = null;
                string[] arryITEMTYPE_T = null;
                string[] arryITEMVALUE_T = null;
                string[] arryITEMVALUE1_T = null;
                string[] arryDSPSEQ_T = null;

                string strITEMCD = "I0000013209";
                string strITEMNO = "I0000013209";
                string strITEMINDEX = "-1";
                string strITEMTYPE = "TEXT";
                string strITEMVALUE = "";
                string strITEMVALUE1 = "";
                string strDSPSEQ = "0";

                strITEMTYPE = "TEXT"; //pFormFlowSheet[i].CellType;
                strITEMVALUE = pITEMVALUE;

                if (strITEMVALUE != "")
                {
                    if (arryEMRNO_T == null)
                    {
                        arryEMRNO_T = new string[0];
                        arryITEMCD_T = new string[0];
                        arryITEMNO_T = new string[0];
                        arryITEMINDEX_T = new string[0];
                        arryITEMTYPE_T = new string[0];
                        arryITEMVALUE_T = new string[0];
                        arryITEMVALUE1_T = new string[0];
                        arryDSPSEQ_T = new string[0];
                    }

                    Array.Resize<string>(ref arryEMRNO_T, arryEMRNO_T.Length + 1);
                    Array.Resize<string>(ref arryITEMCD_T, arryITEMCD_T.Length + 1);
                    Array.Resize<string>(ref arryITEMNO_T, arryITEMNO_T.Length + 1);
                    Array.Resize<string>(ref arryITEMINDEX_T, arryITEMINDEX_T.Length + 1);
                    Array.Resize<string>(ref arryITEMTYPE_T, arryITEMTYPE_T.Length + 1);
                    Array.Resize<string>(ref arryITEMVALUE_T, arryITEMVALUE_T.Length + 1);
                    Array.Resize<string>(ref arryITEMVALUE1_T, arryITEMVALUE1_T.Length + 1);
                    Array.Resize<string>(ref arryDSPSEQ_T, arryDSPSEQ_T.Length + 1);

                    arryEMRNO_T[arryEMRNO_T.Length - 1] = dblEmrNoNew.ToString();
                    arryITEMCD_T[arryEMRNO_T.Length - 1] = strITEMCD;
                    arryITEMNO_T[arryEMRNO_T.Length - 1] = strITEMNO;
                    arryITEMINDEX_T[arryEMRNO_T.Length - 1] = strITEMINDEX;
                    arryITEMTYPE_T[arryEMRNO_T.Length - 1] = strITEMTYPE;
                    arryITEMVALUE_T[arryEMRNO_T.Length - 1] = strITEMVALUE;
                    arryITEMVALUE1_T[arryEMRNO_T.Length - 1] = strITEMVALUE1;
                    arryDSPSEQ_T[arryEMRNO_T.Length - 1] = strDSPSEQ;
                }

                #endregion //차팅이 된 아이템을 변수에 담는다.

                //동일 시간에 저장된 데이타가 있는지 조회 한다
                double dblEmrNo = 0;
                SQL = " ";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    C.EMRNO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C";
                SQL = SQL + ComNum.VBLF + "WHERE C.PTNO = '" + AcpEmr.ptNo + "' ";
                SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE = '" + strChartDate + "' ";
                SQL = SQL + ComNum.VBLF + "    AND SUBSTR(C.CHARTTIME, 1, 4) = '" + VB.Left(strChartTime, 4) + "' ";
                SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = " + strFormNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    dblEmrNo = VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                //당일 아이템을 조회해서 다시 정리 한다
                #region //Query

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    A.CHARTDATE, B.ITEMCD, B.BASNAME, B.BASEXNAME, B.ITEMGROUP, B.ITEMGROUPNM, ";
                SQL = SQL + ComNum.VBLF + "    R.ITEMVALUE, R.ITEMVALUE1, R.INPUSEID, R.INPDATE, R.INPTIME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ( ";
                SQL = SQL + ComNum.VBLF + "    SELECT ";
                SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
                SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
                SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
                SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('임상관찰그룹')  ";
                SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('임상관찰')  ";
                SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                SQL = SQL + ComNum.VBLF + "    SELECT ";
                SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
                SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
                SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
                SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('섭취배설그룹')  ";
                SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('섭취배설') ";
                SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                SQL = SQL + ComNum.VBLF + "    SELECT ";
                SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
                SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
                SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
                SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('특수치료그룹')  ";
                SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('특수치료') ";
                SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                SQL = SQL + ComNum.VBLF + "    SELECT ";
                SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
                SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBASCD B ";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB ";
                SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
                SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('기본간호그룹')  ";
                SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
                SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('기본간호') ";
                SQL = SQL + ComNum.VBLF + "    ) B ";
                SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.ITEMCD ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                SQL = SQL + ComNum.VBLF + "    ON A.ACPNO = C.ACPNO ";
                SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = C.CHARTDATE ";
                SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = C.FORMNO ";
                SQL = SQL + ComNum.VBLF + "    AND SUBSTR(C.CHARTTIME, 1, 4) = '" + VB.Left(strChartTime, 4) + "' ";
                SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                SQL = SQL + ComNum.VBLF + "    ON C.EMRNO = R.EMRNO ";
                SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD = R.ITEMCD ";
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + strFormNo;
                SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
                SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + strChartDate + "'";
                SQL = SQL + ComNum.VBLF + "ORDER BY B.SORT1, B.SORT2, B.SORT3, B.SORT4, B.SORT5, B.SORT6 ";

                #endregion //Query

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBoxEx(this, AcpEmr.ptName + "[" + AcpEmr.ptNo + "] 님의" + ComNum.VBLF + "당일 임상관찰 아이템을 찾을 수 없습니다.");
                    return rtnVal;
                }

                #region //저장을 위하여 배열에 다시 담는다

                string[] arryEMRNO = new string[dt.Rows.Count];
                string[] arryITEMCD = new string[dt.Rows.Count];
                string[] arryITEMNO = new string[dt.Rows.Count];
                string[] arryITEMINDEX = new string[dt.Rows.Count];
                string[] arryITEMTYPE = new string[dt.Rows.Count];
                string[] arryITEMVALUE = new string[dt.Rows.Count];
                string[] arryITEMVALUE1 = new string[dt.Rows.Count];
                string[] arryDSPSEQ = new string[dt.Rows.Count];
                string[] arryINPUSEID = new string[dt.Rows.Count];
                string[] arryINPDATE = new string[dt.Rows.Count];
                string[] arryINPTIME = new string[dt.Rows.Count];

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    bool IsExistsItem = false;
                    for (j = 0; j < arryITEMCD_T.Length; j++)
                    {
                        if (arryITEMCD_T[j].Trim() == dt.Rows[i]["ITEMCD"].ToString().Trim())
                        {
                            IsExistsItem = true;
                            break;
                        }
                    }

                    arryEMRNO[i] = dblEmrNoNew.ToString();
                    arryITEMCD[i] = dt.Rows[i]["ITEMCD"].ToString().Trim();

                    if (VB.InStr(arryITEMCD[i], "_") > 0)
                    {
                        arryITEMNO[i] = VB.Split(arryITEMCD[i], "_")[0];
                        arryITEMINDEX[i] = VB.Split(arryITEMCD[i], "_")[1];
                    }
                    else
                    {
                        arryITEMNO[i] = arryITEMCD[i];
                        arryITEMINDEX[i] = "-1";
                    }

                    arryITEMTYPE[i] = "TEXT";
                    arryITEMVALUE[i] = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
                    arryITEMVALUE1[i] = dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
                    arryDSPSEQ[i] = i.ToString();
                    arryINPUSEID[i] = dt.Rows[i]["INPUSEID"].ToString().Trim();
                    arryINPDATE[i] = dt.Rows[i]["INPDATE"].ToString().Trim();
                    arryINPTIME[i] = dt.Rows[i]["INPTIME"].ToString().Trim();

                    if (IsExistsItem == true)
                    {
                        arryITEMVALUE[i] = arryITEMVALUE_T[j];
                        arryINPUSEID[i] = clsType.User.IdNumber;
                        arryINPDATE[i] = strCurDate;
                        arryINPTIME[i] = strCurTime;
                    }
                }
                dt.Dispose();
                dt = null;

                #endregion //저장을 위하여 배열에 다시 담는다

                dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));

                if (dblEmrNo > 0)
                {
                    dblEmrNoNew = dblEmrNo;

                    #region //과거기록 백업
                    SqlErr = clsEmrQuery.SaveChartMastHis(clsDB.DbCon, dblEmrNo.ToString(), dblEmrHisNo, strCurDate, strCurTime, "C", strSaveFlag, strCHARTUSEID);
                    if (SqlErr != "OK")
                    {
                        ComFunc.MsgBox("SaveAEMRCHRATMSTandAEMRCHARTROW 오류가 발생했습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    #endregion
                }
                else
                {
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                if (clsEmrQuery.SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, strFormNo, strUpdateNo,
                            strChartDate, strChartTime,
                            strCHARTUSEID, strCOMPUSEID, strSAVEGB, strSAVECERT, strFORMGB,
                            strCurDate, strCurTime, strSaveFlag) == false)
                {
                    ComFunc.MsgBox("SaveAEMRCHRATMSTandAEMRCHARTROW 오류가 발생했습니다.");
                    return rtnVal;
                }

                #region //CHARTROW
                SQL = "";
                SQL = SQL + "\r\n" + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                SQL = SQL + "\r\n" + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                SQL = SQL + "\r\n" + "VALUES (";
                SQL = SQL + "\r\n" + dblEmrNoNew.ToString() + ", ";    //EMRNO
                SQL = SQL + "\r\n" + dblEmrHisNo.ToString() + ", ";    //EMRNOHIS
                SQL = SQL + "\r\n" + " :ITEMCD, ";   //ITEMCD
                SQL = SQL + "\r\n" + " :ITEMNO, "; //ITEMNO
                SQL = SQL + "\r\n" + " :ITEMINDEX, "; //ITEMINDEX
                SQL = SQL + "\r\n" + " :ITEMTYPE, ";   //ITEMTYPE
                SQL = SQL + "\r\n" + " :ITEMVALUE, ";   //ITEMVALUE
                SQL = SQL + "\r\n" + " :DSPSEQ, ";   //DSPSEQ
                SQL = SQL + "\r\n" + " :ITEMVALUE1, ";   //ITEMVALUE
                SQL = SQL + ComNum.VBLF + " :INPUSEID,";   //INPUSEID
                SQL = SQL + ComNum.VBLF + " :INPDATE, ";   //INPDATE
                SQL = SQL + ComNum.VBLF + " :INPTIME ";   //INPTIME
                SQL = SQL + ComNum.VBLF + ")";

                SqlErr = clsDB.ExecuteChartRowEx(clsDB.DbCon, SQL, dblEmrNoNew, dblEmrHisNo, arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE,
                    arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1, arryINPUSEID, arryINPDATE, arryINPTIME);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return 0;
                }

                #endregion //CHARTROW

                rtnVal = dblEmrNoNew;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// 폼에 이전내역을 세팅한다
        /// </summary>
        public void LoadForm()
        {
            pClearForm();
            pSetEmrInfo();
            //pMultiTextHeigh();
        }

        /// <summary>
        /// 사용자 템플릿 내용을 세팅한다
        /// </summary>
        public void LoadFormTemplet()
        {
            pClearForm();
            pSetEmrInfoTemplet();
            //pMultiTextHeigh();
        }

        #endregion

        private void ssView_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.Rows.Count == 0) return;
            if (e.ColumnHeader == true)
            {
                if (clsType.User.BuseCode.Equals("078201"))
                {
                    clsSpread.gSpdSortRow(ssView, e.Column);
                }
                return;
            }

            if (clsType.User.BuseCode.Equals("044201") || ErCoordinator == true)
            {
                if(mFLOWGB.Equals("COL"))
                {
                    lblServerDate.Text = string.Format("{0} \r\n{1}", ssView_Sheet1.Cells[e.Row, 0].Tag, VB.Val(ssView_Sheet1.Cells[e.Row, 1].Tag.ToString()).ToString("00:00:00"));
                }
                else
                {
                    lblServerDate.Text = ssView_Sheet1.Columns[e.Column].Tag.ToString();
                }
                lblServerDate.Visible = true;

                Console.WriteLine("Row: {0}, Col: {1}, Text: {2}",e.Row, e.Column, ssView_Sheet1.Cells[e.Row, e.Column].Text.Trim());
            }
            else
            {
                lblServerDate.Visible = false;

                if (mstrMode.Equals("W") && e.Button == MouseButtons.Right)
                {
                    usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                    usFormTopMenuEvent.txtMedFrTime.Text = usFormTopMenuEvent.dtMedFrDate.Value.ToString("HH:mm");
                    usFormTopMenuEvent.dtMedFrDate.Enabled = true;

                    mEmrUseId = string.Empty;
                    mstrEmrNo = string.Empty;

                    ssWrite_Sheet1.Cells[0, 0, ssWrite_Sheet1.RowCount - 1, ssWrite_Sheet1.ColumnCount - 1].Text = "";

                    if (mFLOWGB.Equals("COL"))
                    {
                        for (int i = 0; i < ssWrite_Sheet1.ColumnCount; i++)
                        {
                            ssWrite_Sheet1.Cells[0, i].Text = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, i + 2].Text.Trim();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ssWrite_Sheet1.RowCount; i++)
                        {
                            ssWrite_Sheet1.Cells[i, 0].Text = ssView_Sheet1.Cells[i + 2, ssView_Sheet1.ActiveColumnIndex].Text.Trim();
                            ssWrite_Sheet1.Rows[i].Visible = true;
                        }
                    }
                }
            }
        }

        private void FrmEmrChartFlowOld_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fEmrInterface != null)
            {
                fEmrInterface.Dispose();
                fEmrInterface = null;
            }

            if (frmImgRmk != null)
            {
                frmImgRmk.Dispose();
                frmImgRmk = null;
            }

            if (fEmrChartNew != null)
            {
                fEmrChartNew.Dispose();
                fEmrChartNew = null;
            }

            if (frmEmrNewHisViewX != null)
            {
                frmEmrNewHisViewX.Dispose();
                frmEmrNewHisViewX = null;
            }
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssView_Sheet1.Rows.Count == 0) return;
            if (e.ColumnHeader == true) return;



            if (mFLOWGB.Equals("COL"))
            {
                if (ssView.ActiveSheet.RowCount == 0)
                    return;

                mstrEmrNo = ssView_Sheet1.Cells[e.Row, ssView_Sheet1.ColumnCount - 2].Text.Trim();
                mEmrUseId = ssView_Sheet1.Cells[e.Row, ssView_Sheet1.ColumnCount - 1].Text.Trim();
            }
            else
            {
                if (ssView.ActiveSheet.ColumnCount == 0)
                    return;

                mstrEmrNo = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, e.Column].Text.Trim();
                mEmrUseId = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, e.Column].Text.Trim();
            }

            if (clsType.User.BuseCode.Equals("044201") && clsType.User.AuAMANAGE.Equals("1"))
            {
                if (pForm.FmFORMNO != 963)
                    return;

                if (fEmrChartNew != null)
                {
                    fEmrChartNew.Dispose();
                    fEmrChartNew = null;
                }

                string GBN = ssView_Sheet1.Cells[e.Row, 2].Tag.ToString();
                Screen screen = Screen.FromControl(this);
                fEmrChartNew = new frmEmrChartNew(pForm.FmFORMNO.ToString(), GBN.Equals("OLD") ? "1" :  pForm.FmUPDATENO.ToString(), pAcp, mstrEmrNo, "V");
                fEmrChartNew.FormClosed += fEmrChartNew_FormClosed;
                fEmrChartNew.Text = pForm.FmFORMNAME;
                fEmrChartNew.StartPosition = FormStartPosition.Manual;
                fEmrChartNew.Location = new Point(screen.WorkingArea.Right - fEmrChartNew.Width - 80, 220);
                fEmrChartNew.Show(this);
            }
            else if (clsType.User.BuseCode.Equals("078201") && (pForm.FmFORMNO == 3150 || pForm.FmFORMNO == 1572 || pForm.FmFORMNO == 1573 || pForm.FmFORMNO == 1575 || pForm.FmFORMNO == 1725))
            {
                if (mFLOWGB.Equals("COL") && e.Column == ssView_Sheet1.ColumnCount - 4)
                {
                    ComFunc.MsgBoxEx(this, clsVbfunc.GetSaBunBuSeName(clsDB.DbCon, mEmrUseId));
                }
                else if(mFLOWGB.Equals("ROW") && e.Row == ssView_Sheet1.RowCount - 4)
                {
                    ComFunc.MsgBoxEx(this, clsVbfunc.GetSaBunBuSeName(clsDB.DbCon, mEmrUseId));
                }
            }
            else if (mstrMode.Equals("W"))
            {
                ssWrite_Sheet1.Cells[0, 0, ssWrite_Sheet1.RowCount - 1, ssWrite_Sheet1.ColumnCount - 1].Text = "";

                if (mstrMode.Equals("W"))
                {
                    if(mFLOWGB.Equals("COL"))
                    {
                        usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ssView_Sheet1.Cells[e.Row, 0].Text.Trim());
                        usFormTopMenuEvent.txtMedFrTime.Text = ssView_Sheet1.Cells[e.Row, 1].Text.Trim();
                    }
                    else
                    {
                        usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ssView_Sheet1.Cells[0, e.Column].Text.Trim());
                        usFormTopMenuEvent.txtMedFrTime.Text = ssView_Sheet1.Cells[1, e.Column].Text.Trim();
                    }

                    usFormTopMenuEvent.dtMedFrDate.Enabled = false;
                }
           
                if (mFLOWGB.Equals("COL"))
                {
                    for (int i = 0; i < mFormFlowSheet.Length; i++)
                    {
                        ssWrite_Sheet1.Cells[0, i].Text = ssView_Sheet1.Cells[e.Row, i + 2].Text.Trim();
                    }
                }
                else
                {
                    for (int i = 0; i < mFormFlowSheet.Length; i++)
                    {
                        if (pForm.FmFORMNO == 1573 && pForm.FmOLDGB == 0)
                        {
                            ComboSelChange(i);
                        }

                        ssWrite_Sheet1.Cells[i, 0].Text = ssView_Sheet1.Cells[i + 2, e.Column].Text.Trim();

                        ssWrite_Sheet1.Rows[i].Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// 창 닫힐때 이벤트 체크.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fEmrChartNew_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrChartNew == null)
                return;

            fEmrChartNew.Dispose();
            fEmrChartNew = null;
        }

        private void frmEmrInsulinImgX_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrInsulinImgX == null)
                return;

            frmEmrInsulinImgX.Dispose();
            frmEmrInsulinImgX = null;
        }

        private void frmEmrInsulinImgX_rSendSelect(string Val)
        {
            if (frmEmrInsulinImgX != null)
            {
                frmEmrInsulinImgX.Dispose();
                frmEmrInsulinImgX = null;
            }

            if (string.IsNullOrWhiteSpace(Val))
                return;

            ssWrite_Sheet1.Cells[0, 1].Text = Val;
        }

        private void ssWrite_EditModeOff(object sender, EventArgs e)
        {
            #region 유지Bundle 작성용 유지일일경우
            if (pForm.FmFORMNO == 2641 && mstrMode.Equals("W") && ssWrite_Sheet1.ActiveRowIndex == 1 && (
                ssWrite_Sheet1.ActiveCell.Text.ToUpper().Equals("REMOVE") ||
                ssWrite_Sheet1.ActiveCell.Text.ToUpper().Equals("제거")))

            {
                for (int i = 2; i < ssWrite_Sheet1.RowCount; i++)
                {
                    ssWrite_Sheet1.Cells[i, 0].Text = "";
                }
            }
            #endregion

            if (mFLOWGB.Equals("COL") == false)
                return;

            if (ssWrite_Sheet1.ActiveColumnIndex < ssWrite_Sheet1.ColumnCount)
            {
                ssWrite_Sheet1.ActiveColumnIndex += 1;
            }

            if (ssWrite_Sheet1.Columns[ssWrite_Sheet1.ActiveColumnIndex].CellType != null &&
                ssWrite_Sheet1.Columns[ssWrite_Sheet1.ActiveColumnIndex].CellType.ToString().Equals("TextCellType"))
            {
                if (ssWrite_Sheet1.Rows[0].Height > ComNum.SPDROWHT)
                {
                    ssWrite_Sheet1.Rows[0].Height = ssWrite_Sheet1.Rows[0].GetPreferredHeight() > ssWrite_Sheet1.Rows[0].Height ? ssWrite_Sheet1.Rows[0].GetPreferredHeight() + 5 : ssWrite_Sheet1.Rows[0].Height;
                }
                else
                {
                    ssWrite_Sheet1.Rows[0].Height = 22;
                }
            }
        }

        /// <summary>
        /// 인공신장실 V/S Sheet 가져오기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchData_Click(object sender, EventArgs e)
        {
            using (frmHemodialysisInterface frm = new frmHemodialysisInterface("VITAL", pAcp.ptNo))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.rSendInterface += Frm_rSendInterface;
                frm.ShowDialog(this);
            }
        }

        private void Frm_rSendInterface(string strData)
        {
            string[] str = strData.Split('|');

            if (string.IsNullOrWhiteSpace(str[0]) == false)
            {
                usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(str[0]);
                usFormTopMenuEvent.txtMedFrTime.Text = str[1];
            }

            ssWrite_Sheet1.Cells[0, 15].Text = str[2];   // '혈압
            ssWrite_Sheet1.Cells[0, 16].Text = str[3];   //'혈압
            ssWrite_Sheet1.Cells[0, 18].Text = str[4];   //'맥박
            ssWrite_Sheet1.Cells[0, 23].Text = str[5];   //'체중
            ssWrite_Sheet1.Cells[0, 24].Text = str[6];   //'신장
            ssWrite_Sheet1.Cells[0, 25].Text = str[7];   //'동맥압
            ssWrite_Sheet1.Cells[0, 26].Text = str[8];   //'정맥압
            ssWrite_Sheet1.Cells[0, 27].Text = str[9];   //'초여과율
            ssWrite_Sheet1.Cells[0, 28].Text = str[10];   //'막통과압
        }

        private void ssWrite_Change(object sender, ChangeEventArgs e)
        {
            switch(mstrFormNo)
            {
                case "1573":

                    ComboSelChange(e.Row+1);
                    break;
                //회복실 섭취기록
                case "2136":
                    
                    break;
                //완화의료 전처치 및 진정약제 투약기록지
                case "2430":
                    if (e.Column != 0)
                        return;

                    ListBox listBox = new ListBox();
                    ComboBoxCellType comboBoxCellType = new ComboBoxCellType();

                    switch(ssWrite_Sheet1.Cells[0, 0].Text.Trim())
                    {
                        case "A-POL12G":
                            listBox.Items.Add("120mg");
                            listBox.Items.Add("60mg");
                            listBox.Items.Add("40mg");
                            break;
                        case "A-BASCA":
                            listBox.Items.Add("5mg");
                            listBox.Items.Add("4mg");
                            break;
                        case "N-PTD25":
                            listBox.Items.Add("25mg");
                            break;
                        case "ALGIRO":
                            listBox.Items.Add("5mg");
                            break;
                        case "5DWA":
                        case "NSC":
                            listBox.Items.Add("500ml");
                            break;
                    }

                    comboBoxCellType.ListControl = listBox;
                    ssWrite_Sheet1.Cells[0, 1].CellType = comboBoxCellType;
                    break;
            }
        }

        /// <summary>
        /// 심사팀용 줄이기/ 늘리기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpDown_Click(object sender, EventArgs e)
        {
            //ICU 기본간호활동2
            if (mstrFormNo.Equals("1976"))
            {
                btnUpDown.Text = btnUpDown.Text.Equals("줄이기") ? "늘리기" : "줄이기";
                ssView_Sheet1.Rows[3, 18].Visible = btnUpDown.Text.Equals("줄이기");
            }
            else
            {
                //상처 욕창
                List<string> lstCode = new List<string>();
                if (pForm.FmFORMNO == 1573) //욕창
                {
                    lstCode.Add("I0000034115"); //위치
                    lstCode.Add("I0000034116"); //단계
                    lstCode.Add("I0000004518"); //Dressing
                }
                else if (pForm.FmFORMNO == 1725) //상처
                {
                    lstCode.Add("I0000024733"); //구분
                    lstCode.Add("I0000034121"); //부위
                    lstCode.Add("I0000030799"); //배액관종류
                    lstCode.Add("I0000031440"); //Dressing종류
                    lstCode.Add("I0000001311"); //비고
                }

                //03555645 9월 재원중

            

                if (btnUpDown.Text.Equals("늘리기"))
                {
                    if (mFLOWGB.Equals("COL"))
                    {
                        ssView_Sheet1.Columns[2, ssView_Sheet1.ColumnCount - 5].Visible = true;
                    }
                    else
                    {
                        ssView_Sheet1.Rows[2, ssView_Sheet1.RowCount - 5].Visible = true;
                    }
                    btnUpDown.Text = "줄이기";
                    return;
                }
                else
                {
                    if (mFLOWGB.Equals("COL"))
                    {
                        ssView_Sheet1.Columns[2, ssView_Sheet1.ColumnCount - 5].Visible = false;
                    }
                    else
                    {
                        ssView_Sheet1.Rows[2, ssView_Sheet1.RowCount - 5].Visible = false;
                    }

                    foreach (string Code in lstCode)
                    {
                        List<FormFlowSheet> flowSheets = mFormFlowSheet.Where(d => d.ItemCode.IndexOf(Code) != -1).ToList();
                        foreach (FormFlowSheet formFlow in flowSheets)
                        {
                            if (mFLOWGB.Equals("COL"))
                            {
                                ssView_Sheet1.Columns[formFlow.ItemNumber + 2].Visible = true;
                            }
                            else
                            {
                                ssView_Sheet1.Rows[formFlow.ItemNumber + 2].Visible = true;
                            }
                        }
                    }
                    btnUpDown.Text = "늘리기";
                }
            }
        }

        private void chkAsc_CheckedChanged(object sender, EventArgs e)
        {
            clsEmrPublic.bOrderAsc = chkAsc.Checked;
        }

        private void btnSearchGBN_Click(object sender, EventArgs e)
        {
            if (pAcp == null)
                return;

            StringBuilder GBN = new StringBuilder();
            StringBuilder GBN2 = new StringBuilder();

            ssWrite_Sheet1.RowCount = mFormFlowSheet.Length;

            #region 필터
            if (ssChk1.Sheets[0].RowFilter.GetColumnFilterBy(0) != null)
            {
                IRowFilter rf = ssChk1.ActiveSheet.RowFilter;
                foreach (FilterColumnDefinition fcd in rf.ColumnDefinitions)
                {
                    FilterItemCollection filters = fcd.Filters;
                    IFilterItem iFilterItem = fcd.Filters.Cast<IFilterItem>().Where(d => d.DisplayName.Equals(ssChk1.Sheets[0].RowFilter.GetColumnFilterBy(0))).FirstOrDefault();

                    if (iFilterItem is MultiValuesFilterItem)
                    {
                        MultiValuesFilterItem mvFilterItem = iFilterItem as MultiValuesFilterItem;
                        if (mvFilterItem != null)
                        {
                            string MaxItemValue = mvFilterItem.FilterItems.Last().Value.ToString();
                            foreach (FilterItemValue fi in mvFilterItem.FilterItems)
                            {
                                GBN.Append("'" + fi.Value.ToString() + "'" + (fi.Value.ToString().Equals(MaxItemValue) ? "" : ","));
                            }
                        }
                    }
                }
            }
            else
            {
                for(int i = 0; i < ssChk1_Sheet1.RowCount; i++)
                {
                    GBN.Append("'" + ssChk1_Sheet1.Cells[i, 0].Text.Trim() + "'" + (i == ssChk1_Sheet1.RowCount - 1 ? "" : ","));
                }
            }

            #endregion

            #region 필터2
            if (ssChk2.Visible == true)
            {
                if (ssChk2.Sheets[0].RowFilter.GetColumnFilterBy(0) != null)
                {
                    IRowFilter rf = ssChk2.ActiveSheet.RowFilter;
                    foreach (FilterColumnDefinition fcd in rf.ColumnDefinitions)
                    {
                        FilterItemCollection filters = fcd.Filters;
                        IFilterItem iFilterItem = fcd.Filters.Cast<IFilterItem>().Where(d => d.DisplayName.Equals(ssChk2.Sheets[0].RowFilter.GetColumnFilterBy(0))).FirstOrDefault();

                        if (iFilterItem is MultiValuesFilterItem)
                        {
                            MultiValuesFilterItem mvFilterItem = iFilterItem as MultiValuesFilterItem;
                            if (mvFilterItem != null)
                            {
                                string MaxItemValue = mvFilterItem.FilterItems.Last().Value.ToString();
                                foreach (FilterItemValue fi in mvFilterItem.FilterItems)
                                {
                                    GBN2.Append("'" + fi.Value.ToString() + "'" + (fi.Value.ToString().Equals(MaxItemValue) ? "" : ","));
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ssChk2_Sheet1.RowCount; i++)
                    {
                        GBN2.Append("'" + ssChk2_Sheet1.Cells[i, 0].Text.Trim() + "'" + (i == ssChk2_Sheet1.RowCount - 1 ? "" : ","));
                    }
                }
            }
            #endregion


            if (FlowData != null)
            {
                FlowData.Dispose();
                FlowData = null;
            }

            DataTable mFlowData = clsEmrQuery.QuerySpdLastList(clsDB.DbCon, pAcp, pForm, GBN.ToString(), GBN2.ToString(), chkAsc.Checked);

            if (mFlowData == null)
                return;

            bool mViewNpChart = clsEmrQueryOld.ViewNPChart(clsType.User.Sabun);
            FlowData = clsEmrQuery.QuerySpdList(clsDB.DbCon, pAcp, pForm,
                pAcp.medFrDate, dtpEndDate.Value.ToString("yyyyMMdd"),
                mstrVal, GBN.ToString().Trim(), GBN2.ToString().Trim(), mViewNpChart, chkAsc.Checked);

            if (FlowData == null)
            {
                ComFunc.MsgBoxEx(this, "조회중 오류가 발생했습니다.");
                return;
            }

            ssWrite_Sheet1.RowCount = mFormFlowSheet.Length;

            int intEmrCnt = (from DataRow dr in FlowData.Rows.AsParallel() group dr by dr["EMRNO"]).Count();
            int intEmrCnt2 = (from DataRow dr in mFlowData.Rows.AsParallel() group dr by dr["EMRNO"]).Count();
            int intDateCnt = (from DataRow dr in FlowData.Rows.AsParallel() group dr by dr["CHARTDATE"]).Count();
            lblCount.Text = string.Format("일수:{0}({1})", intDateCnt, intEmrCnt);

            if (ssWrite_Sheet1.RowCount < mFormFlowSheet.Length * intEmrCnt2)
            {
                for (int i = 1; i < intEmrCnt2; i++)
                {
                    btnAdd.PerformClick();
                }
            }

            ChartEmrNo = string.Empty;
            if (mFLOWGB.Equals("ROW"))
            {
                ssView.ActiveSheet.ColumnCount = 0;
                if (bCopy)
                {
                    ssView.ActiveSheet.Rows[ssView.ActiveSheet.RowCount - 3].Visible = false;
                }
            }
            else
            {
                ssView.ActiveSheet.RowCount = 0;
                if (bCopy)
                {
                    ssView.ActiveSheet.Columns[ssView.ActiveSheet.ColumnCount - 3].Visible = false;
                }
            }

            #region 
            SetFlowViewNurChart(mFlowData);
            SetFlowViewDataBind();
            #endregion

            mFlowData.Dispose();
            SetScoreVisible();
        }

        private void SetFlowViewNurChart(DataTable mFlowData)
        {
            if (mFlowData == null)
                return;

            string BradenScale = string.Empty;

            if (pForm.FmFORMNO == 1573)
            {
                GetDcur();
                BradenScale = ssWrite_Sheet1.Cells[0, 0].Text.Trim();
            }

            for (int i = 0; i < mFlowData.Rows.Count; i++)
            {
                if (pForm.FmFORMNO == 1573 && mFlowData.Rows[i]["SEQNO"].ToString().Equals("0"))
                {
                    ssWrite_Sheet1.Cells[i, 0].Text = BradenScale;
                }
                else
                {
                    ssWrite_Sheet1.Cells[i, 0].Text = mFlowData.Rows[i]["ITEMVALUE"].ToString().Trim();
                }
            }
        }

        private void ssWrite_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssWrite_Sheet1.RowCount == 0)
                return;

            #region ONCOLOGY SOLID TUMOR FORM
            if (pForm.FmFORMNO == 3552 && pForm.FmUPDATENO == 1 && e.RowHeader &&
                ((e.Row >= 1 && e.Row <= 10) || e.Row == 48))
            {
                ssWrite_Sheet1.RowHeader.Cells[e.Row, 1].Text = VB.InputBox("약 명칭을 입력해주세요.", "입력");
            }
            #endregion

            #region 당뇨 기록지 피하주사순서 폼 뜨게
            if (pForm.FmFORMNO == 1572 && pForm.FmOLDGB != 1 && e.Column == 1)
            {
                if (frmEmrInsulinImgX != null)
                {
                    frmEmrInsulinImgX.Dispose();
                    frmEmrInsulinImgX = null;
                }

                Screen screen = Screen.FromControl(this);
                frmEmrInsulinImgX = new frmEmrInsulinImg();
                frmEmrInsulinImgX.FormClosed += frmEmrInsulinImgX_FormClosed;
                frmEmrInsulinImgX.rSendSelect += frmEmrInsulinImgX_rSendSelect;
                frmEmrInsulinImgX.Text = pForm.FmFORMNAME;
                frmEmrInsulinImgX.StartPosition = FormStartPosition.Manual;
                frmEmrInsulinImgX.Location = new Point(screen.WorkingArea.Right - frmEmrInsulinImgX.Width - 80, 50);
                frmEmrInsulinImgX.Show(this);
            }
            #endregion
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            #region 추가
            int StartRow = ssWrite_Sheet1.RowCount;
            ssWrite_Sheet1.RowCount += mFormFlowSheet.Length;
            ssWrite_Sheet1.AddRowHeaderSpanCell(StartRow, 0, mFormFlowSheet.Length, 1);
            ssWrite_Sheet1.RowHeader.Cells[StartRow, 0, StartRow + mFormFlowSheet.Length - 1, 0].Text = (ssWrite_Sheet1.RowCount / mFormFlowSheet.Length) + "";

            for (int i = 0; i < mFormFlowSheet.Length; i++)
            {
                ssWrite_Sheet1.Cells[StartRow + mFormFlowSheet[i].ItemNumber, 0].Locked = false;
                ssWrite_Sheet1.Rows.Get(StartRow + mFormFlowSheet[i].ItemNumber).Label = mFormFlowSheet[i].ItemName;
                ssWrite_Sheet1.Cells[StartRow + mFormFlowSheet[i].ItemNumber, 0].CellType = DesignFunc.CellType(mFormFlowSheet[i].CellType, mFormFlowSheet[i].MultiLine,
                    mFormFlowSheet[i].CheckTextAlignment, mFormFlowSheet[i].UserMcro);

                if (mFormFlowSheet[i].CellType.Equals("ComboBoxCellType") && mFormFlowSheet[i].UserMcro.Trim().NotEmpty())
                {
                    string[] arryUserMcro = mFormFlowSheet[i].UserMcro.Trim().Split('^');
                    clsSpread.gSpreadComboDataSetEx1(ssWrite, StartRow + mFormFlowSheet[i].ItemNumber, 0, StartRow + mFormFlowSheet[i].ItemNumber, 0, arryUserMcro, true);
                    if (arryUserMcro.Length > 0)
                    {
                        ssWrite_Sheet1.Cells[StartRow + mFormFlowSheet[i].ItemNumber, 0].Text = arryUserMcro[0];
                    }
                }
            }
            #endregion

            #region 이전
            //List<FormFlowSheet> formFlowse = mFormFlowSheet.Where(d => d.ItemCode.IndexOf("_" + (intMaxIdx + 1)) != -1).OrderBy(d => d.ItemNumber).ToList();
            //if (formFlowse.Count > 0)
            //{
            //    intMaxIdx++;
            //    ssWrite_Sheet1.Rows[formFlowse[0].ItemNumber, formFlowse[formFlowse.Count - 1].ItemNumber].Visible = true;
            //}
            #endregion

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            #region 삭제
            if (ssWrite_Sheet1.ActiveRowIndex <= mFormFlowSheet.Length - 1)
                return;

            for (int i = 0; i < ssWrite_Sheet1.RowCount; i++)
            {
                if (ssWrite_Sheet1.RowHeader.Cells[i, 0].Text.Equals(ssWrite_Sheet1.RowHeader.Cells[ssWrite_Sheet1.ActiveRowIndex, 0].Text))
                {
                    ssWrite_Sheet1.Rows.Remove(i, mFormFlowSheet.Length);
                    break;
                }
            }
            #endregion

            #region 이전 로직
            //List<FormFlowSheet> formFlowse = mFormFlowSheet.Where(d => d.ItemCode.IndexOf("_" + (intMaxIdx)) != -1).OrderBy(d => d.ItemNumber).ToList();
            //if (formFlowse.Count > 0)
            //{
            //    intMaxIdx--;
            //    ssWrite_Sheet1.Rows[formFlowse[0].ItemNumber, formFlowse[formFlowse.Count - 1].ItemNumber].Visible = false;
            //}
            #endregion
        }

        private void ssWrite_ComboSelChange(object sender, EditorNotifyEventArgs e)
        {
            ComboSelChange(e.Row);
        }

        private void ComboSelChange(int Row)
        {
            if (pForm.FmFORMNO == 1573 && pForm.FmOLDGB != 1)
            {
                #region 이전 
                //if (clsDB.DbCon.strSource.Equals("ORA7"))
                //{
                //    //몇번째 인덱스인지 찾고
                //    FormFlowSheet formFlow = mFormFlowSheet.Where(d => d.ItemNumber == Row).FirstOrDefault();
                //    //해당 리스트만 뽑음.
                //    List<FormFlowSheet> formFlowse = mFormFlowSheet.Where(d => d.ItemCode.IndexOf("_" + formFlow.ItemCode.Split('_')[1]) != -1).ToList();

                //    //삼출물의양일때
                //    if (formFlowse != null && formFlow.ItemCode.IndexOf("I0000037213") != -1)
                //    {
                //        FormFlowSheet flowSheet = formFlowse.Find(d => d.ItemCode.IndexOf("I0000037211") != -1);

                //        string[] strSize = ssWrite_Sheet1.Cells[flowSheet.ItemNumber, 0].Text.Trim().Replace("x", "*").Replace("X", "*").Split('*');
                //        if (strSize.Length > 0)
                //        {
                //            strSize[0] = Regex.Replace(strSize[0], @"[^0-9]", "");

                //            if (strSize.Length > 1)
                //            {
                //                if (Regex.IsMatch(strSize[1], "[^0-9]"))
                //                {
                //                    ComFunc.MsgBoxEx(this, "욕창의 크기는 1*1 혹은 1x1 형태로만 입력해주세요.");
                //                    return;
                //                }

                //                strSize[1] = Regex.Replace(strSize[1], @"[^0-9]", "");
                //            }
                //        }

                //        //상처크기 x => * 변환 후 곱함.
                //        double lngValue = strSize.Length > 1 ? VB.Val(strSize[0]) * VB.Val(strSize[1]) : VB.Val(strSize[0]);

                //        #region 점수 하드코딩
                //        if (lngValue > 24)
                //            lngValue = 10;
                //        else if (lngValue <= 24 && lngValue >= 12.1)
                //            lngValue = 9;
                //        else if (lngValue <= 12 && lngValue >= 8.1)
                //            lngValue = 8;
                //        else if (lngValue <= 8 && lngValue >= 4.1)
                //            lngValue = 7;
                //        else if (lngValue <= 4 && lngValue >= 3.1)
                //            lngValue = 6;
                //        else if (lngValue <= 3 && lngValue >= 2.1)
                //            lngValue = 5;
                //        else if (lngValue <= 2 && lngValue >= 1.1)
                //            lngValue = 4;
                //        else if (lngValue <= 1 && lngValue >= 0.7)
                //            lngValue = 3;
                //        else if (lngValue <= 0.6 && lngValue >= 0.3)
                //            lngValue = 2;
                //        else if (lngValue < 0.3 && lngValue > 0)
                //            lngValue = 1;
                //        else if (lngValue == 0)
                //            lngValue = 0;


                //        //조직의종류
                //        //I0000037212_1
                //        flowSheet = formFlowse.Find(d => d.ItemCode.IndexOf("I0000037212") != -1);
                //        switch (ssWrite_Sheet1.Cells[flowSheet.ItemNumber, 0].Text.Trim())
                //        {
                //            case "가피":
                //                lngValue += 4;
                //                break;
                //            case "부육":
                //                lngValue += 3;
                //                break;
                //            case "육아조직":
                //                lngValue += 2;
                //                break;
                //            case "상피세포":
                //                lngValue += 1;
                //                break;
                //        }

                //        //I0000037213_1
                //        flowSheet = formFlowse.Find(d => d.ItemCode.IndexOf("I0000037213") != -1);
                //        switch (ssWrite_Sheet1.Cells[flowSheet.ItemNumber, 0].Text.Trim())
                //        {
                //            case "소량":
                //                lngValue += 1;
                //                break;
                //            case "중정도":
                //                lngValue += 2;
                //                break;
                //            case "다량":
                //                lngValue += 3;
                //                break;
                //        }

                //        flowSheet = formFlowse.Find(d => d.ItemCode.IndexOf("I0000037216") != -1);
                //        ssWrite_Sheet1.Cells[flowSheet.ItemNumber, 0].Text = lngValue.ToString();
                //        #endregion

                //    }
                //}
                #endregion
                #region 신규
                ////몇번째 인덱스인지 찾고
                FormFlowSheet formFlow = mFormFlowSheet.Where(d => d.ItemName.IndexOf(ssWrite_Sheet1.RowHeader.Cells[Row, 1].Text.Trim()) != -1).FirstOrDefault();
                //해당 리스트만 뽑음.
                List<FormFlowSheet> formFlowse = mFormFlowSheet.ToList();

                if (formFlowse != null && formFlow != null)
                {
                    FormFlowSheet flowSheet = formFlowse.Find(d => d.ItemCode.IndexOf("I0000037211") != -1);

                    string[] strSize = ssWrite_Sheet1.Cells[Row - formFlow.ItemNumber + flowSheet.ItemNumber, 0].Text.Trim().Replace("x", "*").Replace("X", "*").Split('*');
                    if (strSize.Length > 0)
                    {
                        strSize[0] = Regex.Replace(strSize[0], @"[^0-9]", "");

                        if (strSize.Length > 1)
                        {
                            if (Regex.IsMatch(strSize[1], "[^0-9]"))
                            {
                                ComFunc.MsgBoxEx(this, "욕창의 크기는 1*1 혹은 1x1 형태로만 입력해주세요.");
                                return;
                            }

                            strSize[1] = Regex.Replace(strSize[1], @"[^0-9]", "");
                        }
                    }

                    //상처크기 x => * 변환 후 곱함.
                    double lngValue = strSize.Length > 1 ? VB.Val(strSize[0]) * VB.Val(strSize[1]) : VB.Val(strSize[0]);

                    #region 점수 하드코딩
                    if (lngValue > 24)
                        lngValue = 10;
                    else if (lngValue <= 24 && lngValue >= 12.1)
                        lngValue = 9;
                    else if (lngValue <= 12 && lngValue >= 8.1)
                        lngValue = 8;
                    else if (lngValue <= 8 && lngValue >= 4.1)
                        lngValue = 7;
                    else if (lngValue <= 4 && lngValue >= 3.1)
                        lngValue = 6;
                    else if (lngValue <= 3 && lngValue >= 2.1)
                        lngValue = 5;
                    else if (lngValue <= 2 && lngValue >= 1.1)
                        lngValue = 4;
                    else if (lngValue <= 1 && lngValue >= 0.7)
                        lngValue = 3;
                    else if (lngValue <= 0.6 && lngValue >= 0.3)
                        lngValue = 2;
                    else if (lngValue < 0.3 && lngValue > 0)
                        lngValue = 1;
                    else if (lngValue == 0)
                        lngValue = 0;


                    //조직의종류
                    //I0000037212_1
                    flowSheet = formFlowse.Find(d => d.ItemCode.IndexOf("I0000037212") != -1);
                    ComboBoxCellType cbo = (ComboBoxCellType) ssWrite_Sheet1.Cells[Row - formFlow.ItemNumber + flowSheet.ItemNumber, 0].CellType;

                    string ChkText = cbo.ListControl.SelectedItem.NotEmpty() ? cbo.ListControl.SelectedItem.ToString().Trim() : cbo.ListControl.Text.Trim();
                    switch (ChkText)
                    {
                        case "가피":
                            lngValue += 4;
                            break;
                        case "부육":
                            lngValue += 3;
                            break;
                        case "육아조직":
                            lngValue += 2;
                            break;
                        case "상피세포":
                            lngValue += 1;
                            break;
                    }

                    //I0000037213_1
                    flowSheet = formFlowse.Find(d => d.ItemCode.IndexOf("I0000037213") != -1);
                    cbo = (ComboBoxCellType)ssWrite_Sheet1.Cells[Row - formFlow.ItemNumber + flowSheet.ItemNumber, 0].CellType;
                    ChkText = cbo.ListControl.SelectedItem.NotEmpty() ? cbo.ListControl.SelectedItem.ToString().Trim() : cbo.ListControl.Text.Trim();

                    switch (ChkText)
                    {
                        case "소량":
                            lngValue += 1;
                            break;
                        case "중정도":
                            lngValue += 2;
                            break;
                        case "다량":
                            lngValue += 3;
                            break;
                    }

                    flowSheet = formFlowse.Find(d => d.ItemCode.IndexOf("I0000037216") != -1);
                    ssWrite_Sheet1.Cells[Row - formFlow.ItemNumber + flowSheet.ItemNumber, 0].Text = lngValue.ToString();
                    #endregion
                }
                #endregion
            }
        }

        #region 투약기록지 코드조회
        private void btnSearchCode_Click(object sender, EventArgs e)
        {
            if (pAcp == null)
                return;

            string SearchCode = VB.InputBox("검색하실 코드나 약품명을 입력하세요.", "입력");
            bool mViewNpChart = clsEmrQueryOld.ViewNPChart(clsType.User.Sabun);

            if (FlowData != null)
            {
                FlowData.Dispose();
                FlowData = null;
            }

            ssView_Sheet1.RowCount = 0;
       
            FlowData = clsEmrQuery.QuerySpdListTuYak(clsDB.DbCon, pAcp, pForm,
                dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                mstrVal, mViewNpChart, chkAsc.Checked, SearchCode);
          

            if (FlowData == null)
            {
                ComFunc.MsgBoxEx(this, "조회중 오류가 발생했습니다.");
                return;
            }

            int intEmrCnt = (from DataRow dr in FlowData.Rows.AsParallel() group dr by dr["EMRNO"]).Count();
            int intDateCnt = (from DataRow dr in FlowData.Rows.AsParallel() group dr by dr["CHARTDATE"]).Count();
            lblCount.Text = string.Format("일수:{0}({1})", intDateCnt, intEmrCnt);

            //19-09-24 의료정보팀 요청(간호기록만 장수 다를경우 색깔 빨간색으로)
            if (clsType.User.BuseCode.Equals("044201") && mstrFormNo.Equals("965") && intEmrCnt != VB.Val(strEmrCnt))
            {
                lblCount.ForeColor = Color.Red;
            }         

            ChartEmrNo = string.Empty;
            if (mFLOWGB.Equals("ROW"))
            {
                ssView.ActiveSheet.ColumnCount = 0;
                if (bCopy)
                {
                    ssView.ActiveSheet.Rows[ssView.ActiveSheet.RowCount - 3].Visible = false;
                }
            }
            else
            {
                ssView.ActiveSheet.RowCount = 0;
                if (bCopy)
                {
                    ssView.ActiveSheet.Columns[ssView.ActiveSheet.ColumnCount - 3].Visible = false;
                }
            }
          
             SetFlowViewDataBind();
        }
        #endregion

        private void btnSearchHis_Click(object sender, EventArgs e)
        {
            if (frmEmrNewHisViewX != null)
            {
                frmEmrNewHisViewX.Dispose();
                frmEmrNewHisViewX = null;
            }

            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            string strEmrNo = string.Empty;
            if (mFLOWGB.Equals("COL"))
            {
                strEmrNo = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, ssView_Sheet1.ColumnCount - 2].Text.Trim();
            }
            else
            {
                strEmrNo = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, ssView_Sheet1.ActiveColumnIndex].Text.Trim();
            }

            frmEmrNewHisViewX = new frmEmrNewHisView(pAcp, pForm, strEmrNo);
            frmEmrNewHisViewX.StartPosition = FormStartPosition.CenterScreen;
            frmEmrNewHisViewX.FormClosed += FrmEmrNewHisViewX_FormClosed;
            frmEmrNewHisViewX.Show(this);
        }

        private void FrmEmrNewHisViewX_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrNewHisViewX != null)
            {
                frmEmrNewHisViewX.Dispose();
                frmEmrNewHisViewX = null;
            }
        }

        private void btnWebRmk_Click(object sender, EventArgs e)
        {
            if (frmImgRmk != null)
            {
                frmImgRmk.Dispose();
                frmImgRmk = null;
            }

            object obj = Properties.Resources.ResourceManager.GetObject(pForm.FmFORMNO.ToString());
            if (obj == null)
            {
                return;
            }

            frmImgRmk = new frmRemarkImage(obj);
            frmImgRmk.StartPosition = FormStartPosition.CenterScreen;
            frmImgRmk.FormClosed += FrmImgRmk_FormClosed;
            frmImgRmk.Show();

            //string EmrUrlWebRmk = "http://192.168.100.33:8090/Emr/refImage.mts?formNo=";
            //Process.Start(@"C:\Program Files\Internet Explorer\iexplore.exe", EmrUrlWebRmk + pForm.FmFORMNO);
        }

        private void FrmImgRmk_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmImgRmk != null)
            {
                frmImgRmk.Dispose();
                frmImgRmk = null;
            }
        }

        private void btnAutoText_Click(object sender, EventArgs e)
        {
            using(frmAutoTextPT fAutoTextPT = new frmAutoTextPT(pForm))
            {
                fAutoTextPT.StartPosition = FormStartPosition.CenterScreen;
                fAutoTextPT.rSendData += FAutoTextPT_rSendData;
                fAutoTextPT.ShowDialog(this);
            }
        }

        private void FAutoTextPT_rSendData(string strData, Form ClosedForm)
        {
            if (ClosedForm != null)
            {
                ClosedForm.Dispose();
            }
            ssWrite_Sheet1.Cells[ssWrite_Sheet1.ActiveRowIndex, ssWrite_Sheet1.ActiveColumnIndex].Text = strData;
        }

        private void ssView_EditModeOff(object sender, EventArgs e)
        {
        }

        #region 환자모니터 인터페이스 연동
        private void btnInterface_Click(object sender, EventArgs e)
        {
            DateTime dtp = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();

            fEmrInterface = new frmPatientMonitorInterface(pAcp, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"), dtp.ToString("HH:mm"));
            fEmrInterface.FormClosed += fEmrHemodialysisInterface_FormClosed;
            (fEmrInterface as frmPatientMonitorInterface).rSendInterface += FrmEmr_rSendInterface;
            fEmrInterface.Show(this);
        }

        private void fEmrHemodialysisInterface_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fEmrInterface != null)
            {
                fEmrInterface.Dispose();
                fEmrInterface = null;
            }
        }

        private void FrmEmr_rSendInterface(Dictionary<string, string> strData)
        {
            if (strData.Count == 0)
                return;

            foreach(KeyValuePair<string, string> keyValue in strData)
            {
                FormFlowSheet flowSheet = mFormFlowSheet.Where(d => d.ItemCode.Equals(keyValue.Key)).FirstOrDefault();
                if (flowSheet == null)
                    continue;

                ssWrite_Sheet1.Cells[0, flowSheet.ItemNumber].Text = keyValue.Value;
            }


        }
        #endregion

        private void btnRmk2_Click(object sender, EventArgs e)
        {
            if (frmImgRmk != null)
            {
                frmImgRmk.Dispose();
                frmImgRmk = null;
            }

            frmImgRmk = new frmRemarkImage(Properties.Resources.욕창분류_단계);
            frmImgRmk.StartPosition = FormStartPosition.CenterScreen;
            frmImgRmk.FormClosed += FrmImgRmk_FormClosed;
            frmImgRmk.Show();
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            string ItemCd = pForm.FmFORMNO == 1725 ? "상처간호" : "욕창간호";
            using (frmSugaOrderSave frmSugaOrderSaveX = new frmSugaOrderSave(usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyy-MM-dd"), pForm.FmFORMNO.ToString(), ItemCd, ItemCd, ItemCd, pAcp, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd"), null, -1))
            {
                frmSugaOrderSaveX.StartPosition = FormStartPosition.CenterScreen;
                frmSugaOrderSaveX.ShowDialog(this);
            }

            if (clsEmrQuery.ChartOrder_Exists(this, pAcp, usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyy-MM-dd"), ItemCd, ItemCd))
            {
                btnSaveOrder.BackColor = Color.Pink;
                btnSaveOrder.Text = "당일처방내역";
                dtpFrDate.Value = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();
                dtpEndDate.Value = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();
                GetSearchData();
            }
            else
            {
                btnSaveOrder.Text = "처방전송";
                btnSaveOrder.BackColor = Color.Gainsboro;
            }
        }

        private void btnGetExam_Click(object sender, EventArgs e)
        {
            //혈종기록지 아니면 작동안되게.
            if (pForm.FmFORMNO != 3552)
                return;

            Set_Exam_Result();
        }


        /// <summary>
        /// 의뢰 약물 등 표시.
        /// </summary>
        private void Set_Exam_Result()
        {
            StringBuilder INData = new StringBuilder();
            Dictionary<string, string> CodeList = new Dictionary<string, string>();
            CodeList.Add("WBC", "I0000002083");
            CodeList.Add("HB" , "I0000001822");
            CodeList.Add("PLT", "I0000003609");
            CodeList.Add("Seg#", "I0000016252");
            CodeList.Add("T.protein", "I0000038269");
            CodeList.Add("Albumin", "I0000038269");
            CodeList.Add("Glucose", "I0000009122");
            CodeList.Add("GOT", "I0000033617");
            CodeList.Add("GPT", "I0000033618");
            CodeList.Add("T.bilirubin", "I0000038270");
            CodeList.Add("Creatinine", "I0000003523");
            CodeList.Add("Ca", "I0000003456");
            CodeList.Add("Na", "I0000038200");
            CodeList.Add("K", "I0000038200");
            CodeList.Add("CL", "I0000038200");
            CodeList.Add("LDH", "I0000003447");
            CodeList.Add("CEA", "I0000003608");
            CodeList.Add("CA-19-9", "I0000003532");
            CodeList.Add("TEXT", "I0000030914");
            CodeList.Add("BUN", "I0000003493");

            int idx = 0;
            foreach (KeyValuePair<string, string> keyValue in CodeList)
            {
                idx++;
                if (idx > 1 && idx <= CodeList.Count)
                {
                    INData.Append("'");
                }

                INData.Append(keyValue.Key.ToUpper());

                if (idx < CodeList.Count)
                {
                    INData.Append("', ");
                }
            }

            MParameter mParameter = new MParameter();

            mParameter.AppendSql("WITH EXAM_MST AS                                                                                                                             ");
            mParameter.AppendSql("(                                                                                                                                             ");
            mParameter.AppendSql("  SELECT  MASTERCODE                                                                                                                         ");
            mParameter.AppendSql("  	,   TRIM(EXAMNAME) EXAMNAME                                                                                                                        ");
            mParameter.AppendSql("    FROM ADMIN.EXAM_MASTER A                                                                                                             ");
            mParameter.AppendSql("   WHERE TRIM(UPPER(EXAMNAME)) IN ('" + INData.ToString().Trim() + "')                                                                        ");
            mParameter.AppendSql(")                                                                                                                                             ");

            mParameter.AppendSql(",	EXAM_DATA AS                                                                                                                                ");
            mParameter.AppendSql("(                                                                                                                                             ");
            mParameter.AppendSql("  SELECT  A.BLOODDATE                                                                                                                         ");
            mParameter.AppendSql("  	,   C.RESULTDATE                                                                                                                        ");
            mParameter.AppendSql("  	,   TRIM(UPPER(D.EXAMNAME)) EXAMNAME                                                                                                           ");
            mParameter.AppendSql("  	,   C.SUBCODE                                                                                                                           ");
            mParameter.AppendSql("  	,   C.RESULT                                                                                                                            ");
            mParameter.AppendSql("  	, ROW_NUMBER() OVER(PARTITION BY C.SUBCODE ORDER BY C.RESULTDATE DESC) AS ROW_NUM                                                       ");
            mParameter.AppendSql("    FROM ADMIN.EXAM_SPECMST A                                                                                                            ");
            mParameter.AppendSql("   INNER JOIN ADMIN.EXAM_RESULTC C                                                                                                       ");
            mParameter.AppendSql("      ON A.SPECNO = C.SPECNO                                                                                                                  ");
            mParameter.AppendSql("     AND C.RESULT IS NOT NULL                                                                                                                 ");
            mParameter.AppendSql("   INNER JOIN EXAM_MST D                                                                                                                      ");
            mParameter.AppendSql("      ON C.SUBCODE = D.MASTERCODE                                                                                                             ");
            mParameter.AppendSql("   WHERE A.PANO = :PANO                                                                                                                       ");
            mParameter.AppendSql("     AND A.BDATE >= TO_DATE(:INDATE, 'YYYY-MM-DD')                                                                                            ");
            mParameter.AppendSql(")                                                                                                                                             ");

            mParameter.AppendSql("  SELECT  TO_CHAR(BLOODDATE,  'YYYY-MM-DD HH24:MI:SS') BLOODDATE                                                                              ");
            mParameter.AppendSql("  	,   TO_CHAR(RESULTDATE, 'YYYY-MM-DD HH24:MI:SS') RESULTDATE                                                                             ");
            mParameter.AppendSql("  	,   EXAMNAME                                                                                                                          ");
            mParameter.AppendSql("  	,   SUBCODE                                                                                                                             ");
            mParameter.AppendSql("  	,   RESULT                                                                                                                              ");
            mParameter.AppendSql("  	,   ROW_NUM                                                                                                                             ");
            mParameter.AppendSql("    FROM EXAM_DATA                                                                                                                            ");
            mParameter.AppendSql("   WHERE ROW_NUM = 1                                                                                                                            ");
            mParameter.AppendSql("   ORDER BY RESULTDATE DESC                                                                                                                   ");

            mParameter.Add("PANO", pAcp.ptNo, OracleDbType.Char);
            mParameter.Add("INDATE", DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd"));

            //mParameter.AddInStatement("SBCODE", CodeList.Keys.ToList(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            //mParameter.AddInStatement("EXAMNAME", CodeList.Keys.ToList(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            //mParameter.Add("EXAMNAME", CodeList.Keys.ToList(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            List<Dictionary<string, object>> dt = clsDB.ExecuteReader(mParameter, clsDB.DbCon);
            if (dt.Count == 0)
                return;

            List<Dictionary<string, object>> sub = null;
            FormFlowSheet item = null;

            foreach (KeyValuePair<string, string> keyValue in CodeList)
            {
                if (mFormFlowSheet.Any(d => d.ItemCode.Equals(keyValue.Value)))
                {
                    item = mFormFlowSheet.FirstOrDefault(d => d.ItemCode.Equals(keyValue.Value));
                    if (item == null)
                        continue;

                    sub = dt.Where(r => r.Any(f => f.Value.Trim().Equals(keyValue.Key.ToUpper()))).ToList();

                    if (sub == null || sub.Count == 0)
                        continue;
                   
                    ssWrite_Sheet1.Cells[item.ItemNumber, 0].Text = sub[0]["RESULT"].ToString();
                }
            }

            #region T.PROTEIN / ALBUMIN
            item = mFormFlowSheet.FirstOrDefault(d => d.ItemCode.Equals("I0000038269"));
            if (item != null)
            {
                sub = dt.Where(r => r.Any(f => f.Value.Trim().Equals("T.BILIRUBIN"))).ToList();
                if (sub.Count > 0)
                {
                    ssWrite_Sheet1.Cells[item.ItemNumber, 0].Text = sub[0]["RESULT"].ToString();
                }

                sub = dt.Where(r => r.Any(f => f.Value.Trim().Equals("ALBUMIN"))).ToList();
                if (sub.Count > 0)
                {
                    ssWrite_Sheet1.Cells[item.ItemNumber, 0].Text += "/" + sub[0]["RESULT"].ToString();
                }
            }
            #endregion

            #region NA/K/CI
            item = mFormFlowSheet.FirstOrDefault(d => d.ItemCode.Equals("I0000038200"));
            if (item != null)
            {
                sub = dt.Where(r => r.Any(f => f.Value.Trim().Equals("NA"))).ToList();
                if (sub.Count > 0)
                {
                    ssWrite_Sheet1.Cells[item.ItemNumber, 0].Text = sub[0]["RESULT"].ToString();
                }

                sub = dt.Where(r => r.Any(f => f.Value.Trim().Equals("K"))).ToList();
                if (sub.Count > 0)
                {
                    ssWrite_Sheet1.Cells[item.ItemNumber, 0].Text += "/" + sub[0]["RESULT"].ToString();
                }

                sub = dt.Where(r => r.Any(f => f.Value.Trim().Equals("CL"))).ToList();
                if (sub.Count > 0)
                {
                    ssWrite_Sheet1.Cells[item.ItemNumber, 0].Text += "/" + sub[0]["RESULT"].ToString();
                }
            }
            #endregion

            if (sub != null)
            {
                sub.Clear();
            }
        }
    }
}
