using ComBase;
using ComDbB;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstDrugInfoPrint.cs
    /// Description     : 복약 안내문 출력
    /// Author          : 이정현
    /// Create Date     : 2018-01-04
    /// <history> 
    /// 복약 안내문 출력
    /// </history>
    /// <seealso>
    /// PSMH\drug\drservice\Frm복약안내문출력.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drservice\drservice.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstDrugInfoPrint : Form
    {
        #region MainFormMessage InterFace

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {

        }

        public void MsgUnloadForm(Form frm)
        {

        }

        public void MsgFormClear()
        {

        }

        public void MsgSendPara(string strPara)
        {

        }

        #endregion


        private string GstrPrtGbn = "";
        private string GstrEXEName = "";
        private string GstrHelpCode = "";
        private string GstrHelpName = "";
        private string GstrPANO = "";
        
        public frmSupDrstDrugInfoPrint()
        {
            InitializeComponent();
        }

        public frmSupDrstDrugInfoPrint(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;

            string gsWard = "";

            gsWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE", "");

            if (gsWard != "")
            {
                GstrEXEName = "NRINFO";
                GstrHelpName = gsWard;
            }
        }

        public frmSupDrstDrugInfoPrint(string strEXEName, string strHelpCode, string strHelpName)
        {
            InitializeComponent();

            GstrEXEName = strEXEName;
            GstrHelpCode = strHelpCode;
            GstrHelpName = strHelpName;
        }

        public frmSupDrstDrugInfoPrint(string strEXEName, string strHelpCode, string strHelpName, string strPANO)
        {
            InitializeComponent();

            GstrEXEName = strEXEName;
            GstrHelpCode = strHelpCode;
            GstrHelpName = strHelpName;
            GstrPANO = strPANO;
        }

        private void frmSupDrstDrugInfoPrint_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");


            ssPrint.ActiveSheet.Columns[0].Visible = false;

            SetCboWard();

            txtPano.Text = "";
            dtpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            rdoDrug0.Checked = true;

            ssPat_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 2;
            ssView_Sheet1.Cells[0, 0, 1, 0].Value = false;
            ssView_Sheet1.Cells[0, 1].Text = "";

            ssPrint_Sheet1.RowCount = 2;
            ssPrint_Sheet1.Cells[0, 0, 1, 0].Value = false;
            ssPrint_Sheet1.Cells[0, 1].Text = "";

            cboTeam.Text = "";
            cboTeam.Items.Clear();
            cboTeam.Items.Add("전체");
            cboTeam.Items.Add("A");
            cboTeam.Items.Add("B");
            cboTeam.SelectedIndex = 0;

            if (GstrHelpCode == "DRUG")
            {
                if (GstrHelpName == "ER")
                {
                    cboWard.Text = "";
                    cboWard.Items.Clear();
                    cboWard.Items.Add("ER" + VB.Space(20) + "ER");
                }
                else
                {
                    cboWard.Text = "";
                    cboWard.Items.Clear();
                    cboWard.Items.Add("외래" + VB.Space(20) + "외래");
                }

                cboWard.SelectedIndex = 0;

                //2019-04-17 유진호
                //약제팀 데레사 수녀님 요청사항
                if (GstrPANO != "")
                {
                    txtPano.Text = GstrPANO;

                    

                    GetData();
                }
            }
        }

        private void SetCboWard()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            cboWard.Text = "";
            cboWard.Items.Clear();
            cboWard.Items.Add("전체" + VB.Space(20) + "00");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     CASE";
                SQL = SQL + ComNum.VBLF + "         WHEN CODE IN('MICU','SICU') THEN 'IU'";
                SQL = SQL + ComNum.VBLF + "     ELSE";
                SQL = SQL + ComNum.VBLF + "         CODE";
                SQL = SQL + ComNum.VBLF + "     END CODE, NAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_CODE ";
                SQL = SQL + ComNum.VBLF + "     WHERE GUBUN ='2' ";
                SQL = SQL + ComNum.VBLF + "         AND GBUSE = 'Y'";
                SQL = SQL + ComNum.VBLF + "ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["NAME"].ToString().Trim() + VB.Space(20) + dt.Rows[i]["CODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboWard.Items.Add("HD" + VB.Space(20) + "HD");
                cboWard.SelectedIndex = 0;

                if (GstrHelpName != "" && GstrHelpName != null)
                {
                    ComFunc.ComboFind(cboWard, "R", 2, GstrHelpName);
                }

                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void rdoDrug_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDrug1.Checked == true)
            {
                chkToday.Enabled = false;
                chkToday.Checked = false;
            }
            else if (rdoDrug0.Checked == true)
            {
                chkToday.Enabled = true;
            }

            if (((RadioButton)sender).Checked == true) { GetData(); }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strWard = "";
            string strPANO = "";
            string strDate = "";

            ssPrt.Visible = false;
            ssPrt.Location = new Point(200, 10);
            ssPrt.Width = 100;
            ssPrt.Height = 10;

            strPANO = txtPano.Text.Trim();
            strWard = VB.Right(cboWard.Text, 4).Trim();
            strDate = dtpDate.Value.ToString("yyyy-MM-dd");

            Clear_SSpa();

            ssPat_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 2;
            ssView_Sheet1.Cells[0, 0, 1, 0].Value = false;
            ssView_Sheet1.Cells[0, 1].Text = "";

            ssPrint_Sheet1.RowCount = 2;
            ssPrint_Sheet1.Cells[0, 0, 1, 0].Value = false;
            ssPrint_Sheet1.Cells[0, 1].Text = "";

            if (strWard == "외래")
            {
                if (strPANO == "")
                {
                    ComFunc.MsgBox("외래일 경우 꼭 등록번호를 입력하세요.");
                    return;
                }
            }
            
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";

                if (strWard == "OPD" || strWard == "외래")
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     '외래' AS WARDCODE, '99' AS ROOMCODE, A.PANO, A.SNAME, A.AGE, A.BI, B.DRNAME, C.DEPTNAMEK, D.JUMIN1 || '-' || D.JUMIN2 AS JUMIN,";
                    SQL = SQL + ComNum.VBLF + "     CASE";
                    SQL = SQL + ComNum.VBLF + "         WHEN A.SEX = 'M' THEN '남' ELSE '여'";
                    SQL = SQL + ComNum.VBLF + "     END SEX, '' AS GUBUN ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C, " + ComNum.DB_PMPA + "BAS_PATIENT D, ";
                    SQL = SQL + ComNum.VBLF + "     (SELECT PTNO FROM " + ComNum.DB_MED + "OCS_OORDER ";
                    SQL = SQL + ComNum.VBLF + "         Where BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                    if (strPANO != "")
                    {
                        SQL = SQL + ComNum.VBLF + "             AND PTNO = '" + strPANO + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "     GROUP BY PTNO) X, ";
                    SQL = SQL + ComNum.VBLF + "     (SELECT DEPTCODE FROM " + ComNum.DB_MED + "OCS_DRUGATC";
                    SQL = SQL + ComNum.VBLF + "         Where BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                    if (strPANO != "")
                    {
                        SQL = SQL + ComNum.VBLF + "             AND PANO  = '" + strPANO + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "     GROUP BY DEPTCODE) Y ";
                    SQL = SQL + ComNum.VBLF + "Where X.PTNO = A.PANO";
                    SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE = Y.DEPTCODE";
                    SQL = SQL + ComNum.VBLF + "     AND A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "     AND A.DRCODE = B.DRCODE ";
                    SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE = C.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "     AND A.PANO = D.PANO";
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.SNAME, A.PANO";
                }
                else if (strWard == "HD")
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     'HD' AS WARDCODE, '99' AS ROOMCODE, A.PANO, A.SNAME, A.AGE, A.BI, B.DRNAME, C.DEPTNAMEK, D.JUMIN1 || '-' || D.JUMIN2 AS JUMIN,";
                    SQL = SQL + ComNum.VBLF + "     CASE";
                    SQL = SQL + ComNum.VBLF + "         WHEN A.SEX = 'M' THEN '남' ELSE '여'";
                    SQL = SQL + ComNum.VBLF + "     END SEX, '' AS GUBUN";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C, " + ComNum.DB_PMPA + "BAS_PATIENT D";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "         AND A.DRCODE = B.DRCODE";
                    SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE = C.DEPTCODE";

                    if (strPANO != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.PANO  = '" + strPANO + "'";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = D.PANO";
                    SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE = 'HD'";
                    SQL = SQL + ComNum.VBLF + "         AND EXISTS (SELECT * FROM " + ComNum.DB_MED + "OCS_OORDER X";
                    SQL = SQL + ComNum.VBLF + "                         WHERE X.BDate = a.BDate";
                    SQL = SQL + ComNum.VBLF + "                             AND X.PTNO = A.PANO)";
                    SQL = SQL + ComNum.VBLF + "ORDER BY  A.SNAME, A.PANO";
                }
                else if (strWard == "ER")
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     'ER' AS WARDCODE, 'ER' AS ROOMCODE, A.PANO, A.SNAME, A.AGE, A.BI, B.DRNAME, C.DEPTNAMEK, D.JUMIN1 || '-' || D.JUMIN2 AS JUMIN, ";
                    SQL = SQL + ComNum.VBLF + "     CASE";
                    SQL = SQL + ComNum.VBLF + "         WHEN A.SEX = 'M' THEN '남' ELSE '여'";
                    SQL = SQL + ComNum.VBLF + "     END SEX, '' AS GUBUN ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C, " + ComNum.DB_PMPA + "BAS_PATIENT D, ";
                    SQL = SQL + ComNum.VBLF + "     (SELECT PTNO FROM " + ComNum.DB_MED + "OCS_IORDER ";
                    SQL = SQL + ComNum.VBLF + "         Where BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "             AND GBTFLAG = 'T' ";
                    SQL = SQL + ComNum.VBLF + "             AND GBIOE IN ('E', 'EI') ";

                    if (strPANO != "")
                    {
                        SQL = SQL + ComNum.VBLF + "             AND PTNO  = '" + strPANO + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "     GROUP BY PTNO) X ";
                    SQL = SQL + ComNum.VBLF + "Where X.PTNO = A.PANO ";
                    SQL = SQL + ComNum.VBLF + "     AND A.DRCODE = B.DRCODE ";
                    SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE = C.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE = 'ER' ";
                    SQL = SQL + ComNum.VBLF + "     AND A.PANO = D.PANO";
                    //SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + dtpDate.Value.AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    //2020-04-05 2일로 수정(임시)
                    SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + dtpDate.Value.AddDays(-2).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.SNAME, A.PANO ";
                }
                else
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.WARDCODE, A.ROOMCODE, A.PANO, A.SNAME, A.AGE, A.BI, B.DRNAME, C.DEPTNAMEK, D.JUMIN1 || '-' || D.JUMIN2 AS JUMIN, ";
                    SQL = SQL + ComNum.VBLF + "     CASE";
                    SQL = SQL + ComNum.VBLF + "         WHEN A.SEX = 'M' THEN '남' ELSE '여'";
                    SQL = SQL + ComNum.VBLF + "     END SEX, '퇴원약' AS GUBUN ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C, " + ComNum.DB_PMPA + "BAS_PATIENT D, ";
                    SQL = SQL + ComNum.VBLF + "     (SELECT PTNO FROM " + ComNum.DB_MED + "OCS_IORDER ";
                    SQL = SQL + ComNum.VBLF + "               WHERE BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                    if (rdoDrug0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "                 AND GBTFLAG <> 'T' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "                 AND GBTFLAG = 'T' ";
                        SQL = SQL + ComNum.VBLF + "                 AND DIVQTY IS NULL ";
                    }

                    if (strPANO != "")
                    {
                        SQL = SQL + ComNum.VBLF + "                 AND PTNO = '" + strPANO + "' ";
                    }

                    if (rdoDrug0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     GROUP BY PTNO) X";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "     GROUP BY PTNO ";
                        SQL = SQL + ComNum.VBLF + "     HAVING SUM(QTY * NAL) > 0) X ";
                    }

                    SQL = SQL + ComNum.VBLF + "WHERE X.PTNO = A.PANO ";

                    if (chkToday.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     AND A.INDATE >= TO_DATE('" + strDate + " 00:00', 'YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "     AND A.INDATE <= TO_DATE('" + strDate + " 23:59', 'YYYY-MM-DD HH24:MI') ";
                    }


                    if (rdoDrug0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND GbSts  IN ('0') ";
                    }
                    else
                    {
                        if (chkOUT.Checked == false)
                        {
                            SQL = SQL + ComNum.VBLF + "     AND (A.ACTDate IS NULL OR A.ACTDate = TO_DATE('" + strDate + "','YYYY-MM-DD'))";
                        }
                    }

                    SQL = SQL + ComNum.VBLF + "     AND A.DRCODE = B.DRCODE ";
                    SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE = C.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + "     AND A.PANO = D.PANO";

                    if (strWard != "00")
                    {
                        //집중치료실
                        if (strWard == "MICU")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND A.ROOMCODE = '234' ";
                        }
                        else if (strWard == "SICU")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND A.ROOMCODE = '233' ";
                        }
                        else if (VB.L(cboWard.Text, "ND") > 1)
                        {
                            SQL = SQL + ComNum.VBLF + "     AND A.WARDCODE IN ('ND','IQ') ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "     AND A.WARDCODE = '" + strWard + "' ";
                        }
                    }


                    if (cboTeam.Text != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "     AND EXISTS (SELECT * FROM " + ComNum.DB_PMPA + "NUR_TEAM_ROOMCODE T";
                        SQL = SQL + ComNum.VBLF + "                     WHERE A.WARDCODE = T.WARDCODE";
                        SQL = SQL + ComNum.VBLF + "                         AND A.ROOMCODE = T.ROOMCODE";
                        SQL = SQL + ComNum.VBLF + "                         AND T.TEAM = '" + cboTeam.Text.Trim() + "')";
                    }

                    if (rdoDrug1.Checked == true && GstrEXEName == "NRINFO")
                    {
                        SQL = SQL + ComNum.VBLF + "UNION ALL";
                        SQL = SQL + ComNum.VBLF + "SELECT";
                        SQL = SQL + ComNum.VBLF + "     A.WARDCODE, A.ROOMCODE, A.PANO, A.SNAME, A.AGE, A.BI, B.DRNAME, C.DEPTNAMEK, D.JUMIN1 || '-' || D.JUMIN2 AS JUMIN,";
                        SQL = SQL + ComNum.VBLF + "     CASE";
                        SQL = SQL + ComNum.VBLF + "         WHEN A.SEX = 'M' THEN '남' ELSE '여'";
                        SQL = SQL + ComNum.VBLF + "     END SEX , '약없음' AS GUBUN";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B, " + ComNum.DB_PMPA + "BAS_CLINICDEPT C, " + ComNum.DB_PMPA + "BAS_PATIENT D";
                        SQL = SQL + ComNum.VBLF + "     Where a.DrCode = b.DrCode";
                        SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE = C.DEPTCODE";
                        SQL = SQL + ComNum.VBLF + "         AND A.PANO = D.PANO";
                        SQL = SQL + ComNum.VBLF + "         AND A.OUTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                        if (strWard != "00")
                        {
                            //집중치료실
                            if (strWard == "MICU")
                            {
                                SQL = SQL + ComNum.VBLF + "         AND A.ROOMCODE = '234' ";
                            }
                            else if (strWard == "SICU")
                            {
                                SQL = SQL + ComNum.VBLF + "         AND A.ROOMCODE = '233' ";
                            }
                            else if (VB.L(cboWard.Text, "ND") > 1)
                            {
                                SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE IN ('ND', 'IQ') ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = '" + strWard + "' ";
                            }
                        }

                        SQL = SQL + ComNum.VBLF + "         AND A.PANO NOT IN (SELECT PTNO FROM " + ComNum.DB_MED + "OCS_IORDER";
                        SQL = SQL + ComNum.VBLF + "                                 WHERE BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "                                     AND GBTFLAG = 'T'";
                        SQL = SQL + ComNum.VBLF + "                                     AND DIVQTY IS NULL";
                        SQL = SQL + ComNum.VBLF + "                             GROUP BY PTNO";
                        SQL = SQL + ComNum.VBLF + "                             HAVING SUM(QTY * NAL) > 0)";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY WARDCODE, ROOMCODE, SNAME, PANO";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " ORDER BY A.WARDCODE, A.ROOMCODE, A.SNAME, A.PANO ";
                    }
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 1)
                {
                    txtSname.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    txtRoom.Text = dt.Rows[0]["WARDCODE"].ToString().Trim() + "-" + dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    txtJumin.Text = dt.Rows[i]["JUMIN"].ToString().Trim();
                    txtDept.Text = dt.Rows[i]["DEPTNAMEK"].ToString().Trim();
                    txtDrName.Text = dt.Rows[i]["AGE"].ToString().Trim() + "세/" + dt.Rows[i]["SEX"].ToString().Trim();
                    txtBi.Text = dt.Rows[i]["BI"].ToString().Trim();
                }
                if (dt.Rows.Count > 0)
                {
                    ssPat_Sheet1.RowCount = dt.Rows.Count;
                    ssPat_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssPat_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTNAMEK"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 5].Text = dt.Rows[i]["JUMIN"].ToString().Trim();

                        if (rdoDrug0.Checked == true)
                        { ssPat_Sheet1.Cells[i, 6].Text = "재원약"; }
                        else { ssPat_Sheet1.Cells[i, 6].Text = "퇴원약"; }

                        ssPat_Sheet1.Cells[i, 7].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 9].Text = dt.Rows[i]["AGE"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 10].Text = dt.Rows[i]["BI"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 11].Text = dt.Rows[i]["GUBUN"].ToString().Trim();

                        if (ssPat_Sheet1.Cells[i, 11].Text.Trim() == "약없음")
                        {
                            ssPat_Sheet1.Cells[i, 0, i, ssPat_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(230, 200, 200);
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void Clear_SSpa()
        {
            txtRoom.Text = "";
            txtSname.Text = "";
            txtDept.Text = "";
            txtJumin.Text = "";
            txtSex.Text = "";
            txtDrName.Text = "";
            txtBi.Text = "";            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear_SSpa();

            ssPat_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 2;
            ssView_Sheet1.Cells[0, 0, 1, 0].Value = false;
            ssView_Sheet1.Cells[0, 1].Text = "";

            ssPrint_Sheet1.RowCount = 2;
            ssPrint_Sheet1.Cells[0, 0, 1, 0].Value = false;
            ssPrint_Sheet1.Cells[0, 1].Text = "";

            txtPano.Text = "";            
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            if (ssView_Sheet1.RowCount == 2) { return; }

            if (((Button)sender).Text == "전체선택")
            {
                ((Button)sender).Text = "전체해제";
                ssView_Sheet1.Cells[2, 0, ssView_Sheet1.RowCount - 2, 0].Value = true;
            }
            else
            {
                ((Button)sender).Text = "전체선택";
                ssView_Sheet1.Cells[2, 0, ssView_Sheet1.RowCount - 2, 0].Value = false;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            GetPrint();
        }

        private void GetPrint()
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            FarPoint.Win.Spread.FpSpread ssSpread = null;
            FarPoint.Win.Spread.SheetView ssSpread_Sheet = null;

            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";
            string strGubun = "";
            string strPrintName = clsPrint.gGetDefaultPrinter();
            int i = 0;
            int intCount = 0;

            if (ssView_Sheet1.RowCount > 2)
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    if (ssView_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        intCount++;
                    }
                }

                if (intCount > 0)
                {
                    //for (i = 2; i < ssView_Sheet1.RowCount - 1; i = i + 3)
                    //2021-02-02 -1 -> -2로 변경 
                    for (i = 2; i < ssView_Sheet1.RowCount - 2; i = i + 3)
                    {
                        if (ssView_Sheet1.Cells[i, 0].Text.Trim().Equals("True") == false)
                        {
                            ssView_Sheet1.Rows[i, i + 2].Visible = false;
                            ssPrint_Sheet1.Rows[i, i + 2].Visible = false;
                        }
                    }
                }

                //2019-11-19 데레사 수녀요청
                if (rdoDrug0.Checked == true)
                {
                    strGubun = "[입원약]";
                }
                else if (rdoDrug1.Checked == true)
                {
                    strGubun = "[퇴원약]";
                }
                else if (rdoDrug2.Checked == true)
                {
                    strGubun = "[외래 원내조제약]";
                }

                //strGubun = rdoDrug0.Checked == true ? "[입원약]" : "[퇴원약]";

                strFont1 = "/fn\"맑은 고딕\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs1";
                strFont2 = "/fn\"맑은 고딕\" /fz\"18\" /fb1 /fi0 /fu0 /fk0 /fs2";

                strHead1 = "/r/f1" + strGubun + "/f1/n";
                strHead2 = "/c/f2" + "복 약 안 내 문" + "/f2/n";

                if (GstrEXEName == "DRATC")
                {
                    if (VB.Left(clsType.PC_CONFIG.IPAddress, 10) == "192.168.30")
                    {
                        strPrintName = "주사집계";
                    }
                    else
                    {
                        strPrintName = "주사라벨";
                    }
                }

                ssPrint_Sheet1.PrintInfo.Printer = strPrintName;
                ssPrint_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
                ssPrint_Sheet1.PrintInfo.ZoomFactor = 0.95f;
                ssPrint_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                ssPrint_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
                ssPrint_Sheet1.PrintInfo.Margin.Top = 20;
                ssPrint_Sheet1.PrintInfo.Margin.Bottom = 20;
                ssPrint_Sheet1.PrintInfo.Margin.Header = 10;
                ssPrint_Sheet1.PrintInfo.ShowColor = true;
                ssPrint_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                ssPrint_Sheet1.PrintInfo.ShowBorder = false;
                ssPrint_Sheet1.PrintInfo.ShowGrid = false;
                ssPrint_Sheet1.PrintInfo.ShowShadows = false;
                ssPrint_Sheet1.PrintInfo.UseMax = true;
                ssPrint_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                ssPrint_Sheet1.PrintInfo.UseSmartPrint = false;
                ssPrint_Sheet1.PrintInfo.ShowPrintDialog = false;
                ssPrint_Sheet1.PrintInfo.Preview = false;
                ssPrint.PrintSheet(0);

                ComFunc.Delay(1500);
            }

            if (rdoDrug1.Checked == true && GstrEXEName == "NRINFO")
            {
                switch (GstrPrtGbn)
                {
                    case "일일수술":
                        ssSpread = ssPrtTodayOP;
                        ssSpread_Sheet = ssPrtTodayOP_Sheet1;
                        break;
                    case "신생아":
                        if (dtpDate.Value.Date >= Convert.ToDateTime("2020-04-22"))
                        {
                            ssSpread = ssPrtNewBorn2;
                            ssSpread_Sheet = ssPrtNewBorn2_Sheet1;
                        }
                        else
                        {
                            ssSpread = ssPrtNewBorn;
                            ssSpread_Sheet = ssPrtNewBorn_Sheet1;
                        }
                        break;
                    default:
                        if (dtpDate.Value.Date >= Convert.ToDateTime("2020-04-22"))
                        {
                            ssSpread = ssPrt2;
                            ssSpread_Sheet = ssPrt2_Sheet1;
                        }
                        else
                        {
                            ssSpread = ssPrt;
                            ssSpread_Sheet = ssPrt_Sheet1;
                        }
                        break;
                }

                ssSpread_Sheet.PrintInfo.Printer = strPrintName;
                ssSpread_Sheet.PrintInfo.AbortMessage = "프린터 중입니다.";
                ssSpread_Sheet.PrintInfo.ZoomFactor = 1.15f;
                ssSpread_Sheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                ssSpread_Sheet.PrintInfo.Margin.Top = 20;
                ssSpread_Sheet.PrintInfo.Margin.Bottom = 20;
                ssSpread_Sheet.PrintInfo.ShowColor = false;
                ssSpread_Sheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                ssSpread_Sheet.PrintInfo.ShowBorder = false;
                ssSpread_Sheet.PrintInfo.ShowGrid = false;
                ssSpread_Sheet.PrintInfo.ShowShadows = false;
                ssSpread_Sheet.PrintInfo.UseMax = true;
                ssSpread_Sheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                ssSpread_Sheet.PrintInfo.UseSmartPrint = false;
                ssSpread_Sheet.PrintInfo.ShowPrintDialog = false;
                ssSpread_Sheet.PrintInfo.Preview = false;
                ssSpread.PrintSheet(0);

                ComFunc.Delay(1500);
            }

            ssView_Sheet1.Rows[0, ssView_Sheet1.RowCount - 1].Visible = true;
            ssPrint_Sheet1.Rows[0, ssPrint_Sheet1.RowCount - 1].Visible = true;

            //Application.DoEvents();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPano.Text.Trim() == "") { return; }
                txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");
                GetData();
            }
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            if (txtPano.Text.Trim() == "") { return; }
            txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");
            GetData();
        }

        private void ssPat_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int k = 0;

            string strPANO = "";
            string strDate = "";
            string strWard = "";
            string strDosCode = "";
            string strSuCode = "";
            string strIMAGE = "";
            string strDrugClear = "";
            string strRoom = "";

            FarPoint.Win.ComplexBorder BorderWhite = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder Border = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.Spread.CellType.CheckBoxCellType chkType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

            FarPoint.Win.Spread.CellType.TextCellType txtType = new FarPoint.Win.Spread.CellType.TextCellType();
            txtType.Multiline = true;
            txtType.WordWrap = true;

            FarPoint.Win.Spread.CellType.ImageCellType imgType = new FarPoint.Win.Spread.CellType.ImageCellType();
            imgType.Style = FarPoint.Win.RenderStyle.Stretch;

            Clear_SSpa();

            ssView_Sheet1.RowCount = 2;
            ssView_Sheet1.Cells[0, 0, 1, 0].Value = false;
            ssView_Sheet1.Cells[0, 1].Text = "";

            ssPrint_Sheet1.RowCount = 2;
            ssPrint_Sheet1.Cells[0, 0, 1, 0].Value = false;
            ssPrint_Sheet1.Cells[0, 1].Text = "";

            strDate = dtpDate.Value.ToString("yyyy-MM-dd");
            txtRoom.Text = ssPat_Sheet1.Cells[e.Row, 0].Text.Trim();
            strWard = txtRoom.Text.Trim();

            txtRoom.Text = txtRoom.Text + "-" + ssPat_Sheet1.Cells[e.Row, 1].Text.Trim();
            strRoom = ssPat_Sheet1.Cells[e.Row, 1].Text.Trim();

            strPANO = ssPat_Sheet1.Cells[e.Row, 2].Text.Trim();
            txtSname.Text = ssPat_Sheet1.Cells[e.Row, 3].Text.Trim();
            txtDept.Text = ssPat_Sheet1.Cells[e.Row, 4].Text.Trim();
            txtJumin.Text = ssPat_Sheet1.Cells[e.Row, 5].Text.Trim();
            txtDrName.Text = ssPat_Sheet1.Cells[e.Row, 7].Text.Trim();
            txtSex.Text = ssPat_Sheet1.Cells[e.Row, 9].Text.Trim();
            txtSex.Text = VB.Val(txtSex.Text).ToString("00") + "세/" + ssPat_Sheet1.Cells[e.Row, 8].Text.Trim();
            txtBi.Text = ssPat_Sheet1.Cells[e.Row, 10].Text.Trim();

            ssPrt.Visible = false;
            ssPrt.Location = new Point(200, 10);
            ssPrt.Width = 100;
            ssPrt.Height = 10;

            if (DateTime.Now.Date >= Convert.ToDateTime("2020-04-22"))
            {
                ssPrt2.Visible = false;
                ssPrt2.Location = new Point(200, 10);
                ssPrt2.Width = 100;
                ssPrt2.Height = 10;
            }
            
            

            if (ssPat_Sheet1.Cells[e.Row, 11].Text.Trim() == "약없음")
            {
                ComFunc.MsgBox("퇴원약이 없습니다."
                                + ComNum.VBLF + "퇴원교육안내서만 조회됩니다."
                                + ComNum.VBLF + "확인하고 인쇄하시기 바랍니다.");
                strDrugClear = "퇴원약 없음";

                if (DateTime.Now.Date >= Convert.ToDateTime("2020-04-22"))
                {
                    ssPrt2.Visible = true;
                    ssPrt2.BringToFront();
                    ssPrt2.Location = new Point(0, 238);
                    ssPrt2.Width = 610;
                    ssPrt2.Height = 400;
                }
                else
                {
                    ssPrt.Visible = true;
                    ssPrt.Location = new Point(0, 238);
                    ssPrt.Width = 610;
                    ssPrt.Height = 400;
                }
                
            }

            if (rdoDrug1.Checked == true)
            {
                if (strWard == "NR" || strWard == "IQ")
                {
                    GstrPrtGbn = "신생아";
                    READ_퇴원간호_신생아(strPANO, strDate, strDate, strDrugClear);
                }
                else
                {
                    GstrPrtGbn = "기타";
                    READ_퇴원간호_NEW(strPANO, strDate, strDate, strDrugClear);
                }
            }

            if (GstrEXEName == "DRSERVICE")
            {
                ssView_Sheet1.Cells[0, 1].Text = "병실:" + VB.Left(txtRoom.Text + VB.Space(9), 9) + "  환자명:" + VB.Left(txtSname.Text + VB.Space(4), 4) +
                             "(" + txtSex.Text + ")" + VB.Space(3) + READ_TUYAKNO(clsDB.DbCon, strPANO, strDate) + VB.Space(7) + "진료과:" + VB.Left(txtDept.Text + VB.Space(6), 6) + VB.Space(2) +
                             "   의사명:" + VB.Left(txtDrName.Text + VB.Space(4), 4) + VB.Space(3) + "  출력일자:" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

                ssPrint_Sheet1.Cells[0, 1].Text = "병실:" + VB.Left(txtRoom.Text + VB.Space(9), 9) + "  환자명:" + VB.Left(txtSname.Text + VB.Space(4), 4) +
                             "(" + txtSex.Text + ")" + VB.Space(3) + READ_TUYAKNO(clsDB.DbCon, strPANO, strDate) + VB.Space(7) + "진료과:" + VB.Left(txtDept.Text + VB.Space(6), 6) + VB.Space(2) +
                             "   의사명:" + VB.Left(txtDrName.Text + VB.Space(4), 4) + VB.Space(3) + "  출력일자:" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            }
            else
            {
                ssView_Sheet1.Cells[0, 1].Text = "병실:" + VB.Left(txtRoom.Text + VB.Space(9), 9) + "환자명:" + VB.Left(txtSname.Text + VB.Space(4), 4) + "(" + txtSex.Text + ")" +
                             VB.Space(3) + "등록번호:" + strPANO + VB.Space(7) + "진료과:" + VB.Left(txtDept.Text + VB.Space(6), 6) + VB.Space(2) +
                             "의사명:" + VB.Left(txtDrName.Text + VB.Space(4), 4) + VB.Space(3) + "출력일자:" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

                ssPrint_Sheet1.Cells[0, 1].Text = "병실:" + VB.Left(txtRoom.Text + VB.Space(9), 9) + "환자명:" + VB.Left(txtSname.Text + VB.Space(4), 4) + "(" + txtSex.Text + ")" +
                             VB.Space(3) + "등록번호:" + strPANO + VB.Space(7) + "진료과:" + VB.Left(txtDept.Text + VB.Space(6), 6) + VB.Space(2) +
                             "의사명:" + VB.Left(txtDrName.Text + VB.Space(4), 4) + VB.Space(3) + "출력일자:" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";

                if ((strWard == "외래" || strWard == "HD") && GstrHelpCode != "DRUG")
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3, B.ENREMARK4, B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "     B.ENREMARK5, C.IDOSNAME, A.DOSCODE, B.IMAGE_YN, A.SUCODE, B.JEHENG  ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW B, " + ComNum.DB_MED + "OCS_ODOSAGE C ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PTNO = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.BUN IN ('11','12') OR (A.BUN = '20' AND B.SUNEXT LIKE 'H-%' AND B.ENREMARK3 LIKE '%인슐린%'))";
                    SQL = SQL + ComNum.VBLF + "         AND A.DOSCODE = C.DOSCODE ";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBSUNAP = '1' ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3,  B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "         B.ENREMARK4, B.ENREMARK5, C.IDOSNAME, A.DOSCODE,B.IMAGE_YN, B.JEHENG ";
                }
                else if ((strWard == "외래" || strWard == "HD") && GstrHelpCode == "DRUG")
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3, B.ENREMARK4, B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "     B.ENREMARK5, C.IDOSNAME, A.DOSCODE, B.IMAGE_YN, A.SUCODE, B.JEHENG";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW B, ";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_MED + "OCS_ODOSAGE C, " + ComNum.DB_MED + "OCS_DRUGATC D";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "         AND A.PtNO = '" + strPANO + "'";
                    SQL = SQL + ComNum.VBLF + "         AND A.PTNO = D.PANO";
                    SQL = SQL + ComNum.VBLF + "         AND A.BDATE = D.BDATE";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = D.SUCODE";
                    
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT";
                    SQL = SQL + ComNum.VBLF + "         AND (A.BUN IN ('11','12') OR (A.BUN = '20' AND B.SUNEXT LIKE 'H-%' AND B.ENREMARK3 LIKE '%인슐린%'))";
                    SQL = SQL + ComNum.VBLF + "         AND A.DOSCODE = C.DOSCODE";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBSUNAP = '1'";
                    //2020-09-25 안정수 추가, 전산의뢰 <2020-2198> 진료과 추가                    
                    SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE = '" + READ_DEPTCODE(ssPat.ActiveSheet.Cells[e.Row, 4].Text.Trim()) + "'";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3,  B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "         B.ENREMARK4, B.ENREMARK5, C.IDOSNAME, A.DOSCODE,B.IMAGE_YN, B.JEHENG ";
                }
                else if (strWard == "ER")
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3, B.ENREMARK7,  ";
                    SQL = SQL + ComNum.VBLF + "     B.ENREMARK4, B.ENREMARK5, C.IDOSNAME, A.DOSCODE, B.IMAGE_YN, A.SUCODE, B.JEHENG  ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER A, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW B, " + ComNum.DB_MED + "OCS_ODOSAGE C ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBTFLAG = 'T' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PtNO = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.BUN IN ('11','12') OR (A.BUN = '20' AND B.SUNEXT LIKE 'H-%' AND B.ENREMARK3 LIKE '%인슐린%')) ";
                    SQL = SQL + ComNum.VBLF + "         AND A.DOSCODE = C.DOSCODE ";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBPRN <> 'P' ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.GBSTATUS IS NULL OR A.GBSTATUS =' ')";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBIOE IN ('E','EI')";
                    
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3,  B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "         B.ENREMARK4, B.ENREMARK5, C.IDOSNAME, A.DOSCODE,B.IMAGE_YN, B.JEHENG  ";
                }
                else
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3,  B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "     B.ENREMARK4, B.ENREMARK5, C.IDOSNAME, A.DOSCODE, B.IMAGE_YN, B.JEHENG";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER A, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW B, " + ComNum.DB_MED + "OCS_ODOSAGE C ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                    if (rdoDrug0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.GBTFLAG <> 'T' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.GBTFLAG = 'T' ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.PtNO = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.BUN IN ('11','12')  OR (A.BUN = '20' AND B.SUNEXT LIKE 'H-%' AND B.REMARK041 LIKE '%인슐린%'))";
                    SQL = SQL + ComNum.VBLF + "         AND A.DOSCODE = C.DOSCODE ";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBPRN <> 'P' ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.GBSTATUS IS NULL OR A.GBSTATUS =' ')";
                    
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3, B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "         B.ENREMARK4, B.ENREMARK5, C.IDOSNAME, A.DOSCODE, B.IMAGE_YN, B.JEHENG ";
                }

                //SQL = SQL + ComNum.VBLF + "ORDER BY C.RANKING           ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    Dir_Check(@"C:\PSMHEXE\YAK_IMAGE\");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 3;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 3, ComNum.SPDROWHT * 2);
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 2, ComNum.SPDROWHT * 3);
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT * 2);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 3;
                        ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 3, ComNum.SPDROWHT * 2);
                        ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 2, ComNum.SPDROWHT * 3);
                        ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 1, ComNum.SPDROWHT * 2);

                        for (k = 0; k < ssView_Sheet1.ColumnCount; k++)
                        {
                            if (k != 2)
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 0].CellType = chkType;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, k].RowSpan = 3;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, k].Border = Border;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, k].CellType = txtType;

                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 0].CellType = chkType;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, k].RowSpan = 3;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, k].Border = Border;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, k].CellType = txtType;
                            }
                            else
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, k].Border = BorderWhite;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, k].CellType = txtType;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, k].Border = BorderWhite;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, k].CellType = imgType;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].Border = Border;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].CellType = txtType;

                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, k].Border = BorderWhite;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, k].CellType = txtType;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, k].Border = BorderWhite;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, k].CellType = imgType;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, k].Border = Border;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, k].CellType = txtType;
                            }
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 1].Text = (i + 1).ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 2].Text = dt.Rows[i]["ENREMARK1"].ToString().Trim();

                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 1].Text = (i + 1).ToString();
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 2].Text = dt.Rows[i]["ENREMARK1"].ToString().Trim();

                        strSuCode = dt.Rows[i]["SUCODE"].ToString().Trim();
                        strIMAGE = dt.Rows[i]["IMAGE_YN"].ToString().Trim();

                        if (strIMAGE == "Y")
                        {
                            string strFile = "";
                            string strHostFile = "";
                            string strHost = "";

                            strFile = @"C:\PSMHEXE\YAK_IMAGE\" + strSuCode.Trim().Replace("/", "__").ToUpper();
                            strHostFile = "/data/YAK_IMAGE/" + strSuCode.Trim().Replace("/", "__").ToUpper();
                            strHost = "/data/YAK_IMAGE/";

                            Image img = null;

                            FileInfo f = new FileInfo(strFile);
                            
                            try
                            {
                                if (f.Exists == true)
                                {
                                    f.Delete();
                                }

                                using (Ftpedt FtpedtX = new Ftpedt())
                                {
                                    if (FtpedtX.FtpConnetBatch("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle")) == false)
                                    {
                                        ComFunc.MsgBox("FTP Server Connect ERROR !!!", "오류");
                                        return;
                                    }

                                    if (FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFile, strHostFile, strHost) == true)
                                    {
                                        using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(strFile)))
                                        {
                                            Image tmpImg = Image.FromStream(ms);
                                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].Value = tmpImg;
                                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, 2].Value = tmpImg;
                                        }
                                    }

                                    FtpedtX.FtpDisConnetBatch();
                                }
                            }
                            catch
                            {
                                if (f.Exists == true)
                                {
                                    using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(strFile)))
                                    {
                                        img = Image.FromStream(ms);
                                    }

                                    //img = Image.FromFile(strFile);
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].Value = img;
                                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, 2].Value = img;

                                    //img = null;
                                }
                                else
                                {
                                    using (Ftpedt FtpedtX = new Ftpedt())
                                    {
                                        if (FtpedtX.FtpConnetBatch("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle")) == false)
                                        {
                                            ComFunc.MsgBox("FTP Server Connect ERROR !!!", "오류");
                                            return;
                                        }

                                        if (FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFile, strHostFile, strHost) == true)
                                        {
                                            using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(strFile)))
                                            {
                                                img = Image.FromStream(ms);
                                            }

                                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].Value = img;
                                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, 2].Value = img;
                                        }

                                        FtpedtX.FtpDisConnetBatch();
                                    }
                                }
                            }
                        }
                        else
                        {
                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 2, ComNum.SPDROWHT * 2);
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].CellType = txtType;
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].Text = dt.Rows[i]["ENREMARK2"].ToString().Trim();

                            ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 2, ComNum.SPDROWHT * 2);
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, 2].CellType = txtType;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, 2].Text = dt.Rows[i]["ENREMARK2"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].Text = dt.Rows[i]["ENREMARK1"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["JEHENG"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 4].Text = dt.Rows[i]["ENREMARK3"].ToString().Trim();

                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, 2].Text = dt.Rows[i]["ENREMARK1"].ToString().Trim();
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["JEHENG"].ToString().Trim();
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 4].Text = dt.Rows[i]["ENREMARK3"].ToString().Trim();

                        switch (VB.Left(dt.Rows[i]["DOSCODE"].ToString().Trim(), 2))
                        {
                            case "01": strDosCode = "1일 1회"; break;
                            case "02": strDosCode = "1일 2회"; break;
                            case "03": strDosCode = "1일 3회"; break;
                            case "04": strDosCode = "1일 4회"; break;
                            default: strDosCode = ""; break;
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 6].Text = strDosCode + ComNum.VBLF + dt.Rows[i]["IDOSNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 8].Text = dt.Rows[i]["ENREMARK5"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 10].Text = dt.Rows[i]["ENREMARK7"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 11].Text = dt.Rows[i]["SUCODE"].ToString().Trim();

                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 6].Text = strDosCode + ComNum.VBLF + dt.Rows[i]["IDOSNAME"].ToString().Trim();
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 8].Text = dt.Rows[i]["ENREMARK5"].ToString().Trim();
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 10].Text = dt.Rows[i]["ENREMARK7"].ToString().Trim();
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 11].Text = dt.Rows[i]["SUCODE"].ToString().Trim();

                        byte[] a = System.Text.Encoding.Default.GetBytes(ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 8].Text);
                        int intHeight = Convert.ToInt32(a.Length / 30);
                        int intHeightValue = 0;

                        if (ComNum.SPDROWHT + (intHeight * 17) >
                            (ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 3].Height
                            + ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 2].Height
                            + ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].Height))
                        {
                            intHeightValue = (int)((ComNum.SPDROWHT + (intHeight * 17)) -
                                                    (ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 3].Height
                                                        + ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 2].Height
                                                        + ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 1].Height));

                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 3, (int)(ssView_Sheet1.Rows[ssView_Sheet1.RowCount - 3].Height + intHeightValue));
                            ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 3, (int)(ssPrint_Sheet1.Rows[ssPrint_Sheet1.RowCount - 3].Height + intHeightValue));
                        }
                    }

                    #region 2021-02-01

                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Font = new Font("맑은 고딕", 9f);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].ColumnSpan = 11;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Border = Border;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Border = Border;

                    ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                    ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Font = new Font("맑은 고딕", 9f);
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].ColumnSpan = 11;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Border = Border;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Border = Border;

                    if (GstrHelpCode == "DRUG")
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "♡의약품부작용 피해구제상담 : 한국의약품안전관리원(1644-6223, 14-3330)";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = "♡의약품부작용 피해구제상담 : 한국의약품안전관리원(1644-6223, 14-3330)";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "♡의약품부작용 피해구제상담 : 한국의약품안전관리원(1644-6223, 14-3330)";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = "♡의약품부작용 피해구제상담 : 한국의약품안전관리원(1644-6223, 14-3330)";
                    }

                    #endregion

                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Font = new Font("맑은 고딕", 9f); 
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].ColumnSpan = 11;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Border = Border;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Border = Border;

                    ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                    ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Font = new Font("맑은 고딕", 9f);
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].ColumnSpan = 11;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Border = Border;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Border = Border;

                    if (GstrHelpCode == "DRUG")
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "♡복용하시는 약에 관하여 궁금하신 사항은 약제팀(054-260-8058 또는 054-260-8057)로 문의하여 주십시오. ♠♠포항성모병원♠♠";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = "♡복용하시는 약에 관하여 궁금하신 사항은 약제팀(054-260-8058 또는 054-260-8057)로 문의하여 주십시오. ♠♠포항성모병원♠♠";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "♡복용하시는 약에 관하여 궁금하신 사항은 간호사실로 문의하여 주십시오.                  ♠♠포항성모병원♠♠";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = "♡복용하시는 약에 관하여 궁금하신 사항은 간호사실로 문의하여 주십시오.                  ♠♠포항성모병원♠♠";
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                //if (FtpedtX != null)
                //{
                //    FtpedtX.FtpDisConnetBatch();
                //    FtpedtX = null;
                //}

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private string READ_TUYAKNO(PsmhDb pDbCon, string strPano, string strBDate)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     MAX(TUYAKNO) AS TUYAKNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGATC";
                SQL = SQL + ComNum.VBLF + "     WHERE BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND GBIO = '1'";
                SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "   약번호 : " + dt.Rows[0]["TUYAKNO"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }
        }

        private void Dir_Check(string sDirPath, string sExe = "*.*")
        {
            DirectoryInfo Dir = new DirectoryInfo(sDirPath);

            if (Dir.Exists == false)
            {
                Dir.Create();
            }
            else
            {
                FileInfo[] File = Dir.GetFiles(sExe, SearchOption.AllDirectories);

                foreach (FileInfo file in File)
                {
                    //file.Delete();
                }
            }
        }

        private void CLEAR_SSPrt_일일수술환자()
        {
            ssPrtTodayOP_Sheet1.Cells[7, 3].Text = "";
            ssPrtTodayOP_Sheet1.Cells[9, 3].Text = "";
            ssPrtTodayOP_Sheet1.Cells[10, 3].Text = "";
            ssPrtTodayOP_Sheet1.Cells[11, 3].Text = "";
            ssPrtTodayOP_Sheet1.Cells[12, 3].Text = "";
            ssPrtTodayOP_Sheet1.Cells[13, 3].Text = "";

            ssPrtTodayOP_Sheet1.Cells[15, 5].Text = "";
            ssPrtTodayOP_Sheet1.Cells[16, 5].Text = "";
            ssPrtTodayOP_Sheet1.Cells[17, 5].Text = "";

            ssPrtTodayOP_Sheet1.Cells[19, 3].Text = "";

            ssPrtTodayOP_Sheet1.Cells[21, 5].Text = "";
            ssPrtTodayOP_Sheet1.Cells[22, 5].Text = "";
            ssPrtTodayOP_Sheet1.Cells[23, 5].Text = "";
            ssPrtTodayOP_Sheet1.Cells[24, 5].Text = "";

            ssPrtTodayOP_Sheet1.Cells[26, 3].Text = "";

            ssPrtTodayOP_Sheet1.Cells[29, 5].Text = "";
            ssPrtTodayOP_Sheet1.Cells[30, 5].Text = "";
            ssPrtTodayOP_Sheet1.Cells[31, 5].Text = "";

            ssPrtTodayOP_Sheet1.Cells[33, 3].Text = "";
            ssPrtTodayOP_Sheet1.Cells[35, 3].Text = "";
            ssPrtTodayOP_Sheet1.Cells[37, 3].Text = "";
            ssPrtTodayOP_Sheet1.Cells[39, 3].Text = "";
        }

        private void READ_퇴원간호_일일수술(string strPtno, string strInDate, string strOutDate, string strDrug = "", string strWARD = "", string strRoom = "")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strTemp = "";
            string strGubun = "";

            CLEAR_SSPrt_일일수술환자();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.SEX, A.JUMIN1, A.JUMIN2, B.OUTDATE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "         AND B.OUTDATE = TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssPrtTodayOP_Sheet1.Cells[4, 0].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssPrtTodayOP_Sheet1.Cells[4, 2].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssPrtTodayOP_Sheet1.Cells[4, 4].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[0]["JUMIN1"].ToString().Trim() + dt.Rows[0]["JUMIN2"].ToString().Trim());
                    ssPrtTodayOP_Sheet1.Cells[4, 6].Text = "   주민등록번호 : " + dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1) + "******";

                    ssPrtTodayOP_Sheet1.Cells[7, 3].Text = Convert.ToDateTime(dt.Rows[0]["OUTDATE"].ToString().Trim()).ToString("yyyy년 MM월 dd일");
                }

                dt.Dispose();
                dt = null;

                //퇴원약 여부
                ssPrtTodayOP_Sheet1.Cells[4, 10].Text = strDrug;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, TO_CHAR(RDATE, 'YYYY-MM-DD PM HH:MI') AS RDATE, TO_CHAR(RDATE,'YYYY-MM-DD') AS RDATE2, A.DEPTCODE, B.DRNAME, '<예약 예정일>' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_RESERVED A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.RDATE >=  TO_DATE('" + strOutDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "         AND A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT";
                //19-09-06 원무과에서 요청으로 <수납완료> 삭제함 (컴플레인 들어왔다고 함).
                SQL = SQL + ComNum.VBLF + "     A.PANO, TO_CHAR(RDATE, 'YYYY-MM-DD PM HH:MI') AS RDATE, TO_CHAR(RDATE,'YYYY-MM-DD') AS RDATE2, A.DEPTCODE, B.DRNAME, '' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "     (SELECT";
                SQL = SQL + ComNum.VBLF + "         PANO, DEPTCODE, DECODE(DATE2, DATE3, DATE2, DATE3) AS RDATE, DRCODE";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW";
                SQL = SQL + ComNum.VBLF + "         WHERE PANO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "             AND (DATE2 >= TO_DATE('" + strOutDate + "','YYYY-MM-DD') OR DATE2 >= TO_DATE('" + strOutDate + "','YYYY-MM-DD') )";
                SQL = SQL + ComNum.VBLF + "     ) A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "MINUS ";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, TO_CHAR(RDATE, 'YYYY-MM-DD PM HH:MI') AS RDATE, TO_CHAR(RDATE,'YYYY-MM-DD') AS RDATE2, A.DEPTCODE, B.DRNAME, '<예약 예정일>' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_RESERVED A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.RDATE >=  TO_DATE('" + strOutDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "         AND A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "ORDER BY RDATE ASC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i > 4) { break; }

                        strTemp = "";
                        strGubun = dt.Rows[i]["GUBUN"].ToString().Trim();
                        strTemp = (i + 1).ToString("0") + ") " + (strGubun != "" ? "(" + strGubun + ")" : "") + dt.Rows[i]["RDATE"].ToString().Trim() + "(" + VB.Left(clsVbfunc.GetYoIl(dt.Rows[i]["RDATE2"].ToString().Trim()), 1) + ")";
                        strTemp = strTemp + "    ◈ 진료과 : " + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[i]["DEPTCODE"].ToString().Trim());
                        strTemp = strTemp + "    ◈ 진료의사 :  " + dt.Rows[i]["DRNAME"].ToString().Trim();

                        ssPrtTodayOP_Sheet1.Cells[i + 9, 3].Text = strTemp;
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik118') IK118, extractValue(chartxml, '//ik119') IK119, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik113') IK113, extractValue(chartxml, '//ik115') IK115, extractValue(chartxml, '//ik114') IK114, extractValue(chartxml, '//ik116') IK116, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik107') IK107, extractValue(chartxml, '//ik110') IK110, extractValue(chartxml, '//ik108') IK108, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik111') IK111, extractValue(chartxml, '//ik109') IK109, extractValue(chartxml, '//ik112') IK112, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik117') IK117, extractValue(chartxml, '//it34') IT34, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik104') IK104, extractValue(chartxml, '//ik105') IK105, extractValue(chartxml, '//ik106') IK106, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik94') IK94, extractValue(chartxml, '//ik95') IK95, extractValue(chartxml, '//ik96') IK96, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik97') IK97, extractValue(chartxml, '//ik120') IK120, extractValue(chartxml, '//ik121') IK121 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "         AND CHARTDATE >= '" + strInDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "         AND CHARTDATE <= '" + strOutDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "         AND FORMNO = 2070";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strTemp = "";

                    if (dt.Rows[0]["IK118"].ToString().Trim() == "true") { strTemp += "복약안내, "; }
                    if (dt.Rows[0]["IK119"].ToString().Trim() == "true") { strTemp += "설명문제공, "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtTodayOP_Sheet1.Cells[16, 5].Text = "";

                    strTemp = "";

                    if (dt.Rows[0]["IK113"].ToString().Trim() == "true") { strTemp += "일상생활, "; }
                    if (dt.Rows[0]["IK115"].ToString().Trim() == "true") { strTemp += "운동, "; }
                    if (dt.Rows[0]["IK114"].ToString().Trim() == "true") { strTemp += "안정, "; }
                    if (dt.Rows[0]["IK116"].ToString().Trim() == "true") { strTemp += "절대안정, "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtTodayOP_Sheet1.Cells[21, 5].Text = "";

                    strTemp = "";

                    if (dt.Rows[0]["IK107"].ToString().Trim() == "true") { strTemp += "정상식, "; }
                    if (dt.Rows[0]["IK110"].ToString().Trim() == "true") { strTemp += "반유동식, "; }
                    if (dt.Rows[0]["IK108"].ToString().Trim() == "true") { strTemp += "유동식, "; }
                    if (dt.Rows[0]["IK111"].ToString().Trim() == "true") { strTemp += "저염식, "; }
                    if (dt.Rows[0]["IK109"].ToString().Trim() == "true") { strTemp += "고단백식, "; }
                    if (dt.Rows[0]["IK112"].ToString().Trim() == "true") { strTemp += "저잔튜식, "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtTodayOP_Sheet1.Cells[22, 5].Text = "";

                    strTemp = "";

                    if (dt.Rows[0]["IK117"].ToString().Trim() == "true") { strTemp += "외래통원, "; }
                    if (dt.Rows[0]["IT34"].ToString().Trim() == "true") { strTemp += "기타 : " + dt.Rows[0]["IT34"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtTodayOP_Sheet1.Cells[24, 5].Text = "";

                    strTemp = "";

                    if (dt.Rows[0]["IK104"].ToString().Trim() == "true") { strTemp += "도보, "; }
                    if (dt.Rows[0]["IK105"].ToString().Trim() == "true") { strTemp += "휠체어, "; }
                    if (dt.Rows[0]["IK106"].ToString().Trim() == "true") { strTemp += "운반차, "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtTodayOP_Sheet1.Cells[29, 5].Text = "";

                    strTemp = "";

                    if (dt.Rows[0]["IK94"].ToString().Trim() == "true") { strTemp += "명료, "; }
                    if (dt.Rows[0]["IK95"].ToString().Trim() == "true") { strTemp += "혼돈, "; }
                    if (dt.Rows[0]["IK96"].ToString().Trim() == "true") { strTemp += "반의식, "; }
                    if (dt.Rows[0]["IK97"].ToString().Trim() == "true") { strTemp += "무의식, "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtTodayOP_Sheet1.Cells[30, 5].Text = "";

                    strTemp = "";

                    if (dt.Rows[0]["IK120"].ToString().Trim() == "true") { strTemp += "본인, "; }
                    if (dt.Rows[0]["IK121"].ToString().Trim() == "true") { strTemp += "보호자, "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtTodayOP_Sheet1.Cells[35, 3].Text = "";

                    ssPrtTodayOP_Sheet1.Cells[37, 3].Text = "054-260-" + READ_NUR_TEAM_TEL(clsDB.DbCon, strWARD, strRoom);

                    ssPrtTodayOP_Sheet1.Cells[43, 0].Text = "출력일시 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");
                    ssPrtTodayOP_Sheet1.Cells[43, 9].Text = "출력자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private string READ_NUR_TEAM_TEL(PsmhDb pDbCon, string strWardCode, string strROOMCODE)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TEL";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_TEAM_ROOMCODE A, " + ComNum.DB_PMPA + "NUR_TEAM B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.WARDCODE = B.WARDCODE ";
                SQL = SQL + ComNum.VBLF + "         AND A.TEAM = B.TEAM";
                SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = '" + strWardCode + "'";
                SQL = SQL + ComNum.VBLF + "         AND A.ROOMCODE = '" + strROOMCODE + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["TEL"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }
        }

        private void CLEAR_SSPRT_NewBorn()
        {
            ssPrtNewBorn_Sheet1.Cells[9, 3, 13, 3].Text = "";

            ssPrtNewBorn_Sheet1.Cells[15, 5].Text = "";
            ssPrtNewBorn_Sheet1.Cells[16, 5].Text = "";
            ssPrtNewBorn_Sheet1.Cells[18, 3].Text = "";
            ssPrtNewBorn_Sheet1.Cells[20, 5].Text = "";
            ssPrtNewBorn_Sheet1.Cells[21, 5].Text = "";
            ssPrtNewBorn_Sheet1.Cells[22, 5].Text = "";
            ssPrtNewBorn_Sheet1.Cells[23, 5].Text = "";
            ssPrtNewBorn_Sheet1.Cells[24, 5].Text = "";
            ssPrtNewBorn_Sheet1.Cells[25, 5].Text = "";
            ssPrtNewBorn_Sheet1.Cells[27, 5].Text = "";
            ssPrtNewBorn_Sheet1.Cells[30, 5].Text = "";
            ssPrtNewBorn_Sheet1.Cells[32, 3].Text = "";
            ssPrtNewBorn_Sheet1.Cells[34, 3].Text = "";
            ssPrtNewBorn_Sheet1.Cells[36, 3].Text = "054-260-8563";

            ssPrtNewBorn_Sheet1.Cells[42, 0].Text = "";
            ssPrtNewBorn_Sheet1.Cells[42, 9].Text = "";

            ssPrtNewBorn2_Sheet1.Cells[9, 3].Text = "";
        }

        private void READ_퇴원간호_신생아(string strPtno, string strInDate, string strOutDate, string strDrug = "")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strTemp = "";
            string strGubun = "";

            CLEAR_SSPRT_NewBorn();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.SEX, A.JUMIN1, A.JUMIN2, B.OUTDATE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "         AND B.OUTDATE = TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    if (DateTime.Now.Date >= Convert.ToDateTime("2020-04-22"))
                    {
                        ssPrtNewBorn2_Sheet1.Cells[4, 0].Text = dt.Rows[0]["PANO"].ToString().Trim();
                        ssPrtNewBorn2_Sheet1.Cells[4, 2].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                        ssPrtNewBorn2_Sheet1.Cells[4, 4].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[0]["JUMIN1"].ToString().Trim() + dt.Rows[0]["JUMIN2"].ToString().Trim());
                        ssPrtNewBorn2_Sheet1.Cells[4, 6].Text = "   주민등록번호 : " + dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1) + "******";
                        ssPrtNewBorn2_Sheet1.Cells[7, 3].Text = Convert.ToDateTime(dt.Rows[0]["OUTDATE"].ToString().Trim()).ToString("yyyy년 MM월 dd일");
                    }
                    else
                    {
                        ssPrtNewBorn_Sheet1.Cells[4, 0].Text = dt.Rows[0]["PANO"].ToString().Trim();
                        ssPrtNewBorn_Sheet1.Cells[4, 2].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                        ssPrtNewBorn_Sheet1.Cells[4, 4].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[0]["JUMIN1"].ToString().Trim() + dt.Rows[0]["JUMIN2"].ToString().Trim());
                        ssPrtNewBorn_Sheet1.Cells[4, 6].Text = "   주민등록번호 : " + dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1) + "******";

                        ssPrtNewBorn_Sheet1.Cells[7, 3].Text = Convert.ToDateTime(dt.Rows[0]["OUTDATE"].ToString().Trim()).ToString("yyyy년 MM월 dd일");

                    }

                }

                dt.Dispose();
                dt = null;

                //퇴원약 여부
                ssPrtNewBorn_Sheet1.Cells[4, 10].Text = strDrug;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, TO_CHAR(RDATE, 'YYYY-MM-DD PM HH:MI') AS RDATE, TO_CHAR(RDATE,'YYYY-MM-DD') AS RDATE2, A.DEPTCODE, B.DRNAME, '<예약 예정일>' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_RESERVED A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.RDATE >=  TO_DATE('" + strOutDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "         AND A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT";
                //19-09-06 원무과에서 요청으로 <수납완료> 삭제함 (컴플레인 들어왔다고 함).
                SQL = SQL + ComNum.VBLF + "     A.PANO, TO_CHAR(RDATE, 'YYYY-MM-DD PM HH:MI') AS RDATE, TO_CHAR(RDATE,'YYYY-MM-DD') AS RDATE2, A.DEPTCODE, B.DRNAME, '' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "     (SELECT";
                SQL = SQL + ComNum.VBLF + "         PANO, DEPTCODE, DECODE(DATE2, DATE3, DATE2, DATE3) AS RDATE, DRCODE";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW";
                SQL = SQL + ComNum.VBLF + "         WHERE PANO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "             AND (DATE2 >= TO_DATE('" + strOutDate + "','YYYY-MM-DD') OR DATE2 >= TO_DATE('" + strOutDate + "','YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "     ) A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "MINUS ";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, TO_CHAR(RDATE, 'YYYY-MM-DD PM HH:MI') AS RDATE, TO_CHAR(RDATE,'YYYY-MM-DD') AS RDATE2, A.DEPTCODE, B.DRNAME, '<예약 예정일>' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_RESERVED A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.RDATE >= TO_DATE('" + strOutDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "         AND A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "ORDER BY RDATE ASC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i > 4) { break; }

                        strTemp = "";
                        strGubun = dt.Rows[i]["GUBUN"].ToString().Trim();
                        strTemp = (i + 1).ToString("0") + ") " + (strGubun != "" ? "(" + strGubun + ")" : "") + dt.Rows[i]["RDATE"].ToString().Trim() + "(" + VB.Left(clsVbfunc.GetYoIl(dt.Rows[i]["RDATE2"].ToString().Trim()), 1) + ")";
                        strTemp = strTemp + "    ◈ 진료과 : " + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[i]["DEPTCODE"].ToString().Trim());
                        strTemp = strTemp + "    ◈ 진료의사 :  " + dt.Rows[i]["DRNAME"].ToString().Trim();

                        ssPrtNewBorn_Sheet1.Cells[i + 9, 3].Text = strTemp;
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//dt2') DT2, extractValue(chartxml, '//it6') IT6,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//it5') IT5, extractValue(chartxml, '//it4') IT4,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//dt3') DT3, extractValue(chartxml, '//it7') IT7, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//it8') IT8, extractValue(chartxml, '//it9') IT9, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik1') IK1, extractValue(chartxml, '//ik2') IK2, extractValue(chartxml, '//it16') IT16, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//it10') IT10, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//it11') IT11, ";
                //수유
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik3') IK3, extractValue(chartxml, '//ik4') IK4, extractValue(chartxml, '//ik5') IK5, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//it14') IT14, extractValue(chartxml, '//it15') IT15, ";
                //목욕
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik6') IK6, extractValue(chartxml, '//ik7') IK7, extractValue(chartxml, '//ik8') IK8, ";
                //예방접종
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik9') IK9, extractValue(chartxml, '//ik10') IK10, extractValue(chartxml, '//ik11') IK11, extractValue(chartxml, '//it16') IT16, ";
                //실내환경
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik12') IK12, extractValue(chartxml, '//it18') IT18, extractValue(chartxml, '//ik13') IK13, extractValue(chartxml, '//it17') IT17, ";
                //대변양상
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik14') IK14, extractValue(chartxml, '//ik15') IK15, extractValue(chartxml, '//it19') IT19,  ";
                //기저귀 발진
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik16') IK16, extractValue(chartxml, '//ik17') IK17, extractValue(chartxml, '//ik18') IK18,  extractValue(chartxml, '//it20') IT20,";
                //퇴원 후 갈 곳
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik22') IK22, extractValue(chartxml, '//ik23') IK23, extractValue(chartxml, '//ik24') IK24, extractValue(chartxml, '//it20') IT21, ";
                //퇴원 시 제공
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik25') IK25, extractValue(chartxml, '//ik26') IK26, extractValue(chartxml, '//ik27') IK27, extractValue(chartxml, '//ik31') IK31, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik32') IK32, extractValue(chartxml, '//ik28') IK28, extractValue(chartxml, '//it22') It22, ";
                //교육대상
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik29') IK29, extractValue(chartxml, '//ik30') IK30, extractValue(chartxml, '//it23') It23 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "         AND CHARTDATE >= '" + strInDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "         AND CHARTDATE <= '" + strOutDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "         AND FORMNO = 1832";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    #region XML
                    if (i < 4)
                    {
                        if (dt.Rows[0]["DT2"].ToString().Trim() != "")
                        {
                            ssPrtNewBorn_Sheet1.Cells[i + 9, 3].Text = dt.Rows[0]["DT2"].ToString().Trim()
                                                                + " " + dt.Rows[0]["IT6"].ToString().Trim()
                                                                + "    ◈ 진료과 : " + dt.Rows[0]["IT5"].ToString().Trim()
                                                                + "    ◈ 진료의사 :  " + dt.Rows[0]["IT4"].ToString().Trim();
                            i++;
                        }
                    }

                    if (i < 4)
                    {
                        if (dt.Rows[0]["DT3"].ToString().Trim() != "")
                        {
                            ssPrtNewBorn_Sheet1.Cells[i + 9, 3].Text = dt.Rows[0]["DT3"].ToString().Trim()
                                                                + " " + dt.Rows[0]["IT7"].ToString().Trim()
                                                                + "    ◈ 진료과 : " + dt.Rows[0]["IT8"].ToString().Trim()
                                                                + "    ◈ 진료의사 :  " + dt.Rows[0]["IT9"].ToString().Trim();
                            i++;
                        }
                    }

                    ssPrtNewBorn_Sheet1.Cells[15, 5].Text = dt.Rows[0]["IK2"].ToString().Trim() == "true" ? "예" : "아니요";
                    ssPrtNewBorn_Sheet1.Cells[16, 5].Text = dt.Rows[0]["IT16"].ToString().Trim();
                    ssPrtNewBorn_Sheet1.Cells[18, 3].Text = dt.Rows[0]["IT11"].ToString().Trim();

                    strTemp = "";

                    if (dt.Rows[0]["IK3"].ToString().Trim() == "true") { strTemp += "모유, "; }
                    if (dt.Rows[0]["IK4"].ToString().Trim() == "true") { strTemp += "수유, "; }
                    if (dt.Rows[0]["IK5"].ToString().Trim() == "true") { strTemp += "혼합영양, "; }
                    if (dt.Rows[0]["IT14"].ToString().Trim() != "") { strTemp += "현재 1회 수유 : " + dt.Rows[0]["IT14"].ToString().Trim() + "ml, "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtNewBorn_Sheet1.Cells[20, 5].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK6"].ToString().Trim() == "true") { strTemp += "목욕방법, "; }
                    if (dt.Rows[0]["IK7"].ToString().Trim() == "true") { strTemp += "제대간호, "; }
                    if (dt.Rows[0]["IK8"].ToString().Trim() == "true") { strTemp += "피부간호, "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtNewBorn_Sheet1.Cells[21, 5].Text = strTemp;

                    strTemp = "B형간염 : ";

                    if (dt.Rows[0]["IK9"].ToString().Trim() == "true") { strTemp += "예, "; }
                    if (dt.Rows[0]["IK10"].ToString().Trim() == "true") { strTemp += "아니요, "; }
                    if (dt.Rows[0]["IK11"].ToString().Trim() == "true") { strTemp += "기타 : "; }
                    if (dt.Rows[0]["IT16"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT16"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtNewBorn_Sheet1.Cells[22, 5].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK12"].ToString().Trim() == "true") { strTemp += "적정온도 : "; }
                    if (dt.Rows[0]["IT18"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT18"].ToString().Trim() + ", "; }
                    if (dt.Rows[0]["IK13"].ToString().Trim() == "true") { strTemp += "적정습도 : "; }
                    if (dt.Rows[0]["IT17"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT17"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtNewBorn_Sheet1.Cells[23, 5].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK14"].ToString().Trim() == "true") { strTemp += "양호, "; }
                    if (dt.Rows[0]["IK15"].ToString().Trim() == "true") { strTemp += "기타 : "; }
                    if (dt.Rows[0]["IT19"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT19"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtNewBorn_Sheet1.Cells[24, 5].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK16"].ToString().Trim() == "true") { strTemp += "무, "; }
                    if (dt.Rows[0]["IK17"].ToString().Trim() == "true") { strTemp += "유, "; }
                    if (dt.Rows[0]["IK18"].ToString().Trim() == "true") { strTemp += "기타 : "; }
                    if (dt.Rows[0]["IT20"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT20"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtNewBorn_Sheet1.Cells[25, 5].Text = strTemp;

                    ssPrtNewBorn_Sheet1.Cells[27, 5].Text = "";

                    strTemp = "";

                    if (dt.Rows[0]["IK22"].ToString().Trim() == "true") { strTemp += "자가, "; }
                    if (dt.Rows[0]["IK23"].ToString().Trim() == "true") { strTemp += "타병원, "; }
                    if (dt.Rows[0]["IK24"].ToString().Trim() == "true") { strTemp += "기타 : "; }
                    if (dt.Rows[0]["IT21"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT21"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtNewBorn_Sheet1.Cells[30, 5].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK25"].ToString().Trim() == "true") { strTemp += "CD, "; }
                    if (dt.Rows[0]["IK26"].ToString().Trim() == "true") { strTemp += "소견서, "; }
                    if (dt.Rows[0]["IK27"].ToString().Trim() == "true") { strTemp += "진료의뢰서, "; }
                    if (dt.Rows[0]["IK31"].ToString().Trim() == "true") { strTemp += "진단서, "; }
                    if (dt.Rows[0]["IK32"].ToString().Trim() == "true") { strTemp += "입원사실증명서, "; }
                    if (dt.Rows[0]["IK28"].ToString().Trim() == "true") { strTemp += "기타 : "; }
                    if (dt.Rows[0]["IT22"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT22"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtNewBorn_Sheet1.Cells[32, 3].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK29"].ToString().Trim() == "true") { strTemp += "어머니, "; }
                    if (dt.Rows[0]["IK30"].ToString().Trim() == "true") { strTemp += "보호자 : "; }
                    if (dt.Rows[0]["IT23"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT23"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrtNewBorn_Sheet1.Cells[34, 3].Text = strTemp;

                    ssPrtNewBorn_Sheet1.Cells[36, 3].Text = "054-260-8563";

                    ssPrtNewBorn_Sheet1.Cells[42, 0].Text = "출력일시 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");
                    ssPrtNewBorn_Sheet1.Cells[42, 9].Text = "출력자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);

                    ssPrtNewBorn_Sheet1.Cells[38, 3].Text = READ_TEMP_JEPSU(strPtno);
                    #endregion
                }
                else
                {
                    #region 신규
                    dt.Dispose();

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + " ITEMNO, ITEMCD, ITEMVALUE";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                    SQL = SQL + ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW B";
                    SQL = SQL + ComNum.VBLF + "     ON A.EMRNO    = B.EMRNO";
                    SQL = SQL + ComNum.VBLF + "    AND A.EMRNOHIS = B.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "    AND B.ITEMCD > CHR(0)";
                    SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + strPtno + "'";
                    SQL = SQL + ComNum.VBLF + "  AND CHARTDATE >= '" + strInDate.Replace("-", "") + "'";
                    SQL = SQL + ComNum.VBLF + "  AND CHARTDATE <= '" + strOutDate.Replace("-", "") + "'";
                    SQL = SQL + ComNum.VBLF + "  AND FORMNO = 1832";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    bool[] bInfo = new bool[4];

                    if (dt.Rows.Count > 0)
                    {
                        ssPrtNewBorn2_Sheet1.Cells[11, 3, 15, 3].Text = "";

                        DataTable CopyTable = null;
                        var tmp = dt.AsEnumerable().Where(d => d.Field<string>("ITEMNO").Equals("I0000035170") && d.Field<string>("ITEMCD").IndexOf("_1") == -1 && d.Field<string>("ITEMVALUE") != null).OrderBy(d => d.Field<string>("ITEMCD"));
                        if (tmp.Any())
                        {
                            CopyTable = tmp.CopyToDataTable();

                            for (int j = 0; j < CopyTable.Rows.Count; j++)
                            {
                                ssPrtNewBorn2_Sheet1.Cells[11 + j, 3].Text = (j + 1) + ") " + CopyTable.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                        }

                        if (CopyTable != null)
                        {
                            CopyTable.Dispose();
                        }

                        ssPrtNewBorn2_Sheet1.Cells[9, 3].Text = "";
                        ssPrtNewBorn2_Sheet1.Cells[17, 5, 18, 5].Text = "";
                        ssPrtNewBorn2_Sheet1.Cells[20, 3].Text = "";
                        ssPrtNewBorn2_Sheet1.Cells[22, 5, 27, 5].Text = "";
                        ssPrtNewBorn2_Sheet1.Cells[29, 3, 44, 3].Text = "";
                        ssPrtNewBorn2_Sheet1.Cells[46, 3].Text = "";
                        ssPrtNewBorn2_Sheet1.Cells[27, 5].Text = "B형 간염 : ";


                        for (int j = 0; j < dt.Rows.Count; j++)
                        {   
                            

                            //설명
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035339"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[15, 5].Text = dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "예" : "아니요";
                            }

                            //복약안내문 제공
                            //if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035180"))
                            //{
                            //    ssPrtNewBorn2_Sheet1.Cells[16, 5].Text = dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "예" : "아니요";
                            //}

                            #region 퇴원형태                            
                            if (string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035172") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[9, 3].Text = "퇴원지시후";
                            }
                            else if (string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035330") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[9, 3].Text = "지시에 따르지 못하는 퇴원(자의퇴원)";
                            }

                            else if (string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035173_1") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[9, 3].Text = "전원: ";
                            }

                            else if (!string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035173_2") &&
                                !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[9, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString();
                            }

                            else if (string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035175_1") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[9, 3].Text = "사망: ";
                            }

                            else if (!string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000007586") &&
                                !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[9, 3].Text += "사망일자 : " + dt.Rows[j]["ITEMVALUE"].ToString() + " ";
                            }

                            else if (!string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000037849") &&
                                !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[9, 3].Text += "사망시간 : " + dt.Rows[j]["ITEMVALUE"].ToString() + " ";
                            }
                            else if (!string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000024438") &&
                                !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[9, 3].Text += "사망장소 : " + dt.Rows[j]["ITEMVALUE"].ToString();
                            }

                            else if (string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035176_1") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[9, 3].Text = "기타: ";
                            }

                            else if (!string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035176_2") &&
                                !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[9, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString();
                            }
                            #endregion

                            #region 퇴원약 복약 안내

                            //퇴원약 제공
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035179") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                bInfo[0] = true;
                            }

                            //복약안내문 제공
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035180") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                bInfo[1] = true;
                            }

                            //복약설명
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035339") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                bInfo[2] = true;
                            }

                            //퇴원약 없음
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035338") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                bInfo[3] = true;
                            }
                            #endregion

                            //환자와의 관계
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000013388"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[18, 5].Text = dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }

                            //퇴원 후 주의사항(문의 요하는 증
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000033457"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[20, 3].Text = dt.Rows[j]["ITEMVALUE"].ToString().Trim();

                                if (ssPrtNewBorn2_Sheet1.Rows[20].GetPreferredHeight() > 80)
                                {
                                    ssPrtNewBorn2_Sheet1.Rows[20].Height = ssPrtNewBorn2_Sheet1.Rows[20].GetPreferredHeight() + 15;
                                }
                                else
                                {
                                    ssPrtNewBorn2_Sheet1.Rows[20].Height = 80;
                                }
                            }

                            #region 퇴원후 관리 - 수유
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035340") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[22, 5].Text += "모유, ";
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035341") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[22, 5].Text += "인공수유, ";
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035342") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[22, 5].Text += "혼합영양, ";
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035343") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[22, 5].Text += " 수유간격 : " + dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035344") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[22, 5].Text += " 현재 1회 수유 : " + dt.Rows[j]["ITEMVALUE"].ToString().Trim() + "ml";
                            }
                            #endregion

                            #region 퇴원후 관리 - 목 욕
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000033836") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[23, 5].Text += "목욕방법, " ;
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000033837") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[23, 5].Text += "제대간호, ";
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000033838") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[23, 5].Text += "피부간호, ";
                            }
                            #endregion

                            #region 퇴원후 관리 - 실내환경
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035345_1") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[24, 5].Text += "적정온도 : ";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035345_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[24, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035346_1") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[24, 5].Text += "적정습도 : ";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035346_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[24, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            #endregion

                            #region 대변양상
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035347") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[25, 5].Text += "양호, ";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035348_1") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[25, 5].Text += "기타, ";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035348_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[25, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            #endregion

                            #region 기저귀 발진
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035349") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[26, 5].Text += "무, ";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035350") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[26, 5].Text += "유, ";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035351_1") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[26, 5].Text += "기타 : ";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035351_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[26, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            #endregion

                            #region 예방 접종
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035352") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[27, 5].Text += "예, ";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035353") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[27, 5].Text += "아니오, ";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035354_1") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[27, 5].Text += "기타 : ";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035354_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[27, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            #endregion

                            #region 선천성 대사이상 검사
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000034411_1") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[29, 3].Text += "미시행";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000034411_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[29, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000034410") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[29, 3].Text += "시행 - " ;
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035356") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[29, 3].Text += "본원" ;
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035357") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[29, 3].Text += "타원 : " ;
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035355") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[29, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            #endregion

                            #region 신생아 청력검사
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000034406") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[31, 3].Text += "미시행" ;
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000034407") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[31, 3].Text += "시행 - ";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000034408") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[31, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000034409") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[31, 3].Text += " Rt: " + dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035358") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[31, 3].Text += " Lt: " + dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            #endregion

                            #region 퇴원시 상태 - 거주장소 갈곳 
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035220") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[34, 3].Text += "자가, ";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035221") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[34, 3].Text += "타병원, ";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035224_1") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[34, 3].Text += "기타 : ";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035224_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[34, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035223") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[34, 3].Text += "영안실안치 ";
                            }
                            #endregion

                            #region 추후관리
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035207") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[36, 3].Text += "외래, ";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035211_1") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[36, 3].Text += "타병원 : ";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035211_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[36, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            #endregion

                            #region 퇴원시 상태
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035331") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[38, 3].Text += "부모님과 함께, ";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035211_1") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[38, 3].Text += "구급차, ";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035219_1") && dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[38, 3].Text += "기타 : ";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035219_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[38, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            #endregion

                            #region 퇴원시 진료정보 제공
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035225"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[40, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "진료의뢰서, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035226"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[40, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "검사결과지, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035227"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[40, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "CD, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035228"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[40, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "소견서, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035229"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[40, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "입퇴원확인서, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035230"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[40, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "진단서, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035231"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[40, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "미해당, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035232_1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[40, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "기타 : " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035232_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[40, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            #endregion

                            #region ◈ 퇴원교육대상
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035234_1"))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[42, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "보호자 : " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035234_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrtNewBorn2_Sheet1.Cells[42, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            #endregion

                            #endregion
                        }

                        if (VB.Right(ssPrtNewBorn2_Sheet1.Cells[22, 5].Text, 2) == ", ")
                        {
                            ssPrtNewBorn2_Sheet1.Cells[22, 5].Text = VB.Left(ssPrtNewBorn2_Sheet1.Cells[22, 5].Text, ssPrtNewBorn2_Sheet1.Cells[22, 5].Text.Length - 2);
                        }

                        if (VB.Right(ssPrtNewBorn2_Sheet1.Cells[23, 5].Text, 2) == ", ")
                        {
                            ssPrtNewBorn2_Sheet1.Cells[23, 5].Text = VB.Left(ssPrtNewBorn2_Sheet1.Cells[23, 5].Text, ssPrtNewBorn2_Sheet1.Cells[23, 5].Text.Length - 2);
                        }

                        if (VB.Right(ssPrtNewBorn2_Sheet1.Cells[24, 5].Text, 2) == ", ")
                        {
                            ssPrtNewBorn2_Sheet1.Cells[24, 5].Text = VB.Left(ssPrtNewBorn2_Sheet1.Cells[24, 5].Text, ssPrtNewBorn2_Sheet1.Cells[24, 5].Text.Length - 2);
                        }

                        if (VB.Right(ssPrtNewBorn2_Sheet1.Cells[25, 5].Text, 2) == ", ")
                        {
                            ssPrtNewBorn2_Sheet1.Cells[25, 5].Text = VB.Left(ssPrtNewBorn2_Sheet1.Cells[25, 5].Text, ssPrtNewBorn2_Sheet1.Cells[25, 5].Text.Length - 2);
                        }


                        if (VB.Right(ssPrtNewBorn2_Sheet1.Cells[26, 5].Text, 2) == ", ")
                        {
                            ssPrtNewBorn2_Sheet1.Cells[26, 5].Text = VB.Left(ssPrtNewBorn2_Sheet1.Cells[26, 5].Text, ssPrtNewBorn2_Sheet1.Cells[26, 5].Text.Length - 2);
                        }


                        if (VB.Right(ssPrtNewBorn2_Sheet1.Cells[27, 5].Text, 2) == ", ")
                        {
                            ssPrtNewBorn2_Sheet1.Cells[27, 5].Text = VB.Left(ssPrtNewBorn2_Sheet1.Cells[27, 5].Text, ssPrtNewBorn2_Sheet1.Cells[27, 5].Text.Length - 2);
                        }

                        if (VB.Right(ssPrtNewBorn2_Sheet1.Cells[29, 5].Text, 2) == ", ")
                        {
                            ssPrtNewBorn2_Sheet1.Cells[29, 5].Text = VB.Left(ssPrtNewBorn2_Sheet1.Cells[29, 5].Text, ssPrtNewBorn2_Sheet1.Cells[29, 5].Text.Length - 2);
                        }


                        if (VB.Right(ssPrtNewBorn2_Sheet1.Cells[31, 3].Text, 2) == ", ")
                        {
                            ssPrtNewBorn2_Sheet1.Cells[31, 3].Text = VB.Left(ssPrtNewBorn2_Sheet1.Cells[31, 3].Text, ssPrtNewBorn2_Sheet1.Cells[31, 3].Text.Length - 2);
                        }

                        if (VB.Right(ssPrtNewBorn2_Sheet1.Cells[34, 3].Text, 2) == ", ")
                        {
                            ssPrtNewBorn2_Sheet1.Cells[34, 3].Text = VB.Left(ssPrtNewBorn2_Sheet1.Cells[34, 3].Text, ssPrtNewBorn2_Sheet1.Cells[34, 3].Text.Length - 2);
                        }


                        if (VB.Right(ssPrtNewBorn2_Sheet1.Cells[36, 3].Text, 2) == ", ")
                        {
                            ssPrtNewBorn2_Sheet1.Cells[36, 3].Text = VB.Left(ssPrtNewBorn2_Sheet1.Cells[36, 3].Text, ssPrtNewBorn2_Sheet1.Cells[36, 3].Text.Length - 2);
                        }

                        if (VB.Right(ssPrtNewBorn2_Sheet1.Cells[38, 3].Text, 2) == ", ")
                        {
                            ssPrtNewBorn2_Sheet1.Cells[38, 3].Text = VB.Left(ssPrtNewBorn2_Sheet1.Cells[38, 3].Text, ssPrtNewBorn2_Sheet1.Cells[38, 3].Text.Length - 2);
                        }

                        ssPrtNewBorn2_Sheet1.Cells[44, 3].Text = "054-260-8563";

                        ssPrtNewBorn2_Sheet1.Cells[50, 0].Text = "출력일시 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");
                        ssPrtNewBorn2_Sheet1.Cells[50, 9].Text = "출력자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);

                        ssPrtNewBorn2_Sheet1.Cells[46, 3].Text = READ_TEMP_JEPSU(strPtno);

                        if (bInfo[0] == false)
                        {
                            ssPrtNewBorn2_Sheet1.Cells[17, 5].Text = "□ 퇴원약 제공";
                        }
                        else
                        {
                            ssPrtNewBorn2_Sheet1.Cells[17, 5].Text = "■ 퇴원약 제공";
                        }

                        if (bInfo[1] == false)
                        {
                            ssPrtNewBorn2_Sheet1.Cells[17, 5].Text += "  □ 복약안내문 제공";
                        }
                        else
                        {
                            ssPrtNewBorn2_Sheet1.Cells[17, 5].Text += "  ■ 복약안내문 제공";
                        }

                        if (bInfo[2] == false)
                        {
                            ssPrtNewBorn2_Sheet1.Cells[17, 5].Text += "  □ 복약설명";
                        }
                        else
                        {
                            ssPrtNewBorn2_Sheet1.Cells[17, 5].Text += "  ■ 복약설명";
                        }

                        if (bInfo[3] == false)
                        {
                            ssPrtNewBorn2_Sheet1.Cells[17, 5].Text += "  □ 퇴원약 없음";
                        }
                        else
                        {
                            ssPrtNewBorn2_Sheet1.Cells[17, 5].Text += "  ■ 퇴원약 없음";
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private void CLEAR_SSPRT()
        {
            ssPrt_Sheet1.Cells[7, 3].Text = "";
            ssPrt_Sheet1.Cells[9, 3].Text = "";
            ssPrt_Sheet1.Cells[10, 3].Text = "";
            ssPrt_Sheet1.Cells[11, 3].Text = "";
            ssPrt_Sheet1.Cells[12, 3].Text = "";
            ssPrt_Sheet1.Cells[13, 3].Text = "";

            ssPrt_Sheet1.Cells[15, 5].Text = "";
            ssPrt_Sheet1.Cells[16, 5].Text = "";
            ssPrt_Sheet1.Cells[17, 5].Text = "";

            ssPrt_Sheet1.Cells[19, 3].Text = "";

            ssPrt_Sheet1.Cells[21, 5].Text = "";
            ssPrt_Sheet1.Cells[22, 5].Text = "";
            ssPrt_Sheet1.Cells[23, 5].Text = "";
            ssPrt_Sheet1.Cells[24, 5].Text = "";

            ssPrt_Sheet1.Cells[26, 3].Text = "";

            ssPrt_Sheet1.Cells[29, 5].Text = "";
            ssPrt_Sheet1.Cells[30, 5].Text = "";
            ssPrt_Sheet1.Cells[31, 5].Text = "";

            ssPrt_Sheet1.Cells[33, 3].Text = "";
            ssPrt_Sheet1.Cells[35, 3].Text = "";
            ssPrt_Sheet1.Cells[37, 3].Text = "";
            ssPrt_Sheet1.Cells[39, 3].Text = "";

            ssPrt2_Sheet1.Cells[9, 3].Text = "";
        }

        private void READ_퇴원간호_NEW(string strPtno, string strInDate, string strOutDate, string strDrug = "")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strTemp = "";
            string strGubun = "";

            CLEAR_SSPRT();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.SEX, A.JUMIN1, A.JUMIN2, B.OUTDATE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPtno + "' ";
                SQL = SQL + ComNum.VBLF + "         AND B.OUTDATE = TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    if (DateTime.Now.Date >= Convert.ToDateTime("2020-04-22"))
                    {
                        ssPrt2_Sheet1.Cells[4, 0].Text = dt.Rows[0]["PANO"].ToString().Trim();
                        ssPrt2_Sheet1.Cells[4, 2].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                        ssPrt2_Sheet1.Cells[4, 4].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[0]["JUMIN1"].ToString().Trim() + dt.Rows[0]["JUMIN2"].ToString().Trim());
                        ssPrt2_Sheet1.Cells[4, 6].Text = "   주민등록번호 : " + dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1) + "******";
                        ssPrt2_Sheet1.Cells[7, 3].Text = Convert.ToDateTime(dt.Rows[0]["OUTDATE"].ToString().Trim()).ToString("yyyy년 MM월 dd일");

                    }
                    else
                    {
                        ssPrt_Sheet1.Cells[4, 0].Text = dt.Rows[0]["PANO"].ToString().Trim();
                        ssPrt_Sheet1.Cells[4, 2].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                        ssPrt_Sheet1.Cells[4, 4].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[0]["JUMIN1"].ToString().Trim() + dt.Rows[0]["JUMIN2"].ToString().Trim());
                        ssPrt_Sheet1.Cells[4, 6].Text = "   주민등록번호 : " + dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1) + "******";

                        ssPrt_Sheet1.Cells[7, 3].Text = Convert.ToDateTime(dt.Rows[0]["OUTDATE"].ToString().Trim()).ToString("yyyy년 MM월 dd일");

                    }
                }

                dt.Dispose();
                dt = null;

                ssPrt2_Sheet1.Cells[9, 3].Text = "";
                ssPrt2_Sheet1.Cells[4, 5, 35, 5].Text = "";
                ssPrt2_Sheet1.Cells[21, 3].Text = "";
                ssPrt2_Sheet1.Cells[35, 3].Text = "";
                ssPrt2_Sheet1.Cells[37, 3].Text = "";
                ssPrt2_Sheet1.Cells[39, 3].Text = "";

                //퇴원약 여부
                ssPrt_Sheet1.Cells[4, 10].Text = strDrug;
                ssPrt2_Sheet1.Cells[4, 10].Text = strDrug;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, TO_CHAR(RDATE, 'YYYY-MM-DD PM HH:MI') AS RDATE, TO_CHAR(RDATE,'YYYY-MM-DD') AS RDATE2, A.DEPTCODE, B.DRNAME, '<예약 예정일>' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_RESERVED A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.RDATE >=  TO_DATE('" + strOutDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "         AND A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT";
                //19-09-06 원무과에서 요청으로 <수납완료> 삭제함 (컴플레인 들어왔다고 함).
                SQL = SQL + ComNum.VBLF + "     A.PANO, TO_CHAR(RDATE, 'YYYY-MM-DD PM HH:MI') AS RDATE, TO_CHAR(RDATE,'YYYY-MM-DD') AS RDATE2, A.DEPTCODE, B.DRNAME, '' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM";
                SQL = SQL + ComNum.VBLF + "     (SELECT";
                SQL = SQL + ComNum.VBLF + "         PANO, DEPTCODE , DECODE(DATE2, DATE3, DATE2, DATE3) AS RDATE, DRCODE";
                SQL = SQL + ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW";
                SQL = SQL + ComNum.VBLF + "         WHERE PANO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "             AND (DATE2 >= TO_DATE('" + strOutDate + "','YYYY-MM-DD') OR DATE2 >= TO_DATE('" + strOutDate + "','YYYY-MM-DD'))";
                SQL = SQL + ComNum.VBLF + "     ) A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "MINUS ";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.PANO, TO_CHAR(RDATE, 'YYYY-MM-DD PM HH:MI') AS RDATE, TO_CHAR(RDATE,'YYYY-MM-DD') AS RDATE2, A.DEPTCODE, B.DRNAME, '<예약 예정일>' AS GUBUN";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_RESERVED A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.RDATE >=  TO_DATE('" + strOutDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "         AND A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "ORDER BY RDATE ASC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i > 4) { break; }

                        strTemp = "";
                        strGubun = dt.Rows[i]["GUBUN"].ToString().Trim();
                        strTemp = (i + 1).ToString("0") + ") " + (strGubun != "" ? "(" + strGubun + ")" : "") + dt.Rows[i]["RDATE"].ToString().Trim() + "(" + VB.Left(clsVbfunc.GetYoIl(dt.Rows[i]["RDATE2"].ToString().Trim()), 1) + ")";
                        strTemp = strTemp + "    ◈ 진료과 : " + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[i]["DEPTCODE"].ToString().Trim());
                        strTemp = strTemp + "    ◈ 진료의사 :  " + dt.Rows[i]["DRNAME"].ToString().Trim();

                        ssPrt_Sheet1.Cells[i + 11, 3].Text = strTemp;
                        ssPrt2_Sheet1.Cells[i + 11, 3].Text = strTemp;
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//dt2') DT2, extractValue(chartxml, '//it6') IT6,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//it5') IT5, extractValue(chartxml, '//it4') IT4,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//dt3') DT3, extractValue(chartxml, '//it7') IT7, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//it8') IT8, extractValue(chartxml, '//it9') IT9, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik1') IK1, extractValue(chartxml, '//ik2') IK2, extractValue(chartxml, '//it16') IT16, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//it10') IT10, extractValue(chartxml, '//ta1') TA1, extractValue(chartxml, '//ik147') IK147,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik3') IK3, extractValue(chartxml, '//ik4') IK4, extractValue(chartxml, '//ik5') IK5, extractValue(chartxml, '//ik6') IK6,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik7') IK7, extractValue(chartxml, '//ik8') IK8, extractValue(chartxml, '//it11') IT11, extractValue(chartxml, '//ik9') IK9,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik10') IK10, extractValue(chartxml, '//ik11') IK11, extractValue(chartxml, '//ik12') IK12,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik135') IK135, extractValue(chartxml, '//ik136') IK136, extractValue(chartxml, '//ik13') IK13,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik14') IK14, extractValue(chartxml, '//ik15') IK15, extractValue(chartxml, '//ik16') IK16, extractValue(chartxml, '//ik17') IK17,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik134') IK134, extractValue(chartxml, '//ik18') IK18, extractValue(chartxml, '//it12') IT12, extractValue(chartxml, '//it14') IT14,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik19') IK19, extractValue(chartxml, '//ik20') IK20, extractValue(chartxml, '//ik21') IK21,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik22') IK22, extractValue(chartxml, '//ik23') IK23, extractValue(chartxml, '//ik24') IK24,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik25') IK25, extractValue(chartxml, '//ik26') IK26, extractValue(chartxml, '//ik27') IK27,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik145') IK145, extractValue(chartxml, '//ik28') IK28, extractValue(chartxml, '//it13') IT13, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik140') IK140, extractValue(chartxml, '//ik141') IK141, extractValue(chartxml, '//ik142') IK142, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik143') IK143, extractValue(chartxml, '//ik144') IK144, extractValue(chartxml, '//dt4') DT4,  extractValue(chartxml, '//it19') IT19, ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik29') IK29, extractValue(chartxml, '//ik30') IK30,  extractValue(chartxml, '//ik31') IK31,  extractValue(chartxml, '//ik32') IK32,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik33') IK33, extractValue(chartxml, '//ik34') IK34,  extractValue(chartxml, '//ik35') IK35,  extractValue(chartxml, '//ik36') IK36,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik37') IK37, extractValue(chartxml, '//ik38') IK38,  extractValue(chartxml, '//ik39') IK39,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik131') IK131, extractValue(chartxml, '//ik132') IK132,  extractValue(chartxml, '//ik133') IK133,  extractValue(chartxml, '//it15') It15,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik40') IK40, extractValue(chartxml, '//ik41') IK41,  ";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//ik137') IK137, extractValue(chartxml, '//ik138') IK138,  extractValue(chartxml, '//ik139') IK139,";
                SQL = SQL + ComNum.VBLF + "     extractValue(chartxml, '//it17') IT17, extractValue(chartxml, '//it18') IT18, extractValue(chartxml, '//it20') IT20, extractValue(chartxml, '//it21') IT21 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + strPtno + "'";
                SQL = SQL + ComNum.VBLF + "         AND CHARTDATE >= '" + strInDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "         AND CHARTDATE <= '" + strOutDate.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "         AND FORMNO = 966";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    #region 이전 서식지 연동
                    if (i < 4)
                    {
                        if (dt.Rows[0]["DT2"].ToString().Trim() != "")
                        {
                            ssPrt_Sheet1.Cells[i + 9, 3].Text = dt.Rows[0]["DT2"].ToString().Trim()
                                                                + " " + dt.Rows[0]["IT6"].ToString().Trim()
                                                                + "    ◈ 진료과 : " + dt.Rows[0]["IT5"].ToString().Trim()
                                                                + "    ◈ 진료의사 :  " + dt.Rows[0]["IT4"].ToString().Trim();
                            i++;
                        }
                    }

                    if (i < 4)
                    {
                        if (dt.Rows[0]["DT3"].ToString().Trim() != "")
                        {
                            ssPrt_Sheet1.Cells[i + 9, 3].Text = dt.Rows[0]["DT3"].ToString().Trim()
                                                                + " " + dt.Rows[0]["IT7"].ToString().Trim()
                                                                + "    ◈ 진료과 : " + dt.Rows[0]["IT8"].ToString().Trim()
                                                                + "    ◈ 진료의사 :  " + dt.Rows[0]["IT9"].ToString().Trim();
                            i++;
                        }
                    }

                    ssPrt_Sheet1.Cells[15, 5].Text = dt.Rows[0]["IK1"].ToString().Trim() == "true" ? "예" : "아니요";
                    ssPrt_Sheet1.Cells[16, 5].Text = dt.Rows[0]["IK2"].ToString().Trim() == "true" ? "예" : "아니요";
                    ssPrt_Sheet1.Cells[17, 5].Text = dt.Rows[0]["IT16"].ToString().Trim();
                    ssPrt_Sheet1.Cells[19, 3].Text = dt.Rows[0]["IT10"].ToString().Trim()
                                                        + ComNum.VBLF + dt.Rows[0]["TA1"].ToString().Trim();

                    strTemp = "";

                    if (dt.Rows[0]["IK3"].ToString().Trim() == "true") { strTemp += "일상생활, "; }
                    if (dt.Rows[0]["IK4"].ToString().Trim() == "true") { strTemp += "안정, "; }
                    if (dt.Rows[0]["IK5"].ToString().Trim() == "true") { strTemp += "정기적인 운동, "; }
                    if (dt.Rows[0]["IK6"].ToString().Trim() == "true") { strTemp += "재활치료, "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrt_Sheet1.Cells[21, 5].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK7"].ToString().Trim() == "true") { strTemp += "일반식, "; }
                    if (dt.Rows[0]["IK8"].ToString().Trim() == "true") { strTemp += "치료식 : "; }
                    if (dt.Rows[0]["IT11"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT11"].ToString().Trim() + ", "; }
                    if (dt.Rows[0]["IK9"].ToString().Trim() == "true") { strTemp += "기타 : "; }
                    if (dt.Rows[0]["IT20"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT20"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrt_Sheet1.Cells[22, 5].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK10"].ToString().Trim() == "true") { strTemp += "통목욕, "; }
                    if (dt.Rows[0]["IK11"].ToString().Trim() == "true") { strTemp += "샤워, "; }
                    if (dt.Rows[0]["IK12"].ToString().Trim() == "true") { strTemp += "침상목욕, "; }
                    if (dt.Rows[0]["IK135"].ToString().Trim() == "true") { strTemp += "목욕금지, "; }
                    if (dt.Rows[0]["IK136"].ToString().Trim() == "true") { strTemp += "부분목욕, "; }
                    if (dt.Rows[0]["IK13"].ToString().Trim() == "true") { strTemp += "기타, "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrt_Sheet1.Cells[23, 5].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK14"].ToString().Trim() == "true") { strTemp += "외래, "; }
                    if (dt.Rows[0]["IK15"].ToString().Trim() == "true") { strTemp += "가정간호, "; }
                    if (dt.Rows[0]["IK16"].ToString().Trim() == "true") { strTemp += "호스피스, "; }
                    if (dt.Rows[0]["IK17"].ToString().Trim() == "true") { strTemp += "재입원, "; }
                    if (dt.Rows[0]["IK134"].ToString().Trim() == "true") { strTemp += "타병원, "; }
                    if (dt.Rows[0]["IK18"].ToString().Trim() == "true") { strTemp += "기타 : "; }
                    if (dt.Rows[0]["IT12"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT12"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrt_Sheet1.Cells[24, 5].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK19"].ToString().Trim() == "true") { strTemp += "감염예방, "; }
                    if (dt.Rows[0]["IK20"].ToString().Trim() == "true") { strTemp += "체중측정, "; }
                    if (dt.Rows[0]["IK21"].ToString().Trim() == "true") { strTemp += "당뇨조절, "; }
                    if (dt.Rows[0]["IK22"].ToString().Trim() == "true") { strTemp += "구강간호, "; }
                    if (dt.Rows[0]["IK23"].ToString().Trim() == "true") { strTemp += "발간호, "; }
                    if (dt.Rows[0]["IK24"].ToString().Trim() == "true") { strTemp += "튜브관리, "; }
                    if (dt.Rows[0]["IK25"].ToString().Trim() == "true") { strTemp += "혈압조절, "; }
                    if (dt.Rows[0]["IK26"].ToString().Trim() == "true") { strTemp += "좌욕, "; }
                    if (dt.Rows[0]["IK27"].ToString().Trim() == "true") { strTemp += "체위변경, "; }
                    if (dt.Rows[0]["IK145"].ToString().Trim() == "true") { strTemp += "상처간호, "; }
                    if (dt.Rows[0]["IK28"].ToString().Trim() == "true") { strTemp += "기타 : "; }
                    if (dt.Rows[0]["IT13"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT13"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrt_Sheet1.Cells[26, 3].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK140"].ToString().Trim() == "true") { strTemp += "퇴원 지시 후, "; }
                    if (dt.Rows[0]["IK141"].ToString().Trim() == "true") { strTemp += "자의퇴원, "; }
                    if (dt.Rows[0]["IK142"].ToString().Trim() == "true") { strTemp += "전원, "; }
                    if (dt.Rows[0]["IK143"].ToString().Trim() == "true") { strTemp += "탈원, "; }
                    if (dt.Rows[0]["IK144"].ToString().Trim() == "true")
                    { strTemp += "사망 : " + dt.Rows[0]["DT4"].ToString().Trim() + " " + dt.Rows[0]["IT13"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrt_Sheet1.Cells[28, 5].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK29"].ToString().Trim() == "true") { strTemp += "응급차, "; }
                    if (dt.Rows[0]["IK30"].ToString().Trim() == "true") { strTemp += "눕는차, "; }
                    if (dt.Rows[0]["IK31"].ToString().Trim() == "true") { strTemp += "휠체어, "; }
                    if (dt.Rows[0]["IK32"].ToString().Trim() == "true") { strTemp += "도보, "; }
                    if (dt.Rows[0]["IT21"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT21"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrt_Sheet1.Cells[29, 5].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK33"].ToString().Trim() == "true") { strTemp += "명료, "; }
                    if (dt.Rows[0]["IK34"].ToString().Trim() == "true") { strTemp += "혼돈, "; }
                    if (dt.Rows[0]["IK35"].ToString().Trim() == "true") { strTemp += "반의식, "; }
                    if (dt.Rows[0]["IK36"].ToString().Trim() == "true") { strTemp += "무의식, "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrt_Sheet1.Cells[30, 5].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK37"].ToString().Trim() == "true") { strTemp += "자가, "; }
                    if (dt.Rows[0]["IK38"].ToString().Trim() == "true") { strTemp += "타병원, "; }
                    if (dt.Rows[0]["IK39"].ToString().Trim() == "true") { strTemp += "기타 : "; }
                    if (dt.Rows[0]["IT14"].ToString().Trim() != "") { strTemp += dt.Rows[0]["IT14"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrt_Sheet1.Cells[31, 5].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK131"].ToString().Trim() == "true") { strTemp += "CD, "; }
                    if (dt.Rows[0]["IK132"].ToString().Trim() == "true") { strTemp += "진료의뢰서, "; }
                    if (dt.Rows[0]["IK133"].ToString().Trim() == "true") { strTemp += "소견서, "; }
                    if (dt.Rows[0]["IK147"].ToString().Trim() == "true") { strTemp += "미해당, "; }
                    if (dt.Rows[0]["IT15"].ToString().Trim() != "") { strTemp += "기타 : " + dt.Rows[0]["IT15"].ToString().Trim() + ", "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrt_Sheet1.Cells[33, 3].Text = strTemp;

                    strTemp = "";

                    if (dt.Rows[0]["IK40"].ToString().Trim() == "true") { strTemp += "환자, "; }
                    if (dt.Rows[0]["IK41"].ToString().Trim() == "true") { strTemp += "보호자, "; }
                    if (strTemp != "") { strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2); }

                    ssPrt_Sheet1.Cells[35, 3].Text = strTemp;

                    ssPrt_Sheet1.Cells[37, 3].Text = dt.Rows[0]["IT17"].ToString().Trim() + " " + dt.Rows[0]["IT18"].ToString().Trim();

                    ssPrt_Sheet1.Cells[43, 0].Text = "출력일시 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");
                    ssPrt_Sheet1.Cells[43, 9].Text = "출력자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);

                    ssPrt_Sheet1.Cells[39, 3].Text = READ_TEMP_JEPSU(strPtno);
                    #endregion
                }
                else
                {
                    #region 신규 기록지 연동
                    dt.Dispose();

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + " ITEMNO, ITEMCD, ITEMVALUE";
                    SQL = SQL + ComNum.VBLF + " , ((SELECT ITEMVALUE";
                    SQL = SQL + ComNum.VBLF + "      FROM ADMIN.AEMRCHARTROW";
                    SQL = SQL + ComNum.VBLF + "      WHERE EMRNO = A.EMRNO";
                    SQL = SQL + ComNum.VBLF + "        AND EMRNOHIS = A.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "        AND ITEMNO = 'I0000030686') || '병동 ' ||";
                    SQL = SQL + ComNum.VBLF + "      (SELECT ITEMVALUE";
                    SQL = SQL + ComNum.VBLF + "      FROM ADMIN.AEMRCHARTROW";
                    SQL = SQL + ComNum.VBLF + "      WHERE EMRNO = A.EMRNO";
                    SQL = SQL + ComNum.VBLF + "        AND EMRNOHIS = A.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "        AND ITEMCD = 'I0000018376')) AS WARDCALL ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                    SQL = SQL + ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW B";
                    SQL = SQL + ComNum.VBLF + "     ON A.EMRNO    = B.EMRNO";   
                    SQL = SQL + ComNum.VBLF + "    AND A.EMRNOHIS = B.EMRNOHIS";   
                    SQL = SQL + ComNum.VBLF + "    AND B.ITEMCD > CHR(0)";   
                    SQL = SQL + ComNum.VBLF + "     WHERE PTNO = '" + strPtno + "'";
                    SQL = SQL + ComNum.VBLF + "       AND CHARTDATE >= '" + strInDate.Replace("-", "") + "'";
                    SQL = SQL + ComNum.VBLF + "       AND CHARTDATE <= '" + strOutDate.Replace("-", "") + "'";
                    SQL = SQL + ComNum.VBLF + "       AND FORMNO = 966";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    bool[] bInfo = new bool[4];

                    if (dt.Rows.Count > 0 )
                    {
                        ssPrt2_Sheet1.Cells[11, 3, 16, 3].Text = "";
                        DataTable CopyTable = null;
                        var tmp = dt.AsEnumerable().Where(d => d.Field<string>("ITEMNO").Equals("I0000035170") && d.Field<string>("ITEMCD").IndexOf("_1") == -1).OrderBy(d => d.Field<string>("ITEMCD"));
                        if (tmp.Any())
                        {
                            CopyTable = tmp.CopyToDataTable();

                            for (int j = 0; j < CopyTable.Rows.Count; j++)
                            {
                                ssPrt2_Sheet1.Cells[11 + j, 3].Text = (j + 1) + ") " + CopyTable.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                        }
                       

                        if (CopyTable != null)
                        {
                            CopyTable.Dispose();
                        }


                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            #region 퇴원형태                            
                            if (string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) && 
                                dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035172") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrt2_Sheet1.Cells[9, 3].Text =  "퇴원지시후";
                            }
                            else if (string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035330") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrt2_Sheet1.Cells[9, 3].Text = "지시에 따르지 못하는 퇴원(자의퇴원)";
                            }

                            else if (string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035173_1") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrt2_Sheet1.Cells[9, 3].Text = "전원: ";
                            }

                            else if (!string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035173_2") &&
                                !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString()))
                            {
                                ssPrt2_Sheet1.Cells[9, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString();
                            }

                            else if (string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035175_1") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrt2_Sheet1.Cells[9, 3].Text = "사망: ";
                            }

                            else if (!string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000007586") &&
                                !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString()))
                            {
                                ssPrt2_Sheet1.Cells[9, 3].Text += "사망일자 : " + dt.Rows[j]["ITEMVALUE"].ToString() + " ";
                            }

                            else if (!string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000037849") &&
                                !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString()))
                            {
                                ssPrt2_Sheet1.Cells[9, 3].Text += "사망시간 : " + dt.Rows[j]["ITEMVALUE"].ToString() + " ";
                            }
                            else if (!string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000024438") &&
                                !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString()))
                            {
                                ssPrt2_Sheet1.Cells[9, 3].Text += "사망장소 : " + dt.Rows[j]["ITEMVALUE"].ToString();
                            }

                            else if (string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035176_1") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                ssPrt2_Sheet1.Cells[9, 3].Text = "기타: ";
                            }

                            else if (!string.IsNullOrWhiteSpace(ssPrt2_Sheet1.Cells[9, 3].Text) &&
                                dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035176_2") && 
                                !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString()))
                            {
                                ssPrt2_Sheet1.Cells[9, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString();
                            }
                            #endregion

                            #region 퇴원약 복약 안내

                            //퇴원약 제공
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035179") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                bInfo[0] = true;
                            }

                            //복약안내문 제공
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035180") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                bInfo[1] = true;
                            }

                            //복약설명
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035339") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                bInfo[2] = true;
                            }

                            //퇴원약 없음
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035338") &&
                                dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1"))
                            {
                                bInfo[3] = true;
                            }
                            #endregion


                            //복약안내문 제공
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035180"))
                            {
                                ssPrt2_Sheet1.Cells[16 + 2, 5].Text = dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "예" : "아니요";
                            }

                            //환자와의 관계
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000013388"))
                            {
                                ssPrt2_Sheet1.Cells[17 + 2, 5].Text = dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }

                            //퇴원 후 주의사항(문의 요하는 증
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000033457"))
                            {
                                ssPrt2_Sheet1.Cells[19 + 2, 3].Text = dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                                if (ssPrt2_Sheet1.Rows[19 + 2].GetPreferredHeight() > 80)
                                {
                                    ssPrt2_Sheet1.Rows[19 + 2].Height = ssPrt2_Sheet1.Rows[19 + 2].GetPreferredHeight() + 15;
                                }
                                else
                                {
                                    ssPrt2_Sheet1.Rows[19 + 2].Height = 80;
                                }
                            }

                            #region 퇴원후 관리 - 활동범위
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035183"))
                            {
                                ssPrt2_Sheet1.Cells[21 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "일상생활, " : "";
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035184"))
                            {
                                ssPrt2_Sheet1.Cells[21 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "안정, " : "";
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035185"))
                            {
                                ssPrt2_Sheet1.Cells[21 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "정기적인 운동, " : "";
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035186"))
                            {
                                ssPrt2_Sheet1.Cells[21 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "재활치료, " : "";
                            }
                            #endregion

                            #region 퇴원후 관리 - 식이
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035187"))
                            {
                                ssPrt2_Sheet1.Cells[22 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "일반식, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035188_1"))
                            {
                                ssPrt2_Sheet1.Cells[22 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "치료식 : " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035188_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrt2_Sheet1.Cells[22 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim() + ", ";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035189_1"))
                            {
                                ssPrt2_Sheet1.Cells[22 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "기타 :  " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035189_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrt2_Sheet1.Cells[22 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            #endregion

                            #region 퇴원후 관리 - 목 욕
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035190"))
                            {
                                ssPrt2_Sheet1.Cells[23 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "통목욕, " : "";
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035191"))
                            {
                                ssPrt2_Sheet1.Cells[23 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "샤워, " : "";
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035192"))
                            {
                                ssPrt2_Sheet1.Cells[23 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "침상목욕, " : "";
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035193"))
                            {
                                ssPrt2_Sheet1.Cells[23 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "목욕금지, " : "";
                            }

                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035194"))
                            {
                                ssPrt2_Sheet1.Cells[23 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "부분목욕, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035195_1"))
                            {
                                ssPrt2_Sheet1.Cells[23 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "기타, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035195_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrt2_Sheet1.Cells[23 + 2, 5].Text += "(" +  dt.Rows[j]["ITEMVALUE"].ToString().Trim() +  ")";
                            }
                            #endregion


                            #region 퇴원후 후 건강관리
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035196"))
                            {
                                ssPrt2_Sheet1.Cells[24 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "감염예방, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035197"))
                            {
                                ssPrt2_Sheet1.Cells[24 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "체중측정, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035198"))
                            {
                                ssPrt2_Sheet1.Cells[24 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "당뇨조절, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035199"))
                            {
                                ssPrt2_Sheet1.Cells[24 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "구강간호, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035200"))
                            {
                                ssPrt2_Sheet1.Cells[24 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "발간호, " : "";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035201_1"))
                            {
                                ssPrt2_Sheet1.Cells[24 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "튜브관리 : " : "";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035201_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrt2_Sheet1.Cells[24 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035202"))
                            {
                                ssPrt2_Sheet1.Cells[24 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "혈압조절, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035203"))
                            {
                                ssPrt2_Sheet1.Cells[24 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "좌욕, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035204"))
                            {
                                ssPrt2_Sheet1.Cells[24 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "체위변경, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035205"))
                            {
                                ssPrt2_Sheet1.Cells[24 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "상처간호, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035206_1"))
                            {
                                ssPrt2_Sheet1.Cells[24 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "기타 : " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035206_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrt2_Sheet1.Cells[24 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim() + ", ";
                            }

                            if (ssPrt2_Sheet1.Rows[24 + 2].GetPreferredHeight() > 80)
                            {
                                ssPrt2_Sheet1.Rows[24 + 2].Height = ssPrt2_Sheet1.Rows[24 + 2].GetPreferredHeight() + 15;
                            }
                            else
                            {
                                ssPrt2_Sheet1.Rows[24 + 2].Height = 80;
                            }
                            #endregion

                            #region 퇴원후 관리 - 추후관리
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035207"))
                            {
                                ssPrt2_Sheet1.Cells[25 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "외래, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035208"))
                            {
                                ssPrt2_Sheet1.Cells[25 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "가정간호, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035209"))
                            {
                                ssPrt2_Sheet1.Cells[25 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "호스피스, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035210"))
                            {
                                ssPrt2_Sheet1.Cells[25 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "재입원, " : "";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035211_1"))
                            {
                                ssPrt2_Sheet1.Cells[25 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "타병원 : " : "";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035211_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrt2_Sheet1.Cells[25 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim() + ", ";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035212_1"))
                            {
                                ssPrt2_Sheet1.Cells[25 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "기타 : " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035212_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrt2_Sheet1.Cells[25 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim() + ", ";
                            }
                            #endregion


                            #region 퇴원시 상태 - 의식상태 
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000034722"))
                            {
                                ssPrt2_Sheet1.Cells[29 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "명료, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000034723"))
                            {
                                ssPrt2_Sheet1.Cells[29 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "혼돈, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000037829"))
                            {
                                ssPrt2_Sheet1.Cells[29 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "기면, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000034724"))
                            {
                                ssPrt2_Sheet1.Cells[29 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "혼미, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035214"))
                            {
                                ssPrt2_Sheet1.Cells[29 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "무의식, " : "";
                            }
                            #endregion

                            #region 퇴원시 상태 - 이동방법
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035215"))
                            {
                                ssPrt2_Sheet1.Cells[30 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "도보, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035216"))
                            {
                                ssPrt2_Sheet1.Cells[30 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "자차, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035217"))
                            {
                                ssPrt2_Sheet1.Cells[30 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "구급차, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035218"))
                            {
                                ssPrt2_Sheet1.Cells[30 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "휠체어, " : "";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035219_1"))
                            {
                                ssPrt2_Sheet1.Cells[30 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "기타 :" : "";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035219_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrt2_Sheet1.Cells[30 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            #endregion

                            #region 퇴원시 상태 - 거주장소 갈곳 
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035220"))
                            {
                                ssPrt2_Sheet1.Cells[31 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "자가, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035221"))
                            {
                                ssPrt2_Sheet1.Cells[31 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "타병원, " : "";
                            }
                            if (dt.Rows[j]["ITEMNO"].ToString().Trim().Equals("I0000035222"))
                            {
                                ssPrt2_Sheet1.Cells[31 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "사설기관(요양원), " : "";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035224_1"))
                            {
                                ssPrt2_Sheet1.Cells[31 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "기타 : " : "";
                            }
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035224_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrt2_Sheet1.Cells[31 + 2, 5].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                            #endregion


                            #region 토원시 제공
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035225"))
                            {
                                ssPrt2_Sheet1.Cells[33 + 2, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "진료의뢰서, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035226"))
                            {
                                ssPrt2_Sheet1.Cells[33 + 2, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "검사결과지, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035227"))
                            {
                                ssPrt2_Sheet1.Cells[33 + 2, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "CD, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035228"))
                            {
                                ssPrt2_Sheet1.Cells[33 + 2, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "소견서, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035229"))
                            {
                                ssPrt2_Sheet1.Cells[33 + 2, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "입퇴원확인서, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035230"))
                            {
                                ssPrt2_Sheet1.Cells[33 + 2, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "진단서, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035231"))
                            {
                                ssPrt2_Sheet1.Cells[33 + 2, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "미해당, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035232_1"))
                            {
                                ssPrt2_Sheet1.Cells[33 + 2, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "기타 : " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035232_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrt2_Sheet1.Cells[33 + 2, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }
                  
                            #endregion

                            #region ◈ 퇴원교육대상
                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035233"))
                            {
                                ssPrt2_Sheet1.Cells[35 + 2, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "환자, " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035234_1"))
                            {
                                ssPrt2_Sheet1.Cells[35 + 2, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim().Equals("1") ? "보호자 : " : "";
                            }

                            if (dt.Rows[j]["ITEMCD"].ToString().Trim().Equals("I0000035234_2") && !string.IsNullOrWhiteSpace(dt.Rows[j]["ITEMVALUE"].ToString().Trim()))
                            {
                                ssPrt2_Sheet1.Cells[35 + 2, 3].Text += dt.Rows[j]["ITEMVALUE"].ToString().Trim();
                            }

                            #endregion
                        }

                        ssPrt2_Sheet1.Cells[37 + 2, 3].Text += dt.Rows[0]["WARDCALL"].ToString().Trim();

                        if (VB.Right(ssPrt2_Sheet1.Cells[21 + 2, 5].Text, 2) == ", ")
                        {
                            ssPrt2_Sheet1.Cells[21 + 2, 5].Text = VB.Left(ssPrt2_Sheet1.Cells[21 + 2, 5].Text, ssPrt2_Sheet1.Cells[21 + 2, 5].Text.Length - 2);
                        }

                        if (VB.Right(ssPrt2_Sheet1.Cells[33 + 2, 3].Text, 2) == ", ")
                        {
                            ssPrt2_Sheet1.Cells[33, 3].Text = VB.Left(ssPrt2_Sheet1.Cells[33, 3].Text, ssPrt2_Sheet1.Cells[33, 3].Text.Length - 2);
                        }

                        if (VB.Right(ssPrt2_Sheet1.Cells[35 + 2, 3].Text, 2) == ", ")
                        {
                            ssPrt2_Sheet1.Cells[35 + 2, 3].Text = VB.Left(ssPrt2_Sheet1.Cells[35 + 2, 3].Text, ssPrt2_Sheet1.Cells[35 + 2, 3].Text.Length - 2);
                        }

                        if (VB.Right(ssPrt2_Sheet1.Cells[31 + 2, 5].Text, 2) == ", ")
                        {
                            ssPrt2_Sheet1.Cells[31 + 2, 5].Text = VB.Left(ssPrt2_Sheet1.Cells[31 + 2, 5].Text, ssPrt2_Sheet1.Cells[31 + 2, 5].Text.Length - 2);
                        }


                        if (VB.Right(ssPrt2_Sheet1.Cells[29 + 2, 5].Text, 2) == ", ")
                        {
                            ssPrt2_Sheet1.Cells[29 + 2, 5].Text = VB.Left(ssPrt2_Sheet1.Cells[29 + 2, 5].Text, ssPrt2_Sheet1.Cells[29 + 2, 5].Text.Length - 2);
                        }

                        if (VB.Right(ssPrt2_Sheet1.Cells[30 + 2, 5].Text, 2) == ", ")
                        {
                            ssPrt2_Sheet1.Cells[30 + 2, 5].Text = VB.Left(ssPrt2_Sheet1.Cells[30 + 2, 5].Text, ssPrt2_Sheet1.Cells[30 + 2, 5].Text.Length - 2);
                        }

                        if (VB.Right(ssPrt2_Sheet1.Cells[26, 3].Text, 2) == ", ")
                        {
                            ssPrt2_Sheet1.Cells[26 + 2, 3].Text = VB.Left(ssPrt2_Sheet1.Cells[26 + 2, 3].Text, ssPrt2_Sheet1.Cells[26 + 2, 3].Text.Length - 2);
                        }


                        if (VB.Right(ssPrt2_Sheet1.Cells[24 + 2, 5].Text, 2) == ", ")
                        {
                            ssPrt2_Sheet1.Cells[24 + 2, 5].Text = VB.Left(ssPrt2_Sheet1.Cells[24 + 2, 5].Text, ssPrt2_Sheet1.Cells[24 + 2, 5].Text.Length - 2);
                        }


                        if (VB.Right(ssPrt2_Sheet1.Cells[23 + 2, 5].Text, 2) == ", ")
                        {
                            ssPrt2_Sheet1.Cells[23 + 2, 5].Text = VB.Left(ssPrt2_Sheet1.Cells[23 + 2, 5].Text, ssPrt2_Sheet1.Cells[23 + 2, 5].Text.Length - 2);
                        }


                        if (VB.Right(ssPrt2_Sheet1.Cells[22 + 2, 5].Text, 2) == ", ")
                        {
                            ssPrt2_Sheet1.Cells[22 + 2, 5].Text = VB.Left(ssPrt2_Sheet1.Cells[22 + 2, 5].Text, ssPrt2_Sheet1.Cells[22 + 2, 5].Text.Length - 2);
                        }

                        ssPrt2_Sheet1.Cells[43 + 2, 0].Text = "출력일시 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");
                        ssPrt2_Sheet1.Cells[43 + 2, 9].Text = "출력자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);

                        ssPrt2_Sheet1.Cells[39 + 2, 3].Text = READ_TEMP_JEPSU(strPtno);
                    }

                    if (bInfo[0] == false)
                    {
                        ssPrt2_Sheet1.Cells[16 + 2, 5].Text = "□ 퇴원약 제공";
                    }
                    else
                    {
                        ssPrt2_Sheet1.Cells[16 + 2, 5].Text = "■ 퇴원약 제공";
                    }

                    if (bInfo[1] == false)
                    {
                        ssPrt2_Sheet1.Cells[16 + 2, 5].Text += "  □ 복약안내문 제공";
                    }
                    else
                    {
                        ssPrt2_Sheet1.Cells[16 + 2, 5].Text += "  ■ 복약안내문 제공";
                    }

                    if (bInfo[2] == false)
                    {
                        ssPrt2_Sheet1.Cells[16 + 2, 5].Text += "  □ 복약설명";
                    }
                    else
                    {
                        ssPrt2_Sheet1.Cells[16 + 2, 5].Text += "  ■ 복약설명";
                    }

                    if (bInfo[3] == false)
                    {
                        ssPrt2_Sheet1.Cells[16 + 2, 5].Text += "  □ 퇴원약 없음";
                    }
                    else
                    {
                        ssPrt2_Sheet1.Cells[16 + 2, 5].Text += "  ■ 퇴원약 없음";
                    }
                    #endregion
                }

                dt.Dispose();
                dt = null;

     

                Cursor.Current = Cursors.Default;
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
                Cursor.Current = Cursors.Default;
            }
        }

        private string READ_TEMP_JEPSU(string strPtno)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            string rtnVal = "";
            string strTemp = "";
            string strRDate = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     RDATE, RTIME, DEPTCODE, B.DRNAME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "WORK_TEMPJEPSU A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.DrCode = B.DrCode";
                SQL = SQL + ComNum.VBLF + "         AND A.GUBUN = '01'";
                SQL = SQL + ComNum.VBLF + "         AND RDATE >= TO_DATE('" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + strPtno + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strTemp = "※임시대리접수내역 ※" + ComNum.VBLF;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strRDate = dt.Rows[i]["RDATE"].ToString().Trim();
                        strRDate = Convert.ToDateTime(strRDate).ToString("yyyy년 MM월 dd일");

                        strTemp += strRDate + " " + dt.Rows[i]["RTIME"].ToString().Trim()
                                            + " " + clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[i]["DEPTCODE"].ToString().Trim())
                                            + " " + dt.Rows[i]["DRNAME"].ToString().Trim() + ComNum.VBLF;
                    }

                    strTemp += " >> 대리 접수하였으므로 진료대기시간이 길어질 수 있습니다 <<";
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;

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
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        public void DRATC_AUTO_PRINT()
        {
            if (GstrHelpCode == "DRUG")
            {
                string[] str = VB.Split(GstrHelpName, "|");

                txtPano.Text = str[3];

                if (str[1] == "ER")
                {
                    cboWard.Text = "";
                    cboWard.Items.Clear();
                    cboWard.Items.Add("ER" + VB.Space(20) + "ER");
                }
                else
                {
                    cboWard.Text = "";
                    cboWard.Items.Clear();
                    cboWard.Items.Add("외래" + VB.Space(20) + "외래");
                }

                cboWard.SelectedIndex = 0;

                READ_복약안내문(str);

                GetPrint();
            }
        }

        private void READ_복약안내문(string[] str)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int k = 0;

            string strPANO = "";
            string strDate = "";
            string strWard = "";
            string strRoom = "";
            string strNAME = "";
            string strSEX = "";
            string strDEPT = "";
            string strDRNAME = "";

            string strDosCode = "";
            string strSuCode = "";
            string strIMAGE = "";
            string strDrugClear = "";

            Ftpedt FtpedtX = null;

            FarPoint.Win.ComplexBorder BorderWhite = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.ComplexBorder Border = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.Spread.CellType.CheckBoxCellType chkType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

            FarPoint.Win.Spread.CellType.TextCellType txtType = new FarPoint.Win.Spread.CellType.TextCellType();
            txtType.Multiline = true;
            txtType.WordWrap = true;

            FarPoint.Win.Spread.CellType.ImageCellType imgType = new FarPoint.Win.Spread.CellType.ImageCellType();
            imgType.Style = FarPoint.Win.RenderStyle.Stretch;

            Clear_SSpa();

            ssView_Sheet1.RowCount = 2;
            ssView_Sheet1.Cells[0, 0, 1, 0].Value = false;
            ssView_Sheet1.Cells[0, 1].Text = "";

            ssPrint_Sheet1.RowCount = 2;
            ssPrint_Sheet1.Cells[0, 0, 1, 0].Value = false;
            ssPrint_Sheet1.Cells[0, 1].Text = "";

            strDate = str[0];
            strWard = str[1];
            strRoom = str[2];
            strPANO = str[3];
            strNAME = str[4];
            strDEPT = str[5];
            strDRNAME = str[8];
            strSEX = str[9];

            ssPrt.Visible = false;
            ssPrt.Location = new Point(200, 10);
            ssPrt.Width = 100;
            ssPrt.Height = 10;

            if (GstrHelpCode != "DRUG")
            {
                strDrugClear = "";

                if (str[12] == "약없음")
                {
                    ComFunc.MsgBox("퇴원약이 없습니다."
                                    + ComNum.VBLF + "퇴원교육안내서만 조회됩니다."
                                    + ComNum.VBLF + "확인하고 인쇄하시기 바랍니다.");
                    strDrugClear = "퇴원약 없음";
                    ssPrt.Visible = true;
                    ssPrt.Location = new Point(0, 238);
                    ssPrt.Width = 610;
                    ssPrt.Height = 400;
                }

                if (rdoDrug1.Checked == true)
                {
                    if (READ_GBSUDAY(strPANO, dtpDate.Value.ToString("yyyy-MM-dd")) == true)
                    {
                        GstrPrtGbn = "일일수술";
                        READ_퇴원간호_일일수술(strPANO, strDate, strDate, strDrugClear, strWard, strRoom);
                    }
                    else if (strWard == "NR" || strWard == "IQ")
                    {
                        GstrPrtGbn = "신생아";
                        READ_퇴원간호_신생아(strPANO, strDate, strDate, strDrugClear);
                    }
                    else
                    {
                        GstrPrtGbn = "기타";
                        READ_퇴원간호_NEW(strPANO, strDate, strDate, strDrugClear);
                    }
                }

            }

            ssView_Sheet1.Cells[0, 1].Text = "병실:" + strRoom + VB.Space(3) + "환자명:" + strNAME + "(" + strSEX + ")" +
                            VB.Space(3) + "등록번호:" + strPANO + VB.Space(7) + "진료과:" + strDEPT + VB.Space(2) +
                            "의사명:" + strDRNAME + VB.Space(3) + "출력일자:" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssPrint_Sheet1.Cells[0, 1].Text = "병실:" + strRoom + VB.Space(3) + "환자명:" + strNAME + "(" + strSEX + ")" +
                            VB.Space(3) + "등록번호:" + strPANO + VB.Space(7) + "진료과:" + strDEPT + VB.Space(2) +
                            "의사명:" + strDRNAME + VB.Space(3) + "출력일자:" + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";

                if ((strWard == "외래" || strWard == "HD") && GstrHelpCode != "DRUG")
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3, B.ENREMARK4, B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "     B.ENREMARK5, C.IDOSNAME, A.DOSCODE, B.IMAGE_YN, A.SUCODE, B.JEHENG  ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW B, " + ComNum.DB_MED + "OCS_ODOSAGE C ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PTNO = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('11','12') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.DOSCODE = C.DOSCODE ";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBSUNAP = '1' ";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3,  B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "         B.ENREMARK4, B.ENREMARK5, C.IDOSNAME, A.DOSCODE,B.IMAGE_YN, B.JEHENG ";
                }
                else if ((strWard == "외래" || strWard == "HD") && GstrHelpCode == "DRUG")
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3, B.ENREMARK4, B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "     B.ENREMARK5, C.IDOSNAME, A.DOSCODE, B.IMAGE_YN, A.SUCODE, B.JEHENG";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW B, ";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_MED + "OCS_ODOSAGE C, " + ComNum.DB_MED + "OCS_DRUGATC D";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "         AND A.PtNO = '" + strPANO + "'";
                    SQL = SQL + ComNum.VBLF + "         AND A.PTNO = D.PANO";
                    SQL = SQL + ComNum.VBLF + "         AND A.BDATE = D.BDATE";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = D.SUCODE";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('11','12')";
                    SQL = SQL + ComNum.VBLF + "         AND A.DOSCODE = C.DOSCODE";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBSUNAP = '1'";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3,  B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "         B.ENREMARK4, B.ENREMARK5, C.IDOSNAME, A.DOSCODE,B.IMAGE_YN, B.JEHENG";
                }
                else if (strWard == "ER")
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3, B.ENREMARK7,  ";
                    SQL = SQL + ComNum.VBLF + "     B.ENREMARK4, B.ENREMARK5, C.IDOSNAME, A.DOSCODE, B.IMAGE_YN, A.SUCODE, B.JEHENG  ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER A, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW B, " + ComNum.DB_MED + "OCS_ODOSAGE C ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBTFLAG = 'T' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.PtNO = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('11','12') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.DOSCODE = C.DOSCODE ";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBPRN <> 'P' ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.GBSTATUS IS NULL OR A.GBSTATUS =' ')";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBIOE IN ('E','EI')";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3,  B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "         B.ENREMARK4, B.ENREMARK5, C.IDOSNAME, A.DOSCODE,B.IMAGE_YN, B.JEHENG  ";
                }
                else
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3,  B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "     B.ENREMARK4, B.ENREMARK5, C.IDOSNAME, A.DOSCODE, B.IMAGE_YN, B.JEHENG";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER A, " + ComNum.DB_MED + "OCS_DRUGINFO_NEW B, " + ComNum.DB_MED + "OCS_ODOSAGE C ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                    if (rdoDrug0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.GBTFLAG <> 'T' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.GBTFLAG = 'T' ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.PtNO = '" + strPANO + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND A.SUCODE = B.SUNEXT ";
                    SQL = SQL + ComNum.VBLF + "         AND A.BUN IN ('11','12') ";
                    SQL = SQL + ComNum.VBLF + "         AND A.DOSCODE = C.DOSCODE ";
                    SQL = SQL + ComNum.VBLF + "         AND A.GBPRN <> 'P' ";
                    SQL = SQL + ComNum.VBLF + "         AND (A.GBSTATUS IS NULL OR A.GBSTATUS =' ')";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.SUCODE, B.ENREMARK1, B.ENREMARK2, B.ENREMARK3, B.ENREMARK7, ";
                    SQL = SQL + ComNum.VBLF + "         B.ENREMARK4, B.ENREMARK5, C.IDOSNAME, A.DOSCODE, B.IMAGE_YN, B.JEHENG";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    Dir_Check(@"C:\PSMHEXE\YAK_IMAGE\");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 3;
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 3, ComNum.SPDROWHT * 2);
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 2, ComNum.SPDROWHT * 3);
                        ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT * 2);

                        ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 3;
                        ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 3, ComNum.SPDROWHT * 2);
                        ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 2, ComNum.SPDROWHT * 3);
                        ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 1, ComNum.SPDROWHT * 2);

                        for (k = 0; k < ssView_Sheet1.ColumnCount; k++)
                        {
                            if (k != 2)
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 0].CellType = chkType;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, k].RowSpan = 3;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, k].Border = Border;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, k].CellType = txtType;

                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 0].CellType = chkType;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, k].RowSpan = 3;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, k].Border = Border;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, k].CellType = txtType;
                            }
                            else
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, k].Border = BorderWhite;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, k].CellType = txtType;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, k].Border = BorderWhite;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, k].CellType = imgType;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].Border = Border;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, k].CellType = txtType;

                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, k].Border = BorderWhite;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, k].CellType = txtType;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, k].Border = BorderWhite;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, k].CellType = imgType;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, k].Border = Border;
                                ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, k].CellType = txtType;
                            }
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 1].Text = (i + 1).ToString();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 2].Text = dt.Rows[i]["ENREMARK1"].ToString().Trim();

                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 1].Text = (i + 1).ToString();
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 2].Text = dt.Rows[i]["ENREMARK1"].ToString().Trim();

                        strSuCode = dt.Rows[i]["SUCODE"].ToString().Trim();
                        strIMAGE = dt.Rows[i]["IMAGE_YN"].ToString().Trim();

                        if (strIMAGE == "Y")
                        {
                            string strFile = "";
                            string strHostFile = "";
                            string strHost = "";

                            strFile = @"C:\PSMHEXE\YAK_IMAGE\" + strSuCode.Trim().Replace("/", "__").ToUpper();
                            strHostFile = "/data/YAK_IMAGE/" + strSuCode.Trim().Replace("/", "__").ToUpper();
                            strHost = "/data/YAK_IMAGE/";

                            Image img = null;

                            FileInfo f = new FileInfo(strFile);
                                                        
                            try
                            {
                                if (f.Exists == true)
                                {
                                    f.Delete();
                                }

                                FtpedtX = new Ftpedt();

                                if (FtpedtX.FtpConnetBatch("192.168.100.33", "pcnfs", "pcnfs1") == false)
                                {
                                    ComFunc.MsgBox("FTP Server Connect ERROR !!!", "오류");
                                    return;
                                }

                                if (FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFile, strHostFile, strHost) == true)
                                {
                                    MemoryStream ms = new MemoryStream();
                                    Image.FromFile(strFile).Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].Value = Image.FromStream(ms);
                                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, 2].Value = Image.FromStream(ms);
                                }

                                FtpedtX.FtpDisConnetBatch();
                                FtpedtX = null;
                            }
                            catch
                            {
                                if (f.Exists == true)
                                {
                                    img = Image.FromFile(strFile);
                                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].Value = img;
                                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, 2].Value = img;

                                    img = null;
                                }
                                else
                                {
                                    FtpedtX = new Ftpedt();

                                    if (FtpedtX.FtpConnetBatch("192.168.100.33", "pcnfs", "pcnfs1") == false)
                                    {
                                        ComFunc.MsgBox("FTP Server Connect ERROR !!!", "오류");
                                        return;
                                    }

                                    if (FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFile, strHostFile, strHost) == true)
                                    {
                                        img = Image.FromFile(strFile);
                                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].Value = img;
                                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, 2].Value = img;

                                        img = null;
                                    }

                                    FtpedtX.FtpDisConnetBatch();
                                    FtpedtX = null;
                                }
                            }
                        }
                        else
                        {
                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 2, ComNum.SPDROWHT * 2);
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].CellType = txtType;
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].Text = dt.Rows[i]["ENREMARK2"].ToString().Trim();

                            ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 2, ComNum.SPDROWHT * 2);
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, 2].CellType = txtType;
                            ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, 2].Text = dt.Rows[i]["ENREMARK2"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].Text = dt.Rows[i]["ENREMARK1"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["JEHENG"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 4].Text = dt.Rows[i]["ENREMARK3"].ToString().Trim();

                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 2, 2].Text = dt.Rows[i]["ENREMARK1"].ToString().Trim();
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["JEHENG"].ToString().Trim();
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 4].Text = dt.Rows[i]["ENREMARK3"].ToString().Trim();

                        switch (VB.Left(dt.Rows[i]["DOSCODE"].ToString().Trim(), 2))
                        {
                            case "01": strDosCode = "1일 1회"; break;
                            case "02": strDosCode = "1일 2회"; break;
                            case "03": strDosCode = "1일 3회"; break;
                            case "04": strDosCode = "1일 4회"; break;
                            default: strDosCode = ""; break;
                        }

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 6].Text = strDosCode + ComNum.VBLF + dt.Rows[i]["IDOSNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 8].Text = dt.Rows[i]["ENREMARK5"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 10].Text = dt.Rows[i]["ENREMARK7"].ToString().Trim();
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 3, 11].Text = dt.Rows[i]["SUCODE"].ToString().Trim();

                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 6].Text = strDosCode + ComNum.VBLF + dt.Rows[i]["IDOSNAME"].ToString().Trim();
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 8].Text = dt.Rows[i]["ENREMARK5"].ToString().Trim();
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 10].Text = dt.Rows[i]["ENREMARK7"].ToString().Trim();
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 3, 11].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                    }

                    #region 2021-02-01

                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Font = new Font("맑은 고딕", 9f);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].ColumnSpan = 11;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Border = Border;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Border = Border;

                    ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                    ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Font = new Font("맑은 고딕", 9f);
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].ColumnSpan = 11;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Border = Border;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Border = Border;

                    if (GstrHelpCode == "DRUG")
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "♡의약품부작용 피해구제상담 : 한국의약품안전관리원(1644-6223, 14-3330)";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = "♡의약품부작용 피해구제상담 : 한국의약품안전관리원(1644-6223, 14-3330)";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "♡의약품부작용 피해구제상담 : 한국의약품안전관리원(1644-6223, 14-3330)";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = "♡의약품부작용 피해구제상담 : 한국의약품안전관리원(1644-6223, 14-3330)";
                    }

                    #endregion

                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Font = new Font("맑은 고딕", 9f);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].ColumnSpan = 11;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Border = Border;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Border = Border;

                    ssPrint_Sheet1.RowCount = ssPrint_Sheet1.RowCount + 1;
                    ssPrint_Sheet1.SetRowHeight(ssPrint_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Font = new Font("맑은 고딕", 9f);
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].ColumnSpan = 11;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 0].Border = Border;
                    ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Border = Border;

                    if (GstrHelpCode == "DRUG")
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "♡복용하시는 약에 관하여 궁금하신 사항은 약제팀(054-260-8058 또는 054-260-8057)로 문의하여 주십시오. ♠♠포항성모병원♠♠";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = "♡복용하시는 약에 관하여 궁금하신 사항은 약제팀(054-260-8058 또는 054-260-8057)로 문의하여 주십시오. ♠♠포항성모병원♠♠";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "♡복용하시는 약에 관하여 궁금하신 사항은 간호사실로 문의하여 주십시오.                  ♠♠포항성모병원♠♠";
                        ssPrint_Sheet1.Cells[ssPrint_Sheet1.RowCount - 1, 1].Text = "♡복용하시는 약에 관하여 궁금하신 사항은 간호사실로 문의하여 주십시오.                  ♠♠포항성모병원♠♠";
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                if (FtpedtX != null)
                {
                    FtpedtX.FtpDisConnetBatch();
                    FtpedtX = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private bool READ_GBSUDAY(string strPano, string strOutDate)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     GBSUDAY ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND OUTDATE = TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND GBSUDAY = 'Y' ";

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

        private void frmSupDrstDrugInfoPrint_Activated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        private void frmSupDrstDrugInfoPrint_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        private void cboWard_TextChanged(object sender, EventArgs e)
        {
            if (VB.Right(cboWard.Text, 4).Trim() == "ER" || VB.Right(cboWard.Text, 4).Trim() == "외래")
            {
                rdoDrug2.Checked = true;
            }
        }

        public string READ_DEPTCODE(string argName)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt10 = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT DEPTCODE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND DEPTNAMEK LIKE '%" + argName + "%'";
            SqlErr = clsDB.GetDataTable(ref dt10, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt10.Rows.Count > 0)
            {
                rtnVal = dt10.Rows[0]["DEPTCODE"].ToString().Trim();
            }

            dt10.Dispose();
            dt10 = null;

            return rtnVal;
        }
    }
}
