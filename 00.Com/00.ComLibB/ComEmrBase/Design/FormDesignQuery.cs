using ComBase; //기본 클래스
using System;
using System.Data;

namespace ComEmrBase
{
    /// <summary>
    /// 디자인 관련 쿼리 모음
    /// </summary>
    public class FormDesignQuery
    {
        #region //AEMRFORMXML
        /// <summary>
        /// AEMRFORMXML 
        /// SELECT : 폼별조회
        /// </summary>
        /// <param name="pFormNo"></param>
        /// <param name="pUpdateNo"></param>
        /// <param name="pOrderBy"></param>
        /// <returns></returns>
        public static string Query_AEMRFORMXML_Select(string pFormNo, string pUpdateNo, string pOrderBy = "")
        {
            //ORDER BY CONTROLPARENT, LOCATIONY, CHILDINDEX DESC
            string strQuery = "";

            string SQL = "";    //Query문

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    FORMNO,UPDATENO,CONTROLNAME,CONTROTYPE,CONTROLPARENT,LOCATIONX,LOCATIONY,";
            SQL = SQL + ComNum.VBLF + "    SIZEWIDTH, SIZEHEIGHT, USERFUNC, CHILDINDEX, BACKCOLOR,FORECOLOR,BOARDSTYLE,DOCK,ENABLED,VISIBLED,TEXT,";
            SQL = SQL + ComNum.VBLF + "    FONTS,MULTILINE,SCROLLBARS,TEXTALIGN,ITEMIMAGE,IMAGESIZEMODE,CHECKED, APPEARANCS, ";
            SQL = SQL + ComNum.VBLF + "    CHECKALIGN, FLATSTYLE, FLATBORDERCOLOR, FLATBORDERSIZE, AUTOSIZE, AUTOHEIGH, MIBI, INITVALUE, TABORDER";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFORMXML";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNO =  " + pFormNo;
            SQL = SQL + ComNum.VBLF + "     AND UPDATENO =  " + pUpdateNo;
            if (pOrderBy != "")
            {
                SQL = SQL + ComNum.VBLF + pOrderBy;
            }
            strQuery = SQL;
            return strQuery;
        }

        /// <summary>
        /// AEMRFORMXML AND IMEGE
        /// SELECT : 전체 서식 조회
        /// </summary>
        /// <param name="pOrderBy"></param>
        /// <returns></returns>
        public static string Query_AEMRFORMXMLandIMAGE(string pFormNo, string pUpdateNo)
        {
            string strQuery = "";

            string SQL = "";    //Query문

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    A.FORMNO, A.UPDATENO, A.CONTROLNAME, A.CONTROTYPE, A.CONTROLPARENT, A.LOCATIONX, A.LOCATIONY,";
            SQL = SQL + ComNum.VBLF + "    A.SIZEWIDTH, A.SIZEHEIGHT, A.USERFUNC, A.CHILDINDEX, A.BACKCOLOR, A.FORECOLOR, A.BOARDSTYLE, A.DOCK, A.ENABLED, A.VISIBLED, A.TEXT,";
            SQL = SQL + ComNum.VBLF + "    A.FONTS, A.MULTILINE, A.SCROLLBARS, A.TEXTALIGN, A.ITEMIMAGE, A.IMAGESIZEMODE, A.CHECKED, A.APPEARANCS, ";
            SQL = SQL + ComNum.VBLF + "    A.CHECKALIGN, A.FLATSTYLE, A.FLATBORDERCOLOR, A.FLATBORDERSIZE, A.AUTOSIZE, A.AUTOHEIGH, A.MIBI, A.INITVALUE, A.TABORDER, A.MAXTEXTLENGTH, A.READONLY, ";
            SQL = SQL + ComNum.VBLF + "    A1.CONTROLNAME AS PARENTNAME, A1.CONTROTYPE AS PARENTTYPE, ";
            SQL = SQL + ComNum.VBLF + "    B.IMAGE";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFORMXML A";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AEMRFORMXML A1";
            SQL = SQL + ComNum.VBLF + "     ON A.FORMNO = A1.FORMNO";
            SQL = SQL + ComNum.VBLF + "     AND A.UPDATENO = A1.UPDATENO";
            SQL = SQL + ComNum.VBLF + "     AND A.CONTROLPARENT = A1.CONTROLNAME";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AEMRFORMXMLIMAGE B";
            SQL = SQL + ComNum.VBLF + "    ON A.FORMNO = B.FORMNO";
            SQL = SQL + ComNum.VBLF + "    AND A.UPDATENO = B.UPDATENO";
            SQL = SQL + ComNum.VBLF + "    AND A.CONTROLNAME = B.CONTROLNAME";
            SQL = SQL + ComNum.VBLF + "WHERE A.FORMNO =  " + pFormNo;
            SQL = SQL + ComNum.VBLF + "     AND A.UPDATENO =  " + pUpdateNo;
            SQL = SQL + ComNum.VBLF + "ORDER BY A.CONTROLPARENT, A.LOCATIONY, A.LOCATIONY, A.CHILDINDEX DESC";
            
            strQuery = SQL;
            return strQuery;
        }

        /// <summary>
        /// AEMRFORMXML
        /// SELECT : 전체 서식 조회
        /// </summary>
        /// <param name="pOrderBy"></param>
        /// <returns></returns>
        //public static string Query_AEMRFORMXML_SelectAll(string pOrderBy)
        //{
        //    string strQuery = "";

        //    string SQL = "";    //Query문

        //    SQL = "";
        //    SQL = SQL + ComNum.VBLF + "SELECT ";
        //    SQL = SQL + ComNum.VBLF + "    FORMNO,UPDATENO,CONTROLNAME,CONTROTYPE,CONTROLPARENT,LOCATIONX,LOCATIONY,";
        //    SQL = SQL + ComNum.VBLF + "    SIZEWIDTH,SIZEHEIGHT,USERFUNC,CHILDINDEX, BACKCOLOR,FORECOLOR,BOARDSTYLE,DOCK,ENABLED,VISIBLED,TEXT,";
        //    SQL = SQL + ComNum.VBLF + "    FONTS,MULTILINE,SCROLLBARS,TEXTALIGN,IMAGE,IMAGESIZEMODE,CHECKED, APPEARANCS, ";
        //    SQL = SQL + ComNum.VBLF + "    CHECKALIGN, FLATSTYLE, FLATBORDERCOLOR, FLATBORDERSIZE, AUTOSIZE, AUTOHEIGH, MIBI";
        //    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFORMXML";
        //    if (pOrderBy != "")
        //    {
        //        SQL = SQL + ComNum.VBLF + pOrderBy;
        //    }
        //    strQuery = SQL;
        //    return strQuery;
        //}

        /// <summary>
        /// AEMRFORMXML
        /// DELETE : 폼별 삭제
        /// </summary>
        /// <param name="pFormNo"></param>
        /// <param name="pUpdateNo"></param>
        /// <returns></returns>
        public static string Query_AEMRFORMXML_Delete(string pFormNo, string pUpdateNo)
        {
            string strQuery = "";

            string SQL = "";    //Query문

            SQL = "";
            SQL = SQL + ComNum.VBLF + "DELETE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFORMXML";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNO =  " + pFormNo;
            SQL = SQL + ComNum.VBLF + "     AND UPDATENO =  " + pUpdateNo;
            strQuery = SQL;
            return strQuery;
        }

        /// <summary>
        /// AEMRFORMXML and IMAGE의 값을 배열에 저장한다
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <returns></returns>
        public static FormXml[] GetDataFormXml(string strFormNo, string strUpdateNo)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            FormXml[] pFormXml = null;

            try
            {
                SQL = "";
                SQL = FormDesignQuery.Query_AEMRFORMXMLandIMAGE(strFormNo, strUpdateNo);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return pFormXml;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return pFormXml;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (pFormXml == null)
                    {
                        pFormXml = new FormXml[1];
                    }
                    else
                    {
                        Array.Resize<FormXml>(ref pFormXml, pFormXml.Length + 1);
                    }

                    pFormXml[pFormXml.Length - 1] = new FormXml();
                    pFormXml[pFormXml.Length - 1].strCONTROLNAME = dt.Rows[i]["CONTROLNAME"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strCONTROTYPE = dt.Rows[i]["CONTROTYPE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strCONTROLPARENT = dt.Rows[i]["CONTROLPARENT"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strLOCATIONX = dt.Rows[i]["LOCATIONX"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strLOCATIONY = dt.Rows[i]["LOCATIONY"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strSIZEWIDTH = dt.Rows[i]["SIZEWIDTH"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strSIZEHEIGHT = dt.Rows[i]["SIZEHEIGHT"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strUSERFUNC = dt.Rows[i]["USERFUNC"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strCHILDINDEX = dt.Rows[i]["CHILDINDEX"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strBACKCOLOR = dt.Rows[i]["BACKCOLOR"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strFORECOLOR = dt.Rows[i]["FORECOLOR"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strBOARDSTYLE = dt.Rows[i]["BOARDSTYLE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strDOCK = dt.Rows[i]["DOCK"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strENABLED = dt.Rows[i]["ENABLED"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strVISIBLED = dt.Rows[i]["VISIBLED"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strTEXT = dt.Rows[i]["TEXT"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strFONTS = dt.Rows[i]["FONTS"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strMULTILINE = dt.Rows[i]["MULTILINE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strSCROLLBARS = dt.Rows[i]["SCROLLBARS"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strTEXTALIGN = dt.Rows[i]["TEXTALIGN"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strITEMIMAGE = dt.Rows[i]["ITEMIMAGE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].imgIMAGE = ComFunc.StringToImage(dt.Rows[i]["IMAGE"].ToString().Trim());
                    pFormXml[pFormXml.Length - 1].strIMAGESIZEMODE = dt.Rows[i]["IMAGESIZEMODE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strCHECKALIGN = dt.Rows[i]["CHECKALIGN"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strCHECKED = dt.Rows[i]["CHECKED"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strAPPEARANCS = dt.Rows[i]["APPEARANCS"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strFLATSTYLE = dt.Rows[i]["FLATSTYLE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strFLATBORDERCOLOR = dt.Rows[i]["FLATBORDERCOLOR"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strFLATBORDERSIZE = dt.Rows[i]["FLATBORDERSIZE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strAUTOSIZE = dt.Rows[i]["AUTOSIZE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strAUTOHEIGH = dt.Rows[i]["AUTOHEIGH"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strMIBI = dt.Rows[i]["MIBI"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strINITVALUE = dt.Rows[i]["INITVALUE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strTABORDER = dt.Rows[i]["TABORDER"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strMAXTEXTLENGTH = VB.Val(dt.Rows[i]["MAXTEXTLENGTH"].ToString().Trim()) == 0 ? "32767" : dt.Rows[i]["MAXTEXTLENGTH"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strTEXTREADONLY = dt.Rows[i]["READONLY"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                return pFormXml;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return pFormXml;
            }
        }
        #endregion //AEMRFORMXML

        #region //AEMRITEMGROUPXML

        /// <summary>
        /// AEMRITEMGROUPXML
        /// SELECT : 템플릿별 조회
        /// </summary>
        /// <param name="pITEMGRPNO"></param>
        /// <param name="pUPDATENO"></param>
        /// <param name="pOrderBy"></param>
        /// <returns></returns>
        //public static string Query_AEMRITEMGROUPXML_Select(string pITEMGRPNO, string pUPDATENO, string pOrderBy = "")
        //{
        //    string strQuery = "";

        //    string SQL = "";    //Query문

        //    SQL = "";
        //    SQL = SQL + ComNum.VBLF + "SELECT ";
        //    SQL = SQL + ComNum.VBLF + "    ITEMGRPNO, UPDATENO,CONTROLNAME,CONTROTYPE,CONTROLPARENT,LOCATIONX,LOCATIONY,";
        //    SQL = SQL + ComNum.VBLF + "    SIZEWIDTH,SIZEHEIGHT,USERFUNC,CHILDINDEX, BACKCOLOR,FORECOLOR,BOARDSTYLE,DOCK,ENABLED,VISIBLED,TEXT,";
        //    SQL = SQL + ComNum.VBLF + "    FONTS,MULTILINE,SCROLLBARS,TEXTALIGN,IMAGE,IMAGESIZEMODE,CHECKED, APPEARANCS, ";
        //    SQL = SQL + ComNum.VBLF + "    CHECKALIGN, FLATSTYLE, FLATBORDERCOLOR, FLATBORDERSIZE, AUTOSIZE, AUTOHEIGH, MIBI";
        //    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMGROUPXML"; 
        //    SQL = SQL + ComNum.VBLF + "WHERE ITEMGRPNO =  " + pITEMGRPNO;
        //    SQL = SQL + ComNum.VBLF + "     AND UPDATENO =  " + pUPDATENO;
        //    if (pOrderBy != "")
        //    {
        //        SQL = SQL + ComNum.VBLF + pOrderBy;
        //    }
        //    strQuery = SQL;
        //    return strQuery;
        //}

        /// <summary>
        /// AEMRITEMGROUPXML and IMAGE
        /// </summary>
        /// <param name="pITEMGRPNO"></param>
        /// <param name="pUPDATENO"></param>
        /// <returns></returns>
        public static string Query_AEMRITEMGROUPXMLandIMAGE(string pITEMGRPNO, string pUPDATENO)
        {
            string strQuery = "";

            string SQL = "";    //Query문

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    A.ITEMGRPNO, A.UPDATENO, A.CONTROLNAME, A.CONTROTYPE, A.CONTROLPARENT, A.LOCATIONX, A.LOCATIONY,";
            SQL = SQL + ComNum.VBLF + "    A.SIZEWIDTH, A.SIZEHEIGHT, A.USERFUNC, A.CHILDINDEX, A.BACKCOLOR, A.FORECOLOR, A.BOARDSTYLE, A.DOCK, A.ENABLED, A.VISIBLED, A.TEXT,";
            SQL = SQL + ComNum.VBLF + "    A.FONTS, A.MULTILINE, A.SCROLLBARS, A.TEXTALIGN, A.ITEMIMAGE, A.IMAGESIZEMODE, A.CHECKED, A.APPEARANCS, ";
            SQL = SQL + ComNum.VBLF + "    A.CHECKALIGN, A.FLATSTYLE, A.FLATBORDERCOLOR, A.FLATBORDERSIZE, A.AUTOSIZE, A.AUTOHEIGH, A.MIBI, A.INITVALUE, A.TABORDER, ";
            SQL = SQL + ComNum.VBLF + "    B.IMAGE";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMGROUPXML A";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN  " + ComNum.DB_EMR + "AEMRITEMGROUPXMLIMAGE B";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMGRPNO = B.ITEMGRPNO";
            SQL = SQL + ComNum.VBLF + "    AND A.UPDATENO = B.UPDATENO";
            SQL = SQL + ComNum.VBLF + "WHERE A.ITEMGRPNO =  " + pITEMGRPNO;
            SQL = SQL + ComNum.VBLF + "     AND A.UPDATENO =  " + pUPDATENO;
            SQL = SQL + ComNum.VBLF + "ORDER BY A.CONTROLPARENT, A.LOCATIONY, A.LOCATIONY, A.CHILDINDEX DESC";
            
            strQuery = SQL;
            return strQuery;
        }

        /// <summary>
        /// AEMRITEMGROUP
        /// </summary>
        /// <param name="pSubSql"></param>
        /// <param name="pOrderBy"></param>
        /// <returns></returns>
        public static string Query_AEMRITEMGROUP_Select(string pSubSql = "", string pOrderBy = "")
        {
            string strQuery = "";

            string SQL = "";    //Query문

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    GRPNO, ITEMGRPNO, ITEMGRPNAME, UPDATENO,";
            SQL = SQL + ComNum.VBLF + "    ITEMGRPCD, ITEMNO, INPUSEID, INPDATE, INPTIME, INPCOMIP";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRITEMGROUP";
            if (pSubSql != "")
            {
                SQL = SQL + ComNum.VBLF + pSubSql;
            }
            if (pOrderBy != "")
            {
                SQL = SQL + ComNum.VBLF + pOrderBy;
            }
            strQuery = SQL;
            return strQuery;
        }

        public static FormXml[] GetDataItemGroupXml(string pITEMGRPNO, string pUPDATENO)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            FormXml[] pFormXml = null;

            try
            {
                SQL = "";
                SQL = Query_AEMRITEMGROUPXMLandIMAGE(pITEMGRPNO, pUPDATENO);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return pFormXml;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return pFormXml;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (pFormXml == null)
                    {
                        pFormXml = new FormXml[1];
                    }
                    else
                    {
                        Array.Resize<FormXml>(ref pFormXml, pFormXml.Length + 1);
                    }

                    pFormXml[pFormXml.Length - 1] = new FormXml();
                    pFormXml[pFormXml.Length - 1].strCONTROLNAME = dt.Rows[i]["CONTROLNAME"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strCONTROTYPE = dt.Rows[i]["CONTROTYPE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strCONTROLPARENT = dt.Rows[i]["CONTROLPARENT"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strLOCATIONX = dt.Rows[i]["LOCATIONX"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strLOCATIONY = dt.Rows[i]["LOCATIONY"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strSIZEWIDTH = dt.Rows[i]["SIZEWIDTH"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strSIZEHEIGHT = dt.Rows[i]["SIZEHEIGHT"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strUSERFUNC = dt.Rows[i]["USERFUNC"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strCHILDINDEX = dt.Rows[i]["CHILDINDEX"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strBACKCOLOR = dt.Rows[i]["BACKCOLOR"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strFORECOLOR = dt.Rows[i]["FORECOLOR"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strBOARDSTYLE = dt.Rows[i]["BOARDSTYLE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strDOCK = dt.Rows[i]["DOCK"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strENABLED = dt.Rows[i]["ENABLED"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strVISIBLED = dt.Rows[i]["VISIBLED"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strTEXT = dt.Rows[i]["TEXT"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strFONTS = dt.Rows[i]["FONTS"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strMULTILINE = dt.Rows[i]["MULTILINE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strSCROLLBARS = dt.Rows[i]["SCROLLBARS"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strTEXTALIGN = dt.Rows[i]["TEXTALIGN"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strITEMIMAGE = dt.Rows[i]["ITEMIMAGE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].imgIMAGE = ComFunc.StringToImage(dt.Rows[i]["IMAGE"].ToString().Trim());
                    pFormXml[pFormXml.Length - 1].strIMAGESIZEMODE = dt.Rows[i]["IMAGESIZEMODE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strCHECKALIGN = dt.Rows[i]["CHECKALIGN"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strCHECKED = dt.Rows[i]["CHECKED"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strAPPEARANCS = dt.Rows[i]["APPEARANCS"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strFLATSTYLE = dt.Rows[i]["FLATSTYLE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strFLATBORDERCOLOR = dt.Rows[i]["FLATBORDERCOLOR"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strFLATBORDERSIZE = dt.Rows[i]["FLATBORDERSIZE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strAUTOSIZE = dt.Rows[i]["AUTOSIZE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strAUTOHEIGH = dt.Rows[i]["AUTOHEIGH"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strMIBI = dt.Rows[i]["MIBI"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strINITVALUE = dt.Rows[i]["INITVALUE"].ToString().Trim();
                    pFormXml[pFormXml.Length - 1].strTABORDER = dt.Rows[i]["TABORDER"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                return pFormXml;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return pFormXml;
            }
        }

        #endregion //AEMRITEMGROUPXML


        #region //AEMRFLOWXML

        /// <summary>
        /// AEMRFLOWXML 조회
        /// </summary>
        /// <param name="pFormNo"></param>
        /// <param name="pUpdateNo"></param>
        /// <returns></returns>
        public static string Query_AEMRFLOWXML(string pFormNo, string pUpdateNo)
        {
            string strQuery = "";

            string SQL = "";    //Query문

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT                                     ";
            SQL = SQL + ComNum.VBLF + "   FORMNO, UPDATENO, ITEMNUMBER,           ";
            SQL = SQL + ComNum.VBLF + "   ITEMNO, ITEMNAME, CELLTYPE,             ";
            SQL = SQL + ComNum.VBLF + "   HALIGN, VALIGN, SIZEWIDTH,              ";
            SQL = SQL + ComNum.VBLF + "   SIZEHEIGHT, MULTILINE, CHECKALIGN,      ";
            SQL = SQL + ComNum.VBLF + "   USERMCRO, USERFUNC, USERFUNCNM          ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFLOWXML      ";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNO =  " + pFormNo;
            SQL = SQL + ComNum.VBLF + "     AND UPDATENO =  " + pUpdateNo;
            SQL = SQL + ComNum.VBLF + "ORDER BY FORMNO, UPDATENO, ITEMNUMBER";

            strQuery = SQL;
            return strQuery;
        }

        /// <summary>
        /// AEMRFLOWHEADXML 조회
        /// </summary>
        /// <param name="pFormNo"></param>
        /// <param name="pUpdateNo"></param>
        /// <returns></returns>
        public static string Query_AEMRFLOWHEADXML(string pFormNo, string pUpdateNo)
        {
            string strQuery = "";

            string SQL = "";    //Query문

            SQL = SQL + ComNum.VBLF + "SELECT                                             ";
            SQL = SQL + ComNum.VBLF + "   FORMNO, UPDATENO, ITEMNUMBER,                   ";
            SQL = SQL + ComNum.VBLF + "   HEADNUMBER, HEADTEXT, HROW,                     ";
            SQL = SQL + ComNum.VBLF + "   HCOL, VROW, VCOL,                               ";
            SQL = SQL + ComNum.VBLF + "   SIZEWIDTH, SIZEHEIGHT, MULTILINE,               ";
            SQL = SQL + ComNum.VBLF + "   FONTNAME, FONTSIZE, FONTBOLD,                   ";
            SQL = SQL + ComNum.VBLF + "   HALIGN, VALIGN, SPANROW,                        ";
            SQL = SQL + ComNum.VBLF + "   SPANCOL                                         ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFLOWHEADXML          ";
            SQL = SQL + ComNum.VBLF + "WHERE FORMNO =  " + pFormNo;
            SQL = SQL + ComNum.VBLF + "     AND UPDATENO =  " + pUpdateNo;
            SQL = SQL + ComNum.VBLF + "ORDER BY FORMNO, UPDATENO, ITEMNUMBER, HEADNUMBER";
            strQuery = SQL;
            return strQuery;
        }

        public static void GetSetDate_AEMRFLOWXML(string pFormNo, string pUpdateNo, ref string pFLOWGB, ref int pFLOWITEMCNT, ref int pFLOWHEADCNT,
                                                 ref int pFLOWINPUTSIZE, ref FormFlowSheet[] pFormFlowSheet, ref FormFlowSheetHead[,] pFormFlowSheetHead)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            pFormFlowSheet = null;
            pFormFlowSheetHead = null;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT  ";
                SQL = SQL + ComNum.VBLF + "     FLOWGB, FLOWITEMCNT, FLOWHEADCNT, FLOWINPUTSIZE";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRFORM ";
                SQL = SQL + ComNum.VBLF + "WHERE FORMNO = " + pFormNo;
                SQL = SQL + ComNum.VBLF + "     AND UPDATENO = " + pUpdateNo;
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

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
                pFLOWGB = dt.Rows[i]["FLOWGB"].ToString().Trim();
                pFLOWITEMCNT = (int)VB.Val(dt.Rows[i]["FLOWITEMCNT"].ToString().Trim());
                pFLOWHEADCNT = (int)VB.Val(dt.Rows[i]["FLOWHEADCNT"].ToString().Trim());
                pFLOWINPUTSIZE = (int)VB.Val(dt.Rows[i]["FLOWINPUTSIZE"].ToString().Trim());
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = Query_AEMRFLOWXML(pFormNo, pUpdateNo);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return ;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return ;
                }

                
                pFormFlowSheet = new FormFlowSheet[pFLOWITEMCNT];
                
                for (i = 0; i < pFLOWITEMCNT; i++)
                {
                    int intItemNum = (int)VB.Val(dt.Rows[i]["ITEMNUMBER"].ToString().Trim());
                    pFormFlowSheet[intItemNum] = new FormFlowSheet();
                    pFormFlowSheet[intItemNum].ItemNumber = (int) VB.Val(dt.Rows[i]["ITEMNUMBER"].ToString().Trim());
                    pFormFlowSheet[intItemNum].ItemCode = dt.Rows[i]["ITEMNO"].ToString().Trim();
                    pFormFlowSheet[intItemNum].ItemName = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    pFormFlowSheet[intItemNum].CellType = dt.Rows[i]["CELLTYPE"].ToString().Trim();
                    pFormFlowSheet[intItemNum].HorizontalAlignment = dt.Rows[i]["HALIGN"].ToString().Trim();
                    pFormFlowSheet[intItemNum].VerticalAlignment = dt.Rows[i]["VALIGN"].ToString().Trim();
                    pFormFlowSheet[intItemNum].Width = (int)VB.Val(dt.Rows[i]["SIZEWIDTH"].ToString().Trim());
                    pFormFlowSheet[intItemNum].Height = (int)VB.Val(dt.Rows[i]["SIZEHEIGHT"].ToString().Trim());
                    pFormFlowSheet[intItemNum].MultiLine = dt.Rows[i]["MULTILINE"].ToString().Trim();
                    pFormFlowSheet[intItemNum].CheckTextAlignment = dt.Rows[i]["CHECKALIGN"].ToString().Trim();
                    pFormFlowSheet[intItemNum].UserMcro = dt.Rows[i]["USERMCRO"].ToString().Trim();
                    pFormFlowSheet[intItemNum].UserFunc = dt.Rows[i]["USERFUNC"].ToString().Trim();
                    pFormFlowSheet[intItemNum].UserFuncNm = dt.Rows[i]["USERFUNCNM"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;

                SQL = "";
                SQL = Query_AEMRFLOWHEADXML(pFormNo, pUpdateNo);

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

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

                pFormFlowSheetHead = new FormFlowSheetHead[pFLOWITEMCNT, pFLOWHEADCNT];
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    int intItemNum = (int)VB.Val(dt.Rows[i]["ITEMNUMBER"].ToString().Trim());
                    int intHeadNum = (int)VB.Val(dt.Rows[i]["HEADNUMBER"].ToString().Trim());
                    pFormFlowSheetHead[intItemNum, intHeadNum] = new FormFlowSheetHead();
                    pFormFlowSheetHead[intItemNum, intHeadNum].ItemSeq = intItemNum;
                    pFormFlowSheetHead[intItemNum, intHeadNum].HeadSeq = intHeadNum;
                    pFormFlowSheetHead[intItemNum, intHeadNum].ItemCode = "";
                    pFormFlowSheetHead[intItemNum, intHeadNum].FontSize = (int)VB.Val(dt.Rows[i]["FONTSIZE"].ToString().Trim());
                    pFormFlowSheetHead[intItemNum, intHeadNum].FontBold = dt.Rows[i]["FONTBOLD"].ToString().Trim();
                    pFormFlowSheetHead[intItemNum, intHeadNum].Height = (int)VB.Val(dt.Rows[i]["SIZEHEIGHT"].ToString().Trim());
                    pFormFlowSheetHead[intItemNum, intHeadNum].Width = (int)VB.Val(dt.Rows[i]["SIZEWIDTH"].ToString().Trim());
                    pFormFlowSheetHead[intItemNum, intHeadNum].MultiLine = dt.Rows[i]["MULTILINE"].ToString().Trim();
                    pFormFlowSheetHead[intItemNum, intHeadNum].HorizontalAlignment = dt.Rows[i]["HALIGN"].ToString().Trim();
                    pFormFlowSheetHead[intItemNum, intHeadNum].VerticalAlignment = dt.Rows[i]["VALIGN"].ToString().Trim();
                    pFormFlowSheetHead[intItemNum, intHeadNum].SpanRow = (int)VB.Val(dt.Rows[i]["SPANROW"].ToString().Trim());
                    pFormFlowSheetHead[intItemNum, intHeadNum].SpanCol = (int)VB.Val(dt.Rows[i]["SPANCOL"].ToString().Trim());
                    pFormFlowSheetHead[intItemNum, intHeadNum].Text = dt.Rows[i]["HEADTEXT"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {

            }
        }


        #endregion //AEMRFLOWXML


    }
}
