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
    /// File Name       : frmService.cs
    /// Description     : 서비스평가항목 조회 및 등록 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-15
    /// Update History  : 
    /// <history>       
    /// D:\타병원\PSMHH\basic\bucode\FrmService.frm(FrmService) => frmService.cs 으로 변경함
    /// 폼 로드시 커서 스타일함수, 셀 TypeEditLen 부분 미적용
    /// 작업자이름을 받아오는 생성자 추가
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\bucode\FrmService.frm(FrmService)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\bucode\bucode.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmService : Form
    {
        string FstrROWID = "";
        public frmService()
        {
            InitializeComponent();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmService_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            COMBO_SET();
        }

        void COMBO_SET()
        {
            cboYY.Items.Clear();
            cboYY.Items.Add("2008");
            cboYY.SelectedIndex = 0;

            cboYY2.Items.Clear();
            cboYY2.Items.Add("2008");
            cboYY2.SelectedIndex = 0;

            cboGB.Items.Clear();
            cboGB.Items.Add("1.진료 및 운영체계");
            cboGB.Items.Add("2.부분별 업무 성과");
            cboGB.SelectedIndex = -1;

            cboPart.Items.Clear();
            cboPart.Items.Add("1.1.환자의 권리와 편의");
            cboPart.Items.Add("1.2.인력환리");
            cboPart.Items.Add("1.3.진료체계");
            cboPart.Items.Add("1.4.감염관리");
            cboPart.Items.Add("1.5.시설환경관리");
            cboPart.Items.Add("1.6.질 향상과 환자안전");
            cboPart.Items.Add("2.1.환자진료");
            cboPart.Items.Add("2.2.의료정보/의무기록");
            cboPart.Items.Add("2.3.영양");
            cboPart.Items.Add("2.4.응급");
            cboPart.Items.Add("2.5.수술관리체계");
            cboPart.Items.Add("2.6.검사");
            cboPart.Items.Add("2.7.약제관리");
            cboPart.Items.Add("2.8.중환자");
            cboPart.Items.Add("2.9.모성과 신생아");
            cboPart.SelectedIndex = -1;

            cboJosa.Items.Clear();
            cboJosa.Items.Add("간호사1");
            cboJosa.Items.Add("간호사2");
            cboJosa.Items.Add("약사");
            cboJosa.Items.Add("의사1");
            cboJosa.Items.Add("의사2");
            cboJosa.Items.Add("영양사");
            cboJosa.Items.Add("병원관리자");
            cboJosa.Items.Add("의무기록사");
            cboJosa.SelectedIndex = -1;

            cboJosa2.Items.Clear();
            cboJosa2.Items.Add("간호사1");
            cboJosa2.Items.Add("간호사2");
            cboJosa2.Items.Add("약사");
            cboJosa2.Items.Add("의사1");
            cboJosa2.Items.Add("의사2");
            cboJosa2.Items.Add("영양사");
            cboJosa2.Items.Add("병원관리자");
            cboJosa2.Items.Add("의무기록사");
            cboJosa2.SelectedIndex = -1;

            Screen_Clear();
        }

        void Screen_Clear()
        {
            cboGB.SelectedIndex = -1;
            cboJosa.SelectedIndex = -1;
            cboPart.SelectedIndex = -1;
            cboJosa.Text = "";

            txtNo.Text = "";
            txtItem.Text = "";
            txtTable.Text = "";
            txtSQL.Text = "";
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            DelData();
        }

        void DelData()
        {
            if(FstrROWID == "")
            {
                ComFunc.MsgBox("삭제할 항목을 선택하세요. ");
                return;
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "DELETE ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_SERVICE";
                SQL += ComNum.VBLF + "WHERE ROWID = '" + FstrROWID + "' ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;

            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void btnReturn_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            FstrROWID = "";

            txtNo.Focus();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            if(SaveData() == true)
            {
                Screen_Clear();
                FstrROWID = "";
                txtNo.Focus();
            }
        }

        bool SaveData()
        {
            bool rtnVal = false;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if(FstrROWID == "")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO ";
                    SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_SERVICE";
                    SQL += ComNum.VBLF + "( SYEAR, SGB, SPART, SNO, SITEM, SJOSA, SEDPS)";
                    SQL += ComNum.VBLF + "VALUES(  ";
                    SQL += ComNum.VBLF + "  '" + cboYY.Text + "', ";
                    SQL += ComNum.VBLF + "  '" + cboGB.Text + "', ";
                    SQL += ComNum.VBLF + "  '" + cboPart.Text + "', ";
                    SQL += ComNum.VBLF + "  '" + txtNo.Text + "', ";
                    SQL += ComNum.VBLF + "  '" + txtItem.Text + "', ";
                    SQL += ComNum.VBLF + "  '" + cboJosa.Text + "', ";
                    SQL += ComNum.VBLF + "  '" + VB.IIf(chkEdps.Checked == true, "*", "") + "' ";
                    SQL += ComNum.VBLF + ")";
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE ";
                    SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_SERVICE SET";
                    SQL += ComNum.VBLF + "  SYEAR  ='" + cboYY.Text + "' , ";
                    SQL += ComNum.VBLF + "  SGB = '" + cboGB.Text + "',  ";
                    SQL += ComNum.VBLF + "  SPART = '" + cboPart.Text + "' , ";
                    SQL += ComNum.VBLF + "  SNO = '" + txtNo.Text + "' ,  ";
                    SQL += ComNum.VBLF + "  SITEM = '" + txtItem.Text + "' , ";
                    SQL += ComNum.VBLF + "  SJOSA = '" + cboJosa.Text + "',";
                    SQL += ComNum.VBLF + "  SEDPS =  '" + VB.IIf(chkEdps.Checked == true, "*", "") + "' ";
                    SQL += ComNum.VBLF + "  WHERE ROWID = '" + FstrROWID + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                //clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;

            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            GetData();
        }
        
        void GetData()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssList_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  SYEAR, SGB, SPART, SNO, SITEM, SJOSA, SEDPS, ROWID";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SERVICE";
                SQL += ComNum.VBLF + "WHERE SYEAR = '" + cboYY2.SelectedItem.ToString() + "' ";
                if(cboJosa2.SelectedIndex != -1)
                {
                    SQL += ComNum.VBLF + "  AND SJOSA = '" + cboJosa2.SelectedItem.ToString() + "'";
                }
                SQL += ComNum.VBLF + "ORDER BY SGB, SPART, SNO";

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssList_Sheet1.RowCount = dt.Rows.Count;

                for(i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SNO"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SITEM"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SJOSA"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SYEAR"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SGB"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SPART"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SEDPS"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";

            ssList_Sheet1.Columns[4].Visible = false;
            ssList_Sheet1.Columns[5].Visible = false;
            ssList_Sheet1.Columns[6].Visible = false;
            ssList_Sheet1.Columns[7].Visible = false;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + " 서비스 평가항목" + "/n";
            strHead2 = "/l/f2" + "인쇄일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss") + VB.Space(107) + "PAGE : /p";
            
            ssList_Sheet1.PrintInfo.Header = strHead1 + strFont1 + "/n" + strFont2 + strHead2;

            ssList_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;

            ssList_Sheet1.PrintInfo.Margin.Top = 50;
            ssList_Sheet1.PrintInfo.Margin.Bottom = 2000;
            ssList_Sheet1.PrintInfo.Margin.Left = 100;
            ssList_Sheet1.PrintInfo.Margin.Right = 0;

            ssList_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssList_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;

            ssList_Sheet1.PrintInfo.ShowBorder = true;
            ssList_Sheet1.PrintInfo.ShowColor = false;
            ssList_Sheet1.PrintInfo.ShowGrid = true;
            ssList_Sheet1.PrintInfo.ShowShadows = true;
            ssList_Sheet1.PrintInfo.UseMax = true;
            
            ssList_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssList_Sheet1.PrintInfo.Preview = true;
            ssList.PrintSheet(0);

            ssList_Sheet1.Columns[4].Visible = true;
            ssList_Sheet1.Columns[5].Visible = true;
            ssList_Sheet1.Columns[6].Visible = true;
            ssList_Sheet1.Columns[7].Visible = true;
        }

        void cboGB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void cboJosa_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void cboPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void cboYY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            txtNo.Text = ssList_Sheet1.Cells[e.Row, 0].Text;
            txtItem.Text = ssList_Sheet1.Cells[e.Row, 1].Text;
            cboJosa.Text = ssList_Sheet1.Cells[e.Row, 2].Text;
            cboYY.Text = ssList_Sheet1.Cells[e.Row, 3].Text;
            cboGB.Text = ssList_Sheet1.Cells[e.Row, 4].Text;
            cboPart.Text = ssList_Sheet1.Cells[e.Row, 5].Text;
            FstrROWID = ssList_Sheet1.Cells[e.Row, 6].Text;

            if (ssList_Sheet1.Cells[e.Row, 7].Text == "*")
            {
                chkEdps.Checked = true;
            }
            else
            {
                chkEdps.Checked = false;
            }

            txtItem.Focus();
        }

        void txtItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar != 13)
            {
                return;
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  SYEAR, SGB, SPART, SNO, SITEM, SJOSA, ROWID ";
                SQL += ComNum.VBLF + "FROM " +ComNum.DB_PMPA + "BAS_SERVICE";
                SQL += ComNum.VBLF + " WHERE SYEAR = '" + cboYY2.SelectedItem.ToString() + "' ";
                SQL += ComNum.VBLF + "  AND SNO = '" + txtNo.Text + "'";
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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    txtItem.Text = dt.Rows[0]["SITEM"].ToString();
                    cboJosa.Text = dt.Rows[0]["SJOSA"].ToString();
                    cboYY.Text = dt.Rows[0]["SYEAR"].ToString();
                    cboGB.Text = dt.Rows[0]["SGB"].ToString();
                    cboPart.Text = dt.Rows[0]["SPART"].ToString();
                    FstrROWID = dt.Rows[0]["ROWID"].ToString();
                }

                SendKeys.Send("{TAB}");

                dt.Dispose();
                dt = null;

            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}
