using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;

namespace ComBase
{
    /// <summary>
    /// Class Name      : MedIpdNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-04-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\mtsEmr\CADEX\Frm영양불량평가지New" >> frmBadPationtEvaluationResult.cs 폼이름 재정의" />
    /// 
    public partial class frmBadPationtEvaluationResult : Form
    {

        public delegate void EventClose();
        public event EventClose rEventClose;

        string strEXEName = "";

        string FstrGubun = "";
        string FstrROWID = "";
        string GstrROWID = "";  //GstrHelpCode

        public frmBadPationtEvaluationResult(string strEXENameX, string GstrROWIDX)
        {
            strEXEName = strEXENameX;
            GstrROWID = GstrROWIDX;

            InitializeComponent();
        }

        public frmBadPationtEvaluationResult()
        {
            InitializeComponent();
        }

        private void frmBadPationtEvaluationResult_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (VB.UCase(strEXEName) != "DIETORDER")
            {
                btnSave.Enabled = false;
                btnPrint.Enabled = false;
            }

            FstrROWID = GstrROWID;

            READ_DATA(FstrROWID);
        }

        private bool Save_Data(string ArgROWID)
        {
            string strDIETRESULT = "";
            string strHEALTH_2 = "";
            string strPIBW_2 = "";
            string strALBUMIN_2 = "";
            string strHB_2 = "";
            string strTLC_2 = "";
            string strTCHOL_2 = "";
            string strAGE_2 = "";
            string strDIETNAME_2 = "";
            string strDIETPROBLEM1 = "";
            string strDIETPROBLEM2 = "";
            string strDIETPROBLEM3 = "";
            string strDIETPROBLEM4 = "";
            string strDIETPROBLEM5 = "";
            string strDIETPROBLEM6 = "";
            string strDIETPROBLEM7 = "";
            string strNEED_1_1 = "";
            string strNEED_1_2 = "";
            string strNEED_2_1 = "";
            string strNEED_2_2 = "";
            string strNEED_3_1 = "";
            string strNEED_3_2 = "";
            string strNEED_3_3 = "";
            string strNEED_MEMO = "";
            string strNEED_DATE = "";
            string strNEED_SABUN = "";
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;


            strDIETRESULT = ss1_Sheet1.Cells[4, 5].Text.Trim();

            strHEALTH_2 = ss1_Sheet1.Cells[14, 2].Text.Trim();

            strPIBW_2 = ss1_Sheet1.Cells[18, 4].Text.Trim();
            strALBUMIN_2 = ss1_Sheet1.Cells[18, 8].Text.Trim();

            strHB_2 = ss1_Sheet1.Cells[19, 4].Text.Trim();
            strTLC_2 = ss1_Sheet1.Cells[19, 8].Text.Trim();

            strTCHOL_2 = ss1_Sheet1.Cells[20, 4].Text.Trim();
            strAGE_2 = ss1_Sheet1.Cells[20, 8].Text.Trim();

            strDIETNAME_2 = ss1_Sheet1.Cells[21, 4].Text.Trim();

            strDIETPROBLEM1 = ss1_Sheet1.Cells[23, 5].Text.Trim();
            strDIETPROBLEM2 = ss1_Sheet1.Cells[24, 5].Text.Trim();
            strDIETPROBLEM3 = ss1_Sheet1.Cells[25, 5].Text.Trim();
            strDIETPROBLEM4 = ss1_Sheet1.Cells[26, 5].Text.Trim();
            strDIETPROBLEM5 = ss1_Sheet1.Cells[27, 5].Text.Trim();

            strDIETPROBLEM6 = ss1_Sheet1.Cells[28, 4].Text.Trim();
            strDIETPROBLEM7 = ss1_Sheet1.Cells[29, 4].Text.Trim();

            strNEED_1_1 = ss1_Sheet1.Cells[34, 4].Text.Trim();
            strNEED_1_2 = ss1_Sheet1.Cells[34, 6].Text.Trim();

            strNEED_2_1 = ss1_Sheet1.Cells[35, 4].Text.Trim();
            strNEED_2_2 = ss1_Sheet1.Cells[35, 6].Text.Trim();

            if (Convert.ToBoolean(ss1_Sheet1.Cells[38, 3].Value) == true)
            {
                strNEED_3_1 = "1";
            }
            else
            {
                strNEED_3_1 = "0";
            }
            if (Convert.ToBoolean(ss1_Sheet1.Cells[38, 5].Value) == true)
            {
                strNEED_3_2 = "1";
            }
            else
            {
                strNEED_3_2 = "0";
            }
            if (Convert.ToBoolean(ss1_Sheet1.Cells[38, 7].Value) == true)
            {
                strNEED_3_3 = "1";
            }
            else
            {
                strNEED_3_3 = "0";
            }

            strNEED_MEMO = ss1_Sheet1.Cells[41, 2].Text.Trim();

            strNEED_DATE = ss1_Sheet1.Cells[43, 3].Text.Trim();
            strNEED_DATE = strNEED_DATE + " " + ss1_Sheet1.Cells[43, 5].Text.Trim();

            if (strNEED_DATE.Trim() == "")
            {
                strNEED_DATE = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "DM");
            }


            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " UPDATE KOSMOS_PMPA.DIET_RATING_PATIENT_NEW SET ";
                SQL = SQL + ComNum.VBLF + " DIETRESULT = '" + (strDIETRESULT).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " HEALTH_2 = '" + (strHEALTH_2).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " PIBW_2 = '" + (strPIBW_2).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " ALBUMIN_2 = '" + (strALBUMIN_2).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " HB_2 = '" + (strHB_2).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " TLC_2 = '" + (strTLC_2).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " TCHOL_2 = '" + (strTCHOL_2).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " DIETNAME_2 = '" + (strDIETNAME_2).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " DIETPROBLEM1_2 = '" + (strDIETPROBLEM1).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " DIETPROBLEM2_2 = '" + (strDIETPROBLEM2).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " DIETPROBLEM3_2 = '" + (strDIETPROBLEM3).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " DIETPROBLEM4_2 = '" + (strDIETPROBLEM4).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " DIETPROBLEM5_2 = '" + (strDIETPROBLEM5).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " DIETPROBLEM6_2 = '" + (strDIETPROBLEM6).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " DIETPROBLEM7_2 = '" + (strDIETPROBLEM7).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " NEED_1_1 = '" + (strNEED_1_1).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " NEED_1_2 = '" + (strNEED_1_2).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " NEED_2_1 = '" + (strNEED_2_1).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " NEED_2_2 = '" + (strNEED_2_2).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " NEED_3_1 = '" + (strNEED_3_1).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " NEED_3_2 = '" + (strNEED_3_2).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " NEED_3_3 = '" + (strNEED_3_3).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " NEED_MEMO = '" + (strNEED_MEMO).Trim() + "',";
                SQL = SQL + ComNum.VBLF + " NEED_DATE = TO_DATE('" + (strNEED_DATE).Trim() + "','YYYY-MM-DD HH24:MI'), ";
                SQL = SQL + ComNum.VBLF + " NEED_SABUN = '" + clsType.User.Sabun + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + ArgROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }



        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (rEventClose != null)
            {
                rEventClose();
            }

            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (VB.UCase(strEXEName) != "DIETORDER")
            {
                MessageBox.Show("사용 할 수 없습니다.");
                return;
            }

            if (Save_Data(FstrROWID) == false)
            {
                return;
            }

            READ_DATA(FstrROWID);
        }

        private void READ_DATA(string ArgROWID)
        {
            CLEAR_SS();

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(INDATE,'YYYY-MM-DD HH24:MI') INDATE, PANO, SNAME, SEX, AGE,";
                SQL = SQL + ComNum.VBLF + " DEPTCODE, ROOMCODE, DIAGNOSIS, HEIGHT, WEIGHT,";
                SQL = SQL + ComNum.VBLF + " HWEIGHT, PIBW, ";
                SQL = SQL + ComNum.VBLF + " DIETRESULT,HEALTH_2,PIBW_2,ALBUMIN_2,";
                SQL = SQL + ComNum.VBLF + " HB_2,TLC_2,TCHOL_2,";
                SQL = SQL + ComNum.VBLF + " DIETNAME_2,DIETPROBLEM1_2,DIETPROBLEM2_2,DIETPROBLEM3_2,";
                SQL = SQL + ComNum.VBLF + " DIETPROBLEM4_2,DIETPROBLEM5_2,DIETPROBLEM6_2,DIETPROBLEM7_2,";
                SQL = SQL + ComNum.VBLF + " NEED_1_1,NEED_1_2,NEED_2_1,NEED_2_2,";
                SQL = SQL + ComNum.VBLF + " NEED_3_1,NEED_3_2,NEED_3_3,NEED_MEMO,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(NEED_DATE,'YYYY-MM-DD HH24:MI') NEED_DATE,NEED_SABUN";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "DIET_RATING_PATIENT_NEW ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + ArgROWID + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss1_Sheet1.Cells[4, 5].Text = dt.Rows[0]["DIETRESULT"].ToString().Trim();

                    ss1_Sheet1.Cells[8, 4].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ss1_Sheet1.Cells[8, 8].Text = dt.Rows[0]["SNAME"].ToString().Trim();

                    ss1_Sheet1.Cells[9, 4].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + dt.Rows[0]["AGE"].ToString().Trim();
                    ss1_Sheet1.Cells[9, 8].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + "/" + dt.Rows[0]["ROOMCODE"].ToString().Trim();

                    ss1_Sheet1.Cells[10, 4].Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();

                    ss1_Sheet1.Cells[14, 2].Text = dt.Rows[0]["HEALTH_2"].ToString().Trim();

                    if (ss1_Sheet1.Cells[14, 2].Text == "")
                    {
                        ss1_Sheet1.Cells[14, 2].Text = "▣Ht:" + dt.Rows[0]["HEIGHT"].ToString().Trim() + "cm  ▣Wt:" + dt.Rows[0]["WEIGHT"].ToString().Trim() + "kg  " +
                       "▣표준체중:" + dt.Rows[0]["HWEIGHT"].ToString().Trim() + "kg  ▣%IBW:" + dt.Rows[0]["PIBW"].ToString().Trim();
                    }


                    ss1_Sheet1.Cells[18, 4].Text = dt.Rows[0]["PIBW_2"].ToString().Trim();
                    ss1_Sheet1.Cells[18, 8].Text = dt.Rows[0]["ALBUMIN_2"].ToString().Trim();

                    ss1_Sheet1.Cells[19, 4].Text = dt.Rows[0]["HB_2"].ToString().Trim();
                    ss1_Sheet1.Cells[19, 8].Text = dt.Rows[0]["TLC_2"].ToString().Trim();


                    ss1_Sheet1.Cells[20, 4].Text = dt.Rows[0]["TCHOL_2"].ToString().Trim();
                    ss1_Sheet1.Cells[20, 8].Text = dt.Rows[0]["AGE"].ToString().Trim();


                    ss1_Sheet1.Cells[21, 4].Text = dt.Rows[0]["DIETNAME_2"].ToString().Trim();


                    ss1_Sheet1.Cells[23, 5].Text = dt.Rows[0]["DIETPROBLEM1_2"].ToString().Trim();
                    ss1_Sheet1.Cells[24, 5].Text = dt.Rows[0]["DIETPROBLEM2_2"].ToString().Trim();
                    ss1_Sheet1.Cells[25, 5].Text = dt.Rows[0]["DIETPROBLEM3_2"].ToString().Trim();
                    ss1_Sheet1.Cells[26, 5].Text = dt.Rows[0]["DIETPROBLEM4_2"].ToString().Trim();
                    ss1_Sheet1.Cells[27, 5].Text = dt.Rows[0]["DIETPROBLEM5_2"].ToString().Trim();

                    ss1_Sheet1.Cells[28, 4].Text = dt.Rows[0]["DIETPROBLEM6_2"].ToString().Trim();
                    ss1_Sheet1.Cells[29, 4].Text = dt.Rows[0]["DIETPROBLEM7_2"].ToString().Trim();


                    ss1_Sheet1.Cells[34, 4].Text = dt.Rows[0]["NEED_1_1"].ToString().Trim();
                    ss1_Sheet1.Cells[34, 6].Text = dt.Rows[0]["NEED_1_2"].ToString().Trim();

                    ss1_Sheet1.Cells[35, 4].Text = dt.Rows[0]["NEED_2_1"].ToString().Trim();
                    ss1_Sheet1.Cells[35, 6].Text = dt.Rows[0]["NEED_2_2"].ToString().Trim();

                    ss1_Sheet1.Cells[38, 3].Text = dt.Rows[0]["NEED_3_1"].ToString().Trim();
                    ss1_Sheet1.Cells[38, 5].Text = dt.Rows[0]["NEED_3_2"].ToString().Trim();
                    ss1_Sheet1.Cells[38, 7].Text = dt.Rows[0]["NEED_3_3"].ToString().Trim();

                    ss1_Sheet1.Cells[41, 2].Text = dt.Rows[0]["NEED_MEMO"].ToString().Trim();

                    ss1_Sheet1.Cells[43, 3].Text = VB.Left(dt.Rows[0]["NEED_DATE"].ToString().Trim(), 10);
                    ss1_Sheet1.Cells[43, 5].Text = VB.Right(dt.Rows[0]["NEED_DATE"].ToString().Trim(), 5);
                    ss1_Sheet1.Cells[43, 8].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["NEED_SABUN"].ToString().Trim());
                }



                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void CLEAR_SS()
        {
            if (VB.UCase(strEXEName) == "DIETORDER")
            {
                btnSave.Enabled = true;
            }

            ss1_Sheet1.Cells[4, 5].Text = "";



            ss1_Sheet1.Cells[8, 4].Text = "";
            ss1_Sheet1.Cells[8, 8].Text = "";

            ss1_Sheet1.Cells[9, 4].Text = "";
            ss1_Sheet1.Cells[9, 8].Text = "";

            ss1_Sheet1.Cells[10, 4].Text = "";
            ss1_Sheet1.Cells[10, 8].Text = "";


            ss1_Sheet1.Cells[14, 2].Text = "";


            ss1_Sheet1.Cells[18, 4].Text = "";
            ss1_Sheet1.Cells[18, 8].Text = "";

            ss1_Sheet1.Cells[19, 4].Text = "";
            ss1_Sheet1.Cells[19, 8].Text = "";

            ss1_Sheet1.Cells[20, 4].Text = "";
            ss1_Sheet1.Cells[20, 8].Text = "";

            ss1_Sheet1.Cells[21, 4].Text = "";
            ss1_Sheet1.Cells[21, 8].Text = "";

            ss1_Sheet1.Cells[23, 5].Text = "";
            ss1_Sheet1.Cells[24, 5].Text = "";
            ss1_Sheet1.Cells[25, 5].Text = "";
            ss1_Sheet1.Cells[26, 5].Text = "";
            ss1_Sheet1.Cells[27, 5].Text = "";

            ss1_Sheet1.Cells[28, 4].Text = "";
            ss1_Sheet1.Cells[29, 4].Text = "";

            ss1_Sheet1.Cells[34, 4].Text = "";
            ss1_Sheet1.Cells[34, 6].Text = "";


            ss1_Sheet1.Cells[35, 4].Text = "";
            ss1_Sheet1.Cells[35, 6].Text = "";


            ss1_Sheet1.Cells[38, 3].Text = "";
            ss1_Sheet1.Cells[38, 5].Text = "";
            ss1_Sheet1.Cells[38, 7].Text = "";

            ss1_Sheet1.Cells[41, 2].Text = "";

            ss1_Sheet1.Cells[43, 3].Text = "";
            ss1_Sheet1.Cells[43, 8].Text = "";
        }
    }
}
