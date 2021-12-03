using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 구입신고 내역 조회
/// Author : 김형범
/// Create Date : 2017.06.30
/// </summary>
/// <history>
/// 함수 및 struct 생성 필요
/// 부모폼에서 문자열 받는 생성자 추가 - 박성완 17-09-11
/// </history>
namespace ComLibB
{
    /// <summary> 구입신고 내역 조회 </summary>
    public partial class frmSearchGuip : Form
    {
        string GstrHelpCode = ""; //global
        
        /// <summary> 구입신고 내역 조회 </summary>
        public frmSearchGuip()
        {
            InitializeComponent();
        }

        public frmSearchGuip(string HelpCode)
        {
            InitializeComponent();
            GstrHelpCode = HelpCode;
        }
        

        void ScreenClear()
        {
            txtData.Text = "";
            btnCancel.Enabled = false;
            txtName.Text = "";
            txtSpac.Text = "";
            txtDanwi.Text = "";
            txtJejo.Text = "";
            txtBun.Text = "";
            ssView_Sheet1.RowCount = 0;
            btnSave.Enabled = false;
        }

        //TODO: READ_EDI_SUGA 모듈(BuSuga00)
        void GuipDataDisplay()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            txtData.Text = VB.UCase(txtData.Text.Trim());

            if (txtData.Text == "")
            {
                return;
            }

            //2곳엔 주석 1곳엔 주석X
            
            //'If TxtData.Text = "30050010" Then
            //'    Call READ_EDI_SUGA((TxtData.Text), "30050010", "8")
            //'Else

            if (rdoGu2.Checked == true)
            {
                READ_EDI_SUGA(txtData.Text);
            }
            else
            {
                READ_EDI_SUGA(txtData.Text, "", "8");
                if(clsType.TES.Jong != "4" && clsType.TES.Jong != "8") {
                    ComFunc.MsgBox ("구입신고는 수입, 원료약,일반재료만 가능함");
                    txtData.Text = "";
                    return;
                }
            }


            if(clsType.TES.PName == "")
            {
                ComFunc.MsgBox(txtData.Text + "가 표준코드에 등록 않됨");
                txtData.Text = "";
                return;
            }



            txtName.Text = "" + clsType.TES.PName;
            txtSpac.Text = " " + clsType.TES.Spec.Trim() + " " + clsType.TES.Effect.Trim();
            txtDanwi.Text = clsType.TES.Danwi1.Trim() + clsType.TES.Danwi2.Trim();
            txtJejo.Text = " " + clsType.TES.COMPNY.Trim();
            txtBun.Text = clsType.TES.Bun.Trim();

            ssView.Enabled = true;
            btnCancel.Enabled = true;

            try
            {
                //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                //{
                //    return; //권한 확인
                //}

                SQL = "";
                SQL = "     SELECT TO_CHAR(a.GDate,'YYYY-MM-DD') GDate,a.Gubun,a.SGbn, a.ROWID, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(a.SDate,'YYYY-MM-DD') SDate,";
                SQL = SQL + ComNum.VBLF + "      a.Qty,a.Amt,a.Price,a.Pano,b.Name ";

                if (rdoGu0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " FROM EDI_GUIP a," + ComNum.DB_ERP + "AIS_LTD b ";
                }
                else if (rdoGu1.Checked == true)
                {

                    SQL = SQL + ComNum.VBLF + " FROM EDI_SANGUIP a," + ComNum.DB_ERP + "AIS_LTD b ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM EDI_TAGUIP a," + ComNum.DB_ERP + "AIS_LTD b ";
                }

                SQL = SQL + ComNum.VBLF + "WHERE a.Bcode = '" + txtData.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND a.GelCode = b.LtdCode(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.GDate DESC ";

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
                    return;
                }

                ssView_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["Name"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 2].Text = VB.Format(VB.Val(dt.Rows[i]["Qty"].ToString().Trim()), "###,###,###,##0");
                    ssView_Sheet1.Cells[i, 3].Text = VB.Format(VB.Val(dt.Rows[i]["Amt"].ToString().Trim()), "###,###,###,##0");
                    ssView_Sheet1.Cells[i, 4].Text = VB.Format(VB.Val(dt.Rows[i]["Price"].ToString().Trim()), "###,###,###,##0");
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["SGbn"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Qty"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["Amt"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["Price"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }

            btnSave.Enabled = true;
        }

        private void READ_EDI_SUGA(string ArgCode, string argSUNEXT = "", string ArgJong = "")  //'EDI 표준수가를 READ
        {

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";
                SQL = SQL + ComNum.VBLF + "";

                SQL = "      SELECT ROWID VROWID,CODE VCODE,JONG VJONG,";
                SQL = SQL + "    PNAME VPNAME,BUN VBUN,DANWI1 VDANWI1,";
                SQL = SQL + "    DANWI2 VDANWI2,SPEC VSPEC,COMPNY VCOMPNY,";
                SQL = SQL + "    EFFECT VEFFECT,GUBUN VGUBUN,DANGN VDANGN,";
                SQL = SQL + "    TO_CHAR(JDATE1,'YYYY-MM-DD') VJDATE1,PRICE1 VPRICE1,";
                SQL = SQL + "    TO_CHAR(JDATE2,'YYYY-MM-DD') VJDATE2,PRICE2 VPRICE2,";
                SQL = SQL + "    TO_CHAR(JDATE3,'YYYY-MM-DD') VJDATE3,PRICE3 VPRICE3,";
                SQL = SQL + "    TO_CHAR(JDATE4,'YYYY-MM-DD') VJDATE4,PRICE4 VPRICE4,";
                SQL = SQL + "    TO_CHAR(JDATE5,'YYYY-MM-DD') VJDATE5,PRICE5 VPRICE5 ";
                SQL = SQL + " FROM KOSMOS_PMPA.EDI_SUGA ";
                SQL = SQL + "WHERE CODE = '" + VB.Trim(ArgCode) + "' ";
                //'표준코드 30050010이 산소,실구입재료 2개가 존재함

                if (ArgJong != "")
                {
                    if (ArgJong == "8")
                    {
                        SQL = SQL + " AND JONG='8' ";
                    }
                    else
                    {
                        SQL = SQL + " AND JONG<>'8' ";
                    }
                }
                else
                {
                    switch (ArgCode)
                    {
                        case "N0041001":
                        case "N0041002":
                        case "N0041003":
                        case "N0021001":
                        case "30050010":
                        case "J5010001":
                        case "C2302005":
                        case "N0051010":
                            if (argSUNEXT == VB.Trim(ArgCode))
                            {
                                SQL = SQL + " AND JONG='8' ";
                            }
                            else
                            {
                                SQL = SQL + " AND JONG<>'8' ";
                            }
                            break;
                        default:
                            break;
                    }
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    clsType.TES.ROWID = dt.Rows[0]["vROWID"].ToString().Trim();
                    clsType.TES.Code = dt.Rows[0]["vCode"].ToString().Trim();
                    clsType.TES.Jong = dt.Rows[0]["vJong"].ToString().Trim();
                    clsType.TES.PName = dt.Rows[0]["vPname"].ToString().Trim();
                    clsType.TES.Bun = dt.Rows[0]["vBun"].ToString().Trim();
                    clsType.TES.Danwi1 = dt.Rows[0]["vDanwi1"].ToString().Trim();
                    clsType.TES.Danwi2 = dt.Rows[0]["vDanwi2"].ToString().Trim();
                    clsType.TES.Spec = dt.Rows[0]["vSpec"].ToString().Trim();
                    clsType.TES.COMPNY = dt.Rows[0]["vCompny"].ToString().Trim();
                    clsType.TES.Effect = dt.Rows[0]["vEffect"].ToString().Trim();
                    clsType.TES.Gubun = dt.Rows[0]["vGubun"].ToString().Trim();
                    clsType.TES.Dangn = dt.Rows[0]["vDangn"].ToString().Trim();
                    clsType.TES.JDate1 = dt.Rows[0]["vJDate1"].ToString().Trim();
                    clsType.TES.Price1 = VB.Val(dt.Rows[0]["vPrice1"].ToString().Trim());
                    clsType.TES.JDate2 = dt.Rows[0]["vJDate2"].ToString().Trim();
                    clsType.TES.Price2 = VB.Val(dt.Rows[0]["vPrice2"].ToString().Trim());
                    clsType.TES.JDate3 = dt.Rows[0]["vJDate3"].ToString().Trim();
                    clsType.TES.Price3 = VB.Val(dt.Rows[0]["vPrice3"].ToString().Trim());
                    clsType.TES.JDate4 = dt.Rows[0]["vJDate4"].ToString().Trim();
                    clsType.TES.Price4 = VB.Val(dt.Rows[0]["vPrice4"].ToString().Trim());
                    clsType.TES.JDate5 = dt.Rows[0]["vJDate5"].ToString().Trim();
                    clsType.TES.Price5 = VB.Val(dt.Rows[0]["vPrice5"].ToString().Trim());
                }
                else
                {
                    clsType.TES.ROWID = "";  clsType.TES.Code = "";  clsType.TES.Jong = "";
                    clsType.TES.PName = "";  clsType.TES.Bun = "";   clsType.TES.Danwi1 = "";
                    clsType.TES.Danwi2 = ""; clsType.TES.Spec = "";  clsType.TES.COMPNY = "";
                    clsType.TES.Effect = ""; clsType.TES.Gubun = ""; clsType.TES.Dangn = "";
                    clsType.TES.JDate1 = ""; clsType.TES.Price1 = 0;
                    clsType.TES.JDate2 = ""; clsType.TES.Price2 = 0;
                    clsType.TES.JDate3 = ""; clsType.TES.Price3 = 0;
                    clsType.TES.JDate4 = ""; clsType.TES.Price4 = 0;
                    clsType.TES.JDate5 = ""; clsType.TES.Price5 = 0;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void btnHelp_Click(object sender, EventArgs e)
        {
            frmSearchBCode frm = new frmSearchBCode();
            frm.Show();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            string strROWID = "";
            int intQty = 0;
            int intAmt = 0;
            int intPrice = 0;

            int intQty_old = 0;
            int intAmt_old = 0;
            int intPrice_old = 0;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i <= ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    intQty = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 2].Text));
                    intAmt = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 3].Text));
                    intPrice = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 4].Text));
                    intQty_old = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 8].Text));
                    intAmt_old = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 9].Text));
                    intPrice_old = Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 10].Text));
                    strROWID = ssView_Sheet1.Cells[i, 11].Text;

                    if (intQty != intQty_old || intAmt != intAmt_old || intPrice != intPrice_old)
                    {

                        //if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                        //{
                        //    return; //권한 확인
                        //}

                        if (rdoGu0.Checked == true)
                        {
                            SQL = "";
                            SQL = " UPDATE " + ComNum.DB_PMPA + "EDI_GUIP SET  ";
                        }
                        else if (rdoGu1.Checked == true)
                        {
                            SQL = "";
                            SQL = " UPDATE " + ComNum.DB_PMPA + "EDI_SANGUIP SET ";
                        }
                        else
                        {
                            SQL = "";
                            SQL = " UPDATE " + ComNum.DB_PMPA + "EDI_TAGUIP SET ";
                        }

                        SQL = SQL + ComNum.VBLF + "QTY = '" + intQty + "', ";
                        SQL = SQL + ComNum.VBLF + " AMT = '" + intAmt + "' ,";
                        SQL = SQL + ComNum.VBLF + " PRICE = '" + intPrice + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE ROWID= '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                Cursor.Current = Cursors.Default;                
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            KeyEnter(Keys.Enter);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            ScreenClear();
            txtData.Text = "";
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strHead1 = "/c/f1" + " 구입신고 내역 " + "/n";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs2";
            strHead2 += "/l/f2" + "표준코드 : " + txtData.Text.Trim() + "/n";
            strHead2 += "/l/f2" + "규    격 : " + txtSpac.Text.Trim() + "/n";
            strHead2 += "/l/f2" + "단    위 : " + txtDanwi.Text.Trim() + "/n";
            strHead2 += "/l/f2" + "제    조 : " + txtJejo.Text.Trim() + "/n";
            strHead2 += "/l/f2" + "분    류 : " + txtBun.Text.Trim() + "/n";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);

            Cursor.Current = Cursors.Default;
        }

        void txtData_KeyDown(object sender, KeyEventArgs e)
        {
            KeyEnter(e.KeyCode);
        }

        void KeyEnter(Keys e)
        {
            if (e == Keys.Enter)
            {
                GuipDataDisplay();

                SendKeys.Send("{TAB}");
            }
        }

        private void frmSearchGuip_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ScreenClear();

            if (GstrHelpCode != "")
            {
                txtData.Text = GstrHelpCode;
                GuipDataDisplay();
            }
        }
    }
}
