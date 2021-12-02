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
using FarPoint.Win.Spread;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmAccountConv1.cs
    /// Description     : 진찰료 계정 변환 Table 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-13
    /// Update History  : try-catch문 수정
    /// <history>       
    /// D:\타병원\PSMHH\basic\bucode\BuCode45.frm(FrmAccountConv1) => frmAccountConv1.cs 으로 변경함
    /// 프린트함수 부분에 ss2 시트가 존재하지 않으므로 주석처리
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\bucode\BuCode45.frm(FrmAccountConv1)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\bucode\bucode.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmAccountConv1 : Form
    {
        ComFunc CF = new ComFunc();
        public frmAccountConv1()
        {
            InitializeComponent();
        }

        void btnExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            Screen_Clear();
            cboJob.Focus();
        }

        void frmAccountConv1_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Screen_Clear();

            ssList_Sheet1.Columns[4].Visible = false;

            //작업종류를 ComboBox에 SET
            cboJob.Items.Clear();
            cboJob.Items.Add("1.진찰료변환");

            cboJob.SelectedIndex = 0;

            cboDate.Items.Clear();
            cboDate.Visible = false;

            optJob0.Checked = true;
            if (optJob0.Checked == true)
            {
                cboDate.Visible = false;
                dtpDate.Visible = true;
            }           

        }

        void Screen_Clear()
        {
            btnView.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            Spread_Clear();


            ssList.Enabled = false;
        }

        void Spread_Clear()
        {
            for (int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                for (int j = 0; j < ssList_Sheet1.ColumnCount; j++)
                {
                    ssList_Sheet1.Cells[i, j].Text = "";
                }
            }
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
            if (MessageBox.Show("화면의 자료를 정말로 삭제를 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            int i = 0;
            string strGubun = "";
            string strSDate = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            strGubun = VB.Left(cboJob.SelectedItem.ToString(), 1);

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            
            
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "DELETE ";
                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV";
                SQL += ComNum.VBLF + "WHERE Gubun = '" + strGubun + "' ";
                SQL += ComNum.VBLF + "  AND SDate = TO_DATE('" + cboDate.SelectedItem.ToString() + "','YYYY-MM-DD') ";
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

                Screen_Clear();
                ComboDate_SET();
                cboJob.Focus();
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

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            SaveData();
        }

        void SaveData()
        {
            int i = 0;
            string strDel = "";
            string strGubun = "";
            string strSDate = "";
            string strCode = "";
            string strSuNext = "";
            string strRemark = "";
            string strROWID = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            strGubun = VB.Left(cboJob.SelectedItem.ToString(), 1);
            strSDate = dtpDate.Text;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            if (optJob1.Checked == true)
            {
                strSDate = cboDate.SelectedItem.ToString().Trim();
            }
            try
            {
                for (i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strDel = ssList_Sheet1.Cells[i, 0].Text;
                    strCode = ssList_Sheet1.Cells[i, 1].Text;
                    strSuNext = ssList_Sheet1.Cells[i, 2].Text;
                    strRemark = ssList_Sheet1.Cells[i, 3].Text;
                    strROWID = ssList_Sheet1.Cells[i, 4].Text;

                    SQL = "";

                    if (strDel == "True")
                    {
                        if (strROWID != "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "DELETE ";
                            SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV";
                            SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        }
                    }
                    else
                    {
                        if (strROWID == "")
                        {

                            SQL = "";
                            SQL += ComNum.VBLF + "INSERT INTO ";
                            SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV";
                            SQL += ComNum.VBLF + "(Gubun,SDate,Code,SuNext,Remark) ";
                            SQL += ComNum.VBLF + "VALUES ( ";
                            SQL += ComNum.VBLF + "'" + strGubun + "', ";
                            SQL += ComNum.VBLF + "TO_DATE('" + strSDate + "','YYYY-MM-DD'), ";
                            SQL += ComNum.VBLF + "'" + strCode + "', ";
                            SQL += ComNum.VBLF + "'" + strSuNext + "', ";
                            SQL += ComNum.VBLF + "'" + strRemark + "' ";
                            SQL += ComNum.VBLF + ")";                           
                        }

                        else
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "UPDATE ";
                            SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV SET";
                            SQL += ComNum.VBLF + "  Code='" + strCode + "', ";
                            SQL += ComNum.VBLF + "  SuNext = '" + strSuNext + "', ";
                            SQL += ComNum.VBLF + "  Remark = '" + strRemark + "'";
                            SQL += ComNum.VBLF + "WHERE ROWID='" + strROWID + "'  ";                                   

                        }
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            GetData();
            cboJob.Focus();
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int i = 0;
            int nRow = 0;

            string strOldData = "";
            string strNewData = "";

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //자료를 SELECT
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  Gubun,TO_CHAR(SDate,'YYYY-MM-DD') SDate,";
            SQL += ComNum.VBLF + "  Code,SuNext,Remark";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV";
            SQL += ComNum.VBLF + "ORDER BY Gubun,SDate DESC,Code";
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

            strOldData = "";
            nRow = 0;

            // ss2 스프레드가 존재하지 않아서 보류
            for (i = 0; i < dt.Rows.Count; i++)
            {
                strNewData = VB.Left(dt.Rows[i]["Gubun"].ToString().Trim() + VB.Space(12), 12);
                strNewData += dt.Rows[i]["SDate"].ToString().Trim();                
                nRow = nRow + 1;

                if(nRow > ssList2.ActiveSheet.Rows.Count)
                {
                    ssList2.ActiveSheet.Rows.Count = nRow;
                }

                if(strNewData != strOldData)
                {
                    ssList2.ActiveSheet.Cells[nRow - 1, 0].Text = READ_ConvGbn_Name(dt.Rows[i]["Gubun"].ToString().Trim());
                    ssList2.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["SDate"].ToString().Trim();
                    strOldData = strNewData;
                }

                if(VB.Right(strOldData, 10) != VB.Right(strNewData, 10))
                {
                    ssList2.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["SDate"].ToString().Trim();
                }

                ssList2.ActiveSheet.Cells[nRow - 1, 2].Text = dt.Rows[i]["Code"].ToString().Trim();
                ssList2.ActiveSheet.Cells[nRow - 1, 3].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                ssList2.ActiveSheet.Cells[nRow - 1, 4].Text = dt.Rows[i]["Remark"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            if (ssList2.ActiveSheet.Rows.Count > 0)
            {

                clsSpread SPR = new clsSpread();
                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;


                strTitle = "진찰료,감액계정 변환정보(BAS_ACCOUNT_CONV)";

                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                SPR.setSpdPrint(ssList2, PrePrint, setMargin, setOption, strHeader, strFooter);
            }    

        }

        string READ_ConvGbn_Name(string argCode)
        {
            string rtnVal = "";

            switch (argCode)
            {
                case "1":
                    rtnVal = "진찰료 변환정보";
                    break;
                case "2":
                    rtnVal = "감액게정 변환정보";
                    break;
                default:
                    rtnVal = "** 오류 **";
                    break;

            }

            return rtnVal;
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
            string strGubun = "";
            string strSDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList.Enabled = true;
            ssList_Sheet1.RowCount = 0;

            Spread_Clear();

            strGubun = VB.Left(cboJob.SelectedItem.ToString(), 1);
            if(strGubun == "")
            {
                return;
            }
            try
            {
                if (optJob0.Checked == true)
                {
                    //신규자료
                    //자료를 READ
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  TO_CHAR(SDate,'YYYY-MM-DD') SDate,COUNT(*) CNT ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV";
                    SQL += ComNum.VBLF + "WHERE Gubun = '" + strGubun + "'";
                    SQL += ComNum.VBLF + "GROUP BY SDate";
                    SQL += ComNum.VBLF + "ORDER BY SDate DESC ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    //if (dt.Rows.Count == 0)
                    //{
                    //    dt.Dispose();
                    //    dt = null;
                    //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    //    return;
                    //}

                    strSDate = "";
                    if (dt.Rows.Count > 0)
                    {
                        strSDate = dt.Rows[0]["SDate"].ToString().Trim();
                    }

                    SqlErr = "";
                    dt.Dispose();
                    dt = null;

                    if (strSDate == "")
                    {
                        return;
                    }

                    //최종자료를 Sheet에 Display
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  Code,SuNext,Remark ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV";
                    SQL += ComNum.VBLF + "WHERE Gubun = '" + strGubun + "'";
                    SQL += ComNum.VBLF + "  AND SDate = TO_DATE('" + strSDate + "','YYYY-MM-DD')";

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

                    ssList_Sheet1.RowCount = dt.Rows.Count + 10;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = "";
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Code"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Remark"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = "";
                    }
                    dt.Dispose();
                    dt = null;


                }

                if (optJob1.Checked == true)
                {
                    cboDate.SelectedIndex = 2;
                    //수정작업
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  Code,SuNext,Remark,ROWID  ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV";
                    SQL += ComNum.VBLF + "WHERE Gubun = '" + strGubun + "'";
                    SQL += ComNum.VBLF + "  AND SDate = TO_DATE('" + cboDate.SelectedItem.ToString() + "','YYYY-MM-DD')";

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

                    ssList_Sheet1.RowCount = dt.Rows.Count + 10;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = "";
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Code"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SuNext"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Remark"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;
                }

                btnView.Enabled = false;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void cboJob_Click(object sender, EventArgs e)
        {
            //ComboDate_SET();
        }

        void ComboDate_SET()
        {
            int i = 0;
            string strGubun = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (cboJob.SelectedItem.ToString() == "")
            {
                return;
            }

            cboDate.Items.Clear();

            strGubun = VB.Left(cboJob.SelectedItem.ToString(), 1);

            if(strGubun == "")
            {
                return;
            }

            //자료를 READ
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  TO_CHAR(SDate,'YYYY-MM-DD') SDate,COUNT(*) CNT ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT_CONV";
                SQL += ComNum.VBLF + "WHERE Gubun = '" + strGubun + "'";
                SQL += ComNum.VBLF + "GROUP BY SDate";
                SQL += ComNum.VBLF + "ORDER BY SDate DESC ";

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

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    cboDate.Items.Add(dt.Rows[i]["SDate"].ToString().Trim());
                }

               

                if(dt.Rows.Count > 0)
                {
                    cboDate.SelectedIndex = 0;
                }

            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
        }

        void ssList_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string strSuNext = "";
            string strRemark = "";

            if(e.Column != 3)
            {
                return;
            }

            strSuNext = ssList_Sheet1.Cells[e.Row, 2].Text.Trim().ToUpper();
            strRemark = ssList_Sheet1.Cells[e.Row, 3].Text.Trim().ToUpper();
            if(strSuNext == "")
            {
                ssList_Sheet1.Cells[e.Row, 3].Text = "";
            }

            if(strRemark == "")
            {
                ssList_Sheet1.Cells[e.Row, 3].Text = CF.READ_SugaName(clsDB.DbCon, strSuNext);
            }
        }

        void optJob0_Click(object sender, EventArgs e)
        {
            cboDate.Visible = false;
            dtpDate.Visible = true;
        }

        void optJob1_Click(object sender, EventArgs e)
        {
            cboDate.Visible = true;
            dtpDate.Visible = false;
            btnDelete.Enabled = true;
            ComboDate_SET();
        }
    }
}
