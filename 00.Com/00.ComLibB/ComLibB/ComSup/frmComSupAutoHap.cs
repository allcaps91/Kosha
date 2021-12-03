using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstOrderPrint.cs
    /// Description     : 원무수납용 합산발행
    /// Author          : 이정현
    /// Create Date     : 2018-01-22
    /// <history> 
    /// 원무수납용 합산발행
    /// </history>
    /// <seealso>
    /// PSMH\drug\drAutoprt\FrmAutoHap.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\drug\drAutoprt\drAutoprt.vbp
    /// </vbp>
    /// </summary>
    public partial class frmComSupAutoHap : Form
    {
        public delegate void SendDataHandler(string strSlipNo);
        public event SendDataHandler SendEvent;
        
        private string GstrPANO = "";
        private string GstrDEPTCODE = "";
        private string GstrBDATE = "";
        private string GstrSLIPNO = "";
        private string GstrSNAME = "";
        
        public frmComSupAutoHap(string strPANO, string strDEPTCODE, string strBDATE, string strSLIPNO, string strSNAME)
        {
            InitializeComponent();

            GstrPANO = strPANO;
            GstrDEPTCODE = strDEPTCODE;
            GstrBDATE = strBDATE;
            GstrSLIPNO = strSLIPNO;
            GstrSNAME = strSNAME;
        }

        private void frmComSupAutoHap_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ssList_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 0;

            lblTitleSub0.Text = "원외처방전 합산발행 ◈ " + GstrPANO + " " + GstrDEPTCODE + " " + GstrBDATE + " ";
            lblTitleSub0.Text = lblTitleSub0.Text + GstrSLIPNO + " " + GstrSNAME + " ◈";

            //ETC_TUYAK의 해당 정보를 READ
            GetList();

            GetData();
        }

        private void GetList()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TuNo, TuIlsu AS SlipNo, Flag, ROWID,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SunapTime,'HH24:MI') AS SuNapTime,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(JepsuTime,'HH24:MI') AS JepsuTime,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(JojeTime,'HH24:MI') AS JojeTime,";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(TuyakTime,'HH24:MI') AS TuyakTime ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_TUYAK ";
                SQL = SQL + ComNum.VBLF + "     WHERE TuDate = TO_DATE('" + GstrBDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND Pano = '" + GstrPANO + "' ";
                SQL = SQL + ComNum.VBLF + "         AND DeptCode='" + GstrDEPTCODE + "' ";
                SQL = SQL + ComNum.VBLF + "         AND SlipGbn = '3' "; //원외처방전만
                SQL = SQL + ComNum.VBLF + "         AND Flag <> 'D' ";   //삭제된것은 제외
                SQL = SQL + ComNum.VBLF + "ORDER BY TuNo ";

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
                    ssList_Sheet1.RowCount = dt.Rows.Count;
                    ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Value = true;
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["TUNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SLIPNO"].ToString().Trim();

                        switch (dt.Rows[i]["FLAG"].ToString().Trim())
                        {
                            case "0": ssList_Sheet1.Cells[i, 3].Text = "대기"; break;
                            case "1": ssList_Sheet1.Cells[i, 3].Text = "조제중"; break;
                            case "2": ssList_Sheet1.Cells[i, 3].Text = "완료"; break;
                            case "3": ssList_Sheet1.Cells[i, 3].Text = "투약"; break;
                            case "4": ssList_Sheet1.Cells[i, 3].Text = "2매"; break;
                            case "D": ssList_Sheet1.Cells[i, 3].Text = "삭제"; break;
                            default: ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["FLAG"].ToString().Trim(); break;
                        }

                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SUNAPTIME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["JEPSUTIME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["JOJETIME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["TUYAKTIME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }

                    txtSlipNo.Text = dt.Rows[0]["SLIPNO"].ToString().Trim();
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strAllSlipNo = "";

            ssView_Sheet1.RowCount = 0;

            for (i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value) == true)
                {
                    txtSlipNo.Text = ssList_Sheet1.Cells[i, 2].Text.Trim();
                    strAllSlipNo = strAllSlipNo + ssList_Sheet1.Cells[i, 2].Text.Trim() + ", ";
                }
            }

            if (strAllSlipNo.Trim() == "")
            {
                ComFunc.MsgBox("합산할 원외처방전을 1건도 선택하지 않음");
                return;
            }

            strAllSlipNo = VB.Mid(strAllSlipNo, 1, strAllSlipNo.Length - 2);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //OCS_OUTDRUG를 READ
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     a.SlipNo, a.DosCode, a.Nal, a.SuCode, a.Bun, a.RealQty, a.DivQty, ";
                SQL = SQL + ComNum.VBLF + "     a.Div, a.OrderNo, a.Remark, a.GbSelf, b.SuNameK, b.Unit, ";
                SQL = SQL + ComNum.VBLF + "     C.DosName, C.LabelName1, C.LabelName2, C.IDosName ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OUTDRUG a";
                SQL = SQL + ComNum.VBLF + "     INNER JOIN " + ComNum.DB_PMPA + "BAS_SUN b ";
                SQL = SQL + ComNum.VBLF + "         ON a.SuCode = b.SuNext";
                SQL = SQL + ComNum.VBLF + "     LEFT OUTER JOIN " + ComNum.DB_MED + "OCS_ODOSAGE C";
                SQL = SQL + ComNum.VBLF + "         ON A.DOSCODE = C.DOSCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE a.SlipDate = TO_DATE('" + GstrBDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND a.SlipNo IN (" + strAllSlipNo + ") ";
                SQL = SQL + ComNum.VBLF + "         AND (a.Flag <> 'D' OR a.Flag IS NULL) "; //취소

                if (rdoSORT0.Checked == true)   //원외처방번호별
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.SlipNo, a.SuCode ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY a.SuCode, a.SlipNo ";
                }

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
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SLIPNO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DOSCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DIVQTY"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DIV"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GBSELF"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["IDOSNAME"].ToString().Trim();

                        if (ssView_Sheet1.Cells[i, 9].Text.Trim() == "")
                        {
                            if (dt.Rows[i]["LABELNAME1"].ToString().Trim() != "")
                            {
                                ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["LABELNAME1"].ToString().Trim();

                                if (dt.Rows[i]["LABELNAME2"].ToString().Trim() != "")
                                {
                                    ssView_Sheet1.Cells[i, 9].Text += ComNum.VBLF + dt.Rows[i]["LABELNAME2"].ToString().Trim();
                                    ssView_Sheet1.SetRowHeight(i, ComNum.SPDROWHT + 16);
                                }
                            }
                            else
                            {
                                ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();
                            }
                        }
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnHap_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
            if (ssList_Sheet1.RowCount == 0) { return; }

            if (HapData() == true)
            {
                txtSlipNo.Text = GstrSLIPNO;
                SendEvent(GstrSLIPNO);
            }
        }

        private bool HapData()
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            int i = 0;

            string strChk = "";
            int intSelTuNo = 0;
            int intSelSlipNo = 0;

            int intTuNo = 0;
            int intSlipNo = 0;
            string strROWID = "";

            for (i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                intTuNo = Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[i, 1].Text.Trim()));
                intSlipNo = Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[i, 2].Text.Trim()));

                if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value) == true && intSlipNo == Convert.ToInt32(VB.Val(txtSlipNo.Text.Trim())))
                {
                    intSelTuNo = Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[i, 1].Text.Trim()));
                    intSelSlipNo = Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[i, 2].Text.Trim()));
                    strChk = "OK";
                    break;
                }
            }

            if (strChk == "")
            {
                ComFunc.MsgBox("변경하실 처방번호는 선택한 번호중 1개만 가능합니다.", "오류");
                return rtnVal;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value) == true)
                    {
                        intTuNo = Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[i, 1].Text.Trim()));
                        intSlipNo = Convert.ToInt32(VB.Val(ssList_Sheet1.Cells[i, 2].Text.Trim()));
                        strROWID = ssList_Sheet1.Cells[i, 8].Text.Trim();

                        //ETC_TUYAK에 합산 처리
                        if (intTuNo == intSelTuNo)      //합산할 투약번호
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_TUYAK";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         Flag = '0', ";
                            SQL = SQL + ComNum.VBLF + "         JojeTime = '', ";
                            SQL = SQL + ComNum.VBLF + "         TuyakTime = '' ";
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                        }
                        else                            //취소(삭제)할 투약번호
                        {
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_PMPA + "ETC_TUYAK";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         Flag = 'D' "; //삭제
                            SQL = SQL + ComNum.VBLF + "WHERE ROWID='" + strROWID + "' ";
                        }

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("ETC_TUYAK UPDATE 도중 오류 발생");
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        //OCS_OUTDRUG,OCS_OUTDRUGMST SET
                        if (intSlipNo == Convert.ToInt32(VB.Val(txtSlipNo.Text)))
                        {
                            //합산된 처방전은 미인쇄 상태로 전환
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_OUTDRUGMST";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         FLAG = '0', ";
                            SQL = SQL + ComNum.VBLF + "         PrtDate = '', ";
                            SQL = SQL + ComNum.VBLF + "         PrtDept = ' ', ";
                            SQL = SQL + ComNum.VBLF + "         SendDate = '', ";
                            SQL = SQL + ComNum.VBLF + "         HAPPRINT = '1' ";
                            SQL = SQL + ComNum.VBLF + "WHERE SlipDate = TO_DATE('" + GstrBDATE + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "     AND SlipNo = " + Convert.ToInt32(VB.Val(txtSlipNo.Text)) + " ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("원외처방전 마스타 대기자로 전환시 오류가 발생함");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                        else
                        {
                            //합산으로 취소된 원외처방마스타는 삭제 SET
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_OUTDRUGMST";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         FLAG = 'D', ";
                            SQL = SQL + ComNum.VBLF + "         HAPPRINT = '' "; //삭제
                            SQL = SQL + ComNum.VBLF + "WHERE SlipDate = TO_DATE('" + GstrBDATE + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "     AND SlipNo = " + intSlipNo + " ";
                            SQL = SQL + ComNum.VBLF + "     AND Pano = '" + GstrPANO + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("원외처방전 마스타 삭제SET 오류가 발생함");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }

                            //해당 원외처방전을 합산
                            SQL = "";
                            SQL = "UPDATE " + ComNum.DB_MED + "OCS_OUTDRUG";
                            SQL = SQL + ComNum.VBLF + "     SET";
                            SQL = SQL + ComNum.VBLF + "         SlipNo = " + Convert.ToInt32(VB.Val(txtSlipNo.Text)) + " ";
                            SQL = SQL + ComNum.VBLF + "WHERE SlipDate = TO_DATE('" + GstrBDATE + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + "     AND SlipNo = " + intSlipNo + " ";
                            SQL = SQL + ComNum.VBLF + "     AND Pano = '" + GstrPANO + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox("원외처방전 번호를 합산시 오류가 발생함");
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return rtnVal;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            SendEvent(GstrSLIPNO);
        }
    }
}
