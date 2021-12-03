using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;
using FarPoint.Win.Spread.CellType;
using ComLibB;
using FarPoint.Win.Spread;
using ComEmrBase;
using System.Collections.Generic;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedIpdNr
    /// File Name       : frmSAPS3Score.cs
    /// Description     : SAPS3 SCORE
    /// Author          : 박창욱
    /// Create Date     : 2018-04-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// TODO : XMLINSRT3
    /// </history>
    /// <seealso cref= "\nurse\nrinfo\FrmSAPS3SCORE.frm(FrmSAPS3SCORE.frm) >> frmSAPS3Score.cs 폼이름 재정의" />
    public partial class frmSAPS3Score : Form
    {
        public delegate void SetHelpCode(string strFlag);
        public event SetHelpCode rSetGstrFlag;

        string GstrIPDNO = "";
        string GstrROWID = "";

        string FStrIpddte = "";
        string FStICUdte = "";
        string FstrPANO = "";
        string FstrSign = "";
        string mstrGuBun = "";

        frmEmrViewer frmEmrViewerX = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strIPDNO"></param>
        /// <param name="strROWID"></param>
        /// <param name="strGuBun"> "" == SAPS3 Score // "" != SOFA Calculator </param>
        public frmSAPS3Score(string strIPDNO, string strROWID = "", string strGuBun = "")
        {
            InitializeComponent();
            GstrIPDNO = strIPDNO;
            GstrROWID = strROWID;
            mstrGuBun = strGuBun;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        double getProbOfDeath(int score, double probVar1, double probvar2, double probvar3)
        {
            double rtnVar = 0;

            rtnVar = (probVar1 + Math.Log(score + probvar2) * probvar3);
            rtnVar = (Math.Exp(rtnVar) / (1 + Math.Exp(rtnVar)));

            return rtnVar;
        }

        double getGeneralProbOfDeathSAPS3(int score)
        {
            double rtnVar = 0;

            rtnVar = getProbOfDeath(score, -32.6659, 20.5958, 7.3068);

            return rtnVar;
        }

        string Read_Sabun(string argSabun)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            try
            {
                SQL = "";
                SQL = " SELECT B.NAME, A.KORNAME ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST A, " + ComNum.DB_PMPA + "BAS_BUSE B";
                SQL = SQL + ComNum.VBLF + " WHERE A.BUSE = B.BUCODE ";
                SQL = SQL + ComNum.VBLF + "    AND A.SABUN = '" + argSabun.PadLeft(5, '0') + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVar;
                }

                rtnVar = dt.Rows[0]["NAME"].ToString().Trim() + " / " + dt.Rows[0]["KORNAME"].ToString().Trim();

                dt.Dispose();
                dt = null;

                return rtnVar;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        void Call_ArvTime()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strWard = "";

            strWard = ssView_Sheet1.Cells[1, 2].Text;

            try
            {
                SQL = "";
                SQL = " SELECT a.CHARTDATE, a.CHARTTIME ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXMLMST A, " + ComNum.DB_EMR + "EMRXML B  ";
                SQL = SQL + ComNum.VBLF + " WHERE a.PTNO = '" + FstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + " AND a.FORMNO = '965'  AND a.emrno = b.emrno ";
                SQL = SQL + ComNum.VBLF + " AND EXTRACTVALUE(b.CHARTXML, '//ta3')  = '" + strWard + "' ";
                SQL = SQL + ComNum.VBLF + " AND a.MEDFRDATE = '" + FStrIpddte + "'";
                SQL = SQL + ComNum.VBLF + " AND a.CHARTDATE >= '" + FStrIpddte + "'";

                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + " SELECT CHARTDATE, CHARTTIME ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "AEMRNURSRECORD B";
                SQL = SQL + ComNum.VBLF + "      ON A.EMRNO    = B.EMRNO";
                SQL = SQL + ComNum.VBLF + "     AND A.EMRNOHIS = B.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "     AND B.WARDCODE = '" + strWard + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE a.PTNO = '" + FstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND a.FORMNO = 965";
                SQL = SQL + ComNum.VBLF + "   AND a.MEDFRDATE = '" + FStrIpddte + "'";
                SQL = SQL + ComNum.VBLF + "   AND a.CHARTDATE >= '" + FStrIpddte + "'";
                SQL = SQL + ComNum.VBLF + "  ORDER BY CHARTDATE asc, CHARTTIME asc ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                FStICUdte = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                ssView_Sheet1.Cells[1, 5].Text = VB.Mid(dt.Rows[0]["CHARTDATE"].ToString().Trim(), 1, 4) + "-" + VB.Mid(dt.Rows[0]["CHARTDATE"].ToString().Trim(), 5, 2) + "-" + VB.Mid(dt.Rows[0]["CHARTDATE"].ToString().Trim(), 7, 2);
                ssView_Sheet1.Cells[1, 6].Text = VB.Mid(dt.Rows[0]["CHARTTIME"].ToString().Trim(), 1, 2) + ":" + VB.Mid(dt.Rows[0]["CHARTTIME"].ToString().Trim(), 3, 2);

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            txtStepE1.Text = ((2 * VB.Val(txtStepE2.Text) + VB.Val(txtStepE3.Text)) / 3).ToString();
        }

        private void frmSAPS3Score_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등

            this.FormClosed += FrmSAPS3Score_FormClosed;

            if (mstrGuBun != "")
            {

                this.Width = 343;
                panTitle.Visible = false;
                panel3.Visible = false;
                panel18.Visible = false;

            }
            else
            {
                panTitle2.Visible = false;
                this.WindowState = FormWindowState.Maximized;
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int j = 0;
            string strData = "";
            string strWard = "";
            string strRoom = "";
            string strDiag = "";
            string strEntDate = "";
            string strENTSABUN = "";
            string strExcept_Ex = "";
            string strDIAGNOSIS = "";

            Sofa_Clear();

            cboUrin.Items.Add("0. Greater than 500 mL/day");
            cboUrin.Items.Add("1. 200 - 500 mL/day");
            cboUrin.Items.Add("2. Less than 200 mL/day");
            cboUrin.SelectedIndex = 0;

            ssView_Sheet1.Cells[39, 5].Text = "16"; //기본값
            ssView_Sheet1.Cells[40, 5].Text = "0";

            ssView_Sheet1.Columns[10].Visible = false;
            ssView_Sheet1.Columns[11].Visible = false;
            ssView_Sheet1.Columns[12].Visible = false;
            ssView_Sheet1.Columns[13].Visible = false;
            ssView_Sheet1.Columns[14].Visible = false;
            ssView_Sheet1.Columns[15].Visible = false;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT b.WARDCODE, b.ROOMCODE, a.Pano,A.Jumin1,A.Jumin2,A.SNAME,TO_CHAR(INDATE,'YYYYMMDD') INDATE ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, KOSMOS_PMPA.IPD_NEW_MASTER B ";
                SQL = SQL + ComNum.VBLF + " WHERE B.IPDNO =" + GstrIPDNO;   //VB - P(GstrHelpCode, "^^", 1)
                SQL = SQL + ComNum.VBLF + "  AND A.PANO = B.PANO";
                SQL = SQL + ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.Cells[1, 0].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[1, 2].Text = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[1, 3].Text = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[1, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[1, 4].Text = dt.Rows[0]["Jumin1"].ToString().Trim() + " - " + VB.Mid(dt.Rows[0]["Jumin2"].ToString().Trim(), 1, 2);
                    FStrIpddte = dt.Rows[0]["INDATE"].ToString().Trim();    //입원일자를 변수에 저장
                    FstrPANO = dt.Rows[0]["PANO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;


                //SAPS_ITEM_A SAPS3플로 챠트 사용되는 컬럼
                //SAPS_ITEM_B
                //SAPS_ITEM_C
                //SAPS_SCORE
                //SAPS_AVG
                //SAPS_WARD
                //SAPS_ROOM
                //SAPS_DIAG
                //SAPS_ENTSABUN
                //SAPS_ENTDATE
                //SAPS_INDATE'
                //EXCEPT_EX


                //기존에 등록된 중증도를 읽어 화면에 표시함
                SQL = "";
                SQL = "SELECT SAPS_ITEM_A ,SAPS_ITEM_B,SAPS_ITEM_C,SAPS_SCORE,SAPS_AVG,SAPS_WARD,SAPS_SIGN, ";
                SQL = SQL + ComNum.VBLF + " SAPS_ROOM,SAPS_DIAG,SAPS_ENTSABUN, to_char(SAPS_ENTDATE,'YYYY-MM-DD HH24:mi') SAPS_ENTDATE , nvl(SAPS_INDATE,to_char(sysdate,'YYYY-MM-DD HH24:MI')) SAPS_INDATE,DIAGNOSIS,nvl(EXCEPT_EX,'0000') EXCEPT_EX ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_MASTER ";
                if (GstrROWID == "")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + GstrIPDNO;
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + GstrROWID + "' ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                strDIAGNOSIS = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
                strRoom = dt.Rows[0]["SAPS_ROOM"].ToString().Trim();
                strWard = dt.Rows[0]["SAPS_WARD"].ToString().Trim();
                strDiag = dt.Rows[0]["SAPS_DIAG"].ToString().Trim();
                strData = dt.Rows[0]["SAPS_INDATE"].ToString().Trim();
                strExcept_Ex = dt.Rows[0]["EXCEPT_EX"].ToString().Trim();
                strEntDate = dt.Rows[0]["SAPS_ENTDATE"].ToString().Trim();
                strENTSABUN = dt.Rows[0]["SAPS_ENTSABUN"].ToString().Trim();

                if (strDiag != "")
                {
                    ssView_Sheet1.Cells[1, 7].Text = strDiag;
                }
                else
                {
                    ssView_Sheet1.Cells[1, 7].Text = strDIAGNOSIS;
                }

                if (strWard != "")
                {
                    ssView_Sheet1.Cells[1, 2].Text = strWard;
                }

                if (strRoom != "")
                {
                    ssView_Sheet1.Cells[1, 3].Text = strRoom;
                }

                if (dt.Rows[0]["SAPS_SIGN"].ToString().Trim() != "")
                {
                    FstrSign = "SIGN";
                    btnSign.Enabled = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    FstrSign = "";
                    btnSign.Enabled = true;
                    btnSave.Enabled = true;
                }

                ssView_Sheet1.Cells[2, 7].Text = strEntDate;
                ssView_Sheet1.Cells[2, 9].Text = Read_Sabun(dt.Rows[0]["SAPS_ENTSABUN"].ToString().Trim());

                ssView_Sheet1.Cells[1, 5].Text = VB.Mid(strData, 1, 10);

                ssView_Sheet1.Cells[1, 6].Text = VB.Mid(strData, 12, 5);

                if (VB.Mid(strExcept_Ex, 1, 1) == "1")
                {
                    ssView_Sheet1.Cells[3, 6].Value = true;
                }
                else
                {
                    ssView_Sheet1.Cells[3, 6].Value = false;
                }

                if (VB.Mid(strExcept_Ex, 2, 1) == "1")
                {
                    ssView_Sheet1.Cells[3, 7].Value = true;
                }
                else
                {
                    ssView_Sheet1.Cells[3, 7].Value = false;
                }

                if (VB.Mid(strExcept_Ex, 3, 1) == "1")
                {
                    ssView_Sheet1.Cells[3, 8].Value = true;
                }
                else
                {
                    ssView_Sheet1.Cells[3, 8].Value = false;
                }

                if (VB.Mid(strExcept_Ex, 4, 1) == "1")
                {
                    ssView_Sheet1.Cells[3, 9].Value = true;
                }
                else
                {
                    ssView_Sheet1.Cells[3, 9].Value = false;
                }

                if (dt.Rows[0]["SAPS_SCORE"].ToString().Trim() != "")
                {
                    j = 1;
                    for (i = 5; i <= 15; i++)
                    {
                        if (ssView_Sheet1.Cells[i, 5].CellType.ToString() == "ComboBoxCellType")
                        {
                            ssView_Sheet1.Cells[i, 5].Text = ((ComboBoxCellType)ssView_Sheet1.Cells[i, 5].CellType).Items[Convert.ToInt32(VB.Val(VB.Pstr(dt.Rows[0]["SAPS_ITEM_A"].ToString().Trim(), "@", j)))].ToString();

                            j = j + 1;
                        }
                    }

                    j = 1;
                    for (i = 17; i <= 27; i++)
                    {
                        if (ssView_Sheet1.Cells[i, 5].CellType.ToString() == "ComboBoxCellType")
                        {
                            ssView_Sheet1.Cells[i, 5].Text = ((ComboBoxCellType)ssView_Sheet1.Cells[i, 5].CellType).Items[Convert.ToInt32(VB.Val(VB.Pstr(dt.Rows[0]["SAPS_ITEM_B"].ToString().Trim(), "@", j)))].ToString();
                            j = j + 1;
                        }
                    }

                    j = 1;
                    for (i = 29; i <= 38; i++)
                    {
                        if (ssView_Sheet1.Cells[i, 5].CellType.ToString() == "ComboBoxCellType")
                        {
                            ssView_Sheet1.Cells[i, 5].Text = ((ComboBoxCellType)ssView_Sheet1.Cells[i, 5].CellType).Items[Convert.ToInt32(VB.Val(VB.Pstr(dt.Rows[0]["SAPS_ITEM_C"].ToString().Trim(), "@", j)))].ToString();
                            j = j + 1;
                        }
                    }
                }

                ssView_White();
                ss_Jumsu_Acc();
                ss_AVG_Acc();

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void FrmSAPS3Score_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmEmrViewerX != null)
            {
                frmEmrViewerX.Dispose();
                frmEmrViewerX = null;
                return;
            }
        }

        private void btnEMR_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //string strPara = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(INDATE, 'YYYY-MM-DD') INDATE, DEPTCODE, DRCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ssView_Sheet1.Cells[1, 0].Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY INDATE DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //strPara = "NRINFO" + "," + "I" + "," + dt.Rows[0]["INDATE"].ToString().Trim() + "," + dt.Rows[0]["DEPTCODE"].ToString().Trim() + "," + dt.Rows[0]["DRCODE"].ToString().Trim();

                //clsVbEmr.EXECUTE_TextEmrViewEx(ssView_Sheet1.Cells[1, 0].Text.Trim(), clsType.User.IdNumber);

                if (frmEmrViewerX != null)
                {
                    frmEmrViewerX.SetNewPatient(ssView_Sheet1.Cells[1, 0].Text.Trim());
                    return;
                }

                frmEmrViewerX = new frmEmrViewer(ssView_Sheet1.Cells[1, 0].Text.Trim());
                frmEmrViewerX.rEventClosed += FrmEmrViewerX_rEventClosed;
                frmEmrViewerX.Show(this);

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void FrmEmrViewerX_rEventClosed()
        {
            if (frmEmrViewerX != null)
            {
                frmEmrViewerX.Dispose();
                frmEmrViewerX = null;
                return;
            }
        }

        void Save_EMR(string argIPDNO)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double nEMRNO = 0;
            string strInDate = "";
            string strPano = "";
            string strOutDate = "";
            string strOutTime = "";
            string strDeptCode = "";
            string strDrCd = "";
            string strXML = "";
            string strChartX1 = "";
            string strChartX2 = "";
            string strHead = "";
            string strTagHead = "";
            string strTagVal = "";
            string strTagTail = "";
            string strXMLCert = "";
            string strIPDNO = "";
            string strIK1 = "";
            string strIK2 = "";
            string strIK3 = "";
            string strIK4 = "";
            string strIT1 = "";
            string strIT2 = "";
            string strDT1 = "";
            string strIT3 = "";
            string strIT4 = "";
            string strIT7 = "";
            string strIT5 = "";
            string strIT8 = "";
            string strIT10 = "";
            string strIT11 = "";
            string strIT12 = "";
            string strIT13 = "";
            string strIT14 = "";
            string strIT15 = "";
            string strIT16 = "";
            string strIT17 = "";
            string strIT18 = "";
            string strIT19 = "";
            string strIT20 = "";
            string strIT21 = "";
            string strIT22 = "";
            string strIT23 = "";
            string strIT24 = "";
            string strIT26 = "";
            string strIT27 = "";
            string strIT25 = "";
            string strIT28 = "";
            string strIT29 = "";
            string strIT30 = "";
            string strIT31 = "";
            string strIT32 = "";
            string strIT33 = "";
            string strIT34 = "";
            string strIT36 = "";
            string strIT37 = "";
            string strDT2 = "";
            string strIT35 = "";
            string strIT38 = "";
            string strIT39 = "";  //2019-02-13

            double dblEmrHisNo = 0;

            strIT1 = ssView_Sheet1.Cells[1, 2].Text.Trim();
            strIT2 = ssView_Sheet1.Cells[1, 3].Text.Trim();
            strDT1 = ssView_Sheet1.Cells[1, 5].Text.Trim();
            strIT3 = ssView_Sheet1.Cells[1, 6].Text.Trim();
            strIT4 = ssView_Sheet1.Cells[1, 7].Text.Trim();

            strIK1 = Convert.ToBoolean(ssView_Sheet1.Cells[3, 6].Value) == true ? "true" : "false";
            strIK2 = Convert.ToBoolean(ssView_Sheet1.Cells[3, 7].Value) == true ? "true" : "false";
            strIK3 = Convert.ToBoolean(ssView_Sheet1.Cells[3, 8].Value) == true ? "true" : "false";
            strIK4 = Convert.ToBoolean(ssView_Sheet1.Cells[3, 9].Value) == true ? "true" : "false";

            if (strIK1 == "false" && strIK2 == "false" && strIK3 == "false" && strIK4 == "false")
            {
                strIT7 = ssView_Sheet1.Cells[5, 5].Text.Trim();
                strIT39 = ssView_Sheet1.Cells[6, 5].Text.Trim();
                strIT5 = ssView_Sheet1.Cells[7, 5].Text.Trim();

                strIT8 = ssView_Sheet1.Cells[9, 5].Text.Trim();
                strIT10 = ssView_Sheet1.Cells[10, 5].Text.Trim();
                strIT11 = ssView_Sheet1.Cells[11, 5].Text.Trim();
                strIT12 = ssView_Sheet1.Cells[12, 5].Text.Trim();
                strIT13 = ssView_Sheet1.Cells[13, 5].Text.Trim();
                strIT14 = ssView_Sheet1.Cells[14, 5].Text.Trim();
                strIT15 = ssView_Sheet1.Cells[15, 5].Text.Trim();

                strIT16 = ssView_Sheet1.Cells[17, 5].Text.Trim();

                strIT17 = ssView_Sheet1.Cells[19, 5].Text.Trim();
                strIT18 = ssView_Sheet1.Cells[20, 5].Text.Trim();
                strIT19 = ssView_Sheet1.Cells[21, 5].Text.Trim();
                strIT20 = ssView_Sheet1.Cells[22, 5].Text.Trim();
                strIT21 = ssView_Sheet1.Cells[23, 5].Text.Trim();
                strIT22 = ssView_Sheet1.Cells[24, 5].Text.Trim();

                strIT23 = ssView_Sheet1.Cells[26, 5].Text.Trim();
                strIT24 = ssView_Sheet1.Cells[27, 5].Text.Trim();

                strIT26 = ssView_Sheet1.Cells[29, 5].Text.Trim();
                strIT27 = ssView_Sheet1.Cells[30, 5].Text.Trim();
                strIT25 = ssView_Sheet1.Cells[31, 5].Text.Trim();
                strIT28 = ssView_Sheet1.Cells[32, 5].Text.Trim();
                strIT29 = ssView_Sheet1.Cells[33, 5].Text.Trim();
                strIT30 = ssView_Sheet1.Cells[34, 5].Text.Trim();
                strIT31 = ssView_Sheet1.Cells[35, 5].Text.Trim();
                strIT32 = ssView_Sheet1.Cells[36, 5].Text.Trim();
                strIT33 = ssView_Sheet1.Cells[37, 5].Text.Trim();
                strIT34 = ssView_Sheet1.Cells[38, 5].Text.Trim();
            }

            strIT36 = ssView_Sheet1.Cells[39, 5].Text.Trim();
            strIT37 = ssView_Sheet1.Cells[40, 5].Text.Trim();

            strDT2 = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            strIT35 = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            strIT38 = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);


            SQL = "";
            SQL = " SELECT PANO, SNAME, INDATE, OUTDATE, DRCODE, DEPTCODE, IPDNO ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
            SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("환자 정보가 없습니다. 전자차트 전송에 실패하였습니다.");
                return;
            }

            strPano = dt.Rows[0]["PANO"].ToString().Trim();
            strInDate = VB.Left(dt.Rows[0]["INDATE"].ToString().Trim(), 10);
            strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
            strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
            strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
            strIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();

            dt.Dispose();
            dt = null;


            SQL = "";
            SQL = " SELECT EMRNO ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_SAPS3_SCORE ";
            SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + strIPDNO;

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nEMRNO = VB.Val(dt.Rows[0]["EMRNO"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            EmrForm pForm = clsEmrChart.SerEmrFormUpdateNo(clsDB.DbCon, "2713");
            EmrPatient AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPano, "I", strInDate, strDeptCode);

            if (nEMRNO > 0)
            {
                if (pForm.FmOLDGB == 1)
                {
                    #region XML
                    //기존차트를 변경할 경우 : 백업 테이블로 백업을 하고 신규 data를 입력한다
                    //KOSMOS_EMR.EMRXMLHISTORY_HISTORYNO_SEQ

                    dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO");

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY";
                    SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                    SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                    SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                    SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                    SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                    SQL = SQL + ComNum.VBLF + "      '" + strDT2.Replace("-", "") + "',";
                    SQL = SQL + ComNum.VBLF + "      '" + strIT35.Replace(":", "") + "', '" + clsType.User.Sabun + "',CERTNO";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }


                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXML";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }


                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXMLMST";
                    SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    #endregion
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_PMPA + "NUR_SAPS3_SCORE";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + nEMRNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            if (pForm.FmOLDGB == 1)
            {
                #region XML
                strXML = "";

                strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
                strChartX1 = "<chart>";
                strChartX2 = "</chart>";

                strXML = strHead + strChartX1;

                strTagHead = "<ik1 type=" + VB.Chr(34) + "inputCheck" + VB.Chr(34) + " label=" + VB.Chr(34) + "16세이하" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIK1;
                strTagTail = "]]></ik1>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<ik2 type=" + VB.Chr(34) + "inputCheck" + VB.Chr(34) + " label=" + VB.Chr(34) + "화상" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIK2;
                strTagTail = "]]></ik2>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<ik3 type=" + VB.Chr(34) + "inputCheck" + VB.Chr(34) + " label=" + VB.Chr(34) + "관상동맥질환" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIK3;
                strTagTail = "]]></ik3>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<ik4 type=" + VB.Chr(34) + "inputCheck" + VB.Chr(34) + " label=" + VB.Chr(34) + "심장수술" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIK4;
                strTagTail = "]]></ik4>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it1 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "병동" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT1;
                strTagTail = "]]></it1>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it2 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "병실" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT2;
                strTagTail = "]]></it2>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<dt1 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "입실일" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strDT1;
                strTagTail = "]]></dt1>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it3 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "일실시" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT3;
                strTagTail = "]]></it3>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it4 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "진단명" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT4;
                strTagTail = "]]></it4>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it7 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "box1_1" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT7;
                strTagTail = "]]></it7>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it39 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "box1_2" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT39;
                strTagTail = "]]></it39>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it5 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "box1_3" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT5;
                strTagTail = "]]></it5>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it8 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "co-morbidities1_1" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT8;
                strTagTail = "]]></it8>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it10 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "co-morbidities1_2" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT10;
                strTagTail = "]]></it10>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it11 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "co-morbidities1_3" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT11;
                strTagTail = "]]></it11>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it12 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "co-morbidities1_4" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT12;
                strTagTail = "]]></it12>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it13 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "co-morbidities1_5" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT13;
                strTagTail = "]]></it13>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it14 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "co-morbidities1_6" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT14;
                strTagTail = "]]></it14>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it15 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "co-morbidities1_7" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT15;
                strTagTail = "]]></it15>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it16 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Box2" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT16;
                strTagTail = "]]></it16>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;


                strTagHead = "<it17 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "ReasonforIcu-1" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT17;
                strTagTail = "]]></it17>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it18 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "ReasonforIcu-2" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT18;
                strTagTail = "]]></it18>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it19 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "ReasonforIcu-3" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT19;
                strTagTail = "]]></it19>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it20 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "ReasonforIcu-4" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT20;
                strTagTail = "]]></it20>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it21 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "ReasonforIcu-5" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT21;
                strTagTail = "]]></it21>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it22 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "ReasonforIcu-6" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT22;
                strTagTail = "]]></it22>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it23 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "AcuteInfectionICU-1" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT23;
                strTagTail = "]]></it23>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it24 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "AcuteInfectionICU-2" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT24;
                strTagTail = "]]></it24>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;


                strTagHead = "<it26 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Box3-1" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT26;
                strTagTail = "]]></it26>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it27 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Box3-2" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT27;
                strTagTail = "]]></it27>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it25 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Box3-3" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT25;
                strTagTail = "]]></it25>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it28 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Box3-4" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT28;
                strTagTail = "]]></it28>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it29 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Box3-5" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT29;
                strTagTail = "]]></it29>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it30 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Box3-6" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT30;
                strTagTail = "]]></it30>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it31 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Box3-7" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT31;
                strTagTail = "]]></it31>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it32 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Box3-8" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT32;
                strTagTail = "]]></it32>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it33 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Box3-9" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT33;
                strTagTail = "]]></it33>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it34 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Box3-10" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT34;
                strTagTail = "]]></it34>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it36 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "SAPS3 POINT" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT36;
                strTagTail = "]]></it36>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it37 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Death 퍼센트" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT37;
                strTagTail = "]]></it37>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<dt2 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "작성일" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strDT2;
                strTagTail = "]]></dt2>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it35 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "작성시" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT35;
                strTagTail = "]]></it35>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strTagHead = "<it38 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "작성자" + VB.Chr(34) + "><![CDATA[";
                strTagVal = strIT38;
                strTagTail = "]]></it38>";
                strXML = strXML + strTagHead + strTagVal + strTagTail;

                strXMLCert = strXML;

                strXML = strXML + strChartX2;

                strXMLCert = strXML;



                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT " + ComNum.DB_EMR + "GetEmrXmlNo() FunSeqNo FROM Dual";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nEMRNO = VB.Val(dt.Rows[0]["FunSeqNo"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                //TODO : XMLINSRT3

                OracleCommand cmd = new OracleCommand();

                PsmhDb pDbCon = clsDB.DbCon;

                cmd.Connection = pDbCon.Con;
                cmd.CommandText = ComNum.DB_EMR + "XMLINSRT3";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_EMRNO", OracleDbType.Double, 0, nEMRNO, ParameterDirection.Input);
                //cmd.Parameters.Add("p_FORMNO", OracleDbType.Double, 0, 2597, ParameterDirection.Input);
                cmd.Parameters.Add("p_FORMNO", OracleDbType.Double, 0, 2713, ParameterDirection.Input);     //2019-02-13
                cmd.Parameters.Add("p_USEID", OracleDbType.Varchar2, 8, clsType.User.Sabun, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTDATE", OracleDbType.Varchar2, 8, strDT2.Replace("-", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTTIME", OracleDbType.Varchar2, 6, strIT35.Replace(":", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_ACPNO", OracleDbType.Double, 0, 0, ParameterDirection.Input);
                cmd.Parameters.Add("p_PTNO", OracleDbType.Varchar2, 9, strPano, ParameterDirection.Input);
                cmd.Parameters.Add("p_INOUTCLS", OracleDbType.Varchar2, 1, "I", ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRDATE", OracleDbType.Varchar2, 8, strInDate.Replace("-", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRTIME", OracleDbType.Varchar2, 6, "120000", ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDDATE", OracleDbType.Varchar2, 8, strOutDate.Replace("-", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDTIME", OracleDbType.Varchar2, 6, strOutTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDEPTCD", OracleDbType.Varchar2, 4, strDeptCode, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDRCD", OracleDbType.Varchar2, 6, strDrCd, ParameterDirection.Input);
                cmd.Parameters.Add("p_MIBICHECK", OracleDbType.Varchar2, 1, "0", ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITEDATE", OracleDbType.Varchar2, 8, strDT2.Replace("-", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITETIME", OracleDbType.Varchar2, 6, strIT35.Replace(":", ""), ParameterDirection.Input);
                cmd.Parameters.Add("p_UPDATENO", OracleDbType.Int32, 0, 1, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTXML", OracleDbType.Long, strXML.Length, strXML, ParameterDirection.Input);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                cmd = null;
                #endregion
            }
            else
            {
                #region 매핑
                Dictionary<string, string> strContent = new Dictionary<string, string>();
                strContent.Add("I0000033162", strIK1);
                strContent.Add("I0000000588", strIK2);
                strContent.Add("I0000004623", strIK3);
                strContent.Add("I0000033159", strIK4);
                strContent.Add("I0000030686", strIT1);
                strContent.Add("I0000001269", strIT2);
                strContent.Add("I0000029882", strDT1);
                strContent.Add("I0000014147", strIT4);
                strContent.Add("I0000028905", strIT3);
                strContent.Add("I0000033180", strIT7);
                strContent.Add("I0000036534", strIT39);
                strContent.Add("I0000033174", strIT5);
                strContent.Add("I0000033175", strIT8);
                strContent.Add("I0000036535", strIT10);
                strContent.Add("I0000033176", strIT11);
                strContent.Add("I0000033177", strIT12);
                strContent.Add("I0000022756", strIT13);
                strContent.Add("I0000033178", strIT14);
                strContent.Add("I0000033179", strIT15);
                strContent.Add("I0000033187", strIT16);
                strContent.Add("I0000011850", strIT17);
                strContent.Add("I0000015354", strIT18);
                strContent.Add("I0000033188", strIT19);
                strContent.Add("I0000014661", strIT20);
                strContent.Add("I0000033189", strIT21);
                strContent.Add("I0000033190", strIT22);
                strContent.Add("I0000033191", strIT23);
                strContent.Add("I0000014895", strIT24);
                strContent.Add("I0000033192", strIT26);
                strContent.Add("I0000033193", strIT27);
                strContent.Add("I0000033194", strIT25);
                strContent.Add("I0000033195", strIT28);
                strContent.Add("I0000033196", strIT29);
                strContent.Add("I0000033197", strIT30);
                strContent.Add("I0000033198", strIT31);
                strContent.Add("I0000033199", strIT32);
                strContent.Add("I0000033200", strIT33);
                strContent.Add("I0000033201", strIT34);
                strContent.Add("I0000033202", strIT36);
                strContent.Add("I0000033203", strIT37);
                strContent.Add("I0000015537", strDT2);
                strContent.Add("I0000030624", strIT35);
                strContent.Add("I0000022529", strIT38);
                #endregion

                nEMRNO = clsEmrQuery.SaveNurChartROW(clsDB.DbCon, this, nEMRNO, AcpEmr, pForm, strDT2.Replace("-", ""), strIT35.Replace(":", ""), strContent);
            }


            SQL = "";
            SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_SAPS3_SCORE(";
            SQL = SQL + ComNum.VBLF + " IPDNO,PANO,INDATE,EMRNO,";
            SQL = SQL + ComNum.VBLF + " IK1,IK2,IK3,IK4,";
            SQL = SQL + ComNum.VBLF + " IT1,IT2,DT1,IT3,";
            SQL = SQL + ComNum.VBLF + " IT4,IT7,IT5,IT8,";
            SQL = SQL + ComNum.VBLF + " IT10,IT11,IT12,IT13,";
            SQL = SQL + ComNum.VBLF + " IT14,IT15,IT16,IT17,";
            SQL = SQL + ComNum.VBLF + " IT18,IT19,IT20,IT21,";
            SQL = SQL + ComNum.VBLF + " IT22,IT23,IT24,IT26,";
            SQL = SQL + ComNum.VBLF + " IT27,IT25,IT28,IT29,";
            SQL = SQL + ComNum.VBLF + " IT30,IT31,IT32,IT33,";
            SQL = SQL + ComNum.VBLF + " IT34,IT36,IT37,DT2,";
            SQL = SQL + ComNum.VBLF + " IT35,IT38,IT39) VALUES (";
            SQL = SQL + ComNum.VBLF + strIPDNO + ", '" + strPano + "', TO_DATE('" + strInDate + "','YYYY-MM-DD'), " + nEMRNO + ", ";
            SQL = SQL + ComNum.VBLF + "'" + strIK1 + "','" + strIK2 + "','" + strIK3 + "','" + strIK4 + "', ";
            SQL = SQL + ComNum.VBLF + "'" + strIT1 + "','" + strIT2 + "','" + strDT1 + "','" + strIT3 + "', ";
            SQL = SQL + ComNum.VBLF + "'" + strIT4 + "','" + strIT7 + "','" + strIT5 + "','" + strIT8 + "', ";
            SQL = SQL + ComNum.VBLF + "'" + strIT10 + "','" + strIT11 + "','" + strIT12 + "','" + strIT13 + "', ";
            SQL = SQL + ComNum.VBLF + "'" + strIT14 + "','" + strIT15 + "','" + strIT16 + "','" + strIT17 + "', ";
            SQL = SQL + ComNum.VBLF + "'" + strIT18 + "','" + strIT19 + "','" + strIT20 + "','" + strIT21 + "', ";
            SQL = SQL + ComNum.VBLF + "'" + strIT22 + "','" + strIT23 + "','" + strIT24 + "','" + strIT26 + "', ";
            SQL = SQL + ComNum.VBLF + "'" + strIT27 + "','" + strIT25 + "','" + strIT28 + "','" + strIT29 + "', ";
            SQL = SQL + ComNum.VBLF + "'" + strIT30 + "','" + strIT31 + "','" + strIT32 + "','" + strIT33 + "', ";
            SQL = SQL + ComNum.VBLF + "'" + strIT34 + "','" + strIT36 + "','" + strIT37 + "','" + strDT2 + "', ";
            SQL = SQL + ComNum.VBLF + "'" + strIT35 + "','" + strIT38 + "','" + strIT39 + "') ";        //2019-02-13

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(SqlErr);
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        void Save_EMR_SOFA(string argIPDNO)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double nEMRNO = 0;
            string strInDate = "";
            string strPano = "";
            string strOutDate = "";
            string strOutTime = "";
            string strDeptCode = "";
            string strDrCd = "";
            string strXML = "";
            string strChartX1 = "";
            string strChartX2 = "";

            string strHead = "";
            string strTagHead = "";
            string strTagVal = "";
            string strTagTail = "";
            string strXMLCert = "";

            string strIPDNO = "";

            string strChartDate = "";
            string strChartTime = "";

            string strDT1 = "";
            string strIT1 = "";
            string strIT2 = "";
            string strIT3 = "";
            string strIR1 = "";
            string strIR2 = "";
            string strIT4 = "";
            string strIT5 = "";
            string strIT6 = "";
            string strIT7 = "";
            string strIT8 = "";
            string strIT10 = "";
            string strIR3 = "";
            string strIR4 = "";
            string strIT9 = "";
            string strIT11 = "";
            string strIT12 = "";
            string strIT13 = "";
            string strIT14 = "";
            string strIT15 = "";
            string strIT16 = "";
            string strIT17 = "";

            string strROWID = "";

            string strSysDate = "";
            string strSysTime = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            strChartDate = strSysDate.Replace("-", "");
            strChartTime = strSysTime.Replace("-", "") + "00";
            strDT1 = strSysDate;
            strIT1 = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);
            strIT2 = txtStepA1.Text.Trim();
            strIT3 = txtStepA2.Text.Trim();
            strIR1 = rdoS10.Checked == true ? "true" : "false";
            strIR2 = rdoS11.Checked == true ? "true" : "false";
            strIT4 = txtStepB1.Text.Trim();
            strIT5 = txtStepC1.Text.Trim();
            strIT6 = txtStepD1.Text.Trim();
            strIT7 = txtStepE1.Text.Trim();
            strIT8 = txtStepE2.Text.Trim();
            strIT10 = txtStepE3.Text.Trim();
            strIR3 = rdoS50.Checked == true ? "true" : "false";
            strIR4 = rdoS51.Checked == true ? "true" : "false";
            strIT9 = txtDrug1.Text.Trim();
            strIT11 = txtDrug2.Text.Trim();
            strIT12 = txtDrug3.Text.Trim();
            strIT13 = txtDrug4.Text.Trim();
            strIT14 = txtStepF1.Text.Trim();
            strIT15 = VB.Mid(cboUrin.Text.Trim(), 3, cboUrin.Text.Trim().Length);
            strIT16 = txtSopa.Text.Trim();
            strIT17 = lblGrade.Text.Trim();

            SQL = "";
            SQL = " SELECT PANO, SNAME, INDATE, OUTDATE, DRCODE, DEPTCODE, IPDNO ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
            SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIPDNO;

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("환자 정보가 없습니다. 전자차트 전송에 실패하였습니다.");
                return;
            }

            strPano = dt.Rows[0]["PANO"].ToString().Trim();
            strInDate = VB.Left(dt.Rows[0]["INDATE"].ToString().Trim(), 10);
            strOutDate = dt.Rows[0]["OUTDATE"].ToString().Trim();
            strDrCd = dt.Rows[0]["DRCODE"].ToString().Trim();
            strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
            strIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();

            dt.Dispose();
            dt = null;


            SQL = "";
            SQL = " SELECT A.ROWID ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_SOFASCOER A";
            SQL = SQL + ComNum.VBLF + " WHERE ENTDATE = (";
            SQL = SQL + ComNum.VBLF + "                  SELECT MAX(ENTDATE)";
            SQL = SQL + ComNum.VBLF + "                    FROM " + ComNum.DB_PMPA + "NUR_SOFASCOER";
            SQL = SQL + ComNum.VBLF + "                   WHERE IPDNO = " + strIPDNO + " )";
            SQL = SQL + ComNum.VBLF + "                     AND A.IPDNO = " + strIPDNO;

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;


            strXML = "";

            strHead = "<?xml version=" + VB.Chr(34) + "1.0" + VB.Chr(34) + " encoding=" + VB.Chr(34) + "UTF-8" + VB.Chr(34) + "?>";
            strChartX1 = "<chart>";
            strChartX2 = "</chart>";

            strXML = strHead + strChartX1;

            strTagHead = "<dt1 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "작성일자" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strDT1;
            strTagTail = "]]></dt1>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it1 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "작성자" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT1;
            strTagTail = "]]></it1>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it2 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "FIO2" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT2;
            strTagTail = "]]></it2>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it3 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "PaO2" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT3;
            strTagTail = "]]></it3>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<ir1 type=" + VB.Chr(34) + "inputRadio" + VB.Chr(34) + " label=" + VB.Chr(34) + "MV_No" + VB.Chr(34) + ">";
            strTagVal = strIR1;
            strTagTail = "</ir1>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<ir2 type=" + VB.Chr(34) + "inputRadio" + VB.Chr(34) + " label=" + VB.Chr(34) + "MV_Yes" + VB.Chr(34) + ">";
            strTagVal = strIR2;
            strTagTail = "</ir2>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it4 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Plateletes" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT4;
            strTagTail = "]]></it4>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it5 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Bilirubin" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT5;
            strTagTail = "]]></it5>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it6 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "GCS" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT6;
            strTagTail = "]]></it6>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it7 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "MAP_1" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT7;
            strTagTail = "]]></it7>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it8 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "MAP_2" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT8;
            strTagTail = "]]></it8>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it10 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "MAP_3" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT10;
            strTagTail = "]]></it10>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<ir3 type=" + VB.Chr(34) + "inputRadio" + VB.Chr(34) + " label=" + VB.Chr(34) + "Vasoo_No" + VB.Chr(34) + ">";
            strTagVal = strIR3;
            strTagTail = "</ir3>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<ir4 type=" + VB.Chr(34) + "inputRadio" + VB.Chr(34) + " label=" + VB.Chr(34) + "Vasoo_Yes" + VB.Chr(34) + ">";
            strTagVal = strIR4;
            strTagTail = "</ir4>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it9 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Dopamine" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT9;
            strTagTail = "]]></it9>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it11 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Dobutamine" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT11;
            strTagTail = "]]></it11>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it12 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Epinephrine" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT12;
            strTagTail = "]]></it12>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it13 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Norepinephrine" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT13;
            strTagTail = "]]></it13>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it14 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Creatinine" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT14;
            strTagTail = "]]></it14>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it15 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Urine" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT15;
            strTagTail = "]]></it15>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it16 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Score" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT16;
            strTagTail = "]]></it16>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strTagHead = "<it17 type=" + VB.Chr(34) + "inputText" + VB.Chr(34) + " label=" + VB.Chr(34) + "Mobility" + VB.Chr(34) + "><![CDATA[";
            strTagVal = strIT17;
            strTagTail = "]]></it17>";
            strXML = strXML + strTagHead + strTagVal + strTagTail;

            strXMLCert = strXML;

            strXML = strXML + strChartX2;

            strXMLCert = strXML;


            SQL = "";
            SQL = "SELECT " + ComNum.DB_EMR + "GetEmrXmlNo() FunSeqNo FROM Dual";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nEMRNO = VB.Val(dt.Rows[0]["FunSeqNo"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            OracleCommand cmd = new OracleCommand();

            PsmhDb pDbCon = clsDB.DbCon;

            cmd.Connection = pDbCon.Con;
            cmd.CommandText = ComNum.DB_EMR + "XMLINSRT3";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_EMRNO", OracleDbType.Double, 0, nEMRNO, ParameterDirection.Input);
            cmd.Parameters.Add("p_FORMNO", OracleDbType.Double, 0, 2658, ParameterDirection.Input);
            cmd.Parameters.Add("p_USEID", OracleDbType.Varchar2, 8, clsType.User.Sabun, ParameterDirection.Input);
            cmd.Parameters.Add("p_CHARTDATE", OracleDbType.Varchar2, 8, strChartDate, ParameterDirection.Input);
            cmd.Parameters.Add("p_CHARTTIME", OracleDbType.Varchar2, 6, strChartTime, ParameterDirection.Input);
            cmd.Parameters.Add("p_ACPNO", OracleDbType.Double, 0, 0, ParameterDirection.Input);
            cmd.Parameters.Add("p_PTNO", OracleDbType.Varchar2, 9, strPano, ParameterDirection.Input);
            cmd.Parameters.Add("p_INOUTCLS", OracleDbType.Varchar2, 1, "I", ParameterDirection.Input);
            cmd.Parameters.Add("p_MEDFRDATE", OracleDbType.Varchar2, 8, strInDate.Replace("-", ""), ParameterDirection.Input);
            cmd.Parameters.Add("p_MEDFRTIME", OracleDbType.Varchar2, 6, "120000", ParameterDirection.Input);
            cmd.Parameters.Add("p_MEDENDDATE", OracleDbType.Varchar2, 8, strOutDate.Replace("-", ""), ParameterDirection.Input);
            cmd.Parameters.Add("p_MEDENDTIME", OracleDbType.Varchar2, 6, strOutTime, ParameterDirection.Input);
            cmd.Parameters.Add("p_MEDDEPTCD", OracleDbType.Varchar2, 4, strDeptCode, ParameterDirection.Input);
            cmd.Parameters.Add("p_MEDDRCD", OracleDbType.Varchar2, 6, strDrCd, ParameterDirection.Input);
            cmd.Parameters.Add("p_MIBICHECK", OracleDbType.Varchar2, 1, "0", ParameterDirection.Input);
            cmd.Parameters.Add("p_WRITEDATE", OracleDbType.Varchar2, 8, strSysDate.Replace("-", ""), ParameterDirection.Input);
            cmd.Parameters.Add("p_WRITETIME", OracleDbType.Varchar2, 6, strSysTime.Replace(":", ""), ParameterDirection.Input);
            cmd.Parameters.Add("p_UPDATENO", OracleDbType.Varchar2, 0, 1, ParameterDirection.Input);
            cmd.Parameters.Add("p_CHARTXML", OracleDbType.Varchar2, strXML.Length, strXML, ParameterDirection.Input);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = null;



            SQL = "";
            SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_SOFASCOER SET ";
            SQL = SQL + ComNum.VBLF + " EMRNO = " + nEMRNO;
            SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(SqlErr);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strAge = "";
            string strWBC = "";
            string strCR = "";
            string strBilirubin = "";
            string strGCS = "";
            string strPH = "";
            string strPLT = "";

            string strPO2 = "";

            string strPano = "";
            string strInDate = "";

            string strResultCode = "";
            string strSDate = "";
            string strEdate = "";

            int j = 0;

            string strPulse = "";
            string strBPsys = "";
            string strBPdia = "";

            string StrTemp = "";
            string strChartDate = "";
            string strFio2 = "";

            string strMAXSbp = "";
            string strMAXDbp = "";
            string strMAXFio2 = "";
            string strMAXPO2 = "";
            string strMAXPLT = "";
            string strMAXBilirubin = "";
            string strMAXGCS = "";
            string strMAXCR = "";

            string strSysDate = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            for (i = 0; i < ssResult_Sheet1.ColumnCount; i++)
            {
                for (j = 0; j < ssResult_Sheet1.RowCount; j++)
                {
                    ssResult_Sheet1.Cells[j, i].Text = "";
                }
            }

            for (i = 0; i < ssEMRResult_Sheet1.ColumnCount; i++)
            {
                for (j = 0; j < ssEMRResult_Sheet1.RowCount; j++)
                {
                    ssEMRResult_Sheet1.Cells[j, i].Text = "";
                }
            }

            for (i = 0; i < ssEMRVent_Sheet1.ColumnCount; i++)
            {
                for (j = 0; j < ssEMRVent_Sheet1.RowCount; j++)
                {
                    ssEMRVent_Sheet1.Cells[j, i].Text = "";
                }
            }

            //ComFunc.MsgBox("★Lab결과는 중환자실 입실후 가장 최초의 결과값을  읽어 옵니다.");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT PANO, TO_CHAR(INDATE, 'YYYY-MM-DD') INDATE, AGE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + GstrIPDNO;    //VB - P(GstrHelpCode, "^^", 1)

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strPano = dt.Rows[0]["PANO"].ToString().Trim();
                    strInDate = dt.Rows[0]["INDATE"].ToString().Trim();
                    strAge = dt.Rows[0]["AGE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                strSDate = strInDate;
                strEdate = VB.Mid(strInDate, 1, 4) + "-12-31";

                Call_ArvTime(); //내원시간 EMR 간호기록 최초시간 호출

                strResultCode = "'CR61C','CR61E','CR61AV','HR01A' ,'CR42A','CR38C','HR01I','CR61AV' ";

                SQL = "";
                SQL = " SELECT /*+rule*/ A.SUBCODE, A.RESULT, A.UNIT, TO_CHAR(B.SENDDATE,'YY.MM.DD') OD, TO_CHAR(B.RESULTDATE,'YY.MM.DD') DD,EXAMYNAME";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_MED + "EXAM_RESULTC A, " + ComNum.DB_MED + "EXAM_SPECMST B, " + ComNum.DB_MED + "EXAM_MASTER D, ";
                SQL = SQL + ComNum.VBLF + "   (SELECT /*+leading(A)*/ min(ORDERDATE) ORDERDATE, SUBCODE";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_MED + "EXAM_RESULTC SubA, " + ComNum.DB_MED + "EXAM_SPECMST SubB";
                SQL = SQL + ComNum.VBLF + " WHERE SubA.SPECNO = SubB.SPECNO";
                SQL = SQL + ComNum.VBLF + "   AND SubB.PANO = SUBA.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND SubB.PANO  = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SubB.RESULTDATE >= TO_DATE('" + strSDate + " 00:00','YYYY-MM-DD HH24:MI') -2";
                SQL = SQL + ComNum.VBLF + "   AND SubB.RESULTDATE <= TO_DATE('" + strEdate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND SubA.SUBCODE IN (" + strResultCode + ")";
                SQL = SQL + ComNum.VBLF + "       GROUP BY SUBCODE) C";
                SQL = SQL + ComNum.VBLF + " WHERE A.SPECNO = B.SPECNO";
                SQL = SQL + ComNum.VBLF + "   AND B.PANO  = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND B.PANO = A.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND B.RESULTDATE >= TO_DATE('" + strSDate + " 00:00','YYYY-MM-DD HH24:MI') -2 ";
                SQL = SQL + ComNum.VBLF + "   AND B.RESULTDATE <= TO_DATE('" + strEdate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND B.ORDERDATE = C.ORDERDATE";
                SQL = SQL + ComNum.VBLF + "   AND A.SUBCODE = C.SUBCODE AND A.SUBCODE = D.MASTERCODE(+)";
                SQL = SQL + ComNum.VBLF + "   AND A.STATUS NOT IN ('N') ";
                //SQL = SQL + ComNum.VBLF + "      ORDER BY EXAMYNAME";
                SQL = SQL + ComNum.VBLF + "ORDER BY EXAMYNAME , B.RESULTDATE DESC  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssResult_Sheet1.RowCount = dt.Rows.Count;
                ssResult_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssResult_Sheet1.Cells[i, 0].Text = dt.Rows[i]["EXAMYNAME"].ToString().Trim();

                        if (dt.Rows[i]["SUBCODE"].ToString().Trim() == "CR61C")
                        {
                            ssResult_Sheet1.Cells[i, 0].Text = "PCO2";
                        }

                        if (dt.Rows[i]["SUBCODE"].ToString().Trim() == "CR61E")
                        {
                            ssResult_Sheet1.Cells[i, 0].Text = "PO2";
                        }

                        if (dt.Rows[i]["SUBCODE"].ToString().Trim() == "CR61AV")
                        {
                            ssResult_Sheet1.Cells[i, 0].Text = "PH";
                        }

                        ssResult_Sheet1.Cells[i, 1].Text = dt.Rows[i]["OD"].ToString().Trim();
                        ssResult_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DD"].ToString().Trim();
                        ssResult_Sheet1.Cells[i, 3].Text = dt.Rows[i]["RESULT"].ToString().Trim() + "  " + dt.Rows[i]["UNIT"].ToString().Trim();

                        switch (dt.Rows[i]["SUBCODE"].ToString().Trim().ToUpper())
                        {
                            case "HR01A":
                                strWBC = dt.Rows[i]["RESULT"].ToString().Trim();
                                ssResult_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(200, 100, 100); //WBC
                                break;
                            case "CR42A":
                                strCR = dt.Rows[i]["RESULT"].ToString().Trim();
                                ssResult_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(200, 100, 100);   //CR
                                break;
                            case "CR38C":
                                strBilirubin = dt.Rows[i]["RESULT"].ToString().Trim();
                                ssResult_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(200, 100, 100); //BILIRUMIN
                                break;
                            case "CR61E":
                                strPO2 = dt.Rows[i]["RESULT"].ToString().Trim();
                                ssResult_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(200, 100, 100);  //PO2
                                break;
                            case "HR01I":
                                strPLT = dt.Rows[i]["RESULT"].ToString().Trim();
                                ssResult_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(200, 100, 100);  //PLT
                                break;
                            case "CR61AV":
                                strPH = dt.Rows[i]["RESULT"].ToString().Trim();
                                ssResult_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(200, 100, 100);  //PH
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;


                strResultCode = "'CR61C','CR61E','CR61AV','HR01A' ,'CR42A','CR38C','HR01I','CR61AV' ";


                SQL = "";
                SQL = " SELECT /*+rule*/ A.SUBCODE, A.RESULT, A.UNIT, TO_CHAR(B.SENDDATE,'YY.MM.DD') OD, TO_CHAR(B.RESULTDATE,'YY.MM.DD') DD,EXAMYNAME";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_MED + "EXAM_RESULTC A, " + ComNum.DB_MED + "EXAM_SPECMST B," + ComNum.DB_MED + "EXAM_MASTER D, ";
                SQL = SQL + ComNum.VBLF + "   (SELECT /*+leading(A)*/ max(ORDERDATE) ORDERDATE, SUBCODE";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_MED + "EXAM_RESULTC SubA, " + ComNum.DB_MED + "EXAM_SPECMST SubB";
                SQL = SQL + ComNum.VBLF + " WHERE SubA.SPECNO = SubB.SPECNO";
                SQL = SQL + ComNum.VBLF + "   AND SubB.PANO = SUBA.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND SubB.PANO  = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SubB.RESULTDATE >= TO_DATE('" + strSDate + " 00:00','YYYY-MM-DD HH24:MI') -2";
                SQL = SQL + ComNum.VBLF + "   AND SubB.RESULTDATE <= TO_DATE('" + strEdate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND SubA.SUBCODE IN (" + strResultCode + ")";
                SQL = SQL + ComNum.VBLF + "       GROUP BY SUBCODE) C";
                SQL = SQL + ComNum.VBLF + " WHERE A.SPECNO = B.SPECNO";
                SQL = SQL + ComNum.VBLF + "   AND B.PANO  = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND B.PANO = A.PANO ";
                SQL = SQL + ComNum.VBLF + "   AND B.RESULTDATE >= TO_DATE('" + strSDate + " 00:00','YYYY-MM-DD HH24:MI') -2 ";
                SQL = SQL + ComNum.VBLF + "   AND B.RESULTDATE <= TO_DATE('" + strEdate + " 23:59','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "   AND B.ORDERDATE = C.ORDERDATE";
                SQL = SQL + ComNum.VBLF + "   AND A.SUBCODE = C.SUBCODE AND A.SUBCODE = D.MASTERCODE(+)";
                SQL = SQL + ComNum.VBLF + "   AND A.STATUS NOT IN ('N') ";
                //SQL = SQL + ComNum.VBLF + "      ORDER BY EXAMYNAME";
                SQL = SQL + ComNum.VBLF + "ORDER BY EXAMYNAME , B.RESULTDATE DESC  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssResult_Sheet1.RowCount = dt.Rows.Count;
                ssResult_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (dt.Rows[i]["SUBCODE"].ToString().Trim().ToUpper())
                        {
                            case "HR01A":
                                strWBC = dt.Rows[i]["RESULT"].ToString().Trim();
                                ssResult_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(200, 100, 100); //WBC
                                break;
                            case "CR42A":
                                strMAXCR = dt.Rows[i]["RESULT"].ToString().Trim();
                                ssResult_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(200, 100, 100);   //CR
                                break;
                            case "CR38C":
                                strMAXBilirubin = dt.Rows[i]["RESULT"].ToString().Trim();
                                ssResult_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(200, 100, 100); //BILIRUMIN
                                break;
                            case "CR61E":
                                strMAXPO2 = dt.Rows[i]["RESULT"].ToString().Trim();
                                ssResult_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(200, 100, 100);  //PO2
                                break;
                            case "HR01I":
                                strMAXPLT = dt.Rows[i]["RESULT"].ToString().Trim();
                                ssResult_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(200, 100, 100);  //PLT
                                break;
                            case "CR61AV":
                                strPH = dt.Rows[i]["RESULT"].ToString().Trim();
                                ssResult_Sheet1.Cells[i, 3].BackColor = Color.FromArgb(200, 100, 100);  //PH
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = " SELECT A.CHARTDATE || ' ' || substr(A.CHARTTIME, 1, 4)  CHARTDATE,";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ta7') PULSE,";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ta25') GCS,";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ta2') BPSYS,";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ta3') BPDIA,";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ta10') TEMP,";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ta8') BREATH";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A, KOSMOS_EMR.EMRXML B";
                SQL = SQL + ComNum.VBLF + " WHERE A.CHARTDATE >= '" + strInDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE <= '" + strSysDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.FORMNO in ( '1722' ,'2599') ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + strPano + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.EMRNO = B.EMRNO";

                SQL = SQL + ComNum.VBLF + " UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT A.CHARTDATE ||' '||substr(A.CHARTTIME,1,4)  CHARTDATE,";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND ITEMNO = 'I0000014815')   PULSE,";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND ITEMNO = 'I0000034033')   GCS,";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND ITEMNO = 'I0000002018')   BPSYS,";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND ITEMNO = 'I0000001765')   BPDIA,";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND ITEMNO = 'I0000001811')   TEMP,";
                SQL = SQL + ComNum.VBLF + " (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND ITEMNO = 'I0000002009')   BREATH";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + " WHERE EXISTS";
                SQL = SQL + ComNum.VBLF + " (";
                SQL = SQL + ComNum.VBLF + "   SELECT 1";
                SQL = SQL + ComNum.VBLF + "     FROM KOSMOS_EMR.AEMRCHARTMST A2";
                SQL = SQL + ComNum.VBLF + "       INNER JOIN KOSMOS_EMR.AEMRCHARTROW R";
                SQL = SQL + ComNum.VBLF + "          ON R.EMRNO = A2.EMRNO";
                SQL = SQL + ComNum.VBLF + "         AND R.EMRNOHIS = A2.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "         AND R.ITEMNO = 'I0000034033'";
                SQL = SQL + ComNum.VBLF + "         AND R.ITEMVALUE > CHR(0)";
                SQL = SQL + ComNum.VBLF + "       INNER JOIN KOSMOS_EMR.AEMRCHARTROW R";
                SQL = SQL + ComNum.VBLF + "          ON R.EMRNO = A2.EMRNO";
                SQL = SQL + ComNum.VBLF + "         AND R.EMRNOHIS = A2.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "         AND R.ITEMNO = 'I0000002018'";
                SQL = SQL + ComNum.VBLF + "         AND R.ITEMVALUE > CHR(0)";
                SQL = SQL + ComNum.VBLF + "       WHERE CHARTDATE >= '" + strInDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "         AND CHARTDATE <= '" + strSysDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "         AND FORMNO = 3150 ";
                SQL = SQL + ComNum.VBLF + "         AND PTNO = '" + strPano + "'";
                SQL = SQL + ComNum.VBLF + "         AND CHARTUSEID <> '합계'";
                SQL = SQL + ComNum.VBLF + "         AND A2.EMRNO = A.EMRNO";
                SQL = SQL + ComNum.VBLF + " )";
                //SQL = SQL + ComNum.VBLF + "  ORDER BY A.CHARTDATE ASC, A.CHARTTIME ASC";
                SQL = SQL + ComNum.VBLF + "  ORDER BY CHARTDATE DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssEMRResult_Sheet1.RowCount = 4;
                    ssEMRResult_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    strChartDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                    strPulse = dt.Rows[0]["PULSE"].ToString().Trim();
                    strBPsys = dt.Rows[0]["BPSYS"].ToString().Trim();   //SBP
                    strBPdia = dt.Rows[0]["BPDIA"].ToString().Trim();   //DBP

                    strMAXSbp = dt.Rows[dt.Rows.Count - 1]["BPSYS"].ToString().Trim();
                    strMAXDbp = dt.Rows[dt.Rows.Count - 1]["BPDIA"].ToString().Trim();
                    strMAXGCS = dt.Rows[dt.Rows.Count - 1]["GCS"].ToString().Trim();

                    StrTemp = dt.Rows[0]["TEMP"].ToString().Trim();
                    strGCS = dt.Rows[0]["GCS"].ToString().Trim();

                    ssEMRResult_Sheet1.Cells[0, 0].Text = "GCS";
                    ssEMRResult_Sheet1.Cells[0, 1].Text = strChartDate;
                    ssEMRResult_Sheet1.Cells[0, 2].Text = strGCS;

                    ssEMRResult_Sheet1.Cells[1, 0].Text = "체온";
                    ssEMRResult_Sheet1.Cells[1, 1].Text = strChartDate;
                    ssEMRResult_Sheet1.Cells[1, 2].Text = StrTemp;

                    ssEMRResult_Sheet1.Cells[2, 0].Text = "PULSE";
                    ssEMRResult_Sheet1.Cells[2, 1].Text = strChartDate;
                    ssEMRResult_Sheet1.Cells[2, 2].Text = strPulse;

                    ssEMRResult_Sheet1.Cells[3, 0].Text = "SBP";
                    ssEMRResult_Sheet1.Cells[3, 1].Text = strChartDate;
                    ssEMRResult_Sheet1.Cells[3, 2].Text = strBPsys;
                }

                dt.Dispose();
                dt = null;


                SQL = "";
                SQL = " SELECT A.CHARTDATE||' '||substr(A.CHARTTIME,1,4)  CHARTDATE , ";
                SQL = SQL + ComNum.VBLF + " extractValue(chartxml, '//ta4') Fio2";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXMLMST A, " + ComNum.DB_EMR + "EMRXML B";
                SQL = SQL + ComNum.VBLF + " WHERE A.CHARTDATE >= '" + FStICUdte.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.CHARTDATE <= '" + strSysDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.FORMNO in ( '2598') ";
                SQL = SQL + ComNum.VBLF + "   AND A.PTNO = '" + strPano + "'";
                SQL = SQL + ComNum.VBLF + "   AND A.EMRNO = B.EMRNO";
                //SQL = SQL + ComNum.VBLF + "  ORDER BY A.CHARTDATE ASC, A.CHARTTIME ASC";
                SQL = SQL + ComNum.VBLF + "  ORDER BY A.CHARTDATE || A.CHARTTIME DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strMAXFio2 = dt.Rows[0]["Fio2"].ToString().Trim();

                    ssEMRVent_Sheet1.RowCount = 1;
                    strChartDate = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                    strFio2 = dt.Rows[0]["Fio2"].ToString().Trim();

                    ssEMRVent_Sheet1.Cells[0, 0].Text = "Fio2";
                    ssEMRVent_Sheet1.Cells[0, 1].Text = strChartDate;
                    ssEMRVent_Sheet1.Cells[0, 2].Text = strFio2;
                }

                dt.Dispose();
                dt = null;

                if (VB.Val(strAge) < 40)
                {
                    ssView_Sheet1.Cells[5, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[5, 5].CellType).Items[0].ToString();

                }
                else if (VB.Val(strAge) < 60)
                {
                    ssView_Sheet1.Cells[5, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[5, 5].CellType).Items[1].ToString();
                }
                else if (VB.Val(strAge) < 70)
                {
                    ssView_Sheet1.Cells[5, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[5, 5].CellType).Items[2].ToString();
                }
                else if (VB.Val(strAge) < 75)
                {
                    ssView_Sheet1.Cells[5, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[5, 5].CellType).Items[3].ToString();
                }
                else if (VB.Val(strAge) < 80)
                {
                    ssView_Sheet1.Cells[5, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[5, 5].CellType).Items[4].ToString();
                }
                else if (VB.Val(strAge) >= 80)
                {
                    ssView_Sheet1.Cells[5, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[5, 5].CellType).Items[5].ToString();
                }

                if (VB.Val(strGCS) >= 13)
                {
                    ssView_Sheet1.Cells[29, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[29, 5].CellType).Items[4].ToString();
                }
                else if (VB.Val(strGCS) >= 7 && VB.Val(strGCS) <= 12)
                {
                    ssView_Sheet1.Cells[29, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[29, 5].CellType).Items[3].ToString();
                }
                else if (VB.Val(strGCS) == 6)
                {
                    ssView_Sheet1.Cells[29, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[29, 5].CellType).Items[2].ToString();
                }
                else if (VB.Val(strGCS) == 5)
                {
                    ssView_Sheet1.Cells[29, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[29, 5].CellType).Items[1].ToString();
                }
                else if (VB.Val(strGCS) >= 3 && VB.Val(strGCS) <= 4)
                {
                    ssView_Sheet1.Cells[29, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[29, 5].CellType).Items[0].ToString();
                }

                if (strBilirubin != "")
                {
                    if (VB.Val(strBilirubin) < 2)
                    {
                        ssView_Sheet1.Cells[30, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[30, 5].CellType).Items[0].ToString();
                    }
                    else if (VB.Val(strBilirubin) < 6)
                    {
                        ssView_Sheet1.Cells[30, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[30, 5].CellType).Items[1].ToString();
                    }
                    else if (VB.Val(strBilirubin) >= 6)
                    {
                        ssView_Sheet1.Cells[30, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[30, 5].CellType).Items[2].ToString();
                    }
                }

                if (StrTemp != "")
                {
                    if (VB.Val(StrTemp) < 35)
                    {
                        ssView_Sheet1.Cells[31, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[31, 5].CellType).Items[0].ToString();
                    }
                    else if (VB.Val(StrTemp) >= 35)
                    {
                        ssView_Sheet1.Cells[31, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[31, 5].CellType).Items[1].ToString();
                    }
                }

                if (strCR != "")
                {
                    if (VB.Val(strCR) < 1.2)
                    {
                        ssView_Sheet1.Cells[32, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[32, 5].CellType).Items[0].ToString();
                    }
                    else if (VB.Val(strCR) < 2)
                    {
                        ssView_Sheet1.Cells[32, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[32, 5].CellType).Items[1].ToString();
                    }
                    else if (VB.Val(strCR) < 3.5)
                    {
                        ssView_Sheet1.Cells[32, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[32, 5].CellType).Items[2].ToString();
                    }
                    else if (VB.Val(strCR) >= 3.5)
                    {
                        ssView_Sheet1.Cells[32, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[32, 5].CellType).Items[3].ToString();
                    }
                }

                if (strPulse != "")
                {
                    if (VB.Val(strPulse) < 120)
                    {
                        ssView_Sheet1.Cells[33, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[33, 5].CellType).Items[0].ToString();
                    }
                    else if (VB.Val(strPulse) >= 120 && VB.Val(strPulse) < 160)
                    {
                        ssView_Sheet1.Cells[33, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[33, 5].CellType).Items[1].ToString();
                    }
                    else if (VB.Val(strPulse) >= 160)
                    {
                        ssView_Sheet1.Cells[33, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[33, 5].CellType).Items[2].ToString();
                    }
                }

                if (strWBC != "")
                {
                    if (VB.Val(strWBC) < 15)
                    {
                        ssView_Sheet1.Cells[34, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[34, 5].CellType).Items[0].ToString();
                    }
                    else if (VB.Val(strWBC) >= 15)
                    {
                        ssView_Sheet1.Cells[34, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[34, 5].CellType).Items[1].ToString();
                    }
                }

                if (strPH != "")
                {
                    if (VB.Val(strPH) > 7.25)
                    {
                        ssView_Sheet1.Cells[35, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[35, 5].CellType).Items[1].ToString();
                    }
                    else if (VB.Val(strPH) <= 7.25)
                    {
                        ssView_Sheet1.Cells[35, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[35, 5].CellType).Items[0].ToString();
                    }
                }

                if (strPLT != "")
                {
                    if (VB.Val(strPLT) < 20)
                    {
                        ssView_Sheet1.Cells[36, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[36, 5].CellType).Items[0].ToString();
                    }
                    else if (VB.Val(strPLT) >= 20 && VB.Val(strPLT) < 50)
                    {
                        ssView_Sheet1.Cells[36, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[36, 5].CellType).Items[1].ToString();
                    }
                    else if (VB.Val(strPLT) >= 50 && VB.Val(strPLT) < 100)
                    {
                        ssView_Sheet1.Cells[36, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[36, 5].CellType).Items[2].ToString();
                    }
                    else if (VB.Val(strPLT) >= 100)
                    {
                        ssView_Sheet1.Cells[36, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[36, 5].CellType).Items[3].ToString();
                    }
                }

                if (strBPsys != "")
                {
                    if (VB.Val(strBPsys) < 40)
                    {
                        ssView_Sheet1.Cells[37, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[37, 5].CellType).Items[0].ToString();
                    }
                    else if (VB.Val(strBPsys) >= 40 && VB.Val(strBPsys) < 70)
                    {
                        ssView_Sheet1.Cells[37, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[37, 5].CellType).Items[1].ToString();
                    }
                    else if (VB.Val(strBPsys) >= 70 && VB.Val(strBPsys) < 120)
                    {
                        ssView_Sheet1.Cells[37, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[37, 5].CellType).Items[2].ToString();
                    }
                    else if (VB.Val(strBPsys) >= 120)
                    {
                        ssView_Sheet1.Cells[37, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[37, 5].CellType).Items[3].ToString();
                    }
                }

                ssView_White();
                ss_Jumsu_Acc();
                ss_AVG_Acc();

                txtStepA1.Text = strMAXFio2;
                txtStepA2.Text = strMAXPO2;

                if (strMAXFio2 != "")
                {
                    rdoS10.Checked = true;
                    rdoS11.Checked = false;
                }
                else
                {
                    rdoS10.Checked = false;
                    rdoS11.Checked = true;
                }

                txtStepB1.Text = strMAXPLT;
                txtStepC1.Text = strMAXBilirubin;
                txtStepD1.Text = strMAXGCS;

                txtStepE2.Text = VB.Val(strMAXSbp).ToString();
                txtStepE3.Text = VB.Val(strMAXDbp).ToString();

                txtStepE1.Text = ((2 * VB.Val(strMAXSbp) + VB.Val(strMAXDbp)) / 3).ToString("#0.0");

                rdoS50.Checked = true;
                rdoS51.Checked = false;

                txtStepF1.Text = strMAXCR;

                cboUrin.SelectedIndex = 0;

                txtSopa.Text = "";
                lblGrade.Text = "";

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void btnGetExam_Click(object sender, EventArgs e)
        {

        }

        private void btnGetOrder_Click(object sender, EventArgs e)
        {
            ss_Clear();
        }

        private void btnGetSBck_Click(object sender, EventArgs e)
        {
            string strPano = "";
            string strSName = "";
            string strWard = "";
            string strRoom = "";
            string strJumin = "";
            string strInDate = "";
            string strInTime = "";
            string strDiagnosis = "";

            strPano = ssView_Sheet1.Cells[1, 0].Text;
            strSName = ssView_Sheet1.Cells[1, 1].Text;
            strWard = ssView_Sheet1.Cells[1, 2].Text;
            strRoom = ssView_Sheet1.Cells[1, 3].Text;
            strJumin = ssView_Sheet1.Cells[1, 4].Text;
            strInDate = ssView_Sheet1.Cells[1, 5].Text;
            strInTime = ssView_Sheet1.Cells[1, 6].Text;
            strDiagnosis = ssView_Sheet1.Cells[1, 7].Text;

            //FrmSepisisUnit
            frmSepisisUnit frm = new frmSepisisUnit(strPano, strSName, strWard, strRoom, strJumin, strInDate, strInTime, strDiagnosis, GstrIPDNO);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
            frm = null;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 15, 15);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, false, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save_Data();
        }

        void Save_Data()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strInDate = "";

            string strSAPSWard = "";
            string strSAPSRoom = "";
            string strSAPSDiag = "";
            string strwkGubn = "";
            string strExcept_Ex = "";

            string strItem_A = "";
            string strItem_B = "";
            string strItem_C = "";
            string strSCORE = "";
            string strAvg = "";


            strSAPSWard = ssView_Sheet1.Cells[1, 2].Text;
            strSAPSRoom = ssView_Sheet1.Cells[1, 3].Text;

            strSAPSDiag = ssView_Sheet1.Cells[1, 7].Text;
            strInDate = ssView_Sheet1.Cells[1, 5].Text;
            strInDate = strInDate + " " + ssView_Sheet1.Cells[1, 6].Text;

            strExcept_Ex = Convert.ToBoolean(ssView_Sheet1.Cells[3, 6].Value) == true ? "1" : "0";
            strExcept_Ex = strExcept_Ex + (Convert.ToBoolean(ssView_Sheet1.Cells[3, 7].Value) == true ? "1" : "0");
            strExcept_Ex = strExcept_Ex + (Convert.ToBoolean(ssView_Sheet1.Cells[3, 8].Value) == true ? "1" : "0");
            strExcept_Ex = strExcept_Ex + (Convert.ToBoolean(ssView_Sheet1.Cells[3, 9].Value) == true ? "1" : "0");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT DRCODE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE TRIM(DOCCODE) = '" + clsType.User.Sabun + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strwkGubn = "DR";
                }
                else
                {
                    strwkGubn = "NR";
                }

                dt.Dispose();
                dt = null;

                if (ComFunc.MsgBoxQ("저장하시겠습니까?") == DialogResult.No)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (clsType.User.Sabun == "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxQ("사번 오류입니다. 프로그램을 종류 후 다시 시작하시기 바랍니다.");
                    return;
                }

                for (i = 5; i < 16; i++)
                {
                    if (ssView_Sheet1.Cells[i, 5].CellType.ToString() == "ComboBoxCellType")
                    {
                        strItem_A = strItem_A + clsSpread.gSpreadComboindex(ssView, i, 5, ssView_Sheet1.Cells[i, 5].Text.Trim()) + "@";
                    }
                }

                for (i = 17; i < 28; i++)
                {
                    if (ssView_Sheet1.Cells[i, 5].CellType.ToString() == "ComboBoxCellType")
                    {
                        strItem_B = strItem_B + clsSpread.gSpreadComboindex(ssView, i, 5, ssView_Sheet1.Cells[i, 5].Text.Trim()) + "@";
                    }
                }

                for (i = 29; i < 39; i++)
                {
                    if (ssView_Sheet1.Cells[i, 5].CellType.ToString() == "ComboBoxCellType")
                    {
                        strItem_C = strItem_C + clsSpread.gSpreadComboindex(ssView, i, 5, ssView_Sheet1.Cells[i, 5].Text.Trim()) + "@";
                    }
                }

                strSCORE = ssView_Sheet1.Cells[39, 5].Text;
                strAvg = ssView_Sheet1.Cells[40, 5].Text.Replace("%", "");


                SQL = "";
                SQL = "UPDATE " + ComNum.DB_PMPA + "NUR_MASTER SET ";
                SQL = SQL + ComNum.VBLF + " EntSabun = " + clsType.User.Sabun + "  , ";
                SQL = SQL + ComNum.VBLF + " SAPS_AVG=" + strAvg + ",";
                SQL = SQL + ComNum.VBLF + " SAPS_SCORE=" + strSCORE + ", ";
                SQL = SQL + ComNum.VBLF + " SAPS_WARD = '" + strSAPSWard + "', ";
                SQL = SQL + ComNum.VBLF + " SAPS_ROOM = '" + strSAPSRoom + "', ";
                SQL = SQL + ComNum.VBLF + " SAPS_DIAG = '" + strSAPSDiag + "', ";

                if (strwkGubn == "DR")
                {
                    SQL = SQL + ComNum.VBLF + " SAPS_ENTSABUN = " + clsType.User.Sabun + ", ";
                    SQL = SQL + ComNum.VBLF + " SAPS_ENTDATE = NVL(SAPS_ENTDATE,SYSDATE), ";
                }

                if (FstrSign == "OK") //저장완료후 추후 수정안되게 수정 2016-12.26 kyo (김정숙과장 요청)
                {
                    SQL = SQL + ComNum.VBLF + " SAPS_SIGN = '" + clsType.User.Sabun + "', ";
                }

                SQL = SQL + ComNum.VBLF + " SAPS_INDATE = '" + strInDate + "', ";
                SQL = SQL + ComNum.VBLF + " EXCEPT_EX = '" + strExcept_Ex + "', ";
                SQL = SQL + ComNum.VBLF + " SAPS_ITEM_A = '" + strItem_A + "', ";
                SQL = SQL + ComNum.VBLF + " SAPS_ITEM_B = '" + strItem_B + "', ";
                SQL = SQL + ComNum.VBLF + " SAPS_ITEM_C = '" + strItem_C + "' ";

                if (GstrROWID == "")
                {
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO= " + GstrIPDNO; //VB - P(GstrHelpCode, "^^", 1)
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID='" + GstrROWID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                Save_EMR(GstrIPDNO);

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                if (rSetGstrFlag != null)
                {
                    rSetGstrFlag("SAVE");
                }

                ComFunc.MsgBox("저장되었습니다.");
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        void ss_Clear()
        {
            int i = 0;

            for (i = 5; i < 39; i++)
            {
                if (i == 24)    //ListControl.SelectedIndex의 default index가 0이 아닌 row
                {
                    ssView_Sheet1.Cells[i, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[i, 5].CellType).Items[4].ToString();
                }
                else if (i == 29)
                {
                    ssView_Sheet1.Cells[i, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[i, 5].CellType).Items[4].ToString();
                }
                else if (i == 31)
                {
                    ssView_Sheet1.Cells[i, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[i, 5].CellType).Items[1].ToString();
                }
                else if (i == 35)
                {
                    ssView_Sheet1.Cells[i, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[i, 5].CellType).Items[1].ToString();
                }
                else if (i == 36)
                {
                    ssView_Sheet1.Cells[i, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[i, 5].CellType).Items[3].ToString();
                }
                else if (i == 37)
                {
                    ssView_Sheet1.Cells[i, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[i, 5].CellType).Items[3].ToString();
                }
                else if (i == 38)
                {
                    ssView_Sheet1.Cells[i, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[i, 5].CellType).Items[3].ToString();
                }
                else
                {
                    if (ssView_Sheet1.Cells[i, 5].CellType != null && ssView_Sheet1.Cells[i, 5].CellType.ToString() == "ComboBoxCellType")
                    {
                        ssView_Sheet1.Cells[i, 5].Value = ((ComboBoxCellType)ssView_Sheet1.Cells[i, 5].CellType).Items[0].ToString();
                    }
                }
            }

            ssView_White();
            ss_Jumsu_Acc();
            ss_AVG_Acc();
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            if (ComFunc.MsgBoxQ("저장 완료 시 추후 수정이 불가능하게 됩니다. 진행하시겠습니까? ") == DialogResult.No)
            {
                return;
            }
            else
            {
                FstrSign = "OK";
                Save_Data();
                FstrSign = "";
            }
        }

        private void btnSofaClear_Click(object sender, EventArgs e)
        {
            Sofa_Clear();
        }

        void Sofa_Clear()
        {
            txtStepA1.Text = "";
            txtStepA2.Text = "";
            rdoS10.Checked = true;
            rdoS11.Checked = false;
            txtStepB1.Text = "";
            txtStepC1.Text = "";
            txtStepD1.Text = "";
            txtStepE1.Text = "";
            txtStepE2.Text = "";
            txtStepE3.Text = "";
            txtDrug1.Text = "";
            txtDrug2.Text = "";
            txtDrug3.Text = "";
            txtDrug4.Text = "";
            rdoS50.Checked = true;
            rdoS51.Checked = false;
            txtStepF1.Text = "";
            txtSopa.Text = "";
            lblGrade.Text = "";
        }

        private void btnSofaSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            double nStep1 = 0;
            double nStep2 = 0;
            double nStep3 = 0;
            double nStep4 = 0;
            double nStep5 = 0;
            double nStep6 = 0;
            double nScore = 0;

            if (VB.Val(txtStepA2.Text) == 0 || (rdoS10.Checked == false && rdoS11.Checked == false) || VB.Val(txtStepB1.Text) == 0 || VB.Val(txtStepC1.Text) == 0 || VB.Val(txtStepD1.Text) == 0 || VB.Val(txtStepE1.Text) == 0 || (rdoS50.Checked == false && rdoS51.Checked == false) || VB.Val(txtStepF1.Text) == 0)
            {
                ComFunc.MsgBoxQ("계산할 수치가 적절하지 않습니다.");
                return;
            }

            nStep1 = (VB.Val(txtStepA2.Text) / VB.Val((txtStepA1.Text != "" ? txtStepA1.Text : "21"))) * 100;
            nStep2 = VB.Val(txtStepB1.Text) * 1000;
            nStep3 = VB.Val(txtStepC1.Text);
            nStep4 = VB.Val(txtStepD1.Text);
            nStep5 = VB.Val(txtStepE1.Text);
            nStep6 = VB.Val(txtStepF1.Text);

            if (nStep1 <= 100 && rdoS11.Checked == true)
            {
                nStep1 = 4;
            }
            else if (nStep1 <= 200 && rdoS11.Checked == true)   //1.Neurological PaO 2 : FiO 2 비율
            {
                nStep1 = 3;
            }
            else if (nStep1 <= 300)
            {
                nStep1 = 2;
            }
            else if (nStep1 <= 400)
            {
                nStep1 = 1;
            }
            else
            {
                nStep1 = 0;
            }

            if (nStep2 <= 150)        //2.Coagulition  혈소판> 150 (x10 3 / mm 3 )
            {
                nStep2 = 1;
            }
            else if (nStep2 <= 100)
            {
                nStep2 = 2;
            }
            else if (nStep2 <= 50)
            {
                nStep2 = 3;
            }
            else if (nStep2 <= 20)
            {
                nStep2 = 4;
            }
            else
            {
                nStep2 = 0;
            }

            if (nStep3 >= 1.2 && nStep3 <= 1.9)   //3.Liver  빌리블린 <1.2 mg / dL
            {
                nStep3 = 1;
            }
            else if (nStep3 >= 2 && nStep3 <= 5.9)
            {
                nStep3 = 2;
            }
            else if (nStep3 >= 6 && nStep3 <= 11.9)
            {
                nStep3 = 3;
            }
            else if (nStep3 >= 12)
            {
                nStep3 = 4;
            }
            else
            {
                nStep3 = 0;
            }

            if (nStep4 >= 13 && nStep4 <= 14)     //4.Neurological  글라스 고 코 혼수 점수 10 - 12
            {
                nStep4 = 1;
            }
            else if (nStep4 >= 10 && nStep4 <= 12)
            {
                nStep4 = 2;
            }
            else if (nStep4 >= 6 && nStep4 <= 9)
            {
                nStep4 = 3;
            }
            else if (nStep4 < 6)
            {
                nStep4 = 4;
            }
            else
            {
                nStep4 = 0;
            }

            if (nStep5 <= 70)                //5.Cardiovascular   MAP <70 mmHg (프레 서 없음)
            {
                nStep5 = 1;
            }
            else
            {
                if (rdoS51.Checked == true)
                {
                    if (VB.Val(txtDrug1.Text) <= 5 || txtDrug2.Text != "")
                        nStep5 = 2;
                    else if (VB.Val(txtDrug1.Text) > 5 || VB.Val(txtDrug3.Text) <= 0.1 || VB.Val(txtDrug4.Text) <= 0.1)
                        nStep5 = 3;
                    else if (VB.Val(txtDrug1.Text) > 15 || VB.Val(txtDrug3.Text) > 0.1 || VB.Val(txtDrug4.Text) > 0.1)
                        nStep5 = 4;
                }

                nStep5 = 0;
            }



            if (nStep6 >= 1.2 && nStep3 <= 1.9)                   //6.Renai  크레아티닌 <1.2 mg / dL
            {
                nStep6 = 1;
            }
            else if (nStep6 >= 2 && nStep3 <= 3.4)
            {
                nStep6 = 2;
            }
            else if (nStep6 >= 3.5 && nStep3 <= 4.9)
            {
                nStep6 = 3;
            }
            else if (nStep6 >= 5)
            {
                nStep6 = 4;
            }
            else
            {
                nStep6 = 0;
            }

            if (VB.Left(cboUrin.Text, 1) == "1")
            {
                nStep6 = 3;
            }
            else if (VB.Left(cboUrin.Text, 1) == "2")
            {
                nStep6 = 4;
            }

            nScore = nStep1 + nStep2 + nStep3 + nStep4 + nStep5 + nStep6;

            txtSopa.Text = nScore.ToString();

            if (nScore >= 0 && nScore <= 6)
            {
                lblGrade.Text = " (<10%)";
            }
            else if (nScore >= 7 && nScore <= 9)
            {
                lblGrade.Text = " (15-20%)";
            }
            else if (nScore >= 10 && nScore <= 12)
            {
                lblGrade.Text = " (40-50%)";
            }
            else if (nScore >= 13 && nScore <= 14)
            {
                lblGrade.Text = " (50-60%)";
            }
            else if (nScore == 15)
            {
                lblGrade.Text = " (>80%)";
            }
            else if (nScore >= 16 && nScore <= 24)
            {
                lblGrade.Text = " (>90%)";
            }
            else
            {
                lblGrade.Text = "";
            }

            Sopa_Save();

            Read_Progress(VB.Val(GstrIPDNO));   //VB - P(GstrHelpCode, "^^", 1)
        }

        void Sopa_Save()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string StrOptionS1 = "";
            string StrOptionS2 = "";
            string strPano = "";
            string strSAPSWard = "";

            strSAPSWard = ssView_Sheet1.Cells[1, 2].Text;
            strPano = ssView_Sheet1.Cells[1, 0].Text;
            strSAPSWard = ssView_Sheet1.Cells[1, 2].Text;

            if (rdoS10.Checked == true)
            {
                StrOptionS1 = "0";
            }
            else
            {
                StrOptionS1 = "1";
            }

            if (rdoS50.Checked == true)
            {
                StrOptionS2 = "0";
            }
            else
            {
                StrOptionS2 = "1";
            }

            if (ComFunc.MsgBoxQ("SOFA 결과값을 저장하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            if (clsType.User.Sabun == "")
            {
                ComFunc.MsgBox("사번오류입니다. 프로그램을 종류 후 다시 시작하시기 바랍니다.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_SOFASCOER (";
                SQL = SQL + ComNum.VBLF + " PANO, BUN1, BUN2, BUN3,BUN4,BUN5,BUN6,BUN7,BUN8,BUN9,BUN10,BUN11,BUN12,BUN13,";
                SQL = SQL + ComNum.VBLF + " BUN14,TOTAL ,Grade,IPDNO,INWARD,ENTDATE,ENTSABUN ) VALUES (";
                SQL = SQL + ComNum.VBLF + "'" + strPano + "','" + txtStepA1.Text + "', '" + txtStepA2.Text + "', '" + StrOptionS1 + "',";
                SQL = SQL + ComNum.VBLF + "'" + txtStepB1.Text + "','" + txtStepC1.Text + "','" + txtStepD1.Text + "','" + txtStepE1.Text + "', ";
                SQL = SQL + ComNum.VBLF + "'" + StrOptionS2 + "','" + txtDrug1.Text + "','" + txtDrug2.Text + "','" + txtDrug3.Text + "', ";
                SQL = SQL + ComNum.VBLF + "'" + txtDrug4.Text + "','" + txtStepF1.Text + "','" + VB.Left(cboUrin.Text, 1) + "','" + txtSopa.Text + "','" + lblGrade.Text + "', ";
                SQL = SQL + ComNum.VBLF + GstrIPDNO + ",'" + strSAPSWard + "', sysdate , " + clsType.User.Sabun + ") "; //VB - P(GstrHelpCode, "^^", 1) -> GstrIPDNO

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                Save_EMR_SOFA(GstrIPDNO);   //VB - P(GstrHelpCode, "^^", 1) -> GstrIPDNO

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("SOFA Score가 저장되었습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        void Read_Progress(double argIPDNO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssSofa_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //의뢰일시 , 접수일시, 회신일시, 입원 / 외래, 진료과, 병동, 병실, 담당과장, 등록번호, 성명, 의뢰자, 답변자, WRTNO

                SQL = "";
                SQL = " SELECT TOTAL ,GRADE,  TO_CHAR(ENTDATE, 'YYYY-MM-DD HH24:MI') ENTDATE, ROWID, EMRNO ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_SOFASCOER MST";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + argIPDNO + " ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ENTDATE DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssSofa_Sheet1.RowCount = dt.Rows.Count;
                ssSofa_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssSofa_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                    ssSofa_Sheet1.Cells[i, 1].Text = dt.Rows[i]["TOTAL"].ToString().Trim() + "   " + dt.Rows[i]["GRADE"].ToString().Trim();
                    ssSofa_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssSofa_Sheet1.Cells[i, 3].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void ssView_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            int i = 0;
            string strCNT = "";

            for (i = 6; i < 10; i++)
            {
                if (Convert.ToBoolean(ssView_Sheet1.Cells[3, i].Value) == true)
                {
                    strCNT = "Y";
                }
            }

            if (strCNT == "Y")
            {
                ssView_Sheet1.Rows[4].Visible = false;
                ss_Clear();
            }
            else
            {
                ssView_Sheet1.Rows[4].Visible = true;
            }
        }

        private void ssView_ComboCloseUp(object sender, EditorNotifyEventArgs e)
        {
            ssView_White();
            ss_Jumsu_Acc();
            ss_AVG_Acc();
        }

        void ss_AVG_Acc()
        {
            int nPoint = 0;

            nPoint = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[39, 5].Text));
            ssView_Sheet1.Cells[40, 5].Text = (getGeneralProbOfDeathSAPS3(nPoint) * 100).ToString("#0.#0") + " %";
        }

        void ss_Jumsu_Acc()
        {
            int i = 0;
            int inx = 0;
            double nPoint1 = 0;

            nPoint1 = 16;

            for (i = 5; i < 39; i++)
            {
                if (ssView_Sheet1.Cells[i, 5].CellType != null && ssView_Sheet1.Cells[i, 5].CellType.ToString() == "ComboBoxCellType")
                {
                    inx = clsSpread.gSpreadComboindex(ssView, i, 5, ssView_Sheet1.Cells[i, 5].Text.Trim()) + 1;
                    nPoint1 = nPoint1 + VB.Val(ssView_Sheet1.Cells[i, 9 + inx].Text);
                }
            }

            ssView_Sheet1.Cells[39, 5].Text = nPoint1.ToString();
        }

        void ssView_White()
        {
            int i = 0;

            for (i = 5; i < 39; i++)
            {
                if (ssView_Sheet1.Cells[i, 5].Text.Trim().IndexOf("(default)") != -1)
                {
                    ssView_Sheet1.Cells[i, 5].BackColor = Color.FromArgb(255, 255, 255);
                }
                else if (ssView_Sheet1.Cells[i, 5].Text == "")
                {
                    ssView_Sheet1.Cells[i, 5].BackColor = Color.FromArgb(192, 192, 192);
                }
                else
                {
                    ssView_Sheet1.Cells[i, 5].BackColor = Color.FromArgb(200, 90, 80);
                }
            }
        }

        void Delete_SOFA_Chart(string argEMRNO)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            double dblEmrHisNo = 0;

            string strSysDate = "";
            string strSysTime = "";

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO");

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRXMLHISTORY";
                SQL = SQL + ComNum.VBLF + "      (HISTORYNO, EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,HISTORYWRITEDATE,HISTORYWRITETIME,DELUSEID,CERTNO)";
                SQL = SQL + ComNum.VBLF + " SELECT  " + dblEmrHisNo + ",";
                SQL = SQL + ComNum.VBLF + "      EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,";
                SQL = SQL + ComNum.VBLF + "      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,";
                SQL = SQL + ComNum.VBLF + "      WRITEDATE,WRITETIME,CHARTXML,CONTENTS,UPDATENO,";
                SQL = SQL + ComNum.VBLF + "      '" + strSysDate.Replace("-", "") + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strSysTime.Replace(":", "") + "', '" + clsType.User.Sabun + "',CERTNO";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + argEMRNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + argEMRNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE FROM " + ComNum.DB_EMR + "EMRXMLMST";
                SQL = SQL + ComNum.VBLF + "  WHERE EMRNO = " + argEMRNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void ssSofa_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strROWID = "";
            string strEMRNO = "";

            if (ComFunc.MsgBoxQ("해당차트일시의 SOFA Score 정보를 삭제합니다. 계속하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            strROWID = ssSofa_Sheet1.Cells[e.Row, 2].Text;
            strEMRNO = ssSofa_Sheet1.Cells[e.Row, 3].Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " DELETE " + ComNum.DB_PMPA + "NUR_SOFASCOER ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID =  '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (strEMRNO != "")
            {
                Delete_SOFA_Chart(strEMRNO);
            }

            Read_Progress(VB.Val(GstrIPDNO));   //VB - P(GstrHelpCode, "^^", 1) -> GstrIPDNO
        }

        private void btnViewResult_Click(object sender, EventArgs e)
        {
            frmViewResult frmViewResultX = new frmViewResult(ssView_Sheet1.Cells[1, 0].Text.Trim());
            frmViewResultX.StartPosition = FormStartPosition.CenterParent;
            frmViewResultX.ShowDialog();
            frmViewResultX.Dispose();
            frmViewResultX = null;
        }
    }
}
