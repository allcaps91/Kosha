using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComMedLibB
{
    public partial class FrmMedDrgSurvey : Form
    {
        clsSpread SP = new clsSpread();

        string SQL;
        int rowcounter;
        string FstrROWID = "";
        string FstrRowid2 = "";
        string FstrTRSNO = "";
        string FstrIPDNO = "";
        string FstrPANO = "";
        string FstrFlag = "";

        DataTable dt = null;
        string SqlErr = ""; //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        public FrmMedDrgSurvey()
        {
            InitializeComponent();
        }

        public FrmMedDrgSurvey(string strPtno)
        {
            InitializeComponent();
            FstrPANO = strPtno;
        }

        /// <summary>
        /// 입원 오더 전송중 사용하는 생성자
        /// </summary>
        /// <param name="strPtno"></param>
        /// <param name="strFlag"></param>
        public FrmMedDrgSurvey(string strPtno, string strFlag)
        {
            InitializeComponent();
            FstrPANO = strPtno;
            FstrFlag = strFlag;

            //폼 못닫게
            this.ControlBox = false;    
            btnExit.Enabled = false;
            lblOrderSend.Visible = true;
        }

        private void FrmMedDrgSurvey_Load(object sender, EventArgs e)
        {
            int j = 0;
            //this.Location = new Point(20, 20);

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            fn_TabBackColor_Set();

            fn_HelpLoad();

            fn_Screen_Clear();
            fn_ListView();

            if (ssList.ActiveSheet.NonEmptyRowCount > 0)
            {
                CellClickEventArgs CellClick = new CellClickEventArgs(new SpreadView(), j, 0, j, 0, new MouseButtons(), false, false);
                ssList_CellClick(ssList, CellClick);
            }
        }

        private void fn_TabBackColor_Set()
        {
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.White;
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.White;

            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.White;
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.White;
        }

        void fn_HelpLoad()
        {
            rtxtHelp.Text = "◈서식 작성요령" + "\r\n\r\n";
            rtxtHelp.Text += "===========================================================================================================================================" + "\r";
            rtxtHelp.Text += "1.수술 전 진료의 점검사항" + "\r";
            rtxtHelp.Text += "===========================================================================================================================================" + "\r";
            rtxtHelp.Text += "" + "\r";
            rtxtHelp.Text += " 1.1 수술 전 검사 시행 여부 및 마취종류" + "\r";
            rtxtHelp.Text += "  마취 시행전 수술전 검사를 시행한 경우 시행에 표시하고, 마취 유형은 수술전 검사 시행여부와 무관하게 반드시 표시" + "\r";
            rtxtHelp.Text += "     1) 전신마취" + "\r";
            rtxtHelp.Text += "     2) 부위마취(척추마취 및 기타 부위마취 포함)" + "\r";
            rtxtHelp.Text += "     3) 국소마취" + "\r";
            rtxtHelp.Text += "  ※ 마취 및 질병군별 수술전 검사항목은 「7개 질병군 포괄수가 급여적정성 평가기준」참조" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "===========================================================================================================================================" + "\r";
            rtxtHelp.Text += "2.입원 중 진료의 점검사항" + "\r";
            rtxtHelp.Text += "===========================================================================================================================================" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "2.1 입원중에 일어난 사고" + "\r";
            rtxtHelp.Text += "  다음의 경우 있음에 표시" + "\r";
            rtxtHelp.Text += "     1) 불의의 병원 내 물리적 사고(낙상 등)" + "\r";
            rtxtHelp.Text += "       - 입원 원인 질병과의 관련성 혹은 상해의 정도와는 상관없이 「물리적 사고」그 자체가 병원의 질적 문제에 속하므로 병원 내 발생한 모든 물리적 사고" + "\r";
            rtxtHelp.Text += "     2) 수혈사고" + "\r";
            rtxtHelp.Text += "       - 환자가 바뀌거나, 이형을 수혈하는 등 부적합 혈액을 투여" + "\r";
            rtxtHelp.Text += "     3) 투약사고" + "\r";
            rtxtHelp.Text += "       - 환자 또는 약물이 바뀌거나, 투약방법(경구, 주사제 등)이 잘못된 경우" + "\r";
            rtxtHelp.Text += "     4) 마취사고" + "\r";
            rtxtHelp.Text += "       - 마취와 관련된 부작용으로(외과적 시술에 따른 부작용은 제외) 환자의 이환이나 사망의 가능성을 증가시키는 모든 상황을 포함" + "\r";
            rtxtHelp.Text += "       - 폐렴 및 마취부위의 염증 등 감염과 관련된 부분은 제외" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "   예시) 전신마취후 발생한 호흡장애 A3" + "\r";
            rtxtHelp.Text += "  (code)" + "\r";
            rtxtHelp.Text += "   A 전신마취           B 부위마취           B 국소마취" + "\r";
            rtxtHelp.Text += "   1 중추신경계(경련, 마비, 의식장애 등)" + "\r";
            rtxtHelp.Text += "   2 순환계(부정맥, 저혈압, 심장정지 등)" + "\r";
            rtxtHelp.Text += "  3 호흡계(후두경련, 호흡장애 등)" + "\r";
            rtxtHelp.Text += "   4 과민반응(Anaphylaxis)" + "\r";
            rtxtHelp.Text += "   5 국소합병증(혈종, 손상 등)" + "\r";
            rtxtHelp.Text += "   6 기타 부작용" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "  2.2 감염증" + "\r";
            rtxtHelp.Text += "   다음의 경우 있음에 표시" + "\r";
            rtxtHelp.Text += "   ○ 감염은 “입원당시 나타나지 않았음은 물론 잠복상태도 아니었던 감염이 입원 기간중 발생한 경우”로 정의함" + "\r";
            rtxtHelp.Text += "   < 의료관련 감염 >" + "\r";
            rtxtHelp.Text += "    - 입원당시 나타나지 않았음은 물론 잠복상태도 아니었던 감염이 입원기간 중 발생한 경우로 입원 후 48시간 이후 다음 중 하나라도 해당되는 경우" + "\r";
            rtxtHelp.Text += "       ① 체온 38.3℃ 이상(2일 이상 지속된 경우) " + "\r";
            rtxtHelp.Text += "       ② 고름 등 화농성 유출(purulent discharge) " + "\r";
            rtxtHelp.Text += "       ③ 농뇨 " + "\r";
            rtxtHelp.Text += "       ④ 미생물 배양검사(혈액, 뇨, 분비믈 등) 양성" + "\r";
            rtxtHelp.Text += "    ※ 범복막염을 동반한 급성충수염(K352)은 제외" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "   < 수술부위 감염 >" + "\r";
            rtxtHelp.Text += "    다음 중 하나 이상에 해당하는 경우" + "\r";
            rtxtHelp.Text += "     - 절개부위 또는 심부에 위치한  드레인에서 농성배액이 있는 경우" + "\r";
            rtxtHelp.Text += "     - 절개부위 또는 심부, 기관에서 무균적으로 채취한 검체의 배양에서 균이 분리된 경우" + "\r";
            rtxtHelp.Text += "     - 38.3℃ 이상의 발열, 국소동통, 압통, 발적 등 감염증상 중 하나이상의 증상이 있고," + "\r";
            rtxtHelp.Text += "       수술창상의 심부가 저절로 파열되거나 의사가 개방한 경우" + "\r";
            rtxtHelp.Text += "     - 조직병리검사, 방사선검사 등에서 심부절개부위 또는 기관이나 강의 농양이나 감염증거 관찰된 경우" + "\r";
            rtxtHelp.Text += "       (수술중 채취된 조직의 병리검사는 해당 안됨)" + "\r";
            rtxtHelp.Text += "     - 수술의, 주치의 또는 감염내과의사에 의한 수술 부위 감염 진단시" + "\r";
            rtxtHelp.Text += "     - 수정체 수술의 경우 수술후 기본처치 이외의 추가적인 약물 혹은 수술치료가 필요한 급성안내염(acute endophthalmitis)" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "  2.3 수술 합병증 및 부작용" + "\r";
            rtxtHelp.Text += "   다음 해당 합병증이 있는 경우 있음에 표시 후 code 기재" + "\r";
            rtxtHelp.Text += "    < 출혈 >" + "\r";
            rtxtHelp.Text += "    - 재수술이 필요한 출혈, 지혈을 위한 시술(창상봉합술, 혈관결찰술, 전혈 또는 농축적혈구 4pint이상의 수혈 등) 및" + "\r";
            rtxtHelp.Text += "      처치가 필요한 출혈(지연일차봉합, 빈혈로 인한 수혈 등은 제외)" + "\r";
            rtxtHelp.Text += "    ※ 수정체 수술의 경우 추가적인 약물치료나 수술적 치료가 필요한 출혈인 경우" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "    1) 수정체 수술" + "\r";
            rtxtHelp.Text += "      11 : 출혈(전방출혈, 초자체출혈 등)" + "\r";
            rtxtHelp.Text += "      12 : 유리체 탈출(vitreous prolapse)" + "\r";
            rtxtHelp.Text += "      13 : 안압상승" + "\r";
            rtxtHelp.Text += "      14 : 기타 합병증" + "\r";
            rtxtHelp.Text += "       ※ 유리체 탈출은 수술 종료시 전방 내 유리체가 남아있는 경우 해당" + "\r";
            rtxtHelp.Text += "       ※ 안압상승은 수술후 안압이  30mmHg이상이 일주일 이상 지속 또는 50mmHg이상이  3일 이상 지속된 경우 해당" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "    2) 편도 및 아데노이드 절제술" + "\r";
            rtxtHelp.Text += "      21 : 출혈(bleeding)" + "\r";
            rtxtHelp.Text += "      22 : 기도폐쇄(airway obstruction)" + "\r";
            rtxtHelp.Text += "      23 : 기타 합병증" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "    3) 충수절제술" + "\r";
            rtxtHelp.Text += "      31 : 출혈(bleeding)" + "\r";
            rtxtHelp.Text += "      32 : 분루(fecal fistula)" + "\r";
            rtxtHelp.Text += "      33 : 기타 합병증" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "    4) 서혜 및 대퇴부 탈장수술" + "\r";
            rtxtHelp.Text += "      41 : 출혈(bleeding)" + "\r";
            rtxtHelp.Text += "      42 : 기타 합병증" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "    5) 항문 및 항문주위 수술" + "\r";
            rtxtHelp.Text += "      51 : 출혈(bleeding)" + "\r";
            rtxtHelp.Text += "      52 : 기타 합병증" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "    6) 기타 자궁 및 자궁 부속기 수술" + "\r";
            rtxtHelp.Text += "      61 : 출혈(bleeding)" + "\r";
            rtxtHelp.Text += "      62 : 요루(urinary fistula)" + "\r";
            rtxtHelp.Text += "      63 : 기타 합병증" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "    7) 제왕절개분만" + "\r";
            rtxtHelp.Text += "      71 : 출혈(bleeding)" + "\r";
            rtxtHelp.Text += "         - 이완성 출혈(Atonic bleeding) 제외" + "\r";
            rtxtHelp.Text += "      72 : 신생아 합병증(수술중 출산 손상)" + "\r";
            rtxtHelp.Text += "      73 : 기타 합병증" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "  2.4 합병증 치료를 위한 수술 및 처치" + "\r";
            rtxtHelp.Text += "   다음의 경우 있음에 표시" + "\r";
            rtxtHelp.Text += "   - 수술과 관련된 합병증을 치료하기 위해 외과적 처치 및 수술을 한 경우" + "\r";
            rtxtHelp.Text += "   - 수술후 출혈로 전혈 또는 농축적혈구 4pint 이상 수혈을 투여한 경우" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "===========================================================================================================================================" + "\r";
            rtxtHelp.Text += "3.퇴원전 진료의 점검사항 (입원기간이 30일을 초과하는 경우는 작성제외)" + "\r";
            rtxtHelp.Text += "===========================================================================================================================================" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "  3.1 정상 퇴원 이외의 퇴원의 유형(Discharge status)" + "\r";
            rtxtHelp.Text += "   다음 퇴원유형의 경우 이상에 표시 후 해당 code에 표시" + "\r";
            rtxtHelp.Text += "   (code)" + "\r";
            rtxtHelp.Text += "    1 : 의학적 권고에 반하는 퇴원(Discharge against medical advice)" + "\r";
            rtxtHelp.Text += "    2 : 타 의료기관으로의 응급전원(Emergency transfer)" + "\r";
            rtxtHelp.Text += "        예시) 수술후 출혈 등으로 환자상태가 위급하여 타 의료기관으로 이송한 경우" + "\r";
            rtxtHelp.Text += "    3 : 타 의료기관으로의 기타전원(other transfer)" + "\r";
            rtxtHelp.Text += "    4 : 사망(Death)" + "\r";
            rtxtHelp.Text += "\r";
            rtxtHelp.Text += "  3.2 퇴원시 환자상태의 안정성(Medical Stability of the Patient)" + "\r";
            rtxtHelp.Text += "   < 퇴원시 환자 상태의 이상소견: 퇴원 전 12시간 이내 마지막 측정한 자료 >" + "\r";
            rtxtHelp.Text += "   1) 혈압(BP)" + "\r";
            rtxtHelp.Text += "     - SBP(< 85 or > 180), DBP(< 50 or > 110)(단위: mmHg)" + "\r";
            rtxtHelp.Text += "   2) 맥박(Pulse)" + "\r";
            rtxtHelp.Text += "     - 맥박이 50회 / min(베타 차단제 복용 중인 경우는 45회 / min) 이하인 경우, 또는 120회 / min 이상인 경우" + "\r";
            rtxtHelp.Text += "   ※고혈압 등 심혈관계 질환자가 혈압 및 맥박 이상 소견을 보이는 경우는 입원시 검사결과와" + "\r";
            rtxtHelp.Text += "      퇴원전 12시간 이내 마지막 검사결과를 비교하여 변화율이 20 % 이내인 경우는 제외" + "\r";
            rtxtHelp.Text += "   ※ 만12세 이하 소아의 경우 혈압, 맥박 제외" + "\r";
            rtxtHelp.Text += "   3) 체온(Temperature)" + "\r";
            rtxtHelp.Text += "     - 측정방법 불문하고 38.3℃ 이상인 경우" + "\r";
            rtxtHelp.Text += "   4) 수술부위 출혈(Wound bleeding)" + "\r";
            rtxtHelp.Text += "     - 2.3 수술 합병증 및 부작용의 ‘출혈’과 동일 적용" + "\r";
            rtxtHelp.Text += "   5) 수술부위 감염(Wound infection)" + "\r";
            rtxtHelp.Text += "    - 2.2 ‘수술부위감염’과 동일 적용";
        }

        void fn_Screen_Clear()
        {
            FstrTRSNO = "";
            FstrIPDNO = "";

            //'기본정보
            txtSName.Text = "";
            txtInDate.Text = "";
            txtOutDate.Text = "";
            dtpOpDate.Text = "";
            txtIllCode1.Text = "";
            txtIllCode2.Text = "";
            txtIllCode3.Text = "";
            txtIllCode4.Text = "";


            //1. 수술 전 진료의 점검 사항
            rdo1_1_0.Checked = false;
            rdo1_1_1.Checked = false;
            rdo1_1_1_1.Checked = false;
            rdo1_1_1_2.Checked = false;
            rdo1_1_1_3.Checked = false;
            //Call rdo1_1_0_Click


            //2. 입원 중 진료의 점검 사항
            rdo2_1_1_0.Checked = false;
            rdo2_1_1_1.Checked = false;
            rdo2_1_2_0.Checked = false;
            rdo2_1_2_1.Checked = false;
            rdo2_1_3_0.Checked = false;
            rdo2_1_3_1.Checked = false;
            rdo2_1_4_0.Checked = false;
            rdo2_1_4_1.Checked = false;
            txtOpt2_1.Text = "";
            txtOpt2_3.Text = "";
            rdo2_2_0.Checked = false;
            rdo2_2_1.Checked = false;
            rdo2_3_0.Checked = false;
            rdo2_3_1.Checked = false;
            rdo2_4_0.Checked = false;
            rdo2_4_1.Checked = false;


            //'3. 퇴원 전 진료의 점검 사항
            rdo3_1_1.Checked = false;
            rdo3_1_1_1.Checked = false;
            rdo3_1_1_2.Checked = false;
            rdo3_1_1_3.Checked = false;
            rdo3_1_1_4.Checked = false;
            //Call rdo3_1_0_Click
            rdo3_2_1_0.Checked = false;
            rdo3_2_1_1.Checked = false;
            rdo3_2_2_0.Checked = false;
            rdo3_2_2_1.Checked = false;
            rdo3_2_3_0.Checked = false;
            rdo3_2_3_1.Checked = false;
            rdo3_2_4_0.Checked = false;
            rdo3_2_4_1.Checked = false;
            rdo3_2_5_0.Checked = false;
            rdo3_2_5_1.Checked = false;


            dtpRegDate.Text = clsPublic.GstrSysDate;                //작성일
            txtEntName.Text = "";
        }

        void fn_ListView()
        {
            SP.Spread_All_Clear(ssList);

            try
            {
                SQL = "";
                SQL += " SELECT TO_CHAR(a.INDATE, 'YYYY-MM-DD') INDATE                      \r";
                SQL += "      , TO_CHAR(a.OUTDATE, 'YYYY-MM-DD') OUTDATE                    \r";
                SQL += "      , b.SNAME, a.DEPTCODE, a.ROWID, a.PANO, a.TRSNO, a.IPDNO      \r";
                SQL += "      , a.ILLCODE1, a.ILLCODE2, a.ILLCODE3, a.ILLCODE4              \r";
                SQL += "   FROM KOSMOS_PMPA.IPD_TRANS      a                                \r";
                SQL += "      , KOSMOS_PMPA.IPD_NEW_MASTER b                                \r";
                SQL += "  WHERE a.Pano  = b.Pano(+)                                         \r";
                SQL += "    AND a.IPDNO = b.IPDNO(+)                                        \r";
                SQL += "    AND a.PANO  = '" + FstrPANO + "'                                \r";
                SQL += "    AND a.GbDRG = 'D'                                               \r"; //'DRG만
                SQL += "    AND A.GBIPD NOT IN ('D')                                        \r"; //2021-01-12 추가
                SQL += "   ORDER BY a.INDATE DESC                                           \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt, ssList, 0, true);
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true)
            {
                return;
            }

            FstrROWID = ssList.ActiveSheet.Cells[e.Row, 4].Text.Trim();

            fn_Screen_Clear();

            fn_select_View(FstrROWID);
        }

        void fn_select_View(string strRowid)
        {
            string strOpDate = "";

            try
            {
                SQL = "";
                SQL += "  SELECT a.TRSNO, a.IPDNO, a.PANO, b.SNAME, a.DEPTCODE, a.ROWID     \r";
                SQL += "       , TO_CHAR(a.INDATE, 'YYYY-MM-DD') INDATE                     \r";
                SQL += "       , TO_CHAR(a.OUTDATE, 'YYYY-MM-DD') OUTDATE                   \r";
                SQL += "       , a.ILLCODE1, a.ILLCODE2, a.ILLCODE3, a.ILLCODE4             \r";
                SQL += "    FROM KOSMOS_PMPA.IPD_TRANS      a                               \r";
                SQL += "       , KOSMOS_PMPA.IPD_NEW_MASTER b                               \r";
                SQL += "   WHERE a.Pano = b.Pano(+)                                         \r";
                SQL += "     AND a.IPDNO = b.IPDNO(+)                                       \r";
                SQL += "     AND a.ROWID = '" + strRowid + "'                               \r";
                SQL += "   ORDER BY INDATE DESC                                             \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    txtSName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                    txtInDate.Text = dt.Rows[0]["INDATE"].ToString().Trim();
                    txtOutDate.Text = dt.Rows[0]["OUTDATE"].ToString().Trim();
                    txtIllCode1.Text = dt.Rows[0]["ILLCODE1"].ToString().Trim();
                    txtIllCode2.Text = dt.Rows[0]["ILLCODE2"].ToString().Trim();
                    txtIllCode3.Text = dt.Rows[0]["ILLCODE3"].ToString().Trim();
                    txtIllCode4.Text = dt.Rows[0]["ILLCODE4"].ToString().Trim();

                    FstrTRSNO = dt.Rows[0]["TRSNO"].ToString().Trim();
                    FstrIPDNO = dt.Rows[0]["IPDNO"].ToString().Trim();
                    FstrPANO = dt.Rows[0]["PANO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            try
            {
                SQL = "";
                SQL += "  SELECT TRSNO, IPDNO, PANO, PNAME                                  \r";
                SQL += "       , OPT110, OPT110A, OPT211, OPT212, OPT213, OPT214, OPT214A   \r";
                SQL += "       , OPT220, OPT230, OPT230A, OPT240, OPT310, OPT310A, OPT321   \r";
                SQL += "       , OPT322, OPT323, OPT324, OPT325                             \r";
                SQL += "       , TO_CHAR(LSDATE, 'YYYY-MM-DD') LSDATE, ENTSABUN, ENTNAME    \r";
                SQL += "       , TO_CHAR(OPDATE,'YYYY-MM-DD') OPDATE                        \r";
                SQL += "    FROM KOSMOS_PMPA.DRG_CHART1                                     \r";
                SQL += "   WHERE TRSNO = '" + FstrTRSNO + "'                                \r";
                SQL += "     AND IPDNO = '" + FstrIPDNO + "'                                \r";
                SQL += "     AND (DelDate IS NULL OR DelDate ='')                           \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }


                if (dt.Rows.Count > 0)
                {

                    dtpOpDate.Text = dt.Rows[0]["OPDATE"].ToString().Trim();
                    strOpDate = dt.Rows[0]["OPDATE"].ToString().Trim();
                    dtpRegDate.Text = clsPublic.GstrSysDate;
                    txtEntName.Text = dt.Rows[0]["ENTNAME"].ToString().Trim();

                    //txtIllCode1.Text = "";
                    //txtIllCode2.Text = "";
                    //txtIllCode3.Text = "";
                    //txtIllCode4.Text = "";


                    //1. 수술 전 진료의 점검 사항
                    rdo1_1_0.Checked = (dt.Rows[0]["OPT110"].ToString().Trim().Substring(0, 1) == "10" ? true : false);
                    rdo1_1_1.Checked = (dt.Rows[0]["OPT110"].ToString().Trim().Substring(0, 2) == "01" ? true : false);
                    //1.1 추가코드
                    if (dt.Rows[0]["OPT110A"].ToString().Trim() == "1")
                    {
                        rdo1_1_1_1.Checked = true;
                        rdo1_1_1_2.Checked = false;
                        rdo1_1_1_3.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT110A"].ToString().Trim() == "2")
                    {
                        rdo1_1_1_1.Checked = false;
                        rdo1_1_1_2.Checked = true;
                        rdo1_1_1_3.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT110A"].ToString().Trim() == "3")
                    {
                        rdo1_1_1_1.Checked = false;
                        rdo1_1_1_2.Checked = false;
                        rdo1_1_1_3.Checked = true;
                    }

                    //Call rdo1_1_0_Click


                    //2. 입원 중 진료의 점검 사항
                    if (dt.Rows[0]["OPT211"].ToString().Trim() == "10")
                    {
                        rdo2_1_1_0.Checked = true;
                        rdo2_1_1_1.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT211"].ToString().Trim() == "01")
                    {
                        rdo2_1_1_0.Checked = false;
                        rdo2_1_1_1.Checked = true;
                    }

                    //2.1. 2)
                    if (dt.Rows[0]["OPT212"].ToString().Trim() == "10")
                    {
                        rdo2_1_2_0.Checked = true;
                        rdo2_1_2_1.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT212"].ToString().Trim() == "01")
                    {
                        rdo2_1_2_0.Checked = false;
                        rdo2_1_2_1.Checked = true;
                    }

                    //2.1. 3)
                    if (dt.Rows[0]["OPT213"].ToString().Trim() == "10")
                    {
                        rdo2_1_3_0.Checked = true;
                        rdo2_1_3_1.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT213"].ToString().Trim() == "01")
                    {
                        rdo2_1_3_0.Checked = false;
                        rdo2_1_3_1.Checked = true;
                    }

                    //2.1. 4)
                    if (dt.Rows[0]["OPT214"].ToString().Trim() == "10")
                    {
                        rdo2_1_4_0.Checked = true;
                        rdo2_1_4_1.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT214"].ToString().Trim() == "01")
                    {
                        rdo2_1_4_0.Checked = false;
                        rdo2_1_4_1.Checked = true;
                    }
                    //2.1. 4) 추가 코드
                    txtOpt2_1.Text = dt.Rows[0]["OPT214A"].ToString().Trim();

                    //2.2.
                    if (dt.Rows[0]["OPT220"].ToString().Trim() == "10")
                    {
                        rdo2_2_0.Checked = true;
                        rdo2_2_1.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT220"].ToString().Trim() == "01")
                    {
                        rdo2_2_0.Checked = false;
                        rdo2_2_1.Checked = true;
                    }

                    //2.3. 
                    if (dt.Rows[0]["OPT230"].ToString().Trim() == "10")
                    {
                        rdo2_3_0.Checked = true;
                        rdo2_3_1.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT230"].ToString().Trim() == "01")
                    {
                        rdo2_3_0.Checked = false;
                        rdo2_3_1.Checked = true;
                    }

                    //2.3. 추가 코드
                    txtOpt2_3.Text = dt.Rows[0]["OPT230A"].ToString().Trim();

                    //2.4.
                    if (dt.Rows[0]["OPT240"].ToString().Trim() == "10")
                    {
                        rdo2_4_0.Checked = true;
                        rdo2_4_1.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT240"].ToString().Trim() == "01") //2021-03-02  10->  01로 수정
                    {
                        rdo2_4_0.Checked = false;
                        rdo2_4_1.Checked = true;
                    }


                    //'3.1 퇴원 전 진료의 점검 사항
                    if (dt.Rows[0]["OPT310"].ToString().Trim() == "10")
                    {
                        rdo3_1_0.Checked = true;
                        rdo3_1_1.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT310"].ToString().Trim() == "01")
                    {
                        rdo3_1_0.Checked = false;
                        rdo3_1_1.Checked = true;
                    }

                    //3.1 추가 코드
                    if (dt.Rows[0]["OPT310A"].ToString().Trim() == "1")
                    {
                        rdo3_1_1_1.Checked = true;
                        rdo3_1_1_2.Checked = false;
                        rdo3_1_1_3.Checked = false;
                        rdo3_1_1_4.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT310A"].ToString().Trim() == "2")
                    {
                        rdo3_1_1_1.Checked = false;
                        rdo3_1_1_2.Checked = true;
                        rdo3_1_1_3.Checked = false;
                        rdo3_1_1_4.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT310A"].ToString().Trim() == "3")
                    {
                        rdo3_1_1_1.Checked = false;
                        rdo3_1_1_2.Checked = false;
                        rdo3_1_1_3.Checked = true;
                        rdo3_1_1_4.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT310A"].ToString().Trim() == "4")
                    {
                        rdo3_1_1_1.Checked = false;
                        rdo3_1_1_2.Checked = false;
                        rdo3_1_1_3.Checked = false;
                        rdo3_1_1_4.Checked = true;
                    }


                    //Call rdo3_1_0_Click
                    //3.2. 1)
                    if (dt.Rows[0]["OPT321"].ToString().Trim() == "10")
                    {
                        rdo3_2_1_0.Checked = true;
                        rdo3_2_1_1.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT321"].ToString().Trim() == "01")
                    {
                        rdo3_2_1_0.Checked = false;
                        rdo3_2_1_1.Checked = true;
                    }

                    //3.2. 2)
                    if (dt.Rows[0]["OPT322"].ToString().Trim() == "10")
                    {
                        rdo3_2_2_0.Checked = true;
                        rdo3_2_2_1.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT322"].ToString().Trim() == "01")
                    {
                        rdo3_2_2_0.Checked = false;
                        rdo3_2_2_1.Checked = true;
                    }

                    //3.2. 3)
                    if (dt.Rows[0]["OPT323"].ToString().Trim() == "10")
                    {
                        rdo3_2_3_0.Checked = true;
                        rdo3_2_3_1.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT323"].ToString().Trim() == "01")
                    {
                        rdo3_2_3_0.Checked = false;
                        rdo3_2_3_1.Checked = true;
                    }

                    //3.2. 4)
                    if (dt.Rows[0]["OPT324"].ToString().Trim() == "10")
                    {
                        rdo3_2_4_0.Checked = true;
                        rdo3_2_4_1.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT324"].ToString().Trim() == "01")
                    {
                        rdo3_2_4_0.Checked = false;
                        rdo3_2_4_1.Checked = true;
                    }
                    //3.2. 5)
                    if (dt.Rows[0]["OPT325"].ToString().Trim() == "10")
                    {
                        rdo3_2_5_0.Checked = true;
                        rdo3_2_5_1.Checked = false;
                    }
                    else if (dt.Rows[0]["OPT325"].ToString().Trim() == "01")
                    {
                        rdo3_2_5_0.Checked = false;
                        rdo3_2_5_1.Checked = true;
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (strOpDate == "")
            {
                try
                {
                    SQL = "";
                    SQL += " SELECT TO_CHAR(OPDATE, 'YYYY-MM-DD') OPDATE        \r";
                    SQL += "   FROM KOSMOS_PMPA.ORAN_MASTER                     \r";
                    SQL += "  WHERE PANO = '" + FstrPANO + "'                   \r";
                    SQL += "  ORDER BY OPDATE DESC                              \r";
                    clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dtpOpDate.Text = dt.Rows[0]["OPDATE"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            rdo1_1_1.Checked = true;
            rdo1_1_1_1.Checked = true;

            rdo2_1_1_0.Checked = true;
            rdo2_1_2_0.Checked = true;
            rdo2_1_3_0.Checked = true;
            rdo2_1_4_0.Checked = true;
            rdo2_2_0.Checked = true;
            rdo2_3_0.Checked = true;
            rdo2_4_0.Checked = true;

            rdo3_1_0.Checked = true;
            rdo3_2_1_0.Checked = true;
            rdo3_2_2_0.Checked = true;
            rdo3_2_3_0.Checked = true;
            rdo3_2_4_0.Checked = true;
            rdo3_2_5_0.Checked = true;
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (FstrTRSNO == "" || FstrIPDNO == "")
            {
                return;
            }
            if (rdo1_1_1.Checked == false && rdo1_1_0.Checked == false)
            {
                MessageBox.Show("점검 사항 체크 후 등록 하여 주십시오.");
                return;
            }

            string strMsg;
            string intMsgType;
            
            string strOPT110 = ""; string strOPT110A = ""; string strOPT211 = ""; string strOPT212 = ""; string strOPT213 = ""; string strOPT214 = ""; string strOPT214A = "";
            string strOPT220 = ""; string strOPT230 = ""; string strOPT230A = ""; string strOPT240 = ""; string strOPT310 = ""; string strOPT310A = ""; string strOPT321 = "";
            string strOPT322 = ""; string strOPT323 = ""; string strOPT324 = ""; string strOPT325 = "";
            
            //'1.1.
            if (rdo1_1_0.Checked == true)
            {
                strOPT110 = "10";
            }
            else if (rdo1_1_1.Checked == true)
            {
                strOPT110 = "01";
            }
            //'1.1. 추가 코드
            if (rdo1_1_1_1.Checked == true)
            {
                strOPT110A = "1";
            }
            else if (rdo1_1_1_2.Checked == true)
            {
                strOPT110A = "2";
            }
            else if (rdo1_1_1_3.Checked == true)
            {
                strOPT110A = "3";
            }
            //'2.1. 1)

            if (rdo2_1_1_0.Checked == true)
            {
                strOPT211 = "10";
            }
            else if (rdo2_1_1_1.Checked == true)
            {
                strOPT211 = "01";
            }
            //2.1. 2)
            if (rdo2_1_2_0.Checked == true)
            {
                strOPT212 = "10";
            }
            else if (rdo2_1_2_1.Checked == true)
            {
                strOPT212 = "01";
            }
            //'2.1. 3)
            if (rdo2_1_3_0.Checked == true)
            {
                strOPT213 = "10";
            }
            else if (rdo2_1_3_1.Checked == true)
            {
                strOPT213 = "01";
            }
            //'2.1. 4)
            if (rdo2_1_4_0.Checked == true)
            {
                strOPT214 = "10";
            }
            else if (rdo2_1_4_1.Checked == true)
            {
                strOPT214 = "01";
            }
            //'2.1. 4) 추가 코드
            strOPT214A = txtOpt2_1.Text;
            //'2.2.
            if (rdo2_2_0.Checked == true)
            {
                strOPT220 = "10";
            }
            else if (rdo2_2_1.Checked == true)
            {
                strOPT220 = "01";
            }
            //2.3.
            if (rdo2_3_0.Checked == true)
            {
                strOPT230 = "10";
            }
            else if (rdo2_3_1.Checked == true)
            {
                strOPT230 = "01";
            }
            //'2.3. 추가 코드
            strOPT230A = txtOpt2_3.Text;
            //'2.4.
            if (rdo2_4_0.Checked == true)
            {
                strOPT240 = "10";
            }
            else if (rdo2_4_1.Checked == true)
            {
                strOPT240 = "01";
            }
            //'3.1.
            if (rdo3_1_0.Checked == true)
            {
                strOPT310 = "10";
            }
            else if (rdo3_1_1.Checked == true)
            {
                strOPT310 = "01";
            }
            //'3.1. 추가 코드
            if (rdo3_1_1_1.Checked == true)
            {
                strOPT310A = "1";
            }
            else if (rdo3_1_1_2.Checked == true)
            {
                strOPT310A = "2";
            }
            else if (rdo3_1_1_3.Checked == true)
            {
                strOPT310A = "3";
            }
            else if (rdo3_1_1_4.Checked == true)
            {
                strOPT310A = "4";
            }

            //'3.2. 1)
            if (rdo3_2_1_0.Checked == true)
            {
                strOPT321 = "10";
            }
            else if (rdo3_2_1_1.Checked == true)
            {
                strOPT321 = "01";
            }
            //'3.2. 2)
            if (rdo3_2_2_0.Checked == true)
            {
                strOPT322 = "10";
            }
            else if (rdo3_2_2_1.Checked == true)
            { 
                strOPT322 = "01";
            }
            //'3.2. 3)
            if (rdo3_2_3_0.Checked ==  true)
            {
                strOPT323 = "10";
            }
            else if (rdo3_2_3_1.Checked ==  true)
            {
                strOPT323 = "01";
            }
            //'3.2. 4)
            if (rdo3_2_4_0.Checked == true)
            {
                strOPT324 = "10";
            }
            else if (rdo3_2_4_1.Checked == true)
            {
                strOPT324 = "01";
            }
            //'3.2. 5)
            if (rdo3_2_5_0.Checked == true)
            {
                strOPT325 = "10";
            }
            else if (rdo3_2_5_1.Checked == true)
            {
                strOPT325 = "01";
            }

            //'저장 기본 체크
            if (strOPT110 == "")
            {
                MessageBox.Show("1.1 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT110 == "01" && strOPT110A == "")
            {
                MessageBox.Show("1.1 시행세부항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT211 == "")
            {
                MessageBox.Show("2.1 1) 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT212 == "")
            {
                MessageBox.Show("2.1 2) 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT213 == "")
            {
                MessageBox.Show("2.1 3) 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT214 == "")
            {
                MessageBox.Show("2.1 4) 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT214 == "01" && strOPT214A == "")
            {
                MessageBox.Show("2.1 4) 세부항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT220 == "")
            {
                MessageBox.Show("2.2 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT230 == "")
            {
                MessageBox.Show("2.3 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT230 == "01" && strOPT230A == "")
            {
                MessageBox.Show("2.3 세부항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT240 == "")
            {
                MessageBox.Show("2.4 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT310 == "")
            {
                MessageBox.Show("3.1 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT310 == "01" && strOPT310A == "")
            {
                MessageBox.Show("3.1 세부항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT321 == "")
            {
                MessageBox.Show("3.2 1) 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT322 == "")
            {
                MessageBox.Show("3.2 2) 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT323 == "")
            {
                MessageBox.Show("3.2 3) 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT324 == "")
            {
                MessageBox.Show("3.2 4) 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (strOPT325 == "")
            {
                MessageBox.Show("3.2 5) 항목 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (txtEntName.Text.Trim() == "")
            {
                MessageBox.Show("작성자 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); txtEntName.Focus();
                return;
            }

            if (dtpOpDate.Text == "")
            {
                MessageBox.Show("수술일자 누락점검!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk); dtpOpDate.Focus();
                return;
            }

            
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {   
                SQL = "";
                SQL += " merge                                                                  \r";
                SQL += "  into KOSMOS_PMPA.DRG_CHART1 a                                         \r";
                SQL += " using dual b                                                           \r";
                SQL += "    on (a.TRSNO = '" + FstrTRSNO + "'                                   \r";
                SQL += "   and  a.IPDNO = '" + FstrIPDNO + "')                                  \r";
                SQL += " when matched then                                                      \r";
                SQL += "      update set                                                        \r";
                SQL += "             PANO = '" + FstrPANO + "'                                  \r";
                SQL += "           , PNAME = '" + txtSName.Text + "'                            \r";
                SQL += "           , OPT110 = '" + strOPT110 + "'                               \r";
                SQL += "           , OPT110A = '" + strOPT110A.Trim() + "'                      \r";
                SQL += "           , OPT211 = '" + strOPT211 + "'                               \r";
                SQL += "           , OPT212 = '" + strOPT212 + "'                               \r";
                SQL += "           , OPT213 = '" + strOPT213 + "'                               \r";
                SQL += "           , OPT214 = '" + strOPT214 + "'                               \r";
                SQL += "           , OPT214A = '" + strOPT214A.Trim() + "'                      \r";
                SQL += "           , OPT220 = '" + strOPT220 + "'                               \r";
                SQL += "           , OPT230 = '" + strOPT230 + "'                               \r";
                SQL += "           , OPT230A = '" + strOPT230A.Trim() + "'                      \r";
                SQL += "           , OPT240 = '" + strOPT240 + "'                               \r";
                SQL += "           , OPT310 = '" + strOPT310 + "'                               \r";
                SQL += "           , OPT310A = '" + strOPT310A.Trim() + "'                      \r";
                SQL += "           , OPT321 = '" + strOPT321 + "'                               \r";
                SQL += "           , OPT322 = '" + strOPT322 + "'                               \r";
                SQL += "           , OPT323 = '" + strOPT323 + "'                               \r";
                SQL += "           , OPT324 = '" + strOPT324 + "'                               \r";
                SQL += "           , OPT325 = '" + strOPT325 + "'                               \r";
                SQL += "           , LSDATE = TO_DATE('" + dtpRegDate.Text + "', 'YYYY-MM-DD')  \r";
                SQL += "           , OPDATE = TO_DATE('" + dtpOpDate.Text + "', 'YYYY-MM-DD')   \r";
                SQL += "           , ENTSABUN = '" + clsPublic. GnJobSabun + "'                 \r";
                SQL += "           , ENTNAME = '" + txtEntName.Text + "'                        \r";
                SQL += "   when not matched then                                                \r";
                SQL += "     INSERT (                                                           \r";
                SQL += "             TRSNO                                                      \r";
                SQL += "           , IPDNO                                                      \r";
                SQL += "           , PANO                                                       \r";
                SQL += "           , PNAME                                                      \r";
                SQL += "           , OPT110                                                     \r";
                SQL += "           , OPT110A                                                    \r";
                SQL += "           , OPT211                                                     \r";
                SQL += "           , OPT212                                                     \r";
                SQL += "           , OPT213                                                     \r";
                SQL += "           , OPT214                                                     \r";
                SQL += "           , OPT214A                                                    \r";
                SQL += "           , OPT220                                                     \r";
                SQL += "           , OPT230                                                     \r";
                SQL += "           , OPT230A                                                    \r";
                SQL += "           , OPT240                                                     \r";
                SQL += "           , OPT310                                                     \r";
                SQL += "           , OPT310A                                                    \r";
                SQL += "           , OPT321                                                     \r";
                SQL += "           , OPT322                                                     \r";
                SQL += "           , OPT323                                                     \r";
                SQL += "           , OPT324                                                     \r";
                SQL += "           , OPT325                                                     \r";
                SQL += "           , LSDATE                                                     \r";
                SQL += "           , OPDATE                                                     \r";
                SQL += "           , ENTSABUN                                                   \r";
                SQL += "           , ENTNAME                                                    \r";
                SQL += "            )                                                           \r";
                SQL += "     VALUES (                                                           \r";
                SQL += "             '" + FstrTRSNO + "'                                        \r";
                SQL += "           , '" + FstrIPDNO + "'                                        \r";
                SQL += "           , '" + FstrPANO + "'                                        \r";
                SQL += "           , '" + txtSName.Text + "'                                    \r";
                SQL += "           , '" + strOPT110 + "'                                        \r";
                SQL += "           , '" + strOPT110A.Trim() + "'                                \r";
                SQL += "           , '" + strOPT211 + "'                                        \r";
                SQL += "           , '" + strOPT212 + "'                                        \r";
                SQL += "           , '" + strOPT213 + "'                                        \r";
                SQL += "           , '" + strOPT214 + "'                                        \r";
                SQL += "           , '" + strOPT214A.Trim() + "'                                \r";
                SQL += "           , '" + strOPT220 + "'                                        \r";
                SQL += "           , '" + strOPT230 + "'                                        \r";
                SQL += "           , '" + strOPT230A.Trim() + "'                                \r";
                SQL += "           , '" + strOPT240 + "'                                        \r";
                SQL += "           , '" + strOPT310 + "'                                        \r";
                SQL += "           , '" + strOPT310A.Trim() + "'                                \r";
                SQL += "           , '" + strOPT321 + "'                                        \r";
                SQL += "           , '" + strOPT322 + "'                                        \r";
                SQL += "           , '" + strOPT323 + "'                                        \r";
                SQL += "           , '" + strOPT324 + "'                                        \r";
                SQL += "           , '" + strOPT325 + "'                                        \r";
                SQL += "           , TO_DATE('" + dtpRegDate.Text + "', 'YYYY-MM-DD')           \r";
                SQL += "           , TO_DATE('" + dtpOpDate.Text + "', 'YYYY-MM-DD')            \r";
                SQL += "           , '" + clsPublic.GnJobSabun + "'                             \r";
                SQL += "           , '" + txtEntName.Text + "'                                  \r";
                SQL += "            )                                                           \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                if (FstrFlag != "")
                {
                    this.ControlBox = true;
                    btnExit.Enabled = true;
                    lblOrderSend.Visible = false;
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            fn_Screen_Clear();
            fn_ListView();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            fn_Screen_Clear();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (FstrTRSNO == "" || FstrIPDNO == "")
            {
                MessageBox.Show("대상 선택후 처리하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (MessageBox.Show("자료를 삭제하시겠습니까?.", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL = " UPDATE KOSMOS_PMPA.DRG_CHART1 SET       \r";
                    SQL += "       DelDate = SYSDATE                \r";
                    SQL += " WHERE TRSNO = '" + FstrTRSNO + "'      \r";
                    SQL += "   AND IPDNO = '" + FstrIPDNO + "'      \r";
                    SQL += "   AND (DelDate IS NULL OR DelDate ='') \r";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("삭제 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                fn_Screen_Clear();
                fn_ListView();
            }  
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //출력루틴 없음
        }
    }
}
