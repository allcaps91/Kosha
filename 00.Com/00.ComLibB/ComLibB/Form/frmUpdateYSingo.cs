using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary> 약품 분기별 실구입가 신고분 수가에 Update </summary>
    public partial class frmUpdateYSingo : Form
    {
        /// <summary> 약품 분기별 실구입가 신고분 수가에 Update </summary>
        public frmUpdateYSingo()
        {
            InitializeComponent();
        }

        void frmUpdateYSingo_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            int i = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssView_Sheet1.ColumnHeader.Columns[26].Visible = false; //변경
            ssView_Sheet1.ColumnHeader.Columns[27].Visible = false; //보험
            ssView_Sheet1.ColumnHeader.Columns[28].Visible = false; //자보
            ssView_Sheet1.ColumnHeader.Columns[29].Visible = false; //일반

            ScreenClear();

            cboBungi.Items.Clear();

            try
            {
                //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                //{
                //    return; //권한 확인
                //}

                //약품실구입 신고내역 READ
                SQL = "";
                SQL = "SELECT BUNGI,TO_CHAR(GDate,'YYYY-MM-DD') GDate ";
                SQL = SQL + ComNum.VBLF + " FROM EDI_YAKGUIP ";
                SQL = SQL + ComNum.VBLF + "WHERE GDate >= TRUNC(SYSDATE-500)";
                SQL = SQL + ComNum.VBLF + "  AND (Gbn IS NULL OR Gbn = ' ') ";
                SQL = SQL + ComNum.VBLF + "  AND SeqNo = 1 ";

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

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboBungi.Items.Add(dt.Rows[i]["Bungi"].ToString().Trim());
                }

                cboBungi.SelectedIndex = 0;
                dtpSuDate.Text = dt.Rows[0]["GDate"].ToString().Trim();

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return;
            }

            ScreenClear();
        }

        void ScreenClear()
        {
            ssView_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 50;
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;
            btnExit.Enabled = true;
            ssView.Enabled = false;
        }

        //TODO: TES.Price1
        void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;

            string strBCode = "";
            int intAvgAmt = 0;

            string strSuCode = "";
            string strSuNext = "";
            string strBun = "";
            string strGbn = "";
            string strSugbE = "";
            string strSugbF = "";
            string strSugbM = "";
            string strSuDate = "";
            int intGesu = 0;
            int intBAmt = 0;
            int intTAmt = 0;
            int intIAmt = 0;
            int intNewBAmt = 0;
            int intNewTAmt = 0;
            int intNewIAmt = 0;
            int intChaAmt = 0;

            int intRow = 0;
            string strData = "";
            string strOK = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            if (cboBungi.Text == "" || dtpSuDate.Text.Trim() == "")
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "     SELECT SuNext,BCode,Suham,SuCode,Gbn,Bun,SugbE,SugbF,SugbM,";
                SQL = SQL + ComNum.VBLF + "      SuNameK,BCode,IAmt,TAmt,BAmt,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(SuDate,'YYYY-MM-DD')  SuDate,OldIAmt,OldTAmt,OldBAmt,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(SuDate3,'YYYY-MM-DD') SuDate3,IAmt3,TAmt3,BAmt3,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(SuDate4,'YYYY-MM-DD') SuDate4,IAmt4,TAmt4,BAmt4,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(SuDate5,'YYYY-MM-DD') SuDate5,IAmt5,TAmt5,BAmt5 ";
                SQL = SQL + ComNum.VBLF + " FROM VIEW_SUGA_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE BCode IN (SELECT BCode FROM EDI_YAKGUIP ";
                SQL = SQL + ComNum.VBLF + "      WHERE Bungi='" + cboBungi.Text + "' ";
                SQL = SQL + ComNum.VBLF + "        AND (Gbn IS NULL OR Gbn=' ')) ";
                SQL = SQL + ComNum.VBLF + "  AND DelDate IS NULL "; //삭제된것 제외
                SQL = SQL + ComNum.VBLF + "  AND BAmt <> 0 ";

                if (rdoJob0.Checked == true) //변경완료분 제외
                {
                    SQL = SQL + " AND (SuDate IS NULL OR SuDate < TO_DATE('" + dtpSuDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')) ";
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY SuCode,SuNext,Gbn ";

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

                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView.Enabled = true;
                intRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBCode = dt.Rows[i]["BCode"].ToString().Trim();

                    //약품 실구입 신고내역을 READ
                    SQL = "";
                    SQL = "SELECT TotQty,TotAmt,AvgAmt ";
                    SQL = SQL + ComNum.VBLF + " FROM EDI_YAKGUIP ";
                    SQL = SQL + ComNum.VBLF + "WHERE Bungi='" + cboBungi.Text + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND BCode = '" + strBCode + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND (Gbn IS NULL OR Gbn=' ') ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt1.Rows.Count == 0)
                    {
                        dt1.Dispose();
                        dt1 = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        return;
                    }

                    intAvgAmt = 0;

                    if (dt.Rows.Count > 0)
                    {
                        intAvgAmt = Convert.ToInt32(dt1.Rows[0]["AvgAmt"].ToString().Trim());
                    }

                    dt1.Dispose();
                    dt1 = null;


                    //실구입가 신고가격이 표준수가보다 크면 신고가격을 표준수가로 변경
                    //READ_EDI_SUGA(strBCode);  TODO: READ_EDI_SUGA
                    clsQuery.READ_EDI_SUGA(clsDB.DbCon, strBCode);
                    //TODO: TES.Price1
                    //변경일자가 적용일자보다 크면 수가2를 수가1로 Move
                    if(Convert.ToDateTime(clsType.TES.JDate1) > dtpSuDate.Value)
                    {
                        clsType.TES.Price1 = clsType.TES.Price2;
                    }

                    if (intAvgAmt > clsType.TES.Price1)
                    {
                        intAvgAmt =(int) clsType.TES.Price1;
                    }

                    strSuCode = dt.Rows[i]["SuCode"].ToString().Trim();
                    strSuNext = dt.Rows[i]["SuNext"].ToString().Trim();
                    strGbn = dt.Rows[i]["Gbn"].ToString().Trim();
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    strSugbE = dt.Rows[i]["SugbE"].ToString().Trim();
                    strSugbF = dt.Rows[i]["SugbF"].ToString().Trim();
                    strSugbM = dt.Rows[i]["SugbM"].ToString().Trim();
                    intGesu = (int)VB.Val(dt.Rows[i]["Suham"].ToString().Trim());
                    intBAmt = Convert.ToInt32(dt.Rows[i]["BAmt"].ToString().Trim());
                    intTAmt = Convert.ToInt32(dt.Rows[i]["TAmt"].ToString().Trim());
                    intIAmt = Convert.ToInt32(dt.Rows[i]["IAmt"].ToString().Trim());
                    strSuDate = dt.Rows[i]["SuDate"].ToString().Trim();

                    if (intGesu == 0)
                        intGesu = 1;

                    if (intGesu > 0)
                        intNewBAmt = intAvgAmt * intGesu;
                    else
                        intNewBAmt = intAvgAmt / intGesu;

                    //퇴장방지의약품 10%가산
                    if (strSugbM == "1")
                        intNewBAmt = VB.Fix(intNewBAmt * (110 / 100));

                    intChaAmt = intNewBAmt - intBAmt;

                    if (intChaAmt < 0)
                        intChaAmt = intChaAmt * (-1);

                    intNewTAmt = intNewBAmt;
                    intNewIAmt = Gesan_IlbanAmt(intNewBAmt, strBun, strSugbE, strSugbF);  

                    strOK = "OK";

                    //변경완료 제외이면
                    if (rdoJob0.Checked == true && intNewBAmt == intBAmt)
                        strOK = "NO";

                    if (strOK == "OK")
                    {
                        if (intRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = intRow;
                        }

                        //종전수가와 단가차이가 20%이상 차이시 자동으로 등록제외 Check
                        intChaAmt = intNewBAmt - intBAmt;

                        if (intChaAmt < 0)
                        {
                            intChaAmt = intChaAmt * (-1);
                        }

                        if (intChaAmt != 0 && intBAmt != 0)
                        {
                            if ((intChaAmt / intBAmt) >= 0.2)
                            {
                                ssView_Sheet1.Cells[intRow, 0].Text = "1";
                                ssView_Sheet1.RowHeader.Rows[intRow].BackColor = Color.FromArgb(255, 255, 128);
                            }
                        }

                        //임의단가 적용수가 Check
                        switch (strBCode)
                        {
                            case "A33292421":
                            case "A33292471":
                            case "A33292451":
                            case "A33292452":
                                ssView_Sheet1.Cells[intRow, 0].Text = "1";
                                ssView_Sheet1.RowHeader.Rows[intRow].BackColor = Color.FromArgb(255, 255, 128);
                                break;
                            case "A02106511":
                            case "A02106501":
                                ssView_Sheet1.Cells[intRow, 0].Text = "1";
                                ssView_Sheet1.RowHeader.Rows[intRow].BackColor = Color.FromArgb(255, 255, 128);
                                break;
                        }

                        ssView_Sheet1.Cells[intRow, 1].Text = strSuCode;
                        ssView_Sheet1.Cells[intRow, 2].Text = strSuNext;
                        ssView_Sheet1.Cells[intRow, 3].Text = strBun;
                        ssView_Sheet1.Cells[intRow, 4].Text = intNewBAmt.ToString();
                        ssView_Sheet1.Cells[intRow, 5].Text = intNewTAmt.ToString();
                        ssView_Sheet1.Cells[intRow, 6].Text = intNewIAmt.ToString();
                        ssView_Sheet1.Cells[intRow, 7].Text = dt.Rows[i]["SuNameK"].ToString().Trim();

                        //변경일자가 수가변경일보다 크거나 같으면
                        if (Convert.ToDateTime(strSuDate) >= dtpSuDate.Value)
                        {
                            ssView_Sheet1.Cells[intRow, 8].Text = dt.Rows[i]["SuDate"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 9].Text = dt.Rows[i]["OldBAmt"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 10].Text = dt.Rows[i]["OldTAmt"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 11].Text = dt.Rows[i]["OldIAmt"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 12].Text = strSugbE + strSugbF + strSugbM;
                            ssView_Sheet1.Cells[intRow, 13].Text = strGbn;
                            ssView_Sheet1.Cells[intRow, 14].Text = dt.Rows[i]["SuDate3"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 15].Text = dt.Rows[i]["BAmt3"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 16].Text = dt.Rows[i]["TAmt3"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 17].Text = dt.Rows[i]["IAmt3"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 18].Text = dt.Rows[i]["SuDate4"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 19].Text = dt.Rows[i]["BAmt4"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 20].Text = dt.Rows[i]["TAmt4"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 21].Text = dt.Rows[i]["IAmt4"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 22].Text = dt.Rows[i]["SuDate5"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 23].Text = dt.Rows[i]["BAmt5"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 24].Text = dt.Rows[i]["TAmt5"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 25].Text = dt.Rows[i]["IAmt5"].ToString().Trim();
                        }
                        else
                        {
                            ssView_Sheet1.Cells[intRow, 8].Text = dtpSuDate.Text;
                            ssView_Sheet1.Cells[intRow, 9].Text = dt.Rows[i]["BAmt"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 10].Text = dt.Rows[i]["TAmt"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 11].Text = dt.Rows[i]["IAmt"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 12].Text = strSugbE + strSugbF + strSugbM;
                            ssView_Sheet1.Cells[intRow, 13].Text = strGbn;
                            ssView_Sheet1.Cells[intRow, 14].Text = dt.Rows[i]["SuDate"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 15].Text = dt.Rows[i]["OldBAmt"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 16].Text = dt.Rows[i]["OldTAmt"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 17].Text = dt.Rows[i]["OldIAmt"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 18].Text = dt.Rows[i]["SuDate3"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 19].Text = dt.Rows[i]["BAmt3"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 20].Text = dt.Rows[i]["TAmt3"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 21].Text = dt.Rows[i]["IAmt3"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 22].Text = dt.Rows[i]["SuDate4"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 23].Text = dt.Rows[i]["BAmt4"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 24].Text = dt.Rows[i]["TAmt4"].ToString().Trim();
                            ssView_Sheet1.Cells[intRow, 25].Text = dt.Rows[i]["IAmt4"].ToString().Trim();
                        }

                        ssView_Sheet1.Cells[intRow, 26].Text = dt.Rows[i]["SuDate"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, 27].Text = dt.Rows[i]["BAmt"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, 28].Text = dt.Rows[i]["TAmt"].ToString().Trim();
                        ssView_Sheet1.Cells[intRow, 29].Text = dt.Rows[i]["IAmt"].ToString().Trim();

                        intRow += 1;
                    }
                }

                ssView_Sheet1.RowCount = intRow;
                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = false;

                if (rdoJob0.Checked == true)
                {
                    btnSave.Enabled = true;
                }

                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
                btnExit.Enabled = false;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return;
            }
        }

        private int Gesan_IlbanAmt(int ArgBAmt, string argBun, string ArgSugbE, string ArgSugbF, string ArgSugbJ = "")
        {
            int nIAmt = 0;

            //'진찰료,입원료는 보험가,일반가 동일하게 처리함
            if (Convert.ToInt32(VB.Val(argBun)) <= Convert.ToInt32(VB.Val("10")))
            {
                return ArgBAmt;
            }


            //'비급여수가(식대(74)-종합건진(84)는 보험가,일반가 동일하게 처리함
            if (Convert.ToInt32(VB.Val(argBun)) >= Convert.ToInt32(VB.Val("74")))
            {
                return ArgBAmt;
            }


            //'내복약,외용약품의 일반가 계산
            if ((argBun == "11" || argBun == "12") && ArgSugbE == "0")
            {
                nIAmt = Gesan_IlbanAmt_YAK(ArgBAmt, ArgSugbF);
            }
            //'주사약 일반가 계산
            else if (argBun == "20" && ArgSugbE == "0")
            {
                nIAmt = Gesan_IlbanAmt_JUSA(ArgBAmt, ArgSugbF);
            }
            //'기타 일반수가를 계산
            else
            {
                nIAmt = Gesan_IlbanAmt_ETC(ArgBAmt, ArgSugbE, ArgSugbJ);
            }

            return nIAmt;
        }

        private int Gesan_IlbanAmt_YAK(int ArgBAmt, string ArgSugbF)   //'내복약,외용약품의 일반가 계산
        {
            int nIAmt = 0;

            //'비급여수가 100,000원이상은 보험가,일반가 동일하게 처리함
            if (ArgSugbF == "1" && ArgBAmt >= 100000)
            {
                nIAmt = ArgBAmt;
                return nIAmt;
            }

            if (ArgBAmt < 11)
            {
                nIAmt = ArgBAmt * 5;
            }
            else if (ArgBAmt < 51)
            {
                nIAmt = ArgBAmt * 4;
            }
            else if (ArgBAmt < 101)
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 3.5);
            }
            else if (ArgBAmt < 500)
            {
                nIAmt = ArgBAmt * 3;
            }
            else if (ArgBAmt < 1001)
            {
                nIAmt = ArgBAmt * 2;
            }
            else
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 1.5);
            }
            return nIAmt;
        }



        private int Gesan_IlbanAmt_JUSA(int ArgBAmt, string ArgSugbF)   //'주사약제 일반수가 계산
        {
            int nIAmt = 0;

            //'비급여수가 100,000원이상은 보험가,일반가 동일하게 처리함
            if (ArgSugbF == "1" && ArgBAmt >= 100000)
            {
                nIAmt = ArgBAmt;
                return nIAmt;
            }

            if (ArgBAmt < 501)
            {
                nIAmt = ArgBAmt * 5;
            }
            else if (ArgBAmt < 1001)
            {
                nIAmt = ArgBAmt * 4;
            }
            else if (ArgBAmt < 3001)
            {
                nIAmt = ArgBAmt * 3;
            }
            else if (ArgBAmt < 5001)
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 2.5);
            }
            else if (ArgBAmt < 10001)
            {
                nIAmt = ArgBAmt * 2;
            }
            else
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 1.5);
            }
            return nIAmt;
        }



        private int Gesan_IlbanAmt_ETC(int ArgBAmt, string ArgSugbE, string ArgSugbJ)    //'기타수가 일반가 계산
        {
            int nIAmt = 0;

            //'행위료이면 보험수가 * 보험병원가산율 * 2
            if (ArgSugbE == "1") nIAmt = Convert.ToInt32((ArgBAmt * 1.25) * 2);
            //'재료대이면 보험수가의 2배
            if (ArgSugbE != "1") nIAmt = ArgBAmt * 2;
            //'10원보다 크면 10원미만 절사
            //'외부의뢰검사 는 절사 않함
            if (ArgSugbJ != "9" && ArgSugbJ != "8")
            {
                if (nIAmt > 10)
                {
                    nIAmt = nIAmt / 10;
                    nIAmt = nIAmt * 10;
                }
            }
            return nIAmt;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            int i = 0;
            string strCheck = "";
            string strSuCode = "";
            string strSuNext = "";
            string strGbn = "";
            int intBAmt = 0;
            int intTAmt = 0;
            int intIAmt = 0;
            string strSuDate = "";
            int intOldBAmt = 0;
            int intOldTAmt = 0;
            int intOldIAmt = 0;
            string strSuDate3 = "";
            int intBAmt3 = 0;
            int intTAmt3 = 0;
            int intIAmt3 = 0;
            string strSuDate4 = "";
            int intBAmt4 = 0;
            int intTAmt4 = 0;
            int intIAmt4 = 0;
            string strSuDate5 = "";
            int intBAmt5 = 0;
            int intTAmt5 = 0;
            int intIAmt5 = 0;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (rdoJob1.Checked == true)
            {
                ComFunc.MsgBox("변경완료분을 포함하여 자료조회를 하면 등록이 안 됨", "오류");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssView_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                {
                    strCheck = ssView_Sheet1.Cells[i, 0].Text;

                    if (strCheck != "1")
                    {

                        strSuCode = ssView_Sheet1.Cells[i, 1].Text.Trim();
                        strSuNext = ssView_Sheet1.Cells[i, 2].Text.Trim();
                        strGbn = ssView_Sheet1.Cells[i, 13].Text.Trim();
                        intBAmt = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 4].Text.Trim())));
                        intTAmt = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 5].Text.Trim())));
                        intIAmt = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 6].Text.Trim())));
                        strSuDate = ssView_Sheet1.Cells[i, 8].Text.Trim();
                        intOldBAmt = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 9].Text.Trim())));
                        intOldTAmt = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 10].Text.Trim())));
                        intOldIAmt = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 11].Text.Trim())));
                        strSuDate3 = ssView_Sheet1.Cells[i, 14].Text.Trim();
                        intBAmt3 = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 15].Text.Trim())));
                        intTAmt3 = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 16].Text.Trim())));
                        intIAmt3 = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 17].Text.Trim())));
                        strSuDate4 = ssView_Sheet1.Cells[i, 18].Text.Trim();
                        intBAmt4 = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 19].Text.Trim())));
                        intTAmt4 = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 20].Text.Trim())));
                        intIAmt4 = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 21].Text.Trim())));
                        strSuDate5 = ssView_Sheet1.Cells[i, 22].Text.Trim();
                        intBAmt5 = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 23].Text.Trim())));
                        intTAmt5 = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 24].Text.Trim())));
                        intIAmt5 = Convert.ToInt32(VB.Val(VB.Format(ssView_Sheet1.Cells[i, 25].Text.Trim())));

                        if (strGbn == "T")
                        {
                            SQL = "";
                            SQL = "UPDATE BAS_SUT SET BAmt=" + intBAmt + ",TAmt=" + intTAmt + ",IAmt=" + intIAmt + ",";
                            SQL = SQL + ComNum.VBLF + "SuDate=TO_DATE('" + strSuDate + "','YYYY-MM-DD'),OldBAmt=" + intOldBAmt + ",";
                            SQL = SQL + ComNum.VBLF + "OldTAmt=" + intOldTAmt + ",OldIAmt=" + intOldIAmt + ",";
                            SQL = SQL + ComNum.VBLF + "SuDate3=TO_DATE('" + strSuDate3 + "','YYYY-MM-DD'),BAmt3=" + intBAmt3 + ",";
                            SQL = SQL + ComNum.VBLF + "TAmt3=" + intTAmt3 + ",IAmt3=" + intIAmt3 + ",";
                            SQL = SQL + ComNum.VBLF + "SuDate4=TO_DATE('" + strSuDate4 + "','YYYY-MM-DD'),BAmt4=" + intBAmt4 + ",";
                            SQL = SQL + ComNum.VBLF + "TAmt4=" + intTAmt4 + ",IAmt4=" + intIAmt4 + ",";
                            SQL = SQL + ComNum.VBLF + "SuDate5=TO_DATE('" + strSuDate5 + "','YYYY-MM-DD'),BAmt5=" + intBAmt5 + ",";
                            SQL = SQL + ComNum.VBLF + "TAmt5=" + intTAmt5 + ",IAmt5=" + intIAmt5 + " ";
                            SQL = SQL + ComNum.VBLF + "WHERE SuCode = '" + strSuCode + "' ";
                        }
                        else
                        {
                            SQL = "";
                            SQL = "UPDATE BAS_SUH SET BAmt=" + intBAmt + ",TAmt=" + intTAmt + ",IAmt=" + intIAmt + ",";
                            SQL = SQL + ComNum.VBLF + "SuDate=TO_DATE('" + strSuDate + "','YYYY-MM-DD'),OldBAmt=" + intOldBAmt + ",";
                            SQL = SQL + ComNum.VBLF + "OldTAmt=" + intOldTAmt + ",OldIAmt=" + intOldIAmt + ",";
                            SQL = SQL + ComNum.VBLF + "SuDate3=TO_DATE('" + strSuDate3 + "','YYYY-MM-DD'),BAmt3=" + intBAmt3 + ",";
                            SQL = SQL + ComNum.VBLF + "TAmt3=" + intTAmt3 + ",IAmt3=" + intIAmt3 + ",";
                            SQL = SQL + ComNum.VBLF + "SuDate4=TO_DATE('" + strSuDate4 + "','YYYY-MM-DD'),BAmt4=" + intBAmt4 + ",";
                            SQL = SQL + ComNum.VBLF + "TAmt4=" + intTAmt4 + ",IAmt4=" + intIAmt4 + ",";
                            SQL = SQL + ComNum.VBLF + "SuDate5=TO_DATE('" + strSuDate5 + "','YYYY-MM-DD'),BAmt5=" + intBAmt5 + ",";
                            SQL = SQL + ComNum.VBLF + "TAmt5=" + intTAmt5 + ",IAmt5=" + intIAmt5 + " ";
                            SQL = SQL + ComNum.VBLF + "WHERE SuCode = '" + strSuCode + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND SuNext = '" + strSuNext + "' ";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                    }
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                //clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            ScreenClear();
            btnSearch.Focus();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            ScreenClear();
            btnSearch.Focus();
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            ssView_Sheet1.ColumnHeader.Columns[22].Visible = false; //변경4
            ssView_Sheet1.ColumnHeader.Columns[23].Visible = false; //보험4
            ssView_Sheet1.ColumnHeader.Columns[24].Visible = false; //자보4
            ssView_Sheet1.ColumnHeader.Columns[25].Visible = false; //일반4

            Cursor.Current = Cursors.WaitCursor;

            //Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"16\" /fs1";
            strHead1 = "/c/f1" + cboBungi.Text + "분기 약품실구입 수가에 Update" + "/n";

            strFont2 = "/fn\"굴림체\" /fz\"10\" /fs2";
            strHead2 = "/n" + "/l/f2" + "인쇄일자 : " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")) + " " + Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":"));
            strHead2 += "/r/f2" + "PAGE:" + "/p";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);

            Cursor.Current = Cursors.Default;

            ssView_Sheet1.ColumnHeader.Columns[22].Visible = true; //변경4
            ssView_Sheet1.ColumnHeader.Columns[23].Visible = true; //보험4
            ssView_Sheet1.ColumnHeader.Columns[24].Visible = true; //자보4
            ssView_Sheet1.ColumnHeader.Columns[25].Visible = true; //일반4
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void dtpSuDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void cboBungi_KeyDown(object sender, KeyEventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    //{
                    //    return; //권한 확인
                    //}

                    //수가적용일자를 READ
                    SQL = "";
                    SQL = "SELECT TO_CHAR(GDate,'YYYY-MM-DD') GDate ";
                    SQL = SQL + ComNum.VBLF + " FROM EDI_YAKGUIP ";
                    SQL = SQL + ComNum.VBLF + "WHERE Bungi = '" + cboBungi.Text.Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND (Gbn IS NULL OR Gbn = ' ') ";
                    SQL = SQL + ComNum.VBLF + "  AND SeqNo = 1 ";

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

                    dtpSuDate.Text = dt.Rows[0]["GDate"].ToString().Trim();

                    dt.Dispose();
                    dt = null;

                    SendKeys.Send("{TAB}");
                    return;
                }
                catch (Exception ex)
                {
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox(ex.Message);
                    return;
                }
            }
        }
    }
}
