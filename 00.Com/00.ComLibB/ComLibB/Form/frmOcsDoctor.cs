using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Oracle.DataAccess.Client;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmOcsOdctor.cs
    /// Description     : 의사코드 등록 및 조회 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-14
    /// Update History  : 퇴사자는 서명란 공백 처리, 작업자사번 받아오는 생성자 추가
    /// <history>       
    /// D:\타병원\PSMHH\basic\bucode\BuCode18.frm(FrmOcsDoctor) => frmOcsDoctor.cs 으로 변경함
    /// VB IsEmpty 구현필요(VB IsNull로 대체하여 구현), 구현 되있으나 사용 안되는 함수들 주석처리
    /// 서명 등록하는 부분 구현되어있음 현재 테스트를 위해 테이블을 ETC_TELBOOK_IMAGE로 구현 해둠, 그리고 별도로 TestExceuteLongRawQuery를 만들어 구현함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\bucode\BuCode18.frm(FrmOcsDoctor)
    /// </seealso>
    /// <vbp>
    /// default         : C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\bucode\bucode.vbp
    /// seealso         :
    /// </vbp>
    /// </summary>
    public partial class frmOcsDoctor : Form
    {
        string mJobSabun = "";
                
        string strUpdateFLAG = "";
        string strInsertFLAG = "";
        string strDeptCode = "";
        string strGradeCode = "";
        string strGbOut = "";

        int nOptionChk = 0;            
        int nDeptCodeCnt = 0;            
        
                       
        public frmOcsDoctor()
        {
            InitializeComponent();
        }

        public frmOcsDoctor(string strJobSabun)
        {
            InitializeComponent();
            mJobSabun = strJobSabun;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmOcsDoctor_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            Combo_Load ();
            DeptDefine();
            ClearItem();

            cboDept.SelectedIndex = 0;
            optReferance0.Checked = true;
            optOut0.Checked = true;

            ssList_Sheet1.Columns[11].Visible = false;

            if(mJobSabun == "4349")
            {
                picSign.Enabled = true;
            }
        }

        void CheckCancelButtonEnable()
        {
            if (txtDrCode.Text != "" ||
                VB.Len(cboGrade.SelectedItem.ToString()) > 0 ||
                VB.Len(txtDrName.Text) > 0 ||
                VB.Len(cboMediDept.SelectedItem.ToString()) > 0 ||
                chkGbOut.Checked == true
                )
                { 
                    btnCancel.Enabled = true;
                }

            else
            {
                 btnCancel.Enabled = false;
            }
        }

        void CheckOkButtonEnable()
        {
            if (txtSabun.Text != "")
            {
                btnOk.Enabled = true;
            }
            else
            {
                btnOk.Enabled = false;
            }
        }

        void ClearItem()
        {
            txtDrCode.Text = "";
            txtSabun.Text = "";
            txtDrName.Text = "";

            cboGrade.SelectedIndex = 0;
            cboMediDept.Text = "";
            txtDrBunho.Text = "";

            txtSort.Text = "";
            chkGbOut.Checked = false;

            btnOk.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;

            for(int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                for(int j = 0; j < ssList_Sheet1.ColumnCount; j++)
                {
                    ssList_Sheet1.Cells[i, j].Text = "";
                }
            }

            strInsertFLAG = "NO";
            picSign.Image = null;
        }

        void Combo_Load()
        {
            cboGrade.Items.Clear();
            cboGrade.Items.Add("1.전문의");
            cboGrade.Items.Add("2.레지던트");
            cboGrade.Items.Add("3.인턴");
            cboGrade.Items.Add("4.일반의");
            cboGrade.Items.Add("5.전임의");
        }

        void DeptDefine()
        {
            int i = 0;
            string strDept = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  DeptCode,ROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
            SQL += ComNum.VBLF + "Order By PrintRanking";
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

            cboDept.Items.Add("ALL");
            for(i = 0; i < dt.Rows.Count; i++)
            {
                strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                cboDept.Items.Add(strDept);
                cboMediDept.Items.Add(strDept);
            }

            dt.Dispose();
            dt = null;
        }

        void NullChk1(int Rowcnt, int Colcnt)
        {
            ssList_Sheet1.RowCount = Rowcnt;
            ssList_Sheet1.ColumnCount = Colcnt;

            //TODO
            //VB.IsEmpty 대신 VB.IsNull로 대체하여 사용
            if(VB.IsNull(ssList_Sheet1.Cells[Rowcnt, Colcnt].Text) || ssList_Sheet1.Cells[Rowcnt, Colcnt].Text == "" || VB.IsNull(ssList_Sheet1.Cells[Rowcnt, Colcnt].Text))
            {
                for(int i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    for(int j = 0; j < ssList_Sheet1.ColumnCount; j++)
                    {
                        ssList_Sheet1.Cells[i, j].Text = "";
                    }
                }
            }
        }

        void chkGbOut_Click(object sender, EventArgs e)
        {
            CheckCancelButtonEnable();
        }

        void chkGbOut_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            ClearItem();
            txtSabun.Focus();
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
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            if (MessageBox.Show("삭제 시 복구 불가능합니다. 삭제하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  * FROM ";
            SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
            SQL += ComNum.VBLF + "WHERE  Sabun = '" + txtSabun.Text + "'";

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
                SqlErr = "";
                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "DELETE";
                    SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
                    SQL += ComNum.VBLF + "WHERE  Sabun = '" + txtSabun.Text + "'";
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

                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }
                ClearItem();
            }
            txtDrCode.Focus();
            dt.Dispose();
            dt = null;
        }

        //서명읽기
        void btnLoad_Click(object sender, EventArgs e)
        {
            Read_Sign();            
        }

        void Read_Sign(string sSabun = "")
        {
            string strSabun = "";
            DataTable dt = null;

            string SQL = "";
            string SqlErr = "";

            if (sSabun != "")
            {
                strSabun = sSabun;
            }
            else
            {
                strSabun = txtSabun.Text.Trim();
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  SIGNATURE ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                SQL += ComNum.VBLF + "WHERE Sabun = '" + strSabun + "'";
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

                if (dt.Rows.Count > 0)
                {
                    byte[] bImage = null;
                    bImage = (byte[])dt.Rows[0]["SIGNATURE"];
                    
                    if (bImage != null)
                    {
                        picSign.Image = null;
                        picSign.Image = new Bitmap(new MemoryStream(bImage));
                        picSign.SizeMode = PictureBoxSizeMode.StretchImage;
                        picSign.Visible = true;
                    }
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;


            //    아래는 기존 소스임

            //    FileStream Fs = nw FileStream();

            //    With Dialog
            //    .DialogTitle = "Open Image1 File...."
            //    .Filter = "All Files(*.*)|*.*| jpg file | *.JPG | Image1 Files (*.gif; *.bmp; *.jpg) | *.gif;*.bmp;*.jpg "
            //    '"All Files (*.*)|*.*|Text Files (*.txt)|*.txt|Batch Files (*.bat)|*.bat"

            //    .CancelError = True

            //    .ShowOpen

            //    If.FileName = "" Then
            //       MsgBox "Invalid filename or file not found.", _
            //            vbOKOnly + vbExclamation, "Oops!"


            //    Else
            //       FstrFileName = LCase(.FileName)


            //         'Image1.Picture = Nothing


            //        'Image1.Picture = LoadPicture()
            //        Image1.Picture = LoadPicture(FstrFileName)


            //       ' Picture1.Picture = LoadPicture(FstrFileName, vbLPLarge, vbLPColor)


            //        'If Not SavePictureToDB(rstRecordset, .FileName) Then
            //        '    MsgBox "Save was unsuccessful :(", vbOKOnly + _
            //        '            vbExclamation, "Oops!"
            //        '    Exit Sub
            //        'End If
            //    End If

            //End With
        }

        void btnOk_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }
            if (SaveData() == true)
            {
                ClearItem();
                txtSabun.Focus();
                GetData();
            }
        }

        bool SaveData()
        {
            string ChkGbOutvalue = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            bool rtnVal = false;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                switch (chkGbOut.Checked)
                {
                    case true:
                        ChkGbOutvalue = "Y";
                        break;

                    case false:
                        ChkGbOutvalue = "N";
                        break;
                }

                if (strInsertFLAG == "NO")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE";
                    SQL += ComNum.VBLF + ComNum.DB_MED + "";
                    SQL += ComNum.VBLF + "OCS_DOCTOR";
                    SQL += ComNum.VBLF + "SET    DrName       = '" + txtDrName.Text + "'";
                    SQL += ComNum.VBLF + "      ,DrCode       = '" + txtDrCode.Text + "' ";
                    SQL += ComNum.VBLF + "      ,Grade        = '" + VB.Left(cboGrade.SelectedItem.ToString(), 1) + "' ";
                    SQL += ComNum.VBLF + "      ,DeptCode     = '" + cboMediDept.SelectedItem.ToString() + "'";
                    SQL += ComNum.VBLF + "      ,GbOut        = '" + ChkGbOutvalue + "'  ";
                    SQL += ComNum.VBLF + "      ,DrBunho      =  " + Convert.ToInt16(txtDrBunho.Text) + " ";
                    SQL += ComNum.VBLF + "       ,Fdate        =  TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "       ,Tdate        =  TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "       ,SORT         =  '" + txtSort.Text + "'  ";
                    SQL += ComNum.VBLF + " WHERE  Sabun        = '" + txtSabun.Text + "' ";
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO ";
                    SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
                    SQL += ComNum.VBLF + "  ( Sabun,     DrCode,    Grade, ";
                    SQL += ComNum.VBLF + "  DrName,    DeptCode,  GbOut, DrBunho ,FDATE, TDATE, SORT, DOCCODE  )";
                    SQL += ComNum.VBLF + "VALUES (  ";
                    SQL += ComNum.VBLF + "  '" + txtSabun.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "  '" + txtDrCode.Text.Trim() + "',  ";
                    SQL += ComNum.VBLF + "  '" + VB.Left(cboGrade.SelectedItem.ToString(), 1) + "', ";
                    SQL += ComNum.VBLF + "  '" + txtDrName.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "  '" + cboMediDept.SelectedItem.ToString() + "', ";
                    SQL += ComNum.VBLF + "  '" + ChkGbOutvalue + "', ";
                    SQL += ComNum.VBLF + "  " + Convert.ToInt16(txtDrBunho.Text.Trim()) + ", ";
                    SQL += ComNum.VBLF + "  TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "  TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "  '" + txtSort.Text + "', ";
                    SQL += ComNum.VBLF + "  '" + Convert.ToInt16(txtSabun.Text.Trim()) + "'";
                    SQL += ComNum.VBLF + "  )";
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

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("수정하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        /// <summary>
        /// 서명등록하는 함수
        /// 기존 폼에서도 오류가 발생하므로 주석처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            string path = "";
            string curfile = "";


            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;            
            

            //의사 서명 사인 저장;
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + " SIGNATURE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
            SQL += ComNum.VBLF + "WHERE SABUN = '" + txtSabun.Text + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {               
                if (MessageBox.Show("Data가 존재하지 않습니다. 새로 등록 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                // 저장할 이미지 파일을 불러와 PictureBox에 보여줍니다.
                OpenFileDialog openDialog = new OpenFileDialog();
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    curfile = openDialog.FileName;
                    path = curfile;
                    picSign.Load(openDialog.FileName);
                }

                if(path != "")
                {
                    FileStream fs = new FileStream(curfile, FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] rawdata = new byte[fs.Length];
                    fs.Close();

                    clsDB.setBeginTran(clsDB.DbCon);

                    try
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "UPDATE KOSMOS_OCS.OCS_DOCTOR set image = " + " :Parameter  ";
                        SQL += ComNum.VBLF + "  WHERE SABUN = '" + txtSabun.Text + "' ";

                        SqlErr = TestExecutLongRawQuery(SQL, rawdata, "Parameter", ref intRowAffected, clsDB.DbCon, true);


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
                    }

                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }

                }
            }

               
        }

        string TestExecutLongRawQuery(string SQL, byte[] pfile, string strCol, ref int RowAffected, PsmhDb pDbCon, bool bTest = false)
        {
            OracleCommand Cmd = null;
            string rtnVal = "";
            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;
                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }
                if (bTest)
                {
                    //Cmd.Parameters.Add("@longdata", OracleDbType.LongRaw).Value = strCol;
                    Cmd.Parameters.Add(strCol, OracleDbType.LongRaw).Value = pfile;
                }
                else
                {
                    OracleParameter LongRawParameter = new OracleParameter();
                    LongRawParameter.OracleDbType = OracleDbType.LongRaw;
                    LongRawParameter.ParameterName = strCol;
                    LongRawParameter.Value = pfile;

                    Cmd.Parameters.Add(LongRawParameter);
                }


                RowAffected = Cmd.ExecuteNonQuery();


                return rtnVal;
            }
            catch (OracleException sqlExc)
            {
                ComFunc.MsgBox(sqlExc.Message);
                rtnVal = sqlExc.Message;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                rtnVal = ex.Message;
                return rtnVal;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
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
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;

            ssList_Sheet1.RowCount = 0;

            //Clear Spread Sheet
            for(i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                for(int j = 0; j < ssList_Sheet1.ColumnCount; j++)
                {
                    ssList_Sheet1.Cells[i, j].Text = "";
                }
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  O.*, O.RowId ,B.TOIDAY ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR O ," + ComNum.DB_ERP + "INSA_MST B";

                if(cboDept.SelectedItem.ToString() == "ALL")
                {
                    SQL += ComNum.VBLF + "Where   O.DeptCode IS NOT NULL ";
                }

                else
                {
                    SQL += ComNum.VBLF + "Where   O.DeptCode = '" + cboDept.SelectedItem.ToString() + "' ";
                }

                if(optOut0.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND   (O.Gbout <> 'Y' or o.Gbout is null) ";
                }

                SQL += ComNum.VBLF + "  AND O.SABUN  = B.SABUN ";

                switch (nOptionChk)
                {
                    case 0:
                        SQL += ComNum.VBLF + "ORDER BY O.DrCode ";
                        break;

                    case 1:
                        SQL += ComNum.VBLF + "ORDER BY O.DrName";
                        break;

                    case 2:
                        SQL += ComNum.VBLF + "ORDER BY O.DeptCode";
                        break;

                    case 3:
                        SQL += ComNum.VBLF + "ORDER BY O.SORT";
                        break;
                }

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
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            ssList_Sheet1.RowCount = dt.Rows.Count;
            for(i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                switch (dt.Rows[i]["Grade"].ToString().Trim())
                {
                    case "1":
                        ssList_Sheet1.Cells[i, 2].Text = "전문의";
                        break;
                    case "2":
                        ssList_Sheet1.Cells[i, 2].Text = "레지던트";
                        break;
                    case "3":
                        ssList_Sheet1.Cells[i, 2].Text = "인턴";
                        break;
                    case "4":
                        ssList_Sheet1.Cells[i, 2].Text = "일반의";
                        break;
                    case "5":
                        ssList_Sheet1.Cells[i, 2].Text = "전임의";
                        break;
                }
                ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DrName"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sabun"].ToString().Trim();
                ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GbOut"].ToString().Trim();
                ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DrBunho"].ToString().Trim();
                ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["FDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["TDATE"].ToString().Trim();
                ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["SORT"].ToString().Trim();
                if(VB.Len(dt.Rows[i]["SIGNATURE"].ToString().Trim()) > 0)
                {
                    ssList_Sheet1.Cells[i, 10].Text = "●";
                }
                else
                {
                    ssList_Sheet1.Cells[i, 10].Text = "";
                }
                ssList_Sheet1.Cells[i, 11].Text = dt.Rows[i]["RowID"].ToString().Trim();

                if (dt.Rows[i]["GbOut"].ToString() == "Y")
                {
                    //해당 Row에만 색상 처리하는 부분
                    ssList_Sheet1.Rows[i].ForeColor = Color.Red;
                }
                
                if(dt.Rows[i]["TOIDAY"].ToString().Trim() != "")
                {
                    ssList_Sheet1.Rows[i].ForeColor = Color.Yellow;
                }
            }

            dt.Dispose();
            dt = null;
        }

        void cboDept_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void cboGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckOkButtonEnable();
            CheckCancelButtonEnable();
        }

        void cboGrade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void cboMediDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckCancelButtonEnable();
        }

        void cboMediDept_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void cboMediDept_Leave(object sender, EventArgs e)
        {
            int i = 0;
            bool ChkFlag;
            string MediDept = cboMediDept.SelectedItem.ToString();

            ChkFlag = false;

            foreach (string item in cboMediDept.Items)
            {
                if (MediDept == item)
                {
                    ChkFlag = true;
                }
            }

            if(ChkFlag == false)
            {
                cboMediDept.Text = "";
            }
        }

        /// <summary>
        /// 서명등록하는 부분
        /// 기존 소스코드에서도 오류 발생, 주석처리 해놓음
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void button3_Click(object sender, EventArgs e)
        {
            int i, k;
            string FileName = "";

            DataTable dt = null;    // rsSing
            DataTable dt1 = null;   // strSTREAM


            string SQL = "";
            string SqlErr = "";

            string strPath = "";
            string strPathB = "";
            string strFile = "";
            string strSabun = "";

            strPath = "C:/cmc/sign";

            //FileSign.Path = strPath
            //FileSign.Refresh

            //for(i = 0; i < FileSign.ListCount; i++)

            //strFile = FileSign.List(i)

            //strSabun = Format(Trim(P(FileSign.List(i), ".", 1)), "00000")

            //의사 서명 사인 저장
            //Set strSTREAM = New ADODB.Stream

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + " * FROM ";
            SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
            SQL += ComNum.VBLF + "WHERE SABUN = '" + strSabun + "' ";
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

            //Set rs = New Recordset
            //rs.Open SQL, adoConnect, adOpenKeyset, adLockOptimistic

            if(dt.Rows.Count > 0)
            {
                //Set strSTREAM = New ADODB.Stream


                //strSTREAM.Type = adTypeBinary
                //strSTREAM.Open
                //strSTREAM.LoadFromFile strPath &"\" & strFile


                //rs.Fields("signature").Value = strSTREAM.Read
                //rs.Update
                //rs.Close

                //Dim fs
                //Set fs = CreateObject("Scripting.FileSystemObject")
                //fs.DeleteFile FileSign.Path & "\" & FileSign.List(i)
            }

            ComFunc.MsgBox("일괄 등록 완료");

            //ErrorHandler: ' 오류 처리 루틴입니다.
            //If 0 <> Err.Number Then
            //    MsgBox Err.Description, vbCritical, Err.Source

            //End If
        }

        void ssList_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            strUpdateFLAG = "OK";

            if(ssList_Sheet1.Cells[ssList_Sheet1.RowCount, 0].Text != "")
            {
                ssList_Sheet1.RowCount += 1;
            }
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            /// VB에서도 사용하지 않는 부분이라 주석처리 함
            
            //FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType =
            //             new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            //string[] cboItems = new string[3];
            //cboItems[0] = strDeptCode;
            //cboItems[1] = strGradeCode;
            //cboItems[2] = strGbOut;

            //switch (e.Column)
            //{
            //    case 2:
            //        ssList_Sheet1.Cells[e.Row, 2].CellType = comboBoxCellType;
            //        comboBoxCellType.Items = cboItems;

            //        break;
            //    case 3:
            //        ssList_Sheet1.Cells[e.Row, 2].CellType = comboBoxCellType;
            //        comboBoxCellType.Items[0] = cboItems[1];
            //        break;
            //    case 6:
            //        ssList_Sheet1.Cells[e.Row, 2].CellType = comboBoxCellType;
            //        comboBoxCellType.Items[0] = cboItems[2];
            //        break;                    

            //        if(nDeptCodeCnt == 0)
            //        {
            //            return;
            //        }
            //}

            if(e.Column == 0)
            {
                txtSabun.Text = ssList_Sheet1.Cells[e.Row, 4].Text;

                DataTable dt = null;
                string SQL = "";
                string SqlErr = "";

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  SABUN, GRADE, DRNAME, DEPTCODE, DRCODE, GBOUT, DRBUNHO, SIGNATURE, SORT,";
                    SQL += ComNum.VBLF + "  TO_CHAR(FDATE,'YYYY-MM-DD') FDATE,";
                    SQL += ComNum.VBLF + "  TO_CHAR(TDATE,'YYYY-MM-DD') TDATE";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                    SQL += ComNum.VBLF + "WHERE Sabun = '" + txtSabun.Text.Trim() + "'";
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

                    btnDelete.Enabled = true;

                    if (dt.Rows.Count > 0)
                    {
                        strInsertFLAG = "NO";

                        txtSabun.Text = dt.Rows[0]["Sabun"].ToString().Trim();
                        txtDrCode.Text = dt.Rows[0]["DrCode"].ToString().Trim();
                        cboGrade.SelectedIndex = Convert.ToInt16(dt.Rows[0]["Grade"].ToString().Trim()) - 1;
                        txtDrName.Text = dt.Rows[0]["DrName"].ToString().Trim();
                        cboMediDept.Text = dt.Rows[0]["Deptcode"].ToString().Trim();
                        txtDrBunho.Text = dt.Rows[0]["DrBunho"].ToString().Trim();

                        dtpFDate.Text = dt.Rows[0]["FDATE"].ToString().Trim();
                        if(VB.Left(dtpTDate.Text, 4) == "2999")
                        {
                            Read_Sign(txtSabun.Text);
                        }
                        // 퇴사자는 서명란 공백으로 만듬
                        else
                        {
                            picSign.Image = null;
                        }
                        dtpTDate.Text = dt.Rows[0]["TDATE"].ToString().Trim();
                        txtSort.Text = dt.Rows[0]["SORT"].ToString().Trim();
                        

                        switch (dt.Rows[0]["GbOut"].ToString().Trim())
                        {
                            case "y":
                                chkGbOut.Checked = true;
                                break;
                            case "Y":
                                chkGbOut.Checked = true;
                                break;
                            default:
                                chkGbOut.Checked = false;
                                break;
                        }

                        btnCancel.Enabled = true;
                        btnDelete.Enabled = true;
                    }
                    else
                    {
                        strInsertFLAG = "OK";
                        btnDelete.Enabled = false;
                    }
                    SendKeys.Send("{TAB}");


                }

                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
            }
        }

        void ssList_Enter(object sender, EventArgs e)
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  DeptCode";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
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

            if (dt.Rows.Count > 0)
            {
                nDeptCodeCnt = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if ((i + 1) == dt.Rows.Count)
                    {
                        strDeptCode += dt.Rows[i]["DeptCode"].ToString().Trim();
                    }
                    else
                    {
                        strDeptCode += dt.Rows[i]["DeptCode"].ToString().Trim() + VB.Chr(9);
                    }
                }
            }

            strGradeCode = "전문의" + VB.Chr(9) +
                           "레지던트" + VB.Chr(9) +
                           "인턴" + VB.Chr(9) +
                           "일반의";

            strGbOut = "Y" + VB.Chr(9) + "N";
        }

        void ssList_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string strROWID = "";
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

           

            FarPoint.Win.Spread.CellType.EditBaseCellType EditBaseCellType =
                         new FarPoint.Win.Spread.CellType.EditBaseCellType();

            if (e.Column == 1)
            {
                ssList_Sheet1.Cells[e.Row, 1].CellType = EditBaseCellType;
                ssList_Sheet1.Cells[e.Row, 1].Text.ToUpper();
            }
            if (e.Column == 2)
            {
                ssList_Sheet1.Cells[e.Row, 2].CellType = EditBaseCellType;
                ssList_Sheet1.Cells[e.Row, 2].Text.ToUpper();
            }
            if (e.Column == 4)
            {
                ssList_Sheet1.Cells[e.Row, 4].CellType = EditBaseCellType;
                ssList_Sheet1.Cells[e.Row, 4].Text.ToUpper();
            }
            if (e.Column == 5)
            {
                ssList_Sheet1.Cells[e.Row, 5].CellType = EditBaseCellType;
                ssList_Sheet1.Cells[e.Row, 5].Text.ToUpper();
            }

            strROWID = ssList_Sheet1.Cells[e.Row, 11].Text;

            if(strUpdateFLAG == "OK")
            {
                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(clsDB.DbCon);
                

                switch (e.Column)
                {
                    case 0:
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "UPDATE";
                            SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
                            SQL += ComNum.VBLF + "SET";
                            SQL += ComNum.VBLF + "DrCode   = ' ";
                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    case 1:
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "UPDATE";
                            SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
                            SQL += ComNum.VBLF + "SET";
                            SQL += ComNum.VBLF + "DeptCode = '";                           
                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    case 2:
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "UPDATE";
                            SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
                            SQL += ComNum.VBLF + "SET";
                            SQL += ComNum.VBLF + "Grade = '";                            
                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    case 3:
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "UPDATE";
                            SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
                            SQL += ComNum.VBLF + "SET";
                            SQL += ComNum.VBLF + "DrName = '";                            
                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    case 4:
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "UPDATE";
                            SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
                            SQL += ComNum.VBLF + "SET";
                            SQL += ComNum.VBLF + "Sabun = '";

                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    case 5:
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "UPDATE";
                            SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
                            SQL += ComNum.VBLF + "SET";
                            SQL += ComNum.VBLF + "GbOut = '";

                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    case 6:
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "UPDATE";
                            SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
                            SQL += ComNum.VBLF + "SET";
                            SQL += ComNum.VBLF + "DrBunho = '";
                           
                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    case 7:
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "UPDATE";
                            SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
                            SQL += ComNum.VBLF + "SET";
                            SQL += ComNum.VBLF + "FDATE = '";                            
                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    case 8:
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "UPDATE";
                            SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
                            SQL += ComNum.VBLF + "SET";
                            SQL += ComNum.VBLF + "TDATE = '";                           
                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    case 9:
                        try
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "UPDATE";
                            SQL += ComNum.VBLF + ComNum.DB_MED + "OCS_DOCTOR";
                            SQL += ComNum.VBLF + "SET";
                            SQL += ComNum.VBLF + "SORT = '";                           
                        }
                        catch (Exception ex)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                }
                switch (e.Column)
                {
                    case 3:
                        switch(ssList_Sheet1.Cells[e.Row, 2].Text)
                        {
                            case "전문의":
                                SQL += ComNum.VBLF + "1' ";
                                break;
                            case "레지던트":
                                SQL += ComNum.VBLF + "2' ";
                                break;
                            case "인턴":
                                SQL += ComNum.VBLF + "3' ";
                                break;
                            case "일반의":
                                SQL += ComNum.VBLF + "4' ";
                                break;
                        }
                        break;

                    case 7:
                        SQL += ComNum.VBLF + Convert.ToInt16(ssList_Sheet1.Cells[e.Row, 6].Text) + " ";
                        break;
                    case 8:
                        SQL += ComNum.VBLF + " TO_DATE('" + ssList_Sheet1.Cells[e.Row, 7].Text + "','YYYY-MM-DD')";
                        break;
                    case 9:
                        SQL += ComNum.VBLF + " TO_DATE('" + ssList_Sheet1.Cells[e.Row, 8].Text + "','YYYY-MM-DD')";
                        break;
                    default:
                        SQL += ComNum.VBLF + Convert.ToInt16(ssList_Sheet1.Cells[e.Row, e.Column].Text) + " ";
                        break;
                }
                SQL += ComNum.VBLF + "WHERE  ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("수정하였습니다.");
                Cursor.Current = Cursors.Default;

                strUpdateFLAG = "NO";
            }


        }

        void txtDrCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtDrName_TextChanged(object sender, EventArgs e)
        {
            CheckCancelButtonEnable();
        }

        void txtDrName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtDrName_Leave(object sender, EventArgs e)
        {
            //TODO VB cvtToEng 구현필요            
            //Call cvtToEng(FrmOcsDoctor)   
        }

        void txtDrBunho_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtSabun_TextChanged(object sender, EventArgs e)
        {
            txtDrCode.Text = "";
            txtDrName.Text = "";

            cboGrade.SelectedIndex = 0;
            cboMediDept.Text = "";
            txtDrBunho.Text = "";

            txtSort.Text = "";
            chkGbOut.Checked = false;
            CheckOkButtonEnable();
            CheckCancelButtonEnable();
        }

        void txtSort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtSabun_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar != 13)
            {
                return;
            }
            
            //TODO IsEmpty 구현필요
            //if(VB.IsNull(txtSabun.Text) || txtSabun.Text == "" || IsEmpty(txtSabun.Text))
            //{
            //    btnDelete.Enabled = false;
            //}

            txtSabun.Text = ComFunc.SetAutoZero(txtSabun.Text, 5);
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  SABUN, GRADE, DRNAME, DEPTCODE, DRCODE, GBOUT, DRBUNHO, SIGNATURE, SORT,";
                SQL += ComNum.VBLF + "  TO_CHAR(FDATE,'YYYY-MM-DD') FDATE,";
                SQL += ComNum.VBLF + "  TO_CHAR(TDATE,'YYYY-MM-DD') TDATE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR";
                SQL += ComNum.VBLF + "WHERE Sabun = '" + txtSabun.Text.Trim() + "'";
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

                btnDelete.Enabled = true;

                if (dt.Rows.Count > 0)
                {
                    strInsertFLAG = "NO";

                    txtSabun.Text = dt.Rows[0]["Sabun"].ToString().Trim();
                    txtDrCode.Text = dt.Rows[0]["DrCode"].ToString().Trim();
                    cboGrade.SelectedIndex = Convert.ToInt16(dt.Rows[0]["Grade"].ToString().Trim()) - 1;
                    txtDrName.Text = dt.Rows[0]["DrName"].ToString().Trim();
                    cboMediDept.Text = dt.Rows[0]["Deptcode"].ToString().Trim();
                    txtDrBunho.Text = dt.Rows[0]["DrBunho"].ToString().Trim();

                    dtpFDate.Text = dt.Rows[0]["FDATE"].ToString().Trim();
                    dtpTDate.Text = dt.Rows[0]["TDATE"].ToString().Trim();
                    txtSort.Text = dt.Rows[0]["SORT"].ToString().Trim();
                    Read_Sign(txtSabun.Text);

                    switch (dt.Rows[0]["GbOut"].ToString().Trim())
                    {
                        case "y":
                            chkGbOut.Checked = true;
                            break;
                        case "Y":
                            chkGbOut.Checked = true;
                            break;
                        default:
                            chkGbOut.Checked = false;
                            break;
                    }

                    btnCancel.Enabled = true;
                    btnDelete.Enabled = true;
                }
                else
                {
                    strInsertFLAG = "OK";
                    btnDelete.Enabled = false;
                }
                SendKeys.Send("{TAB}");


            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
