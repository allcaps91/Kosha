using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmDrJob01
    /// File Name : frmDrJob01.cs
    /// Title or Description : 약품처방일수관리
    /// Author : 유진호
    /// Create Date : 2017-11-03
    /// Update Histroy :     
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso>
    /// VB\basic\busuga\miretc64.frm(FrmSugaDayCount)
    /// </seealso> 
    /// </summary>
    public partial class frmDrJob01 : Form
    {
        ComFunc CF = new ComFunc();
        private string GstrHelpCode = "";

        public frmDrJob01()
        {
            InitializeComponent();
        }

        public frmDrJob01(string GstrHelpCode)
        {
            InitializeComponent();
            this.GstrHelpCode = GstrHelpCode;
        }

        private void frmDrJob01_Load(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            CF.FormInfo_History(clsDB.DbCon, this.Name, this.Text, clsCompuInfo.gstrCOMIP, clsType.User.Sabun, clsPublic.GstrJobPart);

            ComFunc.ReadSysDate(clsDB.DbCon);

            ss1.Parent = panMain;
            ss1.Dock = DockStyle.Fill;
            
            txtJepCode.Text = GstrHelpCode;

            txtPano.Text = "";
            txtFDate.Text = CF.DATE_ADD(clsDB.DbCon, clsPublic.GstrSysDate, -14);
            txtTDate.Text = clsPublic.GstrSysDate;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";
                SQL = " SELECT NAME, DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + " WHERE BUSEBUN  = '2' ";
                SQL = SQL + ComNum.VBLF + "   AND ORDFLAG= 'Y' ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + " ORDER BY BUCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    cboDept.Items.Add("전체                      **");
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["NAME"].ToString().Trim() + "             " + dt.Rows[i]["DEPTCODE"].ToString().Trim());
                    }
                    cboDept.SelectedIndex = 0;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            cboSort.Items.Add("1.병동별");
            cboSort.Items.Add("2.처방의별");
            cboSort.Items.Add("3.약품코드별");
            cboSort.Items.Add("4.진료과별");
            cboSort.SelectedIndex = 0;
        }

        #region // Drug_JepCode FUNC
        private string Drug_JepCode(string ArgJep)
        {
            string rtVal = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return rtVal; //권한 확인

                SQL = "";
                SQL = " SELECT SUNAMEK FROM ADMIN.BAS_SUN ";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + ArgJep + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtVal = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtVal;
            }
        }
        #endregion

        #region // Drug_day FUNC
        private double Drug_Day(string ArgJep)
        {
            double rtVal = 0;
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return rtVal; //권한 확인

                SQL = "";
                SQL = " SELECT DAYQTY FROM ADMIN.JSIM_GIJUN ";
                SQL = SQL + ComNum.VBLF + " WHERE SUCODE  = '" + ArgJep + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtVal = Convert.ToDouble(VB.Val(dt.Rows[0]["DAYQTY"].ToString().Trim()));
                }

                dt.Dispose();
                dt = null;

                return rtVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtVal;
            }

        }
        #endregion

        #region // Pano_name FUNC
        private string Pano_Name(string ArgData)
        {
            string rtVal = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return rtVal; //권한 확인

                SQL = "";
                SQL = " SELECT SNAME FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgData + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtVal = dt.Rows[0]["SNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);

                return rtVal;
            }
        }
        #endregion

        #region // Dr_Name FUNC
        private string Dr_Name(string ArgData)
        {
            string rtVal = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return rtVal; //권한 확인

                SQL = "";
                SQL = " SELECT DRNAME FROM ADMIN.BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DRCODE = '" + ArgData + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtVal = dt.Rows[0]["DRNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtVal;
            }
        }
        #endregion



        private void btnSave_Click(object sender, EventArgs e)
        {
            //TODO
            //FrmdrJob03.Show 1
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strFDate = "";
            string strTDate = "";
            string strJEPCODE = "";
            string strPano = "";
            string strDept = "";
            string strDrCode = "";

            string strJep = "";
            string strJepName = "";
            double nDayQty;
            double nQty;
            string strSname = "";
            string strDept1 = "";
            string strDrname = "";
            string strBun = "";
            string strNewBun = "";
            string strOldBun = "";
            double nSosum = 0;
            string strWard = "";



            strFDate = txtFDate.Text;
            strTDate = txtTDate.Text;
            strJEPCODE = VB.Trim(txtJepCode.Text);
            strPano = VB.Trim(txtPano.Text);
            strDept = VB.Trim(VB.Right(VB.Trim(cboDept.Text), 2));
            strDrCode = VB.Trim(VB.Right(VB.Trim(cboDrCode.Text), 4));
            btnPrint.Enabled = true;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                //'외래
                if (optGubun_0.Checked == true)
                {
                    SQL = " SELECT SUNEXT, PANO, SUM(NAL) QTY, A.DEPTCODE, A.DRCODE, A.BUN, B.DAYS, C.DRNAME  ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.OPD_SLIP A, ADMIN.DRUG_JEPDAY B, ADMIN.BAS_DOCTOR C ";
                    SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND BUN IN ('11','12','20','21') ";
                    SQL = SQL + ComNum.VBLF + "    AND A.DRCODE = C.DRCODE ";

                    if (strJEPCODE != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND SUNEXT = '" + strJEPCODE + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND A.SUNEXT = B.JEPCODE(+) ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.SUNEXT = B.JEPCODE ";
                    }

                    if (strPano != "") SQL = SQL + ComNum.VBLF + " AND PANO  = '" + strPano + "' ";
                    if (strDept != "**") SQL = SQL + ComNum.VBLF + " AND DEPTCODE = '" + strDept + "' ";
                    if (strDrCode != "**") SQL = SQL + ComNum.VBLF + " AND DRCODE = '" + strDrCode + "' ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY BUN, SUNEXT, PANO, DEPTCODE, DRCODE, B.DAYS, C.DRNAME ";
                }
                else
                {
                    SQL = " SELECT A.SUNEXT, A.PANO, SUM(A.NAL) QTY, C.DEPTCODE, C.DRCODE, A.BUN, C.WARDCODE, B.DAYS, D.DRNAME ";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.IPD_NEW_SLIP A, ADMIN.DRUG_JEPDAY B, ADMIN.IPD_NEW_MASTER C, ADMIN.BAS_DOCTOR D ";
                    SQL = SQL + ComNum.VBLF + " WHERE A.BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND BUN IN ('11','12','20','21') ";
                    if (optIpd_0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND C.OUTDATE IS NULL ";
                        SQL = SQL + ComNum.VBLF + "    AND C.GBSTS IN ('0') "; //'＃추가했음;
                    }
                    else if (optIpd_0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "    AND C.GBSTS NOT IN ('0') "; //'＃추가했음;
                    }

                    SQL = SQL + ComNum.VBLF + "    AND A.IPDNO = C.IPDNO ";
                    SQL = SQL + ComNum.VBLF + "    AND C.DRCODE = D.DRCODE ";

                    if (strJEPCODE != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND A.SUNEXT = '" + strJEPCODE + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND A.SUNEXT = B.JEPCODE(+) ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "    AND A.SUNEXT = B.JEPCODE ";
                    }

                    if (strPano != "") SQL = SQL + ComNum.VBLF + " AND A.PANO  = '" + strPano + "' ";
                    if (strDept != "**") SQL = SQL + ComNum.VBLF + " AND C.DEPTCODE = '" + strDept + "' ";
                    if (strDrCode != "**") SQL = SQL + ComNum.VBLF + " AND C.DRCODE = '" + strDrCode + "' ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY A.BUN, C.WARDCODE, A.SUNEXT, A.PANO, C.DEPTCODE, C.DRCODE,B.DAYS, D.DRNAME ";
                }
                SQL = SQL + ComNum.VBLF + "  HAVING SUM(A.NAL) <> 0 ";
                if (VB.Left(cboSort.Text, 1) == "1")
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY BUN, WARDCODE, SUNEXT, PANO ";
                }
                else if (VB.Left(cboSort.Text, 1) == "2")
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY BUN, DRNAME, SUNEXT ";
                }
                else if (VB.Left(cboSort.Text, 1) == "3")
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY BUN, SUNEXT, PANO ";
                }
                else if (VB.Left(cboSort.Text, 1) == "4")
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY BUN, DEPTCODE, SUNEXT, PANO ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY BUN, SUNEXT, PANO ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                strNewBun = "";
                strOldBun = "";
                nSosum = 0;

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.RowCount = ss1_Sheet1.RowCount + 1;

                        strJep = dt.Rows[i]["SUNEXT"].ToString().Trim();
                        strJepName = Drug_JepCode(strJep);
                        strNewBun = dt.Rows[i]["BUN"].ToString().Trim();
                        nDayQty = Convert.ToInt32(VB.Val(dt.Rows[i]["DAYS"].ToString().Trim()));
                        nQty = Convert.ToInt32(VB.Val(dt.Rows[i]["QTY"].ToString().Trim()));
                        nSosum = nSosum + nQty;
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strSname = Pano_Name(strPano);
                        strDept1 = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        strDrname = dt.Rows[i]["DRNAME"].ToString().Trim();

                        if (optGubun_1.Checked == true)
                        {
                            strWard = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        }

                        if (strNewBun != strOldBun)
                        {
                            switch (strNewBun)
                            {
                                case "11":
                                    strBun = "내 복 약";
                                    break;
                                case "12":
                                    strBun = "외 용 약";
                                    break;
                                case "20":
                                    strBun = "주 사 제";
                                    break;
                                case "21":
                                    strBun = "특정재료";
                                    break;
                            }
                            strOldBun = strNewBun;
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 0].Text = strBun;
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, -1].ForeColor = Color.FromArgb(((int)(0)), ((int)(0)), ((int)(0)));
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, -1].BackColor = Color.FromArgb(((int)(236)), ((int)(255)), ((int)(255)));
                            ss1_Sheet1.RowCount = ss1_Sheet1.RowCount + 1;
                        }

                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 0].Text = strJep;
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 1].Text = strJepName;
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 2].Text = nDayQty.ToString();
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 3].Text = nQty.ToString() + "   ";
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 4].Text = strSname;
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 5].Text = strPano;
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 6].Text = strDept1;
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 7].Text = strDrname;
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 8].Text = strWard;

                        if (nDayQty > 0)
                        {
                            if ((nQty / nDayQty * 100) >= 90)
                            {
                                //'Determine the color of background, foreground and border color
                                ss1_Sheet1.Rows[ss1_Sheet1.RowCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                                ss1_Sheet1.Rows[ss1_Sheet1.RowCount - 1].BackColor = Color.FromArgb(254, 218, 228);
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                if (VB.Trim(txtJepCode.Text) != "")
                {
                    ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 1].Text = "   합    계   ";
                    ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 3].Text = VB.Format(nSosum, "##,###,##") + "   ";

                    //'Determine the color of background, foreground and border color
                    ss1_Sheet1.Rows[ss1_Sheet1.RowCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                    ss1_Sheet1.Rows[ss1_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 236, 238);
                }
                else
                {
                    ss1_Sheet1.RowCount = ss1_Sheet1.RowCount - 1;
                    ss1_Sheet1.SetRowHeight(-1, 14);
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            int i = 0;
            double nDay = 0;
            double nNal = 0;
            string strJepName = "";
            string strJEPCODE = "";            
            string strName = "";
            string strPano = "";
            string strDeptCode = "";
            string strDrname = "";
            string strWard = "";

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인


            ss3_Sheet1.RowCount = 1;
            for (i = 0; i < ss1_Sheet1.RowCount; i++)
            {
                nDay = VB.Val(ss1_Sheet1.Cells[i, 2].Text.Trim());
                nNal = VB.Val(ss1_Sheet1.Cells[i, 3].Text.Trim());
                strJepName = ss1_Sheet1.Cells[i, 1].Text.Trim();

                if (strJepName != "" && strJepName != "합    계")
                {
                    strJEPCODE = ss1_Sheet1.Cells[i, 0].Text.Trim();
                    strJepName = ss1_Sheet1.Cells[i, 1].Text.Trim();
                    strName = ss1_Sheet1.Cells[i, 4].Text.Trim();
                    strPano = ss1_Sheet1.Cells[i, 5].Text.Trim();
                    strDeptCode = ss1_Sheet1.Cells[i, 6].Text.Trim();
                    strDrname = ss1_Sheet1.Cells[i, 7].Text.Trim();
                    strWard = ss1_Sheet1.Cells[i, 8].Text.Trim();

                    ss3_Sheet1.Cells[ss3_Sheet1.RowCount - 1, 0].Text = strJEPCODE;
                    ss3_Sheet1.Cells[ss3_Sheet1.RowCount - 1, 1].Text = strJepName;
                    ss3_Sheet1.Cells[ss3_Sheet1.RowCount - 1, 2].Text = nDay.ToString();
                    ss3_Sheet1.Cells[ss3_Sheet1.RowCount - 1, 3].Text = nNal.ToString();
                    ss3_Sheet1.Cells[ss3_Sheet1.RowCount - 1, 4].Text = strName;
                    ss3_Sheet1.Cells[ss3_Sheet1.RowCount - 1, 5].Text = strPano;
                    ss3_Sheet1.Cells[ss3_Sheet1.RowCount - 1, 6].Text = strDeptCode;
                    ss3_Sheet1.Cells[ss3_Sheet1.RowCount - 1, 7].Text = strDrname;
                    ss3_Sheet1.Cells[ss3_Sheet1.RowCount - 1, 8].Text = strWard;
                    ss3_Sheet1.RowCount = ss3_Sheet1.RowCount + 1;
                }
                else
                {
                    strJEPCODE = ss1_Sheet1.Cells[i, 0].Text.Trim();
                    ss3_Sheet1.Cells[ss3_Sheet1.RowCount - 1, 0].Text = strJEPCODE;
                    ss3_Sheet1.RowCount = ss3_Sheet1.RowCount + 1;
                }
            }

            
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";
            string Systime = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Systime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            ss1_Sheet1.Columns[9].Visible = true;
            ss1_Sheet1.Columns[10].Visible = true;
            ss1_Sheet1.Columns[11].Visible = true;

            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"18\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fb0 /fi0 /fu0 /fk0 /fs2";
            if (optGubun_0.Checked == true)
            {
                strHead1 = "/n/n/f1/C 약품처방일수관리(외래)" + "/n/n/n/n";
            }
            else
            {
                strHead1 = "/n/n/f1/C 약품처방일수관리(입원)" + "/n/n/n/n";
            }

            strHead2 = "/l/f2" + "☞조회기간 : " + txtFDate.Text + "~" + txtTDate.Text + VB.Space(35) + "☞인쇄일자 : " + clsPublic.GstrSysDate + "";
            
            ss1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ss1_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ss1_Sheet1.PrintInfo.Margin.Left = 35;
            ss1_Sheet1.PrintInfo.Margin.Right = 0;
            ss1_Sheet1.PrintInfo.Margin.Top = 35;
            ss1_Sheet1.PrintInfo.Margin.Bottom = 30;
            ss1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ss1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ss1_Sheet1.PrintInfo.ShowBorder = true;
            ss1_Sheet1.PrintInfo.ShowColor = false;
            ss1_Sheet1.PrintInfo.ShowGrid = true;
            ss1_Sheet1.PrintInfo.ShowShadows = false;
            ss1_Sheet1.PrintInfo.UseMax = false;
            ss1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ss1.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void optGubun_0_CheckedChanged(object sender, EventArgs e)
        {
            cboSort.Items.Clear();
            if (optGubun_0.Checked == true)
            {
                cboSort.Items.Add("2.등록번호별");
                cboSort.Items.Add("3.약품코드별");
                cboSort.Items.Add("4.진료과별");
                grb2.Enabled = false;
            }
            cboSort.SelectedIndex = 0;
        }

        private void optGubun_1_CheckedChanged(object sender, EventArgs e)
        {
            cboSort.Items.Clear();
            if (optGubun_1.Checked == true)
            {
                cboSort.Items.Add("1.병동별");
                cboSort.Items.Add("2.등록번호별");
                cboSort.Items.Add("3.약품코드별");
                cboSort.Items.Add("4.진료과별");
                grb2.Enabled = true;
            }
            cboSort.SelectedIndex = 0;
        }

        private void ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strPano = "";
            string strJEPCODE = "";
            string strFDate = "";
            string strTDate = "";
            string strIO = "";
            int nREAD = 0;


            strIO = "I";
            if (optGubun_0.Checked == true) strIO = "O";
            strFDate = txtFDate.Text;
            strTDate = txtTDate.Text;


            strJEPCODE = ss1_Sheet1.Cells[e.Row, 0].Text.Trim();
            strPano = ss1_Sheet1.Cells[e.Row, 5].Text.Trim();
            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                ss2_Sheet1.RowCount = 0;

                if (e.Column == 3)
                {
                    if (strIO == "O") //'외래
                    {
                        SQL = " SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.PANO, A.SUNEXT, A.NAL, A.DEPTCODE, B.DRNAME ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.OPD_SLIP A, ADMIN.BAS_DOCTOR B ";
                        SQL = SQL + ComNum.VBLF + " WHERE A.BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.PANO  = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT  = '" + strJEPCODE + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.DRCODE = B.DRCODE(+) ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY BDATE ";                        
                    }
                    else
                    {
                        SQL = " SELECT TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.PANO, A.SUNEXT, A.NAL, A.DEPTCODE, B.DRNAME ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.IPD_NEW_SLIP A, ADMIN.BAS_DOCTOR B , ADMIN.IPD_NEW_MASTER C "; //'＃
                        SQL = SQL + ComNum.VBLF + " WHERE A.BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.PANO  = '" + strPano + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.SUNEXT  = '" + strJEPCODE + "' ";
                        if (optGubun_0.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "   AND C.OUTDATE IS NULL ";  //'＃
                            SQL = SQL + ComNum.VBLF + "   AND C.GBSTS = '0' ";  //'＃
                        }
                        else if (optGubun_0.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "   AND C.GBSTS NOT IN ('0') ";   //'＃
                        }                        
                        SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = C.IPDNO ";
                        SQL = SQL + ComNum.VBLF + "   AND A.DRCODE = B.DRCODE(+) ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY BDATE ";                        
                    }
                }
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {                    
                    ss2_Sheet1.RowCount = dt.Rows.Count;
                    ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ss2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DRNAME"].ToString().Trim();                                                
                    }
                    panPopUP.Visible = true;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }            
        }

        private void btnExit_PopUp_Click(object sender, EventArgs e)
        {
            panPopUP.Visible = false;
        }

        private void txtFDate_DoubleClick(object sender, EventArgs e)
        {
            Calendar_Date_Select(txtFDate);
        }

        private void txtTDate_DoubleClick(object sender, EventArgs e)
        {
            Calendar_Date_Select(txtTDate);
        }

        private void Calendar_Date_Select(Control ArgText)
        {
            clsPublic.GstrCalDate = VB.Trim(ArgText.Text);
            if (VB.Len(VB.Trim(ArgText.Text)) != 10)
            {
                clsPublic.GstrCalDate = clsPublic.GstrSysDate;
            }

            frmCalendar frmCalendarX = new frmCalendar();
            frmCalendarX.ShowDialog();

            if (VB.Len(clsPublic.GstrCalDate) == 10)
            {
                ArgText.Text = clsPublic.GstrCalDate;
            }
        }

        private void txtJepCode_Leave(object sender, EventArgs e)
        {
            txtJepCode.Text = VB.UCase(txtJepCode.Text);
        }

        private void txtPano_Enter(object sender, EventArgs e)
        {
            txtPano.Text = "";
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            txtPano.Text = VB.Format(txtPano.Text, "00000000");
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            Set_ComboDrCode();
        }

        void Set_ComboDrCode()
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strDept = VB.Right(cboDept.Text.Trim(), 2);

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT DRNAME , DRCODE FROM ADMIN.BAS_DOCTOR ";
                SQL += ComNum.VBLF + " WHERE TOUR  = 'N' ";
                if(SQL != "**") SQL += ComNum.VBLF + "   AND DRDEPT1  = '" + strDept + "' ";
                SQL += ComNum.VBLF + " ORDER BY DRCODE, PRINTRANKING ";
                SQL += ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                cboDrCode.Items.Clear();
                cboDrCode.Items.Add("전체                             **");

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    cboDrCode.Items.Add(dt.Rows[i]["DRNAME"].ToString().Trim() + "             " +
                        dt.Rows[i]["DRCODE"].ToString().Trim());
                }

                cboDrCode.SelectedIndex = 0;

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

    }
}
