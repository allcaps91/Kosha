using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using FarPoint.Win;
using FarPoint.Win.Spread.CellType;
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    /// <summary>
    /// \mtsEmr\FrmPatientVital.frm
    /// FrmPatientState
    /// 환자상태 Vital 조회
    /// 19-07-23
    /// </summary>
    public partial class frmPatientState_New : Form
    {
        private class GV
        {
            public string Code = string.Empty;
            public string Y = string.Empty;
            public double X = 0;
        }

        //public delegate void CloseEvent();
        //public event CloseEvent rClosed;

        EmrPatient pAcp;

        public frmPatientState_New()
        {
            InitializeComponent();
        }

        public frmPatientState_New(EmrPatient emr)
        {
            pAcp = emr;
            InitializeComponent();
        }

        FarPoint.Win.ComplexBorder border = null;

        private void FrmPatientState_Load(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = true;
            WindowState = FormWindowState.Maximized;
            dtpGraphFDate.Value = dtpGraphTDate.Value.AddDays(-1);
            dtpGraphTDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

            cboDate.Items.Clear();
            cboDate.Items.Add("1주 이내");
            cboDate.Items.Add("2주 이내");
            cboDate.Items.Add("3주 이내");
            cboDate.Items.Add("4주 이내");
            cboDate.Items.Add("5주 이내");
            cboDate.SelectedIndex = 1;

            border = new ComplexBorder(null, null, new ComplexBorderSide(Color.Black), new ComplexBorderSide(Color.Black), new ComplexBorderSide(Color.Black), false, false);

            SSVITAL_Sheet1.DefaultStyle.Border = border;
            SSVITAL_Sheet1.SheetCornerStyle.Border = border;
            SSVITAL_Sheet1.ColumnHeader.DefaultStyle.Border = border;
            SSVITAL_Sheet1.RowHeader.DefaultStyle.Border = border;

            SSBST_Sheet1.DefaultStyle.Border = border;
            SSBST_Sheet1.SheetCornerStyle.Border = border;
            SSBST_Sheet1.ColumnHeader.DefaultStyle.Border = border;
            SSBST_Sheet1.RowHeader.DefaultStyle.Border = border;

            SSIO_Sheet1.DefaultStyle.Border = border;
            SSIO_Sheet1.SheetCornerStyle.Border = border;
            SSIO_Sheet1.ColumnHeader.DefaultStyle.Border = border;
            SSIO_Sheet1.RowHeader.DefaultStyle.Border = border;

            DataTable dt = null;
            if(GetBMedDept(ref dt))
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["MEDDEPTCD"].ToString().Trim());
                }
                cboDept.Items.Add("ICU");
                cboDept.SelectedIndex = 0;
            }

            //컨설트로 들어왔으면
            if (pAcp != null)
            {
                cboDept.Text = pAcp.medDeptCd;
                SetDoctor(cboDept.Text.Trim());

                cboDr.Text = clsVbfunc.GetOCSDrCodeDrName(clsDB.DbCon, pAcp.medDrCd) + VB.Space(50) + pAcp.medDrCd;
            }
            else
            {
                cboDept.Text = clsType.User.DeptCode.Length > 0 ? clsType.User.DeptCode : "MP";
                SetDoctor(cboDept.Text.Trim());

                cboDr.Text = clsType.User.DrCode.Length > 0 ? clsType.User.UserName + VB.Space(50) + clsType.User.DrCode : cboDr.Text.Trim();
            }

            Read_Patient_List();

            if(pAcp != null)
            {
                int intRow = -1;
                int intCol = -1;
                SSList.Search(0, pAcp.ptNo, true, true, true, true, 0, 0, ref intRow, ref intCol);
                if (intRow != -1)
                {
                    CellClick(intRow);
                }
            }

        }

        /// <summary>
        /// 지표 스프레드 및 라벨 클리어
        /// </summary>
        void Clear()
        {
            ssIndicator_Sheet1.Cells[1, 1].Text = "";
            ssIndicator_Sheet1.Cells[2, 1].Text = "";
            ssIndicator_Sheet1.Cells[3, 1].Text = "";
            ssIndicator_Sheet1.Cells[4, 1].Text = "";

            ssIndicator_Sheet1.Cells[1, 3].Text = "";
            ssIndicator_Sheet1.Cells[2, 3].Text = "";
            ssIndicator_Sheet1.Cells[3, 3].Text = "";
            ssIndicator_Sheet1.Cells[4, 3].Text = "";

            ssIndicator_Sheet1.Cells[1, 5].Text = "";
            ssIndicator_Sheet1.Cells[2, 5].Text = "";
            ssIndicator_Sheet1.Cells[3, 5].Text = "";
            ssIndicator_Sheet1.Cells[4, 5].Text = "";

            lblSubTitle.Text = "";
        }


        /// <summary>
        /// 진료과(BMEDDEPT)에서 결과 가지고 오기
        /// </summary>
        /// <param name="dataTable">넘겨받을 DataTable</param>
        /// <param name="strMedDeptCd">과코드</param>
        /// <param name="strDeptKorName">과 한글이름</param>
        /// <returns></returns>
        bool GetBMedDept(ref DataTable dataTable, string strMedDeptCd = "", string strDeptKorName = "")
        {
            bool rtnVal = false;
            string sqlErr = string.Empty;
            string SQL = string.Empty;
            DataTable dt = null;

            try
            {
                SQL = " SELECT MEDDEPTCD, DEPTKORNAME";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.VIEWBMEDDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE OUTMEDACPYN =  '1' ";

                if(strMedDeptCd.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + " AND MEDDEPTCD =  '" + strMedDeptCd + "'";
                }

                if(strDeptKorName.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + " AND DEPTKORNAME =  '" + strMedDeptCd + "'";
                }

                SQL = SQL + ComNum.VBLF + "  ORDER BY PRTGRD ";

                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0 || dt == null)
                    return rtnVal;

                dataTable = dt;
                rtnVal = true;

                dt.Dispose();
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Read_Patient_List();
        }

        /// <summary>
        /// 조회 함수
        /// </summary>
        void Read_Patient_List()
        {
            string sqlErr = string.Empty;
            string SQL = string.Empty;
            DataTable dt = null;

            SSList_Sheet1.RowCount = 0;

            try
            {
                SQL = " SELECT ROOMCODE, PANO, SNAME, AGE, SEX, DEPTCODE,";
                SQL = SQL + ComNum.VBLF + " (SELECT DRNAME FROM KOSMOS_PMPA.BAS_DOCTOR WHERE DRCODE = A.DRCODE) DRNAME, ";
                SQL = SQL + ComNum.VBLF + "  DRCODE, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE, IPDNO, WARDCODE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER A";
                SQL = SQL + ComNum.VBLF + "  WHERE ((JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') AND GBSTS <> '7') OR (JDATE = TRUNC(SYSDATE) AND GBSTS = '7') OR OUTDATE = TRUNC(SYSDATE))";
                if (cboDept.Text.Equals("ICU"))
                {
                    SQL = SQL + ComNum.VBLF + "    AND WARDCODE IN ('33', '35')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND DEPTCODE = '" + cboDept.Text.Trim() + "'";
                }

                if (VB.Right(cboDr.Text, 10).Trim() != "0")
                {
                    SQL = SQL + ComNum.VBLF + "    AND DRCODE = '" + VB.Right(cboDr.Text, 10).Trim() + "' ";
                }

                if (rdoIpdSort1.Checked == true)
                {
                    SQL += "  ORDER BY SNAME                                                                                                            \r";
                }
                else if (rdoIpdSort2.Checked == true)
                {
                    SQL += "  ORDER BY a.RoomCode, a.BedNum, SNAME                                                                                      \r";
                }
                else if (rdoIpdSort3.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(A.WARDCODE, '33', 1, '35', 2,  '4H', 3, '40', 4, '50', 5, '60', 6, '70', 7, '80', 8, '53', 9, '55', 10, '63', 11, '65', 12, '73', 13, '75', 14, '83', 15, 16),  A.RoomCode, A.BEDNUM ASC,  A.DrCode, SName ";
                }

                //sQL = SQL + ComNum.VBLF + "ORDER BY DECODE(WARDCODE, '33', 1, '35', 2,  '4H', 3, '40', 4, '50', 5, '60', 6, '70', 7, '80', 8, '53', 9, '55', 10, '63', 11, '65', 12, '73', 13, '75', 14, '83', 15, 16), A.RoomCode, A.SName ";
                
                //SQL = SQL + ComNum.VBLF + "  ORDER BY ROOMCODE, SNAME";

                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    SSList_Sheet1.RowCount = dt.Rows.Count;
                    SSList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        SSList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 3].Text = string.Format("{0}/{1}",
                            dt.Rows[i]["AGE"].ToString().Trim(),   dt.Rows[i]["SEX"].ToString().Trim());
                        SSList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();

                    }
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 해당 과 의사 리스트 콤바박스 설정
        /// </summary>
        void SetDoctor(string strDeptCd)
        {
            string sqlErr = string.Empty;
            string SQL = string.Empty;
            DataTable dt = null;

            SSList_Sheet1.RowCount = 0;

            try
            {
                SQL = "SELECT DRNAME,DRCODE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_DOCTOR  ";
                SQL = SQL + ComNum.VBLF + "   WHERE GBOUT = 'N' ";

                if (strDeptCd.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + "    AND DEPTCODE = '" + strDeptCd.Trim() + "'";
                }

                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                cboDr.Items.Clear();
                cboDr.Items.Add("전  체" + VB.Space(50) + "0");

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDr.Items.Add(dt.Rows[i]["DRNAME"].ToString().Trim() + VB.Space(50) + dt.Rows[i]["DRCODE"].ToString().Trim());
                    }
                }

                cboDr.SelectedIndex = 0;

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return;
        }

        private void CboDate_DropDownClosed(object sender, EventArgs e)
        {
            if(VB.Val(VB.Left(cboDate.Text.Trim(), 1)) > 3)
            {
                ComFunc.MsgBoxEx(this, "3주 이상일 경우 조회 시간이 오래 걸릴 수 있습니다.");
                return;
            }
        }

        private void SSList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SSList_Sheet1.RowCount == 0)
                return;

            CellClick(e.Row);
        }

        /// <summary>
        /// 셀 클릭 함수
        /// </summary>
        /// <param name="Row"></param>
        void CellClick(int Row)
        {
            Clear();

            string strROOMCODE = SSList_Sheet1.Cells[Row, 0].Text.Trim();
            string strPtno     = SSList_Sheet1.Cells[Row, 1].Text.Trim();
            string strSNAME    = SSList_Sheet1.Cells[Row, 2].Text.Trim();
            string strDeptcd   = SSList_Sheet1.Cells[Row, 4].Text.Trim();
            string strInDate   = SSList_Sheet1.Cells[Row, 7].Text.Trim();
            string strIPDNO    = SSList_Sheet1.Cells[Row, 8].Text.Trim();
            string strAge      = SSList_Sheet1.Cells[Row, 9].Text.Trim();
            string strWARDCODE = SSList_Sheet1.Cells[Row, 10].Text.Trim();

            pAcp = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtno, "I", strInDate.Replace("-", ""), strDeptcd);

            lblSubTitle.Text = "병동 : " + strWARDCODE + "병동     병실 : " + strROOMCODE + "호     환자명/등록번호 : " + strSNAME + "/" + strPtno + "     입원일자 : " + strInDate;
            READ_DATA(strPtno, pAcp.medFrDate, strIPDNO, strAge, strWARDCODE);

            if (splitContainer1.Panel1.Visible)
            {
                GetVitalGraph();
            }
        }

        void READ_DATA(string argPTNO, string ArgInDate, string argIPDNO,string argAge,string argWARD)
        {
            string strWeek = VB.Left(cboDate.Text.Trim(), 1);

            string strEDATE = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string strSDATE = strWeek.Length == 0 ? strEDATE : Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays((VB.Val(strWeek) * 7) * -1).ToString("yyyyMMdd");
            string GstrSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");

            READ_VITAL(argPTNO, strSDATE, strEDATE, ArgInDate);        
            READ_IO(argPTNO, strSDATE, strEDATE, ArgInDate);
            READ_BST(argPTNO, strSDATE, strEDATE, ArgInDate);
            
            READ_DETAIL_FALL(argPTNO, GstrSysDate, argIPDNO, argAge);
            READ_DETAIL_BRADEN(argPTNO, GstrSysDate, argIPDNO, argAge, argWARD);
            READ_DETAIL_PAIN(argIPDNO, argPTNO);
            READ_간호문제(argPTNO, ArgInDate);
        }

        /// <summary>
        /// 바이탈 조회 함수
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        void READ_VITAL(string argPTNO, string argSDATE, string argEDATE, string argMedFrDate)
        {
            string SQL = string.Empty;
            DataTable dt = null;

            EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "3150");
            SSVITAL_Sheet1.ColumnCount = 1;

            try
            {
                #region XML
                SQL = " SELECT A.CHARTDATE, A.CHARTTIME,";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it3') AS IT3,";     //'혈압(Sys)
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it4') AS IT4,";     //'혈압(Dia)
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it6') AS IT6,";     //'맥박
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it7') AS IT7,";     //'호흡
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it8') AS IT8,";     //'체온
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it10') AS IT10,";   //'BWT
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it12') AS IT12,";    //'배둘레(19-10-12 추가)
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it14') AS IT14,";   //'SpO2
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//it274') AS IT274"; //'비고
                SQL = SQL + ComNum.VBLF + "    From KOSMOS_EMR.EMRXML A, KOSMOS_EMR.EMRXMLMST B";
                SQL = SQL + ComNum.VBLF + "   WHERE B.PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "     AND B.CHARTDATE >= '" + argSDATE + "' ";
                SQL = SQL + ComNum.VBLF + "     AND B.CHARTDATE <= '" + argEDATE + "' ";
                SQL = SQL + ComNum.VBLF + "     AND B.FORMNO = 1562";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNO = B.EMRNO ";
                #endregion

                if (pForm.FmOLDGB != 1)
                {
                    #region 신규
                    SQL = SQL + ComNum.VBLF + "     UNION ALL";
                    SQL = SQL + ComNum.VBLF + "     SELECT ";
                    SQL = SQL + ComNum.VBLF + "            TRIM(A.CHARTDATE) CHARTDATE, TRIM(A.CHARTTIME) CHARTTIME, ";
                    SQL = SQL + ComNum.VBLF + "            B1.ITEMVALUE AS IT3,   B2.ITEMVALUE AS IT4,   B4.ITEMVALUE AS IT6, ";
                    SQL = SQL + ComNum.VBLF + "            B10.ITEMVALUE AS IT7,  B5.ITEMVALUE AS IT8,   B8.ITEMVALUE AS IT10,  ";
                    SQL = SQL + ComNum.VBLF + "            B7.ITEMVALUE AS IT12,  B6.ITEMVALUE AS IT14,  B9.ITEMVALUE AS IT274  ";
                    SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_EMR.AEMRCHARTMST A";
                    SQL = SQL + ComNum.VBLF + "       INNER JOIN KOSMOS_EMR.AEMRFORM F";
                    SQL = SQL + ComNum.VBLF + "          ON A.FORMNO = F.FORMNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.UPDATENO = F.UPDATENO";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B1";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B1.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B1.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B1.ITEMCD IN ('I0000002018') --SBP";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B2";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B2.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B2.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B2.ITEMCD IN ('I0000001765') --DBP";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B4";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B4.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B4.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B4.ITEMCD IN ('I0000014815') --PR";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B5";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B5.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B5.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B5.ITEMCD IN ('I0000001811') --BT(℃)";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B6";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B6.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B6.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B6.ITEMCD IN ('I0000008708') --SpO2 (%)";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B7";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B7.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B7.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B7.ITEMCD IN ('I0000018853') --배둘레(복위)";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B8";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B8.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B8.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B8.ITEMCD IN ('I0000000418') --체중(BWT)";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B9";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B9.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B9.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B9.ITEMCD IN ('I0000037854') --비고";
                    SQL = SQL + ComNum.VBLF + "       LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B10";
                    SQL = SQL + ComNum.VBLF + "          ON A.EMRNO = B10.EMRNO";
                    SQL = SQL + ComNum.VBLF + "         AND A.EMRNOHIS = B10.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "         AND B10.ITEMCD IN ('I0000002009') --RR";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.PTNO = '" + argPTNO + "'";
                    SQL = SQL + ComNum.VBLF + "       AND A.MEDFRDATE = '" + argMedFrDate +"'";
                    SQL = SQL + ComNum.VBLF + "       AND A.CHARTDATE >= '" + argSDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND A.CHARTDATE <= '" + argEDATE + "'";
                    SQL = SQL + ComNum.VBLF + "       AND A.FORMNO IN(1562, 2135, 3150)";
                    SQL = SQL + ComNum.VBLF + "       AND (B1.ITEMVALUE > CHR(0) OR";
                    SQL = SQL + ComNum.VBLF + "            B2.ITEMVALUE > CHR(0) OR";
                    SQL = SQL + ComNum.VBLF + "            B4.ITEMVALUE > CHR(0) OR";
                    SQL = SQL + ComNum.VBLF + "            B5.ITEMVALUE > CHR(0) OR";
                    SQL = SQL + ComNum.VBLF + "            B6.ITEMVALUE > CHR(0) OR";
                    SQL = SQL + ComNum.VBLF + "            B7.ITEMVALUE > CHR(0) OR";
                    SQL = SQL + ComNum.VBLF + "            B8.ITEMVALUE > CHR(0) OR";
                    SQL = SQL + ComNum.VBLF + "            B9.ITEMVALUE > CHR(0))";
                    #endregion
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY CHARTDATE DESC, CHARTTIME DESC ";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }


                if (dt.Rows.Count > 0)
                {
                    TextCellType textCellType = new TextCellType();
                    textCellType.Multiline = true;
                    textCellType.WordWrap = true;

                    SSVITAL_Sheet1.ColumnCount = dt.Rows.Count + 1;
                    //SSVITAL_Sheet1.Columns[1, SSVITAL_Sheet1.ColumnCount - 1].Border = border;
                    SSVITAL_Sheet1.Columns[1, SSVITAL_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    SSVITAL_Sheet1.Columns[1, SSVITAL_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    SSVITAL_Sheet1.Columns[1, SSVITAL_Sheet1.ColumnCount - 1].CellType = textCellType;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SSVITAL_Sheet1.Cells[0, i + 1].Text = VB.Right(VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000/00/00"), 5)  + ComNum.VBLF +       
                                                              VB.Val(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(),4 )).ToString("00:00");

                        SSVITAL_Sheet1.Cells[0, i + 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        SSVITAL_Sheet1.Cells[0, i + 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                        SSVITAL_Sheet1.Cells[2, i + 1].Text = dt.Rows[i]["IT3"].ToString().Trim();
                        SSVITAL_Sheet1.Cells[3, i + 1].Text = dt.Rows[i]["IT4"].ToString().Trim();
                        SSVITAL_Sheet1.Cells[4, i + 1].Text = dt.Rows[i]["IT6"].ToString().Trim();
                        SSVITAL_Sheet1.Cells[5, i + 1].Text = dt.Rows[i]["IT7"].ToString().Trim();
                        SSVITAL_Sheet1.Cells[6, i + 1].Text = dt.Rows[i]["IT8"].ToString().Trim();
                        SSVITAL_Sheet1.Cells[7, i + 1].Text = dt.Rows[i]["IT14"].ToString().Trim();
                        SSVITAL_Sheet1.Cells[8, i + 1].Text = dt.Rows[i]["IT10"].ToString().Trim();

                        SSVITAL_Sheet1.Cells[9, i + 1].Text = dt.Rows[i]["IT12"].ToString().Trim();
                        SSVITAL_Sheet1.Cells[10, i + 1].Text = dt.Rows[i]["IT274"].ToString().Trim();

                        double rtnVal = VB.Val(dt.Rows[i]["IT3"].ToString().Trim());
                        if (rtnVal >= 140 || rtnVal < 80)  //혈압 it3
                        {
                            SSVITAL_Sheet1.Cells[2, i + 1].ForeColor = Color.Red;
                        }

                        rtnVal = VB.Val(dt.Rows[i]["IT4"].ToString().Trim());
                        if (rtnVal >= 90 || rtnVal < 60) //혈압 it4
                        {
                            SSVITAL_Sheet1.Cells[3, i + 1].ForeColor = Color.Red;
                        }

                        rtnVal = VB.Val(dt.Rows[i]["IT6"].ToString().Trim());
                        if (rtnVal >= 100 || rtnVal < 60) //맥박 it6
                        {
                            SSVITAL_Sheet1.Cells[4, i + 1].ForeColor = Color.Red;
                        }


                        rtnVal = VB.Val(dt.Rows[i]["IT7"].ToString().Trim());
                        if (rtnVal >= 21 || rtnVal < 12) //호흡 it7
                        {
                            SSVITAL_Sheet1.Cells[5, i + 1].ForeColor = Color.Red;
                        }

                        rtnVal = VB.Val(dt.Rows[i]["IT8"].ToString().Trim());
                        if (rtnVal >= 37.5 || rtnVal < 36.5) //체온 it8
                        {
                            SSVITAL_Sheet1.Cells[6, i + 1].ForeColor = Color.Red;
                        }

                        SSVITAL_Sheet1.Rows[10].Height = SSVITAL_Sheet1.Rows[10].GetPreferredHeight() + 16;

                    }

                    SSVITAL_Sheet1.Columns[1, SSVITAL_Sheet1.ColumnCount - 1].Width = 80;

                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return;
        }

        /// <summary>
        /// IO 조회 함수
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        void READ_IO(string argPTNO, string argSDATE, string argEDATE, string argMedFrDate)
        {
            string SQL = string.Empty;
            DataTable dt = null;
            SSIO_Sheet1.ColumnCount = 2;
            int nCol = 0;

            string SqlErr = string.Empty;

            try
            {
                #region 쿼리
                SQL = SQL + ComNum.VBLF + "WITH IO_LIST AS 																													";
                SQL = SQL + ComNum.VBLF + "(                                                                                                                                ";
                SQL = SQL + ComNum.VBLF + "SELECT  B.VFLAG3 AS   MAINITEM                                                                                                   ";
                SQL = SQL + ComNum.VBLF + "	   , b1.remark1 AS SUBITEM                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "       , R.ITEMVALUE AS VAL                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "       ,CASE WHEN TRIM(SUBSTR(CHARTTIME, 0, 4)) = '2400' THEN TO_DATE(CHARTDATE, 'YYYYMMDD') - 1                                 ";
                SQL = SQL + ComNum.VBLF + "		      WHEN SUBSTR(CHARTTIME, 0, 4) >= '0000' AND SUBSTR(CHARTTIME, 0, 4) <= '0500' THEN TO_DATE(CHARTDATE, 'YYYYMMDD') - 1  ";
                SQL = SQL + ComNum.VBLF + "        ELSE TO_DATE(CHARTDATE, 'YYYYMMDD') END CDATE                                                                            ";
                SQL = SQL + ComNum.VBLF + "       ,CASE WHEN TRIM(SUBSTR(CHARTTIME, 0, 4)) >= '0501' AND TRIM(SUBSTR(CHARTTIME, 0, 4)) <= '1300' THEN 'DAY' ";
                SQL = SQL + ComNum.VBLF + "             WHEN TRIM(SUBSTR(CHARTTIME, 0, 4)) >= '1301' AND TRIM(SUBSTR(CHARTTIME, 0, 4)) <= '2100' THEN 'EVE' ";
                SQL = SQL + ComNum.VBLF + "             WHEN TRIM(SUBSTR(CHARTTIME, 0, 4)) >= '2101' AND TRIM(SUBSTR(CHARTTIME, 0, 4)) <= '2400' THEN 'NIG' ";
                SQL = SQL + ComNum.VBLF + "             WHEN TRIM(SUBSTR(CHARTTIME, 0, 4)) >= '0000' AND TRIM(SUBSTR(CHARTTIME, 0, 4)) <= '0500' THEN 'NIG' ";
                SQL = SQL + ComNum.VBLF + "        END DUTY                                                                                                 ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTMST A      ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRBASCD B1 ";
                SQL = SQL + ComNum.VBLF + "        ON B1.BSNSCLS = '기록지관리'     ";
                SQL = SQL + ComNum.VBLF + "       AND B1.UNITCLS = '섭취배설그룹'    ";
                SQL = SQL + ComNum.VBLF + "       AND B1.BASCD  > CHR(0)         ";
                SQL = SQL + ComNum.VBLF + "       AND B1.APLFRDATE > CHR(0)      ";
                SQL = SQL + ComNum.VBLF + "       AND B1.REMARK1  IN('경구', '배액관', 'Stool', '혈액', '수액', 'Urine', '구토', 'CRRT')      ";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN KOSMOS_EMR.AEMRBASCD B                                         ";
                SQL = SQL + ComNum.VBLF + "        ON B.BSNSCLS = '기록지관리'          ";
                SQL = SQL + ComNum.VBLF + "       AND B.UNITCLS = '섭취배설'             ";
                SQL = SQL + ComNum.VBLF + "       AND B.BASCD  > CHR(0)              ";
                SQL = SQL + ComNum.VBLF + "       AND B.APLFRDATE > CHR(0)           ";
                SQL = SQL + ComNum.VBLF + "       AND B.VFLAG1 = B1.BASCD            ";
                SQL = SQL + ComNum.VBLF + "       AND B.VFLAG3 IN('11.배설', '01.섭취')  ";
                SQL = SQL + ComNum.VBLF + "     INNER JOIN KOSMOS_EMR.AEMRCHARTROW R ";
                SQL = SQL + ComNum.VBLF + "        ON A.EMRNO  = R.EMRNO             ";
                SQL = SQL + ComNum.VBLF + "       AND A.EMRNOHIS = R.EMRNOHIS        ";
                SQL = SQL + ComNum.VBLF + "       AND R.ITEMCD  = B.BASCD            ";
                SQL = SQL + ComNum.VBLF + "       AND R.ITEMCD NOT IN ('I0000022324', 'I0000030622', 'I0000030623') -- 기존 수혈 삭제      ";
                SQL = SQL + ComNum.VBLF + @"       AND REGEXP_LIKE(R.ITEMVALUE,'^-?\d*\.?\d+([eE]-?\d+)?$')              ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO  + "'";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO IN(3150, 2135)";
                SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE  = '" + argMedFrDate +"'";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE >= '" + argSDATE + "'";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= '" + argEDATE + "'";
                SQL = SQL + ComNum.VBLF + "   AND CHARTUSEID <> '합계'";
                #region 혈액(수혈) 기록지 데이터 가져오기
                SQL = SQL + ComNum.VBLF + " UNION ALL --수혈 기록지에서 값 가저오기                                                                                                                                                                            ";
                SQL = SQL + ComNum.VBLF + "    SELECT '01.섭취', '혈액', ITEMVALUE";
                SQL = SQL + ComNum.VBLF + "       ,CASE WHEN TRIM(SUBSTR(CHARTTIME, 0, 4)) = '2400' THEN TO_DATE(CHARTDATE, 'YYYYMMDD') - 1                                 ";
                SQL = SQL + ComNum.VBLF + "		      WHEN SUBSTR(CHARTTIME, 0, 4) >= '0000' AND SUBSTR(CHARTTIME, 0, 4) <= '0500' THEN TO_DATE(CHARTDATE, 'YYYYMMDD') - 1  ";
                SQL = SQL + ComNum.VBLF + "        ELSE TO_DATE(CHARTDATE, 'YYYYMMDD') END CDATE                                                                            ";
                SQL = SQL + ComNum.VBLF + "       ,CASE WHEN TRIM(SUBSTR(CHARTTIME, 0, 4)) >= '0501' AND TRIM(SUBSTR(CHARTTIME, 0, 4)) <= '1300' THEN 'DAY' ";
                SQL = SQL + ComNum.VBLF + "             WHEN TRIM(SUBSTR(CHARTTIME, 0, 4)) >= '1301' AND TRIM(SUBSTR(CHARTTIME, 0, 4)) <= '2100' THEN 'EVE' ";
                SQL = SQL + ComNum.VBLF + "             WHEN TRIM(SUBSTR(CHARTTIME, 0, 4)) >= '2101' AND TRIM(SUBSTR(CHARTTIME, 0, 4)) <= '2400' THEN 'NIG' ";
                SQL = SQL + ComNum.VBLF + "             WHEN TRIM(SUBSTR(CHARTTIME, 0, 4)) >= '0000' AND TRIM(SUBSTR(CHARTTIME, 0, 4)) <= '0500' THEN 'NIG' ";
                SQL = SQL + ComNum.VBLF + "        END DUTY  ";
                SQL = SQL + ComNum.VBLF + "    FROM                                                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "    (                                                                                                                                                                                                           ";
                SQL = SQL + ComNum.VBLF + "        SELECT A.EMRNO                                                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "             , A.EMRNOHIS                                                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "             , (REPLACE(REPLACE((SELECT ITEMVALUE                                                                                                                                                               ";
                SQL = SQL + ComNum.VBLF + "                                  FROM KOSMOS_EMR.AEMRCHARTROW                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "                                 WHERE EMRNO = A.EMRNO                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "                                   AND EMRNOHIS = A.EMRNOHIS                                                        ";
                SQL = SQL + ComNum.VBLF + "                                   AND ITEMCD = 'I0000037490'),'-',''),'/','')) AS CHARTDATE--수혈종료일자                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "             , REPLACE((SELECT ITEMVALUE                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "                          FROM KOSMOS_EMR.AEMRCHARTROW                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "                         WHERE EMRNO = A.EMRNO                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "                           AND EMRNOHIS = A.EMRNOHIS                                                        ";
                SQL = SQL + ComNum.VBLF + "                           AND ITEMCD = 'I0000037491'),':','' || '00')AS CHARTTIME  --수혈종료시간                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "             , 'I0000022324' AS ITEMCD                                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "             , 'I0000022324' AS ITEMNO                                                                                                                                                                          ";
                SQL = SQL + ComNum.VBLF + "             , B.ITEMVALUE                                                                                                                                                                                       ";
                SQL = SQL + ComNum.VBLF + "          FROM KOSMOS_EMR.AEMRCHARTMST A                                                                                                                                                                        ";
                SQL = SQL + ComNum.VBLF + "         INNER JOIN  KOSMOS_EMR.AEMRCHARTROW B                                                                                                                                                                  ";
                SQL = SQL + ComNum.VBLF + "            ON A.EMRNO = B.EMRNO                                                                                                                                                                                ";
                SQL = SQL + ComNum.VBLF + "           AND A.EMRNOHIS = B.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "         WHERE A.PTNO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + "           AND A.FORMNO IN (1965, 3535)                                                                                                                                                                      ";
                SQL = SQL + ComNum.VBLF + "           AND B.ITEMCD = 'I0000013528'                                                                                                                                                                         ";
                SQL = SQL + ComNum.VBLF + "           AND A.MEDFRDATE = '" + argMedFrDate +"'";
                SQL = SQL + ComNum.VBLF + "           AND CHARTDATE >= '" + argSDATE + "' ";
                SQL = SQL + ComNum.VBLF + "    )                                                                                                                                                                                                           ";
                SQL = SQL + ComNum.VBLF + " WHERE CHARTDATE >= '" + argSDATE + "' ";
                SQL = SQL + ComNum.VBLF + "   AND CHARTDATE <= '" + argEDATE + "' ";
                //SQL = SQL + ComNum.VBLF + " GROUP BY ITEMCD ";
                #endregion
                SQL = SQL + ComNum.VBLF + ")";
                SQL = SQL + ComNum.VBLF + "SELECT     MAINITEM";
                SQL = SQL + ComNum.VBLF + " 		, CASE WHEN MAINITEM = '01.섭취' AND GROUPING_ID(MAINITEM, CDATE, DUTY, SUBITEM)  = 3 THEN '총섭취량'";
                SQL = SQL + ComNum.VBLF + " 		       WHEN MAINITEM = '11.배설' AND GROUPING_ID(MAINITEM, CDATE, DUTY, SUBITEM)  = 3 THEN '총배설량'";
                SQL = SQL + ComNum.VBLF + " 		       WHEN GROUPING_ID(MAINITEM, CDATE, SUBITEM) = 1 THEN '소계'";
                SQL = SQL + ComNum.VBLF + " 		       ELSE SUBITEM";
                SQL = SQL + ComNum.VBLF + " 		  END SUBITEM";
                SQL = SQL + ComNum.VBLF + " 		, CASE WHEN DUTY IS NULL THEN 'TOT' ELSE DUTY END DUTY";
                SQL = SQL + ComNum.VBLF + " 		, CDATE";
                SQL = SQL + ComNum.VBLF + " 		, SUM(VAL) TOTAL";
                SQL = SQL + ComNum.VBLF + "  FROM IO_LIST";
                SQL = SQL + ComNum.VBLF + " GROUP BY CDATE, MAINITEM, CUBE(DUTY, SUBITEM)";
                SQL = SQL + ComNum.VBLF + " ORDER BY CDATE DESC, DUTY, SUBITEM";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                #endregion

                if (dt.Rows.Count > 0)
                {
                    int nRow = -1;

                    Dictionary<string, int> keyChartDateDuty = new Dictionary<string, int>();
                    Font font = new Font("나눔고딕", 10, FontStyle.Bold);
                    string strDate = dt.Rows[0]["CDATE"].ToString().Trim();
                    ComplexBorder border2 = new FarPoint.Win.ComplexBorder(null, null, new FarPoint.Win.ComplexBorderSide(Color.Black, 3), new FarPoint.Win.ComplexBorderSide(Color.Black), null, false, false);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DateTime CDATE = dt.Rows[i]["CDATE"].To<DateTime>();
                        string DUTY = dt.Rows[i]["DUTY"].ToString();

                        if (keyChartDateDuty.TryGetValue(CDATE.ToString("yyyyMMdd") + DUTY, out nCol) == false)
                        {
                            SSIO_Sheet1.ColumnCount += 1;
                            nCol = SSIO_Sheet1.ColumnCount - 1;

                            keyChartDateDuty.Add(CDATE.ToString("yyyyMMdd") + DUTY, nCol);
                        }

                        SSIO_Sheet1.Cells[0, nCol].Text = CDATE.ToString("MM") + "/" + CDATE.ToString("dd");
                        SSIO_Sheet1.Cells[0, nCol].Text += ComNum.VBLF + dt.Rows[i]["DUTY"].ToString().Trim();
                        SSIO_Sheet1.Cells[0, nCol].BackColor = SSIO_Sheet1.Cells[0, 0].BackColor;

                        switch (dt.Rows[i]["SUBITEM"].ToString().Trim())
                        {
                            case "경구":
                                nRow = 2;
                                break;
                            case "수액":
                                nRow = 3;
                                break;
                            case "혈액":
                                nRow = 4;
                                break;
                            case "총섭취량":
                                nRow = 5;
                                break;
                            case "Urine":
                                nRow = 6;
                                break;
                            case "Stool":
                                nRow = 7;
                                break;
                            case "배액관":
                                nRow = 8;
                                break;
                            case "구토":
                                nRow = 10;
                                break;
                            case "CRRT":
                                nRow = 11;
                                break;
                            case "총배설량":
                                nRow = 12;
                                break;
                            case "소계":
                                nRow = dt.Rows[i]["MAINITEM"].ToString().Trim().Equals("01.섭취") ?  5  : 12;
                                break;
                        }

                        if (DUTY.Equals("TOT") && (nRow == 5 || nRow == 12))
                        {
                            SSIO_Sheet1.Cells[0, nCol].Text = DUTY;
                            SSIO_Sheet1.Cells[nRow, nCol].Font = font;
                        }

                        SSIO_Sheet1.Cells[nRow, nCol].Text = dt.Rows[i]["TOTAL"].ToString().Trim();


                        if (strDate.Equals(dt.Rows[i]["CDATE"].ToString().Trim()) == false)
                        {
                            strDate = dt.Rows[i]["CDATE"].ToString().Trim();
                            SSIO_Sheet1.Columns[nCol - 1].Border = border2;
                        }
                    }

                    if (SSIO_Sheet1.ColumnCount > 2)
                    {
                        //SSIO_Sheet1.Columns[2, SSIO_Sheet1.ColumnCount - 1].Border = border;
                        SSIO_Sheet1.Columns[2, SSIO_Sheet1.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        SSIO_Sheet1.Columns[2, SSIO_Sheet1.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                        SSIO_Sheet1.Columns[SSIO_Sheet1.ColumnCount - 1].Border = border2;
                    }
                }

                dt.Dispose();
                dt = null;

                //SSIO_Sheet1.Cells[0, nCol].Text = "총량";

                //SSIO_Sheet1.AddSpanCell(2, nCol, 3, 1);
                //SSIO_Sheet1.Cells[2, nCol].Text = intIn.ToString();

                //SSIO_Sheet1.AddSpanCell(5, nCol, 5, 1);
                //SSIO_Sheet1.Cells[5, nCol].Text = intOut.ToString();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return;
        }

        /// <summary>
        /// 혈당 조회 함수
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <param name="argSDATE"></param>
        /// <param name="argEDATE"></param>
        void READ_BST(string argPTNO, string argSDATE, string argEDATE, string argMedFrDate)
        {
            string SQL = string.Empty;
            DataTable dt = null;

            SSBST_Sheet1.ColumnCount = 1;
            EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "1572");
            //pForm.FmOLDGB = 0;
            ComplexBorder border2 = new ComplexBorder(null, null, new FarPoint.Win.ComplexBorderSide(Color.Black, 3), new FarPoint.Win.ComplexBorderSide(Color.Black), null, false, false);

            string strDate = string.Empty;

            try
            {
                #region XML               
                SQL = " SELECT CHARTDATE, CHARTTIME,";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta2') AS TA2,";
                SQL = SQL + ComNum.VBLF + "              (extractValue(chartxml, '//ta4') || ' ' || ";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta5') ||";
                SQL = SQL + ComNum.VBLF + "              extractValue(chartxml, '//ta9')) AS VAL2";
                SQL = SQL + ComNum.VBLF + "    From KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO IN (";
                SQL = SQL + ComNum.VBLF + "  SELECT EMRNO FROM KOSMOS_EMR.EMRXMLMST WHERE FORMNO = 1572";
                SQL = SQL + ComNum.VBLF + "      AND PTNO = '" + argPTNO + "'";
                SQL = SQL + ComNum.VBLF + "      AND CHARTDATE >= '" + argSDATE + "'";
                SQL = SQL + ComNum.VBLF + "      AND CHARTDATE <= '" + argEDATE + "')";
                #endregion

                #region 신규 기록지
                if (pForm.FmOLDGB != 1)
                {
                    SQL += ComNum.VBLF + "UNION ALL";
                    SQL += ComNum.VBLF + " SELECT CHARTDATE, CHARTTIME, B1.ITEMVALUE AS TA2, (B2.ITEMVALUE || ' ' || B3.ITEMVALUE || B4.ITEMVALUE) AS VAL2";
                    SQL += ComNum.VBLF + "    From KOSMOS_EMR.AEMRCHARTMST A";
                    SQL += ComNum.VBLF + "      LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B1";
                    SQL += ComNum.VBLF + "         ON A.EMRNO = B1.EMRNO";
                    SQL += ComNum.VBLF + "        AND A.EMRNOHIS = B1.EMRNOHIS";
                    SQL += ComNum.VBLF + "        AND B1.ITEMCD = 'I0000009122' -- Glucose";
                    SQL += ComNum.VBLF + "      LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B2";
                    SQL += ComNum.VBLF + "         ON A.EMRNO = B2.EMRNO";
                    SQL += ComNum.VBLF + "        AND A.EMRNOHIS = B2.EMRNOHIS";
                    SQL += ComNum.VBLF + "        AND B2.ITEMCD = 'I0000004686' -- Insulin";
                    SQL += ComNum.VBLF + "      LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B3";
                    SQL += ComNum.VBLF + "         ON A.EMRNO = B3.EMRNO";
                    SQL += ComNum.VBLF + "        AND A.EMRNOHIS = B3.EMRNOHIS";
                    SQL += ComNum.VBLF + "        AND B3.ITEMCD = 'I0000035480' -- 약용량";
                    SQL += ComNum.VBLF + "      LEFT OUTER JOIN KOSMOS_EMR.AEMRCHARTROW B4";
                    SQL += ComNum.VBLF + "         ON A.EMRNO = B4.EMRNO";
                    SQL += ComNum.VBLF + "        AND A.EMRNOHIS = B4.EMRNOHIS";
                    SQL += ComNum.VBLF + "        AND B4.ITEMCD = 'I0000001311' -- 비고";
                    SQL += ComNum.VBLF + "    WHERE FORMNO = 1572";
                    SQL += ComNum.VBLF + "      AND PTNO = '" + argPTNO + "'";
                    SQL += ComNum.VBLF + "      AND MEDFRDATE = '" + argMedFrDate + "'";
                    SQL += ComNum.VBLF + "      AND CHARTDATE >= '" + argSDATE + "'";
                    SQL += ComNum.VBLF + "      AND CHARTDATE <= '" + argEDATE + "'";
                }
                #endregion

                SQL += ComNum.VBLF + "      ORDER BY CHARTDATE DESC, CHARTTIME DESC";


                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }


                if (dt.Rows.Count > 0)
                {
                    string strEmrno = string.Empty;

                    strDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        

                        SSBST_Sheet1.ColumnCount += 1;
                        SSBST_Sheet1.Cells[0, i + 1].Text = VB.Val(VB.Right(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 4)).ToString("00/00") +
                        " " + VB.Val(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4)).ToString("00:00");
                        SSBST_Sheet1.Cells[0, i + 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        SSBST_Sheet1.Cells[0, i + 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                        SSBST_Sheet1.Cells[2, i + 1].Text = dt.Rows[i]["TA2"].ToString().Trim();
                        SSBST_Sheet1.Cells[3, i + 1].Text = dt.Rows[i]["VAL2"].ToString().Trim();

                        SSBST_Sheet1.Columns[i + 1].Width = SSBST_Sheet1.Columns[i + 1].GetPreferredWidth() + 5;

                        if (strDate.Equals(dt.Rows[i]["CHARTDATE"].ToString().Trim()) == false)
                        {
                            strDate = dt.Rows[i]["CHARTDATE"].ToString().Trim();
                            SSBST_Sheet1.Columns[i].Border = border2;
                        }

                    }
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return;
        }

        /// <summary>
        /// 지표 조회 함수
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <param name="argDATE"></param>
        /// <param name="ArgIPDNO"></param>
        /// <param name="ArgAge"></param>
        void READ_DETAIL_FALL(string argPTNO, string argDATE, string ArgIPDNO, string ArgAge)
        {
            try
            {
                string strFall = string.Empty;
                string strBraden = string.Empty;

                string strTOTAL = string.Empty;
                string strExam = string.Empty;
                string strCAUSE = string.Empty;
                string strDrug = string.Empty;

                string strWARD_C = string.Empty;
                string strAGE_C = string.Empty;

                string strTOOL = string.Empty;

                string SQL = string.Empty;

                DataTable dt = null;

                SQL = " SELECT WARDCODE, AGE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["WARDCODE"].ToString().Trim())
                    {
                        case "33":
                        case "35":
                            strFall = "OK";
                            strWARD_C = "중환자실 재원 환자";
                            break;

                        case "NR":
                        case "IQ":
                            strFall = "OK";
                            strWARD_C = "신생아실 재원 환자";
                            break;
                    }

                    double dAge = VB.Val(dt.Rows[0]["AGE"].ToString().Trim());
                    if (dAge >= 70)
                    {
                        strFall = "OK";
                        strAGE_C = "70세 이상 환자";
                    }

                    if (dAge < 7)
                    {
                        strFall = "OK";
                        strAGE_C = "7세 미만 환자";
                    }
                }

                dt.Dispose();

                SQL = "  SELECT PANO, TOTAL ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_FALLMORSE_SCALE";
                SQL += ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                SQL += ComNum.VBLF + " AND IPDNO = " + ArgIPDNO;
                SQL += ComNum.VBLF + " AND TOTAL >= 51";
                SQL += ComNum.VBLF + "     AND ROWID = (";
                SQL += ComNum.VBLF + "   SELECT ROWID FROM (";
                SQL += ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_FALLMORSE_SCALE";
                SQL += ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "       AND IPDNO = " + ArgIPDNO;
                SQL += ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                SQL += ComNum.VBLF + "  Where ROWNUM = 1)";
                SQL += ComNum.VBLF + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";
                strTOOL = "The Morse Fall Scale";

                if (VB.Val(ArgAge) < 18)
                {
                    SQL = "  SELECT PANO, TOTAL ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_FALLHUMPDUMP_SCALE";
                    SQL += ComNum.VBLF + " WHERE PANO = '" + argPTNO + "'";
                    SQL += ComNum.VBLF + " AND IPDNO = " + ArgIPDNO;
                    SQL += ComNum.VBLF + " AND (TOTAL >= 12 OR AGE < 7)";
                    SQL += ComNum.VBLF + "     AND ROWID = (";
                    SQL += ComNum.VBLF + "   SELECT ROWID FROM (";
                    SQL += ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_FALLHUMPDUMP_SCALE";
                    SQL += ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "       AND IPDNO = " + ArgIPDNO;
                    SQL += ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                    SQL += ComNum.VBLF + "  Where ROWNUM = 1)";
                    SQL += ComNum.VBLF + "  ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                    strTOOL = "The Humpty Dumpty Scale";
                }

                //'신생아의 경우 도구표 사용하지 않음
                if (strWARD_C == "신생아실 재원 환자")
                    strTOOL = "";
                

                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strTOTAL = dt.Rows[0]["TOTAL"].ToString().Trim();
                    strFall = "OK";
                }

                dt.Dispose();

                StringBuilder strTemp = new StringBuilder();

                SQL = " SELECT * ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_FALL_WARNING";
                SQL += ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;
                SQL += ComNum.VBLF + "   AND (WARNING1 = '1'";
                SQL += ComNum.VBLF + "                  OR WARNING2 = '1'";
                SQL += ComNum.VBLF + "                  OR WARNING3 = '1'";
                SQL += ComNum.VBLF + "                  OR WARNING4 = '1'";
                SQL += ComNum.VBLF + "                  OR DRUG_01 = '1'";
                SQL += ComNum.VBLF + "                  OR DRUG_02 = '1'";
                SQL += ComNum.VBLF + "                  OR DRUG_03 = '1'";
                SQL += ComNum.VBLF + "                  OR DRUG_04 = '1'";
                SQL += ComNum.VBLF + "                  OR DRUG_05 = '1'";
                SQL += ComNum.VBLF + "                  OR DRUG_06 = '1'";
                SQL += ComNum.VBLF + "                  OR DRUG_07 = '1'";
                SQL += ComNum.VBLF + "                  OR DRUG_08 = '1'";
                SQL += ComNum.VBLF + "                  OR DRUG_08_ETC <> '')";

                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strFall = "OK";

                    strCAUSE = "";

                    if (strAGE_C.Length == 0 && dt.Rows[0]["WARNING1"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 70세이상 ");
                    }

                    if (dt.Rows[0]["WARNING2"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 보행장애 ");
                    }

                    if (dt.Rows[0]["WARNING3"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 혼미 ");
                    }

                    if (dt.Rows[0]["WARNING4"].ToString().Trim() == "1")
                    {
                        strTemp.AppendLine(" ▶ 어지럼증 ");
                    }

                    strCAUSE = strTemp.ToString().Trim();

                    strTemp.Clear();
                    strDrug = "";

                    if (dt.Rows[0]["DRUG_01"].ToString().Trim() == "1")
                    {
                        strTemp.Append("진정제 ");
                    }
                    if (dt.Rows[0]["DRUG_02"].ToString().Trim() == "1")
                    {
                        strTemp.Append("수면제 ");
                    }
                    if (dt.Rows[0]["DRUG_03"].ToString().Trim() == "1")
                    {
                        strTemp.Append("향정신성약물 ");
                    }
                    if (dt.Rows[0]["DRUG_04"].ToString().Trim() == "1")
                    {
                        strTemp.Append("항우울제 ");
                    }
                    if (dt.Rows[0]["DRUG_05"].ToString().Trim() == "1")
                    {
                        strTemp.Append("완하제 ");
                    }
                    if (dt.Rows[0]["DRUG_06"].ToString().Trim() == "1")
                    {
                        strTemp.Append("이뇨제 ");
                    }
                    if (dt.Rows[0]["DRUG_07"].ToString().Trim() == "1")
                    {
                        strTemp.Append("진정약물 ");
                    }
                    if (dt.Rows[0]["DRUG_08"].ToString().Trim() == "1")
                    {
                        strTemp.Append(dt.Rows[0]["DRUG_08_ETC"].ToString().Trim());
                    }

                    strDrug = strTemp.ToString().Trim();

                }

                dt.Dispose();

                ssIndicator_Sheet1.Cells[1, 1].Text = strTOTAL;

                if (strFall == "OK")
                {
                    ssIndicator_Sheet1.Cells[2, 1].Text = "고위험";
                }

                strTemp.Clear();
                if (strWARD_C.Length > 0)
                {
                    strTemp.AppendLine(strWARD_C);
                }


                if (strAGE_C.Length > 0)
                {
                    strTemp.AppendLine(strAGE_C);
                }


                if (strCAUSE.Length > 0)
                {
                    strTemp.AppendLine(strCAUSE);
                }


                if (strDrug.Length > 0)
                {
                    strTemp.AppendLine("-위험약물: " + strDrug);
                }

                ssIndicator_Sheet1.Cells[3, 1].Text = strTemp.ToString().Trim();
                ssIndicator_Sheet1.Cells[4, 1].Text = strTOOL;



            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        /// <summary>
        /// 지표 조회 함수
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <param name="argDate"></param>
        /// <param name="argIPDNO"></param>
        /// <param name="argAge"></param>
        /// <param name="argWARD"></param>
        /// <param name="ArgDate2"></param>
        void READ_DETAIL_BRADEN(string argPTNO, string argDate, string argIPDNO, string argAge, string argWARD, string ArgDate2 = "")
        {
            OracleDataReader dataReader = null ;
            DataTable dt = null;
            string SQL = string.Empty;
            string sqlErr = string.Empty;

            string strBraden = string.Empty;
            string strOK = string.Empty;
            string strGUBUN = string.Empty;
            string strTOOL = string.Empty;

            double Total = 0;

            if (argWARD == "NR" || argWARD == "ND" || argWARD == "IQ")
            {
                strGUBUN = "신생아";
                strTOOL = "생아욕창사정 도구표";
            }
            else if (VB.Val(argAge) < 5)
            {
                strGUBUN = "소아";
                strTOOL = "소아욕창사정 도구표";
            }
            else
            {
                strGUBUN = string.Empty;
                strTOOL = "욕창사정 도구표";
            }

            if (strGUBUN.Length == 0)
            {
                SQL = " SELECT A.PANO, A.TOTAL, A.AGE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_SCALE A";
                SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + argIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + argPTNO + "' ";

                if (ArgDate2.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                }


                SQL = SQL + ComNum.VBLF + "     AND A.ROWID = (";
                SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                SQL = SQL + ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_BRADEN_SCALE";
                SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + argIPDNO;
                SQL = SQL + ComNum.VBLF + "       AND PANO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                SQL = SQL + ComNum.VBLF + "  ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), A.ENTDATE) DESC ";

                sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    double dAge = VB.Val(dt.Rows[0]["AGE"].ToString().Trim());
                    double dTotal = VB.Val(dt.Rows[0]["TOTAL"].ToString().Trim());
                    Total = dTotal;

                    if ((dAge >= 60 && dTotal <= 18) ||
                    dAge < 60 && dTotal <= 16)
                    {
                        strBraden = "OK";
                    }
                }

                dt.Dispose();
            }
            else if (strGUBUN == "소아")
            {
                SQL = " SELECT TOTAL";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_SCALE_CHILD A";
                SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + argIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + argPTNO + "' ";

                if (ArgDate2.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                }


                SQL = SQL + ComNum.VBLF + "  ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dataReader.HasRows && dataReader.Read())
                {
                    Total = VB.Val(dataReader.GetValue(0).ToString().Trim());
                    strOK = Total <= 16 ? "OK" : "";
                }

                dataReader.Dispose();
            }
            else if (strGUBUN == "신생아")
            {
                SQL = " SELECT TOTAL";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_SCALE_BABY A";
                SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + argIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + argPTNO + "' ";

                if (ArgDate2.Length > 0)
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + argDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + argDate + "','YYYY-MM-DD')";
                }


                SQL = SQL + ComNum.VBLF + "  ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), A.ENTDATE) DESC ";

                sqlErr = clsDB.GetAdoRs(ref dataReader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }


                if (dataReader.HasRows && dataReader.Read())
                {
                    Total = VB.Val(dataReader.GetValue(0).ToString().Trim());
                    strBraden = Total <= 20 ? "OK" : "";
                }

                dataReader.Dispose();
            }

            SQL = " SELECT *";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_WARNING ";
            SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;
            SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "   AND ( ";
            SQL = SQL + ComNum.VBLF + "         WARD_ICU = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR GRADE_HIGH = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR PARAL = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR COMA = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR NOT_MOVE = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR DIET_FAIL = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR NEED_PROTEIN = '1' ";
            SQL = SQL + ComNum.VBLF + "      OR EDEMA = '1'";
            SQL = SQL + ComNum.VBLF + "      OR (BRADEN = '1' AND (BRADEN_OK = '0' OR BRADEN_OK = NULL))";
            SQL = SQL + ComNum.VBLF + "      )";

            sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, sqlErr);
                return;
            }

            string strBun = string.Empty;

            if (dt.Rows.Count > 0)
            {
                strBraden = "OK";
                
                if(dt.Rows[0]["WARD_ICU"].ToString().Trim() == "1")
                {
                    strBun += "중환자실 ";
                }

                if (dt.Rows[0]["GRADE_HIGH"].ToString().Trim() == "1")
                {
                    strBun += "중증도 분류 3군 이상 ";
                }

                if (dt.Rows[0]["PARAL"].ToString().Trim() == "1")
                {
                    strBun += "뇌, 척추 관련 마비 ";
                }

                if (dt.Rows[0]["NOT_MOVE"].ToString().Trim() == "1")
                {
                    strBun += "부종 ";
                }

                if (dt.Rows[0]["DIET_FAIL"].ToString().Trim() == "1")
                {
                    strBun += "영양불량 ";
                }

                if (dt.Rows[0]["NEED_PROTEIN"].ToString().Trim() == "1")
                {
                    strBun += "단백질 불량 ";
                }

                if (dt.Rows[0]["EDEMA"].ToString().Trim() == "1")
                {
                    strBun += "부동 ";
                }

                if (dt.Rows[0]["BRADEN"].ToString().Trim() == "1")
                {
                    strBun += "현재 욕창이 있는 환자 ";
                }
            }

            dt.Dispose();

            ssIndicator_Sheet1.Cells[1, 3].Text = Total.ToString();

            ssIndicator_Sheet1.Cells[2, 3].Text = strBraden == "OK" ? "고위험" : "";

            ssIndicator_Sheet1.Cells[3, 3].Text = strBun;
            ssIndicator_Sheet1.Cells[4, 3].Text = strTOOL;

            return;
        }


        /// <summary>
        /// 지표 조회 함수
        /// </summary>
        /// <param name="argIPDNO"></param>
        /// <param name="ArgPano"></param>
        void READ_DETAIL_PAIN(string argIPDNO, string ArgPano)
        {
            string SQL = string.Empty;
            DataTable dt = null;

            try
            {
                
                SQL = " SELECT CYCLE, REGION, ASPECT, DETERIORATION, ";
                SQL = SQL + ComNum.VBLF + "  MITIGATION, SCORE, TOOLS, DURATION, ";
                SQL = SQL + ComNum.VBLF + "  DRUG, NODRUG, TIMES";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_PAIN_SCALE";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TRUNC(SYSDATE)    ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY ACTDATE DESC, ACTTIME DESC";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }


                if (dt.Rows.Count > 0)
                {
                    ssIndicator_Sheet1.Cells[1, 5].Text = dt.Rows[0]["SCORE"].ToString().Trim();
                    ssIndicator_Sheet1.Cells[2, 5].Text = dt.Rows[0]["REGION"].ToString().Trim();
                    ssIndicator_Sheet1.Cells[4, 5].Text = dt.Rows[0]["TOOLS"].ToString().Trim();
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return;
        }

        /// <summary>
        /// 간호문제 조회 함수
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <param name="argMEDFRDATE"></param>
        void READ_간호문제(string argPTNO, string argMEDFRDATE)
        {
            string SQL = string.Empty;
            DataTable dt = null;

            SSNurse_Sheet1.RowCount = 1;
            argMEDFRDATE = argMEDFRDATE.Replace("-", "");

            try
            {
                SQL = " SELECT A.RANKING, A.SEQNO, B.NURPROBLEM, A.GOAL, TO_CHAR(A.SDATE,'YYYY-MM-DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(A.EDATE,'YYYY-MM-DD') EDATE, A.BIGO, A.ROWID, '1' GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_CADEX_NURPROBLEM A, KOSMOS_EMR.EMR_CADEX_NURPROBLEM_CODE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.SEQNO = B.SEQNO";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.MEDFRDATE = '" + argMEDFRDATE + "' ";
                if (chkNurDel.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.EDATE IS NULL";
                }

                SQL = SQL + ComNum.VBLF + "  UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT RANKING, 0, PROBLEM, GOAL, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(EDATE,'YYYY-MM-DD') EDATE, '' BIGO, A.ROWID, '2' GUBUN";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_CARE_GOAL A";
                SQL = SQL + ComNum.VBLF + "  WHERE A.PTNO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.INDATE = TO_DATE('" + argMEDFRDATE + "','YYYY-MM-DD')  ";
                if (chkNurDel.Checked)
                {
                    SQL = SQL + ComNum.VBLF + "   AND EDATE IS NULL";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY SDATE ASC";


                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }


                if (dt.Rows.Count > 0)
                {
                    SSNurse_Sheet1.RowCount = dt.Rows.Count + 1;
                    SSNurse_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SSNurse_Sheet1.Cells[i + 1, 0].Text = dt.Rows[i]["NURPROBLEM"].ToString().Trim();
                        SSNurse_Sheet1.Cells[i + 1, 1].Text = dt.Rows[i]["GOAL"].ToString().Trim();
                        SSNurse_Sheet1.Cells[i + 1, 2].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                        SSNurse_Sheet1.Cells[i + 1, 3].Text = dt.Rows[i]["EDATE"].ToString().Trim();
                    }
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return;
        }

        private void CboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDoctor(cboDept.Text.Trim() == "0" ? "" : cboDept.Text.Trim());
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel2.VerticalScroll != null)
            {
                splitContainer1.Panel2.VerticalScroll.Value = 0;
            }
            panNurse.Visible = true;
            splitContainer1.SplitterDistance = 400;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            panNurse.Visible = false;
            splitContainer1.SplitterDistance = 700;
            if (splitContainer1.Panel2.VerticalScroll != null)
            {
                splitContainer1.Panel2.VerticalScroll.Value = 0;
            }
        }

        private void btnShowGraph_Click(object sender, EventArgs e)
        {
            if (panGraph.Visible == false)
            {
                splitContainer1.SplitterWidth = 15;
                splitContainer1.SplitterDistance = 500;
                GetVitalGraph();
                panGraph.Height = 480;
                panGraph.Visible = true;
                chartVital.Size = panGraphSub.Size;
            }
            else
            {
                SSVITAL.Visible = true;
                splitContainer1.Panel1Collapsed = true;
                panGraph.Visible = false;
            }
        }

        #region Grape관련

        private void mbtnSearchAll_Click(object sender, EventArgs e)
        {
            GetVitalGraph();
        }

        private double WheelValue = 0;

        private void GetVitalGraph()
        {

            //데이터 조회
            if (pAcp == null) return;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SSVITAL.Visible = false;
            splitContainer1.Panel1Collapsed = false;
            //splitContainer1.Panel1.Height = panGraph.Height;

            List<GV> GVLsit = new List<GV>();
            List<string> XList = new List<string>();

            bool blnData = false;
            int i = 0;

            chartVital.Series.Clear();
            chartVital.Titles.Clear();
            chartVital.ChartAreas.Clear();

            chartVital.ChartAreas.Add("Default");
            chartVital.Titles.Add("Vital Sign");
            chartVital.Titles[0].Font = new Font("굴림", 16F, FontStyle.Bold);

            if ((chkSBP.Checked == false) && (chkPR.Checked == false) && (chkRR.Checked == false) && (chkBT.Checked == false))
            {
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT                  ";
                SQL = SQL + ComNum.VBLF + "        A.CHARTDATE      ";
                SQL = SQL + ComNum.VBLF + "      , A.CHARTTIME      ";
                SQL = SQL + ComNum.VBLF + "      , B.ITEMCD         ";
                SQL = SQL + ComNum.VBLF + "      , B.ITEMVALUE      ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_EMR + "AEMRCHARTMST A ";
                SQL = SQL + ComNum.VBLF + "  INNER JOIN  " + ComNum.DB_EMR + "AEMRCHARTROW B";
                SQL = SQL + ComNum.VBLF + "     ON B.EMRNO = A.EMRNO";
                SQL = SQL + ComNum.VBLF + "    AND B.EMRNOHIS = A.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "    AND B.ITEMCD IN('I0000002018','I0000001765','I0000014815','I0000002009','I0000001811' ) ";

                if (pAcp.inOutCls == "I")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = '" + pAcp.acpNo + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + pAcp.ptNo + "'";
                }

                SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = 3150";
                SQL = SQL + ComNum.VBLF + "  AND A.UPDATENO = 1";
                SQL = SQL + ComNum.VBLF + "  AND A.MEDFRDATE = '" + pAcp.medFrDate + "'";
                SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE >= '" + dtpGraphFDate.Value.ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE <= '" + dtpGraphTDate.Value.ToString("yyyyMMdd") + "'";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.CHARTDATE ASC , A.CHARTTIME ASC , A.EMRNO ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    string strDateTime = VB.Right(dt.Rows[i]["CHARTDATE"].ToString().Trim(), 4).Insert(2, "-") + "\r\n" + VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4).Insert(2, ":");

                    if (XList.IndexOf(strDateTime) == -1)
                    {
                        XList.Add(strDateTime);
                    }

                    if (chkSBP.Checked == true)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000002018")
                        {
                            if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 0)
                            {
                                blnData = true;

                                if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 100)
                                {
                                    GVLsit.Add(new GV()
                                    {
                                        Code = "SBP"
                                        ,
                                        X = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                                        ,
                                        Y = strDateTime
                                    }
                                    );
                                }
                                else
                                {
                                    GVLsit.Add(new GV()
                                    {
                                        Code = "SBP2"
                                        ,
                                        X = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                                        ,
                                        Y = strDateTime
                                    }
                                    );
                                }
                            }
                        }

                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000001765")
                        {
                            if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 0)
                            {
                                blnData = true;

                                GVLsit.Add(new GV()
                                {
                                    Code = "DBP"
                                    ,
                                    X = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                                    ,
                                    Y = strDateTime
                                }
                                );
                            }
                        }
                    }

                    if (chkPR.Checked == true)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000014815")
                        {
                            if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 0)
                            {
                                blnData = true;

                                GVLsit.Add(new GV()
                                {
                                    Code = "맥박"
                                    ,
                                    X = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                                    ,
                                    Y = strDateTime
                                }
                                );
                            }
                        }
                    }

                    if (chkRR.Checked == true)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000002009")
                        {

                            if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 0)
                            {
                                blnData = true;

                                GVLsit.Add(new GV()
                                {
                                    Code = "호흡"
                                    ,
                                    X = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                                    ,
                                    Y = strDateTime
                                }
                                );
                            }
                        }
                    }

                    if (chkBT.Checked == true)
                    {
                        if (dt.Rows[i]["ITEMCD"].ToString().Trim() == "I0000001811")
                        {
                            if (VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim()) > 0)
                            {
                                blnData = true;

                                GVLsit.Add(new GV()
                                {
                                    Code = "체온"
                                    ,
                                    X = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                                    ,
                                    Y = strDateTime
                                }
                                );
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if (blnData == false) return;

                chartVital.ChartAreas["Default"].Position = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(0, 5, 85, 90);
                chartVital.ChartAreas["Default"].InnerPlotPosition = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(13, 5, 90, 90);

                if (chkSBP.Checked == true)
                {
                    chartVital.Series.Add("SBP");
                    chartVital.Series["SBP"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chartVital.Series["SBP"].MarkerImage = clsEmrType.EmrSvrInfo.EmrClient + "\\Icon\\SBP.png";
                    chartVital.Series["SBP"].IsValueShownAsLabel = false;

                    chartVital.Series.Add("DBP");
                    chartVital.Series["DBP"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chartVital.Series["DBP"].MarkerImage = clsEmrType.EmrSvrInfo.EmrClient + "\\Icon\\DBP.png";
                    chartVital.Series["DBP"].IsValueShownAsLabel = false;

                    chartVital.Series.Add("SBP2");
                    chartVital.Series["SBP2"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    chartVital.Series["SBP2"].MarkerImage = clsEmrType.EmrSvrInfo.EmrClient + "\\Icon\\SBP2.png";
                    chartVital.Series["SBP2"].IsValueShownAsLabel = false;
                    chartVital.Series["SBP2"].IsVisibleInLegend = false;

                }

                if (chkPR.Checked == true)
                {
                    chartVital.Series.Add("맥박");
                    chartVital.Series["맥박"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chartVital.Series["맥박"].BorderWidth = 2;
                    chartVital.Series["맥박"].Color = Color.Blue;
                    chartVital.Series["맥박"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chartVital.Series["맥박"].MarkerColor = Color.Blue;
                    chartVital.Series["맥박"].MarkerSize = 6;

                }

                if (chkRR.Checked == true)
                {
                    chartVital.Series.Add("호흡");
                    chartVital.Series["호흡"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chartVital.Series["호흡"].BorderWidth = 2;
                    chartVital.Series["호흡"].Color = Color.Green;
                    chartVital.Series["호흡"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chartVital.Series["호흡"].MarkerColor = Color.Green;
                    chartVital.Series["호흡"].MarkerSize = 6;

                }

                if (chkBT.Checked == true)
                {
                    chartVital.Series.Add("체온");
                    chartVital.Series["체온"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chartVital.Series["체온"].BorderWidth = 2;
                    chartVital.Series["체온"].Color = Color.Red;
                    chartVital.Series["체온"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                    chartVital.Series["체온"].MarkerColor = Color.Red;
                    chartVital.Series["체온"].MarkerSize = 6;

                }

                chartVital.Series.Add("주의선");
                chartVital.Series["주의선"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chartVital.Series["주의선"].BorderWidth = 2;
                chartVital.Series["주의선"].Color = Color.Orange;
                chartVital.Series["주의선"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.None;


                // X축 그리기

                XList.Sort();

                foreach (string DateTiem in XList)
                {
                    if (GVLsit.Where(d => d.Y == DateTiem).Any())
                    {
                        List<GV> list = GVLsit.Where(d => d.Y == DateTiem).ToList();

                        foreach (System.Windows.Forms.DataVisualization.Charting.Series series in chartVital.Series)
                        {
                            if (list.Where(d => d.Code == series.Name).Any())
                            {
                                series.Points.AddY(list.Where(d => d.Code == series.Name).First().X);
                            }
                            else
                            {
                                if (series.Name == "주의선")
                                {
                                    series.Points.AddY(100);
                                    continue;
                                }

                                series.Points.AddY(double.NaN);
                                series.Points[series.Points.Count - 1].IsEmpty = true;
                            }
                        }

                        chartVital.Series[0].Points[chartVital.Series[0].Points.Count - 1].AxisLabel = DateTiem;

                    }
                }

                chartVital.ChartAreas["Default"].AxisX.Interval = 1;
                chartVital.ChartAreas["Default"].AxisY.Interval = 10;
                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 10;
                chartVital.ChartAreas["Default"].AxisY.Minimum = 40; //30
                chartVital.ChartAreas["Default"].AxisY.Maximum = 250; //250
                chartVital.ChartAreas["Default"].Position.X = 12;
                chartVital.ChartAreas["Default"].InnerPlotPosition.X = 2;
                chartVital.ChartAreas["Default"].AxisY.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
                chartVital.ChartAreas["Default"].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
                chartVital.ChartAreas["Default"].AxisX.ScrollBar.Enabled = true;
                chartVital.ChartAreas["Default"].AxisX.ScaleView.Zoomable = true;

                chartVital.Series["주의선"].ChartArea = "Default";


                CheckBox[] checkAee = new CheckBox[4];

                checkAee[0] = chkSBP;
                checkAee[1] = chkPR;
                checkAee[2] = chkRR;
                checkAee[3] = chkBT;


                int f = 1;


                foreach (CheckBox box in checkAee)
                {
                    if (box.Checked == true)
                    {

                        if (box == chkPR)
                        {
                            if (chkSBP.Checked == false && f == 1)
                            {
                                chartVital.ChartAreas["Default"].AxisY.Interval = 10;
                                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 10;
                                chartVital.ChartAreas["Default"].AxisY.Minimum = 30;
                                chartVital.ChartAreas["Default"].AxisY.Maximum = 150;
                                chartVital.ChartAreas["Default"].AxisY.LineColor = Color.Blue;
                                chartVital.ChartAreas["Default"].AxisY.InterlacedColor = Color.Blue;

                            }

                            f += 1;

                        }

                        if (box == chkRR)
                        {
                            if (chkSBP.Checked == false && f == 1)
                            {
                                chartVital.ChartAreas["Default"].AxisY.Interval = 1;
                                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 1;
                                chartVital.ChartAreas["Default"].AxisY.Minimum = 10;
                                chartVital.ChartAreas["Default"].AxisY.Maximum = 40;
                                chartVital.ChartAreas["Default"].AxisY.LineColor = Color.Green;
                                chartVital.ChartAreas["Default"].AxisY.InterlacedColor = Color.Green;
                            }

                            f += 1;

                        }

                        if (box == chkBT)
                        {
                            if (chkSBP.Checked == false && f == 1)
                            {
                                chartVital.ChartAreas["Default"].AxisY.Interval = 0.5;
                                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 0.5;
                                chartVital.ChartAreas["Default"].AxisY.Minimum = 34.0;
                                chartVital.ChartAreas["Default"].AxisY.Maximum = 45.0;
                                chartVital.ChartAreas["Default"].AxisY.LineColor = Color.Red;
                                chartVital.ChartAreas["Default"].AxisY.InterlacedColor = Color.Red;
                            }

                            f += 1;

                        }

                    }
                }

                if (chkSBP.Checked == false)
                {
                    f = f - 1;
                }

                int PositionX = 0; // Y축범위 가로 간격

                if (chartVital.Width <= 970)
                {
                    PositionX = 4;
                }
                else
                {
                    PositionX = 3;
                }

                chartVital.ChartAreas["Default"].Position.X = PositionX * f;


                f = 1;

                foreach (CheckBox box in checkAee)
                {
                    if (box.Checked == true)
                    {
                        if (box == chkRR)
                        {
                            if (chartVital.ChartAreas["Default"].AxisY.LineColor == Color.Green)
                                continue;

                            CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["호흡"], PositionX * f, 2);
                            f += 1;
                        }

                        if (box == chkPR)
                        {
                            if (chartVital.ChartAreas["Default"].AxisY.LineColor == Color.Blue)
                                continue;

                            CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["맥박"], PositionX * f, 2);
                            f += 1;
                        }

                        if (box == chkBT)
                        {
                            if (chartVital.ChartAreas["Default"].AxisY.LineColor == Color.Red)
                                continue;

                            CreateYAxis(chartVital, chartVital.ChartAreas["Default"], chartVital.Series["체온"], PositionX * f, 2);
                            f += 1;
                        }
                    }
                }

                chartVital.ChartAreas["Default"].AxisY.LineWidth = 2;

                if (chartVital.ChartAreas.Count > 0)
                {
                    for (int k = 1; k < chartVital.ChartAreas.Count; k++)
                    {
                        if (VB.Split(chartVital.ChartAreas[k].Name, "_")[1] == "맥박")
                        {
                            chartVital.ChartAreas[k].AxisY.Interval = 10;
                            //chartVital.ChartAreas[k].AxisY.IntervalOffset = 10;
                            chartVital.ChartAreas[k].AxisY.Minimum = 30;
                            chartVital.ChartAreas[k].AxisY.Maximum = 150;
                        }
                        else if (VB.Split(chartVital.ChartAreas[k].Name, "_")[1] == "체온")
                        {
                            chartVital.ChartAreas[k].AxisY.Interval = 0.5;
                            //chartVital.ChartAreas[k].AxisY.IntervalOffset = 0.5;
                            chartVital.ChartAreas[k].AxisY.Minimum = 34.0;
                            chartVital.ChartAreas[k].AxisY.Maximum = 45.0;
                        }
                        else if (VB.Split(chartVital.ChartAreas[k].Name, "_")[1] == "호흡")
                        {
                            chartVital.ChartAreas[k].AxisY.Interval = 1;
                            //chartVital.ChartAreas[k].AxisY.IntervalOffset = 0.5;
                            chartVital.ChartAreas[k].AxisY.Minimum = 10;
                            chartVital.ChartAreas[k].AxisY.Maximum = 40;
                        }
                    }
                }

                WheelValue = 21;

                chartVital.ChartAreas["Default"].AxisX.ScaleView.Zoom(0, WheelValue);

                if (chartVital.Series[0].Points.Count > WheelValue)
                {
                    chartVital.ChartAreas["Default"].AxisX.ScaleView.Scroll(chartVital.Series[0].Points.Count - WheelValue);
                }

                foreach (System.Windows.Forms.DataVisualization.Charting.ChartArea item in chartVital.ChartAreas)
                {
                    if (item == chartVital.ChartAreas["Default"])
                        continue;

                    item.AxisX.ScrollBar.Enabled = true;
                    item.AxisX.ScaleView.Zoomable = true;
                    item.AxisX.ScrollBar.ButtonStyle = System.Windows.Forms.DataVisualization.Charting.ScrollBarButtonStyles.None;
                    item.AxisX.ScrollBar.ChartArea.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
                    item.AxisX.ScrollBar.LineColor = Color.Transparent;
                    item.AxisX.ScaleView.Zoom(0, WheelValue);

                    if (chartVital.Series[0].Points.Count > WheelValue)
                    {
                        item.AxisX.ScaleView.Scroll(chartVital.Series[0].Points.Count - WheelValue);
                    }

                }

                //=========
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBoxEx(this, ex.Message);

            }

            Cursor.Current = Cursors.Default;

        }

        private void chartVital_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                if (chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMaximum < WheelValue + 1)
                {
                    WheelValue = chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMaximum;
                }
                else
                {
                    WheelValue = WheelValue + 1;
                }
            }

            if (e.Delta > 0)
            {
                if (chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMinimum > WheelValue)
                {
                    WheelValue = chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMinimum;
                }
                else
                {
                    WheelValue = WheelValue - 1;
                }


            }

            chartVital.ChartAreas["Default"].AxisX.ScaleView.Zoom(0, WheelValue);

            foreach (System.Windows.Forms.DataVisualization.Charting.ChartArea item in chartVital.ChartAreas)
            {
                if (item == chartVital.ChartAreas["Default"])
                    continue;

                item.AxisX.ScaleView.Zoom(0, WheelValue);
            }

        }

        private void chartVital_DoubleClick(object sender, EventArgs e)
        {
            chartVital.ChartAreas["Default"].AxisX.ScaleView.ZoomReset();

            foreach (System.Windows.Forms.DataVisualization.Charting.ChartArea item in chartVital.ChartAreas)
            {
                if (item == chartVital.ChartAreas["Default"])
                    continue;

                item.AxisX.ScaleView.ZoomReset();
            }
        }

        public void CreateYAxis(System.Windows.Forms.DataVisualization.Charting.Chart chart, System.Windows.Forms.DataVisualization.Charting.ChartArea area,
            System.Windows.Forms.DataVisualization.Charting.Series series, float axisOffset, float labelsSize)
        {
            // Create new chart area for original series
            System.Windows.Forms.DataVisualization.Charting.ChartArea areaSeries = chart.ChartAreas.Add("ChartArea_" + series.Name);
            areaSeries.BackColor = Color.Transparent;
            areaSeries.BorderColor = Color.Transparent;
            areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
            areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
            areaSeries.AxisX.MajorGrid.Enabled = false;
            areaSeries.AxisX.MajorTickMark.Enabled = false;
            areaSeries.AxisX.LabelStyle.Enabled = false;
            areaSeries.AxisY.MajorGrid.Enabled = false;
            areaSeries.AxisY.MajorTickMark.Enabled = false;
            areaSeries.AxisY.LabelStyle.Enabled = false;
            areaSeries.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;


            series.ChartArea = areaSeries.Name;

            if (series.Name == "체온"
                || series.Name == "맥박"
                || series.Name == "호흡")
            {

                // Create new chart area for axis
                System.Windows.Forms.DataVisualization.Charting.ChartArea areaAxis = chart.ChartAreas.Add("AxisY-" + series.ChartArea);
                areaAxis.BackColor = Color.Transparent;
                areaAxis.BorderColor = Color.Transparent;
                areaAxis.Position.FromRectangleF(chart.ChartAreas[series.ChartArea].Position.ToRectangleF());
                areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas[series.ChartArea].InnerPlotPosition.ToRectangleF());

                // Create a copy of specified series
                System.Windows.Forms.DataVisualization.Charting.Series seriesCopy = chart.Series.Add(series.Name + "_Copy");
                seriesCopy.ChartType = series.ChartType;
                foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint point in series.Points)
                {
                    seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
                }

                // Hide copied series
                seriesCopy.IsVisibleInLegend = false;
                seriesCopy.Color = Color.Transparent;
                seriesCopy.BorderColor = Color.Transparent;
                seriesCopy.ChartArea = areaAxis.Name;

                // Disable drid lines & tickmarks
                areaAxis.AxisX.LineWidth = 0;
                areaAxis.AxisX.MajorGrid.Enabled = false;
                areaAxis.AxisX.MajorTickMark.Enabled = false;
                areaAxis.AxisX.LabelStyle.Enabled = false;
                areaAxis.AxisY.MajorGrid.Enabled = false;
                areaAxis.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
                areaAxis.AxisY.LabelStyle.Font = area.AxisY.LabelStyle.Font;

                if (series.Name == "체온")
                {
                    areaAxis.AxisY.LineColor = Color.Red;
                    areaAxis.AxisY.InterlacedColor = Color.Red;
                }
                else if (series.Name == "맥박")
                {
                    areaAxis.AxisY.LineColor = Color.Blue;
                    areaAxis.AxisY.InterlacedColor = Color.Blue;
                }
                else if (series.Name == "호흡")
                {
                    areaAxis.AxisY.LineColor = Color.Green;
                    areaAxis.AxisY.InterlacedColor = Color.Green;
                }

                areaAxis.AxisY.LineWidth = 2;

                // Adjust area position
                areaAxis.Position.X = axisOffset;
                areaAxis.InnerPlotPosition.X = labelsSize;
            }

        }
        private void btnHideGraph_Click(object sender, EventArgs e)
        {
            SSVITAL.Visible = true;
            splitContainer1.Panel1Collapsed = true;
            //panGraph.Visible = false;
        }

        #endregion Grape관련

        private void btnSideBarLeft_Click(object sender, EventArgs e)
        {
            if (btnSideBarLeft.Text.Equals("리스트 숨기기"))
            {
                panLeft.Visible = false;
                btnSideBarLeft.Text = "리스트 보이기";
                if (splitContainer1.Panel1Collapsed)
                {
                    GetVitalGraph();
                }
            }
            else
            {
                btnSideBarLeft.Text = "리스트 숨기기";
                panLeft.Visible = true;
            }
        }

        private void lblSideBarLeft_Click(object sender, EventArgs e)
        {
            btnSideBarLeft.Text = "◁";
            panLeft.Visible = true;
            GetVitalGraph();
        }

        private void rdoIpdSort1_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                Read_Patient_List();
            }
        }
    }
}
