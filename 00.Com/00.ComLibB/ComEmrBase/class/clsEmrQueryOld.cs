using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComDbB; //DB연결
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    public class clsEmrQueryOld
    {
        /// <summary>
        /// 지정된 스프레드에 작성
        /// </summary>
        /// <param spred="spred name"></param>
        /// <param pano="strPano"></param>
        /// <returns></returns>

        public static void GetVital(FarPoint.Win.Spread.FpSpread ssVital, string strPano)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssVital.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT CHARTDATE, CHARTTIME,   ";
                SQL = SQL + ComNum.VBLF + "              it3,  ";
                SQL = SQL + ComNum.VBLF + "              it4,  ";
                SQL = SQL + ComNum.VBLF + "              it5,  ";
                SQL = SQL + ComNum.VBLF + "              it6,   ";
                SQL = SQL + ComNum.VBLF + "              it7,  ";
                SQL = SQL + ComNum.VBLF + "              it8,  ";
                SQL = SQL + ComNum.VBLF + "              it9,  ";
                SQL = SQL + ComNum.VBLF + "              it10,  ";
                SQL = SQL + ComNum.VBLF + "              it11,  ";
                SQL = SQL + ComNum.VBLF + "              it12, ";
                SQL = SQL + ComNum.VBLF + "              it13,  ";
                SQL = SQL + ComNum.VBLF + "              it121, ";
                SQL = SQL + ComNum.VBLF + "              it150, ";
                SQL = SQL + ComNum.VBLF + "              it274, ";
                SQL = SQL + ComNum.VBLF + "              it14 ";//   'SPO2
                SQL = SQL + ComNum.VBLF + "FROM ";
                SQL = SQL + ComNum.VBLF + "( ";
                SQL = SQL + ComNum.VBLF + "     SELECT A.CHARTDATE, A.CHARTTIME,  ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it3') AS it3,  ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it4') AS it4,  ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it5') AS it5,  ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it6') AS it6,   ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it7') AS it7,  ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it8') AS it8,  ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it9') AS it9,  ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it10') AS it10,  ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it11') AS it11,  ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it12') AS it12, ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it13') AS it13,  ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it121') AS it121, ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it150') AS it150, ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it274') AS it274, ";
                SQL = SQL + ComNum.VBLF + "                  extractValue(chartxml, '//it14') AS it14 ";//   'SPO2
                SQL = SQL + ComNum.VBLF + "        FROM ADMIN.EMRXML A, ADMIN.EMRXMLMST B";
                SQL = SQL + ComNum.VBLF + "      WHERE A.EMRNO = B.EMRNO ";
                SQL = SQL + ComNum.VBLF + "          AND B.FORMNO = 1562 ";
                SQL = SQL + ComNum.VBLF + "          AND B.PTNO = '" + strPano +  "'  ";
                SQL = SQL + ComNum.VBLF + "          AND B.CHARTDATE = '"  + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "'";

                #region 신규
             
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "      SELECT CHARTDATE, CHARTTIME,   ";
                SQL = SQL + ComNum.VBLF + "                   it3.ITEMVALUE   AS it3  ,  ";
                SQL = SQL + ComNum.VBLF + "                   it4.ITEMVALUE   AS it4  ,  ";
                SQL = SQL + ComNum.VBLF + "                   it5.ITEMVALUE   AS it5  ,  ";
                SQL = SQL + ComNum.VBLF + "                   it6.ITEMVALUE   AS it6  ,   ";
                SQL = SQL + ComNum.VBLF + "                   it7.ITEMVALUE   AS it7  ,  ";
                SQL = SQL + ComNum.VBLF + "                   it8.ITEMVALUE   AS it8  ,  ";
                SQL = SQL + ComNum.VBLF + "                   it9.ITEMVALUE   AS it9  ,  ";
                SQL = SQL + ComNum.VBLF + "                   it10.ITEMVALUE  AS it10,  ";
                SQL = SQL + ComNum.VBLF + "                   it11.ITEMVALUE  AS it11,  ";
                SQL = SQL + ComNum.VBLF + "                   it12.ITEMVALUE  AS it12, ";
                SQL = SQL + ComNum.VBLF + "                   it13.ITEMVALUE  AS it13,  ";
                SQL = SQL + ComNum.VBLF + "                   it121.ITEMVALUE AS it121, ";
                SQL = SQL + ComNum.VBLF + "                   it150.ITEMVALUE AS it150, ";
                SQL = SQL + ComNum.VBLF + "                   it274.ITEMVALUE AS it274, ";
                SQL = SQL + ComNum.VBLF + "                   it14.ITEMVALUE  AS it14";//   'SPO2
                SQL = SQL + ComNum.VBLF + "          FROM ADMIN.AEMRCHARTMST A";
                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it3";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it3.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it3.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it3.ITEMCD IN ('I0000002018') --SBP";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it4";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it4.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it4.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it4.ITEMCD IN ('I0000001765') --DBP";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it5";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it5.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it5.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it5.ITEMCD IN ('I0000037575') --혈압 측정 위치";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it6";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it6.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it6.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it6.ITEMCD IN ('I0000014815') --PR 맥박";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it7";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it7.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it7.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it7.ITEMCD IN ('I0000002009') --RR 호흡수";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it8";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it8.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it8.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it8.ITEMCD IN ('I0000001811') --BT 체온";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it9";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it9.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it9.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it9.ITEMCD IN ('I0000035464') --체온 측정위치";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it10";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it10.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it10.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it10.ITEMCD IN ('I0000000418') --체중";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it11";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it11.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it11.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it11.ITEMCD IN ('I0000000002') --신장";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it12";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it12.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it12.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it12.ITEMCD IN ('I0000018853') --복위";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it13";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it13.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it13.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it13.ITEMCD IN ('I0000029454') --FHR";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it121";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it121.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it121.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it121.ITEMCD IN ('I0000017712') --두위";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it150";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it150.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it150.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it150.ITEMCD IN ('I0000010747') --흉위";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it274";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it274.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it274.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it274.ITEMCD IN ('I0000001311') --비고";

                SQL = SQL + ComNum.VBLF + "            LEFT OUTER JOIN ADMIN.AEMRCHARTROW it14";
                SQL = SQL + ComNum.VBLF + "               ON A.EMRNO = it14.EMRNO";
                SQL = SQL + ComNum.VBLF + "              AND A.EMRNOHIS = it14.EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "              AND it14.ITEMCD IN ('I0000008708') --SpO2 (%)";

                SQL = SQL + ComNum.VBLF + "          WHERE A.PTNO = '" + strPano + "'";
                SQL = SQL + ComNum.VBLF + "            AND A.CHARTDATE = '" + ComQuery.CurrentDateTime(clsDB.DbCon, "D") + "'";
                SQL = SQL + ComNum.VBLF + "            AND A.FORMNO IN(1562, 3150)";
                #endregion
                SQL = SQL + ComNum.VBLF + ") ";

                SQL = SQL + ComNum.VBLF + "   ORDER BY CHARTDATE DESC , CHARTTIME DESC   ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssVital.ActiveSheet.RowCount = 1;
                ssVital.ActiveSheet.Cells[0, 0].Text = dt.Rows[0]["it8"].ToString().Trim();
                ssVital.ActiveSheet.Cells[0, 1].Text = dt.Rows[0]["it9"].ToString().Trim();
                ssVital.ActiveSheet.Cells[0, 2].Text = dt.Rows[0]["it10"].ToString().Trim();
                ssVital.ActiveSheet.Cells[0, 3].Text = dt.Rows[0]["it3"].ToString().Trim() + "/" + dt.Rows[0]["it4"].ToString().Trim();
                ssVital.ActiveSheet.Cells[0, 4].Text = dt.Rows[0]["it5"].ToString().Trim();
                ssVital.ActiveSheet.Cells[0, 5].Text = dt.Rows[0]["it6"].ToString().Trim();
                ssVital.ActiveSheet.Cells[0, 6].Text = dt.Rows[0]["it7"].ToString().Trim();
                ssVital.ActiveSheet.Cells[0, 7].Text = dt.Rows[0]["it11"].ToString().Trim();
                ssVital.ActiveSheet.Cells[0, 8].Text = dt.Rows[0]["it12"].ToString().Trim();
                ssVital.ActiveSheet.Cells[0, 9].Text = dt.Rows[0]["it13"].ToString().Trim();
                ssVital.ActiveSheet.Cells[0, 10].Text = dt.Rows[0]["it121"].ToString().Trim();
                ssVital.ActiveSheet.Cells[0, 11].Text = dt.Rows[0]["it150"].ToString().Trim();
                ssVital.ActiveSheet.Cells[0, 12].Text = dt.Rows[0]["it14"].ToString().Trim();
                ssVital.ActiveSheet.Cells[0, 13].Text = dt.Rows[0]["it274"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 차트에 작성된 이미지를 불러온다
        /// </summary>
        /// <param name="strEmrimageno"></param>
        /// <returns></returns>
        public static Image GetImage_EMRXMLIMAGES(string strEmrimageno)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            string strClientPath = @"C:\PSMHEXE\TextEmr\EmrOld\DownImage\";
            //string FtpPathB = "/emr1/mento/tomcat/webapps/Emr/images/mts/emrimages/";
            string FtpPathS = "/emr1/mento/tomcat/webapps/Emr/images/mts/emrimages/";
            string BImage = "";
            string SImage = "";
            

            Image pic = null;

            SQL = "";
            SQL = " SELECT  ";
            //SQL = SQL + ComNum.VBLF + "    EMRIMAGENO, EMRIMAGEFILE, IMGNO,  ";
            //SQL = SQL + ComNum.VBLF + "    EMRNO, WRITEDATE, WRITETIME,  ";
            //SQL = SQL + ComNum.VBLF + "    USEID, EMRIMAGEMERGE, BIGFILE,  ";
            //SQL = SQL + ComNum.VBLF + "    SMALLFILE, PTNO, CHARTDATE,  ";
            //SQL = SQL + ComNum.VBLF + "    CHARTTIME ";
            SQL = SQL + ComNum.VBLF + "    BIGFILE, SMALLFILE";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.EMRXMLIMAGES ";
            SQL = SQL + ComNum.VBLF + "WHERE EMRIMAGENO = " + VB.Val(strEmrimageno);

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return pic;
            }
            BImage = dt.Rows[0]["BIGFILE"].ToString().Trim();
            SImage = dt.Rows[0]["SMALLFILE"].ToString().Trim();
            dt.Dispose();
            dt = null;
            
            try
            {
                string SvrPath = "";
                string ImageName = "";
                SvrPath = ComFunc.SptChar(BImage, 0, "/") + "/" + ComFunc.SptChar(BImage, 1, "/");
                ImageName = ComFunc.SptChar(BImage, 2, "/");
                if (ImageName != "")
                {
                    
                    Ftpedt FtpedtX = new Ftpedt();
                    FtpedtX.FtpDownload("192.168.100.33", "emr", "emr1234", strClientPath + ImageName , ImageName, FtpPathS + SvrPath);
                    FtpedtX = null;
                    
                    if (File.Exists(strClientPath + ImageName) == false)
                    {
                        return pic;
                    }

                    Image img = Image.FromFile(strClientPath + ImageName);
                    int imgWidth = img.Width;
                    int imgHeight = img.Height;

                    Bitmap newImage = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);

                    Graphics myGraphics = Graphics.FromImage(newImage);
                    myGraphics.SmoothingMode = SmoothingMode.HighQuality;
                    myGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    myGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    myGraphics.DrawImage(img, 0, 0, img.Width, img.Height);

                    img.Dispose(); 
                    img = null;

                    if (newImage != null)
                    {
                        File.Delete(strClientPath + ImageName);
                        pic = (Image)newImage;
                    }
                    return pic;
                }
                else
                {
                    return pic;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return pic;
            }
            
        }

        public static Image GetImage_EMRXMLIMAGES_Ex(string strEmrimageno)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Image pic = null;

            SQL = "";
            SQL = " SELECT  ";
            //SQL = SQL + ComNum.VBLF + "    EMRIMAGENO, EMRIMAGEFILE, IMGNO,  ";
            //SQL = SQL + ComNum.VBLF + "    EMRNO, WRITEDATE, WRITETIME,  ";
            //SQL = SQL + ComNum.VBLF + "    USEID, EMRIMAGEMERGE, BIGFILE,  ";
            //SQL = SQL + ComNum.VBLF + "    SMALLFILE, PTNO, CHARTDATE,  ";
            //SQL = SQL + ComNum.VBLF + "    CHARTTIME ";
            SQL = SQL + ComNum.VBLF + "    BIGFILE, SMALLFILE";
            SQL = SQL + ComNum.VBLF + "FROM ADMIN.EMRXMLIMAGES ";
            SQL = SQL + ComNum.VBLF + "WHERE EMRIMAGENO = " + VB.Val(strEmrimageno);

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return null;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return pic;
            }
            dt.Dispose();

            IDataReader reader = null;
            OracleCommand cmd = null;

            try
            {
                SQL = "";
                SQL = " SELECT  ";
                SQL = SQL + ComNum.VBLF + "    EMRIMAGENO, EMRIMAGEMERGE";
                SQL = SQL + ComNum.VBLF + "FROM ADMIN.EMRXMLIMAGES ";
                SQL = SQL + ComNum.VBLF + "WHERE EMRIMAGENO = " + VB.Val(strEmrimageno);

                cmd = clsDB.DbCon.Con.CreateCommand();
                cmd.InitialLONGFetchSize = -1;
                cmd.CommandText = SQL;
                cmd.CommandTimeout = 30;
                reader = cmd.ExecuteReader();

                cmd.Dispose();
                cmd = null;

                if (reader == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                }

                while (reader.Read())
                {
                    byte[] byteArray = (Byte[])reader.GetValue(1);
                    using (MemoryStream memStream = new MemoryStream(byteArray))
                    {
                        pic = Image.FromStream(memStream);
                    }
                }

                reader.Dispose();

                return pic;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return pic;
            }
        }

        /// <summary>
        /// modMtsEmrQuery / READ_FM_LIMIT
        /// </summary>
        /// <param name="argPTNO"></param>
        /// <param name="argFORMNO"></param>
        /// <returns></returns>
        public static bool READ_FM_LIMIT(string argPTNO, string  strMedDrCd , string argFORMNO = "")
        {
            bool rtnVal = false;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (argFORMNO == "1680")
            {
                return false;
            }
            if (strMedDrCd != "1404")
            {
                return false;
            }

            SQL = " SELECT * ";
            SQL = SQL + ComNum.VBLF +" FROM ADMIN.EMR_PATIENTT ";
            SQL = SQL + ComNum.VBLF +" WHERE PATID = '" + argPTNO + "' ";
            SQL = SQL + ComNum.VBLF +"      AND GBFM = 'Y'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }
            if (dt.Rows.Count > 0)
            {
                rtnVal = true;
            }
            dt.Dispose();
            dt = null;

            //GbViewFMChart : 변수 선언만 되어 있고 값을 주는 부분이 없음.. 항상 false
            if (ViewFMChart(clsType.User.Sabun) == true)
            {
                rtnVal = false;
            }
            return rtnVal;
        }

        /// <summary>
        /// 개인별 EMR 옵션 등록
        /// </summary>
        /// <returns></returns>
        public static bool EmrSaveUserOption(PsmhDb pDbCon, string strOPTCD, string strOPTGB, string strOPTVALUE)
        {
            //DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRUSEROPTION";
                SQL = SQL + ComNum.VBLF + "  WHERE USEID = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "  AND OPTCD = '" + strOPTCD + "'";
                SQL = SQL + ComNum.VBLF + "  AND OPTGB = '" + strOPTGB + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO ADMIN.EMRUSEROPTION";
                SQL = SQL + ComNum.VBLF + "    (USEID, OPTCD, OPTGB, OPTVALUE, WRITEDATE, WRITETIME)";
                SQL = SQL + ComNum.VBLF + "  VALUES(";
                SQL = SQL + ComNum.VBLF + "      '" + clsType.User.IdNumber  + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTCD + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTGB + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTVALUE + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime,8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(pDbCon);
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }

        }

        /// <summary>
        /// 개인별 EMR 옵션 등록
        /// </summary>
        /// <returns></returns>
        public static bool EmrSaveUserOptionEx(PsmhDb pDbCon, string strOPTCD, string strOPTGB, string strOPTVALUE)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " DELETE";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMRUSEROPTION";
                SQL = SQL + ComNum.VBLF + "  WHERE USEID = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "  AND OPTCD = '" + strOPTCD + "'";
                SQL = SQL + ComNum.VBLF + "  AND OPTGB = '" + strOPTGB + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + " INSERT INTO ADMIN.EMRUSEROPTION";
                SQL = SQL + ComNum.VBLF + "    (USEID, OPTCD, OPTGB, OPTVALUE, WRITEDATE, WRITETIME)";
                SQL = SQL + ComNum.VBLF + "  VALUES(";
                SQL = SQL + ComNum.VBLF + "      '" + clsType.User.IdNumber + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTCD + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTGB + "',";
                SQL = SQL + ComNum.VBLF + "      '" + strOPTVALUE + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Left(strCurDateTime, 8) + "',";
                SQL = SQL + ComNum.VBLF + "      '" + VB.Right(strCurDateTime, 6) + "')";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }

        }
        
        /// <summary>
        /// 정신건강의학과 차트열람권한 체크
        /// </summary>
        /// <param name="strUseId"></param>
        /// <returns></returns>
        public static bool ViewNPChart(string strUseId)
        {
            bool rtnVal = false;            
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수

            try
            {
                //'1) 자신의 진료과가 "NP"일 경우
                if (clsType.User.DeptCode.Equals("NP"))
                {
                    rtnVal = true;
                    return rtnVal;
                }


                //'2) 소속 부서가 아래와 같을 때
                switch (clsType.User.BuseCode)
                {
                    case "078201":
                    case "077405":
                    case "011109":
                    case "044400":
                    case "044401":
                    case "044201":
                        //'의무기록실, 정신의료사회사업, 정신건강의학과, 보험심사과
                        rtnVal = true;
                        return rtnVal;
                    case "033112":
                        if (clsType.User.IsNurse.Equals("OK"))
                        {
                            rtnVal = true;
                            return rtnVal;
                        }
                        break;
                }
                

                //'3) 의무기록실에서 조회권한을 줄 때
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT NPVIEW";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMR_USERT ";
                SQL = SQL + ComNum.VBLF + " WHERE USERID = '" + strUseId + "'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                
                if (reader.HasRows && reader.Read() && reader.GetValue(0).ToString().Trim().Equals("*"))
                {
                    rtnVal = true;                        
                }
                Cursor.Current = Cursors.Default;
                reader.Dispose();
                reader = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Dispose();
                }

                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        /// <summary>
        /// 가정의학과 차트열람권한 체크
        /// </summary>
        /// <param name="strUseId"></param>
        /// <returns></returns>
        public static bool ViewFMChart(string strUseId)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                //'0) FM과 이거나 ER과장 일때
                if (clsType.User.DeptCode.Equals("FM") || clsType.User.DeptCode.Equals("ER"))
                {
                    rtnVal = true;
                    return rtnVal;
                }


                //'1) 자신의 이상엽과장일 경우
                if (clsType.User.Sabun.Equals("34626"))
                {
                    rtnVal = true;
                    return rtnVal;
                }


                //'2) 소속 부서가 아래와 같을 때
                switch (clsType.User.BuseCode)
                {
                    case "078201":
                    case "077405":
                    case "044401":
                    case "044201":
                        //'보험심사과, 의무기록실
                        rtnVal = true;
                        return rtnVal;
                }


                //'3) 의무기록실에서 조회권한을 줄 때
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT FMVIEW";
                SQL = SQL + ComNum.VBLF + "    FROM ADMIN.EMR_USERT ";
                SQL = SQL + ComNum.VBLF + " WHERE USERID = '" + strUseId + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["FMVIEW"].ToString().Trim() == "*")
                    {
                        rtnVal = true;
                    }
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }

        public static bool IsDeath(string strPano)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {                                
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PANO, TMODEL                  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.MID_SUMMARY ms    ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPano + "'      ";
                SQL = SQL + ComNum.VBLF + "   AND TMODEL = '5'                  ";
                
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = true;
                }
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }
    }
}
