using ComBase; //기본 클래스
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmComSupMayakPrescription.cs
    /// Description     : 마약처방전
    /// Author          : 이정현
    /// Create Date     : 2017-10-12
    /// <history> 
    /// 마약처방전
    /// </history>
    /// <seealso>
    /// PSMH\drug\drmayak\frm마약처방전.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drmayak\drmayak.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupMayakPrescription : Form
    {
        private string GstrWardCode = string.Empty;
        private string GstrIPDBuse = string.Empty;

        //==================================================
        //2011-09-27 김현욱 추가
        //값이 YES일 경우 의사 서명 없이 인쇄
        //수기 처방전으로 인식함, 마약대장에 표시되지 않음!
        private string GstrNoSign = string.Empty;
        //==================================================

        //처방전 출력 변수
        private string GstrHeader = string.Empty;         //헤더
        private string GstrWardRoom = string.Empty;       //병동.병실
        private string GstrName = string.Empty;           //성명
        private string GstrPtNo = string.Empty;           //등록번호
        private string GstrDept = string.Empty;           //진료과
        private string GstrOrderNo = string.Empty;        //오더번호
        private string GstrSexAge = string.Empty;         //성별.나이
        private string GstrJumin = string.Empty;          //주민번호
        private string GstrJuso = string.Empty;           //주소
        private string GstrIllName1 = string.Empty;       //상병명
        private string GstrIllName2 = string.Empty;       //주요증상
        private string GstrOrdDate = string.Empty;        //처방일
        private string GstrPrtName = string.Empty;        //출력일1
        private string GstrPrtDate = string.Empty;        //출력일2
        private string GstrOrdCode = string.Empty;        //약품코드
        private string GstrOrdName = string.Empty;        //약품명
        private string GstrUnit = string.Empty;           //함량/단위
        private string GstrQty = string.Empty;            //일투량
        private string GstrDiv = string.Empty;            //#
        private string GstrNal = string.Empty;            //일수
        private string GstrDosName = string.Empty;        //용법명
        private string GstrTotalCnt = string.Empty;       //총 불출량
        private string GstrStayName = string.Empty;       //잔여량1
        private string GstrStayCnt = string.Empty;        //잔여량2
        private string GstrDrName = string.Empty;         //의사이름
        private Image GimgDrSign = null;        //의사서명
        private string GstrDrLicense = string.Empty;      //면허번호 
        private Image GimgBarCode = null;       //바코드
        private string GstrEntQty = string.Empty;         //입력수량
           
        clsSpread methodSpd = null;

        public frmComSupMayakPrescription()
        {
            InitializeComponent();
        }

        public frmComSupMayakPrescription(string strWardCode)
        {
            InitializeComponent();

            GstrWardCode = strWardCode;
        }

        private void frmComSupMayakPrescription_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            methodSpd = new clsSpread();

            //SetSpread();

            if (clsType.User.Grade == "EDPS")
            {
                chkLandscape.Visible = true;
            }
            else
            {
                chkLandscape.Visible = false;
            }

            chk2021Paper.Visible = clsType.User.IdNumber.Equals("31660");

            dtpSDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            dtpEDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            lblSecond.Text = "0";

            ssView_Sheet1.RowCount = 0;

            cboPrint.Items.Clear();

            if (PrinterSettings.InstalledPrinters.Count > 0)
            {
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    cboPrint.Items.Add(PrinterSettings.InstalledPrinters[i].ToString());
                }

                cboPrint.SelectedIndex = 0;

                for (int i = 0; i < cboPrint.Items.Count; i++)
                {
                    if (clsPrint.gGetDefaultPrinter() == cboPrint.Items[i].ToString())
                    {
                        cboPrint.SelectedIndex = i;
                        break;
                    }
                }
            }

            if (GstrWardCode == "TO" || GstrWardCode == "HR")
            {
                cboWard.Items.Add("TO");
                cboWard.Items.Add("HR");

                cboWard.SelectedIndex = 0;
                cboWard.Enabled = true;
                return;
            }

            SetCboWard();

            chkDCVisible.Visible = false;
            chkDCVisible.Checked = true;

            panTimer.Visible = true;
            timer1.Enabled = true;

            if (GstrWardCode == "EN")
            {
                chkDCVisible.Visible = true;
                chkDCVisible.Checked = false;
                timer1.Enabled = false;
                panTimer.Visible = false;
            }

            if (ChkWard() == true)
            {
                timer1.Enabled = false;
                panTimer.Visible = false;

                if (Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) >= Convert.ToDateTime("2009-08-05"))
                {
                    btnPrint.Enabled = false;
                }
            }

            if (GstrWardCode == "AN" || GstrWardCode == "AG")
            {
                timer1.Enabled = false;
                panTimer.Visible = false;
            }

            cboWard.Enabled = true;
        }

        private void SetCboWard()
        {
            string SQL = string.Empty;
            DataTable dt = null;
            string SqlErr = string.Empty;
            int i = 0;

            cboWard.Items.Clear();

            cboWard.Items.Add("OPD.외래");
            cboWard.Items.Add("IPD.병동전체");

            GstrIPDBuse = string.Empty;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     WARDCODE, WARDNAME  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + "     WHERE WARDCODE NOT IN ('IU','NP','2W','IQ','ER','AN') ";
                SQL = SQL + ComNum.VBLF + "     AND USED = 'Y'  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());

                        GstrIPDBuse = GstrIPDBuse + "'" + dt.Rows[i]["WARDCODE"].ToString().Trim() + "',";
                    }
                }

                dt.Dispose();
                dt = null;

                cboWard.Items.Add("SICU");
                cboWard.Items.Add("MICU");
                cboWard.Items.Add("ER");
                cboWard.Items.Add("HD");
                cboWard.Items.Add("EN.내시경실");
                cboWard.Items.Add("AN.마취과");
                cboWard.Items.Add("PC.통증치료실");
                cboWard.Items.Add("AG.ANGIO실");
                cboWard.Items.Add("OP.수술실");
                cboWard.Items.Add("HR.신체검사");
                cboWard.Items.Add("TO.종합건진");
                cboWard.Items.Add("TH.검진센터");

                GstrIPDBuse = GstrIPDBuse + "'IU','HD','EN','AG','OP','ER' ";

                cboWard.SelectedIndex = 0;

                if (GstrWardCode != "")
                {
                    for (i = 0; i < cboWard.Items.Count; i++)
                    {
                        if (VB.Pstr(cboWard.Items[i].ToString(), ".", 1) == GstrWardCode)
                        {
                            cboWard.SelectedIndex = i;
                            cboWard.Enabled = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private bool ChkWard()
        {
            string SQL = string.Empty;
            DataTable dt = null;
            string SqlErr = string.Empty;
            bool rtnVal = false;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT * FROM " + ComNum.DB_PMPA + "BAS_WARD";
                SQL = SQL + ComNum.VBLF + "     WHERE WARDCODE = '" + GstrWardCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            //int i = 0;
            bool bolValue = false;

            if (ssView_Sheet1.RowCount == 0) { return; }

            if (btnSelectAll.Text == "전체선택")
            {
                btnSelectAll.Text = "전체해제";
                bolValue = true;
            }
            else if (btnSelectAll.Text == "전체해제")
            {
                btnSelectAll.Text = "전체선택";
                bolValue = false;
            }

            //for (i = 0; i < ssView_Sheet1.RowCount; i++)
            //{
            //    ssView_Sheet1.Cells[i, 0].Value = bolValue;
            //}

            //2018-08-23 YUN
            methodSpd.setSpdCellChk_All(ssView, 0, bolValue);

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인 

            btnSelectAll.Text = "전체선택";
            ssView_Sheet1.RowCount = 0;

            if (VB.Left(cboWard.Text, 2) == "EN")
            {
                //내시경실은 별도 조회되도록 구성
                OCS_MAYAK_ENDO();
                return;
            }

            GstrNoSign = string.Empty;

            //종합건진만 "수면" 칼럼을 표시함(2014-02-20 LYJ)
            if (VB.Left(cboWard.Text, 2) == "TO")
            {
                ssView_Sheet1.Columns[14].Visible = true;
            }
            else
            {
                ssView_Sheet1.Columns[14].Visible = false;
            }

            if (rdoGB0.Checked == true || rdoGB1.Checked == true)
            {
                READ_MAYAK();
            }

            if (rdoGB0.Checked == true || rdoGB2.Checked == true)
            {
                if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "AN"
                    || VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "PC")
                {
                    //마취과 요청 (2012-07-14) 향정 조회 요청
                    READ_HYANG();
                }
                else
                {
                    if (GstrWardCode == "TO" || GstrWardCode == "HR" || VB.Left(cboWard.Text.Trim(), 2) == "TH") { }
                    else
                    {
                        if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "OPD"
                            || VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "TO"
                            || VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "HR") { }
                        else
                        {
                            Total_Cnt();
                            return;//입원은 아래 로직 사용않함
                        }
                    }

                    READ_HYANG();
                }
            }

            Total_Cnt();
        }

        private void OCS_MAYAK_ENDO()
        {
            ssView_Sheet1.RowCount = 0;

            if (rdoGB0.Checked == true || rdoGB1.Checked == true)
            {
                READ_MAYAK_ENDO();
            }

            if (rdoGB0.Checked == true || rdoGB2.Checked == true)
            {
                READ_HYANG_ENDO();
            }
        }

        private void READ_MAYAK_ENDO()
        {
            string SQL = string.Empty;
            DataTable dt = null;
            string SqlErr = string.Empty;
            int i = 0;

            try
            {
                //마약조회
                SQL = string.Empty;
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, A.PTNO, B.SNAME, A.IO, A.DEPTCODE, ";
                SQL = SQL + ComNum.VBLF + "     A.ROOMCODE, A.REMARK1, A.REMARK2, A.QTY, A.NAL, A.SUCODE, A.ROWID, ";
                SQL = SQL + ComNum.VBLF + "     A.SEQNO, A.PRINT , A.REALQTY, A.ORDERNO, A.JDATE_ENDO, C.RDATE, A.DOSCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MAYAK A, " + ComNum.DB_PMPA + "BAS_PATIENT B, ";
                SQL = SQL + ComNum.VBLF + "         (SELECT";
                SQL = SQL + ComNum.VBLF + "             JDATE, PTNO, TO_CHAR(RDATE, 'YYYY-MM-DD') RDATE";
                SQL = SQL + ComNum.VBLF + "         FROM " + ComNum.DB_MED + "ENDO_JUPMST";
                SQL = SQL + ComNum.VBLF + "             WHERE RDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                 AND RDATE < TO_DATE('" + dtpEDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         GROUP BY JDATE, PTNO, TO_CHAR(RDATE, 'YYYY-MM-DD') ) C ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = C.JDATE ";
                SQL = SQL + ComNum.VBLF + "         AND A.PTNO  = C.PTNO ";
                SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'EN' ";

                if (chkPrint.Checked == true)
                {
                    if (txtPtNo.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.PTNO  = '" + txtPtNo.Text.Trim() + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.PRINT = 'Y' ";
                }
                else
                {
                    if (VB.Left(cboWard.Text, 3) == "OPD")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.ENTDATE IS NOT NULL ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.SEQNO IS NULL ";

                    if (chkGubun.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.PRINT IS NULL ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "         AND A.PTNO = B.PANO(+) ";

                if (chkDCVisible.Checked == false)
                {
                    //2014-02-24 김현욱 추가
                    //DC 처방전은 보이지 않도록 함.
                    SQL = SQL + ComNum.VBLF + "         AND NAL > 0";
                    SQL = SQL + ComNum.VBLF + "         AND NOT EXISTS (SELECT * FROM " + ComNum.DB_MED + "OCS_MAYAK SUB";
                    SQL = SQL + ComNum.VBLF + "                             WHERE A.BDATE = SUB.BDATE";
                    SQL = SQL + ComNum.VBLF + "                                  AND A.PTNO = SUB.PTNO";
                    SQL = SQL + ComNum.VBLF + "                                  AND A.ORDERNO = SUB.ORDERNO";
                    SQL = SQL + ComNum.VBLF + "                                  AND A.IO = SUB.IO";
                    SQL = SQL + ComNum.VBLF + "                                  AND SUB.NAL < 0";
                    SQL = SQL + ComNum.VBLF + "                                  AND SUB.ENTDATE IS NOT NULL)";
                }

                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.SUCODE, A.BDATE, A.ROOMCODE  ";
                }
                else if (rdoSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.ROOMCODE, A.BDATE, A.SUCODE ";
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.BDATE, A.ROOMCODE, A.SUCODE ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = 0;

                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["IO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["REMARK1"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["REMARK2"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        if (dt.Rows[i]["SEQNO"].ToString().Trim() == "" && dt.Rows[i]["PRINT"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 13].Text = "마약(삭제)";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 13].Text = "마약";
                        }

                        ssView_Sheet1.Cells[i, 14].Text = string.Empty;
                        ssView_Sheet1.Cells[i, 15].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 16].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 17].Text = READ_DOSNAME(dt.Rows[i]["DOSCODE"].ToString().Trim());

                        if (dt.Rows[i]["RDATE"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 18].Text = Convert.ToDateTime(dt.Rows[i]["RDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }
                    }
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
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_DOSNAME(string strDosCode)
        {
            string SQL = string.Empty;
            DataTable dt = null;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT DOSNAME FROM " + ComNum.DB_MED + "OCS_ODOSAGE ";
                SQL = SQL + ComNum.VBLF + "     WHERE DOSCODE = '" + strDosCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DOSNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void READ_HYANG_ENDO()
        {
            string SQL = string.Empty;
            DataTable dt = null;
            string SqlErr = string.Empty;
            int i = 0;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, A.PTNO, B.SNAME, A.IO, A.DEPTCODE, ";
                SQL = SQL + ComNum.VBLF + "     A.ROOMCODE, A.REMARK1, A.REMARK2, A.QTY, A.NAL, A.SUCODE, A.ROWID, ";
                SQL = SQL + ComNum.VBLF + "     A.SEQNO, A.PRINT , A.REALQTY, A.ORDERNO, A.JDATE_ENDO, C.RDATE, A.DOSCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_HYANG A, " + ComNum.DB_PMPA + "BAS_PATIENT B, ";
                SQL = SQL + ComNum.VBLF + "         (SELECT";
                SQL = SQL + ComNum.VBLF + "             TO_CHAR(RDATE, 'YYYY-MM-DD') RDATE, JDATE, PTNO";
                SQL = SQL + ComNum.VBLF + "         FROM " + ComNum.DB_MED + "ENDO_JUPMST";
                SQL = SQL + ComNum.VBLF + "             WHERE RDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "                 AND RDATE <  TO_DATE('" + dtpEDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         GROUP BY JDATE, PTNO, TO_CHAR(RDATE, 'YYYY-MM-DD')) C ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = C.JDATE ";
                SQL = SQL + ComNum.VBLF + "         AND A.PTNO = C.PTNO ";
                SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'EN' ";

                if (chkPrint.Checked == true)
                {
                    if (txtPtNo.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.PTNO = '" + txtPtNo.Text.Trim() + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.PRINT = 'Y' ";
                }
                else
                {
                    if (VB.Left(cboWard.Text, 3) == "OPD")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.ENTDATE IS NOT NULL ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.SEQNO IS NULL ";

                    if (chkGubun.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.PRINT IS NULL ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "         AND A.PTNO = B.PANO(+) ";

                if (chkDCVisible.Checked == false)
                {
                    //2014-02-24 김현욱 추가
                    //DC 처방전은 보이지 않도록 함.
                    SQL = SQL + ComNum.VBLF + "         AND NAL > 0";
                    SQL = SQL + ComNum.VBLF + "         AND NOT EXISTS (SELECT * FROM " + ComNum.DB_MED + "OCS_HYANG SUB";
                    SQL = SQL + ComNum.VBLF + "                             WHERE A.BDATE = SUB.BDATE";
                    SQL = SQL + ComNum.VBLF + "                                 AND A.PTNO = SUB.PTNO";
                    SQL = SQL + ComNum.VBLF + "                                 AND A.ORDERNO = SUB.ORDERNO";
                    SQL = SQL + ComNum.VBLF + "                                 AND A.IO = SUB.IO";
                    SQL = SQL + ComNum.VBLF + "                                 AND SUB.NAL < 0";
                    SQL = SQL + ComNum.VBLF + "                                 AND SUB.ENTDATE IS NOT NULL)";
                }

                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.SUCODE, A.BDATE, A.ROOMCODE  ";
                }
                else if (rdoSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.ROOMCODE, A.BDATE, A.SUCODE ";
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.BDATE, A.ROOMCODE, A.SUCODE ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["IO"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["REMARK1"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["REMARK2"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        if (dt.Rows[i]["SEQNO"].ToString().Trim() == "" && dt.Rows[i]["PRINT"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = "향정(삭제)";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = "향정";
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Text = string.Empty;
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 15].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 16].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 17].Text = READ_DOSNAME(dt.Rows[i]["DOSCODE"].ToString().Trim());

                        if (dt.Rows[i]["RDATE"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 18].Text = Convert.ToDateTime(dt.Rows[i]["RDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }
                    }
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
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void READ_MAYAK()
        {
            string SQL = string.Empty;
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = string.Empty;
            int i = 0;

            string strDosCodeSunap = string.Empty;

            ssView_Sheet1.RowCount = 0;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, A.PTNO, B.SNAME, A.IO, A.DEPTCODE, ";
                SQL = SQL + ComNum.VBLF + "     A.ROOMCODE, A.REMARK1, A.REMARK2, A.QTY, A.NAL, A.SUCODE, A.ROWID, ";
                SQL = SQL + ComNum.VBLF + "     A.SEQNO, A.PRINT , A.REALQTY, A.ORDERNO, A.DOSCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_MAYAK A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "         AND (A.WARDCODE = 'ER' OR ( A.DEPTCODE = 'ER' AND  A.IO = 'O'))";
                    SQL = SQL + ComNum.VBLF + "         AND A.ENTDATE IS NOT NULL ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "EN")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'EN' ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "AN")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'AN' ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "PC")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'PC' ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "AG")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'AG' ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "OP")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'OP' ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "OPD") //외래
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.IO = 'O'  ";
                    SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE NOT IN ('HR', 'TO')";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "IPD") //병동
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE IN (" + GstrIPDBuse + ") ";
                    SQL = SQL + ComNum.VBLF + "         AND A.IO = 'I'  ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "HR"
                    || VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "TO")    //외래
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE = '" + VB.Split(cboWard.Text, ".")[0].ToString().Trim() + "'  ";
                    //2020-05-04 추가
                    SQL = SQL + ComNum.VBLF + "         AND A.DRSABUN IS NOT NULL";
                }
                //2020-05-06 추가
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "TH") //검진센터
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE IN('HR','TO') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.DRSABUN IS NOT NULL ";
                }
                else
                {
                    if (cboWard.Text.Trim() == "MICU")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND WARDCODE = 'IU'";
                        SQL = SQL + ComNum.VBLF + "         AND RoomCode = '234'  ";
                    }

                    if (cboWard.Text.Trim() == "SICU")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND WARDCODE = 'IU'";
                        SQL = SQL + ComNum.VBLF + "         AND RoomCode = '233'  ";
                    }

                    if (cboWard.Text.Trim() == "ND" || cboWard.Text.Trim() == "NR")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND RoomCode IN ('369','358','368','640','641','642') ";
                    }

                    if (cboWard.Text.Trim() != "MICU" && cboWard.Text.Trim() != "SICU"
                        && cboWard.Text.Trim() != "ND" && cboWard.Text.Trim() != "NR")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND WARDCODE = '" + cboWard.Text.Trim() + "' ";
                    }
                }

                if (txtPtNo.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.PTNO = '" + txtPtNo.Text.Trim() + "' ";
                }

                if (chkPrint.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.PRINT = 'Y' ";

                    if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "ER")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND (A.WARDCODE = 'ER' OR (A.DEPTCODE = 'ER' AND  A.IO = 'O')) ";
                        SQL = SQL + ComNum.VBLF + "         AND A.ENTDATE IS NOT NULL ";
                    }
                    else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "EN")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'EN' ";
                    }
                    else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "AN")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'AN' ";
                    }
                    else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "PC")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'PC' ";
                    }
                    else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "AG")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'AG' ";
                    }
                    else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "OP")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'OP' ";
                    }
                    else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "OPD") //외래
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.IO = 'O'  ";
                    }
                    else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "IPD") //병동
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE IN (" + GstrIPDBuse + ") ";
                        SQL = SQL + ComNum.VBLF + "         AND A.IO = 'I'  ";
                    }
                    else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "HR"
                        || VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "TO")    //외래
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE  = '" + VB.Split(cboWard.Text, ".")[0].ToString().Trim() + "'  ";
                    }
                    else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "TH")
                    {

                    }
                    else
                    {
                        if (cboWard.Text.Trim() == "MICU")
                        {
                            SQL = SQL + ComNum.VBLF + "         AND WARDCODE = 'IU'";
                            SQL = SQL + ComNum.VBLF + "         AND RoomCode = '234'  ";
                        }

                        if (cboWard.Text.Trim() == "SICU")
                        {
                            SQL = SQL + ComNum.VBLF + "         AND WARDCODE = 'IU' ";
                            SQL = SQL + ComNum.VBLF + "         AND RoomCode = '233'  ";
                        }

                        if (cboWard.Text.Trim() == "ND" || cboWard.Text.Trim() == "NR")
                        {
                            SQL = SQL + ComNum.VBLF + "         AND RoomCode IN ('369','358','368','640','641','642') ";
                        }

                        if (cboWard.Text.Trim() != "MICU" && cboWard.Text.Trim() != "SICU"
                            && cboWard.Text.Trim() != "ND" && cboWard.Text.Trim() != "NR")
                        {
                            SQL = SQL + ComNum.VBLF + "          AND WARDCODE = '" + cboWard.Text.Trim() + "' ";
                        }
                    }
                }
                else
                {
                    if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "OPD")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.ENTDATE IS NOT NULL ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.SEQNO IS NULL ";

                    if (chkGubun.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.PRINT IS NULL ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "         AND A.PTNO = B.PANO(+) ";

                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.SUCODE, A.BDATE, A.ROOMCODE  ";
                }
                else if (rdoSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.ROOMCODE, A.BDATE, A.SUCODE ";
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.BDATE, A.ROOMCODE, A.SUCODE ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["IO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["REMARK1"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["REMARK2"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        if (dt.Rows[i]["SEQNO"].ToString().Trim() == "" && dt.Rows[i]["PRINT"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 13].Text = "마약(삭제)";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 13].Text = "마약";
                        }

                        ssView_Sheet1.Cells[i, 15].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 16].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 17].Text = READ_DOSNAME(dt.Rows[i]["DOSCODE"].ToString().Trim());

                        strDosCodeSunap = READ_SUNAP_DOSCODE(dt.Rows[i]["PTNO"].ToString().Trim(), dt.Rows[i]["BDATE"].ToString().Trim(), dt.Rows[i]["SUCODE"].ToString().Trim(), VB.Val(dt.Rows[i]["ORDERNO"].ToString().Trim()));

                        if (dt.Rows[i]["IO"].ToString().Trim() == "O" && dt.Rows[i]["DOSCODE"].ToString().Trim() != strDosCodeSunap && strDosCodeSunap != "")
                        {
                            ssView_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 200, 200);
                        }

                        //수면여부 표시
                        ssView_Sheet1.Cells[i, 14].Text = string.Empty;

                        if (VB.Left(cboWard.Text, 2) == "TO")
                        {
                            SQL = string.Empty;
                            SQL = "SELECT GbSleep FROM " + ComNum.DB_PMPA + "HIC_HYANG_APPROVE ";
                            SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + dt.Rows[i]["PTNO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BDate = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "         AND SuCode = '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["GBSLEEP"].ToString().Trim() == "Y")
                                {
                                    ssView_Sheet1.Cells[i, 14].Text = "◎";
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
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
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_SUNAP_DOSCODE(string strPtNo, string strBDate, string strSuCode, double dblOrderNo)
        {
            string SQL = string.Empty;
            DataTable dt = null;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT DOSCODE FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND SUNEXT = '" + strSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "         AND ORDERNO = " + dblOrderNo;

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DOSCODE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void Total_Cnt()
        {
            int i = 0;
            int k = 0;
            int p = 0;

            int intCol = 0;

            string strJEPCODE = string.Empty;
            string strOK = string.Empty;

            double dblQty = 0;
            double dblCNT = 0;
            double dblNAL = 0;

            Set_MayakCode();

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                strJEPCODE = ssView_Sheet1.Cells[i, 9].Text.Trim();

                if (BUN_11(strJEPCODE) == true)
                {
                    dblQty = VB.Val(ssView_Sheet1.Cells[i, 10].Text.Trim());
                }
                else
                {
                    dblQty = Math.Round(VB.Val(ssView_Sheet1.Cells[i, 10].Text.Trim()) + 0.4);
                }

                dblNAL = VB.Val(ssView_Sheet1.Cells[i, 11].Text.Trim());

                dblCNT = dblQty * dblNAL;

                strOK = string.Empty;

                for (k = 0; k < ssMayakCode_Sheet1.RowCount; k++)
                {
                    for (p = 0; p < ssMayakCode_Sheet1.ColumnCount; p++)
                    {
                        if (strJEPCODE == ssMayakCode_Sheet1.Cells[k, p].Text.Trim())
                        {
                            strOK = "OK";
                            intCol = p + 1;
                            break;
                        }
                    }

                    if (strOK == "OK") { break; }
                }

                if (strOK == "OK")
                {
                    ssMayakCode_Sheet1.Cells[k, intCol].Text = (VB.Val(ssMayakCode_Sheet1.Cells[k, intCol].Text) + dblCNT).ToString();
                    ssMayakCode_Sheet1.Cells[k, intCol].BackColor = Color.FromArgb(120, 150, 120);

                    if (VB.Val(ssMayakCode_Sheet1.Cells[k, intCol].Text) == 0)
                    {
                        ssMayakCode_Sheet1.Cells[k, intCol].Text = string.Empty;
                        ssMayakCode_Sheet1.Cells[k, intCol].BackColor = Color.FromArgb(255, 255, 255);
                    }
                }
            }
        }

        private void Set_MayakCode()
        {
            string SQL = string.Empty;
            DataTable dt = null;
            string SqlErr = string.Empty;
            int i = 0;

            ssMayakCode_Sheet1.Cells[0, 0, ssMayakCode_Sheet1.RowCount - 1, ssMayakCode_Sheet1.ColumnCount - 1].Text = string.Empty;
            ssMayakCode_Sheet1.Cells[0, 0, ssMayakCode_Sheet1.RowCount - 1, ssMayakCode_Sheet1.ColumnCount - 1].BackColor = Color.White;

            ssMayakCode_Sheet1.RowCount = 0;
            ssMayakCode_Sheet1.RowCount = 1;
            ssMayakCode_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            try
            {
                SQL = string.Empty;
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JEPCODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_JEP";
                SQL = SQL + ComNum.VBLF + "     WHERE CHENGGU = '09'";
                SQL = SQL + ComNum.VBLF + "         AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "         AND JEPCODE NOT IN ('N-FT-PC', 'N-MP5') "; //N-FT-PC 는 N-FT-HA와 같은 약
                SQL = SQL + ComNum.VBLF + "ORDER BY JEPCODE ASC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    int intRow = 0;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i != 0)
                        {
                            if (i % 7 == 0)
                            {
                                intRow++;
                                ssMayakCode_Sheet1.RowCount = ssMayakCode_Sheet1.RowCount + 1;
                                ssMayakCode_Sheet1.SetRowHeight(ssMayakCode_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                            }
                        }

                        ssMayakCode_Sheet1.Cells[intRow, (i % 7) * 2].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                    }
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
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private bool BUN_11(string strCode)
        {
            string SQL = string.Empty;
            DataTable dt = null;
            string SqlErr = string.Empty;
            bool rtnVal = false;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT * FROM " + ComNum.DB_MED + "OCS_ORDERCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE BUN = '11'";
                SQL = SQL + ComNum.VBLF + "         AND SUCODE = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool BUN_20(string strCode)
        {
            string SQL = string.Empty;
            DataTable dt = null;
            string SqlErr = string.Empty;
            bool rtnVal = false;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT * FROM " + ComNum.DB_MED + "OCS_ORDERCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE BUN NOT IN ('11', '12')";
                SQL = SQL + ComNum.VBLF + "         AND SUCODE = '" + strCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void READ_HYANG()
        {
            string SQL = string.Empty;
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = string.Empty;
            int i = 0;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.PTNO, B.SNAME, A.IO, A.DEPTCODE, ";
                SQL = SQL + ComNum.VBLF + "     A.ROOMCODE, A.REMARK1, A.REMARK2, A.QTY, A.NAL, A.SUCODE, A.ROWID, ";
                SQL = SQL + ComNum.VBLF + "     A.SEQNO, A.PRINT , A.REALQTY, A.ORDERNO, A.DOSCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_HYANG A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE >= TO_DATE('" + dtpSDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "ER")
                {
                    SQL = SQL + ComNum.VBLF + "         AND (A.WARDCODE ='ER' OR ( A.DEPTCODE ='ER' AND  A.IO ='O' ) )   ";
                    SQL = SQL + ComNum.VBLF + "         AND A.ENTDATE IS NOT NULL ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "EN")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE ='EN' ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "AN")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE ='AN' ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "PC")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE ='PC' ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "AG")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE ='AG' ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "OP")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE ='OP' ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "TO")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE ='TO' ";
                    //2020-05-04 추가
                    SQL = SQL + ComNum.VBLF + "         AND A.DRSABUN IS NOT NULL";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "HR")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE ='HR' ";
                }
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "OPD") //외래
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.IO = 'O'  ";
                    SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE NOT IN ('HR','TO')";
                    SQL = SQL + ComNum.VBLF + "         AND (WARDCODE <> 'EN' OR WARDCODE IS NULL) ";
                }
                //2020-05-06 추가
                else if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "TH") //검진센터
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE IN('HR','TO') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.DRSABUN IS NOT NULL ";
                }

                if (txtPtNo.Text.Trim() != "")
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.PTNO  = '" + txtPtNo.Text.Trim() + "' ";
                }

                if (chkPrint.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "         AND A.PRINT = 'Y' ";
                }
                else
                {
                    if (VB.Split(cboWard.Text, ".")[0].ToString().Trim() == "OPD")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.ENTDATE IS NOT NULL ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.SEQNO IS NULL ";

                    if (chkGubun.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.PRINT IS NULL ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "         AND A.PTNO = B.PANO(+) ";

                if (rdoSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.SUCODE, A.BDATE, A.ROOMCODE  ";
                }
                else if (rdoSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.ROOMCODE, A.BDATE, A.SUCODE ";
                }
                else if (rdoSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.BDATE, A.ROOMCODE, A.SUCODE ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT); 

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["IO"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["REMARK1"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["REMARK2"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        if (dt.Rows[i]["SEQNO"].ToString().Trim() == "" && dt.Rows[i]["PRINT"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = "향정(삭제)";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = "향정";
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 15].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 16].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 17].Text = READ_DOSNAME(dt.Rows[i]["DOSCODE"].ToString().Trim());

                        //수면여부 표시
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Text = string.Empty;

                        if (VB.Left(cboWard.Text, 2) == "TO")
                        {
                            SQL = string.Empty;
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     GbSleep";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "HIC_HYANG_APPROVE ";
                            SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + dt.Rows[i]["PTNO"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND BDate = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "         AND SuCode = '" + dt.Rows[i]["SUCODE"].ToString().Trim() + "' ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["GBSLEEP"].ToString().Trim() == "Y")
                                {
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Text = "◎";
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }
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
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            GetPrint();
        }

        private void GetPrint()
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if (ssView_Sheet1.RowCount == 0) { return; }

            string strDeptCode = string.Empty;
            string strROWID = string.Empty;
            string strGubun = string.Empty;
            string strRDate = string.Empty;

            if (clsPrint.gGetPrinterFind(cboPrint.Text) == false)
            {
                ComFunc.MsgBox("지정된 프린터를 찾을수 없습니다.");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if ((chkAutoPrt.Checked == false && ssView_Sheet1.Cells[i, 0].Text.Trim().Equals("True")) || chkAutoPrt.Checked == true)
                    {
                        strDeptCode = ssView_Sheet1.Cells[i, 6].Text.Trim();
                        strROWID = ssView_Sheet1.Cells[i, 12].Text.Trim();
                        strGubun = ssView_Sheet1.Cells[i, 13].Text.Trim();
                        strRDate = ssView_Sheet1.Cells[i, 18].Text.Trim();

                        #region 출력 변수 초기화

                        GstrHeader = string.Empty;
                        GstrWardRoom = string.Empty;
                        GstrName = string.Empty;
                        GstrPtNo = string.Empty;
                        GstrDept = string.Empty;
                        GstrOrderNo = string.Empty;
                        GstrSexAge = string.Empty;
                        GstrJumin = string.Empty;
                        GstrJuso = string.Empty;
                        GstrIllName1 = string.Empty;
                        GstrIllName2 = string.Empty;
                        GstrOrdDate = string.Empty;
                        GstrPrtName = string.Empty;
                        GstrPrtDate = string.Empty;
                        GstrOrdCode = string.Empty;
                        GstrOrdName = string.Empty;
                        GstrUnit = string.Empty;
                        GstrQty = string.Empty;
                        GstrDiv = string.Empty;
                        GstrNal = string.Empty;
                        GstrDosName = string.Empty;
                        GstrTotalCnt = string.Empty;
                        GstrStayName = string.Empty;
                        GstrStayCnt = string.Empty;
                        GstrDrName = string.Empty;
                        GimgDrSign = null;
                        GstrDrLicense = string.Empty;
                        GimgBarCode = null;

                        #endregion

                        if (PRT_RECEIPT(strROWID, strGubun, cboWard.Text, strDeptCode, strRDate, chkPrint.Checked, cboPrint.Text) == false)
                        {
                            Cursor.Current = Cursors.Default;
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("인쇄 중 에러 발생", "에러");
                            return;
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

            ssView_Sheet1.RowCount = 0;
        }

        private bool PRT_RECEIPT(string strROWID, string strGubun, string strWard, string strDeptGubunCode, string strRDate, bool bolRePrt, string strPrintName)
        {
            string SQL = string.Empty;
            DataTable dt = null;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;

            string strPtNo = string.Empty;
            string strIO = string.Empty;
            string strDeptCode = string.Empty;
            string strWardCode = string.Empty;
            string strRemark1 = string.Empty;
            string strRemark2 = string.Empty;
            string strSname = string.Empty;
            //string strYakRowid = = string.Empty;
            string strBDate = string.Empty;
            string strEntDate = string.Empty;
            string strQty = string.Empty;
            string strJumin = string.Empty;
            string strZipCode1 = string.Empty;
            string strZipCode2 = string.Empty;
            string strJuso = string.Empty;
            string strRoom = string.Empty;
            string strDrCode = string.Empty;
            string strSuCode = string.Empty;
            string strSeqNo = string.Empty;
            string strBi = string.Empty;
            //string strDiv =  string.Empty;
            string strDosCode = string.Empty;
            //string strORDERNO =  string.Empty;
            string strUnit1 = string.Empty;
            string strUnit2 = string.Empty;
            string strUnit3 = string.Empty;
            string strUnit4 = string.Empty;
            string strSex = string.Empty;
            string strFile = string.Empty;
            string strGBPrint = string.Empty;
            string strSEX_A = string.Empty;
            string strAGE_A = string.Empty;
            string strJUMIN_A = string.Empty;
            string strJUSO_A = string.Empty;
            string strTable = string.Empty;
            //string strTitle =  string.Empty;
            string strNRSabun = string.Empty;
            string strSabun = string.Empty;
            string strDRCODE_NEW = string.Empty;
            string strDrName = string.Empty;
            string strDRNo = string.Empty;
            //string strTemp =  string.Empty;
            //string strTemp2 =  string.Empty;
            //string strTemp3 =  string.Empty;
            string strDosCodeSunap = string.Empty;
            string strEntQty = string.Empty;

            double dblOrderNo = 0;
            double dblRealQty = 0;
            double dblNAL = 0;

            //string strFont1 = string.Empty;
            //string strHead1 = string.Empty;
            //string strPrintHead = string.Empty;

            //Cursor.Current = Cursors.WaitCursor;

            //마약처방전 스프레드 초기화
            //SetSpread();
            //SetOrderPrint();

            //clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //종검 마약/향정 처방전 서식변경 수정(2014-02-20:LYJ)
                if (strDeptGubunCode == "TO")
                {
                    if (VB.Left(strGubun, 2) == "마약")
                    {
                        strTable = "OCS_MAYAK";
                        //strTitle = "마   약   처   방   전";
                    }
                    else if (VB.Left(strGubun, 2) == "향정")
                    {
                        strTable = "OCS_HYANG";
                        //strTitle = "향 정 신 성 의 약 품";
                    }
                }
                else
                {
                    if (VB.Left(strGubun, 2) == "마약")
                    {
                        strTable = "OCS_MAYAK";
                        //strTitle = "마   약   처   방   전";
                    }
                    else if (VB.Left(strGubun, 2) == "향정")
                    {
                        strTable = "OCS_HYANG";
                        //strTitle = "향 정 신 성 의 약 품";
                    }
                }

                if (VB.Left(strGubun, 2) == "마약")
                {
                    //strPrintHead = "마 약" + VB.Space(10);
                    GstrHeader = VB.Space(8) + "마 약";
                }
                else if (VB.Left(strGubun, 2) == "향정")
                {
                    //strPrintHead = "향정신성의약품" + VB.Space(20);
                    GstrHeader = "향정신성의약품";
                }

                //2016-07-26 주소 인쇄 오류로 수정함(LYJ)
                if (strDeptGubunCode == "TO" || strDeptGubunCode == "HR")   //일반건진,종합건진
                {
                    SQL = string.Empty;
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PTNO, A.SNAME, A.BI, A.WARDCODE, A.ROOMCODE, TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE,    ";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.EntDate,'YYYY-MM-DD') AS EntDate, ";
                    SQL = SQL + ComNum.VBLF + "     A.DEPTCODE, A.DRSABUN, A.IO, A.SUCODE, A.SEQNO, A.IO, ";
                    SQL = SQL + ComNum.VBLF + "     A.QTY, A.REALQTY, A.NAL, A.DOSCODE, A.REMARK1, A.REMARK2, A.ENTQTY,";
                    SQL = SQL + ComNum.VBLF + "     B.SNAME, B.JUMIN1||'-'||SUBSTR(B.JUMIN2, 1, 1)||'******' AS JUMIN, B.JUMIN3, ";
                    SQL = SQL + ComNum.VBLF + "     B.JUMIN1, SUBSTR(C.MailCode,1,3) ZIPCODE1,SUBSTR(C.MailCode,4,3) AS ZIPCODE2,";
                    SQL = SQL + ComNum.VBLF + "     C.JUSO1 || ' ' || C.JUSO2 AS JUSO,B.SEX, A.NRSABUN, A.ORDERNO, ";
                    SQL = SQL + ComNum.VBLF + "     A.SEX AS SEX_A, A.AGE AS AGE_A, A.JUMIN AS JUMIN_A, A.JUSO AS JUSO_A  ";

                    //if (strDeptGubunCode == "TO") //종합건진
                    //{
                    //    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + strTable + " A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_PMPA + "HEA_PATIENT C ";
                    //}
                    //else
                    //{
                    //2020-04-23 주소 읽어오는 테이블 수정
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + strTable + " A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_PMPA + "HIC_PATIENT C ";
                    //}

                    SQL = SQL + ComNum.VBLF + "     WHERE A.ROWID  = '" + strROWID + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PTNO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PTNO = C.PTNO(+) ";
                }
                else
                {
                    SQL = string.Empty;
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.PTNO, A.SNAME, A.BI, A.WARDCODE, A.ROOMCODE, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, ";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.EntDate,'YYYY-MM-DD') AS EntDate, ";
                    SQL = SQL + ComNum.VBLF + "     A.DEPTCODE, A.DRSABUN, A.IO, A.SUCODE, A.SEQNO, A.IO, ";
                    SQL = SQL + ComNum.VBLF + "     A.QTY, A.REALQTY, A.NAL, A.DOSCODE, A.REMARK1, A.REMARK2, A.ENTQTY, ";
                    SQL = SQL + ComNum.VBLF + "     B.SNAME, B.JUMIN1||'-'||SUBSTR(B.JUMIN2, 1, 1)||'******' AS JUMIN, B.JUMIN3, ";
                    SQL = SQL + ComNum.VBLF + "     B.JUMIN1, C.ZIPCODE1, C.ZIPCODE2, C.JUSO || ' ' || C.JUSO2 JUSO, ";
                    SQL = SQL + ComNum.VBLF + "     B.SEX, A.NRSABUN, A.ORDERNO, ";
                    SQL = SQL + ComNum.VBLF + "     A.SEX SEX_A, A.AGE AGE_A, A.JUMIN JUMIN_A, A.JUSO JUSO_A ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + strTable + " A, " + ComNum.DB_PMPA + "BAS_PATIENT B, " + ComNum.DB_PMPA + "VIEW_PATIENT_JUSO C ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.ROWID  = '" + strROWID + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PTNO = B.PANO(+) ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PTNO = C.PANO(+) ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    //Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strDrCode = dt.Rows[0]["DRSABUN"].ToString().Trim();

                    if (strTable == "OCS_MAYAK")
                    {
                        strDRCODE_NEW = READ_DIFF_DRSABUN_SET(strROWID);

                        if (strDRCODE_NEW != "" && VB.Val(dt.Rows[0]["ORDERNO"].ToString().Trim()) > 1)
                        {
                            if (strDrCode != strDRCODE_NEW && dt.Rows[0]["IO"].ToString().Trim() == "O")
                            {
                                if (ComFunc.MsgBoxQEx(this, "처방의사명이 변경되었으니 확인하시기 바랍니다!" + ComNum.VBLF +
                                  "전자인증된 의사명으로 출력하실려면 'YES', 그대로 출력하시려면 'NO'를 선택하시기 바랍니다.", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    strDrCode = strDRCODE_NEW;
                                    strDRCODE_NEW = "NEW";
                                }
                                else
                                {
                                    strDRCODE_NEW = "NO";
                                }
                            }
                        }
                    }

                    strWardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    strBDate = dt.Rows[0]["BDATE"].ToString().Trim();
                    strEntDate = dt.Rows[0]["ENTDATE"].ToString().Trim();
                    strPtNo = dt.Rows[0]["PTNO"].ToString().Trim();
                    strSname = dt.Rows[0]["SNAME"].ToString().Trim();
                    strIO = dt.Rows[0]["IO"].ToString().Trim();
                    strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    strRemark1 = dt.Rows[0]["REMARK1"].ToString().Trim();
                    strRemark2 = dt.Rows[0]["REMARK2"].ToString().Trim();
                    strSuCode = dt.Rows[0]["SUCODE"].ToString().Trim();
                    strQty = dt.Rows[0]["QTY"].ToString().Trim();
                    dblRealQty = VB.Val(dt.Rows[0]["REALQTY"].ToString().Trim());
                    dblNAL = VB.Val(dt.Rows[0]["NAL"].ToString().Trim());
                    strJumin = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    strZipCode1 = dt.Rows[0]["ZIPCODE1"].ToString().Trim();
                    strZipCode2 = dt.Rows[0]["ZIPCODE2"].ToString().Trim();
                    strJuso = dt.Rows[0]["JUSO"].ToString().Trim();
                    strRoom = clsVbfunc.READ_WARD_FROM_ROOM(clsDB.DbCon, dt.Rows[0]["ROOMCODE"].ToString().Trim()) + "/" + dt.Rows[0]["ROOMCODE"].ToString().Trim();

                    strBi = dt.Rows[0]["BI"].ToString().Trim();
                    strDosCode = dt.Rows[0]["DOSCODE"].ToString().Trim();
                    strSeqNo = dt.Rows[0]["SEQNO"].ToString().Trim();
                    strSex = dt.Rows[0]["SEX"].ToString().Trim();

                    if (strIO == "O" && GstrWardCode == "EN")
                    {
                        strNRSabun = ComFunc.LPAD(clsType.User.Sabun, 5, "0");
                    }
                    else
                    {
                        strNRSabun = dt.Rows[0]["NRSABUN"].ToString().Trim();
                    }

                    dblOrderNo = VB.Val(dt.Rows[0]["ORDERNO"].ToString().Trim());

                    strSEX_A = dt.Rows[0]["SEX_A"].ToString().Trim();
                    strAGE_A = dt.Rows[0]["AGE_A"].ToString().Trim();
                    strJUMIN_A = dt.Rows[0]["JUMIN_A"].ToString().Trim();
                    strJUSO_A = dt.Rows[0]["JUSO_A"].ToString().Trim();

                    strDosCodeSunap = string.Empty;

                    if (strIO == "O")
                    {
                        strDosCodeSunap = READ_SUNAP_DOSCODE(strPtNo, strBDate, strSuCode, dblOrderNo);

                        if (strDosCodeSunap != "" && strDosCodeSunap != strDosCode)
                        {
                            strDosCode = strDosCodeSunap;
                            strDosCodeSunap = "OK";
                        }
                    }

                    strEntQty = dt.Rows[0]["ENTQTY"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (bolRePrt == false)
                {
                    SQL = string.Empty;
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     MAX(SEQNO) SEQNO ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + strTable;
                    SQL = SQL + ComNum.VBLF + "     WHERE BDATE >= TO_DATE('" + dtpSDate.Value.Year + "-01-01" + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        //Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["SEQNO"].ToString().Trim() == "")
                        {
                            strSeqNo = dtpSDate.Value.Year + "0001";
                        }
                        else
                        {
                            strSeqNo = (VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim()) + 1).ToString();
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    SQL = string.Empty;
                    SQL = "UPDATE " + ComNum.DB_MED + strTable;
                    SQL = SQL + ComNum.VBLF + "     SET";
                    SQL = SQL + ComNum.VBLF + "         PRINT = 'Y',";

                    //=========================================================
                    //2014-01-24 김현욱 주임
                    //외래이면서 내시경실에서 인쇄한 건은 간호사 이름이 인쇄한 이름으로 나오도록 요청
                    //김동열부장님, 내시경실 합의,
                    if (GstrWardCode == "EN" && strIO == "O")
                    {
                        SQL = SQL + ComNum.VBLF + "         NRSABUN = '" + clsType.User.Sabun + "', ";
                    }
                    //=========================================================

                    if (strDosCodeSunap == "OK")
                    {
                        SQL = SQL + ComNum.VBLF + "         DOSCODE = '" + strDosCode + "', ";
                    }

                    if (GstrNoSign == "YES")
                    {
                        SQL = SQL + ComNum.VBLF + "         JDATE_ENDO = SYSDATE ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         SEQNO = '" + strSeqNo + "' ";
                    }

                    if (strTable == "OCS_MAYAK" && strDRCODE_NEW == "NEW")
                    {
                        SQL = SQL + ComNum.VBLF + "         , DRSABUN = '" + strDrCode + "' ";
                    }

                    if (strSEX_A == "")
                    {
                        SQL = SQL + ComNum.VBLF + "         , SEX = '" + strSex + "' "; //2014-01-08
                    }

                    if (strAGE_A == "")
                    {
                        SQL = SQL + ComNum.VBLF + "         , AGE = '" + ComFunc.AgeCalcEx(strJumin, ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + "' ";
                    }

                    if (strJUMIN_A == "")
                    {
                        SQL = SQL + ComNum.VBLF + "         , JUMIN = '" + VB.Left(strJumin.Replace("-", ""), 7) + "******' ";
                        SQL = SQL + ComNum.VBLF + "         , JUMIN3 = '" + clsAES.AES(strJumin.Replace("-", "")) + "'";
                    }

                    if (strJUSO_A == "")
                    {
                        SQL = SQL + ComNum.VBLF + "         , JUSO = '" + strJuso + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        //Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }
                else
                {
                    //19-07-17 재출력시 주소 업데이트 치게 수정.
                    if (strJUSO_A == "")
                    {
                        SQL = string.Empty;
                        SQL = "UPDATE " + ComNum.DB_MED + strTable;
                        SQL = SQL + ComNum.VBLF + "     SET";
                        SQL = SQL + ComNum.VBLF + "   JUSO = '" + strJuso + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                    }
                }

                //등록번호
                GstrPtNo = strPtNo;

                //성명
                GstrName = strSname;

                //성별/나이
                GstrSexAge = strSex + "/" + ComFunc.AgeCalcEx(strJumin.Replace("-", ""), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

                //주민번호
                //2019-11-28 고시:마약류의약품 처방전 발급시 환자정보 기재 의무화
                //GstrJumin = VB.Left(strJumin, 8) + "*******";
                GstrJumin = strJumin;

                //종합검진은 검사일,처방일을 인쇄함(2012-02-20:LYJ)
                if (strDeptGubunCode == "TO")
                {
                    GstrOrdDate = strEntDate;
                    GstrPrtName = "검 사 일";
                    GstrPrtDate = strBDate;
                }
                else if (GstrNoSign == "YES")
                {
                    //========================================================
                    //2011-09-27 김현욱 추가
                    //내시경실 인쇄의 경우
                    //마약은 날짜에 검사일을 표시
                    //향정은 날짜 => 처방일,  주요증상 하단에 검사일 표시 요청
                    //2011-09-26 오후 5시 약제과 회의에서 결정한 사항(요한릿다, 데레사 수녀님, 전종윤 계장님 참석)
                    if (VB.Left(cboWard.Text, 2) == "EN")
                    {
                        if (VB.Left(strGubun, 2) == "마약")
                        {
                            //처방일
                            GstrOrdDate = strRDate;
                        }
                        else if (VB.Left(strGubun, 2) == "향정")
                        {
                            GstrOrdDate = strBDate;
                            GstrPrtName = "검 사 일";
                            GstrPrtDate = strRDate;
                        }
                    }
                    else
                    {
                        //처방일
                        GstrOrdDate = strBDate;
                    }
                }
                else
                {
                    //처방일
                    GstrOrdDate = strBDate;
                }
                //========================================================

                //약품명
                GstrOrdName = "*" + clsVbfunc.Read_Drug_Jep_Name(clsDB.DbCon, strSuCode);

                //약품코드
                GstrOrdCode = strSuCode;

                //-------------------2009-06-03
                //함량/단위
                SQL = string.Empty;
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     UNITNEW1, UNITNEW2, UNITNEW3, UNITNEW4";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN ";
                SQL = SQL + ComNum.VBLF + "     WHERE SUNEXT = '" + strSuCode + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strUnit1 = dt.Rows[0]["UNITNEW1"].ToString().Trim();
                    strUnit2 = dt.Rows[0]["UNITNEW2"].ToString().Trim();
                    strUnit3 = dt.Rows[0]["UNITNEW3"].ToString().Trim();
                    strUnit4 = dt.Rows[0]["UNITNEW4"].ToString().Trim();

                    GstrUnit = strUnit1 + strUnit2 + "/" + (VB.Val(strUnit4) > 0 ? strUnit4 + "㎖/" : "") + strUnit3;
                }

                dt.Dispose();
                dt = null;
                //-----------------------------

                //일투량
                GstrQty = dblRealQty.ToString();

                //#, 용법
                //strTemp = string.Empty;

                SQL = string.Empty;
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     GBDIV, IDOSNAME, DOSNAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ODOSAGE ";
                SQL = SQL + ComNum.VBLF + "     WHERE DOSCODE  = '" + strDosCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    //#
                    GstrDiv = dt.Rows[0]["GBDIV"].ToString().Trim();

                    //용법
                    GstrDosName = dt.Rows[0]["IDOSNAME"].ToString().Trim();

                    if (GstrDosName == "")
                    {
                        GstrDosName = dt.Rows[0]["DOSNAME"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                //일수
                GstrNal = dblNAL.ToString();

                if ((strDeptCode == "HR" || strDeptCode == "TO" || strWardCode == "EN") && VB.Left(strGubun, 2) == "향정")
                {
                    GstrStayName = "실 사용량";
                }
                else
                {
                    if (dblNAL < 0)
                    {
                    }
                    else
                    {
                        if (BUN_20(strSuCode) == true && Math.Round(dblRealQty + 0.4) - dblRealQty > 0)
                        {
                            GstrTotalCnt = (Convert.ToInt32(dblRealQty + 0.4)).ToString();
                            GstrStayName = "잔 여 량";
                            GstrStayCnt = (Math.Round(dblRealQty + 0.4) - dblRealQty) + "(   )";
                        }
                        else
                        {
                            GstrTotalCnt = (dblRealQty * dblNAL).ToString();
                        }
                    }
                }

                //========================================================
                //2011-09-27 김현욱 추가
                //내시경실인 경우
                //의사 서명없이 인쇄를 클릭할 경우
                //마약은 의사만 제외 후 인쇄
                //향정은 의사명과 의사사인 모두 제외 후 인쇄
                //2011-09-26 오후 5시 약제과 회의에서 결정한 사항(요한릿다, 데레사 수녀님, 전종윤 계장님 참석)
                if (VB.Left(cboWard.Text, 2) == "EN" && GstrNoSign == "YES")
                {
                    SQL = string.Empty;
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     B.KORNAME, A.DRBUNHO, A.SABUN";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR A, " + ComNum.DB_ERP + "INSA_MST B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE (A.SABUN = '" + ComFunc.LPAD(strDrCode, 5, "0") + "' OR A.DRCODE = '" + strDrCode + "' OR A.DRBUNHO = '" + strDrCode + "') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.SABUN = B.SABUN ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(GBOUT, 'N', 1, 'Y', 2) ASC";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (VB.Left(strGubun, 2) == "마약")
                        {
                            strDrName = dt.Rows[0]["KORNAME"].ToString().Trim();
                            strDRNo = dt.Rows[0]["DRBUNHO"].ToString().Trim();
                        }
                        else
                        {
                            strDrName = string.Empty;
                            strDRNo = string.Empty;
                        }

                        strFile = dt.Rows[0]["SABUN"].ToString().Trim();
                        strSabun = ComFunc.LPAD(strFile, 5, "0");
                    }

                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    SQL = string.Empty;
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     B.KORNAME, A.DRBUNHO, A.SABUN";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR A, " + ComNum.DB_ERP + "INSA_MST B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE (A.SABUN = '" + ComFunc.LPAD(strDrCode, 5, "0") + "' OR A.DRCODE = '" + strDrCode + "' OR A.DRBUNHO = '" + strDrCode + "') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.SABUN = B.SABUN ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY DECODE(GBOUT, 'N', 1, 'Y', 2) ASC";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strDrName = dt.Rows[0]["KORNAME"].ToString().Trim();
                        strDRNo = dt.Rows[0]["DRBUNHO"].ToString().Trim();
                        strFile = dt.Rows[0]["SABUN"].ToString().Trim();
                        strSabun = ComFunc.LPAD(strFile, 5, "0");
                    }
                    else
                    {
                        strDrName = string.Empty;
                        strDRNo = string.Empty;
                    }

                    dt.Dispose();
                    dt = null;

                    //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
                    if (strSabun != "")
                    {
                        Image image = SIGNATUREFILE_DBToFile(strSabun);

                        //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                        if (image != null)
                        {
                            //의사성명
                            GstrDrName = strDrName;

                            //서명
                            GimgDrSign = image;

                            GstrDrLicense = strDRNo;
                        }

                        image = null;
                    }
                    //--------------------------------------------------------------------------------------------------------
                }

                //병동/병실
                GstrWardRoom = (strIO == "O" ? "외래" : strRoom);

                //출력일
                //if (strDeptGubunCode == "TO" || VB.Left(cboWard.Text, 2) == "EN" && VB.Left(strGubun, 2) == "향정")
                //{
                //}
                //else
                //{
                if (chkDateHidden.Checked == false)
                {
                    if (GstrPrtName == "")
                    {
                        GstrPrtName = "출 력 일";
                    }

                    if (GstrPrtDate == "")
                    {
                        if (VB.Left(strGubun, 2) == "향정")
                        {
                            GstrPrtDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");
                        }
                        else
                        {
                            GstrPrtDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");
                        }
                    }
                }
                //}

                //진료과
                GstrDept = strDeptCode;

                if (strJuso == "") { strJuso = strJUSO_A; }

                //주소
                GstrJuso = strJuso;

                if (dblOrderNo > 0)
                {
                    SQL = string.Empty;
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     GBPRINT ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_PHARMACY ";
                    SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND ORDERNO = " + dblOrderNo;
                    SQL = SQL + ComNum.VBLF + "  AND NAL = " + dblNAL;

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        //Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strGBPrint = dt.Rows[0]["GBPRINT"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }

                //오더번호
                GstrOrderNo = dblOrderNo.ToString("###,###,###") + (strIO == "I" || strDeptCode == "ER" ? "(" + strGBPrint + ")" : "");

                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                string strBarCode = "";

                //2018-04-25 이정현 마약/향정 바코드 추가
                if (VB.Left(strGubun, 2) == "마약")
                {
                    GimgBarCode = b.Encode(BarcodeLib.TYPE.CODE128, "M|" + Convert.ToDateTime(strBDate).ToString("yyMMdd") + "|" + dblOrderNo, Color.Black, Color.White, 250, 45);
                    strBarCode = "M|" + Convert.ToDateTime(strBDate).ToString("yyMMdd") + "|" + dblOrderNo;
                }
                else if (VB.Left(strGubun, 2) == "향정")
                {
                    GimgBarCode = b.Encode(BarcodeLib.TYPE.CODE128, "H|" + Convert.ToDateTime(strBDate).ToString("yyMMdd") + "|" + dblOrderNo, Color.Black, Color.White, 250, 45);
                    strBarCode = "H|" + Convert.ToDateTime(strBDate).ToString("yyMMdd") + "|" + dblOrderNo;
                }



                //상병명
                //GstrIllName1 = strRemark1;

                //2019-12-19
                GstrIllName1 = ReadIllCode(bolRePrt, strSeqNo, strIO, strPtNo, strDeptCode, strWardCode, strBDate, strBarCode);

                string[] str = GstrIllName1.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                //주요증상
                GstrIllName2 = "";
                if (str.Length > 0)
                {
                    GstrIllName2 = clsVbfunc.READ_ILLName(clsDB.DbCon, str[0].Replace(".", ""));
                }

                //입력수량
                GstrEntQty = strEntQty;

                Print_Order_Document(strPrintName);
                ComFunc.Delay(100);
                //clsDB.setCommitTran(clsDB.DbCon);

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 처방전 Document 출력
        /// </summary>
        private void Print_Order_Document(string strPrintName)
        {
            using (PrintDocument pd = new PrintDocument())
            {
                PrintController printController = new StandardPrintController();

                pd.PrintController = printController;
                pd.PrinterSettings.PrinterName = strPrintName;
                pd.PrinterSettings.DefaultPageSettings.PaperSize = new PaperSize("ORDER", 210, 150);

                if (chk2021Paper.Checked)
                {
                    pd.PrintPage += new PrintPageEventHandler(eORDER_New_Print);
                }
                else
                {
                    pd.PrintPage += new PrintPageEventHandler(eORDER_Print);
                }
                pd.Print();    //프린트
            }
        }

        /// <summary>
        /// 신규 마약처방전 양식 1차 테스트 완료
        /// 진료과 칸 수정 예정 => 추후 다시 테스트 후 오픈 
        /// 2021-08-03
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void eORDER_New_Print(object sender, PrintPageEventArgs ev)
        {
            int x = 10;
            int y = 10;
            using (StringFormat stringFormat = new StringFormat())
            {
                using (Font FTitle = new Font("맑은 고딕", 16f))
                {
                    ev.Graphics.DrawString("마약", FTitle, Brushes.Black, x + 250, y + 15, stringFormat);
                }

                using (Font FSub = new Font("맑은 고딕", 10f))
                {
                    ev.Graphics.DrawString(GstrWardRoom, FSub, Brushes.Black, x + 100, y + 60, stringFormat);
                    ev.Graphics.DrawString(GstrName, FSub, Brushes.Black, x + 250, y + 60, stringFormat);
                    ev.Graphics.DrawString("KHAN MOHAMMAD QAYYUM HASSAN", FSub, Brushes.Black, x + 475, y + 60, stringFormat);
                    ev.Graphics.DrawString(GstrDept, FSub, Brushes.Black, x + 740, y + 60, stringFormat);
                    ev.Graphics.DrawString(GstrPtNo, FSub, Brushes.Black, x + 680, y + 90, stringFormat);

                    ev.Graphics.DrawString(GstrOrderNo, FSub, Brushes.Black, x + 90, y + 90, stringFormat);
                    ev.Graphics.DrawString(GstrSexAge, FSub, Brushes.Black, x + 320, y + 90, stringFormat);
                    ev.Graphics.DrawString(GstrJumin, FSub, Brushes.Black, x + 475, y + 90, stringFormat);

                    //ev.Graphics.DrawString(GstrJuso, FSub, Brushes.Black, x + 90, y + 120, stringFormat);

                    ev.Graphics.DrawString(GstrIllName1, FSub, Brushes.Black, x + 90, y + 120, stringFormat);

                    ev.Graphics.DrawString(GstrIllName2, FSub, Brushes.Black, x + 90, y + 150, stringFormat);

                    ev.Graphics.DrawString(GstrOrdDate, FSub, Brushes.Black, x + 90, y + 180, stringFormat);
                    ev.Graphics.DrawString(GstrPrtDate, FSub, Brushes.Black, x + 525, y + 180, stringFormat);

                    ev.Graphics.DrawString(GstrOrdCode, FSub, Brushes.Black, x + 20, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrOrdName, FSub, Brushes.Black, x + 90, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrUnit, FSub, Brushes.Black, x + 250, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrQty, FSub, Brushes.Black, x + 400, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrDiv, FSub, Brushes.Black, x + 440, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrNal, FSub, Brushes.Black, x + 480, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrDosName, FSub, Brushes.Black, x + 525, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrTotalCnt, FSub, Brushes.Black, x + 685, y + 245, stringFormat);
                    ev.Graphics.DrawString("폐기량", FSub, Brushes.Black, x + 685, y + 325, stringFormat);

                    //ev.Graphics.DrawString(GstrStayName, FSub, Brushes.Black, x + 685, y + 310, stringFormat);
                    //ev.Graphics.DrawString(GstrStayCnt, FSub, Brushes.Black, x + 710, y + 340, stringFormat);

                    ev.Graphics.DrawString(GstrDrName, FSub, Brushes.Black, x + 90, y + 380, stringFormat);

                    ev.Graphics.DrawString(GstrDrLicense, FSub, Brushes.Black, x + 90, y + 430, stringFormat);
                }

                if (GimgDrSign != null)
                {
                    ev.Graphics.DrawImage(GimgDrSign, x + 140, y + 375, 100, 35);
                }

                if (GimgBarCode != null)
                {
                    ev.Graphics.DrawImage(GimgBarCode, x + 520, y + 390, 250, 45);
                }
            }
        }

        private void eORDER_Print(object sender, PrintPageEventArgs ev)
        {
            int x = 10;
            int y = 10;
            using (StringFormat stringFormat = new StringFormat())
            {
                using (Font FTitle = new Font("맑은 고딕", 16f))
                {
                    ev.Graphics.DrawString(GstrHeader, FTitle, Brushes.Black, x + 250, y + 15, stringFormat);
                }

                using (Font FSub = new Font("맑은 고딕", 10f))
                {
                    ev.Graphics.DrawString(GstrWardRoom, FSub, Brushes.Black, x + 90, y + 60, stringFormat);
                    ev.Graphics.DrawString(GstrName, FSub, Brushes.Black, x + 325, y + 60, stringFormat);
                    ev.Graphics.DrawString(GstrPtNo, FSub, Brushes.Black, x + 525, y + 60, stringFormat);
                    ev.Graphics.DrawString(GstrDept, FSub, Brushes.Black, x + 685, y + 60, stringFormat);

                    ev.Graphics.DrawString(GstrOrderNo, FSub, Brushes.Black, x + 90, y + 90, stringFormat);
                    ev.Graphics.DrawString(GstrSexAge, FSub, Brushes.Black, x + 325, y + 90, stringFormat);
                    ev.Graphics.DrawString(GstrJumin, FSub, Brushes.Black, x + 525, y + 90, stringFormat);

                    ev.Graphics.DrawString(GstrJuso, FSub, Brushes.Black, x + 90, y + 120, stringFormat);

                    ev.Graphics.DrawString(GstrIllName1, FSub, Brushes.Black, x + 90, y + 150, stringFormat);

                    ev.Graphics.DrawString(GstrIllName2, FSub, Brushes.Black, x + 90, y + 180, stringFormat);

                    ev.Graphics.DrawString(GstrOrdDate, FSub, Brushes.Black, x + 90, y + 210, stringFormat);
                    ev.Graphics.DrawString(GstrPrtName, FSub, Brushes.Black, x + 425, y + 210, stringFormat);
                    ev.Graphics.DrawString(GstrPrtDate, FSub, Brushes.Black, x + 525, y + 210, stringFormat);

                    ev.Graphics.DrawString(GstrOrdCode, FSub, Brushes.Black, x + 20, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrOrdName, FSub, Brushes.Black, x + 90, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrUnit, FSub, Brushes.Black, x + 250, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrQty, FSub, Brushes.Black, x + 415, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrDiv, FSub, Brushes.Black, x + 460, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrNal, FSub, Brushes.Black, x + 500, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrDosName, FSub, Brushes.Black, x + 525, y + 265, stringFormat);
                    ev.Graphics.DrawString(GstrTotalCnt, FSub, Brushes.Black, x + 710, y + 265, stringFormat);

                    if ((VB.Left(cboWard.SelectedItem.ToString().Trim(), 2) == "TO"
                        || VB.Left(cboWard.SelectedItem.ToString().Trim(), 2) == "HR"
                        || VB.Left(cboWard.SelectedItem.ToString().Trim(), 2) == "TH") && GstrEntQty != "")
                    {
                        ev.Graphics.DrawString("(=" + String.Format(GstrEntQty, "##0.00") + "ml)", FSub, Brushes.Black, x + 407, y + 290, stringFormat);
                    }
                    ev.Graphics.DrawString(GstrStayName, FSub, Brushes.Black, x + 685, y + 310, stringFormat);
                    ev.Graphics.DrawString(GstrStayCnt, FSub, Brushes.Black, x + 710, y + 340, stringFormat);

                    ev.Graphics.DrawString(GstrDrName, FSub, Brushes.Black, x + 90, y + 390, stringFormat);

                    ev.Graphics.DrawString(GstrDrLicense, FSub, Brushes.Black, x + 90, y + 430, stringFormat);
                }
            }

            if (GimgDrSign != null)
            {
                ev.Graphics.DrawImage(GimgDrSign, x + 140, y + 375, 100, 35);
            }

            if (GimgBarCode != null)
            {
                ev.Graphics.DrawImage(GimgBarCode, x + 520, y + 380, 250, 45);
            }
        }

        private Image SIGNATUREFILE_DBToFile(string strSabun)
        {
            string SQL = string.Empty;
            DataTable dt = null;
            string SqlErr = string.Empty;

            byte[] b = null;
            Image image = null;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT SIGNATURE FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                SQL = SQL + ComNum.VBLF + "where SABUN = '" + strSabun + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return image;
                }
                if (dt.Rows.Count > 0)
                {
                    b = (byte[])(dt.Rows[0]["SIGNATURE"]);

                    using (MemoryStream stream = new MemoryStream(b))
                    {
                        image = Image.FromStream(stream);
                    }
                }

                dt.Dispose();
                dt = null;

                return image;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return image;
            }
        }

        private string READ_DIFF_DRSABUN_SET(string strROWID)
        {
            string SQL = string.Empty;
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            try
            {
                SQL = string.Empty;
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     PTNO, DRSABUN, ORDERNO, TO_CHAR(BDATE_R, 'YYYY-MM-DD') AS BDATE_R ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_MAYAK ";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (VB.Val(dt.Rows[0]["ORDERNO"].ToString().Trim()) < 2)
                    {
                        dt.Dispose();
                        dt = null;
                        return rtnVal;
                    }

                    SQL = string.Empty;
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     DRCODE ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_IORDER ";
                    SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + dt.Rows[0]["PTNO"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND BDATE = TO_DATE('" + dt.Rows[0]["BDATE_R"].ToString().Trim() + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND ORDERNO = " + dt.Rows[0]["ORDERNO"].ToString().Trim();

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        rtnVal = dt1.Rows[0]["DRCODE"].ToString().Trim();
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (rtnVal == "")
                    {
                        SQL = string.Empty;
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.DRCODE, B.SABUN ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_MED + "OCS_DOCTOR B ";
                        SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + dt.Rows[0]["PTNO"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + dt.Rows[0]["BDATE_R"].ToString().Trim() + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "  AND A.ORDERNO = " + dt.Rows[0]["ORDERNO"].ToString().Trim();
                        SQL = SQL + ComNum.VBLF + "  AND A.DRCODE = B.DRCODE";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            rtnVal = dt1.Rows[0]["SABUN"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnNoSignPrint_Click(object sender, EventArgs e)
        {
            if (VB.Left(cboWard.Text, 2) != "EN")
            {
                ComFunc.MsgBox("내시경실에서만 사용이 가능합니다.");
                GstrNoSign = string.Empty;
                return;
            }

            GstrNoSign = "YES";

            GetPrint();

            GstrNoSign = string.Empty;
        }

        private void btnSumPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            int k = 0;
            int intRow = 0;

            ssPrint_Sheet1.Cells[3, 1].Text = "▣ 인쇄일시 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");

            ssPrint_Sheet1.Cells[7, 2, 28, 5].Text = string.Empty;

            for (i = 0; i < ssMayakCode_Sheet1.RowCount; i++)
            {
                for (k = 0; k < ssMayakCode_Sheet1.ColumnCount; k++)
                {
                    if (VB.IsNumeric(ssMayakCode_Sheet1.Cells[i, k].Text) && VB.Val(ssMayakCode_Sheet1.Cells[i, k].Text) > 0)
                    {
                        ssPrint_Sheet1.Cells[intRow + 7, 2].Text = ssMayakCode_Sheet1.Cells[i, k - 1].Text.Trim();
                        ssPrint_Sheet1.Cells[intRow + 7, 3].Text = clsVbfunc.READ_SugaName(clsDB.DbCon, ssMayakCode_Sheet1.Cells[i, k - 1].Text);
                        ssPrint_Sheet1.Cells[intRow + 7, 4].Text = ssMayakCode_Sheet1.Cells[i, k].Text.Trim();

                        intRow++;
                    }
                }
            }

            //ssPrint_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssPrint_Sheet1.PrintInfo.Margin.Top = 20;
            ssPrint_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssPrint_Sheet1.PrintInfo.ShowColor = false;
            ssPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPrint_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssPrint_Sheet1.PrintInfo.ShowBorder = false;
            ssPrint_Sheet1.PrintInfo.ShowGrid = false;
            ssPrint_Sheet1.PrintInfo.ShowShadows = false;
            ssPrint_Sheet1.PrintInfo.UseMax = true;
            ssPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPrint_Sheet1.PrintInfo.UseSmartPrint = false;
            ssPrint_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssPrint_Sheet1.PrintInfo.Preview = false;
            ssPrint.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoGB_CheckedChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void chkAutoPrt_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoPrt.Checked == true)
            {
                ssView_Sheet1.Columns[0].Visible = false;
            }
            else
            {
                ssView_Sheet1.Columns[0].Visible = true;
            }

            if (ssView_Sheet1.RowCount > 0)
            {
                for (int i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Value = false;
                }
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
        }

        private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VB.Left(cboWard.Text, 2) != "EN")
            {
                GstrNoSign = string.Empty;
            }
            else
            {
                chkDCVisible.Visible = true;
                chkDCVisible.Checked = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblSecond.Text = (VB.Val(lblSecond.Text) + 1).ToString();

            if (VB.Val(lblSecond.Text) == 50)
            {
                GetData();
            }
            else if (VB.Val(lblSecond.Text) == 55)
            {
                if (ssView_Sheet1.RowCount > 0 && chkAutoPrt.Checked == true)
                {
                    GetPrint();
                }

                lblSecond.Text = "0";
            }
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true && e.Column != 0)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
            }
        }

        private void frmComSupMayakPrescription_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {

                if (GimgDrSign != null)
                {
                    GimgDrSign.Dispose();
                }

                if (GimgBarCode != null)
                {
                    GimgBarCode.Dispose();
                }

                methodSpd = null;
            }

            catch { }
        }

        /// <summary>
        /// 상병코드 조회
        /// </summary>
        /// <param name="strIO"></param>
        /// <param name="strPano"></param>
        /// <param name="strDept"></param>
        /// <param name="strBdate"></param>
        /// <returns></returns>
        private string ReadIllCode(bool bolRePrt, string strSeqNo, string strIO, string strPano, string strDept, string strWardCode, string strBdate, string strBarCode)
        {
            string SQL = string.Empty;
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            try
            {                

                // 마약상병이 있는지 조회
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                   ";
                SQL = SQL + ComNum.VBLF + "    PTNO, SEQNO, KCDCODE, SORT           ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_MILLS              ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPano + "'          ";
                SQL = SQL + ComNum.VBLF + "   AND BARCODE = '" + strBarCode + "'    ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SORT                           ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        rtnVal += dt.Rows[i]["KCDCODE"].ToString().Trim() + ", ";
                    }

                    return VB.Left(rtnVal, rtnVal.Length - 2);
                }
                
                // 마약상병이 없으면 OCS 상병 조회
                if (strDept == "ER" || strWardCode == "ER")
                {
                    //'응급실
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT PANO, MAX(TO_CHAR(BDATE,'YYYY-MM-DD')) AS BDATE";
                    SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.OPD_MASTER om";
                    SQL = SQL + ComNum.VBLF + "  WHERE om.PANO = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "    AND om.DEPTCODE = 'ER'";
                    SQL = SQL + ComNum.VBLF + "    AND TRUNC(om.BDATE) <= TO_DATE('" + strBdate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  GROUP BY PANO";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT ";
                        SQL = SQL + ComNum.VBLF + "     oe.PTNO, ";
                        SQL = SQL + ComNum.VBLF + "     TO_CHAR(oe.BDATE,'YYYY-MM-DD') AS BDATE, ";
                        SQL = SQL + ComNum.VBLF + "     oe.DEPTCODE, ";
                        SQL = SQL + ComNum.VBLF + "     oe.DRCODE, ";
                        SQL = SQL + ComNum.VBLF + "     oe.ILLCODE, ";
                        SQL = SQL + ComNum.VBLF + "     bi.ILLNAMEK, ";
                        SQL = SQL + ComNum.VBLF + "     CASE ";
                        SQL = SQL + ComNum.VBLF + "         WHEN LENGTH(oe.ILLCODE) > 3 THEN ";
                        SQL = SQL + ComNum.VBLF + "             SUBSTR(oe.ILLCODE, 0, 3) || '.' || SUBSTR(oe.ILLCODE, 4, LENGTH(oe.ILLCODE)) ";
                        SQL = SQL + ComNum.VBLF + "         ELSE ";
                        SQL = SQL + ComNum.VBLF + "             oe.ILLCODE ";
                        SQL = SQL + ComNum.VBLF + "     END AS KCDCODE, ";
                        SQL = SQL + ComNum.VBLF + "     ROWNUM AS SORT, ";
                        SQL = SQL + ComNum.VBLF + "     oe.ROWID ";
                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_EILLS oe ";
                        SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_PMPA.BAS_ILLS bi ";
                        SQL = SQL + ComNum.VBLF + "    ON oe.ILLCODE = bi.ILLCODE ";
                        SQL = SQL + ComNum.VBLF + "   AND bi.DDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + "   AND bi.ILLCLASS <> 'C' ";
                        SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND oe.BDATE >= TO_DATE('" + dt.Rows[0]["BDATE"].ToString().Trim() + "', 'YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND oe.BDATE <= TO_DATE('" + strBdate + "', 'YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY MAIN DESC, ROWID ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                rtnVal += dt1.Rows[i]["KCDCODE"].ToString().Trim() + ", ";
                            }

                            Save_MILLS(dt1, strSeqNo, strBarCode);
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }
                }
                else
                {
                    if (strIO == "O")
                    {
                        //외래
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT";
                        SQL = SQL + ComNum.VBLF + "     oo.PTNO, ";
                        SQL = SQL + ComNum.VBLF + "     TO_CHAR(oo.BDATE,'YYYY-MM-DD') AS BDATE, ";
                        SQL = SQL + ComNum.VBLF + "     oo.DEPTCODE, ";
                        SQL = SQL + ComNum.VBLF + "     '' DRCODE, ";
                        SQL = SQL + ComNum.VBLF + "     oo.ILLCODE,";
                        SQL = SQL + ComNum.VBLF + "     bi.ILLNAMEK,";
                        SQL = SQL + ComNum.VBLF + "     CASE";
                        SQL = SQL + ComNum.VBLF + "         WHEN LENGTH(oo.ILLCODE) > 3 THEN";
                        SQL = SQL + ComNum.VBLF + "             SUBSTR(oo.ILLCODE, 0, 3) || '.' || SUBSTR(oo.ILLCODE, 4, LENGTH(oo.ILLCODE))";
                        SQL = SQL + ComNum.VBLF + "         Else";
                        SQL = SQL + ComNum.VBLF + "             oo.ILLCODE";
                        SQL = SQL + ComNum.VBLF + "     END  AS KCDCODE,";
                        SQL = SQL + ComNum.VBLF + "     ROWNUM AS SORT, ";
                        SQL = SQL + ComNum.VBLF + "     oo.ROWID";
                        SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_OILLS oo";
                        SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_PMPA.BAS_ILLS bi";
                        SQL = SQL + ComNum.VBLF + "     ON oo.ILLCODE = bi.ILLCODE";
                        //SQL = SQL + ComNum.VBLF + "    AND bi.KCD7 = '*' ";
                        SQL = SQL + ComNum.VBLF + "    AND bi.DDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + "    AND bi.ILLCLASS <> 'C' ";
                        SQL = SQL + ComNum.VBLF + " WHERE oo.PTNO = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND oo.DEPTCODE = '" + strDept + "'";
                        SQL = SQL + ComNum.VBLF + "   AND oo.BDATE = TO_DATE('" + strBdate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + " ORDER BY ROWID ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                rtnVal += dt.Rows[i]["KCDCODE"].ToString().Trim() + ", ";
                            }

                            Save_MILLS(dt, strSeqNo, strBarCode);
                        }
                        dt.Dispose();
                        dt = null;
                    }
                    else
                    {
                        //입원
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT PANO, MAX(TO_CHAR(INDATE,'YYYY-MM-DD')) AS INDATE";
                        SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.IPD_NEW_MASTER inm";
                        SQL = SQL + ComNum.VBLF + "  WHERE inm.PANO = '" + strPano + "' ";
                        //SQL = SQL + ComNum.VBLF + "    AND inm.DEPTCODE = '" + strDept + "'";
                        SQL = SQL + ComNum.VBLF + "    AND TRUNC(inm.INDATE) <= TO_DATE('" + strBdate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "  GROUP BY PANO";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return rtnVal;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            SQL = "";                            
                            SQL = SQL + ComNum.VBLF + " SELECT";
                            SQL = SQL + ComNum.VBLF + "     oi.PTNO, ";
                            SQL = SQL + ComNum.VBLF + "     '' AS SEQNO, ";
                            SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE,'YYYY-MM-DD') AS BDATE, ";
                            SQL = SQL + ComNum.VBLF + "     '' AS DEPTCODE, ";
                            SQL = SQL + ComNum.VBLF + "     '' AS DRCODE, ";
                            SQL = SQL + ComNum.VBLF + "     oi.ILLCODE,";
                            SQL = SQL + ComNum.VBLF + "     bi.ILLNAMEK,";
                            SQL = SQL + ComNum.VBLF + "     CASE";
                            SQL = SQL + ComNum.VBLF + "         WHEN LENGTH(oi.ILLCODE) > 3 THEN";
                            SQL = SQL + ComNum.VBLF + "             SUBSTR(oi.ILLCODE, 0, 3) || '.' || SUBSTR(oi.ILLCODE, 4, LENGTH(oi.ILLCODE))";
                            SQL = SQL + ComNum.VBLF + "         Else";
                            SQL = SQL + ComNum.VBLF + "             oi.ILLCODE";
                            SQL = SQL + ComNum.VBLF + "     END  AS KCDCODE,";
                            SQL = SQL + ComNum.VBLF + "     ROWNUM AS SORT ";
                            //SQL = SQL + ComNum.VBLF + "     oi.MAIN,";
                            //SQL = SQL + ComNum.VBLF + "     oi.ROWID";
                            SQL = SQL + ComNum.VBLF + "  FROM ";
                            SQL = SQL + ComNum.VBLF + "(SELECT ";
                            SQL = SQL + ComNum.VBLF + "     PTNO, ILLCODE ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IILLS ";
                            SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + strPano + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND BDATE >= TO_DATE('" + dt.Rows[0]["INDATE"].ToString().Trim() + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "GROUP BY PTNO, ILLCODE) oi ";
                            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_ILLS bi ";
                            SQL = SQL + ComNum.VBLF + "   ON oi.ILLCODE = bi.ILLCODE ";
                            SQL = SQL + ComNum.VBLF + "  AND bi.DDATE IS NULL ";
                            SQL = SQL + ComNum.VBLF + "  AND bi.ILLCLASS <> 'C' ";

                         //   SQL = SQL + ComNum.VBLF + " SELECT";
                         //   SQL = SQL + ComNum.VBLF + "     oi.PTNO, ";
	                        //SQL = SQL + ComNum.VBLF + "     '' SEQNO, ";
                         //   SQL = SQL + ComNum.VBLF + "     TO_CHAR(oi.BDATE,'YYYY-MM-DD') AS BDATE, ";
                         //   SQL = SQL + ComNum.VBLF + "     oi.DEPTCODE, ";
                         //   SQL = SQL + ComNum.VBLF + "     oi.DRCODE, ";
                         //   SQL = SQL + ComNum.VBLF + "     oi.ILLCODE,";
                         //   SQL = SQL + ComNum.VBLF + "     bi.ILLNAMEK,";
                         //   SQL = SQL + ComNum.VBLF + "     CASE";
                         //   SQL = SQL + ComNum.VBLF + "         WHEN LENGTH(oi.ILLCODE) > 3 THEN";
                         //   SQL = SQL + ComNum.VBLF + "             SUBSTR(oi.ILLCODE, 0, 3) || '.' || SUBSTR(oi.ILLCODE, 4, LENGTH(oi.ILLCODE))";
                         //   SQL = SQL + ComNum.VBLF + "         Else";
                         //   SQL = SQL + ComNum.VBLF + "             oi.ILLCODE";
                         //   SQL = SQL + ComNum.VBLF + "     END  AS KCDCODE,";
                         //   SQL = SQL + ComNum.VBLF + "     ROWNUM AS SORT, ";
                         //   SQL = SQL + ComNum.VBLF + "     oi.MAIN,";
                         //   SQL = SQL + ComNum.VBLF + "     oi.ROWID";
                         //   SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_IILLS oi";
                         //   SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_PMPA.BAS_ILLS bi";
                         //   SQL = SQL + ComNum.VBLF + "     ON oi.ILLCODE = bi.ILLCODE";
                         //   //SQL = SQL + ComNum.VBLF + "    AND bi.KCD7 = '*' ";
                         //   SQL = SQL + ComNum.VBLF + "    AND bi.DDATE IS NULL ";
                         //   SQL = SQL + ComNum.VBLF + "    AND bi.ILLCLASS <> 'C' ";
                         //   SQL = SQL + ComNum.VBLF + "  WHERE oi.PTNO = '" + strPano + "' ";
                         //   SQL = SQL + ComNum.VBLF + "    AND oi.BDATE >= TO_DATE('" + dt.Rows[0]["INDATE"].ToString().Trim() + "','YYYY-MM-DD')";
                         //   SQL = SQL + ComNum.VBLF + " ORDER BY MAIN DESC, ROWID ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return rtnVal;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    rtnVal += dt1.Rows[i]["KCDCODE"].ToString().Trim() + ", ";
                                }

                                Save_MILLS(dt1, strSeqNo, strBarCode);
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }

                return VB.Left(rtnVal,rtnVal.Length - 2);
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool Save_MILLS(DataTable dt, string strSeqNo, string strBarCode)
        {
            string SQL = string.Empty;            
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            bool rtnVal = false;


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_OCS.OCS_MILLS                                 ";
                SQL = SQL + ComNum.VBLF + "(                                                                ";
                SQL = SQL + ComNum.VBLF + "    PTNO, SEQNO, BDATE, DEPTCODE, DRCODE, ILLCODE, KCDCODE, SORT, BARCODE ";
                SQL = SQL + ComNum.VBLF + ")                                                                ";
                SQL = SQL + ComNum.VBLF + "VALUES                                                           ";
                SQL = SQL + ComNum.VBLF + "(                                                                ";
                SQL = SQL + ComNum.VBLF + " '" + dt.Rows[i]["PTNO"].ToString().Trim() + "',                 ";
                SQL = SQL + ComNum.VBLF + " '" + strSeqNo + "',                                             ";
                SQL = SQL + ComNum.VBLF + " TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + " '" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "',             ";
                SQL = SQL + ComNum.VBLF + " '" + dt.Rows[i]["DRCODE"].ToString().Trim() + "',               ";
                SQL = SQL + ComNum.VBLF + " '" + dt.Rows[i]["ILLCODE"].ToString().Trim() + "',              ";
                SQL = SQL + ComNum.VBLF + " '" + dt.Rows[i]["KCDCODE"].ToString().Trim() + "',              ";
                SQL = SQL + ComNum.VBLF + " '" + dt.Rows[i]["SORT"].ToString().Trim() + "',                 ";
                SQL = SQL + ComNum.VBLF + " '" + strBarCode + "'                                            ";
                SQL = SQL + ComNum.VBLF + ")                                                                ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                                                               
                    return rtnVal;
                }
            }

            rtnVal = true;
            return rtnVal;
        }
    }
}
