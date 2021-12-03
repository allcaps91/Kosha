using System;
using System.Data;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComPmpaLibB
{
    public partial class frmPmpaDrgResearch : Form
    {
        private string FstrROWID = "";
        private string FstrRowid2 = "";
        private string FstrTRSNO = "";
        private string FstrIPDNO = "";
        private string FstrPano = "";        

        public frmPmpaDrgResearch()
        {
            InitializeComponent();
        }

        private void frmPmpaDrgResearch_Load(object sender, EventArgs e)
        {
            Screen_Clear();
            List_View();
        }

        private void Screen_Clear()
        {
            string strTemp = "";

            strTemp = clsPublic.GnJobSabun.ToString();

            FstrTRSNO = "";
            FstrIPDNO = "";

            //기본정보
            txtPName.Text = "";
            dtpInDate.Text = "";
            dtpOutDate.Text = "";
            dtpOpDate.Text = "";
            txtILLCode1.Text = "";
            txtILLCode2.Text = "";
            txtILLCode3.Text = "";
            txtILLCode4.Text = "";

            //1. 수술 전 진료의 점검 사항
            rdo1_1_0.Checked = false;
            rdo1_1_1.Checked = false;
            rdo1_1_1_1.Checked = false;
            rdo1_1_1_2.Checked = false;
            rdo1_1_1_3.Checked = false;
            rdo1_1_0.Checked = true;

            //2. 입원 중 진료의 점검 사항
            rdo2_1_1_0.Checked = false;
            rdo2_1_1_1.Checked = false;
            rdo2_1_2_0.Checked = false;
            rdo2_1_2_1.Checked = false;
            rdo2_1_3_0.Checked = false;
            rdo2_1_3_1.Checked = false;
            rdo2_1_4_0.Checked = false;
            rdo2_1_4_1.Checked = false;
            txtOpt2_1.Text = "";
            rdo2_2_0.Checked = false;
            rdo2_2_1.Checked = false;
            rdo2_3_0.Checked = false;
            rdo2_3_1.Checked = false;
            txtOpt2_3.Text = "";
            rdo2_4_0.Checked = false;
            rdo2_4_1.Checked = false;

            //3. 퇴원 전 진료의 점검 사항
            rdo3_1_0.Checked = false;
            rdo3_1_1.Checked = false;
            rdo3_1_1_1.Checked = false;
            rdo3_1_1_2.Checked = false;
            rdo3_1_1_3.Checked = false;
            rdo3_1_1_4.Checked = false;
            rdo3_1_0.Checked = true;

            rdo3_2_1_0.Checked = false;
            rdo3_2_1_1.Checked = false;
            rdo3_2_2_0.Checked = false;
            rdo3_2_2_1.Checked = false;
            rdo3_2_3_0.Checked = false;
            rdo3_2_3_1.Checked = false;
            rdo3_2_4_0.Checked = false;
            rdo3_2_4_1.Checked = false;
            rdo3_2_5_0.Checked = false;
            rdo3_2_5_1.Checked = false;
            
            dtpLsDate.Text = clsPublic.GstrSysDate;                //작성일
            txtEntName.Text = "";

        }

        private void List_View()
        {
            int nREAD = 0;

            DataTable Dt = null;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

         //   clsPublic.GstrPANO = "09697165";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT  TO_CHAR(a.INDATE, 'YYYY-MM-DD') INDATE, TO_CHAR(a.OUTDATE, 'YYYY-MM-DD') OUTDATE,    ";
                SQL += ComNum.VBLF + "         b.SNAME, a.DEPTCODE, a.ROWID ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS a, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_MASTER b ";
                SQL += ComNum.VBLF + " WHERE a.Pano =b.Pano(+) ";
                SQL += ComNum.VBLF + "   AND a.IPDNO =b.IPDNO(+) ";
                SQL += ComNum.VBLF + "   AND a.PANO = '" + clsPublic.GstrPANO + "' ";
                SQL += ComNum.VBLF + "   AND a.GbDRG ='D' ";  //DRG만
                SQL += ComNum.VBLF + " ORDER BY a.INDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                //점검표 List
                if (nREAD == 1)
                {
                    clsDB.DataTableToSpdRow(Dt, SS5, 0);
                    FstrROWID = Dt.Rows[0]["ROWID"].ToString().Trim();
                }
                else if (Dt.Rows.Count > 1)
                {
                    clsDB.DataTableToSpdRow(Dt, SS5, 0);
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

        private void rdo1_1_0_CheckedChanged(object sender, EventArgs e)
        {
            pan1_1.Enabled = false;
            rdo1_1_1_1.Checked = false;
            rdo1_1_1_1.Enabled = false;
            rdo1_1_1_2.Checked = false;
            rdo1_1_1_2.Enabled = false;
            rdo1_1_1_3.Checked = false;
            rdo1_1_1_3.Enabled = false;
        }

        private void rdo1_1_1_CheckedChanged(object sender, EventArgs e)
        {
            pan1_1.Enabled = true;
            rdo1_1_1_1.Enabled = true;
            rdo1_1_1_2.Enabled = true;
            rdo1_1_1_3.Enabled = true;
        }

        private void rdo3_1_0_CheckedChanged(object sender, EventArgs e)
        {
            pan3_1.Enabled = false;
            rdo3_1_1_1.Checked = false;
            rdo3_1_1_1.Enabled = false;
            rdo3_1_1_2.Checked = false;
            rdo3_1_1_2.Enabled = false;
            rdo3_1_1_3.Checked = false;
            rdo3_1_1_3.Enabled = false;
            rdo3_1_1_4.Checked = false;
            rdo3_1_1_4.Enabled = false;
        }

        private void rdo3_1_1_CheckedChanged(object sender, EventArgs e)
        {
            pan3_1.Enabled = true;
            rdo3_1_1_1.Enabled = true;
            rdo3_1_1_2.Enabled = true;
            rdo3_1_1_3.Enabled = true;
            rdo3_1_1_4.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            rdo1_1_1.Checked = true;
            rdo1_1_1_1.Checked = true;

            rdo2_1_1_0.Checked = true;
            rdo2_1_2_0.Checked = true;
            rdo2_1_3_0.Checked = true;
            rdo2_1_4_0.Checked = true;
            rdo2_2_0.Checked = true;
            rdo2_3_0.Checked = true;
            rdo2_4_0.Checked = true;
            
            rdo3_1_0.Checked = true;
            rdo3_2_1_0.Checked = true;
            rdo3_2_2_0.Checked = true;
            rdo3_2_3_0.Checked = true;
            rdo3_2_4_0.Checked = true;
            rdo3_2_5_0.Checked = true;
        }

        private void SS5_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            FstrROWID = SS5_Sheet1.Cells[e.Row, 4].Text.ToString();

            Select_View(FstrROWID);
        }

        private void Select_View(string strRowid)
        {
            int nREAD = 0;

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Screen_Clear();

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT  a.TRSNO, a.IPDNO, a.PANO, b.SNAME, a.DEPTCODE, a.ROWID, ";
                SQL += ComNum.VBLF + "         TO_CHAR(a.INDATE, 'YYYY-MM-DD') INDATE, TO_CHAR(a.OUTDATE, 'YYYY-MM-DD') OUTDATE,    ";
                SQL += ComNum.VBLF + "         a.ILLCODE1, a.ILLCODE2, a.ILLCODE3, a.ILLCODE4 ";
                SQL += ComNum.VBLF + "   FROM  " + ComNum.DB_PMPA + "IPD_TRANS a, ";
                SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "IPD_NEW_MASTER b ";
                SQL += ComNum.VBLF + " WHERE a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + " AND a.IPDNO=b.IPDNO(+) ";
                SQL += ComNum.VBLF + " AND a.ROWID = '" + FstrROWID + "' ";
                SQL += ComNum.VBLF + " ORDER BY INDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;

                //기본정보
                if (nREAD > 0)
                {
                    txtPName.Text = Dt.Rows[0]["SNAME"].ToString().Trim();
                    dtpInDate.Text = Dt.Rows[0]["INDATE"].ToString().Trim();
                    dtpOutDate.Text = Dt.Rows[0]["OUTDATE"].ToString().Trim();
                    txtILLCode1.Text = Dt.Rows[0]["ILLCODE1"].ToString().Trim();
                    txtILLCode2.Text = Dt.Rows[0]["ILLCODE2"].ToString().Trim();
                    txtILLCode3.Text = Dt.Rows[0]["ILLCODE3"].ToString().Trim();
                    txtILLCode4.Text = Dt.Rows[0]["ILLCODE4"].ToString().Trim();
                    FstrTRSNO = Dt.Rows[0]["TRSNO"].ToString().Trim();
                    FstrIPDNO = Dt.Rows[0]["IPDNO"].ToString().Trim();
                    FstrPano = Dt.Rows[0]["PANO"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;


                //점검표
                SQL = "";
                SQL += ComNum.VBLF + " SELECT  TRSNO, IPDNO, PANO, PNAME, ";
                SQL += ComNum.VBLF + "         OPT110, OPT110A, OPT211, OPT212, OPT213, OPT214, OPT214A, ";
                SQL += ComNum.VBLF + "         OPT220, OPT230, OPT230A, OPT240, OPT310, OPT310A, OPT321, ";
                SQL += ComNum.VBLF + "         OPT322, OPT323, OPT324, OPT325, TO_CHAR(LSDATE, 'YYYY-MM-DD') LSDATE, ENTSABUN, ENTNAME ,";
                SQL += ComNum.VBLF + "          TO_CHAR(OPDATE,'YYYY-MM-DD') OPDATE ";
                SQL += ComNum.VBLF + "   FROM  " + ComNum.DB_PMPA + "DRG_CHART1 ";
                SQL += ComNum.VBLF + "  WHERE  TRSNO = '" + FstrTRSNO + "' ";
                SQL += ComNum.VBLF + "    AND  IPDNO = '" + FstrIPDNO + "' ";
                SQL += ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='') ";
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
                    dtpOpDate.Text = Dt.Rows[0]["OPDATE"].ToString().Trim();

                    //1.1.
                    if (Dt.Rows[0]["OPT110"].ToString().Trim() == "10")
                    {
                        rdo1_1_0.Checked = true;
                        rdo1_1_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT110"].ToString().Trim() == "01")
                    {
                        rdo1_1_0.Checked = false;
                        rdo1_1_1.Checked = true;
                    }
                    //1.1. 추가 코드
                    if (Dt.Rows[0]["OPT110A"].ToString().Trim() == "1")
                    {
                        rdo1_1_1_1.Checked = true;
                        rdo1_1_1_2.Checked = false;
                        rdo1_1_1_3.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT110A"].ToString().Trim() == "2")
                    {
                        rdo1_1_1_1.Checked = false;
                        rdo1_1_1_2.Checked = true;
                        rdo1_1_1_3.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT110A"].ToString().Trim() == "3")
                    {
                        rdo1_1_1_1.Checked = false;
                        rdo1_1_1_2.Checked = false;
                        rdo1_1_1_3.Checked = true;
                    }
                    //2.1. 1)
                    if (Dt.Rows[0]["OPT211"].ToString().Trim() == "10")
                    {
                        rdo2_1_1_0.Checked = true;
                        rdo2_1_1_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT211"].ToString().Trim() == "01")
                    {
                        rdo2_1_1_0.Checked = false;
                        rdo2_1_1_1.Checked = true;
                    }
                    //2.1. 2)
                    if (Dt.Rows[0]["OPT212"].ToString().Trim() == "10")
                    {
                        rdo2_1_2_0.Checked = true;
                        rdo2_1_2_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT212"].ToString().Trim() == "01")
                    {
                        rdo2_1_2_0.Checked = false;
                        rdo2_1_2_1.Checked = true;
                    }
                    //2.1. 3)
                    if (Dt.Rows[0]["OPT213"].ToString().Trim() == "10")
                    {
                        rdo2_1_3_0.Checked = true;
                        rdo2_1_3_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT213"].ToString().Trim() == "01")
                    {
                        rdo2_1_3_0.Checked = false;
                        rdo2_1_3_1.Checked = true;
                    }
                    //2.1. 4)
                    if (Dt.Rows[0]["OPT214"].ToString().Trim() == "10")
                    {
                        rdo2_1_4_0.Checked = true;
                        rdo2_1_4_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT214"].ToString().Trim() == "01")
                    {
                        rdo2_1_4_0.Checked = false;
                        rdo2_1_4_1.Checked = true;
                    }
                    //2.1. 4) 추가 코드
                    txtOpt2_1.Text = Dt.Rows[0]["OPT214A"].ToString().Trim();

                    //2.2.
                    if (Dt.Rows[0]["OPT220"].ToString().Trim() == "10")
                    {
                        rdo2_2_0.Checked = true;
                        rdo2_2_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT220"].ToString().Trim() == "01")
                    {
                        rdo2_2_0.Checked = false;
                        rdo2_2_1.Checked = true;
                    }
                    //2.3.
                    if (Dt.Rows[0]["OPT230"].ToString().Trim() == "10")
                    {
                        rdo2_3_0.Checked = true;
                        rdo2_3_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT230"].ToString().Trim() == "01")
                    {
                        rdo2_3_0.Checked = false;
                        rdo2_3_1.Checked = true;
                    }
                    //2.3. 추가 코드
                    txtOpt2_3.Text = Dt.Rows[0]["OPT230A"].ToString().Trim();

                    //2.4.
                    if (Dt.Rows[0]["OPT240"].ToString().Trim() == "10")
                    {
                        rdo2_4_0.Checked = true;
                        rdo2_4_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT240"].ToString().Trim() == "01")
                    {
                        rdo2_4_0.Checked = false;
                        rdo2_4_1.Checked = true;
                    }
                    //3.1.
                    if (Dt.Rows[0]["OPT310"].ToString().Trim() == "10")
                    {
                        rdo3_1_0.Checked = true;
                        rdo3_1_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT310"].ToString().Trim() == "01")
                    {
                        rdo3_1_0.Checked = false;
                        rdo3_1_1.Checked = true;
                    }
                    //3.1. 추가 코드
                    if (Dt.Rows[0]["OPT310A"].ToString().Trim() == "1")
                    {
                        rdo3_1_1_1.Checked = true;
                        rdo3_1_1_2.Checked = false;
                        rdo3_1_1_3.Checked = false;
                        rdo3_1_1_4.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT310A"].ToString().Trim() == "2")
                    {
                        rdo3_1_1_1.Checked = false;
                        rdo3_1_1_2.Checked = true;
                        rdo3_1_1_3.Checked = false;
                        rdo3_1_1_4.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT310A"].ToString().Trim() == "3")
                    {
                        rdo3_1_1_1.Checked = false;
                        rdo3_1_1_2.Checked = false;
                        rdo3_1_1_3.Checked = true;
                        rdo3_1_1_4.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT310A"].ToString().Trim() == "4")
                    {
                        rdo3_1_1_1.Checked = false;
                        rdo3_1_1_2.Checked = false;
                        rdo3_1_1_3.Checked = false;
                        rdo3_1_1_4.Checked = true;
                    }
                    //3.2. 1)
                    if (Dt.Rows[0]["OPT321"].ToString().Trim() == "10")
                    {
                        rdo3_2_1_0.Checked = true;
                        rdo3_2_1_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT321"].ToString().Trim() == "01")
                    {
                        rdo3_2_1_0.Checked = false;
                        rdo3_2_1_1.Checked = true;
                    }
                    //3.2. 2)
                    if (Dt.Rows[0]["OPT322"].ToString().Trim() == "10")
                    {
                        rdo3_2_2_0.Checked = true;
                        rdo3_2_2_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT322"].ToString().Trim() == "01")
                    {
                        rdo3_2_2_0.Checked = false;
                        rdo3_2_2_1.Checked = true;
                    }
                    //3.2. 3)
                    if (Dt.Rows[0]["OPT323"].ToString().Trim() == "10")
                    {
                        rdo3_2_3_0.Checked = true;
                        rdo3_2_3_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT323"].ToString().Trim() == "01")
                    {
                        rdo3_2_3_0.Checked = false;
                        rdo3_2_3_1.Checked = true;
                    }
                    //3.2. 4)
                    if (Dt.Rows[0]["OPT324"].ToString().Trim() == "10")
                    {
                        rdo3_2_4_0.Checked = true;
                        rdo3_2_4_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT324"].ToString().Trim() == "01")
                    {
                        rdo3_2_4_0.Checked = false;
                        rdo3_2_4_1.Checked = true;
                    }
                    //3.2. 5)
                    if (Dt.Rows[0]["OPT325"].ToString().Trim() == "10")
                    {
                        rdo3_2_5_0.Checked = true;
                        rdo3_2_5_1.Checked = false;
                    }
                    else if (Dt.Rows[0]["OPT325"].ToString().Trim() == "01")
                    {
                        rdo3_2_5_0.Checked = false;
                        rdo3_2_5_1.Checked = true;
                    }

                    txtEntName.Text = Dt.Rows[0]["ENTNAME"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;



                if (dtpOpDate.Text == "")
                {
                    //수술일(DRG수술기준)
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT  TO_CHAR(OPDATE, 'YYYY-MM-DD') OPDATE, PANO, SNAME, DEPTCODE, DRCODE, IPDOPD, AGE, SEX ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ORAN_MASTER ";
                    SQL += ComNum.VBLF + "  WHERE PANO = '" + clsPublic.GstrPANO + "' ";
                    SQL += ComNum.VBLF + "  ORDER BY OPDATE DESC ";
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
                        dtpOpDate.Text = Dt.Rows[0]["OPDATE"].ToString().Trim();
                    }

                    Dt.Dispose();
                    Dt = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strOPT110 = "", strOPT110A = "", strOPT211 = "", strOPT212 = "", strOPT213 = "", strOPT214 = "", strOPT214A = "";
            string strOPT220 = "", strOPT230 = "", strOPT230A = "", strOPT240 = "", strOPT310 = "", strOPT310A = "", strOPT321 = "";
            string strOPT322 = "", strOPT323 = "", strOPT324 = "", strOPT325 = "";

            DataTable Dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            
             
            
            if (FstrTRSNO == "" || FstrIPDNO == "") { return; }

            #region Data Input
            //1.1.
            if (rdo1_1_0.Checked) { strOPT110 = "10"; }
            else if (rdo1_1_1.Checked) { strOPT110 = "01"; }

            //1.1. 추가 코드
            if (rdo1_1_1_1.Checked) { strOPT110A = "1"; }
            else if (rdo1_1_1_2.Checked) { strOPT110A = "2"; }
            else if (rdo1_1_1_3.Checked) { strOPT110A = "3"; }

            //2.1. 1)
            if (rdo2_1_1_0.Checked) { strOPT211 = "10"; }
            else if (rdo2_1_1_1.Checked) { strOPT211 = "01"; }

            //2.1. 2)
            if (rdo2_1_2_0.Checked) { strOPT212 = "10"; }
            else if (rdo2_1_2_1.Checked) { strOPT212 = "01"; }

            //2.1. 3)
            if (rdo2_1_3_0.Checked) { strOPT213 = "10"; }
            else if (rdo2_1_3_1.Checked) { strOPT213 = "01"; }

            //2.1. 4)
            if (rdo2_1_4_0.Checked) { strOPT214 = "10"; }
            else if (rdo2_1_4_1.Checked) { strOPT214 = "01"; }

            //2.1. 4) 추가 코드
            strOPT214A = txtOpt2_1.Text;
            //2.2.
            if (rdo2_2_0.Checked) { strOPT220 = "10"; }
            else if (rdo2_2_1.Checked) { strOPT220 = "01"; }

            //2.3.
            if (rdo2_3_0.Checked) { strOPT230 = "10"; }
            else if (rdo2_3_1.Checked) { strOPT230 = "01"; }

            //2.3. 추가 코드
            strOPT230A = txtOpt2_3.Text;
            //2.4.
            if (rdo2_4_0.Checked) { strOPT240 = "10"; }
            else if (rdo2_4_1.Checked) { strOPT240 = "01"; }

            //3.1.
            if (rdo3_1_0.Checked) { strOPT310 = "10"; }
            else if (rdo3_1_1.Checked) { strOPT310 = "01"; }

            //3.1. 추가 코드
            if (rdo3_1_1_1.Checked) { strOPT310A = "1"; }
            else if (rdo3_1_1_2.Checked) { strOPT310A = "2"; }
            else if (rdo3_1_1_3.Checked) { strOPT310A = "3"; }
            else if (rdo3_1_1_4.Checked) { strOPT310A = "4"; }

            //3.2. 1)
            if (rdo3_2_1_0.Checked) { strOPT321 = "10"; }
            else if (rdo3_2_1_1.Checked) { strOPT321 = "01"; }

            //3.2. 2)
            if (rdo3_2_3_0.Checked) { strOPT322 = "10"; }
            else if (rdo3_2_2_1.Checked) { strOPT322 = "01"; }

            //3.2. 3)
            if (rdo3_2_3_0.Checked) { strOPT323 = "10"; }
            else if (rdo3_2_3_1.Checked) { strOPT323 = "01"; }

            //3.2. 4)
            if (rdo3_2_4_0.Checked) { strOPT324 = "10"; }
            else if (rdo3_2_4_1.Checked) { strOPT324 = "01"; }

            //3.2. 5)
            if (rdo3_2_5_0.Checked) { strOPT325 = "10"; }
            else if (rdo3_2_5_1.Checked) { strOPT325 = "01"; }

            #endregion Data Input End

            #region Data Check

            //저장 기본 체크
            if (strOPT110 == "") { ComFunc.MsgBox("1.1 항목 누락점검!!"); return; }
            if (strOPT110 == "01" && strOPT110A == "") { ComFunc.MsgBox("1.1 시행세부항목 누락점검!!"); return; }
            if (strOPT211 == "") { ComFunc.MsgBox("2.1 1) 항목 누락점검!!"); return; }
            if (strOPT212 == "") { ComFunc.MsgBox("2.1 2) 항목 누락점검!!"); return; }
            if (strOPT213 == "") { ComFunc.MsgBox("2.1 3) 항목 누락점검!!"); return; }
            if (strOPT214 == "") { ComFunc.MsgBox("2.1 4) 항목 누락점검!!"); return; }
            if (strOPT214 == "01" && strOPT214A == "") { ComFunc.MsgBox("2.1 4) 세부항목 누락점검!!"); return; }
            if (strOPT220 == "") { ComFunc.MsgBox("2.2 항목 누락점검!!"); return; }
            if (strOPT230 == "") { ComFunc.MsgBox("2.3 항목 누락점검!!"); return; }
            if (strOPT230 == "01" && strOPT230A == "") { ComFunc.MsgBox("2.3 세부항목 누락점검!!"); return; }
            if (strOPT240 == "") { ComFunc.MsgBox("2.4 항목 누락점검!!"); return; }
            if (strOPT310 == "") { ComFunc.MsgBox("3.1 항목 누락점검!!"); return; }
            if (strOPT310 == "01" && strOPT310A == "") { ComFunc.MsgBox("3.1 세부항목 누락점검!!"); return; }
            if (strOPT321 == "") { ComFunc.MsgBox("3.2 1) 항목 누락점검!!"); return; }
            if (strOPT322 == "") { ComFunc.MsgBox("3.2 2) 항목 누락점검!!"); return; }
            if (strOPT323 == "") { ComFunc.MsgBox("3.2 3) 항목 누락점검!!"); return; }
            if (strOPT324 == "") { ComFunc.MsgBox("3.2 4) 항목 누락점검!!"); return; }
            if (strOPT325 == "") { ComFunc.MsgBox("3.2 5) 항목 누락점검!!"); return; }

            if (txtEntName.Text.Trim() == "")
            {
                ComFunc.MsgBox("작성자 누락점검!!");
                txtEntName.Focus();
                return;
            }
            if (dtpOpDate.Text.Trim() == "")
            {
                ComFunc.MsgBox("수술일자 누락점검!!");
                dtpOpDate.Focus();
                return;
            }

            #endregion Data Check End
            
            //등록
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //점검표
                SQL = "";
                SQL += ComNum.VBLF + " SELECT  ROWID ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "DRG_CHART1 ";
                SQL += ComNum.VBLF + "  WHERE  TRSNO = '" + FstrTRSNO + "' ";
                SQL += ComNum.VBLF + "    AND  IPDNO = '" + FstrIPDNO + "' ";
                SQL += ComNum.VBLF + "    AND  (DelDate IS NULL OR DelDate ='') ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    FstrRowid2 = Dt.Rows[0]["ROWID"].ToString().Trim();

                    SQL = " UPDATE KOSMOS_PMPA.DRG_CHART1 SET ";
                    SQL += ComNum.VBLF + " TRSNO = '" + FstrTRSNO + "', ";
                    SQL += ComNum.VBLF + " IPDNO = '" + FstrIPDNO + "', ";
                    SQL += ComNum.VBLF + " PANO = '" + FstrPano + "', ";
                    SQL += ComNum.VBLF + " PNAME = '" + txtPName.Text + "', ";
                    SQL += ComNum.VBLF + " OPT110 = '" + strOPT110 + "', ";
                    SQL += ComNum.VBLF + " OPT110A = '" + strOPT110A + "', ";
                    SQL += ComNum.VBLF + " OPT211 = '" + strOPT211 + "', ";
                    SQL += ComNum.VBLF + " OPT212 = '" + strOPT212 + "', ";
                    SQL += ComNum.VBLF + " OPT213 = '" + strOPT213 + "', ";
                    SQL += ComNum.VBLF + " OPT214 = '" + strOPT214 + "', ";
                    SQL += ComNum.VBLF + " OPT214A = '" + strOPT214A + "', ";
                    SQL += ComNum.VBLF + " OPT220 = '" + strOPT220 + "', ";
                    SQL += ComNum.VBLF + " OPT230 = '" + strOPT230 + "', ";
                    SQL += ComNum.VBLF + " OPT230A = '" + strOPT230A + "', ";
                    SQL += ComNum.VBLF + " OPT240 = '" + strOPT240 + "', ";
                    SQL += ComNum.VBLF + " OPT310 = '" + strOPT310 + "', ";
                    SQL += ComNum.VBLF + " OPT310A = '" + strOPT310A + "', ";
                    SQL += ComNum.VBLF + " OPT321 = '" + strOPT321 + "', ";
                    SQL += ComNum.VBLF + " OPT322 = '" + strOPT322 + "', ";
                    SQL += ComNum.VBLF + " OPT323 = '" + strOPT323 + "', ";
                    SQL += ComNum.VBLF + " OPT324 = '" + strOPT324 + "', ";
                    SQL += ComNum.VBLF + " OPT325 = '" + strOPT325 + "', ";
                    SQL += ComNum.VBLF + " LSDATE = TO_DATE('" + dtpLsDate.Text + "', 'YYYY-MM-DD'),";
                    SQL += ComNum.VBLF + " OPDATE = TO_DATE('" + dtpOpDate.Text + "', 'YYYY-MM-DD'),";
                    SQL += ComNum.VBLF + " ENTSABUN = '" + clsPublic.GnJobSabun + "', ";
                    SQL += ComNum.VBLF + " ENTNAME = '" + txtEntName.Text + "' ";
                    SQL += ComNum.VBLF + "  WHERE TRSNO = '" + FstrTRSNO + "' ";
                    SQL += ComNum.VBLF + "    AND IPDNO = '" + FstrIPDNO + "' ";
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "  INSERT INTO KOSMOS_PMPA.DRG_CHART1 ( ";
                    SQL += ComNum.VBLF + "  TRSNO, IPDNO, PANO, PNAME, ";
                    SQL += ComNum.VBLF + "  OPT110, OPT110A, OPT211, OPT212, OPT213, OPT214, OPT214A, ";
                    SQL += ComNum.VBLF + "  OPT220, OPT230, OPT230A, OPT240, OPT310, OPT310A, OPT321, ";
                    SQL += ComNum.VBLF + "  OPT322, OPT323, OPT324, OPT325, LSDATE, OPDATE, ENTSABUN, ENTNAME ) ";
                    SQL += ComNum.VBLF + "  VALUES ( ";
                    SQL += ComNum.VBLF + " '" + FstrTRSNO + "', ";
                    SQL += ComNum.VBLF + " '" + FstrIPDNO + "',";
                    SQL += ComNum.VBLF + " '" + FstrPano + "',";
                    SQL += ComNum.VBLF + " '" + txtPName.Text + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT110 + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT110A + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT211 + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT212 + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT213 + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT214 + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT214A + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT220 + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT230 + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT230A + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT240 + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT310 + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT310A + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT321 + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT322 + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT323 + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT324 + "', ";
                    SQL += ComNum.VBLF + " '" + strOPT325 + "', ";
                    SQL += ComNum.VBLF + " TO_DATE('" + dtpLsDate.Text + "', 'YYYY-MM-DD'),";
                    SQL += ComNum.VBLF + " TO_DATE('" + dtpOpDate.Text + "', 'YYYY-MM-DD'),";
                    SQL += ComNum.VBLF + " '" + clsPublic.GnJobSabun + "', ";
                    SQL += ComNum.VBLF + " '" + txtEntName.Text + "' ) ";
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

                Dt.Dispose();
                Dt = null;

                Screen_Clear();
                List_View();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            
             
            
            if (FstrTRSNO == "" || FstrIPDNO == "")
            {
                ComFunc.MsgBox("대상 선택후 처리하십시오");
                return;
            }

            if (ComFunc.MsgBoxQ("자료를 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE KOSMOS_PMPA.DRG_CHART1 SET  ";
                SQL += ComNum.VBLF + " DelDate =SYSDATE ";
                SQL += ComNum.VBLF + " WHERE TRSNO = '" + FstrTRSNO + "' ";
                SQL += ComNum.VBLF + "   AND IPDNO = '" + FstrIPDNO + "' ";
                SQL += ComNum.VBLF + "   AND (DelDate IS NULL OR DelDate ='') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                    
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                ComFunc.MsgBox("삭제되었습니다.");

                Screen_Clear();
                List_View();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                clsDB.setRollbackTran(clsDB.DbCon);
            }
        }
    }
}
