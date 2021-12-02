using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewERSunapList.cs
    /// Description     : 응급실 내원 환자 명단
    /// Author          : 안정수
    /// Create Date     : 2017-09-21
    /// Update History  : 2017-11-29
    /// 안정수, WardCode null값일때 예외처리 추가
    /// <history>           
    /// d:\psmh\OPD\olrepa\Frm응급수납명단.frm(Frm응급수납명단) => frmPmpaViewERSunapList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm응급수납명단.frm(Frm응급수납명단)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewERSunapList : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();

        frmPmpaSetErCar frmPmpaSetErCarX = new frmPmpaSetErCar();
        frmPmpaTongERCarMonth frmPmpaTongERCarMonthX = new frmPmpaTongERCarMonth();

        public frmPmpaViewERSunapList()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnCarSave.Click += new EventHandler(eBtnEvent);
            this.btnCarCode.Click += new EventHandler(eBtnEvent);
            this.btnCarTong.Click += new EventHandler(eBtnEvent);

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

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            dtpFDate.Text = Convert.ToDateTime(CurrentDate).AddDays(0).ToShortDateString();
            //dtpFTime.Text = "00:01";
            txtFTime.Text = "00:01";
            dtpTDate.Text = Convert.ToDateTime(CurrentDate).AddDays(0).ToShortDateString();
            //dtpTTime.Text = "23:59";
            txtTTime.Text = "23:59";

            //ssList_Sheet1.Columns[13].Visible = true;
            //ssList_Sheet1.Columns[17].Visible = true;
            //ssList_Sheet1.Columns[18].Visible = true;

            ssList_Sheet1.Columns[ssList_Sheet1.Columns.Count - 1].Visible = false;
            ssList_Sheet1.Columns[ssList_Sheet1.Columns.Count - 2].Visible = false;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnPrint)
            {

                ePrint();
            }

            else if (sender == this.btnView)
            {

                eGetData();
            }

            else if (sender == this.btnCarSave)
            {

                btnCarSave_Click();
            }

            else if (sender == this.btnCarCode)
            {
                frmPmpaSetErCarX = new frmPmpaSetErCar();
                frmPmpaSetErCarX.rEventExit += new frmPmpaSetErCar.EventExit(frmSpecialTextX_rEventExit);
                frmPmpaSetErCarX.Show();
            }

            else if (sender == this.btnCarTong)
            {
                frmPmpaTongERCarMonthX = new frmPmpaTongERCarMonth();
                frmPmpaTongERCarMonthX.rEventExit += new frmPmpaTongERCarMonth.EventExit(frmPmpaTongERCarMonthX_rEventExit);
                frmPmpaTongERCarMonthX.Show();
            }
        }

        void frmPmpaTongERCarMonthX_rEventExit()
        {
            frmPmpaTongERCarMonthX.Dispose();
            frmPmpaTongERCarMonthX = null;
        }

        void frmSpecialTextX_rEventExit()
        {
            frmPmpaSetErCarX.Dispose();
            frmPmpaSetErCarX = null;
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;
            //ssList.ActiveSheet.Cells[0, 9].Text = "zzz";
            //ssList.ActiveSheet.Columns[9].Visible = false;


            #endregion

            strTitle = "응급실 내원환자 명단";
            strSubTitle = "조회일자 : " + dtpFDate.Text + " " + txtFTime.Text + " ∼ " + dtpTDate.Text + " " + txtTTime.Text;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(1) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 35, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, false, false, false, false, false, (float)0.9);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eGetData()
        {
            int i = 0;
            int ii = 0;
            int nRow = 0;
            int nRead = 0;
            string strPano = "";
            string strBDate = "";
            string strSname = "";
            string strFDate = "";
            string strTDate = "";
            string strFTime = "";
            string strTTime = "";
            string strOK = "";
            string strDate = "";
            string strDate1 = "";
            string strInTime = "";
            string strOutTime = "";

            string strInDate2 = "";
            string strInTime2 = "";

            string strList = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            CS.Spread_All_Clear(ssList);

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            strFTime = txtFTime.Text;
            strTTime = txtTTime.Text;

            strDate = dtpFDate.Text;
            strDate1 = dtpTDate.Text;

            nRow = 0;

            Cursor.Current = Cursors.WaitCursor;

            //NUR_ER_PATIENT NEw
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                                        ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.InTime,'YYYY-MM-DD HH24:MI') InTime,TO_CHAR(a.InTime,'YYYYMMDD HH24MI') InTime2,                                  ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.OutTime,'YYYY-MM-DD HH24:MI') OutTime,                                                                            ";
            SQL += ComNum.VBLF + "  a.DeptCode,a.WardCode,a.Room,a.Study,a.Disease,a.OutGbn,                                                                    ";
            SQL += ComNum.VBLF + "  DECODE(TRIM(InGbn),'1','직접내원','2','타병원의뢰','3','119','4','129','5','택시','6','엠블런스','9','기타','기타') InGbn,  ";
            SQL += ComNum.VBLF + "  b.Pano,b.SName,b.Age,b.Sex,b.Singu,b.Bi,TO_CHAR(b.BDate,'YYYY-MM-DD') BDate,                                                ";
            SQL += ComNum.VBLF + "  c.Tel,c.Hphone,c.ZipCode1 || c.ZipCode2 AS  ZipCode,c.Juso,a.ErCar,a.Rowid                                                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT a, " + ComNum.DB_PMPA + "OPD_MASTER b, " + ComNum.DB_PMPA + "BAS_PATIENT c          ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                                     ";
            SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                                                                    ";
            SQL += ComNum.VBLF + "      AND b.pano=c.pano(+)                                                                                                    ";
            SQL += ComNum.VBLF + "      AND nvl(DGKD,'0') <> '4'                                                                                                ";
            SQL += ComNum.VBLF + "      AND a.InTime>=TO_DATE('" + strFDate + " " + strFTime + "','YYYY-MM-DD HH24:MI')                                         ";
            SQL += ComNum.VBLF + "      AND a.InTime<=TO_DATE('" + strTDate + " " + strTTime + "' ,'YYYY-MM-DD HH24:MI')                                        ";
            SQL += ComNum.VBLF + "      AND TRUNC(a.InTime) =b.BDATE(+)                                                                                         ";
            SQL += ComNum.VBLF + "      AND b.DeptCode ='ER'                                                                                                    ";

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
                    nRead = dt.Rows.Count;
                    FarPoint.Win.Spread.CellType.ComboBoxCellType Type1 = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

                    ListBox Car = new ListBox();

                    //ER차량 구분추가
                    strList = "" + VB.Chr(9);
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                ";
                    SQL += ComNum.VBLF + "  CODE,NAME                           ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ETC_ER_CAR ";
                    SQL += ComNum.VBLF + "WHERE 1=1                             ";
                    SQL += ComNum.VBLF + "      AND Deldate IS NULL             ";
                    SQL += ComNum.VBLF + "ORDER By Code                         ";
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        for (ii = 0; ii < dt2.Rows.Count; ii++)
                        {
                            //strList += ComFunc.SetAutoZero(dt1.Rows[ii]["CODE"].ToString().Trim(), 3) + "." + dt1.Rows[ii]["NAME"].ToString().Trim() + VB.Chr(9);
                            Car.Items.Add(ComFunc.SetAutoZero(dt2.Rows[ii]["CODE"].ToString().Trim(), 3) + "." + dt2.Rows[ii]["NAME"].ToString().Trim() + VB.Chr(9));
                        }
                    }

                    Type1.Clear();
                    Type1.ListControl = Car;
                    Type1.Editable = true;

                    dt2.Dispose();
                    dt2 = null;

                    for (i = 0; i < nRead; i++)
                    {
                        strPano = dt.Rows[i]["Pano"].ToString().Trim();

                        //if(strPano == "08161993")
                        //{
                        //    i = i;
                        //}

                        strBDate = dt.Rows[i]["BDate"].ToString().Trim();
                        strSname = dt.Rows[i]["SName"].ToString().Trim();

                        strOutTime = dt.Rows[i]["OutTime"].ToString().Trim();
                        strInTime = dt.Rows[i]["InTime"].ToString().Trim();

                        strInDate2 = VB.Left(dt.Rows[i]["InTime2"].ToString().Trim(), 8);
                        strInTime2 = VB.Right(dt.Rows[i]["InTime2"].ToString().Trim(), 4);

                        strOK = "OK";

                        if (strOK == "OK")
                        {
                            nRow += 1;
                            if (nRow > ssList_Sheet1.Rows.Count)
                            {
                                ssList_Sheet1.Rows.Count = nRow;
                            }

                            ssList_Sheet1.Cells[nRow - 1, 0].Text = VB.Mid(strInTime, 6, 2) + "/" + VB.Mid(strInTime, 9, 2);
                            ssList_Sheet1.Cells[nRow - 1, 1].Text = VB.Right(strInTime, 5);

                            ssList_Sheet1.Cells[nRow - 1, 17].Text = strBDate + " " + VB.Right(strInTime, 5);

                            ssList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["Pano"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["SName"].ToString().Trim();

                            ssList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["DeptCode"].ToString().Trim();

                            //응급관리료 읽음 AC101A-감액, 나머지 AC101 에서 gbself 0 =공단, gbself 2 =본인
                            //nur_er_patient 하루에 2건이면 한개만 읽음
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT                                                                                    ";
                            SQL += ComNum.VBLF + "  DECODE(TRIM(SUNEXT),'AC101A','감액',DECODE(GBSELF,'0','공단','2','본인'))  SUNEXT,      ";
                            SQL += ComNum.VBLF + "  GBSELF,SUM(QTY*NAL) CNT                                                                 ";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP                                                       ";
                            SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                            SQL += ComNum.VBLF + "      AND BDATE =TO_DATE('" + strBDate + "','YYYY-MM-DD')                                 ";
                            SQL += ComNum.VBLF + "      AND PANO ='" + dt.Rows[i]["Pano"].ToString().Trim() + "'                            ";
                            SQL += ComNum.VBLF + "      AND SUNEXT LIKE 'AC10%'                                                             ";
                            SQL += ComNum.VBLF + "      AND Part <> '#'                                                                     ";
                            SQL += ComNum.VBLF + "GROUP BY DECODE(TRIM(SUNEXT),'AC101A','감액',DECODE(GBSELF,'0','공단','2','본인')),GBSELF ";
                            SQL += ComNum.VBLF + "HAVING SUM(Qty * NAL) > 0                                                                 ";

                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            ssList_Sheet1.Cells[nRow - 1, 5].Text = "";

                            if (dt2.Rows.Count > 0)
                            {
                                ssList_Sheet1.Cells[nRow - 1, 5].Text = dt2.Rows[0]["SUNEXT"].ToString().Trim();
                            }

                            dt2.Dispose();
                            dt2 = null;

                            ssList_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Age"].ToString().Trim() + "/" + dt.Rows[i]["Sex"].ToString().Trim();

                            ssList_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["SinGu"].ToString().Trim() == "1" ? "신환" : "구환";

                            switch (dt.Rows[i]["Bi"].ToString().Trim())
                            {
                                case "51":
                                case "53":
                                case "54":
                                    ssList_Sheet1.Cells[nRow - 1, 8].Text = "일반";
                                    break;

                                case "52":
                                case "55":
                                    ssList_Sheet1.Cells[nRow - 1, 8].Text = "교통";
                                    break;

                                case "31":
                                case "33":
                                    ssList_Sheet1.Cells[nRow - 1, 8].Text = "산재";
                                    break;

                                case "21":
                                case "22":
                                case "23":
                                case "24":
                                case "25":
                                case "26":
                                case "27":
                                case "28":
                                case "29":
                                    ssList_Sheet1.Cells[nRow - 1, 8].Text = "보호";
                                    break;

                                default:
                                    ssList_Sheet1.Cells[nRow - 1, 8].Text = "보험";
                                    break;
                            }

                            ssList_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["WardCode"].ToString().Trim();

                            if (dt.Rows[i]["Room"].ToString().Trim() != "" && Convert.ToInt32(dt.Rows[i]["Room"].ToString().Trim()) > 0)
                            {
                                ssList_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["Room"].ToString().Trim();
                            }

                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT                                                        ";
                            SQL += ComNum.VBLF + "  PTMIEMCD,PTMIIDNO,PTMIINDT,PTMIINTM,PTMISTAT,               ";
                            SQL += ComNum.VBLF + "  PTMINAME,PTMIBRTD,PTMISEXX,PTMIIUKD,PTMIHSCD,               ";
                            SQL += ComNum.VBLF + "  PTMIDRLC,PTMIAKDT,PTMIAKTM,PTMIDGKD,PTMIARCF,               ";
                            SQL += ComNum.VBLF + "  PTMIARCS,PTMIINRT,PTMIINMN,PTMIMNSY,PTMIMSSR,               ";
                            SQL += ComNum.VBLF + "  PTMISYM2,PTMISYS2,PTMISYM3,PTMISYS3,PTMIETSY,               ";
                            SQL += ComNum.VBLF + "  PTMIEMSY,PTMIRESP,PTMIHIBP,PTMILOBP,PTMIPULS,               ";
                            SQL += ComNum.VBLF + "  PTMIBRTH,PTMIBDHT,PTMIEMRT,PTMIDEPT,PTMIOTDT,               ";
                            SQL += ComNum.VBLF + "  PTMIOTTM,PTMIDCRT,PTMIDCDT,PTMIDCTM,PTMITAIP,               ";
                            SQL += ComNum.VBLF + "  PTMITSBT,PTMITSCS,PTMITSFA,PTMITSSA,PTMITSHM,               ";
                            SQL += ComNum.VBLF + "  PTMITSPT,PTMITSNO,PTMITSUR,PTMITSUK,GBSEND, SEQNO           ";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_ER_EMIHPTMI                    ";
                            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
                            SQL += ComNum.VBLF + "      AND SEQNO IN ( SELECT MAX(SEQNO) FROM NUR_ER_EMIHPTMI   ";
                            SQL += ComNum.VBLF + "                      WHERE PTMIIDNO = '" + strPano + "'      ";
                            SQL += ComNum.VBLF + "                        AND PTMIINDT = '" + strInDate2 + "'   ";
                            SQL += ComNum.VBLF + "                        AND PTMIINTM = '" + strInTime2 + "')  ";
                            SQL += ComNum.VBLF + "      AND PTMIIDNO = '" + strPano + "'                        ";
                            SQL += ComNum.VBLF + "      AND PTMIINDT = '" + strInDate2 + "'                     ";
                            SQL += ComNum.VBLF + "      AND PTMIINTM = '" + strInTime2 + "'                     ";

                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt2.Rows.Count > 0)
                            {
                                ssList_Sheet1.Cells[nRow - 1, 11].Text = ER_ComeMethod(dt2.Rows[0]["PTMIINMN"].ToString().Trim());
                            }

                            dt2.Dispose();
                            dt2 = null;

                            ssList.ActiveSheet.Cells[nRow - 1, 12].CellType = Type1;

                            //TODO : 콤보박스 아이템 추가하기
                            //FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType =
                            //                new FarPoint.Win.Spread.CellType.ComboBoxCellType();
                            //ssList_Sheet1.Columns.Get(12).CellType = comboBoxCellType;

                            //comboBoxCellType.Items = strList;
                            //((FarPoint.Win.Spread.CellType.ComboBoxCellType)ssList_Sheet1.Columns[12].CellType).Items = Items;

                            //2014-04-24
                            if (dt.Rows[i]["ErCar"].ToString().Trim() != "")
                            {
                                ssList_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["ErCar"].ToString().Trim() + "." + CF.READ_ERCAR_NAME(clsDB.DbCon, dt.Rows[i]["ErCar"].ToString().Trim());
                            }
                            else
                            {
                                ssList_Sheet1.Cells[nRow - 1, 12].Text = "";
                            }


                            ssList_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["Disease"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 14].Text = CF.READ_BAS_Mail(clsDB.DbCon, dt.Rows[i]["ZipCode"].ToString().Trim()) + " " + dt.Rows[i]["Juso"].ToString().Trim();

                            if (dt.Rows[i]["Tel"].ToString().Trim() != "")
                            {
                                if (dt.Rows[i]["Hphone"].ToString().Trim() == "")
                                {
                                    ssList_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["Tel"].ToString().Trim();
                                }
                                else
                                {
                                    ssList_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["Hphone"].ToString().Trim() + ComNum.VBLF + dt.Rows[i]["Tel"].ToString().Trim();
                                }
                            }
                            else
                            {
                                ssList_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["Hphone"].ToString().Trim();
                            }

                            switch (dt.Rows[i]["OutGbn"].ToString().Trim())
                            {
                                case "1":
                                    ssList_Sheet1.Cells[nRow - 1, 16].Text = "입원";
                                    break;
                                case "2":
                                    ssList_Sheet1.Cells[nRow - 1, 16].Text = "귀가";
                                    break;
                                case "3":
                                    ssList_Sheet1.Cells[nRow - 1, 16].Text = "DOA";
                                    break;
                                case "4":
                                    ssList_Sheet1.Cells[nRow - 1, 16].Text = "사망";
                                    break;
                                case "5":
                                    ssList_Sheet1.Cells[nRow - 1, 16].Text = "취소";
                                    break;
                                case "6":
                                    ssList_Sheet1.Cells[nRow - 1, 16].Text = "이송";
                                    break;
                                case "7":
                                    ssList_Sheet1.Cells[nRow - 1, 16].Text = "DAMA";
                                    break;
                                case "8":
                                    ssList_Sheet1.Cells[nRow - 1, 16].Text = "OPD";
                                    break;
                                case "9":
                                    ssList_Sheet1.Cells[nRow - 1, 16].Text = "OR입원";
                                    break;
                            }

                            ssList_Sheet1.Cells[nRow - 1, 18].Text = dt.Rows[i]["Rowid"].ToString().Trim();

                            ssList_Sheet1.Rows[nRow - 1].Height = ssList_Sheet1.Rows[nRow - 1].GetPreferredHeight() + 5;
                        }
                    }

                    ssList_Sheet1.Rows.Count = nRow;



                    //Sort
                    //ssList.ActiveSheet.SetColumnAllowAutoSort(-1, true);

                    Cursor.Current = Cursors.Default;

                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
        }

        void btnCarSave_Click()
        {
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strCar = "";
            string strRowID = "";

            if (ssList_Sheet1.Rows.Count == 0)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;


            clsDB.setBeginTran(clsDB.DbCon);

            for (i = 0; i < ssList_Sheet1.Rows.Count; i++)
            {
                strCar = VB.Pstr(ssList_Sheet1.Cells[i, 12].Text, ".", 1);
                strRowID = ssList_Sheet1.Cells[i, 18].Text.Trim();

                SQL = "";
                SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "NUR_ER_PATIENT";
                SQL += ComNum.VBLF + "SET";
                SQL += ComNum.VBLF + "  ErCar = '" + strCar + "'";
                SQL += ComNum.VBLF + "WHERE ROWID = '" + strRowID + "' ";


                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            }

            clsDB.setCommitTran(clsDB.DbCon);
            ComFunc.MsgBox("저장하였습니다.");
            Cursor.Current = Cursors.Default;

            eGetData();
        }

        public string ER_ComeMethod(string ArgCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ArgCode == "")
            {
                return "";
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                    ";
            SQL += ComNum.VBLF + "  CODE,NAME                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
            SQL += ComNum.VBLF + "      AND GUBUN ='EMI_내원수단'           ";
            SQL += ComNum.VBLF + "      AND CODE ='" + ArgCode + "'         ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //   ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return "";
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        private void ssList_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssList, e.Column);
                return;
            }
        }

    }
}
