using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{

    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-10-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\IPD\iusent\iusent.vbp\Frm재원진료비영수증_NEW2.frm" >> frmPmpaViewLastSpecifications.cs 폼이름 재정의" />

    public partial class frmPmpaViewLastSpecifications : Form
    {

        string GstrHelpCode = "";
        double GnDrgBonAmt = 0;
        double GnDrgBiTAmt = 0;
        double[] GnDrgFoodAmt = new double[6];
        double[] GnDrgRoomAmt = new double[6];
        double GnGs100Amt = 0;
        double GnGs80Amt_B = 0;
        double GnGs50Amt_B = 0;

        string SQL = "";
        string[] strBi = new string[3];
        string strSname = "";
        string strPano = "";

        #region 글로벌
        string[] strIndates = new string[10];
        string[] strSexAge = new string[10];
        string[] strWardRoom = new string[10];
        string[] strAmset1 = new string[10];
        string[] StrGbGamek = new string[10];
        string[] strBohun = new string[10];
        string[] strRowIDs = new string[10];
        double GnDrg급여총액 = 0;
        double GnDrg열외군금액 = 0;
        double GnDrg열외군금액_Bon = 0;
        double GnDRG_TAmt = 0;
        double GnDRG_Amt1 = 0;
        double GnGsAddAmt = 0;
        double Gn복강개복Amt = 0;
        double GnDrgSelTAmt = 0;
        double Gn항결핵약제비 = 0;
        string GstrOgAdd = "";
        double GnDRG_WBonAmt = 0;
        double GnDrg추가입원료 = 0;
        double GnDrg추가입원료_Bon = 0;
        double GnDrgJinAmt = 0;
        double GnDrgJinAmt_Bon = 0;
        double GnDrg부수술총액 = 0;
        double GnDrg부수술총액_Bon = 0;
        double Gn응급가산수가 = 0;
        double Gn응급가산수가_Bon;
        double Gn재왕절개수가 = 0;
        double Gn재왕절개수가_Bon = 0;
        double GnDrgSono = 0;
        double douGnDrgSono = 0;
        double GnDrgSono_Bon = 0;
        double GnDrg간호간병료 = 0;
        double GnDrg간호간병료_Bon = 0;
        double GnGs80Amt_T = 0;
        double GnGs50Amt_T = 0;
        double GnDrgBiFAmt = 0;
        double Gn행위별총액 = 0;

        double GnDRG_Amt2 = 0;// 'DRG 점수별 금액*단가

        #endregion


        double[] nAmAMT = new double[61];     //'Master 금액 Setting
        double nTot1 = 0;
        double nTot2 = 0;
        double nBON1 = 0;
        double nBON2 = 0;
        double nJOB = 0;
        double nBON = 0;
        double nGAM = 0;
        double nGAMex = 0;
        double nCT55 = 0;
        double nCT45 = 0;
        double nCT50 = 0;
        double FnTRSNO = 0;
        double FnBoho_IPDNO = 0;
        double FnIPDNO = 0;
        string Fstr임시자격 = "";   //  '임시자격선택
        string FstrInDate = "";   //  '2012-09-07
        string FstrOutDate = "";//2-09-07




        bool FbTodayTewon = false;

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        DRG DR = new DRG();
        clsIuSentChk CIC = new clsIuSentChk();

        string strdtP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewLastSpecifications()
        {
            InitializeComponent();
        }

        private void frmPmpaViewLastSpecifications_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;

            txtPano.Text = "";
            //Call CardVariable_Clear

            Screen_clear3();

            //'헤더병합
            SSAmt_Sheet1.Cells[0, 0].ColumnSpan = 2;
            SSAmt_Sheet1.Cells[1, 0].ColumnSpan = 2;

            for (i = 5; i <= 22; i++)
            {
                if (i != 21)
                {

                }
            }
        }

        #region 함수 모음

        private void SCREEN_CLEAR()
        {
            int i = 0;

            Fstr임시자격 = "";

            SS1_Sheet1.RowCount = 0;
            SSTrans_Sheet1.RowCount = 20;

            for (i = 0; i <= SSAmt_Sheet1.RowCount; i++)
            {
                SSAmt_Sheet1.Cells[i, 3 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 4 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 5 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 6 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 7 - 1].Text = "";

                SSAmt_Sheet1.Cells[i, 9 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 10 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 11 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 12 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 13 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 15 - 1].Text = "";
            }

            //'2016-11-29
            for (i = 1; i <= SSDrgAmt_Sheet1.RowCount; i++)
            {
                SSDrgAmt_Sheet1.Cells[i - 1, 1].Text = "";
            }

            SSDrgAmt_Sheet1.Cells[18 - 1, 2].Text = "◆중간납◆";
            SSDrgAmt_Sheet1.Cells[18 - 1, 0].Text = "";

            FbTodayTewon = false;
        }

        private void SCREEN_CLEAR2()
        {
            int i = 0;

            SS1_Sheet1.RowCount = 0;
            SSTrans_Sheet1.RowCount = 20;

            for (i = 0; i <= SSAmt_Sheet1.RowCount; i++)
            {
                SSAmt_Sheet1.Cells[i, 3 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 4 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 5 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 6 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 7 - 1].Text = "";

                SSAmt_Sheet1.Cells[i, 9 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 10 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 11 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 12 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 13 - 1].Text = "";
                SSAmt_Sheet1.Cells[i, 15 - 1].Text = "";
            }

            //'2016-11-29
            for (i = 1; i <= SSDrgAmt_Sheet1.RowCount; i++)
            {
                SSDrgAmt_Sheet1.Cells[i - 1, 1].Text = "";
            }

            SSDrgAmt_Sheet1.Cells[18 - 1, 2].Text = "◆중간납◆";
            SSDrgAmt_Sheet1.Cells[18 - 1, 0].Text = "";
        }

        private void Screen_clear3()
        {
            int i = 0;

            txtDrgWAmt.Text = "";// '2013-06-26
            txtGsAmt.Text = "";// '2013-06-26
            txt100Amt.Text = "";// '2013-06-26
            txtSelAmt.Text = "";// '2013-06-26
            txtDrgTAmt.Text = "";// '2013-06-26
            txtGuTAmt.Text = "";// '2013-06-26
            txtBiGuTAmt.Text = "";// '2013-06-26
            txtBonAmt.Text = "";// '2013-06-26
            txtJohapAmt.Text = "";// '2013-06-26
            Txt이미납부.Text = "";// '2013-06-26
            txtHalin.Text = "";// '2013-06-26
            Txt지원.Text = "";// '2013-06-26
            txtSunap.Text = "";// '2013-06-26

            rdoOG1.Checked = true;

            for (i = 1; i <= SSAmt_Sheet1.RowCount; i++)
            {
                SSDrg_Sheet1.Cells[i - 1, 0].Text = "";
                SSDrg_Sheet1.Cells[i - 1, 1].Text = "";
                SSDrg_Sheet1.Cells[i - 1, 2].Text = "";
                SSDrg_Sheet1.Cells[i - 1, 3].Text = "";
                SSDrg_Sheet1.Cells[i - 1, 4].Text = "";
                SSDrg_Sheet1.Cells[i - 1, 5].Text = "";
            }
        }


        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            int nRead = 0;
            string strList = "";
            double nIPDNO = 0;
            double nTRSNo = 0;
            DataTable dt = null;
            DataTable dtFc = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strJumin = "";
            string strRemark = "";


            txtPano.Text = txtPano.Text.Trim();
            clsPmpaType.TIT.TROWID = "";    //2012-09-07

            SCREEN_CLEAR();
            Screen_clear3();

            if (rdoJob0.Checked == true)
            {
                if (txtPano.Text != "")
                {
                    txtPano.Text = VB.Format(Convert.ToInt32(txtPano.Text), "00000000");
                }
            }

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            try
            {
                SQL = "";

                SQL = " SELECT A.PANO, B.SNAME, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE,  ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE,                  ";
                SQL = SQL + ComNum.VBLF + " a.ILSU, A.BI, A.DEPTCODE, A.DRCODE, A.GBIPD, A.SANGAMT,   ";
                SQL = SQL + ComNum.VBLF + " A.OGPDBUN,A.OGPDBUNdtl, A.AMSET3,b.Secret,                ";
                SQL = SQL + ComNum.VBLF + " B.ROOMCODE, B.WARDCODE, A.VCODE, A.IPDNO, A.TRSNO,        ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, B.AGE,a.GbSPC,   ";
                SQL = SQL + ComNum.VBLF + " A.DRGCODE,A.GbDrg,A.GbSTS,A.FCODE,A.BOHUN                 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1 ";
                SQL = SQL + ComNum.VBLF + "   AND A.GBSTS <> '7'                                      ";

                if (rdoJob0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND B.PANO  = '" + txtPano.Text.Trim() + "' ";
                }
                else if (rdoJob1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  AND B.SNAME like = '" + txtPano.Text.Trim() + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.OUTDATE = TO_DATE('" + dtpFdate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                }

                SQL = SQL + ComNum.VBLF + "  AND A.ACTDATE IS NUL ";
                SQL = SQL + ComNum.VBLF + "  AND A.IPDNO = B.IPDNO ";
                SQL = SQL + ComNum.VBLF + "  AND A.GBIPD <> 'D'";  //'삭제는 제외

                SQL = SQL + ComNum.VBLF + "ORDER BY PANO,INDATE DESC, TRSNO   ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                nRead = dt.Rows.Count;

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //스프레드 출력문
                SSTrans_Sheet1.Rows.Count = 0;
                SSTrans_Sheet1.Rows.Count = nRead;
                SSTrans_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRead; i++)
                {
                    SSTrans_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    SSTrans_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    SSTrans_Sheet1.Cells[i, 2].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    SSTrans_Sheet1.Cells[i, 3].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                    SSTrans_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Ilsu"].ToString().Trim();

                    if (dt.Rows[i]["GBDRG"].ToString().Trim() == "D")
                    {// 'DRG환자면 일수를 다시 계산함(기존입원 일수 무시함)
                        if (dt.Rows[i]["OUTDATE"].ToString().Trim() == "")
                        {
                            SSTrans_Sheet1.Cells[i, 4].Text = CF.DATE_ILSU(clsDB.DbCon, strdtP, dt.Rows[i]["InDate"].ToString().Trim()) + 1.ToString();
                        }
                        else if (string.Compare(dt.Rows[i]["OutDate"].ToString().Trim(), strdtP) != 0)
                        {
                            SSTrans_Sheet1.Cells[i, 4].Text = CF.DATE_ILSU(clsDB.DbCon, dt.Rows[i]["OutDate"].ToString().Trim(), dt.Rows[i]["InDate"].ToString().Trim()) + 1.ToString();
                        }
                        else
                        {
                            SSTrans_Sheet1.Cells[i, 4].Text = CF.DATE_ILSU(clsDB.DbCon, strdtP, dt.Rows[i]["InDate"].ToString().Trim()) + 1.ToString();
                        }
                    }
                    else
                    {
                        SSTrans_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Ilsu"].ToString().Trim();
                    }

                    SSTrans_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    SSTrans_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    SSTrans_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    SSTrans_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DeptCode"].ToString().Trim();

                    // '2016-04-06 장애구분 추가
                    if (dt.Rows[i]["Bi"].ToString().Trim() == "22")
                    {
                        SSTrans_Sheet1.Cells[i, 9].Text = dt.Rows[i]["Bohun"].ToString().Trim() == "3" ? "장애" : "";
                    }
                    SSTrans_Sheet1.Cells[i, 10].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                    SSTrans_Sheet1.Cells[i, 11].Text = "";

                    if (dt.Rows[i]["GbIPD"].ToString().Trim() == "9")
                    {
                        SSTrans_Sheet1.Cells[i, 11].Text = "지병";
                    }
                    SSTrans_Sheet1.Cells[i, 12].Text = "";

                    if (CIC.Chk_Pre_SangHan(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim(), dt.Rows[i]["InDate"].ToString().Trim(), Convert.ToInt64(dt.Rows[i]["IPDNO"].ToString().Trim())).ToString() == "OK")
                        ;
                    {
                        SSTrans_Sheet1.Cells[i, 12].Text = "이전";
                    }
                    if (VB.Val(dt.Rows[i]["SangAmt"].ToString().Trim()) > 0)
                    {
                        SSTrans_Sheet1.Cells[i, 12].Text = "상한";
                    }
                    SSTrans_Sheet1.Cells[i, 13].Text = dt.Rows[i]["OgPdBun"].ToString().Trim();
                    SSTrans_Sheet1.Cells[i, 14].Text = "";

                    //'If Trim(AdoGetString(AdoRes, "VCODE", I)) <> "" Then SSTrans.Text = "중증"
                    //'V268 뇌출혈추가

                    if (dt.Rows[i]["VCODE"].ToString().Trim() == "V191" || dt.Rows[i]["VCODE"].ToString().Trim() == "V192" || dt.Rows[i]["VCODE"].ToString().Trim() == "V193" || dt.Rows[i]["VCODE"].ToString().Trim() == "V194" || dt.Rows[i]["VCODE"].ToString().Trim() == "V268" || dt.Rows[i]["VCODE"].ToString().Trim() == "V275")
                    {
                        SSTrans_Sheet1.Cells[i, 14].Text = "중증";
                    }

                    if (dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                    {
                        SSTrans_Sheet1.Cells[i, 14].Text = "중증";
                    }
                    SSTrans_Sheet1.Cells[i, 15].Text = "";

                    if (dt.Rows[i]["AmSet3"].ToString().Trim() == "9")
                    {
                        SSTrans_Sheet1.Cells[i, 15].Text = "완료";
                    }

                    SSTrans_Sheet1.Cells[i, 16].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                    nIPDNO = Convert.ToDouble(dt.Rows[i]["IPDNO"].ToString().Trim());
                    nTRSNo = Convert.ToDouble(dt.Rows[i]["TRSNO"].ToString().Trim());
                    SSTrans_Sheet1.Cells[i, 17].Text = nIPDNO.ToString();
                    SSTrans_Sheet1.Cells[i, 18].Text = nTRSNo.ToString();
                    SSTrans_Sheet1.Cells[i, 19].Text = "";

                    if (dt.Rows[i]["GbSPC"].ToString().Trim() == "1")
                    {
                        SSTrans_Sheet1.Cells[i, 19].Text = "Y";
                    }
                    SSTrans_Sheet1.Cells[i, 20].Text = dt.Rows[i]["OgPdBundtl"].ToString().Trim();

                    if (dt.Rows[i]["Secret"].ToString().Trim() != "")
                    {
                        ComFunc.MsgBox("사생활보호 대상요청자 입니다. 안내 시 주의하십시오.", "안내주의");
                    }
                    SSTrans_Sheet1.Cells[i, 21].Text = dt.Rows[i]["DrgCode"].ToString().Trim();
                    SSTrans_Sheet1.Cells[i, 22].Text = dt.Rows[i]["GBDrg"].ToString().Trim();

                    if (SSTrans_Sheet1.Cells[i, 22].Text == "D")
                    {
                        SSTrans_Sheet1.Cells[i, 22].BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
                    }
                    else
                    {
                        SSTrans_Sheet1.Cells[i, 22].BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    }

                    SSTrans_Sheet1.Cells[i, 23].Text = "";

                    if (dt.Rows[i]["GbIPD"].ToString().Trim() == "9")
                    {
                        SSTrans_Sheet1.Cells[i, 23].Text = "지병" + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "P" || dt.Rows[i]["OGPDBUN"].ToString().Trim() == "O")
                    {
                        // '2015-05-18 입원명령결핵 추가
                        if (dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                        {
                            SSTrans_Sheet1.Cells[i, 23].Text = dt.Rows[i]["VCODE"].ToString().Trim() + "+F008";
                        }
                        else if (dt.Rows[i]["VCODE"].ToString().Trim() == "F010")
                        {
                            //'2015-06-30
                            SSTrans_Sheet1.Cells[i, 23].Text = dt.Rows[i]["VCODE"].ToString().Trim() + "+F010";
                        }
                        else
                        {
                            SSTrans_Sheet1.Cells[i, 23].Text = "면제";
                            SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);

                            if (string.Compare(dt.Rows[i]["InDate"].ToString().Trim(), "11/04/01") >= 0 && dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                            {
                                SSTrans_Sheet1.Cells[i, 23].Text = " ★결핵★";
                            }
                        }
                    }
                    else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E")
                    {
                        SSTrans_Sheet1.Cells[i, 23].Text = "중증E+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F")
                    {
                        SSTrans_Sheet1.Cells[i, 23].Text = "중증F+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "1")
                    {
                        //'2013-02-15
                        SSTrans_Sheet1.Cells[i, 23].Text = "E+V";
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "2")
                    {
                        //'2013-02-15
                        SSTrans_Sheet1.Cells[i, 23].Text = "F+V";
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "1")
                    {
                        SSTrans_Sheet1.Cells[i, 23].Text = "중증E+V+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "2")
                    {
                        SSTrans_Sheet1.Cells[i, 23].Text = "중증F+V+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    //'V268 뇌출혈추가
                    else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V191" || dt.Rows[i]["VCODE"].ToString().Trim() == "V192" || dt.Rows[i]["VCODE"].ToString().Trim() == "V193" || dt.Rows[i]["VCODE"].ToString().Trim() == "V194" || dt.Rows[i]["VCODE"].ToString().Trim() == "V268" || dt.Rows[i]["VCODE"].ToString().Trim() == "V275")
                    {
                        SSTrans_Sheet1.Cells[i, 23].Text = "중증+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (string.Compare(dt.Rows[i]["InDate"].ToString().Trim(), "11/04/01") >= 0 && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E" && dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                    {
                        //'2015-05-18 입원명령결핵 추가
                        if (dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                        {
                            SSTrans_Sheet1.Cells[i, 23].Text = "차상위E(" + dt.Rows[i]["VCODE"].ToString().Trim() + "+F008)";
                        }
                        else
                        {
                            SSTrans_Sheet1.Cells[i, 23].Text = "차상위E+★결핵★";
                            SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                        }
                    }

                    else if (string.Compare(dt.Rows[i]["InDate"].ToString().Trim(), "11/04/01") >= 0 && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F" && dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                    {
                        //'2015-05-18 입원명령결핵 추가
                        if (dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                        {
                            SSTrans_Sheet1.Cells[i, 23].Text = "차상위F(" + dt.Rows[i]["VCODE"].ToString().Trim() + "+F008)";
                        }
                        else
                        {
                            SSTrans_Sheet1.Cells[i, 23].Text = "차상위F+★결핵★";
                            SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                        }

                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E" && dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                    {
                        SSTrans_Sheet1.Cells[i, 23].Text = "중증화상E+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F" && dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                    {
                        SSTrans_Sheet1.Cells[i, 23].Text = "중증화상F+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                    {
                        SSTrans_Sheet1.Cells[i, 23].Text = "중증화상+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "H")
                    {
                        SSTrans_Sheet1.Cells[i, 23].Text = "희귀H";
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "V")
                    {
                        SSTrans_Sheet1.Cells[i, 23].Text = "희귀H";
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);

                        if (dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                        {
                            SSTrans_Sheet1.Cells[i, 23].Text = dt.Rows[i]["VCODE"].ToString().Trim() + "+F008";
                        }
                        else
                        {
                            SSTrans_Sheet1.Cells[i, 23].Text = "희귀V";
                            SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);

                            if (string.Compare(dt.Rows[i]["InDate"].ToString().Trim(), "11/04/01") >= 0 && dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                            {
                                SSTrans_Sheet1.Cells[i, 23].Text = " ★결핵★";
                            }
                        }

                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "C")
                    {
                        SSTrans_Sheet1.Cells[i, 23].Text = "차상";
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E")
                    {
                        if (dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() != "")
                        {
                            SSTrans_Sheet1.Cells[i, 23].Text = "차상E+" + dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else
                        {
                            SSTrans_Sheet1.Cells[i, 23].Text = "차상E" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F")
                    {
                        if (dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() != "")
                        {
                            SSTrans_Sheet1.Cells[i, 23].Text = "차상F+" + dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else
                        {
                            SSTrans_Sheet1.Cells[i, 23].Text = "차상F" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    else if (string.Compare(dt.Rows[i]["InDate"].ToString().Trim(), "16/07/01") >= 0 && dt.Rows[i]["FCODE"].ToString().Trim() == "F014")
                    {
                        SSTrans_Sheet1.Cells[i, 23].Text = "F014";
                        SSTrans_Sheet1.Cells[i, 23].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                }
                dt.Dispose();
                dt = null;

                if (nRead == 1)
                {
                    dtpTempFdate.Value = Convert.ToDateTime(dt.Rows[0]["Indate"].ToString().Trim());

                    if (dt.Rows[0]["OUTDATE"].ToString().Trim() == "")
                    {
                        dtpTempTdate.Value = Convert.ToDateTime(dt.Rows[0]["OutDate"].ToString().Trim());
                    }

                    //TODO
                    //IPD_TRANS_Amt_Display_NEW2(SSAmt, 0, FnTRSNO, "")
                    if (Convert.ToInt32(dt.Rows[0]["AGE"].ToString().Trim()) <= 6)
                    {
                        // '입원기간동안에 6세이상인경우 체크함.
                        SQL = " SELECT JUMIN1,JUMIN2, JUMIN3 ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + dt.Rows[0]["PANO"].ToString().Trim() + "' ";

                        if (dtFc.Rows.Count > 0)
                        {
                            //'주민암호화
                            if (dtFc.Rows[0]["JUMIN3"].ToString().Trim() != "")
                            {
                                strJumin = dtFc.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                            }
                            else
                            {
                                strJumin = dtFc.Rows[0]["JUMIN1"].ToString().Trim() + dt.Rows[0]["JUMIN2"].ToString().Trim();
                            }
                        }
                        dtFc.Dispose();
                        dtFc = null;

                        strRemark = "";
                        //TODO
                        //strRemark = AGE_PD_GESAN(strJumin, Trim(AdoGetString(AdoRes, "InDate", 0)), GstrSysDate)
                        if (strRemark != "")
                        {
                            ComFunc.MsgBox(strRemark, "확인");
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnTempSet_Click(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strInDate = "";
            string strOutDate = "";
            DataTable dt = null;
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (clsPmpaType.TIT.Trsno == 0)
                {
                    ComFunc.MsgBox("선택후 작업하세요");
                    return;
                }

                if (ComFunc.MsgBoxQ("설정한 기간으로 임시자격을 생성하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                strInDate = dtpTempFdate.Value.ToString("yyyy-MM-dd");
                strOutDate = dtpTempTdate.Value.ToString("yyyy-MM-dd");


                SQL = "";
                SQL = "INSERT INTO  WORK_IPD_TRANS_TERM (";
                SQL = SQL + ComNum.VBLF + "       TrsNo,IPDNO,PANO,GBIPD,INDATE,OUTDATE,ActDate,DEPTCODE,DRCODE,ILSU,BI,KIHO,";
                SQL = SQL + ComNum.VBLF + "       GKIHO,PNAME,GWANGE,BONRATE,GISULRATE,GBGAMEK,BOHUN,AMSET1,AMSET2,AMSET3,AMSET4,";
                SQL = SQL + ComNum.VBLF + "       AMSET5,AMSETB,FROMTRANS,ERAMT,REMARK,JUPBONO,GBDRG,DRGWRTNO,SANGAMT,dtGAMEK,";
                SQL = SQL + ComNum.VBLF + "       OGPDBUN,OGPDBUNdtl,ENTDATE,ENTSABUN,GBSTS,GELCODE,Gbilban2,GbSPC) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = SQL + ComNum.VBLF + " SELECT TrsNo,IPDNO,PANO,GBIPD,TO_DATE('" + strInDate + "','YYYY-MM-DD') ,TO_DATE('" + strOutDate + "','YYYY-MM-DD'),ActDate,DEPTCODE,DRCODE,ILSU,BI,KIHO,";
                SQL = SQL + ComNum.VBLF + "       GKIHO,PNAME,GWANGE,BONRATE,GISULRATE,GBGAMEK,BOHUN,AMSET1,AMSET2,AMSET3,AMSET4,";
                SQL = SQL + ComNum.VBLF + "       AMSET5,AMSETB,FROMTRANS,ERAMT,REMARK,JUPBONO,GBDRG,DRGWRTNO,SANGAMT,dtGAMEK,";
                SQL = SQL + ComNum.VBLF + "       OGPDBUN,OGPDBUNdtl,ENTDATE,ENTSABUN,GBSTS,GELCODE,Gbilban2,GbSPC ";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.IPD_TRANS ";
                SQL = SQL + ComNum.VBLF + "     WHERE TRSNO =" + clsPmpaType.TIT.Trsno + " ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("임시자격 생성 완료.", "확인");
                Cursor.Current = Cursors.Default;

                return;
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnTView_Click(object sender, EventArgs e)
        {
            CmdtView_Click();
        }

        private void CmdtView_Click()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strList = "";
            double nIPDNO = 0;
            double nTRSNO = 0;
            string strJumin = "";
            string strRemark = "";

            Cursor.Current = Cursors.WaitCursor;
            SCREEN_CLEAR2();

            txtPano.Text = txtPano.Text.Trim();
            clsPmpaType.TIT.TROWID = "";

            if (txtPano.Text == "")
            {
                return;
            }
            try
            {
                SQL = "";
                SQL = " SELECT A.PANO, B.SNAME, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE,  ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE,A.ROWID,          ";
                SQL = SQL + ComNum.VBLF + " a.ILSU, A.BI, A.DEPTCODE, A.DRCODE, A.GBIPD, A.SANGAMT,   ";
                SQL = SQL + ComNum.VBLF + " A.OGPDBUN,A.OGPDBUNdtl, A.AMSET3,                         ";
                SQL = SQL + ComNum.VBLF + " B.ROOMCODE, B.WARDCODE, A.VCODE, A.IPDNO, A.TRSNO,        ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, B.AGE,a.GbSPC    ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "WORK_IPD_TRANS_TERM A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B  ";
                SQL = SQL + ComNum.VBLF + " WHERE A.GBSTS <> '7'                                      ";
                SQL = SQL + ComNum.VBLF + "  AND B.PANO  = '" + txtPano.Text + "'           ";
                SQL = SQL + ComNum.VBLF + "  AND A.ACTDATE IS NULL                                    ";
                SQL = SQL + ComNum.VBLF + "  AND A.IPDNO = B.IPDNO                                    ";
                SQL = SQL + ComNum.VBLF + "  AND A.GBIPD <> 'D'                                       ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PANO,INDATE DESC,OUTDATE DESC, TRSNO                                      ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //스프레드 출력문
                SS1_Sheet1.RowCount = dt.Rows.Count;
                SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SS1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Ilsu"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 9].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                    SS1_Sheet1.Cells[i, 10].Text = "";

                    if (dt.Rows[i]["GbIPD"].ToString().Trim() == "9")
                    {
                        SS1_Sheet1.Cells[i, 10].Text = "지병";
                    }
                    SS1_Sheet1.Cells[i, 12].Text = "";

                    if (VB.Val(dt.Rows[i]["SangAmt"].ToString().Trim()) > 0)
                    {
                        SS1_Sheet1.Cells[i, 12].Text = "상한";
                    }
                    SS1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["OgPdBun"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 14].Text = "";

                    if (dt.Rows[i]["VCODE"].ToString().Trim() == "V191" || dt.Rows[i]["VCODE"].ToString().Trim() == "V192" || dt.Rows[i]["VCODE"].ToString().Trim() == "V193" || dt.Rows[i]["VCODE"].ToString().Trim() == "V194" || dt.Rows[i]["VCODE"].ToString().Trim() == "V268" || dt.Rows[i]["VCODE"].ToString().Trim() == "V275")
                    {
                        SS1_Sheet1.Cells[i, 14].Text = "중증";
                    }

                    SS1_Sheet1.Cells[i, 14].Text = "";

                    if (dt.Rows[i]["AmSet3"].ToString().Trim() == "9")
                    {
                        SS1_Sheet1.Cells[i, 15].Text = "완료";
                    }

                    FnIPDNO = Convert.ToDouble(dt.Rows[i]["IPDNO"].ToString().Trim());
                    FnTRSNO = Convert.ToDouble(dt.Rows[i]["TRSNO"].ToString().Trim());

                    SS1_Sheet1.Cells[i, 16].Text = FnIPDNO.ToString();
                    SS1_Sheet1.Cells[i, 17].Text = FnTRSNO.ToString();
                    SS1_Sheet1.Cells[i, 18].Text = "";

                    if (dt.Rows[i]["GbSPC"].ToString().Trim() == "1")
                    {
                        SS1_Sheet1.Cells[i, 18].Text = "Y";
                    }
                    SS1_Sheet1.Cells[i, 19].Text = dt.Rows[i]["OgPdBundtl"].ToString().Trim();
                    SS1_Sheet1.Cells[i, 21].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void SS1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            //TODO
            //Set Printer = Printers (GnPrtIpdNo2) '영수증
            //Call WORK_READ_IPD_TRANS(FnTRSNO)

            //If Gstr누적계산New = "OK" Then
            //    Call IPD_TRANS_PRTAmt_READ_NEW (FnTRSNO , "임시자격")
            //Else
            //    Call IPD_TRANS_PRTAmt_READ (FnTRSNO , "임시자격")
            //End If


            //Call Ipd_Tewon_PrtAmt_Gesan(FnTRSNO , "임시자격")
            //Call Tewon_Print_Process_2012(FnIPDNO , FnTRSNO , "임시자격" , Picture_Sign)
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            double nIPDNO = 0;
            double nTRSNO = 0;
            string strPano = "";
            string strJumin = "";
            string strRemark = "";
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);


            try
            {
                if (e.Row == 0)
                {
                    return;
                }

                if (e.Column == 0)
                {
                    if (ComFunc.MsgBoxQ("임시자격을 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        clsPmpaType.TIT.TROWID = "";
                        clsPmpaType.TIT.TROWID = SS1_Sheet1.Cells[e.Row, 21].Text;

                    }

                    SQL = " DELETE " + ComNum.DB_PMPA + "WORK_IPD_TRANS_TERM WHERE ROWID ='" + clsPmpaType.TIT.TROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsPmpaType.TIT.TROWID = "";
                    clsDB.setCommitTran(clsDB.DbCon);
                    CmdtView_Click();
                    Cursor.Current = Cursors.Default;
                    return;
                }

                Fstr임시자격 = "OK";

                clsPmpaType.TIT.TROWID = "";

                strPano = SS1_Sheet1.Cells[e.Row, 0].Text;
                FstrInDate = SS1_Sheet1.Cells[e.Row, 2].Text;
                FstrOutDate = SS1_Sheet1.Cells[e.Row, 3].Text;
                FnIPDNO = VB.Val(SS1_Sheet1.Cells[e.Row, 16].Text);
                FnTRSNO = VB.Val(SS1_Sheet1.Cells[e.Row, 17].Text);
                clsPmpaType.TIT.TROWID = SS1_Sheet1.Cells[e.Row, 21].Text;


                //'금액을 표시할 Sheet를 Clear
                for (i = 1; i <= SSAmt_Sheet1.RowCount; i++)
                {
                    SSAmt_Sheet1.Cells[i, 2].Text = "";
                    SSAmt_Sheet1.Cells[i, 3].Text = "";
                    SSAmt_Sheet1.Cells[i, 4].Text = "";
                    SSAmt_Sheet1.Cells[i, 5].Text = "";
                    SSAmt_Sheet1.Cells[i, 6].Text = "";
                    SSAmt_Sheet1.Cells[i, 8].Text = "";
                    SSAmt_Sheet1.Cells[i, 9].Text = "";
                    SSAmt_Sheet1.Cells[i, 10].Text = "";
                    SSAmt_Sheet1.Cells[i, 11].Text = "";
                    SSAmt_Sheet1.Cells[i, 12].Text = "";
                    SSAmt_Sheet1.Cells[i, 14].Text = "";
                }
                SSAmt_Sheet1.Cells[17, 2].Text = "◆중간납◆";
                SSAmt_Sheet1.Cells[17, 0].Text = "";

                //Call IPD_TRANS_Amt_Display_NEW2(SSAmt, 0, FnTRSNO, "임시자격")

                SSAmt_Sheet1.Cells[17, 0].Text = "◆중간납◆";

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void SSAmt_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strHang = "";
            string strJOB = "";
            string strHangdtl = "";

            strHang = "00";
            strHangdtl = "00";
            strJOB = "";

            if (e.Row == 2 || e.Column == 8)
            {
                strJOB = "내역";
            }
            else if (e.Column == 4 || e.Column == 9)
            {
                strJOB = "상세내역";
            }
            else
            {
                return;
            }

            if (e.Column <= 3)
            {
                switch (e.Row)
                {
                    case 1:
                        strHang = 1.ToString("00");
                        break;    //'진찰
                    case 2:
                        strHang = 2.ToString("00");
                        break;   //'입원
                    case 3:
                        strHang = 3.ToString("00");
                        break;   //'식대
                    case 4:
                        strHang = 4.ToString("00");
                        strHangdtl = "투약행위";
                        break;   //'투약
                    case 5:
                        strHang = 4.ToString("00");
                        strHangdtl = "투약약품";
                        break;
                    case 6:
                        strHang = 5.ToString("00");
                        strHangdtl = "주사행위";
                        break;   //'주사
                    case 7:
                        strHang = 5.ToString("00");
                        strHangdtl = "주사약품";
                        break;
                    case 8:
                        strHang = 6.ToString("00");
                        break;  //'마취
                    case 9:
                        strHang = 7.ToString("00");
                        break;  //'처치
                    case 10:
                        strHang = 8.ToString("00");
                        break; //'검사
                    case 11:
                        strHang = 9.ToString("00");
                        break; //'영상
                    case 12:
                        strHang = 9.ToString("00");
                        break; //'방사선
                    case 13:
                        strHang = 10.ToString("00");
                        break; //'치료재
                    case 14:
                        strHang = 12.ToString("00");
                        break; //'물리치료
                    case 15:
                        strHang = 13.ToString("00");
                        break; //'정신
                    case 16:
                        strHang = 18.ToString("00");
                        break; //'전혈

                }
            }
            else if (e.Column <= 9)
            {
                switch (e.Row)
                {
                    case 1:
                        strHang = 14.ToString("00");
                        break;    //'진찰
                    case 2:
                        strHang = 15.ToString("00");
                        break;   //'입원
                    case 3:
                        strHang = 16.ToString("00");
                        break;   //'식대
                    case 4:
                        strHang = 17.ToString("00");
                        break;
                    case 6:
                        strHang = 19.ToString("00");
                        break;
                    case 7:
                        strHang = 20.ToString("00");
                        break;
                    case 8:
                        strHang = 21.ToString("00");
                        break;  //'마취
                    case 9:
                        strHang = 22.ToString("00");
                        break;  //'처치
                }
            }

            if (strHang == "")
            {
                return;
            }

            //TODO
            //If Fstr임시자격 = "OK" Then
            //    GstrHelpCode = strJOB & "{}" & strHang & "{}" & IMST.IPDNO & "{}" & FnTRSNO & "{}{}{}" & strHangdtl & "{}" & TIT.TROWID & "{}"
            //Else
            //    GstrHelpCode = strJOB & "{}" & strHang & "{}" & IMST.IPDNO & "{}" & FnTRSNO & "{}{}{}" & strHangdtl & "{}" & "{}"
            //End If


            //Frm진료비상세내역_NEW.Show 1
        }

        private void SSDrg_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column != 0 || e.Row < 0)
            {
                return;
            }
            SSDrg_Sheet1.Cells[0, SSDrg_Sheet1.ColumnCount - 1, 0, SSDrg_Sheet1.ColumnCount - 1].Text = "";
            SSDrg_Sheet1.RowCount = 0;

            if (SSDrg_Sheet1.Cells[e.Row, e.Column].Text == "부수술비용")
            {
                if (string.Compare(clsPmpaType.TIT.InDate, "2015-01-30") >= 0)
                {
                    //TODO
                    // Call DisPlay_IPD_Slipdtl_부수술비용(SSDrgEr, TIT.TRSNO)
                }
            }
            else if (SSDrg_Sheet1.Cells[e.Row, e.Column].Text == "응급가산수가")
            {
                if (string.Compare(clsPmpaType.TIT.InDate, "2016-07-01") >= 0)
                {
                    //TODO
                    // Call DisPlay_IPD_Slipdtl_응급가산수가(SSDrgEr, TIT.TRSNO)
                }
            }

            else if (SSDrg_Sheet1.Cells[e.Row, e.Column].Text == "의료질평가지")
            {
                if (string.Compare(clsPmpaType.TIT.InDate, "2015-09-01") >= 0)
                {
                    //TODO
                    // Call DisPlay_IPD_Slipdtl(SSDrgEr, TIT.TRSNO, "의료질평가지원")
                }
            }

            else if (SSDrg_Sheet1.Cells[e.Row, e.Column].Text == "간호간병료")
            {

                //TODO
                // Call DisPlay_IPD_Slipdtl(SSDrgEr, TIT.TRSNO, "간호간병료")

            }

            else if (SSDrg_Sheet1.Cells[e.Row, e.Column].Text == "급여초음파")
            {
                //TODO
                //Call DisPlay_IPD_Slipdtl(SSDrgEr, TIT.TRSNO, "급여초음파")
            }
        }

        private void SSTrans_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            double nIPDNO = 0;
            double nTRSNO = 0;
            double nJiwonAmt = 0;
            double ntAmt51 = 0;
            double ntAmt52 = 0;
            double nTest = 0;
            double nGubTot = 0;
            string strNgt = "";
            string strPano = "";
            string strJumin = "";
            string strInDate = "";
            string strOutDate = "";
            string strOutDate_Drg = "";
            string strRemark = "";
            string strDrgCode = "";
            string nBoninTAmt = "";
            string strGbDrg = "";
            string SqlErr = "";
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dtRs = null;
            DataTable dt = null;
            DataTable dtTemp = null;

            nBoninTAmt = "";
            nJiwonAmt = 0;
            nTest = 0;
            nGubTot = 0;
            ntAmt51 = 0;
            ntAmt52 = 0;

            SS1_Sheet1.RowCount = 0;
            Fstr임시자격 = "";
            clsPmpaType.TIT.TROWID = "";// '2012-09-07

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                if (e.Row == 0)
                {
                    return;
                }

                strPano = SSTrans_Sheet1.Cells[e.Row, 0].Text;
                strInDate = SSTrans_Sheet1.Cells[e.Row, 2].Text;
                strOutDate = SSTrans_Sheet1.Cells[e.Row, 3].Text;
                strOutDate_Drg = SSTrans_Sheet1.Cells[e.Row, 3].Text;
                FnIPDNO = Convert.ToDouble(SSTrans_Sheet1.Cells[e.Row, 17].Text);
                FnTRSNO = Convert.ToDouble(SSTrans_Sheet1.Cells[e.Row, 18].Text);
                strDrgCode = SSTrans_Sheet1.Cells[e.Row, 21].Text;
                strGbDrg = SSTrans_Sheet1.Cells[e.Row, 22].Text;

                dtpTempFdate.Value = Convert.ToDateTime(strInDate);

                if (strOutDate == "")
                {
                    dtpTempTdate.Value = Convert.ToDateTime(strdtP);
                }
                else
                {
                    dtpTempTdate.Value = Convert.ToDateTime(strOutDate);
                }

                Screen_clear3();
                // '금액을 표시할 Sheet를 Clear
                for (i = 1; i <= SSAmt_Sheet1.RowCount; i++)
                {
                    SSAmt_Sheet1.Cells[i, 2].Text = "";
                    SSAmt_Sheet1.Cells[i, 3].Text = "";
                    SSAmt_Sheet1.Cells[i, 4].Text = "";
                    SSAmt_Sheet1.Cells[i, 5].Text = "";
                    SSAmt_Sheet1.Cells[i, 6].Text = "";
                    SSAmt_Sheet1.Cells[i, 8].Text = "";
                    SSAmt_Sheet1.Cells[i, 9].Text = "";
                    SSAmt_Sheet1.Cells[i, 10].Text = "";
                    SSAmt_Sheet1.Cells[i, 11].Text = "";
                    SSAmt_Sheet1.Cells[i, 12].Text = "";
                    SSAmt_Sheet1.Cells[i, 14].Text = "";
                }
                SSAmt_Sheet1.Cells[17, 2].Text = "◆중간납◆";
                SSAmt_Sheet1.Cells[i, 0].Text = "";

                //Call IPD_TRANS_Amt_Display_NEW2(SSAmt, 0, FnTRSNO, "")

                //'2013-06-19 DRG 금액산정
                if (Convert.ToDateTime(clsPmpaType.TIT.InDate) >= Convert.ToDateTime("2013-07-01") && strDrgCode != "")
                {
                    //퇴원일자가 없으면 현재일자로 세팅
                    if (strOutDate_Drg == "")
                    {
                        strOutDate_Drg = strdtP;
                    }
                    strNgt = DR.Read_GbNgt_DRG(clsDB.DbCon, strPano, (long)FnIPDNO, (long)FnTRSNO);

                    //TODO
                    //if (READ_DRG_AMT_MASTER (strDrgCode, strPano, FnIPDNO, FnTRSNO, strNgt, strInDate, strOutDate_Drg) == "OK")
                    //{

                    //    //Call IPD_TRANS_PRTAmt_READ_DRG(FnTRSNO, "")
                    //    // '본인부담금 = 급여 본인부담금 +  비급여총액 (원단위반올림) add kyo 2017-02-02
                    nBoninTAmt = CF.FIX_N(GnDrgBonAmt + GnDrgBiTAmt + GnDrgFoodAmt[0] + GnDrgRoomAmt[0] + GnGs100Amt + GnGs80Amt_B + GnGs50Amt_B).ToString();


                    //    nBoninTAmt = CF.FIX_N (nBoninTAmt / 10) * 10; //'절사

                    //    //'2015-03-02
                    //    //TODO
                    //    //nGubTot = READ_행위별진료비총액_급여 (FnTRSNO);

                    //    if (nGubTot - GnDrg급여총액 >= 1000000)
                    //    {
                    //        GnDrg열외군금액 = nGubTot - GnDrg급여총액 - 1000000;
                    //        GnDrg열외군금액_Bon = (GnDrg열외군금액 * 20 / 100);
                    //        nBoninTAmt = nBoninTAmt + GnDrg열외군금액_Bon;
                    //        nBoninTAmt =( CF.FIX_N (Convert.ToInt32(nBoninTAmt) / 10) * 10 ).ToString();  //'절사
                    //        GnDRG_TAmt = GnDRG_TAmt + GnDrg열외군금액_Bon;
                    //    }

                    //    //TODO
                    //    // Call IPD_DRG_AMT_DISPLAY(SSAmt, FnIPDNO, FnTRSNO)

                    //    txtDrgWAmt.Text = GnDRG_Amt1.ToString ("###,###,##0");         //'DRG 금액
                    //   txtGsAmt.Text = GnGsAddAmt.ToString ("###,###,##0");           //'외과가산금액
                    //   txt100Amt.Text = GnGs100Amt.ToString ("###,###,##0");          //'인정비급여
                    //   Txt복강개복.Text = Gn복강개복Amt.ToString ("###,###,##0");                         //'복강개복
                    //   txtSelAmt.Text = GnDrgSelTAmt.ToString ("###,###,##0");        //'선택진료합계
                    //   txtDrgTAmt.Text = GnDRG_TAmt.ToString ("###,###,##0");         //'DRG 총액
                    //   txtGuTAmt.Text = (GnDRG_TAmt - GnDrgBiTAmt).ToString ("###,###,##0");        //'DRG 급여총액
                    //   txtBiGuTAmt.Text = GnDrgBiTAmt.ToString ("###,###,##0");         //'DRG 비급여총액
                    //   Txt이미납부.Text = clsPmpaType.TIT.RAmt [28 , 1].ToString ("#,##0 ");        //'이미납부한금액
                    //   txtHalin.Text = clsPmpaType.TIT.RAmt [29 , 1].ToString ("#,##0 ");           //'할인금액
                    //   txtDrg열외군.Text = GnDrg열외군금액_Bon.ToString ("###,###,##0");


                    if (Convert.ToDateTime(clsPmpaType.TIT.InDate) >= Convert.ToDateTime("2011-04-01") && clsPmpaType.TIT.VCode == "V206" || clsPmpaType.TIT.VCode == "V231")
                    {
                        //'2014-06-07 입원명령결핵지원 대상
                        if (clsPmpaType.TIT.VCode == "F008")
                        {
                            Txt지원.Text = (((((Convert.ToDouble(nBoninTAmt) - GnDrgBiTAmt + Gn항결핵약제비) * (100 / 100)) / 10) + 0.5) * 10).ToString();
                            nJiwonAmt = ((((Convert.ToDouble(nBoninTAmt) - GnDrgBiTAmt + Gn항결핵약제비) * (100 / 100)) / 10) + 0.5) * 10;
                        }
                        else
                        {
                            Txt지원.Text = (((((Convert.ToDouble(nBoninTAmt) - GnDrgBiTAmt) * (50 / 100)) / 10) + 0.5) * 10).ToString();
                            nJiwonAmt = ((((Convert.ToDouble(nBoninTAmt) - GnDrgBiTAmt) * (50 / 100)) / 10) + 0.5) * 10;
                        }
                    }
                    else if (Convert.ToDateTime(clsPmpaType.TIT.InDate) >= Convert.ToDateTime("2015-07-01") && clsPmpaType.TIT.FCode == "F010")
                    {
                        Txt지원.Text = (((((Convert.ToDouble(nBoninTAmt) - GnDrgBiTAmt + Gn항결핵약제비) * (100 / 100)) / 10) + 0.5) * 10).ToString();
                        nJiwonAmt = ((((Convert.ToDouble(nBoninTAmt) - GnDrgBiTAmt + Gn항결핵약제비) * (100 / 100)) / 10) + 0.5) * 10;
                    }
                    else
                    {
                        Txt지원.Text = "0";
                    }
                    txtSunap.Text = ((Convert.ToDouble(nBoninTAmt) - clsPmpaType.TIT.RAmt[28, 1] - clsPmpaType.TIT.RAmt[29, 1] - nJiwonAmt)).ToString("#,##0");

                    //'2013-10-07 ----------------------------
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT SUM(SUM(CASE WHEN SUNEXT = 'Y88' THEN AMT END)) Y88 ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  ";
                    SQL = SQL + ComNum.VBLF + " WHERE TRSNO =" + FnTRSNO + " ";
                    SQL = SQL + ComNum.VBLF + "   AND SuNext IN ('Y88') ";
                    SQL = SQL + ComNum.VBLF + " GROUP BY SuNext ";

                    SqlErr = clsDB.GetDataTable(ref dtTemp, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dtTemp.Rows.Count == 0)
                    {

                        dtTemp.Dispose();
                        dtTemp = null;
                        btnSearch.Enabled = true;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    dtTemp.Dispose();
                    dtTemp = null;

                    ntAmt51 = Convert.ToDouble(dtTemp.Rows[0]["Y88"].ToString().Trim());

                    if (ntAmt51 == 0)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT SUM(SUM(CASE WHEN SUNEXT = 'Y88' THEN AMT END)) Y88, ";
                        SQL = SQL + ComNum.VBLF + " SUM(SUM(CASE WHEN SUNEXT IN ('Y85','Y87') THEN AMT END )) Y8785 ";
                        SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  ";
                        SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + FnIPDNO + " ";
                        SQL = SQL + ComNum.VBLF + "   AND SuNext IN ('Y88','Y85','Y87') ";
                        SQL = SQL + ComNum.VBLF + " GROUP BY SuNext ";

                        SqlErr = clsDB.GetDataTable(ref dtTemp, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (dtTemp.Rows.Count > 0)
                        {
                            ntAmt52 = Convert.ToDouble(dtTemp.Rows[0]["Y8785"].ToString().Trim()) - Convert.ToDouble(dtTemp.Rows[0]["Y88"].ToString().Trim());
                        }
                        dtTemp.Dispose();
                        dtTemp = null;

                        //'보증금
                        if (ntAmt52 > 0)
                        {
                            SSAmt_Sheet1.Cells[17, 3].Text = ntAmt52.ToString("#,##0 ");
                            ComFunc.MsgBox("DRG대상이며 중간납 금액:" + ntAmt52.ToString("#,##0 ") + "이 있습니다.." + ComNum.VBLF + "포괄수가진료비는 중간납을 뺀금액이 아니니 참고하십시오", "확인");
                        }
                    }

                    //    SSDrg_Sheet1.Cells [0 , 0].Text = "DRG 원금액";
                    //    SSDrg_Sheet1.Cells [0 , 1].Text = GnDRG_Amt1.ToString ("###,###,##0");
                    //    SSDrg_Sheet1.Cells [0 , 2].Text = GnDRG_WBonAmt.ToString ("###,###,##0");
                    //    SSDrg_Sheet1.Cells [0 , 3].Text = 0.ToString ("###,###,##0");
                    //    SSDrg_Sheet1.Cells [0 , 4].Text = 0.ToString ("###,###,##0");
                    //    SSDrg_Sheet1.Cells [0 , 5].Text = 0.ToString ("###,###,##0");


                    //    SSDrg_Sheet1.Cells [1 , 1].Text = "추가입원료";
                    //    SSDrg_Sheet1.Cells [1 , 2].Text = GnDrg추가입원료.ToString ("###,###,##0");
                    //    SSDrg_Sheet1.Cells [1 , 3].Text = GnDrg추가입원료_Bon.ToString ("###,###,##0");
                    //    SSDrg_Sheet1.Cells [1 , 4].Text = 0.ToString ("###,###,##0");
                    //    SSDrg_Sheet1.Cells [1 , 5].Text = 0.ToString ("###,###,##0");


                    //    SSDrg_Sheet1.Cells [2 , 0].Text = "의료질평가지원";
                    //    SSDrg_Sheet1.Cells [2 , 1].Text = GnDrgJinAmt.ToString ("###,###,##0");                    //요양급여
                    //    SSDrg_Sheet1.Cells [2 , 2].Text = GnDrgJinAmt_Bon.ToString ("###,###,##0");            //본인부담
                    //    SSDrg_Sheet1.Cells [2 , 3].Text = 0.ToString ("###,###,##0");                                      //전액부담
                    //    SSDrg_Sheet1.Cells [2 , 4].Text = 0.ToString ("###,###,##0");                                      //비급여
                    //    SSDrg_Sheet1.Cells [2 , 5].Text = 0.ToString ("###,###,##0");                                      //선택진료

                    //    SSDrg_Sheet1.Cells [3 , 0].Text = "외과가산";
                    //    SSDrg_Sheet1.Cells [3 , 1].Text = GnGsAddAmt.ToString ("###,###,##0");                                      //요양급여
                    //    SSDrg_Sheet1.Cells [3 , 2].Text = (GnGsAddAmt * 0.2).ToString ("###,###,##0");                                  //본인부담
                    //    SSDrg_Sheet1.Cells [3 , 3].Text = 0.ToString ("###,###,##0");                //전액부담
                    //    SSDrg_Sheet1.Cells [3 , 4].Text = 0.ToString ("###,###,##0");                                 //비급여
                    //    SSDrg_Sheet1.Cells [3 , 5].Text = 0.ToString ("###,###,##0");                                 //선택진료

                    //    SSDrg_Sheet1.Cells [4 , 0].Text = "부수술비용";
                    //    SSDrg_Sheet1.Cells [4 , 1].Text = GnDrg부수술총액.ToString ("###,###,##0");             //요양급여
                    //    SSDrg_Sheet1.Cells [4 , 2].Text = GnDrg부수술총액_Bon.ToString ("###,###,##0");     //본인부담
                    //    SSDrg_Sheet1.Cells [4 , 3].Text = 0.ToString ("###,###,##0");                                 //전액부담
                    //    SSDrg_Sheet1.Cells [4 , 4].Text = 0.ToString ("###,###,##0");                                 //비급여
                    //    SSDrg_Sheet1.Cells [4 , 5].Text = 0.ToString ("###,###,##0");                                 //선택진료

                    //    SSDrg_Sheet1.Cells [5 , 0].Text = "복강경개복비";
                    //    SSDrg_Sheet1.Cells [5 , 1].Text = Gn복강개복Amt.ToString ("###,###,##0");                                      //요양급여
                    //    SSDrg_Sheet1.Cells [5 , 2].Text = 0.ToString ("###,###,##0");                     //본인부담
                    //    SSDrg_Sheet1.Cells [5 , 3].Text = 0.ToString ("###,###,##0");                                 //전액부담
                    //    SSDrg_Sheet1.Cells [5 , 4].Text = 0.ToString ("###,###,##0");                                 //비급여
                    //    SSDrg_Sheet1.Cells [5 , 5].Text = 0.ToString ("###,###,##0");                                 //선택진료

                    //    SSDrg_Sheet1.Cells [6 , 0].Text = "응급가산수가";
                    //    SSDrg_Sheet1.Cells [6 , 1].Text = Gn응급가산수가.ToString ("###,###,##0");                //요양급여
                    //    SSDrg_Sheet1.Cells [6 , 2].Text = Gn응급가산수가_Bon.ToString ("###,###,##0");      //본인부담
                    //    SSDrg_Sheet1.Cells [6 , 3].Text = 0.ToString ("###,###,##0");                //전액부담
                    //    SSDrg_Sheet1.Cells [6 , 4].Text = 0.ToString ("###,###,##0");                                 //비급여
                    //    SSDrg_Sheet1.Cells [6 , 5].Text = 0.ToString ("###,###,##0");                                 //선택진료

                    //    SSDrg_Sheet1.Cells [7 , 0].Text = "PCA";
                    //    SSDrg_Sheet1.Cells [7 , 1].Text = Gn재왕절개수가.ToString ("###,###,##0");           //요양급여
                    //    SSDrg_Sheet1.Cells [7 , 2].Text = Gn재왕절개수가_Bon.ToString ("###,###,##0");                                      //본인부담
                    //    SSDrg_Sheet1.Cells [7 , 3].Text = 0.ToString ("###,###,##0");                //전액부담
                    //    SSDrg_Sheet1.Cells [7 , 4].Text = 0.ToString ("###,###,##0");                                 //비급여
                    //    SSDrg_Sheet1.Cells [7 , 5].Text = 0.ToString ("###,###,##0");                                 //선택진료

                    //    SSDrg_Sheet1.Cells [8 , 0].Text = "급여초음파";
                    //    SSDrg_Sheet1.Cells [8 , 1].Text = GnDrgSono.ToString ("###,###,##0");                    //요양급여
                    //    SSDrg_Sheet1.Cells [8 , 2].Text = GnDrgSono_Bon.ToString ("###,###,##0");            //본인부담
                    //    SSDrg_Sheet1.Cells [8 , 3].Text = 0.ToString ("###,###,##0");                     //전액부담
                    //    SSDrg_Sheet1.Cells [8 , 4].Text = 0.ToString ("###,###,##0");                                 //비급여
                    //    SSDrg_Sheet1.Cells [8 , 5].Text = 0.ToString ("###,###,##0");                                 //선택진료

                    //    SSDrg_Sheet1.Cells [9 , 0].Text = "간호간병료";
                    //    SSDrg_Sheet1.Cells [9 , 1].Text = GnDrg간호간병료.ToString ("###,###,##0");             //요양급여
                    //    SSDrg_Sheet1.Cells [9 , 2].Text = GnDrg간호간병료_Bon.ToString ("###,###,##0");     //본인부담
                    //    SSDrg_Sheet1.Cells [9 , 3].Text = 0.ToString ("###,###,##0");               //전액부담
                    //    SSDrg_Sheet1.Cells [9 , 4].Text = 0.ToString ("###,###,##0");                                 //비급여
                    //    SSDrg_Sheet1.Cells [9 , 5].Text = 0.ToString ("###,###,##0");                                 //선택진료

                    //    SSDrg_Sheet1.Cells [10 , 0].Text = "선별급여";
                    //    SSDrg_Sheet1.Cells [10 , 1].Text = GnGs80Amt_T + GnGs50Amt_T.ToString ("###,###,##0");                     //요양급여
                    //    SSDrg_Sheet1.Cells [10 , 2].Text = GnGs80Amt_B + GnGs50Amt_B.ToString ("###,###,##0");                  //본인부담
                    //    SSDrg_Sheet1.Cells [10 , 3].Text = GnGs100Amt.ToString ("###,###,##0");               //전액부담
                    //    SSDrg_Sheet1.Cells [10 , 4].Text = 0.ToString ("###,###,##0");              //비급여
                    //    SSDrg_Sheet1.Cells [10 , 5].Text = 0.ToString ("###,###,##0");                       //선택진료

                    //    SSDrg_Sheet1.Cells [11 , 0].Text = "식  대";
                    //    SSDrg_Sheet1.Cells [11 , 1].Text = (GnDrgFoodAmt [0] + GnDrgFoodAmt [1]).ToString ("###,###,##0");    //요양급여
                    //    SSDrg_Sheet1.Cells [11 , 2].Text = GnDrgFoodAmt [0].ToString ("###,###,##0");                //본인부담
                    //    SSDrg_Sheet1.Cells [11 , 3].Text = GnDrgFoodAmt [2].ToString ("###,###,##0");                   //전액부담
                    //    SSDrg_Sheet1.Cells [11 , 4].Text = GnDrgFoodAmt [4].ToString ("###,###,##0");                   //비급여
                    //    SSDrg_Sheet1.Cells [11 , 5].Text = GnDrgFoodAmt [3].ToString ("###,###,##0");                   //선택진료

                    //    SSDrg_Sheet1.Cells [12 , 0].Text = "DRG급여총액";
                    //    SSDrg_Sheet1.Cells [12 , 1].Text = (GnDRG_TAmt - GnDrg열외군금액_Bon - GnGs100Amt - GnDrgBiTAmt).ToString ("###,###,##0"); //DRG 급여총액
                    //    SSDrg_Sheet1.Cells [12 , 2].Text = CF.FIX_N ((GnDRG_TAmt - GnDrg열외군금액_Bon - GnGs100Amt - GnDrgBiTAmt) - (GnDRG_TAmt - nBoninTAmt)).ToString ("###,###,##0");                   //본인부담
                    //    SSDrg_Sheet1.Cells [12 , 3].Text = GnGs100Amt.ToString ("###,###,##0");                //전액부담
                    //    SSDrg_Sheet1.Cells [12 , 4].Text = GnDrgBiFAmt.ToString ("###,###,##0");                    //비급여
                    //    SSDrg_Sheet1.Cells [12 , 5].Text = GnDrgSelTAmt.ToString ("###,###,##0");                    //선택진료

                    //    SSDrg_Sheet1.Cells [13 , 0].Text = "DRG진료비총액";
                    //    SSDrg_Sheet1.Cells [13 , 1].Text = (GnDRG_TAmt - GnDrg열외군금액_Bon).ToString ("###,###,##0");              //요양급여
                    //    SSDrg_Sheet1.Cells [13 , 2].Text = 0.ToString ("###,###,##0");               //본인부담
                    //    SSDrg_Sheet1.Cells [13 , 3].Text = 0.ToString ("###,###,##0");                   //전액부담
                    //    SSDrg_Sheet1.Cells [13 , 4].Text = 0.ToString ("###,###,##0");                   //DRG 비급여총액
                    //    SSDrg_Sheet1.Cells [13 , 5].Text = 0.ToString ("###,###,##0");                   //선택진료


                    //    SSDrg_Sheet1.Cells [14 , 0].Text = "본인분담금";
                    //    SSDrg_Sheet1.Cells [14 , 1].Text = nBoninTAmt.ToString ("###,###,##0");     //본인부담
                    //    SSDrg_Sheet1.Cells [14 , 2].Text = 0.ToString ("###,###,##0");                  //본인부담
                    //    SSDrg_Sheet1.Cells [14 , 3].Text = 0.ToString ("###,###,##0");                  //본인부담
                    //    SSDrg_Sheet1.Cells [14 , 4].Text = 0.ToString ("###,###,##0");                   //본인부담
                    //    SSDrg_Sheet1.Cells [14 , 5].Text = 0.ToString ("###,###,##0");                   //본인부담

                    //    SSDrg_Sheet1.Cells [15 , 0].Text = "행위별총액";
                    //    SSDrg_Sheet1.Cells [15 , 1].Text = Gn행위별총액.ToString ("###,###,##0");        //요양급여
                    //    SSDrg_Sheet1.Cells [15 , 2].Text = 0.ToString ("###,###,##0");                 //본인부담
                    //    SSDrg_Sheet1.Cells [15 , 3].Text = 0.ToString ("###,###,##0");                   //전액부담
                    //    SSDrg_Sheet1.Cells [15 , 4].Text = 0.ToString ("###,###,##0");                  //비급여
                    //    SSDrg_Sheet1.Cells [15 , 5].Text = 0.ToString ("###,###,##0");                    //선택진료


                    //    SSDrg_Sheet1.Cells [16 , 0].Text = "열외군";
                    //    SSDrg_Sheet1.Cells [16 , 1].Text = GnDrg열외군금액.ToString ("###,###,##0");  //요양급여
                    //    SSDrg_Sheet1.Cells [16 , 2].Text = GnDrg열외군금액_Bon.ToString ("###,###,##0"); //본인부담
                    //    SSDrg_Sheet1.Cells [16 , 3].Text = 0.ToString ("###,###,##0");                   //전액부담
                    //    SSDrg_Sheet1.Cells [16 , 4].Text = 0.ToString ("###,###,##0");                     //비급여
                    //    SSDrg_Sheet1.Cells [16 , 5].Text = 0.ToString ("###,###,##0");                     //선택진료

                    //    SSDrgAmt_Sheet1.Cells [0 , 1].Text = GnDRG_TAmt.ToString ("###,###,##0");   //'DRG 총액
                    //    SSDrgAmt_Sheet1.Cells [1 , 1].Text = GnDRG_Amt1.ToString ("###,###,##0");
                    //    SSDrgAmt_Sheet1.Cells [2 , 1].Text = (GnDRG_TAmt - GnDrgBiTAmt).ToString ("###,###,##0");
                    //    SSDrgAmt_Sheet1.Cells [3 , 1].Text = GnDrgBiTAmt.ToString ("###,###,##0");
                    //    SSDrgAmt_Sheet1.Cells [4 , 1].Text = GnDrg열외군금액_Bon.ToString ("###,###,##0");

                    //    if (GnDrg열외군금액_Bon > 0)
                    //    {
                    //        SSDrgAmt_Sheet1.Cells [4 , 1].BackColor = System.Drawing.Color.FromArgb (255 , 224 , 192);
                    //    }
                    //    else
                    //    {
                    //        SSDrgAmt_Sheet1.Cells [4 , 1].BackColor = System.Drawing.Color.FromArgb (255 , 225 , 255);
                    //    }
                    //    SSDrgAmt_Sheet1.Cells [5 , 1].Text = GnGsAddAmt.ToString ("###,###,##0");   //'DRG 총액
                    //    SSDrgAmt_Sheet1.Cells [6 , 1].Text = VB.Format (GnGs100Amt , "###,###,##0");//     'DRG 원금액
                    //    SSDrgAmt_Sheet1.Cells [7 , 1].Text = VB.Format (Gn복강개복Amt , "###,###,##0");//     'DRG 급여총액
                    //    SSDrgAmt_Sheet1.Cells [8 , 1].Text = VB.Format (GnDrgSelTAmt , "###,###,##0");//    'DRG 비급여총액
                    //    SSDrgAmt_Sheet1.Cells [9 , 1].Text = (GstrOgAdd == "1" ? "Y" : "N");//    'DRG 열외군금액
                    //    SSDrgAmt_Sheet1.Cells [10 , 1].Text = GnGs80Amt_T.ToString ("###,###,##0"); //'100/80


                    //    SSAmt_Sheet1.Cells [11 , 13].Text = "포괄수가진료비";
                    //    SSAmt_Sheet1.Cells [11 , 14].Text = VB.Format (nBoninTAmt , "###,###,##0");
                    //}

                    txtJohapAmt.Text = (VB.Fix((int)(GnDRG_TAmt - Convert.ToDouble(nBoninTAmt)))).ToString("###,###,##0");
                    txtBonAmt.Text = Convert.ToDouble(nBoninTAmt).ToString("###,###,##0");

                    //'2015-03-27 심사완료전 AMT컬럼에 금액을 갱신

                    if (clsPmpaType.TIT.ActDate == "" && Convert.ToInt32(clsPmpaType.TIT.TGbSts) < Convert.ToInt32("5") && (clsPmpaType.TIT.DeptCode == "OG" || clsPmpaType.TIT.DeptCode == "EN" || clsPmpaType.TIT.DeptCode == "OT"))
                    {
                        SQL = "UPDATE IPD_TRANS SET ";
                        SQL = SQL + ComNum.VBLF + " Amt50 = " + GnDRG_Amt2 + ",";//     '총진료비
                        SQL = SQL + ComNum.VBLF + " Amt53 = " + (Convert.ToDouble(GnDRG_TAmt) - Convert.ToDouble(nBoninTAmt)) + ",";//     '조합부담
                        SQL = SQL + ComNum.VBLF + " Amt55 = " + nBoninTAmt + " ";//     '차인납부
                        SQL = SQL + ComNum.VBLF + " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + " ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr + "IPD_TRANS에 DRG 금액을 저장 도중 오류가 발생함", "오류");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                }

                if (strGbDrg == "D" && strDrgCode == "")
                {
                    ComFunc.MsgBox("DRG코드가 부여되지 않았거나 조건에 맞지않습니다.", "조회불가");
                }

                SSAmt_Sheet1.Cells[17, 2].Text = "◆중간납◆";

                SQL = "";
                SQL = " SELECT AGE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "   AND IPDNO  = " + FnIPDNO + " ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (Convert.ToInt32(dt.Rows[0]["AGE"].ToString().Trim()) <= 6)
                {
                    // '입원기간동안에 6세이상인경우 체크함.
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT JUMIN1,JUMIN2, JUMIN3 ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                    SQL = SQL + ComNum.VBLF + " WHERE 1=1";
                    SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPano + "' ";

                    if (SqlErr != "")
                    {
                        btnSearch.Enabled = true;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (dtRs.Rows.Count > 0)
                    {
                        //'주민암호화
                        if (dtRs.Rows[0]["JUMIN3"].ToString().Trim() != "")
                        {
                            strJumin = dtRs.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dtRs.Rows[0]["JUMIN3"].ToString().Trim());
                        }
                        else
                        {
                            strJumin = dtRs.Rows[0]["JUMIN1"].ToString().Trim() + dtRs.Rows[0]["JUMIN2"].ToString().Trim();
                        }
                    }
                    dtRs.Dispose();
                    dtRs = null;

                    strRemark = "";
                    //TODO
                    //strRemark = AGE_PD_GESAN (strJumin , strInDate , GstrSysDate)
                    if (strRemark != "")
                    {
                        ComFunc.MsgBox(strRemark);
                    }
                }
                if (dt.Rows.Count > 1)
                {
                    dt.Dispose();
                    dt = null;
                }


                //'2013-11-26 감염관리 정보
                //ImgInfect.Visible = False
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT ROWID  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + " EXAM_INFECT_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1 ";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ODATE IS NULL ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows[0]["ROWID"].ToString().Trim() != "")
                {
                    //Call SET_Infection_img ("" , Trim (TxtPano.Text))

                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }
    }
}
