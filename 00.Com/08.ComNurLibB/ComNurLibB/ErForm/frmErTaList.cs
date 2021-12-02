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
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    public partial class frmErTaList : Form
    {
        //string FstrJob = "";
        //string FstrPrtHead = "";
        string strSysDate = "";
        //string FstrViewMode = "";
        //bool FbViewOK = false;

        public frmErTaList()
        {
            InitializeComponent();
        }

        private void frmErTaList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //string GstrJob = "";
            //int GnMinIlsu = 0;
            //int GnMaxIlsu = 0;
            //string GstrCaption = "";

            ssView.Dock = DockStyle.Fill;

            //ssView_Sheet1.Columns[13].Width = 40;
            //ssView_Sheet1.Columns[14].Width = 40;

            //ssPrint_Sheet1.Columns[12].Width = 40;
            //ssPrint_Sheet1.Columns[13].Width = 40;

            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            dtpFDate.Value = Convert.ToDateTime(strSysDate);
            dtpTDate.Value = Convert.ToDateTime(strSysDate);

            

            cboOutGbn.Items.Clear();
            cboOutGbn.Items.Add("0.전체");
            cboOutGbn.Items.Add("1.입원");
            cboOutGbn.Items.Add("2.귀가");
            cboOutGbn.Items.Add("3.DOA");             //사망후 응급실도착
            cboOutGbn.Items.Add("4.사망");             //응급실에서 사망
            cboOutGbn.Items.Add("5.취소");
            cboOutGbn.Items.Add("6.이송");
            cboOutGbn.Items.Add("7.DAMA");
            cboOutGbn.Items.Add("8.OPD");
            cboOutGbn.Items.Add("9.수술후입원(응급수술)");
            cboOutGbn.SelectedIndex = 0;
            
            cboSayu.Items.Clear();
            cboSayu.Items.Add("*.전체");
            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboSayu, "EMI_내원사유(질병여부)", 1, false, "N");
            cboSayu.SelectedIndex = 0;

            cboDept.Items.Clear();
            cboDept.Items.Add("전체");

            cboDept2.Items.Clear();
            cboDept2.Items.Add("전체");

            cboWard.Items.Clear();
            cboWard.Items.Add("전체");
            cboWard.Items.Add("33");
            cboWard.Items.Add("40");
            cboWard.Items.Add("65");
            cboWard.SelectedIndex = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //진료과를 READ
                SQL = "";
                SQL = "SELECT DeptCode";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE DeptCode NOT IN ('TO','HR','R7','II','R6','PT','OC','HC') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PrintRanking,DeptCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                        cboDept2.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                    }

                    cboDept.SelectedIndex = 0;
                    cboDept2.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            btnSearchClick();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            string strSysDateTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");

            strTitle = "응  급  실  TA  대  장";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("조회기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + " ~ " + dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("내원사유 : " + cboSayu.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("인쇄일자 : " + strSysDateTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("응급실장: ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(ssView, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void btnSearchClick()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strDate = "";
            string strDate1 = "";
            string strINTIME = "";
            string strOutTime = "";
            string strDispDate = "";

            ssView_Sheet1.RowCount = 0;
            strDate = dtpFDate.Value.ToString("yyyy-MM-dd");
            strDate1 = dtpTDate.Value.AddDays(1).ToString("yyyy-MM-dd");
            strDispDate = VB.Val(VB.Mid(strDate, 6, 2)).ToString("#0") + "/" + VB.Val(VB.Right(strDate, 2)).ToString("#0");

            try
            {                
                SQL = "";                
                SQL = SQL + ComNum.VBLF + " SELECT                                                                   ";
                SQL = SQL + ComNum.VBLF + "         A.INTIME, A.PANO, B.SNAME, A.DEPTCODE, A.KTASLEVL, A.AGE, A.SEX, ";
                SQL = SQL + ComNum.VBLF + "         C.JICODE, A.SinGu, A.BI, A.WARDCODE, A.ROOM, A.STUDY, A.DISEASE, ";
                SQL = SQL + ComNum.VBLF + "         A.OUTGBN, A.DRPDATE1, HOTIME1, HODATE1, HODRNAME1, NURSE,        ";
                SQL = SQL + ComNum.VBLF + "         A.JDATE, A.HODEPT2, A.HODEPT3, A.HODEPT4, A.HODEPT5, A.OUTTIME   ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_ER_PATIENT A                                        ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_PMPA.OPD_MASTER B                                      ";
                SQL = SQL + ComNum.VBLF + "     ON A.PANO = B.PANO                                                   ";
                SQL = SQL + ComNum.VBLF + " INNER JOIN KOSMOS_PMPA.BAS_PATIENT C                                     ";
                SQL = SQL + ComNum.VBLF + "     ON A.PANO = C.PANO                                                   ";
                SQL = SQL + ComNum.VBLF + "       AND A.JDATE = TRIM(B.JTIME)                                        ";
                SQL = SQL + ComNum.VBLF + "       AND B.OCSJIN = '#'                                                 ";
                SQL = SQL + ComNum.VBLF + "       AND A.JDATE >= TO_DATE('"+ strDate + "', 'YYYY-MM-DD')             ";
                SQL = SQL + ComNum.VBLF + "       AND A.JDATE < TO_DATE('" + strDate1 + "', 'YYYY-MM-DD')             ";
                SQL = SQL + ComNum.VBLF + "       AND A.OUTGBN <> '5'                                                ";
                SQL = SQL + ComNum.VBLF + "       AND A.BI IN ('52', '55')                                           ";
                SQL = SQL + ComNum.VBLF + "       AND(A.DGKD IS NULL OR A.DGKD NOT IN('4'))                          ";
                
                if (VB.Left(cboOutGbn.Text, 1) != "0")
                {
                    SQL = SQL + " AND A.OUTGBN = '" + VB.Left(cboOutGbn.Text, 1) + "' ";
                }

                if (cboDept2.Text != "전체")
                {
                    SQL = SQL + "AND  ( a.HODEPT1='" + cboDept2.Text + "' OR a.HODEPT2='" + cboDept2.Text + "' OR a.HODEPT3='" + cboDept2.Text + "' OR a.HODEPT4='" + cboDept2.Text + "' OR a.HODEPT5='" + cboDept2.Text + "') ";
                }
                if (cboDept.Text != "전체")
                {
                    SQL = SQL + " AND a.DeptCode='" + cboDept.Text + "' ";
                }
                if (cboWard.Text != "전체")
                {
                    SQL = SQL + " AND a.WardCode='" + cboWard.Text + "' ";
                }
                
                if (VB.Left(cboSayu.Text, 1) != "**")
                {
                    switch (VB.Left(cboSayu.Text, 1).Trim())
                    {
                        case "1":
                            SQL = SQL + ComNum.VBLF + " AND A.PANO IN ( SELECT PTMIIDNO FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI WHERE PTMIINDT = '" + strDate.Replace("-", "") + "'  AND PTMIDGKD ='1' )";
                            break;
                        case "2":
                            SQL = SQL + ComNum.VBLF + " AND A.PANO IN ( SELECT PTMIIDNO FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI WHERE PTMIINDT = '" + strDate.Replace("-", "") + "'  AND PTMIDGKD ='2' )";
                            break;
                        case "3":
                            SQL = SQL + ComNum.VBLF + " AND A.PANO IN ( SELECT PTMIIDNO FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI WHERE PTMIINDT = '" + strDate.Replace("-", "") + "'  AND PTMIDGKD ='3' )";
                            break;
                    }
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY a.InTime ";


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
                        strOutTime = dt.Rows[i]["OutTime"].ToString().Trim();
                        strINTIME = dt.Rows[i]["InTime"].ToString().Trim();

                        if (string.Compare(VB.Left(strINTIME, 10), strDate) < 0)
                        {
                            ssView_Sheet1.Cells[i, 1].Text = VB.Mid(strINTIME, 6, 2) + "/" + VB.Mid(strINTIME, 9, 2);
                            ssView_Sheet1.Cells[i, 2].Text = "Kp" + " " + VB.Right(strINTIME, 5);
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 1].Text = strDispDate;
                            ssView_Sheet1.Cells[i, 2].Text = VB.Right(dt.Rows[i]["InTime"].ToString().Trim(), 5);
                        }

                        ssView_Sheet1.Cells[i, 23].Text = dt.Rows[i]["InTime"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["KTASLEVL"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = Read_JiName(dt.Rows[i]["JICODE"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SinGu"].ToString().Trim() == "1" ? "신환" : "구환";

                        switch (dt.Rows[i]["Bi"].ToString().Trim())
                        {
                            case "51":
                            case "53":
                            case "54":
                                ssView_Sheet1.Cells[i, 10].Text = "일반";
                                break;
                            case "52":
                                ssView_Sheet1.Cells[i, 10].Text = "교통";
                                break;
                            case "31":
                                ssView_Sheet1.Cells[i, 10].Text = "산재";
                                break;
                            case "21":
                            case "22":
                            case "23":
                            case "24":
                            case "25":
                            case "26":
                            case "27":
                            case "28":
                            case "29":
                                ssView_Sheet1.Cells[i, 10].Text = "보호";
                                break;
                            default:
                                ssView_Sheet1.Cells[i, 10].Text = "보험";
                                break;
                        }

                        ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["WardCode"].ToString().Trim();

                        if (VB.Val(dt.Rows[i]["Room"].ToString().Trim()) > 0)
                        {
                            ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Room"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["Study"].ToString().Trim();

                        ssView_Sheet1.Cells[i, 14].Text = Read_FinalDiagnosis(dt.Rows[i]["Pano"].ToString().Trim(), VB.Left(strINTIME, 10), VB.Right(strINTIME, 5), dt.Rows[i]["Disease"].ToString().Trim());
                        
                        if (strOutTime == "" || string.Compare(VB.Left(strOutTime, 10), strDate) > 0)
                        {
                            ssView_Sheet1.Cells[i, 15].Text = "Keep";
                        }
                        else
                        {
                            switch (dt.Rows[i]["OutGbn"].ToString().Trim())
                            {
                                case "1":
                                    ssView_Sheet1.Cells[i, 15].Text = "입원";
                                    break;
                                case "2":
                                    ssView_Sheet1.Cells[i, 15].Text = "귀가";
                                    break;
                                case "3":
                                    ssView_Sheet1.Cells[i, 15].Text = "DOA";
                                    break;
                                case "4":
                                    ssView_Sheet1.Cells[i, 15].Text = "사망";
                                    break;
                                case "5":
                                    ssView_Sheet1.Cells[i, 15].Text = "취소";
                                    break;
                                case "6":
                                    ssView_Sheet1.Cells[i, 15].Text = "이송";
                                    break;
                                case "7":
                                    ssView_Sheet1.Cells[i, 15].Text = "DAMA";
                                    break;
                                case "8":
                                    ssView_Sheet1.Cells[i, 15].Text = "OPD";
                                    break;
                                case "9":
                                    ssView_Sheet1.Cells[i, 15].Text = "OR입원";
                                    break;
                                default:
                                    ssView_Sheet1.Cells[i, 15].Text = "**";
                                    break;
                            }

                            if (VB.IsDate(strOutTime) == true)
                            {
                                ssView_Sheet1.Cells[i, 19].Text = Convert.ToDateTime(strOutTime).ToString("HH:mm");
                            }                            
                        }
                                                
                        if(VB.IsDate(dt.Rows[i]["DrPDate1"].ToString().Trim()) == true)
                        {
                            ssView_Sheet1.Cells[i, 16].Text = Convert.ToDateTime(dt.Rows[i]["DrPDate1"].ToString().Trim()).ToString("HH:mm");
                        }
                        if (VB.IsDate(dt.Rows[i]["HOTIME1"].ToString().Trim()) == true)
                        {
                            ssView_Sheet1.Cells[i, 17].Text = Convert.ToDateTime(dt.Rows[i]["HOTIME1"].ToString().Trim()).ToString("HH:mm");
                        }
                        if (VB.IsDate(dt.Rows[i]["HODATE1"].ToString().Trim()) == true)
                        {
                            ssView_Sheet1.Cells[i, 18].Text = Convert.ToDateTime(dt.Rows[i]["HODATE1"].ToString().Trim()).ToString("HH:mm");
                        }
                                                
                        ssView_Sheet1.Cells[i, 20].Text = dt.Rows[i]["HODRNAME1"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 21].Text = dt.Rows[i]["Nurse"].ToString().Trim().Replace(" ", "");
                        ssView_Sheet1.Cells[i, 22].Text = dt.Rows[i]["JDate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 24].Text = dt.Rows[i]["HODEPT2"].ToString().Trim() + " " + dt.Rows[i]["HODEPT3"].ToString().Trim() + " " + dt.Rows[i]["HODEPT4"].ToString().Trim() + " " + dt.Rows[i]["HODEPT5"].ToString().Trim();

                        ssView_Sheet1.SetRowHeight(i, (int)ssView_Sheet1.GetPreferredRowHeight(i) + 2);
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }


        string Read_JiName(string argVar)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT A.JICODE, A.GUBUN";
                SQL = SQL + ComNum.VBLF + "  FROM (";
                SQL = SQL + ComNum.VBLF + "  SELECT JICODE, '포항' GUBUN FROM " + ComNum.DB_PMPA + "BAS_AREA";
                SQL = SQL + ComNum.VBLF + "  WHERE JICODE <= '64'";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "  SELECT JICODE, '경주' GUBUN FROM " + ComNum.DB_PMPA + "BAS_AREA";
                SQL = SQL + ComNum.VBLF + "  WHERE JICODE >= '71' AND JICODE <= '76'";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "  SELECT JICODE, '영천' GUBUN FROM " + ComNum.DB_PMPA + "BAS_AREA";
                SQL = SQL + ComNum.VBLF + "  WHERE JICODE = '77'";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "  SELECT JICODE, '영덕' GUBUN FROM " + ComNum.DB_PMPA + "BAS_AREA";
                SQL = SQL + ComNum.VBLF + "  WHERE JICODE = '78'";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "  SELECT JICODE, '울진' GUBUN FROM " + ComNum.DB_PMPA + "BAS_AREA";
                SQL = SQL + ComNum.VBLF + "  WHERE JICODE = '79'";
                SQL = SQL + ComNum.VBLF + "  Union All";
                SQL = SQL + ComNum.VBLF + "  SELECT JICODE, '그외' GUBUN FROM " + ComNum.DB_PMPA + "BAS_AREA";
                SQL = SQL + ComNum.VBLF + "  WHERE JICODE >= '80') A";
                SQL = SQL + ComNum.VBLF + "  Where a.JICODE = '" + argVar + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                rtnVar = dt.Rows[0]["Gubun"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }

        string Read_FinalDiagnosis(string argPano, string argINDT, string argINTM, string argDisease)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string rtnVar = "";

            rtnVar = argDisease;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT CHARTDATE, CHARTTIME,  decode(FORMNO,'2605',EXTRACTVALUE(CHARTXML, '//ta7'),EXTRACTVALUE(CHARTXML, '//ta4'))  Disease ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMRXML ";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPano.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + " AND writetime >= '" + argINTM.Trim().Replace(":", "") + "00'";
                SQL = SQL + ComNum.VBLF + " AND MEDFRDATE = '" + argINDT.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + " AND FORMNO in ('2074','2224','2276','2605')";

                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT  CHARTDATE, CHARTTIME,  (SELECT ITEMVALUE FROM KOSMOS_EMR.AEMRCHARTROW WHERE EMRNO = A.EMRNO AND ITEMNO = 'I0000014279') AS Disease ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPano.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND writetime >= '" + argINTM.Trim().Replace(":", "") + "00'";
                SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE = '" + argINDT.Replace("-", "") + "'";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO = 2605";
                SQL = SQL + ComNum.VBLF + " ORDER BY CHARTDATE, CHARTTIME ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVar;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnVar;
                }

                if (dt.Rows[0]["Disease"].ToString().Trim() != "")
                {
                    rtnVar = dt.Rows[0]["Disease"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                return rtnVar;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVar;
            }
        }
    }
}
