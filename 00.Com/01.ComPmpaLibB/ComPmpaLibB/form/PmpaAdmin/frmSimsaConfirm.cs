using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComPmpaLibB;
using ComLibB;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : PmpaMir
    /// File Name       : frmSimsaConfirm.cs
    /// Description     : 심사완료 등록 및 취소
    /// Author          : 김민철
    /// Create Date     : 2018-02-20
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\IPD\frm심사완료_New.frm (Frm심사완료_New) >> frmSimsaConfirm.cs 폼이름 재정의" />
    public partial class frmSimsaConfirm : Form
    {
        #region 클래스 선언 및 etc....
        ComFunc CF = new ComFunc();
        clsPmpaFunc cPF = new clsPmpaFunc();
        clsJSimSQL cJSQL = new clsJSimSQL();
        clsIument cIuM = new clsIument();
        clsPmpaPb cPb = new clsPmpaPb();
        clsIpdAcct cIA = new clsIpdAcct();
        clsIuSent cISent = new clsIuSent();
        clsIuSentChk cISentChk = new clsIuSentChk();
        clsBasAcct cBAcct = new clsBasAcct();
        clsIpdArc cIARC = new clsIpdArc();
        clsJSimFunc cJSF = new clsJSimFunc();

        DRG Drg = new DRG();

        clsPmpaType.cBas_Add_Arg cBArg = null;

        long FnTRSNO = 0;
        #endregion

        #region //MainFormMessage
        //public MainFormMessage mCallForm = null;

        //public void MsgActivedForm(Form frm)
        //{
        //}
        //public void MsgUnloadForm(Form frm)
        //{
        //}
        //public void MsgFormClear()
        //{
        //}
        //public void MsgSendPara(string strPara)
        //{
        //}
        #endregion

        public frmSimsaConfirm()
        {
            InitializeComponent();
            setCtrlData();
            setEvent();
        }

        public frmSimsaConfirm(long nTRSNO)
        {
            InitializeComponent();
            setCtrlData();
            setEvent();
            FnTRSNO = nTRSNO;
        }

        void setCtrlData()
        {
            setCombo();
        }

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);

            //Mouse Wheel 이벤트
            this.cboCan.MouseWheel                  += new MouseEventHandler(eCboWheel);
            this.cboEtc.MouseWheel                  += new MouseEventHandler(eCboWheel);
            this.cboFCode.MouseWheel                += new MouseEventHandler(eCboWheel);
            this.cboHC.MouseWheel                   += new MouseEventHandler(eCboWheel);
            this.cboJinDtl.MouseWheel               += new MouseEventHandler(eCboWheel);
            this.cboKTAS.MouseWheel                 += new MouseEventHandler(eCboWheel);
            this.cboPD.MouseWheel                   += new MouseEventHandler(eCboWheel);
            this.cboVDetail.MouseWheel              += new MouseEventHandler(eCboWheel);

            //버튼 클릭 이벤트
            this.btnExit.Click                      += new EventHandler(eBtnClick);
            this.btnSave_VCode.Click                += new EventHandler(eBtnClick);         //중증환자등록
            this.btnSave_FCode.Click                += new EventHandler(eBtnClick);         //특정기호등록
            this.btnSave_PD.Click                   += new EventHandler(eBtnClick);         //소아면제등록
            this.btnSave_HC.Click                   += new EventHandler(eBtnClick);         //희귀.차상 등록
            this.btnSave_VDetail.Click              += new EventHandler(eBtnClick);         //희귀V등록
            this.btnSave_Etc.Click                  += new EventHandler(eBtnClick);         //상해외인등록
            this.btnSave_JinDtl.Click               += new EventHandler(eBtnClick);         //자격정보(노인틀니)
            this.btnSave_KTAS.Click                 += new EventHandler(eBtnClick);         //응급중증도저장
            this.btnSave_AuCode.Click               += new EventHandler(eBtnClick);         //AU코드 발생
            #region 간호간병료 버튼 삭제
            //this.btnSave_Nurse.Click    += new EventHandler(eBtnClick);         //간호간병료 발생
            //this.btnSave_Nurse2.Click   += new EventHandler(eBtnClick);         //간호간병료(내소정) 발생 
            #endregion
            this.btnSave_DRG.Click                  += new EventHandler(eBtnClick);         //DRG 등록
            this.btnSave_DrgCan.Click               += new EventHandler(eBtnClick);         //DRG 해제
            this.btnSearch_Chk.Click                += new EventHandler(eBtnClick);         //자료체크
            this.btnSearch_Chk2.Click               += new EventHandler(eBtnClick);         //퇴원약관리료 발생
            this.btnSearch_BiView.Click             += new EventHandler(eBtnClick);         //자격조회
            this.btnCancel.Click                    += new EventHandler(eBtnClick);         //심사완료 취소
            this.btnSave_OK.Click                   += new EventHandler(eBtnClick);         //심사완료

            //ComboBox 이벤트
            this.cboCan.SelectedIndexChanged        += new EventHandler(eCboSelChanged);
            this.cboEtc.SelectedIndexChanged        += new EventHandler(eCboSelChanged);
            this.cboFCode.SelectedIndexChanged      += new EventHandler(eCboSelChanged);
            this.cboHC.SelectedIndexChanged         += new EventHandler(eCboSelChanged);
            this.cboJinDtl.SelectedIndexChanged     += new EventHandler(eCboSelChanged);
            this.cboKTAS.SelectedIndexChanged       += new EventHandler(eCboSelChanged);
            this.cboPD.SelectedIndexChanged         += new EventHandler(eCboSelChanged);
            this.cboVDetail.SelectedIndexChanged    += new EventHandler(eCboSelChanged);

            //CheckBox 이벤트
            this.chkTax.CheckedChanged      += new EventHandler(eChkChecked);
            this.chkJSimOk.CheckedChanged   += new EventHandler(eChkChecked);
        }

        void setCombo()
        {
            CF.Combo_BCode_SET(clsDB.DbCon, cboCan, "BAS_중증암환자", true, 1, "");
            cboCan.Items.Remove("EV00.차상위+희귀V");
            cboCan.Items.Remove("F003.의약분업 약값 30%");
            cboCan.Items.Remove("V000.결핵치료");
            cboCan.Items.Remove("V010.잠복결핵치료-V010");
            cboCan.Items.Remove("V206.결핵-V206");
            cboCan.Items.Remove("V231.결핵-V231");
            cboCan.Items.Remove("V246.결핵-V246");

            CF.Combo_BCode_SET(clsDB.DbCon, cboPD, "BAS_소아면제", true, 1, "");

            #region 등록희귀 난치성, 차상위 계층
            cboHC.Items.Clear();
            cboHC.Items.Add(" ");
            cboHC.Items.Add("V.등록 희귀난치성질환");
            cboHC.Items.Add("H.희귀.난치성질환");
            cboHC.Items.Add("C.차상위계층");
            cboHC.Items.Add("E.차상위-만성질환자");
            cboHC.Items.Add("F.차상위-장애인만성질환자");
            cboHC.Items.Add("1.차상위E+희귀V");
            cboHC.Items.Add("2.차상위F+희귀V");
            #endregion

            #region 본인부담 지원
            cboFCode.Items.Clear();
            cboFCode.Items.Add(" ");
            cboFCode.Items.Add("F008.입원명령결핵지원대상");
            cboFCode.Items.Add("F010.잠복결핵치료지원대상");
            cboFCode.Items.Add("F012.가다실예방접종대상");
            cboFCode.Items.Add("F013.제왕절개");
            cboFCode.Items.Add("F014.장기입원자경감제외");
            cboFCode.Items.Add("MT04.신종코로나의료지원");
            #endregion

            CF.Combo_BCode_SET(clsDB.DbCon, cboVDetail, "희귀난치V_상세코드", true, 1, "");

            #region 자격정보 (노인틀니)
            cboJinDtl.Items.Clear();
            cboJinDtl.Items.Add(" ");
            cboJinDtl.Items.Add("01.노인틀니");
            #endregion

            #region 지병, 상해외인 등록
            cboEtc.Items.Clear();
            cboEtc.Items.Add(" ");
            cboEtc.Items.Add("C.치과진료");
            cboEtc.Items.Add("D.자연분만(면제)");
            cboEtc.Items.Add("E.의료급여 NP단일");
            cboEtc.Items.Add("F.중증 V등록희귀");
            cboEtc.Items.Add("K.자보 산재");
            cboEtc.Items.Add("I.H희귀난치지원");
            cboEtc.Items.Add("Q.잠복결핵지원");
            #endregion

            #region KTAS 레벨
            cboKTAS.Items.Clear();
            cboKTAS.Items.Add("0.없음");
            cboKTAS.Items.Add("1.1단계");
            cboKTAS.Items.Add("2.2단계");
            cboKTAS.Items.Add("3.3단계");
            cboKTAS.Items.Add("4.4단계");
            cboKTAS.Items.Add("5.5단계");
            #endregion
        }

        void eChkChecked(object sender, EventArgs e)
        {
            if (sender == this.chkTax)
            {
                eSave_Tax(clsDB.DbCon, chkTax);     //부가세대상
            }
            else if (sender == this.chkJSimOk)
            {
                eSave_JSimOk(clsDB.DbCon, chkJSimOk);   //청구내역완성
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strChk = string.Empty;

            if (FnTRSNO == 0)
            {
                this.Close();
                return;
            }

            if (clsType.User.BuseCode == "078201")
            {
                strChk = "OK";
            }

            Screen_Clear();

            Check_Screen_DisPlay(clsDB.DbCon);

            chkJSimOk.Checked = clsPmpaType.TIT.JSIM_OK == "1" ? true : false;
            chkTax.Checked = clsPmpaType.TIT.GbTax == "1" ? true : false;

            //재원,가퇴원
            //if (clsPmpaType.TIT.TGbSts == "0") { return; }
            //if (clsPmpaType.TIT.OutDate == "") { return; }

            //심사완료후 중증코드 입력금지 기능 추가
            if (string.Compare(clsPmpaType.TIT.TGbSts, "5") >= 0)
            {
                #region 심사완료시 Control Set
                btnSave_VCode.Enabled = false;      //중증(암)환자 등록
                btnSave_FCode.Enabled = false;      //특정기호 등록
                btnSave_PD.Enabled = false;         //소아면제 등록
                btnSave_HC.Enabled = false;         //희귀난치성, 차상위계층
                btnSave_VDetail.Enabled = false;    //희귀난치V - 특정기호
                btnSave_Etc.Enabled = false;        //지병 - 상해외인
                btnSave_JinDtl.Enabled = false;     //자격정보(노인틀니)
                btnSave_KTAS.Enabled = false;       //응급실 중증도

                btnSave_DrgCan.Enabled = false;     //DRG해제
                btnSave_AuCode.Enabled = false;     //AU코드 발생
                #region 간호간병료 버튼 비활성화
                //btnSave_Nurse.Enabled = false;      //간호간병료
                //btnSave_Nurse2.Enabled = false;     //간호간병료(내소정) 
                #endregion

                btnSave_OK.Enabled = false;         //심사완료
                btnSave_DRG.Enabled = false;        //DRG 코드부여
                btnCancel.Enabled = true;           //심사완료 취소

                ss1.ActiveSheet.Cells[10, 1].Text = clsPmpaType.TIT.DrgCode == "" ? "" : clsPmpaType.TIT.DrgCode + "(확정)";
                #endregion
            }
            else if (string.Compare(clsPmpaType.TIT.TGbSts, "1") >= 0 && string.Compare(clsPmpaType.TIT.TGbSts, "5") < 0 && clsPmpaType.TIT.AmSet4 == "3")  //정상애기
            {
                #region 정상애기
                btnSave_OK.Enabled = true;          //심사완료
                btnSave_DRG.Enabled = true;         //DRG 코드부여
                btnSave_DrgCan.Enabled = true;      //DRG해제
                btnCancel.Enabled = false;          //심사완료 취소
                #endregion
            }
            else if (string.Compare(clsPmpaType.TIT.TGbSts, "3") >= 0 && string.Compare(clsPmpaType.TIT.TGbSts, "5") < 0)
            {
                #region 대조리스트 인쇄 이후 심사완료 전
                btnSave_OK.Enabled = true;          //심사완료
                btnSave_DRG.Enabled = true;         //DRG 코드부여
                btnSave_DrgCan.Enabled = true;      //DRG해제
                btnCancel.Enabled = false;          //심사완료 취소
                #endregion
            }
            else if (string.Compare(clsPmpaType.TIT.TGbSts, "1") >= 0 && string.Compare(clsPmpaType.TIT.TGbSts, "5") < 0 && clsPmpaType.TIT.GbIpd == "9")
            {
                #region 지병은 대조리스트 이전이라도 가능
                btnSave_OK.Enabled = true;          //심사완료
                btnSave_DRG.Enabled = true;         //DRG 코드부여
                btnSave_DrgCan.Enabled = true;      //DRG해제
                btnCancel.Enabled = false;          //심사완료 취소
                #endregion
            }
            else
            {
                #region 퇴원등록 이전
                btnSave_OK.Enabled = false;         //심사완료
                btnCancel.Enabled = false;
                btnSave_DRG.Enabled = true;         //DRG 코드부여
                btnSave_DrgCan.Enabled = true;      //DRG해제
                #endregion
            }

            //환자변경시에는 심사작업 불가
            if (clsPublic.GstrRetValue == "환자변경")
            {
                btnSave_OK.Enabled = false;
                btnCancel.Enabled = false;
            }
            
        }

        void eCboWheel(object sender, MouseEventArgs e)
        {
            ComboBox CB = sender as ComboBox;

            if (CB.Focused == false)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnSave_VCode)
            {
                eSave_VCode(clsDB.DbCon);               //중증환자등록
            }
            else if (sender == this.btnSave_FCode)
            {
                eSave_FCode(clsDB.DbCon);               //특정기호등록
            }
            else if (sender == this.btnSave_PD)
            {
                eSave_PD(clsDB.DbCon);                  //소아면제등록
            }
            else if (sender == this.btnSave_HC)
            {
                eSave_HC(clsDB.DbCon);                  //희귀.차상 등록
            }
            else if (sender == this.btnSave_VDetail)
            {
                eSave_VDetail(clsDB.DbCon);             //희귀V특정기호
            }
            else if (sender == this.btnSave_Etc)
            {
                eSave_Etc(clsDB.DbCon);                 //상해외인등록
            }
            else if (sender == this.btnSave_JinDtl)
            {
                eSave_JinDtl(clsDB.DbCon);              //자격정보등록
            }
            else if (sender == this.btnSave_KTAS)
            {
                eSave_KTAS(clsDB.DbCon);                //응급중증도 KTAS LEVEL
            }
            else if (sender == this.btnSave_AuCode)     
            {
                eSave_SuCode(clsDB.DbCon, "AU204");     //AU코드 발생
                eSave_SuCode(clsDB.DbCon, "AU302");
                eSave_SuCode(clsDB.DbCon, "AU403");

                ComFunc.MsgBox("작업완료", "확인");
            }
            #region 간호간병료 Event 연결
            //else if (sender == this.btnSave_Nurse)
            //{
            //    eSave_SuCode(clsDB.DbCon, "AV222");     //간호간병료 발생
            //    ComFunc.MsgBox("작업완료", "확인");
            //}
            //else if (sender == this.btnSave_Nurse2)
            //{
            //    eSave_SuCode(clsDB.DbCon, "AV2221");    //간호간병료(내소정)
            //    ComFunc.MsgBox("작업완료", "확인");
            //} 
            #endregion
            else if (sender == this.btnSave_DrgCan)
            {
                eSave_DrgCan(clsDB.DbCon);              //DRG 해제
            }
            else if (sender == this.btnSearch_Chk2)
            {
                eSearch_Chk(clsDB.DbCon, "1");           //자료체크
            }
            else if (sender == this.btnSearch_Chk)
            {
                eSearch_Chk(clsDB.DbCon, "2");           //약품관리료 발생
            }
            else if (sender == this.btnSearch_BiView)
            {
                eSearch_Nhic(clsDB.DbCon);              //자격조회
            }
            else if (sender == this.btnCancel)
            {
                eSave_NO(clsDB.DbCon);                  //심사완료 취소
            }
            else if (sender == this.btnSave_OK)
            {
                eSave_OK(clsDB.DbCon);                  //심사완료
            }
            else if (sender == this.btnSave_DRG)
            {
                #region DRG 세팅
                if (clsPmpaType.TIT.Pano == "" || clsPmpaType.TIT.GbDRG != "D")
                {
                    return;
                }

                clsPublic.GstrPANO = clsPmpaType.TIT.Pano;      //DRG 세팅

                clsPublic.GstrHelpCode = "";
                clsPublic.GstrHelpCode += clsPmpaType.TIT.Bi + "@@";
                clsPublic.GstrHelpCode += clsPublic.GstrSysDate + "@@";
                clsPublic.GstrHelpCode += clsPmpaType.TIT.Trsno.ToString() + "@@";
                clsPublic.GstrHelpCode += clsPmpaType.TIT.Ipdno.ToString() + "@@";
                clsPublic.GstrHelpCode += clsPmpaType.TIT.DrgCode + "@@";

                frmPmpaDrgMain07 frm = new frmPmpaDrgMain07();
                frm.ShowDialog(); 
                #endregion
            }
        }

        void eSearch_Chk(PsmhDb pDbCon, string ArgGbn)
        {
            int nCNT1 = 0, nCNT2 = 0;
            string strBi = clsPmpaType.TIT.Bi;

            Cursor.Current = Cursors.WaitCursor;

            if (clsPmpaType.TIT.OutDate == "")
            {
                clsPmpaType.TIT.OutDate = clsPublic.GstrSysDate;
            }

            clsPmpaType.IA.Date = clsPublic.GstrSysDate;
            clsPmpaType.IA.Bi1 = Convert.ToInt16(clsPmpaType.TIT.Bi.Substring(0, 1));

            if (ArgGbn == "1")
            {
                ss1.ActiveSheet.Cells[12, 0].Text = "";
                ss1.ActiveSheet.Cells[12, 1].Text = "";
                ss1.ActiveSheet.Cells[13, 0].Text = "";
                ss1.ActiveSheet.Cells[13, 1].Text = "";
            }
            else
            {
                nCNT1 = Convert.ToInt16(VB.Val(ss1.ActiveSheet.Cells[12, 0].Text));
                nCNT2 = Convert.ToInt16(VB.Val(ss1.ActiveSheet.Cells[12, 1].Text));

                if (nCNT1 == 0 && nCNT2 == 0)
                {
                    ComFunc.MsgBox("해당사항이 없습니다.", "작업불가");
                    return;
                }
            }

            if (ArgGbn == "2")
            {
                if (strBi.Substring(0, 1) == "1" || ((strBi == "21" || strBi == "22") && clsPmpaType.TIT.DeptCode != "NP") || strBi == "52" || strBi == "55" || strBi == "31" || strBi == "33")
                {
                    if (ComFunc.MsgBoxQ("마지막 퇴원일 경우만 발생할것. 작업하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    ComFunc.MsgBox("건강보험,의료급여(정신과제외) 만 작업을 할수있습니다.",  "확인");
                }
            }

            clsDB.setBeginTran(pDbCon);
            
            if (cJSQL.DisCharge_Drug_Manager_Fee(pDbCon, ArgGbn, ss1, "1") == false)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox("퇴원의약품관리료 수가 발생에러!", "작업불가");
                Cursor.Current = Cursors.Default;
                return;
            }

            clsDB.setCommitTran(pDbCon);

            Cursor.Current = Cursors.Default;

            btnSearch_Chk.Enabled = true;
        }
        
        void eSave_DrgCan(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

          
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (clsPmpaType.TIT.GbDRG == "D")
                {
                    if (ComFunc.MsgBoxQ("현재 저장된 DRG 정보는 모두 삭제됩니다. 그래도 진행하시겠습니까?", "DRG 삭제", MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        clsDB.setBeginTran(pDbCon);

                        SQL = "";
                        SQL += "UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER     ";
                        SQL += "   SET GBDRG ='',                               ";
                        SQL += "       DRGCODE = ''                             ";
                        SQL += " WHERE IPDNO = " + clsPmpaType.TIT.Ipdno + "    ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL += "UPDATE " + ComNum.DB_PMPA + "IPD_TRANS  ";
                        SQL += "   SET GBDRG ='',                                   ";
                        SQL += "       DRGCODE = '',                                ";
                        SQL += "       DRGADC1 = '',                                ";
                        SQL += "       DRGADC2 = '',                                ";
                        SQL += "       DRGADC3 = '',                                ";
                        SQL += "       DRGADC4 = '',                                ";
                        SQL += "       DRGADC5 = ''                                 ";
                        SQL += " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + "        ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                else
                {
                    clsDB.setBeginTran(pDbCon);

                    SQL = "";
                    SQL += "UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER     ";
                    SQL += "   SET GBDRG ='D'                               ";
                    SQL += " WHERE IPDNO = " + clsPmpaType.TIT.Ipdno + "    ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "";
                    SQL += "UPDATE " + ComNum.DB_PMPA + "IPD_TRANS  ";
                    SQL += "   SET GBDRG ='D'                                   ";
                    SQL += " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + "        ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                
                clsDB.setCommitTran(pDbCon);

                ss1.ActiveSheet.Cells[10, 1].Text = "";

                cIuM.Read_Ipd_Mst_Trans(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno, "");

                Cursor.Current = Cursors.Default;

                ComFunc.MsgBox("작업완료.", "확인");
                
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            
        }

        void eSearch_Nhic(PsmhDb pDbCon)
        {
            string strSName = clsPmpaType.IMST.Sname;

            if (strSName.Contains("A") || strSName.Contains("B") || strSName.Contains("C") || strSName.Contains("D"))
            {
                if (ComFunc.MsgBoxQ("성명에 A,B,C,D가 포함되어 있습니다. 성명수정 후 자격조회 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    strSName = VB.InputBox("성명을 다시 확인하십시오.", "이름확인", strSName);
                }
            }

            frmPmpaCheckNhic frm = new frmPmpaCheckNhic(clsPmpaType.TIT.Pano, clsPmpaType.TIT.DeptCode, strSName, clsPmpaType.TIT.Jumin1, clsPmpaType.TIT.Jumin3, clsPublic.GstrSysDate, "");
            frm.ShowDialog();
        }

        void eSave_VCode(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            string strBi = clsPmpaType.TIT.Bi;
            string strOgPd = clsPmpaType.TIT.OgPdBun;
            string strCan = VB.Left(cboCan.Text, 4);
            string strHC = VB.Left(cboHC.Text, 1);

            if ((strBi == "13" || strBi == "12" || strBi == "11") && (strOgPd == "E" || strOgPd == "F"))
            {
                //V268 뇌출혈추가
                if (strCan != "" && strCan != "V191" && strCan != "V192" && strCan != "V193" && strCan != "V268" && strCan != "V275")
                {
                    ComFunc.MsgBox("차상위2종 건강보험 환자는 중증 V191,V192,V193만 가능합니다.. 다시 선택하세요", "확인");
                    return;
                }
            }
            
            //2021-11-03
            if (strCan != "")
            {
                if (chkSlipM(clsPmpaType.TIT.Trsno) == "OK")
                {
                    ComFunc.MsgBox("< 중증/희귀/난치 산정특례대상입니다. 치료재료수가 확인하세요.(결핵,치매는 제외)>", "확인");
                }
            }
            
            

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //중증(암)환자 등록 작업
                clsDB.setBeginTran(pDbCon);

                #region UPDATE Query
                SQL = "";
                SQL += "UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";

                if (strCan != "")
                {
                    if (strCan == "V247" || strCan == "V248" || strCan == "V249" || strCan == "V250")       //중증화상5%
                    {
                        if (strBi.Substring(0, 1) == "1" || strBi == "22")
                        {
                            SQL += " VCODE = '" + strCan + "', ";

                            if (strHC == "C")
                            {
                                SQL += " BONRATE = 0,           ";
                                SQL += " OGPDBUN  = 'C'         ";
                            }
                            else if (strHC == "E")      //차상위2종 만성질환자 및 만18세미만
                            {
                                SQL += " BONRATE = 5,           ";
                                SQL += " OGPDBUN  = 'E'         ";
                            }
                            else if (strHC == "F")      //차상위2종 장애인 만성질환자 및 만18세미만
                            {
                                SQL += " BONRATE = 5,           ";
                                SQL += " OGPDBUN  = 'F'         ";
                            }
                            else
                            {
                                SQL += " BONRATE = 5            ";
                            }
                        }
                        else
                        {
                            SQL += " VCODE = '" + strCan + "' ";
                        }
                    }
                    //V268 뇌출혈추가
                    else if (strCan == "V191" || strCan == "V192" || strCan == "V193" || strCan == "V194" || strCan == "V268" || strCan == "V275")
                    {
                        if (strBi.Substring(0, 1) == "1" || strBi == "22")
                        {
                            SQL += " VCODE = '" + strCan + "', ";

                            if (strHC == "C")
                            {
                                SQL += " BONRATE = 0,           ";
                                SQL += " OGPDBUN  = 'C'         ";
                            }
                            else if (strHC == "E")      //차상위2종 만성질환자 및 만18세미만
                            {
                                SQL += " BONRATE = 5,           ";
                                SQL += " OGPDBUN  = 'E'         ";
                            }
                            else if (strHC == "F")      //차상위2종 장애인 만성질환자 및 만18세미만
                            {
                                SQL += " BONRATE = 5,           ";
                                SQL += " OGPDBUN  = 'F'         ";
                            }
                            else
                            {
                                SQL += " BONRATE = 5            ";
                            }
                        }
                        else
                        {
                            SQL += " VCODE = '" + strCan + "' ";
                        }
                    }
                    else if (strBi.Substring(0, 1) == "1" && (strCan == "V193" || strCan == "V194"))     //건강보험만 5%
                    {
                        if (strBi.Substring(0, 1) == "1" || strBi == "22")
                        {
                            SQL += " VCODE = '" + strCan + "', ";

                            if (strHC == "C")
                            {
                                SQL += " BONRATE = 0,           ";
                                SQL += " OGPDBUN  = 'C'         ";
                            }
                            else if (strHC == "E")      //차상위2종 만성질환자 및 만18세미만
                            {
                                SQL += " BONRATE = 5,           ";
                                SQL += " OGPDBUN  = 'E'         ";
                            }
                            else if (strHC == "F")      //차상위2종 장애인 만성질환자 및 만18세미만
                            {
                                SQL += " BONRATE = 5,           ";
                                SQL += " OGPDBUN  = 'F'         ";
                            }
                            else
                            {
                                SQL += " BONRATE = 5            ";
                            }
                        }
                        else
                        {
                            SQL += " VCODE = '" + strCan + "' ";
                        }
                    }
                    else
                    {
                        if (strBi.Substring(0, 1) == "1" || strBi == "22")
                        {
                            SQL += " VCODE = '" + strCan + "', ";

                            if (strHC == "C")
                            {
                                SQL += " BONRATE = 0,           ";
                                SQL += " OGPDBUN  = 'C'         ";
                            }
                            else if (strHC == "E")      //차상위2종 만성질환자 및 만18세미만
                            {
                                SQL += " BONRATE = 10,           ";
                                SQL += " OGPDBUN  = 'E'          ";
                            }
                            else if (strHC == "F")      //차상위2종 장애인 만성질환자 및 만18세미만
                            {
                                SQL += " BONRATE = 10,           ";
                                SQL += " OGPDBUN  = 'F'          ";
                            }
                            else
                            {
                                SQL += " BONRATE = 10            ";
                            }
                        }
                        else
                        {
                            SQL += " VCODE = '" + strCan + "' ";
                        }
                    }
                }
                else
                {
                    SQL += " VCODE = '',        ";

                    if (strBi.Substring(0, 1) == "1" && (strHC == "E" || strHC == "F" || strHC == "J" || strHC == "K"))
                    {
                        SQL += " BONRATE = 14,                              ";
                        SQL += " OGPDBUN  = '" + strHC + "' ";
                    }
                    else
                    {
                        SQL += " BONRATE = '" + clsPmpaPb.IBON[Convert.ToInt16(strBi)] + "' ";
                    }
                }
                SQL += " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL += "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                #endregion

                clsDB.setCommitTran(pDbCon);

                cIuM.Read_Ipd_Mst_Trans(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno, "");

                Cursor.Current = Cursors.Default;

                string strSName = clsPmpaType.IMST.Sname;

                if (strSName.Contains("A") || strSName.Contains("B") || strSName.Contains("C") || strSName.Contains("D"))
                {
                    if (ComFunc.MsgBoxQ("성명에 A,B,C,D가 포함되어 있습니다. 성명수정 후 자격조회 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        strSName = VB.InputBox("성명을 다시 확인하십시오.", "이름확인", strSName);
                    }
                }

                frmPmpaCheckNhic frm = new frmPmpaCheckNhic(clsPmpaType.TIT.Pano, clsPmpaType.TIT.DeptCode, strSName, clsPmpaType.TIT.Jumin1, clsPmpaType.TIT.Jumin3, clsPublic.GstrSysDate, "");
                frm.ShowDialog();

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            
        }

        void eSave_FCode(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            string strFCode = VB.Left(cboFCode.Text, 4);
            string strCan = cboCan.Text.Trim();
            string strPD = cboPD.Text.Trim();
            string strHC = cboHC.Text.Trim();

            string strBi = clsPmpaType.TIT.Bi;
            string strOgPd = clsPmpaType.TIT.OgPdBun.Trim();
            string strVCode = VB.Left(clsPmpaType.TIT.VCode, 4);

            #region Data_Check
            if (strFCode == "F014")
            {
                if (strCan != "" || strPD != "" || strHC != "" || strOgPd != "")
                {
                    ComFunc.MsgBox("F014 등록시 순수 건강보험만 가능합니다.", "등록불가");
                    return;
                }
            }

            // F014여부 체크
            if (strBi.Substring(0, 1) == "1")
            {
                if (cJSQL.Read_Mir_ILLS(pDbCon, clsPmpaType.TIT.Pano, strBi, clsPmpaType.TIT.InDate, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, clsPmpaType.TIT.FCode) == false)
                {
                    return;
                }
                
            }

            if (strFCode == "F008")
            {
                if (strVCode != "V206" && strVCode != "V231")
                {
                    ComFunc.MsgBox("중증등록(결핵등록)이 되어 있지 않습니다.", "확인");
                    return;
                }

                if (strBi.Substring(0, 1) != "1")
                {
                    ComFunc.MsgBox("입원명령결핵환자 등록은 건강보험만 가능합니다. 다시 선택하세요.", "확인");
                    return;
                }
            }
            else if (strFCode == "F010")
            {
                if (strBi.Substring(0, 1) != "1" && strBi != "22")
                {
                    ComFunc.MsgBox("입원명령결핵환자 등록은 건강보험, 급여 22종만 가능합니다. 다시 선택하세요.", "확인");
                    return;
                }
            }
            #endregion

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //중증(암)환자 등록 작업
                clsDB.setBeginTran(pDbCon);

                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET      ";
                SQL += "  FCODE = '" + strFCode + "'                    ";
                SQL += "  WHERE TRSNO = " + clsPmpaType.TIT.Trsno + "    ";
                SQL += "    AND IPDNO = " + clsPmpaType.TIT.Ipdno + "    ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);

                cIuM.Read_Ipd_Mst_Trans(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno, "");

                Cursor.Current = Cursors.Default;

                ComFunc.MsgBox("특정기호 등록이 처리되었습니다.", "확인");
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            
        }
        
        void eSave_PD(PsmhDb pDbCon)
        {
            

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            double AGEILSU = 0;
            int Age_Chake = 0;

            string strHC = VB.Left(cboHC.Text, 1);
            string strPD = VB.Left(cboPD.Text, 1);
            string strBi = clsPmpaType.TIT.Bi;

            #region Data_Check

            AGEILSU = ComFunc.AgeCalcEx_Zero(clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin3, clsPublic.GstrSysDate);
            Age_Chake = ComFunc.AgeCalcEx(clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin3, clsPublic.GstrSysDate);

            if (strPD == "P")
            {
                if (Age_Chake != 0 )
                {
                    ComFunc.MsgBox("★ 나이별 소아면제코드 확인요망(차상위/의료급여는 재확인)" + '\r' + Age_Chake +"세", "확인");
                }
                else if (Age_Chake == 0 && AGEILSU > 28)
                {
                    ComFunc.MsgBox("★ 나이별 소아면제코드 확인요망(차상위/의료급여는 재확인)" + '\r' + "생후" + AGEILSU + "일", "확인");
                }
            }
            else if (strPD == "S")
            {
                if (Age_Chake >= 6 )
                {
                    ComFunc.MsgBox("★ 나이별 소아면제코드 확인요망(차상위/의료급여는 재확인)" + '\r' + Age_Chake + "세", "확인");
                }
            }
            else if (strPD == "Y")
            {
                if (Age_Chake > 15 || Age_Chake < 6)
                {
                    ComFunc.MsgBox("★ 나이별 소아면제코드 확인요망(차상위/의료급여는 재확인)" + '\r' + Age_Chake + "세", "확인");
                }
            }
            #endregion
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //소아면제
                clsDB.setBeginTran(pDbCon);

                if (strHC == "E" || strHC == "F" || strHC == "J" || strHC == "K")
                {
                    #region strHC == "E" || strHC == "F" || strHC == "J" || strHC == "K"
                    SQL = "";
                    // 차상위2종 해당환자중 자연분만산모,6세미만아동만 해당됨
                    SQL += "UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";
                    if (strPD == "S")                               //소아0%
                    {
                        SQL += " OGPDBUN  = '" + strHC + "', ";
                        SQL += " OGPDBUNdtl  = 'S', ";
                        SQL += " BONRATE= 0 ";
                    }
                    else if (strPD == "P")          //소아면제
                    {
                        SQL += " OGPDBUN  = '" + strHC + "', ";
                        SQL += " OGPDBUNdtl  = 'P', ";
                        SQL += " BONRATE= 0 ";
                    }
                    else if (strPD == "O")          //자연분만 면제
                    {
                        SQL += " OGPDBUN  = '" + strHC + "', ";
                        SQL += " OGPDBUNdtl  = 'O', ";
                        SQL += " BONRATE= 0 ";
                    }
                    else if (string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0 && (strHC == "E" || strHC == "F") && strPD == "Y")
                    {
                        SQL += " OGPDBUN  = '" + strHC + "', ";
                        SQL += " OGPDBUNdtl  = 'Y', ";
                        SQL += " BONRATE= 3 ";
                    }
                    else if ((strHC == "E" || strHC == "F") && strPD == "T")
                    {
                        SQL += " OGPDBUN  = '" + strHC + "', ";
                        SQL += " OGPDBUNdtl  = 'T', ";
                        if (strBi == "22")
                        {
                            SQL += " BONRATE= 5 ";
                        }
                        else
                        {
                            // 고위험임산부 요율 변경
                            if (string.Compare(clsPmpaType.TIT.InDate, "2017-01-01") >= 0)
                            {
                                SQL += " BONRATE= 5 ";
                            }
                            else
                            {
                                SQL = " BONRATE= 10 ";
                            }
                        }
                    }
                    SQL += " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    SQL += "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                    #endregion
                }
                else
                {
                    #region 소아관련 처리
                    SQL = "";
                    SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";
                    SQL += " OGPDBUN  = '" + strPD + "', ";
                    SQL += " OGPDBUNdtl  = '" + strPD + "', ";

                    if (strPD == "S")                   //소아10%
                    {
                        if (string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0)
                        {
                            SQL += " BONRATE= 5 ";      //6세 5%
                        }
                        else
                        {
                            SQL += " BONRATE= 10 ";
                        }
                    }
                    else if (strPD == "Y" && string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0)         //6-15세 5% 
                    {
                        if (strBi == "22")
                        {
                            SQL += " BONRATE= 3 ";
                        }
                        else
                        {
                            SQL += " BONRATE= 5 ";
                        }
                    }
                    else if (strPD == "P")          //소아면제
                    {
                        SQL += " BONRATE= 0 ";
                    }
                    else if (strPD == "O")         //자연분만 면제
                    {
                        SQL += " BONRATE= 0 ";
                    }
                    else if (strPD == "T")
                    {
                        if (strBi == "22")
                        {
                            SQL += " BONRATE= 5 ";
                        }
                        else
                        {
                            SQL += " BONRATE= 10 ";
                        }
                    }
                    else
                    {
                        SQL += " BONRATE= 10 ";
                    }
                    SQL += " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                    SQL += "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                    #endregion
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);

                cIuM.Read_Ipd_Mst_Trans(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno, "");

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            
        }

        private string chkSlipM(long strTrsno)
        {
            
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strRtn = "";
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, BDATE, SUCODE, SUM(QTY * NAL) CNT ";
            SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.IPD_NEW_SLIP ";
            SQL += ComNum.VBLF + "  WHERE SUCODE IN('M6710012',  'M6750029', 'M6720066') ";
            SQL += ComNum.VBLF + "    AND TRSNO = " + clsPmpaType.TIT.Trsno;
            SQL += ComNum.VBLF + "  GROUP BY PANO, BDATE, SUCODE ";
            SQL += ComNum.VBLF + " HAVING SUM(QTY * NAL) > 0 ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return strRtn;
            }
            if (dt.Rows.Count > 0)
            {
                strRtn = "OK";
            }

            dt.Dispose();
            dt = null;

            return strRtn;

        }

        void eSave_HC(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            string strHC = VB.Left(cboHC.Text, 1);
            string strPD = VB.Left(cboPD.Text, 1);
            string strBi = clsPmpaType.TIT.Bi;
            string strVCode = VB.Left(cboCan.Text, 4);

            #region Data_Check
            if (strHC == "H")
            {
                ComFunc.MsgBox("H.지원대상", "확인");

            }

            if (strHC == "V")
            {
                if (string.Compare(strBi, "30") >= 0)
                {
                    ComFunc.MsgBox("V 희귀.난치성 코드는 건강보험/급여 자격만 가능합니다", "확인");
                    return;
                }
            }
            else
            {
                if (strHC.Trim() != "")
                {
                    if (string.Compare(strBi, "21") >= 0)
                    {
                        ComFunc.MsgBox("희귀.난치성, 차상위 건강보험 자격만 가능합니다 (V제외)", "확인");
                        return;
                    }
                }
            }

            if (strBi.Substring(0, 1) != "1" && (strHC == "E" || strHC == "F" || strHC == "J" || strHC == "K"))
            {
                ComFunc.MsgBox("차상위2종은  건강보험11,12,13종 자격만 가능합니다", "확인");
                return;
            }

            //2021-11-03
            switch(strHC)
            {
                case "V":
                case "H":
                case "1":
                case "2":
                    if (chkSlipM(clsPmpaType.TIT.Trsno) == "OK")
                    {
                        ComFunc.MsgBox("< 중증/희귀/난치 산정특례대상입니다. 치료재료수가 확인하세요.(결핵,치매는 제외)>", "확인");
                    }
                    break;
            }



            #endregion

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //'희귀.난치성 차상위계층
                clsDB.setBeginTran(pDbCon);

                #region Make SQL
                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";

                if (strHC == "1")               //2013-02-15 중복자격 추가 E+V
                {
                    SQL += " OGPDBUN  = '1', ";
                    SQL += " BONRATE= 10 ";
                }
                else if (strHC == "2")          //2013-02-15 중복자격 추가 F+V
                {
                    SQL += " OGPDBUN  = '2', ";
                    SQL += " BONRATE= 10 ";
                }
                else if (strHC == "V")          //등록-산정특례-희귀난치성질환자
                {
                    SQL += " OGPDBUN  = 'V', ";
                    if (strBi == "21")
                    {
                        SQL += " BONRATE= 0 ";
                    }
                    else
                    {
                        SQL += " BONRATE= 10 ";
                    }
                }
                else if (strHC == "H")          //희귀.난치성질환자
                {
                    SQL += " OGPDBUN  = 'H' ";
                }
                else if (strHC == "C")           //차상위계층환자
                {
                    #region strHC == "C"
                    if (strVCode == "V247" || strVCode == "V248" || strVCode == "V249" || strVCode == "V250")   //2010-07-01 중증화상
                    {
                        SQL += " OGPDBUN  = 'C', ";
                        SQL += " BONRATE= 0, ";
                        SQL += " VCODE = '" + strVCode + "' ";
                    }
                    else if (strVCode == "V193")
                    {
                        SQL += " OGPDBUN  = 'C', ";
                        SQL += " BONRATE= 0, ";
                        SQL += " VCODE = '" + strVCode + "' ";
                    }
                    else
                    {
                        SQL += " OGPDBUN  = 'C', ";
                        SQL += " BONRATE= 0 ";
                    }
                    #endregion
                }
                else if (strHC == "E")      // 차상위계층환자-만성질환자 2009-03-19
                {
                    #region strHC == "E"
                    if (strVCode == "V247" || strVCode == "V248" || strVCode == "V249" || strVCode == "V250")       //2010=07-01 중증화상
                    { 
                        SQL += " OGPDBUN  = 'E', ";
                        SQL += " BONRATE= 5, ";
                        SQL += " VCODE = '" + strVCode + "' ";
                    }
                    //V268 뇌출혈환자 포함
                    else if (strVCode == "V191" || strVCode == "V192" || strVCode == "V193" || strVCode == "V194" || strVCode == "V268" || strVCode == "V275")
                    {
                        SQL += " OGPDBUN  = 'E', ";
                        SQL += " BONRATE= 5, ";
                        SQL += " VCODE = '" + strVCode + "' ";
                    }
                    else if (clsPmpaType.TIT.DeptCode == "NP")
                    {
                        SQL += " OGPDBUN  = 'E', ";
                        SQL += " BONRATE= 10 ";
                    }
                    //2015-06-29 고위험 임신부 본인부담 10%
                    else if (strPD == "T")
                    {
                        SQL += " OGPDBUN  = 'E', ";
                        //2016-12-30 고위험임산부 요율 변경
                        if (string.Compare(clsPmpaType.TIT.InDate, "2017-01-01") >= 0)
                        {
                            SQL += " BONRATE= 5 ";
                        }
                        else
                        {
                            SQL += " BONRATE= 10 ";
                        }
                    }
                    else if (string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0 &&  strPD == "Y") //2017-10-01 add
                    { 
                        SQL += " OGPDBUN  = 'E', ";
                        SQL += " BONRATE= 3 ";
                    }
                    else
                    {
                        SQL += " OGPDBUN  = 'E', ";
                        SQL += " BONRATE= 14 ";
                    }
                    #endregion
                }
                else if (strHC == "F")          // 차상위계층환자-장애인 만성질환자 2009-03-19
                {
                    #region strHC == "E"
                    if (strVCode == "V247" || strVCode == "V248" || strVCode == "V249" || strVCode == "V250")   //2010==07-01 중증화상
                    {
                        SQL += " OGPDBUN  = 'F', ";
                        SQL += " BONRATE= 5, ";
                        SQL += " VCODE = '" + strVCode + "' ";
                    }
                    //V268 뇌출혈환자 포함
                    else if (strVCode == "V191" || strVCode == "V192" || strVCode == "V193" || strVCode == "V194" || strVCode == "V268" || strVCode == "V275")
                    {
                        SQL += " OGPDBUN  = 'F', ";
                        SQL += " BONRATE= 5, ";
                        SQL += " VCODE = '" + strVCode + "' ";
                    }
                    else if (clsPmpaType.TIT.DeptCode == "NP")
                    {
                        SQL += " OGPDBUN  = 'F', ";
                        SQL += " BONRATE= 10 ";
                    }
                    //2015-06-29 고위험 임신부 본인부담 10%
                    else if (strPD == "T")
                    {
                        SQL += " OGPDBUN  = 'E', ";
                        //2016-12-30 고위험임산부 요율 변경
                        if (string.Compare(clsPmpaType.TIT.InDate, "2017-01-01") >= 0)
                        {
                            SQL += " BONRATE= 5 ";
                        }
                        else
                        {
                            SQL += " BONRATE= 10 ";
                        }
                    }
                    else if (string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0 && strPD == "Y")     //2017-10-01 add
                    {
                        SQL += " OGPDBUN  = 'E', ";
                        SQL += " BONRATE= 3 ";
                    }
                    else
                    {
                        SQL += " OGPDBUN  = 'F', ";
                        SQL += " BONRATE= 14 ";
                    }
                    #endregion
                }
                else if (strHC == "J")          //차상위계층환자-만성질환자-상해외인 2009-03-19
                {
                    SQL += " OGPDBUN  = 'J', ";
                    SQL += " BONRATE= 14 ";
                }
                else if (strHC == "K")          //차상위계층환자-장애인 만성질환자-상해외인 2009-03-19
                {
                    SQL += " OGPDBUN  = 'K', ";
                    SQL += " BONRATE= 14 ";
                }
                else if (strHC == "I")          //희귀.난치성질환자-상해외인
                {
                    SQL += " OGPDBUN  = 'I', ";
                    SQL += " BONRATE= 20 ";
                }
                else
                {
                    #region Else
                    if (clsPmpaType.TIT.GbIpd == "9" && (strHC == "M" || strHC == "N" || strHC == "O" || strHC == "P"))
                    {
                        SQL += " OGPDBUN  = '" + strHC + "', ";
                    }
                    else
                    {
                        SQL += " OGPDBUN  = ' ', ";
                    }

                    if (strBi.Substring(0, 1) == "1")
                    {
                        SQL += " BONRATE= 20 ";
                    }
                    else if (strBi == "21")
                    {
                        SQL += " BONRATE= 0 ";
                    }
                    else if (strBi == "22")
                    { 
                        SQL += " BONRATE= 10 ";
                    }
                    else if (strBi == "31" || strBi == "52")
                    {
                        SQL += " BONRATE= 0 ";
                    }
                    else if (strBi == "33" || strBi == "43" || strBi == "51" || strBi == "55")
                    {
                        SQL += " BONRATE= 100 ";
                    }
                    #endregion
                }

                SQL += " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL += "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                #endregion

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);

                cIuM.Read_Ipd_Mst_Trans(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno, "");

                if (strHC == "V" || strHC == "H" || strHC == "E" || strHC == "F" || strHC == "1" || strHC == "2")
                {
                    grpBox05.Enabled = true;
                }

                ComFunc.MsgBox("저장되었습니다..", "확인");

                Cursor.Current = Cursors.Default;

                string strSName = clsPmpaType.IMST.Sname;

                if (strSName != null && (strSName.Contains("A") || strSName.Contains("B") || strSName.Contains("C") || strSName.Contains("D")))
                {
                    if (ComFunc.MsgBoxQ("성명에 A,B,C,D가 포함되어 있습니다. 성명수정 후 자격조회 하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        strSName = VB.InputBox("성명을 다시 확인하십시오.", "이름확인", strSName);
                    }
                }

                frmPmpaCheckNhic frm = new frmPmpaCheckNhic(clsPmpaType.TIT.Pano, clsPmpaType.TIT.DeptCode, strSName, clsPmpaType.TIT.Jumin1, clsPmpaType.TIT.Jumin3, clsPublic.GstrSysDate, "");
                frm.ShowDialog();
                
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            
        }

        void eSave_VDetail(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            string strHC = VB.Left(cboHC.Text, 1);

            if (strHC != "V" && strHC != "H" && strHC != "E" && strHC != "F" && strHC != "1" && strHC != "2")
            {
                ComFunc.MsgBox("희귀난치 V,H,E,F 자격만 입력할수있습니다.. 다시 선택하세요", "확인");
               // return;
            }

            if (VB.Left(cboVDetail.Text, 4).ToUpper() == "V000" )
            {
                ComFunc.MsgBox("결핵 지원대상", "확인");
                // return;
            }

            if (VB.Left(cboVDetail.Text, 4).ToUpper() == "V010")
            {
                ComFunc.MsgBox("잠복결핵 지원대상", "확인");
                // return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //희귀난치 V 자격 상세코드 등록 작업
                clsDB.setBeginTran(pDbCon);

                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";

                if (cboVDetail.Text.Trim() != "")
                {
                    SQL += " VCODE = '" + VB.Left(cboVDetail.Text, 4).ToUpper() + "'  ";
                }
                else
                {
                    SQL += " VCODE = '' ";
                }
                SQL += " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL += "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);

                cIuM.Read_Ipd_Mst_Trans(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno, "");

                ComFunc.MsgBox("희귀V - 특정기호가 정상 등록 되었습니다.", "확인");

                Cursor.Current = Cursors.Default;
                
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            
        }

        void eSave_Etc(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;            

            if (clsPmpaType.TIT.GbIpd != "9")
            {
                ComFunc.MsgBox("지병자격만 등록가능합니다.", "확인");
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //희귀난치 V 자격 상세코드 등록 작업
                clsDB.setBeginTran(pDbCon);

                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";

                if (cboEtc.Text.Trim() != "")
                {
                    SQL += " OgpdBun2 = '" + VB.Left(cboEtc.Text, 1) + "'  ";
                }
                else
                {
                    SQL += " OgpdBun2 = '' ";
                }
                
                SQL += " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL += "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);

                cIuM.Read_Ipd_Mst_Trans(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno, "");

                ComFunc.MsgBox("지병 상해외인 등록이 처리되었습니다.", "확인");

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        void eSave_JinDtl(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;                      

            if (clsPmpaType.TIT.DeptCode != "DT")
            {
                ComFunc.MsgBox("치과 자격만 입력할수있습니다. 다시 선택하세요.", "확인");
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //희귀난치 V 자격 상세코드 등록 작업
                clsDB.setBeginTran(pDbCon);

                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";

                if (cboJinDtl.Text.Trim() != "")
                {
                    SQL += " JinDtl = '" + VB.Left(cboJinDtl.Text, 2) + "'  ";
                }
                else
                {
                    SQL += " JinDtl = '' ";
                }

                SQL += " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SQL += "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);

                cIuM.Read_Ipd_Mst_Trans(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno, "");

                ComFunc.MsgBox("노인틀니정보가 정상 등록 되었습니다.", "확인");

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        void eSave_KTAS(PsmhDb pDbCon)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            string strHC = VB.Left(cboHC.Text, 1);
            string strPD = VB.Left(cboPD.Text, 1);
            string strBi = clsPmpaType.TIT.Bi;
            string strVCode = VB.Left(cboCan.Text, 4);
            
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //희귀난치 V 자격 상세코드 등록 작업
                clsDB.setBeginTran(pDbCon);

                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER SET ";

                if (cboKTAS.Text.Trim() != "")
                {
                    SQL += " KTASLEVL = '" + VB.Left(cboKTAS.Text, 1) + "'  ";
                }
                else
                {
                    SQL += " KTASLEVL = '' ";
                }

                SQL += " WHERE PANO = '" + clsPmpaType.TIT.Pano + "' ";
                SQL += "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS Set     ";
                SQL += " KTASLEVL = '" + VB.Left(cboKTAS.Text, 1) + "'  ";
                SQL += " WHERE PANO = '" + clsPmpaType.TIT.Pano + "' ";
                SQL += "   AND TRSNO = " + clsPmpaType.TIT.Trsno + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }


                clsDB.setCommitTran(pDbCon);

                cIuM.Read_Ipd_Master(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Ipdno);
                cIuM.Read_Ipd_Mst_Trans(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno, "");

                ComFunc.MsgBox("KTAS 레벨이 정상 등록 되었습니다.", "확인");

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }
        
        void eSave_SuCode(PsmhDb pDbCon, string ArgSuCode)
        {
            if (clsPmpaType.TIT.Pano == "")
            {
                ComFunc.MsgBox("환자 선택 후 작업요망.", "작업불가");
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                ComFunc.ReadSysDate(pDbCon);

                clsDB.setBeginTran(pDbCon);

                if (cPF.Ins_IpdSlip_SuCode(pDbCon, ArgSuCode, clsPublic.GstrSysDate) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(ArgSuCode + " 수가입력 오류!", "작업불가");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.setRollbackTran(pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        void eSave_NO(PsmhDb pDbCon)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            clsPublic.GstrMsgTitle = "심사완료 SET 해제";
            clsPublic.GstrMsgList = "심사완료 Setting을 취소 하시겠습니까?";
            
            if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS        ";
            SQL += "    SET AmSet3 = '0',                          ";
            SQL += "        GbSTS = '3',                           ";
            SQL += "        ACTDATE = '',                          ";
            SQL += "        AMT51 = 0, AMT52 = 0,                  ";
            SQL += "        AMT53 = 0, AMT54 = 0,                  ";
            SQL += "        AMT55 = 0, AMT56 = 0,                  ";
            SQL += "        AMT57 = 0, AMT58 = 0,                  ";
            SQL += "        AMT59 = 0, AMT60 = 0,                  ";
            SQL += "        AMT65 = 0, AMT66 = 0                   ";
            SQL += "  WHERE TRSNO = " + clsPmpaType.TIT.Trsno + "  ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }


            SQL = "";
            SQL += " DELETE " + ComNum.DB_PMPA + "IPD_NEW_CASH              ";
            SQL += "  WHERE TRSNO = " + clsPmpaType.TIT.Trsno + "           ";
            SQL += "    AND BUN  IN ('88','89','91','92','96','98','99')    ";
            SQL += "    AND ACTDATE = TRUNC(SYSDATE)                        ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            //모든 IPD_TRANS가 심사완료가 되었는지 점검함
            SQL = "";
            SQL += " SELECT COUNT(*) CNT ";
            SQL += "   FROM " + ComNum.DB_PMPA + "IPD_TRANS         ";
            SQL += "  WHERE IPDNO = " + clsPmpaType.TIT.Ipdno + "   ";
            SQL += "    AND GBIPD <> 'D'                            "; //삭제가 아닌것            
            SQL += "    AND GBSTS IN ('0','1','2','3','4')          ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            int nCNT = 0;

            if (dt.Rows.Count > 0)
            {
                nCNT = Convert.ToInt16(VB.Val(dt.Rows[0]["CNT"].ToString()));
            }

            dt.Dispose();
            dt = null;

            //IPD_MASTER에 심사완료, 계산서인쇄를 Reset
            if (nCNT > 0)
            {
                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER        ";
                SQL += "    SET GbSTS = '0',                                ";
                SQL += "        SimsaTime = '',                             ";
                SQL += "        PrintTime = ''                              ";
                SQL += "  WHERE IPDNO = " + clsPmpaType.TIT.Ipdno + "       ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

            //2010-11-22 심사완료시 자동발생 약제상한차액코드 BBBBBB 삭제함 -당일건만
            if (cPF.IPD_DRUG_BBBBBB_DELETE(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno) == false)
            {
                clsDB.setRollbackTran(pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            //퇴원걸린것 재원 및 심사완료 취소시
            if (clsPmpaType.TIT.GbDRG == "D")
            {
                if (Drg.IPD_DRG_SLIP_DELETE(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }
            clsDB.setCommitTran(pDbCon);

            cIuM.Read_Ipd_Mst_Trans(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno, "");

            clsDB.setBeginTran(pDbCon);

            if (cJSQL.Simsa_History_SAVE (pDbCon, "", "심사취소", clsPmpaType.TIT.Pano, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, "3", clsPmpaType.TIT.Bi, clsPmpaType.TIT.InDate, clsPmpaType.TIT.OutDate, clsPmpaType.TIT.Sname, clsPmpaType.TIT.ArcDate) == false)
            {
                clsDB.setRollbackTran(pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
          
            clsDB.setCommitTran(pDbCon);
            
            Cursor.Current = Cursors.Default;

            this.Close();

        }

        void eSave_OK(PsmhDb pDbCon)
        {
            int i = 0;
            int nCNT = 0;
            int nIlsu = 0;
            long nIPDNO = 0;
            long nTRSNo = 0;
            
            bool bArcYN = false;

            string strChk2 = string.Empty;
            string strGbSTS = string.Empty;
            string strError = string.Empty;
            string strNgt = string.Empty;
            string strLastChk = string.Empty;
            string strLastInDate1 = string.Empty;
            string strNu = string.Empty;
            string strSelf = string.Empty;
            string strBDate = string.Empty;
            string strSugbs = string.Empty;
            string strSuNext = string.Empty;
            string strOutDate_Drg = string.Empty;
            string strPano = string.Empty;
            string strBi = string.Empty;

            //최종자격 체크용
            long nNew = 0;
            long nOld = 0;

            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            nTRSNo = clsPmpaType.TIT.Trsno;
            nIPDNO = clsPmpaType.TIT.Ipdno;
            strPano = clsPmpaType.TIT.Pano;
            strBi = clsPmpaType.TIT.Bi;

            Cursor.Current = Cursors.WaitCursor;


            #region //소아 면제 나이 점검 
            //2016-08-19
            string strHC = VB.Left(cboHC.Text, 1);
            string strPD = VB.Left(cboPD.Text, 1);
            int Age_Chake = 0;
            double AGEILSU = 0;

            AGEILSU = ComFunc.AgeCalcEx_Zero(clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin3, clsPublic.GstrSysDate);
            // Age_Chake = cBAcct.Bas_Add_Age_Set(clsPmpaType.TIT.Age, AGEILSU, clsPublic.GstrSysDate, false, false, "");
            Age_Chake = ComFunc.AgeCalcEx(clsPmpaType.TIT.Jumin1 + clsPmpaType.TIT.Jumin3, clsPublic.GstrSysDate);
            if (strPD == "P")
            {
                if (Age_Chake != 0 )
                {
                    if (ComFunc.MsgBoxQ("★소아면제코드 확인요망" + '\r' + Age_Chake + "세" + '\r' + "그대로 진행하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return;
                    }
                }
                else if (Age_Chake == 0 && AGEILSU > 28)
                {
                    if (ComFunc.MsgBoxQ("★소아면제코드 확인요망" + '\r' + "생후 " + AGEILSU + "일 " + '\r' + "그대로 진행하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            else if (strPD == "S")
            {
                if (Age_Chake >= 6 )
                {
                    if (ComFunc.MsgBoxQ("★소아면제코드 확인요망" + '\r' + Age_Chake + "세" + '\r' + "그대로 진행하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            else if(strPD == "Y")
            {
                if (Age_Chake > 15 || Age_Chake < 6)
                {
                    if (ComFunc.MsgBoxQ("★소아면제코드 확인요망" + '\r' + Age_Chake + "세" + '\r' + "그대로 진행하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            if (strPD == "Y")
            {
                if (clsPmpaType.TIT.Age == 15 && Age_Chake >= 16)
                {
  
                    if (ComFunc.MsgBoxQ("★ 입원기간중  15세에서 16세로 나이변경 기간분리하세요" + '\r' + clsPmpaType.TIT.Age + "세 입원나이" + Age_Chake + "세 현재나이 " + '\r' + "그대로 진행하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return;
                    }
                }
            }



            #endregion


            #region // 심사완료가 된것을 다시 심사완료 처리 불가
            SQL = "";
            SQL += "SELECT AmSet1,AmSet3,GbSTS                      ";
            SQL += "  FROM " + ComNum.DB_PMPA + "IPD_TRANS          ";
            SQL += " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + "    ";
            SQL += "   AND IPDNO = " + clsPmpaType.TIT.Ipdno + "    ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                if (string.Compare(dt.Rows[0]["GbSTS"].ToString().Trim(), "5") >= 0)
                {
                    if (dt.Rows[0]["AMSET1"].ToString().Trim() == "2")
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("이미 퇴원계산서 발부가 완료 되었습니다.", "오류");
                        return;
                    }
                    else if (dt.Rows[0]["AmSet3"].ToString().Trim() == "9")
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("퇴원심사가 이미 완료되었습니다.", "오류");
                        return;
                    }

                    ComFunc.MsgBox("퇴원심사가 이미 완료되었습니다.", "오류");
                    return;
                }
                else
                {
                    if (dt.Rows[0]["AMSET1"].ToString().Trim() == "2")
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("이미 퇴원계산서 발부가 완료 되었습니다.", "오류");
                        return;
                    }
                    else if (dt.Rows[0]["AmSet3"].ToString().Trim() == "9")
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("퇴원심사가 이미 완료되었습니다.", "오류");
                        return;
                    }
                }
            }

            dt.Dispose();
            dt = null;
            #endregion

            #region // 자격이 분리된경우 앞자격을 먼저 심사완료 해야함.(입원료발생 기준때문)  2014-01-15
            //가퇴원인 상태에서 나중에 심사를 하는경우 발생함.
            if (string.Compare(clsPmpaType.TIT.OutDate, clsPublic.GstrSysDate) < 0)
            {
                SQL = "";
                SQL += " SELECT GbSTS                                                           ";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_TRANS                                 ";
                SQL += "  WHERE IPDNO = " + clsPmpaType.TIT.Ipdno + "                           ";
                SQL += "    AND INDATE < TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD') ";
                SQL += "    AND GBIPD NOT IN ('9','D')                                          ";
                SQL += "    AND GbSTS IN ('1','2','3')                                          ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    clsPublic.GstrMsgList = "";
                    clsPublic.GstrMsgList += "다른자격(앞자격)의 심사가 완료되지 않았습니다." + ComNum.VBLF + ComNum.VBLF;
                    clsPublic.GstrMsgList += "다른자격(앞자격)의 심사를 먼저 완료해주십시오." + ComNum.VBLF;
                    clsPublic.GstrMsgList += "(입원료 발생일자 기준때문)";
                        
                    ComFunc.MsgBox(clsPublic.GstrMsgList, "확인");
                        
                    dt.Dispose();
                    dt = null;
                    
                    return;
                }
                dt.Dispose();
                dt = null;
            }
            #endregion

            #region //DRG 환자의 DRGCODE 점검
            //2016-08-19
            if (clsPmpaType.TIT.GbDRG =="D" )
            {
                if (clsPmpaType.TIT.DrgCode.Trim() == "" || clsPmpaType.TIT.DrgCode.Trim() == null)
                {
                    ComFunc.MsgBox("DRG임시번호부여 환자입니다 DRG코드가 부여되지 않았습니다.", "등록불가!");
                    return;
                }
            }

          
            #endregion


            #region // F014 자격 점검
            //2016-08-19
            if (VB.Left(cboFCode.Text, 4) == "F014")
            { 
                if (cboCan.Text.Trim() != "" || cboPD.Text.Trim() != "" || cboHC.Text.Trim() != "")
                {
                    ComFunc.MsgBox("F014 등록시 순수 건강보험만 가능합니다.", "등록불가!");
                    return;
                }
            }

            string strFCode = VB.Left(cboFCode.Text.Trim(), 4);
            //2016-08-18 F014여부 체크
            if (clsPmpaType.TIT.Bi.Substring(0, 1) == "1")
            {
                if (cJSQL.Read_Mir_ILLS(pDbCon, strPano, strBi, clsPmpaType.TIT.InDate, nIPDNO, nTRSNo, strFCode) == false)
                {
                    return;
                }
            }

            if (clsPmpaType.TIT.GbIpd == "9" && cboEtc.Text.Trim() == "" && VB.Left(cboFCode.Text, 4) != "MT04")  //코로나 지병분리시 상해외인코드 제외가능 심사팀 김준수 20.11.04
            {
                clsPublic.GstrMsgList = "지병자격인데 상해외인코드가 없습니다.";
                clsPublic.GstrMsgList += ComNum.VBLF + ComNum.VBLF + "상해외인코드를 등록후 심사완료하십시오!";

                ComFunc.MsgBox(clsPublic.GstrMsgList, "확인");
                return;
            }
            #endregion

            #region //2014-11-15 입원료 누락건 점검
            if (clsPmpaType.TIT.InDate != clsPmpaType.TIT.OutDate)
            {
                //if (cISentChk.Check_RoomCharge_Exist(pDbCon, nIPDNO, clsPmpaType.TIT.InDate, clsPmpaType.TIT.OutDate) == false)
                //{
                //    return;
                //}
                cISentChk.Check_RoomCharge_Exist(pDbCon, nIPDNO, clsPmpaType.TIT.InDate, clsPmpaType.TIT.OutDate);
            }
            #endregion

            #region //미전송 세부점검 - 입원자격건중 마지막 자격 퇴원걸때 추가 점검
            
            dt = cJSQL.sel_Ipd_NextTrans(pDbCon, strPano, nIPDNO, clsPmpaType.TIT.InDate);
            if (dt != null)
            {   
                if (dt.Rows.Count > 0)
                {
                    strLastChk = "NO";  //'한개라도 있으면 최종자격아님 - 기간정해짐

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nNew = (long)VB.Val(dt.Rows[i]["TRSNO"].ToString());

                        strLastInDate1 = dt.Rows[i]["InDate"].ToString().Trim(); 

                        if (i != 0 && nNew != nOld)
                        {
                            strLastChk = "OK";
                            break;
                        }
                        nOld = (long)VB.Val(dt.Rows[i]["TRSNO"].ToString());
                    }
                }

            }

            dt.Dispose();
            dt = null;

            #endregion

            #region 노인틀니정보 입력 체크
            if (clsPmpaType.TIT.DeptCode != "DT" && cboJinDtl.Text.Trim() != "")
            {
                ComFunc.MsgBox("노인틀니정보는 치과 자격만 입력할수있습니다.. 다시 선택하세요" , "확인");
                return;
            }
            #endregion

            #region //DRG시행 DRG 조사서식 여부 확인
            if (clsPmpaType.TIT.GbDRG == "D")
            {
                SQL = "";
                SQL += " SELECT ROWID FROM " + ComNum.DB_PMPA + "DRG_CHART1     \r\n";
                SQL += "  Where IPDNO = " + nIPDNO + "                          \r\n";
                SQL += "    AND trsno = " + nTRSNo + "                          \r\n";
                SQL += "    AND ( Deldate is null or Deldate ='' )              \r\n";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("DRG 조사서식이 누락되었습니다.", "확인");
                }
                dt.Dispose();
                dt = null;
            }
            #endregion

            #region // 미전송 OCS 오더를 Check
            if (cJSQL.sel_Ipd_NonTrans_Order(pDbCon, strPano, clsPmpaType.TIT.InDate, clsPmpaType.TIT.OutDate, strLastInDate1, strLastChk) == false)
            {
                return;
            }
            #endregion

            #region // 수가발생 일자별 오류 Check  2009-01-03 일 부터 점검
            if (cJSQL.sel_Ipd_NewSlip_Minus(pDbCon, strPano, nIPDNO, nTRSNo) == false)
            {
                return;
            }
            #endregion

            #region //2016-09-05 S항 6, 7 수가오류항목 체크
            if (string.Compare(clsPmpaType.TIT.Bi, "30") < 0)
            {
                if (cJSQL.sel_Ipd_NewSlip_SugbS(pDbCon, strPano, nIPDNO, nTRSNo) == false)
                {
                    return;
                }
            }
            #endregion

            #region 컨설트 미확인 점검
            if (cJSQL.sel_Ipd_NewSlip_Consult(pDbCon, strPano, nIPDNO, clsPmpaType.TIT.InDate, clsPmpaType.TIT.OutDate) == false)
            {
                return;
            }
            #endregion

            #region 혈액불출 관련 미전송 오더 체크 2014-03-17
            if (cJSQL.sel_Ipd_Work_IpdSlip_Send_Chk(pDbCon, strPano, clsPmpaType.TIT.InDate) == false)
            {
                return;
            }
            #endregion

            #region //Verbal 미확정 오더 체크
            if (cISentChk.Chk_Ipd_Verbal_Order(pDbCon, strPano, clsPmpaType.TIT.InDate, clsPmpaType.TIT.OutDate) == false)
            {
                return;
            }
            #endregion

            #region 계약처감액인 경우 계약처 코드 점검
            if (clsPmpaType.TIT.GbGameK == "55" && clsPmpaType.TIT.GelCode == "")
            {
                ComFunc.MsgBox("심사완료 할수없습니다. 계약처코드가 없습니다. 원무과에 자격확인 후 심사완료 하십시오", "작업불가");
                return;
            } 
            #endregion

            #region //의료급여 정신과 단일수가 점검 2012-11-03
            if ((strBi == "21" || strBi == "22") && clsPmpaType.TIT.DeptCode == "NP")
            {
                if (cJSQL.sel_Ipd_Gub_Np_Chk(pDbCon, strPano, nIPDNO, nTRSNo) == false)
                {
                    return;
                }
            }
            #endregion

            #region //퇴원일자 보다 큰 처방일자가 있는지 체크함.2006-02-23
            if (cJSQL.sel_Ipd_Trans_LastSlip_Chk(pDbCon, strPano, nIPDNO, nTRSNo, clsPmpaType.TIT.OutDate) == false)
            {
                return;
            }
            #endregion

            #region //입원일기간 외래 검사예약 시행 확인
            if (cJSQL.sel_Ipd_Trans_Exam_Chk(pDbCon, strPano, nIPDNO, clsPmpaType.TIT.InDate, clsPmpaType.TIT.OutDate) == false) { }
            #endregion

            string strCan = VB.Left(cboCan.Text, 4);

            #region 중증/희귀/난치 산정특례대상 팝업(2021-11-11)
            if (strCan.Trim() != "")
            {
                if (chkSlipM(clsPmpaType.TIT.Trsno) == "OK")
                {
                    ComFunc.MsgBox("< 중증/희귀/난치 산정특례대상입니다. 치료재료수가 확인하세요.(결핵,치매는 제외)>", "확인");
                }
            }
            switch (strHC.Trim())
            {
                case "V":
                case "H":
                case "1":
                case "2":
                    if (chkSlipM(clsPmpaType.TIT.Trsno) == "OK")
                    {
                        ComFunc.MsgBox("< 중증/희귀/난치 산정특례대상입니다. 치료재료수가 확인하세요.(결핵,치매는 제외)>", "확인");
                    }
                    break;
            } 
            #endregion


            try
            {

                #region //중증(암)환자 등록 작업
                if (cboCan.Text.Trim() != "" && strBi != "21")
                {
                    clsDB.setBeginTran(pDbCon);

                    if (VB.Left(cboHC.Text, 1) != "C")
                    {
                        if (string.Compare(clsPmpaType.TIT.InDate, "2010-07-01") >= 0 && (strCan == "V247" || strCan == "V248" || strCan == "V249" || strCan == "V250"))
                        {
                            SQL = "";
                            SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS ";
                            SQL += "    SET VCODE = '" + strCan + "',     ";
                            SQL += "        BONRATE = 5                     ";
                            SQL += "  WHERE TRSNO = " + nTRSNo + "          ";
                            SQL += "    AND IPDNO = " + nIPDNO + "          ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return;
                            }
                        }
                        //V268 뇌출혈추가
                        else if (string.Compare(clsPmpaType.TIT.InDate, "2010-01-01") >= 0 && (strCan == "V191" || strCan == "V192" || strCan == "V193" || strCan == "V194" || strCan == "V268" || strCan == "V275"))
                        {
                            SQL = "";
                            SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS ";
                            SQL += "    SET VCODE = '" + strCan + "',     ";
                            SQL += "        BONRATE = 5                     ";
                            SQL += "  WHERE TRSNO = " + nTRSNo + "          ";
                            SQL += "    AND IPDNO = " + nIPDNO + "          ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return;
                            }
                        }
                        else if (string.Compare(clsPmpaType.TIT.InDate, "2009-12-01") >= 0 && strBi.Substring(0, 1) == "1" && (strCan == "V193" || strCan == "V194"))
                        {
                            SQL = "";
                            SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS ";
                            SQL += "    SET VCODE = '" + strCan + "',       ";
                            SQL += "        BONRATE = 5                     ";
                            SQL += "  WHERE TRSNO = " + nTRSNo + "          ";
                            SQL += "    AND IPDNO = " + nIPDNO + "          ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return;
                            }
                        }
                        else
                        {
                            SQL = "";
                            SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS     ";
                            SQL += "    SET VCODE = '" + strCan + "',         ";
                            SQL += "        BONRATE = 10                        ";
                            SQL += "  WHERE TRSNO = " + nTRSNo + "              ";
                            SQL += "    AND IPDNO = " + nIPDNO + "              ";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return;
                            }
                        }
                    }

                    clsDB.setCommitTran(pDbCon);

                }
                else if (cboCan.Text.Trim() != "" && strBi == "21")
                {
                    clsDB.setBeginTran(pDbCon);

                    SQL = "";
                    SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS     ";
                    SQL += "    SET VCODE = '" + strCan + "'            ";
                    SQL += "  WHERE TRSNO = " + nTRSNo + "              ";
                    SQL += "    AND IPDNO = " + nIPDNO + "              ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    clsDB.setCommitTran(pDbCon);

                }
                #endregion

                #region //2009-06-01 부터 의료급여2종 본인부담 10% 윤조연
                if (string.Compare(clsPmpaType.TIT.InDate, "2009-06-01") >= 0 && strBi == "22")
                {
                    clsDB.setBeginTran(pDbCon);

                    SQL = "";
                    //V268 뇌출혈추가
                    if (string.Compare(clsPmpaType.TIT.InDate, "2010-01-01") >= 0 && (strCan == "V191" || strCan == "V192" || strCan == "V193" || strCan == "V194" || strCan == "V268" || strCan == "V275"))
                    {
                        SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS     ";
                        SQL += "    SET BONRATE = 5                         ";
                    }
                    else if (clsPmpaType.TIT.OgPdBun == "O" || clsPmpaType.TIT.Age < 6)
                    {
                        SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS     ";
                        SQL += "    SET BONRATE = 0                         ";
                    }
                    else
                    {
                        SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS     ";
                        SQL += "    SET BONRATE = 10                        ";
                    }
                    SQL += " WHERE TRSNO = " + nTRSNo + "                   ";
                    SQL += "   AND IPDNO = " + nIPDNO + "                   ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    clsDB.setCommitTran(pDbCon);
                }
                #endregion
                
                #region //차상위 2종 세팅
                if (strBi.Substring(0, 1) == "1" && (clsPmpaType.TIT.OgPdBun == "E" || clsPmpaType.TIT.OgPdBun == "F" || clsPmpaType.TIT.OgPdBun == "J" || clsPmpaType.TIT.OgPdBun == "K"))
                {
                    clsDB.setBeginTran(pDbCon);

                    SQL = "";
                    SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";

                    if (VB.Left(cboHC.Text, 1) == "E")         // 차상위계층환자-만성질환자 2009-03-19
                    {
                        if (string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0 && VB.Left(cboPD.Text, 1) == "Y")
                        {
                            SQL += " OGPDBUN  = 'E', ";
                            SQL += " BONRATE= 3 ";
                        }
                        else if ((strCan == "V247" || strCan == "V248" || strCan == "V249" || strCan == "V250") && string.Compare(clsPmpaType.TIT.InDate, "2010-07-01") >= 0)
                        {
                            SQL += " OGPDBUN  = 'E', ";
                            SQL += " BONRATE= 5, ";
                            SQL += " VCODE = '" + strCan + "' ";
                        }
                        //V268 뇌출혈추가
                        else if (strCan == "V191" || strCan == "V192" || strCan == "V193" || strCan == "V194" || strCan == "V268" || strCan == "V275")
                        {
                            SQL += " OGPDBUN  = 'E', ";
                            SQL += " BONRATE= 5, ";
                            SQL += " VCODE = '" + strCan + "' ";
                        }
                        else if (VB.Left(strCan, 1) == "V")
                        {
                            SQL += " OGPDBUN  = 'E', ";
                            SQL += " BONRATE= 10, ";
                            SQL += " VCODE = '" + VB.Left(cboVDetail.Text, 4).ToUpper() + "' ";
                        }
                        else if (clsPmpaType.TIT.DeptCode == "NP")
                        {
                            SQL += " OGPDBUN  = 'E', ";
                            SQL += " BONRATE= 10 ";
                        }
                        //2015-06-29
                        else if (string.Compare(clsPmpaType.TIT.InDate, "2015-07-01") >= 0 && VB.Left(cboPD.Text, 1) == "T")
                        {
                            SQL += " OGPDBUN  = 'E', ";
                            //2016-12-30 고위험임산부 요율 변경
                            if (string.Compare(clsPmpaType.TIT.InDate, "2017-01-01") >= 0)
                            { 
                                SQL += " BONRATE= 5 ";
                            }
                            else
                            { 
                                SQL += " BONRATE= 10 ";
                            }
                        }
                        else
                        {
                            SQL += " OGPDBUN  = 'E', ";
                            SQL += " BONRATE= 14 ";
                        }
                    }
                    else if (VB.Left(cboHC.Text, 1) == "F")          // 차상위계층환자-장애인 만성질환자 2009-03-19
                    {
                        if (string.Compare(clsPmpaType.TIT.InDate, "2017-10-01") >= 0 && VB.Left(cboPD.Text, 1) == "Y")
                        {
                            SQL += " OGPDBUN  = 'F', ";
                            SQL += " BONRATE= 3 ";
                        }
                        else if ((strCan == "V247" || strCan == "V248" || strCan == "V249" || strCan == "V250") && string.Compare(clsPmpaType.TIT.InDate, "2010-07-01") >= 0)
                        {
                            SQL += " OGPDBUN  = 'F', ";
                            SQL += " BONRATE= 5, ";
                            SQL += " VCODE = '" + strCan + "' ";
                        }
                        //V268 뇌출혈추가
                        else if (strCan == "V191" || strCan == "V192" || strCan == "V193" || strCan == "V194" || strCan == "V268" || strCan == "V275")
                        {
                            SQL += " OGPDBUN  = 'F', ";
                            SQL += " BONRATE= 5, ";
                            SQL += " VCODE = '" + strCan + "' ";
                        }
                        else if (VB.Left(strCan, 1) == "V")
                        {
                            SQL += " OGPDBUN  = 'F', ";
                            SQL += " BONRATE= 10, ";
                            SQL += " VCODE = '" + VB.Left(cboVDetail.Text, 4).ToUpper() + "' ";
                        }
                        else if (clsPmpaType.TIT.DeptCode == "NP")
                        {
                            SQL += " OGPDBUN  = 'F', ";
                            SQL += " BONRATE= 10 ";
                        }
                        //2015-06-29
                        else if (string.Compare(clsPmpaType.TIT.InDate, "2015-07-01") >= 0 && VB.Left(cboPD.Text, 1) == "T")
                        {
                            SQL += " OGPDBUN  = 'E', ";
                            //2016-12-30 고위험임산부 요율 변경
                            if (string.Compare(clsPmpaType.TIT.InDate, "2017-01-01") >= 0)
                            { 
                                SQL += " BONRATE= 5 ";
                            }
                            else
                            { 
                                SQL += " BONRATE= 10 ";
                            }
                        }
                        else
                        { 
                            SQL += " OGPDBUN  = 'F', ";
                            SQL += " BONRATE= 14 ";
                        }
                    }
                    else if (VB.Left(cboHC.Text, 1) == "J")         //차상위계층환자-만성질환자-상해외인 2009-03-19
                    {
                        SQL += " OGPDBUN  = 'J', ";
                        SQL += " BONRATE= 14 ";
                    }
                    else if (VB.Left(cboHC.Text, 1) == "K")         //차상위계층환자-장애인 만성질환자-상해외인 2009-03-19
                    {
                        SQL += " OGPDBUN  = 'K', ";
                        SQL += " BONRATE= 14 ";
                    }
                    SQL += " WHERE TRSNO = " + nTRSNo + " ";
                    SQL += "   AND IPDNO = " + nIPDNO + " ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    clsDB.setCommitTran(pDbCon);

                }

                #endregion

                #region //산정특례 등록 희귀난치성 2009-06-19 윤조연
                if (strBi.Substring(0, 1) == "1" && (clsPmpaType.TIT.OgPdBun == "1" || clsPmpaType.TIT.OgPdBun == "2"))
                {
                    clsDB.setBeginTran(pDbCon);

                    SQL = "";
                    SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";
                    if (cboVDetail.Text.Trim() != "")
                    {
                        SQL += " VCode  = '" + VB.Left(cboVDetail.Text, 4).ToUpper() + "', ";
                    }
                    else
                    {
                        SQL += " VCode  = '', ";
                    }
                    SQL += "       BONRATE= 10 ";
                    SQL += " WHERE TRSNO = " + nTRSNo + "               ";
                    SQL += "   AND IPDNO = " + nIPDNO + "               ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    clsDB.setCommitTran(pDbCon);
                }
                else if (strBi.Substring(0, 1) == "1" && clsPmpaType.TIT.OgPdBun == "V" && string.Compare(clsPmpaType.TIT.InDate, "2009-07-01") >= 0)
                {
                    clsDB.setBeginTran(pDbCon);

                    SQL = "";
                    SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";
                    SQL += " OGPDBUN  = 'V', ";
                    if (cboVDetail.Text.Trim() != "")
                    {
                        SQL += " VCode  = '" + VB.Left(cboVDetail.Text, 4).ToUpper() + "', ";
                    }
                    else
                    {
                        SQL += " VCode  = '',                           ";
                    }
                    SQL += " BONRATE= 10                                ";
                    SQL += " WHERE TRSNO = " + nTRSNo + "               ";
                    SQL += "   AND IPDNO = " + nIPDNO + "               ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    clsDB.setCommitTran(pDbCon);
                }
                else if (strBi.Substring(0, 1) == "1" && clsPmpaType.TIT.OgPdBun == "H" && string.Compare(clsPmpaType.TIT.InDate, "2009-07-01") >= 0)
                {
                    clsDB.setBeginTran(pDbCon);

                    SQL = "";
                    SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET ";
                    SQL += " OGPDBUN  = 'H', ";
                    if (cboVDetail.Text.Trim() != "")
                    {
                        SQL += " VCode  = '" + VB.Left(cboVDetail.Text, 4).ToUpper() + "', ";
                    }
                    else
                    {
                        SQL += " VCode  = '',                           ";
                    }
                    SQL += " BONRATE= 10                                ";
                    SQL += " WHERE TRSNO = " + nTRSNo + "               ";
                    SQL += "   AND IPDNO = " + nIPDNO + "               ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    clsDB.setCommitTran(pDbCon);
                }
                #endregion

                #region //2013-09-26 초음파 급여 대상자 Slip 체크

                dt = cJSQL.sel_Ipd_NewSlip_Sono(pDbCon, strPano, nIPDNO, nTRSNo);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt16(VB.Val(dt.Rows[0]["nQty"].ToString())) > 0)
                        {
                            if (clsPmpaType.TIT.OgPdBun == "V" || clsPmpaType.TIT.OgPdBun == "H")
                            {

                            }
                            else
                            {
                                if (clsPmpaType.TIT.VCode == "V191" || clsPmpaType.TIT.VCode == "V192" || clsPmpaType.TIT.VCode == "V193")
                                {
                                    
                                }
                                else
                                {
                                    ComFunc.MsgBox("초음파 급여항목은 4대 중증질환 환자만 가능합니다. 처방내역을 확인 하십시오.", "처방확인");
                                }
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null; ;

                #endregion
                
                #region //퇴원 ARC 작업

                ComFunc.ReadSysDate(pDbCon);
                clsPmpaPb.GstrActDate = clsPublic.GstrSysDate;

                bArcYN = true;
                if (clsPmpaType.TIT.GbIpd == "9") { bArcYN = false; }   //지병
                if (string.Compare(clsPmpaType.TIT.ArcDate, clsPmpaType.TIT.OutDate) >= 0)
                {
                    bArcYN = false;   //ARC실행일 보다 적거나 같으면 NO
                }
                if (clsPmpaType.TIT.InDate == clsPmpaType.TIT.OutDate && clsPmpaType.IMST.InDate == clsPmpaType.TIT.OutDate && string.Compare(clsPmpaType.IMST.InTime, "17:00") > 0)
                {
                    bArcYN = false;     //2018-01-03 add  IMST.InDate = TIT.OutDate
                }

                //병실료, 실료차, 식대는 계산만 실행
                if (bArcYN)
                {
                    clsDB.setBeginTran(pDbCon);

                    clsPmpaPb.GstrARC = "OUT";  //퇴원 ARC
                    if (cIARC.ARC_MAIN_PROCESS(pDbCon, clsPmpaType.TIT.OutDate) == true)
                    { 
                        clsDB.setCommitTran(pDbCon);
                    }
                    else
                    { 
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("퇴원 A.R.C 작업 중 오류가 발생함", "오류");
                        return;
                    }
                }
                #endregion

                #region Data UpDate
                clsDB.setBeginTran(pDbCon);

                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS                        ";
                SQL += "    SET AmSet3 = '9',                                          ";
                SQL += "        GbSTS = '5',                                           ";  //심사완료
                SQL += "        SimsaTime = SYSDATE,                                   ";
                SQL += "        SimsaSabun = '" + clsType.User.IdNumber + "',          ";
                SQL += "        PrintTime = '',                                        ";
                SQL += "        JinDtl = '" + VB.Left(cboJinDtl.Text, 2).Trim() + "',  ";  //2012-07-03 노인틀니
                SQL += "        SangAmt = 0,                                           ";  //상한제 대상 금액
                SQL += "        Amt51 = 0,                                             ";  //보증금,중간납 대체액
                SQL += "        Amt53 = 0,                                             ";  //조합부담
                SQL += "        Amt54 = 0,                                             ";  //할인액
                SQL += "        Amt55 = 0,                                             ";  //차인납부
                SQL += "        Amt56 = 0,                                             ";  //개인미수
                SQL += "        Amt57 = 0,                                             ";  //퇴원금
                SQL += "        Amt58 = 0,                                             ";  //환불금
                SQL += "        Amt67 = 0,                                             ";  //
                //2010-11-19 지병 outdate 있으면 일수 갱신 - 최초 지병등록시 계산해서 일수계산하는데 추후 날짜 변경후 갱신안되서
                if (clsPmpaType.TIT.GbIpd == "9" && clsPmpaType.TIT.OutDate != "")
                {
                    nIlsu = CF.DATE_ILSU(pDbCon, clsPmpaType.TIT.OutDate, clsPmpaType.TIT.InDate);
                    SQL += "    ilsu = " + nIlsu + ",                                   "; //일수
                }
                SQL += "        Amt60 = 0                                               ";
                SQL += "  WHERE TRSNO  = " + nTRSNo + "                                 ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                #endregion

                #region //모든 IPD_TRANS가 심사완료가 되었는지 점검함
                SQL = "";
                SQL += " SELECT COUNT(*) CNT FROM " + ComNum.DB_PMPA + "IPD_TRANS   ";
                SQL += "  WHERE IPDNO = " + nIPDNO + "                              ";
                SQL += "    AND GBIPD <> 'D'                                        "; //삭제가 아닌것
                SQL += "    AND GBSTS IN ('0','1','2','3','4')                      ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    nCNT = Convert.ToInt16(VB.Val(dt.Rows[0]["CNT"].ToString()));
                }

                dt.Dispose();
                dt = null;
                #endregion

                clsDB.setBeginTran(pDbCon);

                #region IPD_NEW_MASTER 부분 심사완료 SET
                
                if (nCNT == 0)
                { 
                    SQL = "";
                    SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER    ";
                    SQL += "    SET GbSTS = '5',                            ";
                    SQL += "        SimsaTime = '',                         ";
                    SQL += "        PrintTime = ''                          ";
                    SQL += "  WHERE IPDNO = " + nIPDNO + "                  ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                }
                #endregion

                #region //2010-11-22 심사완료시 - 입원 저가약제 금액 계산하여 생성 - BBBBBB 코드 (구현안함 2012년도까지만 시행)
                //if (clsPmpaPb.GstrDrug_Amt_UseFlag == "OK")
                //{ 
                //    //IPD_DRUG_BBBBBB_INSERT2_20120201(TIT.pano, nIPDNO, nTRSNo, GstrSysDate, TIT.OutDate, TIT.Bi, TIT.DeptCode, TIT.DrCode, TIT.WardCode, TIT.RoomCode)
                //}
                #endregion

                #region 안전관리료 생성
                if (clsPmpaType.TIT.GbIpd != "9" && string.Compare(clsPmpaType.TIT.OutDate, "2017-10-01") >= 0 && chkAc421N.Checked == false)
                {
                    if (cIARC.Run_Arc_AcCode_Auto(pDbCon, strPano, clsPmpaType.TIT.OutDate) == false)   //2017-10-01 안전관리료
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("안전관리료 생성 실패!", "오류");
                        return;
                    }
                    if (cIARC.Run_Arc_AHCode_Auto(pDbCon, strPano, clsPmpaType.TIT.OutDate) == false)   //2019-01-21 감염예방관리료
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("감염예방관리료 생성 실패!", "오류");
                        return;
                    }
                    if (cIARC.Run_Arc_AICode_Auto(pDbCon, strPano, clsPmpaType.TIT.OutDate) == false)   //2019-01-21 감염예방관리료
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("야간간호료 생성 실패!", "오류");
                        return;
                    }

                }
                #endregion

                #region //금액 계산 DRG001, DRG002 수가 입력을 위해 수가 전체 다시 계산
                //IPD_TRANS AMT01-AMT50 다시 계산함.
                if (cIA.Ipd_Trans_Amt_ReBuild(pDbCon, nTRSNo, "") == false)
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("총진료비를 재계산 도중에 오류가 발생함!!, 전산실로 연락바람!!!");
                    return;
                }

                //조합부담, 본인부담, 할인금액을 계산
                if (cIA.Ipd_Tewon_Amt_Process(pDbCon, nTRSNo, "", "") == false)
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("조합부담,본인부담,할인액을 계산 도중에 오류가 발생함!!");
                    return;
                }

                //DRG 환자인경우
                if (clsPmpaType.TIT.GbDRG == "D" && clsPmpaType.TIT.DrgCode != "")
                {
                    //퇴원일자가 없으면 현재일자로 세팅
                    if (clsPmpaType.TIT.OutDate == "")
                    {
                        strOutDate_Drg = clsPublic.GstrSysDate;
                    }
                    else
                    {
                        strOutDate_Drg = clsPmpaType.TIT.OutDate;
                    }

                    string strInDate = clsPmpaType.TIT.InDate;
                    string strDrgCode = clsPmpaType.TIT.DrgCode;

                    //strOutDate_Drg = "2017-06-30";

                    strNgt = Drg.Read_GbNgt_DRG(pDbCon, strPano, nIPDNO, nTRSNo);

                    if (Drg.READ_DRG_AMT_MASTER(pDbCon, strDrgCode, strPano, nIPDNO, nTRSNo, strNgt, strInDate, strOutDate_Drg) == false)
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("DRG 금액을 계산 도중에 오류가 발생함!!");
                        return;
                    }

                    if (Drg.IPD_DRG_SLIP_INSERT(pDbCon, "DRG001", strPano, nIPDNO, nTRSNo, clsPublic.GstrSysDate, DRG.GnAmt1) == false)
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("DRG001 수가 처리 도중에 오류가 발생함!!");
                        return;
                    }

                    if (Drg.IPD_DRG_SLIP_INSERT(pDbCon, "DRG002", strPano, nIPDNO, nTRSNo, clsPublic.GstrSysDate, DRG.GnAmt2) == false)
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("DRG002 수가 처리 도중에 오류가 발생함!!");
                        return;
                    }
                }
                #endregion

                #region 최종 수가 전체 다시 계산
                //IPD_TRANS AMT01-AMT50 다시 계산함.
                if (cIA.Ipd_Trans_Amt_ReBuild(pDbCon, nTRSNo, "") == false)
                {
                    Cursor.Current = Cursors.Default;
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("총진료비를 재계산 도중에 오류가 발생함!!, 전산실로 연락바람!!!");
                    return;
                }

                if (clsPmpaType.TIT.GbDRG == "D" && clsPmpaType.TIT.DrgCode != "")
                {
                    //퇴원일자가 없으면 현재일자로 세팅
                    if (clsPmpaType.TIT.OutDate == "")
                    {
                        strOutDate_Drg = clsPublic.GstrSysDate;
                    }

                    string strInDate = clsPmpaType.TIT.InDate;
                    string strDrgCode = clsPmpaType.TIT.DrgCode;

                    //strOutDate_Drg = "2017-06-30";

                    strNgt = Drg.Read_GbNgt_DRG(pDbCon, strPano, nIPDNO, nTRSNo);

                    if (Drg.READ_DRG_AMT_MASTER(pDbCon, strDrgCode, strPano, nIPDNO, nTRSNo, strNgt, strInDate, strOutDate_Drg) == false)
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("DRG 금액을 계산 도중에 오류가 발생함!!");
                        return;
                    }
                    
                }
                else
                {   //일반 환자인 경우
                    //조합부담, 본인부담, 할인금액을 계산
                    if (cIA.Ipd_Tewon_Amt_Process(pDbCon, nTRSNo, "", "") == false)
                    {
                        Cursor.Current = Cursors.Default;
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("조합부담,본인부담,할인액을 계산 도중에 오류가 발생함!!");
                        return;
                    }
                }
                #endregion
                
                clsDB.setCommitTran(pDbCon);

                cIuM.Read_Ipd_Mst_Trans(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno, "");

                #region //심사 HISTORY 기록
                clsDB.setBeginTran(pDbCon);

                if (cJSQL.Simsa_History_SAVE(pDbCon, "", "심사완료", clsPmpaType.TIT.Pano, clsPmpaType.TIT.Ipdno, clsPmpaType.TIT.Trsno, "5", clsPmpaType.TIT.Bi, clsPmpaType.TIT.InDate, clsPmpaType.TIT.OutDate, clsPmpaType.TIT.Sname, clsPmpaType.TIT.ArcDate) == false)
                {
                    ComFunc.MsgBox("심사 HISTORY 내역 기록중 에러발생!");
                    clsDB.setRollbackTran(pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                #endregion

                #region //2011-02-15 정상애기 amtset4 =3 이면 입원료 AB2201 발생안됨체크
                if (clsPmpaType.TIT.AmSet4 == "3")
                {
                    SQL = "";
                    SQL += " SELECT SUM(QTY*NAL) CNT                    ";
                    SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP  ";
                    SQL += "  WHERE IPDNO = " + nIPDNO + "              ";
                    SQL += "    AND TRSNO = " + nTRSNo + "              ";
                    SQL += "    AND SUCODE = 'AB2201'                   ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt16(VB.Val(dt.Rows[0]["CNT"].ToString())) > 0)
                        {
                            ComFunc.MsgBox("심사완료자격 : " + clsPmpaType.TIT.InDate + "~" + clsPmpaType.TIT.OutDate + " 정상애기인데 AB2201 발생함", "확인요망");
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
                #endregion

                #region //중증체크표시 2013-04-22
                if (cboCan.Text.Trim() == "")
                { 
                    SQL = "";
                    SQL += " SELECT GUBUN,PANO                                                      ";
                    SQL += "   FROM " + ComNum.DB_PMPA + "BAS_CANCER                                ";
                    SQL += "  WHERE PANO ='" + strPano + "'                                         ";
                    SQL += "    AND GUBUN IN  ('1')                                                 ";
                    SQL += "    AND (DELDATE IS NULL OR DELDATE ='')                                ";
                    SQL += "    AND fDATE<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')  ";
                    SQL += "    AND TDATE>=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')  ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ComFunc.MsgBox("중증암 등록이 있습니다. 참고하십시오!! ", "확인");
                    }

                    dt.Dispose();
                    dt = null;

                }

                if (cboHC.Text.Trim() == "")
                {
                    SQL = "";
                    SQL += " SELECT GUBUN,PANO                                                          ";
                    SQL += "   FROM " + ComNum.DB_PMPA + "BAS_CANCER                                    ";
                    SQL += "  WHERE PANO ='" + strPano + "'                                             "; 
                    SQL += "    AND GUBUN IN  ('2')                                                     ";
                    SQL += "    AND (DELDATE IS NULL OR DELDATE ='')                                    ";
                    SQL += "    AND FDATE <= TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')    ";
                    SQL += "    AND TDATE >= TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD')    ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ComFunc.MsgBox("희귀 등록이 있습니다. 참고하십시오!! ", "확인");
                    }

                    dt.Dispose();
                    dt = null;
                    
                }
                #endregion

                #region  Control Enable
                btnSave_VCode.Enabled = false;
                btnSave_PD.Enabled = false;
                btnSave_HC.Enabled = false;
                btnSave_VDetail.Enabled = false;
                btnSave_Etc.Enabled = false;
                btnSave_JinDtl.Enabled = false;
                #endregion

                #region //CHK_퇴원당일식대점검
                if (cISentChk.Chk_Discharge_Diet(pDbCon, strPano, nTRSNo, clsPmpaType.TIT.OutDate) == false)
                {
                    ComFunc.MsgBox("퇴원당일 식대점검이 완료되지 않았습니다. 다시 확인 바랍니다.", "확인");
                    return;
                }
                #endregion

                //입원당일과 퇴원당일 상급병실 사용여부 조회
                cISentChk.Chk_IpdTewon_RoomChaGesan(pDbCon, strPano, nIPDNO, clsPmpaType.TIT.InDate, clsPmpaType.TIT.OutDate);

                #region //응급실 경유 당일입퇴원자 입원료체크 확인멘트 2016-11-15
                if (clsPmpaType.TIT.InDate == clsPmpaType.TIT.OutDate)
                { 
                    if (clsPmpaType.TIT.AmSet7 == "3" || clsPmpaType.TIT.AmSet7 == "4" || clsPmpaType.TIT.AmSet7 == "5")
                    {
                        clsPublic.GstrMsgList = "";
                        clsPublic.GstrMsgList += "응급실 경유 당일입퇴원 대상자입니다." + ComNum.VBLF;
                        clsPublic.GstrMsgList += "입원료 산정유무를 확인하십시오." + ComNum.VBLF;
                        clsPublic.GstrMsgTitle = "★★응급실 경유 당일 입퇴원자 대상!!★★";
                        ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                    }
                }
                #endregion

                Cursor.Current = Cursors.Default;

                this.Close();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.setRollbackTran(pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        void eSave_Tax(PsmhDb pDbCon, CheckBox ch)
        {
            string strTax = string.Empty;
            
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            if (clsPmpaType.TIT.Trsno == 0)
            {
                return;
            }

            if (ch.Checked ==  true)
            {
                strTax = "1";
            }
            else
            {
                strTax = "";
            }
            
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(pDbCon);

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET     ";
            if (strTax == "")
            {
                SQL += "   AMT67 = 0,                               ";
            }

            SQL += "       GbTax = '" + strTax + "'                 ";
            SQL += " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + "    ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            
            clsDB.setCommitTran(pDbCon);

            Cursor.Current = Cursors.Default;
        }

        void eSave_JSimOk(PsmhDb pDbCon, CheckBox ch)
        {
            string strOK = string.Empty;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            if (clsPmpaType.TIT.Trsno == 0)
            {
                return;
            }

            strOK = ch.Checked == true ? "1" : "0";

            Cursor.Current = Cursors.WaitCursor;
            
            clsDB.setBeginTran(pDbCon);

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS SET     ";
            SQL += "       JSIM_OK = '" + strOK + "'                ";
            SQL += " WHERE TRSNO = " + clsPmpaType.TIT.Trsno + "    ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            clsDB.setCommitTran(pDbCon);

            Cursor.Current = Cursors.Default;
        }

        void eCboSelChanged(object sender, EventArgs e)
        {
            ComboBox CB = sender as ComboBox;

            if (CB.Text.Trim() == "")
            {
                CB.BackColor = Color.White;
            }
            else
            {
                CB.BackColor = Color.PeachPuff;
            }
        }

        void Screen_Clear()
        {
            int i = 0;

            for (i = 0; i < ss1.ActiveSheet.RowCount; i++)
            {
                if (i != 11)
                {
                    ss1.ActiveSheet.Cells[i, 1].Text = "";
                }
            }

            grpBox05.Enabled = false;
        }

        void Check_Screen_DisPlay(PsmhDb pDbCon)
        {
            string strOgPdBun = clsPmpaType.TIT.OgPdBun.Trim();
            string strOgPdDtl = clsPmpaType.TIT.OgPdBundtl.Trim();
            string strJinDtl = clsPmpaType.TIT.JinDtl == null ? "" : clsPmpaType.TIT.JinDtl.Trim();
            string strVCode = clsPmpaType.TIT.VCode.Trim();
            string strOgPd2 = clsPmpaType.TIT.OgPdBun2.Trim();
            string strFCode = clsPmpaType.TIT.FCode.Trim();
            string strPDBirth = string.Empty;

            if (VB.Val(clsPmpaType.IMST.KTASLEVL) > 0)
            {
                cboKTAS.SelectedIndex = Convert.ToInt16(VB.Val(clsPmpaType.IMST.KTASLEVL));
            }

            #region //중증환자 루틴
            //희귀난치V,H일경우는 중증암 항목코드없으면  희귀난치 항목으로 사용함. 
            if (strOgPdBun == "H" || strOgPdBun == "V" || strOgPdBun == "E" || strOgPdBun == "F" || strOgPdBun == "1" || strOgPdBun == "2")
            {
                //V268 뇌출혈포함
                if (strVCode == "V191" || strVCode == "V192" || strVCode == "V193" || strVCode == "V194" || strVCode == "V268" || strVCode == "V275")
                {
                    cboCan.SelectedIndex = cboCan.FindString(strVCode, -1);
                }
                else
                {
                    cboVDetail.SelectedIndex = cboVDetail.FindString(strVCode, -1);
                    if (cboVDetail.SelectedIndex > 0)
                    {
                        grpBox05.Enabled = true;
                    }
                }
            }
            else
            {
                cboCan.SelectedIndex = cboCan.FindString(strVCode, -1);
            }
            #endregion

            #region //소아면제 루틴
            if (strOgPdBun == "P" || strOgPdBun == "S" || strOgPdBun == "O" || strOgPdBun == "Y")
            {
                cboPD.SelectedIndex = cboPD.FindString(strOgPdBun, -1);
            }
            else if (strOgPdBun == "H" || strOgPdBun == "C" || strOgPdBun == "V" || strOgPdBun == "E" || strOgPdBun == "F" || strOgPdBun == "M" || strOgPdBun == "N" || strOgPdBun == "O" || strOgPdBun == "P" || strOgPdBun == "1" || strOgPdBun == "2")
            {
                cboHC.SelectedIndex = cboHC.FindString(strOgPdBun, -1);
            }
            #endregion

            #region //차상위2계층 구분이 소아면제,정상분만 하고 중복되서 다시 점검
            if (strOgPdBun == "E" || strOgPdBun == "F")
            {
                if (strOgPdDtl == "P" || strOgPdDtl == "S" || strOgPdDtl == "O" || strOgPdDtl == "T" || strOgPdDtl == "Y")
                {
                    cboPD.SelectedIndex = cboPD.FindString(strOgPdBun, -1);
                }
            }
            #endregion

            #region //OgPdBunDtl
            if (strOgPdDtl == "P" || strOgPdDtl == "S" || strOgPdDtl == "O" || strOgPdDtl == "T" || strOgPdDtl == "Y")
            {
                cboPD.SelectedIndex = cboPD.FindString(strOgPdDtl, -1);
            }
            #endregion

            #region //노인틀니 구분
            if (strJinDtl != "")
            {
                cboJinDtl.SelectedIndex = cboJinDtl.FindString(strJinDtl, -1);
            }
            #endregion

            #region 상해외인 구분 
            if (strOgPd2 != "")
            {
                cboEtc.SelectedIndex = cboEtc.FindString(strOgPd2, -1);
            }
            #endregion

            #region 입원명령 결핵지원 대상자
            if (strFCode != "")
            {
                cboFCode.SelectedIndex = cboFCode.FindString(strFCode, -1);
            }
            #endregion

            #region //심사완료전 정상분만,신생아 수가체크 
            if (clsPmpaType.TIT.DeptCode == "OG" || clsPmpaType.TIT.DeptCode == "PD")
            {
                cJSQL.Chk_JSim_OGSuga(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno);    //정상분만수가체크
                cJSQL.Chk_JSim_PDSuga(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno);    //신생아관련수가체크
            }
            #endregion

            //인정비급여 관련 메시지 팝업
            cJSQL.Chk_JSim_Bigub_SugaSP(pDbCon, clsPmpaType.TIT.Pano, clsPmpaType.TIT.Trsno);

            DisPlay_PatInfo();  //환자인적사항 DisPlay

            #region //소아면제 대상 체크
            if (strOgPdBun == "P" || strOgPdDtl == "P")
            {
                strPDBirth = ComFunc.GetBirthDate(clsPmpaType.TIT.Jumin1, clsPmpaType.TIT.Jumin3, "-");

                clsPublic.GstrMsgList = "입원일자 : " + clsPmpaType.TIT.InDate + "  생년월일 : " + strPDBirth + ComNum.VBLF;
                clsPublic.GstrMsgList += "출생일에서 입원일까지 : " + CF.DATE_ILSU(pDbCon, clsPmpaType.TIT.InDate, strPDBirth, "") + ComNum.VBLF;
                clsPublic.GstrMsgList += "소아 면제 대상인지 확인 부탁 드립니다!";

                txtRemark.Text = clsPublic.GstrMsgList;
            }
            else
            {
                if (clsPmpaType.TIT.DeptCode == "PD")
                {
                    strPDBirth = ComFunc.GetBirthDate(clsPmpaType.TIT.Jumin1, clsPmpaType.TIT.Jumin3, "-");

                    clsPublic.GstrMsgList = "입원일자 : " + clsPmpaType.TIT.InDate + "  생년월일 : " + strPDBirth + ComNum.VBLF;
                    clsPublic.GstrMsgList += "출생일에서 입원일까지 : " + CF.DATE_ILSU(pDbCon, clsPmpaType.TIT.InDate, strPDBirth, "");

                    txtRemark.Text = clsPublic.GstrMsgList;
                }
            }
            #endregion
            
        }

        void DisPlay_PatInfo()
        {
            ss1.ActiveSheet.Cells[0, 1].Text = clsPmpaType.TIT.Pano;
            ss1.ActiveSheet.Cells[1, 1].Text = clsPmpaType.TIT.Sname;
            ss1.ActiveSheet.Cells[2, 1].Text = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_환자종류", clsPmpaType.TIT.Bi);
            ss1.ActiveSheet.Cells[3, 1].Text = clsPmpaType.TIT.InDate;
            ss1.ActiveSheet.Cells[4, 1].Text = clsPmpaType.TIT.OutDate;
            ss1.ActiveSheet.Cells[5, 1].Text = clsPmpaType.TIT.GbIpd == "9" ? "지병" : "";
            ss1.ActiveSheet.Cells[6, 1].Text = CF.Read_Bcode_Name(clsDB.DbCon, "IPD_입원상태", clsPmpaType.TIT.TGbSts);
            ss1.ActiveSheet.Cells[7, 1].Text = clsPmpaType.TIT.AmSet3 == "9" ? "심사완료" : "";
            ss1.ActiveSheet.Cells[8, 1].Text = clsPmpaType.TIT.Ipdno.ToString("###,###,###");
            ss1.ActiveSheet.Cells[9, 1].Text = clsPmpaType.TIT.Trsno.ToString("###,###,###");
            ss1.ActiveSheet.Cells[10, 1].Text = clsPmpaType.TIT.DrgCode;
            
        }
        
    }
}
