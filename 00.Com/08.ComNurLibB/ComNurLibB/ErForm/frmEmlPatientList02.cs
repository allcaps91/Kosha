using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;


namespace ComNurLibB
{

    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김현욱
    /// Create Date     : 2019-09-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// 

    public partial class frmEmlPatientList02 : Form
    {
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        //string FstrREAD = "";

        public frmEmlPatientList02()
        {
            InitializeComponent();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            int i = 0;
            string[] colHeader = null;


            colHeader = new string[SS1_Sheet1.ColumnCount];

            for (i = 0; i < SS1_Sheet1.ColumnCount; i++)
            {
                colHeader[i] = SS1_Sheet1.ColumnHeader.Cells[0, i].Text.Trim();
            }

            SS1_Sheet1.AddRows(0, 1);

            for (i = 0; i < SS1_Sheet1.ColumnCount; i++)
            {
                SS1_Sheet1.Cells[0, i].Text = colHeader[i];
            }

            clsSpread CS = new clsSpread();
            CS.ExportToXLS(SS1);
            CS = null;

            SS1_Sheet1.RemoveRows(0, 1);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return;//권한 확인
            //}

            READ_DATA();
        }

        private void FrmEmlPationtListNew_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            SS1_Sheet1.RowCount = 2;
            //TxtSDATE.Text = Convert.ToDateTime(strDTP).AddDays(-20).ToString("yyyy-MM-dd");
            TxtSDATE.Text = strDTP;
            TxtEDATE.Text = strDTP;
            //TxtSDATE.Text = "2018-07-01";
            //TxtEDATE.Text = "2019-06-30";
            //cboWARD.Items.Add("전체");
            //cboWARD.Items.Add("33");
            //cboWARD.Items.Add("65");
            cboWARD.SelectedIndex = 0;

            SS1_Sheet1.RowCount = 2;

        }

        private void READ_DATA()
        {
            int i = 0;
            int k = 0;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strPTMIIDNO = "";
            string strPTMIINDT = "";
            string strPTMIINTM = "";

            string strINDT1 = "";       //응급실 내원일자 FROM
            string strINDT2 = "";       //응급실 내원일자 TO

            string strDept1 = "";       //병원 진료과 코드
            string strDept2 = "";       //NEDIS 진료과 코드

            SS1_Sheet1.RowCount = 2;

            strINDT1 = TxtSDATE.Text.Trim();
            strINDT2 = TxtEDATE.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "   SELECT ";
                SQL += ComNum.VBLF + "        WARD, ROOM, ER, DEPT, ";
                SQL += ComNum.VBLF + "        SNAME, PTNO, EINDATE, EINTIME, ";
                SQL += ComNum.VBLF + "        EOUTDATE, EOUTTIME, WINDATE, WINTIME, ";
                SQL += ComNum.VBLF + "        WOUTDATE, WOUTTIME, WPATH, IINDATE, ";
                SQL += ComNum.VBLF + "        IINTIME, IOUTDATE, IOUTTIME, IPATH, ";
                SQL += ComNum.VBLF + "        DEPT1, DEPT2, ETCPATH, REMARK, ";
                SQL += ComNum.VBLF + "        (SELECT KTASLEVL FROM KOSMOS_PMPA.NUR_ER_PATIENT ";
                SQL += ComNum.VBLF + "                        WHERE PANO = A.PTNO ";
                SQL += ComNum.VBLF + "                          AND INTIME = TO_DATE(TRIM(NVL(A.EINDATE,'1900-01-01')) || ' ' || TRIM(NVL(A.EINTIME,'00:00')),'YYYY-MM-DD HH24:MI') ";
                SQL += ComNum.VBLF + "                          AND ROWNUM = 1 ) KTAS1 ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_SPBOOK_2020 A ";
                if (rb1.Checked == true)
                {
                    SQL += ComNum.VBLF + "    WHERE  EINDATE >= '" + strINDT1 + "' AND EINDATE <= '" + strINDT2 + "' ";
                }
                else if (rb2.Checked == true)
                {
                    SQL += ComNum.VBLF + "    WHERE  WINDATE >= '" + strINDT1 + "' AND WINDATE <= '" + strINDT2 + "' ";
                }
                else if (rb3.Checked == true)
                {
                    SQL += ComNum.VBLF + "    WHERE  IINDATE >= '" + strINDT1 + "' AND IINDATE <= '" + strINDT2 + "' ";
                }

                if (cboWARD.Text.Trim() == "65")
                {
                    SQL += ComNum.VBLF + "    AND WARD = '65' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND WARD = '33' ";
                }

                if (rb1.Checked == true)
                {
                    SQL += ComNum.VBLF + " ORDER BY EINDATE, EINTIME, PTNO ";
                }
                else if (rb2.Checked == true)
                {
                    SQL += ComNum.VBLF + " ORDER BY WINDATE, WINTIME, PTNO ";
                }
                else if (rb3.Checked == true)
                {
                    SQL += ComNum.VBLF + " ORDER BY IINDATE, IINTIME, PTNO ";
                }


                
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS1_Sheet1.Rows.Count = dt.Rows.Count + 2;
                    //SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        k = i + 2;
                        strPTMIIDNO = dt.Rows[i]["PTNO"].ToString().Trim();
                        strPTMIINDT = dt.Rows[i]["EINDATE"].ToString().Trim();
                        strPTMIINTM = dt.Rows[i]["EINTIME"].ToString().Trim();

                        strDept1 = dt.Rows[i]["DEPT1"].ToString().Trim();
                        strDept2 = dt.Rows[i]["DEPT2"].ToString().Trim();


                        SS1_Sheet1.Cells[k, 0].Text = (i + 1).ToString();

                        SS1_Sheet1.Cells[k, 1].Text = dt.Rows[i]["ROOM"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 2].Text = dt.Rows[i]["ER"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 3].Text = dt.Rows[i]["DEPT"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 4].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 5].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 6].Text = dt.Rows[i]["EINDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 7].Text = dt.Rows[i]["EINTIME"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 8].Text = dt.Rows[i]["EOUTDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 9].Text = dt.Rows[i]["EOUTTIME"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 10].Text = dt.Rows[i]["WINDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 11].Text = dt.Rows[i]["WINTIME"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 12].Text = dt.Rows[i]["WOUTDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 13].Text = dt.Rows[i]["WOUTTIME"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 14].Text = dt.Rows[i]["WPATH"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 15].Text = dt.Rows[i]["IINDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 16].Text = dt.Rows[i]["IINTIME"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 17].Text = dt.Rows[i]["IPATH"].ToString().Trim();
                        if (strDept1 != "")
                        {
                            SS1_Sheet1.Cells[k, 18].Text = strDept1;
                            SS1_Sheet1.Cells[k, 19].Text = CHANGE_DEPTCODE(strDept1);
                        }
                        else
                        {
                            SS1_Sheet1.Cells[k, 18].Text = CHANGE_DEPTCODE2(strDept2);
                            SS1_Sheet1.Cells[k, 19].Text = strDept2;
                        }
                        SS1_Sheet1.Cells[k, 20].Text = dt.Rows[i]["IOUTDATE"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 21].Text = dt.Rows[i]["IOUTTIME"].ToString().Trim();
                        SS1_Sheet1.Cells[k, 22].Text = dt.Rows[i]["KTAS1"].ToString().Trim();
                        //SS1_Sheet1.Cells[k, 22].Text = KTAS
                    }

                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        private string CHANGE_DEPTCODE(string argDEPTCODE)
        {

            string strDEPT = "";

            switch (argDEPTCODE)
            {

                case "MI":
                    strDEPT = "AF";
                    break;
                case "MO":
                    strDEPT = "AG";
                    break;
                case "MG":
                    strDEPT = "AC";
                    break;
                case "MC":
                    strDEPT = "AA";
                    break;
                case "MP":
                    strDEPT = "AB";
                    break;
                case "ME":
                    strDEPT = "AE";
                    break;
                case "MN":
                    strDEPT = "AD";
                    break;
                case "MR":
                    strDEPT = "AI";
                    break;
                case "MD":
                    strDEPT = "AX";
                    break;
                case "GS":
                    strDEPT = "BA";
                    break;
                case "NS":
                    strDEPT = "BB";
                    break;
                case "OS":
                    strDEPT = "BD";
                    break;
                case "OG":
                    strDEPT = "CA";
                    break;
                case "CS":
                    strDEPT = "BC";
                    break;
                case "PD":
                    strDEPT = "DA";
                    break;
                case "NP":
                    strDEPT = "EA";
                    break;
                case "OT":
                    strDEPT = "GA";
                    break;
                case "EN":
                    strDEPT = "HA";
                    break;
                case "UR":
                    strDEPT = "IA";
                    break;
                case "ER":
                    strDEPT = "JA";
                    break;
                case "DM":
                    strDEPT = "LA";
                    break;
                case "DT":
                    strDEPT = "NA";
                    break;
                case "NE":
                    strDEPT = "FA";
                    break;
                case "RM":
                    strDEPT = "MA";
                    break;
                default:
                    strDEPT = "XX";
                    break;

            }

            return strDEPT;

        }

        private string CHANGE_DEPTCODE2(string argDEPTCODE)
        {

            string strDEPT = "";

            switch (argDEPTCODE)
            {

                case "AF":
                    strDEPT = "MI";
                    break;
                case "AG":
                    strDEPT = "MO";
                    break;
                case "AC":
                    strDEPT = "MG";
                    break;
                case "AA":
                    strDEPT = "MC";
                    break;
                case "AB":
                    strDEPT = "MP";
                    break;
                case "AE":
                    strDEPT = "ME";
                    break;
                case "AD":
                    strDEPT = "MN";
                    break;
                case "AI":
                    strDEPT = "MR";
                    break;
                case "AX":
                    strDEPT = "MD";
                    break;
                case "BA":
                    strDEPT = "GS";
                    break;
                case "BB":
                    strDEPT = "NS";
                    break;
                case "BD":
                    strDEPT = "OS";
                    break;
                case "CA":
                    strDEPT = "OG";
                    break;
                case "BC":
                    strDEPT = "CS";
                    break;
                case "DA":
                    strDEPT = "PD";
                    break;
                case "EA":
                    strDEPT = "NP";
                    break;
                case "GA":
                    strDEPT = "OT";
                    break;
                case "HA":
                    strDEPT = "EN";
                    break;
                case "IA":
                    strDEPT = "UR";
                    break;
                case "JA":
                    strDEPT = "ER";
                    break;
                case "LA":
                    strDEPT = "DM";
                    break;
                case "NA":
                    strDEPT = "DT";
                    break;
                case "MA":
                    strDEPT = "RM";
                    break;
                case "FA":
                    strDEPT = "NE";
                    break;
                default:
                    strDEPT = "??";
                    break;

            }

            return strDEPT;

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            //string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            { }

                //strTitle = "";

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, false, true, true, false, false, true);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void cboWARD_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}

