using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// Class Name      : ComLibB.dll
    /// File Name       : frmLtdCode.cs
    /// Description     : LMC Code 등록 프로그램
    /// Author          : 박성완
    /// Create Date     : 2017-06-12
    /// </summary>
    /// <history>  
    /// 2017-06-19 코드 찾기, 우편번호 찾기 폼에서 데이터 연동 추가 - 박성완 
    /// 2017-06-27 이벤트 및 메소드 정리 작업 - 박성완
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\basic\BuKiho\Bukiho.vbp

    public partial class frmLtdCode : Form
    {
        string GstrRetValue = ""; //찾기 Help화면의 Return Value값
        string FstrROWID    = "";
        string FstrCode     = "";
    
        //자식 폼의 인스턴스를 
        private frmBuKiho05 frmBuKiho05X;
        private frmMail frmMailX;
        private FrmCodehelp FrmCodehelpX;

        public frmLtdCode()
        {
            InitializeComponent();

            setEvent();
        }

        #region 메소드 모음

        #region 화면 정리 메소드
        void Screen_Clear()
        {
            txtCode.Text      = "";
            txtSangho.Text    = "";
            txtName.Text      = "";
            txtMail.Text      = "";
            txtJuso.Text      = "";
            txtSaupNo.Text    = "";
            txtTel.Text       = "";
            txtFax.Text       = "";
            txtDaepyo.Text    = "";
            txtJumin.Text     = "";
            txtSanKiho.Text   = "";
            txtUptae.Text     = "";
            txtJongmok.Text   = "";
            txtKiho.Text      = "";
            txtUpjong.Text    = "";
            txtBoName.Text    = "";
            txtBoJik.Text     = "";
            txtNodong.Text    = "";
            txtJido.Text      = "";
            txtJisa.Text      = "";
            txtJepumList.Text = "";
            txtEmail.Text     = "";
            txtRemark.Text    = "";
            txtJisa.Text      = "";
            txtArmy.Text      = "";
            txtUpso.Text      = "";
            txtJuDetail.Text  = "";
            txtNegoDate.Text  = "";
            txtMAmt.Text      = "";
            txtFAmt.Text      = "";
            txtInWon.Text     = "";
            dtpSelDate.Text   = "";
            dtpGyeDate.Text   = "";
            dtpDelDate.Text   = "";
            cboGyumo.Text     = "";
            lblArmy.Text      = "";
            lblJisa.Text      = "";
            lblJido.Text      = "";
            lblNoDong.Text    = "";
            lblUpjong.Text    = "";

            ss2_Sheet1.ClearRange(0, 0, ss2_Sheet1.Rows.Count, ss2_Sheet1.ColumnCount, true);

            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
            if (clsPublic.GnJobSabun == 19684)
            {
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                btnNewNo.Enabled = true;
            }
        }

        #endregion

        #region 화면 초기화 메소드
        void Screen_Display()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            FstrCode = string.Format("{0:0000}", txtCode.Text);
            txtCode.Text = FstrCode;
            FstrROWID = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "SELECT Sangho,Name,Tel,Fax,EMail,MailCode,Juso,SaupNo,UpTae,JongMok,Daepyo,Jumin,Jisa,Kiho, ";
                SQL = SQL + ComNum.VBLF + " UpJong,SanKiho,Gwanse,Jidowon,BoName,BoJik,GyuMoGbn,GesiNo,YOUNGUPSO,JUSODETAIL, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SelDate,'YYYY-MM-DD') SelDate,TO_CHAR(negoDate,'YYYY-MM-DD') negoDate,mamt,famt, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(GyeDate,'YYYY-MM-DD') GyeDate,Jepum1,Jepum2,Jepum3,Jepum4,Jepum5, ";
                SQL = SQL + ComNum.VBLF + " GbGemjin,GbChukJeng,GbDaeHang,GbJongGum,GbGukGo,ARMY_HSP,Inwon, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,JepumList,Remark,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM ETC_WONLTD ";
                SQL = SQL + ComNum.VBLF + "WHERE Code='" + FstrCode + "' ";

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
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    
                    txtSangho.Text = dt.Rows[0]["Sangho"].ToString();
                    txtName.Text = dt.Rows[0]["Name"].ToString();
                    txtMail.Text = dt.Rows[0]["MailCode"].ToString();
                    //TOD:READ_MAIL_Name 필요
                    //txtJuso.Text = READ_MAIL_Name(txtMail.Text.Trim());
                    txtJuDetail.Text = dt.Rows[0]["JUSODETAIL"].ToString();
                    txtSaupNo.Text = dt.Rows[0]["SaupNo"].ToString();
                    txtTel.Text = dt.Rows[0]["Tel"].ToString();
                    txtFax.Text = dt.Rows[0]["Fax"].ToString();
                    dtpSelDate.Text = dt.Rows[0]["SelDate"].ToString();
                    txtDaepyo.Text = dt.Rows[0]["DaePyo"].ToString();
                    txtSanKiho.Text = dt.Rows[0]["SanKiho"].ToString();
                    dtpGyeDate.Text = dt.Rows[0]["GyeDate"].ToString();
                    txtUptae.Text = dt.Rows[0]["UpTae"].ToString();
                    txtJongmok.Text = dt.Rows[0]["JongMok"].ToString();
                    txtJisa.Text = dt.Rows[0]["Jisa"].ToString();
                    txtKiho.Text = dt.Rows[0]["Kiho"].ToString();
                    txtUpjong.Text = dt.Rows[0]["UpJong"].ToString();
                    txtBoName.Text = dt.Rows[0]["BoName"].ToString();
                    txtBoJik.Text = dt.Rows[0]["BoJik"].ToString();
                    dtpDelDate.Text = dt.Rows[0]["DelDate"].ToString();
                    txtNodong.Text = dt.Rows[0]["GwanSe"].ToString();
                    txtJido.Text = dt.Rows[0]["JidoWon"].ToString();
                    txtArmy.Text = dt.Rows[0]["ARMY_HSP"].ToString();
                    txtJepumList.Text = dt.Rows[0]["JepumList"].ToString();
                    txtEmail.Text = dt.Rows[0]["EMail"].ToString();
                    txtRemark.Text = dt.Rows[0]["Remark"].ToString();
                    txtUpso.Text = dt.Rows[0]["YOUNGUPSO"].ToString();
                    cboGyumo.SelectedIndex = Convert.ToInt32(dt.Rows[0]["GyumoGbn"]) - 1;
                    txtNegoDate.Text = dt.Rows[0]["negodate"].ToString();
                    txtMAmt.Text = String.Format("{0:#,###,##0}", dt.Rows[0]["mamt"]);
                    txtFAmt.Text = String.Format("{0:#,###,##0}", dt.Rows[0]["famt"]);
                    txtInWon.Text = dt.Rows[0]["Inwon"].ToString();

                    //제품코드 1
                    ss2_Sheet1.Cells[0, 0].Text = dt.Rows[0]["Jepum1"].ToString().Trim();
                    ss2_Sheet1.Cells[0, 1].Text = READ_HIC_CODE("04", dt.Rows[0]["Jepum1"].ToString().Trim());
                    //제품코드 2
                    ss2_Sheet1.Cells[1, 0].Text = dt.Rows[0]["Jepum2"].ToString().Trim();
                    ss2_Sheet1.Cells[1, 1].Text = READ_HIC_CODE("04", dt.Rows[0]["Jepum2"].ToString().Trim());
                    //제품코드 3
                    ss2_Sheet1.Cells[2, 0].Text = dt.Rows[0]["Jepum3"].ToString().Trim();
                    ss2_Sheet1.Cells[2, 1].Text = READ_HIC_CODE("04", dt.Rows[0]["Jepum3"].ToString().Trim());
                    //제품코드 4
                    ss2_Sheet1.Cells[3, 0].Text = dt.Rows[0]["Jepum4"].ToString().Trim();
                    ss2_Sheet1.Cells[3, 1].Text = READ_HIC_CODE("04", dt.Rows[0]["Jepum4"].ToString().Trim());
                    //제품코드 5
                    ss2_Sheet1.Cells[4, 0].Text = dt.Rows[0]["Jepum5"].ToString().Trim();
                    ss2_Sheet1.Cells[4, 1].Text = READ_HIC_CODE("04", dt.Rows[0]["Jepum5"].ToString().Trim());

                    lblUpjong.Text = READ_HIC_CODE("01", dt.Rows[0]["UpJong"].ToString().Trim());
                    lblNoDong.Text = READ_HIC_CODE("02", dt.Rows[0]["GwanSe"].ToString().Trim());
                    lblJido.Text = READ_HIC_CODE("03", dt.Rows[0]["JidoWon"].ToString().Trim());
                    lblJisa.Text = READ_HIC_CODE("21", dt.Rows[0]["Jisa"].ToString().Trim());
                    lblArmy.Text = READ_HIC_CODE("26", dt.Rows[0]["ARMY_HSP"].ToString().Trim());

                    dt.Dispose();
                    dt = null;

                    if (FstrROWID == "") { btnDelete.Enabled = false; }
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnNewNo.Enabled = false;

                    if (clsPublic.GnJobSabun == 14875 || clsPublic.GnJobSabun == 19684) //강성민, 박시철
                    {
                        btnSave.Enabled = true;
                        btnDelete.Enabled = true;
                        btnNewNo.Enabled = true;
                    }

                    txtSangho.Focus();
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        #endregion

        #region 자료조회 메소드
        bool ViewData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "";
                SQL = SQL + ComNum.VBLF + "";
                SQL = SQL + ComNum.VBLF + "";

                if (optSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY Name ";
                }
                else if (optSort1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY Code ";
                }
                else if (optSort2.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY KIHO,YOUNGUPSO ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }

                ss1_Sheet1.Rows.Count = dt.Rows.Count;
                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ss1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Code"].ToString();
                    ss1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString();
                    ss1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SaupNo"].ToString();
                    ss1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["Daepyo"].ToString();
                    ss1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["kiho"].ToString();
                    ss1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["gwanse"].ToString();
                    ss1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["jisa"].ToString();
                    ss1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["sankiho"].ToString();
                    ss1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["upjong"].ToString();
                    ss1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["UpTae"].ToString();
                    ss1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["JongMok"].ToString();
                    ss1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["Tel"].ToString();
                    ss1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["Fax"].ToString();
                    ss1_Sheet1.Cells[i, 13].Text = dt.Rows[i]["BoName"].ToString();
                    ss1_Sheet1.Cells[i, 14].Text = dt.Rows[i]["GyeDate"].ToString();
                    ss1_Sheet1.Cells[i, 15].Text = dt.Rows[i]["MAILCODE"].ToString();
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        #endregion

        #region 자료삭제 메소드(FstrROWID)
        bool DeleteData()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            long nCnt = 0;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            //일반건진에 자료가 발생하였는지 점검
            SQL = "SELECT COUNT(*) CNT FROM HIC_JEPSU ";
            SQL = SQL + ComNum.VBLF + "WHERE LtdCode='" + txtCode.Text.Trim() + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            nCnt = Convert.ToInt64(dt.Rows[0]["CNT"]);

            dt.Dispose();
            dt = null;
            if (nCnt > 0)
            {
                MessageBox.Show("일반검진 접수시 회사코드가 사용되어 삭제 불가", "삭제불가능");
                return false;
            }

            //종합건진에 자료가 발생하였는지 점검
            SQL = "SELECT COUNT(*) CNT FROM HEA_JEPSU ";
            SQL = SQL + ComNum.VBLF + "WHERE LtdCode='" + txtCode.Text.Trim() + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            nCnt = Convert.ToInt64(dt.Rows[0]["CNT"]);

            dt.Dispose();
            dt = null;
            if (nCnt > 0)
            {
                MessageBox.Show("종합건진 접수시 회사코드가 사용되어 삭제 불가", "삭제불가능");
                return false;
            }

            //미수마스타에 자료가 발생하였는지 점검
            SQL = "SELECT COUNT(*) CNT FROM HIC_MISU_MST ";
            SQL = SQL + ComNum.VBLF + "WHERE LtdCode='" + txtCode.Text.Trim() + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            nCnt = Convert.ToInt64(dt.Rows[0]["CNT"]);

            dt.Dispose();
            dt = null;
            if (nCnt > 0)
            {
                MessageBox.Show("미수마스타에 회사코드가 사용되어 삭제 불가", "삭제불가능");
                return false;
            }

            clsPublic.GstrMsgList = "코드를 삭제하면 삭제한 회사와 관련된 자료를 조회 시" + "\n";
            clsPublic.GstrMsgList += "오류가 발생합니다." + "\n";
            clsPublic.GstrMsgList += "정말로 삭제를 하시겠습니까?" + "\n";

            if (MessageBox.Show(clsPublic.GstrMsgList, "선택", MessageBoxButtons.YesNo) == DialogResult.No) { return false; }

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "DELETE ETC_WONLTD WHERE ROWID='" + FstrROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
                Screen_Clear();
                txtCode.Focus();
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        #endregion

        #region 자료저장 메소드
        bool SaveData()
        {
            string[] strJepum = new string[5];
            string[] strGbFlag = new string[5];
            string strRemark = "";
            string strJuso = "";
            long nMAmt = 0;
            long nFAmt = 0;

            int intRowAffected = 0; //변경된 Row 받는 변수
            string SQL = "";
            string SqlErr = "";

            //자료에 오류가 있는지 Check
            if (txtCode.Text.Length != 4)
            {
                MessageBox.Show("회사코드를 반드시 4자리 숫자로 입력하세요", "오류");
                return false;
            }
            if (txtSangho.Text.Trim() == "")
            {
                MessageBox.Show("상호가 공란입니다.", "오류");
                return false;
            }
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("약칭상호가 공란입니다.", "오류");
                return false;
            }
            if (txtJumin.Text.Trim() == "")
            {
                if (txtJumin.Text.Length != 14 || txtJumin.Text.Substring(6, 1) != "-")
                {
                    MessageBox.Show("주민등록번호를 YYMMDD-1234567 형태로 입력하세요", "확인");
                    return false;
                }
            }
            if (txtKiho.Text.Trim().Substring(0, 1) == "5" && txtArmy.Text == "")
            {
                MessageBox.Show("군병원을 선택하세요.", "오류");
                return false;
            }

            if (txtMAmt.Text == "") { nMAmt = 0; }
            nMAmt = Convert.ToInt64(String.Format("{0:######0}", txtMAmt.Text));
            if (txtFAmt.Text == "") { nFAmt = 0; }
            nFAmt = Convert.ToInt64(String.Format("{0:######0}", txtFAmt.Text));

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                //Quotation 문자를 "`"로 변경
                strRemark = txtRemark.Text.Replace("'", "`");
                strRemark = strRemark.Replace("·", ".");
                strJepum[0] = ss2_Sheet1.Cells[0, 0].Text.Trim();
                strJepum[1] = ss2_Sheet1.Cells[1, 0].Text.Trim();
                strJepum[2] = ss2_Sheet1.Cells[2, 0].Text.Trim();
                strJepum[3] = ss2_Sheet1.Cells[3, 0].Text.Trim();
                strJepum[4] = ss2_Sheet1.Cells[4, 0].Text.Trim();
                strGbFlag[0] = "N";
                strGbFlag[1] = "N";
                strGbFlag[2] = "N";
                strGbFlag[3] = "N";
                strGbFlag[4] = "N";

                strJuso = "";
                strJuso = txtJuso.Text.Trim();
                if (FstrROWID == "")
                {
                    //Insert
                    if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
                    SQL = "INSERT INTO ETC_WONLTD (CODE,SANGHO,Name,TEL,FAX,EMAIL,MAILCODE,JUSO,jusodetail,SAUPNO,UPTAE,";
                    SQL = SQL + ComNum.VBLF + "JONGMOK,DAEPYO,JUMIN,Jisa,KIHO,UPJONG,SANKIHO,GWANSE,JIDOWON,BONAME,BOJIK,";
                    SQL = SQL + ComNum.VBLF + "GYUMOGBN,SELDATE,GYEDATE,JEPUM1,JEPUM2,JEPUM3,JEPUM4,JEPUM5,";
                    SQL = SQL + ComNum.VBLF + "GBGEMJIN,GBCHUKJENG,GBDAEHANG,GBJONGGUM,GBGUKGO,DELDATE,JEPUMLIST,";
                    SQL = SQL + ComNum.VBLF + "REMARK,ARMY_HSP,YOUNGUPSO,negodate,mamt,famt,Inwon ) VALUES (";
                    SQL = SQL + ComNum.VBLF + " '" + FstrCode + "','" + txtSangho.Text.Trim() + "','" + txtName.Text.Trim() + "','" + txtTel.Text + "',";
                    SQL = SQL + ComNum.VBLF + " '" + txtFax.Text + "','" + txtEmail.Text + "','" + txtMail.Text + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strJuso + "','" + txtJuDetail.Text + "','" + txtSaupNo.Text + "','" + txtUptae.Text + "',";
                    SQL = SQL + ComNum.VBLF + " '" + txtJongmok.Text + "','" + txtDaepyo.Text + "','" + txtJumin.Text + "',";
                    SQL = SQL + ComNum.VBLF + " '" + txtJisa.Text + "','" + txtKiho.Text.Trim() + "','" + txtUpjong.Text + "','" + txtSanKiho.Text + "',";
                    SQL = SQL + ComNum.VBLF + " '" + txtNodong.Text + "','" + txtJido.Text + "','" + txtBoName.Text + "',";
                    SQL = SQL + ComNum.VBLF + " '" + txtBoJik.Text + "','" + cboGyumo.Text.Substring(0, 1) + "',";
                    SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpSelDate.Text + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpGyeDate.Text + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " '" + strJepum[0] + "','" + strJepum[1] + "','" + strJepum[2] + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strJepum[3] + "','" + strJepum[4] + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strGbFlag[0] + "','" + strGbFlag[1] + "','" + strGbFlag[2] + "',";
                    SQL = SQL + ComNum.VBLF + " '" + strGbFlag[3] + "','" + strGbFlag[4] + "',";
                    SQL = SQL + ComNum.VBLF + "TO_DATE('" + dtpDelDate.Text + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " '" + txtJepumList.Text + "','" + strRemark + "','" + txtArmy.Text.Trim() + "','" + txtUpso.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + txtNegoDate.Text + "','YYYY-MM-DD')," + nMAmt + "," + nFAmt + " , " + int.Parse(txtInWon.Text) + "   ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                }
                else
                {
                    if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return false; //권한 확인
                    //Update
                    SQL = "UPDATE ETC_WONLTD SET SANGHO='" + txtSangho.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "NAME='" + txtName.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "TEL='" + txtTel.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "FAX='" + txtFax.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "EMAIL='" + txtEmail.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "MAILCODE='" + txtMail.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "JUSO='" + strJuso.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "JUSODETAIL='" + txtJuDetail.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "SAUPNO='" + txtSaupNo.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "UPTAE='" + txtUptae.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "JONGMOK='" + txtJongmok.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "DAEPYO='" + txtDaepyo.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "JUMIN='" + txtJumin.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "JISA='" + txtJisa.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "KIHO='" + txtKiho.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "UPJONG='" + txtUpjong.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "SANKIHO='" + txtSanKiho.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "GWANSE='" + txtNodong.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "JIDOWON='" + txtJido.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "BONAME='" + txtBoName.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "BOJIK='" + txtBoJik.Text.Trim() + "',";
                    SQL = SQL + ComNum.VBLF + "GYUMOGBN='" + cboGyumo.Text.Substring(0, 1) + "',";
                    SQL = SQL + ComNum.VBLF + "SELDATE=TO_DATE('" + dtpSelDate.Text + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + "GyeDate=TO_DATE('" + dtpGyeDate.Text + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + "DelDate=TO_DATE('" + dtpDelDate.Text + "','YYYY-MM-DD'),";
                    for (int i = 1; i <= 5; i++)
                    {
                        SQL = SQL + ComNum.VBLF + "GbGemjin='" + strGbFlag[0] + "',";
                        SQL = SQL + ComNum.VBLF + "GbChukJeng='" + strGbFlag[1] + "',";
                        SQL = SQL + ComNum.VBLF + "GbDaeHang='" + strGbFlag[2] + "',";
                        SQL = SQL + ComNum.VBLF + "GbJonggum='" + strGbFlag[3] + "',";
                        SQL = SQL + ComNum.VBLF + "GbGukgo='" + strGbFlag[4] + "',";
                        SQL = SQL + ComNum.VBLF + "JepumList='" + txtJepumList.Text + "',";
                        SQL = SQL + ComNum.VBLF + "Remark='" + strRemark + "', ";
                        SQL = SQL + ComNum.VBLF + "ARMY_HSP='" + txtArmy.Text + "', ";
                        SQL = SQL + ComNum.VBLF + "negodate = TO_DATE('" + txtNegoDate.Text + "','YYYY-MM-DD'), ";
                        SQL = SQL + ComNum.VBLF + "mamt = " + nMAmt + ", ";
                        SQL = SQL + ComNum.VBLF + "famt = " + nFAmt + ", ";
                        SQL = SQL + ComNum.VBLF + "Inwon = " + VB.Val(txtInWon.Text) + ", ";
                        SQL = SQL + ComNum.VBLF + "YOUNGUPSO ='" + txtUpso.Text.Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + FstrROWID + "' ";
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Screen_Clear();
                txtCode.Focus();
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        #endregion

        #region TODO:

        //READ_HIC_CODE 함수 폼안에 임시 사용 - BuKiho1 - HcBas.bas 찾아봐도 없어서 그대로 사용
        string READ_HIC_CODE(string Gubun, string Code)
        {
            string rtnVal = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (Code.Trim() == "") { return ""; }

            SQL = "SELECT Name FROM HIC_CODE ";
            SQL = SQL + ComNum.VBLF + "WHERE Gubun='" + Gubun +"' ";
            SQL = SQL + ComNum.VBLF + "  AND Code='" + Code + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return "";
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }


        #endregion

        #region FrmBukiho05 자식폼 닫는 메소드 (각각 버튼이 사용 하므로 메소드로 따로 구현)
        private void FrmBuKiho05X_rEventClosed()
        {
            frmBuKiho05X.Dispose();
            frmBuKiho05X = null;
        }
        #endregion

        #region 각 컨트롤들의 Tab 넘기는 메소드 이벤트는 디자이너로~
        private void Every_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{Tab}");
            }
        }

        #endregion

        #endregion

        #region 이벤트 처리
        void setEvent()
        {
            //폼로드
            this.Load += (sender, e) => 
            {
                if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
                {
                    this.Close(); //폼 권한 조회
                    return;
                }
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                txtViewCode.Text = "";
                btnPrint.Enabled = false;
                cboGyumo.Items.Add("1.  50명미만");
                cboGyumo.Items.Add("2. 300명미만");
                cboGyumo.Items.Add("3.1000명미만");
                cboGyumo.Items.Add("4.1000명이상");
                Screen_Clear();
                if (GstrRetValue != "") { txtCode.Text = GstrRetValue; }
                btnNewNo.Enabled = false;
                if (clsPublic.GnJobSabun == 14875 || clsPublic.GnJobSabun == 19684)
                {
                    btnSave.Enabled = true;
                    btnDelete.Enabled = true;
                    btnNewNo.Enabled = true;
                }
                GstrRetValue = "";
            };

            #region Click 이벤트 처리~
            btnView.Click += (sender, e) => { if (ViewData() == false) return; };
            btnSave.Click += (sender, e) => { if (SaveData() == false) return; };
            btnDelete.Click += (sender, e) => { if (DeleteData() == false) return; };
            btnCancel.Click += (sender, e) => { Screen_Clear(); txtCode.Focus(); };
            btnExit.Click += (sender, e) => { this.Close(); };
            btnArmy.Click += (sender, e) =>
            {
                GstrRetValue = "26"; //군병원

                //중복체크
                if (frmBuKiho05X != null)
                {
                    frmBuKiho05X.Dispose();
                    frmBuKiho05X = null;
                }

                frmBuKiho05X = new frmBuKiho05(GstrRetValue);
                //넘겨 받은 데이터 셋팅
                frmBuKiho05X.rSetCodeName += (rtnCodeName) => 
                {
                    if (rtnCodeName != "")
                    {
                        txtArmy.Text = VB.Left(rtnCodeName, 10);
                        txtArmy.Text = VB.Right(rtnCodeName, rtnCodeName.Length - 10);
                    }
                };
                frmBuKiho05X.rEventClosed += FrmBuKiho05X_rEventClosed;
                frmBuKiho05X.Show();
            };
            btnHelpKiho.Click += (sender, e) =>
            {
                GstrRetValue = "18"; //사업장기호
                //중복체크
                if (frmBuKiho05X != null)
                {
                    frmBuKiho05X.Dispose();
                    frmBuKiho05X = null;
                }

                frmBuKiho05X = new frmBuKiho05(GstrRetValue);
                //넘겨 받은 데이터 셋팅 
                frmBuKiho05X.rSetCodeName += (rtnCodeName) => 
                {
                    if (rtnCodeName != "")
                    {
                        txtKiho.Text = VB.Left(rtnCodeName, 10).Trim();
                    }
                    GstrRetValue = "";
                };
                frmBuKiho05X.rEventClosed += FrmBuKiho05X_rEventClosed;
                frmBuKiho05X.Show();
            };
            btnHelpJido.Click += (sender, e) =>
            {
                GstrRetValue = "03"; //지도원
                //중복체크
                if (frmBuKiho05X != null)
                {
                    frmBuKiho05X.Dispose();
                    frmBuKiho05X = null;
                }

                frmBuKiho05X = new frmBuKiho05(GstrRetValue);
                //넘겨받은 데이터 셋팅
                frmBuKiho05X.rSetCodeName += (rtnCodeName) =>
                {
                    if (rtnCodeName != "")
                    {
                        txtJido.Text = VB.Left(rtnCodeName, 10).Trim();
                        lblJido.Text = VB.Right(rtnCodeName, rtnCodeName.Length - 10);
                    }
                };
                frmBuKiho05X.rEventClosed += FrmBuKiho05X_rEventClosed;
                frmBuKiho05X.Show();
            };
            btnHelpJisa.Click += (sender, e) =>
            {
                GstrRetValue = "21"; //건강보험지사코드
                //중복체크
                if (frmBuKiho05X != null)
                {
                    frmBuKiho05X.Dispose();
                    frmBuKiho05X = null;
                }

                frmBuKiho05X = new frmBuKiho05(GstrRetValue);
                //넘겨받은 데이터 셋팅
                frmBuKiho05X.rSetCodeName += (rtnCodeName) =>
                {
                    if (rtnCodeName != "")
                    {
                        txtJisa.Text = VB.Left(rtnCodeName, 10).Trim();
                        lblJisa.Text = VB.Right(rtnCodeName, rtnCodeName.Length - 10);
                    }
                };
                frmBuKiho05X.rEventClosed += FrmBuKiho05X_rEventClosed;
                frmBuKiho05X.Show();
            };
            btnHelpNodong.Click += (sender, e) =>
            {
                GstrRetValue = "02";
                //중복체크
                if (frmBuKiho05X != null)
                {
                    frmBuKiho05X.Dispose();
                    frmBuKiho05X = null;
                }

                frmBuKiho05X = new frmBuKiho05(GstrRetValue);
                //넘겨받은 데이터 셋팅
                frmBuKiho05X.rSetCodeName += (rtnCodeName) =>
                {
                    if (rtnCodeName != "")
                    {
                        txtNodong.Text = VB.Left(rtnCodeName, 10).Trim();
                        lblNoDong.Text = VB.Right(rtnCodeName, rtnCodeName.Length - 10);
                    }
                };
                frmBuKiho05X.rEventClosed += FrmBuKiho05X_rEventClosed;
                frmBuKiho05X.Show();
            };
            btnHelpUpjong.Click += (sender, e) =>
            {
                GstrRetValue = "01"; //업종
                //중복체크
                if (frmBuKiho05X != null)
                {
                    frmBuKiho05X.Dispose();
                    frmBuKiho05X = null;
                }

                frmBuKiho05X = new frmBuKiho05(GstrRetValue);
                //넘겨받은 데이터 셋팅
                frmBuKiho05X.rSetCodeName += (rtnCodeName) =>
                {
                    if (rtnCodeName != "")
                    {
                        txtUpjong.Text = VB.Left(rtnCodeName, 10).Trim();
                        lblUpjong.Text = VB.Right(rtnCodeName, rtnCodeName.Length - 10);
                    }
                };
                frmBuKiho05X.rEventClosed += FrmBuKiho05X_rEventClosed;
                frmBuKiho05X.Show();
            };
            btnMailHelp.Click += (sender, e) =>
            {
                GstrRetValue = "";
                //중복체크
                if (frmMailX != null)
                {
                    frmMailX.Dispose();
                    frmMailX = null;
                }

                frmMailX = new frmMail(GstrRetValue);
                //넘겨받은 데이터 셋팅
                frmMailX.SendEvent += (string SendRetValue) =>
                {
                    if (SendRetValue != "")
                    {
                        txtMail.Text = VB.Left(SendRetValue, 6);
                        txtJuso.Text = VB.Right(SendRetValue, SendRetValue.Length - 8);
                        GstrRetValue = "";
                        txtJuDetail.Focus();
                    }
                };
                frmMailX.rEventClosed += () => 
                {
                    if (frmMailX != null)
                    {
                        frmMailX.Dispose();
                        frmMailX = null;
                    }
                };
                frmMailX.Show();
            };
            btnPrint.Click += (sender, e) =>
            {
                int ssPrintRow = 0;
                int nF = 0;
                int nT = 0;

                nF = int.Parse(txtF.Text);
                nT = int.Parse(txtT.Text);
                //txtF ~ txtT 까지의 행 인쇄
                for (int ss1Row = nF; ss1Row <= nT; ss1Row++)
                {
                    ssPrintRow = ss1Row - nF;
                    for (int j = 0; j < 16; j++)
                    {
                        ssPrint_Sheet1.Cells[ssPrintRow, j].Text = ss1_Sheet1.Cells[ss1Row, j].Text;
                    }
                }

                clsSpread SPR = new clsSpread();
                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                strTitle = "사  업  장   코  드  집";

                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "Page : /p " + clsPublic.GstrJobName, new Font("굴림체", 11), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                SPR.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);
            };
            btnNewNo.Click += (sender, e) => 
            {
                int nNewNo = 0;
                string SQL = "";
                string SqlErr = "";
                DataTable dt = null;

                SQL = "SELECT MAX(Code) MaxCode FROM ETC_WONLTD ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                nNewNo = 0;
                if (dt.Rows.Count > 0) { nNewNo = Convert.ToInt32(dt.Rows[0]["MaxCode"]); }
                dt.Dispose();
                dt = null;
                txtCode.Text = string.Format("{0:0000}", nNewNo + 1);
                Screen_Display();
            };
            #endregion

            #region CellDoubleClick 이벤트 처리~
            ss1.CellDoubleClick += (sender, e) => 
            {
                FstrCode = ss1_Sheet1.Cells[e.Row, 0].Text;
                txtCode.Text = FstrCode;
                Screen_Display();
            };
            ss2.CellDoubleClick += (sender, e) => 
            {
                if (e.Column != 1) { return; }

                GstrRetValue = "04";

                if (FrmCodehelpX != null)
                {
                    FrmCodehelpX.Dispose();
                    FrmCodehelpX = null;
                }

                FrmCodehelpX = new FrmCodehelp(GstrRetValue);
                FrmCodehelpX.rSetHelpName += (string rtnHelpCode) =>
                {
                    if (rtnHelpCode != "")
                    {
                        ss2_Sheet1.Cells[e.Row, 0].Text = rtnHelpCode.Substring(0, 10).Trim();
                        ss2_Sheet1.Cells[e.Row, 1].Text = rtnHelpCode.Substring(0, 10).Trim();
                    }
                };
                FrmCodehelpX.rEventClosed += () =>
                {
                    if (FrmCodehelpX != null)
                    {
                        FrmCodehelpX.Dispose();
                        FrmCodehelpX = null;
                    }
                };
                FrmCodehelpX.Show();
            };
            #endregion

            #region Leave 이벤트 처리~
            txtJido.Leave += (sender, e) => { lblJido.Text = READ_HIC_CODE("03", txtJido.Text.Trim()); };
            txtJisa.Leave += (sender, e) => { lblJisa.Text = READ_HIC_CODE("21", txtJisa.Text.Trim()); };
            txtUpjong.Leave += (sender, e) => { lblUpjong.Text = READ_HIC_CODE("01", txtUpjong.Text.Trim()); };
            txtCode.Leave += (sender, e) => 
            {
                txtCode.Text = txtCode.Text.Trim();
                if (txtCode.Text == "") { return; }

                txtCode.Text = String.Format("{0:0000}", txtCode.Text);
                Screen_Display();
            };
            txtKiho.Leave += (sender, e) =>
            {
                string strName = "";

                txtKiho.Text = txtKiho.Text.Trim();
                if (txtKiho.Text == "") { return; }
                strName = READ_HIC_CODE("18", txtKiho.Text.Trim());
                if (strName == "")
                {
                    MessageBox.Show(txtKiho.Text + " 기초코드의 사업장코드에 등록이 않됨", "오류");
                    txtKiho.Text = "";
                }
            };
            #endregion

            #region KeyPress 이벤트 처리~
            txtNodong.KeyPress += (sender, e) => { lblNoDong.Text = READ_HIC_CODE("02", txtNodong.Text.Trim()); };
            txtMail.KeyPress += (sender, e) =>
            {
                if (e.KeyChar == 13)
                {
                    txtJuso.Text = clsVbfunc.GetBASMail(clsDB.DbCon, txtMail.Text.Trim());
                    SendKeys.Send("{Tab}");
                }
            };          
            #endregion

        }
        #endregion


    }
}
