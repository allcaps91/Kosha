using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmPatientSearch : Form
    {
        /// <summary>
        /// Class Name      : ComLibB
        /// File Name       : frmPatientSearch.cs
        /// Description     : 환자조회 : 원무기준으로 작성
        /// Author          : 박웅규
        /// Create Date     : 2018-04-09
        /// <seealso>
        /// D:\타병원\PSMHH\OPD\oumsad\OUMSAD20.FRM(frmPatientSearch)
        /// </seealso>
        /// <vbp>
        /// default 		: D:\타병원\PSMHH\OPD\oumsad\oumsad.vbp
        /// seealso 		: 
        /// </vbp>
        /// </summary>
        /// 

        //글로발 변수 : clsPublic.GstrChoicePano : 사용 권장하지 않음
        private string mCallFlag = "";
        public delegate void SetData(string strPtno);
        public event SetData rSetData; 

        public delegate void EventClose();
        public event EventClose rEventClose;

        public frmPatientSearch()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 이벤트를 처리할경우 
        /// </summary>
        /// <param name="CallFlag">EVENT</param>
        public frmPatientSearch(string CallFlag)
        {
            InitializeComponent();
            mCallFlag = CallFlag;
        }

        private void frmPatientSearch_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) //폼 권한 조회
            //{
            //    this.Close ();
            //    return;
            //} 
            //ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            btnEMR.Visible = clsType.User.BuseCode == "076001";//IdNumber

            panName.Left = panJumin.Left;
            panName.Top = panJumin.Top;
            panNumber.Top = panJumin.Top;
            panNumber.Left = panJumin.Left;
            optSearch1.Checked = true;
            txtJumin1.Select();
      
            optSearch3.Visible = clsType.User.IdNumber == "19684";//IdNumber  원무팀장 권한사용
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (mCallFlag == "EVENT")
            {
                rEventClose();
            }
            else
            {
                this.Close();
            }
        }
        
        private void optSearch1_CheckedChanged(object sender, EventArgs e)
        {
            if (optSearch1.Checked == true)
            {
                panJumin.Visible = true;
                panName.Visible = false;
                panNumber.Visible = false;
                txtJumin1.Focus();
                ssView_Sheet1.RowCount = 0;
            }
        }

        private void optSearch2_CheckedChanged(object sender, EventArgs e)
        {
            if (optSearch2.Checked == true)
            {
                panJumin.Visible = false;
                panName.Visible = true;
                panNumber.Visible = false;
                txtName.Focus();
                ssView_Sheet1.RowCount = 0;
            }
        }


        private void txtJumin1_Enter(object sender, EventArgs e)
        {
            txtJumin1.Select();
        }

        private void txtJumin1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (txtJumin1.Text.Length == 6)
                {
                    //txtJumin2.Focus();
                    this.SelectNextControl((Control)sender, true, true, true, true);
                }
            }
        }


        private void txtJumin2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (txtJumin2.Text.Length == 7)
                {
                    txtJumin2.Select();
                    txtJumin2.Focus();
                }

                GetData();
            }
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            txtName.Select();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (txtName.Text.Trim() != "")
                {
                    GetData();
                }
                GetData();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) //권한 확인
            //{
            //    return;
            //}

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;

            if (optSearch1.Checked == true)
            {
                //if (txtJumin1.Text.Length != 6 || txtJumin2.Text.Length != 7)
                //{
                //    ComFunc.MsgBox("주민번호를 정확히 입력해 주십시요");
                //    txtJumin1.Focus();
                //    return;
                //}
                if (txtJumin1.Text.Length != 6)
                {
                    ComFunc.MsgBox("주민번호를 정확히 입력해 주십시요");
                    txtJumin1.Focus();
                    return;
                }
            }
            else if (optSearch3.Checked == true)
            {
               
                if (txtNumber.Text.Length < 4)
                {
                    ComFunc.MsgBox("4자리 이상 정확히 입력해 주십시요");
                    txtNumber.Focus();
                    return;
                }
            }
            else
            {
                if (txtName.Text.Trim() == "")
                {
                    ComFunc.MsgBox("수진자명을 입력해 주십시요");
                    txtName.Focus();
                    return;
                }
            }

            try
            {
                string strCurDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon , "D"),"D");

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT Pano,Sname,Sex,Jumin1,Jumin2,Jumin3,Pname,Bi,Tel,Hphone,GKiho,";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(LastDate,'YYYY-MM-DD') LastDate,DeptCode,GbGamek ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                if (optSearch1.Checked == true)
                {
                    if (txtJumin1.Text.Trim() != "")
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE Jumin1 = '" + txtJumin1.Text + "'";
                        if (txtJumin2.Text.Trim() != "")
                        {
                            SQL = SQL + ComNum.VBLF + "   and Jumin3 = '" + clsAES.AES(txtJumin2.Text.Trim()) + "'";
                        }
                    }
                    else
                    {
                        if (txtJumin2.Text.Trim() != "")
                        {
                            SQL = SQL + ComNum.VBLF + "   and Jumin3 = '" + clsAES.AES(txtJumin2.Text.Trim()) + "'";
                        }
                    }
                }
                else if (optSearch3.Checked == true)
                {
                    
                    SQL = SQL + ComNum.VBLF + "   WHERE  ((TEL like '%" + txtNumber.Text.Trim() + "%') or (Hphone like '%" + txtNumber.Text.Trim() + "%')  ) ";

                }
                else

                {
                    SQL = SQL + ComNum.VBLF + " WHERE Sname like '" + txtName.Text.Trim() + "%'";
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY Sname,Jumin1,Jumin2 ";

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

                ComFunc ComFuncX = new ComFunc();

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Sname"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Jumin1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[i]["Jumin3"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Pname"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Tel"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["LastDate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["Hphone"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["GKiho"].ToString().Trim();

                        if (ComFuncX.DATE_ILSU(clsDB.DbCon, strCurDate, dt.Rows[i]["LastDate"].ToString().Trim()) <= 30)
                        {
                            ssView_Sheet1.Cells[i, 0, i, ssView_Sheet1.ColumnCount - 1].BackColor = Color.Pink;
                        }

                        string strGamek = ComFunc.SetAutoZero(dt.Rows[i]["GbGamek"].ToString().Trim(), 2);

                        if (strGamek != "00" && VB.Val(strGamek) <= 52)
                        {
                            ssView_Sheet1.Cells[i, 0, i, ssView_Sheet1.ColumnCount - 1].BackColor = Color.LightGreen;
                        }
                    }

                    ssView.Focus();
                }
                dt.Dispose();
                dt = null;


                ComFuncX = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount <= 0) return;
            if (e.ColumnHeader == true) return;

            string strPtno = ssView_Sheet1.Cells[e.Row, 0].Text.Trim();
            string strSname = ssView_Sheet1.Cells[e.Row, 1].Text.Trim();
            string strSex = ssView_Sheet1.Cells[e.Row, 2].Text.Trim();
            string strJumin = ssView_Sheet1.Cells[e.Row, 3].Text.Trim();
            string strTel = ssView_Sheet1.Cells[e.Row, 6].Text.Trim();

            if (e.Column == 0 && clsType.User.BuseCode.Equals("044201"))
            {
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name.Equals("frmEmrViewer"))
                    {
                        (frm as frmEmrViewer).SetNewPatient(strPtno);
                        return;
                    }
                    else
                    {
                        //fEmrViewer.SetNewPatient(GstrPANO);
                    }
                }
                ComFunc.MsgBoxEx(this, "EMR 뷰어를 실행해주세요.");
                return;
            }

            if (mCallFlag == "EVENT")
            {
                rSetData(strPtno);
            }
            else
            {
                //글로발 변수 : clsPublic.GstrChoicePano : 사용 권장하지 않음
                clsPublic.GstrChoicePano = strPtno;
                clsPublic.GstrHelpCode = strPtno + "{}" + strSname + "{}" + strSex + "{}" + strJumin + "{}{}" + strTel;
                this.Close();
            }
        }

        private void txtJumin1_TextChanged(object sender, EventArgs e)
        {
            if (txtJumin1.Text.Length == 6)
            {
                txtJumin2.Focus();
            }
        }

        private void ssView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ssView_Sheet1.RowCount <= 0) return;

                string strPtno = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 0].Text.Trim();

                if (mCallFlag == "EVENT")
                {
                    rSetData(strPtno);
                }
                else
                {
                    //글로발 변수 : clsPublic.GstrChoicePano : 사용 권장하지 않음
                    //clsPublic.GstrChoicePano = strPtno;
                    //this.Close();
                }
            }

        }

        private void btnEMR_Click(object sender, EventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) return;

            clsVbEmr.EXECUTE_TextEmrViewEx(ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 0].Text.Trim(), clsType.User.Sabun);
            return;
        }

        private void optSearch3_CheckedChanged(object sender, EventArgs e)
        {

            if (optSearch3.Checked == true)
            {
                panJumin.Visible = false;
                panName.Visible = false;
                panNumber.Visible = true;
                txtNumber.Focus();
                ssView_Sheet1.RowCount = 0;
            }
        }
    }
}
