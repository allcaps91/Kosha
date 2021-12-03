using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComDbB; //DB연결
using ComLibB;
using FarPoint.Win.Spread;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// 작성자 : 이상훈
    /// 처방 Display Class
    /// </summary>
    public class clsOrdDisp 
    {
        public static int rowcounter;
        public static string SQL;
        public static DataTable dt = null;
        public static DataTable dt1 = null;
        public static DataTable dt2 = null;
        public static string SqlErr = string.Empty; //에러문 받는 변수
        //int intRowAffected = 0; //변경된 Row 받는 변수

        clsSpread SP = new clsSpread();
        clsDB DB = new clsDB();
        ComFunc CF = new ComFunc();
        clsOrdFunction OF = new clsOrdFunction();

        //FrmMedViewSlipSub FrmMedViewSlipSub = null;

        public static int GOrdCnt = 0;



        /// <summary>
        /// CP오더 삭제 함수
        /// 2021-08-12 생성
        /// </summary>
        public static void CP_ORDER_REMOVE(string CPNO, string GBN)
        {
            if (CP_OPTION_VALUE("자동D/C") == false)
                return;

            MTSResult result = new MTSResult(true);
            try
            {
                #region 삭제백업---------------------
                MParameter parameter = new MParameter();
                parameter.AppendSql(" INSERT INTO KOSMOS_OCS.OCS_IORDER_DEL ( PTNO,BDATE,SEQNO,DEPTCODE,DRCODE,STAFFID,SLIPNO,ORDERCODE,SUCODE,BUN   ");
                parameter.AppendSql("      , GBORDER,CONTENTS,BCONTENTS,REALQTY,QTY,REALNAL,NAL,DOSCODE,GBINFO,GBSELF,GBSPC                          ");
                parameter.AppendSql("      , GBNGT,GBER,GBPRN,GBDIV,GBBOTH,GBACT,GBTFLAG,GBSEND,GBPOSITION,GBSTATUS,NURSEID                          ");
                parameter.AppendSql("      , ENTDATE,WARDCODE,ROOMCODE,BI,ORDERNO,REMARK,ACTDATE,GBGROUP,GBPORT,ORDERSITE                            ");
                parameter.AppendSql("      , MULTI,MULTIREMARK,DUR,LABELPRINT,ACTDIV,GBSEND_OORDER,GBPICKUP,PICKUPSABUN                              ");
                parameter.AppendSql("      , PICKUPDATE,EMRSET,GBIOE,MAYAK,POWDER,SEDATION,DCDIV,DIVQTY,NURREMARK,DRORDERVIEW,CERTNO                 ");
                parameter.AppendSql("      , CONSULT , MAYAKREMARK, VER, iP, AUTO_SEND, GBSPC_NO,DELTIME,DELGBN,ENTDATE2,GbTax,VerbC,GbVerb,Verbal   ");
                parameter.AppendSql("      , V_ORDERNO,PRN_REMARK,ASA, ER24, GSADD,TUYEOPOINT, TUYEOTIME )                                           ");
                parameter.AppendSql(" SELECT PTNO,BDATE,SEQNO,DEPTCODE,DRCODE,STAFFID,SLIPNO,ORDERCODE,SUCODE,BUN                                    ");
                parameter.AppendSql("      , GBORDER,CONTENTS,BCONTENTS,REALQTY,QTY,REALNAL,NAL,DOSCODE,GBINFO,GBSELF,GBSPC                          ");
                parameter.AppendSql("      , GBNGT,GBER,GBPRN,GBDIV,GBBOTH,GBACT,GBTFLAG,GBSEND,GBPOSITION,GBSTATUS,NURSEID                          ");
                parameter.AppendSql("      , ENTDATE,WARDCODE,ROOMCODE,BI,ORDERNO,REMARK,ACTDATE,GBGROUP,GBPORT,ORDERSITE                            ");
                parameter.AppendSql("      , MULTI,MULTIREMARK,DUR,LABELPRINT,ACTDIV,GBSEND_OORDER,GBPICKUP,PICKUPSABUN                              ");
                parameter.AppendSql("      , PICKUPDATE,EMRSET,GBIOE,MAYAK,POWDER,SEDATION,DCDIV,DIVQTY,NURREMARK,DRORDERVIEW,CERTNO                 ");
                parameter.AppendSql("      , CONSULT , MAYAKREMARK, VER, '" + clsCompuInfo.gstrCOMIP + "', AUTO_SEND, GBSPC_NO,SYSDATE,'9',ENTDATE2,GbTax,VerbC,GbVerb,Verbal      ");
                parameter.AppendSql("      , V_ORDERNO,PRN_REMARK,ASA, ER24, GSADD,TUYEOPOINT, TUYEOTIME                                             ");
                parameter.AppendSql("   FROM KOSMOS_OCS.OCS_IORDER A                                                                                 ");
                parameter.AppendSql(" WHERE EXISTS                                                                                                   ");
                parameter.AppendSql(" (                                                                                                              ");
                parameter.AppendSql("      SELECT 1                                                                                                  ");
                parameter.AppendSql("        FROM KOSMOS_OCS.OCS_CP_RECORD                                                                           ");
                parameter.AppendSql("       WHERE CPNO = :CPNO                                                                                       ");
                parameter.AppendSql("         AND PTNO = A.PTNO                                                                                      ");
                parameter.AppendSql(" 	      AND CASE WHEN :GBN = 'CP 처방중단' AND A.BDATE >= TO_DATE(CANCERDATE, 'YYYYMMDD')  + 1 THEN 1            ");
                parameter.AppendSql(" 	               WHEN :GBN = 'CP 제외'    AND A.BDATE >= TO_DATE(DROPDATE  , 'YYYYMMDD')  + 1 THEN 1            ");
                parameter.AppendSql(" 	           END = 1                                                                                           ");
                parameter.AppendSql(" 	      AND A.GBSEND = '*'                                                                                     ");
                parameter.AppendSql(" 	      AND A.VER    = 'CPORDER'                                                                               ");
                parameter.AppendSql(" 	      AND A.PICKUPDATE IS NULL                                                                               ");
                parameter.AppendSql(" )                                                                                                              ");

                parameter.Add("CPNO", CPNO, OracleDbType.Int32);
                parameter.Add("GBN", GBN);

                result.SetSuccessCountPlus(clsDB.ExecuteNonQuery(parameter, clsDB.DbCon));
                #endregion

                parameter = new MParameter();
                parameter.AppendSql("DELETE KOSMOS_OCS.OCS_IORDER  A                                                                                ");
                parameter.AppendSql(" WHERE EXISTS                                                                                                  ");
                parameter.AppendSql(" (                                                                                                             ");
                parameter.AppendSql("      SELECT 1                                                                                                 ");
                parameter.AppendSql("        FROM KOSMOS_OCS.OCS_CP_RECORD                                                                          ");
                parameter.AppendSql("       WHERE CPNO = :CPNO                                                                                      ");
                parameter.AppendSql("         AND PTNO = A.PTNO                                                                                     ");
                parameter.AppendSql(" 	      AND CASE WHEN :GBN = 'CP 처방중단'  AND A.BDATE >= TO_DATE(CANCERDATE, 'YYYYMMDD')  + 1 THEN 1          ");
                parameter.AppendSql(" 	               WHEN :GBN = 'CP 제외'     AND A.BDATE >= TO_DATE(DROPDATE  , 'YYYYMMDD')  + 1 THEN 1          ");
                parameter.AppendSql(" 	           END = 1                                                                                          ");
                parameter.AppendSql(" )                                                                                                             ");
                parameter.AppendSql("   AND A.GBSEND = '*'                                                                                          ");
                parameter.AppendSql("   AND A.VER    = 'CPORDER'                                                                                    ");
                parameter.AppendSql("   AND A.PICKUPDATE IS NULL                                                                                    ");

                parameter.Add("CPNO", CPNO, OracleDbType.Int32);
                parameter.Add("GBN", GBN);

                result.SetSuccessCountPlus(clsDB.ExecuteNonQuery(parameter, clsDB.DbCon));

                result.SetSuccessMessage("성공");
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                result.SetErrMessage("실패");
            }
        }

        /// <summary>
        /// 수가매핑 시행여부
        /// </summary>
        /// <returns></returns>
        public static bool SUGA_MAAPING_YN()
        {
            MParameter parameter = new MParameter();
            bool rtnVal = false;

            try
            {
                parameter.AppendSql("SELECT '1'                                        ");
                parameter.AppendSql("  FROM DUAL                                       ");
                parameter.AppendSql(" WHERE EXISTS                                     ");
                parameter.AppendSql(" (                                                ");
                parameter.AppendSql("      SELECT 1                                    ");
                parameter.AppendSql("        FROM KOSMOS_PMPA.BAS_BCODE                ");
                parameter.AppendSql("       WHERE GUBUN = 'SUNAP_수가매핑시행여부'        ");
                parameter.AppendSql("         AND NAME  = 'Y'                          ");
                parameter.AppendSql(" )                                                ");

                if (clsDB.ExecuteScalar<string>(parameter, clsDB.DbCon) != null)
                {
                    rtnVal = true;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }

            return rtnVal;
        }


        /// <summary>
        /// CP 연계 기능 사용여부
        /// </summary>
        /// <returns></returns>
        public static bool CP_OPTION_VALUE(string CODE)
        {
            MParameter parameter = new MParameter();
            bool rtnVal = false;

            try
            {
                parameter.AppendSql("SELECT '1'                                        ");
                parameter.AppendSql("  FROM DUAL                                       ");
                parameter.AppendSql(" WHERE EXISTS                                     ");
                parameter.AppendSql(" (                                                ");
                parameter.AppendSql("      SELECT 1                                    ");
                parameter.AppendSql("        FROM KOSMOS_PMPA.BAS_BCODE                ");
                parameter.AppendSql("       WHERE GUBUN = 'CP_시행'                     ");
                parameter.AppendSql("         AND CODE  = :CODE                        ");
                parameter.AppendSql("         AND NAME  = 'Y'                          ");
                parameter.AppendSql(" )                                                ");

                parameter.Add("CODE", CODE);

                if (clsDB.ExecuteScalar<string>(parameter, clsDB.DbCon) != null)
                {
                    rtnVal = true;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }

            return rtnVal;
        }

        public static DataTable OrdDisplay(string Pano, string BFDate, string BTDate, string DeptCode, string DrCode, string GBJOB, long WrtNo, string strBuCode, string GBIO)
        {
            DataTable rtndt = null;

            switch (GBJOB)
            {
                case "OPD":
                    rtndt = fn_OpdOrdDisp(Pano, BFDate, BTDate, DeptCode, DrCode);
                    break;
                case "ER":
                    rtndt = fn_ErOrdDisp(Pano, BFDate, BTDate, DeptCode, DrCode);
                    break;
                case "IPD":
                    rtndt = fn_IpdOrdDisp(Pano, BFDate, BTDate, DeptCode, DrCode);
                    break;
                default:
                    rtndt = null;
                    break;
            }

            return rtndt;
        }

        public static DataTable fn_OpdOrdDisp(string Pano, string BFDate, string BTDate, string DeptCode, string DrCode)
        {
            return Read_Orders("OPD", clsOrdFunction.Pat.PtNo, clsOrdFunction.Pat.DeptCode, clsOrdFunction.Pat.DrCode);
        }

        public static DataTable fn_ErOrdDisp(string Pano, string BFDate, string BTDate, string DeptCode, string DrCod)
        {
            return Read_Orders("ER", clsOrdFunction.Pat.PtNo, clsOrdFunction.Pat.DeptCode, clsOrdFunction.Pat.DrCode);
        }

        public static DataTable fn_IpdOrdDisp(string Pano, string BFDate, string BTDate, string DeptCode, string DrCod)
        {
            return Read_Orders("IPD", clsOrdFunction.Pat.PtNo, clsOrdFunction.Pat.DeptCode, clsOrdFunction.Pat.DrCode);
        }

        public static DataTable fn_OprOrdDisp(string Pano, string BFDate, string BTDate, string DeptCode, string DrCod, long WrtNo, string strBuCode, string GBIO)
        {
            DataTable rtnDt = null;

            try
            {
                SQL = "";
                SQL += " SELECT '' chk, a.GuBun, a.JepCode, SUM(Qty) Qty, GbSelf, a.GbSunap, b.buse_unit Unit                       \r";
                SQL += "      , '' SuName, b.jepname, 'X',a.GbSusul, a.SuCode, SuBun, a.DeptCode, a.JepCode JepCode1, CodeGbn       \r";
                SQL += "      , a.SuCode SuCode1, SuBun SuBun1, Qty Qty1, GbSelf GbSelf1, a.GbSusul GbSusul1, GbNgt                 \r";
                SQL += "      , a.GbSunap GbSunap1, OpRoom, TO_CHAR(OpDate,'YYYY-MM-DD') OpDate, IpdOpd, CodeGbn CodeGbn1           \r";
                SQL += "      , GuBun GuBun1, '', a.bucode                                                                          \r";
                SQL += "   FROM KOSMOS_PMPA.ORAN_SLIP a                                                                             \r";
                SQL += "      , KOSMOS_ADM.ORD_JEP    b                                                                             \r";
                SQL += "  WHERE a.WRTNO  = '" + WrtNo + "'                                                                          \r";
                SQL += "    AND a.jepcode = b.jepcode                                                                               \r";
                //SQL += "    AND a.BuCode = '" + strBuCode + "'                                                                    \r";
                SQL += "  GROUP BY a.GuBun, a.JepCode, GbSelf, a.GbSunap, b.buse_unit                                               \r";
                SQL += "         , b.jepname, a.GbSusul, a.SuCode, SuBun, a.DeptCode, a.JepCode, CodeGbn, a.SuCode                  \r";
                SQL += "         , SuBun, Qty, GbSelf, a.GbSusul, GbNgt, a.GbSunap                                                  \r";
                SQL += "         , OpRoom, TO_CHAR(OpDate, 'YYYY-MM-DD') , IpdOpd, CodeGbn, GuBun, a.GuBun , a.bucode               \r";
                SQL += "  ORDER BY Gubun,JepCode                                                                                    \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }

                rowcounter = 0;
                rowcounter = dt.Rows.Count;

                if (rowcounter > 0)
                {
                    rtnDt = dt;
                }

                dt.Dispose();
                dt = null;
                return rtnDt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }
        }

        /// <summary>
        /// 처방 Display
        /// </summary>
        public static DataTable Read_Orders(string strGBIO, string strPano, string strDeptCode, string strDrCode)
        {
            DataTable rtnDt = null;
            OracleDataReader reader = null;

            int nReadOrder;
            string strBDateChk = "N";
            string cMaxBDate = "";

            nReadOrder = clsOrdFunction.GnReadOrder;

            clsOrdFunction.GnTuyakno = 0;
            clsOrdFunction.GnReadIlls = 0;

            //clsOrdFunction.GnReadOrder = 0;
            //clsOrdFunction.GnReadOrder2 = 0;
            //clsOrdFunction.GnJinOrdCount = 0;

            if (strGBIO == "OPD")
             {
                #region //OPD
                if (clsOrdFunction.GstrReserved != "OK")
                {
                    try
                    {
                        SQL = "";
                        SQL += " SELECT * FROM KOSMOS_OCS.OCS_OORDER                            \r";      //'당일 예약 Order Search
                        SQL += "  WHERE Ptno     = '" + strPano + "'                            \r";
                        SQL += "    AND BDate    = TO_DATE('" + clsOrdFunction.GstrBDate + "' ,'YYYY-MM-DD')   \r";
                        SQL += "    AND DeptCode = '" + strDeptCode + "'                        \r";
                        if (strDrCode == "1107  " || strDrCode == "1125  ")
                        {
                            SQL += "    AND DRCODE = '" + strDrCode.Trim() + "'                 \r";
                        }
                        else
                        {
                            SQL += "    AND DRCODE NOT IN  ('1107','1125')                      \r";
                        }
                        SQL += "    AND Seqno    = '0'                                          \r";
                        clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 오류가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        }

                        rowcounter = 0;
                        rowcounter = dt.Rows.Count;

                        if (rowcounter > 0)
                        {
                            clsOrdFunction.Pat.RDrCode = VB.Left(dt.Rows[0]["DRCODE"].ToString(), 8);
                            clsOrdFunction.Pat.RDATE = dt.Rows[0]["ORDERCODE"].ToString();

                            clsOrdFunction.Pat.RTime = dt.Rows[0]["SUCODE"].ToString();
                            clsOrdFunction.Pat.Exam = dt.Rows[0]["GBINFO"].ToString();
                            clsOrdFunction.GstrReserved = "YEYAK";
                            clsOrdFunction.Pat.ResSMSNot = clsBagage.CHECK_RES_SMSNOTSEND2(clsDB.DbCon, clsOrdFunction.Pat.PtNo, clsOrdFunction.GstrBDate, clsOrdFunction.Pat.DeptCode);

                        }
                        dt.Dispose();
                        dt = null;
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                }

                try
                {
                    SQL = "";
                    SQL += " SELECT '' cDispHeader, d.remark cOrderName, '' cDispRGB                                \r";
                    SQL += "      , '' cGbBoth, '' cGbInfo, '' cDrugName                                            \r";
                    SQL += "      , '' cGbQty, '' cGbDosage, '' cNextCode, '' DAICODE                               \r";
                    SQL += "      , '' cOrderNameS, '' cGbImiv, '' SuGbF, '' BunSu                                  \r";
                    SQL += "      , d.GBCOPY,    d.GbSunap, d.GBAUTOSEND, d.GBAUTOSEND2, d.Bun,      d.Tuyakno      \r";
                    SQL += "      , d.OrderCode, d.RealQty, d.GbDiv,      d.Nal,         d.GBER,     d.GbSelf       \r";
                    SQL += "      , d.Remark,    d.RES,     d.Multi,      d.Sucode,      d.Slipno,   d.DosCode      \r";
                    SQL += "      , d.GbBoth,    d.Gbinfo,  d.OrderNo,    d.DRCode,      d.MultiRemark              \r";
                    SQL += "      , d.DUR,       d.Resv,    d.GbTax,      d.ScodeRemark, d.GBSPC_NO, d.ScodeSayu    \r";
                    SQL += "      , d.GbFM,      d.Sabun,   d.OCSDRUG,    d.ASA,         d.PCHASU,   d.SUBUL_WARD   \r";
                    SQL += "      , KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(d.DOSCODE) DOSNAME                               \r";
                    SQL += "      , KOSMOS_OCS.FC_OCS_OSPECIMAN_NAME(d.DOSCODE, d.SLIPNO) SPECNAME                  \r";
                    SQL += "      , '' CBUN, d.ROWID                                                                \r";
                    SQL += "      , d.BURNADD, d.OPGUBUN                                                            \r";
                    SQL += "      , d.seqno seqno                                                                   \r";
                    SQL += "      , d.POWDER POWDER                                                                 \r";
                    SQL += "      , d.SEDATION                                                                      \r";
                    SQL += "      , '' DISPHEADER                                                                   \r";
                    SQL += "      , '' NEXTCODE                                                                     \r";
                    //2020-12-29
                    SQL += "      , d.CONTENTS, d.BCONTENTS, d.GBGROUP                                              \r";
                    //2021-01-08
                    SQL += "      , d.TUYEOPOINT, d.TUYEOTIME                                                       \r";
                    SQL += "   FROM KOSMOS_OCS.OCS_OORDER d                                                         \r";
                    SQL += "  WHERE d.Ptno = '" + clsOrdFunction.Pat.PtNo + "'                                      \r";
                    SQL += "    AND d.BDate = TO_DATE('" + clsOrdFunction.GstrBDate + "', 'YYYY-MM-DD')             \r";
                    SQL += "    AND d.DeptCode  = '" + clsOrdFunction.Pat.DeptCode.Trim() + "'                      \r";
                    if (clsOrdFunction.Pat.DrCode == "1107  " || clsOrdFunction.Pat.DrCode == "1125  ") //(1107 : 오동호, 1125 : 최정란)
                    {
                        SQL += "     AND d.DRCODE = '" + clsOrdFunction.Pat.DrCode.Trim() + "'                      \r";
                    }
                    else
                    {
                        SQL += "    AND d.DRCODE NOT IN('1107', '1125')                                             \r";
                    }
                    SQL += "    AND d.Seqno    > '0'                                                                \r";
                    SQL += "    AND d.NAL      > '0'                                                                \r";
                    SQL += "    AND D.ORDERCODE IN('S/O', 'V/S')                                                    \r";
                    SQL += " UNION ALL                                                                              \r";
                    SQL += " SELECT a.DispHeader cDispHeader, a.OrderName cOrderName, a.DispRGB cDispRGB            \r";
                    SQL += "      , a.GbBoth cGbBoth, a.GbInfo cGbInfo, a.DrugName cDrugName                        \r";
                    SQL += "      , a.GbQty cGbQty, a.GbDosage cGbDosage, a.NextCode cNextCode, C.DAICODE           \r";
                    SQL += "      , a.OrderNameS cOrderNameS, a.GbImiv cGbImiv, b.SuGbF, b.Bun BunSu                \r";
                    SQL += "      , d.GBCOPY,    d.GbSunap, d.GBAUTOSEND, d.GBAUTOSEND2, d.Bun,      d.Tuyakno      \r";
                    SQL += "      , d.OrderCode, d.RealQty, d.GbDiv,      d.Nal,         d.GBER,     d.GbSelf       \r";
                    SQL += "      , d.Remark,    d.RES,     d.Multi,      d.Sucode,      d.Slipno,   d.DosCode      \r";
                    SQL += "      , d.GbBoth,    d.Gbinfo,  d.OrderNo,    d.DRCode,      d.MultiRemark              \r";
                    SQL += "      , d.DUR,       d.Resv,    d.GbTax,      d.ScodeRemark, d.GBSPC_NO, d.ScodeSayu    \r";
                    SQL += "      , d.GbFM,      d.Sabun,   d.OCSDRUG,    d.ASA,         d.PCHASU,   d.SUBUL_WARD   \r";
                    SQL += "      , DECODE(A.GBDOSAGE, '0', '', KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(d.DOSCODE)) DOSNAME  \r";
                    SQL += "      , KOSMOS_OCS.FC_OCS_OSPECIMAN_NAME(d.DOSCODE, d.SLIPNO) SPECNAME                  \r";
                    SQL += "      , a.CBUN, d.ROWID                                                                 \r";
                    SQL += "      , d.BURNADD, d.OPGUBUN                                                            \r";
                    SQL += "      , d.seqno seqno                                                                   \r";
                    SQL += "      , d.POWDER POWDER                                                                   \r";
                    SQL += "      , d.SEDATION                                                                   \r";
                    SQL += "      , a.DISPHEADER                                                                    \r";
                    SQL += "      , a.NEXTCODE                                                                      \r";
                    //2020-12-29
                    SQL += "      , d.CONTENTS, d.BCONTENTS, d.GBGROUP                                              \r";
                    //2021-01-08
                    SQL += "      , d.TUYEOPOINT, d.TUYEOTIME                                                       \r";
                    SQL += "   FROM KOSMOS_OCS.OCS_ORDERCODE a                                                      \r";
                    SQL += "      , KOSMOS_PMPA.BAS_SUT      b                                                      \r";
                    SQL += "      , KOSMOS_PMPA.BAS_SUN      c                                                      \r";
                    SQL += "      , KOSMOS_OCS.OCS_OORDER    d                                                      \r";
                    SQL += "  WHERE a.SuCode    = b.SuCode(+)                                                       \r";
                    SQL += "    AND a.SuCode    = C.SUNEXT(+)                                                       \r";
                    SQL += "    AND a.ORDERCODE = d.ORDERCODE(+)                                                    \r";
                    SQL += "    AND a.SLIPNO    = d.SLIPNO(+)                                                       \r";
                    SQL += "    AND d.Ptno      = '" + clsOrdFunction.Pat.PtNo + "'                                 \r";
                    SQL += "    AND d.BDate     = TO_DATE('" + clsOrdFunction.GstrBDate + "', 'YYYY-MM-DD')         \r";
                    SQL += "    AND d.DeptCode  = '" + clsOrdFunction.Pat.DeptCode.Trim() + "'                      \r";
                    if (clsOrdFunction.Pat.DrCode == "1107  " || clsOrdFunction.Pat.DrCode == "1125  ") //(1107 : 오동호, 1125 : 최정란)
                    {
                        SQL += "     AND d.DRCODE = '" + clsOrdFunction.Pat.DrCode.Trim() + "'                      \r";
                    }
                    else
                    {
                        SQL += "    AND d.DRCODE NOT IN('1107', '1125')                                             \r";
                    }
                    SQL += "    AND d.Seqno    >  0                                                                 \r";
                    SQL += "    AND d.NAL      >  0                                                                 \r";
                    SQL += "    AND D.ORDERCODE NOT IN('S/O', 'V/S')                                                \r";

                    //2021-01-05 안정수, 김양수과장 요청으로 SEQNO로 정렬되도록..
                    if(clsType.User.IdNumber == "53775")
                    {
                        SQL += "  ORDER BY GBSUNAP DESC, GBCOPY, GBAUTOSEND2, SEQNO                                 \r";
                    }
                    else
                    {
                        SQL += "  ORDER BY GBSUNAP DESC, GBCOPY, GBAUTOSEND2, SLIPNO, SEQNO                             \r";
                    }
                    
                    clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return null;
                    }

                    rowcounter = 0;
                    rowcounter = dt.Rows.Count;

                    if (rowcounter > 0)
                    {
                        clsOrdFunction.GnSunapOrdCount = dt.Rows.Count;
                        rtnDt = dt;
                    }
                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }
                #endregion //OPD
            }
            else if (strGBIO == "ER")
            {
                #region //ER
                if (clsOrdFunction.GnReadOrder == 0)
                {
                    try
                    {


                        SQL = "";
                        SQL += " SELECT TO_CHAR(MAX(BDate),'YYYY-MM-DD') MAXBDATE                                                               \r"; //전회 처방 일자 Search
                        SQL += "   FROM KOSMOS_OCS.OCS_IORDER                                                                                   \r";
                        SQL += "  WHERE PTNO   =  '" + clsOrdFunction.Pat.PtNo + "'                                                             \r";
                        SQL += "    AND BDate <= TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')                                       \r";
                        SQL += "    AND BDate >= TO_DATE('" + DateTime.Parse(clsOrdFunction.Pat.EntDate).ToShortDateString() + "','YYYY-MM-DD') \r";
                        SQL += "    AND GbStatus IN  (' ', 'D+')                                                                                \r";
                        SQL += "    AND (GBINFO <> 'TRANSFER' OR GbInfo IS NULL )                                                               \r";
                        SQL += "    AND GBIOE IN ('E','EI')                                                                                     \r";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 오류가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        }

                        if (reader.HasRows && reader.Read())
                        {
                            if (clsOrdFunction.GstrBDate.Trim().Equals(reader.GetValue(0).ToString().Trim()))
                            {
                                strBDateChk = "Y";
                            }

                            if (reader.GetValue(0).ToString().Trim().IsNullOrEmpty())
                            {
                                reader.Dispose();
                                reader = null;
                                return null;
                            }

                            cMaxBDate = reader.GetValue(0).ToString().Trim();
                        }
                        reader.Dispose();
                        reader = null;
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                }

                try
                {
                    SQL = "";
                    SQL += " SELECT NVL(d.GbSend , ' ') GbSend, d.EntDate EntDate, d.Seqno SEQNO, d.BDATE               \r";
                    SQL += "      , d.DEPTCODE, d.DRCODE, d.STAFFID, d.SLIPNO, d.ORDERCODE, d.SUCODE                    \r";
                    SQL += "      , d.BUN, d.GBORDER, d.CONTENTS, d.BCONTENTS, d.REALQTY, d.QTY, d.REALNAL              \r";
                    SQL += "      , d.NAL, d.DOSCODE, d.GBINFO, d.GBSELF, d.GBSPC, d.GBNGT, d.GBER, d.GBPRN             \r";
                    SQL += "      , d.GBDIV, d.GBBOTH, d.GBACT, d.GBTFLAG, d.GBPOSITION, d.GBSTATUS                     \r";
                    SQL += "      , d.NURSEID, d.WARDCODE, d.ROOMCODE, d.BI, d.ORDERNO, d.REMARK                        \r";
                    SQL += "      , d.ACTDATE, d.GBGROUP, d.GBPORT, d.ORDERSITE, d.MULTI, d.MULTIREMARK, d.DUR          \r";
                    SQL += "      , d.LABELPRINT, d.ACTDIV, d.GBSEND_OORDER, d.GBPICKUP, d.PICKUPSABUN                  \r";
                    SQL += "      , d.PICKUPDATE, d.EMRSET, d.GBIOE, d.MAYAK, d.POWDER, d.DCDIV, d.DIVQTY               \r";
                    SQL += "      , d.NURREMARK, d.DRORDERVIEW, d.CERTNO, d.CONSULT, d.MAYAKREMARK, d.VER               \r";
                    SQL += "      , d.IP, d.AUTO_SEND, d.GBSPC_NO, d.ENTDATE2, d.GBNGT2, d.DRCODE2, d.PICKUPREMARK      \r";
                    SQL += "      , d.GBTAX, d.AIRSHT, d.GBGUME_ACT, d.GBCHK, d.VERBC, d.VERBAL, d.GBVERB, d.V_ORDERNO  \r";
                    SQL += "      , d.VERB_PRT, d.PRN_REMARK, d.VERB_PRT_DATE, d.PRN_INS_GBN, d.PRN_INS_UNIT            \r";
                    SQL += "      , d.PRN_INS_SDATE, d.PRN_INS_EDATE, d.POWDER_SAYU, d.PRN_INS_MAX, d.ASA               \r";
                    SQL += "      , d.CERTNO2, d.PRN_INS_CDATE, d.PRN_DOSCODE, d.PRN_TERM, d.PRN_NOTIFY, d.PRN_UNIT     \r";
                    SQL += "      , d.PCHASU, d.SUBUL_WARD, d.HIGHRISK, d.ER24, d.GSADD, d.SENDDEPT, d.SENDDEPT_SUB     \r";
                    SQL += "      , d.SENDDEPT_STAT, d.SENDDEPT_INPS, d.SENDDEPT_INPT_DT, d.SENDDEPT_UPPS               \r";
                    SQL += "      , d.SENDDEPT_UPDT, d.CORDERCODE, d.CSUCODE, d.CBUN, d.OPDNO, d.BURNADD, d.OPGUBUN     \r";//, d.ACCSEND
                    SQL += "      , TO_CHAR(d.EntDate,'YYYY-MM-DD HH24:Mi') EntDate1                                    \r";
                    SQL += "      , d.ROWID                                                                             \r";
                    SQL += "      , '' cDispHeader, d.REMARK cOrderName, '' cDispRGB                                    \r";
                    SQL += "      , '' cGbBoth, '' cGbInfo, '' cDrugName                                                \r";
                    SQL += "      , '' cGbQty, '' cGbDosage, '' cNextCode, '' DAICODE                                   \r";
                    SQL += "      , '' cOrderNameS, '' cGbImiv, '' SuGbF, '' BunSu                                      \r";
                    SQL += "      , KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(d.DOSCODE) DOSNAME                                   \r";
                    SQL += "      , KOSMOS_OCS.FC_OCS_ODOSAGE_DIV(d.DOSCODE) DIV                                        \r";
                    SQL += "      , KOSMOS_OCS.FC_OCS_OSPECIMAN_NAME(d.DOSCODE, d.SLIPNO) SPECNAME                      \r";
                    SQL += "      , d.ROWID                                                                             \r";
                    SQL += "      , d.SEDATION                                                                   \r";
                    SQL += "   FROM  KOSMOS_OCS.OCS_IORDER    d                                                         \r";
                    SQL += "  WHERE d.Ptno      = '" + clsOrdFunction.Pat.PtNo + "'                                     \r";
                    SQL += "    AND d.GbStatus  IN  (' ','D+','D')                                                      \r";
                    SQL += "    AND d.BDate     >= TO_DATE('" + DateTime.Parse(clsOrdFunction.Pat.InDate).ToShortDateString() + "', 'YYYY-MM-DD')           \r";
                    if (clsPublic.Gstr구두Chk != "OK")
                    {
                        SQL += "    AND (d.NurseID IS NULL OR d.NurseId = ' ' )                                         \r";
                    }
                    SQL += "    AND d.ORDERSITE NOT IN ('CAN','NDC', 'DC1')                                             \r";
                    SQL += "    AND d.GBIOE IN('E','EI')                                                                \r";
                    SQL += "    AND d.ORDERCODE IN('S/O', 'V/S')                                                        \r";
                    //2021-01-22, NDC건은 안보이도록 보완, 전산의뢰 <2021-68> 
                    SQL += "    AND d.DIVQTY IS NULL                                                                    \r";
                    SQL += "  UNION ALL                                                                                 \r";
                    SQL += " SELECT NVL(d.GbSend ,' ') GbSend, d.EntDate EntDate, d.Seqno SEQNO, d.BDATE                \r";
                    SQL += "      , d.DEPTCODE, d.DRCODE, d.STAFFID, d.SLIPNO, d.ORDERCODE, d.SUCODE                    \r";
                    SQL += "      , d.BUN, d.GBORDER, d.CONTENTS, d.BCONTENTS, d.REALQTY, d.QTY, d.REALNAL              \r";
                    SQL += "      , d.NAL, d.DOSCODE, d.GBINFO, d.GBSELF, d.GBSPC, d.GBNGT, d.GBER, d.GBPRN             \r";
                    SQL += "      , d.GBDIV, d.GBBOTH, d.GBACT, d.GBTFLAG, d.GBPOSITION, d.GBSTATUS                     \r";
                    SQL += "      , d.NURSEID, d.WARDCODE, d.ROOMCODE, d.BI, d.ORDERNO, d.REMARK                        \r";
                    SQL += "      , d.ACTDATE, d.GBGROUP, d.GBPORT, d.ORDERSITE, d.MULTI, d.MULTIREMARK, d.DUR          \r";
                    SQL += "      , d.LABELPRINT, d.ACTDIV, d.GBSEND_OORDER, d.GBPICKUP, d.PICKUPSABUN                  \r";
                    SQL += "      , d.PICKUPDATE, d.EMRSET, d.GBIOE, d.MAYAK, d.POWDER, d.DCDIV, d.DIVQTY               \r";
                    SQL += "      , d.NURREMARK, d.DRORDERVIEW, d.CERTNO, d.CONSULT, d.MAYAKREMARK, d.VER               \r";
                    SQL += "      , d.IP, d.AUTO_SEND, d.GBSPC_NO, d.ENTDATE2, d.GBNGT2, d.DRCODE2, d.PICKUPREMARK      \r";
                    SQL += "      , d.GBTAX, d.AIRSHT, d.GBGUME_ACT, d.GBCHK, d.VERBC, d.VERBAL, d.GBVERB, d.V_ORDERNO  \r";
                    SQL += "      , d.VERB_PRT, d.PRN_REMARK, d.VERB_PRT_DATE, d.PRN_INS_GBN, d.PRN_INS_UNIT            \r";
                    SQL += "      , d.PRN_INS_SDATE, d.PRN_INS_EDATE, d.POWDER_SAYU, d.PRN_INS_MAX, d.ASA               \r";
                    SQL += "      , d.CERTNO2, d.PRN_INS_CDATE, d.PRN_DOSCODE, d.PRN_TERM, d.PRN_NOTIFY, d.PRN_UNIT     \r";
                    SQL += "      , d.PCHASU, d.SUBUL_WARD, d.HIGHRISK, d.ER24, d.GSADD, d.SENDDEPT, d.SENDDEPT_SUB     \r";
                    SQL += "      , d.SENDDEPT_STAT, d.SENDDEPT_INPS, d.SENDDEPT_INPT_DT, d.SENDDEPT_UPPS               \r";
                    SQL += "      , d.SENDDEPT_UPDT, d.CORDERCODE, d.CSUCODE, d.CBUN, d.OPDNO, d.BURNADD, d.OPGUBUN     \r";//, d.ACCSEND                         
                    SQL += "      , TO_CHAR(d.EntDate,'YYYY-MM-DD HH24:Mi') EntDate1                                    \r";
                    SQL += "      , d.ROWID                                                                             \r";
                    SQL += "      , a.DispHeader cDispHeader, a.OrderName cOrderName, a.DispRGB cDispRGB                \r";
                    SQL += "      , a.GbBoth cGbBoth, a.GbInfo cGbInfo,a.DrugName cDrugName                             \r";
                    SQL += "      , a.GbQty cGbQty, a.GbDosage cGbDosage, a.NextCode cNextCode, c.DAICODE               \r";
                    SQL += "      , a.OrderNameS cOrderNameS, a.GbImiv cGbImiv, b.SuGbF, b.Bun BunSu                    \r";
                    SQL += "      , KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(d.DOSCODE) DOSNAME                                   \r";
                    SQL += "      , KOSMOS_OCS.FC_OCS_ODOSAGE_DIV(d.DOSCODE) DIV                                        \r";
                    SQL += "      , KOSMOS_OCS.FC_OCS_OSPECIMAN_NAME(d.DOSCODE, d.SLIPNO) SPECNAME                      \r"; 
                    SQL += "      , d.ROWID                                                                             \r";
                    SQL += "      , d.SEDATION                                                                   \r";
                    SQL += "   FROM KOSMOS_OCS.OCS_ORDERCODE a                                                          \r";
                    SQL += "      , KOSMOS_PMPA.BAS_SUT      b                                                          \r";
                    SQL += "      , KOSMOS_PMPA.BAS_SUN      c                                                          \r";
                    SQL += "      , KOSMOS_OCS.OCS_IORDER    d                                                          \r";
                    SQL += "  WHERE a.SuCode    = b.SuCode(+)                                                           \r";
                    SQL += "    AND a.SuCode    = C.SUNEXT(+)                                                           \r";
                    SQL += "    AND a.ORDERCODE = d.ORDERCODE(+)                                                        \r";
                    SQL += "    AND a.SLIPNO    = d.SLIPNO(+)                                                           \r";
                    SQL += "    AND d.Ptno      = '" + clsOrdFunction.Pat.PtNo + "'                                     \r";
                    SQL += "    AND d.GbStatus  IN  (' ','D+','D')                                                      \r";
                    SQL += "    AND d.BDate     >= TO_DATE('" + DateTime.Parse(clsOrdFunction.Pat.InDate).ToShortDateString() + "', 'YYYY-MM-DD')           \r";
                    if (clsPublic.Gstr구두Chk != "OK")
                    {
                        SQL += "    AND (d.NurseID IS NULL OR d.NurseId = ' ' )                                         \r";
                    }
                    SQL += "    AND d.ORDERSITE NOT IN ('CAN','NDC', 'DC1')                                             \r";
                    SQL += "    AND d.GBIOE IN('E','EI')                                                                \r";
                    SQL += "    AND D.ORDERCODE NOT IN('S/O', 'V/S')                                                    \r";
                    //2021-01-22, NDC건은 안보이도록 보완, 전산의뢰 <2021-68> 
                    SQL += "    AND d.DIVQTY IS NULL                                                                    \r";
                    SQL += "  ORDER BY GbSend, EntDate, Seqno                                                           \r";

                    clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return null;
                    }

                    rowcounter = 0;
                    rowcounter = dt.Rows.Count;

                    if (rowcounter > 0)
                    {
                        //clsOrdFunction.GnReadOrder = dt.Rows.Count;
                        rtnDt = dt;
                    }
                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }
                #endregion //ER
            }
            else if (strGBIO == "IPD")
            {
                #region //IPD
                //string strAntiUsed = "";
                //string strOLD = "NO";
                //string strRowIDChk = "";


                #region 오더 없을시 전회처방(마지막 처방) 가져와서 뿌려줌.( REGION 추가 2021-08-04)
                if (clsOrdFunction.GnReadOrder == 0 || clsOrdFunction.Pat.CPCode.NotEmpty())
                {
                    try
                    {
                        SQL = "";
                        SQL += " SELECT TO_CHAR(MAX(BDATE),'YYYY-MM-DD') MAXBDATE                               \r";  //전회 처방 일자 Search
                        SQL += "   FROM KOSMOS_OCS.OCS_IORDER                                                   \r";
                        SQL += "  WHERE PTNO   =  '" + clsOrdFunction.Pat.PtNo + "'                             \r";
                        SQL += "    AND BDATE <= TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')       \r";
                        SQL += "    AND BDATE >= TO_DATE('" + clsOrdFunction.Pat.EntDate + "','YYYY-MM-DD')     \r";
                        SQL += "    AND GBSTATUS IN  (' ','D+')                                                 \r";
                        SQL += "    AND (GBINFO <> 'TRANSFER' OR GBINFO IS NULL)                                \r";
                        SQL += "    AND SLIPNO NOT IN ('A7')                                                    \r";
                        SQL += "    AND (ORDERSITE IS NULL OR ORDERSITE <>'OPDX' )                              \r";
                        SQL += "    AND (VER IS NULL OR VER <> 'CPORDER')                                       \r";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 오류가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        }

                        if (reader.HasRows && reader.Read())
                        {
                            string BDATE = reader.GetValue(0).ToString().Trim();
                            if (clsOrdFunction.GstrBDate.Trim().Equals(BDATE) && clsOrdFunction.GnReadOrder == 0)
                            {
                                strBDateChk = "Y";  //RowID Display 여부
                            }

                            if (BDATE.IsNullOrEmpty() && clsOrdFunction.GnReadOrder == 0)
                            {
                                reader.Dispose();
                                reader = null;
                                return null;
                            }
                            cMaxBDate = BDATE;
                        }
                        reader.Dispose();
                        reader = null;
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                }
                #endregion

                #region CP 대상이고, 전회처방이 있으며 오늘이 아닐경우.
                bool CP_EXISTS = false;
                if (clsOrdFunction.Pat.CPCode.NotEmpty() && cMaxBDate.NotEmpty() && 
                    clsOrdFunction.GstrBDate.Equals(cMaxBDate) == false && strBDateChk.Equals("N"))
                {
                    try
                    {
                        SQL = "";
                        SQL += " SELECT 1                                                                          \r";  //CP 대상인지
                        SQL += "   FROM DUAL                                                                       \r";
                        SQL += "  WHERE EXISTS                                                                     \r";
                        SQL += "  (                                                                                \r";
                        SQL += "    SELECT 1                                                                       \r";  //CP오더 제외 추가처방 있는지
                        SQL += "      FROM KOSMOS_OCS.OCS_CP_RECORD                                                \r";
                        SQL += "     WHERE IPDNO   =  " + clsOrdFunction.Pat.IPDNO + "                             \r";
                        SQL += "       AND CANCERSABUN IS NULL                                                     \r";
                        SQL += "       AND DROPSABUN IS NULL                                                       \r";
                        SQL += "  )                                                                                \r";
                        SQL += "    AND NOT EXISTS                                                                 \r";
                        SQL += "    (                                                                              \r";
                        SQL += "      SELECT 1                                                                     \r";  //전회 처방 일자 Search
                        SQL += "        FROM KOSMOS_OCS.OCS_IORDER                                                 \r";
                        SQL += "       WHERE PTNO   =  '" + clsOrdFunction.Pat.PtNo + "'                           \r";
                        SQL += "         AND BDATE  = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')     \r";
                        SQL += "         AND GBSTATUS IN  (' ')                                                    \r";
                        SQL += "         AND ( GBINFO <> 'TRANSFER' OR GBINFO IS NULL )                            \r";
                        SQL += "         AND SLIPNO NOT IN ('A7')                                                  \r";
                        SQL += "         AND (VER IS NULL OR VER <>'CPORDER')                                      \r";
                        SQL += "    )                                                                              \r";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 오류가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        }

                        if (reader.HasRows)
                        {
                            CP_EXISTS = true;
                        }
                        reader.Dispose();
                        reader = null;
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                }
                #endregion

                try
                {
                    #region 쿼리
                    SQL = "";

                    SQL = "WITH ORDER_DATA AS                                                                               \r";
                    SQL +="(                                                                                                \r";
                    SQL += " SELECT D.SLIPNO                                                                                \r";
                    SQL += "	,	D.ORDERCODE                                                                             \r";
                    SQL += "	,	D.SUCODE                                                                                \r";
                    SQL += "	,	D.BUN                                                                                   \r";
                    SQL += "	,	D.DOSCODE                                                                               \r";
                    SQL += "	,	D.GBPICKUP                                                                              \r";
                    SQL += "	,	D.CONTENTS                                                                              \r";
                    SQL += "	,	D.BCONTENTS                                                                             \r";
                    SQL += "	,	D.REALQTY                                                                               \r";
                    SQL += "	,	D.QTY                                                                                   \r";
                    SQL += "	,	D.GBDIV                                                                                 \r";
                    SQL += "	,	D.NAL                                                                                   \r";
                    SQL += "	,	D.POWDER                                                                                \r";
                    SQL += "	,	D.GBNGT                                                                                 \r";
                    SQL += "	,	D.GBGROUP                                                                               \r";
                    SQL += "	,	D.GBER                                                                                  \r";
                    SQL += "	,	D.GBSELF                                                                                \r";
                    SQL += "	,	D.REMARK                                                                                \r";
                    SQL += "	,	D.GBTFLAG                                                                               \r";
                    SQL += "	,	D.GBPORT                                                                                \r";
                    SQL += "	,	D.GBINFO                                                                                \r";
                    SQL += "	,	D.GBACT                                                                                 \r";
                    SQL += "	,	D.GBSTATUS                                                                              \r";
                    SQL += "	,	D.GBORDER                                                                               \r";
                    SQL += "	,	D.GBBOTH                                                                                \r";
                    SQL += "	,	D.GBPOSITION                                                                            \r";
                    SQL += "	,	D.ORDERNO                                                                               \r";
                    SQL += "	,	D.ORDERNO AS ORDERNO2                                                                   \r";
                    SQL += "	,	D.DRCODE                                                                                \r";
                    SQL += "	,	D.DEPTCODE                                                                              \r";
                    SQL += "	,	TO_CHAR(D.ENTDATE, 'YYYY-MM-DD HH24:MI') AS ENTDATE1                                    \r";
                    SQL += "	,	D.STAFFID                                                                               \r";
                    SQL += "	,	D.MULTI                                                                                 \r";
                    SQL += "	,	D.MULTIREMARK                                                                           \r";
                    SQL += "	,	D.DUR                                                                                   \r";
                    SQL += "	,	D.CONSULT                                                                               \r";
                    SQL += "	,	D.MAYAK                                                                                 \r";
                    SQL += "	,	D.MAYAKREMARK                                                                           \r";
                    SQL += "	,	D.GBIOE                                                                                 \r";
                    SQL += "	,	D.GBSPC_NO                                                                              \r";
                    SQL += "	,	D.VERBAL                                                                                \r";
                    SQL += "	,	D.VERBC                                                                                 \r";
                    SQL += "	,	D.V_Orderno	                                                                            \r";
                    SQL += "	,	D.GbVerb                                                                                \r";
                    SQL += "	,	D.ASA                                                                                   \r";
                    SQL += "	,	D.NURSEID                                                                               \r";
                    SQL += "	,	D.POWDER_SAYU                                                                           \r";
                    SQL += "	,	D.GBPRN                                                                                 \r";
                    SQL += "	,	D.PRN_REMARK	                                                                        \r";
                    SQL += "	,	D.PRN_INS_GBN                                                                           \r";
                    SQL += "	,	D.PRN_UNIT                                                                              \r";
                    SQL += "	,	D.PRN_INS_SDate                                                                         \r";
                    SQL += "	,	D.PRN_INS_EDate                                                                         \r";
                    SQL += "	,	D.PRN_INS_Max                                                                           \r";
                    SQL += "	,	D.PRN_DosCode                                                                           \r";
                    SQL += "	,	D.PRN_Term                                                                              \r";
                    SQL += "	,	D.PRN_Notify	                                                                        \r";
                    SQL += "	,	D.PCHASU                                                                                \r";
                    SQL += "	,	D.PRN_ORDSEQ                                                                            \r";
                    SQL += "	,	D.AIRSHT                                                                                \r";
                    SQL += "	,	D.SUBUL_WARD                                                                            \r";
                    SQL += "	,	D.ER24                                                                                  \r";
                    SQL += "	,	D.GSADD                                                                                 \r";
                    SQL += "	,	D.ACTDIV                                                                                \r";
                    SQL += "	,	D.BURNADD                                                                               \r";
                    SQL += "	,	D.OPGUBUN                                                                               \r";
                    SQL += "	,	D.VER                                                                                   \r";
                    SQL += "	,	D.TUYEOPOINT                                                                            \r";
                    SQL += "	,	D.TUYEOTIME                                                                             \r";
                    SQL += "    ,   D.GBSEND                                                                                \r";
                    SQL += "    ,   D.GBSEND AS GBSEND1                                                                     \r";
                    SQL += "    ,   D.SEQNO  AS SEQNO1                                                                      \r";
                    SQL += "    ,   '' AS VER2                                                                              \r";
                    SQL += "    ,   D.ROWID AS RID                                                                          \r";
                    SQL += "    ,   D.SEDATION                                                                              \r";
                    SQL += "    ,   D.GBTAX                                                                                 \r";
                    SQL += "    ,   D.ORDERSITE                                                                             \r";
                    SQL += "    ,   a.DispHeader cDispHeader, a.OrderName cOrderName, a.DispRGB cDispRGB                    \r";
                    SQL += "    ,   a.GbBoth cGbBoth, a.GbInfo cGbInfo, a.DrugName cDrugName                                \r";
                    SQL += "    ,   a.GbQty cGbQty, a.GbDosage cGbDosage, a.NextCode cNextCode, c.DAICODE                   \r";
                    SQL += "    ,   a.OrderNameS cOrderNameS, a.GbImiv cGbImiv, b.SuGbF, b.Bun BunSu                        \r";
                    SQL += "    ,   KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(d.DOSCODE) DOSNAME                                       \r";
                    SQL += "    ,   KOSMOS_OCS.FC_OCS_ODOSAGE_DIV(d.DOSCODE) DIV                                            \r";
                    SQL += "    ,   KOSMOS_OCS.FC_OCS_OSPECIMAN_NAME(d.DOSCODE, d.SLIPNO) SPECNAME                          \r";
                    SQL += "    ,   a.CBUN                                                                                  \r";
                    SQL += "    ,   decode(trim(A.ORDERCODE), '', 'DEL', '') GBDEL                                          \r";
                    SQL += "    ,   a.DISPHEADER                                                                            \r";
                    SQL += "    ,   a.NEXTCODE                                                                              \r";

                    SQL += "   FROM KOSMOS_OCS.OCS_ORDERCODE a                                                              \r";
                    SQL += "      , KOSMOS_PMPA.BAS_SUT      b                                                              \r";
                    SQL += "      , KOSMOS_PMPA.BAS_SUN      c                                                              \r";
                    SQL += "      , KOSMOS_OCS.OCS_IORDER    d                                                              \r";
                    SQL += "  WHERE a.SuCode    = b.SuCode(+)                                                               \r";
                    SQL += "    AND a.SuCode    = C.SUNEXT(+)                                                               \r";
                    SQL += "    AND d.ORDERCODE = a.ORDERCODE(+)                                                            \r";
                    SQL += "    AND d.SLIPNO    = a.SLIPNO(+)                                                               \r";
                    SQL += "    AND Ptno        = '" + clsOrdFunction.Pat.PtNo + "'                                         \r";
                    //Consult 처방시에 해당과(소견과) 처방만 보여줌
                    if (clsOrdFunction.GstrConsultOrd == "ON")
                    {
                        SQL += "   AND d.DeptCode = '" + clsPublic.GstrDeptCode.Trim() + "'                                 \r";
                    }
                    //'2015-04-03 재원환자중 PC의사 처방시 cnt 체크
                    if (clsOrdFunction.GstrDeptPC_Doct == "OK" && clsOrdFunction.GstrConsultOrd != "ON")
                    {
                        SQL += "   AND d.DeptCode = 'PC'                                                                     \r";
                    }
                    
                    if (clsOrdFunction.GnReadOrder == 9999)    //당일처방 존재                    
                    {
                        SQL += "    AND d.GbStatus  IN  (' ','D+','D')                                                      \r";
                        SQL += "    AND d.BDate      = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM-DD')             \r";
                    }
                    else                          //전회처방
                    {
                        SQL += "    AND d.BDate = TO_DATE('" + cMaxBDate + "', 'YYYY-MM-DD')                                \r";
                        SQL += "    AND (d.GbPosition = ' '                                                                 \r";
                        SQL += "     OR d.GbPosition IS NULL                                                                \r";
                        SQL += "     OR d.GbPosition = 'R')                                                                 \r"; //전화처방 전송 않하는부분 允(2006-08-01)
                        //'아래는 전회처방은 Repeat 항목과 같이 보여준다.
                        //SQL += "    AND (d.SlipNo IN ('A1','A2','A4')                                                       \r";
                          
                        //2019-06-17  OS 전공의 황성현 요청으로 S/O 1TIME은 리피트 안되도록
                        SQL += "    AND (d.SlipNo IN ('A1', 'A2') OR                                                        \r";
                        SQL += "    (d.SlipNo = 'A4' AND SUBSTR(UPPER(d.REMARK), 1, 7) <> '(1TIME)')                        \r"; 
                        
                        SQL += "     OR (d.Bun >= '11'                                                                      \r";
                        SQL += "    AND d.Bun <= '20'                                                                       \r";
                        SQL += "    AND d.Bun <> '12'                                                                       \r";
                        SQL += "    AND d.OrderCode Not IN ('NEEDLE', 'H-M37V', 'H-NC', 'H-NH')  )                          \r";
                        SQL += "     OR (A.SLIPNO = '0102' AND A.SUCODE NOT IN ('EX773','E6611','E6612','EX780',            \r";
                        SQL += "        'EV771','PTDDST','F6216P','F6222P', 'EY773') )                                               \r";
                        //2021-02-10 재활의학과 전담 요청으로 '0101' 추가
                        SQL += "     OR (d.SlipNo = '0101')                                                                 \r";
                        SQL += "     OR (d.SlipNo = 'A5'                                                                    \r";
                        SQL += "    AND d.GbPosition = 'R')                                                                 \r";
                        SQL += "     OR d.SUCODE IN ('SALS','PULMI',                                                        \r";  
                        //2012-09-28 MYST4A add
                        //2012-12-06 N-FT-HA add
                        //2020-03-31 PULMI2 add
                        //2020-11-10 VENTOLIN add
                        //2020-11-27 PULMICAN add 
                        SQL += "        'ATROS','PT-HO2','MYST4A','N-FT-HA','PT-HO05','PT-HO2','SBTA','ABEN-2A','PULMI2','VENTOLIN','PULMICAN')  \r";                         
                        SQL += "     OR d.SUCODE IN ('C3710', 'C3710', 'C3710A'))                                                                \r"; //2020-08-10, BST 처방 추가
                        SQL += "    AND d.ORDERCODE <> 'C/O'                                                                                     \r";
                        SQL += "    AND (d.ordersite is null or d.ordersite <> 'OPDX' )                                                          \r";  //'2013-04-09
                        SQL += "    AND (d.SuCode IS NULL                                                                                        \r";
                        SQL += "     OR (TRIM(d.SuCode) NOT IN (                                                                                 \r";
                        SQL += "                                SELECT SUCODE                                                                    \r";
                        SQL += "                                  FROM KOSMOS_PMPA.BAS_SUT                                                       \r";
                        SQL += "                                 WHERE DelDate <TRUNC(SYSDATE)                                                   \r";
                        SQL += "                                   AND DelDate >= TRUNC(SYSDATE-365)                                             \r";
                        SQL += "                                   AND BUN IN ('11','12','20')  )  )  )                                          \r"; //2013-05-25 삭제약 제외
                        SQL += "    AND (d.SuCode IS NULL OR d.SuCode NOT IN ('$$39') ) \r"; //'2013-06-12
                        if (strBDateChk.Trim() == "Y")
                        {
                            SQL += "    AND d.GbStatus IN  (' ','D+','D')                                                                        \r";
                        }                                                                                                                        
                        else                                                                                                                     
                        {                                                                                                                        
                            SQL += "    AND d.GbStatus IN  (' ','D+')                                                                            \r";
                            //2019-08-23 유진호 임시                                                                                               
                            SQL += "    AND d.QTY > 0                                                                                            \r";
                        }                                                                                                                        
                        if (clsPublic.GstrCP처방Chk == "OK")                                                                                      
                        {                                                                                                                        
                            SQL += "    AND (d.Ver IS NULL OR d.Ver <>'CPORDER')                                                                 \r";
                        }                                                                                                                        
                    }                                                                                                                            
                    if (clsPublic.GstrJobMan.Trim() != "마취실" && clsPublic.GstrJobMan != "수술실")                                               
                    {                                                                                                                            
                        SQL += "    AND ((d.GBIOE IN ( 'EI' ,'E') AND d.GBACT ='*') OR (d.GBACT <> '*' OR d.GBACT IS NULL))                      \r";
                    }
                    if ((clsPublic.GstrJobMan == "간호사" || clsPublic.GstrJobMan == "수술실" || clsPublic.GstrJobMan == "마취실") && clsOrdFunction.GstrOrdDis.Trim() != "")//각 Part별 처방 보기
                    {
                        SQL += "    AND  d.OrderSite  = '" + clsOrdFunction.GstrOrdDis.Trim() + "'                                               \r";
                    }
                    else if (clsOrdFunction.GstrDept.Trim() == "AN")
                    {
                        //2018.10.30 이상훈 임시
                        //SQL += "    AND  d.OrderSite  = 'AN'                                                                \r";   //마취과 처방만 봄
                        SQL += "    AND  (d.OrderSite  = 'AN' OR d.OrderSite  = 'PC')                                                            \r";   //마취과 처방만 봄
                    }                                                                                                                            
                    else if (clsOrdFunction.GstrDept.Trim() == "XR")                                                                             
                    {                                                                                                                            
                        SQL += "    AND  d.OrderSite  = 'XR'                                                                                     \r";   //방사선과 처방만 봄
                    }
                    else
                    {
                        if (clsOrdFunction.GnReadOrder == 9999)
                        {
                            //'2015-07-29
                            if (clsPublic.Gstr구두Chk == "OK")
                            {
                                SQL += "    AND (d.NurseID IS NULL OR d.NurseId = ' '                                                            \r";   //DRUG(약국 재형 변경 처방; phariord)
                                SQL += "     OR d.OrderSite IN ('','TEL','OPD','DRUG','IPD','ER','" + clsOrdFunction.GstrDept.Trim() + "') )     \r";  //TEL(간호사 Verbar처방)
                                SQL += "    AND (d.VerbC IS NULL OR d.VerbC <> 'Y')                                                              \r";  //의사확인한 구두처방건 제외
                            }                                                                                                                    
                            else                                                                                                                 
                            {                                                                                                                    
                                SQL += "    AND (d.NurseID   IS NULL OR d.NurseId = ' '                                                          \r";   //DRUG(약국 재형 변경 처방; phariord)
                                SQL += "     OR d.OrderSite IN ('','TEL','OPD','DRUG','IPD','ER','" + clsOrdFunction.GstrDept.Trim() + "'))      \r";  //TEL(간호사 Verbar처방)
                            }                                                                                                                    
                        }                                                                                                                        
                        else //ER(응급실)                                                                                                         
                        {                                                                                                                        
                            SQL += "    AND (d.NurseID   IS NULL OR d.NurseId = ' ')                                                             \r";
                            SQL += "    AND  d.Slipno    <> 'TEL'                                                                                \r";
                        }                                                                                                                        
                    }                                                                                                                            
                                                                                                                                                 
                    #region 전회처방 D/C 미표시                                                                                                     
                    if (clsOrdFunction.GnReadOrder != 9999)
                    {
                        SQL += "    AND NOT EXISTS                                                                                                   \r";
                        SQL += "    (                                                                                                                \r";
                        SQL += "      SELECT 1                                                                                                       \r";  //D/C 전회 처방 미표시
                        SQL += "        FROM KOSMOS_OCS.OCS_LASTORDER_MAPPING                                                                        \r";
                        SQL += "       WHERE PTNO     = D.PTNO                                                                                       \r";
                        SQL += "         AND ORDERNO  = D.ORDERNO                                                                                    \r";
                        SQL += "    )                                                                                                                \r";
                    }
                    #endregion

                    SQL += ")                                                                                                \r";

                    #region 전회처방  순서 관련 .

                    SQL += " SELECT SLIPNO                                                                                \r";
                    SQL += "	,	ORDERCODE                                                                             \r";
                    SQL += "	,	SUCODE                                                                                \r";
                    SQL += "	,	BUN                                                                                   \r";
                    SQL += "	,	DOSCODE                                                                               \r";
                    SQL += "	,	GBPICKUP                                                                              \r";
                    SQL += "	,	CONTENTS                                                                              \r";
                    SQL += "	,	BCONTENTS                                                                             \r";
                    SQL += "	,	REALQTY                                                                               \r";
                    SQL += "	,	QTY                                                                                   \r";
                    SQL += "	,	GBDIV                                                                                 \r";
                    SQL += "	,	NAL                                                                                   \r";
                    SQL += "	,	POWDER                                                                                \r";
                    SQL += "	,	GBNGT                                                                                 \r";
                    SQL += "	,	GBGROUP                                                                               \r";
                    SQL += "	,	GBER                                                                                  \r";
                    SQL += "	,	GBSELF                                                                                \r";
                    SQL += "	,	REMARK                                                                                \r";
                    SQL += "	,	GBTFLAG                                                                               \r";
                    SQL += "	,	GBPORT                                                                                \r";
                    SQL += "	,	GBINFO                                                                                \r";
                    SQL += "	,	GBACT                                                                                 \r";
                    SQL += "	,	GBSTATUS                                                                              \r";
                    SQL += "	,	GBORDER                                                                               \r";
                    SQL += "	,	GBBOTH                                                                                \r";
                    SQL += "	,	GBPOSITION                                                                            \r";
                    SQL += "	,	ORDERNO                                                                               \r";
                    SQL += "	,	ORDERNO AS ORDERNO2                                                                   \r";
                    SQL += "	,	DRCODE                                                                                \r";
                    SQL += "	,	DEPTCODE                                                                              \r";
                    SQL += "	,	ENTDATE1                                                                              \r";
                    SQL += "	,	STAFFID                                                                               \r";
                    SQL += "	,	MULTI                                                                                 \r";
                    SQL += "	,	MULTIREMARK                                                                           \r";
                    SQL += "	,	DUR                                                                                   \r";
                    SQL += "	,	CONSULT                                                                               \r";
                    SQL += "	,	MAYAK                                                                                 \r";
                    SQL += "	,	MAYAKREMARK                                                                           \r";
                    SQL += "	,	GBIOE                                                                                 \r";
                    SQL += "	,	GBSPC_NO                                                                              \r";
                    SQL += "	,	VERBAL                                                                                \r";
                    SQL += "	,	VERBC                                                                                 \r";
                    SQL += "	,	V_Orderno	                                                                          \r";
                    SQL += "	,	GbVerb                                                                                \r";
                    SQL += "	,	ASA                                                                                   \r";
                    SQL += "	,	NURSEID                                                                               \r";
                    SQL += "	,	POWDER_SAYU                                                                           \r";
                    SQL += "	,	GBPRN                                                                                 \r";
                    SQL += "	,	PRN_REMARK	                                                                          \r";
                    SQL += "	,	PRN_INS_GBN                                                                           \r";
                    SQL += "	,	PRN_UNIT                                                                              \r";
                    SQL += "	,	PRN_INS_SDate                                                                         \r";
                    SQL += "	,	PRN_INS_EDate                                                                         \r";
                    SQL += "	,	PRN_INS_Max                                                                           \r";
                    SQL += "	,	PRN_DosCode                                                                           \r";
                    SQL += "	,	PRN_Term                                                                              \r";
                    SQL += "	,	PRN_Notify	                                                                          \r";
                    SQL += "	,	PCHASU                                                                                \r";
                    SQL += "	,	PRN_ORDSEQ                                                                            \r";
                    SQL += "	,	AIRSHT                                                                                \r";
                    SQL += "	,	SUBUL_WARD                                                                            \r";
                    SQL += "	,	ER24                                                                                  \r";
                    SQL += "	,	GSADD                                                                                 \r";
                    SQL += "	,	ACTDIV                                                                                \r";
                    SQL += "	,	BURNADD                                                                               \r";
                    SQL += "	,	OPGUBUN                                                                               \r";
                    SQL += "	,	VER                                                                                   \r";
                    SQL += "	,	TUYEOPOINT                                                                            \r";
                    SQL += "	,	TUYEOTIME                                                                             \r";
                    SQL += "    ,   GBSEND                                                                                \r";
                    SQL += "    ,   GBSEND1                                                                               \r";
                    SQL += "    ,   SEQNO1                                                                                \r";
                    SQL += "    ,   VER2                                                                                  \r";
                    SQL += "    ,   RID                                                                                   \r";
                    SQL += "    ,   SEDATION                                                                              \r";
                    SQL += "    ,   GBTAX                                                                                 \r";
                    SQL += "    ,   ORDERSITE                                                                             \r";
                    SQL += "    ,   cDispHeader, cOrderName, cDispRGB                                                     \r";
                    SQL += "    ,   cGbBoth, cGbInfo, cDrugName                                                           \r";
                    SQL += "    ,   cGbQty, cGbDosage, cNextCode, DAICODE                                                 \r";
                    SQL += "    ,   cOrderNameS, cGbImiv, SuGbF, BunSu                                                    \r";
                    SQL += "    ,   DOSNAME                                                                               \r";
                    SQL += "    ,   DIV                                                                                   \r";
                    SQL += "    ,   SPECNAME                                                                              \r";
                    SQL += "    ,   CBUN                                                                                  \r";
                    SQL += "    ,   GBDEL                                                                                 \r";
                    SQL += "    ,   DISPHEADER                                                                            \r";
                    SQL += "    ,   NEXTCODE                                                                              \r";
                    SQL += "  FROM ORDER_DATA                                                                             \r";



                    #endregion



                    #region CP 전회(추가)처방 가져오기
                    if (CP_EXISTS && clsOrdFunction.GstrConsultOrd.IsNullOrEmpty() && clsOrdFunction.GnReadOrder == 9999)
                    {
                        #region 쿼리
                        SQL += "UNION ALL                                                                                                        \r";
                        SQL += " SELECT D.SLIPNO                                                                                                 \r";
                        SQL += "	,	D.ORDERCODE                                                                                              \r";
                        SQL += "	,	D.SUCODE                                                                                                 \r";
                        SQL += "	,	D.BUN                                                                                                    \r";
                        SQL += "	,	D.DOSCODE                                                                                                \r";
                        SQL += "	,	'' AS  GBPICKUP                                                                                          \r";
                        SQL += "	,	D.CONTENTS                                                                                               \r";
                        SQL += "	,	D.BCONTENTS                                                                                              \r";
                        SQL += "	,	D.REALQTY                                                                                                \r";
                        SQL += "	,	D.QTY                                                                                                    \r";
                        SQL += "	,	D.GBDIV                                                                                                  \r";
                        SQL += "	,	D.NAL                                                                                                    \r";
                        SQL += "	,	D.POWDER                                                                                                 \r";
                        SQL += "	,	D.GBNGT                                                                                                  \r";
                        SQL += "	,	D.GBGROUP                                                                                                \r";
                        SQL += "	,	D.GBER                                                                                                   \r";
                        SQL += "	,	D.GBSELF                                                                                                 \r";
                        SQL += "	,	D.REMARK                                                                                                 \r";
                        SQL += "	,	D.GBTFLAG                                                                                                \r";
                        SQL += "	,	D.GBPORT                                                                                                 \r";
                        SQL += "	,	D.GBINFO                                                                                                 \r";
                        SQL += "	,	D.GBACT                                                                                                  \r";
                        SQL += "	,	'' AS GBSTATUS                                                                                           \r";
                        SQL += "	,	D.GBORDER                                                                                                \r";
                        SQL += "	,	D.GBBOTH                                                                                                 \r";
                        SQL += "	,	D.GBPOSITION                                                                                             \r";
                        SQL += "	,	0 AS ORDERNO                                                                                             \r";
                        SQL += "	,	D.ORDERNO AS ORDERNO2                                                                                    \r";
                        SQL += "	,	D.DRCODE                                                                                                 \r";
                        SQL += "	,	D.DEPTCODE                                                                                               \r";
                        SQL += "	,	TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI') AS ENTDATE1                                                       \r";
                        SQL += "	,	D.STAFFID                                                                                                \r";
                        SQL += "	,	D.MULTI                                                                                                  \r";
                        SQL += "	,	D.MULTIREMARK                                                                                            \r";
                        SQL += "	,	D.DUR                                                                                                    \r";
                        SQL += "	,	D.CONSULT                                                                                                \r";
                        SQL += "	,	D.MAYAK                                                                                                  \r";
                        SQL += "	,	D.MAYAKREMARK                                                                                            \r";
                        SQL += "	,	D.GBIOE                                                                                                  \r";
                        SQL += "	,	D.GBSPC_NO                                                                                               \r";
                        SQL += "	,	D.VERBAL                                                                                                 \r";
                        SQL += "	,	D.VERBC                                                                                                  \r";
                        SQL += "	,	0 AS V_Orderno	                                                                                         \r";
                        SQL += "	,	D.GbVerb                                                                                                 \r";
                        SQL += "	,	D.ASA                                                                                                    \r";
                        SQL += "	,	D.NURSEID                                                                                                \r";
                        SQL += "	,	D.POWDER_SAYU                                                                                            \r";
                        SQL += "	,	D.GBPRN                                                                                                  \r";
                        SQL += "	,	D.PRN_REMARK	                                                                                         \r";
                        SQL += "	,	D.PRN_INS_GBN                                                                                            \r";
                        SQL += "	,	D.PRN_UNIT                                                                                               \r";
                        SQL += "	,	D.PRN_INS_SDate                                                                                          \r";
                        SQL += "	,	D.PRN_INS_EDate                                                                                          \r";
                        SQL += "	,	D.PRN_INS_Max                                                                                            \r";
                        SQL += "	,	D.PRN_DosCode                                                                                            \r";
                        SQL += "	,	D.PRN_Term                                                                                               \r";
                        SQL += "	,	D.PRN_Notify	                                                                                         \r";
                        SQL += "	,	D.PCHASU                                                                                                 \r";
                        SQL += "	,	D.PRN_ORDSEQ                                                                                             \r";
                        SQL += "	,	D.AIRSHT                                                                                                 \r";
                        SQL += "	,	D.SUBUL_WARD                                                                                             \r";
                        SQL += "	,	D.ER24                                                                                                   \r";
                        SQL += "	,	D.GSADD                                                                                                  \r";
                        SQL += "	,	D.ACTDIV                                                                                                 \r";
                        SQL += "	,	D.BURNADD                                                                                                \r";
                        SQL += "	,	D.OPGUBUN                                                                                                \r";
                        SQL += "	,	D.VER                                                                                                    \r";
                        SQL += "	,	D.TUYEOPOINT                                                                                             \r";
                        SQL += "	,	D.TUYEOTIME                                                                                              \r";
                        SQL += "    ,   ''  AS GBSEND                                                                                            \r";
                        SQL += "    ,   '*' AS GBSEND1                                                                                           \r";
                        SQL += "    ,   (SELECT MAX(SEQNO1) FROM ORDER_DATA) + D.SEQNO AS  SEQNO1                                                                                           \r";
                        SQL += "    ,   'CP' AS VER2                                                                                             \r";
                        SQL += "    ,   NULL AS RID                                                                                              \r";
                        SQL += "    ,   D.SEDATION                                                                                               \r";
                        SQL += "    ,   D.GBTAX                                                                                                  \r";
                        SQL += "    ,   '' AS ORDERSITE                                                                                          \r";
                        SQL += "    ,   A.DISPHEADER CDISPHEADER, A.ORDERNAME CORDERNAME, A.DISPRGB CDISPRGB                                     \r";
                        SQL += "    ,   A.GBBOTH CGBBOTH, A.GBINFO CGBINFO, A.DRUGNAME CDRUGNAME                                                 \r";
                        SQL += "    ,   A.GBQTY CGBQTY, A.GBDOSAGE CGBDOSAGE, A.NEXTCODE CNEXTCODE, C.DAICODE                                    \r";
                        SQL += "    ,   A.ORDERNAMES CORDERNAMES, A.GBIMIV CGBIMIV, B.SUGBF, B.BUN BUNSU                                         \r";
                        SQL += "    ,   KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(D.DOSCODE) DOSNAME                                                        \r";
                        SQL += "    ,   KOSMOS_OCS.FC_OCS_ODOSAGE_DIV(D.DOSCODE) DIV                                                             \r";
                        SQL += "    ,   KOSMOS_OCS.FC_OCS_OSPECIMAN_NAME(D.DOSCODE, D.SLIPNO) SPECNAME                                           \r";
                        SQL += "    ,   A.CBUN                                                                                                   \r";
                        SQL += "    ,   DECODE(TRIM(A.ORDERCODE), '', 'DEL', '') GBDEL                                                           \r";
                        SQL += "    ,   A.DISPHEADER                                                                                             \r";
                        SQL += "    ,   A.NEXTCODE                                                                                               \r";
                        SQL += "   FROM KOSMOS_OCS.OCS_ORDERCODE A                                                                               \r";
                        SQL += "      , KOSMOS_PMPA.BAS_SUT      B                                                                               \r";
                        SQL += "      , KOSMOS_PMPA.BAS_SUN      C                                                                               \r";
                        SQL += "      , KOSMOS_OCS.OCS_IORDER    D                                                                               \r";
                        SQL += "  WHERE A.SUCODE    = B.SUCODE(+)                                                                                \r";
                        SQL += "    AND A.SUCODE    = C.SUNEXT(+)                                                                                \r";
                        SQL += "    AND D.ORDERCODE = A.ORDERCODE(+)                                                                             \r";
                        SQL += "    AND D.SLIPNO    = A.SLIPNO(+)                                                                                \r";
                        SQL += "    AND PTNO        = '" + clsOrdFunction.Pat.PtNo + "'                                                          \r";
                        SQL += "    AND D.BDATE = TO_DATE('" + cMaxBDate + "', 'YYYY-MM-DD')                                                     \r";
                        SQL += "    AND D.DEPTCODE <> 'PC'                                                                                       \r";
                        SQL += "    AND (D.GBPOSITIOn = ' '                                                                                      \r";
                        SQL += "     OR D.GBPOSITION IS NULL                                                                                     \r";
                        SQL += "     OR D.GBPOSITION = 'R')                                                                                      \r"; //전화처방 전송 않하는부분 允(2006-08-01)

                        //2019-06-17  OS 전공의 황성현 요청으로 S/O 1TIME은 리피트 안되도록
                        SQL += "    AND (D.SLIPNO IN ('A1', 'A2') OR                                                                             \r";
                        SQL += "    (D.SLIPNO = 'A4' AND SUBSTR(UPPER(D.REMARK), 1, 7) <> '(1TIME)')                                             \r";
                                                                                                                                                 
                        SQL += "     OR (D.BUN >= '11'                                                                                           \r";
                        SQL += "    AND D.BUN <= '20'                                                                                            \r";
                        SQL += "    AND D.BUN <> '12'                                                                                            \r";
                        SQL += "    AND D.ORDERCODE NOT IN ('NEEDLE', 'H-M37V', 'H-NC', 'H-NH')  )                                               \r";
                        SQL += "     OR (A.SLIPNO = '0102' AND A.SUCODE NOT IN ('EX773','E6611','E6612','EX780',                                 \r";
                        SQL += "        'EV771','PTDDST','F6216P','F6222P', 'EY773') )                                                           \r";
                        //2021-02-10 재활의학과 전담 요청으로 '0101' 추가
                        SQL += "     OR (D.SLIPNO = '0101')                                                                                      \r";
                        SQL += "     OR (D.SLIPNO = 'A5'                                                                                         \r";
                        SQL += "    AND D.GBPOSITION = 'R')                                                                                      \r";
                        SQL += "     OR D.SUCODE IN ('SALS','PULMI',                                                                             \r";
                        //2012-09-28 MYST4A add
                        //2012-12-06 N-FT-HA add
                        //2020-03-31 PULMI2 add
                        //2020-11-10 VENTOLIN add
                        //2020-11-27 PULMICAN add 
                        SQL += "        'ATROS','PT-HO2','MYST4A','N-FT-HA','PT-HO05','PT-HO2','SBTA','ABEN-2A','PULMI2','VENTOLIN','PULMICAN')  \r";
                        SQL += "     OR D.SUCODE IN ('C3710', 'C3710', 'C3710A'))                                                                \r"; //2020-08-10, BST 처방 추가
                        SQL += "    AND D.ORDERCODE <> 'C/O'                                                                                     \r";
                        SQL += "    AND (D.ORDERSITE IS NULL OR D.ORDERSITE <> 'OPDX' )                                                          \r";  //'2013-04-09
                        SQL += "    AND (D.SUCODE IS NULL                                                                                        \r";
                        SQL += "     OR (TRIM(D.SUCODE) NOT IN (                                                                                 \r";
                        SQL += "                                SELECT SUCODE                                                                    \r";
                        SQL += "                                  FROM KOSMOS_PMPA.BAS_SUT                                                       \r";
                        SQL += "                                 WHERE DELDATE <TRUNC(SYSDATE)                                                   \r";
                        SQL += "                                   AND DELDATE >= TRUNC(SYSDATE-365)                                             \r";
                        SQL += "                                   AND BUN IN ('11','12','20')  )  )  )                                          \r"; //2013-05-25 삭제약 제외
                        SQL += "    AND (D.SUCODE IS NULL OR D.SUCODE NOT IN ('$$39') )                                                          \r"; //'2013-06-12                                       
                        SQL += "    AND D.GBSTATUS IN  (' ')                                                                                     \r";
                        //2019-08-23 유진호 임시                                                                                                   
                        SQL += "    AND D.QTY > 0                                                                                                \r";
                        SQL += "    AND (D.VER IS NULL OR D.VER <>'CPORDER')                                                                     \r";
                        SQL += "    AND (D.NURSEID   IS NULL OR D.NURSEID = ' ')                                                                 \r";
                        SQL += "    AND D.SLIPNO    <> 'TEL'                                                                                     \r";
                        //D/C 전회 처방 미표시
                        SQL += "    AND NOT EXISTS                                                                                               \r";
                        SQL += "    (                                                                                                            \r";
                        SQL += "      SELECT 1                                                                                                   \r";  
                        SQL += "        FROM KOSMOS_OCS.OCS_LASTORDER_MAPPING                                                                    \r";
                        SQL += "       WHERE PTNO     = D.PTNO                                                                                   \r";
                        SQL += "         AND BDATE    = D.BDATE                                                                                  \r";
                        SQL += "         AND ORDERNO  = D.ORDERNO                                                                                \r";
                        SQL += "    )                                                                                                            \r";
                        #endregion
                    }

                    SQL += "  ORDER BY GBSEND1, ENTDATE1, SEQNO1                                                                                 \r";

                    #endregion
                    #endregion

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return null;
                    }
                    
                    rtnDt = dt;
                    
                    dt.Dispose();
                    dt = null;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }
                #endregion //IPD
            }
            return rtnDt;
        }

        /// <summary>
        /// 신경외과의 경우 입원 후 최초 내원 시 입원상병 가지고 오기
        /// </summary>
        /// <param name="sPano"></param>
        public void fn_Read_Last_Ills(string sPano)
        {

        }

        /// <summary>
        /// 활력증상 Display
        /// </summary>
        /// <param name="spd"></param>
        public void fn_GetVital(FarPoint.Win.Spread.FpSpread spd)
        {
            /*
            '=====================================================
            '2009-12-24 김현욱
            '소아일 경우 서식지 자체가 다름 서식지 번호 : 1558
            '진료과가 PD 일경우 다른 프로세서 만들어야 함
            '작업예정
            '====================================================
    
    
    
            ssVital.MaxRows = 0
            'EMR >> Vital >> nix
            SQL = " SELECT MAX(CHARTTIME), "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it3') AS it3,  "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it4') AS it4,  "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it5') AS it5,  "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it6') AS it6,   "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it7') AS it7,  "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it8') AS it8,  "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it9') AS it9,  "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it10') AS it10,  "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it11') AS it11,  "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it12') AS it12, "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it13') AS it13,  "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it121') AS it121, "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it150') AS it150, "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it274') AS it274, "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it274') AS it274, "
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it14') AS it14 "
            SQL = SQL & vbLf & "   FROM KOSMOS_EMR.EMRXML  "
            SQL = SQL & vbCr & " WHERE EMRNO IN ("
            SQL = SQL & vbCr & " SELECT EMRNO FROM KOSMOS_EMR.EMRXMLMST "
            SQL = SQL & vbLf & "  WHERE FORMNO = 1562 "
            SQL = SQL & vbLf & "    AND PTNO = '" & Pat.PtNo & "'  "
            SQL = SQL & vbLf & "    AND CHARTDATE = '" & CurrentDateTime("D") & "') "
            SQL = SQL & vbLf & "GROUP BY extractValue(chartxml, '//it3'),  " '혈압 (Sys)it17
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it4'),  " '혈압 (Dia)it102
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it5'),  " '혈압측정위치
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it6'),            " '맥박it18
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it7'),  " '호흡it19
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it8'),  " '체온it20
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it9'),  " '체온측정위치
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it10'),  " '체중it14
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it11'),  " '신장it13
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it12'), " '배둘레
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it13'),  " 'FHR
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it121'), " '머리 둘레
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it150'), " '가슴 둘레
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it274'), " '비고
            SQL = SQL & vbLf & "             extractValue(chartxml, '//it14') " 'spO2
            SQL = SQL & vbLf & " ORDER by 1 DESC "  '2014-08-20  최종vital


            Result = AdoOpenSet(Rs, SQL, , , False)
    
            If RowIndicator > 0 Then
                With ssVital
                    .MaxRows = 1
                    .Row = 1
                    .Col = 1: .Text = Trim(Rs("it8") & "")
                    .Col = 2: .Text = Trim(Rs("it9") & "")
                    .Col = 3: .Text = Trim(Rs("it10") & "")
                    .Col = 4: .Text = Trim(Rs("it3") & "") & " / " & Trim(Rs("it4") & "")
                    .Col = 5: .Text = Trim(Rs("it5") & "")
                    .Col = 6: .Text = Trim(Rs("it6") & "")
                    .Col = 7: .Text = Trim(Rs("it7") & "")
                    .Col = 8: .Text = Trim(Rs("it11") & "")
                    .Col = 9: .Text = Trim(Rs("it12") & "")
                    .Col = 10: .Text = Trim(Rs("it13") & "")
                    .Col = 11: .Text = Trim(Rs("it121") & "")
                    .Col = 12: .Text = Trim(Rs("it150") & "")
                    .Col = 13: .Text = Trim(Rs("it14") & "")
                    .Col = 14: .Text = Trim(Rs("it274") & "")
                End With
            End If
    
            AdoCloseSet Rs  
            */
        }

        public DataTable Order_Read(FarPoint.Win.Spread.FpSpread SpdNm, string ArgOrderCode, string ArgSlipNo, int startRow)
        {
            DataTable rtnDt = null;

            if (ArgOrderCode == "") return null;

            clsOrdFunction.GstrSelOrderCode = ArgOrderCode.Trim();

            try
            {
                SQL = "";
                SQL += " SELECT a.DispHeader cDispHeader, a.OrderName cOrderName, a.DispRGB cDispRGB            \r";
                SQL += "      , a.GbBoth cGbBoth, a.GbInfo cGbInfo, a.DrugName cDrugName, b.SugbJ SuSugbJ       \r";
                SQL += "      , a.GbQty cGbQty, a.GbDosage cGbDosage, a.NextCode cNextCode, C.DAICODE           \r";
                SQL += "      , a.OrderNameS cOrderNameS, a.GbImiv cGbImiv, b.SuGbF, c.SuGbN, b.Bun BunSu       \r";
                SQL += "      , '' GBCOPY,    '' GbSunap, '' GBAUTOSEND, '' GBAUTOSEND2, a.Bun,   '' Tuyakno, A.SENDDEPT    \r";
                SQL += "      , A.OrderCode, 1 RealQty, 1 GbDiv,     '' Nal,         '' GBER,     '' GbSelf     \r";
                SQL += "      , '' Remark,   '' RES,    '' Multi,    A.Sucode,       A.Slipno,    a.SPECCODE DosCode    \r";
                SQL += "      , '' GbBoth,   '' Gbinfo, '' OrderNo,  '' GbSunap,     '' DRCode,   '' MultiRemark\r";
                SQL += "      , '' DUR,      '' Resv,   '' GbTax,    '' ScodeRemark, '' GBSPC_NO, '' ScodeSayu  \r";
                SQL += "      , '' GbFM,     '' Sabun,  '' OCSDRUG,  '' ASA,         '' PCHASU,   '' SUBUL_WARD \r";
                SQL += "      , DECODE(A.GBDOSAGE, '0', '', KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(a.SPECCODE)) DOSNAME \r";
                SQL += "      , DECODE(A.GBDOSAGE, '0', '', KOSMOS_OCS.FC_OCS_ODOSAGE_DIV(a.SPECCODE)) DIV      \r";
                SQL += "      , KOSMOS_OCS.FC_OCS_OSPECIMAN_NAME(a.SPECCODE, A.SLIPNO) SPECNAME                 \r";
                SQL += "      , DECODE(a.GbImiv, '4', KOSMOS_OCS.FC_OCS_ENDO_REMARK('" + clsOrdFunction.Pat.PtNo + "', a.ORDERCODE) \r";
                SQL += "                       , '5', KOSMOS_OCS.FC_OCS_ENDO_REMARK('" + clsOrdFunction.Pat.PtNo + "', a.ORDERCODE) \r";
                SQL += "                       , '6', KOSMOS_OCS.FC_OCS_ENDO_REMARK('" + clsOrdFunction.Pat.PtNo + "', a.ORDERCODE) \r";
                SQL += "                       , '8', KOSMOS_OCS.FC_OCS_EXAM_ANATMST('" + clsOrdFunction.Pat.PtNo + "', a.ORDERCODE, 'I', '" + clsOrdFunction.Pat.DeptCode + "'), '') OPINION_YN    \r";
                SQL += "      , a.SPECCODE                                                                      \r";
                SQL += "      , a.CBUN                                                                          \r";
                SQL += "      , a.SUBRATE                                                                       \r";
                SQL += "      , '' contents,   '' bcontents                                                     \r";
                SQL += "      , '' PRN_REMARK, '' PRN_INS_GBN, '' PRN_INS_UNIT, '' POWDER_SAYU, '' PRN_INS_MAX  \r";
                SQL += "      , '' PRN_DOSCODE, '' PRN_TERM, '' PRN_NOTIFY, '' PRN_UNIT, '' GBPRN               \r";
                SQL += "      , '' QTY                                                                          \r";
                SQL += "      , '' GBPORT                                                                       \r";
                #region 전산업무 의뢰서(2021-538) 처리
                if (clsOrdFunction.Pat.PtNo.NotEmpty() && clsOrdFunction.Pat.GbIO.NotEmpty() &&
                    clsOrdFunction.Pat.GbIO.Equals("O"))
                {
                    SQL += "      , CASE WHEN EXISTS (                                                              \r";
                    SQL += "                              SELECT 1                                                  \r";
                    SQL += "                                FROM KOSMOS_OCS.ETC_JUPMST                              \r";
                    SQL += "                               WHERE GUBUN4 = '2'                                       \r";
                    SQL += "                                 AND PTNO = '" + clsOrdFunction.Pat.PtNo + "'           \r";
                    SQL += "                                 AND ORDERCODE  = A.ORDERCODE                           \r";
                    SQL += "                                 AND BDATE  < TRUNC(SYSDATE)                            \r";
                    SQL += "                                 AND GBIO = 'O'                                         \r";
                    SQL += "      						     AND EXISTS                                             \r";
                    SQL += "      						     (                                                      \r";
                    SQL += "      						     		SELECT 1                                        \r";
                    SQL += "      						     		  FROM KOSMOS_PMPA.BAS_BCODE                    \r";
                    SQL += "      	                               WHERE GUBUN = '외래_생리기능검사'                   \r";
                    SQL += "                                           AND CODE  = TRIM(A.ORDERCODE)                \r";
                    SQL += "      						     )                                                      \r";
                    SQL += "                           ) THEN '1' END  IS_MC_EXAM_ORDER                             \r";
                }
                #endregion


                SQL += "      , B.DELDATE                                                                           \r";
                SQL += "   FROM KOSMOS_OCS.OCS_ORDERCODE a                                                          \r";
                SQL += "      , KOSMOS_PMPA.BAS_SUT      b                                                          \r";
                SQL += "      , KOSMOS_PMPA.BAS_SUN      c                                                          \r";
                SQL += "  WHERE ORDERCODE = '" + ArgOrderCode.Trim() + "'                                           \r";
                SQL += "    AND SLIPNO    = '" + ArgSlipNo + "'                                                     \r";
                SQL += "    AND A.SUCODE    = B.SUCODE(+)                                                           \r";
                SQL += "    AND A.SUCODE    = C.SUNEXT(+)                                                           \r";
                SQL += "    AND (B.DELDATE IS NULL OR B.DELDATE > TRUNC(SYSDATE))                                  \r";

                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon); 
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog("함수명 : " + "Order_Read" + ComNum.VBLF + SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }

                if (dt1 != null)
                {
                    rowcounter = 0;
                    rowcounter = dt1.Rows.Count;

                    clsOrdDisp.GOrdCnt = dt1.Rows.Count;

                    if (rowcounter > 0)
                    {
                        //if (startRow == 0)
                        //{
                        //    SP.Spread_All_Clear(SpdNm);
                        //    SpdNm.ActiveSheet.RowCount = 300;
                        //}
                        clsOrdFunction.GstrSpecNm = "";
                        clsOrdFunction.GstrSpecCd = "";
                        clsOrdFunction.GstrSelSendDeptOrder = "";
                        clsOrdFunction.GstrSelSendDeptPrint = "";

                        if (string.IsNullOrWhiteSpace(clsOrdFunction.Pat.Bi)) clsOrdFunction.Pat.Bi = "11";

                        //Read_Suga(dt.Rows[0]["SUCODE"].ToString(), clsOrdFunction.Pat.Bi);

                        //clsOrdFunction.GnSunapOrdCount = dt1.Rows.Count; ;

                        rtnDt = dt1;
                    }
                    else
                    {
                        MessageBox.Show("OrderCode 가 없거나 현재 사용할 수 없습니다", "재입력 요망!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                    dt1.Dispose();
                    dt1 = null;
                }
               
                return rtnDt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox("함수명 : " + "Order_Read" + ComNum.VBLF + ex.Message);
                clsDB.SaveSqlErrLog("함수명 : " + "Order_Read" + ComNum.VBLF + ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null; 
            }
        }
        
        public DataTable fn_PRN_Order_Read(FarPoint.Win.Spread.FpSpread SpdNm, string ArgOrderCode, string ArgSlipNo, int startRow)
        {
            DataTable rtnDt = null;

            if (ArgOrderCode == "") return null;

            clsOrdFunction.GstrSelOrderCode = ArgOrderCode.Trim();

            try
            {
                SQL = "";
                SQL += " SELECT A.DISPHEADER CDISPHEADER, A.ORDERNAME CORDERNAME, A.DISPRGB CDISPRGB                                                                                                \r";
                SQL += "      , A.GBBOTH CGBBOTH, A.GBINFO CGBINFO, A.DRUGNAME CDRUGNAME                                                                                                            \r";
                SQL += "      , A.GBQTY CGBQTY, A.GBDOSAGE CGBDOSAGE, A.NEXTCODE CNEXTCODE, C.DAICODE                                                                                               \r";
                SQL += "      , A.ORDERNAMES CORDERNAMES, A.GBIMIV CGBIMIV, B.SUGBF, C.SUGBN, B.BUN BUNSU                                                                                           \r";
                SQL += "      , '' GBCOPY,    '' GBSUNAP, '' GBAUTOSEND, '' GBAUTOSEND2, A.BUN,      '' TUYAKNO                                                                                     \r";
                SQL += "      , A.ORDERCODE, 1 REALQTY,   KOSMOS_OCS.FC_OCS_ODOSAGE_DIV(A.SPECCODE) GBDIV                                                                                           \r";
                SQL += "      , '' NAL,         '' GBER,     '' GBSELF                                                                                                                              \r";
                //SQL += "      , '' REMARK,   '' RES,    '' MULTI,    A.SUCODE,       A.SLIPNO,    '' DOSCODE    \R";
                SQL += "      , '' REMARK,   '' RES,    '' MULTI,    A.SUCODE,       A.SLIPNO,    A.SPECCODE CDOSCODE                                                                               \r";
                SQL += "      , '' GBBOTH,   '' GBINFO, '' ORDERNO,  '' GBSUNAP,     '' DRCODE,   '' MULTIREMARK                                                                                    \r";
                SQL += "      , '' DUR,      '' RESV,   '' GBTAX,    '' SCODEREMARK, '' GBSPC_NO, '' SCODESAYU                                                                                      \r";
                SQL += "      , '' GBFM,     '' SABUN,  '' OCSDRUG,  '' ASA,         '' PCHASU,   '' SUBUL_WARD                                                                                     \r";
                //SQL += "      , KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(A.SPECCODE) DOSNAME                              \R";
                //SQL += "      , KOSMOS_OCS.FC_OCS_ODOSAGE_DIV(A.SPECCODE) DIV                                   \R";
                SQL += "      , CASE WHEN (A.BUN = '11' OR A.BUN = '12') AND A.SUCODE <> 'NIG06' THEN '010501'                                                                                      \r";
                SQL += "             WHEN(A.BUN = '11' OR A.BUN = '12') AND A.SUCODE = 'NIG06' THEN '490301'                                                                                        \r";
                SQL += "             WHEN A.BUN = '20' AND(A.SUCODE = 'NSB' OR A.SUCODE = 'PACETA') THEN '930120'                                                                                   \r";
                SQL += "             WHEN A.BUN = '20' AND(A.SUCODE = 'MIN2') THEN '910120'                                                                                                         \r";
                SQL += "             WHEN A.BUN = '20' AND A.SUCODE NOT IN('NSB', 'PACETA', 'MIN2') THEN '920120' END DOSCODE                                                                       \r";
                SQL += "      , CASE WHEN (A.BUN = '11' OR A.BUN = '12') AND A.SUCODE <> 'NIG06' THEN KOSMOS_OCS.FC_OCS_ODOSAGE_NAME('010501')                                                      \r";
                SQL += "             WHEN (A.BUN = '11' OR A.BUN = '12') AND A.SUCODE = 'NIG06' THEN ''                                                                                             \r";
                SQL += "             WHEN A.BUN = '20' AND (A.SUCODE = 'NSB' OR A.SUCODE = 'PACETA') THEN KOSMOS_OCS.FC_OCS_ODOSAGE_NAME('930120')                                                  \r";
                SQL += "             WHEN A.BUN = '20' AND (A.SUCODE = 'MIN2') THEN KOSMOS_OCS.FC_OCS_ODOSAGE_NAME('910120')                                                                        \r";
                SQL += "             WHEN A.BUN = '20' AND A.SUCODE NOT IN('NSB', 'PACETA', 'MIN2') THEN KOSMOS_OCS.FC_OCS_ODOSAGE_NAME('920120') END DOSNAME                                       \r";
                SQL += "      , CASE WHEN (A.BUN = '11' OR A.BUN = '12') AND A.SUCODE <> 'NIG06' THEN KOSMOS_OCS.FC_OCS_ODOSAGE_DIV('010501')                                                       \r";
                SQL += "             WHEN (A.BUN = '11' OR A.BUN = '12') AND A.SUCODE = 'NIG06' THEN ''                                                                                             \r";
                SQL += "             WHEN A.BUN = '20' AND (A.SUCODE = 'NSB' OR A.SUCODE = 'PACETA') THEN KOSMOS_OCS.FC_OCS_ODOSAGE_DIV('930120')                                                   \r";
                SQL += "             WHEN A.BUN = '20' AND (A.SUCODE = 'MIN2') THEN KOSMOS_OCS.FC_OCS_ODOSAGE_DIV('910120')                                                                         \r";
                SQL += "             WHEN A.BUN = '20' AND A.SUCODE NOT IN('NSB', 'PACETA', 'MIN2') THEN KOSMOS_OCS.FC_OCS_ODOSAGE_DIV('920120') END DIV                                            \r";
                SQL += "      , KOSMOS_OCS.FC_OCS_OSPECIMAN_NAME(A.SPECCODE, A.SLIPNO) SPECNAME                                                                                                     \r";
                SQL += "      , DECODE(A.GBIMIV, '4', KOSMOS_OCS.FC_OCS_ENDO_REMARK('" + clsOrdFunction.Pat.PtNo + "', A.ORDERCODE)                                                                 \r";
                SQL += "                       , '5', KOSMOS_OCS.FC_OCS_ENDO_REMARK('" + clsOrdFunction.Pat.PtNo + "', A.ORDERCODE)                                                                 \r";
                SQL += "                       , '6', KOSMOS_OCS.FC_OCS_ENDO_REMARK('" + clsOrdFunction.Pat.PtNo + "', A.ORDERCODE)                                                                 \r";
                SQL += "                       , '8', KOSMOS_OCS.FC_OCS_EXAM_ANATMST('" + clsOrdFunction.Pat.PtNo + "', A.ORDERCODE, 'I', '" + clsOrdFunction.Pat.DeptCode + "'), '') OPINION_YN    \r";
                SQL += "      , A.SPECCODE                                                                                                                                                          \r";
                SQL += "      , A.CBUN                                                                                                                                                              \r";
                SQL += "   FROM KOSMOS_OCS.OCS_ORDERCODE A                                                                                                                                          \r";
                SQL += "      , KOSMOS_PMPA.BAS_SUT      B                                                                                                                                          \r";
                SQL += "      , KOSMOS_PMPA.BAS_SUN      C                                                                                                                                          \r";
                SQL += "  WHERE ORDERCODE   = '" + ArgOrderCode.Trim() + "'                                                                                                                         \r";
                SQL += "    AND SLIPNO      = '" + ArgSlipNo + "'                                                                                                                                   \r";
                SQL += "    AND A.SUCODE    = B.SUCODE(+)                                                                                                                                           \r";
                SQL += "    AND A.SUCODE    = C.SUNEXT(+)                                                                                                                                           \r";

                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }

                rowcounter = 0;
                rowcounter = dt1.Rows.Count;

                if (rowcounter > 0)
                {
                    //if (startRow == 0)
                    //{
                    //    SP.Spread_All_Clear(SpdNm);
                    //    SpdNm.ActiveSheet.RowCount = 300;
                    //}

                    clsOrdFunction.GstrSpecNm = "";
                    clsOrdFunction.GstrSpecCd = "";
                    clsOrdFunction.GstrSelSendDeptOrder = "";
                    clsOrdFunction.GstrSelSendDeptPrint = "";

                    if (string.IsNullOrWhiteSpace(clsOrdFunction.Pat.Bi)) clsOrdFunction.Pat.Bi = "11";

                    //Read_Suga(dt.Rows[0]["SUCODE"].ToString(), clsOrdFunction.Pat.Bi);

                    //clsOrdFunction.GnReadOrder = dt1.Rows.Count; 

                    rtnDt = dt1;
                }
                else
                {
                    MessageBox.Show("OrderCode 가 없거나 현재 사용할 수 없습니다", "재입력 요망!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                dt1.Dispose();
                dt1 = null;
                return rtnDt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }
        }


        public DataTable SetSPecialOrder_Read(string sRowId)
        {
            DataTable rtnDt = null;

            if (sRowId == "") return null;

            try
            {
                SQL = "";
                //SQL += " SELECT '' cDispHeader, D.REMARK cOrderName, '' cDispRGB                                \r";
                SQL += " SELECT '' CDISPHEADER                                                                          \r";
                SQL += "      , CASE WHEN TRIM(B.ORDERCODE) = 'B001' OR                                                 \r";
                SQL += "                  TRIM(B.ORDERCODE) = 'B002' OR                                                 \r";
                SQL += "                  TRIM(B.ORDERCODE) = 'B003' OR                                                 \r";
                SQL += "                  TRIM(B.ORDERCODE) = 'B004' OR                                                 \r";
                SQL += "                  TRIM(B.ORDERCODE) = 'B005' OR                                                 \r";
                SQL += "                  TRIM(B.ORDERCODE) = 'B006' OR                                                 \r";
                SQL += "                  TRIM(B.ORDERCODE) = 'B009' THEN B.ORDERNAME ELSE D.REMARK END CORDERNAME      \r";
                SQL += "      , '' CDISPRGB                                                                             \r";
                SQL += "      , '' CGBBOTH, '' CGBINFO, '' CDRUGNAME                                                    \r";
                SQL += "      , '' CGBQTY, '' CGBDOSAGE, '' CNEXTCODE, '' DAICODE                                       \r";
                SQL += "      , '' CORDERNAMES, '' CGBIMIV, '' SUGBF, '' BUNSU                                          \r";
                SQL += "      , '' GBCOPY,    '' GBSUNAP, '' GBAUTOSEND, '' GBAUTOSEND2, D.BUN,   '' TUYAKNO            \r";
                SQL += "      , D.ORDERCODE, D.REALQTY, D.GBDIV,     D.NAL,          D.GBER,      D.GBSELF              \r";
                SQL += "      , D.REMARK,    '' RES,    '' MULTI,    D.SUCODE,       D.SLIPNO,    D.DOSCODE             \r";
                SQL += "      , D.GBBOTH,    D.GBINFO,  '' ORDERNO,  '' GBSUNAP,     '' DRCODE,   '' MULTIREMARK        \r";
                SQL += "      , '' DUR,      '' RESV,   '' GBTAX,    '' SCODEREMARK, '' GBSPC_NO, '' SCODESAYU          \r";
                SQL += "      , '' GBFM,     '' SABUN,  '' OCSDRUG,  '' ASA,         '' PCHASU,   '' SUBUL_WARD         \r";
                SQL += "      , D.GBPRN,     '' SUGBN                                                                   \r";
                SQL += "      , '' DOSNAME                                                                              \r";
                SQL += "      , '' DIV                                                                                  \r";
                SQL += "      , '' SPECNAME                                                                             \r";
                SQL += "      , D.CBUN, D.GBGROUP                                                                       \r";
                SQL += "      , D.PRN_REMARK, D.PRN_INS_GBN, D.PRN_INS_UNIT, D.PRN_INS_SDATE, D.PRN_INS_EDATE           \r";
                SQL += "      , D.PRN_INS_MAX, D.PRN_DOSCODE, D.PRN_TERM, D.PRN_NOTIFY, D.PRN_UNIT, D.SUBUL_WARD        \r";
                SQL += "      , '' SUSUGBJ                                                                              \r";
                SQL += "      , '' GBPORT                                                                               \r";
                //2021-01-11 추가
                SQL += "      , D.BCONTENTS, D.CONTENTS, D.TUYEOPOINT, D.TUYEOTIME                                      \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OPRM      D                                                              \r";
                SQL += "      , KOSMOS_OCS.OCS_ORDERCODE B                                                              \r";
                SQL += "  WHERE D.ROWID = '" + sRowId.Trim() + "'                                                       \r";
                SQL += "    AND D.ORDERCODE = B.ORDERCODE(+)                                                            \r";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }

                rowcounter = 0;
                rowcounter = dt1.Rows.Count;

                GOrdCnt = dt1.Rows.Count;

                if (rowcounter > 0)
                {
                    clsOrdFunction.GstrSpecNm = "";
                    clsOrdFunction.GstrSpecCd = "";
                    clsOrdFunction.GstrSelSendDeptOrder = "";
                    clsOrdFunction.GstrSelSendDeptPrint = "";

                    if (string.IsNullOrWhiteSpace(clsOrdFunction.Pat.Bi)) clsOrdFunction.Pat.Bi = "11";

                    rtnDt = dt1;
                }
                else
                {
                    MessageBox.Show("OrderCode 가 없거나 현재 사용할 수 없습니다", "재입력 요망!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                dt1.Dispose();
                dt1 = null;
                return rtnDt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }
        }

        public DataTable OCS_OPRM_Read(string sRowId)
        {
            DataTable rtnDt = null;

            if (sRowId == "") return null;

            try
            {
                SQL = "";
                SQL += " SELECT                                                                                 \r";
                SQL += "      *                                                                                 \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OPRM                                                             \r";
                SQL += "  WHERE ROWID = '" + sRowId.Trim() + "'                                                 \r";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }

                rowcounter = 0;
                rowcounter = dt1.Rows.Count;

                GOrdCnt = dt1.Rows.Count;

                if (rowcounter > 0)
                {
                    rtnDt = dt1;
                }
                else
                {
                    MessageBox.Show("함수 : OCS_OPRM_Read 에러", "재입력 요망!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                dt1.Dispose();
                dt1 = null;
                return rtnDt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }
        }

        public DataTable SetOrder_Read(string sSetName, string sDeptDr, string sOrderCode, string strRowId, string argOrdGbn, string argGBIO, FarPoint.Win.Spread.FpSpread SpdNm, int startRow)
        {
            DataTable rtnDt = null;

            if (sOrderCode == "") return null;

            clsOrdFunction.GstrSelOrderCode = sOrderCode.Trim();

            try
            {
                SQL = "";
                SQL += " SELECT a.DispHeader cDispHeader, a.OrderName cOrderName, a.DispRGB cDispRGB            \r";
                SQL += "      , a.GbBoth cGbBoth, a.GbInfo cGbInfo, a.DrugName cDrugName                        \r";
                SQL += "      , d.Qty cGbQty, a.GBDosage cGbDosage, a.NextCode cNextCode, C.DAICODE             \r";
                SQL += "      , a.OrderNameS cOrderNameS, a.GbImiv cGbImiv, b.SuGbF, b.Bun BunSu                \r";
                SQL += "      , '' GBCOPY,    '' GbSunap, '' GBAUTOSEND, '' GBAUTOSEND2, A.Bun,   '' Tuyakno    \r";
                SQL += "      , A.OrderCode, d.RealQty, d.GbDiv,     d.Nal,          d.GBER,      d.GbSelf      \r";
                //SQL += "      , d.Remark,    '' RES,    '' Multi,    d.Sucode                                   \r";  //아래로 수정(2018.08.11)
                SQL += "      , d.Remark,    '' RES,    '' Multi,    nvl(trim(d.Sucode), trim(a.Sucode)) Sucode \r";
                SQL += "      , A.Slipno,    d.DosCode                                                          \r";
                SQL += "      , d.GbBoth,    d.Gbinfo,  '' OrderNo,  '' GbSunap,     '' DRCode,   '' MultiRemark\r";
                SQL += "      , '' DUR,      '' Resv,   '' GbTax,    '' ScodeRemark, '' GBSPC_NO, '' ScodeSayu  \r";
                SQL += "      , '' GbFM,     '' Sabun,  '' OCSDRUG,  '' ASA,         '' PCHASU,   '' SUBUL_WARD \r";
                SQL += "      , d.GBPRN,     c.SUGBN                                                            \r";
                //SQL += "      , KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(d.DOSCODE) DOSNAME                               \r";
                //SQL += "      , KOSMOS_OCS.FC_OCS_ODOSAGE_DIV(d.DOSCODE) DIV                                    \r";
                SQL += "      , (SELECT DOSNAME FROM KOSMOS_OCS.OCS_ODOSAGE WHERE DOSCODE = d.doscode) DOSNAME  \r";
                SQL += "      , (SELECT GBDIV FROM KOSMOS_OCS.OCS_ODOSAGE WHERE DOSCODE = d.doscode) DIV        \r";
                SQL += "      , KOSMOS_OCS.FC_OCS_OSPECIMAN_NAME(D.DOSCODE, A.SLIPNO) SPECNAME                  \r";
                SQL += "      , a.CBUN, d.PRN_REMARK, d.PRN_INS_UNIT, d.PRN_DOSCODE, d.PRN_INS_GBN              \r";
                SQL += "      , d.PRN_TERM, d.PRN_NOTIFY, d.PRN_INS_SDATE, d.PRN_INS_EDATE, PRN_INS_MAX         \r";
                SQL += "      , d.GBTFLAG, d.GBGROUP, a.SENDDEPT, b.SUGBJ SUSUGBJ                               \r";
                //2020-12-29 BCONTENTS, CONTENTS 추가
                SQL += "      , d.GBPORT, d.BCONTENTS, d.CONTENTS                                               \r";
                //2021-01-11 TUYEOPOINT, TUYEOTIME 추가
                SQL += "      , d.TUYEOPOINT, d.TUYEOTIME                                                       \r";
                //2021-11-05 
                SQL += "      , CASE WHEN d.GBINFO IS NOT NULL AND EXISTS                                       \r";
                SQL += "      	(                                                                               \r";
                SQL += "      		SELECT 1                                                                    \r";
                SQL += "      		  FROM KOSMOS_OCS.OCS_SUBCODE SUB                                           \r";
                SQL += "      		 WHERE SUB.ORDERCODE = D.ORDERCODE                                          \r";
                SQL += "      		   AND SUB.SUCODE = D.SUCODE                                                \r";
                SQL += "      		   AND SUB.SUBNAME = D.GBINFO                                               \r";
                SQL += "      		   AND SUB.DELDATE IS NOT NULL                                              \r";
                SQL += "      	) THEN '1'                                                                      \r";
                SQL += "      	END IS_SUBCODE_ERROR                                                            \r";
                SQL += "      , CASE WHEN B.DELDATE <= TRUNC(SYSDATE) THEN '1' END IS_DELSUGA                   \r";
                SQL += "   FROM KOSMOS_OCS.OCS_ORDERCODE a                                                      \r";
                SQL += "      , KOSMOS_PMPA.BAS_SUT      b                                                      \r";
                SQL += "      , KOSMOS_PMPA.BAS_SUN      c                                                      \r";
                SQL += "      , KOSMOS_OCS.OCS_OPRM      d                                                      \r";
                SQL += "  WHERE d.OrderCode = '" + sOrderCode.Trim() + "'                                       \r";
                if (sDeptDr.Trim() == "MG" || sDeptDr.Trim() == "MC" || sDeptDr.Trim() == "MP" ||
                    sDeptDr.Trim() == "ME" || sDeptDr.Trim() == "MN" || sDeptDr.Trim() == "MR" || sDeptDr.Trim() == "MI")
                {
                    SQL += "    AND d.DEPTDR    IN('" + sDeptDr.Trim() + "', 'MD')                              \r";
                }
                else
                {
                    SQL += "    AND d.DEPTDR    = '" + sDeptDr + "'                                             \r";
                }
                SQL += "    AND d.PRMNAME   = '" + sSetName + "'                                                \r";
                SQL += "    AND a.SuCode    = b.SuCode(+)                                                       \r";
                SQL += "    AND a.SuCode    = C.SUNEXT(+)                                                       \r";
                SQL += "    AND d.OrderCode = a.OrderCode(+)                                                    \r";
                //SQL += "    AND a.CBun       = d.CBun                                                           \r";
                SQL += "    AND d.Bun       = a.Bun(+)                                                          \r";
                SQL += "    and d.ROWID     = '" + strRowId + "'                                                \r";
                //SQL += "    and (A.senddept != 'N' or A.senddept is null)                                       \r";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }

                rowcounter = 0;
                rowcounter = dt1.Rows.Count;

                GOrdCnt = dt1.Rows.Count;

                if (rowcounter > 0)
                {
                    clsOrdFunction.GstrSpecNm = "";
                    clsOrdFunction.GstrSpecCd = "";
                    clsOrdFunction.GstrSelSendDeptOrder = "";
                    clsOrdFunction.GstrSelSendDeptPrint = "";                    

                    if (string.IsNullOrWhiteSpace(clsOrdFunction.Pat.Bi)) clsOrdFunction.Pat.Bi = "11";

                    if (dt1.Rows[0]["ORDERCODE"].ToString().Trim().IsNullOrEmpty() || 
                        dt1.Rows[0]["SENDDEPT"].ToString().Trim().Equals("N") ||
                        dt1.Rows[0]["IS_SUBCODE_ERROR"].ToString().Trim().Equals("1") ||
                        dt1.Rows[0]["IS_DELSUGA"].ToString().Trim().Equals("1"))
                    {
                        if (argOrdGbn != "SETORDER")
                        {
                            MessageBox.Show("OrderCode : " + dt1.Rows[0]["ORDERCODE"].ToString().Trim() + ComNum.VBLF + "가 없거나 현재 사용할 수 없습니다", "재입력 요망!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return rtnDt;
                        }

                        if (argGBIO == "OPD")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DC].Value = true;
                            SpdNm.ActiveSheet.Cells[startRow, 0, startRow, SpdNm.ActiveSheet.ColumnCount - 1].Locked = true;
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = dt1.Rows[0]["ORDERCODE"].ToString().Trim() + " " + "삭제된 코드입니다.. 변경요망 ";
                            SpdNm.ActiveSheet.Cells[startRow, 0, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                        }
                        else if (argGBIO == "IPD")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DC].Value = true;
                            SpdNm.ActiveSheet.Cells[startRow, 0, startRow, SpdNm.ActiveSheet.ColumnCount - 1].Locked = true;
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt1.Rows[0]["ORDERCODE"].ToString().Trim() + " " + "삭제된 코드입니다.. 변경요망 ";
                            SpdNm.ActiveSheet.Cells[startRow, 0, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                        }
                        else if (argGBIO == "ER")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DC].Value = true;
                            SpdNm.ActiveSheet.Cells[startRow, 0, startRow, SpdNm.ActiveSheet.ColumnCount - 1].Locked = true;
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = dt1.Rows[0]["ORDERCODE"].ToString().Trim() + " " + "삭제된 코드입니다.. 변경요망 ";
                            SpdNm.ActiveSheet.Cells[startRow, 0, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                        }
                        return rtnDt;
                    }

                    rtnDt = dt1;
                }
                else
                {
                    MessageBox.Show("OrderCode 가 없거나 현재 사용할 수 없습니다", "재입력 요망!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                dt1.Dispose();
                dt1 = null;
                return rtnDt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }
        }

        public DataTable SetDiag_Read(string sillCode)
        {
            DataTable rtnDt = null;

            if (sillCode == "") return null;

            try
            {
                SQL = "";
                SQL += " SELECT ILLCODE, NVL(ILLNAMEE, ILLNAMEK) ILLNAME                                                                                        \r";
                SQL += "      , ILLNAMEE, ILLNAMEK                                                                                                              \r";
                SQL += "      , ILLCLASS, NAMESPACING, DISPHEADER                                                                                               \r";
                SQL += "      , RETURNVAL, NOUSE, SEX                                                                                                           \r";
                SQL += "      , INFECT, GUBUN, ILLUPCODE                                                                                                        \r";
                SQL += "      , ILLCODED, GBINFECT, KCD6                                                                                                        \r";
                SQL += "      , SDATE, DDATE, NOUSEDATE                                                                                                         \r";
                SQL += "      , CASE                                                                                                                            \r";
                SQL += "           WHEN GBV252 = '*' THEN '*'                                                                                                   \r";
                SQL += "           WHEN GBV352 = '*' THEN '*'                                                                                                   \r";
                SQL += "           ELSE ''                                                                                                                      \r";
                SQL += "        END GbV252                                                                                                                      \r";
                SQL += "      , GBVCODE, GBVCODE1, GBVCODE2, KCD7                                                                                               \r";
                SQL += "      , ILLNAMEK_OLD, ILLNAMEE_OLD, GBCHK                                                                                               \r";
                SQL += "      , KCDOLD, IPDETC, REPCODE                                                                                                         \r";
                SQL += "      ,  (SELECT NVL(ILLNAMEE,ILLNAMEK)  FROM KOSMOS_PMPA.BAS_ILLS WHERE ILLCODE = A.REPCODE AND ROWNUM = 1) AS REPNAME                 \r";
                SQL += "      ,  CASE WHEN EXISTS(SELECT 1 FROM KOSMOS_PMPA.VIEW_MID_CHECK_POA_ILLS WHERE ILLCODE = A.ILLCODE) THEN 'E' ELSE 'Y' END POA        \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_ILLS A                                                                                                          \r";
                SQL += "  WHERE ILLCODE = '" + sillCode + "'                                                                                                    \r";
                

                SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnDt;
                }

                if (dt1.Rows.Count > 0)
                {
                    rtnDt = dt1;
                }
                else
                {
                    //MessageBox.Show("상병코드가 없거나 현재 사용할 수 없습니다", "재입력 요망!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    rtnDt = null;
                }

                dt1.Dispose();
                dt1 = null;
                return rtnDt;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnDt;
            }
        }

        public void Read_Suga(string ArgSuCode, string argBi)
        {
            BaseOrderInfo.SI.BUN = "";
            BaseOrderInfo.SI.CBUN = "";
            BaseOrderInfo.SI.BOSELF = "";
            BaseOrderInfo.SI.BHSELF = "";
            BaseOrderInfo.SI.TASELF = "";
            BaseOrderInfo.SI.SNSELF = "";
            BaseOrderInfo.SI.ILSELF = "";
            BaseOrderInfo.SI.GISELF = "";
            BaseOrderInfo.SI.GBSPC = "";
            BaseOrderInfo.SI.BOPRICE = 0;
            BaseOrderInfo.SI.GBPX = "";

            if (string.IsNullOrWhiteSpace(argBi)) argBi = "110";

            try
            {
                SQL = "";
                SQL += " SELECT BOSELF, BHSELF, TASELF, SNSELF, ILSELF, GISELF, GBPX,   \r";
                SQL += "        GBSPC, BOPRICE, BUN, GBHOSP, CBUN                       \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_SUT                                     \r";
                SQL += "  WHERE SUCODE = '" + ArgSuCode.Trim() + "'                     \r";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    switch (argBi.Substring(0, 1))
                    {
                        case "1": BaseOrderInfo.SI.BOSELF = dt1.Rows[0]["BOSELF"].ToString(); break;
                        case "2":
                            BaseOrderInfo.SI.BHSELF = dt1.Rows[0]["BHSELF"].ToString();
                            BaseOrderInfo.SI.BOSELF = BaseOrderInfo.SI.BHSELF; break;
                        case "3": BaseOrderInfo.SI.TASELF = dt1.Rows[0]["TASELF"].ToString(); break;
                        case "4": BaseOrderInfo.SI.SNSELF = dt1.Rows[0]["SNSELF"].ToString(); break;
                        case "5": BaseOrderInfo.SI.ILSELF = dt1.Rows[0]["ILSELF"].ToString(); break;
                        default: break;
                    }

                    BaseOrderInfo.SI.BUN = dt.Rows[0]["BUN"].ToString();
                    BaseOrderInfo.SI.CBUN = dt.Rows[0]["CBUN"].ToString();
                    BaseOrderInfo.SI.GBSPC = dt.Rows[0]["GBSPC"].ToString();
                    BaseOrderInfo.SI.GBHOSP = dt.Rows[0]["GBHOSP"].ToString();
                    BaseOrderInfo.SI.GBPX = dt.Rows[0]["GBPX"].ToString();
                    if (dt.Rows[0]["BOPRICE"].ToString() != "")
                    {
                        BaseOrderInfo.SI.BOPRICE = Convert.ToDouble(dt.Rows[0]["BOPRICE"].ToString());
                    }

                    if (BaseOrderInfo.SI.GBSPC == "0") BaseOrderInfo.SI.GBSPC = "";

                    if (BaseOrderInfo.SI.BOSELF == "9") BaseOrderInfo.SI.BOSELF = "2";  //기본급여 시킴
                    if (BaseOrderInfo.SI.BOSELF == "0") BaseOrderInfo.SI.BOSELF = "";
                }
                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        public void fn_PRN_Order_Disp(DataTable dt, string argGBIO, FarPoint.Win.Spread.FpSpread SpdNm, int startRow, double nQty, string sEtc, string sGbInsulin, string sGbPrn)
        {
            string strSELECTOrderCode = "";
            string strSELECTOrderName = "";
            string strSELECTDosName = "";
            string strSELECTDosCode = "";
            string strSELECTSlipnos = "";
            string strSELECTBun = "";
            string strSELECTCBun = "";
            string strSELECTDiv = "";
            string strSELECTSpecCode = "";
            //string strSELECTSuCode = "";
            //string strSELECTGbInfo = "";

            string strUnit = "";
            string strName = "";

            
            if (argGBIO == "IPD")
            {
                #region // IPD
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();

                if (dt.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                {
                    strUnit = dt.Rows[0]["CORDERNAME"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = strUnit + " " + dt.Rows[0]["CORDERNAMES"].ToString();
                }
                else if (dt.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["CDISPHEADER"].ToString() + " " +
                    dt.Rows[0]["CORDERNAME"].ToString();
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString();
                }

                //금액적용시
                if (dt.Rows[0]["CGBBOTH"].ToString() == "1")
                {
                    strName = ComFunc.LeftH(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text, 30);
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = strName + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text;
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = (Convert.ToString(nQty) == "0" ? "1" : Convert.ToString(nQty));
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAL].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BCONTENTS].Text = "1";

                if (clsPublic.Gstr산제Chk == "OK")
                {
                    //if (OF.READ_POWDER(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "Y")
                    if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                    }
                }

                if (clsPublic.Gstr산제Chk == "OK")
                {
                    if (clsPublic.Gstr파우더New_STS == "Y")
                    {
                        if (clsOrderEtc.CHK_POWDER_SUCODE_CHK(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                        {
                            if (clsOrdFunction.GnReadOrder < startRow)
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].Text = "True";
                            }
                        }
                    }
                }
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NGT].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ER].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";

                if (clsPublic.GstrSugaFind == "PRN")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "P";
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POTABLE].Text = "";

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text = dt.Rows[0]["SUCODE"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BUN].Text = dt.Rows[0]["BUN"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text = dt.Rows[0]["SLIPNO"].ToString().Trim();

                //이상훈 (2018.04.27) 확인 필요
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = clsPublic.Gstr파우더_SuCode;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = VB.Left(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["SpecCode"].ToString(), 2), dt.Rows[0]["Bun"].ToString()), 7);
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPGUBUN].Text = VB.Right(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["SPECCODE"].ToString(), 2), dt.Rows[0]["BUN"].ToString()).Trim(), 3).Trim();

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBBOTH].Text = "";

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REMARK].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = dt.Rows[0]["CDISPRGB"].ToString();

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CGBBOTH].Text = dt.Rows[0]["GBBOTH"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CGBINFO].Text = dt.Rows[0]["GBINFO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBQTY].Text = dt.Rows[0]["CGBQTY"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBDOSAGE].Text = dt.Rows[0]["CGBDOSAGE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NEXTCODE].Text = dt.Rows[0]["CNEXTCODE"].ToString();

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BCONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString();

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBIMIV].Text = dt.Rows[0]["CGBIMIV"].ToString();

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ROWID].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SEQNO].Text = Convert.ToString(startRow);

                //선수납항목 체크
                if (dt.Rows[0]["SUGBN"].ToString() == "1")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUNSUNAP].Text = "S";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "(A)" + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString().Trim();
                
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDERSAYUMARK].Text = "#";
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDERSAYU].Text = dt.Rows[0]["SUCODE"].ToString() + ">>" + clsPublic.Gstr파우더_SuCode;
                //소견 체크
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.RESULT].Text = dt.Rows[0]["OPINION_YN"].ToString();

                string cDosCode = dt.Rows[0]["SPECCODE"].ToString();

                if (dt.Rows[0]["GBINFO"].ToString() == "1")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["GBINFO"].ToString();
                }
                else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "1")
                {
                    if (dt.Rows[0]["DOSNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBDOSAGE].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                        //if (dt.Rows[0]["GBDIV"].ToString() == "0")
                        if (dt.Rows[0]["DIV"].ToString() == "0")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";//용법을 바꾸면 따라서 총투량도 같이 바꾼다.
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";
                        }
                        else
                        {
                            //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt.Rows[0]["GBDIV"].ToString();
                            //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = dt.Rows[0]["GBDIV"].ToString();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = nQty.ToString();
                        }
                        //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSAGE].Text = dt.Rows[0]["SPECCODE"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                    }
                }
                else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "2")
                {
                    if (dt.Rows[0]["SPECNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["SPECNAME"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = dt.Rows[0]["SPECCODE"].ToString();
                    }
                }

                //추가 정보(방사선, 검사등) 2017-12-11 의미 없음.... SS1.Col = 11 에 Data 넣지 않음.
                //SS1.Col = 6
                //If Trim(SS1.Text) = "1" Then                    '추가 정보(방사선, 검사등)

                //    SS1.Col = 11

                //    If Trim(SS1.Text) <> "" Then

                //        GOrderFORM.SSOrder.Col = 4:    GOrderFORM.SSOrder.Text = SS1.Text
                //    End If
                //End If

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    SpdNm.ActiveSheet.Cells[startRow, 2, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(128, 0, 0);
                }
                else
                {
                    if (dt.Rows[0]["CDISPRGB"].ToString().Trim() != "")
                    {
                        if (dt.Rows[0]["CDISPRGB"].ToString().Trim() != "")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, 2, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                        }
                    }
                }
                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "80";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "";
                }

                strSELECTOrderCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text.Trim();
                strSELECTOrderName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text.Trim();
                strSELECTDosName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text.Trim();
                strSELECTDosCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSAGE].Text.Trim();
                strSELECTSlipnos = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text.Trim();
                strSELECTBun = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BUN].Text.Trim();
                strSELECTCBun = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CBUN].Text.Trim();
                strSELECTDiv = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text.Trim();
                strSELECTSpecCode = dt.Rows[0]["SPECCODE"].ToString().Trim();

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    SpdNm.ActiveSheet.Cells[startRow, 1, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.Aqua;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text;
                }
                else
                {
                    if (dt.Rows[0]["CDISPRGB"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, 1, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                    }
                }

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "80";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "";
                }

                if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "★항혈전 " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].ForeColor = Color.FromArgb(255, 0, 255);
                }

                if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "<!> " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                }

                if (clsOrderEtc.READ_SUGA_MR_EXPENSIVE_MEDICINE(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim(), clsOrdFunction.Pat.DeptCode) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "[재고X] " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                }

                clsOrdFunction.GstrSimCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text;
                clsOrdFunction.GstrSimFlag = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SIMFLAG].Text;
                clsOrdFunction.GstrSimYN = clsOrdFunction.SimSaGiJun_Check(clsDB.DbCon, clsOrdFunction.GstrSimFlag, clsOrdFunction.GstrSimCode, argGBIO);
                if (clsOrdFunction.GstrSimYN == "Y")
                {
                    frmPmpaJSimsaGijun f = new frmPmpaJSimsaGijun(clsOrdFunction.GstrSimCode);
                    f.ShowDialog();
                    OF.fn_ClearMemory(f);
                }
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SIMFLAG].Text = clsOrdFunction.GstrSimYN;

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.MAYAK].Text = OF.READ_MAYAK(clsDB.DbCon, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim());
                
                if (sEtc.Trim() != "")
                {
                    string sPrnRemark = "";
                    string sScale = "";
                    string sQty = "";
                    string sBDate = "";
                    string sEDate = "";
                    string sMaxDiv = "";
                    string sDiv = "";
                    string sTerm = "";
                    string sNotify = "";
                    string sOrdSeq = "";

                    //sPrnRemark = VB.Pstr(sEtc, "@@", 1);
                    //sScale = VB.Pstr(sEtc, "@@", 2);
                    //sBDate = VB.Pstr(sEtc, "@@", 3);
                    //sQty = VB.Pstr(sEtc, "@@", 4);
                    //sEDate = VB.Pstr(sEtc, "@@", 5);
                    //sMaxDiv = VB.Pstr(sEtc, "@@", 6);
                    //sDiv = VB.Pstr(sEtc, "@@", 7);
                    //sTerm = VB.Pstr(sEtc, "@@", 8);
                    //sNotify = VB.Pstr(sEtc, "@@", 9);

                    sPrnRemark = VB.Pstr(sEtc, "@@", 1);                    
                    sQty = VB.Pstr(sEtc, "@@", 2);
                    sMaxDiv = VB.Pstr(sEtc, "@@", 3);
                    sDiv = VB.Pstr(sEtc, "@@", 4);
                    sTerm = VB.Pstr(sEtc, "@@", 5);
                    sNotify = VB.Pstr(sEtc, "@@", 6);
                    sBDate = VB.Pstr(sEtc, "@@", 7);
                    sEDate = VB.Pstr(sEtc, "@@", 8);
                    sScale = VB.Pstr(sEtc, "@@", 9);
                    sOrdSeq = VB.Pstr(sEtc, "@@", 10);

                    if (sGbPrn == "Y")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNMARK].Text = "#";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "P";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNMARK].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";
                    }
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNREMARK].Text = sPrnRemark;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINSCALE].Text = VB.Left(sScale, 1);
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINUNIT].Text = sQty;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINSDATE].Text = sBDate;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINEDATE].Text = sEDate;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINMAX].Text = sMaxDiv;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNDOSCODE].Text = sDiv;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNTERM].Text = sTerm;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNNOTITIME].Text = sNotify;

                    if (sGbPrn == "Y")
                    {
                        if (sGbInsulin == "Y")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = fn_PlusName("950120");
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = "950120";
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "1";

                            //2019-06-28 유진호 INSULINE_3SCALE 시퀀스
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNORDSEQ].Text = sOrdSeq;
                        }
                        else
                        {

                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = fn_PlusName(fn_PRN_IV_2_SET("", "", sDiv));
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = fn_PRN_IV_2_SET("", dt.Rows[0]["DOSCODE"].ToString().Trim(), sDiv);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "1";
                        }
                    }
                    else
                    {
                        if (sGbInsulin == "Y")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = fn_PlusName(dt.Rows[0]["CDOSCODE"].ToString().Trim());
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = dt.Rows[0]["CDOSCODE"].ToString().Trim();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt.Rows[0]["GBDIV"].ToString().Trim();               
                        }
                    }
                }

                //if (clsOrdFunction.GstrCallForm == "PRNSLIP")
                //{
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = clsOrdFunction.GstrPrnDosCode;
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "1";
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = "1";
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "1";
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = fn_PlusName(clsOrdFunction.GstrPrnDosCode);
                //}

                //약제 통합메시지
                clsOrderEtc.ALL_OCS_SUGA_MESSAGE(clsDB.DbCon, "입원", dt.Rows[0]["SUCODE"].ToString().Trim(), SpdNm,
                        (int)BaseOrderInfo.IpdOrderCol.NAMEENG, SpdNm.ActiveSheet.ActiveRowIndex, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text);

                //골다공증메시지
                string sMsg = clsOrdFunction.READ_BONE_Result_Check(clsDB.DbCon, clsOrdFunction.Pat.PtNo, dt.Rows[0]["SUCODE"].ToString());

                if (sMsg != "" && sMsg != null)
                {

                    FrmMedDocMsgBox f = new FrmMedDocMsgBox(sMsg, "");
                    f.ShowDialog();
                    OF.fn_ClearMemory(f);
                }

                //부가세
                if (clsOrderEtc.READ_SUGA_VALUE_ADDED_TAX(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                {
                    clsOrdFunction.Gstr부가세 = "OK";
                }

                if (dt.Rows[0]["SUCODE"].ToString().Trim() == "X2081" || dt.Rows[0]["SUCODE"].ToString().Trim() == "X2082" ||
                dt.Rows[0]["SUCODE"].ToString().Trim() == "X2041" || dt.Rows[0]["SUCODE"].ToString().Trim() == "X2042")
                {
                    if (dt.Rows[0]["SUCODE"].ToString().Trim() == "X2041" || dt.Rows[0]["SUCODE"].ToString().Trim() == "X2042")
                    {
                        MessageBox.Show("FFP 는 아래의 경우에만 보험처방 가능합니다" + "\r\n" + "* PT  또는  PTT 가 정상수치의 30% 이하 저하된경우" + "\r\n"
                        + "* PT결과 INR 1.5 이상인 경우" + "\r\n" + "* Hypofibrinogenemia(100 이하)인경우" + "\r\n" + "* 8 pints 까지 인정됨" + "\r\n"
                        + "그외 처방해야 하는 경우 보험 안됩니다. 비보험 설명. 비보험 처방 해주세요" + "\r\n" + "(금액: 1PINT 당 5 만원)", "수가처방정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (dt.Rows[0]["SUCODE"].ToString().Trim() == "X2081" || dt.Rows[0]["SUCODE"].ToString().Trim() == "X2082")
                    {
                        MessageBox.Show("혈소판 농축액은 혈소판 수치가" + "\r\n" + "5만 이하의 경우 8  pints. " + "\r\n"
                        + "2만 이하의 경우 12 pints 까지 보험인정 됩니다." + "\r\n" + "(검사수치가 안되는 경우에는 보험 안됨.  비보험설명.비보험처방 해주세요)", "수가처방정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim().PadRight(10) + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                #endregion
            }            
            else if (argGBIO == "ER")
            {
                #region // ER
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();

                if (dt.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                {
                    strUnit = dt.Rows[0]["CORDERNAME"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = strUnit + " " + dt.Rows[0]["CORDERNAMES"].ToString();
                }
                else if (dt.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = dt.Rows[0]["CDISPHEADER"].ToString() + " " +
                    dt.Rows[0]["CORDERNAME"].ToString();
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString();
                }

                //금액적용시
                if (dt.Rows[0]["CGBBOTH"].ToString() == "1")
                {
                    strName = ComFunc.LeftH(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text, 30);
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = strName + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text;
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text = "1";

                if (clsPublic.Gstr산제Chk == "OK")  //병동/ER 처방은 무조건 OK
                {
                    if (clsPublic.Gstr파우더New_STS == "Y11")
                    {
                        //if (OF.READ_POWDER(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "Y")
                        if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                        }
                    }
                }

                if (clsPublic.Gstr산제Chk == "OK")  //병동/ER 처방은 무조건 OK
                {
                    if (clsPublic.Gstr파우더New_STS == "Y")
                    {
                        if (clsOrderEtc.CHK_POWDER_SUCODE_CHK(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                        {
                            if (clsOrdFunction.GnReadOrder < startRow)
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].Text = "True";
                            }
                        }
                    }
                }
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NGT].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBER].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SELF].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBREMARK].Text = "";

                if (clsPublic.GstrSugaFind == "PRN")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBREMARK].Text = "P";
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POTABLE].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = dt.Rows[0]["SUCODE"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.BUN].Text = dt.Rows[0]["BUN"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SLIPNO].Text = dt.Rows[0]["SLIPNO"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN].Text = VB.Left(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["SpecCode"].ToString(), 2), dt.Rows[0]["Bun"].ToString()), 7);
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SORT].Text = VB.Right(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["SPECCODE"].ToString(), 2), dt.Rows[0]["BUN"].ToString()).Trim(), 3);

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.QTY].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBBOTH].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REMARK].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text = dt.Rows[0]["CDISPRGB"].ToString();

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBBOTH1].Text = dt.Rows[0]["GBBOTH"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO1].Text = dt.Rows[0]["GBINFO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBQTY].Text = dt.Rows[0]["CGBQTY"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBDOSAGE].Text = dt.Rows[0]["CGBDOSAGE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NEXTCODE].Text = dt.Rows[0]["CNEXTCODE"].ToString();

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.CONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.BCONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString();

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBIMIV].Text = dt.Rows[0]["CGBIMIV"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ROWID].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SEQNO].Text = Convert.ToString(startRow);

                //선수납항목 체크
                if (dt.Rows[0]["SUGBN"].ToString() == "1")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUNSUNAP].Text = "S";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "(A)" + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUNSUNAP].Text = "";
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text.PadRight(10) +
                                                                                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;

                string cDosCode = dt.Rows[0]["SPECCODE"].ToString();

                if (dt.Rows[0]["GBINFO"].ToString() == "1")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dt.Rows[0]["GBINFO"].ToString();
                }
                else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "1")
                {
                    if (dt.Rows[0]["DOSNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBDOSAGE].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                        if (dt.Rows[0]["GBDIV"].ToString() == "0")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = "";
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = "";//용법을 바꾸면 따라서 총투량도 같이 바꾼다.
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNMARK].Text = "";
                        }
                        else
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = dt.Rows[0]["GBDIV"].ToString();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = dt.Rows[0]["GBDIV"].ToString();
                        }
                        //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = dt.Rows[0]["SPECCODE"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                    }
                }
                else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "2")
                {
                    if (dt.Rows[0]["SPECNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dt.Rows[0]["SPECNAME"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = dt.Rows[0]["SPECCODE"].ToString();
                    }
                }

                //추가 정보(방사선, 검사등) 2017-12-11 의미 없는 코딩.... SS1.Col = 11 에 Data 넣지 않음.
                //SS1.Col = 6
                //If Trim(SS1.Text) = "1" Then                    '추가 정보(방사선, 검사등)

                //    SS1.Col = 11

                //    If Trim(SS1.Text) <> "" Then

                //        GOrderFORM.SSOrder.Col = 4:    GOrderFORM.SSOrder.Text = SS1.Text
                //    End If
                //End If

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    SpdNm.ActiveSheet.Cells[startRow, 2, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(128, 0, 0);
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, 2, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                }
                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text = "80";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text = "";
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNMARK].Text = "#";
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDERSAYU].Text = dt.Rows[0]["SUCODE"].ToString() + ">>" + clsPublic.Gstr파우더_SuCode;                //소견 체크
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBOPINION].Text = dt.Rows[0]["OPINION_YN"].ToString();

                strSELECTOrderCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text.Trim();
                strSELECTOrderName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text.Trim();
                strSELECTDosName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text.Trim();
                strSELECTDosCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text.Trim();
                strSELECTSlipnos = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SLIPNO].Text.Trim();
                strSELECTBun = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.BUN].Text.Trim();
                strSELECTCBun = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.CBUN].Text.Trim();
                strSELECTDiv = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text.Trim();
                strSELECTSpecCode = dt.Rows[0]["SPECCODE"].ToString().Trim();

                if (dt.Rows[0]["BUN"].ToString() == "20")
                {
                    switch (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBDOSAGE].Text)
                    {
                        case "1":
                            clsOrdFunction.GstrLoadFlag = "OrdSlips";
                            FrmDosCode f = new FrmDosCode("Order", strSELECTDosCode, strSELECTBun, "", "O", strSELECTSpecCode, true);
                            f.ShowDialog();
                            OF.fn_ClearMemory(f);
                            clsOrdFunction.GstrLoadFlag = "";                            
                            break;
                        case "2":
                            FrmMedViewSpecimen ff = new FrmMedViewSpecimen(strSELECTOrderCode, strSELECTSlipnos, strSELECTDosCode, startRow);
                            ff.ShowDialog();
                            OF.fn_ClearMemory(ff);

                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = clsOrdFunction.GstrSpecNm;
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = clsOrdFunction.GstrSpecCd;
                            break;
                        default:
                            break;
                    }
                }

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    SpdNm.ActiveSheet.Cells[startRow, 1, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.Aqua;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text;
                }
                else
                {
                    if (dt.Rows[0]["CDISPRGB"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, 1, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                    }
                }

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text = "80";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text = "";
                }

                if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "★항혈전 " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].ForeColor = Color.FromArgb(255, 0, 255);
                }

                if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "<!> " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                }

                if (clsOrderEtc.READ_SUGA_MR_EXPENSIVE_MEDICINE(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim(), clsOrdFunction.Pat.DeptCode) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "[재고X] " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNMARK].Text = "#";


                if (sEtc.Trim() != "")
                {
                    string sPrnRemark = "";
                    string sScale = "";
                    string sQty = "";
                    string sBDate = "";
                    string sEDate = "";
                    string sMaxDiv = "";
                    string sDiv = "";
                    string sTerm = "";
                    string sNotify = "";
                    string sOrdSeq = "";

                    //sPrnRemark = VB.Pstr(sEtc, "@@", 1);                    
                    //sQty = VB.Pstr(sEtc, "@@", 2);
                    //sBDate = VB.Pstr(sEtc, "@@", 3);
                    //sEDate = VB.Pstr(sEtc, "@@", 4);
                    //sMaxDiv = VB.Pstr(sEtc, "@@", 5);
                    //sDiv = VB.Pstr(sEtc, "@@", 6);
                    //sTerm = VB.Pstr(sEtc, "@@", 7);
                    //sNotify = VB.Pstr(sEtc, "@@", 8);
                    //sScale = VB.Pstr(sEtc, "@@", 9);
                    //sOrdSeq = VB.Pstr(sEtc, "@@", 10);

                    sPrnRemark = VB.Pstr(sEtc, "@@", 1);
                    sQty = VB.Pstr(sEtc, "@@", 2);
                    sMaxDiv = VB.Pstr(sEtc, "@@", 3);
                    sDiv = VB.Pstr(sEtc, "@@", 4);
                    sTerm = VB.Pstr(sEtc, "@@", 5);
                    sNotify = VB.Pstr(sEtc, "@@", 6);
                    sBDate = VB.Pstr(sEtc, "@@", 7);
                    sEDate = VB.Pstr(sEtc, "@@", 8);
                    sScale = VB.Pstr(sEtc, "@@", 9);
                    sOrdSeq = VB.Pstr(sEtc, "@@", 10);

                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNREMARK].Text = sPrnRemark;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.INSULINSCALE].Text = VB.Left(sScale, 1);
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.INSULINUNIT].Text = sQty;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.INSULINSDATE].Text = sBDate;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.INSULINEDATE].Text = sEDate;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.INSULINMAX].Text = sMaxDiv;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNDOSCODE].Text = sDiv;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNTERM].Text = sTerm;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNNOTITIME].Text = sNotify;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNORDSEQ].Text = sOrdSeq;
                }

                switch (dt.Rows[0]["CGBIMIV"].ToString())
                {
                    case "4":
                    case "5":
                    case "6":
                        try
                        {
                            SQL = "";
                            SQL += " SELECT * FROM KOSMOS_OCS.ENDO_REMARK                                   \r";
                            SQL += "  WHERE Ptno  = '" + clsOrdFunction.Pat.PtNo + "'                       \r";
                            SQL += "    AND JDate = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')   \r";
                            SQL += "    AND OrderCode = '" + dt.Rows[0]["OrderCode"].ToString().Trim() + "' \r";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                clsOrdFunction.GstrResultChk = "1";
                            }
                            dt1.Dispose();
                            dt1 = null;
                        }
                        catch (Exception ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        }
                        break;
                    case "8":
                        try
                        {
                            SQL = "";
                            SQL += " SELECT ROWID                                                               \r";
                            SQL += "   FROM KOSMOS_OCS.EXAM_ANATMST                                             \r"; //Cytology
                            SQL += "  WHERE Ptno      = '" + clsOrdFunction.Pat.PtNo + "'                       \r";
                            SQL += "    AND BDate     = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')   \r";
                            SQL += "    AND OrderCode = '" + dt.Rows[0]["OrderCode"].ToString() + "'            \r";
                            SQL += "    AND GbIO      = 'I'                                                     \r";
                            SQL += "    AND DeptCode  = '" + clsPublic.GstrDeptCode.Trim() + "'                 \r";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                clsOrdFunction.GstrResultChk = "1";
                            }
                            dt1.Dispose();
                            dt1 = null;
                        }
                        catch (Exception ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        }
                        break;
                    default:
                        break;
                }
                if (clsOrdFunction.GstrResultChk == "1")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBOPINION].Text = "True";
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBOPINION].Text = "False";
                }

                //약제 통합메시지
                clsOrderEtc.ALL_OCS_SUGA_MESSAGE(clsDB.DbCon, "입원", dt.Rows[0]["SUCODE"].ToString().Trim(), SpdNm,
                        (int)BaseOrderInfo.ErOrderCol.NAMEENG, SpdNm.ActiveSheet.ActiveRowIndex, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text);

                //골다공증메시지
                string sMsg = clsOrdFunction.READ_BONE_Result_Check(clsDB.DbCon, clsOrdFunction.Pat.PtNo, dt.Rows[0]["SUCODE"].ToString());

                if (sMsg != "" && sMsg != null)
                {

                    FrmMedDocMsgBox f = new FrmMedDocMsgBox(sMsg, "");
                    f.ShowDialog();
                    OF.fn_ClearMemory(f);
                }

                //부가세
                if (clsOrderEtc.READ_SUGA_VALUE_ADDED_TAX(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                {
                    clsOrdFunction.Gstr부가세 = "OK";
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim().PadRight(10) + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                #endregion
            }
        }

        public void fn_PowderOrder_Replace_Disp(DataTable dt, string argGBIO, FarPoint.Win.Spread.FpSpread SpdNm, int startRow, double nQty)
        {
            string strUnit = "";
            string strName = "";

            string strMsg = "";

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();

            if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = dt.Rows[0]["REMARK"].ToString().Trim();
            }
            else
            {
                if (dt.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                {
                    strUnit = dt.Rows[0]["CORDERNAME"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = strUnit + " " + dt.Rows[0]["CORDERNAMES"].ToString();
                }
                else if (dt.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["CDISPHEADER"].ToString() + " " +
                    dt.Rows[0]["CORDERNAME"].ToString();
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString();
                }
            }

            //금액적용시
            if (dt.Rows[0]["CGBBOTH"].ToString() == "1")
            {
                strName = ComFunc.LeftH(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text, 30);
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = strName + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text;
            }

            if (clsOrdFunction.GstrCDSSYN == "Y")
            {
                if (Convert.ToInt32(dt.Rows[0]["BUN"].ToString().Trim()) >= 11 && Convert.ToInt32(dt.Rows[0]["BUN"].ToString().Trim()) <= 20 ||
                    Convert.ToInt32(dt.Rows[0]["BUN"].ToString().Trim()) == 23)
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
                    if (dt.Rows[0]["CNEXTCODE"].ToString() == "0")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Locked = true;
                    }
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NEXTCODE].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
                }
            }

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = (Convert.ToString(nQty) == "0" ? "1" : Convert.ToString(nQty));
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAL].Text = "1";

            if (clsPublic.Gstr산제Chk == "OK")  //병동/ER 처방은 무조건 OK
            {
                //if (OF.READ_POWDER(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "Y")
                if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                }
            }

            if (clsPublic.Gstr산제Chk == "OK")  //병동/ER 처방은 무조건 OK
            {
                if (clsPublic.Gstr파우더New_STS == "Y")
                {
                    if (clsOrderEtc.CHK_POWDER_SUCODE_CHK(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                    {
                        if (clsOrdFunction.GnReadOrder < startRow)
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].Text = "True";
                        }
                    }
                }
            }
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NGT].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ER].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "";
            //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";

            if (clsPublic.GstrSugaFind == "PRN")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "P";
            }

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = dt.Rows[0]["GBPRN"].ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNREMARK].Text = dt.Rows[0]["PRN_REMARK"].ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINSCALE].Text = dt.Rows[0]["PRN_INS_GBN"].ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINUNIT].Text = dt.Rows[0]["PRN_INS_UNIT"].ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDERSAYU].Text = dt.Rows[0]["POWDER_SAYU"].ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINMAX].Text = dt.Rows[0]["PRN_INS_MAX"].ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNDOSCODE].Text = dt.Rows[0]["PRN_DOSCODE"].ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNTERM].Text = dt.Rows[0]["PRN_TERM"].ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNNOTITIME].Text = dt.Rows[0]["PRN_NOTIFY"].ToString().Trim();

            if (dt.Rows[0]["PRN_REMARK"].ToString().Trim() != "")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNMARK].Text = "#";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNREMARK].Text = dt.Rows[0]["PRN_REMARK"].ToString().Trim();
            }

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POTABLE].Text = "";

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text = dt.Rows[0]["SUCODE"].ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BUN].Text = dt.Rows[0]["BUN"].ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text = dt.Rows[0]["SLIPNO"].ToString().Trim();

            //(2018.04.27) 확인필요
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = clsPublic.Gstr파우더_SuCode;
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = VB.Left(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["DOSCODE"].ToString(), 2), dt.Rows[0]["Bun"].ToString()), 7);
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPGUBUN].Text = VB.Right(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["DOSCODE"].ToString(), 2), dt.Rows[0]["BUN"].ToString()).Trim(), 3);

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = nQty.ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBBOTH].Text = "";

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REMARK].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = dt.Rows[0]["CDISPRGB"].ToString();

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CGBBOTH].Text = dt.Rows[0]["GBBOTH"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CGBINFO].Text = dt.Rows[0]["GBINFO"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBQTY].Text = dt.Rows[0]["CGBQTY"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBDOSAGE].Text = dt.Rows[0]["CGBDOSAGE"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NEXTCODE].Text = dt.Rows[0]["CNEXTCODE"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBIMIV].Text = dt.Rows[0]["CGBIMIV"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ROWID].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SEQNO].Text = Convert.ToString(startRow);

            //선수납항목 체크
            if (dt.Rows[0]["SUGBN"].ToString() == "1")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUNSUNAP].Text = "S";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "(A)" + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
            }

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text;

            //if (clsOrdFunction.GSelfMedOrd != "Y")
            //{
            //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDERSAYUMARK].Text = "#";
            //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDERSAYU].Text = dt.Rows[0]["SUCODE"].ToString() + ">>" + clsPublic.Gstr파우더_SuCode;
            //}
            //소견 체크
            //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.RESULT].Text = dt.Rows[0]["OPINION_YN"].ToString();

            string cDosCode = dt.Rows[0]["DOSCODE"].ToString().Trim();

            if (dt.Rows[0]["BUN"].ToString() == "11" || dt.Rows[0]["BUN"].ToString() == "12")
            {
                cDosCode = "010501";
                if (dt.Rows[0]["SUCODE"].ToString() == "NIG06")
                {
                    cDosCode = "490301";
                }
            }
            else if (dt.Rows[0]["BUN"].ToString() == "20")
            {
                switch (dt.Rows[0]["SUCODE"].ToString())
                {
                    case "NSB":
                        cDosCode = "930120";
                        break;
                    case "PACETA":
                        cDosCode = "930120";
                        break;
                    case "MIN2":
                        cDosCode = "910120";
                        break;
                    default:
                        cDosCode = "920120";
                        break;
                }
            }

            if (dt.Rows[0]["GBINFO"].ToString().Trim() != "")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["GBINFO"].ToString().Trim();
            }
            else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "1")
            {
                if (dt.Rows[0]["DOSNAME"].ToString().Trim() == "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = "";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString().Trim();
                    if (dt.Rows[0]["GBDIV"].ToString() == "0")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";//용법을 바꾸면 따라서 총투량도 같이 바꾼다.
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";
                    }
                    else
                    {
                        if (dt.Rows[0]["CONTENTS"].ToString().Trim() == "" || dt.Rows[0]["CONTENTS"].ToString().Trim() == "0")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text = "";
                        }
                        else
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text = dt.Rows[0]["CONTENTS"].ToString().Trim();
                        }
                        if (dt.Rows[0]["BCONTENTS"].ToString().Trim() == "" || dt.Rows[0]["BCONTENTS"].ToString().Trim() == "0")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BCONTENTS].Text = "";
                        }
                        else
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BCONTENTS].Text = dt.Rows[0]["BCONTENTS"].ToString().Trim();
                        }

                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString().Trim();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = (Convert.ToString(nQty) == "0" ? "1" : Convert.ToString(nQty));
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = (Convert.ToString(nQty) == "0" ? "1" : Convert.ToString(nQty));
                    }
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString().Trim();
                }
            }
            else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "2")
            {
                if (dt.Rows[0]["SPECNAME"].ToString().Trim() == "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = "";
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["SPECNAME"].ToString().Trim();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString().Trim();
                }
            }

            if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
            {
                SpdNm.ActiveSheet.Cells[startRow, 2, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(128, 0, 0);
            }
            else
            {
                if (dt.Rows[0]["CDISPRGB"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, 2, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                }
            }

            //if (VB.Left(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text.Trim(), 4) == "★항혈전")
            //{
            //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG, startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].ForeColor = Color.FromArgb(255, 0, 255);
            //}

            if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
            {
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "80";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "";
            }

            if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "★항혈전 " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].ForeColor = Color.FromArgb(255, 0, 255);
            }

            if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "<!> " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
            }

            if (clsOrderEtc.READ_SUGA_MR_EXPENSIVE_MEDICINE(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim(), clsOrdFunction.Pat.DeptCode) == "OK")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "[재고X] " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
            }

            clsOrdFunction.GstrSimCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text;
            clsOrdFunction.GstrSimFlag = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SIMFLAG].Text;
            clsOrdFunction.GstrSimYN = clsOrdFunction.SimSaGiJun_Check(clsDB.DbCon, clsOrdFunction.GstrSimFlag, clsOrdFunction.GstrSimCode, argGBIO);
            if (clsOrdFunction.GstrSimYN == "Y")
            {
                frmPmpaJSimsaGijun f = new frmPmpaJSimsaGijun(clsOrdFunction.GstrSimCode);
                f.ShowDialog();
                OF.fn_ClearMemory(f);
            }
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SIMFLAG].Text = clsOrdFunction.GstrSimYN;

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.MAYAK].Text = OF.READ_MAYAK(clsDB.DbCon, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim());

            if (clsPublic.GstrSugaFind == "PRN")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNMARK].Text = "#";
            }

            //약제 통합메시지
            clsOrderEtc.ALL_OCS_SUGA_MESSAGE(clsDB.DbCon, "입원", dt.Rows[0]["SUCODE"].ToString().Trim(), SpdNm,
                    (int)BaseOrderInfo.IpdOrderCol.NAMEENG, SpdNm.ActiveSheet.ActiveRowIndex, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text);

            //골다공증메시지
            string sMsg = clsOrdFunction.READ_BONE_Result_Check(clsDB.DbCon, clsOrdFunction.Pat.PtNo, dt.Rows[0]["SUCODE"].ToString());

            if (sMsg != "" && sMsg != null)
            {
                FrmMedDocMsgBox f = new FrmMedDocMsgBox(sMsg, "");
                f.ShowDialog();
                OF.fn_ClearMemory(f);
            }

            //부가세
            if (clsOrderEtc.READ_SUGA_VALUE_ADDED_TAX(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
            {
                clsOrdFunction.Gstr부가세 = "OK";
            }

            if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2081" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2082" ||
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2041" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2042")
            {
                if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2041" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2042")
                {
                    strMsg = "";
                    strMsg += "FFP 는 아래의 경우에만 보험처방 가능합니다" + "\r\n";
                    strMsg += "* PT  또는  PTT 가 정상수치의 30% 이하 저하된경우" + "\r\n";
                    strMsg += "* PT결과 INR 1.5 이상인 경우" + "\r\n";
                    strMsg += "* Hypofibrinogenemia(100 이하)인경우" + "\r\n";
                    strMsg += "* 8 pints 까지 인정됨" + "\r\n";
                    strMsg += "그외 처방해야 하는 경우 보험 안됩니다. 비보험 설명. 비보험 처방 해주세요" + "\r\n";
                    strMsg += "(금액: 1PINT 당 5 만원)";
                    MessageBox.Show(strMsg, "수가처방정보", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2081" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2082")
                {
                    strMsg = "";
                    strMsg += "혈소판 농축액은 혈소판 수치가" + "\r\n";
                    strMsg += "5만 이하의 경우 8  pints." + "\r\n";
                    strMsg += "2만 이하의 경우 12 pints 까지 보험인정 됩니다." + "\r\n";
                    strMsg += "(검사수치가 안되는 경우에는 보험 안됨.  비보험설명.비보험처방 해주세요)";
                    MessageBox.Show(strMsg, "수가처방정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (clsOrdFunction.GSelfMedOrd == "Y")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = clsOrdFunction.GSelfMedDosCode;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = clsOrdFunction.GSelfMedQty;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = clsOrdFunction.GSelfMedQty;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = clsOrdFunction.GSelfMedDiv;
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = clsOrdFunction.GSelfMedPlusName;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = fn_PlusName(clsOrdFunction.GSelfMedDosCode);
            }

            if (VB.Left(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text.Trim(), 1) != "A")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text.PadRight(10) +
                                                                                                 SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
            }
        }

        public void fn_Order_Disp(DataTable dt, string argGBIO, FarPoint.Win.Spread.FpSpread SpdNm, int startRow, double nQty, string argPRN = "")
        {
            FarPoint.Win.Spread.CellType.CheckBoxCellType chk = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

            string strUnit = "";  
            string strName = "";

            string strMsg = "";
            //DataTable dt10 = null;

            if (argGBIO == "OPD")   //외래
            {
                #region //OPD

                //2020-08-20 안정수 추가, 전산의뢰 <2020-2148> OCS 처방전달 메세지 추가요청건 
                //2020-08-28 안정수 추가, 전산의뢰 <2020-2187> 선별급여약제 관련, 9/1이전 처방 가져올시 팝업창 생성 
                if (string.Compare(clsPublic.GstrSysDate, "2020-09-01") >= 0)
                {
                    clsMedFunction.SimsaMsg_Check(clsDB.DbCon, dt.Rows[0]["ORDERCODE"].ToString().Trim(), clsOrdFunction.Pat.Bi);

                    //2020-08-31 안정수, 심사팀장님 요청으로 지시있을때까지 보류 
                    //if (dt.Rows[0]["BUN"].ToString().Trim() == "11" 
                    //        && string.Compare(VB.Left(dt.Rows[0]["BDATE"].ToString().Trim(), 10), "2020-09-01") < 0)
                    //{
                    //    SQL = "";
                    //    SQL += ComNum.VBLF + "SELECT";
                    //    SQL += ComNum.VBLF + "  CODE    ";
                    //    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                    //    SQL += ComNum.VBLF + "WHERE 1=1";
                    //    SQL += ComNum.VBLF + "  AND GUBUN = 'C#_OCS_선별급여약제목록'";
                    //    SqlErr = clsDB.GetDataTableREx(ref dt10, SQL, clsDB.DbCon);

                    //    if(dt10.Rows.Count > 0)
                    //    {
                    //        for(int i = 0; i < dt10.Rows.Count; i++)
                    //        {
                    //            if(dt10.Rows[i]["CODE"].ToString().Trim() == dt.Rows[0]["SUCODE"].ToString().Trim())
                    //            {
                    //                strMsg = "";
                    //                strMsg += "[콜린제제 - 2020.9.1 선별급여 변경 협조안내]" + ComNum.VBLF;
                    //                strMsg += "9월 1일 이전 처방 약제는 프로그램 적용 위해 새로 입력 부탁드립니다. ";

                    //                FrmMedDocMsgBox f = new FrmMedDocMsgBox(strMsg, "");
                    //                f.ShowDialog();
                    //                OF.fn_ClearMemory(f);

                    //                break;
                    //            }
                    //        }
                    //    }
                    //}                        
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERCODE].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = dt.Rows[0]["REMARK"].ToString().Trim();
                }
                else
                {
                    if (dt.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                    {
                        strUnit = dt.Rows[0]["CORDERNAME"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = strUnit + " " + dt.Rows[0]["CORDERNAMES"].ToString();
                    }
                    else if (dt.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = dt.Rows[0]["CDISPHEADER"].ToString() + " " +
                        dt.Rows[0]["CORDERNAME"].ToString();
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString();
                    }

                    if (dt.Rows[0]["CGBBOTH"].ToString() == "1")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = ComFunc.LeftH(dt.Rows[0]["CORDERNAME"].ToString(), 30) + " " + dt.Rows[0]["GBINFO"].ToString();
                    }
                }

                if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = "★항혈전 " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].ForeColor = Color.FromArgb(255, 0, 255);
                }

                if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = "<!> " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                }

                if (clsOrderEtc.READ_SUGA_MR_EXPENSIVE_MEDICINE(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim(), clsOrdFunction.Pat.DeptCode) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = "[재고X] " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                }

                //약제 통합메시지
                clsOrderEtc.ALL_OCS_SUGA_MESSAGE(clsDB.DbCon, "외래", dt.Rows[0]["SUCODE"].ToString().Trim(), SpdNm,
                    (int)BaseOrderInfo.OpdOrderCol.NAMEENG, SpdNm.ActiveSheet.ActiveRowIndex, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text);

                //골다공증메시지
                string sMsg = clsOrdFunction.READ_BONE_Result_Check(clsDB.DbCon, clsOrdFunction.Pat.PtNo, dt.Rows[0]["SUCODE"].ToString());

                if (sMsg != "" && sMsg != null)
                {
                    FrmMedDocMsgBox f = new FrmMedDocMsgBox(sMsg, "");
                    f.ShowDialog();
                    OF.fn_ClearMemory(f);
                }

                //부가세
                if (clsOrderEtc.READ_SUGA_VALUE_ADDED_TAX(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                {
                    clsOrdFunction.Gstr부가세 = "OK";
                }

                if (dt.Rows[0]["GBINFO"].ToString() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = dt.Rows[0]["GBINFO"].ToString();
                }
                else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "1")
                {
                    if (dt.Rows[0]["DOSNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                    }
                }
                else
                {
                    if (dt.Rows[0]["SPECNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DOSCODE].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = dt.Rows[0]["SPECNAME"].ToString();
                    }
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.CONTENTS].Text = dt.Rows[0]["REALQTY"].ToString().Trim();
                
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.QTY].Text = dt.Rows[0]["QTY"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DIV].Text = dt.Rows[0]["GBDIV"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAL].Text = (dt.Rows[0]["NAL"].ToString() == "" ? "1" : dt.Rows[0]["NAL"].ToString());
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBER].Text = dt.Rows[0]["GBER"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].Text = dt.Rows[0]["GBSELF"].ToString();

                if (dt.Rows[0]["SUGBF"].ToString().Trim() == "1" || dt.Rows[0]["SUGBF"].ToString() == "2")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].BackColor = Color.FromArgb(255, 210, 234);
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SUGBF].Text = dt.Rows[0]["SUGBF"].ToString();
                }

                //2020-11-23 안정수 추가
                if(dt.Rows[0]["SUCODE"].ToString().Trim() == "TASNA"
                    && SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].Text == " ")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].Text = "2";
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.REMARKGUBUN].Text = (dt.Rows[0]["REMARK"].ToString().Trim() == "" ? "" : "#");
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBRESERVED].Text = dt.Rows[0]["RES"].ToString() == "1" ? "True" : "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBMULTI].Text = dt.Rows[0]["MULTI"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SUCODE].Text = dt.Rows[0]["SUCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.BUN].Text = dt.Rows[0]["BUN"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.CBUN].Text = dt.Rows[0]["CBUN"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SLIPNO].Text = dt.Rows[0]["SLIPNO"].ToString();
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.QTY].Text = dt.Rows[0]["REALQTY"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBBOTH].Text = dt.Rows[0]["GBBOTH"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBINFO].Text = dt.Rows[0]["GBINFO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.REMARK].Text = dt.Rows[0]["REMARK"].ToString();

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DISPRGB].Text = dt.Rows[0]["CDISPRGB"].ToString();
                if (dt.Rows[0]["CDISPRGB"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, 1, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor
                        = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                }

                if (VB.Left(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text.Trim(), 4) == "★항혈전")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG, startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].ForeColor = Color.FromArgb(255, 0, 255);
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBBOTH1].Text = dt.Rows[0]["CGBBOTH"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBINFO1].Text = dt.Rows[0]["CGBINFO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBQTY].Text = dt.Rows[0]["CGBQTY"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBDOSAGE].Text = dt.Rows[0]["CGBDOSAGE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NEXTCODE].Text = dt.Rows[0]["CNEXTCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBIMIV].Text = dt.Rows[0]["CGBIMIV"].ToString();

                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERNO].Text = dt.Rows[0]["ORDERNO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERNO].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSUNAP].Text = dt.Rows[0]["GBSUNAP"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DRCODE].Text = dt.Rows[0]["DRCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.MULTIREMARK].Text = dt.Rows[0]["MULTIREMARK"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DUR].Text = dt.Rows[0]["DUR"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.RESV].Text = dt.Rows[0]["RESV"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBTAX].Text = dt.Rows[0]["GBTAX"].ToString() == "1" ? "True" : "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SCODEREMARK].Text = dt.Rows[0]["SCODEREMARK"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSPCNO].Text = dt.Rows[0]["GBSPC_NO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SCODEREASON].Text = dt.Rows[0]["SCODESAYU"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBCOPY].Text = dt.Rows[0]["GBCOPY"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBBUBUNSUNAP].Text = dt.Rows[0]["GBSUNAP"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DOSCODE1].Text = dt.Rows[0]["DOSCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.REALQTY].Text = dt.Rows[0]["REALQTY"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBDIV].Text = dt.Rows[0]["GBDIV"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBNAL].Text = dt.Rows[0]["NAL"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF1].Text = dt.Rows[0]["GBSELF"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBFM].Text = dt.Rows[0]["GBFM"].ToString() == "1" ? "True" : "";
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SABUN].Text = dt.Rows[0]["SABUN"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SABUN].Text = clsType.User.Sabun.Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.OCSDRUG].Text = dt.Rows[0]["OCSDRUG"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBAUTOSEND].Text = dt.Rows[0]["GBAUTOSEND"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.ASA].Text = dt.Rows[0]["ASA"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PCHASU].Text = dt.Rows[0]["PCHASU"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SUBUL_WARD].Text = dt.Rows[0]["SUBUL_WARD"].ToString();

                if (string.Compare(clsPublic.GstrSysDate, "2019-01-01") >= 0)
                {
                    //if (OF.READ_POWDER(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "Y")
                    //{
                    //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.POWDER].CellType = chk;
                    //}
                    //else
                    //{
                        if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.POWDER].CellType = chk;
                        }
                    //}
                }

                if (clsPublic.Gstr산제Chk == "OK")
                {
                    if (clsPublic.Gstr파우더New_STS == "Y")
                    {
                        clsPublic.Gstr파우더Gubun = "";
                        if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                        {
                            if (clsOrdFunction.GnReadOrder < SpdNm.ActiveSheet.NonEmptyRowCount)
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.POWDER].CellType = chk;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.POWDER].Text = "True";
                            }
                        }
                    }
                }

                //2021-01-07 추가 
                if (clsType.User.IdNumber == "53775")
                {
                    if (dt.Rows[0]["CONTENTS"].ToString() != "0")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NEWCONTENTS].Text = dt.Rows[0]["CONTENTS"].ToString();
                    }
                    if (dt.Rows[0]["BCONTENTS"].ToString() != "0")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.BCONTENTS].Text = dt.Rows[0]["BCONTENTS"].ToString();
                    }
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NGT].Text = dt.Rows[0]["GBGROUP"].ToString();

                    //2021-01-11 추가
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.TUYEOPOINT].Text = dt.Rows[0]["TUYEOPOINT"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.TUYEOTIME].Text = dt.Rows[0]["TUYEOTIME"].ToString().Replace("min", "").Replace("hr", "");

                    if (dt.Rows[0]["TUYEOTIME"].ToString().Contains("hr"))
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.TIMEGUBUN].Text = "hr";
                    }
                    else if (dt.Rows[0]["TUYEOTIME"].ToString().Contains("min"))
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.TIMEGUBUN].Text = "min";
                    }
                }
                #endregion //OPD
            }
            else if (argGBIO == "IPD")  //입원
            {
                #region //IPD
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = dt.Rows[0]["REMARK"].ToString().Trim();
                }
                else
                {
                    if (dt.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                    {
                        strUnit = dt.Rows[0]["CORDERNAME"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = strUnit + " " + dt.Rows[0]["CORDERNAMES"].ToString();
                    }
                    else if (dt.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["CDISPHEADER"].ToString() + " " +
                        dt.Rows[0]["CORDERNAME"].ToString();
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString();
                    }
                }

                //금액적용시
                if (dt.Rows[0]["CGBBOTH"].ToString() == "1")
                {
                    strName = ComFunc.LeftH(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text, 30);
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = strName + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text;
                }

                if (clsOrdFunction.GstrCDSSYN == "Y")
                {
                    if (Convert.ToInt32(dt.Rows[0]["BUN"].ToString().Trim()) >= 11 && Convert.ToInt32(dt.Rows[0]["BUN"].ToString().Trim()) <= 20 ||
                        Convert.ToInt32(dt.Rows[0]["BUN"].ToString().Trim()) == 23)
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
                        if (dt.Rows[0]["CNEXTCODE"].ToString() == "0")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Locked = true;
                        }
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NEXTCODE].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
                    }
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = (Convert.ToString(nQty) == "0" ? "1" : Convert.ToString(nQty));
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt.Rows[0]["GBDIV"].ToString().Trim(); ;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAL].Text = "1";

                if (clsPublic.Gstr산제Chk == "OK")  //병동/ER 처방은 무조건 OK
                {
                    //if (OF.READ_POWDER(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "Y")
                    if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    }
                }

                if (clsPublic.Gstr산제Chk == "OK")  //병동/ER 처방은 무조건 OK
                {
                    if (clsPublic.Gstr파우더New_STS == "Y")
                    {
                        if (clsOrderEtc.CHK_POWDER_SUCODE_CHK(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                        {
                            if (clsOrdFunction.GnReadOrder < startRow)
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].Text = "True";
                            }
                        }
                    }
                }
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NGT].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ER].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "";
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";

                if (clsPublic.GstrSugaFind == "PRN")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "P";
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = dt.Rows[0]["GBPRN"].ToString().Trim();

                if (dt.Rows[0]["REMARK"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REMARK].Text = dt.Rows[0]["REMARK"].ToString();
                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text.Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "#";
                    }
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNREMARK].Text = dt.Rows[0]["PRN_REMARK"].ToString().Trim(); 
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINSCALE].Text = dt.Rows[0]["PRN_INS_GBN"].ToString().Trim(); 
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINUNIT].Text = dt.Rows[0]["PRN_INS_UNIT"].ToString().Trim(); 
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDERSAYU].Text = dt.Rows[0]["POWDER_SAYU"].ToString().Trim(); 
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINMAX].Text = dt.Rows[0]["PRN_INS_MAX"].ToString().Trim(); 
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNDOSCODE].Text = dt.Rows[0]["PRN_DOSCODE"].ToString().Trim(); 
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNTERM].Text = dt.Rows[0]["PRN_TERM"].ToString().Trim(); 
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNNOTITIME].Text = dt.Rows[0]["PRN_NOTIFY"].ToString().Trim();

                if (dt.Rows[0]["PRN_REMARK"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNMARK].Text = "#";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNREMARK].Text = dt.Rows[0]["PRN_REMARK"].ToString().Trim();
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POTABLE].Text = dt.Rows[0]["GBPORT"].ToString().Trim();

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text = dt.Rows[0]["SUCODE"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BUN].Text = dt.Rows[0]["BUN"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text = dt.Rows[0]["SLIPNO"].ToString().Trim();

                //(2018.04.27) 확인필요
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = clsPublic.Gstr파우더_SuCode;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = VB.Left(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["DOSCODE"].ToString(), 2), dt.Rows[0]["Bun"].ToString()), 7);
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPGUBUN].Text = VB.Right(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["DOSCODE"].ToString(), 2), dt.Rows[0]["BUN"].ToString()).Trim(), 3);

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBBOTH].Text = "";

                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text = "";
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REMARK].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = dt.Rows[0]["CDISPRGB"].ToString();

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CGBBOTH].Text = dt.Rows[0]["GBBOTH"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CGBINFO].Text = dt.Rows[0]["GBINFO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBQTY].Text = dt.Rows[0]["CGBQTY"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBDOSAGE].Text = dt.Rows[0]["CGBDOSAGE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NEXTCODE].Text = dt.Rows[0]["CNEXTCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBIMIV].Text = dt.Rows[0]["CGBIMIV"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ROWID].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SEQNO].Text = Convert.ToString(startRow);

                //선수납항목 체크
                if (dt.Rows[0]["SUGBN"].ToString() == "1")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUNSUNAP].Text = "S";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "(A)" + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text;

                //if (clsOrdFunction.GSelfMedOrd != "Y")
                //{
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDERSAYUMARK].Text = "#";
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDERSAYU].Text = dt.Rows[0]["SUCODE"].ToString() + ">>" + clsPublic.Gstr파우더_SuCode;
                //}
                //소견 체크
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.RESULT].Text = dt.Rows[0]["OPINION_YN"].ToString();

                string cDosCode = dt.Rows[0]["DOSCODE"].ToString().Trim();

                if (dt.Rows[0]["BUN"].ToString() == "11" || dt.Rows[0]["BUN"].ToString() == "12")
                {
                    cDosCode = "010501";
                    if (dt.Rows[0]["SUCODE"].ToString() == "NIG06")
                    {
                        cDosCode = "490301";
                    }
                }
                else if (dt.Rows[0]["BUN"].ToString() == "20")
                {
                    switch (dt.Rows[0]["SUCODE"].ToString())
                    {
                        case "NSB":
                            cDosCode = "930120";
                            break;
                        case "PACETA":
                            cDosCode = "930120";
                            break;
                        case "MIN2":
                            cDosCode = "910120";
                            break;
                        default:
                            cDosCode = "920120";
                            break;
                    }
                }

                if (dt.Rows[0]["GBINFO"].ToString().Trim() != "")
                {
                    if (dt.Rows[0]["SPECNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["GBINFO"].ToString().Trim();
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["SPECNAME"].ToString().Trim() + "/" + dt.Rows[0]["GBINFO"].ToString().Trim();
                    }
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text = dt.Rows[0]["GBINFO"].ToString().Trim();
                }
                else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "1")
                {
                    if (dt.Rows[0]["DOSNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString().Trim();
                        if (dt.Rows[0]["GBDIV"].ToString() == "0")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";//용법을 바꾸면 따라서 총투량도 같이 바꾼다.
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";
                        }
                        else
                        {
                            if (dt.Rows[0]["CONTENTS"].ToString().Trim() == "" || dt.Rows[0]["CONTENTS"].ToString().Trim() == "0")
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text = "";
                            }
                            else
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text = dt.Rows[0]["CONTENTS"].ToString().Trim();
                            }
                            if (dt.Rows[0]["BCONTENTS"].ToString().Trim() == "" || dt.Rows[0]["BCONTENTS"].ToString().Trim() == "0")
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BCONTENTS].Text = "";
                            }
                            else
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BCONTENTS].Text = dt.Rows[0]["BCONTENTS"].ToString().Trim();
                            }

                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt.Rows[0]["GBDIV"].ToString().Trim();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = dt.Rows[0]["REALQTY"].ToString().Trim();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = dt.Rows[0]["QTY"].ToString().Trim();
                        }
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString().Trim();
                    }
                }
                else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "2")
                {
                    if (dt.Rows[0]["SPECNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["SPECNAME"].ToString().Trim();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString().Trim();
                    }
                }

                ////2019-08-27 전산업무의뢰서 2019-08-09
                //if ((int)VB.Val(dt.Rows[0]["BUN"].ToString().Trim()) == 20)
                //{
                //    if (dt.Rows[0]["cNEXTCODE"].ToString().Trim() != "")
                //    {
                //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTSUNIT].Text = dt.Rows[0]["cDispHeader"].ToString().Trim();
                //    }
                //}

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    SpdNm.ActiveSheet.Cells[startRow, 2, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(128, 0, 0);
                }
                else
                {
                    if (dt.Rows[0]["CDISPRGB"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, 2, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                    }
                }

                //if (VB.Left(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text.Trim(), 4) == "★항혈전")
                //{
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG, startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].ForeColor = Color.FromArgb(255, 0, 255);
                //}

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "80";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "";
                }

                if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "★항혈전 " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].ForeColor = Color.FromArgb(255, 0, 255);
                }

                if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "<!> " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                }

                if (clsOrderEtc.READ_SUGA_MR_EXPENSIVE_MEDICINE(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim(), clsOrdFunction.Pat.DeptCode) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "[재고X] " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                }

                clsOrdFunction.GstrSimCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text;
                clsOrdFunction.GstrSimFlag = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SIMFLAG].Text;
                clsOrdFunction.GstrSimYN = clsOrdFunction.SimSaGiJun_Check(clsDB.DbCon, clsOrdFunction.GstrSimFlag, clsOrdFunction.GstrSimCode, argGBIO);
                if (clsOrdFunction.GstrSimYN == "Y")
                {
                    if (clsOrdFunction.GstrSetOldYN != "Y")
                    {
                        frmPmpaJSimsaGijun f = new frmPmpaJSimsaGijun(clsOrdFunction.GstrSimCode);
                        f.ShowDialog();
                        OF.fn_ClearMemory(f);
                    }
                }
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SIMFLAG].Text = clsOrdFunction.GstrSimYN;

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.MAYAK].Text = OF.READ_MAYAK(clsDB.DbCon, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim());

                if (clsPublic.GstrSugaFind == "PRN")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNMARK].Text = "#";
                }
                
                //약제 통합메시지
                clsOrderEtc.ALL_OCS_SUGA_MESSAGE(clsDB.DbCon, "입원", dt.Rows[0]["SUCODE"].ToString().Trim(), SpdNm,
                        (int)BaseOrderInfo.IpdOrderCol.NAMEENG, SpdNm.ActiveSheet.ActiveRowIndex, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text);

                //골다공증메시지
                string sMsg = clsOrdFunction.READ_BONE_Result_Check(clsDB.DbCon, clsOrdFunction.Pat.PtNo, dt.Rows[0]["SUCODE"].ToString());

                if (sMsg != "" && sMsg != null)
                {
                    FrmMedDocMsgBox f = new FrmMedDocMsgBox(sMsg, "");
                    f.ShowDialog();
                    OF.fn_ClearMemory(f);
                }

                //부가세
                if (clsOrderEtc.READ_SUGA_VALUE_ADDED_TAX(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                {
                    clsOrdFunction.Gstr부가세 = "OK";
                }

                if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2081" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2082" ||
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2041" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2042")
                {
                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2041" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2042")
                    {
                        strMsg = "";
                        strMsg += "FFP 는 아래의 경우에만 보험처방 가능합니다" + "\r\n";
                        strMsg += "* PT  또는  PTT 가 정상수치의 30% 이하 저하된경우" + "\r\n";
                        strMsg += "* PT결과 INR 1.5 이상인 경우" + "\r\n";
                        strMsg += "* Hypofibrinogenemia(100 이하)인경우" + "\r\n";
                        strMsg += "* 8 pints 까지 인정됨" + "\r\n";
                        strMsg += "그외 처방해야 하는 경우 보험 안됩니다. 비보험 설명. 비보험 처방 해주세요" + "\r\n";
                        strMsg += "(금액: 1PINT 당 5 만원)";
                        MessageBox.Show(strMsg, "수가처방정보", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2081" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2082")
                    {
                        strMsg = "";
                        strMsg += "혈소판 농축액은 혈소판 수치가" + "\r\n";
                        strMsg += "5만 이하의 경우 8  pints." + "\r\n";
                        strMsg += "2만 이하의 경우 12 pints 까지 보험인정 됩니다." + "\r\n";
                        strMsg += "(검사수치가 안되는 경우에는 보험 안됨.  비보험설명.비보험처방 해주세요)";
                        MessageBox.Show(strMsg, "수가처방정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                if (clsOrdFunction.GSelfMedOrd == "Y")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = clsOrdFunction.GSelfMedDosCode;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = clsOrdFunction.GSelfMedQty;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = clsOrdFunction.GSelfMedQty;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = clsOrdFunction.GSelfMedDiv;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = clsOrdFunction.GSelfMedPlusName;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = fn_PlusName(clsOrdFunction.GSelfMedDosCode); 
                }

                if (VB.Left(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text.Trim(), 1) != "A")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text.PadRight(10) +
                                                                                                     SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                }
                #endregion //IPD
            }
            else if (argGBIO == "ER")
            {
                #region //ER
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();

                if (dt.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                {
                    strUnit = dt.Rows[0]["CORDERNAME"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = strUnit + " " + dt.Rows[0]["CORDERNAMES"].ToString();
                }
                else if (dt.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = dt.Rows[0]["CDISPHEADER"].ToString() + " " +
                    dt.Rows[0]["CORDERNAME"].ToString();
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString();
                }

                //금액적용시
                if (dt.Rows[0]["CGBBOTH"].ToString() == "1")
                {
                    strName = ComFunc.LeftH(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text, 30);
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = strName + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text;
                }

                if (clsOrdFunction.GstrCDSSYN == "Y")
                {
                    if (Convert.ToInt32(dt.Rows[0]["BUN"].ToString().Trim()) >= 11 && Convert.ToInt32(dt.Rows[0]["BUN"].ToString().Trim()) <= 20 ||
                        Convert.ToInt32(dt.Rows[0]["BUN"].ToString().Trim()) == 23)
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.CONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
                        if (dt.Rows[0]["CNEXTCODE"].ToString() == "0")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.CONTENTS].Locked = true;
                        }
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NEXTCODE].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
                    }
                }


                


                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = (Convert.ToString(nQty) == "0" ? "1" : Convert.ToString(nQty));
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = dt.Rows[0]["GBDIV"].ToString();
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text = dt.Rows[0]["NAL"].ToString();

                if (clsPublic.Gstr산제Chk == "OK")  //병동/ER 처방은 무조건 OK
                {
                    //if (OF.READ_POWDER(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "Y")
                    if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                    }
                }

                if (clsPublic.Gstr산제Chk == "OK")  //병동/ER 처방은 무조건 OK
                {
                    if (clsPublic.Gstr파우더New_STS == "Y")
                    {
                        if (clsOrderEtc.CHK_POWDER_SUCODE_CHK(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                        {
                            if (clsOrdFunction.GnReadOrder < startRow)
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].Text = "True";
                            }
                        }
                    }
                }
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NGT].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBER].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SELF].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBREMARK].Text = "";

                if (clsPublic.GstrSugaFind == "PRN")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBREMARK].Text = "P";
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POTABLE].Text = "";

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = dt.Rows[0]["SUCODE"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.BUN].Text = dt.Rows[0]["BUN"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SLIPNO].Text = dt.Rows[0]["SLIPNO"].ToString().Trim();

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN].Text = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), ComFunc.LeftH(dt.Rows[0]["DOSCODE"].ToString(), 2), dt.Rows[0]["BUN"].ToString()), 7);
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SORT].Text = VB.Right(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["DOSCODE"].ToString(), 2), dt.Rows[0]["BUN"].ToString()).Trim(), 3);

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.QTY].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBBOTH].Text = "";

                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REMARK].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text = dt.Rows[0]["CDISPRGB"].ToString();

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBBOTH1].Text = dt.Rows[0]["GBBOTH"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO1].Text = dt.Rows[0]["GBINFO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBQTY].Text = dt.Rows[0]["CGBQTY"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBDOSAGE].Text = dt.Rows[0]["CGBDOSAGE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NEXTCODE].Text = dt.Rows[0]["CNEXTCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBIMIV].Text = dt.Rows[0]["CGBIMIV"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.CBUN].Text = dt.Rows[0]["CBUN"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.BCONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ROWID].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SEQNO].Text = Convert.ToString(startRow);

                //선수납항목 체크
                if (dt.Rows[0]["SUGBN"].ToString() == "1")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUNSUNAP].Text = "S";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "(A)" + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text;

                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDERNOMARK].Text = "#";
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDERNOREASON].Text = dt.Rows[0]["SUCODE"].ToString() + ">>" + clsPublic.Gstr파우더_SuCode;
                //소견 체크
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBOPINION].Text = dt.Rows[0]["OPINION_YN"].ToString();

                string cDosCode = dt.Rows[0]["DOSCODE"].ToString();

                if (dt.Rows[0]["BUN"].ToString() == "11" || dt.Rows[0]["BUN"].ToString() == "12")
                {
                    cDosCode = "010501";
                    if (dt.Rows[0]["SUCODE"].ToString() == "NIG06")
                    {
                        cDosCode = "490301";
                    }
                }
                else if (dt.Rows[0]["BUN"].ToString() == "20")
                {
                    switch (dt.Rows[0]["SUCODE"].ToString())
                    {
                        case "NSB":
                            cDosCode = "930120";
                            break;
                        case "PACETA":
                            cDosCode = "930120";
                            break;
                        case "MIN2":
                            cDosCode = "910120";
                            break;
                        default:
                            cDosCode = "920120";
                            break;
                    }
                }

                if (dt.Rows[0]["GBINFO"].ToString() != "") 
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dt.Rows[0]["GBINFO"].ToString();
                }
                else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "1")
                {
                    if (dt.Rows[0]["DOSNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBDOSAGE].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                        if (dt.Rows[0]["GBDIV"].ToString() == "0")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = "";
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = "";//용법을 바꾸면 따라서 총투량도 같이 바꾼다.
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBREMARK].Text = "";
                        }
                        else
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = dt.Rows[0]["GBDIV"].ToString();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = dt.Rows[0]["GBDIV"].ToString();
                        }
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                    }
                }
                else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "2")
                {
                    if (dt.Rows[0]["SPECNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dt.Rows[0]["SPECNAME"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                    }
                }

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    SpdNm.ActiveSheet.Cells[startRow, 2, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(128, 0, 0);
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, 2, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                }

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text = "80";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text = "";
                }

                if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "★항혈전 " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].ForeColor = Color.FromArgb(255, 0, 255);
                }

                if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "<!> " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                }

                if (clsOrderEtc.READ_SUGA_MR_EXPENSIVE_MEDICINE(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim(), clsOrdFunction.Pat.DeptCode) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "[재고X] " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                }

                if (clsPublic.GstrSugaFind == "PRN")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNMARK].Text = "#";
                }

                if (argPRN == "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNREMARK].Text = dt.Rows[0]["PRN_REMARK"].ToString().Trim();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.INSULINSCALE].Text = dt.Rows[0]["PRN_INS_GBN"].ToString().Trim();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.INSULINUNIT].Text = dt.Rows[0]["PRN_INS_UNIT"].ToString().Trim();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.INSULINMAX].Text = dt.Rows[0]["PRN_INS_MAX"].ToString().Trim();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNDOSCODE].Text = dt.Rows[0]["PRN_DOSCODE"].ToString().Trim();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNTERM].Text = dt.Rows[0]["PRN_TERM"].ToString().Trim();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNNOTITIME].Text = dt.Rows[0]["PRN_NOTIFY"].ToString().Trim();

                    if (dt.Rows[0]["PRN_REMARK"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNMARK].Text = "#";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PRNREMARK].Text = dt.Rows[0]["PRN_REMARK"].ToString().Trim();
                    }
                }

                //약제 통합메시지
                clsOrderEtc.ALL_OCS_SUGA_MESSAGE(clsDB.DbCon, "입원", dt.Rows[0]["SUCODE"].ToString().Trim(), SpdNm,
                        (int)BaseOrderInfo.ErOrderCol.NAMEENG, SpdNm.ActiveSheet.ActiveRowIndex, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text);

                //골다공증메시지
                string sMsg = clsOrdFunction.READ_BONE_Result_Check(clsDB.DbCon, clsOrdFunction.Pat.PtNo, dt.Rows[0]["SUCODE"].ToString());

                if (sMsg != "" && sMsg != null)
                {
                    FrmMedDocMsgBox f = new FrmMedDocMsgBox(sMsg, "");
                    f.ShowDialog();
                    OF.fn_ClearMemory(f);
                }

                //부가세
                if (clsOrderEtc.READ_SUGA_VALUE_ADDED_TAX(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                {
                    clsOrdFunction.Gstr부가세 = "OK";
                }

                if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "X2081" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "X2082" ||
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "X2041" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "X2042")
                {
                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "X2041" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "X2042")
                    {
                        strMsg = "";
                        strMsg += "FFP 는 아래의 경우에만 보험처방 가능합니다" + "\r\n";
                        strMsg += "* PT  또는  PTT 가 정상수치의 30% 이하 저하된경우" + "\r\n";
                        strMsg += "* PT결과 INR 1.5 이상인 경우" + "\r\n";
                        strMsg += "* Hypofibrinogenemia(100 이하)인경우" + "\r\n";
                        strMsg += "* 8 pints 까지 인정됨" + "\r\n";
                        strMsg += "그외 처방해야 하는 경우 보험 안됩니다. 비보험 설명. 비보험 처방 해주세요" + "\r\n";
                        strMsg += "(금액: 1PINT 당 5 만원)";
                        MessageBox.Show(strMsg, "수가처방정보", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "X2081" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "X2082")
                    {
                        strMsg = "";
                        strMsg += "혈소판 농축액은 혈소판 수치가" + "\r\n";
                        strMsg += "5만 이하의 경우 8  pints." + "\r\n";
                        strMsg += "2만 이하의 경우 12 pints 까지 보험인정 됩니다." + "\r\n";
                        strMsg += "(검사수치가 안되는 경우에는 보험 안됨.  비보험설명.비보험처방 해주세요)";
                        MessageBox.Show(strMsg, "수가처방정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text.PadRight(10) +
                                                                                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                #endregion //ER
            }
        }

        string fn_PRN_IV_2_SET(string strChk, string strDosCode, string strValue)
        {
            string rtnVal = strDosCode;

            switch (strValue)
            {
                case "경구":
                    rtnVal = "010501";
                    break;
                case "설하":
                    rtnVal = "490301";
                    break;
                case "IM":
                    rtnVal = "910120";
                    break;
                case "IV":
                    rtnVal = "920120";
                    break;
                case "IV infusion":
                    rtnVal = "930120";
                    break;
                case "SC":
                    rtnVal = "950120";
                    break;
                default:
                    rtnVal = strDosCode;
                    break;
            }

            return rtnVal;
        }

        string fn_PlusName(string strDosCode)
        {
            string rtnVal = "";

            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT DOSNAME                                 \r";
                SQL += "   FROM KOSMOS_OCS.OCS_ODOSAGE                  \r";
                SQL += "  WHERE DOSCODE = '" + strDosCode.Trim() + "'   \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return "";
                }

                if (reader.HasRows && reader.Read())
                {
                    //rtnVal = dt.Rows[0]["DOSNAME"].ToString().Trim();
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                reader = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            return rtnVal;
        }

        public void fn_Diag_Select_Move(DataTable dt, string argGBIO, FarPoint.Win.Spread.FpSpread SpdNm, int startRow)
        {
            if (argGBIO == "OPD")
            {
                if (clsOrdFunction.GEnvSet_Item21 != null && clsOrdFunction.GEnvSet_Item21 == "2")
                {
                    SpdNm.ActiveSheet.Cells[startRow, 2].Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                    SpdNm.ActiveSheet.Cells[startRow, 3].Text = dt.Rows[0]["ILLNAME"].ToString().Trim();
                    if (dt.Rows[0]["GBVCODE1"].ToString().Trim() == "*")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, 3].Text = "@(희귀)" + dt.Rows[0]["ILLNAME"].ToString().Trim();
                    }
                    else if (dt.Rows[0]["GBVCODE2"].ToString().Trim() == "*")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, 3].Text = "@(난치)" + dt.Rows[0]["ILLNAME"].ToString().Trim();
                    }
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, 0].Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                    SpdNm.ActiveSheet.Cells[startRow, 1].Text = dt.Rows[0]["ILLNAME"].ToString().Trim();
                    if (dt.Rows[0]["GBVCODE1"].ToString().Trim() == "*")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, 1].Text = "@(희귀)" + dt.Rows[0]["ILLNAME"].ToString().Trim();
                    }
                    else if (dt.Rows[0]["GBVCODE2"].ToString().Trim() == "*")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, 1].Text = "@(난치)" + dt.Rows[0]["ILLNAME"].ToString().Trim();
                    }
                }
            }
            else if (argGBIO == "IPD" || argGBIO == "ER")
            {
                SpdNm.ActiveSheet.Cells[startRow, 0].Text = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");
                SpdNm.ActiveSheet.Cells[startRow, 1].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, 2].Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, 3].Text = dt.Rows[0]["ILLNAME"].ToString().Trim();
                if (dt.Rows[0]["GBVCODE1"].ToString().Trim() == "*")
                {
                    SpdNm.ActiveSheet.Cells[startRow, 3].Text = "@(희귀)" + dt.Rows[0]["ILLNAME"].ToString().Trim();
                }
                else if (dt.Rows[0]["GBVCODE2"].ToString().Trim() == "*")
                {
                    SpdNm.ActiveSheet.Cells[startRow, 3].Text = "@(난치)" + dt.Rows[0]["ILLNAME"].ToString().Trim();
                }
            }
        }

        public void fn_Diag_AutoDisplay(DataTable dt, FarPoint.Win.Spread.FpSpread SpdNm, int startRow, int startCol)
        {
            SpdNm.ActiveSheet.Cells[startRow, 0].Text = clsOrdFunction.GstrBDate;
            SpdNm.ActiveSheet.Cells[startRow, startCol].Text = dt.Rows[0]["ILLCODE"].ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, startCol + 1].Text = (dt.Rows[0]["GBV252"].ToString() == "*" ? "★" : "") + dt.Rows[0]["ILLNAME"].ToString().Trim();
            if (dt.Rows[0]["GbVCode"].ToString().Trim() == "*")
            {
                SpdNm.ActiveSheet.Cells[startRow, startCol + 1].Text = "@" + dt.Rows[0]["ILLNAME"].ToString().Trim();
            }

            //SpdNm.ActiveSheet.Cells[startRow, 2, startRow, SpdNm.ActiveSheet.ColumnCount - 1].Text = "";
        }

        void fn_Read_Drug_Jep(string sOrderCode)
        {
            if (sOrderCode.Length > 1)
            {
                clsPublic.GstrHelpCode = sOrderCode;
                using (FrmMedDrugHelp f = new FrmMedDrugHelp(sOrderCode))
                {
                    f.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("1글자 이상 입력하십시오", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        string fn_Xray_Check(FarPoint.Win.Spread.FpSpread SpdNm, int nRow)
        {
            string strRtn = "";
            string strBun = "";
            string strCBun = "";
            string strNextCode = "";
            string strMsg = "";

            if (SpdNm.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.DC].Text == "True")
            {
                return strRtn;
            }

            strBun = dt.Rows[0]["BUN"].ToString();
            strCBun = dt.Rows[0]["CBUN"].ToString();
            strNextCode = dt.Rows[0]["NEXTCODE"].ToString();

            if (strNextCode == "") return "";

            for (int i = 0; i < SpdNm.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (SpdNm.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.DC].Text != "True")
                {
                    if (SpdNm.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text.Trim() == strNextCode.Trim())
                    {
                        strMsg = "";
                        strMsg = "중복 처방 발생 : [" + SpdNm.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text.Trim() + "]";
                        MessageBox.Show(strMsg, "추가취소", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        strRtn = "NO";
                        break;
                    }
                }
            }

            return strRtn;
        }

        public void fn_OneTimeOrder_Move(DataTable dt, int index, string argOrdGbn, string argGBIO, FarPoint.Win.Spread.FpSpread SpdNm, int startRow, string strOrderCode, string strBun, string strSlipNo, string strSrcDoscode, double nQty, double nDiv, string strSuCode, string strGbInfo, string sMulti, double nQty2, string strDosCode)
        {
            string strUnit = "";
            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType chk = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

            //string strBun = "";
            //string strNextCode = "";
            //string strOrderName = "";

            //string strSELECTOrderCode = "";
            //string strSELECTOrderName = "";
            //string strSELECTDosName = "";
            //string strSELECTDosCode = "";
            //string strSELECTSlipnos = "";
            //string strSELECTBun = "";
            //string strSELECTCBun = "";
            //string strSELECTDiv = "";
            //string strSELECTSuCode = "";
            //string strSELECTGbInfo = "";

            //string strName;
            //string strillName = "";
            //string strMsg = "";
            //string cSlipNo = "";
            string cDosCode = "";
            //string strSpecCode = "";


            startRow = SpdNm.ActiveSheet.NonEmptyRowCount;

            clsMedFunction.SimsaMsg_Check(clsDB.DbCon, strOrderCode, clsOrdFunction.Pat.Bi);
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();

            if (dt.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
            {
                strUnit = dt.Rows[0]["CORDERNAME"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = strUnit + " " + dt.Rows[0]["CORDERNAMES"].ToString();
            }
            else if (dt.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["CDISPHEADER"].ToString() + " " +
                dt.Rows[0]["CORDERNAME"].ToString();
            }
            else
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString();
            }

            if (strGbInfo != "" && dt.Rows[0]["CGBBOTH"].ToString() == "1")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = ComFunc.LeftH(dt.Rows[0]["CORDERNAME"].ToString(), 30) + " " + strGbInfo;
            }

            if (int.Parse(dt.Rows[0]["BUN"].ToString().Trim()) >= 11 && int.Parse(dt.Rows[0]["BUN"].ToString().Trim()) <= 20)
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
                if (dt.Rows[0]["CNEXTCODE"].ToString().Trim() == "0")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Locked = true;
                }
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BCONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
            }


            if (clsPublic.Gstr산제Chk != "OK")
            {
                //if (OF.READ_POWDER(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "Y")
                if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].CellType = chk;
                }
            }
            else
            {
                if (clsPublic.Gstr파우더New_STS == "Y")
                {
                    if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                    {
                        if (clsOrdFunction.GnReadOrder <= startRow)
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].CellType = chk;
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].Text = "True";

                            if (clsOrdFunction.Read_Powder_SuCode_New2(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                            {
                                if (clsPublic.Gstr파우더New_STS == "" || clsPublic.Gstr파우더STS == "OK")
                                {
                                    //return;
                                }

                            }
                        }
                    }
                }
            }

            //if (clsPublic.Gstr산제Chk != "OK")
            //{
            //    if (OF.READ_POWDER(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "Y")
            //    {
            //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].CellType = chk;
            //    }
            //}

            //if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
            //{
            //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].CellType = chk;
            //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].Text = "";
            //}
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAL].Text = "1";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NGT].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ER].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "";

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POTABLE].Text = "";

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text = strSuCode;
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BUN].Text = dt.Rows[0]["BUN"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text = dt.Rows[0]["SLIPNO"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), ComFunc.LeftH(dt.Rows[0]["SPECCODE"].ToString(), 2), dt.Rows[0]["BUN"].ToString()), 7);
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPGUBUN].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), ComFunc.LeftH(dt.Rows[0]["SPECCODE"].ToString(), 2), dt.Rows[0]["BUN"].ToString()), 3);
            //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = "1";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = nQty2.ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBBOTH].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text = strGbInfo.Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = dt.Rows[0]["CDISPRGB"].ToString();

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CGBBOTH].Text = dt.Rows[0]["CGBBOTH"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CGBINFO].Text = dt.Rows[0]["CGBINFO"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBQTY].Text = dt.Rows[0]["CGBQTY"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBDOSAGE].Text = dt.Rows[0]["CGBDOSAGE"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NEXTCODE].Text = dt.Rows[0]["CNEXTCODE"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBIMIV].Text = dt.Rows[0]["CGBIMIV"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ROWID].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SEQNO].Text = (startRow + 1).ToString();

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.STAFFID].Text = clsOrdFunction.GstrDrCode;
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DRCODE].Text = clsType.User.Sabun;

            if (sMulti.Trim() == "" || sMulti.Trim().Length > 3)
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.MULTIREASON].Text = sMulti.Trim();
                if (sMulti.Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.MULTI].Text = "E";
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.MULTI].Text = "";
                }
            }


            OracleDataReader reader = null;
            //선수납 항목 체크
            try
            {
                SQL = "";
                SQL += " SELECT SuGbN                                                       \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_SUN                                         \r";
                SQL += "  WHERE SuNext = '" + dt.Rows[0]["SuCode"].ToString().Trim() + "'   \r";
                SQL += "    AND SuGbN  = '1'                                                \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (reader.HasRows)
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUNSUNAP].Text = "S";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "(A)" + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                }
                reader.Dispose();
                reader = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text.Trim().PadRight(10) + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text.Trim();

            switch (dt.Rows[0]["CGBIMIV"].ToString().Trim())
            {
                case "4":
                case "5":
                case "6":
                    try
                    {
                        SQL = "";
                        SQL += " SELECT 1 FROM KOSMOS_OCS.ENDO_REMARK                                   \r";
                        SQL += "  WHERE Ptno  = '" + clsOrdFunction.Pat.PtNo + "'                       \r";
                        SQL += "    AND JDate = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')   \r";
                        SQL += "    AND OrderCode = '" + dt.Rows[0]["OrderCode"].ToString() + "'        \r";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (reader.HasRows)
                        {
                            clsOrdFunction.GstrResultChk = "1";
                        }
                        reader.Dispose();
                        reader = null;
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                    break;
                case "8":
                    try
                    {
                        SQL = "";
                        SQL += " SELECT ROWID FROM  KOSMOS_OCS.EXAM_ANATMST                                 \r"; //Cytology
                        SQL += "  WHERE Ptno      = '" + clsOrdFunction.Pat.PtNo + "'                       \r";
                        SQL += "    AND BDate     = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')   \r";
                        SQL += "    AND OrderCode = '" + dt.Rows[0]["OrderCode"].ToString().Trim() + "'     \r";
                        SQL += "    AND GbIO      = 'I'                                                     \r";
                        SQL += "    AND DeptCode  = '" + clsPublic.GstrDeptCode.Trim() + "'                 \r";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (reader.HasRows)
                        {
                            clsOrdFunction.GstrResultChk = "1";
                        }
                        reader.Dispose();
                        reader = null;
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                    break;
                default:
                    break;
            }

            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.RESULT].Text = clsOrdFunction.GstrResultChk;

            cDosCode = dt.Rows[0]["SPECCODE"].ToString().Trim();
            cDosCode = "491001";    //약 용법 고정 - other-1time-1time  , 먼저약 A 설정

            if (strBun == "12" || strBun == "20")
            {
                cDosCode = fn_Change_cDosCode(strSrcDoscode);
            }

            if (cDosCode == "오류")
            {
                MessageBox.Show("원 처방 중 1time 용법이 설정되지 않은 용법이 있습니다. 약제과로 문의하시기 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SQL = "";
            SQL += " SELECT DosName, GbDiv  cGbDiv FROM KOSMOS_OCS.OCS_ODOSAGE   \r";
            SQL += "  WHERE DosCode = '" + cDosCode + "'                         \r";
            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt2.Rows.Count == 0)
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSAGE].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";
            }
            else
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt2.Rows[0]["DOSNAME"].ToString();
                if (dt2.Rows[0]["CGBDIV"].ToString() == "0")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt2.Rows[0]["CGBDIV"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = nQty2.ToString();
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = nQty.ToString();
                }

                if (index == 0)
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "A";
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = cDosCode;
            }
            dt2.Dispose();
            dt2 = null;

            //if (dt.Rows[0]["GBINFO"].ToString() == "1")
            //{
            //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["GBINFO"].ToString();
            //}
            //else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "1")
            //{
            //    if (dt.Rows[0]["DOSNAME"].ToString().Trim() == "")
            //    {
            //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
            //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
            //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSAGE].Text = "";
            //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";
            //    }
            //    else
            //    {
            //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
            //        if (dt.Rows[0]["GBDIV"].ToString() == "0")
            //        {
            //            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
            //            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";
            //            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";
            //        }
            //        else
            //        {
            //            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt.Rows[0]["GBDIV"].ToString();
            //            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = nQty2.ToString();
            //        }

            //        if (index == 0)
            //        {
            //            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "A";
            //        }
            //        else
            //        {
            //            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";
            //        }
            //    }
            //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSAGE].Text = cDosCode;
            //}
            //else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "2")
            //{
            //    if (strDosCode.Trim() != "")
            //    {
            //        strSpecCode = strDosCode;
            //    }
            //    else
            //    {
            //        strSpecCode = dt.Rows[0]["SPECCODE"].ToString();
            //    }

            //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["SPECNAME"].ToString();
            //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSAGE].Text = strSpecCode;
            //}
            //else
            //{
            //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
            //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSAGE].Text = "";
            //}

            if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
            {
                SpdNm.ActiveSheet.Cells[startRow, 1, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(128, 0, 0);
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "80";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "";
            }
            else
            {
                SpdNm.ActiveSheet.Cells[startRow, 1, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor
                = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
            }

            if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "★항혈전 " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].ForeColor = Color.FromArgb(255, 0, 255);
            }

            if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "<!> " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
            }
        }

        public string fn_Change_cDosCode(string strSrcDosCode)
        {
            string strRtn = "오류";

            string strDosCode = "";

            strDosCode = VB.Left(strSrcDosCode, 2) + "0000";

            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT NAME                            \r";
                SQL += "   FROM KOSMOS_PMPA.BAS_BCODE           \r";
                SQL += "  WHERE GUBUN = 'OCS_1time_용법'        \r";
                SQL += "    AND CODE = '" + strDosCode + "'     \r";
                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }
                if (reader.HasRows && reader.Read())
                {
                    strRtn = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                reader = null;
                return strRtn;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strRtn;
            }
        }

        public void fn_SOOrder_Move(string argOrdGbn, string argGBIO, FarPoint.Win.Spread.FpSpread SpdNm, int startRow, string argOrderName, string strSlipNo)
        {
            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

            if (argGBIO == "IPD")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = "S/O";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = argOrderName;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = "S/O";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text = strSlipNo;
                if (strSlipNo == "A4")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BUN].Text = "10";
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BUN].Text = "";
                }
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text = "0";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BCONTENTS].Text = "0";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAL].Text = "0";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBDIV].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBBOTH].Text = "0";

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERSITE].Text = clsPublic.GstrDeptCode.Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBVERB].Text = "N";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.VORDERNO].Text = "0";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINUNIT].Text = "0";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CBUN].Text = "0";

                SpdNm.ActiveSheet.Cells[startRow, 2].CellType = txt;
                SpdNm.ActiveSheet.Cells[startRow, 2].Locked = false;
                SpdNm.ActiveSheet.SetActiveCell(startRow, 2);
            }
            else
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN].Text = "S/O";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = argOrderName;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "S/O";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SLIPNO].Text = strSlipNo;
                if (strSlipNo == "A4")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.BUN].Text = "10";
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.BUN].Text = "";
                }
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.CONTENTS].Text = "0";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.BCONTENTS].Text = "0";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.QTY].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text = "0";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBBOTH].Text = "0";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBVERB].Text = "N";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.VORDERNO].Text = "0";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.INSULINUNIT].Text = "0";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.CBUN].Text = "0";
                SpdNm.ActiveSheet.Cells[startRow, 2].CellType = txt;
                SpdNm.ActiveSheet.Cells[startRow, 2].Locked = false;
                SpdNm.ActiveSheet.SetActiveCell(startRow, 2);
            }
        }

        public void fn_Order_Select_Move_OPD(DataTable dt, string argOrdGbn, string argGBIO, FarPoint.Win.Spread.FpSpread SpdNm, int startRow)
        {
            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType chk = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

            string strUnit = "";
            string strOrderName = "";

            string strSELECTOrderCode = "";
            string strSELECTOrderName = "";
            string strSELECTDosName = "";
            string strSELECTDosCode = "";
            string strSELECTSlipnos = "";
            string strSELECTBun = "";
            string strSELECTCBun = "";
            string strSELECTDiv = "";
            string strSELECTSpecCode = "";
            string strSELECTSuCode = "";
            string strSELECTGbInfo = "";

            string strName;

            string strillName = "";
            string strMsg = "";
            //string strchkGS = "";
            //string strSuCode2 = "";

            int nCnt;

            if (argOrdGbn != "SETORDER")
            {
                nCnt = GOrdCnt;
            }
            else
            {
                nCnt = 1;
            }

            #region //OPD
            if (argOrdGbn != "SETORDER")
            {
                clsMedFunction.SimsaMsg_Check(clsDB.DbCon, dt.Rows[0]["ORDERCODE"].ToString().Trim(), clsOrdFunction.Pat.Bi);
            }
            
            if (dt.Columns.IndexOf("IS_MC_EXAM_ORDER") != -1 && dt.Rows[0]["IS_MC_EXAM_ORDER"].ToString().Trim().Equals("1"))
            {
                MessageBox.Show("해당오더 미시행검사(검사부도건)이 있습니다 확인 해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (nCnt == 1)
            {
                if (dt.Rows[0]["SUSUGBJ"].ToString().Trim() == "2")
                {
                    MessageBox.Show("입원 전용 약품입니다.", "처방불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SpdNm.ActiveSheet.Cells[startRow, 0, startRow, SpdNm.ActiveSheet.ColumnCount - 1].Text = "";
                    SpdNm.ActiveSheet.SetActiveCell(startRow, 1);
                    return;
                }

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    //if (dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B001" || dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B002" ||
                    //    dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B003" || dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B004" ||
                    //    dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B005" || dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B006" ||
                    //    dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B009")
                    //{
                    //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString().Trim();
                    //}
                    //else
                    //{
                    if (dt.Rows[0]["CORDERNAME"].ToString().Trim() != "" && dt.Rows[0]["REMARK"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString().Trim();
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = dt.Rows[0]["REMARK"].ToString().Trim();
                    }
                    //}
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERCODE].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();

                    if (dt.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                    {
                        strUnit = dt.Rows[0]["CORDERNAME"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = strUnit + " " + dt.Rows[0]["CORDERNAMES"].ToString();
                    }
                    else if (dt.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = dt.Rows[0]["CDISPHEADER"].ToString() + " " +
                        dt.Rows[0]["CORDERNAME"].ToString();
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString();
                    }

                    if (dt.Rows[0]["CGBBOTH"].ToString() == "1")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = ComFunc.LeftH(dt.Rows[0]["CORDERNAME"].ToString(), 30) + " " + dt.Rows[0]["GBINFO"].ToString();
                    }
                }

                #region 결핵 교육상담료 2021-09-28
                if (dt.Rows[0]["ORDERCODE"].ToString().Trim().Equals("ID110"))
                {
                    string ILLCODE = string.Empty;
                    if (SpdNm.Parent != null)
                    {
                        Form frm = SpdNm.Parent as Form;
                        Control[] SPD = frm.Controls.Find("ssDiagno_OS", true);

                        if (SPD.Length > 0 && SPD[0].Visible)
                        {
                            ILLCODE = (SPD[0] as FpSpread).ActiveSheet.Cells[0, 2].Text.Trim();
                        }
                        else
                        {
                            SPD = frm.Controls.Find("ssDiagno", true);
                            if (SPD.Length > 0 && SPD[0].Visible)
                            {
                                ILLCODE = (SPD[0] as FpSpread).ActiveSheet.Cells[0, 0].Text.Trim();
                            }
                        }
                    }

                    using (frmGyelEdu frm = new frmGyelEdu(clsOrdFunction.Pat.PtNo, ILLCODE))
                    {
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.ShowDialog();
                    }
                }
                else if (dt.Rows[0]["ORDERCODE"].ToString().Trim().Trim().Equals("ID120"))
                {
                    string ILLCODE = string.Empty;
                    if (SpdNm.Parent != null)
                    {
                        Form frm = SpdNm.Parent as Form;
                        Control[] SPD = frm.Controls.Find("ssDiagno_OS", true);

                        if (SPD.Length > 0 && SPD[0].Visible)
                        {
                            ILLCODE = (SPD[0] as FpSpread).ActiveSheet.Cells[0, 2].Text.Trim();
                        }
                        else
                        {
                            SPD = frm.Controls.Find("ssDiagno", true);
                            if (SPD.Length > 0 && SPD[0].Visible)
                            {
                                ILLCODE = (SPD[0] as FpSpread).ActiveSheet.Cells[0, 0].Text.Trim();
                            }
                        }
                    }

                    using (frmGyelEdu2 frm = new frmGyelEdu2(clsOrdFunction.Pat.PtNo, ILLCODE))
                    {
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.ShowDialog();
                    }
                }
                else if (dt.Rows[0]["ORDERCODE"].ToString().Trim().Trim().Equals("ID130"))
                {
                    string ILLCODE = string.Empty;
                    if (SpdNm.Parent != null)
                    {
                        Form frm = SpdNm.Parent as Form;
                        Control[] SPD = frm.Controls.Find("ssDiagno_OS", true);

                        if (SPD.Length > 0 && SPD[0].Visible)
                        {
                            ILLCODE = (SPD[0] as FpSpread).ActiveSheet.Cells[0, 2].Text.Trim();
                        }
                        else
                        {
                            SPD = frm.Controls.Find("ssDiagno", true);
                            if (SPD.Length > 0 && SPD[0].Visible)
                            {
                                ILLCODE = (SPD[0] as FpSpread).ActiveSheet.Cells[0, 0].Text.Trim();
                            }
                        }
                    }

                    using (frmGyelGeneral frm = new frmGyelGeneral(clsOrdFunction.Pat.PtNo, ILLCODE))
                    {
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.ShowDialog();
                    }
                }
                #endregion

                #region 독감 65세이상 체크 로직 추가(의뢰서 작업 2021-10-11) 1956년까지만 되게.
                if (dt.Rows[0]["ORDERCODE"].ToString().Trim().Equals("I-GFLU-0") && clsOrdFunction.Pat.Birth.Left(4).To<int>(0) > 1956)
                {
                    MessageBox.Show("만65세(1957년 이전 출생)이상 전용 처방입니다.", "처방불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SpdNm.ActiveSheet.Cells[startRow, 0, startRow, SpdNm.ActiveSheet.ColumnCount - 1].Text = "";
                    SpdNm.ActiveSheet.SetActiveCell(startRow, 1);
                    return;
                }
                #endregion

                if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
                {
                    if (OF.CHECK_OORDER_IN2(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim(), clsOrdFunction.Pat.PtNo, SpdNm, startRow, "") == "NO")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERCODE].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERCODE, startRow, SpdNm.ActiveSheet.ColumnCount - 1].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERCODE].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();
                    }
                }

                if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = "<!> " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                }

                if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
                {
                    if (clsOrderEtc.READ_SUGA_MR_EXPENSIVE_MEDICINE(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim(), clsOrdFunction.Pat.DeptCode) == "OK")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = "[재고X] " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text;
                        //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                    }

                    if (argOrdGbn != "SETORDER")
                    {
                        //약제 통합메시지
                        clsOrderEtc.ALL_OCS_SUGA_MESSAGE(clsDB.DbCon, "외래", dt.Rows[0]["SUCODE"].ToString().Trim(), SpdNm,
                        (int)BaseOrderInfo.OpdOrderCol.NAMEENG, SpdNm.ActiveSheet.ActiveRowIndex, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text);

                        //골다공증메시지
                        string sMsg = clsOrdFunction.READ_BONE_Result_Check(clsDB.DbCon, clsOrdFunction.Pat.PtNo, dt.Rows[0]["SUCODE"].ToString());
                        if (sMsg != "" && sMsg != null)
                        {
                            using (FrmMedDocMsgBox f = new FrmMedDocMsgBox(sMsg, ""))
                            {
                                f.ShowDialog();
                            }
                        }
                    }
                }

                //부가세
                if (clsOrderEtc.READ_SUGA_VALUE_ADDED_TAX(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBTAX].Text = "True";
                    clsOrdFunction.Gstr부가세 = "OK";
                }

                if (dt.Rows[0]["CGBINFO"].ToString() == "1")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = dt.Rows[0]["GBINFO"].ToString();
                }
                else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "1")
                {
                    if (dt.Rows[0]["DOSNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DIV].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DOSCODE].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                    }
                }
                else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "2")
                {
                    if (dt.Rows[0]["SPECNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DOSCODE].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = dt.Rows[0]["SPECNAME"].ToString();
                    }
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = "";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DIV].Text = dt.Rows[0]["GBDIV"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DOSCODE].Text = "";
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.CONTENTS].Text = dt.Rows[0]["REALQTY"].ToString().Trim();
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DIV].Text = dt.Rows[0]["GBDIV"].ToString();
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAL].Text = dt.Rows[0]["NAL"].ToString().Trim();
                if (argOrdGbn == "SETORDER")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAL].Text = dt.Rows[0]["NAL"].ToString().Trim();
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAL].Text = "1";
                }

                //2020-12-29
                if (VB.Left(dt.Rows[0]["SLIPNO"].ToString().Trim(), 1) != "A")
                {
                    if (dt.Rows[0]["BUN"].ToString().Trim() != "")
                    {
                        if ((int.Parse(dt.Rows[0]["BUN"].ToString().Trim()) >= 11 && int.Parse(dt.Rows[0]["BUN"].ToString().Trim()) <= 20) ||
                             int.Parse(dt.Rows[0]["BUN"].ToString().Trim()) == 23)
                        {
                            if (dt.Rows[0]["CNEXTCODE"].ToString().Trim() != null && dt.Rows[0]["CNEXTCODE"].ToString().Trim() != "")
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NEWCONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
                                if (VB.Val(dt.Rows[0]["CNextCode"].ToString()) == 0)
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NEWCONTENTS].Locked = true;
                                }

                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.BCONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
                            }
                        }
                    }
                }


                #region 2021-01-09                                 

                if (clsType.User.IdNumber == "53775" && argOrdGbn == "SETORDER")
                {
                    if (dt.Rows[0]["TUYEOPOINT"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.TUYEOPOINT].Text = dt.Rows[0]["TUYEOPOINT"].ToString().Trim();
                    }
                    if (dt.Rows[0]["TUYEOTIME"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.TUYEOTIME].Text = dt.Rows[0]["TUYEOTIME"].ToString().Trim().Replace("hr", "").Replace("min", "");

                        if (dt.Rows[0]["TUYEOTIME"].ToString().Trim().Contains("hr"))
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.TIMEGUBUN].Text = "hr";
                        }

                        if (dt.Rows[0]["TUYEOTIME"].ToString().Trim().Contains("min"))
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.TIMEGUBUN].Text = "min";
                        }
                    }
                }

                //cTuyeopoint = VB.Val(ssOpdOrder.ActiveSheet.Cells[nStartRow, (int)BaseOrderInfo.IpdOrderCol.TUYEOPOINT].Text.Trim());
                //cTuyeoTime = ssOpdOrder.ActiveSheet.Cells[nStartRow, (int)BaseOrderInfo.IpdOrderCol.TUYEOTIME].Text.Trim() + ssOpdOrder.ActiveSheet.Cells[nStartRow, (int)BaseOrderInfo.IpdOrderCol.TIMEGUBUN].Text.Trim();

                #endregion

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBER].Text = dt.Rows[0]["GBER"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.REMARKGUBUN].Text = (dt.Rows[0]["REMARK"].ToString().Trim() == "" ? "" : "#");
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBRESERVED].Text = dt.Rows[0]["RES"].ToString() == "1" ? "True" : "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBMULTI].Text = dt.Rows[0]["MULTI"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SUCODE].Text = dt.Rows[0]["SUCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.BUN].Text = dt.Rows[0]["BUN"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.CBUN].Text = dt.Rows[0]["CBUN"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SLIPNO].Text = dt.Rows[0]["SLIPNO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.QTY].Text = dt.Rows[0]["REALQTY"].ToString().Trim();

                if (argOrdGbn == "SETORDER")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.QTY].Text = dt.Rows[0]["CGBQTY"].ToString().Trim();
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBBOTH].Text  = dt.Rows[0]["GBBOTH"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBINFO].Text  = dt.Rows[0]["GBINFO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.REMARK].Text  = dt.Rows[0]["REMARK"].ToString();

                if (dt.Rows[0]["CDISPRGB"].ToString() != null && dt.Rows[0]["CDISPRGB"].ToString() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DISPRGB].Text = dt.Rows[0]["CDISPRGB"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, 1, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor
                        = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                }

                if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = "★항혈전 " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].ForeColor = Color.FromArgb(255, 0, 255);
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBBOTH1].Text = dt.Rows[0]["CGBBOTH"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBINFO1].Text = dt.Rows[0]["CGBINFO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBQTY].Text = dt.Rows[0]["CGBQTY"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBDOSAGE].Text = dt.Rows[0]["CGBDOSAGE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NEXTCODE].Text = dt.Rows[0]["CNEXTCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBIMIV].Text = dt.Rows[0]["CGBIMIV"].ToString();

                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERNO].Text = dt.Rows[0]["ORDERNO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERNO].Text = dt.Rows[0]["ORDERNO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSUNAP].Text = dt.Rows[0]["GBSUNAP"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DRCODE].Text = dt.Rows[0]["DRCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.MULTIREMARK].Text = dt.Rows[0]["MULTIREMARK"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DUR].Text = dt.Rows[0]["DUR"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.RESV].Text = dt.Rows[0]["RESV"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBTAX].Text = dt.Rows[0]["GBTAX"].ToString() == "1" ? "True" : "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SCODEREMARK].Text = dt.Rows[0]["SCODEREMARK"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSPCNO].Text = dt.Rows[0]["GBSPC_NO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SCODEREASON].Text = dt.Rows[0]["SCODESAYU"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBCOPY].Text = dt.Rows[0]["GBCOPY"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBBUBUNSUNAP].Text = dt.Rows[0]["GBSUNAP"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DOSCODE1].Text = dt.Rows[0]["DOSCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.REALQTY].Text = dt.Rows[0]["REALQTY"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBDIV].Text = dt.Rows[0]["GBDIV"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBNAL].Text = dt.Rows[0]["NAL"].ToString();
                //2018.08.07 약속처방 급여/비급여 항목 표시 막고 SUGBF 항목 기준으로 Display 하도록 변경 
                //if (argOrdGbn == "SETORDER")
                //{
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].Text = dt.Rows[0]["GBSELF"].ToString();
                //}
                //else
                //{


     

                if (dt.Rows[0]["GBSELF"].ToString() == "" || dt.Rows[0]["GBSELF"].ToString() == "0")
                {
                    if (dt.Rows[0]["SUGBF"].ToString().Trim() == "1" || dt.Rows[0]["SUGBF"].ToString() == "2")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].BackColor = Color.FromArgb(255, 210, 234);
                        if (clsOrdFunction.GstrSelfTest == "True")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].Text = "2";
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SUGBF].Text = dt.Rows[0]["SUGBF"].ToString();
                        }
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].BackColor = Color.FromArgb(255, 255, 234);
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SUGBF].Text = "0";

                    }
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].Text = dt.Rows[0]["GBSELF"].ToString().Trim();
                }
                //}
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.REMARKGUBUN].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBFM].Text = dt.Rows[0]["GBFM"].ToString() == "1" ? "True" : "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SABUN].Text = clsType.User.Sabun.Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.OCSDRUG].Text = dt.Rows[0]["OCSDRUG"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBAUTOSEND].Text = dt.Rows[0]["GBAUTOSEND"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.ASA].Text = dt.Rows[0]["ASA"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PCHASU].Text = dt.Rows[0]["PCHASU"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SUBUL_WARD].Text = dt.Rows[0]["SUBUL_WARD"].ToString();

                strSELECTOrderCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERCODE].Text.Trim();
                strSELECTOrderName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text.Trim();
                strSELECTDosName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text.Trim();
                strSELECTDosCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DOSCODE].Text.Trim();
                strSELECTSlipnos = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SLIPNO].Text.Trim();
                strSELECTBun = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.BUN].Text.Trim();
                strSELECTCBun = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.CBUN].Text.Trim();
                strSELECTDiv = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DIV].Text.Trim();
                //strSELECTSpecCode = dt.Rows[0]["SPECCODE"].ToString().Trim(); 
                
                if (argOrdGbn == "SETORDER")
                {
                    //임신주수 입력
                    if (clsOrderEtc.READ_PREGNANCY_WEEK_NUMBER_USG_CODE(clsDB.DbCon, strSELECTOrderCode) == true)
                    {
                        clsPublic.Gstr임신차수 = "";
                        using (FrmMedPregnantOrder f = new FrmMedPregnantOrder(strSELECTOrderCode, strSELECTOrderName, startRow))
                        {
                            f.ShowDialog();
                        }
                    }
                }
                else
                {
                    switch (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBDOSAGE].Text.Trim())
                    {
                        case "1":
                            if (argOrdGbn == "AutoSearch" && (clsOrdFunction.GEnvSet_Item06 == "" || clsOrdFunction.GEnvSet_Item06 == "2"))
                            {
                                if (dt.Rows[0]["DIV"].ToString() != "0")
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString();
                                }
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.QTY].Text = dt.Rows[0]["DIV"].ToString();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.CONTENTS].Text = dt.Rows[0]["DIV"].ToString();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                            }
                            else if (argOrdGbn == "VIEWORDER" && (clsOrdFunction.GEnvSet_Item06 == "" || clsOrdFunction.GEnvSet_Item06 == "2"))
                            {
                                if (dt.Rows[0]["DIV"].ToString() != "0")
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString();
                                }
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.REALQTY].Text = dt.Rows[0]["REALQTY"].ToString();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.QTY].Text = dt.Rows[0]["QTY"].ToString();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                            }
                            else
                            {
                                if (clsOrdFunction.GEnvSet_Item06 == "1")
                                {
                                    clsOrdFunction.GstrLoadFlag = "OrdSlips";
                                    using (FrmDosCode f = new FrmDosCode("Order", strSELECTDosCode, strSELECTBun, "", "O", strSELECTSpecCode, true))
                                    {
                                        f.StartPosition = FormStartPosition.CenterScreen;
                                        f.ShowDialog();
                                    }
                                    clsOrdFunction.GstrLoadFlag = "";
                                }
                            }
                            break;
                        case "2":
                            using (FrmMedViewSpecimen Specimen = new FrmMedViewSpecimen(strSELECTOrderCode, strSELECTSlipnos, strSELECTDosCode, startRow))
                            {
                                Specimen.ShowDialog();
                            }

                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = clsOrdFunction.GstrSpecNm;
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.DOSCODE].Text = clsOrdFunction.GstrSpecCd;
                            SpdNm.ActiveSheet.SetActiveCell(startRow + 1, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME);
                            break;
                        default:
                            break;
                    }

                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBINFO1].Text == "1")
                    {
                        strSELECTSuCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SUCODE].Text.Trim();
                        strSELECTGbInfo = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBINFO1].Text.Trim();

                        using (FrmViewBoth Both = new FrmViewBoth("Order", strSELECTOrderCode, startRow))
                        {
                            Both.ShowDialog();
                        }

                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SUCODE].Text = clsOrdFunction.GstrSELECTSuCode;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME].Text = clsOrdFunction.GstrSELECTGbInfo;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBINFO].Text = clsOrdFunction.GstrSELECTGbInfo;

                        //해당 코드 비급비급여 여부표시 로직 추가
                        if (clsOrdFunction.GstrSELECTGbSelf == "1" || clsOrdFunction.GstrSELECTGbSelf == "2")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].BackColor = Color.FromArgb(255, 210, 234);
                            if (clsOrdFunction.GstrSelfTest == "True")
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].Text = "2";
                            }
                            else
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].Text = "";
                            }

                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SUGBF].Text = clsOrdFunction.GstrSELECTGbSelf;
                        }
                        else
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].BackColor = Color.FromArgb(255, 255, 234);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBSELF].Text = "";
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SUGBF].Text = "0";
                        }

                        SpdNm.ActiveSheet.SetActiveCell(startRow + 1, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME);
                    }

                    if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
                    {
                        if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBBOTH1].Text == "1")  //금액적용
                        {
                            strSELECTGbInfo = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBINFO].Text;

                            using (FrmMedViewAmt f = new FrmMedViewAmt(strSELECTGbInfo))
                            {
                                f.ShowDialog();
                            }

                            strName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text.Trim();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text = strName.PadRight(10) + clsOrdFunction.GstrSELECTGbInfo;
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBINFO].Text = clsOrdFunction.GstrSELECTGbInfo;
                        }

                        //임신주수 입력
                        if (clsOrderEtc.READ_PREGNANCY_WEEK_NUMBER_USG_CODE(clsDB.DbCon, strSELECTOrderCode) == true)
                        {
                            clsPublic.Gstr임신차수 = "";
                            using (FrmMedPregnantOrder f = new FrmMedPregnantOrder(strSELECTOrderCode, strSELECTOrderName, startRow))
                            {
                                f.ShowDialog();
                            }
                        }
                        clsOrderEtc.CHECK_F0913(strSELECTOrderCode, clsOrdFunction.Pat.DeptCode, clsOrdFunction.Pat.Sex, clsOrdFunction.Pat.Age);

                        //심사기준등록 Display

                        clsOrdFunction.GstrSimCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SUCODE].Text.Trim();
                        clsOrdFunction.GstrSimFlag = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SIMSAGIJUN].Text;
                        clsOrdFunction.GstrSimYN = clsOrdFunction.SimSaGiJun_Check(clsDB.DbCon, clsOrdFunction.GstrSimFlag, clsOrdFunction.GstrSimCode, "OPD");
                        if (clsOrdFunction.GstrSimYN == "Y")
                        {
                            using (frmPmpaJSimsaGijun f = new frmPmpaJSimsaGijun(clsOrdFunction.GstrSimCode))
                            {
                                f.ShowDialog();
                            }
                        }
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SIMSAGIJUN].Text = clsOrdFunction.GstrSimYN;

                        clsOrdFunction.GstrAnatCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERCODE].Text.Trim();
                        clsOrdFunction.GstrAnatName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.NAMEENG].Text.Trim();
                        clsOrdFunction.GstrSELECTGbImiv = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBIMIV].Text.Trim();

                        clsOrdFunction.GstrResultChk = "";

                        string SuCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SUCODE].Text.Trim();

                        switch (clsOrdFunction.GstrSELECTGbImiv)
                        {
                            case "4":
                                FrmViewEndoRemark f = new FrmViewEndoRemark(clsOrdFunction.GstrAnatCode);  //기관지
                                f.ShowDialog();
                                OF.fn_ClearMemory(f);
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.RESULTCHK].Text = clsOrdFunction.GstrResultChk;
                                break;
                            case "5":
                                FrmViewEndoRemark f1 = new FrmViewEndoRemark(clsOrdFunction.GstrAnatCode); //위
                                f1.ShowDialog();
                                OF.fn_ClearMemory(f1);
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.RESULTCHK].Text = clsOrdFunction.GstrResultChk;
                                break;
                            case "6":
                                FrmViewEndoRemark f2 = new FrmViewEndoRemark(clsOrdFunction.GstrAnatCode); //대장
                                f2.ShowDialog();
                                OF.fn_ClearMemory(f2);
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.RESULTCHK].Text = clsOrdFunction.GstrResultChk;
                                break;
                            case "7":
                                FrmViewEndoRemark f3 = new FrmViewEndoRemark(clsOrdFunction.GstrAnatCode); //E.R.C.P
                                f3.ShowDialog();
                                OF.fn_ClearMemory(f3);
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.RESULTCHK].Text = clsOrdFunction.GstrResultChk;
                                break;
                            case "8":
                                FrmViewAnat f4 = new FrmViewAnat(clsOrdFunction.GstrAnatCode, clsOrdFunction.GstrAnatName, "O", true);   //Cytology
                                f4.ShowDialog();
                                OF.fn_ClearMemory(f4);
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.RESULTCHK].Text = clsOrdFunction.GstrResultChk;
                                break;
                            case "9":
                                FrmViewAnat2 f5 = new FrmViewAnat2(clsOrdFunction.GstrAnatCode, clsOrdFunction.GstrAnatName, "O", SuCode); //Pathology
                                f5.ShowDialog();
                                OF.fn_ClearMemory(f5);
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.RESULTCHK].Text = clsOrdFunction.GstrResultChk;
                                break;
                            case "A":
                                strillName = clsOrdFunction.Read_illName(clsOrdFunction.Pat.PtNo, clsOrdFunction.GstrBDate, clsOrdFunction.Pat.DeptCode);
                                FrmViewAnat3 f6 = new FrmViewAnat3(strillName, "O"); //PB smear 소견
                                f6.ShowDialog();
                                OF.fn_ClearMemory(f6);
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.RESULTCHK].Text = clsOrdFunction.GstrResultChk;
                                break;
                            default:
                                break;
                        }

                        #region //진정관리료
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SEDATION].CellType = txt;
                        if (string.Compare(clsPublic.GstrSysDate, "2019-01-24") <= 0)
                        {
                            if (clsType.User.DeptCode != "PC")
                            {
                                if (clsOrdFunction.Pat.Age < 18)
                                {
                                    if (OF.READ_SEDATION(clsDB.DbCon, dt.Rows[0]["ORDERCODE"].ToString().Trim()) == "Y")
                                    {
                                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SEDATION].CellType = chk;
                                        if (ComFunc.MsgBoxQ(dt.Rows[0]["ORDERCODE"].ToString().Trim() + "처치.검사시 진정목적으로 처방하십니까?", "PSMH", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                        {
                                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.SEDATION].Text = "True";
                                        }
                                    }
                                }
                            }
                        }
                        #endregion //진정관리료

                    }
                }

                if (dt.Rows[0]["ORDERCODE"].ToString().Trim().Equals("K7310250"))
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.QTY].Text = "2";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.REALQTY].Text = "2";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.GBQTY].Text = "2";
                }

                string strBloodOK = "";
                strBloodOK = clsOrderEtc.CHK_BLOOD_RDate_ORDER_CHK("외래", clsOrdFunction.Pat.PtNo, clsOrdFunction.Pat.DeptCode, SpdNm, startRow,
                                                                   (int)BaseOrderInfo.OpdOrderCol.SUCODE, (int)BaseOrderInfo.OpdOrderCol.BUN,
                                                                   (int)BaseOrderInfo.OpdOrderCol.REMARK);
                if (clsPublic.Gn혈액사용예정일Row > 0 || strBloodOK != "OK")
                {
                    clsPublic.Gstr혈액사용예정일Date = "";
                    using (FrmMEdBloodUseDaySet f = new FrmMEdBloodUseDaySet())
                    {
                        f.StartPosition = FormStartPosition.CenterScreen;
                        f.ShowDialog();
                    }

                    SpdNm.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, (int)BaseOrderInfo.OpdOrderCol.REMARK].Text += "[혈액사용예정일:" + clsPublic.Gstr혈액사용예정일Date + "]";
                    SpdNm.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, (int)BaseOrderInfo.OpdOrderCol.REMARKGUBUN].Text = "#";
                }

                if (string.Compare(clsPublic.GstrSysDate, "2019-01-01") >= 0)
                {
                    //if (OF.READ_POWDER(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "Y")
                    //{
                    //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.POWDER].CellType = chk;
                    //}
                    //else
                    //{
                        if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.POWDER].CellType = chk;
                        }
                    //}
                }

                if (clsPublic.Gstr산제Chk == "OK")
                {
                    if (clsPublic.Gstr파우더New_STS == "Y")
                    {
                        clsPublic.Gstr파우더Gubun = "";
                        if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                        {
                            if (clsOrdFunction.GnReadOrder < SpdNm.ActiveSheet.NonEmptyRowCount)
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.POWDER].CellType = chk;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.OpdOrderCol.POWDER].Text = "True";

                                if (clsOrdFunction.Read_Powder_SuCode_New2(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                                {
                                    if (clsPublic.Gstr파우더New_STS == "" || clsPublic.Gstr파우더STS == "OK")
                                    {
                                        //return;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            else if (nCnt == 2)
            {
                if (argOrdGbn != "SETORDER")
                {
                    MessageBox.Show("중복 처방 발행 : [" + strOrderName.Trim() + "]", "추가취소", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    SpdNm.ActiveSheet.SetActiveCell(startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERCODE);
                }
            }
            else if (nCnt == 3)
            {
                strMsg = "";
                strMsg += "물리 치료 Order가 아직도 진행중입니다 " + "\r\n";
                strMsg += "만약 물리치료 Order를 내리시고자 할 경우는 " + "\r\n";
                strMsg += "'물리치료실 Order 요청' 항목으로 Order를 내리셔야 합니다 " + "\r\n";
                strMsg += "자세한 사항은 물리치료실로 문의하십시요 " + "\r\n";
                MessageBox.Show(strMsg, "추가취소", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                SpdNm.ActiveSheet.SetActiveCell(startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERCODE);
            }
            else
            {
                MessageBox.Show("Order Code 가 없습니다", "재입력 요망", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                SpdNm.ActiveSheet.SetActiveCell(startRow, (int)BaseOrderInfo.OpdOrderCol.ORDERCODE);
            }


            #endregion //OPD
        }

        

        public void fn_Order_Select_Move_IPD(DataTable dt, string argOrdGbn, string argGBIO, FarPoint.Win.Spread.FpSpread SpdNm, int startRow)
        {
            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType chk = new FarPoint.Win.Spread.CellType.CheckBoxCellType();


            string strUnit = "";
            //string strOrderName = "";

            string strSELECTOrderCode = "";
            string strSELECTOrderName = "";
            string strSELECTDosName = "";
            string strSELECTDosCode = "";
            string strSELECTSlipnos = "";
            string strSELECTBun = "";
            string strSELECTCBun = "";
            string strSELECTDiv = "";
            string strSELECTSpecCode = "";
            //string strSELECTSuCode = "";
            string strSELECTGbInfo = "";

            string strName;

            string strillName = "";
            string strMsg = "";
            string strchkGS = "";
            //string strSuCode2 = "";

            int nCnt;

            if (argOrdGbn != "SETORDER")
            {
                nCnt = GOrdCnt;
            }
            else
            {
                nCnt = 1;
            }

            #region //IPD
            if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
            {
                if (argOrdGbn != "SETORDER" && argOrdGbn != "VIEWORDER")
                {
                    if (dt.Rows[0]["SUCODE"].ToString().Trim() != "")
                    {
                        if (OF.CHECK_OORDER_IN(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim(), clsOrdFunction.Pat.PtNo) == "NO")
                        {
                            SpdNm.Focus();
                            return;
                        }
                    }
                }
            }

            //if (dt.Rows[0]["SUSUGBJ"].ToString().Trim() == "1")
            //{
            //    MessageBox.Show("원외 전용 약품입니다.", "처방불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    SpdNm.ActiveSheet.Cells[startRow, 0, startRow, SpdNm.ActiveSheet.ColumnCount - 1].Text = "";
            //    SpdNm.ActiveSheet.SetActiveCell(startRow, 1);
            //    return;
            //}

            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            // 기존코드를 변경하는 경우가 있어서 클리어 하는 부분이 필요함
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BCONTENTS].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAL].Text = "";
            //////////////////////////////////////////////////////////////////////////////////////////////////////////


            #region 결핵 교육상담료 2021-09-28
            if (dt.Rows[0]["ORDERCODE"].ToString().Trim().Trim().Equals("ID120"))
            {
                string ILLCODE = string.Empty;
                if (SpdNm.Parent != null && SpdNm.Parent.Parent != null)
                {
                    Form frm = SpdNm.Parent.Parent as Form;
                    Control[] SPD = frm.Controls.Find("ssDiagno", true);
                    if (SPD.Length > 0)
                    {
                        ILLCODE = (SPD[0] as FpSpread).ActiveSheet.Cells[0, 2].Text.Trim();
                    }
                }

                using (frmGyelEdu2 frm = new frmGyelEdu2(clsOrdFunction.Pat.PtNo, ILLCODE))
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog();
                }
            }
            #endregion


            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();

            if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
            {
                //if (dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B001" || dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B002" ||
                //    dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B003" || dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B004" ||
                //    dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B005" || dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B006" ||
                //    dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B009")
                //{
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString().Trim();
                //}
                //else
                //{
                if (dt.Rows[0]["CORDERNAME"].ToString().Trim() != "" && dt.Rows[0]["REMARK"].ToString().Trim() == "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString().Trim();
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["REMARK"].ToString().Trim();
                }
                //}
            }
            else
            {
                if (dt.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                {
                    strUnit = dt.Rows[0]["CORDERNAME"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = strUnit + " " + dt.Rows[0]["CORDERNAMES"].ToString();
                }
                else if (dt.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["CDISPHEADER"].ToString() + " " +
                    dt.Rows[0]["CORDERNAME"].ToString();
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString();
                }

                if (dt.Rows[0]["CGBBOTH"].ToString() == "1")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = ComFunc.LeftH(dt.Rows[0]["CORDERNAME"].ToString(), 30) + " " + dt.Rows[0]["GBINFO"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CGBBOTH].Text = dt.Rows[0]["CGBBOTH"].ToString();
                }
            }

            if (VB.Left(dt.Rows[0]["SLIPNO"].ToString().Trim(), 1) != "A")
            {
                if (dt.Rows[0]["BUN"].ToString().Trim() != "")
                {
                    if ((int.Parse(dt.Rows[0]["BUN"].ToString().Trim()) >= 11 && int.Parse(dt.Rows[0]["BUN"].ToString().Trim()) <= 20) ||
                         int.Parse(dt.Rows[0]["BUN"].ToString().Trim()) == 23)
                    {
                        if (dt.Rows[0]["CNEXTCODE"].ToString().Trim() != null && dt.Rows[0]["CNEXTCODE"].ToString().Trim() != "")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
                            if (VB.Val(dt.Rows[0]["CNextCode"].ToString()) == 0)
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Locked = true;
                            }

                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BCONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
                        }
                    }
                }
            }

            #region 2021-01-09                                 

            if (clsType.User.IdNumber == "53775" && (argOrdGbn == "VIEWORDER" || argOrdGbn == "SETORDER"))
            {
                if (dt.Rows[0]["TUYEOPOINT"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.TUYEOPOINT].Text = dt.Rows[0]["TUYEOPOINT"].ToString().Trim();
                }
                if (dt.Rows[0]["TUYEOTIME"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.TUYEOTIME].Text = dt.Rows[0]["TUYEOTIME"].ToString().Trim().Replace("hr", "").Replace("min", "");

                    if (dt.Rows[0]["TUYEOTIME"].ToString().Trim().Contains("hr"))
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.TIMEGUBUN].Text = "hr";
                    }

                    if (dt.Rows[0]["TUYEOTIME"].ToString().Trim().Contains("min"))
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.TIMEGUBUN].Text = "min";
                    }
                }
            }

            //cTuyeopoint = VB.Val(ssOpdOrder.ActiveSheet.Cells[nStartRow, (int)BaseOrderInfo.IpdOrderCol.TUYEOPOINT].Text.Trim());
            //cTuyeoTime = ssOpdOrder.ActiveSheet.Cells[nStartRow, (int)BaseOrderInfo.IpdOrderCol.TUYEOTIME].Text.Trim() + ssOpdOrder.ActiveSheet.Cells[nStartRow, (int)BaseOrderInfo.IpdOrderCol.TIMEGUBUN].Text.Trim();

            #endregion

            ////2019-08-27 전산업무의뢰서 2019-08-09
            //if ((int)VB.Val(dt.Rows[0]["BUN"].ToString().Trim()) == 20)
            //{
            //    if (dt.Rows[0]["CNEXTCODE"].ToString().Trim() != "")
            //    {
            //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTSUNIT].Text = dt.Rows[0]["cDispHeader"].ToString().Trim();
            //    }
            //}

            //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "1";
            //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAL].Text = "1";

            if (clsPublic.Gstr산제Chk != "OK")
            {
                //if (OF.READ_POWDER(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "Y")
                if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].CellType = chk;
                }
            }
            else
            {
                if (clsPublic.Gstr파우더New_STS == "Y")
                {
                    if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                    {
                        if (clsOrdFunction.GnReadOrder <= startRow)
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].CellType = chk;
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].Text = "True";

                            if (clsOrdFunction.Read_Powder_SuCode_New2(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                            {
                                if (clsPublic.Gstr파우더New_STS == "" || clsPublic.Gstr파우더STS == "OK")
                                {
                                    //return;
                                }

                            }
                        }
                    }
                }
            }

            if (clsPublic.Gstr구두수동Chk == "OK")
            {
                if (clsOrdFunction.GnReadOrder < SpdNm.ActiveSheet.NonEmptyRowCount)
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.VERBALGUBUN].CellType = chk;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.VERBALGUBUN].Text = "False";
                }
            }
            if (VB.Left(dt.Rows[0]["SLIPNO"].ToString().Trim(), 1) != "A")
            {
                if (clsOrderEtc.Read_EMERGENCY_MEDICINE_GROUP(dt.Rows[0]["BUN"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.AIRSHT].CellType = chk;
                }
            }



            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NGT].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ER].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text = dt.Rows[0]["SUCODE"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BUN].Text = dt.Rows[0]["BUN"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CBUN].Text = dt.Rows[0]["CBUN"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text = dt.Rows[0]["SLIPNO"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = dt.Rows[0]["REALQTY"].ToString().Trim();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = dt.Rows[0]["REALQTY"].ToString().Trim();
            if (argOrdGbn == "SETORDER")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = dt.Rows[0]["CGBQTY"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAL].Text = dt.Rows[0]["NAL"].ToString().Trim();

                if (int.Parse(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAL].Text) > 1 &&
                    dt.Rows[0]["GBTFLAG"].ToString().Trim() == "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAL].Text = "1";
                    clsOrdFunction.GstrMessage = "1일 이상은 처방을 내릴수 없어" + "\r\n\r\n" +
                                                 "일수를 1일로 조정 하였습니다." + "\r\n\r\n" +
                                                 "다시한번 확인 하십시오.";
                }
            }
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBBOTH].Text = dt.Rows[0]["GBBOTH"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text = dt.Rows[0]["GBINFO"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REMARK].Text = dt.Rows[0]["REMARK"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = dt.Rows[0]["CDISPRGB"].ToString();
            if (dt.Rows[0]["CDISPRGB"].ToString().Trim() != "")
            {
                SpdNm.ActiveSheet.Cells[startRow, 1, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor
                    = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
            }
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBQTY].Text = dt.Rows[0]["CGBQTY"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBDOSAGE].Text = dt.Rows[0]["CGBDOSAGE"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NEXTCODE].Text = dt.Rows[0]["CNEXTCODE"].ToString();
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBIMIV].Text = dt.Rows[0]["CGBIMIV"].ToString();
            //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ROWID].Text = "";
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SEQNO].Text = startRow.ToString();

            //선수납항목 체크
            if (dt.Rows[0]["SUGBN"].ToString() == "1")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUNSUNAP].Text = "S";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "(A)" + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
            }
            else
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUNSUNAP].Text = "";
            }

            //!=SETORDER
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.MAYAK].Text = OF.READ_MAYAK(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim());
            
            if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.MAYAK].Text.Trim() == "*")
            {
                using (FrmViewAnat4 f = new FrmViewAnat4(dt.Rows[0]["SUCODE"].ToString().Trim()))
                {
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.TopMost = true;
                    f.BringToFront();
                    f.ShowDialog();
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.MAYAKREMARK].Text = clsOrdFunction.GstrMayakRemark;
                clsOrdFunction.GstrMayakSuCode = "";
                clsOrdFunction.GstrMayakRemark = "";
            }

            if (argOrdGbn == "SETORDER")
            {
                //if(clsOrdFunction.GstrSetOrderGubun.Trim() != "")
                if (!string.IsNullOrEmpty(clsOrdFunction.GstrSetOrderGubun))
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = clsOrdFunction.GstrSetOrderGubun.Trim().ToUpper();
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = VB.Left(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["DOSCODE"].ToString(), 2), dt.Rows[0]["Bun"].ToString()), 7);
                }
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPGUBUN].Text = clsOrdFunction.GstrSetSort.ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBACT].Text = clsOrdFunction.GstrSetGbAct;
            }
            else
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = VB.Left(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["DOSCODE"].ToString(), 2), dt.Rows[0]["Bun"].ToString()), 7);
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPGUBUN].Text = VB.Right(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["DOSCODE"].ToString(), 2), dt.Rows[0]["BUN"].ToString()).Trim(), 3).Trim();
            }
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CGBINFO].Text = dt.Rows[0]["CGBINFO"].ToString();

            if (dt.Rows[0]["CGBINFO"].ToString() == "1")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["GBINFO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text = dt.Rows[0]["GBINFO"].ToString();
            }
            else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "1")
            {
                if (dt.Rows[0]["DOSNAME"].ToString().Trim() == "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = "";

                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                }
            }
            else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "2")
            {
                if (dt.Rows[0]["SPECNAME"].ToString().Trim() == "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = "";
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["SPECNAME"].ToString();
                }
            }
            else
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = "";
            }

            #region //누락부분 추가 
            if (argOrdGbn != "VIEWORDER" && argOrdGbn != "SETORDER")
            {
                if (dt.Rows[0]["cGbDosage"].ToString() == "1")
                {
                    #region //Read_Dosage

                    string strDOSCODE = dt.Rows[0]["DOSCODE"].ToString().Trim();
                    string strBun = dt.Rows[0]["BUN"].ToString().Trim();

                    if (VB.Left(VB.Trim(strDOSCODE), 1) == "9" && VB.Right(VB.Trim(strDOSCODE), 1) == "1")
                    {
                        strDOSCODE = VB.Left(strDOSCODE, 5) + "4";
                    }

                    if (clsPublic.Gstr구두Chk == "OK" && clsPublic.Gstr간호처방STS == "구두처방" && (strBun == "11" || strBun == "12" || strBun == "20"))
                    {
                        if (strBun == "11" || strBun == "12")
                        {
                            strDOSCODE = "491001";
                        }
                        else if (strBun == "20")
                        {
                            strDOSCODE = "491001";
                        }
                    }

                    DataTable dtDoge = null;
                    SQL = " SELECT DosName,GbDiv FROM KOSMOS_OCS.OCS_ODOSAGE ";
                    SQL = SQL + ComNum.VBLF + "WHERE DosCode = '" + strDOSCODE + "' ";
                    SqlErr = clsDB.GetDataTableREx(ref dtDoge, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dtDoge.Rows.Count == 0)
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSAGE].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dtDoge.Rows[0]["DosName"].ToString().Trim();
                        if (dtDoge.Rows[0]["GbDiv"].ToString().Trim() == "0")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";
                        }
                        else
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dtDoge.Rows[0]["GbDiv"].ToString().Trim();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = dtDoge.Rows[0]["GbDiv"].ToString().Trim();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = dtDoge.Rows[0]["GbDiv"].ToString().Trim();
                        }
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSAGE].Text = strDOSCODE;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = strDOSCODE;
                    }
                    dtDoge.Dispose();
                    dtDoge = null;
                    #endregion //Read_Dosage
                }
                else if (dt.Rows[0]["cGbDosage"].ToString() == "2")
                {
                    #region //Read_Specman
                    DataTable dtDoge = null;
                    SQL = " SELECT Specname FROM KOSMOS_OCS.OCS_OSPECIMAN ";
                    SQL = SQL + ComNum.VBLF + "WHERE SpecCode = '" + dt.Rows[0]["DOSCODE"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND Slipno   = '" + dt.Rows[0]["SLIPNO"].ToString().Trim() + "'   ";
                    SqlErr = clsDB.GetDataTableREx(ref dtDoge, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dtDoge.Rows.Count == 0)
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSAGE].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dtDoge.Rows[0]["Specname"].ToString().Trim();
                        //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSAGE].Text = "";
                        //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = "";
                    }
                    dtDoge.Dispose();
                    dtDoge = null;
                    #endregion //Read_Specman
                }
            }
            #endregion //누락부분 추가 

            if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
            {
                SpdNm.ActiveSheet.Cells[startRow, 2, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(128, 0, 0);
            }
            else
            {
                SpdNm.ActiveSheet.Cells[startRow, 2, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
            }

            if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "★항혈전 " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].ForeColor = Color.FromArgb(255, 0, 255);
            }

            if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "<!> " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
            }

            //'2013-04-24 약제 통합메시지
            clsOrderEtc.ALL_OCS_SUGA_MESSAGE(clsDB.DbCon, "입원", dt.Rows[0]["SUCODE"].ToString().Trim(), SpdNm,
                    (int)BaseOrderInfo.IpdOrderCol.NAMEENG, SpdNm.ActiveSheet.ActiveRowIndex, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text);



            if (clsOrderEtc.READ_SUGA_MR_EXPENSIVE_MEDICINE(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim(), clsOrdFunction.Pat.DeptCode) == "OK")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = "[재고X] " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
            }

            if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
            {
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "80";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "";
            }

            if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
            {
                if (argOrdGbn != "SETORDER")
                {
                    if (clsOrdFunction.GstrSetOldYN != "Y")
                    {
                        //심사기준등록 Display
                        clsOrdFunction.GstrSimCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text;
                        clsOrdFunction.GstrSimFlag = "";//SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SIMSAGIJUN].Text;
                        clsOrdFunction.GstrSimYN = clsOrdFunction.SimSaGiJun_Check(clsDB.DbCon, clsOrdFunction.GstrSimFlag, clsOrdFunction.GstrSimCode, "IPD");
                        if (clsOrdFunction.GstrSimYN == "Y")
                        {
                            using (frmPmpaJSimsaGijun f = new frmPmpaJSimsaGijun(clsOrdFunction.GstrSimCode))
                            {
                                f.ShowDialog();
                            }
                        }
                    }
                }
            }
            //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SIMSAGIJUN].Text = clsOrdFunction.GstrSimYN;

            if (argOrdGbn == "SETORDER")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NGT].Text = dt.Rows[0]["GBGROUP"].ToString().Trim();

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() != "A1" && dt.Rows[0]["SLIPNO"].ToString().Trim() != "A2" &&
                    dt.Rows[0]["SLIPNO"].ToString().Trim() != "A3" && dt.Rows[0]["SLIPNO"].ToString().Trim() != "A4")
                {
                    if (dt.Rows[0]["GBPRN"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = dt.Rows[0]["GBPRN"].ToString().Trim();
                    }

                    if (dt.Rows[0]["REMARK"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "#";
                    }

                    if (dt.Rows[0]["GBTFLAG"].ToString().Trim() == "T")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "T";
                    }

                    if (dt.Rows[0]["GBTFLAG"].ToString().Trim() == "O")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "O";
                    }
                }

                if (dt.Rows[0]["PRN_REMARK"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNMARK].Text = "#";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNREMARK].Text = dt.Rows[0]["PRN_REMARK"].ToString().Trim();
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINUNIT].Text = dt.Rows[0]["PRN_INS_UNIT"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNDOSCODE].Text = dt.Rows[0]["PRN_DOSCODE"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINSCALE].Text = dt.Rows[0]["PRN_INS_GBN"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNTERM].Text = dt.Rows[0]["PRN_TERM"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNNOTITIME].Text = dt.Rows[0]["PRN_NOTIFY"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINSDATE].Text = dt.Rows[0]["PRN_INS_SDATE"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINEDATE].Text = dt.Rows[0]["PRN_INS_EDATE"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINMAX].Text = dt.Rows[0]["PRN_INS_MAX"].ToString().Trim();
            }

            if (argOrdGbn == "VIEWORDER")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NGT].Text = dt.Rows[0]["GBGROUP"].ToString().Trim();

                if (dt.Rows[0]["SLIPNO"].ToString().Trim() != "A1" && dt.Rows[0]["SLIPNO"].ToString().Trim() != "A2" &&
                    dt.Rows[0]["SLIPNO"].ToString().Trim() != "A3" && dt.Rows[0]["SLIPNO"].ToString().Trim() != "A4")
                {
                    if (dt.Rows[0]["GBPRN"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = dt.Rows[0]["GBPRN"].ToString().Trim();
                    }

                    if (dt.Rows[0]["REMARK"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "#";
                    }

                    if (dt.Rows[0]["GBTFLAG"].ToString().Trim() == "T")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "T";
                    }

                    if (dt.Rows[0]["GBTFLAG"].ToString().Trim() == "O")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "O";
                    }
                }

                if (dt.Rows[0]["PRN_REMARK"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNMARK].Text = "#";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNREMARK].Text = dt.Rows[0]["PRN_REMARK"].ToString().Trim();
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINUNIT].Text = dt.Rows[0]["PRN_INS_UNIT"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNDOSCODE].Text = dt.Rows[0]["PRN_DOSCODE"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINSCALE].Text = dt.Rows[0]["PRN_INS_GBN"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNTERM].Text = dt.Rows[0]["PRN_TERM"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PRNNOTITIME].Text = dt.Rows[0]["PRN_NOTIFY"].ToString().Trim();
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINSDATE].Text = dt.Rows[0]["PRN_INS_SDATE"].ToString().Trim();
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINEDATE].Text = dt.Rows[0]["PRN_INS_EDATE"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.INSULINMAX].Text = dt.Rows[0]["PRN_INS_MAX"].ToString().Trim();
            }

            if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
            {
                if (argOrdGbn != "OrdEdit")
                {
                    if (clsOrderEtc.Read_ZoneEm_Suga_Status(clsDB.DbCon, dt.Rows[0]["ORDERCODE"].ToString().Trim(), OF.fn_RTN_KTAS_LEVEL(clsDB.DbCon, clsOrdFunction.Pat.PtNo, clsOrdFunction.Pat.INDATE), clsOrdFunction.Pat.INDATE) == true)
                    {
                        clsBagage.SET_RETURN_CHECKBOX(clsDB.DbCon, SpdNm, (int)BaseOrderInfo.IpdOrderCol.ONEDAYINORD, startRow, "");
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ONEDAYINORD].Locked = false;

                        if (clsOrdFunction.Pat.INDATE == clsOrdFunction.GstrBDate)
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ONEDAYINORD].Text = "1";
                        }
                        else
                        {
                            if (DateTime.Parse(clsOrdFunction.Pat.INDATE).AddDays(1).ToShortDateString() == clsOrdFunction.GstrBDate)
                            {
                                strMsg = "";
                                strMsg += "★응급실 내원일시 : " + clsOrderEtc.READ_ER_INTIME(clsDB.DbCon, clsOrdFunction.Pat.PtNo, clsOrdFunction.Pat.INDATE) + "\r\n\r\n";
                                strMsg += "★처방코드 : " + dt.Rows[0]["ORDERCODE"].ToString().Trim() + "\r\n";
                                strMsg += "해당 처방코드는 '응급실 내원 24시간 이내 시행 시 가산수가'를 받을 수 있습니다." + "\r\n\r\n";
                                strMsg += "응급실 내원 24시간 이내 시행' 하셨을 경우 'YES', " + "\r\n";
                                strMsg += "그렇지 않은 경우 경우 'NO'를 클릭해주시기 바랍니다.";
                                if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ONEDAYINORD].Text = "True";
                                }
                                else
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ONEDAYINORD].Text = "False";
                                }
                            }
                        }
                    }

                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2081" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2082" ||
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2041" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2042")
                    {
                        if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2041" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2042")
                        {
                            strMsg = "";
                            strMsg += "FFP 는 아래의 경우에만 보험처방 가능합니다" + "\r\n";
                            strMsg += "* PT  또는  PTT 가 정상수치의 30% 이하 저하된경우" + "\r\n";
                            strMsg += "* PT결과 INR 1.5 이상인 경우" + "\r\n";
                            strMsg += "* Hypofibrinogenemia(100 이하)인경우" + "\r\n";
                            strMsg += "* 8 pints 까지 인정됨" + "\r\n";
                            strMsg += "그외 처방해야 하는 경우 보험 안됩니다. 비보험 설명. 비보험 처방 해주세요" + "\r\n";
                            strMsg += "(금액: 1PINT 당 5 만원)";
                            MessageBox.Show(strMsg, "수가처방정보", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2081" || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text == "X2082")
                        {
                            strMsg = "";
                            strMsg += "혈소판 농축액은 혈소판 수치가" + "\r\n";
                            strMsg += "5만 이하의 경우 8  pints." + "\r\n";
                            strMsg += "2만 이하의 경우 12 pints 까지 보험인정 됩니다." + "\r\n";
                            strMsg += "(검사수치가 안되는 경우에는 보험 안됨.  비보험설명.비보험처방 해주세요)";
                            MessageBox.Show(strMsg, "수가처방정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Order_Move end
            //////////////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Accept_Dosage_Speciman Start
            //////////////////////////////////////////////////////////////////////////////////////////////////////////

            strSELECTOrderCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text.Trim();
            strSELECTOrderName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text.Trim();
            strSELECTDosName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text.Trim();
            strSELECTDosCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text.Trim();
            strSELECTSlipnos = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text.Trim();
            strSELECTBun = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BUN].Text.Trim();
            strSELECTCBun = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CBUN].Text.Trim();
            strSELECTDiv = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text.Trim();

            if (dt.Rows[0]["GBSELF"].ToString() == "" || dt.Rows[0]["GBSELF"].ToString() == "0")
            {
                if (dt.Rows[0]["SUGBF"].ToString().Trim() == "1" || dt.Rows[0]["SUGBF"].ToString() == "2")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].BackColor = Color.FromArgb(255, 210, 234);
                    if (clsOrdFunction.GstrSelfTest == "True")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "2";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUGBF].Text = dt.Rows[0]["SUGBF"].ToString();
                    }
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].BackColor = Color.FromArgb(255, 255, 234);
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUGBF].Text = "0";

                }
            }
            else
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = dt.Rows[0]["GBSELF"].ToString().Trim();
            }

            #region //외과,흉부외과 가산
            strchkGS = "0";
            strchkGS = clsBagage.CHECK_GS_ADD_GBN(clsDB.DbCon, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text.Trim(), clsOrdFunction.GstrBDate);
            #region //SET_COMBO_GS_ADD
            FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            string[] sComboList = new string[3];
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].CellType = combo;

            switch (strchkGS)
            {
                case "1":
                    sComboList[0] = "0.기타";
                    sComboList[1] = "1.GS";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = "";
                    combo.Items = sComboList;
                    combo.AutoSearch = FarPoint.Win.AutoSearch.MultipleCharacter;
                    combo.MaxDrop = 3;
                    combo.Editable = false;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].CellType = combo;
                    break;
                case "2":
                    sComboList[0] = "0.기타";
                    sComboList[1] = "2.CS";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = "";
                    combo.Items = sComboList;
                    combo.AutoSearch = FarPoint.Win.AutoSearch.MultipleCharacter;
                    combo.MaxDrop = 3;
                    combo.Editable = false;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].CellType = combo;
                    break;
                case "3":
                    sComboList[0] = "0.기타";
                    sComboList[1] = "1.GS"; 
                    sComboList[2] = "2.CS";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = "";
                    combo.Items = sComboList;
                    combo.AutoSearch = FarPoint.Win.AutoSearch.MultipleCharacter;
                    combo.MaxDrop = 3;
                    combo.Editable = false;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].CellType = combo;
                    break;
                default:
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].CellType = txt;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = "";
                    break;
            }

            #endregion //SET_COMBO_GS_ADD
            if ((int)(VB.Val(strchkGS)) >= 1)
            {
                //전산업무 업무의뢰서 2019-543 수가 코드 (O1510) 관련
                //선택하지않고 흉부외과 가산 수가가 적용되도록
                if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim() == "O1510")
                {
                    strchkGS = "2";
                }
                else
                {
                    FrmMedMsgGsAdd f = new FrmMedMsgGsAdd(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text.Trim(), strchkGS);
                    f.ShowDialog();
                    OF.fn_ClearMemory(f);

                    strchkGS = clsPublic.GstrHelpCode;
                }
                
                switch (strchkGS)
                {
                    case "1":
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = "1.GS";
                        break;
                    case "2":
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = "2.CS";
                        break;
                    default:
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = "0.기타";
                        break;
                }
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = clsPublic.GstrHelpCode;
                clsPublic.GstrHelpCode = "";
            }

            #endregion //외과,흉부외과 가산

            if (argOrdGbn == "SETORDER")
            {
                clsOrdFunction.GstrAnatCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text.Trim();
                clsOrdFunction.GstrAnatName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text.Trim();
                clsOrdFunction.GstrSELECTGbImiv = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBIMIV].Text.Trim();

                clsOrdFunction.GstrResultChk = "";

                Form frm = null;
                Form pForm = null;
                if (SpdNm.Parent != null && SpdNm.Parent.Parent != null)
                {
                    pForm = SpdNm.Parent.Parent as Form;
                }

                Screen screen = Screen.FromControl(pForm);

                switch (dt.Rows[0]["CGBIMIV"].ToString().Trim())
                //switch (dt.Rows[0]["GBIMIV"].ToString().Trim())
                {
                    case "4": //기관지
                    case "5": //위
                    case "6": //대장
                    case "7":  //E.R.C.P
                        //SubForm_Close(pForm, "FrmViewEndoRemark");

                        frm = new FrmViewEndoRemark(clsOrdFunction.GstrAnatCode, SpdNm, startRow);
                        (frm as FrmViewEndoRemark).SendEvent += FrmMedIpdOrder_SendEvent;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = new Point(screen.WorkingArea.Right - frm.Width - 80, 220);
                        frm.FormClosed += Frm_FormClosed;
                        frm.Owner = pForm;
                        frm.Show();
                        frm.BringToFront();
                        break;
                    case "8": //Cytology
                        //SubForm_Close(pForm, "FrmViewAnat");

                        frm = new FrmViewAnat(clsOrdFunction.GstrAnatCode, clsOrdFunction.GstrAnatName, "I", true, SpdNm, startRow);
                        (frm as FrmViewAnat).SendEvent += FrmMedIpdOrder_SendEvent;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = new Point(screen.WorkingArea.Right - frm.Width - 80, 220);
                        frm.FormClosed += Frm_FormClosed;
                        frm.Owner = pForm;
                        frm.Show();
                        frm.BringToFront();
                        break;
                    case "9": //Pathology
                        string SuCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text;

                        //SubForm_Close(pForm, "FrmViewAnat2");

                        frm = new FrmViewAnat2(clsOrdFunction.GstrAnatCode, clsOrdFunction.GstrAnatName, "I", SuCode, SpdNm, startRow);
                        (frm as FrmViewAnat2).SendEvent += FrmMedIpdOrder_SendEvent;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = new Point(screen.WorkingArea.Right - frm.Width - 80, 220);
                        frm.FormClosed += Frm_FormClosed;
                        frm.Owner = pForm;
                        frm.Show();
                        frm.BringToFront();
                        break;
                    case "A":
                        //SubForm_Close(pForm, "FrmViewAnat3");

                        strillName = clsOrdFunction.Read_illName(clsOrdFunction.Pat.PtNo, clsOrdFunction.GstrBDate, clsOrdFunction.Pat.DeptCode);

                        frm = new FrmViewAnat3(strillName, "I", SpdNm, startRow);
                        (frm as FrmViewAnat3).SendEvent += FrmMedIpdOrder_SendEvent;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = new Point(screen.WorkingArea.Right - frm.Width - 80, 220);
                        frm.FormClosed += Frm_FormClosed;
                        frm.Owner = pForm;
                        frm.Show();
                        frm.BringToFront();
                        break;
                    default:
                        break;
                }

                //임신주수 입력
                if (clsOrderEtc.READ_PREGNANCY_WEEK_NUMBER_USG_CODE(clsDB.DbCon, strSELECTOrderCode) == true)
                {
                    clsPublic.Gstr임신차수 = "";
                    FrmMedPregnantOrder f = new FrmMedPregnantOrder(strSELECTOrderCode, strSELECTOrderName, startRow);
                    f.ShowDialog();
                    OF.fn_ClearMemory(f);
                }
                //해당 코드 비급비급여 여부표시 로직 추가
                //if (clsOrdFunction.GstrSELECTGbSelf == "1" || clsOrdFunction.GstrSELECTGbSelf == "2")
                //{
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].BackColor = Color.FromArgb(255, 210, 234);
                //}
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = dt.Rows[0]["GBSELF"].ToString();
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUGBF].Text = clsOrdFunction.GstrSELECTGbSelf;

                #region //외과, 흉부외과 가산 항목은 별도로해서 띄운다
                if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CGBINFO].Text == "1")
                {
                    clsOrdFunction.GstrSELECTSuCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim();
                    clsOrdFunction.GstrSELECTGbInfo = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text.Trim();
                    clsOrdFunction.GstrSELECTOrderCode = strSELECTOrderCode;
                    
                    //외과가산 콤보 Setting
                    strchkGS = clsBagage.CHECK_GS_ADD_GBN(clsDB.DbCon, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text, clsOrdFunction.Pat.BDate);
                    clsBagage.SET_COMBO_GS_ADD(SpdNm, (int)BaseOrderInfo.IpdOrderCol.GSADD, startRow, "");

                    if (int.Parse(strchkGS) >= 1)
                    {
                        clsPublic.GstrHelpCode = clsOrdFunction.GstrSELECTOrderCode;
                        clsPublic.GstrHelpName = strchkGS;


                        //전산업무 업무의뢰서 2019-543 수가 코드 (O1510) 관련
                        //선택하지않고 흉부외과 가산 수가가 적용되도록
                        if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim() == "O1510")
                        {
                            strchkGS = "2";
                        }
                        else
                        {
                            FrmMedMsgGsAdd f = new FrmMedMsgGsAdd(clsOrdFunction.GstrSELECTOrderCode, strchkGS);
                            f.ShowDialog();
                            OF.fn_ClearMemory(f);

                            strchkGS = clsPublic.GstrHelpCode;
                        }
                        
                        switch (strchkGS)
                        {
                            case "1":
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = "1.GS";
                                break;
                            case "2":
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = "2.CS";
                                break;
                            default:
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = "0.기타";
                                break;
                        }
                        clsPublic.GstrHelpCode = "";
                        clsPublic.GstrHelpName = "";
                    }

                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text = clsOrdFunction.GstrSELECTSuCode;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text = clsOrdFunction.GstrSELECTGbInfo;


                    //해당 코드 비급비급여 여부표시 로직 추가
                    if (clsOrdFunction.GstrSELECTGbSelf == "1" || clsOrdFunction.GstrSELECTGbSelf == "2")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].BackColor = Color.FromArgb(255, 210, 234);
                        if (clsOrdFunction.GstrSelfTest == "True")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "2";
                        }
                        else
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "";
                        }

                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUGBF].Text = clsOrdFunction.GstrSELECTGbSelf;
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].BackColor = Color.FromArgb(255, 255, 234);
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUGBF].Text = "0";
                    }
                }
                #endregion //외과, 흉부외과 가산 항목은 별도로해서 띄운다
            }
            else
            {
                switch (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBDOSAGE].Text.Trim())
                {
                    case "1":
                        #region //뭐보고 코딩했는지 모르겠음
                        if (clsPublic.GstrJobMan == "간호사")
                        {
                            DataTable dtDos = null;
                            string strDosCode_Nr = "";
                            string strDosName_Nr = "";

                            if (clsPublic.Gstr구두Chk == "OK" && clsPublic.Gstr간호처방STS == "구두처방" &&
                                (dt.Rows[0]["BUN"].ToString().Trim() == "11" || dt.Rows[0]["BUN"].ToString().Trim() == "12" || dt.Rows[0]["BUN"].ToString().Trim() == "20"))
                            {
                                strDosCode_Nr = dt.Rows[0]["DOSCODE"].ToString().Trim();

                                if (dt.Rows[0]["BUN"].ToString().Trim() == "11" || dt.Rows[0]["BUN"].ToString().Trim() == "12")
                                {
                                    strDosCode_Nr = "491001";
                                }
                                else
                                {
                                    //strDosCode_Nr = "491001";
                                    //2018.10.06 고경자 팀장 요청으로 주사 용법 IM/IV... 등으로 불러 오도록 변경
                                    strDosCode_Nr = VB.Left(fn_VerbalDosCode_Read(VB.Left(dt.Rows[0]["DOSCODE"].ToString().Trim(), 2), "09", "04"), 6);
                                    strDosName_Nr = VB.Mid(fn_VerbalDosCode_Read(VB.Left(dt.Rows[0]["DOSCODE"].ToString().Trim(), 2), "09", "04"), 7, fn_VerbalDosCode_Read(VB.Left(dt.Rows[0]["DOSCODE"].ToString().Trim(), 2), "09", "04").Length).Trim();
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = strDosName_Nr;
                                }

                                try
                                {
                                    SQL = "";
                                    SQL += " SELECT DosName,GbDiv                       \r";
                                    SQL += "   FROM KOSMOS_OCS.OCS_ODOSAGE              \r";
                                    SQL += "  WHERE DOSCODE = '" + strDosCode_Nr + "'   \r";
                                    SqlErr = clsDB.GetDataTable(ref dtDos, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        return;
                                    }
                                    if (dtDos.Rows.Count == 0)
                                    {
                                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = "";
                                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
                                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = "";
                                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";
                                    }
                                    else
                                    {
                                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dtDos.Rows[0]["DOSNAME"].ToString().Trim();
                                        if (dtDos.Rows[0]["GBDIV"].ToString().Trim() == "0")
                                        {
                                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
                                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";
                                        }
                                        else
                                        {
                                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = dtDos.Rows[0]["GBDIV"].ToString().Trim();
                                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dtDos.Rows[0]["GBDIV"].ToString().Trim();
                                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = dtDos.Rows[0]["GBDIV"].ToString().Trim();
                                        }
                                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = strDosCode_Nr;
                                    }

                                    dtDos.Dispose();
                                    dtDos = null;

                                    //return;
                                }
                                catch (Exception ex)
                                {
                                    ComFunc.MsgBox(ex.Message);
                                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }
                            }
                        }
                        else if (argOrdGbn == "AutoSearch" && (clsOrdFunction.GEnvSet_Item06 == "" || clsOrdFunction.GEnvSet_Item06 == "2"))
                        {
                            if (dt.Rows[0]["DIV"].ToString() != "0")
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString();
                            }
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = dt.Rows[0]["DIV"].ToString();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = dt.Rows[0]["DIV"].ToString();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                        }
                        else if (argOrdGbn == "VIEWORDER" && (clsOrdFunction.GEnvSet_Item06 == "" || clsOrdFunction.GEnvSet_Item06 == "2"))
                        {
                            if (dt.Rows[0]["DIV"].ToString() != "0")
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString();
                            }
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = dt.Rows[0]["REALQTY"].ToString();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = dt.Rows[0]["QTY"].ToString();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                        }
                        else
                        {
                            if (clsOrdFunction.GEnvSet_Item06 == "1")
                            {
                                clsOrdFunction.GstrLoadFlag = "OrdSlips";
                                FrmDosCode f = new FrmDosCode("Order", strSELECTDosCode, strSELECTBun, "", "I", strSELECTSpecCode, true);
                                f.ShowDialog();
                                OF.fn_ClearMemory(f);
                                clsOrdFunction.GstrLoadFlag = "";
                            }
                        }
                        #endregion //뭐보고 코딩했는지 모르겠음
                        break;
                    case "2":
                        #region //뭐보고 코딩했는지 모르겠음
                        FrmMedViewSpecimen Specimen = new FrmMedViewSpecimen(strSELECTOrderCode, strSELECTSlipnos, strSELECTDosCode, startRow);
                        Specimen.ShowDialog();
                        OF.fn_ClearMemory(Specimen);
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = clsOrdFunction.GstrSpecNm;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text = clsOrdFunction.GstrSpecCd;
                        SpdNm.ActiveSheet.SetActiveCell(startRow + 1, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME);
                        #endregion //뭐보고 코딩했는지 모르겠음
                        break;
                    default:
                        break;
                }

                ////2019-08-27 전산업무의뢰서 2019-08-09
                //if ((int)VB.Val(dt.Rows[0]["BUN"].ToString().Trim()) == 20)
                //{
                //    if (dt.Rows[0]["cNextCode"].ToString().Trim() != "")
                //    {
                //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTSUNIT].Text = dt.Rows[0]["cDispHeader"].ToString();
                //    }
                //}

                if (dt.Rows[0]["SUGBF"].ToString().Trim() == "1" || dt.Rows[0]["SUGBF"].ToString() == "2")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].BackColor = Color.FromArgb(255, 210, 234);
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUGBF].Text = dt.Rows[0]["SUGBF"].ToString();
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "";
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].BackColor = Color.FromArgb(255, 255, 234);
                }

                if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CGBINFO].Text == "1")
                {
                    clsOrdFunction.GstrSELECTSuCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim();
                    clsOrdFunction.GstrSELECTGbInfo = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text.Trim();
                    clsOrdFunction.GstrSELECTOrderCode = strSELECTOrderCode;

                    FrmViewBoth Both = new FrmViewBoth("Order", strSELECTOrderCode, startRow);
                    Both.ShowDialog();
                    OF.fn_ClearMemory(Both);

                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text = clsOrdFunction.GstrSELECTSuCode;

                    //2019-06-07 의뢰서2019-663
                    if (clsOrdFunction.GstrSELECTGbInfo != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text +=  "/" + clsOrdFunction.GstrSELECTGbInfo;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text = clsOrdFunction.GstrSELECTGbInfo;
                        clsOrdFunction.GstrPlusDosName = clsOrdFunction.GstrSELECTGbInfo;
                        clsOrdFunction.GstrSELECTGbInfo = "";
                    }
                    //외과가산 콤보 Setting
                    strchkGS = clsBagage.CHECK_GS_ADD_GBN(clsDB.DbCon, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text, clsOrdFunction.Pat.BDate);
                    clsBagage.SET_COMBO_GS_ADD(SpdNm, (int)BaseOrderInfo.IpdOrderCol.GSADD, startRow, "");

                    if (int.Parse(strchkGS) >= 1)
                    {
                        clsPublic.GstrHelpCode = clsOrdFunction.GstrSELECTOrderCode;
                        clsPublic.GstrHelpName = strchkGS;

                        //전산업무 업무의뢰서 2019-543 수가 코드 (O1510) 관련
                        //선택하지않고 흉부외과 가산 수가가 적용되도록
                        if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim() == "O1510")
                        {
                            strchkGS = "2";
                        }
                        else
                        {
                            FrmMedMsgGsAdd f = new FrmMedMsgGsAdd(clsOrdFunction.GstrSELECTOrderCode, strchkGS);
                            f.ShowDialog();
                            OF.fn_ClearMemory(f);

                            strchkGS = clsPublic.GstrHelpCode;
                        }
                        
                        switch (strchkGS)
                        {
                            case "1":
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = "1.GS";
                                break;
                            case "2":
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = "2.CS";
                                break;
                            default:
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GSADD].Text = "0.기타";
                                break;
                        }
                        clsPublic.GstrHelpCode = "";
                        clsPublic.GstrHelpName = "";
                    }

                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text = clsOrdFunction.GstrSELECTSuCode;

                    //2019-06-07 의뢰서2019-663
                    if (clsOrdFunction.GstrSELECTGbInfo != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME].Text = clsOrdFunction.GstrSELECTGbInfo;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text = clsOrdFunction.GstrSELECTGbInfo;
                        clsOrdFunction.GstrSELECTGbInfo = "";
                    }

                    //해당 코드 비급비급여 여부표시 로직 추가
                    if (clsOrdFunction.GstrSELECTGbSelf == "1" || clsOrdFunction.GstrSELECTGbSelf == "2")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].BackColor = Color.FromArgb(255, 210, 234);
                        if (clsOrdFunction.GstrSelfTest == "True")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "2";
                        }
                        else
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "";
                        }

                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUGBF].Text = clsOrdFunction.GstrSELECTGbSelf;
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].BackColor = Color.FromArgb(255, 255, 234);
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUGBF].Text = "0";
                    }
                }

                SpdNm.ActiveSheet.SetActiveCell(startRow + 1, (int)BaseOrderInfo.IpdOrderCol.PLUSNAME);

                if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.CGBBOTH].Text == "1")  //금액적용
                {
                    strSELECTGbInfo = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text;

                    FrmMedViewAmt f = new FrmMedViewAmt(strSELECTGbInfo);
                    f.ShowDialog();
                    OF.fn_ClearMemory(f);

                    strName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text.Trim();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = strName.PadRight(10) + clsOrdFunction.GstrSELECTGbInfo;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text = clsOrdFunction.GstrSELECTGbInfo;
                }

                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Accept_Dosage_Speciman End
                //////////////////////////////////////////////////////////////////////////////////////////////////////////

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ER].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ER].BackColor = Color.FromArgb(255, 255, 234);
                string SuCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim();

                if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
                {
                    clsOrdFunction.GstrAnatCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text.Trim();
                    clsOrdFunction.GstrAnatName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text.Trim();
                    clsOrdFunction.GstrSELECTGbImiv = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.GBIMIV].Text.Trim();

                    clsOrdFunction.GstrResultChk = "";

                    switch (dt.Rows[0]["CGBIMIV"].ToString().Trim())
                    //switch (dt.Rows[0]["GBIMIV"].ToString().Trim())
                    {
                        case "4":
                            FrmViewEndoRemark f = new FrmViewEndoRemark(clsOrdFunction.GstrAnatCode);  //기관지
                            f.ShowDialog();
                            f.TopMost = true;
                            OF.fn_ClearMemory(f);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.RESULT].Text = clsOrdFunction.GstrResultChk;
                            break;
                        case "5":
                            FrmViewEndoRemark f1 = new FrmViewEndoRemark(clsOrdFunction.GstrAnatCode); //위
                            f1.ShowDialog();
                            f1.TopMost = true;
                            OF.fn_ClearMemory(f1);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.RESULT].Text = clsOrdFunction.GstrResultChk;
                            break;
                        case "6":
                            FrmViewEndoRemark f2 = new FrmViewEndoRemark(clsOrdFunction.GstrAnatCode); //대장
                            f2.ShowDialog();
                            f2.TopMost = true;
                            OF.fn_ClearMemory(f2);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.RESULT].Text = clsOrdFunction.GstrResultChk;
                            break;
                        case "7":
                            FrmViewEndoRemark f3 = new FrmViewEndoRemark(clsOrdFunction.GstrAnatCode); //E.R.C.P
                            f3.ShowDialog();
                            f3.TopMost = true;
                            OF.fn_ClearMemory(f3);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.RESULT].Text = clsOrdFunction.GstrResultChk;
                            break;
                        case "8":
                            FrmViewAnat f4 = new FrmViewAnat(clsOrdFunction.GstrAnatCode, clsOrdFunction.GstrAnatName, "I", true);   //Cytology
                            f4.ShowDialog();
                            f4.TopMost = true;
                            OF.fn_ClearMemory(f4);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.RESULT].Text = clsOrdFunction.GstrResultChk;
                            break;
                        case "9":
                            FrmViewAnat2 f5 = new FrmViewAnat2(clsOrdFunction.GstrAnatCode, clsOrdFunction.GstrAnatName, "I", SuCode); //Pathology
                            f5.ShowDialog();
                            f5.TopMost = true;
                            OF.fn_ClearMemory(f5);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.RESULT].Text = clsOrdFunction.GstrResultChk;
                            break;
                        case "A":
                            strillName = clsOrdFunction.Read_illName(clsOrdFunction.Pat.PtNo, clsOrdFunction.GstrBDate, clsOrdFunction.Pat.DeptCode);
                            FrmViewAnat3 f6 = new FrmViewAnat3(strillName, "I"); //PB smear 소견
                            f6.ShowDialog();
                            f6.TopMost = true;
                            OF.fn_ClearMemory(f6);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.RESULT].Text = clsOrdFunction.GstrResultChk;
                            break;
                        default:
                            break;
                    }


                    //임신주수 입력
                    if (clsOrderEtc.READ_PREGNANCY_WEEK_NUMBER_USG_CODE(clsDB.DbCon, strSELECTOrderCode) == true)
                    {
                        clsPublic.Gstr임신차수 = "";
                        FrmMedPregnantOrder f = new FrmMedPregnantOrder(strSELECTOrderCode, strSELECTOrderName, startRow);
                        f.ShowDialog();
                        OF.fn_ClearMemory(f);
                    }
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ADD].Text = clsOrdFunction.GstrResultChk;
                SpdNm.ActiveSheet.SetActiveCell(startRow + 1, 1);

                if (SpdNm.ActiveSheet.Cells[startRow, 0].BackColor != Color.FromArgb(255, 255, 234))
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 255, 234);
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ER].Text = "";
            }

            if (VB.Left(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text.Trim(), 1) != "A")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim().PadRight(10) + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text;
            }

            if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.BUN].Text.Trim() == "11")
            //if (dt.Rows[0]["BUN"].ToString().Trim() == "11")
            {
                if (clsPublic.Gstr산제Chk != "OK")
                {
                    //if (OF.READ_POWDER(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                    if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                    {
                        if (MessageBox.Show("이 약은 제형상 파우더 조제가 불가능한 약입니다." + "\r\n\r\n" + "이 약만 알약으로 투약하시겠습니까??" + "\r\n" + "아니오(N):  다른 약으로 변경합니다", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            SpdNm.Focus();
                            return;
                        }
                    }
                }
                else
                {
                    if (clsPublic.Gstr산제Chk == "OK")
                    {
                        if (clsPublic.Gstr파우더New_STS == "Y")
                        {
                            clsPublic.Gstr파우더Gubun = "";
                            if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                            {
                                //if (clsOrdFunction.GnReadOrder < SpdNm.ActiveSheet.NonEmptyRowCount)
                                if (clsOrdFunction.GnReadOrder <= startRow)
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].CellType = chk;
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POWDER].Text = "True";

                                    if (clsPublic.Gstr파우더Gubun != "대체오더전송")
                                    {
                                        if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text.Trim() == "")
                                        {
                                            clsPublic.Gstr파우더_SuCode = ";" + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text.Trim() + ";" + startRow + ";72;";
                                        }
                                        else
                                        {
                                            clsPublic.Gstr파우더_SuCode = ";" + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text.Trim() + ";" + startRow + ";72;";
                                        }
                                    }


                                    //산제약이면서 산제불가 체크
                                    string strSuCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim();
                                    if (clsOrderEtc.CHK_POWDER_SUCODE_CHK_2(clsDB.DbCon, strSuCode) == "OK")
                                    {
                                        if (clsPublic.Gstr파우더Gubun != "대체오더전송")
                                        {
                                            FrmMedPowderOrdInfo f = new FrmMedPowderOrdInfo("", strSuCode, "", startRow, 1);
                                            f.StartPosition = FormStartPosition.CenterParent;
                                            f.ShowDialog();
                                            OF.fn_ClearMemory(f);

                                            if (clsPublic.Gstr파우더STS != "산제사유")
                                            {
                                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NGT].Text = "";
                                                if (clsPublic.Gstr파우더Gubun != "대체오더전송")
                                                {
                                                    MessageBox.Show("산제(Powder) 선택 안됨!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                    SpdNm.ActiveSheet.Cells[startRow, 0, startRow, SpdNm.ActiveSheet.ColumnCount - 1].Text = "";
                                                    if (clsPublic.Gstr파우더Gubun == "") return;
                                                }
                                            }
                                            clsPublic.Gstr파우더Gubun = "";
                                        }
                                    }
                                }

                                if (clsOrdFunction.Read_Powder_SuCode_New2(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                                {
                                    if (clsPublic.Gstr파우더STS == "" || clsPublic.Gstr파우더STS == "OK")
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #region //진정관리료
            if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SEDATION].CellType = txt;
                if (string.Compare(clsPublic.GstrSysDate, "2019-01-24") <= 0)
                {
                    if (clsType.User.DeptCode != "PC")
                    {
                        if (clsOrdFunction.Pat.Age < 18)
                        {
                            if (OF.READ_SEDATION(clsDB.DbCon, dt.Rows[0]["ORDERCODE"].ToString().Trim()) == "Y")
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SEDATION].CellType = chk;
                                if (ComFunc.MsgBoxQ(dt.Rows[0]["ORDERCODE"].ToString().Trim() + "처치.검사시 진정목적으로 처방하십니까?", "PSMH", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SEDATION].Text = "True";
                                }
                            }
                        }
                    }
                }
            }
            #endregion //진정관리료

            //if (argOrdGbn == "SETORDER")
            //{
            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.POTABLE].Text = dt.Rows[0]["GBPORT"].ToString().Trim();
            //}

            string strBloodOK = "";
            strBloodOK = clsOrderEtc.CHK_BLOOD_RDate_ORDER_CHK("입원", clsOrdFunction.Pat.PtNo, clsOrdFunction.Pat.DeptCode, SpdNm, startRow,
                                                               (int)BaseOrderInfo.IpdOrderCol.SUCODE, (int)BaseOrderInfo.IpdOrderCol.BUN,
                                                               (int)BaseOrderInfo.IpdOrderCol.REMARK);
            if (clsPublic.Gn혈액사용예정일Row > 0 || strBloodOK != "OK")
            {
                clsPublic.Gstr혈액사용예정일Date = "";
                using (FrmMEdBloodUseDaySet f = new FrmMEdBloodUseDaySet())
                {
                    f.StartPosition = FormStartPosition.CenterScreen;
                    f.ShowDialog();
                }

                SpdNm.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, (int)BaseOrderInfo.IpdOrderCol.REMARK].Text += "[혈액사용예정일:" + clsPublic.Gstr혈액사용예정일Date + "]";
                SpdNm.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "#";
            }

            if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text.Trim() == "S/O" ||
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text.Trim() == "V/S")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].CellType = txt;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Locked = false;
                SpdNm.ActiveSheet.SetActiveCell(startRow, 2);
            }
            #endregion //IPD
        }

        #region 2021-04-24 모달리스 용도로 추가함.

        private void Frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if ((sender as Form).IsDisposed == false)
            {
                (sender as Form).Dispose();
            }
        }
                

        private void FrmMedIpdOrder_SendEvent(Form frm, FpSpread spd, int Row, string Result)
        {
            if (frm != null && frm.IsDisposed == false)
            {
                frm.Dispose();
            }

            spd.ActiveSheet.Cells[Row, (int)BaseOrderInfo.IpdOrderCol.RESULT].Text = Result;
        }
        #endregion

        public void fn_Order_Select_Move_ER(DataTable dt, string argOrdGbn, string argGBIO, FarPoint.Win.Spread.FpSpread SpdNm, int startRow)
        {
            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType chk = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();


            string strUnit = "";
            //string strOrderName = "";

            string strSELECTOrderCode = "";
            string strSELECTOrderName = "";
            string strSELECTDosName = "";
            string strSELECTDosCode = "";
            string strSELECTSlipnos = "";
            string strSELECTBun = "";
            string strSELECTCBun = "";
            string strSELECTDiv = "";
            string strSELECTSpecCode = "";
            //string strSELECTSuCode = "";
            string strSELECTGbInfo = "";

            string strName;

            string strillName = "";
            string strMsg = "";
            string strchkGS = "";
            //string strSuCode2 = "";

            int nCnt;

            if (argOrdGbn != "SETORDER")
            {
                nCnt = GOrdCnt;
            }
            else
            {
                nCnt = 1;
            }

            #region //ER
            if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
            {
                if (argOrdGbn != "SETORDER")
                {
                    clsMedFunction.SimsaMsg_Check(clsDB.DbCon, dt.Rows[0]["ORDERCODE"].ToString().Trim(), clsOrdFunction.Pat.Bi);
                }
            }

            if (nCnt == 1)
            {
                DateTime dtpSys = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();
                if (dt.Rows[0]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A2" ||
                    dt.Rows[0]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[0]["SLIPNO"].ToString().Trim() == "A4")
                {
                    //if (dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B001" || dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B002" ||
                    //    dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B003" || dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B004" ||
                    //    dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B005" || dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B006" ||
                    //    dt.Rows[0]["ORDERCODE"].ToString().Trim() == "B009")
                    //{
                    //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString().Trim();
                    //}
                    //else
                    //{
                    //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = dt.Rows[0]["REMARK"].ToString().Trim();
                    //}
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();

                    if (dt.Rows[0]["CORDERNAME"].ToString().Trim() != "" && dt.Rows[0]["REMARK"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString().Trim();
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = dt.Rows[0]["REMARK"].ToString().Trim();
                    }

                    //2020-11-19 안정수, 추가 
                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "E6541")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POTABLE].Text = "M";
                    }
                }
                else
                {
                    //2020-08-18, 전산의뢰 <2020-2150> 주말 Chest CT 처방시 코드 변경요청건으로 추가
                    // HA464 -> HA464ER, HA434 -> HA434ER
                    // 2021-08-13 00700610,00700611 추가
                    if (clsOrdFunction.GstrCTChange == "Y" && (CF.READ_YOIL(clsDB.DbCon, clsOrdFunction.GstrBDate) == "토요일" || CF.READ_YOIL(clsDB.DbCon, clsOrdFunction.GstrBDate) == "일요일") && 
                        (dt.Rows[0]["ORDERCODE"].ToString().Trim() == "00700600" && (dt.Rows[0]["SUCODE"].ToString().Trim() == "HA464" || dt.Rows[0]["SUCODE"].ToString().Trim() == "HA434"))
                        //&& argOrdGbn != "OrdEdit"
                        )
                    {
                        ComFunc.MsgBox("주말 Chest CT  처방시 외부의뢰 판독으로 처방코드 변경됩니다!");
                        if (dt.Rows[0]["SUCODE"].ToString().Trim() == "HA434")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "00700611";
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "CT Chest(ER-Weekend) Non contrast";
                        }
                        else if (dt.Rows[0]["SUCODE"].ToString().Trim() == "HA464")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "00700610";
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "CT Chest(ER-weekend) Contrast";
                        }
                    }
                    #region 2021-09-08 주석
                    //else if (clsOrdFunction.GstrCTChange.Equals("Y") && (dtpSys.Hour >= 17 || dtpSys.Hour <= 7 || dtpSys.Hour == 8 && dtpSys.Minute <= 30) &&
                    //            ((dt.Rows[0]["ORDERCODE"].ToString().Trim() == "00700600" && (dt.Rows[0]["SUCODE"].ToString().Trim() == "HA464" || dt.Rows[0]["SUCODE"].ToString().Trim() == "HA434") ||
                    //            (dt.Rows[0]["ORDERCODE"].ToString().Trim() == "HA474F" && dt.Rows[0]["SUCODE"].ToString().Trim() == "HA474F") ||
                    //            (dt.Rows[0]["ORDERCODE"].ToString().Trim() == "00705012" && dt.Rows[0]["SUCODE"].ToString().Trim() == "HA474C"))))
                    //{
                    //    ComFunc.MsgBox("평일(17:00 ~ 익일 08:30) Chest CT 처방시 외부의뢰 판독으로 처방코드 변경됩니다!");
                    //    if (dt.Rows[0]["SUCODE"].ToString().Trim() == "HA434")
                    //    {
                    //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "HA434ERN";
                    //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "CT Chest Non conrast(ER-N)";
                    //    }
                    //    else if (dt.Rows[0]["SUCODE"].ToString().Trim() == "HA464")
                    //    {
                    //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "HA464ERN";
                    //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "CT Chest Conrast(ER-N)";
                    //    }
                    //    else if (dt.Rows[0]["SUCODE"].ToString().Trim() == "HA474F")
                    //    {
                    //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "HA474FER";
                    //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "CT Chest PE DVT(ER-N)";
                    //    }
                    //    else if (dt.Rows[0]["SUCODE"].ToString().Trim() == "HA474C")
                    //    {
                    //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "HA474CER";
                    //        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "CT CTA Coronary without Indenol(ER-N)";
                    //    }
                    //}
                    #endregion
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim();

                        if (dt.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                        {
                            strUnit = dt.Rows[0]["CORDERNAME"].ToString();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = strUnit + " " + dt.Rows[0]["CORDERNAMES"].ToString();
                        }
                        else if (dt.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = dt.Rows[0]["CDISPHEADER"].ToString() + " " +
                            dt.Rows[0]["CORDERNAME"].ToString();
                        }
                        else
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = dt.Rows[0]["CORDERNAME"].ToString();
                        }
                    }

                    if (dt.Rows[0]["CGBBOTH"].ToString() == "1")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = ComFunc.LeftH(dt.Rows[0]["CORDERNAME"].ToString(), 30) + " " + dt.Rows[0]["GBINFO"].ToString();
                    }

                    //2020-11-19 안정수, 추가 
                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "E6541")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POTABLE].Text = "M";
                    }
                }

                if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
                {
                    if (argOrdGbn != "OrdEdit")
                    {
                        if (OF.CHECK_OORDER_IN(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim(), clsOrdFunction.Pat.PtNo) == "NO")
                        {
                            SpdNm.ActiveSheet.RemoveRows(startRow, 1);
                            SpdNm.ActiveSheet.RowCount += 1;
                            SpdNm.Focus();
                            return;
                        }
                    }
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                // 기존코드를 변경하는 경우가 있어서 클리어 하는 부분이 필요함
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.CONTENTS].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.BCONTENTS].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text = "";
                //////////////////////////////////////////////////////////////////////////////////////////////////////////

                if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
                {
                    if (argOrdGbn != "OrdEdit")
                    {
                        if (clsOrderEtc.Read_ZoneEm_Suga_Status(clsDB.DbCon, dt.Rows[0]["ORDERCODE"].ToString().Trim(), OF.fn_RTN_KTAS_LEVEL(clsDB.DbCon, clsOrdFunction.Pat.PtNo, clsOrdFunction.Pat.BDate), clsOrdFunction.Pat.BDate) == true)
                        {
                            clsBagage.SET_RETURN_CHECKBOX(clsDB.DbCon, SpdNm, (int)BaseOrderInfo.ErOrderCol.ONEDAYINORD, startRow, "");
                            if (ComFunc.FormatStrToDateTime(clsOrdFunction.Pat.InDate, "D") == clsOrdFunction.GstrBDate)
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ONEDAYINORD].Text = "1";
                            }
                            else
                            {
                                strMsg = "";
                                strMsg += "★응급실 내원일시 : " + clsOrderEtc.READ_ER_INTIME(clsDB.DbCon, clsOrdFunction.Pat.PtNo, ComFunc.FormatStrToDateTime(clsOrdFunction.Pat.InDate, "D")) + "\r\n\r\n";
                                strMsg += "★처방코드 : " + dt.Rows[0]["ORDERCODE"].ToString().Trim() + "\r\n";
                                strMsg += "해당 처방코드는 '응급실 내원 24시간 이내 시행 시 가산수가'를 받을 수 있습니다." + "\r\n\r\n";
                                strMsg += "응급실 내원 24시간 이내 시행' 하셨을 경우 'YES', " + "\r\n";
                                strMsg += "그렇지 않은 경우 경우 'NO'를 클릭해주시기 바랍니다.";
                                if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ONEDAYINORD].Text = "True";
                                }
                                else
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ONEDAYINORD].Text = "False";
                                }
                            }
                        }
                    }
                }

                if (dt.Rows[0]["BUN"].ToString().Trim() != "")
                {
                    if (VB.Left(dt.Rows[0]["SLIPNO"].ToString().Trim(), 1) != "A")
                    {
                        if ((int.Parse(dt.Rows[0]["BUN"].ToString().Trim()) >= 11 && int.Parse(dt.Rows[0]["BUN"].ToString().Trim()) <= 20) ||
                            int.Parse(dt.Rows[0]["BUN"].ToString().Trim()) == 23)
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.CONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.BCONTENTS].Text = dt.Rows[0]["CNEXTCODE"].ToString().Trim();
                        }
                    }
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NGT].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBER].Text = "";

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text = dt.Rows[0]["NAL"].ToString().Trim();

                if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text == "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text = "1";
                }
                if (argOrdGbn == "SETORDER")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = dt.Rows[0]["CGBQTY"].ToString().Trim();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.QTY].Text = dt.Rows[0]["CGBQTY"].ToString().Trim();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString().Trim();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text = dt.Rows[0]["NAL"].ToString().Trim();
                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text = "1";
                    }
                    if (int.Parse(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text) > 1 &&
                        dt.Rows[0]["GBTFLAG"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text = "1";
                        clsOrdFunction.GstrMessage = "1일 이상은 처방을 내릴수 없어" + "\r\n\r\n" +
                                                     "일수를 1일로 조정 하였습니다." + "\r\n\r\n" +
                                                     "다시한번 확인 하십시오.";
                    }
                }
                

                if (clsPublic.Gstr산제Chk != "OK")
                {
                    if (string.Compare(clsPublic.GstrSysDate, "2019-01-01") >= 0)
                    {
                        //if (OF.READ_POWDER(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "Y")
                        if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].CellType = chk;
                        }
                    }
                }
                else
                {
                    if (clsPublic.Gstr파우더New_STS == "Y")
                    {
                        clsPublic.Gstr파우더Gubun = "";
                        if (clsOrdFunction.Read_Powder_SuCode_New(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                        {
                            if (clsOrdFunction.GnReadOrder < SpdNm.ActiveSheet.NonEmptyRowCount)
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].CellType = chk;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POWDER].Text = "True";

                                if (clsOrdFunction.Read_Powder_SuCode_New2(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                                {
                                    if (clsPublic.Gstr파우더New_STS == "" || clsPublic.Gstr파우더STS == "OK")
                                    {
                                        //return;
                                    }

                                }
                            }
                        }
                    }
                }

                #region //진정관리료
                if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SEDATION].CellType = txt;
                    if (string.Compare(clsPublic.GstrSysDate, "2019-01-24") <= 0)
                    {
                        if (clsType.User.DeptCode != "PC")
                        {
                            if (clsOrdFunction.Pat.Age < 18)
                            {
                                if (OF.READ_SEDATION(clsDB.DbCon, dt.Rows[0]["ORDERCODE"].ToString().Trim()) == "Y")
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SEDATION].CellType = chk;
                                    if (ComFunc.MsgBoxQ(dt.Rows[0]["ORDERCODE"].ToString().Trim() + "처치.검사시 진정목적으로 처방하십니까?", "PSMH", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                    {
                                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SEDATION].Text = "True";
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion //진정관리료

                if (clsPublic.Gstr구두수동Chk == "OK")
                {
                    if (clsOrdFunction.GnReadOrder < SpdNm.ActiveSheet.NonEmptyRowCount)
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBVERB1].CellType = chk;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBVERB1].Text = "False";
                    }
                }

                if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "<!> " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                    //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                }

                if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
                {
                    if (clsOrderEtc.READ_SUGA_MR_EXPENSIVE_MEDICINE(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim(), clsOrdFunction.Pat.DeptCode) == "OK")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "[재고X] " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                        //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].BackColor = Color.FromArgb(255, 0, 0);
                    }
                }

                //약제 통합메시지
                clsOrderEtc.ALL_OCS_SUGA_MESSAGE(clsDB.DbCon, "외래", dt.Rows[0]["SUCODE"].ToString().Trim(), SpdNm,
                    (int)BaseOrderInfo.ErOrderCol.NAMEENG, SpdNm.ActiveSheet.ActiveRowIndex, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text);

                if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
                {
                    //골다공증메시지
                    string sMsg = clsOrdFunction.READ_BONE_Result_Check(clsDB.DbCon, clsOrdFunction.Pat.PtNo, dt.Rows[0]["SUCODE"].ToString());

                    if (sMsg != "" && sMsg != null)
                    {
                        FrmMedDocMsgBox f = new FrmMedDocMsgBox(sMsg, "");
                        f.ShowDialog();
                        OF.fn_ClearMemory(f);
                    }
                }

                //부가세
                if (clsOrderEtc.READ_SUGA_VALUE_ADDED_TAX(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBTAX].Text = "True";
                    clsOrdFunction.Gstr부가세 = "OK";
                }

                if (dt.Rows[0]["GBSELF"].ToString().Trim() == "" || dt.Rows[0]["GBSELF"].ToString().Trim() == "0")
                {
                    if (dt.Rows[0]["SUGBF"].ToString().Trim() == "1" || dt.Rows[0]["SUGBF"].ToString() == "2")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SELF].BackColor = Color.FromArgb(255, 210, 234);
                        if (clsOrdFunction.GstrSelfTest == "True")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SELF].Text = "2";
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUGBF].Text = dt.Rows[0]["SUGBF"].ToString();
                        }
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SELF].BackColor = Color.FromArgb(255, 255, 234);
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SELF].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SELF].Text = "0";
                    }
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SELF].Text = dt.Rows[0]["GBSELF"].ToString();
                }


                if (dt.Rows[0]["GBINFO"].ToString() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dt.Rows[0]["GBINFO"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text = dt.Rows[0]["GBINFO"].ToString();
                }
                else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "1")
                {
                    if (dt.Rows[0]["DOSNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = "";
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = "";
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                    }
                }
                else if (dt.Rows[0]["CGBDOSAGE"].ToString() == "2")
                {
                    if (dt.Rows[0]["SPECNAME"].ToString().Trim() == "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = ""; ;
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dt.Rows[0]["SPECNAME"].ToString();
                    }
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = "";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = dt.Rows[0]["GBDIV"].ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = "";
                }

                if (argOrdGbn == "SETORDER")
                {
                    //if (dt.Rows[0]["GBPRN"].ToString().Trim() == "P")
                    //{
                    //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBREMARK].Text = dt.Rows[0]["GBPRN"].ToString().Trim();
                    //}
                    //else if (dt.Rows[0]["GBTFLAG"].ToString().Trim() == "T")
                    //{
                    //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBREMARK].Text = dt.Rows[0]["GBTFLAG"].ToString().Trim();
                    //}
                    //else
                    //{
                    //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBREMARK].Text = dt.Rows[0]["REMARK"].ToString().Trim() == "" ? " " : "#";
                    //}

                    if (!string.IsNullOrEmpty(clsOrdFunction.GstrSetOrderGubun))
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN].Text = clsOrdFunction.GstrSetOrderGubun;
                    }
                    else
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN].Text = VB.Left(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["DOSCODE"].ToString(), 2), dt.Rows[0]["Bun"].ToString()), 7);
                    }
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SORT].Text = clsOrdFunction.GstrSetSort.ToString();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBACT].Text = clsOrdFunction.GstrSetGbAct;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REMARK].Text = dt.Rows[0]["REMARK"].ToString().Trim();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NGT].Text = dt.Rows[0]["GBGROUP"].ToString().Trim();

                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBREMARK].Text = "";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN].Text = VB.Left(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["SpecCode"].ToString(), 2), dt.Rows[0]["Bun"].ToString()), 7);
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SORT].Text = VB.Right(clsOrdFunction.SlipNo_Gubun(dt.Rows[0]["SLIPNO"].ToString(), VB.Left(dt.Rows[0]["SPECCODE"].ToString(), 2), dt.Rows[0]["BUN"].ToString()).Trim(), 3);
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REMARK].Text = "";
                }
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POTABLE].Text = "";

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.MULTI].Text = dt.Rows[0]["MULTI"].ToString();

                //2020-08-18, 전산의뢰 <2020-2150> 주말 Chest CT 처방시 코드 변경요청건으로 추가
                // HA464 -> HA464ER, HA434 -> HA434ER
                // HA474F => HA474FER,  HA474C => HA474CER
                if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "00700611")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = "HA434ER";
                }
                else if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "00700610")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = "HA464ER";
                }
                #region 2021-09-08 주석
                //else if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "HA474FER")
                //{
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = "HA474FER";
                //}
                //else if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "HA474CER")
                //{
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = "HA474CER";
                //}
                //else if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "HA434ERN")
                //{
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = "HA434ERN";
                //}
                //else if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "HA464ERN")
                //{
                //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = "HA464ERN";
                //}
                #endregion
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = dt.Rows[0]["SUCODE"].ToString();
                }

                //2020-11-19 안정수, 추가 
                if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "E6541")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POTABLE].Text = "M";
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.BUN].Text = dt.Rows[0]["BUN"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.CBUN].Text = dt.Rows[0]["CBUN"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SLIPNO].Text = dt.Rows[0]["SLIPNO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.QTY].Text = "1";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBBOTH].Text = "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text = "";

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text = dt.Rows[0]["CDISPRGB"].ToString();
                if (dt.Rows[0]["CDISPRGB"].ToString().Trim() != "")
                {
                    SpdNm.ActiveSheet.Cells[startRow, 1, startRow, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor
                        = ColorTranslator.FromWin32(int.Parse(dt.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                }

                if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[0]["SUCODE"].ToString().Trim()) == "OK")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "★항혈전 " + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].ForeColor = Color.FromArgb(255, 0, 255);
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBBOTH1].Text = dt.Rows[0]["CGBBOTH"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO1].Text = dt.Rows[0]["CGBINFO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBQTY].Text = dt.Rows[0]["CGBQTY"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBDOSAGE].Text = dt.Rows[0]["CGBDOSAGE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NEXTCODE].Text = dt.Rows[0]["CNEXTCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBIMIV].Text = dt.Rows[0]["CGBIMIV"].ToString();
                //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERNO].Text = dt.Rows[0]["ORDERNO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERNO].Text = dt.Rows[0]["ORDERNO"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DRCODE].Text = dt.Rows[0]["DRCODE"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.MULTIREASON].Text = dt.Rows[0]["MULTIREMARK"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DUR].Text = dt.Rows[0]["DUR"].ToString();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBTAX].Text = dt.Rows[0]["GBTAX"].ToString() == "1" ? "True" : "";
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = dt.Rows[0]["REALQTY"].ToString().Trim();
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ASA].Text = dt.Rows[0]["ASA"].ToString();

                //선수납항목 체크
                if (dt.Rows[0]["SUGBN"].ToString() == "1")
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUNSUNAP].Text = "S";
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "(A)" + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUNSUNAP].Text = "";
                }

                strSELECTOrderCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text.Trim();
                strSELECTOrderName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text.Trim();
                strSELECTDosName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text.Trim();
                strSELECTDosCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text.Trim();
                strSELECTSlipnos = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SLIPNO].Text.Trim();
                strSELECTBun = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.BUN].Text.Trim();
                strSELECTCBun = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.CBUN].Text.Trim();
                strSELECTDiv = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text.Trim();
                //strSELECTSpecCode = dt.Rows[0]["SPECCODE"].ToString().Trim();
                                

                if (argOrdGbn == "SETORDER")
                {
                    //임신주수 입력
                    if (clsOrderEtc.READ_PREGNANCY_WEEK_NUMBER_USG_CODE(clsDB.DbCon, strSELECTOrderCode) == true)
                    {
                        clsPublic.Gstr임신차수 = "";
                        FrmMedPregnantOrder f = new FrmMedPregnantOrder(strSELECTOrderCode, strSELECTOrderName, startRow);
                        f.ShowDialog();
                        OF.fn_ClearMemory(f);
                    }

                    if (dt.Rows[0]["DIV"].ToString() != "0")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString();
                    }

                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBDOSAGE].Text.Trim() == "1")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                        //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.CONTENTS].Text = dt.Rows[0]["GBDIV"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.QTY].Text = dt.Rows[0]["GBDIV"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                    }

                    if (dt.Rows[0]["GBINFO"].ToString().Trim() != "")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dt.Rows[0]["GBINFO"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text = dt.Rows[0]["GBINFO"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO1].Text = dt.Rows[0]["CGBINFO"].ToString();
                        //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.CONTENTS].Text = dt.Rows[0]["GBDIV"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.QTY].Text = dt.Rows[0]["GBDIV"].ToString();
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                    }

                    #region //외과, 흉부외과 가산 항목은 별도로해서 띄운다
                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text == "1")
                    {
                        clsOrdFunction.GstrSELECTSuCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text.Trim();
                        clsOrdFunction.GstrSELECTGbInfo = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text.Trim();
                        clsOrdFunction.GstrSELECTOrderCode = strSELECTOrderCode;
                        
                        //외과가산 콤보 Setting
                        strchkGS = clsBagage.CHECK_GS_ADD_GBN(clsDB.DbCon, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text, clsOrdFunction.Pat.BDate);
                        clsBagage.SET_COMBO_GS_ADD(SpdNm, (int)BaseOrderInfo.IpdOrderCol.GSADD, startRow, "");

                        if (int.Parse(strchkGS) >= 1)
                        {
                            clsPublic.GstrHelpCode = clsOrdFunction.GstrSELECTOrderCode;
                            clsPublic.GstrHelpName = strchkGS;

                            //전산업무 업무의뢰서 2019-543 수가 코드 (O1510) 관련
                            //선택하지않고 흉부외과 가산 수가가 적용되도록
                            if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text.Trim() == "O1510")
                            {
                                strchkGS = "2";
                            }
                            else
                            {
                                FrmMedMsgGsAdd f = new FrmMedMsgGsAdd(clsOrdFunction.GstrSELECTOrderCode, strchkGS);
                                f.ShowDialog();
                                OF.fn_ClearMemory(f);

                                strchkGS = clsPublic.GstrHelpCode;
                            }
                            
                            switch (strchkGS)
                            {
                                case "1":
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = "1.GS";
                                    break;
                                case "2":
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = "2.CS";
                                    break;
                                default:
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = "0.기타";
                                    break;
                            }
                            clsPublic.GstrHelpCode = "";
                            clsPublic.GstrHelpName = "";
                        }

                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = clsOrdFunction.GstrSELECTSuCode;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text = clsOrdFunction.GstrSELECTGbInfo;
                        
                    }
                    #endregion //외과, 흉부외과 가산 항목은 별도로해서 띄운다

                    //2020-08-18, 전산의뢰 <2020-2150> 주말 Chest CT 처방시 코드 변경요청건으로 추가
                    // HA464 -> HA464ER, HA434 -> HA434ER
                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "00700610")
                    {                        
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = "HA464ER";
                    }
                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "00700611")
                    {                        
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = "HA434ER";
                    }

                    //2020-11-19 안정수, 추가 
                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "E6541")
                    {
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POTABLE].Text = "M";
                    }
                }
                else
                {
                    switch (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBDOSAGE].Text.Trim())
                    {
                        case "1":
                            if (argOrdGbn == "AutoSearch" && (clsOrdFunction.GEnvSet_Item06 == "" || clsOrdFunction.GEnvSet_Item06 == "2"))
                            {
                                if (dt.Rows[0]["DIV"].ToString() != "0")
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString();
                                }
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = dt.Rows[0]["DIV"].ToString();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.QTY].Text = dt.Rows[0]["DIV"].ToString();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                            }
                            else if (argOrdGbn == "VIEWORDER" && (clsOrdFunction.GEnvSet_Item06 == "" || clsOrdFunction.GEnvSet_Item06 == "2"))
                            {
                                if (dt.Rows[0]["DIV"].ToString() != "0")
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = dt.Rows[0]["DIV"].ToString();
                                }
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dt.Rows[0]["DOSNAME"].ToString();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = dt.Rows[0]["REALQTY"].ToString();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.QTY].Text = dt.Rows[0]["QTY"].ToString();
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = dt.Rows[0]["DOSCODE"].ToString();
                            }
                            else
                            {
                                if (clsOrdFunction.GEnvSet_Item06 == "1")
                                {
                                    clsOrdFunction.GstrLoadFlag = "OrdSlips";
                                    FrmDosCode f = new FrmDosCode("Order", strSELECTDosCode, strSELECTBun, "", "I", strSELECTSpecCode, true);
                                    f.ShowDialog();
                                    OF.fn_ClearMemory(f);
                                    clsOrdFunction.GstrLoadFlag = "";
                                }
                            }
                            break;
                        case "2":
                            FrmMedViewSpecimen Specimen = new FrmMedViewSpecimen(strSELECTOrderCode, strSELECTSlipnos, strSELECTDosCode, startRow);
                            Specimen.ShowDialog();
                            OF.fn_ClearMemory(Specimen);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = clsOrdFunction.GstrSpecNm;
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = clsOrdFunction.GstrSpecCd;
                            SpdNm.ActiveSheet.SetActiveCell(startRow + 1, (int)BaseOrderInfo.OpdOrderCol.PLUSNAME);
                            break;
                        default:
                            break;
                    }

                    //2018.10.29 
                    //ER 용법 체크 관련 용법코드 
                    if (dt.Rows[0]["CGBDOSAGE"].ToString().Trim() == "1")
                    {
                        DataTable dtDos = null;
                        string strDosCode_ER = "";
                        string strBun_ER = "";

                        strDosCode_ER = dt.Rows[0]["SpecCode"].ToString().Trim();
                        strBun_ER = dt.Rows[0]["BUN"].ToString().Trim();

                        if (VB.Left(dt.Rows[0]["SpecCode"].ToString().Trim(), 1) == "9" && VB.Right(dt.Rows[0]["SpecCode"].ToString().Trim(), 1) == "1")
                        {
                            strDosCode_ER = VB.Left(dt.Rows[0]["SpecCode"].ToString().Trim(), 5) + "4";
                        }

                        if (clsPublic.Gstr구두Chk == "OK" && clsPublic.Gstr간호처방STS == "구두처방" &&
                            (strBun_ER == "11" || strBun_ER == "12" || strBun_ER == "20"))
                        {
                            if (strBun_ER == "11" || strBun_ER == "12")
                            {
                                strDosCode_ER = "491001";
                            }
                            else
                            {
                                strDosCode_ER = "491001";
                            }
                        }

                        try
                        {
                            if (clsOrdFunction.GstrDosER == "Y")
                            {
                                if (strBun_ER == "11")
                                {
                                    strDosCode_ER = "491001";
                                }
                                else if (strBun_ER == "20")
                                {
                                    if (dt.Rows[0]["ORDERCODE"].ToString().Trim() == "TIRA" || dt.Rows[0]["ORDERCODE"].ToString().Trim() == "NSC" || dt.Rows[0]["ORDERCODE"].ToString().Trim() == "NSD" 
                                        || dt.Rows[0]["ORDERCODE"].ToString().Trim() == "NS250" || dt.Rows[0]["ORDERCODE"].ToString().Trim() == "NSB")
                                    {
                                        strDosCode_ER = "920904"; //IV로
                                    }
                                    else
                                    {
                                        strDosCode_ER = VB.Left(strDosCode_ER, 2) + "0904";
                                    }
                                }
                            }

                            SQL = "";
                            SQL += " SELECT DosName,GbDiv                       \r";
                            SQL += "   FROM KOSMOS_OCS.OCS_ODOSAGE              \r";
                            SQL += "  WHERE DOSCODE = '" + strDosCode_ER + "'   \r";
                            SqlErr = clsDB.GetDataTable(ref dtDos, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if (dtDos.Rows.Count == 0)
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = "";
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = "";
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = "";
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = "";
                            }
                            else
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dtDos.Rows[0]["DOSNAME"].ToString().Trim();
                                if (dtDos.Rows[0]["GBDIV"].ToString().Trim() == "0")
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = "";
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = "";
                                }
                                else
                                {
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = dtDos.Rows[0]["GBDIV"].ToString().Trim();
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = dtDos.Rows[0]["GBDIV"].ToString().Trim();
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.QTY].Text = dtDos.Rows[0]["GBDIV"].ToString().Trim();
                                }
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = strDosCode_ER;
                            }

                            dtDos.Dispose();
                            dtDos = null;

                            return;
                        }
                        catch (Exception ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                    }
                    else if (dt.Rows[0]["CGBDOSAGE"].ToString().Trim() == "2")
                    {
                        //위에서 무조건 선택화면이 뜸으로 주석처리함
                        //DataTable dtDos = null;
                        //SQL = " SELECT Specname FROM KOSMOS_OCS.OCS_OSPECIMAN ";
                        //SQL = SQL + ComNum.VBLF + "WHERE SpecCode = '" + dt.Rows[0]["DOSCODE"].ToString().Trim() + "' ";
                        //SQL = SQL + ComNum.VBLF + "  AND Slipno   = '" + dt.Rows[0]["SLIPNO"].ToString().Trim() + "'   ";
                        //SqlErr = clsDB.GetDataTable(ref dtDos, SQL, clsDB.DbCon);

                        //if (SqlErr != "")
                        //{
                        //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        //    return;
                        //}

                        //if (dtDos.Rows.Count == 0)
                        //{
                        //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = "";
                        //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = "";
                        //}
                        //else
                        //{
                        //    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = dtDos.Rows[0]["Specname"].ToString().Trim();
                        //}
                        //dtDos.Dispose();
                        //dtDos = null;
                    }

                    if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO1].Text == "1")
                    {
                        clsOrdFunction.GstrSELECTSuCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text.Trim();
                        clsOrdFunction.GstrSELECTGbInfo = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO1].Text.Trim();
                        clsOrdFunction.GstrSELECTOrderCode = dt.Rows[0]["ORDERCODE"].ToString().Trim();

                        FrmViewBoth Both = new FrmViewBoth("Order", clsOrdFunction.GstrSELECTOrderCode, startRow);
                        Both.ShowDialog();
                        OF.fn_ClearMemory(Both);

                        if ((clsOrdFunction.GstrSELECTSuCode == "HA434" || clsOrdFunction.GstrSELECTSuCode == "HA464")
                            && SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "00700600"
                            && (CF.READ_YOIL(clsDB.DbCon, clsOrdFunction.GstrBDate) == "토요일" || CF.READ_YOIL(clsDB.DbCon, clsOrdFunction.GstrBDate) == "일요일")
                            && clsOrdFunction.GstrCTChange == "Y")
                        {
                            ComFunc.MsgBox("주말 Chest CT  처방시 외부의뢰 판독으로 처방코드 변경됩니다!");

                            if (clsOrdFunction.GstrSELECTSuCode == "HA434")
                            {
                                clsOrdFunction.GstrSELECTSuCode = "HA434ER";
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "00700611";
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "CT Chest(ER-Weekend) Non conrast";
                            }
                            else if (clsOrdFunction.GstrSELECTSuCode == "HA464")
                            {
                                clsOrdFunction.GstrSELECTSuCode = "HA464ER";
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "00700610";
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = "CT Chest(ER-weekend) Conrast";
                            }
                        }

                        //2020-11-19 안정수, 추가 
                        if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "E6541")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POTABLE].Text = "M";
                        }

                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = clsOrdFunction.GstrSELECTSuCode;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = clsOrdFunction.GstrSELECTGbInfo;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text = clsOrdFunction.GstrSELECTGbInfo;

                        //외과가산 콤보 Setting
                        strchkGS = clsBagage.CHECK_GS_ADD_GBN(clsDB.DbCon, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text, clsOrdFunction.Pat.BDate);
                        clsBagage.SET_COMBO_GS_ADD(SpdNm, (int)BaseOrderInfo.ErOrderCol.GSADD, startRow, "");

                        if (int.Parse(strchkGS) >= 1)
                        {
                            clsPublic.GstrHelpCode = clsOrdFunction.GstrSELECTOrderCode;
                            clsPublic.GstrHelpName = strchkGS;

                            //전산업무 업무의뢰서 2019-543 수가 코드 (O1510) 관련
                            //선택하지않고 흉부외과 가산 수가가 적용되도록
                            if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text.Trim() == "O1510")
                            {
                                strchkGS = "2";
                            }
                            else
                            {
                                FrmMedMsgGsAdd f = new FrmMedMsgGsAdd(clsOrdFunction.GstrSELECTOrderCode, strchkGS);
                                f.ShowDialog();
                                OF.fn_ClearMemory(f);

                                strchkGS = clsPublic.GstrHelpCode;
                            }
                            
                            switch (strchkGS)
                            {
                                case "1":
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = "1.GS";
                                    break;
                                case "2":
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = "2.CS";
                                    break;
                                default:
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = "0.기타";
                                    break;
                            }
                            clsPublic.GstrHelpCode = "";
                            clsPublic.GstrHelpName = "";
                        }

                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = clsOrdFunction.GstrSELECTSuCode;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.PLUSNAME].Text = clsOrdFunction.GstrSELECTGbInfo;
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text = clsOrdFunction.GstrSELECTGbInfo;

                        //해당 코드 비급비급여 여부표시 로직 추가
                        if (clsOrdFunction.GstrSELECTGbSelf == "1" || clsOrdFunction.GstrSELECTGbSelf == "2")
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SELF].BackColor = Color.FromArgb(255, 210, 234);
                            if (clsOrdFunction.GstrSelfTest == "True")
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SELF].Text = "2";
                            }
                            else
                            {
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SELF].Text = "";
                            }

                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUGBF].Text = clsOrdFunction.GstrSELECTGbSelf;
                        }
                        else
                        {
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SELF].BackColor = Color.FromArgb(255, 255, 234);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SELF].Text = "";
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUGBF].Text = "0";
                        }

                        SpdNm.ActiveSheet.SetActiveCell(startRow + 1, (int)BaseOrderInfo.ErOrderCol.PLUSNAME);
                    }
                }

                if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
                {
                    if (argOrdGbn != "OrdEdit")
                    {
                        #region //외과,흉부외과 가산
                        strchkGS = "0";
                        strchkGS = clsBagage.CHECK_GS_ADD_GBN(clsDB.DbCon, SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text.Trim(), clsOrdFunction.Pat.BDate);
                        #region //SET_COMBO_GS_ADD
                        SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].CellType = combo;
                        string[] sComboList = new string[3];

                        switch (strchkGS)
                        {
                            case "1":
                                sComboList[0] = "0.기타";
                                sComboList[1] = "1.GS";
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = "";
                                combo.Items = sComboList;
                                combo.AutoSearch = FarPoint.Win.AutoSearch.MultipleCharacter;
                                combo.MaxDrop = 3;
                                combo.Editable = false;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].CellType = combo;
                                break;
                            case "2":
                                sComboList[0] = "0.기타";
                                sComboList[1] = "2.CS";
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = "";
                                combo.Items = sComboList;
                                combo.AutoSearch = FarPoint.Win.AutoSearch.MultipleCharacter;
                                combo.MaxDrop = 3;
                                combo.Editable = false;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].CellType = combo;
                                break;
                            case "3":
                                sComboList[0] = "0.기타";
                                sComboList[2] = "1.GS";
                                sComboList[3] = "2.CS";
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = "";
                                combo.Items = sComboList;
                                combo.AutoSearch = FarPoint.Win.AutoSearch.MultipleCharacter;
                                combo.MaxDrop = 3;
                                combo.Editable = false;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].CellType = combo;
                                break;
                            default:
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].CellType = txt;
                                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = "";
                                break;
                        }
                        #endregion //SET_COMBO_GS_ADD
                        if ((int)(VB.Val(strchkGS)) >= 1)
                        {
                            //전산업무 업무의뢰서 2019-543 수가 코드 (O1510) 관련
                            //선택하지않고 흉부외과 가산 수가가 적용되도록
                            if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text.Trim() == "O1510")
                            {
                                strchkGS = "2";
                            }
                            else
                            {
                                FrmMedMsgGsAdd f = new FrmMedMsgGsAdd(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text.Trim(), strchkGS);
                                f.ShowDialog();
                                OF.fn_ClearMemory(f);

                                strchkGS = clsPublic.GstrHelpCode;
                            }
                            
                            switch (strchkGS)
                            {
                                case "1":
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = "1.GS";
                                    break;
                                case "2":
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = "2.CS";
                                    break;
                                default:
                                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = "0.기타";
                                    break;
                            }
                            //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GSADD].Text = clsPublic.GstrHelpCode;
                            clsPublic.GstrHelpCode = "";
                        }
                        #endregion //외과,흉부외과 가산
                    }
                }

                if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBBOTH1].Text == "1")  //금액적용
                {
                    strSELECTGbInfo = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text;

                    FrmMedViewAmt f = new FrmMedViewAmt(strSELECTGbInfo);
                    f.ShowDialog();
                    OF.fn_ClearMemory(f);

                    strName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text.Trim();
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = strName.PadRight(10) + clsOrdFunction.GstrSELECTGbInfo;
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text = clsOrdFunction.GstrSELECTGbInfo;
                }

                if (clsOrdFunction.GstrSetRegYN != "Y") //약속처방 작성이 아니면
                {
                    //임신주수 입력
                    if (clsOrderEtc.READ_PREGNANCY_WEEK_NUMBER_USG_CODE(clsDB.DbCon, strSELECTOrderCode) == true)
                    {
                        clsPublic.Gstr임신차수 = "";
                        FrmMedPregnantOrder f = new FrmMedPregnantOrder(strSELECTOrderCode, strSELECTOrderName, startRow);
                        f.ShowDialog();
                        OF.fn_ClearMemory(f);
                    }
                    clsOrderEtc.CHECK_F0913(strSELECTOrderCode, clsOrdFunction.Pat.DeptCode, clsOrdFunction.Pat.Sex, clsOrdFunction.Pat.Age);

                    //심사기준등록 Display
                    clsOrdFunction.GstrSimCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text;
                    clsOrdFunction.GstrSimFlag = ""; //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SIMSAGIJUN].Text;
                                                     //clsOrdFunction.GstrSimYN = clsOrdFunction.SimSaGiJun_Check(clsDB.DbCon, clsOrdFunction.GstrSimFlag, clsOrdFunction.GstrSimCode);
                                                     //if (clsOrdFunction.GstrSimYN == "Y")
                                                     //{
                                                     //    if (argOrdGbn != "SETORDER")
                                                     //    {
                                                     //        //frmPmpaJSimsaGijun f = new frmPmpaJSimsaGijun(clsOrdFunction.GstrSimCode);
                                                     //        //f.ShowDialog();
                                                     //        //OF.fn_ClearMemory(f);
                                                     //    }
                                                     //}
                                                     //SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.MULTIREASON].Text = clsOrdFunction.GstrSimYN;

                    clsOrdFunction.GstrAnatCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text.Trim();
                    clsOrdFunction.GstrAnatName = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text.Trim();
                    clsOrdFunction.GstrSELECTGbImiv = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBIMIV].Text.Trim();

                    clsOrdFunction.GstrResultChk = "";
                    string SuCode = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text.Trim();

                    switch (clsOrdFunction.GstrSELECTGbImiv)
                    {
                        case "4":
                            FrmViewEndoRemark f = new FrmViewEndoRemark(clsOrdFunction.GstrAnatCode);  //기관지
                            f.ShowDialog();
                            OF.fn_ClearMemory(f);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBOPINION].Text = clsOrdFunction.GstrResultChk;
                            break;
                        case "5":
                            FrmViewEndoRemark f1 = new FrmViewEndoRemark(clsOrdFunction.GstrAnatCode); //위
                            f1.ShowDialog();
                            OF.fn_ClearMemory(f1);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBOPINION].Text = clsOrdFunction.GstrResultChk;
                            break;
                        case "6":
                            FrmViewEndoRemark f2 = new FrmViewEndoRemark(clsOrdFunction.GstrAnatCode); //대장
                            f2.ShowDialog();
                            OF.fn_ClearMemory(f2);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBOPINION].Text = clsOrdFunction.GstrResultChk;
                            break;
                        case "7":
                            FrmViewEndoRemark f3 = new FrmViewEndoRemark(clsOrdFunction.GstrAnatCode); //E.R.C.P
                            f3.ShowDialog();
                            OF.fn_ClearMemory(f3);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBOPINION].Text = clsOrdFunction.GstrResultChk;
                            break;
                        case "8":
                            FrmViewAnat f4 = new FrmViewAnat(clsOrdFunction.GstrAnatCode, clsOrdFunction.GstrAnatName, "I", true);   //Cytology
                            f4.ShowDialog();
                            OF.fn_ClearMemory(f4);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBOPINION].Text = clsOrdFunction.GstrResultChk;
                            break;
                        case "9":
                            FrmViewAnat2 f5 = new FrmViewAnat2(clsOrdFunction.GstrAnatCode, clsOrdFunction.GstrAnatName, "I", SuCode); //Pathology
                            f5.ShowDialog();
                            OF.fn_ClearMemory(f5);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBOPINION].Text = clsOrdFunction.GstrResultChk;
                            break;
                        case "A":
                            strillName = clsOrdFunction.Read_illName(clsOrdFunction.Pat.PtNo, clsOrdFunction.GstrBDate, clsOrdFunction.Pat.DeptCode);
                            FrmViewAnat3 f6 = new FrmViewAnat3(strillName, "I"); //PB smear 소견
                            f6.ShowDialog();
                            OF.fn_ClearMemory(f6);
                            SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBOPINION].Text = clsOrdFunction.GstrResultChk;
                            break;
                        default:
                            break;
                    }
                }

                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.GBOPINION].Text = clsOrdFunction.GstrResultChk;
                SpdNm.ActiveSheet.SetActiveCell(startRow + 1, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN);

            }

            if (VB.Left(SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text.Trim(), 1) != "A")
            {
                //2020-08-18, 전산의뢰 <2020-2150> 주말 Chest CT 처방시 코드 변경요청건으로 추가
                // HA464 -> HA464ER, HA434 -> HA434ER
                if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "00700610"
                    || SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "00700611"
                    //|| SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "HA464ERN"
                    //|| SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "HA434ERN"
                    //|| SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "HA474FER"
                    //|| SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "HA474CER"
                    )
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text.PadRight(10) + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                }
                else
                {
                    SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = dt.Rows[0]["ORDERCODE"].ToString().Trim().PadRight(10) + SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text;
                }                
            }

            //2020-11-19 안정수, 추가 
            if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text == "E6541")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.POTABLE].Text = "M";
            }

            string strBloodOK = "";
            strBloodOK = clsOrderEtc.CHK_BLOOD_RDate_ORDER_CHK("응급", clsOrdFunction.Pat.PtNo, clsOrdFunction.Pat.DeptCode, SpdNm, startRow,
                                                               (int)BaseOrderInfo.ErOrderCol.SUCODE, (int)BaseOrderInfo.ErOrderCol.BUN,
                                                               (int)BaseOrderInfo.ErOrderCol.REMARK);
            if (clsPublic.Gn혈액사용예정일Row > 0 || strBloodOK != "OK")
            {
                clsPublic.Gstr혈액사용예정일Date = "";
                using (FrmMEdBloodUseDaySet f = new FrmMEdBloodUseDaySet())
                {
                    f.StartPosition = FormStartPosition.CenterScreen;
                    f.ShowDialog();
                }

                SpdNm.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, (int)BaseOrderInfo.ErOrderCol.REMARK].Text += "[혈액사용예정일:" + clsPublic.Gstr혈액사용예정일Date + "]";
                SpdNm.ActiveSheet.Cells[clsPublic.Gn혈액사용예정일Row, (int)BaseOrderInfo.ErOrderCol.GBREMARK].Text = "#";
            }

            if (SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN].Text.Trim() == "S/O" ||
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN].Text.Trim() == "V/S")
            {
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].CellType = txt;
                SpdNm.ActiveSheet.Cells[startRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Locked = false;
                SpdNm.ActiveSheet.SetActiveCell(startRow, 2);
            }
            #endregion //ER
        }

        public void fn_Order_Select_Move(DataTable dt, string argOrdGbn, string argGBIO, FarPoint.Win.Spread.FpSpread SpdNm, int startRow)
        {
            if (argGBIO == "OPD")
            {
                fn_Order_Select_Move_OPD(dt, argOrdGbn, argGBIO, SpdNm, startRow);
            }
            else if (argGBIO == "IPD")
            {
                fn_Order_Select_Move_IPD(dt, argOrdGbn, argGBIO, SpdNm, startRow);
            }
            else if (argGBIO == "ER")
            {
                fn_Order_Select_Move_ER(dt, argOrdGbn, argGBIO, SpdNm, startRow);
            }
        }

        public string fn_VerbalDosCode_Read(string strFirstDos, string strSecondDos, string strLastDos)
        {
            OracleDataReader reader = null;
            string rtnVal = string.Empty;

            try
            {   
                SQL = "";
                SQL += " SELECT DOSCODE || '  ' || DOSNAME DOSCODE              \r";
                SQL += "   FROM KOSMOS_OCS.OCS_ODOSAGE                          \r";
                SQL += "  WHERE SUBSTR(DOSCODE, 1, 2) = '" + strFirstDos + "'   \r";
                SQL += "    AND SUBSTR(DOSCODE, 3, 2) = '" + strSecondDos + "'  \r";
                SQL += "    AND SUBSTR(DOSCODE, 5, 2) = '" + strLastDos + "'    \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (reader.HasRows && reader.Read())
                {
                    //rtnVal = dtVerbDos.Rows[0]["DOSCODE"].ToString().Trim();
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                //dtVerbDos.Dispose();
                //dtVerbDos = null;

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public string fn_ErDos_Check(string sDosCode, string strBun)
        {
            //DataTable dtDos = null;
            string rtnVal = "";

            string strDosCode = sDosCode;

            if (VB.Left(strDosCode, 1) == "9" && VB.Right(strDosCode, 1) == "1")
            {
                strDosCode = VB.Left(strDosCode, 5) + "4";
            }

            if (clsPublic.Gstr구두Chk == "OK" && clsPublic.Gstr간호처방STS == "구두처방" && 
                (strBun == "11" || strBun == "12" || strBun == "20"))
            {
                if (strBun == "11" || strBun == "12")
                {
                    strDosCode = "491001";
                }
                else
                {
                    strDosCode = "491001";
                }
            }

            try
            {
                if (clsOrdFunction.GstrDosER == "Y")
                {
                    if (strBun == "11")
                    {
                        strDosCode = "491001";
                    }
                    else if (strBun == "20")
                    {
                        strDosCode = VB.Left(strDosCode, 2) + "0904";
                    }
                }

                #region 미사용 주석
                //SQL = "";
                //SQL += " SELECT DosName,GbDiv                       \r";
                //SQL += "   FROM KOSMOS_OCS.OCS_ODOSAGE              \r";
                //SQL += "  WHERE DOSCODE = '" + strDosCode + "'      \r";                
                //SqlErr = clsDB.GetDataTable(ref dtDos, SQL, clsDB.DbCon);

                //if (SqlErr != "")
                //{
                //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    return rtnVal;
                //}
                //if (dtDos.Rows.Count == 0)
                //{

                //}
                //else
                //{

                //}

                //dtDos.Dispose();
                //dtDos = null;
                #endregion

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
            

            //rtnVal = strDosCode;

            return rtnVal;
        }

        /// <summary>
        /// 진단명 읽어오기
        /// </summary>
        /// <param name="sillCode"></param>
        /// <returns></returns>
        public static string Read_DiagName(string sillCode)
        {
            string rtnVal = "";

            OracleDataReader reader = null;

            try
            {
                SQL = "";
                SQL += " SELECT NVL(ILLNAMEK, ILLNAMEE) ILLNAME     \r";
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_ILLS      \r";
                SQL += "  WHERE IllCode = '" + sillCode + "'        \r";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (reader.HasRows && reader.Read())
                {
                    //rtnVal = dt1.Rows[0]["ILLNAME"].ToString().Trim();
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                reader = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public void fn_Read_SetOrder(FarPoint.Win.Spread.FpSpread SpdNm, FarPoint.Win.Spread.FpSpread SpdNm2, string strSetName, string strDeptDr, int sGbOrder)
        {
            int nillCount = 0;
            int nBoowi1Count = 0;
            int nBoowi2Count = 0;
            int nBoowi3Count = 0;
            int nBoowi4Count = 0;

            string strUnit = "";

            string[] strIllCodes = new string[10];
            string[] strillName = new string[10];
            string[] strBoowi1s = new string[10];
            string[] strBoowi2s = new string[10];
            string[] strBoowi3s = new string[10];
            string[] strBoowi4s = new string[10];

            try
            {
                SQL = "";
                SQL += " SELECT a.ORDERCODE, B.ORDERNAME || '  ' || B.ORDERNAMES ORDERNAME1         \r";
                SQL += "      , case when a.bun < '30' then KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(a.DOSCODE) \r";
                SQL += "             else KOSMOS_OCS.FC_OCS_OSPECIMAN_NAME(a.DOSCODE, a.SLIPNO) end DOSNAME      \r";
                SQL += "      , a.CONTENTS, a.QTY, a.GBDIV, a.NAL, a.GBSELF, a.GBER, a.GBPORT       \r";
                SQL += "      , a.GBGROUP, a.REMARK, a.SUCODE, b.GBINPUT, a.PRN_REMARK              \r";
                SQL += "      , a.PRN_INS_GBN, a.PRN_INS_UNIT, a.PRN_INS_SDATE, a.PRN_INS_EDATE     \r";
                SQL += "      , a.PRN_INS_MAX, a.PRN_DOSCODE, a.PRN_TERM, a.PRN_NOTIFY, a.PRN_UNIT  \r";
                SQL += "      , a.SUBUL_WARD, a.ROWID RID                                           \r";
                SQL += "      , nvl(a.ILLCODES_KCD6, a.ILLCODES) ILLCODES                           \r";
                SQL += "      , a.BOOWI1,  a.BOOWI2, a.BOOWI3, a.BOOWI4, a.GBINFO , A.SLIPNO        \r";
                SQL += "      , b.SENDDEPT, b.DISPHEADER, B.GBBOTH, B.ORDERNAME, B.ORDERNAMES       \r";
                //2021-01-11 안정수 투여시점, 투여시간 추가
                SQL += "      , a.TUYEOPOINT, a.TUYEOTIME                                           \r";

                //2021-11-05 
                SQL += "      , CASE WHEN A.GBINFO IS NOT NULL AND EXISTS                                       \r";
                SQL += "      	(                                                                               \r";
                SQL += "      		SELECT 1                                                                    \r";
                SQL += "      		  FROM KOSMOS_OCS.OCS_SUBCODE SUB                                           \r";
                SQL += "      		 WHERE SUB.ORDERCODE = A.ORDERCODE                                          \r";
                SQL += "      		   AND SUB.SUCODE    = A.SUCODE                                                \r";
                SQL += "      		   AND SUB.SUBNAME   = A.GBINFO                                               \r";
                SQL += "      		   AND SUB.DELDATE IS NOT NULL                                              \r";
                SQL += "      	) THEN '1'                                                                      \r";
                SQL += "      	END IS_SUBCODE_ERROR                                                            \r";

                //2021-11-05 
                SQL += "      , CASE WHEN EXISTS                                                                \r";
                SQL += "      	(                                                                               \r";
                SQL += "      		SELECT 1                                                                    \r";
                SQL += "      		  FROM KOSMOS_PMPA.BAS_SUT SUB                                              \r";
                SQL += "      		 WHERE SUB.SUCODE = A.SUCODE                                                \r";
                SQL += "      		   AND SUB.DELDATE <= TRUNC(SYSDATE)                                        \r";
                SQL += "      	) THEN '1'                                                                      \r";
                SQL += "      	END IS_SUGA_DEL                                                                 \r";


                SQL += "   FROM KOSMOS_OCS.OCS_OPRM      a                                          \r";
                SQL += "      , KOSMOS_OCS.OCS_ORDERCODE b                                          \r";
                if (strDeptDr.Trim() == "MG" || strDeptDr.Trim() == "MC" || strDeptDr.Trim() == "MP" ||
                    strDeptDr.Trim() == "ME" || strDeptDr.Trim() == "MN" || strDeptDr.Trim() == "MR" || strDeptDr.Trim() == "MI")
                {
                    SQL += "  WHERE a.DeptDr    in('" + strDeptDr.Trim() + "', 'MD')                \r";
                }
                else
                {
                    SQL += "  WHERE a.DeptDr    = '" + strDeptDr.Trim() + "'                        \r";
                }
                SQL += "    AND a.PRMname   = '" + strSetName + "'                                  \r";
                if (sGbOrder == 3)
                {
                    SQL += "    AND A.GBORDER = 'P'                                                 \r";    //검사처방
                }
                SQL += "    AND a.ORDERCODE = b.ORDERCODE(+)                                        \r";
                //SQL += "    AND a.BUN       = b.BUN (+)                                             \r";
                //SQL += "    AND a.CBUN      = b.CBUN                                                \r";
                SQL += "  ORDER BY a.Seqno, a.Slipno                                                \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows[0]["ILLCODES"].ToString().Trim().Length != 0)
                {
                    if (sGbOrder == 3)
                    {
                        nillCount = dt.Rows[0]["ILLCODES"].ToString().Length / 6;
                    }
                    else
                    {
                        nillCount = dt.Rows[0]["ILLCODES"].ToString().Length / 9;
                    }
                }
                else
                {
                    nillCount = 0;
                }

                if (dt.Rows[0]["BOOWI1"].ToString().Trim().Length != 0)
                {
                    nBoowi1Count = dt.Rows[0]["BOOWI1"].ToString().Length / 8;
                }
                else
                {
                    nBoowi1Count = 0;
                }

                if (dt.Rows[0]["BOOWI2"].ToString().Trim().Length != 0)
                {
                    nBoowi2Count = dt.Rows[0]["BOOWI2"].ToString().Length / 8;
                }
                else
                {
                    nBoowi2Count = 0;
                }

                if (dt.Rows[0]["BOOWI3"].ToString().Trim().Length != 0)
                {
                    nBoowi3Count = dt.Rows[0]["BOOWI3"].ToString().Length / 8;
                }
                else
                {
                    nBoowi3Count = 0;
                }

                if (dt.Rows[0]["BOOWI4"].ToString().Trim().Length != 0)
                {
                    nBoowi4Count = dt.Rows[0]["BOOWI4"].ToString().Length / 8;
                }
                else
                {
                    nBoowi4Count = 0;
                }

                Array.Resize(ref strIllCodes, nillCount);
                Array.Resize(ref strillName, nillCount);
                Array.Resize(ref strBoowi1s, nBoowi1Count);
                Array.Resize(ref strBoowi2s, nBoowi2Count);
                Array.Resize(ref strBoowi3s, nBoowi3Count);
                Array.Resize(ref strBoowi4s, nBoowi4Count);

                if (dt.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt, SpdNm, 0, true);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["GBINFO"].ToString().Trim().NotEmpty())
                        {
                            SpdNm.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["GBINFO"].ToString().Trim();
                        }

                        if (dt.Rows[i]["SENDDEPT"].ToString().Trim().Equals("N") ||
                            dt.Rows[i]["IS_SUBCODE_ERROR"].ToString().Trim().Equals("1") ||
                            dt.Rows[i]["IS_SUGA_DEL"].ToString().Trim().Equals("1"))
                        {
                            SpdNm.ActiveSheet.Cells[i, 0, i, SpdNm.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        SpdNm.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();

                        if (dt.Rows[i]["ORDERNAMES"].ToString().Trim() != "")
                        {
                            strUnit = dt.Rows[i]["ORDERNAME"].ToString();
                            SpdNm.ActiveSheet.Cells[i, 1].Text = strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString();
                        }
                        else if (dt.Rows[i]["DISPHEADER"].ToString().Trim() != "")
                        {
                            SpdNm.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DISPHEADER"].ToString() + " " + dt.Rows[i]["ORDERNAME"].ToString();
                        }
                        else
                        {
                            SpdNm.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ORDERNAME"].ToString();
                        }

                        if (dt.Rows[i]["GBBOTH"].ToString() == "1")
                        {
                            SpdNm.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ORDERNAME"].ToString() + " " + dt.Rows[i]["GBINFO"].ToString();
                        }

                        if (VB.Left(dt.Rows[i]["SLIPNO"].ToString().Trim(), 1) == "A")
                        {
                            if (dt.Rows[i]["ORDERNAME"].ToString().Trim() == "")
                            {
                                SpdNm.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                            }
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["ILLCODES"].ToString().Trim() != "")
                        {
                            for (int j = 0; j < nillCount; j++)
                            {
                                if (sGbOrder == 3)
                                {
                                    strIllCodes[j] = VB.Mid(dt.Rows[i]["ILLCODES"].ToString().Trim(), (6 * j) + 1, 6).Trim();
                                }
                                else
                                {
                                    strIllCodes[j] = VB.Mid(dt.Rows[i]["ILLCODES"].ToString().Trim(), (9 * j) + 1, 9).Trim();
                                }
                                strillName[j] = clsOrdDisp.Read_DiagName(strIllCodes[j].Trim());

                                if (strIllCodes[j] != "")
                                {
                                    SpdNm2.ActiveSheet.Cells[j, 0].Text = strIllCodes[j];
                                    SpdNm2.ActiveSheet.Cells[j, 1].Text = strillName[j];
                                }
                            }

                        }
                    }


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["BOOWI1"].ToString().Trim() != "")
                        {
                            for (int j = 0; j < nillCount; j++)
                            {
                                strBoowi1s[j] = VB.Mid(dt.Rows[i]["BOOWI1"].ToString(), (8 * j) + 1, 8).Trim();

                                if (strBoowi1s[j] != "")
                                {
                                    SpdNm2.ActiveSheet.Cells[j, 2].Text = strBoowi1s[j];
                                }
                            }
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["BOOWI2"].ToString().Trim() != "")
                        {
                            for (int j = 0; j < nillCount; j++)
                            {
                                strBoowi2s[j] = VB.Mid(dt.Rows[i]["BOOWI2"].ToString(), (8 * j) + 1, 8).Trim();

                                if (strBoowi2s[j] != "")
                                {
                                    SpdNm2.ActiveSheet.Cells[j, 3].Text = strBoowi2s[j];
                                }
                            }
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["BOOWI3"].ToString().Trim() != "")
                        {
                            for (int j = 0; j < nillCount; j++)
                            {
                                strBoowi3s[j] = VB.Mid(dt.Rows[i]["BOOWI3"].ToString(), (8 * i) + 1, 8).Trim();

                                if (strBoowi3s[j] != "")
                                {
                                    SpdNm2.ActiveSheet.Cells[j, 4].Text = strBoowi3s[j];
                                }
                            }
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["BOOWI4"].ToString().Trim() != "")
                        {
                            for (int j = 0; j < nillCount; j++)
                            {
                                strBoowi4s[j] = VB.Mid(dt.Rows[i]["BOOWI4"].ToString(), (8 * i) + 1, 8).Trim();

                                if (strBoowi4s[j] != "")
                                {
                                    SpdNm2.ActiveSheet.Cells[j, 5].Text = strBoowi4s[j];
                                }
                            }
                        }
                    }

                    //2021-01-11 안정수 추가 
                    if (clsType.User.IdNumber == "53775")
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["TUYEOPOINT"].ToString().Trim() != ""
                                || dt.Rows[i]["TUYEOTIME"].ToString().Trim() != "")
                            {                                
                                SpdNm.ActiveSheet.Cells[i, SpdNm.ActiveSheet.Columns.Count - 2].Text = dt.Rows[i]["TUYEOPOINT"].ToString().Trim();
                                SpdNm.ActiveSheet.Cells[i, SpdNm.ActiveSheet.Columns.Count - 1].Text = dt.Rows[i]["TUYEOTIME"].ToString().Trim();
                            }
                        }
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
        }

        public void Set_AutoSearch_Order(string ArgOrderCode, ListView lv)
        {
            ArgOrderCode = ArgOrderCode.ToUpper();

            try
            {
                SQL = "";
                SQL += " SELECT ordercode CORDERCODE, OrderName CORDERNAME, OrderNameS CORDERNAMES          \r";
                SQL += "      , GBBOTH CGBBOTH, DISPHEADER CDISPHEADER, GBINFO CGBINFO                      \r";
                SQL += "   FROM KOSMOS_OCS.OCS_ORDERCODE                                                    \r";
                SQL += "  WHERE (ORDERCODE LIKE '%'|| '" + ArgOrderCode + "' || '%'                         \r";
                SQL += "     or UPPER(ORDERNAME) LIKE '%'|| '" + ArgOrderCode + "' || '%'                   \r";
                SQL += "     or UPPER(ORDERNAMES) LIKE '%'|| '" + ArgOrderCode + "' || '%')                 \r";
                SQL += "    and (senddept != 'N' or senddept is null)                                       \r";
                SQL += "  ORDER BY ORDERNAME                                                                \r";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    //lv.Items.Clear();
                    //lv.BeginUpdate();

                    //while (i < dt1.Rows.Count)
                    //{
                    //    ListViewItem lvw = new ListViewItem();

                    //lvw.Text = dt1.Rows[i]["CORDERCODE"].ToString().Trim();

                    //if (dt1.Rows[i]["CORDERNAMES"].ToString().Trim() != "")
                    //{
                    //    strUnit = dt1.Rows[i]["CORDERNAME"].ToString();
                    //    lvw.SubItems.Add(strUnit + " " + dt1.Rows[i]["CORDERNAMES"].ToString());
                    //}
                    //else if (dt1.Rows[i]["CDISPHEADER"].ToString().Trim() != "")
                    //{
                    //    lvw.SubItems.Add(dt1.Rows[i]["CDISPHEADER"].ToString() + " " + dt1.Rows[i]["CORDERNAME"].ToString());
                    //}
                    //else
                    //{
                    //    lvw.SubItems.Add(dt1.Rows[i]["CORDERNAME"].ToString());
                    //}

                    //if (dt1.Rows[i]["CGBBOTH"].ToString() == "1")
                    //{
                    //    lvw.SubItems.Add(dt1.Rows[i]["CORDERNAME"].ToString() + " " + dt1.Rows[i]["GBINFO"].ToString());
                    //}
                    //lv.Items.AddRange(new ListViewItem[] { lvw });

                    //lvw.Text = dt1.Rows[i]["CORDERCODE"].ToString().Trim();
                    //lvw.SubItems.Add(dt1.Rows[i]["CORDERNAME"].ToString().Trim());
                    //lv.Items.AddRange(new ListViewItem[] { lvw });

                    //    ++i;
                    //}

                    //lv.EndUpdate();
                }
                else
                {
                    //lv.Items.Clear();
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// ER PRN 처방 Read
        /// </summary>
        public static DataTable Read_ErPrn_Orders(string strPano, string strBDate)
        {
            DataTable rtnDt = null;
            int nReadOrder;
            //string strBDateChk = "N";
            //string cMaxBDate = "";

            nReadOrder = clsOrdFunction.GnReadOrder;

            clsOrdFunction.GnTuyakno = 0;
            clsOrdFunction.GnReadIlls = 0;
            clsOrdFunction.GnReadOrder = 0;
            clsOrdFunction.GnReadOrder2 = 0;
            clsOrdFunction.GnJinOrdCount = 0;

            try
            {
                SQL = "";
                SQL += " SELECT d.*, TO_CHAR(d.EntDate,'YYYY-MM-DD HH24:Mi') EntDate1                               \r";
                SQL += "      , d.ROWID                                                                             \r";
                SQL += "      , a.DispHeader cDispHeader, a.OrderName cOrderName, a.DispRGB cDispRGB                \r";
                SQL += "      , a.GbBoth cGbBoth, a.GbInfo cGbInfo,a.DrugName cDrugName                             \r";
                SQL += "      , a.GbQty cGbQty, a.GbDosage cGbDosage, a.NextCode cNextCode, c.DAICODE               \r";
                SQL += "      , a.OrderNameS cOrderNameS, a.GbImiv cGbImiv, b.SuGbF, b.Bun BunSu                    \r";
                SQL += "      , KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(d.DOSCODE) DOSNAME                                   \r";
                SQL += "      , KOSMOS_OCS.FC_OCS_ODOSAGE_DIV(d.DOSCODE) DIV                                        \r";
                SQL += "      , KOSMOS_OCS.FC_OCS_OSPECIMAN_NAME(d.DOSCODE, d.SLIPNO) SPECNAME                      \r";
                SQL += "   FROM KOSMOS_OCS.OCS_ORDERCODE a                                                          \r";
                SQL += "      , KOSMOS_PMPA.BAS_SUT      b                                                          \r";
                SQL += "      , KOSMOS_PMPA.BAS_SUN      c                                                          \r";
                SQL += "      , KOSMOS_OCS.OCS_IORDER    d                                                          \r";
                SQL += "  WHERE a.SuCode    = b.SuCode(+)                                                           \r";
                SQL += "    AND a.SuCode    = C.SUNEXT(+)                                                           \r";
                SQL += "    AND a.ORDERCODE = d.ORDERCODE(+)                                                        \r";
                SQL += "    AND a.SLIPNO    = d.SLIPNO(+)                                                           \r";
                SQL += "    AND d.Ptno      = '" + strPano + "'                                                     \r";
                SQL += "    AND d.GbStatus  IN  (' ','D+')                                                          \r";
                SQL += "    AND d.BDate     = TO_DATE('" + DateTime.Parse(strBDate).ToShortDateString() + "', 'YYYY-MM-DD') \r";
                SQL += "    AND d.GbPRN     = 'P'                                                                   \r";
                SQL += "  ORDER BY d.Seqno                                                                          \r";
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return null;
                }

                rowcounter = 0;
                rowcounter = dt.Rows.Count;

                if (rowcounter > 0)
                {
                    clsOrdFunction.GnSunapOrdCount = dt.Rows.Count;
                    rtnDt = dt;
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }

            return rtnDt;
        }

        public static int fn_SunapOrderCount(string strPaNo, string sDeptCode, string sDrCode, string sGBIO, string sBDate)
        {
            int nRtn = 0;

            try
            {
                if (sGBIO == "OPD")
                {
                    SQL = "";
                    SQL += " SELECT count('X') OrdCnt                                       \r";
                    SQL += "   FROM KOSMOS_OCS.OCS_OORDER                                   \r";
                    SQL += "  WHERE Ptno      = '" + strPaNo + "'                           \r";
                    SQL += "    AND BDate     = TO_DATE('" + sBDate + "', 'YYYY-MM-DD')     \r";
                    SQL += "    AND DeptCode  = '" + sDeptCode + "'                         \r";
                    SQL += "    AND GBSUNAP IN('1', '2')                                    \r";
                    if (sDrCode == "1107  " || sDrCode == "1125  ") //(1107 : 오동호, 1125 : 최정란)
                    {
                        SQL += "     AND DRCODE = '" + sDrCode + "'                         \r";
                    }
                    else
                    {
                        SQL += "    AND DRCODE NOT IN('1107', '1125')                       \r";
                    }
                    if (sDeptCode.Trim() == "MN")
                    {
                        SQL += "    AND DEPTCODE IN('" + sDeptCode.Trim() + "', 'HD')       \r";
                    }
                    SQL += "    AND SEQNO > 0                                               \r";
                    SQL += "    AND NAL > 0                                                 \r";
                }
                else if (sGBIO == "ER")
                {
                    SQL = "";
                    SQL += " SELECT count('X') OrdCnt                                       \r";
                    SQL += "   FROM KOSMOS_OCS.OCS_IORDER                                   \r";
                    SQL += "  WHERE Ptno      = '" + strPaNo + "'                           \r";
                    SQL += "    AND GbStatus  IN  (' ','D+','D')                            \r";
                    SQL += "    AND BDate     >= TO_DATE('" + sBDate + "', 'YYYY-MM-DD')    \r";
                    if (clsPublic.Gstr구두Chk != "OK")
                    {
                        SQL += "    AND (NurseID IS NULL OR NurseId = ' ' )                 \r";
                    }
                    SQL += "    AND ORDERSITE NOT IN ('CAN','NDC')                          \r";
                    SQL += "    AND GBIOE IN('E','EI')                                      \r";
                    SQL += "    AND GBSEND = ' '                                            \r";    //slip 전송 된 처방
                }
                else if (sGBIO == "IPD")
                {
                    SQL = "";
                    SQL += " SELECT count('X') OrdCnt                                                                       \r";
                    SQL += "   FROM KOSMOS_OCS.OCS_ORDERCODE a                                                              \r";
                    SQL += "      , KOSMOS_PMPA.BAS_SUT      b                                                              \r";
                    SQL += "      , KOSMOS_PMPA.BAS_SUN      c                                                              \r";
                    SQL += "      , KOSMOS_OCS.OCS_IORDER    d                                                              \r";
                    SQL += "  WHERE a.SuCode    = b.SuCode(+)                                                               \r";
                    SQL += "    AND a.SuCode    = C.SUNEXT(+)                                                               \r";
                    SQL += "    AND d.ORDERCODE = a.ORDERCODE(+)                                                            \r";
                    SQL += "    AND d.SLIPNO    = a.SLIPNO(+)                                                               \r";
                    SQL += "    AND Ptno        = '" + clsOrdFunction.Pat.PtNo + "'                                         \r";
                    //Consult 처방시에 해당과(소견과) 처방만 보여줌
                    if (clsOrdFunction.GstrConsultOrd == "ON")
                    {
                        SQL += "   AND d.DeptCode = '" + clsPublic.GstrDeptCode.Trim() + "'                                 \r";
                    }
                    //'2015-04-03 재원환자중 PC의사 처방시 cnt 체크
                    if (clsOrdFunction.GstrDeptPC_Doct == "OK" && clsOrdFunction.GstrConsultOrd != "ON")
                    {
                        SQL += "   AND d.DeptCode = 'PC'                                                                    \r";
                    }
                    //if (clsOrdFunction.GnReadOrder == 9999)    //당일처방 존재
                    SQL += "    AND d.GbStatus  IN  (' ','D+','D')                                                          \r";
                    SQL += "    AND d.BDate      = TO_DATE('" + clsOrdFunction.GstrBDate + "','YYYY-MM/-DD')                 \r";

                    if (clsPublic.GstrJobMan.Trim() != "마취실" && clsPublic.GstrJobMan != "수술실")
                    {
                        SQL += "    AND ((d.GBIOE IN ( 'EI' ,'E') AND d.GBACT ='*') OR (d.GBACT <> '*' OR d.GBACT IS NULL)) \r";
                    }
                    if ((clsPublic.GstrJobMan == "간호사" || clsPublic.GstrJobMan == "수술실" || clsPublic.GstrJobMan == "마취실") && clsOrdFunction.GstrOrdDis.Trim() != "")//각 Part별 처방 보기
                    {
                        SQL += "    AND  d.OrderSite  = '" + clsOrdFunction.GstrOrdDis.Trim() + "'                          \r";
                    }
                    else if (clsOrdFunction.GstrDept.Trim() == "AN")
                    {
                        SQL += "    AND  d.OrderSite  = 'AN'                                                                \r";   //마취과 처방만 봄
                    }
                    else if (clsOrdFunction.GstrDept.Trim() == "XR")
                    {
                        SQL += "    AND  d.OrderSite  = 'XR'                                                                \r";   //방사선과 처방만 봄
                    }
                    SQL += "    AND d.GBSEND = ' '                                                                          \r";    //slip 전송 된 처방
                    SQL += "    AND(d.NurseID IS NULL OR d.NurseId = ' '                                                    \r";
                    SQL += "     OR d.OrderSite IN('', 'TEL', 'OPD', 'DRUG', 'IPD', 'ER', ''))                              \r";
                }
                clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return 0;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["ORDCNT"].ToString() != null)
                    {
                        nRtn = int.Parse(dt.Rows[0]["ORDCNT"].ToString());
                    }
                    else
                    {
                        nRtn = 0;
                    }
                }
                dt.Dispose();
                dt = null;
                return nRtn;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return 0;
            }
        }
        /// <summary>
        /// 처방 자동 발생 (IA231(회송환자관리료 자동 발생) 등)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="SpdNm"></param>
        /// <param name="GBIO"></param>
        /// <param name="sOrdCode"></param>
        /// <param name="sSuCode"></param>
        /// <param name="sSlipNo"></param>
        /// <param name="BlnChk"></param>
        public void Set_Auto_OrderSend(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread SpdNm, string GBIO, string sOrdCode, string sSuCode, string sSlipNo, bool BlnChk)
        {
            int nCnt = 0;
            int nRow = 0;
            DataTable dtOrd = null;
            

            //if (GBIO == "OPD")
            //{
                if (BlnChk == true)
                {
                    for (int i = 0; i < SpdNm.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        if (SpdNm.ActiveSheet.Cells[i, 0].Text != "True")
                        {
                            if (SpdNm.ActiveSheet.Cells[i, 1].Text.Trim() == sOrdCode)
                            {
                                nCnt += 1;
                                break;
                            }
                        }
                    }

                    if (nCnt == 0)
                    {
                        //기 전송 된 처방 Read
                        if (fn_PreAutoOrder_Check(pDbCon, GBIO, sOrdCode) == true)
                        {
                            return;
                        }
                    }

                    nRow = SpdNm.ActiveSheet.NonEmptyRowCount;

                    dtOrd = Order_Read(SpdNm, sOrdCode, sSlipNo, nRow);
                    if (dtOrd == null) return;

                    fn_Order_Select_Move(dtOrd, "AUTOSEND", GBIO, SpdNm, nRow);

                    SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.NonEmptyRowCount - 1, 2);

                    if (SpdNm.ActiveSheet.NonEmptyRowCount > 20)
                    {
                        SpdNm.ShowRow(0, SpdNm.ActiveSheet.NonEmptyRowCount, FarPoint.Win.Spread.VerticalPosition.Center);
                    }
                }
            //}
        }

        /// <summary>
        /// 이미 처방이 발생했는지 체크
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="GBIO"></param>
        /// <param name="sOrdCode"></param>
        /// <returns></returns>
        public bool fn_PreAutoOrder_Check(PsmhDb pDbCon, string GBIO, string sOrdCode)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            bool rtnVal = false;

            if (GBIO == "OPD")
            {
                SQL = "";
                SQL += " SELECT ORDERCODE                                                           \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OORDER                                               \r";
                SQL += "  WHERE ORDERCODE = '" + sOrdCode + "'                                      \r";
                SQL += "    AND PTNO = '" + clsOrdFunction.Pat.PtNo + "'                            \r";
                SQL += "    AND BDATE = TO_DATE('" + clsOrdFunction.GstrBDate + "', 'YYYY-MM-DD')   \r";
                SQL += "    AND DEPTCODE = '" + clsOrdFunction.Pat.DeptCode + "'                    \r";
                SQL += "    AND NAL > 0                                                             \r";
                SQL += "    AND GBSUNAP IN ('0', '1')                                               \r";
            }
            else
            {
                SQL = "";
                SQL += " SELECT ORDERCODE                                                           \r";
                SQL += "   FROM KOSMOS_OCS.OCS_IORDER                                               \r";
                SQL += "  WHERE ORDERCODE = '" + sOrdCode + "'                                      \r";
                SQL += "    AND PTNO = '" + clsOrdFunction.Pat.PtNo + "'                            \r";
                SQL += "    AND BDATE = TO_DATE('" + clsOrdFunction.GstrBDate + "', 'YYYY-MM-DD')   \r";
                SQL += "    AND DEPTCODE = '" + clsOrdFunction.Pat.DeptCode + "'                    \r";
                SQL += "    AND NAL > 0                                                             \r";
                SQL += "    AND GbStatus  IN  (' ','D+','D')                                        \r";
            }
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                rtnVal = true;
            }

            return rtnVal;
        }

        /// <summary>
        /// 진정관리료가 내려져 있는지 확인한다
        /// </summary>
        /// <param name="SpdNm">처방스프레드</param>
        /// <param name="ColOrdercode">처방코드컬럼</param>
        /// <returns></returns>
        public bool fn_Check_Sedation_Order(FarPoint.Win.Spread.FpSpread SpdNm, int ColOrdercode)
        {
            bool rtnVal = false;
            string strSedation = "AC302";

            for (int i = 0; i < SpdNm.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (SpdNm.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    if (SpdNm.ActiveSheet.Cells[i, ColOrdercode].Text.Trim() == strSedation)
                    {
                        rtnVal = true;
                        break;
                    }
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 처방에 진정관련 코드가 있는지 확인한다
        /// </summary>
        /// <param name="SpdNm"></param>
        /// <param name="ColOrdercode"></param>
        /// <param name="arrySEDATION"></param>
        /// <returns></returns>
        public int fn_Check_Sedation_Order_Ex(FarPoint.Win.Spread.FpSpread SpdNm, int ColOrdercode, string[] arrySEDATION)
        {
            int rtnVal = -1;
            if (arrySEDATION == null)
            {
                return -1;
            }

            for (int i = 0; i < SpdNm.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (SpdNm.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    for (int j = 0; j < arrySEDATION.Length; j++)
                    {
                        if (SpdNm.ActiveSheet.Cells[i, ColOrdercode].Text.Trim() == arrySEDATION[j].Trim())
                        {
                            rtnVal = i;
                            break;
                        }
                    }
                }
            }
            return rtnVal;
        }

        public DataTable READ_OPRM_ORDER_LIST(PsmhDb pDbCon, string strDEPTDR, string strPRMNAME)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT a.ROWID, a.* FROM KOSMOS_OCS.OCS_OPRM a  \r";
            SQL += " WHERE DEPTDR = '" + strDEPTDR + "'             \r";
            SQL += "   AND PRMNAME = '" + strPRMNAME + "'           \r";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }
    }
}

