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
    public partial class frmGumeUpdate : Form
    {
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

        private string GstrHelpCode = "";

        public frmGumeUpdate()
        {
            InitializeComponent();
        }

        public frmGumeUpdate(string GstrHelpCode)
        {
            InitializeComponent();
            this.GstrHelpCode = GstrHelpCode;
        }

        private void frmGumeUpdate_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtViewSuga.Text = "";
            txtViewJep.Text = "";
            list1.Items.Clear();
            list2.Items.Clear();

            SCREEN_CLEAR();

            cboViewBun.Items.Clear();
            cboViewBun.Items.Add("**.전체");
            cboViewBun.Items.Add("11.소모성기구");
            cboViewBun.Items.Add("12.수술재료대");
            cboViewBun.Items.Add("13.안과IOL");
            cboViewBun.Items.Add("14.검사실");
            cboViewBun.Items.Add("16.인공신장실");
            cboViewBun.Items.Add("17.CT,MRI필름");
            cboViewBun.Items.Add("18.필름,현상액");
            cboViewBun.Items.Add("19.동위원소,핵");
            cboViewBun.Items.Add("1A.기타소모품");
            cboViewBun.Items.Add("1B.치과재료대");
            cboViewBun.SelectedIndex = 0;

            setCboSugaBun();

            cboSunap.Items.Clear();
            cboSunap.Items.Add("1.수납");
            cboSunap.Items.Add("2.선수납");
            cboSunap.Items.Add("3.수납안함");
            cboSunap.SelectedIndex = -1;

            if (GstrHelpCode != "")
            {
                txtJepCode.Text = GstrHelpCode;
                JepCode_Display(GstrHelpCode);
                GstrHelpCode = "";
            }
        }

        private void setCboSugaBun()
        {

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                SQL = "SELECT CODE,NAME FROM KOSMOS_PMPA.BAS_BUN ";
                SQL = SQL + ComNum.VBLF + "WHERE JONG='1' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY CODE ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    cboSugaBun.Items.Clear();
                    cboSugaBun.Items.Add("**.전체");
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboSugaBun.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                    }
                    cboSugaBun.SelectedIndex = 0;
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

        private void SCREEN_CLEAR()
        {
            txtJepCode.Text = "";
            lblGuUnit.Text = "";
            lblSuUnit.Text = "";
            lblJepName.Text = "";
            lblGeName.Text = "";
            lblPrice.Text = "";
            lblBunName.Text = "";
            lblJepDelDate.Text = "";
            cboSunap.Text = "";
            txtSuCode.Text = "";
            lblSuNameK.Text = "";
            txtGesu.Text = "";
            chkReUse.Checked = false;


            lblSuDelDate.Text = "";
            lblSugbN.Text = "";
            lblSugbF.Text = "";
            lblBCode.Text = "";
            lblBName.Text = "";
            lblGuAmt.Text = "";
            lblBAmt.Text = "";
            lblChaAmt.Text = "";


            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            panJep.Enabled = true;
            panViewSuga.Enabled = false;
        }

        private void JepCode_Display(string ArgCode)
        {
            double nGesu = 0;
            int nJepAmt = 0;
            int nSugaAmt = 0;
            string strSuCode = "";
            string strBCode = "";

            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                //'물품코드의 상세내역을 Display
                SQL = "SELECT JEPNAME,COVQTY,COVUNIT,GELCODE,SUCODE,BCODE,BGESU,GEACODE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE,GBSUNAP,BUSE_UNIT,";
                SQL = SQL + ComNum.VBLF + " GBREUSE,BUSE_GESU,ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.ORD_JEP ";
                SQL = SQL + ComNum.VBLF + "WHERE JEPCODE='" + VB.Trim(ArgCode) + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("물품코드에 등록이 않됨");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblJepName.Text = " " + dt.Rows[0]["JepName"].ToString().Trim();
                    lblGuUnit.Text = dt.Rows[0]["CovUnit"].ToString().Trim();
                    lblSuUnit.Text = dt.Rows[0]["Buse_Unit"].ToString().Trim();
                    lblGeName.Text = READ_AIS_LTD(dt.Rows[0]["GelCode"].ToString().Trim());
                    lblJepDelDate.Text = dt.Rows[0]["DelDate"].ToString().Trim();
                    switch (dt.Rows[0]["DelDate"].ToString().Trim())
                    {
                        case "11":
                            lblBunName.Text = "소모성기구";
                            break;
                        case "12":
                            lblBunName.Text = "수술재료대";
                            break;
                        case "13":
                            lblBunName.Text = "안과IOL";
                            break;
                        case "14":
                            lblBunName.Text = "검사실";
                            break;
                        case "15":
                            lblBunName.Text = "중앙공급실";
                            break;
                        case "16":
                            lblBunName.Text = "인공신장실";
                            break;
                        case "17":
                            lblBunName.Text = "CT,MRI필름";
                            break;
                        case "18":
                            lblBunName.Text = "필름,현상액";
                            break;
                        case "19":
                            lblBunName.Text = "동위원소,핵";
                            break;
                        case "1A":
                            lblBunName.Text = "기타소모품";
                            break;
                        case "1B":
                            lblBunName.Text = "치과재료대";
                            break;
                        default:
                            lblBunName.Text = "기타";
                            break;
                    }
                    strSuCode = dt.Rows[0]["SuCode"].ToString().Trim();
                    strBCode = dt.Rows[0]["BCode"].ToString().Trim();
                    lblBCode.Text = VB.UCase(strBCode);
                    if (VB.Trim(lblBCode.Text) != "")
                    {
                        READ_EDI_SUGA(VB.Trim(lblBCode.Text));
                        lblBName.Text = TES.Pname;
                    }
                    //'소독후 재사용 여부
                    chkReUse.Checked = false;
                    if (dt.Rows[0]["GbReUse"].ToString().Trim() == "Y") chkReUse.Checked = true;

                    txtSuCode.Text = strSuCode;
                    txtGesu.Text = dt.Rows[0]["Buse_Gesu"].ToString().Trim();
                    //'수납여부
                    switch (dt.Rows[0]["GbSunap"].ToString().Trim())
                    {
                        case "1":
                            cboSunap.SelectedIndex = 0;
                            break;
                        case "2":
                            cboSunap.SelectedIndex = 1;
                            break;
                        case "3":
                            cboSunap.SelectedIndex = 2;
                            break;
                        default:
                            cboSunap.SelectedIndex = -1;
                            break;
                    }
                }

                dt.Dispose();
                dt = null;

                //'물품의 최종 구매단가를 READ
                SQL = "SELECT COVQTY,PRICE FROM KOSMOS_ADM.ORD_HIS ";
                SQL = SQL + ComNum.VBLF + "WHERE JEPCODE='" + ArgCode + "' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY LASTDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("물품코드에 등록이 않됨");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(VB.Val(dt.Rows[0]["CovQty"].ToString().Trim())) != 0 && Convert.ToInt32(VB.Val(dt.Rows[0]["Price"].ToString().Trim())) != 0)
                    {
                        lblPrice.Text = VB.Format(VB.Val(dt.Rows[0]["Price"].ToString().Trim()) / VB.Val(dt.Rows[0]["CovQty"].ToString().Trim()), "###,###,##0");
                    }
                    else
                    {
                        lblPrice.Text = "";
                    }
                }

                dt.Dispose();
                dt = null;

                //'수가내역을 READ & Display
                if (strSuCode != "")
                {
                    Suga_Display(strSuCode);
                }
                else
                {
                    nGesu = VB.Val(VB.Format(txtGesu.Text));
                    if (nGesu == 0) nGesu = 1;
                    txtGesu.Text = Convert.ToString(nGesu);
                    nJepAmt = Convert.ToInt32(VB.Val(VB.Format(lblPrice.Text)) / nGesu);
                    nSugaAmt = Convert.ToInt32(VB.Val(VB.Format(lblBAmt.Text)));
                    lblGuAmt.Text = VB.Format(nJepAmt, "###,###,##0");
                    lblChaAmt.Text = VB.Format(nSugaAmt - nJepAmt, "###,###,##0");
                }

                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                panSuga.Enabled = true;
                panViewSuga.Enabled = true;
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

        private void READ_EDI_SUGA(string ArgCode, string argSUNEXT = "", string ArgJong = "")  //'EDI 표준수가를 READ
        {

            //int i = 0;
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


        /// <summary>
        /// 거래처명칭
        /// </summary>
        /// <param name="argCode">LTD Code</param>
        /// <returns></returns>
        private string READ_AIS_LTD(string argCode)
        {
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";
            DataTable dt = null;


            SQL = "";
            SQL += ComNum.VBLF + "SELECT Name FROM KOSMOS_ADM.AIS_LTD ";
            SQL += ComNum.VBLF + "WHERE LtdCode='" + argCode.Trim() + "' ";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["Name"].ToString();
                }
                else
                {
                    rtnVal = "";
                }

                return rtnVal;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        private void Suga_Display(string ArgCode)
        {
            //int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            double nGesu = 0;
            int nJepAmt = 0;
            int nSugaAmt = 0;

            try
            {
                SQL = "SELECT TO_CHAR(A.DELDATE,'YYYY-MM-DD') DELDATE,A.BAMT,B.SUNAMEK,B.BCODE,";
                SQL = SQL + ComNum.VBLF + " A.SUGBF,B.SUGBN ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUT A,KOSMOS_PMPA.BAS_SUN B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.SUCODE='" + VB.Trim(ArgCode) + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.SUNEXT=B.SUNEXT(+) ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    lblSuDelDate.Text = dt.Rows[0]["DelDate"].ToString().Trim();
                    lblBAmt.Text = VB.Format(VB.Val(dt.Rows[0]["BAmt"].ToString().Trim()), "###,###,##0");
                    lblSuNameK.Text = " " + dt.Rows[0]["SuNameK"].ToString().Trim();
                    lblBCode.Text = dt.Rows[0]["BCode"].ToString().Trim();
                    if (dt.Rows[0]["SugbN"].ToString().Trim() != "0") lblSugbN.Text = "선수납";
                    if (dt.Rows[0]["SugbF"].ToString().Trim() != "0") lblSugbF.Text = "비급여";
                }
                else
                {
                    lblSuNameK.Text = " -< 수가코드 오류 >-";
                }

                dt.Dispose();
                dt = null;


                if (VB.Trim(lblBCode.Text) != "")
                {
                    READ_EDI_SUGA(VB.Trim(lblBCode.Text));
                    lblBName.Text = TES.Pname;
                }

                nGesu = VB.Val(VB.Format(txtGesu.Text));
                if (nGesu == 0) nGesu = 1;
                txtGesu.Text = Convert.ToString(nGesu);
                nJepAmt = Convert.ToInt32(VB.Val(VB.Format(lblPrice.Text)) / nGesu);
                nSugaAmt = Convert.ToInt32(VB.Val(VB.Format(lblBAmt.Text)));
                lblGuAmt.Text = VB.Format(nJepAmt, "###,###,##0");
                lblChaAmt.Text = VB.Format(nSugaAmt - nJepAmt, "###,###,##0");
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //int nREAD;
            string strError = "";
            string strBun = "";
            string strData = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                strBun = VB.Left(cboViewBun.Text, 2);
                txtViewJep.Text = VB.Trim(txtViewJep.Text);

                //'자료를 SELECT
                SQL = "SELECT JepCode,JepName FROM KOSMOS_ADM.ORD_JEP ";
                if (strBun == "**")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE GEACODE IN ('11','12','13','14','16','17','18','19','1A','1B') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE GEACODE = '" + strBun + "' ";
                }

                if (txtViewJep.Text != "") SQL = SQL + ComNum.VBLF + " AND JEPNAME LIKE '%" + txtViewJep.Text + "%' ";
                if (chkGbJinUse.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND JEPCODE IN (SELECT JEPCODE FROM KOSMOS_PMPA.OPR_BUSEJEPUM ";
                    SQL = SQL + ComNum.VBLF + "     WHERE CODEGBN='2' ";  //'관리과물품
                    SQL = SQL + ComNum.VBLF + "     GROUP BY JEPCODE) ";
                }
                else
                {
                    if (chkDel.Checked == false) SQL = SQL + ComNum.VBLF + " AND DELDATE IS NULL ";
                }


                if (optJepSORT_0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY JEPCODE ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY JEPNAME ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                list1.Items.Clear();
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (optView_1.Checked == false)
                        {
                            strData = VB.Left(dt.Rows[i]["JepCode"].ToString().Trim() + VB.Space(9), 9);
                            strData = strData + dt.Rows[i]["JepName"].ToString().Trim();
                            list1.Items.Add(strData);
                        }
                        else
                        {
                            if (Code_Error_Check(dt.Rows[i]["JepCode"].ToString().Trim(), ref strError) == true)
                            {
                                strData = VB.Left(dt.Rows[i]["JepCode"].ToString().Trim() + VB.Space(9), 9);
                                strData = strData + dt.Rows[i]["JepName"].ToString().Trim();
                                list1.Items.Add(strData);
                            }
                        }
                    }
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

        private void btnSearchSugaCode_Click(object sender, EventArgs e)
        {
            string strBun = "";
            string strData = "";

            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

                strBun = VB.Left(cboSugaBun.Text, 2);
                strData = VB.Trim(txtViewSuga.Text);
                if (strBun == "**" && strData == "")
                {
                    ComFunc.MsgBox("수가코드 전체는 조회가 불가능합니다.", "오류");
                    return;
                }

                //'해당 자료를 SELECT
                SQL = "SELECT A.SUCODE,B.SUNAMEK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUT A,KOSMOS_PMPA.BAS_SUN B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.SUNEXT=B.SUNEXT(+) ";
                if (strBun != "**") SQL = SQL + ComNum.VBLF + " AND A.BUN='" + strBun + "' ";
                if (strData != "") SQL = SQL + ComNum.VBLF + "  AND UPPER(B.SUNAMEK) LIKE '%" + VB.UCase(strData) + "%' ";
                if (chkSugaDel.Checked == false) SQL = SQL + ComNum.VBLF + " AND A.DELDATE IS NULL ";
                if (optSugaSORT_0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY A.SUCODE ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "ORDER BY B.SUNAMEK ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                list2.Items.Clear();
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strData = VB.Left(dt.Rows[i]["SuCode"].ToString().Trim() + VB.Space(9), 9);
                        strData = strData + dt.Rows[i]["SuNameK"].ToString().Trim();
                        list2.Items.Add(strData);
                    }
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

        private void list1_DoubleClick(object sender, EventArgs e)
        {
            string strJEPCODE = "";

            SCREEN_CLEAR();
            strJEPCODE = VB.Trim(VB.Left(list1.Text, 8));
            txtJepCode.Text = strJEPCODE;
            JepCode_Display(strJEPCODE);

            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            panSuga.Enabled = true;
            panViewSuga.Enabled = true;
        }

        private void list2_DoubleClick(object sender, EventArgs e)
        {
            string strSuCode = "";

            strSuCode = VB.Trim(VB.Left(list2.Text, 8));
            txtSuCode.Text = strSuCode;
            Suga_Display(strSuCode);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            list1.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveData();
        }

        private bool saveData()
        {
            bool rtVal = false;
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strGbSunap = "";
            //string strGbJinUse = "";
            //string strJepBCode = "";
            string strGbReUse = "";

            if (VB.Trim(cboSunap.Text) == "")
            {
                ComFunc.MsgBox("수납구분이 공란입니다.", "오류");
                return rtVal;
            }

            strGbSunap = VB.Left(cboSunap.Text, 1);

            if (lblSugbN.Text == "선수납" && strGbSunap == "1")
            {
                ComFunc.MsgBox("수납구분이 수가의 수납구분과 틀림", "확인");
                return rtVal;
            }

            if (VB.Trim(txtSuCode.Text) == "" && strGbSunap != "3")
            {
                ComFunc.MsgBox("수납을 하여야 하나 수가코드가 공란입니다.", "오류");
                return rtVal;
            }

            strGbReUse = "N";
            if (chkReUse.Checked == true) strGbReUse = "Y";

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인

                ORD_JEP_HIS_INSERT("2", VB.Trim(txtJepCode.Text)); //'변경전

                SQL = "UPDATE KOSMOS_ADM.ORD_JEP SET ";
                SQL = SQL + ComNum.VBLF + " SUCODE='" + VB.Trim(VB.UCase(txtSuCode.Text)) + "',";
                SQL = SQL + ComNum.VBLF + " GBSUNAP='" + strGbSunap + "',";
                SQL = SQL + ComNum.VBLF + " GBREUSE='" + strGbReUse + "',";
                SQL = SQL + ComNum.VBLF + " BUSE_GESU=" + VB.Val(txtGesu.Text) + " ";
                SQL = SQL + ComNum.VBLF + " WHERE JEPCODE='" + VB.Trim(txtJepCode.Text) + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                ORD_JEP_HIS_INSERT("3", VB.Trim(txtJepCode.Text)); //'변경전

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();
                list1.Focus();

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

        private bool Code_Error_Check(string ArgCode, ref string strERROR)
        {
            string strSuCode = "";
            string strBCode = "";
            string strGbSunap = "";
            int nGumePrice = 0;
            
            int nSuga_BAmt = 0;
            string strSuga_DelDate = "";
            string strSuga_BCode = "";
            string strSuga_SugbN = "";
            string strSuga_SugbF = "";
            
            //int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인
                                
                //'자료를 SELECT
                SQL = "SELECT JEPCODE,JEPNAME,BUSE_UNIT,BUSE_GESU,TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE,";
                SQL = SQL + ComNum.VBLF + " GBSUNAP,PRICE,SUCODE,BCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.ORD_JEP ";
                SQL = SQL + ComNum.VBLF + "WHERE JEPCODE='" + ArgCode +"' ";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    strSuCode = dt.Rows[0]["SuCode"].ToString().Trim();
                    strBCode = dt.Rows[0]["BCode"].ToString().Trim();
                    strGbSunap = dt.Rows[0]["GbSunap"].ToString().Trim();
                    strERROR = "";

                    //'최종 구매단가를 READ
                    SQL = "SELECT LASTDATE,PRICE,COVQTY ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.ORD_HIS ";
                    SQL = SQL + ComNum.VBLF + "WHERE JEPCODE='" + dt.Rows[0]["JEPCODE"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY LASTDATE DESC ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return false;
                    }

                    nGumePrice = 0;
                    if (dt2.Rows.Count > 0)
                    {
                        nGumePrice = Convert.ToInt32(VB.Val(dt2.Rows[0]["Price"].ToString().Trim()));
                        if (Convert.ToInt32(VB.Val(dt2.Rows[0]["CovQty"].ToString().Trim())) > 0)
                        {
                            nGumePrice = nGumePrice / Convert.ToInt32(VB.Val(dt2.Rows[0]["CovQty"].ToString().Trim()));
                            if (Convert.ToInt32(VB.Val(dt2.Rows[0]["Buse_Gesu"].ToString().Trim())) != 0)
                            {
                                nGumePrice = nGumePrice / Convert.ToInt32(VB.Val(dt2.Rows[0]["Buse_Gesu"].ToString().Trim()));
                            }                            
                        }                        
                    }

                    dt2.Dispose();
                    dt2 = null;

                    //'수가코드를 읽음
                    nSuga_BAmt = 0;
                    strSuga_DelDate = "";
                    strSuga_BCode = "";

                    if (strSuCode != "")
                    {
                        SQL = "SELECT A.BAMT,TO_CHAR(A.DELDATE,'YYYY-MM-DD') DELDATE,B.BCODE,B.SUGBN,A.SUGBF ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUT A,KOSMOS_PMPA.BAS_SUN B ";
                        SQL = SQL + ComNum.VBLF + "WHERE A.SUCODE='" + strSuCode + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND A.SUNEXT=B.SUNEXT(+) ";

                        SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return false;
                        }
                        if (dt2.Rows.Count == 0)
                        {
                            strERROR = strERROR + "수가없음,";
                        }
                        else
                        {
                            nSuga_BAmt = Convert.ToInt32(VB.Val(dt2.Rows[0]["BAmt"].ToString().Trim()));
                            strSuga_DelDate = dt2.Rows[0]["DelDate"].ToString().Trim();
                            strSuga_BCode = dt2.Rows[0]["BCode"].ToString().Trim(); 
                            strSuga_SugbN = dt2.Rows[0]["SugbN"].ToString().Trim(); 
                            strSuga_SugbF = dt2.Rows[0]["SugbF"].ToString().Trim();    
                        }
                        dt2.Dispose();
                        dt2 = null;                        
                    }

                    //'수납구분이 수납,선수납인 물품코드가 수가코드가 없으면 오류
                    if (dt.Rows[0]["GbSunap"].ToString().Trim() != "3" && strSuCode == "") strERROR = strERROR + "수가누락,";
                    //'수납구분이 수납,선수납,수불이 아닌경우 오류 처리
                    if (VB.Val(dt.Rows[0]["GbSunap"].ToString().Trim()) < VB.Val("1") || VB.Val(dt.Rows[0]["GbSunap"].ToString().Trim()) > VB.Val("3")) strERROR = strERROR + "수납구분,";
                    //'삭제일자 오류 점검
                    if (dt.Rows[0]["DelDate"].ToString().Trim() == "" && strSuga_DelDate != "") strERROR = strERROR + "수가삭제,";
                    //'표준코드,선수납 오류 점검
                    if( strSuCode != "" )
                    {
                        if (strBCode != strSuga_BCode) strERROR = strERROR + "표준코드,";

                        if (strSuga_SugbN == "1" && strGbSunap != "2")
                        {
                            strERROR = strERROR + "선수납,";
                        }
                        else if (strGbSunap == "2" && strSuga_SugbN != "1")
                        {
                            strERROR = strERROR + "선수납,";
                        }                        
                    }
                                                                
                    //'단가오류점검
                    if( strSuCode != "" && strSuga_SugbF == "0" )
                    {
                        if (nSuga_BAmt != nGumePrice) strERROR = strERROR + "단가차이,";
                    }

                    //'수불 수가점검(구매코드는 수불로 되어있으나 수가코드가 등록된것)
                    if (strGbSunap == "3" && strSuCode != "") strERROR = strERROR + "수불수가,";
                }
                else
                {
                    
                    strERROR = "물품코드 없음";                    
                }

                Cursor.Current = Cursors.Default;
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


            if (strERROR == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //'물품코드 변경 History INSERT
        private bool ORD_JEP_HIS_INSERT(string strJobGbn, string strJEPCODE)
        {
            bool rtVal = false;
            //int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return rtVal; ; //권한 확인

                SQL = "INSERT INTO KOSMOS_ADM.ORD_JEP_HIS (";
                SQL = SQL + ComNum.VBLF + "JOBDATE,JOBSABUN,JOBGBN,JEPCODE,BUN,JEPNAME,COVQTY,COVUNIT,JQTY1,JQTY2,";
                SQL = SQL + ComNum.VBLF + "GELCODE,PRICE,SUCODE,GEACODE,GBSUBUL,GBGUME,GBJAJE,GBSUSUL,GBEXCHANGE,GBREMARK,";
                SQL = SQL + ComNum.VBLF + "GBAUTOCHULGO,GBBAL,GIBONBAL,IPGOILSU,USEBUSE1,USEBUSE2,PAPERCODE,PAPERSOMO,";
                SQL = SQL + ComNum.VBLF + "LAP,DELDATE,UNIT,GEABUN,GBSUNAP,DEPTCODE,GYU,PICFILE,CATNO,REMARK,GBCSR,CSRNAME,";
                SQL = SQL + ComNum.VBLF + "BCODE,BGESU,CSRRATE,GBJINUSE) ";
                SQL = SQL + ComNum.VBLF + "SELECT SYSDATE," + clsType.User.Sabun + ",'" + strJobGbn + "',JEPCODE,BUN,JEPNAME,COVQTY,COVUNIT,JQTY1,JQTY2,";
                SQL = SQL + ComNum.VBLF + "       GELCODE,PRICE,SUCODE,GEACODE,GBSUBUL,GBGUME,GBJAJE,GBSUSUL,GBEXCHANGE,GBREMARK,";
                SQL = SQL + ComNum.VBLF + "       GBAUTOCHULGO,GBBAL,GIBONBAL,IPGOILSU,USEBUSE1,USEBUSE2,PAPERCODE,PAPERSOMO,";
                SQL = SQL + ComNum.VBLF + "       LAP,DELDATE,UNIT,GEABUN,GBSUNAP,DEPTCODE,GYU,PICFILE,CATNO,REMARK,GBCSR,CSRNAME,";
                SQL = SQL + ComNum.VBLF + "       BCODE,BGESU,CSRRATE,GBJINUSE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_ADM.ORD_JEP ";
                SQL = SQL + ComNum.VBLF + " WHERE JEPCODE='" + strJEPCODE +"' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return rtVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBox("저장 하였습니다.");
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
        
    }
}
