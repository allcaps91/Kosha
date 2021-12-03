using ComBase;
using ComBase.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBloodInfo : Form, EmrChartForm, FormEmrMessage
    {
        #region 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;
        public string mstrFormNo = "1761";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public EmrPatient p = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "W";
        private Dictionary<string, string> pstrBlood = new Dictionary<string, string>();
        /// <summary>
        /// 수혈기록지.
        /// </summary>
        frmEmrChartNew frmEmrChartNewX = null;
        #endregion

        #region EmrChartForm
        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }

        public double SaveDataMsg(string strFlag)
        {
            //TODO
            //double dblEmrNo = pSaveData(strFlag);
            //return dblEmrNo;
            return 0;
        }

        public bool DelDataMsg()
        {
            //TODO
            //return pDelData();
            return false;
        }

        public void ClearFormMsg()
        {
            //TODO
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

            //if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            //{
            //    return rtnVal;
            //}

            //rtnVal = clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0; // clsFormPrint.PrintToTifFileLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        #endregion

        #region FormEmrMessage
        public void MsgSave(string strSaveFlag)
        {
            GetBloodIO();
        }

        public void MsgDelete()
        {
            return;

        }

        public void MsgClear()
        {
            return;

        }

        public void MsgPrint()
        {
            return;

        }
        #endregion

        #region 생성자
        public frmEmrBloodInfo()
        {
            InitializeComponent();
        }

        public frmEmrBloodInfo(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
        }

        public frmEmrBloodInfo(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
        }
        #endregion

        /// <summary>
        /// 간호부 참고 이미지
        /// </summary>
        Form frmImgRmk = null;

        private void frmEmrBaseVitalAndActing_Load(object sender, EventArgs e)
        {
            if (clsType.User.BuseCode.Equals("078201"))
            {
                splitContainer1.SplitterDistance = 800;
            }

            dtpOptSDate.Value = Convert.ToDateTime(clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade, p.inOutCls, p.medDeptCd, clsType.User.IdNumber,
                        DateTime.ParseExact(p.medFrDate, "yyyyMMdd", null).ToString("yyyy-MM-dd")));
            dtpOptEDate.Value = p.medDeptCd.Equals("ER") ? dtpOptSDate.Value.AddDays(+1) : DateTime.ParseExact(string.IsNullOrWhiteSpace(p.medEndDate) ? ComQuery.CurrentDateTime(clsDB.DbCon, "D") : p.medEndDate,   "yyyyMMdd", null);
            GetBloodIO();
        }

        private void frmEmrBaseVitalAndActing_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmImgRmk != null)
            {
                frmImgRmk.Dispose();
                frmImgRmk = null;
            }

            if (frmEmrChartNewX != null)
            {
                frmEmrChartNewX.Dispose();
                frmEmrChartNewX = null;
            }
        }

        #region 함수

        /// <summary>
        /// 수혈기록지 데이터 넣기
        /// 불출일시, 불출자, 혈액번호, 혈액종류/양, 혈액형
        /// </summary>
        private void SET_BLOOD_CHART(int Row)
        {
            try
            {
                //I0000037485
                //I0000009925, I0000037484, I0000001738

                string[] strArr = ssList_Sheet1.Cells[Row, 0 + 1].Text.Trim().Split(' ');
                string strDate = strArr[0];//불출일자
                string strTime = strArr[1];//불출시간

                string strBlood = ssList_Sheet1.Cells[Row, 2 + 1].Text.Trim();//혈액형
                string strGubun = ssList_Sheet1.Cells[Row, 3 + 1].Text.Trim();//종류
                string strCapacity = ssList_Sheet1.Cells[Row, 4 + 1].Text.Trim();//양

                //불출일
                Control control = panChart.Controls.Find("I0000009935", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strDate;
                }

                //불출시간
                control = panChart.Controls.Find("I0000037483", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strTime;
                }

                //불출자
                control = panChart.Controls.Find("I0000037485", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = ssList_Sheet1.Cells[Row, 5 + 1].Text.Trim();
                }

                //혈액번호
                control = panChart.Controls.Find("I0000009925", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = ssList_Sheet1.Cells[Row, 1 + 1].Text.Trim();
                }

                //혈액종류/양
                control = panChart.Controls.Find("I0000037484", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strGubun + "/" + strCapacity;
                    control.Tag = ssList_Sheet1.Cells[Row, 3 + 1].Tag;
                }

                //혈액형
                control = panChart.Controls.Find("I0000001738", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strBlood;
                }

                //1 Unit 투여량
                control = panChart.Controls.Find("I0000037510", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = ssList_Sheet1.Cells[Row, 6 + 1].Text.Trim();
                }
            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 수혈기록지 데이터 넣기
        /// 불출일시, 불출자, 혈액번호, 혈액종류/양, 혈액형
        /// </summary>
        private void SET_BLOOD_CHART_PLTC(int Row)
        {
            try
            {
                //I0000037485
                //I0000009925, I0000037484, I0000001738

                string[] strArr = ssList_Sheet1.Cells[Row, 0 + 1].Text.Trim().Split(' ');
                string strDate = strArr[0];//불출일자
                string strTime = strArr[1];//불출시간

                string strBlood = ssList_Sheet1.Cells[Row, 2 + 1].Text.Trim();//혈액형
                string strGubun = ssList_Sheet1.Cells[Row, 3 + 1].Text.Trim();//종류
                string strCapacity = ssList_Sheet1.Cells[Row, 4 + 1].Text.Trim();//양

                //불출일
                Control control = panChart.Controls.Find("I0000009935", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strDate;
                }

                //불출시간
                control = panChart.Controls.Find("I0000037483", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strTime;
                }

                //불출자
                control = panChart.Controls.Find("I0000037485", true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = ssList_Sheet1.Cells[Row, 5 + 1].Text.Trim();
                }

                //혈액번호
                control = panChart.Controls.Find("I0000009925_" + (Row + 1), true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = ssList_Sheet1.Cells[Row, 1 + 1].Text.Trim();
                }

                //혈액종류/양
                control = panChart.Controls.Find("I0000037484_" + (Row + 1), true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strGubun + "/" + strCapacity;
                    control.Tag = ssList_Sheet1.Cells[Row, 3 + 1].Tag;
                    clsEmrFunc.PanelAutoSize(control.Parent, true);
                }

                //혈액형
                control = panChart.Controls.Find("I0000001738_" + (Row + 1), true).FirstOrDefault();
                if (control != null)
                {
                    control.Text = strBlood;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
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
            pstrBlood.Add("BT31" , "PRP(혈소판풍부혈장)");
            pstrBlood.Add("BT27" , "ph-P");
            pstrBlood.Add("BT24" , "ph-PLT");
            pstrBlood.Add("BT25" , "ph-WBC");
            pstrBlood.Add("BT26" , "ph-CB");
            pstrBlood.Add("BT081", "F/RBC(백혈구여과제거 적혈구)");
        }

        string GetBloodCnt(string strName, string strCnt)
        {
            string rtnVal = string.Empty;
            switch (strName)
            {
                case "P/C(농축적혈구)":
                    if (strCnt.Equals("400"))
                    {
                        rtnVal = "240";
                    }
                    else if (strCnt.Equals("320"))
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

        /// <summary>
        /// 
        /// </summary>
        void GetBloodIO()
        {
            Blood_Code_SET();


            #region 변수
            //OracleDataReader oracleDataReader = null;
            string SQL = string.Empty;
            string sqlErr = string.Empty;
            DataTable dt = null;
            StringBuilder strStatus = new StringBuilder();
            StringBuilder strBloodPickUp = new StringBuilder();

            /// <summary>
            /// 날짜 음영 
            /// </summary>
            string strOldDate = string.Empty;

            /// <summary>
            /// 날짜 색상
            /// </summary>
            Color dblColorInfo;

            //string strSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            //string strSysTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");

            #endregion
            ssList_Sheet1.RowCount = 0;

            //List<string> BloodList = new List<string>();
            dblColorInfo = Color.White;

            try
            {

                #region 3
                SQL = " SELECT";
                SQL += ComNum.VBLF + "   B.OUTDATE, B.CAPACITY, B.BLOODNO, C.PTABO || C.PTRH PTAR, B.COMPONENT, ";
                SQL += ComNum.VBLF + "   B.JANDATE, B.DestroyDate, B.OUTRDATE, B.EMERGENCY, B.OUTBUSE, B.OUTRCAUSE, '' AS REMARK, B.OUTPERSON, E.EMRNO -- C.GUMSAJA";
                #region 시작시간
                SQL += ComNum.VBLF + "   ,(";
                SQL += ComNum.VBLF + "   SELECT";
                SQL += ComNum.VBLF + "   S.ITEMVALUE || ' ' || S2.ITEMVALUE";
                SQL += ComNum.VBLF + "       FROM ADMIN.AEMRCHARTROW S";
                SQL += ComNum.VBLF + "         INNER JOIN ADMIN.AEMRCHARTROW S2";
                SQL += ComNum.VBLF + "            ON S2.EMRNO  = S.EMRNO";
                SQL += ComNum.VBLF + "           AND S2.EMRNOHIS  = S.EMRNOHIS ";
                SQL += ComNum.VBLF + "           AND S2.ITEMCD  = 'I0000037487'";
                SQL += ComNum.VBLF + "       WHERE S.EMRNO = E.EMRNO";
                SQL += ComNum.VBLF + "         AND S.EMRNOHIS > 0";
                SQL += ComNum.VBLF + "         AND S.ITEMCD = 'I0000037486'";
                SQL += ComNum.VBLF + "   )AS SDATE";
                #endregion
                #region 종료시간
                SQL += ComNum.VBLF + "   ,(";
                SQL += ComNum.VBLF + "   SELECT";
                SQL += ComNum.VBLF + "   E1.ITEMVALUE || ' ' || E2.ITEMVALUE";
                SQL += ComNum.VBLF + "       FROM ADMIN.AEMRCHARTROW E1";
                SQL += ComNum.VBLF + "         INNER JOIN ADMIN.AEMRCHARTROW E2";
                SQL += ComNum.VBLF + "            ON E2.EMRNO  = E1.EMRNO";
                SQL += ComNum.VBLF + "           AND E2.EMRNOHIS  = E1.EMRNOHIS ";
                SQL += ComNum.VBLF + "           AND E2.ITEMCD  = 'I0000037491'";
                SQL += ComNum.VBLF + "       WHERE E1.EMRNO = E.EMRNO";
                SQL += ComNum.VBLF + "         AND E1.EMRNOHIS > 0";
                SQL += ComNum.VBLF + "         AND E1.ITEMCD = 'I0000037490'";
                SQL += ComNum.VBLF + "   )AS EDATE";
                #endregion
                SQL += ComNum.VBLF + "  FROM ADMIN.EXAM_BLOODTRANS A";
                SQL += ComNum.VBLF + "    INNER JOIN ADMIN.EXAM_BLOOD_IO B";
                SQL += ComNum.VBLF + "       ON A.PANO = B.PANO";
                SQL += ComNum.VBLF + "      AND A.BLOODNO = B.BLOODNO";
                
                SQL += ComNum.VBLF + "    INNER JOIN ADMIN.EXAM_BLOODCROSSM C";
                SQL += ComNum.VBLF + "       ON A.PANO = C.PANO";
                SQL += ComNum.VBLF + "      AND A.BLOODNO = C.BLOODNO";
                SQL += ComNum.VBLF + "      AND C.GBSTATUS <> '3'";
                SQL += ComNum.VBLF + "     LEFT OUTER JOIN ADMIN.EMR_DATA_MAPPING E";
                SQL += ComNum.VBLF + "       ON E.MAPPING1 = TRIM(A.BLOODNO) || TRIM(B.COMPONENT)";
                SQL += ComNum.VBLF + "      AND E.FORMNO IN(1965, 3535)";
                SQL += ComNum.VBLF + " WHERE A.PANO = '" + p.ptNo + "'";
                SQL += ComNum.VBLF + "   AND A.GBJOB IN ('3','4')";

                if (rdoGBN2.Checked)
                {
                    SQL += ComNum.VBLF + "   AND B.COMPONENT = 'BT023'";
                }

                SQL += ComNum.VBLF + "   AND A.BDATE >= TO_DATE('" + dtpOptSDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + dtpOptEDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";


                //if (p.inOutCls.Equals("I"))
                //{
                //    if (p.medEndDate.Length > 0)
                //    {
                //        SQL += ComNum.VBLF + "   AND A.BDATE >= TO_DATE('" + dtpOptSDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                //        SQL += ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + dtpOptEDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                //    }

                //    SQL += ComNum.VBLF + "   AND A.BDATE >= TO_DATE('" + dtpOptSDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                //}
                //else
                //{
                //    if (p.medDeptCd.Equals("ER"))
                //    {
                //        SQL += ComNum.VBLF + "   AND A.BDATE >= TO_DATE('" + dtpOptSDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                //        SQL += ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + dtpOptEDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";

                //    }
                //    else
                //    {
                //        SQL += ComNum.VBLF + "   AND A.BDATE = TO_DATE('" + dtpOptSDate.Value.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')";
                //    }
                //}

      
                SQL += ComNum.VBLF + "GROUP BY  B.OUTDATE, B.CAPACITY, B.BLOODNO, C.PTABO || C.PTRH , B.COMPONENT,";
                SQL += ComNum.VBLF + "          B.JANDATE, B.DestroyDate, B.OUTRDATE, B.EMERGENCY, B.OUTBUSE, B.OUTRCAUSE, '' , B.OUTPERSON, E.EMRNO--, C.GUMSAJA,";
                SQL += ComNum.VBLF + "ORDER BY  B.OUTDATE DESC";

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
                    //if (BloodList.IndexOf(dt.Rows[i]["BLOODNO"].ToString().Trim()) != -1)
                    //{
                    //    continue;
                    //}

                    //BloodList.Add(dt.Rows[i]["BLOODNO"].ToString().Trim());

                    ssList_Sheet1.RowCount += 1;

                    strBloodPickUp.Clear();

                    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0 + 1].Text = Convert.ToDateTime(dt.Rows[i]["OUTDATE"].ToString().Trim()).ToShortDateString() + " " + Convert.ToDateTime(dt.Rows[i]["OUTDATE"].ToString().Trim()).ToString("HH:mm");
                    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 1 + 1].Text = dt.Rows[i]["BLOODNO"].ToString().Trim();

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

                    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 2 + 1].Text = dt.Rows[i]["PTAR"].ToString().Trim();
                    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 3 + 1].Text = strVal;
                    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 3 + 1].Tag = dt.Rows[i]["Component"].ToString().Trim();
                    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 4 + 1].Text = dt.Rows[i]["CAPACITY"].ToString().Trim();
                    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 5 + 1].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["OUTPERSON"].ToString().Trim());

                    pstrBlood.TryGetValue(dt.Rows[i]["Component"].ToString().Trim(), out strVal);

                    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 6 + 1].Text = ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 6 + 1].Text.Trim().Length == 0 ? GetBloodCnt(strVal, dt.Rows[i]["CAPACITY"].ToString().Trim()) : ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 6 + 1].Text.Trim();
                    
                    strStatus.Clear();

                    if (dt.Rows[i]["OUTRDATE"].ToString().Trim().Length > 0)
                    {
                        if (strStatus.Length > 0)
                        {
                            strStatus.AppendLine("");
                        }
                        if (clsType.User.BuseCode.Equals("078201"))
                        {
                            if(string.IsNullOrWhiteSpace(dt.Rows[i]["SDATE"].ToString().Trim()))
                            {
                                strStatus.Append("(반납)");
                            }
                        }
                        else
                        {
                            strStatus.Append("(반납)");
                        }
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
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 7 + 1].Text = ("(★임병)" + strStatus.ToString().Trim());
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 7 + 1].ForeColor = System.Drawing.Color.Red;
                        //ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 7].Font = new System.Drawing.Font("맑은고딕", 10, System.Drawing.FontStyle.Bold);
                    }

                    //strBloodPickUp.AppendLine("환자번호: " + p.ptNo);
                    //strBloodPickUp.AppendLine("성명: " + ss1567_Sheet1.Cells[0, 7].Text.Trim() + "/" + ss1567_Sheet1.Cells[0, 9].Text.Trim() + "/" + ss1567_Sheet1.Cells[0, 10].Text.Trim());
                    //strBloodPickUp.AppendLine("진료과/병동:" + ss1567_Sheet1.Cells[0, 11].Text.Trim());

                    //pstrBlood.TryGetValue(dt.Rows[i]["Component"].ToString().Trim(), out strVal);
                    //strBloodPickUp.AppendLine("혈액제제:" + strVal);
                    //strBloodPickUp.AppendLine("일자:" + dt.Rows[i]["OUTDATE"].ToString().Trim());
                    //strBloodPickUp.AppendLine("혈액번호:" + dt.Rows[i]["BLOODNO"].ToString().Trim());
                    //strBloodPickUp.AppendLine("검사자:" + clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["GUMSAJA"].ToString().Trim()));

                    //ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, nCol3].Text = dt.Rows[i]["COMPONENT"].ToString().Trim();
                    //ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, nCol4].Text = strBloodPickUp.ToString().Trim();

                    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 8 + 1].Text = string.IsNullOrWhiteSpace(dt.Rows[i]["EMRNO"].ToString().Trim()) ?  "" : "●";
                    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 9 + 1].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 10 + 1].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 11 + 1].Text = dt.Rows[i]["EDATE"].ToString().Trim();

                    ssList_Sheet1.Rows[ssList_Sheet1.RowCount - 1].Height = ssList_Sheet1.Rows[ssList_Sheet1.RowCount - 1].GetPreferredHeight() + 5;

                    if (clsType.User.BuseCode.Equals("078201")  && !strOldDate.Equals(Convert.ToDateTime(dt.Rows[i]["OUTDATE"].ToString().Trim()).ToString("yyyy-MM-dd")))
                    {
                        dblColorInfo = dblColorInfo == Color.White ? ComNum.SPSELCOLOR : Color.White;
                        strOldDate = Convert.ToDateTime(dt.Rows[i]["OUTDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                    }

                    ssList_Sheet1.Rows[ssList_Sheet1.RowCount - 1].BackColor = dblColorInfo;
                }

                //ss1965_Sheet1.Columns[7].Width = ss1965_Sheet1.Columns[7].GetPreferredWidth() + 5;

                dt.Dispose();
                #endregion

                #region 매칭취소내역
                //strStatus.Clear();
                //strBloodPickUp.Clear();

                //SQL = " SELECT '' AS OUTDATE, T.BLOODNO, '' AS PTAR, '' AS KORNAME, '' AS EXAMNAME, GBJOB ,  ";
                //SQL += ComNum.VBLF + "              '' AS JANDATE, '' AS DestroyDate, '' AS OUTRDATE, '' AS EMERGENCY, '' AS OUTBUSE, '' AS OUTRCAUSE, REMARK, B.ABO, T.COMPONENT, T.CAPACITY, E.EMRNO ";
                //#region 시작시간
                //SQL += ComNum.VBLF + "   ,(";
                //SQL += ComNum.VBLF + "   SELECT";
                //SQL += ComNum.VBLF + "   S.ITEMVALUE || ' ' || S2.ITEMVALUE";
                //SQL += ComNum.VBLF + "       FROM ADMIN.AEMRCHARTROW S";
                //SQL += ComNum.VBLF + "         INNER JOIN ADMIN.AEMRCHARTROW S2";
                //SQL += ComNum.VBLF + "            ON S2.EMRNO  = S.EMRNO";
                //SQL += ComNum.VBLF + "           AND S2.EMRNOHIS  = S.EMRNOHIS ";
                //SQL += ComNum.VBLF + "           AND S2.ITEMCD  = 'I0000037487'";
                //SQL += ComNum.VBLF + "       WHERE S.EMRNO = E.EMRNO";
                //SQL += ComNum.VBLF + "         AND S.EMRNOHIS > 0";
                //SQL += ComNum.VBLF + "         AND S.ITEMCD = 'I0000037486'";
                //SQL += ComNum.VBLF + "   )AS SDATE";
                //#endregion
                //#region 종료시간
                //SQL += ComNum.VBLF + "   ,(";
                //SQL += ComNum.VBLF + "   SELECT";
                //SQL += ComNum.VBLF + "   E1.ITEMVALUE || ' ' || E2.ITEMVALUE";
                //SQL += ComNum.VBLF + "       FROM ADMIN.AEMRCHARTROW E1";
                //SQL += ComNum.VBLF + "         INNER JOIN ADMIN.AEMRCHARTROW E2";
                //SQL += ComNum.VBLF + "            ON E2.EMRNO  = E1.EMRNO";
                //SQL += ComNum.VBLF + "           AND E2.EMRNOHIS  = E1.EMRNOHIS ";
                //SQL += ComNum.VBLF + "           AND E2.ITEMCD  = 'I0000037491'";
                //SQL += ComNum.VBLF + "       WHERE E1.EMRNO = E.EMRNO";
                //SQL += ComNum.VBLF + "         AND E1.EMRNOHIS > 0";
                //SQL += ComNum.VBLF + "         AND E1.ITEMCD = 'I0000037490'";
                //SQL += ComNum.VBLF + "   )AS EDATE";
                //#endregion
                //SQL += ComNum.VBLF + "  FROM ADMIN.EXAM_BLOODTRANS T";
                //SQL += ComNum.VBLF + "    INNER JOIN ADMIN.EXAM_BLOOD_IO B";
                //SQL += ComNum.VBLF + "       ON T.BLOODNO = B.BLOODNO ";
                //SQL += ComNum.VBLF + "     LEFT OUTER JOIN ADMIN.EMR_DATA_MAPPING E";
                //SQL += ComNum.VBLF + "       ON E.MAPPING1 = TRIM(T.BLOODNO) || TRIM(B.COMPONENT)";
                //SQL += ComNum.VBLF + "      AND E.FORMNO = 1965";
                //SQL += ComNum.VBLF + "  WHERE T.PANO = '" + p.ptNo + "'  ";
                //SQL += ComNum.VBLF + "    AND GBJOB IN ('5', '8') ";
                //SQL += ComNum.VBLF + "    AND BDATE >= TO_DATE('" + DateTime.ParseExact(p.medFrDate, "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD')";
                //SQL += ComNum.VBLF + "    AND BDATE <= TO_DATE('" + DateTime.ParseExact((p.medEndDate.Length == 0 ? strSysDate : p.medEndDate), "yyyyMMdd", null).ToShortDateString() + "', 'YYYY-MM-DD') ";

                //sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                //if (sqlErr.Length > 0)
                //{
                //    ComFunc.MsgBoxEx(this, sqlErr);
                //    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                //    return;
                //}

                //if (dt == null)
                //{
                //    return;
                //}

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{

                //    strStatus.Clear();

                //    ssList_Sheet1.RowCount += 1;

                //    if (dt.Rows[i]["OUTDATE"].ToString().Trim().Length > 0)
                //    {
                //        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].Text = Convert.ToDateTime(dt.Rows[i]["OUTDATE"].ToString().Trim()).ToShortDateString() + " " + Convert.ToDateTime(dt.Rows[i]["OUTDATE"].ToString().Trim()).ToString("HH:mm");
                //    }

                //    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BLOODNO"].ToString().Trim();

                //    switch (dt.Rows[i]["OUTBUSE"].ToString().Trim())
                //    {
                //        case "011123":
                //            strStatus.Append("응급실");
                //            break;
                //        case "033102":
                //            strStatus.Append("수술실");
                //            break;
                //    }

                //    if (dt.Rows[i]["EMERGENCY"].ToString().Equals("Y"))
                //    {
                //        if (strStatus.Length > 0)
                //        {
                //            strStatus.AppendLine("");
                //        }

                //        strStatus.Append("응급");
                //    }

                //    string strVal = string.Empty;
                //    pstrBlood.TryGetValue(dt.Rows[i]["PTAR"].ToString().Trim(), out strVal);

                //    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["PTAR"].ToString().Trim();
                //    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 3].Text = strVal;
                //    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["CAPACITY"].ToString().Trim();
                //    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["KORNAME"].ToString().Trim();

                //    pstrBlood.TryGetValue(dt.Rows[i]["Component"].ToString().Trim(), out strVal);
                //    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 6].Text = ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 6].Text.Trim().Length == 0 ? GetBloodCnt(strVal, dt.Rows[i]["CAPACITY"].ToString().Trim()) : ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 6].Text.Trim();

                //    if (dt.Rows[i]["OUTRDATE"].ToString().Trim().Length > 0)
                //    {
                //        if (strStatus.Length > 0)
                //        {
                //            strStatus.AppendLine("");
                //        }
                //        strStatus.Append("(반납)");
                //    }
                //    else if (dt.Rows[i]["JANDATE"].ToString().Trim().Length > 0)
                //    {
                //        if (strStatus.Length > 0)
                //        {
                //            strStatus.AppendLine("");
                //        }
                //        strStatus.Append("(잔량폐기)");
                //    }
                //    else if (dt.Rows[i]["DestroyDate"].ToString().Trim().Length > 0)
                //    {
                //        if (strStatus.Length > 0)
                //        {
                //            strStatus.AppendLine("");
                //        }
                //        strStatus.Append("(출고후폐기)");
                //    }

                //    //strComlText = dt.Rows[i]["ABO"].ToString().Trim() + ComNum.VBLF + strVal + dt.Rows[i]["CAPACITY"].ToString().Trim();

                //    //반납을 하면 반납사유를 표시해준다.
                //    if (dt.Rows[i]["CAPACITY"].ToString().Trim().Length > 0)
                //    {
                //        strStatus.AppendLine(dt.Rows[i]["OUTRCAUSE"].ToString().Trim());
                //    }

                //    switch (dt.Rows[i]["GBJOB"].ToString().Trim())
                //    {
                //        case "5":
                //            strStatus.AppendLine("출고반납");
                //            strStatus.AppendLine(dt.Rows[i]["REMARK"].ToString().Trim());
                //            break;
                //        case "7":
                //            strStatus.AppendLine("폐기");
                //            strStatus.AppendLine(dt.Rows[i]["REMARK"].ToString().Trim());
                //            break;
                //        case "8":
                //            strStatus.AppendLine("매칭취소");
                //            strStatus.AppendLine(dt.Rows[i]["REMARK"].ToString().Trim());
                //            break;
                //    }

                //    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 7].Text = "(★임병)" + strStatus.ToString().Trim();
                //    if (ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 6].Text.Length > 0)
                //    {
                //        ssList_Sheet1.Rows[ssList_Sheet1.RowCount - 1].ForeColor = System.Drawing.Color.Red;
                //    }

                //    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 8].Text = string.IsNullOrWhiteSpace(dt.Rows[i]["EMRNO"].ToString().Trim()) ? "" : "●";
                //    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                //    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["SDATE"].ToString().Trim();
                //    ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["EDATE"].ToString().Trim();
                //    ////ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, nCol3].Text = dt.Rows[i]["COMPONENT"].ToString().Trim();
                //    ////ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, nCol4].Text = strBloodPickUp.ToString().Trim();

                //    ssList_Sheet1.Rows[ssList_Sheet1.RowCount - 1].Height = ssList_Sheet1.Rows[ssList_Sheet1.RowCount - 1].GetPreferredHeight() + 5;
                //}

                //dt.Dispose();
                #endregion
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }
        #endregion

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (frmEmrChartNewX != null)
            {
                frmEmrChartNewX.Dispose();
                frmEmrChartNewX = null;
            }

            string BloodEndDate = ssList_Sheet1.Cells[e.Row, 11 + 1].Text;
            string BloodNo      = ssList_Sheet1.Cells[e.Row, 1 + 1].Text;

            if (clsType.User.BuseCode.Equals("078201") == false &&
                !string.IsNullOrWhiteSpace(BloodEndDate) &&
                ComFunc.MsgBoxQEx(this, "이미 수혈이 완료된 기록지 입니다 수정하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            string strEmrNo = ssList_Sheet1.Cells[e.Row, 9 + 1].Text.Trim();
            EmrForm pForm = GetFormInfo(strEmrNo);

            if (pForm == null)
            {
                pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, "1965", clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, 1965).ToString());
            }

            if (pForm.FmFORMNO == 1965)
            {
                frmEmrChartNewX = new frmEmrChartNew(pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), p, strEmrNo, "W", this);
                frmEmrChartNewX.Dock = DockStyle.Fill;
                frmEmrChartNewX.TopLevel = false;
                frmEmrChartNewX.Parent = splitContainer1.Panel2;
                frmEmrChartNewX.FormBorderStyle = FormBorderStyle.None;
                splitContainer1.Panel2.Controls.Add(frmEmrChartNewX);
                frmEmrChartNewX.Show();
                splitContainer1.SplitterDistance = 0;

                if (string.IsNullOrWhiteSpace(strEmrNo))
                {
                    SET_BLOOD_CHART(e.Row);
                }

                //혈액종류/양
                Control control = panChart.Controls.Find("I0000037484", true).FirstOrDefault();
                if (control != null)
                {
                    control.Tag = ssList_Sheet1.Cells[e.Row, 3 + 1].Tag;
                }
            }
            else
            {
                frmEmrChartNewX = new frmEmrChartNew(pForm.FmFORMNO.ToString(), pForm.FmUPDATENO.ToString(), p, strEmrNo, "W", this);
                frmEmrChartNewX.Dock = DockStyle.Fill;
                frmEmrChartNewX.TopLevel = false;
                frmEmrChartNewX.Parent = splitContainer1.Panel2;
                frmEmrChartNewX.FormBorderStyle = FormBorderStyle.None;
                splitContainer1.Panel2.Controls.Add(frmEmrChartNewX);
                frmEmrChartNewX.Show();
                splitContainer1.SplitterDistance = 0;

                if (string.IsNullOrWhiteSpace(strEmrNo))
                {
                    for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                    {
                        if (ssList_Sheet1.Cells[i, 0].Text.Trim().Equals("True") == false)
                            continue;

                        SET_BLOOD_CHART_PLTC(i);
                    }
                }
                else
                {
                    List<Control> controls = FormFunc.GetAllControls(frmEmrChartNewX).Where(d => d.Name.IndexOf("I0000009925") != -1 && d.Text.NotEmpty()).ToList();

                    foreach(Control control in controls)
                    {
                        for (int i = 0; i < ssList_Sheet1.RowCount; i++)
                        {
                            if (ssList_Sheet1.Cells[i, 1 + 1].Text.Equals(control.Text))
                            //혈액종류/양
                            if (control != null)
                            {
                                control.Tag = ssList_Sheet1.Cells[i, 3 + 1].Tag;
                            }
                        }
                    }
                    
                }
            }
        }

        private void btnSideBarDown_Click(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = 150;
        }

        private void btnSideBarUp_Click(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetBloodIO();
        }

        private void rdoGBN1_Click(object sender, EventArgs e)
        {
            if (sender.Equals(rdoGBN2))
            {
                ssList_Sheet1.Columns[0].Visible = true;
                btnSend.Visible = true;

                EmrForm pForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, "3535", clsEmrQuery.GetMaxNewUpdateNo(clsDB.DbCon, 3535).ToString());
                mstrFormNo = pForm.FmFORMNO.ToString();
                mstrUpdateNo = pForm.FmUPDATENO.ToString();
                chkAll.Visible = true;
            }
            else
            {
                ssList_Sheet1.Columns[0].Visible = false;
                btnSend.Visible = false;

                EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "1965");
                mstrFormNo = pForm.FmFORMNO.ToString();
                mstrUpdateNo = pForm.FmUPDATENO.ToString();
                chkAll.Visible = false;
            }

            GetBloodIO();
        }

        private EmrForm GetFormInfo(string EmrNo)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            EmrForm rtnVal = null;

            if (EmrNo.IsNullOrEmpty())
                return rtnVal;

            SQL = "SELECT FORMNO, UPDATENO";
            SQL += ComNum.VBLF + "FROM ADMIN.AEMRCHARTMST";
            SQL += ComNum.VBLF + "WHERE EMRNO = " + EmrNo;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return rtnVal;
            }


            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return null;
            }


            string FormNo = dt.Rows[0]["FORMNO"].ToString();
            string UpdateNo = dt.Rows[0]["UPDATENO"].ToString();

            dt.Dispose();

            return clsEmrChart.SerEmrFormInfo(clsDB.DbCon, FormNo, UpdateNo);
        }


        private void btnSend_Click(object sender, EventArgs e)
        {
            if (frmEmrChartNewX != null)
            {
                frmEmrChartNewX.Dispose();
                frmEmrChartNewX = null;
            }

            bool bWrite = false;
            for (int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                if (ssList_Sheet1.Cells[i, 0].Text.Trim().Equals("True") == false)
                    continue;

                bWrite = true;
                string BloodEndDate = ssList_Sheet1.Cells[i, 11 + 1].Text;

                if (clsType.User.BuseCode.Equals("078201") == false && !string.IsNullOrWhiteSpace(BloodEndDate) &&
                    ComFunc.MsgBoxQEx(this, "이미 수혈이 완료된 기록지 입니다 수정하시겠습니까?") == DialogResult.No)
                {
                    return;
                }
                else
                {
                    break;
                }

            }

            if (bWrite == false)
                return;

            frmEmrChartNewX = new frmEmrChartNew(mstrFormNo, mstrUpdateNo, p, "0", "W", this);
            frmEmrChartNewX.Dock = DockStyle.Fill;
            frmEmrChartNewX.TopLevel = false;
            frmEmrChartNewX.Parent = splitContainer1.Panel2;
            frmEmrChartNewX.FormBorderStyle = FormBorderStyle.None;
            splitContainer1.Panel2.Controls.Add(frmEmrChartNewX);
            frmEmrChartNewX.Show();
            splitContainer1.SplitterDistance = 0;

            int OneUnit = 0;
            int BloodVal = 0;
            Control control;

            for (int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                if (ssList_Sheet1.Cells[i, 0].Text.Trim().Equals("True") == false)
                    continue;

                string strEmrNo = ssList_Sheet1.Cells[i, 9 + 1].Text.Trim();

                //if (string.IsNullOrWhiteSpace(strEmrNo))
                //{
                    SET_BLOOD_CHART_PLTC(i);
                //}

                //혈액종류/양
                control = panChart.Controls.Find("I0000037484_" + (i + 1), true).FirstOrDefault();
                if (control != null)
                {
                    control.Tag = ssList_Sheet1.Cells[i, 3 + 1].Tag;
                }

                if (OneUnit == 0)
                {
                    OneUnit += ssList_Sheet1.Cells[i, 6 + 1].Text.To<int>();
                }

                BloodVal += ssList_Sheet1.Cells[i, 6 + 1].Text.To<int>();
            }

            //1 Unit 투여량
            control = panChart.Controls.Find("I0000037510", true).FirstOrDefault();
            if (control != null)
            {
                control.Text = OneUnit.ToString();
            }


            //총 투여량
            control = panChart.Controls.Find("I0000013528", true).FirstOrDefault();
            if (control != null)
            {
                control.Text = BloodVal.ToString();
            }

        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            ssList_Sheet1.Cells[0, 0, ssList_Sheet1.RowCount - 1, 0].Text = chkAll.Checked.ToString();
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (frmImgRmk != null)
            {
                frmImgRmk.Dispose();
                frmImgRmk = null;
            }

            object obj = Properties.Resources.ResourceManager.GetObject("1965");
            if (obj == null)
            {
                return;
            }

            frmImgRmk = new frmRemarkImage(obj);
            frmImgRmk.StartPosition = FormStartPosition.CenterScreen;
            frmImgRmk.FormClosed += FrmImgRmk_FormClosed;
            frmImgRmk.Show();
        }

        private void FrmImgRmk_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmImgRmk != null)
            {
                frmImgRmk.Dispose();
                frmImgRmk = null;
            }
        }
    }
}
