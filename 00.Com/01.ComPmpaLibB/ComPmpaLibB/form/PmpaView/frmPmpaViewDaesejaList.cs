using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewDaesejaList.cs
    /// Description     : 대세자 대장 관리 
    /// Author          : 안정수
    /// Create Date     : 2017-10-19
    /// Update History  : 
    /// <history>     
    /// 실서버에서만 데이터가 존재하며, 저장, 삭제 등의 실제 테스트가 필요
    /// d:\psmh\OPD\wonmok\Frm대세자대장.frm(Frm대세자명단) => frmPmpaViewDaesejaList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\wonmok\Frm대세자대장.frm(Frm대세자명단)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewDaesejaList : Form
    {
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        int mnJobSabun = 0;

        public frmPmpaViewDaesejaList()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewDaesejaList(int GnJobSabun)
        {
            InitializeComponent();
            setEvent();
            mnJobSabun = GnJobSabun;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnSave.Click += new EventHandler(eBtnEvent);
            this.btnDel.Click += new EventHandler(eBtnEvent);
            this.btnNew.Click += new EventHandler(eBtnEvent);

            this.txtPANO.LostFocus += new EventHandler(eControl_LostFocus);

            this.dtp4.Leave += new EventHandler(eControl_Leave);
            this.dtp5.Leave += new EventHandler(eControl_Leave);
            this.dtp20.Leave += new EventHandler(eControl_Leave);
            this.dtp23.Leave += new EventHandler(eControl_Leave);

            this.txt1.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt10.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt11.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt12.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt13.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt14.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt15.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt16.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt17.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt18.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt19.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt24.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt2.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            //사망일
            this.dtp20.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.txt21.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt22.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            //일자
            this.dtp23.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt3.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            //생년월일
            this.dtp4.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            //대세일
            this.dtp5.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.txt6.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt7.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt8.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txt9.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtPANO.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

        }

        void eControl_Leave(object sender, EventArgs e)
        {
            if (sender == this.dtp4)
            {
                dtp5.Focus();
            }

            else if (sender == this.dtp5)
            {
                txt10.Focus();
            }

            else if (sender == this.dtp20)
            {
                txt22.Focus();
            }

            else if (sender == this.dtp23)
            {
                btnSave.Focus();
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txt1 || sender == this.txt11 || sender == this.txt12 || sender == this.txt13 || sender == this.txt14
                || sender == this.txt15 || sender == this.txt17 || sender == this.txt18 || sender == this.txt19
                || sender == this.txt2 || sender == this.txt21 || sender == this.txt3 || sender == this.txt6
                || sender == this.txt7 || sender == this.txt8 || sender == this.txt9 || sender == this.txtPANO || sender == this.txt24
                || sender == this.dtp4 || sender == this.dtp5 || sender == this.dtp20 || sender == this.dtp23)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }

            else if (sender == this.txt16)
            {
                dtp20.Focus();
            }

            else if (sender == this.txt22)
            {
                dtp4.Focus();
            }

            else if (sender == this.txt10)
            {
                txt11.Focus();
            }
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if (txtPANO.Text != "")
            {
                ComFunc.SetAutoZero(txtPANO.Text, 8);
                ReadPatientInfo(txtPANO.Text);
            }
        }

        void ReadPatientInfo(string arg)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  SNAME, BIRTH, ZIPCODE1, ZIPCODE2, JUSO";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND PANO = '" + arg + "'";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                txt1.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                dtp4.Text = dt.Rows[0]["BIRTH"].ToString().Trim();
                txt7.Text = clsBagage.ZipCodeToJuso(clsDB.DbCon, dt.Rows[0]["ZIPCODE1"].ToString().Trim(), dt.Rows[0]["ZIPCODE1"].ToString().Trim() + dt.Rows[0]["JUSO"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  DIAGNOSIS";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_MASTER";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND PANO = '" + arg + "'";
            SQL += ComNum.VBLF + "ORDER BY IPDNO DESC";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                txt15.Text = dt.Rows[0]["DIAGNOSIS"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
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

            optSort1.Checked = true;

            dtpFDate.Text = Convert.ToDateTime(clsPmpaPb.GstrSysDate).AddDays(-60).ToShortDateString();
            dtpTDate.Text = clsPmpaPb.GstrSysDate;

            eGetData();
            optSort1.Checked = true;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnNew)
            {
                btnNew_Click();
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
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eSaveData();
            }

            else if (sender == this.btnDel)
            {
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eDelData();
            }
        }

        void btnNew_Click()
        {
            clsPmpaPb.GstrSEQNO = "";
            txtPANO.Focus();
        }

        void eSaveData()
        {
            string strROWID = "";
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;



            createData(this, ComNum.DB_ERP + "WONMOK_LIST1");

            if (clsPmpaPb.GstrSEQNO == "")
            {
                clsPmpaPb.GstrSEQNO = readSeqno(ComNum.DB_ERP + "WONMOK_LIST1").ToString();
            }

            else
            {
                strROWID = readROWID(clsPmpaPb.GstrSEQNO, ComNum.DB_ERP + "WONMOK_LIST1");
                if (strROWID != "")
                {
                    dataDelete(strROWID, ComNum.DB_ERP + "WONMOK_LIST1");
                }
            }

            SqlErr = clsDB.ExecuteNonQuery(createInsertSql(strROWID, ComNum.DB_ERP + "WONMOK_LIST1", clsPmpaPb.GstrSEQNO, "WRITEDATE, WRITESABUN", "SYSDATE, " + mnJobSabun), ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            eGetData();
        }

        /// <summary>
        /// Create Date : 2017-10-20
        /// Author : 안정수
        /// <seealso cref="bagage.bas : dataDelete"/>
        /// </summary>
        /// <param name="ArgROWID"></param>
        /// <param name="ArgTable"></param>
        public void dataDelete(string ArgROWID, string ArgTable)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;




            if (ArgROWID != "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO " + ArgTable + "_HISTORY";
                SQL += ComNum.VBLF + "SELECT * FROM " + ArgTable;
                SQL += ComNum.VBLF + "WHERE ROWID = '" + ArgROWID + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                SQL = "";
                SQL += ComNum.VBLF + "DELETE " + ArgTable;
                SQL += ComNum.VBLF + "WHERE ROWID = '" + ArgROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            }
        }


        /// <summary>
        /// Create Date : 2017-10-20
        /// Author : 안정수
        /// <seealso cref="bagage.bas : createInsertSql"/>
        /// </summary>
        /// <param name="ArgROWID"></param>
        /// <param name="ArgTable"></param>
        /// <param name="ArgSeqno"></param>
        /// <param name="argColName"></param>
        /// <param name="argValue"></param>
        /// <returns></returns>
        public string createInsertSql(string ArgROWID, string ArgTable, string ArgSeqno = "", string argColName = "", string argValue = "")
        {
            string rtnVal = "";
            int i = 0;
            int j = 0;
            int nSeqNo = 0;

            string SQL = "";

            string cColumn = "";
            string cValue = "";

            if (clsPmpaPb.GbCreateData != true)
            {
                ComFunc.MsgBox("데이터 형성을 먼저 하시기 바랍니다.");
                return rtnVal;
            }

            if (VB.UBound(clsPmpaPb.GstrColName) != VB.UBound(clsPmpaPb.GstrColValue))
            {
                ComFunc.MsgBox("프로그램에 에러가 있습니다. 전산실로 연락주시기 바랍니다");
                return rtnVal;
            }

            cColumn = "";

            for (i = 1; i <= VB.UBound(clsPmpaPb.GstrColName); i++)
            {
                cColumn += clsPmpaPb.GstrColName[i];
                if (i != VB.UBound(clsPmpaPb.GstrColName))
                {
                    cColumn += ",";
                }

                if (i % 4 == 0)
                {
                    cColumn += ComNum.VBLF + ComNum.VBLF;
                }
            }

            cValue = "";

            for (i = 1; i <= VB.UBound(clsPmpaPb.GstrColName); i++)
            {
                cValue += returnSqlValue(clsPmpaPb.GstrColValue[i]);


                if (i != VB.UBound(clsPmpaPb.GstrColValue))
                {
                    cValue += ",";
                }

                if (i % 4 == 0)
                {
                    cValue += ComNum.VBLF + ComNum.VBLF;
                }
            }

            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ArgTable + "(";

            if (ArgSeqno != "")
            {
                SQL += ComNum.VBLF + "SEQNO,";
            }

            if (argColName != "")
            {
                SQL += ComNum.VBLF + "argColName, ";
            }

            SQL += ComNum.VBLF + cColumn + ") VALUES ( ";

            if (ArgSeqno != "")
            {
                SQL += ComNum.VBLF + ArgSeqno + ",";
            }

            if (argValue != "")
            {
                SQL += ComNum.VBLF + argValue + ",";
            }

            SQL += ComNum.VBLF + cValue + ")";

            clsPmpaPb.GbCreateData = false;

            rtnVal = SQL;

            return rtnVal;
        }


        /// <summary>
        /// Create Date : 2017-10-20
        /// Author : 안정수
        /// <seealso cref="bagage.bas : returnSqlValue"/>
        /// </summary>
        /// <param name="argValue"></param>
        /// <returns></returns>
        public string returnSqlValue(string argValue)
        {
            string rtnVal = "";

            if (VB.IsDate(argValue))
            {
                if (VB.IsNumeric(argValue))
                {
                    rtnVal = "'" + argValue + "'";
                }

                else if (argValue.StartsWith("년") || argValue.StartsWith("월"))
                {
                    rtnVal = "'" + argValue + "'";
                }

                else
                {
                    rtnVal = "TO_DATE('" + argValue + "','YYYY-MM-DD')";
                }
            }

            else
            {
                rtnVal = "'" + argValue + "'";
            }
            return rtnVal;
        }


        /// <summary>
        /// Create Date : 2017-10-20
        /// Author : 안정수
        /// <seealso cref="bagage.bas : readROWID"/>
        /// </summary>
        /// <param name="ArgSeqno"></param>
        /// <param name="ArgTable"></param>
        /// <returns></returns>
        public string readROWID(string ArgSeqno, string ArgTable)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  ROWID";
            SQL += ComNum.VBLF + "FROM " + ArgTable;
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND SEQNO = " + ArgSeqno;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["ROWID"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            return rtnVal;
        }

        public int readSeqno(string Args)
        {
            int rtnVal = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT " + Args + ".NEXTVAL CNT FROM DUAL ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = Convert.ToInt32(dt.Rows[0]["CNT"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;


            return rtnVal;
        }

        /// <summary>
        /// Create Date : 2017-10-20
        /// Author : 안정수
        /// <seealso cref="bagage.bas : createData"/>
        /// </summary>
        /// <param name="ArgForm"></param>
        /// <param name="ArgTable"></param>
        void createData(Form ArgForm, string ArgTable)
        {
            int i = 0;
            int j = 0;

            Control Ctrl;

            string[] cTagName = new string[100];
            string[] cValue = new string[100];

            clsPmpaPb.GbCreateData = false;

            for (i = 0; i < ArgForm.Controls.Count; i++)
            {
                Ctrl = ArgForm.Controls[i];

                if (Ctrl.Tag != null)
                {
                    j += 1;
                    //if(Ctrl.Tag.ToString().ToUpper() == "CLASS3")
                    //{
                    //    j = j;
                    //}

                    cTagName[j] = Ctrl.Tag.ToString().Trim().ToUpper();
                    cValue[j] = returnValue(Ctrl);
                }
            }

            if (VB.UBound(cTagName) > 1)
            {
                clsPmpaPb.GstrColName = cTagName;
                clsPmpaPb.GstrColValue = cValue;
            }

            clsPmpaPb.GbCreateData = true;
        }

        /// <summary>
        /// Create Date : 2017-10-20
        /// Author : 안정수
        /// <seealso cref="bagage.bas : returnValue"/>
        /// </summary>
        /// <param name="argCtrl"></param>
        /// <returns></returns>
        public string returnValue(Control argCtrl)
        {
            string rtnVal = "";

            if (argCtrl.GetType() == typeof(TextBox))
            {
                rtnVal = argCtrl.Text.Trim();
            }

            else if (argCtrl.GetType() == typeof(ComboBox))
            {
                rtnVal = argCtrl.Text.Trim();
            }

            else if (argCtrl.GetType() == typeof(CheckBox))
            {
                rtnVal = argCtrl.Text;
            }

            else if (argCtrl.GetType() == typeof(RadioButton))
            {
                if (argCtrl.ToString() == "True")
                {
                    rtnVal = "1";
                }
                else
                {
                    rtnVal = "0";
                }
            }

            return rtnVal; ;
        }

        void eDelData()
        {
            string strROWID = "";

            if (clsPmpaPb.GstrSEQNO == "")
            {
                ComFunc.MsgBox("저장 된 내용이 없습니다.");
            }

            else
            {
                if (MessageBox.Show("작성 된 내용을 삭제하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                strROWID = readROWID(clsPmpaPb.GstrSEQNO, ComNum.DB_ERP + "WONMOK_LIST1");

                if (strROWID != "")
                {
                    dataDelete(strROWID, ComNum.DB_ERP + "WONMOK_LIST1");
                    ComFunc.MsgBox("삭제되었습니다.");
                    eGetData();
                }
            }
        }

        void eGetData()
        {
            clsPmpaPb.GstrSEQNO = "";

            string strSDATE = "";
            string strEDATE = "";

            string strGubun = "";

            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList_Sheet1.Rows.Count = 0;

            strSDATE = dtpFDate.Text;
            strEDATE = dtpTDate.Text;

            if (optSort0.Checked == true)
            {
                strGubun = "0";
            }

            else if (optSort1.Checked == true)
            {
                strGubun = "1";
            }

            else if (optSort2.Checked == true)
            {
                strGubun = "2";
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  SEQNO, PANO, PART01, PART02, PART03, PART04, PART05, PART15, PART08, PART09, PART11, PART20";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "WONMOK_LIST1";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND PART05 >= TO_DATE('" + strSDATE + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND PART05 <= TO_DATE('" + strEDATE + "','YYYY-MM-DD')";

            switch (strGubun)
            {
                case "0":
                    SQL += ComNum.VBLF + "ORDER BY PART01 ASC";
                    break;

                case "1":
                    SQL += ComNum.VBLF + "ORDER BY PART05 ASC";
                    break;

                case "2":
                    SQL += ComNum.VBLF + "ORDER BY PART20 ASC";

                    break;
            }

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
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SEQNO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["PART01"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["PART02"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["PART03"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 5].Text = dt.Rows[i]["PART04"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = dt.Rows[i]["PART05"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["PART15"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["PART08"].ToString().Trim() + "(" + dt.Rows[i]["PART09"].ToString().Trim() + ")";
                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["PART11"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["PART20"].ToString().Trim();
                    }
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
    }
}

