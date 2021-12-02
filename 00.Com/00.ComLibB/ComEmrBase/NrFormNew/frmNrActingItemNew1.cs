using ComBase;
using FarPoint.Win.Spread;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmNrActingItemNew1 : Form, FormEmrMessage
    {
        #region 
        private struct LastData
        {
            public string ITEMCD;
            public string EMRNO;
            public string EMRNOHIS;
        }
        #endregion

        #region //변수 선언
        ////폼이 Close될 경우
        //public delegate void EventClosed(double dblEMRNO);
        //public event EventClosed rEventClosed;

        /// <summary>
        /// 완료 여부
        /// </summary>
        /// <param name="Save"></param>
        public delegate void EventSendave(bool Save);
        public event EventSendave rEventSendave;

        private string mstrITEMTIME = string.Empty;
        private string mChartDate = string.Empty;
        private EmrPatient AcpEmr = null;

        private List<string> strItem = null;


        private string mstrFormNo = "1575";
        private string mstrUpdateNo = "2";

        private double mEMRNO = 0;
        private double mEMRNOHIS = 0;
        private string mEmrUseId = string.Empty;
        private string mstrITEMCD = string.Empty;

        System.Drawing.Color mSelColor = System.Drawing.Color.LightCyan;
        System.Drawing.Color mDeSelColor = System.Drawing.Color.White;

        /// <summary>
        /// 삽입 번들용
        /// </summary>
        frmEmrChartNew frmEmrChartNewX = null;

        /// <summary>
        /// 수가 약속처방 저장
        /// </summary>
        frmSugaOrderSave frmSugaOrderSaveX = null;

        ContextMenu PopupMenu = null;

        #endregion //변수 선언

        #region //FormEmrMessage
        public void MsgSave(string strSaveFlag)
        {
            if (frmEmrChartNewX != null)
            {
                frmEmrChartNewX.Dispose();
                frmEmrChartNewX = null;
            }

            if (SaveUserChart() && rEventSendave != null)
            {
                if (frmSugaOrderSaveX != null && frmSugaOrderSaveX.ss2.ActiveSheet.NonEmptyRowCount > 0)
                {
                    clsEmrQuery.SaveOrderData(this, frmSugaOrderSaveX.ss2, AcpEmr, dtpChartDate.Value.ToString("yyyy-MM-dd"), mEMRNO);
                }
                rEventSendave(true);
                Close();
            }
        }
        public void MsgDelete()
        {


        }
        public void MsgClear()
        {
        }
        public void MsgPrint()
        {

        }
        #endregion

        #region //생성자
        public frmNrActingItemNew1()
        {
            InitializeComponent();
        }

        public frmNrActingItemNew1(EmrPatient pAcp, string strActDate, string strITEMTIME, string strFormNo, string strUpdateNo)
        {
            InitializeComponent();

            AcpEmr = pAcp;
            mChartDate = strActDate;
            mstrITEMTIME = strITEMTIME;

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
        }

        public frmNrActingItemNew1(EmrPatient pAcp, string strActDate, string strITEMTIME, string strFormNo, string strUpdateNo, string strITEMCD)
        {
            InitializeComponent();

            AcpEmr = pAcp;
            mChartDate = strActDate;
            mstrITEMTIME = strITEMTIME;

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;

            mstrITEMCD = strITEMCD;
        }

        private void frmNrActingItemNew1_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = clsType.User.AuAWRITE.Equals("1");
            btnDelete.Enabled = clsType.User.AuAWRITE.Equals("1");

            PopupMenu = new ContextMenu();

            dtpChartDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(mChartDate, "D"));
            txtChartTime.Text = mstrITEMTIME;

            mbtnPTOrder.Visible = FormPatInfoFunc.Set_FormPatInfo_PTOrder(clsDB.DbCon, AcpEmr, dtpChartDate.Value.ToString("yyyy-MM-dd"));

            strItem = new List<string>();
            GetEmrNo();

            //if (mEMRNO == 0)
            //{
            //    dtpChartDate.Enabled = true;
            //    txtChartTime.Enabled = true;
            //}
            //else
            //{
            //    dtpChartDate.Enabled = false;
            //    txtChartTime.Enabled = false;
            //}

            dtpChartDate.Enabled = false;
            txtChartTime.Enabled = false;

            GetData(dtpChartDate.Value.ToString("yyyyMMdd"));
            GetActData();

            if (mEMRNO == 0)
            {
                GetLastData("ALL");
            }
            GetDietOrder();
          
            #region
            //if (clsType.User.IdNumber.Equals("8822"))
            //{
                frmSugaOrderSaveX = new frmSugaOrderSave(dtpChartDate.Value.ToString("yyyy-MM-dd"), mstrFormNo, "", "", "", AcpEmr, mEMRNO.ToString(), ssWrite, -1);
                frmSugaOrderSaveX.ControlBox = false;
                frmSugaOrderSaveX.TopLevel = false;
                frmSugaOrderSaveX.Parent = panSuga;
                frmSugaOrderSaveX.FormBorderStyle = FormBorderStyle.None;
                frmSugaOrderSaveX.Top = 0;
                frmSugaOrderSaveX.Left = 0;
                frmSugaOrderSaveX.Dock = DockStyle.Fill;
                panSuga.Controls.Add(frmSugaOrderSaveX);
                frmSugaOrderSaveX.Show();
                frmSugaOrderSaveX.BringToFront();
                frmSugaOrderSaveX.GetOrderData(mEMRNO.ToString());
            //}
            #endregion
        }

        #endregion //생성자

        #region //컨트롤 이벤트

        /// <summary>
        /// 팦업 메뉴 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubMenuAdd_Click(object sender, EventArgs e) // 이벤트 헨들러
        {
            ssWrite.ContextMenu = null;

            string strJob = ((MenuItem)sender).Text.Trim();
            string strItemCd = ((MenuItem)sender).Tag.ToString().Trim();
        }

        private void mbtnSearchAll_Click(object sender, EventArgs e)
        {
            GetData(dtpChartDate.Value.ToString("yyyyMMdd"));
            GetActData();
            GetDietOrder();

            if (frmSugaOrderSaveX != null)
            {
                frmSugaOrderSaveX.GetOrderData(mEMRNO.ToString());
            }
        }

        private void ssChart_ComboCloseUp(object sender, EditorNotifyEventArgs e)
        {
            FarPoint.Win.FpCombo fpCombo = (FarPoint.Win.FpCombo)e.EditingControl;
            fpCombo.EditModeCursorPosition = FarPoint.Win.EditModeCursorPosition.FirstInputPosition;
            fpCombo.SelectionStart = 0;
            fpCombo.SelectionLength = 0;

            if (e.Column != 5) return;

            string strITEMCD = ssWrite_Sheet1.Cells[e.Row, 1].Text.Trim();
            string strITEMVALUE = ssWrite_Sheet1.Cells[e.Row, 5].Text.Trim();

            Get_VentilatorUseDay(e.Row);

            SetDefaultValue1(e.Row, strITEMCD, strITEMVALUE);

            Set_ValueOrder(e.Row, strITEMCD, strITEMVALUE);
        }

        private void Get_VentilatorUseDay(int Row)
        {
            string strITEMCD = ssWrite_Sheet1.Cells[Row, 1].Text.Trim();
            string strITEMVALUE = ssWrite_Sheet1.Cells[Row, 5].Text.Trim();

            if (strITEMCD.Equals("I0000037521") && strITEMVALUE.Equals("유지"))
            {
                string strInsertDay = string.Empty;
                string strMaintaintDay = string.Empty;
                string strSize = string.Empty;

                FormPatInfoFunc.Set_FormPatInfo_VentilatorUseDay(clsDB.DbCon, AcpEmr, ref strInsertDay, ref strMaintaintDay, ref strSize);

                if (string.IsNullOrWhiteSpace(strMaintaintDay))
                {
                    ComFunc.MsgBoxEx(this, "이미 제거된 도뇨관 입니다\r\n다시 확인해주세요.");
                }
                else
                {
                    ssWrite_Sheet1.Cells[Row, 6].Text = strMaintaintDay + "일";
                }

                return;
            }
        }

        private void Set_ValueOrder(int Row, string strITEMCD, string strITEMVALUE)
        {
            string ItemNm = ssWrite_Sheet1.Cells[Row, 3].Text;

            if (string.IsNullOrWhiteSpace(strITEMCD) == false)
            {
                if (Width < 1256)
                {
                    this.Width = 1256;
                }

                if (frmSugaOrderSaveX != null)
                {
                    frmSugaOrderSaveX.SET_BAT(AcpEmr.ward, mstrFormNo, ItemNm, strITEMCD, strITEMVALUE, Row);
                    return;
                }
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == true)
            {
                clsEmrFunc.NowEmrCert(clsDB.DbCon, AcpEmr.medFrDate, AcpEmr.ptNo);
                GetData(dtpChartDate.Value.ToString("yyyyMMdd"));
                GetActData();
            }
        }

        private void FrmEmrChartNewX_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrChartNewX != null)
            {
                frmEmrChartNewX.Dispose();
                frmEmrChartNewX = null;
            }
        }
        #endregion //컨트롤 이벤트

        #region //함수


        private void GetDietOrder()
        {
            if (AcpEmr == null)
                return;

            string strChartTime = txtChartTime.Text.Trim().Replace(":", "");
            string strChartDate = dtpChartDate.Value.ToShortDateString();
            string strChartDate2 = dtpChartDate.Value.AddDays(-1).ToShortDateString();

            string strSql = " SELECT D.DIETDAY, D.BUN,  D.DIETNAME, D.QTY ";
            strSql = strSql + ComNum.VBLF + "   FROM KOSMOS_PMPA.DIET_NEWORDER D ";
            strSql = strSql + ComNum.VBLF + "WHERE D.ACTDATE = TO_DATE('" + strChartDate + "', 'YYYY-MM-DD') ";
            strSql = strSql + ComNum.VBLF + "     AND D.PANO = '" + AcpEmr.ptNo + "' ";
            strSql = strSql + ComNum.VBLF + "     AND D.BUN IN ('01','02','03','04') ";

            DataTable dt = null;
            string SqlErr = clsDB.GetDataTableREx(ref dt, strSql, clsDB.DbCon);
            if (SqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(SqlErr, strSql, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }

           

            try
            {
                string strDiet1 = string.Empty;
                string strDiet2 = string.Empty;
                string strDiet3 = string.Empty;

                string strDiet1Etc = string.Empty;
                string strDiet2Etc = string.Empty;
                string strDiet3Etc = string.Empty;

                double strDiet1Qty = 0;
                double strDiet2Qty = 0;
                double strDiet3Qty = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["BUN"].ToString().Trim() != "04")
                    {
                        switch (dt.Rows[i]["DIETDAY"].ToString().Trim())
                        {
                            case "1":
                                if (!string.IsNullOrWhiteSpace(strDiet1))
                                {
                                    strDiet1 += ComNum.VBLF + dt.Rows[i]["DIETNAME"].ToString().Trim();
                                }
                                else
                                {
                                    strDiet1 = dt.Rows[i]["DIETNAME"].ToString().Trim();
                                    strDiet1Qty = VB.Val(dt.Rows[i]["QTY"].ToString().Trim());
                                }
                                break;
                            case "2":
                                if (!string.IsNullOrWhiteSpace(strDiet2))
                                {
                                    strDiet2 += ComNum.VBLF + dt.Rows[i]["DIETNAME"].ToString().Trim();
                                }
                                else
                                {
                                    strDiet2 = dt.Rows[i]["DIETNAME"].ToString().Trim();
                                    strDiet2Qty = VB.Val(dt.Rows[i]["QTY"].ToString().Trim());
                                }
                                break;
                            case "3":
                                if (!string.IsNullOrWhiteSpace(strDiet3))
                                {
                                    strDiet3 += ComNum.VBLF + dt.Rows[i]["DIETNAME"].ToString().Trim();
                                }
                                else
                                {
                                    strDiet3 = dt.Rows[i]["DIETNAME"].ToString().Trim();
                                    strDiet3Qty = VB.Val(dt.Rows[i]["QTY"].ToString().Trim());
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (dt.Rows[i]["DIETDAY"].ToString().Trim())
                        {
                            case "1":
                                strDiet1Etc = strDiet1 + dt.Rows[i]["DIETNAME"].ToString().Trim();
                                break;
                            case "2":
                                strDiet2Etc = strDiet2 + dt.Rows[i]["DIETNAME"].ToString().Trim();
                                break;
                            case "3":
                                strDiet3Etc = strDiet3 + dt.Rows[i]["DIETNAME"].ToString().Trim();
                                break;
                        }
                    }
                }

                dt.Dispose();

                string strVal = string.Empty;
                string strVal2 = string.Empty;
                if (VB.Val(strChartTime) >= 0700 && VB.Val(strChartTime) <= 1500)
                {
                    if (!string.IsNullOrWhiteSpace(strDiet1))
                    {
                        strVal = "아침 - " + strDiet1 + (strDiet1Qty > 100 ? "(" + strDiet1Qty + ")" : "");
                    }

                    if (!string.IsNullOrWhiteSpace(strDiet2))
                    {
                        if (!string.IsNullOrWhiteSpace(strDiet1))
                        {
                            strVal += ComNum.VBLF;
                        }

                        strVal += "점심 - " + strDiet2 + (strDiet2Qty > 100 ? "(" + strDiet2Qty + ")" : "");

                        strVal2 = (strDiet1Etc.Length != 0 ? "아)" + strDiet1Etc : "");
                        strVal2 += ComNum.VBLF + (strDiet2Etc.Length != 0 ? "점)" + strDiet2Etc : "");
                    }
                }
                else if (VB.Val(strChartTime) >= 1501 && VB.Val(strChartTime) <= 2300)
                {
                    strVal = "저녁 - " + strDiet3 + (strDiet3Qty > 100 ? "(" + strDiet3Qty + ")" : "");

                    if (VB.Val(strChartTime) >= 2100 && strDiet3.IndexOf("#4") != -1)
                    {
                        strVal += ComNum.VBLF + "야식 - " + strDiet3 + (strDiet3Qty > 100 ? "(" + strDiet3Qty + ")" : "");
                    }

                    strVal2 = (strDiet3Etc.Length != 0 ? "저)" + strDiet3Etc : "");
                }

                if ((AcpEmr.ward.Equals("33") || AcpEmr.ward.Equals("35")) && VB.Val(strChartTime) <= 0500)
                {
                    #region 5시 이전에 ICU에서 차팅 할경우
                    strSql = " SELECT D.DIETDAY, D.BUN,  D.DIETNAME, D.QTY ";
                    strSql = strSql + ComNum.VBLF + "   FROM KOSMOS_PMPA.DIET_NEWORDER D ";
                    strSql = strSql + ComNum.VBLF + "WHERE D.ACTDATE = TO_DATE('" + strChartDate2 + "', 'YYYY-MM-DD') ";
                    strSql = strSql + ComNum.VBLF + "     AND D.PANO = '" + AcpEmr.ptNo + "' ";
                    strSql = strSql + ComNum.VBLF + "     AND D.BUN IN ('01','02','03','04') ";
                    strSql = strSql + ComNum.VBLF + "     AND DIETDAY = '3'";
                    strSql = strSql + ComNum.VBLF + "     AND DIETNAME LIKE '%#4%'";

                    SqlErr = clsDB.GetDataTableREx(ref dt, strSql, clsDB.DbCon);
                    if (SqlErr.Length > 0)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, strSql, clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strDiet3 = dt.Rows[0]["DIETNAME"].ToString().Trim();
                        strDiet3Qty = VB.Val(dt.Rows[0]["QTY"].ToString().Trim());
                        mbtnIcuDiet.Visible = true;
                        mbtnIcuDiet.Text = strChartDate2 + "식이";
                        mbtnIcuDiet.Tag = "야식 - " + strDiet3 + (strDiet3Qty > 100 ? "(" + strDiet3Qty + ")" : "");
                    }

                    dt.Dispose();
                    #endregion
                }


                for (int i = 0; i < ssWrite_Sheet1.RowCount; i++)
                {
                    if (string.IsNullOrWhiteSpace(ssWrite_Sheet1.Cells[i, 5].Text))
                    {
                        if (ssWrite_Sheet1.Cells[i, 1].Text.Equals("I0000001465")) //식이
                        {
                            ssWrite_Sheet1.Cells[i, 5].Text = strVal;
                            if (ssWrite_Sheet1.Rows[i].GetPreferredHeight() > ssWrite_Sheet1.Rows[i].Height)
                            {
                                ssWrite_Sheet1.Rows[i].Height = ssWrite_Sheet1.Rows[i].GetPreferredHeight() + 16;
                            }
                        }
                        else if (ssWrite_Sheet1.Cells[i, 1].Text.Equals("I0000037841")) //간식
                        {
                            ssWrite_Sheet1.Cells[i, 5].Text = strVal2;
                            if (ssWrite_Sheet1.Rows[i].GetPreferredHeight() > ssWrite_Sheet1.Rows[i].Height)
                            {
                                ssWrite_Sheet1.Rows[i].Height = ssWrite_Sheet1.Rows[i].GetPreferredHeight() + 16;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, strSql, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }


        /// <summary>
        /// 차트작성 내역 조회
        /// </summary>
        private void GetActData()
        {

            int i = 0;

            strItem.Clear();
            if (ssWrite_Sheet1.RowCount == 0) return;

            //ss2_Sheet1.ColumnHeader.Cells[0, 0].Text = "□";

            for (i = 0; i < ssWrite_Sheet1.RowCount; i++)
            {
                string strITEMCD = ssWrite_Sheet1.Cells[i, 1].Text.Trim();
                string strITEMNM = ssWrite_Sheet1.Cells[i, 3].Text.Trim();
                strItem.Add("'" + strITEMCD + "'");
                GetActDataSub(i, strITEMCD);
            }
        }

        /// <summary>
        /// 차트작성 내역 조회 및 값 세팅
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="strITEMCD"></param>
        /// <param name="strBEFOREEMRNO">가장 최근 EMRNO</param>
        private void GetActDataSub(int Row, string strITEMCD, string strBEFOREEMRNO = "", string strBEFOREEMRNOHIS = "")
        {
            //식이 안가져옴.
            if (string.IsNullOrWhiteSpace(strBEFOREEMRNO) && mEMRNO.Equals("0") ||
                strITEMCD.Equals("I0000001465") || strITEMCD.Equals("I0000037841"))
                return;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "   ITEMVALUE, ITEMVALUE1 ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTROW ";
            if (string.IsNullOrWhiteSpace(strBEFOREEMRNO))
            {
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + mEMRNO;
                SQL = SQL + ComNum.VBLF + "  AND EMRNOHIS = " + mEMRNOHIS;
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + strBEFOREEMRNO;
                SQL = SQL + ComNum.VBLF + "  AND EMRNOHIS = " + strBEFOREEMRNOHIS;
            }

            SQL = SQL + ComNum.VBLF + "  AND ITEMCD = '" + strITEMCD + "'";

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
                return;
            }

            //clsSpread.gSpreadComboListFindEx(ssChart, Row, 5, dt.Rows[0]["ITEMVALUE"].ToString().Trim());
            //clsSpread.gSpreadComboListFindEx(ssChart, Row, 6, dt.Rows[0]["ITEMVALUE1"].ToString().Trim());

            if (string.IsNullOrWhiteSpace(strBEFOREEMRNO))
            {
                ssWrite_Sheet1.Cells[Row, 3, Row, 4].Font = new Font("맑은 고딕", 9.7f, FontStyle.Bold);
            }

            if (ssWrite_Sheet1.Cells[Row, 5].Text.Trim() == "연동" || ssWrite_Sheet1.Cells[Row, 5].Text.Trim() == "설정무")
            {
                ssWrite_Sheet1.Cells[Row, 5].Text = dt.Rows[0]["ITEMVALUE"].ToString().Trim() + dt.Rows[0]["ITEMVALUE1"].ToString().Trim();
            }
            else
            {
                ssWrite_Sheet1.Cells[Row, 5].Text = dt.Rows[0]["ITEMVALUE"].ToString().Trim();
                SetDefaultValue1(Row, strITEMCD, ssWrite_Sheet1.Cells[Row, 5].Text.Trim());
                ssWrite_Sheet1.Cells[Row, 6].Text = dt.Rows[0]["ITEMVALUE1"].ToString().Trim();
            }


            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 작성일자, 시간으로 EMRNO 조회
        /// </summary>
        private void GetEmrNo()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     EMRNO, EMRNOHIS, CHARTUSEID ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
            SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "     AND CHARTDATE = '" + mChartDate + "'";
            SQL = SQL + ComNum.VBLF + "     AND CHARTTIME = '" + txtChartTime.Text.Replace(":", "") + "00" + "'";
            SQL = SQL + ComNum.VBLF + "     AND FORMNO = " + mstrFormNo;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count != 0)
            {
                mEMRNO = VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim());
                mEMRNOHIS = VB.Val(dt.Rows[0]["EMRNOHIS"].ToString().Trim());
                mEmrUseId = dt.Rows[0]["CHARTUSEID"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

       

        /// <summary>
        /// 조회
        /// </summary>
        /// <param name="strChartDate"></param>
        private void GetData(string strChartDate)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssWrite_Sheet1.RowCount = 0;

            string strChartTime = txtChartTime.Text.Replace(":", "") + "00";

            #region //Old Query
            //if (mEMRNO != 0)
            //{
            //    SQL = "SELECT ";
            //    SQL = SQL + ComNum.VBLF + "    A.ACPNO, A.ITEMCD, A.CHARTDATE,  A.ACTINTERVAL, A.ACTINTERVALCD, A.ACTCNT, ";
            //    SQL = SQL + ComNum.VBLF + "    B.BASNAME AS ITEMNAME, B.VFLAG1, ";
            //    SQL = SQL + ComNum.VBLF + "    BB.BASNAME AS GRPNAME ";
            //    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";
            //    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            //    SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            //    SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            //    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
            //    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            //    SQL = SQL + ComNum.VBLF + "    ON B.VFLAG1 = BB.BASCD";
            //    SQL = SQL + ComNum.VBLF + "    AND BB.BSNSCLS = '기록지관리'";
            //    SQL = SQL + ComNum.VBLF + "    AND BB.UNITCLS = '간호활동그룹'";
            //    SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
            //    SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (";
            //    SQL = SQL + ComNum.VBLF + "                                SELECT MAX(A1.CHARTDATE) AS CHARTDATE FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A1";
            //    SQL = SQL + ComNum.VBLF + "                                    WHERE A1.ACPNO = " + AcpEmr.acpNo;
            //    SQL = SQL + ComNum.VBLF + "                                        AND A1.CHARTDATE <= '" + strChartDate + "')";
            //    SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD IN ( ";
            //    SQL = SQL + ComNum.VBLF + "                     SELECT R.ITEMCD ";
            //    SQL = SQL + ComNum.VBLF + "                     FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
            //    SQL = SQL + ComNum.VBLF + "                     INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
            //    SQL = SQL + ComNum.VBLF + "                         ON C.EMRNO = R.EMRNO ";
            //    SQL = SQL + ComNum.VBLF + "                         AND C.EMRNOHIS = R.EMRNOHIS ";
            //    SQL = SQL + ComNum.VBLF + "                     WHERE C.EMRNO = " + mEMRNO;
            //    //SQL = SQL + ComNum.VBLF + "                     WHERE C.ACPNO = " + AcpEmr.acpNo;
            //    //SQL = SQL + ComNum.VBLF + "                         AND C.CHARTDATE = '" + strChartDate + "'";
            //    //SQL = SQL + ComNum.VBLF + "                         AND C.CHARTTIME = '" + strChartTime + "'";
            //    SQL = SQL + ComNum.VBLF + "                     ) ";
            //    SQL = SQL + ComNum.VBLF + "ORDER BY B.VFLAG1, B.NFLAG1, B.NFLAG2, B.NFLAG3";
            //}
            //else
            //{
            //    SQL = "SELECT ";
            //    SQL = SQL + ComNum.VBLF + "    A.ACPNO, A.ITEMCD, A.CHARTDATE,  A.ACTINTERVAL, A.ACTINTERVALCD, A.ACTCNT, ";
            //    SQL = SQL + ComNum.VBLF + "    B.BASNAME AS ITEMNAME, B.VFLAG1, ";
            //    SQL = SQL + ComNum.VBLF + "    BB.BASNAME AS GRPNAME ";
            //    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";

            //    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            //    SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            //    SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            //    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
            //    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            //    SQL = SQL + ComNum.VBLF + "    ON B.VFLAG1 = BB.BASCD";
            //    SQL = SQL + ComNum.VBLF + "    AND BB.BSNSCLS = '기록지관리'";
            //    SQL = SQL + ComNum.VBLF + "    AND BB.UNITCLS = '간호활동그룹'";
            //    SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
            //    SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (";
            //    SQL = SQL + ComNum.VBLF + "                                SELECT MAX(A1.CHARTDATE) AS CHARTDATE FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A1";
            //    SQL = SQL + ComNum.VBLF + "                                    WHERE A1.ACPNO = " + AcpEmr.acpNo;
            //    SQL = SQL + ComNum.VBLF + "                                        AND A1.CHARTDATE <= '" + strChartDate + "')";
            //    if (mstrITEMCD != "")
            //    {
            //        string strITEMCD = "";
            //        string[] arryITEMCD = VB.Split(mstrITEMCD, ",");
            //        for (i = 0; i < arryITEMCD.Length; i++)
            //        {
            //            if (i == arryITEMCD.Length - 1)
            //            {
            //                strITEMCD = strITEMCD + "'" + arryITEMCD[i] + "'";
            //            }
            //            else
            //            {
            //                strITEMCD = strITEMCD + "'" + arryITEMCD[i] + "', ";
            //            }
            //        }
            //        SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD IN (" + strITEMCD + ")";
            //    }
            //    SQL = SQL + ComNum.VBLF + "ORDER BY B.VFLAG1, B.NFLAG1, B.NFLAG2, B.NFLAG3";
            //}
            #endregion //Old Query

            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "    A.ACPNO, A.ITEMCD, A.CHARTDATE,  A.ACTINTERVAL, A.ACTINTERVALCD, A.ACTCNT, ";
            SQL = SQL + ComNum.VBLF + "    B.BASNAME AS ITEMNAME, B.VFLAG1, ";
            SQL = SQL + ComNum.VBLF + "    BB.BASNAME AS GRPNAME ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A";

            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '간호활동항목'";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD BB";
            SQL = SQL + ComNum.VBLF + "    ON B.VFLAG1 = BB.BASCD";
            SQL = SQL + ComNum.VBLF + "    AND BB.BSNSCLS = '기록지관리'";
            SQL = SQL + ComNum.VBLF + "    AND BB.UNITCLS = '간호활동그룹'";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (";
            SQL = SQL + ComNum.VBLF + "                                SELECT MAX(A1.CHARTDATE) AS CHARTDATE FROM " + ComNum.DB_EMR + "AEMRBNRACTSET A1";
            SQL = SQL + ComNum.VBLF + "                                    WHERE A1.ACPNO = " + AcpEmr.acpNo;
            SQL = SQL + ComNum.VBLF + "                                        AND A1.CHARTDATE <= '" + strChartDate + "')";
            if (mstrITEMCD != "")
            {
                string strITEMCD = "";
                string[] arryITEMCD = mstrITEMCD.Split(',');
                for (i = 0; i < arryITEMCD.Length; i++)
                {
                    if (i == arryITEMCD.Length - 1)
                    {
                        strITEMCD = strITEMCD + "'" + arryITEMCD[i] + "'";
                    }
                    else
                    {
                        strITEMCD = strITEMCD + "'" + arryITEMCD[i] + "', ";
                    }
                }
                SQL = SQL + ComNum.VBLF + "    AND A.ITEMCD IN (" + strITEMCD + ")";
            }
            SQL = SQL + ComNum.VBLF + "ORDER BY BB.DISSEQNO, B.NFLAG1, B.NFLAG2, B.NFLAG3";
            
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count != 0)
            {
                ssWrite_Sheet1.RowCount = dt.Rows.Count;
                ssWrite_Sheet1.SetRowHeight(-1, 38);

                string strGRPNAME = "";
                int intS = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssWrite_Sheet1.Cells[i, 0].Text = dt.Rows[i]["VFLAG1"].ToString().Trim();
                    ssWrite_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ITEMCD"].ToString().Trim();
                    //ssChart_Sheet1.Cells[i, 2].Text = dt.Rows[i]["GRPNAME"].ToString().Trim();
                    ssWrite_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    ssWrite_Sheet1.Cells[i, ssWrite_Sheet1.ColumnCount - 1].Text = dt.Rows[i]["ACTINTERVALCD"].ToString().Trim();

                    if (strGRPNAME != dt.Rows[i]["GRPNAME"].ToString().Trim())
                    {
                        ssWrite_Sheet1.Cells[i, 2].Text = dt.Rows[i]["GRPNAME"].ToString().Trim();
                        if (i != 0)
                        {
                            ssWrite_Sheet1.AddSpanCell(intS, 2, i - intS, 1);
                        }
                        intS = i;
                    }
                    strGRPNAME = dt.Rows[i]["GRPNAME"].ToString().Trim();

                    if (dt.Rows[i]["ACTINTERVALCD"].ToString().Trim() == "연동" || dt.Rows[i]["ACTINTERVALCD"].ToString().Trim() == "설정무")
                    {
                        ssWrite_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ACTINTERVALCD"].ToString().Trim();
                        ssWrite_Sheet1.Cells[i, 6].Locked = true;
                        ssWrite_Sheet1.Cells[i, 6].BackColor = Color.LightGray;
                    }
                    else
                    {
                        ssWrite_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ACTINTERVAL"].ToString().Trim() + dt.Rows[i]["ACTINTERVALCD"].ToString().Trim() + " " + dt.Rows[i]["ACTCNT"].ToString().Trim() + "회";

                        SetDefaultValue(i, dt.Rows[i]["ITEMCD"].ToString().Trim());
                    }
                }

                ssWrite_Sheet1.AddSpanCell(intS, 2, i - intS, 1);
            }
            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// 첫번째 값 조회
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="strITEMCD"></param>
        private void SetDefaultValue(int Row, string strITEMCD)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     ITEMCD, ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL";
            SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + "ACT" + "'";
            SQL = SQL + ComNum.VBLF + "    AND ITEMCD = '" + strITEMCD + "'";
            SQL = SQL + ComNum.VBLF + "GROUP BY ITEMCD, ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "ORDER BY MAX(DSPSEQ)";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            string[] arryITEMVALUE = null;

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            arryITEMVALUE = new string[dt.Rows.Count];
            //arryITEMVALUE[0] = "";

            for (i = 0; i < dt.Rows.Count; i++)
            {
                arryITEMVALUE[i] = dt.Rows[i]["ITEMVALUE"].ToString().Trim();
            }

            clsSpread.gSpreadComboDataSetEx1(ssWrite, Row, 5, Row, 5, arryITEMVALUE, true);

            dt.Dispose();
            dt = null;

            ssWrite_Sheet1.Cells[Row, 5].Text = arryITEMVALUE[0].Trim();
            SetDefaultValue1(Row, strITEMCD, ssWrite_Sheet1.Cells[Row, 5].Text.Trim());
        }

        /// <summary>
        /// 두번째 값 조회
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="strITEMCD"></param>
        /// <param name="strITEMVALUE"></param>
        private void SetDefaultValue1(int Row, string strITEMCD, string strITEMVALUE)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText.MaxLength = 1000;
            TypeText.Multiline = true;
            TypeText.WordWrap = true;

            ssWrite_Sheet1.Cells[Row, 6].CellType = TypeText;
            ssWrite_Sheet1.Cells[Row, 6].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssWrite_Sheet1.Cells[Row, 6].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssWrite_Sheet1.Cells[Row, 6].Locked = false;
            ssWrite_Sheet1.Cells[Row, 6].Text = "";

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "    ITEMVALUE1, DSPSEQ1 ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMBASEVAL ";
            SQL = SQL + ComNum.VBLF + "WHERE JOBGB = '" + "ACT" + "'";
            SQL = SQL + ComNum.VBLF + "     AND ITEMCD = '" + strITEMCD + "' ";
            SQL = SQL + ComNum.VBLF + "     AND ITEMVALUE = '" + strITEMVALUE + "' ";
            SQL = SQL + ComNum.VBLF + "     AND ITEMVALUE1 IS NOT NULL ";
            SQL = SQL + ComNum.VBLF + "ORDER BY DSPSEQ1 ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            string[] arryITEMVALUE = null;

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            arryITEMVALUE = new string[dt.Rows.Count];
            //arryITEMVALUE[0] = "";

            for (i = 0; i < dt.Rows.Count; i++)
            {
                arryITEMVALUE[i] = dt.Rows[i]["ITEMVALUE1"].ToString().Trim();
            }

            clsSpread.gSpreadComboDataSetEx1(ssWrite, Row, 6, Row, 6, arryITEMVALUE, true);

            dt.Dispose();
            dt = null;

            ssWrite_Sheet1.Cells[Row, 6].Text = arryITEMVALUE[0].Trim();
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public bool SaveData()
        {
            if (AcpEmr.ptNo.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "환자를 선택해 주십시오.");
                return false;
            }

            if (clsEmrQuery.READ_PRTLOG(this, mEMRNO.ToString()))
                return false;

            if (txtChartTime.Text.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "시간을 설정해 주십시오.");
                return false;
            }

            string strDate = string.Format("{0} {1}:{2}",
                dtpChartDate.Value.ToShortDateString(),
                VB.Left(txtChartTime.Text.Trim(), 2),
                VB.Right(txtChartTime.Text.Trim(), 2));

            if (!VB.IsDate(strDate))
            {
                ComFunc.MsgBoxEx(this, "작성시간 오류입니다.");
                return false;
            }

            #region 작성일자 팝업 알림창
            string strdtpDate = dtpChartDate.Value.ToString("yyyyMMdd");
            string MsgDate = dtpChartDate.Value.ToString("yyyy-MM-dd");
            string strCurDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            if (AcpEmr.inOutCls == "O")
            {
                if (AcpEmr.medFrDate != dtpChartDate.Value.ToString("yyyyMMdd") && AcpEmr.medDeptCd.Equals("ER") == false)
                {
                    if (ComFunc.MsgBoxQEx(this, "작성일자가 외래 진료일이 아닙니다 계속 작성하시겠습니까?") == DialogResult.No)
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (AcpEmr.medEndDate != "")
                {
                    if ((VB.Val(AcpEmr.medFrDate) > VB.Val(strdtpDate)) || (VB.Val(AcpEmr.medEndDate) < VB.Val(strdtpDate)))
                    {
                        if (ComFunc.MsgBoxQEx(this, "작성일자가 재원기간을 벗어났습니다.\r\n현재 지정하신 작성일자는 '" + MsgDate + "' 입니다 정말 이 날짜로 계속 작성하시겠습니까?") == DialogResult.No)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if ((VB.Val(AcpEmr.medFrDate) > VB.Val(strdtpDate)) || (VB.Val(strCurDate) < VB.Val(strdtpDate)))
                    {
                        if (ComFunc.MsgBoxQEx(this, "작성일자가 재원기간을 벗어났습니다.\r\n현재 지정하신 작성일자는 '" + MsgDate + "' 입니다 정말 이 날짜로 계속 작성하시겠습니까?") == DialogResult.No)
                        {
                            return false;
                        }
                    }
                }
            }
            #endregion

            if (AcpEmr.inOutCls == "" || AcpEmr.ptNo == "" || AcpEmr.medDeptCd == "" ||
                AcpEmr.medDrCd == "" || AcpEmr.medFrDate == "")
            {
                ComFunc.MsgBoxEx(this, "환자 정보가 정확하지 않습니다." + ComNum.VBLF + "확인 후 다시 시도 하십시오.");
                return false;
            }

            if (mEMRNO != 0)
            {
                if (clsType.User.IdNumber != mEmrUseId)
                {
                    ComFunc.MsgBoxEx(this, "작성된 사용자가 다릅니다. 변경 할 수 없습니다.");
                    return false;
                }

                if (ComFunc.MsgBoxQEx(this, "기존내용을 변경 하시겠습니까?") == DialogResult.No)
                {
                    return false;
                }

                if (clsEmrQuery.READ_CHART_APPLY(this, mEMRNO.ToString()))
                    return false;

                if (clsEmrQuery.READ_PRTLOG(this, mEMRNO.ToString()))
                    return false;
            }

            #region BUNDLE, CMS 저장 메시지
            bool bundleFormWrite = false;
            bool IsCms = false;

            for (int i = 0; i < ssWrite_Sheet1.RowCount; i++)
            {
                string strItemCd = ssWrite_Sheet1.Cells[i, 1].Text.Trim();
                string strGrpNm = ssWrite_Sheet1.Cells[i, 2].Text.Trim();
                string strValue = ssWrite_Sheet1.Cells[i, 5].Text.Trim();
                //                if (strItemCd.Equals("I0000037521") && strValue.Equals("삽입") && FormPatInfoFunc.Set_FormPatInfo_IsWrite(clsDB.DbCon, bundleForm, AcpEmr, dtpChartDate.Value.ToString("yyyyMMdd")) == false)  //폴리 삽입일때
                if (strItemCd.Equals("I0000037521") && (strValue.Equals("삽입") || strValue.Equals("교환")))  //폴리 삽입(교환)일때
                {
                    bundleFormWrite = true;
                    break;
                }

                if (strGrpNm.Equals("신체 보호대 적용 및 관찰") || strGrpNm.Equals("Cast 간호"))
                {
                    IsCms = true;
                    break;
                }
            }

            if (bundleFormWrite)// && strInsertDay.Equals(dtpChartDate.Value.ToString("yyyyMMdd")) && string.IsNullOrWhiteSpace(strMaintaintDay))
            {
                EmrForm bundleForm = clsEmrChart.SerEmrFormInfo(clsDB.DbCon, "2640", clsEmrQuery.GetNewFormMaxUpdateNo(clsDB.DbCon, 2640).ToString());
                frmEmrChartNewX = new frmEmrChartNew(bundleForm.FmFORMNO.ToString(), bundleForm.FmUPDATENO.ToString(), AcpEmr, "0", "W", this);
                frmEmrChartNewX.StartPosition = FormStartPosition.CenterParent;
                frmEmrChartNewX.FormClosed += FrmEmrChartNewX_FormClosed;
                frmEmrChartNewX.ShowDialog(this);
                return false;
            }

            if (IsCms)
            {
                ComFunc.MsgBoxEx(this, "임상관찰에서 CMS 항목을 작성해주세요.");
            }
            #endregion

            if (SaveUserChart() == true)
            {
                //ComFunc.MsgBoxEx(this, "저장이 완료되었습니다.");

                if (mEMRNO > 0)
                {
                    if (frmSugaOrderSaveX != null && frmSugaOrderSaveX.ss2.ActiveSheet.NonEmptyRowCount > 0)
                    {
                        clsEmrQuery.SaveOrderData(this, frmSugaOrderSaveX.ss2, AcpEmr, dtpChartDate.Value.ToString("yyyy-MM-dd"), mEMRNO);
                    }

                    #region //전자인증 하기
                    if (System.Diagnostics.Debugger.IsAttached == false)
                    {
                        bool blnCert = true;
                        if (mEMRNO > 0)
                        {
                            if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                            {
                                blnCert = clsEmrQuery.SaveEmrCert(clsDB.DbCon, mEMRNO);
                            }
                        }
                    }

                    #endregion

                    //if (clsType.gHosInfo.strEmrCertUseYn == "1")
                    //{
                    //    bool blnCert = clsEmrQuery.SaveDataAEMRCHARTCERTY(this, false, this, dblEmrNo, null);
                    //    if (blnCert == false)
                    //    {
                    //        ComFunc.MsgBoxEx(this, "인증중 에러가 발생했습니다." + ComNum.VBLF + "추후 인증을 실시해 주시기 바랍니다.");
                    //    }
                    //}
                }

                if (rEventSendave != null)
                {
                    rEventSendave(true);
                    Close();
                }

                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 기록지 저장 루틴
        /// </summary>
        /// <returns></returns>
        bool SaveUserChart()
        {
            bool rtnVal = false;

            string strChartDate = dtpChartDate.Value.ToString("yyyyMMdd");
            string strChartTime = txtChartTime.Text.Replace(":", "") + "00";

            if (strChartTime.Length < 6)
            {
                strChartTime = ComFunc.RPAD(strChartTime, 6, "0");
            }

            string SqlErr = "";

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);
                string strSaveFlag = "SAVE";
                string strSAVEGB = "1";
                string strSAVECERT = "1"; // 0:임시저장, 1:인증저장
                string strFORMGB = "0";

                double dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                double dblEmrNoNew = mEMRNO;

                if (mEMRNO > 0)
                {
                    #region //과거기록 백업
                    SqlErr = clsEmrQuery.SaveChartMastHis(clsDB.DbCon, mEMRNO.ToString(), dblEmrHisNo, strCurDate, strCurTime, "C", strSaveFlag, clsType.User.IdNumber);
                    if (SqlErr != "OK")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    #endregion
                }
                else
                {
                    dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));
                }

                if (ssWrite.ActiveSheet.RowCount > 0)
                {
                    //데이타 있을때만 저장
                    #region //저장 CHRATMAST
                    string strSaveId = clsType.User.IdNumber;
                    if (clsEmrQuery.SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, AcpEmr, mstrFormNo, mstrUpdateNo,
                                        strChartDate, strChartTime,
                                        strSaveId, strSaveId, strSAVEGB, strSAVECERT, strFORMGB,
                                        strCurDate, strCurTime, strSaveFlag) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("AEMRCHARTROW저장 중 에러가 발생했습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    #endregion //저장 CHRATMAST

                    #region //저장 AEMRCHARTROW
                    if (pSaveDataAEMRCHARTROW(dblEmrNoNew, dblEmrHisNo) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("AEMRCHARTROW저장 중 에러가 발생했습니다.");
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    #endregion //저장 AEMRCHARTROW
                    mEMRNO = dblEmrNoNew;
                }
                else
                {
                    mEMRNO = 0;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                //if (ss2_Sheet1.NonEmptyRowCount > 0)
                //{
                //    clsEmrQuery.SaveOrderData(this, ss2, AcpEmr, dtpChartDate.Value.ToString("yyyy-MM-dd"), mEMRNO);
                //}

                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// Save AEMRCHARTROW
        /// </summary>
        /// <param name="dblEmrNoNew"></param>
        /// <param name="dblEmrHisNo"></param>
        /// <returns></returns>
        private bool pSaveDataAEMRCHARTROW(double dblEmrNoNew, double dblEmrHisNo)
        {
            bool rtnVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";

            string[] arryEMRNO = null;
            string[] arryITEMCD = null;
            string[] arryITEMNO = null;
            string[] arryITEMINDEX = null;
            string[] arryITEMTYPE = null;
            string[] arryITEMVALUE = null;
            string[] arryITEMVALUE1 = null;
            string[] arryDSPSEQ = null;

            try
            {
                int intMaxValue = ssWrite.ActiveSheet.RowCount;

                for (i = 0; i < intMaxValue; i++)
                {
                    string strITEMCD = "";
                    string strITEMNO = "";
                    string strITEMINDEX = "0";
                    string strITEMTYPE = "";
                    string strITEMVALUE = "";
                    string strITEMVALUE1 = "";
                    string strDSPSEQ = "0";

                    strDSPSEQ = i.ToString();

                    strITEMCD = ssWrite.ActiveSheet.Cells[i, 1].Text.Trim();
                    strITEMNO = strITEMCD;
                    int intFind = VB.InStr(strITEMCD, "_");

                    if (intFind > 0)
                    {
                        strITEMNO = ComFunc.SptChar(strITEMCD, 0, "_");
                        strITEMINDEX = ComFunc.SptChar(strITEMCD, 1, "_");
                    }

                    //strITEMTYPE = ssWrite.ActiveSheet.Cells[i, ssWrite.ActiveSheet.ColumnCount - 1].CellType.ToString();
                    strITEMTYPE = "TEXT";
                    strITEMVALUE = ssWrite.ActiveSheet.Cells[i, 5].Text.Trim();
                    strITEMVALUE1 = ssWrite.ActiveSheet.Cells[i, 6].Text.Trim();

                    strITEMVALUE = strITEMVALUE.Replace("'", "`");
                    strITEMVALUE1 = strITEMVALUE1.Replace("'", "`");

                    if (arryEMRNO == null)
                    {
                        arryEMRNO = new string[0];
                        arryITEMCD = new string[0];
                        arryITEMNO = new string[0];
                        arryITEMINDEX = new string[0];
                        arryITEMTYPE = new string[0];
                        arryITEMVALUE = new string[0];
                        arryITEMVALUE1 = new string[0];
                        arryDSPSEQ = new string[0];
                    }
                    Array.Resize<string>(ref arryEMRNO, arryEMRNO.Length + 1);
                    Array.Resize<string>(ref arryITEMCD, arryITEMCD.Length + 1);
                    Array.Resize<string>(ref arryITEMNO, arryITEMNO.Length + 1);
                    Array.Resize<string>(ref arryITEMINDEX, arryITEMINDEX.Length + 1);
                    Array.Resize<string>(ref arryITEMTYPE, arryITEMTYPE.Length + 1);
                    Array.Resize<string>(ref arryITEMVALUE, arryITEMVALUE.Length + 1);
                    Array.Resize<string>(ref arryITEMVALUE1, arryITEMVALUE1.Length + 1);
                    Array.Resize<string>(ref arryDSPSEQ, arryDSPSEQ.Length + 1);

                    arryITEMCD[arryEMRNO.Length - 1] = strITEMCD;
                    arryITEMNO[arryEMRNO.Length - 1] = strITEMNO;
                    arryITEMINDEX[arryEMRNO.Length - 1] = strITEMINDEX;
                    arryITEMTYPE[arryEMRNO.Length - 1] = strITEMTYPE;
                    arryITEMVALUE[arryEMRNO.Length - 1] = strITEMVALUE;
                    arryITEMVALUE1[arryEMRNO.Length - 1] = strITEMVALUE1;
                    arryDSPSEQ[arryEMRNO.Length - 1] = strDSPSEQ;
                }

                if (arryEMRNO == null) return rtnVal;

                SQL = "";
                SQL = SQL + "\r\n" + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTROW ";
                SQL = SQL + "\r\n" + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1 )";
                SQL = SQL + "\r\n" + "VALUES (";
                SQL = SQL + "\r\n" + dblEmrNoNew.ToString() + ",";    //EMRNO
                SQL = SQL + "\r\n" + dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                SQL = SQL + "\r\n" + " :ITEMCD,";   //ITEMCD
                SQL = SQL + "\r\n" + " :ITEMNO,"; //ITEMNO
                SQL = SQL + "\r\n" + " :ITEMINDEX,"; //ITEMINDEX
                SQL = SQL + "\r\n" + " :ITEMTYPE,";   //ITEMTYPE
                SQL = SQL + "\r\n" + " :ITEMVALUE,";   //ITEMVALUE
                SQL = SQL + "\r\n" + " :DSPSEQ,";   //DSPSEQ
                SQL = SQL + "\r\n" + " :ITEMVALUE1";   //ITEMVALUE
                SQL = SQL + "\r\n" + ")";

                SqlErr = clsDB.ExecuteChartRow(clsDB.DbCon, SQL, dblEmrNoNew, dblEmrHisNo, arryITEMCD, arryITEMNO, arryITEMINDEX, arryITEMTYPE, arryITEMVALUE, arryDSPSEQ, arryITEMVALUE1);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                rtnVal = true;
                return rtnVal;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private void GetLastData(string strFlag)
        {
            #region 변수
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            #endregion

            #region 쿼리
            string SQL = string.Empty;
            SQL += ComNum.VBLF + "WITH ITEM_LIST AS ";
            SQL += ComNum.VBLF + "(";
            SQL += ComNum.VBLF + "  SELECT   MAX(CHARTDATE || CHARTTIME) CHARTDATE";
            SQL += ComNum.VBLF + "         , R.ITEMCD ";
            SQL += ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
            SQL += ComNum.VBLF + "      INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R";
            SQL += ComNum.VBLF + "         ON A.EMRNO = R.EMRNO";
            SQL += ComNum.VBLF + "        AND A.EMRNOHIS = R.EMRNOHIS";
            SQL += ComNum.VBLF + "        AND R.ITEMCD IN";
            SQL += ComNum.VBLF + "        (";
            SQL += ComNum.VBLF + "        " + string.Join(",", strItem);
            SQL += ComNum.VBLF + "        )";
            SQL += ComNum.VBLF + "        AND R.ITEMVALUE > CHR(0) ";
            SQL += ComNum.VBLF + "   WHERE A.PTNO = '" + AcpEmr.ptNo + "'";
            SQL += ComNum.VBLF + "     AND A.FORMNO = 1575";
            SQL += ComNum.VBLF + "     AND A.MEDFRDATE = '" + AcpEmr.medFrDate + "'";
            SQL += ComNum.VBLF + "     AND (A.CHARTDATE || CHARTTIME) < '" + dtpChartDate.Value.ToString("yyyyMMdd") + txtChartTime.Text.Trim().Replace(":", "") + "'";
            SQL += ComNum.VBLF + "   GROUP BY ITEMCD";
            SQL += ComNum.VBLF + ")";
            SQL += ComNum.VBLF + "SELECT   EMRNO";
            SQL += ComNum.VBLF + "       , (SELECT EMRNOHIS FROM KOSMOS_EMR.AEMRCHARTMST WHERE EMRNO = B.EMRNO) AS EMRNOHIS ";
            SQL += ComNum.VBLF + "       , ITEMCD ";
            SQL += ComNum.VBLF + "  FROM ITEM_LIST A";
            SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRCHARTMST B";
            SQL += ComNum.VBLF + "       ON B.PTNO = '" + AcpEmr.ptNo + "'";
            SQL += ComNum.VBLF + "      AND B.FORMNO = 1575";
            SQL += ComNum.VBLF + "      AND B.MEDFRDATE = '" + AcpEmr.medFrDate + "'";
            SQL += ComNum.VBLF + "      AND (B.CHARTDATE || B.CHARTTIME) = A.CHARTDATE";

            #endregion

            //string strEmrNo = string.Empty;
            //string strEmrNoHis = string.Empty;

            try
            {
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    return;
                }

                Dictionary<string, LastData> keys = new Dictionary<string, LastData>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (keys.ContainsKey(reader.GetValue(2).ToString().Trim()) == false)
                        {
                            LastData last = new LastData()
                            {
                                EMRNO = reader.GetValue(0).ToString().Trim(),
                                EMRNOHIS = reader.GetValue(1).ToString().Trim(),
                                ITEMCD = reader.GetValue(2).ToString().Trim()
                            };
                            keys.Add(last.ITEMCD, last);
                        }
                        //strEmrNo = reader.GetValue(0).ToString().Trim();
                        //strEmrNoHis = reader.GetValue(1).ToString().Trim();
                    }
                }

                reader.Dispose();

                if (ssWrite_Sheet1.RowCount == 0)
                    return;

                LastData lastData;

                for (int i = 0; i < ssWrite_Sheet1.RowCount; i++)
                {
                    if (strFlag == "PART")
                    {
                        if (ssWrite_Sheet1.Cells[i, 3].BackColor == mSelColor)
                        {
                            string strITEMCD = ssWrite_Sheet1.Cells[i, 1].Text.Trim();
                            if (keys.TryGetValue(strITEMCD, out lastData))
                            {
                                GetActDataSub(i, strITEMCD, lastData.EMRNO, lastData.EMRNOHIS);
                            }
                        }
                    }
                    else
                    {
                        string strITEMCD = ssWrite_Sheet1.Cells[i, 1].Text.Trim();
                        if (keys.TryGetValue(strITEMCD, out lastData))
                        {
                            GetActDataSub(i, strITEMCD, lastData.EMRNO, lastData.EMRNOHIS);
                            Get_VentilatorUseDay(i);
                        }
                        //GetActDataSub(i, strITEMCD, strEmrNo, strEmrNoHis);
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        #endregion //함수

        private void ssWrite_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssWrite_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            string strITEMCD = ssWrite_Sheet1.Cells[e.Row, 1].Text.Trim();
            string strITEMVALUE = ssWrite_Sheet1.Cells[e.Row, 5].Text.Trim();
            string ItemNm = ssWrite_Sheet1.Cells[e.Row, 3].Text;

            //if (clsType.User.IdNumber.Equals("8822") && e.Column == 5 && e.Button == MouseButtons.Right &&
            //    string.IsNullOrWhiteSpace(strITEMCD) == false &&
            //    FormPatInfoFunc.Set_FormPatInfo_ItemSugaMaaping(clsDB.DbCon, mstrFormNo, AcpEmr.ward, strITEMCD, strITEMVALUE))
            if (e.Column == 5 && e.Button == MouseButtons.Right &&
           string.IsNullOrWhiteSpace(strITEMCD) == false &&
           FormPatInfoFunc.Set_FormPatInfo_ItemSugaMaaping(clsDB.DbCon, mstrFormNo, AcpEmr.ward, strITEMCD, strITEMVALUE))
            {
                if (Width < 1256)
                {
                    this.Width = 1256;
                }

                if (frmSugaOrderSaveX != null)
                {
                    frmSugaOrderSaveX.SET_BAT(AcpEmr.ward, mstrFormNo, ItemNm, strITEMCD, strITEMVALUE, e.Row);
                    return;
                }
                return;
            }

            if (e.Column != 3)
            {
                return;
            }

            if (ssWrite_Sheet1.Cells[e.Row, 3].BackColor == mSelColor)
            {
                ssWrite_Sheet1.Cells[e.Row, 3].BackColor = mDeSelColor;
            }
            else
            {
                ssWrite_Sheet1.Cells[e.Row, 3].BackColor = mSelColor;
            }

            if (e.Column != 5) return;


        }

        private void ssWrite_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssWrite_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            if (e.Column != 2)
            {
                return;
            }

            string strGrpCd = ssWrite_Sheet1.Cells[e.Row, 0].Text.Trim();
            Color SelectedColor = ssWrite_Sheet1.Cells[e.Row, 3].BackColor;
            Color SetColor;
            if (SelectedColor == mSelColor)
            {
                SetColor = mDeSelColor;
            }
            else
            {
                SetColor = mSelColor;
            }

            for (int i = 0; i < ssWrite_Sheet1.RowCount; i++)
            {
                if (strGrpCd == ssWrite_Sheet1.Cells[i, 0].Text.Trim())
                {
                    ssWrite_Sheet1.Cells[i, 3].BackColor = SetColor;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool blnSelect = false;
            for (int i = ssWrite_Sheet1.RowCount - 1; i >= 0; i--)
            {
                if (ssWrite_Sheet1.Cells[i, 3].BackColor == mSelColor)
                {
                    ssWrite_Sheet1.Rows[i].Remove();
                    blnSelect = true;
                }
            }

            if (blnSelect == false)
            {
                return;
            }

            if (SaveData() == true)
            {
                GetData(dtpChartDate.Value.ToString("yyyyMMdd"));
                GetActData();
            }
        }

        private void mbtnNrHistory_Click(object sender, EventArgs e)
        {
            GetLastData("ALL");
        }

        private void mbtnNrHistoryPart_Click(object sender, EventArgs e)
        {
            GetLastData("PART");
        }

        private void mbtnPTOrder_Click(object sender, EventArgs e)
        {
            panPT.Visible = !panPT.Visible;
            FormPatInfoFunc.Set_FormPatInfo_PTOrder(clsDB.DbCon, AcpEmr, dtpChartDate.Value.ToString("yyyy-MM-dd"), SSPT);
        }

        private void dtpChartDate_ValueChanged(object sender, EventArgs e)
        {
            mbtnPTOrder.Visible = FormPatInfoFunc.Set_FormPatInfo_PTOrder(clsDB.DbCon, AcpEmr, dtpChartDate.Value.ToString("yyyy-MM-dd"));
        }

        private void SSPT_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SSPT_Sheet1.RowCount == 0 || ssWrite_Sheet1.RowCount == 0)
                return;


            for (int i = 0; i < ssWrite_Sheet1.RowCount; i++)
            {
                //물리치료
                if (ssWrite_Sheet1.Cells[i, 1].Text.Trim().Equals("I0000001205"))
                {
                    ssWrite_Sheet1.Cells[i, 5].Text = SSPT_Sheet1.Cells[e.Row, 3].Text.Trim().Substring(SSPT_Sheet1.Cells[e.Row, 3].Text.Trim().Length - 5);
                    ssWrite_Sheet1.Cells[i, 6].Text = SSPT_Sheet1.Cells[e.Row, 2].Text.Trim();
                    panPT.Visible = false;
                    return;
                }
            }
        }

        private void ssWrite_EditorFocused(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (ssWrite_Sheet1.Cells[e.Row, e.Column].CellType != null && ssWrite_Sheet1.Cells[e.Row, e.Column].CellType.ToString().Equals("ComboBoxCellType"))
            {
                FarPoint.Win.FpCombo fpCombo = (FarPoint.Win.FpCombo)e.EditingControl;
                fpCombo.EditModeCursorPosition = FarPoint.Win.EditModeCursorPosition.FirstInputPosition;
                fpCombo.SelectionStart = 0;
                fpCombo.SelectionLength = 0;
            }
        }

        private void mbtnIcuDiet_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ssWrite_Sheet1.RowCount; i++)
            {
                if (string.IsNullOrWhiteSpace(ssWrite_Sheet1.Cells[i, 5].Text))
                {
                    if (ssWrite_Sheet1.Cells[i, 1].Text.Equals("I0000001465")) //식이
                    {
                        ssWrite_Sheet1.Cells[i, 5].Text = mbtnIcuDiet.Tag.ToString();
                        if (ssWrite_Sheet1.Rows[i].GetPreferredHeight() > ssWrite_Sheet1.Rows[i].Height)
                        {
                            ssWrite_Sheet1.Rows[i].Height = ssWrite_Sheet1.Rows[i].GetPreferredHeight() + 16;
                        }
                    }
                }
            }
        }

        private void frmNrActingItemNew1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (PopupMenu != null )
            {
                PopupMenu.Dispose();
                PopupMenu = null;
            }

            if (frmSugaOrderSaveX != null)
            {
                frmSugaOrderSaveX.Dispose();
                frmSugaOrderSaveX = null;
            }
        }

        private void ss3_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column == 0)
            {
                //if (ss2_Sheet1.Cells[e.Row, 0].Text.Equals("True"))
                //{
                //    ss2_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(255, 128, 128);
                //}
                //else
                //{
                //    ss2_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(255, 220, 220);
                //}
            }
        }

        private void ss2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComFunc.MsgBoxQEx(this, "해당 항목을 삭제하시겠습니까?") == DialogResult.No)
                return;


            //ss2_Sheet1.Rows.Remove(e.Row, 1);
        }

        private void ss2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //int i = 0;

            //if (ss2_Sheet1.RowCount == 0)
            //{
            //    return;
            //}


            //if (e.ColumnHeader == true)
            //{
            //    if (e.Column == 0)
            //    {
            //        if (ss2_Sheet1.ColumnHeader.Cells[0, 0].Text == "□")
            //        {
            //            ss2_Sheet1.ColumnHeader.Cells[0, 0].Text = "■";
            //        }
            //        else
            //        {
            //            ss2_Sheet1.ColumnHeader.Cells[0, 0].Text = "□";
            //        }

            //        if (ss2_Sheet1.ColumnHeader.Cells[0, 0].Text == "■")
            //        {
            //            for (i = 0; i < ss2_Sheet1.NonEmptyRowCount; i++)
            //            {
            //                if (ss2_Sheet1.Cells[i, 0].CellType == null || ss2_Sheet1.Cells[i, 0].CellType.ToString() != "TextCellType")
            //                {
            //                    ss2_Sheet1.Cells[i, 0].Value = true;
            //                    ss2_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 128, 128);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            for (i = 0; i < ss2_Sheet1.NonEmptyRowCount; i++)
            //            {
            //                if (ss2_Sheet1.Cells[i, 0].CellType == null || ss2_Sheet1.Cells[i, 0].CellType.ToString() != "TextCellType")
            //                {
            //                    ss2_Sheet1.Cells[i, 0].Value = false;
            //                    ss2_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 220, 220);
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    if (e.RowHeader == true || e.Column == 0)
            //    {
            //        return;
            //    }

            //    if (e.Column == 11)
            //    {
            //        return;
            //    }

            //    if (e.Column == 2)
            //    {
            //        return;
            //    }

            //    if (ss2_Sheet1.Cells[e.Row, 5].Text == "0")
            //    {
            //        return;
            //    }


            //    if (e.Column != 0)
            //    {
            //        if (Convert.ToBoolean(ss2_Sheet1.Cells[e.Row, 0].Value) == true)
            //        {
            //            ss2_Sheet1.Cells[e.Row, 0].Value = false;
            //            ss2_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(255, 220, 220);
            //        }
            //        else
            //        {
            //            ss2_Sheet1.Cells[e.Row, 0].Value = true;
            //            ss2_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(255, 128, 128);
            //        }
            //    }
            //}
        }

        private void ss2_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            //if (e.Column == 2)
            //{
            //    ss2_Sheet1.Cells[e.Row, 0].Value = true;
            //}
        }

        private void ssWrite_EnterCell(object sender, EnterCellEventArgs e)
        {
            string strITEMCD = ssWrite_Sheet1.Cells[e.Row, 1].Text.Trim();
            string strITEMVALUE = ssWrite_Sheet1.Cells[e.Row, 5].Text.Trim();

            Set_ValueOrder(e.Row, strITEMCD, strITEMVALUE);
        }
    }
}
