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

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSugaDayCount
    /// File Name : frmSugaDayCount.cs
    /// Title or Description : 약품일수관리
    /// Author : 유진호
    /// Create Date : 2017-11-02
    /// Update Histroy :     
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso>
    /// VB\basic\busuga\miretc64.frm(FrmSugaDayCount)
    /// </seealso> 
    /// </summary>
    public partial class frmSugaDayCount : Form
    {
        public frmSugaDayCount()
        {
            InitializeComponent();
        }

        private void frmSugaDayCount_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtFDate.Text = "2000-01-01";
            txtTDate.Text = clsPublic.GstrSysDate;


            SCREEN_CLEAR();
            cboSuCode.Items.Clear();
            cboSuCode.Items.Add("ZEFIX");
            cboSuCode.Items.Add("BARA05");
            cboSuCode.Items.Add("BARA059");
            cboSuCode.Items.Add("BARA1");
            cboSuCode.Items.Add("BARA19");
            cboSuCode.Items.Add("HEPS");
            cboSuCode.Items.Add("HEPS9");
            cboSuCode.Items.Add("ZEF100");
            cboSuCode.Items.Add("LEVO30");
            cboSuCode.Items.Add("ENBR");
            cboSuCode.Items.Add("ENBR25");
            cboSuCode.Items.Add("HUMIRA");
            cboSuCode.SelectedIndex = 0;
        }

        private void SCREEN_CLEAR()
        {
            ss1_Sheet1.RowCount = 0;
            ss1_Sheet1.RowCount = 20;
            txtPano.Text = "";

            lblSname.Text = "";
            lblSuName.Text = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수


            int nREAD;
            int nRow;
            int nICnt1; //'급여
            int nICnt2; //'비급여
            int nICnt3; //'비급여
            int nOCnt1; //'급여
            int nOCnt2; //'비급여
            int nOCnt3; //'비급여

            ss1_Sheet1.RowCount = 0;

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                if (lblSname.Text == "")
                {
                    ComFunc.MsgBox("등록번호 오류입니다.", "확인");
                    txtPano.Focus();
                    return;
                }
                if (lblSuName.Text == "")
                {
                    ComFunc.MsgBox("수가코드 오류입니다.", "확인");
                    cboSuCode.Focus();
                    return;
                }

                if (txtFDate.Text == "")
                {
                    ComFunc.MsgBox("조회날짜가 오류입니다.", "확인");
                    txtFDate.Focus();
                    return;
                }
                if (txtTDate.Text == "")
                {
                    ComFunc.MsgBox("조회날짜가 오류입니다.", "확인");
                    txtTDate.Focus();
                    return;
                }

                //'외래 처방 Read
                SQL = "";
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE  , ";
                SQL = SQL + ComNum.VBLF + " DECODE(GBSELF,'0', SUM(QTY * NAL),'0')   NOCNT1, ";
                SQL = SQL + ComNum.VBLF + " DECODE(GBSELF,'1', SUM(QTY * NAL),'0')   NOCNT2, ";
                SQL = SQL + ComNum.VBLF + " DECODE(GBSELF,'2', SUM(QTY * NAL),'0')   NOCNT3,  0 NICNT1,0 NICNT2,0 NICNT3 ,'O' GBIO  ";
                SQL = SQL + ComNum.VBLF + " FROM OPD_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TO_DATE('" + txtFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE <= TO_DATE('" + txtTDate.Text + "','YYYY-MM-DD')";
                if (txtPano.Text != "") SQL = SQL + ComNum.VBLF + "   AND PANO ='" + txtPano.Text + "' ";
                if (cboSuCode.Text != "") SQL = SQL + ComNum.VBLF + "   AND SUNEXT = '" + cboSuCode.Text + "' ";

                SQL = SQL + ComNum.VBLF + "  GROUP BY BDATE, GBSELF ";
                SQL = SQL + ComNum.VBLF + " UNION ALL";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE  , 0,0,0, ";
                SQL = SQL + ComNum.VBLF + " DECODE(GBSELF,'0', SUM(QTY * NAL),'0')   NICNT1, ";
                SQL = SQL + ComNum.VBLF + " DECODE(GBSELF,'1', SUM(QTY * NAL),'0')   NICNT2, ";
                SQL = SQL + ComNum.VBLF + " DECODE(GBSELF,'2', SUM(QTY * NAL),'0')   NICNT3, 'I' GBIO";
                SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_SLIP ";
                SQL = SQL + ComNum.VBLF + " WHERE BDATE >= TO_DATE('" + txtFDate.Text + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE <= TO_DATE('" + txtTDate.Text + "','YYYY-MM-DD')";
                if (txtPano.Text != "") SQL = SQL + ComNum.VBLF + "   AND PANO ='" + txtPano.Text + "' ";
                if (cboSuCode.Text != "") SQL = SQL + ComNum.VBLF + "   AND SUNEXT = '" + cboSuCode.Text + "' ";

                SQL = SQL + ComNum.VBLF + "  GROUP BY BDATE, GBSELF ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY 1 ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;
                    nOCnt1 = 0; nICnt1 = 0;
                    nOCnt2 = 0; nICnt2 = 0;
                    nOCnt3 = 0; nICnt3 = 0;
                    nRow = 1;

                    ss1_Sheet1.RowCount = dt.Rows.Count + 1;
                    ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1_Sheet1.Cells[i+1, 0].Text = dt.Rows[i]["GBIO"].ToString().Trim();
                        ss1_Sheet1.Cells[i+1, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ss1_Sheet1.Cells[i+1, 2].Text = dt.Rows[i]["NOCNT1"].ToString().Trim();
                        nOCnt1 = nOCnt1 + Convert.ToInt32(VB.Val(dt.Rows[i]["NOCNT1"].ToString().Trim()));
                        ss1_Sheet1.Cells[i+1, 3].Text = dt.Rows[i]["NOCNT2"].ToString().Trim();
                        nOCnt2 = nOCnt2 + Convert.ToInt32(VB.Val(dt.Rows[i]["NOCNT2"].ToString().Trim()));
                        ss1_Sheet1.Cells[i+1, 4].Text = dt.Rows[i]["NOCNT3"].ToString().Trim();
                        nOCnt3 = nOCnt3 + Convert.ToInt32(VB.Val(dt.Rows[i]["NOCNT3"].ToString().Trim()));
                        ss1_Sheet1.Cells[i+1, 5].Text = dt.Rows[i]["NICNT1"].ToString().Trim();
                        nICnt1 = nICnt1 + Convert.ToInt32(VB.Val(dt.Rows[i]["NICNT1"].ToString().Trim()));
                        ss1_Sheet1.Cells[i+1, 6].Text = dt.Rows[i]["NICNT2"].ToString().Trim();
                        nICnt2 = nICnt2 + Convert.ToInt32(VB.Val(dt.Rows[i]["NICNT2"].ToString().Trim()));
                        ss1_Sheet1.Cells[i+1, 7].Text = dt.Rows[i]["NICNT3"].ToString().Trim();
                        nICnt3 = nICnt3 + Convert.ToInt32(VB.Val(dt.Rows[i]["NICNT3"].ToString().Trim()));
                    }


                    ss1_Sheet1.Cells[0, 1].Text = "합계";
                    ss1_Sheet1.Cells[0, 2].Text = nOCnt1.ToString();
                    ss1_Sheet1.Cells[0, 3].Text = nOCnt2.ToString();
                    ss1_Sheet1.Cells[0, 4].Text = nOCnt3.ToString();
                    ss1_Sheet1.Cells[0, 5].Text = nICnt1.ToString();
                    ss1_Sheet1.Cells[0, 6].Text = nICnt2.ToString();
                    ss1_Sheet1.Cells[0, 7].Text = nICnt3.ToString();;                    
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboSuCode_Leave(object sender, EventArgs e)
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";
                SQL = " SELECT SUNAMEK FROM BAS_SUN ";
                SQL = SQL + ComNum.VBLF + "WHERE SUNEXT = '" + cboSuCode.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("해당코드가 등록 않됨", "확인");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblSuName.Text = "";
                    lblSuName.Text = dt.Rows[0]["SUNAMEK"].ToString().Trim();
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFDate_DoubleClick(object sender, EventArgs e)
        {
            Calendar_Date_Select(txtFDate);
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
            lblSname.Text = Read_PatientName(txtPano.Text);
            if (lblSname.Text == "")
            {
                ComFunc.MsgBox("해당 등록번호는 없는 번호입니다.", "확인");
                return;
            }
          
            SendKeys.Send("{TAB}");            
        }
        
        //TODO: VB READ_PatientName(vbfunc.bas 모듈)함수 임시로 만들어서 사용
        private string Read_PatientName(string argPano)
        {
            string SQL = "";
            string rtnVal = "";
            string SqlErr = "";
            DataTable dt = null;
            try
            {
                if (VB.Val(argPano) == 0)
                {
                    rtnVal = "";
                }

                SQL = "";
                SQL += ComNum.VBLF + "SELECT SName FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + " WHERE Pano='" + argPano + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SName"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }

            return rtnVal;
        }

        private void txtTDate_DoubleClick(object sender, EventArgs e)
        {
            Calendar_Date_Select(txtTDate);
        }

        private void Calendar_Date_Select(Control ArgText)
        {
            clsPublic.GstrCalDate = VB.Trim(ArgText.Text);
            if (VB.Len(VB.Trim(ArgText.Text)) != 10)
            {
                clsPublic.GstrCalDate = clsPublic.GstrSysDate;
            }

            frmCalendar frmCalendarX = new frmCalendar();
            frmCalendarX.ShowDialog();

            if (VB.Len(clsPublic.GstrCalDate) == 10)
            {
                ArgText.Text = clsPublic.GstrCalDate;
            }
        }
    }
}
