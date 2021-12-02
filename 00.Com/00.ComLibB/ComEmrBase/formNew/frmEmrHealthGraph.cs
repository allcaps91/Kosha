using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    public partial class frmEmrHealthGraph : Form, EmrChartForm
    {
        #region // 폼에 사용하는 변수를 코딩하는 부분 ==> 꼭 변경을 요합니다.
        private const string mDirection = "V";   //기록지 작성방향(H: 옆으로, V:아래로)
        private const bool mHeadVisible = true;   //해드를 보이게 할지 여부
        private const int mintHeadCol = 6;  //해드 칼럼 수(작성, 조회 공통)
        private const int mintHeadRow = 2;  //해드 줄 수 (조회시에)

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private Bitmap memoryImage;

        #endregion

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //int mCallFormGb = 0;  //차트작성 0, 기록지등록에서 호출 1,

        string mFLOWGB = "COL"; //서식작성 방향
        int mFLOWITEMCNT = 0;
        int mFLOWHEADCNT = 0;
        int mFLOWINPUTSIZE = 0;

        //int mRow = 0;
        //int mCol = 0;
        FormFlowSheet[] mFormFlowSheet = null;
        FormFlowSheetHead[,] mFormFlowSheetHead = null;
        /// <summary>
        /// EMR 기록지
        /// </summary>
        EmrForm pForm = null;

        public FormEmrMessage mEmrCallForm;
        public string mstrFormNo = string.Empty;
        public string mstrUpdateNo = "0";
        public string mstrFormText = string.Empty;
        public EmrPatient p = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "W";
        public string mstrDeptCode = string.Empty;
        public string mstrVal = string.Empty;
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
            pSaveData("1"); //인증저장
        }
        private void usFormTopMenuEvent_SetSaveTemp(string strFrDate, string strFrTime)
        {
            pSaveData("0"); //임시저장
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

        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = pSaveData(strFlag);
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

        public int PrintFormMsg(string strPRINTFLAG)
        {
            pPrintExcept();
            return 1;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            return 1;
        }
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
            usFormTopMenuEvent.rSetSaveTemp += new usFormTopMenu.SetSaveTemp(usFormTopMenuEvent_SetSaveTemp);
            usFormTopMenuEvent.rSetDel += new usFormTopMenu.SetDel(usFormTopMenuEvent_SetDel);
            usFormTopMenuEvent.rSetClear += new usFormTopMenu.SetClear(usFormTopMenuEvent_SetClear);
            usFormTopMenuEvent.rSetPrint += new usFormTopMenu.SetPrint(usFormTopMenuEvent_SetPrint);
            usFormTopMenuEvent.rEventClosed += new usFormTopMenu.EventClosed(usFormTopMenuEvent_EventClosed);

            this.Controls.Add(usFormTopMenuEvent);
            usFormTopMenuEvent.Parent = this.panTopMenu;
            usFormTopMenuEvent.Dock = DockStyle.Fill;
            //--------------------------
            pClearForm();
            SetFormInfo();
            pInitFormSpc();
            SetColRowCount();
            pSetEmrInfo();
        }

        /// <summary>
        /// 작성내역을 불러 온다
        /// </summary>
        private void pSetEmrInfo()
        {
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);
            usFormTopMenuEvent.mbtnPrint.Visible = false;

            mbtnSaveAll.Visible = clsType.User.AuAWRITE.Equals("1");
            mbtnSaveK.Visible = clsType.User.AuAWRITE.Equals("1");
            mbtnPrint.Visible = clsType.User.AuAPRINTIN.Equals("1");

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
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double pSaveData(string strFlag)
        {
            if (VB.Val(mstrEmrNo) != 0)
            {
                if (clsEmrQuery.IsChangeAuth(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false) return 0;
            }

            double dblEmrNo = 0;
            if (VB.Val(mstrEmrNo) != 0)
            {
                if (ComFunc.MsgBoxQEx(this, "기존 내용을 변경하시겠습니까?", "EMR", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                {
                    return dblEmrNo;
                }
            }

            dblEmrNo = pSaveEmrData(strFlag);
            if (dblEmrNo == 0)
            {
                ComFunc.MsgBoxEx(this, "저장중 에러가 발생했습니다.");
            }
            else
            {
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                mstrEmrNo = Convert.ToString(dblEmrNo);
                pSetEmrInfo();
                mEmrCallForm.MsgSave(strFlag);
            }
            return dblEmrNo;
        }

        #endregion

        #region // 기록지 클리어, 저장, 삭제, 프린터
        /// <summary>
        /// 화면 정리
        /// </summary>
        public void pClearForm()
        {

            //시간 세팅을 한다.
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            usFormTopMenuEvent.txtMedFrTime.Text = ComFunc.FormatStrToDate(VB.Mid(strCurDateTime, 9, 4), "M");
            usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            //자격에따라서 버튼을 설정을 한다.
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);
            //if (clsType.User.AuAWRITE == "1")
            //{
            //    panWrite.Visible = true;
            //    clsEmrFunc.usBtnShow(usFormTopMenuEvent, "mbtnClear");
            //    clsEmrFunc.usBtnShow(usFormTopMenuEvent, "mbtnSave");
            //    clsEmrFunc.usBtnShow(usFormTopMenuEvent, "mbtnSaveTemp");
            //    clsEmrFunc.usBtnShow(usFormTopMenuEvent, "mbtnDelete");
            //}
            // 내원일을 세팅한다.
            //clsEmrFunc.SetMedFrEndDate(mstrEmrNo, p, dtpFrDate, dtpEndDate);
            //clsEmrQuery.SetWriteSpdHide(ssWrite, mDirection, mintHeadRow, mintHeadCol);
        }

        /// <summary>
        /// 폼별로 EMR 작성 내역을 화면에 보여준다.
        /// </summary>
        private void pLoadEmrChartInfo()
        {
            if (VB.Val(mstrEmrNo) == 0)
            {
                return;
            }

            SetFlowViewDataBind();
        }


        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private double pSaveEmrData(string strFlag)
        {
            double dblEmrNo = 0;
            //bool blnCert = false;
            string strChartDate = VB.Format(usFormTopMenuEvent.dtMedFrDate.Value, "yyyyMMdd");
            string strChartTime = VB.Replace(usFormTopMenuEvent.txtMedFrTime.Text, ":", "", 1);

            dblEmrNo = clsEmrQuery.SaveChartMst(clsDB.DbCon, p, this, false, this,
                                                                mstrFormNo, mstrUpdateNo, mstrEmrNo, strChartDate, strChartTime,
                                                                clsType.User.IdNumber, clsType.User.IdNumber, strFlag, "0", "", "", ssWrite);

            //if (dblEmrNo != 0)
            //{
            //    if (strFlag == "1")
            //    {
            //        //전자인증
            //        if (clsType.gHosInfo.strEmrCertUseYn == "1")
            //        {
            //            blnCert = clsEmrQuery.SaveDataAEMRCHARTCERTY(this, false, this, dblEmrNo, null);
            //            if (blnCert == false)
            //            {
            //                MessageBox.Show(new Form() { TopMost = true }, "인증중 에러가 발생했습니다." + ComNum.VBLF + "추후 인증을 실시해 주시기 바랍니다.");
            //            }
            //        }
            //    }
            //}
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
                if (clsEmrQuery.IsChangeAuth(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false) return false;
            }

            if (clsXML.gDeleteEmrXml(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == true)
            {
                mstrEmrNo = "0";
                pClearForm();
                //clsEmrQuery.QuerySpdList(p, mstrFormNo, mstrUpdateNo, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                //                            ssWrite, ssList,
                //                            mDirection, mintHeadRow, mintHeadCol, chkDesc.Checked);
                mEmrCallForm.MsgDelete();
            }
            return false;
        }

        /// <summary>
        /// 기록지를 출력한다.
        /// </summary>
        public void pPrintForm()
        {

        }

        #endregion

        #region // FlowSheet 기록지 관련 이벤트
        private void SetColRowCount()
        {
            if (mDirection != "H")
            {
                ssList_Sheet1.RowCount = mintHeadRow;
            }
        }

        /// <summary>
        /// 폼별 특수한 초기화세팅이 필요할 경우 코딩.
        /// </summary>
        private void pInitFormSpc()
        {
            if (mstrFormNo != "0")
            {
                //FormDesignQuery.GetSetDate_AEMRFLOWXML(mstrFormNo, mstrUpdateNo, ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFLOWINPUTSIZE, ref mFormFlowSheet, ref mFormFlowSheetHead);
                FormDesignQuery.GetSetDate_AEMRFLOWXML("1562", "2", ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFLOWINPUTSIZE, ref mFormFlowSheet, ref mFormFlowSheetHead);
                mFLOWGB = pForm.FmFLOWGB;
            }

            if (mFormFlowSheetHead != null)
            {
                SetWriteSpd();
                ssWrite_Sheet1.ColumnHeader.Visible = true;
                ssList_Sheet1.ColumnHeader.Visible = true;
            }
        }

        private void SetWriteSpd()
        {
            pClearForm();

            //ssWrite_Sheet1.SheetCornerStyle.Border = complexBorder2;
            //ssWrite_Sheet1.DefaultStyle.Border = complexBorder2;
            //ssWrite_Sheet1.ColumnHeader.DefaultStyle.Border = complexBorder2;
            //ssWrite_Sheet1.RowHeader.DefaultStyle.Border = complexBorder2;

            if (mFLOWGB == "ROW") //세로방식(아래로 작성)
            {
                ssWrite_Sheet1.RowCount = mFLOWITEMCNT;
                ssWrite_Sheet1.RowHeader.ColumnCount = mFLOWHEADCNT;
                ssWrite_Sheet1.ColumnCount = 1;
                DesignFunc.SetInitSpd(ssWrite, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet);
                DesignFunc.SetHead(ssWrite_Sheet1, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheetHead);

                ssList_Sheet1.RowCount = mFLOWITEMCNT + clsEmrNum.FLOWVIWADD;
                ssList_Sheet1.RowHeader.ColumnCount = mFLOWHEADCNT;
                ssList_Sheet1.ColumnCount = 1;
                DesignFunc.SetInitSpd(ssList, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet, "V");
                DesignFunc.SetHead(ssList_Sheet1, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheetHead, "V");

                ssList_Sheet1.Rows[0, ssList_Sheet1.RowCount - 1].Locked = false;
            }
            else //가로방식(옆으로 작성)
            {
                ssWrite_Sheet1.ColumnCount = mFLOWITEMCNT;
                ssWrite_Sheet1.ColumnHeader.RowCount = mFLOWHEADCNT;
                ssWrite_Sheet1.RowCount = 1;
                DesignFunc.SetInitSpd(ssWrite, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet);
                DesignFunc.SetHead(ssWrite_Sheet1, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheetHead);

                ssList_Sheet1.ColumnCount = mFLOWITEMCNT + clsEmrNum.FLOWVIWADD;
                ssList_Sheet1.ColumnHeader.RowCount = mFLOWHEADCNT;
                ssList_Sheet1.RowCount = 1;
                DesignFunc.SetInitSpd(ssList, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheet, "V");
                DesignFunc.SetHead(ssList_Sheet1, mFLOWGB, mFLOWITEMCNT, mFLOWHEADCNT, mFormFlowSheetHead, "V");

                ssList_Sheet1.Columns[0, ssList_Sheet1.ColumnCount - 1].Locked = false;
            }

            clsSpread.gSpreadEnter_NextCol(ssWrite);
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
            if (clsType.User.AuAPRINTIN == "1")
            {
                mbtnPrint.Visible = true;
            }
            else
            {
                mbtnPrint.Visible = false;
            }
        }

        private void SetPrintVisible()
        {
            if (mDirection != "H")
            {
                //for (int i = 1; i < mintHeadRow; i++)
                //{
                //    ssList_Sheet1.Rows[i].Visible = true;
                //}
            }
        }
        #endregion

        //==========================================================================================//
        //=============================== 아래부터 코딩을 하면 됨 =========================================//
        //=========================================================================================//

        #region //이벤트 매핑
        private void mbtnSearchAll_Click(object sender, EventArgs e)
        {
            SetFlowViewDataBind();
            SetGraph();
        }

        private void SetFlowViewDataBind(string PrintFlag = "")
        {

            int i = 0;
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수

    

            Cursor.Current = Cursors.WaitCursor;
            #region XML
            SQL = " SELECT  'OLD' AS GBN, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTDATE, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTTIME,    ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it1')   AS COL1, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it2')   AS COL2, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it3')   AS COL3, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it4')   AS COL4, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it5')   AS COL5, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it6')   AS COL6, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it7')   AS COL7, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it8')   AS COL8, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it9')   AS COL9, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it14')  AS COL14, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it10')  AS COL10, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it11')  AS COL11, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it12')  AS COL12, ";
            SQL = SQL + ComNum.VBLF + "         EXTRACTVALUE(A.CHARTXML, '//it13')  AS COL13, ";
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
            SQL = SQL + ComNum.VBLF + "         A.MedFrTime,        ";
            SQL = SQL + ComNum.VBLF + "         CASE WHEN EXISTS (SELECT 1 FROM KOSMOS_EMR.EMRPRTREQ WHERE EMRNO = A.EMRNO AND SCANYN = 'T') THEN '사 본' END PRNTYN";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.EMRXML A, KOSMOS_EMR.EMRXMLMST B ";
            SQL = SQL + ComNum.VBLF + " WHERE a.EMRNO = b.EMRNO ";
            SQL = SQL + ComNum.VBLF + "   AND B.FORMNO = 1562 ";
            SQL = SQL + ComNum.VBLF + "   AND B.PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "'";
            #endregion

            #region 신규
            SQL = SQL + ComNum.VBLF + " UNION ALL";
            SQL = SQL + ComNum.VBLF + " SELECT  'NEW' AS GBN, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTDATE, ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTTIME,    ";
            SQL = SQL + ComNum.VBLF + "         '' AS COL1, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000024733') AS COL2, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002018') AS COL3, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000001765') AS COL4, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000037575') AS COL5, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000014815') AS COL6, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000002009') AS COL7, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000001811') AS COL8, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000035464') AS COL9, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000008708') AS COL14, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000418') AS COL10, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000000002') AS COL11, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000018853') AS COL12, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000029454') AS COL13, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000017712') AS COL21, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000010747') AS COL150, ";
            SQL = SQL + ComNum.VBLF + "         (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND EMRNOHIS = A.EMRNOHIS AND ITEMNO = 'I0000001311') AS COL274, ";
            SQL = SQL + ComNum.VBLF + "         A.EMRNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.ACPNO,            ";
            SQL = SQL + ComNum.VBLF + "         A.INOUTCLS,         ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDEPTCD,        ";
            SQL = SQL + ComNum.VBLF + "         A.MEDDRCD,          ";
            SQL = SQL + ComNum.VBLF + "         A.CHARTUSEID,            ";
            SQL = SQL + ComNum.VBLF + "         A.MEDFRDATE,        ";
            SQL = SQL + ComNum.VBLF + "         A.MedFrTime,        ";
            SQL = SQL + ComNum.VBLF + "         CASE WHEN A.PRNTYN = 'Y' THEN '사 본' END PRNTYN";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST A";
            SQL = SQL + ComNum.VBLF + " WHERE FORMNO = 1562 ";
            SQL = SQL + ComNum.VBLF + "   AND PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= '" + dtpFrDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= '" + dtpEndDate.Value.ToString("yyyyMMdd") + "'";
            #endregion

            SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE DESC, CHARTTIME DESC";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
            if (dt == null)
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
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

                ssList_Sheet1.Cells[i, 2].Text = (dt.Rows[i]["COL2"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 3].Text = (dt.Rows[i]["COL3"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 4].Text = (dt.Rows[i]["COL4"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 5].Text = (dt.Rows[i]["COL5"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 6].Text = (dt.Rows[i]["COL6"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 7].Text = (dt.Rows[i]["COL7"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 8].Text = (dt.Rows[i]["COL8"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 9].Text = (dt.Rows[i]["COL9"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 10].Text = (dt.Rows[i]["COL14"].ToString() + "").Trim();

                ssList_Sheet1.Cells[i, 11].Text = (dt.Rows[i]["COL10"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 12].Text = (dt.Rows[i]["COL11"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 13].Text = (dt.Rows[i]["COL12"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 14].Text = (dt.Rows[i]["COL13"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 15].Text = (dt.Rows[i]["COL21"].ToString() + "").Trim();

                ssList_Sheet1.Cells[i, 16].Text = (dt.Rows[i]["COL150"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 17].Text = (dt.Rows[i]["COL274"].ToString() + "").Trim();

                ssList_Sheet1.Cells[i, 18].Text = clsVbfunc.GetInSaName(clsDB.DbCon, (dt.Rows[i]["USEID"].ToString() + "").Trim());
                ssList_Sheet1.Cells[i, 19].Text = (dt.Rows[i]["PRNTYN"].ToString());
                ssList_Sheet1.Cells[i, 20].Text = (dt.Rows[i]["EMRNO"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 20].Tag = (dt.Rows[i]["GBN"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 21].Text = (dt.Rows[i]["USEID"].ToString() + "").Trim();

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

            ssList_Sheet1.SetRowHeight(-1, 24);
            Cursor.Current = Cursors.Default;
        }

        private void mbtnPrint_Click(object sender, EventArgs e)
        {
            pPrintExcept();
        }

        private void mbtnHis_Click(object sender, EventArgs e)
        {
            Console.WriteLine(ssWrite_Sheet1.RowCount);
        }

        private void mbtnSaveAll_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < 3; i++)
            {
                if (ssWrite_Sheet1.Cells[0, i].Text.Trim() == "")
                {
                    ComFunc.MsgBoxEx(this, "값를 입력하세요.");
                    return;
                }
                else if (VB.IsNumeric(ssWrite_Sheet1.Cells[0, i].Text.Trim()) == false)
                {
                    ComFunc.MsgBoxEx(this, "숫자를 입력하세요.");
                    return;
                }
            }

            pSaveData("1"); //인증저장
            SetGraph();
            pLoadEmrChartInfo();
        }

        /// <summary>
        /// 작성내역 더블클릭시 적용 : 공통함수로 이관
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (mDirection != "H")
            {
                if (e.ColumnHeader == true) return;

                if (ssList_Sheet1.RowCount < mintHeadRow) return;
                if (e.Row < mintHeadRow) return;

                string strEMRNO = ssList_Sheet1.Cells[e.Row, ssList_Sheet1.ColumnCount - 3].Text.Trim();

                mstrEmrNo = strEMRNO;
                pSetEmrInfo();
            }
        }

        #endregion

        //출력방향, 마진 등등 변경
        private void pPrintExcept()
        {
            mbtnPrint.Enabled = false;
            SetPrintVisible();

            clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                                         ssList, "P", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Portrait, "ROW", -1, ssList_Sheet1.ColumnCount - 3, mintHeadRow, "P");
            pInitFormSpc();

            mbtnPrint.Enabled = true;
        }

        public frmEmrHealthGraph()
        {
            InitializeComponent();
        }

        public frmEmrHealthGraph(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
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
        public frmEmrHealthGraph(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, string strDeptCode, string strPrtSeq, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            mstrEmrNo = strEmrNo;
            mstrDeptCode = strDeptCode;
            mstrVal = strPrtSeq;
            InitializeComponent();
        }

        private void frmEmrEF00100033_Load(object sender, EventArgs e)
        {
            int intYear = 0;
            int intMonth = 0;
            int intDay = 0;

            string strSql = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;

            mbtnGpPrint.Size = new Size(75, 23);

            pInitForm();

            #region 날짜 셋팅

            //strSql = string.Empty;
            //strSql = "SELECT CHARTDATE FROM KOSMOS_EMR.AEMRCHARTMST";
            //strSql = strSql + ComNum.VBLF + "    WHERE PTNO = '" + p.ptNo + "' ";
            ////strSql = strSql + ComNum.VBLF + "    AND FORMNO = " + mstrFormNo;
            //strSql = strSql + ComNum.VBLF + "      AND FORMNO = 1562";
            //strSql = strSql + ComNum.VBLF + "      AND CHARTDATE >= '" + DateTime.ParseExact(p.medFrDate, "yyyyMMdd", null).AddDays(-60).ToString("yyyyMMdd") + "'";
            //strSql = strSql + ComNum.VBLF + "    ORDER BY CHARTDATE, CHARTTIME";

            //SqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);
            //if (SqlErr != "")
            //{
            //    clsDB.SaveSqlErrLog(SqlErr, SqlErr, clsDB.DbCon);
            //    ComFunc.MsgBoxEx(this, SqlErr);
            //    return;
            //}

            //if (reader.HasRows == false)
            //{
            //    reader.Dispose();
            //    Cursor.Current = Cursors.Default;
            //}
            //else
            //{
            //    dtpFrDate.Value = DateTime.ParseExact(reader.GetValue(0).ToString().Trim(), "yyyyMMdd", null);
            //    reader.Dispose();
            //}
            dtpFrDate.Value = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>().AddDays(-60);

            #endregion

            #region 나이 셋팅
            intDay = (Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) - Convert.ToDateTime(ComFunc.GetBirthDate(p.ssno1, p.ssno2, "-"))).Days;
            intYear = Convert.ToInt32(ComFunc.TruncToDbl(intDay / 365));
            if (Convert.ToInt32(ComFunc.TruncToDbl(((intDay % 365) / 30))) - 1 < 0)
            {
                intMonth = 0;
            }
            else
            {
                intMonth = Convert.ToInt32(ComFunc.TruncToDbl(((intDay % 365) / 30))) + 1;

                if (intMonth >= 12)
                {
                    intMonth -= 12;
                    intYear += 1;
                }
            }

            ssWrite_Sheet1.Cells[0, 0].Text = intYear.ToString();
            ssWrite_Sheet1.Cells[0, 1].Text = intMonth.ToString();
            #endregion

            #region 부-신장, 모-신장, MPH 있으면 셋팅

            strSql = string.Empty;
            strSql = "SELECT ITEMCD, ITEMVALUE FROM KOSMOS_EMR.EMR_BPTINFO";
            strSql = strSql + ComNum.VBLF + "    WHERE PTNO = '" + p.ptNo + "' ";

            SqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SqlErr, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

            if (reader.HasRows == false)
            {
                reader.Dispose();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                List<Control> controls = ComFunc.GetAllControls(this).Where(d => d is TextBox).ToList();

                while (reader.Read())
                {
                    string strTextBox = reader.GetValue(0).ToString().Trim();

                    Control control = controls.Find(d => d.Name.Equals(strTextBox));
                    if (control != null)
                    {
                        control.Text = reader.GetValue(1).ToString().Trim();
                    }
                }

                reader.Dispose();
            }

            #endregion

            SetFlowViewDataBind();
            SetGraph();
        }

        private void SetGraph()
        {
            if (ssList_Sheet1.RowCount == 0)
                return;

            Pen myPen;
            myPen = new Pen(Color.Black);
            myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            myPen.Width = 1;

            Graphics formGraphics = panGraph.CreateGraphics();

            //화면 클리어
            formGraphics.Clear(Color.White);

            //커다란 네모표시
            formGraphics.DrawRectangle(new Pen(Brushes.LightGray, 2), 40, 40, 648, 650);

            int i = 0;
            int k = 0;
            int X = 0;
            int intYstd = 45;
            int intY = 45;
            int intKi = 190;
            int intKg = 135;
            int intMPH = 0;
            int intMPHCM = 0;

            int[] intMu = new int[20];
            int[] intKe = new int[20];
            int[] intCm = new int[20];
            int[] intkg = new int[20];

            int intRow = 0;

            string strSql = string.Empty;
            OracleDataReader reader = null;
            string strMPH = string.Empty;

            string SqlErr = string.Empty;
            strSql = "SELECT ITEMVALUE FROM KOSMOS_EMR.EMR_BPTINFO";
            strSql = strSql + ComNum.VBLF + "    WHERE PTNO = '" + p.ptNo + "' ";
            strSql = strSql + ComNum.VBLF + "    AND ITEMCD = 'I0000037281'";

            SqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);

            if (reader.HasRows == false)
            {
                reader.Dispose();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                strMPH = reader.GetValue(0).ToString().Trim();
                reader.Dispose();
            }

            #region 가로 선, 좌/우 숫자 표기
            for (i = 0; i < 130; i++)
            {
                intY = intYstd + (5 * i);
                X = X + 1;

                for (intRow = 0; intRow < mFormFlowSheet.Length; intRow++)
                {
                    //MPH
                    if (strMPH != "")
                    {
                        if ((intKi - (i + 1)) == Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(strMPH))))
                        {
                            intMPH = intY;
                            intMPHCM = (intKi - (i + 1));
                        }
                    }

                    //키
                    if (mFormFlowSheet[intRow].ItemCode == "I0000000002")
                    {
                        for (k = 0; k < ssList_Sheet1.RowCount; k++)
                        {
                            if (ssList_Sheet1.Cells[k, intRow + 2].Text.Trim() != "")
                            {
                                if ((intKi - (i + 1)) == Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow + 2].Text.Trim()))))
                                {
                                    intKe[k] = intY;
                                    intCm[k] = (intKi - (i + 1));
                                }
                            }
                            else
                            {
                                ssList_Sheet1.Cells[k, intRow + 2].Text = "0";
                            }
                        }
                    }

                    //체중
                    if (mFormFlowSheet[intRow].ItemCode == "I0000000418")
                    {
                        for (k = 0; k < ssList_Sheet1.RowCount; k++)
                        {
                            if (ssList_Sheet1.Cells[k, intRow + 2].Text.Trim() != "")
                            {
                                if ((intKg - (i + 1)) == Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow + 2].Text.Trim()))))
                                {
                                    intMu[k] = intY;
                                    intkg[k] = (intKg - (i + 1));
                                }
                            }
                            else
                            {
                                ssList_Sheet1.Cells[k, intRow + 2].Text = "0";
                            }
                        }
                    }
                }

                if (X == 5)
                {
                    formGraphics.DrawLine(new Pen(Brushes.LightGray, 2), 40, intY, 688, intY);

                    if ((intKi - (i + 1)) >= 80)
                    {
                        sDrawString(formGraphics, (intKi - (i + 1)).ToString(), "굴림", 8, Brushes.Black, 10, intY - 5);

                        if ((intKi - (i + 1)) >= 155)
                        {
                            sDrawString(formGraphics, (intKi - (i + 1)).ToString(), "굴림", 8, Brushes.Black, 693, intY - 5);
                        }
                        else if ((intKi - (i + 1)) == 150)
                        {
                            sDrawString(formGraphics, "(cm)", "굴림", 8, Brushes.Black, 693, intY - 5);
                        }
                    }
                    else if ((intKi - (i + 1)) == 75)
                    {
                        sDrawString(formGraphics, "(cm)", "굴림", 8, Brushes.Black, 5, intY - 5);
                    }

                    if ((intKg - (i + 1)) <= 85)
                    {
                        if ((intKg - (i + 1)) > 15)
                        {
                            sDrawString(formGraphics, (intKg - (i + 1)).ToString(), "굴림", 8, Brushes.Black, 695, intY - 5);
                        }
                        else if ((intKg - (i + 1)) > 5)
                        {
                            sDrawString(formGraphics, (intKg - (i + 1)).ToString(), "굴림", 8, Brushes.Black, 695, intY - 5);
                            sDrawString(formGraphics, (intKg - (i + 1)).ToString(), "굴림", 8, Brushes.Black, 10, intY - 5);
                        }
                        else if ((intKg - (i + 1)) == 5)
                        {
                            sDrawString(formGraphics, "(kg)", "굴림", 8, Brushes.Black, 5, intY - 5);
                            sDrawString(formGraphics, "(kg)", "굴림", 8, Brushes.Black, 693, intY - 5);
                        }
                    }

                    X = 0;
                }
                else
                {
                    formGraphics.DrawLine(new Pen(Brushes.LightGray, 1), 40, intY, 688, intY);
                }
            }
            #endregion

            #region 세로 선, 상/하 나이 표기
            int intXstd = 45;
            int intX = 45;
            int intAge = 2;

            for (i = 0; i < 108; i++)
            {
                intX = intXstd + (6 * i);
                X += 1;

                if (i == 0)
                {
                    sDrawString(formGraphics, intAge.ToString(), "굴림", 8, Brushes.Black, intX - 11, 20);
                    sDrawString(formGraphics, intAge.ToString(), "굴림", 8, Brushes.Black, intX - 11, 703);
                }

                if (X == 6)
                {
                    intAge = intAge + 1;

                    if (intAge != 20)
                    {
                        formGraphics.DrawLine(new Pen(Brushes.LightGray, 2), intX, 40, intX, 690);
                    }

                    if (intAge < 20)
                    {
                        sDrawString(formGraphics, intAge.ToString(), "굴림", 8, Brushes.Black, intX - 11, 20);
                        sDrawString(formGraphics, intAge.ToString(), "굴림", 8, Brushes.Black, intX - 11, 703);
                    }
                    else// if (intAge == 21)
                    {
                        sDrawString(formGraphics, intAge.ToString() + " (세)", "굴림", 8, Brushes.Black, intX - 11, 20);
                        sDrawString(formGraphics, intAge.ToString() + " (세)", "굴림", 8, Brushes.Black, intX - 11, 703);
                    }

                    X = 0;
                }
                else
                {
                    formGraphics.DrawLine(new Pen(Brushes.LightGray, 1), intX, 40, intX, 690);
                }
            }
            #endregion

            #region 표준 수치 그래프 작성 남/여 구분
            // Create pens.
            Pen MPen1 = new Pen(Color.CornflowerBlue, 2);
            Pen MPen2 = new Pen(Color.CornflowerBlue, 1);
            Pen WPen1 = new Pen(Color.HotPink, 2);
            Pen WPen2 = new Pen(Color.HotPink, 1);

            if (p.sex == "남")
            {
                Point[] curvePoints1 = { new Point(40, 512), new Point(112, 438), new Point(184, 365), new Point(256, 300), new Point(328, 237), new Point(400, 170), new Point(472, 115), new Point(544, 83), new Point(616, 72), new Point(688, 66) };
                Point[] curvePoints2 = { new Point(40, 527), new Point(112, 452), new Point(184, 380), new Point(256, 318), new Point(328, 258), new Point(400, 195), new Point(472, 133), new Point(544, 100), new Point(616, 87), new Point(688, 85) };
                Point[] curvePoints3 = { new Point(40, 542), new Point(112, 465), new Point(184, 395), new Point(256, 340), new Point(328, 283), new Point(400, 220), new Point(472, 153), new Point(544, 116), new Point(616, 107), new Point(688, 104) };
                Point[] curvePoints4 = { new Point(40, 557), new Point(112, 483), new Point(184, 407), new Point(256, 357), new Point(328, 304), new Point(400, 245), new Point(472, 183), new Point(544, 135), new Point(616, 122), new Point(688, 120) };
                Point[] curvePoints5 = { new Point(40, 572), new Point(112, 498), new Point(184, 430), new Point(256, 378), new Point(328, 328), new Point(400, 275), new Point(472, 207), new Point(544, 155), new Point(616, 143), new Point(688, 138) };
                Point[] curvePoints6 = { new Point(40, 581), new Point(112, 510), new Point(184, 443), new Point(256, 395), new Point(328, 350), new Point(400, 300), new Point(472, 238), new Point(544, 175), new Point(616, 162), new Point(688, 158) };
                Point[] curvePoints7 = { new Point(40, 598), new Point(112, 520), new Point(184, 460), new Point(256, 410), new Point(328, 368), new Point(400, 323), new Point(472, 263), new Point(544, 193), new Point(616, 180), new Point(688, 178) };

                Point[] curvePoints8 = { new Point(40, 634), new Point(112, 613), new Point(184, 575), new Point(256, 522), new Point(328, 462), new Point(400, 408), new Point(472, 348), new Point(544, 313), new Point(616, 300), new Point(688, 292) };
                Point[] curvePoints9 = { new Point(40, 641), new Point(112, 621), new Point(184, 590), new Point(256, 547), new Point(328, 495), new Point(400, 442), new Point(472, 385), new Point(544, 352), new Point(616, 337), new Point(688, 327) };
                Point[] curvePoints10 = { new Point(40, 647), new Point(112, 628), new Point(184, 604), new Point(256, 570), new Point(328, 528), new Point(400, 480), new Point(472, 420), new Point(544, 387), new Point(616, 373), new Point(688, 359) };
                Point[] curvePoints11 = { new Point(40, 652), new Point(112, 635), new Point(184, 614), new Point(256, 588), new Point(328, 557), new Point(400, 513), new Point(472, 460), new Point(544, 418), new Point(616, 403), new Point(688, 388) };
                Point[] curvePoints12 = { new Point(40, 656), new Point(112, 640), new Point(184, 622), new Point(256, 603), new Point(328, 577), new Point(400, 542), new Point(472, 493), new Point(544, 446), new Point(616, 426), new Point(688, 413) };
                Point[] curvePoints13 = { new Point(40, 660), new Point(112, 645), new Point(184, 627), new Point(256, 610), new Point(328, 590), new Point(400, 563), new Point(472, 521), new Point(544, 466), new Point(616, 446), new Point(688, 434) };
                Point[] curvePoints14 = { new Point(40, 665), new Point(112, 650), new Point(184, 634), new Point(256, 618), new Point(328, 603), new Point(400, 578), new Point(472, 543), new Point(544, 492), new Point(616, 464), new Point(688, 454) };

                formGraphics.DrawCurve(MPen2, curvePoints1);
                formGraphics.DrawCurve(MPen2, curvePoints2);
                formGraphics.DrawCurve(MPen2, curvePoints3);
                formGraphics.DrawCurve(MPen1, curvePoints4);
                formGraphics.DrawCurve(MPen2, curvePoints5);
                formGraphics.DrawCurve(MPen2, curvePoints6);
                formGraphics.DrawCurve(MPen2, curvePoints7);

                formGraphics.DrawCurve(MPen2, curvePoints8);
                formGraphics.DrawCurve(MPen2, curvePoints9);
                formGraphics.DrawCurve(MPen2, curvePoints10);
                formGraphics.DrawCurve(MPen1, curvePoints11);
                formGraphics.DrawCurve(MPen2, curvePoints12);
                formGraphics.DrawCurve(MPen2, curvePoints13);
                formGraphics.DrawCurve(MPen2, curvePoints14);

                //남
                sDrawString(formGraphics, "신장(cm)", "굴림", 13, Brushes.Black, 603, 43);
                sDrawString(formGraphics, "97", "굴림", 8, Brushes.Black, 660, 63);
                sDrawString(formGraphics, "90", "굴림", 8, Brushes.Black, 660, 80);
                sDrawString(formGraphics, "75", "굴림", 8, Brushes.Black, 660, 100);
                sDrawString(formGraphics, "50", "굴림", 8, Brushes.Black, 660, 115);
                sDrawString(formGraphics, "25", "굴림", 8, Brushes.Black, 660, 135);
                sDrawString(formGraphics, "10", "굴림", 8, Brushes.Black, 660, 155);
                sDrawString(formGraphics, "3", "굴림", 8, Brushes.Black, 663, 173);

                sDrawString(formGraphics, "체중(kg)", "굴림", 13, Brushes.Black, 606, 265);
                sDrawString(formGraphics, "97", "굴림", 8, Brushes.Black, 660, 290);
                sDrawString(formGraphics, "90", "굴림", 8, Brushes.Black, 660, 325);
                sDrawString(formGraphics, "75", "굴림", 8, Brushes.Black, 660, 357);
                sDrawString(formGraphics, "50", "굴림", 8, Brushes.Black, 660, 387);
                sDrawString(formGraphics, "25", "굴림", 8, Brushes.Black, 660, 410);
                sDrawString(formGraphics, "10", "굴림", 8, Brushes.Black, 660, 432);
                sDrawString(formGraphics, "3", "굴림", 8, Brushes.Black, 663, 452);

            }
            else
            {
                //여키
                Point[] curvePoints1 = { new Point(40, 515), new Point(112, 445), new Point(184, 374), new Point(256, 306), new Point(328, 232), new Point(400, 177), new Point(472, 152), new Point(544, 140), new Point(616, 137), new Point(688, 135) };
                Point[] curvePoints2 = { new Point(40, 533), new Point(112, 455), new Point(184, 390), new Point(256, 327), new Point(328, 258), new Point(400, 197), new Point(472, 166), new Point(544, 155), new Point(616, 153), new Point(688, 149) };
                Point[] curvePoints3 = { new Point(40, 544), new Point(112, 468), new Point(184, 407), new Point(256, 343), new Point(328, 285), new Point(400, 227), new Point(472, 185), new Point(544, 170), new Point(616, 168), new Point(688, 166) };
                Point[] curvePoints4 = { new Point(40, 560), new Point(112, 485), new Point(184, 420), new Point(256, 370), new Point(328, 310), new Point(400, 245), new Point(472, 203), new Point(544, 190), new Point(616, 185), new Point(688, 180) };
                Point[] curvePoints5 = { new Point(40, 572), new Point(112, 500), new Point(184, 435), new Point(256, 384), new Point(328, 332), new Point(400, 270), new Point(472, 226), new Point(544, 207), new Point(616, 201), new Point(688, 198) };
                Point[] curvePoints6 = { new Point(40, 585), new Point(112, 516), new Point(184, 454), new Point(256, 403), new Point(328, 356), new Point(400, 298), new Point(472, 247), new Point(544, 226), new Point(616, 219), new Point(688, 217) };
                Point[] curvePoints7 = { new Point(40, 596), new Point(112, 532), new Point(184, 465), new Point(256, 417), new Point(328, 375), new Point(400, 320), new Point(472, 260), new Point(544, 237), new Point(616, 235), new Point(688, 233) };

                //여체중
                Point[] curvePoints8 = { new Point(40, 638), new Point(112, 615), new Point(184, 581), new Point(256, 535), new Point(328, 475), new Point(400, 414), new Point(472, 387), new Point(544, 373), new Point(616, 368), new Point(688, 364) };
                Point[] curvePoints9 = { new Point(40, 643), new Point(112, 620), new Point(184, 594), new Point(256, 555), new Point(328, 497), new Point(400, 441), new Point(472, 415), new Point(544, 405), new Point(616, 398), new Point(688, 396) };
                Point[] curvePoints10 = { new Point(40, 646), new Point(112, 632), new Point(184, 610), new Point(256, 577), new Point(328, 535), new Point(400, 490), new Point(472, 444), new Point(544, 430), new Point(616, 427), new Point(688, 423) };
                Point[] curvePoints11 = { new Point(40, 655), new Point(112, 640), new Point(184, 620), new Point(256, 597), new Point(328, 563), new Point(400, 512), new Point(472, 472), new Point(544, 454), new Point(616, 447), new Point(688, 442) };
                Point[] curvePoints12 = { new Point(40, 659), new Point(112, 646), new Point(184, 625), new Point(256, 605), new Point(328, 580), new Point(400, 543), new Point(472, 508), new Point(544, 476), new Point(616, 462), new Point(688, 457) };
                Point[] curvePoints13 = { new Point(40, 662), new Point(112, 650), new Point(184, 631), new Point(256, 615), new Point(328, 593), new Point(400, 564), new Point(472, 518), new Point(544, 490), new Point(616, 479), new Point(688, 469) };
                Point[] curvePoints14 = { new Point(40, 666), new Point(112, 654), new Point(184, 642), new Point(256, 625), new Point(328, 605), new Point(400, 578), new Point(472, 540), new Point(544, 505), new Point(616, 490), new Point(688, 475) };

                //여
                formGraphics.DrawCurve(WPen2, curvePoints1);
                formGraphics.DrawCurve(WPen2, curvePoints2);
                formGraphics.DrawCurve(WPen2, curvePoints3);
                formGraphics.DrawCurve(WPen1, curvePoints4);
                formGraphics.DrawCurve(WPen2, curvePoints5);
                formGraphics.DrawCurve(WPen2, curvePoints6);
                formGraphics.DrawCurve(WPen2, curvePoints7);

                formGraphics.DrawCurve(WPen2, curvePoints8);
                formGraphics.DrawCurve(WPen2, curvePoints9);
                formGraphics.DrawCurve(WPen2, curvePoints10);
                formGraphics.DrawCurve(WPen1, curvePoints11);
                formGraphics.DrawCurve(WPen2, curvePoints12);
                formGraphics.DrawCurve(WPen2, curvePoints13);
                formGraphics.DrawCurve(WPen2, curvePoints14);

                //여
                sDrawString(formGraphics, "신장(cm)", "굴림", 13, Brushes.Black, 603, 100);
                sDrawString(formGraphics, "97", "굴림", 8, Brushes.Black, 660, 131);
                sDrawString(formGraphics, "90", "굴림", 8, Brushes.Black, 660, 146);
                sDrawString(formGraphics, "75", "굴림", 8, Brushes.Black, 660, 161);
                sDrawString(formGraphics, "50", "굴림", 8, Brushes.Black, 660, 177);
                sDrawString(formGraphics, "25", "굴림", 8, Brushes.Black, 660, 194);
                sDrawString(formGraphics, "10", "굴림", 8, Brushes.Black, 660, 212);
                sDrawString(formGraphics, "3", "굴림", 8, Brushes.Black, 663, 228);

                sDrawString(formGraphics, "체중(kg)", "굴림", 13, Brushes.Black, 606, 335);
                sDrawString(formGraphics, "97", "굴림", 8, Brushes.Black, 660, 360);
                sDrawString(formGraphics, "90", "굴림", 8, Brushes.Black, 660, 392);
                sDrawString(formGraphics, "75", "굴림", 8, Brushes.Black, 660, 419);
                sDrawString(formGraphics, "50", "굴림", 8, Brushes.Black, 660, 439);
                sDrawString(formGraphics, "25", "굴림", 8, Brushes.Black, 660, 453);
                sDrawString(formGraphics, "10", "굴림", 8, Brushes.Black, 660, 466);
                sDrawString(formGraphics, "3", "굴림", 8, Brushes.Black, 663, 477);
            }
            #endregion

            #region 입력한 수치 그래프에서 점으로 표기#
            //나이 년 개월
            int[] intYear = new int[20];
            int[] intMonth = new int[20];
            int[] intNae = new int[20];

            //뼈나이 년 개월
            int[] intBoneYear = new int[20];
            int[] intBoneMonth = new int[20];
            int[] intBoneNae = new int[20];

            for (intRow = 0; intRow < mFormFlowSheet.Length; intRow++)
            {
                //키
                if (mFormFlowSheet[intRow].ItemCode == "I0000000002")
                {
                    for (k = 0; k < ssList_Sheet1.RowCount; k++)
                    {
                        if (ssList_Sheet1.Cells[k, intRow + 2].Text.Trim() != "")
                        {
                            if ((intKi - (i + 1)) == Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow + 2].Text.Trim()))))
                            {
                                intKe[k] = intY;
                            }

                            #region 신규
                            int mYear = 0;
                            int mMonth = 0;

                            DateTime dtpChartDate = ssList_Sheet1.Cells[k, 0].Text.Trim().To<DateTime>();

                            ToAgeString(dtpChartDate, DateTime.ParseExact(p.birthdate, "yyyyMMdd", null), ref mMonth, ref mYear);
                       
                            if (intYear.Where(d => d == mYear).Count() == 0)
                            {
                                //년
                                intYear[k] = mYear;

                                //개월
                                intMonth[k] = mMonth;
                            }
                            #endregion
                        }
                        else
                        {
                            ssList_Sheet1.Cells[k, intRow + 2].Text = "0";
                        }
                    }
                }
            }

            for (intRow = 0; intRow < mFormFlowSheet.Length; intRow++)
            {
                #region 이전
                ////년
                //if (mFormFlowSheet[intRow].ItemCode == "I0000015052")
                //{
                //    for (k = 0; k < ssList_Sheet1.RowCount; k++)
                //    {
                //        if (ssList_Sheet1.Cells[k, intRow + 2].Text.Trim() != "")
                //        {
                //            intYear[k] = Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow + 2].Text.Trim())));
                //        }
                //    }
                //}

                ////개월
                //if (mFormFlowSheet[intRow].ItemCode == "I0000002129")
                //{
                //    for (k = 0; k < ssList_Sheet1.RowCount; k++)
                //    {
                //        if (ssList_Sheet1.Cells[k, intRow + 2].Text.Trim() != "")
                //        {
                //            intMonth[k] = Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow + 2].Text.Trim())));
                //        }
                //    }
                //}
                #endregion

                //뼈나이(년) I0000031665_0
                if (mFormFlowSheet[intRow].ItemCode == "I0000037284")
                {
                    for (k = 0; k < ssList_Sheet1.RowCount; k++)
                    {
                        if (ssList_Sheet1.Cells[k, intRow + 2].Text.Trim() != "")
                        {
                            intBoneYear[k] = Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow + 2].Text.Trim())));
                        }
                    }
                }

                //뼈나이(개월) I0000031665_1
                if (mFormFlowSheet[intRow].ItemCode == "I0000037285")
                {
                    for (k = 0; k < ssList_Sheet1.RowCount; k++)
                    {
                        if (ssList_Sheet1.Cells[k, intRow + 2].Text.Trim() != "")
                        {
                            intBoneMonth[k] = Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow + 2].Text.Trim())));
                        }
                    }
                }
            }

            List<int> lstKe = new List<int>();
            List<int> lstKg = new List<int>();

            for (intRow = 0; intRow < mFormFlowSheet.Length; intRow++)
            {
                //나이
                intNae[intRow] = (35 * (Convert.ToInt32(intYear[intRow]) - 1) + 3) + (Convert.ToInt32(intMonth[intRow]) * 3);

                //뼈나이
                intBoneNae[intRow] = (35 * (Convert.ToInt32(intBoneYear[intRow]) - 1) + 3) + (Convert.ToInt32(intBoneMonth[intRow]) * 3);

                if (intKe[intRow] > 0)
                {
                    if (intBoneNae[intRow] - 2 > 0)
                    {
                        int intNaegu = 0;
                        int intNaeguEnd = 0;

                        //if (intNae[intRow - 4] + 5 > intBoneNae[intRow - 4] + 5)
                        //{
                        //    intNaegu = (intNae[intRow - 4] + 5) - (intBoneNae[intRow - 4] + 5);
                        //    intNaegu = intNaegu / 5;
                        //    intNaegu = (intNae[intRow - 4] + 5) - intNaegu;
                        //    intNaeguEnd = intNae[intRow - 4] + 5;
                        //}
                        //else
                        //{
                        intNaegu = (intBoneNae[intRow] + 5) - (intNae[intRow] + 5);
                        intNaegu = intNaegu / 5;
                        intNaegu = (intBoneNae[intRow] + 5) - intNaegu;
                        intNaeguEnd = intBoneNae[intRow] + 5;
                        //}

                        //나이 - 뼈나이 사이 선
                        formGraphics.DrawLine(new Pen(Color.Red, 2), intNae[intRow] + 5, intKe[intRow], intBoneNae[intRow] + 5, intKe[intRow]);
                        formGraphics.DrawLine(new Pen(Color.Red, 2), intNaegu, intKe[intRow] - 4, intNaegu, intKe[intRow] + 4);
                        formGraphics.DrawLine(new Pen(Color.Red, 2), intNaegu, intKe[intRow] - 4, intNaeguEnd, intKe[intRow]);
                        formGraphics.DrawLine(new Pen(Color.Red, 2), intNaegu, intKe[intRow] + 4, intNaeguEnd, intKe[intRow]);
                    }

                    if (lstKe.IndexOf(intCm[intRow]) == -1)
                    {
                        //나이
                        formGraphics.FillEllipse(Brushes.Black, intNae[intRow] + 5, intKe[intRow] - 3, 4, 4);
                        sDrawString(formGraphics, "(" + intYear[intRow] + "." + intMonth[intRow] + ", " + intCm[intRow] + ")", "굴림", 8, Brushes.Black, intNae[intRow] + 15, intKe[intRow] - 3);
                        lstKe.Add(intCm[intRow]);
                    }


                    ////뼈나이
                    //formGraphics.FillEllipse(Brushes.BlueViolet, intBoneNae[intRow] + 5, intKe[intRow] - 3, 6, 6);
                    //sDrawString(formGraphics, "(" + intBoneYear[intRow] + "." + intBoneMonth[intRow] + ", " + intCm[intRow] + ")", "굴림", 8, Brushes.BlueViolet, intBoneNae[intRow] - 30, intKe[intRow] + 20);
                }

                if (intMu[intRow] > 0)
                {
                    //if (intBoneNae[intRow - 4] - 2 > 0)
                    //{
                    //    //나이 - 뼈나이 사이 선
                    //    formGraphics.DrawLine(new Pen(Color.Red, 2), intNae[intRow - 4] + 5, intMu[intRow - 4], intBoneNae[intRow - 4] + 5, intMu[intRow - 4]);
                    //}

                    if (lstKg.IndexOf(intkg[intRow]) == -1)
                    {
                        //나이
                        formGraphics.FillEllipse(Brushes.Black, intNae[intRow] + 5, intMu[intRow] - 3, 4, 4);
                        sDrawString(formGraphics, "(" + intYear[intRow] + "." + intMonth[intRow] + ", " + intkg[intRow] + ")", "굴림", 8, Brushes.Black, intNae[intRow] + 15, intMu[intRow] - 3);
                        lstKg.Add(intCm[intRow]);
                    }

                    //뼈나이
                    //formGraphics.FillEllipse(Brushes.BlueViolet, intBoneNae[intRow - 4] + 5, intMu[intRow - 4] - 3, 6, 6);
                    //sDrawString(formGraphics, ("(" + intBoneYear[intRow - 4] + "." + intBoneMonth[intRow - 4] + ", " + intkg[intRow - 4] + ")", "굴림", 8, Brushes.BlueViolet, intBoneNae[intRow - 4] - 30, intMu[intRow - 4] + 20);
                }
            }
            #endregion

            //MPH 표기
            if (intMPH > 0)
            {
                formGraphics.DrawLine(new Pen(Color.Violet, 2), 40, intMPH, 688, intMPH);
                sDrawString(formGraphics, intMPHCM.ToString() + " ± 10", "굴림", 9, Brushes.Black, 50, intMPH - 12);
            }

            myPen.Dispose();
            formGraphics.Dispose();
        }

        private void ToAgeString(DateTime dtpChart, DateTime dob, ref int rtnMonth, ref int rtnYears)
        {
            DateTime today = dtpChart;

            int months = today.Month - dob.Month;
            int years = today.Year - dob.Year;

            if (today.Day < dob.Day)
            {
                months--;
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            rtnMonth = months;
            rtnYears = years;
        }

        private void frmEmrEF00100033_ResizeEnd(object sender, EventArgs e)
        {
            SetGraph();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //clsEmrQuery.QuerySpdList(p, mstrFormNo, mstrUpdateNo, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
            //                                ssWrite, ssList,
            //                                mDirection, mintHeadRow, mintHeadCol, chkDesc.Checked);
            SetGraph();
        }

        private void panChart_Scroll(object sender, ScrollEventArgs e)
        {
            SetGraph();
        }

        private void I0000000562_1_TextChanged(object sender, EventArgs e)
        {
            if (I0000037281.Text.Trim() != "" && I0000037282.Text.Trim() != "")
            {
                if (VB.IsNumeric(I0000037281.Text.Trim()) == true && VB.IsNumeric(I0000037282.Text.Trim()) == true)
                {
                    if (p.sex == "남")
                    {
                        I0000037283.Text = VB.Format((Convert.ToDouble(I0000037281.Text.Trim()) + Convert.ToDouble(I0000037282.Text.Trim()) + 13) / 2, "0.0");
                    }
                    else
                    {
                        I0000037283.Text = VB.Format((Convert.ToDouble(I0000037281.Text.Trim()) + Convert.ToDouble(I0000037282.Text.Trim()) - 13) / 2, "0.0");
                    }
                }
            }
        }

        private void mbtnSaveK_Click(object sender, EventArgs e)
        {
            string SqlErr = string.Empty;
            string strSql = "";
            DataTable dt = null;
            int RowAffected = 0;

            if (I0000037281.Text.Trim() == "" || I0000037282.Text.Trim() == "")
            {
                ComFunc.MsgBoxEx(this, "값을 입력하세요");
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                strSql = "";
                strSql = "SELECT 1 FROM KOSMOS_EMR.EMR_BPTINFO";
                strSql = strSql + ComNum.VBLF + "    WHERE PTNO = '" + p.ptNo + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, strSql, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, strSql, clsDB.DbCon);
                    return;
                }


                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    strSql = "";
                    strSql = "DELETE KOSMOS_EMR.EMR_BPTINFO";
                    strSql = strSql + ComNum.VBLF + "    WHERE PTNO = '" + p.ptNo + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(strSql, ref RowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, strSql, clsDB.DbCon);
                        return;
                    }
                }

                strSql = "";
                strSql = "INSERT INTO KOSMOS_EMR.EMR_BPTINFO";
                strSql = strSql + ComNum.VBLF + "    (PTNO, ITEMCD, ITEMNO, ITEMVALUE)";
                strSql = strSql + ComNum.VBLF + "    VALUES (";
                strSql = strSql + ComNum.VBLF + "    '" + p.ptNo + "' ";
                strSql = strSql + ComNum.VBLF + "    ,'I0000037281'";
                strSql = strSql + ComNum.VBLF + "    ,'I0000037281' ";
                strSql = strSql + ComNum.VBLF + "    ,'" + I0000037281.Text.Trim() + "')";

                SqlErr = clsDB.ExecuteNonQuery(strSql, ref RowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, strSql, clsDB.DbCon);
                    return;
                }

                strSql = "";
                strSql = "INSERT INTO KOSMOS_EMR.EMR_BPTINFO";
                strSql = strSql + ComNum.VBLF + "    (PTNO, ITEMCD, ITEMNO, ITEMVALUE)";
                strSql = strSql + ComNum.VBLF + "    VALUES (";
                strSql = strSql + ComNum.VBLF + "    '" + p.ptNo + "' ";
                strSql = strSql + ComNum.VBLF + "    ,'I0000037282'";
                strSql = strSql + ComNum.VBLF + "    ,'I0000037282' ";
                strSql = strSql + ComNum.VBLF + "    ,'" + I0000037282.Text.Trim() + "')";

                SqlErr = clsDB.ExecuteNonQuery(strSql, ref RowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, strSql, clsDB.DbCon);
                    return;
                }

                strSql = "";
                strSql = "INSERT INTO KOSMOS_EMR.EMR_BPTINFO";
                strSql = strSql + ComNum.VBLF + "    (PTNO, ITEMCD, ITEMNO, ITEMVALUE)";
                strSql = strSql + ComNum.VBLF + "    VALUES (";
                strSql = strSql + ComNum.VBLF + "    '" + p.ptNo + "' ";
                strSql = strSql + ComNum.VBLF + "    ,'I0000037283'";
                strSql = strSql + ComNum.VBLF + "    ,'I0000037283' ";
                strSql = strSql + ComNum.VBLF + "    ,'" + I0000037283.Text.Trim() + "')";

                SqlErr = clsDB.ExecuteNonQuery(strSql, ref RowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, strSql, clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "저장 하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void mbtnGpPrint_Click(object sender, EventArgs e)
        {
            //CaptureScreen(); //캡쳐
            ////printDocument1.Print(); //출력
            //memoryImage.Save("C:\\Users\\EMR\\Desktop\\12345.jpg");

            PrintDocument GraphPrint = new PrintDocument();
            PrintController printController = new StandardPrintController();
            GraphPrint.PrintController = printController;  //기본인쇄창 없애기

            PageSettings ps = new PageSettings();
            ps.Margins = new Margins(50, 50, 10, 10);
            ps.Landscape = false;
            GraphPrint.DefaultPageSettings = ps;
            //GraphPrint.PrinterSettings.PrinterName = GraphPrint;
            GraphPrint.PrintPage += new PrintPageEventHandler(GraphPrint_PrintPage);
            GraphPrint.Print();
        }

        private void GraphPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            //상단에 환자 정보 찍어주고 : OCR출력 / 스프래드출력 참고
            Pen cPen = new Pen(Color.Black);
            cPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            cPen.Width = 3;
            StringFormat drawFormat = new StringFormat();

            drawFormat.Alignment = StringAlignment.Center;

            //Header
            e.Graphics.DrawLine(cPen, 20, 170, 786, 170);
            sDrawString(e.Graphics, "성장 곡선", "굴림", 16, Brushes.Black, 350, 120);

            drawFormat.Alignment = StringAlignment.Near;
            e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 20, 20, 230, 140);

            sDrawString(e.Graphics, "등" + VB.Space(1) + "록" + VB.Space(1) + "번" + VB.Space(1) + "호", "굴림", 11, Brushes.Black, 30, 30);
            sDrawString(e.Graphics, p.ptNo, "굴림", 11, Brushes.Black, 120, 30);

            sDrawString(e.Graphics, "성" + VB.Space(9) + "명", "굴림", 11, Brushes.Black, 30, 48);
            sDrawString(e.Graphics, p.ptName, "굴림", 11, Brushes.Black, 120, 48);

            sDrawString(e.Graphics, "주" + VB.Space(1) + "민" + VB.Space(1) + "번" + VB.Space(1) + "호", "굴림", 11, Brushes.Black, 30, 66);
            sDrawString(e.Graphics, p.ssno1 + "-" + VB.Left(p.ssno2, 1) + "******", "굴림", 11, Brushes.Black, 120, 66);

            sDrawString(e.Graphics, "성별" + VB.Space(1) + "/" + VB.Space(1) + "나이", "굴림", 11, Brushes.Black, 30, 86);
            sDrawString(e.Graphics, p.sex + "/" + p.age, "굴림", 11, Brushes.Black, 120, 86);

            sDrawString(e.Graphics, "진료과/주치의", "굴림", 11, Brushes.Black, 30, 105);
            sDrawString(e.Graphics, p.medDeptCd + " " + p.medDrName, "굴림", 11, Brushes.Black, 140, 105);

            string strInOut = "진료일자";
            if (p.inOutCls == "I")
            {
                strInOut = "입원일자";
            } //
            sDrawString(e.Graphics, strInOut, "굴림", 11, Brushes.Black, 30, 124);
            sDrawString(e.Graphics, ComFunc.FormatStrToDate(p.medFrDate, "DK"), "굴림", 11, Brushes.Black, 110, 124);

            sDrawString(e.Graphics, "병동" + VB.Space(1) + "/" + VB.Space(1) + "병실", "굴림", 11, Brushes.Black, 30, 140);
            if (p.room != "")
            {
                sDrawString(e.Graphics, p.ward + "/" + p.room, "굴림", 11, Brushes.Black, 120, 140);
            }

            //Footer
            e.Graphics.DrawLine(cPen, 20, 1094, 786, 1094);

            drawFormat.Alignment = StringAlignment.Near;
            sDrawString(e.Graphics, "출력자 : " + clsType.User.UserName + VB.Space(2) + "출력일시 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"), "굴림", 9, Brushes.Black, 30, 1094 + 20, drawFormat);

            FileInfo fOcrFile = new FileInfo(clsType.SvrInfo.strClient + "\\exenet\\hsplog.png");
            if (fOcrFile.Exists == true)
            {
                e.Graphics.DrawImage(System.Drawing.Image.FromFile(clsType.SvrInfo.strClient + "\\exenet\\hsplog.png"), 640, 1094 + 10, 126, 22);
            }
            else
            {
                sDrawString(e.Graphics, "포항성모병원", "굴림", 16, Brushes.Black, 650, 1094 + 10);
            }

            //==========================================
            //==========================================

            //아래에 성장곡선을 그려 준다       //그래프에서 x + 39 / y + 210
            cPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            cPen.Width = 1;

            e.Graphics.DrawRectangle(new Pen(Brushes.LightGray, 2), 79, 250, 648, 650);

            int i = 0;
            int k = 0;
            int X = 0;
            int intYstd = 255;
            int intY = 255;
            int intKi = 190;
            int intKg = 135;
            int intMPH = 0;
            int intMPHCM = 0;

            int[] intMu = new int[20];
            int[] intKe = new int[20];
            int[] intCm = new int[20];
            int[] intkg = new int[20];

            int intRow = 0;

            string SqlErr = string.Empty;
            string strSql = string.Empty;
            OracleDataReader reader = null;
            string strMPH = string.Empty;

            strSql = "SELECT ITEMVALUE FROM KOSMOS_EMR.EMR_BPTINFO";
            strSql = strSql + ComNum.VBLF + "    WHERE PTNO = '" + p.ptNo + "' ";
            strSql = strSql + ComNum.VBLF + "    AND ITEMCD = 'I0000037283'";

            SqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);


            if (reader.HasRows == false)
            {
                reader.Dispose();
                Cursor.Current = Cursors.Default;
            }
            else
            {
                strMPH = reader.GetValue(0).ToString().Trim();
                reader.Dispose();
            }

            if (ssList_Sheet1.RowCount == 0)
                return;


            for (i = 0; i < 130; i++)
            {
                intY = intYstd + (5 * i);
                X = X + 1;

                for (intRow = 0; intRow < mFormFlowSheet.Length; intRow++)
                {
                    //MPH
                    if (strMPH != "")
                    {
                        if ((intKi - (i + 1)) == Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(strMPH))))
                        {
                            intMPH = intY;
                            intMPHCM = (intKi - (i + 1));
                        }
                    }

                    //키
                    if (mFormFlowSheet[intRow].ItemCode == "I0000000002")
                    {
                        for (k = 0; k < ssList_Sheet1.RowCount; k++)
                        {
                            if (ssList_Sheet1.Cells[k, intRow + 2].Text.Trim() != "")
                            {
                                if ((intKi - (i + 1)) == Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow + 2].Text.Trim()))))
                                {
                                    intKe[k - 2] = intY;
                                    intCm[k - 2] = (intKi - (i + 1));
                                }
                            }
                            else
                            {
                                ssList_Sheet1.Cells[k, intRow + 2].Text = "0";
                            }
                        }
                    }

                    //체중
                    if (mFormFlowSheet[intRow].ItemCode == "I0000000418")
                    {
                        for (k = 0; k < ssList_Sheet1.RowCount; k++)
                        {
                            if (ssList_Sheet1.Cells[k, intRow + 2].Text.Trim() != "")
                            {
                                if ((intKg - (i + 1)) == Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow + 2].Text.Trim()))))
                                {
                                    intMu[k] = intY;
                                    intkg[k] = (intKg - (i + 1));
                                }
                            }
                            else
                            {
                                ssList_Sheet1.Cells[k, intRow + 2].Text = "0";
                            }
                        }
                    }
                }

                if (X == 5)
                {
                    e.Graphics.DrawLine(new Pen(Brushes.LightGray, 2), 79, intY, 727, intY);

                    if ((intKi - (i + 1)) >= 80)
                    {
                        sDrawString(e.Graphics, (intKi - (i + 1)).ToString(), "굴림", 8, Brushes.Black, 49, intY - 5);

                        if ((intKi - (i + 1)) >= 155)
                        {
                            sDrawString(e.Graphics, (intKi - (i + 1)).ToString(), "굴림", 8, Brushes.Black, 732, intY - 5);
                        }
                        else if ((intKi - (i + 1)) == 150)
                        {
                            sDrawString(e.Graphics, "(cm)", "굴림", 8, Brushes.Black, 732, intY - 5);
                        }
                    }
                    else if ((intKi - (i + 1)) == 75)
                    {
                        sDrawString(e.Graphics, "(cm)", "굴림", 8, Brushes.Black, 44, intY - 5);
                    }

                    if ((intKg - (i + 1)) <= 85)
                    {
                        if ((intKg - (i + 1)) > 15)
                        {
                            sDrawString(e.Graphics, (intKg - (i + 1)).ToString(), "굴림", 8, Brushes.Black, 734, intY - 5);
                        }
                        else if ((intKg - (i + 1)) > 5)
                        {
                            sDrawString(e.Graphics, (intKg - (i + 1)).ToString(), "굴림", 8, Brushes.Black, 734, intY - 5);
                            sDrawString(e.Graphics, (intKg - (i + 1)).ToString(), "굴림", 8, Brushes.Black, 49, intY - 5);
                        }
                        else if ((intKg - (i + 1)) == 5)
                        {
                            sDrawString(e.Graphics, "(kg)", "굴림", 8, Brushes.Black, 44, intY - 5);
                            sDrawString(e.Graphics, "(kg)", "굴림", 8, Brushes.Black, 732, intY - 5);
                        }
                    }

                    X = 0;
                }
                else
                {
                    e.Graphics.DrawLine(new Pen(Brushes.LightGray, 1), 79, intY, 727, intY);
                }
            }

            int intXstd = 84;
            int intX = 84;
            int intAge = 2;

            for (i = 0; i < 108; i++)
            {
                intX = intXstd + (6 * i);
                X = X + 1;

                if (i == 0)
                {
                    sDrawString(e.Graphics, intAge.ToString(), "굴림", 8, Brushes.Black, intX - 11, 230);
                    sDrawString(e.Graphics, intAge.ToString(), "굴림", 8, Brushes.Black, intX - 11, 913);
                }

                if (X == 6)
                {
                    intAge = intAge + 1;

                    if (intAge != 20)
                    {
                        e.Graphics.DrawLine(new Pen(Brushes.LightGray, 2), intX, 250, intX, 900);
                    }

                    if (intAge < 20)
                    {
                        sDrawString(e.Graphics, intAge.ToString(), "굴림", 8, Brushes.Black, intX - 11, 230);
                        sDrawString(e.Graphics, intAge.ToString(), "굴림", 8, Brushes.Black, intX - 11, 913);
                    }
                    else// if (intAge == 21)
                    {
                        sDrawString(e.Graphics, intAge.ToString() + " (세)", "굴림", 8, Brushes.Black, intX - 11, 230);
                        sDrawString(e.Graphics, intAge.ToString() + " (세)", "굴림", 8, Brushes.Black, intX - 11, 913);
                    }

                    X = 0;
                }
                else
                {
                    e.Graphics.DrawLine(new Pen(Brushes.LightGray, 1), intX, 250, intX, 900);
                }
            }

            // Create pens.
            Pen MPen1 = new Pen(Color.CornflowerBlue, 2);
            Pen MPen2 = new Pen(Color.CornflowerBlue, 1);
            Pen WPen1 = new Pen(Color.HotPink, 2);
            Pen WPen2 = new Pen(Color.HotPink, 1);

            if (p.sex == "남")
            {
                Point[] curvePoints1 = { new Point(79, 722), new Point(151, 648), new Point(223, 575), new Point(295, 510), new Point(367, 447), new Point(439, 380), new Point(511, 325), new Point(583, 293), new Point(655, 282), new Point(727, 276) };
                Point[] curvePoints2 = { new Point(79, 737), new Point(151, 662), new Point(223, 590), new Point(295, 528), new Point(367, 468), new Point(439, 405), new Point(511, 343), new Point(583, 310), new Point(655, 297), new Point(727, 295) };
                Point[] curvePoints3 = { new Point(79, 752), new Point(151, 675), new Point(223, 605), new Point(295, 550), new Point(367, 493), new Point(439, 430), new Point(511, 363), new Point(583, 326), new Point(655, 317), new Point(727, 314) };
                Point[] curvePoints4 = { new Point(79, 767), new Point(151, 693), new Point(223, 617), new Point(295, 567), new Point(367, 514), new Point(439, 455), new Point(511, 393), new Point(583, 345), new Point(655, 332), new Point(727, 330) };
                Point[] curvePoints5 = { new Point(79, 782), new Point(151, 708), new Point(223, 640), new Point(295, 588), new Point(367, 538), new Point(439, 485), new Point(511, 417), new Point(583, 365), new Point(655, 353), new Point(727, 348) };
                Point[] curvePoints6 = { new Point(79, 791), new Point(151, 720), new Point(223, 653), new Point(295, 605), new Point(367, 560), new Point(439, 510), new Point(511, 448), new Point(583, 385), new Point(655, 372), new Point(727, 368) };
                Point[] curvePoints7 = { new Point(79, 798), new Point(151, 730), new Point(223, 670), new Point(295, 620), new Point(367, 578), new Point(439, 533), new Point(511, 473), new Point(583, 403), new Point(655, 390), new Point(727, 388) };

                Point[] curvePoints8 = { new Point(79, 844), new Point(151, 823), new Point(223, 785), new Point(295, 732), new Point(367, 672), new Point(439, 618), new Point(511, 558), new Point(583, 523), new Point(655, 510), new Point(727, 502) };
                Point[] curvePoints9 = { new Point(79, 851), new Point(151, 831), new Point(223, 800), new Point(295, 757), new Point(367, 705), new Point(439, 652), new Point(511, 595), new Point(583, 562), new Point(655, 547), new Point(727, 537) };
                Point[] curvePoints10 = { new Point(79, 857), new Point(151, 838), new Point(223, 814), new Point(295, 780), new Point(367, 738), new Point(439, 690), new Point(511, 630), new Point(583, 597), new Point(655, 583), new Point(727, 569) };
                Point[] curvePoints11 = { new Point(79, 862), new Point(151, 845), new Point(223, 824), new Point(295, 798), new Point(367, 767), new Point(439, 723), new Point(511, 670), new Point(583, 628), new Point(655, 613), new Point(727, 598) };
                Point[] curvePoints12 = { new Point(79, 866), new Point(151, 850), new Point(223, 832), new Point(295, 813), new Point(367, 787), new Point(439, 752), new Point(511, 703), new Point(583, 656), new Point(655, 636), new Point(727, 623) };
                Point[] curvePoints13 = { new Point(79, 870), new Point(151, 855), new Point(223, 837), new Point(295, 820), new Point(367, 800), new Point(439, 773), new Point(511, 731), new Point(583, 676), new Point(655, 656), new Point(727, 644) };
                Point[] curvePoints14 = { new Point(79, 875), new Point(151, 860), new Point(223, 844), new Point(295, 828), new Point(367, 813), new Point(439, 788), new Point(511, 753), new Point(583, 702), new Point(655, 674), new Point(727, 664) };

                e.Graphics.DrawCurve(MPen2, curvePoints1);
                e.Graphics.DrawCurve(MPen2, curvePoints2);
                e.Graphics.DrawCurve(MPen2, curvePoints3);
                e.Graphics.DrawCurve(MPen1, curvePoints4);
                e.Graphics.DrawCurve(MPen2, curvePoints5);
                e.Graphics.DrawCurve(MPen2, curvePoints6);
                e.Graphics.DrawCurve(MPen2, curvePoints7);

                e.Graphics.DrawCurve(MPen2, curvePoints8);
                e.Graphics.DrawCurve(MPen2, curvePoints9);
                e.Graphics.DrawCurve(MPen2, curvePoints10);
                e.Graphics.DrawCurve(MPen1, curvePoints11);
                e.Graphics.DrawCurve(MPen2, curvePoints12);
                e.Graphics.DrawCurve(MPen2, curvePoints13);
                e.Graphics.DrawCurve(MPen2, curvePoints14);

                //남
                sDrawString(e.Graphics, "신장(cm)", "굴림", 13, Brushes.Black, 642, 253);
                sDrawString(e.Graphics, "97", "굴림", 8, Brushes.Black, 699, 273);
                sDrawString(e.Graphics, "90", "굴림", 8, Brushes.Black, 699, 290);
                sDrawString(e.Graphics, "75", "굴림", 8, Brushes.Black, 699, 310);
                sDrawString(e.Graphics, "50", "굴림", 8, Brushes.Black, 699, 325);
                sDrawString(e.Graphics, "25", "굴림", 8, Brushes.Black, 699, 345);
                sDrawString(e.Graphics, "10", "굴림", 8, Brushes.Black, 699, 365);
                sDrawString(e.Graphics, "3", "굴림", 8, Brushes.Black, 702, 383);

                sDrawString(e.Graphics, "체중(kg)", "굴림", 13, Brushes.Black, 642, 475);
                sDrawString(e.Graphics, "97", "굴림", 8, Brushes.Black, 699, 500);
                sDrawString(e.Graphics, "90", "굴림", 8, Brushes.Black, 699, 535);
                sDrawString(e.Graphics, "75", "굴림", 8, Brushes.Black, 699, 567);
                sDrawString(e.Graphics, "50", "굴림", 8, Brushes.Black, 699, 597);
                sDrawString(e.Graphics, "25", "굴림", 8, Brushes.Black, 699, 620);
                sDrawString(e.Graphics, "10", "굴림", 8, Brushes.Black, 699, 642);
                sDrawString(e.Graphics, "3", "굴림", 8, Brushes.Black, 702, 662);

            }
            else
            {
                //여키
                Point[] curvePoints1 = { new Point(79, 725), new Point(151, 655), new Point(223, 584), new Point(295, 516), new Point(367, 442), new Point(439, 387), new Point(511, 362), new Point(583, 350), new Point(655, 347), new Point(727, 345) };
                Point[] curvePoints2 = { new Point(79, 743), new Point(151, 665), new Point(223, 600), new Point(295, 537), new Point(367, 468), new Point(439, 407), new Point(511, 376), new Point(583, 365), new Point(655, 363), new Point(727, 359) };
                Point[] curvePoints3 = { new Point(79, 754), new Point(151, 678), new Point(223, 617), new Point(295, 553), new Point(367, 495), new Point(439, 437), new Point(511, 395), new Point(583, 380), new Point(655, 378), new Point(727, 376) };
                Point[] curvePoints4 = { new Point(79, 770), new Point(151, 695), new Point(223, 630), new Point(295, 580), new Point(367, 520), new Point(439, 455), new Point(511, 413), new Point(583, 400), new Point(655, 395), new Point(727, 390) };
                Point[] curvePoints5 = { new Point(79, 782), new Point(151, 710), new Point(223, 645), new Point(295, 594), new Point(367, 542), new Point(439, 480), new Point(511, 436), new Point(583, 417), new Point(655, 411), new Point(727, 408) };
                Point[] curvePoints6 = { new Point(79, 795), new Point(151, 726), new Point(223, 664), new Point(295, 613), new Point(367, 566), new Point(439, 508), new Point(511, 457), new Point(583, 436), new Point(655, 429), new Point(727, 427) };
                Point[] curvePoints7 = { new Point(79, 806), new Point(151, 742), new Point(223, 675), new Point(295, 627), new Point(367, 585), new Point(439, 530), new Point(511, 470), new Point(583, 447), new Point(655, 445), new Point(727, 443) };

                //여체중
                Point[] curvePoints8 = { new Point(79, 848), new Point(151, 825), new Point(223, 791), new Point(295, 745), new Point(367, 685), new Point(439, 624), new Point(511, 597), new Point(583, 583), new Point(655, 578), new Point(727, 574) };
                Point[] curvePoints9 = { new Point(79, 853), new Point(151, 830), new Point(223, 804), new Point(295, 765), new Point(367, 707), new Point(439, 651), new Point(511, 625), new Point(583, 615), new Point(655, 608), new Point(727, 606) };
                Point[] curvePoints10 = { new Point(79, 856), new Point(151, 842), new Point(223, 820), new Point(295, 787), new Point(367, 745), new Point(439, 700), new Point(511, 654), new Point(583, 640), new Point(655, 637), new Point(727, 633) };
                Point[] curvePoints11 = { new Point(79, 865), new Point(151, 850), new Point(223, 830), new Point(295, 807), new Point(367, 773), new Point(439, 722), new Point(511, 682), new Point(583, 664), new Point(655, 657), new Point(727, 652) };
                Point[] curvePoints12 = { new Point(79, 869), new Point(151, 856), new Point(223, 835), new Point(295, 815), new Point(367, 790), new Point(439, 753), new Point(511, 718), new Point(583, 686), new Point(655, 672), new Point(727, 667) };
                Point[] curvePoints13 = { new Point(79, 872), new Point(151, 860), new Point(223, 841), new Point(295, 825), new Point(367, 803), new Point(439, 774), new Point(511, 728), new Point(583, 700), new Point(655, 689), new Point(727, 679) };
                Point[] curvePoints14 = { new Point(79, 876), new Point(151, 864), new Point(223, 852), new Point(295, 835), new Point(367, 815), new Point(439, 788), new Point(511, 750), new Point(583, 715), new Point(655, 700), new Point(727, 685) };

                //여
                e.Graphics.DrawCurve(WPen2, curvePoints1);
                e.Graphics.DrawCurve(WPen2, curvePoints2);
                e.Graphics.DrawCurve(WPen2, curvePoints3);
                e.Graphics.DrawCurve(WPen1, curvePoints4);
                e.Graphics.DrawCurve(WPen2, curvePoints5);
                e.Graphics.DrawCurve(WPen2, curvePoints6);
                e.Graphics.DrawCurve(WPen2, curvePoints7);

                e.Graphics.DrawCurve(WPen2, curvePoints8);
                e.Graphics.DrawCurve(WPen2, curvePoints9);
                e.Graphics.DrawCurve(WPen2, curvePoints10);
                e.Graphics.DrawCurve(WPen1, curvePoints11);
                e.Graphics.DrawCurve(WPen2, curvePoints12);
                e.Graphics.DrawCurve(WPen2, curvePoints13);
                e.Graphics.DrawCurve(WPen2, curvePoints14);

                //여
                sDrawString(e.Graphics, "신장(cm)", "굴림", 13, Brushes.Black, 642, 310);
                sDrawString(e.Graphics, "97", "굴림", 8, Brushes.Black, 699, 341);
                sDrawString(e.Graphics, "90", "굴림", 8, Brushes.Black, 699, 356);
                sDrawString(e.Graphics, "75", "굴림", 8, Brushes.Black, 699, 371);
                sDrawString(e.Graphics, "50", "굴림", 8, Brushes.Black, 699, 387);
                sDrawString(e.Graphics, "25", "굴림", 8, Brushes.Black, 699, 404);
                sDrawString(e.Graphics, "10", "굴림", 8, Brushes.Black, 699, 422);
                sDrawString(e.Graphics, "3", "굴림", 8, Brushes.Black, 702, 438);

                sDrawString(e.Graphics, "체중(kg)", "굴림", 13, Brushes.Black, 642, 545);
                sDrawString(e.Graphics, "97", "굴림", 8, Brushes.Black, 699, 570);
                sDrawString(e.Graphics, "90", "굴림", 8, Brushes.Black, 699, 602);
                sDrawString(e.Graphics, "75", "굴림", 8, Brushes.Black, 699, 629);
                sDrawString(e.Graphics, "50", "굴림", 8, Brushes.Black, 699, 649);
                sDrawString(e.Graphics, "25", "굴림", 8, Brushes.Black, 699, 663);
                sDrawString(e.Graphics, "10", "굴림", 8, Brushes.Black, 699, 676);
                sDrawString(e.Graphics, "3", "굴림", 8, Brushes.Black, 702, 687);
            }

            //나이 년 개월
            int[] intYear = new int[20];
            int[] intMonth = new int[20];
            int[] intNae = new int[20];

            //뼈나이 년 개월
            int[] intBoneYear = new int[20];
            int[] intBoneMonth = new int[20];
            int[] intBoneNae = new int[20];

            for (intRow = 0; intRow < mFormFlowSheet.Length; intRow++)
            {
                //키
                if (ssList_Sheet1.Cells[0, intRow].Text.Trim() == "I0000000002")
                {
                    for (k = 0; k < ssList_Sheet1.RowCount; k++)
                    {
                        if (ssList_Sheet1.Cells[k, intRow].Text.Trim() != "")
                        {
                            if ((intKi - (i + 1)) == Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow].Text.Trim()))))
                            {
                                intKe[k] = intY;
                            }
                        }
                        else
                        {
                            ssList_Sheet1.Cells[k, intRow].Text = "0";
                        }
                    }
                }
            }

            for (intRow = 0; intRow < mFormFlowSheet.Length; intRow++)
            {
                //년
                if (mFormFlowSheet[intRow].ItemCode == "I0000015052")
                {
                    for (k = 0; k < ssList_Sheet1.RowCount; k++)
                    {
                        if (ssList_Sheet1.Cells[k, intRow].Text.Trim() != "")
                        {
                            intYear[k] = Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow].Text.Trim())));
                        }
                    }
                }

                //개월
                if (mFormFlowSheet[intRow].ItemCode == "I0000002129")
                {
                    for (k = 0; k < ssList_Sheet1.RowCount; k++)
                    {
                        if (ssList_Sheet1.Cells[k, intRow].Text.Trim() != "")
                        {
                            intMonth[k] = Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow].Text.Trim())));
                        }
                    }
                }

                //뼈나이(년) I0000031665_0
                if (mFormFlowSheet[intRow].ItemCode == "I0000037284")
                {
                    for (k = 0; k < ssList_Sheet1.RowCount; k++)
                    {
                        if (ssList_Sheet1.Cells[k, intRow].Text.Trim() != "")
                        {
                            intBoneYear[k] = Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow].Text.Trim())));
                        }
                    }
                }

                //뼈나이(개월) I0000031665_1
                if (mFormFlowSheet[intRow].ItemCode == "I0000037285")
                {
                    for (k = 0; k < ssList_Sheet1.RowCount; k++)
                    {
                        if (ssList_Sheet1.Cells[k, intRow].Text.Trim() != "")
                        {
                            intBoneMonth[k] = Convert.ToInt32(ComFunc.TruncToDbl(Convert.ToDouble(ssList_Sheet1.Cells[k, intRow].Text.Trim())));
                        }
                    }
                }
            }

            for (intRow = 0; intRow < mFormFlowSheet.Length; intRow++)
            {
                //나이
                intNae[intRow] = (35 * (Convert.ToInt32(intYear[intRow]) - 1) + 3) + (Convert.ToInt32(intMonth[intRow]) * 3);

                //뼈나이
                intBoneNae[intRow] = (35 * (Convert.ToInt32(intBoneYear[intRow]) - 1) + 3) + (Convert.ToInt32(intBoneMonth[intRow]) * 3);

                if (intKe[intRow] > 0)
                {
                    if (intBoneNae[intRow] - 2 > 0)
                    {
                        int intNaegu = 0;
                        int intNaeguEnd = 0;

                        //if (intNae[intRow - 4] + 5 + 39 > intBoneNae[intRow - 4] + 5 + 39)
                        //{
                        //    intNaegu = (intNae[intRow - 4] + 5 + 39) - (intBoneNae[intRow - 4] + 5 + 39);
                        //    intNaegu = intNaegu / 5;
                        //    intNaegu = (intNae[intRow - 4] + 5 + 39) - intNaegu;
                        //    intNaeguEnd = intNae[intRow - 4] + 5 + 39;
                        //}
                        //else
                        //{
                        intNaegu = (intBoneNae[intRow] + 5 + 39) - (intNae[intRow] + 5 + 39);
                        intNaegu = intNaegu / 5;
                        intNaegu = (intBoneNae[intRow] + 5 + 39) - intNaegu;
                        intNaeguEnd = intBoneNae[intRow] + 5 + 39;
                        //}

                        //나이 - 뼈나이 사이 선
                        e.Graphics.DrawLine(new Pen(Color.Red, 2), intNae[intRow] + 5 + 39, intKe[intRow], intBoneNae[intRow] + 5 + 39, intKe[intRow]);
                        e.Graphics.DrawLine(new Pen(Color.Red, 2), intNaegu, intKe[intRow] - 4, intNaegu, intKe[intRow] + 4);
                        e.Graphics.DrawLine(new Pen(Color.Red, 2), intNaegu, intKe[intRow] - 4, intNaeguEnd, intKe[intRow]);
                        e.Graphics.DrawLine(new Pen(Color.Red, 2), intNaegu, intKe[intRow] + 4, intNaeguEnd, intKe[intRow]);

                        //뼈나이
                        e.Graphics.FillEllipse(Brushes.BlueViolet, intBoneNae[intRow] + 5 + 39, intKe[intRow] - 3, 6, 6);
                        sDrawString(e.Graphics, "(" + intBoneYear[intRow] + "." + intBoneMonth[intRow] + ", " + intCm[intRow] + ")", "굴림", 8, Brushes.BlueViolet, intBoneNae[intRow] - 30 + 39, intKe[intRow] + 20);
                    }

                    //나이
                    e.Graphics.FillEllipse(Brushes.Black, intNae[intRow] + 5 + 39, intKe[intRow] - 3, 6, 6);
                    sDrawString(e.Graphics, "(" + intYear[intRow] + "." + intMonth[intRow] + ", " + intCm[intRow] + ")", "굴림", 8, Brushes.Black, intNae[intRow] - 30 + 39, intKe[intRow] + 10);
                }

                if (intMu[intRow] > 0)
                {
                    //if (intBoneNae[intRow - 4] - 2 > 0)
                    //{
                    //    //나이 - 뼈나이 사이 선
                    //    e.Graphics.DrawLine(new Pen(Color.Red, 2), intNae[intRow - 4] + 5, intMu[intRow - 4], intBoneNae[intRow - 4] + 5, intMu[intRow - 4]);
                    //}

                    //나이
                    e.Graphics.FillEllipse(Brushes.Black, intNae[intRow] + 5 + 39, intMu[intRow] - 3, 6, 6);
                    sDrawString(e.Graphics, "(" + intYear[intRow] + "." + intMonth[intRow] + ", " + intkg[intRow] + ")", "굴림", 8, Brushes.Black, intNae[intRow] - 30 + 39, intMu[intRow] + 10);

                    //뼈나이
                    //e.Graphics.FillEllipse(Brushes.BlueViolet, intBoneNae[intRow - 4] + 5, intMu[intRow - 4] - 3, 6, 6);
                    //sDrawString(e.Graphics, "(" + intBoneYear[intRow - 4] + "." + intBoneMonth[intRow - 4] + ", " + intkg[intRow - 4] + ")", "굴림", 8, Brushes.BlueViolet, intBoneNae[intRow - 4] - 30, intMu[intRow - 4] + 20);
                }
            }

            if (intMPH > 0)
            {
                e.Graphics.DrawLine(new Pen(Color.Violet, 2), 40 + 39, intMPH, 688 + 39, intMPH);
                sDrawString(e.Graphics, intMPHCM.ToString() + " ± 10", "굴림", 9, Brushes.Black, 50 + 39, intMPH - 12);
            }

            cPen.Dispose();
            e.Graphics.Dispose();

        }

        private void sDrawString(Graphics graphics, string str, string FontName, float FontSize, Brush brushes, int x, int y, StringFormat stringFormat = null)
        {
            using (Font font = new Font(FontName, FontSize))
            {
                if (stringFormat != null)
                {
                    graphics.DrawString(str, font, brushes, x, y, stringFormat);
                }
                else
                {
                    graphics.DrawString(str, font, brushes, x, y);
                }
            }
        }

        private void CaptureScreen() //폼 캡쳐
        {
            Graphics mygraphics = this.panGraph.CreateGraphics();
            Size s = this.panGraph.Size;
            memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            IntPtr dc1 = mygraphics.GetHdc();
            IntPtr dc2 = memoryGraphics.GetHdc();
            BitBlt(dc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 0, 13369376);
            mygraphics.ReleaseHdc(dc1);
            memoryGraphics.ReleaseHdc(dc2);
        }

        private void printDocument1_PrintPage(System.Object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }

        public void SetUserFormMsg(double dblMACRONO)
        {
            throw new NotImplementedException();
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            throw new NotImplementedException();
        }

        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            throw new NotImplementedException();
        }
    }
}
