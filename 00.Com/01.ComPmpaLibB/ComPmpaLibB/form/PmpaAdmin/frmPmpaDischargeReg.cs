using ComBase;
using ComPmpaLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB;
using FarPoint.Win.Spread;
using ComLibB;

/// <summary>
/// Description : 퇴원등록
/// Author : 박병규
/// Create Date : 2018.03.02
/// </summary>
/// <seealso cref="Frm퇴원등록(Frm퇴원등록.frm)"/>

namespace ComPmpaLibB
{
    public partial class frmPmpaDischargeReg : Form
    {
        //clsSpread CS    = null;
        //ComFunc CF      = null;
        //clsPmpaFunc CPF = null;
        //clsVbfunc CV    = null;

        DataTable Dt    = new DataTable();
        DataTable DtSub = new DataTable();

        string SQL = "";
        string SqlErr = "";
        int intRowCnt = 0;

        long FnIpdNo = 0;
        long FnTrsNo = 0;

        bool FbJSim_Flag = false;

        string FstrGbSuday = "";        
        string FstrRoomOver = "";
        string FstrPtnoFind = "NO";
        string FstrGbSTS = "";
        string FstrGatewonTime = "";
        string[] FstrJSimSabun = new string[15];

        public frmPmpaDischargeReg()
        {
            InitializeComponent();
            setParam();
        }
        
        void setParam()
        {
            this.Load                       += new EventHandler(eFrm_Load);

            this.txtSPtno.KeyPress          += new KeyPressEventHandler(eCtl_KeyPress);
            this.txtSPtno.LostFocus         += new EventHandler(eCtl_LostFocus);

            this.txtSSname.KeyPress         += new KeyPressEventHandler(eCtl_KeyPress);

            this.cboWard.MouseWheel         += new MouseEventHandler(eCbo_Wheel);
            this.cboWard.Click              += new EventHandler(eCtl_Click);

            this.ssList.CellDoubleClick     += new CellClickEventHandler(Spread_DoubleClick);
            this.ssIpdList.CellClick        += new CellClickEventHandler(Spread_Click);
            this.ssIpdList.CellDoubleClick  += new CellClickEventHandler(Spread_DoubleClick);

            this.txtPtno.GotFocus           += new EventHandler(eCtl_GotFocus);
            this.txtPtno.KeyPress           += new KeyPressEventHandler(eCtl_KeyPress);

            this.txtRtime.GotFocus          += new EventHandler(eCtl_GotFocus);
            this.txtRtime.TextChanged       += new EventHandler(eCtl_Changed);

            this.cboSTS.MouseWheel          += new MouseEventHandler(eCbo_Wheel);
            this.cboBun.MouseWheel          += new MouseEventHandler(eCbo_Wheel);

            this.btnKiho.Click              += new EventHandler(eCtl_Click);
            this.btnSearch.Click            += new EventHandler(eCtl_Click);
            this.btnPrint.Click             += new EventHandler(eCtl_Click);
            this.btnSimsaOK.Click           += new EventHandler(eCtl_Click);
            this.btnSimsa.Click             += new EventHandler(eCtl_Click);
            this.btnExit.Click              += new EventHandler(eCtl_Click);
            this.btnOk.Click                += new EventHandler(eCtl_Click);
            this.btnCancel.Click            += new EventHandler(eCtl_Click);
        }

        void Spread_Click(object sender, CellClickEventArgs e)
        {
            clsSpread CS = new clsSpread();
            ComFunc CF = new ComFunc();
            clsPmpaFunc CPF = new clsPmpaFunc();

            if (sender == this.ssIpdList)
            {
                if (e.ColumnHeader == true)
                {
                    CS.setSpdSort(ssIpdList, e.Column, true);
                    return;
                }
                else
                {
                    string strPtno = ssIpdList_Sheet1.Cells[e.Row, 0].Text.Trim();

                    if (ssIpdList_Sheet1.Cells[e.Row, 19].Text.Trim() == "")
                    {
                        ssIpdList_Sheet1.Cells[e.Row, 19].Text = CF.Read_SabunName(clsDB.DbCon, Read_Simsa_Sabun(clsDB.DbCon, strPtno));

                        if (ssIpdList_Sheet1.Cells[e.Row, 19].Text.Trim() != "")
                            ssIpdList_Sheet1.Cells[e.Row, 19].Text += "(" + CPF.Get_TelNo(clsDB.DbCon, ssIpdList_Sheet1.Cells[e.Row, 19].Text.Trim()) + ")";
                    }
                }
            }
        }

        void eCtl_Changed(object sender, EventArgs e)
        {
            if (sender == this.txtRtime)
            {
                int nLen = txtRtime.Text.Length;

                switch (nLen)
                {
                    case 2:
                        txtRtime.Text = txtRtime.Text + ":";
                        //txtRtime.SelectAll();
                        break;
                }
            }
        }

        void eCtl_GotFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPtno)
                txtPtno.SelectAll();
            else if (sender == this.txtRtime)
                txtRtime.SelectAll();
        }

        void Spread_DoubleClick(object sender, CellClickEventArgs e)
        {
            clsVbfunc CV = new clsVbfunc();

            if (sender == this.ssList)
            {
                string strJumin = "";
                if (clsLockCheck.GstrLockPtno != "")
                {
                    clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                }

                clsLockCheck.GstrLockPtno = "";

                string strPtno = ssList_Sheet1.Cells[e.Row, 0].Text.Trim();
                txtPtno.Text = strPtno;
                string strInDate = ssList_Sheet1.Cells[e.Row, 4].Text.Trim();
                FnIpdNo = Convert.ToInt64(ssList_Sheet1.Cells[e.Row, 5].Text.Trim());
                txtIpdNo.Text = FnIpdNo.ToString();
                FnTrsNo = Convert.ToInt64(ssList_Sheet1.Cells[e.Row, 6].Text.Trim());
                txtTrsNo.Text = FnTrsNo.ToString();
                int nAge = Convert.ToInt32(ssList_Sheet1.Cells[e.Row, 7].Text.Trim());
                FstrGbSuday = ssList_Sheet1.Cells[e.Row, 8].Text.Trim();

                if (nAge <= 6)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT JUMIN1,JUMIN2, JUMIN3 ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                    SQL += ComNum.VBLF + "  WHERE PANO = '" + strPtno + "' ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        Dt.Dispose();
                        Dt = null;
                        return;
                    }

                    if (Dt.Rows.Count > 0)
                    {
                        if (Dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                            strJumin = Dt.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(Dt.Rows[0]["JUMIN3"].ToString().Trim());
                        else
                            strJumin = Dt.Rows[0]["JUMIN1"].ToString().Trim() + Dt.Rows[0]["JUMIN2"].ToString().Trim();

                    }

                    Dt.Dispose();
                    Dt = null;

                    string strRemark = "";
                    strRemark = CV.PD_AGE_GESAN(strJumin, strInDate, clsPublic.GstrSysDate);

                    if (strRemark != "")
                        ComFunc.MsgBox(strRemark, "알림");

                    cboBun.SelectedIndex = 1;
                }

                Ptno_Check();

            }
            else if (sender == this.ssIpdList)
            {
                string strPtno = ssIpdList_Sheet1.Cells[e.Row, 0].Text.Trim();
                string strChk = ssIpdList_Sheet1.Cells[e.Row, 14].Text.Trim();
                string strGbSuday = ssIpdList_Sheet1.Cells[e.Row, 21].Text.Trim();

                if (strChk == "9") { return; }

                if (strPtno != "")
                {
                    txtPtno.Text = strPtno;
                    FnIpdNo = Convert.ToInt64(ssIpdList_Sheet1.Cells[e.Row, 12].Text.Trim());
                    txtIpdNo.Text = FnIpdNo.ToString();
                    FnTrsNo = Convert.ToInt64(ssIpdList_Sheet1.Cells[e.Row, 13].Text.Trim());
                    txtTrsNo.Text = FnTrsNo.ToString();

                    Read_Ipd_Trans_Person(clsDB.DbCon);

                    if (FstrPtnoFind == "NO")
                    {
                        Screen_Clear();
                        txtPtno.SelectAll();
                        txtSPtno.Focus();
                        return;
                    }

                    btnOk.Enabled = true;
                    btnCancel.Enabled = true;
                    //btnSimsaOK.Enabled = false;

                    //if (clsVbfunc.ProgramExecuteSabun(clsDB.DbCon, "IPD_퇴원심사완료"))
                        btnSimsaOK.Enabled = true;
                }
            }
        }

        void eCtl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtSPtno)
            { 
                if (txtSPtno.Text.Trim() != "")
                {
                    txtSPtno.Text = string.Format("{0:D8}", Convert.ToInt64(txtSPtno.Text));
                }
            }
        }

        void eCtl_Click(object sender, EventArgs e)
        {
            ComFunc CF = new ComFunc();
            clsPmpaFunc CPF = new clsPmpaFunc();

            if (sender == this.cboWard)
                Read_Ipd_Trans(clsDB.DbCon);
            else if (sender == this.btnKiho)
            {
                #region 자격조회 버튼
                string strPtno = txtPtno.Text;
                string strSname = txtSname.Text;
                string strDept = txtDept.Text;
                string strJumin1 = "";
                string strJumin2 = "";

                SQL = "";
                SQL += ComNum.VBLF + " SELECT JUMIN1,JUMIN2, JUMIN3 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "  WHERE PANO = '" + strPtno + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    strJumin1 = Dt.Rows[0]["JUMIN1"].ToString().Trim();

                    if (Dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                        strJumin2 = clsAES.DeAES(Dt.Rows[0]["JUMIN3"].ToString().Trim());
                    else
                        strJumin2 = Dt.Rows[0]["JUMIN2"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                if (VB.I(strSname, "A") > 1 || VB.I(strSname, "B") > 1 || VB.I(strSname, "C") > 1 || VB.I(strSname, "D") > 1)
                {
                    if (DialogResult.Yes == ComFunc.MsgBoxQ("성명에 A,B,C,D가 포함되어 있습니다...성명수정후 자격조회 하시겠습니까?", "알림"))
                        strSname = VB.InputBox("성명을 다시 입력바람", "이름확인", strSname);
                }

                clsPublic.GstrHelpCode = strPtno + "," + strDept + "," + strSname + "," + strJumin1 + strJumin2 + "," + clsPublic.GstrSysDate;

                frmPmpaCheckNhic frm = new frmPmpaCheckNhic(strPtno, strDept, strSname, strJumin1, strJumin2, clsPublic.GstrSysDate, "");
                frm.ShowDialog();

                if (VB.Pstr(clsPublic.GstrHelpCode, ";", 1) != "")
                    txtKiho.Text = VB.Pstr(clsPublic.GstrHelpCode, ";", 5);
                #endregion
            }
            else if (sender == this.btnSearch)
                Screen_Display(clsDB.DbCon);
            else if (sender == this.btnPrint)
                Print_Process();
            else if (sender == this.btnSimsaOK)
            {
                if (FnTrsNo == 0 || FnIpdNo == 0) { return; }

                clsIument CPI = new clsIument();

                CPI.Read_Ipd_Master(clsDB.DbCon, "", FnIpdNo);
                CPI.Read_Ipd_Mst_Trans(clsDB.DbCon, txtPtno.Text, FnTrsNo, "");

                frmSimsaConfirm frm = new frmSimsaConfirm(FnTrsNo);
                frm.ShowDialog();
                
                Screen_Clear();

                Screen_Display(clsDB.DbCon);

            }
            else if (sender == this.btnSimsa)
            {
                for (int i = 0; i < ssIpdList_Sheet1.Rows.Count; i++)
                {
                    string strPtno = ssIpdList_Sheet1.Cells[i, 0].Text.Trim();

                    if (ssIpdList_Sheet1.Cells[i, 19].Text.Trim() == "")
                    {
                        ssIpdList_Sheet1.Cells[i, 19].Text = CF.Read_SabunName(clsDB.DbCon, Read_Simsa_Sabun(clsDB.DbCon, strPtno));

                        if (ssIpdList_Sheet1.Cells[i, 19].Text.Trim() != "")
                            ssIpdList_Sheet1.Cells[i, 19].Text += "(" + CPF.Get_TelNo(clsDB.DbCon, ssIpdList_Sheet1.Cells[i, 19].Text.Trim()) + ")";
                    }
                }
            }
            else if (sender == this.btnExit)
            {
                if (clsLockCheck.GstrLockPtno != "")
                {
                    clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                }

                clsLockCheck.GstrLockPtno = "";

                this.Close();
            }
            else if (sender == this.btnOk)
                Save_Process(clsDB.DbCon);
            else if (sender == this.btnCancel)
            {
                if (clsLockCheck.GstrLockPtno != "")
                {
                    clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
                }

                clsLockCheck.GstrLockPtno = "";

                Screen_Clear();
                Read_Ipd_Trans(clsDB.DbCon);
                Screen_Display(clsDB.DbCon);

                txtSPtno.Focus();
            }
        }

        void Save_Process(PsmhDb pDbCon)
        {
            clsPmpaFunc CPF = new clsPmpaFunc();

            string strSname = "";
            string strBi = "";
            string strIndate = "";
            string strIntime = "";
            string strROutTime = "";
            string strBdate = "";
            string strGigan = "";
            string strGatewonTime = "";
            string strTime = "";

            ComFunc CF = new ComFunc();

            ComFunc.ReadSysDate(pDbCon);

            //이미 계산서발부,퇴원처리가 완료된 환자는 변경이 불가능함
            if (Convert.ToInt32(FstrGbSTS) >= 6) { return; }

            string strGbSTS = cboSTS.Text.Substring(0, 1);
            if (strGbSTS == "")
            {
                ComFunc.MsgBox("입원환자 재원상태가 공란임", "알림");
                return;
            }

            switch (strGbSTS)
            {
                case "1":
                    ComFunc.MsgBox("가퇴원은 가퇴원 등록 프로그램에서 등록 할 수 있음!!", "알림");
                    return;

                case "5":
                    ComFunc.MsgBox("심사완료는 심사완료에서 작업요망!!", "알림");
                    return;

                case "6":
                case "7":
                case "9":
                    ComFunc.MsgBox("이항목을 사용할 수 없음!!", "알림");
                    return;
            }

            if (Convert.ToInt32(FstrGbSTS) >= 5 && Convert.ToInt32(strGbSTS) <= 4)
            {
                ComFunc.MsgBox("심사완료 상태임. 퇴원상태 변경을 하실경우 심사완료 취소작업을 한후 등록하시기 바랍니다.", "알림");
                return;
            }

            string strDate = VB.DateAdd("D", 1, clsPublic.GstrSysDate).ToString();
            CF.DATE_HUIL_CHECK(pDbCon, strDate);

            if (string.Compare(dtpOutDate.Text, strDate) > 0)
            {
                ComFunc.MsgBox("실퇴원일이 정확한지 확인요망!!", "알림");
                return;
            }

            if (((string.Compare(dtpRdate.Text, clsPublic.GstrSysDate) < 0 || string.Compare(dtpRdate.Text, strDate) > 0)) && strGbSTS != "0")
            {
                ComFunc.MsgBox("퇴원처리일이 정확한지 확인요망!!", "알림");
                return;
            }

            strSname = txtSname.Text.Trim();
            strBi = txtBi.Text.Trim();
            strIndate = txtInDate.Text;

            if (strBi == "12")
            {
                if (txtKiho.Text.Length != 11)
                {
                    ComFunc.MsgBox("자격조회후 다시 작업을 하시기 바랍니다.", "알림");
                    return;
                }
                else
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE IPD_TRANS ";
                    SQL += ComNum.VBLF + "    SET KIHO  = '" + txtKiho.Text.Trim() + "' ";
                    SQL += ComNum.VBLF + "  WHERE IPDNO =  " + FnIpdNo + " ";
                    SQL += ComNum.VBLF + "    AND TRSNO =  " + FnTrsNo + " ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(pDbCon);
                }
            }

            //퇴원예고 등록시간을 Setting
            strROutTime = txtOutDateTime.Text.Trim();
            if (strROutTime == "")
                strROutTime = dtpRdate.Value.ToString("yyyy-MM-dd") + " " + txtRtime.Text.Trim();

            //퇴원등록하면 ipd_torder_chk 에 추가
            Insert_Torder_Chk(pDbCon, strGbSTS, txtPtno.Text.Trim(), FnIpdNo, FnTrsNo, strIndate, dtpOutDate.Value.ToString("yyyy-MM-dd"));

            if (strGbSTS == "1" || strGbSTS == "2")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,ORDERCODE, COUNT(PTNO) CNT ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_IORDER ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PTNO      = '" + txtPtno.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND BDATE     > TO_DATE('" + dtpOutDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND GBSEND    = '*' ";
                SQL += ComNum.VBLF + "    AND (GBIOE ='I' OR GBIOE IS NULL )  ";
                SQL += ComNum.VBLF + "  GROUP BY BDATE,ORDERCODE ";
                SQL += ComNum.VBLF + "  ORDER BY CNT DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    if (Convert.ToInt64(Dt.Rows[0]["CNT"].ToString()) > 0)
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT AMSET4 FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                        SQL += ComNum.VBLF + "  WHERE IPDNO     = " + FnIpdNo + " ";
                        SQL += ComNum.VBLF + "    AND OUTDATE IS NULL ";
                        SQL += ComNum.VBLF + "    AND DEPTCODE  = 'PD' ";
                        SQL += ComNum.VBLF + "    AND AMSET4    = '3' "; // 정상애기
                        SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                            DtSub.Dispose();
                            DtSub = null;
                            return;
                        }

                        if (DtSub.Rows.Count > 0)
                            ComFunc.MsgBox("** 정상애기 **  정상입원수속 또는 병동오더를 정리하시기 바랍니다.", "알림");
                        else
                        {
                            if (DialogResult.No == ComFunc.MsgBoxQ("퇴원날짜 이후의 오더 있음. 확인후 처리요망 !! 구분변경 환자입니까? ", "알림"))
                            {
                                strBdate = "";

                                for (int i = 0; i < Dt.Rows.Count; i++)
                                    strBdate += "오더발생일자 : " + Dt.Rows[i]["BDATE"].ToString() + " " + Dt.Rows[i]["ORDERCODE"].ToString() + " 오더갯수 : " + Dt.Rows[i]["CNT"].ToString() + " " + '\r';

                                clsPublic.GstrMsgTitle = "확인";
                                clsPublic.GstrMsgList = strBdate + '\r';
                                clsPublic.GstrMsgList += VB.DateAdd("D", 1, dtpOutDate.Text).ToString("yyyy-MM-dd") + " 이후 오더가 발생하였습니다." + '\r';
                                clsPublic.GstrMsgList += "꼭 병동에서 오더를 정리하시기 바랍니다.";
                                ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                            }
                        }

                        DtSub.Dispose();
                        DtSub = null;
                    }
                }

                Dt.Dispose();
                Dt = null;

                //퇴원작업 시점 포함 이전 미전송오더 체크
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,ORDERCODE,COUNT(PTNO) CNT ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_IORDER  ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PTNO      = '" + txtPtno.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND GBSEND    = '*' ";
                SQL += ComNum.VBLF + "    AND (GBIOE = 'I' OR GBIOE IS NULL ) ";
                SQL += ComNum.VBLF + "    AND BDATE     >= TO_DATE('" + strIndate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND BDATE     <= TO_DATE('" + dtpOutDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  GROUP BY TO_CHAR(BDATE,'YYYY-MM-DD'),ORDERCODE ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    clsPublic.GstrMsgList = "";

                    for (int i = 0; i < Dt.Rows.Count; i++)
                        clsPublic.GstrMsgList += Dt.Rows[i]["BDATE"].ToString().Trim() + "일 " + Dt.Rows[i]["ORDERCODE"].ToString().Trim() + " " + Dt.Rows[i]["CNT"].ToString().Trim() + "건 \r";


                    clsPublic.GstrMsgTitle = "확인";
                    clsPublic.GstrMsgList += '\r' + "미전송 오더가 있습니다. 꼭 병동에서 오더를 정리해주세요..";
                    clsPublic.GMsgButtons = MessageBoxButtons.OK;
                    clsPublic.GMsgIcon = MessageBoxIcon.Information;
                    MessageBox.Show(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, clsPublic.GMsgButtons, clsPublic.GMsgIcon);

                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                Dt.Dispose();
                Dt = null;


                //퇴원작업 시점 포함 이전 미전송오더 체크('EN','PC','OT') 컨설트 오더 추가 확인 2018-11-19
                SQL = "";
                SQL += ComNum.VBLF + " SELECT to_char(bdate,'yyyy-mm-dd') bdate ,TODEPTCODE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_Itransfer  ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PTNO      = '" + txtPtno.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND IPDNO     = " + FnIpdNo + " ";
                SQL += ComNum.VBLF + "    AND gbsend =' ' ";
                SQL += ComNum.VBLF + "    AND (gbdel =' ' or gbDel is null) ";
                SQL += ComNum.VBLF + "    AND todeptcode  in ('EN','PC','OT') ";
                SQL += ComNum.VBLF + "    AND  (gbconfirm =' ' or  substr(todrcode ,3,2) ='99')  ";
               
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    clsPublic.GstrMsgList = "";

                    for (int i = 0; i < Dt.Rows.Count; i++)
                        clsPublic.GstrMsgList += Dt.Rows[i]["BDATE"].ToString().Trim() + "일 " + Dt.Rows[i]["TODEPTCODE"].ToString().Trim() + " 컨설트 답변 누락건 \r";


                    clsPublic.GstrMsgTitle = "확인";
                    clsPublic.GstrMsgList += '\r' + "미전송 오더가 있습니다. 꼭 병동에서 오더를 정리해주세요..";
                    clsPublic.GMsgButtons = MessageBoxButtons.OK;
                    clsPublic.GMsgIcon = MessageBoxIcon.Information;
                    MessageBox.Show(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, clsPublic.GMsgButtons, clsPublic.GMsgIcon);

                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                Dt.Dispose();
                Dt = null;

                //수술처방 미전송 체크
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(OPDATE,'YYYY-MM-DD') OPDATE,COUNT(*) CNT ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ORAN_SLIP ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND OPDATE    >= TO_DATE('" + strIndate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND IPDOPD    = 'I' ";
                SQL += ComNum.VBLF + "    AND SLIPSEND IS NULL ";
                SQL += ComNum.VBLF + "    AND JepCode  NOT IN ( SELECT Code FROM KOSMOS_PMPA.OPR_CODE  WHERE Gubun = 'C'  ) ";
                SQL += ComNum.VBLF + "    AND WRTNO     > 0 ";
                SQL += ComNum.VBLF + "  GROUP BY TO_CHAR(OPDATE,'YYYY-MM-DD') ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SUM(QTY) TQTY ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ORAN_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND OPDATE    >= TO_DATE('" + strIndate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text.Trim() + "' ";
                    SQL += ComNum.VBLF + "    AND IPDOPD    = 'I' ";
                    SQL += ComNum.VBLF + "    AND SLIPSEND IS NULL ";
                    SQL += ComNum.VBLF + "    AND JepCode  NOT IN ( SELECT Code FROM KOSMOS_PMPA.OPR_CODE  WHERE Gubun = 'C'  ) ";
                    SQL += ComNum.VBLF + "    AND WRTNO     > 0 ";
                    SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                        DtSub.Dispose();
                        DtSub = null;
                        return;
                    }

                    if (DtSub.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(DtSub.Rows[0]["TQTY"].ToString()) > 0)
                        {
                            clsPublic.GstrMsgList = "";

                            for (int i = 0; i < Dt.Rows.Count; i++)
                                clsPublic.GstrMsgList += Dt.Rows[i]["OPDATE"].ToString().Trim() + "일 " + Dt.Rows[i]["CNT"].ToString().Trim() + "건" + '\r';

                            clsPublic.GstrMsgTitle = "확인";
                            clsPublic.GstrMsgList += '\r' + "처방 확정되지않은 오더가 있습니다. 꼭 수술방(마취과)에서 처방확정을 하시기 바랍니다.";
                            clsPublic.GMsgButtons = MessageBoxButtons.OK;
                            clsPublic.GMsgIcon = MessageBoxIcon.Information;
                            MessageBox.Show(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, clsPublic.GMsgButtons, clsPublic.GMsgIcon);

                            DtSub.Dispose();
                            DtSub = null;
                            Dt.Dispose();
                            Dt = null;
                            return;
                        }
                    }

                    DtSub.Dispose();
                    DtSub = null;
                }

                Dt.Dispose();
                Dt = null;
            }

            if (strGbSTS.Trim() != "0" && strGbSTS.Trim() != "")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT PANO FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP ";
                SQL += ComNum.VBLF + "  WHERE IPDNO     = " + FnIpdNo + " ";
                SQL += ComNum.VBLF + "    AND TRSNO     = " + FnTrsNo + " ";
                SQL += ComNum.VBLF + "    AND ACTDATE   > TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  GROUP BY PANO ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("오늘날짜보다 큰 입원SLIP이 있습니다..퇴원등록 안됨!!! 확인요망", "알림");
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                Dt.Dispose();
                Dt = null;
            }

            //보험증 검인기간 Check
            if (clsPmpaType.TIT.Remark == null || clsPmpaType.TIT.Remark.Trim() == "" || clsPmpaType.TIT.Remark.Substring(13, 1) != "")
            {
                
            }
            else
            {
                strGigan = clsPmpaType.TIT.Remark.Substring(7, 2) + "-" + clsPmpaType.TIT.Remark.Substring(9, 2) + "-" + clsPmpaType.TIT.Remark.Substring(11, 2);

                if (string.Compare(strGigan.Substring(0, 2), "60") > 0)
                    strGigan = "19" + strGigan;
                else
                    strGigan = "20" + strGigan;

                if (clsPmpaType.TIT.Bi == "12" || clsPmpaType.TIT.Bi == "13")
                {
                    if (string.Compare(strGigan, clsPmpaType.TIT.OutDate) < 0)
                    {
                        clsPublic.GstrMsgTitle = "경고";
                        clsPublic.GstrMsgList = "검인기간 : " + strGigan + '\r';
                        clsPublic.GstrMsgList += "퇴원일자 : " + clsPmpaType.TIT.OutDate + '\r';
                        clsPublic.GstrMsgList += "검인기간이 초과되었습니다." + '\r';
                        clsPublic.GstrMsgList += "원무과에 문의하시기 바랍니다." + '\r';
                        clsPublic.GstrMsgList += "퇴원등록을 하시겠습니까?" + '\r';
                        clsPublic.GMsgButtons = MessageBoxButtons.YesNo;
                        clsPublic.GMsgIcon = MessageBoxIcon.Question;
                        if (DialogResult.No == MessageBox.Show(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, clsPublic.GMsgButtons, clsPublic.GMsgIcon))
                            return;
                    }
                }
            }

            //가퇴원 등록시간
            strGatewonTime = "";
            if (FstrGatewonTime != "") { strGatewonTime = FstrGatewonTime; }
            if (strGatewonTime == "") { strGatewonTime = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime; }

            //엄마 퇴원등록시 애기가 있는지 확인
            if (txtDept.Text.Trim() == "OG")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT PANO, SNAME ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND OUTDATE IS NULL ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = 'PD' ";
                SQL += ComNum.VBLF + "    AND AMSET4    = '3' ";
                SQL += ComNum.VBLF + "    AND JUPBONO   = '" + txtPtno.Text.Trim() + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    clsPublic.GstrMsgTitle = "확인";
                    clsPublic.GstrMsgList = "정상 애기입니다." + '\r' + '\r';
                    clsPublic.GstrMsgList += "등록번호 : " + Dt.Rows[0]["PANO"].ToString() + '\r';
                    clsPublic.GstrMsgList += "성    명 : " + Dt.Rows[0]["SNAME"].ToString() + '\r' + '\r';
                    clsPublic.GstrMsgList += "꼭 퇴원여부를 확인하시기 바랍니다." + '\r';
                    clsPublic.GMsgButtons = MessageBoxButtons.OK;
                    clsPublic.GMsgIcon = MessageBoxIcon.Information;
                    MessageBox.Show(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, clsPublic.GMsgButtons, clsPublic.GMsgIcon);
                }

                Dt.Dispose();
                Dt = null;
            }

            if (Convert.ToInt32(FstrGbSTS) >= 2 && Convert.ToInt32(FstrGbSTS) <= 5)
            {
                if (strGbSTS == "0")
                {
                    if (DialogResult.No == MessageBox.Show("퇴원을 취소하고 재원상태로 변경하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        return;
                }
                else if (strGbSTS == "1")
                {
                    if (DialogResult.No == MessageBox.Show("퇴원을 취소하고 가퇴원상태로 변경하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        return;
                }
                else if (strGbSTS == "2")
                {
                    if (DialogResult.No == MessageBox.Show("심사계 대조리스트를 다시 인쇄하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        return;
                }
            }

            strTime = ":00";
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
                strTime = VB.Right(Dt.Rows[0]["SDATE"].ToString(), 3);

            Dt.Dispose();
            Dt = null;

            strROutTime += strTime;

            clsIuSentChk CIC = new clsIuSentChk();
            if (strGbSTS == "2" && FstrGbSuday == "")
            {
                strIntime = CIC.Rtn_IpTime(pDbCon, FnIpdNo);

                if (string.Compare(CPF.Date_Time_Hour(pDbCon, strIntime, clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime), "06:00") < 0)
                {
                    clsPublic.GstrMsgTitle = "확인";
                    clsPublic.GstrMsgList = "입원시간 6시간이 경과되지 않았습니다." + '\r' + '\r';
                    clsPublic.GstrMsgList += "입원취소 작업대상자임."+ '\r';
                    clsPublic.GstrMsgList += "입원시각 : " + strIntime + '\r';
                    clsPublic.GstrMsgList += "퇴원처리를 진행하시겠습니까?" + '\r';
                    clsPublic.GMsgButtons = MessageBoxButtons.YesNo;
                    clsPublic.GMsgIcon = MessageBoxIcon.Question;
                    if (DialogResult.No == MessageBox.Show(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, clsPublic.GMsgButtons, clsPublic.GMsgIcon))
                        return;
                }

            }

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = "";
            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS ";
            SQL += ComNum.VBLF + "    SET GbSTS  = '" + strGbSTS + "', ";
            
            if ( strGbSTS == "0") //재원
            {
                SQL += ComNum.VBLF + "    OutDate       = '', ";
                SQL += ComNum.VBLF + "    RoutDate      = '', ";
                SQL += ComNum.VBLF + "    SimsaTime     = '', ";
                SQL += ComNum.VBLF + "    PrintTime     = '', ";
                SQL += ComNum.VBLF + "    Tewon_sabun   = 0 , ";
                SQL += ComNum.VBLF + "    GbCheckList   = '', ";
                SQL += ComNum.VBLF + "    AMSET5        = ''  ";
            }
            else if ( strGbSTS == "1") //가퇴원
            {
                SQL += ComNum.VBLF + "    OutDate       = TO_DATE('" + dtpOutDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "    RoutDate      = '', ";
                SQL += ComNum.VBLF + "    SimsaTime     = '', ";
                SQL += ComNum.VBLF + "    PrintTime     = '', ";
                SQL += ComNum.VBLF + "    Tewon_sabun   = " + clsType.User.IdNumber + ", ";
                SQL += ComNum.VBLF + "    GbCheckList   = '' , ";
                SQL += ComNum.VBLF + "    AMSET5        = ''  ";
            }
            else if (strGbSTS == "2") //퇴원접수
            {
                SQL += ComNum.VBLF + "    OutDate       = TO_DATE('" + dtpOutDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "    RoutDate      = TO_DATE('" + strROutTime + "','YYYY-MM-DD HH24:MI:SS'), ";
                SQL += ComNum.VBLF + "    SimsaTime     = '', ";
                SQL += ComNum.VBLF + "    PrintTime     = '', ";
                SQL += ComNum.VBLF + "    Tewon_sabun   = " + clsType.User.IdNumber + ", ";
                SQL += ComNum.VBLF + "    GbCheckList   = '', ";
                SQL += ComNum.VBLF + "    AMSET5        = '" + cboBun.Text.Substring(0, 1) + "' ";  //퇴원구분1.완쾌, 2.자위, 3.사망, 4.전원
            }
            else if (strGbSTS == "3") //대조리스트인쇄
            {
                SQL += ComNum.VBLF + "    OutDate       = TO_DATE('" + dtpOutDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "    RoutDate      = TO_DATE('" + strROutTime + "','YYYY-MM-DD HH24:MI:SS'), ";
                SQL += ComNum.VBLF + "    SimsaTime     = '', ";
                SQL += ComNum.VBLF + "    PrintTime     = '', ";                                                    
                SQL += ComNum.VBLF + "    Tewon_sabun   = " + clsType.User.IdNumber + ", ";
                SQL += ComNum.VBLF + "    GbCheckList   = 'Y' , ";
                SQL += ComNum.VBLF + "    AMSET5        = '" + cboBun.Text.Substring(0, 1) + "' ";  //퇴원구분1.완쾌, 2.자위, 3.사망, 4.전원
            }

            SQL += ComNum.VBLF + "  WHERE IPDNO         = " + FnIpdNo + " ";
            SQL += ComNum.VBLF + "    AND TRSNO         = " + FnTrsNo + " ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT COUNT(*) CNT FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
            SQL += ComNum.VBLF + "  WHERE IPDNO = " + FnIpdNo + " ";
            SQL += ComNum.VBLF + "    AND OUTDATE IS NULL ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Convert.ToInt32(Dt.Rows[0]["CNT"].ToString()) == 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER  ";
                SQL += ComNum.VBLF + "    SET OutDate   = TO_DATE('" + dtpOutDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "        JDate     = TO_DATE('" + dtpOutDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "        GBSTS     = '2' ";
                SQL += ComNum.VBLF + "  WHERE IPDNO     = " + FnIpdNo + " ";
                SQL += ComNum.VBLF + "    AND OutDate IS NULL ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER  ";
                SQL += ComNum.VBLF + "    SET GBSTS = '2' ";
                SQL += ComNum.VBLF + "  WHERE IPDNO = " + FnIpdNo + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            else if (strGbSTS == "3") //대조리스트로 변경
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT GBSTS ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL += ComNum.VBLF + "  Where IPDNO = " + FnIpdNo + " ";
                SQL += ComNum.VBLF + "    AND GBSTS = '0' ";
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    DtSub.Dispose();
                    DtSub = null;
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                if (DtSub.Rows.Count == 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE IPD_NEW_MASTER ";
                    SQL += ComNum.VBLF + "    SET GBSTS     = 3 , ";
                    SQL += ComNum.VBLF + "        SimsaTime = '', ";
                    SQL += ComNum.VBLF + "        PRINTTIME = ''  ";
                    SQL += ComNum.VBLF + "  WHERE IPDNO     = " + FnIpdNo + " ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                DtSub.Dispose();
                DtSub = null;
            }
            else if (strGbSTS == "0") //재원으로 변경
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + "    SET OutDate       = '', ";
                SQL += ComNum.VBLF + "        JDate         = TO_DATE('1900-01-01','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "        GBSTS         = 0, ";
                SQL += ComNum.VBLF + "        GATEWONTIME   = '', ";
                SQL += ComNum.VBLF + "        PRINTTIME     = '' ";
                SQL += ComNum.VBLF + "  WHERE IPDNO         = " + FnIpdNo + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            Dt.Dispose();
            Dt = null;

            //심사자 history
            clsPmpaQuery CPQ = new clsPmpaQuery();
            if (strGbSTS.Trim() != "")
                CPQ.Simsa_History_SAVE(pDbCon, "", "심사취소", txtPtno.Text.Trim(), FnIpdNo, FnTrsNo, strGbSTS, strBi, strIndate, dtpOutDate.Value.ToString("yyyy-MM-dd"), strSname, "");

            if (clsType.User.Grade == "EDPS" && FstrGbSTS != "0")
                CPF.IPD_DRUG_BBBBBB_DELETE(pDbCon, txtPtno.Text.Trim(), FnIpdNo, FnTrsNo);

            //퇴원취소의 경우 이전날의 ArcDate를 강제 업데이트
            //가퇴원인경우 다음날 재원으로 돌리게 되면 가퇴원 당일 식대가 생성안됨-> 가퇴원시 가퇴원하는 전날로 ArcDate 세팅되어있음
            //가퇴원하고 다음날 재원으로 돌리는 경우 가퇴원전날로 세팅되어있는 ArcDate가 가퇴원하는날(재원으로 돌리는 날 전날)로 세팅되어짐으로 퇴원날 식대 발생안됨
            int nArcQty = 0;
            string strArcDate = "";

            if (Convert.ToInt32(FstrGbSTS) <= 4 && strGbSTS == "0")
            {
                strArcDate = VB.DateAdd("D", -1, dtpRdate.Text).ToString("yyyy-MM-dd");
                nArcQty = CF.DATE_ILSU(pDbCon, strArcDate, strIndate.Substring(0, 10));

                if (string.Compare(strArcDate, strIndate) < 0)
                {
                    strArcDate = "";
                    nArcQty = 0;
                }

                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER SET ";

                if (strArcDate == "")
                    SQL += ComNum.VBLF + "    ArcDate   = '', ";
                else
                    SQL += ComNum.VBLF + "    ArcDate   = TO_DATE('" + strArcDate + "','YYYY-MM-DD'), ";

                SQL += ComNum.VBLF + "        ArcQty    = " + nArcQty + " ";
                SQL += ComNum.VBLF + "  WHERE IPDNO     = " + FnIpdNo + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            clsDB.setCommitTran(pDbCon);
            
            Screen_Clear();
            Read_Ipd_Trans(pDbCon);
            Screen_Display(pDbCon);

            txtSPtno.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgGbSTS"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTrsNo"></param>
        /// <param name="ArgIndate"></param>
        /// <param name="ArgOutDate"></param>
        void Insert_Torder_Chk(PsmhDb pDbCon, string ArgGbSTS, string ArgPtno, long ArgIpdNo, long ArgTrsNo, string ArgIndate, string ArgOutDate)
        {
            if (ArgGbSTS != "0")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT PANO FROM " + ComNum.DB_PMPA + "IPD_TORDER_CHK ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND IPDNO =  " + ArgIpdNo + " ";
                SQL += ComNum.VBLF + "    AND TRSNO =  " + ArgTrsNo + " ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);       //에러로그 저장
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    clsDB.setBeginTran(pDbCon);

                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "IPD_TORDER_CHK ";
                    SQL += ComNum.VBLF + "        (PANO,IPDNO,TRSNO, ";
                    SQL += ComNum.VBLF + "         ACTDATE,INDATE,OUTDATE ) ";
                    SQL += ComNum.VBLF + " VALUES ('" + ArgPtno + "', ";
                    SQL += ComNum.VBLF + "          " + ArgIpdNo + ", ";
                    SQL += ComNum.VBLF + "          " + ArgTrsNo + ", ";
                    SQL += ComNum.VBLF + "          SYSDATE, ";
                    SQL += ComNum.VBLF + "          TO_DATE('" + ArgIndate + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "          TO_DATE('" + ArgOutDate + "','YYYY-MM-DD')) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(pDbCon);
                }

                Dt.Dispose();
                Dt = null;

                clsDB.setBeginTran(clsDB.DbCon);

                SQL = "";
                SQL += ComNum.VBLF + " DELETE FROM " + ComNum.DB_PMPA + "IPD_TORDER_CHK ";
                SQL += ComNum.VBLF + "  WHERE ACTDATE < TRUNC(SYSDATE) ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);
            }
        }

        void Print_Process()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) { return; }     //권한확인

            strTitle = "퇴원예정환자명단";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssIpdList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtSPtno && e.KeyChar == (char)13)
                Read_Ipd_Trans(clsDB.DbCon);
            else if (sender == this.txtSSname && e.KeyChar == (char)13)
                Read_Ipd_Trans(clsDB.DbCon);
            else if (sender == this.txtPtno && e.KeyChar == (char)13)
                Ptno_Check();
        }

        void Ptno_Check()
        {
            if (txtPtno.Text.Trim() == "") { return; }

            if (clsLockCheck.GstrLockPtno != "")
            {
                clsLockCheck.IpdOcs_Lock_Delete_NEW(clsLockCheck.GstrLockPtno);
            }

            clsLockCheck.GstrLockPtno = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            txtPtno.Text = string.Format("{0:D8}", Convert.ToInt64(txtPtno.Text));

            Read_Ipd_Trans_Person(clsDB.DbCon);

            if (FstrPtnoFind == "NO")
            {
                Screen_Clear();
                txtPtno.SelectAll();
                txtSPtno.Focus();
                return;
            }

            btnOk.Enabled = true;
            btnCancel.Enabled = true;
        }

        void Read_Ipd_Trans_Person(PsmhDb pDbCon)
        {
            ComFunc CF = new ComFunc();

            FstrPtnoFind = "NO";
            FstrGbSTS = "";
            FstrGatewonTime = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.IPDNO, A.TRSNO, A.Pano, ";
            SQL += ComNum.VBLF + "        B.Sname, B.Sex, B.Age, ";
            SQL += ComNum.VBLF + "        B.WardCode, B.RoomCode,  ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.InDate,'YYYY-MM-DD') InDate, ";
            SQL += ComNum.VBLF + "        A.Bi, A.Ilsu, A.AMSETB, ";
            SQL += ComNum.VBLF + "        A.AMSET1, A.DeptCode,A.DrCode, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DrCode) DRNAME, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.OutDate,'YYYY-MM-DD') OutDate, ";
            SQL += ComNum.VBLF + "        A.AMSET5, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.RoutDate,'YYYY-MM-DD HH24:MI') RoutDate, ";
            SQL += ComNum.VBLF + "        A.GbSTS , A.KIHO, B.GBSUDAY ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_MASTER B ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND A.Pano    = '" + txtPtno.Text + "'  ";
            SQL += ComNum.VBLF + "    AND A.IPDNO   = '" + FnIpdNo + "' ";
            SQL += ComNum.VBLF + "    AND A.TRSNO   = '" + FnTrsNo + "' ";
            SQL += ComNum.VBLF + "    AND A.ActDate IS NULL  ";
            SQL += ComNum.VBLF + "    AND A.IPDNO   = B.IPDNO ";
            SQL += ComNum.VBLF + "  ORDER BY A.InDate ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                txtSname.Text = Dt.Rows[0]["SNAME"].ToString().Trim();
                txtSexAge.Text = Dt.Rows[0]["SEX"].ToString().Trim() + "/" + Dt.Rows[0]["AGE"].ToString().Trim();
                txtWardRoom.Text = Dt.Rows[0]["WARDCODE"].ToString().Trim() + "/" + Dt.Rows[0]["ROOMCODE"].ToString().Trim();
                txtInDate.Text = Dt.Rows[0]["INDATE"].ToString().Trim();
                txtIlsu.Text = Dt.Rows[0]["ILSU"].ToString().Trim();
                txtBi.Text = Dt.Rows[0]["BI"].ToString().Trim();
                txtDept.Text = Dt.Rows[0]["DEPTCODE"].ToString().Trim();
                txtDr.Text = Dt.Rows[0]["DRNAME"].ToString().Trim();
                dtpOutDate.Text = Dt.Rows[0]["OUTDATE"].ToString().Trim();
                dtpRdate.Text = VB.Left(Dt.Rows[0]["ROUTDATE"].ToString().Trim(), 10);
                txtRtime.Text = VB.Right(Dt.Rows[0]["ROUTDATE"].ToString().Trim(), 5);
                txtOutDateTime.Text = Dt.Rows[0]["ROUTDATE"].ToString().Trim();

                FstrGbSTS = Dt.Rows[0]["GbSTS"].ToString().Trim();
                txtSTS.Text = FstrGbSTS + "." + CF.Read_Bcode_Name(pDbCon, "IPD_입원상태", FstrGbSTS);

                if (FstrGbSTS == "0" || FstrGbSTS == "1")
                    cboSTS.SelectedIndex = 3;
                else
                {
                    cboSTS.SelectedIndex = -1;
                    for (int i = 0; i < cboSTS.Items.Count; i++)
                    {
                        cboSTS.SelectedIndex = i;
                        if (FstrGbSTS == cboSTS.Text.Substring(0, 1))
                            break;
                    }
                }

                string strAmSet5 = Dt.Rows[0]["AMSET5"].ToString().Trim();
                for (int i = 0; i < cboBun.Items.Count; i++)
                {
                    cboBun.SelectedIndex = i;
                    if (strAmSet5 == cboBun.Text.Substring(0,1))
                        break;
                }

                if (strAmSet5 == "0" || strAmSet5 == "") { cboBun.SelectedIndex = 1; }
                
                txtIpdNo.Text = Dt.Rows[0]["IPDNO"].ToString().Trim();
                FnIpdNo = Convert.ToInt64(txtIpdNo.Text);
                txtTrsNo.Text = Dt.Rows[0]["TRSNO"].ToString().Trim();
                FnTrsNo = Convert.ToInt64(txtTrsNo.Text);
                
                txtKiho.Text = Dt.Rows[0]["KIHO"].ToString().Trim();
                txtPaKiho.Text = CF.Read_MiaName(pDbCon, txtKiho.Text, false);
            }
            else
            {
                sBarMsg1.Text = "재원자 등록번호가 아님";
                Dt.Dispose();
                Dt = null;
                return;
            }

            Dt.Dispose();
            Dt = null;

            //퇴원일자, 퇴원예정일자를 기본 Setting
            ComFunc.ReadSysDate(pDbCon);

            if (string.Compare(clsPublic.GstrSysTime, "20:00") >= 0)
            {
                if (dtpOutDate.Text == "") { dtpOutDate.Value = VB.DateAdd("D", 1, clsPublic.GstrSysDate); }
                if (dtpRdate.Text == "") { dtpRdate.Value = VB.DateAdd("D", 1, clsPublic.GstrSysDate); }
            }
            else
            {
                if (dtpOutDate.Text == "") { dtpOutDate.Text = clsPublic.GstrSysDate; }
                if (dtpRdate.Text.Trim() == "")
                {
                    dtpRdate.Text = clsPublic.GstrSysDate;
                }
                txtRtime.Text = clsPublic.GstrSysTime;
            }

            cboSTS.Enabled = false;
            FstrPtnoFind = "";

            if (Convert.ToInt32(FstrGbSTS) < 5)
            {
                cboSTS.Enabled = true;
                FstrPtnoFind = "OK";
            }
        }

        void Read_Ipd_Trans(PsmhDb pDbCon)
        {
            clsSpread CS = new clsSpread();

            int nRow = 0;

            if (txtSPtno.Text.Trim() != "")
                txtSPtno.Text = string.Format("{0:D8}", Convert.ToInt64(txtSPtno.Text));

            CS.Spread_Clear_Range(ssList, 0, 0, ssList_Sheet1.RowCount, ssList_Sheet1.ColumnCount);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.IPDNO, A.TRSNO, A.Pano, ";
            SQL += ComNum.VBLF + "        B.SName, A.DeptCode,B.RoomCode, ";
            SQL += ComNum.VBLF + "        TO_CHAR(A.InDate,'YYYY-MM-DD') InDate, ";
            SQL += ComNum.VBLF + "        B.Age, B.GBSuDay,decode(A.GBIPD,'9','지병') GBIPD ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_MASTER B ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND A.ActDate IS NULL ";
            SQL += ComNum.VBLF + "    AND B.JDATE   = TO_DATE('1900-01-01','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.GBIPD IN ('1','9')  ";
            SQL += ComNum.VBLF + "    AND A.IPDNO   = B.IPDNO(+)  ";
            SQL += ComNum.VBLF + "    AND A.GBSTS IN ('0')  ";

            if (cboWard.Text != "전체")
                SQL += ComNum.VBLF + "AND B.WardCode = '" + cboWard.Text.Trim() + "' ";

            if (txtSSname.Text.Trim() != "")
                SQL += ComNum.VBLF + "AND B.SName LIKE '%" + txtSSname.Text.Trim() + "%' ";

            if (txtSPtno.Text.Trim() != "")
                SQL += ComNum.VBLF + "AND B.PANO    = '" + txtSPtno.Text.Trim() + "' ";

            SQL += ComNum.VBLF + "  ORDER BY 4,3 ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    nRow += 1;
                    if (ssList_Sheet1.RowCount < nRow)
                    {
                        ssList_Sheet1.RowCount = nRow;
                    }

                    ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["RoomCode"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["InDate"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["IPDNO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["TRSNO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["AGE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["GBSuDay"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["GBIPD"].ToString().Trim();
                }
            }

            Dt.Dispose();
            Dt = null;

            txtSSname.Text = "";
        }

        void eCbo_Wheel(object sender, MouseEventArgs e)
        {
            ComboBox CB = sender as ComboBox;

            if (CB.Focused == false)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        void eFrm_Load(object sender, EventArgs e)
        {
            clsSpread CS = new clsSpread();
            ComFunc CF = new ComFunc();
            clsVbfunc CV = new clsVbfunc();

            Control[] con = ComFunc.GetAllControls(this);

            if (con != null)
            {
                foreach (Control ctl in con)
                {
                    if (ctl is TextBox)
                    {
                        ((TextBox)ctl).Text = "";
                    }

                    if (ctl is ComboBox)
                    {
                        ((ComboBox)ctl).Text = "";
                    }

                    if (ctl is CheckBox)
                    {
                        ((CheckBox)ctl).Checked = false;
                    }

                    if (ctl is DateTimePicker)
                    {
                        ((DateTimePicker)ctl).Checked = false;
                    }
                }
            }

            CS.Spread_Clear_Range(ssList, 0, 0, ssList_Sheet1.RowCount, ssList_Sheet1.ColumnCount);
            CS.Spread_Clear_Range(ssIpdList, 0, 0, ssIpdList_Sheet1.RowCount, ssIpdList_Sheet1.ColumnCount);

            clsLockCheck.GstrLockPtno = "";

            Screen_Clear();

            cboWard.Items.Clear();
            cboWard.Items.Add("전체");
            CV.Combo_Wardcode_Set(clsDB.DbCon, cboWard, "0", false, 2);
            cboWard.SelectedIndex = 0;

            CF.Combo_BCode_SET(clsDB.DbCon, cboBun, "IPD_퇴원종류", true, 1, "");
            CF.Combo_BCode_SET(clsDB.DbCon, cboSTS, "IPD_입원상태", true, 1, "");

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SABUN,KORNAME ";
            SQL += ComNum.VBLF + "   FRom " + ComNum.DB_ERP + "INSA_MST ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND BUSE  = '078201' ";
            SQL += ComNum.VBLF + "    AND TOIDAY IS NULL ";
            SQL += ComNum.VBLF + "  ORDER By Jik,Sabun ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                    FstrJSimSabun[i] = Dt.Rows[i]["SABUN"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            FbJSim_Flag = false;

            Screen_Display(clsDB.DbCon);

            //frmPmpaDischargeReg frm = new frmPmpaDischargeReg();
            //ComFunc.Form_Center(frm);

            CS.setColStyle(ssIpdList, -1, 12, clsSpread.enmSpdType.Hide);
            CS.setColStyle(ssIpdList, -1, 13, clsSpread.enmSpdType.Hide);
            CS.setColStyle(ssIpdList, -1, 14, clsSpread.enmSpdType.Hide);
            CS.setColStyle(ssIpdList, -1, 15, clsSpread.enmSpdType.Hide);
            CS.setColStyle(ssIpdList, -1, 25, clsSpread.enmSpdType.Hide);
        }

        void Screen_Display(PsmhDb pDbCon)
        {
            ComFunc CF = new ComFunc();

            string strGbSTS = "";
            string strGbTAX = "";
            string strAmSet3 = "";

            ssIpdList_Sheet1.Rows.Count = 0;
            FstrRoomOver = "";

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.RoomCode, c.TBED, SUM(DECODE(a.PANO,'1',1,1)) UCNT ,";
            SQL += ComNum.VBLF + "        c.TBED - SUM(DECODE(a.PANO,'1',1,1)) UCNT2 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_ROOM c ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND a.ActDate  IS NULL ";
            SQL += ComNum.VBLF + "    AND a.ROOMCODE    = c.ROOMCODE ";
            SQL += ComNum.VBLF + "    AND (a.OUTDATE IS NULL OR a.OUTDATE >= TRUNC(SYSDATE) )  ";
            SQL += ComNum.VBLF + " GROUP BY a.RoomCode,c.TBED ";
            SQL += ComNum.VBLF + "  HAVING  c.TBED - SUM(DECODE(a.PANO,'1',1,1)) < 0 ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                    FstrRoomOver += Dt.Rows[i]["RoomCode"].ToString().Trim() + ",";
            }

            Dt.Dispose();
            Dt = null;

            if (chkName.Checked == false)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT /*+use_concat*/ B.IPDNO, B.Pano,a.Sname, ";
                SQL += ComNum.VBLF + "        a.RoomCode,b.JinDtl, ";
                SQL += ComNum.VBLF + "        TO_CHAR(A.InDate,'YYYY-MM-DD') RealInDate, ";
                SQL += ComNum.VBLF + "        TO_CHAR(B.InDate,'YY/MM/DD') InDate, ";
                SQL += ComNum.VBLF + "        a.GbDrg, B.GbSTS, ";
                SQL += ComNum.VBLF + "        A.GbSTS GBSTS2, ";
                SQL += ComNum.VBLF + "        TO_CHAR(B.OutDate,'YY/MM/DD') OutDate, ";
                SQL += ComNum.VBLF + "        b.DeptCode, b.DrCode, KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(b.DrCode) DRNAME,";
                SQL += ComNum.VBLF + "        TO_CHAR(B.RoutDate,'YY/MM/DD HH24:MI:SS') RoutDate, ";
                SQL += ComNum.VBLF + "        TO_CHAR(B.SunapTime,'YY/MM/DD HH24:MI') SunapTime, ";
                SQL += ComNum.VBLF + "        TO_CHAR(B.SIMSATime,'YY/MM/DD HH24:MI') SIMSATime, ";
                SQL += ComNum.VBLF + "        b.TRSNO, b.Bi, b.GbIPD, ";
                SQL += ComNum.VBLF + "        b.AmSet3, B.AmSet5, B.AmSet4, ";
                SQL += ComNum.VBLF + "        a.GbSuDay,a.GbSPC, B.OGPDBUN, ";
                SQL += ComNum.VBLF + "        b.OGPDBUNdtl, B.VCODE,b.SimsaSabun, ";
                SQL += ComNum.VBLF + "        b.Tewon_Sabun, B.GBCHECKLIST,b.Gbilban2, ";
                SQL += ComNum.VBLF + "        B.JSIM_SABUN ,b.GbTax, a.WardCode, ";
                SQL += ComNum.VBLF + "        b.FCode,a.Jsim_Remark ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_TRANS b ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND (B.ActDate IS NULL or b.OUTDATE = TRUNC(SYSDATE) OR B.ActDate = TRUNC(SYSDATE)) ";
                SQL += ComNum.VBLF + "    AND B.GbSTS IN ('1','2','3','4','5','6','7') ";
                SQL += ComNum.VBLF + "    AND b.GbIPD IN ('1','9') ";
                SQL += ComNum.VBLF + "    AND (A.ActDate  IS NULL or A.OUTDATE = TRUNC(SYSDATE) OR A.ACTDATE  = TRUNC(SYSDATE))  ";
                SQL += ComNum.VBLF + "    AND a.IPDNO = b.IPDNO(+) ";
                SQL += ComNum.VBLF + "  ORDER BY B.ROUTDATE ASC , B.PANO, B.OutDate,a.RoomCode,B.IPDNO,b.TRSNO  ";
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT B.IPDNO, B.Pano,a.Sname, ";
                SQL += ComNum.VBLF + "        a.RoomCode,b.JinDtl,";
                SQL += ComNum.VBLF + "        TO_CHAR(A.InDate,'YYYY-MM-DD') RealInDate, ";
                SQL += ComNum.VBLF + "        TO_CHAR(B.InDate,'YY/MM/DD') InDate, ";
                SQL += ComNum.VBLF + "        a.GbDrg, B.GbSTS, ";
                SQL += ComNum.VBLF + "        TO_CHAR(B.OutDate,'YY/MM/DD') OutDate, ";
                SQL += ComNum.VBLF + "        b.DeptCode,b.DrCode, KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(b.DrCode) DRNAME,";
                SQL += ComNum.VBLF + "        TO_CHAR(B.RoutDate,'YY/MM/DD HH24:MI:SS') RoutDate, ";
                SQL += ComNum.VBLF + "        TO_CHAR(B.SunapTime,'YY/MM/DD HH24:MI') SunapTime, ";
                SQL += ComNum.VBLF + "        TO_CHAR(B.SIMSATime,'YY/MM/DD HH24:MI') SIMSATime, ";
                SQL += ComNum.VBLF + "        b.TRSNO,b.Bi,b.GbIPD, ";
                SQL += ComNum.VBLF + "        b.AmSet3, B.AmSet5,B.AmSet4, ";
                SQL += ComNum.VBLF + "        a.GbSuDay,a.GbSPC, B.OGPDBUN, ";
                SQL += ComNum.VBLF + "        b.OGPDBUNdtl, B.VCODE,b.SimsaSabun , ";
                SQL += ComNum.VBLF + "        b.Tewon_Sabun, B.GBCHECKLIST,b.Gbilban2, ";
                SQL += ComNum.VBLF + "        B.JSIM_SABUN,b.GbTax,a.WardCode, ";
                SQL += ComNum.VBLF + "        b.FCode, a.Jsim_Remark ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_TRANS b, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "NUR_TEWON_NAMESEND c ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND (b.ActDate IS NULL or b.OUTDATE = TRUNC(SYSDATE) OR B.ActDate = TRUNC(SYSDATE)) ";
                SQL += ComNum.VBLF + "    AND B.GbSTS IN ('0') ";
                SQL += ComNum.VBLF + "    AND a.IPDNO   = b.IPDNO(+)  ";
                SQL += ComNum.VBLF + "    AND a.IPDNO   = c.IPDNO ";
                SQL += ComNum.VBLF + "    AND (c.DelDate IS NULL OR c.DelDate ='') ";
                SQL += ComNum.VBLF + "    AND c.Gubun1  = '1' "; // 퇴원만, 가퇴원 전송제외
                SQL += ComNum.VBLF + "    AND b.GbIPD IN ('1','9') ";
                SQL += ComNum.VBLF + "    AND A.ActDate  IS NULL  ";
                SQL += ComNum.VBLF + "  ORDER BY c.EntDate DESC,B.ROUTDATE ASC , ";
                SQL += ComNum.VBLF + "           B.PANO, B.OutDate,a.RoomCode, ";
                SQL += ComNum.VBLF + "           B.IPDNO,b.TRSNO ";
            }
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                Dt.Dispose();
                Dt = null;
                return;
            }

            ssIpdList_Sheet1.RowCount = Dt.Rows.Count;
            //ssIpdList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            if (Dt.Rows.Count > 0)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    strGbSTS = Dt.Rows[i]["GBSTS"].ToString().Trim();
                    strGbTAX = Dt.Rows[i]["GbTax"].ToString().Trim();

                    switch (strGbSTS)//입원상태
                    {
                        case "2"://퇴원접수
                            ssIpdList_Sheet1.Cells[i, 0, i, ssIpdList_Sheet1.ColumnCount - 1].BackColor = Color.Fuchsia;
                            break;

                        case "6"://퇴원계산서인쇄
                            ssIpdList_Sheet1.Cells[i, 0, i, ssIpdList_Sheet1.ColumnCount - 1].BackColor = Color.PowderBlue;
                            break;

                        case "7"://퇴원수납완료
                            ssIpdList_Sheet1.Cells[i, 0, i, ssIpdList_Sheet1.ColumnCount - 1].BackColor = Color.Salmon;
                            break;

                        case "5"://심사완료
                            ssIpdList_Sheet1.Cells[i, 0, i, ssIpdList_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;
                            break;
                    }

                    if (strGbTAX == "1" && string.Compare(strGbSTS, "7") < 0)
                        ssIpdList_Sheet1.Cells[i, 0, i, ssIpdList_Sheet1.ColumnCount - 1].BackColor = Color.LightGoldenrodYellow;

                    ssIpdList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["PANO"].ToString();
                    ssIpdList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["BI"].ToString();
                    ssIpdList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["SNAME"].ToString();
                    ssIpdList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["ROOMCODE"].ToString();
                    ssIpdList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["INDATE"].ToString();
                    ssIpdList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["DEPTCODE"].ToString();
                    ssIpdList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["DRNAME"].ToString();
                    ssIpdList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["OUTDATE"].ToString();
                    ssIpdList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["GBDRG"].ToString();

                    ssIpdList_Sheet1.Cells[i, 9].Text = "";
                    if (Dt.Rows[i]["GBIPD"].ToString().Trim() == "9")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "지병 " + Dt.Rows[i]["VCODE"].ToString().Trim();
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "P" || Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "O")
                    {
                        if (Dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                            ssIpdList_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["VCODE"].ToString().Trim() + "+F008";
                        else if (Dt.Rows[i]["FCODE"].ToString().Trim() == "F010")
                            ssIpdList_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["VCODE"].ToString().Trim() + "+F010";
                        else
                        {
                            ssIpdList_Sheet1.Cells[i, 9].Text = "면제";
                            ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;

                            if (Dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                                ssIpdList_Sheet1.Cells[i, 9].Text = "결핵";
                        }
                    }
                    else if (Dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "중증E+ " + Dt.Rows[i]["VCODE"].ToString().Trim();
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "중증F+ " + Dt.Rows[i]["VCODE"].ToString().Trim();
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "1")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "E+V";
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "2")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "F+V";
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "1")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "중증E+V+ " + Dt.Rows[i]["VCODE"].ToString().Trim();
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "2")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "중증F+V+ " + Dt.Rows[i]["VCODE"].ToString().Trim();
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["VCODE"].ToString().Trim() == "V191" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V192" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V193" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V194" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V268" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V275")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "중증+ " + Dt.Rows[i]["VCODE"].ToString().Trim();
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E" && (Dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V231"))
                    {
                        if (Dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                            ssIpdList_Sheet1.Cells[i, 9].Text = "차상위E(" + Dt.Rows[i]["VCODE"].ToString().Trim() + "+F008)";
                        else
                        {
                            ssIpdList_Sheet1.Cells[i, 9].Text = "차상위E+★결핵★";
                            ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                        }
                    }
                    else if (Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F" && (Dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V231"))
                    {
                        if (Dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                            ssIpdList_Sheet1.Cells[i, 9].Text = "차상위F(" + Dt.Rows[i]["VCODE"].ToString().Trim() + "+F008)";
                        else
                        {
                            ssIpdList_Sheet1.Cells[i, 9].Text = "차상위F+★결핵★";
                            ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                        }
                    }
                    else if (Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E" && (Dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V250"))
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "중증화상E+" + Dt.Rows[i]["VCODE"].ToString().Trim();
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F" && (Dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V250"))
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "중증화상F+" + Dt.Rows[i]["VCODE"].ToString().Trim();
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "중증화상+" + Dt.Rows[i]["VCODE"].ToString().Trim();
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "H")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "희귀H";
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "V")
                    {
                        if (Dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                            ssIpdList_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["VCODE"].ToString().Trim() + "+F008";
                        else
                        {
                            ssIpdList_Sheet1.Cells[i, 9].Text = "희귀V";
                            ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;

                            if (Dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                                ssIpdList_Sheet1.Cells[i, 9].Text = "★결핵★";

                        }
                    }
                    else if (Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "C")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "차상";
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E")
                    {
                        if (Dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() != "")
                            ssIpdList_Sheet1.Cells[i, 9].Text = "차상E+" + Dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() + " " + Dt.Rows[i]["VCODE"].ToString().Trim();
                        else
                            ssIpdList_Sheet1.Cells[i, 9].Text = "차상E" + " " + Dt.Rows[i]["VCODE"].ToString().Trim();

                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F")
                    {
                        if (Dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() != "")
                            ssIpdList_Sheet1.Cells[i, 9].Text = "차상F+" + Dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() + " " + Dt.Rows[i]["VCODE"].ToString().Trim();
                        else
                            ssIpdList_Sheet1.Cells[i, 9].Text = "차상F" + " " + Dt.Rows[i]["VCODE"].ToString().Trim();

                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }
                    else if (Dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() != "")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "면제(" + Dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() + ")";
                    }
                    else if (Dt.Rows[i]["FCODE"].ToString().Trim() == "F014")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text = "장기입원";
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }

                    if (Dt.Rows[i]["JinDtl"].ToString().Trim() == "01")
                        ssIpdList_Sheet1.Cells[i, 9].Text += "+노인틀니";

                    if (Dt.Rows[i]["Gbilban2"].ToString().Trim() == "Y")
                    {
                        ssIpdList_Sheet1.Cells[i, 9].Text += "★외국일반2배★";
                        ssIpdList_Sheet1.Cells[i, 9, i, 9].BackColor = Color.Red;
                    }


                    ssIpdList_Sheet1.Cells[i, 10].Text = Dt.Rows[i]["ROutDate"].ToString();
                    ssIpdList_Sheet1.Cells[i, 11].Text = CF.Read_Bcode_Name(pDbCon, "IPD_입원상태", strGbSTS);
                    strAmSet3 = Dt.Rows[i]["AmSet3"].ToString().Trim();

                    ssIpdList_Sheet1.Cells[i, 12].Text = Dt.Rows[i]["IPDNO"].ToString();
                    ssIpdList_Sheet1.Cells[i, 13].Text = Dt.Rows[i]["TRSNO"].ToString();
                    ssIpdList_Sheet1.Cells[i, 14].Text = Dt.Rows[i]["GbSTS"].ToString();
                    ssIpdList_Sheet1.Cells[i, 15].Text = Dt.Rows[i]["AmSet3"].ToString();
                    ssIpdList_Sheet1.Cells[i, 16].Text = CF.Read_Bcode_Name(pDbCon, "IPD_퇴원종류", Dt.Rows[i]["AmSet5"].ToString().Trim());

                    if (Dt.Rows[i]["AMSET4"].ToString().Trim() == "3")
                        ssIpdList_Sheet1.Cells[i, 17].Text = "NBST";
                    else
                        ssIpdList_Sheet1.Cells[i, 17].Text = "";

                    //간호부에서 퇴원 예고 등록자 표시
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT TO_CHAR(ROUTDATE,'YYYY-MM-DD') ROUTDATE, ";
                    SQL += ComNum.VBLF + "        TO_CHAR(ROUTENTTIME ,'YYYY-MM-DD') ROUTENTDATE,";
                    SQL += ComNum.VBLF + "        TO_CHAR(ROUTENTTIME ,'HH24:MI') RTIME,";
                    SQL += ComNum.VBLF + "        TO_CHAR(ROUTENTTIME ,'YYYY-MM-DD HH24:MI') ROUTENTDATENEW";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_MASTER";
                    SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                    SQL += ComNum.VBLF + "    AND PANO  = '" + Dt.Rows[i]["Pano"].ToString() + "' ";
                    SQL += ComNum.VBLF + "    AND IPDNO = '" + Dt.Rows[i]["IPDNO"].ToString() + "' ";
                    SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtSub.Dispose();
                        DtSub = null;
                        return;
                    }

                    if (DtSub.Rows.Count > 0)
                    {
                        if (string.Compare(DtSub.Rows[0]["ROutDate"].ToString(), DtSub.Rows[0]["ROUTENTDATE"].ToString()) > 0)
                        {
                            if (string.Compare(DtSub.Rows[0]["ROUTENTDATENEW"].ToString(), VB.DateAdd("D", -1, clsPublic.GstrSysDate).ToString("yyyy-MM-dd") + " 17:31") < 0)
                            {
                                //ssIpdList_Sheet1.Cells[i, 18].Text = "예고(" + Dt.Rows[i]["GBCHECKLIST"].ToString() + ")";
                                ssIpdList_Sheet1.Cells[i, 18].Text = "Y";
                                ssIpdList_Sheet1.Cells[i, 18, i, 18].BackColor = Color.Red;
                            }
                        }
                    }

                    DtSub.Dispose();
                    DtSub = null;

                    //재원심사자 표시
                    if (FbJSim_Flag == false)
                       ssIpdList_Sheet1.Cells[i, 19].Text = "";
                    else
                    {
                        ssIpdList_Sheet1.Cells[i, 19].Text = CF.Read_SabunName(pDbCon, Read_Simsa_Sabun(pDbCon, Dt.Rows[i]["pano"].ToString()));

                        if (ssIpdList_Sheet1.Cells[i, 19].Text == "")
                            ssIpdList_Sheet1.Cells[i, 19].Text = CF.Read_SabunName(pDbCon, Dt.Rows[i]["JSIM_SABUN"].ToString());
                    }
                    
                    ssIpdList_Sheet1.Cells[i, 20].Text = Dt.Rows[i]["SunapTime"].ToString();

                    if (Dt.Rows[i]["GbSuDay"].ToString() == "Y")
                    {
                        ssIpdList_Sheet1.Cells[i, 21].Text = "Y";
                        ssIpdList_Sheet1.Cells[i, 21, i, 21].BackColor = Color.Yellow;
                        ssIpdList_Sheet1.Cells[i, 2, i, 2].BackColor = Color.Yellow;
                    }

                    if (Dt.Rows[i]["GbSPC"].ToString() == "1")
                        ssIpdList_Sheet1.Cells[i, 22].Text = "Y";

                    ssIpdList_Sheet1.Cells[i, 23].Text = Dt.Rows[i]["SIMSATIME"].ToString();

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SIMSA_SNAME ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_SIMSA_HIS";
                    SQL += ComNum.VBLF + "  WHERE PANO      = '" + Dt.Rows[i]["PANO"].ToString() + "' ";
                    SQL += ComNum.VBLF + "    AND Trsno     =  " + Convert.ToInt64(Dt.Rows[i]["TRSNO"].ToString()) + " ";
                    SQL += ComNum.VBLF + "    AND GbSTS     = '5' ";
                    SQL += ComNum.VBLF + "  ORDER BY EntDate DESC ";
                    SqlErr = clsDB.GetDataTableEx(ref DtSub, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtSub.Dispose();
                        DtSub = null;
                        return;
                    }

                    if (DtSub.Rows.Count == 1)
                        ssIpdList_Sheet1.Cells[i, 11, i, 11].BackColor = Color.Yellow;
                    else if (DtSub.Rows.Count > 1)
                        ssIpdList_Sheet1.Cells[i, 11, i, 11].BackColor = Color.MistyRose;
                    else
                        ssIpdList_Sheet1.Cells[i, 11, i, 11].BackColor = Color.White;

                    DtSub.Dispose();
                    DtSub = null;

                    if (VB.I(FstrRoomOver, Dt.Rows[i]["RoomCode"].ToString()) >= 2)
                        ssIpdList_Sheet1.Cells[i, 11, i, 11].BackColor = Color.Purple;

                    if ((strGbSTS == "1" || strGbSTS == "2" || strGbSTS == "3" || strGbSTS == "4") && Read_Simsa_Part(pDbCon, Convert.ToInt64(VB.Val(Dt.Rows[i]["Tewon_Sabun"].ToString()))))
                        // ssIpdList_Sheet1.Cells[i, 0, i, ssIpdList_Sheet1.ColumnCount - 1].BackColor = Color.Aqua;
                        ssIpdList_Sheet1.Cells[i, 0, i, ssIpdList_Sheet1.ColumnCount - 1].ForeColor = Color.Blue;
                }
            }

            Dt.Dispose();
            Dt = null;

            Cursor.Current = Cursors.Default;
        }

        bool Read_Simsa_Part(PsmhDb pDbCon, long ArgSabun)
        {
            bool rtnVal = false;
            DataTable DtSim = new DataTable();

            string strSabun = string.Format("{0:D5}", ArgSabun);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Buse FROM  KOSMOS_ADM.INSA_MST ";
            SQL += ComNum.VBLF + "  WHERE Sabun     = '" + strSabun + "'  ";
            SQL += ComNum.VBLF + "    AND Buse      = '078201' ";
            SqlErr = clsDB.GetDataTable(ref DtSim, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtSim.Dispose();
                DtSim = null;
                return rtnVal;
            }

            if (DtSim.Rows.Count > 0)
                rtnVal = true;

            DtSim.Dispose();
            DtSim = null;

            return rtnVal;
        }

        string Read_Simsa_Sabun(PsmhDb pDbCon, string ArgPtno)
        {
            int x = 0;
            string rtnVal = "";
            string strOK = "";
            string strOK2 = "";
            string strOK3 = "";
            string strOK4 = "";
            string[] strBi = new string[4];
            string[] strWard = new string[4];
            string[] strRoom = new string[4];
            string[] strDept = new string[4];
            DataTable DtSim = new DataTable();

            for (int i = 0; i < 12; i++)
            {
                strOK = "OK";

                for (int j = 0; j < 4; j++)
                {
                    strBi[j] = "";
                    strWard[j] = "";
                    strRoom[j] = "";
                    strDept[j] = "";
                }

                if (Convert.ToInt32(FstrJSimSabun[i]) > 0)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT SABUN,GUBUN,REMARK, ";
                    SQL += ComNum.VBLF + "        ENTDATE,Seqno ";
                    SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "ETC_JSIM_SET ";
                    SQL += ComNum.VBLF + "  WHERE Sabun = " + Convert.ToInt32(FstrJSimSabun[i]) + " ";
                    SQL += ComNum.VBLF + "  ORDER By Seqno,Gubun ";
                    SqlErr = clsDB.GetDataTable(ref DtSim, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtSim.Dispose();
                        DtSim = null;
                        return rtnVal;
                    }

                    if (DtSim.Rows.Count > 0)
                    {
                        for (int j = 0; j < DtSim.Rows.Count; j++)
                        {
                            x = Convert.ToInt32(DtSim.Rows[j]["SEQNO"].ToString()) ;
                            
                            switch (DtSim.Rows[j]["GUBUN"].ToString().Trim())
                            {
                                case "1":
                                    strBi[x] = strBi[x] + "'" + DtSim.Rows[j]["REMARK"].ToString().Trim() + "',";
                                    break;

                                case "2":
                                    if (VB.I(strWard[x], VB.Pstr(DtSim.Rows[j]["REMARK"].ToString().Trim(), "/", 1)) < 2)
                                        strWard[x] = strWard[x] + "'" + VB.Pstr(DtSim.Rows[j]["REMARK"].ToString().Trim(), "/", 1) + "',";

                                    strRoom[x] = strRoom[x] + VB.Pstr(DtSim.Rows[j]["REMARK"].ToString().Trim(), "/", 2) + ",";
                                    break;

                                case "3":
                                    strDept[x] = strDept[x] + "'" + DtSim.Rows[j]["REMARK"].ToString().Trim() + "',";
                                    break;
                            }
                        }
                    }
                    else
                        strOK = "";

                    DtSim.Dispose();
                    DtSim = null;

                    if (strOK == "OK")
                    {
                        for (x = 0; x < 4; x++)
                        {
                            if (strBi[x] != "")   { strBi[x]    = VB.Right(strBi[x], 1)   == "," ? strBi[x].Substring(0, strBi[x].Length - 1) : strBi[x]; }
                            if (strWard[x] != "") { strWard[x]  = VB.Right(strWard[x], 1) == "," ? strWard[x].Substring(0, strWard[x].Length - 1) : strWard[x]; }
                            if (strRoom[x] != "") { strRoom[x]  = VB.Right(strRoom[x], 1) == "," ? strRoom[x].Substring(0, strRoom[x].Length - 1) : strRoom[x]; }
                            if (strDept[x] != "") { strDept[x]  = VB.Right(strDept[x], 1) == "," ? strDept[x].Substring(0, strDept[x].Length - 1) : strDept[x]; }
                        }

                        strOK2 = "OK";
                        if (strBi[1] == "" && strWard[1] == "" && strRoom[1] == "" && strDept[1] == "") { strOK2 = ""; }
                        strOK3 = "OK";
                        if (strBi[2] == "" && strWard[2] == "" && strRoom[2] == "" && strDept[2] == "") { strOK3 = ""; }
                        strOK4 = "OK";
                        if (strBi[3] == "" && strWard[3] == "" && strRoom[3] == "" && strDept[3] == "") { strOK4 = ""; }

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT /*+ INDEX_DESC(kosmos_ocs.ipd_trans INDEX_IPDTRS0) */ ";
                        SQL += ComNum.VBLF + "        A.IPDNO, A.TRSNO, A.GBSTS, ";
                        SQL += ComNum.VBLF + "        A.Pano, A.GBIPD, B.SName, ";
                        SQL += ComNum.VBLF + "        B.WARDCODE, B.RoomCode, B.ILLCODE1, ";
                        SQL += ComNum.VBLF + "        B.ILLCODE2 , B.ILLCODE3, B.ILLCODE4 , ";
                        SQL += ComNum.VBLF + "        A.DeptCode, A.DrCode,  A.ILSU, ";
                        SQL += ComNum.VBLF + "        A.VCODE, A.OGPDBUN, A.OGPDBUNDTL,";
                        SQL += ComNum.VBLF + "        A.Bi, ";
                        SQL += ComNum.VBLF + "        TO_CHAR(A.InDate,'YYYY-MM-DD') InDate,  ";
                        SQL += ComNum.VBLF + "        TO_CHAR(A.JSIM_LDATE ,'YYYY-MM-DD') JSIM_LDATE , ";
                        SQL += ComNum.VBLF + "        JSIM_SABUN ,JSIM_SET, B.JSIM_REMARK, ";
                        SQL += ComNum.VBLF + "        a.Ilsu , b.ilsu ilsu2, ";
                        SQL += ComNum.VBLF + "        a.DrgCode, a.GbDrg  ";
                        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS A, ";
                        SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_MASTER B  ";
                        SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                        SQL += ComNum.VBLF + "    AND A.ACTDATE IS NULL ";
                        SQL += ComNum.VBLF + "    AND A.INDATE  < TRUNC(SYSDATE)  ";
                        SQL += ComNum.VBLF + "    AND A.GBIPD  NOT IN ('D') ";
                        SQL += ComNum.VBLF + "    AND ((A.JSIM_SET IS NULL ";

                        if (strBi[0] != "")
                            SQL += ComNum.VBLF + "AND A.BI IN (" + strBi[0] + ") ";
                        if (strDept[0] != "")
                            SQL += ComNum.VBLF + "AND A.DEPTCODE IN (" + strDept[0] + ") ";
                        if (strWard[0] != "")
                            SQL += ComNum.VBLF + "AND B.WARDCODE IN (" + strWard[0] + ") ";
                        if (strRoom[0] != "")
                            SQL += ComNum.VBLF + "AND B.ROOMCODE IN (" + strRoom[0] + ") ";

                        if ( strOK2 != "")
                        {
                            SQL += ComNum.VBLF + "            ) OR ( A.JSIM_SET IS NULL ";

                            if (strBi[1] != "")
                                SQL += ComNum.VBLF + "AND A.BI IN (" + strBi[1] + ") ";
                            if (strDept[1] != "")
                                SQL += ComNum.VBLF + "AND A.DEPTCODE IN (" + strDept[1] + ") ";
                            if (strWard[1] != "")
                                SQL += ComNum.VBLF + "AND B.WARDCODE IN (" + strWard[1] + ") ";
                            if (strRoom[1] != "")
                                SQL += ComNum.VBLF + "AND B.ROOMCODE IN (" + strRoom[1] + ") ";
                        }

                        if ( strOK3 != "")
                        {
                            SQL += ComNum.VBLF + "            ) OR ( A.JSIM_SET IS NULL ";

                            if (strBi[2] != "")
                                SQL += ComNum.VBLF + "AND A.BI IN (" + strBi[2] + ") ";
                            if (strDept[2] != "")
                                SQL += ComNum.VBLF + "AND A.DEPTCODE IN (" + strDept[2] + ") ";
                            if (strWard[2] != "")
                                SQL += ComNum.VBLF + "AND B.WARDCODE IN (" + strWard[2] + ") ";
                            if (strRoom[2] != "")
                                SQL += ComNum.VBLF + "AND B.ROOMCODE IN (" + strRoom[2] + ") ";
                        }


                        if ( strOK4 != "")
                        {
                            SQL += ComNum.VBLF + "            ) OR ( A.JSIM_SET IS NULL ";

                            if (strBi[3] != "")
                                SQL += ComNum.VBLF + "AND A.BI IN (" + strBi[3] + ") ";
                            if (strDept[3] != "")
                                SQL += ComNum.VBLF + "AND A.DEPTCODE IN (" + strDept[3] + ") ";
                            if (strWard[3] != "")
                                SQL += ComNum.VBLF + "AND B.WARDCODE IN (" + strWard[3] + ") ";
                            if (strRoom[3] != "")
                                SQL += ComNum.VBLF + "AND B.ROOMCODE IN (" + strRoom[3] + ") ";
                        }

                        SQL += ComNum.VBLF + "           )  OR A.JSIM_SET = '" + FstrJSimSabun[i] + "' ) ";
                        SQL += ComNum.VBLF + "       AND A.IPDNO    = B.IPDNO ";
                        SQL += ComNum.VBLF + "       AND A.PANO     = '" + ArgPtno + "' ";
                        SQL += ComNum.VBLF + "     ORDER BY B.ROOMCODE ";
                        SqlErr = clsDB.GetDataTable(ref DtSim, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            DtSim.Dispose();
                            DtSim = null;
                            return rtnVal;
                        }

                        if (DtSim.Rows.Count > 0)
                        {
                            rtnVal = FstrJSimSabun[i];
                            DtSim.Dispose();
                            DtSim = null;
                            return rtnVal;
                        }

                        DtSim.Dispose();
                        DtSim = null;
                    }
                }
            }

            return rtnVal;
        }

        void Screen_Clear()
        {
            FnIpdNo = 0;
            FnTrsNo = 0;
            FstrGbSuday = "";

            txtSPtno.Text = "";
            txtSSname.Text = "";

            txtPtno.Text = "";
            txtSname.Text = "";
            txtSexAge.Text = "";
            txtWardRoom.Text = "";
            txtInDate.Text = "";
            txtIlsu.Text = "";
            txtBi.Text = "";
            txtDept.Text = "";
            txtDr.Text = "";
            dtpOutDate.Text = clsPublic.GstrSysDate;
            dtpRdate.Text = clsPublic.GstrSysDate;
            txtRtime.Text = "";
            txtOutDateTime.Text = "";
            cboSTS.SelectedIndex = -1;
            cboBun.SelectedIndex = -1;
            txtSTS.Text = "";
            txtIpdNo.Text = "";
            txtTrsNo.Text = "";
            txtKiho.Text = "";
            txtPaKiho.Text = "";

            btnOk.Enabled = false;
            btnCancel.Enabled = false;
        }
        
    }
}
