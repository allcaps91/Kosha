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
    /// Class Name      : ComLibB
    /// File Name       : frmYGuipView.cs
    /// Description     : 의약품 실구입 신고내역 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-05
    /// Update History  : try-catch문 수정, GstrHelpCode, GstrJobSabun을 받아오는 생성자 추가
    /// <history>       
    /// D:\타병원\PSMHH\drug\drmain\dredi06.frm(FrmYGuipView) => frmYguipView.cs 으로 변경함
    /// frmBCodeHelp 폼 구현 필요, BuSuga00 -> READ_EDI_SUGA 구현필요, EDI_SUGA테이블의 자료를 담을 변수 필요(TODO처리해놓음)
    /// <history>       
    /// <seealso>
    /// D:\타병원\PSMHH\drug\drmain\dredi06.frm(FrmYGuipView)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\drug\drmain\drmain.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmYGuipView : Form
    {
        //string GstrHelpCode = "";
        //string GnJobSabun = "41827";

        string mstrHelpCode = "";
        string mstrJobSabun = "";

        private frmSearchBCode frmSearchBCodeX = null;
        struct TABLE_EDI_SUGA
        {
            public string ROWID;
            public string Code;
            public string Jong;
            public string Pname;
            public string Bun;
            public string Danwi1;
            public string Danwi2;
            public string Spec;
            public string COMPNY;
            public string Effect;
            public string Gubun;
            public string Dangn;
            public string JDate1;
            public string Price1;
            public string JDate2;
            public string Price2;
            public string JDate3;
            public string Price3;
            public string JDate4;
            public string Price4;
            public string JDate5;
            public string Price5;
        }

        TABLE_EDI_SUGA TES = new TABLE_EDI_SUGA();

        void READ_EDI_SUGA(string ArgCode)
        {
            //string cEdiKey = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            //cEdiKey = VB.Left(ArgCode + VB.Space(9), 8);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  ROWID vROWID,Code vCode,Jong vJong,";
                SQL += ComNum.VBLF + "  Pname vPname,Bun vBun,Danwi1 vDanwi1,";
                SQL += ComNum.VBLF + "  Danwi2 vDanwi2,Spec vSpec,Compny vCompny,";
                SQL += ComNum.VBLF + "  Effect vEffect,Gubun vGubun,Dangn vDangn,";
                SQL += ComNum.VBLF + "  TO_CHAR(JDate1,'YYYY-MM-DD') vJDate1,Price1 vPrice1,";
                SQL += ComNum.VBLF + "  TO_CHAR(JDate2,'YYYY-MM-DD') vJDate2,Price2 vPrice2,";
                SQL += ComNum.VBLF + "  TO_CHAR(JDate3,'YYYY-MM-DD') vJDate3,Price3 vPrice3,";
                SQL += ComNum.VBLF + "  TO_CHAR(JDate4,'YYYY-MM-DD') vJDate4,Price4 vPrice4,";
                SQL += ComNum.VBLF + "  TO_CHAR(JDate5,'YYYY-MM-DD') vJDate5,Price5 vPrice5 ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "EDI_SUGA";
                SQL += ComNum.VBLF + "WHERE Code = '" + ArgCode + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                TES.ROWID = "";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count == 1)
                {
                    TES.ROWID = dt.Rows[0]["vROWID"].ToString().Trim();
                    TES.Code = dt.Rows[0]["vCode"].ToString().Trim();
                    TES.Jong = dt.Rows[0]["vJong"].ToString().Trim();
                    TES.Pname = dt.Rows[0]["vPname"].ToString().Trim();
                    TES.Bun = dt.Rows[0]["vBun"].ToString().Trim();
                    TES.Danwi1 = dt.Rows[0]["vDanwi1"].ToString().Trim();
                    TES.Danwi2 = dt.Rows[0]["vDanwi2"].ToString().Trim();
                    TES.Spec = dt.Rows[0]["vSpec"].ToString().Trim();
                    TES.COMPNY = dt.Rows[0]["vCompny"].ToString().Trim();
                    TES.Effect = dt.Rows[0]["vEffect"].ToString().Trim();
                    TES.Gubun = dt.Rows[0]["vGubun"].ToString().Trim();
                    TES.Dangn = dt.Rows[0]["vDangn"].ToString().Trim();
                    TES.JDate1 = dt.Rows[0]["vJDate1"].ToString().Trim();
                    TES.Price1 = dt.Rows[0]["vPrice1"].ToString().Trim();
                    TES.JDate2 = dt.Rows[0]["vJDate2"].ToString().Trim();
                    TES.Price2 = dt.Rows[0]["vPrice2"].ToString().Trim();
                    TES.JDate3 = dt.Rows[0]["vJDate3"].ToString().Trim();
                    TES.Price3 = dt.Rows[0]["vPrice3"].ToString().Trim();
                    TES.JDate4 = dt.Rows[0]["vJDate4"].ToString().Trim();
                    TES.Price4 = dt.Rows[0]["vPrice4"].ToString().Trim();
                    TES.JDate5 = dt.Rows[0]["vJDate5"].ToString().Trim();
                    TES.Price5 = dt.Rows[0]["vPrice5"].ToString().Trim();
                }
                else
                {
                    TES.ROWID = ""; TES.Code = ""; TES.Jong = "";
                    TES.Pname = ""; TES.Bun = ""; TES.Danwi1 = "";
                    TES.Danwi2 = ""; TES.Spec = ""; TES.COMPNY = "";
                    TES.Effect = ""; TES.Gubun = ""; TES.Dangn = "";
                    TES.JDate1 = ""; TES.Price1 = ""; TES.JDate2 = ""; TES.Price2 = "";
                    TES.JDate3 = ""; TES.Price3 = ""; TES.JDate4 = ""; TES.Price4 = "";
                    TES.JDate5 = ""; TES.Price5 = "";
                }

                dt.Dispose();
                dt = null;

            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
        public frmYGuipView()
        {
            InitializeComponent();
        }

        public frmYGuipView(string strHelpCode, string strJobSabun)
        {
            InitializeComponent();
            mstrHelpCode = strHelpCode;
            mstrJobSabun = strJobSabun;
        }

        void frmYGuipView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SCREEN_CLEAR();
            
            if(mstrHelpCode != "")
            {
                txtData.Text = mstrHelpCode;
                GetData();
            }

        }
        void btnHelp_Click(object sender, EventArgs e)
        {
            frmSearchBCodeX = new frmSearchBCode();
            frmSearchBCodeX.rEventClosed += new frmSearchBCode.EventClosed(frmSearchBCodeX_EventClose);
            frmSearchBCodeX.Show();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmSearchBCodeX_EventClose()
        {
            frmSearchBCodeX.Dispose();
            frmSearchBCodeX = null;
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            txtData.Text = "";
            SCREEN_CLEAR();
        }

        void SCREEN_CLEAR()
        {
            txtBun.Text = "";
            txtDanwi.Text = "";
            txtData.Text = "";
            txtJejo.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";

            ssYGuipView_Sheet1.RowCount = 30;

            for(int i = 0; i < ssYGuipView_Sheet1.RowCount; i++)
            {
                for(int j = 0; j < ssYGuipView_Sheet1.ColumnCount; j++)
                {
                    ssYGuipView_Sheet1.Cells[i, j].Text = "";
                }
            }

            ssYGuipView.Enabled = false;
        }

        void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar != 13)
            {
                return;
            }
            GetData();
        }

        void GetData()
        {
            int i = 0;
            //string strPano = "";

            string SQL = "";
            string SqlErr = ""; // 에러문 받는 변수
            DataTable dt = null;

            txtData.Text = txtData.Text.Trim().ToUpper();
            if(txtData.Text == "")
            {
                return;
            }

            READ_EDI_SUGA(txtData.Text);
            if(TES.Pname == "")
            {
                ComFunc.MsgBox(txtData.Text + "가 표준코드에 등록 않됨");
                txtData.Text = "";
                txtData.Focus();
            }

            txtName.Text = " " + TES.Pname;
            txtSpec.Text = " " + TES.Spec.Trim() + " " + TES.Effect.Trim();
            txtDanwi.Text = TES.Danwi1.Trim() + TES.Danwi2.Trim();
            txtJejo.Text = " " + TES.COMPNY.Trim();
            txtBun.Text = TES.Bun.Trim();

            ssYGuipView.Enabled = true;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(GDate,'YYYY-MM-DD') GDate,Gbn, ";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(SDate,'YYYY-MM-DD') SDate, ";
                SQL = SQL + ComNum.VBLF + "    TotQty,TotAmt,Price, rowid  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "EDI_YAKGUIP a";
                SQL = SQL + ComNum.VBLF + "WHERE Bcode = '" + txtData.Text.Trim() + "'";
                SQL = SQL + ComNum.VBLF + "ORDER BY GDate DESC,SDate DESC ";
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
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssYGuipView_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssYGuipView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GDate"].ToString().Trim();
                    ssYGuipView_Sheet1.Cells[i, 1].Text = string.Format("{0:#,##0}", dt.Rows[i]["TotQty"]);
                    ssYGuipView_Sheet1.Cells[i, 2].Text = string.Format("{0:#,##0}", dt.Rows[i]["TotAmt"]);
                    ssYGuipView_Sheet1.Cells[i, 3].Text = string.Format("{0:#,##0}", Convert.ToDouble(dt.Rows[i]["TotAmt"].ToString().Trim()) / Convert.ToDouble(dt.Rows[i]["TotQty"].ToString().Trim()));
                    ssYGuipView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Gbn"].ToString().Trim();
                    ssYGuipView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SDate"].ToString().Trim();
                    ssYGuipView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["rowid"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }

            
        }

        void ssYGuipView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strROWID = "";
            string SQL = "";
            string SqlErr = "";     //에러문 받는 변수

            int intRowAffected = 0; //변경된 Row 받는 변수

            strROWID = ssYGuipView_Sheet1.Cells[e.Row, 7].Text;

            if (MessageBox.Show("해당 구입내역을 삭제 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "INSERT INTO EDI_YAKGUIP_DEL (SDATE,SEQNO,BUNGI,Gbn,BCode,GDATE,TOTQTY,TOTAMT,AVGAMT,BASEAMT,GASANAMT,PRICE,DelDate,DELSABUN) ";
                SQL = SQL + ComNum.VBLF + "SELECT SDATE,SEQNO,BUNGI,Gbn,BCode,GDATE,TOTQTY,TOTAMT,AVGAMT,BASEAMT,GASANAMT,PRICE, SYSDATE, '" + mstrJobSabun + "'";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.EDI_YAKGUIP ";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE EDI_YAKGUIP ";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                GetData();
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
       
    }
}
