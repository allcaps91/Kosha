using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스


/// <summary>
/// Description : 입원동기 조회
/// Author : 박병규
/// Create Date : 2017.05.25
/// </summary>
/// <history>
/// </history>

namespace ComLibB
{
    public partial class frmViewIpwonMotive : Form
    {
        clsUser CU = null;
        clsSpread SPR = null;

        string strFdate;
        string strTdate;

        public frmViewIpwonMotive()
        {
            InitializeComponent();
        }

        private void frmViewIpwonMotive_Load(object sender, EventArgs e)
        {
            CU = new clsUser();
            SPR = new clsSpread();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            lblDate.Visible = false;
            dtpFdate.Visible = false;
            dtpTdate.Visible = false;

            ComFunc.SetAllControlClear(pnlBody);

            strFdate = VB.DateAdd("D",-31, VB.Now()).ToString("yyyy-MM-dd");
            strTdate = VB.Now().ToString("yyyy-MM-dd");
        }


        private void rdoGb0_Click(object sender, EventArgs e)
        {
            rdoGb_Click(0);
        }

        private void rdoGb1_Click(object sender, EventArgs e)
        {
            rdoGb_Click(1);
        }

        private void rdoGb_Click(int Index)
        {
            ComFunc.SetAllControlClear(pnlBody);

            if (Index == 0)
            {
                lblDate.Visible = false;
                dtpFdate.Visible = false;
                dtpTdate.Visible = false;
            }
            else
            {
                lblDate.Visible = true;
                dtpFdate.Visible = true;
                dtpTdate.Visible = true;

                dtpFdate.Value = Convert.ToDateTime( strFdate);
                dtpTdate.Value = Convert.ToDateTime( strTdate);
                dtpFdate.Focus();
            }

        }

        private void dtpFdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) SendKeys.Send("{TAB}");
        }

        private void dtpTdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) SendKeys.Send("{TAB}");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            String SqlErr = "";

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) { return; }     //권한확인

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ComFunc.SetAllControlClear(ssList);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(A.InDate,'YYYY-MM-DD') InDate, A.Pano, A.SName,";
                SQL += ComNum.VBLF + "        A.Bi, C.NAME AS BINAME, A.Age, A.Sex,";
                SQL += ComNum.VBLF + "        A.DeptCode, D.DEPTNAMEK, A.WardCode, A.RoomCode, B.NAME ";
                SQL += ComNum.VBLF + "   FROM ADMIN.IPD_NEW_MASTER A,";
                SQL += ComNum.VBLF + "        ETC_CSINFO_CODE B,";
                SQL += ComNum.VBLF + "        BAS_BCODE C,";
                SQL += ComNum.VBLF + "        BAS_CLINICDEPT D";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND A.GbDonggi > '00' ";
                if (rdoGb0.Checked == true)
                {
                    SQL += ComNum.VBLF + "    AND A.OUTDATE IS NULL ";
                    SQL += ComNum.VBLF + "    AND A.GBSTS  = '0' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND A.ActDate >= TO_DATE('" + dtpFdate.Value + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND A.ActDate <= TO_DATE('" + dtpTdate.Value + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND A.OUTDATE IS NOT NULL ";
                    SQL += ComNum.VBLF + "    AND A.GBSTS  = '7' ";
                }
                SQL += ComNum.VBLF + "    AND B.GUBUN = '2' ";
                SQL += ComNum.VBLF + "    AND C.GUBUN = 'BAS_환자종류' ";
                SQL += ComNum.VBLF + "    AND A.GBDONGGI = TRIM(B.CODE) ";
                SQL += ComNum.VBLF + "    AND A.BI = C.CODE ";
                SQL += ComNum.VBLF + "    AND A.DEPTCODE = D.DEPTCODE ";
                SQL += ComNum.VBLF + "ORDER BY InDate,Pano ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                    Dt.Dispose();
                    Dt = null;
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Dt.Dispose();
                    Dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssList_Sheet1.RowCount = Dt.Rows.Count;
                ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("데이터 내역이 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Dt.Dispose();
                    Dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["INDATE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["SEX"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["AGE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["BINAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["DEPTNAMEK"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 9].Text = Dt.Rows[i]["NAME"].ToString().Trim();
                }
                Dt.Dispose();
                Dt = null;

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void dtpFdate_ValueChanged(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlBody);
        }

        private void dtpTdate_ValueChanged(object sender, EventArgs e)
        {
            ComFunc.SetAllControlClear(pnlBody);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            string strDate = "";
            bool PrePrint = true;
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) { return; }     //권한확인

            strDate = Convert.ToString( dtpFdate.Value) + " ~ " + Convert.ToString( dtpTdate.Value);

            if (rdoGb0.Checked == true)
                strTitle = "재원자 입원동기 내역";
            else
                strTitle = "퇴원자 입원동기 내역";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            if (rdoGb1.Checked == true)
            {
                strHeader += SPR.setSpdPrint_String("조회기간 : " + strDate, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            }

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + CU.GstrJobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }


        private void ssList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            string strPtno = "";

            strPtno = ssList_Sheet1.Cells[e.Row, 1].Text;

            frmViewCsinfo frm = new frmViewCsinfo(strPtno);
            frm.ShowDialog();
        }

    }
}
