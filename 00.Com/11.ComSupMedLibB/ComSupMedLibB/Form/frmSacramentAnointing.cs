using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupMedLibB
{
    public partial class frmSacramentAnointing : Form
    {
        string GstrSEQNO = "";

        public frmSacramentAnointing()
        {
            InitializeComponent();
        }

        private void frmSacramentAnointing_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            GstrSEQNO = "";
            Screen_Clear();
            txtSName.Focus();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
            Screen_Clear();
        }

        private void GetData()
        {
            int i = 0;
            string strNameS = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;
            strNameS = txtAName.Text.Trim();
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT *";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.WONMOK_PATIENT_APPLICATION ";

                if (txtAName.Text.Trim()!= "")
                {
                    SQL = SQL + ComNum.VBLF + "    WHERE PTNAME = '" + strNameS + "'";
                }

                SQL = SQL + ComNum.VBLF + " ORDER BY SEQNO";

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
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PTNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SERENAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["BIRTHDATE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SEX"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["APPLYRE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["PROTECTOR"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["RELATION"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["BONDANG"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ADDRESS"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["HOMENO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["PHONENO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["SUNGHOPE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["COMMUNION"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 14].Text = dt.Rows[i]["PATCOMMUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 15].Text = dt.Rows[i]["SINNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 16].Text = dt.Rows[i]["SINSERE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 17].Text = dt.Rows[i]["SINRELATION"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 18].Text = dt.Rows[i]["SINPHONE"].ToString().Trim();
                        //ssView_Sheet1.Cells[i, 19].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 19].Text = dt.Rows[i]["JIPDAY"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 20].Text = dt.Rows[i]["JIPSPACE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 21].Text = dt.Rows[i]["JIPNAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 22].Text = dt.Rows[i]["JIPSERE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 23].Text = dt.Rows[i]["GYOJUKNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 24].Text = dt.Rows[i]["TERMIREASON"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 25].Text = dt.Rows[i]["REGDATE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            Screen_Clear();
        }

        private void SaveData()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strActiveNo = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (txtSeqno.Text == "")
                {
                    SQL = "SELECT KOSMOS_ADM.SEQ_WONMOK_PATIENT_APPLICATION.NEXTVAL FROM DUAL";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strActiveNo = dt.Rows[0]["NEXTVAL"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "INSERT INTO KOSMOS_ADM.WONMOK_PATIENT_APPLICATION";
                    SQL = SQL + ComNum.VBLF + "(";
                    SQL = SQL + ComNum.VBLF + " SEQNO, PTNAME, SERENAME, BIRTHDATE, SEX, APPLYRE, PROTECTOR, RELATION, BONDANG, ADDRESS, HOMENO, PHONENO, SUNGHOPE, COMMUNION, PATCOMMUN,";
                    SQL = SQL + ComNum.VBLF + "SINNAME, SINSERE, SINRELATION, SINPHONE, REMARK, JIPDAY, JIPSPACE, JIPNAME, JIPSERE, GYOJUKNO, TERMIREASON, REGDATE";
                    SQL = SQL + ComNum.VBLF + ")";
                    SQL = SQL + ComNum.VBLF + "VALUES";
                    SQL = SQL + ComNum.VBLF + "(";
                    SQL = SQL + ComNum.VBLF + "     '" + strActiveNo + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtSName.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtSereName.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtAge.Text + "',";

                    if (rdoMan.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     'M',";
                    }
                    else if (rdoWoman.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     'W',";
                    }

                    SQL = SQL + ComNum.VBLF + "     '" + txtApplyReason.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtBohoName.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtRelation.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtBondang.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtAddress.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtHomeNo.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtPhoneNo.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtSungHope.Text + "',";

                    if (rdoPositive.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     'Y',";
                    }
                    else if (rdoNegative.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     'N',";
                    }

                    SQL = SQL + ComNum.VBLF + "     '" + txtPatYungSung.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtSinName.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtSinSere.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtSinRelation.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtSinPhone.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtJipDay.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtJipSpace.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtJipName.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtJipSere.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtGyoJukNo.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     '" + txtTermiReason.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE,'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    SQL = "";
                    SQL = "UPDATE KOSMOS_ADM.WONMOK_PATIENT_APPLICATION SET";
                    SQL = SQL + ComNum.VBLF + "     SEQNO = '" + txtSeqno.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "     PTNAME = '" + txtSName.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     SERENAME = '" + txtSereName.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     BIRTHDATE = '" + txtAge.Text + "',";

                    if (rdoMan.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     SEX = 'M',";
                    }
                    else if (rdoWoman.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     SEX = 'W',";
                    }

                    SQL = SQL + ComNum.VBLF + "     APPLYRE = '" + txtApplyReason.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     PROTECTOR = '" + txtBohoName.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     RELATION = '" + txtRelation.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     BONDANG = '" + txtBondang.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     ADDRESS = '" + txtAddress.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     HOMENO = '" + txtHomeNo.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     PHONENO = '" + txtPhoneNo.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     SUNGHOPE = '" + txtSungHope.Text + "',";

                    if (rdoPositive.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     COMMUNION = 'Y',";
                    }
                    else if (rdoNegative.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "     COMMUNION = 'N',";
                    }

                    SQL = SQL + ComNum.VBLF + "     PATCOMMUN = '" + txtPatYungSung.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     SINNAME = '" + txtSinName.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     SINSERE = '" + txtSinSere.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     SINRELATION = '" + txtSinRelation.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     SINPHONE = '" + txtSinPhone.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     REMARK = '',";
                    SQL = SQL + ComNum.VBLF + "     JIPDAY = '" + txtJipDay.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     JIPSPACE = '" + txtJipSpace.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     JIPNAME = '" + txtJipName.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     JIPSERE = '" + txtJipSere.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     GYOJUKNO = '" + txtGyoJukNo.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     TERMIREASON = '" + txtTermiReason.Text + "',";
                    SQL = SQL + ComNum.VBLF + "     REGDATE = TO_CHAR(SYSDATE,'YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " WHERE SEQNO = '" + txtSeqno.Text + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
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
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strTitle = "";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            //strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(ssPrint1, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
            CS = null;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GstrSEQNO = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 0].Text;

            if (GstrSEQNO == "")
            {
                ComFunc.MsgBox("저장된 내용이 없습니다.");
            }
            else
            {
                if (ComFunc.MsgBoxQ("작성된 내용을 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }

                if (GstrSEQNO != "")
                {
                    dataDelete(GstrSEQNO);
                }
            }

            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Screen_Clear()
        {
            txtSeqno.Text = "";
            txtSName.Text = "";
            txtSereName.Text = "";
            txtAge.Text = "";
            rdoMan.Checked = false;
            rdoWoman.Checked = false;
            txtApplyReason.Text = "";
            txtBohoName.Text = "";
            txtRelation.Text = "";
            txtBondang.Text = "";
            txtAddress.Text = "";
            txtHomeNo.Text = "";
            txtPhoneNo.Text = "";
            txtSungHope.Text = "";
            txtPatYungSung.Text = "";
            txtSinName.Text = "";
            txtSinSere.Text = "";
            txtSinRelation.Text = "";
            txtSinPhone.Text = "";
            txtJipDay.Text = "";
            txtJipSpace.Text = "";
            txtJipName.Text = "";
            txtJipSere.Text = "";
            txtGyoJukNo.Text = "";
            txtTermiReason.Text = "";
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GstrSEQNO = ssView_Sheet1.Cells[e.Row, 0].Text;

            dataView(GstrSEQNO);

            ssPrint1_Sheet1.Cells[4, 2].Text = txtSName.Text;
            ssPrint1_Sheet1.Cells[4, 5].Text = txtSereName.Text;
            ssPrint1_Sheet1.Cells[5, 2].Text = txtAge.Text;

            if (rdoMan.Checked == true)
            {
                ssPrint1_Sheet1.Cells[5, 5].Text = "남";
            }
            else if (rdoWoman.Checked == true)
            {
                ssPrint1_Sheet1.Cells[5, 5].Text = "여";
            }

            ssPrint1_Sheet1.Cells[6, 2].Text = txtApplyReason.Text;
            ssPrint1_Sheet1.Cells[7, 2].Text = txtBohoName.Text;
            ssPrint1_Sheet1.Cells[7, 5].Text = txtRelation.Text;
            ssPrint1_Sheet1.Cells[8, 2].Text = txtBondang.Text;
            ssPrint1_Sheet1.Cells[9, 2].Text = txtAddress.Text;
            ssPrint1_Sheet1.Cells[10, 3].Text = txtHomeNo.Text;
            ssPrint1_Sheet1.Cells[10, 5].Text = txtPhoneNo.Text;
            ssPrint1_Sheet1.Cells[11, 2].Text = txtSungHope.Text;


            if (rdoPositive.Checked == true)
            {
                ssPrint1_Sheet1.Cells[11, 5].Text = "가능";
            }
            else if (rdoNegative.Checked == true)
            {
                ssPrint1_Sheet1.Cells[11, 5].Text = "불가";
            }

            ssPrint1_Sheet1.Cells[12, 2].Text = txtPatYungSung.Text;
            ssPrint1_Sheet1.Cells[13, 3].Text = txtSinName.Text;
            ssPrint1_Sheet1.Cells[13, 5].Text = txtSinSere.Text;
            ssPrint1_Sheet1.Cells[14, 3].Text = txtSinRelation.Text;
            ssPrint1_Sheet1.Cells[14, 5].Text = txtSinPhone.Text;
            ssPrint1_Sheet1.Cells[24, 2].Text = txtJipDay.Text;
            ssPrint1_Sheet1.Cells[24, 5].Text = txtJipSpace.Text;
            ssPrint1_Sheet1.Cells[25, 2].Text = txtJipName.Text;
            ssPrint1_Sheet1.Cells[25, 5].Text = txtJipSere.Text;
            ssPrint1_Sheet1.Cells[26, 2].Text = txtGyoJukNo.Text;
            ssPrint1_Sheet1.Cells[26, 5].Text = txtTermiReason.Text;
        }

        private void dataView(string strSeqno)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT * FROM KOSMOS_ADM.WONMOK_PATIENT_APPLICATION";
                SQL = SQL + ComNum.VBLF + "  WHERE SEQNO = '" + strSeqno + "'";

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
                    txtSeqno.Text = dt.Rows[0]["SEQNO"].ToString().Trim();
                    txtSName.Text = dt.Rows[0]["PTNAME"].ToString().Trim();
                    txtSereName.Text = dt.Rows[0]["SERENAME"].ToString().Trim();
                    txtAge.Text = dt.Rows[0]["BIRTHDATE"].ToString().Trim();

                    if (dt.Rows[0]["SEX"].ToString().Trim() == "M")
                    {
                        rdoMan.Checked = true;
                    }
                    else if (dt.Rows[0]["SEX"].ToString().Trim() == "W")
                    {
                        rdoWoman.Checked = true;
                    }

                    txtApplyReason.Text = dt.Rows[0]["APPLYRE"].ToString().Trim();
                    txtBohoName.Text = dt.Rows[0]["PROTECTOR"].ToString().Trim();
                    txtRelation.Text = dt.Rows[0]["RELATION"].ToString().Trim();
                    txtBondang.Text = dt.Rows[0]["BONDANG"].ToString().Trim();
                    txtAddress.Text = dt.Rows[0]["ADDRESS"].ToString().Trim();
                    txtHomeNo.Text = dt.Rows[0]["HOMENO"].ToString().Trim();
                    txtPhoneNo.Text = dt.Rows[0]["PHONENO"].ToString().Trim();
                    txtSungHope.Text = dt.Rows[0]["SUNGHOPE"].ToString().Trim();

                    if (dt.Rows[0]["COMMUNION"].ToString().Trim() == "Y")
                    {
                        rdoPositive.Checked = true;
                    }
                    else if (dt.Rows[0]["COMMUNION"].ToString().Trim() == "N")
                    {
                        rdoPositive.Checked = true;
                    }

                    txtPatYungSung.Text = dt.Rows[0]["PATCOMMUN"].ToString().Trim();
                    txtSinName.Text = dt.Rows[0]["SINNAME"].ToString().Trim();
                    txtSinSere.Text = dt.Rows[0]["SINSERE"].ToString().Trim();
                    txtSinRelation.Text = dt.Rows[0]["SINRELATION"].ToString().Trim();
                    txtSinPhone.Text = dt.Rows[0]["SINPHONE"].ToString().Trim();
                    txtJipDay.Text = dt.Rows[0]["JIPDAY"].ToString().Trim();
                    txtJipSpace.Text = dt.Rows[0]["JIPSPACE"].ToString().Trim();
                    txtJipName.Text = dt.Rows[0]["JIPNAME"].ToString().Trim();
                    txtJipSere.Text = dt.Rows[0]["JIPSERE"].ToString().Trim();
                    txtGyoJukNo.Text = dt.Rows[0]["GYOJUKNO"].ToString().Trim();
                    txtTermiReason.Text = dt.Rows[0]["TERMIREASON"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strTitle = "";
            strTitle = "";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            //strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 50, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(ssPrint2, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);
            CS = null;
        }

        private void dataDelete(string strSeqno)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strActiveNo = "";

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = "DELETE FROM KOSMOS_ADM.WONMOK_PATIENT_APPLICATION";
                SQL = SQL + ComNum.VBLF + " WHERE SEQNO = '" + strSeqno + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                    return;
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
                return;
            }
        }
    }
}
