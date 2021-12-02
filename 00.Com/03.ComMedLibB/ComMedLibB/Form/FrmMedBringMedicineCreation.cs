using System;
using System.Data;
using System.Windows.Forms;
using ComBase;


namespace ComMedLibB
{
    /// <summary>
    /// Class Name      : ComMedLibB
    /// File Name       : FrmMedBringMedicineCreation.cs
    /// Description     : 본원지참약생성
    /// Author          : 안정수
    /// Create Date     : 2017-12-05
    /// Update History  : 
    /// TODO : exe명으로 if문 사용한 부분 To-be 기준으로 변경 필요
    /// <history>       
    /// d:\psmh\Ocs\Frm본원지참약생성.frm(Frm본원지참약생성) => FrmMedBringMedicineCreation.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\Ocs\Frm본원지참약생성.frm(Frm본원지참약생성)
    /// </seealso>
    /// </summary>
    public partial class FrmMedBringMedicineCreation : Form
    {
        string FstrPano = "";        
        string FstrROWID = "";
        string FstrInfo = "";
        long FnIPDNO = 0;
        long FnWRTNO = 0;
        int intRowAffected = 0;

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

        string FormName = "";

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        public FrmMedBringMedicineCreation()
        {
            InitializeComponent();
            setEvent();
        }

        public FrmMedBringMedicineCreation(string form, string strPano, long nIPDNO)
        {
            InitializeComponent();
            setEvent();
            FormName = form;
            FstrPano = strPano;
            FnIPDNO = nIPDNO;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);
            this.btnAdd.Click += new EventHandler(eBtnEvent);
            this.btnDel.Click += new EventHandler(eBtnEvent);

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnSave)
            {
                if (ssOrder.ActiveSheet.NonEmptyRowCount == 0) return;

                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                btnSave_Click();
            }

            else if (sender == this.btnAdd)
            {
                //
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                btnAdd_Click();

            }

            else if (sender == this.btnDel)
            {
                //
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                btnDel_Click();
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            Set_Init();          
        }

        void Set_Init()
        {
            btnAdd.Visible = false;
            btnDel.Visible = false;
            btnSave.Visible = false;

            //If UCase(App.EXEName) = "CADEX"
            if (FormName != "" && FormName.ToUpper() == "CADEX")
            {
                btnAdd.Visible = true;
                btnDel.Visible = true;

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  PANO, IPDNO, DEPTCODE, DRCODE, WARDCODE, ROOMCODE, TO_CHAR(INDATE,'YYYY-MM-DD') INDATE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                SQL += ComNum.VBLF + "WHERE IPDNO = " + clsPublic.GstrHelpCode;

                try
                {
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        clsOrdFunction.Pat.PtNo = dt.Rows[0]["PANO"].ToString().Trim();
                        clsOrdFunction.Pat.IPDNO = Convert.ToInt64(clsPublic.GstrHelpCode);
                        clsOrdFunction.Pat.DeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                        clsOrdFunction.Pat.DrCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                        clsOrdFunction.Pat.WardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                        clsOrdFunction.Pat.RoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                        clsOrdFunction.Pat.INDATE = dt.Rows[0]["INDATE"].ToString().Trim();
                        FnIPDNO = Convert.ToInt64(clsPublic.GstrHelpCode);
                    }
                }

                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
                dt.Dispose();
                dt = null;

                clsPublic.GstrHelpCode = "";
            }
            else
            {
                btnSave.Visible = true;
            }

            FstrPano = clsOrdFunction.Pat.PtNo;
            FnIPDNO = clsOrdFunction.Pat.IPDNO;

            if (FormName != "" && FormName.ToUpper() == "EORDER")
            {
                FnIPDNO = clsOrdFunction.Pat.IPDNO;
            }

            FstrInfo = clsOrdFunction.Pat.DeptCode.Trim() + "^^" + clsOrdFunction.Pat.DrCode.Trim()
                                                          + "^^" + clsOrdFunction.Pat.WardCode.Trim()
                                                          + "^^" + clsOrdFunction.Pat.RoomCode.Trim() + "^^";

            ssOrder.ActiveSheet.Columns[9].Visible = false;

            ssOrder.ActiveSheet.Columns[12].Visible = false;
            ssOrder.ActiveSheet.Columns[13].Visible = false;
            ssOrder.ActiveSheet.Columns[14].Visible = false;
            ssOrder.ActiveSheet.Columns[15].Visible = false;
            ssOrder.ActiveSheet.Columns[16].Visible = false;

            FstrROWID = READ_본원지참약_마스터체크("", FstrPano, FnIPDNO, "");
        }

        public string READ_본원지참약_마스터체크(string ArgJob, string ArgPano, long ArgIPDNO, string ArgInfo)
        {
            string rtnVal = "";
            //int ix = 0;
            string sGBIO = "";
            string sWardCode = "";

            #region 변수 선언부
            string strDept = "";
            string strWard = "";
            string strRoom = "";
            string strDrCode = "";
            //string strRDATE = "";
            //string strRTime = "";
            string strHosp = "";
            string strHOSPGBN = "";
            string strPhar = "";
            string strYTIME = "";
            string strSAYU1 = "";
            string strSAYU2 = "";
            string strSAYU3 = "";
            string strSAYU4 = "";
            string strSAYU5 = "";
            string strSAYU6 = "";
            string strSAYU7 = "";
            string strSAYU8 = "";
            string strSAYU9 = "";
            string strSAYU10 = "";
            string strSAYU11 = "";
            string strSAYU12 = "";
            string strSAYU13 = "";
            string strSAYU14 = "";
            string strSAYU15 = "";
            string strSAYU16 = "";
            string strSAYU17 = "";
            string strBLOODHISTORY = "";
            string strDRUGQTY = "";
            string strFAST = "";
            string strSabun = "";
            string strWRTNO = "";
            #endregion

            if (ArgIPDNO == 0)
            {
                return "";
            }

            rtnVal = "";
            FnWRTNO = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  ROWID,WRTNO";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIMST";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND PANO ='" + ArgPano + "'";
            SQL += ComNum.VBLF + "      AND IPDNO =" + ArgIPDNO + "";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }

            if(dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["ROWID"].ToString().Trim();
                FnWRTNO = Convert.ToInt64(dt.Rows[0]["WRTNO"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            //신규이면 마스터 생성
            if(ArgJob == "INSERT" && rtnVal == "")
            {
                strDept = VB.Pstr(FstrInfo, "^^", 1).Trim();
                strDrCode = VB.Pstr(FstrInfo, "^^", 2).Trim();
                strWard = VB.Pstr(FstrInfo, "^^", 3).Trim();
                strRoom = VB.Pstr(FstrInfo, "^^", 4).Trim();

                strSabun = ComFunc.SetAutoZero(clsPublic.GstrSabun.Trim(), 5);
                strHOSPGBN = "01";
                strYTIME = "06";
                strHosp = "포항성모병원";
                strPhar = "본원처방약";

                strSAYU1 = "";
                strSAYU2 = "";
                strSAYU3 = "";
                strSAYU4 = "";
                strSAYU5 = "";
                strSAYU6 = "";
                strSAYU7 = "";
                strSAYU8 = "";
                strSAYU9 = "";
                strSAYU10 = "";
                strSAYU11 = "";
                strSAYU12 = "";
                strSAYU13 = "";
                strSAYU14 = "";
                strSAYU15 = "";
                strSAYU16 = "";
                strSAYU17 = "";
                strBLOODHISTORY = "";
                strDRUGQTY = "1";

                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  MAX(WRTNO)+1 WRTNO";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIMST";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if(dt.Rows.Count > 0)
                {
                    strWRTNO = dt.Rows[0]["WRTNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);

                sGBIO = VB.Left(strWard, 2) == "외래" ? "O" : "I";
                sWardCode = VB.Left(strWard, 2) == "외래" ? "OPD" : VB.Left(strWard, 2);

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_ERP + "DRUG_HOIMST";
                    SQL += ComNum.VBLF + "(";
                    SQL += ComNum.VBLF + "BDATE, PANO, DEPTCODE,";
                    SQL += ComNum.VBLF + "ROOMCODE, HOSP, PHAR, IPDOPD,";
                    SQL += ComNum.VBLF + "DRSABUN, DRCODE, WARDCODE, HOSPCODE,";
                    SQL += ComNum.VBLF + "DABCODE, WRTNO, REMCODE1, REMCODE2,";
                    SQL += ComNum.VBLF + "REMCODE3, REMCODE4, REMCODE5, REMCODE6, VERSION, FASTRETURN,";
                    SQL += ComNum.VBLF + "REMCODE7, REMCODE8, REMCODE9, REMCODE10,";
                    SQL += ComNum.VBLF + "REMCODE11, REMCODE12, REMCODE13, REMCODE14,";
                    SQL += ComNum.VBLF + "REMCODE15, REMCODE16, REMCODE17,  BLOODHISTORY,";
                    SQL += ComNum.VBLF + "DRUGQTY,GbJob,IPDNO,Bun) ";
                    SQL += ComNum.VBLF + "VALUES (";
                    SQL += ComNum.VBLF + " trunc(SYSDATE), '" + ArgPano + "','" + strDept + "',";
                    SQL += ComNum.VBLF + "'" + strRoom + "',   '" + strHosp + "',   '" + strPhar + "', '" + sGBIO + "',";
                    SQL += ComNum.VBLF + "'" + strSabun + "',  '" + strDrCode + "', '" + sWardCode + "','" + strHOSPGBN + "', "; 
                    SQL += ComNum.VBLF + "'" + strYTIME + "',  " + strWRTNO + ",    '" + strSAYU1 + "', '" + strSAYU2 + "', ";
                    SQL += ComNum.VBLF + "'" + strSAYU3 + "',  '" + strSAYU4 + "',  '" + strSAYU5 + "', '" + strSAYU6 + "','2','" + strFAST + "',";
                    SQL += ComNum.VBLF + "'" + strSAYU7 + "',  '" + strSAYU8 + "',  '" + strSAYU9 + "', '" + strSAYU10 + "',";
                    SQL += ComNum.VBLF + "'" + strSAYU11 + "', '" + strSAYU12 + "', '" + strSAYU13 + "','" + strSAYU14 + "', ";
                    SQL += ComNum.VBLF + "'" + strSAYU15 + "', '" + strSAYU16 + "', '" + strSAYU17 + "','" + strBLOODHISTORY + "',";
                    SQL += ComNum.VBLF + strDRUGQTY + ",'1'," + ArgIPDNO + " ,'2' ) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return "";
                    }

                    clsDB.setCommitTran(clsDB.DbCon);                    
                    Cursor.Current = Cursors.Default;
                }

                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return "";
                }

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  ROWID,WRTNO";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIMST";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND PANO ='" + ArgPano + "'";
                SQL += ComNum.VBLF + "      AND IPDNO =" + ArgIPDNO + "";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if(dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["ROWID"].ToString().Trim();
                    FnWRTNO = Convert.ToInt64(dt.Rows[0]["WRTNO"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;
            }

            return rtnVal;
        }

        void btnSave_Click()
        {
            int i = 0;

            string strRemark1 = "";
            string strRemark2 = "";
            string strRemark3 = "";
            string strRemark4 = "";
            string strRemark5 = "";
            string strRemark6 = "";
            string strRemark7 = "";
            string strRemark8 = "";
            string strRemark9 = "";
            string strRemark10 = "";
            string strRemark11 = "";
            string strRemark12 = "";
            string strRemark13 = "";
            string strRemark14 = "";

            string strBlood = "";
            string strEDICODE = "";

            //string strPano = "";
            string strBDate = "";
            string strSUBCOUNT = "";

            string strOK = "";

            string strRP = "";
            string strDOSCODE = "";
            string strQTY = "";
            string strNAL = "";
            string strTUYAKGBN = "";
            string strNOT_SIKBYUL = "";

            string strNOTEDICODE = "";

            string strMetformin = "";

            string strTemp1 = "";
            string strTemp2 = "";
            string strJAGA_OCode = "";
            string strORDERCODE = "";

            if(ssOrder.ActiveSheet.Rows.Count == 0)
            {
                return;
            }

            FnWRTNO = 0;

            FstrROWID = READ_본원지참약_마스터체크("INSERT", FstrPano, FnIPDNO, FstrInfo);

            for(i = 0; i < ssOrder.ActiveSheet.Rows.Count; i++)
            {
                //선택한것만
                if(ssOrder.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strOK = "OK";

                    strRemark1 = "";    //txtDrugName.Text               //상품명
                    strRemark7 = "";    //txtMaker.Text                  //제조회사
                    strRemark2 = "";    //txtComponent.Text              //성분명 및 함량
                    strRemark3 = "";    //txtEffect.Text                 //약리작용 및 효능
                    strRemark8 = "";    //comboForm.Text                 //제형
                    strRemark6 = "";    //txtForm.Text                   //색상/모양
                    strRemark9 = "";    //txtFormF.Text                  //식별모양(앞)
                    strRemark10 = "";   //txtFormB.Text                  //식별모양(뒤)
                    strRemark11 = "";   //Left(ComboUsed.Text, 2) .Text  //원내사용여부
                    strRemark5 = "";    //txtSameD1.Text                 //동종약1
                    strRemark12 = "";   //txtSameD2.Text                 //동종약2
                    strRemark13 = "";   //txtSameD3.Text                 //동종약3

                    strORDERCODE = ssOrder.ActiveSheet.Cells[i, 1].Text;

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  SuNext,Jong,BunCode,HName,EName,SName,Unit,JeHeng, JEHENG2, JEHENG3_1, JEHENG3_2, Jeyak, EffEct,";
                    SQL += ComNum.VBLF + "  Remark011,Remark012,Remark013,Remark014,Remark015,";
                    SQL += ComNum.VBLF + "  Remark021,Remark022,Remark023,Remark024,Remark025,";
                    SQL += ComNum.VBLF + "  Remark031,Remark032,Remark033,Remark034,Remark035,";
                    SQL += ComNum.VBLF + "  Remark041,Remark042,Remark043,Remark044,Remark045,";
                    SQL += ComNum.VBLF + "  Remark051,Remark052,Remark053,Remark054,Remark055,";
                    SQL += ComNum.VBLF + "  Remark061,Remark062,Remark063,Remark064,Remark065,";
                    SQL += ComNum.VBLF + "  Remark071,Remark072,Remark073,Remark074,Remark075,";
                    SQL += ComNum.VBLF + "  Remark081,Remark082,Remark083,Remark084,Remark085,";
                    SQL += ComNum.VBLF + "  Remark091,Remark092,Remark093,Remark094,Remark095,";
                    SQL += ComNum.VBLF + "  Remark101,Remark102,Remark103,Remark104,Remark105,";
                    SQL += ComNum.VBLF + "  Remark111,Remark112,Remark113,Remark114,Remark115,";
                    SQL += ComNum.VBLF + "  Remark121,Remark122,Remark123,Remark124,Remark125,";
                    SQL += ComNum.VBLF + "  Remark131,Remark132,Remark133,Remark134,Remark135,";
                    SQL += ComNum.VBLF + "  Remark141,Remark142,Remark143,Remark144,Remark145,";
                    SQL += ComNum.VBLF + "  Remark151,Remark152,Remark153,Remark154,Remark155,";
                    SQL += ComNum.VBLF + "  Remark161,Remark162,Remark163,Remark164,Remark165,";
                    SQL += ComNum.VBLF + "  Remark171,Remark172,Remark173,Remark174,Remark175,";
                    SQL += ComNum.VBLF + "  Remark181,Remark182,Remark183,Remark184,Remark185,";
                    SQL += ComNum.VBLF + "  Remark191,Remark192,Remark193,Remark194,Remark195,";
                    SQL += ComNum.VBLF + "  Remark201,Remark202,Remark203,Remark204,Remark205,";
                    SQL += ComNum.VBLF + "  Remark211,Remark212,Remark213,Remark214,Remark215,";
                    SQL += ComNum.VBLF + "  Remark221,Remark222,Remark223,Remark224,Remark225,";
                    SQL += ComNum.VBLF + "  Remark231,Remark232,Remark233,Remark234,Remark235,";
                    SQL += ComNum.VBLF + "  Remark241,Remark242,Remark243,Remark244,Remark245,";
                    SQL += ComNum.VBLF + "  Remark251,Remark252,Remark253,Remark254,Remark255,";
                    SQL += ComNum.VBLF + "  Remark261,Remark262,Remark263,Remark264,Remark265,";
                    SQL += ComNum.VBLF + "  Remark271,Remark272,Remark273,Remark274,Remark275,";
                    SQL += ComNum.VBLF + "  Remark281,Remark282,Remark283,Remark284,Remark285,";
                    SQL += ComNum.VBLF + "  Image_YN, ROWID, DRBUN, ";
                    SQL += ComNum.VBLF + "  TO_CHAR(ENTDATE,'YYYY-MM-DD') ENTDATE, TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
                    SQL += ComNum.VBLF + "  POWDER, PCLSCODE, CAUTION, CAUTION_STRING, METFORMIN";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DRUGINFO_NEW";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND SuNext = '" + strORDERCODE + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        strRemark1 = dt.Rows[0]["HName"].ToString().Trim();
                        strRemark2 = dt.Rows[0]["sName"].ToString().Trim();
                        strRemark7 = dt.Rows[0]["Jeyak"].ToString().Trim();
                    }

                    else
                    {
                        strRemark1 = "";
                        strRemark2 = "";
                        strRemark7 = "";
                        ComFunc.MsgBox("약 명칭누락.. !!", "생성오류");
                        strOK = "";
                    }

                    dt.Dispose();
                    dt = null;

                    strRemark14 = "";       //cboQty.Text               '수량

                    strRP = "";             //Trim(ComboRP.Text)
                    strTUYAKGBN = "0";      //Chk사용불가.Value
                    strNOTEDICODE = "0";    //chkNotEdicode.Value

                    //===========================================
                    //2016-05-09
                    strQTY = ssOrder.ActiveSheet.Cells[i, 4].Text;
                    strNAL = ssOrder.ActiveSheet.Cells[i, 5].Text;
                    //===========================================
                    strNOT_SIKBYUL = "0";   //chk식별불가.Value


                    strBlood = "0";         //ChkBlood.Value               '항혈전제
                    strMetformin = "0";     //chkMetformin.Value

                    strEDICODE = ssOrder.ActiveSheet.Cells[i, 12].Text.Trim();
                    strDOSCODE = ssOrder.ActiveSheet.Cells[i, 13].Text.Trim();

                    //add

                    strTemp1 = "";
                    strTemp2 = "";

                    strTemp1 = (strRemark1.Replace("\r\n", "")).Replace("\r\n", "");
                    strTemp2 = (strRemark2.Replace("\r\n", "")).Replace("\r\n", "");

                    if(strOK == "OK" && READ_본원지참약_슬립체크(FstrPano, FnWRTNO, strEDICODE, strDOSCODE) == "OK")
                    {
                        strOK = "";
                    }

                    if(strOK == "OK")
                    {
                        if(strNOT_SIKBYUL == "1")
                        {
                            strTUYAKGBN = "1";
                        }

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ";
                        SQL += ComNum.VBLF + "  PANO, TO_CHAR(BDATE,'YYYY-MM-DD HH24:MI') BDATE ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "DRUG_HOIMST";
                        SQL += ComNum.VBLF + "WHERE WRTNO = " + FnWRTNO;

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if(dt.Rows.Count > 0)
                        {
                            strBDate = dt.Rows[0]["BDATE"].ToString().Trim();
                        }

                        dt.Dispose();
                        dt = null;

                        strSUBCOUNT = "1";


                        clsDB.setBeginTran(clsDB.DbCon);

                        //2015-08-20 정상저장건에 대한 자가약 오더코드 체크 및 자동생성
                        //2015-10-26 위치변경 - 표준코드 없는것 체크
                        
                        strJAGA_OCode = "";

                        if (clsOrderEtc.Make_Jaga_Auto_OrderCode(clsDB.DbCon, strEDICODE, strTemp1, strTemp2, strDOSCODE, ref strJAGA_OCode) == false)
                        {
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = "";
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_ERP + "DRUG_HOISLIP";
                        SQL += ComNum.VBLF + "(";
                        SQL += ComNum.VBLF + "  PANO, BDATE, REMARK1, REMARK2, ";
                        SQL += ComNum.VBLF + "  REMARK3, REMARK4, REMARK5, REMARK6,";
                        SQL += ComNum.VBLF + "  WRTNO , SUBCOUNT, REMARK7, REMARK8,";
                        SQL += ComNum.VBLF + "  REMARK9, REMARK10, REMARK11, REMARK12,";
                        SQL += ComNum.VBLF + "  REMARK13, BLOOD, EDICODE, METFORMIN,";
                        SQL += ComNum.VBLF + "  REMARK14, QTY, NAL, DOSCODE,";
                        SQL += ComNum.VBLF + "  NOT_SIKBYUL, RP, TUYAKGBN, NOT_EDICODE,OrderCode";
                        SQL += ComNum.VBLF + ")";
                        SQL += ComNum.VBLF + "VALUES (";
                        SQL += ComNum.VBLF + "  '" + FstrPano + "',                              ";
                        SQL += ComNum.VBLF + "  TO_DATE('" + strBDate + "','YYYY-MM-DD HH24:MI'),";
                        SQL += ComNum.VBLF + "  '" + strRemark1 + "',                            ";
                        SQL += ComNum.VBLF + "  '" + strRemark2 + "',                            ";
                        SQL += ComNum.VBLF + "  '" + strRemark3 + "',                            ";
                        SQL += ComNum.VBLF + "  '" + strRemark4 + "',                            ";
                        SQL += ComNum.VBLF + "  '" + strRemark5 + "',                            ";
                        SQL += ComNum.VBLF + "  '" + strRemark6 + "',                            ";
                        SQL += ComNum.VBLF + FnWRTNO + ",                                        ";
                        SQL += ComNum.VBLF + "  " + strSUBCOUNT + ",                             ";
                        SQL += ComNum.VBLF + "  '" + strRemark7 + "',                            ";
                        SQL += ComNum.VBLF + "  '" + strRemark8 + "',                            ";
                        SQL += ComNum.VBLF + "  '" + strRemark9 + "',                            ";
                        SQL += ComNum.VBLF + "  '" + strRemark10 + "',                           ";
                        SQL += ComNum.VBLF + "  '" + strRemark11 + "',                           ";
                        SQL += ComNum.VBLF + "  '" + strRemark12 + "',                           ";
                        SQL += ComNum.VBLF + "  '" + strRemark13 + "',                           ";
                        SQL += ComNum.VBLF + "  '" + strBlood + "',                              ";
                        SQL += ComNum.VBLF + "  '" + strEDICODE + "',                            ";
                        SQL += ComNum.VBLF + "  '" + strMetformin + "',                          ";
                        SQL += ComNum.VBLF + "  '" + strRemark14 + "',                           ";
                        SQL += ComNum.VBLF + "  " + VB.Val(strQTY) + ",                          ";
                        SQL += ComNum.VBLF + "  " + VB.Val(strNAL) + ",                          ";
                        SQL += ComNum.VBLF + "  '" + strDOSCODE + "',                            ";
                        SQL += ComNum.VBLF + "  '" + strNOT_SIKBYUL + "',                        ";
                        SQL += ComNum.VBLF + "  '" + strRP + "',                                 ";
                        SQL += ComNum.VBLF + "  '" + strTUYAKGBN + "',                           ";
                        SQL += ComNum.VBLF + "  '" + strNOTEDICODE + "',                         ";
                        SQL += ComNum.VBLF + "  '" + strJAGA_OCode + "'                          ";
                        SQL += ComNum.VBLF + ")";

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

                    }
                }
            }

            ComFunc.MsgBox("작업완료");

            ssOrder.ActiveSheet.Rows.Count = 0;
        }

        void btnAdd_Click()
        {
            int i = 0;
            string strBDate = "";
            clsDB.setBeginTran(clsDB.DbCon);

            //PTNO, IPDNO, INDATE, GUBUN, BDATE, CDATE, CSABUN
            for (i = 0; i < ssList1.ActiveSheet.Rows.Count; i++)
            {
                if(ssList1.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strBDate = ssList1.ActiveSheet.Cells[i, 2].Text.Trim();

                    if(ssList1.ActiveSheet.Cells[i, 6].Text.Trim() == "")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "NUR_SELFMED_CONFIRM";
                        SQL += ComNum.VBLF + "(";
                        SQL += ComNum.VBLF + "PTNO, IPDNO, INDATE, GUBUN,";
                        SQL += ComNum.VBLF + "BDATE, CDATE, CSABUN";
                        SQL += ComNum.VBLF + ")";
                        SQL += ComNum.VBLF + "VALUES (";
                        SQL += ComNum.VBLF + "  '" + clsOrdFunction.Pat.PtNo + "',                           ";
                        SQL += ComNum.VBLF + "  " + clsOrdFunction.Pat.IPDNO + ",                            ";
                        SQL += ComNum.VBLF + "  TO_DATE('" + clsOrdFunction.Pat.INDATE + "','YYYY-MM-DD'),   ";
                        SQL += ComNum.VBLF + "  '외래',                                                      ";
                        SQL += ComNum.VBLF + "  TO_DATE('" + strBDate + "','YYYY-MM-DD'),                    ";
                        SQL += ComNum.VBLF + "  SYSDATE,                                                     ";
                        SQL += ComNum.VBLF +    clsPublic.GnJobSabun;
                        SQL += ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
            }

            for(i = 0; i < ssList2.ActiveSheet.Rows.Count; i++)
            {
                if(ssList2.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strBDate = ssList2.ActiveSheet.Cells[i, 2].Text.Trim();

                    if(ssList2.ActiveSheet.Cells[i, 6].Text.Trim() == "")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "NUR_SELFMED_CONFIRM";
                        SQL += ComNum.VBLF + "(";
                        SQL += ComNum.VBLF + "PTNO, IPDNO, INDATE, GUBUN,";
                        SQL += ComNum.VBLF + "BDATE, CDATE, CSABUN";
                        SQL += ComNum.VBLF + ")";
                        SQL += ComNum.VBLF + "VALUES (";
                        SQL += ComNum.VBLF + "'" + clsOrdFunction.Pat.PtNo + "',                            ";
                        SQL += ComNum.VBLF + "" + clsOrdFunction.Pat.IPDNO + ",                             ";
                        SQL += ComNum.VBLF + "TO_DATE('" + clsOrdFunction.Pat.INDATE + "','YYYY-MM-DD'),    ";
                        SQL += ComNum.VBLF + "'퇴원',                                                       ";
                        SQL += ComNum.VBLF + "TO_DATE('" + strBDate + "','YYYY-MM-DD'),                     ";
                        SQL += ComNum.VBLF + "SYSDATE,                                                      ";
                        SQL += ComNum.VBLF + clsPublic.GnJobSabun;
                        SQL += ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
            eGetData();
        }

        void btnDel_Click()
        {
            int i = 0;
            string strBDate = "";

            //PTNO, IPDNO, INDATE, GUBUN, BDATE, CDATE, CSABUN

            for(i = 0; i < ssList1.ActiveSheet.Rows.Count; i++)
            {
                if(ssList1.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strBDate = ssList1.ActiveSheet.Cells[i, 2].Text.Trim();
                    if(ssList1.ActiveSheet.Cells[i, 6].Text != "")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "NUR_SELFMED_CONFIRM";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND IPDNO = " + clsOrdFunction.Pat.IPDNO + " ";
                        SQL += ComNum.VBLF + "      AND GUBUN = '외래' ";
                        SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
            }

            for(i = 0; i < ssList2.ActiveSheet.Rows.Count; i++)
            {
                if (ssList2.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strBDate = ssList2.ActiveSheet.Cells[i, 2].Text;
                    if(ssList2.ActiveSheet.Cells[i, 6].Text != "")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "NUR_SELFMED_CONFIRM";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "      AND IPDNO = " + clsOrdFunction.Pat.IPDNO + " ";
                        SQL += ComNum.VBLF + "      AND GUBUN = '퇴원' ";
                        SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD')";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

            }
            eGetData();
        }    

        void eGetData()
        {
            int i = 0;

            int nREAD = 0;

            CS.Spread_All_Clear(ssList1);
            CS.Spread_All_Clear(ssList2);

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  '외래약' gbn, DEPTCODE, TO_CHAR(BDate,'YYYY-MM-DD') BDate";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A";
                SQL += ComNum.VBLF + "WHERE 1=1";
                if (FstrPano == "81000004")
                {
                    SQL += ComNum.VBLF + "  AND a.Ptno    = '05792890'";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND a.Ptno    = '" + FstrPano + "' ";
                }
                SQL += ComNum.VBLF + "      AND  a.Seqno   > 0       ";
                SQL += ComNum.VBLF + "      AND  a.BDate >=TRUNC(SYSDATE - 180)";
                SQL += ComNum.VBLF + "      AND A.Bun IN ('11','12','20')";
                SQL += ComNum.VBLF + "GROUP BY DEPTCODE, TO_CHAR(BDate,'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "UNION ALL";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  '응급실' gbn, DEPTCODE, TO_CHAR(BDate,'YYYY-MM-DD') BDate";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_iORDER A";
                SQL += ComNum.VBLF + "WHERE 1=1";
                if (FstrPano == "81000004")
                {
                    SQL += ComNum.VBLF + "  AND a.Ptno    = '05792890'";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND a.Ptno    = '" + FstrPano + "'"; 
                }
                SQL += ComNum.VBLF + "      AND  a.Seqno   > 0   ";
                SQL += ComNum.VBLF + "      AND  a.BDate >=TRUNC(SYSDATE -180)";
                SQL += ComNum.VBLF + "      AND A.Bun IN ('11','12','20')";
                SQL += ComNum.VBLF + "      AND A.GBIOE ='E' ";
                SQL += ComNum.VBLF + "      AND A.GbTFlag ='T'";
                SQL += ComNum.VBLF + "      AND A.GbAct = ' ' ";
                SQL += ComNum.VBLF + "GROUP BY DEPTCODE, TO_CHAR(BDate,'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "ORDER BY BDate DESC ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;

                    ssList1.ActiveSheet.RowCount = nREAD;

                    for(i = 0; i < nREAD; i++)
                    {
                        ssList1.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["gbn"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["BDate"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList1.ActiveSheet.Cells[i, 7].Text = "";

                        #region  21-06-16 미사용 주석
                        //ssList1.ActiveSheet.Cells[i, 7].Text = READ_CONFIRM_SABUN("외래", clsOrdFunction.Pat.IPDNO, dt.Rows[i]["BDate"].ToString().Trim());
                        //if (ssList1.ActiveSheet.Cells[i, 7].Text != "")
                        //{
                        //    ssList1.ActiveSheet.Cells[i, 6].Text = "지참";
                        //}
                        #endregion
                    }
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.OUTDate,'YYYY-MM-DD') BDate, A.IPDNO,  ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.OutDate,'YYYY-MM-DD') BDate1,";
                SQL += ComNum.VBLF + "  BI, DeptCode, DrName, TO_CHAR(InDate,'YYYY-MM-DD') InDate1";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL += ComNum.VBLF + "WHERE 1=1";
                if (FstrPano == "81000004")
                {
                    SQL += ComNum.VBLF + "  AND Pano     = '05885990'  ";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND Pano     = '" + FstrPano + "'";
                }
                //SQL += ComNum.VBLF + "      AND a.InDate >=TRUNC(SYSDATE - 180)";
                //2021-02-10, 임시로 변경..
                SQL += ComNum.VBLF + "      AND a.InDate >=TRUNC(SYSDATE - 240)";
                SQL += ComNum.VBLF + "      AND A.DrCode = B.DrCode";
                SQL += ComNum.VBLF + "      AND (A.OUTDATE IS NOT NULL AND A.OUTDATE >= TRUNC(SYSDATE - 60))";
                SQL += ComNum.VBLF + "ORDER BY OUTDATE DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;
                    ssList2.ActiveSheet.Rows.Count = nREAD;

                    for (i = 0; i < nREAD; i++)
                    {
                        ssList2.ActiveSheet.Cells[i, 1].Text = "퇴원약";
                        ssList2.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["InDate1"].ToString().Trim();
                        ssList2.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["BDate1"].ToString().Trim();
                        ssList2.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList2.ActiveSheet.Cells[i, 7].Text = ""; //

                        #region 21-06-16 미사용 주석 
                        //ssList2.ActiveSheet.Cells[i, 7].Text = READ_CONFIRM_SABUN("퇴원", clsOrdFunction.Pat.IPDNO, dt.Rows[i]["InDate1"].ToString().Trim());
                        //if (ssList2.ActiveSheet.Cells[i, 7].Text != "")
                        //{
                        //    ssList2.ActiveSheet.Cells[i, 6].Text = "지참";
                        //}
                        #endregion
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
            Cursor.Current = Cursors.Default;
        }

        public string READ_CONFIRM_SABUN(string ArgGubun, long ArgIPDNO, string ArgBDate)
        {
            DataTable dt1 = null;
            string rtnVal = "";
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  CSABUN";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_SELFMED_CONFIRM";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND IPDNO = " + ArgIPDNO ;
                SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND GUBUN = '" + ArgGubun + "'";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count == 0)
                {
                    dt1.Dispose();
                    dt1 = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return "";
                }

                if (dt1.Rows.Count > 0)
                {
                    rtnVal = CF.Read_SabunName(clsDB.DbCon, dt1.Rows[0]["CSABUN"].ToString().Trim());
                }

                dt1.Dispose();
                dt1 = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            return rtnVal;
        }

        public string READ_표준코드(string ArgSuCode)
        {
            DataTable dt2 = null;
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  B.CODE,  B.SCODE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN A, " + ComNum.DB_PMPA + "EDI_SUGA B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND A.SUNEXT = '" + ArgSuCode + "'";
            SQL += ComNum.VBLF + "      AND A.BCODE IS NOT NULL";
            SQL += ComNum.VBLF + "      AND A.BCODE = B.CODE";

            SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }

            if(dt2.Rows.Count > 0)
            {
                rtnVal = dt2.Rows[0]["Code"].ToString().Trim();
            }

            dt2.Dispose();
            dt2 = null;

            return rtnVal;
        }

        public string READ_본원지참약_슬립체크(string ArgPano, long argWRTNO, string ArgEdiCode, string ArgDoscode)
        {
            DataTable dt2 = null;
            string rtnVal = "";
            //int ix = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  ROWID";
            SQL += ComNum.VBLF + " FROM " + ComNum.DB_ERP + "DRUG_HOISLIP";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND PANO ='" + ArgPano + "' ";
            SQL += ComNum.VBLF + "  AND WRTNO =" + argWRTNO + "";
            SQL += ComNum.VBLF + "  AND EDICODE ='" + ArgEdiCode + "'";
            SQL += ComNum.VBLF + "  AND DosCode ='" + ArgDoscode + "'";

            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return "";
            }

            if(dt2.Rows.Count > 0)
            {
                rtnVal = "OK";
            }

            dt2.Dispose();
            dt2 = null;

            return rtnVal;
        }

        void ssList2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strInDate = "";
            string strOutDate = "";
            string strDeptCode = "";

            chkAll.Checked = false;

            strInDate = ssList2.ActiveSheet.Cells[e.Row, 2].Text;
            strOutDate = ssList2.ActiveSheet.Cells[e.Row, 3].Text;
            strDeptCode = ssList2.ActiveSheet.Cells[e.Row, 4].Text;

            READ_IPD_ORDER(FstrPano, strInDate, strOutDate, strDeptCode);
        }

        void ssList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strGbn = "";
            string strBDate = "";
            string strDeptCode = "";

            chkAll.Checked = false;

            strGbn = ssList1.ActiveSheet.Cells[e.Row, 1].Text.Trim();
            strBDate = ssList1.ActiveSheet.Cells[e.Row, 2].Text.Trim();
            strDeptCode = ssList1.ActiveSheet.Cells[e.Row, 4].Text.Trim();

            READ_OPD_ORDER(strGbn, FstrPano, strBDate, strDeptCode);
        }

        void READ_IPD_ORDER(string ArgPano, string ArgInDate, string argOUTDATE, string ArgDeptCode)
        {
            int i = 0;
            int nRow = 0;
            //int nCNT = 0;

            string strUnit = "";
            string strBCode = "";
            string strDOSCODE = "";


            #region WITH
            SQL = "";
            SQL += ComNum.VBLF + "WITH ORDER_DATA AS ";
            SQL += ComNum.VBLF + "(";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  ORDERNAME,   DISPHEADER, C.DOSNAME,  A.REALQTY,";
            SQL += ComNum.VBLF + "  A.NAL,       A.GBDIV,    A.REMARK,   B.DISPRGB,";
            SQL += ComNum.VBLF + "  A.ORDERCODE, A.SEQNO,    ORDERNAMES, A.GBINFO,A.GBSELF,";
            SQL += ComNum.VBLF + "  B.GBBOTH,    A.SLIPNO,   A.DOSCODE,  A.SUCODE,";
            SQL += ComNum.VBLF + "  A.GBBOTH     JUSA,       A.ROWID AS RID ";

            #region 21-06-16 쿼리 합침
            SQL += ComNum.VBLF + ", CASE WHEN A.DOSCODE IS NOT NULL THEN  ";
            SQL += ComNum.VBLF + "  COALESCE( (SELECT SPECNAME  ";
            SQL += ComNum.VBLF + "               FROM " + ComNum.DB_MED + "OCS_OSPECIMAN";
            SQL += ComNum.VBLF + "               WHERE SLIPNO   = A.SLIPNO ";
            SQL += ComNum.VBLF + "                 AND SPECCODE = A.DOSCODE";
            SQL += ComNum.VBLF + "                 AND ROWNUM = 1 ";
            SQL += ComNum.VBLF + "             ) ";
            SQL += ComNum.VBLF + "           , A.GBINFO) END  DOSCODENM";

            SQL += ComNum.VBLF + ", (SELECT B2.CODE";
            SQL += ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "BAS_SUN A2, " + ComNum.DB_PMPA + "EDI_SUGA B2";
            SQL += ComNum.VBLF + "    WHERE 1=1";
            SQL += ComNum.VBLF + "      AND A2.SUNEXT = A.SUCODE";
            SQL += ComNum.VBLF + "      AND A2.BCODE IS NOT NULL";
            SQL += ComNum.VBLF + "      AND A2.BCODE = B2.CODE";
            SQL += ComNum.VBLF + ") AS READ_CODE";
            #endregion

            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER A, " + ComNum.DB_MED + "OCS_ORDERCODE B, " + ComNum.DB_MED + "OCS_ODOSAGE C";
            SQL += ComNum.VBLF + "WHERE 1=1";
            if (ArgPano == "81000004")
            {
                SQL += ComNum.VBLF + "  AND Ptno        = '05885990' ";
            }

            else
            {
                SQL += ComNum.VBLF + "  AND Ptno        = '" + ArgPano + "' ";
            }

            SQL += ComNum.VBLF + "      AND BDate       >= TO_DATE('" + ArgInDate + "','YYYY-MM-DD') ";

            if(argOUTDATE != "")
            {
                SQL += ComNum.VBLF + "  AND BDate       <= TO_DATE('" + argOUTDATE + "','YYYY-MM-DD')";
            }

            else
            {
                //SQL += ComNum.VBLF + "  AND BDate       <= TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND BDate       <= TRUNC(SYSDATE)";
            }
            SQL += ComNum.VBLF + "      AND A.DEPTCODE = '" + ArgDeptCode + "' ";
            SQL += ComNum.VBLF + "      AND A.NAL     > 0";
            SQL += ComNum.VBLF + "      AND A.Seqno     > 0";
            SQL += ComNum.VBLF + "      AND A.OrderCode = B.OrderCode ";
            SQL += ComNum.VBLF + "      AND A.Slipno    = B.Slipno ";
            SQL += ComNum.VBLF + "      AND A.DosCode   = C.DosCode(+)";
            SQL += ComNum.VBLF + "      AND A.Slipno <> '0106'";    //지참약 분류제외
            SQL += ComNum.VBLF + "      AND A.Bun IN ('11','12','20')";
            SQL += ComNum.VBLF + "      AND A.GbTFlag ='T'";        //퇴원약
            SQL += ComNum.VBLF + "ORDER BY A.BDATE, A.DEPTCODE,A.DRCODE, A.SLIPNO, A.SEQNO";
            SQL += ComNum.VBLF + ")";

            #endregion

            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "      ORDERNAME,   DISPHEADER, DOSNAME,  REALQTY,";
            SQL += ComNum.VBLF + "      NAL,       GBDIV,    REMARK,   DISPRGB,";
            SQL += ComNum.VBLF + "      ORDERCODE, SEQNO,    ORDERNAMES, GBINFO, GBSELF,";
            SQL += ComNum.VBLF + "      GBBOTH,    SLIPNO,   DOSCODE,  SUCODE,";
            SQL += ComNum.VBLF + "      JUSA,     RID ";
            SQL += ComNum.VBLF + ",     DOSCODENM,  READ_CODE";

            SQL += ComNum.VBLF + ", CASE WHEN EXISTS (SELECT 1";
            SQL += ComNum.VBLF + "                      FROM " + ComNum.DB_ERP + "DRUG_HOISLIP";
            SQL += ComNum.VBLF + "                     WHERE PANO    = '" + FstrPano + "' ";
            SQL += ComNum.VBLF + "                       AND WRTNO   = " + FnWRTNO + "";
            SQL += ComNum.VBLF + "                       AND DOSCODE = A.DOSCODE";
            SQL += ComNum.VBLF + "                    )  THEN 'OK' END MST_CHK ";

            SQL += ComNum.VBLF + "  FROM ORDER_DATA A";


            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                nRow = dt.Rows.Count;

                ssOrder.ActiveSheet.Rows.Count = nRow;
                //nCNT = 0;

                for(i = 0; i < nRow; i++)
                {
                    ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["OrderCode"].ToString().Trim();

                    if(dt.Rows[i]["OrderNameS"].ToString().Trim() != "")
                    {
                        strUnit = dt.Rows[i]["OrderName"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 2].Text = strUnit + dt.Rows[i]["OrderNameS"].ToString().Trim();
                    }

                    else if(dt.Rows[i]["DispHeader"].ToString().Trim() != "")
                    {
                        ssOrder.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DispHeader"].ToString().Trim() + " " + dt.Rows[i]["OrderName"].ToString().Trim();
                    }

                    else
                    {
                        ssOrder.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                    }

                    if(dt.Rows[i]["GbBoth"].ToString().Trim() == "1" && dt.Rows[i]["GbInfo"].ToString().Trim() != "")
                    {
                        ssOrder.ActiveSheet.Cells[i, 2].Text = ComFunc.LeftH(ssOrder.ActiveSheet.Cells[i, 2].Text, 30) + dt.Rows[i]["GbInfo"].ToString().Trim();
                    }

                    strDOSCODE = dt.Rows[i]["DOsCOde"].ToString().Trim();

                    if (dt.Rows[i]["DosName"].ToString().Trim() != "")
                    {
                        ssOrder.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["DosName"].ToString().Trim();
                    }
                    else
                    {
                        //21-06-16 쿼리 합침
                        ssOrder.ActiveSheet.Cells[i, 3].Text = dt.Rows[0]["DOSCODENM"].ToString().Trim();

                        #region 이전 로직 21-06-16 주석
                        //if (dt.Rows[i]["DosCode"].ToString().Trim() != "")
                        //{
                        //    SQL = "";
                        //    SQL += ComNum.VBLF + "SELECT ";
                        //    SQL += ComNum.VBLF + "  SpecName";
                        //    SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OSPECIMAN";
                        //    SQL += ComNum.VBLF + "WHERE 1=1";
                        //    SQL += ComNum.VBLF + "      AND Slipno   = '" + dt.Rows[i]["Slipno"].ToString().Trim() + "' ";
                        //    SQL += ComNum.VBLF + "      AND SpecCode = '" + dt.Rows[i]["DosCode"].ToString().Trim() + "'";
                        //    SQL += ComNum.VBLF + "      AND ROWNUM = 1 ";

                        //    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        //    if (SqlErr != "")
                        //    {
                        //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        //        return;
                        //    }

                        //    if(dt1.Rows.Count == 1)
                        //    {
                        //        ssOrder.ActiveSheet.Cells[i, 3].Text = dt1.Rows[0]["SpecName"].ToString().Trim();
                        //    }

                        //    else
                        //    {
                        //        ssOrder.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["GbInfo"].ToString().Trim();
                        //    }

                        //    dt1.Dispose();
                        //    dt1 = null;
                        //}

                        #endregion
                    }

                    ssOrder.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["RealQty"].ToString().Trim();
                    ssOrder.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["Nal"].ToString().Trim();

                    if(dt.Rows[i]["GbDiv"].ToString().Trim() != "0")
                    {
                        ssOrder.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["GbDiv"].ToString().Trim();
                    }

                    if(VB.Mid(dt.Rows[i]["DosCode"].ToString().Trim(), 5, 2) == "01")
                    {
                        if(dt.Rows[i]["JUSA"].ToString().Trim() == "3")
                        {
                            ssOrder.ActiveSheet.Cells[i, 7].Text = "완료";
                        }
                    }

                    if(VB.Mid(dt.Rows[i]["DOsCOde"].ToString().Trim(), 5, 2) == "02")
                    {
                        if(dt.Rows[i]["JUSA"].ToString().Trim() == "1")
                        {
                            ssOrder.ActiveSheet.Cells[i, 7].Text = "완료";
                        }
                    }

                    ssOrder.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    ssOrder.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["RID"].ToString().Trim();

                    ssOrder.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["GbSelf"].ToString().Trim();

                    #region 21-06-16 쿼리합침
                    //strBCode = READ_표준코드(dt.Rows[i]["SuCode"].ToString().Trim());
                    strBCode = dt.Rows[i]["READ_CODE"].ToString().Trim();
     
                    #endregion

                    ssOrder.ActiveSheet.Cells[i, 12].Text = strBCode;

                    ssOrder.ActiveSheet.Cells[i, 13].Text = strDOSCODE;

                    #region 21-06-16 쿼리 합침
                    //if (READ_본원지참약_슬립체크(FstrPano, FnWRTNO, strBCode, strDOSCODE) == "OK")
                    //{
                    //    ssOrder.ActiveSheet.Cells[i, 0].Text = "";
                    //}

                    if (dt.Rows[i]["MST_CHK"].ToString().Trim().Equals("OK"))
                    {
                        ssOrder.ActiveSheet.Cells[i, 0].Text = "";
                    }
                    #endregion

                    //10진수의 칼라값을 HTML형식의 RGB코드로 변환
                    string Temp = "&H" + dt.Rows[i]["DispRGB"].ToString().Trim();

                    System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml(Temp);                    
                    ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.Columns.Count - 1].ForeColor = col;


                }

                dt.Dispose();
                dt = null;
            }
        }

        void READ_OPD_ORDER(string argGBN, string ArgPano, string ArgBDate, string ArgDeptCode)
        {
            int nRow = 0;
            int nCNT = 0;

            string strUnit = "";
            string strBCode = "";
            string strDOSCODE = "";

            //DataTable dt1 = null;

            SQL = "";

            SQL += ComNum.VBLF + "WITH ORDER_DATA AS";
            SQL += ComNum.VBLF + "(";


            if (argGBN == "응급실")
            {
                //SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  OrderName,   DispHeader, C.DosName,  A.RealQty, ";
                SQL += ComNum.VBLF + "  A.Nal,       A.GbDiv,    A.Remark,   B.DispRGB,";
                SQL += ComNum.VBLF + "  A.OrderCode, A.Seqno,    OrderNameS, A.GbInfo,a.GbSelf,";
                SQL += ComNum.VBLF + "  B.GbBoth,    A.Slipno,   A.DosCode,a.SuCode,";
                SQL += ComNum.VBLF + "  A.Gbboth     JUSA,       A.ROWID AS RID , '' as GBSUNAP";
                #region 21-06-16 쿼리 합침
                SQL += ComNum.VBLF + ", CASE WHEN A.DOSCODE IS NOT NULL THEN  ";
                SQL += ComNum.VBLF + "  COALESCE( (SELECT SPECNAME  ";
                SQL += ComNum.VBLF + "               FROM " + ComNum.DB_MED + "OCS_OSPECIMAN";
                SQL += ComNum.VBLF + "               WHERE SLIPNO   = A.SLIPNO ";
                SQL += ComNum.VBLF + "                 AND SPECCODE = A.DOSCODE";
                SQL += ComNum.VBLF + "                 AND ROWNUM = 1 ";
                SQL += ComNum.VBLF + "             ) ";
                SQL += ComNum.VBLF + "           , A.GBINFO) END  DOSCODENM";

                SQL += ComNum.VBLF + ", (SELECT B2.CODE";
                SQL += ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "BAS_SUN A2, " + ComNum.DB_PMPA + "EDI_SUGA B2";
                SQL += ComNum.VBLF + "    WHERE 1=1";
                SQL += ComNum.VBLF + "      AND A2.SUNEXT = A.SUCODE";
                SQL += ComNum.VBLF + "      AND A2.BCODE IS NOT NULL";
                SQL += ComNum.VBLF + "      AND A2.BCODE = B2.CODE";
                SQL += ComNum.VBLF + ") AS READ_CODE";
                #endregion

                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER A, " + ComNum.DB_MED + "OCS_ORDERCODE B, " + ComNum.DB_MED + "OCS_ODOSAGE C";
                SQL += ComNum.VBLF + "WHERE 1=1";                
                SQL += ComNum.VBLF + "  AND Ptno        = '" + ArgPano + "'";
                SQL += ComNum.VBLF + "      AND BDate       = TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "      AND DEPTCODE    = '" + ArgDeptCode + "' ";
                SQL += ComNum.VBLF + "      AND A.NAL       > 0 ";
                SQL += ComNum.VBLF + "      AND A.Seqno     > 0";
                SQL += ComNum.VBLF + "      AND A.OrderCode = B.OrderCode";
                SQL += ComNum.VBLF + "      AND A.Slipno    = B.Slipno";
                SQL += ComNum.VBLF + "      AND A.DosCode   = C.DosCode(+)";
                SQL += ComNum.VBLF + "      AND A.Slipno <> '0106'";    //지참약 분류제외
                SQL += ComNum.VBLF + "      AND A.Bun IN ('11','12','20')";
                SQL += ComNum.VBLF + "      AND A.GbTFlag ='T'";        //퇴원약
                SQL += ComNum.VBLF + "      AND A.GbIOE ='E'";
                SQL += ComNum.VBLF + "ORDER BY a.BDate, a.DeptCode,a.DrCode, a.Slipno, a.Seqno";
            }

            else
            {
                //SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  OrderName,   DispHeader, C.DosName,  A.RealQty,";
                SQL += ComNum.VBLF + "  A.Nal,       A.GbDiv,    A.Remark,   B.DispRGB,";
                SQL += ComNum.VBLF + "  A.OrderCode, A.Seqno,    OrderNameS, A.GbInfo,a.GbSelf,";
                SQL += ComNum.VBLF + "  B.GbBoth,    A.Slipno,   A.DosCode,a.SuCode,";
                SQL += ComNum.VBLF + "  A.Gbboth     JUSA,       A.ROWID  AS RID,a.GbSunap";

                #region 21-06-16 쿼리 합침
                SQL += ComNum.VBLF + ", CASE WHEN A.DOSCODE IS NOT NULL THEN  ";
                SQL += ComNum.VBLF + "  COALESCE( (SELECT SPECNAME  ";
                SQL += ComNum.VBLF + "               FROM " + ComNum.DB_MED + "OCS_OSPECIMAN";
                SQL += ComNum.VBLF + "               WHERE SLIPNO   = A.SLIPNO ";
                SQL += ComNum.VBLF + "                 AND SPECCODE = A.DOSCODE";
                SQL += ComNum.VBLF + "                 AND ROWNUM = 1 ";
                SQL += ComNum.VBLF + "             ) ";
                SQL += ComNum.VBLF + "           , A.GBINFO) END  DOSCODENM";

                SQL += ComNum.VBLF + ", (SELECT B2.CODE";
                SQL += ComNum.VBLF + "     FROM " + ComNum.DB_PMPA + "BAS_SUN A2, " + ComNum.DB_PMPA + "EDI_SUGA B2";
                SQL += ComNum.VBLF + "    WHERE 1=1";
                SQL += ComNum.VBLF + "      AND A2.SUNEXT = A.SUCODE";
                SQL += ComNum.VBLF + "      AND A2.BCODE IS NOT NULL";
                SQL += ComNum.VBLF + "      AND A2.BCODE = B2.CODE";
                SQL += ComNum.VBLF + ") AS READ_CODE";
                #endregion

                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_MED + "OCS_ORDERCODE B, " + ComNum.DB_MED + "OCS_ODOSAGE C";
                SQL += ComNum.VBLF + "WHERE 1=1";
                if (ArgPano == "81000004")
                {
                    SQL += ComNum.VBLF + "  AND Ptno        = '05792890'";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND Ptno        = '" + ArgPano + "'";
                }
                SQL += ComNum.VBLF + "      AND BDate       = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND DEPTCODE    = '" + ArgDeptCode + "' ";
                SQL += ComNum.VBLF + "      AND A.NAL     > 0 ";
                SQL += ComNum.VBLF + "      AND A.Seqno     > 0 ";
                SQL += ComNum.VBLF + "      AND A.OrderCode = B.OrderCode";
                SQL += ComNum.VBLF + "      AND A.Slipno    = B.Slipno";
                SQL += ComNum.VBLF + "      AND A.DosCode   = C.DosCode(+)";
                SQL += ComNum.VBLF + "      AND A.Bun IN ('11','12','20')";
                SQL += ComNum.VBLF + "ORDER BY a.GbSunap DESC,a.DeptCode,a.DrCode, a.GBCOPY ,a.GBAUTOSEND2, a.Slipno, a.Seqno";                
            }

            SQL += ComNum.VBLF + ")";

            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "      ORDERNAME,   DISPHEADER, DOSNAME,  REALQTY,";
            SQL += ComNum.VBLF + "      NAL,       GBDIV,    REMARK,   DISPRGB, GBSUNAP ,";
            SQL += ComNum.VBLF + "      ORDERCODE, SEQNO,    ORDERNAMES, GBINFO, GBSELF, ";
            SQL += ComNum.VBLF + "      GBBOTH,    SLIPNO,   DOSCODE,  SUCODE,";
            SQL += ComNum.VBLF + "      JUSA,     RID, GBSUNAP ";
            SQL += ComNum.VBLF + ",     DOSCODENM,  READ_CODE";

            SQL += ComNum.VBLF + ", CASE WHEN EXISTS (SELECT 1";
            SQL += ComNum.VBLF + "                      FROM " + ComNum.DB_ERP + "DRUG_HOISLIP";
            SQL += ComNum.VBLF + "                     WHERE PANO    = '" + FstrPano + "' ";
            SQL += ComNum.VBLF + "                       AND WRTNO   = " + FnWRTNO + "";
            SQL += ComNum.VBLF + "                       AND DOSCODE = A.DOSCODE";
            SQL += ComNum.VBLF + "                    )  THEN 'OK' END MST_CHK ";

            SQL += ComNum.VBLF + "  FROM ORDER_DATA A";


            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            nRow = dt.Rows.Count;

            ssOrder.ActiveSheet.Rows.Count = nRow;

            nCNT = 0;

            for(var i = 0; i < nRow; i++)
            {
                ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["OrderCode"].ToString().Trim();

                if(dt.Rows[i]["OrderNameS"].ToString().Trim() != "")
                {
                    strUnit = dt.Rows[i]["OrderName"].ToString().Trim();
                    ssOrder.ActiveSheet.Cells[i, 2].Text = strUnit + dt.Rows[i]["OrderNameS"].ToString().Trim();
                }

                else if(dt.Rows[i]["DispHeader"].ToString().Trim() != "")
                {
                    ssOrder.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DispHeader"].ToString().Trim() + " " + dt.Rows[i]["OrderName"].ToString().Trim();
                }

                else
                {
                    ssOrder.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                }

                if(dt.Rows[i]["GbBoth"].ToString().Trim() == "1" && dt.Rows[i]["GbInfo"].ToString().Trim() != "")
                {
                    ssOrder.ActiveSheet.Cells[i, 3].Text = ComFunc.LeftH(ssOrder.ActiveSheet.Cells[i, 3].Text, 30) + dt.Rows[i]["GbInfo"].ToString().Trim();
                }

                if(dt.Rows[i]["DosName"].ToString().Trim() != "")
                {
                    ssOrder.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["DosName"].ToString().Trim();
                }
                else
                {
                    //21-06-16 쿼리 합침
                    ssOrder.ActiveSheet.Cells[i, 3].Text = dt.Rows[0]["DOSCODENM"].ToString().Trim();

                    #region 21-06-16 주석 처리
                    //if (dt.Rows[i]["DosCode"].ToString().Trim() != "")
                    //{
                    //    SQL = "";
                    //    SQL += ComNum.VBLF + "SELECT ";
                    //    SQL += ComNum.VBLF + "  SpecName";
                    //    SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OSPECIMAN";
                    //    SQL += ComNum.VBLF + "WHERE 1=1";
                    //    SQL += ComNum.VBLF + "      AND Slipno   = '" + dt.Rows[i]["Slipno"].ToString().Trim() + "' ";
                    //    SQL += ComNum.VBLF + "      AND SpecCode = '" + dt.Rows[i]["DosCode"].ToString().Trim() + "' ";
                    //    SQL += ComNum.VBLF + "      AND ROWNUM = 1";

                    //    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    //    if (SqlErr != "")
                    //    {
                    //        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    //        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    //        return;
                    //    }

                    //    if(dt1.Rows.Count == 1)
                    //    {
                    //        ssOrder.ActiveSheet.Cells[i, 3].Text = dt1.Rows[0]["SpecName"].ToString().Trim();
                    //    }

                    //    else
                    //    {
                    //        ssOrder.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["GbInfo"].ToString().Trim();
                    //    }

                    //    dt1.Dispose();
                    //    dt1 = null;
                    //}
                    #endregion
                }

                ssOrder.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["RealQty"].ToString().Trim();
                ssOrder.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["Nal"].ToString().Trim();

                if(dt.Rows[i]["GbDiv"].ToString().Trim() != "0")
                {
                    ssOrder.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["GbDiv"].ToString().Trim(); 
                }

                if(VB.Mid(dt.Rows[i]["DosCode"].ToString().Trim(), 5, 2) == "01")
                {
                    if(dt.Rows[i]["JUSA"].ToString().Trim() == "3")
                    {
                        ssOrder.ActiveSheet.Cells[i, 7].Text = "완료";
                    }                   
                }

                if (VB.Mid(dt.Rows[i]["DosCode"].ToString().Trim(), 5, 2) == "02")
                {
                    if (dt.Rows[i]["JUSA"].ToString().Trim() == "1")
                    {
                        ssOrder.ActiveSheet.Cells[i, 7].Text = "완료";
                    }
                }

                ssOrder.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["RealQty"].ToString().Trim();
                ssOrder.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["Nal"].ToString().Trim();

              
                ssOrder.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["Remark"].ToString().Trim();
                ssOrder.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["RID"].ToString().Trim();

                ssOrder.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["GbSelf"].ToString().Trim();

                #region 21-06-16 쿼리합침
                //strBCode = READ_표준코드(dt.Rows[i]["SuCode"].ToString().Trim());
                strBCode = dt.Rows[i]["READ_CODE"].ToString().Trim();
                if (dt.Rows[i]["MST_CHK"].ToString().Trim().Equals("OK"))
                {
                    ssOrder.ActiveSheet.Cells[i, 0].Text = "";
                }

                //if (READ_본원지참약_슬립체크(FstrPano, FnWRTNO, strBCode, strDOSCODE) == "OK")
                //{
                //    ssOrder.ActiveSheet.Cells[i, 0].Text = "";
                //}
                #endregion

                ssOrder.ActiveSheet.Cells[i, 12].Text = strBCode;
                strDOSCODE = dt.Rows[i]["DosCode"].ToString().Trim();
                ssOrder.ActiveSheet.Cells[i, 13].Text = strDOSCODE;

                string Temp = "&H" + dt.Rows[i]["DispRGB"].ToString().Trim();

                System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml(Temp);
                ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.Columns.Count - 1].ForeColor = col;

                if (argGBN == "응급실")
                {
                    nCNT += 1;
                }

                else
                {
                    if(dt.Rows[i]["GbSunap"].ToString().Trim() == "1")
                    {
                        nCNT += 1;
                    }

                    //부분수납방법
                    if(dt.Rows[i]["GbSunap"].ToString().Trim() == "2")
                    {
                        ssOrder.ActiveSheet.Cells[i, 0].Text = "1";
                        nCNT += 1;
                    }
                    else
                    {
                        ssOrder.ActiveSheet.Cells[i, 0].Text = "";
                    }
                }
            }

            dt.Dispose();
            dt = null;


        }

        private void FrmMedBringMedicineCreation_Load(object sender, EventArgs e)
        {
            eGetData();
        }
        
        private void ssOrder_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ssOrder.ActiveSheet.RowCount == 0) return;

            if (chkAll.Checked == true)
            {                
                for (int i = 0; i < ssOrder.ActiveSheet.RowCount; i++)
                {                    
                    ssOrder.ActiveSheet.Cells[i, 0].Value = true;                    
                }
            }
            else
            {
                for (int i = 0; i < ssOrder.ActiveSheet.RowCount; i++)
                {
                    ssOrder.ActiveSheet.Cells[i, 0].Value = false;
                }
            }
        }
    }
}
