using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmTACostView
    /// File Name : frmTACostView.cs
    /// Title or Description : 자보 비용산정 조회
    /// Author : 박창욱
    /// Create Date : 2017-06-01
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    /// </summary>
    /// <history>  
    /// VB\BuSuga28.frm(FrmTABiYongView) -> frmTACostView.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\busuga\BuSuga28.frm(FrmTABiYong)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\busuga\\busuga.vbp
    /// </vbp>
    public partial class frmTACostView : Form
    {
        private string gstrHelpCode = "";
        
        public frmTACostView()
        {
            InitializeComponent();
        }

        public frmTACostView(string strHelpCode)
        {
            InitializeComponent();
            gstrHelpCode = strHelpCode;
        }

        private void SCREEN_CLEAR()
        {
            txtData.Enabled = true;
            txtData.Text = "";
            btnCancel.Enabled = false;

            txtName.Text = "";
            txtSpec.Text = "";
            txtUnit.Text = "";
            txtManufacture.Text = "";
            txtGrouping.Text = "";

            ssView_Sheet1.RowCount = 29;
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount-1, ssView_Sheet1.ColumnCount-1].Text = "";
            ssView.Enabled = false;
            btnSave.Enabled = false;
        }

        private void Guip_Data_Display()
        {
            int i = 0;
            int nRead = 0;

            txtData.Text = txtData.Text.ToString().ToUpper().Trim();
            if (txtData.Text == "")
            {
                return;
            }

            READ_EDI_SUGA((txtData.Text));

            if (clsType.TES.PName == "")
            {
                ComFunc.MsgBox(txtData.Text + "가 표준코드에 등록 안 됨");
                txtData.Text = "";
                return;
            }

            txtName.Text = " " + clsType.TES.PName;
            txtSpec.Text = " " + clsType.TES.Spec.Trim() + " " + clsType.TES.Effect.Trim();
            txtUnit.Text = clsType.TES.Danwi1.Trim() + clsType.TES.Danwi2.Trim();
            txtManufacture.Text = " " + clsType.TES.COMPNY.Trim();
            txtGrouping.Text = clsType.TES.Bun.Trim();

            txtData.Enabled = false;
            ssView.Enabled = true;
            btnCancel.Enabled = true;

            //해당 자료를 SELECT
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = "     SELECT TO_CHAR(a.JDate,'YYYY-MM-DD') JDate,a.Gubun,a.JIN, a.ROWID, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(a.SDate,'YYYY-MM-DD') SDate,";
                SQL = SQL + ComNum.VBLF + "      a.BUNNAME, A.AMT, ADDFILE, REMARK ";
                SQL = SQL + ComNum.VBLF + " FROM EDI_TABIYONG  a";
                SQL = SQL + ComNum.VBLF + " WHERE a.Bcode = '" + txtData.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.JDate DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    txtData.Enabled = true;
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    txtData.Enabled = true;
                    return;
                }

                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = nRead;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < nRead - 1; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["JDate"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["sDate"].ToString().Trim();
                    switch (dt.Rows[i]["Gubun"].ToString().Trim())
                    {
                        case "A":
                            ssView_Sheet1.Cells[i, 2].Text = "A. 수가";
                            break;
                        case "B":
                            ssView_Sheet1.Cells[i, 2].Text = "B. 신의료(행위)";
                            break;
                        case "C":
                            ssView_Sheet1.Cells[i, 2].Text = "C. 신의료(치료재료)";
                            break;
                    }
                    ssView_Sheet1.Cells[i, 3].Text = VB.IIf(dt.Rows[i]["JIN"].ToString().Trim() == "1", "1.의과", "2.치과").ToString();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["BUNNAME"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = VB.Format(VB.Val(dt.Rows[i]["Amt"].ToString().Trim()), "###,###,###,##0 ");
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["ADDFILE"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
            btnSave.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            txtData.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //VB에서 주석처리되어 있음
            //    Dim i As Integer
            //    Dim strROWID As String
            //    Dim nQty As Double
            //    Dim nAmt As Double
            //    Dim nPrice As Double
            //
            //    Dim nQty_old As Double
            //    Dim nAmt_old As Double
            //    Dim nPrice_old As Double
            //
            //
            //
            //    For i = 1 To SS1.DataRowCnt
            //       SS1.Row = i
            //
            //       SS1.Col = 3: nQty = Val(SS1.Text)
            //       SS1.Col = 4: nAmt = Val(SS1.Text)
            //       SS1.Col = 5: nPrice = Val(SS1.Text)
            //       SS1.Col = 9: nQty_old = Val(SS1.Text)
            //       SS1.Col = 10: nAmt_old = Val(SS1.Text)
            //       SS1.Col = 11: nPrice_old = Val(SS1.Text)
            //       SS1.Col = 12: strROWID = SS1.Text
            //
            //       If nQty <> nQty_old Or nAmt <> nAmt_old Or nPrice <> nPrice_old Then
            //
            //
            //            If OptGu(0).Value = True Then
            //               SQL = " UPDATE ADMIN.EDI_GUIP SET  "
            //            ElseIf OptGu(1).Value = True Then
            //               SQL = " UPDATE KOSMOS_PMAP.EDI_SANGUIP SET "
            //            Else
            //               SQL = " UPDATE KOSMOS_PMAP.EDI_TAGUIP SET "
            //            End If
            //
            //            SQL = SQL & "QTY = '" & nQty & "', "
            //            SQL = SQL & " AMT = '" & nAmt & "' ,"
            //            SQL = SQL & " PRICE = '" & nPrice & "' "
            //            SQL = SQL & " WHERE ROWID= '" & strROWID & "' "
            //
            //            Result = AdoExecute(SQL)
            //       End If
            //
            //    Next i
            //
            //
            //    Call TxtData_KeyPress(13)
        }

        private void frmTACostView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SCREEN_CLEAR();
            if(gstrHelpCode != "")
            {
                txtData.Text = gstrHelpCode;
                Guip_Data_Display();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            //FrmBCodeHelp.Show 1           //------- VB
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            Cursor.Current = Cursors.WaitCursor;

            //Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"16\"";
            strFont2 = "/fn\"굴림체\" /fz\"11\"";

            strHead1 = "/c/f1" + " 비용산정 내역 " + "/n";
            strHead2 = "/l/f2" + "표준코드 : " + txtData.Text + "/n";
            strHead2 = strHead2 + "품    명 : " + txtName.Text.Trim() + "/n";
            //strHead2 = strHead2 + "규    격 : " + txtName.Text.Trim() + "/n";
            //strHead2 = strHead2 + "단    위 : " + txtUnit.Text.Trim() + "/n";
            //strHead2 = strHead2 + "제    조 : " + txtManufacture.Text.Trim() + "/n";
            //strHead2 = strHead2 + "분    류 : " + txtGrouping.Text.Trim() + "/n";
            //strHead2 = strHead2 + VB.Space(50) + "PAGE : /p"

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Margin.Top = 5;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);

            Cursor.Current = Cursors.Default;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            Guip_Data_Display();
        }

        private void txtData_KeyDown(object sender, KeyEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            Guip_Data_Display();
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
                SQL = SQL + " FROM ADMIN.EDI_SUGA ";
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
                    clsType.TES.ROWID = ""; clsType.TES.Code = ""; clsType.TES.Jong = "";
                    clsType.TES.PName = ""; clsType.TES.Bun = ""; clsType.TES.Danwi1 = "";
                    clsType.TES.Danwi2 = ""; clsType.TES.Spec = ""; clsType.TES.COMPNY = "";
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
    }
}
