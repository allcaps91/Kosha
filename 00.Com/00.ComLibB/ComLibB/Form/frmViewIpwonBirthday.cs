using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 재원자 생일자명단 조회
/// Author : 박병규
/// Create Date : 2017.05.26
/// </summary>
/// <history>
/// </history>

namespace ComLibB
{
    public partial class frmViewIpwonBirthday : Form
    {
        clsUser CU = new clsUser();
        clsSpread SPR = new clsSpread();
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;

        public frmViewIpwonBirthday()
        {
            InitializeComponent();
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
                SQL += ComNum.VBLF + " SELECT to_char(b.indate, 'yyyy-mm-dd') INDATE, a.pano, a.sname, B.SEX, B.AGE, ";
                SQL += ComNum.VBLF + "        b.wardcode,b.roomcode, TO_CHAR(a.birth, 'YYYY-MM-DD') BIRTH";
                SQL += ComNum.VBLF + "   FROM kosmos_pmpa.bas_patient a,";
                SQL += ComNum.VBLF + "        kosmos_pmpa.ipd_new_master b";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND a.pano = b.pano";
                SQL += ComNum.VBLF + "    AND b.gbsts = '0'";
                SQL += ComNum.VBLF + "    AND B.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "    AND to_char(a.birth, 'mm') between to_char(sysdate, 'mm') and to_char(sysdate, 'mm')";
                SQL += ComNum.VBLF + "    AND to_char(a.birth, 'dd') >= to_char(sysdate,'dd')";
                SQL += ComNum.VBLF + "  ORDER BY to_char(a.birth, 'mm-dd')";
                SqlErr  = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
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


                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["INDATE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["SEX"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["AGE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 5].Text = Dt.Rows[i]["WARDCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 6].Text = Dt.Rows[i]["ROOMCODE"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["BIRTH"].ToString().Trim();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "재원자 생일명단";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + CU.GstrJobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

        }

        private void frmViewIpwonBirthday_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

        }
    }
}
