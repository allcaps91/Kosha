using ComBase;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : EmrJob
    /// File Name       : Frm외래데이타조회1
    /// Description     : 접수자 List
    /// Author          : 이현종
    /// Create Date     : 2020-01-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\Etc\callmid\Frm외래데이타조회1.frm >> frmEmrJobMasterList.cs 폼이름 재정의" />
    public partial class frmEmrJobOpdDataSearch : Form
    {
        #region 변수
        string strFlagChange = string.Empty;

        /// <summary>
        /// 자동조회
        /// </summary>
        string mstrPano = string.Empty;
        #endregion

        public frmEmrJobOpdDataSearch()
        {
            InitializeComponent();
        }

        public frmEmrJobOpdDataSearch(string strPano)
        {
            mstrPano = strPano;
            InitializeComponent();
        }

        private void frmEmrJobOpdDataSearch_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;

            }
            ssPat_Sheet1.RowCount = 0;
            ssSlip_Sheet1.RowCount = 0;
            SS1_Sheet1.RowCount = 0;

            if (mstrPano.Length > 0)
            {
                txtPano.Text = mstrPano;
                Read_PAT_MAST();
                Read_OS_JINRYO();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region 함수
        /// <summary>
        /// 환자정보
        /// </summary>
        private void Read_PAT_MAST()
        {
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                #region 쿼리
                SQL = "SELECT TO_CHAR(StartDate, 'YYYY-MM-DD') Sdate,";
                SQL += ComNum.VBLF + "       TO_CHAR(LastDate, 'YYYY-MM-DD') Ldate, ";
                SQL += ComNum.VBLF + "       Sname, Sex, Jumin1, Jumin2 ";
                SQL += ComNum.VBLF + "  FROM ADMIN.BAS_PATIENT ";
                SQL += ComNum.VBLF + " WHERE Pano = '" + txtPano.Text + "' ";
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                if (reader.HasRows && reader.Read())
                {
                    LabelID1.Text = "수진자명 : "  ;
                    LabelID2.Text = "성    별 : " ;
                    LabelID3.Text = "주민번호 : "  ;
                    LabelID4.Text = "최초내원 : "  ;
                    LabelID5.Text = "최종내원 : ";

                    LabelID1.Text += reader.GetValue(2).ToString().Trim();
                    LabelID2.Text += reader.GetValue(3).ToString().Trim();
                    LabelID3.Text += reader.GetValue(4).ToString().Trim() + " - ";
                    LabelID3.Text += reader.GetValue(5).ToString().Trim();
                    LabelID4.Text += reader.GetValue(0).ToString().Trim();
                    LabelID5.Text += reader.GetValue(1).ToString().Trim();
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }
        }

        /// <summary>
        /// 진료내역 조회
        /// </summary>
        private void Read_OS_JINRYO()
        {
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                #region 쿼리
                SQL = "";
                SQL += ComNum.VBLF + "SELECT TO_CHAR(ActDate,'YYYY-MM-DD') Adate, ";
                SQL += ComNum.VBLF + "       DeptCode, DrName, Bi, o.DRCode,   ";
                SQL += ComNum.VBLF + "       TO_CHAR(BDATE,'YYYY-MM-DD') BDATE ";
                SQL += ComNum.VBLF + "  FROM ADMIN.OPD_SLIP O, ADMIN.BAS_DOCTOR B ";
                SQL += ComNum.VBLF + " WHERE Pano     = '" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "   AND O.DrCode = B.DrCode(+) ";
                SQL += ComNum.VBLF + " GROUP BY ActDate, DeptCode, DrName, Bi, o.DrCode, BDATE ";
                SQL += ComNum.VBLF + " ORDER BY ActDate Desc, BDATE ";
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ssPat_Sheet1.RowCount += 1;

                        ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 0].Text = reader.GetValue(0).ToString().Trim();
                        ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 1].Text = reader.GetValue(1).ToString().Trim();
                        ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 2].Text = reader.GetValue(2).ToString().Trim();
                        ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 3].Text = clsVbfunc.GetBiName(reader.GetValue(3).ToString().Trim());
                        ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 4].Text = reader.GetValue(4).ToString().Trim();
                        ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 5].Text = reader.GetValue(3).ToString().Trim();
                        ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 7].Text = reader.GetValue(5).ToString().Trim();

                    }
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }
        }


        /// <summary>
        /// 처방 데이터 조회
        /// </summary>
        private void Read_OPD_SLIP(string ArgDate, string ArgDept, string ArgDrCode, string ArgBi, string ArgBDate)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            ssSlip_Sheet1.RowCount = 0;

            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                #region 쿼리
                SQL = "";
                SQL += ComNum.VBLF + "SELECT Sucode, SunameK, BaseAmt, Qty, Nal,";
                SQL += ComNum.VBLF + "       GbSpc, GbNgt, GbGisul, GbSelf, GbChild,";
                SQL += ComNum.VBLF + "       Amt1, Amt2, SeqNo, Part, O.Rowid ";
                SQL += ComNum.VBLF + "  FROM ADMIN.OPD_SLIP O,  ADMIN.BAS_SUN B";
                SQL += ComNum.VBLF + " WHERE ActDate  = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND Pano     = '" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "   AND Bi       = '" + ArgBi + "' ";
                SQL += ComNum.VBLF + "   AND BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND DeptCode = '" + ArgDept + "' ";
                SQL += ComNum.VBLF + "   AND DrCode = '" + ArgDrCode + "' ";
                SQL += ComNum.VBLF + "   AND O.Sunext = B.Sunext ";
                SQL += ComNum.VBLF + " ORDER BY  SeqNo, O.Bun, O.Sucode, O.Sunext";
                // jjy(프로그램 수정) 영수증 번호 별로 display 요청(심사계 김순옥 샘) 응급실만
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ssSlip_Sheet1.RowCount += 1;

                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 0].Text = reader.GetValue(0).ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 1].Text = reader.GetValue(1).ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 2].Text = VB.Format(VB.Val(reader.GetValue(2).ToString().Trim()), "##,###,##0");
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 3].Text = VB.Format(VB.Val(reader.GetValue(3).ToString().Trim()), "#0.0");
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 4].Text = VB.Format(VB.Val(reader.GetValue(4).ToString().Trim()), "##0");
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 5].Text = reader.GetValue(5).ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 6].Text = reader.GetValue(6).ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 7].Text = reader.GetValue(7).ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 8].Text = reader.GetValue(8).ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 9].Text = reader.GetValue(9).ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 10].Text = VB.Format(VB.Val(reader.GetValue(10).ToString().Trim()), "##,###,##0");
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 11].Text = VB.Format(VB.Val(reader.GetValue(11).ToString().Trim()), "####,##0");
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 12].Text = VB.Format(VB.Val(reader.GetValue(12).ToString().Trim()), "###0");
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 13].Text = reader.GetValue(13).ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.RowCount - 1, 14].Text = reader.GetValue(14).ToString().Trim();

                        ssSlip_Sheet1.Rows[ssSlip_Sheet1.RowCount - 1].Height = ssSlip_Sheet1.Rows[ssSlip_Sheet1.RowCount - 1].GetPreferredHeight() + 4;
                    }
                }

                reader.Dispose();

                #region 상병

                #region 쿼리
                SQL = "SELECT A.ILLCODE, B.ILLNAMEK FROM ADMIN.OCS_OILLS A, ADMIN.BAS_ILLS B";
                SQL += ComNum.VBLF + " WHERE a.BDate  = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND a.PTno     = '" + txtPano.Text + "' ";
                SQL += ComNum.VBLF + "   AND A.DeptCode = '" + ArgDept + "' ";
                SQL += ComNum.VBLF + "   AND b.IllClass ='1' ";
                SQL += ComNum.VBLF + "   AND A.ILLCODE = B.ILLCODE";
                SQL += ComNum.VBLF + " ORDER BY A.SEQNO ";
                // jjy(프로그램 수정) 영수증 번호 별로 display 요청(심사계 김순옥 샘) 응급실만
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                SS1_Sheet1.RowCount = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SS1_Sheet1.RowCount += 1;

                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 0].Text = reader.GetValue(0).ToString().Trim();
                        SS1_Sheet1.Cells[SS1_Sheet1.RowCount - 1, 1].Text = reader.GetValue(1).ToString().Trim();

                    }
                }

                reader.Dispose();

                #endregion
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr);
                return;
            }
        }
        #endregion

        #region 텍스트 이벤트

        private void txtPano_Enter(object sender, EventArgs e)
        {
            txtPano.SelectAll();
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                if (strFlagChange.Equals("**"))
                {
                    txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");

                    if (VB.IsNumeric(txtPano.Text))
                    {
                        if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                        {
                            return; //권한 확인
                        }

                        Read_PAT_MAST();
                        Read_OS_JINRYO();
                        string strCHK = string.Empty;
                        for(int i = 0; i < cboPano.Items.Count; i++)
                        {
                            if (cboPano.Items[i].ToString().Trim().Equals(txtPano.Text.Trim()))
                            {
                                strCHK = "NO";
                                break;
                            }
                        }

                        if (string.IsNullOrWhiteSpace(strCHK))
                        {
                            cboPano.Items.Add(txtPano.Text.Trim());
                        }
                    }
                }
            }
        }

        private void txtPano_TextChanged(object sender, EventArgs e)
        {
            strFlagChange = "**";

            if (LabelID1.Text.Length > 0)
            {
                LabelID1.Text = "";
                LabelID2.Text = "";
                LabelID3.Text = "";
                LabelID4.Text = "";
                LabelID5.Text = "";

                ssPat_Sheet1.RowCount = 0;
                ssSlip_Sheet1.RowCount = 0;
            }
        }
        #endregion

        private void ssPat_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssPat_Sheet1.RowCount == 0)
                return;

            string strDate = ssPat_Sheet1.Cells[e.Row, 0].Text.Trim();
            string strArrActDate = ssPat_Sheet1.Cells[e.Row, 0].Text.Trim();
            string strArrDept = ssPat_Sheet1.Cells[e.Row, 1].Text.Trim();
            string strArrDrCode = ssPat_Sheet1.Cells[e.Row, 4].Text.Trim();
            string strArrBi = ssPat_Sheet1.Cells[e.Row, 5].Text.Trim();
            string strArrDate = ssPat_Sheet1.Cells[e.Row, 7].Text.Trim();

            Read_OPD_SLIP(strDate, strArrDept, strArrDrCode, strArrBi, strArrDate);
        }
    }
}
