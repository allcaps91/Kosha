using System;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComDbB; //DB연결

/// <summary>
/// Description : 반환값 있는 공용쿼리문
/// Author : 박병규, 김민철
/// Create Date : 2017.05.25
/// <history>
/// </history>
/// </summary>

namespace ComPmpaLibB
{
    public class clsPmpaFunc : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public void fn_ClearMemory(Form FormName)
        {
            FormName.Dispose();
            FormName = null;
            clsApi.FlushMemory();
        }

        /// <summary>
        /// 영문체크
        /// </summary>
        /// <param name="ArgStr">문자
        /// <returns></returns>
        public bool CheckEnglish(string ArgStr)
        {

            bool IsCheck = true;

            Regex engRegex = new Regex(@"[a-zA-Z]");

            bool ismatch = engRegex.IsMatch(ArgStr);

            if (!ismatch)
            {
                IsCheck = false;
            }

            return IsCheck;
        }

        public static DialogResult InputBox(string title, string promptText, ref string Value)
        {
            Form form = new Form();
            Label lblTitle = new Label();
            TextBox txtBox = new TextBox();
            Button btnOk = new Button();
            Button btnCancel = new Button();

            form.Text = title;
            lblTitle.Text = promptText;
            txtBox.Text = Value;

            btnOk.Text = "OK";
            btnCancel.Text = "Cancel";
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            lblTitle.SetBounds(9, 20, 372, 13);
            txtBox.SetBounds(12, 36, 32, 20);
            btnOk.SetBounds(278, 72, 75, 23);
            btnCancel.SetBounds(309, 72, 75, 23);

            lblTitle.AutoSize = true;
            txtBox.Anchor = txtBox.Anchor | AnchorStyles.Right;
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new System.Drawing.Size(396, 107);
            form.Controls.AddRange(new Control[] { lblTitle, txtBox, btnOk, btnCancel });
            form.ClientSize = new System.Drawing.Size(Math.Max(300, lblTitle.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            DialogResult dR = form.ShowDialog();
            Value = txtBox.Text;
            return dR;
        }


        /// <summary>
        /// 선택진료 정보 가져오기
        /// <param name="strPano">등록번호</param>
        /// <param name="strDel">삭제포함여부</param>
        /// 2017-06-28 김민철
        /// </summary>
        public DataTable Get_Bas_Select_Mst(PsmhDb pDbCon, string strPano, string strDel)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT SNAME,GUBUN,DEPTCODE,DRCODE,TO_CHAR(SDATE,'YYYY-MM-DD') SDATE,";
                SQL += ComNum.VBLF + "        TO_CHAR(EDATE,'YYYY-MM-DD') EDATE,TO_CHAR(DELDATE,'YYYY-MM-DD') DELDATE,";
                SQL += ComNum.VBLF + "        TO_CHAR(ENTDATE,'YYYY-MM-DD') ENTDATE,ENTSABUN,WORK,";
                SQL += ComNum.VBLF + "        TO_CHAR(ENTDATE2,'YYYY-MM-DD') ENTDATE2,BIGO,";
                SQL += ComNum.VBLF + "        SET1,SET2,SET3,SET4,SET5,SET6,SET7,SET8,SET9,";
                SQL += ComNum.VBLF + "        SETC1,SETC2,SETC3,SETC4,SETC5,SETC6,SETC7,SETC8,SETC9 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SELECT_MST ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND PANO = '" + strPano.Trim() + "' ";
                if (strDel.Trim() == "Y")
                {
                    SQL += ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='') ";
                }
                SQL += ComNum.VBLF + " ORDER BY Gubun,DrCode,SDate DESC ";

                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return Dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        /// <summary>
        /// 선택진료 정보 가져오기
        /// <param name="strPano">등록번호</param>
        /// <param name="strDel">삭제포함여부</param>
        /// 2017-06-15 김민철
        /// </summary>
        /// <history> clsPmpaSel 로 이관 2017.08.09 KMC</history>
        //public string Get_Bas_Select_Mst_BDate(string strPano, string strIO, string strDrCode, string strBDate)
        //{
        //    string rtnVal = "";

        //    DataTable Dt = new DataTable();
        //    string SqlErr = ""; //에러문 받는 변수

        //    try
        //    {
        //        SQL = "";
        //        SQL += ComNum.VBLF + " SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, TO_CHAR(EDATE,'YYYY-MM-DD') EDATE ";
        //        SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SELECT_MST ";
        //        SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
        //        SQL += ComNum.VBLF + "    AND PANO = '" + strPano.Trim() + "' ";
        //        SQL += ComNum.VBLF + "    AND DRCODE ='" + strDrCode + "' ";
        //        SQL += ComNum.VBLF + "    AND GUBUN ='" + strIO + "' ";
        //        SQL += ComNum.VBLF + "    AND SDate <=TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
        //        SQL += ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='') ";
        //        SQL += ComNum.VBLF + "  ORDER BY SDate DESC ";

        //        SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

        //        if (SqlErr != "")
        //        {
        //            ComFunc.MsgBox("조회중 문제가 발생했습니다");
        //            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
        //            return "";
        //        }

        //        if (Dt.Rows.Count == 0)
        //        {
        //            Dt.Dispose();
        //            Dt = null;
        //            return "";
        //        }
        //        if (Dt.Rows[0]["EDATE"].ToString().Trim() == "")
        //        {
        //            rtnVal = Dt.Rows[0]["SDATE"].ToString().Trim();
        //        }
        //        else if (string.Compare(strBDate, Dt.Rows[0]["EDATE"].ToString().Trim()) <= 0)
        //        {
        //            rtnVal = Dt.Rows[0]["SDATE"].ToString().Trim();
        //        }

        //        Dt.Dispose();
        //        Dt = null;

        //        return rtnVal;
        //    }
        //    catch (Exception ex)
        //    {
        //        ComFunc.MsgBox(ex.Message);
        //        clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
        //        return "";
        //    }
        //}

        /// <summary>
        /// 전실전과 정보 가져오기
        /// <param name="strPano">등록번호</param>
        /// <param name="strInDate">입원일자</param>
        /// 2017-06-15 김민철
        /// </summary>
        /// 

        public bool BAS_HANGAMMIX(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtSad = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            bool rtnVal = false;


            if (ArgCode == "") { return rtnVal; }


            SQL = "";
            SQL += ComNum.VBLF + " SELECT dosfullcode  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "ocs_odosage ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND doscode      = '" + ArgCode + "' ";
            SQL += ComNum.VBLF + "    AND DOSNAME LIKE '%(m)%'  ";
            //SQL += ComNum.VBLF + "    AND DOSFULLCODE LIKE '%항암%' ";
            SQL += ComNum.VBLF + "    AND DOSNAME LIKE '%항암%' ";
            SQL += ComNum.VBLF + "    AND DOSNAME LIKE '%약국조제%' ";  //2021-06-29 용법기준 변경 요청
            SqlErr = clsDB.GetDataTableEx(ref DtSad, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다" + ComNum.VBLF + SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtSad.Dispose();
                DtSad = null;
                return rtnVal;
            }

            if (DtSad.Rows.Count > 0)         //같은것이 있으면 Update
            {
                rtnVal = true;
            }

            DtSad.Dispose();
            DtSad = null;
            return rtnVal;
        }

        public DataTable Get_Ipd_Transfor(PsmhDb pDbCon, string strPano, string strInDate)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(TrsDate,'YYYY-MM-DD HH24:MI') TRSDATE,FRSPC,TOSPC,";
                SQL += ComNum.VBLF + "        FRWARD,FRROOM,FRDEPT,FRDOCTOR,TOWARD,TOROOM,TODEPT,TODOCTOR ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND PANO = '" + strPano.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND TRSDATE >=TO_DATE('" + strInDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " ORDER BY TrsDate DESC  ";

                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return Dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        /// <summary>
        /// 입원 마스터 정보 가져오기
        /// <param name="strPano">등록번호</param>
        /// <param name="strWard">병동구분</param>
        /// 2017-06-16 김민철
        /// </summary>
        /// 
        public string Read_Suga_BUN(PsmhDb pDbCon, string ArgSunext)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            strVal ="02";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT BUN ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "bas_sut ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1";
                SQL += ComNum.VBLF + "    AND Sunext = '" + ArgSunext + "' ";

                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (DtFunc.Rows.Count == 0)
                {
                    DtFunc.Dispose();
                    DtFunc = null;
                    return strVal;
                }

                if (DtFunc.Rows.Count >= 1)
                    strVal = DtFunc.Rows[0]["BUN"].ToString().Trim();
                else
                    strVal = "02";

                DtFunc.Dispose();
                DtFunc = null;

                return strVal;
            }
            catch (Exception ex)
            {
                strVal = "";
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strVal;
            }
        }
        public DataTable Get_Ipd_New_Master(PsmhDb pDbCon, string strPano, string strWard, long nIPDNO)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            //DataTable 에서 특정 칼럼만 Select해서 DataTable 만들기
            //DataTable dt2 = dt.DefaultView.ToTable(false, new string[] {"Col1", "Col2", "Col3"});
            //Column 명을 enum 으로 선언후 원하는 값만 가져오게 구현한다면 명확하게 나타낼수 있음.

            if (VB.Val(strPano) == 0)
            {
                strPano = "";
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT   IPDNO                                                       ";
                SQL += ComNum.VBLF + "          ,PANO                                                       ";
                SQL += ComNum.VBLF + "          ,SNAME                                                      ";
                SQL += ComNum.VBLF + "          ,SEX                                                        ";
                SQL += ComNum.VBLF + "          ,AGE                                                        ";
                SQL += ComNum.VBLF + "          ,BI                                                         ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(INDATE,'YYYY-MM-DD HH24:MI') INDATE                ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(OUTDATE,'YYYY-MM-DD') OUTDATE                      ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE                      ";
                SQL += ComNum.VBLF + "          ,ILSU                                                       ";
                SQL += ComNum.VBLF + "          ,GBSTS                                                      ";
                SQL += ComNum.VBLF + "          ,DEPTCODE                                                   ";
                SQL += ComNum.VBLF + "          ,DRCODE                                                     ";
                SQL += ComNum.VBLF + "          ,WARDCODE                                                   ";
                SQL += ComNum.VBLF + "          ,ROOMCODE                                                   ";
                SQL += ComNum.VBLF + "          ,WARD                                                       ";
                SQL += ComNum.VBLF + "          ,TRSCNT                                                     ";
                SQL += ComNum.VBLF + "          ,LASTTRS                                                    ";
                SQL += ComNum.VBLF + "          ,PNAME                                                      ";
                SQL += ComNum.VBLF + "          ,GBSPC                                                      ";
                SQL += ComNum.VBLF + "          ,GBKEKLI                                                    ";
                SQL += ComNum.VBLF + "          ,GBGAMEK                                                    ";
                SQL += ComNum.VBLF + "          ,GBTEWON                                                    ";
                SQL += ComNum.VBLF + "          ,FEE6                                                       ";
                SQL += ComNum.VBLF + "          ,BOHUN                                                      ";
                SQL += ComNum.VBLF + "          ,JIYUK                                                      ";
                SQL += ComNum.VBLF + "          ,GELCODE                                                    ";
                SQL += ComNum.VBLF + "          ,RELIGION                                                   ";
                SQL += ComNum.VBLF + "          ,GBCANCER                                                   ";
                SQL += ComNum.VBLF + "          ,INOUT                                                      ";
                SQL += ComNum.VBLF + "          ,OTHER                                                      ";
                SQL += ComNum.VBLF + "          ,GBDONGGI                                                   ";
                SQL += ComNum.VBLF + "          ,OGPDBUN                                                    ";
                SQL += ComNum.VBLF + "          ,ARTICLE                                                    ";
                SQL += ComNum.VBLF + "          ,GBDRG                                                      ";
                SQL += ComNum.VBLF + "          ,DRGWRTNO                                                   ";
                SQL += ComNum.VBLF + "          ,GBOLDSLIP                                                  ";
                SQL += ComNum.VBLF + "          ,JUPBONO                                                    ";
                SQL += ComNum.VBLF + "          ,FROMTRANS                                                  ";
                SQL += ComNum.VBLF + "          ,ERAMT                                                      ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(ARCDATE,'YYYY-MM-DD') ARCDATE                      ";
                SQL += ComNum.VBLF + "          ,ARCQTY                                                     ";
                SQL += ComNum.VBLF + "          ,ICUQTY                                                     ";
                SQL += ComNum.VBLF + "          ,IM180                                                      ";
                SQL += ComNum.VBLF + "          ,ILLCODE1,ILLCODE2,ILLCODE3                                 ";
                SQL += ComNum.VBLF + "          ,ILLCODE4,ILLCODE5,ILLCODE6                                 ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(TRSDATE,'YYYY-MM-DD') TRSDATE                      ";
                SQL += ComNum.VBLF + "          ,DEPT1,DEPT2,DEPT3                                          ";
                SQL += ComNum.VBLF + "          ,DOCTOR1,DOCTOR2,DOCTOR3                                    ";
                SQL += ComNum.VBLF + "          ,ILSU1,ILSU2,ILSU3                                          ";
                SQL += ComNum.VBLF + "          ,AMSET1,AMSET2,AMSET3,AMSET4,AMSET5                         ";
                SQL += ComNum.VBLF + "          ,AMSET6,AMSET7,AMSET8,AMSET9,AMSETA                         ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(IPWONTIME,'YYYY-MM-DD HH24:MI') IPWONTIME          ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(CANCELTIME,'YYYY-MM-DD HH24:MI') CANCELTIME        ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(GATEWONTIME,'YYYY-MM-DD HH24:MI') GATEWONTIME      ";
                SQL += ComNum.VBLF + "          ,GATEWONSAYU                                                ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(RDATE,'YYYY-MM-DD') RDATE                          ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(ROUTDATE,'YYYY-MM-DD') ROUTDATE                    ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(SIMSATIME,'YYYY-MM-DD HH24:MI') SIMSATIME          ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(PRINTTIME,'YYYY-MM-DD HH24:MI') PRINTTIME          ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(SUNAPTIME,'YYYY-MM-DD HH24:MI') SUNAPTIME          ";
                SQL += ComNum.VBLF + "          ,GBCHECKLIST                                                ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(MIRBUILDTIME,'YYYY-MM-DD HH24:MI') MIRBUILDTIME    ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(WARDINDATE,'YYYY-MM-DD HH24:MI') WARDINDATE        ";
                SQL += ComNum.VBLF + "          ,NUR_GBOBGY                                                 ";
                SQL += ComNum.VBLF + "          ,ROUTGBPRT                                                  ";
                SQL += ComNum.VBLF + "          ,GBINFECTION                                                ";
                SQL += ComNum.VBLF + "          ,HUHOSPITAL                                                 ";
                SQL += ComNum.VBLF + "          ,HUJIYEK                                                    ";
                SQL += ComNum.VBLF + "          ,HUSAYU                                                     ";
                SQL += ComNum.VBLF + "          ,GBFUNERAL                                                  ";
                SQL += ComNum.VBLF + "          ,DIAGNOSIS                                                  ";
                SQL += ComNum.VBLF + "          ,REMARK                                                     ";
                SQL += ComNum.VBLF + "          ,OUTDRUG                                                    ";
                SQL += ComNum.VBLF + "          ,OUTDEPT                                                    ";
                SQL += ComNum.VBLF + "          ,OP_JIPYO                                                   ";
                SQL += ComNum.VBLF + "          ,HEIGHT,WEIGHT                                              ";
                SQL += ComNum.VBLF + "          ,GBSUDAY                                                    ";
                SQL += ComNum.VBLF + "          ,PNEUMONIA                                                  ";
                SQL += ComNum.VBLF + "          ,GBGOOUT                                                    ";
                SQL += ComNum.VBLF + "          ,OGPDBUNDTL                                                 ";
                SQL += ComNum.VBLF + "          ,PREGNANT                                                   ";
                SQL += ComNum.VBLF + "          ,EMR                                                        ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(WARDINTIME,'YYYY-MM-DD HH24:MI') WARDINTIME        ";
                SQL += ComNum.VBLF + "          ,TELREMARK                                                  ";
                SQL += ComNum.VBLF + "          ,GBNIGHT                                                    ";
                SQL += ComNum.VBLF + "          ,GBEXAM                                                     ";
                SQL += ComNum.VBLF + "          ,THYROID                                                    ";
                SQL += ComNum.VBLF + "          ,JSIM_REMARK                                                ";
                SQL += ComNum.VBLF + "          ,TUBERCULOSIS                                               ";
                SQL += ComNum.VBLF + "          ,GBSPC2                                                     ";
                SQL += ComNum.VBLF + "          ,GBPT                                                       ";
                SQL += ComNum.VBLF + "          ,SECRET,SECRET_SABUN                                        ";
                SQL += ComNum.VBLF + "          ,ICUQTY2                                                    ";
                SQL += ComNum.VBLF + "          ,GBDIV                                                      ";
                SQL += ComNum.VBLF + "          ,DRGCODE                                                    ";
                SQL += ComNum.VBLF + "          ,MIILSU                                                     ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(MIARCDATE,'YYYY-MM-DD') MIARCDATE                  ";
                SQL += ComNum.VBLF + "          ,DRSABUN2                                                   ";
                SQL += ComNum.VBLF + "          ,BEDNUM                                                     ";
                SQL += ComNum.VBLF + "          ,PACS_ADT                                                   ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(JDATE,'YYYY-MM-DD') JDATE                          ";
                SQL += ComNum.VBLF + "          ,JOBSABUN                                                   ";
                SQL += ComNum.VBLF + "          ,DUR_SEND                                                   ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(SECRETINDATE,'YYYY-MM-DD HH24:MI') SECRETINDATE    ";
                SQL += ComNum.VBLF + "          ,TO_CHAR(SECRETOUTDATE,'YYYY-MM-DD HH24:MI') SECRETOUTDATE  ";
                SQL += ComNum.VBLF + "          ,KTASLEVL                                                   ";
                SQL += ComNum.VBLF + "          ,FROOM                                                      ";
                SQL += ComNum.VBLF + "          ,FROOMETC                                                   ";
                SQL += ComNum.VBLF + "          ,DOCTOR_MEMO                                                ";
                SQL += ComNum.VBLF + "          ,GBJIWON                                                    ";
                SQL += ComNum.VBLF + "          ,T_CARE                                                     ";
                SQL += ComNum.VBLF + "          ,PASS_INFO                                                  ";
              //  SQL += ComNum.VBLF + "          ,RETUN_HOSP                                                 ";
                SQL += ComNum.VBLF + "          ,FC_KTAS_HIS(PANO,TO_CHAR(INDATE,'YYYYMMDD')) KTAS_HIS      ";
                SQL += ComNum.VBLF + "          ,FC_RETURN_SIMSA(PANO,TO_CHAR(INDATE,'YYYY-MM-DD'),nvl(TO_CHAR(OUTDATE,'YYYY-MM-DD'),to_char(trunc(sysdate),'YYYY-MM-DD'))) RETUN_HOSP     ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                if (nIPDNO > 0)
                {
                    SQL += ComNum.VBLF + "    AND IPDNO = " + nIPDNO + " ";
                }
                else
                {
                    SQL += ComNum.VBLF + "    AND GBSTS NOT IN ('7') ";
                    SQL += ComNum.VBLF + "    AND ACTDATE IS NULL ";
                    if (strPano != "")
                    {
                        SQL += ComNum.VBLF + "    AND PANO = '" + strPano + "' ";
                    }
                    if (strWard != "")
                    {
                        SQL += ComNum.VBLF + "    AND WARDCODE = '" + strWard + "' ";
                    }
                }

                SQL += ComNum.VBLF + " ORDER BY InDate DESC  ";

                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return Dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        /// <summary>
        /// 입원 자격정보 TRANS 가져오기
        /// <param name="strPano">등록번호</param>
        /// <param name="strIpdNo">입원번호</param>
        /// <param name="strTrsNo">자격번호</param>
        /// 2017-06-16 김민철
        /// </summary>
        public DataTable Get_Ipd_Trans(PsmhDb pDbCon, string strPano, long nIpdNo, long nTrsNo)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            //DataTable 에서 특정 칼럼만 Select해서 DataTable 만들기
            //DataTable dt2 = dt.DefaultView.ToTable(false, new string[] {"Col1", "Col2", "Col3"});
            //Column 명을 enum 으로 선언후 원하는 값만 가져오게 구현한다면 명확하게 나타낼수 있음.

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TRSNO,IPDNO,PANO,GBIPD,INDATE,OUTDATE,ACTDATE,DEPTCODE,DRCODE,ILSU,";
                SQL += ComNum.VBLF + "        BI,KIHO,GKIHO,PNAME,GWANGE,BONRATE,GISULRATE,GBGAMEK,BOHUN,GELCODE,AMSET1,AMSET2,";
                SQL += ComNum.VBLF + "        AMSET3,AMSET4,AMSET5,AMSETB,FROMTRANS,ERAMT,JUPBONO,GBDRG,DRGWRTNO,SANGAMT,DTGAMEK,";
                SQL += ComNum.VBLF + "        OGPDBUN,ILLCODE1,ILLCODE2,ILLCODE3,ILLCODE4,ILLCODE5,ILLCODE6,AMT01,AMT02,AMT03,";
                SQL += ComNum.VBLF + "        AMT04,AMT05,AMT06,AMT07,AMT08,AMT09,AMT10,AMT11,AMT12,AMT13,AMT14,AMT15,AMT16,AMT17,";
                SQL += ComNum.VBLF + "        AMT18,AMT19,AMT20,AMT21,AMT22,AMT23,AMT24,AMT25,AMT26,AMT27,AMT28,AMT29,AMT30,AMT31,";
                SQL += ComNum.VBLF + "        AMT32,AMT33,AMT34,AMT35,AMT36,AMT37,AMT38,AMT39,AMT40,AMT41,AMT42,AMT43,AMT44,AMT45,";
                SQL += ComNum.VBLF + "        AMT46,AMT47,AMT48,AMT49,AMT50,AMT51,AMT52,AMT53,AMT54,AMT55,AMT56,AMT57,AMT58,AMT59,";
                SQL += ComNum.VBLF + "        AMT60,REMARK,ENTDATE,ENTSABUN,GBSTS,ROUTDATE,SIMSATIME,PRINTTIME,SUNAPTIME,";
                SQL += ComNum.VBLF + "        GBCHECKLIST,MIRBUILDTIME,VCODE,MSEQNO,GBSANG,AMT61,AMT62,AMT63,OGPDBUNDTL,SIMSASABUN,";
                SQL += ComNum.VBLF + "        GBILBAN2,AMT64,JSIM_SABUN,JSIM_LDATE,JSIM_SET,GBSPC,JSIM_OK,JINDTL,OGPDBUN2,";
                SQL += ComNum.VBLF + "        TEWON_SABUN,DRGCODE,DRGADC1,DRGADC2,DRGADC3,DRGADC4,DRGADC5,AMT65,AMT66,AMT67,";
                SQL += ComNum.VBLF + "        GBTAX,FCODE,KTASLEVL";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND GBSTS = '0' ";
                SQL += ComNum.VBLF + "    AND ACTDATE IS NULL ";
                if (strPano != "")
                {
                    SQL += ComNum.VBLF + "    AND PANO = '" + strPano + "' ";
                }
                if (nIpdNo > 0)
                {
                    SQL += ComNum.VBLF + "    AND IPDNO = " + nIpdNo + " ";
                }
                if (nTrsNo > 0)
                {
                    SQL += ComNum.VBLF + "    AND TRSNO = " + nTrsNo + " ";
                }

                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return Dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        /// <summary>
        /// Description : 등록번호로 환자기본정보 가져오기
        /// Author : 박병규
        /// Create Date : 2017.06.20
        /// <param name="ArgPtno">등록번호</param>
        /// </summary>
        /// <seealso cref="READ_BAS_PATIENT"/>
        public DataTable Get_BasPatient(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable DtPat = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT * ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND PANO = '" + ArgPtno + "' ";
                SqlErr = clsDB.GetDataTableEx(ref DtPat, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return DtPat;
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        /// <summary>
        /// Description : 등록번호로 포스코 기본정보 가져오기
        /// Author : 박병규
        /// Create Date : 2017.08.03
        /// <param name="ArgPtno">등록번호</param>
        /// </summary>
        /// <seealso cref=""/>
        public DataTable Get_PoscoPatient(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable DtPat = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT * ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO ";
                SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND GUBUN = '01'  order by jdate desc  ";
                SqlErr = clsDB.GetDataTableEx(ref DtPat, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return DtPat;
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        /// <summary>
        /// Description : BAS_ILLS 테이블에서 ILLCODED 코드값 가져오기
        /// Author : 박병규
        /// Create Date : 2017.07.06
        /// <param name="ArgCode">상병코드</param>
        /// <seealso cref="OUMSAD.bas : 상병2_KCD6_NEW"/>
        /// </summary>
        public String Get_KCD6(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtPF = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ILLCODED ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND ILLCODE   = '" + ArgCode + "' ";
                SQL += ComNum.VBLF + "    AND KCD6      = '*' ";
                SqlErr = clsDB.GetDataTableEx(ref DtPF, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                if (DtPF.Rows.Count != 0)
                    rtnVal = DtPF.Rows[0]["ILLCODED"].ToString().Trim();

                DtPF.Dispose();
                DtPF = null;

                return rtnVal;
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        /// <summary>
        /// Description : 원외처방전번호 가져오기
        /// Author : 박병규
        /// Create Date : 2017.07.07
        /// <param name="ArgCode">상병코드</param>
        /// </summary>
        /// <seealso cref="vb의료급여승인.bas : GET_OPD_BOHO_ODRUG"/>
        public String Get_Opd_ODrug(PsmhDb pDbCon, string ArgPtno, string ArgBdate, string ArgDeptCode)
        {
            DataTable DtPF = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = String.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SlipDate,'YYYYMMDD') SlipDate, SlipNo ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDeptCode + "' ";
            SQL += ComNum.VBLF + "    AND FLAG      <> 'D' ";       //취소(삭제)가 아닌것만
            SQL += ComNum.VBLF + "  ORDER BY SlipDate DESC,SlipNo DESC ";
            SqlErr = clsDB.GetDataTableEx(ref DtPF, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtPF.Rows.Count >= 1)
            {
                rtnVal = DtPF.Rows[0]["SLIPDATE"].ToString();
                rtnVal += string.Format("{0:00000}", Convert.ToInt64(DtPF.Rows[0]["SLIPNO"].ToString()));
            }

            DtPF.Dispose();
            DtPF = null;

            return rtnVal;
        }

        /// <summary>
        /// 입원환자 정보 읽어오기
        /// <param name="strPano">등록번호</param>
        /// <param name="nIpdNo">입원번호</param>
        /// <param name="nTrsNo">자격번호</param>
        /// 2017.06.28 김민철
        /// </summary>
        public DataTable Get_Ipd_Mst_Trs(PsmhDb pDbCon, string strPano, long nTrsNo, string strTemp)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strPano == "")
            {
                return null;
            }

            SQL = "";
            SQL += " SELECT a.TRSNO,a.IPDNO,a.PANO,a.GBIPD,a.DEPTCODE,a.DRCODE,a.ILSU, a.ROWID TROWID                                                       \r\n";
            SQL += "        ,TO_CHAR(a.INDATE,'YYYY-MM-DD') InDate, TO_CHAR(a.OUTDATE,'YYYY-MM-DD') OutDate, TO_CHAR(a.ACTDATE,'YYYY-MM-DD') ActDate        \r\n";
            SQL += "        ,a.BI,a.KIHO,a.GKIHO,a.PNAME,a.GWANGE,a.BONRATE,a.GISULRATE,a.AMSET1,a.AMSET2,a.AMSET3,a.AMSET4,a.AMSET5,a.AMSETB,a.FROMTRANS   \r\n";
            SQL += "        ,a.ERAMT,a.JUPBONO,a.GBDRG,a.DRGWRTNO,a.SANGAMT,a.DTGAMEK,a.OGPDBUN                                                             \r\n";
            SQL += "        ,a.IllCode1,a.IllCode2,a.IllCode3,a.IllCode4,a.IllCode5,a.IllCode6                                                              \r\n";
            SQL += "        ,b.SName,b.Age,b.Sex,b.GbSTS MGbSts,b.WardCode,b.RoomCode,a.GBGAMEK,a.BOHUN,  b.JSIM_REMARK,b.JSIM_REMARK9, b.GbSuDay,b.MiIlsu  \r\n";
            SQL += "        ,TO_CHAR(b.MiArcDate,'YYYY-MM-DD') MiArcDate, TO_CHAR(b.INDATE,'YYYY-MM-DD') M_InDate                                           \r\n";
            SQL += "        ,TO_CHAR(b.OUTDATE,'YYYY-MM-DD') M_OutDate, TO_CHAR(b.ACTDATE,'YYYY-MM-DD') M_ActDate                                           \r\n";
            SQL += "        ,TO_CHAR(b.ArcDate,'YYYY-MM-DD') ArcDate, b.Ilsu M_Ilsu,b.Fee6,b.ArcQty,b.GbKekli,b.IcuQty,b.GelCode,b.GbTewon                  \r\n";
            SQL += "        ,a.AMT01,a.AMT02,a.AMT03,a.AMT04,a.AMT05,a.AMT06,a.AMT07,a.AMT08,a.AMT09,a.AMT10                                                \r\n";
            SQL += "        ,a.AMT11,a.AMT12,a.AMT13,a.AMT14,a.AMT15,a.AMT16,a.AMT17,a.AMT18,a.AMT19,a.AMT20                                                \r\n";
            SQL += "        ,a.AMT21,a.AMT22,a.AMT23,a.AMT24,a.AMT25,a.AMT26,a.AMT27,a.AMT28,a.AMT29,a.AMT30                                                \r\n";
            SQL += "        ,a.AMT31,a.AMT32,a.AMT33,a.AMT34,a.AMT35,a.AMT36,a.AMT37,a.AMT38,a.AMT39,a.AMT40                                                \r\n";
            SQL += "        ,a.AMT41,a.AMT42,a.AMT43,a.AMT44,a.AMT45,a.AMT46,a.AMT47,a.AMT48,a.AMT49,a.AMT50                                                \r\n";
            SQL += "        ,a.AMT51,a.AMT52,a.AMT53,a.AMT54,a.AMT55,a.AMT56,a.AMT57,a.AMT58,a.AMT59,a.AMT60, a.AMT61,a.AMT62,a.AMT63,a.AMT64               \r\n";
            SQL += "        ,a.AMT65,a.AMT66,a.AMT67,a.AMT68,a.AMT69,a.AMT70,a.AMT71,a.AMT72,a.AMT73,a.AMT74,a.AMT75,a.AMT76, a.AMT77,a.AMT78               \r\n";
            SQL += "        ,a.AMT79,a.AMT80,a.AMT81,a.AMT82,a.AMT83,a.AMT84,a.AMT85,a.AMT86,a.AMT87,a.AMT88,a.AMT89,a.AMT90, a.AMT91,a.AMT92 ,a.AMT93,a.AMT94 ,a.AMT95             \r\n";
            SQL += "        ,TO_CHAR(A.ROUTDATE,'YYYY-MM-DD HH24:MI') ROUTDATE,   TO_CHAR(A.SIMSATIME,'YYYY-MM-DD HH24:MI') SIMSATIME                       \r\n";
            SQL += "        ,TO_CHAR(A.PRINTTIME,'YYYY-MM-DD HH24:MI') PRINTTIME, TO_CHAR(A.SUNAPTIME,'YYYY-MM-DD HH24:MI') SUNAPTIME                       \r\n";
            SQL += "        ,TO_CHAR(A.MIRBUILDTIME,'YYYY-MM-DD HH24:MI') MIRBUILDTIME, b.GBJIWON, TO_CHAR(B.RDATE,'YYYY-MM-DD') RDATE                      \r\n";
            SQL += "        ,A.GBCHECKLIST,A.GBSTS TGbSts, a.Vcode, C.JUMIN1, C.JUMIN2,C.JUMIN3, A.GBSANG,a.OGPDBUNdtl,a.Gbilban2,a.GbSPC                   \r\n";
            SQL += "        ,TO_CHAR(c.Birth,'YYYY-MM-DD') Birth                                                                                            \r\n";
            if (strTemp == "임시자격")
            {
                SQL += "        ,'' DrgCode,'' JSIM_LDATE, '' JSIM_SABUN , '' JSIM_SET, '' JSIM_OK, '' FCODE, '' KTASLEVL, '' T_CARE, '' OGPDBUN2           \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "WORK_IPD_TRANS_TERM a,                                                                                 \r\n";
            }
            else
            {
                SQL += "        ,a.DrgCode,TO_CHAR(A.JSIM_LDATE,'YYYY-MM-DD') JSIM_LDATE  , A.JSIM_SABUN , A.JSIM_SET , A.JSIM_OK,A.FCODE,b.KTASLEVL        \r\n";
                SQL += "        ,b.T_CARE ,a.OGPDBUN2, a.DRGOG ,a.GBHU                                                                                        \r\n";
                SQL += "  FROM " + ComNum.DB_PMPA + "IPD_TRANS a,                                                                                           \r\n";
            }
            SQL += "       " + ComNum.DB_PMPA + "IPD_NEW_MASTER b,                                                                                          \r\n";
            SQL += "       " + ComNum.DB_PMPA + "BAS_PATIENT c                                                                                              \r\n";
            SQL += " WHERE A.Pano = '" + strPano + "'                                                                                                       \r\n";
            //SQL += "   AND a.GbIPD != 'D'                                                                                                                   \r\n"; //jjy : 2018-07-06  심사팀에서 진료내역조회시 삭제된 내역도 조회 되어야함 다른 sql 과중복되면 로직 변경해야함
            SQL += "   AND A.PANO = C.PANO                                                                                                                  \r\n";
            SQL += "   AND a.IPDNO = b.IPDNO(+)                                                                                                             \r\n";
            if (nTrsNo > 0)
            {
                SQL += "   AND A.TRSNO = " + nTrsNo + "                                                                                                     \r\n";
            }
            else
            {
                SQL += "   AND A.ActDate IS NULL                                                                                                            \r\n";
                SQL += "   AND A.GbSTS IN ('0','1','2','3','4','5','6','7')                                                                                 \r\n";
                SQL += " ORDER BY a.GBSTS ,A.InDate DESC                                                                                                            \r\n";
            }
            try
            {
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return null;
                }
                return Dt;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return null;
            }
        }

        /// <summary>
        /// Description : 주민번호로 환자기본정보 가져오기
        /// Author : 박병규
        /// Create Date : 2017.06.20
        /// <param name="ArgJumin1">주민번호(앞)</param>
        /// <param name="ArgJumin2">주민번호(뒤)</param>
        /// </summary>
        /// <seealso cref=""/>
        public DataTable Get_Jumin_BasPatient(PsmhDb pDbCon, string ArgJumin1, string ArgJumin2)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT * ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND JUMIN1 = '" + ArgJumin1 + "' ";
                SQL += ComNum.VBLF + "    AND JUMIN3 = '" + ArgJumin2 + "' ";
                SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return null;
                }

                return DtPf;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return null;
            }
        }

        /// <summary>
        /// Description : 입원번호 SEQNO 가져오기
        /// Author : 김민철
        /// Create Date : 2017.07.12
        /// </summary>
        /// <seealso cref="IUMENT.bas : READ_NEXT_IPDNO"/>
        public long GET_NEXT_IPDNO(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            long rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT KOSMOS_PMPA.SEQ_IPDNO.NEXTVAL IPDNO ";
            SQL += ComNum.VBLF + "   FROM DUAL";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            rtnVal = long.Parse(Dt.Rows[0]["IPDNO"].ToString());

            Dt.Dispose();
            Dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 입원자격번호 SEQNO 가져오기
        /// Author : 김민철
        /// Create Date : 2017.07.12
        /// </summary>
        /// <seealso cref="IUMENT.bas : READ_Next_TRSNO"/>
        public long GET_NEXT_TRSNO(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            long rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT KOSMOS_PMPA.SEQ_IPDNO2.NEXTVAL IPDNO ";
            SQL += ComNum.VBLF + "   FROM DUAL";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            rtnVal = long.Parse(Dt.Rows[0]["IPDNO"].ToString());

            Dt.Dispose();
            Dt = null;

            return rtnVal;
        }

        /// <summary>
        /// DataTable 조인
        /// </summary>
        /// <param name="First">조인할 데이타테이블1</param>
        /// <param name="Second">조인할 데이타테이블2</param>
        /// <param name="FJC">데이타테이블1의 관계 컬럼들</param>
        /// <param name="SJC">데이타테이블2의 관계 컬럼들</param>
        /// <returns>DataTable</returns>
        public DataTable Join(DataTable First, DataTable Second, DataColumn[] FJC, DataColumn[] SJC)
        {
            DataTable table = new DataTable("Join");

            // 데이타 셋을 사용하여 데이타 관계를 설정
            using (DataSet ds = new DataSet())
            {
                //테이블들의 복사본추가
                ds.Tables.AddRange(new DataTable[] { First.Copy(), Second.Copy() });

                //첫 데이타 테이블의 조인관계 파악
                DataColumn[] parentcolumns = new DataColumn[FJC.Length];

                for (int i = 0; i < parentcolumns.Length; i++)
                {
                    parentcolumns[i] = ds.Tables[0].Columns[FJC[i].ColumnName];
                }

                //두번째 데이타 테이블의 의 컬럼 조인관계 파악
                DataColumn[] childcolumns = new DataColumn[SJC.Length];

                for (int i = 0; i < childcolumns.Length; i++)
                {
                    childcolumns[i] = ds.Tables[1].Columns[SJC[i].ColumnName];
                }


                //관계설정 만듬.
                DataRelation r = new DataRelation(string.Empty, parentcolumns, childcolumns, false);
                ds.Relations.Add(r);

                //JOIN된 테이블로 부터 컬럼 생성
                for (int i = 0; i < First.Columns.Count; i++)
                {
                    table.Columns.Add(First.Columns[i].ColumnName, First.Columns[i].DataType);
                }

                for (int i = 0; i < Second.Columns.Count; i++)
                {
                    //중복된것 거름
                    if (!table.Columns.Contains(Second.Columns[i].ColumnName))
                        table.Columns.Add(Second.Columns[i].ColumnName, Second.Columns[i].DataType);
                    else
                        table.Columns.Add(Second.Columns[i].ColumnName + "_Second", Second.Columns[i].DataType);
                }


                table.BeginLoadData();

                foreach (DataRow firstrow in ds.Tables[0].Rows)
                {
                    //관계설정된 row들을 가져옴
                    DataRow[] childrows = firstrow.GetChildRows(r);

                    if (childrows != null && childrows.Length > 0)
                    {
                        object[] parentarray = firstrow.ItemArray;

                        foreach (DataRow secondrow in childrows)
                        {
                            object[] secondarray = secondrow.ItemArray;
                            object[] joinarray = new object[parentarray.Length + secondarray.Length];
                            Array.Copy(parentarray, 0, joinarray, 0, parentarray.Length);
                            Array.Copy(secondarray, 0, joinarray, parentarray.Length, secondarray.Length);
                            table.LoadDataRow(joinarray, true);
                        }
                    }
                }
                table.EndLoadData();
            }
            return table;
        }

        /// <summary>
        /// DataTable 간 조인 Linq 예제 함수
        /// <param name="strPano"></param>
        /// <param name="nIpdNo"></param>
        /// <param name="nTrsNo"></param>
        /// DataRelation 객체를 이용하는 방법도 있으나 테이블간 조인을 하게 되면 불필요한 컬럼까지 제거해야 하는 번거러움이 발생
        /// Linq를 이용한 DataTable 간 Join 방법을 강구.
        /// 2017-06-20
        /// </summary>
        public DataTable Get_Dt_Join_Ipd_Mst_Trs(PsmhDb pDbCon, string strPano, long nIpdNo, long nTrsNo)
        {
            DataTable DtIpdMst = Get_Ipd_New_Master(pDbCon, strPano, "", nIpdNo);
            DataTable DtIpdTrs = Get_Ipd_Trans(pDbCon, strPano, nIpdNo, nTrsNo);
            DataTable DtBP = Get_BasPatient(pDbCon, strPano);

            DataTable DtIpdMstTrs = null;

            DataRow drTemp;

            //기존 사용자 정보 테이블 구조 및 스키마만 복사(데이터는 복사하지 않음) 
            DtIpdMstTrs = DtIpdMst.Clone();

            //Join으로 추가할 컬럼 추가 
            DtIpdMstTrs.Columns.Add(new DataColumn { ColumnName = "AMT50", Caption = "총진료비", DataType = typeof(string), DefaultValue = string.Empty, AllowDBNull = true });

            /* * BEGIN 사용자 테이블과 부서 테이블간 JOIN후 최종 사용자 정보 DataTable에 반영 * */
            //var : Variable의 약자, C# 3.0부터 추가된 타입이 없는 변수, 초기에 대입되는 값에 의하여 변수의 형식이 결정 됨(int, string, double ... 등 활용 가능) 
            // 여기서는 IEnumerable타입으로 이용 
            var SQL = from M in DtIpdMst.AsEnumerable()                                     // 주 Table
                      join T in DtIpdTrs.AsEnumerable() on M["ipdno"] equals T["ipdno"]     //JOIN 절1
                      join B in DtBP.AsEnumerable() on M["pano"] equals B["pano"]           //JOIN 절2
                      into MT
                      from MTList in MT.DefaultIfEmpty()
                      select new
                      {
                          pano = M["pano"],
                          user_id = M["ipdno"].ToString(),
                          user_name = M["sname"].ToString(),
                          deptcode = M["deptcode"].ToString(),
                          arcdate = M["arcdate"].ToString(),
                          amt50 = (MTList != null ? MTList["AMT50"] : string.Empty)
                      };

            foreach (var v in SQL)
            {
                drTemp = DtIpdMstTrs.NewRow();
                drTemp.BeginEdit(); //생략가능 
                drTemp["pano"] = v.pano;
                drTemp["ipdno"] = v.user_id;
                drTemp["sname"] = v.user_name;
                drTemp["deptcode"] = v.deptcode;
                drTemp["ArcDate"] = v.arcdate;
                drTemp["AMT50"] = v.amt50;
                drTemp.EndEdit(); //생략가능 
                DtIpdMstTrs.Rows.Add(drTemp);
            }

            return DtIpdMstTrs;

        }

        /// <summary>
        /// Description : 의사스케줄 구분값
        /// Author : 박병규
        /// Create Date : 2017.07.28
        /// <param name="ArgCode">상병코드</param>
        /// </summary>
        /// <seealso cref="OUMSAD.bas : 스케줄_구분"/>
        public string Get_DrSchedule_Gubun(string ArgBun)
        {
            string rtnVal = String.Empty;

            switch (ArgBun)
            {
                case "1":
                    rtnVal = "진료;";
                    break;
                case "2":
                    rtnVal = "수술;";
                    break;
                case "3":
                    rtnVal = "특수검사;";
                    break;
                case "4":
                    rtnVal = "진료없음;";
                    break;
                case "5":
                    rtnVal = "학회;";
                    break;
                case "6":
                    rtnVal = "휴가;";
                    break;
                case "7":
                    rtnVal = "추장;";
                    break;
                case "9":
                    rtnVal = "OFF(주40시간);";
                    break;
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 선택진료 토요일 체크 - 있으면 선택진료함
        /// Author : 박병규
        /// Create Date : 2017.07.28
        /// <param name="ArgJumin1">주민번호(앞)</param>
        /// <param name="ArgJumin2">주민번호(뒤)</param>
        /// </summary>
        /// <seealso cref="vb선택진료.bas:READ_SELECT_SAT_DAY_CHK"/>
        public string CHECK_CHOICE_TREAT_SAT(PsmhDb pDbCon, string ArgDept, string ArgDate)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            if (clsVbfunc.GetYoIl(ArgDate) != "토요일")
            {
                return rtnVal;
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Code ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN         = '선택진료_외래토요일' ";
            SQL += ComNum.VBLF + "    AND TRIM(Code)    = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND JDate         <= TO_DATE('" + ArgDate + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='' ) ";
            SQL += ComNum.VBLF + "  ORDER BY JDate DESC ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }


            if (DtPf.Rows.Count > 0)
                rtnVal = "OK";

            DtPf.Dispose();
            DtPf = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 각 검사별 완료 확인
        /// Author : 박병규
        /// Create Date : 2017.08.01
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgGb"></param>
        /// </summary>
        /// <seealso cref=""/>
        public string RETURN_EXAM_END(PsmhDb pDbCon, string ArgPtno, string ArgDate, int ArgGb)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            switch (ArgGb)
            {
                case 1:   //초음파
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT PANO ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
                    SQL += ComNum.VBLF + "    AND XJONG     = '3' ";
                    SQL += ComNum.VBLF + "    AND GBEND     = '1' ";
                    SQL += ComNum.VBLF + "    AND SEEKDATE  = TO_DATE('" + ArgDate + "' ,'YYYY-MM-DD') ";
                    SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

                    break;

                case 3:   //위 내시경
                case 4:   //위 내시경(수면)
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT GBJOB, TO_CHAR(RDATE,'YYYY-MM-DD') RDATE, COUNT(PTNO) CNT ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "ENDO_JUPMST ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND PTNO      = '" + ArgPtno + "' ";
                    SQL += ComNum.VBLF + "    AND RDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND GBIO      = 'O' ";
                    SQL += ComNum.VBLF + "    AND GBJOB     = '2' ";
                    SQL += ComNum.VBLF + "    AND GBSUNAP  != '*' ";
                    SQL += ComNum.VBLF + "    AND RESULTDATE IS NULL ";
                    SQL += ComNum.VBLF + "  GROUP BY GBJOB,RDATE ";
                    SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

                    break;

                case 5:   //대장 내시경(수면)
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT GBJOB, TO_CHAR(RDATE,'YYYY-MM-DD') RDATE, COUNT(PTNO) CNT ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "ENDO_JUPMST ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND PTNO      = '" + ArgPtno + "' ";
                    SQL += ComNum.VBLF + "    AND RDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND GBIO      = 'O' ";
                    SQL += ComNum.VBLF + "    AND GBJOB     = '3' ";
                    SQL += ComNum.VBLF + "    AND GBSUNAP  != '*' ";
                    SQL += ComNum.VBLF + "    AND RESULTDATE IS NULL ";
                    SQL += ComNum.VBLF + "  GROUP BY GBJOB,RDATE ";
                    SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

                    break;

                case 6:   //CT
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT PANO ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "XRAY_DETAIL ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
                    SQL += ComNum.VBLF + "    AND XJONG     = '4' ";
                    SQL += ComNum.VBLF + "    AND GBEND     = '1' ";
                    SQL += ComNum.VBLF + "    AND SEEKDATE  = TO_DATE('" + ArgDate + "' ,'YYYY-MM-DD') ";
                    SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

                    break;
            }

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }

            if (DtPf.Rows.Count > 0)
                rtnVal = "(Y)";

            DtPf.Dispose();
            DtPf = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 휴일체크
        /// Author : 박병규
        /// Create Date : 2017.08.02
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <seealso cref=""/>
        public string CHECK_HOLYDAY(PsmhDb pDbCon, string ArgDate)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT HOLYDAY ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_JOB ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND JOBDATE   = TO_DATE('" + ArgDate + "','YYYY-MM-DD')  ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }

            if (DtPf.Rows.Count > 0)
                if (DtPf.Rows[0]["HOLYDAY"].ToString().Trim() == "*")
                    rtnVal = "OK";

            DtPf.Dispose();
            DtPf = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 포스코예약 공지사항 가져오기
        /// Author : 박병규
        /// Create Date : 2017.08.02
        /// <param name="ArgGb"></param>
        /// <param name="ArgGubun"></param>
        /// </summary>
        /// <seealso cref="oiguide10.frm:포스코예약_공지사항_읽기"/>
        public string READ_POSCO_MSG(PsmhDb pDbCon, string ArgGb, string ArgGubun)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT BIGO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO_MSG ";
            SQL += ComNum.VBLF + "  WHERE PANO   = '" + ArgGb + "' ";
            SQL += ComNum.VBLF + "    AND GUBUN  = '" + ArgGubun + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }

            if (DtPf.Rows.Count > 0)
                rtnVal = DtPf.Rows[0]["BIGO"].ToString().Trim();

            DtPf.Dispose();
            DtPf = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 진료일자 및 시간에 따른 의사 스케줄 확인
        /// Author : 박병규
        /// Create Date : 2017.08.04
        /// <param name="ArgDept">진료과</param>
        /// <param name="ArgDate">진료일자</param>
        /// <param name="ArgTime">진료시간</param>
        /// </summary>
        /// <seealso cref="frmPoscoResMain.frm:READ_JIN_DRCODE"/>
        public string READ_DOCTOR_SCHEDULE(PsmhDb pDbCon, string ArgDept, string ArgDate, string ArgTime)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT A.DRCODE, B.GbJin, B.GbJin2 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR A, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SCHEDULE B ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND A.DRDEPT1 = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND B.SCHDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND A.TOUR    = 'N' ";
            SQL += ComNum.VBLF + "    AND A.DrCode NOT IN ('1109','1113','3111') "; //2016-12-05 산부인과 김도균 과장제외
            SQL += ComNum.VBLF + "    AND A.DRCODE  = B.DrCode(+) ";
            SQL += ComNum.VBLF + "  ORDER BY A.PRINTRANKING ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }


            if (DtPf.Rows.Count > 0)
            {
                for (int i = 0; i < DtPf.Rows.Count; i++)
                {
                    if (DateTime.Compare(Convert.ToDateTime(ArgTime), Convert.ToDateTime("12:30")) > 0)
                    {
                        if (DtPf.Rows[i]["GBJIN2"].ToString().Trim() == "1")
                        {
                            rtnVal = DtPf.Rows[i]["DRCODE"].ToString().Trim();
                            break;
                        }
                    }
                    else
                    {
                        if (DtPf.Rows[i]["GBJIN"].ToString().Trim() == "1")
                        {
                            rtnVal = DtPf.Rows[i]["DRCODE"].ToString().Trim();
                            break;
                        }
                    }
                }
                rtnVal = DtPf.Rows[0]["DRCODE"].ToString().Trim();
            }

            DtPf.Dispose();
            DtPf = null;

            //신경과의 경우 박수현과장님 우선
            if (ArgDept == "NE" && rtnVal == "2601")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.DRCODE, B.GbJin, B.GbJin2 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR A, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SCHEDULE B ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND A.DRDEPT1 = 'NE' ";
                SQL += ComNum.VBLF + "    AND B.SCHDATE = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND A.TOUR    = 'N' ";
                SQL += ComNum.VBLF + "    AND A.DrCode  = '2602' ";
                SQL += ComNum.VBLF + "    AND A.DRCODE  = B.DrCode(+) ";
                SQL += ComNum.VBLF + "  ORDER BY a.PRINTRANKING ";
                SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    rtnVal = "";
                    return rtnVal;
                }

                if (DtPf.Rows.Count > 0)
                {
                    for (int i = 0; i < DtPf.Rows.Count; i++)
                    {
                        if (DateTime.Compare(Convert.ToDateTime(ArgTime), Convert.ToDateTime("12:30")) > 0)
                        {
                            if (DtPf.Rows[i]["GBJIN2"].ToString().Trim() == "1")
                            {
                                rtnVal = "2602";
                                break;
                            }
                        }
                        else
                        {
                            if (DtPf.Rows[i]["GBJIN"].ToString().Trim() == "1")
                            {
                                rtnVal = "2602";
                                break;
                            }
                        }
                    }
                }

                DtPf.Dispose();
                DtPf = null;
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 전화예약 MASTER에서 예약부도 확인
        /// Author : 박병규
        /// Create Date : 2017.08.08
        /// <param name="ArgPtno"></param>
        /// <param name="ArgRdate"></param>
        /// <param name="ArgDept"></param>
        /// </summary>
        /// <seealso cref="frm접수현황통합조회.frm:READ_TELRESV_NOSHOW"/>
        public string READ_TELRESV_NOSHOW(PsmhDb pDbCon, string ArgPtno, string ArgRdate, string ArgDept)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NOSHOW ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_TELRESV ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND RDATE     = TO_DATE('" + ArgRdate + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }

            if (DtPf.Rows.Count > 0)
            {
                if (DtPf.Rows[0]["NOSHOW"].ToString().Trim() == "Y")
                    rtnVal = "부도";
                else
                    rtnVal = "";
            }

            DtPf.Dispose();
            DtPf = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 원무접수제한자 확인(이곳에 등록된것은 대리접수, 접수2만 가능)
        /// Author : 박병규
        /// Create Date : 2017.08.10
        /// <param name="ArgSabun"></param>
        /// </summary>
        /// <seealso cref="OPD_세계병자의날.BAS:원무접수권한_대리접수2"/>
        public string READ_JUPSU_AUTH(PsmhDb pDbCon, string ArgSabun)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT CODE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN     = '원무접수제한자' ";
            SQL += ComNum.VBLF + "    AND CODE      = '" + ArgSabun + "' ";
            SQL += ComNum.VBLF + "    AND (DELDATE >=TRUNC(SYSDATE) OR DELDATE IS NULL ) ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }

            if (DtPf.Rows.Count > 0)
                rtnVal = "Y";

            DtPf.Dispose();
            DtPf = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 해당환자의 초재진 가져오기
        /// Author : 박병규
        /// Create Date : 2017.08.21
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDr"></param>
        /// </summary>
        /// <seealso cref="oumsad.BAS:Read_Gwa_ChoJae"/>
        public string READ_GWA_CHOJAE(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDr)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = "C";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND DRCODE    = '" + ArgDr + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "C";
                return rtnVal;
            }

            if (DtPf.Rows.Count > 0)
                rtnVal = "J";

            DtPf.Dispose();
            DtPf = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 감액율(자격) 가져오기
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// <param name="ArgGbGamek"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgOutDate"></param>
        /// <param name="ArgLtdCode"></param>
        /// <param name="ArgIO"></param>
        /// </summary>
        /// <seealso cref="OPDGAMEK.BAS:READ_Gamek_Rate"/>
        public bool READ_GAMEK_RATE(PsmhDb pDbCon, string ArgGbGamek, string ArgBi, string ArgOutDate, string ArgLtdCode, string ArgIO)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            bool rtnVal = false;

            //감액율을 보관하는 변수
            clsPmpaType.GAM.GbGameK = "";
            clsPmpaType.GAM.Gam_Rate = 0;
            clsPmpaType.GAM.Jin_Rate = 0;
            clsPmpaType.GAM.SONO_Rate = 0;
            clsPmpaType.GAM.MRI_Rate = 0;
            clsPmpaType.GAM.FOOD_Rate = 0;
            clsPmpaType.GAM.ROOM_Rate = 0;
            clsPmpaType.GAM.ER_Rate = 0;
            clsPmpaType.GAM.DTJin_Rate = 0;
            clsPmpaType.GAM.DTGam_Rate = 0;
            clsPmpaType.GAM.DT1_Rate = 0;
            clsPmpaType.GAM.DT2_Rate = 0;
            clsPmpaType.GAM.DT3_Rate = 0;
            clsPmpaType.GAM.Amt50_Rate = 0;
            //할인 계산금액
            clsPmpaType.GAM.Jin_Halin = 0;
            clsPmpaType.GAM.Gam_Halin = 0;
            clsPmpaType.GAM.SONO_Halin = 0;
            clsPmpaType.GAM.MRI_Halin = 0;
            clsPmpaType.GAM.FOOD_Halin = 0;
            clsPmpaType.GAM.ROOM_Halin = 0;
            clsPmpaType.GAM.ER_Halin = 0;
            clsPmpaType.GAM.DTJin_Halin = 0;
            clsPmpaType.GAM.DTGam_Halin = 0;
            clsPmpaType.GAM.DT1_Halin = 0;
            clsPmpaType.GAM.DT2_Halin = 0;
            clsPmpaType.GAM.DT3_Halin = 0;
            clsPmpaType.GAM.Halin_Tot = 0;
            clsPmpaType.GAM.DTHalin_Tot = 0;

            //감액코드가 없으면 감액율에 0을 설정함
            if (ArgGbGamek == "" || ArgGbGamek == "0" || ArgGbGamek == "00")
            {
                rtnVal = true;
                return rtnVal;
            }

            clsPmpaType.GAM.GbGameK = ArgGbGamek.Trim();
            clsPmpaType.GAM.LtdCode = ArgLtdCode.Trim();

            //계약처 할인율 SET
            if (ArgGbGamek == "55")
            {
                //계약처는 11,12,13,21,22,33,41,51종만 감액이 가능함
                if (ArgBi != "11" && ArgBi != "12" && ArgBi != "13" && ArgBi != "33" && ArgBi != "51" && ArgBi != "21" && ArgBi != "22" && ArgBi != "41")
                {
                    rtnVal = true;
                    return rtnVal;
                }

                if (ArgIO != "I" && ArgLtdCode.Trim() == "H400")  //입원만 적용되는 코드
                {
                    rtnVal = true;
                    return rtnVal;
                }

                //회사별 감액설정 Table를 읽음
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Bi_51, Bi_33, DT1, ";
                SQL += ComNum.VBLF + "        DT2, DT3, Sono, ";
                SQL += ComNum.VBLF + "        MRI, Food, Room, ";
                SQL += ComNum.VBLF + "        Bohum ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMLTD ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND LtdCode   = '" + ArgLtdCode.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND SDate    <= TO_DATE('" + ArgOutDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  ORDER BY SDate DESC ";
                SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                    ;
                }

                if (DtPf.Rows.Count == 0)
                {
                    DtPf.Dispose();
                    DtPf = null;

                    rtnVal = true;
                    return rtnVal;
                }
                else
                {
                    switch (ArgBi)
                    {
                        case "33":
                            clsPmpaType.GAM.Amt50_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["BI_33"].ToString()));
                            break;

                        case "41":
                        case "51":
                            clsPmpaType.GAM.Amt50_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["BI_51"].ToString()));
                            break;
                        case "11":
                        case "12":
                        case "13":
                        case "21":
                        case "22":
                            if (ArgIO == "I" && ArgLtdCode.Trim() != "H400")
                            {
                                clsPmpaType.GAM.Jin_Rate = 0;
                                clsPmpaType.GAM.Gam_Rate = 0;
                            }
                            else if (ArgIO == "I" && ArgLtdCode.Trim() == "H400")
                            {
                                clsPmpaType.GAM.Jin_Rate = 0;
                                clsPmpaType.GAM.Gam_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["Bohum"].ToString()));
                            }
                            else
                            {
                                clsPmpaType.GAM.Jin_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["Bohum"].ToString()));
                                clsPmpaType.GAM.Gam_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["Bohum"].ToString()));
                            }

                            clsPmpaType.GAM.Amt50_Rate = 0;
                            clsPmpaType.GAM.DT1_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["DT1"].ToString()));
                            clsPmpaType.GAM.DT2_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["DT2"].ToString()));
                            clsPmpaType.GAM.DT3_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["DT3"].ToString()));
                            clsPmpaType.GAM.SONO_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["SONO"].ToString()));
                            clsPmpaType.GAM.MRI_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["MRI"].ToString()));
                            clsPmpaType.GAM.FOOD_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["FOOD"].ToString()));
                            clsPmpaType.GAM.ROOM_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["ROOM"].ToString()));
                            break;
                    }
                }
                DtPf.Dispose();
                DtPf = null;

                rtnVal = true;
                return rtnVal;
            }

            //자동차 보험은 감액을 안함
            if (ArgBi == "52")
            {
                rtnVal = true;
                return rtnVal;
            }

            //감액 기준 Table에서 감액율을 읽음(치과 제외)
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SDate,'YYYY-MM-DD') SDate, Jin, Bohum, ";
            SQL += ComNum.VBLF + "        Gongsang, Ilban, SONO, ";
            SQL += ComNum.VBLF + "        MRI, Food, Room, Er ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMCODE ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND Code  = '" + ArgGbGamek + "' ";
            SQL += ComNum.VBLF + "    AND GbDT  = '1' "; //일반과 감액율
            SQL += ComNum.VBLF + "    AND SDate <= TO_DATE('" + ArgOutDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  ORDER BY SDate DESC ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtPf.Rows.Count == 0)
            {
                DtPf.Dispose();
                DtPf = null;

                rtnVal = true;
                return rtnVal;
            }
            else
            {
                clsPmpaType.GAM.Jin_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["JIN"].ToString()));

                switch (ArgBi)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                  //  case "43"://원무팀장 승인후 2019-03-15
                        clsPmpaType.GAM.Gam_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["Bohum"].ToString()));
                        break;
                    case "33":
                        clsPmpaType.GAM.Gam_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["GONGSANG"].ToString()));
                        break;
                    case "51":
                        clsPmpaType.GAM.Gam_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["Ilban"].ToString()));
                        break;
                    default:
                        clsPmpaType.GAM.Gam_Rate = 0;
                        break;
                }

                clsPmpaType.GAM.SONO_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["SONO"].ToString()));
                clsPmpaType.GAM.MRI_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["MRI"].ToString()));
                clsPmpaType.GAM.FOOD_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["FOOD"].ToString()));
                clsPmpaType.GAM.ROOM_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["ROOM"].ToString()));
                clsPmpaType.GAM.ER_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["ER"].ToString()));
            }
            DtPf.Dispose();
            DtPf = null;

            //치과 감액 기준 Table에서 감액율을 읽음
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SDate,'YYYY-MM-DD') SDate, Jin, Bohum, ";
            SQL += ComNum.VBLF + "        Gongsang, Ilban, DT1, DT2, DT3 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMCODE ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND Code  = '" + ArgGbGamek + "' ";
            SQL += ComNum.VBLF + "    AND GbDT  = '2' "; //치과 감액율
            SQL += ComNum.VBLF + "    AND SDate <= TO_DATE('" + ArgOutDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  ORDER BY SDate DESC ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
                ;
            }

            if (DtPf.Rows.Count == 0)
            {
                DtPf.Dispose();
                DtPf = null;

                rtnVal = true;
                return rtnVal;
            }
            else
            {
                clsPmpaType.GAM.GbGameK = ArgGbGamek;
                clsPmpaType.GAM.DTJin_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["JIN"].ToString()));

                switch (ArgBi)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                        clsPmpaType.GAM.DTGam_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["Bohum"].ToString()));
                        break;
                    case "51":
                        clsPmpaType.GAM.DTGam_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["Ilban"].ToString()));
                        break;
                    default:
                        clsPmpaType.GAM.DTGam_Rate = 0;
                        break;
                }

                clsPmpaType.GAM.DT1_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["DT1"].ToString()));
                clsPmpaType.GAM.DT2_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["DT2"].ToString()));
                clsPmpaType.GAM.DT3_Rate = Convert.ToInt32(VB.Val(DtPf.Rows[0]["DT3"].ToString()));
            }
            DtPf.Dispose();
            DtPf = null;

            rtnVal = true;

            return rtnVal;
        }

        /// <summary>
        /// Description : 감액율(CASE) 가져오기
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// <param name="ArgGbGamek"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgOutDate"></param>
        /// <param name="ArgLtdCode"></param>
        /// <param name="ArgIO"></param>
        /// </summary>
        /// <seealso cref="OPDGAMEK.BAS:READ_Gamek_Rate"/>
        public bool READ_GAMEK_CASE_RATE(PsmhDb pDbCon, string ArgGbGamek, string ArgBi, string ArgOutDate, string ArgLtdCode, string ArgIO)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            bool rtnVal = false;

            //감액율을 보관하는 변수
            clsPmpaType.GAMC.GbGameK = "";
            clsPmpaType.GAMC.Gam_Rate = 0;
            clsPmpaType.GAMC.Jin_Rate = 0;
            clsPmpaType.GAMC.SONO_Rate = 0;
            clsPmpaType.GAMC.MRI_Rate = 0;
            clsPmpaType.GAMC.FOOD_Rate = 0;
            clsPmpaType.GAMC.ROOM_Rate = 0;
            clsPmpaType.GAMC.ER_Rate = 0;
            clsPmpaType.GAMC.DTJin_Rate = 0;
            clsPmpaType.GAMC.DTGam_Rate = 0;
            clsPmpaType.GAMC.DT1_Rate = 0;
            clsPmpaType.GAMC.DT2_Rate = 0;
            clsPmpaType.GAMC.DT3_Rate = 0;
            clsPmpaType.GAMC.Amt50_Rate = 0;
            //할인 계산금액
            clsPmpaType.GAMC.Jin_Halin = 0;
            clsPmpaType.GAMC.Gam_Halin = 0;
            clsPmpaType.GAMC.SONO_Halin = 0;
            clsPmpaType.GAMC.MRI_Halin = 0;
            clsPmpaType.GAMC.FOOD_Halin = 0;
            clsPmpaType.GAMC.ROOM_Halin = 0;
            clsPmpaType.GAMC.ER_Halin = 0;
            clsPmpaType.GAMC.DTJin_Halin = 0;
            clsPmpaType.GAMC.DTGam_Halin = 0;
            clsPmpaType.GAMC.DT1_Halin = 0;
            clsPmpaType.GAMC.DT2_Halin = 0;
            clsPmpaType.GAMC.DT3_Halin = 0;
            clsPmpaType.GAMC.Halin_Tot = 0;
            clsPmpaType.GAMC.DTHalin_Tot = 0;

            //감액코드가 없으면 감액율에 0을 설정함
            if (ArgGbGamek == "" || ArgGbGamek == "0" || ArgGbGamek == "00" || ArgGbGamek == "50")
            {
                rtnVal = true;
                return rtnVal;
            }

            clsPmpaType.GAMC.GbGameK = ArgGbGamek.Trim();
            clsPmpaType.GAMC.LtdCode = ArgLtdCode.Trim();

            //자동차 보험은 감액을 안함
            if (ArgBi == "52")
            {
                rtnVal = true;
                return rtnVal;
            }

            //감액 기준 Table에서 감액율을 읽음(치과 제외)
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SDate,'YYYY-MM-DD') SDate, Jin, Bohum, ";
            SQL += ComNum.VBLF + "        Gongsang, Ilban, SONO, ";
            SQL += ComNum.VBLF + "        MRI, Food, Room, Er ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMCODE ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND Code  = '" + ArgGbGamek + "' ";
            SQL += ComNum.VBLF + "    AND GbDT  = '1' "; //일반과 감액율
            SQL += ComNum.VBLF + "    AND SDate <= TO_DATE('" + ArgOutDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  ORDER BY SDate DESC ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtPf.Rows.Count == 0)
            {
                DtPf.Dispose();
                DtPf = null;

                rtnVal = true;
                return rtnVal;
            }
            else
            {
                clsPmpaType.GAMC.Jin_Rate = Convert.ToInt32(DtPf.Rows[0]["JIN"].ToString());

                switch (ArgBi)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                        clsPmpaType.GAMC.Gam_Rate = Convert.ToInt32(DtPf.Rows[0]["Bohum"].ToString());
                        break;
                    case "33":
                        clsPmpaType.GAMC.Gam_Rate = Convert.ToInt32(DtPf.Rows[0]["GONGSANG"].ToString());
                        break;
                    case "51":
                        clsPmpaType.GAMC.Gam_Rate = Convert.ToInt32(DtPf.Rows[0]["Ilban"].ToString());
                        break;
                    default:
                        clsPmpaType.GAMC.Gam_Rate = 0;
                        break;
                }

                clsPmpaType.GAMC.SONO_Rate = Convert.ToInt32(DtPf.Rows[0]["SONO"].ToString());
                clsPmpaType.GAMC.MRI_Rate = Convert.ToInt32(DtPf.Rows[0]["MRI"].ToString());
                clsPmpaType.GAMC.FOOD_Rate = Convert.ToInt32(DtPf.Rows[0]["FOOD"].ToString());
                clsPmpaType.GAMC.ROOM_Rate = Convert.ToInt32(DtPf.Rows[0]["ROOM"].ToString());
                clsPmpaType.GAMC.ER_Rate = Convert.ToInt32(DtPf.Rows[0]["ER"].ToString());
            }
            DtPf.Dispose();
            DtPf = null;

            //치과 감액 기준 Table에서 감액율을 읽음
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SDate,'YYYY-MM-DD') SDate, Jin, Bohum, ";
            SQL += ComNum.VBLF + "        Gongsang, Ilban, DT1, DT2, DT3 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMCODE ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND Code  = '" + ArgGbGamek + "' ";
            SQL += ComNum.VBLF + "    AND GbDT  = '2' "; //치과 감액율
            SQL += ComNum.VBLF + "    AND SDate <= TO_DATE('" + ArgOutDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  ORDER BY SDate DESC ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
                ;
            }

            if (DtPf.Rows.Count == 0)
            {
                DtPf.Dispose();
                DtPf = null;

                rtnVal = true;
                return rtnVal;
            }
            else
            {
                clsPmpaType.GAMC.GbGameK = ArgGbGamek;
                clsPmpaType.GAMC.DTJin_Rate = Convert.ToInt32(DtPf.Rows[0]["JIN"].ToString());

                switch (ArgBi)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                        clsPmpaType.GAMC.DTGam_Rate = Convert.ToInt32(DtPf.Rows[0]["Bohum"].ToString());
                        break;
                    case "51":
                        clsPmpaType.GAMC.DTGam_Rate = Convert.ToInt32(DtPf.Rows[0]["Ilban"].ToString());
                        break;
                    default:
                        clsPmpaType.GAMC.DTGam_Rate = 0;
                        break;
                }

                clsPmpaType.GAMC.DT1_Rate = Convert.ToInt32(DtPf.Rows[0]["DT1"].ToString());
                clsPmpaType.GAMC.DT2_Rate = Convert.ToInt32(DtPf.Rows[0]["DT2"].ToString());
                clsPmpaType.GAMC.DT3_Rate = Convert.ToInt32(DtPf.Rows[0]["DT3"].ToString());
            }
            DtPf.Dispose();
            DtPf = null;

            rtnVal = true;

            return rtnVal;
        }

        /// <summary>
        /// Description : 소방전문치료센터 협약관련(자격)
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// <param name="ArgGbGamek"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgOutDate"></param>
        /// <param name="ArgLtdCode"></param>
        /// <param name="ArgIO"></param>
        /// <param name="ArgJumin"></param>
        /// </summary>
        /// <seealso cref="OPDGAMEK.BAS:READ_Gamek_Rate_소방처"/>
        public void READ_GAMEK_RATE_H911(PsmhDb pDbCon, string ArgGbGamek, string ArgBi, string ArgOutDate, string ArgLtdCode, string ArgIO, string ArgJumin = "")
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            clsPmpaPb.GstrSpecail_Gam = "";
            clsPmpaType.GAM.DTJin_Rate = 0;
            clsPmpaType.GAM.DTGam_Rate = 0;
            clsPmpaType.GAM.DT1_Rate = 0;
            clsPmpaType.GAM.DT2_Rate = 0;
            clsPmpaType.GAM.DT3_Rate = 0;

            //감액 기준 Table에서 감액율을 읽음(치과 제외)
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SDate,'YYYY-MM-DD') SDate, Jin, Bohum, ";
            SQL += ComNum.VBLF + "        Gongsang, Ilban, SONO, ";
            SQL += ComNum.VBLF + "        MRI, Food, Room, Er ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMCODE ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND Code  = '23' ";//병원 직원의 직계존비속,친형제,자매에 준하는 감액
            SQL += ComNum.VBLF + "    AND GbDT  = '1' "; //일반과 감액율
            SQL += ComNum.VBLF + "    AND SDate <= TO_DATE('" + ArgOutDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  ORDER BY SDate DESC ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (DtPf.Rows.Count == 0)
            {
                DtPf.Dispose();
                DtPf = null;
                return;
            }
            else
            {
                clsPmpaType.GAM.Jin_Rate = Convert.ToInt32(DtPf.Rows[0]["JIN"].ToString());

                switch (ArgBi)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                        clsPmpaType.GAM.Gam_Rate = Convert.ToInt32(DtPf.Rows[0]["Bohum"].ToString());
                        break;
                    case "33":
                        clsPmpaType.GAM.Gam_Rate = Convert.ToInt32(DtPf.Rows[0]["GONGSANG"].ToString());
                        break;
                    case "51":
                        clsPmpaType.GAM.Gam_Rate = Convert.ToInt32(DtPf.Rows[0]["Ilban"].ToString());
                        break;
                    default:
                        clsPmpaType.GAM.Gam_Rate = 0;
                        break;
                }

                clsPmpaType.GAM.SONO_Rate = Convert.ToInt32(DtPf.Rows[0]["SONO"].ToString());
                clsPmpaType.GAM.MRI_Rate = Convert.ToInt32(DtPf.Rows[0]["MRI"].ToString());
                clsPmpaType.GAM.FOOD_Rate = Convert.ToInt32(DtPf.Rows[0]["FOOD"].ToString());
                clsPmpaType.GAM.ROOM_Rate = Convert.ToInt32(DtPf.Rows[0]["ROOM"].ToString());
                clsPmpaType.GAM.ER_Rate = Convert.ToInt32(DtPf.Rows[0]["ER"].ToString());
            }
            DtPf.Dispose();
            DtPf = null;

            return;
        }

        /// <summary>
        /// Description : 소방전문치료센터 협약관련(CASE)
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// <param name="ArgGbGamek"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgOutDate"></param>
        /// <param name="ArgLtdCode"></param>
        /// <param name="ArgIO"></param>
        /// <param name="ArgJumin"></param>
        /// </summary>
        /// <seealso cref="OPDGAMEK.BAS:READ_Gamek_Rate_소방처"/>
        public void READ_GAMEK_CASE_RATE_H911(string ArgGbGamek, string ArgBi, string ArgOutDate, string ArgLtdCode, string ArgIO, string ArgJumin = "")
        {
            clsPmpaPb.GstrSpecail_GamC = "";
            clsPmpaType.GAMC.DTJin_Rate = 0;
            clsPmpaType.GAMC.DTGam_Rate = 0;
            clsPmpaType.GAMC.DT1_Rate = 0;
            clsPmpaType.GAMC.DT2_Rate = 0;
            clsPmpaType.GAMC.DT3_Rate = 0;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// <param name="ArgGbGamek"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgOutDate"></param>
        /// <param name="ArgLtdCode"></param>
        /// <param name="ArgIO"></param>
        /// <param name="ArgJumin"></param>
        /// </summary>
        /// <seealso cref="OPDGAMEK.BAS:READ_Gamek_Rate_특정행사"/>
        public void READ_GAMEK_RATE_EVENT(PsmhDb pDbCon, string ArgGbGamek, string ArgBi, string ArgOutDate, string ArgLtdCode, string ArgIO, string ArgJumin = "")
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            clsPmpaPb.GstrSpecail_Gam = "";

            //영일직원 직계존손 배우자 추가
            if (ArgGbGamek != "22" && ArgGbGamek != "23" && ArgGbGamek != "24" && ArgGbGamek != "27" && ArgGbGamek != "32" && ArgGbGamek != "33" && ArgGbGamek != "34") { return; }
            //감액코드가 없으면 감액율에 0을 설정함
            if (ArgGbGamek == "" || ArgGbGamek == "0" || ArgGbGamek == "00") { return; }
            //계약처 할인율 SET
            if (ArgGbGamek == "55") { return; }
            //자동차 보험은 감액을 안함
            if (ArgBi == "52") { return; }

            if (READ_GAMEK_COERCION_OBJECT(pDbCon, ArgJumin) != "OK")
            {
                if (string.Compare(ArgOutDate, "2021-05-03") < 0 || string.Compare(ArgOutDate, "2021-05-29") > 0)
                    return;
            }
            //강제예외 처리
            if (READ_GAMEK_COERCION_OBJECT(pDbCon, ArgJumin,"NOT") == "OK")
            {
                return;
            }


            if ((string.Compare(ArgOutDate, "2021-05-03") >= 0 && string.Compare(ArgOutDate, "2021-05-29") <= 0) || READ_GAMEK_COERCION_OBJECT2(pDbCon, ArgJumin, ArgOutDate) == "OK")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.ROWID, B.GamMessage ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_ERP + "INSA_MSTB A, ";
                SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_GAMF B ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND A.JUMIN3 = '" + clsAES.AES(ArgJumin) + "' ";
                SQL += ComNum.VBLF + "    AND A.KWAN IN  ('0','2','3','6','7','10','11','12','13') "; //부모,시부모,장인,장모 만 다시 읽음 , 2012-05 (조부모추가)
                SQL += ComNum.VBLF + "    AND (B.GAMEND >= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') OR B.GAMEND IS NULL) ";
                SQL += ComNum.VBLF + "    AND A.JUMIN3 = B.GAMJUMIN3 ";
                SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (DtPf.Rows.Count > 0)
                {
                    clsPmpaPb.GstrSpecail_Gam = DtPf.Rows[0]["GamMessage"].ToString();

                    clsPmpaType.GAM.Jin_Rate = 50;

                    switch (ArgBi)
                    {
                        case "11":
                        case "12":
                        case "13":
                        case "21":
                        case "22":
                        case "23":
                        case "24":
                            clsPmpaType.GAM.Gam_Rate = 50;
                            break;
                        case "33":
                            clsPmpaType.GAM.Gam_Rate = 0;
                            break;
                        case "51":
                            clsPmpaType.GAM.Gam_Rate = 50;
                            break;
                        default:
                            clsPmpaType.GAM.Gam_Rate = 0;
                            break;
                    }

                    clsPmpaType.GAM.SONO_Rate = 50;
                    clsPmpaType.GAM.MRI_Rate = 50;

                    if (clsPmpaType.GAM.ER_Rate < 50)
                    {
                        clsPmpaType.GAM.ER_Rate = 50;
                    }
                }

                DtPf.Dispose();
                DtPf = null;
            }
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// <param name="ArgGbGamek"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgOutDate"></param>
        /// <param name="ArgLtdCode"></param>
        /// <param name="ArgIO"></param>
        /// <param name="ArgJumin"></param>
        /// </summary>
        /// <seealso cref="OPDGAMEK.BAS:READ_Gamek_Rate_특정행사"/>
        public void READ_GAMEK_CASE_RATE_EVENT(string ArgGbGamek, string ArgBi, string ArgOutDate, string ArgLtdCode, string ArgIO, string ArgJumin = "")
        {
            clsPmpaPb.GstrSpecail_GamC = "";

            //감액코드가 없으면 감액율에 0을 설정함
            if (ArgGbGamek == "" || ArgGbGamek == "0" || ArgGbGamek == "00" || ArgGbGamek == "50")
            {
                return;
            }
            //자동차 보험은 감액을 안함
            if (ArgBi == "52")
            {
                return;
            }
        }

        /// <summary>
        /// Description : 대상내시경 예약때문
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// <param name="ArgJumin"></param>
        /// </summary>
        /// <seealso cref="OPDGAMEK.BAS:READ_외래효도감액강제대상"/>
        public string READ_GAMEK_COERCION_OBJECT(PsmhDb pDbCon, string ArgJumin, [Optional]string arg )
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = "";

            ArgJumin = VB.Replace(ArgJumin, "-", "");

            SQL = "";
            SQL += ComNum.VBLF + " SELECT CODE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            if (arg == "NOT")
            {
                SQL += ComNum.VBLF + "    AND GUBUN = '외래_효도감액강제예외대상' ";
            }
            else
            {
                SQL += ComNum.VBLF + "    AND GUBUN = '외래_효도감액강제대상' ";
            }
          

            SQL += ComNum.VBLF + "    AND TRIM(CODE) = '" + ArgJumin + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtPf.Rows.Count > 0)
                rtnVal = "OK";

            DtPf.Dispose();
            DtPf = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 예약검사 내시경때문 날짜무시함
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// <param name="ArgJumin"></param>
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <seealso cref="OPDGAMEK.BAS:READ_외래효도감액강제대상2"/>
        public string READ_GAMEK_COERCION_OBJECT2(PsmhDb pDbCon, string ArgJumin, string ArgDate)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = "";

            ArgJumin = VB.Replace(ArgJumin, "-", "");

            SQL = "";
            SQL += ComNum.VBLF + " SELECT CODE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN = '외래_효도감액강제대상' ";
            SQL += ComNum.VBLF + "    AND TRIM(CODE) = '" + ArgJumin + "' ";
            SQL += ComNum.VBLF + "    AND JDate = TO_DATE('" + ArgDate + "','YYYY-MM-DD')  ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtPf.Rows.Count > 0)
                rtnVal = "OK";

            DtPf.Dispose();
            DtPf = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 계약처명 가져오기
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// <param name="ArgJumin"></param>
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <seealso cref="OUMSAD.BAS:Bas_계약처명"/>
        public string READ_LTDNAME(PsmhDb pDbCon, string ArgLtdCode)
        {
            DataTable DtPf = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = "";

            clsPmpaPb.GstrMiaFlag = "NO";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT MIACODE, MIANAME, MiaDetail, ";
            SQL += ComNum.VBLF + "       TO_CHAR(DelDate,'YYYY-MM-DD') DelDate ";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_MIA ";
            SQL += ComNum.VBLF + " WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "   AND MIACODE  = '" + ArgLtdCode.Trim() + "' ";
            if (ArgLtdCode.Trim() != "5000")
                SQL += ComNum.VBLF + "   AND MIADETAIL = '99' ";

            SQL += ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE = '') ";
            SqlErr = clsDB.GetDataTableEx(ref DtPf, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtPf.Rows.Count > 0)
            {
                clsPmpaPb.GstrMiaFlag = "OK";
                clsPmpaPb.GstrMiaDetail = DtPf.Rows[0]["MiaDetail"].ToString().Trim();

                if (DtPf.Rows[0]["DelDate"].ToString().Trim() != "")
                {
                    rtnVal = "계약이 해지되었습니다.";
                    clsPmpaPb.GstrMiaFlag = "NO";
                }
                else
                    rtnVal = DtPf.Rows[0]["MIANAME"].ToString().Trim();
            }

            DtPf.Dispose();
            DtPf = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : OPD_MASTER JIN 구분 이름 읽어오기
        /// Author : 김민철
        /// Create Date : 2017.08.22
        /// <param name="ArgJin"></param>
        /// </summary>
        /// <seealso cref=""/>
        public string READ_JIN(PsmhDb pDbCon, string ArgJin)
        {
            string rtnVal = string.Empty;
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Name FROM " + ComNum.DB_PMPA + "BAS_OPDJIN ";
            SQL += ComNum.VBLF + "  WHERE Code = '" + ArgJin.Trim() + "' ";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }

            if (Dt.Rows.Count > 0)
            {
                rtnVal = Dt.Rows[0]["Name"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 문제환자 관리 (외래진료 쪽 Data)
        /// Author : 김민철
        /// Create Date : 2017.08.22
        /// <param name="ArgPano"></param>
        /// <param name="ArgGbn"></param>
        /// </summary>
        /// <seealso cref="IUMENT.bas:Read_Bas_OcsMemo_Check "/>
        /// <seealso cref="oumsad.bas:Read_Bas_OcsMemo_Check"/>
        public string READ_BAS_OCSMEMO_CHECK(PsmhDb pDbCon, string ArgPano, string ArgGbn)
        {
            string rtnVal = string.Empty;
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO,SNAME,MEMO FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_MID ";
            SQL += ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "'              ";
            SQL += ComNum.VBLF + "    AND (DDATE IS NULL OR DDATE ='' )         ";
            SQL += ComNum.VBLF + "    AND GBN  in ( '" + ArgGbn + "' ,'0' )     ";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }

            if (Dt.Rows.Count > 0)
            {
                rtnVal = VB.Replace(Dt.Rows[0]["MEMO"].ToString().Trim(), "`", "'");
            }

            Dt.Dispose();
            Dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 심사용 수납메세지
        /// Author : 박병규
        /// Create Date : 2017.10.18
        /// <param name="ArgPano"></param>
        /// <param name="ArgGbn"></param>
        /// </summary>
        /// <seealso cref="oumsad.bas:Read_Bas_OcsMemo_Check2"/>
        public string READ_BAS_OCSMEMO_CHECK2(PsmhDb pDbCon, string ArgPano, string ArgGbn)
        {
            string rtnVal = string.Empty;
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, SNAME, MEMO, JobSabun, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_BAS_PASS_NAME(JOBSABUN) NAME, ";
            SQL += ComNum.VBLF + "        KOSMOS_OCS.FC_ETC_TELBOOK(JOBSABUN) TEL ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_SIM ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPano + "' ";
            SQL += ComNum.VBLF + "    AND (DDATE IS NULL OR DDATE ='' ) ";
            SQL += ComNum.VBLF + "    AND GBN  in ( '" + ArgGbn + "' ,'0' )  ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                rtnVal = VB.Replace(DtFunc.Rows[0]["MEMO"].ToString().Trim(), "`", "'") + '\r' + '\r';
                rtnVal += "심사과 메세지 전달!" + '\r';
                rtnVal += "등록사원 : " + DtFunc.Rows[0]["NAME"].ToString().Trim() + '\r';
                rtnVal += "내선번호 : " + DtFunc.Rows[0]["TEL"].ToString().Trim();
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 응급실 진료시 KTAS LEVEL 가져오기
        /// Author : 김민철
        /// Create Date : 2017.08.23
        /// <param name="ArgPano"></param>
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <seealso cref="ERACCT.bas:RTN_KTAS_LEVEL"/>
        public string RTN_KTAS_LEVEL(PsmhDb pDbCon, string ArgPano, string ArgDate)
        {
            string rtnVal = string.Empty;
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (ArgDate.Trim() == "")
            {
                ArgDate = clsPublic.GstrSysDate;
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT /*+ INDEX(NUR_ER_PATIENT index_nurerpatient4) */MIN(KTASLEVL) KTASLEVL ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "NUR_ER_PATIENT                                   ";
            SQL += ComNum.VBLF + "  WHERE PANO = '" + ArgPano + "'                                               ";
            SQL += ComNum.VBLF + "    AND JDATE>= TO_DATE('" + ArgDate + "','YYYY-MM-DD')                        ";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                rtnVal = "";
                return rtnVal;
            }

            if (Dt.Rows.Count > 0)
            {
                rtnVal = VB.Replace(Dt.Rows[0]["KTASLEVL"].ToString().Trim(), "`", "'");
            }

            Dt.Dispose();
            Dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 등록번호로 조건을 검사한다.
        /// Author : 박병규
        /// Create Date : 2017.10.16
        /// </summary>
        /// <param name="strPano"></param>
        /// <seealso cref="New_Patient_No"/>
        public string NewPtno_Check_Digit(string strPtno)
        {
            int i = 0;
            int j = 0;
            int Sum = 0;
            int na = 0;
            int mok = 0;
            int c = 0;

            string rtnVal = "";

            i = 7;
            for (j = 1; j <= 7; j++)
            {
                Sum += Convert.ToInt32(VB.Val(VB.Mid(strPtno, j, 1))) * i;
                i -= 1;
            }

            mok = (Sum / 11);
            na = Sum - (mok * 11);
            c = 11 - na;

            if (c == 10 || c == 11)
                c = 0;

            rtnVal = strPtno + c;

            return rtnVal;
        }

        /// <summary>
        /// Description : 등록번호로 조건을 검사한다.
        /// Author : 안정수
        /// Create Date : 2017.08.24
        /// </summary>
        /// <param name="strPano"></param>
        /// <seealso cref="OumSad2.bas : Pano_Check_Digit"/>
        public string Pano_Check_Digit(PsmhDb pDbCon, string strPano)
        {
            int i = 0;
            int j = 0;

            int Sum = 0;
            int na = 0;
            int mok = 0;
            int ChkNo = 0;

            string rtnVal = "";

            i = 7;
            for (j = 1; j <= 7; j++)
            {
                Sum += Convert.ToInt32(VB.Val(VB.Mid(strPano, j, 1))) * i;
                i -= 1;
            }
            mok = (Sum / 11);
            na = Sum - (mok * 11);
            ChkNo = 11 - na;

            rtnVal = "NO";

            if (ChkNo == 10 || ChkNo == 11)
            {
                if (VB.Val(VB.Right(strPano, 1)) == 0)
                {
                    rtnVal = "OK";
                }
            }
            else
            {
                if (VB.Val(VB.Right(strPano, 1)) == ChkNo)
                {
                    rtnVal = "OK";
                }

            }

            if (VB.Left(strPano, 6) == "810000")
            {
                rtnVal = "OK";
            }

            if (VB.Left(strPano, 4) == "2015")
            {
                rtnVal = "OK";
            }

            //2011-04-14 산재환자임.입원/외래 동시에 진료 때문에 SETTING함
            if (OPD_JEPSU_PANO_ETC_OK(pDbCon, strPano) == "OK")
            {
                rtnVal = "OK";
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : Pano_Check_Digit 함수에서 산재환자임.입원/외래 동시에 진료 때문에 SETTING 확인
        /// Author : 안정수
        /// Create Date : 2017.08.24
        /// </summary>
        /// <param name="ArgPano"></param>
        /// <seealso cref="OumSad2.bas : OPD_JEPSU_PANO_ETC_OK"/>
        public string OPD_JEPSU_PANO_ETC_OK(PsmhDb pDbCon, string ArgPano)
        {
            string rtnVal = "NO";
            string SQL = "";
            string SqlErr = "";

            DataTable dt = null;


            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  CODE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND TRIM(CODE) ='" + ArgPano + "'";
            SQL += ComNum.VBLF + "      AND Gubun ='접수등록번호예외'";

            try
            {

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return rtnVal;
                //}

                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : GnAge, GstrSex 공용변수 Setting
        /// Author : 김민철
        /// Create Date : 2017.08.25
        /// </summary>
        public void Set_Pat_AgeSex(string strJumin1, string strJumin2, string strDate)
        {
            clsPmpaPb.GnAge = 0;

            if (strJumin1.Length == 6)
            {
                clsPmpaPb.GnAge = ComFunc.AgeCalcEx(strJumin1 + strJumin2, strDate);
            }

            if (VB.Left(strJumin2, 1) == "0" || VB.Left(strJumin2, 1) == "1" || VB.Left(strJumin2, 1) == "3" || VB.Left(strJumin2, 1) == "5" || VB.Left(strJumin2, 1) == "7")
            {
                clsPmpaPb.GstrSex = "M";
            }
            else
            {
                clsPmpaPb.GstrSex = "F";
            }
        }

        /// <summary>
        /// Description : 휴대폰번호 체크
        /// Author : 박병규
        /// Create Date : 2017.08.28
        /// <param name="ArgNum"></param>
        /// </summary>
        /// <seealso cref="oumsad.bas:HandPhoneNumber_Check"/>
        public bool CHECK_HPHONE_NUMBER(string ArgNum)
        {
            string strHP = "";
            bool rtnVal = true;

            if (ArgNum.Trim() == "")
                return rtnVal;

            for (int i = 1; i <= ArgNum.Trim().Length; i++)
            {
                switch (VB.Asc(VB.Mid(ArgNum, i, 1)))
                {
                    case 48:
                    case 49:
                    case 50:
                    case 51:
                    case 52:
                    case 53:
                    case 54:
                    case 55:
                    case 56:
                    case 57:
                        strHP += VB.Mid(ArgNum, i, 1);
                        break;
                }
            }

            //번호 Check
            switch (VB.Left(strHP, 3))
            {
                case "010":
                case "011":
                case "016":
                case "017":
                case "018":
                case "019":

                    break;

                default:
                    rtnVal = false;
                    break;
            }

            if (strHP.Length < 10)
                rtnVal = false;

            return rtnVal;
        }

        /// <summary>
        /// Description : 진료의사 출장 및 퇴직여부 점검
        /// Author : 박병규
        /// Create Date : 2017.08.28
        /// <param name="ArgCode"></param>
        /// </summary>
        /// <seealso cref="oumsad.bas:Doctor_Check"/>
        public bool CHECK_DOCTOR(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT * ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND DRCODE    = '" + ArgCode + "' ";
            SQL += ComNum.VBLF + "    AND TOUR      <> 'Y' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = false;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = true;

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 수가이름을 Read
        /// Author : 안정수
        /// Create Date : 2017.08.30
        /// <param name="ArgCode"></param>
        /// </summary>
        /// <seealso cref="opdgam.bas : SuCode_Name"/>
        /// <returns></returns>
        public string SuCode_Name(PsmhDb pDbCon, string ArgSuCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                        ";
            SQL += ComNum.VBLF + "  SUNAMEK                                     ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUN            ";
            SQL += ComNum.VBLF + "  WHERE 1=1                                   ";
            SQL += ComNum.VBLF + "    AND SUNEXT = '" + ArgSuCode.Trim() + "'   ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = dt.Rows[0]["SUNAMEK"].ToString().Trim();
            }
            return rtnVal;
        }

        /// <summary>
        /// Description : 선택진료 입원,외래 부담율 읽기
        /// Author : 박병규
        /// Create Date : 2017.08.31
        /// </summary>
        /// <seealso cref="vb선택진료.bas:READ_SELECT_RATE_CHK"/>
        /// <history>clsPmpaSel.cs 로 이동 2017.11.03 박병규</history>
        //public string READ_SELECT_RATE_CHK(PsmhDb pDbCon, string ArgDeptCode, string ArgBdate, string ArgDrCode)
        //{
        //    DataTable DtFunc = new DataTable();
        //    string SQL = string.Empty;
        //    string SqlErr = string.Empty;
        //    string strUse = string.Empty;
        //    string rtnVal = string.Empty;

        //    clsPmpaType.SEL.Current_Rate = 0;//수가의 내용 읽어 해당율

        //    clsPmpaType.SEL.OPD_Gb_Select = "";//외래선택진료여부   2014-02-03
        //    clsPmpaType.SEL.OPD_Jin_Rate = 0;//진찰료
        //    clsPmpaType.SEL.OPD_Med_Rate = 0;//의학관리료
        //    clsPmpaType.SEL.OPD_Gum_Rate = 0;//검사료
        //    clsPmpaType.SEL.OPD_Xray_Rate = 0;//영상진단 및 방사선치료
        //    clsPmpaType.SEL.OPD_Mach_Rate = 0;//마취료
        //    clsPmpaType.SEL.OPD_Psy_Rate = 0;//정신요법
        //    clsPmpaType.SEL.OPD_Op_Rate = 0;//처치수술료

        //    clsPmpaType.SEL.IPD_Gb_Select = "";//입원선택진료여부   2014-02-03
        //    clsPmpaType.SEL.IPD_Jin_Rate = 0;//진찰료
        //    clsPmpaType.SEL.IPD_Med_Rate = 0;//의학관리료
        //    clsPmpaType.SEL.IPD_Gum_Rate = 0;//검사료
        //    clsPmpaType.SEL.IPD_Xray_Rate = 0;//영상진단 및 방사선치료
        //    clsPmpaType.SEL.IPD_Mach_Rate = 0;//마취료
        //    clsPmpaType.SEL.IPD_Psy_Rate = 0;//정신요법
        //    clsPmpaType.SEL.IPD_Op_Rate = 0;//처치수술료

        //    strUse = "Y";

        //    SQL = "";
        //    SQL += ComNum.VBLF + " SELECT ALL_USE ";
        //    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SELECT_DEPT  ";
        //    SQL += ComNum.VBLF + "  WHERE DeptCode  = '" + ArgDeptCode + "' ";
        //    SQL += ComNum.VBLF + "    AND JDate     <= TO_DATE('" + ArgBdate + "','YYYY-MM-DD')  ";
        //    SQL += ComNum.VBLF + "    AND ALL_USE   = 'N' ";  //의사코드 사용여부
        //    SQL += ComNum.VBLF + "  ORDER BY JDate DESC ";
        //    SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

        //    if (DtFunc.Rows.Count == 0)
        //    {
        //        DtFunc.Dispose();
        //        DtFunc = null;
        //        return rtnVal;
        //    }

        //    if (DtFunc.Rows.Count > 0)
        //        strUse = "N";

        //    DtFunc.Dispose();
        //    DtFunc = null;

        //    SQL = "";
        //    SQL += ComNum.VBLF + " SELECT ISET0, ISET1, ISET2, ";
        //    SQL += ComNum.VBLF + "        ISET3, ISET4, ISET5, ";
        //    SQL += ComNum.VBLF + "        ISET6, ISET7, OSET0, ";
        //    SQL += ComNum.VBLF + "        OSET1, OSET2, OSET3, ";
        //    SQL += ComNum.VBLF + "        OSET4, OSET5, OSET6, ";
        //    SQL += ComNum.VBLF + "        OSET7 ";
        //    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SELECT_SET  ";
        //    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
        //    SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDeptCode + "' ";
        //    SQL += ComNum.VBLF + "    AND JDate     <= TO_DATE('" + ArgBdate + "','YYYY-MM-DD')  ";

        //    if (strUse.Equals("N"))
        //        SQL += ComNum.VBLF + "   AND DrCode = '" + ArgDrCode + "' ";

        //    SQL += ComNum.VBLF + "  ORDER BY JDate DESC ";
        //    SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

        //    if (DtFunc.Rows.Count == 0)
        //    {
        //        DtFunc.Dispose();
        //        DtFunc = null;
        //        return rtnVal;
        //    }

        //    if (DtFunc.Rows.Count > 0)
        //    {
        //        rtnVal = "OK";

        //        if (DtFunc.Rows[0]["ISET0"].ToString().Trim() == "1")
        //        {
        //            clsPmpaType.SEL.IPD_Gb_Select = DtFunc.Rows[0]["ISET0"].ToString().Trim();
        //            clsPmpaType.SEL.IPD_Jin_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET1"].ToString().Trim()); //진찰료
        //            clsPmpaType.SEL.IPD_Med_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET2"].ToString().Trim()); //의학관리료
        //            clsPmpaType.SEL.IPD_Gum_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET3"].ToString().Trim()); //검사료 2012-05-25 내시경검사추가
        //            clsPmpaType.SEL.IPD_Xray_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET4"].ToString().Trim()); //영상진단 및 방사선치료
        //            clsPmpaType.SEL.IPD_Mach_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET5"].ToString().Trim()); //마취료
        //            clsPmpaType.SEL.IPD_Psy_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET6"].ToString().Trim()); //정신요법
        //            clsPmpaType.SEL.IPD_Op_Rate = Convert.ToInt32(DtFunc.Rows[0]["ISET7"].ToString().Trim()); //처치수술료
        //        }

        //        if (DtFunc.Rows[0]["OSET0"].ToString().Trim() == "1")
        //        {
        //            clsPmpaType.SEL.OPD_Gb_Select = DtFunc.Rows[0]["OSET0"].ToString().Trim();
        //            clsPmpaType.SEL.OPD_Jin_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET1"].ToString().Trim()); //진찰료
        //            clsPmpaType.SEL.OPD_Med_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET2"].ToString().Trim()); //의학관리료
        //            clsPmpaType.SEL.OPD_Gum_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET3"].ToString().Trim()); //검사료 2012-05-25 내시경검사추가
        //            clsPmpaType.SEL.OPD_Xray_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET4"].ToString().Trim()); //영상진단 및 방사선치료
        //            clsPmpaType.SEL.OPD_Mach_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET5"].ToString().Trim()); //마취료
        //            clsPmpaType.SEL.OPD_Psy_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET6"].ToString().Trim()); //정신요법
        //            clsPmpaType.SEL.OPD_Op_Rate = Convert.ToInt32(DtFunc.Rows[0]["OSET7"].ToString().Trim()); //처치수술료
        //        }
        //    }

        //    DtFunc.Dispose();
        //    DtFunc = null;

        //    return rtnVal;
        //}

        /// <summary>
        /// Description : 지역코드 이름 읽어오기
        /// Author : 김민철
        /// Create Date : 2017.08.31
        /// </summary>
        /// <param name="strJiCode"></param>
        /// <returns></returns>
        public string READ_JiCode_Name(PsmhDb pDbCon, string strJiCode)
        {
            string rtnVal = string.Empty;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (strJiCode == "")
            {
                rtnVal = "";
                return rtnVal;
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT JiName FROM " + ComNum.DB_PMPA + "BAS_AREA ";
            SQL += ComNum.VBLF + "  WHERE JiCode = '" + strJiCode + "' ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
                rtnVal = dt.Rows[0]["JiName"].ToString().Trim();
            else
                rtnVal = "";

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 거래처정보 가져오기
        /// Author : 박병규
        /// Create Date : 2017.09.01
        /// </summary>
        /// <param name="strJiCode"></param>
        /// <returns></returns>
        /// <seealso cref="oumsad.bas:BAS_기관명"/>
        public string GET_BAS_MIA(PsmhDb pDbCon, string strKiho)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            clsPmpaPb.GstrMiaFlag = "NO";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT MiaCode, MiaName, MiaClass, ";
            SQL += ComNum.VBLF + "        MiaDetail, TO_CHAR(DelDate,'YYYY-MM-DD') DelDate ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_MIA ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND MiaCode  = '" + strKiho + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                rtnVal = DtFunc.Rows[0]["MIANAME"].ToString().Trim();

                clsPmpaPb.GstrReturnMiaClass = DtFunc.Rows[0]["MiaClass"].ToString().Trim();
                clsPmpaPb.GstrReturnMiaCode = DtFunc.Rows[0]["MiaCode"].ToString().Trim();
                clsPmpaPb.GstrReturnMiaName = DtFunc.Rows[0]["MiaName"].ToString().Trim();

                clsPmpaPb.GstrMiaClass = DtFunc.Rows[0]["MiaClass"].ToString().Trim();
                clsPmpaPb.GstrMiaDetail = DtFunc.Rows[0]["MiaDetail"].ToString().Trim();

                clsPmpaPb.GstrMiaFlag = "OK";
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 후불대상자 체크
        /// Author : 김민철
        /// Create Date : 2017.09.04
        /// </summary>
        /// <param name="ArgPano"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgDept"></param>
        /// <param name="strBDate"></param>
        /// <returns></returns>
        /// <seealso cref="Vb후불수납.bas : READ_A_SUNAP_CHK"/>
        public string READ_A_SUNAP_CHK(PsmhDb pDbCon, string ArgPano, string ArgBi, string ArgDept, string strBDate)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT Pano,TO_CHAR(EDATE,'YYYY-MM-DD') EDATE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_AUTO_MST ";
                SQL += ComNum.VBLF + "  WHERE Pano ='" + ArgPano + "' ";
                SQL += ComNum.VBLF + "    AND (DELDATE ='' OR DELDATE IS NULL) ";
                SQL += ComNum.VBLF + "    AND GUBUN ='1'";
                SQL += ComNum.VBLF + "  ORDER BY SDATE DESC ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (Dt.Rows.Count > 0)
                {
                    if (strBDate.Equals(""))
                    {
                        rtnVal = "OK";
                    }
                    else
                    {
                        if (Dt.Rows[0]["EDATE"].ToString().Trim() == "")
                        {
                            rtnVal = "OK";
                        }
                        else if (string.Compare(strBDate, Dt.Rows[0]["EDATE"].ToString().Trim()) <= 0)
                        {
                            rtnVal = "OK";
                        }
                        else
                        {
                            rtnVal = "";
                        }
                    }
                }

                Dt.Dispose();
                Dt = null;

                if (ArgBi != "")
                {
                    if (ArgBi.Equals("21") || ArgBi.Equals("22") || ArgBi.Equals("31") || ArgBi.Equals("32") || ArgBi.Equals("33") || ArgBi.Equals("52") || ArgBi.Equals("55"))
                    {
                        rtnVal = "";
                    }
                }

                if (ArgDept != "")
                {
                    if (ArgDept.Equals("HD"))
                    {
                        rtnVal = "";
                    }
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }



        }

        /// <summary>
        /// Description : 가족 감액 체크?
        /// Author : 안정수
        /// Create Date : 2017.09.05
        /// </summary>
        /// <param name="Spd"></param>
        /// <param name="ArgIO"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgActdate"></param>
        /// <param name="ArgBDate"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgPart"></param>
        /// <param name="ArgSeqno"></param>
        /// <param name="ArgIPDNO"></param>
        /// <param name="ArgTRSNO"></param>
        /// <seealso cref="patient_rate.bas : Read_Patient_Rate_Chk"/>
        public void Read_Patient_Rate_Chk(PsmhDb pDbCon, FarPoint.Win.Spread.SheetView Spd, string ArgIO, string ArgPano, string ArgActdate, string ArgBDate,
                                          string ArgBi, string ArgDept, string ArgPart, int ArgSeqno, int ArgIPDNO, int ArgTRSNO)
        {
            string strJin = "";

            string strMCode = "";
            string strMCodeDtl = "";
            string strVCode = "";
            int nBonRate = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ArgIO == "O")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                        ";
                SQL += ComNum.VBLF + "  PANO,VCODE,MCODE,JIN,JINDTL,ETCAMT,ETCAMT2,JINDTL2,GELCODE                  ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SUNAP                                          ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                SQL += ComNum.VBLF + "      AND PANO ='" + ArgPano + "'                                             ";
                SQL += ComNum.VBLF + "      AND (DEPTCODE ='" + ArgDept + "' OR DEPTCODE ='II')                     ";
                SQL += ComNum.VBLF + "      AND ACTDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD')                 ";
                SQL += ComNum.VBLF + "      AND (BDATE =TO_DATE('" + ArgBDate + "','YYYY-MM-DD') OR BDATE IS NULL)  ";
                SQL += ComNum.VBLF + "      AND PART ='" + ArgPart + "'                                             ";
                SQL += ComNum.VBLF + "      AND SEQNO2 =" + ArgSeqno + "                                            ";
                SQL += ComNum.VBLF + "      AND (DELDATE IS NULL OR DELDATE ='')                                    ";

                //접수비
                if (ArgSeqno == 0)
                {
                    SQL += ComNum.VBLF + "  AND REMARK IN ('접수비')";
                }

                try
                {
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        //return rtnVal;
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
                        Spd.Rows.Count = dt.Rows.Count;

                        strJin = dt.Rows[0]["Jin"].ToString().Trim();
                        strMCode = dt.Rows[0]["MCODE"].ToString().Trim();
                        strVCode = dt.Rows[0]["VCODE"].ToString().Trim();

                        Spd.Cells[0, 0].Text = ArgBi;
                        Spd.Cells[0, 1].Text = strMCode == "" ? "-" : strMCode;
                        Spd.Cells[0, 2].Text = "-";
                        Spd.Cells[0, 3].Text = "-";
                        Spd.Cells[0, 4].Text = "-";
                        Spd.Cells[0, 5].Text = "-";
                        Spd.Cells[0, 6].Text = "-";
                        Spd.Cells[0, 7].Text = "-";

                        if (strVCode == "F003")
                        {
                            Spd.Cells[0, 8].Text = "30";
                            Spd.Cells[0, 2].Text = "-";
                        }
                        else
                        {
                            Spd.Cells[0, 2].Text = strVCode == "" ? "-" : strVCode;
                            Spd.Cells[0, 8].Text = "-";
                        }

                        if (strMCode == "" && (strVCode == "" || strVCode == "F003"))
                        {
                            Spd.Cells[0, 3].Text = READ_OpdBonin_Rate_CHK(pDbCon, ArgBi, ArgBDate).ToString();
                        }

                        else if (strMCode != "")
                        {
                            switch (strMCode)
                            {
                                case "C000":
                                    Spd.Cells[0, 3].Text = "0";
                                    break;
                                case "H000":
                                    Spd.Cells[0, 3].Text = "10";
                                    break;
                                case "V000":
                                    Spd.Cells[0, 3].Text = "10";
                                    break;
                                case "E000":
                                case "F000":
                                    if (strJin != "I" && strJin != "J")
                                    {
                                        Spd.Cells[0, 3].Text = "14";
                                        Spd.Cells[0, 7].Text = "-";
                                    }
                                    else
                                    {
                                        Spd.Cells[0, 3].Text = "0";
                                        Spd.Cells[0, 7].Text = strJin == "I" ? "1500" : "1000";
                                    }

                                    //차상위2 + 중증 고가장비
                                    Spd.Cells[0, 4].Text = "14";
                                    break;
                            }
                        }

                        else if (strVCode != "")
                        {
                            switch (strVCode)
                            {
                                case "V193":
                                case "V194":
                                    Spd.Cells[0, 3].Text = READ_Cancer_BonRate_CHK(pDbCon, ArgBi, ArgBDate, strVCode).ToString();
                                    Spd.Cells[0, 4].Text = "5";     //고가장비
                                    break;

                                case "V247":
                                case "V248":
                                case "V249":
                                case "V250":
                                    Spd.Cells[0, 0].Text = "5";     //중증화상
                                    Spd.Cells[0, 4].Text = "5";     //고가장비
                                    break;

                                case "V206":
                                case "V231":
                                    Spd.Cells[0, 0].Text = "10";    //결핵
                                    Spd.Cells[0, 4].Text = "10";    //고가장비
                                    break;
                                case "EV00":
                                    Spd.Cells[0, 3].Text = "10";    //차상위2+중증
                                    Spd.Cells[0, 4].Text = "10";    //차상위2+중증 고가장비
                                    break;
                            }
                        }

                        //의료급여
                        if (ArgBi == "21" || ArgBi == "22")
                        {
                            if (Convert.ToInt32(VB.Val(dt.Rows[0]["EtcAmt"].ToString().Trim())) != 0)
                            {
                                Spd.Cells[0, 6].Text = dt.Rows[0]["EtcAmt"].ToString().Trim();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                }
                dt.Dispose();
                dt = null;
            }

            else if (ArgIO == "I")
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT";
                SQL += ComNum.VBLF + "  PANO,Bi,GBIPD,BonRate,DeptCode,Bohun,VCODE,OGPDbun,OGPDbunDtl,OGPDbun2,JinDtl,GELCODE";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_TRANS";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND PANO ='" + ArgPano + "'";
                SQL += ComNum.VBLF + "      AND TRSNO =" + ArgTRSNO + "";
                SQL += ComNum.VBLF + "      AND DEPTCODE ='" + ArgDept + "'  ";

                try
                {
                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        //return rtnVal;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        //return rtnVal;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        Spd.Rows.Count = dt.Rows.Count;

                        strMCode = dt.Rows[0]["OGPDbun"].ToString().Trim();
                        strMCodeDtl = dt.Rows[0]["OGPDbunDtl"].ToString().Trim();
                        strVCode = dt.Rows[0]["VCODE"].ToString().Trim();
                        nBonRate = Convert.ToInt32(VB.Val(dt.Rows[0]["BonRate"].ToString().Trim()));

                        Spd.Cells[0, 0].Text = ArgBi;
                        Spd.Cells[0, 1].Text = strMCode == "" ? "-" : strMCode + " " + strMCodeDtl.Trim();
                        Spd.Cells[0, 2].Text = "-";
                        Spd.Cells[0, 3].Text = "-"; //기본부담
                        Spd.Cells[0, 4].Text = "-"; //고가장비
                        Spd.Cells[0, 5].Text = "-"; //식대
                        Spd.Cells[0, 6].Text = "-";
                        Spd.Cells[0, 7].Text = "-";

                        Spd.Cells[0, 2].Text = strVCode == "" ? "-" : strVCode;
                        Spd.Cells[0, 8].Text = "-";

                        if (strMCodeDtl == "")
                        {
                            Spd.Cells[0, 3].Text = READ_IpdBon_Rate_CHK(pDbCon, ArgBi, ArgBDate).ToString();
                        }

                        else if (strMCodeDtl != "")
                        {
                            switch (strMCode)
                            {
                                case "V193":
                                case "V194":
                                    Spd.Cells[0, 3].Text = READ_Cancer_BonRate_CHK(pDbCon, ArgBi, ArgBDate, strVCode).ToString();
                                    Spd.Cells[0, 4].Text = "5"; //고가장비
                                    break;

                                case "V247":
                                case "V248":
                                case "V249":
                                case "V250":
                                    Spd.Cells[0, 0].Text = "5"; //중증화상
                                    Spd.Cells[0, 4].Text = "5"; //고가장비
                                    break;

                                case "V206":
                                case "V231":
                                    Spd.Cells[0, 0].Text = "10"; //결핵
                                    Spd.Cells[0, 4].Text = "10"; //고가장비
                                    break;

                                case "EV00":
                                    Spd.Cells[0, 3].Text = "10"; //차상위2+중증
                                    Spd.Cells[0, 4].Text = "10"; //차상위2+중증 고가장비
                                    break;
                            }
                        }

                        //고가장비
                        if (String.Compare(ArgBi, "30") < 1)
                        {
                            if (String.Compare(ArgBDate, "2007-12-31") <= 1)
                            {
                                if (strMCode == "E" || strMCode == "F")
                                {
                                    if (strMCodeDtl == "0" || strMCodeDtl == "S" || strMCodeDtl == "P")
                                    {
                                        //자연분만산모,6세미만아동,신생아 ct.mri 본인부담 0% 2009-04-01
                                        Spd.Cells[0, 4].Text = "0";
                                    }
                                    else if (strVCode == "V193" || ArgDept == "NP")
                                    {
                                        //차상위2종-중증,정신과 ct.mri 본인부담 10%  2009-04-01
                                        Spd.Cells[0, 4].Text = "10";
                                    }
                                    else
                                    {
                                        //차상위2종 ct.mri 본인부담 14%  2009-04-01
                                        Spd.Cells[0, 4].Text = "14";
                                    }
                                }

                                else if (ArgBi == "21")
                                {
                                    Spd.Cells[0, 4].Text = "0";
                                }

                                else if (ArgBi == "22")
                                {
                                    //Y268 뇌출혈추가
                                    if (String.Compare(ArgBDate, "2010-01-01") >= 1 && (strVCode == "V191" || strVCode == "V192" || strVCode == "V193" || strVCode == "V194") || strVCode == "V268" || strVCode == "V275")
                                    {
                                        //2009-06-01 의료급여2종 중증 환자는 CT,MRI =>본인부담률이 5%
                                        Spd.Cells[0, 4].Text = "5";
                                    }

                                    else if (String.Compare(ArgBDate, "2009-06-01") >= 1)
                                    {
                                        //2009-06-01 의료급여2종 환자는 CT,MRI =>본인부담률이 10%
                                        Spd.Cells[0, 4].Text = "10";
                                    }

                                    else
                                    {
                                        if (strVCode == "V191" || strVCode == "V193" || strVCode == "V268" || strVCode == "V275")
                                        {
                                            //V191,V193환자는 CT,MRI =>본인부담률이 10%
                                            Spd.Cells[0, 4].Text = "10";
                                        }
                                        else
                                        {
                                            //나머지 자격은 외래본인부담율 다름.
                                            Spd.Cells[0, 4].Text = "50";
                                        }
                                    }
                                }

                                else
                                {
                                    Spd.Cells[0, 4].Text = "50";
                                }
                            }

                            else
                            {
                                //현재적용
                                if (strMCode == "C")
                                {
                                    //차상위계층환자는 CT,MRI 본인부담율이 0%
                                    Spd.Cells[0, 4].Text = "0";
                                }

                                else if (String.Compare(ArgBDate, "2010-07-01") >= 1 && (ArgBi == "13" || ArgBi == "12" || ArgBi == "11" || ArgBi == "21" || ArgBi == "22") &&
                                                                                       (strVCode == "V247" || strVCode == "V248" || strVCode == "V249" || strVCode == "V250"))
                                {
                                    //2010-07-01 중증화상 2010-07-01 5%
                                    Spd.Cells[0, 4].Text = "5";
                                }

                                else if (String.Compare(ArgBDate, "2009-12-01") >= 1 && (ArgBi == "13" || ArgBi == "12" || ArgBi == "11") &&
                                                                                       (strVCode == "V193" || strVCode == "V194"))
                                {
                                    //2009-12-01 중증암 2009-12-01 5%
                                    Spd.Cells[0, 4].Text = "5";
                                }

                                else if (String.Compare(ArgBDate, "2009-07-01") >= 1 && (strMCode == "V" || strMCode == "H"))
                                {
                                    //2009-07-01 등록희귀난치V, 희귀난치H 는 10%
                                    Spd.Cells[0, 4].Text = "10";
                                }

                                else if (strMCode == "E" || strMCode == "F")
                                {
                                    if (String.Compare(ArgBDate, "2010-01-01") >= 1)
                                    {
                                        if (strMCodeDtl == "0" || strMCodeDtl == "S" || strMCodeDtl == "P")
                                        {
                                            //자연분만산모,6세미만아동,신생아 ct.mri 본인부담 0% 2009-04-01
                                            Spd.Cells[0, 4].Text = "0";
                                        }

                                        //V268 뇌출혈 추가
                                        else if (strVCode == "V191" || strVCode == "V192" || strVCode == "V193" || strVCode == "V194" || strVCode == "V268" || strVCode == "V275")
                                        {
                                            //차상위2종-중증, ct.mri 본인부담 5%  2010-01-01
                                            Spd.Cells[0, 4].Text = "5";
                                        }

                                        else if (ArgDept == "NP")
                                        {
                                            //차상위2종-중증,정신과 ct.mri 본인부담 10%  2009-04-01
                                            Spd.Cells[0, 4].Text = "10";
                                        }

                                        else if (strMCode == "E" || strMCode == "F" && VB.Left(strVCode, 1).ToUpper() == "V" && String.Compare(ArgBDate, "2009-07-01") >= 1)
                                        {
                                            //차상위 E,F 인데 희귀V 코드 있을경우 10% 2009-11-16 김준수샘 요청
                                            Spd.Cells[0, 4].Text = "10";
                                        }

                                        else
                                        {
                                            //차상위2종 ct.mri 본인부담 14%  2009-04-01
                                            Spd.Cells[0, 4].Text = "14";
                                        }
                                    }
                                    else
                                    {
                                        if (strMCodeDtl == "0" || strMCodeDtl == "S" || strMCode == "P")
                                        {
                                            //자연분만산모,6세미만아동,신생아 ct.mri 본인부담 0% 2009-04-01
                                            Spd.Cells[0, 4].Text = "0";
                                        }

                                        else if (strVCode == "V193" || ArgDept == "NP")
                                        {
                                            //차상위2종-중증,정신과 ct.mri 본인부담 10%  2009-04-01
                                            Spd.Cells[0, 4].Text = "0";
                                        }

                                        else if (strMCode == "E" || strMCode == "F" && VB.Left(strVCode, 1).ToUpper() == "V" && String.Compare(ArgBDate, "2009-07-01") >= 1)
                                        {
                                            //차상위 E,F 인데 희귀V 코드 있을경우 10% 2009-11-16 김준수샘 요청
                                            Spd.Cells[0, 4].Text = "0";
                                        }
                                        else
                                        {
                                            //차상위2종 ct.mri 본인부담 14%  2009-04-01
                                            Spd.Cells[0, 4].Text = "14";
                                        }
                                    }
                                }

                                else if (ArgBi == "11" || ArgBi == "12" || ArgBi == "13" && strMCode == "S")
                                {
                                    //건강보험 소아 6세미만 CT.MRI 본인부담 10% 김순옥계장 요청
                                    Spd.Cells[0, 4].Text = "10";
                                }

                                else
                                {
                                    if (ArgBi == "21")
                                    {
                                        //의료급여 CT.MRI 본인부담 0% - 2009-03-03 윤조연 수정 기존잘못되어있었음 (5%->0%))- 심사과 김순옥샘
                                        Spd.Cells[0, 4].Text = "0";
                                    }

                                    else if (ArgBi == "22")
                                    {
                                        //V268 뇌출혈 추가
                                        if (String.Compare(ArgBDate, "2010-01-01") >= 1 && (strVCode == "V191" || strVCode == "V192" || strVCode == "V193" || strVCode == "V194" || strVCode == "V268" || strVCode == "V275"))
                                        {
                                            //2009-06-01 의료급여2종 중증 환자는 CT,MRI =>본인부담률이 5%
                                            Spd.Cells[0, 4].Text = "5";
                                        }

                                        else if (String.Compare(ArgBDate, "2009-06-01") >= 1)
                                        {
                                            //2009-06-01 의료급여2종 환자는 CT,MRI =>본인부담률이 10%
                                            Spd.Cells[0, 4].Text = "10";
                                        }

                                        else
                                        {
                                            if (strVCode == "V191" || strVCode == "V193" || strVCode == "V268" || strVCode == "V275")
                                            {
                                                //V191,V193환자는 CT,MRI =>본인부담률이 10%
                                                Spd.Cells[0, 4].Text = "10";
                                            }
                                            else
                                            {
                                                //나머지 자격은 외래본인부담율 다름.
                                                Spd.Cells[0, 4].Text = "10";
                                            }
                                        }
                                    }

                                    else
                                    {
                                        if (strMCode == "C")
                                        {
                                            //차상위계층환자는 CT,MRI 본인부담율이 0% 2008-09-22일 수정함.
                                            Spd.Cells[0, 4].Text = "0";
                                        }

                                        //Y268 뇌출혈추가
                                        else if (String.Compare(ArgBDate, "2010-01-01") >= 1 && (ArgBi == "11" || ArgBi == "12" || ArgBi == "13") && (strVCode == "V191" || strVCode == "V192" || strVCode == "V193" || strVCode == "V194" || strVCode == "V268" || strVCode == "V275"))
                                        {
                                            //중증환자는 CT,MRI =>본인부담률이 5% 2010-01-01
                                            Spd.Cells[0, 4].Text = "5";
                                        }

                                        else if ((ArgBi == "13" || ArgBi == "12" || ArgBi == "11") && strMCode == "E")
                                        {
                                            //차상위계층2 만성질환자는 CT,MRI 본인부담율이 14% 2009-04-01일 수정함.
                                            Spd.Cells[0, 4].Text = "14";
                                        }

                                        else if ((ArgBi == "13" || ArgBi == "12" || ArgBi == "11") && strMCode == "F")
                                        {
                                            //차상위계층2 장애인 만성질환자는 CT,MRI 본인부담율이 14% 2009-04-01일 수정함.
                                            Spd.Cells[0, 4].Text = "14";
                                        }

                                        else
                                        {
                                            //나머지 자격은 외래본인부담율 다름.
                                            Spd.Cells[0, 4].Text = "50";
                                        }
                                    }
                                }
                            }
                        }

                        //식대, 급여
                        if (ArgBi == "21" || ArgBi == "22")
                        {
                            if (String.Compare(ArgBDate, "2010-01-01") >= 1)
                            {
                                switch (strVCode)
                                {
                                    //V268 뇌출혈 추가

                                    case "V191":
                                    case "V192":
                                    case "V193":
                                    case "V194":
                                    case "V268":
                                    case "V275":
                                        Spd.Cells[0, 5].Text = "5";
                                        break;

                                    case "V247":
                                    case "V248":
                                    case "V249":
                                    case "V250":
                                        if (String.Compare(ArgBDate, "2010-07-01") >= 1)
                                        {
                                            Spd.Cells[0, 5].Text = "5";
                                        }
                                        else
                                        {
                                            Spd.Cells[0, 5].Text = "20";
                                        }
                                        break;

                                    default:
                                        Spd.Cells[0, 5].Text = "20";
                                        break;

                                }
                            }

                            else
                            {
                                switch (strVCode)
                                {
                                    case "V191":
                                    case "V192":
                                    case "V193":
                                    case "V194":
                                    case "V268":
                                    case "V275":
                                        Spd.Cells[0, 5].Text = "10";
                                        break;

                                    default:
                                        Spd.Cells[0, 5].Text = "20";
                                        break;
                                }

                            }
                        }

                        else if (String.Compare(ArgBDate, "2007-12-31") >= 1)
                        {
                            //나머지 자격은 본인부담율이 적용됨
                            Spd.Cells[0, 5].Text = nBonRate.ToString();
                        }

                        else if (String.Compare(ArgBDate, "2008-01-01") >= 1)
                        {
                            //보험
                            if (strMCode == "C" || strMCode == "E" || strMCode == "F")
                            {
                                Spd.Cells[0, 5].Text = "20";
                            }
                            else if (String.Compare(ArgBi, "13") <= 1)
                            {
                                Spd.Cells[0, 5].Text = "50";
                            }
                            else
                            {
                                Spd.Cells[0, 5].Text = nBonRate.ToString();
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                }
                dt.Dispose();
                dt = null;
            }
        }

        /// <summary>
        /// Description : 암 본인부담 체크
        /// Author : 안정수
        /// Create Date : 2017.09.05
        /// </summary>
        /// <param name="ArgBi"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgVCode"></param>
        /// <seealso cref="patient_rate.bas : READ_Cancer_BonRate_CHK"/>

        public int READ_Cancer_BonRate_CHK(PsmhDb pDbCon, string ArgBi, string ArgDate, string ArgVCode)
        {
            //'-------------------------------------------------------------------------
            //'   중증 질환자 본인부담율을 구함
            //'   ①ArgBi: 환자종류 ②ArgDate:처방,퇴원일자 ③ArgVCode:V코드(V191~V194)
            //'   Ex: nIpdBonRate = READ_Cancer_BonRate("11","2005-09-20","V193")
            //'   조건에 맞으면 0~100을 반환하고, 오류가 있으면 -1을 반환한다.
            //'-------------------------------------------------------------------------

            int rtnVal = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //보험환자만 중증질환 본인부담이 경각됨
            //2005-12-05일부터 보호, 보험 환자 적용됨

            if (Convert.ToInt32(ArgBi) > 25)
            {
                rtnVal = -1;
                return rtnVal;
            }

            //DB에 저장된 값을 읽음
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                             ";
            SQL += ComNum.VBLF + "  RateValue,TO_CHAR(StartDate,'YYYY-MM-DD') StartDate              ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT                             ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                          ";
            SQL += ComNum.VBLF + "      AND IdName = 'JUNG_BON'                                      ";
            SQL += ComNum.VBLF + "      AND StartDate<=TO_DATE('" + ArgDate.Trim() + "','YYYY-MM-DD')";
            switch (ArgVCode)
            {
                case "V191":
                    SQL += ComNum.VBLF + "AND ArrayClass=1                                           ";    //개두술
                    break;
                case "V192":
                    SQL += ComNum.VBLF + "AND ArrayClass=2                                           ";    //개심술
                    break;
                case "V193":
                    SQL += ComNum.VBLF + "AND ArrayClass=3                                           ";    //등록암
                    break;
                case "V194":
                    SQL += ComNum.VBLF + "AND ArrayClass=4                                           ";    //등록암 가정간호
                    break;
                case "F003":
                    SQL += ComNum.VBLF + "AND ArrayClass=5                                           ";    //의약분업 환자 약값 30%
                    break;
                case "V268":
                    SQL += ComNum.VBLF + "AND ArrayClass=6                                           ";    //뇌출혈환자 2015-04-06
                    break;
                case "V275":
                    SQL += ComNum.VBLF + "AND ArrayClass=7                                           ";    //뇌경색환자 2015-04-06
                    break;

                default:
                    rtnVal = -1;
                    return rtnVal;
            }
            SQL += ComNum.VBLF + "ORDER BY StartDate DESC";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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
                    rtnVal = Convert.ToInt32(dt.Rows[0]["RateValue"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;


                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

        }

        /// <summary>
        /// Description : 입원 본인부담율 구함
        /// Author : 안정수
        /// Create Date : 2017.09.05
        /// </summary>
        /// <param name="ArgBi"></param>
        /// <param name="ArgGDate"></param>
        /// <seealso cref="patient_rate.bas : READ_IpdBon_Rate_CHK"/>
        public int READ_IpdBon_Rate_CHK(PsmhDb pDbCon, string ArgBi, string ArgGDate)
        {
            //'-----------------------------------------------------------
            //'   환자종류 및 기준일자로 입원본인부담율을 구함
            //'   ex: nBonRate = READ_IpdBon_Rate("12","2005-06-28")
            //'-----------------------------------------------------------
            int rtnVal = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  TO_CHAR(StartDate,'YYYY-MM-DD') StartDate,RateValue             ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT                            ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
            SQL += ComNum.VBLF + "      AND IDNAME='IPD_BON'                                        ";
            SQL += ComNum.VBLF + "      AND ArrayClass=" + VB.Val(ArgBi) + "                        ";
            SQL += ComNum.VBLF + "      AND StartDate<=TO_DATE('" + ArgGDate + "','YYYY-MM-DD')     ";
            SQL += ComNum.VBLF + "ORDER BY StartDate DESC                                           ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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
                    rtnVal = Convert.ToInt32(dt.Rows[0]["RateValue"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

        }

        /// <summary>
        /// Description : 외래 본인부담율 구함
        /// Author : 안정수
        /// Create Date : 2017.09.05
        /// </summary>
        /// <param name="ArgBi"></param>
        /// <param name="ArgGDate"></param>
        /// <seealso cref="patient_rate.bas : READ_OpdBonin_Rate_CHK"/>
        public int READ_OpdBonin_Rate_CHK(PsmhDb pDbCon, string ArgBi, string ArgGDate)
        {
            //'-----------------------------------------------------------
            //'   환자종류 및 기준일자로 외래본인부담율을 구함
            //'   ex: nOpdBonRate = READ_OpdBonin_Rate("12","2005-06-28")
            //'-----------------------------------------------------------
            int rtnVal = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  TO_CHAR(StartDate,'YYYY-MM-DD') StartDate,RateValue             ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT                            ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
            SQL += ComNum.VBLF + "      AND IDNAME='OPD_BON'                                        ";
            SQL += ComNum.VBLF + "      AND ArrayClass=" + VB.Val(ArgBi) + "                        ";
            SQL += ComNum.VBLF + "      AND StartDate<=TO_DATE('" + ArgGDate + "','YYYY-MM-DD')     ";
            SQL += ComNum.VBLF + "ORDER BY StartDate DESC                                           ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
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
                    rtnVal = Convert.ToInt32(dt.Rows[0]["RateValue"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// Description : 환자종별 기술가산율 구함
        /// Author : 김민철
        /// Create Date : 2017.09.07
        /// </summary>
        /// <param name="ArgBi"></param>
        /// <param name="ArgGDate"></param>
        /// <returns></returns>
        /// <seealso cref="BASACCT.bas : READ_Gisul_Rate"/>
        public int READ_Gisul_Rate(PsmhDb pDbCon, string ArgBi, string ArgGDate)
        {
            //'-----------------------------------------------------------
            //'   환자종류 및 기준일자로 병원가산율을 구함
            //'   ex: nGisulRage = READ_Gisul_Rate("12","2005-06-28")
            //'-----------------------------------------------------------
            int rtnVal = 0;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  TO_CHAR(StartDate,'YYYY-MM-DD') StartDate,RateValue             ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT                            ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                         ";
            SQL += ComNum.VBLF + "      AND IDNAME='GISUL'                                          ";
            SQL += ComNum.VBLF + "      AND ArrayClass=" + VB.Val(ArgBi) + "                        ";
            SQL += ComNum.VBLF + "      AND StartDate<=TO_DATE('" + ArgGDate + "','YYYY-MM-DD')     ";
            SQL += ComNum.VBLF + "ORDER BY StartDate DESC                                           ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = Convert.ToInt32(dt.Rows[0]["RateValue"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public string Ins_Ipd_Mst(string[] Arg, PsmhDb pDbCon, ref int intRowAffected)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "IPD_NEW_MASTER (                                                   ";
            SQL += ComNum.VBLF + "        IPDNO,            WardCode,           RoomCode,           Pano,               Bi,             ";
            SQL += ComNum.VBLF + "        Sname,            Sex,                Age,                InDate,             WardInTime,     ";
            SQL += ComNum.VBLF + "        DeptCode,         DrCode,             Ilsu,               Pname,              GbSpc,          ";
            SQL += ComNum.VBLF + "        GbSPC2,           GbKekli,            Bohun,              Religion,           Jiyuk,          ";
            SQL += ComNum.VBLF + "        IpwonTime,        ArcQty,             IcuQty,             Im180,              Fee6,           ";
            SQL += ComNum.VBLF + "        GbGameK,          AmSet1,             AmSet4,             AmSet5,             AmSet6,         ";
            SQL += ComNum.VBLF + "        AmSet7,           AmSet8,             AmSet9,             AmSetA,             GelCode,        ";
            SQL += ComNum.VBLF + "        FromTrans,        ErAmt,              Remark,             JupboNo,            article,        ";
            SQL += ComNum.VBLF + "        Gbcancer,         InOut,              Other,              GbDonggi,           OgPdBun,        ";
            SQL += ComNum.VBLF + "        TrsCnt,           LastTrs,            GbSTS,              OUTDRUG,            OUTDEPT,        ";
            SQL += ComNum.VBLF + "        GBSUDAY,          Secret,             Secret_sabun,       PNEUMONIA,          Pregnant,       ";
            SQL += ComNum.VBLF + "        GbGoOut,          GbNight,            TelRemark,          GbExam,             THYROID,        ";
            SQL += ComNum.VBLF + "        GbDrg,            DrgCode,            JDate,              JobSabun,           SECRETINDATE,   ";
            SQL += ComNum.VBLF + "        KTASLEVL,         FROOM,              FROOMETC,           GBJIWON,            T_CARE,         ";
            SQL += ComNum.VBLF + "        OPDNO,            PASS_INFO,          RETUN_HOSP                                              ";
            SQL += ComNum.VBLF + " ) VALUES (                                                                                           ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmIpdMst.IPDNO] + ",                                                 ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.WardCode] + "',                                             ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.RoomCode] + "',                                             ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Pano] + "',                                                 ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Bi] + "',                                                   ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Sname] + "',                                                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Sex] + "',                                                  ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Age] + "',                                                  ";
            SQL += ComNum.VBLF + "TO_DATE('" + Arg[(int)clsPmpaPb.enmIpdMst.InDate] + "','YYYY-MM-DDHH24MI'),                           ";
            SQL += ComNum.VBLF + "TO_DATE('" + Arg[(int)clsPmpaPb.enmIpdMst.WardInTime] + "','YYYY-MM-DDHH24MI'),                       ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.DeptCode] + "',                                             ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.DrCode] + "',                                               ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmIpdMst.Ilsu] + ",                                                  ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Pname] + "',                                                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.GbSpc] + "',                                                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.GbSPC2] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.GbKekli] + "',                                              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Bohun] + "',                                                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Religion] + "',                                             ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Jiyuk] + "',                                                ";
            SQL += ComNum.VBLF + "        SYSDATE,                                                                                      ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.ArcQty] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.IcuQty] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Im180] + "',                                                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Fee6] + "',                                                 ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.GbGameK] + "',                                              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.AmSet1] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.AmSet4] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.AmSet5] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.AmSet6] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.AmSet7] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.AmSet8] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.AmSet9] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.AmSetA] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.GelCode] + "',                                              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.FromTrans] + "',                                            ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.ErAmt] + "',                                                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Remark] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.JupboNo] + "',                                              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.article] + "',                                              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Gbcancer] + "',                                             ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.InOut] + "',                                                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Other] + "',                                                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.GbDonggi] + "',                                             ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.OgPdBun] + "',                                              ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmIpdMst.TrsCnt] + ",                                                ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmIpdMst.LastTrs] + ",                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.GbSTS] + "',                                                ";
            SQL += ComNum.VBLF + "TO_DATE('" + Arg[(int)clsPmpaPb.enmIpdMst.OUTDRUG] + "','YYYY-MM-DD'),                                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.OUTDEPT] + "',                                              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.GBSUDAY] + "',                                              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Secret] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Secret_sabun] + "',                                         ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.PNEUMONIA] + "',                                            ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.Pregnant] + "',                                             ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.GbGoOut] + "',                                              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.GbNight] + "',                                              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.TelRemark] + "',                                            ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.GbExam] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.THYROID] + "',                                              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.GbDrg] + "',                                                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.DrgCode] + "',                                              ";
            SQL += ComNum.VBLF + "TO_DATE('" + Arg[(int)clsPmpaPb.enmIpdMst.JDate] + "','YYYY-MM-DD'),                                  ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.JobSabun] + "',                                             ";
            SQL += ComNum.VBLF + "TO_DATE('" + Arg[(int)clsPmpaPb.enmIpdMst.SECRETINDATE] + "','YYYY-MM-DD HH24:MI'),                           ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.KTASLEVL] + "',                                             ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.FROOM] + "',                                                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.FROOMETC] + "',                                             ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.GBJIWON] + "',                                              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.T_CARE] + "',                                               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.OPDNO] + "',                                                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.PASSINFO] + "',                                             ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdMst.RTNHOSP] + "'                                               ";
            SQL += ComNum.VBLF + "          )";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string Ins_Ipd_Trs(string[] Arg, PsmhDb pDbCon, ref int intRowAffected)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "IPD_TRANS (                         ";
            SQL += ComNum.VBLF + " TRSNO,IPDNO,PANO,GBIPD,INDATE, OUTDATE,DEPTCODE,DRCODE,ILSU,BI,      ";
            SQL += ComNum.VBLF + " KIHO,GKIHO,PNAME,GWANGE,BONRATE,GISULRATE,GBGAMEK,BOHUN,             ";
            SQL += ComNum.VBLF + " AMSET1,AMSET2,AMSET3,AMSET4,AMSET5,AMSETB,FROMTRANS,ERAMT,           ";
            SQL += ComNum.VBLF + " JUPBONO,GbSPC,GBDRG,DRGCODE,                                         ";
            SQL += ComNum.VBLF + " AMT01,AMT02,AMT03,AMT04,AMT05,AMT06,AMT07,AMT08,AMT09,AMT10,         ";
            SQL += ComNum.VBLF + " AMT11,AMT12,AMT13,AMT14,AMT15,AMT16,AMT17,AMT18,AMT19,AMT20,         ";
            SQL += ComNum.VBLF + " AMT21,AMT22,AMT23,AMT24,AMT25,AMT26,AMT27,AMT28,AMT29,AMT30,         ";
            SQL += ComNum.VBLF + " AMT31,AMT32,AMT33,AMT34,AMT35,AMT36,AMT37,AMT38,AMT39,AMT40,         ";
            SQL += ComNum.VBLF + " AMT41,AMT42,AMT43,AMT44,AMT45,AMT46,AMT47,AMT48,AMT49,AMT50,         ";
            SQL += ComNum.VBLF + " AMT51,AMT52,AMT53,AMT54,AMT55,AMT56,AMT57,AMT58,AMT59,AMT60,         ";
            SQL += ComNum.VBLF + " AMT64,ENTDATE,ENTSABUN,GBSTS,VCODE,OGPDBUN,OGPDBUNdtl,GELCODE,       ";
            SQL += ComNum.VBLF + " Gbilban2,KTASLEVL )                                                  ";
            SQL += ComNum.VBLF + " VALUES (                                                             ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmIpdTrs.TRSNO] + ",                 ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmIpdTrs.IPDNO] + ",                 ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.PANO] + "',                 ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.GBIPD] + "',                ";
            SQL += ComNum.VBLF + "TO_DATE('" + Arg[(int)clsPmpaPb.enmIpdTrs.INDATE] + "','YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.OUTDATE] + "',              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.DEPTCODE] + "',             ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.DRCODE] + "',               ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmIpdTrs.ILSU] + ",                  ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.BI] + "',                   ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.KIHO] + "',                 ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.GKIHO] + "',                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.PNAME] + "',                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.GWANGE] + "',               ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmIpdTrs.BONRATE] + ",               ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmIpdTrs.GISULRATE] + ",             ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.GBGAMEK] + "',              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.BOHUN] + "',                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.AMSET1] + "',               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.AMSET2] + "',               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.AMSET3] + "',               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.AMSET4] + "',               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.AMSET5] + "',               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.AMSETB] + "',               ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.FROMTRANS] + "',            ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmIpdTrs.ERAMT] + ",                 ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.JUPBONO] + "',              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.GbSPC] + "',                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.GBDRG] + "',                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.DRGCODE] + "',              ";
            for (int i = 30; i < 90; i++)
            {
                //AMT01 - AMT60 까지 변수 세팅
                SQL += ComNum.VBLF + "         " + Arg[i] + ",                                          ";
            }
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmIpdTrs.AMT64] + ",                 ";
            SQL += ComNum.VBLF + "        SYSDATE,                                                      ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmIpdTrs.ENTSABUN] + ",              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.GBSTS] + "',                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.VCODE] + "',                ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.OGPDBUN] + "',              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.OGPDBUNdtl] + "',           ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.GELCODE] + "',              ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.Gbilban2] + "',             ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmIpdTrs.KTASLEVL] + "'              ";
            SQL += ComNum.VBLF + "        )";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string Ins_Bas_Pat(string[] Arg, PsmhDb pDbCon, ref int intRowAffected)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_PATIENT (                               ";
            SQL += ComNum.VBLF + " Pano, Sname, Sex, Jumin1, Jumin2, Jumin3, ZipCode1, ZipCode2, Juso,          ";
            SQL += ComNum.VBLF + " StartDate, LastDate, JiCode, Tel, EmbPrt, Bi, Pname, Gwange, Kiho,           ";
            SQL += ComNum.VBLF + " GKiho, DeptCode, DrCode, GbSpc, GbGameK, Jinilsu, JinAmt, TuyakGwa,          ";
            SQL += ComNum.VBLF + " TuyakMonth, TuyakJulDate, TuyakIlsu, Bohun, Religion, Remark, Sabun,         ";
            SQL += ComNum.VBLF + " Birth, GbBirth, Email, HPhone, Jikup, GbJuger, RoadDetail, BuildNo, ZipCode3)";
            SQL += ComNum.VBLF + " VALUES (                                                                     ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Pano] + "',                         ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Sname] + "',                        ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Sex] + "',                          ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Jumin1] + "',                       ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Jumin2] + "',                       ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Jumin3] + "',                       ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.ZipCode1] + "',                     ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.ZipCode2] + "',                     ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Juso] + "',                         ";
            SQL += ComNum.VBLF + "SYSDATE,                                                                      ";
            SQL += ComNum.VBLF + "SYSDATE,                                                                      ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.JiCode] + "',                       ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Tel] + "',                          ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.EmbPrt] + "',                       ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Bi] + "',                           ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Pname] + "',                        ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Gwange] + "',                       ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Kiho] + "',                         ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.GKiho] + "',                        ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.DeptCode] + "',                     ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.DrCode] + "',                       ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.GbSpc] + "',                        ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.GbGameK] + "',                      ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmBasPat.Jinilsu] + ",                       ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmBasPat.JinAmt] + ",                        ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmBasPat.TuyakGwa] + ",                      ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmBasPat.TuyakMonth] + ",                    ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.TuyakJulDate] + "',                 ";
            SQL += ComNum.VBLF + "         " + Arg[(int)clsPmpaPb.enmBasPat.TuyakIlsu] + ",                     ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Bohun] + "',                        ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Religion] + "',                     ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Remark] + "',                       ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Sabun] + "',                        ";
            SQL += ComNum.VBLF + "TO_DATE('" + Arg[(int)clsPmpaPb.enmBasPat.Birth] + "','YYYY-MM-DD'),          ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.GbBirth] + "',                      ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Email] + "',                        ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.HPhone] + "',                       ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.Jikup] + "',                        ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.GbJuger] + "',                      ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.RoadDetail] + "',                   ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.BuildNo] + "',                      ";
            SQL += ComNum.VBLF + "        '" + Arg[(int)clsPmpaPb.enmBasPat.ZipCode3] + "'                      ";
            SQL += ComNum.VBLF + "        )                                                                     ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string Up_Bas_Pat(string[] Arg, PsmhDb pDbCon, ref int intRowAffected)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            //#region 환자인적사항 변경 내역 백업
            //ComFunc CF1 = new ComFunc();
            //Dictionary<string, string> dict = new Dictionary<string, string>();
            //dict.Add("SNAME", Arg[(int)clsPmpaPb.enmBasPat.Sname]);
            //dict.Add("SEX", Arg[(int)clsPmpaPb.enmBasPat.Sex]);
            //dict.Add("JUMIN1", Arg[(int)clsPmpaPb.enmBasPat.Jumin1]);
            //dict.Add("JUMIN2", Arg[(int)clsPmpaPb.enmBasPat.Jumin2]);
            //dict.Add("JUMIN3", Arg[(int)clsPmpaPb.enmBasPat.Jumin3]);
            //dict.Add("ZIPCODE1", Arg[(int)clsPmpaPb.enmBasPat.ZipCode1]);
            //dict.Add("ZIPCODE2", Arg[(int)clsPmpaPb.enmBasPat.ZipCode2]);
            //dict.Add("ZIPCODE3", Arg[(int)clsPmpaPb.enmBasPat.ZipCode3]);
            //dict.Add("BUILDNO", Arg[(int)clsPmpaPb.enmBasPat.BuildNo]);
            //dict.Add("ROADDETAIL", Arg[(int)clsPmpaPb.enmBasPat.RoadDetail]);
            //dict.Add("JUSO", Arg[(int)clsPmpaPb.enmBasPat.Juso]);
            //dict.Add("TEL", Arg[(int)clsPmpaPb.enmBasPat.Tel]);
            //dict.Add("BIRTH", Arg[(int)clsPmpaPb.enmBasPat.Birth]);
            //dict.Add("HPHONE", Arg[(int)clsPmpaPb.enmBasPat.HPhone]);
            //CF1.INSERT_BAS_PATIENT_HIS(Arg[(int)clsPmpaPb.enmBasPat.Pano], dict);
            //#endregion


            SQL = "";
            SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT                                                 ";
            SQL += ComNum.VBLF + "    SET Sname      = '" + Arg[(int)clsPmpaPb.enmBasPat.Sname] + "',                       ";
            SQL += ComNum.VBLF + "        Sex        = '" + Arg[(int)clsPmpaPb.enmBasPat.Sex] + "',                         ";
            SQL += ComNum.VBLF + "        Jumin1     = '" + Arg[(int)clsPmpaPb.enmBasPat.Jumin1] + "',                      ";
            SQL += ComNum.VBLF + "        Jumin2     = '" + Arg[(int)clsPmpaPb.enmBasPat.Jumin2] + "',                      ";
            SQL += ComNum.VBLF + "        Jumin3     = '" + Arg[(int)clsPmpaPb.enmBasPat.Jumin3] + "',                      ";
            SQL += ComNum.VBLF + "        ZipCode1   = '" + Arg[(int)clsPmpaPb.enmBasPat.ZipCode1] + "',                    ";
            SQL += ComNum.VBLF + "        ZipCode2   = '" + Arg[(int)clsPmpaPb.enmBasPat.ZipCode2] + "',                    ";
            SQL += ComNum.VBLF + "        ZipCode3   = '" + Arg[(int)clsPmpaPb.enmBasPat.ZipCode3] + "',                    ";
            SQL += ComNum.VBLF + "        BUILDNO    = '" + Arg[(int)clsPmpaPb.enmBasPat.BuildNo] + "',                     ";
            SQL += ComNum.VBLF + "        ROADDETAIL = '" + Arg[(int)clsPmpaPb.enmBasPat.RoadDetail] + "',                  ";
            SQL += ComNum.VBLF + "        Juso       = '" + Arg[(int)clsPmpaPb.enmBasPat.Juso] + "',                        ";
            SQL += ComNum.VBLF + "        JiCode     = '" + Arg[(int)clsPmpaPb.enmBasPat.JiCode] + "',                      ";
            SQL += ComNum.VBLF + "        Tel        = '" + Arg[(int)clsPmpaPb.enmBasPat.Tel] + "',                         ";
            SQL += ComNum.VBLF + "        Bi         = '" + Arg[(int)clsPmpaPb.enmBasPat.Bi] + "',                          ";
            SQL += ComNum.VBLF + "        Pname      = '" + Arg[(int)clsPmpaPb.enmBasPat.Pname] + "',                       ";
            SQL += ComNum.VBLF + "        Sabun      = '" + Arg[(int)clsPmpaPb.enmBasPat.Sabun] + "',                       ";
            SQL += ComNum.VBLF + "        Gwange     = '" + Arg[(int)clsPmpaPb.enmBasPat.Gwange] + "',                      ";
            SQL += ComNum.VBLF + "        Kiho       = '" + Arg[(int)clsPmpaPb.enmBasPat.Kiho] + "',                        ";
            SQL += ComNum.VBLF + "        GKiho      = '" + Arg[(int)clsPmpaPb.enmBasPat.GKiho] + "',                       ";
            SQL += ComNum.VBLF + "        Bohun      = '" + Arg[(int)clsPmpaPb.enmBasPat.Bohun] + "',                       ";
            SQL += ComNum.VBLF + "        GbGameK    = '" + Arg[(int)clsPmpaPb.enmBasPat.GbGameK] + "',                     ";
            SQL += ComNum.VBLF + "        Religion   = '" + Arg[(int)clsPmpaPb.enmBasPat.Religion] + "',                    ";
            SQL += ComNum.VBLF + "        Remark     = '" + Arg[(int)clsPmpaPb.enmBasPat.Remark] + "',                      ";
            SQL += ComNum.VBLF + "        Birth      = TO_DATE('" + Arg[(int)clsPmpaPb.enmBasPat.Birth] + "','YYYY-MM-DD'), ";
            SQL += ComNum.VBLF + "        GbBirth    = '" + Arg[(int)clsPmpaPb.enmBasPat.GbBirth] + "',                     ";
            SQL += ComNum.VBLF + "        EMail      = '" + Arg[(int)clsPmpaPb.enmBasPat.Email] + "',                       ";
            SQL += ComNum.VBLF + "        HPhone     = '" + Arg[(int)clsPmpaPb.enmBasPat.HPhone] + "',                      ";
            SQL += ComNum.VBLF + "        Jikup      = '" + Arg[(int)clsPmpaPb.enmBasPat.Jikup] + "',                       ";
            SQL += ComNum.VBLF + "        GbJuger    = '" + Arg[(int)clsPmpaPb.enmBasPat.GbJuger] + "',                     ";
            SQL += ComNum.VBLF + "        BIDATE     = TO_DATE('" + Arg[(int)clsPmpaPb.enmBasPat.BiDate] + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  WHERE PANO      = '" + Arg[(int)clsPmpaPb.enmBasPat.Pano] + "'                        ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// Description : 해당 월(MM) 계산 ex)201708 -> 201707 or 201709
        /// Author : 안정수
        /// Create Date : 2017.09.07
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argADD"></param>
        /// <seealso cref="VBFunction.bas : DATE_YYMM_ADD"/>
        public string DATE_YYMM_ADD(string ArgYYMM, int argADD)
        {
            string rtnVal = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int ArgI = 0;
            int ArgJ = 0;
            int ArgYY = 0;
            int ArgMM = 0;

            if (ArgYYMM.Length != 6 || argADD == 0)
            {
                return ArgYYMM;
            }

            ArgYY = Convert.ToInt32(VB.Left(ArgYYMM, 4));
            ArgMM = Convert.ToInt32(VB.Right(ArgYYMM, 2));

            ArgJ = argADD;

            if (ArgJ < 0)
            {
                ArgJ = ArgJ * -1;
            }

            for (ArgI = 1; ArgI <= ArgJ; ArgI++)
            {
                if (argADD < 0)
                {
                    ArgMM -= 1;
                    if (ArgMM == 0)
                    {
                        ArgMM = 12;
                        ArgYY -= 1;
                    }
                }
                else
                {
                    ArgMM += 1;
                    if (ArgMM == 13)
                    {
                        ArgYY += 1;
                        ArgMM = 1;
                    }
                }

            }

            rtnVal = ComFunc.SetAutoZero(ArgYY.ToString(), 4) + ComFunc.SetAutoZero(ArgMM.ToString(), 2);
            return rtnVal;
        }

        /// <summary>
        /// Description : 의사명 가져오기
        /// Author : 박병규
        /// Create Date : 2017.09.11
        /// <param name="ArgCode"></param>
        /// </summary>
        /// <seealso cref="oumsad.bas:Read_DoctorName"/>
        public string READ_DOCTOR_NAME(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DRNAME ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND DRCODE    = '" + ArgCode + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = "";
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["DRNAME"].ToString().Trim();
            else
                rtnVal = "";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 해당일자 의료급여 승인여부(True:승인 False:미승인)
        /// Author : 박병규
        /// Create Date : 2017.09.12
        /// <param name="ArgPtno"></param>
        /// <param name="ArgBdate"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgDeptCode"></param>
        /// </summary>
        /// <seealso cref="vb의료급여승인.bas:GET_BohoApprove_No"/>
        public string GET_BOHO_APPROVENO(PsmhDb pDbCon, string ArgPtno, string ArgBdate, string ArgBi, string ArgDeptCode)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT MSeqNo, BOHO_WRTNO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND Bi        = '" + ArgBi + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDeptCode + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = "";
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["MSeqNo"].ToString().Trim();

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : Ipd_New_Slip 테이블 Insert
        /// Author : 김민철
        /// Create Date : 2017.10.01
        /// </summary>
        /// <param name="Arg"></param>
        /// <param name="Dt"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string Ins_IpdNewSlip(string[] Arg, PsmhDb pDbCon, ref int intRowAffected)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "IPD_NEW_SLIP (                             ";
            SQL += ComNum.VBLF + "        IPDNO,TRSNO,ACTDATE,PANO,BI,BDATE,ENTDATE,SUNEXT,BUN,NU,QTY,NAL,      ";
            SQL += ComNum.VBLF + "        BASEAMT,GBSPC,GBNGT,GBGISUL,GBSELF,GBCHILD,DEPTCODE,DRCODE,WARDCODE,  ";
            SQL += ComNum.VBLF + "        SUCODE,GBSLIP,GBHOST,PART,AMT1, AMT2 , SEQNO, YYMM, DRGSELF, ORDERNO, ";
            SQL += ComNum.VBLF + "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,RoomCode,  ";
            SQL += ComNum.VBLF + "        DIV,GBSELNOT,GBSUGBS,GBER,GBSGADD, CBUN, CSUNEXT, CSUCODE, GBSUGBAB,  ";
            SQL += ComNum.VBLF + "        GBSUGBAC,GBSUGBAD,BCODE,OPGUBUN,HIGHRISK,GBOP,GBNGT2 ,POWDER,ASADD )  ";
            SQL += ComNum.VBLF + " VALUES (                                                                     ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmIpdNewSlip.IPDNO] + ",                    ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmIpdNewSlip.TRSNO] + ",                    ";
            SQL += ComNum.VBLF + " TO_DATE('" + Arg[(int)clsPmpaPb.enmIpdNewSlip.ACTDATE] + "','YYYY-MM-DD'),   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.PANO] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.BI] + "',                      ";
            SQL += ComNum.VBLF + " TO_DATE('" + Arg[(int)clsPmpaPb.enmIpdNewSlip.BDATE] + "','YYYY-MM-DD'),     ";
            SQL += ComNum.VBLF + " SYSDATE,                                                                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.SUNEXT] + "',                  ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.BUN] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.NU] + "',                      ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.QTY] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.NAL] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.BASEAMT] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSPC] + "',                   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBNGT] + "',                   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBGISUL] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSELF] + "',                  ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBCHILD] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.DEPTCODE] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.DRCODE] + "',                  ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.WARDCODE] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.SUCODE] + "',                  ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSLIP] + "',                  ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBHOST] + "',                  ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.PART] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.AMT1] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.AMT2] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.SEQNO] + "',                   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.YYMM] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.DRGSELF] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.ORDERNO] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.ABCDATE] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.OPER_DEPT] + "',               ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.OPER_DCT] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DEPT] + "',              ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DCT] + "',               ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.EXAM_WRTNO] + "',              ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.RoomCode] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.DIV] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSELNOT] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBS] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBER] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSGADD] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.CBUN] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.CSUNEXT] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.CSUCODE] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAB] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAC] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAD] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.BCODE] + "',                   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.OPGUBUN] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.HIGHRISK] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBOP] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBNGT2] + "',                   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.POWDER] + "',                   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.ASADD] + "'                   ";
            SQL += ComNum.VBLF + "        )                                                                     ";

            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// Description : Test 용 Ipd_New_Slip 테이블 Insert
        /// Author : 김민철
        /// Create Date : 2018.09.18
        /// </summary>
        /// <param name="Arg"></param>
        /// <param name="Dt"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string Ins_IpdNewSlip_Test(string[] Arg, PsmhDb pDbCon, ref int intRowAffected)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "IPD_NEW_SLIP_TEST (                        ";
            SQL += ComNum.VBLF + "        IPDNO,TRSNO,ACTDATE,PANO,BI,BDATE,ENTDATE,SUNEXT,BUN,NU,QTY,NAL,      ";
            SQL += ComNum.VBLF + "        BASEAMT,GBSPC,GBNGT,GBGISUL,GBSELF,GBCHILD,DEPTCODE,DRCODE,WARDCODE,  ";
            SQL += ComNum.VBLF + "        SUCODE,GBSLIP,GBHOST,PART,AMT1, AMT2 , SEQNO, YYMM, DRGSELF, ORDERNO, ";
            SQL += ComNum.VBLF + "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,RoomCode,  ";
            SQL += ComNum.VBLF + "        DIV,GBSELNOT,GBSUGBS,GBER,GBSGADD, CBUN, CSUNEXT, CSUCODE, GBSUGBAB,  ";
            SQL += ComNum.VBLF + "        GBSUGBAC,GBSUGBAD,BCODE,OPGUBUN,HIGHRISK,GBOP,GBNGT2 )                ";
            SQL += ComNum.VBLF + " VALUES (                                                                     ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmIpdNewSlip.IPDNO] + ",                    ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmIpdNewSlip.TRSNO] + ",                    ";
            SQL += ComNum.VBLF + " TO_DATE('" + Arg[(int)clsPmpaPb.enmIpdNewSlip.ACTDATE] + "','YYYY-MM-DD'),   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.PANO] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.BI] + "',                      ";
            SQL += ComNum.VBLF + " TO_DATE('" + Arg[(int)clsPmpaPb.enmIpdNewSlip.BDATE] + "','YYYY-MM-DD'),     ";
            SQL += ComNum.VBLF + " SYSDATE,                                                                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.SUNEXT] + "',                  ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.BUN] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.NU] + "',                      ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.QTY] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.NAL] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.BASEAMT] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSPC] + "',                   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBNGT] + "',                   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBGISUL] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSELF] + "',                  ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBCHILD] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.DEPTCODE] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.DRCODE] + "',                  ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.WARDCODE] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.SUCODE] + "',                  ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSLIP] + "',                  ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBHOST] + "',                  ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.PART] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.AMT1] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.AMT2] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.SEQNO] + "',                   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.YYMM] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.DRGSELF] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.ORDERNO] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.ABCDATE] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.OPER_DEPT] + "',               ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.OPER_DCT] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DEPT] + "',              ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DCT] + "',               ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.EXAM_WRTNO] + "',              ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.RoomCode] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.DIV] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSELNOT] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBS] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBER] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSGADD] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.CBUN] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.CSUNEXT] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.CSUCODE] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAB] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAC] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAD] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.BCODE] + "',                   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.OPGUBUN] + "',                 ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.HIGHRISK] + "',                ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBOP] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmIpdNewSlip.GBNGT2] + "'                   ";
            SQL += ComNum.VBLF + "        )                                                                     ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        public string del_IpdNewSlip(PsmhDb pDbCon, string ArgRowid, ref int intRowAffected)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + " IPD_NEW_SLIP (                                          \r\n";
            SQL += "        IPDNO,TRSNO,ACTDATE,PANO,BI,BDATE,ENTDATE,SUNEXT,BUN,NU,QTY,NAL,BASEAMT,            \r\n";
            SQL += "        GBSPC,GBNGT,GBGISUL,GBSELF,GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,   \r\n";
            SQL += "        PART,AMT1,AMT2,SEQNO,YYMM,DRGSELF,ORDERNO,ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,    \r\n";
            SQL += "        ORDER_DCT,EXAM_WRTNO,ROOMCODE,DIV,AMT3,WRTNO,BUILDDATE,GBSELNOT,GBSUGBS,GBOP,       \r\n";
            SQL += "        PART2,BONRATE,GBER,GBER2,GBSGADD,CBUN,CSUNEXT,CSUCODE,GBSUGBAB,GBSUGBAC,GBSUGBAD,   \r\n";
            SQL += "        BCODE,GBNGT2 )                                                                      \r\n";
            SQL += " SELECT IPDNO,TRSNO,TRUNC(SYSDATE),PANO,BI,BDATE,SYSDATE,SUNEXT,BUN,NU,QTY,NAL*-1,BASEAMT,  \r\n";
            SQL += "        GBSPC,GBNGT,GBGISUL,GBSELF,GBCHILD,DEPTCODE,DRCODE,WARDCODE,SUCODE,GBSLIP,GBHOST,   \r\n";
            SQL += "        '" + clsType.User.IdNumber + "', AMT1*-1, AMT2*-1, SEQNO,YYMM,DRGSELF,ORDERNO,      \r\n";
            SQL += "        ABCDATE,OPER_DEPT,OPER_DCT,ORDER_DEPT,ORDER_DCT,EXAM_WRTNO,ROOMCODE, DIV,AMT3,      \r\n";
            SQL += "        WRTNO,BUILDDATE,GBSELNOT,GBSUGBS,GBOP,PART2,BONRATE,GBER,GBER2,GBSGADD,CBUN,        \r\n";
            SQL += "        CSUNEXT,CSUCODE,GBSUGBAB,GBSUGBAC,GBSUGBAD,BCODE,GBNGT2                             \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP                                                  \r\n";
            SQL += "  WHERE ROWID = '" + ArgRowid + "'                                                          \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// Description : Opd_Slip 테이블 Insert
        /// Author : 김민철
        /// Create Date : 2017.10.01
        /// </summary>
        /// <param name="Arg"></param>
        /// <param name="Dt"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string Ins_OpdSlip(string[] Arg, PsmhDb pDbCon, ref int intRowAffected)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SLIP (                                 ";
            SQL += ComNum.VBLF + "        ActDate,Pano,Bi,BDate,EntDate,SuNext,Bun,Nu,Qty,Nal,BaseAmt,          ";
            SQL += ComNum.VBLF + "        GbSpc,GbNgt,GbGisul,GbSelf,GbChild,DeptCode,DrCode,WardCode,SuCode,   ";
            SQL += ComNum.VBLF + "        GbSlip,GbHost,Part,Amt1,Amt2,SeqNo,OrderNo,GbImiv,YYMM,GbBunup,       ";
            SQL += ComNum.VBLF + "        DosCode, CardSeqNo,DIV,DUR,KSJIN,OgAmt,GBSUGBS,GBER )                 ";
            SQL += ComNum.VBLF + " VALUES (                                                                     ";
            SQL += ComNum.VBLF + " TO_DATE('" + Arg[(int)clsPmpaPb.enmOpdSlip.ActDate] + "','YYYY-MM-DD'),      ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.Pano] + "',                       ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.Bi] + "',                         ";
            SQL += ComNum.VBLF + " TO_DATE('" + Arg[(int)clsPmpaPb.enmOpdSlip.BDate] + "','YYYY-MM-DD'),        ";
            SQL += ComNum.VBLF + " SYSDATE,                                                                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.SuNext] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.Bun] + "',                        ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.Nu] + "',                         ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmOpdSlip.Qty] + ",                         ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmOpdSlip.Nal] + ",                         ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmOpdSlip.BaseAmt] + ",                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.GbSpc] + "',                      ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.GbNgt] + "',                      ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.GbGisul] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.GbSelf] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.GbChild] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.DeptCode] + "',                   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.DrCode] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.WardCode] + "',                   ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.SuCode] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.GbSlip] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.GbHost] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.Part] + "',                       ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmOpdSlip.Amt1] + ",                        ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmOpdSlip.Amt2] + ",                        ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmOpdSlip.SeqNo] + ",                       ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.OrderNo] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.GbImiv] + "',                     ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmOpdSlip.YYMM] + ",                        ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.GbBunup] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.DosCode] + "',                    ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.CardSeqNo] + "',                  ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.DIV] + "',                        ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.DUR] + "',                        ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmOpdSlip.KSJIN] + ",                       ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmOpdSlip.OgAmt] + ",                       ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmOpdSlip.GBSUGBS] + ",                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmOpdSlip.GBER] + "'                        ";
            SQL += ComNum.VBLF + "        )                                                                     ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// Description : 신용카드 승인 일련번호 SEQNO 가져오기
        /// Author : 박병규
        /// Create Date : 2017.09.13
        /// </summary>
        public long GET_NEXT_CARDSEQNO(PsmhDb pDbCon)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            long rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT KOSMOS_PMPA.SEQ_CARDSEQNO.NEXTVAL CARDSEQNO  ";
            SQL += ComNum.VBLF + "   FROM DUAL";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            rtnVal = long.Parse(DtFunc.Rows[0]["CARDSEQNO"].ToString());

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 거래 일련번호 SEQNO 가져오기
        /// Author : 박병규
        /// Create Date : 2017.09.13
        /// </summary>
        public long GET_NEXT_CDSEQNO(PsmhDb pDbCon)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            long rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT KOSMOS_PMPA.SEQ_CDSEQNO.NEXTVAL CDSEQNO ";
            SQL += ComNum.VBLF + "   FROM DUAL";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            rtnVal = long.Parse(DtFunc.Rows[0]["CDSEQNO"].ToString());

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }
        public bool Suga_Read2(PsmhDb pDbCon, string ArgSuCode)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            #region RS Clear
            clsPmpaType.RS.SuCode = "";
            clsPmpaType.RS.SuNext = "";
            clsPmpaType.RS.Bun = "";
            clsPmpaType.RS.Nu = "";
            clsPmpaType.RS.SugbA = "";
            clsPmpaType.RS.SugbB = "";
            clsPmpaType.RS.SugbC = "";
            clsPmpaType.RS.SugbD = "";
            clsPmpaType.RS.SugbE = "";
            clsPmpaType.RS.SugbF = "";
            clsPmpaType.RS.SugbG = "";
            clsPmpaType.RS.SugbH = "";
            clsPmpaType.RS.SugbI = "";
            clsPmpaType.RS.SugbJ = "";
            clsPmpaType.RS.SugbK = "";
            clsPmpaType.RS.SugbM = "";
            clsPmpaType.RS.SugbO = "";
            clsPmpaType.RS.SugbQ = "";
            clsPmpaType.RS.SugbR = "";
            clsPmpaType.RS.SugbS = "";
            clsPmpaType.RS.SugbW = "";
            clsPmpaType.RS.SugbX = "";
            clsPmpaType.RS.SugbY = "";
            clsPmpaType.RS.SugbZ = "";
            clsPmpaType.RS.SugbAA = "";
            clsPmpaType.RS.SugbAB = "";
            clsPmpaType.RS.SugbAC = "";
            clsPmpaType.RS.SugbAD = "";
            clsPmpaType.RS.TotMax = "";
            clsPmpaType.RS.IAmt = 0;
            clsPmpaType.RS.TAmt = 0;
            clsPmpaType.RS.BAmt = 0;
            clsPmpaType.RS.SuDate = "";
            clsPmpaType.RS.OldIAmt = 0;
            clsPmpaType.RS.OldTAmt = 0;
            clsPmpaType.RS.OldBAmt = 0;
            clsPmpaType.RS.SuDate3 = "";
            clsPmpaType.RS.IAmt3 = 0;
            clsPmpaType.RS.TAmt3 = 0;
            clsPmpaType.RS.BAmt3 = 0;
            clsPmpaType.RS.SuDate4 = "";
            clsPmpaType.RS.IAmt4 = 0;
            clsPmpaType.RS.TAmt4 = 0;
            clsPmpaType.RS.BAmt4 = 0;
            clsPmpaType.RS.SuDate5 = "";
            clsPmpaType.RS.IAmt5 = 0;
            clsPmpaType.RS.TAmt5 = 0;
            clsPmpaType.RS.BAmt5 = 0;
            #endregion

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT a.Sucode,  a.Sunext,  a.Bun,     a.Nu,                                     ";
                SQL += ComNum.VBLF + "       a.SugbA,   a.SugbB,   a.SugbC,   a.SugbD,  a.SugbE,  a.SugbF,  a.SugbG,    ";
                SQL += ComNum.VBLF + "       a.SugbH,   a.SugbI,   a.SugbJ,   a.SugbK,  b.SugbM,  b.SugbO,  b.SugbP,    ";
                SQL += ComNum.VBLF + "       b.SugbQ,   b.SugbR,   b.SugbS,   b.SugbW,  b.SugbX,  b.SugbY,  b.SugbZ,    ";
                SQL += ComNum.VBLF + "       a.Iamt,    a.Tamt,    a.Bamt,    TO_CHAR(a.Sudate,  'yyyy-mm-dd') Suday,   ";
                SQL += ComNum.VBLF + "       a.OldIamt, a.OldTamt, a.OldBamt, TO_CHAR(a.Sudate3, 'yyyy-mm-dd') Suday3,  ";
                SQL += ComNum.VBLF + "       a.Iamt3,   a.Tamt3,   a.Bamt3,   TO_CHAR(a.Sudate4, 'yyyy-mm-dd') Suday4,  ";
                SQL += ComNum.VBLF + "       a.Iamt4,   a.Tamt4,   a.Bamt4,   TO_CHAR(a.Sudate5, 'yyyy-mm-dd') Suday5,  ";
                SQL += ComNum.VBLF + "                                        TO_CHAR(a.DelDate, 'yyyy-mm-dd') DELDATE, ";
                SQL += ComNum.VBLF + "       a.Iamt5,   a.Tamt5,   a.Bamt5,                                    ";
                SQL += ComNum.VBLF + "       b.SugbAA,  b.SugbAB,  b.SugbAC,  b.SugbAD, b.GBNS                          ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SUH a,                                           ";
                SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_SUN b                                            ";
                SQL += ComNum.VBLF + " WHERE a.SuNext = '" + ArgSuCode + "'                                             ";
                SQL += ComNum.VBLF + "   AND a.SuNext = b.SuNext(+) ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    clsPmpaType.RS.SuCode = Dt.Rows[0]["Sucode"].ToString().Trim();
                    clsPmpaType.RS.SuNext = Dt.Rows[0]["Sunext"].ToString().Trim();
                    clsPmpaType.RS.Bun = Dt.Rows[0]["Bun"].ToString().Trim();
                    clsPmpaType.RS.Nu = Dt.Rows[0]["Nu"].ToString().Trim();
                    clsPmpaType.RS.SugbA = Dt.Rows[0]["SugbA"].ToString().Trim();
                    clsPmpaType.RS.SugbB = Dt.Rows[0]["SugbB"].ToString().Trim();
                    clsPmpaType.RS.SugbC = Dt.Rows[0]["SugbC"].ToString().Trim();
                    clsPmpaType.RS.SugbD = Dt.Rows[0]["SugbD"].ToString().Trim();
                    clsPmpaType.RS.SugbE = Dt.Rows[0]["SugbE"].ToString().Trim();
                    clsPmpaType.RS.SugbF = Dt.Rows[0]["SugbF"].ToString().Trim();
                    clsPmpaType.RS.SugbG = Dt.Rows[0]["SugbG"].ToString().Trim();
                    clsPmpaType.RS.SugbH = Dt.Rows[0]["SugbH"].ToString().Trim();
                    clsPmpaType.RS.SugbI = Dt.Rows[0]["SugbI"].ToString().Trim();
                    clsPmpaType.RS.SugbJ = Dt.Rows[0]["SugbJ"].ToString().Trim();
                    clsPmpaType.RS.SugbK = Dt.Rows[0]["SugbK"].ToString().Trim();
                    clsPmpaType.RS.SugbM = Dt.Rows[0]["SugbM"].ToString().Trim();
                    clsPmpaType.RS.SugbO = Dt.Rows[0]["SugbO"].ToString().Trim();
                    clsPmpaType.RS.SugbP = Dt.Rows[0]["SugbP"].ToString().Trim();
                    clsPmpaType.RS.SugbQ = Dt.Rows[0]["SugbQ"].ToString().Trim();
                    clsPmpaType.RS.SugbR = Dt.Rows[0]["SugbR"].ToString().Trim();
                    clsPmpaType.RS.SugbS = Dt.Rows[0]["SugbS"].ToString().Trim();
                    clsPmpaType.RS.SugbW = Dt.Rows[0]["SugbW"].ToString().Trim();
                    clsPmpaType.RS.SugbX = Dt.Rows[0]["SugbX"].ToString().Trim();
                    clsPmpaType.RS.SugbY = Dt.Rows[0]["SugbY"].ToString().Trim();
                    clsPmpaType.RS.SugbZ = Dt.Rows[0]["SugbZ"].ToString().Trim();
                    clsPmpaType.RS.SugbAA = Dt.Rows[0]["SugbAA"].ToString().Trim();
                    clsPmpaType.RS.SugbAB = Dt.Rows[0]["SugbAB"].ToString().Trim();
                    clsPmpaType.RS.SugbAC = Dt.Rows[0]["SugbAC"].ToString().Trim();
                    clsPmpaType.RS.SugbAD = Dt.Rows[0]["SugbAD"].ToString().Trim();
                    clsPmpaType.RS.GBNS = Dt.Rows[0]["GBNS"].ToString().Trim();
                    clsPmpaType.RS.IAmt = Convert.ToInt64(Dt.Rows[0]["Iamt"].ToString());
                    clsPmpaType.RS.TAmt = Convert.ToInt64(Dt.Rows[0]["Tamt"].ToString());
                    clsPmpaType.RS.BAmt = Convert.ToInt64(Dt.Rows[0]["Bamt"].ToString());
                    clsPmpaType.RS.SuDate = Dt.Rows[0]["SuDay"].ToString().Trim();
                    clsPmpaType.RS.OldIAmt = Convert.ToInt64(Dt.Rows[0]["OldIamt"].ToString());
                    clsPmpaType.RS.OldTAmt = Convert.ToInt64(Dt.Rows[0]["OldTamt"].ToString());
                    clsPmpaType.RS.OldBAmt = Convert.ToInt64(Dt.Rows[0]["OldBamt"].ToString());
                    clsPmpaType.RS.SuDate3 = Dt.Rows[0]["SuDay3"].ToString().Trim();
                    clsPmpaType.RS.IAmt3 = Convert.ToInt64(Dt.Rows[0]["Iamt3"].ToString());
                    clsPmpaType.RS.TAmt3 = Convert.ToInt64(Dt.Rows[0]["Tamt3"].ToString());
                    clsPmpaType.RS.BAmt3 = Convert.ToInt64(Dt.Rows[0]["Bamt3"].ToString());
                    clsPmpaType.RS.SuDate4 = Dt.Rows[0]["SuDay4"].ToString().Trim();
                    clsPmpaType.RS.IAmt4 = Convert.ToInt64(Dt.Rows[0]["Iamt4"].ToString());
                    clsPmpaType.RS.TAmt4 = Convert.ToInt64(Dt.Rows[0]["Tamt4"].ToString());
                    clsPmpaType.RS.BAmt4 = Convert.ToInt64(Dt.Rows[0]["Bamt4"].ToString());
                    clsPmpaType.RS.SuDate5 = Dt.Rows[0]["Suday5"].ToString().Trim();
                    clsPmpaType.RS.IAmt5 = Convert.ToInt64(Dt.Rows[0]["Iamt5"].ToString());
                    clsPmpaType.RS.TAmt5 = Convert.ToInt64(Dt.Rows[0]["Tamt5"].ToString());
                    clsPmpaType.RS.BAmt5 = Convert.ToInt64(Dt.Rows[0]["Bamt5"].ToString());
                    clsPmpaType.RS.DelDate = Dt.Rows[0]["DELDATE"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                return true;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Description : Suga 정보 읽어오기 
        /// 공용변수를 그대로 사용하여도 되고, 각 파트별 SG, ISG 구조체에 담아서 사용하는것을 권장
        /// Author : 김민철
        /// Create Date : 2017.09.30
        /// </summary>
        /// <param name="ArgSuCode"></param>
        /// <seealso cref="IPDACCT.bas : Suga_Read"/>
        /// <seealso cref="OPDACCT.bas : Suga_Read"/>
        /// 
        public bool Suga_Read(PsmhDb pDbCon, string ArgSuCode)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            #region RS Clear
            clsPmpaType.RS.SuCode = "";
            clsPmpaType.RS.SuNext = "";
            clsPmpaType.RS.Bun = "";
            clsPmpaType.RS.Nu = "";
            clsPmpaType.RS.SugbA = "";
            clsPmpaType.RS.SugbB = "";
            clsPmpaType.RS.SugbC = "";
            clsPmpaType.RS.SugbD = "";
            clsPmpaType.RS.SugbE = "";
            clsPmpaType.RS.SugbF = "";
            clsPmpaType.RS.SugbG = "";
            clsPmpaType.RS.SugbH = "";
            clsPmpaType.RS.SugbI = "";
            clsPmpaType.RS.SugbJ = "";
            clsPmpaType.RS.SugbK = "";
            clsPmpaType.RS.SugbM = "";
            clsPmpaType.RS.SugbO = "";
            clsPmpaType.RS.SugbQ = "";
            clsPmpaType.RS.SugbR = "";
            clsPmpaType.RS.SugbS = "";
            clsPmpaType.RS.SugbW = "";
            clsPmpaType.RS.SugbX = "";
            clsPmpaType.RS.SugbY = "";
            clsPmpaType.RS.SugbZ = "";
            clsPmpaType.RS.SugbAA = "";
            clsPmpaType.RS.SugbAB = "";
            clsPmpaType.RS.SugbAC = "";
            clsPmpaType.RS.SugbAD = "";
            clsPmpaType.RS.SugbAG = "";
            clsPmpaType.RS.TotMax = "";
            clsPmpaType.RS.IAmt = 0;
            clsPmpaType.RS.TAmt = 0;
            clsPmpaType.RS.BAmt = 0;
            clsPmpaType.RS.SuDate = "";
            clsPmpaType.RS.OldIAmt = 0;
            clsPmpaType.RS.OldTAmt = 0;
            clsPmpaType.RS.OldBAmt = 0;
            clsPmpaType.RS.SuDate3 = "";
            clsPmpaType.RS.IAmt3 = 0;
            clsPmpaType.RS.TAmt3 = 0;
            clsPmpaType.RS.BAmt3 = 0;
            clsPmpaType.RS.SuDate4 = "";
            clsPmpaType.RS.IAmt4 = 0;
            clsPmpaType.RS.TAmt4 = 0;
            clsPmpaType.RS.BAmt4 = 0;
            clsPmpaType.RS.SuDate5 = "";
            clsPmpaType.RS.IAmt5 = 0;
            clsPmpaType.RS.TAmt5 = 0;
            clsPmpaType.RS.BAmt5 = 0;
            #endregion

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT a.Sucode,  a.Sunext,  a.Bun,     a.Nu,                                     ";
                SQL += ComNum.VBLF + "       a.SugbA,   a.SugbB,   a.SugbC,   a.SugbD,  a.SugbE,  a.SugbF,  a.SugbG,    ";
                SQL += ComNum.VBLF + "       a.SugbH,   a.SugbI,   a.SugbJ,   a.SugbK,  b.SugbM,  b.SugbO,  b.SugbP,    ";
                SQL += ComNum.VBLF + "       b.SugbQ,   b.SugbR,   b.SugbS,   b.SugbW,  b.SugbX,  b.SugbY,  b.SugbZ,    ";
                SQL += ComNum.VBLF + "       a.Iamt,    a.Tamt,    a.Bamt,    TO_CHAR(a.Sudate,  'yyyy-mm-dd') Suday,   ";
                SQL += ComNum.VBLF + "       a.OldIamt, a.OldTamt, a.OldBamt, TO_CHAR(a.Sudate3, 'yyyy-mm-dd') Suday3,  ";
                SQL += ComNum.VBLF + "       a.Iamt3,   a.Tamt3,   a.Bamt3,   TO_CHAR(a.Sudate4, 'yyyy-mm-dd') Suday4,  ";
                SQL += ComNum.VBLF + "       a.Iamt4,   a.Tamt4,   a.Bamt4,   TO_CHAR(a.Sudate5, 'yyyy-mm-dd') Suday5,  ";
                SQL += ComNum.VBLF + "                                        TO_CHAR(a.DelDate, 'yyyy-mm-dd') DELDATE, ";
                SQL += ComNum.VBLF + "       a.Iamt5,   a.Tamt5,   a.Bamt5,   a.TotMax,                                 ";
                SQL += ComNum.VBLF + "       b.SugbAA,  b.SugbAB,  b.SugbAC,  b.SugbAD, b.GBNS ,b.SugbAG                  ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SUT a,                                           ";
                SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_SUN b                                            ";
                SQL += ComNum.VBLF + " WHERE a.Sucode = '" + ArgSuCode + "'                                             ";
                SQL += ComNum.VBLF + "   AND a.SuNext = b.SuNext(+) ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                if (Dt.Rows.Count > 0)
                {
                    clsPmpaType.RS.SuCode = Dt.Rows[0]["Sucode"].ToString().Trim();
                    clsPmpaType.RS.SuNext = Dt.Rows[0]["Sunext"].ToString().Trim();
                    clsPmpaType.RS.Bun = Dt.Rows[0]["Bun"].ToString().Trim();
                    clsPmpaType.RS.Nu = Dt.Rows[0]["Nu"].ToString().Trim();
                    clsPmpaType.RS.SugbA = Dt.Rows[0]["SugbA"].ToString().Trim();
                    clsPmpaType.RS.SugbB = Dt.Rows[0]["SugbB"].ToString().Trim();
                    clsPmpaType.RS.SugbC = Dt.Rows[0]["SugbC"].ToString().Trim();
                    clsPmpaType.RS.SugbD = Dt.Rows[0]["SugbD"].ToString().Trim();
                    clsPmpaType.RS.SugbE = Dt.Rows[0]["SugbE"].ToString().Trim();
                    clsPmpaType.RS.SugbF = Dt.Rows[0]["SugbF"].ToString().Trim();
                    clsPmpaType.RS.SugbG = Dt.Rows[0]["SugbG"].ToString().Trim();
                    clsPmpaType.RS.SugbH = Dt.Rows[0]["SugbH"].ToString().Trim();
                    clsPmpaType.RS.SugbI = Dt.Rows[0]["SugbI"].ToString().Trim();
                    clsPmpaType.RS.SugbJ = Dt.Rows[0]["SugbJ"].ToString().Trim();
                    clsPmpaType.RS.SugbK = Dt.Rows[0]["SugbK"].ToString().Trim();
                    clsPmpaType.RS.SugbM = Dt.Rows[0]["SugbM"].ToString().Trim();
                    clsPmpaType.RS.SugbO = Dt.Rows[0]["SugbO"].ToString().Trim();
                    clsPmpaType.RS.SugbP = Dt.Rows[0]["SugbP"].ToString().Trim();
                    clsPmpaType.RS.SugbQ = Dt.Rows[0]["SugbQ"].ToString().Trim();
                    clsPmpaType.RS.SugbR = Dt.Rows[0]["SugbR"].ToString().Trim();
                    clsPmpaType.RS.SugbS = Dt.Rows[0]["SugbS"].ToString().Trim();
                    clsPmpaType.RS.SugbW = Dt.Rows[0]["SugbW"].ToString().Trim();
                    clsPmpaType.RS.SugbX = Dt.Rows[0]["SugbX"].ToString().Trim();
                    clsPmpaType.RS.SugbY = Dt.Rows[0]["SugbY"].ToString().Trim();
                    clsPmpaType.RS.SugbZ = Dt.Rows[0]["SugbZ"].ToString().Trim();
                    clsPmpaType.RS.SugbAA = Dt.Rows[0]["SugbAA"].ToString().Trim();
                    clsPmpaType.RS.SugbAB = Dt.Rows[0]["SugbAB"].ToString().Trim();
                    clsPmpaType.RS.SugbAC = Dt.Rows[0]["SugbAC"].ToString().Trim();
                    clsPmpaType.RS.SugbAD = Dt.Rows[0]["SugbAD"].ToString().Trim();
                    clsPmpaType.RS.SugbAG = Dt.Rows[0]["SugbAG"].ToString().Trim();
                    clsPmpaType.RS.GBNS = Dt.Rows[0]["GBNS"].ToString().Trim();
                    clsPmpaType.RS.IAmt = Convert.ToInt64(Dt.Rows[0]["Iamt"].ToString());
                    clsPmpaType.RS.TAmt = Convert.ToInt64(Dt.Rows[0]["Tamt"].ToString());
                    clsPmpaType.RS.BAmt = Convert.ToInt64(Dt.Rows[0]["Bamt"].ToString());
                    clsPmpaType.RS.SuDate = Dt.Rows[0]["SuDay"].ToString().Trim();
                    clsPmpaType.RS.OldIAmt = Convert.ToInt64(Dt.Rows[0]["OldIamt"].ToString());
                    clsPmpaType.RS.OldTAmt = Convert.ToInt64(Dt.Rows[0]["OldTamt"].ToString());
                    clsPmpaType.RS.OldBAmt = Convert.ToInt64(Dt.Rows[0]["OldBamt"].ToString());
                    clsPmpaType.RS.SuDate3 = Dt.Rows[0]["SuDay3"].ToString().Trim();
                    clsPmpaType.RS.IAmt3 = Convert.ToInt64(Dt.Rows[0]["Iamt3"].ToString());
                    clsPmpaType.RS.TAmt3 = Convert.ToInt64(Dt.Rows[0]["Tamt3"].ToString());
                    clsPmpaType.RS.BAmt3 = Convert.ToInt64(Dt.Rows[0]["Bamt3"].ToString());
                    clsPmpaType.RS.SuDate4 = Dt.Rows[0]["SuDay4"].ToString().Trim();
                    clsPmpaType.RS.IAmt4 = Convert.ToInt64(Dt.Rows[0]["Iamt4"].ToString());
                    clsPmpaType.RS.TAmt4 = Convert.ToInt64(Dt.Rows[0]["Tamt4"].ToString());
                    clsPmpaType.RS.BAmt4 = Convert.ToInt64(Dt.Rows[0]["Bamt4"].ToString());
                    clsPmpaType.RS.SuDate5 = Dt.Rows[0]["Suday5"].ToString().Trim();
                    clsPmpaType.RS.IAmt5 = Convert.ToInt64(Dt.Rows[0]["Iamt5"].ToString());
                    clsPmpaType.RS.TAmt5 = Convert.ToInt64(Dt.Rows[0]["Tamt5"].ToString());
                    clsPmpaType.RS.BAmt5 = Convert.ToInt64(Dt.Rows[0]["Bamt5"].ToString());
                    clsPmpaType.RS.DelDate = Dt.Rows[0]["DELDATE"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                return true;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Description : 미수 체크
        /// Author : 안정수
        /// Create Date : 2017.10.10
        /// <param name="ArgPano"></param>
        /// </summary>
        /// <seealso cref="Jengsan01.bas:READ_MISU_CHECK"/>
        /// <returns></returns>
        public bool READ_MISU_CHECK(PsmhDb pDbCon, string ArgPano)
        {
            bool rtnVal = false;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                ";
            SQL += ComNum.VBLF + "  SUM(DECODE(GUBUN1,'2', AMT *-1, AMT )) as SAMT      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "MISU_GAINSLIP              ";
            SQL += ComNum.VBLF + "WHERE 1=1                                             ";
            SQL += ComNum.VBLF + "      AND BDATE>=TO_DATE('2012-07-18','YYYY-MM-DD')   ";
            SQL += ComNum.VBLF + "      AND PANO = '" + ArgPano + "'                    ";
            SQL += ComNum.VBLF + "GROUP BY PANO                                         ";
            SQL += ComNum.VBLF + "HAVING SUM(DECODE(GUBUN1,'2', AMT *-1, AMT ))  <> 0   ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                if (VB.Val(dt.Rows[0]["SAMT"].ToString().Trim()) > 0)
                {
                    rtnVal = true;
                }
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 수납 정보 읽어오기         
        /// Author : 안정수
        /// Create Date : 2017.10.10
        /// <seealso cref="SengSanView_gesan.bas : Report_Print_Sunap_2012_Gesan"/>        
        /// </summary>
        /// <param name="ArgPno"></param>
        /// <param name="ArgGwa"></param>
        /// <param name="ArgNam"></param>
        /// <param name="ArgRetn"></param>
        /// <param name="ArgSeq"></param>
        /// <param name="ArgRdate"></param>
        /// <param name="ArgDr"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgBDate"></param>
        /// <param name="ArgCardBun"></param>
        /// <param name="ArgSunap"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgIPDJungmil"></param>
        /// <param name="ArgSpc"></param>
        /// <param name="ArgGelCode"></param>
        /// <param name="ArgMCode"></param>
        /// <param name="ArgVCode"></param>
        /// <param name="ArgJin"></param>
        /// <param name="ArgJinDtl"></param>
        /// <param name="GstrJobName"></param>
        public void Report_Print_Sunap_2012_Gesan(PsmhDb pDbCon, string ArgPno, string ArgGwa, string ArgNam, string ArgRetn, int ArgSeq,
                                            //병록번호 진료과목 수진자명 취소여부 영수증번호 
                                            string ArgRdate, string ArgDr, string ArgBi, string ArgBDate, string ArgCardBun,
                                            //예약일자 예약의사 환자구분 처방일자 카드구분
                                            string ArgSunap, string ArgDept, string ArgIPDJungmil, string ArgSpc, string ArgGelCode,
                                            string ArgMCode, string ArgVCode, string ArgJin, string ArgJinDtl,
                                            string GstrJobName = "")
        {
            ComFunc CF = new ComFunc();

            int i = 0;
            int j = 0;
            int k = 0;
            int h = 0;
            int g = 0;
            int nPo = 0;

            long[,] nPAmt = new long[28, 6];
            int nSelf = 0;
            int nX = 0;
            int nY = 0;

            string PR_PASSNAME = "";

            int AmtJin = 0;
            double nToAmt = 0;

            string[] strSite = new string[10];
            string strAMT_1 = "";   //금액 2
            string strAMT_2 = "";   //금액 3
            string strAMT1 = "";    //금액 1
            string strAMT2 = "";    //금액 2
            string strPno = "";
            string strGwa = "";
            string strBi = "";
            string strNam = "";
            string strSpc = "";
            string strIds = "";
            int nCNT = 0;
            long nAmt8 = 0;

            long nSpcAmt = 0;       //선택진료비
            string strSpcResv = "";

            string strFname = "";
            int nSeqNo = 0;
            string strMsgChk2 = "";
            string strTimeSS = "";  //수납시간초

            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            DataTable dt3 = null;

            string SQL = "";
            string SqlErr = "";

            int nCntEtc = 0;        //계정별 기타
            int nCntEtc2 = 0;       //정산별 기타

            int nSlipCount = 0;     //수납slip 수량(2021-07-22)

            nX = 0;
            nY = 300;
            nCntEtc = 0;
            nCntEtc2 = 0;

            PR_PASSNAME = GstrJobName;

            //외래 Slip 및 예약 금액 읽음

            #region READ_OPD_SLIP(GoSub)

            for (i = 0; i < 28; i++)
            {
                nPAmt[i, 1] = 0;
                nPAmt[i, 2] = 0;
                nPAmt[i, 3] = 0;
                nPAmt[i, 4] = 0;
                nPAmt[i, 5] = 0;
            }

            nSpcAmt = 0;

            strSpcResv = "";

            //2015-09-01
            clsPmpaPb.GstatEROVER = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                    ";
            SQL += ComNum.VBLF + "  BONRATE                                                 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                 ";
            SQL += ComNum.VBLF + "      AND PANO = '" + ArgPno + "'                         ";
            SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND DEPTCODE ='" + ArgDept + "'                     ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["BONRATE"].ToString().Trim() == "E")
                {
                    clsPmpaPb.GstatEROVER = "*";
                }
            }
            dt.Dispose();
            dt = null;


            //최대 수납 갯수 읽기(배열 범위 벗어나는 경우 관련)
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, PART, SEQNO, COUNT(*) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1=1                               ";
            SQL += ComNum.VBLF + "    AND PANO = '" + ArgPno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND ACTDATE = TO_DATE('" + ArgBDate + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  GROUP BY PANO, PART, SEQNO ";
            SQL += ComNum.VBLF + " HAVING COUNT(*) > 160 ";
            SQL += ComNum.VBLF + " ORDER BY CNT DESC ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            nSlipCount = 0;
            if (dt.Rows.Count > 0)
            {
                nSlipCount = Convert.ToInt32(dt.Rows[0]["CNT"].ToString()) + 10;
            }
            dt.Dispose();
            dt = null;



            //환자정보읽기
            #region Bas_Patient
            dt = Get_BasPatient(pDbCon, ArgPno);

            if (dt.Rows.Count > 0)
            {
                clsPmpaType.TBP.Ptno = dt.Rows[0]["Pano"].ToString().Trim();
                clsPmpaType.TBP.Sname = dt.Rows[0]["Sname"].ToString().Trim();
                clsPmpaType.TBP.Sex = dt.Rows[0]["Sex"].ToString().Trim();
                clsPmpaType.TBP.Jumin1 = dt.Rows[0]["Jumin1"].ToString().Trim();
                clsPmpaType.TBP.Jumin2 = dt.Rows[0]["Jumin2"].ToString().Trim();

                clsPmpaType.TBP.StartDate = dt.Rows[0]["StartDate"].ToString().Trim();
                clsPmpaType.TBP.LastDate = dt.Rows[0]["LastDate"].ToString().Trim();
                clsPmpaType.TBP.ZipCode1 = dt.Rows[0]["ZipCode1"].ToString().Trim();
                clsPmpaType.TBP.ZipCode2 = dt.Rows[0]["ZipCode2"].ToString().Trim();
                clsPmpaType.TBP.Juso = dt.Rows[0]["Juso"].ToString().Trim();

                clsPmpaType.TBP.Jiyuk = dt.Rows[0]["JiCode"].ToString().Trim();
                clsPmpaType.TBP.Tel = dt.Rows[0]["Tel"].ToString().Trim();
                clsPmpaType.TBP.HPhone = dt.Rows[0]["Hphone"].ToString().Trim();
                clsPmpaType.TBP.Sabun = dt.Rows[0]["Sabun"].ToString().Trim();
                clsPmpaType.TBP.EmbPrt = dt.Rows[0]["EmbPrt"].ToString().Trim();

                clsPmpaType.TBP.Bi = dt.Rows[0]["Bi"].ToString().Trim();
                clsPmpaType.TBP.PName = dt.Rows[0]["PName"].ToString().Trim();
                clsPmpaType.TBP.Gwange = dt.Rows[0]["Gwange"].ToString().Trim();
                clsPmpaType.TBP.Kiho = dt.Rows[0]["Kiho"].ToString().Trim();
                clsPmpaType.TBP.GKiho = dt.Rows[0]["GKiho"].ToString().Trim();

                clsPmpaType.TBP.DeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                clsPmpaType.TBP.DrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                clsPmpaType.TBP.GbSpc = dt.Rows[0]["GbSpc"].ToString().Trim();
                clsPmpaType.TBP.GbGameK = dt.Rows[0]["GbGameK"].ToString().Trim();
                clsPmpaType.TBP.JinIlsu = Convert.ToInt32(VB.Val(dt.Rows[0]["JinIlsu"].ToString().Trim()));

                clsPmpaType.TBP.JinAMT = Convert.ToInt64(VB.Val(dt.Rows[0]["JinAMT"].ToString().Trim()));
                clsPmpaType.TBP.TuyakGwa = dt.Rows[0]["TuyakGwa"].ToString().Trim();
                clsPmpaType.TBP.TuyakMonth = dt.Rows[0]["TuyakMonth"].ToString().Trim();
                clsPmpaType.TBP.TuyakJulDate = Convert.ToInt32(VB.Val(dt.Rows[0]["TuyakJulDate"].ToString().Trim()));
                clsPmpaType.TBP.TuyakIlsu = Convert.ToInt32(VB.Val(dt.Rows[0]["TuyakIlsu"].ToString().Trim()));

                clsPmpaType.TBP.Bohun = "0";
                clsPmpaType.TBP.Remark = dt.Rows[0]["Remark"].ToString().Trim();
                clsPmpaType.TBP.GbMsg = dt.Rows[0]["GbMsg"].ToString().Trim();
                clsPmpaType.TBP.Sabun = dt.Rows[0]["Sabun"].ToString().Trim();
                clsPmpaType.TBP.Bunup = dt.Rows[0]["Bunup"].ToString().Trim();

                clsPmpaType.TBP.Birth = dt.Rows[0]["Birth"].ToString().Trim();
                clsPmpaType.TBP.GbBirth = dt.Rows[0]["GbBirth"].ToString().Trim();
                clsPmpaType.TBP.EMail = dt.Rows[0]["EMail"].ToString().Trim();
                clsPmpaType.TBP.GbInfor = dt.Rows[0]["GbInfor"].ToString().Trim() + ComNum.VBLF + dt.Rows[0]["GB_BLACK"].ToString().Trim(); ;
                clsPmpaType.TBP.GbSMS = dt.Rows[0]["GbSMS"].ToString().Trim();

                clsPmpaType.TBP.GBJuso = dt.Rows[0]["GBJUSO"].ToString().Trim();
                clsPmpaType.TBP.Tel_Confirm = dt.Rows[0]["Tel_Confirm"].ToString().Trim();
                clsPmpaType.TBP.Gbinfo_Detail = dt.Rows[0]["Gbinfo_Detail"].ToString().Trim();

                //성별다시 확인함
                clsPmpaType.TBP.Sex = CF.SEX_SEARCH(clsPmpaType.TBP.Jumin2);
            }
            else
            {
                clsPmpaType.TBP.Ptno = "";
                clsPmpaType.TBP.Sname = "";
                clsPmpaType.TBP.Sex = "";
                clsPmpaType.TBP.Jumin1 = "";
                clsPmpaType.TBP.Jumin2 = "";

                clsPmpaType.TBP.StartDate = "";
                clsPmpaType.TBP.LastDate = "";
                clsPmpaType.TBP.ZipCode1 = "";
                clsPmpaType.TBP.ZipCode2 = "";
                clsPmpaType.TBP.Juso = "";

                clsPmpaType.TBP.Jiyuk = "";
                clsPmpaType.TBP.Tel = "";
                clsPmpaType.TBP.HPhone = "";
                clsPmpaType.TBP.Sabun = "";
                clsPmpaType.TBP.EmbPrt = "";

                clsPmpaType.TBP.Bi = "";
                clsPmpaType.TBP.PName = "";
                clsPmpaType.TBP.Gwange = "";
                clsPmpaType.TBP.Kiho = "";
                clsPmpaType.TBP.GKiho = "";

                clsPmpaType.TBP.DeptCode = "";
                clsPmpaType.TBP.DrCode = "";
                clsPmpaType.TBP.GbSpc = "";
                clsPmpaType.TBP.GbGameK = "";
                clsPmpaType.TBP.JinIlsu = 0;

                clsPmpaType.TBP.JinAMT = 0;
                clsPmpaType.TBP.TuyakGwa = "";
                clsPmpaType.TBP.TuyakMonth = "";
                clsPmpaType.TBP.TuyakJulDate = 0;
                clsPmpaType.TBP.TuyakIlsu = 0;

                clsPmpaType.TBP.Bohun = "0";
                clsPmpaType.TBP.Remark = "";
                clsPmpaType.TBP.GbMsg = "";
                clsPmpaType.TBP.Sabun = "";
                clsPmpaType.TBP.Bunup = "";

                clsPmpaType.TBP.Birth = "";
                clsPmpaType.TBP.GbBirth = "";
                clsPmpaType.TBP.EMail = "";
                clsPmpaType.TBP.GbInfor = "";
                clsPmpaType.TBP.GbSMS = "";

                clsPmpaType.TBP.GBJuso = "";
                clsPmpaType.TBP.Tel_Confirm = "";
                clsPmpaType.TBP.Gbinfo_Detail = "";
            }

            dt.Dispose();
            dt = null;
            #endregion
            clsOumsad CO = new clsOumsad();
            CO.READ_OPD_MASTER(pDbCon, ArgPno, ArgDept, "", ArgBDate);

            //변수 Clear
            Report_Print_2012_Clear(nSlipCount);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                    ";
            SQL += ComNum.VBLF + "  BUN, GBSELF, SUM(AMT1) NAMT,SUM(AMT2) NAMT2,SUM(OGAMT) OGAMT                                            ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP                                                                       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
            SQL += ComNum.VBLF + "      AND ActDate  = TO_DATE('" + clsPmpaPb.GstrJeaDate + "','YYYY-MM-DD')                                ";
            SQL += ComNum.VBLF + "      AND Pano     = '" + ArgPno + "'                                                                     ";
            SQL += ComNum.VBLF + "      AND Bi       = '" + ArgBi + "'                                                                      ";
            SQL += ComNum.VBLF + "      AND DeptCode = '" + ArgDept + "'                                                                    ";
            SQL += ComNum.VBLF + "      AND SeqNo    = " + ArgSeq + "                                                                       ";
            SQL += ComNum.VBLF + "      AND TRIM(SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드')   ";
            SQL += ComNum.VBLF + "GROUP BY BUN, GBSELF                                                                                      ";
            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, pDbCon);

            if (dt1.Rows.Count > 0)
            {
                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    if (VB.Val(dt1.Rows[i]["GBSELF"].ToString().Trim()) == 0)
                    {
                        nSelf = 1;
                    }

                    else
                    {
                        nSelf = 2;
                    }

                    switch (dt1.Rows[i]["BUN"].ToString().Trim())
                    {
                        case "01":
                        case "02":
                            nPAmt[1, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //진찰료
                            break;

                        case "11":
                        case "12":
                        case "13":
                        case "14":
                        case "15":
                            nPAmt[3, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //투약료및 조제료
                            break;

                        case "16":
                        case "17":
                        case "18":
                        case "19":
                        case "20":
                        case "21":
                            nPAmt[4, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //주사료
                            break;

                        case "22":
                        case "23":
                            nPAmt[5, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //마취료
                            break;

                        case "24":
                        case "25":
                            nPAmt[6, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //이학요법료(물리치료)
                            break;

                        case "26":
                        case "27":
                            nPAmt[7, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //정신요법료
                            break;

                        case "28":
                        case "29":
                        case "30":
                        case "31":
                        case "32":
                        case "33":
                        case "34":
                        case "35":
                        case "36":
                        case "38":
                        case "39":
                            nPAmt[8, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //처치및 수술
                            break;

                        case "37":
                            nPAmt[6, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //처치및
                            break;

                        case "41":
                        case "42":
                        case "43":
                        case "44":
                        case "45":
                        case "46":
                        case "47":
                        case "48":
                        case "49":
                        case "50":
                        case "51":
                        case "52":
                        case "53":
                        case "54":
                        case "55":
                        case "56":
                        case "57":
                        case "58":
                        case "59":
                        case "60":
                        case "61":
                        case "62":
                        case "63":
                        case "64":
                            nPAmt[9, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));   //검사료
                            break;

                        case "65":
                        case "66":
                        case "67":
                        case "68":
                        case "69":
                        case "70":
                            nPAmt[10, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //검사료
                            break;

                        case "72":
                            nPAmt[11, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //CT
                            break;
                        case "73":
                            nPAmt[12, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //MRI
                            break;
                        case "71":
                            nPAmt[13, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //초음파
                            break;
                        case "40":
                            nPAmt[14, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //보철료
                            break;
                        case "75":
                            nPAmt[15, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //증명료
                            break;
                        case "99":
                            nPAmt[25, 1] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //영수액
                            break;
                        case "98":
                            nPAmt[20, 1] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //조합부담액
                            break;
                        case "92":
                            nPAmt[23, 1] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //감액
                            break;
                        case "96":
                            nPAmt[24, 1] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));      //미수액
                            break;

                        default:
                            nPAmt[17, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //기타
                            break;
                    }

                    if (String.Compare(dt1.Rows[i]["BUN"].ToString().Trim(), "84") <= 0)
                    {
                        nPAmt[18, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT"].ToString().Trim()));  //합계
                    }

                    //선택진료
                    nSelf = 2;

                    if (String.Compare(dt1.Rows[i]["BUN"].ToString().Trim(), "84") <= 0)
                    {
                        nPAmt[18, nSelf] += Convert.ToInt64(VB.Val(dt1.Rows[i]["NAMT2"].ToString().Trim()));  //선택진료 합계
                    }

                    //산전승인금액 SUM 2009-01-09 윤조연 추가
                    if (dt1.Rows[i]["BUN"].ToString().Trim() == "99" && VB.Val(dt1.Rows[i]["OGAMT"].ToString().Trim()) != 0)
                    {
                        //GnOgAmt = GnOgAmt + AdoGetNumber(rsSub1, "OGAMT", i)
                    }

                    //선택진료비 합
                    if (dt1.Rows[i]["BUN"].ToString().Trim() != "99" && VB.Val(dt1.Rows[i]["NAMT2"].ToString().Trim()) != 0)
                    {
                        nSpcAmt += Convert.ToInt64(dt1.Rows[i]["NAMT2"].ToString().Trim());
                    }
                }

                //자보-특진일 경우
                if (ArgBi == "52" && nPAmt[18, 2] > 0)
                {
                    nPAmt[21, 1] = nPAmt[18, 1] + nPAmt[18, 2];                 //진료비 총액
                    nPAmt[19, 1] = nPAmt[18, 1] - nPAmt[20, 1] + nPAmt[18, 2];  //급여본인부담금
                    nPAmt[22, 1] = nPAmt[19, 1];                                //환자부담총액
                }
                else
                {
                    nPAmt[21, 1] = nPAmt[18, 1] + nPAmt[18, 2];                 //진료비 총액
                    nPAmt[19, 1] = nPAmt[18, 1] - nPAmt[20, 1];                 //급여본인부담금
                    nPAmt[22, 1] = nPAmt[19, 1] + nPAmt[18, 2];                 //환자부담총액
                }

                nPAmt[26, 1] = nPAmt[22, 1] - nPAmt[23, 1] - nPAmt[24, 1];      //수납금액

                //2009-01-02 윤조연 영수증인쇄시 인공신장 급여본인부담금이 - 금액이면 0 으로 표시
                if ((nPAmt[19, 1] < 0 && ArgDept == "HD") || (nPAmt[19, 1] < 0 && VB.Len(nPAmt[19, 1].ToString()) < 3))
                {
                    nPAmt[19, 1] = nPAmt[19, 1] * (-1);
                    if (nPAmt[19, 1] < 10)
                    {
                        nPAmt[19, 1] = 0;
                    }
                    else
                    {
                        nPAmt[19, 1] = nPAmt[19, 1] * (-1);
                    }
                }
            }

            dt1.Dispose();
            dt1 = null;

            //마지막 절사액 읽기
            OPD_SUNAP_Last_Info(pDbCon, ArgPno, clsPmpaPb.GstrJeaDate, clsPmpaType.TOM.BDate, ArgDept, clsPmpaPb.GstrJeaPart, ArgSeq, "", clsPmpaType.TOM.Bi, "", "");

            //II과이면서 자보는 일반수가를 적용함
            if (ArgDept == "II" && clsPmpaType.TOM.Bi == "52")
            {
                clsPmpaType.TOM.Bi = "51";
            }
           
            clsPmpaType.a.Date = ArgBDate;
            clsPmpaType.a.Dept = ArgDept;
            clsPmpaType.a.Sex = clsPmpaType.TOM.Sex;
            clsPmpaType.a.GbGameK = clsPmpaType.TOM.GbGameK;
            clsPmpaType.a.Retn = 0;
            clsPmpaType.a.Bi = Convert.ToInt32(VB.Val(clsPmpaType.TOM.Bi));
            clsPmpaType.a.Bi1 = Convert.ToInt32(VB.Val(VB.Mid(clsPmpaType.TOM.Bi, 1, 1)));

            if (clsPmpaType.a.Bi == 52 || clsPmpaType.a.Bi == 55)
            {
                clsPmpaType.a.Bi1 = 6;  //자보
            }

            clsPmpaType.a.Age = clsPmpaType.TOM.Age;
            clsPmpaType.a.AgeiLsu = 99;

            if (clsPmpaType.a.Age == 0)
            {
                clsPmpaType.a.AgeiLsu = CF.DATE_ILSU(pDbCon, clsPmpaType.a.Date, "20" + VB.Left(clsPmpaType.TBP.Jumin1, 2) + "-" + VB.Mid(clsPmpaType.TBP.Jumin1, 3, 2) + "-" + VB.Right(clsPmpaType.TBP.Jumin1, 2));
            }

            clsPmpaType.a.Gbilban2 = clsPmpaType.TOM.Gbilban2;  //외국 new
            clsPmpaType.a.Pano = clsPmpaType.TOM.Pano;
            clsPmpaType.a.DrCode = clsPmpaType.TOM.DrCode;
            clsPmpaType.a.GbSpc = clsPmpaType.TOM.GbSpc;

            if (clsPmpaPb.GOpd_Sunap_GelCode != "")
            {
                clsPmpaType.TOM.GelCode = clsPmpaPb.GOpd_Sunap_GelCode;
            }
            else if (clsPmpaPb.GOpd_Sunap_MCode != "")
            {
                clsPmpaType.TOM.MCode = clsPmpaPb.GOpd_Sunap_MCode;
            }
            else if (clsPmpaPb.GOpd_Sunap_VCode != "")
            {
                clsPmpaType.TOM.VCode = clsPmpaPb.GOpd_Sunap_VCode;
            }

            if (String.Compare(clsPmpaPb.GstrSysDate, "2012-01-10") >= 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                                                    ";
                SQL += ComNum.VBLF + "  a.DeptCode,a.SuCode,a.SuNext,a.Bi,a.Bun,a.Nu,a.Qty,a.Nal,a.BaseAmt,a.GbSugbS ,                     ";
                SQL += ComNum.VBLF + "  a.GbSpc,a.GbNgt,a.GbGisul,a.GbSelf,a.GbChild,a.DrCode,a.WardCode,                                       ";
                SQL += ComNum.VBLF + "  a.GbSlip,a.GbHost,a.Amt1,a.Amt2,a.Jamt,a.Bamt,a.OrderNo,a.GbImiv,a.GbBunup,                             ";
                SQL += ComNum.VBLF + "  TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,a.GbIpd, a.CardSeqNo,a.DosCode,                                     ";
                SQL += ComNum.VBLF + "  a.ABCDATE,a.OPER_DEPT,a.OPER_DCT,a.CARDSEQNO,a.OgAmt,a.DanAmt,a.GbSpc_No,                               ";
                SQL += ComNum.VBLF + "  a.MULTI,a.MULTIREMARK,a.DIV,a.DUR,a.KSJIN,a.SCODESAYU,a.SCODEREMARK                                     ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP a, " + ComNum.DB_PMPA + "BAS_SUN b                                    ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
                SQL += ComNum.VBLF + "      AND a.Sunext=b.Sunext(+)                                                                            ";
                SQL += ComNum.VBLF + "      AND a.Pano = '" + ArgPno + "'                                                                       ";
                SQL += ComNum.VBLF + "      AND a.DeptCode = '" + ArgDept + "'                                                                  ";
                SQL += ComNum.VBLF + "      AND a.ActDate  = TO_DATE('" + clsPmpaPb.GstrJeaDate + "','YYYY-MM-DD')                              ";
                SQL += ComNum.VBLF + "      AND a.Part = '" + clsPmpaPb.GstrJeaPart.Trim() + "'                                                 ";
                SQL += ComNum.VBLF + "      AND TRIM(a.SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";
                SQL += ComNum.VBLF + "      AND a.SeqNo = " + ArgSeq + "                                                                        ";
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                                                    ";
                SQL += ComNum.VBLF + "  DeptCode,SuCode,SuNext,Bi,Bun,Nu,Qty,Nal,BaseAmt,GbSugbS,                                               ";
                SQL += ComNum.VBLF + "  GbSpc,GbNgt,GbGisul,GbSelf,GbChild,DrCode,WardCode,                                                     ";
                SQL += ComNum.VBLF + "  GbSlip,GbHost,Amt1,Amt2,Jamt,Bamt,OrderNo,GbImiv,GbBunup,                                               ";
                SQL += ComNum.VBLF + "  TO_CHAR(BDate,'YYYY-MM-DD') BDate,GbIpd, CardSeqNo,DosCode,                                             ";
                SQL += ComNum.VBLF + "  ABCDATE,OPER_DEPT,OPER_DCT,CARDSEQNO,OgAmt,DanAmt,GbSpc_No,                                             ";
                SQL += ComNum.VBLF + "  MULTI,MULTIREMARK,DIV,DUR,KSJIN,SCODESAYU,SCODEREMARK                                                   ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP                                                                       ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                                 ";
                SQL += ComNum.VBLF + "      AND Pano = '" + ArgPno + "'                                                                         ";
                SQL += ComNum.VBLF + "      AND DeptCode = '" + ArgDept + "'                                                                    ";
                SQL += ComNum.VBLF + "      AND ActDate  = TO_DATE('" + clsPmpaPb.GstrJeaDate + "','YYYY-MM-DD')                                ";
                SQL += ComNum.VBLF + "      AND Part = '" + clsPmpaPb.GstrJeaPart.Trim() + "'                                                   ";
                SQL += ComNum.VBLF + "      AND TRIM(SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드')   ";
                SQL += ComNum.VBLF + "      AND SeqNo = " + ArgSeq + "                                                                          ";
            }

            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            j = 1;

            if (dt1.Rows.Count > 0)
            {
                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    clsPmpaType.RP.Sucode[j] = dt1.Rows[i]["Sucode"].ToString().Trim();
                    clsPmpaType.RP.Sunext[j] = dt1.Rows[i]["Sunext"].ToString().Trim();
                    clsPmpaType.RP.Bi[j] = dt1.Rows[i]["Bi"].ToString().Trim();
                    clsPmpaType.RP.BDate[j] = dt1.Rows[i]["BDate"].ToString().Trim();
                    clsPmpaType.RP.Bun[j] = dt1.Rows[i]["Bun"].ToString().Trim();
                    clsPmpaType.RP.Nu[j] = dt1.Rows[i]["Nu"].ToString().Trim();
                    clsPmpaType.RP.Qty[j] = Convert.ToInt64(VB.Val(dt1.Rows[i]["Qty"].ToString().Trim()));
                    clsPmpaType.RP.Nal[j] = Convert.ToInt32(dt1.Rows[i]["Nal"].ToString().Trim());
                    clsPmpaType.RP.BaseAmt[j] = Convert.ToInt64(VB.Val(dt1.Rows[i]["BaseAmt"].ToString().Trim()));
                    clsPmpaType.RP.GbSpc[j] = dt1.Rows[i]["GbSpc"].ToString().Trim();
                    clsPmpaType.RP.GbNgt[j] = dt1.Rows[i]["GbNgt"].ToString().Trim();
                    clsPmpaType.RP.GbGisul[j] = dt1.Rows[i]["GbGisul"].ToString().Trim();
                    clsPmpaType.RP.SugbS[j] = dt1.Rows[i]["GbSugbS"].ToString().Trim();

                    if (String.Compare(clsPmpaPb.GstrSysDate, "2012-01-10") >= 0)
                    {
                        clsPmpaType.RP.GbSelf[j] = dt1.Rows[i]["GbSelf"].ToString().Trim();

                        switch (clsPmpaType.RP.GbSelf[j])
                        {
                            //2021-06-28 SUGBS = '2' 추가
                            case "0": //이거 찾을려고 며칠이 걸렸다. QT
                                if (clsPmpaType.RP.SugbS[j] == "2" || clsPmpaType.RP.SugbS[j] == "4" || clsPmpaType.RP.SugbS[j] == "5"|| clsPmpaType.RP.SugbS[j] == "6"|| clsPmpaType.RP.SugbS[j] == "7"|| clsPmpaType.RP.SugbS[j] == "8"|| clsPmpaType.RP.SugbS[j] == "9")
                                {
                                    clsPmpaType.RP.GbSelf[j] = "2";
                                }
                                break;
                            case "1":
                                if (dt1.Rows[i]["GbSugbS"].ToString().Trim() == "1")
                                {
                                    clsPmpaType.RP.GbSelf[j] = "2";
                                }
                                break;

                            case "2":
                                if (dt1.Rows[i]["GbSugbS"].ToString().Trim() == "1")
                                {
                                    clsPmpaType.RP.GbSelf[j] = "2";
                                }
                                else
                                {
                                    clsPmpaType.RP.GbSelf[j] = "1";
                                }
                                break;
                        }
                    }

                    else
                    {
                        clsPmpaType.RP.GbSelf[j] = dt1.Rows[i]["GbSelf"].ToString().Trim();
                    }

                    clsPmpaType.RP.GbChild[j] = dt1.Rows[i]["GbChild"].ToString().Trim();
                    clsPmpaType.RP.DrCode[j] = dt1.Rows[i]["DrCode"].ToString().Trim();
                    clsPmpaType.RP.DeptCode[j] = dt1.Rows[i]["DeptCode"].ToString().Trim();
                    clsPmpaType.RP.WardCode[j] = dt1.Rows[i]["WardCode"].ToString().Trim();
                    clsPmpaType.RP.GbSlip[j] = dt1.Rows[i]["GbSlip"].ToString().Trim();
                    clsPmpaType.RP.GbHost[j] = dt1.Rows[i]["GbHost"].ToString().Trim();
                    clsPmpaType.RP.Amt1[j] = Convert.ToInt64(VB.Val(dt1.Rows[i]["Amt1"].ToString().Trim()));
                    clsPmpaType.RP.Amt2[j] = Convert.ToInt64(VB.Val(dt1.Rows[i]["Amt2"].ToString().Trim()));

                    clsPmpaType.RP.OrderNo[j] = Convert.ToInt64(VB.Val(dt1.Rows[i]["Orderno"].ToString().Trim()));
                    clsPmpaType.RP.GbImiv[j] = dt1.Rows[i]["GbImiv"].ToString().Trim();
                    clsPmpaType.RP.GbBunup[j] = dt1.Rows[i]["GbBunup"].ToString().Trim();
                    clsPmpaType.RP.DosCode[j] = dt1.Rows[i]["DosCode"].ToString().Trim();
                    clsPmpaType.RP.GBIPD[j] = dt1.Rows[i]["GBIPD"].ToString().Trim();
                    clsPmpaType.RP.KsJin[j] = dt1.Rows[i]["KSJIN"].ToString().Trim();
                    clsPmpaType.RP.DanAmt[j] = Convert.ToInt64(VB.Val(dt1.Rows[i]["DanAmt"].ToString().Trim()));
                    clsPmpaType.RP.GbSpc_No[j] = dt1.Rows[i]["GbSpc_No"].ToString().Trim();

                    j += 1;
                }

                //영수 금액 읽기
                for (i = 1; i <= 170; i++)
                {
                    if (clsPmpaType.RP.Sucode[i] != "")
                    {
                        Report_Slip_AmtAdd(i);
                    }
                }

                //영수 금액 계산   Last_Report_Amt_Gesan(ref RPG); 

                Last_Report_Amt_Gesan_2017();
            }

            dt1.Dispose();
            dt1 = null;





            #endregion READ_OPD_SLIP(GoSub) End

            //sub 합계

            clsPmpaType.RPG.Amt1[36] = clsPmpaType.RPG.Amt1[36];            //급여 본인합
            clsPmpaType.RPG.Amt1[37] = clsPmpaType.RPG.Amt1[37];            //급여 공단합
            clsPmpaType.RPG.Amt1[38] = clsPmpaType.RPG.Amt1[38];            //전액부담
            clsPmpaType.RPG.Amt1[39] = clsPmpaType.RPG.Amt1[39];            //선택진료합
            clsPmpaType.RPG.Amt1[40] = clsPmpaType.RPG.Amt1[40];            //비급여합

            nPAmt[21, 1] = nPAmt[21, 1];                                    //진료비총액 RPG.Amt1[31]

            clsPmpaType.RPG.Amt1[33] = clsPmpaType.RPG.Amt1[33] - nAmt8;    //환자부담총액
        }

        /// <summary>
        /// author : 안정수
        /// Create Date : 2017.10.10
        /// <seealso cref="SengSanView_gesan.bas : Report_Print_2012_Clear"/> 
        /// </summary>
        public void Report_Print_2012_Clear(int nArrayCount = 0)
        {
            int i = 0;

            clsPmpaPb.GnDrugRPAmt = 0;
            clsPmpaPb.GnToothRpAmt = 0;
            clsPmpaPb.GnJinRp회신료 = 0;
            clsPmpaPb.GnJinRp의뢰료 = 0;
            clsPmpaPb.GnJinRp재택결핵 = 0;      //2021-09-16 결핵관리료, 상담료
            clsPmpaPb.GnNPNnAmt = 0;

            clsPmpaType.RPA.Date = "";
            clsPmpaType.RPA.Dept = "";
            clsPmpaType.RPA.Sex = "";
            clsPmpaType.RPA.GbSpc = "";
            clsPmpaType.RPA.GbGameK = "";

            clsPmpaType.RPA.Retn = 0;
            clsPmpaType.RPA.Bi = 0;
            clsPmpaType.RPA.Bi1 = 0;
            clsPmpaType.RPA.Age = 0;
            clsPmpaType.RPA.AgeiLsu = 0;

            clsPmpaType.RPA.Gbilban2 = "";
            clsPmpaType.RPA.Pano = "";
            clsPmpaType.RPA.DrCode = "";

            Report_Print_RPG_Clear();

            if (nArrayCount == 0)
            {
                nArrayCount = 171;
            }
            
            #region 박웅규 추가 : 2018-09-20
            #region 주석
            //clsPmpaType.RP.Sucode = new string[171];
            //clsPmpaType.RP.Sunext = new string[171];
            //clsPmpaType.RP.Bi = new string[171];
            //clsPmpaType.RP.BDate = new string[171];
            //clsPmpaType.RP.Bun = new string[171];

            //clsPmpaType.RP.Nu = new string[171];
            //clsPmpaType.RP.Qty = new double[171];
            //clsPmpaType.RP.Nal = new int[171];
            //clsPmpaType.RP.BaseAmt = new long[171];
            //clsPmpaType.RP.GbSpc = new string[171];

            //clsPmpaType.RP.GbNgt = new string[171];
            //clsPmpaType.RP.GbGisul = new string[171];
            //clsPmpaType.RP.GbSelf = new string[171];
            //clsPmpaType.RP.GbChild = new string[171];
            //clsPmpaType.RP.DrCode = new string[171];

            //clsPmpaType.RP.DeptCode = new string[171];
            //clsPmpaType.RP.WardCode = new string[171];
            //clsPmpaType.RP.GbSlip = new string[171];
            //clsPmpaType.RP.GbHost = new string[171];
            //clsPmpaType.RP.Amt1 = new long[171];

            //clsPmpaType.RP.Amt2 = new long[171];
            //clsPmpaType.RP.OrderNo = new double[171];
            //clsPmpaType.RP.GbImiv = new string[171];
            //clsPmpaType.RP.DosCode = new string[171];
            //clsPmpaType.RP.GbBunup = new string[171];

            //clsPmpaType.RP.GBIPD = new string[171];
            //clsPmpaType.RP.Div = new int[171];
            //clsPmpaType.RP.KsJin = new string[171];
            //clsPmpaType.RP.DanAmt = new long[171];
            //clsPmpaType.RP.GbSpc_No = new string[171];
            //clsPmpaType.RP.SugbS = new string[171]; 
            #endregion
            //수납내역 171개 넘으면 에러 발생하여 보완(2021-07-22)
            clsPmpaType.RP.Sucode = new string[nArrayCount];
            clsPmpaType.RP.Sunext = new string[nArrayCount];
            clsPmpaType.RP.Bi = new string[nArrayCount];
            clsPmpaType.RP.BDate = new string[nArrayCount];
            clsPmpaType.RP.Bun = new string[nArrayCount];

            clsPmpaType.RP.Nu = new string[nArrayCount];
            clsPmpaType.RP.Qty = new double[nArrayCount];
            clsPmpaType.RP.Nal = new int[nArrayCount];
            clsPmpaType.RP.BaseAmt = new long[nArrayCount];
            clsPmpaType.RP.GbSpc = new string[nArrayCount];

            clsPmpaType.RP.GbNgt = new string[nArrayCount];
            clsPmpaType.RP.GbGisul = new string[nArrayCount];
            clsPmpaType.RP.GbSelf = new string[nArrayCount];
            clsPmpaType.RP.GbChild = new string[nArrayCount];
            clsPmpaType.RP.DrCode = new string[nArrayCount];

            clsPmpaType.RP.DeptCode = new string[nArrayCount];
            clsPmpaType.RP.WardCode = new string[nArrayCount];
            clsPmpaType.RP.GbSlip = new string[nArrayCount];
            clsPmpaType.RP.GbHost = new string[nArrayCount];
            clsPmpaType.RP.Amt1 = new long[nArrayCount];

            clsPmpaType.RP.Amt2 = new long[nArrayCount];
            clsPmpaType.RP.OrderNo = new double[nArrayCount];
            clsPmpaType.RP.GbImiv = new string[nArrayCount];
            clsPmpaType.RP.DosCode = new string[nArrayCount];
            clsPmpaType.RP.GbBunup = new string[nArrayCount];

            clsPmpaType.RP.GBIPD = new string[nArrayCount];
            clsPmpaType.RP.Div = new int[nArrayCount];
            clsPmpaType.RP.KsJin = new string[nArrayCount];
            clsPmpaType.RP.DanAmt = new long[nArrayCount];
            clsPmpaType.RP.GbSpc_No = new string[nArrayCount];
            clsPmpaType.RP.SugbS = new string[nArrayCount];
            #endregion 박웅규 추가 : 2018-09-20

            //for (i = 0; i <= 170; i++)
            for (i = 0; i <= nArrayCount - 1; i++)
            {
                clsPmpaType.RP.Sucode[i] = "";
                clsPmpaType.RP.Sunext[i] = "";
                clsPmpaType.RP.OrderNo[i] = 0;
                clsPmpaType.RP.GbSlip[i] = " ";
                clsPmpaType.RP.GbImiv[i] = "";
                clsPmpaType.RP.DosCode[i] = "";
                clsPmpaType.RP.GbBunup[i] = "";
                clsPmpaType.RP.GBIPD[i] = "";
                clsPmpaType.RP.KsJin[i] = "";
                clsPmpaType.RP.GbSpc_No[i] = "0";
                clsPmpaType.RP.Amt1[i] = 0;
                clsPmpaType.RP.Amt2[i] = 0;
                clsPmpaType.RP.SugbS[i] = "";
            }
        }

        public void Report_Print_RPG_Clear()
        {
            int i = 0;

            #region 박웅규 추가 : 2018-09-20
            clsPmpaType.RPG.Amt1 = new long[51];
            clsPmpaType.RPG.Amt2 = new long[51];
            clsPmpaType.RPG.Amt3 = new long[51];
            clsPmpaType.RPG.Amt4 = new long[51];
            clsPmpaType.RPG.Amt5 = new long[51];
            clsPmpaType.RPG.Amt6 = new long[51];
            clsPmpaType.RPG.Amt7 = new long[51];
            clsPmpaType.RPG.Amt8 = new long[51];
            clsPmpaType.RPG.Amt9 = new long[51];
            #endregion 박웅규 추가 : 2018-09-20

            for (i = 0; i <= 50; i++)
            {
                clsPmpaType.RPG.Amt1[i] = 0;
                clsPmpaType.RPG.Amt2[i] = 0;
                clsPmpaType.RPG.Amt3[i] = 0;
                clsPmpaType.RPG.Amt4[i] = 0;
                clsPmpaType.RPG.Amt5[i] = 0;
                clsPmpaType.RPG.Amt6[i] = 0;
                clsPmpaType.RPG.Amt7[i] = 0;
                clsPmpaType.RPG.Amt8[i] = 0;
                clsPmpaType.RPG.Amt9[i] = 0;
            }
        }

        /// <summary>
        /// author : 안정수
        /// Create Date : 2017.10.10
        /// <seealso cref="SengSanView_gesan.bas : OPD_SUNAP_Last_Info"/>
        /// <seealso cref="Report_Print.bas : OPD_SUNAP_Last_Info"/>
        /// </summary>
        /// <param name="ArgPano"></param>
        /// <param name="ArgActdate"></param>
        /// <param name="ArgBDate"></param>
        /// <param name="ArgDeptCode"></param>
        /// <param name="ArgPart"></param>
        /// <param name="ArgSeqno"></param>
        /// <param name="ArgJin"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgMCode"></param>
        /// <param name="ArgVCode"></param>
        public void OPD_SUNAP_Last_Info(PsmhDb pDbCon, string ArgPano, string ArgActdate, string ArgBDate, string ArgDeptCode, string ArgPart,
                                        int ArgSeqno, string ArgJin, string ArgBi, string ArgMCode, string ArgVCode)
        {
            clsPmpaPb.GnOpd_Sunap_LastDan = 0;      //영수증용 최종 100단위 절사
            clsPmpaPb.GOpd_Sunap_GelCode = "";      //영수증용 계약코드
            clsPmpaPb.GOpd_Sunap_MCode = "";        //영수증용 MCode
            clsPmpaPb.GOpd_Sunap_VCode = "";        //영수증용 VCode
            clsPmpaPb.GOpd_Sunap_Jin = "";          //영수증용 Jin
            clsPmpaPb.GOpd_Sunap_JinDtl = "";       //영수증용 JinDtl
            clsPmpaPb.GOpd_Sunap_JinDtl2 = "";      //영수증용 JinDtl2
            clsPmpaPb.GOpd_Sunap_Boamt = 0;         //영수증용 보호금액
            clsPmpaPb.GOpd_Sunap_EFamt = 0;         //영수증용 EF금액
            clsPmpaPb.GOpd_Sunap_II = "";
            clsPmpaPb.GnPtVoucherAmt = 0;            //물리치료 바우처

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                    ";
            SQL += ComNum.VBLF + "  deptcode, BDan,CDan,GelCode,MCode,VCode,Jin,JinDtl,EtcAmt,EtcAmt2,PtAmt,JinDtl2   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SUNAP                                      ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
            SQL += ComNum.VBLF + "      AND PANO ='" + ArgPano + "'                                         ";
            SQL += ComNum.VBLF + "      AND DEPTCODE IN ('" + ArgDeptCode + "', 'II')                       ";
            SQL += ComNum.VBLF + "      AND ActDATE =TO_DATE('" + ArgActdate + "','YYYY-MM-DD')             ";
            SQL += ComNum.VBLF + "      AND (DELDATE IS NULL OR DELDATE = '')                               ";
            SQL += ComNum.VBLF + "      AND Part ='" + ArgPart + "'                                         ";
            SQL += ComNum.VBLF + "      AND Seqno2 =" + ArgSeqno + "                                        ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                if (ArgMCode == "E000" || ArgMCode == "F000" || clsPmpaType.TOM.GbDementia == "Y" || (clsPmpaPb.GstrResExamCHK == "OK" && clsPmpaPb.GstrResExamFlag == "OK"))
                {
                    clsPmpaPb.GOpd_Sunap_GelCode = dt.Rows[0]["GelCode"].ToString().Trim();
                    clsPmpaPb.GOpd_Sunap_MCode = dt.Rows[0]["MCode"].ToString().Trim();
                    clsPmpaPb.GOpd_Sunap_VCode = dt.Rows[0]["VCode"].ToString().Trim();
                    clsPmpaPb.GOpd_Sunap_Jin = dt.Rows[0]["Jin"].ToString().Trim();
                    clsPmpaPb.GOpd_Sunap_JinDtl = dt.Rows[0]["JinDtl"].ToString().Trim();
                    clsPmpaPb.GOpd_Sunap_JinDtl2 = dt.Rows[0]["JinDtl2"].ToString().Trim();

                    if (dt.Rows[0]["DeptCode"].ToString().Trim() == "II")
                        clsPmpaPb.GOpd_Sunap_II = "II";

                    clsPmpaPb.GOpd_Sunap_Boamt = Convert.ToInt64(VB.Val(dt.Rows[0]["EtcAmt"].ToString().Trim()));
                    clsPmpaPb.GOpd_Sunap_EFamt = Convert.ToInt64(VB.Val(dt.Rows[0]["EtcAmt2"].ToString().Trim()));
                }

                else
                {
                    clsPmpaPb.GnOpd_Sunap_LastDan = Convert.ToInt64(VB.Val(dt.Rows[0]["BDan"].ToString().Trim()));
                    clsPmpaPb.GOpd_Sunap_GelCode = dt.Rows[0]["GelCode"].ToString().Trim();
                    clsPmpaPb.GOpd_Sunap_MCode = dt.Rows[0]["MCode"].ToString().Trim();
                    clsPmpaPb.GOpd_Sunap_VCode = dt.Rows[0]["VCode"].ToString().Trim();
                    clsPmpaPb.GOpd_Sunap_Jin = dt.Rows[0]["Jin"].ToString().Trim();
                    clsPmpaPb.GOpd_Sunap_JinDtl = dt.Rows[0]["JinDtl"].ToString().Trim();
                    clsPmpaPb.GOpd_Sunap_JinDtl2 = dt.Rows[0]["JinDtl2"].ToString().Trim();

                    if (dt.Rows[0]["DeptCode"].ToString().Trim() == "II")
                        clsPmpaPb.GOpd_Sunap_II = "II";

                    clsPmpaPb.GOpd_Sunap_Boamt = Convert.ToInt64(VB.Val(dt.Rows[0]["EtcAmt"].ToString().Trim()));
                    clsPmpaPb.GOpd_Sunap_EFamt = Convert.ToInt64(VB.Val(dt.Rows[0]["EtcAmt2"].ToString().Trim()));

                    clsPmpaPb.GnPtVoucherAmt = Convert.ToInt64(VB.Val(dt.Rows[0]["PtAmt"].ToString().Trim()));
                }
            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// author : 안정수
        /// Create Date : 2017.10.10
        /// </summary>
        /// <param name="ArgCnt"></param>
        public void Report_Slip_AmtAdd(int ArgCnt)
        {
            long n100BAmt = 0; //100/100본인부담금
            long n100JAmt = 0; //100/100조합부담금

            //진료비 총액
            if (clsPmpaType.RP.Bun[ArgCnt] == "99" || clsPmpaType.RP.Bun[ArgCnt] == "98" || clsPmpaType.RP.Bun[ArgCnt] == "96" || clsPmpaType.RP.Bun[ArgCnt] == "92")
            {
                clsPmpaType.RPG.Amt1[31] += clsPmpaType.RP.Amt1[ArgCnt];
                clsPmpaType.RPG.Amt1[31] += clsPmpaType.RP.Amt2[ArgCnt];
            }

            //공단부담총액
            if (clsPmpaType.RP.Bun[ArgCnt] == "98")
            {
                clsPmpaType.RPG.Amt1[32] += clsPmpaType.RP.Amt1[ArgCnt];
                clsPmpaType.RPG.Amt1[32] += clsPmpaType.RP.Amt2[ArgCnt];
            }

            //본인부담총액
            if (clsPmpaType.RP.Bun[ArgCnt] == "99")
            {
                clsPmpaType.RPG.Amt1[33] += clsPmpaType.RP.Amt1[ArgCnt];
                clsPmpaType.RPG.Amt1[33] += clsPmpaType.RP.Amt2[ArgCnt];
            }

            //감액
            if (clsPmpaType.RP.Bun[ArgCnt] == "92")
            {
                clsPmpaType.RPG.Amt1[34] += clsPmpaType.RP.Amt1[ArgCnt];
                clsPmpaType.RPG.Amt1[34] += clsPmpaType.RP.Amt2[ArgCnt];
            }

            //미수
            else if (clsPmpaType.RP.Bun[ArgCnt] == "96")
            {
                clsPmpaType.RPG.Amt1[35] += clsPmpaType.RP.Amt1[ArgCnt];
                clsPmpaType.RPG.Amt1[35] += clsPmpaType.RP.Amt2[ArgCnt];
            }

            else if (String.Compare(clsPmpaType.RP.Bun[ArgCnt], "85") < 0)
            {
                //건강보험, 의료급여만 정산
                if (string.Compare(clsPmpaType.RP.Bi[ArgCnt], "30") < 0)
                {
                    if (clsPmpaType.RP.SugbS[ArgCnt] == "4" || clsPmpaType.RP.SugbS[ArgCnt] == "6")
                    {
                        n100BAmt = (long)Math.Truncate(clsPmpaType.RP.Amt1[ArgCnt] * (80 / 100.00));
                        n100JAmt = clsPmpaType.RP.Amt1[ArgCnt] - n100BAmt;
                        clsPmpaType.RPG.Amt7[50] += clsPmpaType.RP.Amt1[ArgCnt]; //선별급여 총액
                        clsPmpaType.RPG.Amt8[50] += n100JAmt;        //선별급여 조합
                        clsPmpaType.RPG.Amt9[50] += n100BAmt;        //선별급여 본인
                    }

                    else if (clsPmpaType.RP.SugbS[ArgCnt] == "2")       //선별 계산로직 추가(2021-06-28)
                    {
                        n100BAmt = (long)Math.Truncate(clsPmpaType.RP.Amt1[ArgCnt] * (20 / 100.00));
                        n100JAmt = clsPmpaType.RP.Amt1[ArgCnt] - n100BAmt;
                        clsPmpaType.RPG.Amt7[50] += clsPmpaType.RP.Amt1[ArgCnt]; //선별급여 총액
                        clsPmpaType.RPG.Amt8[50] += n100JAmt;        //선별급여 조합
                        clsPmpaType.RPG.Amt9[50] += n100BAmt;        //선별급여 본인
                    }

                    else if (clsPmpaType.RP.SugbS[ArgCnt]  == "3")
                    {
                        n100BAmt = (long)Math.Truncate(clsPmpaType.RP.Amt1[ArgCnt] * (30 / 100.00));
                        n100JAmt = clsPmpaType.RP.Amt1[ArgCnt] - n100BAmt;
                        clsPmpaType.RPG.Amt7[50] += clsPmpaType.RP.Amt1[ArgCnt]; //선별급여 총액
                        clsPmpaType.RPG.Amt8[50] += n100JAmt;        //선별급여 조합
                        clsPmpaType.RPG.Amt9[50] += n100BAmt;        //선별급여 본인
                    }

                    else if (clsPmpaType.RP.SugbS[ArgCnt] == "8" || clsPmpaType.RP.SugbS[ArgCnt] == "9")
                    {
                        n100BAmt = (long)Math.Truncate(clsPmpaType.RP.Amt1[ArgCnt] * (90 / 100.00));
                        n100JAmt = clsPmpaType.RP.Amt1[ArgCnt] - n100BAmt;
                        clsPmpaType.RPG.Amt7[50] += clsPmpaType.RP.Amt1[ArgCnt]; //선별급여 총액
                        clsPmpaType.RPG.Amt8[50] += n100JAmt;        //선별급여 조합
                        clsPmpaType.RPG.Amt9[50] += n100BAmt;        //선별급여 본인
                    }

                    else if (clsPmpaType.RP.SugbS[ArgCnt] == "5" || clsPmpaType.RP.SugbS[ArgCnt] == "7")
                    {
                        n100BAmt = (long)Math.Truncate(clsPmpaType.RP.Amt1[ArgCnt] * (50 / 100.00));
                        n100JAmt = clsPmpaType.RP.Amt1[ArgCnt] - n100BAmt;
                        clsPmpaType.RPG.Amt7[50] += clsPmpaType.RP.Amt1[ArgCnt]; //선별급여 총액
                        clsPmpaType.RPG.Amt8[50] += n100JAmt;        //선별급여 조합
                        clsPmpaType.RPG.Amt9[50] += n100BAmt;        //선별급여 본인
                    }
                    else
                    {
                        n100BAmt = 0;
                        n100JAmt = 0;
                    }
                }

                switch (clsPmpaType.RP.Bun[ArgCnt])
                {
                    case "01":
                    case "02":
                    case "03":
                    case "04":
                        //진찰료 (보험환자 비급여 :인공신장,혈우병제외)
                        #region R_Amt_Add_01(GoSub)
                        if (string.Compare(clsPmpaType.RP.Bi[ArgCnt], "30") < 0 && (string.Compare(clsPmpaType.RP.SugbS[ArgCnt], "2") > 0))
                        {
                            clsPmpaType.RPG.Amt7[1] += n100JAmt + n100BAmt; //선별급여 총액
                            clsPmpaType.RPG.Amt8[1] += n100JAmt;            //선별급여 조합
                            clsPmpaType.RPG.Amt9[1] += n100BAmt;           //선별급여 본인

                            clsPmpaType.RPG.Amt3[1] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }
                        else
                        {
                            if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                            {
                                clsPmpaType.RPG.Amt2[1] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                                clsPmpaType.RPG.Amt3[1] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }

                            else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                            {
                                clsPmpaType.RPG.Amt3[1] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                clsPmpaType.RPG.Amt4[1] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                            }

                            else
                            {
                                if ((clsPmpaType.RP.Sucode[ArgCnt] == "IA213" || clsPmpaType.RP.Sucode[ArgCnt] == "IA313" || clsPmpaType.RP.Sucode[ArgCnt] == "IA231") && string.Compare(clsPmpaType.TOM.BDate, "2020-11-01") < 0)
                                {
                                    clsPmpaPb.GnJinRp회신료 += clsPmpaType.RP.Amt1[ArgCnt];
                                    clsPmpaType.RPG.Amt1[1] += clsPmpaType.RP.Amt1[ArgCnt] - clsPmpaPb.GnJinRp회신료; //보험합
                                    clsPmpaType.RPG.Amt3[1] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합

                                }
                                else if ((clsPmpaType.RP.Sucode[ArgCnt] == "IA313" || clsPmpaType.RP.Sucode[ArgCnt] == "IA231" || clsPmpaType.RP.Sucode[ArgCnt] == "IA110" || clsPmpaType.RP.Sucode[ArgCnt] == "IA120") && string.Compare(clsPmpaType.TOM.BDate, "2020-11-01") >= 0)
                                {
                                    clsPmpaPb.GnJinRp회신료 += clsPmpaType.RP.Amt1[ArgCnt];
                                    clsPmpaType.RPG.Amt1[1] += clsPmpaType.RP.Amt1[ArgCnt] - clsPmpaPb.GnJinRp회신료; //보험합
                                    clsPmpaType.RPG.Amt3[1] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                }
                                // @F022 건강검진 결핵 유소견자 면제
                                else if ((clsPmpaType.RP.Sucode[ArgCnt] == "AA176" || clsPmpaType.RP.Sucode[ArgCnt] == "AA276" || clsPmpaType.RP.Sucode[ArgCnt] == "AU312" || clsPmpaType.RP.Sucode[ArgCnt] == "AU214" || clsPmpaType.RP.Sucode[ArgCnt] == "AU413") && clsPmpaType.TOM.JinDtl == "29")
                                {
                                    clsPmpaPb.GnJinRp회신료 += clsPmpaType.RP.Amt1[ArgCnt];
                                    clsPmpaType.RPG.Amt1[1] += clsPmpaType.RP.Amt1[ArgCnt] - clsPmpaPb.GnJinRp회신료; //보험합
                                    clsPmpaType.RPG.Amt3[1] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                }
                                else if (clsPmpaType.RP.Sucode[ArgCnt] == "IA213" && string.Compare(clsPmpaType.TOM.BDate, "2020-11-01") >= 0)
                                {
                                    if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaType.TOM.MCode.Trim() == "" && clsPmpaType.TOM.VCode.Trim() == "")
                                    {
                                        clsPmpaPb.GnJinRp의뢰료 += clsPmpaType.RP.Amt1[ArgCnt];
                                        clsPmpaType.RPG.Amt1[1] += clsPmpaType.RP.Amt1[ArgCnt] - clsPmpaPb.GnJinRp의뢰료; //보험합
                                        clsPmpaType.RPG.Amt3[1] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                    }

                                }
                                //주석해제!
                                ////2021-09-16 결핵상담료,관리료 본인부담 없음.
                                else if (clsPmpaType.RP.Sucode[ArgCnt] == "ID110" || clsPmpaType.RP.Sucode[ArgCnt] == "ID120" || clsPmpaType.RP.Sucode[ArgCnt] == "ID130")  //2021-09-16
                                {
                                    clsPmpaPb.GnJinRp재택결핵 += clsPmpaType.RP.Amt1[ArgCnt];
                                    clsPmpaType.RPG.Amt1[1] += clsPmpaType.RP.Amt1[ArgCnt] - clsPmpaPb.GnJinRp재택결핵; //보험합
                                    clsPmpaType.RPG.Amt3[1] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                }

                                else
                                {
                                    clsPmpaType.RPG.Amt1[1] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                                    clsPmpaType.RPG.Amt3[1] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                }

                            }
                        }
                        

                        #endregion R_Amt_Add_01(GoSub) End
                        break;

                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "15":
                        //약 ADD
                        #region R_Amt_Add_04(GoSub)


                        if (clsPmpaType.RP.GbGisul[ArgCnt] == "1")
                        {
                            if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                            {
                                clsPmpaType.RPG.Amt2[4] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                                clsPmpaType.RPG.Amt3[4] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }

                            else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                            {
                                clsPmpaType.RPG.Amt3[4] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                clsPmpaType.RPG.Amt4[4] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                            }

                            else
                            {
                                clsPmpaType.RPG.Amt1[4] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                                clsPmpaType.RPG.Amt3[4] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합

                                if (String.Compare(clsPmpaType.TOM.BDate, "2011-07-23") >= 0)
                                {
                                    //심사과 김준수주임 요청 F003 외용약 제외
                                    if (clsPmpaType.RP.Bun[ArgCnt] == "11")
                                    {
                                        clsPmpaPb.GnDrugRPAmt += clsPmpaType.RP.Amt1[ArgCnt];
                                    }

                                    else
                                    {
                                        if (clsPmpaType.RP.Bun[ArgCnt] == "11" || clsPmpaType.RP.Bun[ArgCnt] == "12")
                                        {
                                            clsPmpaPb.GnDrugRPAmt += clsPmpaType.RP.Amt1[ArgCnt];
                                        }
                                    }
                                }
                            }
                        }

                        else
                        {
                            if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                            {
                                clsPmpaType.RPG.Amt2[5] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                                clsPmpaType.RPG.Amt3[5] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }

                            else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                            {
                                clsPmpaType.RPG.Amt3[5] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                clsPmpaType.RPG.Amt4[5] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                            }

                            else
                            {
                                clsPmpaType.RPG.Amt1[5] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                                clsPmpaType.RPG.Amt3[5] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합

                                if (String.Compare(clsPmpaType.TOM.BDate, "2011-07-23") >= 0)
                                {
                                    //심사과 김준수주임 요청 F003 외용약 제외
                                    if (clsPmpaType.RP.Bun[ArgCnt] == "11")
                                    {
                                        clsPmpaPb.GnDrugRPAmt += clsPmpaType.RP.Amt1[ArgCnt];
                                    }

                                    else
                                    {
                                        if (clsPmpaType.RP.Bun[ArgCnt] == "11" || clsPmpaType.RP.Bun[ArgCnt] == "12")
                                        {
                                            clsPmpaPb.GnDrugRPAmt += clsPmpaType.RP.Amt1[ArgCnt];
                                        }
                                    }
                                }
                            }
                        }

                        #endregion R_Amt_Add_04(GoSub) End
                        break;

                    case "16":
                    case "17":
                    case "18":
                    case "19":
                    case "20":
                    case "21":
                        //주사
                        #region R_Amt_Add_06(GoSub)

                        if (clsPmpaType.RP.GbGisul[ArgCnt] == "1")
                        {
                            if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                            {
                                clsPmpaType.RPG.Amt2[6] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                                clsPmpaType.RPG.Amt3[6] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }

                            else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                            {
                                clsPmpaType.RPG.Amt3[6] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                clsPmpaType.RPG.Amt4[6] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                            }

                            else
                            {
                                clsPmpaType.RPG.Amt1[6] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                                clsPmpaType.RPG.Amt3[6] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }
                        }
                        else
                        {
                            if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                            {
                                clsPmpaType.RPG.Amt2[7] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                                clsPmpaType.RPG.Amt3[7] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }

                            else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                            {
                                clsPmpaType.RPG.Amt3[7] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                clsPmpaType.RPG.Amt4[7] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                            }

                            else
                            {
                                clsPmpaType.RPG.Amt1[7] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                                clsPmpaType.RPG.Amt3[7] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }
                        }

                        #endregion R_Amt_Add_06(GoSub) End
                        break;

                    case "22":
                    case "23":
                        //마취
                        #region R_Amt_Add_08(GoSub)

                        if (string.Compare(clsPmpaType.RP.Bi[ArgCnt], "30") < 0 && (string.Compare(clsPmpaType.RP.SugbS[ArgCnt], "2") > 0))
                        {
                            clsPmpaType.RPG.Amt7[8] += n100JAmt + n100BAmt; //선별급여 총액
                            clsPmpaType.RPG.Amt8[8] += n100JAmt;            //선별급여 조합
                            clsPmpaType.RPG.Amt9[8] += n100BAmt;           //선별급여 본인

                            clsPmpaType.RPG.Amt3[8] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }
                        else
                        {
                            if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                            {
                                clsPmpaType.RPG.Amt2[8] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                                clsPmpaType.RPG.Amt3[8] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }

                            else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                            {
                                clsPmpaType.RPG.Amt3[8] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                clsPmpaType.RPG.Amt4[8] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                            }

                            else
                            {
                                clsPmpaType.RPG.Amt1[8] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                                clsPmpaType.RPG.Amt3[8] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }
                        }
                       

                        #endregion R_Amt_Add_08(GoSub) End
                        break;

                    case "28":
                    case "29":
                    case "30":
                    case "31":
                    case "32":
                    case "33":
                    case "34":
                    case "35":
                    case "36":
                    case "38":
                    case "39":
                        //처치 수술
                        #region R_Amt_ADD_09(GoSub)

                        if (string.Compare(clsPmpaType.RP.Bi[ArgCnt], "30") < 0 && (string.Compare(clsPmpaType.RP.SugbS[ArgCnt], "2") > 0))
                        {
                            clsPmpaType.RPG.Amt7[9] += n100JAmt + n100BAmt; //선별급여 총액
                            clsPmpaType.RPG.Amt8[9] += n100JAmt;            //선별급여 조합
                            clsPmpaType.RPG.Amt9[9] += n100BAmt;           //선별급여 본인

                            clsPmpaType.RPG.Amt3[9] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }
                        else
                        {
                            if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                            {
                                clsPmpaType.RPG.Amt2[9] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                                clsPmpaType.RPG.Amt3[9] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }

                            else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                            {
                                clsPmpaType.RPG.Amt3[9] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                clsPmpaType.RPG.Amt4[9] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                            }

                            else
                            {
                                clsPmpaType.RPG.Amt1[9] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                                clsPmpaType.RPG.Amt3[9] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }

                        }

                        #endregion R_Amt_ADD_09(GoSub) End
                        break;

                    case "41":
                    case "42":
                    case "43":
                    case "44":
                    case "45":
                    case "46":
                    case "47":
                    case "48":
                    case "49":
                    case "50":
                    case "51":
                    case "52":
                    case "53":
                    case "54":
                    case "55":
                    case "56":
                    case "57":
                    case "58":
                    case "59":
                    case "60":
                    case "61":
                    case "62":
                    case "63":
                    case "64":
                        //검사
                        #region R_Amt_ADD_10(GoSub)
                        if (string.Compare(clsPmpaType.RP.Bi[ArgCnt], "30") < 0 && (string.Compare(clsPmpaType.RP.SugbS[ArgCnt].Trim(), "1") > 0))
                        {
                            clsPmpaType.RPG.Amt7[10] += n100JAmt + n100BAmt; //선별급여 총액
                            clsPmpaType.RPG.Amt8[10] += n100JAmt;            //선별급여 조합
                            clsPmpaType.RPG.Amt9[10] += n100BAmt;           //선별급여 본인

                            clsPmpaType.RPG.Amt3[10] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }
                        else

                        {
                                if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                                {
                                    clsPmpaType.RPG.Amt2[10] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                                    clsPmpaType.RPG.Amt3[10] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                }

                                else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                                {
                                    clsPmpaType.RPG.Amt3[10] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                    clsPmpaType.RPG.Amt4[10] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                                }

                                else
                                {
                                    clsPmpaType.RPG.Amt1[10] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                                    clsPmpaType.RPG.Amt3[10] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                }
                            }
                       

                        #endregion R_Amt_ADD_10(GoSub) End
                        break;

                    case "65":
                    case "66":
                    case "67":
                    case "68":
                    case "69":
                    case "70":
                        //XRAY ADD
                        #region R_Amt_ADD_11(GoSub)

                        if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                        {
                            clsPmpaType.RPG.Amt2[11] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                            clsPmpaType.RPG.Amt3[5] = clsPmpaType.RPG.Amt3[11] + clsPmpaType.RP.Amt2[ArgCnt];  //특진합
                        }

                        else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                        {
                            clsPmpaType.RPG.Amt3[11] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            clsPmpaType.RPG.Amt4[11] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                        }

                        else
                        {
                            clsPmpaType.RPG.Amt1[11] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                            clsPmpaType.RPG.Amt3[11] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }

                        #endregion R_Amt_ADD_11(GoSub) End
                        break;

                    case "24":
                    case "25":
                        //물리치료
                        #region R_Amt_ADD_14(GoSub)

                        if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                        {
                            clsPmpaType.RPG.Amt2[14] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                            clsPmpaType.RPG.Amt3[14] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }

                        else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                        {
                            clsPmpaType.RPG.Amt3[14] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            clsPmpaType.RPG.Amt4[14] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                        }

                        else
                        {
                            clsPmpaType.RPG.Amt1[14] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                            clsPmpaType.RPG.Amt3[14] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }

                        #endregion R_Amt_ADD_14(GoSub) End
                        break;

                    case "26":
                    case "27":
                        //정신요법
                        #region R_Amt_ADD_15(GoSub)

                        if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                        {
                            clsPmpaType.RPG.Amt2[15] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                            clsPmpaType.RPG.Amt3[15] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }

                        else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                        {
                            clsPmpaType.RPG.Amt3[15] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            clsPmpaType.RPG.Amt4[15] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                        }

                        else
                        {
                            clsPmpaType.RPG.Amt1[15] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                            clsPmpaType.RPG.Amt3[15] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }

                        #endregion R_Amt_ADD_15(GoSub) End
                        break;

                    case "37":
                        //수혈
                        #region R_Amt_ADD_16(GoSub)

                        if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                        {
                            clsPmpaType.RPG.Amt2[16] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                            clsPmpaType.RPG.Amt3[16] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }

                        else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                        {
                            clsPmpaType.RPG.Amt3[16] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            clsPmpaType.RPG.Amt4[16] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                        }

                        else
                        {
                            clsPmpaType.RPG.Amt1[16] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                            clsPmpaType.RPG.Amt3[16] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }

                        #endregion R_Amt_ADD_16(GoSub) End
                        break;

                    case "72":
                        //CT
                        #region R_Amt_ADD_17(GoSub)
                        if (string.Compare(clsPmpaType.RP.Bi[ArgCnt], "30") < 0 && (string.Compare(clsPmpaType.RP.SugbS[ArgCnt], "2") > 0))
                        {
                            clsPmpaType.RPG.Amt7[17] += n100JAmt + n100BAmt; //선별급여 총액
                            clsPmpaType.RPG.Amt8[17] += n100JAmt;            //선별급여 조합
                            clsPmpaType.RPG.Amt9[17] += n100BAmt;           //선별급여 본인

                            clsPmpaType.RPG.Amt3[17] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }
                        else
                        {
                            if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                            {
                                clsPmpaType.RPG.Amt2[17] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                                clsPmpaType.RPG.Amt3[17] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }

                            else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                            {
                                clsPmpaType.RPG.Amt3[17] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                clsPmpaType.RPG.Amt4[17] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                            }

                            else
                            {
                                clsPmpaType.RPG.Amt1[17] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                                clsPmpaType.RPG.Amt3[17] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }
                        }

                      

                        #endregion R_Amt_ADD_17(GoSub) End
                        break;

                    case "73":
                        //MRI
                        #region R_Amt_ADD_18(GoSub)

                        if (string.Compare(clsPmpaType.RP.Bi[ArgCnt], "30") < 0 && (string.Compare(clsPmpaType.RP.SugbS[ArgCnt], "2") > 0))
                        {
                            clsPmpaType.RPG.Amt7[18] += n100JAmt + n100BAmt; //선별급여 총액
                            clsPmpaType.RPG.Amt8[18] += n100JAmt;            //선별급여 조합
                            clsPmpaType.RPG.Amt9[18] += n100BAmt;           //선별급여 본인

                            clsPmpaType.RPG.Amt3[18] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }
                        else
                        {
                            if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                            {
                                clsPmpaType.RPG.Amt2[18] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                                clsPmpaType.RPG.Amt3[18] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }

                            else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                            {
                                clsPmpaType.RPG.Amt3[18] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                clsPmpaType.RPG.Amt4[18] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                            }

                            else
                            {
                                clsPmpaType.RPG.Amt1[18] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                                clsPmpaType.RPG.Amt3[18] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            }
                        }
                     

                        #endregion R_Amt_ADD_18(GoSub) End
                        break;

                    case "71":
                        //초음파
                        #region R_Amt_ADD_19(GoSub)

                        if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                        {
                            clsPmpaType.RPG.Amt2[19] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                            clsPmpaType.RPG.Amt3[19] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }

                        else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                        {
                            clsPmpaType.RPG.Amt3[19] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            clsPmpaType.RPG.Amt4[19] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                        }

                        else
                        {
                            clsPmpaType.RPG.Amt1[19] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                            clsPmpaType.RPG.Amt3[19] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }

                        #endregion R_Amt_ADD_19(GoSub) End
                        break;

                    case "40":
                        //보철
                        #region R_Amt_ADD_20(GoSub)

                        if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                        {
                            clsPmpaType.RPG.Amt2[20] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                            clsPmpaType.RPG.Amt3[20] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }

                        else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                        {
                            clsPmpaType.RPG.Amt3[20] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            clsPmpaType.RPG.Amt4[20] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                        }

                        else
                        {
                            clsPmpaType.RPG.Amt1[20] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                            clsPmpaType.RPG.Amt3[20] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합

                            if (String.Compare(clsPmpaType.TOM.BDate, "2012-07-01") >= 0)
                            {
                                clsPmpaPb.GnToothRpAmt += clsPmpaType.RP.Amt1[ArgCnt];  //노인틀니 2012-07-02
                            }
                        }

                        #endregion R_Amt_ADD_20(GoSub) End
                        break;

                    case "75":
                        //증명료
                        #region R_Amt_ADD_22(GoSub)

                        if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                        {
                            clsPmpaType.RPG.Amt2[22] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                            clsPmpaType.RPG.Amt3[21] = clsPmpaType.RPG.Amt3[22] + clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }

                        else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                        {
                            clsPmpaType.RPG.Amt3[22] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                            clsPmpaType.RPG.Amt4[22] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                        }

                        else
                        {
                            clsPmpaType.RPG.Amt1[22] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                            clsPmpaType.RPG.Amt3[22] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }

                        #endregion R_Amt_ADD_22(GoSub) End
                        break;

                    default:
                        //기타 ADD
                        #region R_Amt_ADD_30(GoSub)

                        if (string.Compare(clsPmpaType.RP.Bi[ArgCnt], "30") < 0 && (string.Compare(clsPmpaType.RP.SugbS[ArgCnt], "2") > 0))
                        {
                            clsPmpaType.RPG.Amt7[30] += n100JAmt + n100BAmt; //선별급여 총액
                            clsPmpaType.RPG.Amt8[30] += n100JAmt;            //선별급여 조합
                            clsPmpaType.RPG.Amt9[30] += n100BAmt;           //선별급여 본인

                            clsPmpaType.RPG.Amt3[30] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                        }
                        else
                        {
                            //수가기초코드에 R항에 0:자보급여 1:자보비급여 2005-10-13 윤
                            if (clsPmpaType.TOM.Bi == "52")
                            {
                                if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                                {
                                    clsPmpaType.RPG.Amt2[30] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                                    clsPmpaType.RPG.Amt3[30] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                }

                                else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                                {
                                    clsPmpaType.RPG.Amt3[30] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                    clsPmpaType.RPG.Amt4[8] = clsPmpaType.RPG.Amt4[30] + clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                                }

                                else
                                {
                                    clsPmpaType.RPG.Amt1[6] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                                    clsPmpaType.RPG.Amt3[6] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                }
                            }
                            else
                            {
                                if (clsPmpaType.RP.GbSelf[ArgCnt] == "1")
                                {
                                    clsPmpaType.RPG.Amt2[30] += clsPmpaType.RP.Amt1[ArgCnt]; //비급여
                                    clsPmpaType.RPG.Amt3[30] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                }

                                else if (clsPmpaType.RP.GbSelf[ArgCnt] == "2")
                                {
                                    clsPmpaType.RPG.Amt3[30] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                    clsPmpaType.RPG.Amt4[30] += clsPmpaType.RP.Amt1[ArgCnt]; //본인총액
                                }

                                else
                                {
                                    clsPmpaType.RPG.Amt1[30] += clsPmpaType.RP.Amt1[ArgCnt]; //보험합
                                    clsPmpaType.RPG.Amt3[30] += clsPmpaType.RP.Amt2[ArgCnt]; //특진합
                                }
                            }
                        }
                     

                        #endregion R_Amt_ADD_30(GoSub) End
                        break;
                }
            }
        }

        /// <summary>
        /// author : 안정수
        /// Create Date : 2017.10.10
        /// </summary>
        public void Last_Report_Amt_Gesan_2016()
        {
            clsBasAcct CBA = new clsBasAcct();

            int i = 0;
            long Bamt = 0;
            long Jamt = 0;
            int nTemp = 0;
            string strChk = "";

            for (i = 1; i <= 30; i++)
            {
                Bamt = 0;
                Jamt = 0;
                nTemp = 0;
                if (i == 30)
                {
                    MessageBox.Show("");
                }
                //예약진찰제외
                if (i != 23)
                {
                    #region Gensan_Johap_Chk(GoSub)

                    if (clsPmpaType.TOM.DeptCode == "DT" && String.Compare(clsPmpaType.TOM.BDate, "2017-11-01") >= 0 && clsPmpaPb.GOpd_Sunap_JinDtl == "02" && String.Compare(clsPmpaType.TOM.Bi, "22") <= 0)
                    {
                        if (String.Compare(clsPmpaType.TOM.Bi, "13") <= 0)
                        {
                            if (i == 20)
                            {
                                if (clsPmpaPb.GOpd_Sunap_MCode == "C000")
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 5) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 5 / 100);    //노인틀니
                                }
                                else if (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000")
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 15) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 15 / 100);    //노인틀니
                                }
                                else
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 30) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 30 / 100);    //노인틀니
                                }
                            }
                            else
                            {
                                Bamt = (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                            }
                        }
                        else if (clsPmpaType.TOM.Bi == "21")
                        {
                            if (i == 20)
                            {
                                Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Bamt += (clsPmpaPb.GnToothRpAmt * 5) / 100;    //노인틀니
                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt -= (clsPmpaPb.GnToothRpAmt * 5 / 100);    //노인틀니
                            }
                            else
                            {
                                Bamt = (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                            }
                        }
                        else if (String.Compare(clsPmpaType.TOM.Bi, "22") <= 0)
                        {
                            if (i == 20)
                            {
                                Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Bamt += (clsPmpaPb.GnToothRpAmt * 15) / 100;    //노인틀니
                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt -= (clsPmpaPb.GnToothRpAmt * 15 / 100);    //노인틀니
                            }
                            else
                            {
                                Bamt = (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                            }
                        }
                    }
                    else if (clsPmpaType.TOM.DeptCode == "DT" && String.Compare(clsPmpaType.TOM.BDate, "2018-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_JinDtl == "07" && String.Compare(clsPmpaType.TOM.Bi, "22") <= 0)
                    {
                        if (String.Compare(clsPmpaType.TOM.Bi, "13") <= 0)
                        {
                            if (i == 20)
                            {
                                if (clsPmpaPb.GOpd_Sunap_MCode == "C000")
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 10) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 10 / 100);    //노인틀니
                                }
                                else if (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000")
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 20) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 20 / 100);    //노인틀니
                                }
                                else
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 30) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 30 / 100);    //노인틀니
                                }
                            }
                            else
                            {
                                Bamt = (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                            }
                        }
                        else if (clsPmpaType.TOM.Bi == "21")
                        {
                            if (i == 20)
                            {
                                Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Bamt += (clsPmpaPb.GnToothRpAmt * 10) / 100;    //노인틀니
                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt -= (clsPmpaPb.GnToothRpAmt * 10 / 100);    //노인틀니
                            }
                            else
                            {
                                Bamt = (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                            }
                        }
                        else if (String.Compare(clsPmpaType.TOM.Bi, "22") <= 0)
                        {
                            if (i == 20)
                            {
                                Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Bamt += (clsPmpaPb.GnToothRpAmt * 20) / 100;    //노인틀니
                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt -= (clsPmpaPb.GnToothRpAmt * 20 / 100);    //노인틀니
                            }
                            else
                            {
                                Bamt = (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                            }
                        }
                    }
                    //2012-07-01 노인틀니
                    else if (clsPmpaType.TOM.DeptCode == "DT" && String.Compare(clsPmpaType.TOM.BDate, "2012-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_JinDtl == "02" && String.Compare(clsPmpaType.TOM.Bi, "22") <= 0)
                    {
                        if (String.Compare(clsPmpaType.TOM.Bi, "13") <= 0)
                        {
                            if (i == 20)
                            {
                                if (clsPmpaPb.GOpd_Sunap_MCode == "C000")
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 20) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 20 / 100);    //노인틀니
                                }
                                else if (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000")
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 30) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 30 / 100);    //노인틀니
                                }
                                else
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 50) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 50 / 100);    //노인틀니
                                }
                            }

                            else if (clsPmpaType.TOM.Bi == "21")
                            {
                                if (i == 20)
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 20) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 20 / 100);    //노인틀니
                                }
                                else
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                }
                            }

                            else if (String.Compare(clsPmpaType.TOM.Bi, "22") <= 0)
                            {
                                if (i == 20)
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 30) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 30 / 100);    //노인틀니
                                }
                                else
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                }
                            }

                            else if (String.Compare(clsPmpaType.TOM.BDate, "2009-06-01") >= 0 && clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.Jin == "9")
                            {
                                //2009 - 05 - 29 윤조연 수정함
                                //가정간호 보호2종일경우 2009-06-01 부터 외래본인부담 10%
                                if (String.Compare(clsPmpaType.TOM.BDate, "2010-07-01") >= 0 && (clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "Y248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250"))
                                {
                                    //급여 본인부담 5%
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                }

                                else if (String.Compare(clsPmpaType.TOM.BDate, "2010-01-01") >= 0 && clsPmpaPb.GOpd_Sunap_VCode == "V194")
                                {
                                    //급여 본인부담 5%
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                }

                                else
                                {
                                    //급여 본인부담 10%
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                }
                            }

                            else if (clsPmpaType.TOM.Bi == "22" && (clsPmpaType.a.Dept == "HD" || clsPmpaType.TOM.Jin == "6"))
                            {
                                Jamt = clsPmpaType.RPG.Amt1[i];
                            }

                            //2009-04-01 차상위2 만성질환(장애인) 및 만18세미만
                            else if ((clsPmpaType.TOM.Bi == "13" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "11") && clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000")
                            {
                                //MRI, CT
                                if (i == 17 || i == 18)
                                {
                                    //중증화상 5%
                                    if ((clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "V248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250") && String.Compare(clsPmpaType.TOM.BDate, "2010-07-01") >= 0)
                                    {
                                        //중증화상 Ct,Mri 5%'급여본인부담
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                    }

                                    else if ((clsPmpaPb.GOpd_Sunap_VCode == "V193" || clsPmpaPb.GOpd_Sunap_VCode == "V194") && String.Compare(clsPmpaType.TOM.BDate, "2009-12-01") >= 0)
                                    {
                                        //중증화상 Ct,Mri 5%'급여본인부담
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                    }

                                    //2011-04-01 차상위2이면서 결핵 V -> EV01 MR,CT 10%
                                    else if ((clsPmpaPb.GOpd_Sunap_VCode == "V206" || clsPmpaPb.GOpd_Sunap_VCode == "V231" || clsPmpaPb.GOpd_Sunap_VCode == "V246") && String.Compare(clsPmpaType.TOM.BDate, "2011-04-01") >= 0)
                                    {
                                        //중증환자 Ct,Mri 10%'급여본인부담
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                    }

                                    //중증기호 V193 , 2009-08-28 차상위2이면서 희귀 V -> EV00 MR,CT 10%
                                    else if (clsPmpaPb.GOpd_Sunap_VCode == "V193" || (clsPmpaPb.GOpd_Sunap_VCode == "EV00" && String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0))
                                    {
                                        //중증환자 Ct,Mri 10%'급여본인부담
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                    }
                                    //응급실 입원 6세미만은 급여 100% 조합부담금임
                                    else if (clsPmpaType.TOM.Age < 6 && (clsPmpaType.TOM.Jin == "R" || clsPmpaType.TOM.Jin == "S" || clsPmpaType.TOM.Jin == "T" || clsPmpaType.TOM.Jin == "U") && clsPmpaPb.GOpd_Sunap_MCode == "E000" && clsPmpaPb.GstatEROVER == "*")
                                    {
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                    }
                                    //차상위 15세 미만 본부 3%
                                    else if (clsPmpaType.TOM.Age <= 15 && (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000") && clsPmpaPb.GstatEROVER == "*")
                                    {
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 3 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 3 / 100));
                                    }
                                    else
                                    {
                                        if ( clsPmpaType.TOM.JinDtl == "22" || clsPmpaType.TOM.JinDtl == "25")
                                        {
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                        }
                                        else
                                        {
                                            //Ct,Mri 14%'급여본인부담
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 14 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 14 / 100));
                                        }
                                    }
                                }

                                else
                                {
                                    if (clsPmpaType.TOM.Jin == "I" || clsPmpaType.TOM.Jin == "J")
                                    {
                                        //본인부담 0%
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                    }

                                    else
                                    {
                                        if (clsPmpaPb.GOpd_Sunap_VCode == "EV00")
                                        {
                                            //급여 본인부담 10%
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                        }
                                        else if (String.Compare(clsPmpaType.TOM.BDate, "2011-04-01") >= 0 && clsPmpaPb.GOpd_Sunap_VCode == "EV01")
                                        {
                                            //급여 본인부담 10%
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                        }
                                        else if (clsPmpaType.TOM.Age < 6 && (clsPmpaType.TOM.Jin == "R" || clsPmpaType.TOM.Jin == "S" || clsPmpaType.TOM.Jin == "T" || clsPmpaType.TOM.Jin == "U") && clsPmpaPb.GOpd_Sunap_MCode == "E000" && clsPmpaPb.GstatEROVER == "*")
                                        {
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                        }
                                        //차상위 15세 미만 본부 3%
                                        else if (clsPmpaType.TOM.Age <= 15 && (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000") && clsPmpaPb.GstatEROVER == "*")
                                        {
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 3 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 3 / 100));
                                        }
                                        else
                                        {
                                            //add
                                            if (clsPmpaPb.GOpd_Sunap_MCode == "F000")
                                            {
                                                //급여 본인부담 14% - 장애기금 100%
                                                Jamt = clsPmpaType.RPG.Amt1[i];
                                            }
                                            else if ( clsPmpaType.TOM.JinDtl == "22" || clsPmpaType.TOM.JinDtl == "25")
                                            {
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                            }
                                            else
                                            {
                                                //급여 본인부담 14%
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 14 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 14 / 100));
                                            }
                                        }
                                    }
                                }
                            }

                            else if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") && (clsPmpaType.a.Dept == "HD" || clsPmpaPb.GstrOtherHD == "*" || clsPmpaType.TOM.Jin == "6" || clsPmpaType.TOM.Jin == "9" || clsPmpaType.TOM.Jin == "A" || clsPmpaPb.GstatHULWOO == "*" || clsPmpaPb.GstatEROVER == "*"))
                            {
                                if (clsPmpaPb.GOpd_Sunap_MCode == "C000")
                                {
                                    //급여본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                }

                                else if (clsPmpaPb.GOpd_Sunap_MCode == "V001" && String.Compare(clsPmpaType.TOM.BDate, "2011-04-01") >= 0 && clsPmpaType.TOM.Jin == "9")
                                {
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                }

                                //2009-07-30 심사과 통화후 수정함 - HD 접수 및 타과 진료시 10%
                                else if ((clsPmpaPb.GOpd_Sunap_MCode == "V000" || clsPmpaPb.GOpd_Sunap_MCode == "H000") && String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaType.TOM.Jin == "9")
                                {
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                }

                                //2010-01-29 - HD 접수 및 타과 진료시 10%
                                else if ((clsPmpaPb.GOpd_Sunap_MCode == "V000" || clsPmpaPb.GOpd_Sunap_MCode == "H000") && String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaType.TOM.Jin == "6")
                                {
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                }

                                //2010-07-01 중증화상 5%
                                else if ((clsPmpaPb.GOpd_Sunap_MCode == "V247" || clsPmpaPb.GOpd_Sunap_MCode == "V248" || clsPmpaPb.GOpd_Sunap_MCode == "V249" || clsPmpaPb.GOpd_Sunap_MCode == "V250") && String.Compare(clsPmpaType.TOM.BDate, "2010-07-01") >= 0)
                                {
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                }

                                else if (clsPmpaPb.GOpd_Sunap_VCode == "V194")
                                {
                                    //급여본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaType.TOM.VCode) / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaType.TOM.VCode) / 100));
                                }

                                //2007-05-02 추가 ER6시간이상이사이고, V193이 일 경우
                                else if (clsPmpaPb.GOpd_Sunap_VCode == "V193" && clsPmpaPb.GstatEROVER == "*")
                                {
                                    //급여본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));
                                }

                                //2007-08-08 HD환자이고, V193환자
                                else if (clsPmpaPb.GOpd_Sunap_VCode == "V193" && clsPmpaType.a.Dept == "HD")
                                {
                                    //급여본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));
                                }

                                //2007-05-02 추가 ER6시간이상이사이고, 상병특례환자
                                else if (clsPmpaType.TOM.Jin == "F" && clsPmpaPb.GstatEROVER == "*")
                                {
                                    if (String.Compare(clsPmpaType.TOM.Bi, "24") <= 0)
                                    {
                                        if (i == 17 || i == 18)
                                        {
                                            //산정특례 Ct 50%'급여본인부담
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 50 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 50 / 100));
                                        }

                                        else
                                        {
                                            //본인부담 20%
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 20 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 20 / 100));
                                        }
                                    }

                                    else
                                    {
                                        //급여본인부담+비급여 본인부담
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                    }
                                }

                                else
                                {
                                    if (i != 17 && i != 18)
                                    {
                                        //F005 신생아
                                        if (clsPmpaType.TOM.Age == 0 && clsPmpaType.TOM.JinDtl == "24" && clsPmpaPb.GstatEROVER == "*")
                                        {
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                        }
                                        else if (clsPmpaType.TOM.Age < 6)
                                        {
                                            //6세미만은 급여 100% 조합부담금임
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                        }

                                        else
                                        {
                                            //2010-01-29 자격없으면 외래부담
                                            if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") || String.Compare(clsPmpaType.TOM.BDate, "2009-10-01") >= 0 && clsPmpaType.TOM.DeptCode == "HD" && clsPmpaType.a.Dept == "HD" && clsPmpaPb.GOpd_Sunap_MCode == "")
                                            {
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                            }

                                            else if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") || String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaType.TOM.Jin != "9" && clsPmpaPb.GstatEROVER.Trim() == "")
                                            {
                                                //10% hd접수 및 타과
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                            }

                                            else
                                            {
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100));
                                            }
                                        }
                                    }

                                    //CT급여분은 외래부담율
                                    else if (i == 17 || i == 18)
                                    {
                                        if (String.Compare(clsPmpaType.TOM.BDate, clsPmpaPb.OBON_DATE) >= 0)
                                        {
                                            if (clsPmpaType.TOM.Age == 0 && clsPmpaType.TOM.JinDtl == "24" && clsPmpaPb.GstatEROVER == "*")
                                            {
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                            }
                                            //6세미만은 급여 100% 조합부담금임.2007-01-05
                                            else if (clsPmpaType.TOM.Age < 6)
                                            {
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                            }

                                            else
                                            {
                                                //2010-01-29 자격없으면 외래부담
                                                if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") && String.Compare(clsPmpaType.TOM.BDate, "2009-10-01") >= 0 && clsPmpaType.TOM.DeptCode == "HD" && clsPmpaType.a.Dept == "HD" && clsPmpaPb.GOpd_Sunap_MCode == "")
                                                {
                                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                }

                                                else if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") && String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaType.TOM.Jin != "9" && clsPmpaPb.GstatEROVER == "")
                                                {
                                                    //10% hd접수 및 타과
                                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                                }

                                                else
                                                {
                                                    //50%'급여본인부담+비급여 본인부담
                                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                }

                                            }
                                        }

                                        else
                                        {
                                            //60% '급여본인부담+비급여 본인부담
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OLD_OBON[clsPmpaType.a.Bi] / 100));
                                        }
                                    }
                                }
                            }

                            else if (clsPmpaType.TOM.Bi == "22" && (clsPmpaType.TOM.Jin == "I" || clsPmpaType.TOM.Jin == "J"))
                            {
                                if (i == 17 || i == 18)
                                {
                                    if (String.Compare(clsPmpaType.TOM.BDate, "2010-07-01") >= 0 && (clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "V248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250"))
                                    {
                                        //보호22종 환자는 CT,MRI 5% 본인부담금 2010-07-01
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                    }

                                    else if (String.Compare(clsPmpaType.TOM.BDate, "2010-01-01") >= 0)
                                    {
                                        switch (clsPmpaPb.GOpd_Sunap_VCode)
                                        {
                                            case "V191":
                                            case "V192":
                                            case "V193":
                                            case "V194":
                                                //보호22종 환자는 CT,MRI 5% 본인부담금 2010-01-01
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                                break;

                                            default:
                                                //보호22종 환자는 CT,MRI 15% 본인부담금 2005-11-4
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 15 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 15 / 100));
                                                break;
                                        }
                                    }

                                    else
                                    {
                                        switch (clsPmpaPb.GOpd_Sunap_VCode)
                                        {
                                            case "V193":
                                                //보호22종 환자는 CT,MRI 10% 본인부담금 2005-12-01
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                                break;

                                            default:
                                                //보호22종 환자는 CT,MRI 15% 본인부담금 2005-11-4
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 15 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 15 / 100));
                                                break;
                                        }
                                    }
                                }

                                else
                                {
                                    Jamt = clsPmpaType.RPG.Amt1[i];
                                }
                            }

                            else if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.a.Dept == "NP")
                            {
                                if (i == 7)
                                {
                                    if (clsPmpaPb.GstrSPR == "OK")
                                    {
                                        if (string.Compare(clsPmpaType.TOM.BDate, "2021-04-01") >= 0)
                                        {
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 5 / 100);
                                            Bamt += clsPmpaPb.GnNPInjAmt * 5 / 100;
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 5 / 100);
                                            Jamt -= clsPmpaPb.GnNPInjAmt * 5 / 100;
                                        }
                                        else
                                        {
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 5 / 100);
                                            Bamt += clsPmpaPb.GnNPInjAmt * 10 / 100;
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 5 / 100);
                                            Jamt -= clsPmpaPb.GnNPInjAmt * 10 / 100;
                                        }
                                    }
                                    else
                                    {
                                        if (string.Compare(clsPmpaType.TOM.BDate, "2021-04-01") >= 0)
                                        {
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 10 / 100);
                                            Bamt += clsPmpaPb.GnNPInjAmt * 5 / 100;

                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 10 / 100);
                                            Jamt -= clsPmpaPb.GnNPInjAmt * 5 / 100;
                                        }
                                        else
                                        {
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 10 / 100);
                                            Bamt += clsPmpaPb.GnNPInjAmt * 10 / 100;

                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 10 / 100);
                                            Jamt -= clsPmpaPb.GnNPInjAmt * 10 / 100;
                                        }
                                         
                                    }
                                }
                                else if (i == 17 || i == 18)
                                {
                                    Bamt = clsPmpaType.RPG.Amt1[i] * 15 / 100;
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * 15 / 100);
                                }
                                else
                                {
                                    if (clsPmpaPb.GstrSPR == "")
                                    {
                                        Bamt = clsPmpaType.RPG.Amt1[i] * 10 / 100;
                                        Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * 10 / 100);
                                    }
                                    else if (clsPmpaPb.GstrSPR == "OK")
                                    {
                                        Bamt = clsPmpaType.RPG.Amt1[i] * 5 / 100;
                                        Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * 5 / 100);
                                    }
                                }
                            }
                            //타병원 정신과 입원중인환자 내원 진료시 보험적용 및 본인부담 10%
                            else if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.JinDtl == "18")
                            {
                                Bamt = clsPmpaType.RPG.Amt1[i] * 10 / 100;
                                Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * 10 / 100);
                            }
                            else
                            {
                                if (String.Compare(clsPmpaType.TOM.BDate, clsPmpaPb.OBON_DATE) >= 0)
                                {
                                    //윤2004
                                    //2008-04-01 시행

                                    //차상위 계층환자 본인부담금 0%
                                    if (clsPmpaPb.GOpd_Sunap_MCode == "C000")
                                    {
                                        if (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13")
                                        {
                                            //급여본인부담+비급여 본인부담
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                        }
                                    }

                                    else if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") && (clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "V248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250") && String.Compare(clsPmpaType.TOM.BDate, "2010-07-01") >= 0)
                                    {
                                        //중증화상 5%'급여본인부담
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                    }

                                    else if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") && (clsPmpaPb.GOpd_Sunap_VCode == "V193" || clsPmpaPb.GOpd_Sunap_VCode == "V194") && String.Compare(clsPmpaType.TOM.BDate, "2009-12-01") >= 0)
                                    {
                                        //중증화상 5%'급여본인부담
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                    }

                                    //윤조연추가 2009-07-01 등록 산정특례 희귀난치성시행
                                    else if (String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_MCode == "V000" && (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13"))
                                    {
                                        //중증화상 5%'급여본인부담
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                    }

                                    //2008-11-04 추가
                                    else if (clsPmpaType.TOM.Age >= 6 && clsPmpaType.TOM.Jin == "C" && clsPmpaPb.GOpd_Sunap_MCode == "H000" && String.Compare(clsPmpaType.TOM.Bi, "13") <= 0)
                                    {
                                        if (i == 17 || i == 18)
                                        {
                                            //윤조연추가 2009-07-01 산정특례 희귀난치성시행
                                            if (String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_MCode == "H000" && (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13"))
                                            {
                                                //산정특례 Ct 10%'급여본인부담
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                            }

                                            else
                                            {
                                                //산정특례 Ct 50%'급여본인부담
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 50 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 50 / 100));
                                            }
                                        }

                                        else
                                        {
                                            //윤조연추가 2009-07-01 산정특례 희귀난치성시행
                                            if (String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_MCode == "H000" && (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13"))
                                            {
                                                //본인부담 10%
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                            }

                                            else
                                            {
                                                //본인부담 20%
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 20 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 20 / 100));
                                            }
                                        }

                                    }

                                    else if (clsPmpaType.TOM.Age >= 6 && (clsPmpaType.TOM.Jin == "F" || clsPmpaType.TOM.Jin == "G"))
                                    {
                                        if (String.Compare(clsPmpaType.TOM.Bi, "24") <= 0)
                                        {
                                            if (i == 17 || i == 18)
                                            {
                                                //윤조연추가 2009-07-01 산정특례 희귀난치성시행
                                                if (String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_MCode == "H000" && (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13"))
                                                {
                                                    //산정특례 Ct 10%'급여본인부담
                                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                                }

                                                else
                                                {
                                                    //산정특례 Ct 50%'급여본인부담
                                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 50 / 100));
                                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 50 / 100));
                                                }
                                            }

                                            else
                                            {
                                                //윤조연추가 2009-07-01 산정특례 희귀난치성시행
                                                if (String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_MCode == "H000" && (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13"))
                                                {
                                                    //본인부담 10%
                                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                                }

                                                else
                                                {
                                                    //본인부담 20%
                                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 20 / 100));
                                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 20 / 100));
                                                }
                                            }
                                        }

                                        else
                                        {
                                            //급여본인부담+비급여 본인부담
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                        }
                                    }

                                    else if (clsPmpaType.TOM.Age <= 5 && (clsPmpaType.TOM.Jin == "R" || clsPmpaType.TOM.Jin == "S" || clsPmpaType.TOM.Jin == "T" || clsPmpaType.TOM.Jin == "U") && String.Compare(clsPmpaType.TOM.BDate, "2007-08-01") >= 0 && String.Compare(clsPmpaType.TOM.Bi, "13") <= 0)
                                    {
                                        //윤조연추가 2009-07-01 산정특례 희귀난치성시행
                                        if (String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_MCode == "H000" && (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13"))
                                        {
                                            //본인부담 10%
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                        }

                                        else
                                        {
                                            //소아(만6세미만) 본인부담율 35% => CT.MRI 35%
                                            if (String.Compare(clsPmpaType.TOM.BDate, "2007-08-01") >= 0 && (clsPmpaType.TOM.Jin == "R" || clsPmpaType.TOM.Jin == "U"))
                                            {
                                                //급여본인부담+비급여 본인부담
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 35 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 35 / 100));
                                            }

                                            //소아(만6세미만) 상병특례환자 본인부담율 14%
                                            else if (String.Compare(clsPmpaType.TOM.BDate, "2007-08-01") >= 0 && (clsPmpaType.TOM.Jin == "S" || clsPmpaType.TOM.Jin == "T"))
                                            {
                                                if (i == 17 || i == 18)
                                                {
                                                    //CT.MRI 35%
                                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 35 / 100));
                                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 35 / 100));
                                                }

                                                else
                                                {
                                                    //급여본인부담+비급여 본인부담
                                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 14 / 100));
                                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 14 / 100));
                                                }

                                            }
                                        }
                                    }

                                    else
                                    {
                                        //2007-07-01                                        
                                        if (clsPmpaType.TOM.Bi == "21" && String.Compare(clsPmpaType.TOM.BDate, "2007-07-01") >= 0 && (i == 17 || i == 18) && clsPmpaPb.GOpd_Sunap_MCode == "M000")
                                        {
                                            //보호1종 환자 CT,MRI 5% 본인 부담
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                        }

                                        else
                                        {
                                            //2005-09-01
                                            switch (clsPmpaPb.GOpd_Sunap_VCode)
                                            {
                                                case "V193":
                                                case "V194":
                                                    if ((String.Compare(clsPmpaType.TOM.Bi, "11") >= 0 && String.Compare(clsPmpaType.TOM.Bi, "13") <= 0) || clsPmpaType.TOM.Bi == "22")
                                                    {
                                                        //급여본인부담+비급여 본인부담
                                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));
                                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));
                                                    }

                                                    else if (clsPmpaType.TOM.Bi == "21")
                                                    {
                                                        //급여본인부담+비급여 본인부담
                                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                    }
                                                    break;

                                                case "V247":
                                                case "V248":
                                                case "V249":
                                                case "V250":
                                                    if ((String.Compare(clsPmpaType.TOM.Bi, "11") >= 0 && String.Compare(clsPmpaType.TOM.Bi, "13") <= 0 || clsPmpaType.TOM.Bi == "22") && String.Compare(clsPmpaType.TOM.BDate, "2010-07-01") >= 0)
                                                    {
                                                        //중증화상 5%'급여본인부담
                                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                                    }

                                                    else
                                                    {
                                                        //급여본인부담+비급여 본인부담
                                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                    }
                                                    break;

                                                case "F003":
                                                    //약품비만
                                                    if (i == 5)
                                                    {
                                                        //급여본인부담+비급여 본인부담
                                                        Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnDrugRPAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                                        Bamt += ((clsPmpaPb.GnDrugRPAmt * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));   //약값 GnDrugAmt


                                                        Jamt = clsPmpaType.RPG.Amt1[i] - (((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnDrugRPAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                        Jamt -= ((clsPmpaPb.GnDrugRPAmt * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));   //약값 GnDrugAmt
                                                    }

                                                    else
                                                    {
                                                        //급여본인부담+비급여 본인부담
                                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                    }
                                                    break;

                                                default:
                                                    //2013-11-07  응급실 6시간이상 입원의 경우 입원기준으로 출력되어야 하므로  기준을 변경함
                                                    if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.Age < 6 && (clsPmpaPb.GstatEROVER == "*" || clsPmpaPb.GOpd_Sunap_JinDtl2 == "E"))
                                                    {
                                                        Bamt = clsPmpaType.RPG.Amt1[i] * 0 / 100;
                                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                                    }
                                                    else if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.Age <= 15 && (clsPmpaPb.GstatEROVER == "*" || clsPmpaPb.GOpd_Sunap_JinDtl2 == "E"))
                                                    {
                                                        Bamt = clsPmpaType.RPG.Amt1[i] * 3 / 100;
                                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 3 / 100));
                                                    }
                                                    else if (String.Compare(clsPmpaType.TOM.ActDate, "2013-11-07") >= 0 && (clsPmpaPb.GstatEROVER == "*" || clsPmpaPb.GOpd_Sunap_JinDtl2 == "E"))
                                                    {
                                                        //급여본인부담+비급여 본인부담
                                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100));
                                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100));
                                                    }

                                                    else if (String.Compare(clsPmpaType.TOM.ActDate, "2012-06-01") >= 0 && clsPmpaType.TOM.Bi == "22" && (clsPmpaPb.GstatEROVER == "*" || clsPmpaPb.GOpd_Sunap_JinDtl2 == "E"))
                                                    {
                                                        //2012-06-13
                                                        //급여본인부담+비급여 본인부담
                                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100));
                                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100));

                                                    }

                                                    else
                                                    {
                                                        //급여본인부담+비급여 본인부담
                                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    //급여본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OLD_OBON[clsPmpaType.a.Bi] / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OLD_OBON[clsPmpaType.a.Bi] / 100));
                                }
                            }

                        }
                    }

                    #endregion Gensan_Johap_Chk(GoSub) End

                    if (clsPmpaType.RPG.Amt4[i] > 0)
                    {
                        MessageBox.Show(""); 
                    }
                    clsPmpaType.RPG.Amt2[i] = clsPmpaType.RPG.Amt2[i];  //비급여
                    clsPmpaType.RPG.Amt3[i] = clsPmpaType.RPG.Amt3[i];  //특진
                    clsPmpaType.RPG.Amt4[i] = clsPmpaType.RPG.Amt4[i];  //본인총액

                    if ((clsPmpaPb.GOpd_Sunap_MCode == "H000" || clsPmpaPb.GOpd_Sunap_MCode == "F000") && clsPmpaPb.GOpd_Sunap_JinDtl != "02")
                    {
                        Jamt += Bamt;
                        Bamt = 0;
                    }

                    else if (clsPmpaType.TOM.Bi == "21" || clsPmpaType.TOM.Bi == "22")
                    {
                        if (clsPmpaType.TOM.Bi == "21" && String.Compare(clsPmpaType.TOM.BDate, "2007-07-01") >= 0 && (i == 17 || i == 18) && clsPmpaPb.GOpd_Sunap_MCode == "M000" || clsPmpaPb.GOpd_Sunap_JinDtl == "02")
                        {
                            i = i;
                        }
                        else if (clsPmpaType.TOM.Bi == "22" && clsPmpaPb.GOpd_Sunap_JinDtl2 == "E")
                        {
                            i = i;
                        }
                        else
                        {
                            Jamt += Bamt;
                            Bamt = 0;
                        }
                    }

                    clsPmpaType.RPG.Amt5[i] = Bamt;                         //본인부담
                    clsPmpaType.RPG.Amt6[i] = Jamt;                         //공단부담
                    clsPmpaType.RPG.Amt7[i] = clsPmpaType.RPG.Amt4[i];      //본인총액

                    //Sub 합
                    clsPmpaType.RPG.Amt1[36] += clsPmpaType.RPG.Amt5[i];    //본인총액
                    clsPmpaType.RPG.Amt1[37] += clsPmpaType.RPG.Amt6[i];    //공단부담
                    clsPmpaType.RPG.Amt1[38] += clsPmpaType.RPG.Amt4[i];    //전액본인
                    clsPmpaType.RPG.Amt1[39] += clsPmpaType.RPG.Amt3[i];    //선택료
                    clsPmpaType.RPG.Amt1[40] += clsPmpaType.RPG.Amt2[i];    //비급여
                }
            }

            //예약진찰 누적
            clsPmpaType.RPG.Amt1[31] += clsPmpaType.RPG.Amt1[23] + clsPmpaType.RPG.Amt3[23];    //-> 기존금액 + 예약비

            clsPmpaType.RPG.Amt1[33] += clsPmpaType.RPG.Amt5[23] + clsPmpaType.RPG.Amt3[23];

            clsPmpaType.RPG.Amt1[36] += clsPmpaType.RPG.Amt5[23];           //본인총액
            clsPmpaType.RPG.Amt1[37] += clsPmpaType.RPG.Amt6[23];           //공단부담
            clsPmpaType.RPG.Amt1[38] += clsPmpaType.RPG.Amt4[23];           //전액본인
            clsPmpaType.RPG.Amt1[39] += clsPmpaType.RPG.Amt3[23];           //선택료
            clsPmpaType.RPG.Amt1[40] += clsPmpaType.RPG.Amt2[23];           //비급여

            //이전 절사액 포함여부
            strChk = "";

            if (clsPmpaType.RPG.Amt1[33] != (VB.Fix(Convert.ToInt32(clsPmpaType.RPG.Amt1[36] / 100)) * 100) && (VB.Fix(Convert.ToInt32(clsPmpaType.RPG.Amt1[36] / 100)) * 100) != 0)
            {
                for (i = 1; i <= 30; i++)
                {
                    clsPmpaType.RPG.Amt2[i] = clsPmpaType.RPG.Amt2[i];      //비급여
                    clsPmpaType.RPG.Amt3[i] = clsPmpaType.RPG.Amt3[i];      //특진
                    clsPmpaType.RPG.Amt4[i] = clsPmpaType.RPG.Amt4[i];      //본인총액

                    clsPmpaType.RPG.Amt5[i] = clsPmpaType.RPG.Amt5[i];      //본인부담
                    clsPmpaType.RPG.Amt6[i] = clsPmpaType.RPG.Amt6[i];      //공단부담
                    //clsPmpaType.RPG.Amt7[i] = clsPmpaType.RPG.Amt4[i];      //본인총액

                    //에약진찰 제외
                    if (i != 23)
                    {
                        if (i > 1 && clsPmpaType.RPG.Amt5[i] > 0 && strChk != "OK" && clsPmpaPb.GnOpd_Sunap_LastDan > 0)
                        {
                            strChk = "OK";

                            clsPmpaType.RPG.Amt5[i] += clsPmpaPb.GnOpd_Sunap_LastDan;   //본인부담
                            clsPmpaType.RPG.Amt6[i] = clsPmpaType.RPG.Amt6[i];

                            //Sub 합
                            clsPmpaType.RPG.Amt1[36] += clsPmpaPb.GnOpd_Sunap_LastDan;  //본인총액
                            clsPmpaType.RPG.Amt1[37] = clsPmpaType.RPG.Amt1[37];        //공단부담
                            clsPmpaType.RPG.Amt1[38] = clsPmpaType.RPG.Amt1[38];        //전액본인
                            clsPmpaType.RPG.Amt1[39] = clsPmpaType.RPG.Amt1[39];        //선택료
                            clsPmpaType.RPG.Amt1[40] = clsPmpaType.RPG.Amt1[40];        //비급여

                            //진료비총액
                            //clsPmpaType.RPG.Amt1[40] += clsPmpaPb.GnOpd_Sunap_LastDan;
                        }
                    }
                }

                //이전절사 있는데 진찰만 있을경우
                if (strChk != "OK" && clsPmpaType.RPG.Amt5[1] > 0 && strChk != "OK" && clsPmpaPb.GnOpd_Sunap_LastDan > 0)
                {
                    strChk = "OK";
                    clsPmpaType.RPG.Amt5[1] += clsPmpaPb.GnOpd_Sunap_LastDan;           //본인부담
                    clsPmpaType.RPG.Amt6[1] = clsPmpaType.RPG.Amt6[1];

                    //Sub 합
                    clsPmpaType.RPG.Amt1[36] += clsPmpaPb.GnOpd_Sunap_LastDan;          //본인총액
                    clsPmpaType.RPG.Amt1[37] = clsPmpaType.RPG.Amt1[37];                //공단부담
                    clsPmpaType.RPG.Amt1[38] = clsPmpaType.RPG.Amt1[38];                //전액본인
                    clsPmpaType.RPG.Amt1[39] = clsPmpaType.RPG.Amt1[39];                //선택료
                    clsPmpaType.RPG.Amt1[40] = clsPmpaType.RPG.Amt1[40];                //비급여

                    //진료비총액
                    clsPmpaType.RPG.Amt1[31] += clsPmpaPb.GnOpd_Sunap_LastDan;
                }
            }

            //건강보험 희귀난치 지원금
            if (String.Compare(clsPmpaType.TOM.Bi, "14") < 0 && clsPmpaPb.GOpd_Sunap_MCode == "H000")
            {
                clsPmpaType.RPG.Amt1[37] += clsPmpaType.RPG.Amt1[36];
                clsPmpaType.RPG.Amt1[36] -= clsPmpaType.RPG.Amt1[36];
            }

            //차상위2종 정액 처리
            else if (String.Compare(clsPmpaType.TOM.Bi, "14") < 0 && clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000" && (clsPmpaType.TOM.Jin == "I" || clsPmpaType.TOM.Jin == "J"))
            {
                if (clsPmpaPb.GOpd_Sunap_VCode == "V206" || clsPmpaPb.GOpd_Sunap_VCode == "V231" || clsPmpaPb.GOpd_Sunap_VCode == "V246")
                {
                    if (clsPmpaPb.GOpd_Sunap_MCode == "E000")
                    {
                        if (clsPmpaType.TOM.Jin == "I")
                        {
                            clsPmpaType.RPG.Amt1[36] = Convert.ToInt32((clsPmpaType.RPG.Amt1[36] + 1500) * 50 / 100);
                            clsPmpaType.RPG.Amt1[37] += clsPmpaType.RPG.Amt1[36] - 1500;
                        }

                        else if (clsPmpaType.TOM.Jin == "J")
                        {
                            clsPmpaType.RPG.Amt1[36] = Convert.ToInt32((clsPmpaType.RPG.Amt1[36] + 1000) * 50 / 100);
                            clsPmpaType.RPG.Amt1[37] += clsPmpaType.RPG.Amt1[36] - 1000;
                        }
                    }

                    else if (clsPmpaType.TOM.MCode == "F000")
                    {

                    }
                }

                else if (clsPmpaPb.GOpd_Sunap_MCode == "E000")
                {
                    if (clsPmpaType.TOM.Jin == "I")
                    {
                        clsPmpaType.RPG.Amt1[36] = 1500;
                        clsPmpaType.RPG.Amt1[37] -= 1500;
                    }
                    else if (clsPmpaType.TOM.Jin == "J")
                    {
                        clsPmpaType.RPG.Amt1[36] = 1000;
                        clsPmpaType.RPG.Amt1[37] -= 1000;
                    }
                }

                else if (clsPmpaPb.GOpd_Sunap_MCode == "F000")
                {

                }
            }

            //차상위2종 정액 처리
            else if (String.Compare(clsPmpaType.TOM.Bi, "14") < 0 && clsPmpaType.TOM.MCode == "F000")
            {

            }

            else if (clsPmpaType.TOM.Bi == "21")
            {
                clsPmpaType.RPG.Amt1[33] += clsPmpaType.RPG.Amt1[35];
                clsPmpaType.RPG.Amt1[36] += clsPmpaPb.GOpd_Sunap_Boamt + clsPmpaType.RPG.Amt1[35];
                clsPmpaType.RPG.Amt1[37] -= clsPmpaPb.GOpd_Sunap_Boamt;
            }

            else if (clsPmpaType.TOM.Bi == "22")
            {
                if (clsPmpaType.TOM.Bohun == "3")
                {
                    clsPmpaType.RPG.Amt1[37] += clsPmpaType.RPG.Amt1[36];
                    clsPmpaType.RPG.Amt1[36] -= clsPmpaType.RPG.Amt1[36];
                }
            }

        }

        public void Last_Report_Amt_Gesan_2017()
        {
            clsBasAcct CBA = new clsBasAcct();

            int i = 0;
            long Bamt = 0;
            long Jamt = 0;
            int nTemp = 0;
            string strChk = "";

            for (i = 1; i <= 30; i++)
            {
                Bamt = 0;
                Jamt = 0;
                nTemp = 0;
                
                //예약진찰제외
                if (i != 23)
                {
                    #region Gensan_Johap_Chk(GoSub)
                    //2012 - 07 - 01 노인틀니
                    if (clsPmpaType.TOM.DeptCode == "DT" && String.Compare(clsPmpaType.TOM.BDate, "2017-11-01") >= 0 && clsPmpaPb.GOpd_Sunap_JinDtl == "02" && String.Compare(clsPmpaType.TOM.Bi, "22") <= 0)
                    {
                        if (String.Compare(clsPmpaType.TOM.Bi, "13") <= 0)
                        {
                            if (i == 20)
                            {
                                if (clsPmpaPb.GOpd_Sunap_MCode == "C000")
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 5) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 5 / 100);    //노인틀니
                                }
                                else if (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000")
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 15) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 15 / 100);    //노인틀니
                                }
                                else
                                {
                                    //급여 본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Bamt += (clsPmpaPb.GnToothRpAmt * 30) / 100;    //노인틀니
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                    Jamt -= (clsPmpaPb.GnToothRpAmt * 30 / 100);    //노인틀니
                                }
                            }
                            else
                            {
                                Bamt = (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                            }
                        }
                        else if (clsPmpaType.TOM.Bi == "21")
                        {
                            if (i == 20)
                            {
                                Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Bamt += (clsPmpaPb.GnToothRpAmt * 5) / 100;    //노인틀니
                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt -= (clsPmpaPb.GnToothRpAmt * 5 / 100);    //노인틀니
                            }
                            else
                            {
                                Bamt = (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                            }
                        }
                        else if (String.Compare(clsPmpaType.TOM.Bi, "22") <= 0)
                        {
                            if (i == 20)
                            {
                                Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Bamt += (clsPmpaPb.GnToothRpAmt * 15) / 100;    //노인틀니
                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt -= (clsPmpaPb.GnToothRpAmt * 15 / 100);    //노인틀니
                            }
                            else
                            {
                                Bamt = (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                            }
                        }
                    }
                    else if (string.Compare(clsPmpaType.TOM.BDate, "2009-06-01") >= 0 && clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.Jin == "9")
                    {
                        if (string.Compare(clsPmpaType.TOM.BDate , "2010-07-01") >=0 && (clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "V248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250"))
                        {
                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100)); // '급여 본인부담 5%
                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                        }
                        else if (string.Compare(clsPmpaType.TOM.BDate , "2010-01-01") >= 0 && clsPmpaPb.GOpd_Sunap_VCode == "V194" )
                        {
                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));// '급여 본인부담 5%
                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                        }
                        else
                        {
                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));// '급여 본인부담 10%
                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                        }
                    }
                    else if (clsPmpaType.TOM.Bi == "22" && (clsPmpaType.a.Dept == "HD" || clsPmpaType.TOM.Jin == "6") )
                    {
                        Jamt = clsPmpaType.RPG.Amt1[i];
                    }
                    else if ((clsPmpaType.TOM.Bi == "13" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "11") && (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000"))
                    {
                        if (i == 17 || i == 18)
                        {
                            if (((clsPmpaPb.GOpd_Sunap_VCode == "V000" || clsPmpaPb.GOpd_Sunap_VCode == "V010") && string.Compare(clsPmpaType.TOM.BDate, "2016-07-01") >= 0))
                            {
                                Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100)); //2009-12-01 중증화상 Ct,Mri 5%'급여본인부담
                                Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                            }
                            else if (clsPmpaType.TOM.Age <= 5 && (clsPmpaType.TOM.Jin == "R" || clsPmpaType.TOM.Jin == "S" || clsPmpaType.TOM.Jin == "T" || clsPmpaType.TOM.Jin == "U") && clsPmpaPb.GOpd_Sunap_MCode == "E000" && clsPmpaPb.GstatEROVER == "*")
                            {
                                Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100)); //2009-12-01 중증화상 Ct,Mri 5%'급여본인부담
                                Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                            }

                            else if (clsPmpaType.TOM.Age < 6 && clsPmpaPb.GOpd_Sunap_MCode == "E000" && clsPmpaPb.GstatEROVER == "*")//차상위 15세 미만 본부 3%
                            {
                                Bamt = (int)(clsPmpaType.RPG.Amt1[i] * 0 / 100); // '중증환자 Ct,Mri 10%'급여본인부담
                                Jamt = clsPmpaType.RPG.Amt1[i] - (int)(clsPmpaType.RPG.Amt1[i] * 0 / 100);
                            }
                            else if (clsPmpaType.TOM.Age <= 15 && (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000") && clsPmpaPb.GstatEROVER == "*")//차상위 15세 미만 본부 3%
                            {
                                Bamt = (int)(clsPmpaType.RPG.Amt1[i] * 3 / 100); // '중증환자 Ct,Mri 10%'급여본인부담
                                Jamt = clsPmpaType.RPG.Amt1[i] - (int)(clsPmpaType.RPG.Amt1[i] * 3 / 100);
                            }
                            else if ((clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "V248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250") && string.Compare(clsPmpaType.TOM.BDate , "2010-07-01") >= 0)
                            {
                                Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100)); //2009-12-01 중증화상 Ct,Mri 5%'급여본인부담
                                Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                            }
                            else if ((clsPmpaPb.GOpd_Sunap_VCode == "V193" || clsPmpaPb.GOpd_Sunap_VCode == "V194") && string.Compare(clsPmpaType.TOM.BDate , "2009-12-01") >= 0)
                            {
                                Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));  // '2009-12-01 중증환자 Ct,Mri 5%'급여본인부담
                                Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                            }
                            else if ((clsPmpaPb.GOpd_Sunap_VCode == "V206" || clsPmpaPb.GOpd_Sunap_VCode == "V231" || clsPmpaPb.GOpd_Sunap_VCode == "V246") && string.Compare(clsPmpaType.TOM.BDate , "2011-04-01") >=0)
                            {
                                Bamt = (int)(clsPmpaType.RPG.Amt1[i] * 10 / 100); // '중증환자 Ct,Mri 10%'급여본인부담
                                Jamt = clsPmpaType.RPG.Amt1[i] - (int)(clsPmpaType.RPG.Amt1[i] * 10 / 100);
                            }

                            else if (clsPmpaPb.GOpd_Sunap_VCode == "V193" || (clsPmpaPb.GOpd_Sunap_VCode == "EV00" && string.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0))
                            {
                                //'중증기호 V193 , 2009-08-28 차상위2이면서 희귀 V -> EV00 MR,CT 10%
                                Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100)); //'중증환자 Ct,Mri 10%'급여본인부담
                                Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                            }
                            else
                            {
                                if (clsPmpaType.TOM.JinDtl == "22" || clsPmpaType.TOM.JinDtl == "25")
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100)); //'중증환자 Ct,Mri 10%'급여본인부담
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                }
                                else if (clsPmpaType.TOM.Age == 0 && string.Compare(clsPmpaType.TOM.BDate, "2019-01-01") >= 0)
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100)); //'중증환자 Ct,Mri 10%'급여본인부담
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                }
                                else
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 14 / 100)); //'Ct,Mri 14%'급여본인부담
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 14 / 100));
                                }


                            }
                        }
                        else
                        {
                            if (clsPmpaType.TOM.Jin == "I" || clsPmpaType.TOM.Jin == "J")
                            {
                                Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));//'본인부담 0%
                                Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                            }
                            else
                            {
                                if ((clsPmpaPb.GOpd_Sunap_VCode == "V000" || clsPmpaPb.GOpd_Sunap_VCode == "V010") && string.Compare(clsPmpaType.TOM.BDate, "2016-07-01") >= 0)
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));//'본인부담 0%
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                }
                                else if (clsPmpaType.TOM.Age == 0 && string.Compare(clsPmpaType.TOM.BDate, "2019-01-01") >= 0 && clsPmpaPb.GstatEROVER == "")
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));//'본인부담 10%
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                }
                                else if (clsPmpaPb.GOpd_Sunap_VCode == "EV00")
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));//'본인부담 0%
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                }
                                else if (clsPmpaPb.GOpd_Sunap_VCode == "EV01")
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));//'본인부담 0%
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                }
                                else if (clsPmpaType.TOM.Age <= 5 && (clsPmpaType.TOM.Jin == "R" || clsPmpaType.TOM.Jin == "S" || clsPmpaType.TOM.Jin == "T" || clsPmpaType.TOM.Jin == "U") && clsPmpaPb.GOpd_Sunap_MCode == "E000" && clsPmpaPb.GstatEROVER == "*")
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));//'본인부담 0%
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                }
                                else if (clsPmpaType.TOM.Age <= 15 && (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000") && clsPmpaPb.GstatEROVER == "*")
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 3 / 100));//'본인부담 0%
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 3 / 100));
                                }

                                else
                                {
                                    if (clsPmpaPb.GOpd_Sunap_MCode == "F000")
                                    {
                                        Jamt = clsPmpaType.RPG.Amt1[i];//  '급여 본인부담 14% - 장애기금 100%
                                    }
                                    else if (clsPmpaType.TOM.JinDtl == "22" || clsPmpaType.TOM.JinDtl == "25")
                                    {
                                        Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));//'본인부담 5%
                                        Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                    }
                                    else if (clsPmpaType.TOM.Age == 0 && string.Compare(clsPmpaType.TOM.BDate, "2019-01-01") >= 0)
                                    {
                                        Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));//'본인부담 5%
                                        Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                    }

                                    else
                                    {
                                        Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 14 / 100));// '급여 본인부담 14%
                                        Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 14 / 100)); 
                                    }
                                }
                            }
                        }
                    }
                    else if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") && (clsPmpaType.a.Dept == "HD" || clsPmpaPb.GstrOtherHD == "*" || clsPmpaType.TOM.Jin == "6" || clsPmpaType.TOM.Jin == "9" || clsPmpaType.TOM.Jin == "A" || clsPmpaPb.GstatHULWOO == "*" || clsPmpaPb.GstatEROVER == "*"))
                    {
                        if (clsPmpaPb.GOpd_Sunap_MCode == "C000")
                        {
                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));// //급여본인부담+비급여 본인부담
                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                        }
                        else if (clsPmpaPb.GOpd_Sunap_MCode == "V001" && string.Compare(clsPmpaType.TOM.BDate, "2011-04-01") >= 0 && clsPmpaType.TOM.Jin == "9")
                        {
                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                        }

                        else if (clsPmpaType.TOM.Age == 0 && clsPmpaType.TOM.JinDtl == "24" && clsPmpaPb.GstatEROVER == "*")
                        {
                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                        }
                        else if (clsPmpaType.TOM.Age <= 15 && clsPmpaPb.GstatEROVER == "*")
                        {
                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                        }


                        else if ((clsPmpaPb.GOpd_Sunap_MCode == "V000" || clsPmpaPb.GOpd_Sunap_MCode == "H000") && string.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaType.TOM.Jin == "9") //2009-07-30 심사과 통화후 수정함 - HD 접수 및 타과 진료시 10%
                        {
                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                        }
                        else if ((clsPmpaPb.GOpd_Sunap_MCode == "V000" || clsPmpaPb.GOpd_Sunap_MCode == "H000") && string.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaType.TOM.Jin == "6") //2010-01-29 - HD 접수 및 타과 진료시 10%
                        {
                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                        }
                        else if ((clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "V248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250") && string.Compare(clsPmpaType.TOM.BDate, "2010-07-01") >= 0) //2010-07-01 중증화상 5%
                        {
                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                        }
                        else if (clsPmpaPb.GOpd_Sunap_VCode == "V194")
                        {
                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * READ_Cancer_BonRate_CHK(clsDB.DbCon, clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaType.TOM.VCode) / 100));// //급여본인부담+비급여 본인부담
                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * READ_Cancer_BonRate_CHK(clsDB.DbCon,clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaType.TOM.VCode) / 100));
                        }
                        else if (clsPmpaPb.GOpd_Sunap_VCode == "V193" && clsPmpaPb.GstatEROVER == "*")
                        {
                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * READ_Cancer_BonRate_CHK(clsDB.DbCon,clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));// //급여본인부담+비급여 본인부담
                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * READ_Cancer_BonRate_CHK(clsDB.DbCon,clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));
                        }
                        else if (clsPmpaType.TOM.VCode == "V193" && clsPmpaType.a.Dept == "HD")
                        {
                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * READ_Cancer_BonRate_CHK(clsDB.DbCon,clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));// //급여본인부담+비급여 본인부담
                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * READ_Cancer_BonRate_CHK(clsDB.DbCon,clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));
                        }
                        else if (clsPmpaType.TOM.Jin == "F" && clsPmpaPb.GstatEROVER == "*")
                        {
                            if (string.Compare(clsPmpaType.TOM.Bi , "24") <= 0)
                            {
                                if (i == 17 || i == 18)
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 50 / 100)); //산정특례 Ct 50%'급여본인부담
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 50 / 100));
                                }
                                else
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 20 / 100)); //본인부담 20%
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 20 / 100));
                                }
                            }
                            else
                            {
                                Bamt = (int)((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100)); //급여본인부담+비급여 본인부담
                                Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                            }
                        }
                        else
                        {
                            if (i != 17 && i != 18)
                            {
                                if (clsPmpaType.TOM.Age == 0 && clsPmpaType.TOM.JinDtl == "24" && clsPmpaPb.GstatEROVER == "*") //6세미만은 급여 100% 조합부담금임.2007-01-05
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                }
                                else if (clsPmpaType.TOM.Age < 6) //6세미만은 급여 100% 조합부담금임.2007-01-05
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                }
                                else
                                {
                                    if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") && string.Compare(clsPmpaType.TOM.BDate, "2009-10-01") >= 0 && clsPmpaType.TOM.DeptCode == "HD" && clsPmpaType.a.Dept == "HD" && clsPmpaPb.GOpd_Sunap_MCode == "") //2010-01-29 자격없으면 외래부담
                                    {
                                        Bamt = (int)((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                    }
                                    else if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") && string.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaType.TOM.Jin != "9" && VB.Trim(clsPmpaPb.GstatEROVER) == "")
                                    {
                                        Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100)); //10% hd접수 및 타과
                                        Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                    }
                                    else
                                    {
                                        Bamt = (int)((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100));
                                    }
                                }
                            }
                            else if (i == 17 || i == 18) //CT급여분은 외래부담율
                            {
                                if (string.Compare(clsPmpaType.TOM.BDate, clsPmpaPb.OBON_DATE) >= 0)
                                {
                                    if (clsPmpaType.TOM.Age ==0 && clsPmpaPb.GstatEROVER == "*" ) //6세미만은 급여 100% 조합부담금임.2007-01-05
                                    {
                                        Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                    }
                                    else if (clsPmpaType.TOM.Age < 6) //6세미만은 급여 100% 조합부담금임.2007-01-05
                                    {
                                        Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                    }
                                    else
                                    {
                                        if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") && string.Compare(clsPmpaType.TOM.BDate, "2009-10-01") >= 0 && clsPmpaType.TOM.DeptCode == "HD" && clsPmpaType.a.Dept == "HD" && clsPmpaPb.GOpd_Sunap_MCode == "") //2010-01-29 자격없으면 외래부담
                                        {
                                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                        }
                                        else if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") && string.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaType.TOM.Jin != "9" && VB.Trim(clsPmpaPb.GstatEROVER) == "")
                                        {
                                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100)); //10% hd접수 및 타과
                                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                        }
                                        else
                                        {
                                            Bamt = (int)((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100)); //50%'급여본인부담+비급여 본인부담
                                            Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                        }
                                    }
                                }
                                else
                                {
                                    Bamt = (int)((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OLD_OBON[clsPmpaType.a.Bi] / 100)); //60% //급여본인부담+비급여 본인부담
                                    Jamt = clsPmpaType.RPG.Amt1[i] - (int)((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OLD_OBON[clsPmpaType.a.Bi] / 100));
                                }
                            }
                        }
                    }
                    else if (clsPmpaType.TOM.Bi == "22" && (clsPmpaType.TOM.Jin == "I" || clsPmpaType.TOM.Jin == "J"))
                    {
                        if (i == 17 || i == 18)
                        {
                            if (String.Compare(clsPmpaType.TOM.BDate, "2010-07-01") >= 0 && (clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "V248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250"))
                            {
                                //보호22종 환자는 CT,MRI 5% 본인부담금 2010-07-01
                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                            }

                            else if (String.Compare(clsPmpaType.TOM.BDate, "2010-01-01") >= 0)
                            {
                                switch (clsPmpaPb.GOpd_Sunap_VCode)
                                {
                                    case "V191":
                                    case "V192":
                                    case "V193":
                                    case "V194":
                                        //보호22종 환자는 CT,MRI 5% 본인부담금 2010-01-01
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                        break;

                                    default:
                                        //보호22종 환자는 CT,MRI 15% 본인부담금 2005-11-4
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 15 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 15 / 100));
                                        break;
                                }
                            }

                            else
                            {
                                switch (clsPmpaPb.GOpd_Sunap_VCode)
                                {
                                    case "V193":
                                        //보호22종 환자는 CT,MRI 10% 본인부담금 2005-12-01
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                        break;

                                    default:
                                        //보호22종 환자는 CT,MRI 15% 본인부담금 2005-11-4
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 15 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 15 / 100));
                                        break;
                                }
                            }
                        }

                        else
                        {
                            Jamt = clsPmpaType.RPG.Amt1[i];
                        }
                    }
                    else if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.a.Dept == "NP")
                    {
                        if (i == 7)
                        {
                            if (clsPmpaPb.GstrSPR == "OK")
                            {
                                if (string.Compare(clsPmpaType.TOM.BDate, "2021-04-01") >= 0)
                                {
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 5 / 100);
                                    Bamt += clsPmpaPb.GnNPInjAmt * 5 / 100;

                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 5 / 100);
                                    Jamt -= clsPmpaPb.GnNPInjAmt * 5 / 100;
                                }
                                else
                                {
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 5 / 100);
                                    Bamt += clsPmpaPb.GnNPInjAmt * 10 / 100;

                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 5 / 100);
                                    Jamt -= clsPmpaPb.GnNPInjAmt * 10 / 100;
                                }
                                
                            }
                            else
                            {
                                if (string.Compare(clsPmpaType.TOM.BDate, "2021-04-01") >= 0)
                                {
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 10 / 100);
                                    Bamt += clsPmpaPb.GnNPInjAmt * 5 / 100;

                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 10 / 100);
                                    Jamt -= clsPmpaPb.GnNPInjAmt * 5 / 100;
                                }
                                else
                                {
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 10 / 100);
                                    Bamt += clsPmpaPb.GnNPInjAmt * 10 / 100;

                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnNPInjAmt) * 10 / 100);
                                    Jamt -= clsPmpaPb.GnNPInjAmt * 10 / 100;
                                }
                                  
                            }
                        }
                        else if (i == 17 || i == 18)
                        {
                            Bamt = clsPmpaType.RPG.Amt1[i] * 15 / 100;
                            Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * 15 / 100);
                        }
                        else
                        {
                            if (clsPmpaPb.GstrSPR == "")
                            {
                                Bamt = clsPmpaType.RPG.Amt1[i] * 10 / 100;
                                Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * 10 / 100);
                            }
                            else if (clsPmpaPb.GstrSPR == "OK")
                            {
                                Bamt = clsPmpaType.RPG.Amt1[i] * 5 / 100;
                                Jamt = clsPmpaType.RPG.Amt1[i] - (clsPmpaType.RPG.Amt1[i] * 5 / 100);
                            }
                        }
                    }
                    else
                    {
                        if (String.Compare(clsPmpaType.TOM.BDate, clsPmpaPb.OBON_DATE) >= 0)
                        {
                            //윤2004
                            //2008-04-01 시행

                            //차상위 계층환자 본인부담금 0%
                            if (clsPmpaPb.GOpd_Sunap_MCode == "C000")
                            {
                                if (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13")
                                {
                                    //급여본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                }
                            }

                            else if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") && (clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "V248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250") && String.Compare(clsPmpaType.TOM.BDate, "2010-07-01") >= 0)
                            {
                                //중증화상 5%'급여본인부담
                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                            }

                            else if ((clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13") && (clsPmpaPb.GOpd_Sunap_VCode == "V191" || clsPmpaPb.GOpd_Sunap_VCode == "V192" || clsPmpaPb.GOpd_Sunap_VCode == "V193" || clsPmpaPb.GOpd_Sunap_VCode == "V194") && String.Compare(clsPmpaType.TOM.BDate, "2009-12-01") >= 0)
                            {
                                //중증화상 5%'급여본인부담
                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                            }

                            //윤조연추가 2009-07-01 등록 산정특례 희귀난치성시행
                            else if (String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_MCode == "V000" && (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13"))
                            {
                                //중증화상 5%'급여본인부담(???? copy&paste 한듯. 결핵, 잠복결핵의 경우 본인부담 0% <= 해당 진료건에 한해서...)
                                if (String.Compare(clsPmpaType.TOM.BDate, "2016-07-01") >= 0 && (clsPmpaPb.GOpd_Sunap_VCode == "V000" || clsPmpaPb.GOpd_Sunap_VCode == "V010") && (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13"))
                                {
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                }   
                                else
                                {
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                }
                            }

                            //2008-11-04 추가
                            else if (clsPmpaType.TOM.Age >= 6 && clsPmpaType.TOM.Jin == "C" && clsPmpaPb.GOpd_Sunap_MCode == "H000" && String.Compare(clsPmpaType.TOM.Bi, "13") <= 0)
                            {
                                if (i == 17 || i == 18)
                                {
                                    //윤조연추가 2009-07-01 산정특례 희귀난치성시행
                                    if (String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_MCode == "H000" && (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13"))
                                    {
                                        //산정특례 Ct 10%'급여본인부담
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                    }

                                    else
                                    {
                                        //산정특례 Ct 50%'급여본인부담
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 50 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 50 / 100));
                                    }
                                }

                                else
                                {
                                    //윤조연추가 2009-07-01 산정특례 희귀난치성시행
                                    if (String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_MCode == "H000" && (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13"))
                                    {
                                        //본인부담 10%
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                    }

                                    else
                                    {
                                        //본인부담 20%
                                        Bamt = ((clsPmpaType.RPG.Amt1[i] * 20 / 100));
                                        Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 20 / 100));
                                    }
                                }

                            }

                            else if (clsPmpaType.TOM.Age >= 6 && (clsPmpaType.TOM.Jin == "F" || clsPmpaType.TOM.Jin == "G"))
                            {
                                if (String.Compare(clsPmpaType.TOM.Bi, "24") <= 0)
                                {
                                    if (i == 17 || i == 18)
                                    {
                                        //윤조연추가 2009-07-01 산정특례 희귀난치성시행
                                        if (String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_MCode == "H000" && (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13"))
                                        {
                                            //산정특례 Ct 10%'급여본인부담
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                        }

                                        else
                                        {
                                            //산정특례 Ct 50%'급여본인부담
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 50 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 50 / 100));
                                        }
                                    }

                                    else
                                    {
                                        //윤조연추가 2009-07-01 산정특례 희귀난치성시행
                                        if (String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_MCode == "H000" && (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13"))
                                        {
                                            //본인부담 10%
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                        }

                                        else
                                        {
                                            //본인부담 20%
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 20 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 20 / 100));
                                        }
                                    }
                                }

                                else
                                {
                                    //급여본인부담+비급여 본인부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                }
                            }

                            else if (clsPmpaType.TOM.Age <= 5 && (clsPmpaType.TOM.Jin == "R" || clsPmpaType.TOM.Jin == "S" || clsPmpaType.TOM.Jin == "T" || clsPmpaType.TOM.Jin == "U") && String.Compare(clsPmpaType.TOM.BDate, "2007-08-01") >= 0 && String.Compare(clsPmpaType.TOM.Bi, "13") <= 0)
                            {
                                //윤조연추가 2009-07-01 산정특례 희귀난치성시행
                                if (String.Compare(clsPmpaType.TOM.BDate, "2009-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_MCode == "H000" && (clsPmpaType.TOM.Bi == "11" || clsPmpaType.TOM.Bi == "12" || clsPmpaType.TOM.Bi == "13"))
                                {
                                    //본인부담 10%
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 10 / 100));
                                }
                               

                                else
                                {
                                    //소아(만6세미만) 본인부담율 35% => CT.MRI 35%
                                    if (String.Compare(clsPmpaType.TOM.BDate, "2007-08-01") >= 0 && (clsPmpaType.TOM.Jin == "R" || clsPmpaType.TOM.Jin == "U"))
                                    {
                                        if (String.Compare(clsPmpaType.TOM.BDate, "2019-01-01") >= 0 && clsPmpaType.TOM.Age == 0 )
                                        {
                                            //본인부담 10%
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 15 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 15 / 100));
                                        }
                                        else//급여본인부담+비급여 본인부담
                                        {
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 35 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 35 / 100));

                                        }
                                    }


                                    //소아(만6세미만) 상병특례환자 본인부담율 14%
                                    else if (String.Compare(clsPmpaType.TOM.BDate, "2007-08-01") >= 0 && (clsPmpaType.TOM.Jin == "S" || clsPmpaType.TOM.Jin == "T"))
                                    {
                                        if (i == 17 || i == 18)
                                        {
                                            //CT.MRI 35%
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 35 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 35 / 100));
                                        }

                                        else
                                        {
                                            //급여본인부담+비급여 본인부담
                                            Bamt = ((clsPmpaType.RPG.Amt1[i] * 14 / 100));
                                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 14 / 100));
                                        }

                                    }
                                }
                            }

                            else
                            {
                                //2007-07-01                                        
                                if (clsPmpaType.TOM.Bi == "21" && String.Compare(clsPmpaType.TOM.BDate, "2007-07-01") >= 0 && (i == 17 || i == 18) && clsPmpaPb.GstatEROVER == "*")
                                {
                                    //보호1종 환자 CT,MRI 5% 본인 부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                }

                                else if (clsPmpaType.TOM.Bi == "21" && String.Compare(clsPmpaType.TOM.BDate, "2007-07-01") >= 0 && (i == 17 || i == 18) && clsPmpaPb.GOpd_Sunap_MCode == "M000")
                                {
                                    //보호1종 환자 CT,MRI 5% 본인 부담
                                    Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                    Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                }

                                else
                                {
                                    //2005-09-01
                                    switch (clsPmpaPb.GOpd_Sunap_VCode)
                                    {
                                        case "V193":
                                        case "V194":
                                            if ((String.Compare(clsPmpaType.TOM.Bi, "11") >= 0 && String.Compare(clsPmpaType.TOM.Bi, "13") <= 0) || clsPmpaType.TOM.Bi == "22")
                                            {
                                                //급여본인부담+비급여 본인부담
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));
                                            }

                                            else if (clsPmpaType.TOM.Bi == "21")
                                            {
                                                //급여본인부담+비급여 본인부담
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                            }
                                            break;

                                        case "V247":
                                        case "V248":
                                        case "V249":
                                        case "V250":
                                            if ((String.Compare(clsPmpaType.TOM.Bi, "11") >= 0 && String.Compare(clsPmpaType.TOM.Bi, "13") <= 0 || clsPmpaType.TOM.Bi == "22") && String.Compare(clsPmpaType.TOM.BDate, "2010-07-01") >= 0)
                                            {
                                                //중증화상 5%'급여본인부담
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                            }

                                            else
                                            {
                                                //급여본인부담+비급여 본인부담
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                            }
                                            break;

                                        case "F003":
                                            //약품비만
                                            if (i == 5)
                                            {
                                                //급여본인부담+비급여 본인부담
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnDrugRPAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100);
                                                Bamt += ((clsPmpaPb.GnDrugRPAmt * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));   //약값 GnDrugAmt


                                                Jamt = clsPmpaType.RPG.Amt1[i] - (((clsPmpaType.RPG.Amt1[i] - clsPmpaPb.GnDrugRPAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                Jamt -= ((clsPmpaPb.GnDrugRPAmt * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100));   //약값 GnDrugAmt
                                            }

                                            else
                                            {
                                                //급여본인부담+비급여 본인부담
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                            }
                                            break;

                                        default:
                                            //2013-11-07  응급실 6시간이상 입원의 경우 입원기준으로 출력되어야 하므로  기준을 변경함
                                            if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.Age < 6 && (clsPmpaPb.GstatEROVER == "*" || clsPmpaPb.GOpd_Sunap_JinDtl2 == "E"))
                                            {
                                                Bamt = clsPmpaType.RPG.Amt1[i] * 0 / 100;
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 0 / 100));
                                            }
                                            else if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.Age <= 15 && (clsPmpaPb.GstatEROVER == "*" || clsPmpaPb.GOpd_Sunap_JinDtl2 == "E"))
                                            {
                                                Bamt = clsPmpaType.RPG.Amt1[i] * 3 / 100;
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 3 / 100));
                                            }
                                            else if (String.Compare(clsPmpaType.TOM.ActDate, "2013-11-07") >= 0 && (clsPmpaPb.GstatEROVER == "*" || clsPmpaPb.GOpd_Sunap_JinDtl2 == "E"))
                                            {
                                                //급여본인부담+비급여 본인부담
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100));
                                            }

                                            else if (String.Compare(clsPmpaType.TOM.ActDate, "2019-01-01") >= 0 && clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.Age == 0 )
                                            {
                                                //급여본인부담+비급여 본인부담  22종 5% 적용
                                                Bamt = clsPmpaType.RPG.Amt1[i] * 5 / 100;
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * 5 / 100));
                                            }

                                            else if (String.Compare(clsPmpaType.TOM.ActDate, "2012-06-01") >= 0 && clsPmpaType.TOM.Bi == "22" && (clsPmpaPb.GstatEROVER == "*" || clsPmpaPb.GOpd_Sunap_JinDtl2 == "E"))
                                            {
                                                //2012-06-13
                                                //급여본인부담+비급여 본인부담
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100));

                                            }

                                            else
                                            {
                                                //급여본인부담+비급여 본인부담
                                                Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                                Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100));
                                            }
                                            break;
                                    }
                                }
                            }
                        }

                        else
                        {
                            //급여본인부담+비급여 본인부담
                            Bamt = ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OLD_OBON[clsPmpaType.a.Bi] / 100));
                            Jamt = clsPmpaType.RPG.Amt1[i] - ((clsPmpaType.RPG.Amt1[i] * clsPmpaPb.OLD_OBON[clsPmpaType.a.Bi] / 100));
                        }
                    }

                    #endregion Gensan_Johap_Chk(GoSub) End

                    if ( clsPmpaPb.GnJinRp회신료 > 0 && i == 1) { Jamt += clsPmpaPb.GnJinRp회신료; }// '회송료 공단부담100% } 

                    // '2021-09-16 결핵관리료 공단부담100% 
                    if (clsPmpaPb.GnJinRp재택결핵 > 0 && i == 1)
                    {
                        Jamt += clsPmpaPb.GnJinRp재택결핵;
                    }

                    if (clsPmpaPb.GnJinRp의뢰료 > 0 && i == 1)
                    {
                        Bamt = (int)(clsPmpaType.RPG.Amt1[i] * 30 / 100); // '의뢰료 30% 본부 0세및 기타는 낮은 부담률 적용
                        Jamt = clsPmpaType.RPG.Amt1[i] - (int)(clsPmpaType.RPG.Amt1[i] * 30 / 100);

                    }// '의뢰


                    clsPmpaType.RPG.Amt2[i] = clsPmpaType.RPG.Amt2[i];  //비급여
                    clsPmpaType.RPG.Amt3[i] = clsPmpaType.RPG.Amt3[i];  //특진
                    clsPmpaType.RPG.Amt4[i] = clsPmpaType.RPG.Amt4[i];  //본인총액

                    if ((clsPmpaPb.GOpd_Sunap_MCode == "H000" || clsPmpaPb.GOpd_Sunap_MCode == "F000") && clsPmpaPb.GOpd_Sunap_JinDtl != "02")
                    {
                        Jamt += Bamt;
                        Bamt = 0;
                    }

                    else if (clsPmpaType.TOM.Bi == "21" || clsPmpaType.TOM.Bi == "22")
                    {
                        if (clsPmpaType.TOM.Bi == "21" && String.Compare(clsPmpaType.TOM.BDate, "2007-07-01") >= 0 && (i == 17 || i == 18) && clsPmpaPb.GOpd_Sunap_MCode == "M000" || clsPmpaPb.GOpd_Sunap_JinDtl == "02")
                        {
                            i = i;
                        }
                        else if (clsPmpaType.TOM.Bi == "22" && clsPmpaPb.GOpd_Sunap_JinDtl2 == "E")
                        {
                            i = i;
                        }
                        else if (clsPmpaType.TOM.Bi == "22" )
                        {
                            i = i;
                        }
                        else
                        {
                            Jamt += Bamt;
                            Bamt = 0;
                        }
                    }

                    clsPmpaType.RPG.Amt5[i] = Bamt;                         //본인부담
                    clsPmpaType.RPG.Amt6[i] = Jamt;                         //공단부담
                    clsPmpaType.RPG.Amt7[i] = clsPmpaType.RPG.Amt4[i];      //본인총액

                    //Sub 합
                    clsPmpaType.RPG.Amt1[36] += clsPmpaType.RPG.Amt5[i];    //본인총액
                    clsPmpaType.RPG.Amt1[37] += clsPmpaType.RPG.Amt6[i];    //공단부담
                    clsPmpaType.RPG.Amt1[38] += clsPmpaType.RPG.Amt4[i];    //전액본인
                    clsPmpaType.RPG.Amt1[39] += clsPmpaType.RPG.Amt3[i];    //선택료
                    clsPmpaType.RPG.Amt1[40] += clsPmpaType.RPG.Amt2[i];    //비급여
                }
            }

            //예약진찰 누적
            clsPmpaType.RPG.Amt1[31] += clsPmpaType.RPG.Amt1[23] + clsPmpaType.RPG.Amt3[23];    //-> 기존금액 + 예약비

            clsPmpaType.RPG.Amt1[33] += clsPmpaType.RPG.Amt5[23] + clsPmpaType.RPG.Amt3[23];

            clsPmpaType.RPG.Amt1[36] += clsPmpaType.RPG.Amt5[23];           //본인총액
            clsPmpaType.RPG.Amt1[37] += clsPmpaType.RPG.Amt6[23];           //공단부담
            clsPmpaType.RPG.Amt1[38] += clsPmpaType.RPG.Amt4[23];           //전액본인
            clsPmpaType.RPG.Amt1[39] += clsPmpaType.RPG.Amt3[23];           //선택료
            clsPmpaType.RPG.Amt1[40] += clsPmpaType.RPG.Amt2[23];           //비급여

            //이전 절사액 포함여부
            strChk = "";

            if (clsPmpaType.RPG.Amt1[33] != (VB.Fix(Convert.ToInt32(clsPmpaType.RPG.Amt1[36] / 100)) * 100) && (VB.Fix(Convert.ToInt32(clsPmpaType.RPG.Amt1[36] / 100)) * 100) != 0)
            {
                for (i = 1; i <= 30; i++)
                {
                    clsPmpaType.RPG.Amt2[i] = clsPmpaType.RPG.Amt2[i];      //비급여
                    clsPmpaType.RPG.Amt3[i] = clsPmpaType.RPG.Amt3[i];      //특진
                    clsPmpaType.RPG.Amt4[i] = clsPmpaType.RPG.Amt4[i];      //본인총액

                    clsPmpaType.RPG.Amt5[i] = clsPmpaType.RPG.Amt5[i];      //본인부담
                    clsPmpaType.RPG.Amt6[i] = clsPmpaType.RPG.Amt6[i];      //공단부담
                    //clsPmpaType.RPG.Amt7[i] = clsPmpaType.RPG.Amt4[i];      //본인총액

                    //에약진찰 제외
                    if (i != 23)
                    {
                        if (i > 1 && clsPmpaType.RPG.Amt5[i] > 0 && strChk != "OK" && clsPmpaPb.GnOpd_Sunap_LastDan > 0)
                        {
                            strChk = "OK";

                            clsPmpaType.RPG.Amt5[i] += clsPmpaPb.GnOpd_Sunap_LastDan;   //본인부담
                            clsPmpaType.RPG.Amt6[i] = clsPmpaType.RPG.Amt6[i];

                            //Sub 합
                            clsPmpaType.RPG.Amt1[36] += clsPmpaPb.GnOpd_Sunap_LastDan;  //본인총액
                            clsPmpaType.RPG.Amt1[37] = clsPmpaType.RPG.Amt1[37];        //공단부담
                            clsPmpaType.RPG.Amt1[38] = clsPmpaType.RPG.Amt1[38];        //전액본인
                            clsPmpaType.RPG.Amt1[39] = clsPmpaType.RPG.Amt1[39];        //선택료
                            clsPmpaType.RPG.Amt1[40] = clsPmpaType.RPG.Amt1[40];        //비급여

                            //진료비총액
                            //clsPmpaType.RPG.Amt1[40] += clsPmpaPb.GnOpd_Sunap_LastDan;
                        }
                    }
                }

                //이전절사 있는데 진찰만 있을경우
                if (strChk != "OK" && clsPmpaType.RPG.Amt5[1] > 0 && strChk != "OK" && clsPmpaPb.GnOpd_Sunap_LastDan > 0)
                {
                    strChk = "OK";
                    clsPmpaType.RPG.Amt5[1] += clsPmpaPb.GnOpd_Sunap_LastDan;           //본인부담
                    clsPmpaType.RPG.Amt6[1] = clsPmpaType.RPG.Amt6[1];

                    //Sub 합
                    clsPmpaType.RPG.Amt1[36] += clsPmpaPb.GnOpd_Sunap_LastDan;          //본인총액
                    clsPmpaType.RPG.Amt1[37] = clsPmpaType.RPG.Amt1[37];                //공단부담
                    clsPmpaType.RPG.Amt1[38] = clsPmpaType.RPG.Amt1[38];                //전액본인
                    clsPmpaType.RPG.Amt1[39] = clsPmpaType.RPG.Amt1[39];                //선택료
                    clsPmpaType.RPG.Amt1[40] = clsPmpaType.RPG.Amt1[40];                //비급여

                    //진료비총액
                    clsPmpaType.RPG.Amt1[31] += clsPmpaPb.GnOpd_Sunap_LastDan;
                }
            }

            //건강보험 희귀난치 지원금
            if (String.Compare(clsPmpaType.TOM.Bi, "14") < 0 && clsPmpaPb.GOpd_Sunap_MCode == "H000")
            {
                clsPmpaType.RPG.Amt1[37] += clsPmpaType.RPG.Amt1[36];
                clsPmpaType.RPG.Amt1[36] -= clsPmpaType.RPG.Amt1[36];
            }

            //차상위2종 정액 처리
            else if (String.Compare(clsPmpaType.TOM.Bi, "14") < 0 && clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000" && (clsPmpaType.TOM.Jin == "I" || clsPmpaType.TOM.Jin == "J"))
            {
                if (clsPmpaPb.GOpd_Sunap_VCode == "V206" || clsPmpaPb.GOpd_Sunap_VCode == "V231" || clsPmpaPb.GOpd_Sunap_VCode == "V246")
                {
                    if (clsPmpaPb.GOpd_Sunap_MCode == "E000")
                    {
                        if (clsPmpaType.TOM.Jin == "I")
                        {
                            clsPmpaType.RPG.Amt1[36] = Convert.ToInt32((clsPmpaType.RPG.Amt1[36] + 1500) * 50 / 100);
                            clsPmpaType.RPG.Amt1[37] += clsPmpaType.RPG.Amt1[36] - 1500;
                        }

                        else if (clsPmpaType.TOM.Jin == "J")
                        {
                            clsPmpaType.RPG.Amt1[36] = Convert.ToInt32((clsPmpaType.RPG.Amt1[36] + 1000) * 50 / 100);
                            clsPmpaType.RPG.Amt1[37] += clsPmpaType.RPG.Amt1[36] - 1000;
                        }
                    }

                    else if (clsPmpaType.TOM.MCode == "F000")
                    {

                    }
                }

                else if (clsPmpaPb.GOpd_Sunap_MCode == "E000")
                {
                    if (clsPmpaType.TOM.Jin == "I")
                    {
                        clsPmpaType.RPG.Amt1[36] = 1500;
                        clsPmpaType.RPG.Amt1[37] -= 1500;
                    }
                    else if (clsPmpaType.TOM.Jin == "J")
                    {
                        clsPmpaType.RPG.Amt1[36] = 1000;
                        clsPmpaType.RPG.Amt1[37] -= 1000;
                    }
                }

                else if (clsPmpaPb.GOpd_Sunap_MCode == "F000")
                {

                }
            }

            //차상위2종 정액 처리
            else if (String.Compare(clsPmpaType.TOM.Bi, "14") < 0 && clsPmpaType.TOM.MCode == "F000")
            {

            }

            else if (clsPmpaType.TOM.Bi == "21")
            {
                clsPmpaType.RPG.Amt1[33] += clsPmpaType.RPG.Amt1[35];
                clsPmpaType.RPG.Amt1[36] += clsPmpaPb.GOpd_Sunap_Boamt + clsPmpaType.RPG.Amt1[35];
                clsPmpaType.RPG.Amt1[37] -= clsPmpaPb.GOpd_Sunap_Boamt;
            }

            else if (clsPmpaType.TOM.Bi == "22")
            {
                if (clsPmpaType.TOM.Bohun == "3")
                {
                    clsPmpaType.RPG.Amt1[37] += clsPmpaType.RPG.Amt1[36];
                    clsPmpaType.RPG.Amt1[36] -= clsPmpaType.RPG.Amt1[36];
                }
            }

        }
        /// <summary>
        /// author : 안정수
        /// Create Date : 2017-10-12
        /// <seealso cref="vbfunc.bas : 상한제_마감일자(ArgDate As String)"/>
        /// </summary>
        /// <param name="ArgDate"></param>
        /// <returns></returns>
        public string SangHan_MagamDay(PsmhDb pDbCon, string ArgDate)
        {
            ComFunc CF = new ComFunc();

            string strStartDate = "";
            string strEndDate = "";
            string strDD = "";
            string strMM = "";
            string strEndMM = "";
            string strEndYear = "";

            string rtnVal = "";

            if (String.Compare(ArgDate, "2004-06-30") <= 0)
            {
                ArgDate = "2004-07-01";
            }

            if (String.Compare(ArgDate, "2009-01-01") >= 0)
            {
                strStartDate = VB.Left(ArgDate, 10);
                strMM = VB.Mid(strStartDate, 6, 2);
                strDD = VB.Right(strStartDate, 2);
                strEndYear = VB.Left(strStartDate, 4);
                strEndDate = strEndYear + "-12-31";
            }

            else if (String.Compare(ArgDate, "2008-12-31") <= 0)
            {
                strStartDate = VB.Left(ArgDate, 10);
                strMM = VB.Mid(strStartDate, 6, 2);
                strDD = VB.Right(strStartDate, 2);
                strEndYear = VB.Left(strStartDate, 4);
                strEndMM = ComFunc.SetAutoZero((Convert.ToInt32(strMM) + 6).ToString(), 2);

                if (Convert.ToInt32(strEndMM) >= 13)
                {
                    strEndMM = ComFunc.SetAutoZero((Convert.ToInt32(strEndMM) - 12).ToString(), 2);
                    strEndYear = (Convert.ToInt32(strEndYear) + 1).ToString();
                }

                if (strDD == "01")
                {
                    strEndDate = Convert.ToDateTime(strEndYear + "-" + strEndMM + "-" + strDD).AddDays(-1).ToShortDateString();
                }

                else
                {
                    strEndDate = strEndYear + "-" + strEndMM + "-" + strDD;
                    if ((String.Compare((strEndMM + strDD), "0229") >= 0 && String.Compare((strEndMM + strDD), "0231") <= 0) || String.Compare((strEndMM + strDD), "0431") == 0 || String.Compare((strEndMM + strDD), "0631") == 0 || String.Compare((strEndMM + strDD), "0931") == 0 || String.Compare((strEndMM + strDD), "1131") == 0)
                    {
                        strEndDate = CF.READ_LASTDAY(pDbCon, strEndYear + "-" + strEndMM + "-01");
                    }
                    else
                    {
                        strEndDate = Convert.ToDateTime(strEndDate).AddDays(-1).ToShortDateString();
                    }
                }
            }

            rtnVal = strEndDate;

            return rtnVal;
        }

        /// <summary>
        /// Description : 환자정보를 읽어 TBP 구조체 변수에 삽입.
        /// author : 안정수
        /// Create Date : 2017-10-12
        /// <seealso cref="Jengsan01.bas : READ_BAS_PATIENT2"/>
        /// </summary>
        /// <param name="ArgStr"></param>
        /// <param name="Gubun"></param>
        /// 1. 등록번호
        /// 2. ROWID
        /// <returns></returns>
        public string READ_BAS_PATIENT2(PsmhDb pDbCon, string ArgStr, string Gubun)
        {
            string rtnVal = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  Pano vPano,Sname vSname,Jumin1 vJumin1,";
            SQL += ComNum.VBLF + "  Jumin2 vJumin2,Sex vSex,Bi vBi,Pname vPname,";
            SQL += ComNum.VBLF + "  Kiho vKiho,GKiho vGKiho,Remark vRemark,";
            SQL += ComNum.VBLF + "  Gwange vGwange,Jumin3 vJumin3 ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_Patient";
            SQL += ComNum.VBLF + "WHERE 1=1";
            if (Gubun == "1")
            {
                SQL += ComNum.VBLF + "      AND Pano = '" + ArgStr + "'";
            }
            else if (Gubun == "2")
            {
                SQL += ComNum.VBLF + "      AND ROWID = '" + ArgStr + "'";
            }

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    rtnVal = "NO";

                    clsPmpaType.TBP.Ptno = "";
                    clsPmpaType.TBP.Sname = "";
                    clsPmpaType.TBP.Bi = "";
                    clsPmpaType.TBP.Jumin1 = "";
                    clsPmpaType.TBP.Jumin2 = "";
                    clsPmpaType.TBP.Sex = "";
                    clsPmpaType.TBP.PName = "";
                    clsPmpaType.TBP.Kiho = "";
                    clsPmpaType.TBP.GKiho = "";
                    clsPmpaType.TBP.Remark = "";
                    clsPmpaType.TBP.Gwange = "";
                }

                if (dt.Rows.Count > 0)
                {
                    clsPmpaType.TBP.Ptno = dt.Rows[0]["vPano"].ToString().Trim();
                    clsPmpaType.TBP.Sname = dt.Rows[0]["vSname"].ToString().Trim();
                    clsPmpaType.TBP.Bi = dt.Rows[0]["vBi"].ToString().Trim();
                    clsPmpaType.TBP.Jumin1 = dt.Rows[0]["vJumin1"].ToString().Trim();
                    clsPmpaType.TBP.Jumin2 = dt.Rows[0]["vJumin3"].ToString().Trim();
                    clsPmpaType.TBP.Sex = dt.Rows[0]["vSex"].ToString().Trim();
                    clsPmpaType.TBP.PName = dt.Rows[0]["vPname"].ToString().Trim();
                    clsPmpaType.TBP.Kiho = dt.Rows[0]["vKiho"].ToString().Trim();
                    clsPmpaType.TBP.GKiho = dt.Rows[0]["vGKiho"].ToString().Trim();
                    clsPmpaType.TBP.Remark = dt.Rows[0]["vRemark"].ToString().Trim();
                    clsPmpaType.TBP.Gwange = dt.Rows[0]["vGwange"].ToString().Trim();
                    rtnVal = "OK";
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Read_Bas_Patient
        /// </summary>
        /// <param name="strPano"></param>
        /// <returns></returns>
        public string Read_Bas_Patient(PsmhDb pDbCon, string strPano)
        {
            string strVal = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (strPano == "")
            {
                return strVal;
            }

            strPano = Convert.ToInt32(strPano).ToString("00000000");

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                ";
                SQL += ComNum.VBLF + "  Sname";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "  AND Pano = '" + strPano + "'        ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장

                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    strVal = "";

                }
                else
                {
                    strVal = dt.Rows[0]["Sname"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                return strVal;

            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strVal;
            }

        }

        /// <summary>
        /// Description : 영문명변환
        /// author : 박병규
        /// Create Date : 2017-10-18
        /// <param name="ArgSname"></param>
        /// <seealso cref="OUMSAD.bas : HanName_TO_EngName"/>
        /// </summary>
        public string HanName_TO_EngName(PsmhDb pDbCon, string ArgSname)
        {
            int i = 0;
            string strData = "";
            string strCode = "";
            string rtnVal = "";

            for (i = 1; i <= VB.Len(ArgSname); i++)
                if (VB.Mid(ArgSname, i, 1) != " ")
                    strData += VB.Mid(ArgSname, i, 1);

            strData = VB.Left(strData + VB.Space(10), 10);

            for (i = 1; i <= 5; i++)
            {
                strCode = VB.Mid(strData, i, 1);
                if (strCode != "")
                    rtnVal += READ_EngName(pDbCon, strCode) + " ";
            }

            //'GIM(김) -> KIM으로 변환
            if (VB.Left(rtnVal, 4) == "GIM ")

            { rtnVal = "KIM " + VB.Right(rtnVal, VB.Len(rtnVal) - 4); }

            //'GO(고) -> KO으로 변환

            if (VB.Left(rtnVal, 3) == "GO ")
            { rtnVal = "KO " + VB.Right(rtnVal, VB.Len(rtnVal) - 3);  }


            //'GU(구) -> KOO으로 변환
            if (VB.Left(rtnVal, 3) == "GU ")
            { rtnVal = "KOO " + VB.Right(rtnVal, VB.Len(rtnVal) - 3); }


            //'GI(기) -> KI으로 변환
            if (VB.Left(rtnVal, 3) == "GI ")
            { rtnVal = "KI " + VB.Right(rtnVal, VB.Len(rtnVal) - 3); }


            //'GA(가) -> KA으로 변환
            if (VB.Left(rtnVal, 3) == "GA ")
            { rtnVal = "KA " + VB.Right(rtnVal, VB.Len(rtnVal) - 3); }


            //'GONG(공) -> KONG으로 변환
            if (VB.Left(rtnVal, 5) == "GONG ")
            { rtnVal = "KONG " + VB.Right(rtnVal, VB.Len(rtnVal) - 5); }



            //'GANG(공) -> KANG으로 변환
            if (VB.Left(rtnVal, 5) == "GANG ")
            { rtnVal = "KANG " + VB.Right(rtnVal, VB.Len(rtnVal) - 5); }


            if (VB.Left(rtnVal, 2) == "쿠 ")
            { rtnVal = "KU " + VB.Right(rtnVal, VB.Len(rtnVal) - 2); }
          

            return rtnVal;
        }

        /// <summary>
        /// Description : BAS_Z300FONT에서 한글을 영문으로 변환(성)
        /// author : 박병규
        /// Create Date : 2017-10-18
        /// <param name="str"></param>
        /// <seealso cref="OUMSAD.bas : READ_EngName"/>
        /// </summary>
        public string READ_EngName(PsmhDb pDbCon, string str)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT EngName ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_Z300FONT ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND Z300Code = '" + str + "' ";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("한글을 영문으로 변환시 오류발생");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
                rtnVal = dt.Rows[0]["EngName"].ToString().Trim();
            else
                rtnVal = "";

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 플루예방접종 예약내역 조회
        /// author : 박병규
        /// Create Date : 2017-10-19
        /// <param name="ArgJumin"></param>
        /// <param name="ArgGbn">1.메세지표시, 2.예약날짜축약, 기타.예약날짜</param>
        /// <seealso cref="vbfunc.bas : READ_FLUE_RESERVED"/>
        /// </summary>
        public string READ_FLUE_RESERVED(PsmhDb pDbCon, string ArgJumin, string ArgGbn = "")
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strHash = string.Empty;
            string rtnVal = string.Empty;

            rtnVal = "";
            strHash = clsAES.AES(ArgJumin.Replace("-", ""));

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(RDATE,'YYYY-MM-DD') RDATE1, ";
            SQL += ComNum.VBLF + "        TO_CHAR(RDATE,'YY/MM/DD') RDATE2, ";
            SQL += ComNum.VBLF + "        TO_CHAR(RDATE2,'YYYY-MM-DD') RDATE_TO, GBN ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "VACCINE_RESERVED            --플루예약현황테이블";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND (JUMIN = '" + ArgJumin.Replace("-", "") + "' or JUMIN3 = '" + strHash + "')  ";
            SQL += ComNum.VBLF + "    AND (VDATE IS NULL OR VDATE = TRUNC(SYSDATE) ) ";
            SQL += ComNum.VBLF + "    AND CANDATE IS NULL ";
            SQL += ComNum.VBLF + "    AND (RDATE >= TRUNC(SYSDATE - 14)  or  RDATE2 >= TRUNC(SYSDATE - 14) ) ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("플루예방접종 예약내역 조회오류");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                if (ArgGbn == "1")
                {
                    if (DtFunc.Rows[0]["RATE_TO"].ToString().Trim() != "")
                    {
                        if (string.Compare(DtFunc.Rows[0]["RATE_TO"].ToString().Trim(), clsPublic.GstrSysDate) > 0)
                        {
                            rtnVal = "신종플루예방접종" + '\r';
                            rtnVal += "구  분 : " + DtFunc.Rows[0]["GBN"].ToString().Trim() + '\r';
                            rtnVal += "예약일 : " + DtFunc.Rows[0]["RDATE1"].ToString().Trim() + (DtFunc.Rows[0]["RATE_TO"].ToString().Trim() != "" ? "~" + DtFunc.Rows[0]["RATE_TO"].ToString().Trim() : "");
                        }
                        else
                        {
                            rtnVal = "신종플루예방접종(일자경과)" + '\r';
                            rtnVal += "구  분 : " + DtFunc.Rows[0]["GBN"].ToString().Trim() + '\r';
                            rtnVal += "예약일 : " + DtFunc.Rows[0]["RDATE1"].ToString().Trim() + (DtFunc.Rows[0]["RATE_TO"].ToString().Trim() != "" ? "~" + DtFunc.Rows[0]["RATE_TO"].ToString().Trim() : "");
                        }
                    }
                    else
                    {
                        if (string.Compare(DtFunc.Rows[0]["RDATE1"].ToString().Trim(), clsPublic.GstrSysDate) > 0)
                        {
                            rtnVal = "신종플루예방접종" + '\r';
                            rtnVal += "구  분 : " + DtFunc.Rows[0]["GBN"].ToString().Trim() + '\r';
                            rtnVal += "예약일 : " + DtFunc.Rows[0]["RDATE1"].ToString().Trim() + (DtFunc.Rows[0]["RATE_TO"].ToString().Trim() != "" ? "~" + DtFunc.Rows[0]["RATE_TO"].ToString().Trim() : "");
                        }
                        else
                        {
                            rtnVal = "신종플루예방접종(일자경과)" + '\r';
                            rtnVal += "구  분 : " + DtFunc.Rows[0]["GBN"].ToString().Trim() + '\r';
                            rtnVal += "예약일 : " + DtFunc.Rows[0]["RDATE1"].ToString().Trim() + (DtFunc.Rows[0]["RATE_TO"].ToString().Trim() != "" ? "~" + DtFunc.Rows[0]["RATE_TO"].ToString().Trim() : "");
                        }
                    }
                }
                else if (ArgGbn == "2")
                {
                    rtnVal = DtFunc.Rows[0]["RDATE2"].ToString().Trim();
                }
                else
                {
                    rtnVal = DtFunc.Rows[0]["RDATE1"].ToString().Trim();
                }
            }
            else
            {
                rtnVal = "";
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 직원 및 직계감액조회
        /// author : 박병규
        /// Create Date : 2017-10-25
        /// <param name="ArgPtno"></param>
        /// <seealso cref="OPDACCT.bas : Gam_Pano_Search"/>
        /// </summary>
        public string READ_GAMEKF(PsmhDb pDbCon, string ArgGam, string ArgJumin1, string ArgJumin2, string ArgBdate)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT gamjumin ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMF ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND GAMJUMIN3 = '" + clsAES.AES(ArgJumin1 + ArgJumin2) + "' ";
            SQL += ComNum.VBLF + "    AND GAMCODE   = '" + ArgGam + "'";
            SQL += ComNum.VBLF + "    AND (GAMEND   >= TO_DATE('" + ArgBdate + "','YYYY-MM-DD') OR GAMEND IS NULL) ";
            SQL += ComNum.VBLF + "  union all ";
            SQL += ComNum.VBLF + " SELECT code ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN     = '원무강제퇴사자감액' ";
            SQL += ComNum.VBLF + "    AND TRIM(CODE) = '" + ArgJumin1 + ArgJumin2 + "' ";
            SQL += ComNum.VBLF + " ORDER BY 1  DESC  ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("직원 및 직계감액조회오류");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = "OK";
            else
                rtnVal = "";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : YYYY-MM-DD => YYYYMMDD
        /// author : 박병규
        /// Create Date : 2017-10-25
        /// <param name="ArgPtno"></param>
        /// <seealso cref="vbfunc.bas : DATE_TO_YYMMDD"/>
        /// </summary>
        public string DATE_TO_YYMMDD(string ArgDate)
        {
            string rtnVal = string.Empty;

            if (ArgDate.Length == 10)
                rtnVal = VB.Left(ArgDate, 4) + VB.Mid(ArgDate, 6, 2) + VB.Right(ArgDate, 2);
            else
                rtnVal = "";

            return rtnVal;
        }

        /// <summary>
        /// Description : 포스코 위탁검사 미수관리 번호 생성
        /// author : 김민철
        /// Create Date : 2017-10-25
        /// </summary>        
        public long READ_New_Mir_Posco_Wrtno(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";
            long nWRTNO = 0;

            try
            {
                SQL = "SELECT " + ComNum.DB_PMPA + "SEQ_misu_posco.NEXTVAL pWRTNO FROM DUAL";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return nWRTNO;
                }
                if (Dt.Rows.Count == 0)
                {
                    Dt.Dispose();
                    Dt = null;
                    return nWRTNO;
                }
                else
                {
                    nWRTNO = Convert.ToInt64(Dt.Rows[0]["pWRTNO"].ToString());
                }

                Dt.Dispose();
                Dt = null;

                return nWRTNO;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return nWRTNO;
            }

        }

        /// <summary>
        /// Description : MISU_IDMST 테이블 Insert
        /// Author : 김민철
        /// Create Date : 2017.10.27
        /// </summary>
        /// <param name="Arg"></param>
        /// <param name="Dt"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string Ins_MisuIdMst(string[] Arg, PsmhDb pDbCon, ref int intRowAffected)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "MISU_IDMST (                                   ";
            SQL += ComNum.VBLF + "        WRTNO,MISUID,BDATE,CLASS,IPDOPD,BI,GELCODE,BUN,FROMDATE,TODATE,ILSU,      ";
            SQL += ComNum.VBLF + "        DEPTCODE,MGRRANK,QTY1,QTY2,QTY3,QTY4,AMT1,AMT2,AMT3,AMT4,AMT5,AMT6,AMT7,  ";
            SQL += ComNum.VBLF + "        GBEND,REMARK,JEPSUNO,ENTDATE,ENTPART,MIRYYMM,CHASU,MUKNO,TONGGBN,DRCODE,  ";
            SQL += ComNum.VBLF + "        TDATE,JDATE,CARNO,DRIVER,COPNAME,AMT8,GUBUN,EDIMIRNO )                    ";
            SQL += ComNum.VBLF + " VALUES (                                                                         ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmMisuIdMst.WRTNO] + ",                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.MISUID] + "',                       ";
            SQL += ComNum.VBLF + " TO_DATE('" + Arg[(int)clsPmpaPb.enmMisuIdMst.BDATE] + "','YYYY-MM-DD'),          ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.CLASS] + "',                        ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.IPDOPD] + "',                       ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.BI] + "',                           ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.GELCODE] + "',                      ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmMisuIdMst.BUN] + ",                           ";
            SQL += ComNum.VBLF + " TO_DATE('" + Arg[(int)clsPmpaPb.enmMisuIdMst.FROMDATE] + "','YYYY-MM-DD'),       ";
            SQL += ComNum.VBLF + " TO_DATE('" + Arg[(int)clsPmpaPb.enmMisuIdMst.TODATE] + "','YYYY-MM-DD'),         ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmMisuIdMst.ILSU] + ",                          ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.DEPTCODE] + "',                     ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.MGRRANK] + "',                      ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.QTY1] + "',                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.QTY2] + "',                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.QTY3] + "',                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.QTY4] + "',                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.AMT1] + "',                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.AMT2] + "',                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.AMT3] + "',                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.AMT4] + "',                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.AMT5] + "',                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.AMT6] + "',                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.AMT7] + "',                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.GBEND] + "',                        ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.REMARK] + "',                       ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.JEPSUNO] + "',                      ";
            SQL += ComNum.VBLF + " SYSDATE,                                                                         ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmMisuIdMst.ENTPART] + ",                       ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.MIRYYMM] + "',                      ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.CHASU] + "',                        ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.MUKNO] + "',                        ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.TONGGBN] + "',                      ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.DRCODE] + "',                       ";
            SQL += ComNum.VBLF + " TO_DATE('" + Arg[(int)clsPmpaPb.enmMisuIdMst.TDATE] + "','YYYY-MM-DD'),          ";
            SQL += ComNum.VBLF + " TO_DATE('" + Arg[(int)clsPmpaPb.enmMisuIdMst.JDATE] + "','YYYY-MM-DD'),          ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.CARNO] + "',                        ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.DRIVER] + "',                       ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.COPNAME] + "',                      ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.AMT8] + "',                          ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.GUBUN] + "',                        ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuIdMst.EDIMIRNO] + "'                      ";
            SQL += ComNum.VBLF + "        )                                                                         ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// Description : MISU_SLIP 테이블 Insert
        /// Author : 김민철
        /// Create Date : 2017.10.27
        /// </summary>
        /// <param name="Arg"></param>
        /// <param name="Dt"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string Ins_MisuSlip(string[] Arg, PsmhDb pDbCon, ref int intRowAffected)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "MISU_SLIP (                                    ";
            SQL += ComNum.VBLF + "        WRTNO,MISUID,BDATE,GELCODE,IPDOPD,CLASS,GUBUN,QTY,TAMT,AMT,REMARK,ENTDATE,";
            SQL += ComNum.VBLF + "        ENTPART,CHASU )                                                           ";
            SQL += ComNum.VBLF + " VALUES (                                                                         ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmMisuSlip.WRTNO] + ",                          ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuSlip.MISUID] + "',                        ";
            SQL += ComNum.VBLF + " TO_DATE('" + Arg[(int)clsPmpaPb.enmMisuSlip.BDATE] + "','YYYY-MM-DD'),           ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuSlip.GELCODE] + "',                       ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuSlip.IPDOPD] + "',                        ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuSlip.CLASS] + "',                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuSlip.GUBUN] + "',                         ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuSlip.QTY] + "',                           ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuSlip.TAMT] + "',                          ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuSlip.AMT] + "',                           ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuSlip.REMARK] + "',                        ";
            SQL += ComNum.VBLF + " SYSDATE,                                                                         ";
            SQL += ComNum.VBLF + "          " + Arg[(int)clsPmpaPb.enmMisuSlip.ENTPART] + ",                        ";
            SQL += ComNum.VBLF + "         '" + Arg[(int)clsPmpaPb.enmMisuSlip.CHASU] + "'                          ";
            SQL += ComNum.VBLF + "        )                                                                         ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        /// <summary>
        /// Description : MISU_IDMST WRTNO 값 가져오기
        /// Author : 김민철
        /// Create Date : 2017.10.27
        /// </summary>
        public long Read_Misu_IdMst_WRTNO(PsmhDb pDbCon)
        {
            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            long nWRTNO = 0;

            try
            {
                SQL = " SELECT MAX(WRTNO) cMaxNO FROM " + ComNum.DB_PMPA + "MISU_IDMST ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return nWRTNO;
                }
                if (Dt.Rows.Count == 0)
                {
                    nWRTNO = 1;
                }
                else
                {
                    nWRTNO = Convert.ToInt64(Dt.Rows[0]["cMaxNO"].ToString()) + 1;
                }

                Dt.Dispose();
                Dt = null;

                return nWRTNO;

            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return 0;
            }
        }

        /// <summary>
        /// 2017-10-30 김민철, 콤보박스 아이템(조회년월 YYYYMM)을 추가
        /// </summary>
        /// <param name="ArgCombo"></param>
        /// <param name="ArgMonthCNT"></param>
        public void cboYYYYMM_Set(ComboBox ArgCombo, int ArgMonthCNT)
        {
            int i = 0;
            int ArgYY = 0;
            int ArgMM = 0;
            string strYY = string.Empty;
            string strMM = string.Empty;

            ArgYY = Convert.ToInt16(DateTime.Now.ToString("yyyy"));
            ArgMM = Convert.ToInt16(DateTime.Now.ToString("MM"));
            ArgCombo.Items.Clear();

            for (i = 1; i < ArgMonthCNT; i++)
            {
                strMM = VB.Format(ArgMM, "00");
                ArgCombo.Items.Add(ArgYY + strMM);
                ArgMM -= 1;
                if (ArgMM == 0)
                {
                    ArgMM = 12;
                    ArgYY -= 1;
                }
            }

            ArgCombo.SelectedIndex = 0;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2017.11.03
        /// </summary>
        /// <param name="ArgDate">수진일자</param>
        /// <param name="ArgDept">진료과코드</param>
        /// <param name="ArgDr">의사코드</param>
        /// <param name="ArgChojae">초재구분</param>
        /// <param name="ArgTime"></param>
        /// <param name="ArgSch">스케줄무시여부</param>
        /// <seealso cref=""/>
        public bool READ_FM_RESERVED_CHECK(PsmhDb pDbCon, string ArgDate, string ArgDept, string ArgDr, string ArgChojae, string ArgTime, string ArgSch)
        {
            clsPmpaQuery CPQ = new clsPmpaQuery();

            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;
            int nPCNT_A = 0;  //당일 진료환자 수(오전)
            int nPCNT_P = 0;  //당일 진료환자 수(오후)

            ComFunc.ReadSysDate(pDbCon);
            CPQ.READ_FM_CHOJAE_INWON(pDbCon, ArgDate, ArgDept, ArgDr, ArgSch);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT COUNT(PANO) PCNT, TO_CHAR(JTIME,'YYYY-MM-DD HH24:MI') JTime";
            SQL += ComNum.VBLF + "  From " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + " Where 1          = 1 ";
            SQL += ComNum.VBLF + "   AND BDate      = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND DEPTCODE   = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "   AND DRCODE     = '" + ArgDr + "' ";
            SQL += ComNum.VBLF + "   AND GWACHOJAE  = '" + ArgChojae + "' ";
            SQL += ComNum.VBLF + "   AND JIN NOT IN ('4','7','D') "; //진단서, 진단서 재발급, 건진접수는 제외
            SQL += ComNum.VBLF + " GROUP By JTime ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                for (int i = 0; i < DtFunc.Rows.Count; i++)
                {
                    if (String.Compare(VB.Right(DtFunc.Rows[i]["JTime"].ToString().Trim(), 5), "12:30") <= 0)
                        nPCNT_A += Convert.ToInt32(DtFunc.Rows[i]["PCNT"].ToString().Trim());
                    else
                        nPCNT_P += Convert.ToInt32(DtFunc.Rows[i]["PCNT"].ToString().Trim());
                }
            }

            DtFunc.Dispose();
            DtFunc = null;

            if (ArgChojae == "C")
            {
                if (String.Compare(ArgTime, "12:30") <= 0)
                    if (clsPublic.GnChoInWon_A > nPCNT_A)
                    {
                        rtnVal = true;
                    }
                    else
                    if (clsPublic.GnChoInWon_P > nPCNT_P)
                    {
                        rtnVal = true;
                    }
            }
            else if (ArgChojae == "J")
            {
                if (String.Compare(ArgTime, "12:30") <= 0)
                    if (clsPublic.GnJaeInWon_A > nPCNT_A)
                    {
                        rtnVal = true;
                    }
                    else
                    if (clsPublic.GnJaeInWon_P > nPCNT_P)
                    {
                        rtnVal = true;
                    }
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2017.11.03
        /// </summary>
        /// <param name="ArgDept">진료과코드</param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDr">의사코드</param>
        /// <seealso cref="READ_GWA_JIN"/>
        public string READ_GWA_JIN(PsmhDb pDbCon, string ArgDept, string ArgPtno, string ArgDr)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "C";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND DrCODE    = '" + ArgDr + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = "J";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// 김효성
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgIO"></param>
        /// <param name="ArgBi"></param>
        /// <param name="argDATE"></param>
        /// <param name="ArgVcode"></param>
        /// <returns></returns>
        public string READ_BonRate(PsmhDb pDbCon, string ArgIO, string ArgBi, string argDATE, string ArgVcode = "")
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string strRtn = "";

            SQL = "";
            SQL = "SELECT RateValue FROM KOSMOS_PMPA.BAS_ACCOUNT ";
            SQL = SQL + ComNum.VBLF + "WHERE 1=1";
            if (ArgIO == "IPD")
                SQL = SQL + ComNum.VBLF + "AND IDName='IPD_BON' ";
            else
                SQL = SQL + ComNum.VBLF + "AND IDName='OPD_BON' ";
            SQL = SQL + ComNum.VBLF + " AND ArrayClass=" + ArgBi + " ";
            SQL = SQL + ComNum.VBLF + " AND StartDate<=TO_DATE('" + argDATE + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "ORDER BY StartDate DESC ";

            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return strRtn;
            }

            if (DtFunc.Rows.Count > 0)
            {
                if (string.Compare(ArgBi, "30") < 0 && (ArgVcode == "V193" || ArgVcode == "V194"))
                {
                    strRtn = "10";
                }
                else
                    strRtn = DtFunc.Rows[0]["RateValue"].ToString().Trim();
            }
            else
                strRtn = "-1";

            return strRtn;
        }

        /// <summary>
        /// 김효성
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgBi"></param>
        /// <param name="argDATE"></param>
        /// <returns></returns>
        public int READ_RateGasan(PsmhDb pDbCon, string ArgBi, string argDATE)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            int intRtn = 0;

            switch (ArgBi)
            {
                case "11":
                case "12":
                case "13":
                case "14":
                case "15":
                case "16":
                case "17":
                case "18":
                case "19":
                    intRtn = 1; //보험
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
                    intRtn = 2; //보호
                    break;
                case "31":
                case "32":
                case "33":
                case "34":
                case "35":
                case "36":
                case "37":
                case "38":
                case "39":
                    intRtn = 3; //산재
                    break;
                case "41":
                case "42":
                case "43":
                case "44":
                case "45":
                case "46":
                case "47":
                case "48":
                case "49":
                    intRtn = 4; //보험100%
                    break;
                case "52":
                    intRtn = 6; //자보
                    break;
                default:
                    intRtn = 5; //일반
                    break;
            }
            SQL = "";
            SQL = "SELECT RateValue FROM KOSMOS_PMPA.BAS_ACCOUNT ";
            SQL = SQL + ComNum.VBLF + "WHERE IDName='GISUL' ";
            SQL = SQL + ComNum.VBLF + " AND ArrayClass=" + intRtn + " ";
            SQL = SQL + ComNum.VBLF + " AND StartDate<=TO_DATE('" + argDATE + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "ORDER BY StartDate DESC ";

            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return intRtn;
            }

            if (DtFunc.Rows.Count > 0)
            {

                intRtn = Convert.ToInt32(DtFunc.Rows[0]["RateValue"].ToString().Trim());
            }
            else
                intRtn = -1;

            return intRtn;
        }

        /// <summary>
        /// Description : 접수전 해당일 OPD_MASTER 체크
        /// Author : 박병규
        /// Create Date : 2017.11.08
        /// </summary>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept">의사코드</param>
        /// <param name="ArgActdate">의사코드</param>
        /// <param name="ArgBdate">의사코드</param>
        /// <seealso cref="OUMSAD.BAS:Read_TODAY_OPD_MASTER_CHK"/>
        public string CHECK_TODAY_OPD_MASTER(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgActdate, string ArgBdate)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Pano, ";
            SQL += ComNum.VBLF + "        TO_CHAR(ActDate,'YYYY-MM-DD') ActDate, ";
            SQL += ComNum.VBLF + "        TO_CHAR(BDate,'YYYY-MM-DD') BDate ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + ArgBdate + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND ActDate   = TO_DATE('" + ArgActdate + "', 'YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND Jin NOT IN ( 'E' )  ";  //전화접수 제외(전화예약하면 E되고 접수하면 H됨)
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                if (DtFunc.Rows[0]["PANO"].ToString().Trim() == ArgPtno)
                    rtnVal = "OK";
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 접수구분 I,J 확인
        /// Author : 박병규
        /// Create Date : 2017.11.09
        /// </summary>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept">의사코드</param>
        public int CHECK_JIN_IJ(PsmhDb pDbCon, string ArgPtno, string ArgDept)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            int rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUCODE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PTNO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                for (int i = 0; i < DtFunc.Rows.Count; i++)
                {
                    switch (DtFunc.Rows[i]["SUCODE"].ToString().Trim())
                    {
                        case "@V001":
                        case "@V003":
                        case "@V005":
                        case "@V117":
                        case "@V027":
                        case "@V009":
                        case "@V012":
                        case "@V013":
                        case "@V014":
                        case "@V015":
                        case "@V193":
                        case "@V194":
                            rtnVal = 1;
                            break;
                    }
                }
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 진찰료발생유무(접수||, 진단서발급, 대리접수, 전화접수, 결핵쿠폰접수, 일반건진
        /// Author : 박병규
        /// Create Date : 2017.11.09
        /// </summary>
        /// <param name="ArgJin"></param>
        public bool CHECK_JIN_YN(string ArgJin)
        {
            bool rtnVal = true;

            //if (VB.Left(clsType.User.BuseCode.Trim(), 4) != "0774" && VB.Left(clsType.User.BuseCode.Trim(), 4) != "0445" && clsType.User.IdNumber != "4349" && clsType.User.IdNumber != "2222" && clsType.User.IdNumber != "222")
    
                if (VB.Left(clsType.User.BuseCode.Trim(), 4) != "0774" && VB.Left(clsType.User.BuseCode.Trim(), 4) != "0445" && clsType.User.IdNumber != "21403" && clsType.User.BuseCode.Trim() != "101773")        //김현욱 테스트 추가, 건진 접수 추가
            {
                if (ArgJin != "2" && ArgJin != "4" && ArgJin != "5" && ArgJin != "E" && ArgJin != "L" && ArgJin != "D")
                    rtnVal = false;
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 당일DRG입원대상
        /// Author : 박병규
        /// Create Date : 2017.11.10
        /// </summary>
        /// <param name="ArgDept"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="OPD_FM_RESV.BAS:READ_FM_JEPSU"/>
        public bool READ_FM_JEPSU(PsmhDb pDbCon, string ArgDept, string ArgPtno, string ArgDate)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            bool rtnVal = false;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "OPD_MASTER_DEL ";
            SQL += ComNum.VBLF + "  Where 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = true;

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 신/구환 체크
        /// Author : 박병규
        /// Create Date : 2017.11.10
        /// </summary>
        /// <param name="ArgDept"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <seealso cref="OUMSAD.BAS:Check_Singu"/>
        public string CHECK_SINGU(PsmhDb pDbCon, string ArgPtno, string ArgDate)
        {
            DataTable DtFunc = null;
            DataTable DtFSub = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "0";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT STARTDATE ";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
            SQL += ComNum.VBLF + " WHERE 1          = 1 ";
            SQL += ComNum.VBLF + "   AND PANO       = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "   AND STARTDATE  >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "   AND STARTDATE  <  TO_DATE('" + VB.DateAdd("D", 1, ArgDate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT PANO ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + " WHERE 1      = 1 ";
                SQL += ComNum.VBLF + "   AND PANO   = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "   AND BDATE  <= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND SINGU  = '1' ";
                SqlErr = clsDB.GetDataTableEx(ref DtFSub, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                if (DtFSub.Rows.Count > 0)
                    rtnVal = "0";
                else
                    rtnVal = "1";

                DtFSub.Dispose();
                DtFSub = null;
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// 김효성
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strArg"></param>
        /// <returns></returns>
        public string READ_BAS_SANID(PsmhDb pDbCon, string strArg)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strVal = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT TODATE                    ";
            SQL = SQL + ComNum.VBLF + "   FROM BAS_SANJIN                ";
            SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + strArg + "'    ";
            SQL = SQL + ComNum.VBLF + "    AND TODATE IS NOT NULL        ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY DATEBAL DESC           ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return strVal = "";
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return strVal;
            }
            else
            {
                strVal = dt.Rows[0]["TODATE"].ToString().Trim();
            }
            return strVal;
        }

        /// <summary>
        /// 김효성
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strArg"></param>
        /// <returns></returns>
        public string GelNameGubun(PsmhDb pDbCon, string strArg)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strVal = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT MiaName                            ";
            SQL = SQL + ComNum.VBLF + "   FROM Bas_Mia                            ";
            SQL = SQL + ComNum.VBLF + "  WHERE MiaCode = '" + (strArg).Trim() + "'  ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return strVal = "";
            }
            if (dt.Rows.Count > 0)
            {
                strVal = dt.Rows[0]["Kiho"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            return strVal;
        }

        /// <summary>
        /// Description : 진료비 본인부담율 나이구분 
        /// Author : 김민철
        /// Create Date : 2017.11.14
        /// </summary>
        /// <param name="nAge"></param>
        /// <param name="strJumin"></param>
        /// <param name="strCurDate"> 기준일자 </param>
        /// <returns></returns>
        public string Acct_Age_Gubun(int nAge, string strJumin, string strCurDate, string strIO)
        {
            string strFDate = string.Empty;
            string rtnVal = "0";

            ComFunc CF = new ComFunc();

            if (strCurDate == "")
            {
                strCurDate = clsPublic.GstrSysDate;
            }
            
            //나이체크
            if (nAge == 0)      // 0 살
            {
                //개월수 체크 1달 미만 신생아 여부 체크
                if (ComFunc.AgeCalcEx_Zero(strJumin, strCurDate) <= 28)
                {
                    rtnVal = "1";     //신생아
                }
                else
                {
                   if (string.Compare(strCurDate, "2019-01-01") >= 0 && strIO =="O")
                    {
                        rtnVal = "5";     //1세미만
                    }
                    else
                    {
                        rtnVal = "2";     //6세미만
                    }

                }
            }
            else if (nAge < 6)
            {
                rtnVal = "2";     //6세미만
            }
            else if (nAge >= 6 && nAge <= 15)
            {
                rtnVal = "3";     //6세이상 15세미만
            }
            else if (nAge >= 65)
            {
                rtnVal = "4";     //65세이상 
            }
            else
            {
                rtnVal = "0";
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : READ_Gamek_Rate 루틴 뒤에 들어가야 기본감액 세팅 clear 되고 처리됨
        /// Author : 박병규
        /// Create Date : 2017.12.12
        /// <param name="ArgGbGamek"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgOutDate"></param>
        /// <param name="ArgLtdCode"></param>
        /// <param name="ArgIO"></param>
        /// </summary>
        /// <seealso cref="OPDGAMEK.BAS:READ_Gamek_Rate_응급관리료"/>
        public string READ_GAMEK_RATE_ERCOST(PsmhDb pDbCon, string ArgGbGamek, string ArgJumin)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            if (ArgGbGamek != "23") { return rtnVal; }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.ROWID, b.GamMessage ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_ERP + "INSA_MSTB a,     --인사가족사항";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_GAMF b      --직원 및 직계 감액 Table";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND a.JUMIN3  = '" + clsAES.AES(ArgJumin) + "' ";
            SQL += ComNum.VBLF + "    AND a.KWAN IN ('0','1','2','3') ";  //자녀만...
            SQL += ComNum.VBLF + "    AND (b.GAMEND >= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') OR b.GAMEND IS NULL) ";
            SQL += ComNum.VBLF + "    AND a.JUMIN3  = b.GAMJUMIN3 ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                rtnVal = "OK";
                clsPmpaType.GAM.ER_Rate = 100;
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 자격점검
        /// Author : 박병규
        /// Create Date : 2017.12.13
        /// <param name="ArgDate">날짜</param>
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgBi">보험종류</param>
        /// <param name="ArgDept">진료과</param>
        /// <param name="ArgType">자격점검</param>
        /// <param name="ArgGubun">예약구분</param>
        /// </summary>
        /// <seealso cref="oumsad.bas:READ_OPD_NHIC_자격점검"/>
        public string CHECK_OPD_NHIC_CONDITION(PsmhDb pDbCon, string ArgDate, string ArgPtno, string ArgBi, string ArgDept, string ArgType, string ArgGubun)
        {
            DataTable DtFunc = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            clsVbfunc vb = new clsVbfunc();

            clsPmpaPb.GstrNhic_Message = "";

            if (ArgBi == "21" || ArgBi == "22") { return rtnVal; }

            string strOK = "OK";
            string strOK2 = "";

            string strM_Jin = "";
            string strM_Bi = "";
            string strM_Mcode = "";
            string strM_Vcode = "";

            string strRegs1 = "";
            string strRegs2 = "";
            string strRegs3 = "";
            string strRegs4 = "";
            string strRegs1_2 = "";
            string strRegs2_2 = "";
            string strRegs3_2 = "";
            string strRegs4_2 = "";

            string str_Nhic = "";
            string str_Nhic_Mcode = "";

            string strReg_Bi = "";
            string strSangSil = "";

            //예약체크
            if (ArgGubun == "1")
            {
                strOK = "";

                SQL = "";
                SQL += ComNum.VBLF + " SELECT Pano, Bi, MCode, ";
                SQL += ComNum.VBLF + "        VCode, Jin ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
                SQL += ComNum.VBLF + "    AND RESERVED  = '1' ";
                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return rtnVal;
                }

                if (DtFunc.Rows.Count > 0)
                {
                    strOK = "OK";

                    strM_Jin = DtFunc.Rows[0]["JIN"].ToString().Trim();
                    strM_Bi = DtFunc.Rows[0]["BI"].ToString().Trim();
                    strM_Mcode = DtFunc.Rows[0]["MCODE"].ToString().Trim();
                    strM_Vcode = DtFunc.Rows[0]["VCODE"].ToString().Trim();
                }

                DtFunc.Dispose();
                DtFunc = null;
            }

            if (strOK != "OK") { return rtnVal; }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, M2_JAGEK, M2_CDATE, ";
            SQL += ComNum.VBLF + "        M2_SUJIN_NAME, M2_RESTRICT, M2_SEDAE_NAME, ";
            SQL += ComNum.VBLF + "        M2_KIHO, M2_GKIHO, M2_SANGSIL, ";
            SQL += ComNum.VBLF + "        M2_BONIN, M2_GJAN_AMT, M2_CHULGUK, ";
            SQL += ComNum.VBLF + "        M2_JANG_DATE, M2_SHOSPITAL1, M2_SHOSPITAL2, ";
            SQL += ComNum.VBLF + "        M2_SHOSPITAL3, M2_SHOSPITAL4, M2_SHOSPITAL_NAME1, ";
            SQL += ComNum.VBLF + "        M2_SHOSPITAL_NAME2, M2_SHOSPITAL_NAME3, M2_SHOSPITAL_NAME4, ";
            SQL += ComNum.VBLF + "        JOB_STS, M2_DISREG1, M2_DISREG2, ";
            SQL += ComNum.VBLF + "        M2_DISREG3, M2_DISREG4, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, ";
            SQL += ComNum.VBLF + "        M2_REMAMT  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND JOB_STS   = '2' "; //자격조회 점검완료된것
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "'  ";
            SQL += ComNum.VBLF + "   ORDER BY SENDTIME DESC  ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                strSangSil = DtFunc.Rows[0]["M2_RESTRICT"].ToString().Trim(); //자격상실구분
                strReg_Bi = DtFunc.Rows[0]["M2_JAGEK"].ToString().Trim(); 

                //희귀난치대상자 H000
                strRegs1 = DtFunc.Rows[0]["M2_DISREG1"].ToString().Trim();
                if (strRegs1 != "")
                {
                    str_Nhic_Mcode += VB.Mid(strRegs1.Trim(), 1, 1) + "^^";
                    strRegs1_2 = VB.Mid(strRegs1.Trim(), 1, 4).Trim() + "@@";
                    strRegs1_2 += VB.Mid(strRegs1.Trim(), 5, 8).Trim() + "@@";
                    strRegs1_2 += vb.Date_Format(VB.Mid(strRegs1.Trim(), 5, 8)).Trim() + "@@";
                    strRegs1_2 += VB.Mid(strRegs1.Trim(), 13, 8).Trim();

                    for (int i = 21; i <= 45; i = i + 5)
                        strRegs1_2 += VB.Mid(strRegs1.Trim(), i, 5).Trim() + "@@";
                }

                //산정특례(희귀)등록대상자 V000
                strRegs2 = DtFunc.Rows[0]["M2_DISREG2"].ToString().Trim();
                if (strRegs2 != "")
                {
                    str_Nhic_Mcode += VB.Mid(strRegs2.Trim(), 1, 1) + "^^";
                    strRegs2_2 = VB.Mid(strRegs2.Trim(), 1, 4).Trim() + "@@";
                    strRegs2_2 += VB.Mid(strRegs2.Trim(), 20, 8).Trim() + "@@";
                    strRegs2_2 += vb.Date_Format(VB.Mid(strRegs2.Trim(), 20, 8)).Trim() + "@@";
                    strRegs2_2 += VB.Mid(strRegs2.Trim(), 28, 8).Trim() + "@@";
                    strRegs2_2 += VB.Mid(strRegs2.Trim(), 5, 15).Trim() + "@@";
                }

                //차상위대상자 C000,E000,F000
                strRegs3 = DtFunc.Rows[0]["M2_DISREG3"].ToString().Trim();
                if (strRegs3 != "")
                {
                    str_Nhic_Mcode += VB.Mid(strRegs3.Trim(), 1, 1) + "^^";
                    strRegs3_2 = VB.Mid(strRegs3.Trim(), 1, 4).Trim() + "@@";
                    strRegs3_2 += VB.Mid(strRegs3.Trim(), 5, 8).Trim() + "@@";
                    strRegs3_2 += vb.Date_Format(VB.Mid(strRegs3.Trim(), 5, 8)).Trim() + "@@";
                    strRegs3_2 += VB.Mid(strRegs3.Trim(), 13, 8).Trim() + "@@";
                    strRegs3_2 += VB.Mid(strRegs3.Trim(), 21, 1).Trim() + "@@"; //차상위구분
                    strRegs3_2 += VB.Mid(strRegs3.Trim(), 1, 1).Trim() + "@@"; //차상위기호
                }

                //중증암환자
                strRegs4 = DtFunc.Rows[0]["M2_DISREG4"].ToString().Trim();
                if (strRegs4 != "")
                {
                    strRegs4_2 = VB.Mid(strRegs4.Trim(), 1, 4).Trim() + "@@";
                    strRegs4_2 += VB.Mid(strRegs4.Trim(), 20, 8).Trim() + "@@";
                    strRegs4_2 += VB.Mid(strRegs4.Trim(), 28, 8).Trim() + "@@";
                    strRegs4_2 += VB.Mid(strRegs4.Trim(), 36, 5).Trim() + "@@";
                    strRegs4_2 += VB.Mid(strRegs4.Trim(), 5, 15).Trim() + "@@";
                }

                if (str_Nhic_Mcode != "")
                {
                    for (int i = 1; i <= VB.I(str_Nhic_Mcode, "^^") - 1; i++)
                        str_Nhic += VB.Pstr(str_Nhic_Mcode, "^^", i).Trim() + "000 ";
                }

                //차상위, 희귀 자격체크
                if (str_Nhic_Mcode == "")
                {
                    if (VB.Left(strM_Mcode.Trim(), 1) != str_Nhic_Mcode)
                    {
                        rtnVal += "예약환자 자격점검 [예약당시 자격과 현재접수 자격 비교]" + '\r' + '\r';
                        rtnVal += "예약당시[" + strM_Mcode + "] 자격과 현재자격[" + str_Nhic + "]이 다름..자격확인후 처리요망" + '\r';
                    }
                }
                else
                {
                    if (Convert.ToInt32(VB.I(str_Nhic_Mcode, "^^") - 1) == 1) //자격1개
                    {
                        if (VB.Left(strM_Mcode.Trim(), 1) != VB.TR(str_Nhic_Mcode, "^^", "").Trim())
                        {
                            rtnVal += "예약환자 자격점검 [예약당시 자격과 현재접수 자격 비교]" + '\r' + '\r';
                            rtnVal += "예약당시[" + strM_Mcode + "] 자격과 현재자격[" + str_Nhic + "]이 다름..자격확인후 처리요망" + '\r';
                        }
                    }
                    else
                    {
                        if (strM_Mcode == "")
                        {
                            rtnVal += "예약환자 자격점검 [예약당시 자격과 현재접수 자격 비교]" + '\r' + '\r';
                            rtnVal += "예약당시[" + strM_Mcode + "] 자격과 현재자격[" + str_Nhic + "]이 다름..자격확인후 처리요망" + '\r';
                        }
                        else
                        {
                            strOK2 = "";
                            for (int i = 1; i <= VB.I(str_Nhic_Mcode, "^^") - 1; i++)
                            {
                                if (VB.Left(strM_Mcode.Trim(), 1) == VB.Pstr(str_Nhic_Mcode, "^^", i).Trim())
                                    strOK2 = "OK";
                            }

                            if (strOK2 == "") //2건이상 자격인데 OPD_MASTER 자격이 없는경우
                            {
                                rtnVal += "예약환자 자격점검 [예약당시 자격과 현재접수 자격 비교]" + '\r' + '\r';
                                rtnVal += "예약당시[" + strM_Mcode + "] 자격과 현재자격[" + str_Nhic + "]이 다름..자격확인후 처리요망" + '\r';
                            }
                            else //'2건이상 자격인데 OPD_MASTER 자격이 다를경우
                            {
                                rtnVal += "예약환자 자격점검 [예약당시 자격과 현재접수 자격 비교]" + '\r' + '\r';
                                rtnVal += "예약당시[" + strM_Mcode + "] 자격과 현재자격[" + str_Nhic + "]이 다름..자격확인후 처리요망" + '\r';
                            }
                        }
                    }
                }

                //중증코드 체크 F003 의약분업코드는 제외
                if (strM_Vcode != "F003" && strM_Vcode != VB.Left(strRegs4.Trim(), 4))
                    rtnVal += "예약당시 중증코드[" + strM_Vcode + "] 와 자격조회후 중증코드[" + VB.Left(strRegs4.Trim(), 4) + "] 가 불일치함.";

                if (strSangSil != "" && strSangSil != "00")
                {
                    if (rtnVal != "")
                    {
                        rtnVal += '\r' + '\r';
                        rtnVal += "자격상실자임. 필히 접수확인요망!!!";
                    }
                    else
                    {
                        rtnVal += "자격상실자임. 필히 접수확인요망!!!";
                    }
                }

                switch (strReg_Bi.Trim())
                {
                    case "1":
                    case "2":
                    case "4":
                    case "5":
                    case "6":
                        strReg_Bi = "건보";
                        break;

                    case "7":
                    case "8":
                        strReg_Bi = "급여";
                        break;
                }

                switch (strM_Bi.Trim())
                {
                    case "11":
                    case "12":
                    case "13":
                        strM_Bi = "건보";
                        break;

                    case "21":
                    case "22":
                        strM_Bi = "급여";
                        break;
                }

                if (strM_Bi != strReg_Bi && Convert.ToInt32(ArgBi) < 30)
                    rtnVal += "예약당시 자격[" + strM_Bi + "] 와 자격조회후 자격[" + strReg_Bi + "] 가 불일치합니다.";

                if (strSangSil != "" && strSangSil != "00")
                {
                    if (rtnVal != "")
                    {
                        rtnVal += '\r' + '\r';
                        rtnVal += "자격상실자임. 필히 접수확인요망!!!";
                    }
                    else
                    {
                        rtnVal += "자격상실자임. 필히 접수확인요망!!!";
                    }
                }
            }
            else
            {
                rtnVal = "당일 예약자이나, 당일 자격조회 자료가 없음..자격조회후 다시 수납요망!!";
            }

            DtFunc.Dispose();
            DtFunc = null;

            clsPmpaPb.GstrNhic_Message = rtnVal;

            return rtnVal;
        }

        public string CHECK_OPD_NHIC_CONDITION_NOW(PsmhDb pDbCon, string ArgDate, string ArgPtno, string ArgBi, string ArgDept, string ArgType, string ArgGubun)
        {
            DataTable DtFunc = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            clsVbfunc vb = new clsVbfunc();

            clsPmpaPb.GstrNhic_Message = "";


            string strOK = "OK";
            string strOK2 = "";

            string strM_Jin = "";
            string strM_Bi = "";
            string strM_Mcode = "";
            string strM_Vcode = "";

            string strRegs1 = "";
            string strRegs2 = "";
            string strRegs3 = "";
            string strRegs4 = "";
            string strRegs1_2 = "";
            string strRegs2_2 = "";
            string strRegs3_2 = "";
            string strRegs4_2 = "";

            string str_Nhic = "";
            string str_Nhic_Mcode = "";

            string strReg_Bi = "";
            string strSangSil = "";

          
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, M2_JAGEK, M2_CDATE, ";
            SQL += ComNum.VBLF + "        M2_SUJIN_NAME, M2_RESTRICT, M2_SEDAE_NAME, ";
            SQL += ComNum.VBLF + "        M2_KIHO, M2_GKIHO, M2_SANGSIL, ";
            SQL += ComNum.VBLF + "        M2_BONIN, M2_GJAN_AMT, M2_CHULGUK, ";
            SQL += ComNum.VBLF + "        M2_JANG_DATE, M2_SHOSPITAL1, M2_SHOSPITAL2, ";
            SQL += ComNum.VBLF + "        M2_SHOSPITAL3, M2_SHOSPITAL4, M2_SHOSPITAL_NAME1, ";
            SQL += ComNum.VBLF + "        M2_SHOSPITAL_NAME2, M2_SHOSPITAL_NAME3, M2_SHOSPITAL_NAME4, ";
            SQL += ComNum.VBLF + "        JOB_STS, M2_DISREG1, M2_DISREG2, ";
            SQL += ComNum.VBLF + "        M2_DISREG3, M2_DISREG4, TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, ";
            SQL += ComNum.VBLF + "        M2_REMAMT  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_NHIC ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND JOB_STS   = '2' "; //자격조회 점검완료된것
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "'  ";
            SQL += ComNum.VBLF + "   ORDER BY SENDTIME DESC  ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return rtnVal;
            }

            if (DtFunc.Rows.Count == 0)
            {

                rtnVal += "자격확인안됨. 필히 접수확인요망!!!";

            }

            DtFunc.Dispose();
            DtFunc = null;

            clsPmpaPb.GstrNhic_Message = rtnVal;

            return rtnVal;
        }


        /// <summary>
        /// Description : 인체면역결핍오더체크
        /// Author : 박병규
        /// Create Date : 2017.12.14
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <seealso cref="frmjepsufailedbuild.frm:TelNo_Edit_Process"/>
        public string TelNo_Edit_Process(string ArgTel)
        {
            string rtnVal = "";

            if (ArgTel.Trim() == "")
                return rtnVal;

            for (int i = 1; i <= ArgTel.Length; i++)
            {
                if (string.Compare(VB.Mid(ArgTel, i, 1), "0") >= 0 && string.Compare(VB.Mid(ArgTel, i, 1), "9") <= 0)
                    rtnVal += VB.Mid(ArgTel, i, 1);
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 상병명 및 NOUSE 값 가져오기
        /// Author : 박병규
        /// Create Date : 2018.1.2
        /// <param name="pDbCon"></param>
        /// <param name="ArgCode"></param>
        /// <param name="ArgOpt">1.ILLNAMEK, 2.NOUSE</param>
        /// </summary>
        /// <seealso cref=""/>
        public string READ_BAS_ILLS(PsmhDb pDbCon, string ArgCode, string ArgOpt)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ILLNAMEK, NOUSE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ILLS ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND ILLCODE   = '" + ArgCode + "' ";
            SQL += ComNum.VBLF + "    AND ILLCLASS  = '1' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                if (ArgOpt == "1")
                    rtnVal = DtFunc.Rows[0]["ILLNAMEK"].ToString().Trim();
                else if (ArgOpt == "2")
                    rtnVal = (DtFunc.Rows[0]["NOUSE"].ToString().Trim() == "N") ? "불완전" : "";
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 소요분을 소요시간으로 변환
        /// Author : 박병규
        /// Create Date : 2018.1.4
        /// <param name="ArgFtime">종료시각</param>
        /// <param name="ArgTtime">시작시각</param>
        /// </summary>
        /// <seealso cref=""/>
        public string Date_Time_Hour(PsmhDb pDbCon, string ArgFtime, string ArgTtime)
        {
            ComFunc CF = new ComFunc();
            long nTime;
            long nHour;
            long nMinu;
            string rtnVal = "";

            nTime = CF.DATE_TIME(pDbCon, ArgFtime, ArgTtime);
            rtnVal = "00:00";

            nHour = Convert.ToInt32(nTime / 60);
            nMinu = nTime % 60;
            rtnVal = string.Format("{0:00}", nHour) + ":" + string.Format("{0:00}", nMinu);

            return rtnVal;
        }

        /// <summary>
        /// Description : 치매약제 확인
        /// Author : 박병규
        /// Create Date : 2018.1.5
        /// <param name="pDbCon"></param>
        /// <param name="ArgCode"></param>
        /// <param name="ArgOpt">1.ILLNAMEK, 2.NOUSE</param>
        /// </summary>
        /// <seealso cref=""/>
        public string Check_Dementia_Drug(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUNEXT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN ";
            SQL += ComNum.VBLF + "  WHERE 1          = 1 ";
            SQL += ComNum.VBLF + "    AND SUNEXT     = '" + ArgCode + "' ";
            SQL += ComNum.VBLF + "    AND GBDEMENTIA = 'Y' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["SUNEXT"].ToString().Trim();

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 소아면제코드 Query
        /// Author : 김민철
        /// Create Date : 2018.1.5
        /// <param name="pDbCon"></param>
        /// </summary>
        /// <seealso cref=""/>
        public DataTable sel_Bas_OgPdBun(PsmhDb pDbCon)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT Code,Name                                  ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE                 ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                       ";
            SQL = SQL + ComNum.VBLF + "    AND Gubun = 'BAS_소아면제'                        ";
            SQL = SQL + ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY Sort,Code                                   ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return dt;
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

        }

        public string Rtn_Ipd_OgPdBunDtl(string OgPdBunDtl)
        {
            string rtnVal = string.Empty;

            switch (OgPdBunDtl)
            {
                case "C": rtnVal = "C000"; break;
                case "E": rtnVal = "E000"; break;
                case "F": rtnVal = "F000"; break;
                case "H": rtnVal = "H000"; break;
                case "V": rtnVal = "V000"; break;
                case "1": rtnVal = "EV00"; break;
                case "2": rtnVal = "EV00"; break;
                default: rtnVal = ""; break;
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 진료비 조회 구분별 TRANS 내역 조회
        /// Author : 김민철
        /// Create Date : 2018.1.15
        /// <param name="pDbCon"></param>
        /// </summary>
        /// <seealso cref=""/>
        public DataTable sel_IpdTrs(PsmhDb pDbCon, string strKeyWard, string strGbn, string strSTS, string strJob)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT A.PANO, B.SNAME, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE, TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE  ";
                SQL = SQL + ComNum.VBLF + "        ,A.ILSU, A.BI, A.DEPTCODE, " + ComNum.DB_MED + "FC_BAS_DOCTOR_DRNAME(A.DRCODE) DRCODE, A.GBIPD   ";
                SQL = SQL + ComNum.VBLF + "        ,A.SANGAMT, A.OGPDBUN,A.OGPDBUNdtl, A.AMSET3, b.SECRET, a.GBSPC ,A.GBDRG  ,OGPDBUN2,JINDTL       ";
                SQL = SQL + ComNum.VBLF + "        ,B.ROOMCODE, B.WARDCODE, A.VCODE, A.IPDNO, A.TRSNO, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE      ";
                SQL = SQL + ComNum.VBLF + "        ,A.BOHUN, " + ComNum.DB_MED + "FC_IPD_GBSTS_NM(A.GBSTS) GBSTS ,A.ROWID                           ";
                if (strJob == "임시자격")
                {
                    SQL = SQL + ComNum.VBLF + "        ,'' FCODE, '' DRGCODE ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "WORK_IPD_TRANS_TERM A,                                                 ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "        ,A.FCODE, A.DRGCODE ";
                    SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS A,                                                           ";
                }
                SQL = SQL + ComNum.VBLF + "        " + ComNum.DB_PMPA + "IPD_NEW_MASTER B                                                           ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1 = 1                                                                                            ";
                SQL = SQL + ComNum.VBLF + "    AND A.GBIPD != 'D'                                                                                   ";    //삭제자격
                SQL = SQL + ComNum.VBLF + "    AND B.GBSTS != '9'                                                                                   ";    //입원취소
                if (strGbn == "1")
                {
                    SQL = SQL + ComNum.VBLF + "  AND B.PANO  = '" + strKeyWard + "' ";
                }
                else if (strGbn == "2")
                {
                    SQL = SQL + ComNum.VBLF + "  AND B.SNAME = '" + strKeyWard + "' ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  AND A.OUTDATE = TO_DATE('" + strKeyWard + "','YYYY-MM-DD') ";
                }

                if (strSTS == "J")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE IS NULL ";
                }
                else if (strSTS == "T")
                {
                    SQL = SQL + ComNum.VBLF + "   AND A.ACTDATE IS NOT NULL ";
                }
                SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = B.IPDNO ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PANO,INDATE DESC, TRSNO ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return dt;
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

        }

        /// <summary>
        /// Description : 물리치료 바우처
        /// Author : 박병규
        /// Create Date : 2018.1.16
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgBdate"></param>
        /// </summary>
        /// <seealso cref="READ_물리바우처_AMT"/>
        public long Read_Pt_VoucherAmt(PsmhDb pDbCon, string ArgPtno, string ArgBdate)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            long rtnVal = 0;

            string strYyMm = VB.Left(VB.Replace(ArgBdate, "-", "").Trim(), 6);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT Gubun  ";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "NUR_PT_PATIENT ";
            SQL += ComNum.VBLF + " WHERE 1      = 1 ";
            SQL += ComNum.VBLF + "   AND Pano   = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "   AND YYYYMM = '" + strYyMm + "' ";
            SQL += ComNum.VBLF + "   AND FLAG   = '1' ";  //물리바우처
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
            {
                switch (DtFunc.Rows[0]["GUBUN"].ToString().Trim())
                {
                    case "1":
                        rtnVal = 27500;
                        break;

                    case "2":
                        rtnVal = 25000;
                        break;

                    case "3":
                        rtnVal = 22500;
                        break;

                    case "4":
                        rtnVal = 20000;
                        break;
                }
            }

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 희귀난치VCode
        /// Author : 박병규
        /// Create Date : 2018.1.16
        /// <param name="pDbCon"></param>
        /// <param name="ArgCode"></param>
        /// </summary>
        /// <seealso cref="oumsad.bas:희귀난치VCode"/>
        public string Read_Vcode(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "NO";


            SQL = "";
            SQL += ComNum.VBLF + " SELECT SuNext ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND GBRARE    = 'Y' ";
            SQL += ComNum.VBLF + "    AND SuNext    = '" + ArgCode + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = "OK";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 감액제외코드점검
        /// Author : 박병규
        /// Create Date : 2018.1.16
        /// <param name="pDbCon"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgCode"></param>
        /// </summary>
        /// <seealso cref="opdgamek.bas:감액제외코드점검"/>
        public string Read_Bas_Bcode(PsmhDb pDbCon, string ArgGubun, string ArgCode)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "NO";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT CODE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN     = '" + ArgGubun + "' ";
            SQL += ComNum.VBLF + "    AND CODE      = '" + ArgCode.Trim() + "' ";
            SQL += ComNum.VBLF + "    AND (DELDATE IS NULL or DELDATE = '') ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = "OK";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 수가 상세분류 조회
        /// Author : 박병규
        /// Create Date : 2018.1.17
        /// <param name="pDbCon"></param>
        /// <param name="ArgCode">SUNEXT</param>
        /// </summary>
        /// <seealso cref="oumsad_chk.bas:RTN_BAS_SUN_DTLBUN"/>
        public string Read_DtlBun(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DTLBUN ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SUNEXT    = '" + ArgCode + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["DTLBUN"].ToString().Trim();

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 의사코드에 따른 진료과 가져오기
        /// Author : 박병규
        /// Create Date : 2018.1.20
        /// <param name="ArgDr">SUNEXT</param>
        /// </summary>
        /// <seealso cref=""/>
        public string Read_DrDept(string ArgDr)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DRDEPT1 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND DRCODE    = '" + ArgDr + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["DRDEPT1"].ToString().Trim();

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description :환자종류 및 기준일자로 약제상한부담율을 구함
        /// Author : 박병규
        /// Create Date : 2018.1.23
        /// <param name="ArgBi">ArrayClass</param>
        /// <param name="ArgGdate">StartDate</param>
        /// </summary>
        /// <seealso cref="vbSugaRead_New.bas:READ_DRUG_MIR_Rate"/>
        public int Rtn_DrugMir_Rate(string ArgBi, string ArgGdate)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            int rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(StartDate,'YYYY-MM-DD') StartDate, RateValue ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ACCOUNT ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND IDNAME        = 'DRUG_MIR' ";
            SQL += ComNum.VBLF + "    AND ArrayClass    = " + VB.Val(ArgBi) + " ";
            SQL += ComNum.VBLF + "    AND StartDate     <= TO_DATE('" + ArgGdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  ORDER BY StartDate DESC ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = Convert.ToInt32(DtFunc.Rows[0]["RateValue"].ToString().Trim());

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 예방접종코드점검
        /// Author : 박병규
        /// Create Date : 2018.1.23
        /// <param name="pDbCon"></param>
        /// <param name="ArgCode"></param>
        /// </summary>
        /// <seealso cref=""/>
        public string Check_Vaccine(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUNEXT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN ";
            SQL += ComNum.VBLF + "  WHERE 1          = 1 ";
            SQL += ComNum.VBLF + "    AND SUNEXT     = '" + ArgCode + "' ";
            SQL += ComNum.VBLF + "    AND GBYEBANG   = 'Y' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = "OK";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description :
        /// Author : 박병규
        /// Create Date : 2018.1.29
        /// <param name="ArgPtno">ArrayClass</param>
        /// <param name="ArgSuCode">StartDate</param>
        /// <param name="ArgOrderNo">StartDate</param>
        /// </summary>
        /// <seealso cref="ErAcct.bas:READ_ORDER_ACTTING_TIME"/>
        public string Rtn_Actting_Time(PsmhDb pDbCon, string ArgPtno, string ArgSuCode, double ArgOrderNo, string ArgBdate)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ER24 ActTime ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_IORDER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PTNO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND ORDERNO   = " + ArgOrderNo + " ";
            SQL += ComNum.VBLF + "   Union all SELECT '1' ActTime       ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ORAN_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PaNO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND opdate   = to_date('" + ArgBdate + "','yyyy-mm-dd') ";

            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["ActTime"].ToString().Trim();

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description :ORDER 외과 흉부외과 가산 확인
        /// Author : 박병규
        /// Create Date : 2018.1.29
        /// <param name="ArgPtno">ArrayClass</param>
        /// <param name="ArgSuCode">StartDate</param>
        /// <param name="ArgOrderNo">StartDate</param>
        /// </summary>
        /// <seealso cref="ErAcct.bas:READ_ORDER_ACTTING_TIME"/>
        public string Rtn_Actting_GSAdd(PsmhDb pDbCon, string ArgPtno, string ArgSuCode, double ArgOrderNo , string ArgBdate )
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT GSADD ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_IORDER ";
            SQL += ComNum.VBLF + "  WHERE PTNO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND ORDERNO   = " + ArgOrderNo + " ";
            SQL += ComNum.VBLF + "  union all ";
            SQL += ComNum.VBLF + " SELECT decode(deptcode,'GS','1','CS','2','0'   ) GSADD ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
            SQL += ComNum.VBLF + "  WHERE PTNO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND ORDERNO   = " + ArgOrderNo + " ";
            SQL += ComNum.VBLF + "  union all ";
            SQL += ComNum.VBLF + " SELECT GSADD ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ORAN_SLIP ";
            SQL += ComNum.VBLF + "  WHERE PaNO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND ipdopd='O' ";
            SQL += ComNum.VBLF + "    AND opdate   = TO_DATE('" + ArgBdate + " ')   group by opdate,sucode,gsadd HAVING SUM(QTY) <> 0 ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["GSADD"].ToString().Trim();

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : ORDER 수술구분확인(0.주수술, 1.부수술, 2.제2수술)
        /// Author : 박병규
        /// Create Date : 2018.3.19
        /// <param name="ArgPtno">ArrayClass</param>
        /// <param name="ArgSuCode">StartDate</param>
        /// <param name="ArgOrderNo">StartDate</param>
        /// </summary>
        public string Rtn_Actting_OpGubun(PsmhDb pDbCon, string ArgPtno, string ArgSuCode, double ArgOrderNo)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT OPGUBUN ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_IORDER ";
            SQL += ComNum.VBLF + "  WHERE PTNO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND ORDERNO   = " + ArgOrderNo + " ";
            SQL += ComNum.VBLF + "  union all ";
            SQL += ComNum.VBLF + " SELECT OPGUBUN ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
            SQL += ComNum.VBLF + "  WHERE PTNO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND ORDERNO   = " + ArgOrderNo + " ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["OPGUBUN"].ToString().Trim();

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }


        /// <summary>
        /// Description : 특정기호 구분 읽어오기
        /// Author : 김민철
        /// Create Date : 2018.1.31
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public DataTable sel_Bas_BCode_FCode(PsmhDb pDbCon)
        {
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = "";

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT Code,Name                                  ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE                 ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                       ";
            SQL = SQL + ComNum.VBLF + "    AND Gubun = 'BAS_특정기호'                        ";
            SQL = SQL + ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate > TRUNC(SYSDATE)) ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY Sort,Code                                   ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return dt;
            }

            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

        }


        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.2.2
        /// <param name="ArgGam"></param>
        /// </summary>
        /// <seealso cref="opdacct.bas:Gam_Code_Search_new"/>
        public string Gam_Code_Search(PsmhDb pDbCon, string ArgGam)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUCODE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMCODE  ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND CODE      = '" + ArgGam + "' ";
            SQL += ComNum.VBLF + "    AND SUCODE IS NOT NULL ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["SUCODE"].ToString().Trim();
            else
                rtnVal = "Y92Z"; //감액(기타)

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 가정간호 의료급여 선택병원 예외
        /// Author : 박병규
        /// Create Date : 2018.2.5
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <seealso cref="oumsad.bas:READ_의료급여_가정간호_강제대상"/>
        public string Read_Boho_HomeCare_Exception(PsmhDb pDbCon, string ArgPtno, string ArgDate)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT CODE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN         = '외래_가정간호선택병원강제대상' ";
            SQL += ComNum.VBLF + "    AND TRIM(CODE)    = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND JDate         = TO_DATE('" + ArgDate + "','YYYY-MM-DD')  ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = "OK";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.2.5
        /// <param name="ArgCode"></param>
        /// </summary>
        /// <seealso cref="oumsad.bas:READ_희귀난치VCode"/>
        public string Read_Rare_Vcode(PsmhDb pDbCon, string ArgCode)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "NO";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SuNext ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUN ";
            SQL += ComNum.VBLF + "  WHERE 1      = 1 ";
            SQL += ComNum.VBLF + "    AND GBRARE = 'Y'  ";
            SQL += ComNum.VBLF + "    AND SuNext = '" + ArgCode + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = "OK";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.2.8
        /// <param name="ArgPos"></param>
        /// <param name="ArgBw"></param>
        /// </summary>
        /// <seealso cref="frmSunapMain_New.frm:DN_BOOWI_CHANGE"/>
        public string DN_Boowi_Change(int ArgPos, string ArgBw)
        {
            string rtnVal = "";
            string strBOOWi = "        ";

            if (ArgPos == 2 || ArgPos == 3)
            {
                for (int fi = 0; fi < 8; fi++)
                {
                    if (VB.Mid(ArgBw.Trim(), fi + 1, 1) == "1")
                    {
                        if (clsPmpaType.TOM.Age > 8 || fi > 4)
                            strBOOWi = VB.Left(strBOOWi.Trim(), fi - 1) + fi + VB.Mid(strBOOWi.Trim(), fi + 1, strBOOWi.Length);
                        else
                            strBOOWi = VB.Left(strBOOWi.Trim(), fi - 1) + VB.Chr(64 + fi) +  VB.Mid(strBOOWi.Trim(), fi + 1, strBOOWi.Length);
                    }
                }
            }
            else
            {
                for (int fi = 0; fi < 8; fi++)
                {
                    if (VB.Mid(ArgBw.Trim(), fi + 1, 1) == "1")
                    {
                        if (clsPmpaType.TOM.Age > 8 || fi > 4)
                            strBOOWi = VB.Mid(strBOOWi.Trim(), (8 - fi), 1) + fi + VB.Mid(strBOOWi.Trim(), 8 + fi + 1, strBOOWi.Length);
                        else
                            strBOOWi = VB.Mid(strBOOWi.Trim(), (8 - fi), 1) + VB.Chr(64 + fi) + VB.Mid(strBOOWi.Trim(), 8 + fi + 1, strBOOWi.Length);
                    }
                }
            }

            rtnVal = strBOOWi;

            return rtnVal;
        }

        /// <summary>
        /// Description : 투약번호 생성
        /// Author : 박병규
        /// Create Date : 2018.2.12
        /// <param name="ArgCode"></param>
        /// </summary>
        /// <seealso cref="oumsad.bas:READ_DRUG_YAKNO"/>
        public int Read_Drug_YakNo()
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            int rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SEQ_TUYAK.NEXTVAL dNEXTVAL FROM DUAL ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = Convert.ToInt32(DtFunc.Rows[0]["dNEXTVAL"].ToString());

            DtFunc.Dispose();
            DtFunc = null;

            if (rtnVal == 0)
            {
                ComFunc.MsgBox("0번 투약번호 발생!!  : 전산실에 연락 요망 !");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;

                clsDB.setRollbackTran(clsDB.DbCon);
                Application.Exit();
            }

            return rtnVal;
        }

        /// <summary>
        /// 의료급여승인
        /// </summary>
        /// <param name="o"></param>
        public void ComboMsend_Set(ComboBox o)
        {
            o.Items.Add("");
            o.Items.Add("M009.응급환자인 선택의료급여기관 이용자 1종");
            o.Items.Add("M010.장애인보장구 지급받은 선택의료급여기관 이용자 1종");
            o.Items.Add("M011.행려환자 1종");
            o.Items.Add("B003.응급환자인 선택의료급여기관 이용자 2종");
            o.Items.Add("B004.장애인보장구 지급받은 선택의료급여기관 이용자 2종");
            o.Items.Add("B005.선택의료급여기관에서 의뢰된 자(1,2종)");
            o.Items.Add("B006.선택의료급여기관에서 외뢰되어 재의뢰된 자(1,2종)");
            o.Items.Add("B009.선택의료급여기관 적용자로서 의료급여의뢰서를 제출할것으로 갈음하는자(1,2종)");
            o.Items.Add("B010.임신부(2종)");
            o.Items.Add("B011.등록 조산아 및 저체중 출산아 (2종)");
            o.SelectedIndex = 0;
        }

        /// <summary>
        /// Description : 환불내역정보 읽기
        /// Author : 박병규
        /// Create Date : 2018.2.23
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgDept">진료과목</param>
        /// <param name="ArgJobdate">작업일자</param>
        /// <param name="ArgBdate">발생일자</param>
        /// <param name="ArgSeqNo">영수증번호</param>
        /// </summary>
        /// <seealso cref=""/>
        public string Read_Opd_Hoanbul(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgJobdate, string ArgBdate, int ArgSeqNo)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_HOANBUL ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND JOBDATE   = TO_DATE('" + ArgJobdate + "','YYYY-MM-DD')  ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND (SEQNO = " + ArgSeqNo + " OR SEQNO = 0 ) ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return rtnVal;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = "◈";

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : Ipd_New_Slip 수가 입력하기
        /// Author : 김민철
        /// Create Date : 2018.02.24
        /// <param name="pDbCon"></param>
        /// <param name="ArgSuCode"></param>
        /// <param name="ArgSuName"></param>
        /// <param name="ArgBDate"></param>
        /// <returns>bool</returns>
        /// </summary>
        public bool Ins_IpdSlip_SuCode(PsmhDb pDbCon, string ArgSuCode, string ArgBDate)
        {
            bool rtnVal = true;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;

            string strPCode = string.Empty;
            long nBaseAmt = 0;
            long nAMT1 = 0;

            clsPmpaPb cPb = new clsPmpaPb();
            clsIpdAcct cIA = new clsIpdAcct();
            clsIuSentChk cISentChk = new clsIuSentChk();
            clsBasAcct cBAcct = new clsBasAcct();

            clsPmpaType.cBas_Add_Arg cBArg = new clsPmpaType.cBas_Add_Arg();   //수가가산용 Class
            clsPmpaType.Bas_Acc_Rtn cBAR = new clsPmpaType.Bas_Acc_Rtn();    //EDI 계산 금액과 표준코드값을 받기 위함

            if (ArgSuCode != "AC421" && ArgSuCode != "AH013" && ArgSuCode != "AI120" && ArgSuCode != "AC321" && ArgSuCode != "AH011")
            {
                clsPublic.GstrMsgTitle = "★ " + ArgSuCode + " 발생 ★";
                clsPublic.GstrMsgList = ArgSuCode + "를 생성하시겠습니까?";

                if (ComFunc.MsgBoxQ(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return rtnVal;
                }
            }
            

            try
            {
                if (Suga_Read(pDbCon, ArgSuCode) == false)
                {
                    ComFunc.MsgBox(ArgSuCode + " 수가정보 오류!", "작업불가");
                    return false;
                }

                cIA.Move_RS_TO_ISG();                   //수가정보 세팅

                #region 가산항목 세팅
                if (clsPmpaType.ISG.SugbB == "0") { cBArg.AGE = 50; }  //나이가산 안타도록 보
                else { cBArg.AGE = clsPmpaType.TIT.Age; }
                
                cBArg.AGEILSU = clsPmpaType.TIT.AgeDays;
                cBArg.SUNEXT = clsPmpaType.ISG.Sunext;           //수가코드
                cBArg.BUN = clsPmpaType.ISG.Bun;              //수가분류
                cBArg.SUGBE = clsPmpaType.ISG.SugbE;            //수가 E항(기술료)
                
                cBArg.Bi = Convert.ToInt32(VB.Left(clsPmpaType.TIT.Bi, 1));               //자격
                cBArg.BDATE = ArgBDate;
                cBArg.GBER = "";               //응급 가산
                cBArg.NIGHT = "";               //공휴, 야간 가산
                cBArg.AN1 = "";               //마취 가산
                cBArg.OP1 = "";               //외과 / 흉부외과 가산
                cBArg.OP2 = "";               //화상 가산          
                cBArg.OP3 = "";               //
                cBArg.OP4 = "";               //산모가산
                cBArg.XRAY1 = "";               //판독 가산
                #endregion

                cBAR = cBAcct.Rtn_BasAdd_EdiSuga_Amt(pDbCon, cBArg);

                strPCode = cBAR.PCODE;

                //이미 가산된 코드이거나 인정비급여, 병원 임의수가 인경우 기존대로 금액 설정
                if (strPCode == "999999" || strPCode == "")
                {
                    nBaseAmt = clsPmpaType.ISG.BaseAmt;
                    nAMT1 = clsPmpaType.ISG.BaseAmt;
                }
                else
                {
                    //EDI 수가와 표준코드 조회
                    cBAR = cBAcct.Rtn_BasAdd_EdiSuga_Amt(pDbCon, cBArg);

                    strPCode = cBAR.PCODE;
                    nBaseAmt = cBAR.BASEAMT;
                    nAMT1 = cBAR.AMT;
                }

                if (nBaseAmt > 0)
                {
                    #region Ipd_New_Slip Data Set
                    cPb.ArgV = new string[Enum.GetValues(typeof(clsPmpaPb.enmIpdNewSlip)).Length];
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.IPDNO] = clsPmpaType.TIT.Ipdno.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.TRSNO] = clsPmpaType.TIT.Trsno.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ACTDATE] = clsPmpaPb.GstrSysDate;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PANO] = clsPmpaType.TIT.Pano;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BI] = clsPmpaType.TIT.Bi;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BDATE] = clsPmpaType.TIT.OutDate == "" ? clsPublic.GstrSysDate : clsPmpaType.TIT.OutDate;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUNEXT] = clsPmpaType.ISG.Sunext;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BUN] = clsPmpaType.ISG.Bun;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NU] = clsPmpaType.ISG.Nu;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.QTY] = "1";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.NAL] = "1";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BASEAMT] = nBaseAmt.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSPC] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBNGT] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBGISUL] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELF] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBCHILD] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DEPTCODE] = clsPmpaType.TIT.DeptCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRCODE] = clsPmpaType.TIT.DrCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.WARDCODE] = clsPmpaType.TIT.WardCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SUCODE] = clsPmpaType.ISG.Sucode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSLIP] = " ";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBHOST] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.PART] = clsType.User.IdNumber;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT1] = nAMT1.ToString();
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.AMT2] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.SEQNO] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.YYMM] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DRGSELF] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDERNO] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ABCDATE] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DEPT] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.OPER_DCT] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DEPT] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.ORDER_DCT] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.EXAM_WRTNO] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.RoomCode] = clsPmpaType.TIT.RoomCode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.DIV] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSELNOT] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBS] = "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBER] = "";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CBUN] = clsPmpaType.ISG.Bun + "0";
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUCODE] = clsPmpaType.ISG.Sucode;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.CSUNEXT] = clsPmpaType.ISG.Sunext;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSGADD] = cBArg.OP1;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAB] = cBArg.XRAY1;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAC] = cBArg.AN1;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.GBSUGBAD] = cBArg.OP2;
                    cPb.ArgV[(int)clsPmpaPb.enmIpdNewSlip.BCODE] = strPCode;
                    #endregion
                    SqlErr = Ins_IpdNewSlip(cPb.ArgV, pDbCon, ref intRowAffected);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return false;
                    }
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }


        }


        /// <summary>
        /// Description : 외래접수정보 가져오기
        /// Author : 박병규
        /// Create Date : 2018.02.26
        /// <param name="ArgPtno">등록번호</param>
        /// </summary>
        /// <seealso cref="READ_OPD_MASTER"/>
        public DataTable Get_OpdMaster(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgActdate = "", string ArgBdate = "")
        {
            DataTable DtTOM = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.*, A.ROWID ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER A";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";

                if (ArgBdate != "")
                    SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgBdate + "', 'YYYY-MM-DD') ";

                if (ArgActdate != "")
                    SQL += ComNum.VBLF + "    AND ACTDATE   <= TO_DATE('" + ArgActdate + "', 'YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTableEx(ref DtTOM, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return DtTOM;
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }


        /// <summary>
        /// Description : 당일 퇴원자 BBBBBB발생건 Ipd_New_Slip 수가 삭제하기
        /// Author : 김민철
        /// Create Date : 2018.02.24
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTrsNo"></param>
        /// <returns></returns>
        public bool IPD_DRUG_BBBBBB_DELETE(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTrsNo)
        {
            bool rtnVal = true;

            DataTable dt = null;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowAffected = 0;
            long nTrans_Drug = 0;

            ComFunc.ReadSysDate(pDbCon);

            try
            {
                //당일건은 BBBBBB발생건 삭제처리
                SQL = "";
                SQL += " DELETE " + ComNum.DB_PMPA + "IPD_NEW_SLIP      \r\n";
                SQL += "  WHERE TRSNO = " + ArgTrsNo + "                \r\n";
                SQL += "    AND IPDNO = " + ArgIpdNo + "                \r\n";
                SQL += "    AND PANO  ='" + ArgPano + "'                \r\n";
                SQL += "    AND ActDate=TRUNC(SYSDATE)                  \r\n";
                SQL += "    AND SuNext='BBBBBB'                         \r\n";
                SQL += "    AND Part NOT IN ('**')                      \r\n";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                //BBBBBB TRSNO 별 재계산 - 저가약제
                SQL = "";
                SQL += " SELECT SUM(AMT1+AMT2) AMT                      \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP      \r\n";
                SQL += "  WHERE PANO = '" + ArgPano + "'                \r\n";
                SQL += "    AND IPDNO = " + ArgIpdNo + "                \r\n";
                SQL += "    AND TRSNO = " + ArgTrsNo + "                \r\n";
                SQL += "    AND SUNEXT = 'BBBBBB'                       \r\n";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    nTrans_Drug = Convert.ToInt64(VB.Val(dt.Rows[0]["Amt"].ToString()));
                }

                dt.Dispose();
                dt = null;

                //TRANS AMT64에 금액저장
                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS     \r\n";
                SQL += "    SET AMT64 = " + nTrans_Drug + "         \r\n";
                SQL += "  WHERE TRSNO = " + ArgTrsNo + "            \r\n";
                SQL += "    AND IPDNO = " + ArgIpdNo + "            \r\n";
                SQL += "    AND PANO  = '" + ArgPano + "'           \r\n";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
        }


        /// <summary>
        /// Description : 전화번호 가져오기
        /// Author : 박병규
        /// Create Date : 2018.03.05
        /// <param name="ArgName"></param>
        /// </summary>
        /// <seealso cref="frm퇴원등록.frm : Read_TelNo"/>
        public string Get_TelNo(PsmhDb pDbCon, string ArgName)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TEL ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "ETC_TELBOOK ";
            SQL += ComNum.VBLF + "  Where Name2 LIKE '%" + ArgName + "%' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                DtFunc.Dispose();
                DtFunc = null;
                return null;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["TEL"].ToString().Trim();

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        public DataTable get_Bas_Sut_Sun(PsmhDb pDbCon, string ArgSuCode)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                SQL = "";
                SQL += " SELECT Sucode,Bun,Nu,SugbA,SugbB,SugbC,SugbAA,                                     \r\n";
                SQL += "        SugbD,SugbE,SugbF,SugbG,SugbH,SugbI,SugbJ, n.SugbQ, n.SugbR,n.SugbS,Iamt,   \r\n";
                SQL += "        Tamt,Bamt,TO_CHAR(Sudate, 'yyyy-mm-dd') Suday,                              \r\n";
                SQL += "        OldIamt,OldTamt,OldBamt,DayMax,TotMax, t.Sunext,                            \r\n";
                SQL += "        TO_CHAR(t.Sudate3, 'yyyy-mm-dd') Sudate3,                                   \r\n";
                SQL += "        t.Iamt3, t.Tamt3, t.Bamt3, TO_CHAR(t.Sudate4, 'yyyy-mm-dd') Sudate4,        \r\n";
                SQL += "        t.Iamt4, t.Tamt4, t.Bamt4, TO_CHAR(t.Sudate5, 'yyyy-mm-dd') Sudate5,        \r\n";
                SQL += "        t.Iamt5, t.Tamt5, t.Bamt5,                                                  \r\n";
                SQL += "        Sunamek,SuHam,Unit,Hcode,TO_CHAR(DelDate,'YYYY-MM-DD') DelDay               \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_SUT t,                                            \r\n";
                SQL += "        " + ComNum.DB_PMPA + "BAS_SUN n                                             \r\n";
                SQL += "  WHERE t.Sucode = '" + ArgSuCode + "'                                              \r\n";
                SQL += "    AND T.SuNext = n.SuNext(+)                                                          ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return Dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }
       

        /// <summary>
        /// Description : 진료과 전화번호 가져오기
        /// Author : 박병규
        /// Create Date : 2018.03.07
        /// <param name="pDbCon"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDr"></param>
        /// </summary>
        /// <seealso cref="oumsad.bas : DEPT_TELNO_GET"/>
        public string Get_Dept_TelNo(PsmhDb pDbCon, string ArgDept, string ArgDr)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TELNO FROM BAS_DOCTOR ";
            SQL += ComNum.VBLF + "  WHERE DRCODE    = '" + ArgDr + "' ";
            SQL += ComNum.VBLF + "    AND DRDEPT1   = '" + ArgDept + "' ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                DtFunc.Dispose();
                DtFunc = null;
                return null;
            }

            if (DtFunc.Rows.Count > 0)
                rtnVal = DtFunc.Rows[0]["TELNO"].ToString().Trim();

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 환자구분 변경 시 환자 입원 마스터 조회
        /// Author : 김민철
        /// Create Date : 2018.03.09
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <returns></returns>
        public DataTable sel_IpdMst_BasPat(PsmhDb pDbCon, string ArgPano)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            try
            {
                SQL = "";
                SQL += " SELECT a.IPDNO,a.Pano,a.Bi,b.Sname,a.Sex,a.Age,a.GbSTS,       \r\n";
                SQL += "        a.GbSpc,a.GbSPC2,a.Pname,b.Jumin1,b.Jumin2,b.Jumin3,   \r\n";
                SQL += "        TO_CHAR(a.InDate, 'yyyy-mm-dd') InDate,                \r\n";
                SQL += "        TO_CHAR(a.OutDate, 'yyyy-mm-dd') OutDate,              \r\n";
                SQL += "        a.AMSET1,a.AMSET4,a.AMSET5,a.AMSET6,a.AMSET7,          \r\n";
                SQL += "        a.AMSET8,a.AMSET9,a.AMSETA,a.GbGamek,a.GbSpc,GelCode,  \r\n";
                SQL += "        TO_CHAR(B.BIRTH,'YYYY-MM-DD') BIRTH                    \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a,                \r\n";
                SQL += "        " + ComNum.DB_PMPA + "BAS_PATIENT b                    \r\n";
                SQL += "  WHERE a.Pano   = '" + ArgPano + "'                           \r\n";
                SQL += "    AND a.ActDate IS NULL                                      \r\n"; //원무과 퇴원 미처리자
                SQL += "    AND a.GbSTS <> '9'                                         \r\n"; //입원취소
                SQL += "    AND a.Pano=b.Pano(+)                                       \r\n";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                return Dt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }
        }

        /// <summary>
        /// Description : 외래번호 SEQNO 가져오기
        /// Author : 박병규
        /// Create Date : 2018.03.19
        /// </summary>
        public long GET_NEXT_OPDNO(PsmhDb pDbCon)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            long rtnVal = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT KOSMOS_PMPA.SEQ_OPDNO.NEXTVAL OPDNO ";
            SQL += ComNum.VBLF + "   FROM DUAL";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            rtnVal = long.Parse(DtFunc.Rows[0]["OPDNO"].ToString());

            DtFunc.Dispose();
            DtFunc = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : OCS에서 @~ 발생코드로 본인부담율 가져오기
        /// Author : 박병규
        /// Create Date : 2018.3.19
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgSuCode">수가코드</param>
        /// <param name="ArgOrderNo">처방번호</param>
        /// </summary>
        public void Rtn_Input_Vcode(PsmhDb pDbCon, string ArgPtno, string ArgSuCode, double ArgOrderNo)
        {
            DataTable DtFunc = null;
            string SQL = "";
            string SqlErr = "";
            clsPmpaFunc cPF = new clsPmpaFunc();
            clsIpdAcct cIAcct = new clsIpdAcct();
            clsOumsad cPO = new ComPmpaLibB.clsOumsad();
            ComFunc CF = new ComFunc();
            clsAlert cA = new ComPmpaLibB.clsAlert();
            clsPmpaType.BonRate cBON = new clsPmpaType.BonRate();
            string strJuminNo = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT SUCODE, ORDERCODE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_IORDER ";
            SQL += ComNum.VBLF + "  WHERE PTNO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND ORDERNO   = " + ArgOrderNo + " ";
            SQL += ComNum.VBLF + "  union all ";
            SQL += ComNum.VBLF + " SELECT SUCODE, ORDERCODE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
            SQL += ComNum.VBLF + "  WHERE PTNO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND ORDERNO   = " + ArgOrderNo + " ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return;
            }

            if (DtFunc.Rows.Count > 0)
            {
                //건강보험 유형 통합 (11,12,13 >> 11)
                cBON.BI = clsPmpaType.TOM.Bi;
                if (VB.Left(cBON.BI, 1) == "1") { cBON.BI = "11"; }
                cBON.SDATE = clsPmpaType.TOM.BDate;
                cBON.VCODE = "";
                //기준일자 세팅
                strJuminNo = clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2;
                
                //입원시 면제코드 구분
                cBON.OGPDBUN = "";
                cBON.MCODE = clsPmpaType.TOM.MCode;
                //@+V코드
                if (VB.Left(ArgSuCode.Trim(), 2) != "$$" && VB.Left(ArgSuCode.Trim(), 2) != "##")
                {
                    cBON.VCODE = DtFunc.Rows[0]["SUCODE"].ToString().Trim();
                    cBON.VCODE = VB.Mid(cBON.VCODE, 2, cBON.VCODE.Length);
                }

                //특정기호 구분(01 고위험, 02 임산부외래, 03 저체중조산아)
                cBON.FCODE = clsVbfunc.GetBCodeCODE(pDbCon, "BAS_특정기호", clsPmpaType.TOM.BDate, ArgSuCode);
                cBON.DEPT = clsPmpaType.TOM.DeptCode;

                if (cBON.FCODE == "")
                {
                    if (clsPmpaType.TOM.JinDtl == "22")
                        cBON.FCODE = "03";
                    else if (clsPmpaType.TOM.JinDtl == "25")
                        cBON.FCODE = "02";
                }

                //2018.06.20. 박병규 : 건강보험 DT 틀니, 임플란트
                if (VB.Left(cBON.BI, 1) == "1" && cBON.DEPT == "DT")
                {
                    if (clsPmpaType.TOM.JinDtl != "02" || clsPmpaType.TOM.JinDtl != "07")
                        cBON.DEPT = "**";
                }

                cBON.IO = "I";
                //나이구분(0 성인, 1 신생아, 2 6세미만, 3 6세이상15세미만, 4 65세이상)
                cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaType.TOM.Age, strJuminNo, clsPmpaType.TOM.BDate, cBON.IO);
                //***입원 본인부담율 세팅
                if (cIAcct.Read_IBon_Rate(pDbCon, cBON) == false)
                {
                    if (cBON.DEPT == "ER") { cA.Alert_BonRate(cBON); }
                }
                //cA.Alert_BonRate(cBON);

                if (clsPmpaType.TOM.Bohun == "3")   //장애인은 본인부담율 재조정 
                {
                    if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                    {
                        clsPmpaType.IBR.Jin = 0;
                        clsPmpaType.IBR.Bohum = 0;
                        clsPmpaType.IBR.CTMRI = 0;
                    }
                }

                //2018.05.31 박병규 : 입원본인부담율 구하면서 cBON 변수값을 치환시키므로 외래본인부담율 구할때 다시 조건을 설정해준다
                //건강보험 유형 통합 (11,12,13 >> 11)
                cBON.BI = clsPmpaType.TOM.Bi;
                if (VB.Left(cBON.BI, 1) == "1") { cBON.BI = "11"; }
                cBON.SDATE = clsPmpaType.TOM.BDate;
                cBON.VCODE = "";
                //기준일자 세팅
                strJuminNo = clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2;
                
                //입원시 면제코드 구분
                cBON.OGPDBUN = "";
                cBON.MCODE = clsPmpaType.TOM.MCode;
                //@+V코드
                if (VB.Left(ArgSuCode.Trim(), 2) != "$$" && VB.Left(ArgSuCode.Trim(), 2) != "##")
                {
                    cBON.VCODE = DtFunc.Rows[0]["SUCODE"].ToString().Trim();
                    cBON.VCODE = VB.Mid(cBON.VCODE, 2, cBON.VCODE.Length);
                }

                //특정기호 구분(01 고위험, 02 임산부외래, 03 저체중조산아)
                cBON.FCODE = clsVbfunc.GetBCodeCODE(pDbCon, "BAS_특정기호", clsPmpaType.TOM.BDate, ArgSuCode);
                cBON.DEPT = clsPmpaType.TOM.DeptCode;

                if (cBON.FCODE == "")
                {
                    if (clsPmpaType.TOM.JinDtl == "22")
                        cBON.FCODE = "03";
                    else if (clsPmpaType.TOM.JinDtl == "25")
                        cBON.FCODE = "02";
                }

                //2018.06.20. 박병규 : 건강보험 DT 틀니, 임플란트
                if ( cBON.DEPT == "DT")
                {
                    if (clsPmpaType.TOM.JinDtl != "02" && clsPmpaType.TOM.JinDtl != "07")
                        cBON.DEPT = "**";
                }


                cBON.IO = "O";
                //나이구분(0 성인, 1 신생아, 2 6세미만, 3 6세이상15세미만, 4 65세이상)
                cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaType.TOM.Age, strJuminNo, clsPmpaType.TOM.BDate, cBON.IO);
                cBON.JINDTL = clsPmpaType.TOM.JinDtl;

                //***외래 본인부담율 세팅
                if (cPO.Read_OBon_Rate(pDbCon, cBON) == false)
                    cA.Alert_BonRate(cBON);

                if (clsPmpaType.TOM.Bohun == "3")   //장애인은 본인부담율 재조정 
                {
                    if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                    {
                        clsPmpaType.OBR.Jin = 0;
                        clsPmpaType.OBR.Bohum = 0;
                        clsPmpaType.OBR.CTMRI = 0;
                    }
                }

            }

            DtFunc.Dispose();
            DtFunc = null;

            return;
        }

        public string Read_Drug_Flag(string ArgFlag)
        {
            string rtnVal = "";

            switch (ArgFlag)
            {
                case "0": rtnVal = "대기"; break;
                case "S": rtnVal = "정리중"; break;
                case "D": rtnVal = "취소"; break;
                case "Y": rtnVal = "심사완료"; break;
                case "P": rtnVal = "인쇄"; break;
                case "2": rtnVal = "동일과2매이상"; break;
            }

            return rtnVal;
        }

        public string Read_Drug_Bun(string ArgFlag)
        {
            string rtnVal = "";

            switch (ArgFlag)
            {
                case "11": rtnVal = "내복약"; break;
                case "12": rtnVal = "외용약"; break;
                case "20": rtnVal = "주사약"; break;
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 격리 병실 종류 리턴
        /// Author : 김민철
        /// Create Date : 2018.4.3
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argRoom"></param>
        /// <param name="arg"></param>
        /// <seealso cref="VbFunction : READ_ROOM_SPECIAL"/>
        /// <returns></returns>
        public string READ_ROOM_SPECIAL(PsmhDb pDbCon, string argRoom, [Optional]string arg)
        {
            DataTable dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT NAME FROM " + ComNum.DB_PMPA + "BAS_BCODE     ";
                SQL += ComNum.VBLF + "  WHERE GUBUN = 'BAS_격리실종류'                   ";
                SQL += ComNum.VBLF + "    AND CODE   = '" + argRoom + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (arg == "")
                    {
                        rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                    }
                    else
                    {
                        rtnVal = VB.Left(dt.Rows[0]["NAME"].ToString().Trim(), 2);

                        if (arg == "1")
                        {
                            rtnVal = "★" + rtnVal;
                        }
                        else
                        {
                            rtnVal = "★" + rtnVal + ComNum.VBLF;
                        }
                    }

                }

                dt.Dispose();
                dt = null;

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 진료과 코드 및 진료과명 static 변수에 저장
        /// </summary>
        public void LOAD_DEPTCODE()
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            
            for (int i = 0; i < 50; i++)
            {
                clsPmpaPb.GstrSetDeptCodes[i] = "";
                clsPmpaPb.GstrSetDepts[i] = "";
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT * ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND GBJUPSU   = '1' ";
            SQL += ComNum.VBLF + "  ORDER BY PRINTRANKING ";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                clsPmpaPb.GstrSetDeptCodes[i] = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                clsPmpaPb.GstrSetDepts[i] = Dt.Rows[i]["DEPTNAMEK"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;
        }

        /// <summary>
        /// PC 프린트 출력 및 사용위치 정보
        /// </summary>
        public void Set_ClientPC()
        {
            ComFunc CF = new ComFunc();
            clsOrdFunction OF = new clsOrdFunction();

            bool blnSet = false;

            //환자인식밴드 출력위치 확인함.
            clsPmpaPb.GstrPrtBand = "";

            if (CF.Reg_Get_Setting("BASIC", "BAND") == "")
            {
                DialogResult result = ComFunc.MsgBoxQ("환자인식밴드 출력위치 확인이 필요합니다.", "설정확인");
                if (result == DialogResult.Yes) blnSet = true;
            }
            else
                clsPmpaPb.GstrPrtBand = VB.Left(CF.Reg_Get_Setting("BASIC", "BAND"), 1);


            //접수,수납 프로그램 위치 확인
            clsPmpaPb.GstrPrtBun = "";
            if (CF.Reg_Get_Setting("BASIC", "OUMSAD") == "")
            {
                DialogResult result = ComFunc.MsgBoxQ("접수/수납프로그램을 사용하는 위치 확인이 필요합니다.", "설정확인");
                if (result == DialogResult.Yes) blnSet = true;
            }
            else
                clsPmpaPb.GstrPrtBun = VB.Left(CF.Reg_Get_Setting("BASIC", "OUMSAD"), 1);

            if (blnSet == true)
            {
                if (DialogResult.Yes == ComFunc.MsgBoxQ("환경설정 화면으로 이동하시겠습니까?", "환경설정", MessageBoxDefaultButton.Button1))
                {
                    frmSetPmpaPC frm = new frmSetPmpaPC();
                    frm.ShowDialog();
                    OF.fn_ClearMemory(frm);
                }
            }

        }

        /// <summary>
        /// Description : 외래 접수번호 가져오기
        /// Author : 김민철
        /// Create Date : 2018.4.8
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgBDate"></param>
        /// <returns></returns>
        /// </summary>
        public long Get_Opd_Master_OpdNo(PsmhDb pDbCon, string ArgPano, string ArgDept, string ArgBDate)
        {
            long rtnVal = 0;

            DataTable Dt = null;
            string SQL = "";
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += " SELECT OPDNO FROM " + ComNum.DB_PMPA + "OPD_MASTER         \r\n";
                SQL += "  WHERE PANO = '" + ArgPano + "'                            \r\n";
                SQL += "    AND DEPTCODE = '" + ArgDept + "'                        \r\n";
                SQL += "    AND BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')        ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }

                if (Dt.Rows.Count > 0)
                {
                    rtnVal = Convert.ToInt64(VB.Val(Dt.Rows[0]["OPDNO"].ToString()));
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
        }

        /// <summary>
        /// 나이가산 전 주민번호가 외국인이나 시설환자 인지 체크
        /// 주민번호가 날짜형식이 아닌경우 체크
        /// </summary>
        /// <param name="ArgJumin"></param>
        /// <returns></returns>
        public string Chk_JuminNo_BirthDay(string ArgJumin)
        {
            string rtnVal = string.Empty;

            //주민번호 월 체크 01월 ~ 12월 범위에 속하지 않으면 Error
            if (string.Compare(VB.Mid(ArgJumin, 3, 2), "01") < 0 || string.Compare(VB.Mid(ArgJumin, 3, 2), "12") > 0)
            {
                rtnVal = "NO";
            }

            //2월생이면서 29일이 넘어가는 경우 Error
            if (VB.Mid(ArgJumin, 3, 2) == "02" && string.Compare(VB.Mid(ArgJumin, 5, 2), "29") > 0)
            {
                rtnVal = "NO";
            }

            //31일이 넘어가는 경우 Error
            if (string.Compare(VB.Mid(ArgJumin, 5, 2), "31") > 0)
            {
                rtnVal = "NO";
            }

            return rtnVal;
        }

        /// <summary>
        /// Description : 계약처 목록중 계약처명을 가져온다.
        /// Author : 박병규
        /// Create Date : 2018.05.24
        /// <param name="ArgGelCode"></param>
        /// <param name="ArgClass">MIACLASS 조건 사용여부</param>
        /// </summary>
        /// <seealso cref="READ_BAS_MIA" 사용 시 ArgClass = false />

        public string Read_MiaName(PsmhDb pDbCon, string ArgGelCode)
        {
            DataTable DtFunc = new DataTable();
            string SQL = String.Empty;
            string SqlErr = String.Empty;
            string strVal = String.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT MIACODE, MIANAME, MiaDetail, ";
                SQL += ComNum.VBLF + "        TO_CHAR(DelDate,'YYYY-MM-DD') DelDate ";
                SQL += ComNum.VBLF + "   FROM BAS_MIA ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND MIACODE   = '" + ArgGelCode + "' ";

                if (ArgGelCode.Trim() != "5000")
                    SQL += ComNum.VBLF + "   AND MIADETAIL = '99' ";

                SQL += ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE = '') ";
                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

                if (DtFunc.Rows.Count == 0)
                {
                    DtFunc.Dispose();
                    DtFunc = null;
                    return strVal;
                }

                if (DtFunc.Rows.Count > 0)
                {
                    clsPmpaPb.GstrMiaFlag = "OK";
                    clsPmpaPb.GstrMiaDetail = DtFunc.Rows[0]["MiaDetail"].ToString().Trim();

                    if (DtFunc.Rows[0]["DELDATE"].ToString().Trim() != "")
                    {
                        clsPmpaPb.GstrMiaFlag = "NO";
                        strVal = "계약이 해지됨";
                    }
                    else
                    {
                        strVal = DtFunc.Rows[0]["MIANAME"].ToString().Trim();
                    }
                }
                else
                    strVal = "";

                DtFunc.Dispose();
                DtFunc = null;

                return strVal;
            }
            catch (Exception ex)
            {
                strVal = "";
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strVal;
            }
        }

        public void Screen_Display_Secondary(Form frm)
        {
            Screen[] screens = Screen.AllScreens;

            if (screens.Length > 1)
            {
                Screen scrn = (screens[0].WorkingArea.Contains(frm.Location))
                                         ? screens[1] : screens[0];
                frm.Show();
                frm.Location = new System.Drawing.Point(scrn.Bounds.Left, 0);
                frm.WindowState = FormWindowState.Normal;
            }
        }

        public string Convert_GbNgt2(clsPmpaType.cBas_Add_Arg cBArg, string ArgHang, bool bOG)
        {
            string rtnVal = "";
            string strGbChild = "0";
            string strPCodeDtl = string.Empty;
            string strHang = "";
            string strNgt = "0";

            // 나이구분                       // ArgHang  
            // 0.성인                         // 01.진찰료
            // 1.신생아                       // 02.약제조제료
            // 2.만1세미만                    // 03.주사수기료
            // 3.만1세이상 - 만6세미만        // 04.마취료
            // 4.만6세미만                    // 05.처치수술료
            // 5.만70세이상                   // 06.검사료
            // 6.만35세이상(분만수가)         // 07.영상진단료

            try
            {
                if (cBArg.MIDNIGHT != "")
                {
                    rtnVal = "D";
                    return rtnVal;
                }

                //Hang = 1.수술, 2.부수술, 3.제2수술, 4.마취
                //GbChild = 0.성인, 1.신생아, 2.소아, 노인  
                GBNGT[] GBNIGHT =
                {
                    #region Linq Data Set
		            //성인
                    new GBNGT() { Night = "0", GbChild = "0", Hang = "1", GbNgt2 = "0" },
                    new GBNGT() { Night = "1", GbChild = "0", Hang = "1", GbNgt2 = "1" },
                    new GBNGT() { Night = "2", GbChild = "0", Hang = "1", GbNgt2 = "2" },

                    new GBNGT() { Night = "0", GbChild = "0", Hang = "2", GbNgt2 = "5" },
                    new GBNGT() { Night = "1", GbChild = "0", Hang = "2", GbNgt2 = "7" },
                    new GBNGT() { Night = "2", GbChild = "0", Hang = "2", GbNgt2 = "6" },

                    new GBNGT() { Night = "0", GbChild = "0", Hang = "3", GbNgt2 = "A" },
                    new GBNGT() { Night = "1", GbChild = "0", Hang = "3", GbNgt2 = "C" },
                    new GBNGT() { Night = "2", GbChild = "0", Hang = "3", GbNgt2 = "B" },

                    new GBNGT() { Night = "0", GbChild = "0", Hang = "4", GbNgt2 = "0" },
                    new GBNGT() { Night = "1", GbChild = "0", Hang = "4", GbNgt2 = "1" },
                    new GBNGT() { Night = "2", GbChild = "0", Hang = "4", GbNgt2 = "2" },

                     //신생아
                    new GBNGT() { Night = "0", GbChild = "1", Hang = "1", GbNgt2 = "0" },
                    new GBNGT() { Night = "1", GbChild = "1", Hang = "1", GbNgt2 = "1" },
                    new GBNGT() { Night = "2", GbChild = "1", Hang = "1", GbNgt2 = "2" },

                    new GBNGT() { Night = "0", GbChild = "1", Hang = "2", GbNgt2 = "5" },
                    new GBNGT() { Night = "1", GbChild = "1", Hang = "2", GbNgt2 = "7" },
                    new GBNGT() { Night = "2", GbChild = "1", Hang = "2", GbNgt2 = "6" },

                    new GBNGT() { Night = "0", GbChild = "1", Hang = "3", GbNgt2 = "A" },
                    new GBNGT() { Night = "1", GbChild = "1", Hang = "3", GbNgt2 = "C" },
                    new GBNGT() { Night = "2", GbChild = "1", Hang = "3", GbNgt2 = "B" },

                    new GBNGT() { Night = "0", GbChild = "1", Hang = "4", GbNgt2 = "6" },
                    new GBNGT() { Night = "1", GbChild = "1", Hang = "4", GbNgt2 = "8" },
                    new GBNGT() { Night = "2", GbChild = "1", Hang = "4", GbNgt2 = "7" },

                    //소아, 노인
                    new GBNGT() { Night = "0", GbChild = "2", Hang = "1", GbNgt2 = "3" },
                    new GBNGT() { Night = "1", GbChild = "2", Hang = "1", GbNgt2 = "1" },
                    new GBNGT() { Night = "2", GbChild = "2", Hang = "1", GbNgt2 = "2" },

                    new GBNGT() { Night = "0", GbChild = "2", Hang = "2", GbNgt2 = "5" },
                    new GBNGT() { Night = "1", GbChild = "2", Hang = "2", GbNgt2 = "7" },
                    new GBNGT() { Night = "2", GbChild = "2", Hang = "2", GbNgt2 = "6" },

                    new GBNGT() { Night = "0", GbChild = "2", Hang = "3", GbNgt2 = "A" },
                    new GBNGT() { Night = "1", GbChild = "2", Hang = "3", GbNgt2 = "C" },
                    new GBNGT() { Night = "2", GbChild = "2", Hang = "3", GbNgt2 = "B" },

                    new GBNGT() { Night = "0", GbChild = "2", Hang = "4", GbNgt2 = "3" },
                    new GBNGT() { Night = "1", GbChild = "2", Hang = "4", GbNgt2 = "5" },
                    new GBNGT() { Night = "2", GbChild = "2", Hang = "4", GbNgt2 = "4" }, 
	                #endregion
                };

                clsBasAcct cBAcct = new clsBasAcct();

                #region Data Setting
                strGbChild = cBAcct.Bas_Add_Age_Set(cBArg.AGE, cBArg.AGEILSU, cBArg.BDATE, false, bOG, ArgHang);     //나이구분

                if (string.Compare(strGbChild, "2") >= 0 && string.Compare(strGbChild, "5") <= 0)
                {
                    strGbChild = "2";
                }
                else if (strGbChild == "6")
                {
                    strGbChild = "0";
                }

                if (ArgHang == "04")
                {
                    strHang = "4";
                }
                else if (ArgHang == "05" && cBArg.BUN == "34")
                {
                    if (cBArg.OP3 == "1")
                    {
                        strHang = "2";  //부수술
                    }
                    else if (cBArg.OP3 == "2")
                    {
                        strHang = "3";  //제2수술
                    }
                    else
                    {
                        strHang = "1";
                    }
                }

                if (cBArg.NIGHT == "")
                {
                    strNgt = "0";
                }
                else
                {
                    strNgt = cBArg.NIGHT;
                } 
                #endregion

                //LINQ 실행 쿼리
                var Night = from NGT in GBNIGHT
                            where (NGT.Hang == strHang && NGT.GbChild  == strGbChild && NGT.Night == strNgt)
                            select NGT;

                //LINQ 실행 부분
                foreach (var n in Night)
                {
                    rtnVal = n.GbNgt2;
                }
                

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.ToString());
                return "";
            }
        }
    }
}
