using ComBase;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// 수혈기록지
    /// FORMNO = 1567
    /// \mtsEmrf\frmTextEmrTRFS.frm
    /// </summary>
    public partial class frmTextEmrTRFS : Form, EmrChartForm
    {
        #region // TopMenu관련 선언
        private usFormTopMenu usFormTopMenuEvent;
        private usTimeSet usTimeSetEvent;
        public ComboBox mMaskBox = null;
        #endregion

        #region //폼에서 사용하는 변수
        int mCallFormGb = 0;  //차트작성 0, 기록지등록에서 호출 1,
        int nCol1 = 0;
        int nCol2 = 0;
        int nCol3 = 0;
        int nCol4 = 0;

        /// <summary>
        /// 차트작성 일자.
        /// </summary>
        string mstrOrderDate = string.Empty;

        #endregion //폼에서 사용하는 변수

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //Form mCallForm = null;
        FormEmrMessage mEmrCallForm = null;
        EmrPatient pAcp = null;
        //EmrForm pForm = null;

        bool mLoading = false;

        public string mstrFormNo = "1567";
        public string mstrUpdateNo = "1";
        public string mstrFormText = "";
        public string mstrEmrNo = "42694720";  //961 131641  //963 735603
        public string mstrMode = "W";

        private Dictionary<string, string> pstrBlood = new Dictionary<string, string>();
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

            //usFormTopMenuEvent.dtMedFrDate.ValueChanged += new EventHandler(dtMedFrDate_ValueChanged);

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
            usFormTopMenuEvent.mbtnSave.Enabled = false;
            pSaveData();
            usFormTopMenuEvent.mbtnSave.Enabled = true;
        }

        private void usFormTopMenuEvent_SetDel(string strFrDate, string strFrTime)
        {
            if(pDelData())
            {
                pClearForm();
                GetChartHistory();
            }
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

        private void dtMedFrDate_ValueChanged(object sender, EventArgs e)
        {
            if (mLoading == false)
                return;

            string strdtpDate = usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd");
            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            if (pAcp.medEndDate != "")
            {
                if ((VB.Val(pAcp.medFrDate) > VB.Val(strdtpDate)) || (VB.Val(pAcp.medEndDate) < VB.Val(strdtpDate)))
                {
                    ComFunc.MsgBoxEx(this, "재원 기간을 넘어서는 작성 할수 없습니다.");
                    usFormTopMenuEvent.dtMedFrDate.Value = DateTime.ParseExact(pAcp.medEndDate, "yyyyMMdd", null);
                }
            }
            else
            {
                if ((VB.Val(pAcp.medFrDate) > VB.Val(strdtpDate)) || (VB.Val(strCurDate) < VB.Val(strdtpDate)))
                {
                    ComFunc.MsgBoxEx(this, "재원 기간을 넘어서는 작성 할수 없습니다.");
                    usFormTopMenuEvent.dtMedFrDate.Value = DateTime.ParseExact(strCurDate, "yyyyMMdd", null);
                }
            }
        }

        #endregion

        #region //EmrChartForm        

        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }

        /// <summary>
        /// 권한별 환경을 설정을 한다(작성화면 안보이게 등)
        /// </summary>
        private void SetUserAut()
        {
            if (clsType.User.AuAWRITE == "1")
            {
                panTopMenu.Visible = true;
                //panWrite.Visible = true;
            }
            else
            {
                panTopMenu.Visible = false;
                //panWrite.Visible = false;
            }

            if (clsType.User.AuAPRINTIN == "1")
            {
                usFormTopMenuEvent.mbtnPrint.Visible = true;
            }
            else
            {
                usFormTopMenuEvent.mbtnPrint.Visible = true;
            }
        }

        /// <summary>
        /// 사용자 템플릿 뷰어용
        /// </summary>
        public void pInitFormTemplet()
        {
            SetTopMenuLoad();
        }


        #endregion

        #region //Private Function 기록지 클리어, 저장, 삭제, 프린터

        /// <summary>
        /// 스프래드 클리어
        /// </summary>
        private void pClearForm()
        {
            mstrEmrNo = "0";
            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            usFormTopMenuEvent.txtMedFrTime.Text = usFormTopMenuEvent.dtMedFrDate.Value.ToString("HH:mm");
            usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            usFormTopMenuEvent.txtMedFrTime.Enabled = true;
            //ClearForm();
            //ClearSp();
            //ClearChart();
        }

        // <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private double pSaveEmrData()
        {
            double dblEmrNo = 0;

            if (pAcp.ptNo.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "환자를 선택해 주십시오.");
                return dblEmrNo;
            }

            if (usFormTopMenuEvent.txtMedFrTime.Text.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "시간을 설정해 주십시오.");
                return dblEmrNo;
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
            }
            #endregion

            if (clsEmrQuery.READ_CHART_APPLY(this, mstrEmrNo))
                return VB.Val(mstrEmrNo);

            if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo))
                return VB.Val(mstrEmrNo);

            string strDate = string.Format("{0} {1}:{2}",
                usFormTopMenuEvent.dtMedFrDate.Value.ToShortDateString(),
                VB.Left(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2),
                VB.Right(usFormTopMenuEvent.txtMedFrTime.Text.Trim(), 2));

            if (!VB.IsDate(strDate))
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return dblEmrNo;
            }

            if (SaveUserChart(13, ss1567, "1567") && SaveUserChart(5, ss1965, "1965"))
            {
                ComFunc.MsgBoxEx(this, "간호기록지 저장이 완료되었습니다.");
                pClearForm();
                GetChartHistory();
            }

            return dblEmrNo;
        }

        bool SaveUserChart(int lngStartCol, FpSpread objSpread, string strFormNo)
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

                if (objSpread.ActiveSheet.RowCount == 0)
                {
                    return rtnVal;
                }

                StringBuilder XMLBuilder = new StringBuilder();
                string strUPDATENO = clsEmrQuery.GetMaxUpdateNo(clsDB.DbCon, VB.Val(strFormNo)).ToString();

                Cursor.Current = Cursors.WaitCursor;
                string strInOutCls = pAcp.inOutCls;

                for (int i = 0; i < objSpread.ActiveSheet.RowCount; i++)
                {
                    if (objSpread.ActiveSheet.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        string strChartDate = objSpread.ActiveSheet.Cells[i, 3].Text.Trim().Replace("-", "");
                        strChartDate = strChartDate.Length == 0 ? usFormTopMenuEvent.dtMedFrDate.Value.ToString("yyyyMMdd") : strChartDate;
                        string strChartTime = objSpread.ActiveSheet.Cells[i, 4].Text.Trim().Replace(":", "");
                        strChartTime = strChartTime.Length == 0 ? usFormTopMenuEvent.txtMedFrTime.Text.Replace(":", "") : strChartTime;
                        double dblEmrNo = VB.Val(objSpread.ActiveSheet.Cells[i, 1].Text.Trim());

                        if (dblEmrNo != 0)
                        {
                            if (clsEmrQuery.READ_CHART_APPLY(this, dblEmrNo.ToString()))
                            {
                                return rtnVal;
                            }

                            if (clsEmrQuery.READ_PRTLOG(this, dblEmrNo.ToString()))
                            {
                                return rtnVal;
                            }

                            if (clsEmrQuery.DeleteEmrXmlData(dblEmrNo.ToString()) == false)
                            {
                                return rtnVal;
                            }

                        }

                        dblEmrNo = ComQuery.GetSequencesNo(clsDB.DbCon, "GetEmrXmlNo");

                        #region XML 만듫기
                        XMLBuilder.Clear();
                        XMLBuilder.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                        XMLBuilder.AppendLine("<chart>");

                        for (int j = lngStartCol; j < objSpread.ActiveSheet.ColumnCount; j++)
                        {
                            if (objSpread.ActiveSheet.Columns[j].Tag != null)
                            {
                                XMLBuilder.AppendLine("<" + objSpread.ActiveSheet.Columns[j].Tag.ToString() + " type=\"inputText\" label=\"" + objSpread.ActiveSheet.Columns[j].Label
                                + "\"><![CDATA[" + objSpread.ActiveSheet.Cells[i, j].Text.Trim().Replace("'", "`") + "]]><" + "/" + objSpread.ActiveSheet.Columns[j].Tag.ToString() + ">");
                            }
                        }
                        XMLBuilder.AppendLine("</chart>");
                        clsEmrQuery.SaveEmrXmlData(dblEmrNo.ToString(), strFormNo, strChartDate, strChartTime,
                            pAcp.acpNo, pAcp.ptNo, strInOutCls, pAcp.medFrDate, pAcp.medFrTime,
                            pAcp.medEndDate, pAcp.medEndTime, pAcp.medDeptCd, pAcp.medDrCd,
                            XMLBuilder.ToString().Trim(), strUPDATENO);
                        #endregion

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


        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        private bool pDelData()
        {
            bool rtnVal = DeleteProgress();

            return rtnVal;
        }

        bool DeleteProgress()
        {
            bool rtnVal = false;

            for (int i = 0; i < ss1965_Sheet1.RowCount; i++)
            {
                if (ss1965_Sheet1.Cells[i, 0].Text.Trim().Equals("True") == false)
                    continue;

                double dEmrNo = VB.Val(ss1965_Sheet1.Cells[i, 1].Text.Trim());

                if (dEmrNo != 0)
                {
                    //string strUseId = ss1965_Sheet1.Cells[i, 2].Text.Trim();

                    //if (strUseId.Trim() != clsType.User.IdNumber)
                    //{
                    //    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "삭제할 수 없습니다.");
                    //    return rtnVal;
                    //}

                    if (clsEmrQuery.READ_CHART_APPLY(this, dEmrNo.ToString()))
                    {
                        return rtnVal;
                    }

                    if (clsEmrQuery.READ_PRTLOG(this, dEmrNo.ToString()))
                    {
                        return rtnVal;
                    }

                    if (ComFunc.MsgBoxQ("기존내용을 삭제하시겠습니까?") == DialogResult.No)
                    {
                        return rtnVal;
                    }


                    if (clsEmrQuery.DeleteEmrXmlData(dEmrNo.ToString()) == false)
                    {
                        ComFunc.MsgBoxEx(this, "삭제도중 오류가 발생했습니다.");
                        return rtnVal;
                    }

                }

                Cursor.Current = Cursors.WaitCursor;
                rtnVal = clsEmrQuery.DeleteEmrXmlData(mstrEmrNo);
                Cursor.Current = Cursors.Default;
            }

            rtnVal = true;
            return rtnVal;
        }

        /// <summary>
        /// 출력
        /// </summary>
        private void pPrintForm()
        {
            using (frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption())
            {
                frmEmrPrintOptionX.ShowDialog();

                if (clsFormPrint.mstrPRINTFLAG == "-1")
                {
                    return;
                }

                PrintCopy("V");
            }
        }


        #endregion //기록지 클리어, 저장, 삭제, 프린터

        #region //Public Function 외부에서 이벤트 받아서 처리 클리어, 저장, 삭제, 프린터
        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = 0;
            //isReciveOrderSave = true;
            //dblEmrNo = pSaveData(strFlag);
            //isReciveOrderSave = false;
            return dblEmrNo;
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double pSaveData()
        {
            return pSaveEmrData();
        }


        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        public bool DelDataMsg()
        {
            return pDelData();
        }

        /// <summary>
        /// 클리어
        /// </summary>
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
            //if (strPRINTFLAG == "N")
            //{
            //    frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
            //    frmEmrPrintOptionX.ShowDialog();
            //}

            //if (clsFormPrint.mstrPRINTFLAG == "-1")
            //{
            //    return rtnVal;
            //}

            //if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            //{ 
            //    return rtnVal;
            //}

            //rtnVal = clsFormPrint.PrintFormLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //rtnVal = clsFormPrint.PrintToTifFileLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        #endregion

        #region //생성자

        public frmTextEmrTRFS(string strFormNo, string strUpdateNo, string strEmrNo, int pCallFormGb)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mCallFormGb = pCallFormGb;

            pInitFormTemplet();
        }


        /// <summary>
        /// 19-07-05 신규 생성자 
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="po"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strMode"></param>
        /// <param name="strOrderDate"></param>
        /// <param name="pEmrCallForm"></param>
        public frmTextEmrTRFS(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, string strOrderDate, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            mstrOrderDate = strOrderDate;
            InitializeComponent();

            pInitFormTemplet();
        }

        public frmTextEmrTRFS(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            InitializeComponent();

            pInitFormTemplet();
        }

        #endregion //생성자

        #region 함수
        void PrintCopy(string PrintType)
        {
            DataTable dt = null;
            OracleDataReader reader = null;

            string strPatName = string.Empty;
            string strSex = string.Empty;
            string strAge = string.Empty;
            string strJumin = string.Empty;

            try
            {
                string SQL = " SELECT SNAME, SEX, JUMIN1, JUMIN2 ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + pAcp.ptNo + "' ";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strPatName = dt.Rows[0]["SNAME"].ToString().Trim();
                    strSex = dt.Rows[0]["SEX"].ToString().Trim();
                    strAge = ComFunc.AgeCalc(clsDB.DbCon,
                        dt.Rows[0]["JUMIN1"].ToString().Trim() +
                        dt.Rows[0]["JUMIN2"].ToString().Trim()).ToString();
                    strJumin = string.Format("{0}-{1}******",
                       dt.Rows[0]["JUMIN1"].ToString().Trim(),
                       VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1));

                }

                dt.Dispose();

                string strFormName = string.Empty;
                SQL = " SELECT FORMNAME ";
                SQL = SQL += ComNum.VBLF + " FROM KOSMOS_EMR.AEMRFORM ";
                SQL = SQL += ComNum.VBLF + "WHERE FORMNO = " + VB.Val(mstrFormNo);

                sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    strFormName = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();

                //'Print Head 지정
                //string strFont1 = @"/fn""바탕체"" /fz""14"" /fb1 /fi0 /fu0 /fk0 /fs1";
                //string strFont2 = @"/fn""바탕체"" /fz""8"" /fb0 /fi0 /fu0 /fk0 /fs2";
                string strHead1 = "/c/f1" + strFormName + "/f1/n/n";
                string strHead2 = "/n/l/f2" + "환자번호 : " + pAcp.ptNo +
                                  VB.Space(5) + "환자성명 : " + strPatName + VB.Space(5) + "성별/나이 : " + strSex + "/" + strAge + "세              주민번호 :  " + strJumin + "/n/n";
                string strFooter = "/n/l/f2" + "포항성모병원" + VB.Space(10) + "출력일자 : " + VB.Format(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "@@@@-@@-@@") + VB.Space(10) + "출력자 : " + clsType.User.UserName;
                strFooter = strFooter + "/n/l/f2" + "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.";
                strFooter = strFooter + "/n/l/f2" + "This is an electronically authorized offical medical record";

                //ssUserChartHisIO_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
                //ssUserChartHisIO_Sheet1.PrintInfo.Footer = strFooter;
                //ssUserChartHisIO_Sheet1.PrintInfo.Margin.Left = 20;
                //ssUserChartHisIO_Sheet1.PrintInfo.Margin.Right = 20;
                //ssUserChartHisIO_Sheet1.PrintInfo.Margin.Top = 20;
                //ssUserChartHisIO_Sheet1.PrintInfo.Margin.Bottom = 20;

                //ssUserChartHisIO_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
                //ssUserChartHisIO_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
                //ssUserChartHisIO_Sheet1.PrintInfo.ShowBorder = true;
                //ssUserChartHisIO_Sheet1.PrintInfo.ShowColor = false;
                //ssUserChartHisIO_Sheet1.PrintInfo.ShowGrid = true;
                //ssUserChartHisIO_Sheet1.PrintInfo.ShowShadows = false;
                //ssUserChartHisIO_Sheet1.PrintInfo.UseMax = false;
                //ssUserChartHisIO_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                //ssUserChartHisIO_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;

                SQL = " SELECT PRTGB ";
                SQL = SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRFORM ";
                SQL = SQL += ComNum.VBLF + "WHERE FORMNO = " + VB.Val(mstrFormNo);

                sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    //switch (reader.GetValue(0).ToString().Trim())
                    //{
                    //    //case "L":
                    //    //    ssUserChartHisIO_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
                    //    //    break;
                    //    //case "P":
                    //    //    ssUserChartHisIO_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                    //    //    break;
                    //    //case "S":
                    //    //    ssUserChartHisIO_Sheet1.PrintInfo.UseSmartPrint = true;
                    //    //    break;
                    //}
                }
                reader.Dispose();


                if (PrintType == "V")
                {
                    //ssUserChartHisIO_Sheet1.PrintInfo.Preview = true;
                }

                //ssUserChartHisIO.PrintSheet(0);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }
        #endregion

        private void frmTextEmrTRFS_Load(object sender, EventArgs e)
        {
            pClearForm();

            clsEmrChart.SetChartHead(clsDB.DbCon, usFormTopMenuEvent, pAcp);

            if (mCallFormGb != 1)
            {
                SetUserAut();
            }

            if(pAcp.medFrDate.Length == 0)
            {
                dtpOptSDate.Value = Convert.ToDateTime(clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.IdNumber, ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10)));
            }
            else
            {
                dtpOptSDate.Value = Convert.ToDateTime(clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.IdNumber, DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToShortDateString()));
            }


            if (pAcp.medEndDate.Length == 0)
            {
                dtpOptEDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            }
            else
            {
                dtpOptEDate.Value = DateTime.ParseExact(pAcp.medEndDate, "yyyyMMdd", null);
            }

            GetChartHistory();

            mLoading = true;


        }

        #region 조회 함수
        void GetChartHistory()
        {
            #region 권한 버튼
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);

            //초기값 세팅
            if (VB.Val(mstrEmrNo) <= 0)
            {
                usFormTopMenuEvent.mbtnPrint.Visible = false;
                usFormTopMenuEvent.mbtnPrintNull.Visible = true;
            }
            else if (VB.Val(mstrEmrNo) > 0)
            {
                //if (clsEmrQuery.IsChangeAuthOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false)
                //{
                //    usFormTopMenuEvent.mbtnSave.Visible = false;
                //    usFormTopMenuEvent.mbtnSaveTemp.Visible = false;
                //    usFormTopMenuEvent.mbtnDelete.Visible = false;
                //}
            }

            //사본발급 출력여부
            usFormTopMenuEvent.lblPrntYn.Visible = clsEmrQuery.READ_PRTLOG2(mstrEmrNo);
            usFormTopMenuEvent.mbtnSaveTemp.Visible = false;
            #endregion

            MakeUserChart("1567", 13, ss1567);
            MakeUserChart("1965", 5, ss1965);

            ss1567_Sheet1.Columns[15].CellType = new DateTimeCellType();

            for(int i = 0; i < ss1965_Sheet1.ColumnCount; i++)
            {
                switch(ss1965_Sheet1.Columns[i].Label)
                {
                    case "수혈량":  nCol1 = i; break;
                    case "비고":  nCol2 = i;  break;
                    case "성분":  nCol3 = i; break;
                    case "혈액정보":  nCol4 = i; break;
                    //case "부작용(15분이내)":  nCol5 = i; break;
                    //case "부작용(종료 시)":  nCol6 = i; break;
                }
            }

            ss1965_Sheet1.Columns[nCol3, nCol4].Visible = false;
            ss1965_Sheet1.Columns[0].Locked = true;
            ss1965_Sheet1.Columns[9].Locked = true;

            GetUchartHis(13, pAcp.ptNo, "1567", dtpOptSDate.Value.ToString("yyyyMMdd"), dtpOptEDate.Value.ToString("yyyyMMdd"),  ss1567);
            GetDefult(pAcp.ptNo, pAcp.medFrDate);

            ss1965_Sheet1.RowCount = 0;


            GetUchartHis(5, pAcp.ptNo, "1965", dtpOptSDate.Value.ToString("yyyyMMdd"), dtpOptEDate.Value.ToString("yyyyMMdd"),  ss1965);
            GetBloodIO(pAcp.ptNo, dtpOptSDate.Value.ToString("yyyyMMdd"), dtpOptEDate.Value.ToString("yyyyMMdd"));

            //READ_INIT_FALL();
        }

        private void GetUchartHis(int lngStartCol, string strPtNo, string strFormNo, string strSDate, string strEDATE, FarPoint.Win.Spread.FpSpread objSpread, 
            string strChartTime = "", string ArgInOutCls = "")
        {
            StringBuilder strXmlCol = new StringBuilder();
            OracleDataReader oracleDataReader = null;
            DataTable dt = null;
            int nXmlCnt = 0;

            #region XML 값 만들기
            string SQL = " SELECT 'EXTRACTVALUE(CHARTXML,' ||  CHR(39)  || '/chart/' ||  CONTROLID  || CHR(39) || ') as ' ";
            SQL += "  || chr(34) || 'XML' ||itemno || chr(34)    as XMLCOL  ";
            SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMROPTFORM";
            //'SQL = SQL & " WHERE FORMNO ='1965'";
            SQL += ComNum.VBLF + "  WHERE FORMNO = " + strFormNo;
            SQL += ComNum.VBLF + " ORDER BY ITEMNO";

            string sqlErr = clsDB.GetAdoRs(ref oracleDataReader, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, sqlErr);
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                return;
            }

            if(oracleDataReader.HasRows)
            {
                while(oracleDataReader.Read())
                {
                    strXmlCol.AppendLine(oracleDataReader.GetValue(0).ToString().Trim()).Append(",");
                    nXmlCnt++;
                }
            }

            oracleDataReader.Dispose();
            #endregion

            #region 진짜 쿼리
            SQL = " SELECT  " + strXmlCol.Replace("\"",  "") + " A.EMRNO, ";
            SQL += ComNum.VBLF + "    A.ACPNO, A.INOUTCLS, A.CHARTDATE, A.CHARTTIME, ";
            SQL += ComNum.VBLF + "    A.MEDDEPTCD, A.MEDDRCD, A.USEID, ";
            SQL += ComNum.VBLF + "    A.MEDFRDATE, A.MEDFRTIME,  B.FORMNO, B.FORMNAME1 FORMNAME,  B.USERFORMNO, T.NAME ";
            SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMRXML A";
            SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMRXMLMST D";
            SQL += ComNum.VBLF + "     ON A.EMRNO = D.EMRNO";
            SQL += ComNum.VBLF + "     AND D.FORMNO = " + strFormNo;
            SQL += ComNum.VBLF + "     AND D.PTNO = '" + strPtNo + "' ";
            SQL += ComNum.VBLF + "     AND D.MEDFRDATE = '" + strSDate + "' ";
            SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMRFORM B";
            SQL += ComNum.VBLF + "     ON B.FORMNO = D.FORMNO";
            SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMR_USERT T";
            SQL += ComNum.VBLF + "     ON D.USEID = T.USERID";
            SQL += ComNum.VBLF + "  ORDER BY A.CHARTDATE DESC, TRIM(A.CHARTTIME) DESC";
            try
            {
                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if(dt.Rows.Count == 0 )
                {
                    dt.Dispose();
                    return;
                }

                objSpread.ActiveSheet.RowCount = dt.Rows.Count;
                int Col = lngStartCol;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objSpread.ActiveSheet.Cells[i, 0].Locked = false;
                    objSpread.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    objSpread.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["USEID"].ToString().Trim();
                    objSpread.ActiveSheet.Cells[i, 3].Text = VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000-00-00");
                    objSpread.ActiveSheet.Cells[i, 4].Text = VB.Val(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4)).ToString("00:00");

                    Col = lngStartCol;
                    for(int j = 0; j < nXmlCnt; j++)
                    {
                        objSpread.ActiveSheet.Cells[i, Col].Text = dt.Rows[i]["xml" + (j + 1).ToString("0")].ToString().Trim();
                        if (objSpread.ActiveSheet.Columns[Col].Label.Equals("진단명"))
                        {
                            objSpread.ActiveSheet.Columns[Col].Width = 310;
                        }
                        else if (objSpread.ActiveSheet.Columns[Col].Label.Equals("수혈전투약"))
                        {
                            objSpread.ActiveSheet.Columns[Col].Width = 110;
                        }
                        else
                        {
                            objSpread.ActiveSheet.Columns[Col].Width = objSpread.ActiveSheet.Columns[Col].GetPreferredWidth();
                        }
                        Col++;
                    }

                    objSpread.ActiveSheet.ColumnCount = Col + 1;
                    objSpread.ActiveSheet.Columns[Col ].Label = "작성자";
                    objSpread.ActiveSheet.Cells[i, Col].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    objSpread.ActiveSheet.Rows[i].Height = objSpread.ActiveSheet.Rows[i].GetPreferredHeight() + 5;
                }
                dt.Dispose();
            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            #endregion
        }

        void GetDefult(string strPtNo, string strMedFrDate)
        {
            if (ss1567_Sheet1.RowCount == 0)
                return;

            DataTable dt = null;
            OracleDataReader oracleDataReader = null;
            string SQL = string.Empty;

            #region 진짜 쿼리
            if (pAcp.inOutCls.Equals("O"))
            {
                SQL = " SELECT M.PANO, M.SNAME, P.JUMIN1, P.JUMIN2, M.SEX, M.AGE, M.DEPTCODE, M.ACTDate AS INDATE, '' AS ILSU, '' AS ICUDATE,  B.ABO, 0 AS ROOMCODE  ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.OPD_MASTER M INNER JOIN KOSMOS_PMPA.BAS_PATIENT P ";
                SQL += ComNum.VBLF + "                                           ON M.PANO = P.PANO ";
                SQL += ComNum.VBLF + "                                         AND M.PANO = '" + strPtNo + "' ";
                SQL += ComNum.VBLF + "                                         AND TRUNC(M.ACTDATE) =  TO_DATE('" + strMedFrDate + "', 'YYYY-MM-DD')  ";
                SQL += ComNum.VBLF + "                    LEFT OUTER JOIN KOSMOS_OCS.EXAM_BLOOD_MASTER B ";
                SQL += ComNum.VBLF + "                                        ON M.PANO = B.PANO ";
                SQL += ComNum.VBLF + "                                        AND M.SNAME = B.SNAME ";
                SQL += ComNum.VBLF + "                    LEFT OUTER JOIN KOSMOS_PMPA.BAS_DOCTOR D  ";
                SQL += ComNum.VBLF + "                                           ON D.DRCODE=M.DRCODE ";
            }
            else if (pAcp.inOutCls.Equals("I"))
            {
                SQL = " SELECT M.PANO, M.SNAME, P.JUMIN1, P.JUMIN2, M.SEX, M.AGE, M.DEPTCODE, M.InDate, M.ILSU, C.ICUDATE,  B.ABO, M.ROOMCODE ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.IPD_NEW_MASTER M INNER JOIN KOSMOS_PMPA.BAS_PATIENT P ";
                SQL += ComNum.VBLF + "                                           ON M.PANO = P.PANO ";
                SQL += ComNum.VBLF + "                                         AND M.PANO = '" + strPtNo + "' ";
                SQL += ComNum.VBLF + "                                         AND TRUNC(M.INDATE) =  TO_DATE('" + strMedFrDate + "', 'YYYY-MM-DD')  ";
                SQL += ComNum.VBLF + "                    LEFT OUTER JOIN KOSMOS_OCS.EXAM_BLOOD_MASTER B ";
                SQL += ComNum.VBLF + "                                        ON M.PANO = B.PANO ";
                SQL += ComNum.VBLF + "                                        AND M.SNAME = B.SNAME ";
                SQL += ComNum.VBLF + "                    LEFT OUTER JOIN KOSMOS_PMPA.BAS_DOCTOR D  ";
                SQL += ComNum.VBLF + "                                           ON D.DRCODE=M.DRCODE ";
                SQL += ComNum.VBLF + "                    LEFT OUTER JOIN KOSMOS_PMPA.NUR_ICU_PATIENT C ";
                SQL += ComNum.VBLF + "                                           ON C.PANO = M.PANO ";
                SQL += ComNum.VBLF + "                                         AND C.IPDNO = M.IPDNO ";
                SQL += ComNum.VBLF + "                                         AND C.ROOMCODE IN ('233','234')   ";
                SQL += ComNum.VBLF + "                                         AND C.DELDATE IS NULL ";
            }

            try
            {
                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if(dt != null && dt.Rows.Count > 0)
                {
                    ss1567.ActiveSheet.Cells[0, 6].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ss1567.ActiveSheet.Cells[0, 7].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ss1567.ActiveSheet.Cells[0, 8].Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + "*******";
                    ss1567.ActiveSheet.Cells[0, 9].Text = dt.Rows[0]["SEX"].ToString().Trim();
                    ss1567.ActiveSheet.Cells[0, 10].Text = dt.Rows[0]["AGE"].ToString().Trim();
                    ss1567.ActiveSheet.Cells[0, 11].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + "/" + dt.Rows[0]["ROOMCODE"].ToString().Trim();
                }
                dt.Dispose();

                #region 2
                SQL = "SELECT Abo FROM KOSMOS_OCS.EXAM_BLOOD_MASTER ";
                SQL += ComNum.VBLF + "WHERE Pano = '" + strPtNo + "' ";

                sqlErr = clsDB.GetAdoRs(ref oracleDataReader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                ssTitle_Sheet1.Cells[0, 1].Text = "";
                if (oracleDataReader.HasRows && oracleDataReader.Read())
                {
                    ss1567_Sheet1.Cells[0, 17].Text = oracleDataReader.GetValue(0).ToString().Trim();
                    ssTitle_Sheet1.Cells[0, 1].Text = oracleDataReader.GetValue(0).ToString().Trim();
                }

                oracleDataReader.Dispose();
                #endregion


                #region 진단명
                SQL = "SELECT B.ILLNAMEE";
                if (pAcp.inOutCls.Equals("O"))
                {
                    SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_OILLS A INNER JOIN KOSMOS_PMPA.BAS_ILLS B  ";
                    SQL += ComNum.VBLF + "                                    ON A.ILLCODE = B.ILLCODE ";
                    SQL += ComNum.VBLF + "                                   AND A.PTNO = '" + strPtNo + "' ";
                    SQL += ComNum.VBLF + "                                   AND TRUNC(A.BDATE) = TO_DATE('" + strMedFrDate + "', 'YYYY-MM-DD') ";
                }
                else
                {
                    SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IILLS A INNER JOIN KOSMOS_PMPA.BAS_ILLS B  ";
                    SQL += ComNum.VBLF + "                                    ON A.ILLCODE = B.ILLCODE ";
                    SQL += ComNum.VBLF + "                                   AND A.PTNO = '" + strPtNo + "' ";
                    SQL += ComNum.VBLF + "                                   AND TRUNC(A.ENTDATE) = TO_DATE('" + strMedFrDate + "', 'YYYY-MM-DD') ";
                }


                sqlErr = clsDB.GetAdoRs(ref oracleDataReader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (oracleDataReader.HasRows && oracleDataReader.Read())
                {
                    if(oracleDataReader.GetValue(0).ToString().Trim().Length > 0 && ss1567_Sheet1.Cells[0, ss1567_Sheet1.ColumnCount - 1].Text.Length == 0)
                    {
                        ss1567_Sheet1.Cells[0, ss1567_Sheet1.ColumnCount - 2].Text = oracleDataReader.GetValue(0).ToString().Trim();
                    }
                }

                oracleDataReader.Dispose();
                #endregion


            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            #endregion
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strSDate"></param>
        /// <param name="strEDATE"></param>
        void GetBloodIO(string strPtNo, string strSDate, string strEDATE)
        {
            Blood_Code_SET();


            OracleDataReader oracleDataReader = null;
            DataTable dt = null;
            StringBuilder strQUERY1 = new StringBuilder();
            StringBuilder strQUERY2 = new StringBuilder();
            StringBuilder strStatus = new StringBuilder();
            StringBuilder strBloodPickUp = new StringBuilder();

            string strSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string strSysTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");

            try
            {
                #region 1
                string SQL = "  SELECT  EXTRACTVALUE(CHARTXML,'/chart/ta2')  ||  EXTRACTVALUE(CHARTXML,'/chart/ta14')  QUERY1   ";
                SQL += ComNum.VBLF + "    FROM KOSMOS_EMR.EMRXML A";
                SQL += ComNum.VBLF + "   WHERE EMRNO IN (SELECT EMRNO FROM KOSMOS_EMR.EMRXMLMST WHERE FORMNO = 1965   ";
                SQL += ComNum.VBLF + "     AND PTNO = '" + strPtNo + "'";
                SQL += ComNum.VBLF + "     AND MEDFRDATE = '" + pAcp.medFrDate + "') ";
                SQL += ComNum.VBLF + "     AND EXTRACTVALUE(CHARTXML,'/chart/ta14') IS NOT NULL";

                string sqlErr = clsDB.GetAdoRs(ref oracleDataReader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (oracleDataReader.HasRows)
                {
                    while (oracleDataReader.Read())
                    {
                        strQUERY1.Append("'" + oracleDataReader.GetValue(0).ToString().Trim() + "',");
                    }
                }

                oracleDataReader.Dispose();
                #endregion

                #region 2
                SQL = "  SELECT  EXTRACTVALUE(CHARTXML,'/chart/ta2') QUERY2  ";
                SQL += ComNum.VBLF + "    FROM KOSMOS_EMR.EMRXML A";
                SQL += ComNum.VBLF + "   WHERE EMRNO IN (SELECT EMRNO FROM KOSMOS_EMR.EMRXMLMST WHERE FORMNO = 1965   ";
                SQL += ComNum.VBLF + "     AND PTNO = '" + strPtNo + "'";
                SQL += ComNum.VBLF + "     AND MEDFRDATE = '" + pAcp.medFrDate + "') ";
                SQL += ComNum.VBLF + "     AND EXTRACTVALUE(CHARTXML,'/chart/ta14') IS NULL  ";

                sqlErr = clsDB.GetAdoRs(ref oracleDataReader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (oracleDataReader.HasRows && oracleDataReader.Read())
                {
                    while (oracleDataReader.Read())
                    {
                        strQUERY2.Append("'" + oracleDataReader.GetValue(0).ToString().Trim() + "',");
                    }
                }

                oracleDataReader.Dispose();

                #endregion

                #region 3
                SQL = " SELECT";
                SQL += ComNum.VBLF + "   B.OUTDATE, B.CAPACITY, B.BLOODNO, C.PTABO || C.PTRH PTAR, B.COMPONENT, ";
                SQL += ComNum.VBLF + "   B.JANDATE, B.DestroyDate, B.OUTRDATE, B.EMERGENCY, B.OUTBUSE, B.OUTRCAUSE, '' AS REMARK, B.OUTPERSON, C.GUMSAJA";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.EXAM_BLOODTRANS A, KOSMOS_OCS.EXAM_BLOOD_IO B, KOSMOS_OCS.EXAM_BLOODCROSSM C";
                SQL += ComNum.VBLF + " WHERE A.PANO = '" + strPtNo + "'";
                SQL += ComNum.VBLF + "   AND A.GBJOB IN ('3','4')";

                if (pAcp.medEndDate.Length > 0)
                {
                    SQL += ComNum.VBLF + "   AND A.BDATE >= TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + DateTime.ParseExact(pAcp.medEndDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')";
                }
                else
                {
                    if (pAcp.inOutCls.Equals("I"))
                    {
                        SQL += ComNum.VBLF + "   AND A.BDATE >= TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')";
                    }
                    else
                    {
                        if (pAcp.medDeptCd.Equals("ER"))
                        {
                            SQL += ComNum.VBLF + "   AND A.BDATE >= TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')";
                            SQL += ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).AddDays(1).ToShortDateString() + "', 'YYYY-MM-DD')";

                        }
                        else
                        {
                            SQL += ComNum.VBLF + "   AND A.BDATE = TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')";
                        }
                    }
                }

                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO";
                SQL += ComNum.VBLF + "   AND A.PANO = C.PANO";
                SQL += ComNum.VBLF + "   AND A.BLOODNO = B.BLOODNO";
                SQL += ComNum.VBLF + "   AND A.BLOODNO = C.BLOODNO";
                SQL += ComNum.VBLF + "   AND C.GBSTATUS <> '3'";

                if (strQUERY1.Length > 0)
                {
                    strQUERY1.Remove(strQUERY1.Length - 1, 1);
                    SQL += ComNum.VBLF + "   AND B.BLOODNO || B.COMPONENT NOT IN (" + strQUERY1.ToString().Trim() + ") ";
                }

                if (strQUERY2.Length > 0)
                {
                    strQUERY2.Remove(strQUERY2.Length - 1, 1);
                    SQL += ComNum.VBLF + "   AND B.BLOODNO  NOT IN (" + strQUERY2.ToString().Trim() + ") ";
                }

                SQL += ComNum.VBLF + "GROUP BY  B.OUTDATE, B.CAPACITY, B.BLOODNO, C.PTABO || C.PTRH , B.COMPONENT,";
                SQL += ComNum.VBLF + "          B.JANDATE, B.DestroyDate, B.OUTRDATE, B.EMERGENCY, B.OUTBUSE, B.OUTRCAUSE, '' , B.OUTPERSON, C.GUMSAJA";

                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (dt == null)
                {
                    return;
                }


                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    ss1965_Sheet1.RowCount += 1;

                    strBloodPickUp.Clear();

                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 0].Locked = false;
                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 3].Text = strSysDate;
                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 4].Text = strSysTime;

                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 5].Text = Convert.ToDateTime(dt.Rows[i]["OUTDATE"].ToString().Trim()).ToShortDateString() + ComNum.VBLF +
                        Convert.ToDateTime(dt.Rows[i]["OUTDATE"].ToString().Trim()).ToString("HH:mm");
                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["BLOODNO"].ToString().Trim();

                    switch (dt.Rows[i]["OUTBUSE"].ToString().Trim())
                    {
                        case "011123":
                            strStatus.Append("응급실");
                            break;
                        case "033102":
                            strStatus.Append("수술실");
                            break;
                    }

                    if (dt.Rows[i]["EMERGENCY"].ToString().Equals("Y"))
                    {
                        if (strStatus.Length > 0)
                        {
                            strStatus.AppendLine("");
                        }

                        strStatus.Append("응급");
                    }

                    string strVal = string.Empty;
                    pstrBlood.TryGetValue(dt.Rows[i]["Component"].ToString().Trim(), out strVal);
                    string strComlText = dt.Rows[i]["PTAR"].ToString().Trim() + ComNum.VBLF + strVal + ComNum.VBLF + dt.Rows[i]["CAPACITY"].ToString().Trim();

                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 7].Text = strComlText;
                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 8].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["OUTPERSON"].ToString().Trim());

                    pstrBlood.TryGetValue(dt.Rows[i]["Component"].ToString().Trim(), out strVal);

                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1,nCol1].Text = ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, nCol1].Text.Trim().Length == 0 ? GetBloodCnt(strVal, dt.Rows[i]["CAPACITY"].ToString().Trim()) : ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, nCol1].Text.Trim();

                    if (dt.Rows[i]["OUTRDATE"].ToString().Trim().Length > 0)
                    {
                        if (strStatus.Length > 0)
                        {
                            strStatus.AppendLine("");
                        }
                        strStatus.Append("(반납)");
                    }
                    else if (dt.Rows[i]["JANDATE"].ToString().Trim().Length > 0)
                    {
                        if (strStatus.Length > 0)
                        {
                            strStatus.AppendLine("");
                        }
                        strStatus.Append("(잔량폐기)");
                    }
                    else if (dt.Rows[i]["DestroyDate"].ToString().Trim().Length > 0)
                    {
                        if (strStatus.Length > 0)
                        {
                            strStatus.AppendLine("");
                        }
                        strStatus.Append("(출고후폐기)");
                    }

                    if (strStatus.Length > 0)
                    {
                        ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, nCol2].Text = ("(★임병)" + strStatus.ToString().Trim());
                    }

                    strBloodPickUp.AppendLine("환자번호: " + pAcp.ptNo);
                    //strBloodPickUp.AppendLine("성명: " + ss1567_Sheet1.Cells[0, 7].Text.Trim() + "/" + ss1567_Sheet1.Cells[0, 9].Text.Trim() + "/" + ss1567_Sheet1.Cells[0, 10].Text.Trim());
                    //strBloodPickUp.AppendLine("진료과/병동:" + ss1567_Sheet1.Cells[0, 11].Text.Trim());

                    strBloodPickUp.Append("성명: " + ss1567_Sheet1.Cells[0, 7].Text.Trim());
                    strBloodPickUp.Append("/" + ss1567_Sheet1.Cells[0, 9].Text.Trim());
                    strBloodPickUp.AppendLine("/" + ss1567_Sheet1.Cells[0, 10].Text.Trim());
                    strBloodPickUp.Append("진료과/병동:" + ss1567_Sheet1.Cells[0, 11].Text.Trim());

                    strBloodPickUp.AppendLine("혈액형:" + ss1567_Sheet1.Cells[0, nCol2].Text.Trim());
                    pstrBlood.TryGetValue(dt.Rows[i]["Component"].ToString().Trim(), out strVal);
                    strBloodPickUp.AppendLine("혈액제제:" + strVal);
                    strBloodPickUp.AppendLine("일자:" + dt.Rows[i]["OUTDATE"].ToString().Trim());
                    strBloodPickUp.AppendLine("혈액번호:" + dt.Rows[i]["BLOODNO"].ToString().Trim());
                    strBloodPickUp.AppendLine("검사자:" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["GUMSAJA"].ToString().Trim()));

                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, nCol3].Text = dt.Rows[i]["COMPONENT"].ToString().Trim();
                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, nCol4].Text = strBloodPickUp.ToString().Trim();

                    ss1965_Sheet1.Rows[ss1965_Sheet1.RowCount - 1].Height = ss1965_Sheet1.Rows[ss1965_Sheet1.RowCount - 1].GetPreferredHeight() + 5;
                }

                ss1965_Sheet1.Columns[7].Width = ss1965_Sheet1.Columns[7].GetPreferredWidth() + 5;

                dt.Dispose();
                #endregion

                #region 매칭취소내역
                strStatus.Clear();
                strBloodPickUp.Clear();

                SQL = " SELECT '' AS OUTDATE, T.BLOODNO, '' AS PTAR, '' AS KORNAME, '' AS EXAMNAME, GBJOB ,  ";
                SQL += ComNum.VBLF + "              '' AS JANDATE, '' AS DestroyDate, '' AS OUTRDATE, '' AS EMERGENCY, '' AS OUTBUSE, '' AS OUTRCAUSE, REMARK, B.ABO, T.COMPONENT, T.CAPACITY ";
                SQL += ComNum.VBLF + "     FROM KOSMOS_OCS.EXAM_BLOODTRANS T INNER JOIN KOSMOS_OCS.EXAM_BLOOD_IO B ON T.BLOODNO = B.BLOODNO ";
                SQL += ComNum.VBLF + "  WHERE T.PANO = '" + strPtNo + "'  ";
                SQL += ComNum.VBLF + "    AND GBJOB IN ('5', '8') ";
                SQL += ComNum.VBLF + "    AND BDATE >= TO_DATE('" + DateTime.ParseExact(pAcp.medFrDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')"; 
                SQL += ComNum.VBLF + "    AND BDATE <= TO_DATE('" + DateTime.ParseExact((pAcp.medEndDate.Length == 0 ? strSysDate : pAcp.medEndDate), "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD') ";

                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (dt == null)
                {
                    return;
                }
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    strStatus.Clear();

                    ss1965_Sheet1.RowCount += 1;


                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 0].Locked = false;
                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 3].Text = strSysDate;
                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 4].Text = strSysTime;

                    if (dt.Rows[i]["OUTDATE"].ToString().Trim().Length > 0)
                    {
                        ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 5].Text = Convert.ToDateTime(dt.Rows[i]["OUTDATE"].ToString().Trim()).ToShortDateString() + ComNum.VBLF +
                        Convert.ToDateTime(dt.Rows[i]["OUTDATE"].ToString().Trim()).ToString("HH:mm");
                    }
                    
                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["BLOODNO"].ToString().Trim();

                    switch (dt.Rows[i]["OUTBUSE"].ToString().Trim())
                    {
                        case "011123":
                            strStatus.Append("응급실");
                            break;
                        case "033102":
                            strStatus.Append("수술실");
                            break;
                    }

                    if (dt.Rows[i]["EMERGENCY"].ToString().Equals("Y"))
                    {
                        if (strStatus.Length > 0)
                        {
                            strStatus.AppendLine("");
                        }

                        strStatus.Append("응급");
                    }

                    string strVal = string.Empty;
                    pstrBlood.TryGetValue(dt.Rows[i]["PTAR"].ToString().Trim(), out strVal);
                    string strComlText = dt.Rows[i]["PTAR"].ToString().Trim() + ComNum.VBLF + strVal + dt.Rows[i]["CAPACITY"].ToString().Trim();

                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 7].Text = strComlText;
                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["KORNAME"].ToString().Trim();

                    pstrBlood.TryGetValue(dt.Rows[i]["Component"].ToString().Trim(), out strVal);
                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, nCol1].Text = ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, nCol1].Text.Trim().Length == 0 ? GetBloodCnt(strVal, dt.Rows[i]["CAPACITY"].ToString().Trim()) : ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, nCol1].Text.Trim();

                    if (dt.Rows[i]["OUTRDATE"].ToString().Trim().Length > 0)
                    {
                        if (strStatus.Length > 0)
                        {
                            strStatus.AppendLine("");
                        }
                        strStatus.Append("(반납)");
                    }
                    else if (dt.Rows[i]["JANDATE"].ToString().Trim().Length > 0)
                    {
                        if (strStatus.Length > 0)
                        {
                            strStatus.AppendLine("");
                        }
                        strStatus.Append("(잔량폐기)");
                    }
                    else if (dt.Rows[i]["DestroyDate"].ToString().Trim().Length > 0)
                    {
                        if (strStatus.Length > 0)
                        {
                            strStatus.AppendLine("");
                        }
                        strStatus.Append("(출고후폐기)");
                    }

                    strComlText = dt.Rows[i]["ABO"].ToString().Trim() + ComNum.VBLF + strVal + dt.Rows[i]["CAPACITY"].ToString().Trim();

                    //반납을 하면 반납사유를 표시해준다.
                    if (dt.Rows[i]["CAPACITY"].ToString().Trim().Length > 0)
                    {
                        strStatus.AppendLine(dt.Rows[i]["OUTRCAUSE"].ToString().Trim());
                    }

                    switch(dt.Rows[i]["GBJOB"].ToString().Trim())
                    {
                        case "5":
                            strStatus.AppendLine("출고반납");
                            strStatus.AppendLine(dt.Rows[i]["REMARK"].ToString().Trim());
                            break;
                        case "7":
                            strStatus.AppendLine("폐기");
                            strStatus.AppendLine(dt.Rows[i]["REMARK"].ToString().Trim());
                            break;
                        case "8":
                            strStatus.AppendLine("매칭취소");
                            strStatus.AppendLine(dt.Rows[i]["REMARK"].ToString().Trim());
                            break;
                    }

                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, nCol2].Text = "(★임병)" + strStatus.ToString().Trim();
                    if(ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, nCol2].Text.Length > 0)
                    {
                        ss1965_Sheet1.Rows[ss1965_Sheet1.RowCount - 1].ForeColor = System.Drawing.Color.Red;
                    }

                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, nCol3].Text = dt.Rows[i]["COMPONENT"].ToString().Trim();
                    ss1965_Sheet1.Cells[ss1965_Sheet1.RowCount - 1, nCol4].Text = strBloodPickUp.ToString().Trim();

                    ss1965_Sheet1.Rows[ss1965_Sheet1.RowCount - 1].Height = ss1965_Sheet1.Rows[ss1965_Sheet1.RowCount - 1].GetPreferredHeight() + 5;
                }

                dt.Dispose();
                #endregion
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        string GetBloodCnt(string strName, string strCnt)
        {
            string rtnVal = string.Empty;
            switch(strName)
            {
                case "P/C(농축적혈구)":
                    if(strCnt.Equals("400"))
                    {
                        rtnVal = "240";
                    }
                    else if(strCnt.Equals("320"))
                    {
                        rtnVal = "195";
                    }
                    break;
                case "FFP(신선동결혈장)":
                    if (strCnt.Equals("400"))
                    {
                        rtnVal = "195";
                    }
                    else if (strCnt.Equals("320"))
                    {
                        rtnVal = "160";
                    }
                    break;
                case "PLT/C(농축혈소판)":
                    if (strCnt.Equals("400"))
                    {
                        rtnVal = "50";
                    }
                    else if (strCnt.Equals("320"))
                    {
                        rtnVal = "40";
                    }
                    break;
                case "W/B(전혈)":
                    if (strCnt.Equals("400"))
                    {
                        rtnVal = "400";
                    }
                    else if (strCnt.Equals("320"))
                    {
                        rtnVal = "320";
                    }
                    break;
                default:
                    break;
            }
            return rtnVal;
        }

        void Blood_Code_SET()
        {
            pstrBlood.Clear();
            pstrBlood.Add("BT021", "P/C(농축적혈구)");
            pstrBlood.Add("BT041", "FFP(신선동결혈장)");
            pstrBlood.Add("BT023", "PLT/C(농축혈소판)");
            pstrBlood.Add("BT011", "W/B(전혈)");
            pstrBlood.Add("BT051", "Cyro(동결침전제제)");
            pstrBlood.Add("BT071", "W/RBC(세척적혈구)");
            pstrBlood.Add("BT101", "WBC/C(농축백혈구)");
            pstrBlood.Add("BT31",  "PRP(혈소판풍부혈장)");
            pstrBlood.Add("BT27",  "ph-P") ;
            pstrBlood.Add("BT24",  "ph-PLT") ;
            pstrBlood.Add("BT25",  "ph-WBC") ;
            pstrBlood.Add("BT26",  "ph-CB") ;
            pstrBlood.Add("BT081", "F/RBC(백혈구여과제거 적혈구)");
        }


        #endregion

        #region 차트 그리기
        void MakeUserChart(string strFormNo, int lngStartCol, FpSpread objSpread)
        {


            objSpread.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            string SQL = string.Empty;
            DataTable dt = null;

            SQL = SQL + ComNum.VBLF + "  SELECT A.FORMNO, A.FORMNAME1 FORMNAME,  ";
            SQL = SQL + ComNum.VBLF + "      B.ITEMNO, B.ITEMNAME, B.ITEMTYPE, B.ITEMHALIGN, B.ITEMVALIGN,";
            SQL = SQL + ComNum.VBLF + "      B.ITEMHEIGHT, B.ITEMWIDTH, B.MULTILINE, B.USEMACRO, B.CONTROLID, B.ITEMRMK, B.TAGHEAD, B.TAGTAIL";
            SQL = SQL + ComNum.VBLF + "      FROM KOSMOS_EMR.EMRFORM A INNER JOIN KOSMOS_EMR.EMROPTFORM B";
            SQL = SQL + ComNum.VBLF + "         ON A.FORMNO = B.FORMNO";
            SQL = SQL + ComNum.VBLF + "      WHERE A.FORMNO = " + VB.Val(strFormNo);
            SQL = SQL + ComNum.VBLF + "      ORDER BY B.ITEMNO";

            string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return;
            }

            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Silver), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Silver), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            objSpread.ActiveSheet.ColumnCount = lngStartCol + dt.Rows.Count + 1;
            objSpread.ActiveSheet.Columns[1, objSpread.ActiveSheet.ColumnCount - 1].Border = complexBorder1;

            try
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region ssUserChart
                    objSpread.ActiveSheet.Columns[lngStartCol + i].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    objSpread.ActiveSheet.Columns[lngStartCol + i].Width = objSpread.ActiveSheet.Columns[lngStartCol + i].Label.Equals("진단명") ? 255 :  objSpread.ActiveSheet.Columns[lngStartCol + i].GetPreferredWidth() + 5;

                    if (objSpread.ActiveSheet.ColumnCount == 0)
                        continue;

                    switch (dt.Rows[i]["ITEMTYPE"].ToString().Trim().ToUpper())
                    {
                        case "EDIT":
                            TextCellType TypeText = new TextCellType();
                            TypeText.Multiline = dt.Rows[i]["MULTILINE"].ToString().Trim() == "1" ? true : false;
                            objSpread.ActiveSheet.Columns[lngStartCol + i].CellType = TypeText;
                            break;
                        case "COMBO":

                            ListBox list1 = new ListBox();
                            foreach (string Code in dt.Rows[i]["ITEMRMK"].ToString().Trim().Split('^'))
                            {
                                list1.Items.Add(Code);
                            }
                            ComboBoxCellType TypeComboBox = new ComboBoxCellType();
                            TypeComboBox.ListControl = list1;
                            TypeComboBox.Editable = true;
                            objSpread.ActiveSheet.Columns[lngStartCol + i].CellType = TypeComboBox;
                            break;
                        case "CHECK":
                            CheckBoxCellType TypeCheckBox = new CheckBoxCellType(); 
                            objSpread.ActiveSheet.Columns[lngStartCol + i].CellType = TypeCheckBox;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMHALIGN"].ToString().Trim().ToUpper())
                    {
                        case "LEFT":
                            objSpread.ActiveSheet.Columns[lngStartCol + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            break;
                        case "CENTER":
                            objSpread.ActiveSheet.Columns[lngStartCol + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            break;
                        case "RIGHT":
                            objSpread.ActiveSheet.Columns[lngStartCol + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMVALIGN"].ToString().Trim().ToUpper())
                    {
                        case "TOP":
                            objSpread.ActiveSheet.Columns[lngStartCol + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                            break;
                        case "CENTER":
                            objSpread.ActiveSheet.Columns[lngStartCol + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "BOTTOM":
                            objSpread.ActiveSheet.Columns[lngStartCol + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Bottom;
                            break;
                    }

                    if (dt.Rows[i]["CONTROLID"].ToString().Trim().Length > 0)
                    {
                        objSpread.ActiveSheet.Columns[lngStartCol + i].Tag = dt.Rows[i]["CONTROLID"].ToString().Trim();
                    }
                    #endregion
                }

                objSpread.ActiveSheet.RowCount = 1;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }
        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetChartHistory();
        }

        #region 수혈 시간대 별 활력측정 및 부작용

        private void btnSelectTime_Click(object sender, EventArgs e)
        {
            using (frmEmrTimeTable frmEmrTime = new frmEmrTimeTable())
            {
                frmEmrTime.StartPosition = FormStartPosition.CenterParent;
                frmEmrTime.rSendTime += FrmEmrTime_rSendTime;
                frmEmrTime.ShowDialog(this);
            }
        }

        private void FrmEmrTime_rSendTime(int Row, int Col, string Time)
        {
            string[] str = Time.Split('/');
            for(int i = 0; i < str.Length; i++)
            {
                if(!string.IsNullOrWhiteSpace(str[i]))
                {
                    Control control = panTime.Controls.Find("ChkTime" + i, true).FirstOrDefault();
                    if(control != null)
                    {
                        (control as CheckBox).Checked = true;
                    }

                    control = panTime.Controls.Find("txtTime" + i, true).FirstOrDefault();
                    if (control != null)
                    {
                        control.Text = str[i];
                    }
                }
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            Control control = null; 

            StringBuilder str1 = new StringBuilder();
            StringBuilder str2 = new StringBuilder();
            StringBuilder str3 = new StringBuilder();
            StringBuilder strFail = new StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                control = panTime.Controls.Find("chkTime" + i.ToString("0"), true).FirstOrDefault();
                if (control != null && (control as CheckBox).Checked)
                {
                    control = panTime.Controls.Find("txtTime" + i.ToString("0"), true).FirstOrDefault();
                    if (control != null)
                    {
                        str1.Append(control.Text.Trim()).Append("\n");
                    }

                    control = panTime.Controls.Find("txtTimeVital" + i.ToString("0"), true).FirstOrDefault();
                    if (control != null)
                    {
                        str2.Append(control.Text.Trim()).Append("\n");
                    }

                    strFail.Clear();

                    control = panTime.Controls.Find("chkFail1" + i.ToString("0"), true).FirstOrDefault();
                    if (control != null && (control as CheckBox).Checked)
                    {
                        strFail.Append("주사부위 통증 있음 ");
                    }

                    control = panTime.Controls.Find("chkFail2" + i.ToString("0"), true).FirstOrDefault();
                    if (control != null && (control as CheckBox).Checked)
                    {
                        strFail.Append("불쾌감 있음 ");
                    }

                    control = panTime.Controls.Find("chkFail3" + i.ToString("0"), true).FirstOrDefault();
                    if (control != null && (control as CheckBox).Checked)
                    {
                        strFail.Append("흉통 있음 ");
                    }

                    control = panTime.Controls.Find("chkFail4" + i.ToString("0"), true).FirstOrDefault();
                    if (control != null && (control as CheckBox).Checked)
                    {
                        strFail.Append("복통 있음 ");
                    }

                    control = panTime.Controls.Find("chkFail5" + i.ToString("0"), true).FirstOrDefault();
                    if (control != null && (control as CheckBox).Checked)
                    {
                        strFail.Append("발열 있음 ");
                    }

                    if(strFail.Length == 0)
                    {
                        strFail.Append("주사부위 통증 불쾌감 흉통 복통 발열 모두 없음");
                    }

                    str3.AppendLine(strFail.ToString());
                }
            }

            ss1965_Sheet1.Cells[(int)VB.Val(txtRow.Text), 12].Text = str1.ToString();
            ss1965_Sheet1.Cells[(int)VB.Val(txtRow.Text), 13].Text = str2.ToString();
            ss1965_Sheet1.Cells[(int)VB.Val(txtRow.Text), 15].Text = str3.ToString();
            panTime.Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panTime.Visible = false;
        }

        #endregion

        #region 스프레드 이벤트
        private void ss1567_EditModeOff(object sender, EventArgs e)
        {
            if (ss1567_Sheet1.ActiveColumnIndex == 16)
            {
                string strUseId = ss1567_Sheet1.Cells[ss1567_Sheet1.ActiveRowIndex, 16].Text.Trim();

                #region 사용자 이름 읽어오기
                OracleDataReader reader = null;
                string strSql = " SELECT NAME";
                strSql += ComNum.VBLF + "FROM KOSMOS_EMR.EMR_USERT";
                strSql += ComNum.VBLF + "    WHERE USERID =  '" + strUseId + "'";

                string sqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, sqlErr, clsDB.DbCon);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    ss1567_Sheet1.Cells[ss1567_Sheet1.ActiveRowIndex, 16].Text = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                #endregion

            }
        }

        private void ss1965_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ss1965_Sheet1.RowCount == 0)
                return;

            if (e.Column == 9)
            {
                string strBLOODNO = ss1965_Sheet1.Cells[e.Row, 6].Text.Trim();

                txtConfirmID1.Clear();
                txtConfirmID2.Clear();
                txtConfirmPw2.Clear();

                txtConfirmPw2.Tag = null;
                txtConfirmID1.Text = clsType.User.UserName;

                READ_BLOOD_INFO(pAcp.ptNo, strBLOODNO);
                panUserChk.Visible = true;
                txtConfirmID2.Focus();
            }

            if (e.Column == 12 || e.Column == 13 || e.Column == 15)
            {
                SET_TIME_DATA(e.Row);
            }
        }

        void READ_BLOOD_INFO(string argPTNO, string argBLOODNO)
        {
            chk1.Checked = false;
            chk2.Checked = false;
            chk3.Checked = false;
            chk4.Checked = false;
            chk5.Checked = false;

            Txt1.Clear();
            Txt2.Clear();
            Txt3.Clear();
            Txt4.Clear();
            Txt5.Clear();
            Txt6.Clear();
            Txt7.Clear();

            DataTable dt = null;

            #region 진짜 쿼리
            string SQL = " SELECT TO_CHAR(B.OUTDATE, 'YYYY-MM-DD HH24:MI:SS') OUTDATE , A.PANO, C.PTABO, B.COMPONENT, B.BLOODNO, TO_CHAR(B.EXPIRE, 'YYYY-MM-DD') EXPIRE, C.GUMSAJA";
            SQL += ComNum.VBLF + "   FROM KOSMOS_OCS.EXAM_BLOODTRANS A, KOSMOS_OCS.EXAM_BLOOD_IO B, KOSMOS_OCS.EXAM_BLOODCROSSM C";
            SQL += ComNum.VBLF + "  WHERE B.BLOODNO = '" + argBLOODNO + "'";
            SQL += ComNum.VBLF + "     AND A.PANO = '" + argPTNO + "'";
            SQL += ComNum.VBLF + "       AND A.GBJOB IN ('3','4')";
            SQL += ComNum.VBLF + "       AND A.PANO = B.PANO";
            SQL += ComNum.VBLF + "       AND A.PANO = C.PANO";
            SQL += ComNum.VBLF + "       AND A.BLOODNO = B.BLOODNO";
            SQL += ComNum.VBLF + "       AND A.BLOODNO = C.BLOODNO";
            SQL += ComNum.VBLF + "       AND C.GBSTATUS <> '3'";
            SQL += ComNum.VBLF + "       GROUP BY B.OUTDATE, A.PANO, C.PTABO, B.COMPONENT, B.BLOODNO, B.EXPIRE, C.GUMSAJA";

            try
            {
                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (dt.Rows.Count > 0 )
                {
                    Txt1.Text = argPTNO;
                    Txt2.Text = dt.Rows[0]["PTABO"].ToString().Trim();
                    string strVal = string.Empty;
                    pstrBlood.TryGetValue(dt.Rows[0]["COMPONENT"].ToString().Trim(), out strVal);
                    Txt3.Text = strVal;
                    Txt4.Text = argBLOODNO;
                    Txt5.Text = dt.Rows[0]["EXPIRE"].ToString().Trim();
                    Txt6.Text = dt.Rows[0]["OUTDATE"].ToString().Trim();
                    Txt7.Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["GUMSAJA"].ToString().Trim());
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            #endregion

        }

        void SET_TIME_DATA(int Row)
        {
            foreach(Control control in ComFunc.GetAllControls(panTime))
            {
                if (control is TextBox && !control.Name.Equals("txtRow"))
                {
                    control.Text = "";
                }
                else if (control is CheckBox)
                {
                    (control as CheckBox).Checked = false;
                }
            }

            txtRow.Text = Row.ToString();

            List<string> str1 = ss1965_Sheet1.Cells[Row, 12].Text.Trim().Split(new[] { "\n" }, StringSplitOptions.None).ToList();
            List<string> str2 = ss1965_Sheet1.Cells[Row, 13].Text.Trim().Split(new[] { "\n" }, StringSplitOptions.None).ToList();
            List<string> str3 = ss1965_Sheet1.Cells[Row, 15].Text.Trim().Split(new[] { "\n" }, StringSplitOptions.None).ToList();


            Control[] controls = null;
            for (int i = 0; i < str1.Count; i++)
            {
                if (str1[i].Length > 0)
                {
                    controls = panTime.Controls.Find("chkTime" + i, true);
                    if (controls.Length > 0)
                    {
                        (controls[0] as CheckBox).Checked = true;
                    }

                    controls = panTime.Controls.Find("txtTime" + i.ToString(), true);
                    if (controls.Length > 0)
                    {
                        (controls[0] as TextBox).Text = str1[i].Replace("\r", "");
                        (controls[0] as TextBox).Text = (controls[0] as TextBox).Text.Replace("\n", "");
                    }
                }
            }

            for (int i = 0; i < str2.Count; i++)
            {
                if (str2[i].Length > 0)
                {
                    controls = panTime.Controls.Find("chkTime" + i, true);
                    if (controls.Length > 0)
                    {
                        (controls[0] as CheckBox).Checked = true;
                    }

                    controls = panTime.Controls.Find("txtTimeVital" + i.ToString(), true);
                    if (controls.Length > 0)
                    {
                        (controls[0] as TextBox).Text = str2[i].Replace("\r", "");
                        (controls[0] as TextBox).Text = (controls[0] as TextBox).Text.Replace("\n", "");
                    }
                }
            }

            for (int i = 0; i < str3.Count; i++)
            {
                if (str3[i].Length > 0)
                {
                    if (str3[i].IndexOf("주사부위 통증 있음") != -1)
                    {
                        controls = panTime.Controls.Find("chkFail1" + i, true);
                        if (controls.Length > 0)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }
                    }
                    if (str3[i].IndexOf("불쾌감 있음") != -1)
                    {
                        controls = panTime.Controls.Find("chkFail2" + i, true);
                        if (controls.Length > 0)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }
                    }
                    if (str3[i].IndexOf("흉통 있음") != -1)
                    {
                        controls = panTime.Controls.Find("chkFail3" + i, true);
                        if (controls.Length > 0)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }
                    }
                    if (str3[i].IndexOf("복통 있음") != -1)
                    {
                        controls = panTime.Controls.Find("chkFail4" + i, true);
                        if (controls.Length > 0)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }
                    }
                    if (str3[i].IndexOf("발열 있음") != -1)
                    {
                        controls = panTime.Controls.Find("chkFail5" + i, true);
                        if (controls.Length > 0)
                        {
                            (controls[0] as CheckBox).Checked = true;
                        }
                    }
                }
            }

            panTime.Visible = true;
        }

        #endregion

        private void btnAppUserChk_Click(object sender, EventArgs e)
        {
            string strUserName = txtConfirmID1.Text.Trim() + ComNum.VBLF + txtConfirmID2.Text.Trim();
            ss1965_Sheet1.Cells[ss1965_Sheet1.ActiveRowIndex, 9].Text = strUserName;
            panUserChk.Visible = false;

            string strOK = txtConfirmPw2.Tag == null ? "NO" : "OK";

            if(strOK.Equals("OK"))
            {
                if(chk1.Checked && chk2.Checked && chk3.Checked && chk4.Checked && chk5.Checked)
                {
                    string strBloodPickUp = ss1965_Sheet1.Cells[ss1965_Sheet1.ActiveRowIndex, nCol4].Text.Trim();
                    ss1965_Sheet1.Cells[ss1965_Sheet1.ActiveRowIndex, 10].Text = strBloodPickUp;
                    ss1965_Sheet1.Columns[10].Width = ss1965_Sheet1.Columns[10].GetPreferredWidth() + 5;
                    ss1965_Sheet1.Rows[ss1965_Sheet1.ActiveRowIndex].Height = ss1965_Sheet1.Rows[ss1965_Sheet1.ActiveRowIndex].GetPreferredHeight() + 5;
                }
            }
            else
            {
                ComFunc.MsgBoxEx(this, "모든 항목을 확인하여야 혈액확인이 가능합니다.");
            }
        }

        private void btnExitUserChk_Click(object sender, EventArgs e)
        {
            btnAppUserChk.Enabled = false;
            panUserChk.Visible = false;
        }

        private void ssUserChk_EditModeOff(object sender, EventArgs e)
        {
            if (ssUserChk_Sheet1.ActiveColumnIndex == 1)
            {
                

            }
        }

        private void ssUserChk_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtConfirmID2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                txtConfirmPw2.Focus();
            }
        }

        private void txtConfirmPw2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                if (clsType.User.IdNumber.Equals(txtConfirmID2.Text.Trim()))
                {
                    ComFunc.MsgBoxEx(this, "동일한 작성자를 입력하실 수 없습니다.");
                    return;
                }

                #region 쿼리
                OracleDataReader reader = null;
                string SQL = " SELECT USERNAME AS USENAME  ";
                SQL = SQL += ComNum.VBLF + "FROM KOSMOS_PMPA.BAS_USER";
                SQL = SQL += ComNum.VBLF + "WHERE IDNUMBER =  '" + txtConfirmID2.Text.Trim() + "'";
                SQL = SQL += ComNum.VBLF + "  AND PASSHASH256 =  '" + clsSHA.SHA256(txtConfirmPw2.Text.Trim()) + "'";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    txtConfirmID2.Text = reader.GetValue(0).ToString().Trim();
                    txtConfirmPw2.Tag = "Y";
                    btnAppUserChk.Enabled = true;
                }

                reader.Dispose();
                #endregion
            }

        }

        private void btnSearchPreHis_Click(object sender, EventArgs e)
        {

        }
    }
}
