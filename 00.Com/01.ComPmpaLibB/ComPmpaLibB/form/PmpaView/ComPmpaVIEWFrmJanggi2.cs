using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class ComPmpaVIEWFrmJanggi2 : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : ComPmpaVIEWFrmJanggi2.cs
        /// Description     : 진료과장별 장기입원자 현황
        /// Author          : 김효성
        /// Create Date     : 2017-09-11
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "\psmh\IPD\iviewa\IVIEWAH.FRM  >> ComPmpaVIEWFrmJanggi2.cs 폼이름 재정의" />	
        public ComPmpaVIEWFrmJanggi2()
        {
            InitializeComponent();
        }

        private void ComPmpaVIEWFrmJanggi2_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{ this.Close(); return; } //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssView_Sheet1.SetColumnMerge(0, FarPoint.Win.Spread.Model.MergePolicy.Always);
            ssView_Sheet1.SetColumnMerge(1, FarPoint.Win.Spread.Model.MergePolicy.Always);

            txtDay.Text = "100";
            lblDays.Text = "0 명";
            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            cboJong.Items.Add("01. 보험");
            cboJong.Items.Add("02. 보호");
            cboJong.Items.Add("03. 산재");
            cboJong.Items.Add("04. 자보");
            cboJong.Items.Add("05. 보험+보호");
            cboJong.SelectedIndex = 0;

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            int nIlsu = 0;
            string strOldDept = "";
            string strNewDept = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            nIlsu = Convert.ToInt32(VB.Val(txtDay.Text));

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.DEPTCODE, A.DRCODE, B.DRNAME, d.ROOMCODE,A.BOHUN,                        ";
                SQL = SQL + ComNum.VBLF + "        A.PANO, d.SNAME, A.ILSU, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE,            ";
                SQL = SQL + ComNum.VBLF + "        d.AGE, d.SEX, A.BI, A.AMT50, A.AMT51+A.AMT52 SUMAMT3, A.AMT55,  A.AMT53,   ";
                SQL = SQL + ComNum.VBLF + "        A.AMT01+A.AMT02+A.AMT03+A.AMT04+A.AMT05+A.AMT06+A.AMT07+A.AMT08+A.AMT09    ";
                SQL = SQL + ComNum.VBLF + "        +A.AMT10+A.AMT11+A.AMT12+A.AMT13+A.AMT14+A.AMT15+A.AMT16+A.AMT17+A.AMT18   ";
                SQL = SQL + ComNum.VBLF + "        +A.AMT19+AMT20 SUMAMT1, A.AMT21+A.AMT22+A.AMT23+A.AMT24+A.AMT25+A.AMT26    ";
                SQL = SQL + ComNum.VBLF + "        +A.AMT27+A.AMT28+A.AMT29+A.AMT30+A.AMT31+A.AMT32+A.AMT33+A.AMT34+A.AMT35   ";
                SQL = SQL + ComNum.VBLF + "        +A.AMT36+A.AMT37+A.AMT38+A.AMT39+A.AMT40+A.AMT41+A.AMT42+A.AMT43+A.AMT44   ";
                SQL = SQL + ComNum.VBLF + "        +A.AMT45+A.AMT46+A.AMT47+A.AMT48+A.AMT49 SUMAMT2                           ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS A, " + ComNum.DB_PMPA + "BAS_DOCTOR B, " + ComNum.DB_PMPA + "bas_clinicdept c, " + ComNum.DB_PMPA + "IPD_NEW_MASTER  D             ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1	                                                                ";
                SQL = SQL + ComNum.VBLF + "    AND A.GBIPD='1'                                                                ";
                SQL = SQL + ComNum.VBLF + "    AND A.IPDNO = D.IPDNO                                                          ";
                SQL = SQL + ComNum.VBLF + "    AND A.GBSTS IN ('0','2')                                                       ";
                SQL = SQL + ComNum.VBLF + "    AND D.OUTDATE IS NULL                                                          ";
                SQL = SQL + ComNum.VBLF + "    AND A.PANO <> '81000004'                                                       ";
                if (VB.Left(cboJong.Text, 2) == "01")
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.BI  < '20'                          ";
                }
                if (VB.Left(cboJong.Text, 2) == "02")
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.BI  >= '20'  AND A.BI < '30'        ";
                }
                if (VB.Left(cboJong.Text, 2) == "03")
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.BI  = '31'                          ";
                }
                if (VB.Left(cboJong.Text, 2) == "04")
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.BI  = '52'                          ";
                }
                if (VB.Left(cboJong.Text, 2) == "05")
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.BI  < '30'                           ";
                }

                if (nIlsu != 0)
                {
                    SQL = SQL + ComNum.VBLF + "    AND d.ILSU >= '" + nIlsu + "'                               ";
                }

                SQL = SQL + ComNum.VBLF + "    AND A.DRCODE   = B.DRCODE                                                      ";
                SQL = SQL + ComNum.VBLF + "    AND A.DEPTCODE = C.DEPTCODE                                                    ";

                if (rdo0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY C.PRINTRANKING,B.PRINTRANKING                ";
                }
                if (rdo1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY d.ROOMCODE,C.PRINTRANKING,B.PRINTRANKING     ";
                }
                if (rdo2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.INDATE,C.PRINTRANKING,B.PRINTRANKING       ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                strOldDept = dt.Rows[0]["deptcode"].ToString().Trim();
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.Cells[0, 0].Text = dt.Rows[0]["deptcode"].ToString().Trim();

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strNewDept = dt.Rows[i]["Deptcode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["deptcode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["drname"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Roomcode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["pano"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ilsu"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["indate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["SEX"].ToString().Trim();

                    switch (dt.Rows[i]["bi"].ToString().Trim())
                    {
                        case "11":
                        case "12":
                        case "13":
                        case "14":
                        case "15":
                        case "41":
                        case "42":
                        case "43":
                        case "44":
                        case "45":
                            ssView_Sheet1.Cells[i, 8].Text = "의료보험";
                            break;
                        case "21":
                            ssView_Sheet1.Cells[i, 8].Text = "보호1종";
                            break;
                        case "22":
                            ssView_Sheet1.Cells[i, 8].Text = "보호2종";
                            break;
                        case "23":
                            ssView_Sheet1.Cells[i, 8].Text = "보호3종";
                            break;
                        case "31":
                        case "32":
                            ssView_Sheet1.Cells[i, 8].Text = "산재";
                            break;
                        case "52":
                            ssView_Sheet1.Cells[i, 8].Text = "자보";
                            break;
                        case "53":
                        case "33":
                            ssView_Sheet1.Cells[i, 8].Text = "계약처";
                            break;
                        case "51":
                            ssView_Sheet1.Cells[i, 8].Text = "일반";
                            break;
                        default:
                            ssView_Sheet1.Cells[i, 8].Text = "기타";
                            break;
                    }
                    ssView_Sheet1.Cells[i, 9].Text = Convert.ToDouble(dt.Rows[i]["amt50"].ToString().Trim()).ToString("###,###,###,##0");

                    switch (dt.Rows[i]["bi"].ToString().Trim())
                    {
                        case "11":
                        case "12":
                        case "13":
                        case "23":
                            ssView_Sheet1.Cells[i, 10].Text = (Convert.ToDouble(dt.Rows[i]["SUMAmt1"].ToString().Trim()) * 0.2 + Convert.ToDouble(dt.Rows[i]["Sumamt2"].ToString().Trim())).ToString("###,###,###,##0");
                            break;
                        case "22":
                            if (dt.Rows[i]["Bohun"].ToString().Trim() == "3")
                            {
                                ssView_Sheet1.Cells[i, 10].Text = Convert.ToDouble(dt.Rows[i]["SumAmt2"].ToString().Trim()).ToString("###,###,###,##0");
                            }
                            else
                            {
                                ssView_Sheet1.Cells[i, 10].Text = (Convert.ToDouble(dt.Rows[i]["SUMAmt1"].ToString().Trim()) * 0.2 + Convert.ToDouble(dt.Rows[i]["Sumamt2"].ToString().Trim())).ToString("###,###,###,##0");
                            }
                            break;
                        case "21":
                        case "24":
                        case "31":
                        case "32":
                        case "44":
                        case "52":
                            ssView_Sheet1.Cells[i, 10].Text = Convert.ToDouble(dt.Rows[i]["SumAmt2"].ToString().Trim()).ToString("###,###,###,##0");
                            break;
                        default:
                            ssView_Sheet1.Cells[i, 10].Text = (Convert.ToDouble(dt.Rows[i]["SUMAmt1"].ToString().Trim()) + Convert.ToDouble(dt.Rows[i]["Sumamt2"].ToString().Trim())).ToString("###,###,###,##0");
                            break;
                    }

                    ssView_Sheet1.Cells[i, 11].Text = Convert.ToDouble(dt.Rows[i]["SumAmt3"].ToString().Trim()).ToString("###,###,###,##0");

                    switch (dt.Rows[i]["bi"].ToString().Trim())
                    {
                        case "11":
                        case "12":
                        case "13":
                        case "23":
                            ssView_Sheet1.Cells[i, 12].Text = (Convert.ToDouble(dt.Rows[i]["SUMAmt1"].ToString().Trim()) * 0.2 + Convert.ToDouble(dt.Rows[i]["Sumamt2"].ToString().Trim()) - Convert.ToDouble(dt.Rows[i]["Sumamt3"].ToString().Trim())).ToString("###,###,###,##0");
                            break;
                        case "22":
                            if (dt.Rows[i]["Bohun"].ToString().Trim() == "3")
                            {
                                ssView_Sheet1.Cells[i, 12].Text = (Convert.ToDouble(dt.Rows[i]["SumAmt2"].ToString().Trim()) - Convert.ToDouble(dt.Rows[i]["Sumamt3"].ToString().Trim())).ToString("###,###,###,##0");
                            }
                            else
                            {
                                ssView_Sheet1.Cells[i, 12].Text = (Convert.ToDouble(dt.Rows[i]["SUMAmt1"].ToString().Trim()) * 0.2 + Convert.ToDouble(dt.Rows[i]["Sumamt2"].ToString().Trim()) - Convert.ToDouble(dt.Rows[i]["Sumamt3"].ToString().Trim())).ToString("###,###,###,##0");
                            }
                            break;
                        case "21":
                        case "24":
                        case "31":
                        case "32":
                        case "44":
                        case "52":
                            ssView_Sheet1.Cells[i, 12].Text = (Convert.ToDouble(dt.Rows[i]["SumAmt2"].ToString().Trim()) - Convert.ToDouble(dt.Rows[i]["Sumamt3"].ToString().Trim())).ToString("###,###,###,##0");
                            break;
                        default:
                            ssView_Sheet1.Cells[i, 12].Text = (Convert.ToDouble(dt.Rows[i]["SUMAmt1"].ToString().Trim()) + Convert.ToDouble(dt.Rows[i]["Sumamt2"].ToString().Trim()) - Convert.ToDouble(dt.Rows[i]["Sumamt3"].ToString().Trim())).ToString("###,###,###,##0");
                            break;
                    }
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                ssView_Sheet1.RowCount = i;
                lblDays.Text = i + " 명";
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }
            ssView_Sheet1.RowCount = 0;
            lblDays.Text = "0 명";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";
            string sFont3 = "";
            string sFoot = "";

            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            strFont1 = "/c/fn\"굴림체\" /fz\"16\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/c/f1" + "진료과장별 장기입원환자 현황" + "/f1/n";   //제목 센터
            strHead2 = "/l/f2" + "재원일수: " + VB.Val(txtDay.Text) + " 일 이상" + cboJong.Text + "/f2";
            strHead2 = strHead2 = "/n/l/f2" + VB.Space(10) + "작업일자 : " + mdtp + VB.Space(3) + "Page: " + "/p";
            btnPrint.Enabled = false;

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2 + "/n";
            ssView_Sheet1.PrintInfo.Footer = sFont3 + sFoot;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
