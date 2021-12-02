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
using ComLibB;

namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmOpdNurEndoList.cs
    /// Description     : 내시경 환자관리
    /// Author          : 유진호
    /// Create Date     : 2018-01-17
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\FrmENDOList
    /// </history>
    /// </summary>
    public partial class frmOpdNurEndoList : Form
    {
        private ContextMenu PopupMenu = null;
        private MenuItem SubItem1 = null;
        ComFunc CF = null;

        private string strPANO = "";
        private string strDEPT = "";
        private string strBDATE = "";
        private string strDRCD = "";

        public frmOpdNurEndoList()
        {
            InitializeComponent();
        }

        private void frmOpdNurEndoList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            CF = new ComFunc();


            dtpFDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            dtpTDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);

            CF.COMBO_DEPT_SET(clsDB.DbCon, cboDEPT);

            cboIO.Items.Clear();
            cboIO.Items.Add("*.전체");
            cboIO.Items.Add("I.입원");
            cboIO.Items.Add("O.외래");
            cboIO.SelectedIndex = 2;

            cboDrCode.Items.Clear();
            cboDrCode.Items.Add("****.전체");
            cboDrCode.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DateTime dtBDate;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.BDATE, A.RDATE, A.PTNO, A.SNAME, ";
                SQL = SQL + ComNum.VBLF + "  A.DEPTCODE, A.DRCODE, B.DRNAME, RESULTDATE, ";
                SQL = SQL + ComNum.VBLF + " GBIO, C.ORDERNAME,  A.REMARK, A.GBSUNAP, ";
                SQL = SQL + ComNum.VBLF + "  A.ROWID , D.TEL, D.HPHONE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.ENDO_JUPMST A , BAS_DOCTOR B, KOSMOS_OCS.OCS_ORDERCODE C, KOSMOS_PMPA.BAS_PATIENT D ";
                SQL = SQL + ComNum.VBLF + " WHERE A.DRCODE =B.DRCODE (+) ";
                if (opt_1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "   AND RDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                    SQL = SQL + ComNum.VBLF + "   AND RDATE <  " + ComFunc.ConvOraToDate(dtpFDate.Value.AddDays(1), "D");
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.BDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                    SQL = SQL + ComNum.VBLF + "   AND A.BDATE <  " + ComFunc.ConvOraToDate(dtpFDate.Value.AddDays(1), "D");
                }
                if (VB.Left(cboDEPT.Text, 2) != "**") SQL = SQL + ComNum.VBLF + " AND A.DEPTCODE = '" + VB.Left(cboDEPT.Text, 2) + "' ";
                if (VB.Left(cboDrCode.Text, 4) != "****") SQL = SQL + ComNum.VBLF + " AND A.DRCODE = '" + VB.Left(cboDrCode.Text, 4) + "' ";
                if (VB.Left(cboIO.Text, 1) != "*") SQL = SQL + ComNum.VBLF + " AND A.GBIO = '" + VB.Left(cboIO.Text, 1) + "' ";

                SQL = SQL + ComNum.VBLF + "   AND A.ORDERCODE =C.ORDERCODE(+) ";
                SQL = SQL + ComNum.VBLF + "    AND A.PTNO  = D.PANO(+)";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.RDATE, A.GBIO ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (VB.IsDate(dt.Rows[i]["BDate"].ToString().Trim()) == true)
                        {
                            ssView_Sheet1.Cells[i, 0].Text = Convert.ToDateTime(dt.Rows[i]["BDate"].ToString().Trim()).ToShortDateString();
                        }
                        //ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                        if (VB.IsDate(dt.Rows[i]["RDate"].ToString().Trim()) == true)
                        {
                            ssView_Sheet1.Cells[i, 1].Text = Convert.ToDateTime(dt.Rows[i]["RDate"].ToString().Trim()).ToShortDateString();
                        }
                        //ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RDate"].ToString().Trim();
                        if (VB.IsDate(READ_OPD_RESERVED(dt.Rows[i]["PtNo"].ToString().Trim())) == true)
                        {
                            ssView_Sheet1.Cells[i, 2].Text = Convert.ToDateTime(READ_OPD_RESERVED(dt.Rows[i]["PtNo"].ToString().Trim())).ToShortDateString();
                        }
                        //ssView_Sheet1.Cells[i, 2].Text = READ_OPD_RESERVED(dt.Rows[i]["PtNo"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PtNo"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["sName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DrName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GBIO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();

                        if (dt.Rows[i]["GbSunap"].ToString().Trim() == "1")     //접수
                        {
                            ssView_Sheet1.Cells[i, 9].Text = "접수";
                        }
                        else if (dt.Rows[i]["GbSunap"].ToString().Trim() == "2")    //미접수
                        {
                        }
                        else if (dt.Rows[i]["GbSunap"].ToString().Trim() == "*")    //취소
                        {
                            ssView_Sheet1.Cells[i, 9].Text = "취소";
                            ssView_Sheet1.Cells[i, 9].ForeColor = Color.Red;
                        }

                        if (dt.Rows[i]["ResultDate"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 9].Text = "결과";
                        }


                        ssView_Sheet1.Cells[i, 10].Text = "T:" + dt.Rows[i]["Tel"].ToString().Trim() + "H:" + dt.Rows[i]["HPhone"].ToString().Trim();

                        if (VB.IsDate(dt.Rows[i]["BDate"].ToString().Trim()) == true)
                        {
                            dtBDate = Convert.ToDateTime(dt.Rows[i]["BDate"].ToString().Trim());
                            if (clsOpdNr.READ_OPD_HAPPYCALL(clsDB.DbCon, "06", dt.Rows[i]["PtNo"].ToString().Trim(), dt.Rows[i]["DeptCode"].ToString().Trim(), dtBDate.ToShortDateString()) == true)
                            {
                                ssView_Sheet1.Cells[i, 11].Text = "Y";
                            }
                        }
                        
                        ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Remark"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 14].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C 내시경 예약 환자 LIST" + "/n/n/n/n";
            strHead2 = "/l/f2" + "조회기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + "~" + dtpTDate.Value.ToString("yyyy-MM-dd") + VB.Space(10) + "인쇄일자 : " + SysDate;

            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 35;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 35;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string READ_OPD_RESERVED(string strPTNO)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = " SELECT TO_CHAR (R.DATE3, 'YYYY-MM-DD') Date1,       TO_CHAR (R.DATE3, 'HH24:MI') Date3";
                SQL = SQL + ComNum.VBLF + "   FROM OPD_RESERVED_NEW R";
                SQL = SQL + ComNum.VBLF + "  WHERE     R.DATE3 >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "    AND R.DATE3 < " + ComFunc.ConvOraToDate(dtpTDate.Value.AddDays(200), "D");
                SQL = SQL + ComNum.VBLF + "    AND R.RETDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND R.TRANSDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "    AND R.PANO = '" + strPTNO + "'";
                SQL = SQL + ComNum.VBLF + "    AND R.DEPTCODE = 'MG'";
                SQL = SQL + ComNum.VBLF + " Union All";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR (R.RDATE, 'YYYY-MM-DD') Date1,       R.RTime Date3";
                SQL = SQL + ComNum.VBLF + "   FROM OPD_TELRESV R";
                SQL = SQL + ComNum.VBLF + "  WHERE     R.RDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "    AND R.RDATE <= " + ComFunc.ConvOraToDate(dtpTDate.Value.AddDays(200), "D");
                SQL = SQL + ComNum.VBLF + "    AND R.PANO = '" + strPTNO + "'";
                SQL = SQL + ComNum.VBLF + "    AND R.DEPTCODE = 'MG'";
                SQL = SQL + ComNum.VBLF + " ORDER BY 1 ASC, 2 ASC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["DATE1"].ToString().Trim() + " " + dt.Rows[0]["DATE3"].ToString().Trim();
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

            return rtnVal;
        }

        private void cboDEPT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VB.Left(cboDEPT.Text, 2) != "**")
            {
                clsVbfunc.SetDrCodeCombo(clsDB.DbCon, cboDrCode, VB.Left(cboDEPT.Text, 2), "", 1, "");
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {            
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true || e.RowHeader == true) return;

            strBDATE = ssView_Sheet1.Cells[e.Row, 0].Text;
            strPANO = ssView_Sheet1.Cells[e.Row, 3].Text;
            strDEPT = ssView_Sheet1.Cells[e.Row, 5].Text;
            strDRCD = ssView_Sheet1.Cells[e.Row, 14].Text;

            // 마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                PopupMenu = new ContextMenu();
                ssView.ContextMenu = null;

                PopupMenu.Name = "ssView";
                PopupMenu.MenuItems.Add("EMR 뷰어", new System.EventHandler(mnuSet_Click));
                PopupMenu.MenuItems.Add("참고사항", new System.EventHandler(mnuSet_Click));

                SubItem1 = new MenuItem();
                SubItem1.Text = "해피콜설정";
                SubItem1.MenuItems.Clear();
                SubItem1.MenuItems.Add("해피콜(01. 예약부도)", new System.EventHandler(SubMenuClick)); SubItem1.MenuItems[0].Name = "mnuItem_0";
                SubItem1.MenuItems.Add("해피콜(02. 예약안내)", new System.EventHandler(SubMenuClick)); SubItem1.MenuItems[1].Name = "mnuItem_1";
                SubItem1.MenuItems.Add("해피콜(03. 환자안부)", new System.EventHandler(SubMenuClick)); SubItem1.MenuItems[2].Name = "mnuItem_2";
                SubItem1.MenuItems.Add("해피콜(04. 설정지움)", new System.EventHandler(SubMenuClick)); SubItem1.MenuItems[3].Name = "mnuItem_3";
                PopupMenu.MenuItems.Add(SubItem1);

                PopupMenu.MenuItems[0].Name = "PopupMenu_0";
                PopupMenu.MenuItems[1].Name = "PopupMenu_1";
                PopupMenu.MenuItems[2].Name = "PopupMenu_2";

            }
        }

        private void mnuSet_Click(object sender, EventArgs e) // 이벤트 헨들러
        {
            string strPopMenuName = "";
            string strSelectMenuName = "";
            string strSelectMenuText = "";

            strPopMenuName = ((MenuItem)sender).Parent.Name;
            strSelectMenuName = ((MenuItem)sender).Name;
            strSelectMenuText = ((MenuItem)sender).Text;

            if (strPopMenuName == "ssView")
            {
                if (strSelectMenuName == "EMR 뷰어")
                {
                    //clsVbEmr.EXECUTE_TextEmrView(strPANO, clsType.User.Sabun);
                    clsVbEmr.EXECUTE_TextEmrView(strPANO, clsVbfunc.GetOCSDrCodeSabun(clsDB.DbCon, strDRCD));
                }
                else if (strSelectMenuName == "참고사항")
                {
                    frmMemo frmMemoX = new frmMemo(strPANO, strDEPT, clsOpdNr.GstrEmrViewDoct);
                    frmMemoX.StartPosition = FormStartPosition.CenterParent;
                    frmMemoX.ShowDialog();
                }

                ssView.ContextMenu = null;
            }
        }

        private void SubMenuClick(object sender, EventArgs e)
        {

            string strMenuName = "";
            string strSubName = "";
            string strSubText = "";

            strMenuName = ((MenuItem)sender).Parent.Name;
            strSubName = ((MenuItem)sender).Name;
            strSubText = ((MenuItem)sender).Text;

            if (strMenuName == "PopupMenu_2")
            {
                if (strSubName == "mnuItem_0")
                {
                    //해피콜(01. 예약부도)
                    if (clsOpdNr.READ_OPD_HAPPYCALL(clsDB.DbCon, "03", strPANO, strDEPT, strBDATE) == true)
                    {
                        clsOpdNr.UPDATE_HappyCall_OPD(clsDB.DbCon, "03", strPANO, "01", strDEPT, strBDATE);
                    }
                    else
                    {
                        clsOpdNr.INSERT_HappyCall_OPD(clsDB.DbCon, "03", strPANO, "01", strDEPT, strBDATE);
                    }
                }
                if (strSubName == "mnuItem_1")
                {
                    //해피콜(02. 예약안내)
                    if (clsOpdNr.READ_OPD_HAPPYCALL(clsDB.DbCon, "03", strPANO, strDEPT, strBDATE) == true)
                    {
                        clsOpdNr.UPDATE_HappyCall_OPD(clsDB.DbCon, "03", strPANO, "02", strDEPT, strBDATE);
                    }
                    else
                    {
                        clsOpdNr.INSERT_HappyCall_OPD(clsDB.DbCon, "03", strPANO, "02", strDEPT, strBDATE);
                    }
                }
                if (strSubName == "mnuItem_2")
                {
                    //해피콜(03. 환자안부)
                    if (clsOpdNr.READ_OPD_HAPPYCALL(clsDB.DbCon, "03", strPANO, strDEPT, strBDATE) == true)
                    {
                        clsOpdNr.UPDATE_HappyCall_OPD(clsDB.DbCon, "03", strPANO, "03", strDEPT, strBDATE);
                    }
                    else
                    {
                        clsOpdNr.INSERT_HappyCall_OPD(clsDB.DbCon, "03", strPANO, "03", strDEPT, strBDATE);
                    }
                }
                if (strSubName == "mnuItem_2")
                {
                    //해피콜(04. 설정지움)
                    if (clsOpdNr.READ_OPD_HAPPYCALL(clsDB.DbCon, "03", strPANO, strDEPT, strBDATE) == true)
                    {
                        clsOpdNr.UPDATE_HappyCall_OPD(clsDB.DbCon, "03", strPANO, "", strDEPT, strBDATE);
                    }
                    else
                    {
                        clsOpdNr.INSERT_HappyCall_OPD(clsDB.DbCon, "03", strPANO, "", strDEPT, strBDATE);
                    }
                }
            }
        }

        private void ssView_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strROWID = "";
            string strREMARK = "";

            if (e.Column != 12) return;

            strREMARK = ssView_Sheet1.Cells[e.Row, 12].Text;
            strROWID = ssView_Sheet1.Cells[e.Row, 13].Text;

            if (strROWID == "") return;

            
            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE KOSMOS_OCS.ENDO_JUPMST SET REMARK = '" + strREMARK + "'";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

    }
}
