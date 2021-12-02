using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComLibB;
using FarPoint.Win.Spread;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewOpdSlipNew.cs
    /// Description     : 외래진료비내역
    /// Author          : 박웅규
    /// Create Date     : 2018-11-01
    /// <history>       
    /// TODO : 출력 부분 실제 테스트 필요
    /// d:\psmh\OPD\oviewa\OVIEWA10_NEW.FRM(FrmOpdSlip_new) => frmPmpaViewOpdSlipNew.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA10_NEW.FRM(FrmOpdSlip_new)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewOpdSlipNew : Form
    {
        ComFunc CF = new ComFunc();
        //frmSetPrintInfo frmSetPrintInfoX = new frmSetPrintInfo();

        string mstrPano = "";
        string mstrBuseCode = "";
        string mstrJobName = "";
        string mstrJobSabun = "";

        string strFnu = "";
        string strTnu = "";
        string strBi = "";
        string strBiName = "";
        string strDeptCode = "";
        string strDeptName = "";
        string strSname = "";
        string strChkBi = "";
        int nGETcount = 0;

        public frmPmpaViewOpdSlipNew()
        {
            InitializeComponent();
        }

        public frmPmpaViewOpdSlipNew(string GstrPANO, string PassBuse, string strJobName, string strJobSabun)
        {
            InitializeComponent();
            mstrPano = GstrPANO;
            mstrBuseCode = PassBuse;
            mstrJobName = strJobName;
            mstrJobSabun = strJobSabun;
        }

        private void frmPmpaViewOpdSlipNew_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            CF = new ComFunc();

            lblSname.Text = "이　　름";
            chkJin.Checked = true;

            this.Text = "";
            Set_Combo();
            NuName_Setting();
            ClearData();
            txtPano.Focus();

            if (mstrPano != "")
            {
                txtPano.Text = mstrPano;
                
            }
        }

        void ClearData()
        {
            ssList_Sheet1.Rows.Count = 0;

            cboFnu.SelectedIndex = 0;
            cboTnu.SelectedIndex = 0;
            cboBi.SelectedIndex = 0;
            cboDept.SelectedIndex = 0;
            
            strFnu = "01";
            strTnu = "50";
            txtPano.Text = "";
            lblSname.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void Set_Combo()
        {
            int i = 0;
            string strDept = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            cboFnu.Items.Add(""); cboTnu.Items.Add("");
            cboFnu.Items.Add("진찰료"); cboTnu.Items.Add("진찰료");
            cboFnu.Items.Add("입원료"); cboTnu.Items.Add("입원료");
            cboFnu.Items.Add("투약료"); cboTnu.Items.Add("투약료");
            cboFnu.Items.Add("주사료"); cboTnu.Items.Add("주사료");
            cboFnu.Items.Add("마취료"); cboTnu.Items.Add("마취료");
            cboFnu.Items.Add("PT/NS"); cboTnu.Items.Add("PT/NS");
            cboFnu.Items.Add("처치료"); cboTnu.Items.Add("처치료");
            cboFnu.Items.Add("수술료"); cboTnu.Items.Add("수술료");
            cboFnu.Items.Add("검사료"); cboTnu.Items.Add("검사료");
            cboFnu.Items.Add("방사선"); cboTnu.Items.Add("방사선");
            cboFnu.Items.Add("비급여"); cboTnu.Items.Add("비급여");
            cboFnu.Items.Add("현금계정"); cboTnu.Items.Add("현금계정");

            //환자종류
            cboBi.Items.Clear();
            cboBi.Items.Add("00.전체");
            cboBi.Items.Add("11.공단");
            cboBi.Items.Add("12.연합회");
            cboBi.Items.Add("13.지역");
            cboBi.Items.Add("21.보호1종");
            cboBi.Items.Add("22.보호2종");
            cboBi.Items.Add("23.의료부조");
            cboBi.Items.Add("24.행려환자");
            cboBi.Items.Add("31.산재");
            cboBi.Items.Add("32.공무원공상");
            cboBi.Items.Add("33.산재공상");
            cboBi.Items.Add("41.공단100%");
            cboBi.Items.Add("42.직장100%");
            cboBi.Items.Add("43.지역100%");
            cboBi.Items.Add("44.가족계획");
            cboBi.Items.Add("45.보험계약");
            cboBi.Items.Add("51.일반");
            cboBi.Items.Add("52.TA보험");
            cboBi.Items.Add("53.계약");
            cboBi.Items.Add("54.미확인");
            cboBi.Items.Add("55.TA일반");
            cboBi.SelectedIndex = 1;

            //진료과
            cboDept.Items.Clear();
            cboDept.Items.Add("00.전체");

            strDept = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "* FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
            SQL += ComNum.VBLF + "WHERE PrintRanking < 40";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strDept = dt.Rows[i]["DeptCode"].ToString().Trim() + ".";
                        strDept += dt.Rows[i]["DeptNameK"].ToString().Trim();
                        cboDept.Items.Add(strDept);
                    }
                }
                cboDept.Items.Add("RD.영상의학과");
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

        }

        void NuName_Setting()
        {
            clsPmpaPb.GstrSETNus[0] = "진찰료";
            clsPmpaPb.GstrSETNus[1] = "입원실료";
            clsPmpaPb.GstrSETNus[2] = "환자관리";
            clsPmpaPb.GstrSETNus[3] = "투약료";
            clsPmpaPb.GstrSETNus[4] = "주사료";
            clsPmpaPb.GstrSETNus[5] = "마취료";
            clsPmpaPb.GstrSETNus[6] = "물리치료";
            clsPmpaPb.GstrSETNus[7] = "신경정신";
            clsPmpaPb.GstrSETNus[8] = "처치료";
            clsPmpaPb.GstrSETNus[9] = "수술분만";
            clsPmpaPb.GstrSETNus[10] = "수혈료";
            clsPmpaPb.GstrSETNus[11] = "기브스료";
            clsPmpaPb.GstrSETNus[12] = "특수검사";
            clsPmpaPb.GstrSETNus[13] = "기타검사";
            clsPmpaPb.GstrSETNus[14] = "방사선료";
            clsPmpaPb.GstrSETNus[15] = "식대료";
            clsPmpaPb.GstrSETNus[16] = "보호안치료";
            clsPmpaPb.GstrSETNus[17] = "MRI(급여)";
            clsPmpaPb.GstrSETNus[18] = "급여C/T";
            clsPmpaPb.GstrSETNus[19] = "기타급여";
            clsPmpaPb.GstrSETNus[20] = "비급기본";
            clsPmpaPb.GstrSETNus[21] = "비급투약료";
            clsPmpaPb.GstrSETNus[22] = "비급주사료";
            clsPmpaPb.GstrSETNus[23] = "비급마취료";
            clsPmpaPb.GstrSETNus[24] = "비급물리료";
            clsPmpaPb.GstrSETNus[25] = "비급신경료";
            clsPmpaPb.GstrSETNus[26] = "비급처치료";
            clsPmpaPb.GstrSETNus[27] = "비급수술료";
            clsPmpaPb.GstrSETNus[28] = "비급수혈료";
            clsPmpaPb.GstrSETNus[29] = "비급기브료";
            clsPmpaPb.GstrSETNus[30] = "비급특검료";
            clsPmpaPb.GstrSETNus[31] = "비급검사료";
            clsPmpaPb.GstrSETNus[32] = "비급방사선";
            clsPmpaPb.GstrSETNus[33] = "식대료";
            clsPmpaPb.GstrSETNus[34] = "실료차";
            clsPmpaPb.GstrSETNus[35] = "초음파";
            clsPmpaPb.GstrSETNus[36] = "C/T";
            clsPmpaPb.GstrSETNus[37] = "MRI(비급여)";
            clsPmpaPb.GstrSETNus[38] = "보조기";
            clsPmpaPb.GstrSETNus[39] = "보철료";
            clsPmpaPb.GstrSETNus[40] = "구급차사용";
            clsPmpaPb.GstrSETNus[41] = "안치료";
            clsPmpaPb.GstrSETNus[42] = "골밀도검사";
            clsPmpaPb.GstrSETNus[43] = "특진료";
            clsPmpaPb.GstrSETNus[44] = "";
            clsPmpaPb.GstrSETNus[45] = "전화료";
            clsPmpaPb.GstrSETNus[46] = "제증명료";
            clsPmpaPb.GstrSETNus[47] = "특수재료대";
            clsPmpaPb.GstrSETNus[48] = "";
            clsPmpaPb.GstrSETNus[49] = "합계금액";
            clsPmpaPb.GstrSETNus[50] = "보증금";
            clsPmpaPb.GstrSETNus[51] = "중간납";
            clsPmpaPb.GstrSETNus[52] = "조합부담금";
            clsPmpaPb.GstrSETNus[53] = "감액";
            clsPmpaPb.GstrSETNus[54] = "차인납부액";
            clsPmpaPb.GstrSETNus[55] = "개인미수";
            clsPmpaPb.GstrSETNus[56] = "퇴원금";
            clsPmpaPb.GstrSETNus[57] = "환불금";
            clsPmpaPb.GstrSETNus[58] = "";
            clsPmpaPb.GstrSETNus[59] = "전액본인부담";
            clsPmpaPb.GstrSETNus[60] = "";
            clsPmpaPb.GstrSETNus[61] = "";
            clsPmpaPb.GstrSETNus[62] = "";
            clsPmpaPb.GstrSETNus[63] = "외래영수금";
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (!VB.IsNumeric(txtPano.Text))
                {

                }
                else
                {
                    txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
                    Read_Bas_Patient(txtPano.Text);
                    cboDept.Focus();
                }

            }
        }
       

        void Read_Bas_Patient(string strPano)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                ";
            SQL += ComNum.VBLF + "  Sname";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
            SQL += ComNum.VBLF + "WHERE Pano = '" + strPano + "'        ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox(txtPano.Text + " 환자마스타 없습니다.");
                    txtPano.Focus();
                }

                strSname = dt.Rows[0]["Sname"].ToString().Trim();
                lblSname.Text = strSname;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            btnClear.Enabled = true;
            btnExit.Enabled = true;

            dtpFdate.Enabled = true;
            dtpTdate.Enabled = true;
            cboFnu.Enabled = true;
            cboTnu.Enabled = true;
            btnSearch.Enabled = true;

            dt.Dispose();
            dt = null;
        }

        private void cboFnu_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboFnu.SelectedIndex)
            {
                case 2:
                    strFnu = "02";
                    break;
                case 3:
                    strFnu = "04";
                    break;
                case 4:
                    strFnu = "05";
                    break;
                case 5:
                    strFnu = "06";
                    break;
                case 6:
                    strFnu = "07";
                    break;
                case 7:
                    strFnu = "09";
                    break;
                case 8:
                    strFnu = "10";
                    break;
                case 9:
                    strFnu = "13";
                    break;
                case 10:
                    strFnu = "15";
                    break;
                case 11:
                    strFnu = "21";
                    break;
                case 12:
                    strFnu = "51";
                    break;
                default:
                    strFnu = "01";
                    break;
            }
        }

        private void cboTnu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTnu.SelectedIndex < cboFnu.SelectedIndex)
            {
                cboTnu.SelectedIndex = cboFnu.SelectedIndex;
            }

            switch (cboTnu.SelectedIndex)
            {
                case 1:
                    strTnu = "01";
                    break;
                case 2:
                    strTnu = "03";
                    break;
                case 3:
                    strTnu = "04";
                    break;
                case 4:
                    strTnu = "05";
                    break;
                case 5:
                    strTnu = "06";
                    break;
                case 6:
                    strTnu = "08";
                    break;
                case 7:
                    strTnu = "09";
                    break;
                case 8:
                    strTnu = "12";
                    break;
                case 9:
                    strTnu = "14";
                    break;
                case 10:
                    strTnu = "20";
                    break;
                case 11:
                    strTnu = "50";
                    break;
                case 12:
                    strTnu = "99";
                    break;
                default:
                    strTnu = "50";
                    break;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintData();
        }

        private void GetData()
        {
            ssList_Sheet1.Rows.Count = 0;

            strBi = VB.Left(cboBi.SelectedItem.ToString(), 2);
            strBiName = VB.Mid(cboBi.SelectedItem.ToString() + VB.Space(12), cboBi.SelectedItem.ToString().Length + 1, 12);
            strDeptCode = VB.Left(cboDept.Text, 2);
            strDeptName = VB.Mid(cboDept.SelectedItem.ToString() + VB.Space(12), cboDept.SelectedItem.ToString().Length + 1, 12);

            if (dtpFdate.Text == "")
            {
                ComFunc.MsgBox("시작일자를 입력하세요 !!");
                return;
            }

            if (dtpTdate.Text == "")
            {
                ComFunc.MsgBox("종료일자를 입력하세요 !!");
                return;
            }

            if (String.Compare(strTnu, strFnu) < 0)
            {
                ComFunc.MsgBox("행위범위 FROM < TO Error !!");
                return;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            Read_OPD_SLIP2();

            strChkBi = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strDrCode = "";

            if (chkDoctor.Checked == true)
            {
                strDrCode = "";

                SQL = " SELECT DRCODE,BI From OPD_MASTER Where Pano='" + VB.Trim(txtPano.Text) + "' ";
                SQL = SQL + ComNum.VBLF + " AND BDATE>=TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND BDATE<=TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                if (VB.Left(cboDept.Text, 2) != "00")
                {
                    SQL = SQL + ComNum.VBLF + " AND DEPTCODE='" + VB.Trim(VB.Pstr(cboDept.Text, ".", 1)) + "' ";
                }
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (VB.L(strDrCode, CF.ReadBASDoctor(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim())) < 2)
                        {
                            strDrCode = strDrCode + CF.ReadBASDoctor(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim()) + "(" + CF.READ_OCS_Doctor3_DrBunho(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim()) + "), ";
                            if (string.Compare(dt.Rows[i]["BI"].ToString().Trim(), "30") <= 0)
                            {
                                strChkBi = "OK";
                            }
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                if (VB.Right(strDrCode, 2) == ", ")
                {
                    strDrCode = VB.Left(strDrCode, VB.Len(strDrCode) - 2);
                }

                //TODO
                ssList_Sheet1.RowCount = ssList_Sheet1.RowCount + 1;
                ssList_Sheet1.AddSpanCell(ssList_Sheet1.RowCount - 1, 0, 1, 3);
                ssList_Sheet1.AddSpanCell(ssList_Sheet1.RowCount - 1, 3, 1, ssList_Sheet1.Columns.Count - 3);
                ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 3].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].Text = "요양기관번호: 37100068";
                ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 3].Text = "주치의: " + strDrCode;
            }
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            if (String.Compare(dtpFdate.Text, Convert.ToDateTime(dtpTdate.Text).AddDays(-1830).ToShortDateString()) <= 0)
            {
                ComFunc.MsgBox("영수증 재발행은 5년까지만 가능합니다.");

                btnPrint.Enabled = false;

                if (CF.JIN_AMT_PRINT_CHK(clsDB.DbCon, clsType.User.Sabun) == "OK")
                {
                    btnPrint.Enabled = true;
                }
            }
            else
            {
                btnPrint.Enabled = true;
            }
        }

        private void Read_OPD_SLIP2()
        {
            string strBday = "";
            string strBdaySW = "";
            string strBdayPrint = "";
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAmt1 = "";
            string strAmt2 = "";
            string strAddName = "";
            string strAddER = "";
            string strAddNight = "";
            string strAddHoliday = "";

            int i = 0;
            int j = 0;
            int nRowInx = 0;

            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;
            long nBAmt1 = 0;
            long nTBAmt1 = 0;
            long nBAmt2 = 0;
            long nTBAmt2 = 0;
            long nBAmt3 = 0;
            long nTBAmt3 = 0;
            long nBAmt4 = 0;
            long nTBAmt4 = 0;

            nGETcount = 0;

            ComFunc CF = new ComFunc();

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            #region //SQL_SET_SLIP2

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT bi,TO_CHAR(Bdate, 'yy-mm-dd') Bday,deptcode,bun, ";
            SQL = SQL + ComNum.VBLF + "       Nu,Sucode,SunameK,B.bcode ,BaseAmt,Qty,b.SugbP,i.GBSugbs,";
            if (chkMinus.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "       GbSpc,GbNgt,GbGisul,GbSelf,GbChild,Amt1 Amtt1 ,Amt2 Amtt2,Nal SNal, ";
                SQL = SQL + ComNum.VBLF + "        FC_ACCOUNT_BON_AMT(i.pano,i.bi,i.Sucode,Amt1, Qty ,bun,nu,to_char(i.bdate,'yyyy-mm-dd'),'O',i.deptcode,gbsugbs,BAS_VCODE_SELECT(i.pano,to_char(i.bdate,'yyyy-mm-dd'),i.deptcode),i.GbSelf) BON    ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "       GbSpc,GbNgt,GbGisul,GbSelf,GbChild,Sum(Amt1) Amtt1, Sum(Amt2) Amtt2, Sum(Nal) SNal, ";
                SQL = SQL + ComNum.VBLF + "        FC_ACCOUNT_BON_AMT(i.pano,i.bi,i.Sucode,sum(Amt1), Qty ,bun,nu,to_char(i.bdate,'yyyy-mm-dd'),'O',i.deptcode,gbsugbs,BAS_VCODE_SELECT(i.pano,to_char(i.bdate,'yyyy-mm-dd'),i.deptcode),i.GbSelf) BON    ";
            }
            SQL = SQL + ComNum.VBLF + "  , GBER ";
            SQL = SQL + ComNum.VBLF + "  FROM OPD_SLIP I,  BAS_SUN B";
            SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + txtPano.Text + "' ";
            if (strBi != "00")
            {
                SQL = SQL + ComNum.VBLF + "AND Bi   = '" + strBi + "' ";
            }
            if (strDeptCode != "00")
            {
                SQL = SQL + ComNum.VBLF + "AND DeptCode = '" + strDeptCode + "' ";
            }
            SQL = SQL + ComNum.VBLF + "   AND Nu >= '" + strFnu + "' ";
            SQL = SQL + ComNum.VBLF + "   AND Nu <= '" + strTnu + "' ";
            SQL = SQL + ComNum.VBLF + "   AND Bdate >= TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND Bdate <= TO_DATE('" + dtpTdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND I.Sunext = B.Sunext ";
            SQL = SQL + ComNum.VBLF + "   AND TRIM(i.SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";  //저가약제 제외코드 2011-04-09
            if (chkMinus.Checked == false)
            {
                SQL = SQL + ComNum.VBLF + " group by   i.pano,i.bi,Bdate,bun,i.deptcode,Nu,Sucode,SunameK,b.bcode,BaseAmt,Qty,GbSpc,GbNgt,GbGisul,GbSelf,GbChild,gbsugbs, b.SugbP";
                SQL = SQL + ComNum.VBLF + "  , GBER ";
                SQL = SQL + ComNum.VBLF + " Having Sum(Nal) != 0 ";
            }
            if (clsPublic.GstrSysDate == dtpTdate.Value.ToString("yyyy-MM-dd") && chkJin.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " UNION ALL ";
                SQL = SQL + ComNum.VBLF + "  SELECT  bi,TO_CHAR(Bdate, 'yy-mm-dd') Bday,deptcode,'01' Nu, '01' bun , ";
                SQL = SQL + ComNum.VBLF + "  decode(chojae,'1','AA156','2','AA156010','3','AA256','4','AA256010','5','AA156050','6','AA256050')  Sucode, ";
                if (chkMinus.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " '진찰료' SunameK, ";
                    SQL = SQL + ComNum.VBLF + " decode(chojae,'1','AA156','2','AA156010','3','AA256','4','AA256010','5','AA156050','6','AA256050') bcode, ";
                    SQL = SQL + ComNum.VBLF + " AMT1 BASEAMT  , 1 Qty, '' SugbP, '' GBSugbs, ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " '진찰료' SunameK, ";
                    SQL = SQL + ComNum.VBLF + " decode(chojae,'1','AA156','2','AA156010','3','AA256','4','AA256010','5','AA156050','6','AA256050') bcode, ";
                    SQL = SQL + ComNum.VBLF + " AMT1 BASEAMT  , COUNT(Pano) Qty, '' SugbP, '' GBSugbs, ";
                }
                SQL = SQL + ComNum.VBLF + "    '' GbSpc, '' GbNgt,'' GbGisul, '1' GbSelf, '' GbChild,  ";
                if (chkMinus.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "     Amt1 Amtt1,0 as Amtt2, 1 Nal,  ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "     SUM(Amt1) Amtt1,0 as Amtt2, 1 Nal , ";
                }
                SQL = SQL + ComNum.VBLF + "        FC_ACCOUNT_BON_AMT(pano,bi,decode(chojae,'1','AA156','2','AA156010','3','AA256','4','AA256010','5','AA156050','6','AA256050'),Amt1 ,1,'01','01',to_char(bdate,'yyyy-mm-dd'),'O',deptcode,'0',BAS_VCODE_SELECT(pano,to_char(bdate,'yyyy-mm-dd'),deptcode),'') BON    ";
                SQL = SQL + ComNum.VBLF + "  , '0' GBER ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + "   WHERE PANO ='" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "    AND ACTDATE =trunc(sysdate) and jin!=  'D'";
                if (strBi != "00")
                {
                    SQL = SQL + ComNum.VBLF + "AND Bi   = '" + strBi + "' ";
                }
                if (strDeptCode != "00")
                {
                    SQL = SQL + ComNum.VBLF + "AND DeptCode = '" + strDeptCode + "' ";
                }
                if (chkMinus.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + " GROUP by pano,bi,Bdate,deptcode,2,decode(chojae,'1','AA156','2','AA156010','3','AA256','4','AA256010','5','AA156050','6','AA256050') ,4,5,AMT1,8,9,10,11,12,13,15  ";
                    SQL = SQL + ComNum.VBLF + " , '0' "; //GBER
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY  2,1, 3, 4";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY Bdate, Nu, Sucode ";
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }
            
            #endregion //SQL_SET_SLIP2

            #region //SQL_MAIN_SLIP2
            nStot1 = 0;
            nStot2 = 0;
            nGtot1 = 0;
            nGtot2 = 0;
            nRowInx = 0;
            strBdaySW = "";

            nGETcount = nGETcount + dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strBday = dt.Rows[i]["Bday"].ToString().Trim();
                if (strBdaySW.Trim() == "")
                {
                    strBdayPrint = strBday;
                    strBdaySW = strBday;
                }

                if (strBday != strBdaySW)
                {
                    #region //SUB_TOT_SLIP2
                    nRowInx = nRowInx + 1;
                    if (ssList_Sheet1.RowCount < nRowInx)
                    {
                        ssList_Sheet1.RowCount = nRowInx;
                    }

                    //TODO
                    //굵은선
                    ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGray;

                    strAmt1 = nStot1.ToString("###,###,##0");
                    strAmt2 = nStot2.ToString("###,###,##0");

                    ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "일별소계";
                    //ssList_Sheet1.AddSpanCell(nRowInx - 1, 0, 1, 7);
                    //ssList_Sheet1.Cells[nRowInx - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                    ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                    ssList_Sheet1.Cells[nRowInx - 1, 15].Text = nBAmt1.ToString("###,###,##0");
                    ssList_Sheet1.Cells[nRowInx - 1, 16].Text = nBAmt2.ToString("###,###,##0");
                    ssList_Sheet1.Cells[nRowInx - 1, 17].Text = nBAmt3.ToString("###,###,##0");
                    ssList_Sheet1.Cells[nRowInx - 1, 18].Text = nBAmt4.ToString("###,###,##0");

                    nStot1 = 0;
                    nStot2 = 0;

                    nBAmt1 = 0;
                    nBAmt2 = 0;
                    nBAmt3 = 0;
                    nBAmt4 = 0;

                    #endregion //SUB_TOT_SLIP2

                    strBdayPrint = strBday;
                    strBdaySW = strBday;
                }

                #region //DATA_MOVE_SLIP2
                nRowInx = nRowInx + 1;
                if (ssList_Sheet1.RowCount < nRowInx)
                {
                    ssList_Sheet1.RowCount = nRowInx;
                }
                nAmt1 = (long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim());
                nAmt2 = (long)VB.Val(dt.Rows[i]["Amtt2"].ToString().Trim());
                nStot1 = nStot1 + nAmt1;
                nStot2 = nStot2 + nAmt2;
                nGtot1 = nGtot1 + nAmt1;
                nGtot2 = nGtot2 + nAmt2;

                strBaseAmt = ((long)VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim())).ToString("##,###,##0");
                strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                strNal = ((int)VB.Val(dt.Rows[i]["SNal"].ToString().Trim())).ToString("##,###,##0");
                strAmt1 = nAmt1.ToString("###,###,##0");
                strAmt2 = nAmt2.ToString("###,###,##0");

                if (strNujuk != CF.Read_Bcode_Name(clsDB.DbCon, "BAS_누적행위명", ComFunc.SetAutoZero(dt.Rows[i]["NU"].ToString().Trim(), 2)))
                {
                    ssList_Sheet1.Cells[nRowInx - 1, 0].Text = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_누적행위명", ComFunc.SetAutoZero(dt.Rows[i]["NU"].ToString().Trim(), 2));
                    strNujuk = ssList_Sheet1.Cells[nRowInx - 1, 0].Text;
                }
                else
                {
                    ssList_Sheet1.Cells[nRowInx - 1, 0].Text = "";
                }

                strAddName = "";
                if (dt.Rows[i]["GBER"].ToString().Trim() != "0" && dt.Rows[i]["GBER"].ToString().Trim() != "")
                {
                    strAddName += "응급";
                }
                //야간은 기준이 모호하여 제외함.
                //if (dt.Rows[i]["GbNgt"].ToString().Trim() != "0" && dt.Rows[i]["GBER"].ToString().Trim() != "")
                //{
                //    strAddName += "야간";
                //}
                //if ( CF.DATE_HUIL_CHECK(clsDB.DbCon, "20" + dt.Rows[i]["Bday"].ToString().Trim()) == true)
                //{
                //    strAddName += "공휴";
                //}
                if(strAddName != "")
                {
                    strAddName += ")";
                }

                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = strBdayPrint;
                ssList_Sheet1.Cells[nRowInx - 1, 2].Text = dt.Rows[i]["Bcode"].ToString().Trim();
                ssList_Sheet1.Cells[nRowInx - 1, 3].Text = strAddName + dt.Rows[i]["SunameK"].ToString().Trim();
                ssList_Sheet1.Cells[nRowInx - 1, 4].Text = strBaseAmt;
                ssList_Sheet1.Cells[nRowInx - 1, 5].Text = strQty;
                ssList_Sheet1.Cells[nRowInx - 1, 6].Text = strNal;
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = dt.Rows[i]["sucode"].ToString().Trim();
                //2021-09-13
                if (VB.Val(dt.Rows[i]["GBSugbs"].ToString().Trim()) > 1)
                {
                    ssList_Sheet1.Cells[nRowInx - 1, 19].Text = dt.Rows[i]["GBSugbs"].ToString().Trim();
                }

                nBAmt4 = nBAmt4 + (long)VB.Val(dt.Rows[i]["Amtt2"].ToString().Trim());
                nTBAmt4 = nTBAmt4 + (long)VB.Val(dt.Rows[i]["Amtt2"].ToString().Trim());
                ssList_Sheet1.Cells[nRowInx - 1, 18].Text = ((long)VB.Val(dt.Rows[i]["Amtt2"].ToString().Trim())).ToString("###,###,###,##0");

                //컬럼 없어서 에러남 : 여기로 들어가지 않음
                if ((dt.Rows[i]["SugbP"].ToString().Trim() == "0" || dt.Rows[i]["SugbP"].ToString().Trim() == "2" || dt.Rows[i]["SugbP"].ToString().Trim() == "9") 
                    && dt.Rows[i]["GBSugbs"].ToString().Trim() != "0" 
                    && string.Compare(dt.Rows[i]["Nu"].ToString().Trim(), "21") >= 0 && dt.Rows[i]["gbself"].ToString().Trim() != "1")
                {
                    nBAmt3 = nBAmt3 + (long)VB.Val(dt.Rows[i]["bon"].ToString().Trim());
                    nTBAmt3 = nTBAmt3 + (long)VB.Val(dt.Rows[i]["bon"].ToString().Trim());
                    ssList_Sheet1.Cells[nRowInx - 1, 17].Text = ((long)VB.Val(dt.Rows[i]["BON"].ToString().Trim())).ToString("###,###,###,##0");
                }
                else if (dt.Rows[i]["bi"].ToString().Trim() == "55" && (dt.Rows[i]["gbself"].ToString().Trim() != "0" || dt.Rows[i]["GBSugbs"].ToString().Trim() != "1"))
                   
                {
                    nBAmt4 = nBAmt4 + (long)VB.Val(dt.Rows[i]["bon"].ToString().Trim());
                    nTBAmt4 = nTBAmt4 + (long)VB.Val(dt.Rows[i]["bon"].ToString().Trim());
                    ssList_Sheet1.Cells[nRowInx - 1, 18].Text = ((long)VB.Val(dt.Rows[i]["BON"].ToString().Trim())).ToString("###,###,###,##0");
                }

                else if (string.Compare(dt.Rows[i]["Nu"].ToString().Trim(), "21") >= 0)
                {
                    nBAmt4 = nBAmt4 + (long)VB.Val(dt.Rows[i]["bon"].ToString().Trim());
                    nTBAmt4 = nTBAmt4 + (long)VB.Val(dt.Rows[i]["bon"].ToString().Trim());
                    if (((long)VB.Val(dt.Rows[i]["BON"].ToString().Trim())) != 0)
                    {
                        ssList_Sheet1.Cells[nRowInx - 1, 18].Text = ((long)VB.Val(dt.Rows[i]["BON"].ToString().Trim())).ToString("###,###,###,##0");
                    }
                }
                else
                {
                    nBAmt1 = nBAmt1 + (long)VB.Val(dt.Rows[i]["bon"].ToString().Trim());
                    nTBAmt1 = nTBAmt1 + (long)VB.Val(dt.Rows[i]["bon"].ToString().Trim());
                    nBAmt2 = nBAmt2 + (long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim()) - (long)VB.Val(dt.Rows[i]["bon"].ToString().Trim());
                    nTBAmt2 = nTBAmt2 + (long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim()) - (long)VB.Val(dt.Rows[i]["bon"].ToString().Trim());
                    if (((long)VB.Val(dt.Rows[i]["BON"].ToString().Trim())) != 0)
                    {
                        ssList_Sheet1.Cells[nRowInx - 1, 15].Text = ((long)VB.Val(dt.Rows[i]["BON"].ToString().Trim())).ToString("###,###,###,##0");
                    }
                    if (((long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim()) - (long)VB.Val(dt.Rows[i]["bon"].ToString().Trim())) != 0)
                    {
                        ssList_Sheet1.Cells[nRowInx - 1, 16].Text = ((long)VB.Val(dt.Rows[i]["Amtt1"].ToString().Trim()) - (long)VB.Val(dt.Rows[i]["bon"].ToString().Trim())).ToString("###,###,###,##0");
                    }
                }

                #endregion //DATA_MOVE_SLIP2
                strBdayPrint = "";
            }

            dt.Dispose();
            dt = null;

            if (nGETcount > 0)
            {
                #region //SUB_TOT_SLIP2
                nRowInx = nRowInx + 1;
                if (ssList_Sheet1.RowCount < nRowInx)
                {
                    ssList_Sheet1.RowCount = nRowInx;
                }

                //TODO
                //굵은선
                ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGray;
                strAmt1 = nStot1.ToString("###,###,##0");
                strAmt2 = nStot2.ToString("###,###,##0");

                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "일별소계";
                //ssList_Sheet1.AddSpanCell(nRowInx - 1, 0, 1, 7);
                //ssList_Sheet1.Cells[nRowInx - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                ssList_Sheet1.Cells[nRowInx - 1, 15].Text = nBAmt1.ToString("###,###,##0");
                ssList_Sheet1.Cells[nRowInx - 1, 16].Text = nBAmt2.ToString("###,###,##0");
                ssList_Sheet1.Cells[nRowInx - 1, 17].Text = nBAmt3.ToString("###,###,##0");
                ssList_Sheet1.Cells[nRowInx - 1, 18].Text = nBAmt4.ToString("###,###,##0");

                nStot1 = 0;
                nStot2 = 0;

                nBAmt1 = 0;
                nBAmt2 = 0;
                nBAmt3 = 0;
                nBAmt4 = 0;

                #endregion //SUB_TOT_SLIP2

                #region //GRAND_TOT_SLIP2
                nRowInx = nRowInx + 1;
                if (ssList_Sheet1.RowCount < nRowInx)
                {
                    ssList_Sheet1.RowCount = nRowInx;
                }
                //TODO
                //굵은선
                ssList_Sheet1.Cells[nRowInx - 1, 0, nRowInx - 1, ssList_Sheet1.ColumnCount - 1].BackColor = Color.LightGray;
                strAmt1 = nGtot1.ToString("###,###,##0");
                strAmt2 = nGtot2.ToString("###,###,##0");

                ssList_Sheet1.Cells[nRowInx - 1, 1].Text = "전체합계";
                //ssList_Sheet1.AddSpanCell(nRowInx - 1, 0, 1, 7);
                //ssList_Sheet1.Cells[nRowInx - 1, 0].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssList_Sheet1.Cells[nRowInx - 1, 7].Text = strAmt1;
                ssList_Sheet1.Cells[nRowInx - 1, 8].Text = strAmt2;
                ssList_Sheet1.Cells[nRowInx - 1, 15].Text = nTBAmt1.ToString("###,###,##0");
                ssList_Sheet1.Cells[nRowInx - 1, 16].Text = nTBAmt2.ToString("###,###,##0");
                ssList_Sheet1.Cells[nRowInx - 1, 17].Text = nTBAmt3.ToString("###,###,##0");
                ssList_Sheet1.Cells[nRowInx - 1, 18].Text = nTBAmt4.ToString("###,###,##0");

                nGtot1 = 0;
                nGtot2 = 0;

                nTBAmt1 = 0;
                nTBAmt2 = 0;
                nTBAmt3 = 0;
                nTBAmt4 = 0;
                #endregion //GRAND_TOT_SLIP2

                btnPrint.Enabled = true;
            }

            #endregion //SQL_MAIN_SLIP2
        }

        private void PrintData()
        {
            clsSpread SPR = new clsSpread();

            if (MessageBox.Show("인쇄를 하시겠습니까?", "알 림", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            if (strChkBi == "OK" && chkDoctor.Checked == true)
            {
                if (DialogResult.No == MessageBox.Show("건보+보호자격에 요양병원체크가 되어있습니다. 그래도 출력하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    return;
            }

            clsPublic.GstrRetValue = txtPano.Text + "^^" + "외래^^" + dtpFdate.Text + "~" + dtpTdate.Text + "^^";
            frmSetPrintInfo f = new frmSetPrintInfo();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog(this);

            if (ComFunc.SptChar(clsPublic.GstrRetValue, 0, "^^") != "OK")
            {
                clsPublic.GstrRetValue = "";
                return;
            }

            int nPage = (int)VB.Val(ComFunc.SptChar(clsPublic.GstrRetValue, 1, "^^"));
            Font font = new Font("바탕체", 9);

            for (int nX = 1; nX <= nPage; nX++)
            {
                #region //Serial_Printer_Print

                ComFunc.ReadSysDate(clsDB.DbCon);
                ssList_Sheet1.Columns[8].Visible = false;
                ssList_Sheet1.Columns[9].Visible = false;
                ssList_Sheet1.Columns[10].Visible = false;
                ssList_Sheet1.Columns[11].Visible = false;
                ssList_Sheet1.Columns[12].Visible = false;
                ssList_Sheet1.Columns[13].Visible = false;
                ssList_Sheet1.Columns[14].Visible = false;
                ssList_Sheet1.Columns[19].Visible = false;

                string strFont1 = "";
                string strHead1 = "";
                string strFont2 = "";
                string strHead2 = "";
                string strFoot = "";
                string strTitle = "";

                strFont1 = "/fn\"바탕체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
                strFont2 = "/fn\"바탕체\" /fz\"9\" /fb0 /fi0 /fu0 /fk0 /fs2";

                if (chkPrtIP.Checked == true)
                {
                    strTitle = "입 원 진 료 비  세 부 산 정  내 역";
                }

                else
                {
                    strTitle = "외 래 진 료 비  세 부 산 정  내 역";
                }

                strHead1 = "/c/f1" + strTitle + "/f1/n/n";
                strHead2 = "/c/f2" + "등록번호: " + txtPano.Text + VB.Space(5) + " 환자성명: " + lblSname.Text + VB.Space(5) + " 진료기간: " + dtpFdate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTdate.Value.ToString("yyyy-MM-dd") + VB.Space(5) + " 진료과: " + cboDept.Text + VB.Space(5) + " 환자구분" + cboBi.Text + VB.Space(10) + " 비고" + "/f2/n";

                strFoot = "/n";
                //strFoot = SPR.setSpdPrint_String(ComFunc.SptChar(clsPublic.GstrRetValue, 2, "^^") + "(   " + ComFunc.SptChar(clsPublic.GstrRetValue, 3, "^^") + ") 의 요청에 따라 진료비 계산서 영수증 세부산정내역을 발급합니다.", font, clsSpread.enmSpdHAlign.Center, false, true);

                if (ComFunc.SptChar(clsPublic.GstrRetValue, 2, "^^") == "본인")
                {
                    strFoot = SPR.setSpdPrint_String("(본인) 의 요청에 따라 진료비  계산서·영수증 세부산정내역을 발급합니다.", font, clsSpread.enmSpdHAlign.Center, false, true);
                }
                else
                {
                    strFoot = SPR.setSpdPrint_String("(" + ComFunc.SptChar(clsPublic.GstrRetValue, 2, "^^") + ") 의 요청에 따라 진료비 계산서 영수증 세부산정내역을 발급합니다.", font, clsSpread.enmSpdHAlign.Center, false, true);
                }                
                
                strFoot += SPR.setSpdPrint_String(VB.Left(clsPublic.GstrSysDate, 4) + "년  " + ComFunc.SptChar(clsPublic.GstrSysDate, 1, "-") + "월  " + VB.Right(clsPublic.GstrSysDate, 2) + "일  ", font, clsSpread.enmSpdHAlign.Center, false, true);
                strFoot += SPR.setSpdPrint_String(VB.Space(10) + "요양기관명칭  포항성모병원" + VB.Space(90) + "대표자  최 순 호 ", font, clsSpread.enmSpdHAlign.Left, false, true);
                strFoot += SPR.setSpdPrint_String("PAGE : " + "/p", font, clsSpread.enmSpdHAlign.Center, false, true);
                strFoot += SPR.setSpdPrint_String(VB.Space(10) + "** 본원은 종합병원으로서 종별가산율은 의료급여 18%, 건강보험 25% 입니다. **", font, clsSpread.enmSpdHAlign.Left, false, true);

                if (chkJiwon.Checked == true)
                {
                    //strFoot += VB.Space(2) + "*  진찰료에 의료질평가 지원금이 포함되어 있습니다. **" + "/n";
                    //strFoot += VB.Space(2) + "*  AU214  AU233  AU413 의료질평가지원금 - 의료질, 안전 3등급(외래)   2,150원 *" + "/n";
                    //strFoot += VB.Space(2) + "*  AU214  AU233  AU413 의료질평가지원금 - 교육수련 3등급(외래)          60원 *" + "/n";
                    //strFoot += VB.Space(2) + "*  AU312  AU313  AU413 의료질평가지원금 - 연구개발분야 3등급(외래)    70원 *";

                    //2018-12-14 변경함
                    strFoot += VB.Space(2) + "*  진찰료에 의료질평가 지원금이 포함되어 있습니다. **" + "/n";
                    strFoot += VB.Space(2) + "*  AU233 의료질평가지원금 - 의료질, 안전 3등급(외래)   2,260원 *" + "/n";
                    strFoot += VB.Space(2) + "*  AU313 의료질평가지원금 - 교육수련 3등급(외래)          60원 *" + "/n";
                    strFoot += VB.Space(2) + "*  AU413 연구개발분야지원금 - 교육수련 3등급(외래)   70원 *";
                }

                ssList_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
                //ssList_Sheet1.PrintInfo.Printer = "\\\\192.168.30.50\\주사집계";
                
                ssList_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
                ssList_Sheet1.PrintInfo.Footer = strFont2 + strFoot;

                ssList_Sheet1.PrintInfo.Margin.Top = 20;
                ssList_Sheet1.PrintInfo.Margin.Bottom = 20;
                ssList_Sheet1.PrintInfo.Margin.Left = 60;
                ssList_Sheet1.PrintInfo.Margin.Right = 60;

                ssList_Sheet1.PrintInfo.Margin.Header = 20;
                ssList_Sheet1.PrintInfo.Margin.Bottom = 20;

                ssList_Sheet1.PrintInfo.ShowColor = true;
                ssList_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                ssList_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                ssList_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ssList_Sheet1.PrintInfo.ShowBorder = true;
                ssList_Sheet1.PrintInfo.ShowGrid = false;
                ssList_Sheet1.PrintInfo.ShowShadows = false;
                ssList_Sheet1.PrintInfo.UseMax = true;
                ssList_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                ssList_Sheet1.PrintInfo.UseSmartPrint = false;
                ssList_Sheet1.PrintInfo.ShowPrintDialog = false;
                ssList_Sheet1.PrintInfo.Preview = chkPrePrint.Checked;
                ssList.PrintSheet(0);

                ComFunc.Delay(6000);

                ssList_Sheet1.Columns[8].Visible = true;
                ssList_Sheet1.Columns[9].Visible = true;
                ssList_Sheet1.Columns[10].Visible = true;
                ssList_Sheet1.Columns[11].Visible = true;
                ssList_Sheet1.Columns[12].Visible = true;
                ssList_Sheet1.Columns[13].Visible = true;
                ssList_Sheet1.Columns[14].Visible = true;

                #endregion //Serial_Printer_Print
            }

            clsPublic.GstrRetValue = "";

        }

 

    

        private void dtpTdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.dtpTdate && e.KeyChar == (char)13)
            {
                btnSearch.Focus();
            }
        }

        private void dtpFdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.dtpFdate && e.KeyChar == (char)13)
            {
                dtpTdate.Focus();
            }
        }

        private void cboDept_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.cboDept && e.KeyChar == (char)13)
            {
                dtpFdate.Focus();
            }
        }
    }
}
