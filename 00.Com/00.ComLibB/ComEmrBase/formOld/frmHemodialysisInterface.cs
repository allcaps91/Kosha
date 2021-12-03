using ComBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// PSMHVB/mtsEmr/Frm투석인터페이스연동.frm
    /// </summary>
    public partial class frmHemodialysisInterface : Form
    {
        #region 변수
        /// <summary>
        /// 등록번호
        /// </summary>
        string mstrPano = string.Empty;

        /// <summary>
        /// GstrHelpName
        /// </summary>
        string mstrHelpName = string.Empty;
        #endregion

        #region 전달 이벤트
        /// <summary>
        /// XML용 연동
        /// </summary>
        /// <param name="strData"></param>
        public delegate void SendInterFace(string strData);
        public event SendInterFace rSendInterface;
        
        /// <summary>
        /// 신규기록지 인터페이스 연동
        /// </summary>
        /// <param name="strData"></param>
        public delegate void SendInterFace2(Dictionary<string, string> strData);
        public event SendInterFace2 rSendInterface2;
        #endregion

        #region 생성자
        /// <summary>
        /// 이것만 쓰세요.
        /// </summary>
        /// <param name="strHelpName">GstrHelpName</param> 
        /// <param name="strHelpCode">등록번호</param>
        public frmHemodialysisInterface(string strHelpName, string strHelpCode)
        {
            mstrHelpName = strHelpName;
            mstrPano = strHelpCode;
            InitializeComponent();
        }
        #endregion

        #region 폼 이벤트
        private void frmHemodialysisInterface_Load(object sender, EventArgs e)
        {
            mstrPano = VB.Val(mstrPano).ToString("00000000");
            mstrPano = VB.Left(mstrPano, 1).Equals("0") ? VB.Mid(mstrPano, 2, mstrPano.Length) : mstrPano;

            dtpSDATE.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            dtpEDATE.Value = dtpSDATE.Value;

            SS1.Visible = false;
            SS2.Visible = false;

            if (mstrHelpName.Equals("VITAL"))
            {
                SS2.Visible = true;
                Text = "인공신장실 V/S Sheet";
            }
            else if (mstrHelpName.Equals("PAPER"))
            {
                SS1.Visible = true;
                Text = "혈액투석 기록지 연동";
            }

            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            SS1_Sheet1.DefaultStyle.Border = complexBorder2;
            SS1_Sheet1.SheetCornerStyle.Border = complexBorder2;
            SS1_Sheet1.ColumnHeader.DefaultStyle.Border = complexBorder2;
            SS1_Sheet1.RowHeader.DefaultStyle.Border = complexBorder2;
            SS1.BorderStyle = BorderStyle.FixedSingle;

            SS2_Sheet1.SheetCornerStyle.Border = complexBorder2;
            SS2_Sheet1.DefaultStyle.Border = complexBorder2;
            SS2_Sheet1.ColumnHeader.DefaultStyle.Border = complexBorder2;
            SS2_Sheet1.RowHeader.DefaultStyle.Border = complexBorder2;
            SS2.BorderStyle = BorderStyle.FixedSingle;
        }
        #endregion

        #region 버튼 이벤트
        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(mstrHelpName))
            {
                ComFunc.MsgBoxEx(this, "등록번호가 없습니다.");
                return;
            }

            READ_PAPER();
            READ_VITAL();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (rSendInterface != null )
            {
                if (mstrHelpName.Equals("VITAL"))
                {
                    rSendInterface("||||||||||");
                }
                else
                {
                    rSendInterface("||||||||");
                }
            }
            
            Close();
        }
        #endregion


        #region 조회 함수
        /// <summary>
        /// 혈액투석 기록지 연동
        /// </summary>
        void READ_PAPER()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT PANO A, DIALDATE B, DIALSTRTIME C, DIALENDTIME D,"         ;
                SQL += ComNum.VBLF + " DIALINSTR E, DIALTOTTIME F, DIALSPECIES G,"          ;
                SQL += ComNum.VBLF + " TEMPERATURE H, HEPARINFSCAP I, HEPARINMAINTCAP J,"   ;
                SQL += ComNum.VBLF + " WGTH K, TARGET L"                                    ;
                SQL += ComNum.VBLF + "  FROM ADMIN.EXAM_INTERFACE_DIALTRANS"           ;
                SQL += ComNum.VBLF + " WHERE PANO = '" + mstrPano + "'"                     ;
                SQL += ComNum.VBLF + "   AND DIALDATE >= '" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "'"           ;
                SQL += ComNum.VBLF + "   AND DIALDATE <= '" + dtpEDATE.Value.ToString("yyyy-MM-dd") + "'"           ;
                SQL += ComNum.VBLF + "  ORDER BY B ASC, C ASC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["A"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["B"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["C"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["D"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["E"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["F"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["G"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["H"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["I"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["J"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["K"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 12].Text = dt.Rows[i]["L"].ToString().Trim();
                    }
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }


        /// <summary>
        /// 인공신장실 V/S Sheet
        /// </summary>
        void READ_VITAL()
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT PANO A, DIALDATE B, DIALTIME C, CNSTBIPRS D, RIXBIPRS E, PLUS F, AP G, VP H, TMP I,";
                SQL += ComNum.VBLF + "         '' J, NULL K, NULL L,";
                SQL += ComNum.VBLF + "         '' M, '' N, '' O, '' P, '' Q,";
                SQL += ComNum.VBLF + "         NULL R, NULL S, NULL T, NULL U, NULL V";
                SQL += ComNum.VBLF + "  FROM ADMIN.EXAM_INTERFACE_BIPRSTRANS";
                SQL += ComNum.VBLF + " WHERE PANO IN ('" + mstrPano + "', '" + mstrPano.PadLeft(8, '0') + "', '" + VB.Val(mstrPano) + "')";
                SQL += ComNum.VBLF + "   AND DIALDATE >= '" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "'";
                SQL += ComNum.VBLF + "   AND DIALDATE <= '" + dtpEDATE.Value.ToString("yyyy-MM-dd") + "'";
                SQL += ComNum.VBLF + " Union All";
                SQL += ComNum.VBLF + " SELECT PANO A, DIALDATE B, DIALSTRTIME C, NULL D, NULL E, NULL F, '' G, '' H, '' I,";
                SQL += ComNum.VBLF + "       DIALENDTIME J, DIALPREWGHT K, DIALPOSTWGHT L,";
                SQL += ComNum.VBLF + "       UF1 M, UF2 N, UF3 O, UF4 P, UF5 Q,";
                SQL += ComNum.VBLF + "       TMP1 R, TMP2 S, TMP3 T, TMP4 U, TMP5 V";
                SQL += ComNum.VBLF + "  FROM ADMIN.EXAM_INTERFACE_DIALTRANS";
                SQL += ComNum.VBLF + " WHERE PANO IN ('" + mstrPano + "', '" + mstrPano.PadLeft(8, '0') + "', '" + VB.Val(mstrPano) + "')";
                SQL += ComNum.VBLF + "   AND DIALDATE >= '" + dtpSDATE.Value.ToString("yyyy-MM-dd") + "'";
                SQL += ComNum.VBLF + "   AND DIALDATE <= '" + dtpEDATE.Value.ToString("yyyy-MM-dd") + "'";
                SQL += ComNum.VBLF + " ORDER BY B DESC, C DESC";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS2_Sheet1.RowCount = dt.Rows.Count;
                    SS2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SS2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["A"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["B"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["C"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["D"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["E"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["F"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["G"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["H"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 9].Text = dt.Rows[i]["I"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 10].Text = dt.Rows[i]["J"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 11].Text = dt.Rows[i]["K"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 12].Text = dt.Rows[i]["L"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 13].Text = dt.Rows[i]["M"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 14].Text = dt.Rows[i]["N"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 15].Text = dt.Rows[i]["O"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 16].Text = dt.Rows[i]["P"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 17].Text = dt.Rows[i]["Q"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 18].Text = dt.Rows[i]["R"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 19].Text = dt.Rows[i]["S"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 20].Text = dt.Rows[i]["T"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 21].Text = dt.Rows[i]["U"].ToString().Trim();
                        SS2_Sheet1.Cells[i, 22].Text = dt.Rows[i]["V"].ToString().Trim();
                    }
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SS1_Sheet1.RowCount == 0)
                return;

            string strHelpCode = SS1_Sheet1.Cells[e.Row, 5].Text.Trim();
            strHelpCode += "|" + (VB.Val(SS1_Sheet1.Cells[e.Row, 6].Text.Trim()) / 60).ToString() + "시간";
            strHelpCode += "|" + SS1_Sheet1.Cells[e.Row, 7].Text.Trim();
            strHelpCode += "|" + SS1_Sheet1.Cells[e.Row, 8].Text.Trim();
            strHelpCode += "|" + SS1_Sheet1.Cells[e.Row, 9].Text.Trim() + "unit";
            strHelpCode += "|" + SS1_Sheet1.Cells[e.Row, 10].Text.Trim() + "unit";
            strHelpCode += "|" + SS1_Sheet1.Cells[e.Row, 11].Text.Trim();
            strHelpCode += "|" + SS1_Sheet1.Cells[e.Row, 12].Text.Trim();

            rSendInterface(strHelpCode);
            Close();
        }

        private void SS2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SS2_Sheet1.RowCount == 0)
                return;

            string strHelpCode = SS2_Sheet1.Cells[e.Row, 2].Text.Trim();
            strHelpCode += "|" + SS2_Sheet1.Cells[e.Row, 3].Text.Trim();
            strHelpCode += "|" + SS2_Sheet1.Cells[e.Row, 4].Text.Trim();
            strHelpCode += "|" + SS2_Sheet1.Cells[e.Row, 5].Text.Trim();
            strHelpCode += "|" + SS2_Sheet1.Cells[e.Row, 6].Text.Trim();
            strHelpCode += "|" + SS2_Sheet1.Cells[e.Row, 11].Text.Trim();  //pre wt
            strHelpCode += "|" + SS2_Sheet1.Cells[e.Row, 12].Text.Trim();  //post wt
            strHelpCode += "|" + SS2_Sheet1.Cells[e.Row, 7].Text.Trim();   //동맥압
            strHelpCode += "|" + SS2_Sheet1.Cells[e.Row, 8].Text.Trim();   //정맥압
            strHelpCode += "|" + SS2_Sheet1.Cells[e.Row, 13].Text.Trim();  //초여과율
            strHelpCode += "|" + SS2_Sheet1.Cells[e.Row, 9].Text.Trim();   //막통과압

            rSendInterface(strHelpCode);
            Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (rSendInterface2 == null)
                return;

            Dictionary<string, string> lstVal = new Dictionary<string, string>();
            for (int i = SS2_Sheet1.RowCount - 1; i >= 0; i--)
            {
                if (SS2_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                {
                    lstVal.Clear();
                    lstVal.Add("측정시간", SS2_Sheet1.Cells[i, 3].Text.Trim()); //측정시간
                    
                    string strVal = SS2_Sheet1.Cells[i, 4].Text.Trim();

                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000002018", strVal); //수축기압
                    }

                    strVal = SS2_Sheet1.Cells[i, 5].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000001765", strVal); //이완기압
                    }


                    strVal = SS2_Sheet1.Cells[i, 6].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000014815", strVal); //맥박
                    }


                    strVal = SS2_Sheet1.Cells[i, 7].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000001791", strVal); //동맥압
                    }

                    strVal = SS2_Sheet1.Cells[i, 8].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000037825", strVal); //정맥압
                    }

                    strVal = SS2_Sheet1.Cells[i, 13].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000035472", strVal); //UFR
                    }

                    //lstVal.Add("I0000014815", SS2_Sheet1.Cells[i, 6].Text.Trim()); //호흡
                    //lstVal.Add("I0000001811", SS2_Sheet1.Cells[i, 6].Text.Trim()); //체온

                    rSendInterface2(lstVal);
                }
            }
            Close();
        }
    }
}
