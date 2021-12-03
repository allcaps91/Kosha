using ComBase;
using ComDbB;
using ComSupLibB.Com;
using ComSupLibB.SupEnds;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayPRT01.cs
    /// Description     : 영상의학과 판독 결과 VIEW 및 출력
    /// Author          : 윤조연
    /// Create Date     : 2017-12-11
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 PACS view 폼에 포함되어있는것을 폼 분리하여 신규폼 생성
    /// </history>
    /// <seealso cref= " >> frmComSupXrayPRT01.cs 폼이름 재정의" />
    public partial class frmComSupXrayPRT01 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd xraySpd = new clsComSupXraySpd();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();
        clsComSupXrayRead xread = new clsComSupXrayRead();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSup sup = new clsComSup();
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();
        clsComSupEndsSQL cendsSql = new clsComSupEndsSQL();

        clsComSupXrayRead.cXrayPatient cXrayPatient = null;
        clsComSupXrayRead.cHic_Xray_Result cHic_Xray_Result = null;
        clsComSupEndsSQL.cEndoJupmst cEndoJupmst = null;

        ComFunc CF = new ComFunc();
        clsSpread spread = new clsSpread();



        //string gJob = "";
        //string gROWID = "";

        #endregion

        //public frmComSupXrayPRT01(string argJob,string argROWID)
        //{
        //    InitializeComponent();
        //    gJob = argJob;
        //    gROWID = argROWID;
        //    setEvent();
        //}

        public frmComSupXrayPRT01(clsComSupXrayRead.cXrayPatient argCls)
        {
            InitializeComponent();
            cXrayPatient = argCls;
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {

            screen_clear();

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            
            this.btnExit.Click += new EventHandler(eBtnClick);

            this.btnSearch.Click += new EventHandler(eBtnSearch);

            this.btnPrint.Click += new EventHandler(eBtnPrint);
            
            //this.txtPano.KeyDown += new KeyEventHandler(eTxtEvent);

            //this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

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
                //xraySpd.sSpd_XrayList01(ssList, xraySpd.sSpdXrayList01, xraySpd.nSpdXrayList01, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                //ComFunc.SetAllControlClear(this); //컨트롤 초기화

                screen_clear();

                setCtrlData();

                //
                screen_display();
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
            if (sender == this.btnSearch)
            {
                //
                screen_display();
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {

        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                ePrint();
            }
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            
            FpSpread o = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }
        }

        void eTxtEvent(object sender, KeyEventArgs e)
        {

        }

        void screen_display()
        {
            GetData(clsDB.DbCon,cXrayPatient.Job,cXrayPatient.ROWID);
        }

        void ePrint()
        {
            int i = 0;
            int ii = 0;            
            string strlast = "";
            string strfirst = "";
            char sresultremark;            
            string[] sResultRemark1 = null;
            sResultRemark1 = new string[1];

            strfirst = txtResult.Text;
            int nlen = strfirst.Length;

            for (ii = 1; ii < 4000; ii++)
            {
                sresultremark = VB.Mid(strfirst, ii, 1) != "" ? Convert.ToChar(VB.Mid(strfirst, ii, 1)) : ' ';
                strlast = strlast + VB.Mid(strfirst, ii, 1);

                if (sresultremark == VB.Chr(13) || strlast.Length >= 75)
                {
                    i += 1;

                    if (strlast.Length == 1 && (Convert.ToChar(strlast) == VB.Chr(13)))
                    {
                        Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                        sResultRemark1[i] = "";
                        strlast = "";
                    }
                    else if (strlast.Length >= 75 && Convert.ToChar(VB.Left(strlast, 1)) != VB.Chr(10))
                    {
                        Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                        sResultRemark1[i] = strlast;
                        strlast = "";
                    }
                    else if (strlast.Length >= 75 && Convert.ToChar(VB.Left(strlast, 1)) == VB.Chr(10))
                    {
                        Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                        sResultRemark1[i] = VB.Right(strlast, strlast.Length - 1);
                        strlast = "";
                    }
                    else if (Convert.ToChar(VB.Left(strlast, 1)) == VB.Chr(10) && i != 1)
                    {
                        strlast = VB.Right(strlast, strlast.Length - 1);
                        Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                        sResultRemark1[i] = VB.Left(strlast, strlast.Length - 1);
                        strlast = "";
                    }
                    else if (Convert.ToChar(VB.Right(strlast, 1)) == VB.Chr(13) && i != 1)
                    {
                        strlast = VB.Right(strlast, strlast.Length - 1);
                        Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                        sResultRemark1[i] = VB.Left(strlast, strlast.Length - 1);
                        strlast = "";
                    }
                    else if (i == 1)
                    {
                        Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                        sResultRemark1[i] = VB.Left(strlast, strlast.Length - 1);
                        strlast = "";
                    }
                }
                sresultremark = ' ';
            }

            i += 1;

            if (strlast != "" && (strlast.Length >= 1 && Convert.ToChar(VB.Left(strlast, 1)) == VB.Chr(13) || Convert.ToChar(VB.Left(strlast, 1)) == VB.Chr(10)))
            {
                Array.Resize<string>(ref sResultRemark1, sResultRemark1.Length + 1);
                sResultRemark1[i] = VB.Right(strlast, strlast.Length - 1);
                strlast = "";
            }
            else if (strlast.Length >= 1)
            {
                this.ssPrt.ActiveSheet.Cells[this.ssPrt.ActiveSheet.Rows.Count - 1, 0].Text = strlast.Trim();
                strlast = "";
            }

            this.ssPrt.ActiveSheet.Rows.Count += 1;
            this.ssPrt.ActiveSheet.Cells[5, 0].Value = "검사명 : " + txtXName.Text.Trim();

            this.ssPrt.ActiveSheet.Cells[2, 1].Text = cHic_Xray_Result.PTNO;
            this.ssPrt.ActiveSheet.Cells[2, 3].Text = cHic_Xray_Result.SNAME;
            
            if(cHic_Xray_Result.WardCode != "")
            {
                this.ssPrt.ActiveSheet.Cells[2, 5].Text = cHic_Xray_Result.AGE + "/" + cHic_Xray_Result.SEX + 
                                                          VB.Space(3) + cHic_Xray_Result.WardCode + "/" + cHic_Xray_Result.RoomCode;
            }
            else
            {
                this.ssPrt.ActiveSheet.Cells[2, 5].Text = cHic_Xray_Result.AGE + "/" + cHic_Xray_Result.SEX;
            }

            //의뢰과
            this.ssPrt.ActiveSheet.Cells[3, 1].Text = cHic_Xray_Result.DeptName;
            //의사
            this.ssPrt.ActiveSheet.Cells[3, 3].Text = cHic_Xray_Result.DRName; 
            //검사요청일
            this.ssPrt.ActiveSheet.Cells[3, 5].Text = VB.Left(cHic_Xray_Result.ENTTIME, 10);

            this.ssPrt.ActiveSheet.Rows.Count += 1;
            this.ssPrt.ActiveSheet.AddSpanCell(ssPrt.ActiveSheet.Rows.Count - 1, 0, ssPrt.ActiveSheet.Rows.Count - 1, 2);

            if (sResultRemark1.Length > 0)
            {
                for (int j = 0; j < sResultRemark1.Length; j++)
                {
                    this.ssPrt.ActiveSheet.Rows.Count += 1;
                    this.ssPrt.ActiveSheet.AddSpanCell(6 + j, 0, 1, this.ssPrt.ActiveSheet.Columns.Count);                    
                    if (j > 19 && VB.Left(sResultRemark1[j], 15) == "               ")
                    {
                        if (VB.Left(sResultRemark1[j], 45) == "                                             ")
                        {
                            this.ssPrt.ActiveSheet.Cells[6 + j, 0].Text = "         " + sResultRemark1[j];
                        }
                        else
                        {
                            this.ssPrt.ActiveSheet.Cells[6 + j, 0].Text = "   " + sResultRemark1[j];
                        }
                    }
                    else
                    {
                        this.ssPrt.ActiveSheet.Cells[6 + j, 0].Text = sResultRemark1[j];
                    }             
                }

                string header = string.Empty;
                string foot = string.Empty;
                char s = '"';

                clsSpread.SpdPrint_Margin margin = new clsSpread.SpdPrint_Margin(100, 50, 10, 10, 10, 10);
                clsSpread.SpdPrint_Option option = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait
                                                , PrintType.All, 0, 0, false, false, false, false, false, false, false);
                if(string.IsNullOrEmpty(cHic_Xray_Result.PANDRCode.ToString()) == false)
                {
                    if (Convert.ToInt32(cHic_Xray_Result.PANDRCode.ToString().Trim()) > 99000 && Convert.ToInt32(cHic_Xray_Result.PANDRCode.ToString().Trim()) < 99100)
                    {
                        foot = spread.setSpdPrint_String("         촬영일자 : " + cHic_Xray_Result.SeekDate + "         판독일자 : " + VB.Left(cHic_Xray_Result.READTIME1, 10) + " " + VB.Right(cHic_Xray_Result.READTIME1, 5) + "         판독의사 : " + cHic_Xray_Result.PANDRName + " (" + CF.READ_OutPanDoctorDRNO(clsDB.DbCon, cHic_Xray_Result.PANDRCode) + ")", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                    }
                    else
                    {
                        foot = spread.setSpdPrint_String("         촬영일자 : " + cHic_Xray_Result.SeekDate + "         판독일자 : " + VB.Left(cHic_Xray_Result.READTIME1, 10) + " " + VB.Right(cHic_Xray_Result.READTIME1, 5) + "         판독의사 : " + cHic_Xray_Result.PANDRName + " (" + clsVbfunc.GetOCSDoctorDRBUNHO(clsDB.DbCon, cHic_Xray_Result.PANDRCode) + ")", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                    }
                }
                else
                {
                    foot = spread.setSpdPrint_String("           촬영일자 : " + cHic_Xray_Result.SeekDate + "           판독일자 : " + VB.Left(cHic_Xray_Result.READTIME1, 10) + " " + VB.Right(cHic_Xray_Result.READTIME1, 5) + "           판독의사 : " + cHic_Xray_Result.PANDRName , new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                }
                
                foot += spread.setSpdPrint_String("         ---------------------------------------------------------------------------------------------", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("                        ※포항성모병원 영상의학과※    전화:054-260-8163 FAX:054-260-8006", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("  ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("  ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("  ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("  ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("  ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                foot += spread.setSpdPrint_String("  ", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                //header += "/r                                                                           (" + " /p " + "/" + " /pc " + ")";
                spread.setSpdPrint(this.ssPrt, false, margin, option, header, foot, Centering.Horizontal);

            }
        }

        void screen_clear()
        {
            //
            read_sysdate();

            lblOut.Visible = false;
            btnPrint.Enabled = false;

            //콘트롤 값 clear
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {

                if (ctl is TextBox)
                {
                    ctl.Text = "";
                }
                else if (ctl is CheckBox)
                {
                    ((CheckBox)ctl).Checked = false;
                }
                else if (ctl is RadioButton)
                {
                    ((RadioButton)ctl).Checked = false;
                }
                else if (ctl is DateTimePicker)
                {
                    if (((DateTimePicker)ctl).Name == "dtpDate")
                    {
                        ((DateTimePicker)ctl).Text = "";
                    }
                }
            }

            ssPrt.ActiveSheet.Cells[2, 1].Text = "";
            ssPrt.ActiveSheet.Cells[3, 1].Text = "";

            ssPrt.ActiveSheet.Cells[2, 3].Text = "";
            ssPrt.ActiveSheet.Cells[3, 3].Text = "";

            ssPrt.ActiveSheet.Cells[2, 5].Text = "";
            ssPrt.ActiveSheet.Cells[3, 5].Text = "";

            ssPrt.ActiveSheet.Cells[5, 0].Text = "";
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void GetData(PsmhDb pDbCon,string argJob,string argROWID)
        {
            
            DataTable dt = null;
            
            Cursor.Current = Cursors.WaitCursor;

            //판독 VIEW 권한체크
            if(clsPacs.ChkPacsLogin(pDbCon, clsType.User.IdNumber, "XRESULT") == false)
            {
                ComFunc.MsgBox("판독결과 조회 권한이 없습니다!!");
                return;
            }


            #region // class 초기화 , 변수 설정
            cHic_Xray_Result = new clsComSupXrayRead.cHic_Xray_Result();
            
            cHic_Xray_Result.ROWID = argROWID;
            #endregion
            
            if (argJob == "XRAY")
            {
                cHic_Xray_Result.Job = "02"; //wrtno 조건
                cHic_Xray_Result.WRTNO = cXrayPatient.WRTNO;
                dt = xread.sel_XRAY_RESULTNEW(pDbCon, cHic_Xray_Result);

                #region //데이터셋 읽어 자료 표시

                if (ComFunc.isDataTableNull(dt) == false)
                {

                    txtXCode.Text = dt.Rows[0]["XCode"].ToString().Trim();
                    txtXName.Text = cXrayPatient.XName;
                    txtPano.Text = cXrayPatient.Pano;
                    txtSName.Text = cXrayPatient.SName;

                    txtResult.Text = "";

                    //2020-01-10 안정수, 추가
                    cHic_Xray_Result.PTNO = cXrayPatient.Pano;
                    cHic_Xray_Result.SNAME = cXrayPatient.SName;
                    cHic_Xray_Result.SEX = dt.Rows[0]["Sex"].ToString().Trim();
                    cHic_Xray_Result.AGE = Convert.ToInt32(VB.Val(dt.Rows[0]["Age"].ToString().Trim()));
                    cHic_Xray_Result.WardCode = dt.Rows[0]["WardCode"].ToString().Trim();
                    cHic_Xray_Result.RoomCode = dt.Rows[0]["RoomCode"].ToString().Trim();

                    cHic_Xray_Result.DeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                    cHic_Xray_Result.DeptName = CF.READ_DEPTNAMEK(clsDB.DbCon, dt.Rows[0]["DeptCode"].ToString().Trim());
                    cHic_Xray_Result.DRCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                    cHic_Xray_Result.DRName = CF.READ_DrName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                    cHic_Xray_Result.SeekDate = dt.Rows[0]["SeekDate"].ToString().Trim();

                    cHic_Xray_Result.READDATE = dt.Rows[0]["ReadDate"].ToString().Trim();
                    cHic_Xray_Result.READTIME1 = dt.Rows[0]["READTIME"].ToString().Trim();
                    cHic_Xray_Result.ENTTIME = dt.Rows[0]["ENTDATE"].ToString().Trim();
                    cHic_Xray_Result.PANDRName = CF.Read_SabunName(clsDB.DbCon, dt.Rows[0]["XDrCode1"].ToString().Trim());
                    cHic_Xray_Result.PANDRCode = dt.Rows[0]["XDrCode1"].ToString().Trim();

                    switch (dt.Rows[0]["XJONG"].ToString().Trim())
                    {
                        case "6":
                            ssPrt.ActiveSheet.Cells[0, 0].Text = "R I Study Report";
                            break;
                        case "E":
                            ssPrt.ActiveSheet.Cells[0, 0].Text = "전기진단검사 결과지";
                            break;
                        default:
                            ssPrt.ActiveSheet.Cells[0, 0].Text = "방사선 촬영 결과지";
                            break;
                    }

                    if (dt.Rows[0]["Approve"].ToString().Trim() == "N")
                    {
                        txtResult.Text += "\r\n" + "\r\n" + "\r\n";
                        txtResult.Text += "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                    }
                    else
                    {
                        txtResult.Text += dt.Rows[0]["Result"].ToString().Trim() + dt.Rows[0]["Result1"].ToString().Trim();
                        if ((dt.Rows[0]["ADDENDUM1"].ToString().Trim() + dt.Rows[0]["ADDENDUM2"].ToString().Trim()) !="" )
                        {
                            txtResult.Text += "\r\n" + "\r\n" + "Addendum Report: " + "\r\n" + "\r\n" + dt.Rows[0]["addendum1"].ToString().Trim() + dt.Rows[0]["addendum2"].ToString().Trim();
                        }

                        //심장초음파결과 체크
                        if (dt.Rows[0]["XJong"].ToString().Trim() =="C")
                        {
                            btnPrint.Enabled = false;

                            if (clsType.User.IdNumber =="20175") //원무함종현
                            {
                                btnPrint.Enabled = true;
                            }
                        }
                        else
                        {
                            btnPrint.Enabled = true;
                        }

                        //외주판독 체크
                        if (dt.Rows[0]["XDrCode1"].ToString().Trim() != "")
                        {
                            if ( Convert.ToInt32(dt.Rows[0]["XDrCode1"].ToString().Trim()) >= 99001 && Convert.ToInt32(dt.Rows[0]["XDrCode1"].ToString().Trim()) <= 99099)
                            {
                                lblOut.Visible = true;
                                btnPrint.Enabled = true;
                            }

                        }

                        //인피니트 팍스 DB - 판독갱신
                        clsPacs.SET_XRAY_READ_UPDATE_INFINITT(pDbCon, cXrayPatient.Pano, cXrayPatient.PacsNo);

                    }

                }
                else
                {
                    ComFunc.MsgBox("판독결과가 없습니다.");
                }

                #endregion

                dt.Dispose();

                btnPrint.Enabled = true;

            }
            else if (argJob == "DRXRAY")
            {
                cHic_Xray_Result.Job = "02"; //wrtno 조건
                cHic_Xray_Result.WRTNO = cXrayPatient.WRTNO;
                dt = xread.sel_XRAY_RESULTNEW_DR(pDbCon, cHic_Xray_Result);

                #region //데이터셋 읽어 자료 표시

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    
                    txtXCode.Text = dt.Rows[0]["XCode"].ToString().Trim();
                    txtXName.Text = cXrayPatient.XName;
                    txtPano.Text = cXrayPatient.Pano;
                    txtSName.Text = cXrayPatient.SName;

                    txtResult.Text = "";

                    if (dt.Rows[0]["Approve"].ToString().Trim() == "N")
                    {
                        txtResult.Text += "\r\n" + "\r\n" + "\r\n";
                        txtResult.Text += "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                    }
                    else
                    {
                        txtResult.Text += dt.Rows[0]["Result"].ToString().Trim() + dt.Rows[0]["Result1"].ToString().Trim();                        
                    }

                }
                else
                {
                    ComFunc.MsgBox("판독결과가 없습니다.");
                }
                
                #endregion

                dt.Dispose();

                btnPrint.Enabled = true;

            }
            else if (argJob == "ENDO")
            {
                string strResult = string.Empty;
                string strGbJob = string.Empty;
                string strInfo = string.Empty;
                string strTPro = string.Empty;
                string strNew = string.Empty;
                string strCLO = string.Empty;
                string strLowTime = string.Empty;
                string strClean = string.Empty;

                #region //endo_jupmst
                cEndoJupmst = new clsComSupEndsSQL.cEndoJupmst();
                cEndoJupmst.STS = "9";
                cEndoJupmst.Job = "*";
                cEndoJupmst.ROWID = cXrayPatient.ROWID;
                dt = cendsSql.sel_ENDO_JUPMST(pDbCon, cEndoJupmst,false);

                #region //데이터셋 읽어 자료 표시

                if (ComFunc.isDataTableNull(dt) == false)
                {                    
                    txtXCode.Text = dt.Rows[0]["OrderCode"].ToString().Trim();
                    txtXName.Text = cXrayPatient.XName;
                    txtPano.Text = cXrayPatient.Pano;
                    txtSName.Text = cXrayPatient.SName;

                    txtResult.Text = "";


                    strGbJob = dt.Rows[0]["GbJob"].ToString().Trim();
                    strNew = dt.Rows[0]["GbNEW"].ToString().Trim();

                    strInfo = "";



                    //2013-01-15 new add -------------------------------------------------------
                    strInfo += "◈ Premedication ◈" + ComNum.VBLF;


                    if (dt.Rows[0]["GBPRE_1"].ToString().Trim() == "Y")
                    {
                        strInfo += "None ";
                    }
                    else
                    {
                        strInfo += "";
                    }    
               
                    if (dt.Rows[0]["GBPRE_2"].ToString().Trim() == "Y")
                    {
                        strInfo += "Aigiron ";
                    }
                    else
                    {
                        //strInfo +=  "";
                    }
                    if (dt.Rows[0]["GBPRE_21"].ToString().Trim() != "")
                    {
                        strInfo += dt.Rows[0]["GBPRE_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBPRE_22"].ToString().Trim() + ", ";
                    }
                    if (dt.Rows[0]["GBPRE_3"].ToString().Trim() == "Y")
                    {
                        strInfo += " " + dt.Rows[0]["GBPRE_31"].ToString().Trim();
                    }
                    else
                    {
                        strInfo += dt.Rows[0]["GBPRE_31"].ToString().Trim();
                    }
                    
                    strInfo += ComNum.VBLF;                    

                    strInfo += ComNum.VBLF + "◈ Conscious Sedation ◈" + ComNum.VBLF;


                    //add2
                    if (dt.Rows[0]["MOAAS"].ToString().Trim() != "")
                    {
                        strInfo += "MOAAS / Children`s Hospital of Wisconsin sedation scale " + dt.Rows[0]["MOAAS"].ToString().Trim() + ", " + ComNum.VBLF;
                    }
                    if (dt.Rows[0]["GBCon_1"].ToString().Trim() == "Y")
                    {
                        strInfo += "None ";
                    }
                    else
                    {
                        //strInfo +=  "";
                    }
                    if (dt.Rows[0]["GBCon_2"].ToString().Trim() == "Y")
                    {
                        strInfo += "Mediazolam ";
                    }
                    else
                    {
                        //strInfo +=  "Mediazolam ";
                    }
                    if (dt.Rows[0]["GBCon_21"].ToString().Trim() != "")
                    {
                        strInfo += dt.Rows[0]["GBCon_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_22"].ToString().Trim() + ", ";
                    }
                    if (dt.Rows[0]["GBCon_3"].ToString().Trim() == "Y")
                    {
                        strInfo += "Propotol ";
                    }
                    else
                    {
                        //strInfo +=  "Propotol ";
                    }
                    if (dt.Rows[0]["GBCon_31"].ToString().Trim() != "")
                    {
                        strInfo += dt.Rows[0]["GBCon_31"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_32"].ToString().Trim() + ", ";
                    }
                    if (dt.Rows[0]["GBCon_4"].ToString().Trim() == "Y")
                    {
                        strInfo += "Pethidine ";
                    }
                    else
                    {
                        //strInfo +=  "Pethidine ";
                    }
                    if (dt.Rows[0]["GBCon_41"].ToString().Trim() != "")
                    {
                        strInfo += dt.Rows[0]["GBCon_41"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_42"].ToString().Trim() + ", ";
                    }
                    
                    strClean = dt.Rows[0]["Gb_Clean"].ToString().Trim(); //장정결도
                    
                    if (dt.Rows[0]["D_INTIME1"].ToString().Trim() != "")
                    {
                        strLowTime += "내시경 삽입시간:" + dt.Rows[0]["D_INTIME1"].ToString().Trim()  + "분" + dt.Rows[0]["D_INTIME2"].ToString().Trim() + "초";
                        strLowTime += "  회수시간:" + dt.Rows[0]["D_EXTIME1"].ToString().Trim() + "분" + dt.Rows[0]["D_EXTIME2"].ToString().Trim() + "초";
                    }                       
                    strCLO = dt.Rows[0]["GBPRO_2"].ToString().Trim() ;  //CLO
                    
                    strInfo += ComNum.VBLF;


                    //2015-07-23  
                    strTPro = "";

                    if (dt.Rows[0]["PRO_BX1"].ToString().Trim() == "Y") strTPro += "Bx. bottle ";
                    if (dt.Rows[0]["PRO_BX2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_BX2"].ToString().Trim() + "ea, ";

                    if (dt.Rows[0]["PRO_PP1"].ToString().Trim() == "Y") strTPro += "PP ";
                    if (dt.Rows[0]["PRO_PP2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_PP2"].ToString().Trim() + "ea, ";

                    //2019-05-29 안정수 추가, Rapid Urease Test
                    if (dt.Rows[0]["PRO_RUT"].ToString().Trim() == "Y") strTPro += "Rapid Urease Test, ";

                    if (dt.Rows[0]["PRO_ESD1"].ToString().Trim() == "Y") strTPro += "ESD, ";
                    if (dt.Rows[0]["PRO_ESD2"].ToString().Trim() == "Y") strTPro += "en-bloc, ";
                    if (dt.Rows[0]["PRO_ESD3_1"].ToString().Trim() == "Y") strTPro += "piecemeal ";
                    if (dt.Rows[0]["PRO_ESD3_2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_ESD3_2"].ToString().Trim() + ", " + ComNum.VBLF;

                    if (dt.Rows[0]["PRO_EMR1"].ToString().Trim() == "Y") strTPro += "EMR, ";
                    if (dt.Rows[0]["PRO_EMR2"].ToString().Trim() == "Y") strTPro += "en-bloc, ";
                    if (dt.Rows[0]["PRO_EMR3_1"].ToString().Trim() == "Y") strTPro += "piecemeal ";
                    if (dt.Rows[0]["PRO_EMR3_2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_EMR3_2"].ToString().Trim() + ", " + ComNum.VBLF;

                    if (dt.Rows[0]["PRO_APC"].ToString().Trim() == "Y") strTPro += "APC, ";
                    if (dt.Rows[0]["PRO_ELEC"].ToString().Trim() == "Y") strTPro += "Electrocauterization, ";

                    if (dt.Rows[0]["PRO_HEMO1"].ToString().Trim() == "Y") strTPro += "Hemoclip ";
                    if (dt.Rows[0]["PRO_HEMO2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_HEMO2"].ToString().Trim() + "ea, " + ComNum.VBLF;

                    if (dt.Rows[0]["PRO_EPNA1"].ToString().Trim() == "Y") strTPro += "Hemoclip ";
                    if (dt.Rows[0]["PRO_EPNA2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_EPNA2"].ToString().Trim() + "cc, ";

                    if (dt.Rows[0]["PRO_MBAND"].ToString().Trim() == "Y") strTPro += "multi-band, " + ComNum.VBLF ;

                    if (dt.Rows[0]["PRO_EST"].ToString().Trim() == "Y") strTPro += "EST (";
                    if (dt.Rows[0]["PRO_EST_STS"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_EST_STS"].ToString().Trim() + ") ";

                    if (strGbJob == "3")
                    {
                        if (dt.Rows[0]["PRO_BAND1"].ToString().Trim() == "Y") strTPro += "band ";
                    }  
                    else
                    {
                        if (dt.Rows[0]["PRO_BAND1"].ToString().Trim() == "Y") strTPro += "Single-band ";
                    }
                    if (dt.Rows[0]["PRO_BAND2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_BAND2"].ToString().Trim() + "ea, ";

                    if (dt.Rows[0]["PRO_HIST1"].ToString().Trim() == "Y") strTPro += "Histoacyl ";
                    if (dt.Rows[0]["PRO_HIST2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_HIST2"].ToString().Trim() + "ample, ";

                    if (dt.Rows[0]["PRO_DETA"].ToString().Trim() == "Y") strTPro += "Detachable snare, " + ComNum.VBLF;


                    if (dt.Rows[0]["PRO_BALL"].ToString().Trim() == "Y") strTPro += "Ballooon, ";
                    if (dt.Rows[0]["PRO_BASKET"].ToString().Trim() == "Y") strTPro += "Basket, " + ComNum.VBLF;

                    if (dt.Rows[0]["PRO_EPBD1"].ToString().Trim() == "Y") strTPro += "EPBD ";
                    if (dt.Rows[0]["PRO_EPBD2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_EPBD2"].ToString().Trim() + "mm ";
                    if (dt.Rows[0]["PRO_EPBD3"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_EPBD3"].ToString().Trim() + "atm ";
                    if (dt.Rows[0]["PRO_EPBD4"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_EPBD4"].ToString().Trim() + "sec" + ComNum.VBLF;

                    if (dt.Rows[0]["PRO_ENBD1"].ToString().Trim() == "Y") strTPro += "ENBD ";
                    if (dt.Rows[0]["PRO_ENBD2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_ENBD2"].ToString().Trim() + "Fr. ";
                    if (dt.Rows[0]["PRO_ENBD3"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_ENBD3"].ToString().Trim() + "type ";

                    if (dt.Rows[0]["PRO_ERBD1"].ToString().Trim() == "Y") strTPro += "ERBD ";
                    if (dt.Rows[0]["PRO_ERBD2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_ERBD2"].ToString().Trim() + "Fr. ";
                    if (dt.Rows[0]["PRO_ERBD3"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_ERBD3"].ToString().Trim() + "type ";
                       
                    
                }
                else
                {
                    ComFunc.MsgBox("판독결과가 없습니다.");
                }
                

                #endregion

                dt.Dispose();

                #endregion

                #region //endo_result
                
                dt = cendsSql.sel_ENDO_RESULT(pDbCon, cXrayPatient.WRTNO);
                
                #region //데이터셋 읽어 자료 표시

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    string strResult1 = VB.RTrim(dt.Rows[0]["Remark1"].ToString());
                    string strResult2 = VB.RTrim(dt.Rows[0]["Remark2"].ToString());
                    string strResult3 = VB.RTrim(dt.Rows[0]["Remark3"].ToString());
                    string strResult4 = VB.RTrim(dt.Rows[0]["Remark4"].ToString());
                    string strResult5 = VB.RTrim(dt.Rows[0]["Remark5"].ToString());
                    string strResult6 = VB.RTrim(dt.Rows[0]["Remark6"].ToString());
                    string strResult62 = VB.RTrim(dt.Rows[0]["Remark6_2"].ToString());
                    string strResult63 = VB.RTrim(dt.Rows[0]["Remark6_3"].ToString());

                    if (strGbJob =="1")
                    {
                        strResult += "▶Vocal Cord:" + ComNum.VBLF + strResult1 + ComNum.VBLF + ComNum.VBLF ;
                        strResult += "▶Carina:" + ComNum.VBLF + strResult2 + ComNum.VBLF + ComNum.VBLF;
                        strResult += "▶Bronchi:" + ComNum.VBLF + strResult3 + ComNum.VBLF + ComNum.VBLF;
                        if (strTPro!="")
                        {
                            strResult += "▶EndoScopic Procedure:" + ComNum.VBLF + strTPro + ComNum.VBLF + strResult4 + ComNum.VBLF + ComNum.VBLF;
                        }
                        else
                        {
                            strResult += "▶EndoScopic Procedure:" + ComNum.VBLF + strResult4 + ComNum.VBLF + ComNum.VBLF;
                        }
                        strResult += "▶Endoscopic Biopsy:" + ComNum.VBLF + strResult6 ;
                    }
                    else if (strGbJob == "2")
                    {
                        strResult += "▶Esophagus:" + ComNum.VBLF + strResult1 + ComNum.VBLF + ComNum.VBLF;
                        strResult += "▶Stomach:" + ComNum.VBLF + strResult2 + ComNum.VBLF + ComNum.VBLF;
                        strResult += "▶Duodenum:" + ComNum.VBLF + strResult3 + ComNum.VBLF + ComNum.VBLF;
                        
                        if (strNew =="Y")
                        {

                            strResult += "▶Endoscopic Diagnosis:" + ComNum.VBLF + strResult4 + ComNum.VBLF + ComNum.VBLF;
                            strResult += strInfo + ComNum.VBLF + ComNum.VBLF;

                            if (strCLO =="Y")
                            {
                                if (strTPro !="")
                                {
                                    strResult += "▶Endoscopic Procedure:" + ComNum.VBLF + "CLO" + ComNum.VBLF + strTPro + ComNum.VBLF + strResult5 + ComNum.VBLF + ComNum.VBLF;
                                }
                                else
                                {
                                    strResult += "▶Endoscopic Procedure:" + ComNum.VBLF + "CLO" + ComNum.VBLF  + strResult5 + ComNum.VBLF + ComNum.VBLF;
                                }
                            }
                            else
                            {
                                if (strTPro != "")
                                {
                                    strResult += "▶Endoscopic Procedure:" + ComNum.VBLF + ComNum.VBLF + strTPro + ComNum.VBLF + strResult5 + ComNum.VBLF + ComNum.VBLF;
                                }
                                else
                                {
                                    strResult += "▶Endoscopic Procedure:" + ComNum.VBLF + ComNum.VBLF + strResult5 + ComNum.VBLF + ComNum.VBLF;
                                }
                            }
                            
                            strResult += "▶Endoscopic Biopsy:" + ComNum.VBLF + strResult6 + ComNum.VBLF + strResult62 + ComNum.VBLF + strResult63 + ComNum.VBLF + ComNum.VBLF;
                            strResult += "◈ Remark ◈" + ComNum.VBLF + dt.Rows[0]["Remark"].ToString();
                        }
                        else
                        {
                            strResult += strInfo + ComNum.VBLF + ComNum.VBLF;
                            strResult += "▶Endoscopic Procedure:" + ComNum.VBLF + strResult5 + ComNum.VBLF + ComNum.VBLF;
                            strResult += "▶Endoscopic Biopsy:" + ComNum.VBLF + strResult6 ;

                        }

                    }
                    else if (strGbJob == "3")
                    {
                        if (strNew == "Y")
                        {
                            strResult += "▶Endoscopic Findings:" + ComNum.VBLF + "small Intestinal:"+ strResult1 + ComNum.VBLF + "large Intestinal :" + strResult4 + ComNum.VBLF + "rectum :" + strResult5 + ComNum.VBLF;
                            strResult += "▶Endoscopic Diagnosis:" + ComNum.VBLF + strResult2 + ComNum.VBLF + ComNum.VBLF;
                            if (strLowTime !="")
                            {
                                strResult += "◈ 장정결도 ◈" + ComNum.VBLF + strClean + ComNum.VBLF + strLowTime + ComNum.VBLF + ComNum.VBLF;
                            }
                            else
                            {
                                strResult += "◈ 장정결도 ◈" + ComNum.VBLF + strClean + ComNum.VBLF + ComNum.VBLF;
                            }
                            strResult += strInfo + ComNum.VBLF + ComNum.VBLF;
                            if (strTPro !="")
                            {
                                strResult += "▶Endoscopic Procedure:" + ComNum.VBLF + strTPro + ComNum.VBLF + strResult3 + ComNum.VBLF + ComNum.VBLF;
                            }
                            else
                            {
                                strResult += "▶Endoscopic Procedure:" + ComNum.VBLF + strResult3 + ComNum.VBLF + ComNum.VBLF;
                            }
                            strResult += "▶Endoscopic Biopsy:" + ComNum.VBLF + strResult6 + ComNum.VBLF + strResult62 + ComNum.VBLF + strResult63 + ComNum.VBLF + ComNum.VBLF;
                            strResult += "◈ Remark ◈" + ComNum.VBLF + dt.Rows[0]["Remark"].ToString();
                        }
                        else
                        {
                            strResult += "▶Endoscopic Findings:" + ComNum.VBLF +  strResult1 + ComNum.VBLF + ComNum.VBLF;
                            strResult += "▶Endoscopic Diagnosis:" + ComNum.VBLF + strResult2 + ComNum.VBLF + ComNum.VBLF;
                            strResult += "▶Endoscopic Procedure:" + ComNum.VBLF + strResult3 + ComNum.VBLF + ComNum.VBLF;
                            strResult += "▶Endoscopic Biopsy:" + ComNum.VBLF + strResult6 ;
                        }

                    }
                    else if (strGbJob == "4")
                    {
                        strResult += "▶ERCP Finding:" + ComNum.VBLF + strResult1 + ComNum.VBLF + ComNum.VBLF;
                        strResult += "▶Diagnosis:" + ComNum.VBLF + strResult2 + ComNum.VBLF + ComNum.VBLF;
                        strResult += "▶Plan & Tx:" + ComNum.VBLF + strResult3 + ComNum.VBLF + ComNum.VBLF;
                        if (strTPro!="")
                        {
                            strResult += "▶EndoScopic Procedure:" + ComNum.VBLF + strTPro + ComNum.VBLF + strResult4 + ComNum.VBLF + ComNum.VBLF;
                        }
                        else
                        {
                            strResult += "▶EndoScopic Procedure:" + ComNum.VBLF + strResult4 + ComNum.VBLF + ComNum.VBLF;
                        }
                        strResult += "▶Endoscopic Biopsy:" + ComNum.VBLF + strResult6 ;
                    }

                }
                else
                {
                    ComFunc.MsgBox("판독결과가 없습니다.");
                }
                #endregion

                dt.Dispose();

                #endregion

                txtResult.Text = strResult;

            }
            else if (argJob == "HIC")
            {
                cHic_Xray_Result.Job = "04"; //rowid 조건
                dt = xread.sel_HIC_XRAY_RESULT(pDbCon, cHic_Xray_Result);
                                
                #region //데이터셋 읽어 자료 표시

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    txtXCode.Text = dt.Rows[0]["XCode"].ToString().Trim();
                    txtXName.Text = cXrayPatient.XName;
                    txtPano.Text = cXrayPatient.Pano;
                    txtSName.Text = cXrayPatient.SName;

                    txtResult.Text = "";

                    if (dt.Rows[0]["Result1"].ToString().Trim() == "")
                    {
                        txtResult.Text += "\r\n" + "\r\n" + "\r\n";
                        txtResult.Text += "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                    }
                    else
                    {
                        txtResult.Text += "판독분류: " + dt.Rows[0]["Result2"].ToString().Trim() + "\r\n";
                        txtResult.Text += "판독분류명: " + dt.Rows[0]["Result3"].ToString().Trim() + "\r\n";
                        txtResult.Text += "판독소견: " + dt.Rows[0]["Result4"].ToString().Trim() + "\r\n";
                    }

                }
                else
                {
                    ComFunc.MsgBox("판독결과가 없습니다.");
                }




                #endregion

                dt.Dispose();
            }
            

            Cursor.Current = Cursors.Default;
            
        }

    }
}
