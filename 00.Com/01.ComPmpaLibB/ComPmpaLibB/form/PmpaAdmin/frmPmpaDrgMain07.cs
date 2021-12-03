using ComLibB;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System.Diagnostics;

/// <summary>
/// Description : DRG 환자 관리 및 세팅
/// Author : 김민철
/// Create Date : 2017.07.03
/// </summary>
/// <history>
/// </history>
/// <seealso cref="DRGMAIN07.frm, FrmDRGJob "/>
/// 
namespace ComPmpaLibB
{
    public partial class frmPmpaDrgMain07 : Form
    {
        private string FstrPano = "";
        private string FstrBi = "";
        private string FstrActdate = "";
        private string FstrTRSNO = "";
        private string FstrIPDNO = "";
        private string FstrDRGCode = "";
        private string FstrROWID = "";
        private string FstrDeptCode = "";
        private string FstrInDate = "";
        private int FnTimerCNT = 0;

        public frmPmpaDrgMain07()
        {
            InitializeComponent();
            Set_Ctrl();
            SetEvent();
        }

        void SetEvent()
        {
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            txtDrgAmt.Text = "";
            txtDrgBAmt.Text = "";

            //DRG 확인금액
            if (e.Row == 3 || e.Row == 7 || e.Row == 11 || e.Row == 15 || e.Row == 19 || e.Row == 23)
            {
                if (e.Column > 0)
                {
                    //DRG 총금액
                    txtDrgAmt.Text = SS2.ActiveSheet.Cells[e.Row, e.Column].Text;
                    DRG.GnJSimDrgAmt = (long)VB.Val(VB.Replace(SS2.ActiveSheet.Cells[e.Row, e.Column].Text, ",", ""));

                    //DRG 확인금액(본인)
                    txtDrgBAmt.Text = SS2.ActiveSheet.Cells[e.Row - 1, e.Column].Text;
                    DRG.GnJSimDrgBAmt = (long)VB.Val(VB.Replace(SS2.ActiveSheet.Cells[e.Row - 1, e.Column].Text, ",", ""));
                }
            }

            ////DRG 확인금액(본인)
            //if (e.Row == 2 || e.Row == 6 || e.Row == 10 || e.Row == 14 || e.Row == 18 || e.Row == 22)
            //{
            //    if (e.Column > 0)
            //    {
            //        txtDrgBAmt.Text = SS2.ActiveSheet.Cells[e.Row, e.Column].Text;
            //        DRG.GnJSimDrgBAmt = (long)VB.Val(VB.Replace(SS2.ActiveSheet.Cells[e.Row, e.Column].Text, ",", ""));
            //    }
            //}
        }

        private void Set_Ctrl()
        {
            cboDept.Items.Clear();
            cboDept.Items.Add("GS.일반외과");
            cboDept.Items.Add("OG.산부인과");
            cboDept.Items.Add("OT.안과");
            cboDept.Items.Add("EN.이비인후과");

            //알콜및약물중독재활치료코드
            cboACode.Items.Clear();
            cboACode.Items.Add("*****.해당없음");
            cboACode.Items.Add("NCV01.알콜 재활치료");
            cboACode.Items.Add("NCV02.마약 재활치료");
            cboACode.Items.Add("NCV03.알콜 및 마약 재활치료 동시실시");
            cboACode.Items.Add("NCV04.알콜 및 해독치료");
            cboACode.Items.Add("NCV05.마약 재활 및 해독치과");
            cboACode.Items.Add("NCV06.알콜 및 마약 재활 및 해독치료 동시 실시");
            cboACode.SelectedIndex = 0;

            cboSex.Items.Clear();
            cboSex.Items.Add("M");
            cboSex.Items.Add("F");
            
            //본인부담율
            cboGbn.Items.Clear();
            cboGbn.Items.Add("1.본인부담 20%");
            cboGbn.Items.Add("2.본인부담 14%(차상위 만성질환 18세미만)");
            cboGbn.Items.Add("3.본인부담 10%(희귀난치성질환 6세미만포함)");
            cboGbn.Items.Add("4.본인부담  5%(중증질환자)");
            cboGbn.Items.Add("5.본인부담  3%(차장위 만성질활 15세이하)");
            cboGbn.SelectedIndex = 0;

            //야간가산구분
            cboNgt.Items.Clear();
            cboNgt.Items.Add("0.무가산");
            cboNgt.Items.Add("1.야간공휴가산");
            cboNgt.Items.Add("2.심야");
            cboNgt.SelectedIndex = 0;

            //절사구분
            cboTrunc.Items.Clear();
            cboTrunc.Items.Add("1.절사");
            cboTrunc.Items.Add("2.무절사");
            cboTrunc.SelectedIndex = 0;

            //산모가산구분
            cboOgAdd.Items.Clear();
            cboOgAdd.Items.Add("0.무가산");
            cboOgAdd.Items.Add("1.부인과 30%가산");
            cboOgAdd.Items.Add("2.재왕절개 50%가산");
            cboOgAdd.Items.Add("3.재왕절개 취약지 50%가산");
            cboOgAdd.SelectedIndex = 0;

            //진료결과 Setting
            cboJin.Items.Add("0.완쾌");
            cboJin.Items.Add("1.빈사");
            cboJin.Items.Add("2.자의");
            cboJin.Items.Add("3.이송");
            cboJin.Items.Add("4.사망");
            cboJin.Items.Add("5.도주");
            cboJin.Items.Add("6.기타");
            cboJin.SelectedIndex = 0;
            
        }
        
        private void Screen_Clear()
        {
            //일반사항 -----------------
            txtPano.Text = "";
            txtSName.Text = "";
            txtYYMM.Text = "";
            cboDept.Text = "";
            cboSex.Text = "";
            txtAge.Text = "";
            txtDrgCodeOP.Text = "";
            txtDrgNameOP.Text = "";
            txtDAmtOP.Text = "";
            txtDrName.Text = "";
            //drg번호생성
            txtJumin.Text = "";
            txtJinDate.Text = "";
            txtJinIlsu.Text = "";

            txtILLCode1.Text = "";
            txtILLCode2.Text = "";
            txtILLCode3.Text = "";
            txtILLCode4.Text = "";
            txtILLCode5.Text = "";
            txtILLCode6.Text = "";
            txtILLCode7.Text = "";
            txtILLCode8.Text = "";
            txtILLCode9.Text = "";
            txtILLCode10.Text = "";
            
            cboJin.Text = "";
            
            txtOpCode1.Text = "";
            txtOpCode2.Text = "";
            txtOpCode3.Text = "";
            txtOpCode4.Text = "";
            txtOpCode5.Text = "";
            txtOpCode6.Text = "";
            txtOpCode7.Text = "";
            txtOpCode8.Text = "";
            txtOpCode9.Text = "";
            txtOpCode10.Text = "";

            txtExCode1.Text = "";
            txtExCode2.Text = "";
            txtExCode3.Text = "";
            txtExCode4.Text = "";
            txtExCode5.Text = "";

            txtXCode1.Text = "";
            txtXCode2.Text = "";
            txtXCode3.Text = "";
            txtXCode4.Text = "";
            txtXCode5.Text = "";

            txtJCode1.Text = "";
            txtJCode2.Text = "";
            txtJCode3.Text = "";
            txtJCode4.Text = "";
            txtJCode5.Text = "";

            txtMCode1.Text = "";
            txtMCode2.Text = "";
            txtMCode3.Text = "";
            txtMCode4.Text = "";
            txtMCode5.Text = "";

            txtBCode1.Text = "";
            txtBCode2.Text = "";
            txtBCode3.Text = "";
            txtBCode4.Text = "";
            txtBCode5.Text = "";

            txtWeight.Text = "";
            txtTAmt.Text = "";
            txtUpBamt.Text = "";

            cboACode.SelectedIndex = 0;

            txtCPR.Text = "";
            
            //실행 결과 --------------------------------------------
            txtMDC.Text = "";
            txtADrg.Text = "";
            txtPCCL.Text = "";
            txtDCode.Text = "";
            
            txtDAmt1.Text = "";
            txtDAmt2.Text = "";
            txtDAmt3.Text = "";
            
            txtDrgName.Text = "";
            
            txtAmt1.Text = "";
            txtAmt2.Text = "";
            txtAmt3.Text = "";
            txtAmt4.Text = "";
            txtAmt5.Text = "";
            txtAmt6.Text = "";
            txtAmt7.Text = "";
            txtAmt8.Text = "";
            txtAmt9.Text = "";
            txtAmt10.Text = "";
            txtDrgAmt.Text = "";
            txtDrgBAmt.Text = "";

            //panRight.Enabled = false;

            btnSaveRun.Enabled = false;
            btnUpdate.Enabled = false;
            btnViewForm.Enabled = false;
            txtDCode.Enabled = false;
            txtDrgName.Enabled = false;
        }

        private void frmPmpaDrgMain07_Load(object sender, EventArgs e)
        {
            string strHelpCode = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Screen_Clear();

            string _strFile = @"C:\kdrg33\kdrg33.exe";       //파일경로
            FileInfo _finfo = new FileInfo(_strFile);
            if (_finfo.Exists == false)
            {
                btnSaveRun.Enabled = false;
                btnUpdate.Enabled = false;
            }

            strHelpCode = clsPublic.GstrHelpCode;

            if (strHelpCode != "")
            {
                FstrPano = clsPublic.GstrPANO;
                FstrBi = VB.Pstr(strHelpCode, "@@", 1);
                FstrActdate = VB.Pstr(strHelpCode, "@@", 2);
                FstrTRSNO = VB.Pstr(strHelpCode, "@@", 3);
                FstrIPDNO = VB.Pstr(strHelpCode, "@@", 4);
                FstrDRGCode = VB.Pstr(strHelpCode, "@@", 5);


                FstrROWID = "";

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT ROWID ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "DRG_MASTER_NEW  ";
                    SQL += ComNum.VBLF + "  WHERE IPDNO = '" + FstrIPDNO + "' ";
                    SQL += ComNum.VBLF + "    AND  TRSNO = '" + FstrTRSNO + "' ";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (Dt.Rows.Count > 0)
                    {
                        FstrROWID = Dt.Rows[0]["ROWID"].ToString().Trim();
                    }

                    Dt.Dispose();
                    Dt = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                }
                
                if (FstrROWID != "")
                {
                    View_Drg_Master();
                }
                else
                {
                    Read_Ipd_Trans();
                }

                Drg_Suga_View();

                //panRight.Enabled = true;

                btnSaveRun.Enabled = true;
                btnUpdate.Enabled = true;
                btnViewForm.Enabled = true;
                txtDCode.Enabled = true;
                txtDrgName.Enabled = true;
            }

        }

        private void Drg_Suga_View()
        {
            string strSdate, strDDate = "";

            int i, j, nRow = 0, nCol, nRowB = 0, nREAD = 0;

            DataTable Dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //FstrDRGCode = "C05100";

            if (FstrDRGCode == "") { return; }

            strSdate = txtJinDate.Text.Trim();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT DCODE,DDATE,GBBON, ";
                SQL += ComNum.VBLF + "        DAY1,DAY2,DAY3,DAY4,DAY5,DAY6,DAY7,DAY8,DAY9,DAY10,";
                SQL += ComNum.VBLF + "        DAY11,DAY12,DAY13,DAY14,DAY15,DAY16,DAY17,DAY18,DAY19,DAY20,";
                SQL += ComNum.VBLF + "        DAY21,DAY22,DAY23,DAY24,DAY25,DAY26,DAY27,DAY28,DAY29,DAY30 ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "DRG_DAY_COST_NEW ";
                SQL += ComNum.VBLF + "  WHERE DCODE = '" + FstrDRGCode + "'";
                SQL += ComNum.VBLF + "    AND DDATE <= TO_DATE('" + strSdate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND GBN = '" + VB.Left(cboGbn.Text, 1) + "' ";
                SQL += ComNum.VBLF + "    AND GBNGT = '" + VB.Left(cboNgt.Text, 1) + "' ";
                SQL += ComNum.VBLF + "    AND GBNTRUNC = '" + VB.Left(cboTrunc.Text, 1) + "' ";
                SQL += ComNum.VBLF + "    AND GBOGADD = '" + VB.Left(cboOgAdd.Text, 1) + "' ";
                SQL += ComNum.VBLF + "  ORDER BY DDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                if (nREAD == 0)
                {
                    ComFunc.MsgBox("해당하는 DATA가 없습니다.");
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                for (i = 0; i < nREAD; i++)
                {
                    if (strDDate == "")
                    {
                        strDDate = Dt.Rows[i]["DDATE"].ToString().Trim();
                    }

                    if (strDDate != "" && strDDate != Dt.Rows[i]["DDATE"].ToString().Trim())
                    {
                        break;
                    }

                    nCol = 1;
                    switch (Dt.Rows[i]["GbBon"].ToString().Trim())
                    {
                        case "1":
                            nRowB = 2; //총액
                            break;
                        case "2":
                            nRowB = 0; //보험자
                            break;
                        case "3":
                            nRowB = 1; //본인
                            break;
                    }

                    for (j = 1; j <= 30; j++)
                    {
                        if (j >= 1 && j <= 5) { nRow = 1; }
                        else if (j >= 6  && j <= 10) { nRow = 5; }
                        else if (j >= 11 && j <= 15) { nRow = 9; }
                        else if (j >= 16 && j <= 20) { nRow = 13; }
                        else if (j >= 21 && j <= 25) { nRow = 17; }
                        else if (j >= 26 && j <= 30) { nRow = 21; }

                        if (j % 5 == 0)
                        {
                            nCol = 5;
                        }
                        else
                        {
                            nCol = (j % 5);
                        }

                        SS2_Sheet1.Cells[nRow + nRowB, nCol].Text = VB.Format(VB.Val(Dt.Rows[i]["Day" + j.ToString()].ToString()), "#,###.###0");
                    }
                    
                }

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void View_Drg_Master()
        {
            string strGbOGADD = "";
            int nREAD = 0;

            DataTable Dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            if (FstrPano == "") { return; }

            strGbOGADD = "0";

            Master_Read_Rtn();
            
            Read_Drg_Master_New();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT DDATE, DCODE, DNAME,  DJUMSUM, DGOBI, DILSU_AV, DILSU_MIN, DILSU_MAX, DJUMDANGA, DHJUMSUM ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.VBLF + "DRG_CODE_NEW ";
                SQL += ComNum.VBLF + " WHERE DCODE = '" + txtDrgCodeOP.Text + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                if (nREAD > 0)
                {
                    txtDrgNameOP.Text = Dt.Rows[0]["DNAME"].ToString().Trim();
                    Read_Drg_Cost_New(txtDrgCodeOP.Text, txtJinDate.Text, txtJinIlsu.Text, strGbOGADD, "1");
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void Master_Read_Rtn()
        {
            string strJumin1 = "";
            string strJumin2 = "";
            string strMsg = "";
            //string strInDate = "";
            string strOutDate = "";
            //string strDeptCode = "";
            DateTime t1;
            DateTime t2;
            TimeSpan r1;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Jumin1, Jumin2 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "  WHERE Pano = '" + FstrPano + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    strJumin1 = Dt.Rows[0]["Jumin1"].ToString().Trim();
                    strJumin2 = Dt.Rows[0]["Jumin2"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.Pano,  B.Sname,  B.Age,   B.Sex,   A.Deptcode, A.DrCode, a.drgcode, ";
                SQL += ComNum.VBLF + "        A.BI, TO_CHAR(A.InDate,'YYYY-MM-DD') InDate, TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE, ";
                SQL += ComNum.VBLF + "        A.IllCode1, A.IllCode2, A.IllCode3, A.IllCode4, A.IllCode5, A.ILLCODE6,  ";
                SQL += ComNum.VBLF + "        A.DRGADC1 , A.DRGADC2, A.DRGADC3,  A.DRGADC4,  A.DRGADC5, ";
                SQL += ComNum.VBLF + "        A.Ilsu,  B.Amset7, A.Amt49, A.Amt50 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS A, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_MASTER B ";
                SQL += ComNum.VBLF + "  WHERE A.IPDNO = '" + FstrIPDNO + "' ";
                SQL += ComNum.VBLF + "    AND A.TRSNO = '" + FstrTRSNO + "' ";
                SQL += ComNum.VBLF + "    AND A.IPDNO = B.IPDNO ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    Screen_Clear();
                    strMsg = "퇴원 MASTER에 자료가 없거나 DRG Check가 않되었거나, 진료과가 맞지 않습니다 !!" + ComNum.VBLF;
                    strMsg += " ☞   원무과 확인요망!!! ";
                    ComFunc.MsgBox(strMsg);
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                FstrInDate = Dt.Rows[0]["InDate"].ToString().Trim();
                strOutDate = Dt.Rows[0]["OUTDATE"].ToString().Trim();
                FstrDeptCode = Dt.Rows[0]["DeptCode"].ToString().Trim();

                txtPano.Text = FstrPano;
                txtSName.Text = Dt.Rows[0]["Sname"].ToString().Trim();
                txtJinDate.Text = Dt.Rows[0]["InDate"].ToString().Trim();

                if (Dt.Rows[0]["OUTDATE"].ToString().Trim() != "")
                {
                    t1 = DateTime.Parse(strOutDate);
                    t2 = DateTime.Parse(FstrInDate);
                    r1 = t1 - t2;
                    txtJinIlsu.Text = (Convert.ToInt32(r1.Days) + 1).ToString();
                }
                else
                {
                    t1 = DateTime.Parse(clsPublic.GstrSysDate);
                    t2 = DateTime.Parse(FstrInDate);
                    r1 = t1 - t2;
                    txtJinIlsu.Text = (Convert.ToInt32(r1.Days) + 1).ToString();
                }

                txtDrgCodeOP.Text = Dt.Rows[0]["drgcode"].ToString().Trim();

                cboSex.Text = Dt.Rows[0]["sex"].ToString().Trim();
                txtAge.Text = Dt.Rows[0]["age"].ToString().Trim();

                //진료과
                switch (Dt.Rows[0]["DeptCode"].ToString().Trim())
                {
                    case "GS":
                        cboDept.SelectedIndex = 0;
                        break;
                    case "OG":
                        cboDept.SelectedIndex = 1;
                        break;
                    case "OT":
                        cboDept.SelectedIndex = 2;
                        break;
                    case "EN":
                        cboDept.SelectedIndex = 3;
                        break;
                }

                txtJumin.Text = strJumin1 + strJumin2;

                txtILLCode1.Text = Dt.Rows[0]["IllCode1"].ToString().Trim();
                txtILLCode2.Text = Dt.Rows[0]["IllCode2"].ToString().Trim();
                txtILLCode3.Text = Dt.Rows[0]["IllCode3"].ToString().Trim();
                txtILLCode4.Text = Dt.Rows[0]["IllCode4"].ToString().Trim();
                txtILLCode5.Text = Dt.Rows[0]["IllCode5"].ToString().Trim();
                txtILLCode6.Text = Dt.Rows[0]["IllCode6"].ToString().Trim();

                txtBCode1.Text = Dt.Rows[0]["DRGADC1"].ToString().Trim();
                txtBCode2.Text = Dt.Rows[0]["DRGADC2"].ToString().Trim();
                txtBCode3.Text = Dt.Rows[0]["DRGADC3"].ToString().Trim();
                txtBCode4.Text = Dt.Rows[0]["DRGADC4"].ToString().Trim();
                txtBCode5.Text = Dt.Rows[0]["DRGADC5"].ToString().Trim();

                txtTAmt.Text = VB.Format(VB.Val(Dt.Rows[0]["Amt50"].ToString().Trim()), "#,##0 ");

                Dt.Dispose();
                Dt = null;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void Read_Ipd_Trans()
        {
            string strGbOGADD = "";
            int nREAD = 0;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            
            if (FstrPano == "") { return; }

            strGbOGADD = "0";

            Master_Read_Rtn();
            Ills_Get_Rtn(FstrDeptCode, FstrInDate);
            OpCode_Get_Rtn(ref strGbOGADD);
            LabCode_Get_Rtn();
            
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT DDATE, DCODE, DNAME,  DJUMSUM, DGOBI, DILSU_AV, DILSU_MIN, DILSU_MAX, DJUMDANGA, DHJUMSUM ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.VBLF + "DRG_CODE_NEW ";
                SQL += ComNum.VBLF + " WHERE DCODE = '" + txtDrgCodeOP.Text + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                if (nREAD > 0)
                {
                    txtDrgNameOP.Text = Dt.Rows[0]["DNAME"].ToString().Trim();
                    Read_Drg_Cost_New(txtDrgCodeOP.Text, txtJinDate.Text, txtJinIlsu.Text, strGbOGADD, "1");
                }

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void Ills_Get_Rtn(string strDeptCode, string strInDate)
        {
            string[] strILLCode = new string[10];
            int i = 0;
            int nREAD = 0;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            
            for (i=0; i<10; i++)
            {
                strILLCode[i] = "";
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT IllCode ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_IILLS ";
                SQL += ComNum.VBLF + "  WHERE PTno = '" + FstrPano + "' ";
                SQL += ComNum.VBLF + "    AND DeptCode='" + strDeptCode + "' ";
                SQL += ComNum.VBLF + "    AND BDate >=TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND RemoveDate IS NULL ";
                SQL += ComNum.VBLF + "  ORDER BY SEQNO ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    if (i > 9) { break; }
                    strILLCode[i] = Dt.Rows[i]["IllCode"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                txtILLCode1.Text = strILLCode[0];
                txtILLCode2.Text = strILLCode[1];
                txtILLCode3.Text = strILLCode[2];
                txtILLCode4.Text = strILLCode[3];
                txtILLCode5.Text = strILLCode[4];
                txtILLCode6.Text = strILLCode[5];
                txtILLCode7.Text = strILLCode[6];
                txtILLCode8.Text = strILLCode[7];
                txtILLCode9.Text = strILLCode[8];
                txtILLCode10.Text = strILLCode[9];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void OpCode_Get_Rtn(ref string strGbOgAdd)
        {
            string[] strSusul = new string[10];
            int i = 0;
            int nREAD = 0;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            
            for (i = 0; i < 10; i++)
            {
                strSusul[i] = "";
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " Select N.BCode,Sum(Amt1+Amt2) Tamt, n.drgogadd  ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "Ipd_NEW_Slip s,";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "Bas_Sun n ";
                SQL += ComNum.VBLF + "  WHERE s.TRSNO = '" + FstrTRSNO + "'";
                SQL += ComNum.VBLF + "    AND s.Pano = '" + FstrPano + "' ";
                SQL += ComNum.VBLF + "    AND s.BI = '" + FstrBi + "' ";
                SQL += ComNum.VBLF + "    AND s.bun    in ('34','35')         ";
                SQL += ComNum.VBLF + "    AND (s.Gbhost in ('0','1') or Gbhost is null) ";
                SQL += ComNum.VBLF + "    AND s.Sunext = n.Sunext  ";
                SQL += ComNum.VBLF + "    AND n.bcode <>'999999'   ";
                SQL += ComNum.VBLF + "  GROUP BY  N.Bcode , n.drgogadd      ";
                SQL += ComNum.VBLF + " HAVING Sum(Amt1 + Amt2) > 0 ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    if (i > 9) { break; }
                    strSusul[i] = VB.Left(Dt.Rows[i]["BCode"].ToString().Trim(), 5);
                    if (Dt.Rows[i]["BCode"].ToString().Trim()=="Y") { strGbOgAdd = "1"; }
                }

                Dt.Dispose();
                Dt = null;

                txtOpCode1.Text = strSusul[0];
                txtOpCode2.Text = strSusul[1];
                txtOpCode3.Text = strSusul[2];
                txtOpCode4.Text = strSusul[3];
                txtOpCode5.Text = strSusul[4];
                txtOpCode6.Text = strSusul[5];
                txtOpCode7.Text = strSusul[6];
                txtOpCode8.Text = strSusul[7];
                txtOpCode9.Text = strSusul[8];
                txtOpCode10.Text = strSusul[9];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void LabCode_Get_Rtn()
        {
            string[] strGumSa = new string[5];
            int i = 0;
            int nREAD = 0;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            for (i = 0; i < 5; i++)
            {
                strGumSa[i] = "";
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " Select T.BAmt, N.BCode, Sum(Amt1+Amt2) Tamt  ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "Ipd_NEW_Slip s, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "Bas_Sun n,";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "Bas_Sut t ";
                SQL += ComNum.VBLF + " WHERE s.TRSNO = '" + FstrTRSNO + "' ";
                SQL += ComNum.VBLF + "   AND s.Pano = '" + FstrPano + "' ";
                SQL += ComNum.VBLF + "   AND s.BI = '" + FstrBi + "' ";
                SQL += ComNum.VBLF + "   AND s.Bun  = '48'       ";
                SQL += ComNum.VBLF + "   AND s.Gbhost in ('0','1') ";
                SQL += ComNum.VBLF + "   AND s.Sunext = n.Sunext  ";
                SQL += ComNum.VBLF + "   AND s.SuCode = T.Sucode  ";
                SQL += ComNum.VBLF + " GROUP BY T.Bamt, N.Bcode      ";
                SQL += ComNum.VBLF + "HAVING Sum(Amt1 + Amt2) > 0 ";
                SQL += ComNum.VBLF + " ORDER BY T.BAmt Desc  ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    if (i > 5) { break; }
                    strGumSa[i] = Dt.Rows[i]["BCode"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                txtExCode1.Text = strGumSa[0];
                txtExCode2.Text = strGumSa[1];
                txtExCode3.Text = strGumSa[2];
                txtExCode4.Text = strGumSa[3];
                txtExCode5.Text = strGumSa[4];
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void Read_Drg_Master_New()
        {
            int nREAD = 0;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TRSNO,IPDNO,pano,sName,jindate ,Ilsu, ";
                SQL += ComNum.VBLF + "        ILLCODE1,ILLCODE2,ILLCODE3,ILLCODE4,ILLCODE5,ILLCODE6,ILLCODE7,ILLCODE8,ILLCODE9,ILLCODE10,";
                SQL += ComNum.VBLF + "        OPCODE1,OPCODE2,OPCODE3,OPCODE4,OPCODE5,OPCODE6,OPCODE7,OPCODE8,OPCODE9,OPCODE10,";
                SQL += ComNum.VBLF + "        EXCODE1,EXCODE2,EXCODE3,EXCODE4,EXCODE5,";
                SQL += ComNum.VBLF + "        XRCODE1,XRCODE2,XRCODE3,XRCODE4,XRCODE5,";
                SQL += ComNum.VBLF + "        JCODE1,JCODE2,JCODE3,JCODE4,JCODE5,";
                SQL += ComNum.VBLF + "        MCODE1,MCODE2,MCODE3,MCODE4,MCODE5,";
                SQL += ComNum.VBLF + "        BCODE1,BCODE2,BCODE3,BCODE4,BCODE5,";
                SQL += ComNum.VBLF + "        ACODE,Weight,CPR,MDCCODE,DRGCODE,ENTDATE1,ENTDATE2,ENTSABUN1,ENTSABUN2 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "DRG_MASTER_NEW ";
                SQL += ComNum.VBLF + "  WHERE IPDNO = '" + FstrIPDNO + "' ";
                SQL += ComNum.VBLF + "    AND TRSNO = '" + FstrTRSNO + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                if (nREAD == 0)
                {
                    ComFunc.MsgBox("해당하는 DATA가 없습니다.");
                    Dt.Dispose();
                    Dt = null;
                    return;
                }
                
                txtJinDate.Text = Dt.Rows[0]["JINDATE"].ToString().Trim();

                txtILLCode1.Text = Dt.Rows[0]["ILLCODE1"].ToString().Trim();
                txtILLCode2.Text = Dt.Rows[0]["ILLCODE2"].ToString().Trim();
                txtILLCode3.Text = Dt.Rows[0]["ILLCODE3"].ToString().Trim();
                txtILLCode4.Text = Dt.Rows[0]["ILLCODE4"].ToString().Trim();
                txtILLCode5.Text = Dt.Rows[0]["ILLCODE5"].ToString().Trim();
                txtILLCode6.Text = Dt.Rows[0]["ILLCODE6"].ToString().Trim();
                txtILLCode7.Text = Dt.Rows[0]["ILLCODE7"].ToString().Trim();
                txtILLCode8.Text = Dt.Rows[0]["ILLCODE8"].ToString().Trim();
                txtILLCode9.Text = Dt.Rows[0]["ILLCODE9"].ToString().Trim();
                txtILLCode10.Text = Dt.Rows[0]["ILLCODE10"].ToString().Trim();

                txtOpCode1.Text = Dt.Rows[0]["OPCODE1"].ToString().Trim();
                txtOpCode2.Text = Dt.Rows[0]["OPCODE2"].ToString().Trim();
                txtOpCode3.Text = Dt.Rows[0]["OPCODE3"].ToString().Trim();
                txtOpCode4.Text = Dt.Rows[0]["OPCODE4"].ToString().Trim();
                txtOpCode5.Text = Dt.Rows[0]["OPCODE5"].ToString().Trim();
                txtOpCode6.Text = Dt.Rows[0]["OPCODE6"].ToString().Trim();
                txtOpCode7.Text = Dt.Rows[0]["OPCODE7"].ToString().Trim();
                txtOpCode8.Text = Dt.Rows[0]["OPCODE8"].ToString().Trim();
                txtOpCode9.Text = Dt.Rows[0]["OPCODE9"].ToString().Trim();
                txtOpCode10.Text = Dt.Rows[0]["OPCODE10"].ToString().Trim();

                txtExCode1.Text = Dt.Rows[0]["EXCODE1"].ToString().Trim();
                txtExCode2.Text = Dt.Rows[0]["EXCODE2"].ToString().Trim();
                txtExCode3.Text = Dt.Rows[0]["EXCODE3"].ToString().Trim();
                txtExCode4.Text = Dt.Rows[0]["EXCODE4"].ToString().Trim();
                txtExCode5.Text = Dt.Rows[0]["EXCODE5"].ToString().Trim();

                txtXCode1.Text = Dt.Rows[0]["XRCODE1"].ToString().Trim();
                txtXCode2.Text = Dt.Rows[0]["XRCODE2"].ToString().Trim();
                txtXCode3.Text = Dt.Rows[0]["XRCODE3"].ToString().Trim();
                txtXCode4.Text = Dt.Rows[0]["XRCODE4"].ToString().Trim();
                txtXCode5.Text = Dt.Rows[0]["XRCODE5"].ToString().Trim();

                txtJCode1.Text = Dt.Rows[0]["JCODE1"].ToString().Trim();
                txtJCode2.Text = Dt.Rows[0]["JCODE2"].ToString().Trim();
                txtJCode3.Text = Dt.Rows[0]["JCODE3"].ToString().Trim();
                txtJCode4.Text = Dt.Rows[0]["JCODE4"].ToString().Trim();
                txtJCode5.Text = Dt.Rows[0]["JCODE5"].ToString().Trim();

                cboACode.Text = Dt.Rows[0]["ACODE"].ToString().Trim();
                txtWeight.Text = Dt.Rows[0]["Weight"].ToString().Trim();
                txtCPR.Text = Dt.Rows[0]["CPR"].ToString().Trim();

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);

            }

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Drg_Suga_View();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Dim nIlsu          As Integer
            int nREAD = 0;
            string strPath = "";
            string strData = "";
            string strMDC = "";
            string strADRG = "";
            string strDrgCode = "";
            string strPCCL = "";
            string strVER = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (timer1.Enabled == false) { return; }

            FnTimerCNT += 1;

            if (FnTimerCNT < 4) { return; }
            timer1.Enabled = false;

            strPath = @"c:\KDRG33\kdrg33.out";

            strData = File.ReadAllText(strPath);

            strMDC = VB.Trim(VB.Mid(strData + VB.Space(308), 284, 3));
            strADRG = VB.Trim(VB.Mid(strData + VB.Space(308), 287, 4));
            strPCCL = VB.Trim(VB.Mid(strData + VB.Space(308), 291, 1));
            strDrgCode = VB.Trim(VB.Mid(strData + VB.Space(308), 292, 6));
            strVER = VB.Trim(VB.Mid(strData + VB.Space(308), 298, 8));
            
            if (strDrgCode == "" || strDrgCode == "000000")
            {
                txtDCode.Text = "";
                ComFunc.MsgBox("정상적으로 DRG번호를 부여하지 못하였습니다.");
                return; 
            }

            txtMDC.Text = strMDC;
            txtADrg.Text = strADRG;
            txtPCCL.Text = strPCCL;
            txtDCode.Text = strDrgCode;

            btnUpdate.Enabled = false;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT DDATE, DCODE, DNAME,  DJUMSUM, DGOBI, DILSU_AV, DILSU_MIN, DILSU_MAX, DJUMDANGA, DHJUMSUM ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DRG_CODE_NEW ";
                SQL += ComNum.VBLF + " WHERE DCODE = '" + txtDCode.Text + "' ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                if (nREAD == 0)
                {
                    ComFunc.MsgBox("DRG 해당 환자가 아닙니다.");
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                txtDrgName.Text = Dt.Rows[0]["DNAME"].ToString().Trim();

                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);

            }

            btnUpdate.Enabled = true;

            //금액읽기
            Read_Drg_Cost_New(strDrgCode, txtJinDate.Text, txtJinIlsu.Text, "0", "2");

            FstrDRGCode = txtDCode.Text.Trim();
            
            Drg_Suga_View();
        }

        private void Read_Drg_Cost_New(string strDrgCode, string strInDate, string strNal, string strOgAdd, string strGbn)
        {
            if (VB.Val(strNal) > 30) { return; }

            string strCol = "";
            int nREAD = 0;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (strNal == "0") { strNal = "1"; }
            strCol = "DAY" + strNal;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT DDATE, DCODE,";
                SQL += ComNum.VBLF + "     SUM(decode(gbbon, '1',  " + strCol + ")) AMT1, ";
                SQL += ComNum.VBLF + "     SUM(decode(gbbon, '2',  " + strCol + ")) AMT3, ";
                SQL += ComNum.VBLF + "     SUM(decode(gbbon, '3',  " + strCol + ")) AMT2  ";
                SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "DRG_DAY_COST_NEW ";
                SQL += ComNum.VBLF + "  WHERE DCODE = '" + strDrgCode + "'";
                SQL += ComNum.VBLF + "    AND DDATE <= TO_DATE('" + strInDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND GBN = '1' ";      //본인부담 20 % 금액
                SQL += ComNum.VBLF + "    AND GBNGT = '0' ";
                SQL += ComNum.VBLF + "    AND GBNTRUNC = '1' ";
                SQL += ComNum.VBLF + "    AND GBOGADD  = '" + strOgAdd + "' ";
                SQL += ComNum.VBLF + "  GROUP BY DDATE, DCODE ";
                SQL += ComNum.VBLF + "  ORDER BY DDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                if (nREAD > 0)
                {
                    if (strGbn == "1")
                    {
                        txtDAmtOP.Text = VB.Format(VB.Val(Dt.Rows[0]["AMT1"].ToString()), "#,###,##0");
                    }
                    else if(strGbn == "2")
                    {
                        txtDAmt1.Text = VB.Format(VB.Val(Dt.Rows[0]["AMT1"].ToString()), "#,###,##0");
                        txtDAmt2.Text = VB.Format(VB.Val(Dt.Rows[0]["AMT1"].ToString()), "#,###,##0");
                        txtDAmt3.Text = VB.Format(VB.Val(Dt.Rows[0]["AMT1"].ToString()), "#,###,##0");
                    }
                }

                Dt.Dispose();
                Dt = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);

            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private bool CmdRun_Data_Check()
        {
            if (txtJinDate.Text == "")
            {
                ComFunc.MsgBox("입원일자가 공란입니다.");
                return false;
            }

          //  if (VB.Left(cboDept.Text, 2) != "OG" && VB.Left(cboDept.Text, 2) != "OT" && VB.Left(cboDept.Text, 2) != "GS" && VB.Left(cboDept.Text, 2) != "EN")
          //  {
          //      return false;
          //  }

            if (txtILLCode1.Text.Trim() == "")
            {
                ComFunc.MsgBox("주진단명이 공란입니다.");
                return false;
            }

            if (txtOpCode1.Text.Trim() == "")
            {
                ComFunc.MsgBox("수술코드(1)이 공란입니다.");
                return false;
            }

            return true;
        }

        private void btnSaveRun_Click(object sender, EventArgs e)
        {
            txtMDC.Text = "";
            txtADrg.Text = "";
            txtPCCL.Text = "";
            txtDCode.Text = "";

            string _strFile = @"C:\kdrg33\kdrg33.exe";       //파일경로
            FileInfo _finfo = new FileInfo(_strFile);
            if (_finfo.Exists == false)
            {
                FnTimerCNT = 0;
                timer1.Enabled = true;
                return;
            }
            
            if (CmdRun_Data_Check() == false)
            {
                return;
            }
            
            CmdRun_Temp_Write();  //Temp 파일에 저장

            string strPath = @"C:\KDRG33\kdrg33.exe";

            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "kdrg33.exe";
            startInfo.WorkingDirectory = @"C:\KDRG33";
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            startInfo.ErrorDialog = true;
            p = Process.Start(startInfo);

            //VB.Shell(strPath);

            FnTimerCNT = 0;

            
            timer1.Enabled = true;
            timer1.Start();

        }

        private void CmdRun_Temp_Write()
        {
            
            string strPath = @"c:\kdrg33\kdrg33.in";

            StreamWriter sw = new StreamWriter(strPath);

            sw.Write("37100068");   //요양기관기호
            sw.Write(txtJumin.Text.Replace("*", "0"));   //주민번호1
            sw.Write(txtJinDate.Text.Replace("-", ""));   //요양개시일
            sw.Write(VB.Format(VB.Val(txtJinIlsu.Text), "00#"));   //입원일수
            
            switch (VB.Left(cboJin.Text, 1))     //진료결과
            {
                case "3":
                    sw.Write("2");  //이송
                    break;
                case "4":
                    sw.Write("4");  //사망
                    break;
                default:
                    sw.Write("5");  //기타
                    break;
            }

            //진단코드 1-10
            sw.Write(VB.Left(txtILLCode1.Text + VB.Space(6), 6));
            sw.Write(VB.Left(txtILLCode2.Text + VB.Space(6), 6));
            sw.Write(VB.Left(txtILLCode3.Text + VB.Space(6), 6));
            sw.Write(VB.Left(txtILLCode4.Text + VB.Space(6), 6));
            sw.Write(VB.Left(txtILLCode5.Text + VB.Space(6), 6));
            sw.Write(VB.Left(txtILLCode6.Text + VB.Space(6), 6));
            sw.Write(VB.Left(txtILLCode7.Text + VB.Space(6), 6));
            sw.Write(VB.Left(txtILLCode8.Text + VB.Space(6), 6));
            sw.Write(VB.Left(txtILLCode9.Text + VB.Space(6), 6));
            sw.Write(VB.Left(txtILLCode10.Text + VB.Space(6), 6));

            //수술코드 1-10
            sw.Write(VB.Left(txtOpCode1.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtOpCode2.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtOpCode3.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtOpCode4.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtOpCode5.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtOpCode5.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtOpCode7.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtOpCode8.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtOpCode9.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtOpCode10.Text + VB.Space(5), 5));

            //검사코드 1-5
            sw.Write(VB.Left(txtExCode1.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtExCode2.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtExCode3.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtExCode4.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtExCode5.Text + VB.Space(5), 5));
    
            //방사선코드 1-5
            sw.Write(VB.Left(txtXCode1.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtXCode2.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtXCode3.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtXCode4.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtXCode5.Text + VB.Space(5), 5));

            //주사, 혈액제제 1-5
            sw.Write(VB.Left(txtJCode1.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtJCode2.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtJCode3.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtJCode4.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtJCode5.Text + VB.Space(5), 5));

            //마취, 호흡치료
            sw.Write(VB.Left(txtMCode1.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtMCode2.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtMCode3.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtMCode4.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtMCode5.Text + VB.Space(5), 5));
    
            //알콜및약물중독재활치료코드
            if (VB.Left(cboACode.Text, 5) != "*****")
            {
                sw.Write(VB.Left(cboACode.Text + VB.Space(5), 5));
            }
            else
            {
                sw.Write(VB.Space(5));
            }

            //부가코드 1-5
            sw.Write(VB.Left(txtBCode1.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtBCode2.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtBCode3.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtBCode4.Text + VB.Space(5), 5));
            sw.Write(VB.Left(txtBCode5.Text + VB.Space(5), 5));

            //입원시체중(g)
            sw.Write(VB.Left(txtWeight.Text + VB.Space(5), 5));

            //인공호흡시간(hour)
            sw.Write(VB.Left(txtCPR.Text + VB.Space(5), 5));

            sw.Close();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            
             
            DRG clsDRG = new DRG();

            if (txtDrgName.Text == "")
            {
                ComFunc.MsgBox("DRG 해당환자가 아닙니다. 또는 실행버턴을 클랙해서 DRG번호를 부여후에 작업 하세요");
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER SET ";
                SQL += ComNum.VBLF + " DRGCODE = '" + txtDCode.Text + "' ";
                SQL += ComNum.VBLF + " WHERE IPDNO = '" + FstrIPDNO + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";
                SQL += ComNum.VBLF + "       DRGCODE = '" + txtDCode.Text + "', ";
                SQL += ComNum.VBLF + "       DRGADC1 = '" + txtBCode1.Text + "', ";
                SQL += ComNum.VBLF + "       DRGADC2 = '" + txtBCode2.Text + "', ";
                SQL += ComNum.VBLF + "       DRGADC3 = '" + txtBCode3.Text + "', ";
                SQL += ComNum.VBLF + "       DRGADC4 = '" + txtBCode4.Text + "', ";
                SQL += ComNum.VBLF + "       DRGADC5 = '" + txtBCode5.Text + "'  ";
                SQL += ComNum.VBLF + " WHERE TRSNO = '" + FstrTRSNO + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                clsDRG.Insert_Drg_History(clsDB.DbCon, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.Ipdno, txtDCode.Text, txtBCode1.Text, txtBCode2.Text, txtBCode3.Text, txtBCode4.Text, txtBCode5.Text, "1");

                ComFunc.MsgBox("DRG 번호 부여완료");

                Drg_Master_New_Rtn();

                Screen_Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
            }

        }

        private void Drg_Master_New_Rtn()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (FstrROWID == "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO ADMIN.DRG_MASTER_NEW (     ";
                    SQL += ComNum.VBLF + "      TRSNO,IPDNO,pano,sName,jindate ,Ilsu,   ";
                    SQL += ComNum.VBLF + "      ILLCODE1,ILLCODE2,ILLCODE3,ILLCODE4,    ";
                    SQL += ComNum.VBLF + "      ILLCODE5,ILLCODE6,ILLCODE7,ILLCODE8,    ";
                    SQL += ComNum.VBLF + "      ILLCODE9,ILLCODE10,                     ";
                    SQL += ComNum.VBLF + "      OPCODE1,OPCODE2,OPCODE3,OPCODE4,        ";
                    SQL += ComNum.VBLF + "      OPCODE5,OPCODE6,OPCODE7,OPCODE8,        ";
                    SQL += ComNum.VBLF + "      OPCODE9,OPCODE10,                       ";
                    SQL += ComNum.VBLF + "      EXCODE1,EXCODE2,EXCODE3,EXCODE4,EXCODE5,";
                    SQL += ComNum.VBLF + "      XRCODE1,XRCODE2,XRCODE3,XRCODE4,XRCODE5,";
                    SQL += ComNum.VBLF + "      JCODE1,JCODE2,JCODE3,JCODE4,JCODE5,     ";
                    SQL += ComNum.VBLF + "      MCODE1,MCODE2,MCODE3,MCODE4,MCODE5,     ";
                    SQL += ComNum.VBLF + "      BCODE1,BCODE2,BCODE3,BCODE4,BCODE5,     ";
                    SQL += ComNum.VBLF + "      ACODE,Weight,CPR,MDCCODE,DRGCODE, MDCCODEOLD,DRGCODEOLD, ENTDATE1,ENTDATE2,ENTSABUN1,ENTSABUN2  ) ";
                    SQL += ComNum.VBLF + " VALUES (  ";
                    SQL += ComNum.VBLF + "      '" + FstrTRSNO + "', '" + FstrIPDNO + "' , '" + FstrPano + "', ";
                    SQL += ComNum.VBLF + "      '" + txtSName.Text + "', '" + txtJinDate.Text + "', '" + txtJinIlsu.Text + "', ";
                    SQL += ComNum.VBLF + "      '" + txtILLCode1.Text + "', '" + txtILLCode2.Text + "' , '" + txtILLCode3.Text + "', '" + txtILLCode4.Text + "', ";
                    SQL += ComNum.VBLF + "      '" + txtILLCode5.Text + "', '" + txtILLCode6.Text + "' , '" + txtILLCode7.Text + "', '" + txtILLCode8.Text + "', ";
                    SQL += ComNum.VBLF + "      '" + txtILLCode9.Text + "', '" + txtILLCode10.Text + "', ";
                    SQL += ComNum.VBLF + "      '" + txtOpCode1.Text + "', '" + txtOpCode2.Text + "' , '" + txtOpCode3.Text + "', '" + txtOpCode4.Text + "', ";
                    SQL += ComNum.VBLF + "      '" + txtOpCode5.Text + "', '" + txtOpCode6.Text + "' , '" + txtOpCode7.Text + "' , '" + txtOpCode8.Text + "',";
                    SQL += ComNum.VBLF + "      '" + txtOpCode9.Text + "', '" + txtOpCode10.Text + "', ";
                    SQL += ComNum.VBLF + "      '" + txtExCode1.Text + "', '" + txtExCode2.Text + "', '" + txtExCode3.Text + "', '" + txtExCode4.Text + "', '" + txtExCode5.Text + "', ";
                    SQL += ComNum.VBLF + "      '" + txtXCode1.Text + "' , '" + txtXCode2.Text + "' , '" + txtXCode3.Text + "', '" + txtXCode4.Text + "', '" + txtXCode5.Text + "', ";
                    SQL += ComNum.VBLF + "      '" + txtJCode1.Text + "' , '" + txtJCode2.Text + "' , '" + txtJCode3.Text + "', '" + txtJCode4.Text + "', '" + txtJCode5.Text + "', ";
                    SQL += ComNum.VBLF + "      '" + txtMCode1.Text + "' , '" + txtMCode2.Text + "' , '" + txtMCode3.Text + "', '" + txtMCode4.Text + "', '" + txtMCode5.Text + "', ";
                    SQL += ComNum.VBLF + "      '" + txtBCode1.Text + "' , '" + txtBCode2.Text + "' , '" + txtBCode3.Text + "', '" + txtBCode4.Text + "', '" + txtBCode5.Text + "', ";
                    SQL += ComNum.VBLF + "      '" + VB.Left(cboACode.Text, 5) + "' , '" + txtWeight.Text + "' , '" + txtCPR.Text + "' , ";
                    SQL += ComNum.VBLF + "      '" + txtMDC.Text + "', '" + txtDCode.Text + "',  '" + txtMDC.Text + "', '" + txtDCode.Text + "', ";
                    SQL += ComNum.VBLF + " SYSDATE, '', '" + clsPublic.GnJobSabun + "'  , '' ";
                    SQL += ComNum.VBLF + ")";
                }
                else
                {
                    SQL = "UPDATE  ADMIN.DRG_MASTER_NEW SET ";
                    SQL += ComNum.VBLF + "  Jindate  = '" + txtJinDate.Text + "', ";
                    SQL += ComNum.VBLF + "  Ilsu  = '" + txtJinIlsu.Text + "', ";
                    SQL += ComNum.VBLF + "  ILLCODE1 = '" + txtILLCode1.Text + "', ";
                    SQL += ComNum.VBLF + "  ILLCODE2 = '" + txtILLCode2.Text + "', ";
                    SQL += ComNum.VBLF + "  ILLCODE3 = '" + txtILLCode3.Text + "', ";
                    SQL += ComNum.VBLF + "  ILLCODE4 = '" + txtILLCode4.Text + "', ";
                    SQL += ComNum.VBLF + "  ILLCODE5 = '" + txtILLCode5.Text + "', ";
                    SQL += ComNum.VBLF + "  ILLCODE6 = '" + txtILLCode6.Text + "', ";
                    SQL += ComNum.VBLF + "  ILLCODE7 = '" + txtILLCode7.Text + "', ";
                    SQL += ComNum.VBLF + "  ILLCODE8 = '" + txtILLCode8.Text + "', ";
                    SQL += ComNum.VBLF + "  ILLCODE9 = '" + txtILLCode9.Text + "', ";
                    SQL += ComNum.VBLF + "  ILLCODE10 = '" + txtILLCode10.Text + "', ";

                    SQL += ComNum.VBLF + "   OPCODE1 = '" + txtOpCode1.Text + "', ";
                    SQL += ComNum.VBLF + "   OPCODE2 = '" + txtOpCode2.Text + "', ";
                    SQL += ComNum.VBLF + "   OPCODE3 = '" + txtOpCode3.Text + "', ";
                    SQL += ComNum.VBLF + "   OPCODE4 = '" + txtOpCode4.Text + "', ";
                    SQL += ComNum.VBLF + "   OPCODE5 = '" + txtOpCode5.Text + "', ";
                    SQL += ComNum.VBLF + "   OPCODE6 = '" + txtOpCode6.Text + "', ";
                    SQL += ComNum.VBLF + "   OPCODE7 = '" + txtOpCode7.Text + "', ";
                    SQL += ComNum.VBLF + "   OPCODE8 = '" + txtOpCode8.Text + "', ";
                    SQL += ComNum.VBLF + "   OPCODE9 = '" + txtOpCode9.Text + "', ";
                    SQL += ComNum.VBLF + "   OPCODE10 = '" + txtOpCode10.Text + "', ";

                    SQL += ComNum.VBLF + "  EXCODE1 = '" + txtExCode1.Text + "', ";
                    SQL += ComNum.VBLF + "  EXCODE2 = '" + txtExCode2.Text + "', ";
                    SQL += ComNum.VBLF + "  EXCODE3 = '" + txtExCode3.Text + "', ";
                    SQL += ComNum.VBLF + "  EXCODE4 = '" + txtExCode4.Text + "', ";
                    SQL += ComNum.VBLF + "  EXCODE5 = '" + txtExCode5.Text + "', ";

                    SQL += ComNum.VBLF + "  XRCODE1 = '" + txtXCode1.Text + "',";
                    SQL += ComNum.VBLF + "  XRCODE2 = '" + txtXCode2.Text + "',";
                    SQL += ComNum.VBLF + "  XRCODE3 = '" + txtXCode3.Text + "',";
                    SQL += ComNum.VBLF + "  XRCODE4 = '" + txtXCode4.Text + "',";
                    SQL += ComNum.VBLF + "  XRCODE5 = '" + txtXCode5.Text + "',";

                    SQL += ComNum.VBLF + "   JCODE1 = '" + txtJCode1.Text + "', ";
                    SQL += ComNum.VBLF + "   JCODE2 = '" + txtJCode2.Text + "', ";
                    SQL += ComNum.VBLF + "   JCODE3 = '" + txtJCode3.Text + "', ";
                    SQL += ComNum.VBLF + "   JCODE4 = '" + txtJCode4.Text + "', ";
                    SQL += ComNum.VBLF + "   JCODE5 = '" + txtJCode5.Text + "', ";

                    SQL += ComNum.VBLF + "   MCODE1 = '" + txtMCode1.Text + "', ";
                    SQL += ComNum.VBLF + "   MCODE2 = '" + txtMCode2.Text + "', ";
                    SQL += ComNum.VBLF + "   MCODE3 = '" + txtMCode3.Text + "', ";
                    SQL += ComNum.VBLF + "   MCODE4 = '" + txtMCode4.Text + "', ";
                    SQL += ComNum.VBLF + "   MCODE5 = '" + txtMCode5.Text + "', ";


                    SQL += ComNum.VBLF + "   BCODE1 = '" + txtBCode1.Text + "', ";
                    SQL += ComNum.VBLF + "   BCODE2 = '" + txtBCode2.Text + "', ";
                    SQL += ComNum.VBLF + "   BCODE3 = '" + txtBCode3.Text + "', ";
                    SQL += ComNum.VBLF + "   BCODE4 = '" + txtBCode4.Text + "', ";
                    SQL += ComNum.VBLF + "   BCODE5 = '" + txtBCode5.Text + "', ";

                    SQL += ComNum.VBLF + "   ACODE = '" + VB.Left(cboACode.Text, 5) + "', ";
                    SQL += ComNum.VBLF + "   WEIGHT = '" + txtWeight.Text + "', ";
                    SQL += ComNum.VBLF + "   CPR = '" + txtCPR.Text + "', ";
                    SQL += ComNum.VBLF + "   ENTDATE2 = SYSDATE , ";
                    SQL += ComNum.VBLF + "   ENTSABUN2 = '" + clsPublic.GnJobSabun + "', ";
                    
                    SQL += ComNum.VBLF + "   MDCCODE =  '" + txtMDC.Text + "', ";
                    SQL += ComNum.VBLF + "   DRGCODE =  '" + txtDCode.Text + "'  ";
                    
                    SQL += ComNum.VBLF + "  WHERE ROWID = '" + FstrROWID + "' ";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
            }
            
        }

        private void btnViewForm_Click(object sender, EventArgs e)
        {
            clsPublic.GstrPANO = txtPano.Text.Trim();
            frmPmpaDrgResearch frm = new frmPmpaDrgResearch();
            frm.ShowDialog();
        }
    }
}
