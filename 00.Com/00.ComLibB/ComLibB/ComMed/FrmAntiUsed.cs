using ComBase; //기본 클래스
using ComBase.Controls;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Description : 기간별 항생제 사용내역
/// Author : 이상훈
/// Create Date : 2017.07.25
/// </summary>
/// <history>
/// </history>
/// <seealso cref="FrmAntiUSED.frm"/>

namespace ComLibB
{
    public partial class FrmAntiUsed : Form
    {
        string SQL;
        DataTable dt = null;
        string SqlErr = "";

        string strPano;
        string strName;
        string strSex;
        string strAge;
        string strInDate;
        string strDept;
        string strRoom;
        string strIlsu;
        string strGubun;
        string strDrName;


        public FrmAntiUsed(string sPano, string sName, string sSex, string sAge, string sInDate, string sDept, string sRoom, string sIlsu, string sGubun, string sDrName)
        {
            strPano = sPano;
            strName = sName;
            strSex = sSex;
            strAge = sAge;
            strInDate = sInDate;
            strDept = sDept;
            strRoom = sRoom;
            strIlsu = sIlsu;
            strGubun = sGubun;
            strDrName = sDrName;

            InitializeComponent();
        }

        public FrmAntiUsed()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string strCode = "";
            int j=0;

            using (clsSpread SP = new clsSpread())
            {
                SP.Spread_All_Clear(ssAntiUsed);
            }

            if (strPano == "") return;
            if (strPano == "03724560") return; //2013-12-04 김태진님 입원기간많아 제외

            #region 약국일경우 처리
            if (clsType.User.JobGroup.Left(6).Equals("JOB011"))
            {
                panDrug.Visible = true;
                strInDate = dtpFrDate.Value.ToString("yyyy-MM-dd");
                strPano = txtPano.Text.Trim().PadLeft(8, '0');
                label1.Text = " 항생제 사용내역(입원, 응급실)";
            }
            #endregion

            try
            {
                SQL = "";
                SQL += " SELECT to_char(A.BDATE, 'yyyy-MM-dd') BDATE                                                    \r";
                SQL += "      , A.SUCODE                                                                                \r";
                SQL += "      , B.SUNAMEK                                                                               \r";
                SQL += "      , C.GBN                                                                                   \r";
                SQL += "      , (SELECT TRIM(DRNAME) FROM KOSMOS_OCS.OCS_DOCTOR WHERE SABUN = A.DRCODE) AS  DRNAME      \r";
                SQL += "      , decode(A.GBIOE, null, null, '응급실') GBIOE                                              \r";
                SQL += "      , SUM(A.QTY*A.NAL) nal                                                                    \r";
                SQL += "   FROM KOSMOS_OCS.OCS_IORDER A                                                                 \r";
                SQL += "      , KOSMOS_PMPA.BAS_SUN   B                                                                 \r";
                SQL += "      , KOSMOS_PMPA.NUR_AST   C                                                                 \r";
                SQL += "  WHERE A.BDATE >= TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')       \r";
                SQL += "    AND A.BDATE <= TO_DATE('" + dtpToDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')       \r";
                SQL += "    AND A.BDATE >= TO_DATE('" + strInDate + "','YYYY-MM-DD')                                    \r";
                ///TODO : 이상훈 항생제 조건 통합 필요
                //SQL += "    AND TRIM(a.SuCode) IN ( SELECT TRIM(CODE) FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='OCS_장기항생제코드' AND (DELDATE IS NULL OR DELDATE ='')  )  \r";
                //SQL += "    AND TRIM(a.SuCode) IN ( SELECT TRIM(CODE) FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='OCS_항생제코드' AND (DELDATE IS NULL OR DELDATE ='')  )  \r 21-06-24주석";
                //2021-06-24 조회 기준 약품마스터로 변경
                SQL += "    AND A.SUCODE IN ( SELECT JEPCODE FROM KOSMOS_ADM.DRUG_MASTER2 WHERE SUB  IN (02, 07))       \r";
                SQL += "    AND A.PTNO = '" + strPano + "'                                                              \r";
                SQL += "    AND A.SUCODE = B.SUNEXT                                                                     \r";
                SQL += "    AND A.ORDERCODE = C.ORDERCODE(+)                                                            \r";
                SQL += "    AND A.PTNO = C.PTNO(+)                                                                      \r";
                SQL += "  GROUP BY  A.SUCODE, A.BDATE, A.DRCODE, A.GBIOE, A.STAFFID, B.SUNAMEK, C.GBN                   \r";
                SQL += " HAVING SUM(A.QTY * A.NAL) > 0                                                                  \r";
                SQL += " ORDER BY A.BDATE DESC, A.SUCODE                                                                \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt, ssAntiUsed, 0, true);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (strCode != dt.Rows[i]["SUCODE"].ToString().Trim())
                        {
                            j += 1;
                            strCode = dt.Rows[i]["SUCODE"].ToString().Trim();
                        }
                        if ((j % 2) == 0)
                        {
                            ssAntiUsed.ActiveSheet.Cells[i, 0, i, ssAntiUsed.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(240, 240, 240);
                        }
                        else
                        {
                            ssAntiUsed.ActiveSheet.Cells[i, 0, i, ssAntiUsed.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                        }
                    }
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void FrmAntiUsed_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = strPano;
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = strName;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = strAge + "/" + strSex;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = strInDate;
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = strDept;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = strRoom;
            ssPatInfo.ActiveSheet.Cells[0, 6].Text = strIlsu;
            ssPatInfo.ActiveSheet.Cells[0, 7].Text = strGubun;
            ssPatInfo.ActiveSheet.Cells[0, 8].Text = strDrName;

            ComFunc.ReadSysDate(clsDB.DbCon);
            
            dtpFrDate.Text = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-15).ToString("yyyy-MM-dd");
            dtpToDate.Text = clsPublic.GstrSysDate;
            
            btnSearch_Click(btnSearch, e);
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            using (FrmAntiUSED2 f = new FrmAntiUSED2())
            {
                f.ShowDialog();
            }
        }
    }
}
