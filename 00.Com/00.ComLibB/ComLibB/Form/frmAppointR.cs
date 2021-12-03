using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComLibB 
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmAppointR.cs
    /// Description     : 예약현황
    /// Author          : 이정현
    /// Create Date     : 2018-05-24
    /// <history> 
    /// 예약현황
    /// </history>
    /// <seealso>
    /// PSMH\FrmAppointR.frm
    /// </seealso>
    /// <vbp>
    /// default 		: 
    /// </vbp>
    /// </summary>
    public partial class frmAppointR : Form 
    {
        private string GstrPrtGb = "";

        public frmAppointR()
        {
            InitializeComponent();
        }
        
        private void frmAppointR_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssView_Sheet1.RowCount = 0;

            Get_Ward();
            Get_Dept();
            Get_SlipNo();

            cboGb.Items.Add("1.병    동");
            cboGb.Items.Add("2.진료과목");
            cboGb.Items.Add("3.환자번호");

            cboGb.SelectedIndex = 1;

            dtpFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-1);
            dtpToDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(7);

            GstrPrtGb = "NO";
        }

        private void Get_Ward()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     WardCode, WardName ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_WARD ";
                SQL = SQL + ComNum.VBLF + "ORDER BY WardCode ";

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
                        cboWard.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
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

        private void Get_Dept()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     DeptCode, DeptNameK ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "     WHERE DeptCode NOT IN ('II','RT','TO','AN','HR','PT','PC') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking ";

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
                        cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboDept.SelectedIndex = 0;

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

        private void Get_SlipNo()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     SlipNo, OrderName";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE ";
                SQL = SQL + ComNum.VBLF + "     WHERE OrderCode = ' ' ";
                SQL = SQL + ComNum.VBLF + "         AND Seqno = 0 ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SlipNo ";

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
                        cboSlipNo.Items.Add(dt.Rows[i]["SLIPNO"].ToString().Trim() + dt.Rows[i]["ORDERNAME"].ToString().Trim());
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;

            string strOldSlipNo = "";
            string strNewSlipNo = "";

            ssView_Sheet1.RowCount = 0;
            GstrPrtGb = "NO";

            if (cboGb.SelectedIndex == 0) { return; }
            if (cboGb.SelectedIndex == 2 && txtPtno.Text.Trim() == "") { return; }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (chkXray.Checked == true)
                {
                    #region GoSub Xray_Disp

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     D.XJong, M.RoomCode, M.SName, M.Pano, M.Sex, M.Age, M.DeptCode, C.XName, ";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(D.SeekDate,'YYYY-MM-DD') AS SeekDate, TO_CHAR(D.SeekDate,'hh24:mi') AS SeekTime, ";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(D.EnterDate,'YYYY-MM-DD') AS EnterDate, D.GbEnd ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "XRAY_DETAIL D, ";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "XRAY_CODE C, ";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "IPD_NEW_MASTER M ";
                    SQL = SQL + ComNum.VBLF + "     WHERE D.SeekDate >= TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND D.SeekDate <= TO_DATE('" + dtpToDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND D.GbReserved <> '9' ";
                    SQL = SQL + ComNum.VBLF + "         AND D.XJong IN ('2', '3', '4', '5', '6', '7') ";
                    SQL = SQL + ComNum.VBLF + "         AND D.Pano = M.Pano ";

                    if (cboGb.SelectedIndex == 0)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND M.WardCode = '" + cboWard.Text + "' ";
                    }
                    else if (cboGb.SelectedIndex == 1)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND M.DeptCode = '" + cboDept.Text + "' ";
                    }
                    else if (cboGb.SelectedIndex == 2)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND M.Pano = '" + txtPtno.Text + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND M.GBSTS = '0' ";
                    SQL = SQL + ComNum.VBLF + "         AND D.XCode = C.XCode ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY D.XJong, D.SeekDate, M.RoomCode, M.SName ";

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
                            strNewSlipNo = dt.Rows[i]["XJONG"].ToString().Trim();

                            if (strOldSlipNo != strNewSlipNo)
                            {
                                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].ColumnSpan = 10;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = VB.Space(3) + "▶" + Get_XJongName(dt.Rows[i]["XJONG"].ToString().Trim());
                                strOldSlipNo = strNewSlipNo;
                            }

                            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["SEX"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["AGE"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["XNAME"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = Convert.ToDateTime(dt.Rows[i]["SEEKDATE"].ToString().Trim()).ToString("MM/dd");
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["SEEKTIME"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = Convert.ToDateTime(dt.Rows[i]["ENTERDATE"].ToString().Trim()).ToString("MM/dd");

                            if (dt.Rows[i]["SEEKTIME"].ToString().Trim() == "")
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "미접수";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "";
                            }
                            else
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "접수";
                            }

                            switch (dt.Rows[i]["GBEND"].ToString().Trim())
                            {
                                case "1": ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "촬영"; break;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                if (chkEndo.Checked == true)
                {
                    #region GoSub Endo_Disp

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     M.RoomCode, M.SName, M.Pano, M.Sex, ";
                    SQL = SQL + ComNum.VBLF + "     M.Age, M.DeptCode, C.OrderName, ";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(E.RDate,'YYYY-MM-DD') AS RDate, E.GbSunap, ";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(E.JDate,'YYYY-MM-DD') AS JDate, E.ResultDate ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "ENDO_JUPMST E, ";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_MED + "OCS_ORDERCODE C, ";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "IPD_NEW_MASTER M ";
                    SQL = SQL + ComNum.VBLF + "     WHERE E.RDate >= TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND E.RDate <= TO_DATE('" + dtpToDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND E.GbSunap <> '*'  ";
                    SQL = SQL + ComNum.VBLF + "         AND E.Gbio = 'I' ";
                    SQL = SQL + ComNum.VBLF + "         AND E.Ptno = M.Pano ";

                    if (cboGb.SelectedIndex == 0)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND M.WardCode = '" + cboWard.Text + "' ";
                    }
                    else if (cboGb.SelectedIndex == 1)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND M.DeptCode = '" + cboDept.Text + "' ";
                    }
                    else if (cboGb.SelectedIndex == 2)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND M.Pano = '" + txtPtno.Text + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND M.GBSTS = '0' ";
                    SQL = SQL + ComNum.VBLF + "         AND C.SlipNo = '0044' ";
                    SQL = SQL + ComNum.VBLF + "         AND C.OrderCode = E.OrderCode ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY E.RDate, M.RoomCode, M.SName ";

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
                            strNewSlipNo = "0044";

                            if (strOldSlipNo != strNewSlipNo)
                            {
                                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].ColumnSpan = 10;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = VB.Space(3) + "▶" + Get_SlipName(strNewSlipNo);
                                strOldSlipNo = strNewSlipNo;
                            }

                            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["SEX"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["AGE"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = Convert.ToDateTime(dt.Rows[i]["RDATE"].ToString().Trim()).ToString("MM/dd");
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "00:00";
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = Convert.ToDateTime(dt.Rows[i]["JDATE"].ToString().Trim()).ToString("MM/dd");
                            
                            if (dt.Rows[i]["GBSUNAP"].ToString().Trim() == "2")
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "미접수";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "";
                            }
                            else if (dt.Rows[i]["GBSUNAP"].ToString().Trim() == "1")
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "접수";
                            }

                            if (dt.Rows[i]["RESULTDATE"].ToString().Trim() != "")
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "결과";
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                if (chkEkg.Checked == true)
                {
                    #region GoSub Ekg_Disp

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     M.RoomCode, M.SName, M.Pano, M.Sex, ";
                    SQL = SQL + ComNum.VBLF + "     M.Age, M.DeptCode, E.OrderCode, C.OrderName, ";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(E.RDate,'YYYY-MM-DD HH24:MI') ResDate, ";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(E.EntDate,'YYYY-MM-DD') EntDate,  E.GbJob, E.GuBun ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_JUPMST E, ";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_MED + "OCS_ORDERCODE C, ";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "IPD_NEW_MASTER M ";
                    SQL = SQL + ComNum.VBLF + "     WHERE TO_CHAR(E.RDate,'YYYY-MM-DD') >= '" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND TO_CHAR(E.RDate,'YYYY-MM-DD') <= '" + dtpToDate.Value.ToString("yyyy-MM-dd") + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND E.GbJob <> '9' ";
                    SQL = SQL + ComNum.VBLF + "         AND E.GbIO = 'I' ";
                    SQL = SQL + ComNum.VBLF + "         AND E.Ptno = M.Pano ";

                    if (cboGb.SelectedIndex == 0)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND M.WardCode = '" + cboWard.Text + "' ";
                    }
                    else if (cboGb.SelectedIndex == 1)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND M.DeptCode = '" + cboDept.Text + "' ";
                    }
                    else if (cboGb.SelectedIndex == 2)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND M.Ptno = '" + txtPtno.Text + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND M.GBSTS = '0' ";
                    SQL = SQL + ComNum.VBLF + "         AND C.OrderCode = E.OrderCode ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY E.RDate, M.RoomCode, M.SName ";

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
                            strNewSlipNo = dt.Rows[i]["GUBUN"].ToString().Trim();

                            if (strOldSlipNo != strNewSlipNo)
                            {
                                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].ColumnSpan = 10;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = VB.Space(3) + "▶" + Get_FunName(strNewSlipNo);
                                strOldSlipNo = strNewSlipNo;
                            }

                            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["SEX"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["AGE"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = Convert.ToDateTime(dt.Rows[i]["RESDATE"].ToString().Trim()).ToString("MM/dd");
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = Convert.ToDateTime(dt.Rows[i]["RESDATE"].ToString().Trim()).ToString("HH:mm");
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = Convert.ToDateTime(dt.Rows[i]["ENTDATE"].ToString().Trim()).ToString("MM/dd");
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["GBJOB"].ToString().Trim();

                            if (ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text == "1")
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "미접수";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "";
                            }
                            else if (ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text == "2")
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "예약";
                            }
                            else
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "접수";
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                }

                if (chkPT.Checked == true)
                {
                    #region GoSub PT_Disp 물리치료는 항상 현재 시점에서 미치료/치료를 확인함

                    SQL = "";
                    SQL = "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(B.ACTDATE, 'YYYY-MM-DD') AS ACTDATE, TO_CHAR(B.CDATE, 'YYYY-MM-DD') AS CDATE,";
                    SQL = SQL + ComNum.VBLF + "     A.PANO ,A.Sname, A.RoomCode, A.SEX, A.AGE, A.DEPTCODE, B.SUCODE , C.SUNAMEK ";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, ";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "ETC_PTORDER B, ";
                    SQL = SQL + ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_SUN C ";
                    SQL = SQL + ComNum.VBLF + "     WHERE GBSTS IN ('0') ";

                    if (cboGb.SelectedIndex == 0)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.WardCode = '" + cboWard.Text + "' ";
                    }
                    else if (cboGb.SelectedIndex == 1)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.DeptCode = '" + cboDept.Text + "' ";
                    }
                    else if (cboGb.SelectedIndex == 2)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.Pano = '" + txtPtno.Text + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.PANO = B.PANO ";
                    SQL = SQL + ComNum.VBLF + "         AND B.GBIO = 'I'";
                    SQL = SQL + ComNum.VBLF + "         AND B.ACTDATE >= TRUNC(SYSDATE-2) ";
                    SQL = SQL + ComNum.VBLF + "         AND B.ACTDATE <= TRUNC(SYSDATE-1) ";
                    SQL = SQL + ComNum.VBLF + "         AND B.SUCODE = C.SUNEXT(+)";
                    SQL = SQL + ComNum.VBLF + "GROUP BY B.ACTDATE, B.CDATE, A.PANO, A.Sname, A.RoomCode, A.SEX, A.AGE, A.DEPTCODE, B.SUCODE, C.SUNAMEK  ";
                    SQL = SQL + ComNum.VBLF + "Union All ";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE,'YYYY-MM-DD') AS ACTDATE, TO_CHAR(SYSDATE,'YYYY-MM-DD') AS CDATE,";
                    SQL = SQL + ComNum.VBLF + "     A.PANO ,A.Sname, A.RoomCode, A.SEX, A.AGE, A.DEPTCODE, 'PT######' AS SUCODE, '물리치료의뢰' AS SUNAMEK";
                    SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_PMPA + "IPD_NEW_MASTER A ";
                    SQL = SQL + ComNum.VBLF + "     WHERE A.GBSTS = '0' ";

                    if (cboGb.SelectedIndex == 0)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.WardCode = '" + cboWard.Text + "' ";
                    }
                    else if (cboGb.SelectedIndex == 1)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.DeptCode = '" + cboDept.Text + "' ";
                    }
                    else if (cboGb.SelectedIndex == 2)
                    {
                        SQL = SQL + ComNum.VBLF + "         AND A.Pano = '" + txtPtno.Text + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + "         AND A.PANO IN ";
                    SQL = SQL + ComNum.VBLF + "                 (SELECT Ptno FROM " + ComNum.DB_MED + "OCS_IORDER ";
                    SQL = SQL + ComNum.VBLF + "                     WHERE BDate >= TRUNC(SYSDATE ) ";
                    SQL = SQL + ComNum.VBLF + "                         AND OrderCode = 'PT######'";
                    SQL = SQL + ComNum.VBLF + "                 GROUP BY Ptno)";
                    SQL = SQL + ComNum.VBLF + "GROUP BY A.PANO, A.Sname, A.RoomCode, A.SEX, A.AGE, A.DEPTCODE";

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
                            strNewSlipNo = "0102";

                            if (strOldSlipNo != strNewSlipNo)
                            {
                                ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].ColumnSpan = 10;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = VB.Space(3) + "▶" + Get_SlipName("0102");
                                strOldSlipNo = strNewSlipNo;
                            }

                            ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                            ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["SEX"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["AGE"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "물리치료:" + dt.Rows[i]["SUCODE"].ToString().Trim() + "->" + dt.Rows[i]["SUNAMEK"].ToString().Trim();

                            SQL = "";
                            SQL = "SELECT";
                            SQL = SQL + ComNum.VBLF + "     Pano";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_PTORDER ";
                            SQL = SQL + ComNum.VBLF + "     WHERE CDATE = TRUNC(SYSDATE) ";
                            SQL = SQL + ComNum.VBLF + "         AND PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "         AND GbIO = 'I' ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            if (dt1.Rows.Count == 0)
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = "";
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "미치료";
                            }
                            else
                            {
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).ToString("MM/dd");
                                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "치료";
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
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

        private string Get_XJongName(string strXJong)
        {
            string rtnVal = "";

            switch (strXJong)
            {
                case "1": rtnVal = "X선  일반 촬영"; break;
                case "2": rtnVal = "X선  특수 촬영"; break;
                case "3": rtnVal = "초    음    파"; break;
                case "4": rtnVal = "C           T"; break;
                case "5": rtnVal = "M     R     I"; break;
                case "6": rtnVal = "R           I"; break;
                case "7": rtnVal = "B     M     D"; break;
            }

            return rtnVal;
        }

        private string Get_SlipName(string strSlipNo)
        {
            string rtnVal = "";

            for (int i = 0; i < cboSlipNo.Items.Count; i++)
            {
                if (strSlipNo.Trim() == VB.Left(cboSlipNo.Items[i].ToString(), 4))
                {
                    rtnVal = VB.Mid(cboSlipNo.Items[i].ToString(), 5, cboSlipNo.Items[i].ToString().Length);
                    break;
                }
            }

            return rtnVal;
        }

        private string Get_FunName(string strSlipNo)
        {
            string rtnVal = "";

            switch (strSlipNo)
            {
                case "1": rtnVal = "EKG"; break;
                case "2": rtnVal = "뇌파"; break;
                case "3": rtnVal = "Echo"; break;
                case "4": rtnVal = "Pft"; break;
                case "5": rtnVal = "Mct"; break;
                case "6": rtnVal = "청력"; break;
                default: rtnVal = "???"; break;
            }

            return rtnVal;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";
            string strSubTitle = "";

            if (cboGb.SelectedIndex == 0)
            {
                strSubTitle = "병    동 : " + VB.Left(cboWard.Text, 4).Trim();
            }
            else if (cboGb.SelectedIndex == 1)
            {
                strSubTitle = "진료과목 : " + VB.Left(cboDept.Text, 4).Trim();
            }
            else if (cboGb.SelectedIndex == 2)
            {
                strSubTitle = "환자번호 : " + txtPtno.Text.Trim();
            }

            strFont1 = "/fn\"맑은 고딕\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "예약 현황" + "/f1/n";
            strHead2 = "/l/f2" + strSubTitle + "/f2/n";
            strHead2 += VB.Space(10) + "/l/f2" + "예약일자 : " + dtpFrDate.Value.ToString("yyyy-MM-dd") + " - " + dtpToDate.Value.ToString("yyyy-MM-dd")
                + VB.Space(5) + " 출력시간 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + VB.Space(20) + "Page : /p" + "/f2/n";

            //ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Top = 20;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.Margin.Header = 10;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView_Sheet1.PrintInfo.UseSmartPrint = false;
            ssView_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssView_Sheet1.PrintInfo.Preview = false;
            ssView.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboGb_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (VB.Left(cboGb.Text, 1))
            {
                case "1":
                    cboWard.Visible = true;
                    cboDept.Visible = false;
                    txtPtno.Visible = false;
                    cboWard.Focus();
                    break;
                case "2":
                    cboWard.Visible = false;
                    cboDept.Visible = true;
                    txtPtno.Visible = false;
                    ComFunc.ComboFind(cboDept, "L", 2, clsPublic.GstrDeptCode);
                    cboDept.Focus();
                    break;
                case "3":
                    cboWard.Visible = false;
                    cboDept.Visible = false;
                    txtPtno.Visible = true;
                    txtPtno.Focus();
                    break;
            }
        }
    }
}
