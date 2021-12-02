using ComBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBaseRRSDetail : Form
    {
        EmrPatient pAcp = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emrPatient">환자 정보</param>
        public frmEmrBaseRRSDetail(EmrPatient emrPatient)
        {
            pAcp = emrPatient;
            InitializeComponent();
        }

        private void frmEmrBaseRRSDetail_Load(object sender, EventArgs e)
        {
            SetRRS();
        }

        private void SetRRS()
        {
            #region 변수
            string SQL = FormPatInfoQuery.Query_FormPatInfo_RRSSCORE(pAcp);

            DataTable dt = null;
            int Score = 0;
            #endregion

            try
            {
                string SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                #region 아이템, 로우 매핑
                Dictionary<string, int> keyValues = new Dictionary<string, int>
                {
                    { "I0000014815", 1 },
                    { "I0000002018", 2 },
                    { "I0000002009", 3 },
                    { "I0000001811", 4 },
                    { "I0000037778", 5 }
                };
                #endregion

                //스코어에 따른 칼럼 매핑
                int ScoreCol = -1;
                Font BoldFont = new Font("굴림체", 10, FontStyle.Bold);
                #region 점수 계산
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    double Val = VB.Val(dt.Rows[i]["ITEMVALUE"].ToString().Trim());
                    #region 맥박 점수
                    if (dt.Rows[i]["ITEMNO"].ToString().Equals("I0000014815"))
                    {
                        if (Val <= 40)
                        {
                            Score += 2;
                            ScoreCol = 2;
                        }
                        else if (Val >= 41 && Val <= 50)
                        {
                            Score += 1;
                            ScoreCol = 3;
                        }
                        else if (Val >= 51 && Val <= 100)
                        {
                            Score += 0;
                            ScoreCol = 4;
                        }
                        else if (Val >= 101 && Val <= 110)
                        {
                            Score += 1;
                            ScoreCol = 5;
                        }
                        else if (Val >= 111 && Val <= 130)
                        {
                            Score += 2;
                            ScoreCol = 6;
                        }
                        else if (Val >= 131)
                        {
                            Score += 3;
                            ScoreCol = 7;
                        }
                    }
                    #endregion

                    #region 수축기 혈압 점수
                    if (dt.Rows[i]["ITEMNO"].ToString().Equals("I0000002018"))
                    {
                        if (Val <= 70)
                        {
                            Score += 3;
                            ScoreCol = 1;
                        }
                        else if (Val >= 71 && Val <= 80)
                        {
                            Score += 2;
                            ScoreCol = 2;
                        }
                        else if (Val >= 81 && Val <= 100)
                        {
                            Score += 1;
                            ScoreCol = 3;
                        }
                        else if (Val >= 101 && Val <= 199)
                        {
                            Score += 0;
                            ScoreCol = 4;
                        }
                        else if (Val >= 200)
                        {
                            Score += 2;
                            ScoreCol = 6;
                        }

                    }
                    #endregion

                    #region 호흡수 점수
                    if (dt.Rows[i]["ITEMNO"].ToString().Equals("I0000002009"))
                    {
                        if (Val >= 30)
                        {
                            Score += 3;
                            ScoreCol = 7;
                        }
                        else if (Val <= 8)
                        {
                            Score += 2;
                            ScoreCol = 2;
                        }
                        else if (Val >= 9 && Val <= 14)
                        {
                            Score += 0;
                            ScoreCol = 4;
                        }
                        else if (Val >= 15 && Val <= 20)
                        {
                            Score += 1;
                            ScoreCol = 5;
                        }
                        else if (Val >= 21 && Val <= 29)
                        {
                            Score += 2;
                            ScoreCol = 6;
                        }
                    }
                    #endregion

                    #region 체온 점수
                    if (dt.Rows[i]["ITEMNO"].ToString().Equals("I0000001811"))
                    {
                        if (Val <= 35)
                        {
                            Score += 2;
                            ScoreCol = 2;
                        }
                        else if (Val >= 35.1 && Val <= 36.0 )
                        {
                            Score += 1;
                            ScoreCol = 3;
                        }
                        else if (Val >= 36.1 && Val <= 37.4)
                        {
                            Score += 0;
                            ScoreCol = 4;
                        }
                        else if (Val >= 37.5)
                        {
                            Score += 1;
                            ScoreCol = 5;
                        }
                    }
                    #endregion

                    #region 의식수준 점수(AVPU SCORE)
                    if (dt.Rows[i]["ITEMNO"].ToString().Equals("I0000037778"))
                    {
                        switch (dt.Rows[i]["ITEMVALUE"].ToString().Trim())
                        {
                            case "Alert": //정상
                                Score += 0;
                                ScoreCol = 4;
                                break;
                            case "Verbal Response": //목소리 반응
                                Score += 1;
                                ScoreCol = 5;
                                break;
                            case "Pain Response": // 통증 반응
                                Score += 2;
                                ScoreCol = 6;
                                break;
                            case "Unconsciousness": //무반응
                                Score += 3;
                                ScoreCol = 7;
                                break;
                        }
                    }
                    #endregion

                    int nRow = -1;
                    if (keyValues.TryGetValue(dt.Rows[i]["ITEMNO"].ToString().Trim(), out nRow) && nRow > -1)
                    {
                        ssView_Sheet1.Cells[nRow, ScoreCol].Font = BoldFont;
                    }
                }
                #endregion

                ssView_Sheet1.Cells[6, 1].Text = Score.ToString();

                #region 색상
                if (Score >= 0 && Score <= 4)
                {
                    ssView_Sheet1.Rows[6].BackColor = Color.LightGreen;
                }
                else if (Score >= 5 && Score <= 6)
                {
                    ssView_Sheet1.Rows[6].BackColor = Color.Orange;
                }
                else if (Score >= 7)
                {
                    ssView_Sheet1.Rows[6].BackColor = Color.Red;
                }
                #endregion
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }
    }
}
