using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMeal.cs
    /// Description     : 식사내역 조회
    /// Author          : 박창욱
    /// Create Date     : 2017-09-05
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\Frm식사내역조회.frm(Frm식사내역조회.frm) >> frmPmpaViewMeal.cs 폼이름 재정의" />	
    public partial class frmPmpaViewMeal : Form
    {
        long nGETcount = 0;
        long GnIPDNO = 0;

        public frmPmpaViewMeal()
        {
            InitializeComponent();
        }

        public frmPmpaViewMeal(long IPDNO)
        {
            InitializeComponent();
            GnIPDNO = IPDNO;
        }

        void SCREEN_CLEAR()
        {
            ssInfo_Sheet1.Cells[0, 0, ssInfo_Sheet1.RowCount - 1, ssInfo_Sheet1.ColumnCount - 1].Text = "";
            ssOrder_Sheet1.Cells[0, 0, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].Text = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            long nIpdNo = 0;
            int nRead = 0;
            string strList = "";

            txtPano.Text = txtPano.Text.Trim();

            if (rdoJob2.Checked == true)
            {
                txtPano.Text = dtpOutDate.Value.ToString("yyyy-MM-dd");
            }

            if (rdoOut2.Checked == true && txtPano.Text == "")
            {
                ComFunc.MsgBox("퇴원환자는 등록번호, 성명 그리고 퇴원일자 중 한 가지를 반드시 입력하셔야 됩니다.");
                return;
            }

            if (rdoJob0.Checked == true)
            {
                if (txtPano.Text != "")
                {
                    txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");
                }
            }



            try
            {
                //환자명단을 SELECT
                SQL = "";
                SQL = "SELECT IPDNO,Pano,SName,DeptCode,RoomCode,GbSTS,Bi,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(InDate,'YYYY-MM-DD') InDate ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                if (rdoOut0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "  AND OutDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + "  AND GbSTS NOT IN ('1','7','9') ";
                    if (rdoJob0.Checked == true && txtPano.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND Pano='" + txtPano.Text + "' ";
                    }
                    if (rdoJob1.Checked == true && txtPano.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND SName LIKE '%" + txtPano.Text + "%' ";
                    }
                }
                else if (rdoOut1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "  AND ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + "  AND GbSTS = '1' ";
                    if (rdoJob0.Checked == true && txtPano.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND Pano='" + txtPano.Text + "' ";
                    }
                    if (rdoJob1.Checked == true && txtPano.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND SName LIKE '%" + txtPano.Text + "%' ";
                    }
                }
                else
                {
                    if (rdoJob0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "  AND Pano = '" + txtPano.Text + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND ActDate IS NOT NULL ";
                    }
                    else if (rdoJob1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "  AND SName = '" + txtPano.Text + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND ActDate IS NOT NULL ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                        SQL = SQL + ComNum.VBLF + "  AND ActDate = TO_DATE('" + txtPano.Text + "','YYYY-MM-DD') ";
                    }
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY SName,Pano ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                ssList_Sheet1.RowCount = 0;
                ssList_Sheet1.RowCount = nRead;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRead; i++)
                {
                    strList = dt.Rows[i]["Pano"].ToString().Trim() + " ";
                    strList += ComFunc.LeftH(dt.Rows[i]["SName"].ToString().Trim() + VB.Space(11), 11);
                    strList += dt.Rows[i]["Bi"].ToString().Trim() + " ";
                    strList += dt.Rows[i]["InDate"].ToString().Trim() + VB.Space(10) + "{}";
                    strList += dt.Rows[i]["IPDNO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 0].Text = strList;
                }

                dt.Dispose();
                dt = null;
                ssList.Focus();

                if (ssList_Sheet1.RowCount == 1)
                {
                    nIpdNo = (long)VB.Val(VB.Pstr(ssList_Sheet1.Cells[0, 0].Text, "{}", 2));
                    Display_IPD_Master(nIpdNo);
                    txtPano.Text = clsPmpaType.IMST.Pano;
                    Food_Slip_View();
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewMeal_Load(object sender, EventArgs e)
        {
            
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            rdoJob2.Enabled = false;
            ssOrder_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            txtPano.Text = "";
            dtpOutDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            if (GnIPDNO != 0)
            {
                Display_IPD_Master(GnIPDNO);
                txtPano.Text = clsPmpaType.IMST.Pano;
                Food_Slip_View();
                GnIPDNO = 0;
            }
        }

        void Display_IPD_Master(long argIPDNO)
        {
            SCREEN_CLEAR();

            //재원마스타를 읽음
            clsIument Iument = new clsIument();
            Iument.Read_Ipd_Master(clsDB.DbCon, "", argIPDNO);

            ssInfo_Sheet1.Cells[0, 0].Text = clsPmpaType.IMST.Pano;
            ssInfo_Sheet1.Cells[0, 1].Text = clsPmpaType.IMST.Sname;
            ssInfo_Sheet1.Cells[0, 2].Text = clsPmpaType.IMST.Age + "/" + clsPmpaType.IMST.Sex;
            ssInfo_Sheet1.Cells[0, 3].Text = clsPmpaType.IMST.WardCode;
            ssInfo_Sheet1.Cells[0, 4].Text = clsPmpaType.IMST.RoomCode.ToString();
            ssInfo_Sheet1.Cells[0, 5].Text = clsPmpaType.IMST.InDate;
            ssInfo_Sheet1.Cells[0, 6].Text = clsPmpaType.IMST.Ilsu.ToString();
            ssInfo_Sheet1.Cells[0, 7].Text = clsPmpaType.IMST.Bi;
            ssInfo_Sheet1.Cells[0, 8].Text = clsPmpaType.IMST.DeptCode;
            ssInfo_Sheet1.Cells[0, 9].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, clsPmpaType.IMST.DrCode);
            ssInfo_Sheet1.Cells[0, 10].Text = clsPmpaType.IMST.IPDNO.ToString();
            ssInfo_Sheet1.Cells[0, 11].Text = "";

        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssList, e.Column);
                return;
            }

            ssList_Sheet1.Cells[0, 0, ssList_Sheet1.RowCount - 1, ssList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssList_Sheet1.Cells[e.Row, 0, e.Row, ssList_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            long nIpdNo = 0;

            nIpdNo = (long)VB.Val(VB.Pstr(ssList_Sheet1.Cells[e.Row, e.Column].Text, "{}", 2));
            Display_IPD_Master(nIpdNo);
            txtPano.Text = clsPmpaType.IMST.Pano;
            Food_Slip_View();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPano.Text = "";
            SCREEN_CLEAR();
            txtPano.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void Food_Slip_View()
        {
            string strNujuk = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAmt1 = "";
            string strAmt2 = "";
            string strCFdate = "";
            string strBDate = "";

            int nNu = 0;
            int nNuChk = 0;
            int i = 0;
            int nRow = 0;
            int nRead = 0;

            long nAmt1 = 0;
            long nAmt2 = 0;
            long nStot1 = 0;
            long nStot2 = 0;
            long nGtot1 = 0;
            long nGtot2 = 0;

            ssOrder_Sheet1.RowCount = 0;
            ssOrder_Sheet1.RowCount = 1;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(Bdate, 'yyyy-mm-dd') Bday,";
                SQL = SQL + ComNum.VBLF + "        Bun,Nu,Sucode,SunameK,BaseAmt,Qty,Nal,";
                SQL = SQL + ComNum.VBLF + "        GbSpc,GbNgt,GbGisul,GbSelf,GbChild,Amt1,Amt2,Part";
                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP I,  " + ComNum.DB_PMPA + "BAS_SUN B";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP I,  " + ComNum.DB_PMPA + "BAS_SUN B";
                }
                SQL = SQL + ComNum.VBLF + "  WHERE I.IPDNO = " + clsPmpaType.IMST.IPDNO + " ";
                SQL = SQL + ComNum.VBLF + "    AND I.Bun = '74' ";
                SQL = SQL + ComNum.VBLF + "    AND I.Sunext = B.Sunext(+) ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY I.Bdate,I.Sucode,I.Sunext ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nRead = dt.Rows.Count;
                nGETcount += nRead;

                for (i = 0; i < nRead; i++)
                {
                    nNu = (int)VB.Val(dt.Rows[i]["Bun"].ToString().Trim());

                    if (nNuChk == 0)
                    {
                        nNuChk = nNu;
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                        strNujuk = "식대";
                    }

                    if (nNu != nNuChk)
                    {
                        SUB_TOT_FOOD(ref nRow, ref nStot1, ref nStot2);
                        nNuChk = nNu;
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                        strNujuk = "식대";
                    }

                    if (strCFdate != dt.Rows[i]["Bday"].ToString().Trim())
                    {
                        strCFdate = dt.Rows[i]["Bday"].ToString().Trim();
                        strBDate = dt.Rows[i]["Bday"].ToString().Trim();
                    }

                    #region GoSub DATA_MOVE_FOOD

                    nRow += 1;
                    if (ssOrder_Sheet1.RowCount < nRow)
                    {
                        ssOrder_Sheet1.RowCount = nRow;
                    }
                    nAmt1 = (long)VB.Val(dt.Rows[i]["Amt1"].ToString().Trim());
                    nAmt2 = (long)VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());
                    nStot1 += nAmt1;
                    nStot2 += nAmt2;
                    nGtot1 += nAmt1;
                    nGtot2 += nAmt2;

                    strBaseAmt = VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("##,###,##0");
                    strQty = VB.Val(dt.Rows[i]["Qty"].ToString().Trim()).ToString("##0.00");
                    strNal = VB.Val(dt.Rows[i]["Nal"].ToString().Trim()).ToString("##0");
                    strAmt1 = nAmt1.ToString("###,###,##0");
                    strAmt2 = nAmt2.ToString("###,###,##0");

                    ssOrder_Sheet1.Cells[nRow - 1, 0].Text = strNujuk;
                    ssOrder_Sheet1.Cells[nRow - 1, 1].Text = strBDate;
                    ssOrder_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                    ssOrder_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                    ssOrder_Sheet1.Cells[nRow - 1, 4].Text = strBaseAmt;
                    ssOrder_Sheet1.Cells[nRow - 1, 5].Text = strQty;
                    ssOrder_Sheet1.Cells[nRow - 1, 6].Text = strNal;
                    ssOrder_Sheet1.Cells[nRow - 1, 7].Text = strAmt1;
                    ssOrder_Sheet1.Cells[nRow - 1, 8].Text = strAmt2;
                    ssOrder_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["Part"].ToString().Trim();
                    ssOrder_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                    ssOrder_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                    ssOrder_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                    ssOrder_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    ssOrder_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                    #endregion

                    strNujuk = "";
                    strBDate = "";
                }
                dt.Dispose();
                dt = null;

                if (nGETcount > 0)
                {
                    SUB_TOT_FOOD(ref nRow, ref nStot1, ref nStot2);

                    #region GoSub GRAND_TOT_FOOD

                    nRow += 1;
                    if (ssOrder_Sheet1.RowCount < nRow)
                    {
                        ssOrder_Sheet1.RowCount = nRow;
                    }

                    ssOrder_Sheet1.Cells[nRow - 1, 0, nRow - 1, ssOrder_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(128, 255, 128);

                    strAmt1 = nGtot1.ToString("###,###,##0");
                    strAmt2 = nGtot2.ToString("###,###,##0");

                    ssOrder_Sheet1.Cells[nRow - 1, 1].Text = "전체합계";
                    ssOrder_Sheet1.Cells[nRow - 1, 7].Text = strAmt1;
                    ssOrder_Sheet1.Cells[nRow - 1, 8].Text = strAmt2;
                    nGtot1 = 0;
                    nGtot2 = 0;

                    #endregion

                    btnPrint.Enabled = true;
                    ssOrder.Enabled = true;
                }
                ssOrder_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void SUB_TOT_FOOD(ref int nRow, ref long nStot1, ref long nStot2)
        {
            string strAmt1 = "";
            string strAmt2 = "";

            nRow += 1;
            if (ssOrder_Sheet1.RowCount < nRow)
            {
                ssOrder_Sheet1.RowCount = nRow;
            }

            ssOrder_Sheet1.Cells[nRow - 1, 0, nRow - 1, ssOrder_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(128, 255, 255);

            strAmt1 = nStot1.ToString("###,###,##0");
            strAmt2 = nStot2.ToString("###,###,##0");

            ssOrder_Sheet1.Cells[nRow - 1, 1].Text = "누적별계";
            ssOrder_Sheet1.Cells[nRow - 1, 7].Text = strAmt1;
            ssOrder_Sheet1.Cells[nRow - 1, 8].Text = strAmt2;
            nStot1 = 0;
            nStot2 = 0;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string strFoot = "";

            //Print Head
            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/f1" + VB.Space(18) + "일 자 별  식 사 내 역" + "/n/n";
            strHead2 = "/l/f2" + "등록번호: " + clsPmpaType.IMST.Pano + " 수진자명:" + clsPmpaType.IMST.Sname + VB.Space(4);
            strHead2 = strHead2 + "환자종류: " + clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", clsPmpaType.IMST.Bi) + VB.Space(4);
            strHead2 = strHead2 + "진료과: " + clsPmpaType.IMST.DeptCode + " " + clsVbfunc.GetBASDoctorName(clsDB.DbCon, clsPmpaType.IMST.DrCode) + "/n";
            strHead2 = strHead2 + "출력기간: " + clsPmpaType.IMST.InDate + " ~ " + clsPmpaType.IMST.OutDate + VB.Space(10);
            strHead2 = strHead2 + "출력일자: " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");
            //Print Flooter  지정
            strFoot = "/l/f2" + "작성자 : " + clsType.User.JobName + VB.Space(30) + "PAGE : " + "/p";

            //Print Body
            ssOrder_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssOrder_Sheet1.PrintInfo.Footer = strFont2 + strFoot;
            ssOrder_Sheet1.PrintInfo.Margin.Left = 0;
            ssOrder_Sheet1.PrintInfo.Margin.Right = 0;
            ssOrder_Sheet1.PrintInfo.Margin.Top = 5;
            ssOrder_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssOrder_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssOrder_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssOrder_Sheet1.PrintInfo.ShowBorder = true;
            ssOrder_Sheet1.PrintInfo.ShowColor = true;
            ssOrder_Sheet1.PrintInfo.ShowGrid = false;
            ssOrder_Sheet1.PrintInfo.ShowShadows = false;
            ssOrder_Sheet1.PrintInfo.UseMax = true;
            ssOrder_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssOrder.PrintSheet(0);
        }

        private void rdoJob_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoJob0.Checked == true)
            {
                grbPano.Text = "등록번호";
                txtPano.Text = "";
                txtPano.Visible = true;
                dtpOutDate.Visible = false;
            }
            else if (rdoJob1.Checked == true)
            {
                grbPano.Text = "환자성명";
                txtPano.Text = "";
                txtPano.Visible = true;
                dtpOutDate.Visible = false;
            }
            else if (rdoJob2.Checked == true)
            {
                grbPano.Text = "퇴원일자";
                txtPano.Text = "";
                txtPano.Visible = false;
                dtpOutDate.Visible = true;
            }
        }

        private void rdoOut_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOut2.Checked == true)
            {
                lblJewon.Text = "퇴원환자";
                rdoJob2.Enabled = true;
                rdoJob0.Checked = true;
            }
            else
            {
                if (rdoOut0.Checked == true)
                {
                    lblJewon.Text = "재원환자";
                }
                if (rdoOut1.Checked == true)
                {
                    lblJewon.Text = "가퇴원";
                }
                if (rdoJob2.Checked == true)
                {
                    rdoJob0.Checked = true;
                }
                rdoJob2.Enabled = false;
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
            }
        }
    }
}
