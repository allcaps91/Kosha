using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrOrderOld : Form, EmrChartForm
    {
        string mFLOWGB = "COL"; //서식작성 방향
        int mFLOWITEMCNT = 0;
        int mFLOWHEADCNT = 0;

        //int mRow = 0;
        //int mCol = 0;
        FormFlowSheet[] mFormFlowSheet = null;
        FormFlowSheetHead[,] mFormFlowSheetHead = null;

        FarPoint.Win.Spread.CellType.CheckBoxCellType TypeCheckBox = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
        FarPoint.Win.Spread.CellType.ComboBoxCellType TypeComboBox = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
        FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();


        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //Form mCallForm = null;
        FormEmrMessage mEmrCallForm = null;
        EmrPatient pAcp = null;

        public string mstrFormNo = "2185";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public string mstrEmrNo = "42694720";  //961 131641  //963 735603
        public string mstrMode = "W";

        public string mstrOrderDate = "";
        public bool mViewNpChart = false;
        #endregion

        #region // 상용구 관련 변수 선언
        //private Control mControl = null;
        //private frmMacrowordProg frmMacrowordProgEvent;

        //private Control mCalControl = null; //달력 띄우기
        //private frmEmrCaledar frmEmrCaledarEvent;

        //private FarPoint.Win.Spread.FpSpread ssMacroWord;
        //private FarPoint.Win.Spread.SheetView ssMacroWord_Sheet1;
        #endregion // 상용구 관련 모듈

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

            //if (strPRINTFLAG == "N")
            //{
            //    frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
            //    frmEmrPrintOptionX.ShowDialog();
            //}

            //if (clsFormPrint.mstrPRINTFLAG == "-1")
            //{
            //    return rtnVal;
            //}

            //if (clsEmrQuery.SaveEmrXmlPrnYnForm(mstrEmrNo, "0") == false)
            //{
            //    return rtnVal;
            //}

            //rtnVal = clsFormPrint.PrintFormLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");

            rtnVal = clsSpreadPrint.PrintSpdFormCnt(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dateTimePicker1.Value.ToString("yyyyMMdd"), dateTimePicker2.Value.ToString("yyyyMMdd"), ssView, "P", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Portrait, clsPublic.GstrSysDate);

            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //rtnVal = clsFormPrint.PrintToTifFileLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        #endregion
        
        public frmEmrOrderOld()
        {
            InitializeComponent();
        }

        public frmEmrOrderOld(string pMake, int pItemCnt, int pHeadCnt, FormFlowSheet[] pFormFlowSheet, FormFlowSheetHead[,] pFormFlowSheetHead)
        {
            InitializeComponent();
            mFLOWGB = pMake;
            mFLOWITEMCNT = pItemCnt;
            mFLOWHEADCNT = pHeadCnt;
            mFormFlowSheet = pFormFlowSheet;
            mFormFlowSheetHead = pFormFlowSheetHead;
        }

        public frmEmrOrderOld(string strFormNo, string strUpdateNo, string strEmrNo)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
        }

        public frmEmrOrderOld(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            InitializeComponent();
            //pInitForm();
            //LoadForm();
        }

        private void frmEmrOrderOld_Load(object sender, EventArgs e)
        {
            //if (mstrFormNo != "0")
            //{
            //FormDesignQuery.GetSetDate_AEMRFLOWXML(mstrFormNo, mstrUpdateNo, ref mFLOWGB, ref mFLOWITEMCNT, ref mFLOWHEADCNT, ref mFormFlowSheet, ref mFormFlowSheetHead);
            //}

            //if (mFormFlowSheetHead != null)
            //{
            //SetWriteSpd();
            //}            
            GetChartHistoryCopy();
        }

        private void SetPatInfo()
        {
            ssView_Sheet1.RowCount = 0;
            dateTimePicker1.Text = ReadOptionDate(clsEmrPublic.strUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.Sabun, pAcp.medFrDate);
            if (pAcp.inOutCls == "O")
            {
                dateTimePicker1.Text = ReadOptionDate(clsEmrPublic.strUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.Sabun, pAcp.medFrDate);
            }
            else
            {
                if (pAcp.medEndDate == "")
                {
                    dateTimePicker2.Text = ReadOptionDate(clsEmrPublic.strUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.Sabun, clsPublic.GstrSysDate);
                }
                else
                {
                    dateTimePicker2.Text = ReadOptionDate(clsEmrPublic.strUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.Sabun, pAcp.medEndDate);
                }

                if (mstrOrderDate != "")
                {
                    dateTimePicker1.Text = ReadOptionDate(clsEmrPublic.strUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.Sabun, mstrOrderDate);
                    dateTimePicker2.Text = ComFunc.FormatStrToDateEx(mstrOrderDate.Replace("-",""),"D","-");
                }
            }

            //'정신과차트조회권한
            mViewNpChart = clsEmrQueryOld.ViewNPChart(clsType.User.Sabun);
            if (pAcp.ptNo != "")
            {
                GetChartHistory();
            }
        }

        private void GetChartHistory()
        {
            GetOrder();
        }

        private void GetChartHistoryCopy()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strUnit = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {

                ssView_Sheet1.RowCount = 0;
                dateTimePicker1.Text = ReadOptionDate(clsEmrPublic.strUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.Sabun, pAcp.medFrDate);
                if (pAcp.inOutCls == "O")
                {
                    dateTimePicker1.Text = ReadOptionDate(clsEmrPublic.strUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.Sabun, pAcp.medFrDate);
                }
                else
                {
                    if (pAcp.medEndDate == "")
                    {
                        dateTimePicker2.Text = ReadOptionDate(clsEmrPublic.strUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.Sabun, clsPublic.GstrSysDate);
                    }
                    else
                    {
                        dateTimePicker2.Text = ReadOptionDate(clsEmrPublic.strUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.Sabun, pAcp.medEndDate);
                    }

                    if (mstrOrderDate != "")
                    {
                        dateTimePicker1.Text = ReadOptionDate(clsEmrPublic.strUserGrade, pAcp.inOutCls, pAcp.medDeptCd, clsType.User.Sabun, mstrOrderDate);
                        dateTimePicker2.Text = ComFunc.FormatStrToDateEx(mstrOrderDate.Replace("-", ""), "D", "-");
                    }
                }

                if (pAcp.inOutCls == "I")
                {
                    SQL = "  SELECT O.BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames, O.REMARK,";
                    SQL = SQL + ComNum.VBLF + "    C.DispHeader, O.Qty, O.CONTENTS, O.GbGroup,";
                    SQL = SQL + ComNum.VBLF + "    O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE, '' RES ";
                    SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_OCS.OCS_IORDER O,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_ORDERCODE C,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_ODOSAGE D,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_DOCTOR N,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_PMPA.BAS_SUN     S";
                    SQL = SQL + ComNum.VBLF + "      WHERE O.PTNO = '" + pAcp.ptNo + "' ";
                    if (mstrOrderDate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "          AND O.BDATE = TO_DATE('" + mstrOrderDate + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         AND O.BDATE >= TO_DATE('" + dateTimePicker1.Value.ToShortDateString() + "','YYYY-MM-DD') ";
                        if (pAcp.medEndDate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "          AND O.BDATE <= TO_DATE('" + dateTimePicker2.Value.ToShortDateString() + "','YYYY-MM-DD') ";
                        }
                    }

                    if (pAcp.inOutCls == "O")
                    {
                        if (pAcp.medDeptCd == "RA" || (pAcp.medDeptCd == "MD" && (pAcp.medDrCd == "1107" || pAcp.medDrCd == "1125")))
                        {
                            SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = 'MD'";
                            SQL = SQL + ComNum.VBLF + "                AND O.DRCODE IN ('1107','1125')";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = '" + pAcp.medDeptCd + "'";
                            SQL = SQL + ComNum.VBLF + "                AND O.DRCODE NOT IN ('1107','1125')";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = '" + pAcp.medDeptCd + "'";
                    }
                    if (mViewNpChart == false) SQL = SQL + ComNum.VBLF + "        AND O.DEPTCODE <> 'NP'";

                    SQL = SQL + ComNum.VBLF + "          AND O.GBSTATUS NOT IN ('D-','D','D+' )  ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SlipNo     =  C.SlipNo(+)      ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.OrderCode  =  C.OrderCode(+)   ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DosCode    =  D.DosCode(+)     ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DRCODE      =  N.SABUN(+)      ";
                    SQL = SQL + ComNum.VBLF + "          AND    (O.GBIOE NOT IN ('E','EI') OR O.GBIOE IS NULL OR GBIOE = '')";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SUCODE = S.SUNEXT(+) ";
                    SQL = SQL + ComNum.VBLF + "          AND (NOT (O.GBVERB = 'Y' AND O.VERBC IS NULL)  OR O.GBVERB IS NULL)";
                    SQL = SQL + ComNum.VBLF + "          ORDER BY O.BDATE DESC, O.Slipno, O.Seqno";
                }
                else if (pAcp.inOutCls == "O" && pAcp.medDeptCd != "ER" && mstrFormNo != "2090")
                {
                    SQL = "  SELECT O.BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames,  O.REMARK,";
                    SQL = SQL + ComNum.VBLF + "    C.DispHeader, O.Qty, O.RealQty AS CONTENTS, '' AS GbGroup,";
                    SQL = SQL + ComNum.VBLF + "     O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE, O.RES ";
                    SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_OCS.OCS_OORDER O,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_ORDERCODE C,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_ODOSAGE D,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_DOCTOR N,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_PMPA.BAS_SUN     S";
                    SQL = SQL + ComNum.VBLF + "      WHERE O.PTNO = '" + pAcp.ptNo + "' ";
                    SQL = SQL + ComNum.VBLF + "          AND O.BDATE = TO_DATE('" + dateTimePicker1.Value.ToShortDateString() + "','YYYY-MM-DD') ";

                    if (pAcp.inOutCls == "O")
                    {
                        if (pAcp.medDeptCd == "RA" || (pAcp.medDeptCd == "MD" && (pAcp.medDrCd == "1107" || pAcp.medDrCd == "1125")))
                        {
                            SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = 'MD'";
                            SQL = SQL + ComNum.VBLF + "                AND O.DRCODE IN ('1107','1125')";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = '" + pAcp.medDeptCd + "'";
                            SQL = SQL + ComNum.VBLF + "                AND O.DRCODE NOT IN ('1107','1125')";
                        }
                    }
                    if (mViewNpChart == false) SQL = SQL + ComNum.VBLF + "        AND O.DEPTCODE <> 'NP'";

                    SQL = SQL + ComNum.VBLF + "          AND    O.GBSUNAP ='1' AND O.Seqno    > '0'   AND O.NAL      > '0'";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SlipNo     =  C.SlipNo(+)      ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.OrderCode  =  C.OrderCode(+)   ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DosCode    =  D.DosCode(+)     ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DRCODE      =  N.SABUN(+)      ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SUCODE = S.SUNEXT(+) ";
                    SQL = SQL + ComNum.VBLF + "          ORDER BY O.BDATE DESC, O.GbSunap DESC, O.Slipno, O.Seqno";
                }
                else if ((pAcp.inOutCls == "O" && pAcp.medDeptCd == "ER") || (mstrFormNo == "2090"))
                {
                    SQL = "  SELECT O.BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames, O.REMARK,";
                    SQL = SQL + ComNum.VBLF + "  C.DispHeader, O.Qty, O.CONTENTS, O.GbGroup,";
                    SQL = SQL + ComNum.VBLF + "  O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE , '' RES";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_IORDER O,";
                    SQL = SQL + ComNum.VBLF + "       KOSMOS_OCS.OCS_ORDERCODE C,";
                    SQL = SQL + ComNum.VBLF + "       KOSMOS_OCS.OCS_ODOSAGE D,";
                    SQL = SQL + ComNum.VBLF + "       KOSMOS_OCS.OCS_DOCTOR N,";
                    SQL = SQL + ComNum.VBLF + "       KOSMOS_PMPA.BAS_SUN     S";
                    SQL = SQL + ComNum.VBLF + "  WHERE O.PTNO = '" + pAcp.ptNo + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND O.BDATE >= TO_DATE('" + dateTimePicker1.Value.ToShortDateString() + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND O.BDATE <= TO_DATE('" + dateTimePicker1.Value.AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND O.GBSTATUS NOT IN ('D-','D','D+' )  ";
                    SQL = SQL + ComNum.VBLF + "    AND    O.SlipNo     =  C.SlipNo(+)      ";
                    SQL = SQL + ComNum.VBLF + "    AND    O.OrderCode  =  C.OrderCode(+)   ";
                    SQL = SQL + ComNum.VBLF + "    AND    O.DosCode    =  D.DosCode(+)     ";
                    SQL = SQL + ComNum.VBLF + "    AND    O.DRCODE      =  N.SABUN(+)      ";
                    SQL = SQL + ComNum.VBLF + "    AND    O.GBIOE IN ('E','EI') ";
                    SQL = SQL + ComNum.VBLF + "    AND (NOT (O.GBVERB = 'Y' AND O.VERBC IS NULL)  OR O.GBVERB IS NULL)";
                    SQL = SQL + ComNum.VBLF + "    AND    O.SUCODE = S.SUNEXT(+) ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY O.BDATE DESC, O.Slipno, O.Seqno";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        if (dt.Rows[i]["BUN"].ToString().Trim() == "" || dt.Rows[i]["BUN"].ToString().Trim() == "10")
                        {
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        }
                        else
                        {
                            if (dt.Rows[i]["ORDERNAMES"].ToString().Trim() != "")
                            {
                                strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 2].Text = strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                            }
                            else if (dt.Rows[i]["DISPHEADER"].ToString().Trim() != "")
                            {
                                ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DISPHEADER"].ToString().Trim() + " " + dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            }
                            else
                            {
                                ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            }
                        }

                        if (dt.Rows[i]["RES"].ToString().Trim() == "1")
                        {
                            ssView_Sheet1.Cells[i, 2].Text = "(예약)" + ssView_Sheet1.Cells[i, 2].Text;
                        }
                        ssView_Sheet1.Cells[i, 3].Text = (VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim()) == 0 ? "" : dt.Rows[i]["CONTENTS"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RealQty"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DosName"].ToString().Trim();
                        if (dt.Rows[i]["REMARK"].ToString().Trim() == "PRN" && dt.Rows[i]["GBACT"].ToString().Trim() == "*")
                        {
                            ssView_Sheet1.Cells[i, 5].Text = "(간호사수행)" + ssView_Sheet1.Cells[i, 5].Text;
                        }
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GbGroup"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        //'외래는 의사코드로 움직이는데 불명확하다..??????
                        if (pAcp.inOutCls == "I")
                        {
                            ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 9].Text = clsVbfunc.GetOCSDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
                        }

                        if (dt.Rows[i]["cDispRGB"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Rows[i].ForeColor = System.Drawing.ColorTranslator.FromWin32(int.Parse(dt.Rows[i]["cDispRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
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
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private string ReadOptionDate(string strGrade, string strIO, string strDeptCode, string strUseId, string strDate)
        {
            string rtnVal = "";            
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strNal = "";

            Cursor.Current = Cursors.WaitCursor;
            
            try
            {
                ComFunc.ReadSysDate(clsDB.DbCon);
                rtnVal = clsPublic.GstrSysDate;

                if (strUseId == "45924" || strUseId == "22536" || strUseId == "36522" || strUseId == "44892")
                {
                    rtnVal = ComFunc.FormatStrToDateEx(pAcp.medFrDate, "D", "-");
                    return rtnVal;
                }

                if (strGrade != "SIMSA")
                {
                    rtnVal = ComFunc.FormatStrToDateEx(strDate.Replace("-",""), "D", "-"); ;
                    return rtnVal;
                }

                SQL = " SELECT NAL ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_OPTION_SETDATE ";
                SQL = SQL + ComNum.VBLF + " WHERE IO = '" + strIO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND USEID = " + strUseId;
                if (strIO != "I")
                {
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "' ";
                }
                SQL = SQL + ComNum.VBLF + "   AND USED = '1' ";
                
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                
                if (dt.Rows.Count > 0)
                {                    
                    strNal = dt.Rows[0]["NAL"].ToString().Trim();                    
                }
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;

                if (strNal == "" || VB.IsNumeric(strNal) == false)
                {
                    rtnVal = ComFunc.FormatStrToDateEx(strDate.Replace("-", ""), "D", "-"); ;
                    return rtnVal;
                }
                rtnVal = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays((int)VB.Val(strNal) * -1).ToShortDateString();
                return rtnVal;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
                return rtnVal;
            }
        }

        private void GetOrder()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strUnit = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {                                
                if (pAcp.inOutCls == "I")
                {
                    SQL = "  SELECT O.BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames, O.REMARK,";
                    SQL = SQL + ComNum.VBLF + "    C.DispHeader, O.Qty, O.CONTENTS, O.GbGroup,";
                    SQL = SQL + ComNum.VBLF + "    O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE, '' RES ";
                    SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_OCS.OCS_IORDER O,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_ORDERCODE C,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_ODOSAGE D,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_DOCTOR N,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_PMPA.BAS_SUN     S";
                    SQL = SQL + ComNum.VBLF + "      WHERE O.PTNO = '" +pAcp.ptNo + "' ";
                    if (mstrOrderDate != "")
                    {
                        SQL = SQL + ComNum.VBLF + "          AND O.BDATE = TO_DATE('" + mstrOrderDate + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         AND O.BDATE >= TO_DATE('" + dateTimePicker1.Value.ToShortDateString() + "','YYYY-MM-DD') ";
                        if (pAcp.medEndDate != "")
                        {
                            SQL = SQL + ComNum.VBLF + "          AND O.BDATE <= TO_DATE('" + dateTimePicker2.Value.ToShortDateString() + "','YYYY-MM-DD') ";
                        }                        
                    }

                    if (pAcp.inOutCls == "O")
                    {
                        if( pAcp.medDeptCd == "RA" ||(pAcp.medDeptCd == "MD" &&(pAcp.medDrCd == "1107" || pAcp.medDrCd == "1125")) )
                        {
                            SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = 'MD'";
                            SQL = SQL + ComNum.VBLF + "                AND O.DRCODE IN ('1107','1125')";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = '" + pAcp.medDeptCd + "'";
                            SQL = SQL + ComNum.VBLF + "                AND O.DRCODE NOT IN ('1107','1125')";
                        }
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = '" + pAcp.medDeptCd + "'";
                    }
                    if (mViewNpChart == false) SQL = SQL + ComNum.VBLF + "        AND O.DEPTCODE <> 'NP'";

                    SQL = SQL + ComNum.VBLF + "          AND O.GBSTATUS NOT IN ('D-','D','D+' )  ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SlipNo     =  C.SlipNo(+)      ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.OrderCode  =  C.OrderCode(+)   ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DosCode    =  D.DosCode(+)     ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DRCODE      =  N.SABUN(+)      ";
                    SQL = SQL + ComNum.VBLF + "          AND    (O.GBIOE NOT IN ('E','EI') OR O.GBIOE IS NULL OR GBIOE = '')";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SUCODE = S.SUNEXT(+) ";
                    SQL = SQL + ComNum.VBLF + "          AND (NOT (O.GBVERB = 'Y' AND O.VERBC IS NULL)  OR O.GBVERB IS NULL)";
                    SQL = SQL + ComNum.VBLF + "          ORDER BY O.BDATE DESC, O.Slipno, O.Seqno";
                }
                else if (pAcp.inOutCls == "O" && pAcp.medDeptCd != "ER" && mstrFormNo != "2090")
                {
                    SQL = "  SELECT O.BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames,  O.REMARK,";
                    SQL = SQL + ComNum.VBLF + "    C.DispHeader, O.Qty, O.RealQty AS CONTENTS, '' AS GbGroup,";
                    SQL = SQL + ComNum.VBLF + "     O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE, O.RES ";
                    SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_OCS.OCS_OORDER O,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_ORDERCODE C,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_ODOSAGE D,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_OCS.OCS_DOCTOR N,";
                    SQL = SQL + ComNum.VBLF + "          KOSMOS_PMPA.BAS_SUN     S";
                    SQL = SQL + ComNum.VBLF + "      WHERE O.PTNO = '" + pAcp.ptNo + "' ";
                    SQL = SQL + ComNum.VBLF + "          AND O.BDATE = TO_DATE('" + dateTimePicker1.Value.ToShortDateString() + "','YYYY-MM-DD') ";

                    if (pAcp.inOutCls == "O")
                    {
                        if(pAcp.medDeptCd == "RA" ||(pAcp.medDeptCd == "MD" &&(pAcp.medDrCd == "1107" || pAcp.medDrCd == "1125")) )
                            {
                            SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = 'MD'";
                            SQL = SQL + ComNum.VBLF + "                AND O.DRCODE IN ('1107','1125')";
                                }
                        else
                         {
                            SQL = SQL + ComNum.VBLF + "                AND O.DEPTCODE = '" + pAcp.medDeptCd + "'";
                            SQL = SQL + ComNum.VBLF + "                AND O.DRCODE NOT IN ('1107','1125')";
                        }
                    }
                    if (mViewNpChart == false) SQL = SQL + ComNum.VBLF + "        AND O.DEPTCODE <> 'NP'";

                    SQL = SQL + ComNum.VBLF + "          AND    O.GBSUNAP ='1' AND O.Seqno    > '0'   AND O.NAL      > '0'";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SlipNo     =  C.SlipNo(+)      ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.OrderCode  =  C.OrderCode(+)   ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DosCode    =  D.DosCode(+)     ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.DRCODE      =  N.SABUN(+)      ";
                    SQL = SQL + ComNum.VBLF + "          AND    O.SUCODE = S.SUNEXT(+) ";
                    SQL = SQL + ComNum.VBLF + "          ORDER BY O.BDATE DESC, O.GbSunap DESC, O.Slipno, O.Seqno";
                }
                else if ((pAcp.inOutCls == "O" && pAcp.medDeptCd == "ER") || (mstrFormNo == "2090"))
                {
                    SQL = "  SELECT O.BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames, O.REMARK,";
                    SQL = SQL + ComNum.VBLF + "  C.DispHeader, O.Qty, O.CONTENTS, O.GbGroup,";
                    SQL = SQL + ComNum.VBLF + "  O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE , '' RES";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_IORDER O,";
                    SQL = SQL + ComNum.VBLF + "       KOSMOS_OCS.OCS_ORDERCODE C,";
                    SQL = SQL + ComNum.VBLF + "       KOSMOS_OCS.OCS_ODOSAGE D,";
                    SQL = SQL + ComNum.VBLF + "       KOSMOS_OCS.OCS_DOCTOR N,";
                    SQL = SQL + ComNum.VBLF + "       KOSMOS_PMPA.BAS_SUN     S";
                    SQL = SQL + ComNum.VBLF + "  WHERE O.PTNO = '" + pAcp.ptNo + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND O.BDATE >= TO_DATE('" + dateTimePicker1.Value.ToShortDateString() +  "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND O.BDATE <= TO_DATE('" + dateTimePicker1.Value.AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "    AND O.GBSTATUS NOT IN ('D-','D','D+' )  ";
                    SQL = SQL + ComNum.VBLF + "    AND    O.SlipNo     =  C.SlipNo(+)      ";
                    SQL = SQL + ComNum.VBLF + "    AND    O.OrderCode  =  C.OrderCode(+)   ";
                    SQL = SQL + ComNum.VBLF + "    AND    O.DosCode    =  D.DosCode(+)     ";
                    SQL = SQL + ComNum.VBLF + "    AND    O.DRCODE      =  N.SABUN(+)      ";
                    SQL = SQL + ComNum.VBLF + "    AND    O.GBIOE IN ('E','EI') ";
                    SQL = SQL + ComNum.VBLF + "    AND (NOT (O.GBVERB = 'Y' AND O.VERBC IS NULL)  OR O.GBVERB IS NULL)";
                    SQL = SQL + ComNum.VBLF + "    AND    O.SUCODE = S.SUNEXT(+) ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY O.BDATE DESC, O.Slipno, O.Seqno";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;                    
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {                    
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        if (dt.Rows[i]["BUN"].ToString().Trim() == "" || dt.Rows[i]["BUN"].ToString().Trim() == "10")
                        {
                            ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        }
                        else
                        {
                            if (dt.Rows[i]["ORDERNAMES"].ToString().Trim() != "")
                            {
                                strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                ssView_Sheet1.Cells[i, 2].Text = strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                            }
                            else if (dt.Rows[i]["DISPHEADER"].ToString().Trim() != "")
                            {
                                ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DISPHEADER"].ToString().Trim() + " " + dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            }
                            else
                            {
                                ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            }
                        }

                        if (dt.Rows[i]["RES"].ToString().Trim() == "1")
                        {
                            ssView_Sheet1.Cells[i, 2].Text = "(예약)" + ssView_Sheet1.Cells[i, 2].Text;
                        }
                        ssView_Sheet1.Cells[i, 3].Text = (VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim()) == 0 ? "" : dt.Rows[i]["CONTENTS"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["RealQty"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DosName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GbGroup"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        //'외래는 의사코드로 움직이는데 불명확하다..??????
                        if (pAcp.inOutCls == "I")
                        {
                            ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DrName"].ToString().Trim();                            
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 9].Text = clsVbfunc.GetOCSDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());
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
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void mbtnSearch_Click(object sender, EventArgs e)
        {
            GetOrder();
        }

        private void mbtnPrint_Click(object sender, EventArgs e)
        {
            int rtnVal = 0;

            rtnVal = clsSpreadPrint.PrintSpdFormCnt(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dateTimePicker1.Value.ToString("yyyyMMdd"), dateTimePicker2.Value.ToString("yyyyMMdd"), ssView, "P", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Portrait, clsPublic.GstrSysDate);
        }
    }
}
