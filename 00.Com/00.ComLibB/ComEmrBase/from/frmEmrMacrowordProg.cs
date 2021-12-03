using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrMacrowordProg : Form
    {
        string gStrId = "";
        string gStrIdIdx = "";
        string gStrFormNo = "";
        string gStrMaxLength = "";
        //string gStrMaxRow = "";
        string gStrLabel = "";
        //string pGrpNo = "";
        string gstrUserGb = "U";
        //이벤트를 전달할 경우
        public delegate void SetMacro(string strCtlName , string strMacrono , string strMacro , string strCtlNameIdx);
        public event SetMacro rSetMacro;

        //폼이 Close될 경우
        public delegate void EventClosed();
        public event EventClosed rEventClosed;

        public frmEmrMacrowordProg()
        {
            InitializeComponent();
        }

        public frmEmrMacrowordProg(string strStrId, string strStrIdIdx, string strStrFormNo, string strStrMaxLength, string strStrLabel)
        {
            InitializeComponent();

            gStrId = strStrId;
            gStrIdIdx = strStrIdIdx;
            gStrFormNo = strStrFormNo;
            gStrMaxLength = strStrMaxLength;
            gStrLabel = strStrLabel;
        }

        public frmEmrMacrowordProg(string strStrId, string strStrIdIdx, string strStrFormNo, string strStrMaxLength, string strStrLabel, string strUserGb)
        {
            InitializeComponent();

            gStrId = strStrId;
            gStrIdIdx = strStrIdIdx;
            gStrFormNo = strStrFormNo;
            gStrMaxLength = strStrMaxLength;
            gStrLabel = strStrLabel;
            gstrUserGb = strUserGb;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            SetAllClear();
        }

        private void SetAllClear()
        {
            txtMacroNo.Text = "";
		    txtTitle.Text = "";
		    txtContent.Text = "";
		    txtOrderSeq.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) //권한 확인
            //{
            //    return;
            //}
            if (DeleteData() == true)
            {
                SetAllClear();
                GetMacroList();
            }
        }

        private bool DeleteData()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            OracleDataReader reader = null;

            if (VB.Val(txtMacroNo.Text.Trim()) == 0)
            {
                return false;
            }

            try 
            {
                #region 고경자 팀장 저장한 항목 삭제 못하게
                SQL = "SELECT USEID";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRUSERSENTENCE";
                SQL += ComNum.VBLF + "WHERE MACRONO = " + txtMacroNo.Text.Trim();

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                if (reader.HasRows && reader.Read())
                {
                    if (reader.GetValue(0).ToString().Trim().Equals("8822") && clsType.User.IdNumber.Equals("8822") == false)
                    {
                        ComFunc.MsgBoxEx(this, "해당 항목은 삭제 할 수 없습니다.");
                        reader.Dispose();
                        return true;
                    }
                   
                }
                reader.Dispose();
                #endregion

                SQL = "";
		        SQL = SQL + "delete from " + ComNum.DB_EMR + "AEMRUSERSENTENCE WHERE MACRONO = " + txtMacroNo.Text.Trim();
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show (ex.Message);
                return false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            rEventClosed();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) //권한 확인
            //{
            //    return;
            //}

            if (optGAll.Checked && ComFunc.MsgBoxQEx(this, "전체 항목으로 저장시 해당 기록지를 사용하는\r\n모든 직원들이 해당 상용구를 수정/삭제 할 수 있습니다.\r\n정말 전체로 저장하시겠습니까?") == DialogResult.No)
                return;

            if (SaveData() == true)
            {
                SetAllClear();
                GetMacroList();
            }
        }

        private bool SaveData()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string macroNo = "";

            string strFormNo = "";
            string strConId = "";
            string strConIdIdx = "";
            string strTitle = "";
            string strContent = "";
            string strFrDate = "";
            string strFrTime = "";
            string strOrderSeq = "";
            string strUseId = "";
            string strMaxVal = "";
            //string strType = "";
            string strGRPGB = "";
            string strUSEGB = "";
            string strCurDateTime = "";
            OracleDataReader reader = null;

            macroNo = txtMacroNo.Text;

            if (txtTitle.Text == "" || txtContent.Text == "")
            {
                MessageBox.Show(new Form() { TopMost = true }, "제목 또는 내용이 빈칸입니다");
                return false;
            }

            if (txtMacroNo.Text != "")
            {
                if ((MessageBox.Show(new Form() { TopMost = true }, "수정 하시겠습니까?", "확인" ,MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No))
                {
                    return false;
                }
            }

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                if (optGAll.Checked == true)
                {
                    strGRPGB = "A";
			        strUSEGB = "ALL";
                }
                else if (optGdept.Checked == true)
                {
                    strGRPGB = "D";
                    strUSEGB = clsType.User.DeptCode.Length > 0 ? clsType.User.DeptCode : clsType.User.BuseCode;
                }
                else
                {
                    strGRPGB = "U";
			        strUSEGB = clsType.User.IdNumber;
                }

                strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
		
		        strFormNo = txtFormNo.Text;
		        strConId = ComFunc.QuotConv(txtConId.Text);
                strConIdIdx = ComFunc.QuotConv(txtConIdIdx.Text);
                strTitle = ComFunc.QuotConv(txtTitle.Text);
                strContent = ComFunc.QuotConv(txtContent.Text);
		        strFrDate = VB.Left(strCurDateTime, 8);
		        strFrTime = VB.Right(strCurDateTime, 6);
            
                if (txtOrderSeq.Text == "")
                {
                    strOrderSeq = "999";
                }
                else
                {
                    strOrderSeq = txtOrderSeq.Text;
                }
                
                strMaxVal = lblMaxval.Text;
                strUseId =  clsType.User.IdNumber;
               
                if (macroNo == "")
                {
                    macroNo = Convert.ToString(ComQuery.GetSequencesNoEx(clsDB.DbCon, "" + ComNum.DB_EMR + "SEQ_AEMRUSERSENTENCE_MACRONO"));

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "  INSERT INTO " + ComNum.DB_EMR + "AEMRUSERSENTENCE (";
			        SQL = SQL + ComNum.VBLF + "        MACRONO";
			        SQL = SQL + ComNum.VBLF + "        , FORMNO";
			        SQL = SQL + ComNum.VBLF + "        , CONTROLID";
			        SQL = SQL + ComNum.VBLF + "        , CONTROLIDIDX";
			        SQL = SQL + ComNum.VBLF + "        , GRPTYPE";
			        SQL = SQL + ComNum.VBLF + "        , GRPGB";
			        SQL = SQL + ComNum.VBLF + "        , USEGB";
			        SQL = SQL + ComNum.VBLF + "        , TITLE";
			        SQL = SQL + ComNum.VBLF + "        , CONTENT";
			        SQL = SQL + ComNum.VBLF + "        , USEID";
			        SQL = SQL + ComNum.VBLF + "        , WRITEDATE";
			        SQL = SQL + ComNum.VBLF + "        , WRITETIME";
			        SQL = SQL + ComNum.VBLF + "        , ORDERSEQ";
			        SQL = SQL + ComNum.VBLF + "        , MAXVAL";
			        SQL = SQL + ComNum.VBLF + "        , USETYPE)";
			        SQL = SQL + ComNum.VBLF + "    VALUES (";
			        SQL = SQL + ComNum.VBLF + "  '" + macroNo + "',";
			        SQL = SQL + ComNum.VBLF + "  '" + strFormNo + "',";
			        SQL = SQL + ComNum.VBLF + "  '" + strConId + "',";
			        SQL = SQL + ComNum.VBLF + "  '" + strConIdIdx + "',";
			        SQL = SQL + ComNum.VBLF + "  'M',";
			        SQL = SQL + ComNum.VBLF + "  '" + strGRPGB + "',";
			        SQL = SQL + ComNum.VBLF + "  '" + strUSEGB + "',";
			        SQL = SQL + ComNum.VBLF + "  '" + VB.Replace(strTitle, "'", "`") + "',";
			        SQL = SQL + ComNum.VBLF + "  '" + VB.Replace(strContent, "'", "`") + "',";
			        SQL = SQL + ComNum.VBLF + "  '" + strUseId + "',";
			        SQL = SQL + ComNum.VBLF + "  '" + strFrDate + "',";
			        SQL = SQL + ComNum.VBLF + "  '" + strFrTime + "',";
			        SQL = SQL + ComNum.VBLF + "  '" + strOrderSeq + "',";
			        SQL = SQL + ComNum.VBLF + "  '" + strMaxVal + "',";
			        SQL = SQL + ComNum.VBLF + "  '0')";
                }
                else
                {
                    #region 고경자 팀장 저장한 항목 삭제 못하게
                    SQL = "SELECT USEID";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRUSERSENTENCE";
                    SQL += ComNum.VBLF + "WHERE MACRONO = " + macroNo;

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return false;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        if (reader.GetValue(0).ToString().Trim().Equals("8822") && clsType.User.IdNumber.Equals("8822") == false)
                        {
                            ComFunc.MsgBoxEx(this, "해당 항목은 삭제 할 수 없습니다.");
                            reader.Dispose();
                            return true;
                        }

                    }
                    reader.Dispose();
                    #endregion

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "  UPDATE " + ComNum.DB_EMR + "AEMRUSERSENTENCE SET ";
                    SQL = SQL + ComNum.VBLF + "        TITLE = '" + ComFunc.QuotConv(txtTitle.Text) + "'";
                    SQL = SQL + ComNum.VBLF + "        , CONTENT = '" + ComFunc.QuotConv(txtContent.Text) + "'";
			        SQL = SQL + ComNum.VBLF + "        , ORDERSEQ = " + txtOrderSeq.Text;
			        SQL = SQL + ComNum.VBLF + "        , WRITEDATE = '" + strFrDate + "'";
			        SQL = SQL + ComNum.VBLF + "        , WRITETIME = '" + strFrTime + "'";
			        SQL = SQL + ComNum.VBLF + "   WHERE MACRONO = " + macroNo;
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;

                return true;
            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetMacroList();
        }

        private void frmMacrowordProg_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //}
            //ComFunc.SetFormInit(clsDB.DbCon,this, "Y", "Y", "Y");

            txtConId.Text = gStrId;
		    txtConIdIdx.Text = gStrIdIdx;
		    lblMaxval.Text = gStrMaxLength;
		    txtFormNo.Text = gStrFormNo;
            txtFormName.Text = gStrLabel;

            if(gstrUserGb == "A")
            {
                optMacroSeachAll.Checked = true;
                optGAll.Checked = true;
            }
            else if (gstrUserGb == "D")
            {
                optMacroSeachDept.Checked = true;
                optGdept.Checked = true;
            }
            else
            {
                optMacroSeachUseid.Checked = true;
                optGuseid.Checked = true;
            }
        }

        private void GetMacroList()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) //권한 확인
            //{
            //    return;
            //}

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssMacroWord_Sheet1.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
		    SQL = SQL + ComNum.VBLF + " SELECT A.GRPGB ,C.FORMNAME, A.TITLE, A.CONTENT, A.USEID, A.GRPTYPE";
		    SQL = SQL + ComNum.VBLF + "              , A.ORDERSEQ, A.MAXVAL, A.GRPGB, A.USEGB, A.MACRONO,A.FORMNO, A.CONTROLID, A.CONTROLIDIDX";
		    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRUSERSENTENCE A";
            SQL = SQL + ComNum.VBLF + " INNER JOIN " + ComNum.DB_EMR + "AEMRFORM C ";
            SQL = SQL + ComNum.VBLF + "       ON A.FORMNO = C.FORMNO ";
            SQL = SQL + ComNum.VBLF + "       AND C.UPDATENO =(SELECT MAX(UPDATENO) FROM " + ComNum.DB_EMR + "AEMRFORM WHERE FORMNO = C.FORMNO) ";

            if (optMacroSeachAll.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "  WHERE A.GRPGB = 'A'";
                SQL = SQL + ComNum.VBLF + "      AND A.USEGB = 'ALL'";
            }
            else if (optMacroSeachDept.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + "  WHERE A.GRPGB = 'D'";
                SQL = SQL + ComNum.VBLF + "      AND A.USEGB = '" + (clsType.User.DrCode.Length > 0 ?  clsType.User.DeptCode : clsType.User.BuseCode)  + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "  WHERE A.GRPGB = 'U'";
                SQL = SQL + ComNum.VBLF + "      AND A.USEGB = '" + clsType.User.IdNumber + "'";
            }
                                                           
            if (txtFormNo.Text.Trim() != "")
            {
                SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = '" + txtFormNo.Text.Trim() + "'";
            }
            if (txtConId.Text.Trim() != "")
            {
                SQL = SQL + ComNum.VBLF + "  AND A.CONTROLID = '" + txtConId.Text.Trim() + "'";
                //if (VB.Trim(txtConIdIdx.Text) == "")
                //{
                //    SQL = SQL + ComNum.VBLF + "  AND (A.CONTROLIDIDX IS NULL OR A.CONTROLIDIDX = '')";
                //}
                //else
                //{
                //    SQL = SQL + ComNum.VBLF + "  AND A.CONTROLIDIDX = '" + txtConIdIdx.Text.Trim() + "'";
                //}
            }

            if (VB.Trim(SearchTxt.Text) != "")
            {
                SQL = SQL + ComNum.VBLF + "  AND A.TITLE LIKE '%" + SearchTxt.Text.Trim() + "%'";
            }

            SQL = SQL + ComNum.VBLF + " ORDER BY A.ORDERSEQ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssMacroWord_Sheet1.RowCount = dt.Rows.Count;
            ssMacroWord_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (i=0; i < dt.Rows.Count; i++)
            {
                ssMacroWord_Sheet1.Cells[i, 0].Text = VB.Trim(dt.Rows[i]["GRPGB"].ToString());
                ssMacroWord_Sheet1.Cells[i, 1].Text = VB.Trim(dt.Rows[i]["TITLE"].ToString());
                ssMacroWord_Sheet1.Cells[i, 2].Text = VB.Trim(dt.Rows[i]["CONTENT"].ToString());
                ssMacroWord_Sheet1.Cells[i, 3].Text = VB.Trim(dt.Rows[i]["GRPTYPE"].ToString());
                ssMacroWord_Sheet1.Cells[i, 4].Text = VB.Trim(dt.Rows[i]["USEID"].ToString());
                ssMacroWord_Sheet1.Cells[i, 5].Text = VB.Trim(dt.Rows[i]["ORDERSEQ"].ToString());
                ssMacroWord_Sheet1.Cells[i, 6].Text = VB.Trim(dt.Rows[i]["MAXVAL"].ToString());
                ssMacroWord_Sheet1.Cells[i, 7].Text = VB.Trim(dt.Rows[i]["USEGB"].ToString());
                ssMacroWord_Sheet1.Cells[i, 8].Text = VB.Trim(dt.Rows[i]["MACRONO"].ToString());
                ssMacroWord_Sheet1.Cells[i, 9].Text = VB.Trim(dt.Rows[i]["FORMNO"].ToString());
                ssMacroWord_Sheet1.Cells[i, 10].Text = VB.Trim(dt.Rows[i]["FORMNAME"].ToString());
                ssMacroWord_Sheet1.Cells[i, 11].Text = VB.Trim(dt.Rows[i]["CONTROLID"].ToString());
                ssMacroWord_Sheet1.Cells[i, 12].Text = VB.Trim(dt.Rows[i]["CONTROLIDIDX"].ToString());
            }

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;
        }

        private void optMacroSeachAll_CheckedChanged(object sender, EventArgs e)
        {
            if (optMacroSeachAll.Checked == true)
            {
                GetMacroList();
            }
        }

        private void optMacroSeachDept_CheckedChanged(object sender, EventArgs e)
        {
            if (optMacroSeachDept.Checked == true)
            {
                GetMacroList();
            }
        }

        private void optMacroSeachUseid_CheckedChanged(object sender, EventArgs e)
        {
            if (optMacroSeachUseid.Checked == true)
            {
                GetMacroList();
            }
        }

        private void SearchTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                GetMacroList();
            }
        }

        private void ssMacroWord_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strCtlName = null;
            string strCtlNameIdx = null;
            string strMacrono = null;
            string strMacro = null;

            if (ssMacroWord_Sheet1.RowCount == 0)
            {
                return;
            }
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssMacroWord, e.Column);
                return;
            }

            strCtlName = ssMacroWord_Sheet1.Cells[e.Row, 11].Text;
            strCtlNameIdx = ssMacroWord_Sheet1.Cells[e.Row, 12].Text;
            strMacro = ssMacroWord_Sheet1.Cells[e.Row, 2].Text;
            strMacrono = ssMacroWord_Sheet1.Cells[e.Row, 8].Text;

            strMacro = VB.Replace(VB.Replace(strMacro, "<pre>", ""), "</pre>", "");
            
            rSetMacro(strCtlName, strMacrono, strMacro, strCtlNameIdx);
        }

        private void ssMacroWord_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssMacroWord_Sheet1.RowCount == 0)
            {
                return;
            }
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssMacroWord, e.Column);
                return;
            }

            txtConName.Text = ssMacroWord_Sheet1.Cells[e.Row, 10].Text ;//서식지명
            txtTitle.Text = ssMacroWord_Sheet1.Cells[e.Row, 1].Text;//매크로제목
            txtContent.Text = ssMacroWord_Sheet1.Cells[e.Row, 2].Text;//매크로내용
            txtOrderSeq.Text = ssMacroWord_Sheet1.Cells[e.Row, 5].Text;//정렬순서
            lblMaxval.Text = ssMacroWord_Sheet1.Cells[e.Row, 6].Text;//최대길이

            txtMacroNo.Text = ssMacroWord_Sheet1.Cells[e.Row, 8].Text;//매크로넘버
            txtFormNo.Text = ssMacroWord_Sheet1.Cells[e.Row, 9].Text;//서시지번호
            txtConId.Text = ssMacroWord_Sheet1.Cells[e.Row, 11].Text;
            txtConIdIdx.Text = ssMacroWord_Sheet1.Cells[e.Row, 12].Text;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT CONTENT FROM " + ComNum.DB_EMR + "AEMRUSERSENTENCE WHERE MACRONO=" + VB.Val(VB.Trim(txtMacroNo.Text));
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBoxEx(this, "해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }
            txtContent.Text = VB.Replace(VB.Replace(VB.Trim(dt.Rows[0]["CONTENT"].ToString()), "<pre>", ""), "</pre>", "");

            dt.Dispose();
            dt = null;
        }

        private void btnMoveMacro_Click(object sender, EventArgs e)
        {
            frmEmrMacrowordProgTrs frmMacrowordProgTrsX = new frmEmrMacrowordProgTrs(gStrId, gStrIdIdx, gStrFormNo, gStrMaxLength, gStrLabel);
            frmMacrowordProgTrsX.StartPosition = FormStartPosition.CenterParent;
            frmMacrowordProgTrsX.ShowDialog(); 

            SetAllClear();
            GetMacroList();
        }

        

    }
}
