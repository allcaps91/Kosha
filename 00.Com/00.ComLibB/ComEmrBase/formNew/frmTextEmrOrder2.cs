using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Dr Order, Dr(Er) Order
    /// </summary>
    public partial class frmTextEmrOrder2 : Form, EmrChartForm
    {
        #region // TopMenu관련 선언
        private usFormTopMenu usFormTopMenuEvent;
        private usTimeSet usTimeSetEvent;
        public ComboBox mMaskBox = null;
        #endregion

        #region //폼에서 사용하는 변수
        //const string mDirection = "V";   //기록지 작성방향(H: 옆으로, V:아래로)
        //const bool mHeadVisible = true;   //해드를 보이게 할지 여부
        //const int mintHeadCol = 4;  //해드 칼럼 수(작성, 조회 공통)
        //const int mintHeadRow = 2;  //해드 줄 수 (조회시에)

        int mCallFormGb = 0;  //차트작성 0, 기록지등록에서 호출 1,

        //bool mHeadVisible = true;   //해드를 보이게 할지 여부

        /// <summary>
        /// 정신과 여부
        /// </summary>
        bool mViewNpChart = false;

        string mFLOWGB = "COL"; //서식작성 방향
        //int mFLOWITEMCNT = 0;
        int mFLOWHEADCNT = 0;

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

        public string mstrFormNo = "1680";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public string mstrEmrNo = "42694720";  //961 131641  //963 735603
        public string mstrMode = "W";

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
            pSaveData();
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

        private void dtMedFrDate_ValueChanged(object sender, EventArgs e)
        {
            //필요시만 코딩함
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
            //isReciveOrderSave = true;
            //dblEmrNo = pSaveData(strFlag);
            //isReciveOrderSave = false;
            return dblEmrNo;
        }

        /// <summary>
        /// 출력
        /// </summary>
        private void pPrintForm()
        {
            string strMode = "A";
            if (mstrOrderDate != "")
            {
                strMode = "P";
                clsFormPrint.mstrPRINTFLAG = "0"; //원외출력으로 변경
            }

            btnPrint.Enabled = false;
            //SetPrintVisible();

            if (mFLOWGB == "ROW")
            {
                clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpOptSDate.Value.ToString("yyyyMMdd"), dtpOptEDate.Value.ToString("yyyyMMdd"),
                                         ssOrder, "P", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, mFLOWGB, -1, ssOrder_Sheet1.RowCount - 3, mFLOWHEADCNT, strMode);
            }
            else
            {
                clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpOptSDate.Value.ToString("yyyyMMdd"), dtpOptEDate.Value.ToString("yyyyMMdd"),
                                         ssOrder, "P", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, mFLOWGB, -1, ssOrder_Sheet1.ColumnCount - 3, mFLOWHEADCNT, strMode);
            }

            //pInitFormSpc();



            btnPrint.Enabled = true;
        }

        /// <summary>
        /// 스프래드 클리어
        /// </summary>
        private void pClearForm()
        {
            //ssOrder_Sheet1.RowCount = 0;
            //mRow = -2;
            //mCol = -2;
            GetChartHistoryCopy();
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double pSaveData()
        {
            double dblEmrNo = 0;

            return dblEmrNo;
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        private bool pDelData()
        {
            bool rtnVal = false;

            return rtnVal;
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

        public int PrintFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            btnPrint.Enabled = false;
            //SetPrintVisible();

            if (mFLOWGB == "ROW")
            {
                rtnVal = clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpOptSDate.Value.ToString("yyyyMMdd"), dtpOptEDate.Value.ToString("yyyyMMdd"),
                                         ssOrder, "P", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, mFLOWGB, -1, ssOrder_Sheet1.RowCount - 3, mFLOWHEADCNT, "P");
            }
            else
            {
                rtnVal = clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpOptSDate.Value.ToString("yyyyMMdd"), dtpOptEDate.Value.ToString("yyyyMMdd"),
                                         ssOrder, "P", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, mFLOWGB, -1, ssOrder_Sheet1.ColumnCount - 3, mFLOWHEADCNT, "P");
            }

            //pInitFormSpc();

            btnPrint.Enabled = true;

            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //rtnVal = clsFormPrint.PrintToTifFileLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
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
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = true;
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

        #region //생성자
        public frmTextEmrOrder2()
        {
            InitializeComponent();
        }

        public frmTextEmrOrder2(string strFormNo, string strUpdateNo, string strEmrNo, int pCallFormGb)
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
        public frmTextEmrOrder2(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, string strOrderDate, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
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

        public frmTextEmrOrder2(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
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

        #region 스프레드 enum
        public enum Order
        {
            /// <summary>
            /// 처방일
            /// </summary>
            Bdate,
            /// <summary>
            /// 처방코드
            /// </summary>
            OrderCode,
            /// <summary>
            /// 처방명
            /// </summary>
            OrderName,
            /// <summary>
            /// 일용량
            /// </summary>
            Content,
            /// <summary>
            /// 일투량
            /// </summary>
            RealQty,
            /// <summary>
            /// 용법/검체
            /// </summary>
            DosName,
            /// <summary>
            /// Mix
            /// </summary>
            GbGroup,
            /// <summary>
            /// 횟수
            /// </summary>
            GbDiv,
            /// <summary>
            /// 일수
            /// </summary>
            Nal,
            /// <summary>
            /// Sign
            /// </summary>
            Sign
        }
        #endregion
        
        private void FrmEmrBaseOrder2_Load(object sender, EventArgs e)
        {
            clsEmrChart.SetChartHead(clsDB.DbCon, usFormTopMenuEvent, pAcp);

            if (mCallFormGb != 1)
            {
                SetUserAut();
            }

            panTopMenu.Visible = false;
            //usFormTopMenuEvent.mbtnClear.Visible = true;
            //usFormTopMenuEvent.mbtnPrint.Visible = true;
            //usFormTopMenuEvent.mbtnSave.Visible = true;
            //usFormTopMenuEvent.mbtnSaveTemp.Visible = true;

            //if (pAcp == null)
            //{
            //    ComFunc.MsgBoxEx(this, "환자 정보가 올바르지 않습니다.");
            //    return;
            //}

            mViewNpChart = clsEmrQueryOld.ViewNPChart(clsType.User.Sabun);

            dtpOptSDate.Value = Convert.ToDateTime(
                ReadOptionDate(clsEmrPublic.gUserGrade, 
                pAcp.inOutCls,
                pAcp.medDeptCd,
                clsType.User.IdNumber,
                Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-3).ToShortDateString())
                );
           dtpOptEDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

           btnPrint.Visible = false;

            if(clsEmrPublic.gUserGrade == "SIMSA" || clsEmrPublic.gUserGrade == "WRITE")
            {
                btnPrint.Visible = true;
                //ssOrder_Sheet1.Columns[9].Visible = true;
            }

            if(VB.Val(mstrEmrNo) > 0)
            {
                mstrOrderDate = mstrEmrNo;
            }

            if(mstrOrderDate != "")
            {
                //panBtn.Visible = false;
                GetChartHistoryCopy();
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

            if (clsType.User.Sabun == "14472")
            {
                rtnVal= "SIMSA";
            }

            return rtnVal;

        }

        void GetChartHistoryCopy()
        {
            ssOrder_Sheet1.RowCount = 0;

            dtpOptSDate.Value = Convert.ToDateTime(
             ReadOptionDate(clsEmrPublic.gUserGrade,
             pAcp.inOutCls,
             pAcp.medDeptCd,
             clsType.User.IdNumber,
             VB.Val(pAcp.medFrDate).ToString("0000-00-00"))
             );

            if(pAcp.inOutCls == "O")
            {
                dtpOptSDate.Value = Convert.ToDateTime(
                 ReadOptionDate(clsEmrPublic.gUserGrade,
                 pAcp.inOutCls,
                 pAcp.medDeptCd,
                 clsType.User.IdNumber,
                 VB.Val(pAcp.medFrDate).ToString("0000-00-00"))
                 );
            }
            else
            {
                if(pAcp.medEndDate == "")
                {
                    dtpOptEDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                }
                else
                {
                    dtpOptEDate.Value = Convert.ToDateTime(VB.Val(pAcp.medEndDate).ToString("0000-00-00"));
                }

                if(mstrOrderDate != "")
                {
                    dtpOptSDate.Value = Convert.ToDateTime(
                     ReadOptionDate(clsEmrPublic.gUserGrade,
                     pAcp.inOutCls,
                     pAcp.medDeptCd,
                     clsType.User.IdNumber,
                     VB.Val(mstrOrderDate).ToString("0000-00-00"))
                     );

                    dtpOptEDate.Value = Convert.ToDateTime(VB.Val(mstrOrderDate).ToString("0000-00-00"));
                }
            }

            GetCopyOrder();
        }

        void GetCopyOrder()
        {
            ssOrder_Sheet1.RowCount = 0;
            StringBuilder SQL = new StringBuilder();
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = null;
            
            try
            {
                #region 쿼리
                if (pAcp.inOutCls == "I")
                {
                    SQL.AppendLine("  SELECT TO_CHAR(O.BDate, 'YYYY-MM-DD') BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames, O.REMARK,");
                    SQL.AppendLine("    C.DispHeader, O.Qty, O.CONTENTS, O.GbGroup,");
                    SQL.AppendLine("    O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE, '' RES, O.GBPRN, O.GBACT ");
                    SQL.AppendLine("    FROM ADMIN.OCS_IORDER O,");
                    SQL.AppendLine("          ADMIN.OCS_ORDERCODE C,");
                    SQL.AppendLine("          ADMIN.OCS_ODOSAGE D,");
                    SQL.AppendLine("          ADMIN.OCS_DOCTOR N,");
                    SQL.AppendLine("          ADMIN.BAS_SUN     S");
                    SQL.AppendLine("      WHERE O.PTNO = '" + pAcp.ptNo + "' ");

                    if (mstrOrderDate != "")
                    {
                        SQL.AppendLine("        AND O.BDATE = TO_DATE('" + VB.Val(mstrOrderDate).ToString("0000-00-00") + "','YYYY-MM-DD') ");
                    }
                    else
                    {
                        SQL.AppendLine("        AND O.BDATE >= TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");

                        if (pAcp.medEndDate != "")
                        {
                            SQL.AppendLine("        AND O.BDATE <= TO_DATE('" + dtpOptEDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");
                        }
                    }

                  
                    SQL.AppendLine("        AND O.GBSTATUS NOT IN ('D-','D','D+' )  ");
                    SQL.AppendLine("        AND O.SlipNo     =  C.SlipNo(+)      ");
                    SQL.AppendLine("        AND O.OrderCode  =  C.OrderCode(+)   ");
                    SQL.AppendLine("        AND O.DosCode    =  D.DosCode(+)     ");
                    SQL.AppendLine("        AND O.DRCODE      =  N.SABUN(+)      ");
                    SQL.AppendLine("        AND (O.GBIOE NOT IN ('E','EI') OR O.GBIOE IS NULL OR GBIOE = '')");
                    SQL.AppendLine("        AND O.SUCODE = S.SUNEXT(+) ");

                    if (pAcp.medDeptCd == "RA" ||
                        (pAcp.medDeptCd == "MD" &&
                        (pAcp.medDrCd == "1107" ||
                        pAcp.medDrCd == "1125")))
                    {
                        SQL.AppendLine("        AND O.DEPTCODE = 'MD'");
                        SQL.AppendLine("        AND O.DRCODE IN ('1107','1125')");
                    }
                    else
                    {
                        SQL.AppendLine("        AND O.DEPTCODE = '" + pAcp.medDeptCd + "'");
                        SQL.AppendLine("        AND O.DRCODE NOT IN ('1107','1125')");
                    }

                    SQL.AppendLine("ORDER BY O.BDATE DESC, O.Slipno, O.Seqno");

                }
                else if (pAcp.inOutCls == "O" &&    pAcp.medDeptCd != "ER" &&   pAcp.formNo != 2090)
                {
                    SQL.AppendLine("  SELECT TO_CHAR(O.BDate, 'YYYY-MM-DD') BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames,  O.REMARK,");
                    SQL.AppendLine("    C.DispHeader, O.Qty, O.CONTENTS, '' AS GbGroup,");
                    SQL.AppendLine("     O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE, O.RES, '' GBPRN, '' GBACT  ");
                    SQL.AppendLine("    FROM ADMIN.OCS_OORDER O,");
                    SQL.AppendLine("         ADMIN.OCS_ORDERCODE C,");
                    SQL.AppendLine("         ADMIN.OCS_ODOSAGE D,");
                    SQL.AppendLine("         ADMIN.OCS_DOCTOR N,");
                    SQL.AppendLine("         ADMIN.BAS_SUN     S");
                    SQL.AppendLine("    WHERE O.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("      AND O.BDATE = TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");

                    if (pAcp.inOutCls == "O")
                    {
                        if (pAcp.medDeptCd == "RA" ||
                            (pAcp.medDeptCd == "MD" &&
                            (pAcp.medDrCd == "1107" ||
                            pAcp.medDrCd == "1125")))
                        {
                            SQL.AppendLine("        AND O.DEPTCODE = 'MD'");
                            SQL.AppendLine("        AND O.DRCODE IN ('1107','1125')");
                        }
                        else
                        {
                            SQL.AppendLine("        AND O.DEPTCODE = '" + pAcp.medDeptCd + "'");
                            SQL.AppendLine("        AND O.DRCODE NOT IN ('1107','1125')");
                        }
                    }

                    SQL.AppendLine("        AND O.GBSUNAP ='1' AND O.Seqno    > 0   AND O.NAL      > 0");
                    SQL.AppendLine("        AND O.SlipNo     =  C.SlipNo(+)      ");
                    SQL.AppendLine("        AND O.OrderCode  =  C.OrderCode(+)   ");
                    SQL.AppendLine("        AND O.DosCode    =  D.DosCode(+)     ");
                    SQL.AppendLine("        AND O.DRCODE      =  N.SABUN(+)      ");

                    SQL.AppendLine("        AND    O.SUCODE = S.SUNEXT(+) ");
                    SQL.AppendLine("ORDER BY O.BDATE DESC, O.GbSunap DESC, O.Slipno, O.Seqno");
                }
                else if ((pAcp.inOutCls == "O" &&  pAcp.medDeptCd == "ER"))
                {
                    SQL.AppendLine("  SELECT TO_CHAR(O.BDate, 'YYYY-MM-DD') BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames, O.REMARK,");
                    SQL.AppendLine("    C.DispHeader, O.Qty, O.CONTENTS, O.GbGroup,");
                    SQL.AppendLine("    O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE , O.GBACT ");
                    SQL.AppendLine("    FROM ADMIN.OCS_IORDER O,");
                    SQL.AppendLine("         ADMIN.OCS_ORDERCODE C,");
                    SQL.AppendLine("         ADMIN.OCS_ODOSAGE D,");
                    SQL.AppendLine("         ADMIN.OCS_DOCTOR N,");
                    SQL.AppendLine("         ADMIN.BAS_SUN     S");
                    SQL.AppendLine("      WHERE O.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("        AND O.BDATE >= TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");
                    SQL.AppendLine("        AND O.BDATE <= TO_DATE('" + dtpOptSDate.Value.AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ");
                    SQL.AppendLine("        AND O.GBSTATUS NOT IN ('D-','D','D+' )  ");
                    SQL.AppendLine("        AND O.SlipNo     =  C.SlipNo(+)      ");
                    SQL.AppendLine("        AND O.OrderCode  =  C.OrderCode(+)   ");
                    SQL.AppendLine("        AND O.DosCode    =  D.DosCode(+)     ");
                    SQL.AppendLine("        AND O.DRCODE      =  N.SABUN(+)      ");
                    SQL.AppendLine("        AND O.GBIOE IN ('E','EI') ");
                    SQL.AppendLine("        AND O.SUCODE = S.SUNEXT(+) ");
                    SQL.AppendLine("ORDER BY O.BDATE DESC, O.Slipno, O.Seqno");
                }
                #endregion

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssOrder_Sheet1.RowCount = dt.Rows.Count;
                ssOrder_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssOrder_Sheet1.Cells[i, (int)Order.Bdate].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.OrderCode].Text = dt.Rows[i]["OrderCode"].ToString().Trim();

                    if (dt.Rows[i]["BUN"].ToString().Trim().Length == 0 ||
                        dt.Rows[i]["BUN"].ToString().Trim() == "10")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    }
                    else
                    {
                        if (dt.Rows[i]["ORDERNAMES"].ToString().Trim().Length > 0)
                        {
                            string strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["DispHeader"].ToString().Trim().Length > 0)
                        {
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text =
                                dt.Rows[i]["DispHeader"].ToString().Trim() +
                                dt.Rows[i]["OrderName"].ToString().Trim();
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                        }
                    }

                    if(pAcp.inOutCls != "O" && dt.Rows[i]["RES"].ToString().Trim() == "1")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = "(예약)" + ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text;
                    }


                    ssOrder_Sheet1.Cells[i, (int)Order.Content].Text = VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim()) == 0 ?
                        "" : dt.Rows[i]["CONTENTS"].ToString().Trim();

                    ssOrder_Sheet1.Cells[i, (int)Order.RealQty].Text = dt.Rows[i]["RealQty"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.DosName].Text = dt.Rows[i]["DosName"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.DosName].Text =
                        dt.Rows[i]["REMARK"].ToString().Trim() == "PRN" && dt.Rows[i]["GBACT"].ToString().Trim() == "*" ?
                    "(간호사수행)" + ssOrder_Sheet1.Cells[i, (int)Order.DosName].Text  : ssOrder_Sheet1.Cells[i, (int)Order.DosName].Text;

                    ssOrder_Sheet1.Cells[i, (int)Order.GbGroup].Text = dt.Rows[i]["GbGroup"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.GbDiv].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.Nal].Text = dt.Rows[i]["NAL"].ToString().Trim();


                    if(pAcp.inOutCls == "I")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.Sign].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    }
                    else if (pAcp.inOutCls == "O" && pAcp.medDeptCd != "ER")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.Sign].Text =
                            GetDrNm(dt.Rows[i]["DRCODE"].ToString().Trim(), dt.Rows[i]["BDATE"].ToString().Trim());
                    }
                    else if (pAcp.inOutCls == "O" && pAcp.medDeptCd == "ER")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.Sign].Text = "";
                    }

                    //string strRgb = dt.Rows[i]["cDispRGB"].ToString().Trim();
                    //ssOrder_Sheet1.Rows[i].ForeColor = strRgb.Length == 0 || strRgb == "00000000" ? Color.Black :
                    //    ColorTranslator.FromOle((int) new Int32Converter().ConvertFromString(string.Format("0x{0}", strRgb)));

                    if (ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text.Length >= 70)
                    {
                        ssOrder_Sheet1.Rows[i].Height = ssOrder_Sheet1.Rows[i].GetPreferredHeight() + 15;
                    }
                }

                dt.Dispose();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        string ReadOptionDate(string argGrade, string ArgIO, string ArgDeptCode, string argUSEID, string argDATE)
        {
            string rtnVal = string.Empty;

            switch (argUSEID)
            {

                case "45924":
                case "22536":
                case "36522":
                case "44892":
                    rtnVal = VB.Left(pAcp.medFrDate, 4) + "-" +
                        VB.Mid(pAcp.medFrDate, 5, 2) + "-" +
                        VB.Right(pAcp.medFrDate, 2);
                return rtnVal;
            }

            if (argGrade != "SIMSA")
            {
                return argDATE;
            }

            OracleDataReader reader = null;

            try
            {
                StringBuilder SQL = new StringBuilder();
                SQL.AppendLine(" SELECT NAL ");
                SQL.AppendLine(" FROM ADMIN.EMR_OPTION_SETDATE ");
                SQL.AppendLine(" WHERE IO = '" + ArgIO + "' ");
                SQL.AppendLine("   AND USEID = " + argUSEID);
                if (ArgIO != "I")
                {
                    SQL.AppendLine("   AND DEPTCODE = '" + ArgDeptCode + "' ");
                }
                SQL.AppendLine("   AND USED = '1' ");

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal; 
                }

                if(reader.HasRows && reader.Read())
                {
                    if(reader.GetValue(0).ToString().Trim().Length == 0 ||
                       VB.IsNumeric(reader.GetValue(0).ToString().Trim()) == false)
                    {
                        rtnVal = argDATE;
                        reader.Dispose();
                        return rtnVal;
                    }

                    rtnVal = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).
                        AddDays(VB.Val(reader.GetValue(0).ToString().Trim()) * -1).ToShortDateString();
                }

                reader.Dispose();
            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (mstrOrderDate != "")
            {
                dtpOptSDate.Enabled = false;
                dtpOptEDate.Enabled = false;
                //panBtn.Visible = false;
                GetChartHistoryCopy();
                return;
            }

            //GetOrder();
        }



        /// <summary>
        /// 버튼 조회 함수
        /// </summary>
        void GetOrder()
        {
            ssOrder_Sheet1.RowCount = 0;
            StringBuilder SQL = new StringBuilder();
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = null;

            try
            {
                #region 쿼리
                if (pAcp.inOutCls == "I")
                {
                    SQL.AppendLine("  SELECT TO_CHAR(O.BDate, 'YYYY-MM-DD') BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames, O.REMARK,");
                    SQL.AppendLine("    C.DispHeader, O.Qty, O.CONTENTS, O.GbGroup,");
                    SQL.AppendLine("    O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE, '' RES ");
                    SQL.AppendLine("    FROM ADMIN.OCS_IORDER O,");
                    SQL.AppendLine("          ADMIN.OCS_ORDERCODE C,");
                    SQL.AppendLine("          ADMIN.OCS_ODOSAGE D,");
                    SQL.AppendLine("          ADMIN.OCS_DOCTOR N,");
                    SQL.AppendLine("          ADMIN.BAS_SUN     S");
                    SQL.AppendLine("      WHERE O.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("          AND O.BDATE >= TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");

                    if (mstrOrderDate != "")
                    {
                        SQL.AppendLine("          AND O.BDATE = TO_DATE('" + VB.Val(mstrOrderDate).ToString("0000-00-00") + "','YYYY-MM-DD') ");
                    }
                    else
                    {
                        SQL.AppendLine("          AND O.BDATE >= TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");

                        if (pAcp.medEndDate != "")
                        {
                            SQL.AppendLine("          AND O.BDATE <= TO_DATE('" + dtpOptEDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");
                        }
                    }


                    if (pAcp.inOutCls == "O")
                    {
                        if (pAcp.medDeptCd == "RA" ||
                            (pAcp.medDeptCd == "MD" &&
                            (pAcp.medDrCd == "1107" ||
                             pAcp.medDrCd == "1125")))
                        {
                            SQL.AppendLine("                AND O.DEPTCODE = 'MD'");
                            SQL.AppendLine("                AND O.DRCODE IN ('1107','1125')");
                        }
                        else
                        {
                            SQL.AppendLine("                AND O.DEPTCODE = '" + pAcp.medDeptCd + "'");
                            SQL.AppendLine("                AND O.DRCODE NOT IN ('1107','1125')");
                        }
                    }
                    else
                    {
                        SQL.AppendLine("                AND O.DEPTCODE = '" + pAcp.medDeptCd + "'");
                    }

                    if (!mViewNpChart)
                    {
                        SQL.AppendLine("        AND O.DEPTCODE <> 'NP'");
                    }

                    SQL.AppendLine("          AND O.GBSTATUS NOT IN ('D-','D','D+' )  ");
                    SQL.AppendLine("          AND    O.SlipNo     =  C.SlipNo(+)      ");
                    SQL.AppendLine("          AND    O.OrderCode  =  C.OrderCode(+)   ");
                    SQL.AppendLine("          AND    O.DosCode    =  D.DosCode(+)     ");
                    SQL.AppendLine("          AND    O.DRCODE      =  N.SABUN(+)      ");
                    SQL.AppendLine("          AND    (O.GBIOE NOT IN ('E','EI') OR O.GBIOE IS NULL OR GBIOE = '')");
                    SQL.AppendLine("          AND    O.SUCODE = S.SUNEXT(+) ");
                    SQL.AppendLine("          AND (NOT (O.GBVERB = 'Y' AND O.VERBC IS NULL)  OR O.GBVERB IS NULL)");
                    SQL.AppendLine("          ORDER BY O.BDATE DESC, O.Slipno, O.Seqno");

                }
                else if (pAcp.inOutCls == "O" && pAcp.medDeptCd != "ER" && pAcp.formNo != 2090)
                {
                    SQL.AppendLine("  SELECT TO_CHAR(O.BDate, 'YYYY-MM-DD') BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames,  O.REMARK,");
                    SQL.AppendLine("    C.DispHeader, O.Qty, O.RealQty AS CONTENTS, '' AS GbGroup,");
                    SQL.AppendLine("     O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE, O.RES ");
                    SQL.AppendLine("    FROM ADMIN.OCS_OORDER O,");
                    SQL.AppendLine("          ADMIN.OCS_ORDERCODE C,");
                    SQL.AppendLine("          ADMIN.OCS_ODOSAGE D,");
                    SQL.AppendLine("          ADMIN.OCS_DOCTOR N,");
                    SQL.AppendLine("          ADMIN.BAS_SUN     S)");
                    SQL.AppendLine("      WHERE O.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("          AND O.BDATE = TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");

                    if (pAcp.inOutCls == "O")
                    {
                        if (pAcp.medDeptCd == "RA" ||
                            (pAcp.medDeptCd == "MD" &&
                            (pAcp.medDrCd == "1107" ||
                            pAcp.medDrCd == "1125")))
                        {
                            SQL.AppendLine("                AND O.DEPTCODE = 'MD'");
                            SQL.AppendLine("                AND O.DRCODE IN ('1107','1125')");
                        }
                        else
                        {
                            SQL.AppendLine("                AND O.DEPTCODE = '" + pAcp.medDeptCd + "'");
                            SQL.AppendLine("                AND O.DRCODE NOT IN ('1107','1125')");
                        }
                    }

                    if (!mViewNpChart)
                    {
                        SQL.AppendLine("        AND O.DEPTCODE <> 'NP'");
                    }

                    SQL.AppendLine("          AND    O.GBSUNAP = '1' AND O.Seqno    > 0   AND O.NAL      > 0");
                    SQL.AppendLine("          AND    O.SlipNo     =  C.SlipNo(+)      ");
                    SQL.AppendLine("          AND    O.OrderCode  =  C.OrderCode(+)   ");
                    SQL.AppendLine("          AND    O.DosCode    =  D.DosCode(+)     ");
                    SQL.AppendLine("          AND    O.DRCODE      =  N.SABUN(+)      ");

                    SQL.AppendLine("          AND    O.SUCODE = S.SUNEXT(+) ");
                    SQL.AppendLine("          ORDER BY O.BDATE DESC, O.GbSunap DESC, O.Slipno, O.Seqno");
                }
                else if ((pAcp.inOutCls == "O" &&
                         pAcp.medDeptCd == "ER") ||
                         pAcp.formNo == 2090)
                {
                    SQL.AppendLine("  SELECT TO_CHAR(O.BDate, 'YYYY-MM-DD') BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames, O.REMARK,");
                    SQL.AppendLine("    C.DispHeader, O.Qty, O.CONTENTS, O.GbGroup,");
                    SQL.AppendLine("    O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE , '' RES");
                    SQL.AppendLine("    FROM ADMIN.OCS_IORDER O,");
                    SQL.AppendLine("          ADMIN.OCS_ORDERCODE C,");
                    SQL.AppendLine("          ADMIN.OCS_ODOSAGE D,");
                    SQL.AppendLine("          ADMIN.OCS_DOCTOR N,");
                    SQL.AppendLine("          ADMIN.BAS_SUN     S");
                    SQL.AppendLine("      WHERE O.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("          AND O.BDATE >= TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");
                    SQL.AppendLine("          AND O.BDATE <= TO_DATE('" + dtpOptSDate.Value.AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ");
                    SQL.AppendLine("          AND O.GBSTATUS NOT IN ('D-','D','D+' )  ");
                    SQL.AppendLine("          AND O.SlipNo     =  C.SlipNo(+)      ");
                    SQL.AppendLine("          AND O.OrderCode  =  C.OrderCode(+)   ");
                    SQL.AppendLine("          AND O.DosCode    =  D.DosCode(+)     ");
                    SQL.AppendLine("          AND O.DRCODE      =  N.SABUN(+)      ");
                    SQL.AppendLine("          AND O.GBIOE IN ('E','EI') ");
                    SQL.AppendLine("          AND (NOT (O.GBVERB = 'Y' AND O.VERBC IS NULL)  OR O.GBVERB IS NULL)");
                    SQL.AppendLine("          AND O.SUCODE = S.SUNEXT(+) ");
                    SQL.AppendLine("          ORDER BY O.BDATE DESC, O.Slipno, O.Seqno");
                }
                #endregion

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssOrder_Sheet1.RowCount = dt.Rows.Count;
                ssOrder_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ssOrder_Sheet1.Cells[i, (int)Order.Bdate].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.OrderCode].Text = dt.Rows[i]["OrderCode"].ToString().Trim();

                    if (dt.Rows[i]["BUN"].ToString().Trim().Length == 0 ||
                        dt.Rows[i]["BUN"].ToString().Trim() == "10")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    }
                    else
                    {
                        if (dt.Rows[i]["ORDERNAMES"].ToString().Trim().Length > 0)
                        {
                            string strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["DispHeader"].ToString().Trim().Length > 0)
                        {
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text =
                                dt.Rows[i]["DispHeader"].ToString().Trim() + 
                                dt.Rows[i]["OrderName"].ToString().Trim();
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                        }
                    }

                    if(dt.Rows[i]["RES"].ToString().Trim() == "1")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = "(예약)" + ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text;
                    }


                    ssOrder_Sheet1.Cells[i, (int)Order.Content].Text = VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim()) == 0 ?
                        "" : dt.Rows[i]["CONTENTS"].ToString().Trim();

                    ssOrder_Sheet1.Cells[i, (int)Order.RealQty].Text = dt.Rows[i]["RealQty"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.DosName].Text = dt.Rows[i]["DosName"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.GbGroup].Text = dt.Rows[i]["GbGroup"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.GbDiv].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.Nal].Text = dt.Rows[i]["NAL"].ToString().Trim();


                    ssOrder_Sheet1.Cells[i, (int)Order.Sign].Text = pAcp.inOutCls == "I" ?
                        dt.Rows[i]["DrName"].ToString().Trim() :
                        GetDrNm(dt.Rows[i]["DRCODE"].ToString().Trim(), dt.Rows[i]["BDATE"].ToString().Trim());



                }

                dt.Dispose();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        string GetDrNm(string strDrCode, string strBdate)
        {
            string rtnVal = string.Empty;
            OracleDataReader reader = null;

            try
            {
                StringBuilder SQL = new StringBuilder();
                SQL.AppendLine("SELECT DRNAME FROM ADMIN.OCS_DOCTOR ");
                SQL.AppendLine("          WHERE DRCODE = '" + strDrCode + "'");
                SQL.AppendLine("          AND ((GBOUT <> 'Y')");
                SQL.AppendLine("              OR (GBOUT = 'Y' AND ");
                SQL.AppendLine("                      (FDATE >= TO_DATE('" + strBdate + "','YYYY-MM-DD'))");
                SQL.AppendLine("                      AND (TDATE <= TO_DATE('" + strBdate + "','YYYY-MM-DD'))))");

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                if(reader.HasRows &&
                   reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            pPrintForm();
            //PrintCopy("");
        }

        //void PrintCopy(string PrintType)
        //{
        //    DataTable dt = null;

        //    string strPatName = string.Empty;
        //    string strSex = string.Empty;
        //    string strAge = string.Empty;
        //    string strJumin = string.Empty;

        //    try
        //    {
        //        StringBuilder SQL = new StringBuilder();
        //        string SQL = " SELECT SNAME, SEX, JUMIN1, JUMIN2 ";
        //        SQL.AppendLine(" FROM ADMIN.BAS_PATIENT ";
        //        SQL.AppendLine(" WHERE PANO = '" + pAcp.ptNo + "' ";

        //        string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
        //        if (sqlErr.Length > 0)
        //        {
        //            ComFunc.MsgBoxEx(this, sqlErr);
        //            return;
        //        }

        //        if (dt.Rows.Count > 0)
        //        {
        //            strPatName = dt.Rows[0]["SNAME"].ToString().Trim();
        //            strSex = dt.Rows[0]["SEX"].ToString().Trim();
        //            strAge = ComFunc.AgeCalc(clsDB.DbCon,
        //                dt.Rows[0]["JUMIN1"].ToString().Trim() +
        //                dt.Rows[0]["JUMIN2"].ToString().Trim()).ToString();
        //            strJumin = string.Format("{0}-{1}******",
        //               dt.Rows[0]["JUMIN1"].ToString().Trim(),
        //               VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1));

        //        }

        //        dt.Dispose();



        //        string strFormName = "Dr Order";

        //        'Print Head 지정
        //        string strFont1 = @"/fn""바탕체"" /fz""14"" /fb1 /fi0 /fu0 /fk0 /fs1";
        //        string strFont2 = @"/fn""바탕체"" /fz""10"" /fb0 /fi0 /fu0 /fk0 /fs2";
        //        string strHead1 = "/c/f1" + strFormName + "/f1/n/n";
        //        string strHead2 = "/n/l/f2" + "환자번호 : " + pAcp.ptNo +
        //                          VB.Space(3) + "환자성명 : " + strPatName + VB.Space(3) + "성별/나이 : " + strSex + "/" + strAge + "세" + "  주민번호 : " + strJumin + "/n/n";
        //        string strFooter = "/n/l/f2" + "포항성모병원" + VB.Space(40) + "출력일자 : " + VB.Format(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "@@@@-@@-@@") + VB.Space(40) + "출력자 : " + clsType.User.UserName;
        //        strFooter = strFooter + "/n/l/f2" + "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.";
        //        strFooter = strFooter + "/n/l/f2" + "This is an electronically authorized offical medical record";

        //        ssOrder_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
        //        ssOrder_Sheet1.PrintInfo.Footer = strFooter;
        //        ssOrder_Sheet1.PrintInfo.Margin.Left = 20;
        //        ssOrder_Sheet1.PrintInfo.Margin.Right = 20;
        //        ssOrder_Sheet1.PrintInfo.Margin.Top = 20;
        //        ssOrder_Sheet1.PrintInfo.Margin.Bottom = 20;

        //        ssOrder_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
        //        ssOrder_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
        //        ssOrder_Sheet1.PrintInfo.ShowBorder = true;
        //        ssOrder_Sheet1.PrintInfo.ShowColor = false;
        //        ssOrder_Sheet1.PrintInfo.ShowGrid = true;
        //        ssOrder_Sheet1.PrintInfo.ShowShadows = false;
        //        ssOrder_Sheet1.PrintInfo.UseMax = false;
        //        ssOrder_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;

        //        if (PrintType == "V")
        //        {
        //            ssOrder_Sheet1.PrintInfo.Preview = true;
        //        }

        //        ssOrder.PrintSheet(0);
        //    }
        //    catch (Exception ex)
        //    {
        //        Cursor.Current = Cursors.Default;
        //        ComFunc.MsgBoxEx(this, ex.Message);
        //    }

        //}

        private void SsOrder_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssOrder_Sheet1.RowCount == 0)
                return;

            if(e.Column == 1)
            {
                string strCode = ssOrder_Sheet1.Cells[e.Row, e.Column].Text.Trim();
                if(strCode.Length > 0)
                {
                    StringBuilder SQL = new StringBuilder();

                    SQL.AppendLine(" SELECT BUN ");
                    SQL.AppendLine(" FROM ADMIN.BAS_SUT ");
                    SQL.AppendLine(" WHERE SUNEXT = '" + strCode + "' ");
                    SQL.AppendLine("   AND BUN IN ('11','12','20','23')");

                    //OracleDataReader reader = null;
                    //string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    //if (sqlErr.Length > 0)
                    //{
                    //    ComFunc.MsgBoxEx(this, sqlErr);
                    //    return;
                    //}

                    //if(reader.HasRows)
                    //{

                    //}
                }
            }
        }
    }
}
