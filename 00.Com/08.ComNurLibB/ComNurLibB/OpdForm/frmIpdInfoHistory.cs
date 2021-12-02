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
using ComLibB;

namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmIpdInfoHistory.cs
    /// Description     : 입원정보
    /// Author          : 유진호
    /// Create Date     : 2018-01-12
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\Frm입원정보
    /// </history>
    /// </summary>
    public partial class frmIpdInfoHistory : Form
    {
        ComFunc CF = new ComFunc();
        private string strPaNo = "";
        private string strPaName = "";

        public frmIpdInfoHistory()
        {
            InitializeComponent();
        }

        public frmIpdInfoHistory(string strPaNo, string strPaName)
        {
            InitializeComponent();
            this.strPaNo = strPaNo;
            this.strPaName = strPaName;
        }

        private void frmIpdInfoHistory_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtPaNo.Text = strPaNo;
            txtPaName.Text = strPaName;

            getHistory();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void getHistory()
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                ssView_Sheet1.RowCount = 0;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT a.Pano,b.SName,TO_CHAR(b.INDATE,'YYYY-MM-DD') INDATE,TO_CHAR(b.OutDate,'YYYY-MM-DD') OUTDATE, ";
                SQL = SQL + ComNum.VBLF + " a.InWard,a.InRoom,a.InDept,a.InDrCode,DECODE(b.GbSts,'0','재원','7','퇴원','퇴원중' ) GbSts ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_MASTER a, KOSMOS_PMPA.IPD_NEW_MASTER b ";
                SQL = SQL + ComNum.VBLF + "  WHERE a.Pano=b.Pano  ";
                SQL = SQL + ComNum.VBLF + "   AND a.PANO ='" + strPaNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND b.GbSTS <> '9'  ";
                SQL = SQL + ComNum.VBLF + " ORDER BY b.InDate ";

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
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["InDept"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["InDrCode"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["InWard"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["InRoom"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["GbSts"].ToString().Trim();                        
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
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void txtPaNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                strPaNo = VB.Format(txtPaNo.Text, "00000000");
                strPaName = CF.Read_Patient(clsDB.DbCon, strPaNo, "2");

                txtPaNo.Text = strPaNo;
                txtPaName.Text = strPaName;

                getHistory();
            }            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            strPaNo = VB.Format(txtPaNo.Text, "00000000");
            strPaName = CF.Read_Patient(clsDB.DbCon, strPaNo, "2");

            txtPaNo.Text = strPaNo;
            txtPaName.Text = strPaName;

            getHistory();
        }
    }
}
