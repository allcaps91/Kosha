using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComNurLibB.Properties;
using ComDbB;
using ComBase;

namespace ComNurLibB
{
    public partial class frmAutoPrint : Form
    {
        int FnCntPic = 0;
        int FnCntWork = 0;
        int FnDelay = 0;
        bool FnWorkFlag = false;
        //int FnDeptCNT = 0;
        //string[] GstrSETNus = null;
        //string[] FstrJepsuDept = null;

        #region //MainFormMessage
        string mPara1 = "";
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion //MainFormMessage

        public frmAutoPrint()
        {
            InitializeComponent();
        }

        public frmAutoPrint(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
        }

        public frmAutoPrint(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
        }

        private void frmAutoPrint_Load(object sender, EventArgs e)
        {            
            ComFunc.ReadSysDate(clsDB.DbCon);
            SCREEN_CLEAR();

            FnDelay = 10;            //'Timer 지연 간격
            FnCntPic = 1;            //'초기화
            FnWorkFlag = false;
            
            lblID.Text = "";
            lblSDate.Text = "";
            lblFDate.Text = "";
            lblSDate.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            lblMsg.Text = "인쇄를 누르면 작업이 진행 됩니다.";
        }

        private void SCREEN_CLEAR()
        {
            txtPano.Text = "";
            txtSName.Text = "";
            txtRowId.Text = "";
            btnRePrint.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            FnCntPic = 1;
            FnWorkFlag = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            FnCntWork = 1;
            AUTO_PRINT_MAIN();

            timer1.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            FnCntPic = 1;
            FnWorkFlag = false;
        }

        private void btnRePrint_Click(object sender, EventArgs e)
        {                        
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {                
                SQL = " UPDATE OPD_WORK SET ERNAME ='' WHERE ROWID = '" + txtRowId.Text + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = FnWorkFlag;
            if (FnWorkFlag == false) return;

            FnCntPic = FnCntPic + 1;
            if (FnCntPic > 5)
            {
                FnCntPic = 1;
                FnCntWork = FnCntWork + 1;
            }
            
            if(FnCntPic == 1) pictureBox1.Image = Resources.pic_0;
            else if (FnCntPic == 2) pictureBox1.Image = Resources.pic_1;
            else if (FnCntPic == 3) pictureBox1.Image = Resources.pic_2;
            else if (FnCntPic == 4) pictureBox1.Image = Resources.pic_3;
            else if (FnCntPic == 5) pictureBox1.Image = Resources.pic_4;
            
            Application.DoEvents();

            if (FnCntWork > FnDelay)
            {
                FnCntWork = 1;
                AUTO_PRINT_MAIN();
            }

            lblMsg.Text = "작업이 대기중입니다.";
        }

        private void AUTO_PRINT_MAIN()
        {
            //int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strROWID = "";
            bool isPrint = false;
            
            try
            {                
                ComFunc.ReadSysDate(clsDB.DbCon);

                lblFDate.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

                SQL = " SELECT A.PANO , A.SNAME, A.AGE, A.ROWID, B.SEX, ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE , A.WRTTIME ";
                SQL = SQL + ComNum.VBLF + "   FROM OPD_WORK A, BAS_PATIENT B ";
                SQL = SQL + ComNum.VBLF + "  WHERE A.BDATE >= TRUNC(SYSDATE-1)";
                SQL = SQL + ComNum.VBLF + "     AND DELMARK <>'*' ";
                //2020-02-26 진단서 입사증도 이름표 인쇄 되도록 요청함
                //SQL = SQL + ComNum.VBLF + "     AND JIN NOT IN ('4','7') ";
                SQL = SQL + ComNum.VBLF + "     AND A.DEPTCODE ='ER'";
                SQL = SQL + ComNum.VBLF + "     AND A.ERNAME IS NULL ";
                SQL = SQL + ComNum.VBLF + "     AND A.PANO = B.PANO(+) ";
                SQL = SQL + ComNum.VBLF + "     AND (A.GBFLU IS NULL OR A.GBFLU <>'Y' ) ";
                SQL = SQL + ComNum.VBLF + "     AND A.Pano <> '81000004' "; //'전산실연습제외
                SQL = SQL + ComNum.VBLF + "     AND ROWNUM =1 ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.BDATE";

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
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }
                
                //'당일같은 인쇄건 있으면 2009-12-17
                SQL = " SELECT PANO FROM OPD_WORK ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + dt.Rows[0]["PANO"].ToString().Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DELMARK <>'*'  ";
                //2020-02-26 진단서 입사증도 이름표 인쇄되도록 보완
                //SQL = SQL + ComNum.VBLF + "  AND JIN NOT IN ('4','7') ";
                SQL = SQL + ComNum.VBLF + "  AND DEPTCODE ='ER' ";
                SQL = SQL + ComNum.VBLF + "  AND ERNAME='Y'  ";
                SQL = SQL + ComNum.VBLF + "  AND (GBFLU IS NULL OR GBFLU <>'Y' )  ";
                SQL = SQL + ComNum.VBLF + "  AND BDATE =TO_DATE('" + dt.Rows[0]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND Pano <> '81000004' "; //'전산실연습제외

                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt1.Rows.Count == 0)
                {
                    isPrint = false;
                    ss1_Sheet1.Cells[2, 1].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    ss1_Sheet1.Cells[3, 1].Text = dt.Rows[0]["SEX"].ToString().Trim();
                    ss1_Sheet1.Cells[3, 3].Text = dt.Rows[0]["AGE"].ToString().Trim();
                    ss1_Sheet1.Cells[4, 1].Text = VB.Space(5) + dt.Rows[0]["PANO"].ToString().Trim();
                    ss1_Sheet1.Cells[5, 1].Text = VB.Space(5) + 
                                                ComFunc.FormatStrToDate(dt.Rows[0]["BDATE"].ToString().Trim().Replace("-", ""), "DK") + " " +
                                                ComFunc.FormatStrToDate(dt.Rows[0]["WRTTIME"].ToString().Trim(), "MK");
                }
                else
                {
                    isPrint = true;
                }       
                dt.Dispose();
                dt = null;
                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                dt1.Dispose();
                dt1 = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            if (isPrint == false)
            {                                
                ss1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;   //세로
                ss1_Sheet1.PrintInfo.Header = "";
                ss1_Sheet1.PrintInfo.Margin.Left = 0;
                ss1_Sheet1.PrintInfo.Margin.Right = 0;
                ss1_Sheet1.PrintInfo.Margin.Top = 0;
                ss1_Sheet1.PrintInfo.Margin.Bottom = 0;
                ss1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ss1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ss1_Sheet1.PrintInfo.ShowBorder = false;
                ss1_Sheet1.PrintInfo.ShowColor = false;
                ss1_Sheet1.PrintInfo.ShowGrid = true;
                ss1_Sheet1.PrintInfo.ShowShadows = false;
                ss1_Sheet1.PrintInfo.UseMax = false;
                ss1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                ss1.PrintSheet(0);
            }
            
            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                SQL = " UPDATE OPD_WORK SET ERNAME ='Y' WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                clsDB.setCommitTran(clsDB.DbCon);                
                Cursor.Current = Cursors.Default;                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }            
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (e.KeyCode != Keys.Enter) return;

            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

            try
            {
                SQL = " SELECT SNAME, ROWID FROM OPD_WORK ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPano.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE >= TRUNC(SYSDATE -1)";
                //2020-02-26 진단서 입사증도 인쇄되도록 보완
                //SQL = SQL + ComNum.VBLF + "   AND JIN NOT IN ('4','7') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='ER'";
                SQL = SQL + ComNum.VBLF + " ORDER BY BDATE DESC, SEQNO DESC ";  //'최종건 조회

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
                    ComFunc.MsgBox("해당환자는 응급실 접수환자가 아닙니다.");
                    Cursor.Current = Cursors.Default;
                    SCREEN_CLEAR();
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtRowId.Text = dt.Rows[0]["ROWID"].ToString().Trim();
                    txtSName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    btnRePrint.Enabled = true;
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

        private void frmAutoPrint_Activated(object sender, EventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgActivedForm(this);
            }
        }

        private void frmAutoPrint_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCallForm != null)
            {
                mCallForm.MsgUnloadForm(this);
            }
        }

        private void frmAutoPrint_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;   // 창을 보이지 않게 한다.
                this.ShowIcon = false;  // 작업표시줄에서 제거
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.ShowIcon = true;
            notifyIcon1.Visible = false;
        }
    }
}
