using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComSupLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-04-05
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= D:\psmh\nurse\nrinfo\FrmHistory2" >> frmHistory2.cs 폼이름 재정의" />

    public partial class frmHistory2 : Form
    {
        public frmHistory2()
        {
            InitializeComponent();
        }

        string FstrFlag = "";
        string mstrPtNo = "";
        string mstrGuBun = "";
        string mstrFDate = "";
        string mstrTDate = "";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strGuBun">1 = 변경 History 2 = 환자별 식이 내역조회</param>
        public frmHistory2(string strPtNo, string strGuBun)
        {
            InitializeComponent();

            mstrPtNo = strPtNo;
            mstrGuBun = strGuBun;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strGuBun">1 = 변경 History 2 = 환자별 식이 내역조회</param>
        /// <param name="strFDate">조회 시작일 yyyy-MM-dd 형식으로 </param>
        /// <param name="strTDate">조회 종료일자 ""으로 들어 올시 현재 일자로 함. 형식은 yyyy-mm-dd</param>
        public frmHistory2(string strPtNo, string strGuBun, string strFDate, string strTDate)
        {
            InitializeComponent();

            mstrPtNo = strPtNo;
            mstrGuBun = strGuBun;
            mstrFDate = strFDate;
            mstrTDate = strTDate;
        }
        
        public frmHistory2(string strFlag, string strPtNo, string strGuBun, string strFDate, string strTDate)
        {
            InitializeComponent();

            FstrFlag = strFlag;
            mstrPtNo = strPtNo;
            mstrGuBun = strGuBun;
            mstrFDate = strFDate;
            mstrTDate = strTDate;
        }


        private void frmHistory2_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            if (FstrFlag == "SUB")
            {
                panTitle.Visible = false;
                panTitleSub0.Visible = false;
            }

            if (mstrPtNo != "")
            {
                txtPano.Text = mstrPtNo;

                if (mstrFDate != "" && VB.IsDate(mstrFDate) == true)
                {
                    TxtFDate.Value = Convert.ToDateTime(mstrFDate);
                }
                else
                {
                    TxtFDate.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                }

                if (mstrTDate != "" && VB.IsDate(mstrTDate) == true)
                {
                    TxtTDate.Value = Convert.ToDateTime(mstrTDate);
                }
                else
                {
                    TxtTDate.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                }
            }
            else
            {
                if (rdo0.Checked == true)
                {
                    TxtFDate.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                }
                else
                {
                    TxtFDate.Text = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-4).ToString("yyyy-MM-dd");
                }

                TxtTDate.Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
            }

            if (mstrGuBun == "1")
            {
                rdo0.Checked = true;
            }
            else if (mstrGuBun == "2")
            {
                rdo1.Checked = true;
            }
            else
            {
                rdo0.Checked = true;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            if (txtPano.Text.Trim() == "")
            {
                return;
            }


            if (rdo0.Checked == true)
            {
                Search();
            }
            else
            {
                Search2();
            }

        }

        private void Search2()
        {
            string strACTDATE = "";
            string strDietDay = "";
            int nRow = 0;
            int nCol = 0;
            string strWardCode = "";
            string strROOMCODE = "";

            //string strDiet1 = "";
            //string strDiet2 = "";
            //string strDiet3 = "";

            int nMaxHight = 0;
            int nHight = 0;

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ss2_Sheet1.Rows.Count = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                strACTDATE = "";
                strDietDay = "";
                //strDiet1 = "";
                //strDiet2 = "";
                //strDiet3 = "";

                SQL = "";
                SQL = " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE , PANO, BI, DEPTCODE, DRCODE, ";
                SQL = SQL + ComNum.VBLF + " WARDCODE, ROOMCODE, DIETCODE, replace(DIETNAME,'(식사 후 식기는 병실에 두세요)','') DIETNAME, SUCODE, DIETDAY,";
                SQL = SQL + ComNum.VBLF + " QTY, UNIT, BUN, ENTDATE, INPUTID, GBSUNAP, PRINT";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.DIET_NEWORDER";
                SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE >= TO_DATE('" + TxtFDate.Text + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND ACTDATE <= TO_DATE('" + TxtTDate.Text + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND PANO ='" + txtPano.Text + "' and ( TRIM(SUCODE) <> '########'  or  DIETCODE <> ('23') ) ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY ACTDATE,DIETDAY,decode(trim(sucode),'########','2','1' ) , BUN ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당하는일자에 식이 오더가 없습니다.");
                    return;
                }


                ss2_Sheet1.Rows.Count = dt.Rows.Count;


                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (strACTDATE != dt.Rows[i]["ACTDATE"].ToString().Trim())
                    {
                        nRow = nRow + 1;
                        strACTDATE = dt.Rows[i]["ACTDATE"].ToString().Trim();
                        strWardCode = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        strROOMCODE = dt.Rows[i]["ROOMCODE"].ToString().Trim();

                        ss2_Sheet1.Cells[nRow - 1, 0].Text = strACTDATE;
                        ss2_Sheet1.Cells[nRow - 1, 1].Text = strWardCode;
                        ss2_Sheet1.Cells[nRow - 1, 2].Text = strROOMCODE;
                    }

                    if (strACTDATE != dt.Rows[i]["ACTDATE"].ToString().Trim() || strDietDay != dt.Rows[i]["DIETDAY"].ToString().Trim())
                    {
                        if (nMaxHight < nHight)
                        {
                            nMaxHight = nHight;
                        }
                        nHight = 0;
                        strDietDay = dt.Rows[i]["DIETDAY"].ToString().Trim();
                    }

                    switch (dt.Rows[i]["DIETDAY"].ToString().Trim())
                    {
                        case "1":
                            nCol = 4;
                            break;
                        case "2":
                            nCol = 5;
                            break;
                        default:
                            nCol = 6;
                            break;
                    }

                    if (ss2_Sheet1.Cells[nRow - 1, nCol - 1].Text == "")
                    {
                        if (VB.Val(dt.Rows[i]["QTY"].ToString().Trim()) > 1)
                        {
                            ss2_Sheet1.Cells[nRow - 1, nCol - 1].Text = dt.Rows[i]["DIETNAME"].ToString().Trim() + ComNum.VBLF + "[ " + dt.Rows[i]["QTY"].ToString().Replace(",", "") + " ]";
                            nHight = nHight + 2;
                        }
                        else
                        {
                            ss2_Sheet1.Cells[nRow - 1, nCol - 1].Text = dt.Rows[i]["DIETNAME"].ToString().Trim();
                        }
                        nHight = nHight + 1;
                    }
                    else
                    {
                        if (VB.Val(dt.Rows[i]["QTY"].ToString().Replace(",", "")) > 1)
                        {
                            ss2_Sheet1.Cells[nRow - 1, nCol - 1].Text = ss2_Sheet1.Cells[nRow - 1, nCol - 1].Text + ComNum.VBLF + dt.Rows[i]["DIETNAME"].ToString().Trim() + ComNum.VBLF + "[ " + dt.Rows[i]["QTY"].ToString().Trim() + " ]";
                            nHight = nHight + 2;
                        }
                        else
                        {
                            ss2_Sheet1.Cells[nRow - 1, nCol - 1].Text = ss2_Sheet1.Cells[nRow - 1, nCol - 1].Text.Trim() 
                                + ComNum.VBLF + dt.Rows[i]["DIETNAME"].ToString().Trim();
                        }
                        nHight = nHight + 1;
                    }
                    ss2_Sheet1.SetRowHeight(nRow - 1, Convert.ToInt32(ss2_Sheet1.GetPreferredRowHeight(nRow - 1) + 10));
                }

                ss2_Sheet1.RowCount = nRow;



                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void Search()
        {
            string strACTDATE = "";
            //string strDietDay = "";
            //string nRow = "";
            //string nCol = "";
            string strWardCode = "";
            string strROOMCODE = "";


            //int nMaxHight = 0;
            //int nHight = 0;

            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            ss1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI:SS') ENTDATE, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE , PANO, BI, DEPTCODE, DRCODE, ";
                SQL = SQL + ComNum.VBLF + " WARDCODE, ROOMCODE, DIETCODE, DIETNAME, SUCODE, DIETDAY,";
                SQL = SQL + ComNum.VBLF + " QTY, UNIT, BUN,  INPUTID, GBSUNAP, PRINT";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "DIET_NEWORDER_HIS";
                SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE >= TO_DATE('" + TxtFDate.Text + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND ACTDATE <= TO_DATE('" + TxtTDate.Text + "' ,'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "    AND PANO ='" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY ENTDATE , ACTDATE,DIETDAY, BUN ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당하는일자에 식이 HISTORY 가 없습니다.");
                    return;
                }


                ss1_Sheet1.Rows.Count = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    //nHight = 0;
                    strACTDATE = dt.Rows[i]["ACTDATE"].ToString().Trim();
                    strWardCode = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    strROOMCODE = dt.Rows[i]["ROOMCODE"].ToString().Trim();

                    ss1_Sheet1.Cells[i, 0].Text = strACTDATE;//일자
                    ss1_Sheet1.Cells[i, 1].Text = strWardCode;
                    ss1_Sheet1.Cells[i, 2].Text = strROOMCODE;

                    switch (dt.Rows[i]["DIETDAY"].ToString().Trim())
                    {
                        case "1":
                            ss1_Sheet1.Cells[i, 3].Text = "아침";
                            break;
                        case "2":
                            ss1_Sheet1.Cells[i, 3].Text = "점심";
                            break;
                        case "3":
                            ss1_Sheet1.Cells[i, 3].Text = "저녁";
                            break;
                    }

                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ENTDATE"].ToString().Trim();
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DIETNAME"].ToString().Trim() + ComNum.VBLF + "[ " + dt.Rows[i]["QTY"].ToString().Replace(",", "") + " ]";

                    if (VB.Val(dt.Rows[i]["QTY"].ToString().Replace(",", "")) > 1)
                    {
                        ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DIETNAME"].ToString().Trim() + ComNum.VBLF + "[ " + dt.Rows[i]["QTY"].ToString().Replace(",", "") + " ]";
                    }
                    else
                    {
                        ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DIETNAME"].ToString().Trim();
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
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            //프린트 버튼

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "( 환자 History ) ";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("등록번호 : " + txtPano.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += CS.setSpdPrint_String("작업일자 : " + TxtFDate.Text + " - " + TxtTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);


            if (rdo0.Checked == true)
            {

                CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else
            {
                CS.setSpdPrint(ss2, PrePrint, setMargin, setOption, strHeader, strFooter);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPano.Text = txtPano.Text.PadLeft(8, '0');	//0 숫자 포맷형식 8자리 채우기
                SendKeys.Send("{Tab}");
            }
        }

        private void rdo_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                rdoCheck();

                btnSearch_Click(null, null);
            }


        }

        private void rdoCheck()
        {
            if (rdo0.Checked == true)
            {
                lblTitle.Text = "변경 History";
                ss1.Visible = true;
                ss1.Dock = DockStyle.Fill;
                ss2.Visible = false;
            }
            else
            {
                lblTitle.Text = "환자별 식이 내역조회";
                ss2.Visible = true;
                ss1.Visible = false;
                ss2.Dock = DockStyle.Fill;
            }
        }
    }
}
