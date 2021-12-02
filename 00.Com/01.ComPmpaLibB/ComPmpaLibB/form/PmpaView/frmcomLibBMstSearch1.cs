using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using ComPmpaLibB.form.PmpaView;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-10-16
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\misu\misumir.vbp\MUMAIN03.FRM(FrmMstSearch1.frm)" >> frmcomLibBMstSearch1.cs 폼이름 재정의" />

    public partial class frmcomLibBMstSearch1 : Form
    {
        string GstrClass = "";

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();

        clsPmpaPb.GstrBunSu GstrBunSu = null;

        frmPmpaMisuMast fPMM = null;
        frmPmpaMisuMast1 fPMM1 = null;
        frmPmpaMisuMast2 fPMM2 = null;
        frmPmpaMisuMast2TA fPMMT = null;
        

        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        int nREAD = 0;
        int nYY = 0;
        int nMM = 0;
        string strData = "";
        string strFData = "";
        string strTData = "";
        double[] nAmt = new double[5];
        string strTDate = "";
        string strFDate = "";
        string MisuGubun = "";

        public frmcomLibBMstSearch1()
        {
            InitializeComponent();
        }

        public frmcomLibBMstSearch1(clsPmpaPb.GstrBunSu argBunSu, string argGubun)
        {
            InitializeComponent();
            
            GstrBunSu = argBunSu;

            MisuGubun = argGubun;
        }

        private void frmcomLibBMstSearch1_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            GstrClass = GstrBunSu.GstrClass;//2018-12-07,  김해수

            lblCaption.Text = "";

            ssView_Sheet1.Columns[11].Visible = false;

            if (TxtYYMM.Text.Trim() != "" || txtKiho.Text != "")
            {
                ssView.Select();
            }
        }

        private void rdoMisu0_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoMisu0.Checked == true)
            {
                rdoMisu0.Checked = true;
                rdoMisu1.Checked = false;
                rdoMisu2.Checked = false;
            }
            else if (rdoMisu1.Checked == true)
            {
                rdoMisu0.Checked = false;
                rdoMisu1.Checked = true;
                rdoMisu2.Checked = false;

            }
            else
            {
                rdoMisu0.Checked = false;
                rdoMisu1.Checked = false;
                rdoMisu2.Checked = true;

            }
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //double GnWRTNO = 0;  한기호

            //GnWRTNO = VB.Val(ssView_Sheet1.Cells[e.Row, 11].Text);
            GstrBunSu.GnWRTNO = Convert.ToInt64(VB.Val(ssView_Sheet1.Cells[e.Row, 11].Text));//2018-12-07,  김해수

            if (GstrBunSu.GnWRTNO != 0)//2018-12-07,  김해수
            {
                switch (MisuGubun)
                {
                    case "GITA":
                        fPMM = new frmPmpaMisuMast();
                        fPMM.setClass(GstrBunSu);
                        this.Visible = false;
                        break;
                    case "SANJE":
                        fPMM2 = new frmPmpaMisuMast2();
                        fPMM2.setClass(GstrBunSu);
                        this.Visible = false;
                        break;
                    case "TA":
                        fPMMT = new frmPmpaMisuMast2TA();
                        fPMMT.setClass(GstrBunSu);
                        this.Visible = false;
                        break;
                    case "bohun":
                        fPMM1 = new frmPmpaMisuMast1();
                        fPMM1.setClass(GstrBunSu);
                        this.Visible = false;
                        break;
                    default:
                        break;
                }
                
                
            }
        }

        private void txtKiho_Leave(object sender, EventArgs e)
        {
            txtKiho.Text = VB.UCase(txtKiho.Text);

            if (txtKiho.Text == "")
            {
                return;
            }
            lblCaption.Text = CPF.GET_BAS_MIA(clsDB.DbCon, txtKiho.Text);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
           
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            for (i = 0; i < 5; i++)
            {
                nAmt[i] = 0;
            }

            if (string.Compare(GstrClass, "02") > 0 && TxtYYMM.Text.Trim() == "" && txtKiho.Text == "")
            {
                ComFunc.MsgBox("청구년월 및 조합기호가 공란입니다", "확인");
                TxtYYMM.Focus();
                return;
            }

            try
            {

                if (TxtYYMM.Text != "")
                {
                    if (TxtYYMM.Text.Trim().Length != 6)
                    {
                        ComFunc.MsgBox("청구년월이 잘못입력되었습니다", "확인");
                        TxtYYMM.Focus();
                        return;

                    }
                    else
                    {
                        nYY = Convert.ToInt32(VB.Val(VB.Left(TxtYYMM.Text, 4)));
                        nMM = Convert.ToInt32(VB.Val(VB.Right(TxtYYMM.Text, 2)));

                        if (nMM < 1 || nMM > 12)
                        {
                            ComFunc.MsgBox("청구년월이 잘못입력되었습니다", "확인");
                            TxtYYMM.Focus();
                            return;
                        }

                        strData = (nYY.ToString("0000") + "-" + (nMM.ToString("00"))) + "-01";
                        strFDate = strData;
                        nMM = nMM + 1;

                        if (nMM == 13)
                        {
                            nMM = 1;
                            nYY = nYY + 1;
                        }
                        strData = (nYY.ToString("0000") + "-" + (nMM.ToString("00"))) + "-01";
                        strTDate = strData;
                    }
                }

                switch (GstrClass)
                {
                    case "01":
                    case "02":
                    case "03":
                    case "04":
                        Class_13_RTN();
                        break;
                    case "05":
                    case "06":
                    case "07":
                        Class_31_RTN();
                        break;
                    case "08":
                    case "09":
                    case "10":
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "15":
                        Class_ETC_RTN();
                        break;
                }

                if (!(nAmt[0] == 0 && nAmt[1] == 0 && nAmt[2] == 0 && nAmt[3] == 0))
                {
                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3 - 1].Text = "합  계";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5 - 1].Text = nAmt[0].ToString("############0");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6 - 1].Text = nAmt[1].ToString("############0");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7 - 1].Text = nAmt[2].ToString("############0");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8 - 1].Text = nAmt[4].ToString("############0");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9 - 1].Text = nAmt[3].ToString("############0");
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10 - 1].Text = (nAmt[0] - nAmt[1] - nAmt[2] - nAmt[3] - nAmt[4]).ToString("############0");
                }
                ssView.Select();
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

        private string BunName_Set(string Arg1)
        {
            string ArgReturn = "";

            switch (Arg1)
            {
                case "01":
                    ArgReturn = "내과분야";
                    break;
                case "02":
                    ArgReturn = "외과분야";
                    break;
                case "03":
                    ArgReturn = "산소아과";
                    break;
                case "04":
                    ArgReturn = "안.이비인후";
                    break;
                case "05":
                    ArgReturn = "피.비뇨기";
                    break;
                case "06":
                    ArgReturn = "치과";
                    break;
                case "07":
                    ArgReturn = "NP정액";
                    break;
                case "08":
                    ArgReturn = "장애대불";
                    break;
                case "09":
                    ArgReturn = "가정간호";
                    break;
                case "10":
                    ArgReturn = "재청구";
                    break;
                case "11":
                    ArgReturn = "이의신청";
                    break;
                case "12":
                    ArgReturn = "정산진료비";
                    break;
                case "13":
                    ArgReturn = "추가청구";
                    break;
                case "14":
                    ArgReturn = "NP장애대불";
                    break;
                case "15":
                    ArgReturn = "HD정액";
                    break;
                case "19":
                    ArgReturn = "기타청구";
                    break;
                case "20":
                    ArgReturn = "상한대불";
                    break;
                case "21":
                    ArgReturn = "희귀지원금";
                    break;
                case "22":
                    ArgReturn = "결핵지원금";
                    break;
                case "23":
                    ArgReturn = "DRG(포괄수가)";
                    break;
                case "24":
                    ArgReturn = "100/100 미만";
                    break;
            }
            return ArgReturn;
        }

        /// <summary>
        /// 지역,보호 Select
        /// </summary>
        private void Class_13_RTN()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,TO_CHAR(a.Bdate,'YYYY-MM-DD') Bdate,                   ";
            SQL = SQL + ComNum.VBLF + "        a.MisuID,a.IpdOpd,a.Bun,a.Amt2 MAmt,                           ";
            SQL = SQL + ComNum.VBLF + "        a.Amt3+a.Amt6+a.Amt7 IAMT, a.Amt4 SAMT, a.Amt5 BAMT, Amt8 SAmt2, b.MiaName ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.VBLF + "MISU_IDMST a," + ComNum.VBLF + "BAS_MIA b ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "    AND Class = '" + GstrClass + "' ";

            if (TxtYYMM.Text != "")
            {
                SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')          ";
                SQL = SQL + ComNum.VBLF + "    AND Bdate <  TO_DATE('" + strTDate + "','YYYY-MM-DD')          ";
            }
            if (txtKiho.Text != "")
            {
                SQL = SQL + ComNum.VBLF + "    AND RTRIM(a.GelCode) = '" + txtKiho.Text + "'                   ";
            }

            if (rdoMisu0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "    AND a.GbEnd = '1'                                              ";
            }
            else if (rdoMisu1.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "    AND a.GbEnd = '0'                                              ";
            }

            SQL = SQL + ComNum.VBLF + "    AND b.MiaCode = a.GelCode                                   ";

            if (rdoMisu0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "  ORDER BY Bdate,b.MiaName,a.MisuID                                ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "  ORDER BY Bdate DESC,b.MiaName,a.MisuID                           ";
            }

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
            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssView_Sheet1.Cells[i, 1 - 1].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                ssView_Sheet1.Cells[i, 2 - 1].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                ssView_Sheet1.Cells[i, 3 - 1].Text = dt.Rows[i]["MiaName"].ToString().Trim();
                ssView_Sheet1.Cells[i, 4 - 1].Text = BunName_Set(dt.Rows[i]["Bun"].ToString().Trim());

                ssView_Sheet1.Cells[i, 5 - 1].Text = VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()).ToString("###########0");
                nAmt[0] = nAmt[0] + VB.Val(dt.Rows[i]["MAmt"].ToString().Trim());

                ssView_Sheet1.Cells[i, 6 - 1].Text = VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()).ToString("###########0");
                nAmt[1] = nAmt[1] + VB.Val(dt.Rows[i]["IAmt"].ToString().Trim());


                ssView_Sheet1.Cells[i, 7 - 1].Text = VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()).ToString("###########0");
                nAmt[2] = nAmt[2] + VB.Val(dt.Rows[i]["SAmt"].ToString().Trim());


                ssView_Sheet1.Cells[i, 8 - 1].Text = VB.Val(dt.Rows[i]["SAmt2"].ToString().Trim()).ToString("###########0");
                nAmt[4] = nAmt[4] + VB.Val(dt.Rows[i]["SAmt2"].ToString().Trim());


                ssView_Sheet1.Cells[i, 9 - 1].Text = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()).ToString("###########0");
                nAmt[3] = nAmt[3] + VB.Val(dt.Rows[i]["BAmt"].ToString().Trim());


                ssView_Sheet1.Cells[i, 10 - 1].Text = (VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["SAmt2"].ToString().Trim()) - VB.Val(dt.Rows[i]["Bamt"].ToString().Trim())).ToString("###########0");

                ssView_Sheet1.Cells[i, 11 - 1].Text = VB.Val(dt.Rows[i]["MisuID"].ToString().Trim()).ToString("00000000");
                ssView_Sheet1.Cells[i, 12 - 1].Text = VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim()).ToString();
            }
            dt.Dispose();
            dt = null;

        }

        /// <summary>
        /// 산재,자보 Select
        /// </summary>
        private void Class_31_RTN()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,TO_CHAR(a.Bdate,'YYYY-MM-DD') Bdate,a.GelCode,     ";
            SQL = SQL + ComNum.VBLF + "        a.MisuID,a.IpdOpd,a.DeptCode,a.Amt2 MAmt,                  ";
            SQL = SQL + ComNum.VBLF + "        a.Amt3+a.Amt6+a.Amt7 IAMT, a.Amt4 SAMT, a.Amt5 BAMT,  Amt8 SAmt2,      ";
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(a.FromDate,'YY-MM-DD') Fdate,                      ";
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(a.ToDate,'YY-MM-DD') Tdate, b.Sname                ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.VBLF + "MISU_IDMST a," + ComNum.VBLF + "BAS_PATIENT b                                 ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "    AND Class = '" + GstrClass + "'                                ";


            if (TxtYYMM.Text != "")
            {
                SQL = SQL + ComNum.VBLF + "    AND Bdate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')          ";
                SQL = SQL + ComNum.VBLF + "    AND Bdate <  TO_DATE('" + strTDate + "','YYYY-MM-DD')          ";
            }
            if (txtKiho.Text != "")
            {
                SQL = SQL + ComNum.VBLF + "    AND a.GelCode = '" + txtKiho.Text + "'                   ";
            }

            if (rdoMisu0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "    AND a.GbEnd = '1'                                              ";
            }
            else if (rdoMisu1.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "    AND a.GbEnd = '0'                                              ";
            }

            SQL = SQL + ComNum.VBLF + "    AND a.MisuID = b.Pano(+)                                       ";
            SQL = SQL + ComNum.VBLF + "    ORDER BY b.Sname,a.Bdate,a.FromDate                                        ";

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
            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssView_Sheet1.Cells[i, 1 - 1].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                ssView_Sheet1.Cells[i, 2 - 1].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                ssView_Sheet1.Cells[i, 3 - 1].Text = dt.Rows[i]["Sname"].ToString().Trim();


                strData = dt.Rows[i]["GelCode"].ToString().Trim() + ",";
                strData = strData + dt.Rows[i]["FDate"].ToString().Trim() + "-";
                strData = strData + dt.Rows[i]["TDate"].ToString().Trim() + ",";
                strData = strData + dt.Rows[i]["DeptCode"].ToString().Trim();
                ssView_Sheet1.Cells[i, 4 - 1].Text = strData;

                ssView_Sheet1.Cells[i, 5 - 1].Text = VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()).ToString("###########0");
                nAmt[0] = nAmt[0] + VB.Val(dt.Rows[i]["MAmt"].ToString().Trim());

                ssView_Sheet1.Cells[i, 6 - 1].Text = VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()).ToString("###########0");
                nAmt[1] = nAmt[1] + VB.Val(dt.Rows[i]["IAmt"].ToString().Trim());

                ssView_Sheet1.Cells[i, 7 - 1].Text = VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()).ToString("###########0");
                nAmt[2] = nAmt[2] + VB.Val(dt.Rows[i]["SAmt"].ToString().Trim());

                ssView_Sheet1.Cells[i, 8 - 1].Text = VB.Val(dt.Rows[i]["SAmt2"].ToString().Trim()).ToString("###########0");
                nAmt[4] = nAmt[4] + VB.Val(dt.Rows[i]["SAmt2"].ToString().Trim());

                ssView_Sheet1.Cells[i, 9 - 1].Text = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()).ToString("###########0");
                nAmt[3] = nAmt[3] + VB.Val(dt.Rows[i]["BAmt"].ToString().Trim());

                ssView_Sheet1.Cells[i, 10 - 1].Text = (VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["SAmt2"].ToString().Trim()) - VB.Val(dt.Rows[i]["Bamt"].ToString().Trim())).ToString("###########0");

                ssView_Sheet1.Cells[i, 11 - 1].Text = VB.Val(dt.Rows[i]["MisuID"].ToString().Trim()).ToString("00000000");
                ssView_Sheet1.Cells[i, 12 - 1].Text = VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim()).ToString();
            }
            dt.Dispose();
            dt = null;

        }

        /// <summary>
        /// 헌혈,계약처
        /// </summary>
        private void Class_ETC_RTN()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수



            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT a.WRTNO,TO_CHAR(a.Bdate,'YYYY-MM-DD') Bdate,a.GelCode, ";
            SQL = SQL + ComNum.VBLF + "        a.MisuID,a.IpdOpd,a.DeptCode,a.Amt2 MAmt,              ";
            SQL = SQL + ComNum.VBLF + "        a.Amt3+a.Amt6+a.Amt7 IAMT, a.Amt4 SAMT, a.Amt5 BAMT, Amt8 SAmt2,  ";
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(a.FromDate,'YY-MM-DD') Fdate,                  ";
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(a.ToDate,'YY-MM-DD') Tdate, b.Sname            ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.VBLF + "MISU_IDMST a," + ComNum.VBLF + "BAS_PATIENT b   ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1";
            SQL = SQL + ComNum.VBLF + "    AND Class = '" + GstrClass + "'                            ";

            if (TxtYYMM.Text != "")
            {
                SQL = SQL + ComNum.VBLF + "    AND a.MirYYMM = '" + TxtYYMM.Text + "'                 ";
            }
            if (txtKiho.Text != "")
            {
                SQL = SQL + ComNum.VBLF + "    AND a.GelCode = '" + txtKiho.Text + "'                   ";
            }

            if (rdoMisu0.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "    AND a.GbEnd = '1'                                              ";
            }
            else if (rdoMisu1.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "    AND a.GbEnd = '0'                                              ";
            }

            SQL = SQL + ComNum.VBLF + "    AND a.MisuID = b.Pano(+)                                   ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY b.Sname,a.Bdate,a.FromDate                          ";

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
            ssView_Sheet1.RowCount = dt.Rows.Count;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssView_Sheet1.Cells[i, 1 - 1].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                ssView_Sheet1.Cells[i, 2 - 1].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                ssView_Sheet1.Cells[i, 3 - 1].Text = dt.Rows[i]["Sname"].ToString().Trim();


                strData = dt.Rows[i]["GelCode"].ToString().Trim() + ",";
                strData = strData + dt.Rows[i]["FDate"].ToString().Trim() + "-";
                strData = strData + dt.Rows[i]["TDate"].ToString().Trim() + ",";
                strData = strData + dt.Rows[i]["DeptCode"].ToString().Trim();
                ssView_Sheet1.Cells[i, 4 - 1].Text = strData;

                ssView_Sheet1.Cells[i, 5 - 1].Text = VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()).ToString("###########0");
                nAmt[3] = nAmt[3] + VB.Val(dt.Rows[i]["MAmt"].ToString().Trim());

                ssView_Sheet1.Cells[i, 6 - 1].Text = VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()).ToString("###########0");
                nAmt[3] = nAmt[3] + VB.Val(dt.Rows[i]["IAmt"].ToString().Trim());

                ssView_Sheet1.Cells[i, 7 - 1].Text = VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()).ToString("###########0");
                nAmt[3] = nAmt[3] + VB.Val(dt.Rows[i]["SAmt"].ToString().Trim());

                ssView_Sheet1.Cells[i, 8 - 1].Text = VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()).ToString("###########0");
                nAmt[4] = nAmt[4] + VB.Val(dt.Rows[i]["SAmt"].ToString().Trim());

                ssView_Sheet1.Cells[i, 9 - 1].Text = VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()).ToString("###########0");
                nAmt[3] = nAmt[3] + VB.Val(dt.Rows[i]["BAmt"].ToString().Trim());

                ssView_Sheet1.Cells[i, 10 - 1].Text = (VB.Val(dt.Rows[i]["MAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["SAmt"].ToString().Trim()) - VB.Val(dt.Rows[i]["SAmt2"].ToString().Trim()) - VB.Val(dt.Rows[i]["Bamt"].ToString().Trim())).ToString("###########0");

                ssView_Sheet1.Cells[i, 11 - 1].Text = VB.Val(dt.Rows[i]["MisuID"].ToString().Trim()).ToString("00000000");
                ssView_Sheet1.Cells[i, 12 - 1].Text = VB.Val(dt.Rows[i]["WRTNO"].ToString().Trim()).ToString();
            }
            dt.Dispose();
            dt = null;

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //프린트 버튼
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }     //권한확인

            strTitle = "월 별 청 구  및  미 수 금 현 황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조합기호 : " + lblCaption.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            if (rdoMisu0.Checked == true)
            {
                strHeader += CS.setSpdPrint_String(rdoMisu0.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            if (rdoMisu1.Checked == true)
            {
                strHeader += CS.setSpdPrint_String(rdoMisu1.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }
            if (rdoMisu2.Checked == true)
            {
                strHeader += CS.setSpdPrint_String(rdoMisu2.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

