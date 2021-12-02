using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using ComDbB;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaillEnter
    /// Description     : 
    /// Author          : 전상원
    /// Create Date     : 2018-04-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\iusent\iusent.vbp(IUSENT15.FRM(FrmIllEnter)) >> frmPmpaillEnter.cs 폼이름 재정의" />
    public partial class frmPmpaillEnter : Form
    {
        int i = 0;
        int j = 0;
        //int k = 0;
        //int nIpwonHisCnt = 0; //입퇴원 Read 건수
        int nIllHisCnt = 0; //상병 History 건수

        string FstrPano = "";
        //string strBi = "";
        //string strDept = "";
        string strSname = "";
        string strData = "";
        //string strIpwon = "";
        //string strSS1Flag = "";
        string strIllCode = "";

        int[] nRankTAB = new int[31];
        string[] strIpwonHis = new string[11]; //입퇴원 History 배열
        string[] strIllHis = new string[11]; //청구상병 등록 History
        string[] strRowIdTAB = new string[31];
        string[] strCodeTAB = new string[31];
        string[] strNameTAB = new string[31];

        double FnIPDNO = 0;
        double FnTRSNO = 0;
        string FstrInDate = "";
        string FstrOutDate = "";
        string FstrGBIPD = "";
        string FstrBi = "";
        string FstrDept = "";
        string FstrRowidRemark = "";

        public frmPmpaillEnter()
        {
            InitializeComponent();
        }

        public frmPmpaillEnter(string ArgPano)
        {
            InitializeComponent();
            FstrPano = ArgPano;
        }
      
        private void frmPmpaillEnter_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            panILLS.Visible = false;

            SCREEN_CLEAR();

            this.Show();

            if (clsPublic.GstrHelpCode != "")
            {
                txtPano.Text = clsPublic.GstrHelpCode;
                btnSearch.Enabled = true;
                btnCancel.Enabled = true;
            }

            if (FstrPano != "")
            {
                txtPano.Text = FstrPano;
                btnSearch.Enabled = true;
                btnCancel.Enabled = true;

                Display_Patient();

                if (ssView.ActiveSheet.RowCount == 1)
                {
                    READ_MIR_ILLS();
                }
            }

            panAutotext.Visible = false;

            //READ_Autotext();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            int nRank = 0;
            int j = 0;
            string strIllCode = "";
            string strIllName = "";
            string strGBILL = "";
            string strROWID = "";

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (chkAll2.Checked == true)
            {
                if (ComFunc.MsgBoxQ("전체상병 선택 후 저장시 무조건 자료 저장되니 이미 작업을 했다면" + ComNum.VBLF + ComNum.VBLF + "전체상병체크를 해제하시고 저장하십시오!!", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //참고사항 등록
                if (FstrRowidRemark == "" || chkAll2.Checked == true)
                {
                    #region Insert Mir_iLLS
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " INSERT INTO MIR_ILLS";
                    SQL = SQL + ComNum.VBLF + "        (Pano,Bi,IpdOpd,InDate,Rank,IllCode,IllName, DEPTCODE, IPDNO, TRSNO, GBIPD, REMARK,GBILL )";
                    SQL = SQL + ComNum.VBLF + " VALUES ";
                    SQL = SQL + ComNum.VBLF + "        ('" + txtPano.Text + "','" + FstrBi + "','I', ";
                    SQL = SQL + ComNum.VBLF + "        TO_DATE('" + FstrInDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + "        '0', '' ,'" + txtRemark.Text + "', '" + FstrDept + "', ";
                    SQL = SQL + ComNum.VBLF + "        " + FnIPDNO + "," + FnTRSNO + ", " + " '" + FstrGBIPD + "' , ";
                    SQL = SQL + ComNum.VBLF + "        '" + txtRemark2.Text + "','' ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("청구상병 수정 후 자료 등록 오류");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        SCREEN_CLEAR();
                        txtPano.Focus();
                        return;
                    } 
                    #endregion
                }
                else
                {
                    #region Update Mir_iLLs
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " UPDATE  MIR_ILLS  SET illname = '" + txtRemark.Text + "', REMARK= '" + txtRemark2.Text + "'  ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + FstrRowidRemark + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox("청구상병 수정 후 자료 등록 오류");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        SCREEN_CLEAR();
                        txtPano.Focus();
                        return;
                    } 
                    #endregion
                }

                for (j = 0; j <= 5; j++)
                {
                    strCodeTAB[j] = "";
                }

                for (i = 1; i < ssView1_Sheet1.RowCount; i++)
                {
                    //삭제 Combo Click 여부 Check
                    nRank = i * 10;
                    strIllCode = ssView1_Sheet1.Cells[i - 1, 2].Text.Trim();
                    strGBILL = ssView1_Sheet1.Cells[i - 1, 3].Text.Trim();
                    strIllName = ssView1_Sheet1.Cells[i - 1, 4].Text.Trim();
                    strROWID = ssView1_Sheet1.Cells[i - 1, 5].Text.Trim();

                    if (ssView1_Sheet1.Cells[i - 1, 0].Text == "True")
                    {
                        if (strROWID != "")
                        {
                            #region Mir_Ill_Delete
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " DELETE MIR_ILLS                           ";
                            SQL = SQL + ComNum.VBLF + "  WHERE RowId = '" + strROWID + "'   ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("청구상병 수정 전 자료 삭제 오류");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                SCREEN_CLEAR();
                                txtPano.Focus();
                                return;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        if (strROWID == "" && strIllName.Trim() != "")
                        {
                            #region Mir_Ill_Insert
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + " INSERT INTO MIR_ILLS                                                                  ";
                            SQL = SQL + ComNum.VBLF + "        (Pano,Bi,IpdOpd,InDate,Rank,IllCode,IllName, DEPTCODE, IPDNO, TRSNO, GBIPD,GBILL)    ";
                            SQL = SQL + ComNum.VBLF + " VALUES                                                                                ";
                            SQL = SQL + ComNum.VBLF + "        ('" + txtPano.Text + "','" + FstrBi + "','I',                                   ";
                            SQL = SQL + ComNum.VBLF + "        TO_DATE('" + FstrInDate + "','YYYY-MM-DD'),                                ";
                            SQL = SQL + ComNum.VBLF + "        '" + nRank + "','" + strIllCode + "','" + strIllName.Trim() + "', '" + FstrDept + "', ";
                            SQL = SQL + ComNum.VBLF + "        " + FnIPDNO + "," + FnTRSNO + ", " + " '" + FstrGBIPD + "' , '" + strGBILL + "') ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("청구상병 수정 후 자료 등록 오류");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                SCREEN_CLEAR();
                                txtPano.Focus();
                                return;
                            }
                            #endregion
                        }
                        else
                        {
                            if (strROWID != "" && strIllCode.Trim() == "" && strIllName.Trim() == "")
                            {
                                #region Mir_Ill_Delete
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + " DELETE MIR_ILLS                           ";
                                SQL = SQL + ComNum.VBLF + "  WHERE RowId = '" + strROWID + "'   ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox("청구상병 수정 전 자료 삭제 오류");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    SCREEN_CLEAR();
                                    txtPano.Focus();
                                    return;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Mir_Ill_Update
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + " UPDATE MIR_ILLS SET                       ";
                                SQL = SQL + ComNum.VBLF + "        Rank = '" + nRank + "',            ";
                                SQL = SQL + ComNum.VBLF + "        IllCode = '" + strIllCode + "',    ";
                                SQL = SQL + ComNum.VBLF + "        IllName = '" + strIllName + "' ,    ";
                                SQL = SQL + ComNum.VBLF + "        GBILL = '" + strGBILL + "'     ";
                                SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "'   ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox("청구상병 수정 전 자료 UPDATE 오류");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    SCREEN_CLEAR();
                                    txtPano.Focus();
                                    return;
                                }
                                #endregion
                            }
                        }
                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon); 
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
                return;
            }

            //입원,퇴원마스타에 Update
            #region Mir_Ill_Ipwon_Update 
            //입원마스타에 Update하기 위해 TAB에 Setting
            #region Mir_Ill_TAB_Setting 
            DataTable dt = null;

            for (i = 0; i <= 5; i++)
            {
                nRankTAB[i] = 0;
                strRowIdTAB[i] = "";
                strCodeTAB[i] = "";
                strNameTAB[i] = "";
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT IllCode,IllName FROM MIR_ILLS                              ";
                SQL = SQL + ComNum.VBLF + "  WHERE Pano = '" + txtPano.Text + "'                              ";


                SQL = SQL + ComNum.VBLF + "    AND DEPTCODE = '" + FstrDept + "'                               ";
                SQL = SQL + ComNum.VBLF + "    AND IpdOpd = 'I'                                               ";
                SQL = SQL + ComNum.VBLF + "    AND InDate = TO_DATE('" + FstrInDate + "','YYYY-MM-DD')    ";
                SQL = SQL + ComNum.VBLF + "    AND ( IPDNO = " + FnIPDNO + " OR IPDNO IS NULL)";
                SQL = SQL + ComNum.VBLF + "    AND ( TRSNO = " + FnTRSNO + " OR TRSNO IS NULL)";
                if (FstrGBIPD == "9")
                {
                    SQL = SQL + ComNum.VBLF + "  AND GBIPD = '9'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND (GBIPD IS NULL OR GBIPD NOT IN ('9'))";
                }

                SQL = SQL + ComNum.VBLF + "  ORDER BY Rank,IllCode                                            ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strCodeTAB[i] = dt.Rows[i]["IllCode"].ToString().Trim();
                    strNameTAB[i] = dt.Rows[i]["IllName"].ToString().Trim();
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
            #endregion
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " UPDATE IPD_TRANS SET                    ";
                SQL = SQL + ComNum.VBLF + "        IllCode1 = '" + strCodeTAB[0] + "',    ";
                SQL = SQL + ComNum.VBLF + "        IllCode2 = '" + strCodeTAB[1] + "',    ";
                SQL = SQL + ComNum.VBLF + "        IllCode3 = '" + strCodeTAB[2] + "',    ";
                SQL = SQL + ComNum.VBLF + "        IllCode4 = '" + strCodeTAB[3] + "',    ";
                SQL = SQL + ComNum.VBLF + "        IllCode5 = '" + strCodeTAB[4] + "',    ";
                SQL = SQL + ComNum.VBLF + "        IllCode6 = '" + strCodeTAB[5] + "'     ";
                SQL = SQL + ComNum.VBLF + "  WHERE Pano = '" + txtPano.Text + "'          ";
                SQL = SQL + ComNum.VBLF + "    AND InDate = TO_DATE('" + FstrInDate + "','YYYY-MM-DD')    ";
                SQL = SQL + ComNum.VBLF + "    AND ( IPDNO = " + FnIPDNO + " OR IPDNO IS NULL)";
                SQL = SQL + ComNum.VBLF + "    AND ( TRSNO = " + FnTRSNO + " OR TRSNO IS NULL)";
                if (FstrGBIPD == "9")
                {
                    SQL = SQL + ComNum.VBLF + "  AND GBIPD = '9'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND (GBIPD IS NULL OR GBIPD NOT IN ('9'))";
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("청구상병 IPD_MASTER UPDATE 오류");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    SCREEN_CLEAR();
                    txtPano.Focus();
                    return;
                }

                //clsDB.setRollbackTran(clsDB.DbCon); //TEST
                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            #endregion

        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (FnTRSNO == 0)
            {
                ComFunc.MsgBox("작업하실 내용을 선택해 주세요.", "작업내용 선택");
                return;
            }

            GetData();

            SCREEN_CLEAR();
            txtPano.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            txtPano.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            btnSearch.Enabled = false;
            btnCancel.Enabled = false;
            btnCopy.Enabled = false;
            panILLS.Visible = true;
            ssList2_Sheet1.RowCount = 0;
            ssList3_Sheet1.RowCount = 0;
            #region Copy_IllHistory_Display
            ssList2_Sheet1.RowCount = 0;
            for (i = 0; i < nIllHisCnt; i++)
            {
                //List2.Items.Add(strIllHis[i]);
            }
            #endregion
            btnCopyOK.Enabled = false;
            ssList2.Focus();
        }

        private void btnIlls_Click(object sender, EventArgs e)
        {
            //TODO: 폼 불러오기
            //FrmKCD6_ViewIllss.Show
        }

        private void btnCopyOK_Click(object sender, EventArgs e)
        {
            int i = 0;
            int nOldDataNo = 0;

            for (i = 1; i <= 50; i++)
            {
                if (ssView1_Sheet1.Cells[i - 1, 2].Text != "")
                {
                    nOldDataNo = i;
                }
            }

            j = 0;

            for (i = 0; i < ssList3_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssList3_Sheet1.Cells[i, 0].Value) == true)
                {
                    j++;
                    strData = ssList3_Sheet1.Cells[i, 1].Text.Trim();
                    ssView1_Sheet1.Cells[nOldDataNo + j - 1, 1].Text = ((nOldDataNo + j) * 10).ToString("##0");
                    ssView1_Sheet1.Cells[nOldDataNo + j - 1, 2].Text = VB.Left(strData, 6);
                    ssView1_Sheet1.Cells[nOldDataNo + j - 1, 4].Text = VB.Mid(strData, 8, 50);

                    nRankTAB[nOldDataNo + j - 1] = (int)VB.Val(ssView1_Sheet1.Cells[nOldDataNo + j - 1, 1].Text);
                    strCodeTAB[nOldDataNo + j - 1] = ssView1_Sheet1.Cells[nOldDataNo + j - 1, 2].Text;
                    strNameTAB[nOldDataNo + j - 1] = ssView1_Sheet1.Cells[nOldDataNo + j - 1, 4].Text;
                }
            }

            panILLS.Visible = false;
            btnCopy.Enabled = true;
            ssView1.Focus();

        }

        private void btnCopyCancel_Click(object sender, EventArgs e)
        {
            panILLS.Visible = false;
            btnCopy.Enabled = true;
            ssView1.Focus();
        }


        private void btnSelOK_Click(object sender, EventArgs e)
        {
            int i = 0;

            for (i = 0; i < ssView10_0_Sheet1.RowCount; i++)
            {
                if (ssView10_0_Sheet1.Cells[i, 0].Text == "True")
                {
                    ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 2].Text = ssView10_0_Sheet1.Cells[i, 1].Text;
                    ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 4].Text = ssView10_0_Sheet1.Cells[i, 2].Text;
                }
            }
            ssView1_Sheet1.ActiveRowIndex += 1;
            panAutotext.Visible = false;

            //switch (ssTab1.SelectedIndex)
            //{
            //    case 0:
            //        for (i = 1; i <= ssView10_0_Sheet1.RowCount; i++)
            //        {
            //            if (ssView10_0_Sheet1.Cells[i - 1, 0].Text == "True")
            //            {
            //                ssView1_Sheet1.Cells[ssView1_Sheet1.RowCount, 2].Text = ssView10_0_Sheet1.Cells[i - 1, 1].Text;                            
            //            }
            //        }
            //        break;
            //    case 1:
            //        for (i = 1; i <= ssView10_1_Sheet1.RowCount; i++)
            //        {
            //            if (ssView10_1_Sheet1.Cells[i - 1, 0].Text == "True")
            //            {
            //                txtRemark.Text = txtRemark.Text + ssView10_1_Sheet1.Cells[i - 1, 2].Text + " ";
            //            }
            //        }
            //        break;
            //    case 2:
            //        for (i = 1; i <= ssView10_2_Sheet1.RowCount; i++)
            //        {
            //            if (ssView10_2_Sheet1.Cells[i - 1, 0].Text == "True")
            //            {
            //                txtRemark3.Text = txtRemark3.Text + ssView10_2_Sheet1.Cells[i - 1, 2].Text + " ";
            //            }
            //        }
            //        break;
            //}
        }

        private void btnExit2_Click(object sender, EventArgs e)
        {
            panAutotext.Visible = false;
        }

        private void SCREEN_CLEAR()
        {
            txtRemark.Text = "";
            txtRemark2.Text = "";
            txtRemark3.Text = "";
            lblSname.Text = "";

            lblMsg.Text = "";

            FnIPDNO = 0;
            FnTRSNO = 0;
            FstrInDate = "";
            FstrOutDate = "";
            FstrBi = "";
            FstrGBIPD = "";
            FstrDept = "";
            FstrRowidRemark = "";

            ssView_Sheet1.RowCount = 0;
            ssView1_Sheet1.RowCount = 0;
            ssView1_Sheet1.RowCount = 30;
            lblTRS.Text = "";
            btnSearch.Enabled = false;
            btnCancel.Enabled = false;

            //FstrLoad = true;

            chkGbDiv.Checked = false;
            chkGbDiv.ForeColor = Color.FromArgb(0, 0, 0);

            panAutotext.Visible = false;

            //lblTa.BackColor = ColorTranslator.FromWin32(int.Parse("&H8000000F", System.Globalization.NumberStyles.AllowHexSpecifier));
        }

        private void READ_Autotext()
        {
            //int i = 0;
            //int nREAD = 0;

            //string SQL = "";
            //DataTable dt = null;
            //string SqlErr = "";


            //Cursor.Current = Cursors.WaitCursor;

            //try
            //{
            //    //상병코드
            //    SQL = "";
            //    SQL = " SELECT SABUN, Code, Memo  ";
            //    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.VBLF + "ETC_SET_REMARK ";
            //    SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='1' ";
            //    SQL = SQL + ComNum.VBLF + "   AND SABUN =" + clsPublic.GnJobSabun + " ";

            //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            //    if (SqlErr != "")
            //    {
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //        Cursor.Current = Cursors.Default;
            //        return;
            //    }

            //    ssView10_0_Sheet1.RowCount = 0;

            //    nREAD = dt.Rows.Count;

            //    ssView10_0_Sheet1.RowCount = nREAD + 10;

            //    if (dt.Rows.Count > 0)
            //    {
            //        for (i = 0; i < dt.Rows.Count; i++)
            //        {
            //            ssView10_0_Sheet1.Cells[i, 0].Text = "";
            //            ssView10_0_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
            //            ssView10_0_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MEMO"].ToString().Trim();
            //        }
            //    }

            //    dt.Dispose();
            //    dt = null;

            //    //청구 참고사항
            //    SQL = "";
            //    SQL = " SELECT SABUN, Code, Memo  ";
            //    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.VBLF + "ETC_SET_REMARK ";
            //    SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='2' ";
            //    SQL = SQL + ComNum.VBLF + "   AND SABUN =" + clsPublic.GnJobSabun + " ";

            //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            //    if (SqlErr != "")
            //    {
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //        Cursor.Current = Cursors.Default;
            //        return;
            //    }

            //    ssView10_1_Sheet1.RowCount = 0;

            //    nREAD = dt.Rows.Count;

            //    ssView10_1_Sheet1.RowCount = nREAD + 10;

            //    if (dt.Rows.Count > 0)
            //    {
            //        for (i = 0; i < dt.Rows.Count; i++)
            //        {
            //            ssView10_1_Sheet1.Cells[i, 0].Text = "";
            //            ssView10_1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
            //            ssView10_1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MEMO"].ToString().Trim();
            //        }
            //    }

            //    dt.Dispose();
            //    dt = null;

            //    //재원심사 참고사항
            //    SQL = "";
            //    SQL = " SELECT SABUN, Code, Memo  ";
            //    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.VBLF + "ETC_SET_REMARK ";
            //    SQL = SQL + ComNum.VBLF + "  WHERE GUBUN ='3' ";
            //    SQL = SQL + ComNum.VBLF + "   AND SABUN =" + clsPublic.GnJobSabun + " ";

            //    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            //    if (SqlErr != "")
            //    {
            //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
            //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            //        Cursor.Current = Cursors.Default;
            //        return;
            //    }

            //    ssView10_2_Sheet1.RowCount = 0;

            //    nREAD = dt.Rows.Count;

            //    ssView10_2_Sheet1.RowCount = nREAD + 10;

            //    if (dt.Rows.Count > 0)
            //    {
            //        for (i = 0; i < dt.Rows.Count; i++)
            //        {
            //            ssView10_2_Sheet1.Cells[i, 0].Text = "";
            //            ssView10_2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
            //            ssView10_2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MEMO"].ToString().Trim();
            //        }
            //    }

            //    dt.Dispose();
            //    dt = null;
            //}
            //catch (Exception ex)
            //{
            //    if (dt != null)
            //    {
            //        dt.Dispose();
            //        dt = null;
            //    }

            //    ComFunc.MsgBox(ex.Message);
            //    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //}
        }

        private void READ_MIR_ILLS()
        {
            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            int nRow = 0;
            string strString = "";
            string strPano = "";

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            strString = "";
            FstrGBIPD = "";
            FstrRowidRemark = "";

            strString = "입원상태: " + ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 0].Text;
            if (VB.Len(ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 1].Text) != VB.Len(VB.Replace(ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 1].Text, "지병", "")))
            {
                FstrGBIPD = "9";
            }
            strString = strString + "  구분: " + ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 1].Text;

            strPano = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].Text;
            strString = strString + "  등록번호: " + ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].Text;

            FstrBi = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 3].Text;
            strString = strString + "  자격: " + ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 3].Text;

            FstrDept = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 4].Text;

            FstrInDate = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 5].Text;
            strString = strString + "  입원일: " + ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 5].Text;

            FstrOutDate = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 6].Text;
            strString = strString + "  퇴원일: " + ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 6].Text;

            if (FstrOutDate == "")
            {
                FstrOutDate = strSysDate;
            }

            FnIPDNO = VB.Val(ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 9].Text);
            FnTRSNO = VB.Val(ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 10].Text);

            clsOrdFunction.GstrOutDate = FstrOutDate;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT JSIM_REMARK, GbDiv FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "  WHERE IPDNO= '" + FnIPDNO + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //txtRemark3.Text = dt.Rows[ssView_Sheet1.ActiveRowIndex]["JSIM_REMARK"].ToString().Trim();
                txtRemark3.Text = dt.Rows[0]["JSIM_REMARK"].ToString().Trim();

                //FstrLoad = true;

                if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[ssView_Sheet1.ActiveRowIndex]["GBDIV"].ToString().Trim() == "*")
                    if (dt.Rows[0]["GBDIV"].ToString().Trim() == "*")
                    {
                        chkGbDiv.Checked = true;
                        chkGbDiv.ForeColor = Color.FromArgb(255, 0, 0);
                    }
                    else
                    {
                        chkGbDiv.Checked = false;
                        chkGbDiv.ForeColor = Color.FromArgb(255, 255, 255);
                    }
                }

                //FstrLoad = false;

                dt.Dispose();
                dt = null;

                lblTRS.Text = strString;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.RowId,A.IllCode,IllName,RANK,REMARK, GBILL "; // , TO_CHAR(DDATE,'YYYY - MM - DD') DDATE , TO_CHAR(NOUSEDATE,'YYYY - MM - DD') NOUSEDATE    "
                SQL = SQL + ComNum.VBLF + " FROM MIR_ILLS  A ";   // --, BAS_ILLS B                  " & vbLf
                SQL = SQL + ComNum.VBLF + "  WHERE Pano = '" + strPano + "'                              ";
                SQL = SQL + ComNum.VBLF + "    AND IpdOpd = 'I'                                          ";
                SQL = SQL + ComNum.VBLF + "    AND (IPDNO IS NULL OR IPDNO = " + FnIPDNO + ")";
                if (chkAll2.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + "    AND Bi = '" + FstrBi + "'                                 ";
                    SQL = SQL + ComNum.VBLF + "    AND InDate  = TO_DATE('" + FstrInDate + "','YYYY-MM-DD')  ";
                    SQL = SQL + ComNum.VBLF + "    AND TRSNO =" + FnTRSNO + "  "; //2009-09-14 윤조연 추가함
                }
                if (FstrGBIPD == "9")
                {
                    SQL = SQL + ComNum.VBLF + "  AND GBIPD = '9' ";
                }
                SQL = SQL + ComNum.VBLF + "  ORDER BY Rank,IllCode                                       ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    for (i = 1; i <= ssView1_Sheet1.RowCount; i++)
                    {
                        ssView1_Sheet1.Cells[i - 1, 5].Text = "";
                    }
                    return;
                }

                if (dt.Rows.Count == 1)
                {
                    for (i = 1; i <= ssView1_Sheet1.RowCount; i++)
                    {
                        ssView1_Sheet1.Cells[i - 1, 5].Text = "";
                    }
                }

                nRow = 0;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["RANK"].ToString().Trim() == "0")
                    {
                        txtRemark.Text = dt.Rows[i]["Illname"].ToString().Trim();
                        txtRemark2.Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        FstrRowidRemark = dt.Rows[i]["RowId"].ToString().Trim();
                    }
                    else
                    {
                        nRow = nRow + 1;
                        ssView1_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["RANK"].ToString().Trim();
                        ssView1_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["IllCode"].ToString().Trim();

                        if (READ_BAS_ILLS_IPDETC(clsDB.DbCon, dt.Rows[i]["IllCode"].ToString().Trim()) == true)
                        {
                            ssView1_Sheet1.Cells[nRow - 1, 2].BackColor = Color.FromArgb(255, 224, 192);
                        }
                        else
                        {
                            ssView1_Sheet1.Cells[nRow - 1, 2].BackColor = Color.White;
                        }

                        ssView1_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["GBILL"].ToString().Trim();
                        ssView1_Sheet1.Cells[nRow - 1, 4].Text = Read_VIll_CHK(dt.Rows[i]["IllCode"].ToString().Trim()) + dt.Rows[i]["IllName"].ToString().Trim();
                        ssView1_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["RowId"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                //2013-05-27 산재, 자보 아이디 메모사항
                lblTa.BackColor = Color.Gray;

                if (FstrBi == "31" || FstrBi == "52")
                {
                    SQL = "";
                    SQL = " SELECT MEMO,BI ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SANID ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + strPano + "' AND BI = '" + FstrBi + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        lblTa.BackColor = Color.Pink;
                    }

                    dt.Dispose();
                    dt = null;
                }

                ssView1_Sheet1.SetActiveCell(3, 1);

                btnSearch.Enabled = true;
                btnCancel.Enabled = true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void ssList3_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList3_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.Column == 1)
            {
                ssList3_Sheet1.Cells[e.Row, 0].Value = !(Convert.ToBoolean(ssList3_Sheet1.Cells[e.Row, 0].Value));
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Display_Patient(); 
            }
        }

        void Display_Patient()
        {
            int i = 0;
            int nREAD = 0;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            if (txtPano.Text == "" || VB.IsNumeric(txtPano.Text) == false)
            {
                ComFunc.MsgBox("등록번호가 공란이거나 등록번호 형식이 아닙니다!!!" + ComNum.VBLF + ComNum.VBLF + "병록 번호를 확인하시고 입력 바랍니다!!!", "◈◈◈ 등록번호 확인 ◈◈◈");
                txtPano.Text = "";
                txtPano.Focus();
            }

            txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Sname                          ";
                SQL = SQL + ComNum.VBLF + "   FROM BAS_PATIENT                    ";
                SQL = SQL + ComNum.VBLF + "  WHERE Pano = '" + txtPano.Text + "'  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("환자마스타가 존재하지 않습니다.");
                    txtPano.Focus();
                    return;
                }

                strSname = dt.Rows[0]["Sname"].ToString().Trim();
                lblSname.Text = strSname;

                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TRSNO, IPDNO, GBSTS, PANO, BI, DeptCode, GBIPD, ILSU, VCODE,FCODE, OGPDBUN, OGPDBUNdtl,JSim_Sabun,FCODE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(InDate,'yyyy-mm-dd') INDATE,   ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(OUTDATE,'yyyy-mm-dd') OUTDATE,   ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(ACTDATE,'yyyy-mm-dd') ACTDATE   ";
                SQL = SQL + ComNum.VBLF + "   FROM IPD_TRANS                                             ";
                SQL = SQL + ComNum.VBLF + "  WHERE Pano = '" + txtPano.Text + "'                          ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY INDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssView_Sheet1.RowCount = 0;

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("입퇴원 내역이 없습니다.");
                    txtPano.Focus();
                    return;
                }

                nREAD = dt.Rows.Count;
                ssView_Sheet1.RowCount = nREAD;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = READ_BCODE_Name("IPD_입원상태", dt.Rows[i]["GBSTS"].ToString().Trim());
                    if (dt.Rows[i]["GbIPD"].ToString().Trim() == "9")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "지병" + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "P" || dt.Rows[i]["OGPDBUN"].ToString().Trim() == "O")
                    {
                        //2015-05-18 입원명령결핵 추가
                        if (dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                        {
                            ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["VCODE"].ToString().Trim() + "+F008";
                        }
                        //2015-06-30
                        else if (dt.Rows[i]["FCODE"].ToString().Trim() == "F010")
                        {
                            ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["VCODE"].ToString().Trim() + "+F010";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 1].Text = "면제";
                            ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                            if (VB.Val(dt.Rows[i]["InDate"].ToString().Trim()) >= VB.Val("11/04/01") && dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || dt.Rows[i]["VCODE"].ToString().Trim() == "V246" || dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                            {
                                ssView_Sheet1.Cells[i, 1].Text = ssView_Sheet1.Cells[i, 1].Text + " ★결핵★";
                            }
                        }
                    }
                    else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "중증E+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "중증F+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    //2013-02-15
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "E+V";
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "2")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "F+V";
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "중증E+V" + dt.Rows[i]["VCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "2")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "중증F+V" + dt.Rows[i]["VCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    //V268 뇌출혈추가
                    else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V191" || dt.Rows[i]["VCODE"].ToString().Trim() == "V192" || dt.Rows[i]["VCODE"].ToString().Trim() == "V193" || dt.Rows[i]["VCODE"].ToString().Trim() == "V194" || dt.Rows[i]["VCODE"].ToString().Trim() == "V268")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "중증+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (VB.Val(dt.Rows[i]["InDate"].ToString().Trim()) >= VB.Val("11/04/01") && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E" && dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || dt.Rows[i]["VCODE"].ToString().Trim() == "V246" || dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                    {
                        //2015-05-18 입원명령결핵 추가
                        if (dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                        {
                            ssView_Sheet1.Cells[i, 1].Text = "차상위E(" + dt.Rows[i]["VCODE"].ToString().Trim() + "+F008";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 1].Text = "차상위E+★결핵★";
                            ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                        }
                    }
                    else if (VB.Val(dt.Rows[i]["InDate"].ToString().Trim()) >= VB.Val("11/04/01") && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F" && dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || dt.Rows[i]["VCODE"].ToString().Trim() == "V246" || dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                    {
                        //2015-05-18 입원명령결핵 추가
                        if (dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                        {
                            ssView_Sheet1.Cells[i, 1].Text = "차상위F(" + dt.Rows[i]["VCODE"].ToString().Trim() + "+F008";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 1].Text = "차상위F+★결핵★";
                            ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                        }
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E" && dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "중증화상E+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F" && dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "중증화상F+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "중증화상+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "H")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "희귀H";
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "V")
                    {
                        //2015-05-18 입원명령결핵 추가
                        if (dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                        {
                            ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["VCODE"].ToString().Trim() + "+F008";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 1].Text = "희귀V";
                            ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                            if (VB.Val(dt.Rows[i]["InDate"].ToString().Trim()) >= VB.Val("11/04/01") && dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || dt.Rows[i]["VCODE"].ToString().Trim() == "V246" || dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                            {
                                ssView_Sheet1.Cells[i, 1].Text = ssView_Sheet1.Cells[i, 1].Text + " ★결핵★";
                            }
                        }
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "C")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "차상";
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E")
                    {
                        if (dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 1].Text = "차상E+" + dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 1].Text = "차상E" + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F")
                    {
                        if (dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() != "")
                        {
                            ssView_Sheet1.Cells[i, 1].Text = "차상F+" + dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 1].Text = "차상F" + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        ssView_Sheet1.Cells[i, 1].BackColor = Color.FromArgb(255, 0, 0);
                    }
                    //2015-10-08
                    else if (dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = "면제(" + dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() + ")";
                    }

                    if (dt.Rows[i]["FCODE"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[i, 1].Text = ssView_Sheet1.Cells[i, 1].Text +  "특정기호 (" + dt.Rows[i]["FCODE"].ToString().Trim() + ")";
                    }

                    ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BI"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();

                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["INDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["OUTDATE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();

                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ILSU"].ToString().Trim();

                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["TRSNO"].ToString().Trim();

                    ssView_Sheet1.Cells[i, 11].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["JSim_Sabun"].ToString().Trim());

                    //2010-08-12 -그냥 심사완료 5일경우
                    if (dt.Rows[i]["GBSTS"].ToString().Trim() == "5")
                    {
                        ssView_Sheet1.Cells[i, 0].BackColor = Color.FromArgb(255, 255, 0);
                    }
                    else
                    {
                        ssView_Sheet1.Cells[i, 0].BackColor = Color.FromArgb(255, 255, 255);
                    }
                }

                dt.Dispose();
                dt = null;

                btnSearch.Enabled = true;
                btnCancel.Enabled = true;

                ssView.Focus();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private string READ_BCODE_Name(string ArgGubun, string ArgPano)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT NAME ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE Gubun='" + ArgGubun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND TRIM(CODE) ='" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                return rtnVal;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            if (chkAll.Checked == true)
            {
                ssView1_Sheet1.RowCount = 0;
                ssView1_Sheet1.RowCount = 30;
            }

            READ_MIR_ILLS();
            //TODO : 색깔표시 
            //lblTa.BackColor = ColorTranslator.FromWin32(int.Parse("&H8000000F", System.Globalization.NumberStyles.AllowHexSpecifier));
        }

        private string Read_VIll_CHK(string ArgCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_ILLS ";
                SQL = SQL + ComNum.VBLF + "  WHERE ILLCODE ='" + ArgCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GbVCode ='*' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                rtnVal = "";

                if (dt.Rows.Count > 0)
                {
                    rtnVal = "@";
                }

                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private bool READ_BAS_ILLS_IPDETC(PsmhDb pDbCon, string ArgILLCode)
        {
            bool rtnVal = false;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT IPDETC FROM " + ComNum.DB_PMPA + "BAS_ILLS  ";
                SQL = SQL + ComNum.VBLF + "  WHERE IllCode = '" + ArgILLCode + "'              ";
                SQL = SQL + ComNum.VBLF + "    AND ILLCLASS = '1'                              ";
                SQL = SQL + ComNum.VBLF + "    AND IPDETC = 'Y'                                ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }

        private void lblTa_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //2013-05-27 산재,자보 아이디 메모사항
            if (FstrBi == "31" || FstrBi == "52")
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    SQL = "";
                    SQL = " SELECT MEMO,BI ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_SANID ";
                    SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + txtPano.Text + "' AND BI = '" + FstrBi + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Bi"].ToString().Trim() == "31")
                        {
                            ComFunc.MsgBox("※산재메모:" + dt.Rows[0]["Memo"].ToString().Trim());
                        }
                        else if (dt.Rows[0]["Bi"].ToString().Trim() == "52")
                        {
                            ComFunc.MsgBox("※자보메모:" + dt.Rows[0]["Memo"].ToString().Trim());
                        }
                    }

                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
            }
        }

        private void ssList2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            btnCopyOK.Enabled = false;
            ssList3_Sheet1.RowCount = 0;

            strData = strIllHis[ssList2.ActiveSheetIndex];

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT IllCode,IllName FROM MIR_ILLS                                     ";
                SQL = SQL + ComNum.VBLF + "  WHERE Pano = '" + txtPano.Text + "'                                     ";
                SQL = SQL + ComNum.VBLF + "    AND Bi = '" + VB.Mid(strData, 11, 2) + "'                             ";
                SQL = SQL + ComNum.VBLF + "    AND IpdOpd = '" + VB.Mid(strData, 15, 1) + "'                         ";
                SQL = SQL + ComNum.VBLF + "    AND InDate = TO_DATE('" + VB.Mid(strData, 17, 10) + "','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY Rank,IllCode                                                   ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strData = VB.Left(dt.Rows[i]["IllCode"].ToString().Trim(), 6) + " ";
                    strData = strData + dt.Rows[i]["IllName"].ToString().Trim();
                    //List3.AddItem(strData);
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

                if (ssList3_Sheet1.RowCount > 0)
                {
                    btnCopyOK.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ssView1_EditModeOff(object sender, EventArgs e)
        {
            int nRank = 0;
            string strData = "";
            string strILLMSG = "";

            int i = 0;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (ssView1_Sheet1.ActiveColumnIndex != 3 && ssView1_Sheet1.ActiveColumnIndex != 2 && ssView1_Sheet1.ActiveColumnIndex != 5)
            {
                return;
            }

            strData = ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].Text;

            switch (ssView1_Sheet1.ActiveColumnIndex)
            {
                case 1:
                    #region Col_02_Process
                    //Rank 0인경우 처리
                    nRank = (int)VB.Val(ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 1].Text);
                    if (nRank == 0)
                    {
                        ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 1].Text = VB.Format(ssView1_Sheet1.ActiveRowIndex * 10, "##0");
                    }
                    #endregion
                    break;
                case 2:
                    #region Col_03_Process
                    //상병 Routine
                    strData = VB.Trim(ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].Text).ToUpper();
                    ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].Text = strData;

                    if (strData == "")
                    {
                        return;
                    }

                    clsPublic.GstrSILLCode = "";
                    strIllCode = strData;

                    Cursor.Current = Cursors.WaitCursor;

                    try
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " SELECT IllCode IllCodeK,IllNameK,NOUSE,  TO_CHAR(DDATE,'YYYY-MM-DD') DDATE , TO_CHAR(NOUSEDATE,'YYYY-MM-DD') NOUSEDATE,IPDETC    ";
                        SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS                       ";
                        SQL = SQL + ComNum.VBLF + "  WHERE IllCode = '" + strData + "'    ";
                        SQL = SQL + ComNum.VBLF + "    AND ILLCLASS ='1'";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count == 0)
                        {
                            ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 4].Text = "";
                            ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 2].Text = "";

                            #region 기본 상병코드가 없다면
                            if (strData != "")
                            {
                                SQL = "";
                                SQL = " SELECT ILLCODE, ILLNAMEK FROM " + ComNum.DB_PMPA + "BAS_ILLS ";
                                SQL = SQL + ComNum.VBLF + " WHERE (ILLCODE  LIKE '%" + VB.Trim(strData) + "%' ";
                                SQL = SQL + ComNum.VBLF + "     OR UPPER(ILLNAMEK) LIKE '%" + VB.Trim(strData).ToUpper() + "%' ";
                                SQL = SQL + ComNum.VBLF + "     OR UPPER(ILLNAMEE) LIKE '%" + VB.Trim(strData).ToUpper() + "%' ) ";
                                SQL = SQL + ComNum.VBLF + "   AND (NOUSE <>'N' OR NOUSE IS NULL) ";
                                SQL = SQL + ComNum.VBLF + "   AND ILLCLASS ='1' ";
                                SQL = SQL + ComNum.VBLF + "   AND DDATE IS NULL ";
                                //2015-01-08
                                if (string.Compare(FstrOutDate, "2016-01-01") < 0)
                                {
                                    SQL = SQL + ComNum.VBLF + "              AND  ( KCDOLD ='*' OR KCD6  ='*' ) ";
                                }
                                //else if (VB.Val(FstrOutDate) >= VB.Val("2016-01-01"))
                                else if (string.Compare(FstrOutDate, "2016-01-01") >= 0)
                                {
                                    SQL = SQL + ComNum.VBLF + "              AND  ( KCDOLD ='*' OR KCD6  ='*' OR  KCD7 ='*') ";
                                }

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                ssView10_0.ActiveSheet.RowCount = 0;

                                if (dt2.Rows.Count > 0)
                                {
                                    panAutotext.Visible = true;

                                    strILLMSG = " 사용가능 상병 아래 상병참조 바랍니다.";
                                    //strILLMSG = strILLMSG + " 사용가능 상병 아래 상병참조 바랍니다." + ComNum.VBLF + ComNum.VBLF;
                                    //strILLMSG = strILLMSG + "===================================================================" + ComNum.VBLF + ComNum.VBLF;

                                    for (i = 0; i < dt2.Rows.Count; i++)
                                    {
                                        if (ssView10_0.ActiveSheet.RowCount < i + 1)
                                        {
                                            ssView10_0.ActiveSheet.RowCount = i + 1;
                                        }

                                        ssView10_0.ActiveSheet.Cells[i, 1].Text = dt2.Rows[i]["illcode"].ToString().Trim();
                                        ssView10_0.ActiveSheet.Cells[i, 2].Text = dt2.Rows[i]["IllNameK"].ToString().Trim();
                                    }

                                    dt2.Dispose();
                                    dt2 = null;

                                    //strILLMSG = strILLMSG + "===================================================================" + ComNum.VBLF + ComNum.VBLF;

                                    lblIllMsg.Text = strILLMSG;

                                    //if (clsPublic.GstrSILLCode != "")
                                    //{
                                    //    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].Text = clsPublic.GstrSILLCode;
                                    //    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 4].Text = Read_VIll_CHK(clsPublic.GstrSILLCode) + clsPublic.GstrSILLNameK;
                                    //}
                                    //else
                                    //{
                                    //    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 4].Text = "";
                                    //    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].Text = "";
                                    //}

                                    //ssView1.Focus();
                                }
                            } 
                            #endregion
                        }
                        else if (dt1.Rows.Count > 0)
                        {
                            #region 기본 상병코드가 있다면
                            //if (VB.Val(FstrOutDate) >= VB.Val(dt1.Rows[ssView_Sheet1.ActiveRowIndex]["DDATE"].ToString().Trim()) && dt1.Rows[ssView_Sheet1.ActiveRowIndex]["DDATE"].ToString().Trim() != "")
                            if ( string.Compare(FstrOutDate, dt1.Rows[i]["DDATE"].ToString().Trim()) >= 0 && dt1.Rows[i]["DDATE"].ToString().Trim() != "")
                            {
                                strILLMSG = "[삭제상병]" + strIllCode + " 는 삭제상병입니다. " + ComNum.VBLF;
                                //strILLMSG = "[삭제상병]" + ComNum.VBLF + ComNum.VBLF;
                                //strILLMSG = strILLMSG + strIllCode + " 는 삭제상병입니다. " + ComNum.VBLF + ComNum.VBLF;

                                SQL = "";
                                SQL = " SELECT ILLCODE, ILLNAMEK FROM " + ComNum.VBLF + "BAS_ILLS ";
                                SQL = SQL + ComNum.VBLF + " WHERE ILLCODE  LIKE '" + VB.Left(VB.Trim(strIllCode), VB.Len(VB.Trim(strIllCode)) - 1) + "%' ";
                                SQL = SQL + ComNum.VBLF + "   AND LENGTH(ILLCODE) <= 6 ";
                                SQL = SQL + ComNum.VBLF + "   AND (NOUSE <>'N' OR NOUSE IS NULL) ";
                                SQL = SQL + ComNum.VBLF + "   AND ILLCLASS ='1' ";
                                SQL = SQL + ComNum.VBLF + "   AND DDATE IS NULL ";
                                //2015-01-08
                                //if (VB.Val(FstrOutDate) < VB.Val("2016-01-01"))
                                if (string.Compare(FstrOutDate, "2016-01-01") < 0)
                                {
                                    SQL = SQL + ComNum.VBLF + "              AND  ( KCDOLD ='*' OR KCD6  ='*' ) ";
                                }
                                //else if (VB.Val(FstrOutDate) >= VB.Val("2016-01-01"))
                                else if (string.Compare(FstrOutDate, "2016-01-01") >= 0)
                                {
                                    SQL = SQL + ComNum.VBLF + "              AND  ( KCDOLD ='*' OR KCD6  ='*' OR  KCD7 ='*') ";
                                }

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                ssView10_0.ActiveSheet.RowCount = 0;

                                if (dt2.Rows.Count > 0)
                                {
                                    panAutotext.Visible = true;

                                    strILLMSG += " 사용가능 상병 아래 상병참조 바랍니다.";
                                    //strILLMSG = strILLMSG + " 사용가능 상병 아래 상병참조 바랍니다." + ComNum.VBLF + ComNum.VBLF;
                                    //strILLMSG = strILLMSG + "===================================================================" + ComNum.VBLF + ComNum.VBLF;

                                    for (i = 0; i < dt2.Rows.Count; i++)
                                    {
                                        if (ssView10_0.ActiveSheet.RowCount < i + 1)
                                        {
                                            ssView10_0.ActiveSheet.RowCount = i + 1;
                                        }

                                        ssView10_0.ActiveSheet.Cells[i, 1].Text = dt2.Rows[i]["illcode"].ToString().Trim();
                                        ssView10_0.ActiveSheet.Cells[i, 2].Text = dt2.Rows[i]["IllNameK"].ToString().Trim();
                                    }

                                    dt2.Dispose();
                                    dt2 = null;
                                }
                           
                                //strILLMSG = strILLMSG + "===================================================================" + ComNum.VBLF + ComNum.VBLF;

                                lblIllMsg.Text = strILLMSG;

                                //TODO
                                //FrmILLHelp.SSPanelInfor.Caption = strILLMSG
                                //FrmILLHelp.Show 1

                                //if (clsPublic.GstrSILLCode != "")
                                //{
                                //    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].Text = clsPublic.GstrSILLCode;
                                //    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 4].Text = Read_VIll_CHK(clsPublic.GstrSILLCode) + clsPublic.GstrSILLNameK;
                                //}
                                //else
                                //{
                                //    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 4].Text = "";
                                //    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].Text = "";
                                //}

                                //2016-07-14
                                if (VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && dt1.Rows[0]["IPDETC"].ToString().Trim() == "Y")
                                {
                                    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].BackColor = Color.FromArgb(255, 224, 192);
                                }
                                else
                                {
                                    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].BackColor = Color.White;
                                }

                                //ssView1.Focus(); 
                                
                            }
                            //else if (VB.Val(FstrOutDate) >= VB.Val(dt1.Rows[ssView1_Sheet1.ActiveRowIndex]["NOUSEDATE"].ToString().Trim()) && dt1.Rows[ssView1_Sheet1.ActiveRowIndex]["NOUSEDATE"].ToString().Trim() == "N")
                            
                            else if (string.Compare(FstrOutDate, dt1.Rows[i]["NOUSEDATE"].ToString().Trim() ) >= 0
                                && dt1.Rows[i]["NOUSEDATE"].ToString().Trim() == "N")
                            {
                                strILLMSG = "[불완전상병] " + strIllCode + " 는 불완전상병입니다. " + ComNum.VBLF;
                                //strILLMSG = "[불완전상병]" + ComNum.VBLF + ComNum.VBLF;
                                //strILLMSG = strILLMSG + strIllCode + " 는 불완전상병입니다. " + ComNum.VBLF + ComNum.VBLF;

                                SQL = "";
                                SQL = " SELECT ILLCODE, ILLNAMEK FROM " + ComNum.VBLF + "BAS_ILLS ";
                                SQL = SQL + ComNum.VBLF + " WHERE ILLCODE  LIKE '" + VB.Trim(strIllCode) + "%' ";
                                SQL = SQL + ComNum.VBLF + "   AND LENGTH(ILLCODE) <= 6 ";
                                SQL = SQL + ComNum.VBLF + "   AND (NOUSE <>'N' OR NOUSE IS NULL) ";
                                SQL = SQL + ComNum.VBLF + "   AND ILLCLASS ='1' ";
                                SQL = SQL + ComNum.VBLF + "   AND DDATE IS NULL ";
                                //2015-01-08
                                //if (VB.Val(FstrOutDate) < VB.Val("2016-01-01"))
                                if (string.Compare(FstrOutDate, "2016-01-01") < 0) 
                                {
                                    SQL = SQL + ComNum.VBLF + "              AND  ( KCDOLD ='*' OR KCD6  ='*' ) ";
                                }
                                //else if (VB.Val(FstrOutDate) >= VB.Val("2016-01-01"))
                                else if (string.Compare(FstrOutDate, "2016-01-01") >= 0)
                                {
                                    SQL = SQL + ComNum.VBLF + "              AND  ( KCDOLD ='*' OR KCD6  ='*' OR  KCD7 ='*') ";
                                }

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    Cursor.Current = Cursors.Default;
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                ssView10_0.ActiveSheet.RowCount = 0;

                                if (dt2.Rows.Count > 0)
                                {
                                    panAutotext.Visible = true;

                                    strILLMSG = strILLMSG + " 사용가능 상병 아래 상병참조 바랍니다.";
                                    //strILLMSG = strILLMSG + " 사용가능 상병 아래 상병참조 바랍니다." + ComNum.VBLF + ComNum.VBLF;
                                    //strILLMSG = strILLMSG + "===================================================================" + ComNum.VBLF + ComNum.VBLF;

                                    for (i = 0; i < dt2.Rows.Count; i++)
                                    {
                                        if (ssView10_0.ActiveSheet.RowCount < i + 1)
                                        {
                                            ssView10_0.ActiveSheet.RowCount = i + 1;
                                        }

                                        ssView10_0.ActiveSheet.Cells[i, 1].Text = dt2.Rows[i]["illcode"].ToString().Trim();
                                        ssView10_0.ActiveSheet.Cells[i, 2].Text = dt2.Rows[i]["IllNameK"].ToString().Trim();
                                    }

                                    dt2.Dispose();
                                    dt2 = null;
                                }

                                //FrmILLHelp.SSPanelInfor.Caption = strILLMSG
                                //FrmILLHelp.Show 1

                                //if (clsPublic.GstrSILLCode != "")
                                //{
                                //    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].Text = clsPublic.GstrSILLCode;
                                //    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 4].Text = Read_VIll_CHK(clsPublic.GstrSILLCode) + clsPublic.GstrSILLNameK;
                                //}
                                //else
                                //{
                                //    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 4].Text = "";
                                //    ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].Text = "";
                                //}

                                //ssView1.Focus();
                            }

                            ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 4].Text = Read_VIll_CHK(dt1.Rows[0]["IllCodeK"].ToString().Trim()) + dt1.Rows[0]["IllNameK"].ToString().Trim();
                            ssView1_Sheet1.ActiveRowIndex += 1;
                            #endregion
                        }
                        else
                        {
                            if (VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && dt1.Rows[0]["IPDETC"].ToString().Trim() == "Y")
                            {
                                ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 2].BackColor = Color.FromArgb(255, 224, 192);
                            }
                            else
                            {
                                ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 2].BackColor = Color.White;
                            }

                            ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 4].Text = Read_VIll_CHK(dt1.Rows[0]["IllCodeK"].ToString().Trim()) + dt1.Rows[0]["IllNameK"].ToString().Trim();
                            ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, 2].Text = "";
                        }

                        dt1.Dispose();
                        dt1 = null;
                        Cursor.Current = Cursors.Default;
                    }
                    catch (Exception ex)
                    {
                        if (dt1 != null)
                        {
                            dt1.Dispose();
                            dt1 = null;
                        }

                        if (dt2 != null)
                        {
                            dt2.Dispose();
                            dt2 = null;
                        }

                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                    #endregion
                    break;
                case 4:
                    #region Col_05_Process
                    strData = VB.Trim(ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].Text);

                    if (VB.Len(strData) > 50)
                    {
                        strData = VB.Left(ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].Text, 50);
                        ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex, ssView1_Sheet1.ActiveColumnIndex].Text = strData;
                    }

                    nRank = (int)VB.Val(VB.Trim(ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex + 1, 1].Text));

                    if (nRank == 0)
                    {
                        ssView1_Sheet1.Cells[ssView1_Sheet1.ActiveRowIndex + 1, 1].Text = VB.Format((ssView1_Sheet1.ActiveRowIndex + 1) * 10, "##0");
                    }
                    #endregion
                    break;
            }
        }

        private void ssView1_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            strIllCode = VB.Trim(ssView1_Sheet1.Cells[e.Row, 2].Text);

            if (VB.Left(clsPmpaType.TIT.Bi, 1) == "1" && READ_BAS_ILLS_IPDETC(clsDB.DbCon, strIllCode) == true)
            {
                ssView1_Sheet1.Cells[e.Row, 2].BackColor = Color.FromArgb(255, 224, 192);
            }
            else
            {
                ssView1_Sheet1.Cells[e.Row, 2].BackColor = Color.White;
            }
        }

        private void txtRemark_TextChanged(object sender, EventArgs e)
        {
        

            if (sender == txtRemark)
            {
                P_Len.Text = VB.Len(txtRemark.Text) + " / " + "700";
                P_Len.ForeColor = Color.Black; ;
                if (VB.Len(txtRemark.Text) >= 700) { P_Len.ForeColor = Color.Red; ; }

            }
        }
    }
}
