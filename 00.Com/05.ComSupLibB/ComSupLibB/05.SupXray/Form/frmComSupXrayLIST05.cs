using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using System.Windows.Forms;
using ComSupLibB.SupFnEx;
using FarPoint.Win.Spread;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupFnEx
    /// File Name       : frmComSupXrayLIST05.cs
    /// Description     : 혈관조영실 시디복사 신청명단리스트
    /// Author          : 안정수
    /// Create Date     : 2019-11-19
    /// Update History  : 
    /// </summary>
    public partial class frmComSupXrayLIST05 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        clsComSup sup = new clsComSup();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSupFnExSQL fnExSql = new clsComSupFnExSQL();
        clsComSupXraySQL xraySql = new clsComSupXraySQL();
        clsComSupFnExSpd fnSpd = new clsComSupFnExSpd();

        clsComSupXraySQL.cXrayDetail cXrayDetail = null;
        clsComSupXraySQL.cXrayCdCopy cXrayCdCopy = null;

        //시트정보


        #endregion

        public frmComSupXrayLIST05()
        {
            InitializeComponent();

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            read_sysdate();

            dtpFDate.Text = cpublic.strSysDate;
            dtpTDate.Text = cpublic.strSysDate;


        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);


            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnSearch1.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);
            this.btnDelete.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);

            this.dtpFDate.TextChanged += new EventHandler(eDtpTxtChange);

            this.txtExamSabun.KeyDown += new KeyEventHandler(eTxtKeyDown);


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

                fnSpd.sSpd_CdList1(ssList1, fnSpd.sSpdCdList1, fnSpd.nSpdCdList1, 5, 0);

                fnSpd.sSpd_CdList2(ssList2, fnSpd.sSpdCdList2, fnSpd.nSpdCdList2, 5, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                setCtrlData();

                screen_clear();
            }

        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
            else if (sender == this.btnSearch1)
            {
                //조회
                screen_display();
            }
            else if (sender == this.btnSave)
            {
                eSave(clsDB.DbCon);
            }
            else if (sender == this.btnDelete)
            {
                eDel(clsDB.DbCon);
            }
            else if (sender == this.btnPrint)
            {
                ePrint();
            }

        }

        void eDtpTxtChange(object sender, EventArgs e)
        {
            dtpTDate.Text = dtpFDate.Text.Trim();
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {

            if (sender == this.txtExamSabun)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strSabun = txtExamSabun.Text.Trim();
                    if (strSabun != "")
                    {
                        txtExamName.Text = clsVbfunc.GetInSaName(clsDB.DbCon, strSabun);

                    }
                }
            }

        }

        void eSave(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;


            if (txtExamName.Text.Trim() == "")
            {
                ComFunc.MsgBox("작업자 공란입니다.... 확인하세요!!", "값공란");
                return;
            }

            int nLastRow = ssList2.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;

            read_sysdate();

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            for (int i = 0; i < nLastRow; i++)
            {
                if (ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.chk].Text.Trim() == "True")
                {

                    #region  //class 초기화 변수 세팅

                    cXrayCdCopy = new clsComSupXraySQL.cXrayCdCopy();
                    cXrayCdCopy.Pano = ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.Ptno].Text.Trim();
                    cXrayCdCopy.SName = ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.SName].Text.Trim();
                    cXrayCdCopy.BDate = ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.BDate].Text.Trim();
                    //cXrayCdCopy.SeekDate = ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.SeekDate].Text.Trim();
                    cXrayCdCopy.SeekDate = ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.BDate].Text.Trim();
                    cXrayCdCopy.DeptCode = ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.DeptCode].Text.Trim();
                    cXrayCdCopy.DrCode = ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.DrCode].Text.Trim();
                    cXrayCdCopy.GbIO = ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.GbIO].Text.Trim();
                    cXrayCdCopy.RoomCode = ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.Room].Text.Trim();
                    cXrayCdCopy.WardCode = ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.Ward].Text.Trim();
                    cXrayCdCopy.XJong = "9";
                    cXrayCdCopy.XCode = ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.XCdoe].Text.Trim();

                    DataTable dt2 = xraySql.sel_Xray_Code(clsDB.DbCon, cXrayCdCopy.XCode, "**", false);

                    if (dt2.Rows.Count > 0)
                    {
                        cXrayCdCopy.XName = dt2.Rows[0]["XNAME"].ToString().Trim();
                    }

                    dt2.Dispose();
                    dt2 = null;
                    
                    cXrayCdCopy.XSubCode = "01";
                    cXrayCdCopy.CdGubun = ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.CdGubun].Text.Trim();
                    cXrayCdCopy.OrderNo = Convert.ToInt32(ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.OrderNo].Text.Trim());
                    cXrayCdCopy.CdQty = Convert.ToInt16(ssList2.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.QTY].Text.Trim());
                    cXrayCdCopy.Job = "01";
                    cXrayCdCopy.CdMake = Convert.ToInt32(txtExamSabun.Text.Trim());
                    cXrayCdCopy.EntSabun = Convert.ToInt32(clsType.User.Sabun);

                    #endregion

                    dt = xraySql.sel_XRAY_CDCOPY(pDbCon, cXrayCdCopy);

                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        SqlErr = xraySql.up_Xray_CdCopy(pDbCon, cXrayCdCopy, ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = xraySql.ins_Xray_CdCopy(pDbCon, cXrayCdCopy, ref intRowAffected);
                    }

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


            screen_clear();

            screen_display();

        }

        void eDel(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ComFunc.MsgBoxQ("선택한건을 삭제처리 하시겠습니까??", "삭제확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            int nLastRow = ssList1.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;

            read_sysdate();

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            for (int i = 0; i < nLastRow; i++)
            {
                if (ssList1.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.chk].Text.Trim() == "True")
                {

                    #region  //class 초기화 변수 세팅

                    cXrayCdCopy = new clsComSupXraySQL.cXrayCdCopy();
                    cXrayCdCopy.Job = "02";
                    cXrayCdCopy.DelDate = cpublic.strSysDate;
                    cXrayCdCopy.ROWID = ssList1.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.ROWID].Text.Trim();
                    cXrayCdCopy.EntSabun = Convert.ToInt32(clsType.User.Sabun);

                    #endregion

                    if (cXrayCdCopy.ROWID != "")
                    {
                        SqlErr = xraySql.up_Xray_CdCopy(pDbCon, cXrayCdCopy, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                        
                            return;
                        }
                    }

                }

            }


            clsDB.setCommitTran(pDbCon);


            screen_clear();

            screen_display();


        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = ""; 
            bool PrePrint = true;

            strTitle = "혈관조영실 CD COPY 대장 " + "(" + dtpFDate.Text.Trim() + " ~ " + dtpTDate.Text.Trim() + ")";

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            SPR.setSpdPrint(ssList1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");


        }

        void screen_display()
        {
            GetData1(clsDB.DbCon, ssList1, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
            GetData2(clsDB.DbCon, ssList2, dtpFDate.Text.Trim(), dtpTDate.Text.Trim());
        }

        void screen_clear(string Job = "")
        {
            read_sysdate();


            txtExamSabun.Text = "";
            txtExamName.Text = "";


        }

        void GetData1(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate)
        {

            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            //쿼리실행    
            cXrayCdCopy = new clsComSupXraySQL.cXrayCdCopy();
            cXrayCdCopy.Job = "10";
            //cXrayCdCopy.CdGubun = "3";
            cXrayCdCopy.Date1 = argSDate;
            cXrayCdCopy.Date2 = argTDate;

            dt = xraySql.sel_XRAY_CDCOPY(pDbCon, cXrayCdCopy);

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.chk].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.BDate].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.GbIO].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.Ptno].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.Ward].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.Room].Text = dt.Rows[i]["RoomCode"].ToString().Trim();

                    if (dt.Rows[i]["FC_TeamNo"].ToString().Trim() == "" && dt.Rows[i]["IpdOpd"].ToString().Trim() == "I" && dt.Rows[i]["WardCode"].ToString().Trim() != "")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.TeamNo].Text = sup.Read_Nur_TeamNo(pDbCon, dt.Rows[i]["RoomCode"].ToString().Trim(), true, dt.Rows[i]["WardCode"].ToString().Trim());
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.TeamNo].Text = dt.Rows[i]["FC_TeamNo"].ToString().Trim();
                    }
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.CdDate].Text = dt.Rows[i]["CopyTime"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.sabun].Text = clsVbfunc.GetInSaName(pDbCon, dt.Rows[i]["CdMake"].ToString().Trim());
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.XName].Text = dt.Rows[i]["XNAME"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList1.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }

        void GetData2(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argSDate, string argTDate)
        {

            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            //쿼리실행    
            cXrayDetail = new clsComSupXraySQL.cXrayDetail();
            cXrayDetail.Job = "02";
            cXrayDetail.Date1 = argSDate;
            cXrayDetail.Date2 = argTDate;
            cXrayDetail.XCode = "'CAGCOPY'";

            dt = xraySql.sel_XrayDetail_order(pDbCon, cXrayDetail, "ANGIO");

            #region //데이터셋 읽어 자료 표시

            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {


                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.GbIO].Text = dt.Rows[i]["IpdOpd"].ToString().Trim();

                    if (dt.Rows[i]["XCode"].ToString().Trim() != "")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.STS].Text = "수납";

                        if(dt.Rows[i]["XCode"].ToString().Trim() == "XCDC")
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.Remark].Text = "CD";
                        }
                        else
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.Remark].Text = "DVD";
                        }
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.STS].Text = "처방";
                    }

                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.BDate].Text = dt.Rows[i]["BDate"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.SeekDate].Text = dt.Rows[i]["SeekDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.Ptno].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.DrCode].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.Ward].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.Room].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.XCdoe].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.XJong].Text = dt.Rows[i]["XJong"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.XSub].Text = dt.Rows[i]["XSubCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.QTY].Text = dt.Rows[i]["CNT"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.CdGubun].Text = "3";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupFnExSpd.enmCdList2.OrderNo].Text = dt.Rows[i]["OrderNo"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }
    }
}
