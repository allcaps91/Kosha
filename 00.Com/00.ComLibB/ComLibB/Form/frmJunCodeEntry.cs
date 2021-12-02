using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmJunCodeEntry : Form
    {
        /// Class Name      : ComLibB.dll
        /// File Name       : frmJunCodeEntry.cs
        /// Description     : 준용코드 산출식 등록
        /// Author          : 김효성
        /// Create Date     : 2017-06-20
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// VB\basic\busuga\BuSuga13.frm => frmJunCodeEntry.cs 으로 변경함
        /// </history>
        /// <seealso> 
        /// VB\basic\busuga\FrmJunCodeEntry.frm(BuSuga13)
        /// </seealso>
        /// <vbp>
        /// default : VB\basic\busuga\busuga.vbp
        /// </vbp>

        string GstrBcode = "";
        int result = 0;
        
        //TODO: 구조체 배열로 선언됨 수정일 필요
        public struct TABLEEDISUGA
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
            public int Price1;
            public string JDate2;
            public int Price2;
            public string JDate3;
            public int Price3;
            public string JDate4;
            public int Price4;
            public string JDate5;
            public int Price5;

        }

        TABLEEDISUGA t = new TABLEEDISUGA();

        public frmJunCodeEntry()
        {
            InitializeComponent();
        }

        public frmJunCodeEntry(string strBcode)
        {
            InitializeComponent();
            GstrBcode = strBcode;
        }

        private void frmJunCodeEntry_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtsuga.Text = "";
            lblsugahelp.Text = "";
            ssView.Enabled = false;
            btnCancel.Enabled = false;

            if (GstrBcode != "")
            {
                txtsuga.Text = GstrBcode;
                SendKeys.Send("{Enter}");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            frmSearchBCode frm = new frmSearchBCode ();
            frm.Show ();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ssView_Sheet1.Cells[3, 0].ColumnSpan = 3;
            ssView_Sheet1.Cells[3, 0].Text = "합계금액";

            ssView.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            lblsugahelp.Text = "";
            txtsuga.Text = "";
            txtsuga.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData ();

            ssView_Sheet1.Cells[3, 0].ColumnSpan = 3;
            ssView_Sheet1.Cells[3, 0].Text = "합계금액";

            ssView.Enabled = true;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            lblsugahelp.Text = "";
            txtsuga.Text = "";
            txtsuga.Focus();
        }

        private bool SaveData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인
            bool rtnVal = false;

            int i = 0;
            string strBcode = "";
            string strQty = "";
            string strNameE = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            strNameE = "";

            for (i = 1; i <= 3; i++)
            {
                strBcode = ssView_Sheet1.Cells[i +1, 0].Text.Trim();
                strQty = ssView_Sheet1.Cells[i +1, 1].Text.Trim();

                if (strBcode != "" && VB.Val(strQty) != 0)
                {
                    strNameE = strNameE + ComFunc.LeftH (strBcode + VB.Space (9) , 9);
                    strNameE = strNameE + ComFunc.LeftH (strQty + VB.Space (6) , 6);
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "UPDATE BAS_SUN SET SuNameE = '" + strNameE + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + txtsuga.Text + "' ";

                SqlErr = clsDB.ExecuteNonQuery (SQL , ref intRowAffected , clsDB.DbCon);

                clsDB.setCommitTran (clsDB.DbCon);
                ComFunc.MsgBox ("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran (clsDB.DbCon);
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void READEDISUGA (string ArgCode, string ArgSuNext,string ArgJong)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "      SELECT ROWID vROWID,Code vCode,Jong vJong,";
                SQL = SQL + ComNum.VBLF + "    Pname vPname,Bun vBun,Danwi1 vDanwi1,";
                SQL = SQL + ComNum.VBLF + "    Danwi2 vDanwi2,Spec vSpec,Compny vCompny,";
                SQL = SQL + ComNum.VBLF + "    Effect vEffect,Gubun vGubun,Dangn vDangn,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(JDate1,'YYYY-MM-DD') vJDate1,Price1 vPrice1,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(JDate2,'YYYY-MM-DD') vJDate2,Price2 vPrice2,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(JDate3,'YYYY-MM-DD') vJDate3,Price3 vPrice3,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(JDate4,'YYYY-MM-DD') vJDate4,Price4 vPrice4,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(JDate5,'YYYY-MM-DD') vJDate5,Price5 vPrice5 ";
                SQL = SQL + ComNum.VBLF + " FROM EDI_SUGA ";
                SQL = SQL + ComNum.VBLF + "WHERE Code = '" + ArgCode.Trim() + "' ";

                if (ArgJong != "")
                {
                    if (ArgJong == "8")
                    {
                        SQL = SQL + ComNum.VBLF + " AND Jong='8' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND Jong<>'8' ";
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
                            if (ArgSuNext == ArgCode.Trim ()) SQL = SQL + ComNum.VBLF + " AND Jong='8' ";
                            else SQL = SQL + ComNum.VBLF + " AND Jong<>'8' ";
                            break;
                    }
                }

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    //ComFunc.MsgBox ("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count == 1)
                {
                    t.ROWID = dt.Rows[0]["vROWID"].ToString().Trim();
                    t.Code = dt.Rows[0]["vCode"].ToString().Trim();
                    t.Jong = dt.Rows[0]["vJong"].ToString().Trim();
                    t.Pname = dt.Rows[0]["vPname"].ToString().Trim();
                    t.Bun = dt.Rows[0]["vBun"].ToString().Trim();
                    t.Danwi1 = dt.Rows[0]["vDanwi1"].ToString().Trim();
                    t.Danwi2 = dt.Rows[0]["vDanwi2"].ToString().Trim();
                    t.Spec = dt.Rows[0]["vSpec"].ToString().Trim();
                    t.COMPNY = dt.Rows[0]["vCompny"].ToString().Trim();
                    t.Effect = dt.Rows[0]["vEffect"].ToString().Trim();
                    t.Gubun = dt.Rows[0]["vGubun"].ToString().Trim();
                    t.Dangn = dt.Rows[0]["vDangn"].ToString().Trim();
                    t.JDate1 = dt.Rows[0]["vJDate1"].ToString().Trim();
                    t.Price1 = Convert.ToInt32(VB.Val(dt.Rows[0]["vPrice1"].ToString().Trim()));
                    t.JDate2 = dt.Rows[0]["vJDate2"].ToString().Trim();
                    t.Price2 = Convert.ToInt32(VB.Val(dt.Rows[0]["vPrice2"].ToString().Trim()));
                    t.JDate3 = dt.Rows[0]["vJDate3"].ToString().Trim();
                    t.Price3 = Convert.ToInt32(VB.Val(dt.Rows[0]["vPrice3"].ToString().Trim()));
                    t.JDate4 = dt.Rows[0]["vJDate4"].ToString().Trim();
                    t.Price4 = Convert.ToInt32(VB.Val(dt.Rows[0]["vPrice4"].ToString().Trim()));
                    t.JDate5 = dt.Rows[0]["vJDate5"].ToString().Trim();
                    t.Price5 = Convert.ToInt32(VB.Val(dt.Rows[0]["vPrice5"].ToString().Trim()));
                }
                else
                {
                    t.ROWID = "";
                    t.Pname = "";
                    t.Danwi2 = "";
                    t.Effect = "";
                    t.JDate1 = "";
                    t.JDate2 = "";
                    t.JDate3 = "";
                    t.JDate4 = "";
                    t.JDate5 = "";
                    t.Code = "";
                    t.Bun = "";
                    t.Spec = "";
                    t.Gubun = "";
                    t.Jong = "";
                    t.Danwi1 = "";
                    t.COMPNY = "";
                    t.Dangn = "";
                    t.Price1 = 0;
                    t.Price2 = 0;
                    t.Price3 = 0;
                    t.Price4 = 0;
                    t.Price5 = 0;
                }
                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
            
        }

        private void ssView_EditModeOff(object sender, EventArgs e)
        {

            string strBcode = "";
            string strQty = "";

            strBcode = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 0].Text;
            strQty = ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 1].Text;

            BCodeChange(strBcode);
            TotAmtDisplay();
        }
        private void BCodeChange(string strBcode)
        {
            int nBAmt=0;

            READEDISUGA(strBcode, "","");

            if (t.Pname == "")
            {
                ComFunc.MsgBox("표준코드에 등록 안됨", "확인");
                ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 1].Text = "";
            }
            ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 2].Text = Convert.ToString(t.Price1);
            ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 3].Text = Convert.ToString(nBAmt);
            ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 4].Text = VB.Right(t.JDate1, 8);
            ssView_Sheet1.Cells[ssView_Sheet1.ActiveRowIndex, 5].Text = t.Pname + "" + t.Bun + "" + t.Danwi1 + t.Danwi2;

            return;
        }

        private void TotAmtDisplay()
        {
            int i = 0;
            int nTotAmt = 0;

            for (i=0; i<3; i++)
            {
                nTotAmt = nTotAmt + Convert.ToInt32(VB.Val(ssView_Sheet1.Cells[i, 3].Text));
            }
            ssView_Sheet1.Cells[3, 3].Text = Convert.ToInt32(nTotAmt).ToString().Trim();
        }

        private void txtsuga_KeyDown(object sender, KeyEventArgs e)
        {
            int i = 0;
            int nTotAmt = 0;
            int nBAmt =0;
            string strData = "";
            string strBcode = "";
            string strQty = "";
            string strEdiDate = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                if (e.KeyCode != Keys.Enter)
                {
                    return;
                }

                txtsuga.Text = VB.UCase(txtsuga.Text).Trim();

                if (txtsuga.Text == "")
                {
                    return;
                }

                SQL = "SELECT SuNameK,SuNameE,Bcode,OldBCode, ";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(EdiDate,'YYYY-MM-DD') EdiDate ";
                SQL = SQL + ComNum.VBLF + " FROM BAS_SUN ";
                SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + txtsuga.Text + "' ";

                SqlErr = clsDB.GetDataTable (ref dt , SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox ("해당 코드는 준용수가가 아닙니다" , "확인");
                    clsDB.SaveSqlErrLog (SqlErr , SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("수가코드가 등록 안 됨");
                    return;
                }

                lblsugahelp.Text = dt.Rows [0] ["SUNAMEK"].ToString ().Trim ();

                if (dt.Rows[0]["Bcode"].ToString().Trim() != "JJJJJJ" && dt.Rows[0]["OldBcode"].ToString().Trim() != "JJJJJJ")
                {
                    dt.Dispose ();
                    dt = null;
                    ComFunc.MsgBox ("해당 코드는 준용수가가 아닙니다.");
                    return;
                }
                
                ssView.Enabled = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;

                nTotAmt = 0;
                strData = VB.Left(dt.Rows[0]["SuNameE"].ToString().Trim() + VB.Space(45), 45);
                strEdiDate = dt.Rows[0]["EdiDate"].ToString().Trim();

                //ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight (-1 , ComNum.SPDROWHT);

                for (i = 1; i <= 3; i++)
                {
                    //strBcode = VB.Mid(strData, i * 15 - 14, 9).Trim();
                    //strQty = VB.Mid(strData, i * 15 - 5, 6).Trim();
                    strBcode = VB.Mid (strData , i * 15 - 14 , 9).Trim();
                    strQty = VB.Mid (strData , i * 15 - 5 , 6).Trim();

                    READEDISUGA (strBcode, txtsuga.Text.Trim(),"");

                    if (t.ROWID != "")
                    {
                        ssView_Sheet1.Cells[i - 1, 0].Text = strBcode;
                        ssView_Sheet1.Cells[i - 1, 1].Text = Convert.ToString(VB.Val(strQty));
                        ssView_Sheet1.Cells[i - 1, 2].Text = Convert.ToString(t.Price1);
                        nBAmt = VB.Fix(Convert.ToInt32(t.Price1 * VB.Val (strQty) + 0.59));
                        ssView_Sheet1.Cells[i - 1, 3].Text = Convert.ToString(nBAmt);
                        ssView_Sheet1.Cells[i - 1, 4].Text = VB.Right(t.JDate1, 8);

                        if (dt.Rows[0]["Bcode"].ToString().Trim() != "JJJJJJ" && dt.Rows[0]["OldBcode"].ToString().Trim() == "JJJJJJ")
                        {
                            if (Convert.ToDateTime(t.JDate1) >= Convert.ToDateTime(strEdiDate))
                            {
                                ssView_Sheet1.Cells[i - 1, 2].Text = Convert.ToString(t.Price2);
                                nBAmt = VB.Fix(Convert.ToInt32(t.Price2 * Convert.ToDouble(strQty) + 0.59));
                                ssView_Sheet1.Cells[i - 1, 3].Text = Convert.ToString(nBAmt);
                                ssView_Sheet1.Cells[i - 1, 4].Text = Convert.ToString(VB.Right(t.JDate2, 8));
                            }
                        }
                        ssView_Sheet1.Cells[i - 1, 5].Text = t.Pname + " " + t.Bun + " " + t.Danwi1 + t.Danwi2;
                        nTotAmt = nTotAmt + nBAmt;
                    }
                }

                dt.Dispose();
                dt = null;

                ssView_Sheet1.Cells[3, 3].Text = Convert.ToString(nTotAmt);
                ssView.Focus();
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox (ex.Message);
                clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}