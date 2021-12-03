using ComBase;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ComMedLibB
{
    public partial class frmMedScreenAllegyResult : Form
    {
        private StringBuilder SQL;
        private DataTable dtAllegy;

        public frmMedScreenAllegyResult(DataTable argDt)
        {
            InitializeComponent();

            dtAllegy = argDt;
        }

        private void frmMedScreenAllegyResult_Load(object sender, EventArgs e)
        {
            SQL = new StringBuilder();

            lblPtno.Text = clsOrdFunction.Pat.PtNo;
            lblSname.Text = clsOrdFunction.Pat.sName;
            lblGenderAge.Text = $"{clsOrdFunction.Pat.Sex} / {clsOrdFunction.Pat.Age}";
            lblPregnant.Text = $"임신:{(clsOrdFunction.Pat.Pregnant == "Y" ? "Y" : "N")}";
            //lblLact.Text = $"수유:{clsOrdFunction.Pat.Lact}";

            //알러지 기왕력 정보 셋팅
            ssAllergyResult_Sheet1.Rows.Count = 0;
            ssAllergyResult_Sheet1.Rows.Count = dtAllegy.Rows.Count;

            ssAllergyResult_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);


            for (int i = 0; i < dtAllegy.Rows.Count; i++)
            {
                ssAllergyResult_Sheet1.Cells[i, 0].Text = dtAllegy.Rows[i]["MODULENM"].ToString();
                ssAllergyResult_Sheet1.Cells[i, 1].Text = dtAllegy.Rows[i]["LVDESC"].ToString();
                ssAllergyResult_Sheet1.Cells[i, 2].Text = dtAllegy.Rows[i]["ORDDRUGNM"].ToString();
                ssAllergyResult_Sheet1.Cells[i, 3].Text = dtAllegy.Rows[i]["SCNMESSAGE"].ToString();

                ssAllergyResult_Sheet1.Cells[i, 5].Text = dtAllegy.Rows[i]["GROUPID"].ToString();
                ssAllergyResult_Sheet1.Cells[i, 6].Text = dtAllegy.Rows[i]["ORDSEQ"].ToString();
                ssAllergyResult_Sheet1.Cells[i, 7].Text = dtAllegy.Rows[i]["LVDESC"].ToString();
                ssAllergyResult_Sheet1.Cells[i, 8].Text = dtAllegy.Rows[i]["ORDERDRUGCD"].ToString();
            }

            //셋팅내용 팝업 확인 하였는지 체크하여서 이미 확인한것들은 스킵 로직
            //위의 프로시저에서 나온 검토결과와 이미 팝업을 확인을 하였는지 확인한다.
            string SqlErr = "";
            DataTable dt = null;
            for (int i = 0; i < ssAllergyResult_Sheet1.Rows.Count; i++)
            {
                SQL.Clear();
                SQL.AppendLine("  SELECT *                                                          ");
                SQL.AppendLine("  FROM ADMIN.DUR_ALLERGY_SAYU                                  ");
                SQL.AppendLine($"  WHERE SABUN = '{clsType.User.Sabun}'                             ");
                //SQL.AppendLine($"  AND BDATE = TO_DATE('{resultInfo.m_strDpPrscYYMMDD}','YYYYMMDD') ");
                SQL.AppendLine($"  AND DEPTCODE = '{clsOrdFunction.Pat.DeptCode}'                   ");
                SQL.AppendLine($"  AND PTNO = '{clsOrdFunction.Pat.PtNo}'                           ");
                SQL.AppendLine($"  AND ORDERDRUGCD = '{ssAllergyResult_Sheet1.Cells[i, 8].Text}'    ");
                SQL.AppendLine($"  AND SCNMESSAGE = '{ssAllergyResult_Sheet1.Cells[i, 3].Text}'     ");
                SQL.AppendLine("   ORDER BY BDATE DESC                                              ");
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString(), clsDB.DbCon);

                if (dt.Rows.Count >= 1)
                {
                    ssAllergyResult_Sheet1.RemoveRows(i, 1);
                }
            }

            for (int i = 0; i < ssAllergyResult_Sheet1.Columns.Count; i++)
            {
                ssAllergyResult_Sheet1.Columns[i].Width = ssAllergyResult_Sheet1.GetPreferredColumnWidth(i, true, true, true, true);
            }

            ssAllergyResult_Sheet1.Columns[4].Width = 150;

            //위의 체크로직에서 스프레드 로우가 0 -> 다 이미 팝업 확인 하였음 이면 창을 닫고 프로세스 그대로 간다.
            if (ssAllergyResult_Sheet1.Rows.Count == 0)
            {
                clsDur.gstrDur_알러지취소 = "";
                this.Close();
                return;
            }

            //환자알러지 등록정보
            READ_ALLERGYNEW();

            //숨김
            ssAllergyResult_Sheet1.Columns[5, 8].Visible = false;
        }

        /// <summary>
        /// 환자알러지 등록정보
        /// </summary>
        private void READ_ALLERGYNEW()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT PANO, SNAME, CODE, TO_CHAR(ENTDATE, 'YYYY-MM-DD HH24:MI:SS') ENTDATE, REMARK, SABUN, DAMCD, DAMTYPE, RMK, DAMTYPENM, DAMDESC, DAMDESCKR";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.ETC_ALLERGY_MST";
            SQL = SQL + ComNum.VBLF + "WHERE PANO = '" + lblPtno.Text + "' ";
            SQL = SQL + ComNum.VBLF + "AND CODE = '100' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY ENTDATE DESC  ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            ssAllergyView_Sheet1.Rows.Count = 0;
            ssAllergyView_Sheet1.Rows.Count = dt.Rows.Count;
            ssAllergyView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssAllergyView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DAMDESC"].ToString();
                ssAllergyView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["DAMDESCKR"].ToString();
                ssAllergyView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DAMTYPENM"].ToString();
                ssAllergyView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["RMK"].ToString();
                ssAllergyView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ENTDATE"].ToString();
                ssAllergyView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["SABUN"].ToString();
                ssAllergyView_Sheet1.Cells[i, 6].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["SABUN"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            for (int i = 0; i < ssAllergyView_Sheet1.Columns.Count; i++)
            {
                ssAllergyView_Sheet1.Columns[i].Width = ssAllergyView_Sheet1.GetPreferredColumnWidth(i, true, true, true, true);
            }

            ssAllergyView_Sheet1.Columns[5].Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //한번 팝업 확인한 알러지들을 다시 팝업 띄우지 않기 위한 로직, 사유 또한 선택사항으로 데이터 저장
            string SqlErr = "";
            DataTable dt = null;

            //현재는 로드 조회시에 이미 팝업 확인한 것은 안뜨게 되어있음
            for (int i = 0; i < ssAllergyResult_Sheet1.Rows.Count; i++)
            {
                SQL.Clear();
                SQL.AppendLine("  SELECT *                                                          ");
                SQL.AppendLine("  FROM ADMIN.DUR_ALLERGY_SAYU                                  ");
                SQL.AppendLine($"  WHERE SABUN = '{clsType.User.Sabun}'                             ");
                //SQL.AppendLine($"  AND BDATE = TO_DATE('{resultInfo.m_strDpPrscYYMMDD}','YYYYMMDD') ");
                SQL.AppendLine($"  AND DEPTCODE = '{clsOrdFunction.Pat.DeptCode}'                   ");
                SQL.AppendLine($"  AND PTNO = '{clsOrdFunction.Pat.PtNo}'                           ");
                SQL.AppendLine($"  AND ORDERDRUGCD = '{ssAllergyResult_Sheet1.Cells[i, 8].Text}'    ");
                SQL.AppendLine("   ORDER BY BDATE DESC                                              ");
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString(), clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    continue;
                }
                else if (dt.Rows.Count == 0)
                {
                    int nRowAffected = 0;

                    clsDB.setBeginTran(clsDB.DbCon);

                    SQL.Clear();
                    SQL.AppendLine(" INSERT INTO ADMIN.DUR_ALLERGY_SAYU                        ");
                    SQL.AppendLine(" (SABUN, PTNO, DEPTCODE, BDATE, GROUPID,                        ");
                    SQL.AppendLine(" ORDSEQ, LVDESC, ORDERDRUGCD, ORDDRUGNM, SCNMESSAGE,            ");
                    SQL.AppendLine(" SAYU, INPUTIP, INPUTDATETIME)                                  ");
                    SQL.AppendLine($" VALUES('{clsType.User.Sabun}',                                ");
                    SQL.AppendLine($" '{clsOrdFunction.Pat.PtNo}',                                  ");
                    SQL.AppendLine($" '{clsOrdFunction.Pat.DeptCode}',                              ");
                    SQL.AppendLine($" TO_DATE('{clsDur.DurPrescription.PrscPresDt}','YYYYMMDD'),    ");
                    SQL.AppendLine($" '{ssAllergyResult_Sheet1.Cells[i, 5].Text}',                  ");
                    SQL.AppendLine($" '{ssAllergyResult_Sheet1.Cells[i, 6].Text}',                  ");
                    SQL.AppendLine($" '{ssAllergyResult_Sheet1.Cells[i, 7].Text}',                  ");
                    SQL.AppendLine($" '{ssAllergyResult_Sheet1.Cells[i, 8].Text}',                  ");
                    SQL.AppendLine($" '{ssAllergyResult_Sheet1.Cells[i, 2].Text}',                  ");
                    SQL.AppendLine($" '{ssAllergyResult_Sheet1.Cells[i, 3].Text}',                  ");  //사유 선택사항
                    SQL.AppendLine($" '{ssAllergyResult_Sheet1.Cells[i, 4].Text}',                  ");  //사유 선택사항
                    SQL.AppendLine($" '{clsCompuInfo.gstrCOMIP}',                                   ");
                    SQL.AppendLine($" sysdate)                                                      ");

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString(), ref nRowAffected, clsDB.DbCon);

                    clsDB.setCommitTran(clsDB.DbCon);
                }
            }

            clsDur.gstrDur_알러지취소 = "";
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            clsDur.gstrDur_알러지취소 = "취소";
            this.Close();
        }
    }
}
