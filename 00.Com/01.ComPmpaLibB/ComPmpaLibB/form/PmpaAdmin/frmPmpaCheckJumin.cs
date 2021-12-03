using ComLibB;
using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System.Drawing;

/// <summary>
/// Description : 신환환자 등록
/// Author : 박병규
/// Create Date : 2017.05.20
/// </summary>
/// <history>
/// </history>


namespace ComPmpaLibB
{
    public partial class frmCheckJumin : Form
    {
        DataTable Dt = new DataTable();
        string SQL = "";
        string SqlErr = "";
        clsOrdFunction OF = null;


        private string strFlag;
        private int nReadCnt;
        private string[] strTblPtno = new string[10];

        public frmCheckJumin()
        {
            InitializeComponent();

            setParam();
            Display_One_Monitor();
        }

        private void setParam()
        {
            this.Load += new EventHandler(eFrm_Load);
        }
        void Display_One_Monitor()
        {
            Screen[] screens = Screen.AllScreens;

          
            Screen scrn = (screens[0]);
            this.Location = new Point(scrn.Bounds.Left, 0);

           
        }
        private void eFrm_Load(object sender, EventArgs e)
        {
            OF = new clsOrdFunction();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlTop);
            ComFunc.SetAllControlClear(pnlBody);

            lblMsg.Text = "";
            txtJumin1.Select();

            if (clsCall.GstrJumin1_Call != "" && clsCall.GstrJumin2_Call != "")
            {
                txtJumin1.Text = clsCall.GstrJumin1_Call;
                txtJumin2.Text = clsCall.GstrJumin2_Call;
                txtSname.Select();
            }
            else if (clsCall.GstrJumin1_WaitCall != "" && clsCall.GstrJumin2_WaitCall != "")
            {
                txtJumin1.Text = clsCall.GstrJumin1_WaitCall;
                txtJumin2.Text = clsCall.GstrJumin2_WaitCall;
                txtSname.Select();
            }
           // clsCall.GstrJumin1_Call = "";
           // clsCall.GstrJumin2_Call = "";
            clsCall.GstrJumin1_WaitCall = "";
            clsCall.GstrJumin2_WaitCall = "";

            btnSave.Enabled = false;
        }

        //닫기버튼
        private void btnExit_Click(object sender, EventArgs e)
        {
            clsPmpaPb.GstrJuminFlag = "";
            clsCall.GstrSunapFlag_Call = "";

            this.Close();
        }

        //등록버튼
        private void btnOk_Click(object sender, EventArgs e)
        {
            if(clsPmpaPb.GstrJuminFlag == "OK")
            {
                ComFunc.MsgBox("★ 해당 주민번호로 외래등록번호가 이미 존재합니다.", "확인요망");

                if (chkBaby.Checked == true)
                {
                    DialogResult result = ComFunc.MsgBoxQ("★ 신생아 신환등록 ★" + '\r' + "주민번호 중복을 무시하고 신환을 부여하시겠습니까?", "확인요망", MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.No) {return;}
                }
                else
                {
                    return;
                }
            }

            clsPmpaPb.GstrJumin1 = txtJumin1.Text;
            clsPmpaPb.GstrJumin2 = txtJumin2.Text;
            clsPmpaPb.GstrSname = txtSname.Text;

            clsPmpaPb.GstrJuminFlag = "NO";
            this.Close();
        }
        
        
        private void txtJumin1_KeyPress(object sender, KeyPressEventArgs e)
        {
            double nMM;
            double nDD;

            if (e.KeyChar == (char)13)
            {
                if (txtJumin1.Text == "") return;
                if (txtJumin1.Text.Length != 6)
                {
                    lblMsg.Text = "주민번호 입력 ERROR !";
                    txtJumin1.Focus();
                    return;
                }

                txtJumin1.Text = txtJumin1.Text.Trim().Substring(0, 6);

                nMM = VB.Val(txtJumin1.Text.Trim().Substring(2, 2));
                nDD = VB.Val(txtJumin1.Text.Trim().Substring(4, 2));

                if (nMM < 1 || nMM > 12)
                {
                    BaseAPI.Beep(3000, 200);
                    strFlag = "NO";
                    lblMsg.Text  = "월(Month) ERROR !  PLEASE 01 - 12";
                    txtJumin1.Focus();
                    return;
                }
                else if (nDD < 1 || nDD > 31)
                {
                    BaseAPI.Beep(3000, 200);
                    strFlag = "NO";
                    lblMsg.Text = "일(Day) ERROR !  PLEASE 01 - 31";
                    txtJumin1.Focus();
                    return;
                }
                else
                {
                    strFlag = "OK";
                    clsPmpaPb.GnAge = ComFunc.AgeCalc(clsDB.DbCon, txtJumin1.Text + txtJumin2.Text);

                }

                SendKeys.Send("{TAB}");
            }

           
        }

        //아기번호 조회
        private void Read_BabyPtno()
        {
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT PANO, SNAME, SEX, JUMIN1, JUMIN2, JUMIN3";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
            SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
            SQL = SQL + ComNum.VBLF + "    AND JUMIN1     = '" + txtJumin1.Text + "'";
            SQL = SQL + ComNum.VBLF + "    AND SNAME      LIKE '%애기'";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                return;
            }

            if (Dt.Rows.Count > 0)
            {
                ComFunc.MsgBox("동일 생년월일을 가진 " + "("+ Dt.Rows[0]["SNAME"].ToString().Trim() + Dt.Rows[0]["PANO"].ToString().Trim() + ") 가 존재합니다. 확인바랍니다.", "이중차트점검");
            }
            Dt.Dispose();
            Dt = null;
        }


        private void txtJumin2_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strDate;
            string strSex;
            string strJumin_YY;
            string strJumin_MM;
            string strJumin_DD;
            string strJumin2;
            
            if (e.KeyChar == (char) 13)
            {
                if( txtJumin1.Text.Trim() != "" && txtJumin2.Text.Trim() != "")
                {
                    strFlag = "OK";
                   // Jumin_No_Check();

                    if ( strFlag != "OK")
                    {
                        strJumin2 = txtJumin2.Text;
                        txtJumin2.Text = "";
                        BaseAPI.Beep(3000, 200);
                        lblMsg.Text = "주민번호 CHECK DIGIT ERROR !! (" + strJumin2 + ")";
                        txtJumin2.Focus();
                        return;
                    }

                    clsPmpaPb.GnAge = ComFunc.AgeCalc(clsDB.DbCon, txtJumin1.Text + txtJumin2.Text);

                    strDate = "";

                    strSex = txtJumin2.Text.Trim().Substring(0, 1);

                    strJumin_YY = txtJumin1.Text.Trim().Substring(0, 2);
                    strJumin_MM = txtJumin1.Text.Trim().Substring(2, 2);
                    strJumin_DD = txtJumin1.Text.Trim().Substring(4, 2);

                    switch (strSex)
                    {
                        case "1":
                        case "2":
                            strDate = "19" + strJumin_YY + "-" + strJumin_MM + "-" + strJumin_DD;
                            break;
                        case "3":
                        case "4":
                            strDate = "20" + strJumin_YY + "-" + strJumin_MM + "-" + strJumin_DD;
                            break;
                        case "5":
                        case "6":
                            strDate = "19" + strJumin_YY + "-" + strJumin_MM + "-" + strJumin_DD;
                            break;
                        case "7":
                        case "8":
                            strDate = "20" + strJumin_YY + "-" + strJumin_MM + "-" + strJumin_DD;
                            break;
                        case "0":
                        case "9":
                            strDate = "18" + strJumin_YY + "-" + strJumin_MM + "-" + strJumin_DD;
                            break;
                        default:
                            strDate = "19" + strJumin_YY + "-" + strJumin_MM + "-" + strJumin_DD;
                            break;
                    }

                    if (VB.DateAdd("D",365,strDate) > DateTime.Parse( clsPublic.GstrSysDate))
                    {
                        if (txtJumin2.Text.Substring(1,6) == "000000")
                        {
                            ComFunc.MsgBox("신생아 신환입니다. 엄마이름으로 구환번호가 있는지 확인바랍니다.", "이중차트점검");
                        }
                    }

                    txtSname.Focus();
                }
            }
        }


        private void txtSname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (txtSname.Text == "")
                {
                    BaseAPI.Beep(3000, 200);
                    lblMsg.Text = "성명공란(주민번호만으로 검색함)";
                    txtSname.Focus();
                }

                if (txtJumin1.Text == "" || txtJumin2.Text == "")
                {
                    BaseAPI.Beep(3000, 200);
                    lblMsg.Text = "주민번호를 입력하세요.";
                    txtJumin2.Focus();
                }

                strFlag = "OK";

                New_Patient_Check();

                if (nReadCnt == 0)
                    this.Close();
                else
                    btnNhic.Focus();
            }
        }


        private void New_Patient_Check()
        {
            clsPmpaPb.GstrSex = ComFunc.SexCheck(txtJumin1.Text + txtJumin2.Text, "2");

            clsPmpaPb.GstrJumin1 = txtJumin1.Text;
            clsPmpaPb.GstrJumin2 = txtJumin2.Text;
            clsPmpaPb.GstrSname = txtSname.Text;

            if (clsPmpaPb.GstrPtnoGbn == "E")
                Read_Etc_Ptno();
            else
                Read_HJM();
        }


        private void Read_HJM()
        {
            string strPtno;
            string strSname;
            string strPname;
            string strJumin1;
            string strJumin2;

            nReadCnt = 0;

            clsPmpaPb.GstrJuminFlag = "NO";

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ComFunc.SetAllControlClear(this.sprList);

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT PANO, SNAME, SEX, JUMIN1, JUMIN2, JUMIN3, PNAME";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND JUMIN1     = '" + txtJumin1.Text + "'";
                SQL = SQL + ComNum.VBLF + "    AND JUMIN3     = '" + clsAES.AES(txtJumin2.Text) + "'";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                    return;
                }

                sprList_Sheet1.RowCount = Dt.Rows.Count;
                sprList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);


                if (Dt.Rows.Count > 0)
                {
                    if (Dt.Rows.Count >= 2)
                    {
                        ComFunc.MsgBox("같은 주민번호로 2건이상의 환자마스터가 존재합니다.", "확인요망");
                    }

                    clsPmpaPb.GstrJuminFlag = "OK";

                    clsPmpaType.TBP.Ptno = Dt.Rows[0]["PANO"].ToString().Trim();
                    clsPmpaType.TBP.Sname = Dt.Rows[0]["SNAME"].ToString().Trim();
                    clsPmpaType.TBP.PName = Dt.Rows[0]["PNAME"].ToString().Trim();
                    clsPmpaType.TBP.Sex = Dt.Rows[0]["SEX"].ToString().Trim();
                    clsPmpaType.TBP.Jumin1 = Dt.Rows[0]["JUMIN1"].ToString().Trim();
                    clsPmpaType.TBP.Jumin2 = clsAES.DeAES(Dt.Rows[0]["JUMIN3"].ToString().Trim());

                    sprList_Sheet1.Cells[0, 0].Text = clsPmpaType.TBP.Ptno;
                    sprList_Sheet1.Cells[0, 1].Text = clsPmpaType.TBP.Sname;
                    sprList_Sheet1.Cells[0, 2].Text = clsPmpaType.TBP.Jumin1 + "-" + clsPmpaType.TBP.Jumin2;
                    sprList_Sheet1.Cells[0, 3].Text = clsPmpaType.TBP.PName;

                    nReadCnt = nReadCnt + 1;
                    strTblPtno[nReadCnt] = clsPmpaType.TBP.Ptno;
                }

                Dt.Dispose();
                Dt = null;


                if (chkJumin.Checked == false)
                {
                    nReadCnt = 0;
                    ComFunc.SetAllControlClear(this.sprList);

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + " SELECT PANO, SNAME, SEX, JUMIN1, JUMIN2, JUMIN3, PNAME";
                    SQL = SQL + ComNum.VBLF + "   FROM ADMIN.BAS_PATIENT";
                    SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                    SQL = SQL + ComNum.VBLF + "    AND SNAME      = '" + txtSname.Text.Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "    AND JUMIN1     = '" + txtJumin1.Text + "'";
                    SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                        return;
                    }

                    sprList_Sheet1.RowCount =  Dt.Rows.Count;
                    sprList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    if (Dt.Rows.Count > 0)
                    {
                        for (int i =0; i < Dt.Rows.Count; i++)
                        {
                            strPtno = Dt.Rows[i]["PANO"].ToString().Trim();
                            strSname = Dt.Rows[i]["SNAME"].ToString().Trim().PadRight(10,' ').Substring(0,10);
                            strPname = Dt.Rows[i]["PNAME"].ToString().Trim().PadRight(10,' ').Substring(0, 10);
                            strJumin1  = Dt.Rows[i]["JUMIN1"].ToString().Trim();
                            strJumin2 =  clsAES.DeAES(Dt.Rows[i]["JUMIN3"].ToString().Trim());

                            sprList_Sheet1.Cells[i, 0].Text = strPtno;
                            sprList_Sheet1.Cells[i, 1].Text = strSname;
                            sprList_Sheet1.Cells[i, 2].Text = strJumin1 + "-" + strJumin2;
                            sprList_Sheet1.Cells[i, 3].Text = strPname;

                            nReadCnt = nReadCnt + 1;
                            strTblPtno[nReadCnt] = strPtno;
                        }
                    }

                    Dt.Dispose();
                    Dt = null;
                }

                Cursor.Current = Cursors.Default;

                if (nReadCnt ==0)
                    clsPmpaPb.GstrJuminFlag = "NO";
                else
                    clsPmpaPb.GstrJuminFlag = "OK";

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }


        private void Read_Etc_Ptno()
        {

            nReadCnt = 0;
            clsPmpaPb.GstrJuminFlag = "NO";

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ComFunc.SetAllControlClear(this.sprList);

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT PANO, SNAME, SEX, JUMIN1, JUMIN2, JUMIN3";
                SQL = SQL + ComNum.VBLF + "   FROM ADMIN.OCS_ETCPANO";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "    AND SNAME      = '" + txtSname.Text.Trim() + "'";
                SQL = SQL + ComNum.VBLF + "    AND JUMIN1     = '" + txtJumin1.Text + "'";
                SQL = SQL + ComNum.VBLF + "    AND JUMIN3     = '" + clsAES.AES(txtJumin2.Text) + "'";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);       //에러로그 저장
                    return;
                }

                sprList_Sheet1.RowCount = Dt.Rows.Count;
                sprList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                if (Dt.Rows.Count > 0)
                {
                    clsPmpaPb.GstrJuminFlag = "OK";

                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        clsPmpaType.TBP.Ptno = Dt.Rows[i]["PANO"].ToString().Trim();
                        clsPmpaType.TBP.Sname = Dt.Rows[i]["SNAME"].ToString().Trim().PadRight(10, ' ').Substring(0, 10);
                        clsPmpaType.TBP.Jumin1 = Dt.Rows[i]["JUMIN1"].ToString().Trim();
                        clsPmpaType.TBP.Jumin2 = clsAES.DeAES(Dt.Rows[i]["JUMIN3"].ToString().Trim());
                        clsPmpaType.TBP.Sex = Dt.Rows[i]["SEX"].ToString().Trim();

                        sprList_Sheet1.Cells[i, 0].Text = clsPmpaType.TBP.Ptno;
                        sprList_Sheet1.Cells[i, 1].Text = clsPmpaType.TBP.Sname;
                        sprList_Sheet1.Cells[i, 2].Text = clsPmpaType.TBP.Jumin1 + "-" + clsPmpaType.TBP.Jumin2;

                        nReadCnt = nReadCnt + 1;
                        strTblPtno[nReadCnt] = clsPmpaType.TBP.Ptno;
                    }
                }

                Dt.Dispose();
                Dt = null;

                Cursor.Current = Cursors.Default;

                if (nReadCnt == 0)
                    clsPmpaPb.GstrJuminFlag = "NO";
                else
                    clsPmpaPb.GstrJuminFlag = "OK";
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void Jumin_No_Check()
        {
            int nCheckDigit =0; 
            int nCheckTotal =0;
            int nCheckCount =0;
            int j = 0;

            if (txtJumin1.Text.Length < 6 || txtJumin2.Text.Length < 7)
            {
                strFlag = "NO";
                btnNhic.Enabled = false;
                return;
            }


            strFlag = "OK";
            btnNhic.Enabled = true;

            //신생아제외
            if (txtJumin2.Text.Trim().Substring(1) == "000000") return;
            
            for (int i = 1; i <= 12; i++)
            {
                j = i + 1;
                if (j > 9) j = j - 8;

                switch (i)
                {
                    case 1:
                        nCheckDigit = Convert.ToInt32(VB.Val(txtJumin1.Text.Trim().Substring(0, 1))) * j;
                        break;
                    case 2:
                        nCheckDigit = Convert.ToInt32(VB.Val(txtJumin1.Text.Trim().Substring(1, 1))) * j;
                        break;
                    case 3:
                        nCheckDigit = Convert.ToInt32(VB.Val(txtJumin1.Text.Trim().Substring(2, 1))) * j;
                        break;
                    case 4:
                        nCheckDigit = Convert.ToInt32(VB.Val(txtJumin1.Text.Trim().Substring(3, 1))) * j;
                        break;
                    case 5:
                        nCheckDigit = Convert.ToInt32(VB.Val(txtJumin1.Text.Trim().Substring(4, 1))) * j;
                        break;
                    case 6:
                        nCheckDigit = Convert.ToInt32(VB.Val(txtJumin1.Text.Trim().Substring(5, 1))) * j;
                        break;
                    default:
                        nCheckDigit = Convert.ToInt32(VB.Val(txtJumin2.Text.Trim().Substring(i - 7, 1))) * j;
                        break;
                }

                nCheckTotal = nCheckTotal + nCheckDigit;
            }

            nCheckDigit = (int)nCheckTotal / 11;
            nCheckDigit = (int)nCheckDigit * 11;

            nCheckCount = nCheckTotal - nCheckDigit;

            if (nCheckCount == 0)
            {
                nCheckDigit = 1;
            }
            else if (nCheckCount == 1)
            {
                nCheckDigit = 0;
            }
            else
            {
                nCheckDigit = 11 - nCheckCount;
            }

            if (nCheckTotal < 20 || nCheckDigit != VB.Val(txtJumin2.Text.Substring(6, 1)))
            {
                strFlag = "NO";
                btnNhic.Enabled = false;
            }

            return;
        }
        
        private void sprList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                sprListDoubleClick(sprList_Sheet1.ActiveRowIndex);
            }

        }

        private void sprList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (sprList_Sheet1.Rows.Count == 0)
            {
                return;
            }

            sprListDoubleClick(e.Row);
        }

        private void sprListDoubleClick(int intRow)
        {
            clsPmpaPb.GstrJuminFlag = "GU";

            clsPublic.GstrPtno = sprList_Sheet1.Cells[intRow, 0].Text;
            this.Close();
        }

        private void btnNhic_Click(object sender, EventArgs e)
        {
            //주민번호 검증이 "OK"
            if (strFlag == "OK")
            {
                if (txtJumin1.Text == "") { lblMsg.Text = "주민번호 앞자리가 공란입니다."; txtJumin1.Focus(); return; }
                if (txtJumin2.Text == "") { lblMsg.Text = "주민번호 뒷자리가 공란입니다."; txtJumin2.Focus(); return; }
                if (txtSname.Text == "") { lblMsg.Text = "성명란이 공란입니다."; txtSname.Focus(); return; }

                clsPublic.GstrHelpCode = "00000000" + ",ME," + txtSname.Text.Trim();
                clsPublic.GstrHelpCode += txtJumin1.Text.Trim() + txtJumin2.Text.Trim() + "," + clsPublic.GstrSysDate;

                frmPmpaCheckNhic frm = new frmPmpaCheckNhic("00000000","ME",txtSname.Text, txtJumin1.Text, txtJumin2.Text, clsPublic.GstrSysDate,"");
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(10, 10);
                frm.ShowDialog();
                OF.fn_ClearMemory(frm);

                clsPublic.GstrHelpCode = "";
                btnSave.Enabled = true;
            }
        }

        private void txtJumin1_TextChanged(object sender, EventArgs e)
        {
            if (txtJumin1.Text.Length == 6)
            {
                txtJumin2.Focus();
            }
            if (txtJumin1.Text.Trim().Length == 6)
            {
                Read_BabyPtno();
            }
        }
    }
}
