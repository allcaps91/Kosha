using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;

namespace ComEmrBase
{
    /// <summary>
    /// Patient Monitor Interface 
    /// </summary>
    public partial class frmPatientMonitorInterface : Form
    {
        #region 변수
        /// <summary>
        /// EMR 환자정보
        /// </summary>
        EmrPatient AcpEmr = null;

        string strDate = string.Empty;
        string strTime = string.Empty;
        #endregion

        #region 전달 이벤트
        /// <summary>
        /// 신규기록지 인터페이스 연동
        /// </summary>
        /// <param name="strData"></param>
        public delegate void SendInterFace(Dictionary<string, string> strData);
        public event SendInterFace rSendInterface;
        #endregion

        #region 생성자
        /// <summary>
        /// 이것만 쓰세요.
        /// </summary>
        /// <param name="p">EmrPatient</param>
        public frmPatientMonitorInterface(EmrPatient p, string Date, string Time)
        {
            AcpEmr = p;
            strDate = Date;
            strTime = Time;
            InitializeComponent();
        }
        #endregion

        #region 폼 이벤트
        private void frmPatientMonitorInterface_Load(object sender, EventArgs e)
        {
            SS1_Sheet1.RowCount = 0;

            #region 콤보박스
            for(int i = 0; i < 25; i++)
            {
                if (i == 24)
                {
                    cboTime1.Items.Add("23:59");
                    cboTime2.Items.Add("23:59");
                }
                else
                {
                    cboTime1.Items.Add(i.ToString("00") + ":00");
                    cboTime2.Items.Add(i.ToString("00") + ":00");
                }
            }
            try
            {
                cboTime1.Text = strTime.To<DateTime>().AddMinutes(-20).ToString("HH:mm");
            }
            catch { }
            cboTime2.Text = strTime;
            #endregion

            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            SS1_Sheet1.DefaultStyle.Border = complexBorder2;
            SS1_Sheet1.SheetCornerStyle.Border = complexBorder2;
            SS1_Sheet1.ColumnHeader.DefaultStyle.Border = complexBorder2;
            SS1_Sheet1.RowHeader.DefaultStyle.Border = complexBorder2;
            SS1.BorderStyle = BorderStyle.FixedSingle;

            if (AcpEmr == null || string.IsNullOrWhiteSpace(AcpEmr.ptNo))
            {
                ComFunc.MsgBoxEx(this, "등록번호가 없습니다.");
                return;
            }

            if (AcpEmr.medDeptCd.Equals("ER"))
            {
                DateTime dtp = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();
                if (strTime.IsNullOrEmpty())
                {
                    cboTime1.Text = dtp.AddMinutes(-60).ToString("HH:mm");
                    cboTime2.Text = dtp.ToString("HH:mm");
                }

                //rdoTime5.Checked = true;
                rdoNIBP.Checked = true;
            }

            READ_DATA();
        }
        #endregion

        #region 버튼 이벤트
        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            if (AcpEmr == null || string.IsNullOrWhiteSpace(AcpEmr.ptNo))
            {
                ComFunc.MsgBoxEx(this, "등록번호가 없습니다.");
                return;
            }

            READ_DATA();
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }



        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region 조회 함수
        /// <summary>
        /// 데이터 가져오기
        /// </summary>
        void READ_DATA()
        {
            SS1_Sheet1.RowCount = 0;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT "         ;
                SQL += ComNum.VBLF + "   A.ACTDATE            -- 회계일자";
                SQL += ComNum.VBLF + " , A.PANO               -- 등록번호";
                SQL += ComNum.VBLF + " , EXDATE               -- 시행시간";
                SQL += ComNum.VBLF + " , NIBPDATE             -- NIBP시행시간";
                SQL += ComNum.VBLF + " , B.SNAME              -- 이름";
                SQL += ComNum.VBLF + " , BEDNAME              -- 베드명";
                SQL += ComNum.VBLF + " , RESULT_NIBP_SYS      -- NIBP 혈압 수축";
                SQL += ComNum.VBLF + " , RESULT_NIBP_DIAS     -- NIBP 혈압 확장";
                SQL += ComNum.VBLF + " , RESULT_NIBP_MEAN     -- NIBP 혈압 평균";
                SQL += ComNum.VBLF + " , RESULT_ART_S         -- 동맥혈압 수축";
                SQL += ComNum.VBLF + " , RESULT_ART_D         -- 동맥혈압 확장";
                SQL += ComNum.VBLF + " , RESULT_ART_M         -- 동맥혈압 평균";
                SQL += ComNum.VBLF + " , RESULT_HR            -- 맥박";
                SQL += ComNum.VBLF + " , RESULT_RESP          -- 호흡수";
                SQL += ComNum.VBLF + " , RESULT_RRESP_IMP     -- 호흡수2";
                SQL += ComNum.VBLF + " , RESULT_TEMP_RECTAL   -- 온도";
                SQL += ComNum.VBLF + " , RESULT_SPO2          -- 산소포화도";
                SQL += ComNum.VBLF + " , RESULT_CVP_M         -- 중심정맥압 평균";
                SQL += ComNum.VBLF + " , RESULT_ETCO2         -- 호기말 이산화탄소 분압";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.EXAM_INTERFACE_PM A";
                SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_PMPA.BAS_PATIENT B";
                SQL += ComNum.VBLF + "       ON A.PANO = B.PANO";
                SQL += ComNum.VBLF + " WHERE A.PANO = '" + AcpEmr.ptNo + "'";
                SQL += ComNum.VBLF + "   AND A.ACTDATE = '" + strDate + "'";
                //SQL += ComNum.VBLF + "   AND A.NIBPDATE >= '" + strDate + cboTime1.Text.Replace(":", "") +  "00'";
                //SQL += ComNum.VBLF + "   AND A.NIBPDATE <= '" + strDate + cboTime2.Text.Replace(":", "") +  "00'";

                SQL += ComNum.VBLF + "   AND A.EXDATE >= '" + strDate + cboTime1.Text.Replace(":", "") + "00'";
                SQL += ComNum.VBLF + "   AND A.EXDATE <= '" + strDate + cboTime2.Text.Replace(":", "") + "00'";

                if (rdoTime1.Checked)
                {
                    SQL += ComNum.VBLF + "   AND SUBSTR(A.EXDATE, 11, 2) = '00'";
                }
                else if (rdoTime2.Checked)
                {
                    SQL += ComNum.VBLF + "   AND SUBSTR(A.EXDATE, 11, 2) IN('05', '10', '15', '20', '25', '30', '35', '40', '45', '50', '55', '00')";
                }
                else if (rdoTime3.Checked)
                {
                    SQL += ComNum.VBLF + "   AND SUBSTR(A.EXDATE, 11, 2) IN('15', '30', '45', '00')";
                }
                else if (rdoTime4.Checked)
                {
                    SQL += ComNum.VBLF + "   AND SUBSTR(A.EXDATE, 11, 2) IN('30', '00')";
                }
                //else if (rdoNIBP2.Checked)
                //{
                //    SQL += ComNum.VBLF + "   AND SUBSTR(EXDATE, 0, 12)  = SUBSTR(NIBPDATE, 0, 12)";
                //}

                SQL += ComNum.VBLF + " ORDER BY A.EXDATE";

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
                    SS1_Sheet1.RowCount = dt.Rows.Count;

                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        DateTime ExDate = DateTime.ParseExact(dt.Rows[i]["EXDATE"].ToString().Trim(), "yyyyMMddHHmmss", null);

                        if (dt.Rows[i]["NIBPDATE"].ToString().Equals(dt.Rows[i]["EXDATE"].ToString().Trim()))
                        {
                            SS1_Sheet1.Rows[i].BackColor = Color.LightPink;
                        }

                        SS1_Sheet1.Cells[i, 1].Text  = dt.Rows[i]["PANO"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 2].Text  = dt.Rows[i]["SNAME"].ToString().Trim();
                        
                        SS1_Sheet1.Cells[i, 3].Text  = ExDate.ToString("yyyy-MM-dd");
                        SS1_Sheet1.Cells[i, 4].Text  = ExDate.ToString("HH:mm");

                        if (dt.Rows[i]["NIBPDATE"].ToString().NotEmpty())
                        {
                            SS1_Sheet1.Cells[i, 5].Text = DateTime.ParseExact(dt.Rows[i]["NIBPDATE"].ToString().Trim(), "yyyyMMddHHmmss", null).ToString("HH:mm");
                        }
                        SS1_Sheet1.Cells[i, 5 + 1].Text  = dt.Rows[i]["RESULT_NIBP_SYS"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 6 + 1].Text  = dt.Rows[i]["RESULT_NIBP_DIAS"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 7 + 1].Text  = dt.Rows[i]["RESULT_NIBP_MEAN"].ToString().Trim();

                        SS1_Sheet1.Cells[i, 8 + 1].Text  = dt.Rows[i]["RESULT_ART_S"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 9 + 1].Text  = dt.Rows[i]["RESULT_ART_D"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 10 + 1].Text = dt.Rows[i]["RESULT_ART_M"].ToString().Trim();

                        SS1_Sheet1.Cells[i, 11 + 1].Text = dt.Rows[i]["RESULT_HR"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 12 + 1].Text = dt.Rows[i]["RESULT_RESP"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 13 + 1].Text = dt.Rows[i]["RESULT_TEMP_RECTAL"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 14 + 1].Text = dt.Rows[i]["RESULT_SPO2"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 15 + 1].Text = dt.Rows[i]["RESULT_CVP_M"].ToString().Trim();
                        SS1_Sheet1.Cells[i, 16 + 1].Text = dt.Rows[i]["RESULT_ETCO2"].ToString().Trim();
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

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (rSendInterface == null)
                return;

            Dictionary<string, string> lstVal = new Dictionary<string, string>();
            for (int i = 0; i  < SS1_Sheet1.RowCount; i++)
            {
                if (SS1_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                {
                    lstVal.Clear();
                    if (rdoServer.Checked)
                    {
                        lstVal.Add("측정시간", SS1_Sheet1.Cells[i, 4].Text.Trim()); //측정시간
                    }
                    else
                    {
                        lstVal.Add("측정시간", SS1_Sheet1.Cells[i, 5].Text.Trim()); //측정시간
                    }

                    string strVal = SS1_Sheet1.Cells[i, 5 + 1].Text.Trim();

                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000002018", strVal); //수축기압
                    }

                    strVal = SS1_Sheet1.Cells[i, 6 + 1].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000001765", strVal); //이완기압
                    }

                    strVal = SS1_Sheet1.Cells[i, 7 + 1].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000019150", strVal); //평균기압
                    }

                    strVal = SS1_Sheet1.Cells[i, 8 + 1].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000037255", strVal); //ASBP
                    }

                    strVal = SS1_Sheet1.Cells[i, 9 + 1].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000037256", strVal); //ADBP
                    }

                    strVal = SS1_Sheet1.Cells[i, 10 + 1].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000037990", strVal); //A-MEAN-BP
                    }

                    strVal = SS1_Sheet1.Cells[i, 11 + 1].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000014815", strVal); //PR(HR)
                    }

                    strVal = SS1_Sheet1.Cells[i, 12 + 1].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000002009", strVal); //RR
                    }

                    strVal = SS1_Sheet1.Cells[i, 13 + 1].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000001811_1", strVal); //BT(항문)
                    }

                    strVal = SS1_Sheet1.Cells[i, 14 + 1].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000008708", strVal); //SpO2 (%)
                    }

                    strVal = SS1_Sheet1.Cells[i, 15 + 1].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000030045", strVal); //CVP(cmH2O)
                    }

                    strVal = SS1_Sheet1.Cells[i, 16 + 1].Text.Trim();
                    if (string.IsNullOrWhiteSpace(strVal) == false)
                    {
                        lstVal.Add("I0000031627", strVal); //ETCO2(mmHg)
                    }

                    rSendInterface(lstVal);
                }
            }
            Close();

        }

        private void rdoTime1_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                if (AcpEmr == null || string.IsNullOrWhiteSpace(AcpEmr.ptNo))
                {
                    return;
                }

                READ_DATA();
            }
        }
    }
}
