using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;
using Oracle.DataAccess.Client;

namespace ComEmrBase
{
    /// <summary>
    /// PSMHVB/mtsEmr/frmTextEmrList.frm
    /// </summary>
    public partial class frmEmrBaseEmrListOld : Form
    {
        string mCon = "";
        ContextMenu contextMenu = null;

        public frmEmrBaseEmrListOld()
        {
            InitializeComponent();
        }

        private void frmEmrBaseEmrListOld_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            btnSaveMacro.Visible = MacroAuth();
            MACRO_ADD();

            Set_Dept();
            dtpChartDate1.Value = DateTime.Now;
            MagamView();
        }

        /// <summary>
        /// 상용구 관리자 여부
        /// </summary>
        /// <returns></returns>
        private bool MacroAuth()
        {
            bool rtnVal = false;
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return false; //권한 확인
            }

            OracleDataReader reader = null;
            string SQL = " SELECT 1 AS CNT";
            SQL += ComNum.VBLF + " FROM DUAL";
            SQL += ComNum.VBLF + "WHERE EXISTS";
            SQL += ComNum.VBLF + "(";
            SQL += ComNum.VBLF + " SELECT 1 ";
            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BASCD";
            SQL += ComNum.VBLF + " WHERE GRPCDB = '간호EMR 관리'";
            SQL += ComNum.VBLF + "   AND GRPCD  = 'EMRList 상용구 관리'";
            SQL += ComNum.VBLF + "   AND BASCD  = '" + clsType.User.IdNumber + "'";
            SQL += ComNum.VBLF + ")";

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (SqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return rtnVal;
            }

            if (reader.HasRows)
            {
                rtnVal = true;
            }

            reader.Dispose();

            return rtnVal;
        }

        /// <summary>
        /// 상용구 불러오기
        /// </summary>
        private void MACRO_ADD()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            contextMenu = new ContextMenu();
            OracleDataReader reader = null;
            string SQL = " SELECT NAME";
            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE";
            SQL += ComNum.VBLF + " WHERE GUBUN = 'EMR_LIST_사용자_상용구'";
            SQL += ComNum.VBLF + " ORDER BY SORT";

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
            if (SqlErr.Length > 0)
            {
                ComFunc.MsgBoxEx(this, SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;
            }

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    MenuItem menuItem = new MenuItem(reader.GetValue(0).ToString().Trim());
                    menuItem.Click += MenuItem_Click;
                    contextMenu.MenuItems.Add(menuItem);
                }
            }

            reader.Dispose();
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = (sender as MenuItem);
            FarPoint.Win.Spread.FpSpread spd = contextMenu.SourceControl as FarPoint.Win.Spread.FpSpread;
            if (spd.ActiveSheet.ActiveCell == null)
                return;

            if(spd.ActiveSheet.Cells[spd.ActiveSheet.ActiveRowIndex, 9].Text.Trim().Length == 0)
            {
                spd.ActiveSheet.Cells[spd.ActiveSheet.ActiveRowIndex, 9].Text = (sender as MenuItem).Text.Trim();
                spd.ActiveSheet.Cells[spd.ActiveSheet.ActiveRowIndex, 11].Text = "Y";
            }
            else
            {
                spd.ActiveSheet.Cells[spd.ActiveSheet.ActiveRowIndex, 9].Text += ", " + (sender as MenuItem).Text.Trim();
                spd.ActiveSheet.Cells[spd.ActiveSheet.ActiveRowIndex, 11].Text = "Y";
            }
        }

        void Set_Dept()
        {

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;

            string strDeptCode = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (clsType.User.DrCode.Length > 0)
                {
                    SQL = " SELECT DEPTCODE  ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_DOCTOR ";
                    SQL += ComNum.VBLF + " WHERE SABUN = '" + ComFunc.SetAutoZero(clsType.User.Sabun, 5) + "' ";
                }
                else
                {
                    SQL = " SELECT VALUEV AS DEPTCODE ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PCCONFIG";
                    SQL += ComNum.VBLF + "WHERE IPADDRESS = '" + clsCompuInfo.gstrCOMIP + "'";
                    //SQL += ComNum.VBLF + "WHERE IPADDRESS = '192.168.33.58'";
                    SQL += ComNum.VBLF + "  AND GUBUN = '외래OCS진료과세팅'";
                    SQL += ComNum.VBLF + "  AND CODE  = '간호사_DeptCode' ";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;

                SQL = " SELECT MEDDEPTCD, DEPTKORNAME";
                SQL += ComNum.VBLF + "    FROM KOSMOS_EMR.VIEWBMEDDEPT ";
                SQL += ComNum.VBLF + " WHERE OUTMEDACPYN =  '1' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if(dt.Rows.Count > 0)
                { 
                   cboDept.Items.Add("전  체" + VB.Space(50) + "0");
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["DEPTKORNAME"].ToString().Trim() + VB.Space(50) + dt.Rows[i]["MEDDEPTCD"].ToString().Trim());
                    }
                    cboDept.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;

                if(strDeptCode != "")
                {
                    for(i = 0; i < cboDept.Items.Count; i++)
                    {
                        if(VB.Right(cboDept.Items[i].ToString(), 2) == strDeptCode)
                        {
                            cboDept.SelectedIndex = i;
                            break;
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void MagamView()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strBDATE = dtpChartDate1.Text;
            string strDeptCode = VB.Right(cboDept.Text, 6).Trim();
            string strDrCode = VB.Right(cboDr.Text, 5).Trim();

            btnNameChange1.Visible = false;
            btnNameChange2.Visible = false;

            if(strDeptCode == "PC")
            {
                strDrCode = "6299";
            }
            else if(strDeptCode == "HU")
            {
                strDrCode = "1501";
            }

            txtMagam1.Text = "";
            txtMagam2.Text = "";

            txtMagam1Sabun.Text = "";
            txtMagam2Sabun.Text = "";

            txtMagam1Name.Text = "";
            txtMagam2Name.Text = "";

            btnMagam1.Enabled = true;
            btnMagam1.Text = "오전진료마감";
            btnMagam2.Enabled = true;
            btnMagam2.Text = "오후진료마감";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT GUBUN, WRITESABUN, TO_CHAR(WRITEDATE, 'HH24:MI') WRITEDATE, MAGAMNAME ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_EMR.EMR_LIST_MAGAM";
                SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND DRCODE = '" + strDrCode + "'";
                SQL += ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "BMEDDEPT 조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    switch(dt.Rows[i]["GUBUN"].ToString().Trim())
                    {
                        case "1": // 오전
                            if(dt.Rows[i]["WRITEDATE"].ToString().Trim() != "")
                            {
                                btnMagam1.Text = "오전마감취소";
                                btnMagam1.Enabled = false;
                            }
                            else
                            {
                                btnMagam1.Text = "오전진료마감";
                                btnMagam1.Enabled = true;
                            }

                            txtMagam1.Text = dt.Rows[i]["WRITEDATE"].ToString().Trim();
                            txtMagam1Name.Text = dt.Rows[i]["MAGAMNAME"].ToString().Trim();
                            btnNameChange1.Visible = txtMagam1Name.Text.Trim().Length > 0;
                            txtMagam1Sabun.Text = dt.Rows[i]["WRITESABUN"].ToString().Trim();
                            break;
                        case "2": // 오후
                            if (dt.Rows[i]["WRITEDATE"].ToString().Trim() != "")
                            {
                                btnMagam2.Text = "오후마감취소";
                                btnMagam2.Enabled = false;
                            }
                            else
                            {
                                btnMagam2.Text = "오후진료마감";
                                btnMagam2.Enabled = true;
                            }

                            txtMagam2.Text = dt.Rows[i]["WRITEDATE"].ToString().Trim();
                            txtMagam2Name.Text = dt.Rows[i]["MAGAMNAME"].ToString().Trim();
                            btnNameChange2.Visible = txtMagam2Name.Text.Trim().Length > 0;
                            txtMagam2Sabun.Text = dt.Rows[i]["WRITESABUN"].ToString().Trim();
                            break;
                        //case "3": //야간은 아직 적용안함
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (rdoALL.Checked == true) return;

            if(VB.Left(cboDr.Text, 10).Trim() == "전  체")
            {
                if(VB.Right(cboDept.Text, 6).Trim() == "PC" || VB.Right(cboDept.Text, 6).Trim() == "HU")
                {
                }
                else
                {
                    ComFunc.MsgBoxEx(this, "진료의사를 선택하시기 바랍니다.");
                    return;
                }
            }

            ReadList();
            UNSEEN_LIST();
            MagamView();
        }

        void ReadList(string argPrt = "")
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            DataTable dt2 = null;

            //string cActDate   = string.Empty;

            string cDeptCode1 = string.Empty;
            string cDeptCode2 = string.Empty;
            string cDrCode1   = string.Empty;
            string cDrCode2 = string.Empty;
            string strDRCODE2 = string.Empty;
            string strBDATE  = string.Empty;
            string strDeptCd = string.Empty;
            string strDrCd   = string.Empty;

            string strJUPTIME = string.Empty;

            string strGUBUN_AMPM = string.Empty;

            string strTemp = string.Empty;
            strDeptCd = VB.Right(cboDept.Text, 6).Trim();
            if (strDeptCd == "0")
            {
                strDeptCd = "";

            }
            strDrCd = VB.Right(cboDr.Text, 5).Trim();
            if (strDrCd == "0") {
                strDrCd = "";
            }

            if (strDeptCd == "PC") strDrCd = "6299" ;
            if (strDeptCd == "HU") strDrCd = "1501";

            strBDATE = dtpChartDate1.Text;

            GetDrDpet(strDrCd, ref cDeptCode1, ref cDeptCode2);


            if (rdoAM.Checked == true)
            {
                strGUBUN_AMPM = "1";
            }
            else if (rdoPM.Checked == true)
            {
                strGUBUN_AMPM = "2";
            }


            ssHis_Sheet1.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                if (strDrCd == "1107" || strDrCd == "1125" || strDeptCd == "NS" || strDeptCd == "GS" || strDeptCd == "OS" || strDeptCd == "RM")
                {

                    //GoSub Read_Mst_RU '접수 Master Read(류마티스)

                    //strDept2 = "HD";                 //'AK        인공신장실
                    //cActDate = clsOrdFunction.GstrActDate == null ? "" : clsOrdFunction.GstrActDate;

                    //cDeptCode1 = clsOrdFunction.GstrJupsuDept1 == null ? "" : clsOrdFunction.GstrJupsuDept1;
                    //cDeptCode2 = clsOrdFunction.GstrJupsuDept2 == null ? "" : clsOrdFunction.GstrJupsuDept2;

                    SQL = "  SELECT A.Pano Ptno,A.SName,A.DrCode, A.DeptCode,TO_CHAR(A.JTime,'HH24:MI') JTime1,  a.OcsJin,";
                    SQL += ComNum.VBLF + "      TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE,";
                    SQL += ComNum.VBLF + " (SELECT  TO_CHAR(MAX(LASTDATE),'YYYY-MM-DD') FROM KOSMOS_PMPA.BAS_LASTEXAM WHERE PANO = A.PANO AND DEPTCODE = A.DEPTCODE) AS LASTDATE";
                    SQL += ComNum.VBLF + " , '' ETC_ADD";
                    //     
                    SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER a,KOSMOS_PMPA.OPD_DEPTJEPSU b ";
                    SQL += ComNum.VBLF + "WHERE a.BDate  = TO_DATE('" + dtpChartDate1.Text + "','YYYY-MM-DD')";
                    if (strDeptCd != "")
                    {
                        if (strDrCd == "1107" || strDrCd == "1125")
                        {
                            SQL += ComNum.VBLF + "  AND A.DeptCode ='MD' ";

                        }
                        else
                        {
                            SQL += ComNum.VBLF + "  AND A.DeptCode ='" + strDeptCd + "' ";

                        }
                    }

                    if (strDrCd != "" && strDeptCd != "PC")
                    {
                        SQL += ComNum.VBLF + "  AND a.DrCode    = '" + strDrCd + "'     ";
                    }

                    SQL += ComNum.VBLF + "  AND a.Jin      IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B')  ";

                    //'진료대기순번의 과도착(과접수) 여부
                    SQL += ComNum.VBLF + "  AND a.Pano=b.Pano(+) ";
                    SQL += ComNum.VBLF + "  AND a.DeptCode=b.DeptCode(+) ";
                    SQL += ComNum.VBLF + "  AND a.BDate=b.ActDate(+) ";

                    //SQL += ComNum.VBLF + "  AND A.PANO = C.PANO(+) ";
                    //if (clsOrdFunction.GstrDrCode == "1107")
                    //{
                    //    SQL += ComNum.VBLF + "  AND A.DRCODE = C.DRCODE(+) ";

                    //}
                    //else
                    //{
                    //    SQL += ComNum.VBLF + "  AND A.DEPTCODE = C.DEPTCODE(+) ";

                    //}

                    if (rdoAM.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND trim(A.OcsJin) = '*' ";
                        SQL += ComNum.VBLF + "  AND A.JINTIME <= TO_DATE('" + dtpChartDate1.Text + " 13:30','YYYY-MM-DD HH24:Mi')";
                    }
                    else if (rdoPM.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND trim(A.OcsJin) = '*'  ";
                        SQL += ComNum.VBLF + "  AND A.JINTIME > TO_DATE('" + dtpChartDate1.Text + " 13:30','YYYY-MM-DD HH24:Mi') ";
                    }

                    if (strDrCd == "1107") SQL += ComNum.VBLF + "  AND a.DrCode='" + strDrCd + "' ";
                }
                else
                {
                    //GoSub Read_Mst '접수 Master Read


                    //strDept2 = "HD";                 //'AK        인공신장실
                    //cActDate   = clsOrdFunction.GstrActDate == null ? "" : clsOrdFunction.GstrActDate;
                    //cDeptCode1 = clsOrdFunction.GstrJupsuDept1 == null ? "" : clsOrdFunction.GstrJupsuDept1;
                    //cDeptCode2 = clsOrdFunction.GstrJupsuDept2 == null ? "" : clsOrdFunction.GstrJupsuDept2;
                    //cDeptCode3 = cDeptCode2;

                    //if (cDeptCode2.Trim() == "EM" || cDeptCode2.Trim() == "ER")
                    //{
                    //    cDeptCode2 = "";
                    //    cDeptCode3 = ""; //'~1
                    //}

                    //cDrCode1 = clsOrdFunction.GstrDrCode;
                    //cDrCode2 = VB.Left(clsOrdFunction.GstrDrCode, 2) + "99";   //'공통환자


                    //'Debug
                    SQL = "  SELECT A.Pano Ptno,A.SName,A.DrCode, A.DeptCode,TO_CHAR(A.JTime,'HH24:MI') JTime1,  a.OcsJin,";
                    SQL += ComNum.VBLF + " TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE,";
                    SQL += ComNum.VBLF + " (SELECT TO_CHAR(MAX(LASTDATE),'YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    FROM KOSMOS_PMPA.BAS_LASTEXAM";
                    SQL += ComNum.VBLF + "   WHERE PANO = A.PANO";
                    if (clsOrdFunction.GstrDrCode == "1107")
                    {
                        SQL += ComNum.VBLF + "   AND DRCODE = A.DRCODE";

                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   AND DEPTCODE = A.DEPTCODE";
                    }
                    SQL += ComNum.VBLF + ")  AS LASTDATE";
                    SQL += ComNum.VBLF + " , '' ETC_ADD";
                    //SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER A, KOSMOS_PMPA.BAS_LASTEXAM B ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.OPD_MASTER A";
                    SQL += ComNum.VBLF + "WHERE A.BDate  = TO_DATE('" + dtpChartDate1.Text + "','YYYY-MM-DD')";

                    if (strDeptCd != "")
                    {
                        SQL += ComNum.VBLF + "  AND A.DeptCode ='" + strDeptCd + "' ";
                    }

                    if (strDrCd != "" && strDeptCd != "PC")
                    {
                        SQL += ComNum.VBLF + "  AND A.DrCode    = '" + strDrCd + "'     ";
                    }

                    SQL += ComNum.VBLF + "  AND A.Jin IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K', 'N','I','J','Q','R','S','A','B')  ";


                    SQL += ComNum.VBLF + "   AND A.PANO NOT IN ('81000000', '81000001', '81000002', '81000003', '81000004', '81000005', '81000006', '81000007', '81000008', '81000009', '81000010', '81000011', '81000012') "; //'add

                    if (strDeptCd == "PC")
                    {
                        if (rdoAM.Checked == true)
                        {
                            SQL += ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + dtpChartDate1.Text + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "  AND A.JTIME <= TO_DATE('" + dtpChartDate1.Text + " 13:30','YYYY-MM-DD HH24:Mi') ";
                        }
                        else if (rdoPM.Checked == true)
                        {
                            SQL += ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + dtpChartDate1.Text + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "  AND A.JTIME > TO_DATE('" + dtpChartDate1.Text + " 13:30','YYYY-MM-DD HH24:Mi') ";
                            SQL += ComNum.VBLF + "  AND A.JTIME < TO_DATE('" + dtpChartDate1.Text + " 18:30','YYYY-MM-DD HH24:Mi') ";
                        }
                        else if (rdoNT.Checked == true)
                        {
                            SQL += ComNum.VBLF + "  AND A.BDATE = TO_DATE('" + dtpChartDate1.Text + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "  AND A.JTIME >= TO_DATE('" + dtpChartDate1.Text + " 18:30','YYYY-MM-DD HH24:Mi') ";

                        }
                    }
                    else
                    {
                        if (rdoAM.Checked == true)
                        {
                            SQL += ComNum.VBLF + "  AND trim(A.OcsJin) = '*' ";
                            SQL += ComNum.VBLF + "  AND A.JINTIME <= TO_DATE('" + dtpChartDate1.Text + " 13:30','YYYY-MM-DD HH24:Mi') ";
                        }
                        else if (rdoPM.Checked == true)
                        {
                            SQL += ComNum.VBLF + "  AND trim(A.OcsJin) = '*'  ";
                            SQL += ComNum.VBLF + "  AND A.JINTIME > TO_DATE('" + dtpChartDate1.Text + " 13:30','YYYY-MM-DD HH24:Mi') ";
                            SQL += ComNum.VBLF + "  AND A.JINTIME < TO_DATE('" + dtpChartDate1.Text + " 18:30','YYYY-MM-DD HH24:Mi') ";
                        }
                        else if (rdoNT.Checked == true)
                        {
                            SQL += ComNum.VBLF + "  AND trim(A.OcsJin) = '*'  ";
                            SQL += ComNum.VBLF + "  AND A.JINTIME >= TO_DATE('" + dtpChartDate1.Text + " 18:30','YYYY-MM-DD HH24:Mi') ";
                        }
                    }
                }


                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT A.PTNO, B.SNAME, A.DRCODE, C.DRDEPT1, JUPTIME JTIME1, '' OCSJIN, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, ";
                if (argPrt == "1") {
                    SQL += ComNum.VBLF + " TO_CHAR(A.BDATE,'YYYY-MM-DD') LASTDATE, DECODE(A.ETC_ADD, '2', '', A.ETC_ADD) ETC_ADD";
                }
                else
                {
                    SQL += ComNum.VBLF + " TO_CHAR(A.BDATE,'YYYY-MM-DD') LASTDATE, A.ETC_ADD";

                }

                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMR_LIST A, KOSMOS_PMPA.BAS_PATIENT B, KOSMOS_PMPA.BAS_DOCTOR C";
                SQL += ComNum.VBLF + " Where a.PtNo = b.Pano";
                SQL += ComNum.VBLF + " AND A.DRCODE = C.DRCODE";
                SQL += ComNum.VBLF + " AND A.BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD')";
                if(strDeptCd == "PC")
                {
                    SQL += ComNum.VBLF + "  AND C.DRDEPT1 = 'PC' ";

                }
                else
                {
                    SQL += ComNum.VBLF + " AND A.DRCODE = '" + strDrCd + "'";

                }

                SQL += ComNum.VBLF + " AND A.ETC_ADD = '2'";
                if (rdoAM.Checked == true) {
                    SQL += ComNum.VBLF + "  AND A.GUBUN_AMPM = '1' ";
                }
                else if(rdoPM.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND A.GUBUN_AMPM = '2' ";
                }

                if (argPrt == "1")
                {
                    SQL += ComNum.VBLF + " UNION ALL ";
                    SQL += ComNum.VBLF + " SELECT A.PTNO, B.SNAME, A.DRCODE, C.DRDEPT1, JUPTIME JTIME1, '' OCSJIN, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, ";
                    SQL += ComNum.VBLF + " TO_CHAR(A.BDATE,'YYYY-MM-DD') LASTDATE, A.ETC_ADD";
                    SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMR_LIST A, KOSMOS_PMPA.BAS_PATIENT B, KOSMOS_PMPA.BAS_DOCTOR C";
                    SQL += ComNum.VBLF + " Where a.PtNo = b.Pano";
                    SQL += ComNum.VBLF + " AND A.DRCODE = C.DRCODE";
                    SQL += ComNum.VBLF + " AND A.BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + " AND A.DRCODE = '" + strDrCd + "'";
                    SQL += ComNum.VBLF + " AND A.ETC_ADD = '1'";

                    if (rdoAM.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND A.GUBUN_AMPM = '1' ";

                    }
                    else if (rdoPM.Checked == true)
                    {
                        SQL += ComNum.VBLF + "  AND A.GUBUN_AMPM = '2' ";

                    }
                }

                SQL += ComNum.VBLF + "ORDER BY ETC_ADD DESC, 2 ";   //'11 ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //'List에 접수(예약,접수,공통환자 Display)
                string strPtNo = "";
                string strOldETCADD = "";
                int intRow = 0;
                FarPoint.Win.Spread.CellType.TextCellType fTextType = new FarPoint.Win.Spread.CellType.TextCellType();
                fTextType.Static = true;


                int i;
                if (argPrt == "")
                {
                    ssHis_Sheet1.RowCount = dt.Rows.Count;
                    if (VB.Val(textBox1.Text) != 0)
                    {
                        ssHis_Sheet1.RowCount = dt.Rows.Count + (int)VB.Val(textBox1.Text);
                        for (i = 0; i < ssHis_Sheet1.RowCount; i++)
                        {
                            if (i > dt.Rows.Count)
                            {
                                ssHis_Sheet1.Cells[i, 12].Text = "2";

                            }
                        }
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    txtMagam1.Clear();
                    txtMagam1Name.Clear();
                    txtMagam1Sabun.Clear();
                    txtMagam2.Clear();
                    txtMagam2Name.Clear();
                    txtMagam2Sabun.Clear();

                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    if (argPrt == "") ReadUserList();
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (argPrt == "1")
                    {
                        ssHis_Sheet1.RowCount = ssHis_Sheet1.RowCount + 1;
                        intRow = ssHis_Sheet1.RowCount - 1;
                        if (strOldETCADD != dt.Rows[i]["ETC_ADD"].ToString().Trim() && i > 1)
                        {
                            ssHis_Sheet1.AddSpanCell(intRow, 0, intRow, 12);
                            ssHis_Sheet1.Rows[intRow].Label = " ";

                            ssHis_Sheet1.RowCount = ssHis_Sheet1.RowCount + 1;
                            intRow = ssHis_Sheet1.RowCount - 1;
                            ssHis_Sheet1.AddSpanCell(intRow, 0, intRow, 12);
                            fTextType.Static = true;
                            ssHis_Sheet1.Cells[intRow, 0].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                            ssHis_Sheet1.Cells[intRow, 0].CellType = fTextType;
                            ssHis_Sheet1.Cells[intRow, 0].VerticalAlignment = CellVerticalAlignment.Center;
                            ssHis_Sheet1.Cells[intRow, 0].HorizontalAlignment = CellHorizontalAlignment.Center;
                            ssHis_Sheet1.Cells[intRow, 0].Text = "사용자 입력 목록";
                            ssHis_Sheet1.Rows[intRow].BackColor = Color.FromArgb(200, 200, 200);
                            ssHis_Sheet1.Rows[intRow].Label = " ";

                            ssHis_Sheet1.RowCount = ssHis_Sheet1.RowCount + 1;
                            intRow = ssHis_Sheet1.RowCount - 1;
                            ssHis_Sheet1.AddSpanCell(intRow, 0, intRow, 12);
                            ssHis_Sheet1.Rows[intRow].Label = " ";

                            ssHis_Sheet1.RowCount = ssHis_Sheet1.RowCount + 1;
                            intRow = ssHis_Sheet1.RowCount - 1;
                            ssHis_Sheet1.Rows[intRow].Label = " ";
                        }
                    }
                    else
                    {
                        intRow = i;
                    }

                    strPtNo = dt.Rows[i]["Ptno"].ToString();
                    ssHis_Sheet1.Cells[intRow, 1].Text = strPtNo;
                    ssHis_Sheet1.Cells[intRow, 2].Text = dt.Rows[i]["Sname"].ToString();
                    //진료의
                    strDRCODE2 = dt.Rows[i]["DrCode"].ToString();
                    ssHis_Sheet1.Cells[intRow, 3].Text = GetDrNm(strDRCODE2);

                    //ssHis_Sheet1.Cells[intRow, 3] // 과초진\
                    //TODO
                    if(READ_DEPT_CHOJEA(dt.Rows[i]["Ptno"].ToString().Trim(), dt.Rows[i]["DEPTCODE"].ToString().Trim(), dt.Rows[i]["DrCode"].ToString().Trim()) != "")
                    {
                        ssHis_Sheet1.Cells[intRow, 4].Text = "과신환";
                    }
                    else
                    {
                        ssHis_Sheet1.Cells[intRow, 4].Text = "";
                    }

                    if (dt.Rows[i]["ETC_ADD"].ToString().Trim() == "2")
                    {
                        ssHis_Sheet1.Rows[intRow].BackColor = Color.FromArgb(200, 200, 200);
                    }

                    ssHis_Sheet1.Cells[intRow, 5].Text = dt.Rows[i]["JTime1"].ToString();
                    strJUPTIME = ssHis_Sheet1.Cells[intRow, 5].Text;

                    //마감 이후 접수 환자 색 표시
                    if(rdoAM.Checked == true)
                    {
                        if(txtMagam1.Text.Trim() != "" && strJUPTIME != "")
                        {
                            if(VB.IsDate(dtpChartDate1.Text + " " + txtMagam1.Text.Trim()) &&
                               VB.IsDate(dtpChartDate1.Text + " " + strJUPTIME))
                            {
                                if(DateTime.Compare(Convert.ToDateTime(dtpChartDate1.Text + " " + txtMagam1.Text.Trim()) , 
                                                    Convert.ToDateTime(dtpChartDate1.Text + " " + strJUPTIME)) < 0)
                                {
                                    ssHis_Sheet1.Rows[intRow].ForeColor = Color.FromArgb(0, 0, 255);
                                }
                            }
                        }
                    }
                    else if(rdoPM.Checked == true)
                    {
                        if (txtMagam2.Text.Trim() != "" && strJUPTIME != "")
                        {
                            if (VB.IsDate(dtpChartDate1.Text + " " + txtMagam2.Text.Trim()) &&
                               VB.IsDate(dtpChartDate1.Text + " " + strJUPTIME))
                            {
                                if (DateTime.Compare(Convert.ToDateTime(dtpChartDate1.Text + " " + txtMagam2.Text.Trim()) ,
                                                     Convert.ToDateTime(dtpChartDate1.Text + " " + strJUPTIME)) < 0)
                                {
                                    ssHis_Sheet1.Rows[intRow].ForeColor = Color.FromArgb(0, 0, 255);
                                }
                            }
                        }
                    }

                    #region EMR 차트 정보 읽어오기
                    //6/7 EMR
                    SQL = "SELECT A.CHARTTIME, A.FORMNO, FORMNAME";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.EMRXMLMST A";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_EMR.AEMRFORM B";
                    SQL = SQL + ComNum.VBLF + "   ON A.FORMNO = B.FORMNO";
                    SQL = SQL + ComNum.VBLF + "  AND B.UPDATENO > 0";
                    SQL = SQL + ComNum.VBLF + "  AND B.OLDGB = '1'";
                    SQL = SQL + ComNum.VBLF + "  AND (B.GRPFORMNO = 1000 OR B.FORMNO = 963)";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_EMR.AEMRGRPFORM C";
                    SQL = SQL + ComNum.VBLF + "   ON C.GRPFORMNO = B.GRPFORMNO";
                    SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE = '" + dt.Rows[i]["BDATE"].ToString().Replace("-", "") + "'";
                    SQL = SQL + ComNum.VBLF + "  AND A.MEDFRDATE = '" + dt.Rows[i]["BDATE"].ToString().Replace("-", "") + "'";
                    SQL = SQL + ComNum.VBLF + "  AND A.MEDDEPTCD = '" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "'";
                    if(strDeptCd == "PC")
                    {

                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND A.MEDDRCD = '" + dt.Rows[i]["DrCode"].ToString() + "'";
                    }

                    SQL = SQL + ComNum.VBLF + "UNION ALL";
                    SQL = SQL + ComNum.VBLF + "SELECT A.CHARTTIME, A.FORMNO, FORMNAME";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_EMR.AEMRCHARTMST A";
                    SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.AEMRFORM B";
                    SQL = SQL + ComNum.VBLF + "     ON A.FORMNO = B.FORMNO";
                    SQL = SQL + ComNum.VBLF + "    AND A.UPDATENO = B.UPDATENO";
                    SQL = SQL + ComNum.VBLF + "    AND (B.GRPFORMNO = 1000 OR B.FORMNO = 963)";
                    if (strDeptCd == "PC")
                    {

                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_OCS.OCS_DOCTOR C";
                        SQL = SQL + ComNum.VBLF + "     ON TRIM(LTRIM(C.SABUN, '0')) = A.CHARTUSEID";
                        SQL = SQL + ComNum.VBLF + "    AND C.DRCODE = '" + dt.Rows[i]["DrCode"].ToString() + "'";
                    }
          
                    SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "  AND A.CHARTDATE = '" + dt.Rows[i]["BDATE"].ToString().Replace("-", "") + "'";
                    SQL = SQL + ComNum.VBLF + "  AND A.MEDFRDATE = '" + dt.Rows[i]["BDATE"].ToString().Replace("-", "") + "'";
                    SQL = SQL + ComNum.VBLF + "  AND A.MEDDEPTCD = '" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "'";
             

                    SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if(dt2.Rows.Count == 0)
                    {
                        ssHis_Sheet1.Cells[intRow, 6].Text = "";
                        ssHis_Sheet1.Cells[intRow, 7].Text = "";
                    }
                    else
                    {
                        ssHis_Sheet1.Cells[intRow, 6].Text = dt2.Rows[0]["FORMNAME"].ToString().Trim();
                        ssHis_Sheet1.Cells[intRow, 7].Text = ComFunc.FormatStrToDate(VB.Left(dt2.Rows[0]["CHARTTIME"].ToString().Trim(), 4), "M");
                    }

                    dt2.Dispose();
                    dt2 = null;
                    #endregion

                    if (dt.Rows[i]["OcsJin"].ToString().Trim() == "")
                    {
                        ssHis_Sheet1.Cells[intRow, 9].Text = "진료전";
                    }
                    else
                    {
                        ssHis_Sheet1.Cells[intRow, 9].Text = "";
                    }

                    READBigo(strPtNo, strBDATE, strJUPTIME, ssHis, strDeptCd == "PC" ? strDRCODE2 : strDrCd, strGUBUN_AMPM, dt.Rows[i]["ETC_ADD"].ToString(), intRow);

                    ssHis_Sheet1.Rows[intRow].Height = ssHis_Sheet1.Rows[intRow].GetPreferredHeight() + 5;

                    strOldETCADD = dt.Rows[i]["ETC_ADD"].ToString().Trim();

                    if (argPrt == "1" && strOldETCADD == "1")
                    {
                        ssHis_Sheet1.Rows[intRow].Label = " ";
                        strTemp = ssHis_Sheet1.Cells[intRow, 9].Text;
                        ssHis_Sheet1.AddSpanCell(intRow, 3, 7, intRow);
                        ssHis_Sheet1.Cells[intRow, 3].Text = strTemp;

                        fTextType.Static = false;
                        ssHis_Sheet1.Cells[intRow, 3].CellType = fTextType;
                        ssHis_Sheet1.Cells[intRow, 3].HorizontalAlignment  = CellHorizontalAlignment.Left;
                        ssHis_Sheet1.Cells[intRow, 3].VerticalAlignment = CellVerticalAlignment.Center;
                        fTextType.Multiline = true;
                    }

                    ssHis_Sheet1.Cells[intRow, 13].Text = READ_SAVE_YN(strPtNo, strDrCd, dt.Rows[i]["BDATE"].ToString().Trim(),
                        strGUBUN_AMPM);

                }

                dt.Dispose();
                dt = null;
                fTextType.Dispose();
                fTextType = null;

                ssHis_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (argPrt == "") ReadUserList();

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private string READ_DEPT_CHOJEA(string ArgPano, string ArgDeptCode, string ArgDrCode)
        {
            string rtnVal = "";
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "";
                SQL = " SELECT ROWID FROM KOSMOS_PMPA.OPD_MASTER";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE = '" + ArgDeptCode + "'";
                SQL = SQL + ComNum.VBLF + "    AND ACTDATE < TRUNC(SYSDATE) ";
                if (ArgDeptCode == "MD")
                {
                    if (ArgDrCode == "1107")
                    {
                        SQL = SQL + ComNum.VBLF + " AND DRCODE = '1107' ";
                    }
                    else if (ArgDrCode == "1125")
                    {
                        SQL = SQL + ComNum.VBLF + " AND DRCODE = '1125'  ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "  AND DRCODE NOT IN ('1107','1125') ";
                    }
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = "";
                }
                else
                {
                    rtnVal = "과초진";
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
                return rtnVal;
            }
        }

        void ReadUserList()
        {

            string strDeptCd = "";
            string strDrCd = "";
            string strBDATE = "";
            string strPtNo = "";

            string strJUPTIME = "";

            string strGUBUN_AMPM = "";
            if (rdoAM.Checked == true)
            {
                strGUBUN_AMPM = "1";
            }
            else if (rdoPM.Checked == true)
            {
                strGUBUN_AMPM = "2";
            }

            strDeptCd = VB.Right(cboDept.Text, 6).Trim();
            if (strDeptCd == "0")
            {
                strDeptCd = "";

            }
            strDrCd = VB.Right(cboDr.Text, 5).Trim();
            if (strDrCd == "0")
            {
                strDrCd = "";
            }

            if (strDeptCd == "PC") strDrCd = "6299";

            ssHisUser_Sheet1.RowCount                 = 0;

            strBDATE = dtpChartDate1.Text;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT A.PTNO, B.SNAME, A.DRCODE, A.DEPTCODE, JUPTIME JTIME1, '' OCSJIN, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, ";
                SQL += ComNum.VBLF + " TO_CHAR(A.BDATE,'YYYY-MM-DD') LASTDATE, A.ETC_ADD";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMR_LIST A, KOSMOS_PMPA.BAS_PATIENT B, KOSMOS_PMPA.BAS_DOCTOR C";
                SQL += ComNum.VBLF + " Where a.PtNo = b.Pano";
                SQL += ComNum.VBLF + " AND A.DRCODE = C.DRCODE";
                SQL += ComNum.VBLF + " AND A.BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + " AND A.DRCODE = '" + strDrCd + "'";
                SQL += ComNum.VBLF + " AND A.ETC_ADD = '1'";

                if(rdoAM.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND A.GUBUN_AMPM = '1' ";
                }
                else if(rdoPM.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND A.GUBUN_AMPM = '2' ";
                }

                SQL += ComNum.VBLF + " ORDER BY DISPSEQ ";
                //SQL += ComNum.VBLF + " ORDER BY 2 ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                FarPoint.Win.Spread.CellType.TextCellType cellType = new FarPoint.Win.Spread.CellType.TextCellType();
                if(VB.Val(textBox1.Text) != 0)
                {
                    ssHisUser_Sheet1.RowCount = dt.Rows.Count + (int) VB.Val(textBox1.Text);
                    for (i = 0; i < ssHisUser_Sheet1.RowCount; i++)
                    {
                        cellType.TextOrientation = FarPoint.Win.TextOrientation.TextHorizontal;
                        ssHisUser_Sheet1.Cells[i, 1, i, 7].CellType = cellType;
                        ssHisUser_Sheet1.Cells[i, 1, i, 7].HorizontalAlignment = CellHorizontalAlignment.Center;
                        ssHisUser_Sheet1.Cells[i, 1, i, 7].VerticalAlignment = CellVerticalAlignment.Center;

                        ssHisUser_Sheet1.Cells[i, 8].CellType = cellType;
                        ssHisUser_Sheet1.Cells[i, 8].HorizontalAlignment = CellHorizontalAlignment.Left;
                        ssHisUser_Sheet1.Cells[i, 8].VerticalAlignment = CellVerticalAlignment.Center;
                    }
                }
                else
                {
                    ssHisUser_Sheet1.RowCount = dt.Rows.Count;
                    ssHisUser_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strPtNo = dt.Rows[i]["Ptno"].ToString();
                    ssHisUser_Sheet1.Cells[i, 1].Text = strPtNo;
                    ssHisUser_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssHisUser_Sheet1.Cells[i, 3].Text = GetDrNm(dt.Rows[i]["DrCode"].ToString().Trim());

                    if (READ_DEPT_CHOJEA(dt.Rows[i]["Ptno"].ToString().Trim(), dt.Rows[i]["DEPTCODE"].ToString().Trim(), dt.Rows[i]["DrCode"].ToString().Trim()) != "")
                    {
                        ssHisUser_Sheet1.Cells[i, 4].Text = "과신환";
                    }
                    else
                    {
                        ssHisUser_Sheet1.Cells[i, 4].Text = "";
                    }


                    if (dt.Rows[i]["ETC_ADD"].ToString().Trim() == "1")
                    {
                        ssHisUser_Sheet1.Rows[i].BackColor = Color.FromArgb(200, 200, 200);
                    }

                    ssHisUser_Sheet1.Cells[i, 4].Text = dt.Rows[i]["JTime1"].ToString();
                    strJUPTIME = ssHisUser_Sheet1.Cells[i, 4].Text;

                    //마감 이후 접수 환자 색 표시
                    if (rdoAM.Checked == true)
                    {
                        if (txtMagam1.Text.Trim() != "" && strJUPTIME != "")
                        {
                            if (VB.IsDate(dtpChartDate1.Text + " " + txtMagam1.Text.Trim()) &&
                               VB.IsDate(dtpChartDate1.Text + " " + strJUPTIME))
                            {
                                if (DateTime.Compare(Convert.ToDateTime(dtpChartDate1.Text + " " + txtMagam1.Text.Trim()),
                                                     Convert.ToDateTime(dtpChartDate1.Text + " " + strJUPTIME)) < 0 )
                                {
                                    ssHisUser_Sheet1.Rows[i].ForeColor = Color.FromArgb(0, 0, 255);
                                }
                            }
                        }
                    }
                    else if (rdoPM.Checked == true)
                    {
                        if (txtMagam2.Text.Trim() != "" && strJUPTIME != "")
                        {
                            if (VB.IsDate(dtpChartDate1.Text + " " + txtMagam2.Text.Trim()) &&
                               VB.IsDate(dtpChartDate1.Text + " " + strJUPTIME))
                            {
                                if (DateTime.Compare(Convert.ToDateTime(dtpChartDate1.Text + " " + txtMagam2.Text.Trim()),
                                                     Convert.ToDateTime(dtpChartDate1.Text + " " + strJUPTIME)) < 0)
                                {
                                    ssHisUser_Sheet1.Rows[i].ForeColor = Color.FromArgb(0, 0, 255);
                                }
                            }
                        }
                    }

                    READBigo(strPtNo, strBDATE, strJUPTIME, ssHisUser, strDrCd, strGUBUN_AMPM, dt.Rows[i]["ETC_ADD"].ToString().Trim(), i);
                    ssHisUser_Sheet1.Rows[i].Height = ssHisUser_Sheet1.Rows[i].GetPreferredHeight() + 5;
                }


                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }


        }

        string READ_SAVE_YN(string argPTNO, string ArgDrCode, string argDATE, string argAMPM)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT PTNO ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMR_LIST";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "' ";
                SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "      AND GUBUN_AMPM = '" + argAMPM + "' ";
                SQL += ComNum.VBLF + "      AND DRCODE = '" + ArgDrCode + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                rtnVal = "●";
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                    return rtnVal;
            }
        }

        void READBigo(string argPTNO, string ArgBDate, string argJUPTIME, object argOBJ, string ArgDrCode, string argAMPM, string argETCADD, int intRow)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT SAYU, BIGO, ROWID";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMR_LIST ";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "'";
                SQL += ComNum.VBLF + "   AND BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND DRCODE = '" + ArgDrCode + "' ";
                SQL += ComNum.VBLF + "   AND GUBUN_AMPM = '" + argAMPM + "' ";
                if(argETCADD == "1")
                {
                    SQL += ComNum.VBLF + "       AND ETC_ADD = '1' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "       AND (ETC_ADD <> '1' OR ETC_ADD IS NULL) ";
                }
                if (argJUPTIME != "")
                {
                    SQL += ComNum.VBLF + "    AND JUPTIME = '" + argJUPTIME + "' ";
                }

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
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ((FarPoint.Win.Spread.FpSpread)argOBJ).ActiveSheet.Cells[intRow, 8].Text = dt.Rows[0]["SAYU"].ToString().Trim();
                ((FarPoint.Win.Spread.FpSpread)argOBJ).ActiveSheet.Cells[intRow, 9].Text = dt.Rows[0]["BIGO"].ToString().Trim();
                ((FarPoint.Win.Spread.FpSpread)argOBJ).ActiveSheet.Cells[intRow, 10].Text = dt.Rows[0]["ROWID"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }


        string GetDrNm(string strDrCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT DRNAME FROM KOSMOS_OCS.OCS_DOCTOR";
                SQL += ComNum.VBLF +"WHERE DRCODE = '" + strDrCode + "'";
                SQL += ComNum.VBLF +"  AND GBOUT = 'N'";
                SQL += ComNum.VBLF + "  AND GRADE = '1'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["DrName"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        void UNSEEN_LIST()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //20-02-22 마감 안해도 참고사항 볼수 있게하기 위해서 추가함.
            //string strPtNo = "'00000000'";
            string strPtNo = string.Empty;

            int i = 0;

            string strBDATE = dtpChartDate1.Text;
            string strDeptCd = VB.Right(cboDept.Text, 6).Trim();
            string strDrCd = VB.Right(cboDr.Text, 5).Trim();

            string strGUBUN_AMPM = "";

            if (strDeptCd == "0")
            {
                strDeptCd = "";
            }

            if(strDrCd == "0")
            {
                strDrCd = "";
            }

            if (strDeptCd == "PC") strDrCd = "6299";

            if(rdoAM.Checked == true)
            {
                strGUBUN_AMPM = "1";
            }
            else if(rdoPM.Checked == true)
            {
                strGUBUN_AMPM = "2";
            }

            for(i = 0; i < ssHis_Sheet1.RowCount; i++)
            {
                strPtNo += "'" + ssHis_Sheet1.Cells[i, 0].Text.Trim() + "',";
            }

            if (strPtNo != "") strPtNo = VB.Mid(strPtNo, 1, strPtNo.Length - 1);

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT A.PTNO, B.SNAME, C.DRNAME, A.JUPTIME, A.BIGO, A.SAYU, A.ROWID";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMR_LIST A, KOSMOS_PMPA.BAS_PATIENT B, KOSMOS_PMPA.BAS_DOCTOR C";
                SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND A.PTNO = B.PANO";
                SQL += ComNum.VBLF + "   AND A.DRCODE = C.DRCODE";
                SQL += ComNum.VBLF + "   AND A.DRCODE = '" + strDrCd + "' ";
                SQL += ComNum.VBLF + "   AND A.GUBUN_AMPM = '" + strGUBUN_AMPM + "' ";
                SQL += ComNum.VBLF + "   AND ETC_ADD NOT IN ('1')";
                if(strPtNo != "") SQL += ComNum.VBLF + "     AND A.PTNO NOT IN (" + strPtNo + ")";

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
                    CHOJAE();
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    ssHis_Sheet1.RowCount = ssHis_Sheet1.RowCount + 1;

                    ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                    ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                    ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["JUPTIME"].ToString().Trim();
                    ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["SAYU"].ToString().Trim();
                    ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["BIGO"].ToString().Trim();
                    ssHis_Sheet1.Cells[ssHis_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssHis_Sheet1.Rows[ssHis_Sheet1.RowCount - 1].BackColor = Color.FromArgb(250, 221, 221) ;
                }

                ssHis_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                dt.Dispose();
                dt = null;

                CHOJAE();
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void CHOJAE()
        {
            //과신환
            int intCount = 0;
            //재진
            int intCount2 = 0;

            for (int i = 0; i < ssHis_Sheet1.RowCount; i++)
            {
                if (ssHis_Sheet1.Cells[i, 4].Text.Trim().Equals("과신환"))
                {
                    intCount++;
                }
                else
                {
                    intCount2++;
                }
            }

            lblCho.Text = "초진: " + intCount;
            lblJae.Text = "재진: " + intCount2;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (Save_Data() == false) return;

            ReadList();
            UNSEEN_LIST();
        }

        bool Save_Data()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            //DataTable dt = null;

            string strBDATE = "";
            string strPtNo = "";
            string strJUPTIME = "";
            string strBigo = "";

            string strROWID = "";
            string strChange = "";

            string strDRCODE2 = "";    //'pc 과를 위한 변수 (pc는 과전체의사 조회하기 때문)

            string strDeptCode = "";

            string strDrCode = "";
            string strGUBUN_AMPM = "";
            string strETCADD = "";
            string strSayu = "";

            string strTEMP1 = "";
            string strTEMP2 = "";
            string strTEMP3 = "";

            int i = 0;

            for (i = 0; i < ssHis_Sheet1.RowCount; i++)
            {
                strTEMP3 = ssHis_Sheet1.Cells[i, 1].Text.Trim();
                strTEMP1 = ssHis_Sheet1.Cells[i, 6].Text.Trim();
                strTEMP2 = ssHis_Sheet1.Cells[i, 8].Text.Trim();
                if(strTEMP3 != "" && strTEMP1 == "" && strTEMP2 == "")
                {
                    ComFunc.MsgBoxEx(this, "'기록지', '누락사유' 둘 다 공란일 경우 저장이 안됩니다. 내용을 입력하신 후 저장을 하시기 바랍니다.");
                    return false;
                }
            }

            strDeptCode = VB.Right(cboDept.Text, 6).Trim();

            if(strDeptCode != "PC" && strDeptCode != "HU")
            {
                if (VB.Left(cboDr.Text, 10) == "전  체")
                {
                    ComFunc.MsgBoxEx(this, "진료의사를 선택하시기 바랍니다.");
                    return false;
                }

                if (strDrCode == "0")
                {
                    ComFunc.MsgBoxEx(this, "진료의사 선택이 되질 않았습니다.");
                    return false;
                }
            }

            strDrCode = VB.Right(cboDr.Text, 5).Trim();

            if (strDeptCode == "PC") strDrCode = "6299";
            if (strDeptCode == "HU") strDrCode = "1501";

            if(rdoALL.Checked == true)
            {
                ComFunc.MsgBoxEx(this, "오전/오후 구분을 선택하시기 바랍니다.");
                return false;
            }

            if(rdoAM.Checked == true)
            {
                strGUBUN_AMPM = "1";
            }
            else if(rdoPM.Checked == true)
            {
                strGUBUN_AMPM = "2";
            }

            strBDATE = dtpChartDate1.Text;

            if(ReadWriteMagam(strBDATE, strDrCode, strGUBUN_AMPM, strDeptCode) == true)
            {
                ComFunc.MsgBoxEx(this, "저장 안됨" + ComNum.VBLF + " 기록실 문의 요망");
                return false;
            }


            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strMsg = string.Empty;

                for (i = 0; i < ssHis_Sheet1.RowCount; i++)
                {
                    strPtNo = ssHis_Sheet1.Cells[i, 1].Text.Trim();
                    strDRCODE2 = GetDrCD(ssHis_Sheet1.Cells[i, 3].Text.Trim());
                    strJUPTIME = ssHis_Sheet1.Cells[i, 5].Text.Trim();
                    strSayu = ssHis_Sheet1.Cells[i, 8].Text.Trim();
                    strBigo = ssHis_Sheet1.Cells[i, 9].Text.Trim();
                    strROWID = ssHis_Sheet1.Cells[i, 10].Text.Trim();
                    strChange = ssHis_Sheet1.Cells[i, 11].Text.Trim();
                    strETCADD = ssHis_Sheet1.Cells[i, 12].Text.Trim();

                    if(strPtNo != "")
                    {
                        if (strROWID == "")
                        {
                            #region 중복 점검
                            //// '2016-08-11 김경동 작업
                            //SQL = " SELECT PTNO, B.SNAME, BDATE, A.GUBUN_AMPM, A.BIGO";
                            //SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMR_LIST A";
                            //SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_PMPA.BAS_PATIENT B";
                            //SQL += ComNum.VBLF + "     ON A.PTNO = B.PANO";
                            //SQL += ComNum.VBLF + " WHERE A.BDate = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                            //SQL += ComNum.VBLF + "   AND A.PTNO = '" + strPtNo + "' ";
                            //SQL += ComNum.VBLF + "   AND A.DEPTCODE = '" + strDeptCode + "' ";
                            //SQL += ComNum.VBLF + "   AND GUBUN_AMPM = '" + strGUBUN_AMPM + "'";
                            //SQL += ComNum.VBLF + "   AND (ETC_ADD <> '1' OR ETC_ADD IS NULL)";
                            //SQL += ComNum.VBLF + "   AND DISPSEQ <> " + i;
                            //SQL += ComNum.VBLF + "ORDER BY A.WRITEDATE";

                            //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                            //if (SqlErr != "")
                            //{
                            //    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            //    Cursor.Current = Cursors.Default;
                            //    return false;
                            //}

                            //if (dt.Rows.Count > 1)
                            //{
                            //    strMsg = "등록번호 [" + strPtNo + "]  중복입력입니다.";
                            //    strMsg += ComNum.VBLF + ("성명 : " + dt.Rows[0]["SNAME"].ToString().Trim());
                            //}

                            //dt.Dispose();
                            //dt = null;


                            //if (!string.IsNullOrWhiteSpace(strMsg))
                            //{
                            //    ComFunc.MsgBoxEx(this, strMsg);
                            //    continue;
                            //}
                            #endregion

                            SQL = " INSERT INTO KOSMOS_EMR.EMR_LIST( ";
                            SQL = SQL + ComNum.VBLF + " BDATE, PTNO, JUPTIME, WRITEDATE, ";
                            SQL = SQL + ComNum.VBLF + " WRITESABUN, BIGO, DRCODE, GUBUN_AMPM, ";
                            SQL = SQL + ComNum.VBLF + " ETC_ADD, SAYU, DEPTCODE, DISPSEQ) VALUES (";
                            SQL = SQL + ComNum.VBLF + "TO_DATE('" + strBDATE + "','YYYY-MM-DD'), '" + strPtNo + "','" + strJUPTIME + "', SYSDATE, ";
                            SQL = SQL + ComNum.VBLF + clsType.User.IdNumber + ",'" + strBigo + "','" + (strDeptCode == "PC" ? strDRCODE2 : strDrCode) + "','" + strGUBUN_AMPM + "',";
                            SQL = SQL + ComNum.VBLF + "'" + strETCADD + "','" + strSayu + "','" + strDeptCode + "', " + i + " )";
                        }
                        else
                        {
                            SQL = " UPDATE KOSMOS_EMR.EMR_LIST SET ";
                            //'SQL = SQL + ComNum.VBLF + " WRITEDATE = SYSDATE, "
                            //'SQL = SQL + ComNum.VBLF + " WRITESABUN = " + GnJobSabun + ", "
                            //'SQL = SQL + ComNum.VBLF + "  GUBUN_AMPM = '" + strGUBUN_AMPM + "', "
                            SQL = SQL + ComNum.VBLF + " DISPSEQ = " + i + ", ";
                            SQL = SQL + ComNum.VBLF + " SAYU = '" + strSayu + "', ";
                            SQL = SQL + ComNum.VBLF + " BIGO = '" + strBigo + "' ";
                            SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND DRCODE = '" + (strDeptCode == "PC" ? strDRCODE2 : strDrCode) + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND GUBUN_AMPM = '" + strGUBUN_AMPM + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND (ETC_ADD <> '1' OR ETC_ADD IS NULL) ";
                            //'SQL = SQL & vbCr & " WHERE ROWID = '" & strROWID & "' "
                            //'SQL = SQL & vbCr & "    AND WRITESABUN = " & GnJobSabun
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                for (i = 0; i < ssHisUser_Sheet1.RowCount; i++)
                {
                    strMsg = string.Empty;

                    strPtNo = ssHisUser_Sheet1.Cells[i, 1].Text.Trim();
                    strJUPTIME = ssHisUser_Sheet1.Cells[i, 5].Text.Trim();
                    strSayu = ssHisUser_Sheet1.Cells[i, 8].Text.Trim();
                    strBigo = ssHisUser_Sheet1.Cells[i, 9].Text.Trim();
                    strROWID = ssHisUser_Sheet1.Cells[i, 10].Text.Trim();
                    strChange = ssHisUser_Sheet1.Cells[i, 11].Text.Trim();
                    strETCADD = ssHisUser_Sheet1.Cells[i, 12].Text.Trim();

                    if(strChange == "Y" && strPtNo != "")
                    {
                        if (strROWID == "")
                        {
                            #region 중복 점검
                            // '2016-08-11 김경동 작업
                            //SQL = " SELECT PTNO, B.SNAME, BDATE, A.GUBUN_AMPM, A.BIGO";
                            //SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMR_LIST A";
                            //SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_PMPA.BAS_PATIENT B";
                            //SQL += ComNum.VBLF + "     ON A.PTNO = B.PANO";
                            //SQL += ComNum.VBLF + " WHERE A.BDate = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                            //SQL += ComNum.VBLF + "   AND A.PTNO = '" + strPtNo + "' ";
                            //SQL += ComNum.VBLF + "   AND A.DEPTCODE = '" + strDeptCode + "' ";
                            //SQL += ComNum.VBLF + "   AND GUBUN_AMPM = '" + strGUBUN_AMPM + "'";
                            //SQL += ComNum.VBLF + "   AND ETC_ADD = '1'";
                            //SQL += ComNum.VBLF + "   AND DISPSEQ <> " + i;
                            //SQL += ComNum.VBLF + "ORDER BY A.WRITEDATE";

                            //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                            //if (SqlErr != "")
                            //{
                            //    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            //    Cursor.Current = Cursors.Default;
                            //    return false;
                            //}

                            //if (dt.Rows.Count > 1)
                            //{
                            //    strMsg = "등록번호 [" + strPtNo + "]  중복입력입니다.";
                            //    strMsg += ComNum.VBLF + ("성명 : " + dt.Rows[0]["SNAME"].ToString().Trim());
                            //}

                            //dt.Dispose();
                            //dt = null;

                            //if (!string.IsNullOrWhiteSpace(strMsg))
                            //{
                            //    ComFunc.MsgBoxEx(this, strMsg);
                            //    continue;
                            //}
                            #endregion

                            SQL = " INSERT INTO KOSMOS_EMR.EMR_LIST( ";
                            SQL = SQL + ComNum.VBLF + " BDATE, PTNO, JUPTIME, WRITEDATE, ";
                            SQL = SQL + ComNum.VBLF + " WRITESABUN, BIGO, DRCODE, GUBUN_AMPM, ";
                            SQL = SQL + ComNum.VBLF + " ETC_ADD, SAYU, DEPTCODE, DISPSEQ ) VALUES (";
                            SQL = SQL + ComNum.VBLF + "TO_DATE('" + strBDATE + "','YYYY-MM-DD'), '" + strPtNo + "','" + strJUPTIME + "', SYSDATE, ";
                            SQL = SQL + ComNum.VBLF + clsType.User.IdNumber + ",'" + strBigo + "','" + strDrCode + "','" + strGUBUN_AMPM + "',";
                            SQL = SQL + ComNum.VBLF + "'1','" + strSayu + "','" + strDeptCode + "', " + i + " )";
                        }
                        else
                        {
                            SQL = " UPDATE KOSMOS_EMR.EMR_LIST SET ";
                            //'SQL = SQL + vbCr + " WRITEDATE = SYSDATE, "
                            //'SQL = SQL + vbCr + " WRITESABUN = " + GnJobSabun + ", "
                            //'SQL = SQL + vbCr + "  GUBUN_AMPM = '" + strGUBUN_AMPM + "', "
                            SQL = SQL + ComNum.VBLF + " DISPSEQ = " + i + ", ";
                            SQL = SQL + ComNum.VBLF + " SAYU = '" + strSayu + "', ";
                            SQL = SQL + ComNum.VBLF + " BIGO = '" + strBigo + "' ";
                            SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPtNo + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "   AND DRCODE = '" + strDrCode + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND GUBUN_AMPM = '" + strGUBUN_AMPM + "' ";
                            SQL = SQL + ComNum.VBLF + "   AND ETC_ADD = '1' ";
                            //'SQL = SQL & vbCr & " WHERE ROWID = '" & strROWID & "' "
                            //'SQL = SQL & vbCr & "    AND WRITESABUN = " & GnJobSabun
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        string GetDrCD(string arg)
        {
            string rtnVal = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT DRCODE FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL += ComNum.VBLF + " WHERE DRNAME = '" + arg + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["DRCODE"].ToString().Trim();
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        void GetDrDpet(string DrCd, ref string strDept1, ref string strDept2)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT DRDEPT1, DRDEPT2 ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_DOCTOR ";
                SQL += ComNum.VBLF + "WHERE DRCODE = '" + DrCd + "' ";

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
                    Cursor.Current = Cursors.Default;
                    return;
                }

                strDept1 = dt.Rows[0]["DRDEPT1"].ToString().Trim();
                strDept2 = dt.Rows[0]["DRDEPT2"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        bool ReadWriteMagam(string argDATE, string argDRCD, string ArgGubun, string ArgDeptCode)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT RECNAME FROM KOSMOS_EMR.EMR_LIST_MAGAM";
                SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND DRCODE = '" + argDRCD + "'";
                SQL += ComNum.VBLF + "   AND GUBUN = '" + ArgGubun + "'";
                SQL += ComNum.VBLF + "   AND DEPTCODE = '" + ArgDeptCode + "'";
                SQL += ComNum.VBLF + "   AND RECNAME IS NOT NULL";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if(dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if (Save_Data() == true)
            {
                ReadList();
                UNSEEN_LIST();
            }

            ssHis_Sheet1.RowCount = 0;
            ssHisUser_Sheet1.RowCount = 0;

            ReadList("1");

            Set_Print("P");

            ReadList();
        }

        void Set_Print(string strPrintType)
        {
            string strTitle = "";
            string strTitle2 = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            ssHis_Sheet1.Columns[12].Visible = false;

            if (rdoALL.Checked == true)
            {
                strTitle2 = "※진료시간 : 전체";
            }
            else if (rdoAM.Checked == true)
            {
                strTitle2 = "※진료시간 : 오전";
            }
            else if (rdoPM.Checked == true)
            {
                strTitle2 = "※진료시간 : 오후";
            }
            else if (rdoNT.Checked == true)
            {
                strTitle2 = "※진료시간 : 야간";
            }

            strTitle2 = "  ※진료의 : " + VB.Left(cboDr.Text, 10).Trim() + "  " + strTitle2;

            //strTitle = "TEXT EMR 작성현황조회" + ComNum.VBLF;
            strTitle = "의무기록 스캔 리스트 작성현황조회" + ComNum.VBLF;

            btnPrint.Enabled = false;

            strHeader = CS.setSpdPrint_String(strTitle, new Font("바탕체", 14, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력 일자 : " + ComFunc.FormatStrToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"), "DK") + ComNum.VBLF +
                //"※진료과  : " + clsType.User.DeptCode + strTitle2 + "     ※기록사 : " + ComNum.VBLF
                "※진료과  : " + VB.Right(cboDept.Text, 6).Trim() + strTitle2 + "     ※기록사 : " + ComNum.VBLF
                , new Font("바탕체", 12), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 70, 20, 70, 20);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);
            if(strPrintType == "P")
            {
                CS.setSpdPrint(ssHis, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if(strPrintType == "V")
            {
                //TODO
                //Set gSSPrintPreView = ssHis
                //frmPrintPreView.ssSpreadPreview.hWndSpread = .Hwnd
                //Call frmPrintPreView.Show(vbModal)
            }

            btnPrint.Enabled = true;
            ssHis_Sheet1.Columns[12].Visible = true;
        }

        private void btnPreView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if (Save_Data() == true)
            {
                ReadList();
                UNSEEN_LIST();
            }

            ssHis_Sheet1.RowCount = 0;
            ssHisUser_Sheet1.RowCount = 0;

            ReadList("1");

            Set_Print("V");

            ReadList();
        }

        private void btnMagam1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            Save_Magam("1");
            MagamView();
        }

        private void btnMagam2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            Save_Magam("2");
            MagamView();
        }

        bool Save_Magam(string strGubun)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;


            string strName = strGubun == "1" ? txtMagam1Name.Text.Trim() : txtMagam2Name.Text.Trim();
            string strBDATE = dtpChartDate1.Text;
            string strDeptCode = VB.Right(cboDept.Text, 6).Trim();
            string strDrCode = VB.Right(cboDr.Text, 5).Trim();

            switch (strDeptCode)
            {
                case "PC":
                case "HU":
                    break;
                default:
                    if(VB.Left(cboDr.Text, 10).Trim() == "전  체")
                    {
                        ComFunc.MsgBoxEx(this, "진료의사를 선택하시기 바랍니다.");
                        return rtnVal;
                    }

                    if(strDrCode == "0")
                    {
                        ComFunc.MsgBoxEx(this, "진료의사 선택이 되질 않았습니다.");
                        return rtnVal;
                    }
                    break;
            }

            if(strDeptCode == "PC")
            {
                strDrCode = "6299";
            }
            else if(strDeptCode == "HU")
            {
                strDrCode = "1501";
            }

            if (rdoALL.Checked == true)
            {
                ComFunc.MsgBoxEx(this, "오전/오후 구분을 선택하시기 바랍니다.");
                return rtnVal;
            }


            if (VB.Left(dtpChartDate1.Text, 10) == ComQuery.CurrentDateTime(clsDB.DbCon, "S").Substring(0, 10) && VB.Val(ComQuery.CurrentDateTime(clsDB.DbCon, "T")) < 1100)
            {
                ComFunc.MsgBoxEx(this, "오전 11시 이전에는 마감이 불가능합니다.");
                return rtnVal;
            }

            if (strDeptCode == "0")
            {
                ComFunc.MsgBoxEx(this, "진료과 선택이 되질 않았습니다.");
                return rtnVal;
            }

            if (strName == "")
            {
                ComFunc.MsgBoxEx(this, "마감한 직원의 이름이 공란입니다.");
                return rtnVal;
            }

            if (ReadSaveDataCnt(strBDATE, strDrCode, strGubun, strDeptCode) == false && ComFunc.MsgBoxQ("저장된 내용이 없는데 마감을 완료 하시겠습니까?", "마감", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return rtnVal;
            }

            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                bool msg = false;

                if (VB.Right(btnMagam1.Text, 2) == "마감" || VB.Right(btnMagam2.Text, 2) == "마감")
                {
                    SQL = "SELECT COUNT(*) CNT";
                    SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_LIST_MAGAM";
                    SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "' ";
                    SQL += ComNum.VBLF + "   AND DRCODE = '" + strDrCode + "' ";
                    SQL += ComNum.VBLF + "   AND GUBUN = '" + strGubun + "' ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }

                    if (dt.Rows.Count > 0 && VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) > 0)
                    {
                        msg = true;
                    }

                    dt.Dispose();

                    if (msg)
                    {
                        ComFunc.MsgBoxEx(this, "이미 마감 되어있습니다.");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return rtnVal;
                    }

                    SQL = " INSERT INTO KOSMOS_EMR.EMR_LIST_MAGAM( ";
                    SQL += ComNum.VBLF + " BDATE, DEPTCODE, DRCODE, GUBUN, ";
                    SQL += ComNum.VBLF + " WRITESABUN, WRITEDATE, MAGAMNAME ) VALUES ( ";
                    SQL += ComNum.VBLF + "TO_DATE('" + strBDATE + "','YYYY-MM-DD'), '" + strDeptCode + "','" + strDrCode + "','" + strGubun + "', ";
                    SQL += ComNum.VBLF + clsType.User.Sabun + ", SYSDATE,'" + strName + "') ";
                }
                else
                {
                    SQL = " DELETE KOSMOS_EMR.EMR_LIST_MAGAM";
                    SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND DEPTCODE = '" + strDeptCode + "' ";
                    SQL += ComNum.VBLF + "   AND DRCODE = '" + strDrCode + "' ";
                    SQL += ComNum.VBLF + "   AND GUBUN = '" + strGubun + "' ";
                    //SQL += ComNum.VBLF + "    AND WRITESABUN = " + clsType.User.Sabun;
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
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
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        bool ReadSaveDataCnt(string ArgBDate, string ArgDrCode, string argAMPM, string argDEPTCD)
        {
            //switch(argDEPTCD)
            //{
            //    case "FM":
            //    case "DT":
            //    case "PC":
            //    case "CS":
            //    case "HU":
            //    case "OG":
            //        return true;
            //}

            bool rtnVal = false;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT * FROM KOSMOS_EMR.EMR_LIST";
                SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF +  " AND DRCODE = '" + ArgDrCode + "'";
                SQL += ComNum.VBLF +  " AND GUBUN_AMPM = '" + argAMPM + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnNameChange1_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            Set_Name_Update(txtMagam1Name.Text.Trim(), "1");
        }

        private void btnNameChange2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            Set_Name_Update(txtMagam2Name.Text.Trim(), "2");
        }

        void Set_Name_Update(string strMagamName, string strGubun)
        {
            if (string.IsNullOrWhiteSpace(strMagamName))
            {
                ComFunc.MsgBoxEx(this, "수정하시려는 이름이 공란입니다.");
                return;
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strBDATE = dtpChartDate1.Text;
            string strDeptCode = VB.Right(cboDept.Text, 6).Trim();
            string strDrCode = VB.Right(cboDr.Text, 5).Trim();

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = " UPDATE KOSMOS_EMR.EMR_LIST_MAGAM SET";
                SQL += ComNum.VBLF + " MAGAMNAME = '" + strMagamName + "' ";
                SQL += ComNum.VBLF + " WHERE BDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DEPTCODE = '" + strDeptCode + "' ";
                if(strDeptCode != "PC")
                {
                    SQL += ComNum.VBLF + "    AND DRCODE = '" + strDrCode + "' ";
                }
                SQL += ComNum.VBLF + "    AND GUBUN = '" + strGubun +"' ";
                //'SQL = SQL & vbCr & "    AND WRITESABUN = " & GnJobSabun

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "이름 수정 완료");
                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTime_Click(object sender, EventArgs e)
        {
            string strBtnText = ((Button) sender).Text;

            if(mCon == "txtTime1")
            {
                txtTime1.Text = strBtnText;
            }
            else if(mCon == "txtTime2")
            {
                txtTime2.Text = strBtnText;
            }
            panTime.Visible = false;
        }

        private void txtTime1_DoubleClick(object sender, EventArgs e)
        {
            mCon = "txtTime1";
            panTime.Visible = true;
        }

        private void txtTime2_DoubleClick(object sender, EventArgs e)
        {
            mCon = "txtTime2";
            panTime.Visible = true;
        }
        
        void Set_Cbo_Dr()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strDeptCd = VB.Right(cboDept.Text.Trim(), 6).Trim();

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT DRNAME,DRCODE FROM KOSMOS_PMPA.BAS_DOCTOR  ";
                SQL += ComNum.VBLF + "  WHERE TOUR = 'N' ";

                if(strDeptCd == "RA" )
                {
                    SQL += ComNum.VBLF + " AND DRCODE IN ( '1107','1125') ";
                }
                else
                {
                    //if(strDeptCd.Equals("0") == false)
                    //{
                    //    SQL += ComNum.VBLF + " AND DEPTCODE = '" + strDeptCd + "' ";
                    //}

                    if (strDeptCd.Equals("0") == false)
                    {
                        SQL += ComNum.VBLF + " AND DRDEPT1 = '" + strDeptCd + "' ";
                    }
                }
                //SQL += ComNum.VBLF + "  ORDER BY GRADE ";
                SQL += ComNum.VBLF + "  ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //진료의
                cboDr.Items.Clear();
                cboDr.Items.Add("전  체" + VB.Space(50) + "0");

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    cboDr.SelectedIndex = 0;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    cboDr.Items.Add(dt.Rows[i]["DRNAME"].ToString().Trim() + VB.Space(50) + dt.Rows[i]["DRCODE"].ToString().Trim());
                }

                cboDr.SelectedIndex = 0;

                dt.Dispose();
                dt = null;

                if (rdoALL.Checked == true) return;

                if (VB.Left(cboDr.Text, 10).Trim() == "전  체")
                {
                    if (VB.Right(cboDept.Text, 6).Trim() == "PC" || VB.Right(cboDept.Text, 6).Trim() == "HU")
                    {

                    }
                    else
                    {
                        ComFunc.MsgBoxEx(this, "진료의사를 선택하시기 바랍니다.");
                        return;
                    }
                }

                ReadList();
                UNSEEN_LIST();
                MagamView();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void rdoTime_CheckedChanged(object sender, EventArgs e)
        {
            txtTime1.Text = VB.Format(DateTime.Now, "hh:mm");
            txtTime2.Text = VB.Format(DateTime.Now, "hh:mm");
            txtTime1.Visible = true;
            txtTime2.Visible = true;
            label8.Visible = true;
        }

        private void rdoAM_CheckedChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (rdoALL.Checked == true) return;

            if (VB.Left(cboDr.Text, 10).Trim() == "전  체")
            {
                if (VB.Right(cboDept.Text, 6).Trim() == "PC" || VB.Right(cboDept.Text, 6).Trim() == "HU")
                {

                }
                else
                {
                    ComFunc.MsgBoxEx(this, "진료의사를 선택하시기 바랍니다.");
                    return;
                }
            }

            ReadList();
            UNSEEN_LIST();
        }

        private void rdoPM_CheckedChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (rdoALL.Checked == true) return;

            if (VB.Left(cboDr.Text, 10).Trim() == "전  체")
            {
                if (VB.Right(cboDept.Text, 6).Trim() == "PC" || VB.Right(cboDept.Text, 6).Trim() == "HU")
                {

                }
                else
                {
                    ComFunc.MsgBoxEx(this, "진료의사를 선택하시기 바랍니다.");
                    return;
                }
            }

            ReadList();
            UNSEEN_LIST();
        }

        private void rdoNT_CheckedChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (rdoALL.Checked == true) return;

            if (VB.Left(cboDr.Text, 10).Trim() == "전  체")
            {
                if (VB.Right(cboDept.Text, 6).Trim() == "PC" || VB.Right(cboDept.Text, 6).Trim() == "HU")
                {

                }
                else
                {
                    ComFunc.MsgBoxEx(this, "진료의사를 선택하시기 바랍니다.");
                    return;
                }
            }

            ReadList();
            UNSEEN_LIST();
        }

        private void ssHis_Change(object sender, ChangeEventArgs e)
        {
            if (e.Row < 0) return;

            ssHis_Sheet1.Cells[e.Row, 11].Text = "Y";
            ssHis_Sheet1.Rows[e.Row].ForeColor = Color.FromArgb(0, 0, 255);
            ssHis_Sheet1.Rows[e.Row].Height = ssHis_Sheet1.Rows[e.Row].GetPreferredHeight() + 5;

            if(e.Column == 1)
            {
                string strPano = ComFunc.SetAutoZero(ssHis_Sheet1.Cells[e.Row, 1].Text, 8);

                if (ssHis_Sheet1.RowCount > 0 && ssHis_Sheet1.RowCount != (e.Row + 1))
                {
                    for (int i = 0; i < ssHis_Sheet1.Rows.Count; i++)
                    {
                        if (i != e.Row && ssHis_Sheet1.Cells[i, 1].Text.Trim().Equals(strPano))
                        {
                            ssHis_Sheet1.Cells[e.Row, 1, e.Row, ssHis_Sheet1.ColumnCount - 1].Text = "";
                            ssHis.ShowRow(0, i, VerticalPosition.Top);
                            ssHis.ShowCell(0, 0, i, 0, VerticalPosition.Top, HorizontalPosition.Center);
                            ssHis_Sheet1.SetActiveCell(i, 1);
                            ComFunc.MsgBoxEx(this, (i + 1) + "번째에 있는 등록번호 입니다.");
                            return;
                        }
                    }
                }

                ssHis_Sheet1.Cells[e.Row, 1].Text = strPano;
                ssHis_Sheet1.Cells[e.Row, 2].Text = clsVbfunc.GetPatientName(clsDB.DbCon, strPano);
                ssHis_Sheet1.Cells[e.Row, 12].Text = "2";
            }

        }

        private void ssHis_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0) return;

            if (e.Column == 1)
            {
                clsVbEmr.EXECUTE_TextEmrViewEx(ssHis_Sheet1.Cells[e.Row, 1].Text.Trim(), clsType.User.IdNumber);
                return;
            }                    

        }

        private void ssHisUser_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (ssHisUser_Sheet1.RowCount == 0) return;

            if (ssHisUser_Sheet1.Cells[e.Row, 1].Text.Trim().Length == 0) return;

            if (e.Column == 1)
            {
                clsVbEmr.EXECUTE_TextEmrViewEx(ssHisUser_Sheet1.Cells[e.Row, 1].Text.Trim(), clsType.User.IdNumber);
                return;
            }
        }

        private void ssHisUser_Change(object sender, ChangeEventArgs e)
        {
            if (e.Row < 0) return;

            ssHisUser_Sheet1.Cells[e.Row, 11].Text = "Y";
            ssHisUser_Sheet1.Rows[e.Row].ForeColor = Color.FromArgb(0, 0, 255);
            ssHisUser_Sheet1.Rows[e.Row].Height = ssHisUser_Sheet1.Rows[e.Row].GetPreferredHeight() + 5;

            if (ssHisUser_Sheet1.ActiveColumnIndex == 1)
            {
                string strPano = ComFunc.SetAutoZero(ssHisUser_Sheet1.Cells[e.Row, 1].Text, 8);

                if (ssHis_Sheet1.RowCount > 0)
                {
                    for (int i = 0; i < ssHis_Sheet1.Rows.Count; i++)
                    {
                        if (ssHis_Sheet1.Cells[i, 1].Text.Trim().Equals(strPano))
                        {
                            ssHis.Focus();
                            ssHisUser_Sheet1.Cells[e.Row, 1, e.Row, ssHisUser_Sheet1.ColumnCount - 1].Text = "";
                            ssHis_Sheet1.SetActiveCell(i, 1);
                            ssHis.ShowRow(0, i, VerticalPosition.Nearest);
                            ssHis.ShowColumn(0, 1, HorizontalPosition.Nearest);
                            //ssHisUser.ShowCell(0, 0, i, 0, VerticalPosition.Nearest, HorizontalPosition.Center);
                            ComFunc.MsgBoxEx(this, "위 쪽 리스트 " + (i + 1) + "번째에 있는 등록번호 입니다.");
                            panel3.Focus();
                            return;
                        }
                    }
                }

                if (ssHisUser_Sheet1.RowCount > 0)
                {
                    for (int i = 0; i < ssHisUser_Sheet1.Rows.Count; i++)
                    {
                        if (i != e.Row && ssHisUser_Sheet1.Cells[i, 1].Text.Trim().Equals(strPano))
                        {
                            ssHisUser.Focus();
                            ssHisUser_Sheet1.Cells[e.Row, 1, e.Row, ssHisUser_Sheet1.ColumnCount - 1].Text = "";
                            ssHisUser_Sheet1.SetActiveCell(i, 1);
                            ssHisUser.ShowRow(0, i, VerticalPosition.Nearest);
                            ssHisUser.ShowColumn(0, 1, HorizontalPosition.Nearest);
                            //ssHisUser.ShowCell(0, 0, i, 0, VerticalPosition.Nearest, HorizontalPosition.Center);
                            ComFunc.MsgBoxEx(this, (i + 1) + "번째에 있는 등록번호 입니다.");
                            panel3.Focus();
                            return;
                        }
                    }
                }

                ssHisUser_Sheet1.Cells[e.Row, 1].Text = strPano;
                ssHisUser_Sheet1.Cells[e.Row, 2].Text = clsVbfunc.GetPatientName(clsDB.DbCon, strPano);
                ssHisUser_Sheet1.Cells[e.Row, 12].Text = "1";
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            panTime.Visible = false;
        }

        private void ssHis_CellClick(object sender, CellClickEventArgs e)
        {
            if (ssHis_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssHis, e.Column);
                return;
            }

            if (e.Column == 9 && e.Button == MouseButtons.Right && contextMenu != null)
            {
                contextMenu.Show(ssHis, new System.Drawing.Point(e.X, e.Y));
            }
        }
        
        private void cboDr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인


            if (VB.Right(cboDept.Text, 6).Trim() == "PC" && VB.Left(cboDr.Text, 10).Trim() != "전 체")
            {
                ComFunc.MsgBoxEx(this, "통증치료실의 경우 진료의사는 '전 체' 만 선택이 가능합니다.");
                cboDr.SelectedIndex = 0;
                return;
            }

            if (rdoALL.Checked == false)
            {
                if (VB.Left(cboDr.Text, 10).Trim() == "전  체")
                {
                    if (VB.Right(cboDept.Text, 6).Trim() == "PC" || VB.Right(cboDept.Text, 6).Trim() == "HU")
                    {

                    }
                    else
                    {
                        //ComFunc.MsgBoxEx(this, "진료의사를 선택하시기 바랍니다.");
                        return;
                    }
                }

                ReadList();
                UNSEEN_LIST();
            }

            MagamView();
        }

        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            Set_Cbo_Dr();
            MagamView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            string strDeptCode = VB.Right(cboDept.Text, 6).Trim();
            string strDrCode = "";
            string strGUBUN_AMPM = "";

            if (strDeptCode != "PC" && strDeptCode != "HU")
            {
                if (VB.Left(cboDr.Text, 10) == "전  체")
                {
                    ComFunc.MsgBoxEx(this, "진료의사를 선택하시기 바랍니다.");
                    return;
                }

                if (strDrCode == "0")
                {
                    ComFunc.MsgBoxEx(this, "진료의사 선택이 되질 않았습니다.");
                    return;
                }
            }

            strDrCode = VB.Right(cboDr.Text, 5).Trim();

            if (strDeptCode == "PC") strDrCode = "6299";

            if (rdoALL.Checked == true)
            {
                ComFunc.MsgBoxEx(this, "오전/오후 구분을 선택하시기 바랍니다.");
                return;
            }

            if (rdoAM.Checked == true)
            {
                strGUBUN_AMPM = "1";
            }
            else if (rdoPM.Checked == true)
            {
                strGUBUN_AMPM = "2";
            }

            string strBDATE = dtpChartDate1.Text;

            if (ReadWriteMagam(strBDATE, strDrCode, strGUBUN_AMPM, strDeptCode) == true) return;
            if (ComFunc.MsgBoxQEx(this, "체크 하신 메모들을 삭제 하시겠습니까?") == DialogResult.No) return;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            try
            {
                for (int i = 0; i < ssHis_Sheet1.RowCount; i++)
                {
                    if (ssHis_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        string strROWID = ssHis_Sheet1.Cells[i, 10].Text.Trim();

                        if (string.IsNullOrWhiteSpace(strROWID))
                            continue;

                        SQL = " UPDATE KOSMOS_EMR.EMR_LIST ";
                        SQL += ComNum.VBLF + " SET BIGO = NULL";
                        SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                ReadList();
                UNSEEN_LIST();

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

            string strDeptCode = VB.Right(cboDept.Text, 6).Trim();
            string strDrCode = "";
            string strGUBUN_AMPM = "";

            if (strDeptCode != "PC" && strDeptCode != "HU")
            {
                if (VB.Left(cboDr.Text, 10) == "전  체")
                {
                    ComFunc.MsgBoxEx(this, "진료의사를 선택하시기 바랍니다.");
                    return;
                }

                if (strDrCode == "0")
                {
                    ComFunc.MsgBoxEx(this, "진료의사 선택이 되질 않았습니다.");
                    return;
                }
            }

            strDrCode = VB.Right(cboDr.Text, 5).Trim();

            if (strDeptCode == "PC") strDrCode = "6299";

            if (rdoALL.Checked == true)
            {
                ComFunc.MsgBoxEx(this, "오전/오후 구분을 선택하시기 바랍니다.");
                return;
            }

            if (rdoAM.Checked == true)
            {
                strGUBUN_AMPM = "1";
            }
            else if (rdoPM.Checked == true)
            {
                strGUBUN_AMPM = "2";
            }

            string strBDATE = dtpChartDate1.Text;

            if (ReadWriteMagam(strBDATE, strDrCode, strGUBUN_AMPM, strDeptCode) == true) return;

            if (ComFunc.MsgBoxQEx(this, "체크 하신 메모들을 삭제 하시겠습니까?") == DialogResult.No) return;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            try
            {
                for (int i = 0; i < ssHisUser_Sheet1.RowCount; i++)
                {
                    if (ssHisUser_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        string strROWID = ssHisUser_Sheet1.Cells[i, 10].Text.Trim();

                        if (string.IsNullOrWhiteSpace(strROWID))
                            continue;

                        SQL = " DELETE KOSMOS_EMR.EMR_LIST ";
                        SQL += ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                ReadList();
                UNSEEN_LIST();

            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void ssHisUser_EditModeOff(object sender, EventArgs e)
        {

        }

        private void ssHisUser_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Column == 9 && e.Button == MouseButtons.Right && contextMenu != null)
            {
                contextMenu.Show(ssHisUser, new Point(e.X, e.Y));
            }
        }

        private void btnSaveMacro_Click(object sender, EventArgs e)
        {
            using(frmEmrListMacro frmEmrListMacroX = new frmEmrListMacro())
            {
                frmEmrListMacroX.StartPosition = FormStartPosition.CenterParent;
                frmEmrListMacroX.rSetSendData += FrmEmrListMacroX_rSetSendData;
                frmEmrListMacroX.ShowDialog(this);
            }
        }

        private void FrmEmrListMacroX_rSetSendData(bool Refresh)
        {
            if (Refresh)
            {
                if (contextMenu != null)
                {
                    contextMenu.Dispose();
                    contextMenu = null;
                }

                MACRO_ADD();
            }
        }

        private void ssHisUser_ClipboardPasted(object sender, ClipboardPastedEventArgs e)
        {
            if (ssHisUser_Sheet1.ActiveColumnIndex == 1)
            {
                string strPano = ComFunc.SetAutoZero(ssHisUser_Sheet1.Cells[ssHisUser_Sheet1.ActiveRowIndex, 1].Text, 8);

                if (ssHis_Sheet1.RowCount > 0)
                {
                    for (int i = 0; i < ssHis_Sheet1.Rows.Count; i++)
                    {
                        if (ssHis_Sheet1.Cells[i, 1].Text.Trim().Equals(strPano))
                        {
                            ssHis.Focus();
                            ssHisUser_Sheet1.Cells[ssHisUser_Sheet1.ActiveRowIndex, 1, ssHisUser_Sheet1.ActiveRowIndex, ssHisUser_Sheet1.ColumnCount - 1].Text = "";
                            ssHis_Sheet1.SetActiveCell(i, 1);
                            ssHis.ShowRow(0, i, VerticalPosition.Nearest);
                            ssHis.ShowColumn(0, 1, HorizontalPosition.Nearest);
                            //ssHisUser.ShowCell(0, 0, i, 0, VerticalPosition.Nearest, HorizontalPosition.Center);
                            ComFunc.MsgBoxEx(this, "위 쪽 리스트 " + (i + 1) + "번째에 있는 등록번호 입니다.");
                            panel3.Focus();
                            return;
                        }
                    }
                }

                if (ssHisUser_Sheet1.RowCount > 0)
                {
                    for (int i = 0; i < ssHisUser_Sheet1.Rows.Count; i++)
                    {
                        if (i != ssHisUser_Sheet1.ActiveRowIndex && ssHisUser_Sheet1.Cells[i, 1].Text.Trim().Equals(strPano))
                        {
                            ssHisUser.Focus();
                            ssHisUser_Sheet1.Cells[ssHisUser_Sheet1.ActiveRowIndex, 1, ssHisUser_Sheet1.ActiveRowIndex, ssHisUser_Sheet1.ColumnCount - 1].Text = "";
                            ssHisUser_Sheet1.SetActiveCell(i, 1);
                            ssHisUser.ShowRow(0, i, VerticalPosition.Nearest);
                            ssHisUser.ShowColumn(0, 1, HorizontalPosition.Nearest);
                            //ssHisUser.ShowCell(0, 0, i, 0, VerticalPosition.Nearest, HorizontalPosition.Center);
                            ComFunc.MsgBoxEx(this, (i + 1) + "번째에 있는 등록번호 입니다.");
                            panel3.Focus();
                            return;
                        }
                    }
                }

                ssHisUser_Sheet1.Cells[ssHisUser_Sheet1.ActiveRowIndex, 1].Text = strPano;
                ssHisUser_Sheet1.Cells[ssHisUser_Sheet1.ActiveRowIndex, 2].Text = clsVbfunc.GetPatientName(clsDB.DbCon, strPano);
                ssHisUser_Sheet1.Cells[ssHisUser_Sheet1.ActiveRowIndex, 12].Text = "1";
            }
        }
    }
}
