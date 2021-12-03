using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread;

namespace ComEmrBase
{
    public partial class frmSugaOrderSave : Form
    {
        string FormNo = string.Empty;
        string ItemCd = string.Empty;
        string ItemNm = string.Empty;
        string ItemValue = string.Empty;
        string EmrNo = string.Empty;
        string ChartDate = string.Empty;
        string WardCode = string.Empty;

        int nRow = -1;

        EmrPatient pAcp = null;
        FpSpread ssWrite = null;

        public frmSugaOrderSave(string ChartDate, string FormNo, string ItemCd, string ItemValue, string ItemNm,  EmrPatient pAcps, string EmrNo, FpSpread ssWrite, int Row)
        {
            this.ChartDate = ChartDate;
            this.FormNo = FormNo;
            this.ItemCd = ItemCd;
            this.ItemNm = ItemNm;
            this.ItemValue = ItemValue;
            this.EmrNo = EmrNo;
            pAcp = pAcps;
            this.ssWrite = ssWrite;
            nRow = Row;
            InitializeComponent();
        }

        private void frmSugaOrderSave_Load(object sender, EventArgs e)
        {
            ss2_Sheet1.RowCount = 0;
            dtpBdate.Value = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();


            ORDER_SUBUL_DEPT(ss2, 12);
            ORDER_SUBUL_DEPT(ss3, 11);


            WardCode = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");
            if (FormNo.NotEmpty() && ItemCd.NotEmpty() && pAcp.NotEmpty())
            {
                SET_MST(WardCode, FormNo, ItemNm, ItemCd, ItemValue);
                SET_BAT(WardCode, FormNo, ItemNm, ItemCd, ItemValue, this.nRow);
            }

            if (EmrNo.To<double>() > 0)
            {
                btnSaveOrder.Visible = true;
                btnDeleteOrder.Visible = true;
                if (FormNo.Equals("3150") || FormNo.Equals("1573") || FormNo.Equals("1725") || FormNo.Equals("2638") || FormNo.Equals("2240"))
                {
                    GetOrderData(EmrNo);
                }
            }
        }

        public void SET_MST(string Ward, string FormNo, string ItemNm, string Itemcd, string ItemVal)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수

            ss3_Sheet1.RowCount = 0;
            this.ItemNm = ItemNm;
            //this.nRow = nRow;
            ItemCd = Itemcd;
            ItemValue = ItemVal;

            cboList.Items.Clear();

            try
            {
                //SQL = " SELECT ORDERNAME, QTY, ORDERCODE, SUCODE, GBINPUT, SLIPNO, BUN, SUBNAME, GBINFO, BUSE ";
                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "   (B.SEQNAME || RPAD(' ', 50, ' ') || B.SEQNO) AS SEQNAME";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRSUGAMAPPING A";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.NUR_CAREMST B";
                SQL = SQL + ComNum.VBLF + "       ON A.SEQNO = B.SEQNO";
                SQL = SQL + ComNum.VBLF + " WHERE A.FORMNO     = " + FormNo;
                SQL = SQL + ComNum.VBLF + "   AND A.ITEMCD     = '" + Itemcd + "'";
                if (FormNo.Equals("3150") && ItemVal.Equals("전체"))
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.ITEMVALUE IS NOT NULL";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.ITEMVALUE  = '" + ItemVal + "'";
                }
                SQL = SQL + ComNum.VBLF + "   AND A.WARD       = '" + Ward + "'";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.SEQNO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (cboList.Items.IndexOf(dt.Rows[i]["SEQNAME"].ToString().Trim()) == -1)
                        {
                            cboList.Items.Add(dt.Rows[i]["SEQNAME"].ToString().Trim());
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        public void SET_BAT(string Ward, string FormNo, string ItemNm, string Itemcd, string ItemVal, int nRow, int SeqNo = -1)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수

            ss3_Sheet1.RowCount = 0;
            this.ItemNm = ItemNm;
            this.nRow = nRow;
            ItemCd = Itemcd;
            ItemValue = ItemVal;

            try
            {
                //SQL = " SELECT ORDERNAME, QTY, ORDERCODE, SUCODE, GBINPUT, SLIPNO, BUN, SUBNAME, GBINFO, BUSE ";
                SQL = " SELECT ";
                SQL = SQL + ComNum.VBLF + "   ORDERNAME";
                SQL = SQL + ComNum.VBLF + " , QTY";
                SQL = SQL + ComNum.VBLF + " , ORDERCODE";
                SQL = SQL + ComNum.VBLF + " , SUCODE";
                SQL = SQL + ComNum.VBLF + " , GBINPUT";
                SQL = SQL + ComNum.VBLF + " , SLIPNO";
                SQL = SQL + ComNum.VBLF + " , BUN";
                SQL = SQL + ComNum.VBLF + " , SUBNAME";
                SQL = SQL + ComNum.VBLF + " , GBINFO";
                SQL = SQL + ComNum.VBLF + " , BUSE";
                SQL = SQL + ComNum.VBLF + " , (SELECT SEQNAME || RPAD(' ', 50, ' ') || SEQNO FROM KOSMOS_PMPA.NUR_CAREMST WHERE SEQNO = A.SEQNO AND ROWNUM = 1) AS SEQNAME";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRSUGAMAPPING A";
                SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.NUR_CARECODE B";
                SQL = SQL + ComNum.VBLF + "       ON A.SEQNO = B.SEQNO";
                SQL = SQL + ComNum.VBLF + " WHERE A.FORMNO     = " + FormNo;
                SQL = SQL + ComNum.VBLF + "   AND A.ITEMCD     = '" + Itemcd + "'";
                if (FormNo.Equals("3150") && ItemVal.Equals("전체"))
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.ITEMVALUE IS NOT NULL";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.ITEMVALUE  = '" + ItemVal + "'";
                }
                SQL = SQL + ComNum.VBLF + "   AND A.WARD       = '" + Ward + "'";
                if (SeqNo != -1)
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.SEQNO  = " + SeqNo;
                }
                //'2013-07-02 삭제처리된 오더코드 제외
                SQL = SQL + ComNum.VBLF + "   AND (B.ORDERCODE, B.SUCODE) NOT IN ( SELECT ORDERCODE,SUCODE FROM KOSMOS_OCS.OCS_ORDERCODE WHERE SENDDEPT='N' ) ";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.SEQNO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ss3_Sheet1.RowCount = dt.Rows.Count;
                    ss3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GBINPUT"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 6].Text = dt.Rows[i]["SLIPNO"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 7].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 8].Text = dt.Rows[i]["SUBNAME"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 9].Text = dt.Rows[i]["GBINFO"].ToString().Trim();
                        ss3_Sheet1.Cells[i, 10].Text = dt.Rows[i]["BUSE"].ToString().Trim();
                    }

                    ss3_Sheet1.ColumnHeader.Cells[0, 0].Text = "■";

                    ss3_Sheet1.Cells[0, 0, ss3_Sheet1.RowCount - 1, 0].Value = true;
                    ss3_Sheet1.Rows[0, ss3_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 128, 128);

                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        private void ORDER_SUBUL_DEPT(FpSpread spdNm, int nCol)
        {
            FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            string[] sComboList = new string[5];

            try
            {
                SQL = "";
                SQL += " SELECT NAME, CODE                      \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_BCODE           \r";
                SQL += "  WHERE GUBUN = 'OCS_불출부서_코드'       \r";
                SQL += "  ORDER BY SORT                         \r";
                
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    Array.Resize(ref sComboList, dt.Rows.Count);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sComboList[i] = dt.Rows[i]["NAME"].ToString().Trim() + "." + dt.Rows[i]["CODE"].ToString().Trim();
                    }

                    combo.Items = sComboList;
                    combo.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                    //combo.MaxDrop = dt.Rows.Count;
                    combo.MaxDrop = 20;
                    combo.MaxLength = 100;
                    combo.ListWidth = 200;
                    combo.Editable = false;

                    spdNm.ActiveSheet.Columns[nCol].CellType = combo;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            //'등록시점 재원상태체크
            string strGbSTS = "";
            string strSex = "";
            try
            {
                string strPano = pAcp.ptNo;
                string strSName = pAcp.ptName;

                SQL = " SELECT PANO, SEX ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER WHERE PANO ='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GBSTS IN ('0','2','3','4') ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return;
                }

                strGbSTS = "";
                if (dt.Rows.Count > 0)
                {
                    strGbSTS = "";
                    if (dt.Rows[0]["SEX"].ToString().Trim() == "M")
                    {
                        strSex = "M";
                    }
                }
                else
                {
                    strGbSTS = "N";
                }

                dt.Dispose();
                dt = null;

                if (strGbSTS == "N")
                {
                    ComFunc.MsgBoxEx(this, strPano + "[" + strSName + "]" + "  재원환자 아님!! 재원상태를 확인하세요!!");
                    return;
                }


                //'2014-11-11 주임 김현욱
                //'간호활동 오더 제한 부분
                //'현재 두가지 사용중인데 한가지가 더 추가되면 코드화 예정임.
                for (int i = 0; i < ss3.ActiveSheet.RowCount; i++)
                {
                    string strOrderName = ss3.ActiveSheet.Cells[i, 1].Text.Trim();
                    int nCnt = (int)VB.Val(ss3.ActiveSheet.Cells[i, 2].Text.Trim());
                    string strORDERCODE = ss3.ActiveSheet.Cells[i, 3].Text.Trim();
                    string strSucode = ss3.ActiveSheet.Cells[i, 4].Text.Trim();

                    if (ss3.ActiveSheet.Cells[i, 0].Text.Equals("True"))
                    {
                        switch (strORDERCODE)
                        {
                            case "M0151":
                                if (strSex == "M")
                                {
                                    ComFunc.MsgBoxEx(this, "회음부간호는 여자만 가능합니다." + ComNum.VBLF + ComNum.VBLF +
                                                   "선택하신 환자목록 중 남자가 포함되어 있습니다." + ComNum.VBLF + ComNum.VBLF +
                                                   "확인하시기 바랍니다.");
                                    return;
                                }
                                break;
                            case "M0040":
                                if (nCnt > 1)
                                {
                                    ComFunc.MsgBoxEx(this, "산소흡입 치료(수가코드 : M0040)는 1일당 1회만 가능합니다." + ComNum.VBLF + ComNum.VBLF +
                                                   "확인하시기 바랍니다.");
                                    return;
                                }
                                break;
                        }


                        //'입원1일처방한도 체크 2012-09-10
                        SQL = "SELECT  SUCODE, FIELDA FROM KOSMOS_PMPA.BAS_MSELF_I ";
                        SQL = SQL + ComNum.VBLF + "  WHERE SUCODE = '" + strSucode + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND GUBUNA ='8'";
                        SQL = SQL + ComNum.VBLF + "    AND GUBUNB ='5'";
                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            //'2013-08-05
                            if ((int)VB.Val(ss3.ActiveSheet.Cells[i, 2].Text) > (int)VB.Val(dt.Rows[0]["FIELDA"].ToString().Trim()))
                            {
                                ComFunc.MsgBoxEx(this, "▶" + strOrderName + "은(는) 1일허용갯수 " + (int)VB.Val(dt.Rows[0]["FIELDA"].ToString().Trim()) + "개 입니다!!");
                                dt.Dispose();
                                dt = null;
                                return;
                            }
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            int nCnt2 = 0;
            for(int i = 0; i < ss3_Sheet1.RowCount; i++)
            {
                if (ss3_Sheet1.Cells[i, 0].Text.Equals("True"))
                {
                    nCnt2 += 1;
                    ss2_Sheet1.RowCount += 1;
                    ss2_Sheet1.Cells[ss2_Sheet1.RowCount - 1, 0].Text = "True";
                    ss2_Sheet1.Cells[ss2_Sheet1.RowCount - 1, 1].Text = ItemNm;
                    ss2_Sheet1.Cells[ss2_Sheet1.RowCount - 1, 1].Tag = ItemCd;
                    ss2_Sheet1.Cells[ss2_Sheet1.RowCount - 1, 2].Tag = ItemValue;
                    ss2_Sheet1.Rows[ss2_Sheet1.RowCount - 1].BackColor = Color.FromArgb(255, 128, 128);
                    for (int j = 2; j < ss2_Sheet1.ColumnCount; j++)
                    {
                        ss2_Sheet1.Cells[ss2_Sheet1.RowCount - 1, j].Text = ss3_Sheet1.Cells[i, j - 1].Text;
                    }
                }
            }

            if (nCnt2 > 0)
            {
                if (nRow > -1)
                {
                    ssWrite.ActiveSheet.Rows[nRow].BackColor = Color.LightPink;
                }
            }
        }
       

        private void ss2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;

            if (ss3_Sheet1.RowCount == 0)
            {
                return;
            }


            if (e.ColumnHeader == true)
            {
                if (e.Column == 0)
                {
                    if (ss3_Sheet1.ColumnHeader.Cells[0, 0].Text == "□")
                    {
                        ss3_Sheet1.ColumnHeader.Cells[0, 0].Text = "■";
                    }
                    else
                    {
                        ss3_Sheet1.ColumnHeader.Cells[0, 0].Text = "□";
                    }

                    if (ss3_Sheet1.ColumnHeader.Cells[0, 0].Text == "■")
                    {
                        for (i = 0; i < ss3_Sheet1.NonEmptyRowCount; i++)
                        {
                            if (ss3_Sheet1.Cells[i, 0].CellType == null || ss3_Sheet1.Cells[i, 0].CellType.ToString() != "TextCellType")
                            {
                                ss3_Sheet1.Cells[i, 0].Value = true;
                                ss3_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 128, 128);
                            }
                        }
                    }
                    else
                    {
                        for (i = 0; i < ss3_Sheet1.NonEmptyRowCount; i++)
                        {
                            if (ss3_Sheet1.Cells[i, 0].CellType == null || ss3_Sheet1.Cells[i, 0].CellType.ToString() != "TextCellType")
                            {
                                ss3_Sheet1.Cells[i, 0].Value = false;
                                ss3_Sheet1.Rows[i].BackColor = Color.FromArgb(255, 220, 220);
                            }
                        }
                    }
                }
            }
            else
            {
                if (e.RowHeader == true || e.Column == 0)
                {
                    return;
                }

                if (e.Column == 11)
                {
                    return;
                }

                if (e.Column == 2)
                {
                    return;
                }

                if (ss3_Sheet1.Cells[e.Row, 5].Text == "0")
                {
                    return;
                }


                if (e.Column != 0)
                {
                    if (Convert.ToBoolean(ss3_Sheet1.Cells[e.Row, 0].Value) == true)
                    {
                        ss3_Sheet1.Cells[e.Row, 0].Value = false;
                        ss3_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(255, 220, 220);
                    }
                    else
                    {
                        ss3_Sheet1.Cells[e.Row, 0].Value = true;
                        ss3_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(255, 128, 128);
                    }
                }
            }
        }

        private void ss2_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column == 0)
            {
                if (ss3_Sheet1.Cells[e.Row, 0].Text.Equals("True"))
                {
                    ss3_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(255, 128, 128);
                }
                else
                {
                    ss3_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(255, 220, 220);
                }
            }
        }

        private void ss2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            string strORDERCODE = "";

            if (e.RowHeader == true || e.ColumnHeader == true)
                return;

            if (ss3_Sheet1.Cells[e.Row, 9].Text == "1")
            {
                strORDERCODE = ss3_Sheet1.Cells[e.Row, 3].Text;
                ss3_Sheet1.Rows[e.Row].Visible = false;

                for (i = e.Row; i < ss3_Sheet1.Rows.Count; i++)
                {
                    if (strORDERCODE != ss3_Sheet1.Cells[i, 3].Text)
                    {
                        break;
                    }
                    ss3_Sheet1.Rows[i].Visible = true;
                }
            }
        }

        private void ss2_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            if (e.Column == 2)
            {
                ss3_Sheet1.Cells[e.Row, 0].Value = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            
        }

        private void ss3_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            if (EmrNo.To<double>() == 0 || EmrNo.IsNullOrEmpty() || ss2_Sheet1.NonEmptyRowCount == 0)
                return;

            clsEmrQuery.SaveOrderData(this, ss2, pAcp, ChartDate, EmrNo.To<double>());
            ComFunc.MsgBoxEx(this, "처방전송 완료");
            if (FormNo.Equals("3150") || FormNo.Equals("1573") || FormNo.Equals("1725") || FormNo.Equals("2638") || FormNo.Equals("2240"))
            {
                Close();
            }
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (EmrNo.To<double>() == 0 || EmrNo.IsNullOrEmpty())
                return;

            if (ss2_Sheet1.NonEmptyRowCount > 0)
            {
                for (int i = 0; i < ss2_Sheet1.RowCount; i++)
                {
                    if (ss2_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                    {
                        string ItemCd = ss2_Sheet1.Cells[i, 1].Tag.ToString();
                        string ItemValue = ss2_Sheet1.Cells[i, 2].Tag.ToString();
                        if (ss2_Sheet1.Cells[i, 3].Tag != null)
                        {
                            string OrderNo = ss2_Sheet1.Cells[i, 3].Tag?.ToString();
                            clsEmrQuery.DeleteOrderData(this, EmrNo.To<double>(), ItemCd, ItemValue, OrderNo);
                        }
                        
                    }
                }

                GetOrderData(EmrNo);
            }
        }

        /// <summary>
        /// 첫번째 값 조회
        /// </summary>
        /// <param name="strChartDate"></param>
        public void GetOrderData(string EmrNo)
        {
            if (this.EmrNo.Equals(EmrNo) == false)
            {
                this.EmrNo = EmrNo;
            }

            if (EmrNo.To<double>() == 0 || EmrNo.IsNullOrEmpty())
                return;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT B.SLIPNO, B.PTNO, B.GBSTATUS, B.ROWID , B.ORDERSITE, B.ORDERCODE, B.QTY,  B.GBINFO, ";
            SQL = SQL + ComNum.VBLF + " B.NURSEID, B.GBSEND , B.ORDERNO,  ";
            SQL = SQL + ComNum.VBLF + " C.ORDERNAME, B.SUBUL_WARD   ";
            SQL = SQL + ComNum.VBLF + " , (SELECT BASNAME FROM KOSMOS_EMR.AEMRBASCD WHERE BSNSCLS = '기록지관리' AND UNITCLS IN ('임상관찰', '특수치료', '기본간호', '섭취배설', '간호활동항목') AND BASCD  = O.ITEMCD AND ROWNUM = 1) AS ITEMNM";
            SQL = SQL + ComNum.VBLF + " , O.ITEMCD, O.ITEMVALUE";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_IORDER B";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRSUGAMAPPING_ORDER O";
            SQL = SQL + ComNum.VBLF + "       ON O.ORDERNO = B.ORDERNO";
            SQL = SQL + ComNum.VBLF + "      AND O.EMRNO = " + EmrNo;
            SQL = SQL + ComNum.VBLF + "      AND O.GBSTATUS NOT IN ('D-')";

            if (FormNo.Equals("3150"))
            {
                SQL = SQL + ComNum.VBLF + "      AND O.ITEMVALUE NOT IN ('상처간호', '욕창간호')";
            }
            else if (FormNo.Equals("1725"))
            {
                SQL = SQL + ComNum.VBLF + "      AND O.ITEMVALUE IN ('상처간호')";
            }
            else if (FormNo.Equals("1573"))
            {
                SQL = SQL + ComNum.VBLF + "      AND O.ITEMVALUE IN ('욕창간호')";
            }
            else if (FormNo.Equals("2638"))
            {
                SQL = SQL + ComNum.VBLF + "      AND O.ITEMVALUE IN ('삽입', '유지', '제거')";
            }
            else if (FormNo.Equals("2240"))
            {
                SQL = SQL + ComNum.VBLF + "      AND O.ITEMVALUE IN ('삽입', '유지')";
            }
            SQL = SQL + ComNum.VBLF + "     LEFT OUTER JOIN KOSMOS_OCS.OCS_ORDERCODE C ";
            SQL = SQL + ComNum.VBLF + "       ON B.ORDERCODE = C.ORDERCODE";
            SQL = SQL + ComNum.VBLF + " WHERE B.PTNO = '" + pAcp.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "   AND B.BDATE = TO_DATE('" + ChartDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND B.SLIPNO IN ('A7','A6','A5','0100' )";
            SQL = SQL + ComNum.VBLF + "   AND NVL(B.ORDERSITE, ' ') NOT IN ('ERO') ";
            SQL = SQL + ComNum.VBLF + "   AND B.GBSTATUS NOT IN ('D-')";
            SQL = SQL + ComNum.VBLF + " ORDER BY O.ITEMCD, O.ITEMVALUE, B.SLIPNO, B.ORDERCODE, B.SEQNO ,B.GBSTATUS DESC ";


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
                return;
            }

            ss2_Sheet1.RowCount = 0;
            ss2_Sheet1.RowCount = dt.Rows.Count;
            ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ss2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ITEMNM"].ToString().Trim();
                ss2_Sheet1.Cells[i, 1].Tag = dt.Rows[i]["ITEMCD"].ToString().Trim();

                if (dt.Rows[i]["GBSEND"].ToString().Trim() == "*")
                {
                    ss2_Sheet1.Cells[i, 0].Locked = true;
                    ss2_Sheet1.Cells[i, 2].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ss2_Sheet1.Cells[i, 2].VerticalAlignment = CellVerticalAlignment.Top;
                    ss2_Sheet1.Cells[i, 2].Text = "Wait";
                }
                else
                {
                    ss2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                    ss2_Sheet1.Cells[i, 2].Tag = dt.Rows[i]["ITEMVALUE"].ToString().Trim();

                    if (dt.Rows[i]["GBINFO"].ToString().Trim() != "")
                    {
                        ss2_Sheet1.Cells[i, 2].Text += dt.Rows[i]["GBINFO"].ToString().Trim();
                    }
                }

                if (dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D")
                {
                    ss2_Sheet1.Rows[i].ForeColor = Color.FromArgb(255, 128, 128);
                    ss2_Sheet1.Rows[i].Locked = true;

                    if (dt.Rows[i]["SLIPNO"].ToString().Trim() == "A7")
                    {
                        ss2_Sheet1.Rows[i].Visible = false;
                    }
                }

                ss2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["QTY"].ToString().Trim();
                //ss2_Sheet1.Cells[i, 10].Text = clsErNr.READ_INSA_NAME(clsDB.DbCon, dt.Rows[i]["NURSEID"].ToString().Trim());
                ss2_Sheet1.Cells[i, 3].Tag = dt.Rows[i]["ORDERNO"].ToString().Trim();
                ss2_Sheet1.Cells[i, 4].Tag = dt.Rows[i]["ROWID"].ToString().Trim();
                //ss2_Sheet1.Cells[i, 16].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                ss2_Sheet1.Cells[i, 6].Text = clsBagage.READ_ORDER_SUBUL_BUSE(clsDB.DbCon, dt.Rows[i]["SUBUL_WARD"].ToString().Trim(), "");
            }

            dt.Dispose();
            dt = null;
        }

        private void cboList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SeqNo = cboList.Text.Substring(cboList.Text.LastIndexOf(' ') + 1).To<int>();
            SET_BAT(WardCode, FormNo, ItemNm, ItemCd, ItemValue, this.nRow, SeqNo);
        }
    }
}
