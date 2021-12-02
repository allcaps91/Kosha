using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmJemsuSuga1 : Form
    {
        struct TABLE_SUGA_CODE
        {
            public string SuCode;
            public string SuNext;
            public string Bun;
            public string Nu;
            public string Gbn;
            public string DelDate;
            public string SugbA;
            public string SugbB;
            public string SugbC;
            public string SugbD;
            public string SugbE;
            public string SugbF;
            public string SugbG;
            public string SugbH;
            public string SugbI;
            public string SugbJ;
            public string SugbK;
            public string SugbL;
            public string SugbM;
            public string SugbN;
            public string SugbO;
            public int[] BAmt;
            public int[] TAmt;
            public int[] IAmt;
            public string[] SuDate;
            public string BCode;
            public double Gesu;
            public string EdiDate;
            public string OldBCode;
            public double OldGesu;
            public string SuNameK;
            public string TRowid;
            public string NRowid;
        }
        TABLE_SUGA_CODE TSC = new TABLE_SUGA_CODE();

        struct TABLE_EDI_SUGA
        {
            public string ROWID;
            public string Code;
            public string Jong;
            public string Pname;
            public string Bun;
            public string Danwi1;
            public string Danwi2;
            public string Spec;
            public string COMPNY;
            public string Effect;
            public string Gubun;
            public string Dangn;
            public string JDate1;
            public string Price1;
            public string JDate2;
            public string Price2;
            public string JDate3;
            public string Price3;
            public string JDate4;
            public string Price4;
            public string JDate5;
            public string Price5;
        }        
        TABLE_EDI_SUGA TES = new TABLE_EDI_SUGA();

        
        public frmJemsuSuga1()
        {
            InitializeComponent();
        }

        private void frmJemsuSuga1_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            TSC.BAmt = new int[5];
            TSC.TAmt = new int[5];
            TSC.IAmt = new int[5];
            TSC.SuDate = new string[4];


            ComFunc.ReadSysDate(clsDB.DbCon);

            //TxtDate.Text = "2009-01-01"
            txtPrice.Text = "63.4";
            lblJob.Text = "";
            txtFCode.Text = "";
            txtTCode.Text = "";

            if (clsPublic.FstrPassGrade == "EDPS")
            {
                btnSaveJob.Visible = true;
            }
            else
            {
                btnSaveJob.Visible = false;
            }
        }

        private void btnSaveStart_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            saveData();
        }

        private bool saveData()
        {
            int i = 0;
            bool rtVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strBcode = "";
            double nJemsu = 0;
            double nJemsuPrice = 0;
            int nPrice = 0;
            string strOK = "";
            string strName = "";
            int nIlsu = 0;
            int nJobCNT = 0;
            int nRow = 0;
            string GstrMsgList = "";

            lblJob.Text = "";

            ComFunc.ReadSysDate(clsDB.DbCon);


            if (dtpDate.Value < Convert.ToDateTime(clsPublic.GstrSysDate))
            {
                nIlsu = Convert.ToInt32(DATE_ILSU(clsPublic.GstrSysDate, dtpDate.Text));
            }
            else
            {
                nIlsu = Convert.ToInt32(DATE_ILSU(dtpDate.Text, clsPublic.GstrSysDate));
            }

            if (nIlsu > 10)
            {
                ComFunc.MsgBox("작업일자는 현재일 기준 전후 10일간만 가능함.");
                dtpDate.Focus();
                return rtVal;
            }

            if (VB.Val(txtPrice.Text) < 10)
            {
                ComFunc.MsgBox("상대가치 단가를 입력하세요.", "확인");
                txtPrice.Focus();
                return rtVal;
            }

            GstrMsgList = "변경할 수가 적용일자가 정말로 " + dtpDate.Text.Trim() + "일이" + ComNum.VBLF;
            GstrMsgList = GstrMsgList + "맞습니까? 만일 오류날짜를 지정하면 " + ComNum.VBLF;
            GstrMsgList = GstrMsgList + "다시 복구가 불가능 합니다. ";

            if (MessageBox.Show(GstrMsgList, "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return rtVal;
            }

            btnSaveStart.Enabled = false;
            btnCancelJob.Enabled = false;

            strOK = "OK";
            //ss1_Sheet1.RowCount = 50;
            SS_Clear(ss1_Sheet1);

            txtFCode.Text = VB.UCase(VB.Trim(txtFCode.Text));
            txtTCode.Text = VB.UCase(VB.Trim(txtTCode.Text));


            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                //'점수가 0인것은 처리 안함
                //'적용일자1이 수가적용일과 동일한것만 작업을 함.
                SQL = "SELECT BCODE,JEMSU1,PRICE1 FROM BAS_SUGAJEMSU ";
                SQL = SQL + ComNum.VBLF + "WHERE JEMSU1 > 0 ";
                SQL = SQL + ComNum.VBLF + "  AND JDATE1=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
                if (txtFCode.Text != "") SQL = SQL + ComNum.VBLF + "  AND BCODE>='" + txtFCode.Text + "' ";
                if (txtTCode.Text != "") SQL = SQL + ComNum.VBLF + "  AND BCODE<='" + txtTCode.Text + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BCODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count > 0)
                {
                    nRow = 0;
                    nJemsuPrice = VB.Val(txtPrice.Text);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strBcode = dt.Rows[i]["BCode"].ToString().Trim();
                        nJemsu = Convert.ToDouble(VB.Val(dt.Rows[i]["Jemsu1"].ToString().Trim()));
                        nPrice = Convert.ToInt32(VB.Val(dt.Rows[i]["Price1"].ToString().Trim()));

                        if (nPrice == 0 && nJemsu != 0)
                        {
                            nPrice = VB.Fix((int)(((nJemsu * nJemsuPrice) / 10) + 0.5));
                            nPrice = nPrice * 10;
                        }

                        nJobCNT = 0;
                        if (nPrice > 0)
                        {
                            if (BCode_SUGA_UPDATE((dtpDate.Text), strBcode, nPrice, ref nJobCNT) == false)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                //ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                Cursor.Current = Cursors.Default;
                                return rtVal;
                            }
                        }
                        else
                        {
                            nJobCNT = -1;
                        }

                        lblJob.Text = Convert.ToString(i) + ": " + strBcode + " " + Convert.ToString(nPrice);

                        if (nJobCNT != -2 && nJobCNT != 0)
                        {
                            nRow = nRow + 1;
                            ss1_Sheet1.RowCount = nRow;

                            ss1_Sheet1.Cells[nRow - 1, 0].Text = strBcode;
                            ss1_Sheet1.Cells[nRow - 1, 1].Text = VB.Format(nJemsu, "###,###,###,##0.00 ");
                            ss1_Sheet1.Cells[nRow - 1, 2].Text = VB.Format(nPrice, "###,###,###,##0 ");
                            if (nJobCNT == -1)
                            {
                                ss1_Sheet1.Cells[nRow - 1, 3].Text = "** ERROR **";
                            }
                            else
                            {
                                ss1_Sheet1.Cells[nRow - 1, 3].Text = VB.Format(nJobCNT, "###,###,##0");
                            }
                        }
                    }
                    dt.Dispose();
                    dt = null;

                    btnSaveStart.Enabled = true;
                    btnCancelJob.Enabled = true;

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("정상적으로 처리됨", "확인");
                    Cursor.Current = Cursors.Default;
                    rtVal = true;
                    return rtVal;
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private void btnCancelJob_Click(object sender, EventArgs e)
        {
            ss1_Sheet1.RowCount = 50;
            SS_Clear(ss1_Sheet1);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string DATE_ILSU(string ArgTdate, string ArgFdate, string ArgGb = "")
        {
            //TODO Busuga1, VbFunc.bas
            DataTable dt = null;
            string SQL = "";
            string rtnVal = "";
            string SqlErr = "";

            if (VB.Len(ArgFdate) != 10 || VB.IsDate(ArgFdate) || VB.Len(ArgTdate) != 10 || VB.IsDate(ArgFdate))
            {
                return rtnVal;
            }

            if (String.Compare(ArgFdate, ArgTdate) > 0)
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "   TO_DATE('" + ArgTdate + "','YYYY-MM-DD') - ";
                SQL = SQL + ComNum.VBLF + "   TO_DATE('" + ArgFdate + "','YYYY-MM-DD') Gigan ";
                SQL = SQL + ComNum.VBLF + "FROM DUAL";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 1)
                {
                    rtnVal = dt.Rows[0]["Gigan"].ToString().Trim();
                }
                else
                {
                    rtnVal = "";
                }

                if (ArgGb != "ALL")
                {
                    //if DATE_ILSU >= 1000 Then  '일수 계산 제한 옵션으로 풀도록 함수 수정
                    //DATE_ILSU = 999
                    //End If
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private bool BCode_SUGA_UPDATE(string ArgDate, string ArgBCode, int ArgPrice, ref int ArgJobCNT)
        {
            //TODO, Busuga00.bas 수가 관련 코드 구현필요
            bool rtVal = false;
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int nBAmt = 0;
            int nTAmt = 0;
            int nIAmt = 0;
            int nJobCNT = 0;
            int nJemsu = 0;
            string strOK = "";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인

                //' 표준코드를 기준으로 해당 수가코드를 SELECT
                //'   -삭제된것은 작업을 안함
                //'   -수가변경일이후에 이미 변경된 것은 작업을 안함

                SQL = "SELECT SUCODE,SUNEXT,BUN,NU,GBN,SUGBA,SUGBB,SUGBC,SUGBD,SUGBE,SUGBF,";
                SQL = SQL + ComNum.VBLF + " SUGBG,SUGBH,SUGBI,SUGBJ,SUGBK,SUGBL,SUGBM,SUGBN,SUGBO,";
                SQL = SQL + ComNum.VBLF + " BAMT,TAMT,IAMT, BCODE, TO_CHAR(EDIDATE,'YYYY-MM-DD') EDIDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE,OLDBAMT,OLDTAMT,OLDIAMT,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE3,'YYYY-MM-DD') SUDATE3,BAMT3,TAMT3,IAMT3,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE4,'YYYY-MM-DD') SUDATE4,BAMT4,TAMT4,IAMT4,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE5,'YYYY-MM-DD') SUDATE5,BAMT5,TAMT5,IAMT5,";
                SQL = SQL + ComNum.VBLF + " SUHAM,SUNAMEK,TROWID,NROWID, OLDBCODE,OLDGESU ";
                SQL = SQL + ComNum.VBLF + " FROM VIEW_SUGA_CODE ";
                SQL = SQL + ComNum.VBLF + "WHERE RTRIM(BCODE) = '" + ArgBCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND (SUDATE IS NULL OR SUDATE <= TO_DATE('" + ArgDate + "','YYYY-MM-DD')) ";
                SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "  AND SUGBF = '0' ";     // '비급여는 제외
                SQL = SQL + ComNum.VBLF + "  AND NOT (GBN='T' AND SUGBA > '1') ";       // '그룹코드의 BAS_SUT는 제외
                SQL = SQL + ComNum.VBLF + "ORDER BY SUCODE,SUNEXT,GBN DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ArgJobCNT = -2;
                    return true;
                }

                nJobCNT = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    //'읽은 자료를 변수에 저장
                    TSC.SuCode = dt.Rows[i]["SuCode"].ToString().Trim();
                    TSC.SuNext = dt.Rows[i]["SuNext"].ToString().Trim();
                    TSC.Bun = dt.Rows[i]["Bun"].ToString().Trim();
                    TSC.Nu = dt.Rows[i]["Nu"].ToString().Trim();
                    TSC.Gbn = dt.Rows[i]["Gbn"].ToString().Trim();
                    TSC.DelDate = "";
                    TSC.SugbA = dt.Rows[i]["SugbA"].ToString().Trim();
                    TSC.SugbB = dt.Rows[i]["SugbB"].ToString().Trim();
                    TSC.SugbC = dt.Rows[i]["SugbC"].ToString().Trim();
                    TSC.SugbD = dt.Rows[i]["SugbD"].ToString().Trim();
                    TSC.SugbE = dt.Rows[i]["SugbE"].ToString().Trim();
                    TSC.SugbF = dt.Rows[i]["SugbF"].ToString().Trim();
                    TSC.SugbG = dt.Rows[i]["SugbG"].ToString().Trim();
                    TSC.SugbH = dt.Rows[i]["SugbH"].ToString().Trim();
                    TSC.SugbI = dt.Rows[i]["SugbI"].ToString().Trim();
                    TSC.SugbJ = dt.Rows[i]["SugbJ"].ToString().Trim();
                    TSC.SugbK = dt.Rows[i]["SugbK"].ToString().Trim();
                    TSC.SugbL = dt.Rows[i]["SugbL"].ToString().Trim();
                    TSC.SugbM = dt.Rows[i]["SugbM"].ToString().Trim();
                    TSC.SugbN = dt.Rows[i]["SugbN"].ToString().Trim();
                    TSC.SugbO = dt.Rows[i]["SugbO"].ToString().Trim();
                    //'보험수가
                    TSC.BAmt[0] = Convert.ToInt32(VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()));
                    TSC.BAmt[1] = Convert.ToInt32(VB.Val(dt.Rows[i]["OldBAmt"].ToString().Trim()));
                    TSC.BAmt[2] = Convert.ToInt32(VB.Val(dt.Rows[i]["BAmt3"].ToString().Trim()));
                    TSC.BAmt[3] = Convert.ToInt32(VB.Val(dt.Rows[i]["BAmt4"].ToString().Trim()));
                    TSC.BAmt[4] = Convert.ToInt32(VB.Val(dt.Rows[i]["BAmt5"].ToString().Trim()));
                    //'자보수가
                    TSC.TAmt[0] = Convert.ToInt32(VB.Val(dt.Rows[i]["TAmt"].ToString().Trim()));
                    TSC.TAmt[1] = Convert.ToInt32(VB.Val(dt.Rows[i]["OldTAmt"].ToString().Trim()));
                    TSC.TAmt[2] = Convert.ToInt32(VB.Val(dt.Rows[i]["TAmt3"].ToString().Trim()));
                    TSC.TAmt[3] = Convert.ToInt32(VB.Val(dt.Rows[i]["TAmt4"].ToString().Trim()));
                    TSC.TAmt[4] = Convert.ToInt32(VB.Val(dt.Rows[i]["TAmt5"].ToString().Trim()));
                    //'일반수가
                    TSC.IAmt[0] = Convert.ToInt32(VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()));
                    TSC.IAmt[1] = Convert.ToInt32(VB.Val(dt.Rows[i]["OldIAmt"].ToString().Trim()));
                    TSC.IAmt[2] = Convert.ToInt32(VB.Val(dt.Rows[i]["IAmt3"].ToString().Trim()));
                    TSC.IAmt[3] = Convert.ToInt32(VB.Val(dt.Rows[i]["IAmt4"].ToString().Trim()));
                    TSC.IAmt[4] = Convert.ToInt32(VB.Val(dt.Rows[i]["IAmt5"].ToString().Trim()));
                    //'변경일자
                    TSC.SuDate[0] = dt.Rows[i]["SuDate"].ToString().Trim();
                    TSC.SuDate[1] = dt.Rows[i]["SuDate3"].ToString().Trim();
                    TSC.SuDate[2] = dt.Rows[i]["SuDate4"].ToString().Trim();
                    TSC.SuDate[3] = dt.Rows[i]["SuDate5"].ToString().Trim();
                    //'표준코드
                    TSC.BCode = dt.Rows[i]["BCode"].ToString().Trim();
                    TSC.Gesu = Convert.ToDouble(VB.Val(dt.Rows[i]["SuHam"].ToString().Trim()));
                    TSC.EdiDate = dt.Rows[i]["EdiDate"].ToString().Trim();
                    TSC.OldBCode = dt.Rows[i]["OldBCode"].ToString().Trim();
                    TSC.OldGesu = Convert.ToDouble(VB.Val(dt.Rows[i]["OldGesu"].ToString().Trim()));
                    //'명칭 및 ROWID
                    TSC.SuNameK = dt.Rows[i]["SuNameK"].ToString().Trim();
                    TSC.TRowid = dt.Rows[i]["TRowid"].ToString().Trim();
                    TSC.NRowid = dt.Rows[i]["NRowid"].ToString().Trim();

                    //'환산단위를 기준으로 보험수가를 계산
                    if (TSC.Gesu == 0) TSC.Gesu = 1;
                    if (TSC.Gesu == 1)
                    {
                        nBAmt = ArgPrice;
                    }
                    else
                    {
                        nBAmt = Convert.ToInt32(ArgPrice * TSC.Gesu);
                    }

                    //'외부의뢰검사 10%가산
                    if (dt.Rows[i]["SugbJ"].ToString().Trim() == "9")
                    {
                        nBAmt = VB.Fix((int)Convert.ToDouble((nBAmt * 1.1)) + 5);
                        nBAmt = VB.Fix(nBAmt / 10);
                        nBAmt = nBAmt * 10;
                    }

                    //'자보수가(보험수가와 동일함)
                    nTAmt = nBAmt;
                    //'일반수가를 계산
                    nIAmt = Gesan_IlbanAmt(nBAmt, TSC.Bun, TSC.SugbE, TSC.SugbF);


                    strOK = "OK";
                    //'변경일자,보;험수가,일반수가가 변경할 내용과 동일하면 작업을 안함
                    if (TSC.BAmt[0] == nBAmt) strOK = "NO";

                    if (strOK == "OK")
                    {
                        if (BCode_Suga_Update_SUB(ArgDate, nBAmt, nTAmt, nIAmt, ref nJobCNT) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return rtVal;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                ArgJobCNT = nJobCNT;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }


        private bool BCode_Suga_Update_SUB(string ArgDate, int nBAmt, int nTAmt, int nIAmt, ref int nJobCNT)
        {
            bool rtVal = false;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            //'재작업이면 Old수가는 처리 안함
            if (TSC.SuDate[0] == ArgDate)
            {
                if (TSC.Gbn == "T") SQL = "UPDATE BAS_SUT SET ";
                if (TSC.Gbn == "H") SQL = "UPDATE BAS_SUH SET ";
                SQL = SQL + ComNum.VBLF + " BAMT=" + nBAmt + ",TAMT=" + nTAmt + ",IAMT=" + nIAmt + " ";
                SQL = SQL + ComNum.VBLF + "WHERE SUCODE = '" + VB.Trim(TSC.SuCode) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SUNEXT = '" + VB.Trim(TSC.SuNext) + "' ";
            }
            else
            {
                //'현재수가를 Old수가로 한단계씩 아래로 이동함
                TSC.SuDate[3] = TSC.SuDate[2];
                TSC.BAmt[4] = TSC.BAmt[3];
                TSC.TAmt[4] = TSC.TAmt[3];
                TSC.IAmt[4] = TSC.IAmt[3];

                TSC.SuDate[2] = TSC.SuDate[1];
                TSC.BAmt[3] = TSC.BAmt[2];
                TSC.TAmt[3] = TSC.TAmt[2];
                TSC.IAmt[3] = TSC.IAmt[2];

                TSC.SuDate[1] = TSC.SuDate[0];
                TSC.BAmt[2] = TSC.BAmt[1];
                TSC.TAmt[2] = TSC.TAmt[1];
                TSC.IAmt[2] = TSC.IAmt[1];

                TSC.SuDate[0] = ArgDate;
                TSC.BAmt[1] = TSC.BAmt[0];
                TSC.TAmt[1] = TSC.TAmt[0];
                TSC.IAmt[1] = TSC.IAmt[0];

                TSC.BAmt[0] = nBAmt;
                TSC.TAmt[0] = nTAmt;
                TSC.IAmt[0] = nIAmt;


                //'BAS_SUT에 UPDATE
                if (TSC.Gbn == "T") SQL = "UPDATE BAS_SUT SET ";
                if (TSC.Gbn == "H") SQL = "UPDATE BAS_SUH SET ";

                SQL = SQL + ComNum.VBLF + " BAMT=" + TSC.BAmt[0] + ",TAMT=" + TSC.TAmt[0] + ",IAMT=" + TSC.IAmt[0] + ",";
                SQL = SQL + ComNum.VBLF + " OLDBAMT=" + TSC.BAmt[1] + ",OLDTAMT=" + TSC.TAmt[1] + ",OLDIAMT=" + TSC.IAmt[1] + ",";
                SQL = SQL + ComNum.VBLF + " BAMT3=" + TSC.BAmt[2] + ",TAMT3=" + TSC.TAmt[2] + ",IAMT3=" + TSC.IAmt[2] + ",";
                SQL = SQL + ComNum.VBLF + " BAMT4=" + TSC.BAmt[3] + ",TAMT4=" + TSC.TAmt[3] + ",IAMT4=" + TSC.IAmt[3] + ",";
                SQL = SQL + ComNum.VBLF + " BAMT5=" + TSC.BAmt[4] + ",TAMT5=" + TSC.TAmt[4] + ",IAMT5=" + TSC.IAmt[4] + ",";
                SQL = SQL + ComNum.VBLF + " SUDATE=TO_DATE('" + VB.Trim(TSC.SuDate[0]) + "','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + " SUDATE3=TO_DATE('" + VB.Trim(TSC.SuDate[1]) + "','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + " SUDATE4=TO_DATE('" + VB.Trim(TSC.SuDate[2]) + "','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + " SUDATE5=TO_DATE('" + VB.Trim(TSC.SuDate[3]) + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "WHERE SUCODE = '" + VB.Trim(TSC.SuCode) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SUNEXT = '" + VB.Trim(TSC.SuNext) + "' ";
            }
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return rtVal;
            }

            //'그룹코드의 합계금액을 UPDATE
            if (TSC.Gbn == "H")
            {
                if (Group_SUGA_UPDATE(VB.Trim(TSC.SuCode), ArgDate) != "OK")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                    
                    //ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }
            }

            nJobCNT = nJobCNT + 1;

            rtVal = true;
            return rtVal;
        }

        private string Group_SUGA_UPDATE(string ArgSuCode, string ArgSuDate)
        {
            string rtVal = "NO";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            //' 묶음코드의 보험,자보,일반수가를 하위코드를 기준으로 다시 계산함
            //' ArgSuCode$:수가코드  ArgSuDate$:적용일자

            int nTotBAmt;
            int nTotTAmt;
            int nTotIAmt;

            string[] strSuDate = new string[4];
            int[] nBAmt = new int[5];
            int[] nTAmt = new int[5];
            int[] nIAmt = new int[5];

            //'보험총액,자보총액,일반수가총액을 계산함.

            SQL = "SELECT SUM(DECODE(SUGBE,'1',BAMT*SUQTY*1.25,BAMT*SUQTY)) CBAMT,";
            SQL = SQL + ComNum.VBLF + " SUM(DECODE(SUGBE,'1',TAMT*SUQTY*1.37,TAMT*SUQTY)) CTAMT,";
            SQL = SQL + ComNum.VBLF + " SUM(IAMT*SUQTY) CIAMT ";
            SQL = SQL + ComNum.VBLF + " FROM BAS_SUH ";
            SQL = SQL + ComNum.VBLF + "WHERE SUCODE = '" + ArgSuCode + "' ";
            SQL = SQL + ComNum.VBLF + "  AND SUGBSS IN ('0','1') "; //'일반수가 계산시 성인기준으로

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return rtVal;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                clsPublic.GstrMsgList = ArgSuCode + " 수가코드는 그룹(묶음)코드가 아니거나," + ComNum.VBLF;
                clsPublic.GstrMsgList = clsPublic.GstrMsgList + "수가코드가 등록되지 않았습니다." + ComNum.VBLF + ComNum.VBLF;
                clsPublic.GstrMsgList = clsPublic.GstrMsgList + "그룹(묶음)코드의 보험,자보,일반수가" + ComNum.VBLF;
                clsPublic.GstrMsgList = clsPublic.GstrMsgList + "변경하지 못 하였습니다.";

                ComFunc.MsgBox(clsPublic.GstrMsgList, "오류발생");
                return rtVal;
            }

            nTotBAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["cBAmt"].ToString().Trim()));
            nTotTAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["cTAmt"].ToString().Trim()));

            if (Convert.ToInt32(VB.Val(dt.Rows[0]["cIAmt"].ToString().Trim())) >= 1000)
            {
                nTotIAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["cIAmt"].ToString().Trim())) / 100;
                nTotIAmt = nTotIAmt * 100;
            }
            else
            {
                nTotIAmt = Convert.ToInt32(VB.Val(dt.Rows[0]["cIAmt"].ToString().Trim())) / 10;
                nTotIAmt = nTotIAmt * 10;
            }

            dt.Dispose();
            dt = null;

            //'UPDATE할 BAS_SUT를 READ
            SQL = "SELECT BAMT,TAMT,IAMT,TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE,";
            SQL = SQL + ComNum.VBLF + " OLDBAMT,OLDTAMT,OLDIAMT,";
            SQL = SQL + ComNum.VBLF + " BAMT3,TAMT3,IAMT3,TO_CHAR(SUDATE3,'YYYY-MM-DD') SUDATE3,";
            SQL = SQL + ComNum.VBLF + " BAMT4,TAMT4,IAMT4,TO_CHAR(SUDATE4,'YYYY-MM-DD') SUDATE4,";
            SQL = SQL + ComNum.VBLF + " BAMT5,TAMT5,IAMT5,TO_CHAR(SUDATE5,'YYYY-MM-DD') SUDATE5 ";
            SQL = SQL + ComNum.VBLF + " FROM BAS_SUT ";
            SQL = SQL + ComNum.VBLF + "WHERE SUCODE = '" + VB.Trim(ArgSuCode) + "' ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return rtVal;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;

                clsPublic.GstrMsgList = ArgSuCode + "가 수가코드(BAS_SUT)에 등록되지" + ComNum.VBLF;
                clsPublic.GstrMsgList = clsPublic.GstrMsgList + "않았습니다. 확인 하세요.";
                ComFunc.MsgBox(clsPublic.GstrMsgList, "오류발생");
                return rtVal;
            }


            nBAmt[0] = Convert.ToInt32(VB.Val(dt.Rows[0]["BAmt"].ToString().Trim()));
            nTAmt[0] = Convert.ToInt32(VB.Val(dt.Rows[0]["TAmt"].ToString().Trim()));
            nIAmt[0] = Convert.ToInt32(VB.Val(dt.Rows[0]["IAmt"].ToString().Trim()));
            strSuDate[0] = dt.Rows[0]["SuDate"].ToString().Trim();
            nBAmt[1] = Convert.ToInt32(VB.Val(dt.Rows[0]["OldBAmt"].ToString().Trim()));
            nTAmt[1] = Convert.ToInt32(VB.Val(dt.Rows[0]["OldTAmt"].ToString().Trim()));
            nIAmt[1] = Convert.ToInt32(VB.Val(dt.Rows[0]["OldIAmt"].ToString().Trim()));
            strSuDate[1] = dt.Rows[0]["SuDate3"].ToString().Trim();
            nBAmt[2] = Convert.ToInt32(VB.Val(dt.Rows[0]["BAmt3"].ToString().Trim()));
            nTAmt[2] = Convert.ToInt32(VB.Val(dt.Rows[0]["TAmt3"].ToString().Trim()));
            nIAmt[2] = Convert.ToInt32(VB.Val(dt.Rows[0]["IAmt3"].ToString().Trim()));
            strSuDate[2] = dt.Rows[0]["SuDate4"].ToString().Trim();
            nBAmt[3] = Convert.ToInt32(VB.Val(dt.Rows[0]["BAmt4"].ToString().Trim()));
            nTAmt[3] = Convert.ToInt32(VB.Val(dt.Rows[0]["TAmt4"].ToString().Trim()));
            nIAmt[3] = Convert.ToInt32(VB.Val(dt.Rows[0]["IAmt4"].ToString().Trim()));
            strSuDate[3] = dt.Rows[0]["SuDate5"].ToString().Trim();
            nBAmt[4] = Convert.ToInt32(VB.Val(dt.Rows[0]["BAmt5"].ToString().Trim()));
            nTAmt[4] = Convert.ToInt32(VB.Val(dt.Rows[0]["TAmt5"].ToString().Trim()));
            nIAmt[4] = Convert.ToInt32(VB.Val(dt.Rows[0]["IAmt5"].ToString().Trim()));

            dt.Dispose();
            dt = null;


            if (strSuDate[0] == ArgSuDate)
            {
                SQL = "UPDATE BAS_SUT SET BAMT=" + nTotBAmt + ",";
                SQL = SQL + ComNum.VBLF + " TAMT=" + nTotTAmt + ",IAMT=" + nTotIAmt + " ";
                SQL = SQL + ComNum.VBLF + "WHERE SUCODE='" + VB.Trim(ArgSuCode) + "' ";
            }
            else
            {
                SQL = "UPDATE BAS_SUT SET ";
                SQL = SQL + ComNum.VBLF + " BAMT=" + nTotBAmt + ",TAMT=" + nTotTAmt + ",IAMT=" + nTotIAmt + ",";
                SQL = SQL + ComNum.VBLF + " OLDBAMT=" + nBAmt[0] + ",OLDTAMT=" + nTAmt[0] + ",OLDIAMT=" + nIAmt[0] + ",";
                SQL = SQL + ComNum.VBLF + " BAMT3=" + nBAmt[1] + ",TAMT3=" + nTAmt[1] + ",IAMT3=" + nIAmt[1] + ",";
                SQL = SQL + ComNum.VBLF + " BAMT4=" + nBAmt[2] + ",TAMT4=" + nTAmt[2] + ",IAMT4=" + nIAmt[2] + ",";
                SQL = SQL + ComNum.VBLF + " BAMT5=" + nBAmt[3] + ",TAMT5=" + nTAmt[3] + ",IAMT5=" + nIAmt[3] + ",";
                SQL = SQL + ComNum.VBLF + " SUDATE=TO_DATE('" + ArgSuDate + "','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + " SUDATE3=TO_DATE('" + VB.Trim(strSuDate[0]) + "','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + " SUDATE4=TO_DATE('" + VB.Trim(strSuDate[1]) + "','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + " SUDATE5=TO_DATE('" + VB.Trim(strSuDate[2]) + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "WHERE SUCODE='" + VB.Trim(ArgSuCode) + "' ";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                Cursor.Current = Cursors.Default;
                return rtVal;
            }

            if (intRowAffected != 0)
            {
                clsPublic.GstrMsgList = ArgSuCode + "BAS_SUT에 그룹(묶음) 수가를" + ComNum.VBLF;
                clsPublic.GstrMsgList = clsPublic.GstrMsgList + "UPDATE 도중에 오류가 발생함";
                ComFunc.MsgBox(clsPublic.GstrMsgList, "오류발생");
                return rtVal;
            }

            rtVal = "OK";
            return rtVal;
        }

        private int Gesan_IlbanAmt(int ArgBAmt, string argBun, string ArgSugbE, string ArgSugbF, string ArgSugbJ = "")
        {
            int nIAmt = 0;

            //'진찰료,입원료는 보험가,일반가 동일하게 처리함
            if (Convert.ToInt32(argBun) <= Convert.ToInt32("10"))
            {
                return ArgBAmt;
            }


            //'비급여수가(식대(74)-종합건진(84)는 보험가,일반가 동일하게 처리함
            if (Convert.ToInt32(argBun) >= Convert.ToInt32("74"))
            {
                return ArgBAmt;
            }


            //'내복약,외용약품의 일반가 계산
            if ((argBun == "11" || argBun == "12") && ArgSugbE == "0")
            {
                nIAmt = Gesan_IlbanAmt_YAK(ArgBAmt, ArgSugbF);
            }
            //'주사약 일반가 계산
            else if (argBun == "20" && ArgSugbE == "0")
            {
                nIAmt = Gesan_IlbanAmt_JUSA(ArgBAmt, ArgSugbF);
            }
            //'기타 일반수가를 계산
            else
            {
                nIAmt = Gesan_IlbanAmt_ETC(ArgBAmt, ArgSugbE, ArgSugbJ);
            }

            return nIAmt;
        }

        private int Gesan_IlbanAmt_YAK(int ArgBAmt, string ArgSugbF)   //'내복약,외용약품의 일반가 계산
        {
            int nIAmt = 0;

            //'비급여수가 100,000원이상은 보험가,일반가 동일하게 처리함
            if (ArgSugbF == "1" && ArgBAmt >= 100000)
            {
                nIAmt = ArgBAmt;
                return nIAmt;
            }

            if (ArgBAmt < 11)
            {
                nIAmt = ArgBAmt * 5;
            }
            else if (ArgBAmt < 51)
            {
                nIAmt = ArgBAmt * 4;
            }
            else if (ArgBAmt < 101)
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 3.5);
            }
            else if (ArgBAmt < 500)
            {
                nIAmt = ArgBAmt * 3;
            }
            else if (ArgBAmt < 1001)
            {
                nIAmt = ArgBAmt * 2;
            }
            else
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 1.5);
            }
            return nIAmt;
        }



        private int Gesan_IlbanAmt_JUSA(int ArgBAmt, string ArgSugbF)   //'주사약제 일반수가 계산
        {
            int nIAmt = 0;

            //'비급여수가 100,000원이상은 보험가,일반가 동일하게 처리함
            if (ArgSugbF == "1" && ArgBAmt >= 100000)
            {
                nIAmt = ArgBAmt;
                return nIAmt;
            }

            if (ArgBAmt < 501)
            {
                nIAmt = ArgBAmt * 5;
            }
            else if (ArgBAmt < 1001)
            {
                nIAmt = ArgBAmt * 4;
            }
            else if (ArgBAmt < 3001)
            {
                nIAmt = ArgBAmt * 3;
            }
            else if (ArgBAmt < 5001)
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 2.5);
            }
            else if (ArgBAmt < 10001)
            {
                nIAmt = ArgBAmt * 2;
            }
            else
            {
                nIAmt = Convert.ToInt32(ArgBAmt * 1.5);
            }
            return nIAmt;
        }



        private int Gesan_IlbanAmt_ETC(int ArgBAmt, string ArgSugbE, string ArgSugbJ)    //'기타수가 일반가 계산
        {
            int nIAmt = 0;

            //'행위료이면 보험수가 * 보험병원가산율 * 2
            if (ArgSugbE == "1") nIAmt = Convert.ToInt32((ArgBAmt * 1.25) * 2);
            //'재료대이면 보험수가의 2배
            if (ArgSugbE != "1") nIAmt = ArgBAmt * 2;
            //'10원보다 크면 10원미만 절사
            //'외부의뢰검사 는 절사 않함
            if (ArgSugbJ != "9" && ArgSugbJ != "8")
            {
                if (nIAmt > 10)
                {
                    nIAmt = nIAmt / 10;
                    nIAmt = nIAmt * 10;
                }
            }
            return nIAmt;
        }

        private void SS_Clear(FarPoint.Win.Spread.SheetView Spd)
        {
            for (int i = 0; i < Spd.RowCount; i++)
            {
                for (int j = 0; j < Spd.ColumnCount; j++)
                {
                    Spd.Cells[i, j].Text = "";
                }
            }
        }

        private void btnSaveJob_Click(object sender, EventArgs e)
        {

        }

        private bool saveJob()
        {
            string strBCode = "";
            int nPrice = 0;
            int nJobCNT = 0;
            string strOK = "";

            int nCount = 0;
            int nRow = 0;
            string strDate = "";

            bool rtVal = false;
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인



                strDate = "2017-07-21";

                clsPublic.GstrMsgList = "변경할 수가 적용일자가 정말로 " + "일이" + ComNum.VBLF;
                clsPublic.GstrMsgList = clsPublic.GstrMsgList + "맞습니까? 만일 오류날짜를 지정하면" + ComNum.VBLF;
                clsPublic.GstrMsgList = clsPublic.GstrMsgList + "다시 복구가 불가능 합니다. ";

                if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                {
                    return rtVal;
                }

                nCount = 0;
                nRow = 0;

                SQL = " SELECT  A.SUGBJ, B.CODE, B.PRICE1 PRICE1, B.LPRICE1 LPRICE1     ";
                SQL = SQL + ComNum.VBLF + "  FROM VIEW_SUGA_CODE A, EDI_SUGA B";
                SQL = SQL + ComNum.VBLF + " WHERE A.EDIDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND  SUCODE = 'Q0257'";
                SQL = SQL + ComNum.VBLF + "   AND  A.BCODE = B.CODE";
                SQL = SQL + ComNum.VBLF + "   AND A.DELDATE IS NULL ";
                SQL = SQL + ComNum.VBLF + "   AND  A.BAMT <> 0 ";
                SQL = SQL + ComNum.VBLF + "    AND JONG ='1' ";     //'수가만 일괄 변경 작업함               
                SQL = SQL + ComNum.VBLF + " GROUP BY  A.SUGBJ, B.CODE, B.PRICE1, B.LPRICE1  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    dt.Dispose();
                    dt = null;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtVal;
                }

                ss1_Sheet1.RowCount = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBCode = dt.Rows[i]["Code"].ToString().Trim();

                    if (dt.Rows[i]["SugbJ"].ToString().Trim() == "9" || dt.Rows[i]["SugbJ"].ToString().Trim() == "8")    //'외부의뢰는 의원수가로 변경 2008년1월1일부터
                    {
                        nPrice = Convert.ToInt32(VB.Val(dt.Rows[i]["LPrice1"].ToString().Trim()));
                    }
                    else
                    {
                        nPrice = Convert.ToInt32(VB.Val(dt.Rows[i]["Price1"].ToString().Trim()));
                    }

                    nCount = nCount + 1;
                    //txtDate.Text = "  ";
                    //txtDate.Text = nCount;
                    txtPrice.Text = strBCode;

                    //nJobCNT = BCode_SUGA_UPDATE_WORK(strDate, strBCode, nPrice, "");

                    if (BCode_SUGA_UPDATE_WORK(strDate, strBCode, nPrice, "", ref nJobCNT) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    if (nJobCNT == -2 || nJobCNT == -1)
                    {
                        ss1_Sheet1.RowCount = ss1_Sheet1.RowCount + 1;
                        ss1_Sheet1.Cells[i, 0].Text = strBCode;
                        ss1_Sheet1.Cells[i, 1].Text = Convert.ToString(nJobCNT);
                        ss1_Sheet1.Cells[i, 2].Text = Convert.ToString(nPrice);
                    }
                    else
                    {
                        ss1_Sheet1.RowCount = ss1_Sheet1.RowCount + 1;
                        ss1_Sheet1.Cells[i, 0].Text = strBCode;
                        ss1_Sheet1.Cells[i, 1].Text = Convert.ToString(nJobCNT);
                        ss1_Sheet1.Cells[i, 2].Text = Convert.ToString(nPrice);
                    }
                }

                dt.Dispose();
                dt = null;


                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                rtVal = true;
                return rtVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtVal;
            }
        }

        private bool BCode_SUGA_UPDATE_WORK(string ArgDate, string ArgBCode, int ArgPrice, string argGBN, ref int nJobCNT)
        {
            bool rtVal = false;
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            int nBAmt = 0;
            int nTAmt = 0;
            int nIAmt = 0;            
            int nJemsu = 0;
            int nBPrice = 0;
            int nSAmt = 0;

            string strOK = "";

            //' 표준코드를 기준으로 해당 수가코드를 SELECT
            //'   -삭제된것은 작업을 안함
            //'   -수가변경일이후에 이미 변경된 것은 작업을 안함
            SQL = "SELECT SUCODE,SUNEXT,BUN,NU,GBN,SUGBA,SUGBB,SUGBC,SUGBD,SUGBE,SUGBF,";
            SQL = SQL + ComNum.VBLF + " SUGBG,SUGBH,SUGBI,SUGBJ,SUGBK,SUGBL,SUGBM,SUGBN,SUGBO,";
            SQL = SQL + ComNum.VBLF + " BAMT,TAMT,IAMT, BCODE, TO_CHAR(EDIDATE,'YYYY-MM-DD') EDIDATE,EDIJONG,";
            SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE,'YYYY-MM-DD') SUDATE,OLDBAMT,OLDTAMT,OLDIAMT,";
            SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE3,'YYYY-MM-DD') SUDATE3,BAMT3,TAMT3,IAMT3,";
            SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE4,'YYYY-MM-DD') SUDATE4,BAMT4,TAMT4,IAMT4,";
            SQL = SQL + ComNum.VBLF + " TO_CHAR(SUDATE5,'YYYY-MM-DD') SUDATE5,BAMT5,TAMT5,IAMT5,";
            SQL = SQL + ComNum.VBLF + " SUHAM,SUNAMEK,TROWID,NROWID, OLDBCODE,OLDGESU,  ";
            SQL = SQL + ComNum.VBLF + " TO_CHAR(EDIDATE3,'YYYY-MM-DD') EDIDATE3, GESU3, BCODE3,  ";
            SQL = SQL + ComNum.VBLF + " TO_CHAR(EDIDATE4,'YYYY-MM-DD') EDIDATE4, GESU4, BCODE4,  ";
            SQL = SQL + ComNum.VBLF + " TO_CHAR(EDIDATE5,'YYYY-MM-DD') EDIDATE5, GESU5, BCODE5  ";
            SQL = SQL + ComNum.VBLF + " FROM VIEW_SUGA_CODE ";
            SQL = SQL + ComNum.VBLF + "WHERE RTRIM(BCODE) = '" + ArgBCode + "' ";
            SQL = SQL + ComNum.VBLF + "  AND (SUDATE IS NULL OR SUDATE <= TO_DATE('" + ArgDate + "','YYYY-MM-DD')) ";
            SQL = SQL + ComNum.VBLF + "  AND DELDATE IS NULL  ";
            SQL = SQL + ComNum.VBLF + "  AND NOT (GBN='T' AND SUGBA > '1') ";   //'그룹코드의 BAS_SUT는 제외;
            SQL = SQL + ComNum.VBLF + "  AND BAMT <> 0 ";
            SQL = SQL + ComNum.VBLF + "  AND BUN NOT IN ('71') ";   //'심사과장 초음파수가 제외요청                ;
            SQL = SQL + ComNum.VBLF + "ORDER BY SUCODE,SUNEXT,GBN DESC ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return rtVal;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Cursor.Current = Cursors.Default;

                nJobCNT = -2;
                return rtVal;
            }

            nJobCNT = 0;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                //'읽은 자료를 변수에 저장
                TSC.SuCode = dt.Rows[i]["SuCode"].ToString().Trim();
                TSC.SuNext = dt.Rows[i]["SuNext"].ToString().Trim();
                TSC.Bun = dt.Rows[i]["Bun"].ToString().Trim();
                TSC.Nu = dt.Rows[i]["Nu"].ToString().Trim();
                TSC.Gbn = dt.Rows[i]["Gbn"].ToString().Trim();
                TSC.DelDate = "";
                TSC.SugbA = dt.Rows[i]["SugbA"].ToString().Trim();
                TSC.SugbB = dt.Rows[i]["SugbB"].ToString().Trim();
                TSC.SugbC = dt.Rows[i]["SugbC"].ToString().Trim();
                TSC.SugbD = dt.Rows[i]["SugbD"].ToString().Trim();
                TSC.SugbE = dt.Rows[i]["SugbE"].ToString().Trim();
                TSC.SugbF = dt.Rows[i]["SugbF"].ToString().Trim();
                TSC.SugbG = dt.Rows[i]["SugbG"].ToString().Trim();
                TSC.SugbH = dt.Rows[i]["SugbH"].ToString().Trim();
                TSC.SugbI = dt.Rows[i]["SugbI"].ToString().Trim();
                TSC.SugbJ = dt.Rows[i]["SugbJ"].ToString().Trim();
                TSC.SugbK = dt.Rows[i]["SugbK"].ToString().Trim();
                TSC.SugbL = dt.Rows[i]["SugbL"].ToString().Trim();
                TSC.SugbM = dt.Rows[i]["SugbM"].ToString().Trim();
                TSC.SugbN = dt.Rows[i]["SugbN"].ToString().Trim();
                TSC.SugbO = dt.Rows[i]["SugbO"].ToString().Trim();
                //'보험수가
                TSC.BAmt[0] = Convert.ToInt32(VB.Val(dt.Rows[i]["BAmt"].ToString().Trim()));
                TSC.BAmt[1] = Convert.ToInt32(VB.Val(dt.Rows[i]["OldBAmt"].ToString().Trim()));
                TSC.BAmt[2] = Convert.ToInt32(VB.Val(dt.Rows[i]["BAmt3"].ToString().Trim()));
                TSC.BAmt[3] = Convert.ToInt32(VB.Val(dt.Rows[i]["BAmt4"].ToString().Trim()));
                TSC.BAmt[4] = Convert.ToInt32(VB.Val(dt.Rows[i]["BAmt5"].ToString().Trim()));
                //'자보수가
                TSC.TAmt[0] = Convert.ToInt32(VB.Val(dt.Rows[i]["TAmt"].ToString().Trim()));
                TSC.TAmt[1] = Convert.ToInt32(VB.Val(dt.Rows[i]["OldTAmt"].ToString().Trim()));
                TSC.TAmt[2] = Convert.ToInt32(VB.Val(dt.Rows[i]["TAmt3"].ToString().Trim()));
                TSC.TAmt[3] = Convert.ToInt32(VB.Val(dt.Rows[i]["TAmt4"].ToString().Trim()));
                TSC.TAmt[4] = Convert.ToInt32(VB.Val(dt.Rows[i]["TAmt5"].ToString().Trim()));
                //'일반수가
                TSC.IAmt[0] = Convert.ToInt32(VB.Val(dt.Rows[i]["IAmt"].ToString().Trim()));
                TSC.IAmt[1] = Convert.ToInt32(VB.Val(dt.Rows[i]["OldIAmt"].ToString().Trim()));
                TSC.IAmt[2] = Convert.ToInt32(VB.Val(dt.Rows[i]["IAmt3"].ToString().Trim()));
                TSC.IAmt[3] = Convert.ToInt32(VB.Val(dt.Rows[i]["IAmt4"].ToString().Trim()));
                TSC.IAmt[4] = Convert.ToInt32(VB.Val(dt.Rows[i]["IAmt5"].ToString().Trim()));
                //'변경일자
                TSC.SuDate[0] = dt.Rows[i]["SuDate"].ToString().Trim();
                TSC.SuDate[1] = dt.Rows[i]["SuDate3"].ToString().Trim();
                TSC.SuDate[2] = dt.Rows[i]["SuDate4"].ToString().Trim();
                TSC.SuDate[3] = dt.Rows[i]["SuDate5"].ToString().Trim();
                //'표준코드
                TSC.BCode = dt.Rows[i]["BCode"].ToString().Trim();
                TSC.Gesu = Convert.ToDouble(VB.Val(dt.Rows[i]["SuHam"].ToString().Trim()));
                TSC.EdiDate = dt.Rows[i]["EdiDate"].ToString().Trim();
                TSC.OldBCode = dt.Rows[i]["OldBCode"].ToString().Trim();
                TSC.OldGesu = Convert.ToDouble(VB.Val(dt.Rows[i]["OldGesu"].ToString().Trim()));
                //'명칭 및 ROWID
                TSC.SuNameK = dt.Rows[i]["SuNameK"].ToString().Trim();
                TSC.TRowid = dt.Rows[i]["TRowid"].ToString().Trim();
                TSC.NRowid = dt.Rows[i]["NRowid"].ToString().Trim();

                //'환산단위를 기준으로 보험수가를 계산
                if (TSC.Gesu == 0) TSC.Gesu = 1;
                if (TSC.Gesu == 1)
                {
                    nBAmt = ArgPrice;
                }
                else
                {
                    nBAmt = Convert.ToInt32(ArgPrice * TSC.Gesu);
                }

                //'외부의뢰검사 10%가산
                if (dt.Rows[i]["SugbJ"].ToString().Trim() == "9" || dt.Rows[i]["SugbJ"].ToString().Trim() == "8")
                {                    
                    nBAmt = (int)Convert.ToDouble(nBAmt * 1.1);
                }

                //'자보수가(보험수가와 동일함)
                nTAmt = nBAmt;
                //'일반수가를 계산
                nIAmt = Gesan_IlbanAmt(nBAmt, TSC.Bun, TSC.SugbE, TSC.SugbF);


                //'약제 상한액은 항상 현재 수가의 표준계수를 읽도록 처리 하였습니다.                
                READ_EDI_SUGA(ArgBCode, "", dt.Rows[i]["EDIJONG"].ToString().Trim());

                //'수가,보험등재약,협약가의 단가를 Move
                if (VB.Trim(TES.JDate1) != "" && Convert.ToDateTime(TES.JDate1) <= Convert.ToDateTime(ArgDate))
                {
                    nBPrice = Convert.ToInt32(VB.Val(TES.Price1));
                }
                else if (VB.Trim(TES.JDate2) != "" && Convert.ToDateTime(TES.JDate2) <= Convert.ToDateTime(ArgDate))
                {
                    nBPrice = Convert.ToInt32(VB.Val(TES.Price2));
                }                    
                else if (VB.Trim(TES.JDate3) != "" && Convert.ToDateTime(TES.JDate3) <= Convert.ToDateTime(ArgDate))
                {
                    nBPrice = Convert.ToInt32(VB.Val(TES.Price3));
                }                    
                else if (VB.Trim(TES.JDate4) != "" && Convert.ToDateTime(TES.JDate4) <= Convert.ToDateTime(ArgDate))
                {
                    nBPrice = Convert.ToInt32(VB.Val(TES.Price4));
                }                    
                else if (VB.Trim(TES.JDate5) != "" && Convert.ToDateTime(TES.JDate5) <= Convert.ToDateTime(ArgDate))
                {
                    nBPrice = Convert.ToInt32(VB.Val(TES.Price5));
                }

                nSAmt = GeSan_DrugAmt(nBPrice, nBAmt, ArgDate);
                    
                strOK = "OK";


                //'If ArgGBN = "약제상한구입신고" Then ' "구입신고"
                //'   If nBPrice < nBAmt Then nBAmt = nBPrice '" strOK = "NO" '약제상한액은 표준코드 보다 크면  표준코드로 금액 변경
                //'End If

                if (argGBN == "DRUG")
                {
                    //'변경일자,보;험수가,일반수가가 변경할 내용과 동일하면 작업을 안함
                    if (TSC.BAmt[0] == nBAmt && TSC.SuDate[0] == ArgDate) strOK = "NO";
                }
                else
                {
                    //'변경일자,보;험수가,일반수가가 변경할 내용과 동일하면 작업을 안함
                    if (TSC.BAmt[0] == nBAmt) strOK = "NO";
                }
                

                if (strOK == "OK")
                {
                    if (BCode_Suga_Update_SUB(ArgDate, nBAmt, nTAmt, nIAmt, ref nJobCNT) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return rtVal;
                    }
                }
            }

            rtVal = true;
            return rtVal;
        }

        private int GeSan_DrugAmt(int ArgBPrice, int ArgBAmt, string ArgBDate)
        {            
            if (Convert.ToDateTime(ArgBDate) >= Convert.ToDateTime("2012-01-01"))
            {
                return 0;
                //'2012-02-01 부터 약제 상한 적용않함
            }

            //'절사            
            if ((ArgBPrice - ArgBAmt) < 0)
            {
                return 0;
            }
            else
            {
                return VB.Fix(ArgBPrice - ArgBAmt);
            }            
        }

    
        private void READ_EDI_SUGA(string ArgCode, string argSUNEXT = "", string ArgJong = "")  //'EDI 표준수가를 READ
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "";
                SQL = SQL + ComNum.VBLF + "";

                SQL = "      SELECT ROWID VROWID,CODE VCODE,JONG VJONG,";
                SQL = SQL + "    PNAME VPNAME,BUN VBUN,DANWI1 VDANWI1,";
                SQL = SQL + "    DANWI2 VDANWI2,SPEC VSPEC,COMPNY VCOMPNY,";
                SQL = SQL + "    EFFECT VEFFECT,GUBUN VGUBUN,DANGN VDANGN,";
                SQL = SQL + "    TO_CHAR(JDATE1,'YYYY-MM-DD') VJDATE1,PRICE1 VPRICE1,";
                SQL = SQL + "    TO_CHAR(JDATE2,'YYYY-MM-DD') VJDATE2,PRICE2 VPRICE2,";
                SQL = SQL + "    TO_CHAR(JDATE3,'YYYY-MM-DD') VJDATE3,PRICE3 VPRICE3,";
                SQL = SQL + "    TO_CHAR(JDATE4,'YYYY-MM-DD') VJDATE4,PRICE4 VPRICE4,";
                SQL = SQL + "    TO_CHAR(JDATE5,'YYYY-MM-DD') VJDATE5,PRICE5 VPRICE5 ";
                SQL = SQL + " FROM KOSMOS_PMPA.EDI_SUGA ";
                SQL = SQL + "WHERE CODE = '" + VB.Trim(ArgCode) + "' ";
                //'표준코드 30050010이 산소,실구입재료 2개가 존재함

                if (ArgJong != "")
                {
                    if (ArgJong == "8")
                    {
                        SQL = SQL + " AND JONG='8' ";
                    }
                    else
                    {
                        SQL = SQL + " AND JONG<>'8' ";
                    }
                }
                else
                {
                    switch (ArgCode)
                    {
                        case "N0041001":
                        case "N0041002":
                        case "N0041003":
                        case "N0021001":
                        case "30050010":
                        case "J5010001":
                        case "C2302005":
                        case "N0051010":
                            if (argSUNEXT == VB.Trim(ArgCode))
                            {
                                SQL = SQL + " AND JONG='8' ";
                            }
                            else
                            {
                                SQL = SQL + " AND JONG<>'8' ";
                            }
                            break;
                        default:
                            break;
                    }
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    TES.ROWID = dt.Rows[0]["vROWID"].ToString().Trim();
                    TES.Code = dt.Rows[0]["vCode"].ToString().Trim();
                    TES.Jong = dt.Rows[0]["vJong"].ToString().Trim();
                    TES.Pname = dt.Rows[0]["vPname"].ToString().Trim();
                    TES.Bun = dt.Rows[0]["vBun"].ToString().Trim();
                    TES.Danwi1 = dt.Rows[0]["vDanwi1"].ToString().Trim();
                    TES.Danwi2 = dt.Rows[0]["vDanwi2"].ToString().Trim();
                    TES.Spec = dt.Rows[0]["vSpec"].ToString().Trim();
                    TES.COMPNY = dt.Rows[0]["vCompny"].ToString().Trim();
                    TES.Effect = dt.Rows[0]["vEffect"].ToString().Trim();
                    TES.Gubun = dt.Rows[0]["vGubun"].ToString().Trim();
                    TES.Dangn = dt.Rows[0]["vDangn"].ToString().Trim();
                    TES.JDate1 = dt.Rows[0]["vJDate1"].ToString().Trim();
                    TES.Price1 = dt.Rows[0]["vPrice1"].ToString().Trim();
                    TES.JDate2 = dt.Rows[0]["vJDate2"].ToString().Trim();
                    TES.Price2 = dt.Rows[0]["vPrice2"].ToString().Trim();
                    TES.JDate3 = dt.Rows[0]["vJDate3"].ToString().Trim();
                    TES.Price3 = dt.Rows[0]["vPrice3"].ToString().Trim();
                    TES.JDate4 = dt.Rows[0]["vJDate4"].ToString().Trim();
                    TES.Price4 = dt.Rows[0]["vPrice4"].ToString().Trim();
                    TES.JDate5 = dt.Rows[0]["vJDate5"].ToString().Trim();
                    TES.Price5 = dt.Rows[0]["vPrice5"].ToString().Trim();
                }
                else
                {
                    TES.ROWID = ""; TES.Code = ""; TES.Jong = "";
                    TES.Pname = ""; TES.Bun = ""; TES.Danwi1 = "";
                    TES.Danwi2 = ""; TES.Spec = ""; TES.COMPNY = "";
                    TES.Effect = ""; TES.Gubun = ""; TES.Dangn = "";
                    TES.JDate1 = ""; TES.Price1 = "0";
                    TES.JDate2 = ""; TES.Price2 = "0";
                    TES.JDate3 = ""; TES.Price3 = "0";
                    TES.JDate4 = ""; TES.Price4 = "0";
                    TES.JDate5 = ""; TES.Price5 = "0";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }
    }
}
