using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    public partial class frmPmpaViewHighClassWardProposal : Form
    {
        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : frmPmpaViewHighClassWardProposal.cs
        /// Description     : 상급병실 신청대상 명단조회
        /// Author          : 박창욱
        /// Create Date     : 2017-09-11
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= "\IPD\iument\Frm상급병실신청명단.frm(Frm상급병실신청명단.frm) >> frmPmpaViewHighClassWardProposal.cs 폼이름 재정의" />	
        public frmPmpaViewHighClassWardProposal()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            int nRead = 0;
            int nRow = 0;

            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;

            try
            {
                SQL = "";
                SQL = " SELECT Pano, Sname, Age || '/' || Sex as SAge,";
                SQL += ComNum.VBLF + "       TO_CHAR(InDate,'YYYY-MM-DD') INDATE, DeptCode, DrCode,";
                SQL += ComNum.VBLF + "       WardCode, RoomCode, SeCret_Sabun,";
                SQL += ComNum.VBLF + "       FROOM, FROOMETC";
                SQL += ComNum.VBLF + "  From " + ComNum.DB_PMPA + "IPD_NEW_MASTER                                  ";
                SQL += ComNum.VBLF + " Where 1 = 1";
                SQL += ComNum.VBLF + "   AND InDate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND InDate < TO_DATE('" + dtpTDate.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                if (txtPano.Text != "")
                {
                    SQL += ComNum.VBLF + "   AND PANO ='" + txtPano.Text + "' ";
                }
                if (chkAll.Checked == false)
                {
                    SQL += ComNum.VBLF + "   AND JDate= TO_DATE('1900-01-01','YYYY-MM-DD') ";
                }
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
                ssView_Sheet1.RowCount = nRead;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRead; i++)
                {
                    nRow += 1;
                    if (ssView_Sheet1.RowCount < nRow)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }

                    ssView_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["SAge"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    switch (dt.Rows[i]["FROOM"].ToString().Trim())
                    {
                        case "1":
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "1.본인요청";
                            break;
                        case "2":
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "2.병실부족";
                            break;
                        case "3":
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "3.격리사용";
                            break;
                        case "9":
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "9.기타사유";
                            break;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["FROOMETC"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void frmPmpaViewHighClassWardProposal_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtPano.Text = "";
            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(-15);
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 1;
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");
            }
        }
    }
}
