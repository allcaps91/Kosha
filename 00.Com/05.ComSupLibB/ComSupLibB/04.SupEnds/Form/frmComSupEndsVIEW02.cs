using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using ComSupLibB.SupXray;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupEndsVIEW02.cs
    /// Description     : 내시경관리 주사오더 리스트 조회
    /// Author          : 윤조연
    /// Create Date     : 2017-09-07
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 내시경 예약메인의 시트를 폼으로 추가 생성
    /// </history>
    /// <seealso cref= " >> frmComSupEndsVIEW02.cs 폼이름 재정의" />
    public partial class frmComSupEndsVIEW02 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSupEndsSQL cendsSql = new clsComSupEndsSQL();
        clsComSupEndsSpd cendSpd = new clsComSupEndsSpd();
        clsComSupXray cxray = new clsComSupXray();

        clsComSupEndsSQL.cEndo_JupMst cEndo_JupMst = null;
        clsComSupEndsSQL.cEndoHyangCnt cEndoHyangCnt = null;
        clsComSupSpd.cComSupPRT_ENDOBar cEndoBar = null;


        bool show = true;
        string gROWID = "";
        string gTab = "";
        string gJob = "";
        string gdrname = "";

        #endregion

        public frmComSupEndsVIEW02(bool bShow =true)
        {
            InitializeComponent();
            show = bShow;
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;


        }
        
        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);

            this.btnSearch1.Click += new EventHandler(eBtnSearch);

            this.btnSave.Click += new EventHandler(eBtnSave);
            this.btnDelete.Click += new EventHandler(eBtnSave);

            this.btnPrint.Click += new EventHandler(eBtnPrint);
            this.btnPrint2.Click += new EventHandler(eBtnPrint);
            this.btnPrint3.Click += new EventHandler(eBtnPrint);
            this.btnPrint4.Click += new EventHandler(eBtnPrint);
            this.btnPrint5.Click += new EventHandler(eBtnPrint);

            this.ssHyang1.EditChange += new EditorNotifyEventHandler(eSpreadEditChange);
            this.ssHyang1.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);
            this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            this.ssJusa.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            this.ssHyang1.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            this.ssHyang2.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);


        }

        void setTxtTip()
        {
            //툴팁
            ssList.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
            ssList.TextTipDelay = 500;
            ssJusa.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
            ssJusa.TextTipDelay = 500;
            ssHyang1.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
            ssHyang1.TextTipDelay = 500;
            ssHyang2.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
            ssHyang2.TextTipDelay = 500;

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                //
                cendSpd.sSpd_enmSupEndsJusa01(ssList, cendSpd.sSpdenmSupEndsJusa01, cendSpd.nSpdenmSupEndsJusa01, 1, 0);
                cendSpd.sSpd_enmSupEndsJusa02(ssJusa, cendSpd.sSpdenmSupEndsJusa02, cendSpd.nSpdenmSupEndsJusa02, 5, 0);
                cendSpd.sSpd_enmSupEndsJusa03(ssHyang1, cendSpd.sSpdenmSupEndsJusa03, cendSpd.nSpdenmSupEndsJusa03, 5, 0);
                cendSpd.sSpd_enmSupEndsJusa04(ssHyang2, cendSpd.sSpdenmSupEndsJusa04, cendSpd.nSpdenmSupEndsJusa04, 5, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                if (show == false)
                {
                    panheader4.Visible = false;
                    panel1.Visible = false;
                    line1.Visible = false;
                }

                Tab.ControlBox.Visible = false; //컨트롤 박스 단축키 없기

                //툴팁
                setTxtTip();

                setCtrlData();
                          
                screen_clear();

                //
                //setSpd(ssList, sSpdTelview, nSpdTelview, 1, 0);
            }
        }
        
        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch1)
            {
                //조회
                screen_display( "", txtROWID.Text);

            }
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
            {
                eSave(clsDB.DbCon);
            }
            else if (sender == this.btnDelete)
            {
                eDel(clsDB.DbCon);
            }
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                ePrint();
            }
            else if (sender == this.btnPrint2)
            {
                ePrint_Bar(ssList, ssHyang1, gdrname);

                //2019-12-16 안정수, 내시경실 이지원 요청으로
                //                   전체출력시 5DWA 출력되도록 추가
                ePrint_Bar_5DWA(clsDB.DbCon, ssList, ssHyang1, "", gdrname); 
            }
            else if (sender == this.btnPrint3)
            {
                ePrint_Bar_5DWA(clsDB.DbCon, ssList, ssHyang1, "", gdrname);
            }
            else if (sender == this.btnPrint4)
            {
                ePrint_Bar_5DWA(clsDB.DbCon, ssList, ssHyang1, "1", gdrname);
            }
            else if (sender == this.btnPrint5)
            {
                ePrint_Bar_Jusa(clsDB.DbCon, ssJusa, "", gdrname);
            }
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eSpreadEditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            double nCont = 0;
            double nCnt = 0;
            double nTemp = 0;

            if (sender == this.ssHyang1)
            {
                if (e.Column == (int)clsComSupEndsSpd.enmSupEndsJusa03.Use || e.Column == (int)clsComSupEndsSpd.enmSupEndsJusa03.OrderCnt)
                {
                    if (ssHyang1.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsJusa03.DrugUnit].Text!="")
                    {
                        nCont = Convert.ToDouble(ssHyang1.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsJusa03.DrugUnit].Text);
                    }
                    if (ssHyang1.ActiveSheet.Cells[e.Row, e.Column].Text !="")
                    {
                        nCnt = Convert.ToDouble(ssHyang1.ActiveSheet.Cells[e.Row, e.Column].Text);
                    }                    

                    ssHyang1.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsJusa03.Change].Text = "Y";

                    if (e.Column == (int)clsComSupEndsSpd.enmSupEndsJusa03.Use)
                    {
                        //nTemp = nCnt / nCont;
                        //2019-05-08 안정수, 이지원s 요청으로 소수 둘째자리까지 표기하고 나머지 버림
                        nTemp = Math.Truncate((nCnt / nCont) * 100) / 100;
                        ssHyang1.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsJusa03.OrderCnt].Text = nTemp.ToString();

                    }
                    else if (e.Column == (int)clsComSupEndsSpd.enmSupEndsJusa03.OrderCnt)
                    {
                        //nTemp = nCnt / nCont;
                        nTemp = Math.Truncate((nCnt / nCont) * 100) / 100;
                        ssHyang1.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsJusa03.Use].Text = nTemp.ToString();
                    }

                }
                
            }

            

        }

        void eSpreadButtonClick(object sender, EditorNotifyEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            if (e.Column == (int)clsComSupEndsSpd.enmSupEndsJusa03.PRT)
            {
                cEndoBar = new clsComSupSpd.cComSupPRT_ENDOBar();

                cEndoBar.strPano = ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Pano].Text.Trim();
                cEndoBar.strSName = ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.SName].Text.Trim();
                cEndoBar.strSexAge = ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Sex].Text.Trim();

                cEndoBar.strSuCode = s.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsJusa03.SuCode].Text.Trim();
                cEndoBar.strSuName = s.ActiveSheet.Cells[e.Row, (int)clsComSupEndsSpd.enmSupEndsJusa03.SuName].Text.Trim(); 

                //2019-11-26 안정수 추가 
                if(ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.RDrCode].Text.Trim() == "종합건진")
                {
                    cEndoBar.strDrName = "TO";
                }
                else if (ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.RDrCode].Text.Trim() == "신체검사")
                {
                    cEndoBar.strDrName = "HR";
                }
                else
                {
                    cEndoBar.strDrName = "MG";
                }

                frmComSupPRT01 f = new frmComSupPRT01(clsComSupSpd.enmPrtType.ENDO_BAR, "혈액환자정보", cEndoBar, gdrname);

            }
        }

        void eTxtTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            if (o.ActiveSheet.RowCount <= 0)
            {
                return;
            }

            if (e.RowHeader == true || e.Column < 1)
            {
                return;
            }
            
            e.TipText = o.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
            e.ShowTip = true;
        }

        void eSave(PsmhDb pDbCon,int argRow = -1)
        {
            int i = 0;
            int nstartRow = 0;
            int nLastRow = -1;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            read_sysdate();

            if (argRow == -1)
            {
                nLastRow = ssHyang1.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;
            }
            else
            {
                nstartRow = argRow;
                nLastRow = argRow + 1;
            }
                     

            try
            {
                clsDB.setBeginTran(pDbCon);

                for (i = nstartRow; i < nLastRow; i++)
                {
                    //변경사항 있는건만
                    if (ssHyang1.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.Change].Text.Trim() == "Y" || argRow != -1)
                    {
                        //제외아닌것만
                        if (ssHyang1.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.Del].Text.Trim() == "")
                        {
                            #region //클래스 초기화 및 변수세팅

                            cEndoHyangCnt = new clsComSupEndsSQL.cEndoHyangCnt();
                            cEndoHyangCnt.Ptno = ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Pano].Text.Trim();
                            cEndoHyangCnt.JDate = ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.JDate].Text.Trim();
                            cEndoHyangCnt.RDate = ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.RDate].Text.Trim();
                            cEndoHyangCnt.DrCode = ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.RDrCode].Text.Trim();
                            cEndoHyangCnt.Buse = ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Buse].Text.Trim();

                            cEndoHyangCnt.Change = ssHyang1.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.Change].Text.Trim();
                            cEndoHyangCnt.OrderCode = ssHyang1.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.SuCode].Text.Trim();
                            cEndoHyangCnt.Content = Convert.ToDouble(ssHyang1.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.Use].Text.Trim());
                            cEndoHyangCnt.Cnt = Convert.ToDouble(ssHyang1.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.OrderCnt].Text.Trim());
                            cEndoHyangCnt.OrderNo = Convert.ToInt32(ssHyang1.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.OrderNo].Text.Trim());
                            cEndoHyangCnt.Del = ssHyang1.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.Del].Text.Trim();
                            cEndoHyangCnt.ROWID = ssHyang1.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.ROWID2].Text.Trim();

                            #endregion

                            //건수있는건만
                            if (cEndoHyangCnt.OrderCode != "" && cEndoHyangCnt.Cnt > 0)
                            {
                                if (cEndoHyangCnt.ROWID != "")
                                {
                                    SqlErr = cendsSql.up_ENDO_HYANG_CNT(pDbCon, cEndoHyangCnt, ref intRowAffected);
                                }
                                else
                                {
                                    SqlErr = cendsSql.ins_ENDO_HYANG_CNT(pDbCon, cEndoHyangCnt, ref intRowAffected);
                                }

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
                    }
                }


                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);


                screen_display_one(pDbCon, gTab, gJob, gROWID);

            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            


        }

        void eDel(PsmhDb pDbCon, int argRow = -1)
        {
            int i = 0;
            int nstartRow = 0;
            int nLastRow = -1;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            read_sysdate();

            if (argRow == -1)
            {
                nLastRow = ssHyang2.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;
            }
            else
            {
                nstartRow = argRow;
                nLastRow = argRow + 1;
            }
                        
            try
            {
                clsDB.setBeginTran(pDbCon);

                for (i = nstartRow; i < nLastRow; i++)
                {
                    //제외아닌것만
                    if (ssHyang2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa04.CHK].Text.Trim() == "True")
                    {
                        #region //클래스 초기화 및 변수세팅

                        cEndoHyangCnt = new clsComSupEndsSQL.cEndoHyangCnt();
                        cEndoHyangCnt.ROWID = ssHyang2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa04.ROWID].Text.Trim();

                        #endregion


                        if (cEndoHyangCnt.ROWID != "")
                        {
                            SqlErr = cendsSql.del_ENDO_HYANG_CNT(pDbCon, cEndoHyangCnt, ref intRowAffected);
                        }

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


                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);

                screen_display_one(pDbCon, gTab, gJob, gROWID);
            }
            catch(Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            

        }

        void ePrint()
        {
            //clsSpread SPR = new clsSpread();
            //clsSpread.SpdPrint_Margin setMargin;
            //clsSpread.SpdPrint_Option setOption;

            //string strTitle = "";
            //string strHeader = "";
            //string strFooter = "";
            //bool PrePrint = true;

            //strTitle = "전화통보 LIST " + "(" + dtpFDate.Text.Replace("-", "") + " - " + dtpTDate.Text.Replace("-", "") + ")";

            //strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            //setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            //SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void ePrint_Bar(FpSpread Spd, FpSpread Spd2, string gdrname = "")
        {            
            for (int i = 0; i < Spd2.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1; i++)
            {
                if (Spd2.ActiveSheet.Cells[i,(int)clsComSupEndsSpd.enmSupEndsJusa03.SuCode].Text !="" )
                {
                    cEndoBar = new clsComSupSpd.cComSupPRT_ENDOBar();

                    cEndoBar.strPano = Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Pano].Text.Trim();
                    cEndoBar.strSName = Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.SName].Text.Trim();
                    cEndoBar.strSexAge = Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Sex].Text.Trim();

                    cEndoBar.strSuCode = Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.SuCode].Text.Trim();
                    cEndoBar.strSuName = Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.SuName].Text.Trim();

                    //2019-12-11 안정수 추가
                    if (cEndoBar.strDrName == "")
                    {
                        if (ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.RDrCode].Text.Trim() == "종합건진")
                        {
                            cEndoBar.strDrName = "TO";
                        }
                        else if (ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.RDrCode].Text.Trim() == "신체검사")
                        {
                            cEndoBar.strDrName = "HR";
                        }
                        else
                        {
                            cEndoBar.strDrName = "MG";
                        }
                    }

                    frmComSupPRT01 f = new frmComSupPRT01(clsComSupSpd.enmPrtType.ENDO_BAR, "혈액환자정보", cEndoBar, gdrname);
                }
            }
        }

        void ePrint_Bar_5DWA(PsmhDb pDbCon, FpSpread Spd, FpSpread Spd2, string argJob = "", string gdrname = "")
        {           
            cEndoBar = new clsComSupSpd.cComSupPRT_ENDOBar();
          
            cEndoBar.strPano = Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Pano].Text.Trim();
            cEndoBar.strSName = Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.SName].Text.Trim();
            cEndoBar.strSexAge = Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Sex].Text.Trim();
               

            if (argJob == "")
            {
                cEndoBar.strSuCode = "5DWA";
            }
            else if (argJob == "1")
            {
                cEndoBar.strSuCode = "NSB";
            }         

            cEndoBar.strSuName = clsVbfunc.READ_SugaName(pDbCon, cEndoBar.strSuCode);

            //2019-12-06 안정수 추가
            if (ssList.ActiveSheet.Rows.Count > 0)
            {
                if (ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.RDrCode].Text.Trim() == "종합건진")
                {
                    cEndoBar.strDrName = "TO";
                }
                else if (ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.RDrCode].Text.Trim() == "신체검사")
                {
                    cEndoBar.strDrName = "HR";
                }
                else
                {
                    cEndoBar.strDrName = "MG";
                }
            }

            frmComSupPRT01 f = new frmComSupPRT01(clsComSupSpd.enmPrtType.ENDO_BAR, "혈액환자정보", cEndoBar, gdrname);
           
        }

        void ePrint_Bar_Jusa(PsmhDb pDbCon, FpSpread Spd, string argJob = "" , string gdrname = "")
        {
            if (Spd.ActiveSheet.Rows.Count > 0) 
            {
                cEndoBar = new clsComSupSpd.cComSupPRT_ENDOBar();

                for (int i = 0; i < ssJusa.ActiveSheet.Rows.Count; i++)
                {
                    if (Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa02.CHK].Text.Trim() == "True")
                    {
                        cEndoBar.strPano = ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Pano].Text.Trim();
                        cEndoBar.strSName = ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.SName].Text.Trim();
                        cEndoBar.strSexAge = ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Sex].Text.Trim();

                        cEndoBar.strSuCode = Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa02.SuCode2].Text.Trim();

                        cEndoBar.strSuName = clsVbfunc.READ_SugaName(pDbCon, cEndoBar.strSuCode);
                        cEndoBar.strType = clsComSup.setP(Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa02.DosCode].Text.Trim(), "/", 1);

                        //2019-11-26 안정수 추가 
                        if (ssList.ActiveSheet.Rows.Count > 0)
                        {
                            if (ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.RDrCode].Text.Trim() == "종합건진")
                            {
                                cEndoBar.strDrName = "TO";
                            }
                            else if (ssList.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.RDrCode].Text.Trim() == "신체검사")
                            {
                                cEndoBar.strDrName = "HR";
                            }
                            else
                            {
                                cEndoBar.strDrName = "MG";
                            }
                        }

                        frmComSupPRT01 f = new frmComSupPRT01(clsComSupSpd.enmPrtType.ENDO_BAR, "혈액환자정보", cEndoBar, gdrname);                        
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa02.CHK].Text = "False";
                    }
                }

                //Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa02.CHK, Spd.ActiveSheet.Rows.Count - 1, (int)clsComSupEndsSpd.enmSupEndsJusa02.CHK].Text = "False";
            }

        }

        void screen_clear()
        {
            //
            read_sysdate();

            ssList.ActiveSheet.RowCount = 0;
            ssJusa.ActiveSheet.RowCount = 0;
            ssHyang1.ActiveSheet.RowCount = 0;
            ssHyang2.ActiveSheet.RowCount = 0;

            btnSave.Enabled = true;
            btnDelete.Enabled = true;

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void screen_display(string argJob,string argROWID)
        {
            screen_clear();

            GetData(clsDB.DbCon,ssList, argJob, argROWID);            
        }

        public void screen_display_one(PsmhDb pDbCon, string argTab, string argJob, string argROWID, string strdrname = "")
        {
            gTab = argTab;
            gJob = argJob;
            gROWID = argROWID;
            gdrname = strdrname;
            screen_clear();

            if (argROWID !="" )
            {
                GetData(pDbCon,ssList, argJob, argROWID);
            }
            
            if (argTab =="A")
            {
                this.Tab.SelectedTabIndex = 0; 
            }
            else
            {
                this.Tab.SelectedTabIndex = 1;
            }                                    

        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argJob, string argROWID)
        {
            int i = 0;
            long SEQNO = 0;
            string strIO = "";

            string gPtno = "";
            string gRdate = "";
            string gOrdercode = "";

            DataTable dt = null;

            #region //환자기본체크

            cEndo_JupMst = new clsComSupEndsSQL.cEndo_JupMst();

            Spd.ActiveSheet.RowCount = 0;            
            dt = cendsSql.sel_ENDO_JUPMST(pDbCon, argROWID, "", "", "");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                cEndo_JupMst.Ptno = dt.Rows[i]["Ptno"].ToString().Trim();
                cEndo_JupMst.BDate = dt.Rows[i]["BDate"].ToString().Trim();
                cEndo_JupMst.JDate = dt.Rows[i]["JDate"].ToString().Trim();
                cEndo_JupMst.RDate = dt.Rows[i]["RDate"].ToString().Trim();
                cEndo_JupMst.Dept = dt.Rows[i]["DeptCode"].ToString().Trim();
                cEndo_JupMst.ResultDate = dt.Rows[i]["ResultDate2"].ToString().Trim();

                gPtno = dt.Rows[i]["Ptno"].ToString().Trim();
                gRdate = dt.Rows[i]["RDATE"].ToString().Trim();
                gOrdercode = dt.Rows[i]["Ordercode"].ToString().Trim();

                if (dt.Rows[i]["Res"].ToString().Trim() =="1")
                {
                    btnSave.Enabled = false;
                    btnDelete.Enabled = false;
                }

                if (dt.Rows[0]["SEQNO"].ToString().Trim() !="")
                {
                    SEQNO = Convert.ToInt32(dt.Rows[0]["SEQNO"].ToString().Trim());
                }             
                
                strIO = dt.Rows[0]["GbIO"].ToString().Trim();

                Spd.ActiveSheet.RowCount = 1;
                Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Pano].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                if (dt.Rows[i]["RoomCode"].ToString().Trim() !="")
                {
                    Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Sex].Text = dt.Rows[i]["Sex"].ToString().Trim() + "/" + dt.Rows[i]["FC_Age"].ToString().Trim() +"("+ dt.Rows[i]["RoomCode"].ToString().Trim() + ")";
                }
                else
                {
                    Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Sex].Text = dt.Rows[i]["Sex"].ToString().Trim() + "/" + dt.Rows[i]["FC_Age"].ToString().Trim();
                }
                
                Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.OrderCode].Text = dt.Rows[i]["OrderCode"].ToString().Trim();
                Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.OrderName].Text = cxray.OCS_XNAME_READ(pDbCon, dt.Rows[i]["OrderCode"].ToString().Trim(),false);
                Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.JDate].Text = dt.Rows[i]["JDate"].ToString().Trim();
                Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.RDate].Text = dt.Rows[i]["RDate"].ToString().Trim();
                Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.RDrCode].Text = clsVbfunc.GetBASDoctorName(pDbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Buse].Text = dt.Rows[i]["Buse"].ToString().Trim() + "." + dt.Rows[i]["FC_BuseName"].ToString().Trim();
                Spd.ActiveSheet.Cells[0, (int)clsComSupEndsSpd.enmSupEndsJusa01.Orderno].Text = dt.Rows[i]["Orderno"].ToString().Trim();


            }
            else
            {
                return;
            }

            #endregion
            
            GetData2(pDbCon, ssJusa, argJob, strIO, argROWID, SEQNO, gPtno, gRdate, gOrdercode);
            GetData3(pDbCon, ssHyang1, ssHyang2, cEndo_JupMst);


        }

        void GetData2(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argJob, string argIO, string argROWID,long argSEQNO, string gPtno, string gRdate, string gOrdercode)
        {
            int i = 0;
            int nRow = 0;                    
            string strWheres = "";
            DataTable dt = null;
                        

            if (argJob == "2") //미접수
            {
                if (argIO =="O")
                {
                    strWheres = "  AND A.GbSuNap  = '1' \r\n AND RTrim(A.DosCode) LIKE '9%3' ";
                }
                else
                {
                    strWheres = "  AND RTrim(A.DosCode) LIKE '9%3' ";
                }
                
                dt = cendsSql.sel_Endo_JupMst_Order(pDbCon, argIO, argROWID, "", strWheres);

                if (dt.Rows.Count > 0)
                {
                    Spd.ActiveSheet.RowCount += dt.Rows.Count;
                    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.SuCode].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.OrderCode].Text = dt.Rows[i]["OrderCode"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.OrderName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.DosCode].Text = dt.Rows[i]["DosName"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.Qty].Text = dt.Rows[i]["Qty"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.Nal].Text = dt.Rows[i]["Nal"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.Remark].Text = dt.Rows[i]["Remark"].ToString().Trim();

                        nRow++;

                    }
                }


                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            else
            {
                //쿼리실행                      
                if (argSEQNO > 0)
                {
                    //추가오더 체크
                    if (argIO == "O")
                    {
                        strWheres = "  AND A.GbSuNap  = '1' \r\n AND d.dosfullcode like '%내시경%' ";
                    }
                    else
                    {
                        strWheres = " AND A.GbSend <> '*' \r\n AND A.GBPRN = ' ' \r\n  AND d.wardcode ='EN' ";
                    }

                    dt = cendsSql.sel_Endo_JupMst_Order(pDbCon, argIO, argROWID, "", strWheres);

                    if (dt.Rows.Count > 0)
                    {
                        Spd.ActiveSheet.RowCount = dt.Rows.Count;
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.SuCode].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.OrderCode].Text = dt.Rows[i]["OrderCode"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.OrderName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.DosCode].Text = dt.Rows[i]["DosName"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.Qty].Text = dt.Rows[i]["Qty"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.Nal].Text = dt.Rows[i]["Nal"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.Remark].Text = "**추가오더**" + dt.Rows[i]["Remark"].ToString().Trim();

                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.SuCode2].Text = dt.Rows[i]["SuCode2"].ToString().Trim();

                            nRow++;
                        }
                    }


                    dt.Dispose();
                    dt = null;

                    dt = cendsSql.sel_Endo_JusaMst_OrderCode( pDbCon,argSEQNO,  gPtno,  gRdate,  gOrdercode, "");

                    #region //데이터셋 읽어 자료 표시

                    if (dt == null) return;
                                        
                    if (dt.Rows.Count > 0)
                    {
                        Spd.ActiveSheet.RowCount += dt.Rows.Count;
                        Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.STS].Text = dt.Rows[i]["STS"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.SuCode].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.SuCode2].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.OrderCode].Text = dt.Rows[i]["OrderCode"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.OrderName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.DosCode].Text = dt.Rows[i]["DosName"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.Qty].Text = dt.Rows[i]["Qty"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.Nal].Text = dt.Rows[i]["Nal"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.Remark].Text = dt.Rows[i]["Remark"].ToString().Trim();
                            Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                            nRow++;

                        }
                    }

                    //2019-05-13 안정수, 이지원s 요청으로 추가오더부분 상단표기함 
                    ////추가오더 체크
                    //if (argIO == "O")
                    //{
                    //    strWheres = "  AND A.GbSuNap  = '1' \r\n AND d.dosfullcode like '%내시경%' ";
                    //}
                    //else
                    //{
                    //    strWheres = " AND A.GbSend <> '*' \r\n AND A.GBPRN = ' ' \r\n  AND d.wardcode ='EN' ";
                    //}

                    //dt = cendsSql.sel_Endo_JupMst_Order(pDbCon, argIO, argROWID, "", strWheres);

                    //if (dt.Rows.Count > 0)
                    //{                        
                    //    Spd.ActiveSheet.RowCount += dt.Rows.Count;
                    //    Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                    //    for (i = 0; i < dt.Rows.Count; i++)
                    //    {
                    //        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.SuCode].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                    //        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.OrderCode].Text = dt.Rows[i]["OrderCode"].ToString().Trim();
                    //        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.OrderName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                    //        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.DosCode].Text = dt.Rows[i]["DosName"].ToString().Trim();
                    //        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.Qty].Text = dt.Rows[i]["Qty"].ToString().Trim();
                    //        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.Nal].Text = dt.Rows[i]["Nal"].ToString().Trim();
                    //        Spd.ActiveSheet.Cells[nRow, (int)clsComSupEndsSpd.enmSupEndsJusa02.Remark].Text = "**추가오더**" + dt.Rows[i]["Remark"].ToString().Trim();

                    //        nRow++;

                    //    }
                    //}
                    

                    //dt.Dispose();
                    //dt = null;
                    Cursor.Current = Cursors.Default;

                    #endregion

                }
                else
                {
                    return;
                }
            }    

        }

        void GetData3(PsmhDb pDbCon,FarPoint.Win.Spread.FpSpread Spd, FarPoint.Win.Spread.FpSpread Spd2, clsComSupEndsSQL.cEndo_JupMst argCls)
        {
            int i = 0;        
            DataTable dt = null;
            DataTable dt2 = null;

            double nCont = 0;
            double nCnt = 0;
            double nTemp = 0;

            Spd.ActiveSheet.RowCount = 0;
            Spd2.ActiveSheet.RowCount = 0;
      
            dt = cendsSql.sel_ORDER_ENDO(pDbCon,argCls);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count; 

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.SuCode].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.SuName].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.DrugUnit].Text = dt.Rows[i]["UNITNEW1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.OrderNo].Text = dt.Rows[i]["OrderNo"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.ROWID1].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    dt2 = cendsSql.sel_ENDO_HYANG(pDbCon,argCls.Ptno, argCls.BDate, dt.Rows[i]["SuCode"].ToString().Trim());
                    if (ComFunc.isDataTableNull(dt2) == false)
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.STS].Text = "수정";
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.Change].Text = "";
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.Use].Text = dt2.Rows[0]["CONTENT"].ToString().Trim();
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.OrderCnt].Text = dt2.Rows[0]["CNT"].ToString().Trim();
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.DrName].Text = dt2.Rows[0]["DrCode"].ToString().Trim();
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.ROWID2].Text = dt2.Rows[0]["ROWID"].ToString().Trim();
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.STS].Text = "신규";
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.Change].Text = "Y";
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.Use].Text = (Convert.ToDouble(clsVbfunc.GetBCODENameCode(pDbCon, "1", "C#_ENDO_마약향정기본량", dt.Rows[i]["SuCode"].ToString().Trim())) * Convert.ToDouble(dt.Rows[i]["REALQTY"].ToString().Trim())).ToString();
                        Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.OrderCnt].Text = dt.Rows[i]["QTY"].ToString().Trim();

                        

                        nCont = 0;
                        nCnt = 0;
                        nTemp = 0;

                        //eSpreadEditChange(Spd,null);
                        
                        if (Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.OrderCnt].Text != "")
                        {
                            nCnt = Convert.ToDouble(Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.OrderCnt].Text);
                        }
                        
                        if (nCnt != 1 )
                        {
                            if (Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.DrugUnit].Text != "")
                            {
                                nCont = Convert.ToDouble(Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.DrugUnit].Text);
                            }

                            if (nCont > 0 && nCnt > 0)
                            {
                                nTemp = nCnt * nCont;
                                Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa03.Use].Text = nTemp.ToString();
                            }
                            
                        }       
                          
                    }
                    
                }                

            }

            //입력된 데이터
            dt = cendsSql.sel_ENDO_HYANG_CNT(pDbCon,argCls.Ptno, argCls.JDate, argCls.RDate);
            if (ComFunc.isDataTableNull(dt) == false)
            {
                Spd2.ActiveSheet.RowCount = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa04.SuCode].Text = dt.Rows[i]["OrderCode"].ToString().Trim();
                    Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa04.SuName].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa04.Use].Text = dt.Rows[i]["ENTQTY"].ToString().Trim();
                    if (dt.Rows[i]["ENTQTY"].ToString().Trim()=="")
                    {
                        Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa04.Use].Text = dt.Rows[i]["CONTENT"].ToString().Trim();
                    }
                    Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa04.OrderCnt].Text = dt.Rows[i]["CNT"].ToString().Trim();
                    Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa04.DrName].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa04.OrderNo].Text = dt.Rows[i]["OrderNo"].ToString().Trim();
                    Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa04.DrugUnit].Text = dt.Rows[i]["UNITNEW1"].ToString().Trim();
                    Spd2.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmSupEndsJusa04.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                    
                }

            }

        }
    }
}
