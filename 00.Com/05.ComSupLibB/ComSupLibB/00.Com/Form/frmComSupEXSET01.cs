using ComLibB;
using ComSupLibB.Com;
using ComSupLibB.SupXray;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.SupFnEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupFnEx
    /// File Name       : frmComSupEXSET01.cs
    /// Description     : 기능검사 ABR 스케쥴 등록 관리
    /// Author          : 윤조연
    /// Create Date     : 2017-07-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 산부인과 검사 스케쥴 등록 폼 추가생성함 frmComSupEXSET01.cs 
    /// </history>
    /// <seealso cref= "\Ocs\.frm() >> frmComSupEXSET01.cs 폼이름 재정의" />
    public partial class frmComSupEXSET01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSQL comSql = new clsComSQL();
        ComFunc fun = new ComFunc();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSupXraySQL cSQL = new clsComSupXraySQL();
        clsSpread cspd = new clsSpread();
        clsComSup sup = new clsComSup();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSupFnExSpd fnexSpd = new clsComSupFnExSpd();
        clsComSupFnExSQL fnexSql = new clsComSupFnExSQL();
       

        clsComSupFnExSQL.c_ETC_SCHEDULE_OG cEtcSchOG = null;
        clsComSupXraySQL.cXrayDetail cXrayDetail = null;
        clsComSup.cBasPatient cBasPatient = null;

        frmComSupFnExPOP01 frmComSupFnExPOP01x = null; //예약변경시 달력사용

        string gDate1 = "";
        string gDate2 = "";
        string gJob = "";

        #endregion

        public frmComSupEXSET01(string argDate1,string argDate2, string argJob="")
        {
            InitializeComponent();
                        
            gDate1 = argDate1;
            gDate2 = argDate2;
            gJob = argJob;

            setEvent();
        }

        //기본값 세팅
        void setCtrlData(PsmhDb pDbCon)
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            if (gDate1 == "")
            {
                this.Close();
                return;
            }
            else
            {
                dtpFDate.Text = gDate1;
                if (gDate2 != "")
                {
                    dtpTDate.Text = gDate2;
                }
                else
                {
                    dtpTDate.Text = gDate1;
                }
            }

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnSearch1.Click += new EventHandler(eBtnEvent);
            this.btnExit.Click += new EventHandler(eBtnEvent);            
            this.btnSave1.Click += new EventHandler(eBtnEvent);
            this.btnSave2.Click += new EventHandler(eBtnEvent);
            this.btnDelete.Click += new EventHandler(eBtnEvent);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.Change += new ChangeEventHandler(eSpreadChange);
            this.ssList.EnterCell += new EnterCellEventHandler(sSpreadEnter);
            this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadBtnClicked);

            this.dtpFDate.TextChanged += eDtpTxtChange;


        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                //
                supSpd.sSpd_OgSch(ssList, supSpd.sSpdOgSch, supSpd.nSpdOgSch, 5, 0);

                setSpd(ssList);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등


                if (gJob == "XRAY")
                {
                    btnSave2.Visible = true;
                }

                screen_clear();

                setCtrlData(clsDB.DbCon);


                //기본 조회
                screen_display();
            }
            
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnSave1)
            {
                //저장
                eSave(clsDB.DbCon, "저장");

            }
            else if (sender == this.btnSave2)
            {
                //
                eSave(clsDB.DbCon, "선택수동예약");

            }
            else if (sender == this.btnDelete)
            {
                //삭제
                eSave(clsDB.DbCon, "삭제");
            }           
            else if (sender == this.btnSearch1)
            {
                screen_display();
            }


        }
                
        void eDtpTxtChange(object sender, EventArgs e)
        {
            //dtpTDate.Text = dtpFDate.Text.Trim();
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread s = (FpSpread)sender;
            
           
            if (e.Row < 0 || e.Column < 0) return;

            btnSave1.Enabled = true;


        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread s = (FpSpread)sender;
            
            screen_clear("1");

            if (e.Row < 0 || e.Column < 0) return;
                        
            txtPano.Text = s.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmOgSch.Pano].Text.Trim();
            txtRemark.Text = s.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmOgSch.Remark].Text.Trim();

            //2018-06-20 안정수, 김재훈계장 요청으로, 더블클릭시 팝업창으로 날짜 및 시간 변경 가능하도록 보완
            if(e.Column == (int)clsComSupSpd.enmOgSch.RDate || e.Column == (int)clsComSupSpd.enmOgSch.RTime)
            {
                #region 메뉴 더블클릭시 델리게이트용 폼 팝업

                string[] argfrm = new string[Enum.GetValues(typeof(clsComSupFnEx.enmfrmComSupFnExPOP01)).Length];
                argfrm[(int)clsComSupFnEx.enmfrmComSupFnExPOP01.Part] = "Xray";
                argfrm[(int)clsComSupFnEx.enmfrmComSupFnExPOP01.STS] = "DClick";
                argfrm[(int)clsComSupFnEx.enmfrmComSupFnExPOP01.RDate] = s.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmOgSch.RDate].Text.Trim();
                argfrm[(int)clsComSupFnEx.enmfrmComSupFnExPOP01.RTime] = s.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmOgSch.RTime].Text.Trim();

                if (frmComSupFnExPOP01x == null)
                {
                    frmComSupFnExPOP01x = new frmComSupFnExPOP01(argfrm);
                    frmComSupFnExPOP01x.rSendMsg += new frmComSupFnExPOP01.SendMsg(frmComSupFnExPOP01x_SendMsg);
                }
                else
                {
                    frmComSupFnExPOP01x = null;
                    frmComSupFnExPOP01x = new frmComSupFnExPOP01(argfrm);
                    frmComSupFnExPOP01x.rSendMsg += new frmComSupFnExPOP01.SendMsg(frmComSupFnExPOP01x_SendMsg);
                }

                #endregion

                frmComSupFnExPOP01x.ShowDialog();
            }
        } 

        void eSpreadChange(object sender , ChangeEventArgs e)
        {
            ssList.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmOgSch.Change].Text = "Y";
        }

        void sSpreadEnter(object sender,EnterCellEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            methodSpd.setEnterKey(s, clsSpread.enmSpdEnterKey.Right);

        }

        void eSpreadBtnClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;
            
            if (s.ActiveSheet.Cells[e.Row, (int)clsComSupSpd.enmOgSch.Chk].Text == "True")
            {
                s.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightPink;
            }
            else
            {
                s.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.White;
            }
            
        }

        void frmComSupFnExPOP01x_SendMsg(string[] str)
        {
            if (str[(int)clsComSupFnEx.enmRDateTime.OK] == "OK" && str[(int)clsComSupFnEx.enmRDateTime.Part] == "Xray")
            {
                if (str[(int)clsComSupFnEx.enmRDateTime.STS] == "DClick") //시트에 값변경
                {
                    ssList.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmOgSch.RDate].Text = str[(int)clsComSupFnEx.enmRDateTime.RDate];
                    ssList.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmOgSch.RTime].Text = str[(int)clsComSupFnEx.enmRDateTime.RTime];

                    
                    ssList.ActiveSheet.Rows.Get(ssList.ActiveSheet.ActiveRowIndex).BackColor = System.Drawing.Color.LightGreen;
                    ssList.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsComSupSpd.enmOgSch.Change].Text = "Y";

                }
            }
            else
            {
                //ComFunc.MsgBox("예약변경 안됨!!");
            }
        }

        void eSave(PsmhDb pDbCon, string Job)
        {

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            int i = 0;

            if (Job == "삭제")
            {                
                if (ComFunc.MsgBoxQ("선택한 대상을 " + Job + " 하시겠습니까 ??", Job + "처리", MessageBoxDefaultButton.Button2) == DialogResult.No) return;
            }                

            read_sysdate();

            //// clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            try
            {
                if (Job == "저장")
                {
                    for (i = 0; i <= ssList_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                    {
                        #region 저장 루틴

                        #region class 변수 생성 및 값세팅
                        cEtcSchOG = new clsComSupFnExSQL.c_ETC_SCHEDULE_OG();

                        cEtcSchOG.Pano = ComFunc.SetAutoZero(ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.Pano].Text.Trim(), ComNum.LENPTNO);
                        cEtcSchOG.strChange = ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.Change].Text.Trim();
                        cEtcSchOG.RDate = ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.RDate].Text.Trim();
                        cEtcSchOG.RTime = ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.RTime].Text.Trim();
                        cEtcSchOG.Jong = ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.Jong].Text.Trim();
                        cEtcSchOG.Remark = ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.Remark].Text.Trim();
                        cEtcSchOG.DeptCode = "OG";
                        cEtcSchOG.Sabun = clsPublic.GnJobSabun;

                        cEtcSchOG.ROWID = ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.ROWID].Text.Trim();

                        #endregion


                        if (cEtcSchOG.strChange == "Y")
                        {

                            if (cEtcSchOG.Pano == "" || cEtcSchOG.RDate == "" || cEtcSchOG.RTime == "" || cEtcSchOG.Jong == "")
                            {
                                ComFunc.MsgBox("저장시 필요한 데이타[등록번호,예약일시,구분] 값중 공백이 있습니다", "확인");
                                clsDB.setRollbackTran(pDbCon);
                                return;
                            }

                            if (cEtcSchOG.RTime.Length != 5)
                            {
                                ComFunc.MsgBox("예약시간형식오류 !!  (예13:00)", "시간확인");
                                clsDB.setRollbackTran(pDbCon);
                                return;
                            }

                            if (cEtcSchOG.ROWID != "")
                            {
                                SqlErr = fnexSql.up_ETC_SCHEDULE_OG(pDbCon, cEtcSchOG, ref intRowAffected);
                            }
                            else
                            {
                                SqlErr = fnexSql.ins_ETC_SCHEDULE_OG(pDbCon, cEtcSchOG, ref intRowAffected);
                            }

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                return;
                            }
                        }

                        #endregion
                    }

                }
                else if (Job == "삭제")
                {
                    for (i = 0; i <= ssList_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                    {
                        #region 삭제 루틴

                        if (ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.Chk].Text.Trim() == "True")
                        {
                            cEtcSchOG = new clsComSupFnExSQL.c_ETC_SCHEDULE_OG();

                            cEtcSchOG.ROWID = ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.ROWID].Text.Trim();

                            if (cEtcSchOG.ROWID != "")
                            {
                                SqlErr = fnexSql.del_ETC_SCHEDULE_OG(pDbCon, cEtcSchOG, ref intRowAffected);
                            }

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                return;
                            }
                        }

                        #endregion
                    }
                }
                else if (Job == "선택수동예약")
                {
                    for (i = 0; i <= ssList_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
                    {
                        if (ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.Chk].Text.Trim() == "True" && ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.Jong].Text.Trim() == "1") //MRI
                        {

                            #region 선택수동예약 루틴

                            #region 변수 관련 클래스생성 및 변수 값 세팅

                            cXrayDetail = new clsComSupXraySQL.cXrayDetail();

                            cXrayDetail.Job = "01";
                            cXrayDetail.Pano = "00000300";
                            cXrayDetail.DrRemark = ComFunc.SetAutoZero(ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.Pano].Text.Trim(), ComNum.LENPTNO);

                            cXrayDetail.IPDOPD = "O";
                            cXrayDetail.GbReserved = "6";
                            cXrayDetail.SeekDate = ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.RDate].Text.Trim() + " " + ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.RTime].Text.Trim();

                            cXrayDetail.Age = 10;
                            cXrayDetail.DeptCode = "R6";
                            cXrayDetail.DrCode = "8101";
                            cXrayDetail.Exid = 0;
                            cXrayDetail.XJong = "5";
                            if(ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.Part].Text.Trim() == "pelvic")
                            {
                                cXrayDetail.XSubCode = "04";
                                cXrayDetail.XCode = "MR62C";
                                cXrayDetail.QTY = 1;
                                cXrayDetail.Remark = "Contrast";
                                cXrayDetail.XrayRoom = "M";
                                cXrayDetail.OrderCode = "00720300";
                                cXrayDetail.OrderName = "MRI Pelvis";
                            }
                            else if (ssList_Sheet1.Cells[i, (int)clsComSupSpd.enmOgSch.Part].Text.Trim() == "chest")
                            {
                                cXrayDetail.XSubCode = "03";
                                cXrayDetail.XCode = "MR604";
                                cXrayDetail.QTY = 1;
                                cXrayDetail.Remark = "Contrast";
                                cXrayDetail.XrayRoom = "M";
                                cXrayDetail.OrderCode = "00720831";
                                cXrayDetail.OrderName = "MRI Chest";
                            }
                           
                            cXrayDetail.BDate = cpublic.strSysDate;
                            cXrayDetail.RDate = cXrayDetail.SeekDate;
                            cXrayDetail.GBSTS = "2";
                            cXrayDetail.GB_Manual = "Y";

                            //환자정보 체크
                            cBasPatient = sup.sel_Bas_Patient_cls(clsDB.DbCon,cXrayDetail.DrRemark);
                            cXrayDetail.Sex = cBasPatient.Sex;
                            cXrayDetail.SName = cBasPatient.SName;

                            #endregion

                            if (cXrayDetail.SName == "")
                            {
                                ComFunc.MsgBox("등록번호 오류!! 수동접수 안됨");
                                return;
                            }
                            else
                            {
                                dt = cSQL.sel_XrayDetail(pDbCon, cXrayDetail);
                                if (ComFunc.isDataTableNull(dt) == false)
                                {
                                    cXrayDetail.ROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                                }

                                if (cXrayDetail.ROWID != "")
                                {
                                    SqlErr = cSQL.up_Xray_Detail(pDbCon, cXrayDetail, ref intRowAffected);
                                }
                                else
                                {
                                    SqlErr = cSQL.ins_Xray_Detail(pDbCon, cXrayDetail, ref intRowAffected);
                                }

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(pDbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                    return;
                                }

                            }

                            #endregion

                        }
                    }
                }

                if (SqlErr == "")
                {
                    clsDB.setCommitTran(pDbCon);
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }            
                   
            screen_clear();

            //조회            
            screen_display();

        }

        void setSpd(FarPoint.Win.Spread.FpSpread Spd)
        {
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();

            textCellType1.MaxLength = 8;
            Spd.ActiveSheet.Columns.Get((int)clsComSupSpd.enmOgSch.Pano).CellType = textCellType1;
            textCellType2.MaxLength = 5;
            Spd.ActiveSheet.Columns.Get((int)clsComSupSpd.enmOgSch.RTime).CellType = textCellType2;
            textCellType3.MaxLength = 50;
            textCellType3.Multiline = false;
            Spd.ActiveSheet.Columns.Get((int)clsComSupSpd.enmOgSch.Remark).CellType = textCellType3;

        }             
               
        void screen_clear(string Job = "")
        {
            read_sysdate();

            if(Job =="")  btnSave1.Enabled = false;

            txtPano.Text = "";
            txtRemark.Text = "";
         

        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argFDate, string argTDate)
        {
            int i = 0;
            DataTable dt = null;

            string strJong = "0";
            if (optGubun1.Checked ==true)
            {
                strJong = "1";
            }
            else if (optGubun2.Checked == true)
            {
                strJong = "2";
            }

            read_sysdate();

            screen_clear();

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            dt = fnexSql.sel_ETC_SCHEDULE_OG(pDbCon, strJong,argFDate, argTDate);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {

                Spd.ActiveSheet.RowCount = dt.Rows.Count+5;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmOgSch.Chk].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmOgSch.Change].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmOgSch.Pano].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmOgSch.RDate].Text = dt.Rows[i]["SCHDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmOgSch.RTime].Text = dt.Rows[i]["STime"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmOgSch.Remark].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmOgSch.Jong].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmOgSch.Part].Text = "pelvic";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupSpd.enmOgSch.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }
            }

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.Default;

            #endregion


        }

        void screen_display()
        {
            GetData(clsDB.DbCon,ssList, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

    }
}
