using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// Class Name      : ComLibB.dll
    /// File Name       : frmCurtailSet.cs
    /// Description     : 감액 대상자 관리
    /// Author          : 김효성
    /// Create Date     : 2017-06-26
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// VB\basic\bugamf\Frm감액설정.frm => frmCurtailSet.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\bugamf\Frm감액설정.frm(Frm감액설정)
    /// </seealso>
    /// <vbp>
    /// default : VB\basic\bugamf\bugamf.vbp
    /// </vbp>

    public partial class frmCurtailSet : Form
    {
        string FstrROWID = "";
        string GJobSabun = "";
        string GstrPassProgramID = "";

        public frmCurtailSet ()
        {
            InitializeComponent ();
        }

        public frmCurtailSet (string strSabun , string strProgramID)
        {
            InitializeComponent ();
            //test 
            //GJobSabun = Convert.ToString (0345454);
            GJobSabun = strSabun;
            GstrPassProgramID = strProgramID;
        }

        private void frmCurtailSet_Load (object sender , EventArgs e)
        {
            ComFunc CF = new ComFunc();

            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }


            CF.dtpClear(dtpEnter);
            CF.dtpClear(dtpOut);
            CF.dtpClear(dtpEnd);
            
            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ComFunc.ReadSysDate(clsDB.DbCon);
            Init ();
            Gubun_Fill ();

            cboGbn.Items.Clear ();
            cboGbn.Items.Add (" ");
            cboGbn.Items.Add ("1.서울관구");
            cboGbn.Items.Add ("2.대구관구");
            cboGbn.Items.Add ("3.부산관구");
            cboGbn.Items.Add ("4.총    원");

            cboGbn1.Items.Clear ();
            cboGbn1.Items.Add ("전체");
            cboGbn1.Items.Add (" ");
            cboGbn1.Items.Add ("1.서울관구");
            cboGbn1.Items.Add ("2.대구관구");
            cboGbn1.Items.Add ("3.부산관구");
            cboGbn1.Items.Add ("4.총    원");
            cboGbn1.SelectedIndex = 0;

            btnDelete.Enabled = false;
            btnSave.Enabled = true;

            ssView_Sheet1.Columns [12].Visible = false;
            panExcel.Visible = false;
             

        }

        private void btnExit_Click (object sender , EventArgs e)
        {
            this.Close ();
        }

        private void btnJoinSerch_Click (object sender , EventArgs e)
        {
            frmbugamf2 frm = new frmbugamf2 ();
            frm.Show ();
        }

        private void cboGubun_Click (object sender , EventArgs e)
        {

        }

        private bool Save ()
        {
            bool rtnVal = false;

            if (ComQuery.IsJobAuth(this , "C", clsDB.DbCon) == false) return rtnVal; //rtnVal;


            if (BAS_GAMF_Save_Check () == 0) return rtnVal;
            if (FstrROWID == "") READ_BAS_GAMF (txtJumin0.Text.Trim () + txtJumin1.Text.Trim ());
            if (FstrROWID == "") BAS_GAMF_INSERT ();
            else BAS_GAMF_Update ();

            GamYegJaeSimSungJigjaSave ();//재단 성직자관구저장
            INSERT_BAS_GAMF_HIS ();
            AllHubulTagetingJoin ();//일괄후불대상자등록
            Init ();

            txtJumin0.Focus ();
            rtnVal = true;
            return rtnVal;
        }

        private void btnSave_Click (object sender , EventArgs e)
        {
            Save ();
        }

        private bool Delet ()
        {
            int i = 0;
            bool rtnVal = false;
            string SQL = "";
            string strOK = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ComQuery.IsJobAuth(this , "D", clsDB.DbCon) == false) return rtnVal; //rtnVal;

            Cursor.Current = Cursors.WaitCursor;

            if (FstrROWID != "")
            {
                if (ComFunc.MsgBoxQ ("정말 선택한 대상자를 삭제처리 하시겠습니까?" , "확인" , MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    clsDB.setBeginTran(clsDB.DbCon);
                    

                    try
                    {
                        SQL = "DELETE FROM BAS_GAMF WHERE ROWID = '" + FstrROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                        clsDB.setCommitTran (clsDB.DbCon);
                        ComFunc.MsgBox ("저장하였습니다.");
                        Cursor.Current = Cursors.Default;
                        rtnVal = true;
                        return rtnVal;
                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran (clsDB.DbCon);
                        ComFunc.MsgBox (ex.Message);
                        clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                ComFunc.MsgBox ("선택 후 삭제하세요" , "오류");
                return false;
            }
        }

        private void btnDelete_Click (object sender , EventArgs e)
        {
            if (Delet () == true)
            {
                Init ();
                Serch ();
                txtJumin0.Focus ();
            }
        }

        private void btnCancel_Click (object sender , EventArgs e)
        {
            Init ();
            txtJumin0.Focus ();
            Serch ();
            txtJumin0.Focus ();
        }
        private void Serch ()
        {
            int i = 0;
            int nRead = 0;
            string strGamGbn = "";
            string strGamSabun = "";
            string strGbn = "";
            string strPsmh = "";
            string strOK = "";
            string strJumin = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strSname = ""; ;
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = ""; //에러문 받는 변수

            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return;//권한 확인

            try
            {
                strGbn = VB.Pstr ((cboGbn1.Text).Trim () , "." , 1);
                strGamGbn = VB.Pstr ((cboGubun1.Text).Trim () , "." , 1);
                strGamSabun = VB.Format (txttBotomSabun , "000000");
                strSname = txttBotomname.Text;
                strPsmh = (chkBotomSungmo.Checked == true ? "1" : "");

                FstrROWID = "";

                SQL = " SELECT a.GamJumin , a.GamSabun, a.Gamcode, a.GamJumin3, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(a.GamEnter,'YYYY-MM-DD') GamEnter, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(a.GamOut,'YYYY-MM-DD') GamOut, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(a.GamEnd,'YYYY-MM-DD') GamEnd, ";
                SQL = SQL + ComNum.VBLF + "      a.GamMessage, a.GamName, a.GamSosok, a.ROWID,b.Gubun,b.PSMH ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_GAMF a," + ComNum.DB_PMPA + "BAS_GAMF_SR b  ";
                SQL = SQL + ComNum.VBLF + "  WHERE  a.GamJumin3 IS NOT NULL ";
                SQL = SQL + ComNum.VBLF + "   AND  a.GamJumin3=b.GamJumin3(+) ";

                if (strGamGbn != "") SQL = SQL + ComNum.VBLF + "   AND a.GamCode ='" + strGamGbn + "' ";
                if (strJumin1 != "") SQL = SQL + ComNum.VBLF + "   AND a.GamJumin ='" + strJumin1 + "' ";
                if (strSname != "") SQL = SQL + ComNum.VBLF + "   AND a.Gamname like '%" + strSname + "%' ";
                if (strGamSabun != "") SQL = SQL + ComNum.VBLF + "   AND a.GamSabun ='" + strGamSabun + "' ";
                if (strPsmh != "") SQL = SQL + ComNum.VBLF + "   AND b.PSMH ='" + strPsmh + "' ";

                SQL = SQL + ComNum.VBLF + " ORDER BY A.gamname ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                nRead = dt.Rows.Count;

                if (nRead > 200)
                {
                    if (ComFunc.MsgBoxQ ("조회건수 200건만 조회하시겠습니까?" , "확인" , MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        nRead = 200;
                    }
                }

                ssView_Sheet1.RowCount = nRead;
                ssView_Sheet1.SetRowHeight (-1 , ComNum.SPDROWHT);

                for (i = 0; i < nRead; i++)
                {
                    strOK = "OK";

                    if (strGbn != "")
                    {
                        if (strGbn != dt.Rows [i] ["Gubun"].ToString ().Trim ()) { strOK = ""; }
                        if (strGbn == "전체") { strOK = "OK"; }
                    }
                    else
                    {
                        if (dt.Rows [i] ["Gubun"].ToString ().Trim () != "") strOK = "";
                    }

                    if (strOK == "OK")
                    {
                        strJumin = clsAES.DeAES ((dt.Rows [i] ["GamJumin3"].ToString ().Trim ()));
                        strJumin1 = VB.Left (strJumin , 6);
                        strJumin2 = VB.Mid (strJumin , 7 , 7);

                        ssView_Sheet1.Cells [i , 1].Text = clsAES.DeAES ((dt.Rows [i] ["gamjumin3"].ToString ().Trim ()));
                        ssView_Sheet1.Cells [i , 2].Text = dt.Rows [i] ["gamsabun"].ToString ().Trim ();
                        ssView_Sheet1.Cells [i , 3].Text = dt.Rows [i] ["gamCode"].ToString ().Trim ();
                        ssView_Sheet1.Cells [i , 4].Text = dt.Rows [i] ["gamenter"].ToString ().Trim ();
                        ssView_Sheet1.Cells [i , 5].Text = dt.Rows [i] ["gamout"].ToString ().Trim ();
                        ssView_Sheet1.Cells [i , 6].Text = dt.Rows [i] ["gamend"].ToString ().Trim ();
                        ssView_Sheet1.Cells [i , 7].Text = dt.Rows [i] ["gammessage"].ToString ().Trim ();
                        ssView_Sheet1.Cells [i , 8].Text = dt.Rows [i] ["gamname"].ToString ().Trim ();
                        ssView_Sheet1.Cells [i , 9].Text = dt.Rows [i] ["gamsosok"].ToString ().Trim ();
                        ssView_Sheet1.Cells [i , 9].Text = dt.Rows [i] ["gamsosok"].ToString ().Trim ();

                        if (dt.Rows [i] ["Gubun"].ToString ().Trim () != "")
                        {
                            switch (dt.Rows [i] ["Gubun"].ToString ().Trim ())
                            {
                                case "1":
                                    ssView_Sheet1.Cells [i , 10].Text = "1. 서울관구";
                                    break;
                                case "2":
                                    ssView_Sheet1.Cells [i , 10].Text = "2. 대구관구";
                                    break;
                                case "3":
                                    ssView_Sheet1.Cells [i , 10].Text = "3. 부산관구";
                                    break;
                                case "4":
                                    ssView_Sheet1.Cells [i , 10].Text = "4. 총    원";
                                    break;
                                default:
                                    ssView_Sheet1.Cells [i , 10].Text = dt.Rows [i] ["Gubun"].ToString ().Trim ();
                                    break;
                            }
                        }

                        SQL = " SELECT PANO FROM BAS_PATIENT ";
                        SQL = SQL + ComNum.VBLF + " WHERE JUMIN1 = '" + strJumin1 + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND JUMIN3 = '" + clsAES.AES (strJumin2) + "' ";

                        SqlErr = clsDB.GetDataTable (ref dt1 , SQL, clsDB.DbCon);

                        ssView_Sheet1.Cells [i , 11].Text = "";

                        if (dt1.Rows.Count > 0) ssView_Sheet1.Cells [i , 11].Text = dt1.Rows [0] ["PANO"].ToString ().Trim ();

                        dt1.Dispose ();
                        dt1 = null;

                        ssView_Sheet1.Cells [i , 12].Text = dt.Rows [i] ["rowid"].ToString ().Trim ();
                        ssView_Sheet1.Cells [i , 13].Text = dt.Rows [i] ["PSMH"].ToString ().Trim ();
                    }
                }
                dt.Dispose ();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox (ex.Message);
            }
        }
        private void btnSearch_Click (object sender , EventArgs e)
        {
            Serch ();
        }

        private void btnExcell_Click (object sender , EventArgs e)
        {
            int i = 0;
            int nRow = 0;
            bool x = false;
            string f = "";

            if (ComFunc.MsgBoxQ ("파일로 만드시겠습니까?" , "확인" , MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No) return;

            ssExcel_Sheet1.RowCount = 0;

            for (i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean (ssView_Sheet1.Cells [i , 0].Value) == true)
                {
                    nRow = nRow + 1;
                    if (ssExcel_Sheet1.RowCount < nRow) ssExcel_Sheet1.RowCount = nRow;

                    ssExcel_Sheet1.Cells [nRow - 1 , 0].Text = ssView_Sheet1.Cells [i , 1].Text;
                    ssExcel_Sheet1.Cells [nRow - 1 , 1].Text = ssView_Sheet1.Cells [i , 8].Text;
                    ssExcel_Sheet1.Cells [nRow - 1 , 2].Text = ssView_Sheet1.Cells [i , 11].Text;
                }

            }

            x = ssExcel.SaveExcel ("C:\\모원수녀원명단.xlsx" , FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);
            {
                if (x == true) ComFunc.MsgBox ("엑셀파일이 생성이 되었습니다." , "확인");
                else ComFunc.MsgBox ("엑셀파일 생성에 오류가 발생 하였습니다." , "확인");
            }
        }

        private void txtJumin0_Enter (object sender , EventArgs e)
        {
            if (txtJumin0.Text == "") return;
            if (VB.IsDate (VB.Format (txtJumin0.Text , "0000-00-00")))
            {
                txtJumin1.Focus ();
                return;
            }
        }

        private void txtJumin0_KeyDown (object sender , KeyEventArgs e)
        {
            if (lblMsg.Text != "") lblMsg.Text = "";
            if (ssView_Sheet1.RowCount > 0) ssView_Sheet1.RowCount = 0;
            if (e.KeyCode == Keys.Enter) SendKeys.Send ("{Tab}");
        }

        private void txtJumin1_KeyDown (object sender , KeyEventArgs e)
        {
            if (lblMsg.Text != "") lblMsg.Text = "";
            if (e.KeyCode == Keys.Enter) SendKeys.Send ("{Tab}");
        }

        private void txtMessage_KeyDown (object sender , KeyEventArgs e)
        {
            if (lblMsg.Text != "") lblMsg.Text = "";
            if (e.KeyCode == Keys.Enter) SendKeys.Send ("{Tab}");
        }

        private void txtName_KeyDown (object sender , KeyEventArgs e)
        {
            if (lblMsg.Text != "") lblMsg.Text = "";
            if (e.KeyCode == Keys.Enter) SendKeys.Send ("{Tab}");
        }

        private void txttTopSabun_KeyDown (object sender , KeyEventArgs e)
        {
            if (lblMsg.Text != "") lblMsg.Text = "";
            if (e.KeyCode == Keys.Enter) SendKeys.Send ("{Tab}");
        }

        private string BuseName2Code (string ArgName)
        {
            int i = 0;
            string SQL = "";
            string strVal = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (ComFunc.LenH (ArgName) < 2)
            {
                strVal = ArgName;
                return strVal;
            }
            if (VB.IsNumeric (ArgName))
            {
                strVal = ArgName;
                return strVal;
            }
            try
            {
                SQL = "SELECT BuCode FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + "WHERE SName LIKE '" + ArgName + "%' ";
                SQL = SQL + ComNum.VBLF + "  AND DelDate IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND Jas='*' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);


                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    return strVal;
                }

                if (dt.Rows.Count > 0)
                {
                    ArgName = dt.Rows [0] ["BuCode"].ToString ().Trim ();
                    strVal = ArgName;
                }
                else
                {
                    ArgName = "";
                    strVal = ArgName;
                }

                dt.Dispose ();
                dt = null;

                strVal = ArgName;
                return strVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                return strVal;
            }

        }

        private string READ_BuseName (string ArgCode)
        {
            string strVal = "";
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "SELECT Sname FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + "WHERE BuCode = '" + VB.Format (ArgCode , "000000") + "' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }

                if (dt.Rows.Count == 1)
                {
                    strVal = dt.Rows [0] ["Sname"].ToString ().Trim ();
                }
                else
                {
                    return strVal;
                }
                dt.Dispose ();
                dt = null;

                return strVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox (ex.Message);
                return strVal;
            }
        }

        private void txtSosok0_KeyDown (object sender , KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send ("{Tab}");
        }

        private void txtSosok0_Enter (object sender , EventArgs e)
        {
            txtSosok0.ImeMode = ImeMode.Hangul;

            txtSosok0.Text = txtSosok0.Text.Trim ();

            if (txtSosok0.Text == "") lblSosok0.Text = "";

            txtSosok0.Text = BuseName2Code (txtSosok0.Text.Trim ());
            lblSosok0.Text = READ_BuseName (txtSosok0.Text);

            if (lblSosok0.Text == "")
            {
                ComFunc.MsgBox ("사용부서 코드가 오류입니다" , "오류");
            }
        }

        //(string.Compare(dtpBdate.Text, clsPublic.GstrSysDate) > 0
        //(string.Compare(dtpEnd.Text,clsPublic.GstrSysDate) > 0)

        private void cboGubun_Enter (object sender , EventArgs e)
        {
            if ((string.Compare(dtpEnter.Text, dtpEnd.Text) > 0) || (string.Compare(dtpEnd.Text, clsPublic.GstrSysDate) > 0))
            {
                lblMsg.Text = "종료일자 확인요망";
            }
        }
       
        private void dtpEnter_Enter (object sender , EventArgs e)
        {
            if (string.Compare(dtpEnter.Text, clsPublic.GstrSysDate) > 0) 
            {
                lblMsg.Text = "오늘보다 큰 날짜입니다.";
            }
        }

        private void dtpOut_Enter (object sender , EventArgs e)
        {
            if (dtpOut.Value < dtpEnter.Value || dtpOut.Value > Convert.ToDateTime (clsPublic.GstrSysDate))
            {
                lblMsg.Text = "퇴사일자 확인요망!!";
            }
        }

        private void ssView_ButtonClicked (object sender , FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != 0) return;

            if (Convert.ToBoolean (ssView_Sheet1.Cells [e.Row , e.Column].Value) == true)
            {
                ssView_Sheet1.Cells [e.Row , 1 , e.Row , 9].ForeColor = System.Drawing.Color.FromArgb (255 , 0 , 0);
                btnDelete.Enabled = true;
            }
            else
            {
                ssView_Sheet1.Cells [e.Row , 1 , e.Row , 9].ForeColor = System.Drawing.Color.FromArgb (0 , 0 , 0);
                btnDelete.Enabled = false;
            }

        }

        private void ssView_CellDoubleClick (object sender , FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            if (e.Row < 0 || e.Column < 0) return;

            FstrROWID = "";
            cboGubun.SelectedIndex = 0;

            txtJumin0.Text = VB.Mid (ssView_Sheet1.Cells [e.Row , 1].Text , 1 , 6);
            txtJumin1.Text = VB.Mid (ssView_Sheet1.Cells [e.Row , 1].Text , 7 , 7);

            txttTopSabun.Text = ssView_Sheet1.Cells [e.Row , 2].Text;

            for (i = 0; i < cboGubun.Items.Count; i++)
            {
                cboGubun.SelectedIndex = i;
                if (ssView_Sheet1.Cells [e.Row , 3].Text == VB.Pstr (cboGubun.Text.Trim () , "." , 1))
                {
                    cboGubun.SelectedIndex = i;

                    break;
                }
            }

            if (VB.IsDate (ssView_Sheet1.Cells [e.Row , 4].Text.Trim ()) == true) dtpEnter.Text = ssView_Sheet1.Cells [e.Row , 4].Text.Trim ();
           // else dtpEnter.Text = Convert.ToDateTime ("9997-12-01");

            if (VB.IsDate (ssView_Sheet1.Cells [e.Row , 5].Text.Trim ()) == true) dtpOut.Text = ssView_Sheet1.Cells[e.Row, 5].Text.Trim();
          //  else dtpOut.Text = Convert.ToDateTime ("9997-12-01");

            if (VB.IsDate (ssView_Sheet1.Cells [e.Row , 6].Text.Trim ()) == true) dtpEnd.Text = ssView_Sheet1.Cells[e.Row, 6].Text.Trim();
           // else dtpEnd.Text = Convert.ToDateTime ("9997-12-01");

            txtMessage.Text = ssView_Sheet1.Cells [e.Row , 7].Text;
            txtName.Text = ssView_Sheet1.Cells [e.Row , 8].Text;
            txtSosok0.Text = ssView_Sheet1.Cells [e.Row , 9].Text;
            Sosok_read ();

            if (ssView_Sheet1.Cells [e.Row , 10].Text == "") cboGbn.SelectedIndex = 0;
            else cboGbn.Text = ssView_Sheet1.Cells [e.Row , 10].Text;

            FstrROWID = ssView_Sheet1.Cells [e.Row , 12].Text;
            chkTopSungmo.Checked = Convert.ToBoolean (VB.IIf (ssView_Sheet1.Cells [e.Row , 13].Text == "1" , true , false));
            btnSave.Enabled = true;
            btnDelete.Enabled = true;
            txtJumin0.Focus ();

        }

        private bool INSERT_BAS_GAMF_HIS ()
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ComQuery.IsJobAuth(this , "C", clsDB.DbCon) == false) return rtnVal; //rtnVal;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = " INSERT INTO BAS_GAMF_HISTORY (GAMJUMIN,GAMSABUN,GAMCODE,GAMENTER,GAMOUT,GAMEND,GAMMESSAGE,GAMNAME,GAMSOSOK,GAMGUBUN,EntDate,EntSabun,GamJumin3) ";
                SQL = SQL + ComNum.VBLF + " VALUES ( '" + txtJumin0.Text + VB.Left (txtJumin1.Text , 1) + "******', ";
                SQL = SQL + ComNum.VBLF + "'" + txttTopSabun.Text + "', ";
                SQL = SQL + ComNum.VBLF + "'" + VB.Left (cboGubun.Text , 2) + "', ";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpEnter.Text.Trim() + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpOut.Text.Trim() + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpEnd.Text.Trim() + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "'" + txtMessage.Text + "', ";
                SQL = SQL + ComNum.VBLF + "'" + txtName.Text + "',";
                SQL = SQL + ComNum.VBLF + "'" + txtSosok0.Text + "','0',SYSDATE," + clsType.User.IdNumber + ",'" + clsAES.AES (txtJumin0.Text + txtJumin1.Text) + "') ";

                SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                clsDB.setCommitTran (clsDB.DbCon);
                //ComFunc.MsgBox ("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                btnDelete.Enabled = false;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private bool BAS_GAMF_INSERT ()
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            if (ComQuery.IsJobAuth(this , "C", clsDB.DbCon) == false) return rtnVal; //rtnVal;
            
            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                SQL = " INSERT INTO BAS_GAMF (GAMJUMIN,GAMSABUN,GAMCODE,GAMENTER,GAMOUT,GAMEND,GAMMESSAGE,GAMNAME,GAMSOSOK,GAMGUBUN,EntDate,EntSabun,GamJumin3) ";
                SQL = SQL + ComNum.VBLF + " VALUES ( '" + txtJumin0.Text + VB.Left (txtJumin1.Text , 1) + "******', ";
                SQL = SQL + ComNum.VBLF + "'" + txttTopSabun.Text + "', ";
                SQL = SQL + ComNum.VBLF + "'" + VB.Left (cboGubun.Text , 2) + "', ";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpEnter.Text.Trim() + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpOut.Text.Trim() + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpEnd.Text.Trim() + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "'" + txtMessage.Text + "', ";
                SQL = SQL + ComNum.VBLF + "'" + txtName.Text + "',";
                SQL = SQL + ComNum.VBLF + "'" + txtSosok0.Text + "','0',SYSDATE," + clsType.User.IdNumber + ",'" + clsAES.AES (txtJumin0.Text + txtJumin1.Text) + "') ";

                SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                clsDB.setCommitTran (clsDB.DbCon);
                ComFunc.MsgBox ("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                btnDelete.Enabled = false;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private bool BAS_GAMF_Update ()
        {
            bool rtnVal = false;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            if (ComQuery.IsJobAuth(this , "C", clsDB.DbCon) == false) return rtnVal; //rtnVal;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = " UPDATE BAS_GAMF SET Gamsabun    = '" + txttTopSabun.Text + "' ,";
                SQL = SQL + ComNum.VBLF + "               EntDate =SYSDATE, ";
                SQL = SQL + ComNum.VBLF + "               EntSabun =" + clsType.User.IdNumber + ", ";
                SQL = SQL + ComNum.VBLF + "               GamJumin   ='" + (txtJumin0.Text).Trim () + VB.Left (txtJumin1.Text , 1) + "******',";
                SQL = SQL + ComNum.VBLF + "               Gamcode     = '" + VB.Left (cboGubun.Text , 2) + "',";
                SQL = SQL + ComNum.VBLF + "               GamName     = '" + txtName.Text + "', ";
                SQL = SQL + ComNum.VBLF + "   GAMENTER = TO_DATE('" + dtpEnter.Text.Trim() + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "   GAMOUT=    TO_DATE('" + dtpOut.Text.Trim() + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "   GAMEND=    TO_DATE('" + dtpEnd.Text.Trim() + "','YYYY-MM-DD'), ";
                SQL = SQL + ComNum.VBLF + "               Gammessage  = '" + txtMessage.Text + "', ";
                SQL = SQL + ComNum.VBLF + "   GAMJUMIN3='" + clsAES.AES (txtJumin0.Text + txtJumin1.Text) + "',";
                SQL = SQL + ComNum.VBLF + "               Gamsosok    = '" + txtSosok0.Text + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE         ROWID = '" + FstrROWID + "'";

                SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                clsDB.setCommitTran (clsDB.DbCon);
                ComFunc.MsgBox ("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                btnDelete.Enabled = false;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private bool Sosok_Select ()
        {
            bool rtnVal = false;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            if (ComQuery.IsJobAuth(this , "C", clsDB.DbCon) == false) return rtnVal; //rtnVal;

            try
            {
                SQL = "     SELECT Name FROM BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + "  WHERE Bucode = '" + txtSosok0.Text + "' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    lblSosok0.Text = dt.Rows [0] ["Name"].ToString ().Trim ();
                }
                else
                {
                    lblSosok0.Text = "";
                }

                dt.Dispose ();
                dt = null;

                ComFunc.MsgBox ("저장 하였습니다.");
                btnDelete.Enabled = false;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private int BAS_GAMF_Save_Check ()
        {
            int intVal = 0;

            //BAS_GAMF_Save_Check = 0

            if (VB.Len (txtJumin0.Text) != 6)
            {
                txtJumin0.Focus ();
                return intVal;
            }

            if (VB.Len (txtJumin1.Text) != 7)
            {
                txtJumin1.Focus ();
                return intVal;
            }
            if (txtMessage.Text == "")
            {
                txtMessage.Focus ();
                return intVal;
            }
            if (txtName.Text == "")
            {
                txtName.Focus ();
                return intVal;
            }

            if (VB.Left (cboGubun.Text , 2) != "11" && (cboGbn.Text).Trim () != "")
            {
                ComFunc.MsgBox ("관구설정은 재단성직자 11감액만 해당됨!!");
                return intVal;
            }

            intVal = 1;
            return intVal;
        }

        private int Check_JuminNo ()
        {
            int intVal = 0;
            int Hap = 0;
            int Remainder = 0;
            int i = 0;
            int [] j = new int [6];

            j [1] = 8;
            j [2] = 9;
            j [3] = 2;
            j [4] = 3;
            j [5] = 4;
            j [6] = 5;

            Hap = 0;

            for (i = 0; i < 6; i++)
            {
                Hap = Hap + Convert.ToInt32 (VB.Val (ComFunc.MidH (txtJumin0.Text , i , 1)) * (i + 1));
                Hap = Hap + Convert.ToInt32 (VB.Val (ComFunc.MidH (txtJumin1.Text , i , 1)) * j [i]);
            }

            intVal = 0;
            Remainder = Hap % 11;

            switch (Remainder)
            {
                case 0:
                    if (ComFunc.MidH (txtJumin1.Text , 7 , 1) == "1")
                    {
                        intVal = 1;
                        return intVal;
                    }
                    break;
                case 1:
                    if (ComFunc.MidH (txtJumin1.Text , 7 , 1) == "0")
                    {
                        intVal = 1;
                        return intVal;
                    }
                    break;
                default:
                    if (ComFunc.MidH (txtJumin1.Text , 7 , 1) == Convert.ToString (11 - Remainder))
                    {
                        intVal = 1;
                        return intVal;
                    }
                    break;
            }

            return intVal;
        }

        private void Gubun_Fill ()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            if (ComQuery.IsJobAuth(this , "R", clsDB.DbCon) == false) return;

            cboGbn.Items.Clear ();
            cboGbn1.Items.Clear ();

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT CODE, NAME FROM BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'BAS_감액코드명' ";
                SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";

                if (GstrPassProgramID.Trim () == "OUMSAD" || (GstrPassProgramID.Trim ()) == "OUMSADER") SQL = SQL + ComNum.VBLF + "   AND CODE IN ('11','12','13','14','71') ";
                else SQL = SQL + ComNum.VBLF + "   AND (CODE >= 11 AND CODE <= 44 OR CODE = 52 OR CODE = 71 ) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY CODE ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight (-1 , ComNum.SPDROWHT);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboGubun.Items.Add (dt.Rows [i] ["CODE"].ToString ().Trim () + "." + VB.Left (dt.Rows [i] ["NAME"].ToString ().Trim () , 30));
                    cboGubun1.Items.Add (dt.Rows [i] ["CODE"].ToString ().Trim () + "." + VB.Left (dt.Rows [i] ["NAME"].ToString ().Trim () , 30));
                }

                dt.Dispose ();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Init ()
        {
            txtJumin0.Text = "";
            txtJumin1.Text = "";
            txttTopSabun.Text = "";
            txttBotomSabun.Text = "";
            txttBotomname.Text = "";
            lblMessage.Text = "";
            txtName.Text = "";
            txtSosok0.Text = "";
            lblMsg.Text = "";
            lblSosok0.Text = "";
            cboGubun.SelectedIndex = -1;
            cboGbn.SelectedIndex = -1;
            btnDelete.Enabled = false;
            chkTopSungmo.Checked = false;
            FstrROWID = "";
        }

        private void Sosok_read ()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "   SELECT Name FROM BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + "  WHERE Bucode = '" + txtSosok0.Text + "' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
             //       ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0) lblSosok0.Text = dt.Rows [0] ["Name"].ToString ().Trim ();
                else lblSosok0.Text = "";

                dt.Dispose ();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void READ_BAS_GAMF (string ArgJumin)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = " SELECT ROWID From BAS_GAMF ";
                SQL = SQL + ComNum.VBLF + " Where GamJumin3 = '" + clsAES.AES (ArgJumin) + "' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrROWID = dt.Rows [0] ["ROWID"].ToString ().Trim ();
                }

                dt.Dispose ();
                dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void AllHubulTagetingJoin () //일괄후불대상자등록()
        {
            string strPano = "";
            string strSname = "";
            string strMessage = "";
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                strPano = "";
                strMessage = txtMessage.Text.Trim ();

                SQL = " SELECT Pano,SName From BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " Where Jumin1 ='" + (txtJumin0.Text).Trim () + "' ";
                SQL = SQL + ComNum.VBLF + "   And Jumin3 ='" + clsAES.AES (txtJumin1.Text).Trim () + "' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    strPano = dt.Rows [0] ["PANO"].ToString ().Trim ();
                    strSname = dt.Rows [0] ["SName"].ToString ().Trim ();
                }

                dt.Dispose ();
                dt = null;

                if (strPano != "")
                {
                    SQL = "SELECT PANO, SNAME, DELDATE";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_AUTO_MST";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO = '" + strPano + "'";
                    SQL = SQL + ComNum.VBLF + "  AND GUBUN = '1' ";
                    SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE = '') ";

                    SqlErr = clsDB.GetDataTable (ref dt1 , SQL, clsDB.DbCon);

                    if (dt1.Rows.Count == 0) INSERT_BAS_AUTO_MST (strPano , strSname , strMessage);
                }
                dt1.Dispose ();
                dt1 = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private void INSERT_BAS_AUTO_MST (string ArgPano , string ArgSName , string ArgMessage)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "INSERT INTO ADMIN.BAS_AUTO_MST(";
                SQL = SQL + ComNum.VBLF + "PANO, SNAME, GUBUN, SDATE, ENTDATE, ENTDATE2, ENTSABUN,REMARK) VALUES(";
                SQL = SQL + ComNum.VBLF + "'" + ArgPano + "',";
                SQL = SQL + ComNum.VBLF + "'" + ArgSName + "',";
                SQL = SQL + ComNum.VBLF + "'1',";
                SQL = SQL + ComNum.VBLF + "TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + "SYSDATE,SYSDATE,";
                SQL = SQL + ComNum.VBLF + "'" + (clsType.User.IdNumber).ToString ().Trim () + "','" + ArgMessage + "')";

                SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                clsDB.setCommitTran (clsDB.DbCon);
                ComFunc.MsgBox ("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void GamYegJaeSimSungJigjaSave ()    //Gam_재단성직자관구저장()
        {
            string SQL = "";
            DataTable dt = null;
            string strGbn = "";
            string strPsmh = "";
            string strRowId = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                strGbn = VB.Left (cboGbn.Text , 1);
                strPsmh = (chkTopSungmo.Checked == true ? "1" : "");

                SQL = " SELECT ROWID FROM " + ComNum.VBLF + "BAS_GAMF_SR ";
                SQL = SQL + ComNum.VBLF + " WHERE GAMJUMIN3 ='" + clsAES.AES ((txtJumin0.Text).Trim () + (txtJumin1.Text)).Trim () + "' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                strRowId = "";

                if (dt.Rows.Count > 0)
                {
                    strRowId = dt.Rows [0] ["ROWID"].ToString ().Trim ();
                }

                dt.Dispose ();
                dt = null;

                if (strRowId == "")
                {
                    SQL = " INSERT INTO " + ComNum.DB_PMPA + "BAS_GAMF_SR ( GAMJUMIN,GAMCODE,ENTDATE,ENTPART,GUBUN,PSMH,GAMJUMIN3 ) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "  '" + txtJumin0.Text + VB.Left (txtJumin1.Text , 1) + "******','" + VB.Left (cboGubun.Text , 2) + "',";
                    SQL = SQL + ComNum.VBLF + "SYSDATE," + clsType.User.IdNumber + ",'" + strGbn + "','" + strPsmh + "','" + clsAES.AES (txtJumin0.Text + txtJumin1.Text) + "' ) ";
                    SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);
                }
                else
                {
                    SQL = " UPDATE " + ComNum.DB_PMPA + "BAS_GAMF_SR  SET ";
                    SQL = SQL + "  GAMJUMIN ='" + txtJumin0.Text + VB.Left (txtJumin1.Text , 1) + "******', ";
                    SQL = SQL + "  GAMJUMIN3='" + clsAES.AES (txtJumin0.Text + txtJumin1.Text) + "', ";
                    SQL = SQL + "  GAMCODE ='" + VB.Left (cboGubun.Text , 2) + "',";
                    SQL = SQL + "  ENTDATE =SYSDATE, ";
                    SQL = SQL + "  ENTPART =" + clsType.User.IdNumber + ", ";
                    SQL = SQL + "  PSMH ='" + strPsmh + "', ";
                    SQL = SQL + "  Gubun ='" + strGbn + "' ";
                    SQL = SQL + " WHERE ROWID ='" + strRowId + "' ";
                    SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);
                }
                clsDB.setCommitTran (clsDB.DbCon);
                //                ComFunc.MsgBox ("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void dtpEnter_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            DateTime date = new DateTime(1900, 1, 1, 00, 00, 00);

            if (dtp.Value == date)
            {
                return;
            }

            dtp.Format = DateTimePickerFormat.Short;
        }

        private void dtpOut_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            DateTime date = new DateTime(1900, 1, 1, 00, 00, 00);

            if (dtp.Value == date)
            {
                return;
            }

            dtp.Format = DateTimePickerFormat.Short;
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            DateTime date = new DateTime(1900, 1, 1, 00, 00, 00);

            if (dtp.Value == date)
            {
                return;
            }

            dtp.Format = DateTimePickerFormat.Short;
        }
    }
}
