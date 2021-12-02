using ComBase;
using ComDbB;
using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstDrugIdentificationReferral.cs
    /// Description     : 약품식별 의뢰/회신 업무 현황
    /// Author          : 이정현
    /// Create Date     : 2017-12-06
    /// <history> 
    /// 약품식별 의뢰/회신 업무 현황
    /// </history>
    /// <seealso>
    /// PSMH\drug\drinfo\Frm약품식별의뢰서.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drinfo\drinfo.vbp
    /// </vbp>
    /// </summary>
    public partial class frmSupDrstDrugIdentificationReferral : Form
    {
        private string GstrPano = "";
        private string GstrWard = "";
        private string GstrBuse = "";
        private string GstrDRCODE = "";

        private string GstrWRTNO = "";
        private string GstrNoMatch = "";

        public frmSupDrstDrugIdentificationReferral()
        {
            InitializeComponent();
        }

        public frmSupDrstDrugIdentificationReferral(string strPano, string strWard = "", string strBuse = "", string strDRCODE = "")
        {
            InitializeComponent();

            GstrPano = strPano;
            GstrWard = strWard;
            GstrBuse = strBuse;
            GstrDRCODE = strDRCODE;
        }

        private void frmSupDrstDrugIdentificationReferral_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            panNoMatch.Visible = false;
            panHelp.Visible = false;
            panMsg.Visible = false;
            rdoTime1.Visible = false;

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-2);
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            txtPano2.Text = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            try
            {
                if (GstrBuse == "NR" || GstrDRCODE == "")
                {
                    cboWard.Text = "";
                    cboWard.Items.Clear();
                    cboWard.Items.Add(" ");
                    
                    cboWARD2.Text = "";
                    cboWARD2.Items.Clear();
                    cboWARD2.Items.Add(" ");

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     WARDCODE, WARDNAME ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY WARDCODE ";

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
                            cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim() + VB.Space(20) + dt.Rows[i]["WARDNAME"].ToString().Trim());
                            cboWARD2.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    cboWard.Items.Add("외래" + VB.Space(20) + "외래");
                    cboWARD2.Items.Add("외래");

                    cboWard.SelectedIndex = 0;
                    cboWARD2.SelectedIndex = 0;
                    
                    for (i = 0; i < cboWARD2.Items.Count; i++)
                    {
                        if (cboWARD2.Items[i].ToString() == GstrWard)
                        {
                            cboWARD2.SelectedIndex = i;
                            break;
                        }
                    }

                    cboDept.Text = "";
                    cboDept.Items.Clear();
                    cboDept.Items.Add(" ");

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     DEPTCODE, DEPTNAMEK ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                    SQL = SQL + ComNum.VBLF + "     WHERE DEPTCODE NOT IN ('OC', 'II', 'R6', 'TO', 'HR', 'PT', 'AN', 'HC', 'OM', 'LM') ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY PRINTRANKING ";

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
                            cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + VB.Space(20) + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    cboDept.SelectedIndex = 0;
                }
                else
                {
                    cboWard.Text = "";
                    cboWard.Items.Clear();
                    cboWard.Items.Add(" ");
                    cboWard.Items.Add("외래" + VB.Space(20) + "외래");
                    cboWard.SelectedIndex = 1;

                    cboWARD2.Text = "";
                    cboWARD2.Items.Clear();
                    cboWARD2.Items.Add(" ");                    
                    cboWARD2.Items.Add("외래");
                    cboWARD2.SelectedIndex = 1;

                    cboDept.Text = "";
                    cboDept.Items.Clear();
                    cboDept.Items.Add(" ");

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.DRDEPT1 AS DEPTCODE, B.DEPTNAMEK";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.DRCODE = '" + GstrDRCODE + "'";
                    SQL = SQL + ComNum.VBLF + "         AND A.DRDEPT1 = B.DEPTCODE";

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
                            cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + VB.Space(20) + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    cboDept.SelectedIndex = 1;
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
            
            if (clsType.User.Sabun == "04349") { cboWARD2.Enabled = true; }

            cboHosp.Text = "";
            cboHosp.Items.Clear();
            cboHosp.Items.Add(" ");
            cboHosp.Items.Add("01.우리병원");
            cboHosp.Items.Add("02.포항시 소재");
            cboHosp.Items.Add("03.경북 소재");
            cboHosp.Items.Add("04.대구시 소재");
            cboHosp.Items.Add("05.서울지역");
            cboHosp.Items.Add("06.부산지역");
            cboHosp.Items.Add("98.기타");
            cboHosp.Items.Add("99.불명");
            cboHosp.SelectedIndex = 0;

            rdoGubun1.Checked = true;

            SCREEN_CLEAR();

            ssPat_Sheet1.RowCount = 0;
            ssHIS_Sheet1.RowCount = 0;

            if (GstrPano != "")
            {
                dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-10);
                rdoGubun0.Checked = true;
                txtPano2.Text = GstrPano;

                if (GstrBuse == "EMRPRT")
                {
                    cboWARD2.Text = "외래";
                }

                GstrPano = "";
            }

            if (clsType.User.BuseCode == "044101")
            {
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            
            GetData();            
        }

        private void SCREEN_CLEAR()
        {
            panNoMatch.Visible = false;

            txtPano.Text = "";
            lblPaName.Text = "";
            cboDept.Text = "";
            lblPaDeptName.Text = "";
            cboWard.Text = "";
            cboRoom.Text = "";
            txtDrSabun.Text = "";
            lblPaDrName.Text = "";

            dtpBDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            dtpBTime.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":"));
            dtpHDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            dtpHTime.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":"));
            txtHosp.Text = "";
            cboHosp.Text = "";
            txtPhar.Text = "";
            txtDruggist.Text = "";
            lblPaDrugName.Text = "";

            rdoTime0.Checked = false;
            rdoTime1.Checked = false;
            rdoTime2.Checked = false;
            rdoTime3.Checked = false;
            rdoTime4.Checked = false;

            chkGubun0.Checked = false;
            chkGubun1.Checked = false;
            chkGubun2.Checked = false;
            chkGubun3.Checked = false;
            chkGubun4.Checked = false;
            chkGubun5.Checked = false;
            chkGubun6.Checked = false;
            chkGubun7.Checked = false;
            chkGubun8.Checked = false;
            chkGubun9.Checked = false;
            chkGubun10.Checked = false;
            chkGubun11.Checked = false;
            chkGubun12.Checked = false;
            chkGubun13.Checked = false;
            chkGubun14.Checked = false;
            chkGubun15.Checked = false;
            chkGubun16.Checked = false;

            txtRemark.Text = "";

            txtSABUN.Text = "";
            txtKORNAME.Text = "";

            GstrWRTNO = "";
            txtFast.Text = "";
            btnSave.Enabled = true;

            rdoAntiBlood1.Checked = false;
            rdoAntiBlood2.Checked = false;
            rdoAntiBlood3.Checked = false;

            txtQTY.Text = "";

            lblOurOrder.Text = "";
            
            ssPRT_CLEAR();
            ssPRT2_CLEAR();
            ssPRT3_CLEAR();
            ssDrugQTY_CLEAR();            
        }

        private void ssPRT_CLEAR()
        {
            ssPRT_Sheet1.RowCount = 17;

            ssPRT_Sheet1.Cells[3, 4].Text = "";
            ssPRT_Sheet1.Cells[3, 7].Text = "";
            ssPRT_Sheet1.Cells[3, 9].Text = "";

            ssPRT_Sheet1.Cells[4, 4].Text = "";
            ssPRT_Sheet1.Cells[4, 6].Text = "";
            ssPRT_Sheet1.Cells[4, 9].Text = "";

            ssPRT_Sheet1.Cells[5, 4].Text = "";
            ssPRT_Sheet1.Cells[5, 6].Text = "";
            ssPRT_Sheet1.Cells[5, 9].Text = "";

            ssPRT_Sheet1.Cells[6, 4].Text = "";
            ssPRT_Sheet1.Cells[6, 9].Text = "";

            ssPRT_Sheet1.Cells[7, 4].Text = "";
            ssPRT_Sheet1.Cells[7, 9].Text = "";

            ssPRT_Sheet1.Cells[12, 5].Value = false;
        }

        private void ssPRT2_CLEAR()
        {
            ssPRT2_Sheet1.RowCount = 10;

            ssPRT2_Sheet1.Cells[1, 4].Text = "";
            ssPRT2_Sheet1.Cells[1, 8].Text = "";
            ssPRT2_Sheet1.Cells[1, 12].Text = "";
            ssPRT2_Sheet1.Cells[1, 16].Text = "";

            ssPRT2_Sheet1.Cells[2, 4].Text = "";
            ssPRT2_Sheet1.Cells[2, 8].Text = "";
            ssPRT2_Sheet1.Cells[2, 12].Text = "";
            ssPRT2_Sheet1.Cells[2, 16].Text = "";

            ssPRT2_Sheet1.Cells[3, 4].Text = "";
            ssPRT2_Sheet1.Cells[3, 8].Text = "";
            ssPRT2_Sheet1.Cells[3, 12].Text = "";
            ssPRT2_Sheet1.Cells[3, 16].Text = "";

            ssPRT2_Sheet1.Cells[4, 4].Text = "";
            ssPRT2_Sheet1.Cells[4, 8].Text = "";
            ssPRT2_Sheet1.Cells[4, 14].Text = "";

            ssPRT2_Sheet1.Cells[5, 5].Text = "";
            ssPRT2_Sheet1.Cells[5, 14].Text = "";
        }

        private void ssPRT3_CLEAR()
        {            
            ssPRT3_Sheet1.RowCount = 12;

            ssPRT3_Sheet1.Cells[1, 3].Text = "";
            ssPRT3_Sheet1.Cells[1, 5].Text = "";
            ssPRT3_Sheet1.Cells[1, 8].Text = "";
            ssPRT3_Sheet1.Cells[1, 10].Text = "";

            ssPRT3_Sheet1.Cells[2, 3].Text = "";
            ssPRT3_Sheet1.Cells[2, 5].Text = "";
            ssPRT3_Sheet1.Cells[2, 8].Text = "";
            ssPRT3_Sheet1.Cells[2, 10].Text = "";
            ssPRT3_Sheet1.Cells[2, 13].Text = "";

            ssPRT3_Sheet1.Cells[3, 3].Text = "";
            ssPRT3_Sheet1.Cells[3, 7].Text = "";
            ssPRT3_Sheet1.Cells[3, 10].Text = "";
            ssPRT3_Sheet1.Cells[3, 11].Text = "";
            ssPRT3_Sheet1.Cells[3, 13].Text = "";

            ssPRT3_Sheet1.Cells[4, 3].Text = "";
            ssPRT3_Sheet1.Cells[4, 13].Text = "";

            ssPRT3_Sheet1.Cells[5, 3].Text = "";            
            ssPRT3_Sheet1.Cells[5, 5].Text = "";

            ssPRT3_Sheet1.Cells[6, 3].Text = "";
            ssPRT3_Sheet1.Cells[7, 3].Text = "";            
        }

        private void ssDrugQTY_CLEAR()
        {
            ssDrugQTY_Sheet1.Cells[1, 2, 5, 2].Text = "";
            ssDrugQTY_Sheet1.Cells[1, 5, 4, 17].Text = "";

            ssDrugQTY_Sheet1.Cells[5, 2].Text = "";
            ssDrugQTY_Sheet1.Cells[5, 7].Text = "";
            ssDrugQTY_Sheet1.Cells[5, 15].Text = "";
        }

        private void rdoGubun_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoGubun0.Checked == true)
            {
                grpDate.Enabled = true;
            }
            else if (rdoGubun1.Checked == true)
            {
                grpDate.Enabled = false;
            }
        }

        private void txtPano2_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtPano2.Text.Trim() == "") { return; }

            if (e.KeyCode == Keys.Enter)
            {
                txtPano2.Text = ComFunc.LPAD(txtPano2.Text, 8, "0");

                GetData();
            }
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

            SCREEN_CLEAR();

            ssPat_Sheet1.RowCount = 0;
            ssHIS_Sheet1.RowCount = 0;

            try
            {
                SQL = "";

                if (rdoGubun1.Checked == true)
                {
                    if (cboWARD2.Text == "외래")
                    {
                        if (txtPano2.Text.Trim() == "")
                        {
                            ComFunc.MsgBox("외래는 등록번호를 입력하여 조회하시기 바랍니다.");
                            return;
                        }

                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     'OPD' AS ROOMCODE, PANO, SNAME, TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE, '' AS BUN, '' AS ROWID1, '' AS WRTNO, '' AS JDATE, '' AS NOMATCH, '' AS DRSABUN, DRCODE";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER";
                        SQL = SQL + ComNum.VBLF + "     WHERE BDATE >= TRUNC(SYSDATE-30) ";
                        SQL = SQL + ComNum.VBLF + "         AND PANO = '" + txtPano2.Text.Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY BDATE DESC ";
                    }
                    else
                    {
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     ROOMCODE, PANO, SNAME, TO_CHAR(INDATE,'YYYY-MM-DD') AS BDATE, '' AS BUN, '' AS ROWID1, '' AS WRTNO, '' AS JDATE, '' AS NOMATCH, '' AS DRSABUN, DRCODE ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                        SQL = SQL + ComNum.VBLF + "     WHERE JDATE = TO_DATE('1900-01-01', 'YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "         AND GBSTS NOT IN ('7', '9')";

                        if (txtPano2.Text.Trim() != "")
                        {
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + txtPano2.Text.Trim() + "' ";
                        }

                        if (cboWARD2.Text == "MICU")
                        {
                            SQL = SQL + ComNum.VBLF + "         AND WARDCODE = 'IU' ";
                            SQL = SQL + ComNum.VBLF + "         AND ROOMCODE = '234' ";
                        }
                        else if (cboWARD2.Text == "SICU")
                        {
                            SQL = SQL + ComNum.VBLF + "         AND WARDCODE = 'IU' ";
                            SQL = SQL + ComNum.VBLF + "         AND ROOMCODE = '233' ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "         AND WARDCODE = '" + cboWARD2.Text + "' ";
                        }

                        SQL = SQL + ComNum.VBLF + "ORDER BY ROOMCODE, SNAME";
                    }
                }
                else if (rdoGubun0.Checked == true)
                {
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     A.ROOMCODE, A.PANO, B.SNAME, TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, ";
                    SQL = SQL + ComNum.VBLF + "     A.BUN, A.ROWID ROWID1, A.WRTNO, TO_CHAR(A.JDATE, 'HH24:MI') AS JDATE, A.NOMATCH, A.DRSABUN, '' AS DRCODE ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "         AND A.BDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + " 00:00', 'YYYY-MM-DD HH24:MI')";
                    SQL = SQL + ComNum.VBLF + "         AND A.BDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + " 23:59', 'YYYY-MM-DD HH24:MI') ";

                    if (txtPano2.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.PANO = '" + txtPano2.Text.Trim() + "' ";
                    }
                    else
                    {
                        if (cboWARD2.Text == "외래")
                        {
                            SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'O' ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "         AND A.IPDOPD = 'I' ";

                            if (cboWARD2.Text == "MICU")
                            {
                                SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'IU' ";
                                SQL = SQL + ComNum.VBLF + "         AND A.ROOMCODE = '234' ";
                            }
                            else if (cboWARD2.Text == "SICU")
                            {
                                SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = 'IU' ";
                                SQL = SQL + ComNum.VBLF + "         AND A.ROOMCODE = '233' ";
                            }
                            else
                            {
                                SQL = SQL + ComNum.VBLF + "         AND A.WARDCODE = '" + cboWARD2.Text + "' ";
                            }
                        }
                    }

                    if (chkMine.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.DRSABUN = '" + clsType.User.Sabun + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "ORDER BY A.ROOMCODE, B.SNAME";
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
                    ssPat_Sheet1.RowCount = dt.Rows.Count;
                    ssPat_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssPat_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BUN"].ToString().Trim();

                        if (ssPat_Sheet1.Cells[i, 4].Text.Trim() == "1")
                        {
                            ssPat_Sheet1.Cells[i, 4].Text = "진행중";
                        }
                        else if (ssPat_Sheet1.Cells[i, 4].Text.Trim() == "2")
                        {
                            ssPat_Sheet1.Cells[i, 4].Text = "완료";
                        }
                        else
                        {
                            ssPat_Sheet1.Cells[i, 4].Text = "";
                        }

                        if (dt.Rows[i]["NOMATCH"].ToString().Trim() == "1")
                        {
                            ssPat_Sheet1.Cells[i, 4].Text = "재의뢰요망";
                            ssPat_Sheet1.Cells[i, 4].ForeColor = Color.Red;
                        }

                        ssPat_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ROWID1"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 6].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                        ssPat_Sheet1.Cells[i, 7].Text = dt.Rows[i]["JDATE"].ToString().Trim();

                        if (dt.Rows[i]["DRSABUN"].ToString().Trim() != "")
                        {
                            ssPat_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DRSABUN"].ToString().Trim();
                        }

                        if (dt.Rows[i]["DRCODE"].ToString().Trim() != "")
                        {
                            ssPat_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        }

                        ssPat_Sheet1.Cells[i, 8].Text = Read_DeptCode(ssPat_Sheet1.Cells[i, 8].Text.Trim());
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

        private string Read_DeptCode(string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DEPTCODE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                SQL = SQL + ComNum.VBLF + "     WHERE (DRCODE = '" + strCode + "') OR (SABUN = '" + strCode + "')";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DEPTCODE"].ToString().Trim();
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

        private void ssPat_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (e.ColumnHeader == true || e.RowHeader == true) { return; }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            int intA = 0;

            string strPTNO = "";
            string strRoom = "";
            string strInDate = "";
            string strSname = "";
            string strDeptCode = "";

            SCREEN_CLEAR();

            ssHIS_Sheet1.RowCount = 0;

            btnDelete.Enabled = true;

            strPTNO = ssPat_Sheet1.Cells[e.Row, 1].Text.Trim();
            strSname = ssPat_Sheet1.Cells[e.Row, 2].Text.Trim();
            strRoom = ssPat_Sheet1.Cells[e.Row, 0].Text.Trim();
            strInDate = ssPat_Sheet1.Cells[e.Row, 3].Text.Trim();
            strDeptCode = ssPat_Sheet1.Cells[e.Row, 8].Text.Trim();

            GstrWRTNO = ssPat_Sheet1.Cells[e.Row, 6].Text.Trim();
            GstrNoMatch = ssPat_Sheet1.Cells[e.Row, 4].Text.Trim() == "재의뢰요망" ? "Y" : "";
            
            lblOurOrder.Text = Read_DRUG_HOISLIP(clsDB.DbCon, GstrWRTNO);

            if (ssPat_Sheet1.Cells[e.Row, 4].Text.Trim() == "진행중")
            {
                ComFunc.MsgBox("해당 회신서는 약제에서 회신작업 중입니다."
                    + ComNum.VBLF + "회신작업이 완료 된 후에 조회가 가능합니다.");
                return;
            }
            else if (ssPat_Sheet1.Cells[e.Row, 4].Text.Trim() == "완료")
            {
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }

            if (strPTNO == "") { return; }

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE, A.PANO, B.SNAME, DECODE(A.BUN, '1', '부분완료','2','완료','') AS BUN, ";
                SQL = SQL + ComNum.VBLF + "     A.ROWID, A.WRTNO, B.SNAME, A.NOMATCH, A.ROOMCODE, A.DEPTCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B  ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssHIS_Sheet1.RowCount = dt.Rows.Count;
                    ssHIS_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssHIS_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();

                        if (VB.Val(ComFunc.TimeDiffMin(dt.Rows[i]["BDATE"].ToString().Trim(), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"))) <= 43200
                            && intA < 1 && VB.Val(ComFunc.TimeDiffMin(dt.Rows[i]["BDATE"].ToString().Trim(), ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"))) > 0)
                        {
                            ComFunc.MsgBox(Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()).ToString("yyyy년 MM월 dd일") + " 식별의뢰/회신 건 있음(한달이내 의뢰건)");
                            intA = 1;
                        }

                        ssHIS_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssHIS_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssHIS_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BUN"].ToString().Trim();

                        if (dt.Rows[i]["NOMATCH"].ToString().Trim() == "1")
                        {
                            ssHIS_Sheet1.Cells[i, 3].Text = "재의뢰요망";
                            ssHIS_Sheet1.Cells[i, 3].ForeColor = Color.Red;
                        }

                        if (dt.Rows[i]["ROOMCODE"].ToString().Trim() == "OPD")
                        {
                            ssHIS_Sheet1.Cells[i, 3].Text = ssHIS_Sheet1.Cells[i, 3].Text + " ★외래식별(" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + ")";
                        }

                        ssHIS_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssHIS_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                if (rdoGubun1.Checked == true)
                {
                    if (ComFunc.MsgBoxQ("신규 약품 식별을 의뢰하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        btnSave.Enabled = true;
                        btnDelete.Enabled = true;

                        SQL = "";

                        if (strRoom == "OPD")
                        {
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.DEPTCODE, '외래' AS WARDCODE, 'OPD' AS ROOMCODE, A.DRCODE, B.DRNAME ";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
                            SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPTNO + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND A.BDATE = TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                            //SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE = '" + strDeptCode + "' ";
                        }
                        else
                        {
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     A.PANO, A.SNAME, A.DEPTCODE, A.WARDCODE, A.ROOMCODE, A.DRCODE, B.DRNAME ";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
                            SQL = SQL + ComNum.VBLF + "     WHERE A.PANO = '" + strPTNO + "' ";
                            SQL = SQL + ComNum.VBLF + "         AND TRUNC(A.INDATE) = TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                            //SQL = SQL + ComNum.VBLF + "         AND A.DEPTCODE = '" + strDeptCode + "' ";
                        }

                        SQL = SQL + ComNum.VBLF + "         AND A.DRCODE = B.DRCODE";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            //2020-04-09 안정수 추가, 1개이상 진료과 접수내역이 있을 경우
                            if (dt.Rows.Count > 1)
                            {
                                if (MessageBox.Show(dt.Rows[0]["DEPTCODE"].ToString().Trim() + " 진료과 외 "
                                    + dt.Rows[1]["DEPTCODE"].ToString().Trim() + "과 접수내역 이 있습니다."
                                    + dt.Rows[1]["DEPTCODE"].ToString().Trim() + "로 변경 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    txtPano.Text = dt.Rows[0]["PANO"].ToString().Trim();
                                    lblPaName.Text = dt.Rows[0]["SNAME"].ToString().Trim();

                                    ComFunc.ComboFind(cboDept, "L", 4, dt.Rows[0]["DEPTCODE"].ToString().Trim());
                                    ComFunc.ComboFind(cboWard, "L", 4, dt.Rows[0]["WARDCODE"].ToString().Trim());

                                    cboRoom.Text = "";
                                    cboRoom.Items.Clear();
                                    cboRoom.Items.Add(dt.Rows[0]["ROOMCODE"].ToString().Trim());
                                    cboRoom.SelectedIndex = 0;

                                    txtDrSabun.Text = dt.Rows[0]["DRCODE"].ToString().Trim();
                                    lblPaDrName.Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                                }
                                else
                                {
                                    txtPano.Text = dt.Rows[0]["PANO"].ToString().Trim();
                                    lblPaName.Text = dt.Rows[0]["SNAME"].ToString().Trim();

                                    ComFunc.ComboFind(cboDept, "L", 4, dt.Rows[1]["DEPTCODE"].ToString().Trim());
                                    ComFunc.ComboFind(cboWard, "L", 4, dt.Rows[1]["WARDCODE"].ToString().Trim());

                                    cboRoom.Text = "";
                                    cboRoom.Items.Clear();
                                    cboRoom.Items.Add(dt.Rows[1]["ROOMCODE"].ToString().Trim());
                                    cboRoom.SelectedIndex = 0;

                                    txtDrSabun.Text = dt.Rows[1]["DRCODE"].ToString().Trim();
                                    lblPaDrName.Text = dt.Rows[1]["DRNAME"].ToString().Trim();
                                }
                            }
                            else
                            {
                                txtPano.Text = dt.Rows[0]["PANO"].ToString().Trim();
                                lblPaName.Text = dt.Rows[0]["SNAME"].ToString().Trim();

                                ComFunc.ComboFind(cboDept, "L", 4, dt.Rows[0]["DEPTCODE"].ToString().Trim());
                                ComFunc.ComboFind(cboWard, "L", 4, dt.Rows[0]["WARDCODE"].ToString().Trim());

                                cboRoom.Text = "";
                                cboRoom.Items.Clear();
                                cboRoom.Items.Add(dt.Rows[0]["ROOMCODE"].ToString().Trim());
                                cboRoom.SelectedIndex = 0;

                                txtDrSabun.Text = dt.Rows[0]["DRCODE"].ToString().Trim();
                                lblPaDrName.Text = dt.Rows[0]["DRNAME"].ToString().Trim();
                            }

                            txtSABUN.Text = clsType.User.Sabun;
                            txtKORNAME.Text = clsVbfunc.GetInSaName(clsDB.DbCon, clsType.User.Sabun);
                            dtpBDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
                            dtpBTime.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":"));
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }

                if (rdoGubun0.Checked == true)
                {
                    SET_DRUGHOI(GstrWRTNO);
                    SET_DRUGHOI2(GstrWRTNO);
                    SET_DRUGHOI3(GstrWRTNO);
                    READ_HOIMST(GstrWRTNO);
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

        private void SET_DRUGHOI(string strWRTNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strSAYU1 = "";
            string strSAYU2 = "";
            string strSAYU3 = "";
            string strSAYU4 = "";
            string strSAYU5 = "";
            string strSAYU6 = "";
            string strSAYU7 = "";
            string strSAYU8 = "";
            string strSAYU9 = "";
            string strSAYU10 = "";
            string strSAYU11 = "";
            string strSAYU12 = "";
            string strSAYU13 = "";
            string strSAYU14 = "";
            string strSAYU15 = "";
            string strSAYU16 = "";
            string strSAYU17 = "";
            string strTemp = "";
            string strTemp2 = "";
            string strJEP1 = "";
            string strJEP2 = "";
            string strJEP3 = "";
            
            FarPoint.Win.ComplexBorder BorderWhite = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), 
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), 
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), 
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.White), 
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderLeft1 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine, System.Drawing.Color.Black), 
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), 
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), 
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), 
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder Border1 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderRight1 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), 
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), 
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine, System.Drawing.Color.Black), 
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), 
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderLeft2 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine, System.Drawing.Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine, System.Drawing.Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder Border2 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine, System.Drawing.Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderRight2 = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine, System.Drawing.Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.MediumLine, System.Drawing.Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            ssPRT_CLEAR();

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.JDATE, A.BDATE, A.DEPTCODE, A.DRCODE, A.DRSABUN, A.WARDCODE, ";
                SQL = SQL + ComNum.VBLF + "     A.ROOMCODE, A.PANO, A.REMCODE1, A.REMCODE2, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE3, A.REMCODE4, A.REMCODE5, A.REMCODE6, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE7, A.REMCODE8, A.REMCODE9, A.REMCODE10, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE11, A.REMCODE12, A.REMCODE13, A.REMCODE14, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE15, A.REMCODE16, A.REMCODE17, ";
                SQL = SQL + ComNum.VBLF + "     A.HOSP, A.PHAR, B.SNAME, B.JUMIN1, B.JUMIN2, B.JUMIN3, A.DABCODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.WRTNO = '" + strWRTNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + " 00:00','YYYY-MM-DD HH24:MI') ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDATE <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["BDATE"].ToString().Trim() != "")
                    {
                        ssPRT_Sheet1.Cells[3, 4].Text = Convert.ToDateTime(dt.Rows[0]["BDATE"].ToString().Trim()).ToString("yyyy-MM-dd, HH:mm");
                    }

                    ssPRT_Sheet1.Cells[3, 6].Text = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    ssPRT_Sheet1.Cells[3, 9].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());

                    if (dt.Rows[0]["JDATE"].ToString().Trim() != "")
                    {
                        ssPRT_Sheet1.Cells[4, 4].Text = Convert.ToDateTime(dt.Rows[0]["JDATE"].ToString().Trim()).ToString("yyyy-MM-dd, HH:mm");
                    }

                    ssPRT_Sheet1.Cells[5, 4].Text = dt.Rows[0]["WARDCODE"].ToString().Trim() + " / " + dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    ssPRT_Sheet1.Cells[5, 6].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ssPRT_Sheet1.Cells[5, 9].Text = dt.Rows[0]["PANO"].ToString().Trim();

                    switch (dt.Rows[0]["DABCODE"].ToString().Trim())
                    {
                        case "01": ssPRT_Sheet1.Cells[6, 4].Text = "10분 이내"; break;
                        case "02": ssPRT_Sheet1.Cells[6, 4].Text = "30분 이내"; break;
                        case "03": ssPRT_Sheet1.Cells[6, 4].Text = "1시간 이내"; break;
                        case "04": ssPRT_Sheet1.Cells[6, 4].Text = "2시간 이내"; break;
                        case "05": ssPRT_Sheet1.Cells[6, 4].Text = "3시간 이내"; break;
                        case "06": ssPRT_Sheet1.Cells[6, 4].Text = "금일 이내"; break;
                        case "07": ssPRT_Sheet1.Cells[6, 4].Text = "48시간 이내"; break;
                    }

                    ssPRT_Sheet1.Cells[6, 6].Text = clsVbfunc.READ_INSA_BUSE(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());
                    ssPRT_Sheet1.Cells[6, 9].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());

                    strSAYU1 = dt.Rows[0]["REMCODE1"].ToString().Trim();
                    strSAYU2 = dt.Rows[0]["REMCODE2"].ToString().Trim();
                    strSAYU3 = dt.Rows[0]["REMCODE3"].ToString().Trim();
                    strSAYU4 = dt.Rows[0]["REMCODE4"].ToString().Trim();
                    strSAYU5 = dt.Rows[0]["REMCODE5"].ToString().Trim();
                    strSAYU6 = dt.Rows[0]["REMCODE6"].ToString().Trim();
                    strSAYU7 = dt.Rows[0]["REMCODE7"].ToString().Trim();
                    strSAYU8 = dt.Rows[0]["REMCODE8"].ToString().Trim();
                    strSAYU9 = dt.Rows[0]["REMCODE9"].ToString().Trim();
                    strSAYU10 = dt.Rows[0]["REMCODE10"].ToString().Trim();
                    strSAYU11 = dt.Rows[0]["REMCODE11"].ToString().Trim();
                    strSAYU12 = dt.Rows[0]["REMCODE12"].ToString().Trim();
                    strSAYU13 = dt.Rows[0]["REMCODE13"].ToString().Trim();
                    strSAYU14 = dt.Rows[0]["REMCODE14"].ToString().Trim();
                    strSAYU15 = dt.Rows[0]["REMCODE15"].ToString().Trim();
                    strSAYU16 = dt.Rows[0]["REMCODE16"].ToString().Trim();
                    strSAYU17 = dt.Rows[0]["REMCODE17"].ToString().Trim();

                    strTemp = "";
                    strTemp = strTemp + strSAYU1 != "" ? strSAYU1 + ", " : "";
                    strTemp = strTemp + strSAYU2 != "" ? strSAYU2 + ", " : "";
                    strTemp = strTemp + strSAYU3 != "" ? strSAYU3 + ", " : "";
                    strTemp = strTemp + strSAYU4 != "" ? strSAYU4 + ", " : "";
                    strTemp = strTemp + strSAYU5 != "" ? strSAYU5 + ", " : "";
                    strTemp = strTemp + strSAYU7 != "" ? strSAYU7 + ", " : "";
                    strTemp = strTemp + strSAYU8 != "" ? strSAYU8 + ", " : "";
                    strTemp = strTemp + strSAYU9 != "" ? strSAYU9 + ", " : "";
                    strTemp = strTemp + strSAYU10 != "" ? strSAYU10 + ", " : "";
                    strTemp = strTemp + strSAYU11 != "" ? strSAYU11 + ", " : "";
                    strTemp = strTemp + strSAYU12 != "" ? strSAYU12 + ", " : "";
                    strTemp = strTemp + strSAYU13 != "" ? strSAYU13 + ", " : "";
                    strTemp = strTemp + strSAYU14 != "" ? strSAYU14 + ", " : "";
                    strTemp = strTemp + strSAYU15 != "" ? strSAYU15 + ", " : "";
                    strTemp = strTemp + strSAYU16 != "" ? strSAYU16 + ", " : "";
                    strTemp = strTemp + strSAYU17 != "" ? strSAYU17 + ", " : "";
                    strTemp = strTemp + strSAYU6 != "" ? strSAYU6 + ", " : "";

                    if (strTemp != "")
                    {
                        strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2);
                    }

                    ssPRT_Sheet1.Cells[7, 4].Text = strTemp;

                    if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    {
                        ssPRT_Sheet1.Cells[7, 8].Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + " - " + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                    }
                    else
                    {
                        ssPRT_Sheet1.Cells[7, 8].Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + " - " + dt.Rows[0]["JUMIN2"].ToString().Trim();
                    }

                    ssPRT_Sheet1.Cells[8, 4].Text = dt.Rows[0]["HOSP"].ToString().Trim();
                    ssPRT_Sheet1.Cells[8, 8].Text = dt.Rows[0]["PHAR"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                ssPRT_Sheet1.Cells[12, 5].Value = false;
                ssPRT_Sheet1.Cells[12, 6].Value = true;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     REMARK1, REMARK2, REMARK3, REMARK4, ";
                SQL = SQL + ComNum.VBLF + "     REMARK5, REMARK6, REMARK7, REMARK8, ";
                SQL = SQL + ComNum.VBLF + "     REMARK9, REMARK10, REMARK11, REMARK12, ";
                SQL = SQL + ComNum.VBLF + "     REMARK13, REMARK14, BLOOD, ROWID, EDICODE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOISLIP ";
                SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + strWRTNO;

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
                        ssPRT_Sheet1.RowCount = ssPRT_Sheet1.RowCount + 1;
                        ssPRT_Sheet1.SetRowHeight(ssPRT_Sheet1.RowCount - 1, 50);
                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 0, ssPRT_Sheet1.RowCount - 1, ssPRT_Sheet1.ColumnCount - 1].Border = BorderWhite;

                        if (i == dt.Rows.Count - 1)
                        {
                            ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 2].Border = BorderLeft2;
                            ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 3, ssPRT_Sheet1.RowCount - 1, 8].Border = Border2;
                            ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 9].Border = BorderRight2;
                        }
                        else
                        { 
                            ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 2].Border = BorderLeft1;
                            ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 3, ssPRT_Sheet1.RowCount - 1, 8].Border = Border1;
                            ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 9].Border = BorderRight1;
                        }

                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 2].Text = (i + 1).ToString();
                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["REMARK1"].ToString().Trim()
                            + (dt.Rows[i]["EDICODE"].ToString().Trim() != "" ? ComNum.VBLF + "(" + dt.Rows[i]["EDICODE"].ToString().Trim() + ")" : "")
                            + (dt.Rows[i]["REMARK7"].ToString().Trim() != "" ? ComNum.VBLF + "(" + dt.Rows[i]["REMARK7"].ToString().Trim() + ")" : "");
                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["REMARK2"].ToString().Trim();
                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["REMARK3"].ToString().Trim();
                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["REMARK8"].ToString().Trim();

                        strTemp2 = dt.Rows[i]["REMARK14"].ToString().Trim();

                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["REMARK6"].ToString().Trim()
                            + (strTemp2 != "" ? ComNum.VBLF + "───────" + ComNum.VBLF + "수량 : " + strTemp2 : "");
                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 8].Text = (dt.Rows[i]["REMARK9"].ToString().Trim() != "" ? "앞)" + dt.Rows[i]["REMARK9"].ToString().Trim() : "")
                            + (dt.Rows[i]["REMARK10"].ToString().Trim() != "" ? ComNum.VBLF + "뒤)" + dt.Rows[i]["REMARK10"].ToString().Trim() : "");

                        strJEP1 = dt.Rows[i]["REMARK5"].ToString().Trim();
                        strJEP2 = dt.Rows[i]["REMARK12"].ToString().Trim();
                        strJEP3 = dt.Rows[i]["REMARK13"].ToString().Trim();

                        if (strJEP1 != "") { strJEP1 = ComNum.VBLF + strJEP1; }
                        if (strJEP2 != "") { strJEP2 = ComNum.VBLF + strJEP2; }
                        if (strJEP3 != "") { strJEP3 = ComNum.VBLF + strJEP3; }

                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 9].Text = READ_USED_GUBUN(dt.Rows[i]["REMARK11"].ToString().Trim()) + ((strJEP1 + strJEP2 + strJEP3) != "" ? ComNum.VBLF + strJEP1 + strJEP2 + strJEP3 : "");
                        ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        //항혈전제 표시
                        if (dt.Rows[i]["BLOOD"].ToString().Trim() == "1")
                        {
                            ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 3].Text = "★" + ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 3].Text;
                            ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 2, ssPRT_Sheet1.RowCount - 1, 9].BackColor = Color.FromArgb(255, 220, 255);
                            ssPRT_Sheet1.Cells[12, 5].Value = true;
                            ssPRT_Sheet1.Cells[12, 6].Value = false;
                        }

                        byte[] a = System.Text.Encoding.Default.GetBytes(ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 3].Text);
                        int intHeight = Convert.ToInt32(a.Length / 12);

                        a = System.Text.Encoding.Default.GetBytes(ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 4].Text);

                        if (intHeight < Convert.ToInt32(a.Length / 17))
                        {
                            intHeight = Convert.ToInt32(a.Length / 17);
                        }

                        a = System.Text.Encoding.Default.GetBytes(ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 5].Text);

                        if (intHeight < Convert.ToInt32(a.Length / 8))
                        {
                            intHeight = Convert.ToInt32(a.Length / 8);
                        }

                        if (intHeight < VB.Split(ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 8].Text, ComNum.VBLF).Length)
                        {
                            intHeight = VB.Split(ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 8].Text, ComNum.VBLF).Length;
                        }

                        if (intHeight < VB.Split(ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 9].Text, ComNum.VBLF).Length)
                        {
                            intHeight = VB.Split(ssPRT_Sheet1.Cells[ssPRT_Sheet1.RowCount - 1, 9].Text, ComNum.VBLF).Length;
                        }

                        ssPRT_Sheet1.SetRowHeight(ssPRT_Sheet1.RowCount - 1, ComNum.SPDROWHT + (intHeight * 16));
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

        private void SET_DRUGHOI2(string strWRTNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strSAYU1 = "";
            string strSAYU2 = "";
            string strSAYU3 = "";
            string strSAYU4 = "";
            string strSAYU5 = "";
            string strSAYU6 = "";
            string strSAYU7 = "";
            string strSAYU8 = "";
            string strSAYU9 = "";
            string strSAYU10 = "";
            string strSAYU11 = "";
            string strSAYU12 = "";
            string strSAYU13 = "";
            string strSAYU14 = "";
            string strSAYU15 = "";
            string strSAYU16 = "";
            string strSAYU17 = "";
            string strREMARK14 = "";
            string strTemp = "";
            string strTemp2 = "";
            string strJEP1 = "";
            string strJEP2 = "";
            string strJEP3 = "";
            string strJEP4 = "";
            string strJEP5 = "";
            string strJEP6 = "";

            ssPRT2_CLEAR();
            
            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.JDATE, A.BDATE, A.DEPTCODE, A.DRCODE, A.DRSABUN, A.WARDCODE, ";
                SQL = SQL + ComNum.VBLF + "     A.ROOMCODE, A.PANO, A.REMCODE1, A.REMCODE2, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE3, A.REMCODE4, A.REMCODE5, A.REMCODE6, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE7, A.REMCODE8, A.REMCODE9, A.REMCODE10, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE11, A.REMCODE12, A.REMCODE13, A.REMCODE14, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE15, A.REMCODE16, A.REMCODE17, ";
                SQL = SQL + ComNum.VBLF + "     A.HOSP, A.PHAR, B.SNAME, B.SEX, B.JUMIN1, B.JUMIN2, A.DABCODE, ";
                SQL = SQL + ComNum.VBLF + "     A.FASTRETURN, A.RETURNMEMO, A.HDATE, A.DRUGNAME, ";
                SQL = SQL + ComNum.VBLF + "     A.PANO, A.DRUGGIST, A.ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.WRTNO = '" + strWRTNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssPRT2_Sheet1.Cells[1, 4].Text = Convert.ToDateTime(dt.Rows[0]["BDATE"].ToString().Trim()).ToString("yyyy/MM/dd HH:mm");
                    
                    switch (dt.Rows[0]["DABCODE"].ToString().Trim())
                    {
                        case "02": ssPRT2_Sheet1.Cells[1, 4].Text = ssPRT2_Sheet1.Cells[1, 4].Text + ComNum.VBLF + "( 30분 이내 )"; break;
                        case "03": ssPRT2_Sheet1.Cells[1, 4].Text = ssPRT2_Sheet1.Cells[1, 4].Text + ComNum.VBLF + "( 1시간 이내 )"; break;
                        case "05": ssPRT2_Sheet1.Cells[1, 4].Text = ssPRT2_Sheet1.Cells[1, 4].Text + ComNum.VBLF + "( 3시간 이내 )"; break;
                        case "06": ssPRT2_Sheet1.Cells[1, 4].Text = ssPRT2_Sheet1.Cells[1, 4].Text + ComNum.VBLF + "( 금일 이내 )"; break;
                        case "07": ssPRT2_Sheet1.Cells[1, 4].Text = ssPRT2_Sheet1.Cells[1, 4].Text + ComNum.VBLF + "( 48시간 이내 )"; break;
                    }

                    if (dt.Rows[0]["JDATE"].ToString().Trim() != "")
                    {
                        ssPRT2_Sheet1.Cells[1, 8].Text = Convert.ToDateTime(dt.Rows[0]["JDATE"].ToString().Trim()).ToString("yyyy/MM/dd HH:mm");
                    }

                    if (dt.Rows[0]["HDATE"].ToString().Trim() != "")
                    {
                        ssPRT2_Sheet1.Cells[1, 12].Text = Convert.ToDateTime(dt.Rows[0]["HDATE"].ToString().Trim()).ToString("yyyy/MM/dd HH:mm");
                    }

                    ssPRT2_Sheet1.Cells[1, 16].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRUGGIST"].ToString().Trim()) + " 약사";

                    ssPRT2_Sheet1.Cells[2, 4].Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[0]["PANO"].ToString().Trim());
                    ssPRT2_Sheet1.Cells[2, 8].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssPRT2_Sheet1.Cells[2, 12].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                    ssPRT2_Sheet1.Cells[2, 16].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[0]["DEPTCODE"].ToString().Trim());

                    ssPRT2_Sheet1.Cells[3, 4].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, dt.Rows[0]["PANO"].ToString().Trim());
                    ssPRT2_Sheet1.Cells[3, 8].Text = dt.Rows[0]["WARDCODE"].ToString().Trim() + "/" + dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    ssPRT2_Sheet1.Cells[3, 12].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());
                    ssPRT2_Sheet1.Cells[3, 16].Text = clsVbfunc.READ_INSA_BUSE(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());

                    strSAYU1 = dt.Rows[0]["REMCODE1"].ToString().Trim();
                    strSAYU2 = dt.Rows[0]["REMCODE2"].ToString().Trim();
                    strSAYU3 = dt.Rows[0]["REMCODE3"].ToString().Trim();
                    strSAYU4 = dt.Rows[0]["REMCODE4"].ToString().Trim();
                    strSAYU5 = dt.Rows[0]["REMCODE5"].ToString().Trim();
                    strSAYU6 = dt.Rows[0]["REMCODE6"].ToString().Trim();
                    strSAYU7 = dt.Rows[0]["REMCODE7"].ToString().Trim();
                    strSAYU8 = dt.Rows[0]["REMCODE8"].ToString().Trim();
                    strSAYU9 = dt.Rows[0]["REMCODE9"].ToString().Trim();
                    strSAYU10 = dt.Rows[0]["REMCODE10"].ToString().Trim();
                    strSAYU11 = dt.Rows[0]["REMCODE11"].ToString().Trim();
                    strSAYU12 = dt.Rows[0]["REMCODE12"].ToString().Trim();
                    strSAYU13 = dt.Rows[0]["REMCODE13"].ToString().Trim();
                    strSAYU14 = dt.Rows[0]["REMCODE14"].ToString().Trim();
                    strSAYU15 = dt.Rows[0]["REMCODE15"].ToString().Trim();
                    strSAYU16 = dt.Rows[0]["REMCODE16"].ToString().Trim();
                    strSAYU17 = dt.Rows[0]["REMCODE17"].ToString().Trim();


                    strTemp = "";
                    strTemp = strTemp + (strSAYU1 != "" ? strSAYU1 + ", " : "");
                    strTemp = strTemp + (strSAYU1 != "" ? strSAYU1 + ", " : "");
                    strTemp = strTemp + (strSAYU3 != "" ? strSAYU3 + ", " : "");
                    strTemp = strTemp + (strSAYU4 != "" ? strSAYU4 + ", " : "");
                    strTemp = strTemp + (strSAYU5 != "" ? strSAYU5 + ", " : "");
                    strTemp = strTemp + (strSAYU7 != "" ? strSAYU7 + ", " : "");
                    strTemp = strTemp + (strSAYU8 != "" ? strSAYU8 + ", " : "");
                    strTemp = strTemp + (strSAYU9 != "" ? strSAYU9 + ", " : "");
                    strTemp = strTemp + (strSAYU10 != "" ? strSAYU10 + ", " : "");
                    strTemp = strTemp + (strSAYU11 != "" ? strSAYU11 + ", " : "");
                    strTemp = strTemp + (strSAYU12 != "" ? strSAYU12 + ", " : "");
                    strTemp = strTemp + (strSAYU13 != "" ? strSAYU13 + ", " : "");
                    strTemp = strTemp + (strSAYU14 != "" ? strSAYU14 + ", " : "");
                    strTemp = strTemp + (strSAYU15 != "" ? strSAYU15 + ", " : "");
                    strTemp = strTemp + (strSAYU16 != "" ? strSAYU16 + ", " : "");
                    strTemp = strTemp + (strSAYU17 != "" ? strSAYU17 + ", " : "");
                    strTemp = strTemp + (strSAYU6 != "" ? strSAYU6 + ", " : "");

                    if (strTemp != "")
                    {
                        strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2);
                    }

                    ssPRT2_Sheet1.Cells[4, 4].Text = strTemp;
                    ssPRT2_Sheet1.Cells[4, 8].Text = dt.Rows[0]["FASTRETURN"].ToString().Trim();
                    ssPRT2_Sheet1.Cells[4, 14].Text = dt.Rows[0]["HOSP"].ToString().Trim();

                    ssPRT2_Sheet1.Cells[5, 4].Text = dt.Rows[0]["RETURNMEMO"].ToString().Trim();
                    ssPRT2_Sheet1.Cells[5, 14].Text = dt.Rows[0]["PHAR"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                ssPRT2_Sheet1.Cells[6, 6].Value = false;
                ssPRT2_Sheet1.Cells[6, 8].Value = true;

                ssPRT2_Sheet1.Cells[7, 6].Value = false;
                ssPRT2_Sheet1.Cells[7, 8].Value = true;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     REMARK1, REMARK2, REMARK3, REMARK4, ";
                SQL = SQL + ComNum.VBLF + "     REMARK5, REMARK6, REMARK7, REMARK8, ";
                SQL = SQL + ComNum.VBLF + "     REMARK9, REMARK10, REMARK11, REMARK12, ";
                SQL = SQL + ComNum.VBLF + "     REMARK13, BLOOD, ROWID, EDICODE, METFORMIN,";
                SQL = SQL + ComNum.VBLF + "     REMARK14, QTY, NAL, DOSCODE, DECODE(TUYAKGBN, '1', '●', '') AS TUYAKGBN, RP, ";
                SQL = SQL + ComNum.VBLF + "     REMARK15, REMARK16, REMARK17, REMARK18, ";
                SQL = SQL + ComNum.VBLF + "     NOT_SIKBYUL, IMGYN, ROWID  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOISLIP ";
                SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + strWRTNO;
                SQL = SQL + ComNum.VBLF + "ORDER BY ENTDATE ASC, RP ASC, EDICODE ASC";
                
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
                        DRAW_LINE();

                        ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 0].Text = (i + 1).ToString();
                        ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 2].Text = dt.Rows[i]["RP"].ToString().Trim();

                        ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 4].Text = dt.Rows[i]["REMARK1"].ToString().Trim()
                            + (dt.Rows[i]["EDICODE"].ToString().Trim() != "" ? ComNum.VBLF + "(" + dt.Rows[i]["EDICODE"].ToString().Trim() + ")" : "")
                            + (dt.Rows[i]["REMARK7"].ToString().Trim() != "" ? ComNum.VBLF + "(" + dt.Rows[i]["REMARK7"].ToString().Trim() + ")" : "");
                        ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["REMARK2"].ToString().Trim();

                        ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 6].Text = dt.Rows[i]["REMARK6"].ToString().Trim();
                        ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 8].Text = "앞) " +dt.Rows[i]["REMARK9"].ToString().Trim();

                        ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 6].Text = "제형:" +dt.Rows[i]["REMARK8"].ToString().Trim();
                        ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 8].Text = "뒤) " + dt.Rows[i]["REMARK10"].ToString().Trim();

                        ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 10].Text = dt.Rows[i]["REMARK3"].ToString().Trim();

                        
                        if (VB.Val(dt.Rows[i]["QTY"].ToString().Trim()) > 0)
                        {
                            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 12].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        }

                        if (VB.Val(dt.Rows[i]["NAL"].ToString().Trim()) > 0)
                        {
                            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 14].Text = "(" + dt.Rows[i]["NAL"].ToString().Trim() + "일)";
                        }

                        strREMARK14 = dt.Rows[i]["REMARK14"].ToString().Trim();

                        if (VB.Val(strREMARK14) > 0)
                        {
                            strREMARK14 = "(수량:" + strREMARK14 + ")";
                        }

                        ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 12].Text = READ_DOSNAME(clsDB.DbCon, dt.Rows[i]["DOSCODE"].ToString().Trim()) + ComNum.VBLF + strREMARK14;


                        strJEP1 = dt.Rows[i]["REMARK5"].ToString().Trim();
                        strJEP2 = dt.Rows[i]["REMARK12"].ToString().Trim();
                        strJEP3 = dt.Rows[i]["REMARK13"].ToString().Trim();
                        if (strJEP1 != "") { strJEP1 = ComNum.VBLF + strJEP1 + READ_DRUGNAME(strJEP1); }
                        if (strJEP2 != "") { strJEP2 = ComNum.VBLF + strJEP2 + READ_DRUGNAME(strJEP2); }
                        if (strJEP3 != "") { strJEP3 = ComNum.VBLF + strJEP3 + READ_DRUGNAME(strJEP3); }

                        strTemp2 = "";
                        strTemp2 += READ_USED_GUBUN(dt.Rows[i]["REMARK11"].ToString().Trim());
                        strTemp2 += ((strJEP1 + strJEP2 + strJEP3) != "" ? strJEP1 + strJEP2 + strJEP3 : "");
                        
                        if (dt.Rows[i]["REMARK15"].ToString().Trim() != "")
                        {
                            strJEP4 = dt.Rows[i]["REMARK16"].ToString().Trim();
                            strJEP5 = dt.Rows[i]["REMARK17"].ToString().Trim();
                            strJEP6 = dt.Rows[i]["REMARK18"].ToString().Trim();
                            if (strJEP4 != "") { strJEP4 = ComNum.VBLF + strJEP4 + READ_DRUGNAME(strJEP4); }
                            if (strJEP5 != "") { strJEP5 = ComNum.VBLF + strJEP5 + READ_DRUGNAME(strJEP5); }
                            if (strJEP6 != "") { strJEP6 = ComNum.VBLF + strJEP6 + READ_DRUGNAME(strJEP6); }

                            strTemp2 += ComNum.VBLF + "----------------------------------------" + ComNum.VBLF;
                            strTemp2 += READ_USED_GUBUN(dt.Rows[i]["REMARK15"].ToString().Trim());
                            strTemp2 += ((strJEP4 + strJEP5 + strJEP6) != "" ? strJEP4 + strJEP5 + strJEP6 : "");
                        }
                        ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 14].Text = strTemp2;


                        

                        //ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 14].Text = READ_USED_GUBUN(dt.Rows[i]["REMARK11"].ToString().Trim())
                        //    + ((strJEP1 + strJEP2 + strJEP3) != "" ? ComNum.VBLF + strJEP1 + strJEP2 + strJEP3 : "");

                        ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 18].Text = dt.Rows[i]["TUYAKGBN"].ToString().Trim();
                        ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 20].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        //항혈전제 표시
                        if (dt.Rows[i]["BLOOD"].ToString().Trim() == "1")
                        {
                            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 4].Text = "★" + ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 4].Text;

                            ssPRT2_Sheet1.Cells[6, 6].Value = true;
                            ssPRT2_Sheet1.Cells[6, 8].Value = false;

                            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 0, ssPRT2_Sheet1.RowCount - 2, ssPRT2_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(10, 10, 220);
                            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0, ssPRT2_Sheet1.RowCount - 1, ssPRT2_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(10, 10, 220);
                        }

                        //METFORMIN
                        if (dt.Rows[i]["METFORMIN"].ToString().Trim() == "1")
                        {
                            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 4].Text = "▣" + ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 4].Text;

                            ssPRT2_Sheet1.Cells[7, 6].Value = true;
                            ssPRT2_Sheet1.Cells[7, 8].Value = false;

                            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 0, ssPRT2_Sheet1.RowCount - 2, ssPRT2_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(220, 10, 10);
                            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0, ssPRT2_Sheet1.RowCount - 1, ssPRT2_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(220, 10, 10);
                        }

                        byte[] a = System.Text.Encoding.Default.GetBytes(ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 4].Text);
                        int intHeight = Convert.ToInt32(a.Length / 28);

                        a = System.Text.Encoding.Default.GetBytes(ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 12].Text);

                        if (intHeight < Convert.ToInt32(a.Length / 23))
                        {
                            intHeight = Convert.ToInt32(a.Length / 23);

                            if (intHeight > VB.Split(ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 12].Text, ComNum.VBLF).Length)
                            {
                                intHeight = VB.Split(ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 12].Text, ComNum.VBLF).Length;
                            }
                        }

                        a = System.Text.Encoding.Default.GetBytes(ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 16].Text);

                        if (intHeight < Convert.ToInt32(a.Length / 9))
                        {
                            intHeight = Convert.ToInt32(a.Length / 9);
                        }

                        ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 1, ComNum.SPDROWHT + (intHeight * 16));
                    }
                }

                dt.Dispose();
                dt = null;

                DRAW_BOTTOM();
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

        private void SET_DRUGHOI3(string strWRTNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strFDRUGCD = "";
            string strSAYU1 = "";
            string strSAYU2 = "";
            string strSAYU3 = "";
            string strSAYU4 = "";
            string strSAYU5 = "";
            string strSAYU6 = "";
            string strSAYU7 = "";
            string strSAYU8 = "";
            string strSAYU9 = "";
            string strSAYU10 = "";
            string strSAYU11 = "";
            string strSAYU12 = "";
            string strSAYU13 = "";
            string strSAYU14 = "";
            string strSAYU15 = "";
            string strSAYU16 = "";
            string strSAYU17 = "";
            string strREMARK14 = "";
            string strTemp = "";
            string strTemp2 = "";
            string strJEP1 = "";
            string strJEP2 = "";
            string strJEP3 = "";

            string strJEP4 = "";
            string strJEP5 = "";
            string strJEP6 = "";

            ssPRT3_CLEAR();

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.JDATE, A.BDATE, A.DEPTCODE, A.DRCODE, A.DRSABUN, A.WARDCODE, ";
                SQL = SQL + ComNum.VBLF + "     A.ROOMCODE, A.PANO, A.REMCODE1, A.REMCODE2, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE3, A.REMCODE4, A.REMCODE5, A.REMCODE6, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE7, A.REMCODE8, A.REMCODE9, A.REMCODE10, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE11, A.REMCODE12, A.REMCODE13, A.REMCODE14, ";
                SQL = SQL + ComNum.VBLF + "     A.REMCODE15, A.REMCODE16, A.REMCODE17, ";
                SQL = SQL + ComNum.VBLF + "     A.HOSP, A.PHAR, B.SNAME, B.SEX, B.JUMIN1, B.JUMIN2, A.DABCODE, ";
                SQL = SQL + ComNum.VBLF + "     A.FASTRETURN, A.RETURNMEMO, A.HDATE, A.DRUGNAME, ";
                SQL = SQL + ComNum.VBLF + "     A.PANO, A.DRUGGIST, A.ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIMST A, " + ComNum.DB_PMPA + "BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.WRTNO = '" + strWRTNO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ssPRT3_Sheet1.Cells[3, 3].Text = Convert.ToDateTime(dt.Rows[0]["BDATE"].ToString().Trim()).ToString("yyyy/MM/dd HH:mm");

                    switch (dt.Rows[0]["DABCODE"].ToString().Trim())
                    {
                        case "02": ssPRT3_Sheet1.Cells[4, 3].Text = "( 30분 이내 )"; break;
                        case "03": ssPRT3_Sheet1.Cells[4, 3].Text = "( 1시간 이내 )"; break;
                        case "05": ssPRT3_Sheet1.Cells[4, 3].Text = "( 3시간 이내 )"; break;
                        case "06": ssPRT3_Sheet1.Cells[4, 3].Text = "( 금일 이내 )"; break;
                        case "07": ssPRT3_Sheet1.Cells[4, 3].Text = "( 48시간 이내 )"; break;
                    }

                    //접수일시
                    if (dt.Rows[0]["JDATE"].ToString().Trim() != "")
                    {
                        ssPRT3_Sheet1.Cells[3, 13].Text = Convert.ToDateTime(dt.Rows[0]["JDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        ssPRT3_Sheet1.Cells[4, 13].Text = Convert.ToDateTime(dt.Rows[0]["JDATE"].ToString().Trim()).ToString("HH:mm");
                    }

                    //회신일자
                    if (dt.Rows[0]["HDATE"].ToString().Trim() != "")
                    {
                        ssPRT3_Sheet1.Cells[5, 3].Text = Convert.ToDateTime(dt.Rows[0]["HDATE"].ToString().Trim()).ToString("yyyy-MM-dd");
                        ssPRT3_Sheet1.Cells[6, 3].Text = Convert.ToDateTime(dt.Rows[0]["HDATE"].ToString().Trim()).ToString("HH:mm");
                    }

                    //회신자
                    ssPRT3_Sheet1.Cells[7, 3].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRUGGIST"].ToString().Trim()) + " 약사";

                    //환자정보
                    ssPRT3_Sheet1.Cells[1, 3].Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[0]["PANO"].ToString().Trim());
                    ssPRT3_Sheet1.Cells[1, 5].Text = dt.Rows[0]["PANO"].ToString().Trim();
                    ssPRT3_Sheet1.Cells[1, 8].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, dt.Rows[0]["DEPTCODE"].ToString().Trim());

                    ssPRT3_Sheet1.Cells[2, 3].Text = dt.Rows[0]["SEX"].ToString().Trim() + "/" + clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, dt.Rows[0]["PANO"].ToString().Trim());
                    ssPRT3_Sheet1.Cells[2, 5].Text = dt.Rows[0]["WARDCODE"].ToString().Trim() + "/" + dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    ssPRT3_Sheet1.Cells[2, 8].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());

                    //의뢰자
                    ssPRT3_Sheet1.Cells[3, 10].Text = clsVbfunc.READ_INSA_BUSE(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());
                    ssPRT3_Sheet1.Cells[3, 11].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());

                    strSAYU1 = dt.Rows[0]["REMCODE1"].ToString().Trim();
                    strSAYU2 = dt.Rows[0]["REMCODE2"].ToString().Trim();
                    strSAYU3 = dt.Rows[0]["REMCODE3"].ToString().Trim();
                    strSAYU4 = dt.Rows[0]["REMCODE4"].ToString().Trim();
                    strSAYU5 = dt.Rows[0]["REMCODE5"].ToString().Trim();
                    strSAYU6 = dt.Rows[0]["REMCODE6"].ToString().Trim();
                    strSAYU7 = dt.Rows[0]["REMCODE7"].ToString().Trim();
                    strSAYU8 = dt.Rows[0]["REMCODE8"].ToString().Trim();
                    strSAYU9 = dt.Rows[0]["REMCODE9"].ToString().Trim();
                    strSAYU10 = dt.Rows[0]["REMCODE10"].ToString().Trim();
                    strSAYU11 = dt.Rows[0]["REMCODE11"].ToString().Trim();
                    strSAYU12 = dt.Rows[0]["REMCODE12"].ToString().Trim();
                    strSAYU13 = dt.Rows[0]["REMCODE13"].ToString().Trim();
                    strSAYU14 = dt.Rows[0]["REMCODE14"].ToString().Trim();
                    strSAYU15 = dt.Rows[0]["REMCODE15"].ToString().Trim();
                    strSAYU16 = dt.Rows[0]["REMCODE16"].ToString().Trim();
                    strSAYU17 = dt.Rows[0]["REMCODE17"].ToString().Trim();


                    strTemp = "";
                    strTemp = strTemp + (strSAYU1 != "" ? strSAYU1 + ", " : "");
                    strTemp = strTemp + (strSAYU1 != "" ? strSAYU1 + ", " : "");
                    strTemp = strTemp + (strSAYU3 != "" ? strSAYU3 + ", " : "");
                    strTemp = strTemp + (strSAYU4 != "" ? strSAYU4 + ", " : "");
                    strTemp = strTemp + (strSAYU5 != "" ? strSAYU5 + ", " : "");
                    strTemp = strTemp + (strSAYU7 != "" ? strSAYU7 + ", " : "");
                    strTemp = strTemp + (strSAYU8 != "" ? strSAYU8 + ", " : "");
                    strTemp = strTemp + (strSAYU9 != "" ? strSAYU9 + ", " : "");
                    strTemp = strTemp + (strSAYU10 != "" ? strSAYU10 + ", " : "");
                    strTemp = strTemp + (strSAYU11 != "" ? strSAYU11 + ", " : "");
                    strTemp = strTemp + (strSAYU12 != "" ? strSAYU12 + ", " : "");
                    strTemp = strTemp + (strSAYU13 != "" ? strSAYU13 + ", " : "");
                    strTemp = strTemp + (strSAYU14 != "" ? strSAYU14 + ", " : "");
                    strTemp = strTemp + (strSAYU15 != "" ? strSAYU15 + ", " : "");
                    strTemp = strTemp + (strSAYU16 != "" ? strSAYU16 + ", " : "");
                    strTemp = strTemp + (strSAYU17 != "" ? strSAYU17 + ", " : "");
                    strTemp = strTemp + (strSAYU6 != "" ? strSAYU6 + ", " : "");

                    if (strTemp != "")
                    {
                        strTemp = VB.Mid(strTemp, 1, strTemp.Length - 2);
                    }

                    //복용사유
                    ssPRT3_Sheet1.Cells[1, 10].Text = strTemp;
                    //긴급요청사유
                    ssPRT3_Sheet1.Cells[3, 7].Text = dt.Rows[0]["FASTRETURN"].ToString().Trim();
                    //처방병원
                    ssPRT3_Sheet1.Cells[2, 10].Text = dt.Rows[0]["HOSP"].ToString().Trim();
                    //회신자 전달사항
                    ssPRT3_Sheet1.Cells[5, 5].Text = dt.Rows[0]["RETURNMEMO"].ToString().Trim();
                    //조제약국
                    ssPRT3_Sheet1.Cells[2, 13].Text = dt.Rows[0]["PHAR"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                ssPRT3_Sheet1.Cells[8, 5].Value = false;
                ssPRT3_Sheet1.Cells[8, 6].Value = true;

                ssPRT3_Sheet1.Cells[9, 5].Value = false;
                ssPRT3_Sheet1.Cells[9, 6].Value = true;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     REMARK1, REMARK2, REMARK3, REMARK4, ";
                SQL = SQL + ComNum.VBLF + "     REMARK5, REMARK6, REMARK7, REMARK8, ";
                SQL = SQL + ComNum.VBLF + "     REMARK9, REMARK10, REMARK11, REMARK12, ";
                SQL = SQL + ComNum.VBLF + "     REMARK13, BLOOD, ROWID, EDICODE, METFORMIN,";
                SQL = SQL + ComNum.VBLF + "     REMARK14, QTY, NAL, DOSCODE, DECODE(TUYAKGBN, '1', '●', '') AS TUYAKGBN, RP, ";
                SQL = SQL + ComNum.VBLF + "     REMARK15, REMARK16, REMARK17, REMARK18, ";
                SQL = SQL + ComNum.VBLF + "     NOT_SIKBYUL, IMGYN, ROWID  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOISLIP ";
                SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + strWRTNO;
                SQL = SQL + ComNum.VBLF + "ORDER BY ENTDATE ASC, RP ASC, EDICODE ASC";

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
                        DRAW_LINE3();

                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 0].Text = (i + 1).ToString();
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 1].Text = dt.Rows[i]["RP"].ToString().Trim();

                        //이미지
                        if (dt.Rows[i]["IMGYN"].ToString().Trim() != "1")
                        {
                            strFDRUGCD = READ_FDRUGCD(dt.Rows[i]["EDICODE"].ToString().Trim());
                            if (strFDRUGCD != "")
                            {
                                GetDrugInfoImg(strFDRUGCD, ssPRT3_Sheet1.RowCount - 2, 2);
                            }
                        }

                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Text = dt.Rows[i]["REMARK1"].ToString().Trim()
                            + (dt.Rows[i]["EDICODE"].ToString().Trim() != "" ? ComNum.VBLF + "(" + dt.Rows[i]["EDICODE"].ToString().Trim() + ")" : "")
                            + (dt.Rows[i]["REMARK7"].ToString().Trim() != "" ? ComNum.VBLF + "(" + dt.Rows[i]["REMARK7"].ToString().Trim() + ")" : "");
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["REMARK2"].ToString().Trim();

                        //성상
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 4].Text = dt.Rows[i]["REMARK6"].ToString().Trim();
                        //제형
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 4].Text = "제형:" + dt.Rows[i]["REMARK8"].ToString().Trim();

                        //식별 (앞)
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 2].Text = "앞) " + dt.Rows[i]["REMARK9"].ToString().Trim();

                        //식별 (뒤)
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 3].Text = "뒤) " + dt.Rows[i]["REMARK10"].ToString().Trim();

                        //효능/효과
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 8].Text = dt.Rows[i]["REMARK3"].ToString().Trim();

                        //일투량
                        if (VB.Val(dt.Rows[i]["QTY"].ToString().Trim()) > 0)
                        {
                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 9].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        }

                        //if (VB.Val(dt.Rows[i]["NAL"].ToString().Trim()) > 0)
                        //{
                        //    ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 14].Text = "(" + dt.Rows[i]["NAL"].ToString().Trim() + "일)";
                        //}

                        strREMARK14 = dt.Rows[i]["REMARK14"].ToString().Trim();

                        if (VB.Val(strREMARK14) > 0)
                        {
                            strREMARK14 = "(수량:" + strREMARK14 + ")";
                        }

                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 9].Text = READ_DOSNAME(clsDB.DbCon, dt.Rows[i]["DOSCODE"].ToString().Trim()) + ComNum.VBLF + strREMARK14;

                        strJEP1 = dt.Rows[i]["REMARK5"].ToString().Trim();
                        strJEP2 = dt.Rows[i]["REMARK12"].ToString().Trim();
                        strJEP3 = dt.Rows[i]["REMARK13"].ToString().Trim();
                        if (strJEP1 != "") { strJEP1 = ComNum.VBLF + strJEP1 + READ_DRUGNAME(strJEP1); }
                        if (strJEP2 != "") { strJEP2 = ComNum.VBLF + strJEP2 + READ_DRUGNAME(strJEP2); }
                        if (strJEP3 != "") { strJEP3 = ComNum.VBLF + strJEP3 + READ_DRUGNAME(strJEP3); }

                        strTemp2 = "";
                        strTemp2 += READ_USED_GUBUN(dt.Rows[i]["REMARK11"].ToString().Trim());
                        strTemp2 += ((strJEP1 + strJEP2 + strJEP3) != "" ? strJEP1 + strJEP2 + strJEP3 : "");

                        //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].Text = READ_USED_GUBUN(dt.Rows[i]["REMARK11"].ToString().Trim());
                        //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 11].Text = ((strJEP1 + strJEP2 + strJEP3) != "" ? strJEP1 + strJEP2 + strJEP3 : "");

                        if (dt.Rows[i]["REMARK15"].ToString().Trim() != "")
                        {
                            strJEP4 = dt.Rows[i]["REMARK16"].ToString().Trim();
                            strJEP5 = dt.Rows[i]["REMARK17"].ToString().Trim();
                            strJEP6 = dt.Rows[i]["REMARK18"].ToString().Trim();
                            if (strJEP4 != "") { strJEP4 = ComNum.VBLF + strJEP4 + READ_DRUGNAME(strJEP4); }
                            if (strJEP5 != "") { strJEP5 = ComNum.VBLF + strJEP5 + READ_DRUGNAME(strJEP5); }
                            if (strJEP6 != "") { strJEP6 = ComNum.VBLF + strJEP6 + READ_DRUGNAME(strJEP6); }

                            strTemp2 += ComNum.VBLF + "----------------------------------------" + ComNum.VBLF;
                            strTemp2 += READ_USED_GUBUN(dt.Rows[i]["REMARK15"].ToString().Trim());
                            strTemp2 += ((strJEP4 + strJEP5 + strJEP6) != "" ? strJEP4 + strJEP5 + strJEP6 : "");
                        }
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].Text = strTemp2;

                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 13].Text = dt.Rows[i]["TUYAKGBN"].ToString().Trim();
                        ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 14].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        //항혈전제 표시
                        if (dt.Rows[i]["BLOOD"].ToString().Trim() == "1")
                        {
                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Text = "★" + ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Text;

                            ssPRT3_Sheet1.Cells[8, 5].Value = true;
                            ssPRT3_Sheet1.Cells[8, 6].Value = false;

                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 0, ssPRT3_Sheet1.RowCount - 2, ssPRT3_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(10, 10, 220);
                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0, ssPRT3_Sheet1.RowCount - 1, ssPRT3_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(10, 10, 220);
                        }

                        //METFORMIN
                        if (dt.Rows[i]["METFORMIN"].ToString().Trim() == "1")
                        {
                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Text = "▣" + ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Text;

                            ssPRT3_Sheet1.Cells[9, 5].Value = true;
                            ssPRT3_Sheet1.Cells[9, 6].Value = false;

                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 0, ssPRT3_Sheet1.RowCount - 2, ssPRT3_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(220, 10, 10);
                            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0, ssPRT3_Sheet1.RowCount - 1, ssPRT3_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(220, 10, 10);
                        }


                        byte[] a = System.Text.Encoding.Default.GetBytes(ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 5].Text);
                        int intHeight = Convert.ToInt32(a.Length / 18);

                        if (intHeight > 2)
                        {
                            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT + (intHeight * 16));
                        }

                        //a = System.Text.Encoding.Default.GetBytes(ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 9].Text);

                        //if (intHeight < Convert.ToInt32(a.Length / 23))
                        //{
                        //    intHeight = Convert.ToInt32(a.Length / 23);

                        //    if (intHeight > VB.Split(ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 9].Text, ComNum.VBLF).Length)
                        //    {
                        //        intHeight = VB.Split(ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 9].Text, ComNum.VBLF).Length;
                        //    }
                        //}

                        //ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT + (intHeight * 20));

                        a = System.Text.Encoding.Default.GetBytes(ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].Text);
                        intHeight = Convert.ToInt32(a.Length / 18);

                        if (intHeight > 2)
                        {
                            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 2, ComNum.SPDROWHT + (intHeight * 16));
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                DRAW_BOTTOM3();
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

        private void DRAW_LINE()
        {
            FarPoint.Win.ComplexBorder BorderThreeThin = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderTwoThin = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderRightDashed = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dashed, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderBottomDashed = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dashed, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderTwoDashed = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dashed, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dashed, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            
            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 2;
            ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 2, 35);
            ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 1, 35);

            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 0].RowSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 0].ColumnSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 0].Border = BorderThreeThin;

            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 2].RowSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 2].ColumnSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 2].Border = BorderTwoThin;

            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 4].ColumnSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 4].Border = BorderBottomDashed;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 4].Font = new Font("맑은 고딕", 8F);

            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 4].ColumnSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 4].Border = BorderTwoThin;

            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 6].ColumnSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 6].Border = BorderTwoDashed;

            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 6].ColumnSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 6].Border = BorderRightDashed;

            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 8].ColumnSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 8].Border = BorderBottomDashed;

            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 8].ColumnSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 8].Border = BorderTwoThin;

            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 10].RowSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 10].ColumnSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 10].Border = BorderTwoThin;

            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 12].ColumnSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 12].Border = BorderTwoDashed;

            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 12].ColumnSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 12].Border = BorderRightDashed;

            //ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 14].ColumnSpan = 2;

            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 14].RowSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 14].ColumnSpan = 4;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 14].Border = BorderTwoThin;
            
            //ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 12].ColumnSpan = 4;
            //ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 12].Border = BorderTwoThin;
            //ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 12].Font = new Font("맑은 고딕", 8F);
                        
            
            //ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 15].Border = BorderBottomDashed;

            //ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 15].ColumnSpan = 3;
            //ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 15].Border = BorderTwoThin;

            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 18].RowSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 18].ColumnSpan = 2;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 2, 18].Border = BorderTwoThin;
        }

        private void DRAW_BOTTOM()
        {
            FarPoint.Win.ComplexBorder BorderWhite = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderTop = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderMid = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderBottom = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0, ssPRT2_Sheet1.RowCount - 1, ssPRT2_Sheet1.ColumnCount - 1].Border = BorderWhite;

            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 1, ComNum.SPDROWHT + 3);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Border = BorderTop;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Text = "[회신서 작성 및 지참약 투여에 대한 참고사항]";

            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Border = BorderMid;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Text = VB.Space(10) + "1. 원내 사용여부 및 대체약 정보에 '◎원내-동일성분, 제형다름(약동학적 차이 있음)'으로 표시된 경우,";

            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Border = BorderMid;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Text = VB.Space(13) + "대체약 처방 시 용량, 용법을 신중히 고려하시기 바랍니다.";

            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Border = BorderMid;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Text = VB.Space(10) + "2. 다음과 같은 경우 입원 시 지참약의 투여가 불가능합니다.";

            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Border = BorderMid;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Text = VB.Space(13) + "1) 1회분 포장으로 되어 있는 2종 이상의 약품(No. 우측의 그룹 표시가 같음) 중 일부 투여";

            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Border = BorderMid;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Text = VB.Space(13) + "2) 식별불가능한 약품 및 식별불가능한 약품과 함께 1회분 포장으로 되어 있는 약품 전체";

            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Border = BorderBottom;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Text = VB.Space(13) + "3) 타의료기관에서 처방된 마약";

            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 1, 5);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0, ssPRT2_Sheet1.RowCount - 1, ssPRT2_Sheet1.ColumnCount - 1].Border = BorderWhite;

            ssPRT2_Sheet1.RowCount = ssPRT2_Sheet1.RowCount + 1;
            ssPRT2_Sheet1.SetRowHeight(ssPRT2_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Border = BorderWhite;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 10F);
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT2_Sheet1.Cells[ssPRT2_Sheet1.RowCount - 1, 0].Text = "포항성모병원 약제팀";
        }

        private void DRAW_LINE3()
        {
            FarPoint.Win.ComplexBorder BorderThreeThin = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderTwoThin = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderRightDashed = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dashed, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderBottomDashed = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dashed, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderTwoDashed = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dashed, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.Dashed, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            FarPoint.Win.Spread.CellType.TextCellType TEXTTYPE = new FarPoint.Win.Spread.CellType.TextCellType();
            TEXTTYPE.Multiline = true;
            TEXTTYPE.WordWrap = true;
            TEXTTYPE.MaxLength = 2000;

            FarPoint.Win.Spread.CellType.ImageCellType IMGTYPE = new FarPoint.Win.Spread.CellType.ImageCellType();
            IMGTYPE.Style = FarPoint.Win.RenderStyle.Stretch;

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 2;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 2, 80);
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, 40);

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 0].RowSpan = 2;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 0].Border = BorderThreeThin;

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 1].RowSpan = 2;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 1].Border = BorderTwoThin;

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 2].ColumnSpan = 2;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 2].CellType = IMGTYPE;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 2].Border = BorderTwoDashed;

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 2].Border = BorderRightDashed;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 3].Border = BorderRightDashed;

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 4].Border = BorderBottomDashed;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 4].Border = BorderTwoThin;


            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].ColumnSpan = 3;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Border = BorderTwoDashed;
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 5].Font = new Font("맑은 고딕", 9F);

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 5].ColumnSpan = 3;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 5].Border = BorderRightDashed;


            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 8].RowSpan = 2;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 8].Border = BorderTwoThin;


            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 9].Border = BorderBottomDashed;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 9].Border = BorderTwoThin;


            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].RowSpan = 2;
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].Border = BorderTwoDashed;

            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].ColumnSpan = 3;
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 11].Border = BorderTwoDashed;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].RowSpan = 2;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 10].Border = BorderRightDashed;


            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 13].RowSpan = 2;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 2, 13].Border = BorderTwoThin;

        }

        private void DRAW_BOTTOM3()
        {
            FarPoint.Win.ComplexBorder BorderWhite = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderTop = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderMid = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder BorderBottom = new FarPoint.Win.ComplexBorder(
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.White),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Black),
                new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0, ssPRT3_Sheet1.RowCount - 1, ssPRT3_Sheet1.ColumnCount - 1].Border = BorderWhite;

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT + 3);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderTop;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = "[회신서 작성 및 지참약 투여에 대한 참고사항]";

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderMid;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = VB.Space(10) + "1. 원내 사용여부 및 대체약 정보에 '◎원내-동일성분, 제형다름(약동학적 차이 있음)'으로 표시된 경우, 대체약 처방 시 용량, 용법을 신중히 고려하시기 바랍니다.";

            //ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            //ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderMid;
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            //ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = VB.Space(13) + "대체약 처방 시 용량, 용법을 신중히 고려하시기 바랍니다.";

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderMid;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = VB.Space(10) + "2. 다음과 같은 경우 입원 시 지참약의 투여가 불가능합니다.";

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderMid;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = VB.Space(13) + "1) 1회분 포장으로 되어 있는 2종 이상의 약품(No. 우측의 그룹 표시가 같음) 중 일부 투여";

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderMid;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = VB.Space(13) + "2) 식별불가능한 약품 및 식별불가능한 약품과 함께 1회분 포장으로 되어 있는 약품 전체";

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderBottom;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 9F);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = VB.Space(13) + "3) 타의료기관에서 처방된 마약";

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, 5);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0, ssPRT3_Sheet1.RowCount - 1, ssPRT3_Sheet1.ColumnCount - 1].Border = BorderWhite;

            ssPRT3_Sheet1.RowCount = ssPRT3_Sheet1.RowCount + 1;
            ssPRT3_Sheet1.SetRowHeight(ssPRT3_Sheet1.RowCount - 1, ComNum.SPDROWHT);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].ColumnSpan = 20;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Border = BorderWhite;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Font = new Font("맑은 고딕", 10F);
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssPRT3_Sheet1.Cells[ssPRT3_Sheet1.RowCount - 1, 0].Text = "포항성모병원 약제팀";
        }

        private bool READ_HOIMST(string strWRTNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            bool rtnVal = false;

            if (strWRTNO == "") { return rtnVal; }

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE1, TO_CHAR(BDATE, 'HH24:MI') AS BDATE2, PANO, DEPTCODE, DRNAME, ";
                SQL = SQL + ComNum.VBLF + "     ROOMCODE, HOSP, PHAR, IPDOPD, ";
                SQL = SQL + ComNum.VBLF + "     DRSABUN, DRCODE, WARDCODE, HOSPCODE, ";
                SQL = SQL + ComNum.VBLF + "     DABCODE, WRTNO, REMCODE1, REMCODE2, ";
                SQL = SQL + ComNum.VBLF + "     REMCODE3, REMCODE4, REMCODE5, REMCODE6, ";
                SQL = SQL + ComNum.VBLF + "     REMCODE7, REMCODE8, REMCODE9, REMCODE10, ";
                SQL = SQL + ComNum.VBLF + "     REMCODE11, REMCODE12, REMCODE13, REMCODE14, ";
                SQL = SQL + ComNum.VBLF + "     REMCODE15, REMCODE16, REMCODE17, ";
                SQL = SQL + ComNum.VBLF + "     FASTRETURN, TO_CHAR(HDATE,'YYYY-MM-DD') AS HDATE1, TO_CHAR(HDATE,'HH24:MI') AS HDATE2, DRUGGIST, ";
                SQL = SQL + ComNum.VBLF + "     BLOODHISTORY, DRUGQTY, NOMATCH ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIMST ";
                SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = '" + strWRTNO + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                if (dt.Rows[0]["NOMATCH"].ToString().Trim() == "1")
                {
                    panNoMatch.Visible = true;
                }

                txtPano.Text = dt.Rows[0]["PANO"].ToString().Trim();
                lblPaName.Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[0]["PANO"].ToString().Trim());

                ComFunc.ComboFind(cboDept, "L", 2, dt.Rows[0]["DEPTCODE"].ToString().Trim());
                ComFunc.ComboFind(cboWard, "L", 2, dt.Rows[0]["WARDCODE"].ToString().Trim());

                cboRoom.Items.Add(dt.Rows[0]["ROOMCODE"].ToString().Trim());
                cboRoom.SelectedIndex = 0;

                txtDrSabun.Text = dt.Rows[0]["DRCODE"].ToString().Trim();
                lblPaDrName.Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, txtDrSabun.Text.Trim());

                dtpBDate.Value = Convert.ToDateTime(dt.Rows[0]["BDATE1"].ToString().Trim());
                dtpBTime.Value = Convert.ToDateTime(dt.Rows[0]["BDATE2"].ToString().Trim());
                txtHosp.Text = dt.Rows[0]["HOSP"].ToString().Trim();

                ComFunc.ComboFind(cboHosp, "L", dt.Rows[0]["HOSPCODE"].ToString().Trim().Length, dt.Rows[0]["HOSPCODE"].ToString().Trim());

                txtPhar.Text = dt.Rows[0]["PHAR"].ToString().Trim();

                switch (dt.Rows[0]["DABCODE"].ToString().Trim())
                {
                    case "02": rdoTime0.Checked = true; break;
                    case "03": rdoTime1.Checked = true; break;
                    case "05": rdoTime2.Checked = true; break;
                    case "06": rdoTime3.Checked = true; break;
                    case "07": rdoTime4.Checked = true; break;
                }

                if (dt.Rows[0]["REMCODE1"].ToString().Trim() != "") { chkGubun0.Checked = true; }
                if (dt.Rows[0]["REMCODE2"].ToString().Trim() != "") { chkGubun1.Checked = true; }
                if (dt.Rows[0]["REMCODE3"].ToString().Trim() != "") { chkGubun2.Checked = true; }
                if (dt.Rows[0]["REMCODE4"].ToString().Trim() != "") { chkGubun3.Checked = true; }
                if (dt.Rows[0]["REMCODE5"].ToString().Trim() != "") { chkGubun4.Checked = true; }
                if (dt.Rows[0]["REMCODE6"].ToString().Trim() != "")
                {
                    chkGubun5.Checked = true;
                    txtRemark.Text = dt.Rows[0]["REMCODE6"].ToString().Trim();
                }
                if (dt.Rows[0]["REMCODE7"].ToString().Trim() != "") { chkGubun6.Checked = true; }
                if (dt.Rows[0]["REMCODE8"].ToString().Trim() != "") { chkGubun7.Checked = true; }
                if (dt.Rows[0]["REMCODE9"].ToString().Trim() != "") { chkGubun8.Checked = true; }
                if (dt.Rows[0]["REMCODE10"].ToString().Trim() != "") { chkGubun9.Checked = true; }
                if (dt.Rows[0]["REMCODE11"].ToString().Trim() != "") { chkGubun10.Checked = true; }
                if (dt.Rows[0]["REMCODE12"].ToString().Trim() != "") { chkGubun11.Checked = true; }
                if (dt.Rows[0]["REMCODE13"].ToString().Trim() != "") { chkGubun12.Checked = true; }
                if (dt.Rows[0]["REMCODE14"].ToString().Trim() != "") { chkGubun13.Checked = true; }
                if (dt.Rows[0]["REMCODE15"].ToString().Trim() != "") { chkGubun14.Checked = true; }
                if (dt.Rows[0]["REMCODE16"].ToString().Trim() != "") { chkGubun15.Checked = true; }
                if (dt.Rows[0]["REMCODE17"].ToString().Trim() != "") { chkGubun16.Checked = true; }

                txtFast.Text = dt.Rows[0]["FASTRETURN"].ToString().Trim();

                txtKORNAME.Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRSABUN"].ToString().Trim());
                txtSABUN.Text = dt.Rows[0]["DRSABUN"].ToString().Trim();

                if (dt.Rows[0]["HDATE1"].ToString().Trim() != "")
                {
                    dtpHDate.Value = Convert.ToDateTime(dt.Rows[0]["HDATE1"].ToString().Trim());
                    dtpHTime.Value = Convert.ToDateTime(dt.Rows[0]["HDATE2"].ToString().Trim());
                }
                lblPaDrugName.Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[0]["DRUGGIST"].ToString().Trim());

                switch (dt.Rows[0]["BLOODHISTORY"].ToString().Trim())
                {
                    case "0": rdoAntiBlood1.Checked = true; break;
                    case "1": rdoAntiBlood2.Checked = true; break;
                    case "2": rdoAntiBlood3.Checked = true; break;
                }

                txtQTY.Text = dt.Rows[0]["DRUGQTY"].ToString().Trim();

                dt.Dispose();
                dt = null;

                READ_DRUGQTY(strWRTNO);

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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void READ_DRUGQTY(string strWRTNO)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strGubun = "";

            ssDrugQTY_CLEAR();

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     GUBUN, QTY, MEMO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIQTY_MST ";
                SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + strWRTNO;

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
                        strGubun = dt.Rows[i]["GUBUN"].ToString().Trim();

                        if (strGubun == "6")
                        {
                            ssDrugQTY_Sheet1.Cells[5, 7].Text = dt.Rows[i]["MEMO"].ToString().Trim();
                        }
                        else if (strGubun == "7")
                        {
                            ssDrugQTY_Sheet1.Cells[5, 15].Value = true;
                        }
                        else
                        {
                            ssDrugQTY_Sheet1.Cells[(int)(VB.Val(strGubun)), 2].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     GUBUN, SEQNO, QTY ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIQTY_SLIP ";
                SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + strWRTNO;
                SQL = SQL + ComNum.VBLF + "ORDER BY GUBUN ASC, SEQNO ASC ";

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
                        ssDrugQTY_Sheet1.Cells[(int)(VB.Val(dt.Rows[i]["GUBUN"].ToString().Trim())), (int)(VB.Val(dt.Rows[i]["SEQNO"].ToString().Trim()) + 4)].Text = dt.Rows[i]["QTY"].ToString().Trim();
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

        private string READ_USED_GUBUN(string strGUBUN)
        {
            string rtnVal = "";

            switch (strGUBUN.Trim())
            {
                case "01": rtnVal = "◎원내/외 동종ㆍ동효약 없음"; break;
                case "02": rtnVal = "◎원내-동일약"; break;
                case "03": rtnVal = "◎원내-대체약(성분 동일, 함량 동일)"; break;
                case "04": rtnVal = "◎원내-동종약(성분 동일, 함량 다름)"; break;
                case "05": rtnVal = "◎원내-단일성분만 사용"; break;
                case "06": rtnVal = "◎원외전용-동일약"; break;
                case "07": rtnVal = "◎원외전용-대체약(성분 동일, 함량 동일)"; break;
                case "08": rtnVal = "◎원외전용-동종약(성분 동일, 함량 다름)"; break;
                case "09": rtnVal = "◎원내-효능유사약(성분 다름)"; break;
                case "10": rtnVal = "◎원내-동일성분,제형다름(약동학적 차이 있음)"; break;
            }

            return rtnVal;
        }

        private void ssHIS_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) { return; }

            GstrNoMatch = VB.Left(ssHIS_Sheet1.Cells[e.Row, 3].Text, 5).Trim() == "재의뢰요망" ? "Y" : "";

            if (ssHIS_Sheet1.Cells[e.Row, 5].Text.Trim() == "") { return; }

            GstrWRTNO = ssHIS_Sheet1.Cells[e.Row, 5].Text.Trim();

            if (READ_HOIMST(GstrWRTNO) == false)
            {
                ComFunc.MsgBox("자료가 없습니다.");
                GetData();
            }
            else
            {
                SET_DRUGHOI(GstrWRTNO);
                SET_DRUGHOI2(GstrWRTNO);
                SET_DRUGHOI3(GstrWRTNO);

                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblPaDeptName.Text = VB.Mid(cboDept.Text, 3, 50).Trim();
        }

        private void btnMsgVisible_Click(object sender, EventArgs e)
        {
            panMsg.Visible = true;
        }

        private void btnUnVisible_Click(object sender, EventArgs e)
        {
            panMsg.Visible = false;
        }

        private void btnQtyHelp_Click(object sender, EventArgs e)
        {
            panHelp.Visible = true;
        }

        private void btnHelpClose_Click(object sender, EventArgs e)
        {
            panHelp.Visible = false;
        }

        private void btnOurOrder_Click(object sender, EventArgs e)
        {
            string strPano = "";
            string strWRTNO = "";
            string strBDate = "";

            strPano = txtPano.Text.Trim();
            strWRTNO = GstrWRTNO;
            strBDate = dtpBDate.Value.ToString("yyyy-MM-dd");

            frmSupDrstOutDrugIdentification frm = new frmSupDrstOutDrugIdentification(strPano, strWRTNO, strBDate);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();

            lblOurOrder.Text = Read_DRUG_HOISLIP(clsDB.DbCon, GstrWRTNO);
            cboHosp.SelectedIndex = 1;
            txtHosp.Text = "포항성모병원";
        }

        private void btnCert_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            dtpBDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            dtpBTime.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":"));

            rdoTime0.Checked = false;
            rdoTime1.Checked = false;
            rdoTime2.Checked = false;
            rdoTime3.Checked = false;
            rdoTime4.Checked = false;

            txtFast.Text = "";

            panNoMatch.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (SaveData() == true)
            {
                GetData();
            }
        }

        private bool SaveData()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            string strPano = "";
            string strDept = "";
            string strWard = "";
            string strRoom = "";
            string StrDrCode = "";
            string StrRDate = "";
            string strRTime = "";
            string strHosp = "";
            string strHOSPGBN = "";
            string strPhar = "";
            string strYTIME = "";
            string strSAYU1 = "";
            string strSAYU2 = "";
            string strSAYU3 = "";
            string strSAYU4 = "";
            string strSAYU5 = "";
            string strSAYU6 = "";
            string strSAYU7 = "";
            string strSAYU8 = "";
            string strSAYU9 = "";
            string strSAYU10 = "";
            string strSAYU11 = "";
            string strSAYU12 = "";
            string strSAYU13 = "";
            string strSAYU14 = "";
            string strSAYU15 = "";
            string strSAYU16 = "";
            string strSAYU17 = "";
            string strSIK = "";
            string strBLOODHISTORY = "";
            string strDRUGQTY = "";
            string strFAST = "";
            string strSabun = "";
            string strWRTNO = "";

            strPano = txtPano.Text.Trim();
            strDept = VB.Left(cboDept.Text.Trim(), 2);
            strWard = cboWard.Text.Trim();
            strRoom = cboRoom.Text.Trim();
            StrDrCode = txtDrSabun.Text.Trim();
            
            StrRDate = dtpBDate.Value.ToString("yyyy-MM-dd");
            strRTime = dtpBTime.Value.ToString("HH:mm");
            strHosp = txtHosp.Text.Trim();
            strHOSPGBN = VB.Left(cboHosp.Text.Trim(), 2);
            strPhar = txtPhar.Text.Trim();

            if (rdoTime0.Checked == true) { strYTIME = "02"; }
            else if (rdoTime1.Checked == true) { strYTIME = "03"; }
            else if (rdoTime2.Checked == true) { strYTIME = "05"; }
            else if (rdoTime3.Checked == true) { strYTIME = "06"; }
            else if (rdoTime4.Checked == true) { strYTIME = "07"; }

            if (chkGubun0.Checked == true) { strSAYU1 = "신장질환"; }
            if (chkGubun1.Checked == true) { strSAYU2 = "간질환"; }
            if (chkGubun2.Checked == true) { strSAYU3 = "갑상선질환"; }
            if (chkGubun3.Checked == true) { strSAYU4 = "류마티스/관절염"; }
            if (chkGubun4.Checked == true) { strSAYU5 = "척추관련질환"; }
            if (chkGubun5.Checked == true) { strSAYU6 = txtRemark.Text.Trim(); }
            if (chkGubun6.Checked == true) { strSAYU7 = "고혈압"; }
            if (chkGubun7.Checked == true) { strSAYU8 = "당뇨"; }
            if (chkGubun8.Checked == true) { strSAYU9 = "호흡기질환"; }
            if (chkGubun9.Checked == true) { strSAYU10 = "소화기질환"; }
            if (chkGubun10.Checked == true) { strSAYU11 = "심장질환"; }
            if (chkGubun11.Checked == true) { strSAYU12 = "뇌혈관관련"; }
            if (chkGubun12.Checked == true) { strSAYU13 = "외상질환"; }
            if (chkGubun13.Checked == true) { strSAYU14 = "암질환"; }
            if (chkGubun14.Checked == true) { strSAYU15 = "안과질환"; }
            if (chkGubun15.Checked == true) { strSAYU16 = "피부과질환"; }
            if (chkGubun16.Checked == true) { strSAYU17 = "NP질환"; }
            
            if (chkSik.Checked == true) { strSIK = "Y"; }
            if (chkSik1.Checked == true) { strSIK = "N"; }
            
            strSabun = txtSABUN.Text.Trim();
            strFAST = txtFast.Text.Trim();

            if (rdoAntiBlood1.Checked == true) { strBLOODHISTORY = "0"; }
            else if (rdoAntiBlood2.Checked == true) { strBLOODHISTORY = "1"; }
            else if (rdoAntiBlood3.Checked == true) { strBLOODHISTORY = "2"; }

            strDRUGQTY = txtQTY.Text;

            if (txtPano.Text.Trim() == "")
            {
                ComFunc.MsgBox("등록번호가 공란입니다.");
                return rtnVal;
            }

            if (cboDept.Text.Trim() == "")
            {
                ComFunc.MsgBox("과가 공란입니다.");
                return rtnVal;
            }

            if (cboWard.Text.Trim() == "")
            {
                ComFunc.MsgBox("병동(부서)가 공란입니다.");
                return rtnVal;
            }

            if (cboWard.Text.Trim() != "OPD")
            {
                if (cboRoom.Text.Trim() == "")
                {
                    ComFunc.MsgBox("호실이 공란입니다.");
                    return rtnVal;
                }
            }

            if (txtDrSabun.Text.Trim() == "")
            {
                ComFunc.MsgBox("의사사번이 공란입니다.");
                return rtnVal;
            }

            if (txtHosp.Text.Trim() == "")
            {
                ComFunc.MsgBox("처방병원명이 공란입니다.");
                return rtnVal;
            }

            if (cboHosp.Text.Trim() == "")
            {
                ComFunc.MsgBox("병원분류가 공란입니다.");
                return rtnVal;
            }

            if (txtPhar.Text.Trim() == "")
            {
                ComFunc.MsgBox("조제약국이 공란입니다.");
                return rtnVal;
            }

            if (chkGubun5.Checked == true)
            {
                if (txtRemark.Text.Trim() == "")
                {
                    ComFunc.MsgBox("약물복용사유의 기타란이 공란입니다.");
                    return rtnVal;
                }
            }

            if (txtSABUN.Text.Trim() == "")
            {
                ComFunc.MsgBox("의뢰자가 공란입니다.");
                return rtnVal;
            }


            //if (Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":")) > Convert.ToDateTime("16:00") && rdoTime4.Checked == false)
            //{
            //    ComFunc.MsgBox("의뢰시간이 오후 4시 00분 이후 일 경우 48시간 이내만 선택 가능합니다.");
            //    return rtnVal;
            //}

            if (rdoTime0.Checked == false && rdoTime1.Checked == false && rdoTime2.Checked == false && rdoTime3.Checked == false && rdoTime4.Checked == false)
            {
                ComFunc.MsgBox("요청시간을 선택하여 주시기 바랍니다.");
                return rtnVal;
            }

            //전산업무의뢰서 2019-1298
            if ((VB.Val(VB.Left(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), 4)) >= VB.Val("0000")) && (VB.Val(VB.Left(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), 4)) <= VB.Val("0730")))
            {
                if (rdoTime3.Checked == false)
                {
                    ComFunc.MsgBox("의뢰시간이 00시 00분 부터 07시 30분 까지는 '금일이내'만 선택 가능합니다.");
                    return rtnVal;
                }
            }

            if ((VB.Val(VB.Left(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), 4)) >= VB.Val("1600")) && (VB.Val(VB.Left(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), 4)) < VB.Val("2359")))
            {                
                if (rdoTime4.Checked == false)
                {
                    ComFunc.MsgBox("의뢰시간이 오후 4시 00분 이후 일 경우 48시간 이내만 선택 가능합니다.");
                    return rtnVal;
                }
            }

            if (rdoTime0.Checked == true || rdoTime1.Checked == true || rdoTime2.Checked == true)
            {
                if (txtFast.Text.Trim() == "")
                {
                    ComFunc.MsgBox("요청시간이 30분이내/1시간이내/3시간이내 일 경우 '긴급요청사유'를 반드시 입력하여야 합니다.");
                    return rtnVal;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     (MAX(WRTNO) + 1) AS WRTNO";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIMST ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strWRTNO = dt.Rows[0]["WRTNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (GstrWRTNO == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_HOIMST";
                    SQL = SQL + ComNum.VBLF + "     (BDATE, PANO, DEPTCODE, ROOMCODE, HOSP, PHAR, IPDOPD, ";
                    SQL = SQL + ComNum.VBLF + "     DRSABUN, DRCODE, WARDCODE, HOSPCODE, DABCODE, WRTNO, REMCODE1, ";
                    SQL = SQL + ComNum.VBLF + "     REMCODE2, REMCODE3, REMCODE4, REMCODE5, REMCODE6, VERSION, ";
                    SQL = SQL + ComNum.VBLF + "     FASTRETURN, REMCODE7, REMCODE8, REMCODE9, REMCODE10, REMCODE11, ";
                    SQL = SQL + ComNum.VBLF + "     REMCODE12, REMCODE13, REMCODE14, REMCODE15, REMCODE16, REMCODE17, ";
                    SQL = SQL + ComNum.VBLF + "     BLOODHISTORY, DRUGQTY, DANSOON)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         SYSDATE, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPano + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strDept + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strRoom + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHosp + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strPhar + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + (VB.Left(strWard, 2) == "외래" ? "O" : "I") + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSabun + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + StrDrCode + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + (VB.Left(strWard, 2) == "외래" ? "OPD" : VB.Left(strWard, 2)) + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strHOSPGBN + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strYTIME + "', ";
                    SQL = SQL + ComNum.VBLF + "         " + strWRTNO + ", ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU4 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU5 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU6 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '2', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strFAST + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU7 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU8 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU9 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU10 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU11 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU12 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU13 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU14 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU15 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU16 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSAYU17 + "', ";
                    SQL = SQL + ComNum.VBLF + "         '" + strBLOODHISTORY + "', ";
                    SQL = SQL + ComNum.VBLF + "         " + VB.Val(strDRUGQTY) + ", ";
                    SQL = SQL + ComNum.VBLF + "         '" + strSIK + "' ";
                    SQL = SQL + ComNum.VBLF + "     ) ";
                }
                else
                {
                    SQL = "";
                    SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_HOIMST";
                    SQL = SQL + ComNum.VBLF + "     SET";
                    SQL = SQL + ComNum.VBLF + "         HOSP = '" + strHosp + "', ";
                    SQL = SQL + ComNum.VBLF + "         PHAR = '" + strPhar + "', ";
                    SQL = SQL + ComNum.VBLF + "         HOSPCODE = '" + strHOSPGBN + "', ";
                    //2020-02-04 약제팀 데레사 수녀 요청사항. 의뢰서 작성 당일만 요청시간 변경가능
                    if (VB.Left(ssPRT3_Sheet1.Cells[3, 3].Text.Replace("/", "").Trim(), 8) == ComQuery.CurrentDateTime(clsDB.DbCon, "D"))
                    {
                        SQL = SQL + ComNum.VBLF + "         DABCODE = '" + strYTIME + "', ";
                    }
                    SQL = SQL + ComNum.VBLF + "         REMCODE1 = '" + strSAYU1 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE2 = '" + strSAYU2 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE3 = '" + strSAYU3 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE4 = '" + strSAYU4 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE5 = '" + strSAYU5 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE6 = '" + strSAYU6 + "', ";
                    SQL = SQL + ComNum.VBLF + "         FASTRETURN = '" + strFAST + "',";
                    SQL = SQL + ComNum.VBLF + "         REMCODE7 = '" + strSAYU7 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE8 = '" + strSAYU8 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE9 = '" + strSAYU9 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE10 = '" + strSAYU10 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE11 = '" + strSAYU11 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE12 = '" + strSAYU12 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE13 = '" + strSAYU13 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE14 = '" + strSAYU14 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE15 = '" + strSAYU15 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE16 = '" + strSAYU16 + "', ";
                    SQL = SQL + ComNum.VBLF + "         REMCODE17 = '" + strSAYU17 + "', ";
                    SQL = SQL + ComNum.VBLF + "         BLOODHISTORY = '" + strBLOODHISTORY + "', ";
                    SQL = SQL + ComNum.VBLF + "         DANSOON = '" + strSIK + "', ";

                    if (GstrNoMatch == "Y")
                    {
                        SQL = SQL + ComNum.VBLF + "         DRSABUN = '" + strSabun + "', ";
                        SQL = SQL + ComNum.VBLF + "         BDATE = SYSDATE, ";
                        SQL = SQL + ComNum.VBLF + "         NOMATCH = '0',";
                        SQL = SQL + ComNum.VBLF + "         GUBUN =  NULL,";
                        SQL = SQL + ComNum.VBLF + "         BUN = '0', ";
                    }

                    SQL = SQL + ComNum.VBLF + "         DRUGQTY = " + VB.Val(strDRUGQTY) + " ";
                    SQL = SQL + ComNum.VBLF + "WHERE WRTNO = " + GstrWRTNO;
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (INSERT_DRUGQTY(clsDB.DbCon, (GstrWRTNO == "" ? strWRTNO : GstrWRTNO)) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "";
                SQL = "UPDATE " + ComNum.DB_ERP + "DRUG_HOISLIP_SEND";
                SQL = SQL + ComNum.VBLF + "     SET";
                SQL = SQL + ComNum.VBLF + "         WRTNO = " + (GstrWRTNO == "" ? strWRTNO : GstrWRTNO);
                SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "     AND BDATE = TO_DATE('" + dtpBDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND WRTNO IS NULL ";

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
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

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

                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private bool INSERT_DRUGQTY(PsmhDb pDbCon, string strWRTNO)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;
            int k = 0;
            
            string strGubun = "";
            string strQTY_MST = "";
            string strQTY_SUB = "";
            string strMemo = "";

            try
            {
                SQL = "";
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_HOIQTY_MST ";
                SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + strWRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                SQL = "";
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_HOIQTY_SLIP ";
                SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + strWRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                for (i = 1; i <= 5; i++)
                {
                    switch (i)
                    {
                        case 1: strGubun = "1"; break;
                        case 2: strGubun = "2"; break;
                        case 3: strGubun = "3"; break;
                        case 4: strGubun = "4"; break;
                        case 5: strGubun = "5"; break;
                    }

                    if (VB.Val(ssDrugQTY_Sheet1.Cells[i, 2].Text.Trim()) > 0)
                    {
                        strQTY_MST = ssDrugQTY_Sheet1.Cells[i, 2].Text.Trim();

                        SQL = "";
                        SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_HOIQTY_MST";
                        SQL = SQL + ComNum.VBLF + "     (WRTNO, GUBUN, QTY, MEMO)";
                        SQL = SQL + ComNum.VBLF + "VALUES";
                        SQL = SQL + ComNum.VBLF + "     (";
                        SQL = SQL + ComNum.VBLF + "         " + strWRTNO + ", ";
                        SQL = SQL + ComNum.VBLF + "         '" + strGubun + "', ";
                        SQL = SQL + ComNum.VBLF + "         " + strQTY_MST + ", ";
                        SQL = SQL + ComNum.VBLF + "         '' ";
                        SQL = SQL + ComNum.VBLF + "     )";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return rtnVal;
                        }

                        if (i == 5) { break; }

                        for (k = 5; k <= 17; k++)
                        {
                            if (VB.Val(ssDrugQTY_Sheet1.Cells[i, k].Text.Trim()) > 0)
                            {
                                strQTY_SUB = ssDrugQTY_Sheet1.Cells[i, k].Text.Trim();

                                SQL = "";
                                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_HOIQTY_SLIP";
                                SQL = SQL + ComNum.VBLF + "     (WRTNO, GUBUN, SEQNO, QTY)";
                                SQL = SQL + ComNum.VBLF + "VALUES";
                                SQL = SQL + ComNum.VBLF + "     (";
                                SQL = SQL + ComNum.VBLF + "         " + strWRTNO + ", ";
                                SQL = SQL + ComNum.VBLF + "         '" + strGubun + "', ";
                                SQL = SQL + ComNum.VBLF + "         " + (k - 4) + ", ";
                                SQL = SQL + ComNum.VBLF + "         " + strQTY_SUB;
                                SQL = SQL + ComNum.VBLF + "     )";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                    return rtnVal;
                                }
                            }
                        }
                    }
                }

                strMemo = ssDrugQTY_Sheet1.Cells[5, 7].Text.Trim();

                if (strMemo != "")
                {
                    strGubun = "6";

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_HOIQTY_MST";
                    SQL = SQL + ComNum.VBLF + "     (WRTNO, GUBUN, QTY, MEMO)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         " + strWRTNO + ", ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGubun + "', ";
                    SQL = SQL + ComNum.VBLF + "         1, ";
                    SQL = SQL + ComNum.VBLF + "         '" + strMemo + "' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                }

                if (Convert.ToBoolean(ssDrugQTY_Sheet1.Cells[5, 15].Value) == true)
                {
                    strGubun = "7";

                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_HOIQTY_MST";
                    SQL = SQL + ComNum.VBLF + "     (WRTNO, GUBUN, QTY, MEMO)";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "         " + strWRTNO + ", ";
                    SQL = SQL + ComNum.VBLF + "         '" + strGubun + "', ";
                    SQL = SQL + ComNum.VBLF + "         1, ";
                    SQL = SQL + ComNum.VBLF + "         '처방전' ";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                }

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (GstrWRTNO == "")
            {
                ComFunc.MsgBox("삭제할 식별의뢰서를 선택해주세요.");
                return;
            }

            if (ComFunc.MsgBoxQ("삭제 후 복구는 불가능 합니다. 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                if (DelData() == true)
                {
                    GetData();
                }
            }
        }

        private bool DelData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "INSERT INTO " + ComNum.DB_ERP + "DRUG_HOIMST_HISTORY ";
                SQL = SQL + ComNum.VBLF + "     SELECT * FROM " + ComNum.DB_ERP + "DRUG_HOIMST";
                SQL = SQL + ComNum.VBLF + "         WHERE WRTNO = " + GstrWRTNO;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                SQL = "";
                SQL = "DELETE " + ComNum.DB_ERP + "DRUG_HOIMST ";
                SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + GstrWRTNO;

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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            //string strFont1 = "";
            //string strFoot1 = "";

            //strFont1 = "/fn\"맑은 고딕\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs1";

            //strFoot1 = "/l/f1" + "  ⊙ ◎원내-동일성분, 제형다름(약동학적 차이 있음)" + "/f1/n";
            //strFoot1 = strFoot1 + "/l/f1" + VB.Space(15) + "  => 약동학적 성질을 변화시킨 약품의 약효 및 약동학적 변화는 동일함량의 일반제형 약품과는" + "/f1/n";
            //strFoot1 = strFoot1 + "/l/f1" + VB.Space(15) + "     차이가 있으므로 원내약으로 대체할 경우 용량, 용법 등을 신중히 고려하시기 바랍니다." + "/f1/n";
            //strFoot1 = strFoot1 + "/l/f1" + VB.Space(15) + "  ⊙ 회신일자 : " + dtpHDate.Value.ToString("yyyy-MM-dd") + " " + dtpHTime.Value.ToString("HH:mm") + "                  ⊙  작 성 자 : 약제팀 " + lblPaDrugName.Text + " 약사 " + "/f1";
                        

            ssPRT3_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            //ssPRT3_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssPRT3_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            //ssPRT3_Sheet1.PrintInfo.Footer = strFont1 + strFoot1;
            ssPRT3_Sheet1.PrintInfo.Margin.Top = 20;
            ssPRT3_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssPRT3_Sheet1.PrintInfo.Margin.Footer = 10;
            ssPRT3_Sheet1.PrintInfo.ShowColor = false;
            ssPRT3_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPRT3_Sheet1.PrintInfo.ShowBorder = false;
            ssPRT3_Sheet1.PrintInfo.ShowGrid = false;
            ssPRT3_Sheet1.PrintInfo.ShowShadows = false;
            ssPRT3_Sheet1.PrintInfo.UseMax = true;
            ssPRT3_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPRT3_Sheet1.PrintInfo.UseSmartPrint = false;
            ssPRT3_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssPRT3_Sheet1.PrintInfo.Preview = false;
            ssPRT3.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string READ_DOSNAME(PsmhDb pDbCon, string strSuCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     IDOSNAME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ODOSAGE";
                SQL = SQL + ComNum.VBLF + "     WHERE DOSCODE = '" + strSuCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["IDOSNAME"].ToString().Trim();
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

        /// <summary>
        /// READ_본원약제
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        private string Read_DRUG_HOISLIP(PsmhDb pDbCon, string strWRTNO, string strPano = "")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            if (strWRTNO.Trim() == "") { return rtnVal; }

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     WRTNO ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOISLIP_SEND ";

                if (strWRTNO == "")
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE WSABUN = " + clsType.User.Sabun;
                    SQL = SQL + ComNum.VBLF + "         AND WRTNO IS NULL ";
                    SQL = SQL + ComNum.VBLF + "         AND PANO = '" + strPano.Trim() + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     WHERE WRTNO = " + strWRTNO;
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "본원약품 의뢰 품목수 : " + dt.Rows.Count;
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

        private void Dir_Check(string sDirPath, string sExe = "*.jpg")
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

        private void GetDrugInfoImg(string strDifKey, int iRow, int iCol)
        {
            if (strDifKey == "") { return; }

            Dir_Check(@"c:\cmc\ocsexe\dif");

            string strLocal = "";
            string strPath = "";
            string strHost = "";
            string strImgFileName = "";

            Ftpedt FtpedtX = new Ftpedt();

            if (FtpedtX.FtpConnetBatch("192.168.100.33", "pcnfs", "pcnfs1") == false)
            {
                ComFunc.MsgBox("FTP Server Connect ERROR !!!", "오류");
                return;
            }

            Image img = null;

            strImgFileName = strDifKey + ".jpg";

            strLocal = "c:\\cmc\\ocsexe\\dif\\" + strImgFileName;

            strPath = "/pcnfs/firstdis/" + strImgFileName;
            strHost = "/pcnfs/firstdis";

            FileInfo f = new FileInfo(strLocal);
                        
            try
            {
                ////기존파일 삭제
                if (f.Exists == true)
                {
                    f.Delete();
                }
                if (FtpedtX.FtpDownloadEx("192.168.100.33", "pcnfs", "pcnfs1", strLocal, strPath, strHost) == true)
                {
                    //img = Image.FromFile(strLocal);
                    //ssPRT3_Sheet1.Cells[iRow, iCol].Value = img;
                    //img = null;

                    MemoryStream ms = new MemoryStream();
                    Image.FromFile(strLocal).Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ssPRT3_Sheet1.Cells[iRow, iCol].Value = Image.FromStream(ms);
                }

                FtpedtX.FtpDisConnetBatch();
                FtpedtX = null;
            }
            catch
            {
                if (f.Exists == true)
                {
                    img = Image.FromFile(strLocal);
                    ssPRT3_Sheet1.Cells[iRow, iCol].Value = img;

                    img = null;
                }
                else
                {
                    if (FtpedtX.FtpDownloadEx("192.168.100.33", "pcnfs", "pcnfs1", strLocal, strPath, strHost) == true)
                    {
                        img = Image.FromFile(strLocal);
                        ssPRT3_Sheet1.Cells[iRow, iCol].Value = img;

                        img = null;
                    }
                }
            }
        }

        private string READ_FDRUGCD(string strEDICODE)
        {
            string rtnVal = "";

            if (strEDICODE == "") return rtnVal;

            OracleCommand cmd = new OracleCommand();
            PsmhDb pDbCon = clsDB.DbCon;
            OracleDataReader reader = null;
            DataTable dt = new DataTable();
            
            string pSearchType = "06";
            string pKeyword = strEDICODE;
            string pScope = "02";


            cmd.Connection = pDbCon.Con;
            cmd.InitialLONGFetchSize = 1000;
            cmd.CommandText = "KOSMOS_DRUG.up_DrugSearch";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("pSearchType", OracleDbType.Varchar2, 9, pSearchType, ParameterDirection.Input);
            cmd.Parameters.Add("pKeyword", OracleDbType.Varchar2, 9, pKeyword, ParameterDirection.Input);
            cmd.Parameters.Add("pScope", OracleDbType.Varchar2, 9, pScope, ParameterDirection.Input);

            cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

            reader = cmd.ExecuteReader();

            dt.Load(reader);
            reader.Dispose();
            reader = null;

            cmd.Dispose();
            cmd = null;
            
            if (dt == null)
            {
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["FdrugCd"].ToString().Trim();
            }

            return rtnVal;
        }

        private string READ_DRUGNAME(string strCode)
        {
            DataTable dt = null;
            StringBuilder SQL = new StringBuilder();
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL.Clear();
                SQL.AppendLine("SELECT A.SUNEXT, A.HNAME, A.SNAME, A.UNIT, A.EFFECT, ");
                SQL.AppendLine("       A.JEHENG, A.JEHENG2, A.JEHENG3_1, A.JEHENG3_2,  ");
                SQL.AppendLine("       A.JEYAK, B.GELCODE, C.NAME, D.DELDATE ");
                SQL.AppendLine(" FROM KOSMOS_OCS.OCS_DRUGINFO_NEW A, KOSMOS_ADM.DRUG_JEP B, KOSMOS_ADM.AIS_LTD C, KOSMOS_PMPA.BAS_SUT D ");
                SQL.AppendLine(" WHERE A.SUNEXT = '" + strCode + "' ");
                SQL.AppendLine("   AND A.SUNEXT = B.JEPCODE(+) ");
                SQL.AppendLine("   AND B.GELCODE = C.LTDCODE(+) ");
                SQL.AppendLine("   AND A.SUNEXT = D.SUNEXT(+) ");
                SQL.AppendLine("  ORDER BY SUNEXT ASC ");

                SqlErr = clsDB.GetDataTable(ref dt, SQL.ToString(), clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString(), clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = "(" + dt.Rows[0]["HNAME"].ToString().Trim() + ")";
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
                clsDB.SaveSqlErrLog(ex.Message, SQL.ToString(), clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            if (btnList.Text == "회신서보기")
            {
                panList.Visible = false;
                panel7.Visible = false;
                btnList.Text = "리스트보기";
            }
            else
            {
                panList.Visible = true;
                panel7.Visible = true;
                btnList.Text = "회신서보기";
            }            
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            //string strFont1 = "";
            //string strFoot1 = "";

            //strFont1 = "/fn\"맑은 고딕\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs1";

            //strFoot1 = "/l/f1" + "  ⊙ ◎원내-동일성분, 제형다름(약동학적 차이 있음)" + "/f1/n";
            //strFoot1 = strFoot1 + "/l/f1" + VB.Space(15) + "  => 약동학적 성질을 변화시킨 약품의 약효 및 약동학적 변화는 동일함량의 일반제형 약품과는" + "/f1/n";
            //strFoot1 = strFoot1 + "/l/f1" + VB.Space(15) + "     차이가 있으므로 원내약으로 대체할 경우 용량, 용법 등을 신중히 고려하시기 바랍니다." + "/f1/n";
            //strFoot1 = strFoot1 + "/l/f1" + VB.Space(15) + "  ⊙ 회신일자 : " + dtpHDate.Value.ToString("yyyy-MM-dd") + " " + dtpHTime.Value.ToString("HH:mm") + "                  ⊙  작 성 자 : 약제팀 " + lblPaDrugName.Text + " 약사 " + "/f1";


            ssPRT2_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssPRT2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;            
            //ssPRT2_Sheet1.PrintInfo.Footer = strFont1 + strFoot1;
            ssPRT2_Sheet1.PrintInfo.Margin.Top = 20;
            ssPRT2_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssPRT2_Sheet1.PrintInfo.Margin.Footer = 10;
            ssPRT2_Sheet1.PrintInfo.ShowColor = false;
            ssPRT2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssPRT2_Sheet1.PrintInfo.ShowBorder = false;
            ssPRT2_Sheet1.PrintInfo.ShowGrid = false;
            ssPRT2_Sheet1.PrintInfo.ShowShadows = false;
            ssPRT2_Sheet1.PrintInfo.UseMax = true;
            ssPRT2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssPRT2_Sheet1.PrintInfo.UseSmartPrint = false;
            ssPRT2_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssPRT2_Sheet1.PrintInfo.Preview = false;
            ssPRT2.PrintSheet(0);
        }
    }
}
