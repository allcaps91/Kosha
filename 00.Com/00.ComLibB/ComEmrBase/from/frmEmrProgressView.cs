using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ComEmrBase
{
    public partial class frmEmrProgressView : Form, EmrChartForm
    {
        #region 변수
        /// <summary>
        /// 부모폼
        /// </summary>
        Form prForm = null;
        /// <summary>
        /// 환자정보
        /// </summary>
        EmrPatient AcpEmr = null;
        /// <summary>
        /// 기록지 정보
        /// </summary>
        EmrForm pForm = null;
        /// <summary>
        /// 차트복사 시퀀스
        /// </summary>
        string CopyReqNo = string.Empty;
        /// <summary>
        /// 차트복사 과
        /// </summary>
        string mstrDeptCode = string.Empty;

        DateTimePicker Sdate = null;
        DateTimePicker Edate = null;
        #endregion

        #region EmrChartForm

        public double SaveDataMsg(string strFlag)
        {
            return 0;
        }

        public bool DelDataMsg()
        {
            return false;
        }

        public void ClearFormMsg()
        {
        }

        public void SetUserFormMsg(double dblMACRONO)
        {
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            return false;
        }

        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
        }

        public int PrintFormMsg(string strPRINTFLAG)
        {
            int rtnVal = pPrintForm();
            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            return 0;
        }
        #endregion

        #region public Print
        /// <summary>
        /// 기록지를 출력한다.
        /// </summary>
        public int pPrintForm()
        {
            int rtnVal = 0;

            if (CopyReqNo.Length == 0)
            {
                Screen screen = Screen.FromControl(prForm);

                using (frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption())
                {
                    frmEmrPrintOptionX.StartPosition = FormStartPosition.Manual;
                    frmEmrPrintOptionX.Location = new Point()
                    {
                        X = Math.Max(screen.WorkingArea.X, screen.WorkingArea.X + (screen.WorkingArea.Width - this.Width) / 2),
                        Y = Math.Max(screen.WorkingArea.Y, screen.WorkingArea.Y + (screen.WorkingArea.Height - this.Height) / 2)
                    };
                    frmEmrPrintOptionX.TopMost = true;
                    frmEmrPrintOptionX.ShowDialog(this);
                }

                if (clsFormPrint.mstrPRINTFLAG == "-1")
                {
                    return rtnVal;
                }
            }
            else
            {
                clsFormPrint.mstrPRINTFLAG = "0";
            }

            rtnVal = clsFormPrint.PrintRtf(clsDB.DbCon, "963", "1", AcpEmr, "0", panChart, txtProgress, "C");
            return rtnVal;
        }
        #endregion

        #region 생성자

        public frmEmrProgressView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 경과기록지 보는용도
        /// </summary>
        /// <param name="parentForm"></param>
        /// <param name="emrPatient"></param>
        /// <param name="emrForm"></param>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        public frmEmrProgressView(Form parentForm, EmrPatient emrPatient, EmrForm emrForm, DateTimePicker sDate, DateTimePicker eDate)
        {
            prForm = parentForm;
            AcpEmr = emrPatient;
            pForm = emrForm;
            Sdate = sDate;
            Edate = eDate;

            InitializeComponent();
        }

        /// <summary>
        /// 경과기록지 보는용도
        /// </summary>
        /// <param name="emrPatient">환자정보</param>
        /// <param name="emrForm">폼 정보</param>
        /// <param name="reqNo">복사신청 시퀀스</param>
        public frmEmrProgressView(EmrPatient emrPatient, EmrForm emrForm, string strDeptCode, string reqNo = "")
        {
            AcpEmr = emrPatient;
            pForm = emrForm;
            mstrDeptCode = strDeptCode;
            CopyReqNo = reqNo;

            InitializeComponent();
        }

        #endregion

        private void frmEmrProgressView_Load(object sender, EventArgs e)
        {
            txtProgress.Font = new Font("굴림", 9f);

            chkSortAs2.Checked = clsEmrPublic.bOrderAsc;

            if (Sdate != null)
            {
                dtpSSDATE = Sdate;
            }

            if (Edate != null)
            {
                dtpEEDATE = Edate;
            }

            if (string.IsNullOrWhiteSpace(CopyReqNo) == false && pForm != null || AcpEmr != null)
            {
                SetChartView();
            }
        }

        public void SetChartView()
        {
            bool mViewNpChart = clsEmrQueryOld.ViewNPChart(clsType.User.Sabun);

            StringBuilder SQL = new StringBuilder();    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            //차트복사
            if (!string.IsNullOrWhiteSpace(CopyReqNo))
            {
                SQL.AppendLine("SELECT C.EMRNO");
                SQL.AppendLine("     , C.CHARTDATE");
                SQL.AppendLine("     , C.CHARTTIME");
                if (pForm.FmOLDGB == 1)
                {
                    SQL.AppendLine("     , 'OLD' GBN");
                    SQL.AppendLine("     , EXTRACT(chartxml, '//ta1') AS ITEMVALUE");
                    SQL.AppendLine("     , '' AS ITEMVALUE1");
                    SQL.AppendLine("     , '' AS ITEMVALUE2");
                    SQL.AppendLine("     ,(SELECT NAME FROM ADMIN.EMR_USERT WHERE USERID = LTRIM(C.USEID, '0')) AS NAME");
                }
                else
                {
                    SQL.AppendLine("     , 'NEW' GBN");
                    SQL.AppendLine("     , C2.ITEMVALUE");
                    SQL.AppendLine("     , C2.ITEMVALUE1");
                    SQL.AppendLine("     , C2.ITEMVALUE2");
                    SQL.AppendLine("     ,(SELECT NAME FROM ADMIN.EMR_USERT WHERE USERID = C.CHARTUSEID) AS NAME");
                }
                if (pForm.FmOLDGB == 1)
                {
                    SQL.AppendLine("FROM ADMIN.EMRXML C ");
                }
                else
                {
                    SQL.AppendLine("FROM ADMIN.AEMRCHARTMST C ");
                    SQL.AppendLine("  INNER JOIN ADMIN.AEMRCHARTROW C2");
                    SQL.AppendLine("     ON C.EMRNO = C2.EMRNO");
                    SQL.AppendLine("    AND C.EMRNOHIS = C2.EMRNOHIS");
                    SQL.AppendLine("    AND C2.ITEMCD = 'I0000000981'");
                }
                SQL.AppendLine("  INNER JOIN ADMIN.EMRPRTREQ E");
                SQL.AppendLine("     ON C.EMRNO = E.EMRNO");
                SQL.AppendLine("    AND E.PRTREQNO = " + CopyReqNo);
                SQL.AppendLine("    AND E.SCANYN = 'T'");
                SQL.AppendLine("WHERE C.PTNO = '" + AcpEmr.ptNo + "' ");
                SQL.AppendLine("  AND C.FORMNO IN(963)");
                SQL.AppendLine("  AND C.MEDDEPTCD = '" + mstrDeptCode + "'");
            }
            else
            {
                SQL.AppendLine("--경과(재진/경과)");
                SQL.AppendLine("SELECT EMRNO");
                SQL.AppendLine("     , GBN");
                SQL.AppendLine("     , INOUTCLS");
                SQL.AppendLine("     , CHARTDATE");
                SQL.AppendLine("     , CHARTTIME");
                SQL.AppendLine("     , ITEMVALUE");
                SQL.AppendLine("     , ITEMVALUE1");
                SQL.AppendLine("     , ITEMVALUE2");
                SQL.AppendLine("     , NAME");
                SQL.AppendLine("  FROM ");
                SQL.AppendLine("( ");

                #region XML
                SQL.AppendLine("SELECT C.EMRNO");
                SQL.AppendLine("     , 'OLD' GBN");
                SQL.AppendLine("     , C.INOUTCLS");
                SQL.AppendLine("     , C.CHARTDATE");
                SQL.AppendLine("     , C.CHARTTIME");
                SQL.AppendLine("     , EXTRACT(C.CHARTXML, '//ta1').GETCLOBVAL() AS ITEMVALUE");
                SQL.AppendLine("     , '' AS ITEMVALUE1");
                SQL.AppendLine("     , '' AS ITEMVALUE2");
                SQL.AppendLine("     , (SELECT NAME FROM ADMIN.EMR_USERT WHERE USERID = LTRIM(C.USEID, '0')) AS NAME");
                SQL.AppendLine("  FROM ADMIN.EMRXML C ");
                SQL.AppendLine("    INNER JOIN ADMIN.BAS_CLINICDEPT D");
                SQL.AppendLine("       ON C.MEDDEPTCD = D.DEPTCODE");
                SQL.AppendLine(" WHERE C.PTNO       = '" + AcpEmr.ptNo + "' ");
                SQL.AppendLine("   AND C.FORMNO     IN(963) --경과기록지");

                if (AcpEmr.inOutCls == "O")
                {
                    if (AcpEmr.medDeptCd == "RA" || (AcpEmr.medDeptCd == "MD" && (AcpEmr.medDrCd == "1107" || AcpEmr.medDrCd == "1125")))
                    {
                        SQL.AppendLine("           AND C.MEDDEPTCD = 'MD'");
                        SQL.AppendLine("           AND C.MEDDRCD IN ('1107','1125')");
                    }
                    else
                    {
                        SQL.AppendLine("           AND C.MEDDEPTCD = '" + AcpEmr.medDeptCd + "'");
                        SQL.AppendLine("           AND C.MEDDRCD NOT IN ('1107','1125')");
                    }
                }
                else
                {
                    SQL.AppendLine("           AND C.MEDFRDATE = '" + AcpEmr.medFrDate + "' ");
                    if (AcpEmr.medDeptCd == "ER")
                    {
                        SQL.AppendLine("           AND C.INOUTCLS IN ('O') ");
                    }
                    else
                    {
                        SQL.AppendLine("           AND C.INOUTCLS IN ('I') ");
                    }
                }

                if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd == "HD")
                {
                    SQL.AppendLine("           AND C.INOUTCLS IN ('O','I') ");
                }
                else if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd != "HD")
                {
                    SQL.AppendLine("           AND C.INOUTCLS IN ('O') ");
                }

                if (mViewNpChart == false)
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD <> 'NP'");
                }

                SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSSDATE.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEEDATE.Value.ToString("yyyyMMdd") + "' ");
                #endregion

                #region 신규
                SQL.AppendLine("UNION ALL");
                SQL.AppendLine("SELECT C.EMRNO");
                SQL.AppendLine("     , 'NEW' GBN");
                SQL.AppendLine("     , C.INOUTCLS");
                SQL.AppendLine("     , C.CHARTDATE");
                SQL.AppendLine("     , C.CHARTTIME");
                SQL.AppendLine("     , TO_CLOB(C2.ITEMVALUE) AS ITEMVALUE");
                SQL.AppendLine("     , C2.ITEMVALUE1");
                SQL.AppendLine("     , C2.ITEMVALUE2");
                SQL.AppendLine("     , (SELECT NAME FROM ADMIN.EMR_USERT WHERE USERID = C.CHARTUSEID) AS NAME");
                SQL.AppendLine(" FROM ADMIN.AEMRCHARTMST C ");
                SQL.AppendLine("   INNER JOIN ADMIN.AEMRCHARTROW C2");
                SQL.AppendLine("      ON C.EMRNO = C2.EMRNO");
                SQL.AppendLine("     AND C.EMRNOHIS = C2.EMRNOHIS");
                SQL.AppendLine("     AND C2.ITEMCD IN ('I0000000981')");
                SQL.AppendLine("   INNER JOIN ADMIN.BAS_CLINICDEPT D");
                SQL.AppendLine("     ON C.MEDDEPTCD = D.DEPTCODE");
                SQL.AppendLine(" WHERE C.PTNO       = '" + AcpEmr.ptNo + "' ");
                SQL.AppendLine("   AND C.FORMNO     IN(963) --경과기록지");

                if (AcpEmr.inOutCls == "O")
                {
                    if (AcpEmr.medDeptCd == "RA" || (AcpEmr.medDeptCd == "MD" && (AcpEmr.medDrCd == "1107" || AcpEmr.medDrCd == "1125")))
                    {
                        SQL.AppendLine("           AND C.MEDDEPTCD = 'MD'");
                        SQL.AppendLine("           AND C.MEDDRCD IN ('1107','1125')");
                    }
                    else
                    {
                        SQL.AppendLine("           AND C.MEDDEPTCD = '" + AcpEmr.medDeptCd + "'");
                        SQL.AppendLine("           AND C.MEDDRCD NOT IN ('1107','1125')");
                    }
                }
                else
                {
                    SQL.AppendLine("           AND C.MEDFRDATE = '" + AcpEmr.medFrDate + "' ");
                    if (AcpEmr.medDeptCd == "ER")
                    {
                        SQL.AppendLine("           AND C.INOUTCLS IN ('O') ");
                    }
                    else
                    {
                        SQL.AppendLine("           AND C.INOUTCLS IN ('I') ");
                    }
                }

                if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd == "HD")
                {
                    SQL.AppendLine("           AND C.INOUTCLS IN ('O','I') ");
                }
                else if (AcpEmr.inOutCls == "O" && AcpEmr.medDeptCd != "HD")
                {
                    SQL.AppendLine("           AND C.INOUTCLS IN ('O') ");
                }

                if (mViewNpChart == false)
                {
                    SQL.AppendLine("           AND C.MEDDEPTCD <> 'NP'");
                }

                SQL.AppendLine("           AND C.CHARTDATE >= '" + dtpSSDATE.Value.ToString("yyyyMMdd") + "' ");
                SQL.AppendLine("           AND C.CHARTDATE <= '" + dtpEEDATE.Value.ToString("yyyyMMdd") + "' ");
                #endregion
                SQL.AppendLine(") ");
            }

            if (chkSortAs2.Checked)
            {
                SQL.AppendLine("ORDER BY (CHARTDATE || CHARTTIME), INOUTCLS ASC");
            }
            else
            {
                SQL.AppendLine("ORDER BY (CHARTDATE || CHARTTIME) DESC, INOUTCLS ASC");
            }


            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string strValue = string.Concat(dt.Rows[i]["ITEMVALUE"].ToString().Trim(), dt.Rows[i]["ITEMVALUE1"].ToString().Trim(), dt.Rows[i]["ITEMVALUE2"].ToString().Trim());

                    RichTxtBold("작성일시: " + VB.Val(dt.Rows[i]["CHARTDATE"].ToString().Trim()).ToString("0000-00-00") + " " + VB.Val(VB.Left(dt.Rows[i]["CHARTTIME"].ToString().Trim(), 4)).ToString("00:00") + "  작성자: " + dt.Rows[i]["NAME"].ToString().Trim());
                    if (pForm.FmOLDGB == 1 || dt.Rows[i]["GBN"].ToString().Trim().Equals("OLD"))
                    {
                        RichTextAdd(GetContent(strValue).Replace(ComNum.VBLF, "\n"));
                    }
                    else
                    {
                        RichTextAdd(strValue.Replace(ComNum.VBLF, "\n"));
                    }
                }
            }

            dt.Dispose();
        }

        #region 리치텍스트박스 관련 함수
        /// <summary>
        /// 리치텍스트 볼드
        /// </summary>
        /// <param name="strTitle"></param>
        /// <param name="FontName"></param>
        private void RichTxtBold(string strTitle, string FontName = "굴림체", float fontSize = 11, bool LineHeader = true, bool LineSpace = true)
        {
            using (Font bFont = new Font(FontName, fontSize, FontStyle.Bold))
            {
                int intStart = txtProgress.TextLength;

                if (LineHeader)
                {
                    txtProgress.AppendText(new string(' ', 30));
                    txtProgress.Select(intStart, 30);
                    using (Font font = new Font("굴림체", 2))
                    {
                        txtProgress.SelectionFont = font;
                    }
                    txtProgress.AppendText("\n");
                }

                intStart = txtProgress.TextLength;
                txtProgress.AppendText(strTitle);
                txtProgress.Select(intStart, strTitle.Length + 2);
                txtProgress.SelectionFont = bFont;
               
                if (LineSpace)
                {
                    txtProgress.Select(intStart > 1 ? intStart - 1 : 0, strTitle.Length + 2);
                    txtProgress.SelectionCharOffset = 4;
                    txtProgress.SelectionLength = 0;
                }

                txtProgress.SelectionLength = 0;
                txtProgress.AppendText("\n");
            }
        }

        /// <summary>
        /// 리치텍스트 추가
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="LineSpace"></param>
        private void RichTextAdd(string Content, string FontName = "굴림체", float FontSize = 11, bool changeFont = false)
        {
            int Start = txtProgress.TextLength;
            txtProgress.AppendText(Content);
            txtProgress.AppendText("\n");
            txtProgress.AppendText("----------------------------------------------------------------------------------------");
            if (changeFont)
            {
                txtProgress.Select(Start, Content.Length + 1);
                using (Font font = new Font(FontName, FontSize))
                {
                    txtProgress.SelectionFont = font;
                }
                txtProgress.SelectionLength = 0;
            }
        }

        private string GetContent(string strXml)
        {
            if (string.IsNullOrWhiteSpace(strXml))
                return string.Empty;

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(strXml);
            return xml.InnerText;
        }

        #endregion

        private void btnPrint_Click(object sender, EventArgs e)
        {
            pPrintForm();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SetChartView();
        }
    }
}
