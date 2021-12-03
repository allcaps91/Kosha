using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
//using Microsoft.VisualBasic;
using ComMirLibB.MirEnt; //청구기본 클래스us

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmComMirSpec.cs
    /// Description     : 특정내역(명세서/진료내역)
    /// Author          : 박성완
    /// Create Date     : 2018-04-12
    public partial class frmComMirSpec : Form
    {

        //public delegate void FormSendDataHandler(long GNSpecWRTNO);
        //public event FormSendDataHandler FormSendEvent;


        //string FstrOK = "";
        string FstrTable = "";
        string FstrBun = "";

        //받은거
        string GstrGuBun = "";
        string GstrTime = "";
        string GstrSpecTit = "";
        string GstrSpecKtasER = "";
        string GstrSpecSlipNo = "";
        string GstrSpecBun = "";
        string GstrSpec = "";
        int GnSeqNo1 = 0;
        int GnJulNo = 0;
        string GstrC = "";
        string GstrFrDate = "";

        long GnChoiceWRTNO = 0;
        long GNSpecWRTNO = 0;

        clsComMirEntSpd ComMirEntSpd = new clsComMirEntSpd();
        clsComMir.cls_Table_Mir_Insid TID = new clsComMir.cls_Table_Mir_Insid();
        ComFunc CF = new ComFunc();

        /// <summary>
        /// 필요한 피라미터 값
        /// </summary>
        /// <param name="argTID">VB:TID</param>
        /// <param name="argGuBun">VB에서의 프로그램명</param>
        /// <param name="argTime">VB:Gstrtime</param>
        /// <param name="argSpecTit">VB:GstrSpecTit</param>
        /// <param name="argSpecKtasER">VB:GstrSpecKtasER</param>
        /// <param name="argSpecSlipNo">VB:GstrSpecSlipNo</param>
        /// <param name="argSpecBun">VB:GstrSpecBun</param>
        /// <param name="argSpec">VB:GstrSpec</param>
        /// <param name="argSeqNo1">VB:GstrSeqNo1</param>
        /// <param name="argJulNo">VB:GstrJulNo</param>
        /// <param name="argC">VB:GstrC</param>
        /// <param name="argFrDate">VB:GstrFrDate</param>
        /// <param name="argChoiceWRTNO">VB:GstrChoiceWRTNO</param>
        /// <param name="argSpecWRTNO">VB:GstrSpecWRTNO</param>
        public frmComMirSpec(clsComMir.cls_Table_Mir_Insid argTID, string argGuBun, string argTime, string argSpecTit, string argSpecKtasER, string argSpecSlipNo,string argSpecBun, string argSpec, int argSeqNo1, string argJulNo, string argC, string argFrDate, long argChoiceWRTNO, long argSpecWRTNO)
        {
            TID = argTID;
            GstrGuBun = argGuBun;
            GstrTime = argTime;
            GstrSpecTit = argSpecTit;
            GstrSpecKtasER = argSpecKtasER;
            GstrSpecSlipNo = argSpecSlipNo;
            GstrSpecBun = argSpecBun;
            GstrSpec = argSpec;
            GnSeqNo1 = argSeqNo1;

            if (argJulNo == "")
            {
                GnJulNo = 0;
            }
            else
            {
                GnJulNo = Convert.ToInt32(argJulNo);
            }
            
            GstrC = argC;
            GstrFrDate = argFrDate;

            GnChoiceWRTNO = argChoiceWRTNO;
            GNSpecWRTNO = argSpecWRTNO;

            InitializeComponent();

            SetEvent();
        }


        private void SetEvent()
        {
            this.Load += FrmComMirSpec_Load;
            

            this.btnSearch.Click += BtnSearch_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnExit.Click += BtnExit_Click;
            this.btnDelete.Click += BtnDelete_Click;

            this.cboSpec.LostFocus += CboSpec_LostFocus;

           // this.FormClosed += FrmComMirSpec_FormClosed;

            this.menuReFlash.Click += MenuReFlash_Click;
            

            this.optGB0.Click += OptGB_Click;
            this.optGB1.Click += OptGB_Click;
            this.optGB2.Click += OptGB_Click;
            this.optGB3.Click += OptGB_Click;

            this.lblTitle.Click += LblTitle_Click;

            this.ss1.CellDoubleClick += Ss1_CellDoubleClick;
            this.ss2.CellClick += Ss2_CellClick;
            this.ss3.CellDoubleClick += Ss3_CellDoubleClick;
            this.ss4.CellDoubleClick += Ss4_CellDoubleClick;
            this.ss51.CellDoubleClick += Ss51_CellDoubleClick;
            this.ss6.CellDoubleClick += Ss6_CellDoubleClick;
            this.ss7.CellDoubleClick += Ss7_CellDoubleClick;
            this.ssDCT.CellDoubleClick += SsDCT_CellDoubleClick;
            this.SS9.CellDoubleClick += SS9_CellDoubleClick;
            this.ss10.CellDoubleClick += SS10_CellDoubleClick;


            this.txtSpec.KeyPress += TxtSpec_KeyPress;
        }

        private void SS10_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader)
            {
                return;
            }

            cboSpec.Text = "JX999.기타내역";

            txtSpec.Text = ss10.ActiveSheet.Cells[e.Row, 0].Text + "/" + VB.Replace(ss10.ActiveSheet.Cells[e.Row, 1].Text,"-","") + "/수술후경과관찰";
        }

        private void SS9_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader)
            {
                return;
            }

            cboSpec.Text = SS9.ActiveSheet.Cells[e.Row, 0].Text;

            txtSpec.Text = SS9.ActiveSheet.Cells[e.Row, 1].Text;
        }

        private void Ss7_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader)
            {
                return;
            }

            string strSpec = "";

            strSpec = ss7.ActiveSheet.Cells[e.Row, 0].Text;
            strSpec += "/" + ss7.ActiveSheet.Cells[e.Row, 1].Text;

            cboSpec.Text = "JT003.간호간병입원기간";

            txtSpec.Text = strSpec;
        }

        private void ss8_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader)
            {
                return;
            }

            string strSpec = "";

            strSpec = ss8.ActiveSheet.Cells[e.Row, 0].Text + "/" + ss8.ActiveSheet.Cells[e.Row, 1].Text;

            cboSpec.Text = "JX999.기타내역";

            txtSpec.Text = strSpec;
        }

        private void Ss6_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader)
            {
                return;
            }

            string strSpec = "";

            strSpec = ss6.ActiveSheet.Cells[e.Row, 0].Text;
            strSpec += "/" + ss6.ActiveSheet.Cells[e.Row, 1].Text;

            cboSpec.Text = "JT003.간호간병입원기간";

            txtSpec.Text = strSpec;
        }

        private void TxtSpec_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnSave.Focus();
            }
        }

        private void SsDCT_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            string strDrName = "";
            string strDrBunho = "";

            strDrName = ssDCT.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            strDrBunho = ssDCT.ActiveSheet.Cells[e.Row, 1].Text.Trim();

            txtSpec.Text += strDrBunho + "/" + strDrName + "/";
        }

        private void Ss51_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                return;
            }

            txtSpec.Text = ss51.ActiveSheet.Cells[e.Row, 9].Text.Trim();
        }

        private void Ss4_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            string strSpec = "";

            if(e.Column == 0 || e.Column == 1)
            {
                strSpec = ss4.ActiveSheet.Cells[e.Row, 0].Text;
                strSpec += "/" + ss4.ActiveSheet.Cells[e.Row, 1].Text;

                cboSpec.Text = "JT003.집중치료실 (신생아집중치료실포함)";

                txtSpec.Text = strSpec;
            }
            else if(e.Column == 2)
            {
                if(ss4.ActiveSheet.Cells[e.Row,2].Text.Trim() == "33")
                {
                    strSpec = "117UN";
                }
                else if(ss4.ActiveSheet.Cells[e.Row, 2].Text.Trim() == "35")
                {
                    strSpec = "101UN";
                }

                cboSpec.Text = "JT001.확인코드";

                txtSpec.Text = strSpec;
            }
            
        }

        private void Ss3_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            string strSpecCode = "";
            string strSpec = "";

            strSpecCode = ss3.ActiveSheet.Cells[e.Row, 0].Text;
            strSpec = ss3.ActiveSheet.Cells[e.Row, 1].Text.Replace("'", "");

            if (strSpec.Trim() == "")
            {
                MessageBox.Show("특정내역이 없습니다.", "확인");
                return;
            }

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "INSERT INTO " + FstrTable + " ( WRTNO, SEQNO1, SEQNO2, GUBUN, JULNO, SPECCODE, SPEC, WRTNOS, BI)";
                SQL = SQL + "VALUES ( '" + GnChoiceWRTNO + "', '" + txtSeqNo1.Text + "', '" + txtSeqNo2.Text + "', ";
                if (optGB0.Checked == true)
                {
                    SQL = SQL + " '1', ";
                }
                else if (optGB1.Checked == true)
                {
                    SQL = SQL + " '2',";
                }
                else
                {
                    SQL = SQL + " '4',";
                }

                if (GstrSpec == "1" || GstrSpec == "4") //명세서단위는 0
                {
                    SQL = SQL + " '0', ";
                }
                else
                {
                    SQL = SQL + " '" + txtJulNo.Text + "', ";
                }
                SQL = SQL + " '" + strSpecCode + "', '" + strSpec + "', '" + GNSpecWRTNO + "', '" + TID.Bi + "'  )";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                clsDB.setCommitTran(clsDB.DbCon);

                Screen_Clear();
                //menuReFlash.PerformClick();
                MenuReFlash();


            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                return;
            }


        }

        private void Screen_Clear()
        {
            txtSpec.Text = "";
            ss1.ActiveSheet.Rows.Count = 0;
            txtROWID.Text = "";
        }

        private void Ss2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            if (e.Column != 1 && e.Column != 2)
            {
                return;
            }

            cboSpec.Text = "JS010.야간가산, 응급의료수가";
            txtSpec.Text = ss2.ActiveSheet.Cells[e.Row, e.Column].Text;
        }

        private void Ss1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                return;
            }

            if (e.ColumnHeader == false && e.RowHeader == true)
            {
                string SQL = "";
                string SqlErr = "";
                int intRowAffected = 0;
                string strRowid = ss1.ActiveSheet.Cells[e.Row, 7].Text;

                if (MessageBox.Show(ss1.ActiveSheet.Cells[e.Row, 2].Text + " " + ss1.ActiveSheet.Cells[e.Row, 3].Text + "을 정말삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //clsDB.setBeginTran(clsDB.DbCon);

                    SQL = "";
                    SQL = SQL + "DELETE " + FstrTable + " WHERE ROWID = '" + strRowid + "' ";
                    clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("DELETE " + FstrTable + " ERROR!!!");
                        return;
                    }



                    if (Read_MIR_SPEC(GnChoiceWRTNO, GNSpecWRTNO, TID.Bi) != 0)
                    {
                        MenuReFlash();
                        return;
                    }

                    if (optGB1.Checked == true)
                    {

                        if (TID.Bi == "52")
                        {
                            SQL = "";
                            SQL = SQL + "UPDATE MIR_TADTL SET  WRTNOS = '' ";
                        }
                        else if (TID.Bi == "31")
                        {
                            SQL = "";
                            SQL = SQL + "UPDATE MIR_SANDTL SET  WRTNOS = '' ";
                        }
                        else
                        {
                            SQL = "";
                            SQL = SQL + "UPDATE MIR_INSDTL SET  WRTNOS = '' ";
                        }

                        SQL = SQL + " WHERE WRTNO = '" + GnChoiceWRTNO + "'";
                        SQL = SQL + "   AND WRTNOS = '" + GNSpecWRTNO + "'";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        
                        if (SqlErr != "")
                        {
                            //clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("DELETE " + FstrTable + " ERROR!!!");
                            return;
                        }
                    }

                    //clsDB.setCommitTran(clsDB.DbCon);
                    //DialogResult = System.Windows.Forms.DialogResult.OK;
                    MenuReFlash();
                }

                return;
            }

            if (ss1.ActiveSheet.Cells[e.Row, 0].Text == "1")
            {
                optGB0.Checked = true;
            }
            else if (ss1.ActiveSheet.Cells[e.Row, 0].Text == "2")
            {
                optGB1.Checked = true;
            }

            txtJulNo.Text = ss1.ActiveSheet.Cells[e.Row, 1].Text;
            txtSpec.Text = ss1.ActiveSheet.Cells[e.Row, 3].Text;
            txtSeqNo1.Text = VB.Val(ss1.ActiveSheet.Cells[e.Row, 4].Text).ToString("0");
            txtSeqNo2.Text = VB.Val(ss1.ActiveSheet.Cells[e.Row, 5].Text).ToString("0");

            txtROWID.Text = ss1.ActiveSheet.Cells[e.Row, 7].Text;

            for (int i = 0; i < cboSpec.Items.Count; i++)
            {
                if (ss1.ActiveSheet.Cells[e.Row, 2].Text.Trim() == VB.Left(cboSpec.Items[i].ToString(), 5).Trim())
                {
                    cboSpec.SelectedIndex = i;
                    return;
                }
            }

            
        }

        private void LblTitle_Click(object sender, EventArgs e)
        {
            //2005-06-25
            if (optGB0.Checked == true)
            {
                cboSpec.Items.Clear();

                if (TID.Bi == "31")
                {
                    cboSpec.Items.Add("MB001.간병인현황");
                    cboSpec.Items.Add("MB002.병행진료");
                    cboSpec.Items.Add("MB003.특별진찰선택진료료");
                    cboSpec.Items.Add("MB004.최초입원일자");
                    cboSpec.Items.Add("MB005.특정내역");
                }

                cboSpec.Items.Add("MS001.원내투약일수(경구/외용)");
                cboSpec.Items.Add("MS002.원내투약일수(주사제)");
                cboSpec.Items.Add("MS003.의약분업예외구분코드");
                cboSpec.Items.Add("MS004.신생아체중");
                cboSpec.Items.Add("MS005.낮병동 재원시간");
                cboSpec.Items.Add("MS006.개두술 수술일자");
                cboSpec.Items.Add("MS007.암질환 Stage 분류");
                cboSpec.Items.Add("MS008.암질환 TNM 분류");
                cboSpec.Items.Add("MS009.항암화학요법 투여단계 및 주기");
                cboSpec.Items.Add("MS010.민원처리결과급여결정진료분: 1자리 "); //drg
                cboSpec.Items.Add("MS011.야간 및 공휴일 수술 ccyymmddhhmm: 201307081850 ");             //drg
                cboSpec.Items.Add("MS013.단순초음파"); //2016-10-07
                cboSpec.Items.Add("MT001.상해외인");
                cboSpec.Items.Add("MT002.특정기호");
                cboSpec.Items.Add("MT003.개방병원진료시의뢰기관기호");
                cboSpec.Items.Add("MT004.소명자료 구분");
                cboSpec.Items.Add("MT005.주민등록번호 이상건");
                cboSpec.Items.Add("MT006.분만: 재왕절개만출술의 경우 임신주수를 기재 2자리");        // DRG  
                cboSpec.Items.Add("MT007.DRG세부내역 내역구분(식대:EAT, 외과:SUR, 건강보험100/100: ALL)/투여일시/코드구분/코드/단가/1일투여량/총투여일수/금액/준용명 ");  //DRG
                cboSpec.Items.Add("MT010.폐렴정보");
                cboSpec.Items.Add("MT011.패혈증 정보");
                cboSpec.Items.Add("MT015.제출자료목록표");
                cboSpec.Items.Add("MT016.제출자료목록표(기타)");
                cboSpec.Items.Add("MT014.중증(완전틀니)환자등록번호");
                cboSpec.Items.Add("MT018.본인부담코드");
                cboSpec.Items.Add("MT019.진료확인번호");
                cboSpec.Items.Add("MT020.의료급여수급권자 및 차상위대상자 원내직접조제투약횟수");
                cboSpec.Items.Add("MT021.입원유형(1.자의입원, 2.보호의무자에의한 입원, 3.시장, 군수 구정창퇴원, 4.응급입원 9.기타)");
                cboSpec.Items.Add("MT022.퇴원유형(1.자의퇴원, 2.보호의무자에의한 퇴원, 3.정신보건심의위원회명령 퇴원, 9기타)");
                cboSpec.Items.Add("MT023.퇴원후거주지(1.자가, 2.사회복지시설, 9기타)");
                cboSpec.Items.Add("MT024.임부정보 및 임부금기의약품 처방( 조제) 사유");
                cboSpec.Items.Add("MT025.물리치료사공휴일근무현황");
                cboSpec.Items.Add("MT026.인공호흡시간");
                cboSpec.Items.Add("MT027.영아체중");

                cboSpec.Items.Add("MT028.산정특례대상 세부상병명");
                cboSpec.Items.Add("MT029.진찰횟수");
                cboSpec.Items.Add("MT031.인공수정체재료대 사용내역기재");  //DRG
                cboSpec.Items.Add("MT033.내시경적 점막하 박리 절제술");
                cboSpec.Items.Add("MT034.행위 질병군 분리청구의 경우 최초입원 개시일 ccyymmdd ");    //drg
                cboSpec.Items.Add("MT035.입원시 상병 유무");    //drg
                cboSpec.Items.Add("MT036.의료의 질 점검 내용"); //drg
                cboSpec.Items.Add("MT037.레진상틀니 및 타상명진료");
                cboSpec.Items.Add("MT039.복강경 수술중 개복하여수술 :1자리");      //DRG
                cboSpec.Items.Add("MT040.본인부담금 발생 횟수");
                cboSpec.Items.Add("MT041.산부인과 가산점수산정 :1자리 Y"); //DRG
                cboSpec.Items.Add("MT043.코로나 지원대상"); //DRG

                cboSpec.Items.Add("MT046.응급환자중증도 분류기준 :1자리");     //2016-01-11
                cboSpec.Items.Add("MT048.응급의료센터 구분코드 :1자리");       //2016-01-11
                cboSpec.Items.Add("MT049.최초입원시점");              //2016-01-11
                cboSpec.Items.Add("MT051.조산아 등록번호");                      //2017-02-27
                cboSpec.Items.Add("MT059.문제의약품 유형");                      //2019-10-14
                cboSpec.Items.Add("MT060.인공수정체 제외금액 유형");             //2020-01-07
                cboSpec.Items.Add("MT063.요양병원 기관기호/의뢰일자");           //2020-01-06
                cboSpec.Items.Add("MT064.질병군 적용제외 유형");           //2020-07-27
                cboSpec.Items.Add("MT066.진료의뢰회송번호");           //2020-11-16
                cboSpec.Items.Add("MT070.폐렴 중증도 판정 결과");           //2021-10-26

                cboSpec.Items.Add("MT998.100/100진료(조제)내역");
                cboSpec.Items.Add("MT999.100/100약제 처방 내역");

                cboSpec.Items.Add("MJ001.가정간호구분자");
                cboSpec.Items.Add("MJ002.환자납무액발생사유");

                cboSpec.Items.Add("MX999.기타내역");

                cboSpec.SelectedIndex = 0;
                lblTitle.Text = "명세서단위";
            }
            else if (optGB1.Checked == true)
            {
                cboSpec.Items.Clear();

                if (GstrSpecBun == "49")
                {
                    cboSpec.Items.Add("JT020.초음파검사, MRI검사 시행일자");
                    cboSpec.Items.Add("JS013.단순,유도 초음파");
                    cboSpec.Items.Add("JT005.분만");
                }

                if (TID.Bi == "31")
                {
                    cboSpec.Items.Add("JB001.진료담당의사간병필요소견");
                    cboSpec.Items.Add("JB002.이송료");
                    cboSpec.Items.Add("JB003.재활보조기구");
                    cboSpec.Items.Add("JB004.확인수수료");
                    cboSpec.Items.Add("JB005.향정신약물처방(조제)사유");
                    cboSpec.Items.Add("JB006.상급병실사용사유");
                }
                cboSpec.Items.Add("JT002.진찰료");
                cboSpec.Items.Add("JS010.야간가산, 응급의료수가");
                cboSpec.Items.Add("JS001.마취과전문의");
                cboSpec.Items.Add("JS002.의약분업예외구분코드");
                cboSpec.Items.Add("JS003.입원시각");
                cboSpec.Items.Add("JS004.퇴원시각");
                cboSpec.Items.Add("JS005.검체검사 위탁");
                cboSpec.Items.Add("JS006.시설등의공동이용진료");
                cboSpec.Items.Add("JS007.개방병원의뢰진료");
                cboSpec.Items.Add("JS008.위탁진료");
                cboSpec.Items.Add("JS009.준용명");
                cboSpec.Items.Add("JS011.혈명코드");
                cboSpec.Items.Add("JS013.단순,유도 초음파");
                cboSpec.Items.Add("JT001.확인코드");

                cboSpec.Items.Add("JT003.집중치료실(신생아집중치료실포함)");
                cboSpec.Items.Add("JT004.신생아집중치료실");
                cboSpec.Items.Add("JT005.분만");
                cboSpec.Items.Add("JT006.DUR관련확인코드");
                cboSpec.Items.Add("JT007.치매검사결과");
                cboSpec.Items.Add("JT010.저함량 배수처방(조제)관련");
                cboSpec.Items.Add("JT011.병용 연령금기등 약제처방사유");
                cboSpec.Items.Add("JT012.동일성분 의약품 중복처방사유");
                cboSpec.Items.Add("JT013.수술일자");
                cboSpec.Items.Add("JT014.향정신성약물 장기처방 사유");
                cboSpec.Items.Add("JT015.내시경적 점막하 박리 절제술 병리결과 ");
                cboSpec.Items.Add("JT016.(내시경적 점막하 박리절제술 시술의사) 기재 형식:면허번호/의사");
                cboSpec.Items.Add("JT025.내시경적 점막하 박리절제술(ESD) 조직유형분류 기재 형식:숫자(1)/숫자(!)"); //2019-04-22,김해수, 심사팀장님 요청 새로운코드 추가
                cboSpec.Items.Add("JT031.혈청크레아티닌 검사 결과 : 9(2).V9(1)");//2021-10-26 심사팀장 의뢰서
                cboSpec.Items.Add("JT032.요단백 검사결과 : X(1)");//2021-10-26 심사팀장 의뢰서
                cboSpec.Items.Add("JT033.혈정 포타슘 검사 결과 : 9(2).V9(1)");//2021-10-26 심사팀장 의뢰서
                cboSpec.Items.Add("JT017.내용액제 처방(조제)사유");
                cboSpec.Items.Add("JT018.요류역학검사결과");
                cboSpec.Items.Add("JT020.초음파검사, MRI검사 시행일자");
                cboSpec.Items.Add("JT021.경피적관상동맥스텐트 삽입혈관");
                cboSpec.Items.Add("JT024.골밀도검사");
                cboSpec.Items.Add("JJ001.필요시 투약하는약제(PRN)처방(조제)(의료기관)");
                cboSpec.Items.Add("JJ002.신의료기술등 명칭");
                cboSpec.Items.Add("JJ005.응급의료행위 산정시 면허종류/면허번호");
                cboSpec.Items.Add("JJ006.입원사유코드/입원기간");
                cboSpec.Items.Add("JX999.기타내역");
                cboSpec.SelectedIndex = 0;
                lblTitle.Text = "진료내역줄단위";
            }
            else if (optGB2.Checked == true)
            {
                cboSpec.Items.Add("JT007.치매검사결과");
                cboSpec.Items.Add("JT014.향정신성약물 장기처방 사유");
                cboSpec.Items.Add("JT017.내용액제 처방(조제)사유");
                cboSpec.Items.Add("JT018.요류역학검사결과");
                cboSpec.Items.Add("JX999.기타내역");
                cboSpec.SelectedIndex = 0;
                lblTitle.Text = "처방내역줄단위";
            }
            else
            {
                cboSpec.Items.Add("CT001.동일 성분 의약품 중복처방사유");
                cboSpec.Items.Add("CT002.약국본인부담차등적용");
                cboSpec.SelectedIndex = 0;
                lblTitle.Text = "처방내역단위";
            }
        }

        private void OptGB_Click(object sender, EventArgs e)
        {
            if (sender == optGB0)
            {
                cboSpec.Items.Clear();

                if (TID.Bi == "31")
                {
                    cboSpec.Items.Add("MB001.간병인현황");
                    cboSpec.Items.Add("MB002.병행진료");
                    cboSpec.Items.Add("MB003.특별진찰선택진료료");
                    cboSpec.Items.Add("MB004.최초입원일자");
                    cboSpec.Items.Add("MB005.특정내역");
                }

                if (TID.DrgCode != "")
                {
                    cboSpec.Items.Add("MS004.신생아체중");
                    cboSpec.Items.Add("MS011.야간 및 공휴일 수술 ccyymmddhhmm: 201307081850 ");              //drg
                    cboSpec.Items.Add("MT006.분만: 재왕절개만출술의 경우 임신주수를 기재 2자리");        // DRG
                    cboSpec.Items.Add("MT041.산부인과 가산점수산정 :1자리 Y");      //DRG
                    cboSpec.Items.Add("MT039.복강경 수술중 개복하여수술 :1자리");      //DRG
                }

                cboSpec.Items.Add("MS001.원내투약일수(경구/외용)");
                cboSpec.Items.Add("MS002.원내투약일수(주사제)");
                cboSpec.Items.Add("MS003.의약분업예외구분코드");
                cboSpec.Items.Add("MS004.신생아체중");
                cboSpec.Items.Add("MS005.낮병동 재원시간");
                cboSpec.Items.Add("MS006.개두술 수술일자");
                cboSpec.Items.Add("MS007.암질환 Stage 분류");
                cboSpec.Items.Add("MS008.암질환 TNM 분류");
                cboSpec.Items.Add("MS009.항암화학요법 투여단계 및 주기");
                cboSpec.Items.Add("MS010.민원처리결과급여결정진료분: 1자리 "); //drg
                cboSpec.Items.Add("MS011.야간 및 공휴일 수술 ccyymmddhhmm: 201307081850 "); //drg
                cboSpec.Items.Add("MS013.단순초음파 "); //2016-10-07
                cboSpec.Items.Add("MT001.상해외인");
                cboSpec.Items.Add("MT002.특정기호");
                cboSpec.Items.Add("MT003.개방병원진료시의뢰기관기호");
                cboSpec.Items.Add("MT004.소명자료 구분");
                cboSpec.Items.Add("MT005.주민등록번호 이상건");
                cboSpec.Items.Add("MT006.분만: 재왕절개만출술의 경우 임신주수를 기재 2자리");        //DRG
                cboSpec.Items.Add("MT007.DRG세부내역 내역구분(식대:EAT, 외과:SUR, 건강보험100/100: ALL)/투여일시/코드구분/코드/단가/1일투여량/총투여일수/금액/준용명 ");  //DRG
                cboSpec.Items.Add("MT010.폐렴 정보");
                cboSpec.Items.Add("MT011.폐혈증 정보");
                cboSpec.Items.Add("MT014.산정특례대상자 중증(완전틀니)환자등록번호");
                cboSpec.Items.Add("MT015.제출자료목록표");
                cboSpec.Items.Add("MT016.제출자료목록표(기타)");
                cboSpec.Items.Add("MT018.본인부담코드");
                cboSpec.Items.Add("MT019.진료확인번호");
                cboSpec.Items.Add("MT020.의료급여수급권자 및 차상위대상자 원내직접조제투약횟수");
                cboSpec.Items.Add("MT021.입원유형(1.자의입원, 2.보호의무자에의한 입원, 3.시장, 군수 구정창퇴원, 4.응급입원 9.기타)");
                cboSpec.Items.Add("MT022.퇴원유형(1.자의퇴원, 2.보호의무자에의한 퇴원, 3.정신보건심의위원회명령 퇴원, 9기타)");
                cboSpec.Items.Add("MT023.퇴원후거주지(1.자가, 2.사회복지시설, 9기타)");
                cboSpec.Items.Add("MT024.임부정보 및 임부금기의약품 처방( 조제) 사유");
                cboSpec.Items.Add("MT025.물리치료사공휴일근무현황");
                cboSpec.Items.Add("MT026.인공호흡시간");
                cboSpec.Items.Add("MT027.영아체중");
                cboSpec.Items.Add("MT028.산정특례대상 세부상병명");
                cboSpec.Items.Add("MT029.진찰횟수");
                cboSpec.Items.Add("MT031.인공수정체재료대 사용내역기재");  //DRG
                cboSpec.Items.Add("MT033.(내시경적 점막하 박리절제술 직검사결과)");
                cboSpec.Items.Add("MT034.행위 질병군 분리청구의 경우 최초입원 개시일 ccyymmdd "); //drg
                cboSpec.Items.Add("MT035.입원시 상병 유무");    //drg
                cboSpec.Items.Add("MT036.의료의 질 점검 내용");    //drg
                cboSpec.Items.Add("MT037.레진상틀니 및 타상명진료");
                cboSpec.Items.Add("MT039.복강경 수술중 개복하여수술 :1자리");     //DRG
                cboSpec.Items.Add("MT040.본인부담금 발생 횟수");
                cboSpec.Items.Add("MT041.산부인과 가산점수산정 :1자리 Y");   //DRG
                cboSpec.Items.Add("MT043.코로나 지원대상"); //DRG

                cboSpec.Items.Add("MT046.응급환자중증도 분류기준 :1자리");     //2016-01-11
                cboSpec.Items.Add("MT048.응급의료센터 구분코드 :1자리");      //2016-01-11
                cboSpec.Items.Add("MT049.최초입원시점");                 //2016-01-11
                cboSpec.Items.Add("MT051.조산아 등록번호");                     //2017-02-27
                cboSpec.Items.Add("MT052.치매질환사전승인번호");                       //2018-02-13
                cboSpec.Items.Add("MT059.문제의약품 유형");                      //2019-10-14
                cboSpec.Items.Add("MT060.인공수정체 제외금액 유형");             //2020-01-07
                cboSpec.Items.Add("MT063.요양병원 기관기호/의뢰일자");           //2020-01-06
                cboSpec.Items.Add("MT064.질병군 적용제외 유형");           //2020-07-27
                cboSpec.Items.Add("MT066.진료의뢰회송번호");           //2020-11-16
                cboSpec.Items.Add("MT070.폐렴 중증도 판정 결과");           //2021-10-26

                cboSpec.Items.Add("MT998.100/100진료(조제)내역");
                cboSpec.Items.Add("MT999.100/100약제 처방 내역");
                cboSpec.Items.Add("MJ001.가장간호구분자");
                cboSpec.Items.Add("MJ002.환자납부액 발생사유");
                cboSpec.Items.Add("MX999.기타내역");


                READ_SPEC_USER_SET("1", cboSpec, "");

                cboSpec.SelectedIndex = 0;
                lblTitle.Text = "명세서단위";
            }
            else if (sender == optGB1)
            {
                cboSpec.Items.Clear();

                if (GstrSpecBun == "49")
                {
                    cboSpec.Items.Add("JT020.초음파검사, MRI검사 시행일자");
                    cboSpec.Items.Add("JS013.단순,유도 초음파");
                    cboSpec.Items.Add("JT005.분만");
                }

                if (TID.Bi == "31")
                {
                    cboSpec.Items.Add("JB001.진료담당의사간병필요소견");
                    cboSpec.Items.Add("JB002.이송료");
                    cboSpec.Items.Add("JB003.재활보조기구");
                    cboSpec.Items.Add("JB004.확인수수료");
                    cboSpec.Items.Add("JB005.향정신약물처방(조제)사유");
                    cboSpec.Items.Add("JB006.상급병실사용사유");
                    cboSpec.Items.Add("JB007.선택진료의사");
                }

                if (GstrSpecBun == "49")
                {
                    cboSpec.Items.Add("JT002.진찰료");
                    cboSpec.Items.Add("JS010.야간가산, 응급의료수가");
                }
                else
                {
                    cboSpec.Items.Add("JS010.야간가산, 응급의료수가");
                    cboSpec.Items.Add("JT002.진찰료");
                }

                cboSpec.Items.Add("JS001.마취과전문의");
                cboSpec.Items.Add("JS002.의약분업예외코드");
                cboSpec.Items.Add("JS003.입원시각");
                cboSpec.Items.Add("JS004.퇴원시각");
                cboSpec.Items.Add("JS005.검체검사위탁");

                cboSpec.Items.Add("JT003.집중치료실(신생아집중치료실포함)");
                cboSpec.Items.Add("JT004.신생아집중치료실");
                cboSpec.Items.Add("JT001.확인코드");
                cboSpec.Items.Add("JT005.분만");
                cboSpec.Items.Add("JT007.치매검사결과");
                cboSpec.Items.Add("JT010.저함량배수처방(조제)관련");
                cboSpec.Items.Add("JT014.향정신성약물 장기처방 사유");
                cboSpec.Items.Add("JT015.(내시경적 점막하 박리절제술 병리조직검사결과)");
                cboSpec.Items.Add("JT016.(내시경적 점막하 박리절제술 시술의사) 기재 형식:면허번호/의사");
                cboSpec.Items.Add("JT025.내시경적 점막하 박리절제술(ESD) 조직유형분류 기재 형식:숫자(1)/숫자(!)");//2019-04-22,김해수, 심사팀장님 요청 새로운코드 추가
                cboSpec.Items.Add("JT031.혈청크레아티닌 검사 결과 : 9(2).V9(1)");//2021-10-26 심사팀장 의뢰서
                cboSpec.Items.Add("JT032.요단백 검사결과 : X(1)");//2021-10-26 심사팀장 의뢰서
                cboSpec.Items.Add("JT033.혈정 포타슘 검사 결과 : 9(2).V9(1)");//2021-10-26 심사팀장 의뢰서
                cboSpec.Items.Add("JS001.마취과전문의");
                cboSpec.Items.Add("JS002.의약분업예외구분코드");
                cboSpec.Items.Add("JS005.검체검사 위탁");
                cboSpec.Items.Add("JS006.시설등의공동이용진료");
                cboSpec.Items.Add("JS007.개방병원의뢰진료");
                cboSpec.Items.Add("JS008.위탁진료");
                cboSpec.Items.Add("JS009.준용명");
                cboSpec.Items.Add("JS010.야간가산, 응급의료수가");
                cboSpec.Items.Add("JS011.혈명코드");
                cboSpec.Items.Add("JS013.단순,유도 초음파");
                cboSpec.Items.Add("JT001.확인코드");

                cboSpec.Items.Add("JT006.DUR관련확인코드");
                cboSpec.Items.Add("JT011.병용 연령금기등 약제처방사유");
                cboSpec.Items.Add("JT012.동일성분 의약품 중복처방");
                cboSpec.Items.Add("JT013.수술일자");
                cboSpec.Items.Add("JT017.내용액제 처방(조제)사유");
                cboSpec.Items.Add("JT018.건강검진 실시 당일 진찰료 산정사유");
                cboSpec.Items.Add("JT019.필요시 투약하는약제(PRN),처방(조제)(의료기관)");
                cboSpec.Items.Add("JT020.초음파검사, MRI검사 시행일자");
                cboSpec.Items.Add("JT021.경피적관상동맥스텐트 삽입혈관");
                cboSpec.Items.Add("JT023.신경인지기능검사 세부 검사항목코드");
                cboSpec.Items.Add("JT024.골밀도검사");
                cboSpec.Items.Add("JJ001.선택진료의사");
                cboSpec.Items.Add("JJ002.신의료기술등 명칭");
                cboSpec.Items.Add("JJ005.응급의료행위 산정시 면허종류/면허번호");
                cboSpec.Items.Add("JJ006.입원사유코드/입원기간");
                cboSpec.Items.Add("JX999.기타내역");

                READ_SPEC_USER_SET("2", cboSpec, "");

                cboSpec.SelectedIndex = 0;
                lblTitle.Text = "진료내역단위";
            }
            else if (sender == optGB2)
            {
                cboSpec.Items.Clear();
                cboSpec.Items.Add("JT007.치매검사결과");
                cboSpec.Items.Add("JT014.향정신성약물 장기처방 사유");
                cboSpec.Items.Add("JT010.저함량배수처방(조제)관련");
                cboSpec.Items.Add("JT011.병용 연령금기등 약제처방사유");
                cboSpec.Items.Add("JT012.동일성분 의약품 중복처방");
                cboSpec.Items.Add("JT017.내용액제 처방(조제)사유");
                cboSpec.Items.Add("JT018.건강검진 실시 당일 진찰료 산정사유");
                cboSpec.Items.Add("JT019.필요시 투약하는약제(PRN),처방(조제)(의료기관)");
                cboSpec.Items.Add("JX999.기타내역");

                cboSpec.SelectedIndex = 0;
                lblTitle.Text = "처방내역줄단위";
            }
            else
            {
                cboSpec.Items.Clear();
                cboSpec.Items.Add("CT001.동일성분 의약품 중복");
                cboSpec.SelectedIndex = 0;
                lblTitle.Text = "처방내역단위";
            }
        }

        private void READ_SPEC_USER_SET(string argGubun, ComboBox argCombo, string argBi)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //1.EDI_특정내역세팅_명세서
            //2.EDI_특정내역세팅_줄단위
            SQL = " SELECT CODE,NAME ";
            SQL = SQL + " FROM KOSMOS_PMPA.BAS_BCODE ";
            if (argGubun == "1")
            {
                SQL = SQL + " WHERE GUBUN='EDI_특정내역세팅_명세서' ";
            }
            else if (argGubun == "2")
            {
                SQL = SQL + " WHERE GUBUN='EDI_특정내역세팅_줄단위' ";
            }

            SQL = SQL + "  AND (DELDATE IS NULL OR DELDATE ='')";
            SQL = SQL + " ORDER BY CODE ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    argCombo.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                }
            }

            dt.Dispose();
            dt = null;

        }
        
        private void MenuReFlash()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL = SQL + "SELECT WRTNO, SEQNO1, SEQNO2, GUBUN, JULNO, SPECCODE, SPEC, ROWID  " + ComNum.VBLF;
            SQL = SQL + "  FROM " + FstrTable + ComNum.VBLF;
            SQL = SQL + " WHERE WRTNO = '" + GnChoiceWRTNO + "' " + ComNum.VBLF;

            if (TID.Bi == "31")
            {
                SQL = SQL + " AND BI ='31' " + ComNum.VBLF;
            }
            else
            {
                SQL = SQL + " AND ( BI NOT IN ( '31') OR BI IS NULL) " + ComNum.VBLF;
            }

            if (optGB0.Checked == true)
            {
                SQL = SQL + "   AND GUBUN ='1' " + ComNum.VBLF;
            }
            else if (optGB1.Checked == true)
            {
                SQL = SQL + "   AND GUBUN ='2'" + ComNum.VBLF;
                SQL = SQL + "   AND WRTNOS = '" + GNSpecWRTNO + "'" + ComNum.VBLF;
                //'       SQL  = SQL  + " AND SEQNO1 = '" + GstrSeqNo1 + "' "
            }
            else if (optGB2.Checked == true)
            {
                SQL = SQL + "   AND GUBUN ='3'" + ComNum.VBLF;
                SQL = SQL + "   AND WRTNOS = '" + GNSpecWRTNO + "'" + ComNum.VBLF;
            }
            else
            {
                SQL = SQL + "   AND GUBUN ='4'" + ComNum.VBLF;
            }

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);


            ss1.ActiveSheet.RowCount = 0;
            ss1.ActiveSheet.RowCount = dt.Rows.Count;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ss1.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["GuBun"].ToString();
                ss1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["JULNO"].ToString();
                ss1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["SPECCODE"].ToString();
                ss1.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["SPEC"].ToString();
                ss1.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["SEQNO1"].ToString();
                ss1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["SEQNO2"].ToString();
                ss1.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["WRTNO"].ToString();
                ss1.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString();
            }

            dt.Dispose();
            dt = null;
        }

        private void MenuReFlash_Click(object sender, EventArgs e)
        {
            MenuReFlash();


        }

        private void FrmComMirSpec_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ss1.ActiveSheet.NonEmptyRowCount == 0)
            {
                GNSpecWRTNO = 0;
            }
        }
                
        private void CboSpec_LostFocus(object sender, EventArgs e)
        {
            switch (VB.Left(cboSpec.Text, 5))
            {
                case "MS001": lblHelp.Text = "원내조제 (경구/외용)투약일이 3일인경우 3자리: 003"; break;
                case "MS002": lblHelp.Text = "원내조제 (주사)투약일이 1일인경우 3자리 : 001 "; break;
                case "MS003": lblHelp.Text = "의약분업 예외구분코드 2자리 정신과정액 : 13 "; break;
                case "MS004": lblHelp.Text = "신상아 체중 그람 단위로 표시 2.84 kg  4자리 : 2840 "; break;
                case "MS005": lblHelp.Text = "낮병동재원시간:2005.1.3일 오전 10시10분부터 오후 5시까지 : 200501031010/200501031700 "; break;
                case "MS006": lblHelp.Text = "년도수술일자CCYYMMDD수술일자 2005년 10월03일 경우: 20051003"; break;
                case "MS007": lblHelp.Text = "암질환 Stage 분류 위유문부 악성 신생물로 StageIIIA인경우: C164/3A"; break;
                case "MS008": lblHelp.Text = "암질환 TNM 분류 위유문부 악성 신생물로 T2aN2M0 인경우: C164/T2a/N2/M0"; break;
                case "MS009": lblHelp.Text = "항암화학요법 투여댈계 및 주기 항암요법 1차(1St line)에 3주기부터 5주기까지 투여시: 1/03/05 "; break;
                case "MS010": lblHelp.Text = "민원처리결과급여결정진료분: 한자리 "; break;   //drg
                case "MS011": lblHelp.Text = "야간 및 공휴일 수술 ccyymmddhhmm: 201307081850 "; break; //drg
                case "MS013": lblHelp.Text = "단순초음파"; break; //2016-10-07
                case "MT001": lblHelp.Text = "상해외인 추갈으로 인한 목뼈의 골절인 경우 : W"; break;
                case "MT002": lblHelp.Text = "특정기호 만성신부증환자의 인공신장투석시 4자리 : V001"; break;
                case "MT003": lblHelp.Text = "개방병원 요양기호 8자리/입원(1),외래(2) 한자리"; break;
                case "MT004": lblHelp.Text = "전산청구시 우편 또는 전송망등으로 명세서와 관련된 소명차료첨부시:  Y"; break;
                case "MT005": lblHelp.Text = "건강보험증과 주민등록상의 주민등록번호가 상이한 경우 13자리"; break;
                case "MT006": lblHelp.Text = "분만: 재왕절개만출술의 경우 임신주수를 기재 2자리 "; break;        //DRG
                case "MT007": lblHelp.Text = "DRG세부내역 내역구분(식대:EAT, 외과:SUR, 건강보험100/100: ALL)/투여일시/코드구분/코드/단가/1일투여량/총투여일수/금액/준용명 "; break;  //DRG
                case "MT010": lblHelp.Text = "폐렴 정보 "; break;  //DRG
                case "MT011": lblHelp.Text = "패혈증 정보 "; break;  //DRG
                case "MT014": lblHelp.Text = "산정특례대상자등로번호 15자리"; break; //DRG

                case "MT015": lblHelp.Text = "제출자료목록표:  제출자료코드/제출자료코드(03/61 "; break; 
                case "MT016": lblHelp.Text = "제출자료목록표(기타):  협의진료기록지"; break;
                case "MT018": lblHelp.Text = "본인부담확인코드"; break;
                case "MT019": lblHelp.Text = "진료확인번호: 23자리코드"; break;
                case "MT020": lblHelp.Text = "의료급여수급권자 및 차상위대상자 원내직접조제투약횟수: 2자리 횟수"; break;
                case "MT021": lblHelp.Text = "입원유형(1.자의입원, 2.보호의무자에의한 입원, 3.시장, 군수 구정창퇴원, 4.응급입원 9.기타)"; break;
                case "MT022": lblHelp.Text = "퇴원유형(1.자의퇴원, 2.보호의무자에의한 퇴원, 3.정신보건심의위원회명령 퇴원, 9기타)"; break;
                case "MT023": lblHelp.Text = "퇴원후거주지(1.자가, 2.사회복지시설, 9기타)"; break;
                case "MT024": lblHelp.Text = "임부정보 및 임부금기의약품 처방( 조제) 사유"; break;

                case "MT025": lblHelp.Text = "물리치료사공휴일근무현황: ccyymmdd/x.x"; break;
                case "MT026": lblHelp.Text = "인공호흡시간: 5자리"; break;
                case "MT027": lblHelp.Text = "영아체중: 4자리"; break;
                case "MT028": lblHelp.Text = "산정특례대상 세부상병명: 상병분류기호: 5자리 / 세부상병명(한글) 60자리"; break;
                case "MT029": lblHelp.Text = "진찰횟수 2자리"; break;
                case "MT031": lblHelp.Text = "인공수정체재료대 사용내역기재"; break;  //DRG


                case "MT033": lblHelp.Text = "(내시경적 점막하 박리절제술 시술의사) 기재 형식:면허번호/의사"; break;

                case "MT034": lblHelp.Text = "행위 질병군 분리청구의 경우 최초입원 개시일 ccyymmdd "; break;   //drg
                case "MT035": lblHelp.Text = "입원시 상병 유무"; break; //drg
                case "MT036": lblHelp.Text = "의료의 질 점검 내용"; break;    //drg

                case "MT037": lblHelp.Text = "레진상틀리 및 타 상병 진료 기재 형식:M"; break;
                case "MT039": lblHelp.Text = "복강경 수술중 개복하여수술 :1자리"; break;      //DRG
                case "MT040": lblHelp.Text = "본인부담금 발생횟수 :2자리"; break;     //DRG
                case "MT041": lblHelp.Text = "산부인과 가산점수산정 :1자리 Y"; break;     //DRG
                case "MT043": lblHelp.Text = "3(기타)/02(신종코로나바이러스감염증)"; break;


                case "MT046": lblHelp.Text = "응급환자중증도 분류기준 :1자리"; break;  //2016-01-11 
                case "MT048": lblHelp.Text = "응급의료센터구분코드 :1자리"; break;    //2016-01-11
                case "MT049": lblHelp.Text = "최초입원시점"; break;                  //2016-01-11

                case "MT051": lblHelp.Text = "조산아 등록번호"; break;                      //2017-02-27
                case "MT059": lblHelp.Text = "X(1)/X(2) A/01"; break;
                case "MT060": lblHelp.Text = "1.연성,단안제외 2.연성,양안제외 3.경성,단안제외 4.경성 양안제외"; break;
                case "MT063": lblHelp.Text = "요양병원 기관기호/의뢰일자"; break;                    //2020-01-06
                case "MT064": lblHelp.Text = "X(1)/X(2)"; break; //2020-07-27
                case "MT066": lblHelp.Text = "요양병원기관기호/년월일/의뢰번호(5)"; break;  //2020-11-16
                case "MT070": lblHelp.Text = "X(1)/9(3)/X(2)/9(1)   폐렴구분코드/산소포화도수치/중증도판정도구코드/중증도판정점수"; break;  //2021-10-26

                case "MT998":
                    lblHelp.Text = "100/100 진료(조제)내역 제픽스정을 1일 1정씩 30일 분을 100/100한경우  " + ComNum.VBLF;
                    lblHelp.Text += "  코드구분/코드/1일투여량(실시횟수)/총두여(실시)일수/준용명 " + ComNum.VBLF;
                    lblHelp.Text += "  3/A39101341/1/30";
                    break;
                case "MT999":
                    lblHelp.Text = "100/100 처방내역 제픽스정을 1일 1정씩 30일분 100/100 원외처방한 경우" + ComNum.VBLF;
                    lblHelp.Text += " 코드구분/코드/1회투약량/1일투여횟수/총투여일수 " + ComNum.VBLF;
                    lblHelp.Text += " 3/A39101341/1/1/30";
                    break;

                case "MJ001": lblHelp.Text = "가정간호구분자 한자리: Z"; break;     //자보
                case "MJ002": lblHelp.Text = "환자납부액 발생사유: 200자"; break;   //자보
                case "MX999": lblHelp.Text = "기타내역 영문(700자), 한글 (350자) 평문기재 "; break;

                case "JS001": lblHelp.Text = "마취과전문의사를 초방하여마취를 실시한 경우 주민등록번호/자격번호/성명:6502011123456/123456/홍길동"; break;
                case "JS002": lblHelp.Text = "의약분업 예외구분코드 퇴장방지약의 경우 : 99 "; break;
                case "JS003": lblHelp.Text = "입원시각 2005.1.13일 05시 20분에 입원한 경우 : 200501130520"; break;
                case "JS004": lblHelp.Text = "퇴원시각 2005.1.12일 19시 30분에 퇴원한 경우 : 200501121930"; break;
                case "JS005": lblHelp.Text = "검체검사 위탁한경우 수탁기관기호/검사의뢰일: 11366036/20050105"; break;
                case "JS006": lblHelp.Text = "시설등의 공동이용진료 실시기관기호/진료의뢰일: 9(8)/20050130"; break;
                case "JS007": lblHelp.Text = "개방병원 의뢰진료 개방병원기호/의뢰일: 9(8)/20050120"; break;
                case "JS008": lblHelp.Text = "위탁진료(공동이용지료 계약하지않은경우) 시실기관기호/진료의뢰일: 9(8)/20051012"; break;
                case "JS009":
                    lblHelp.Text = "준용명 무의식 상태 등의 환자의 치료상 반드시 체중측정이 필요하여 특수 체중계로 체중을 측정한경우 " + ComNum.VBLF;
                    lblHelp.Text += " : 침상내 체중측정, 자2-1-바 체위변경처치 *50% ";
                    break;
                case "JS010": lblHelp.Text = "야간가산 진찰료, 수술처치,마취료등 야간 가산시 실시시간: 2005년 1월 8일 15시 30분 내원 : 200501081530"; break;
                case "JS011": lblHelp.Text = "혈명 코드(한방의료기관에서 침술시) "; break;
                case "JS013": lblHelp.Text = "단순,유도 초음파"; break;
                case "JT001": lblHelp.Text = "확인코드 진료행위에 대한 추가기술 사항 족관절(양측) 촬영한 경우 : B"; break;
                case "JT002": lblHelp.Text = "진찰료 1일 2회  산정시 진료과목/진찰일/진료과목/진찰일... : 01/20050113/05.20050103"; break;
                case "JT003": lblHelp.Text = "집중치료실입원 기간 FROM / TO 2005.1.13부터 2005.1.17 : 20051013/20050117 "; break;
                case "JT004":
                    lblHelp.Text = "신생아집중치료실 입원한경우 재태기간,출생시체중 " + ComNum.VBLF;
                    lblHelp.Text += " 33주 2일째 1450g의 체중으로 출생한 신생아가 2005.1.25 부터  2005.1.30일까지 " + ComNum.VBLF;
                    lblHelp.Text += " : JT003 20050125/20050103 , JT004 33/1450 ";
                    break;
                case "JT005": lblHelp.Text = "분만 임신주수 38주 6일에 셋째아이 출산 : 38"; break;
                case "JT007": lblHelp.Text = "치매검사결과: MMSE/YYYYMMDD/CDR.0/YYYYMMDD/GDS/YYYYMMDD/"; break;
                case "JT010":
                    lblHelp.Text = "저함량배수(조제)관련 저방사유코드(A,B,C,E)/저함량 의약품 배수 처방(조제)사유";
                    txtSpec.Text = "E/2V#3";
                    break;
                case "JT011": lblHelp.Text = "병용 연령금기등 약제처방사유(400)"; break;
                case "JT012": lblHelp.Text = "동일성분 의약품 중복처방사유코드(A,B,C,D)/구체적 사유"; break;
                case "JT013": lblHelp.Text = "수술일자: CCYYMMDD : 20100702"; break;
                case "JT014": lblHelp.Text = "향정신성약물장기처방사유: Y/구체적사유"; break;
                case "JT015": lblHelp.Text = "(내시경적 점막하 박리절제술 병리조직검사결과) 기재형식:X(150)/X(150)/X(1)/X(1)/X(1)/9(3)/9(3)"; break;
                case "JT016": lblHelp.Text = "(내시경적 점막하 박리절제술 시술의사) 기재 형식:면허번호/의사"; break;
                case "JT017": lblHelp.Text = "내용액제 처방(조제)사유: 기재형식: X(1)/X(200) "; break;
                case "JT018": lblHelp.Text = "건강검진 실시 당일 진찰료 산정사유"; break;
                case "JT019": lblHelp.Text = "필요시 투약하는 약제(PRN),처방(조제)의료기관 한자리: P"; break;
                case "JT020": lblHelp.Text = "초음파검사, MRI검사 시행일자 CCYYMMDD: 20131017"; break;
                case "JT021": lblHelp.Text = "경피적관상동맥스텐트 삽입혈관 9(1)"; break;
                case "JT024":lblHelp.Text =  "골밀도검사  X(1) / 9(1)V9(1)"; break;
                    

                case "CT001": lblHelp.Text = "동일성분 의약품 중복처방: 사유코드(A,B,C,D)/구체적 사유"; break;
                case "CT002": lblHelp.Text = "약국본인부담차등: V252"; break;
                //case "JT001": lblHelp.Text = "선택지료의사: 주민번호(-:생략)/자격번호/성명"; break;
                //case "JT004": lblHelp.Text = "신의료기술등명칭 ccyymmdd/신의료기술등 명칭"; break;
                case "JT006": lblHelp.Text = "약국조제시 상호금기 연령 금기약제 사용시 상호금기(1),연령금기(2)/확인시간/관련조제"; break;

                case "JJ001": lblHelp.Text = "선택진료의사"; break;
                case "JJ004": lblHelp.Text = "신의료기술등 명칭"; break; 

                case "JX999": lblHelp.Text = "기타 추가내역은 평문으로 기재 영문(700자),한글(350자)"; break; 
                case "MB001": lblHelp.Text = "간병인현황:기재형식 이름/주민번호/면허(자격)번호)/자격취득일자/입사일(최초근무일)/퇴사일(근무종료일)"; break;
                case "MB002": lblHelp.Text = "병행진료사유코드(2자리)"; break;
                case "MB003": lblHelp.Text = "특진진찰료선택진료료 기재형식: 담당의사명/담당의사면허번호"; break;
                case "MB004": lblHelp.Text = "최초입원일자(CCYYMMDD): 입원청구시 동일 의료기관에 최초 입원한 일자를 기재"; break;
                case "MB005": lblHelp.Text = "특정내역 영문(700자), 한글 (350자) 평문기재 "; break;
                case "JB001": lblHelp.Text = "진료담당의사간병필요 소견: 기재형식 : 간병사유(200자)/담당의면허번호/담당의사명"; break;
                case "JB002": lblHelp.Text = "이송료: 기재형식: 이송수단/이송거리(km)/동행인/이송일자"; break;
                case "JB003": lblHelp.Text = "재활보조기구 적용일자기재: 기재형식 CCYYMMDD"; break;
                case "JB004": lblHelp.Text = "확인수수료가 발생된 경우에 기재 기재형식: 요양비용청구서구분코드(2자리)/발급일자"; break;
                case "JB005": lblHelp.Text = "향정신약물처방(조제)사유"; break;
                case "JB006": lblHelp.Text = "상급병실사용사유"; break;
                case "JB007": lblHelp.Text = "선택진료 면허종류/면허번호/성명"; break;
            }
        }
        
        private void BtnDelete_Click(object sender, EventArgs e)
        {

            string SQL = "";
            string SqlErr = "";

            int nRow = 0;
            

            nRow = ss1.ActiveSheet.ActiveRowIndex;
            

            int intRowAffected = 0;
            string strRowid = ss1.ActiveSheet.Cells[nRow, 7].Text;
            if (MessageBox.Show(ss1.ActiveSheet.Cells[nRow, 2].Text + " " + ss1.ActiveSheet.Cells[nRow, 3].Text + "을 정말삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                clsDB.setBeginTran(clsDB.DbCon);

                SQL = "";
                SQL = SQL + "DELETE " + FstrTable + " WHERE ROWID = '" + strRowid + "' ";
                clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("DELETE " + FstrTable + " ERROR!!!");
                    return;
                }

                if (optGB1.Checked == true)
                {
                    if (TID.Bi == "52")
                    {
                        SQL = "";
                        SQL = SQL + "UPDATE MIR_TADTL SET  WRTNOS = '' ";
                    }
                    else if (TID.Bi == "31")
                    {
                        SQL = "";
                        SQL = SQL + "UPDATE MIR_SANDTL SET  WRTNOS = '' ";
                    }
                    else
                    {
                        SQL = "";
                        SQL = SQL + "UPDATE MIR_INSDTL SET  WRTNOS = '' ";
                    }

                    SQL = SQL + " WHERE WRTNO = '" + GnChoiceWRTNO + "'";
                    SQL = SQL + "   AND WRTNOS = '" + GNSpecWRTNO + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("DELETE " + FstrTable + " ERROR!!!");
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //DialogResult = System.Windows.Forms.DialogResult.OK;
                MenuReFlash();// enuReFlash.PerformClick();

            }
            return;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;

            if (ComMirEntSpd.Spd_DataRowCnt(ss1) == 0)
            {
                GNSpecWRTNO = 0;
            }

            //this.FormSendEvent(this.GNSpecWRTNO);
            this.Close();
            //this.Dispose();

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string strFDate = "";
            string strTDate = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0;

            if (txtSpec.Text == "")
            {
                MessageBox.Show("특정내역이 공란입니다.");
                return;
            }

            //jt007 check

            if (VB.Left(cboSpec.Text,5) == "JT007" && TID.IpdOpd == "O")
            {
                strFDate = VB.Mid(txtSpec.Text, 4, 10);
                strFDate = VB.Left(strFDate, 4) + "-" + VB.Mid(strFDate, 5, 2) + "-" + VB.Left(strFDate, 2);

                strTDate = VB.Left(TID.JinDate1, 4) + "-" + VB.Mid(TID.JinDate1, 5, 2) + "-" + VB.Left(TID.JinDate1, 2);

                DateTime FDate = Convert.ToDateTime(strFDate);
                DateTime TDate = Convert.ToDateTime(strTDate);
                TimeSpan ts = TDate - FDate;
                if (365 < ts.Days)
                {
                    MessageBox.Show("골다공약제 365일 경과 하였습니다. " + ComNum.VBLF + ComNum.VBLF + "확인 바랍니다.");
                }
            }

            //clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                if (txtROWID.Text == "")
                {
                    SQL += "INSERT INTO " + FstrTable + " ( WRTNO, SEQNO1, SEQNO2, GUBUN, JULNO, SPECCODE, SPEC, WRTNOS, BI, SLIPNO )";
                    SQL += "VALUES ( '" + GnChoiceWRTNO + "', '" + txtSeqNo1.Text + "', '" + txtSeqNo2.Text + "', ";
                    if (optGB0.Checked == true)
                    {
                        SQL += " '1', ";
                    }
                    else if (optGB1.Checked == true)
                    {
                        SQL += " '2',";
                    }
                    else if (optGB2.Checked == true)
                    {
                        SQL += " '3',";
                    }
                    else
                    {
                        SQL += " '4',";
                    }

                    if (GstrSpec == "1" || GstrSpec == "4") //명세서단위, 처방내역 단위 는 0
                    { 
                        SQL += " '0', ";
                    }
                    else
                    {
                        SQL += " '" + txtJulNo.Text + "', ";
                    }
                    SQL += " '" + VB.Left(cboSpec.Text, 5) + "', '" + txtSpec.Text + "', '" + GNSpecWRTNO + "', '" + TID.Bi + "', '" + GstrSpecSlipNo + "'  )";
                }
                else
                {
                    SQL += "UPDATE " + FstrTable;
                    SQL += "   SET SPECCODE = '" + VB.Left(cboSpec.Text, 5) + "', ";
                    SQL += "       SPEC = '" + txtSpec.Text + "' ";
                    //SQL += "       AUTO = '' ";
                    SQL += " WHERE ROWID = '" + txtROWID.Text + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                //2016-08-05
                //JS003 >> MT049 자동 발생
                if (TID.Johap == "1" && TID.IpdOpd == "I" && (TID.KTASLVL == "1" || TID.KTASLVL == "2" || TID.KTASLVL == "3") && VB.Left(cboSpec.Text, 5) == "JS003")
                {
                    SQL = " SELECT ROWID ";
                    SQL += " FROM MIR_INSSPEC ";
                    SQL += "  WHERE WRTNO =" + GnChoiceWRTNO + " "; 
                    SQL += "   AND SPECCODE ='MT049' ";
                    SQL += "   AND GUBUN ='1' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (dt.Rows.Count > 0)
                    {
                        SQL = "";
                        SQL += " UPDATE MIR_INSSPEC ";
                        SQL += "   SET SPECCODE = 'MT049', ";
                        SQL += "       SPEC = '" + txtSpec.Text + "' ";
                        SQL += " WHERE ROWID = '" + VB.Trim(dt.Rows[0]["ROWID"].ToString()) + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    }
                    else
                    {
                        SQL = "";
                        SQL += "INSERT INTO " + FstrTable + " ( WRTNO, SEQNO1, SEQNO2, GUBUN, JULNO, SPECCODE, SPEC, WRTNOS, BI, SLIPNO )";
                        SQL += "VALUES ( '" + GnChoiceWRTNO + "', '" + txtSeqNo1.Text + "', '" + txtSeqNo2.Text + "', ";
                        SQL += " '1', ";
                        SQL += " '0', ";
                        SQL += " 'MT049', '" + txtSpec.Text + "', '" + GNSpecWRTNO + "', '" + TID.Bi + "', '" + GstrSpecSlipNo + "'  )";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (SqlErr != "")
                {
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show(FstrTable + " 수정중 Error!!!");
                    return;
                }
                else
                {
                    //clsDB.setCommitTran(clsDB.DbCon);
                }
                //DialogResult = System.Windows.Forms.DialogResult.OK;
                Screen_Clear();
                //menuReFlash.PerformClick();
                MenuReFlash();
                //DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //퇴사자 상관없이 한 화면에, 퇴사자를 밑으로 정렬
            SQL = "SELECT DRNAME, DRBUNHO FROM KOSMOS_OCS.OCS_DOCTOR";
            SQL += " WHERE DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' ";
            //'If ChkGbOut.Value = False Then SQL += "    AND GBOUT = 'N' "
            SQL += "   AND GRADE ='1' ";
            //2017-08-24
            SQL += " ORDER BY GBOUT, FDATE DESC ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ssDCT.ActiveSheet.Rows.Count = 0;
            ssDCT.ActiveSheet.Rows.Count = dt.Rows.Count;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ssDCT.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["DrName"].ToString();
                ssDCT.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DRBUNHO"].ToString();
            }

            dt.Dispose();
            dt = null;
        }

        private void FrmComMirSpec_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회
                                                                               // ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등 
            Cursor.Current = Cursors.WaitCursor;
         
            int i = 0;
            int nINXSAN = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;


            if (GstrGuBun == "RUTRAP" || GstrGuBun =="8") //자보
            {
                FstrTable = " MIR_TASPEC ";
            }
            else
            {
                FstrTable = " MIR_INSSPEC ";
            }

            if (GstrGuBun == "RUSANJ" ||  GstrGuBun == "7") //산재
            {
                nINXSAN = 4;
            }
            else
            {
                nINXSAN = 0;
            }

            txtSeqNo1.Text = "";
            txtSeqNo2.Text = "";
            txtJulNo.Text = "";
            txtSpec.Text = "";
            ss1.ActiveSheet.Rows.Count = 0;
            txtROWID.Text = "";
            lblHelp.Text = "";

            //FstrOK = "OK";

            if (GstrSpec == "1")
            {
                READ_OPD_MASTER();
                READ_NUR_ER_PATIENT();
                READ_ER_ACT();
                READ_OLD_INSSPEC();
                READ_ORAN_SLIP();

                optGB0.Checked = true;
                OptGB_Click(optGB0, e);
                lblTitle.Text = "명세서단위 [ " + GstrSpecTit + " ]";

                if (TID.IpdOpd == "I")
                {
                    //재원심사 특정 내역 조회
                    SQL = "SELECT SPECCODE ,SPEC  FROM KOSMOS_PMPA.MIR_IPDSPEC ";
                    SQL += " WHERE PANO = '" + TID.Pano + "' ";
                    //2017-12-19
                    //SQL = SQL + "   AND INDATE = TO_DATE('" + TID.JinDate1 + "','YYYYMMDD') "
                    SQL += "   AND ( INDATE = TO_DATE('" + TID.JinDate1 + "','YYYYMMDD') OR IPDNO = '" + TID.IPDNO + "')  ";
                    SQL += "   AND GBSPEC ='1' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    ss3.ActiveSheet.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss3.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["SPECCODE"].ToString().Trim();
                        ss3.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SPEC"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }


                if (TID.Johap == "5" && TID.IpdOpd == "I" && TID.DeptCode1 == "NP")
                {
                    cboSpec.SelectedIndex = 20 + nINXSAN;
                }
            }
            else if (GstrSpec == "2")
            {
                READ_OPD_MASTER();
                READ_NUR_ER_PATIENT();
                READ_ER_ACT();
                READ_OLD_INSSPEC();
                READ_ORAN_SLIP();

                optGB1.Checked = true;
                OptGB_Click(optGB1, e);
                lblTitle.Text = "진료내역 [ " + GstrSpecTit + " ]";

                switch (VB.Left(GstrSpecTit, 8).Trim())
                {
                    case "AJ200":
                    case "AJ210":
                    case "AJ210A":
                    case "AJ220":
                    case "AJ220A": //2021-03-15 이민주 선생님 의뢰서
                    case "AJ230":
                    case "AJ250":
                    case "AJ240":
                    case "AJ260":
                        {
                            //' ComboSpec.ListIndex = 4 + nINXSAN
                            cboSpec.Text = "JS003.입원시각";
                            //중환자실 입원 내역 조회
                            if (TID.IpdOpd == "I")
                            {
                                cboSpec.Text = "JT003.집중치료실(신생아집중치료실포함)";
                                READ_ITRANSFOR_NEW();
                            }
                        }
                        break;

                    //2018-05-03 추가 
                    case "AO200":
                    case "AO2001":
                    case "AO220":
                    case "AO2201":
                    case "AO280":
                    case "AO2801":
                        {
                            cboSpec.Text = "JS003.입원시각";
                            if (TID.IpdOpd == "I")
                            {
                                cboSpec.Text = "JT003.입원기간";
                                READ_INWARD_1("5인실");
                            }
                        }
                        break;
                    case "AO240":
                    case "AO2401":
                        {
                            cboSpec.Text = "JS003.입원시각";
                            if (TID.IpdOpd == "I")
                            {
                                cboSpec.Text = "JT003.입원기간";
                                READ_INWARD_1("4인실");
                            }
                        }
                        break;
                    case "AV222":
                    case "AV2221":
                    case "AV222A":
                    case "AV2221A":
                    case "AV820":
                    case "AV8201":
                        {
                            cboSpec.Text = "JS003.입원시각";

                            if (TID.IpdOpd == "I")
                            {
                                cboSpec.Text = "JT003.입원기간";
                                READ_INWARD_2();
                            }
                        }
                        break;

                    case "AJ250A":
                        {
                            cboSpec.SelectedIndex = 5 + nINXSAN;
                        }
                        break;
                    case "AA1761":
                    case "AA2761":
                    case "AJ230A":
                        //'ComboSpec.ListIndex = 2 + nINXSAN

                        cboSpec.Text = "JS010.야간가산, 응급의료수가";
                        break;

                    case "AB220":
                    case "AB240":
                    case "AB2201":
                        //'ComboSpec.ListIndex = 0
                        cboSpec.Text = "JS003.입원시각";
                        break;

                    case "AB2201A":
                    case "AB220A":
                    case "AJ240A":
                        //'ComboSpec.ListIndex = 0
                        cboSpec.Text = "JS004.퇴원시각";
                        break;
                    case "L3002002":
                    case "L3002106":
                    case "L3002503":
                    case "L3105006":
                    case "L3105302":
                    case "L3106102":
                    case "L3107002":
                    case "B4062A":
                    case "B4062B":
                        //'ComboSpec.ListIndex = 25
                        cboSpec.Text = "JX999.기타내역";
                        break;

                    case "W-RUKA":
                    case "W-CT05":
                    case "W-HMCT1":
                        cboSpec.Text = "JT010.저함량배수처방(조제)관련";
                        txtSpec.Text = "E/2V#3";
                        break;
                    case "ZOY200":
                        cboSpec.Text = "JT010.저함량배수처방(조제)관련";
                        txtSpec.Text = "E/4T#3";
                        break;
                    case "TMDS":
                        cboSpec.Text = "JT017.내용액제 처방(조제)사유";
                        break;
                    case "MM151A":
                    case "MM151":
                    case "AID70":
                    case "RLX60":
                    case "OPT75":
                    case "BONVI":
                    case "ACT35":
                        cboSpec.Text = "MX999.기타내역";
                        break;

                    //case "AID70":
                    //case "RLX60":
                    //case "OPT75":
                    //case "BONVI":
                    //case "ACT35":
                    //    cboSpec.Text = "MX999.기타내역";
                    //    txtSpec.Text = "BMD-2.5이하 결과/DXA";
                    //    break;
                    case "HA471A":
                        cboSpec.Text = "JS009.준용명";
                        txtSpec.Text = "";
                        break;
                    case "V2200":
                        cboSpec.Text = "JJ005.응급의료행위 산정시 면허종류/면허번호";
                        txtSpec.Text = "";
                        break;
                    //2019-07-09 김해수 1인실 AB901 코드 관련 추가 작업
                    case "AB901":
                        cboSpec.Text = "JX999.기타내역";
                        REAT_AB901_DAY();
                        break;
                    default:
                        cboSpec.SelectedIndex = 0;
                        break;
                }

                //2018-02-23
                if (GstrSpecTit.Contains("F001") || GstrSpecTit.Contains("F002") || GstrSpecTit.Contains("F003"))
                {
                    cboSpec.Text = "JT007.치매검사결과";
                    txtSpec.Text = "";
                }

                if (GstrSpecKtasER == "1" || GstrSpecKtasER == "2")
                {
                    cboSpec.Text = "JS010.야간가산, 응급의료수가";
                }
                else
                {
                    if (GstrC == "2")
                    {
                        cboSpec.Text = "JS010.야간가산, 응급의료수가";
                    }
                }

                if (GstrSpecBun == "49")
                {
                    //2021-03-23 김연서 선생님 의뢰서 
                    if(VB.Left(GstrSpecTit, 8).Trim() != "EB455001")
                    {
                        cboSpec.Text = "JT020.초음파검사, MRI검사 시행일자";
                    }
                    else
                    {
                        cboSpec.Text = "JX999.기타내역";
                    }
                    
                }

                SQL = " SELECT  ROWID FROM KOSMOS_PMPA.BAS_SUN WHERE GBDEMENTIA ='Y' ";
                SQL += " AND SUNEXT = '" + VB.Left(GstrSpecTit, 8).Trim() + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    cboSpec.Text = "JT007.치매검사결과";
                    if (GstrFrDate !="" && GstrTime !="")  txtSpec.Text = Convert.ToDateTime(GstrFrDate).ToString("yyyyMMdd") + Convert.ToDateTime(GstrTime).ToString("HHmm");
                }

                dt.Dispose();
                dt = null;

                if (GNSpecWRTNO == 0)
                {
                    SQL = "SELECT SEQ_WRTNOS.NEXTVAL MWRTNOS FROM DUAL ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (dt.Rows.Count > 0)
                    {
                        GNSpecWRTNO = (long)VB.Val(dt.Rows[0]["MWRTNOS"].ToString());
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (TID.IpdOpd == "I")
                {
                    //재원심사 특정 내역 조회
                    SQL = "SELECT SPECCODE ,SPEC  FROM KOSMOS_PMPA.MIR_IPDSPEC ";
                    //'SQL = SQL & "  WHERE PANO ='08303186'"
                    SQL += " WHERE PANO = '" + TID.Pano + "' ";
                    //'2017-12-19
                    //'SQL += "   AND INDATE = TO_DATE('" + TID.JinDate1 + "','YYYYMMDD') "

                    //2020-08-26
                    //SQL += "   AND (INDATE = TO_DATE('" + TID.JinDate1 + "','YYYYMMDD') OR IPDNO = '" + TID.IPDNO + "') ";

                    switch (TID.Bi)
                    {//자보분만변경
                        case "51":
                        case "52":
                            SQL += "   AND BDATE >= TO_DATE('" + TID.JinDate1 + "','YYYYMMDD')  ";
                            SQL += "   AND BDATE <= TO_DATE('" + VB.Replace(CF.READ_LASTDAY(clsDB.DbCon, TID.JinDate1),"-","") + "','YYYYMMDD')  ";
                            break;
                        default:
                            SQL += "   AND (INDATE = TO_DATE('" + TID.JinDate1 + "','YYYYMMDD') OR IPDNO = '" + TID.IPDNO + "') ";
                            break;

                    }
                    SQL += "   AND SUNEXT = '" + VB.Left(GstrSpecTit, 8).Trim() + "' ";
                    SQL += "   AND (GBSPEC ='2' OR GBSPEC IS NULL)  ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    ss3.ActiveSheet.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ss3.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["SPECCODE"].ToString().Trim();
                        ss3.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SPEC"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
            }
            else if (GstrSpec == "3")   //처방내역 줄번호 내역
            {
                if (GNSpecWRTNO == 0)
                {
                    SQL = "SELECT SEQ_WRTNOS.NEXTVAL MWRTNOS FROM DUAL ";
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (dt.Rows.Count > 0)
                    {
                        GNSpecWRTNO = (long)VB.Val(dt.Rows[0]["MWRTNOS"].ToString());
                    }

                    dt.Dispose();
                    dt = null;
                }

                optGB2.Checked = true;
                OptGB_Click(optGB2, e);
                lblTitle.Text = "원외 처방내역 줄단위 [ " + GstrSpecTit + " ]";
            }
            else
            {
                lblTitle.Text = "원외 처방내역 [ " + GstrSpecSlipNo + " ]";
                optGB3.Checked = true;
                OptGB_Click(optGB3, e);
            }

            READ_BUN();

            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept); 

            //menuReFlash.PerformClick();
            MenuReFlash();

            txtSeqNo1.Text = GnSeqNo1.ToString("0");

            txtJulNo.Text = GnJulNo.ToString();

            Cursor.Current = Cursors.Default;
        }

        private void READ_INWARD_2()
        {
            //간병입실/간병퇴실
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int nRow = -1;
            string strFDate = "";
            string strTDate = "";

            string strBDATE = "";

            strFDate = VB.Left(TID.IODate, 8);
            strTDate = VB.Right(TID.IODate, TID.IODate.Length - 8);

            if (strTDate == "")
            {
                strTDate = TID.OutDate.Replace("-", "");
            }

            SQL = "   SELECT  /*+INDEX(KOSMOS_PMPA.IPD_NEW_SLIP INDEX_IPDNEWSL5)*/   TO_CHAR(BDATE ,'YYYY-MM-DD') BDATE  ";
            SQL += "    FROM KOSMOS_PMPA.IPD_NEW_SLIP  " + ComNum.VBLF;
            SQL += "   WHERE PANO   ='" + TID.Pano + "' " + ComNum.VBLF;
            SQL += "     AND SUNEXT IN ('AV222','AV2221','AV222A','AV2221A','AV820','AV8201')  " + ComNum.VBLF;
            SQL += "     AND BDATE >=TO_DATE('" + strFDate + "','YYYYMMDD') " + ComNum.VBLF;
            SQL += "     AND BDATE <=TO_DATE('" + strTDate + "','YYYYMMDD') " + ComNum.VBLF;
            SQL += "  GROUP BY  BDATE" + ComNum.VBLF;
            SQL += "  Having Sum(Qty * Nal) <> 0" + ComNum.VBLF;
            SQL += "  ORDER BY BDATE ASC  " + ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);


            ss4.ActiveSheet.Rows.Count = 0;
            ss7.ActiveSheet.Rows.Count = 0;
            ss7.ActiveSheet.Rows.Count = dt.Rows.Count;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (strBDATE == "")
                {
                    nRow = nRow + 1;

                    ss7.ActiveSheet.Cells[nRow, 0].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");
                }
                else
                {
                    if (dt.Rows[i]["BDate"].ToString().Trim() != Convert.ToDateTime(strBDATE).AddDays(1).ToShortDateString())
                    {
                        nRow = nRow + 1;

                        ss7.ActiveSheet.Cells[nRow, 0].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");
                        
                    }
                }

                strBDATE = dt.Rows[i]["BDate"].ToString().Trim();
                ss7.ActiveSheet.Cells[nRow, 1].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");

            }

            dt.Dispose();
            dt = null;
        }

        private void READ_INWARD_1(string ArgInsil)
        {
            //입원입실/입원퇴실
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strFDate = "";
            string strTDate = "";

            string strBDATE = "";
            int nRow = -1;

            strFDate = VB.Left(TID.IODate, 8);
            strTDate = VB.Right(TID.IODate, TID.IODate.Length - 8);

            if (strTDate == "")
            {
                strTDate = TID.OutDate.Replace("-", "");
            }

            SQL = "   SELECT  /*+INDEX(KOSMOS_PMPA.IPD_NEW_SLIP INDEX_IPDNEWSL5)*/   TO_CHAR(BDATE ,'YYYY-MM-DD') BDATE  ";
            SQL += "    FROM KOSMOS_PMPA.IPD_NEW_SLIP  " + ComNum.VBLF; 
            SQL += "   WHERE PANO   ='" + TID.Pano + "' " + ComNum.VBLF;

            if(ArgInsil == "4인실")
            {
                SQL += "     AND SUNEXT IN ('AO240', 'AO2401')  " + ComNum.VBLF;
            }
            else
            {
                SQL += "     AND SUNEXT IN ('AO200','AO2001','AO220','AO2201','AO280','AO2801')  " + ComNum.VBLF;
            }
            
            SQL += "     AND BDATE >=TO_DATE('" + strFDate + "','YYYYMMDD') " + ComNum.VBLF;
            SQL += "     AND BDATE <=TO_DATE('" + strTDate + "','YYYYMMDD') " + ComNum.VBLF;
            SQL += "  GROUP BY  BDATE" + ComNum.VBLF;
            SQL += "  Having Sum(Qty * Nal) <> 0" + ComNum.VBLF;
            SQL += "  ORDER BY BDATE ASC  " + ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);


            ss4.ActiveSheet.Rows.Count = 0;
            ss6.ActiveSheet.Rows.Count = 0;
            ss6.ActiveSheet.Rows.Count = dt.Rows.Count;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //if (strBDATE == "")
                //{
                //    ss6.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");
                //}
                //else
                //{
                //    if (dt.Rows[i]["BDate"].ToString().Trim() != Convert.ToDateTime(strBDATE).AddDays(1).ToShortDateString())
                //    {
                //        ss6.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");
                //    }
                //}

                //strBDATE = dt.Rows[i]["BDate"].ToString().Trim();

                //ss6.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");
                
                if (strBDATE == "")
                {
                    nRow = nRow + 1;
                    ss6.ActiveSheet.Cells[nRow, 0].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");
                }
                else
                {
                    if (dt.Rows[i]["BDate"].ToString().Trim() != Convert.ToDateTime(strBDATE).AddDays(1).ToShortDateString())
                    {
                        nRow = nRow + 1;
                        ss6.ActiveSheet.Cells[nRow, 0].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");
                    }
                }
                strBDATE = dt.Rows[i]["BDate"].ToString().Trim();
                ss6.ActiveSheet.Cells[nRow, 1].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");
            }

            dt.Dispose();
            dt = null;

        }

        private void READ_BUN()
        {
            FstrBun = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = " SELECT BUN " + ComNum.VBLF;
            SQL += " FROM KOSMOS_PMPA.BAS_SUT " + ComNum.VBLF;
            SQL += " WHERE SUNEXT = '" + VB.Left(GstrSpecTit, 8).Trim() + "' " + ComNum.VBLF;
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count > 0)
            {
                FstrBun = dt.Rows[0]["Bun"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        private void READ_OPD_MASTER()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, TO_CHAR(STIME,'YYYYMMDDHH24MI') STIME ," + ComNum.VBLF;
            SQL = SQL + "TO_CHAR(JTIME,'YYYYMMDDHH24MI') JTIME ,TO_CHAR(outTIME,'YYYYMMDDHH24MI') outTIME , " + ComNum.VBLF;
            SQL = SQL + " TO_CHAR(STIME,'HH24MI') TIME,  DEPTCODE " + ComNum.VBLF;
            SQL = SQL + "  FROM OPD_MASTER " + ComNum.VBLF;
            SQL = SQL + " WHERE PANO = '" + TID.Pano + "' " + ComNum.VBLF;
            //'SQL = SQL + "   AND TO_CHAR(ACTDATE,'YYYYMM') = '" + GstrOpenYYMM + "' "
            SQL = SQL + "   AND ACTDATE >= TO_DATE('" + TID.JinDate1 + "' ,'YYYYMMDD') " + ComNum.VBLF;
            SQL = SQL + " ORDER BY BDATE " + ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ss2.ActiveSheet.Rows.Count = 0;
            ss2.ActiveSheet.Rows.Count = dt.Rows.Count;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ss2.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["BDate"].ToString().Trim();
                ss2.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["JTIME"].ToString().Trim();
                ss2.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["STIME"].ToString().Trim();
                ss2.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                ss2.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["OUTTIME"].ToString().Trim();

                if (dt.Rows[i]["Time"].ToString().Trim().CompareTo("17:30") >= 0)
                {
                    ss2.ActiveSheet.Rows[i].BackColor = Color.FromArgb(255, 200, 200);
                }
            }

            dt.Dispose();
            dt = null;
        }

        private void READ_NUR_ER_PATIENT()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "SELECT  TO_CHAR(JDate,'YYYYMMDD') JDATE ," + ComNum.VBLF;
            SQL += "TO_CHAR(INTIME,'YYYYMMDDHH24MI') INTIME ,TO_CHAR(outTIME,'YYYYMMDDHH24MI') outTIME  " + ComNum.VBLF;
            SQL += "  FROM NUR_ER_PATIENT " + ComNum.VBLF;
            SQL += " WHERE PANO = '" + TID.Pano + "' " + ComNum.VBLF;
            SQL += "   AND JDATE >= TO_DATE('" + TID.JinDate1 + "' ,'YYYYMMDD') " + ComNum.VBLF;
            SQL += " ORDER BY JDATE " + ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ss5.ActiveSheet.Rows.Count = 0;
            ss5.ActiveSheet.Rows.Count = dt.Rows.Count;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ss5.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["JDATE"].ToString().Trim();
                ss5.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["InTime"].ToString().Trim();
                ss5.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["OUTTIME"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;        
        } 

        private void READ_ER_ACT()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "SELECT  PTNO,TO_CHAR(BDATE,'YYYY-MM-DD') BDATE,ORDERCODE,SUCODE,BUN,QTY,NAL,GBSTATUS,ORDERNO,ACTSABUN,GBIOE," + ComNum.VBLF;
            SQL += " TO_CHAR(ACTTIME,'YYYYMMDDHH24MI') ACTTIME " + ComNum.VBLF;
            SQL += "  FROM KOSMOS_OCS.OCS_IORDER_ACT_ER " + ComNum.VBLF;
            SQL += " WHERE PTNO = '" + TID.Pano + "' " + ComNum.VBLF;
            SQL += "   AND BDATE >= TO_DATE('" + TID.JinDate1 + "' ,'YYYYMMDD') " + ComNum.VBLF;
            SQL += " ORDER BY BDATE,ORDERCODE,SUCODE,ACTTIME,GBSTATUS  " + ComNum.VBLF;
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ss51.ActiveSheet.Rows.Count = 0;
            ss51.ActiveSheet.Rows.Count = dt.Rows.Count;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ss51.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["PTNO"].ToString().Trim();
                ss51.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                ss51.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();

                ss51.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                ss51.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["Bun"].ToString().Trim();
                ss51.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["Qty"].ToString().Trim();
                ss51.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["Nal"].ToString().Trim();
                ss51.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["GBSTATUS"].ToString().Trim();
                ss51.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["OrderNo"].ToString().Trim();
                ss51.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["ACTTIME"].ToString().Trim();
                ss51.ActiveSheet.Cells[i, 10].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["ACTSABUN"].ToString().Trim());
                ss51.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["GBIOE"].ToString().Trim();
            }
        }

        private void READ_OLD_INSSPEC()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            SQL = " SELECT A.WRTNO FROM KOSMOS_PMPA.MIR_INSID A, KOSMOS_PMPA.MIR_INSDTL B   " + ComNum.VBLF;
            SQL += " WHERE A.WRTNO = B.WRTNO                                                " + ComNum.VBLF;
            SQL += " AND A.PANO = '" + TID.Pano + "'                                        " + ComNum.VBLF;
            SQL += " AND A.YYMM < '" + TID.YYMM + "'                                        " + ComNum.VBLF;
            SQL += " AND A.DEPTCODE1 = '" + TID.DeptCode1 + "'                              " + ComNum.VBLF;
            SQL += " AND A.IPDOPD = 'O'                                                     " + ComNum.VBLF;
            SQL += " AND B.SUNEXT NOT IN('IA213','IA221','IA231')                           " + ComNum.VBLF;
            SQL += " GROUP BY A.WRTNO                                                       " + ComNum.VBLF;
            SQL += " ORDER BY A.WRTNO DESC                                                  " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            SS9.ActiveSheet.Rows.Count = 0;

            if (dt.Rows.Count > 0)
            {
                SQL = " SELECT SPECCODE, SPEC" + ComNum.VBLF;
                SQL += " FROM KOSMOS_PMPA.MIR_INSSPEC " + ComNum.VBLF;
                SQL += " WHERE 1=1  " + ComNum.VBLF;
                SQL += " AND WRTNO = '" + dt.Rows[0]["WRTNO"].ToString().Trim() + "' " + ComNum.VBLF;

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (dt1.Rows.Count > 0)
                {
                    SS9.ActiveSheet.Rows.Count = dt1.Rows.Count;

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        SS9_Sheet1.Cells[i, 0].Text = dt1.Rows[i]["SPECCODE"].ToString().Trim();
                        SS9_Sheet1.Cells[i, 1].Text = dt1.Rows[i]["SPEC"].ToString().Trim();
                    }
                }

                dt1.Dispose();
                dt1 = null;
            }

            dt.Dispose();
            dt = null;
        }

        private void READ_ORAN_SLIP()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            DataTable dt3 = null;

            SQL = " SELECT SUNEXT, TO_CHAR(FRDATE,'YYYY-MM-DD') FRDATE FROM KOSMOS_PMPA.MIR_INSDTL  " + ComNum.VBLF;
            SQL += " WHERE 1=1                                                                      " + ComNum.VBLF;
            SQL += " AND WRTNO = '" + TID.WRTNO + "'                                                " + ComNum.VBLF;
            SQL += " AND SUNEXT = 'EB455001'                                                        " + ComNum.VBLF;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ss10.ActiveSheet.Rows.Count = 0;

            if (dt.Rows.Count > 0)
            {
                SQL = " SELECT TO_CHAR(OPDATE,'YYYY-MM-DD') OPDATE, WRTNO                                           " + ComNum.VBLF;
                SQL += " FROM KOSMOS_PMPA.ORAN_MASTER                                                               " + ComNum.VBLF;
                SQL += " WHERE PANO = '" + TID.Pano + "'                                                            " + ComNum.VBLF;
                SQL += " AND OPDATE < TO_DATE('" + dt.Rows[0]["FRDATE"].ToString().Trim() + "','YYYY-MM-DD')        " + ComNum.VBLF;
                SQL += " AND OPDATE > TO_DATE('" + dt.Rows[0]["FRDATE"].ToString().Trim() + "','YYYY-MM-DD')-1095   " + ComNum.VBLF;
                SQL += " ORDER BY OpDate DESC,WRTNO                                                                 " + ComNum.VBLF;


                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (dt1.Rows.Count > 0)
                {
                    //SS9.ActiveSheet.Rows.Count = dt1.Rows.Count;

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        SQL = " SELECT                                                              " + ComNum.VBLF;
                        SQL += " BuCode,TO_CHAR(OpDate,'YYYY-MM-DD') OpDate,DeptCode,IpdOpd,        " + ComNum.VBLF;
                        SQL += " CodeGbn,GuBun,JepCode,SuCode,SuBun,GbSelf,GbSunap,GbSusul,GbNgt,   " + ComNum.VBLF;
                        SQL += " GbSunap,OpRoom,SUM(Qty) Qty                                        " + ComNum.VBLF;
                        SQL += " FROM KOSMOS_PMPA.ORAN_SLIP                                         " + ComNum.VBLF;
                        SQL += " WHERE 1=1                                                          " + ComNum.VBLF;
                        SQL += " AND WRTNO='" + dt1.Rows[i]["WRTNO"].ToString().Trim() + "'         " + ComNum.VBLF;
                        SQL += " AND SuBun IN ('34')                                                " + ComNum.VBLF;
                        SQL += " GROUP BY BuCode,OpDate,DeptCode,IpdOpd,CodeGbn,GuBun,JepCode,      " + ComNum.VBLF;
                        SQL += " SuCode,SuBun,GbSelf,GbSunap,GbSusul,GbNgt,GbSunap,OpRoom           " + ComNum.VBLF;
                        SQL += " ORDER BY BuCode,Gubun,JepCode                                      " + ComNum.VBLF;

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);


                        if (dt2.Rows.Count > 0)
                        {
                            ss10.ActiveSheet.Rows.Count += dt2.Rows.Count;

                            for(int j = 0; j < dt2.Rows.Count; j++)
                            {
                                ss10_Sheet1.Cells[j, 0].Text = dt2.Rows[j]["SUCODE"].ToString().Trim();
                                ss10_Sheet1.Cells[j, 1].Text = dt2.Rows[j]["OPDATE"].ToString().Trim();

                                SQL = " SELECT SUNEXT FROM KOSMOS_PMPA.MIR_INSDTL                                                       " + ComNum.VBLF;
                                SQL += " WHERE 1=1                                                                                      " + ComNum.VBLF;
                                SQL += " AND WRTNO IN (                                                                               " + ComNum.VBLF;
                                SQL += "    SELECT WRTNO FROM KOSMOS_PMPA.MIR_INSID                                                     " + ComNum.VBLF;
                                SQL += "    WHERE PANO = '" + TID.Pano + "'                                                             " + ComNum.VBLF;
                                SQL += "    AND JINDATE1 >= TO_DATE('" + dt2.Rows[j]["OPDATE"].ToString().Trim() + "','YYYY-MM-DD') - 7 " + ComNum.VBLF;
                                SQL += "    AND WRTNO < '" + TID.WRTNO + "'                                                             " + ComNum.VBLF;
                                SQL += " )                                                                                              " + ComNum.VBLF;
                                SQL += " AND SUNEXT = 'EB455001'                                                                        " + ComNum.VBLF;

                                SqlErr = clsDB.GetDataTable(ref dt3, SQL, clsDB.DbCon);

                                if (dt3.Rows.Count > 0)
                                {
                                    ss10_Sheet1.Cells[j, 2].BackColor = Color.FromArgb(255,0,0);
                                }
                            }
                        }
                        
                        dt2.Dispose();
                        dt2 = null;
                    }
                }

                dt1.Dispose();
                dt1 = null;
            }

            dt.Dispose();
            dt = null;
        }

        private void READ_ITRANSFOR_NEW()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strFDate = "";
            string strTDate = "";
            string strBDATE = "";
            string strWARDCODE = "";

            int i = 0;
            int nRow = 0;

            strFDate = VB.Left(TID.IODate, 8);
            strTDate = VB.Right(TID.IODate, TID.IODate.Length - 8);

            strBDATE = "";
            strWARDCODE = "";

            if (strTDate == "")
            {
                strTDate = TID.OutDate.Replace("-", "");
            }

            SQL = "   SELECT  /*+INDEX(KOSMOS_PMPA.IPD_NEW_SLIP INDEX_IPDNEWSL5)*/   TO_CHAR(BDATE ,'YYYY-MM-DD') BDATE, WARDCODE  ";
            SQL += "    FROM KOSMOS_PMPA.IPD_NEW_SLIP  ";
            SQL += "   WHERE PANO   ='" + TID.Pano + "' ";
            SQL += "     AND SUNEXT = '" + VB.Left(GstrSpecTit, 8).Trim() + "' ";
            SQL += "     AND BDATE >=TO_DATE('" + strFDate + "','YYYYMMDD') ";
            SQL += "     AND BDATE <=TO_DATE('" + strTDate + "','YYYYMMDD') "; 
            //2020-10-30 이민주 선생님 요청
            SQL += "     AND WARDCODE IN ('33', '35') ";
            SQL += "  GROUP BY  BDATE, WARDCODE";
            SQL += "  Having Sum(Qty * Nal) <> 0";
            SQL += "  ORDER BY BDATE ASC  ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ss4.ActiveSheet.Rows.Count = 0;

            strBDATE = "";

            for (i = 0; i < dt.Rows.Count; i++)
            {
                //if (strBDATE == "")
                //{
                //    nRow = nRow + 1;
                //    if (ss4.ActiveSheet.Rows.Count < nRow)
                //    {
                //        ss4.ActiveSheet.Rows.Count = nRow;
                //    }

                //    ss4.ActiveSheet.Cells[nRow - 1, 0].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");
                //}
                //else
                //{
                //    if (dt.Rows[i]["BDATE"].ToString().Trim() != Convert.ToDateTime(strBDATE).AddDays(1).ToShortDateString())
                //    {
                //        nRow = nRow + 1;
                //        if (ss4.ActiveSheet.Rows.Count < nRow)
                //        {
                //            ss4.ActiveSheet.Rows.Count = nRow;
                //        }

                //        ss4.ActiveSheet.Cells[nRow - 1, 0].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");
                //    }
                //}

                //strBDATE = dt.Rows[i]["BDATE"].ToString().Trim();
                //ss4.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", ""); 

                if(strBDATE == "" && strWARDCODE == "")
                {
                    nRow = nRow + 1;

                    if (ss4.ActiveSheet.Rows.Count < nRow)
                    {
                        ss4.ActiveSheet.Rows.Count = nRow;
                    }

                    ss4.ActiveSheet.Cells[nRow - 1, 0].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");
                    ss4.ActiveSheet.Cells[nRow - 1, 2].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();

                    if(dt.Rows[i]["WARDCODE"].ToString().Trim() == "33")
                    {
                        ss4.ActiveSheet.Rows[nRow - 1].BackColor = Color.FromArgb(255, 216, 216);
                    }
                    else
                    {
                        ss4.ActiveSheet.Rows[nRow - 1].BackColor = Color.FromArgb(206, 251, 201);
                    }
                    
                }
                else if (dt.Rows[i]["BDATE"].ToString().Trim() != Convert.ToDateTime(strBDATE).AddDays(1).ToShortDateString() || dt.Rows[i]["WARDCODE"].ToString().Trim() != strWARDCODE)
                {
                    nRow = nRow + 1;

                    if (ss4.ActiveSheet.Rows.Count < nRow)
                    {
                        ss4.ActiveSheet.Rows.Count = nRow;
                    }

                    ss4.ActiveSheet.Cells[nRow - 1, 0].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");
                    ss4.ActiveSheet.Cells[nRow - 1, 2].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();

                    if (dt.Rows[i]["WARDCODE"].ToString().Trim() == "33")
                    {
                        ss4.ActiveSheet.Rows[nRow - 1].BackColor = Color.FromArgb(255, 216, 216);
                    }
                    else
                    {
                        ss4.ActiveSheet.Rows[nRow - 1].BackColor = Color.FromArgb(206, 251, 201);
                    }
                }

                ss4.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["BDate"].ToString().Trim().Replace("-", "");
                strBDATE = dt.Rows[i]["BDATE"].ToString().Trim();
                strWARDCODE = dt.Rows[i]["WARDCODE"].ToString().Trim();

            }

            dt.Dispose();
            dt = null;
        }

        private void REAT_AB901_DAY()
        {
            //2019-07-09 김해수 1인실 코드 입원 기간 읽어오기
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strFDate = "";
            string strTDate = "";

            int i = 0;

            strFDate = VB.Left(TID.IODate, 8);
            strTDate = VB.Right(TID.IODate, TID.IODate.Length - 8);


            if (strTDate == "")
            {
                strTDate = TID.OutDate;
            }

            SQL = "SELECT SUNEXT, TO_CHAR(MIN(BDATE),'YYYYMMDD') MINDATE, TO_CHAR(MAX(BDATE),'YYYYMMDD') MAXDATE" + ComNum.VBLF;
            SQL += "FROM (" + ComNum.VBLF;
            SQL += "        SELECT SUNEXT, BDATE" + ComNum.VBLF;
            SQL += "        FROM KOSMOS_PMPA.IPD_NEW_SLIP" + ComNum.VBLF;
            SQL += "        WHERE PANO = '" + TID.Pano + "'" + ComNum.VBLF;
            SQL += "        AND SUNEXT = 'AB901'" + ComNum.VBLF;
            SQL += "        AND BDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')" + ComNum.VBLF;
            SQL += "        AND BDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')" + ComNum.VBLF;
            SQL += "        AND TRSNO = '" + TID.TRSNO + "'" + ComNum.VBLF;
            SQL += "        ORDER BY SUNEXT, BDATE" + ComNum.VBLF;
            SQL += ")" + ComNum.VBLF;
            SQL += "GROUP BY SUNEXT, BDATE - ROWNUM" + ComNum.VBLF;
            SQL += "ORDER BY SUNEXT, MAX(BDATE)" + ComNum.VBLF;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            ss8.ActiveSheet.Rows.Count = 0;
            
            for (i = 0; i < dt.Rows.Count; i++)
            {
                if (ss8.ActiveSheet.Rows.Count <= i)
                {
                    ss8.ActiveSheet.Rows.Count = i + 1;
                }

                ss8.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["MINDATE"].ToString().Trim();
                ss8.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["MAXDATE"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
        }

        public long DraftWrtno
        {
            get { return this.GNSpecWRTNO; }
        }
        
        public int Read_MIR_SPEC( long argWrtno, long argWrtnoS, string argBi)
        {
            string SQL = "";
            DataTable dt = null;
            int retVal = 0;

            // int nVal = 0;
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            //'JJY ( 등록않된건 0으로 SET) '2005 - 01 - 01
            SQL = "";
            if (argBi == "52")
            {
                SQL += ComNum.VBLF + " SELECT COUNT(*) CNT FROM KOSMOS_PMPA.MIR_TASPEC ";
            }
            else
            {
                SQL += ComNum.VBLF + " SELECT COUNT(*) CNT FROM KOSMOS_PMPA.MIR_INSSPEC ";
            }
            SQL += ComNum.VBLF + "  WHERE WRTNO = '" + argWrtno + "' ";
            SQL += ComNum.VBLF + "    AND WRTNOS = '" + argWrtnoS + "'";
            SQL += ComNum.VBLF + "    AND GUBUN='2' ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return retVal;
                }

                if (dt.Rows.Count > 0)
                {
                    retVal = Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim());
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return retVal;
            }

            return retVal;

        }

    }
}
