using ComBase;
using ComBase.Controls;
using ComDbB;
using FarPoint.Win.Spread;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// imgcvt 함수 클래스
    /// </summary>
    public class clsImgcvt
    {
        #region 변수
        /// <summary>
        /// 페이지 번호
        /// </summary>
        public static long PageNum = 0;
        /// <summary>
        /// 폰트
        /// </summary>
        public static string FontName = "굴림체";
        /// <summary>
        /// 이미지 저장 비트맵
        /// </summary>
        public static Bitmap mBitmap = null;
        /// <summary>
        /// 이미지 저장 그래픽
        /// </summary>
        public static Graphics mGraphics = null;

        public const string strPath = @"C:\PSMHEXE\IMGCVT\";

        /// <summary>
        /// 서식지 코드
        /// </summary>
        public static string gstrFormcode = string.Empty;
        #endregion

        #region ConvertImg
        /// <summary>
        /// 이미지 변환
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strInDate"></param>
        /// <param name="strOutDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="strTREATNO"></param>
        /// <param name="strDocCode"></param>
        /// <param name="argRow"></param>
        /// <returns></returns>
        public static bool Convertimg(string strPatid, string strInDate, string strOutDate, string strClinCode, string strClass, string strTREATNO, string strDocCode, int argRow, FpSpread spd)
        {
            bool rtnVal = false;

            try
            {
                DelAllFile(@"C:\PSMHEXE\IMGCVT");
                CreateSaveFolder();

                Cursor.Current = Cursors.WaitCursor;
                //변환 목록
                int maxlistcount = 13;
                for (int i = 0; i < maxlistcount; i++)
                {
                    //if ((i + 1) != 6)
                    //    continue;

                    #region 검사변환 이미지 저장용 비트맵 삭제
                    if (mBitmap != null)
                    {
                        mBitmap.Dispose();
                        mBitmap = null;
                    }

                    if (mGraphics != null)
                    {
                        mGraphics.Dispose();
                        mGraphics = null;
                    }
                    #endregion

                    DelAllFile(@"C:\PSMHEXE\IMGCVT");
                    DelAllFile(@"C:\PSMHEXE\IMGCVT\PostScan");

                    switch (i + 1)
                    {
                        #region 검사지 결과
                        case 1:
                            PageNum = 0;

                            if (NEW_spec_SelectSpecNo(clsDB.DbCon, "", strPatid.Trim(), strInDate.Trim(), strClinCode.Trim(), strClass.Trim(), strOutDate.Trim(), strDocCode, strTREATNO))
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 11].Text = "●";
                                //}
                            }
                            else
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 11].Text = "X";
                                //}

                            }

                            break;
                        #endregion

                        #region 방사선 판독지
                        case 2:
                            PageNum = 0;

                            if (New_Xray_SelectNo(clsDB.DbCon, "", strPatid.Trim(), strInDate.Trim(), strClinCode.Trim(), strClass.Trim(), strOutDate.Trim(), strDocCode, strTREATNO))
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 9].Text = "●";
                                //}
                            }
                            else
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 9].Text = "X";
                                //}
                            }

                            break;
                        #endregion

                        #region 처방의 판독지
                        case 3:
                            PageNum = 0;

                            if (New_Xray_SelectNo_Dr(clsDB.DbCon, "", strPatid.Trim(), strInDate.Trim(), strClinCode.Trim(), strClass.Trim(), strOutDate.Trim(), strDocCode, strTREATNO))
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 10].Text = "●";
                                //}
                            }
                            else
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 10].Text = "X";
                                //}
                            }

                            break;
                        #endregion

                        #region Consult(협진) - 2008/07/01 부터 전산화함
                        case 4:
                            PageNum = 0;

                            if (New_Consult_Select(clsDB.DbCon, "", strPatid.Trim(), strInDate.Trim(), strClinCode.Trim(), strClass.Trim(), strOutDate.Trim(), strDocCode, strTREATNO))
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 12].Text = "●";
                                //}
                            }
                            else
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 12].Text = "X";
                                //}
                            }
                            break;
                        #endregion

                        #region PFT 검사결과
                        case 5:
                            PageNum = 0;

                            if (New_PFT_Select(clsDB.DbCon, "", strPatid.Trim(), strInDate.Trim(), strClinCode.Trim(), strClass.Trim(), strOutDate.Trim(), strTREATNO))
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 13].Text = "●";
                                //}
                            }
                            else
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 13].Text = "X";
                                //}
                            }

                            break;
                        #endregion

                        #region 해부병리결과지
                        case 6:
                            PageNum = 0;

                            if (New_Exam_Anat(clsDB.DbCon, "", strPatid.Trim(), strInDate.Trim(), strClinCode.Trim(), strClass.Trim(), strOutDate.Trim(), strDocCode, strTREATNO.Trim()))
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 15].Text = "●";
                                //}
                            }
                            else
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 15].Text = "X";
                                //}
                            }

                            break;
                        #endregion

                        #region 종합검증
                        case 7:
                            
                            PageNum = 0;

                            if (New_Exam_Verify(clsDB.DbCon, "", strPatid.Trim(), strInDate.Trim(), strClinCode.Trim(), strClass.Trim(), strOutDate.Trim(), strDocCode, strTREATNO.Trim()))
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 16].Text = "●";
                                //}
                            }
                            else
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 16].Text = "X";
                                //}
                            }

                            break;
                        #endregion

                        #region 내시경
                        case 8:
                            PageNum = 0;

                            if (New_Exam_Endo(clsDB.DbCon, "", strPatid.Trim(), strInDate.Trim(), strClinCode.Trim(), strClass.Trim(), strOutDate.Trim(), strDocCode, strTREATNO.Trim()))
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 17].Text = "●";
                                //}
                            }
                            else
                            {
                                ////if (spd != spdPatErr)
                                ////{
                                ////    spd.ActiveSheet.Cells[argRow, 17].Text = "X";
                                ////}
                            }

                            break;
                        #endregion

                        #region EEG(뇌파)
                        case 9:
                            PageNum = 0;

                            if (New_Exam_EEG(clsDB.DbCon, "", strPatid.Trim(), strInDate.Trim(), strClinCode.Trim(), strClass.Trim(), strOutDate.Trim(), strDocCode, strTREATNO.Trim()))
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 18].Text = "●";
                                //}
                            }
                            else
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 18].Text = "X";
                                //}
                            }

                            break;
                        #endregion

                        #region 자가약품 식별 회신서
                        case 10:
                            PageNum = 0;

                            if (New_Exam_Drug_Hoimst(clsDB.DbCon, "", strPatid.Trim(), strInDate.Trim(), strClinCode.Trim(), strClass.Trim(), strOutDate.Trim(), strDocCode, strTREATNO.Trim()))
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 19].Text = "●";
                                //}
                            }
                            else
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 19].Text = "X";
                                //}
                            }

                            break;
                        #endregion

                        #region 회신서
                        case 11:
                            PageNum = 0;

                            //if (clsImgcvt.New_Etc_Return_mst(clsDB.DbCon, strPatid.Trim(), strInDate.Trim(), strClinCode.Trim(), strClass.Trim(), strOutDate.Trim(), strDocCode, strTREATNO.Trim()))
                            //{
                            //    spdPat_Sheet1.Cells[argRow, 20].Text = "●";
                            //}
                            //else
                            //{
                            //    spdPat_Sheet1.Cells[argRow, 20].Text = "X";
                            //}
                            break;
                        #endregion

                        #region 종검 동의서
                        case 12:
                            PageNum = 0;

                            if (New_HIC_CONSENT(clsDB.DbCon, "", strPatid.Trim(), strInDate.Trim(), strClinCode.Trim(), strClass.Trim(), strOutDate.Trim(), strDocCode, strTREATNO.Trim()))
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 21].Text = "●";
                                //}
                            }
                            else
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 21].Text = "X";
                                //}
                            }

                            break;
                        #endregion

                        #region 일반 동의서
                        case 13:
                            PageNum = 0;

                            if (New_HIC_CONSENT1(clsDB.DbCon, "", strPatid.Trim(), strInDate.Trim(), strClinCode.Trim(), strClass.Trim(), strOutDate.Trim(), strDocCode, strTREATNO.Trim()))
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 21].Text = "●";
                                //}

                            }
                            else
                            {
                                //if (spd != spdPatErr)
                                //{
                                //    spd.ActiveSheet.Cells[argRow, 21].Text = "X";
                                //}
                            }

                            break;
                            #endregion
                    }

                    string[] dirs = Directory.GetFiles(@"C:\PSMHEXE\IMGCVT\", "*.tif");
                    if (dirs.Length > 0 && strTREATNO.Equals("0") == false)
                    {
                        if (string.IsNullOrWhiteSpace(strOutDate))
                        {
                            strOutDate = strInDate;
                        }

                        ADO_LabUpload(clsDB.DbCon, gstrFormcode, strTREATNO, strOutDate, (i + 1), dirs);
                        rtnVal = true;
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }


            return rtnVal;
        }

        #endregion

        #region 변환 로그

        /// <summary>
        /// 변환 로그 삭제 
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strTREATNO"></param>
        /// <param name="strGBN"></param>
        public static void DeleteCvtLog(DateTime dtpSys)
        {
            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            //OracleDataReader reader = null;
            #endregion

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                #region 쿼리
                SQL = "DELETE KOSMOS_EMR.EMR_IMG_CVT_HISTORY";
                SQL += ComNum.VBLF + "WHERE BDATE >= TO_DATE('" + dtpSys.AddDays(-30).ToString("yyyy-MM-01") + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  AND BDATE <= LAST_DAY(TO_DATE('" + dtpSys.AddDays(-30).ToString("yyyy-MM") + "', 'YYYY-MM'))";
                #endregion

                int RowAffected = 0;
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// 변환 로그 저장 
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strTREATNO"></param>
        /// <param name="strGBN"></param>
        public static void SaveCvtLog(string strPatid, string strTREATNO, string strGBN)
        {
            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            //OracleDataReader reader = null;
            #endregion

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                //bool bInsert = true;
                if (strGBN.IndexOf("에러") == -1)
                {
                    //#region 쿼리
                    //SQL = "SELECT 1 AS CNT";
                    //SQL += ComNum.VBLF + "FROM DUAL";
                    //SQL += ComNum.VBLF + "WHERE EXISTS";
                    //SQL += ComNum.VBLF + "(";
                    //SQL += ComNum.VBLF + "SELECT 1";
                    //SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_IMG_CVT_HISTORY";
                    //SQL += ComNum.VBLF + " WHERE PANO    = '" + strPatid + "'";
                    //SQL += ComNum.VBLF + "   AND TREATNO =  " + strTREATNO;
                    //SQL += ComNum.VBLF + "   AND GBN     = '" + strGBN + "'";
                    //SQL += ComNum.VBLF + "   AND BDATE = TRUNC(SYSDATE)";
                    //SQL += ComNum.VBLF + ")";
                    //#endregion

                    //SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    //if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    //{
                    //    ComFunc.MsgBox(SqlErr);
                    //    clsDB.setRollbackTran(clsDB.DbCon);
                    //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    //    return;
                    //}

                    //bInsert = !reader.HasRows;

                    //reader.Dispose();

                    //if (bInsert == false)
                    //{
                    //    clsDB.setRollbackTran(clsDB.DbCon);
                    //    return;
                    //}
                }
                
                #region 쿼리
                SQL = "INSERT INTO KOSMOS_EMR.EMR_IMG_CVT_HISTORY";
                SQL += ComNum.VBLF + "(USERID, PANO, TREATNO, GBN, BDATE, CVTDATE)";
                SQL += ComNum.VBLF + "VALUES";
                SQL += ComNum.VBLF + "(";
                SQL += ComNum.VBLF + "  '" + clsType.User.IdNumber + "'";
                SQL += ComNum.VBLF + ", '" + strPatid + "'";
                SQL += ComNum.VBLF + ", " + strTREATNO;
                SQL += ComNum.VBLF + ", '" + strGBN + "'";
                SQL += ComNum.VBLF + ", TRUNC(SYSDATE)";
                SQL += ComNum.VBLF + ", SYSDATE";
                SQL += ComNum.VBLF + ")";
                #endregion

                int RowAffected = 0;
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox( ex.Message);
            }
        }


        /// <summary>
        /// 변환 에러 로그 가져오기 
        /// </summary>
        /// <param name="StartDate"></param>
        public static long GetCvtErrLog(PsmhDb pDbCon, string StartDate)
        {
            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            long rtnVal = 0;
            #endregion

            try
            {

                #region 쿼리
                SQL += ComNum.VBLF + "SELECT SUM(COUNT(TREATNO)) CNT";
                SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_IMG_CVT_HISTORY";
                SQL += ComNum.VBLF + " WHERE CVTDATE >= TO_DATE('" + StartDate + "', 'YYYY-MM-DD HH24:MI:SS')";
                SQL += ComNum.VBLF + "   AND GBN LIKE '%에러%'";
                SQL += ComNum.VBLF + "GROUP BY USERID, PANO, TREATNO";
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = (long) (VB.Val(reader.GetValue(0).ToString().Trim()));
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }
        #endregion

        #region 파일 삭제
        /// <summary>
        /// 경로안에 모든 파일 삭제
        /// </summary>
        /// <param name="FolderPath">폴더경로</param>
        /// <returns></returns>
        public static bool DelAllFile(string FolderPath)
        {
            bool rtnVal = false;
            try
            {
                if (Directory.Exists(FolderPath))
                {
                    foreach (string s in Directory.GetFiles(FolderPath))
                    {
                        File.Delete(s);
                    }
                }

                rtnVal = true;
            }
            catch { }

            return rtnVal;
        }

        #endregion

        #region 폴더 생성
        /// <summary>
        /// 업로드 이미지 저장 폴더 생성
        /// </summary>
        public static void CreateSaveFolder()
        {
            string strPath = @"C:\PSMHEXE\IMGCVT\";
            //if (Directory.Exists(strPath))
            //{
            //    clsScan.DeleteFoldAll(strPath);
            //}

            if (Directory.Exists(strPath) == false)
            {
                if (Directory.Exists(strPath) == false)
                {
                    Directory.CreateDirectory(strPath);
                }

                if (Directory.Exists(@"C:\PSMHEXE\IMGCVT\PostScan") == false)
                {
                    Directory.CreateDirectory(@"C:\PSMHEXE\IMGCVT\PostScan");
                }
            }

            if (Directory.Exists("c:\\cmc\\ocsexe\\dif\\") == false)
            {
                Directory.CreateDirectory("c:\\cmc\\ocsexe\\dif");
            }
        }
        #endregion

        #region 스프레드 => 이미지
        /// <summary>
        /// 스프레드 => 이미지
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="spd">스프레드</param>
        /// <param name="strPatid">환자 등록번호</param>
        /// <param name="strDate">입원일자</param>
        /// <param name="strDeptCode">입원과</param>
        /// <param name="strFormCode">스캔 차트에 저장할 폼코드(KOSMOS_EMR.EMR_FORMT)</param>
        /// <param name="nPageNum">페이지번호</param>
        /// <returns></returns>
        public static bool SpreadToImg(PsmhDb pDbCon, FpSpread spd, string strPatid, string strDate, string strDeptCode, string strFormCode, ref long nPageNum)
        {
            #region 폴더 생성
            CreateSaveFolder();
            #endregion

            spd.ActiveSheet.PrintInfo.UseMax = false;
            spd.ActiveSheet.PrintInfo.Centering = Centering.Vertical;
            spd.ActiveSheet.PrintInfo.Margin.Top = 30;
            spd.ActiveSheet.PrintInfo.Margin.Left = 0;
            spd.ActiveSheet.PrintInfo.Margin.Right = 0;
            spd.ActiveSheet.PrintInfo.Margin.Bottom = 0;

            #region 변수
            bool rtnVal = false;
            Rectangle PageRect = new Rectangle(0, 0, 1250, 1775);
            int PageCnt = spd.GetOwnerPrintPageCount(spd.CreateGraphics(), PageRect, spd.ActiveSheetIndex);
            strDate = strDate.Replace("-", "");
            #endregion

            try
            {
                if (mBitmap != null)
                {
                    mBitmap.Dispose();
                    mBitmap = null;
                }

                for (int i = 0; i < PageCnt; i++)
                {
                    if (i == 0)
                    {
                        spd.ActiveSheet.PrintInfo.Centering = Centering.Both;
                    }
                    else
                    {
                        spd.ActiveSheet.PrintInfo.Centering = Centering.None;
                    }

                    mBitmap = new Bitmap(1250, 1775);

                    using (Graphics g = Graphics.FromImage(mBitmap))
                    {
                        PageRect = new Rectangle(0, 0, 1250, 1775);
                        g.Clear(Color.White);
                        spd.OwnerPrintDraw(g, PageRect, spd.ActiveSheetIndex, (i + 1));
                        //TifSave(strPath + strPatid + "_" + strDate + "_" + strDeptCode + "_" + strFormCode + "_" + nPageNum.ToString("0000") + ".tif", true);
                        SaveJpeg(strPath + strPatid + "_" + strDate + "_" + strDeptCode + "_" + strFormCode + "_" + nPageNum.ToString("0000") + ".tif", mBitmap, 50);
                    }

                    nPageNum += 1;
                }

                rtnVal = true;
            }
            catch(Exception ex)
            {
                if (mBitmap != null)
                {
                    mBitmap.Dispose();
                    mBitmap = null;
                }

                if (mGraphics != null)
                {
                    mGraphics.Dispose();
                    mGraphics = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog("SpreadToImg\r\n" + ex.Message + "\r\n스프레드를 이미로 저장하는 도중 에러 발생", "", pDbCon);
            }

            return rtnVal;
        }


        public static bool SpreadToImg2(PsmhDb pDbCon, FpSpread spd, string strPatid, string strDate, string strDeptCode, string strFormCode, ref long nPageNum)
        {
            //SpreadToImg => A4 세로
            //SpreadToImg2 => A4 가로
            //사용중 특별한 문제 없으면 인자값 추가하여 SpreadToImg와 합칠 예정(2021-01-29)

            #region 폴더 생성
            CreateSaveFolder();
            #endregion

            spd.ActiveSheet.PrintInfo.UseMax = false;
            spd.ActiveSheet.PrintInfo.Centering = Centering.Vertical;
            spd.ActiveSheet.PrintInfo.Margin.Top = 5;
            spd.ActiveSheet.PrintInfo.Margin.Left = 5;
            spd.ActiveSheet.PrintInfo.Margin.Right = 0;
            spd.ActiveSheet.PrintInfo.Margin.Bottom = 0;

            #region 변수
            bool rtnVal = false;
            Rectangle PageRect = new Rectangle(0, 0, 1775, 1250);
            int PageCnt = spd.GetOwnerPrintPageCount(spd.CreateGraphics(), PageRect, spd.ActiveSheetIndex);
            strDate = strDate.Replace("-", "");
            #endregion

            try
            {
                if (mBitmap != null)
                {
                    mBitmap.Dispose();
                    mBitmap = null;
                }

                for (int i = 0; i < PageCnt; i++)
                {
                    if (i == 0)
                    {
                        spd.ActiveSheet.PrintInfo.Centering = Centering.Both;
                    }
                    else
                    {
                        spd.ActiveSheet.PrintInfo.Centering = Centering.None;
                    }

                    mBitmap = new Bitmap(1775, 1250);

                    using (Graphics g = Graphics.FromImage(mBitmap))
                    {
                        PageRect = new Rectangle(0, 0, 1775, 1250);
                        g.Clear(Color.White);
                        spd.OwnerPrintDraw(g, PageRect, spd.ActiveSheetIndex, (i + 1));
                        //TifSave(strPath + strPatid + "_" + strDate + "_" + strDeptCode + "_" + strFormCode + "_" + nPageNum.ToString("0000") + ".tif", true);
                        SaveJpeg(strPath + strPatid + "_" + strDate + "_" + strDeptCode + "_" + strFormCode + "_" + nPageNum.ToString("0000") + ".tif", mBitmap, 50);
                    }

                    nPageNum += 1;
                }

                rtnVal = true;
            }
            catch (Exception ex)
            {
                if (mBitmap != null)
                {
                    mBitmap.Dispose();
                    mBitmap = null;
                }

                if (mGraphics != null)
                {
                    mGraphics.Dispose();
                    mGraphics = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog("SpreadToImg\r\n" + ex.Message + "\r\n스프레드를 이미로 저장하는 도중 에러 발생", "", pDbCon);
            }

            return rtnVal;
        }

        #endregion

        #region 해당 내원내역 정보
        /// <summary>
        /// 해당 내원내역 정보 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano">등록번호</param>
        /// <param name="strClass">입원(I), 외래(O)</param>
        /// <param name="strInDate">입원일/내원일</param>
        /// <param name="strDeptCode">입원과</param>
        /// <returns></returns>
        public static void GetPatIpdInfo(PsmhDb pDbCon, string strPano, string strClass, string strInDate, string strDeptCode, ref string Treatno, ref string OutDate)
        {
            string SQL = string.Empty;
            OracleDataReader reader = null;

            SQL = "SELECT TREATNO, OUTDATE";
            SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMR_TREATT A";
            SQL += ComNum.VBLF + "   WHERE A.PATID = '" + strPano + "'";
            SQL += ComNum.VBLF + "     AND A.CLASS = '" + strClass + "'";
            if (strClass.Equals("O"))
            {
                SQL += ComNum.VBLF + "     AND A.CLINCODE = '" + strDeptCode + "'";
            }
            SQL += ComNum.VBLF + "     AND A.INDATE = '" + strInDate.Replace("-", "") + "'";
            SQL += ComNum.VBLF + "     AND DELDATE IS NULL";

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return;
            }

            if (reader.HasRows && reader.Read())
            {
                Treatno = reader.GetValue(0).ToString().Trim();
                OutDate = reader.GetValue(1).ToString().Trim();
            }

            reader.Dispose();

            return;
        }
        #endregion

        #region 해당 내원내역에 스캔 서식지 마지막 PAGE번호 가져오기
        /// <summary>
        /// 해당 내원내역에 스캔 서식지 마지막 PAGE번호 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPano">등록번호</param>
        /// <param name="strClass">입원(I), 외래(O)</param>
        /// <param name="strInDate">입원일/내원일</param>
        /// <param name="strDeptCode">입원과</param>
        /// <param name="strFormCode">기록지 폼코트(KOSMOS_EMR.EMR_FORMT) 참고</param>
        /// <param name="defaultPageNo">기본 페이지 번호 기본값 0</param>
        /// <returns></returns>
        public static long GetFormPageNo(PsmhDb pDbCon, string strPano, string strClass, string strInDate, string strDeptCode, string strFormCode, long defaultPageNo = 0)
        {
            long rtnVal = defaultPageNo;
            string SQL = string.Empty;
            OracleDataReader reader = null;

            SQL = "SELECT MAX(PAGE) + 1 AS MAXPAGE";
            SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMR_TREATT A";
            SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET B";
            SQL += ComNum.VBLF + "     ON A.TREATNO = B.TREATNO";
            SQL += ComNum.VBLF + "    AND B.FORMCODE = '" + strFormCode + "'";
            SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMR_PAGET C";
            SQL += ComNum.VBLF + "     ON C.PAGENO = B.PAGENO";
            SQL += ComNum.VBLF + "   WHERE A.PATID = '" + strPano + "'";
            SQL += ComNum.VBLF + "     AND A.CLASS = '" + strClass + "'";
            SQL += ComNum.VBLF + "     AND A.CLINCODE = '" + strDeptCode + "'";
            SQL += ComNum.VBLF + "     AND A.INDATE = '" + strInDate.Replace("-", "") + "'";

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = long.Parse(reader.GetValue(0).ToString().Trim());
            }

            reader.Dispose();

            return rtnVal;
        }
        #endregion
        


        #region NST용 
        public struct NSTInfo
        {
            /// <summary>
            /// KOSMOS_EMR.EMR_TREATT 조인용도
            /// </summary>
            public static string TREATNO;
            /// <summary>
            /// 협진, 의뢰서 2개
            /// </summary>
            public static string GBN;
            /// <summary>
            /// 의뢰일자
            /// </summary>
            public static string REQUESTDATE;
        }
        #endregion

        #region NST 변환내역 삭제(재업로드 용도)
        /// <summary>
        /// NST 변환내역 삭제(재업로드 용도)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strTREATNO">내원 시퀀스번호</param>
        /// <param name="strBdate">의뢰일자</param>
        /// <returns></returns>
        public static bool DelNSTCvt(PsmhDb pDbCon, string strTREATNO, string strBdate)
        {

            #region 변수
            string strdates = ComQuery.CurrentDateTime(pDbCon, "D");
            string strtimes = ComQuery.CurrentDateTime(pDbCon, "T");
            bool rtnVal = false;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            OracleDataReader reader = null;

            strBdate = strBdate.Length > 10 ? strBdate.Substring(0, 10) : strBdate;
            #endregion

            clsDB.setBeginTran(pDbCon);
            try
            {
                #region 변환전 EMR_PRINTNEEDT_BACKUP로 백업 
                SQL = "INSERT INTO KOSMOS_EMR.EMR_PRINTNEEDT_BACKUP(PAGENO, PRINTCODE, CDATE, CUSERID, NEEDNAME, NEEDJUMINNO, NEEDJOIN, NEEDADDR, NEEDGUBUN, PRINTED, TREATNO, NEEDCNT)";
                SQL += ComNum.VBLF + "SELECT PAGENO, PRINTCODE, CDATE, CUSERID, NEEDNAME, NEEDJUMINNO, NEEDJOIN, NEEDADDR, NEEDGUBUN, PRINTED, TREATNO, NEEDCNT";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMR_PRINTNEEDT A";
                SQL += ComNum.VBLF + "WHERE PAGENO IN";
                SQL += ComNum.VBLF + "  (";
                SQL += ComNum.VBLF + "SELECT A.PAGENO";
                SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_NST_CVT_HISTORY A";
                SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET B";
                SQL += ComNum.VBLF + "       ON A.TREATNO = B.TREATNO";
                SQL += ComNum.VBLF + "      AND B.FORMCODE = '109'";
                SQL += ComNum.VBLF + "      AND A.PAGENO = B.PAGENO";
                SQL += ComNum.VBLF + " WHERE A.TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "   AND A.REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  )";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region DELETE
                SQL = "DELETE FROM KOSMOS_EMR.EMR_PRINTNEEDT";
                SQL += ComNum.VBLF + " WHERE PAGENO IN";
                SQL += ComNum.VBLF + "(";
                SQL += ComNum.VBLF + "SELECT A.PAGENO";
                SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_NST_CVT_HISTORY A";
                SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET B";
                SQL += ComNum.VBLF + "       ON A.TREATNO = B.TREATNO";
                SQL += ComNum.VBLF + "      AND B.FORMCODE = '109'";
                SQL += ComNum.VBLF + "      AND A.PAGENO = B.PAGENO";
                SQL += ComNum.VBLF + " WHERE A.TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "   AND A.REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + ")";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region INSERT EMR_DELETEPAGET
                SQL = "INSERT INTO KOSMOS_EMR.EMR_DELETEPAGET(CUSERID, PAGENO, PATID, INDATE, CLINCODE, CLASS, DOCCODE, FORMCODE, PAGE, CDATE, CTIME, TREATNO)" + " ";
                SQL += ComNum.VBLF + "SELECT 'TrsConsult', C.PAGENO, T.PATID, T.INDATE, T.CLINCODE, T.CLASS, T.DOCCODE, C.FORMCODE, C.PAGE" + " ";
                SQL += ComNum.VBLF + "," + strdates + " ," + strtimes + " , T.TREATNO" + " ";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMR_NST_CVT_HISTORY A";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET C";
                SQL += ComNum.VBLF + "     ON A.PAGENO = C.PAGENO";
                SQL += ComNum.VBLF + "    AND A.TREATNO = C.TREATNO";
                SQL += ComNum.VBLF + "    AND C.FORMCODE = '109'";
                SQL += ComNum.VBLF + "    AND C.CUSERID = 'TrsConsult'";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMR_TREATT T";
                SQL += ComNum.VBLF + "     ON A.TREATNO = T.TREATNO";
                SQL += ComNum.VBLF + "WHERE A.TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "  AND A.REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region INSERT EMR_PAGEDELLOGT
                SQL = "INSERT INTO KOSMOS_EMR.EMR_PAGEDELLOGT(CUSERID, PAGENO, PATID, INDATE, CLINCODE, CLASS, DOCCODE, FORMCODE, PAGE, CDATE, CTIME)" + " ";
                SQL += ComNum.VBLF + "SELECT 'TrsConsult', C.PAGENO, T.PATID, T.INDATE, T.CLINCODE, T.CLASS, T.DOCCODE, C.FORMCODE, C.PAGE" + " ";
                SQL += ComNum.VBLF + "," + strdates + ", " + strtimes + "" + " ";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMR_NST_CVT_HISTORY A";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET C";
                SQL += ComNum.VBLF + "     ON A.PAGENO = C.PAGENO";
                SQL += ComNum.VBLF + "    AND A.TREATNO = C.TREATNO";
                SQL += ComNum.VBLF + "    AND C.FORMCODE = '109'";
                SQL += ComNum.VBLF + "    AND C.CUSERID = 'TrsConsult'";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMR_TREATT T";
                SQL += ComNum.VBLF + "     ON A.TREATNO = T.TREATNO";
                SQL += ComNum.VBLF + "WHERE A.TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "  AND A.REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region SELECT => UPDATE
                SQL = "SELECT CDNO   ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMR_CHARTPAGET";
                SQL += ComNum.VBLF + "WHERE TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "  AND FORMCODE = '109'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SQL = "UPDATE KOSMOS_EMR.EMR_CDINFOT SET DIRTY = '1'" + " " +
                              "WHERE CDNO ='" + reader.GetValue(0).ToString().Trim() + "'";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            clsDB.setRollbackTran(pDbCon);
                            return rtnVal;
                        }
                    }
                }

                reader.Dispose();
                #endregion

                SQL = "SELECT CLASS,CLINCODE,INDATE,PATID,OUTDATE FROM KOSMOS_EMR.EMR_TREATT WHERE TREATNO = " + strTREATNO;

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    return rtnVal;
                }

                reader.Dispose();

                #region EMR_CHARTPAGET DELETE
                SQL = "DELETE FROM KOSMOS_EMR.EMR_CHARTPAGET";
                SQL += ComNum.VBLF + "WHERE TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "  AND FORMCODE = '109'";
                SQL += ComNum.VBLF + "  AND CUSERID  = 'TrsConsult'";
                SQL += ComNum.VBLF + "  AND PAGENO IN";
                SQL += ComNum.VBLF + "  (";
                SQL += ComNum.VBLF + "    SELECT A.PAGENO";
                SQL += ComNum.VBLF + "      FROM KOSMOS_EMR.EMR_NST_CVT_HISTORY A";
                SQL += ComNum.VBLF + "        INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET B";
                SQL += ComNum.VBLF + "           ON A.TREATNO = B.TREATNO";
                SQL += ComNum.VBLF + "          AND B.FORMCODE = '109'";
                SQL += ComNum.VBLF + "          AND A.PAGENO = B.PAGENO";
                SQL += ComNum.VBLF + "     WHERE A.TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "       AND A.REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  )";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region EMR_NST_CVT_HISTORY DELETE
                SQL = "DELETE FROM KOSMOS_EMR.EMR_NST_CVT_HISTORY";
                SQL += ComNum.VBLF + "WHERE TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "  AND REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                clsDB.setCommitTran(pDbCon);
                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                clsDB.setRollbackTran(pDbCon);
            }

            return rtnVal;
        }
        #endregion

        #region NST 변환 내역 여부 확인
        /// <summary>
        /// NST 변환 점검함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strTREATNO">내원 시퀀스번호</param>
        /// <param name="strBdate">의뢰일자</param>
        /// <returns></returns>
        public static bool IsNSTCvt(PsmhDb pDbCon, string strTREATNO, string strBdate)
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            OracleDataReader reader = null;

            if (string.IsNullOrWhiteSpace(strTREATNO))
                return rtnVal;

            strBdate = strBdate.Length > 10 ? strBdate.Substring(0, 10) : strBdate;

            SQL = "SELECT 1 AS CNT";
            SQL += ComNum.VBLF + "FROM DUAL";
            SQL += ComNum.VBLF + "WHERE EXISTS";
            SQL += ComNum.VBLF + "(";
            SQL += ComNum.VBLF + "SELECT 1";
            SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_NST_CVT_HISTORY A";
            SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET B";
            SQL += ComNum.VBLF + "       ON A.TREATNO = B.TREATNO";
            SQL += ComNum.VBLF + "      AND B.FORMCODE = '109'";
            SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.EMR_PAGET C";
            SQL += ComNum.VBLF + "       ON C.PAGENO = B.PAGENO";
            SQL += ComNum.VBLF + " WHERE A.TREATNO = " + strTREATNO;
            SQL += ComNum.VBLF + "   AND A.REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";
            SQL += ComNum.VBLF + ")";


            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows)
            {
                rtnVal = true;
            }

            reader.Dispose();

            return rtnVal;
        }
        #endregion



        #region DRUG 복약안내문용 
        public struct DRUGInfo
        {
            /// <summary>
            /// KOSMOS_EMR.EMR_TREATT 조인용도
            /// </summary>
            public static string TREATNO;
            /// <summary>
            /// 협진, 의뢰서 2개
            /// </summary>
            public static string GBN;
            /// <summary>
            /// 의뢰일자
            /// </summary>
            public static string REQUESTDATE;
            /// <summary>
            /// 약품명
            /// </summary>
            public static string DRUGNAME;
        }
        #endregion

        #region DRUG 복약안내문 변환내역 삭제(재업로드 용도)
        /// <summary>
        /// NST 변환내역 삭제(재업로드 용도)
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strTREATNO">내원 시퀀스번호</param>
        /// <param name="strBdate">의뢰일자</param>
        /// <returns></returns>
        public static bool DelDRUGCvt(PsmhDb pDbCon, string strTREATNO, string strBdate, string strDrugName)
        {

            #region 변수
            string strdates = ComQuery.CurrentDateTime(pDbCon, "D");
            string strtimes = ComQuery.CurrentDateTime(pDbCon, "T");
            bool rtnVal = false;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            OracleDataReader reader = null;

            strBdate = strBdate.Length > 10 ? strBdate.Substring(0, 10) : strBdate;
            #endregion

            clsDB.setBeginTran(pDbCon);
            try
            {
                #region 변환전 EMR_PRINTNEEDT_BACKUP로 백업 
                SQL = "INSERT INTO KOSMOS_EMR.EMR_PRINTNEEDT_BACKUP(PAGENO, PRINTCODE, CDATE, CUSERID, NEEDNAME, NEEDJUMINNO, NEEDJOIN, NEEDADDR, NEEDGUBUN, PRINTED, TREATNO, NEEDCNT)";
                SQL += ComNum.VBLF + "SELECT PAGENO, PRINTCODE, CDATE, CUSERID, NEEDNAME, NEEDJUMINNO, NEEDJOIN, NEEDADDR, NEEDGUBUN, PRINTED, TREATNO, NEEDCNT";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMR_PRINTNEEDT A";
                SQL += ComNum.VBLF + "WHERE PAGENO IN";
                SQL += ComNum.VBLF + "  (";
                SQL += ComNum.VBLF + "SELECT A.PAGENO";
                SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_DRUG_CVT_HISTORY A";
                SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET B";
                SQL += ComNum.VBLF + "       ON A.TREATNO = B.TREATNO";
                SQL += ComNum.VBLF + "      AND B.FORMCODE = '154'";
                SQL += ComNum.VBLF + "      AND A.PAGENO = B.PAGENO";
                SQL += ComNum.VBLF + " WHERE A.TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "   AND A.DRUGNAME = '" + strDrugName + "'";
                SQL += ComNum.VBLF + "   AND A.REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  )";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region DELETE
                SQL = "DELETE FROM KOSMOS_EMR.EMR_PRINTNEEDT";
                SQL += ComNum.VBLF + " WHERE PAGENO IN";
                SQL += ComNum.VBLF + "(";
                SQL += ComNum.VBLF + "SELECT A.PAGENO";
                SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_DRUG_CVT_HISTORY A";
                SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET B";
                SQL += ComNum.VBLF + "       ON A.TREATNO = B.TREATNO";
                SQL += ComNum.VBLF + "      AND B.FORMCODE = '154'";
                SQL += ComNum.VBLF + "      AND A.PAGENO = B.PAGENO";
                SQL += ComNum.VBLF + " WHERE A.TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "   AND A.DRUGNAME = '" + strDrugName + "'";
                SQL += ComNum.VBLF + "   AND A.REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + ")";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region INSERT EMR_DELETEPAGET
                SQL = "INSERT INTO KOSMOS_EMR.EMR_DELETEPAGET(CUSERID, PAGENO, PATID, INDATE, CLINCODE, CLASS, DOCCODE, FORMCODE, PAGE, CDATE, CTIME, TREATNO)" + " ";
                SQL += ComNum.VBLF + "SELECT 'TrsConsult', C.PAGENO, T.PATID, T.INDATE, T.CLINCODE, T.CLASS, T.DOCCODE, C.FORMCODE, C.PAGE" + " ";
                SQL += ComNum.VBLF + "," + strdates + " ," + strtimes + " , T.TREATNO" + " ";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMR_DRUG_CVT_HISTORY A";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET C";
                SQL += ComNum.VBLF + "     ON A.PAGENO = C.PAGENO";
                SQL += ComNum.VBLF + "    AND A.TREATNO = C.TREATNO";
                SQL += ComNum.VBLF + "    AND C.FORMCODE = '154'";
                SQL += ComNum.VBLF + "    AND C.CUSERID = 'TrsConsult'";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMR_TREATT T";
                SQL += ComNum.VBLF + "     ON A.TREATNO = T.TREATNO";
                SQL += ComNum.VBLF + "WHERE A.TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "  AND A.DRUGNAME = '" + strDrugName + "'";
                SQL += ComNum.VBLF + "  AND A.REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region INSERT EMR_PAGEDELLOGT
                SQL = "INSERT INTO KOSMOS_EMR.EMR_PAGEDELLOGT(CUSERID, PAGENO, PATID, INDATE, CLINCODE, CLASS, DOCCODE, FORMCODE, PAGE, CDATE, CTIME)" + " ";
                SQL += ComNum.VBLF + "SELECT 'TrsConsult', C.PAGENO, T.PATID, T.INDATE, T.CLINCODE, T.CLASS, T.DOCCODE, C.FORMCODE, C.PAGE" + " ";
                SQL += ComNum.VBLF + "," + strdates + ", " + strtimes + "" + " ";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMR_DRUG_CVT_HISTORY A";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET C";
                SQL += ComNum.VBLF + "     ON A.PAGENO = C.PAGENO";
                SQL += ComNum.VBLF + "    AND A.TREATNO = C.TREATNO";
                SQL += ComNum.VBLF + "    AND C.FORMCODE = '154'";
                SQL += ComNum.VBLF + "    AND C.CUSERID = 'TrsConsult'";
                SQL += ComNum.VBLF + "  INNER JOIN KOSMOS_EMR.EMR_TREATT T";
                SQL += ComNum.VBLF + "     ON A.TREATNO = T.TREATNO";
                SQL += ComNum.VBLF + "WHERE A.TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "  AND A.DRUGNAME = '" + strDrugName + "'";
                SQL += ComNum.VBLF + "  AND A.REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region SELECT => UPDATE
                SQL = "SELECT CDNO   ";
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMR_CHARTPAGET";
                SQL += ComNum.VBLF + "WHERE TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "  AND FORMCODE = '154'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SQL = "UPDATE KOSMOS_EMR.EMR_CDINFOT SET DIRTY = '1'" + " " +
                              "WHERE CDNO ='" + reader.GetValue(0).ToString().Trim() + "'";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            clsDB.setRollbackTran(pDbCon);
                            return rtnVal;
                        }
                    }
                }

                reader.Dispose();
                #endregion

                SQL = "SELECT CLASS,CLINCODE,INDATE,PATID,OUTDATE FROM KOSMOS_EMR.EMR_TREATT WHERE TREATNO = " + strTREATNO;

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    return rtnVal;
                }

                reader.Dispose();

                #region EMR_CHARTPAGET DELETE
                SQL = "DELETE FROM KOSMOS_EMR.EMR_CHARTPAGET";
                SQL += ComNum.VBLF + "WHERE TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "  AND FORMCODE = '154'";
                SQL += ComNum.VBLF + "  AND CUSERID  = 'TrsConsult'";
                SQL += ComNum.VBLF + "  AND PAGENO IN";
                SQL += ComNum.VBLF + "  (";
                SQL += ComNum.VBLF + "    SELECT A.PAGENO";
                SQL += ComNum.VBLF + "      FROM KOSMOS_EMR.EMR_DRUG_CVT_HISTORY A";
                SQL += ComNum.VBLF + "        INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET B";
                SQL += ComNum.VBLF + "           ON A.TREATNO = B.TREATNO";
                SQL += ComNum.VBLF + "          AND B.FORMCODE = '154'";
                SQL += ComNum.VBLF + "          AND A.PAGENO = B.PAGENO";
                SQL += ComNum.VBLF + "     WHERE A.TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "       AND A.DRUGNAME = '" + strDrugName + "'";
                SQL += ComNum.VBLF + "       AND A.REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  )";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region EMR_NST_CVT_HISTORY DELETE
                SQL = "DELETE FROM KOSMOS_EMR.EMR_DRUG_CVT_HISTORY";
                SQL += ComNum.VBLF + "WHERE TREATNO = " + strTREATNO;
                SQL += ComNum.VBLF + "  AND DRUGNAME = '" + strDrugName + "'";
                SQL += ComNum.VBLF + "  AND REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                clsDB.setCommitTran(pDbCon);
                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                clsDB.setRollbackTran(pDbCon);
            }

            return rtnVal;
        }
        #endregion

        #region DRUG 복약안내문 변환 내역 여부 확인
        /// <summary>
        /// NST 변환 점검함수
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strTREATNO">내원 시퀀스번호</param>
        /// <param name="strBdate">의뢰일자</param>
        /// <returns></returns>
        public static bool IsDRUGCvt(PsmhDb pDbCon, string strTREATNO, string strBdate, string strDrugName)
        {
            bool rtnVal = false;
            string SQL = string.Empty;
            OracleDataReader reader = null;

            if (string.IsNullOrWhiteSpace(strTREATNO))
                return rtnVal;

            strBdate = strBdate.Length > 10 ? strBdate.Substring(0, 10) : strBdate;

            SQL = "SELECT 1 AS CNT";
            SQL += ComNum.VBLF + "FROM DUAL";
            SQL += ComNum.VBLF + "WHERE EXISTS";
            SQL += ComNum.VBLF + "(";
            SQL += ComNum.VBLF + "SELECT 1";
            SQL += ComNum.VBLF + "  FROM KOSMOS_EMR.EMR_DRUG_CVT_HISTORY A";
            SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.EMR_CHARTPAGET B";
            SQL += ComNum.VBLF + "       ON A.TREATNO = B.TREATNO";
            //현재는 입원환자만 변환. 외래도 사용할 경우 STRINOUTCLS = 'O' => 155 사용해야함
            SQL += ComNum.VBLF + "      AND B.FORMCODE = '154'";
            SQL += ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.EMR_PAGET C";
            SQL += ComNum.VBLF + "       ON C.PAGENO = B.PAGENO";
            SQL += ComNum.VBLF + " WHERE A.TREATNO = " + strTREATNO;
            SQL += ComNum.VBLF + "   AND A.DRUGNAME = '" + strDrugName + "'";
            SQL += ComNum.VBLF + "   AND A.REQUESTDATE = TO_DATE('" + strBdate + "', 'YYYY-MM-DD')";
            SQL += ComNum.VBLF + ")";


            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows)
            {
                rtnVal = true;
            }

            reader.Dispose();

            return rtnVal;
        }
        #endregion



        #region READ_PassName
        /// <summary>
        /// 이름 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argSABUN"></param>
        /// <returns></returns>
        public static string READ_PassName(PsmhDb pDbCon, string argSABUN)
        {
            string rtnVal = string.Empty;
            string SQL = string.Empty;

            SQL = "SELECT Name";
            SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PASS ";
            SQL += ComNum.VBLF + "WHERE IDnumber=" + argSABUN;
            SQL += ComNum.VBLF + "  AND PrograMid = ' '";

            OracleDataReader reader = null;

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();

            return rtnVal;
        }
        #endregion

        #region ADO_LabUpload
        /// <summary>
        /// 이미지 업로드
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="crtFormCode">폼코드(서식지 번호)</param>
        /// <param name="strTREATNO">내원번호</param>
        /// <param name="strOutDate">퇴원날짜</param>
        /// <param name="ArgJob">i값</param>
        /// <param name="sDir">경로 배열</param>
        /// <param name="Delete">기존 내원 내역안에 있는 해당 서식지들 모두 삭제 여부(기본값 = True)</param>
        /// <param name="Log">히스토리 저장 여부 기본값 False</param>
        /// <returns></returns>
        public static bool ADO_LabUpload(PsmhDb pDbCon, string crtFormCode, string strTREATNO, string strOutDate, int ArgJob, string[] sDir, bool Delete = true, bool Log = false)
        {
            #region 변수
            bool rtnVal = false;
            long nFileSize = 0;
            long nPage = 0;
            string strPAGE = string.Empty;
            string REMOTE_PATH = string.Empty;
            string strPathID = string.Empty;
            string strRemotePath = string.Empty;
            string strLocation = string.Empty;
            string strCUserID = !Delete ? "TrsConsult" : "Trans";

            if (Delete == true && (ArgJob == 7 || ArgJob >= 9))
            {
                strCUserID = "Trans" + ArgJob;
            }

            if (string.IsNullOrWhiteSpace(strPathID))
            {
                strPathID = GetTreatNoToPathInfo(strTREATNO);  //clsPath. SelectAll
            }

            OracleDataReader reader = null;
            #endregion

            if (string.IsNullOrWhiteSpace(strTREATNO))
                return rtnVal;

            //string strPath = @"C:\Program Files\BitNixChart\IMG";
            Ftpedt ftpedt = new Ftpedt();
            string strIp = string.Empty;
            string SQL = "SELECT  ipaddress ,pathport , localpath,FTPUSER, FTPPASSWD   FROM KOSMOS_EMR.EMR_PATHT WHERE PATHID ='" + strPathID + "'";
            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                strRemotePath = reader.GetValue(2).ToString().Trim();
                strIp = reader.GetValue(0).ToString().Trim();
                if (ftpedt.FtpConnetBatch(strIp, reader.GetValue(3).ToString().Trim(), reader.GetValue(4).ToString().Trim()) == false)
                {
                    reader.Dispose();
                    ftpedt.FtpDisConnetBatch();
                    ftpedt.Dispose();
                    ComFunc.MsgBox("서버 접속 실패!");
                    return rtnVal;
                }
            }

            reader.Dispose();

            string istrFormCode = string.Empty;
            string[] FORMCODE;

            if (Delete == true)
            {
                #region 차트삭제?
                for (int i = 0; i < sDir.Length; i++)
                {
                    FORMCODE = sDir[i].Split('_');
                    crtFormCode = FORMCODE[3].Trim();

                    if (crtFormCode.Equals("019") == false)
                    {
                        if (ADO_DeleteChart(pDbCon, strTREATNO, crtFormCode, strCUserID) == false)
                        {
                            return rtnVal;
                        }
                        istrFormCode = string.Empty;
                    }
                    else if(string.IsNullOrWhiteSpace(istrFormCode))
                    {
                        if (ADO_DeleteChart(pDbCon, strTREATNO, crtFormCode, strCUserID) == false)
                        {
                            return rtnVal;
                        }
                        istrFormCode = "019";
                    }
                }
                #endregion
            }

            #region 차트 업로드?
            for (int i = 0; i < sDir.Length; i++)
            {
                FORMCODE = sDir[i].Split('_');
                crtFormCode = FORMCODE[3].Trim();

                FileInfo fileInfo = new FileInfo(sDir[i]);
                string CryptFile = fileInfo.FullName.Replace(fileInfo.Extension, ".env");

                clsCyper.Encrypt(sDir[i], CryptFile);

                sDir[i] = CryptFile;

                fileInfo = new FileInfo(CryptFile);
                nFileSize = fileInfo.Length;

                #region 이전로직
                //if (ADO_InsertImage(pDbCon, ref nPage, strTREATNO, crtFormCode, strPathID, strCUserID, nFileSize, "tif", strOutDate, ref strLocation, Log) == false)
                //{
                //    return rtnVal;
                //}
                #endregion


                #region 신규로직
                if (ADO_InsertImage(pDbCon, ref nPage, strTREATNO, crtFormCode, strPathID, strCUserID, nFileSize, "env", strOutDate, ref strLocation, Log) == false)
                {
                    return rtnVal;
                }
                #endregion

                #region 스캔된 이미지 파일을 sftsvr에 업로드 한다.
                if (nPage < 1000)
                {
                    strPAGE = VB.Val(nPage.ToString()).ToString("0000");
                }
                else
                {
                    strPAGE = VB.Right(nPage.ToString(), 4);
                }

                string strServerPath = string.Empty;
                #region 이전로직
                //if (strIp.Equals("192.168.100.33"))
                //{
                //    strServerPath = strRemotePath.Replace("\\", "/") + "/" + strLocation;
                //    REMOTE_PATH = strRemotePath.Replace("\\", "/") + "/" + strLocation + "/" + nPage + ".tif";
                //}
                //else
                //{
                //    strServerPath = strRemotePath;
                //    REMOTE_PATH = strRemotePath + @"/" + strPAGE + "/" + nPage + ".tif";
                //}
                #endregion

                #region 신규로직
                if (strIp.Equals("192.168.100.33"))
                {
                    strServerPath = strRemotePath.Replace("\\", "/") + "/" + strLocation;
                    REMOTE_PATH = strRemotePath.Replace("\\", "/") + "/" + strLocation + "/" + nPage + ".env";
                }
                else
                {
                    strServerPath = strRemotePath;
                    REMOTE_PATH = strRemotePath + @"/" + strPAGE + "/" + nPage + ".env";
                }
                #endregion

                if (ftpedt.FtpUploadBatch(sDir[i], REMOTE_PATH, strServerPath) == false)
                {
                    ComFunc.MsgBox("생성된 서식지를 서버에 저장하는데 실패하였습니다.\r\n다시 변환 버튼을 눌러 주십시요");
                    return rtnVal;
                }

                #endregion

                //istrFormCode = string.Empty;
                
            }
            #endregion

            rtnVal = true;
            return rtnVal;
        }

        public static string GetTreatNoToPathInfo(string TreatNo)
        {
            string rtnVal = string.Empty;

            return TreatNoToIndate(clsDB.DbCon, TreatNo).To<int>() < 2020 ? "0001" : "0003";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string TreatNoToIndate(PsmhDb pDbCon, string TreatNo)
        {
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            DataTable dt = null;

            SQL = "";
            SQL = "SELECT ";
            SQL = SQL + ComNum.VBLF + "     INDATE";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMR_TREATT";
            SQL = SQL + ComNum.VBLF + "WHERE TREATNO = " + TreatNo;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return string.Empty;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                return "2020";
            }

            string rtnVal = dt.Rows[0]["INDATE"].ToString().Trim().Substring(0, 4);
            dt.Dispose();

            return rtnVal;
        }

        public static bool ADO_DeleteChart(PsmhDb pDbCon, string nTreatNo, string strFormCode, string strUser)
        {
            #region 변수
            string strdates = ComQuery.CurrentDateTime(pDbCon, "D");
            string strtimes = ComQuery.CurrentDateTime(pDbCon, "T");
            bool rtnVal = false;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            OracleDataReader reader = null;
            #endregion

            clsDB.setBeginTran(pDbCon);
            try
            {
                #region 변환전 EMR_PRINTNEEDT_BACKUP로 백업 
                SQL = "INSERT INTO KOSMOS_EMR.EMR_PRINTNEEDT_BACKUP(PAGENO, PRINTCODE, CDATE, CUSERID, NEEDNAME, NEEDJUMINNO, NEEDJOIN, NEEDADDR, NEEDGUBUN, PRINTED, TREATNO, NEEDCNT, CVTSABUN, CVTDATE)";
                SQL += ComNum.VBLF + "SELECT PAGENO, PRINTCODE, CDATE, CUSERID, NEEDNAME, NEEDJUMINNO, NEEDJOIN, NEEDADDR, NEEDGUBUN, PRINTED, TREATNO, NEEDCNT, '" + clsType.User.IdNumber + "', SYSDATE" ;
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMR_PRINTNEEDT A";
                SQL += ComNum.VBLF + "WHERE PAGENO IN";
                SQL += ComNum.VBLF + "  (";
                SQL += ComNum.VBLF + "  SELECT PAGENO";
                SQL += ComNum.VBLF + "    FROM KOSMOS_EMR.EMR_CHARTPAGET";
                SQL += ComNum.VBLF + "   WHERE TREATNO  = " + nTreatNo;
                SQL += ComNum.VBLF + "     AND FORMCODE = '" + strFormCode + "'";
                SQL += ComNum.VBLF + "  )";
                SQL += ComNum.VBLF + "  AND NOT EXISTS";
                SQL += ComNum.VBLF + "  (";
                SQL += ComNum.VBLF + "  SELECT 1";
                SQL += ComNum.VBLF + "    FROM KOSMOS_EMR.EMR_PRINTNEEDT_BACKUP";
                SQL += ComNum.VBLF + "   WHERE PAGENO  = A.PAGENO";
                SQL += ComNum.VBLF + "     AND PRINTCODE = A.PRINTCODE";
                SQL += ComNum.VBLF + "     AND CDATE = A.CDATE";
                SQL += ComNum.VBLF + "     AND CUSERID = A.CUSERID";
                SQL += ComNum.VBLF + "  )";
                //SQL += ComNum.VBLF + "  AND PRINTED = 'Y'";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region DELETE
                SQL = "DELETE FROM KOSMOS_EMR.EMR_PRINTNEEDT WHERE PAGENO IN" + " " + 
                   ComNum.VBLF + "(SELECT PAGENO FROM KOSMOS_EMR.EMR_CHARTPAGET WHERE TREATNO = " + nTreatNo + " AND FORMCODE = '" + strFormCode + "')" + " ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region INSERT EMR_DELETEPAGET
                SQL = "INSERT INTO KOSMOS_EMR.EMR_DELETEPAGET(CUSERID, PAGENO, PATID, INDATE, CLINCODE, CLASS, DOCCODE, FORMCODE, PAGE, CDATE, CTIME, TREATNO)" + " ";
                SQL += ComNum.VBLF + "SELECT '" + strUser + "', C.PAGENO, T.PATID, T.INDATE, T.CLINCODE, T.CLASS, T.DOCCODE, C.FORMCODE, C.PAGE" + " ";
                SQL += ComNum.VBLF + "," + strdates + " ," + strtimes + " , T.TREATNO" + " ";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMR_CHARTPAGET C, KOSMOS_EMR.EMR_TREATT T" + " ";
                SQL += ComNum.VBLF + "WHERE C.TREATNO = " + nTreatNo + "  AND C.FORMCODE = '" + strFormCode + "' AND T.TREATNO = C.TREATNO" + " ";
                SQL += ComNum.VBLF + "  AND C.CUSERID = '" + strUser + "'";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region INSERT EMR_PAGEDELLOGT
                SQL = "INSERT INTO KOSMOS_EMR.EMR_PAGEDELLOGT(CUSERID, PAGENO, PATID, INDATE, CLINCODE, CLASS, DOCCODE, FORMCODE, PAGE, CDATE, CTIME)" + " ";
                SQL += ComNum.VBLF + "SELECT '" + strUser + "', C.PAGENO, T.PATID, T.INDATE, T.CLINCODE, T.CLASS, T.DOCCODE, C.FORMCODE, C.PAGE" + " ";
                SQL += ComNum.VBLF + "," + strdates + ", " + strtimes + "" + " ";
                SQL += ComNum.VBLF + "FROM KOSMOS_EMR.EMR_CHARTPAGET C, KOSMOS_EMR.EMR_TREATT T" + " ";
                SQL += ComNum.VBLF + "WHERE C.TREATNO = " + nTreatNo + "  AND C.FORMCODE = '" + strFormCode + "' AND T.TREATNO = C.TREATNO" + " ";
                SQL += ComNum.VBLF + "  AND C.CUSERID = '" + strUser + "'";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region SELECT => UPDATE
                SQL = "SELECT CDNO FROM KOSMOS_EMR.EMR_CHARTPAGET WHERE TREATNO = " + nTreatNo + " AND FORMCODE = '" + strFormCode + "'" + " " +
                      " GROUP BY CDNO" + " ";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        SQL = "UPDATE KOSMOS_EMR.EMR_CDINFOT SET DIRTY = '1'" + " " +
                              "WHERE CDNO ='" + reader.GetValue(0).ToString().Trim() + "'";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            clsDB.setRollbackTran(pDbCon);
                            return rtnVal;
                        }
                    }
                }

                reader.Dispose();
                #endregion

                SQL = "SELECT CLASS,CLINCODE,INDATE,PATID,OUTDATE FROM KOSMOS_EMR.EMR_TREATT WHERE TREATNO = " + nTreatNo;

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }
                

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    return rtnVal;
                }

                reader.Dispose();

                #region DELETE
                SQL = "DELETE FROM KOSMOS_EMR.EMR_CHARTPAGET" + " " +
                ComNum.VBLF + "WHERE TREATNO = " + nTreatNo + " AND FORMCODE = '" + strFormCode + "'  AND CUSERID ='" + strUser + "'" + " ";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                #region UPDATE
                SQL = "UPDATE KOSMOS_EMR.EMR_CHARTPAGET " + "   SET CDNO = '00000'  WHERE TREATNO = '" + nTreatNo + "'";

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }
                #endregion

                clsDB.setCommitTran(pDbCon);
                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                clsDB.setRollbackTran(pDbCon);
            }

            return rtnVal;
        }


        public static bool ADO_InsertImage(PsmhDb pDbCon, ref long nPageNo, string ntreat, string strFormCode, string strPathID, string strUserID, long nFileSize, string strExten, string strOutDate, ref string strLocation, bool Log)
        {
            #region 변수
            string strdates = ComQuery.CurrentDateTime(pDbCon, "D");
            string strtimes = ComQuery.CurrentDateTime(pDbCon, "T");
            bool rtnVal = false;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            OracleDataReader reader = null;
            #endregion

            clsDB.setBeginTran(pDbCon);
            try
            {
                #region SQL
                SQL = "SELECT KOSMOS_EMR.SEQ_PAGENO.NEXTVAL SEQ_PAGENO FROM DUAL";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }


                if (reader.HasRows && reader.Read())
                {
                    nPageNo = (long) VB.Val(reader.GetValue(0).ToString().Trim());
                }

                reader.Dispose();
                #endregion

                if (strOutDate.Equals("20041231"))
                {
                    strLocation = VB.Left(strOutDate, 4) + "/" + strOutDate + "/" + VB.Right(nPageNo.ToString(), 4);
                }
                else if (DateTime.ParseExact(strOutDate, "yyyyMMdd", null) <= DateTime.ParseExact("20040101", "yyyyMMdd", null))
                {
                    strLocation = VB.Left(strOutDate, 4) + "/" + strOutDate + "/" + VB.Right(nPageNo.ToString(), 2);
                }
                else
                {
                    strLocation = VB.Left(strOutDate, 4) + "/" + strOutDate + "/" + VB.Right(nPageNo.ToString(), 1);
                }

                #region CHARTPAGET에 INSERT 
                SQL = "INSERT INTO KOSMOS_EMR.EMR_PAGET(PAGENO ,PATHID, CDATE, CUSERID, FILESIZE, EXTENSION, LOCATION) " +
                      "VALUES (" + nPageNo + ", '" + strPathID + "'," + strdates + ",'" + strUserID + "', " + nFileSize + ",'" + strExten + "' ,'" + strLocation + "')";
                
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }

                #endregion


                #region SQL
                SQL = "SELECT NVL(MAX(PAGE), 0) + 1  PAGE   FROM KOSMOS_EMR.EMR_CHARTPAGET ";
                SQL += ComNum.VBLF + " WHERE TREATNO = " + ntreat + " AND FORMCODE = '" + strFormCode + "'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }


                if (reader.HasRows && reader.Read())
                {
                    SQL = "INSERT INTO KOSMOS_EMR.EMR_CHARTPAGET(PAGENO, TREATNO, FORMCODE, PAGE, CDATE, CUSERID) VALUES(" + 
                    nPageNo + ", " + ntreat + ", '" + strFormCode + "' , " + reader.GetValue(0).ToString().Trim() + ", '" + strdates + "', '" + strUserID + "') ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        clsDB.setRollbackTran(pDbCon);
                        return rtnVal;
                    }

                    #region 로그 저장
                    if (RowAffected > 0 && Log)
                    {
                        #region 쿼리
                        if (string.IsNullOrWhiteSpace(NSTInfo.GBN) == false)
                        {
                            SQL = "INSERT INTO KOSMOS_EMR.EMR_NST_CVT_HISTORY";
                            SQL += ComNum.VBLF + "(USERID, PANO, TREATNO, PAGE, PAGENO, GBN, REQUESTDATE, CVTDATE)";
                            SQL += ComNum.VBLF + "SELECT ";
                            SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";
                            SQL += ComNum.VBLF + "PATID,";
                            SQL += ComNum.VBLF + "TREATNO,";
                            SQL += ComNum.VBLF + reader.GetValue(0).ToString().Trim() + ",";
                            SQL += ComNum.VBLF + nPageNo + ",";
                            SQL += ComNum.VBLF + "'" + NSTInfo.GBN + "', ";
                            SQL += ComNum.VBLF + "TO_DATE('" + NSTInfo.REQUESTDATE + "', 'YYYY-MM-DD'), ";
                            SQL += ComNum.VBLF + "SYSDATE";
                            SQL += ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMR_TREATT";
                            SQL += ComNum.VBLF + "WHERE TREATNO = " + ntreat;

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                clsDB.setRollbackTran(pDbCon);
                                return rtnVal;
                            }
                        }
                        if (string.IsNullOrWhiteSpace(DRUGInfo.GBN) == false)
                        {
                            SQL = "INSERT INTO KOSMOS_EMR.EMR_DRUG_CVT_HISTORY";
                            SQL += ComNum.VBLF + "(USERID, PANO, TREATNO, PAGE, PAGENO, GBN, REQUESTDATE, CVTDATE, DRUGNAME)";
                            SQL += ComNum.VBLF + "SELECT ";
                            SQL += ComNum.VBLF + "'" + clsType.User.IdNumber + "',";
                            SQL += ComNum.VBLF + "PATID,";
                            SQL += ComNum.VBLF + "TREATNO,";
                            SQL += ComNum.VBLF + reader.GetValue(0).ToString().Trim() + ",";
                            SQL += ComNum.VBLF + nPageNo + ",";
                            SQL += ComNum.VBLF + "'" + DRUGInfo.GBN + "', ";
                            SQL += ComNum.VBLF + "TO_DATE('" + DRUGInfo.REQUESTDATE + "', 'YYYY-MM-DD'), ";
                            SQL += ComNum.VBLF + "SYSDATE,";
                            SQL += ComNum.VBLF + "'" + DRUGInfo.DRUGNAME + "' ";
                            SQL += ComNum.VBLF + " FROM " + ComNum.DB_EMR + "EMR_TREATT";
                            SQL += ComNum.VBLF + "WHERE TREATNO = " + ntreat;

                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                            if (string.IsNullOrWhiteSpace(SqlErr) == false)
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                                clsDB.setRollbackTran(pDbCon);
                                return rtnVal;
                            }
                        }
                        #endregion
                    }
                    #endregion
                }

                reader.Dispose();
                #endregion

                clsDB.setCommitTran(pDbCon);
                rtnVal = true;
            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                clsDB.setRollbackTran(pDbCon);
            }

            return rtnVal;
        }

        #endregion

        #region 종검 동의서
        public static bool New_HIC_CONSENT(PsmhDb pDbCon, string var, string strPatid, string strDate, string strClinCode, string strClass, string strOutDate, string strDocCode, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            Ftpedt ftpedt = null;

            //D10.내시경 D20.대장내시경 D30.수면내시경승낙서 D40.조영제사용동의서
            gstrFormcode = strClass.Trim().Equals("I") ? "113" : "012";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            PageNum = 0;
            #endregion

            try
            {
                CreateSaveFolder();

                #region 쿼리
                SQL = " SELECT TO_CHAR(b.SDATE,'YYYY-MM-DD') SDATE,a.PTNO,a.SNAME,b.DEPTCODE,b.PageCnt,b.formcode,b.wrtno,b.ROWID  ";
                SQL += ComNum.VBLF + "    FROM KOSMOS_PMPA.HEA_JEPSU a,KOSMOS_PMPA.HIC_CONSENT b ";
                SQL += ComNum.VBLF + "   WHERE b.SDate = TO_DATE('" + strDate + "','YYYYMMDD') ";
                SQL += ComNum.VBLF + "     AND a.PTNO = '" + strPatid + "' ";
                SQL += ComNum.VBLF + "     AND a.DelDate IS NULL ";//      '삭제된것 제외
                SQL += ComNum.VBLF + "     AND a.GbSts>='1' ";  //       '수진등록된 수검자
                SQL += ComNum.VBLF + "     AND a.PTno  = b.PTno";
                SQL += ComNum.VBLF + "     AND a.WRTNO = b.WRTNO  ";
                SQL += ComNum.VBLF + "     AND b.DRSABUN IS NOT NULL  ";//   '의사가 승인한것만
                SQL += ComNum.VBLF + "     AND B.CONSENT_TIME IS NOT NULL";
                //SQL += ComNum.VBLF + "     AND B.SendTime IS NULL"; //20-02-14 EMR전송 안한것만
                SQL += ComNum.VBLF + "     AND B.DELDATE IS NULL";
                SQL += ComNum.VBLF + "     AND b.EntDate>=TO_DATE('" + DateTime.ParseExact(strDate, "yyyyMMdd", null).AddDays(-365).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "     AND B.DEPTCODE = '" + strClinCode + "' ";
                SQL += ComNum.VBLF + "     AND B.FORMCODE NOT IN ('D50')";
                SQL += ComNum.VBLF + " ORDER BY a.SDATE DESC,a.Ptno ";
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    return rtnVal;
                }

                SaveCvtLog(strPatid, strTREATNO, "종검동의서");

                rtnVal = HIC_CONSENT_UPLOAD(pDbCon, strPatid, strDate, strClinCode, reader, "종검", strTREATNO);
                                      
                reader.Dispose();
            }
            catch (Exception ex)
            {
                if (ftpedt != null)
                {
                    ftpedt.Dispose();
                }

                //ComFunc.MsgBox(ex.Message);
                SaveCvtLog(strPatid, strTREATNO, "종검동의서 에러");
                clsDB.SaveSqlErrLog(ex.Message + "\r\n종검 동의서 업로드중 에러", "", pDbCon);
            }

            return rtnVal;
        }

        public static bool HIC_CONSENT_UPLOAD(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, OracleDataReader reader, string GBN, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            Ftpedt ftpedt = null;
            int RowAffected = 0;
            PageNum = 0;
            #endregion

            try
            {
                ftpedt = new Ftpedt();
                if (ftpedt.FtpConnetBatch("192.168.100.31", "oracle", ftpedt.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle")) == false)
                {
                    ftpedt.FtpDisConnetBatch();
                    ftpedt.Dispose();
                    ComFunc.MsgBox("서버 접속 실패!");
                    return rtnVal;
                }

                //ftpedt.SetServerDirectory("/");

                string strPathL = @"C:\PSMHEXE\IMGCVT\PostScan\";
                while (reader.Read())
                {
                    int PageCnt = (int)VB.Val(reader.GetValue(4).ToString().Trim());
                    string strWrtNo = reader.GetValue(6).ToString().Trim();
                    string strDeptCode = reader.GetValue(3).ToString().Trim();
                    string strFormCode = reader.GetValue(5).ToString().Trim();
                    string strPtno = reader.GetValue(1).ToString().Trim();
                    string strSdate = reader.GetValue(0).ToString().Trim();
                    string strRID = reader.GetValue(7).ToString().Trim();

                    for (int i = 0; i < PageCnt; i++)
                    {
                        PageNum += 1;
                        string strFile = VB.Format(VB.Val(strWrtNo), "#0") + "_" + strDeptCode + "_" + VB.Left(strFormCode, 2) + i.ToString("0") + "_1.jpg";
                        string strLocal = strPathL + strFile;
                        string strPathR = "/data/hic_result/consent_temp/" + strFile;

                        if (File.Exists(strLocal))
                        {
                            File.Delete(strLocal);
                        }

                        if (ftpedt.FtpDownloadBatch2(strLocal, strFile, "/data/hic_result/consent_temp/") == true)
                        {
                            if (File.Exists(strLocal))
                            {
                                strPathR = @"C:\PSMHEXE\IMGCVT\PostScan\" + strFile;
                                mBitmap = new Bitmap(strPathR);
                            }

                            TifSave(@"C:\PSMHEXE\IMGCVT\" + strPtno + "_" + strSdate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");
                            //mBitmap.Dispose();

                            rtnVal = true;
                        }
                        else
                        {
                            rtnVal = false;
                            clsDB.SaveSqlErrLog(GBN + strFile + "\r\n파일 다운로드 실패!", "", pDbCon);
                            SaveCvtLog(strPatid, strTREATNO, GBN + "동의서 에러");

                            //ComFunc.MsgBox("파일 다운로드 실패!");
                        }

                        if (File.Exists(strLocal))
                        {
                            File.Delete(strLocal);
                        }

                    }

                    #region EMR 전송완료 설정
                    clsDB.setBeginTran(pDbCon);
                    string SQL = "UPDATE KOSMOS_PMPA.HIC_CONSENT SET SendTime=TRUNC(SYSDATE) ";
                    SQL += ComNum.VBLF + "WHERE ROWID='" + strRID  + "' ";

                    string SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        return rtnVal;
                    }

                    clsDB.setCommitTran(pDbCon);
                    #endregion
                }

                //rtnVal = true;
                ftpedt.Dispose();
            }
            catch(Exception ex)
            {
                if (ftpedt != null)
                {
                    ftpedt.Dispose();
                }

                SaveCvtLog(strPatid, strTREATNO, GBN + "동의서 에러");
                clsDB.SaveSqlErrLog(GBN + "동의서 업로드중 에러\r\n" + ex.Message, "", pDbCon);
            }
            return rtnVal;
        }

        #endregion

        #region 일반검진 동의서
        public static bool New_HIC_CONSENT1(PsmhDb pDbCon, string var, string strPatid, string strDate, string strClinCode, string strClass, string strOutDate, string strDocCode, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            Ftpedt ftpedt = null;

            //D10.내시경 D20.대장내시경 D30.수면내시경승낙서 D40.조영제사용동의서
            gstrFormcode = strClass.Trim().Equals("I") ? "113" : "012";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            PageNum = 0;
            #endregion

            try
            {
                CreateSaveFolder();

                #region 쿼리
                SQL = " SELECT TO_CHAR(a.JEPDATE,'YYYY-MM-DD') SDATE,a.PTNO,a.SNAME,b.DEPTCODE,b.PageCnt,b.formcode,b.wrtno,b.ROWID  ";
                SQL += ComNum.VBLF + "    FROM KOSMOS_PMPA.HIC_JEPSU a,KOSMOS_PMPA.HIC_CONSENT b ";
                SQL += ComNum.VBLF + "   WHERE a.JEPDATE = TO_DATE('" + strDate + "','YYYYMMDD') ";
                SQL += ComNum.VBLF + "     AND a.PTNO = '" + strPatid + "' ";
                SQL += ComNum.VBLF + "     AND a.DelDate IS NULL ";//      '삭제된것 제외
                SQL += ComNum.VBLF + "     AND a.GbSts>='1' ";  //       '수진등록된 수검자
                SQL += ComNum.VBLF + "     AND a.PTno  = b.PTno";
                SQL += ComNum.VBLF + "     AND a.WRTNO = b.WRTNO  ";
                SQL += ComNum.VBLF + "     AND b.DRSABUN IS NOT NULL  ";//   '의사가 승인한것만
                SQL += ComNum.VBLF + "     AND B.CONSENT_TIME IS NOT NULL";
                //SQL += ComNum.VBLF + "     AND B.SendTime IS NULL"; //20-02-14 EMR전송 안한것만
                SQL += ComNum.VBLF + "     AND B.DELDATE IS NULL";
                SQL += ComNum.VBLF + "     AND b.EntDate>=TO_DATE('" + DateTime.ParseExact(strDate, "yyyyMMdd", null).AddDays(-365).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "     AND B.DEPTCODE = '" + strClinCode + "' ";
                SQL += ComNum.VBLF + "     AND A.GJJONG = '31'";
                SQL += ComNum.VBLF + "     AND B.FORMCODE NOT IN ('D50')";
                SQL += ComNum.VBLF + " ORDER BY a.JEPDATE DESC,a.Ptno ";
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    return rtnVal;
                }

                SaveCvtLog(strPatid, strTREATNO, "일반검진동의서");

                rtnVal = HIC_CONSENT_UPLOAD(pDbCon, strPatid, strDate, strClinCode, reader, "일반", strTREATNO);

                reader.Dispose();
            }
            catch (Exception ex)
            {
                if (ftpedt != null)
                {
                    ftpedt.Dispose();
                }

                //ComFunc.MsgBox(ex.Message);
                SaveCvtLog(strPatid, strTREATNO, "일반검진동의서 에러");
                clsDB.SaveSqlErrLog(ex.Message + "\r\n일반종검 동의서 업로드중 에러", "", pDbCon);
            }

            return rtnVal;
        }
        #endregion

        #region 회신서
        public static bool New_Etc_Return_mst(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string strOutDate, string strDocCode, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            gstrFormcode = strClass.Trim().Equals("I") ? "142" : "143";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            PageNum = 0;

            //string FileName = string.Empty;
            string strPtno = string.Empty;
            //string strPath = string.Empty;
            //string strPathB = string.Empty;
            //string strPathR = string.Empty;
            strOutDate = string.IsNullOrWhiteSpace(strOutDate) ? ComQuery.CurrentDateTime(pDbCon, "S").Substring(0, 10) : strOutDate;
            #endregion

            try
            {
                #region 쿼리
                SQL = "  SELECT B.ROWID, B.BIGO ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_RETURN B ";
                SQL += ComNum.VBLF + " WHERE B.PANO = '" + strPatid + "' ";

                if (strClass.Equals("I"))
                {
                    SQL += ComNum.VBLF + " AND B.OPDIPD = '1'";
                }
                else
                {
                    SQL += ComNum.VBLF + " AND (B.OPDIPD = '2' OR B.OPDIPD = '3') ";
                }

                SQL += ComNum.VBLF + "   AND B.BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')  ";
                SQL += ComNum.VBLF + "   AND B.DEPTCODE = '" + strClinCode + "' ";
                //'=============================================================================
                SQL += ComNum.VBLF + "   AND ( B.GbSend = 'Y' OR  B.GbSend = '' )";
                //SQL += ComNum.VBLF + "   AND ( B.GbPrePrint = 'Y'  )";//                '회신서 발급여부 확인(2020-02-27 제외처리함)
                SQL += ComNum.VBLF + "   AND ( B.NUR_CHK IS NULL OR B.NUR_CHK = 'N')";//    '2019-01-09 담당자 작성한 내용은 변환되지 않도록.
                SQL += ComNum.VBLF + " ORDER BY B.ActDATE DESC";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    return rtnVal;
                }

                while (reader.Read())
                {
                    PageNum += 1;

                    New_Etc_Return_mst_DTL(pDbCon, reader.GetValue(0).ToString().Trim(), reader.GetValue(1).ToString().Trim(), strDate, strClinCode, strPatid);
                }

                reader.Dispose();
                #endregion

                rtnVal = true;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        public static void New_Etc_Return_mst_DTL(PsmhDb pDbCon, string ArgRowid, string ArgBigo, string strDate, string strClinCode, string strPatid)
        {
            #region 변수
            //int LNGY = 40;
            int lngLine = 0;
            //int PAGENO = 0;
            string[] strResult;
            //string[] strResult1;
            string strTResult = string.Empty;

            string sRname = string.Empty;
            string sRdRname = string.Empty;
            string sSname   = string.Empty;
            string sJumin   = string.Empty;
            string sJuso    = string.Empty;
            string sPano    = string.Empty;
            string sOpdIpd  = string.Empty;
            string sBdate = string.Empty;
            string sRequest       = string.Empty;
            string sDrName        = string.Empty;
            string sDrCode        = string.Empty;
            string sRequestDate   = string.Empty;
            string sActDate       = string.Empty;
            string sJindan        = string.Empty;
            string sInDate        = string.Empty;
            string sDeptCode      = string.Empty;
            string sDrBunho       = string.Empty;
            string sTelno = string.Empty;

            DataTable dt = null;
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            #endregion

            New_initFormRetrun();

            if (ArgBigo.Equals("경과회신서"))
            {
                #region 쿼리
                SQL = " SELECT B.RNAME, B.RDRNAME, A.SNAME, A.JUMIN1, A.JUMIN2, A.JUSO, B.PANO, B.OPDIPD, B.BDATE, ";
                SQL += ComNum.VBLF + " B.REQUEST , B.DrName, B.DrCode, B.REQUESTDATE, B.ACTDATE, A.ZIPCODE1, A.ZIPCODE2, B.JINDAN ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT A, KOSMOS_PMPA.ETC_RETURN B";
                SQL += ComNum.VBLF + " WHERE A.PANO = B.PANO ";
                SQL += ComNum.VBLF + "   AND ( b.GbSend ='Y' OR  b.GbSend ='' )";
                SQL += ComNum.VBLF + "   AND B.ROWID = '" + ArgRowid + "' ";
                SQL += ComNum.VBLF + " ORDER BY ACTDATE DESC";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    sRname = dt.Rows[0][ "RNAME"].ToString().Trim();//         '의뢰병원
                    sRdRname = dt.Rows[0][ "RDRNAME"].ToString().Trim();//     '의뢰의사명
                    sSname = dt.Rows[0][ "SNAME"].ToString().Trim();//         '성명
                    sJumin = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1) + "******";//        '주민등록번호
                    sPano = dt.Rows[0][ "PANO"].ToString().Trim();//           '병록번호
                    sBdate = dt.Rows[0][ "BDATE"].ToString().Trim();//         '환자내원일
                    sDrCode = dt.Rows[0][ "DRCODE"].ToString().Trim();//       '의사코드
                    sRequestDate = dt.Rows[0][ "REQUESTDATE"].ToString().Trim();//     '회신 작성일자
                    sActDate = dt.Rows[0][ "ACTDATE"].ToString().Trim();//             '의뢰일자
                    sJindan = dt.Rows[0]["JINDAN"].ToString().Trim();// '진단명


                    //'주소 완성용
                    SQL = " SELECT ZIPNAME1, ZIPNAME2, ZIPNAME3 ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_ZIPSNEW ";
                    SQL += ComNum.VBLF + " WHERE ZIPCODE1 = '" + dt.Rows[0]["ZIPCODE1"].ToString().Trim() + "' " ;
                    SQL += ComNum.VBLF + "   AND ZIPCODE2 = '" + dt.Rows[0]["ZIPCODE2"].ToString().Trim() + "' ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (string.IsNullOrEmpty(SqlErr) == false)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        return;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        sJuso = " " + reader.GetValue(0).ToString().Trim() + " " +
                                      reader.GetValue(1).ToString().Trim() + " " +
                                      reader.GetValue(2).ToString().Trim() + " " +
                                      dt.Rows[0]["JUSO"].ToString().ToString().Trim();
                    }
                    else
                    {
                        sJuso = dt.Rows[0]["JUSO"].ToString().Trim(); //'주소(우편번호로 주소 완성)
                    }

                    reader.Dispose();

                    #region 입원, 외래, 응급 구분용
                    if (dt.Rows[0]["OPDIPD"].ToString().Trim().Equals("1"))
                    {
                        sOpdIpd = "■입원 □외래 □응급";
                    }
                    else if (dt.Rows[0]["OPDIPD"].ToString().Trim().Equals("2"))
                    {
                        sOpdIpd = "□입원 ■외래 □응급";
                    }
                    else if (dt.Rows[0]["OPDIPD"].ToString().Trim().Equals("3"))
                    {
                        sOpdIpd = "□입원 □외래 ■응급";
                    }
                    #endregion

                    #region 의사정보 완성용
                    SQL = " SELECT A.DRNAME, A.DRDEPT1, A.TELNO, B.DRBUNHO ";
                    SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_DOCTOR A, KOSMOS_OCS.OCS_DOCTOR B ";
                    SQL += ComNum.VBLF + "  WHERE A.DRCODE = B.DRCODE ";
                    SQL += ComNum.VBLF + "    AND A.DRCODE = '" + sDrCode + "' ";


                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (string.IsNullOrEmpty(SqlErr) == false)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        return;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        sDeptCode = clsVbfunc.GetBASClinicDeptNameK(pDbCon, reader.GetValue(1).ToString().Trim()) + "(" + reader.GetValue(1).ToString().Trim() + ")";//      '진료과
                        sDrName = reader.GetValue(0).ToString().Trim();//          '담당의사
                        sDrBunho = reader.GetValue(3).ToString().Trim();// '면허번호
                        sTelno = "054-" + reader.GetValue(2).ToString().Trim();// '외래전화
                    }
                    else
                    {
                        reader.Dispose();
                        return;
                    }

                    reader.Dispose();

                    #endregion

                    #region 회신서 정보 완성용
                    WriteStr(35, 600, 80, "의뢰환자 경과회신서");

                    WriteStr(25, 70, 170, sRname);//             '의뢰병원
                    if (dt.Rows[0]["RDRNAME"].ToString().Trim() != "") // '의뢰의사명
                    {
                        WriteStr(25, 70, 205, sRdRname + " 원장님(선생님) 귀하");
                    }
                    else
                    {
                        WriteStr(25, 70, 205, "         원장님(선생님) 귀하");
                    }
                    WriteStr(25, 70, 240, "환자를 의뢰해 주셔서 감사합니다.");


                    WriteStr(25, 125, 1190, "회      신");//
                    WriteStr(25, 125, 1290, "안      내");//
                    WriteStr(25, 415, 620, "환자내원일");//


                    WriteStr(25, 650, 320,  sSname  );//         '성명
                    WriteStr(25, 1200, 320, sJumin  );//        '주민등록번호
                    WriteStr(25, 650, 420,  sJuso   );//         '주소
                    WriteStr(25, 650, 520,  sPano   );//         '병록번호
                    WriteStr(25, 650, 620,  sBdate  );//         '환자내원일
                    WriteStr(25, 1200, 520, sOpdIpd );//        '진료구분
                    WriteStr(20, 1200, 620, sInDate); //        '입원기간


                    WriteStr(25, 400, 725, sJindan);         //  '상병명
                    //'frmImgConvert.ltkPV.TextOut FontName, 25, 400, 810, TextColor, BackColor, sRequest         '회신서 내용


                    WriteStr(25, 305, 1950, sDeptCode);//      '진료과
                    WriteStr(25, 805, 1950, sDrName);//       '담당의사
                    WriteStr(25, 1340, 1950, sDrBunho);//       '면허번호


                    WriteStr(25, 305, 2000, sTelno);//외래전화
                    WriteStr(25, 805, 2000, sRequestDate);//     '작성일자


                    #region 회신서 내용 완성용
                    strTResult = dt.Rows[0]["REQUEST"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                    strResult = strTResult.Split('\n');
                    #endregion


                    for (int K = 0; K < strResult.Length; K++)
                    {
                        if (string.IsNullOrWhiteSpace(strResult[K]))
                        {
                            lngLine += 1;
                        }
                        else
                        {

                            int strByte = Encoding.Default.GetBytes(strResult[K]).Length;
                            for (double b = 1; b < Math.Truncate((double)strByte / 75 + 0.9) + 1; b++)
                            {
                                WriteStr(20, 400, 810 + (lngLine * 34), VB.Mid(strResult[K], 1 + 65 * (int)(b - 1), 65));

                                lngLine += 1;

                                if (lngLine > 37)
                                {
                                    TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                                    //LNGY = 40;
                                    lngLine = 1;
                                    PageNum += 1;

                                    New_initFormRetrun();

                                    WriteStr(35, 600, 80, "의뢰환자 경과회신서");

                                    WriteStr(25, 70, 170, sRname);//             '의뢰병원
                                    if (dt.Rows[0]["RDRNAME"].ToString().Trim() != "") // '의뢰의사명
                                    {
                                        WriteStr(25, 70, 205, sRdRname + " 원장님(선생님) 귀하");
                                    }
                                    else
                                    {
                                        WriteStr(25, 70, 205, "         원장님(선생님) 귀하");
                                    }
                                    WriteStr(25, 70, 240, "환자를 의뢰해 주셔서 감사합니다.");


                                    WriteStr(25, 125, 1190, "회      신");//
                                    WriteStr(25, 125, 1290, "안      내");//
                                    WriteStr(25, 415, 620, "환자내원일");//


                                    WriteStr(25, 650, 320, sSname);//         '성명
                                    WriteStr(25, 1200, 320, sJumin);//        '주민등록번호
                                    WriteStr(25, 650, 420, sJuso);//         '주소
                                    WriteStr(25, 650, 520, sPano);//         '병록번호
                                    WriteStr(25, 650, 620, sBdate);//         '환자내원일
                                    WriteStr(25, 1200, 520, sOpdIpd);//        '진료구분
                                    WriteStr(20, 1200, 620, sInDate); //        '입원기간


                                    WriteStr(25, 400, 725, sJindan);         //  '상병명
                                                                             //'frmImgConvert.ltkPV.TextOut FontName, 25, 400, 810, TextColor, BackColor, sRequest         '회신서 내용


                                    WriteStr(25, 305, 1950, sDeptCode);//      '진료과
                                    WriteStr(25, 805, 1950, sDrName);//       '담당의사
                                    WriteStr(25, 1340, 1950, sDrBunho);//       '면허번호


                                    WriteStr(25, 305, 2000, sTelno);//외래전화
                                    WriteStr(25, 805, 2000, sRequestDate);//     '작성일자
                                }
                            }
                        }
                    }

                    #endregion


                }
                else
                {
                    dt.Dispose();
                    return;
                }

                dt.Dispose();
            }
            //기존 진료소견
            else
            {
                #region 쿼리
                SQL = " SELECT A.MCCLASS, A.PTNO, A.MCNO, A.JUMIN1, A.JUMIN2, A.SNAME, A.SEX, A.POSTCODE1, A.POSTCODE2, A.JUSO, B.RName, B.RDrName, B.H_Code, ";
                SQL += ComNum.VBLF + "  A.TEL, A.DIAGONOSIS, A.GUBUN, A.OPINION, A.LicenseNo, A.DrName, A.DEPTCODE, A.DrCode, A.SDate, A.Ilsu, A.RTel, ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.LSDATE,'YYYY-MM-DD') LSDATE, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE, ";
                SQL += ComNum.VBLF + "  TO_CHAR(A.IDate1,'YYYY/MM/DD') IDate1, TO_CHAR(A.IDate2,'YYYY/MM/DD') IDate2, A.OPINION2 , TO_CHAR(B.ACTDATE,'YYYY-MM-DD') ACTDATE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_MCCERTIFI12 A, KOSMOS_PMPA.ETC_RETURN B";
                SQL += ComNum.VBLF + "  WHERE A.Ptno=b.Pano(+)";
                SQL += ComNum.VBLF + "    AND ( B.GbSend ='Y' OR  B.GbSend ='' ) ";
                SQL += ComNum.VBLF + "    AND B.ROWID ='" + ArgRowid + "' ";
                SQL += ComNum.VBLF + "    AND A.BDATE= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " AND A.RET_NEW IS NULL ";
                SQL += ComNum.VBLF + " ORDER BY B.ACTDATE DESC";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    sRname = dt.Rows[0]["RNAME"].ToString().Trim();//         '의뢰병원
                    sRdRname = dt.Rows[0]["RDRNAME"].ToString().Trim();//     '의뢰의사명
                    sSname = dt.Rows[0]["SNAME"].ToString().Trim();//         '성명
                    sJumin = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1) + "******";//        '주민등록번호
                    sPano = dt.Rows[0]["PTNO"].ToString().Trim();//           '병록번호
                    sBdate = dt.Rows[0]["BDATE"].ToString().Trim();//         '환자내원일
                    sDrCode = dt.Rows[0]["DRCODE"].ToString().Trim();//       '의사코드
                    sRequestDate = dt.Rows[0]["LSDATE"].ToString().Trim();//     '회신 작성일자
                    sActDate = dt.Rows[0]["ACTDATE"].ToString().Trim();//             '의뢰일자
                    sJindan = dt.Rows[0]["DIAGONOSIS"].ToString().Trim();// '진단명
                    sDrName = dt.Rows[0]["DRNAME"].ToString().Trim();// '의사명


                    //'주소 완성용
                    SQL = " SELECT ZIPNAME1, ZIPNAME2, ZIPNAME3 ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_ZIPSNEW ";
                    SQL += ComNum.VBLF + " WHERE ZIPCODE1 = '" + dt.Rows[0]["POSTCODE1"].ToString().Trim() + "' ";
                    SQL += ComNum.VBLF + "   AND ZIPCODE2 = '" + dt.Rows[0]["POSTCODE2"].ToString().Trim() + "' ";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (string.IsNullOrEmpty(SqlErr) == false)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        return;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        sJuso = " " + reader.GetValue(0).ToString().Trim() + " " +
                                      reader.GetValue(1).ToString().Trim() + " " +
                                      reader.GetValue(2).ToString().Trim() + " " +
                                      dt.Rows[0]["JUSO"].ToString().ToString().Trim();
                    }
                    else
                    {
                        sJuso = dt.Rows[0]["JUSO"].ToString().Trim(); //'주소(우편번호로 주소 완성)
                    }

                    reader.Dispose();

                    #region 입원, 외래, 응급 구분용
                    if (dt.Rows[0]["GUBUN"].ToString().Trim().Equals("1"))
                    {
                        sOpdIpd = "■입원 □외래 □응급";
                    }
                    else if (dt.Rows[0]["GUBUN"].ToString().Trim().Equals("2"))
                    {
                        sOpdIpd = "□입원 ■외래 □응급";
                    }
                    else if (dt.Rows[0]["GUBUN"].ToString().Trim().Equals("3"))
                    {
                        sOpdIpd = "□입원 □외래 ■응급";
                    }
                    #endregion


                    #region 입원기간
                    if (dt.Rows[0]["GUBUN"].ToString().Trim().Equals("1"))
                    {
                        if (dt.Rows[0]["IDate1"].ToString().Trim() != "" && dt.Rows[0]["IDate2"].ToString().Trim() != "" && dt.Rows[0]["Ilsu"].ToString().Trim() != "")
                        {
                            sInDate = dt.Rows[0]["IDate1"].ToString().Trim() + " ~ " + dt.Rows[0]["IDate2"].ToString().Trim();
                        }
                        else if (dt.Rows[0]["IDate1"].ToString().Trim() != "" && dt.Rows[0]["IDate2"].ToString().Trim() != "")
                        {
                            sInDate = dt.Rows[0]["IDate1"].ToString().Trim() + " ~ ";
                        }
                    }
                    else
                    {
                        sInDate = dt.Rows[0]["Ilsu"].ToString().Trim();

                    }
                    #endregion

                    #region 의사정보 완성용
                    SQL = " SELECT A.DRNAME, A.DRDEPT1, A.TELNO, B.DRBUNHO ";
                    SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_DOCTOR A, KOSMOS_OCS.OCS_DOCTOR B ";
                    SQL += ComNum.VBLF + "  WHERE A.DRCODE = B.DRCODE ";
                    SQL += ComNum.VBLF + "    AND B.DRNAME = '" + sDrName + "' ";


                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (string.IsNullOrEmpty(SqlErr) == false)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        return;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        sDeptCode = clsVbfunc.GetBASClinicDeptNameK(pDbCon, reader.GetValue(1).ToString().Trim()) + "(" + reader.GetValue(1).ToString().Trim() + ")";//      '진료과
                        sDrName = reader.GetValue(0).ToString().Trim();//          '담당의사
                        sDrBunho = reader.GetValue(3).ToString().Trim();// '면허번호
                        sTelno = "054-" + reader.GetValue(2).ToString().Trim();// '외래전화
                    }
                    else
                    {
                        reader.Dispose();
                        return;
                    }

                    reader.Dispose();

                    #endregion


                    WriteStr(35, 600, 80, "의뢰환자 회신서");

                    WriteStr(25, 70, 170, sRname);//             '의뢰병원
                    if (dt.Rows[0]["RDRNAME"].ToString().Trim() != "") // '의뢰의사명
                    {
                        WriteStr(25, 70, 205, sRdRname + " 원장님(선생님) 귀하");
                    }
                    else
                    {
                        WriteStr(25, 70, 205, "         원장님(선생님) 귀하");
                    }
                    WriteStr(25, 70, 240, "환자를 의뢰해 주셔서 감사합니다.  본원에서의 진료내역을 알려드립니다.");


                    WriteStr(25, 125, 1190, "회      신");//
                    WriteStr(25, 125, 1290, "안      내");//
                    WriteStr(25, 415, 620, "환자내원일");//


                    WriteStr(25, 650, 320, sSname);//         '성명
                    WriteStr(25, 1200, 320, sJumin);//        '주민등록번호
                    WriteStr(25, 650, 420, sJuso);//         '주소
                    WriteStr(25, 650, 520, sPano);//         '병록번호
                    WriteStr(25, 650, 620, sBdate);//         '환자내원일
                    WriteStr(25, 1200, 520, sOpdIpd);//        '진료구분
                    WriteStr(20, 1200, 620, sInDate); //        '입원기간


                    WriteStr(25, 400, 725, sJindan);         //  '상병명
                    //'frmImgConvert.ltkPV.TextOut FontName, 25, 400, 810, TextColor, BackColor, sRequest         '회신서 내용


                    WriteStr(25, 305, 1950, sDeptCode);//      '진료과
                    WriteStr(25, 805, 1950, sDrName);//       '담당의사
                    WriteStr(25, 1340, 1950, sDrBunho);//       '면허번호


                    WriteStr(25, 305, 2000, sTelno);//외래전화
                    WriteStr(25, 805, 2000, sRequestDate);//     '작성일자

                    #region 회신서 내용 완성용
                    strTResult = dt.Rows[0]["OPINION"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                    strResult = strTResult.Split('\n');

                    for (int K = 0; K < strResult.Length; K++)
                    {
                        if (string.IsNullOrWhiteSpace(strResult[K]))
                        {
                            lngLine += 1;
                        }
                        else
                        {

                            int strByte = Encoding.Default.GetBytes(strResult[K]).Length;
                            for (double b = 1; b < Math.Truncate((double)strByte / 75 + 0.9) + 1; b++)
                            {
                                WriteStr(20, 400, 810 + (lngLine * 34), VB.Mid(strResult[K], 1 + 65 * (int)(b - 1), 65));

                                lngLine += 1;

                                if (lngLine > 37)
                                {
                                    TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                                    //LNGY = 40;
                                    lngLine = 1;
                                    PageNum += 1;

                                    New_initFormRetrun();

                                    WriteStr(35, 600, 80, "의뢰환자 경과회신서");

                                    WriteStr(25, 70, 170, sRname);//             '의뢰병원
                                    if (dt.Rows[0]["RDRNAME"].ToString().Trim() != "") // '의뢰의사명
                                    {
                                        WriteStr(25, 70, 205, sRdRname + " 원장님(선생님) 귀하");
                                    }
                                    else
                                    {
                                        WriteStr(25, 70, 205, "         원장님(선생님) 귀하");
                                    }
                                    WriteStr(25, 70, 240, "환자를 의뢰해 주셔서 감사합니다.  본원에서의 진료내역을 알려드립니다.");


                                    WriteStr(25, 125, 1190, "회      신");//
                                    WriteStr(25, 125, 1290, "안      내");//
                                    WriteStr(25, 415, 620, "환자내원일");//


                                    WriteStr(25, 650, 320, sSname);//         '성명
                                    WriteStr(25, 1200, 320, sJumin);//        '주민등록번호
                                    WriteStr(25, 650, 420, sJuso);//         '주소
                                    WriteStr(25, 650, 520, sPano);//         '병록번호
                                    WriteStr(25, 650, 620, sBdate);//         '환자내원일
                                    WriteStr(25, 1200, 520, sOpdIpd);//        '진료구분
                                    WriteStr(20, 1200, 620, sInDate); //        '입원기간


                                    WriteStr(25, 400, 725, sJindan);         //  '상병명
                                    //'frmImgConvert.ltkPV.TextOut FontName, 25, 400, 810, TextColor, BackColor, sRequest         '회신서 내용


                                    WriteStr(25, 305, 1950, sDeptCode);//      '진료과
                                    WriteStr(25, 805, 1950, sDrName);//       '담당의사
                                    WriteStr(25, 1340, 1950, sDrBunho);//       '면허번호


                                    WriteStr(25, 305, 2000, sTelno);//외래전화
                                    WriteStr(25, 805, 2000, sRequestDate);//     '작성일자
                                }
                            }
                        }
                    }

                    #endregion

                }

                dt.Dispose();
            }

            TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

        }

        public static void New_initFormRetrun()
        {
            #region 이미지 그리기
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            //'테이블 그리기
            RectLine(40, 305, 1550, 1950);
            vline(350, 305, 1900);
            hline2(40, 1900, 1590);

            vline(630, 305, 690); //성명 ~ 진료개시일 
            vline(930, 305, 390); //주민번호
            vline(1180, 305, 390); //주민번호

            vline(930, 480, 590); //병록번호
            vline(1180, 480, 590); //병록번호

            vline(930, 590, 690); //개시일등                               
            vline(1180, 590, 690); //개시일등


            hline2(350, 390, 1590);
            hline2(350, 480, 1590);
            hline2(350, 590, 1590);
            hline2(40, 690, 1590);
            hline2(40, 790, 1590);

            //Call hline(frmImgConvert.ltkPV, 50, 265, 53)
            //Call hline(frmImgConvert.ltkPV, 365, 365, 42)
            //Call hline(frmImgConvert.ltkPV, 365, 465, 42)
            //Call hline(frmImgConvert.ltkPV, 365, 565, 42)
            //Call hline(frmImgConvert.ltkPV, 50, 665, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 765, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 1900, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 2220, 53)

            //Call vline(frmImgConvert.ltkPV, 33, 285, 88)
            //Call vline(frmImgConvert.ltkPV, 345, 290, 73)
            //Call vline(frmImgConvert.ltkPV, 605, 290, 17)
            //Call vline(frmImgConvert.ltkPV, 905, 283, 4)
            //Call vline(frmImgConvert.ltkPV, 905, 490, 8)
            //Call vline(frmImgConvert.ltkPV, 1155, 283, 4)
            //Call vline(frmImgConvert.ltkPV, 1155, 490, 8)
            //Call vline(frmImgConvert.ltkPV, 1560, 285, 88)

            //'테이블 내용 입력
            WriteStr(25, 125, 460, "수  진  자");
            WriteStr(25, 125, 725, "상  병  명");

            WriteStr(25, 425, 320, "성    명");
            WriteStr(25, 425, 420, "주    소"   );
            WriteStr(25, 425, 520, "병록번호"    );
            WriteStr(25, 950, 320, "주민등록번호");
            WriteStr(25, 980, 520, "진료구분"    );
            WriteStr(25, 980, 620, "입원기간"    );

            WriteStr(25, 115, 1950, "진료과   :");
            WriteStr(25, 615, 1950, "담당의사 :" );
            WriteStr(25, 1150, 1950, "면허번호 :");

            WriteStr(25, 115, 2000, "외래전화 :");
            WriteStr(25, 615, 2000, "작성일자 :");

            WriteStr(40, 650, 2080, "포항성모병원");
            WriteStr(25, 280, 2150, "(37661) 포항시 남구 대잠동길 17 홈페이지 www.pohangsmh.co.kr");
            WriteStr(25, 115, 2200, "진료의뢰센터 054) 260-8005    FAX 054) 260-8006    응급의료센터 054) 260-8118~9");

            #endregion
        }


        #endregion

        #region 지참약 식별회신서
        public static bool New_Exam_Drug_Hoimst(PsmhDb pDbCon, string var, string strPatid, string strDate, string strClinCode, string strClass, string strOutDate, string strDocCode, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            gstrFormcode = strClass.Trim().Equals("I") ? "140" : "141";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            PageNum = 0;

            //string strPtno = string.Empty;
            strOutDate = string.IsNullOrWhiteSpace(strOutDate) ? ComQuery.CurrentDateTime(pDbCon, "S").Substring(0, 10) : strOutDate;
            #endregion



            try
            {

                #region 쿼리
                SQL = " SELECT  WRTNO ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_ADM.DRUG_HOIMST  ";
                SQL += ComNum.VBLF + " WHERE PANO = '" + strPatid + "' ";
                SQL += ComNum.VBLF + "   AND BUN = '2' ";// '입력완료;
                SQL += ComNum.VBLF + "   AND HDATE IS NOT NULL";

                if (strClass.Equals( "I"))
                {
                    SQL += ComNum.VBLF + " AND IPDOPD ='I'";
                    SQL += ComNum.VBLF + " AND TRUNC(BDate) >=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + " AND TRUNC(BDate) <=TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                }
                else
                {

                    SQL += ComNum.VBLF + " AND TRUNC(BDATE) = TO_DATE('" + strDate + "','YYYY-MM-DD')  ";
                    SQL += ComNum.VBLF + " AND IPDOPD='O'";

                    if(strClinCode.Equals("RA") || strClinCode.Equals("IN"))
                    {
                        SQL += ComNum.VBLF + "   AND DEPTCODE IN ('MD','MR')  AND DRCODE in ('1107','1125','0901','0902','0903') ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   AND (  DEPTCODE = '" + strClinCode + "'  OR DEPTCODE ='NE' )  ";
                    }
                }

                SQL += ComNum.VBLF + " ORDER BY HDATE DESC,PAno ";

                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    return rtnVal;
                }

                SaveCvtLog(strPatid, strTREATNO, "지참약");

                while (reader.Read())
                {
                    PageNum += 1;
                    New_Exam_Drug_hoimst_DTL(pDbCon, reader.GetValue(0).ToString().Trim(), strPatid, strDate, strClinCode, strTREATNO);
                }

                reader.Dispose();
                rtnVal = true;
            }
            catch (Exception ex)
            {
                SaveCvtLog(strPatid, strTREATNO, "지참약 에러");
                //ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
            }

            return rtnVal;
        }

        /// <summary>
        /// 지참약 식별회신서
        /// 20-02-18 새로 개발
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgWrtno"></param>
        public static void New_Exam_Drug_hoimst_DTL(PsmhDb pDbCon, string ArgWrtno, string strPatid, string strDate, string strClinCode, string strTREATNO)
        {
            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            int RectY = 790;
            #endregion

            try
            {
                New_Exam_Drug_hoimst_Title(pDbCon, ArgWrtno);

                #region 쿼리
                SQL = " SELECT";
                #region 항혈전제, 메트로폴린
                SQL += ComNum.VBLF + "(               ";
                SQL += ComNum.VBLF + "SELECT COUNT(BLOOD)           ";
                SQL += ComNum.VBLF + "FROM KOSMOS_ADM.DRUG_HOISLIP  ";
                SQL += ComNum.VBLF + "WHERE WRTNO = " + ArgWrtno;
                SQL += ComNum.VBLF + "  AND BLOOD = '1'             ";
                SQL += ComNum.VBLF + ") AS BLOOD                    ";
                SQL += ComNum.VBLF + ", (               ";
                SQL += ComNum.VBLF + "SELECT COUNT(METFORMIN)           ";
                SQL += ComNum.VBLF + "FROM KOSMOS_ADM.DRUG_HOISLIP  ";
                SQL += ComNum.VBLF + "WHERE WRTNO = " + ArgWrtno;
                SQL += ComNum.VBLF + "  AND METFORMIN = '1'             ";
                SQL += ComNum.VBLF + ") AS METFORMIN                       ";
                #endregion

                SQL += ComNum.VBLF + " , A.IMGYN, A.EDICODE, A.RP,  DECODE(TUYAKGBN, '1', '●', '') AS TUYAKGBN";
                SQL += ComNum.VBLF + " , A.QTY, A.DOSCODE, B.SDOSNAME ";
                SQL += ComNum.VBLF + " , A.REMARK1,  A.REMARK2,  A.REMARK3,  A.REMARK4";
                SQL += ComNum.VBLF + " , A.REMARK5,  A.REMARK6,  A.REMARK7,  A.REMARK8";
                SQL += ComNum.VBLF + " , A.REMARK9,  A.REMARK10, A.REMARK11, A.REMARK12";
                SQL += ComNum.VBLF + " , A.REMARK13, A.REMARK15, A.REMARK16, A.REMARK17, A.REMARK18, A.ROWID";

                #region REMARK 5
                SQL += ComNum.VBLF + ", (               ";
                SQL += ComNum.VBLF + "SELECT '(' || HNAME || ')'";
                SQL += ComNum.VBLF + "FROM KOSMOS_OCS.OCS_DRUGINFO_NEW R5A";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.DRUG_JEP R5B";
                SQL += ComNum.VBLF + "    ON R5A.SUNEXT = R5B.JEPCODE";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.AIS_LTD R5C";
                SQL += ComNum.VBLF + "    ON R5B.GELCODE = R5C.LTDCODE";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_PMPA.BAS_SUT R5D";
                SQL += ComNum.VBLF + "    ON R5A.SUNEXT = R5D.SUNEXT";
                SQL += ComNum.VBLF + "WHERE R5A.SUNEXT  > CHR(0)";
                SQL += ComNum.VBLF + "  AND TRIM(R5A.SUNEXT) = A.REMARK5";
                SQL += ComNum.VBLF + ") JEP1";
                #endregion

                #region REMARK 12
                SQL += ComNum.VBLF + ", (               ";
                SQL += ComNum.VBLF + "SELECT '(' || HNAME || ')'";
                SQL += ComNum.VBLF + "FROM KOSMOS_OCS.OCS_DRUGINFO_NEW R12A";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.DRUG_JEP R12B";
                SQL += ComNum.VBLF + "    ON R12A.SUNEXT = R12B.JEPCODE";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.AIS_LTD R12C";
                SQL += ComNum.VBLF + "    ON R12B.GELCODE = R12C.LTDCODE";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_PMPA.BAS_SUT R12D";
                SQL += ComNum.VBLF + "    ON R12A.SUNEXT = R12D.SUNEXT";
                SQL += ComNum.VBLF + "WHERE R12A.SUNEXT  > CHR(0)";
                SQL += ComNum.VBLF + "  AND TRIM(R12A.SUNEXT) = A.REMARK12";
                SQL += ComNum.VBLF + ") JEP2";
                #endregion

                #region REMARK 13
                SQL += ComNum.VBLF + ", (               ";
                SQL += ComNum.VBLF + "SELECT '(' || HNAME || ')'";
                SQL += ComNum.VBLF + "FROM KOSMOS_OCS.OCS_DRUGINFO_NEW R13A";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.DRUG_JEP R13B";
                SQL += ComNum.VBLF + "    ON R13A.SUNEXT = R13B.JEPCODE";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.AIS_LTD R13C";
                SQL += ComNum.VBLF + "    ON R13B.GELCODE = R13C.LTDCODE";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_PMPA.BAS_SUT R13D";
                SQL += ComNum.VBLF + "    ON R13A.SUNEXT = R13D.SUNEXT";
                SQL += ComNum.VBLF + "WHERE R13A.SUNEXT > CHR(0)";
                SQL += ComNum.VBLF + "  AND TRIM(R13A.SUNEXT) = A.REMARK13";
                SQL += ComNum.VBLF + ") JEP3";
                #endregion

                #region REMARK 16
                SQL += ComNum.VBLF + ", (               ";
                SQL += ComNum.VBLF + "SELECT '(' || HNAME || ')'";
                SQL += ComNum.VBLF + "FROM KOSMOS_OCS.OCS_DRUGINFO_NEW R16A";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.DRUG_JEP R16B";
                SQL += ComNum.VBLF + "    ON R16A.SUNEXT = R16B.JEPCODE";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.AIS_LTD R16C";
                SQL += ComNum.VBLF + "    ON R16B.GELCODE = R16C.LTDCODE";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_PMPA.BAS_SUT R16D";
                SQL += ComNum.VBLF + "    ON R16A.SUNEXT = R16D.SUNEXT";
                SQL += ComNum.VBLF + "WHERE R16A.SUNEXT > CHR(0)";
                SQL += ComNum.VBLF + "  AND TRIM(R16A.SUNEXT) = A.REMARK16";
                SQL += ComNum.VBLF + ") JEP4";
                #endregion

                #region REMARK 17
                SQL += ComNum.VBLF + ", (               ";
                SQL += ComNum.VBLF + "SELECT '(' || HNAME || ')'";
                SQL += ComNum.VBLF + "FROM KOSMOS_OCS.OCS_DRUGINFO_NEW R17A";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.DRUG_JEP R17B";
                SQL += ComNum.VBLF + "    ON R17A.SUNEXT = R17B.JEPCODE";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.AIS_LTD R17C";
                SQL += ComNum.VBLF + "    ON R17B.GELCODE = R17C.LTDCODE";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_PMPA.BAS_SUT R17D";
                SQL += ComNum.VBLF + "    ON R17A.SUNEXT = R17D.SUNEXT";
                SQL += ComNum.VBLF + "WHERE R17A.SUNEXT > CHR(0)";
                SQL += ComNum.VBLF + "  AND TRIM(R17A.SUNEXT) = A.REMARK17";
                SQL += ComNum.VBLF + ") JEP5";
                #endregion

                #region REMARK 18
                SQL += ComNum.VBLF + ", (               ";
                SQL += ComNum.VBLF + "SELECT '(' || HNAME || ')'";
                SQL += ComNum.VBLF + "FROM KOSMOS_OCS.OCS_DRUGINFO_NEW R18A";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.DRUG_JEP R18B";
                SQL += ComNum.VBLF + "    ON R18A.SUNEXT = R18B.JEPCODE";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.AIS_LTD R18C";
                SQL += ComNum.VBLF + "    ON R18B.GELCODE = R18C.LTDCODE";
                SQL += ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_PMPA.BAS_SUT R18D";
                SQL += ComNum.VBLF + "    ON R18A.SUNEXT = R18D.SUNEXT";
                SQL += ComNum.VBLF + "WHERE R18A.SUNEXT > CHR(0)";
                SQL += ComNum.VBLF + "  AND TRIM(R18A.SUNEXT) = A.REMARK18";
                SQL += ComNum.VBLF + ") JEP6";
                #endregion

                SQL += ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_HOISLIP A";
                SQL += ComNum.VBLF + "   LEFT OUTER JOIN KOSMOS_OCS.OCS_ODOSAGE B";
                SQL += ComNum.VBLF + "     ON A.DOSCODE = B.DOSCODE";
                SQL += ComNum.VBLF + " WHERE A.WRTNO = " + ArgWrtno;
                SQL += ComNum.VBLF + "ORDER BY A.RP, A.ENTDATE -- 그룹, 입력순";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return;
                }
                #endregion

                Rectangle rect;
                if (dt.Rows.Count > 0)
                {
                    #region 항혈전제, 메트로폴린 표시
                    if (dt.Rows[0]["BLOOD"].ToString().Trim().Equals("1"))
                    {
                        WriteStr(21, 640, 560, "■ 있음 " + "□ 없음  (항혈전제 : 상품명 앞에 ★ 표시가 있는 약품)");
                    }
                    else
                    {
                        WriteStr(21, 640, 560, "□ 있음 " + "■ 없음  (항혈전제 : 상품명 앞에 ★ 표시가 있는 약품)");
                    }

                    if (dt.Rows[0]["METFORMIN"].ToString().Trim().Equals("1"))
                    {
                        WriteStr(21, 640, 610, "■ 있음 " + "□ 없음  (Metformain제제 : 상품명 앞에 ▣ 표시가 있는 약품)");
                    }
                    else
                    {
                        WriteStr(21, 640, 610, "□ 있음 " + "■ 없음  (Metformain제제 : 상품명 앞에 ▣ 표시가 있는 약품)");
                    }
                    #endregion

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (PageNum == 1 && (i + 1) % 4 == 0 || PageNum > 1 && (i + 1) % 9 == 0)
                        {
                            //TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif", true);
                            SaveJpeg(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif", mBitmap, 50);

                            PageNum += 1;

                            New_initFormDrug_NewPage();
                            RectY = 40;
                        }

                        #region No, 그룹
                        rect = Set_Rect(30, RectY, 50, 280);
                        WriteStr(21, false, (i + 1).ToString(), rect);

                        rect = Set_Rect(80, RectY, 60, 280);
                        WriteStr(21, false, dt.Rows[i]["RP"].ToString().Trim(), rect);
                        #endregion


                        #region 약품사진, 앞뒤 설명
                        //이미지
                        if (dt.Rows[i]["IMGYN"].ToString().Trim().Equals("1") == false)
                        {
                            Image image = GetDrugInfoImg(READ_FDRUGCD(dt.Rows[i]["EDICODE"].ToString().Trim()));
                            rect = Set_Rect(140, RectY, 340, 200);
                            if (image != null)
                            {
                                WriteImage(image, rect);
                                image.Dispose();
                            }
                        }
                        else
                        {
                            WriteStr(21, false, "", rect);
                        }

                        //약 앞
                        rect = Set_Rect(140, RectY + 200, 170, 80);
                        WriteStr(21, false, string.Format("앞) {0}", dt.Rows[i]["REMARK9"].ToString().Trim()), rect);

                        //약 뒤
                        rect = Set_Rect(310, RectY + 200, 170, 80);
                        WriteStr(21, false, string.Format("뒤) {0}", dt.Rows[i]["REMARK10"].ToString().Trim()), rect);
                        #endregion

                        #region 성상
                        rect = Set_Rect(480, RectY, 160, 200);
                        WriteStr(21, false, dt.Rows[i]["REMARK6"].ToString().Trim(), rect);

                        rect = Set_Rect(480, RectY + 200, 160, 80);
                        WriteStr(21, false, string.Format("제형:{0}", dt.Rows[i]["REMARK8"].ToString().Trim()), rect);
                        #endregion

                        #region 성분 및 함량
                        string strTemp2 = dt.Rows[i]["REMARK1"].ToString().Trim();

                        if (string.IsNullOrWhiteSpace(dt.Rows[i]["EDICODE"].ToString().Trim()) == false) 
                        {
                            strTemp2 += ComNum.VBLF + "(" + dt.Rows[i]["EDICODE"].ToString().Trim() + ")";
                        }
                        
                        if (string.IsNullOrWhiteSpace(dt.Rows[i]["REMARK7"].ToString().Trim()) == false)
                        {
                            strTemp2 += ComNum.VBLF + "(" + dt.Rows[i]["REMARK7"].ToString().Trim() + ")";
                        }

                        rect = Set_Rect(640, RectY, 440, 200);
                        WriteStr(21, false, strTemp2, rect);

                        rect = Set_Rect(640, RectY + 200, 440, 80);
                        using(Font font = new Font("맑은 고딕", 21))
                        {
                            //2줄 넘어가면 폰트 사이즈 줄여서 3줄 맞춤..
                            Size s = TextRenderer.MeasureText(dt.Rows[i]["REMARK2"].ToString().Trim(), font, new Size(440, 80), TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                            WriteStr((s.Width / (double)440) > 2 ? 18f : 21, false, dt.Rows[i]["REMARK2"].ToString().Trim(), rect);
                        }

                        #endregion


                        #region 효능/효과
                        rect = Set_Rect(1080, RectY, 370, 280);
                        WriteStr(21, false, dt.Rows[i]["REMARK3"].ToString().Trim(), rect);
                        #endregion

                        #region 일투량 용법
                        rect = Set_Rect(1450, RectY, 160, 200);
                        WriteStr(21, false, dt.Rows[i]["QTY"].ToString().Trim(), rect);

                        rect = Set_Rect(1450, RectY + 200, 160, 80);
                        WriteStr(21, false, dt.Rows[i]["SDOSNAME"].ToString().Trim(), rect);
                        #endregion


                        #region 원내대체약 정보, 본원사용여부/코드/약품명
                        StringBuilder strTemp = new StringBuilder();
                        strTemp.Append(READ_USED_GUBUN(dt.Rows[i]["REMARK11"].ToString().Trim()));

                        if (string.IsNullOrWhiteSpace(dt.Rows[i]["JEP1"].ToString().Trim()) == false)
                        {
                            strTemp.Append("\r\n");
                            strTemp.Append(dt.Rows[i]["REMARK5"].ToString().Trim() + dt.Rows[i]["JEP1"].ToString().Trim());
                        }

                        if (string.IsNullOrWhiteSpace(dt.Rows[i]["JEP2"].ToString().Trim()) == false)
                        {
                            strTemp.Append("\r\n");
                            strTemp.Append(dt.Rows[i]["REMARK12"].ToString().Trim() + dt.Rows[i]["JEP2"].ToString().Trim());
                        }

                        if (string.IsNullOrWhiteSpace(dt.Rows[i]["JEP3"].ToString().Trim()) == false)
                        {
                            strTemp.Append("\r\n");
                            strTemp.Append(dt.Rows[i]["REMARK13"].ToString().Trim() + dt.Rows[i]["JEP3"].ToString().Trim());
                        }

                        if (string.IsNullOrWhiteSpace(dt.Rows[i]["REMARK15"].ToString().Trim()) == false)
                        {
                            strTemp.Append("\r\n");
                            strTemp.Append("----------------------------------------");
                            strTemp.Append(READ_USED_GUBUN(dt.Rows[i]["REMARK15"].ToString().Trim()));


                            if (string.IsNullOrWhiteSpace(dt.Rows[i]["JEP4"].ToString().Trim()) == false)
                            {
                                strTemp.Append("\r\n");
                                strTemp.Append(dt.Rows[i]["REMARK16"].ToString().Trim() + dt.Rows[i]["JEP4"].ToString().Trim());
                            }

                            if (string.IsNullOrWhiteSpace(dt.Rows[i]["JEP5"].ToString().Trim()) == false)
                            {
                                strTemp.Append("\r\n");
                                strTemp.Append(dt.Rows[i]["REMARK17"].ToString().Trim() + dt.Rows[i]["JEP5"].ToString().Trim());
                            }

                            if (string.IsNullOrWhiteSpace(dt.Rows[i]["JEP6"].ToString().Trim()) == false)
                            {
                                strTemp.Append("\r\n");
                                strTemp.Append(dt.Rows[i]["REMARK18"].ToString().Trim() + dt.Rows[i]["JEP6"].ToString().Trim());
                            }
                        }

                        rect = Set_Rect(1610, RectY, 510, 280);
                        WriteStr(21, false, strTemp.ToString(), rect);
                        #endregion

                        #region 지참약 투여불가
                        rect = Set_Rect(2120, RectY, 190, 280);
                        WriteStr(40, false, dt.Rows[i]["TUYAKGBN"].ToString().Trim(), rect);
                        #endregion

                        RectY += 280;
                    }
                }


                if (1648 - RectY < 420)
                {
                    //TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif", true);
                    SaveJpeg(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif", mBitmap, 50);
                    PageNum += 1;
                    New_initFormDrug_NewPage();
                    RectY = 40;
                }

                RectLine(30, RectY + 40, 2280, 360, 2);
                rect = Set_Rect(40, RectY + 50, 2260, 60, 2, "");

                using (StringFormat stringFormat = new StringFormat())
                {
                    using (Font font = new Font(FontName, 25, FontStyle.Bold))
                    {
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        mGraphics.DrawString("[회신서 작성 및 지참약 투여에 대한 참고사항]", font, Brushes.Black, rect, stringFormat);
                    }

                    rect = Set_Rect(40, RectY + 110, 2260, 300, 2, "");

                    using (Font font = new Font(FontName, 20, FontStyle.Regular))
                    {
                        stringFormat.Alignment = StringAlignment.Near;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        mGraphics.DrawString(   "      1. 원내사용여부 및 대체약 정보에 \'◎원내-동일성분, 제형다름(약동학적 차이 있음)\'으로 표시된 경우,\r\n\n" +
                                                "      2. 다음과 같은 경우 입원 시 지참약의 투여가 불가능합니다.\r\n\n" +
                                                "         1) 1회분 포장으로 되어 있는 2종 이상의 약품(No.우측의 그룹 표시가 같음) 중 일부 투여\r\n\n" +
                                                "         2) 식별불가능한 약품 및 식별불가능한 약품과 함께 1회분 포장으로 되어 있는 약품 전체\r\n\n" +
                                                "         3) 타의료기관에서 처방된 마약\r\n\n"
                                                , font, Brushes.Black, rect, stringFormat);


                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        rect = Set_Rect(30, RectY + 420, 2280, 60, 2, "");
                        mGraphics.DrawString("포항성모병원 약제팀", font, Brushes.Black, rect, stringFormat);
                    }
                }

                //WriteStr(25, 900, RectY + 50, );

                //WriteStr(20, 100, RectY + 100,  "1. 원내사용여부 및 대체약 정보에 \'◎원내-동일성분, 제형다름(약동학적 차이 있음)\'으로 표시된 경우,");
                //WriteStr(20, 100, RectY + 140, "2. 다음과 같은 경우 입원 시 지참약의 투여가 불가능합니다.");
                //WriteStr(20, 140, RectY + 180, "  1) 1회분 포장으로 되어 있는 2종 이상의 약품(No.우측의 그룹 표시가 같음) 중 일부 투여");
                //WriteStr(20, 140, RectY + 220, "  2) 식별불가능한 약품 및 식별불가능한 약품과 함께 1회분 포장으로 되어 있는 약품 전체");
                //WriteStr(20, 140, RectY + 260, "  3) 타의료기관에서 처방된 마약");
                //WriteStr(20, 1100, RectY + 320, "포항성모병원 약제팀");

                FontName = "굴림체";

                dt.Dispose();

                //TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif", true);
                SaveJpeg(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif", mBitmap, 50);
            }
            catch (Exception ex)
            {
                SaveCvtLog(strPatid, strTREATNO, "지참약 에러");
                //ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return;
            }
        }

        public static string READ_USED_GUBUN(string strGUBUN)
        {
            string rtnVal = string.Empty;

            switch (strGUBUN.Trim())
            {
                case "01": rtnVal = "◎원내/외 동종ㆍ동효약 없음"; break;
                case "02": rtnVal = "◎원내-동일약"; break;
                case "03": rtnVal = "◎원내-대체약(성분 동일, 함량 동일)"; break;
                case "04": rtnVal = "◎원내-동종약(성분 동일, 함량 다름)"; break;
                case "05": rtnVal = "◎원내-단일성분만 사용"; break;
                case "06": rtnVal = "◎원외전용-동일약"; break;
                case "07": rtnVal = "◎원외전용-대체약(성분 동일, 함량 동일)"; break;
                case "08": rtnVal = "◎원외전용-동종약(성분 동일, 함량 다름)"; break;
                case "09": rtnVal = "◎원내-효능유사약(성분 다름)"; break;
                case "10": rtnVal = "◎원내-동일성분,제형다름(약동학적 차이 있음)"; break;
            }

            return rtnVal;
        }

        /// <summary>
        /// 퍼스트디스 약품 식별코드 가져오기
        /// </summary>
        /// <param name="strEDICODE">보험코드</param>
        /// <returns></returns>
        public static string READ_FDRUGCD(string strEDICODE)
        {
            string rtnVal = string.Empty;

            if (string.IsNullOrWhiteSpace(strEDICODE)) 
                return rtnVal;

            PsmhDb pDbCon = clsDB.DbCon;

            string pSearchType = "06";
            string pKeyword = strEDICODE;
            string pScope = "02";

            using (OracleCommand cmd = new OracleCommand())
            {
                cmd.Connection = pDbCon.Con;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandText = "KOSMOS_DRUG.up_DrugSearch";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("pSearchType", OracleDbType.Varchar2, 9, pSearchType, ParameterDirection.Input);
                cmd.Parameters.Add("pKeyword", OracleDbType.Varchar2, 9, pKeyword, ParameterDirection.Input);
                cmd.Parameters.Add("pScope", OracleDbType.Varchar2, 9, pScope, ParameterDirection.Input);

                cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        rtnVal = reader.GetValue(12).ToString().Trim();
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 퍼스트디스 약품 식별코드로 이미지 가져오기
        /// </summary>
        /// <param name="strDifKey">퍼스트디스 약품 식별코드</param>
        /// <returns></returns>
        public static Image GetDrugInfoImg(string strDifKey)
        {
            Image img = null;

            if (string.IsNullOrWhiteSpace(strDifKey))
                return img;

            if (Directory.Exists("c:\\cmc\\ocsexe\\dif\\") == false)
            {
                Directory.CreateDirectory("c:\\cmc\\ocsexe\\dif");
            }

            //Dir_Check(@"c:\cmc\ocsexe\dif");

            string strImgFileName = strDifKey + ".jpg";
            string strLocal = "c:\\cmc\\ocsexe\\dif\\" + strImgFileName;
            string strPath = "/pcnfs/firstdis/" + strImgFileName;
            string strHost = "/pcnfs/firstdis";

            Ftpedt FtpedtX = new Ftpedt();
            if (FtpedtX.FtpConnetBatch("192.168.100.33", "pcnfs", "pcnfs1") == false)
            {
                FtpedtX.Dispose();
                ComFunc.MsgBox("FTP Server Connect ERROR !!!", "오류");
                return img;
            }

            FileInfo f = new FileInfo(strLocal);

            try
            {
                ////기존파일 삭제
                if (f.Exists == true)
                {
                    f.Delete();
                }

                if (FtpedtX.FtpDownloadEx("192.168.100.33", "pcnfs", "pcnfs1", strLocal, strPath, strHost) == true)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Image.FromFile(strLocal).Save(ms, ImageFormat.Jpeg);
                        img = Image.FromStream(ms);
                    }
                }

                FtpedtX.FtpDisConnetBatch();
                FtpedtX.Dispose();
                FtpedtX = null;
            }
            catch
            {
                if (f.Exists == true)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Image.FromFile(strLocal).Save(ms, ImageFormat.Jpeg);
                        img = Image.FromStream(ms);
                    }
                }
                else
                {
                    if (FtpedtX.FtpDownloadEx("192.168.100.33", "pcnfs", "pcnfs1", strLocal, strPath, strHost) == true)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            Image.FromFile(strLocal).Save(ms, ImageFormat.Jpeg);
                            img = Image.FromStream(ms);
                        }
                    }
                }
            }

            if (FtpedtX != null)
            {
                FtpedtX.Dispose();
            }

            return img;
        }

        /// <summary>
        /// 기본정보표시
        /// </summary>
        /// <param name=""></param>
        public static void New_Exam_Drug_hoimst_Title(PsmhDb pDbCon, string ArgWrtno)
        {
            New_initFormDrug("지참약 식별회신서");

            #region 변수
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            #endregion

            try
            {
                #region 쿼리
                SQL = " SELECT A.JDATE, A.BDATE, A.DEPTCODE, A.DRCODE, A.DRSABUN, A.WARDCODE, A.HDate, A.DRUGGIST, ";
                SQL += ComNum.VBLF + " A.ROOMCODE, A.PANO, A.REMCODE1, A.REMCODE2, ";
                SQL += ComNum.VBLF + " A.REMCODE3, A.REMCODE4, A.REMCODE5, A.REMCODE6, ";
                SQL += ComNum.VBLF + " A.REMCODE7, A.REMCODE8, A.REMCODE9, A.REMCODE10, ";
                SQL += ComNum.VBLF + " A.REMCODE11, A.REMCODE12, A.REMCODE13, A.REMCODE14, ";
                SQL += ComNum.VBLF + " A.REMCODE15, A.REMCODE16, A.REMCODE17, ";
                SQL += ComNum.VBLF + " A.DABCODE, A.FASTRETURN, A.HOSP, A.PHAR, A.RETURNMEMO, ";
                SQL += ComNum.VBLF + " B2.NAME AS BUSENAME, B.SNAME, B.SEX, B.JUMIN1, B.JUMIN2, B.JUMIN3, ";
                SQL += ComNum.VBLF + " D.DRNAME, CD.DEPTNAMEK,";
                SQL += ComNum.VBLF + " (SELECT USERNAME FROM KOSMOS_PMPA.BAS_USER WHERE IDNUMBER = A.DRUGGIST) AS USERNAME,";
                SQL += ComNum.VBLF + " C.KORNAME";
                SQL += ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_HOIMST A";
                SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_PMPA.BAS_CLINICDEPT CD";
                SQL += ComNum.VBLF + "      ON A.DEPTCODE = CD.DEPTCODE";
                SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_OCS.OCS_DOCTOR D";
                SQL += ComNum.VBLF + "      ON A.DRCODE = D.DRCODE";
                SQL += ComNum.VBLF + "   INNER JOIN KOSMOS_PMPA.BAS_PATIENT B ";
                SQL += ComNum.VBLF + "      ON A.PANO = B.PANO";
                SQL += ComNum.VBLF + "    LEFT OUTER JOIN KOSMOS_ADM.INSA_MST C --의뢰자용";
                SQL += ComNum.VBLF + "      ON TRIM(A.DRSABUN) = TRIM(C.SABUN)";
                SQL += ComNum.VBLF + "    LEFT OUTER JOIN KOSMOS_PMPA.BAS_BUSE B2 --의뢰자 부서";
                SQL += ComNum.VBLF + "      ON C.BUSE = B2.BUCODE";
                SQL += ComNum.VBLF + " WHERE A.WRTNO = " + ArgWrtno;
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    DateTime dtpBDATE = Convert.ToDateTime(dt.Rows[0]["BDATE"].ToString().Trim());
                    DateTime dtpHDATE = Convert.ToDateTime(dt.Rows[0]["HDATE"].ToString().Trim());

                    #region 1(내용)
                    WriteStr(20, 350, 150, dt.Rows[0]["SNAME"].ToString().Trim());
                    WriteStr(20, 360, 220, dt.Rows[0]["SEX"].ToString().Trim() + "/" + clsVbfunc.AGE_YEAR_GESAN2(dt.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim()), dtpBDATE.ToString("yyyy-MM-dd")));
                    hline2(150, 205, 315);
                    vline(480, 130, 270);

                    WriteStr(20, 365, 285, dtpBDATE.ToString("yyyy-MM-dd HH:mm"));
                    WriteStr(20, 385, 325, DABCODE(dt.Rows[0]["DABCODE"].ToString().Trim()));
                    hline2(150, 480, 315);

                    WriteStr(20, 325, 400, dtpHDATE.ToString("yyyy-MM-dd"));
                    WriteStr(20, 360, 440, dtpHDATE.ToString("HH:mm"));
                    WriteStr(20, 315, 500, dt.Rows[0]["USERNAME"].ToString().Trim() + " 약사");
                    vline(480, 370, 550);

                    //회신자 전달사항
                    Rectangle rect = Set_Rect(640, 370, 1670, 180, 2);
                    WriteStr(20, false, " " + dt.Rows[0]["RETURNMEMO"].ToString().Trim(), rect, StringAlignment.Near);
                    #endregion

                    #region 2(내용)
                    WriteStr(20, 690, 150, dt.Rows[0]["PANO"].ToString().Trim());
                    WriteStr(20, 690, 220, dt.Rows[0]["WARDCODE"].ToString().Trim() + "/" + dt.Rows[0]["ROOMCODE"].ToString().Trim());

                    //긴급요청사유
                    rect = Set_Rect(880, 270, 570, 100, 2);
                    WriteStr(20, false, " " + dt.Rows[0]["FASTRETURN"].ToString().Trim(), rect, StringAlignment.Near);
                    #endregion

                    #region 3(내용)
                    rect = Set_Rect(1080, 130, 370, 75, 2);
                    WriteStr(20, false, dt.Rows[0]["DEPTNAMEK"].ToString().Trim(), rect);

                    rect = Set_Rect(1080, 205, 370, 67, 2);
                    WriteStr(20, false, dt.Rows[0]["DRNAME"].ToString().Trim(), rect);
                    #endregion

                    #region 4(내용)
                    rect = Set_Rect(1610, 205, 350, 67, 2);
                    WriteStr(20, false, dt.Rows[0]["HOSP"].ToString().Trim(), rect);

                    WriteStr(20, 1620, 305, dt.Rows[0]["BUSENAME"].ToString().Trim());
                    vline(1810, 270, 370);

                    WriteStr(20, 1840, 305, dt.Rows[0]["KORNAME"].ToString().Trim());
                    vline(1960, 205, 370);
                    #endregion

                    #region 5(내용)
                    rect = Set_Rect(1610, 130, 700, 75, 2);
                    WriteStr(20, false, dt.Rows[0]["REMCODE6"].ToString().Trim(), rect);

                    rect = Set_Rect(2120, 205, 190, 67, 2);
                    WriteStr(20, false, dt.Rows[0]["PHAR"].ToString().Trim(), rect);

                    if (string.IsNullOrWhiteSpace(dt.Rows[0]["HDATE"].ToString().Trim()) == false)
                    {
                        DateTime dtpJdate = Convert.ToDateTime(dt.Rows[0]["JDATE"].ToString().Trim());
                        WriteStr(20, 2140, 285, dtpJdate.ToString("yyyy-MM-dd"));
                        WriteStr(20, 2180, 325, dtpJdate.ToString("HH:mm"));
                    }
                    #endregion
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                //ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
            }
        }

        public static string DABCODE(string strDABCODE)
        {
            string rtnVal = string.Empty;
            switch (strDABCODE)
            {
                case "01":
                    rtnVal = "10분이내";
                    break;
                case "02":
                    rtnVal = "30분이내";
                    break;
                case "03":
                    rtnVal = "1시간이내";
                    break;
                case "04":
                    rtnVal = "2시간이내";
                    break;
                case "05":
                    rtnVal = "3시간이내";
                    break;
                case "06":
                    rtnVal = "금일이내";
                    break;
                case "07":
                    rtnVal = "48시간이내";
                    break;
            }

            return "( " + rtnVal + " )";
        }

        public static void New_initFormDrug(string str)
        {
            #region 이미지 그리기
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(2327, 1648);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            Rectangle rect2 = new Rectangle(0, 30, mBitmap.Width, 120);
            //mGraphics.FillRectangle(Brushes.White, rect2);

            FontName = "맑은고딕";
            WriteStr(50, true, str, rect2);

            RectLine(30, 130, 2280, 140, 6);
            RectLine(30, 270, 2280, 100, 6);
            RectLine(30, 370, 2280, 180, 6);

            vline(150, 130, 550);

            WriteStr(20, 55, 160, "환자");
            WriteStr(20, 55, 200, "정보");

            WriteStr(20, 55, 285, "의뢰");
            WriteStr(20, 55, 325, "정보");

            WriteStr(20, 55, 420, "회신");
            WriteStr(20, 55, 460, "정보");

            //vline(500, 100, 690);

            #region 1800
            WriteStr(20, 200, 150, "이름");
            WriteStr(20, 170, 220, "성별/나이");
            hline2(150, 205, 2310);

            WriteStr(20, 170, 285, "의뢰일시");
            WriteStr(20, 170, 325, "요청시간");
            hline2(150, 320, 315);

            WriteStr(20, 170, 410, "회신일시");
            WriteStr(20, 180, 500, "회신자");
            hline2(150, 480, 480);

            vline(315, 130, 550);
            #endregion

         

            #region 2
            WriteStr(20, 500, 150, "등록번호");
            WriteStr(20, 495, 220, "병동/병실");
            vline(640, 130, 650);

            hline2(150, 205, 315);

            //WriteStr(20, 510, 410, "회신자");
            //WriteStr(20, 525, 450, "전달");
            //WriteStr(20, 525, 490, "사항");
            //hline2(150, 480, 315);

            WriteStr(20, 670, 310, "긴급요청사유");
            vline(880, 130, 370);
            hline2(150, 480, 315);
            #endregion


            #region 3
            WriteStr(20, 930, 150, "진료과");
            WriteStr(20, 920, 220, "담당의사");
            vline(1080, 130, 270);
            #endregion

          

            #region 4
            WriteStr(20, 1470, 150, "복용사유");
            WriteStr(20, 1470, 220, "처방병원");
            vline(1610, 130, 370);

            WriteStr(20, 1485, 305, "의뢰자");
            #endregion

           

            #region 5
            WriteStr(20, 1980, 220, "조제약국");
            WriteStr(20, 1980, 305, "접수일시");
            vline(2120, 205, 370);
            #endregion

            

            #region 6
            WriteStr(20, 510, 410, "회신자");
            WriteStr(20, 525, 450, "전달");
            WriteStr(20, 525, 490, "사항");
            #endregion

            #region 7
            RectLine(30, 550, 2280, 100, 3);
            hline2(30, 600, 2310);
            WriteStr(20, 160, 560, "지참약 중 항혈전제 포함 여부");
            WriteStr(20, 90, 610, "지참약 중 Metformain제제 포함 여부");
            #endregion

           

            #region 8
            RectLine(30, 650, 2280, 140, 3);

            WriteStr(20, 30, 700, "No.");
            vline(80, 650, 790);

            WriteStr(20, 90, 680, "그");
            WriteStr(20, 90, 720, "룹");
            vline(140, 650, 790);
            #endregion

            #region 9
            WriteStr(20, 90, 680, "그");
            WriteStr(20, 90, 720, "룹");
            vline(140, 650, 790);

            hline2(140, 700, 640);
            WriteStr(20, 280, 660, "식별 표시 정보");
            vline(640, 650, 790);

            vline(480, 700, 790);
            WriteStr(20, 240, 730, "약품사진");
            WriteStr(20, 530, 730, "성상");
            #endregion

            #region 10
            hline2(640, 700, 1080);
            WriteStr(20, 660, 660, "상품명(표준코드/제조,수입사)");
            vline(1080, 650, 790);

            WriteStr(20, 780, 730, "성분 및 함량");
            #endregion

            #region 11
            WriteStr(20, 1200, 700, "효능/효과");
            vline(1450, 650, 790);
            #endregion

            #region 12
            hline2(1450, 700, 2120);
            WriteStr(20, 1480, 660, "일투량");
            WriteStr(20, 1500, 730, "용법");
            vline(1610, 650, 790);


            vline(2120, 650, 790);
            WriteStr(20, 1750, 660, "원내대체약 정보");
            WriteStr(20, 1680, 730, "(본원사용여부/코드/약품명)");

            WriteStr(20, 2165, 680, "지참약");
            WriteStr(20, 2155, 720, "투여불가");
            #endregion

  

            #endregion
        }

        /// <summary>
        /// 식별회신서 다음장
        /// </summary>
        public static void New_initFormDrug_NewPage()
        {
            #region 이미지 그리기
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(2327, 1648);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);
            //Rectangle rect2 = new Rectangle(0, 30, mBitmap.Width, 120);
            //mGraphics.FillRectangle(Brushes.White, rect2);

            FontName = "맑은고딕";
            #endregion
        }

        #endregion

        #region EEG(뇌파)
        public static bool New_Exam_EEG(PsmhDb pDbCon, string var, string strPatid, string strDate, string strClinCode, string strClass, string strOutDate, string strDocCode, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            gstrFormcode = strClass.Trim().Equals("I") ? "126" : "006";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            PageNum = 0;

            string strPtno = string.Empty;
            strOutDate = string.IsNullOrWhiteSpace(strOutDate) ? ComQuery.CurrentDateTime(pDbCon, "S").Substring(0, 10) : strOutDate;
            #endregion

            try
            {

                #region 쿼리
                SQL = " SELECT  TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE ,  A.READ_WRTNO,   DECODE(A.GBJOB ,'3','EEG','기타') GBJOB, A.ORDERNO, A.ROWID,  ";
                SQL += ComNum.VBLF + "   B.ORDERNAME, A.DEPTCODE, A.DRCODE ,  C.DRNAME   ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_OCS.ETC_JUPMST  A,  KOSMOS_OCS.OCS_ORDERCODE B , KOSMOS_PMPA.BAS_DOCTOR C ";
                SQL += ComNum.VBLF + " WHERE A.PTNO = '" + strPatid + "' ";
                SQL += ComNum.VBLF + "   AND A.BDATE >=TO_DATE('2010-01-01','YYYY-MM-DD') ";
                //SQL += ComNum.VBLF + "   AND A.BDATE >=TO_DATE('2011-01-01','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND A.GUBUN = '2' ";// 'EEG;
                SQL += ComNum.VBLF + "   AND A.ORDERCODE = B.ORDERCODE(+)";
                SQL += ComNum.VBLF + "   AND A.DRCODE =C.DRCODE ";
                SQL += ComNum.VBLF + "   AND A.GBJOB <> '9' ";
                SQL += ComNum.VBLF + "   AND A.READ_WRTNO > 0 ";

                if (strClass.Equals("I"))
                {
                    SQL += ComNum.VBLF + " AND GBIO ='I'";
                    SQL += ComNum.VBLF + " AND BDate >=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + " AND BDate <=TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL += ComNum.VBLF + " AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')  ";
                    SQL += ComNum.VBLF + " AND GBIO ='O'";

                    if (strClinCode.Equals("RA") || strClinCode.Equals("MR"))
	                {
                        SQL += ComNum.VBLF + "   AND A.DEPTCODE IN ('MD' ,'MR') AND A.DRCODE in ('1107','1125','0901','0902','0903') ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   AND (  A.DEPTCODE = '" + strClinCode + "'  )  ";//  '의무기록실 요청 으로 삭제
                    }
                }

                SQL += ComNum.VBLF + " ORDER BY a.RDATE DESC,a.Ptno ";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                SaveCvtLog(strPatid, strTREATNO, "EEG(뇌파)");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PageNum += 1;
                    New_Exam_EEG_Result(pDbCon, strPatid, strDate, strClinCode, strClass, dt.Rows[i]["READ_WRTNO"].ToString().Trim(), dt.Rows[i]["Rdate"].ToString().Trim(), strTREATNO);
                }

                dt.Dispose();
                rtnVal = true;
            }
            catch(Exception ex)
            {
                SaveCvtLog(strPatid, strTREATNO, "EEG(뇌파) 에러");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        /// <summary>
        /// EEG 뇌파검사결과지
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <returns></returns>
        public static bool New_Exam_EEG_Result(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode,
                                  string strClass, string ArgRead_Wrtno, string argRdate, string strTREATNO)
        {

            if (ArgRead_Wrtno.Equals("1"))
                return false;

            #region 변수
            bool rtnVal = false;
            //int LNGY = 40;
            int lngLine = 0;
            string[] strResult;
            //string[] strResult1;
            string strTResult = string.Empty;
            //int PAGENO = 0;

            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            string strDrName = string.Empty;
            //string strPath 

            #endregion

            try
            {


                SQL = " SELECT PANO,SNAME,AGE ||'/'|| SEX AGESEX , (CASE WHEN IPDOPD ='I' THEN '입원' ELSE '외래' END) GBIO  ," + ComNum.VBLF+
                      " WARDCODE ||'/'||ROOMCODE WARD, D.DEPTNAMEK,DRCODE ,XDRCODE1,READDATE , RESULT , RESULT1  ,XNAME , SYSDATE" + ComNum.VBLF+
                      "    FROM KOSMOS_PMPA.XRAY_RESULTNEW X, kosmos_pmpa.BAS_CLINICDEPT  D " + ComNum.VBLF +
                      "   WHERE D.DEPTCODE =X.DEPTCODE " + ComNum.VBLF+
                      "     AND WRTNO =  '" + ArgRead_Wrtno + "' " + ComNum.VBLF+
                      "     AND APPROVE = 'Y' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }


                New_initFormEEG("뇌파검사(EEG) Study Report");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strDrName = dt.Rows[i]["DrCode"].ToString().Trim();


                    if (string.IsNullOrWhiteSpace(strDrName)) strDrName = New_select_Doccode(pDbCon, ArgRead_Wrtno);

                    WriteStr(25, 250, 245, dt.Rows[i]["Pano"].ToString());
                    WriteStr(25, 250, 295, dt.Rows[i]["DeptNamek"].ToString());
                    WriteStr(25, 740, 245, dt.Rows[i]["sName"].ToString());
                    WriteStr(25, 740, 295, New_Spec_SelectDocName(pDbCon, dt.Rows[i]["DrCode"].ToString()));
                    WriteStr(25, 1350, 245, dt.Rows[i]["agesex"].ToString());
                    WriteStr(25, 1440, 245, strClass.Trim().Equals("O") ? dt.Rows[i]["GBio"].ToString() : dt.Rows[i]["Ward"].ToString());
                    WriteStr(25, 1350, 295, Convert.ToDateTime(argRdate).ToString("yyyy-MM-dd"));// '검사요청일

                    WriteStr(25, 250, 2060, Convert.ToDateTime(dt.Rows[i]["ReadDate"].ToString()).ToString("yyyy-MM-dd"));// '판독일자


                    WriteStr(25, 1030, 2060, "판독의사: " + READ_PassName(pDbCon, dt.Rows[i]["xdrcode1"].ToString()));


                    WriteStr(25, 90, 450, "검사명 : " + dt.Rows[i]["xname"].ToString());


                    strTResult = dt.Rows[i]["RESULT"].ToString().Trim() + dt.Rows[i]["RESULT1"].ToString().Trim();
                    strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                    strResult = strTResult.Split('\n');

                    for (int k = 0; k < strResult.Length; k++)
                    {
                        if (string.IsNullOrWhiteSpace(strResult[k]))
                        {
                            lngLine += 1;
                        }
                        else
                        {
                            #region 기존
                            //int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                            //if (strByte >= 325)
                            //{
                            //    WriteStr(20, 70, 550 + (lngLine * 35), VB.Left(strResult[k], 65));
                            //    lngLine += 1;

                            //    if (strResult[k].Length - 65 > 65)
                            //    {
                            //        WriteStr(20, 70, 550 + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));
                            //        lngLine += 1;
                            //        WriteStr(20, 70, 550 + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 130));

                            //    }
                            //    else
                            //    {
                            //        WriteStr(20, 70, 550 + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));

                            //    }
                            //}
                            //else
                            //{
                            //    WriteStr(22, 70, 550 + (lngLine * 35), strResult[k]);
                            //}
                            #endregion

                            #region 신규
                            int lastPos = 0;
                            using (Font font = new Font(FontName, 20))
                            {
                                Size PageSize = new Size(1648 - 70, 20);
                                Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                                if (strWidth.Width < 1100)
                                {
                                    WriteStr(20, 70, 550 + (lngLine * 35), strResult[k]);
                                    lngLine += 1;
                                }
                                else
                                {
                                    for (int l = 0; l < strResult[k].Length + 1;l++)
                                    {
                                        bool DataOutPut = false;
                                        string strText = strResult[k].Substring(lastPos, l);
                                        strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                        if (strWidth.Width >= 1100)
                                        {
                                            lastPos += l;
                                            l = 0;
                                            WriteStr(20, 70, 550 + (lngLine * 35), strText);
                                            lngLine += 1;
                                            DataOutPut = true;
                                        }


                                        if (DataOutPut && lastPos + l == strResult[k].Length)
                                        {
                                            break;
                                        }

                                        if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                        {
                                            WriteStr(20, 70, 550 + (lngLine * 35), strText);
                                            lngLine += 1;
                                            break;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                        #region New Page
                        if (lngLine > 37)
                        {
                            TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                            //LNGY = 40;
                            lngLine = 1;
                            PageNum += 1;

                            New_initFormEEG("뇌파검사(EEG) Study Report");

                            WriteStr(25, 250, 245, dt.Rows[i]["Pano"].ToString());
                            WriteStr(25, 250, 245, dt.Rows[i]["Pano"].ToString());
                            WriteStr(25, 250, 295, dt.Rows[i]["DeptNamek"].ToString());
                            WriteStr(25, 740, 245, dt.Rows[i]["sName"].ToString());
                            WriteStr(25, 740, 295, New_Spec_SelectDocName(pDbCon, dt.Rows[i]["DrCode"].ToString()));
                            WriteStr(25, 1350, 245, dt.Rows[i]["agesex"].ToString());
                            WriteStr(25, 1440, 245, strClass.Trim().Equals("O") ? dt.Rows[i]["GBio"].ToString() : dt.Rows[i]["Ward"].ToString());
                            WriteStr(25, 1350, 295, Convert.ToDateTime(argRdate).ToString("yyyy-MM-dd"));// '검사요청일


                            WriteStr(25, 250, 2060, Convert.ToDateTime(dt.Rows[i]["ReadDate"].ToString()).ToString("yyyy-MM-dd"));// '판독일자


                            WriteStr(25, 1030, 2060, "판독의사: " + READ_PassName(pDbCon, dt.Rows[i]["xdrcode1"].ToString()));


                            WriteStr(25, 90, 450, "검사명 : " + dt.Rows[i]["xname"].ToString());
                        }
                        #endregion
                    }
                }

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                rtnVal = true;


            }
            catch (Exception ex)
            {
                SaveCvtLog(strPatid, strTREATNO, "EEG(뇌파) 에러");
                clsDB.SaveSqlErrLog(ex.Message + "\r\nEEG(뇌파) 에러", SQL, clsDB.DbCon);
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;

        }

        public static void New_initFormEEG(string str, string ArgDrName = "")
        {
            #region 이미지 그리기
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(40, 450, 80,  str);

            WriteStr(25, 50, 200,  "======================================================================================");
            WriteStr(25, 50, 340,  "======================================================================================");

            WriteStr(25, 80, 245,  "등록번호:"    );
            WriteStr(25, 80, 295,  "의 뢰 과:"    );
            WriteStr(25, 570, 245,  "성 명 : "   );
            WriteStr(25, 570, 295,  "의 사 : "   );
            WriteStr(25, 1140, 245,  "성    별: ");
            WriteStr(25, 1140, 295,  "의뢰일자: " );

            WriteStr(25, 60, 2060, "판독일자: ");

            WriteStr(25, 50, 2100, "-------------------------------------------------------------------------------------");

            WriteStr(25, 70, 2140, "* 포항성모병원 신경과 *          전화:054-260-8157");
            WriteStr(25, 70, 2180, "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.");

            #endregion
        }

        #endregion

        #region 내시경
        public static bool New_Exam_Endo(PsmhDb pDbCon, string var, string strPatid, string strDate, string strClinCode, string strClass, string strOutDate, string strDocCode, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            gstrFormcode = strClass.Trim().Equals("I") ? "128" : "135";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            PageNum = 0;

            //string FileName = string.Empty;
            string strPtno = string.Empty;
            //string strPath = string.Empty;
            //string strPathB = string.Empty;
            //string strPathR = string.Empty;
            strOutDate = string.IsNullOrWhiteSpace(strOutDate) ? ComQuery.CurrentDateTime(pDbCon, "S").Substring(0, 10) : strOutDate;
            #endregion

            try
            {

                SQL = " SELECT A.Seqno, A.GBJOB, A.GBNew,  ROWID, TO_CHAR(A.RESULTDATE, 'YYYY-MM-DD') RESULTDATE  ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.ENDO_JUPMST  A ";
                SQL += ComNum.VBLF + " WHERE A.PTNO = '" + strPatid + "' ";
                SQL += ComNum.VBLF + "   AND A.RESULTDATE IS NOT NULL ";

                //'김민원 오동호류마티스내과....
                if (strClinCode.Equals("RA") || strClinCode.Equals("MR"))
                {
                    if (strClass.Equals("O"))
                    {
                        SQL += ComNum.VBLF + "  AND A.DEPTCODE IN ('MD' ,'MR') ";
                        SQL += ComNum.VBLF + "  AND A.GBIO ='" + strClass + "' ";
                        SQL += ComNum.VBLF + "  AND to_char(A.JDATE,'YYYYMMDD') ='" + strDate + "'  and A.DRCODE IN ( '1107','1125' ,'0901','0902','0903') ";
                    }
                    else if (strClass.Equals("I"))
                    {
                        SQL += ComNum.VBLF + "  AND to_char(A.JDATE,'YYYYMMDD') >='" + strDate + "' ";
                        SQL += ComNum.VBLF + "  AND to_char(A.JDATE,'YYYYMMDD') <='" + strOutDate + "'   ";
                    }
                }
                else
                {
                    if (strClass.Equals("O"))
                    {
                        SQL += ComNum.VBLF + "  AND A.DEPTCODE ='" + strClinCode + "' ";
                        SQL += ComNum.VBLF + "  AND A.GBIO ='" + strClass + "' ";
                        SQL += ComNum.VBLF + "  AND to_char(A.JDATE,'YYYYMMDD') ='" + strDate + "' ";
                    }
                    else if (strClass.Equals("I"))
                    {
                        SQL += ComNum.VBLF + "  AND ( A.GBIO ='" + strClass + "' or A.DeptCode ='ER') ";// '당일입원 등록요청
                        SQL += ComNum.VBLF + "  AND to_char(A.JDATE,'YYYYMMDD') >='" + strDate + "' ";

                        SQL += ComNum.VBLF + "  AND to_char(A.JDATE,'YYYYMMDD') <='" + strOutDate + "'  ";
                        SQL += ComNum.VBLF + "  AND A.DRCODE NOT IN ('1107','1125','0901','0902','0903')  ";
                    }
                }


                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }


                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;

                }

                SaveCvtLog(strPatid, strTREATNO, "내시경");

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    PageNum += 1;

                    #region ' GBjob  구분 (1:기관지, 2:위, 3:장, 4:ERCP)
                    switch (Trim(dt.Rows[i]["GBJOB"].ToString()))
                    {
                        case "1":
                            if (Convert.ToDateTime(Trim(dt.Rows[i]["RESULTDATE"].ToString())) >= Convert.ToDateTime("2015-08-04"))
                            {
                                New_Exam_Endo_Result_1_NEW2(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), Trim(dt.Rows[i]["SEQNO"].ToString()), Trim(dt.Rows[i]["ROWID"].ToString()), strTREATNO);
                            }
                            else
                            {
                                if (Trim(dt.Rows[i]["gbnew"].ToString()) == "Y")
                                {
                                    New_Exam_Endo_Result_1NEW(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), Trim(dt.Rows[i]["SEQNO"].ToString()), Trim(dt.Rows[i]["ROWID"].ToString()), strTREATNO);
                                }
                                else
                                {
                                    New_Exam_Endo_Result_1(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), Trim(dt.Rows[i]["SEQNO"].ToString()), Trim(dt.Rows[i]["ROWID"].ToString()), strTREATNO);
                                }
                            }
                            break;
                        case "2":
                            if (Convert.ToDateTime(Trim(dt.Rows[i]["RESULTDATE"].ToString())) >= Convert.ToDateTime("2015-08-04"))
                            {
                                 New_Exam_Endo_Result_2_NEW2(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), Trim(dt.Rows[i]["SEQNO"].ToString()), Trim(dt.Rows[i]["ROWID"].ToString()), strTREATNO);
                            }
                            else
                            {
                                if (Trim(dt.Rows[i]["gbnew"].ToString()) == "Y")
                                {

                                    New_Exam_Endo_Result_2NEW(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), Trim(dt.Rows[i]["SEQNO"].ToString()), Trim(dt.Rows[i]["ROWID"].ToString()), strTREATNO);
                                }
                                else
                                {
                                    New_Exam_Endo_Result_2(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), Trim(dt.Rows[i]["SEQNO"].ToString()), Trim(dt.Rows[i]["ROWID"].ToString()), strTREATNO);							   
                                }
                            }
                            break;
                        case "3":
                            if (Convert.ToDateTime(Trim(dt.Rows[i]["RESULTDATE"].ToString())) >= Convert.ToDateTime("2015-08-04"))
                            {
                                New_Exam_Endo_Result_3_NEW2(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), Trim(dt.Rows[i]["SEQNO"].ToString()), Trim(dt.Rows[i]["ROWID"].ToString()), strTREATNO);
                            }
                            else
                            {
                                if (Trim(dt.Rows[i]["gbnew"].ToString()) == "Y")
                                {
                                    New_Exam_Endo_Result_3NEW(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), Trim(dt.Rows[i]["SEQNO"].ToString()), Trim(dt.Rows[i]["ROWID"].ToString()), strTREATNO);
                                }
                                else
                                {
                                    New_Exam_Endo_Result_3(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), Trim(dt.Rows[i]["SEQNO"].ToString()), Trim(dt.Rows[i]["ROWID"].ToString()), strTREATNO);
                                }
                            }
                            break;
                        case "4":
                            if (Convert.ToDateTime(Trim(dt.Rows[i]["RESULTDATE"].ToString())) >= Convert.ToDateTime("2015-08-04"))
                            {
                                New_Exam_Endo_Result_4_NEW2(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), Trim(dt.Rows[i]["SEQNO"].ToString()), Trim(dt.Rows[i]["ROWID"].ToString()), strTREATNO);
                            }
                            else
                            {
                                New_Exam_Endo_Result_4(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), Trim(dt.Rows[i]["SEQNO"].ToString()), Trim(dt.Rows[i]["ROWID"].ToString()), strTREATNO);
                            }
                            break;
                    }
                    #endregion
                }

                dt.Dispose();

                rtnVal = true;
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "내시경 에러");
                //ComFunc.MsgBox(ex.Message);
            }


            return rtnVal;
        }

        /// <summary>
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <param name="ArgGBJob">(1:기관지, 2:위, 3:장, 4:ERCP)</param>
        public static void New_Exam_Endo_Result_1_NEW2(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode,  string strClass, string ArgRowid, string ArgGBJob, string strTREATNO)
        {
            if (string.IsNullOrWhiteSpace(ArgRowid))
                return;

            #region 변수
            //int LNGY = 40;
            int nTop = 580;
            //int nLeft = 150;
            int lngLine = 0;


            string strGu = string.Empty;
            string strHap = string.Empty;
            string strHapGu = string.Empty;
            string strTSName = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            DataTable dt2 = null;

            #endregion


            try
            {
                #region 쿼리

                SQL = "         SELECT  A.PTNO, A.SEX , (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PTNO) SNAME, A.GBIO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SEQNUM, A.ORDERCODE, ";


                //'추가정보 추가
                //'PreMEDICATION Pethidine -----------------------------------------------


                SQL += ComNum.VBLF + "          GBPRE_1,";//   ' PreMEDICATION 여부


                SQL += ComNum.VBLF + "          GBCON_4,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBCON_41,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBCON_42,";//  ' 단위


               // 'PreMEDICATION Atropin
                SQL += ComNum.VBLF + "          GBPRE_4,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBPRE_41,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBPRE_42,";//  ' 단위
                SQL += ComNum.VBLF + "          GBPRE_3, ";//  ' OTHER 여부
                SQL += ComNum.VBLF + "          GBPRE_31, ";// ' OTHER



               // 'Conscious sedation  midasolam -----------------------------------------------------
                SQL += ComNum.VBLF + "          GBCON_1,";//  ' Conscious 여부


                SQL += ComNum.VBLF + "          GBCON_2,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBCON_21,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBCON_22,";//  ' 단위



                //'Conscious sedation Propofol
                SQL += ComNum.VBLF + "          GBCON_3," ;//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBCON_31,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBCON_32,";//  ' 단위


                SQL += ComNum.VBLF + "          GBCON_5, " ;// ' OTHER  여부
                SQL += ComNum.VBLF + "          GBCON_51, ";// ' OTHER


                //'medication Epinephrine --------------------------------------------------
                SQL += ComNum.VBLF + "          GBMED_1,";//   ' medication 여부


                SQL += ComNum.VBLF + "          GBMED_2," ;//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBMED_21,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBMED_22,";//  ' 단위


                SQL += ComNum.VBLF + "          GBMED_3," ;//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBMED_31,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBMED_32,";//  ' 단위


                SQL += ComNum.VBLF + "          GBMED_4, " ;// ' OTHER  여부
                SQL += ComNum.VBLF + "          GBMED_41, ";// ' OTHER


                //'2015-08-05 추가 ---------------------------------------------------------
                SQL += ComNum.VBLF + " A.Gb_Clean, ";// '장정결도


                SQL += ComNum.VBLF + " GUBUN_GUE,MOAAS,D_INTIME1,D_INTIME2,D_EXTIME1,D_EXTIME2,";//
                SQL += ComNum.VBLF + " PRO_BX1,PRO_BX2,PRO_PP1,PRO_PP2,PRO_RUT,PRO_ESD1,PRO_ESD2,PRO_ESD3_1,PRO_ESD3_2,PRO_EMR1,";
                SQL += ComNum.VBLF + " PRO_EMR2,PRO_EMR3_1,PRO_EMR3_2,PRO_APC,PRO_ELEC,PRO_HEMO1,PRO_HEMO2,PRO_EPNA1,";
                SQL += ComNum.VBLF + " PRO_EPNA2,PRO_BAND1,PRO_BAND2,PRO_MBAND,PRO_HIST1,PRO_HIST2,PRO_DETA,PRO_EST,";
                SQL += ComNum.VBLF + " PRO_BALL,PRO_BASKET,PRO_EPBD1,PRO_EPBD2,PRO_EPBD3,PRO_EPBD4,PRO_ENBD1,PRO_ENBD2,";
                SQL += ComNum.VBLF + " PRO_ENBD3 , PRO_ERBD1, PRO_ERBD2, PRO_ERBD3, PRO_ERBD4, PRO_EST_STS,";


                SQL += ComNum.VBLF + " B.Remark6_2,B.Remark6_3,B.Remark, ";


                //'----------------------------------------------------------------------------------


                SQL += ComNum.VBLF + "          TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL += ComNum.VBLF + "          A.RESULTDRCODE, TO_CHAR(A.BirthDate,'YYYY-MM-DD') BIRTHDate, ";
                SQL += ComNum.VBLF + "          B.REMARK1, B.REMARK2, B.REMARK3, B.REMARK4, B.REMARK5, B.REMARK6,";
                SQL += ComNum.VBLF + "          C.REMARKC , C.REMARKX, C.REMARKP, C.REMARKD, A.GUBUN";
                SQL += ComNum.VBLF + "     FROM KOSMOS_OCS.ENDO_JUPMST A, KOSMOS_OCS.ENDO_RESULT B , KOSMOS_OCS.ENDO_REMARK C";
                SQL += ComNum.VBLF + "     WHERE A.SEQNO = " + ArgRowid;
                SQL += ComNum.VBLF + "       AND A.RESULTDATE IS NOT NULL";
                SQL += ComNum.VBLF + "       AND A.SEQNO = B.SEQNO";
                SQL += ComNum.VBLF + "       AND A.PTNO = C.PTNO(+)";
                SQL += ComNum.VBLF + "       AND A.JDATE = C.JDATE"; 
                SQL += ComNum.VBLF + "       AND A.ORDERCODE = C.ORDERCODE";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                New_initFormEndo_1NEW("내 시 경 판 독 결 과 지");

                //'2017-08-18
                strGu = string.Empty;
                if(Trim(dt.Rows[0]["Gubun_Gue"].ToString().Trim()) .Equals("Y"))
                {
                    strGu = "궤양 +";
                }

                switch(dt.Rows[0]["GUBUN"].ToString().Trim())
                {
                    case "01":
                    case "03":
                    case "05":
                    case "07":
                        strHap = "합병증 +";
                        break;

                }

                if (string.IsNullOrWhiteSpace(strGu) == false && string.IsNullOrWhiteSpace(strHap) == false)
                {
                    strHapGu = strHap + " / " + strGu;
                }
                else if(string.IsNullOrWhiteSpace(strGu) == false || string.IsNullOrWhiteSpace(strHap) == false)
                {
                    strHapGu = strHap + strGu;
                }
                else
                {
                    strHapGu = string.Empty;
                }


                string strPano = (dt.Rows[0]["PTNO"].ToString().Trim());
                string strSName = (dt.Rows[0]["sName"].ToString().Trim());
                string strGBIO = (dt.Rows[0]["GBio"].ToString().Trim()).Equals("I") ?"입원" :"외래";


                //'strAge = AGE_YEAR_Gesan2(Trim(dt.Rows[0]["BIRTHdate"].ToString().Trim()), Trim(dt.Rows[0]["JDATE"].ToString().Trim()))
                string strAge = AGE_YEAR_Birth(dt.Rows[0]["BIRTHdate"].ToString().Trim(), Trim(dt.Rows[0]["JDATE"].ToString().Trim())).ToString();
                string strSex = (dt.Rows[0]["Sex"].ToString().Trim());


                string strWard = (dt.Rows[0]["WardCode"].ToString().Trim());
                string strRoomCode = (dt.Rows[0]["roomcode"].ToString().Trim());
                string strDeptName =  clsVbfunc.GetBASClinicDeptNameK(pDbCon, Trim(dt.Rows[0]["DeptCode"].ToString().Trim()));

                string strDrName = clsVbfunc.GetBASDoctorName(pDbCon, Trim(dt.Rows[0]["DrCode"].ToString().Trim()));
                string strJDate = (dt.Rows[0]["JDATE"].ToString().Trim());
                string strRDate = (dt.Rows[0]["Rdate"].ToString().Trim());
                string strResultDate = (dt.Rows[0]["RESULTDATE"].ToString().Trim());


                string strSeqNUM = (dt.Rows[0]["seqnum"].ToString().Trim());
                //'strSDate = Trim(RsVerify!SDate"].ToString().Trim())
                //'strEDate = Trim(RsVerify!EDate"].ToString().Trim())


                if (!string.IsNullOrWhiteSpace(Trim(dt.Rows[0]["RoomCode"].ToString().Trim())))
                {
                    strTSName = strSName +"(" + strSex +"/" + strAge +"세/" + strGBIO +"/" + VB.Left(strRoomCode + VB.Space(3), 3) +"(" + strWard +"))";
                }
                else
                {
                    strTSName = strSName +"(" + strSex +"/" + strAge +"세/" + strGBIO +"/" + VB.Left(strRoomCode + VB.Space(3), 3) +")";
                }
                               
                string strTitle ="등록번호:" + strPano +" 성명:" + strTSName +" Dr:" + strDrName +" No:" + strSeqNUM +" 검사요청일:" + strJDate;
                
                string strRDRName = clsVbfunc.GetInSaName(pDbCon, Trim(dt.Rows[0]["Resultdrcode"].ToString().Trim()));


                #region 서브쿼리1
                SQL =" SELECT      REMARKC , REMARKX, REMARKP, REMARKD";
                SQL += ComNum.VBLF +"  FROM KOSMOS_OCS.ENDO_REMARK";
                SQL += ComNum.VBLF +" WHERE PTNO = '" + strPano +"'";
                SQL += ComNum.VBLF +"   AND JDATE = TO_DATE('" + strJDate +"','YYYY-MM-DD')";
                SQL += ComNum.VBLF +"   AND ORDERCODE = '" + Trim(dt.Rows[0]["ORDERCODE"].ToString()) +"'";


                string strChiefCom = string.Empty;
                string strClinicalDia = string.Empty;

                SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt2.Rows.Count > 0)
                {
                    strChiefCom = dt2.Rows[0]["remarkc"].ToString().Trim().Replace(ComNum.VBLF,"");
                    strClinicalDia = dt2.Rows[0]["remarkd"].ToString().Trim().Replace(ComNum.VBLF,"");
                }

                dt2.Dispose();
                #endregion

                //int nPrintLine = 0;


                WriteStr(22, 50, 170, strTitle);       // 'titel

                WriteStr(25, 650, 230, strChiefCom);//          'Chief complaint
                WriteStr(25, 650, 320, strClinicalDia);//      'clinical Diagnosis


                string strInfo ="◈ Premedication ◈" + ComNum.VBLF;

                #region 데이터
                if (dt.Rows[0]["GBPRE_1"].ToString().Trim().Equals("Y"))
                {
                    strInfo = strInfo + "None ";

                 }
                else
                {
                    //..strInfo = strInfo 
            
                }

                if (dt.Rows[0]["GBCon_4"].ToString().Trim().Equals("Y"))
                {
                    strInfo = strInfo + "Petihdine ";
                }
                else
                {
                    //'strInfo = strInfo + "";
                }


                if (Trim(dt.Rows[0]["GBCon_41"].ToString().Trim()) != "")
                {
                    strInfo = strInfo + dt.Rows[0]["GBCon_41"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_22"].ToString().Trim() + ", ";
            
                }


                if (dt.Rows[0]["GBPRE_3"].ToString().Trim().Equals("Y"))
            
                {
                    strInfo = strInfo + " " + dt.Rows[0]["GBPRE_31"].ToString().Trim();
            
                 }
                else
                {
                    strInfo = strInfo + dt.Rows[0]["GBPRE_31"].ToString().Trim();
                }


                //'atropine

                if (dt.Rows[0]["GBPre_4"].ToString().Trim().Equals("Y"))

                {
                    strInfo = strInfo + "Atropine ";

                }
                else
                {
                    //'strInfo = strInfo + ""
                }

                if (dt.Rows[0]["GBPre_41"].ToString().Trim() != "")
            
                 {
                    strInfo = strInfo + dt.Rows[0]["GBPre_41"].ToString().Trim() + "mg " + dt.Rows[0]["GBPre_42"].ToString().Trim() + ", ";
                }


                strInfo = strInfo + ComNum.VBLF;


                strInfo = strInfo + ComNum.VBLF + "◈ Conscious Sedation ◈" + ComNum.VBLF;


                //'add2
                if (dt.Rows[0]["MOAAS"].ToString().Trim() != "")
            
                {
                    strInfo = strInfo + "MOAAS //Children`s Hospital of Wisconsin sedation Scale " + dt.Rows[0]["MOAAS"].ToString().Trim() + ", ";
                }


                if (dt.Rows[0]["GBCon_1"].ToString().Trim().Equals("Y"))
            
                {
                    strInfo = strInfo + "None ";
                }
                else
                {
                   // 'strInfo = strInfo + "";
            
                }

                if (dt.Rows[0]["GBCon_2"].ToString().Trim().Equals("Y"))
            
                {
                    strInfo = strInfo + "Mediazolam ";
                }
                else
                {
                    //'strInfo = strInfo + "Mediazolam ";
            
                }

                if (dt.Rows[0]["GBCon_21"].ToString().Trim() != "")
            
                {
                    strInfo = strInfo + dt.Rows[0]["GBCon_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_22"].ToString().Trim() + ", ";
                }

                if (dt.Rows[0]["GBCon_3"].ToString().Trim().Equals("Y"))
            
                {
                    strInfo = strInfo + "Propofol ";
                }
                else
                {
                    //'strInfo = strInfo + "Propotol ";
            
                }

                if (dt.Rows[0]["GBCon_31"].ToString().Trim() != "")
                {
                    strInfo = strInfo + dt.Rows[0]["GBCon_31"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_32"].ToString().Trim() + ", ";
                }


                //'remark
                if (dt.Rows[0]["GBCon_5"].ToString().Trim().Equals("Y"))
            
                {
                    strInfo = strInfo + " " + dt.Rows[0]["GBCon_51"].ToString().Trim();
            
                }
                else
                {
                    strInfo = strInfo + dt.Rows[0]["GBCon_51"].ToString().Trim();
            
                }


                strInfo = strInfo + ComNum.VBLF;


                strInfo = strInfo + ComNum.VBLF +"◈ Medication ◈" + ComNum.VBLF;


                //'MED
                if (dt.Rows[0]["GBMed_1"].ToString().Trim().Equals("Y"))
            
                {
                    strInfo = strInfo + "None ";
                }
                else
                {
                    strInfo = strInfo + "";
                }


                if (dt.Rows[0]["GBMed_2"].ToString().Trim().Equals("Y"))
            
                 {
                    strInfo = strInfo + "Epinephrine ";
                }
                else
                {
                    //'strInfo = strInfo + ""
                }

                if ((dt.Rows[0]["GBMed_21"].ToString().Trim() != ""))
            
                {
                    strInfo += dt.Rows[0]["GBMed_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBMed_22"].ToString().Trim() + ", ";
                }

                if ((dt.Rows[0]["GBMed_3"].ToString().Trim().Equals("Y")))
            
                {
                    strInfo = strInfo + "Botrooase ";
                }
                else
                {
                    //'strInfo = strInfo + "Propotol ";
            
                }
    
                if ((dt.Rows[0]["GBMed_31"].ToString().Trim() != ""))
            
                 {
                    strInfo += dt.Rows[0]["GBCon_31"].ToString().Trim() + "KU " + dt.Rows[0]["GBMed_32"].ToString().Trim() + ", ";
                }

                //'remark
                if ((dt.Rows[0]["GBMed_4"].ToString().Trim().Equals("Y")))
            
                {
                    strInfo = strInfo + " " + dt.Rows[0]["GBMed_41"].ToString().Trim();
                }
                else
                {
                    strInfo += dt.Rows[0]["GBMed_41"].ToString().Trim();
                }



                //'2015-07-23
                string strTPro = string.Empty;
                if (dt.Rows[0]["PRO_BX1"].ToString().Trim().Equals("Y")) strTPro += "Bx. bottle ";
                if (dt.Rows[0]["PRO_BX2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_BX2"].ToString().Trim() + "ea, ";


                if (dt.Rows[0]["PRO_PP1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "PP ";
                if (dt.Rows[0]["PRO_PP2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_PP2"].ToString().Trim() + "ea, ";


                if (dt.Rows[0]["PRO_RUT"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Rapid Urease Test, ";


                if (dt.Rows[0]["PRO_ESD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "ESD, ";
                if (dt.Rows[0]["PRO_ESD2"].ToString().Trim().Equals("Y")) strTPro = strTPro + "en-bloc, ";
                if (dt.Rows[0]["PRO_ESD3_1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "piecemeal ";
                if (dt.Rows[0]["PRO_ESD3_2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ESD3_2"].ToString().Trim() + ", " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_EMR1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EMR, ";
                if (dt.Rows[0]["PRO_EMR2"].ToString().Trim().Equals("Y")) strTPro = strTPro + "en-bloc, ";
                if (dt.Rows[0]["PRO_EMR3_1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "piecemeal ";
                if (dt.Rows[0]["PRO_EMR3_2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EMR3_2"].ToString().Trim() + ", " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_APC"].ToString().Trim().Equals("Y")) strTPro = strTPro + "APC, ";
                if (dt.Rows[0]["PRO_ELEC"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Electrocauterization, ";


                if (dt.Rows[0]["PRO_HEMO1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Hemoclip ";
                if (dt.Rows[0]["PRO_HEMO2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_HEMO2"].ToString().Trim() + "ea, " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_EPNA1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EPNA ";
                if (dt.Rows[0]["PRO_EPNA2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPNA2"].ToString().Trim() + "cc, ";



                if (dt.Rows[0]["PRO_MBAND"].ToString().Trim().Equals("Y")) strTPro = strTPro + "multi-band, " + ComNum.VBLF;



                if (dt.Rows[0]["PRO_EST"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EST (";
                if (dt.Rows[0]["PRO_EST_STS"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EST_STS"].ToString().Trim() + ") ";



                if (dt.Rows[0]["PRO_BAND1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Single-band ";
                if (dt.Rows[0]["PRO_BAND2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_BAND2"].ToString().Trim() + "ea, ";



                if (dt.Rows[0]["PRO_HIST1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Histoacyl ";
                if (dt.Rows[0]["PRO_HIST2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_HIST2"].ToString().Trim() + "ample, ";


                if (dt.Rows[0]["PRO_DETA"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Detachable snare, " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_BALL"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Ballooon, ";
                if (dt.Rows[0]["PRO_BASKET"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Basket, " + ComNum.VBLF;



                if (dt.Rows[0]["PRO_EPBD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EPBD ";
                if (dt.Rows[0]["PRO_EPBD2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPBD2"].ToString().Trim() + "mm ";
                if (dt.Rows[0]["PRO_EPBD3"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPBD3"].ToString().Trim() + "atm ";
                if (dt.Rows[0]["PRO_EPBD4"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPBD4"].ToString().Trim() + "sec" + ComNum.VBLF;


                if (dt.Rows[0]["PRO_ENBD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "ENBD ";
                if (dt.Rows[0]["PRO_ENBD2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ENBD2"].ToString().Trim() + "Fr.";
                if (dt.Rows[0]["PRO_ENBD3"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ENBD3"].ToString().Trim() + "type ";


                if (dt.Rows[0]["PRO_ERBD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "ERBD ";
                if (dt.Rows[0]["PRO_ERBD2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ERBD2"].ToString().Trim() + "Fr.";
                if (dt.Rows[0]["PRO_ERBD3"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ERBD3"].ToString().Trim() + "type ";


                string strResult6_new = dt.Rows[0]["Remark6"].ToString().Trim();


                string strTResult = "◈ Vocal Cord ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Carina ◈ " + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Bronchi ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;


                if (strTPro != "")
                {
                    strTResult = strTResult + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + strTPro + ComNum.VBLF + dt.Rows[0]["Remark4"].ToString().Trim() + ComNum.VBLF + (string.IsNullOrEmpty(strHapGu) == false ? strHapGu + ComNum.VBLF : "") + ComNum.VBLF;

                }
                else
                {
                    strTResult = strTResult + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark4"].ToString().Trim() + ComNum.VBLF + (string.IsNullOrEmpty(strHapGu) == false ? strHapGu + ComNum.VBLF : "") + ComNum.VBLF;
                }
                strTResult = strTResult + "◈ Endoscopic Biopsy ◈" + ComNum.VBLF + ComNum.VBLF + strResult6_new + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + strInfo + ComNum.VBLF + ComNum.VBLF;//  'add

                #endregion

                strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                string[] strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 530;


                #region Text Print
                for (int k = 0; k < strResult.Length; k++)
                {
                    if (string.IsNullOrWhiteSpace(strResult[k]))
                    {
                        lngLine += 1;
                    }
                    else
                    {
                        #region 신규
                        int lastPos = 0;
                        using (Font font = new Font(FontName, 20))
                        {
                            Size PageSize = new Size(1648 - (150), 20);
                            Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                            if (strWidth.Width < 1100)
                            {
                                WriteStr(20, 150, nTop + (lngLine * 35), strResult[k]);
                                lngLine += 1;
                            }
                            else
                            {
                                for (int l = 0; l < strResult[k].Length + 1;l++)
                                {
                                    bool DataOutPut = false;
                                    string strText = strResult[k].Substring(lastPos, l);
                                    strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                    if (strWidth.Width >= 1100)
                                    {
                                        lastPos += l;
                                        l = 0;
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        DataOutPut = true;
                                    }

                                    if (DataOutPut && lastPos + l == strResult[k].Length)
                                    {
                                        break;
                                    }

                                    if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                    {
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        break;
                                    }
                                }
                            }
                        }
                        #endregion

                        //#region 기존
                        //int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                        //if (strByte >= 325)
                        //{
                        //    WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Left(strResult[k], 65));
                        //    lngLine += 1;

                        //    if (strResult[k].Length - 65 > 65)
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));
                        //        lngLine += 1;
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 130));

                        //    }
                        //    else
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));

                        //    }
                        //}
                        //else
                        //{
                        //    WriteStr(22, nLeft, nTop + (lngLine * 35), strResult[k]);
                        //}

                        //lngLine += 1;
                        //#endregion
                    }

                    #region NEW PAGE
                    if (nTop + (lngLine * 35) > 2080)
                    {
                        #region 1장 에도 검사결과 등등 정보ㅓㅓ 들어가개ㅔ 2020-03-23
                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);
                        WriteStr(25, 1260, 2230, strRDRName);
                        #endregion
                        WriteStr(25, 750, 2100, "(계속)");

                        TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                        //LNGY = 40;
                        lngLine = 1;
                        PageNum += 1;

                        New_initFormEndo_1("내 시 경 판 독 결 과 지");

                        WriteStr(22, 50, 170, strTitle);//             'titel
                        WriteStr(25, 150, 280, strChiefCom);//         'Chief complaint
                        WriteStr(25, 650, 380, strClinicalDia);//      'clinical Diagnosis
                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);
                        WriteStr(25, 1260, 2230, strRDRName);
                    }
                    #endregion

                }
                #endregion

                WriteStr(30, 300, 2140, strRDate);
                WriteStr(30, 300, 2180, strResultDate);
                WriteStr(30, 950, 2160, strRDRName);
                WriteStr(25, 1260, 2230, strRDRName);
                
                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "내시경 에러");
                //ComFunc.MsgBox(ex.Message);
            }
        }

        public static void New_initFormEndo_1NEW(string str)
        {
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(43, 540, 80, str);

            hline2(50, 150, 1600);
            hline2(50, 220, 1600);
            hline2(50, 430, 1600);

            //Call hline(frmImgConvert.ltkPV, 50, 132, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 190, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 400, 53)

            WriteStr(30, 100, 230, "Chief Complaint");
            WriteStr(30, 100, 320, "Clinical Diagnosis");
            WriteStr(30, 80, 450, "Fiberroptic bronchoscopic report");


            WriteStr(23, 80, 2140, "검사 시행일 :");
            WriteStr(23, 80, 2180, "결과 보고일 :");


            WriteStr(23, 700, 2160, "Reported by :");


            hline2(50, 2220, 1600);
            //Call hline(frmImgConvert.ltkPV, 50, 2200, 53)


            WriteStr(23, 80, 2230, "포항성모병원");
            WriteStr(23, 1100, 2230, "처치의사:");


            WriteStr(25, 70, 2280, "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.");
        }

        public static void New_initFormEndo_1(string str)
        {
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(43, 540, 80, str);

            hline2(50, 150, 1600);
            hline2(50, 220, 1600);
            hline2(50, 430, 1600);

            //Call hline(frmImgConvert.ltkPV, 50, 132, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 190, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 400, 53)
            WriteStr(30, 100, 230, "Chief Complaint");
            WriteStr(30, 100, 320,  "Clinical Diagnosis");
            WriteStr(30, 80, 450,  "Fiberroptic bronchoscopic report");


            WriteStr( 30, 150, 500,  "Vocal cord");

            WriteStr( 30, 150, 800,  "Carina ");

            WriteStr( 30, 150, 1100,  "Bronchi ");

            WriteStr( 30, 150, 1500,  "Endoscopic Biopsy");

            WriteStr( 30, 950, 1500,  "Endoscopic Procedure");

            WriteStr( 23, 80, 2140,  "검사 시행일 :");
            WriteStr( 23, 80, 2180,  "결과 보고일 :");

            WriteStr( 23, 700, 2160,  "Reported by :");

            //Call hline(frmImgConvert.ltkPV, 50, 2200, 53)

            hline2(50, 2220, 1600);

            WriteStr( 23, 80, 2230,  "포항성모병원");
            WriteStr( 23, 1100, 2230,  "처치의사:");

            WriteStr( 25, 70, 2280,  "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.");
        }

        /// <summary>
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <param name="ArgGBJob">(1:기관지, 2:위, 3:장, 4:ERCP)</param>
        public static void New_Exam_Endo_Result_2_NEW2(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string ArgRowid, string ArgGBJob, string strTREATNO)        {
            if (string.IsNullOrWhiteSpace(ArgRowid))
                return;

            #region 변수
            //int LNGY = 40;
            int nTop = 580;
            //int nLeft = 150;
            int lngLine = 0;


            string strGu = string.Empty;
            string strHap = string.Empty;
            string strHapGu = string.Empty;
            string strTSName = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            DataTable dt2 = null;

            #endregion


            try
            {
                #region 쿼리

                SQL = "         SELECT  A.PTNO, A.SEX , (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PTNO) SNAME, A.GBIO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SEQNUM,A.ORDERCODE ,";

                SQL += ComNum.VBLF + "          TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL += ComNum.VBLF + "          A.RESULTDRCODE, TO_CHAR(A.BirthDate,'YYYY-MM-DD') BIRTHDate, ";
                SQL += ComNum.VBLF + "          B.REMARK1, B.REMARK2, B.REMARK3, B.REMARK4, B.REMARK5, B.REMARK6, ";


                //'2015-08-05 추가 ---------------------------------------------------------
                SQL += ComNum.VBLF + " A.Gb_Clean, ";// '장정결도


                SQL += ComNum.VBLF + " GUBUN_GUE,MOAAS,D_INTIME1,D_INTIME2,D_EXTIME1,D_EXTIME2,";
                SQL += ComNum.VBLF + " PRO_BX1,PRO_BX2,PRO_PP1,PRO_PP2,PRO_RUT,PRO_ESD1,PRO_ESD2,PRO_ESD3_1,PRO_ESD3_2,PRO_EMR1,";
                SQL += ComNum.VBLF + " PRO_EMR2,PRO_EMR3_1,PRO_EMR3_2,PRO_APC,PRO_ELEC,PRO_HEMO1,PRO_HEMO2,PRO_EPNA1,";
                SQL += ComNum.VBLF + " PRO_EPNA2,PRO_BAND1,PRO_BAND2,PRO_MBAND,PRO_HIST1,PRO_HIST2,PRO_DETA,PRO_EST,";
                SQL += ComNum.VBLF + " PRO_BALL,PRO_BASKET,PRO_EPBD1,PRO_EPBD2,PRO_EPBD3,PRO_EPBD4,PRO_ENBD1,PRO_ENBD2,";
                SQL += ComNum.VBLF + " PRO_ENBD3 , PRO_ERBD1, PRO_ERBD2, PRO_ERBD3, PRO_ERBD4, PRO_EST_STS,";


                SQL += ComNum.VBLF + " B.Remark6_2,B.Remark6_3,B.Remark, ";


                //'----------------------------------------------------------------------------------


                SQL += ComNum.VBLF + " A.GBPRE_1,A.GBPRE_2,A.GBPRE_21,A.GBPRE_22,A.GBPRE_3,A.GBCON_1,A.GBCON_2,A.GBCON_21,";
                SQL += ComNum.VBLF + " A.GBCON_22,A.GBCON_3,A.GBCON_31,A.GBCON_32,A.GBCON_4,A.GBCON_41,A.GBCON_42,A.GBPRO_1,A.GBPRO_2,A.GBPRE_31, ";
                SQL += ComNum.VBLF + " B.Remark6_2, B.Remark6_3, B.Remark, A.GUBUN   ";


                SQL += ComNum.VBLF + "     FROM KOSMOS_OCS.ENDO_JUPMST A, KOSMOS_OCS.ENDO_RESULT B ";
                SQL += ComNum.VBLF + "     WHERE A.SEQNO = " + ArgRowid;
                SQL += ComNum.VBLF + "       AND A.RESULTDATE IS NOT NULL";
                SQL += ComNum.VBLF + "       AND A.SEQNO = B.SEQNO";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                //'2017-08-18        
                New_initFormEndo_2New("내 시 경 판 독 결 과 지");

                string strPano = (dt.Rows[0]["PTNO"].ToString().Trim());
                string strSName = (dt.Rows[0]["sName"].ToString().Trim());
                string strGBIO = (dt.Rows[0]["GBio"].ToString().Trim()).Equals("I") ? "입원" : "외래";


                //'strAge = AGE_YEAR_Gesan2(Trim(dt.Rows[0]["BIRTHdate"].ToString().Trim()), Trim(dt.Rows[0]["JDATE"].ToString().Trim()))
                string strAge = AGE_YEAR_Birth(dt.Rows[0]["BIRTHdate"].ToString().Trim(), Trim(dt.Rows[0]["JDATE"].ToString().Trim())).ToString();
                string strSex = (dt.Rows[0]["Sex"].ToString().Trim());


                string strWard = (dt.Rows[0]["WardCode"].ToString().Trim());
                string strRoomCode = (dt.Rows[0]["roomcode"].ToString().Trim());
                string strDeptName = clsVbfunc.GetBASClinicDeptNameK(pDbCon, Trim(dt.Rows[0]["DeptCode"].ToString().Trim()));

                string strDrName = clsVbfunc.GetBASDoctorName(pDbCon, Trim(dt.Rows[0]["DrCode"].ToString().Trim()));
                string strJDate = (dt.Rows[0]["JDATE"].ToString().Trim());
                string strRDate = (dt.Rows[0]["Rdate"].ToString().Trim());
                string strResultDate = (dt.Rows[0]["RESULTDATE"].ToString().Trim());


                string strSeqNUM = (dt.Rows[0]["seqnum"].ToString().Trim());
                //'strSDate = Trim(RsVerify!SDate"].ToString().Trim())
                //'strEDate = Trim(RsVerify!EDate"].ToString().Trim())

                strGu = string.Empty;
                if (Trim(dt.Rows[0]["Gubun_Gue"].ToString().Trim()).Equals("Y"))
                {
                    strGu = "궤양 +";
                }

                switch (dt.Rows[0]["GUBUN"].ToString().Trim())
                {
                    case "01":
                    case "03":
                    case "05":
                    case "07":
                        strHap = "합병증 +";
                        break;

                }

                if (string.IsNullOrWhiteSpace(strGu) == false && string.IsNullOrWhiteSpace(strHap) == false)
                {
                    strHapGu = strHap + " / " + strGu;
                }
                else if (string.IsNullOrWhiteSpace(strGu) == false || string.IsNullOrWhiteSpace(strHap) == false)
                {
                    strHapGu = strHap + strGu;
                }
                else
                {
                    strHapGu = string.Empty;
                }


                if (!string.IsNullOrWhiteSpace(Trim(dt.Rows[0]["RoomCode"].ToString().Trim())))
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + "(" + strWard + "))";
                }
                else
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + ")";
                }

                string strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;

                string strRDRName = clsVbfunc.GetInSaName(pDbCon, Trim(dt.Rows[0]["Resultdrcode"].ToString().Trim()));


                #region 서브쿼리1
                SQL = " SELECT      REMARKC , REMARKX, REMARKP, REMARKD";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ENDO_REMARK";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + strPano + "'";
                SQL += ComNum.VBLF + "   AND JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND ORDERCODE = '" + Trim(dt.Rows[0]["ORDERCODE"].ToString()) + "'";


                string strChiefCom = string.Empty;
                string strClinicalDia = string.Empty;

                SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt2.Rows.Count > 0)
                {
                    strChiefCom = dt2.Rows[0]["remarkc"].ToString().Trim().Replace(ComNum.VBLF, "");
                    strClinicalDia = dt2.Rows[0]["remarkd"].ToString().Trim().Replace(ComNum.VBLF, "");
                }

                dt2.Dispose();
                #endregion

                //int nPrintLine = 0;


                WriteStr(22, 50, 170, strTitle);       // 'titel

                WriteStr(25, 150, 280, strChiefCom);//          'Chief complaint
                WriteStr(25, 650, 380, strClinicalDia);//      'clinical Diagnosis


                string strInfo = "◈ Premedication ◈" + ComNum.VBLF;

                #region 데이터
                if (dt.Rows[0]["GBPRE_1"].ToString().Trim().Equals("Y"))
                {
                    strInfo = strInfo + "None ";

                }
                else
                {
                    //..strInfo = strInfo 

                }

                if (dt.Rows[0]["GBPRE_2"].ToString().Trim().Equals("Y"))
                {
                    strInfo = strInfo + "Aigiron ";
                }
                else
                {
                    //'strInfo = strInfo + "";
                }


                if (Trim(dt.Rows[0]["GBPRE_21"].ToString().Trim()) != "")
                {
                    strInfo = strInfo + dt.Rows[0]["GBPRE_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBPRE_22"].ToString().Trim() + ", ";

                }


                if (dt.Rows[0]["GBPRE_3"].ToString().Trim().Equals("Y"))

                {
                    strInfo += " " + dt.Rows[0]["GBPRE_31"].ToString().Trim();

                }
                else
                {
                    strInfo += dt.Rows[0]["GBPRE_31"].ToString().Trim();
                }

                strInfo += ComNum.VBLF;
                strInfo += ComNum.VBLF + "◈ Conscious Sedation ◈" + ComNum.VBLF;


                if (string.IsNullOrWhiteSpace(dt.Rows[0]["MOAAS"].ToString().Trim()) == false)
                {
                    strInfo += "MOAAS //Children`s Hospital of Wisconsin sedation Scale " + dt.Rows[0]["MOAAS"].ToString().Trim() + ", ";
                }

                if (dt.Rows[0]["GBCon_1"].ToString().Trim().Equals("Y"))
                {
                    strInfo += "None ";
                }

                if (dt.Rows[0]["GBCon_2"].ToString().Trim().Equals("Y"))
                {
                    strInfo += "Mediazolam ";
                }

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["GBCon_21"].ToString().Trim()) == false)
                {
                    strInfo += dt.Rows[0]["GBCon_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_22"].ToString().Trim() + ", ";
                }

                if (dt.Rows[0]["GBCon_3"].ToString().Trim().Equals("Y"))
                {
                    strInfo += "Propofol ";
                }

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["GBCon_31"].ToString().Trim()) == false)
                {
                    strInfo += dt.Rows[0]["GBCon_31"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_32"].ToString().Trim() + ", ";
                }

                if (dt.Rows[0]["GBCon_4"].ToString().Trim().Equals("Y"))
                {
                    strInfo += "Pethidine ";
                }


                if (string.IsNullOrWhiteSpace(dt.Rows[0]["GBCon_41"].ToString().Trim()) == false)
                {
                    strInfo += dt.Rows[0]["GBCon_41"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_42"].ToString().Trim() + ", ";
                }

                string strLowTime = string.Empty;
                if (string.IsNullOrWhiteSpace(dt.Rows[0]["D_INTIME1"].ToString().Trim()) == false)
                {
                    strLowTime = "내시경 삽입시간:" + dt.Rows[0]["D_INTIME1"].ToString().Trim() + "분" + dt.Rows[0]["D_INTIME2"].ToString().Trim() + "초";
                    strLowTime = strLowTime + "  회수시간:" + dt.Rows[0]["D_EXTIME1"].ToString().Trim() + "분" + dt.Rows[0]["D_EXTIME2"].ToString().Trim() + "초";
                }


                //'2015-07-23
                string strTPro = string.Empty;
                if (dt.Rows[0]["PRO_BX1"].ToString().Trim().Equals("Y")) strTPro += "Bx. bottle ";
                if (dt.Rows[0]["PRO_BX2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_BX2"].ToString().Trim() + "ea, ";


                if (dt.Rows[0]["PRO_PP1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "PP ";
                if (dt.Rows[0]["PRO_PP2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_PP2"].ToString().Trim() + "ea, ";


                if (dt.Rows[0]["PRO_RUT"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Rapid Urease Test, ";


                if (dt.Rows[0]["PRO_ESD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "ESD, ";
                if (dt.Rows[0]["PRO_ESD2"].ToString().Trim().Equals("Y")) strTPro = strTPro + "en-bloc, ";
                if (dt.Rows[0]["PRO_ESD3_1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "piecemeal ";
                if (dt.Rows[0]["PRO_ESD3_2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ESD3_2"].ToString().Trim() + ", " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_EMR1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EMR, ";
                if (dt.Rows[0]["PRO_EMR2"].ToString().Trim().Equals("Y")) strTPro = strTPro + "en-bloc, ";
                if (dt.Rows[0]["PRO_EMR3_1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "piecemeal ";
                if (dt.Rows[0]["PRO_EMR3_2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EMR3_2"].ToString().Trim() + ", " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_APC"].ToString().Trim().Equals("Y")) strTPro = strTPro + "APC, ";
                if (dt.Rows[0]["PRO_ELEC"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Electrocauterization, ";


                if (dt.Rows[0]["PRO_HEMO1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Hemoclip ";
                if (dt.Rows[0]["PRO_HEMO2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_HEMO2"].ToString().Trim() + "ea, " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_EPNA1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EPNA ";
                if (dt.Rows[0]["PRO_EPNA2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPNA2"].ToString().Trim() + "cc, ";



                if (dt.Rows[0]["PRO_MBAND"].ToString().Trim().Equals("Y")) strTPro = strTPro + "multi-band, " + ComNum.VBLF;



                if (dt.Rows[0]["PRO_EST"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EST (";
                if (dt.Rows[0]["PRO_EST_STS"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EST_STS"].ToString().Trim() + ") ";



                if (dt.Rows[0]["PRO_BAND1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Single-band ";
                if (dt.Rows[0]["PRO_BAND2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_BAND2"].ToString().Trim() + "ea, ";



                if (dt.Rows[0]["PRO_HIST1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Histoacyl ";
                if (dt.Rows[0]["PRO_HIST2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_HIST2"].ToString().Trim() + "ample, ";


                if (dt.Rows[0]["PRO_DETA"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Detachable snare, " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_BALL"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Ballooon, ";
                if (dt.Rows[0]["PRO_BASKET"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Basket, " + ComNum.VBLF;



                if (dt.Rows[0]["PRO_EPBD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EPBD ";
                if (dt.Rows[0]["PRO_EPBD2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPBD2"].ToString().Trim() + "mm ";
                if (dt.Rows[0]["PRO_EPBD3"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPBD3"].ToString().Trim() + "atm ";
                if (dt.Rows[0]["PRO_EPBD4"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPBD4"].ToString().Trim() + "sec" + ComNum.VBLF;


                if (dt.Rows[0]["PRO_ENBD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "ENBD ";
                if (dt.Rows[0]["PRO_ENBD2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ENBD2"].ToString().Trim() + "Fr.";
                if (dt.Rows[0]["PRO_ENBD3"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ENBD3"].ToString().Trim() + "type ";


                if (dt.Rows[0]["PRO_ERBD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "ERBD ";
                if (dt.Rows[0]["PRO_ERBD2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ERBD2"].ToString().Trim() + "Fr.";
                if (dt.Rows[0]["PRO_ERBD3"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ERBD3"].ToString().Trim() + "type ";

                strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;

                string strResult6_new = dt.Rows[0]["Remark6"].ToString().Trim();

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["Remark6"].ToString().Trim()) == false)
                {
                    strResult6_new = "Esophagus:" + dt.Rows[0]["Remark6"].ToString().Trim();
                }

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["Remark6_2"].ToString().Trim()) == false)
                {
                    strResult6_new += ComNum.VBLF + "Stomach:" + dt.Rows[0]["Remark6_2"].ToString().Trim();
                }

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["Remark6_3"].ToString().Trim()) == false)
                {
                    strResult6_new += ComNum.VBLF + "Duodenum:" + dt.Rows[0]["Remark6_3"].ToString().Trim();
                }

                string strTResult = "◈ Esophagus ◈" + ComNum.VBLF + dt.Rows[0]["Remark1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Stomach ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Duodenum ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Endoscopic Diagnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark4"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;

                strTResult += strInfo + ComNum.VBLF + ComNum.VBLF;//  'add


                if (dt.Rows[0]["GBPRO_2"].ToString().Trim().Equals("Y"))
                {
                    if (strTPro != "")
                    {
                        strTResult = strTResult + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + "CLO" + ComNum.VBLF + strTPro + ComNum.VBLF + dt.Rows[0]["Remark5"].ToString().Trim() + ComNum.VBLF + (string.IsNullOrEmpty(strHapGu) == false ? strHapGu + ComNum.VBLF : "") + ComNum.VBLF;

                    }
                    else
                    {
                        strTResult = strTResult + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + "CLO" + ComNum.VBLF + dt.Rows[0]["Remark5"].ToString().Trim() + ComNum.VBLF + (string.IsNullOrEmpty(strHapGu) == false ? strHapGu + ComNum.VBLF : "") + ComNum.VBLF;
                    }
                }
                else
                {
                    if (strTPro != "")
                    {
                        strTResult = strTResult + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + strTPro + ComNum.VBLF + dt.Rows[0]["Remark5"].ToString().Trim() + ComNum.VBLF + (string.IsNullOrEmpty(strHapGu) == false ? strHapGu + ComNum.VBLF : "") + ComNum.VBLF;

                    }
                    else
                    {
                        strTResult = strTResult + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark5"].ToString().Trim() + ComNum.VBLF + (string.IsNullOrEmpty(strHapGu) == false ? strHapGu + ComNum.VBLF : "") + ComNum.VBLF;
                    }
                }

             
                strTResult += "◈ Endoscopic Biopsy ◈" + ComNum.VBLF + ComNum.VBLF + strResult6_new + ComNum.VBLF + ComNum.VBLF;
                //strTResult = strTResult + strInfo + ComNum.VBLF + ComNum.VBLF;//  'add

                #endregion

                string strGubun_Gue = string.Empty;
                #region 2018-11-14 내시경 요청으로 궤양, 합병증, 응급환자, CVR 관련내용 추가표기 요청
                switch (dt.Rows[0]["GUBUN_GUE"].ToString().Trim())
                {
                    case "Y":
                        strGubun_Gue = "궤양(+) ";
                        break;
                    case "N":
                        strGubun_Gue = "궤양(-) ";
                        break;
                }


                string strGubun = string.Empty;
                switch (dt.Rows[0]["GUBUN"].ToString().Trim())
                {
                    case "00":
                        strGubun = "합병증(-) 응급검사(-) CVR(-)";
                        break;
                    case "01":
                        strGubun = "합병증(+) 응급검사(-) CVR(-)";
                        break;
                    case "02":
                        strGubun = "합병증(-) 응급검사(+) CVR(-)";
                        break;
                    case "03":
                        strGubun = "합병증(+) 응급검사(+) CVR(-)";
                        break;
                    case "04":
                        strGubun = "합병증(-) 응급검사(-) CVR(+)";
                        break;
                    case "05":
                        strGubun = "합병증(+) 응급검사(-) CVR(+)";
                        break;
                    case "06":
                        strGubun = "합병증(-) 응급검사(+) CVR(+)";
                        break;
                    case "07":
                        strGubun = "합병증(+) 응급검사(+) CVR(+)";
                        break;
                }
                #endregion

                strTResult += "◈ Remark ◈" + ComNum.VBLF + dt.Rows[0]["Remark"].ToString().Trim() + ComNum.VBLF + strGubun_Gue + strGubun + ComNum.VBLF + ComNum.VBLF;


                strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                string[] strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 530;


                #region Text Print
                for (int k = 0; k < strResult.Length; k++)
                {
                    if (string.IsNullOrWhiteSpace(strResult[k]))
                    {
                        lngLine += 1;
                    }
                    else
                    {
                        #region 신규
                        int lastPos = 0;
                        using (Font font = new Font(FontName, 20))
                        {
                            Size PageSize = new Size(1648 - 150, 20);
                            Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                            if (strWidth.Width < 1100)
                            {
                                WriteStr(20, 150, nTop + (lngLine * 35), strResult[k]);
                                lngLine += 1;
                            }
                            else
                            {
                                for (int l = 0; l < strResult[k].Length + 1;l++)
                                {
                                    bool DataOutPut = false;
                                    string strText = strResult[k].Substring(lastPos, l);
                                    strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                    if (strWidth.Width >= 1100)
                                    {
                                        lastPos += l;
                                        l = 0;
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        DataOutPut = true;
                                    }

                                    if (DataOutPut && lastPos + l == strResult[k].Length)
                                    {
                                        break;
                                    }

                                    if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                    {
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        break;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 기존
                        //int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                        //if (strByte >= 325)
                        //{
                        //    WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Left(strResult[k], 65));
                        //    lngLine += 1;

                        //    if (strResult[k].Length - 65 > 65)
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));
                        //        lngLine += 1;
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 130));

                        //    }
                        //    else
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));

                        //    }
                        //}
                        //else
                        //{
                        //    WriteStr(22, nLeft, nTop + (lngLine * 35), strResult[k]);
                        //}

                        //lngLine += 1;
                        #endregion
                    }

                    #region NEW PAGE
                    if (nTop + (lngLine * 35) > 2080)
                    {
                        #region 1장 에도 검사결과 등등 정보ㅓㅓ 들어가개ㅔ 2020-03-23
                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);
                        WriteStr(25, 1260, 2230, strRDRName);
                        #endregion
                        WriteStr(25, 750, 2100, "(계속)");

                        TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                        //LNGY = 40;
                        lngLine = 1;
                        PageNum += 1;

                        New_initFormEndo_2New("내 시 경 판 독 결 과 지");

                        WriteStr(22, 50, 170, strTitle);//             'titel
                        WriteStr(25, 150, 280, strChiefCom);//         'Chief complaint
                        WriteStr(25, 650, 380, strClinicalDia);//      'clinical Diagnosis
                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);
                        WriteStr(25, 1260, 2230, strRDRName);
                    }
                    #endregion

                }
                #endregion

                WriteStr(30, 300, 2140, strRDate);
                WriteStr(30, 300, 2180, strResultDate);
                WriteStr(30, 950, 2160, strRDRName);
                WriteStr(25, 1260, 2230, strRDRName);

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "내시경 에러");
                //ComFunc.MsgBox(ex.Message);
            }
        }

        public static void New_initFormEndo_2(string str)
        {
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(43, 540, 80, str);

            hline2(50, 150, 1600);
            hline2(50, 220, 1600);
            hline2(50, 430, 1600);

            //Call hline(frmImgConvert.ltkPV, 50, 132, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 190, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 400, 53)
            WriteStr(30, 100, 230, "Chief Complaint");
            WriteStr(30, 100, 320,  "Clinical Diagnosis");
            WriteStr(30, 80, 450,  "Endoscopic findings.");


            WriteStr(30, 150, 500,  "Esophagus :");



            WriteStr(30, 150, 800,  "Stomach : ");


            WriteStr(30, 150, 1100,  "Duodenum : ");


            WriteStr(30, 150, 1300,  "Endoscopic Diagnosis : ");


            WriteStr(30, 150, 1700,  "Endoscopic Biopsy : ");
            WriteStr(30, 950, 1700,  "Endoscopic Procedure : ");



            WriteStr(23, 80, 2140,  "검사 시행일 :");
            WriteStr(23, 80, 2180,  "결과 보고일 :");


            WriteStr(23, 700, 2160,  "Reported by :");

            hline2(50, 2220, 1600);
            //Call hline(frmImgConvert.ltkPV, 50, 2200, 53)

            WriteStr(23, 80, 2230,  "포항성모병원");
            WriteStr(23, 1100, 2230,  "처치의사:");

            WriteStr(25, 70, 2280,  "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.");
        }

        public static void New_initFormEndo_2New(string str)
        {
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(43, 540, 80, str);

            hline2(50, 150, 1600);
            hline2(50, 220, 1600);
            hline2(50, 430, 1600);

            //Call hline(frmImgConvert.ltkPV, 50, 132, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 190, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 400, 53)
            WriteStr(30, 100, 230,  "Chief Complaint");
            WriteStr(30, 100, 320,  "Clinical Diagnosis");
            WriteStr(30, 80, 450,  "Endoscopic findings.");


            WriteStr(23, 80, 2140,  "검사 시행일 :");
            WriteStr(23, 80, 2180,  "결과 보고일 :");


            WriteStr(23, 700, 2160,  "Reported by :");

            hline2(50, 2220, 1600);
            //Call hline(frmImgConvert.ltkPV, 50, 2200, 53)

            WriteStr(23, 80, 2230,  "포항성모병원");
            WriteStr(23, 1100, 2230,  "처치의사:");


            WriteStr(25, 70, 2280,  "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.");
        }


        public static void New_initFormEndo_3(string str)
        {
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(43, 540, 80, str);
            hline2(50, 150, 1600);
            hline2(50, 220, 1600);
            hline2(50, 430, 1600);

            //Call hline(frmImgConvert.ltkPV, 50, 132, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 190, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 400, 53)

            WriteStr(30, 100, 230, "Chief Complaint    :");
            WriteStr(30, 100, 320, "Clinical Diagnosis :");
            WriteStr(30, 80, 450, "Endoscopic Findings.");


            WriteStr(30, 80, 900, "Endoscopic Diagnosis : ");


            WriteStr(30, 80, 1500, "Endoscopic Biopsy");



            WriteStr(30, 950, 1500, "Endoscopic Procedure");



            WriteStr(23, 80, 2140, "검사 시행일 :");
            WriteStr(23, 80, 2180, "결과 보고일 :");


            WriteStr(23, 700, 2160, "Reported by :");
            hline2(50, 2220, 1600);

            //Call hline(frmImgConvert.ltkPV, 50, 2200, 53)

            WriteStr(23, 80, 2230, "포항성모병원");
            WriteStr(23, 1100, 2230, "처치의사:");


            WriteStr(25, 70, 2280, "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.");
        }

        public static void New_initFormEndo_4(string str)
        {
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(43, 540, 80, str);
            hline2(50, 150, 1600);
            hline2(50, 220, 1600);
            hline2(50, 430, 1600);

            //Call hline(frmImgConvert.ltkPV, 50, 132, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 190, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 400, 53)

            WriteStr(30, 100, 230, "Chief Complaint");
            WriteStr(30, 100, 320,  "Clinical Diagnosis");
            WriteStr(30, 80, 450,  "ERCP Findings.");


            WriteStr(30, 150, 500,  "ERCP Finding :");



            WriteStr(30, 150, 900,  "Diagnosis : ");


            WriteStr(30, 150, 1300,  "Plan & Tx : ");




            WriteStr(30, 150, 1700,  "Endoscopic Biopsy : ");
            WriteStr(30, 950, 1700,  "Endoscopic Procedure : ");



            WriteStr(23, 80, 2140,  "검사 시행일 :");
            WriteStr(23, 80, 2180,  "결과 보고일 :");


            WriteStr(23, 700, 2160,  "Reported by :");
            hline2(50, 2220, 1600);

            //Call hline(frmImgConvert.ltkPV, 50, 2200, 53)

            WriteStr(23, 80, 2230,  "포항성모병원");
            WriteStr(23, 1100, 2230,  "처치의사:");


            WriteStr(25, 70, 2280,  "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.");


        }

        /// <summary>
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <param name="ArgGBJob">(1:기관지, 2:위, 3:장, 4:ERCP)</param>
        public static void New_Exam_Endo_Result_3_NEW2(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string ArgRowid, string ArgGBJob, string strTREATNO)
        {
            if (string.IsNullOrWhiteSpace(ArgRowid))
                return;

            #region 변수
            //int LNGY = 40;
            int nTop = 580;
            //int nLeft = 150;
            int lngLine = 0;


            string strGu = string.Empty;
            string strHap = string.Empty;
            string strHapGu = string.Empty;
            string strTSName = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            DataTable dt2 = null;

            #endregion


            try
            {
                #region 쿼리

                SQL = "         SELECT  A.PTNO, A.SEX , (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PTNO) SNAME, A.GBIO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SEQNUM, A.ORDERCODE, ";
                SQL += ComNum.VBLF + "          TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL += ComNum.VBLF + "          A.RESULTDRCODE, TO_CHAR(A.BirthDate,'YYYY-MM-DD') BIRTHDate, "; ;
                SQL += ComNum.VBLF + "          B.REMARK1, B.REMARK2, B.REMARK3, B.REMARK4, B.REMARK5, B.REMARK6, ";
                SQL += ComNum.VBLF + " A.GBPRE_1,A.GBPRE_2,A.GBPRE_21,A.GBPRE_22,A.GBPRE_3,A.GBCON_1,A.GBCON_2,A.GBCON_21,";
                SQL += ComNum.VBLF + " A.GBCON_22,A.GBCON_3,A.GBCON_31,A.GBCON_32,A.GBCON_4,A.GBCON_41,A.GBCON_42,A.GBPRO_1,A.GBPRO_2,A.GBPRE_31, ";
                SQL += ComNum.VBLF + " B.Remark6_2, B.Remark6_3, B.Remark,  ";


                //'2015-08-05 추가 ---------------------------------------------------------
                SQL += ComNum.VBLF + " A.Gb_Clean, ";// '장정결도


                SQL += ComNum.VBLF + " GUBUN_GUE,MOAAS,D_INTIME1,D_INTIME2,D_EXTIME1,D_EXTIME2,";
                SQL += ComNum.VBLF + " PRO_BX1,PRO_BX2,PRO_PP1,PRO_PP2,PRO_RUT, PRO_ESD1,PRO_ESD2,PRO_ESD3_1,PRO_ESD3_2,PRO_EMR1,";
                SQL += ComNum.VBLF + " PRO_EMR2,PRO_EMR3_1,PRO_EMR3_2,PRO_APC,PRO_ELEC,PRO_HEMO1,PRO_HEMO2,PRO_EPNA1,";
                SQL += ComNum.VBLF + " PRO_EPNA2,PRO_BAND1,PRO_BAND2,PRO_MBAND,PRO_HIST1,PRO_HIST2,PRO_DETA,PRO_EST,";
                SQL += ComNum.VBLF + " PRO_BALL,PRO_BASKET,PRO_EPBD1,PRO_EPBD2,PRO_EPBD3,PRO_EPBD4,PRO_ENBD1,PRO_ENBD2,";
                SQL += ComNum.VBLF + " PRO_ENBD3 , PRO_ERBD1, PRO_ERBD2, PRO_ERBD3, PRO_ERBD4, PRO_EST_STS,";
                    ;

                SQL += ComNum.VBLF + " B.Remark6_2,B.Remark6_3,B.Remark, GUBUN ";


                //'----------------------------------------------------------------------------------


                SQL += ComNum.VBLF + "     FROM KOSMOS_OCS.ENDO_JUPMST A, KOSMOS_OCS.ENDO_RESULT B ";
                //' SQL = SQL & "     WHERE A.PTNO ='" & strPatid & "'"
                SQL += ComNum.VBLF + "     WHERE A.SEQNO = " + ArgRowid;
                SQL += ComNum.VBLF + "       AND A.RESULTDATE IS NOT NULL";
                SQL += ComNum.VBLF + "       AND A.SEQNO = B.SEQNO";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }
                
                New_initFormEndo_3New("내 시 경 판 독 결 과 지");

                //'2017-08-18        

                string strPano = (dt.Rows[0]["PTNO"].ToString().Trim());
                string strSName = (dt.Rows[0]["sName"].ToString().Trim());
                string strGBIO = (dt.Rows[0]["GBio"].ToString().Trim()).Equals("I") ? "입원" : "외래";


                //'strAge = AGE_YEAR_Gesan2(Trim(dt.Rows[0]["BIRTHdate"].ToString().Trim()), Trim(dt.Rows[0]["JDATE"].ToString().Trim()))
                string strAge = AGE_YEAR_Birth(dt.Rows[0]["BIRTHdate"].ToString().Trim(), Trim(dt.Rows[0]["JDATE"].ToString().Trim())).ToString();
                string strSex = (dt.Rows[0]["Sex"].ToString().Trim());


                string strWard = (dt.Rows[0]["WardCode"].ToString().Trim());
                string strRoomCode = (dt.Rows[0]["roomcode"].ToString().Trim());
                string strDeptName = clsVbfunc.GetBASClinicDeptNameK(pDbCon, Trim(dt.Rows[0]["DeptCode"].ToString().Trim()));

                string strDrName = clsVbfunc.GetBASDoctorName(pDbCon, Trim(dt.Rows[0]["DrCode"].ToString().Trim()));
                string strJDate = (dt.Rows[0]["JDATE"].ToString().Trim());
                string strRDate = (dt.Rows[0]["Rdate"].ToString().Trim());
                string strResultDate = (dt.Rows[0]["RESULTDATE"].ToString().Trim());


                string strSeqNUM = (dt.Rows[0]["seqnum"].ToString().Trim());
                //'strSDate = Trim(RsVerify!SDate"].ToString().Trim())
                //'strEDate = Trim(RsVerify!EDate"].ToString().Trim())

                strGu = string.Empty;
                if (Trim(dt.Rows[0]["Gubun_Gue"].ToString().Trim()).Equals("Y"))
                {
                    strGu = "궤양 +";
                }

                switch (dt.Rows[0]["GUBUN"].ToString().Trim())
                {
                    case "01":
                    case "03":
                    case "05":
                    case "07":
                        strHap = "합병증 +";
                        break;

                }

                if (string.IsNullOrWhiteSpace(strGu) == false && string.IsNullOrWhiteSpace(strHap) == false)
                {
                    strHapGu = strHap + " / " + strGu;
                }
                else if (string.IsNullOrWhiteSpace(strGu) == false || string.IsNullOrWhiteSpace(strHap) == false)
                {
                    strHapGu = strHap + strGu;
                }
                else
                {
                    strHapGu = string.Empty;
                }


                if (!string.IsNullOrWhiteSpace(Trim(dt.Rows[0]["RoomCode"].ToString().Trim())))
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + "(" + strWard + "))";
                }
                else
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + ")";
                }

                string strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;

                string strRDRName = clsVbfunc.GetInSaName(pDbCon, Trim(dt.Rows[0]["Resultdrcode"].ToString().Trim()));


                #region 서브쿼리1
                SQL = " SELECT      REMARKC , REMARKX, REMARKP, REMARKD";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ENDO_REMARK";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + strPano + "'";
                SQL += ComNum.VBLF + "   AND JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND ORDERCODE = '" + Trim(dt.Rows[0]["ORDERCODE"].ToString()) + "'";


                string strChiefCom = string.Empty;
                string strClinicalDia = string.Empty;

                SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt2.Rows.Count > 0)
                {
                    strChiefCom = dt2.Rows[0]["remarkc"].ToString().Trim().Replace(ComNum.VBLF, "");
                    strClinicalDia = dt2.Rows[0]["remarkd"].ToString().Trim().Replace(ComNum.VBLF, "");
                }

                dt2.Dispose();
                #endregion

                //int nPrintLine = 0;


                WriteStr(22, 50, 170, strTitle);       // 'titel

                WriteStr(25, 550, 230, strChiefCom);//          'Chief complaint
                WriteStr(25, 550, 320, strClinicalDia);//      'clinical Diagnosis


                string strInfo = "◈ Premedication ◈" + ComNum.VBLF;

                #region 데이터
                if (dt.Rows[0]["GBPRE_1"].ToString().Trim().Equals("Y"))
                {
                    strInfo = strInfo + "None ";

                }
                else
                {
                    //..strInfo = strInfo 

                }

                if (dt.Rows[0]["GBPRE_2"].ToString().Trim().Equals("Y"))
                {
                    strInfo = strInfo + "Aigiron ";
                }
                else
                {
                    //'strInfo = strInfo + "";
                }


                if (Trim(dt.Rows[0]["GBPRE_21"].ToString().Trim()) != "")
                {
                    strInfo = strInfo + dt.Rows[0]["GBPRE_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBPRE_22"].ToString().Trim() + ", ";

                }


                if (dt.Rows[0]["GBPRE_3"].ToString().Trim().Equals("Y"))

                {
                    strInfo += " " + dt.Rows[0]["GBPRE_31"].ToString().Trim();

                }
                else
                {
                    strInfo += dt.Rows[0]["GBPRE_31"].ToString().Trim();
                }

                strInfo += ComNum.VBLF;
                strInfo += ComNum.VBLF + "◈ Conscious Sedation ◈" + ComNum.VBLF;


                if (string.IsNullOrWhiteSpace(dt.Rows[0]["MOAAS"].ToString().Trim()) == false)
                {
                    strInfo += "MOAAS //Children`s Hospital of Wisconsin sedation Scale " + dt.Rows[0]["MOAAS"].ToString().Trim() + ", ";
                }

                if (dt.Rows[0]["GBCon_1"].ToString().Trim().Equals("Y"))
                {
                    strInfo += "None ";
                }

                if (dt.Rows[0]["GBCon_2"].ToString().Trim().Equals("Y"))
                {
                    strInfo += "Mediazolam ";
                }

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["GBCon_21"].ToString().Trim()) == false)
                {
                    strInfo += dt.Rows[0]["GBCon_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_22"].ToString().Trim() + ", ";
                }

                if (dt.Rows[0]["GBCon_3"].ToString().Trim().Equals("Y"))
                {
                    strInfo += "Propofol ";
                }

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["GBCon_31"].ToString().Trim()) == false)
                {
                    strInfo += dt.Rows[0]["GBCon_31"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_32"].ToString().Trim() + ", ";
                }

                if (dt.Rows[0]["GBCon_4"].ToString().Trim().Equals("Y"))
                {
                    strInfo += "Pethidine ";
                }


                if (string.IsNullOrWhiteSpace(dt.Rows[0]["GBCon_41"].ToString().Trim()) == false)
                {
                    strInfo += dt.Rows[0]["GBCon_41"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_42"].ToString().Trim() + ", ";
                }

                string strLowTime = string.Empty;
                if (string.IsNullOrWhiteSpace(dt.Rows[0]["D_INTIME1"].ToString().Trim()) == false)
                {
                    strLowTime = "내시경 삽입시간:" + dt.Rows[0]["D_INTIME1"].ToString().Trim() + "분" + dt.Rows[0]["D_INTIME2"].ToString().Trim() + "초";
                    strLowTime = strLowTime + "  회수시간:" + dt.Rows[0]["D_EXTIME1"].ToString().Trim() + "분" + dt.Rows[0]["D_EXTIME2"].ToString().Trim() + "초";
                }


                //'2015-07-23
                string strTPro = string.Empty;
                if (dt.Rows[0]["PRO_BX1"].ToString().Trim().Equals("Y")) strTPro += "Bx. bottle ";
                if (dt.Rows[0]["PRO_BX2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_BX2"].ToString().Trim() + "ea, ";


                if (dt.Rows[0]["PRO_PP1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "PP ";
                if (dt.Rows[0]["PRO_PP2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_PP2"].ToString().Trim() + "ea, ";


                if (dt.Rows[0]["PRO_RUT"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Rapid Urease Test, ";


                if (dt.Rows[0]["PRO_ESD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "ESD, ";
                if (dt.Rows[0]["PRO_ESD2"].ToString().Trim().Equals("Y")) strTPro = strTPro + "en-bloc, ";
                if (dt.Rows[0]["PRO_ESD3_1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "piecemeal ";
                if (dt.Rows[0]["PRO_ESD3_2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ESD3_2"].ToString().Trim() + ", " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_EMR1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EMR, ";
                if (dt.Rows[0]["PRO_EMR2"].ToString().Trim().Equals("Y")) strTPro = strTPro + "en-bloc, ";
                if (dt.Rows[0]["PRO_EMR3_1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "piecemeal ";
                if (dt.Rows[0]["PRO_EMR3_2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EMR3_2"].ToString().Trim() + ", " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_APC"].ToString().Trim().Equals("Y")) strTPro = strTPro + "APC, ";
                if (dt.Rows[0]["PRO_ELEC"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Electrocauterization, ";


                if (dt.Rows[0]["PRO_HEMO1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Hemoclip ";
                if (dt.Rows[0]["PRO_HEMO2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_HEMO2"].ToString().Trim() + "ea, " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_EPNA1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EPNA ";
                if (dt.Rows[0]["PRO_EPNA2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPNA2"].ToString().Trim() + "cc, ";



                if (dt.Rows[0]["PRO_MBAND"].ToString().Trim().Equals("Y")) strTPro = strTPro + "multi-band, " + ComNum.VBLF;



                if (dt.Rows[0]["PRO_EST"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EST (";
                if (dt.Rows[0]["PRO_EST_STS"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EST_STS"].ToString().Trim() + ") ";



                if (dt.Rows[0]["PRO_BAND1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Single-band ";
                if (dt.Rows[0]["PRO_BAND2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_BAND2"].ToString().Trim() + "ea, ";



                if (dt.Rows[0]["PRO_HIST1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Histoacyl ";
                if (dt.Rows[0]["PRO_HIST2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_HIST2"].ToString().Trim() + "ample, ";


                if (dt.Rows[0]["PRO_DETA"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Detachable snare, " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_BALL"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Ballooon, ";
                if (dt.Rows[0]["PRO_BASKET"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Basket, " + ComNum.VBLF;



                if (dt.Rows[0]["PRO_EPBD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EPBD ";
                if (dt.Rows[0]["PRO_EPBD2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPBD2"].ToString().Trim() + "mm ";
                if (dt.Rows[0]["PRO_EPBD3"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPBD3"].ToString().Trim() + "atm ";
                if (dt.Rows[0]["PRO_EPBD4"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPBD4"].ToString().Trim() + "sec" + ComNum.VBLF;


                if (dt.Rows[0]["PRO_ENBD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "ENBD ";
                if (dt.Rows[0]["PRO_ENBD2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ENBD2"].ToString().Trim() + "Fr.";
                if (dt.Rows[0]["PRO_ENBD3"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ENBD3"].ToString().Trim() + "type ";


                if (dt.Rows[0]["PRO_ERBD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "ERBD ";
                if (dt.Rows[0]["PRO_ERBD2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ERBD2"].ToString().Trim() + "Fr.";
                if (dt.Rows[0]["PRO_ERBD3"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ERBD3"].ToString().Trim() + "type ";

                strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;

                string strResult6_new = dt.Rows[0]["Remark6"].ToString().Trim();

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["Remark6"].ToString().Trim()) == false)
                {
                    strResult6_new = "small Intestinal:" + dt.Rows[0]["Remark6"].ToString().Trim();
                }

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["Remark6_2"].ToString().Trim()) == false)
                {
                    strResult6_new += ComNum.VBLF + "large Intestinal:" + dt.Rows[0]["Remark6_2"].ToString().Trim();
                }

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["Remark6_3"].ToString().Trim()) == false)
                {
                    strResult6_new += ComNum.VBLF + "rectum:" + dt.Rows[0]["Remark6_3"].ToString().Trim();
                }

                string strTResult = "◈ small Intestinal ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ large Intestinal ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark4"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ rectum ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark5"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Endoscopic Diagnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;

                if (string.IsNullOrWhiteSpace(strLowTime) == false)
                {
                    strTResult += "◈ 장정결도 ◈" + ComNum.VBLF + dt.Rows[0]["Gb_Clean"].ToString().Trim() + ComNum.VBLF + strLowTime + ComNum.VBLF + ComNum.VBLF;//   '2013-06-17;
                }
                else
                {
                    strTResult += "◈ 장정결도 ◈" + ComNum.VBLF + dt.Rows[0]["Gb_Clean"].ToString().Trim() + ComNum.VBLF + strLowTime + ComNum.VBLF + ComNum.VBLF;//   '2013-06-17;
                }

                strTResult += strInfo + ComNum.VBLF + ComNum.VBLF;//  'add


                if (strTPro != "")
                {
                    strTResult = strTResult + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + strTPro + ComNum.VBLF + dt.Rows[0]["Remark3"].ToString().Trim() + ComNum.VBLF + (string.IsNullOrEmpty(strHapGu) == false ? strHapGu + ComNum.VBLF : "") + ComNum.VBLF;
                }
                else
                {
                    strTResult = strTResult + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark3"].ToString().Trim() + ComNum.VBLF + (string.IsNullOrEmpty(strHapGu) == false ? strHapGu + ComNum.VBLF : "") + ComNum.VBLF;
                }
               
                strTResult = strTResult + "◈ Endoscopic Biopsy ◈" + ComNum.VBLF + ComNum.VBLF + strResult6_new + ComNum.VBLF + ComNum.VBLF;


                string strGubun = string.Empty;
                switch (dt.Rows[0]["GUBUN"].ToString().Trim())
                {
                    case "00":
                        strGubun = "합병증(-) 응급검사(-) CVR(-)";
                        break;
                    case "01":
                        strGubun = "합병증(+) 응급검사(-) CVR(-)";
                        break;
                    case "02":
                        strGubun = "합병증(-) 응급검사(+) CVR(-)";
                        break;
                    case "03":
                        strGubun = "합병증(+) 응급검사(+) CVR(-)";
                        break;
                    case "04":
                        strGubun = "합병증(-) 응급검사(-) CVR(+)";
                        break;
                    case "05":
                        strGubun = "합병증(+) 응급검사(-) CVR(+)";
                        break;
                    case "06":
                        strGubun = "합병증(-) 응급검사(+) CVR(+)";
                        break;
                    case "07":
                        strGubun = "합병증(+) 응급검사(+) CVR(+)";
                        break;
                }
                

                strTResult += "◈ Remark ◈" + ComNum.VBLF + dt.Rows[0]["Remark"].ToString().Trim() + ComNum.VBLF  + strGubun + ComNum.VBLF + ComNum.VBLF;




                #endregion

                strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                string[] strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 470;


                #region Text Print
                for (int k = 0; k < strResult.Length; k++)
                {
                    if (string.IsNullOrWhiteSpace(strResult[k]))
                    {
                        lngLine += 1;
                    }
                    else
                    {
                        #region 신규
                        int lastPos = 0;
                        using (Font font = new Font(FontName, 20))
                        {
                            Size PageSize = new Size(1648 - 150, 20);
                            Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                            if (strWidth.Width < 1100)
                            {
                                WriteStr(20, 150, nTop + (lngLine * 35), strResult[k]);
                                lngLine += 1;
                            }
                            else
                            {
                                for (int l = 0; l < strResult[k].Length + 1;l++)
                                {
                                    bool DataOutPut = false;
                                    string strText = strResult[k].Substring(lastPos, l);
                                    strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                    if (strWidth.Width >= 1100)
                                    {
                                        lastPos += l;
                                        l = 0;
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        DataOutPut = true;
                                    }

                                    if (DataOutPut && lastPos + l == strResult[k].Length)
                                    {
                                        break;
                                    }

                                    if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                    {
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        break;
                                    }
                                }
                            }
                        }
                        #endregion
                        //#region 기존
                        //int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                        //if (strByte >= 85)
                        //{
                        //    WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Left(strResult[k], 85));
                        //    lngLine += 1;

                        //    if (strResult[k].Length - 85 > 85)
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 85));
                        //        lngLine += 1;
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 170));

                        //    }
                        //    else
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 85));

                        //    }
                        //}
                        //else
                        //{
                        //    WriteStr(22, nLeft, nTop + (lngLine * 35), strResult[k]);
                        //}

                        //lngLine += 1;
                        //#endregion
                    }

                    #region NEW PAGE
                    if (nTop + (lngLine * 35) > 2080)
                    {
                        #region 1장 에도 검사결과 등등 정보ㅓㅓ 들어가개ㅔ 2020-03-23
                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);
                        WriteStr(25, 1260, 2230, strRDRName);
                        #endregion
                        WriteStr(25, 750, 2100, "(계속)");

                        TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                        //LNGY = 40;
                        lngLine = 1;
                        PageNum += 1;

                        New_initFormEndo_3New("내 시 경 판 독 결 과 지");

                        WriteStr(22, 50, 170, strTitle);//             'titel
                        WriteStr(25, 550, 230, strChiefCom);//         'Chief complaint
                        WriteStr(25, 550, 320, strClinicalDia);//      'clinical Diagnosis


                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);


                        WriteStr(25, 1260, 2230, strRDRName);
                    }
                    #endregion

                }
                #endregion

                WriteStr(30, 300, 2140, strRDate);
                WriteStr(30, 300, 2180, strResultDate);
                WriteStr(30, 950, 2160, strRDRName);
                WriteStr(25, 1260, 2230, strRDRName);

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "내시경 에러");
                //ComFunc.MsgBox(ex.Message);
            }
        }

        public static void New_initFormEndo_3New(string str)
        {
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(43, 540, 80, str);
            hline2(50, 150, 1600);
            hline2(50, 220, 1600);
            hline2(50, 430, 1600);

            //Call hline(frmImgConvert.ltkPV, 50, 132, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 190, 53)
            //Call hline(frmImgConvert.ltkPV, 50, 400, 53)
            WriteStr(30, 100, 230, "Chief Complaint");
            WriteStr(30, 100, 320, "Clinical Diagnosis");
            WriteStr(30, 80, 450, "Endoscopic findings.");


            WriteStr(23, 80, 2140, "검사 시행일 :");
            WriteStr(23, 80, 2180, "결과 보고일 :");


            WriteStr(23, 700, 2160, "Reported by :");
            hline2(50, 2220, 1600);

            //Call hline(frmImgConvert.ltkPV, 50, 2200, 53)

            WriteStr(23, 80, 2230, "포항성모병원");
            WriteStr(23, 1100, 2230, "처치의사:");


            WriteStr(25, 70, 2280, "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.");
        }

        /// <summary>
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <param name="ArgGBJob">(1:기관지, 2:위, 3:장, 4:ERCP)</param>
        public static void New_Exam_Endo_Result_4_NEW2(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string ArgRowid, string ArgGBJob, string strTREATNO)
        {
            if (string.IsNullOrWhiteSpace(ArgRowid))
                return;

            #region 변수
            //int LNGY = 40;
            int nTop = 580;
            //int nLeft = 150;
            int lngLine = 0;


            string strGu = string.Empty;
            string strHap = string.Empty;
            string strHapGu = string.Empty;
            string strTSName = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            DataTable dt2 = null;

            #endregion


            try
            {
                #region 쿼리

                SQL = "         SELECT  A.PTNO, A.SEX , (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PTNO) SNAME, A.GBIO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SEQNUM, A.ORDERCODE, ";
                SQL += ComNum.VBLF + "          TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL += ComNum.VBLF + "          A.RESULTDRCODE, TO_CHAR(A.BirthDate,'YYYY-MM-DD') BIRTHDate, "; ;
                SQL += ComNum.VBLF + "          B.REMARK1, B.REMARK2, B.REMARK3, B.REMARK4, B.REMARK5, B.REMARK6, ";
                SQL += ComNum.VBLF + " A.GBPRE_1,A.GBPRE_2,A.GBPRE_21,A.GBPRE_22,A.GBPRE_3,A.GBCON_1,A.GBCON_2,A.GBCON_21,";
                SQL += ComNum.VBLF + " A.GBCON_22,A.GBCON_3,A.GBCON_31,A.GBCON_32,A.GBCON_4,A.GBCON_41,A.GBCON_42,A.GBPRO_1,A.GBPRO_2,A.GBPRE_31, ";
                SQL += ComNum.VBLF + " B.Remark6_2, B.Remark6_3, B.Remark,  ";


                //'2015-08-05 추가 ---------------------------------------------------------
                SQL += ComNum.VBLF + " A.Gb_Clean, ";// '장정결도


                SQL += ComNum.VBLF + " GUBUN_GUE,MOAAS,D_INTIME1,D_INTIME2,D_EXTIME1,D_EXTIME2,";
                SQL += ComNum.VBLF + " PRO_BX1,PRO_BX2,PRO_PP1,PRO_PP2,PRO_RUT, PRO_ESD1,PRO_ESD2,PRO_ESD3_1,PRO_ESD3_2,PRO_EMR1,";
                SQL += ComNum.VBLF + " PRO_EMR2,PRO_EMR3_1,PRO_EMR3_2,PRO_APC,PRO_ELEC,PRO_HEMO1,PRO_HEMO2,PRO_EPNA1,";
                SQL += ComNum.VBLF + " PRO_EPNA2,PRO_BAND1,PRO_BAND2,PRO_MBAND,PRO_HIST1,PRO_HIST2,PRO_DETA,PRO_EST,";
                SQL += ComNum.VBLF + " PRO_BALL,PRO_BASKET,PRO_EPBD1,PRO_EPBD2,PRO_EPBD3,PRO_EPBD4,PRO_ENBD1,PRO_ENBD2,";
                SQL += ComNum.VBLF + " PRO_ENBD3 , PRO_ERBD1, PRO_ERBD2, PRO_ERBD3, PRO_ERBD4, PRO_EST_STS,";
                ;

                SQL += ComNum.VBLF + " B.Remark6_2,B.Remark6_3,B.Remark, A.GUBUN ";


                //'----------------------------------------------------------------------------------


                SQL += ComNum.VBLF + "     FROM KOSMOS_OCS.ENDO_JUPMST A, KOSMOS_OCS.ENDO_RESULT B ";
                //' SQL = SQL & "     WHERE A.PTNO ='" & strPatid & "'"
                SQL += ComNum.VBLF + "     WHERE A.SEQNO = " + ArgRowid;
                SQL += ComNum.VBLF + "       AND A.RESULTDATE IS NOT NULL";
                SQL += ComNum.VBLF + "       AND A.SEQNO = B.SEQNO";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                New_initFormEndo_4New("내 시 경 판 독 결 과 지");


                //'2017-08-18        

                string strPano = (dt.Rows[0]["PTNO"].ToString().Trim());
                string strSName = (dt.Rows[0]["sName"].ToString().Trim());
                string strGBIO = (dt.Rows[0]["GBio"].ToString().Trim()).Equals("I") ? "입원" : "외래";


                //'strAge = AGE_YEAR_Gesan2(Trim(dt.Rows[0]["BIRTHdate"].ToString().Trim()), Trim(dt.Rows[0]["JDATE"].ToString().Trim()))
                string strAge = AGE_YEAR_Birth(dt.Rows[0]["BIRTHdate"].ToString().Trim(), Trim(dt.Rows[0]["JDATE"].ToString().Trim())).ToString();
                string strSex = (dt.Rows[0]["Sex"].ToString().Trim());


                string strWard = (dt.Rows[0]["WardCode"].ToString().Trim());
                string strRoomCode = (dt.Rows[0]["roomcode"].ToString().Trim());
                string strDeptName = clsVbfunc.GetBASClinicDeptNameK(pDbCon, Trim(dt.Rows[0]["DeptCode"].ToString().Trim()));

                string strDrName = clsVbfunc.GetBASDoctorName(pDbCon, Trim(dt.Rows[0]["DrCode"].ToString().Trim()));
                string strJDate = (dt.Rows[0]["JDATE"].ToString().Trim());
                string strRDate = (dt.Rows[0]["Rdate"].ToString().Trim());
                string strResultDate = (dt.Rows[0]["RESULTDATE"].ToString().Trim());


                string strSeqNUM = (dt.Rows[0]["seqnum"].ToString().Trim());
                //'strSDate = Trim(RsVerify!SDate"].ToString().Trim())
                //'strEDate = Trim(RsVerify!EDate"].ToString().Trim())

                strGu = string.Empty;
                if (Trim(dt.Rows[0]["Gubun_Gue"].ToString().Trim()).Equals("Y"))
                {
                    strGu = "궤양 +";
                }

                switch (dt.Rows[0]["GUBUN"].ToString().Trim())
                {
                    case "01":
                    case "03":
                    case "05":
                    case "07":
                        strHap = "합병증 +";
                        break;

                }

                if (string.IsNullOrWhiteSpace(strGu) == false && string.IsNullOrWhiteSpace(strHap) == false)
                {
                    strHapGu = strHap + " / " + strGu;
                }
                else if (string.IsNullOrWhiteSpace(strGu) == false || string.IsNullOrWhiteSpace(strHap) == false)
                {
                    strHapGu = strHap + strGu;
                }
                else
                {
                    strHapGu = string.Empty;
                }


                if (!string.IsNullOrWhiteSpace(Trim(dt.Rows[0]["RoomCode"].ToString().Trim())))
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + "(" + strWard + "))";
                }
                else
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + ")";
                }

                string strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;

                string strRDRName = clsVbfunc.GetInSaName(pDbCon, Trim(dt.Rows[0]["Resultdrcode"].ToString().Trim()));


                #region 서브쿼리1
                SQL = " SELECT      REMARKC , REMARKX, REMARKP, REMARKD";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ENDO_REMARK";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + strPano + "'";
                SQL += ComNum.VBLF + "   AND JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND ORDERCODE = '" + Trim(dt.Rows[0]["ORDERCODE"].ToString()) + "'";


                string strChiefCom = string.Empty;
                string strClinicalDia = string.Empty;

                SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt2.Rows.Count > 0)
                {
                    strChiefCom = dt2.Rows[0]["remarkc"].ToString().Trim().Replace(ComNum.VBLF, "");
                    strClinicalDia = dt2.Rows[0]["remarkd"].ToString().Trim().Replace(ComNum.VBLF, "");
                }

                dt2.Dispose();
                #endregion

                //int nPrintLine = 0;




                string strInfo = "◈ Premedication ◈" + ComNum.VBLF;

                #region 데이터
                if (dt.Rows[0]["GBPRE_1"].ToString().Trim().Equals("Y"))
                {
                    strInfo = strInfo + "None ";

                }
                else
                {
                    //..strInfo = strInfo 

                }

                if (dt.Rows[0]["GBPRE_2"].ToString().Trim().Equals("Y"))
                {
                    strInfo = strInfo + "Aigiron ";
                }
                else
                {
                    //'strInfo = strInfo + "";
                }


                if (Trim(dt.Rows[0]["GBPRE_21"].ToString().Trim()) != "")
                {
                    strInfo = strInfo + dt.Rows[0]["GBPRE_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBPRE_22"].ToString().Trim() + ", ";

                }


                if (dt.Rows[0]["GBPRE_3"].ToString().Trim().Equals("Y"))

                {
                    strInfo += " " + dt.Rows[0]["GBPRE_31"].ToString().Trim();

                }
                else
                {
                    strInfo += dt.Rows[0]["GBPRE_31"].ToString().Trim();
                }

                strInfo += ComNum.VBLF;
                strInfo += ComNum.VBLF + "◈ Conscious Sedation ◈" + ComNum.VBLF;


                if (string.IsNullOrWhiteSpace(dt.Rows[0]["MOAAS"].ToString().Trim()) == false)
                {
                    strInfo += "MOAAS //Children`s Hospital of Wisconsin sedation Scale " + dt.Rows[0]["MOAAS"].ToString().Trim() + ", ";
                }

                if (dt.Rows[0]["GBCon_1"].ToString().Trim().Equals("Y"))
                {
                    strInfo += "None ";
                }

                if (dt.Rows[0]["GBCon_2"].ToString().Trim().Equals("Y"))
                {
                    strInfo += "Mediazolam ";
                }

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["GBCon_21"].ToString().Trim()) == false)
                {
                    strInfo += dt.Rows[0]["GBCon_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_22"].ToString().Trim() + ", ";
                }

                if (dt.Rows[0]["GBCon_3"].ToString().Trim().Equals("Y"))
                {
                    strInfo += "Propofol ";
                }

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["GBCon_31"].ToString().Trim()) == false)
                {
                    strInfo += dt.Rows[0]["GBCon_31"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_32"].ToString().Trim() + ", "; ;
                }

                if (dt.Rows[0]["GBCon_4"].ToString().Trim().Equals("Y"))
                {
                    strInfo += "Pethidine ";
                }


                if (string.IsNullOrWhiteSpace(dt.Rows[0]["GBCon_41"].ToString().Trim()) == false)
                {
                    strInfo += dt.Rows[0]["GBCon_41"].ToString().Trim() + "mg " + dt.Rows[0]["GBCon_42"].ToString().Trim() + ", "; ;
                }

                string strLowTime = string.Empty;
                if (string.IsNullOrWhiteSpace(dt.Rows[0]["D_INTIME1"].ToString().Trim()) == false)
                {
                    strLowTime = "내시경 삽입시간:" + dt.Rows[0]["D_INTIME1"].ToString().Trim() + "분" + dt.Rows[0]["D_INTIME2"].ToString().Trim() + "초";
                    strLowTime = strLowTime + "  회수시간:" + dt.Rows[0]["D_EXTIME1"].ToString().Trim() + "분" + dt.Rows[0]["D_EXTIME2"].ToString().Trim() + "초";
                }


                //'2015-07-23
                string strTPro = string.Empty;
                if (dt.Rows[0]["PRO_BX1"].ToString().Trim().Equals("Y")) strTPro += "Bx. bottle ";
                if (dt.Rows[0]["PRO_BX2"].ToString().Trim() != "") strTPro += dt.Rows[0]["PRO_BX2"].ToString().Trim() + "ea, ";


                if (dt.Rows[0]["PRO_PP1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "PP ";
                if (dt.Rows[0]["PRO_PP2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_PP2"].ToString().Trim() + "ea, ";


                if (dt.Rows[0]["PRO_RUT"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Rapid Urease Test, ";


                if (dt.Rows[0]["PRO_ESD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "ESD, ";
                if (dt.Rows[0]["PRO_ESD2"].ToString().Trim().Equals("Y")) strTPro = strTPro + "en-bloc, ";
                if (dt.Rows[0]["PRO_ESD3_1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "piecemeal ";
                if (dt.Rows[0]["PRO_ESD3_2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ESD3_2"].ToString().Trim() + ", " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_EMR1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EMR, ";
                if (dt.Rows[0]["PRO_EMR2"].ToString().Trim().Equals("Y")) strTPro = strTPro + "en-bloc, ";
                if (dt.Rows[0]["PRO_EMR3_1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "piecemeal ";
                if (dt.Rows[0]["PRO_EMR3_2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EMR3_2"].ToString().Trim() + ", " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_APC"].ToString().Trim().Equals("Y")) strTPro = strTPro + "APC, ";
                if (dt.Rows[0]["PRO_ELEC"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Electrocauterization, ";


                if (dt.Rows[0]["PRO_HEMO1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Hemoclip ";
                if (dt.Rows[0]["PRO_HEMO2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_HEMO2"].ToString().Trim() + "ea, " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_EPNA1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EPNA ";
                if (dt.Rows[0]["PRO_EPNA2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPNA2"].ToString().Trim() + "cc, ";



                if (dt.Rows[0]["PRO_MBAND"].ToString().Trim().Equals("Y")) strTPro = strTPro + "multi-band, " + ComNum.VBLF;



                if (dt.Rows[0]["PRO_EST"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EST (";
                if (dt.Rows[0]["PRO_EST_STS"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EST_STS"].ToString().Trim() + ") ";



                if (dt.Rows[0]["PRO_BAND1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Single-band ";
                if (dt.Rows[0]["PRO_BAND2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_BAND2"].ToString().Trim() + "ea, ";



                if (dt.Rows[0]["PRO_HIST1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Histoacyl ";
                if (dt.Rows[0]["PRO_HIST2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_HIST2"].ToString().Trim() + "ample, ";


                if (dt.Rows[0]["PRO_DETA"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Detachable snare, " + ComNum.VBLF;


                if (dt.Rows[0]["PRO_BALL"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Ballooon, ";
                if (dt.Rows[0]["PRO_BASKET"].ToString().Trim().Equals("Y")) strTPro = strTPro + "Basket, " + ComNum.VBLF;



                if (dt.Rows[0]["PRO_EPBD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "EPBD ";
                if (dt.Rows[0]["PRO_EPBD2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPBD2"].ToString().Trim() + "mm ";
                if (dt.Rows[0]["PRO_EPBD3"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPBD3"].ToString().Trim() + "atm ";
                if (dt.Rows[0]["PRO_EPBD4"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_EPBD4"].ToString().Trim() + "sec" + ComNum.VBLF;


                if (dt.Rows[0]["PRO_ENBD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "ENBD ";
                if (dt.Rows[0]["PRO_ENBD2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ENBD2"].ToString().Trim() + "Fr.";
                if (dt.Rows[0]["PRO_ENBD3"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ENBD3"].ToString().Trim() + "type ";


                if (dt.Rows[0]["PRO_ERBD1"].ToString().Trim().Equals("Y")) strTPro = strTPro + "ERBD ";
                if (dt.Rows[0]["PRO_ERBD2"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ERBD2"].ToString().Trim() + "Fr.";
                if (dt.Rows[0]["PRO_ERBD3"].ToString().Trim() != "") strTPro = strTPro + dt.Rows[0]["PRO_ERBD3"].ToString().Trim() + "type ";

                strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;

                string strResult6_new = dt.Rows[0]["Remark6"].ToString().Trim();

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["Remark6"].ToString().Trim()) == false)
                {
                    strResult6_new = "small Intestinal:" + dt.Rows[0]["Remark6"].ToString().Trim() + ComNum.VBLF;
                }

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["Remark6_2"].ToString().Trim()) == false)
                {
                    strResult6_new += ComNum.VBLF + "large Intestinal:" + dt.Rows[0]["Remark6_2"].ToString().Trim();
                }

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["Remark6_3"].ToString().Trim()) == false)
                {
                    strResult6_new += ComNum.VBLF + "rectum:" + dt.Rows[0]["Remark6_3"].ToString().Trim();
                }


                WriteStr(22, 50, 170, strTitle);       // 'titel

                WriteStr(25, 150, 280, strChiefCom);//          'Chief complaint
                WriteStr(25, 650, 380, strClinicalDia);//      'clinical Diagnosis

                string strTResult = "◈ ERCP Finding ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Disgnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Paln & Tx ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;

                strTResult += strInfo + ComNum.VBLF + ComNum.VBLF;//  'add

                if (strTPro != "")
                {
                    strTResult = strTResult + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + strTPro + ComNum.VBLF + dt.Rows[0]["Remark4"].ToString().Trim() + ComNum.VBLF + (string.IsNullOrEmpty(strHapGu) == false ? strHapGu + ComNum.VBLF : "") + ComNum.VBLF;
                }
                else
                {
                    strTResult = strTResult + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark4"].ToString().Trim() + ComNum.VBLF + (string.IsNullOrEmpty(strHapGu) == false ? strHapGu + ComNum.VBLF : "") + ComNum.VBLF;
                }

                strTResult = strTResult + "◈ Endoscopic Biopsy ◈" + ComNum.VBLF + ComNum.VBLF + strResult6_new + ComNum.VBLF + ComNum.VBLF;

                #endregion

                strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                string[] strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 470;


                #region Text Print
                for (int k = 0; k < strResult.Length; k++)
                {
                    if (string.IsNullOrWhiteSpace(strResult[k]))
                    {
                        lngLine += 1;
                    }
                    else
                    {

                        #region 신규
                        int lastPos = 0;
                        using(Font font = new Font(FontName, 20))
                        {
                            Size PageSize = new Size(1648 - 150, 20);
                            Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                            if (strWidth.Width < 1100)
                            {
                                WriteStr(20, 150, nTop + (lngLine * 35), strResult[k]);
                                lngLine += 1;
                            }
                            else
                            {
                                for (int l = 0; l < strResult[k].Length + 1;l++)
                                {
                                    bool DataOutPut = false;
                                    string strText = strResult[k].Substring(lastPos, l);
                                    strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                    if (strWidth.Width >= 1100)
                                    {
                                        lastPos += l;
                                        l = 0;
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        DataOutPut = true;
                                    }

                                    if (DataOutPut && lastPos + l == strResult[k].Length)
                                    {
                                        break;
                                    }

                                    if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                    {
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        break;
                                    }
                                }
                            }
                        }
                        #endregion


                        //int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                        //if (strByte >= 325)
                        //{
                        //    WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Left(strResult[k], 65));
                        //    lngLine += 1;

                        //    if (strResult[k].Length - 65 > 65)
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));
                        //        lngLine += 1;
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 130));

                        //    }
                        //    else
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));

                        //    }
                        //}
                        //else
                        //{
                        //    WriteStr(22, nLeft, nTop + (lngLine * 35), strResult[k]);
                        //}

                        //lngLine += 1;
                    }

                    #region NEW PAGE
                    if (nTop + (lngLine * 35) > 2080)
                    {
                        #region 1장 에도 검사결과 등등 정보ㅓㅓ 들어가개ㅔ 2020-03-23
                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);
                        WriteStr(25, 1260, 2230, strRDRName);
                        #endregion
                        WriteStr(25, 750, 2100, "(계속)");

                        TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                        //LNGY = 40;
                        lngLine = 1;
                        PageNum += 1;

                        New_initFormEndo_4New("내 시 경 판 독 결 과 지");

                        WriteStr(22, 50, 170, strTitle);//             'titel
                        WriteStr(25, 150, 280, strChiefCom);//         'Chief complaint
                        WriteStr(25, 650, 380, strClinicalDia);//      'clinical Diagnosis
                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);
                        WriteStr(25, 1260, 2230, strRDRName);
                    }
                    #endregion

                }
                #endregion

                WriteStr(30, 300, 2140, strRDate);
                WriteStr(30, 300, 2180, strResultDate);
                WriteStr(30, 950, 2160, strRDRName);
                WriteStr(25, 1260, 2230, strRDRName);

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "내시경 에러");
                //ComFunc.MsgBox(ex.Message);
            }
        }

        public static void New_initFormEndo_4New(string str)
        {
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(43, 540, 80, str);

            hline2(50, 150, 1600);
            hline2(50, 220, 1600);
            hline2(50, 430, 1600);

            WriteStr(30, 100, 230, "Chief Complaint");
            WriteStr(30, 100, 320, "Clinical Diagnosis");
            WriteStr(30, 80, 450, "ERCP Findings.");


            WriteStr(23, 80, 2140, "검사 시행일 :");
            WriteStr(23, 80, 2180, "결과 보고일 :");


            WriteStr(23, 700, 2160, "Reported by :");

            hline2(50, 2220, 1600);

            WriteStr(23, 80, 2230, "포항성모병원");
            WriteStr(23, 1100, 2230, "처치의사:");


            WriteStr(25, 70, 2280, "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.");
        }


        /// <summary>
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <param name="ArgGBJob">(1:기관지, 2:위, 3:장, 4:ERCP)</param>
        public static void New_Exam_Endo_Result_1NEW(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string ArgRowid, string ArgGBJob, string strTREATNO)
        {
            if (string.IsNullOrWhiteSpace(ArgRowid))
                return;

            #region 변수
            //int LNGY = 40;
            int nTop = 580;
            //int nLeft = 150;
            int lngLine = 0;

            string strTSName = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            DataTable dt2 = null;

            #endregion


            try
            {
                #region 쿼리

                SQL = "         SELECT  A.PTNO, A.SEX , (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PTNO) SNAME, A.GBIO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SEQNUM, A.ORDERCODE, ";


                //'추가정보 추가
                //'PreMEDICATION Pethidine -----------------------------------------------


                SQL += ComNum.VBLF + "          GBPRE_1,";//   ' PreMEDICATION 여부


                SQL += ComNum.VBLF + "          GBCON_4,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBCON_41,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBCON_42,";//  ' 단위


                // 'PreMEDICATION Atropin
                SQL += ComNum.VBLF + "          GBPRE_4,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBPRE_41,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBPRE_42,";//  ' 단위
                SQL += ComNum.VBLF + "          GBPRE_3, ";//  ' OTHER 여부
                SQL += ComNum.VBLF + "          GBPRE_31, ";// ' OTHER



                // 'Conscious sedation  midasolam -----------------------------------------------------
                SQL += ComNum.VBLF + "          GBCON_1,";//  ' Conscious 여부


                SQL += ComNum.VBLF + "          GBCON_2,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBCON_21,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBCON_22,";//  ' 단위



                //'Conscious sedation Propofol
                SQL += ComNum.VBLF + "          GBCON_3,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBCON_31,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBCON_32,";//  ' 단위


                SQL += ComNum.VBLF + "          GBCON_5, ";// ' OTHER  여부
                SQL += ComNum.VBLF + "          GBCON_51, ";// ' OTHER


                //'medication Epinephrine --------------------------------------------------
                SQL += ComNum.VBLF + "          GBMED_1,";//   ' medication 여부


                SQL += ComNum.VBLF + "          GBMED_2,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBMED_21,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBMED_22,";//  ' 단위


                SQL += ComNum.VBLF + "          GBMED_3,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBMED_31,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBMED_32,";//  ' 단위


                SQL += ComNum.VBLF + "          GBMED_4, ";// ' OTHER  여부
                SQL += ComNum.VBLF + "          GBMED_41, ";// ' OTHER


                SQL += ComNum.VBLF + "          TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL += ComNum.VBLF + "          A.RESULTDRCODE, TO_CHAR(A.BirthDate,'YYYY-MM-DD') BIRTHDate, ";
                SQL += ComNum.VBLF + "          B.REMARK1, B.REMARK2, B.REMARK3, B.REMARK4, B.REMARK5, B.REMARK6,";
                SQL += ComNum.VBLF + "          C.REMARKC , C.REMARKX, C.REMARKP, C.REMARKD";
                SQL += ComNum.VBLF + "     FROM KOSMOS_OCS.ENDO_JUPMST A, KOSMOS_OCS.ENDO_RESULT B , KOSMOS_OCS.ENDO_REMARK C";
                SQL += ComNum.VBLF + "     WHERE A.SEQNO = '" + ArgRowid + "' ";
                SQL += ComNum.VBLF + "       AND A.RESULTDATE IS NOT NULL";
                SQL += ComNum.VBLF + "       AND A.SEQNO = B.SEQNO";
                SQL += ComNum.VBLF + "       AND A.PTNO = C.PTNO(+)";
                SQL += ComNum.VBLF + "       AND A.JDATE = C.JDATE";
                SQL += ComNum.VBLF + "       AND A.ORDERCODE = C.ORDERCODE";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                New_initFormEndo_1NEW("내 시 경 판 독 결 과 지");

                string strPano = (dt.Rows[0]["PTNO"].ToString().Trim());
                string strSName = (dt.Rows[0]["sName"].ToString().Trim());
                string strGBIO = (dt.Rows[0]["GBio"].ToString().Trim()).Equals("I") ? "입원" : "외래";


                //'strAge = AGE_YEAR_Gesan2(Trim(dt.Rows[0]["BIRTHdate"].ToString().Trim()), Trim(dt.Rows[0]["JDATE"].ToString().Trim()))
                string strAge = AGE_YEAR_Birth(dt.Rows[0]["BIRTHdate"].ToString().Trim(), Trim(dt.Rows[0]["JDATE"].ToString().Trim())).ToString();
                string strSex = (dt.Rows[0]["Sex"].ToString().Trim());


                string strWard = (dt.Rows[0]["WardCode"].ToString().Trim());
                string strRoomCode = (dt.Rows[0]["roomcode"].ToString().Trim());
                string strDeptName = clsVbfunc.GetBASClinicDeptNameK(pDbCon, Trim(dt.Rows[0]["DeptCode"].ToString().Trim()));

                string strDrName = clsVbfunc.GetBASDoctorName(pDbCon, Trim(dt.Rows[0]["DrCode"].ToString().Trim()));
                string strJDate = (dt.Rows[0]["JDATE"].ToString().Trim());
                string strRDate = (dt.Rows[0]["Rdate"].ToString().Trim());
                string strResultDate = (dt.Rows[0]["RESULTDATE"].ToString().Trim());


                string strSeqNUM = (dt.Rows[0]["seqnum"].ToString().Trim());
                //'strSDate = Trim(RsVerify!SDate"].ToString().Trim())
                //'strEDate = Trim(RsVerify!EDate"].ToString().Trim())


                if (!string.IsNullOrWhiteSpace(Trim(dt.Rows[0]["RoomCode"].ToString().Trim())))
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + "(" + strWard + "))";
                }
                else
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + ")";
                }

                string strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;

                string strRDRName = clsVbfunc.GetInSaName(pDbCon, Trim(dt.Rows[0]["Resultdrcode"].ToString().Trim()));


                #region 서브쿼리1
                SQL = " SELECT      REMARKC , REMARKX, REMARKP, REMARKD";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ENDO_REMARK";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + strPano + "'";
                SQL += ComNum.VBLF + "   AND JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND ORDERCODE = '" + Trim(dt.Rows[0]["ORDERCODE"].ToString()) + "'";


                string strChiefCom = string.Empty;
                string strClinicalDia = string.Empty;

                SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt2.Rows.Count > 0)
                {
                    strChiefCom = dt2.Rows[0]["remarkc"].ToString().Trim().Replace(ComNum.VBLF, "");
                    strClinicalDia = dt2.Rows[0]["remarkd"].ToString().Trim().Replace(ComNum.VBLF, "");
                }

                dt2.Dispose();
                #endregion

                //int nPrintLine = 0;


                WriteStr(22, 50, 170, strTitle);       // 'titel

                WriteStr(25, 650, 230, strChiefCom);//          'Chief complaint
                WriteStr(25, 650, 320, strClinicalDia);//      'clinical Diagnosis


                string strInfo = "◈ Premedication ◈" + ComNum.VBLF;
                strInfo += dt.Rows[0]["GBPRE_1"].ToString().Trim().Equals("Y") ? "None" + ComNum.VBLF : "";
                strInfo += dt.Rows[0]["GBCON_4"].ToString().Trim().Equals("Y") ? "Petihdine " + dt.Rows[0]["GBcon_41"].ToString().Trim() + "mg " + dt.Rows[0]["GBcon_42"].ToString().Trim() + ComNum.VBLF : "";
                strInfo += dt.Rows[0]["GBPRE_4"].ToString().Trim().Equals("Y") ? "Atropine "  + dt.Rows[0]["gbpre_41"].ToString().Trim() + "mg " + dt.Rows[0]["gbpre_42"].ToString().Trim() + ComNum.VBLF : "";
                strInfo += dt.Rows[0]["gbpre_3"].ToString().Trim().Equals("Y") ? " " + dt.Rows[0]["gbpre_31 "].ToString().Trim() + ComNum.VBLF : "";
                strInfo += ComNum.VBLF;
                strInfo += "◈ Conscious Sedation ◈" + ComNum.VBLF;
                strInfo += dt.Rows[0]["GBcon_1"].ToString().Trim().Equals("Y") ? "None" + ComNum.VBLF : "";
                strInfo += dt.Rows[0]["gbcon_2"].ToString().Trim().Equals("Y") ? "Midazolam " + dt.Rows[0]["gbCON_21"].ToString().Trim() + "mg " + dt.Rows[0]["gbCON_22"].ToString().Trim() + ComNum.VBLF : "";
                strInfo += dt.Rows[0]["gbCON_3"].ToString().Trim().Equals("Y") ? "Propofol " + dt.Rows[0]["gbCON_31"].ToString().Trim() + "mg " + dt.Rows[0]["gbCON_32"].ToString().Trim()  + ComNum.VBLF : "";
                strInfo += dt.Rows[0]["gbcon_5"].ToString().Trim().Equals("Y") ? " " + dt.Rows[0]["gbCON_51"].ToString().Trim() + ComNum.VBLF : "";
                strInfo += ComNum.VBLF;
                strInfo += "◈ Medication ◈" + ComNum.VBLF;
                strInfo += dt.Rows[0]["GBmed_1"].ToString().Trim().Equals("Y") ? "None" + ComNum.VBLF : "";
                strInfo += dt.Rows[0]["gbmed_2"].ToString().Trim().Equals("Y") ? "Epinephrine " + dt.Rows[0]["gbmed_21"].ToString().Trim() + "mg " + dt.Rows[0]["gbmed_22"].ToString().Trim() + ComNum.VBLF : "";
                strInfo += dt.Rows[0]["gbmed_3"].ToString().Trim().Equals("Y") ? "Botrooase " + dt.Rows[0]["gbmed_31"].ToString().Trim() + "mg " + dt.Rows[0]["gbmed_32"].ToString().Trim() + ComNum.VBLF : "";
                strInfo += dt.Rows[0]["gbmed_4"].ToString().Trim().Equals("Y") ? " " + dt.Rows[0]["gbmed_41"].ToString().Trim() + ComNum.VBLF : "";
                strInfo += ComNum.VBLF;


                string strTResult = "◈ Vocal Cord ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Carina ◈ " + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Bronchi ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark4"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Endoscopic Biopsy ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark6"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + strInfo + ComNum.VBLF + ComNum.VBLF;//  'add

                strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                string[] strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 530;


                #region Text Print
                for (int k = 0; k < strResult.Length; k++)
                {
                    if (string.IsNullOrWhiteSpace(strResult[k]))
                    {
                        lngLine += 1;
                    }
                    else
                    {
                        #region 신규
                        int lastPos = 0;
                        using (Font font = new Font(FontName, 20))
                        {
                            Size PageSize = new Size(1648 - 150, 20);
                            Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                            if (strWidth.Width < 1100)
                            {
                                WriteStr(20, 150, nTop + (lngLine * 35), strResult[k]);
                                lngLine += 1;
                            }
                            else
                            {
                                for (int l = 0; l < strResult[k].Length + 1;l++)
                                {
                                    bool DataOutPut = false;
                                    string strText = strResult[k].Substring(lastPos, l);
                                    strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                    if (strWidth.Width >= 1100)
                                    {
                                        lastPos += l;
                                        l = 0;
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        DataOutPut = true;
                                    }

                                    if (DataOutPut && lastPos + l == strResult[k].Length)
                                    {
                                        break;
                                    }

                                    if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                    {
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        break;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 기존
                        //int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                        //if (strByte >= 325)
                        //{
                        //    WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Left(strResult[k], 65));
                        //    lngLine += 1;

                        //    if (strResult[k].Length - 65 > 65)
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));
                        //        lngLine += 1;
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 130));

                        //    }
                        //    else
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));

                        //    }
                        //}
                        //else
                        //{
                        //    WriteStr(22, nLeft, nTop + (lngLine * 35), strResult[k]);
                        //}

                        //lngLine += 1;
                        #endregion
                    }

                    #region NEW PAGE
                    if (nTop + (lngLine * 35) > 2080)
                    {
                        #region 1장 에도 검사결과 등등 정보ㅓㅓ 들어가개ㅔ 2020-03-23
                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);
                        WriteStr(25, 1260, 2230, strRDRName);
                        #endregion

                        WriteStr(25, 750, 2100, "(계속)");

                        TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                        //LNGY = 40;
                        lngLine = 1;
                        PageNum += 1;

                        New_initFormEndo_1("내 시 경 판 독 결 과 지");

                        WriteStr(22, 50, 170, strTitle);//             'titel
                        WriteStr(25, 150, 280, strChiefCom);//         'Chief complaint
                        WriteStr(25, 650, 380, strClinicalDia);//      'clinical Diagnosis
                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);
                        WriteStr(25, 1260, 2230, strRDRName);
                    }
                    #endregion

                }
                #endregion

                WriteStr(30, 300, 2140, strRDate);
                WriteStr(30, 300, 2180, strResultDate);
                WriteStr(30, 950, 2160, strRDRName);
                WriteStr(25, 1260, 2230, strRDRName);

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "내시경 에러");
                //ComFunc.MsgBox(ex.Message);
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <param name="ArgGBJob">(1:기관지, 2:위, 3:장, 4:ERCP)</param>
        public static void New_Exam_Endo_Result_2NEW(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string ArgRowid, string ArgGBJob, string strTREATNO)
        {
            if (string.IsNullOrWhiteSpace(ArgRowid))
                return;

            #region 변수
            //int LNGY = 40;
            int nTop = 580;
            //int nLeft = 150;
            int lngLine = 0;

            string strTSName = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            DataTable dt2 = null;

            #endregion


            try
            {
                #region 쿼리

                SQL = "         SELECT  A.PTNO, A.SEX , (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PTNO) SNAME, A.GBIO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SEQNUM,A.ORDERCODE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL += ComNum.VBLF + "          A.RESULTDRCODE, TO_CHAR(A.BirthDate,'YYYY-MM-DD') BIRTHDate, ";
                SQL += ComNum.VBLF + "          B.REMARK1, B.REMARK2, B.REMARK3, B.REMARK4, B.REMARK5, B.REMARK6, ";


                SQL += ComNum.VBLF + " A.GBPRE_1,A.GBPRE_2,A.GBPRE_21,A.GBPRE_22,A.GBPRE_3,A.GBCON_1,A.GBCON_2,A.GBCON_21,";
                SQL += ComNum.VBLF + " A.GBCON_22,A.GBCON_3,A.GBCON_31,A.GBCON_32,A.GBCON_4,A.GBCON_41,A.GBCON_42,A.GBPRO_1,A.GBPRO_2,A.GBPRE_31, ";
                SQL += ComNum.VBLF + " B.Remark6_2, B.Remark6_3, B.Remark  ";


                SQL += ComNum.VBLF + "     FROM KOSMOS_OCS.ENDO_JUPMST A, KOSMOS_OCS.ENDO_RESULT B ";
                SQL += ComNum.VBLF + "     WHERE A.SEQNO = '" + ArgRowid + "' ";
                SQL += ComNum.VBLF + "       AND A.RESULTDATE IS NOT NULL";
                SQL += ComNum.VBLF + "       AND A.SEQNO = B.SEQNO";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                New_initFormEndo_2New("내 시 경 판 독 결 과 지");


                string strPano = (dt.Rows[0]["PTNO"].ToString().Trim());
                string strSName = (dt.Rows[0]["sName"].ToString().Trim());
                string strGBIO = (dt.Rows[0]["GBio"].ToString().Trim()).Equals("I") ? "입원" : "외래";


                //'strAge = AGE_YEAR_Gesan2(Trim(dt.Rows[0]["BIRTHdate"].ToString().Trim()), Trim(dt.Rows[0]["JDATE"].ToString().Trim()))
                string strAge = AGE_YEAR_Birth(dt.Rows[0]["BIRTHdate"].ToString().Trim(), Trim(dt.Rows[0]["JDATE"].ToString().Trim())).ToString();
                string strSex = (dt.Rows[0]["Sex"].ToString().Trim());


                string strWard = (dt.Rows[0]["WardCode"].ToString().Trim());
                string strRoomCode = (dt.Rows[0]["roomcode"].ToString().Trim());
                string strDeptName = clsVbfunc.GetBASClinicDeptNameK(pDbCon, Trim(dt.Rows[0]["DeptCode"].ToString().Trim()));

                string strDrName = clsVbfunc.GetBASDoctorName(pDbCon, Trim(dt.Rows[0]["DrCode"].ToString().Trim()));
                string strJDate = (dt.Rows[0]["JDATE"].ToString().Trim());
                string strRDate = (dt.Rows[0]["Rdate"].ToString().Trim());
                string strResultDate = (dt.Rows[0]["RESULTDATE"].ToString().Trim());


                string strSeqNUM = (dt.Rows[0]["seqnum"].ToString().Trim());
                //'strSDate = Trim(RsVerify!SDate"].ToString().Trim())
                //'strEDate = Trim(RsVerify!EDate"].ToString().Trim())


                if (!string.IsNullOrWhiteSpace(Trim(dt.Rows[0]["RoomCode"].ToString().Trim())))
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + "(" + strWard + "))";
                }
                else
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + ")";
                }

                #region 2013 - 01 - 15 new add ------------------------------------------------------ -
                string strInfo = "◈ Premedication ◈" + ComNum.VBLF;
                strInfo += dt.Rows[0]["GBPRE_1"].ToString().Trim().Equals("Y") ? "None" : "";
                strInfo += dt.Rows[0]["GBPRE_2"].ToString().Trim().Equals("Y") ? "Aigiron " : "";
                strInfo += dt.Rows[0]["GBPRE_21"].ToString().Trim() != "" ? dt.Rows[0]["GBPRE_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBPRE_22"].ToString().Trim() + ", " : "";
                strInfo += dt.Rows[0]["gbpre_3"].ToString().Trim().Equals("Y") ? " " + dt.Rows[0]["gbpre_31"].ToString().Trim() : dt.Rows[0]["gbpre_31"].ToString().Trim();
                strInfo += ComNum.VBLF + ComNum.VBLF;
                strInfo += "◈ Conscious Sedation ◈" + ComNum.VBLF;
                strInfo += dt.Rows[0]["GBcon_1"].ToString().Trim().Equals("Y") ? "None " : "";
                strInfo += dt.Rows[0]["gbcon_2"].ToString().Trim().Equals("Y") ? "Mediazolam " : "";
                strInfo += dt.Rows[0]["gbCON_21"].ToString().Trim() != "" ? dt.Rows[0]["gbCON_21"].ToString().Trim() + "mg " + dt.Rows[0]["gbCON_22"].ToString().Trim() + ", " : "";
                strInfo += dt.Rows[0]["gbCON_3"].ToString().Trim().Equals("Y") ? "Propotol " : "";
                strInfo += dt.Rows[0]["gbCON_31"].ToString().Trim() != "" ? dt.Rows[0]["gbCON_31"].ToString().Trim() + "mg " + dt.Rows[0]["gbCON_32"].ToString().Trim() + ", " : "";
                strInfo += dt.Rows[0]["GBCON_4"].ToString().Trim().Equals("Y") ? "Pethidine " : "";
                strInfo += dt.Rows[0]["GBcon_41"].ToString().Trim() != "" ? dt.Rows[0]["GBcon_41"].ToString().Trim() + "mg " + dt.Rows[0]["GBcon_42"].ToString().Trim() + ", " :"";
                strInfo += ComNum.VBLF;
                #endregion

                string strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;

                string strRDRName = clsVbfunc.GetInSaName(pDbCon, Trim(dt.Rows[0]["Resultdrcode"].ToString().Trim()));


                #region 서브쿼리1
                SQL = " SELECT      REMARKC , REMARKX, REMARKP, REMARKD";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ENDO_REMARK";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + strPano + "'";
                SQL += ComNum.VBLF + "   AND JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND ORDERCODE = '" + Trim(dt.Rows[0]["ORDERCODE"].ToString()) + "'";


                string strChiefCom = string.Empty;
                string strClinicalDia = string.Empty;

                SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt2.Rows.Count > 0)
                {
                    strChiefCom = dt2.Rows[0]["remarkc"].ToString().Trim().Replace(ComNum.VBLF, "");
                    strClinicalDia = dt2.Rows[0]["remarkd"].ToString().Trim().Replace(ComNum.VBLF, "");
                }

                dt2.Dispose();
                #endregion

                //int nPrintLine = 0;


                WriteStr(22, 50, 170, strTitle);       // 'titel

                WriteStr(25, 150, 280, strChiefCom);//          'Chief complaint
                WriteStr(25, 650, 380, strClinicalDia);//      'clinical Diagnosis


                string strTResult = "◈ Esophagus ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Stomach ◈ " + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Duodenum ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Endoscopic Diagnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark4"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + strInfo + ComNum.VBLF + ComNum.VBLF;//  'add

                if (dt.Rows[0]["GBPRO_2"].ToString().Trim().Equals("Y"))
                {
                    strTResult += "◈ Endoscopic Procedure ◈" + ComNum.VBLF + "CLO" + ComNum.VBLF + dt.Rows[0]["Remark5"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                }
                else
                {
                    strTResult += "◈ Endoscopic Procedure ◈" + ComNum.VBLF + dt.Rows[0]["Remark5"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                }

                strTResult += "◈ Endoscopic Biopsy ◈" + ComNum.VBLF;
                strTResult += dt.Rows[0]["Remark6"].ToString().Trim() != "" ? "Esophagus:" + dt.Rows[0]["Remark6"].ToString().Trim() + ComNum.VBLF : "";
                strTResult += dt.Rows[0]["Remark6_2"].ToString().Trim() != "" ? "Stomach:" + dt.Rows[0]["Remark6_2"].ToString().Trim() + ComNum.VBLF : "";
                strTResult += dt.Rows[0]["Remark6_3"].ToString().Trim() != "" ? "Duodenum:" + dt.Rows[0]["Remark6_3"].ToString().Trim() + ComNum.VBLF : "";
                strTResult += ComNum.VBLF;


                //참고사항
                strTResult += "◈ Remark ◈" + dt.Rows[0]["Remark"].ToString().Trim();


                strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                string[] strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 530;


                #region Text Print
                for (int k = 0; k < strResult.Length; k++)
                {
                    if (string.IsNullOrWhiteSpace(strResult[k]))
                    {
                        lngLine += 1;
                    }
                    else
                    {
                        #region 신규
                        int lastPos = 0;
                        using (Font font = new Font(FontName, 20))
                        {
                            Size PageSize = new Size(1648 - 150, 20);
                            Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                            if (strWidth.Width < 1100)
                            {
                                WriteStr(20, 150, nTop + (lngLine * 35), strResult[k]);
                                lngLine += 1;
                            }
                            else
                            {
                                for (int l = 0; l < strResult[k].Length + 1;l++)
                                {
                                    bool DataOutPut = false;
                                    string strText = strResult[k].Substring(lastPos, l);
                                    strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                    if (strWidth.Width >= 1100)
                                    {
                                        lastPos += l;
                                        l = 0;
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        DataOutPut = true;
                                    }

                                    if (DataOutPut && lastPos + l == strResult[k].Length)
                                    {
                                        break;
                                    }

                                    if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                    {
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        break;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 기존
                        //int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                        //if (strByte >= 325)
                        //{
                        //    WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Left(strResult[k], 65));
                        //    lngLine += 1;

                        //    if (strResult[k].Length - 65 > 65)
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));
                        //        lngLine += 1;
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 130));

                        //    }
                        //    else
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));

                        //    }
                        //}
                        //else
                        //{
                        //    WriteStr(22, nLeft, nTop + (lngLine * 35), strResult[k]);
                        //}

                        //lngLine += 1;
                        #endregion
                    }

                    #region NEW PAGE
                    if (nTop + (lngLine * 35) > 2080)
                    {
                        #region 1장 에도 검사결과 등등 정보ㅓㅓ 들어가개ㅔ 2020-03-23
                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);
                        WriteStr(25, 1260, 2230, strRDRName);
                        #endregion
                        WriteStr(25, 750, 2100, "(계속)");

                        TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                        //LNGY = 40;
                        lngLine = 1;
                        PageNum += 1;

                        New_initFormEndo_2New("내 시 경 판 독 결 과 지");

                        WriteStr(22, 50, 170, strTitle);//             'titel
                        WriteStr(25, 150, 280, strChiefCom);//         'Chief complaint
                        WriteStr(25, 650, 380, strClinicalDia);//      'clinical Diagnosis
                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);
                        WriteStr(25, 1260, 2230, strRDRName);
                    }
                    #endregion

                }
                #endregion

                WriteStr(30, 300, 2140, strRDate);
                WriteStr(30, 300, 2180, strResultDate);
                WriteStr(30, 950, 2160, strRDRName);
                WriteStr(25, 1260, 2230, strRDRName);

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "내시경 에러");
                //ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <param name="ArgGBJob">(1:기관지, 2:위, 3:장, 4:ERCP)</param>
        public static void New_Exam_Endo_Result_3NEW(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string ArgRowid, string ArgGBJob, string strTREATNO)
        {
            if (string.IsNullOrWhiteSpace(ArgRowid))
                return;

            #region 변수
            //int LNGY = 40;
            int nTop = 580;
            //int nLeft = 150;
            int lngLine = 0;

            string strTSName = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            DataTable dt2 = null;

            #endregion


            try
            {
                #region 쿼리

                SQL = "         SELECT  A.PTNO, A.SEX , (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PTNO) SNAME, A.GBIO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SEQNUM,A.ORDERCODE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL += ComNum.VBLF + "          A.RESULTDRCODE, TO_CHAR(A.BirthDate,'YYYY-MM-DD') BIRTHDate, ";
                SQL += ComNum.VBLF + "          B.REMARK1, B.REMARK2, B.REMARK3, B.REMARK4, B.REMARK5, B.REMARK6, ";


                SQL += ComNum.VBLF + " A.GBPRE_1,A.GBPRE_2,A.GBPRE_21,A.GBPRE_22,A.GBPRE_3,A.GBCON_1,A.GBCON_2,A.GBCON_21,";
                SQL += ComNum.VBLF + " A.GBCON_22,A.GBCON_3,A.GBCON_31,A.GBCON_32,A.GBCON_4,A.GBCON_41,A.GBCON_42,A.GBPRO_1,A.GBPRO_2,A.GBPRE_31, ";
                SQL += ComNum.VBLF + " B.Remark6_2, B.Remark6_3, B.Remark  ";


                SQL += ComNum.VBLF + "     FROM KOSMOS_OCS.ENDO_JUPMST A, KOSMOS_OCS.ENDO_RESULT B ";
                SQL += ComNum.VBLF + "     WHERE A.SEQNO = '" + ArgRowid + "' ";
                SQL += ComNum.VBLF + "       AND A.RESULTDATE IS NOT NULL";
                SQL += ComNum.VBLF + "       AND A.SEQNO = B.SEQNO";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                New_initFormEndo_3New("내 시 경 판 독 결 과 지");

                string strPano = (dt.Rows[0]["PTNO"].ToString().Trim());
                string strSName = (dt.Rows[0]["sName"].ToString().Trim());
                string strGBIO = (dt.Rows[0]["GBio"].ToString().Trim()).Equals("I") ? "입원" : "외래";


                //'strAge = AGE_YEAR_Gesan2(Trim(dt.Rows[0]["BIRTHdate"].ToString().Trim()), Trim(dt.Rows[0]["JDATE"].ToString().Trim()))
                string strAge = AGE_YEAR_Birth(dt.Rows[0]["BIRTHdate"].ToString().Trim(), Trim(dt.Rows[0]["JDATE"].ToString().Trim())).ToString();
                string strSex = (dt.Rows[0]["Sex"].ToString().Trim());


                string strWard = (dt.Rows[0]["WardCode"].ToString().Trim());
                string strRoomCode = (dt.Rows[0]["roomcode"].ToString().Trim());
                string strDeptName = clsVbfunc.GetBASClinicDeptNameK(pDbCon, Trim(dt.Rows[0]["DeptCode"].ToString().Trim()));

                string strDrName = clsVbfunc.GetBASDoctorName(pDbCon, Trim(dt.Rows[0]["DrCode"].ToString().Trim()));
                string strJDate = (dt.Rows[0]["JDATE"].ToString().Trim());
                string strRDate = (dt.Rows[0]["Rdate"].ToString().Trim());
                string strResultDate = (dt.Rows[0]["RESULTDATE"].ToString().Trim());


                string strSeqNUM = (dt.Rows[0]["seqnum"].ToString().Trim());
                //'strSDate = Trim(RsVerify!SDate"].ToString().Trim())
                //'strEDate = Trim(RsVerify!EDate"].ToString().Trim())


                if (!string.IsNullOrWhiteSpace(Trim(dt.Rows[0]["RoomCode"].ToString().Trim())))
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + "(" + strWard + "))";
                }
                else
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + ")";
                }

                #region 2013 - 01 - 15 new add ------------------------------------------------------ -
                string strInfo = "◈ Premedication ◈" + ComNum.VBLF;
                strInfo += dt.Rows[0]["GBPRE_1"].ToString().Trim().Equals("Y") ? "None" : "";
                strInfo += dt.Rows[0]["GBPRE_2"].ToString().Trim().Equals("Y") ? "Aigiron " : "";
                strInfo += dt.Rows[0]["GBPRE_21"].ToString().Trim() != "" ? dt.Rows[0]["GBPRE_21"].ToString().Trim() + "mg " + dt.Rows[0]["GBPRE_22"].ToString().Trim() + ", " : "";
                strInfo += dt.Rows[0]["gbpre_3"].ToString().Trim().Equals("Y") ? " " + dt.Rows[0]["gbpre_31"].ToString().Trim() : dt.Rows[0]["gbpre_31"].ToString().Trim();
                strInfo += ComNum.VBLF + ComNum.VBLF;
                strInfo += "◈ Conscious Sedation ◈" + ComNum.VBLF;
                strInfo += dt.Rows[0]["GBcon_1"].ToString().Trim().Equals("Y") ? "None " : "";
                strInfo += dt.Rows[0]["gbcon_2"].ToString().Trim().Equals("Y") ? "Mediazolam " : "";
                strInfo += dt.Rows[0]["gbCON_21"].ToString().Trim() != "" ? dt.Rows[0]["gbCON_21"].ToString().Trim() + "mg " + dt.Rows[0]["gbCON_22"].ToString().Trim() + ", " : "";
                strInfo += dt.Rows[0]["gbCON_3"].ToString().Trim().Equals("Y") ? "Propotol " : "";
                strInfo += dt.Rows[0]["gbCON_31"].ToString().Trim() != "" ? dt.Rows[0]["gbCON_31"].ToString().Trim() + "mg " + dt.Rows[0]["gbCON_32"].ToString().Trim() + ", " : "";
                strInfo += dt.Rows[0]["GBCON_4"].ToString().Trim().Equals("Y") ? "Pethidine " : "";
                strInfo += dt.Rows[0]["GBcon_41"].ToString().Trim() != "" ? dt.Rows[0]["GBcon_41"].ToString().Trim() + "mg " + dt.Rows[0]["GBcon_42"].ToString().Trim() + ", " : "";
                strInfo += ComNum.VBLF;
                #endregion

                string strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;

                string strRDRName = clsVbfunc.GetInSaName(pDbCon, Trim(dt.Rows[0]["Resultdrcode"].ToString().Trim()));


                #region 서브쿼리1
                SQL = " SELECT      REMARKC , REMARKX, REMARKP, REMARKD";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ENDO_REMARK";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + strPano + "'";
                SQL += ComNum.VBLF + "   AND JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND ORDERCODE = '" + Trim(dt.Rows[0]["ORDERCODE"].ToString()) + "'";


                string strChiefCom = string.Empty;
                string strClinicalDia = string.Empty;

                SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt2.Rows.Count > 0)
                {
                    strChiefCom = dt2.Rows[0]["remarkc"].ToString().Trim().Replace(ComNum.VBLF, "");
                    strClinicalDia = dt2.Rows[0]["remarkd"].ToString().Trim().Replace(ComNum.VBLF, "");
                }

                dt2.Dispose();
                #endregion

                //int nPrintLine = 0;


                WriteStr(22, 50, 170, strTitle);       // 'titel

                WriteStr(25, 550, 230, strChiefCom);//          'Chief complaint
                WriteStr(25, 550, 320, strClinicalDia);//      'clinical Diagnosis


                string strTResult = "◈ small Intestinal ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ large Intestinal ◈ " + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark4"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ rectum ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark5"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + "◈ Endoscopic Diagnosis ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark2"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult = strTResult + strInfo + ComNum.VBLF + ComNum.VBLF;//  'add

                strTResult += "◈ Endoscopic Procedure ◈" + ComNum.VBLF + ComNum.VBLF + dt.Rows[0]["Remark3"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF;
                strTResult += "◈ Endoscopic Biopsy ◈" + ComNum.VBLF;

                strTResult += dt.Rows[0]["Remark6"].ToString().Trim() != "" ? "small Intestinal:" + dt.Rows[0]["Remark6"].ToString().Trim() + ComNum.VBLF : "";
                strTResult += dt.Rows[0]["Remark6_2"].ToString().Trim() != "" ? "large Intestinal:" + dt.Rows[0]["Remark6_2"].ToString().Trim() + ComNum.VBLF : "";
                strTResult += dt.Rows[0]["Remark6_3"].ToString().Trim() != "" ? "rectum:" + dt.Rows[0]["Remark6_3"].ToString().Trim() + ComNum.VBLF : "";
                strTResult += ComNum.VBLF;


                //참고사항
                strTResult += "◈ Remark ◈" + dt.Rows[0]["Remark"].ToString().Trim();


                strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                string[] strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 470;


                #region Text Print
                for (int k = 0; k < strResult.Length; k++)
                {
                    if (string.IsNullOrWhiteSpace(strResult[k]))
                    {
                        lngLine += 1;
                    }
                    else
                    {
                        #region 신규
                        int lastPos = 0;
                        using (Font font = new Font(FontName, 20))
                        {
                            Size PageSize = new Size(1648 - 150, 20);
                            Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                            if (strWidth.Width < 1100)
                            {
                                WriteStr(20, 150, nTop + (lngLine * 35), strResult[k]);
                                lngLine += 1;
                            }
                            else
                            {
                                for (int l = 0; l < strResult[k].Length + 1;l++)
                                {
                                    bool DataOutPut = false;
                                    string strText = strResult[k].Substring(lastPos, l);
                                    strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                    if (strWidth.Width >= 1100)
                                    {
                                        lastPos += l;
                                        l = 0;
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        DataOutPut = true;
                                    }

                                    if (DataOutPut && lastPos + l == strResult[k].Length)
                                    {
                                        break;
                                    }

                                    if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                    {
                                        WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                        lngLine += 1;
                                        break;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 기존
                        //int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                        //if (strByte >= 85)
                        //{
                        //    WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Left(strResult[k], 85));
                        //    lngLine += 1;

                        //    if (strResult[k].Length - 85 > 85)
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 85));
                        //        lngLine += 1;
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 170));

                        //    }
                        //    else
                        //    {
                        //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 85));

                        //    }
                        //}
                        //else
                        //{
                        //    WriteStr(22, nLeft, nTop + (lngLine * 35), strResult[k]);
                        //}

                        //lngLine += 1;
                        #endregion
                    }

                    #region NEW PAGE
                    if (nTop + (lngLine * 35) > 2080)
                    {
                        #region 1장 에도 검사결과 등등 정보ㅓㅓ 들어가개ㅔ 2020-03-23
                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);
                        WriteStr(25, 1260, 2230, strRDRName);
                        #endregion
                        WriteStr(25, 750, 2100, "(계속)");

                        TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                        //LNGY = 40;
                        lngLine = 1;
                        PageNum += 1;

                        New_initFormEndo_3New("내 시 경 판 독 결 과 지");

                        WriteStr(23, 50, 170, strTitle);//             'titel
                        WriteStr(25, 550, 280, strChiefCom);//         'Chief complaint
                        WriteStr(25, 550, 380, strClinicalDia);//      'clinical Diagnosis


                        WriteStr(30, 300, 2140, strRDate);
                        WriteStr(30, 300, 2180, strResultDate);
                        WriteStr(30, 950, 2160, strRDRName);
                        WriteStr(25, 1260, 2230, strRDRName);
                    }
                    #endregion

                }
                #endregion

                WriteStr(30, 300, 2140, strRDate);
                WriteStr(30, 300, 2180, strResultDate);
                WriteStr(30, 950, 2160, strRDRName);
                WriteStr(25, 1260, 2230, strRDRName);

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "내시경 에러");
                //ComFunc.MsgBox(ex.Message);
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <param name="ArgGBJob">(1:기관지, 2:위, 3:장, 4:ERCP)</param>
        public static void New_Exam_Endo_Result_4NEW(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string ArgRowid, string ArgGBJob, string strTREATNO)
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <param name="ArgGBJob">(1:기관지, 2:위, 3:장, 4:ERCP)</param>
        public static void New_Exam_Endo_Result_1(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string ArgRowid, string ArgGBJob, string strTREATNO)
        {
            if (string.IsNullOrWhiteSpace(ArgRowid))
                return;

            #region 변수
            //int LNGY = 40;
            int nTop = 580;
            int nLeft = 150;
            int lngLine = 0;

            string strTSName = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            DataTable dt2 = null;

            #endregion


            try
            {
                #region 쿼리

                SQL = "         SELECT  A.PTNO, A.SEX , (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PTNO) SNAME, A.GBIO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SEQNUM, A.ORDERCODE, ";


                //'추가정보 추가
                //'PreMEDICATION Pethidine -----------------------------------------------


                SQL += ComNum.VBLF + "          GBPRE_1,";//   ' PreMEDICATION 여부


                SQL += ComNum.VBLF + "          GBCON_4,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBCON_41,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBCON_42,";//  ' 단위


                // 'PreMEDICATION Atropin
                SQL += ComNum.VBLF + "          GBPRE_4,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBPRE_41,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBPRE_42,";//  ' 단위
                SQL += ComNum.VBLF + "          GBPRE_3, ";//  ' OTHER 여부
                SQL += ComNum.VBLF + "          GBPRE_31, ";// ' OTHER



                // 'Conscious sedation  midasolam -----------------------------------------------------
                SQL += ComNum.VBLF + "          GBCON_1,";//  ' Conscious 여부


                SQL += ComNum.VBLF + "          GBCON_2,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBCON_21,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBCON_22,";//  ' 단위



                //'Conscious sedation Propofol
                SQL += ComNum.VBLF + "          GBCON_3,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBCON_31,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBCON_32,";//  ' 단위


                SQL += ComNum.VBLF + "          GBCON_5, ";// ' OTHER  여부
                SQL += ComNum.VBLF + "          GBCON_51, ";// ' OTHER


                //'medication Epinephrine --------------------------------------------------
                SQL += ComNum.VBLF + "          GBMED_1,";//   ' medication 여부


                SQL += ComNum.VBLF + "          GBMED_2,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBMED_21,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBMED_22,";//  ' 단위


                SQL += ComNum.VBLF + "          GBMED_3,";//  ' 사용 여부
                SQL += ComNum.VBLF + "          GBMED_31,";//  ' 사용량
                SQL += ComNum.VBLF + "          GBMED_32,";//  ' 단위


                SQL += ComNum.VBLF + "          GBMED_4, ";// ' OTHER  여부
                SQL += ComNum.VBLF + "          GBMED_41, ";// ' OTHER


                SQL += ComNum.VBLF + "          TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL += ComNum.VBLF + "          A.RESULTDRCODE, TO_CHAR(A.BirthDate,'YYYY-MM-DD') BIRTHDate, ";
                SQL += ComNum.VBLF + "          B.REMARK1, B.REMARK2, B.REMARK3, B.REMARK4, B.REMARK5, B.REMARK6,";
                SQL += ComNum.VBLF + "          C.REMARKC , C.REMARKX, C.REMARKP, C.REMARKD";
                SQL += ComNum.VBLF + "     FROM KOSMOS_OCS.ENDO_JUPMST A, KOSMOS_OCS.ENDO_RESULT B , KOSMOS_OCS.ENDO_REMARK C";
                SQL += ComNum.VBLF + "     WHERE A.SEQNO = '" + ArgRowid + "' ";
                SQL += ComNum.VBLF + "       AND A.RESULTDATE IS NOT NULL";
                SQL += ComNum.VBLF + "       AND A.SEQNO = B.SEQNO";
                SQL += ComNum.VBLF + "       AND A.PTNO = C.PTNO(+)";
                SQL += ComNum.VBLF + "       AND A.JDATE = C.JDATE";
                SQL += ComNum.VBLF + "       AND A.ORDERCODE = C.ORDERCODE";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                New_initFormEndo_1NEW("내 시 경 판 독 결 과 지");


                string strPano = (dt.Rows[0]["PTNO"].ToString().Trim());
                string strSName = (dt.Rows[0]["sName"].ToString().Trim());
                string strGBIO = (dt.Rows[0]["GBio"].ToString().Trim()).Equals("I") ? "입원" : "외래";


                //'strAge = AGE_YEAR_Gesan2(Trim(dt.Rows[0]["BIRTHdate"].ToString().Trim()), Trim(dt.Rows[0]["JDATE"].ToString().Trim()))
                string strAge = AGE_YEAR_Birth(dt.Rows[0]["BIRTHdate"].ToString().Trim(), Trim(dt.Rows[0]["JDATE"].ToString().Trim())).ToString();
                string strSex = (dt.Rows[0]["Sex"].ToString().Trim());


                string strWard = (dt.Rows[0]["WardCode"].ToString().Trim());
                string strRoomCode = (dt.Rows[0]["roomcode"].ToString().Trim());
                string strDeptName = clsVbfunc.GetBASClinicDeptNameK(pDbCon, Trim(dt.Rows[0]["DeptCode"].ToString().Trim()));

                string strDrName = clsVbfunc.GetBASDoctorName(pDbCon, Trim(dt.Rows[0]["DrCode"].ToString().Trim()));
                string strJDate = (dt.Rows[0]["JDATE"].ToString().Trim());
                string strRDate = (dt.Rows[0]["Rdate"].ToString().Trim());
                string strResultDate = (dt.Rows[0]["RESULTDATE"].ToString().Trim());


                string strSeqNUM = (dt.Rows[0]["seqnum"].ToString().Trim());
                //'strSDate = Trim(RsVerify!SDate"].ToString().Trim())
                //'strEDate = Trim(RsVerify!EDate"].ToString().Trim())


                if (string.IsNullOrWhiteSpace(Trim(dt.Rows[0]["RoomCode"].ToString().Trim())))
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + "(" + strWard + "))";
                }
                else
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + ")";
                }

                string strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;

                string strRDRName = clsVbfunc.GetInSaName(pDbCon, Trim(dt.Rows[0]["Resultdrcode"].ToString().Trim()));


                #region 서브쿼리1
                SQL = " SELECT      REMARKC , REMARKX, REMARKP, REMARKD";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ENDO_REMARK";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + strPano + "'";
                SQL += ComNum.VBLF + "   AND JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND ORDERCODE = '" + Trim(dt.Rows[0]["ORDERCODE"].ToString()) + "'";


                string strChiefCom = string.Empty;
                string strClinicalDia = string.Empty;

                SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt2.Rows.Count > 0)
                {
                    strChiefCom = dt2.Rows[0]["remarkc"].ToString().Trim().Replace(ComNum.VBLF, "");
                    strClinicalDia = dt2.Rows[0]["remarkd"].ToString().Trim().Replace(ComNum.VBLF, "");
                }

                dt2.Dispose();
                #endregion

                //int nPrintLine = 0;


                WriteStr(22, 50, 170, strTitle);       // 'titel

                WriteStr(25, 650, 230, strChiefCom);//          'Chief complaint
                WriteStr(25, 650, 320, strClinicalDia);//      'clinical Diagnosis


                string strTResult = dt.Rows[0]["Remark1"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                string[] strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 530;
                TextPrint_Endo(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark2"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 820;
                TextPrint_Endo(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark3"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 1120;
                TextPrint_Endo(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark6"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 1520;
                TextPrint_Endo(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark4"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nLeft = 990;
                nTop = 1520;
                TextPrint_Endo(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);



                WriteStr(30, 300, 2140, strRDate);
                WriteStr(30, 300, 2180, strResultDate);
                WriteStr(30, 950, 2160, strRDRName);
                WriteStr(25, 1260, 2230, strRDRName);

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "내시경 에러");
                //ComFunc.MsgBox(ex.Message);
            }
        }

        public static void TextPrint_Endo(string[] strResult, string strPatid, string strDate, string strClinCode, string strTitle, string strChiefCom,
            string strClinicalDia, string strRDate, string strResultDate,  string strRDRName, ref int nLeft, ref int nTop, ref int lngLine)
        {
            #region Text Print
            for (int k = 0; k < strResult.Length; k++)
            {
                if (string.IsNullOrWhiteSpace(strResult[k]))
                {
                    lngLine += 1;
                }
                else
                {
                    #region 신규
                    int lastPos = 0;
                    using (Font font = new Font(FontName, 20))
                    {
                        Size PageSize = new Size(1648 - nLeft, 20);
                        Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                        if (strWidth.Width < 1100)
                        {
                            WriteStr(20, nLeft, nTop + (lngLine * 35), strResult[k]);
                            lngLine += 1;
                        }
                        else
                        {
                            for (int l = 0; l < strResult[k].Length + 1;l++)
                            {
                                bool DataOutPut = false;
                                string strText = strResult[k].Substring(lastPos, l);
                                strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                if (strWidth.Width >= 1100)
                                {
                                    lastPos += l;
                                    l = 0;
                                    WriteStr(20, nLeft, nTop + (lngLine * 35), strText);
                                    lngLine += 1;
                                    DataOutPut = true;
                                }

                                if (DataOutPut && lastPos + l == strResult[k].Length)
                                {
                                    break;
                                }

                                if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                {
                                    WriteStr(20, nLeft, nTop + (lngLine * 35), strText);
                                    lngLine += 1;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    #region 기존
                    //int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                    //if (strByte >= 325)
                    //{
                    //    WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Left(strResult[k], 65));
                    //    lngLine += 1;

                    //    if (strResult[k].Length - 65 > 65)
                    //    {
                    //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));
                    //        lngLine += 1;
                    //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 130));

                    //    }
                    //    else
                    //    {
                    //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));

                    //    }
                    //}
                    //else
                    //{
                    //    WriteStr(22, nLeft, nTop + (lngLine * 35), strResult[k]);
                    //}

                    //lngLine += 1;
                    #endregion
                }

                #region NEW PAGE
                if (nTop + (lngLine * 35) > 2000)
                {
                    WriteStr(25, 750, 2010, "(계속)");

                    TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                    //LNGY = 40;
                    lngLine = 1;
                    PageNum += 1;

                    New_initFormEndo_1("내 시 경 판 독 결 과 지");

                    WriteStr(22, 50, 170, strTitle);//             'titel
                    WriteStr(25, 150, 280, strChiefCom);//         'Chief complaint
                    WriteStr(25, 650, 380, strClinicalDia);//      'clinical Diagnosis
                    WriteStr(30, 300, 2140, strRDate);
                    WriteStr(30, 300, 2180, strResultDate);
                    WriteStr(30, 950, 2160, strRDRName);
                    WriteStr(25, 1260, 2230, strRDRName);
                }
                #endregion

            }
            #endregion
        }

        public static void TextPrint_Endo2(string[] strResult, string strPatid, string strDate, string strClinCode, string strTitle, string strChiefCom,
           string strClinicalDia, string strRDate, string strResultDate, string strRDRName, ref int nLeft, ref int nTop, ref int lngLine)
        {
            #region Text Print
            for (int k = 0; k < strResult.Length; k++)
            {
                if (string.IsNullOrWhiteSpace(strResult[k]))
                {
                    lngLine += 1;
                }
                else
                {
                    #region 신규
                    int lastPos = 0;
                    using (Font font = new Font(FontName, 20))
                    {
                        Size PageSize = new Size(1648 - nLeft, 20);
                        Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                        if (strWidth.Width < 1100)
                        {
                            WriteStr(20, nLeft, nTop + (lngLine * 35), strResult[k]);
                            lngLine += 1;
                        }
                        else
                        {
                            for (int l = 0; l < strResult[k].Length + 1;l++)
                            {
                                bool DataOutPut = false;
                                string strText = strResult[k].Substring(lastPos, l);
                                strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                if (strWidth.Width >= 1100)
                                {
                                    lastPos += l;
                                    l = 0;
                                    WriteStr(20, nLeft, nTop + (lngLine * 35), strText);
                                    lngLine += 1;
                                    DataOutPut = true;
                                }


                                if (DataOutPut && lastPos + l == strResult[k].Length)
                                {
                                    break;
                                }

                                if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                {
                                    WriteStr(20, nLeft, nTop + (lngLine * 35), strText);
                                    lngLine += 1;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    #region 기존
                    //int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                    //if (strByte >= 325)
                    //{
                    //    WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Left(strResult[k], 65));
                    //    lngLine += 1;

                    //    if (strResult[k].Length - 65 > 65)
                    //    {
                    //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));
                    //        lngLine += 1;
                    //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 130));

                    //    }
                    //    else
                    //    {
                    //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));

                    //    }
                    //}
                    //else
                    //{
                    //    WriteStr(22, nLeft, nTop + (lngLine * 35), strResult[k]);
                    //}

                    //lngLine += 1;
                    #endregion
                }

                #region NEW PAGE
                if (nTop + (lngLine * 35) > 2000)
                {
                    WriteStr(25, 750, 2010, "(계속)");

                    TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                    //LNGY = 40;
                    lngLine = 1;
                    PageNum += 1;

                    New_initFormEndo_2("내 시 경 판 독 결 과 지");

                    WriteStr(23, 50, 170, strTitle);//             'titel
                    WriteStr(25, 150, 280, strChiefCom);//         'Chief complaint
                    WriteStr(25, 650, 380, strClinicalDia);//      'clinical Diagnosis
                    WriteStr(30, 300, 2140, strRDate);
                    WriteStr(30, 300, 2180, strResultDate);
                    WriteStr(30, 950, 2160, strRDRName);
                    WriteStr(25, 1260, 2230, strRDRName);
                }
                #endregion

            }
            #endregion
        }

        public static void TextPrint_Endo3(string[] strResult, string strPatid, string strDate, string strClinCode, string strTitle, string strChiefCom,
           string strClinicalDia, string strRDate, string strResultDate, string strRDRName, ref int nLeft, ref int nTop, ref int lngLine)
        {
            #region Text Print
            for (int k = 0; k < strResult.Length; k++)
            {
                if (string.IsNullOrWhiteSpace(strResult[k]))
                {
                    lngLine += 1;
                }
                else
                {
                    #region 신규
                    int lastPos = 0;
                    using (Font font = new Font(FontName, 20))
                    {
                        Size PageSize = new Size(1648 - nLeft, 20);
                        Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                        if (strWidth.Width < 1100)
                        {
                            WriteStr(20, nLeft, nTop + (lngLine * 35), strResult[k]);
                            lngLine += 1;
                        }
                        else
                        {
                            for (int l = 0; l < strResult[k].Length + 1;l++)
                            {
                                bool DataOutPut = false;
                                string strText = strResult[k].Substring(lastPos, l);
                                strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                if (strWidth.Width >= 1100)
                                {
                                    lastPos += l;
                                    l = 0;
                                    WriteStr(20, nLeft, nTop + (lngLine * 35), strText);
                                    lngLine += 1;
                                    DataOutPut = true;
                                }

                                if (DataOutPut && lastPos + l == strResult[k].Length)
                                {
                                    break;
                                }

                                if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                {
                                    WriteStr(20, nLeft, nTop + (lngLine * 35), strText);
                                    lngLine += 1;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    #region 기존
                    //int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                    //if (strByte >= 85)
                    //{
                    //    WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Left(strResult[k], 85));
                    //    lngLine += 1;

                    //    if (strResult[k].Length - 65 > 65)
                    //    {
                    //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 85));
                    //        lngLine += 1;
                    //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 170));

                    //    }
                    //    else
                    //    {
                    //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 85));

                    //    }
                    //}
                    //else
                    //{
                    //    WriteStr(22, nLeft, nTop + (lngLine * 35), strResult[k]);
                    //}

                    //lngLine += 1;
                    #endregion
                }

                #region NEW PAGE
                if (nTop + (lngLine * 35) > 2000)
                {
                    WriteStr(25, 750, 2010, "(계속)");

                    TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                    //LNGY = 40;
                    lngLine = 1;
                    PageNum += 1;

                    New_initFormEndo_3("내 시 경 판 독 결 과 지");

                    WriteStr(23, 50, 170, strTitle);//             'titel
                    WriteStr(25, 550, 230, strChiefCom);//         'Chief complaint
                    WriteStr(25, 550, 320, strClinicalDia);//      'clinical Diagnosis

                    WriteStr(30, 300, 2140, strRDate);
                    WriteStr(30, 300, 2180, strResultDate);
                    WriteStr(30, 950, 2160, strRDRName);
                    WriteStr(25, 1260, 2230, strRDRName);
                }
                #endregion

            }
            #endregion
        }

        public static void TextPrint_Endo4(string[] strResult, string strPatid, string strDate, string strClinCode, string strTitle, string strChiefCom,
        string strClinicalDia, string strRDate, string strResultDate, string strRDRName, ref int nLeft, ref int nTop, ref int lngLine)
        {
            #region Text Print
            for (int k = 0; k < strResult.Length; k++)
            {
                if (string.IsNullOrWhiteSpace(strResult[k]))
                {
                    lngLine += 1;
                }
                else
                {
                    #region 신규
                    int lastPos = 0;
                    using (Font font = new Font(FontName, 20))
                    {
                        Size PageSize = new Size(1648 - nLeft, 20);
                        Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                        if (strWidth.Width < 1100)
                        {
                            WriteStr(20, nLeft, nTop + (lngLine * 35), strResult[k]);
                            lngLine += 1;
                        }
                        else
                        {
                            for (int l = 0; l < strResult[k].Length + 1;l++)
                            {
                                bool DataOutPut = false;
                                string strText = strResult[k].Substring(lastPos, l);
                                strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                if (strWidth.Width >= 1100)
                                {
                                    lastPos += l;
                                    l = 0;
                                    WriteStr(20, nLeft, nTop + (lngLine * 35), strText);
                                    lngLine += 1;
                                    DataOutPut = true;
                                }

                                if (DataOutPut && lastPos + l == strResult[k].Length)
                                {
                                    break;
                                }

                                if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                {
                                    WriteStr(20, nLeft, nTop + (lngLine * 35), strText);
                                    lngLine += 1;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    #region 기존
                    //int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                    //if (strByte >= 325)
                    //{
                    //    WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Left(strResult[k], 65));
                    //    lngLine += 1;

                    //    if (strResult[k].Length - 65 > 65)
                    //    {
                    //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));
                    //        lngLine += 1;
                    //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 130));

                    //    }
                    //    else
                    //    {
                    //        WriteStr(20, nLeft, nTop + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 85));

                    //    }
                    //}
                    //else
                    //{
                    //    WriteStr(22, nLeft, nTop + (lngLine * 35), strResult[k]);
                    //}

                    //lngLine += 1;
                    #endregion
                }

                #region NEW PAGE
                if (nTop + (lngLine * 35) > 2000)
                {
                    WriteStr(25, 750, 2010, "(계속)");

                    TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                    //LNGY = 40;
                    lngLine = 1;
                    PageNum += 1;

                    New_initFormEndo_4("내 시 경 판 독 결 과 지");

                    WriteStr(23, 50, 170, strTitle);//             'titel
                    WriteStr(25, 150, 280, strChiefCom);//         'Chief complaint
                    WriteStr(25, 650, 380, strClinicalDia);//      'clinical Diagnosis
                    WriteStr(30, 300, 2140, strRDate);
                    WriteStr(30, 300, 2180, strResultDate);
                    WriteStr(30, 950, 2160, strRDRName);
                    WriteStr(25, 1260, 2230, strRDRName);
                }
                #endregion

            }
            #endregion
        }

        /// <summary>
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <param name="ArgGBJob">(1:기관지, 2:위, 3:장, 4:ERCP)</param>
        public static void New_Exam_Endo_Result_2(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string ArgRowid, string ArgGBJob, string strTREATNO)
        {
            if (string.IsNullOrWhiteSpace(ArgRowid))
                return;

            #region 변수
            //int LNGY = 40;
            int nTop = 580;
            int nLeft = 150;
            int lngLine = 0;

            string strTSName = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            DataTable dt2 = null;

            #endregion


            try
            {
                #region 쿼리

                SQL = "         SELECT  A.PTNO, A.SEX , (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PTNO) SNAME, A.GBIO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SEQNUM,A.ORDERCODE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL += ComNum.VBLF + "          A.RESULTDRCODE, TO_CHAR(A.BirthDate,'YYYY-MM-DD') BIRTHDate, ";
                SQL += ComNum.VBLF + "          B.REMARK1, B.REMARK2, B.REMARK3, B.REMARK4, B.REMARK5, B.REMARK6 ";


                SQL += ComNum.VBLF + "     FROM KOSMOS_OCS.ENDO_JUPMST A, KOSMOS_OCS.ENDO_RESULT B ";
               //' SQL = SQL & "     WHERE A.PTNO ='" & strPatid & "'"
                SQL += ComNum.VBLF + "     WHERE A.SEQNO = '" + ArgRowid +"' ";
                SQL += ComNum.VBLF + "       AND A.RESULTDATE IS NOT NULL";
                SQL += ComNum.VBLF + "       AND A.SEQNO = B.SEQNO";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                New_initFormEndo_2("내 시 경 판 독 결 과 지");

                string strPano = (dt.Rows[0]["PTNO"].ToString().Trim());
                string strSName = (dt.Rows[0]["sName"].ToString().Trim());
                string strGBIO = (dt.Rows[0]["GBio"].ToString().Trim()).Equals("I") ? "입원" : "외래";


                //'strAge = AGE_YEAR_Gesan2(Trim(dt.Rows[0]["BIRTHdate"].ToString().Trim()), Trim(dt.Rows[0]["JDATE"].ToString().Trim()))
                string strAge = AGE_YEAR_Birth(dt.Rows[0]["BIRTHdate"].ToString().Trim(), Trim(dt.Rows[0]["JDATE"].ToString().Trim())).ToString();
                string strSex = (dt.Rows[0]["Sex"].ToString().Trim());


                string strWard = (dt.Rows[0]["WardCode"].ToString().Trim());
                string strRoomCode = (dt.Rows[0]["roomcode"].ToString().Trim());
                string strDeptName = clsVbfunc.GetBASClinicDeptNameK(pDbCon, Trim(dt.Rows[0]["DeptCode"].ToString().Trim()));

                string strDrName = clsVbfunc.GetBASDoctorName(pDbCon, Trim(dt.Rows[0]["DrCode"].ToString().Trim()));
                string strJDate = (dt.Rows[0]["JDATE"].ToString().Trim());
                string strRDate = (dt.Rows[0]["Rdate"].ToString().Trim());
                string strResultDate = (dt.Rows[0]["RESULTDATE"].ToString().Trim());


                string strSeqNUM = (dt.Rows[0]["seqnum"].ToString().Trim());
                //'strSDate = Trim(RsVerify!SDate"].ToString().Trim())
                //'strEDate = Trim(RsVerify!EDate"].ToString().Trim())


                if (string.IsNullOrWhiteSpace(Trim(dt.Rows[0]["RoomCode"].ToString().Trim())))
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + "(" + strWard + "))";
                }
                else
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + ")";
                }

                string strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;

                string strRDRName = clsVbfunc.GetInSaName(pDbCon, Trim(dt.Rows[0]["Resultdrcode"].ToString().Trim()));


                #region 서브쿼리1
                SQL = " SELECT      REMARKC , REMARKX, REMARKP, REMARKD";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ENDO_REMARK";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + strPano + "'";
                SQL += ComNum.VBLF + "   AND JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND ORDERCODE = '" + Trim(dt.Rows[0]["ORDERCODE"].ToString()) + "'";


                string strChiefCom = string.Empty;
                string strClinicalDia = string.Empty;

                SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt2.Rows.Count > 0)
                {
                    strChiefCom = dt2.Rows[0]["remarkc"].ToString().Trim().Replace(ComNum.VBLF, "");
                    strClinicalDia = dt2.Rows[0]["remarkd"].ToString().Trim().Replace(ComNum.VBLF, "");
                }

                dt2.Dispose();
                #endregion

                //int nPrintLine = 0;


                WriteStr(22, 50, 170, strTitle);       // 'titel

                WriteStr(25, 150, 280, strChiefCom);//          'Chief complaint
                WriteStr(25, 650, 380, strClinicalDia);//      'clinical Diagnosis


                string strTResult = dt.Rows[0]["Remark1"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                string[] strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 530;
                TextPrint_Endo2(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark2"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 820;
                TextPrint_Endo2(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark3"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 1120;
                TextPrint_Endo2(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark4"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 1320;
                TextPrint_Endo2(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark6"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 1720;
                TextPrint_Endo2(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);

                strTResult = dt.Rows[0]["Remark5"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nLeft = 990;
                nTop = 1720;
                TextPrint_Endo2(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                WriteStr(30, 300, 2140, strRDate);
                WriteStr(30, 300, 2180, strResultDate);
                WriteStr(30, 950, 2160, strRDRName);
                WriteStr(25, 1260, 2230, strRDRName);

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "내시경 에러");
                //ComFunc.MsgBox(ex.Message);
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <param name="ArgGBJob">(1:기관지, 2:위, 3:장, 4:ERCP)</param>
        public static void New_Exam_Endo_Result_3(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string ArgRowid, string ArgGBJob, string strTREATNO)
        {
            if (string.IsNullOrWhiteSpace(ArgRowid))
                return;

            #region 변수
            //int LNGY = 40;
            int nTop = 580;
            int nLeft = 150;
            int lngLine = 0;

            string strTSName = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            DataTable dt2 = null;

            #endregion


            try
            {
                #region 쿼리

                SQL = "         SELECT  A.PTNO, A.SEX , (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PTNO) SNAME, A.GBIO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SEQNUM,A.ORDERCODE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL += ComNum.VBLF + "          A.RESULTDRCODE, TO_CHAR(A.BirthDate,'YYYY-MM-DD') BIRTHDate, ";
                SQL += ComNum.VBLF + "          B.REMARK1, B.REMARK2, B.REMARK3, B.REMARK4, B.REMARK5, B.REMARK6 ";


                SQL += ComNum.VBLF + "     FROM KOSMOS_OCS.ENDO_JUPMST A, KOSMOS_OCS.ENDO_RESULT B ";
                //' SQL = SQL & "     WHERE A.PTNO ='" & strPatid & "'"
                SQL += ComNum.VBLF + "     WHERE A.SEQNO = '" + ArgRowid + "' ";
                SQL += ComNum.VBLF + "       AND A.RESULTDATE IS NOT NULL";
                SQL += ComNum.VBLF + "       AND A.SEQNO = B.SEQNO";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                New_initFormEndo_3("내 시 경 판 독 결 과 지");

                string strPano = (dt.Rows[0]["PTNO"].ToString().Trim());
                string strSName = (dt.Rows[0]["sName"].ToString().Trim());
                string strGBIO = (dt.Rows[0]["GBio"].ToString().Trim()).Equals("I") ? "입원" : "외래";


                //'strAge = AGE_YEAR_Gesan2(Trim(dt.Rows[0]["BIRTHdate"].ToString().Trim()), Trim(dt.Rows[0]["JDATE"].ToString().Trim()))
                string strAge = AGE_YEAR_Birth(dt.Rows[0]["BIRTHdate"].ToString().Trim(), Trim(dt.Rows[0]["JDATE"].ToString().Trim())).ToString();
                string strSex = (dt.Rows[0]["Sex"].ToString().Trim());


                string strWard = (dt.Rows[0]["WardCode"].ToString().Trim());
                string strRoomCode = (dt.Rows[0]["roomcode"].ToString().Trim());
                string strDeptName = clsVbfunc.GetBASClinicDeptNameK(pDbCon, Trim(dt.Rows[0]["DeptCode"].ToString().Trim()));

                string strDrName = clsVbfunc.GetBASDoctorName(pDbCon, Trim(dt.Rows[0]["DrCode"].ToString().Trim()));
                string strJDate = (dt.Rows[0]["JDATE"].ToString().Trim());
                string strRDate = (dt.Rows[0]["Rdate"].ToString().Trim());
                string strResultDate = (dt.Rows[0]["RESULTDATE"].ToString().Trim());


                string strSeqNUM = (dt.Rows[0]["seqnum"].ToString().Trim());
                //'strSDate = Trim(RsVerify!SDate"].ToString().Trim())
                //'strEDate = Trim(RsVerify!EDate"].ToString().Trim())


                if (string.IsNullOrWhiteSpace(Trim(dt.Rows[0]["RoomCode"].ToString().Trim())))
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + "(" + strWard + "))";
                }
                else
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + ")";
                }

                string strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;

                string strRDRName = clsVbfunc.GetInSaName(pDbCon, Trim(dt.Rows[0]["Resultdrcode"].ToString().Trim()));


                #region 서브쿼리1
                SQL = " SELECT      REMARKC , REMARKX, REMARKP, REMARKD";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ENDO_REMARK";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + strPano + "'";
                SQL += ComNum.VBLF + "   AND JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND ORDERCODE = '" + Trim(dt.Rows[0]["ORDERCODE"].ToString()) + "'";


                string strChiefCom = string.Empty;
                string strClinicalDia = string.Empty;

                SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt2.Rows.Count > 0)
                {
                    strChiefCom = dt2.Rows[0]["remarkc"].ToString().Trim().Replace(ComNum.VBLF, "");
                    strClinicalDia = dt2.Rows[0]["remarkd"].ToString().Trim().Replace(ComNum.VBLF, "");
                }

                dt2.Dispose();
                #endregion

                //int nPrintLine = 0;


                WriteStr(22, 50, 170, strTitle);       // 'titel

                WriteStr(25, 550, 230, strChiefCom);//          'Chief complaint
                WriteStr(25, 550, 320, strClinicalDia);//      'clinical Diagnosis


                string strTResult = dt.Rows[0]["Remark1"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                string[] strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 470;
                TextPrint_Endo3(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark2"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 920;
                TextPrint_Endo3(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark6"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 1520;
                TextPrint_Endo3(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark3"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 1520;
                nLeft = 990;
                TextPrint_Endo3(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                WriteStr(30, 300, 2140, strRDate);
                WriteStr(30, 300, 2180, strResultDate);
                WriteStr(30, 950, 2160, strRDRName);
                WriteStr(25, 1260, 2230, strRDRName);

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "내시경 에러");
                //ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <param name="ArgGBJob">(1:기관지, 2:위, 3:장, 4:ERCP)</param>
        public static void New_Exam_Endo_Result_4(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string ArgRowid, string ArgGBJob, string strTREATNO)
        {
            if (string.IsNullOrWhiteSpace(ArgRowid))
                return;

            #region 변수
            //int LNGY = 40;
            int nTop = 580;
            int nLeft = 150;
            int lngLine = 0;

            string strTSName = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            DataTable dt2 = null;

            #endregion


            try
            {
                #region 쿼리

                SQL = "         SELECT  A.PTNO, A.SEX , (SELECT SNAME FROM KOSMOS_PMPA.BAS_PATIENT WHERE PANO = A.PTNO) SNAME, A.GBIO, A.WARDCODE, A.ROOMCODE, A.DEPTCODE, A.DRCODE, A.SEQNUM,A.ORDERCODE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.JDATE,'YYYY-MM-DD') JDATE ,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RDATE,'YYYY-MM-DD') RDATE,";
                SQL += ComNum.VBLF + "          TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE,";
                SQL += ComNum.VBLF + "          A.RESULTDRCODE, TO_CHAR(A.BirthDate,'YYYY-MM-DD') BIRTHDate, ";
                SQL += ComNum.VBLF + "          B.REMARK1, B.REMARK2, B.REMARK3, B.REMARK4, B.REMARK5, B.REMARK6 ";
                SQL += ComNum.VBLF + "     FROM KOSMOS_OCS.ENDO_JUPMST A, KOSMOS_OCS.ENDO_RESULT B ";
                //' SQL = SQL & "     WHERE A.PTNO ='" & strPatid & "'"
                SQL += ComNum.VBLF + "     WHERE A.SEQNO = '" + ArgRowid + "' ";
                SQL += ComNum.VBLF + "       AND A.RESULTDATE IS NOT NULL";
                SQL += ComNum.VBLF + "       AND A.SEQNO = B.SEQNO";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                New_initFormEndo_4("내 시 경 판 독 결 과 지");

                string strPano = (dt.Rows[0]["PTNO"].ToString().Trim());
                string strSName = (dt.Rows[0]["sName"].ToString().Trim());
                string strGBIO = (dt.Rows[0]["GBio"].ToString().Trim()).Equals("I") ? "입원" : "외래";


                //'strAge = AGE_YEAR_Gesan2(Trim(dt.Rows[0]["BIRTHdate"].ToString().Trim()), Trim(dt.Rows[0]["JDATE"].ToString().Trim()))
                string strAge = AGE_YEAR_Birth(dt.Rows[0]["BIRTHdate"].ToString().Trim(), Trim(dt.Rows[0]["JDATE"].ToString().Trim())).ToString();
                string strSex = (dt.Rows[0]["Sex"].ToString().Trim());


                string strWard = (dt.Rows[0]["WardCode"].ToString().Trim());
                string strRoomCode = (dt.Rows[0]["roomcode"].ToString().Trim());
                string strDeptName = clsVbfunc.GetBASClinicDeptNameK(pDbCon, Trim(dt.Rows[0]["DeptCode"].ToString().Trim()));

                string strDrName = clsVbfunc.GetBASDoctorName(pDbCon, Trim(dt.Rows[0]["DrCode"].ToString().Trim()));
                string strJDate = (dt.Rows[0]["JDATE"].ToString().Trim());
                string strRDate = (dt.Rows[0]["Rdate"].ToString().Trim());
                string strResultDate = (dt.Rows[0]["RESULTDATE"].ToString().Trim());


                string strSeqNUM = (dt.Rows[0]["seqnum"].ToString().Trim());
                //'strSDate = Trim(RsVerify!SDate"].ToString().Trim())
                //'strEDate = Trim(RsVerify!EDate"].ToString().Trim())


                if (!string.IsNullOrWhiteSpace(Trim(dt.Rows[0]["RoomCode"].ToString().Trim())))
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + "(" + strWard + "))";
                }
                else
                {
                    strTSName = strSName + "(" + strSex + "/" + strAge + "세/" + strGBIO + "/" + VB.Left(strRoomCode + VB.Space(3), 3) + ")";
                }

                string strTitle = "등록번호:" + strPano + " 성명:" + strTSName + " Dr:" + strDrName + " No:" + strSeqNUM + " 검사요청일:" + strJDate;

                string strRDRName = clsVbfunc.GetInSaName(pDbCon, Trim(dt.Rows[0]["Resultdrcode"].ToString().Trim()));


                #region 서브쿼리1
                SQL = " SELECT      REMARKC , REMARKX, REMARKP, REMARKD";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.ENDO_REMARK";
                SQL += ComNum.VBLF + " WHERE PTNO = '" + strPano + "'";
                SQL += ComNum.VBLF + "   AND JDATE = TO_DATE('" + strJDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "   AND ORDERCODE = '" + Trim(dt.Rows[0]["ORDERCODE"].ToString()) + "'";


                string strChiefCom = string.Empty;
                string strClinicalDia = string.Empty;

                SqlErr = clsDB.GetDataTableREx(ref dt2, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return;
                }

                if (dt2.Rows.Count > 0)
                {
                    strChiefCom = dt2.Rows[0]["remarkc"].ToString().Trim().Replace(ComNum.VBLF, "");
                    strClinicalDia = dt2.Rows[0]["remarkd"].ToString().Trim().Replace(ComNum.VBLF, "");
                }

                dt2.Dispose();
                #endregion

                //int nPrintLine = 0;


                WriteStr(22, 50, 170, strTitle);       // 'titel

                WriteStr(25, 150, 280, strChiefCom);//          'Chief complaint
                WriteStr(25, 650, 380, strClinicalDia);//      'clinical Diagnosis


                string strTResult = dt.Rows[0]["Remark1"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                string[] strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 520;
                TextPrint_Endo4(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark2"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 920;
                TextPrint_Endo4(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark3"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 1320;
                TextPrint_Endo4(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                strTResult = dt.Rows[0]["Remark6"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 1720;
                TextPrint_Endo4(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);

                strTResult = dt.Rows[0]["Remark4"].ToString().Trim().Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                //'lngLine = lngLine + 2
                lngLine = 1;
                nTop = 1720;
                nLeft = 990;
                TextPrint_Endo4(strResult, strPatid, strDate, strClinCode, strTitle, strChiefCom, strClinicalDia, strRDate, strResultDate, strRDRName, ref nLeft, ref nTop, ref lngLine);


                WriteStr(30, 300, 2140, strRDate);
                WriteStr(30, 300, 2180, strResultDate);
                WriteStr(30, 950, 2160, strRDRName);
                WriteStr(25, 1260, 2230, strRDRName);

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "내시경 에러");
                //ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// AGE_YEAR_Birth( *** 생년월일이 오류인 경우 10살로 처리함 ***)
        /// </summary>
        /// <param name="strBirth">생년월일</param>
        /// <param name="argDate">나이를 계산할 기준일자 (YYYY-MM-DD)</param>
        /// <returns></returns>
        public static string AGE_YEAR_Birth(string strBirth, string argDate)
        {
            string rtnVal = string.Empty;
            OracleDataReader reader = null;
            string SqlErr = string.Empty;
            string SQL = string.Empty;

            try
            {
                int ArgJuminLen = strBirth.Replace("-", "").Trim().Length;

                #region 주민번호가 7보다 적으면 오류, 기준일자는 반드시 'YYYYMMDD' Type이여야 함
                if (ArgJuminLen != 8 || argDate.Length != 10)
                {
                    rtnVal = "10";
                    return rtnVal;
                }
                #endregion

                #region 기준일자가 생년월일 보다 적으면 0살 처리
                if (VB.Val(strBirth.Replace("-", "")) > VB.Val(argDate.Replace("-", "")))
                {
                    rtnVal = "10";
                    return rtnVal;
                }
                #endregion

                #region 주민번호가 오류이면 10살 처리
                if (VB.IsDate(strBirth) == false)
                {
                    rtnVal = "10";
                    return rtnVal;
                }
                #endregion

                #region 쿼리
                SQL = "SELECT TRUNC(MONTHS_BETWEEN(TO_DATE('" + argDate + "','YYYY-MM-DD'),"; ;
                SQL += ComNum.VBLF + "       TO_DATE('" + strBirth + "','YYYY-MM-DD'))) cAge FROM DUAL";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    rtnVal = (Math.Truncate(VB.Val(reader.GetValue(0).ToString().Trim()) / 12)).ToString();
                }

                reader.Dispose();
                #endregion
            }
            catch
            {
                //ComFunc.MsgBox(ex.Message);
                rtnVal = "10";
                return rtnVal;
            }

            return rtnVal;
        }

        #endregion

        #region 진단검사의학과 검사 종합검증/판독 보고서

        public static bool New_Exam_Verify(PsmhDb pDbCon, string var, string strPatid, string strDate, string strClinCode, string strClass, string strOutDate, string strDocCode, string strTREATNO)
        {
            //외래는 제외
            if (strClass.Equals("O"))
                return false;


            #region 변수
            bool rtnVal = false;
            gstrFormcode = strClass.Trim().Equals("I") ? "111" : "004";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            PageNum = 0;

            //string FileName = string.Empty;
            string strPtno = string.Empty;
            //string strPath = string.Empty;
            //string strPathB = string.Empty;
            //string strPathR = string.Empty;
            strOutDate = string.IsNullOrWhiteSpace(strOutDate) ? ComQuery.CurrentDateTime(pDbCon, "S").Substring(0, 10) : strOutDate;
            #endregion
           

            try
            {

                #region 쿼리
                SQL = " SELECT A.ROWID ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.EXAM_VERIFY A ";
                SQL += ComNum.VBLF + " WHERE A.PANO = '" + strPatid + "' ";
                SQL += ComNum.VBLF + "   AND A.JDATE >=TO_DATE('2010-01-01','YYYY-MM-DD') ";
                //SQL += ComNum.VBLF + "   AND A.JDATE >=TO_DATE('2011-01-01','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND A.STATUS ='3'";//  '결과 완료된것
                SQL += ComNum.VBLF + "   AND A.RESULTDATE IS NOT NULL ";


                //'김민원 오동호류마티스내과....
                if(strClinCode.Equals("RA") || strClinCode.Equals("MR"))
                {
                    //SQL += ComNum.VBLF + "   AND A.JDATE >=TO_DATE('2011-01-01','YYYY-MM-DD') ";

                    SQL += ComNum.VBLF + "   AND A.INDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "   AND A.INDATE <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";

                    //SQL += ComNum.VBLF + "  AND to_char(A.INDATE,'YYYYMMDD') >='" + strDate + "' ";
                    //SQL += ComNum.VBLF + "  AND to_char(A.INDATE,'YYYYMMDD') <='" + strOutDate + "' ";
                }
                else
                {
                    SQL += ComNum.VBLF + "   AND A.INDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";

                    if (strClinCode.Equals("ER"))
                    {
                        SQL += ComNum.VBLF + "   AND A.INDATE <= TO_DATE('" + DateTime.ParseExact(strOutDate, "yyyyMMdd", null).AddDays(+1).ToString("yyyyMMdd") + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "   AND A.INDATE <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";

                    }


                    //SQL += ComNum.VBLF + "  AND to_char(A.INDATE,'YYYYMMDD') >='" + strDate + "' ";
                    //SQL += ComNum.VBLF + "  AND to_char(A.INDATE,'YYYYMMDD') <='" + strOutDate + "' ";
                    SQL += ComNum.VBLF + "  AND A.DRCODE NOT IN (  '1107','1125' ,'0901','0902','0903')  ";
                }
                SQL += ComNum.VBLF + " ORDER BY A.INDATE ";
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    return rtnVal;
                }

                SaveCvtLog(strPatid, strTREATNO, "종합검증");

                while (reader.Read())
                {
                    PageNum += 1;

                    New_Exam_Verify_Result(pDbCon, strPatid, strDate, strClinCode, strClass, reader.GetValue(0).ToString().Trim(), strTREATNO);
                }

                reader.Dispose();

                rtnVal = true;
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "종합검증 에러");
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// 진단검사의학과 검사 종합검증/판독 보고서
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="ArgRowid"></param>
        /// <returns></returns>
        public static bool New_Exam_Verify_Result(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode,
                                     string strClass, string ArgRowid, string strTREATNO)
        {
            if (string.IsNullOrWhiteSpace(ArgRowid))
                return false;

            #region 변수
            bool rtnVal = false;
            gstrFormcode = strClass.Trim().Equals("I") ? "111" : "004";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            PageNum = 0;


            int LNGY = 40;
            int lngLine = 0;
            string[] strResult;
            //string[] strResult1;
            string strTResult = string.Empty;
            //int PAGENO = 0;

            string strDrName = string.Empty;
            //string strPath = string.Empty;
            int nTop = 580;

            string strPano = string.Empty;
            string strSName = string.Empty;
            string strAgeSex = string.Empty;
            string strWard = string.Empty;
            string strDeptName = string.Empty;

            string strJDate = string.Empty;
            string strSDate = string.Empty;
            string strEdate = string.Empty;
            string strResultDate = string.Empty;
            int nResultSabun = 0;
            string strRDRName = string.Empty;
            string strRDrBunho = string.Empty;
            string strYear = string.Empty;

            //int nPrintLine = 0;
            #endregion


            try
            {

                #region 쿼리
                SQL = "SELECT Pano,Sname,Sex,Age,DeptCode,DrCode,Ward,Status,";
                SQL += ComNum.VBLF + "TO_CHAR(JDate,'YYYY-MM-DD') JDate,";
                SQL += ComNum.VBLF + "TO_CHAR(InDate,'YYYY-MM-DD') InDate,";
                SQL += ComNum.VBLF + "TO_CHAR(ResultDate,'YYYY-MM-DD') ResultDate,";
                SQL += ComNum.VBLF + "TO_CHAR(SDate,'YYYY-MM-DD') SDate,";
                SQL += ComNum.VBLF + "TO_CHAR(EDate,'YYYY-MM-DD') EDate,";
                SQL += ComNum.VBLF + "ResultSabun,Items1,Items2,Disease,";
                SQL += ComNum.VBLF + "Verify1,Verify2,Verify3,Verify4,Verify5,Verify6,";
                SQL += ComNum.VBLF + "Comments,Recommendation,Print ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.EXAM_VERIFY ";
                SQL += ComNum.VBLF + "WHERE ROWID = '" + ArgRowid + "' ";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                New_initFormVerify("진단검사의학과 검사 종합검증/판독 보고서");

                strPano = dt.Rows[0]["Pano"].ToString().Trim();
                strSName = dt.Rows[0]["sName"].ToString().Trim();
                strAgeSex = dt.Rows[0]["Age"].ToString().Trim() + "/" + dt.Rows[0]["Sex"].ToString().Trim();
                strWard = dt.Rows[0]["Ward"].ToString().Trim();
                strDeptName = clsVbfunc.GetBASClinicDeptNameK(pDbCon, dt.Rows[0]["DeptCode"].ToString().Trim());

                strDrName = clsVbfunc.GetBASDoctorName(pDbCon, dt.Rows[0]["DrCode"].ToString().Trim());
                strJDate = dt.Rows[0]["JDATE"].ToString().Trim();
                strResultDate = dt.Rows[0]["RESULTDATE"].ToString().Trim();
                strSDate = dt.Rows[0]["SDate"].ToString().Trim();
                strEdate = dt.Rows[0]["EDate"].ToString().Trim();
                nResultSabun = (int)VB.Val(dt.Rows[0]["ResultSabun"].ToString().Trim());
                //'nPrint = Val(Trim(RsVerify!Print & ""))
                //nPrintLine = 0;

                #region 전문의번호 및 성명을 READ
                switch (nResultSabun)
                {
                    case 9089:
                        strRDRName = "김성철";
                        strRDrBunho = "301";
                        break;
                    case 18210:
                        strRDRName = "은상진";
                        strRDrBunho = "424";
                        break;
                    case 39874:
                        strRDRName = "양선문";
                        strRDrBunho = "605";
                        break;
                    default:
                        strRDRName = "의사오류";
                        strRDrBunho = "***";
                        break;
                }
                #endregion

                WriteStr(25, 350, 170, strPano);//  '등록번호
                WriteStr(25, 350, 210, strSName);//     '좐자 성명
                WriteStr(25, 350, 250, strAgeSex);//     '나이/성별

                WriteStr(25, 1080, 170, strWard);//    '등록번호
                WriteStr(25, 1080, 210, strDeptName);//  '좐자 성명
                WriteStr(25, 1080, 250, strDrName);//    '나이/성별


                nTop = 310;

                #region 임상소견
                WriteStr(25, 150, nTop + (lngLine * 35), "임상 소견: " + dt.Rows[0]["Disease"].ToString().Trim());// '임상소견

                hline2(50, nTop + (lngLine * 35) + 50, 1553);
                lngLine += 1;
                #endregion

                #region 검증항목(열거)
                lngLine += 1;

                if (string.IsNullOrWhiteSpace(strSDate))
                {
                    WriteStr(30, 90, nTop + (lngLine * 35), "■ 검증항목(열거)");
                }
                else
                {
                    WriteStr(30, 90, nTop + (lngLine * 35), "■ 검증항목");

                    WriteStr(25, 370, nTop + (lngLine * 35), "(검사기간:  " + strSDate + " ~ " + strEdate + ")");
                }

                strTResult = dt.Rows[0]["Items1"].ToString().Trim();
                strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                lngLine += 2;

                TextPrint(strResult, nTop, strPatid, strDate, strClinCode, strPano, strSName, strAgeSex, strWard, strDeptName, strDrName,
                    strJDate, strRDRName, strRDrBunho, ref lngLine, ref LNGY);

                hline2(50, nTop + (lngLine * 35) + 10, 1553);
                #endregion


                #region 비정상 결과 혹은 유의한 결과를 보이는 항목
                lngLine += 1;

                WriteStr(30, 90, nTop + (lngLine * 35), "■ 비정상 결과 혹은 유의한 결과를 보이는 항목");

                strTResult = dt.Rows[0]["Items2"].ToString().Trim();
                strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                lngLine += 2;

                TextPrint(strResult, nTop, strPatid, strDate, strClinCode, strPano, strSName, strAgeSex, strWard, strDeptName, strDrName,
                    strJDate, strRDRName, strRDrBunho, ref lngLine, ref LNGY);

                hline2(50, nTop + (lngLine * 35) + 10, 1553);
                #endregion

                #region 검증방법
                lngLine += 1;
                WriteStr(30, 90, nTop + (lngLine * 35), "■ 검증방법");

                lngLine += 1;
                WriteStr(25, 150, nTop + (lngLine * 35), (dt.Rows[0]["Verify1"].ToString().Trim().Equals("Y") ? "●" : "○") + "Calibratrion Verification");

                WriteStr(25, 900, nTop + (lngLine * 35), (dt.Rows[0]["Verify4"].ToString().Trim().Equals("Y") ? "●" : "○") + "Internal Quality Control");

                lngLine += 1;
                WriteStr(25, 150, nTop + (lngLine * 35), (dt.Rows[0]["Verify2"].ToString().Trim().Equals("Y") ? "●" : "○") + "Delta Check Verification");
                WriteStr(25, 900, nTop + (lngLine * 35), (dt.Rows[0]["Verify5"].ToString().Trim().Equals("Y") ? "●" : "○") + "Panic/Alert Value Verification");

                lngLine += 1;
                WriteStr(25, 150, nTop + (lngLine * 35), (dt.Rows[0]["Verify3"].ToString().Trim().Equals("Y") ? "●" : "○") + "Repeat/Recheck");

                if (string.IsNullOrWhiteSpace(dt.Rows[0]["Verify6"].ToString().Trim()) == false)
                {
                    WriteStr(25, 900, nTop + (lngLine * 35), "●Others" + dt.Rows[0]["Verify6"].ToString().Trim());
                }
                else
                {
                    WriteStr(25, 900, nTop + (lngLine * 35), "○Others");
                }

                lngLine += 1;
                WriteStr(25, 150, nTop + (lngLine * 35), "* 검사결과는 위의 표시된 방법에 의하여 검증되었습니다.");

                lngLine += 1;
                //WriteStr(25, 150, nTop + (lngLine * 35), "■ 검증방법");

                hline2(50, nTop + (lngLine * 35) + 10, 1553);
                #endregion

                #region 검증/판독 소견(Comments)
                lngLine += 1;

                WriteStr(30, 90, nTop + (lngLine * 35), "■ 검증/판독 소견(Comments)");

                strTResult = dt.Rows[0]["Comments"].ToString().Trim();
                strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                lngLine += 2;

                TextPrint(strResult, nTop, strPatid, strDate, strClinCode, strPano, strSName, strAgeSex, strWard, strDeptName, strDrName,
                    strJDate, strRDRName, strRDrBunho, ref lngLine, ref LNGY);
                #endregion

                #region 추천(Recommendation)
                lngLine += 1;

                WriteStr(30, 90, nTop + (lngLine * 35), "■ 추천(Recommendation)");

                strTResult = dt.Rows[0]["ReCommendation"].ToString().Trim();
                strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');
                lngLine += 2;

                TextPrint(strResult, nTop, strPatid, strDate, strClinCode, strPano, strSName, strAgeSex, strWard, strDeptName, strDrName,
                    strJDate, strRDRName, strRDrBunho, ref lngLine, ref LNGY);

                hline2(50, nTop + (lngLine * 35) + 10, 1553);
                #endregion

                #region 다음장
                if (lngLine > 50)
                {
                    WriteStr(25, 650, 2150, "(계속)");

                    TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                    LNGY = 40;
                    lngLine = 1;
                    PageNum += 1;


                    New_initFormVerify("진단검사의학과 검사 종합검증/판독 보고서");

                    WriteStr(25, 350, 170, strPano);//  '등록번호
                    WriteStr(25, 350, 210, strSName);//  '좐자 성명
                    WriteStr(25, 350, 250, strAgeSex);//  '나이/성별

                    WriteStr(25, 1080, 170, strWard);//  '등록번호
                    WriteStr(25, 1080, 210, strDeptName);//  '좐자 성명
                    WriteStr(25, 1080, 250, strDrName);//  '나이/성별

                    WriteStr(25, 800, 2100, "보고일 : " + VB.Left(strJDate, 4) + "년 " + VB.Format(VB.Val(VB.Mid(strJDate, 6, 2)), "#0") + "월 " + VB.Format(VB.Val(VB.Right(strJDate, 2)), "#0") + "일");
                    WriteStr(25, 800, 2130, "보고자 : 진단검사의학전문의 " + strRDRName);
                    WriteStr(25, 800, 2160, "         전문의 번호( " + strRDrBunho + " ) ");
                }
                #endregion

                lngLine += 1;

                WriteStr(25, 800, 2100, "보고일 : " + VB.Left(strJDate, 4) + "년 " + VB.Format(VB.Val(VB.Mid(strJDate, 6, 2)), "#0") + "월 " + VB.Format(VB.Val(VB.Right(strJDate, 2)), "#0") + "일");
                WriteStr(25, 800, 2130, "보고자 : 진단검사의학전문의 " + strRDRName);
                WriteStr(25, 800, 2160, "         전문의 번호( " + strRDrBunho + " ) ");

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                dt.Dispose();

                rtnVal = true;

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog("New_Exam_Verify_Result\r\n" + ex.Message, "", pDbCon);
                SaveCvtLog(strPatid, strTREATNO, "종합검증 에러");
                return rtnVal;
            }

            return rtnVal;
        }

        public static void TextPrint(string[] strResult,int nTop, 
            string strPatid, string strDate, string strClinCode, string strPano, string strSName,
            string strAgeSex, string strWard, string strDeptName, string strDrName, string strJDate,
            string strRDRName, string strRDrBunho, ref int lngLine, ref int LNGY)
        {
            if (strResult == null || strResult != null && strResult.Length == 0)
            {
                return;
            }

            for (int K = 0; K < strResult.Length; K++)
            {
                if (string.IsNullOrWhiteSpace(strResult[K]))
                {
                    lngLine += 1;
                }
                else
                {

                    int lastPos = 0;
                    using (Font font = new Font(FontName, 20))
                    {
                        Size PageSize = new Size(1648 - 150, 20);
                        Size strWidth = TextRenderer.MeasureText(strResult[K], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                        if (strWidth.Width < 1100)
                        {
                            WriteStr(20, 150, nTop + (lngLine * 35), strResult[K]);
                            lngLine += 1;
                        }
                        else
                        {
                            for (int l = 0; l < strResult[K].Length + 1;l++)
                            {
                                if (lngLine > 50)
                                {
                                    WriteStr(25, 650, 2150, "(계속)");

                                    TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                                    LNGY = 40;
                                    lngLine = 1;
                                    PageNum += 1;


                                    New_initFormVerify("진단검사의학과 검사 종합검증/판독 보고서");

                                    WriteStr(25, 350, 170, strPano);//  '등록번호
                                    WriteStr(25, 350, 210, strSName);//  '좐자 성명
                                    WriteStr(25, 350, 250, strAgeSex);//  '나이/성별

                                    WriteStr(25, 1080, 170, strWard);//  '등록번호
                                    WriteStr(25, 1080, 210, strDeptName);//  '좐자 성명
                                    WriteStr(25, 1080, 250, strDrName);//  '나이/성별

                                    WriteStr(25, 800, 2100, "보고일 : " + VB.Left(strJDate, 4) + "년 " + VB.Format(VB.Val(VB.Mid(strJDate, 6, 2)), "#0") + "월 " + VB.Format(VB.Val(VB.Right(strJDate, 2)), "#0") + "일");
                                    WriteStr(25, 800, 2130, "보고자 : 진단검사의학전문의 " + strRDRName);
                                    WriteStr(25, 800, 2160, "         전문의 번호( " + strRDrBunho + " ) ");
                                }

                                bool DataOutPut = false;
                                string strText = strResult[K].Substring(lastPos, l);
                                strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                if (strWidth.Width >= 1100)
                                {
                                    lastPos += l;
                                    l = 0;
                                    WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                    lngLine += 1;
                                    DataOutPut = true;
                                }

                                if (DataOutPut && lastPos + l == strResult[K].Length)
                                {
                                    break;
                                }

                                if (DataOutPut == false && (l == strResult[K].Length || lastPos + l == strResult[K].Length))
                                {
                                    WriteStr(20, 150, nTop + (lngLine * 35), strText);
                                    lngLine += 1;
                                    break;
                                }
                            }
                        }
                    }

                    if (lngLine > 50)
                    {
                        WriteStr(25, 650, 2150, "(계속)");

                        TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                        LNGY = 40;
                        lngLine = 1;
                        PageNum += 1;


                        New_initFormVerify("진단검사의학과 검사 종합검증/판독 보고서");

                        WriteStr(25, 350, 170, strPano);//  '등록번호
                        WriteStr(25, 350, 210, strSName);//  '좐자 성명
                        WriteStr(25, 350, 250, strAgeSex);//  '나이/성별

                        WriteStr(25, 1080, 170, strWard);//  '등록번호
                        WriteStr(25, 1080, 210, strDeptName);//  '좐자 성명
                        WriteStr(25, 1080, 250, strDrName);//  '나이/성별

                        WriteStr(25, 800, 2100, "보고일 : " + VB.Left(strJDate, 4) + "년 " + VB.Format(VB.Val(VB.Mid(strJDate, 6, 2)), "#0") + "월 " + VB.Format(VB.Val(VB.Right(strJDate, 2)), "#0") + "일");
                        WriteStr(25, 800, 2130, "보고자 : 진단검사의학전문의 " + strRDRName);
                        WriteStr(25, 800, 2160, "         전문의 번호( " + strRDrBunho + " ) ");
                    }

                    #region 기존
                    //if (strResult[K].Length >= 75)
                    //{
                    //    WriteStr(20, 150, nTop + (lngLine * 35), VB.Left(strResult[K], 75));
                    //    lngLine += 1;

                    //    if(strResult[K].Length -75 > 75)
                    //    {
                    //        WriteStr(20, 150, nTop + (lngLine * 35), VB.Right(strResult[K], (strResult[K]).Length - 75));
                    //        lngLine += 1;
                    //        WriteStr(20, 150, nTop + (lngLine * 35), VB.Right(strResult[K], (strResult[K]).Length - 150));
                    //    }
                    //    else
                    //    {
                    //        WriteStr(20, 150, nTop + (lngLine * 35), VB.Right(strResult[K], (strResult[K]).Length - 75));
                    //    }
                    //}
                    //else
                    //{
                    //    WriteStr(22, 150, nTop + (lngLine * 35), strResult[K]);
                    //}
                    //lngLine += 1;
                    #endregion

                }
            }                       
        }

        public static void New_initFormVerify(string str)
        {
            #region 이미지 그리기
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(38, 290, 80, str);

            WriteStr(23, 150, 170, "등록 번호:");

            WriteStr(23, 150, 210, "환자 성명:");

            WriteStr(23, 150, 250, "나이/성별:");

            WriteStr(23, 900, 170, "진료병동:");

            WriteStr(23, 900, 210, "진 료 과:");

            WriteStr(23, 900, 250, "주치의사:");

            RectLine(50, 150, 1500, 150);

            WriteStr(23, 70, 2200, "* 본 검사실은 2010년 대한진단검사의학회 검사실 신임제도 종합검증 분야의 인증을 필하였습니다.");
            WriteStr(30, 900, 2240, "포항성모병원 진단검사의학과");
            WriteStr(25, 70, 2280, "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.");

            #endregion
        }
        #endregion

        #region 해부병리 결과지

        public static bool New_Exam_Anat(PsmhDb pDbCon, string var, string strPatid, string strDate, string strClinCode, string strClass, string strOutDate, string strDocCode, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            gstrFormcode = strClass.Trim().Equals("I") ? "116" : "007";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            PageNum = 0;

            //string FileName = string.Empty;
            //string strPtno = string.Empty;
            //string strPath = string.Empty;
            //string strPathB = string.Empty;
            //string strPathR = string.Empty;
            strOutDate = string.IsNullOrWhiteSpace(strOutDate) ? ComQuery.CurrentDateTime(pDbCon, "S").Substring(0, 10) : strOutDate;
            #endregion


            try
            {

                #region
                SQL = " SELECT A.ANATNO ,A.PTNO,  C.JUMIN1, C.JUMIN2, C.SNAME,  B.SEX,  B.AGE,  A.DEPTCODE,  A.DRCODE, B.WARD, B.ROOM,";
                SQL += ComNum.VBLF + "     A.REMARK1,  A.RESULT1,  a.resultsabun, a.gbsname,";
                SQL += ComNum.VBLF + "     TO_CHAR(B.RECEIVEDATE,'YYYY-MM-DD') RECEIVEDATE ,";
                SQL += ComNum.VBLF + "     TO_DATE(B.RESULTDATE,'YYYY-MM-DD') RESULTDATE";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.EXAM_ANATMST A,  KOSMOS_OCS.EXAM_SPECMST B    , KOSMOS_PMPA.BAS_PATIENT C";
                SQL += ComNum.VBLF + " WHERE A.PTNO = '" + strPatid + "' ";
                SQL += ComNum.VBLF + "   AND A.BDATE >=TO_DATE('2010-01-01','YYYY-MM-DD') ";
                //SQL += ComNum.VBLF + "   AND A.BDATE >=TO_DATE('2011-01-01','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND A.GBJOB ='V'";//  '결과 완료된것;
                SQL += ComNum.VBLF + "   AND A.RESULTDATE IS NOT NULL ";
                SQL += ComNum.VBLF + "   AND A.PTNO  =B.PANO";
                SQL += ComNum.VBLF + "   AND A.SPECNO = B.SPECNO";
                SQL += ComNum.VBLF + "   AND A.PTNO = C.PANO(+)";

                //'김민원 오동호류마티스내과....
                if (strClinCode.Equals("RA") || strClinCode.Equals("ER"))
                {
                    if (strClass.Equals("O"))
                    {
                        SQL += ComNum.VBLF + "  AND A.DEPTCODE IN ('MD' ,'MR')  ";
                        SQL += ComNum.VBLF + "  AND A.GBIO ='" + strClass + "' ";
                        SQL += ComNum.VBLF + "  AND to_char(A.BDATE,'YYYYMMDD') ='" + strDate + "'  And A.DRCODE IN ( '1107' ,'1125','0901','0902','0903')";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "  AND to_char(A.BDATE,'YYYYMMDD') >='" + strDate + "' ";
                        SQL += ComNum.VBLF + "  AND to_char(A.BDATE,'YYYYMMDD') <='" + strOutDate + "'   ";
                    }

                }
                else
                {
                    if (strClass.Equals("O"))
                    {
                        SQL += ComNum.VBLF + "  AND A.DEPTCODE ='" + strClinCode + "' ";
                        SQL += ComNum.VBLF + "  AND A.GBIO ='" + strClass + "' ";

                        if (strClinCode.Equals("ER"))
                        {
                            SQL += ComNum.VBLF + "  AND to_char(A.BDATE,'YYYYMMDD') >= '" + strDate + "' ";
                            SQL += ComNum.VBLF + "  AND to_char(A.BDATE,'YYYYMMDD') <= '" + DateTime.ParseExact(strDate, "yyyyMMdd", null).AddDays(+1).ToString("yyyyMMdd") + "' ";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "  AND to_char(A.BDATE,'YYYYMMDD') ='" + strDate + "' ";
                        }

                    }
                    else
                    {
                        SQL += ComNum.VBLF + "  AND ( A.GBIO ='" + strClass + "' or A.DeptCode ='ER') ";// '당일입원 등록요청
                        SQL += ComNum.VBLF + "  AND to_char(A.BDATE,'YYYYMMDD') >='" + strDate + "' ";
                        SQL += ComNum.VBLF + "  AND to_char(A.BDATE,'YYYYMMDD') <='" + strOutDate + "'  ";
                        //SQL += ComNum.VBLF + "  AND A.DRCODE NOT IN ( '1107','1125' ,'0901','0902','0903') "; //2020-11-09 기록실 이동춘 팀장님 풀어달라고 함.
                    }
                }

                SQL += ComNum.VBLF + " ORDER BY A.BDATE ";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrEmpty(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }


                if (dt.Rows.Count ==0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                SaveCvtLog(strPatid, strTREATNO, "해부병리");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PageNum += 1;

                    //조직
                    if (dt.Rows[i]["ANATNO"].ToString().Trim().Substring(0, 1).Equals("S") || dt.Rows[i]["ANATNO"].ToString().Trim().Substring(0, 2).Equals("OS") || dt.Rows[i]["ANATNO"].ToString().Trim().Substring(0, 2).Equals("IH"))
                    {
                        if (New_Exam_Anat_Result_S(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), Trim(dt.Rows[i]["ANATNO"].ToString()), strTREATNO) == false)
                        {
                            return rtnVal;
                        }
                    }
                    //해부병리 결과지('2018 - 09 - 06)
                    else if (dt.Rows[i]["ANATNO"].ToString().Trim().Substring(0, 1).Equals("C") || dt.Rows[i]["ANATNO"].ToString().Trim().Substring(0, 1).Equals("A"))
                    {
                        if (New_Exam_Anat_Result_C(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), Trim(dt.Rows[i]["ANATNO"].ToString()), strTREATNO) == false)
                        {
                            return rtnVal;
                        }
                    }

                }

                dt.Dispose();
                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n해부병리 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "해부병리 에러");
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        /// <summary>
        /// 해부병리 결과지
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="strArgAnato"></param>
        /// <returns></returns>
        public static bool New_Exam_Anat_Result_S(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string ArgAnatno, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            //int LNGY  = 40;
            int lngLine  = 0;
            //string FileName  = string.Empty;
            string[] strResult;
            //string[] strResult1;
            string strTResult  = string.Empty;
            //int PAGENO  = 0;

            //string strDrName  = string.Empty;

            //string nPage  = string.Empty;

            string sPtno  = string.Empty;
            string sAge  = string.Empty;
            string sSex  = string.Empty;
            string sWard = string.Empty;
            string sRoom = string.Empty;
            string sDept  = string.Empty;
            string sDr  = string.Empty;
            string sSpecName  = string.Empty;
            string sFDate  = string.Empty;
            string sRDate = string.Empty;
            string sSname  = string.Empty;
            string sAnatNo = string.Empty;
            string sJumin1  = string.Empty;
            string sJumin2  = string.Empty;
            string sDeptCode = string.Empty;
            string strRDRName  = string.Empty;
            string strGBSName  = string.Empty;
            string strJDate  = string.Empty;

            FontName = "굴림체";

            #endregion

            if (string.IsNullOrWhiteSpace(ArgAnatno))
                return rtnVal;

            try
            {
                #region 쿼리
                SQL = " SELECT A.ANATNO ,  A.PTNO,  C.JUMIN1, C.JUMIN2, C.SNAME,  B.SEX,  B.AGE,  A.DEPTCODE,  A.DRCODE, B.WARD, B.ROOM,";
                SQL += ComNum.VBLF + "      A.REMARK1,  A.RESULT1, a.Result2, a.resultsabun, a.gbsname, A.MASTERCODE, ";
                SQL += ComNum.VBLF + "      TO_CHAR(B.RECEIVEDATE,'YYYY-MM-DD') RECEIVEDATE ,";
                SQL += ComNum.VBLF + "      TO_CHAR(a.JDATE,'YYYY-MM-DD') JDATE ,";
                SQL += ComNum.VBLF + "      TO_CHAR(B.RESULTDATE,'YYYY-MM-DD') RESULTDATE ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.EXAM_ANATMST A,  KOSMOS_OCS.EXAM_SPECMST B    , KOSMOS_PMPA.BAS_PATIENT C";
                SQL += ComNum.VBLF + " WHERE A.ANATNO ='" + ArgAnatno + "'";
                SQL += ComNum.VBLF + "   AND A.PTNO  =B.PANO";
                SQL += ComNum.VBLF + "   AND A.SPECNO = B.SPECNO";
                SQL += ComNum.VBLF + "   AND A.PTNO = C.PANO(+)";
                #endregion 

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }


                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                New_initFormAnat_S("병리 조직 검사 보고서");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sAnatNo = dt.Rows[i]["AnatNo"].ToString().Trim();
                    sPtno = dt.Rows[i]["Ptno"].ToString().Trim();
                    sDeptCode = dt.Rows[i]["deptcode"].ToString().Trim();
                    sJumin1 = dt.Rows[i]["Jumin1"].ToString().Trim();
                    sJumin2 = VB.Left(dt.Rows[i]["Jumin2"].ToString().Trim(), 1) + "******";


                    sSname = Trim(dt.Rows[i]["SName"].ToString().Trim());
                    sAge = dt.Rows[i]["Age"].ToString().Trim();
                    sSex = Trim(dt.Rows[i]["Sex"].ToString()).Equals("M") ? "남" : "여";

                    sWard = dt.Rows[i]["Ward"].ToString().Trim();
                    sRoom = dt.Rows[i]["Room"].ToString().Trim();
                    sDept = clsVbfunc.GetBASClinicDeptNameK(pDbCon, sDeptCode);
                    sDr = clsVbfunc.GetBASDoctorName(pDbCon, dt.Rows[i]["DRCode"].ToString().Trim());

                    switch(dt.Rows[i]["ResultSabun"].ToString().Trim())
                    {
                        case "42619":
                        case "39515":
                        case "36442":
                        case "47114":
                        case "47123":
                            strRDRName = "";
                            break;
                        default:
                            strRDRName = clsVbfunc.GetInSaName(pDbCon, VB.Val(Trim(dt.Rows[i]["ResultSabun"].ToString().Trim())).ToString());
                            break;
                    }

                    strGBSName = dt.Rows[i]["GBSName"].ToString().Trim();

                    if (sDeptCode.Equals("TO"))
                    {
                        sDr = "종합검진";
                    }

                    sFDate   = dt.Rows[i]["ReceiveDate"].ToString().Trim();
                    strJDate = dt.Rows[i]["jdate"].ToString().Trim();
                    sRDate   = dt.Rows[i]["ResultDate"].ToString().Trim();


                    if (VB.Left(sAnatNo, 2).Equals("OS"))
                    {
                        WriteStr(VB.Left(sAnatNo, 2) + " - " + VB.Mid(sAnatNo, 3, 2) + " - " + VB.Right(sAnatNo, 5), 25, 350, 195);
                    }
                    else if(VB.Left(sAnatNo, 2).Equals("SW"))
                    {
                        WriteStr(VB.Left(sAnatNo, 2) + " - " + VB.Mid(sAnatNo, 3, 2) + " - " + VB.Right(sAnatNo, 5), 25, 350, 195);
                    }
                    else if (VB.Left(sAnatNo, 2).Equals("IH"))
                    {
                        WriteStr(VB.Left(sAnatNo, 2) + " - " + VB.Mid(sAnatNo, 3, 2) + " - " + VB.Right(sAnatNo, 5), 25, 350, 195);
                    }
                    else
                    {
                        WriteStr(VB.Left(sAnatNo, 1) + " - " + VB.Mid(sAnatNo, 2, 2) + " - " + VB.Right(sAnatNo, 5), 25, 350, 195);
                    }


                    WriteStr(25, 340, 245, sPtno);//  '등록번호
                    WriteStr(25, 340, 295, sSname);// '성명
                    WriteStr(25, 340, 345, sDept);// '진료과
                    WriteStr(25, 850, 245, sJumin1 + "-" + sJumin2);// '주민번호
                    WriteStr(25, 850, 295, sSex);// '성별
                    WriteStr(25, 850, 345, Trim(sWard) + "  " + sRoom + VB.Space(13 - ((Trim(sWard)).Length + (sRoom).Length)));// '병동
                    WriteStr(25, 1400, 295, sAge);// '연령
                    WriteStr(25, 1400, 345, sDr);// '의뢰의사
                    WriteStr(25, 350, 1950, sFDate);// '검사채취일


                    WriteStr(25, 350, 2010, strJDate);// '검사접수일
                    WriteStr(25, 350, 2060, sRDate);// '결과보고일


                    WriteStr(25, 350, 2110, strGBSName);// '결과입력자


                    WriteStr(strRDRName, 25, 1350, 2110); //'판독의사


                    strTResult = READ_MasterName(pDbCon, dt.Rows[i]["MASTERCODE"].ToString().Trim())
                        + ComNum.VBLF + ComNum.VBLF + dt.Rows[i]["RESULT1"].ToString().Trim() + ComNum.VBLF + ComNum.VBLF + dt.Rows[i]["RESULT2"].ToString().Trim();
                    strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                    strResult = strTResult.Split('\n');

                    for (int k = 0; k < strResult.Length; k++)
                    {
                        if (string.IsNullOrWhiteSpace(strResult[k]))
                        {
                            #region New Page
                            if (lngLine > 37)
                            {
                                WriteStr(25, 850, 2010, "(계속)");


                                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                                //LNGY = 40;
                                lngLine = 1;
                                PageNum += 1;

                                New_initFormAnat_S("병리 조직 검사 보고서");

                                #region 내용
                                if (VB.Left(sAnatNo, 2).Equals("OS"))
                                {
                                    WriteStr(VB.Left(sAnatNo, 2) + " - " + VB.Mid(sAnatNo, 3, 2) + " - " + VB.Right(sAnatNo, 5), 25, 350, 195);
                                }
                                else if (VB.Left(sAnatNo, 2).Equals("SW"))
                                {
                                    WriteStr(VB.Left(sAnatNo, 2) + " - " + VB.Mid(sAnatNo, 3, 2) + " - " + VB.Right(sAnatNo, 5), 25, 350, 195);
                                }
                                else if (VB.Left(sAnatNo, 2).Equals("IH"))
                                {
                                    WriteStr(VB.Left(sAnatNo, 2) + " - " + VB.Mid(sAnatNo, 3, 2) + " - " + VB.Right(sAnatNo, 5), 25, 350, 195);
                                }
                                else
                                {
                                    WriteStr(VB.Left(sAnatNo, 1) + " - " + VB.Mid(sAnatNo, 2, 2) + " - " + VB.Right(sAnatNo, 5), 25, 350, 195);
                                }


                                WriteStr(25, 340, 245, sPtno);//  '등록번호
                                WriteStr(25, 340, 295, sSname);// '성명
                                WriteStr(25, 340, 345, sDept);// '진료과
                                WriteStr(25, 850, 245, sJumin1 + "-" + sJumin2);// '주민번호
                                WriteStr(25, 850, 295, sSex);// '성별
                                WriteStr(25, 850, 345, Trim(sWard) + "  " + sRoom + VB.Space(13 - ((Trim(sWard)).Length + (sRoom).Length)));// '병동
                                WriteStr(25, 1400, 295, sAge);// '연령
                                WriteStr(25, 1400, 345, sDr);// '의뢰의사
                                WriteStr(25, 350, 1950, sFDate);// '검사채취일


                                WriteStr(25, 350, 2010, strJDate);// '검사접수일
                                WriteStr(25, 350, 2060, sRDate);// '결과보고일


                                WriteStr(25, 350, 2110, strGBSName);// '결과입력자


                                WriteStr(strRDRName, 25, 1350, 2110); //'판독의사
                                #endregion
                            }
                            #endregion
                            lngLine += 1;
                        }
                        else
                        {
                            int strByte = Encoding.Default.GetBytes(strResult[k]).Length;
                            for (double b = 1; b < Math.Truncate((double) strByte / 75  + 0.9) + 1; b++)
                            {
                                WriteStr(20, 190, 550 + (lngLine * 35), VB.Mid(strResult[k],  1 + 75 * (int) (b - 1), 75));

                                lngLine += 1;

                                #region New Page
                                if (lngLine > 37)
                                {
                                    WriteStr(25, 850, 2010, "(계속)");


                                    TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                                    //LNGY = 40;
                                    lngLine = 1;
                                    PageNum += 1;

                                    New_initFormAnat_S("병리 조직 검사 보고서");

                                    #region 내용
                                    if (VB.Left(sAnatNo, 2).Equals("OS"))
                                    {
                                        WriteStr(VB.Left(sAnatNo, 2) + " - " + VB.Mid(sAnatNo, 3, 2) + " - " + VB.Right(sAnatNo, 5), 25, 350, 195);
                                    }
                                    else if (VB.Left(sAnatNo, 2).Equals("SW"))
                                    {
                                        WriteStr(VB.Left(sAnatNo, 2) + " - " + VB.Mid(sAnatNo, 3, 2) + " - " + VB.Right(sAnatNo, 5), 25, 350, 195);
                                    }
                                    else if (VB.Left(sAnatNo, 2).Equals("IH"))
                                    {
                                        WriteStr(VB.Left(sAnatNo, 2) + " - " + VB.Mid(sAnatNo, 3, 2) + " - " + VB.Right(sAnatNo, 5), 25, 350, 195);
                                    }
                                    else
                                    {
                                        WriteStr(VB.Left(sAnatNo, 1) + " - " + VB.Mid(sAnatNo, 2, 2) + " - " + VB.Right(sAnatNo, 5), 25, 350, 195);
                                    }


                                    WriteStr(25, 340, 245, sPtno);//  '등록번호
                                    WriteStr(25, 340, 295, sSname);// '성명
                                    WriteStr(25, 340, 345, sDept);// '진료과
                                    WriteStr(25, 850, 245, sJumin1 + "-" + sJumin2);// '주민번호
                                    WriteStr(25, 850, 295, sSex);// '성별
                                    WriteStr(25, 850, 345, Trim(sWard) + "  " + sRoom + VB.Space(13 - ((Trim(sWard)).Length + (sRoom).Length)));// '병동
                                    WriteStr(25, 1400, 295, sAge);// '연령
                                    WriteStr(25, 1400, 345, sDr);// '의뢰의사
                                    WriteStr(25, 350, 1950, sFDate);// '검사채취일


                                    WriteStr(25, 350, 2010, strJDate);// '검사접수일
                                    WriteStr(25, 350, 2060, sRDate);// '결과보고일


                                    WriteStr(25, 350, 2110, strGBSName);// '결과입력자


                                    WriteStr(strRDRName, 25, 1350, 2110); //'판독의사
                                    #endregion
                                }
                                #endregion
                            }
                        }
                    }
                }

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");
                rtnVal = true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n내시경 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "해부병리 에러");
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// BAS_SUN에서 해당 자료를 READ
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgCode"></param>
        /// <returns></returns>
        public static string READ_MasterName(PsmhDb pDbCon, string ArgCode)
        {
            if (string.IsNullOrWhiteSpace(ArgCode))
                return string.Empty;

            string rtnVal = string.Empty;
            string SQL = "SELECT ExamName FROM KOSMOS_OCS.Exam_Master            ";
            SQL += ComNum.VBLF + "WHERE MasterCode = '" + ArgCode + "' ";

            OracleDataReader reader = null;

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();

            return rtnVal;
        }

        /// <summary>
        /// 해부병리 결과지
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="strArgAnato"></param>
        /// <returns></returns>
        public static bool New_Exam_Anat_Result_C(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string ArgAnatno, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            OracleDataReader reader = null;

            //int LNGY = 0;
            int lngLine = 0;
            string FileName = string.Empty;
            string[] strResult;
            //string[] strResult1;
            string strTResult = string.Empty;
            //int PAGENO = 0;

            string strDrName = string.Empty;

            string nPage = string.Empty;

            string sPtno = string.Empty;
            string sAge = string.Empty;
            string sSex = string.Empty;
            string sWard = string.Empty;
            string sRoom = string.Empty;
            string sDept = string.Empty;
            string sDr = string.Empty;
            string sSpecName = string.Empty;
            string sFDate = string.Empty;
            string sRDate = string.Empty;
            string sSname = string.Empty;
            string sAnatNo = string.Empty;
            string sJumin1 = string.Empty;
            string sJumin2 = string.Empty;
            string sDeptCode = string.Empty;
            string strRDRName = string.Empty;
            string strGBSName = string.Empty;
            string strJDate = string.Empty;
            string strSpecNo = string.Empty;

            FontName = "굴림체";

            #endregion

            if (string.IsNullOrWhiteSpace(ArgAnatno))
                return rtnVal;


            #region 쿼리
            SQL = " SELECT A.ANATNO ,  A.PTNO,  C.JUMIN1, C.JUMIN2, C.SNAME,  B.SEX,  B.AGE,  A.DEPTCODE,  A.DRCODE, B.WARD, B.ROOM,";
            SQL += ComNum.VBLF + "      A.REMARK1,  A.RESULT1,  a.resultsabun, a.gbsname, A.MASTERCODE, A.SPECNO, ";
            SQL += ComNum.VBLF + "      TO_CHAR(B.RECEIVEDATE,'YYYY-MM-DD') RECEIVEDATE ,";
            SQL += ComNum.VBLF + "      TO_CHAR(a.JDATE,'YYYY-MM-DD') JDATE ,";
            SQL += ComNum.VBLF + "      TO_CHAR(B.RESULTDATE,'YYYY-MM-DD') RESULTDATE, b.speccode ";
            SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.EXAM_ANATMST A,  KOSMOS_OCS.EXAM_SPECMST B    , KOSMOS_PMPA.BAS_PATIENT C";
            SQL += ComNum.VBLF + " WHERE A.ANATNO ='" + ArgAnatno + "'";
            SQL += ComNum.VBLF + "   AND A.PTNO   = B.PANO";
            SQL += ComNum.VBLF + "   AND A.SPECNO = B.SPECNO";
            SQL += ComNum.VBLF + "   AND A.PTNO   = C.PANO(+)";
            #endregion

            try
            {

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }


                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                New_initFormAnat_C("세포병리 보고서");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sAnatNo = dt.Rows[i]["AnatNo"].ToString().Trim();
                    sPtno = dt.Rows[i]["Ptno"].ToString().Trim();
                    sDeptCode = dt.Rows[i]["deptcode"].ToString().Trim();
                    sJumin1 = dt.Rows[i]["Jumin1"].ToString().Trim();
                    sJumin2 = VB.Left(dt.Rows[i]["Jumin2"].ToString().Trim(), 1) + "******";


                    sSname = Trim(dt.Rows[i]["SName"].ToString().Trim());
                    sAge = dt.Rows[i]["Age"].ToString().Trim();
                    sSex = Trim(dt.Rows[i]["Sex"].ToString()).Equals("M") ? "남" : "여";

                    sWard = dt.Rows[i]["Ward"].ToString().Trim();
                    sRoom = dt.Rows[i]["Room"].ToString().Trim();
                    sDept = clsVbfunc.GetBASClinicDeptNameK(pDbCon, sDeptCode);
                    sDr = clsVbfunc.GetBASDoctorName(pDbCon, dt.Rows[i]["DRCode"].ToString().Trim());

                    switch (dt.Rows[i]["ResultSabun"].ToString().Trim())
                    {
                        case "42619":
                        case "39515":
                        case "36442":
                        case "47114":
                        case "47123":
                            strRDRName = "";
                            break;
                        default:
                            strRDRName = clsVbfunc.GetInSaName(pDbCon, VB.Val(Trim(dt.Rows[i]["ResultSabun"].ToString().Trim())).ToString());
                            break;
                    }

                    strGBSName = dt.Rows[i]["GBSName"].ToString().Trim();
                    strSpecNo = dt.Rows[i]["specno"].ToString().Trim();

                    #region 서브 쿼리
                    SQL = " SELECT Name FROM KOSMOS_OCS.EXAM_SPECODE WHERE Gubun = '14' AND Code = '" + dt.Rows[0]["SpecCode"].ToString().Trim() + "'";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        return rtnVal;
                    }

                    if (reader.HasRows && reader.Read())
                    {
                        sSpecName = reader.GetValue(0).ToString().Trim();
                    }
                    else
                    {
                        reader.Dispose();

                        SQL = " SELECT A.EXAMNAME FROM KOSMOS_OCS.EXAM_MASTER A,  KOSMOS_OCS.EXAM_ORDER B";
                        SQL += ComNum.VBLF + " WHERE SPECNO = '" + strSpecNo + "'";
                        SQL += ComNum.VBLF + " AND PANO = '" + sPtno + "'";
                        SQL += ComNum.VBLF + " AND B.MASTERCODE = A.MASTERCODE";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            return rtnVal;
                        }

                        if (reader.HasRows && reader.Read())
                        {
                            sSpecName = reader.GetValue(0).ToString().Trim();
                        }
                    }

                    reader.Dispose();

                    #endregion

                    if (sDeptCode.Equals("TO"))
                    {
                        sDr = "종합검진";
                    }

                    sFDate = dt.Rows[i]["ReceiveDate"].ToString().Trim();
                    strJDate = dt.Rows[i]["jdate"].ToString().Trim();
                    sRDate = dt.Rows[i]["ResultDate"].ToString().Trim();

                    if (sAnatNo.Length == 8)
                    {
                        WriteStr(25, 80, 180, VB.Left(sAnatNo, 3) + "-" + VB.Right(sAnatNo, 5));//'병리번호
                    }
                    else if (sAnatNo.Length == 9)
                    {
                        WriteStr(25, 80, 180, VB.Left(sAnatNo, 4) + "-" + VB.Right(sAnatNo, 5));//'병리번호
                    }


                    WriteStr(25, 340, 244, sPtno);//'등록번호
                    WriteStr(25, 860, 244, sSname);// '성명
                    WriteStr(25, 1400, 244, sAge + "/" + sSex);// '나이/성별
                    WriteStr(25, 340, 305, Trim(sWard) + "  " + sRoom + VB.Space(13 - ((Trim(sWard)).Length + (sRoom).Length)));// '병동
                    WriteStr(25, 890, 305, sDeptCode);// 'sDept '진료과
                    WriteStr(25, 1380, 305, sDr);// '의뢰의사

                    WriteStr(25, 350, 1990, VB.Left(sSpecName, 26));// '검체
                    WriteStr(25, 350, 2060, strGBSName);// '결과입력자

                    WriteStr(25, 350, 2130, strRDRName);//  '판독의사
                    WriteStr(25, 1200, 1990, sFDate);//     '검사채취일
                    WriteStr(25, 1200, 2060, strJDate);//   '검사접수일
                    WriteStr(25, 1200, 2130, sRDate);//     '결과보고일





                    strTResult = "▶" + READ_MasterName(pDbCon, dt.Rows[i]["MASTERCODE"].ToString().Trim())
                        + ComNum.VBLF + dt.Rows[i]["RESULT1"].ToString().Trim();
                    strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                    strResult = strTResult.Split('\n');

                    for (int k = 0; k < strResult.Length; k++)
                    {
                        if (string.IsNullOrWhiteSpace(strResult[k]))
                        {
                            lngLine += 1;
                        }
                        else
                        {
                            int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                            if (strByte >= 325)
                            {
                                WriteStr(20, 70, 550 + (lngLine * 35), VB.Left(strResult[k], 65));
                                lngLine += 1;

                                if(strResult[k].Length - 65 > 65)
                                {
                                    WriteStr(20, 70, 550 + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));
                                    lngLine += 1;
                                    WriteStr(20, 70, 550 + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 130));

                                }
                                else
                                {
                                    WriteStr(20, 70, 550 + (lngLine * 35), VB.Right(strResult[k], (strResult[k]).Length - 65));

                                }
                            }
                            else
                            {
                                WriteStr(22, 70, 550 + (lngLine * 35), strResult[k]);
                            }

                            lngLine += 1;
                        }

                        #region New Page
                        if (lngLine > 37)
                        {
                            WriteStr(25, 750, 2010, "(계속)");


                            TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                            //LNGY = 40;
                            lngLine = 1;
                            PageNum += 1;

                            New_initFormAnat_C("세포병리 보고서");

                            WriteStr(VB.Left(sAnatNo, 3) + "-" + VB.Right(sAnatNo, 5), 25, 80, 180);

                            WriteStr(25, 340, 244, sPtno);//'등록번호
                            WriteStr(25, 860, 244, sSname);// '성명
                            WriteStr(25, 1400, 244, sAge + "/" + sSex);// '나이/성별
                            WriteStr(25, 340, 305, Trim(sWard) + "  " + sRoom + VB.Space(13 - ((Trim(sWard)).Length + (sRoom).Length)));// '병동
                            WriteStr(25, 890, 305, sDeptCode);// 'sDept '진료과
                            WriteStr(25, 1380, 305, sDr);// '의뢰의사

                            WriteStr(25, 350, 1990, VB.Left(sSpecName, 26));// '검체
                            WriteStr(25, 350, 2060, strGBSName);// '결과입력자
                            WriteStr(25, 350, 2130, strRDRName);//  '판독의사
                            WriteStr(25, 1200, 1990, sFDate);//     '검사채취일
                            WriteStr(25, 1200, 2060, strJDate);//   '검사접수일
                            WriteStr(25, 1200, 2130, sRDate);//     '결과보고일
                        }
                        #endregion
                    }
                }

                dt.Dispose();


                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                rtnVal = true;
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n해부병리 에러", SQL, clsDB.DbCon);
                SaveCvtLog(strPatid, strTREATNO, "해부병리 에러");
            }

            return rtnVal;
        }

        public static void New_initFormAnat_S(string str)
        {
            #region 이미지 그리기
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(str, 50, 500, 80);

            WriteStr("병리번호:", 25, 150, 195);

            WriteStr("등록번호: ", 25, 150, 245);

            WriteStr("성    명:", 25, 150, 295);
            WriteStr("진 료 과:", 25, 150, 345);

            WriteStr("주민번호 : ", 25, 640, 245);

            WriteStr("성    별 : ", 25, 640, 295);
            WriteStr("병    동 : ", 25, 640, 345);


            WriteStr("연    령 : ", 25, 1210, 295);


            WriteStr("의뢰의사: ", 25, 1210, 345);

            WriteStr("-----------------------------------------------------------------------------------", 25, 120, 380);

            WriteStr("검체채취일: ", 25, 130, 1950);
            WriteStr("검사접수일: ", 25, 130, 2010);

            WriteStr("결과보고일: ", 25, 130, 2060);
            WriteStr("결과입력자: ", 25, 130, 2110);

            WriteStr("판독의사: ", 25, 1170, 2110);


            WriteStr("-----------------------------------------------------------------------------------", 25, 120, 2140);
            WriteStr("우(37661) 경북 포항시 남구 대잠동길 17", 25, 140, 2180);
            WriteStr("포항성모병원 병리과   ☎:직) (054)260-8265  대) (054)272-0151 ", 25, 140, 2220);
            WriteStr("* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.", 25, 140, 2260);

            #endregion
        }


        public static void New_initFormAnat_C(string str)
        {
            #region 이미지 그리기
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(str, 50, 550, 80);

            WriteStr("등록번호", 23, 100, 244);
            WriteStr("병    실", 23, 100, 305);
            WriteStr("이    름", 23, 620, 244);
            WriteStr("진 료 과", 23, 620, 305);
            WriteStr("나이/성별", 23, 1120, 244);
            WriteStr("의뢰의사", 23, 1120, 305);

            RectLine(65, 235, 1500, 1940);
            vline(295, 235, 355);
            vline(560, 235, 355);
            vline(810, 235, 355);
            vline(1060, 235, 355);
            vline(1330, 235, 355);
            hline2(65, 295, 1565);

            hline2(65, 355, 1565);
            hline2(65, 375, 1565);

            hline2(65, 1970, 1565);
            vline(315, 1970, 2175);
            vline(880, 1970, 2175);
            vline(1140, 1970, 2175);

            hline2(65, 2040, 1565);
            hline2(65, 2110, 1565);

            WriteStr("검    체", 23, 120, 1990);
            WriteStr("일 력 자", 23, 120, 2060);
            WriteStr("판독의사", 23, 120, 2130);

            WriteStr("검체채취일", 23, 930, 1990);
            WriteStr("검사접수일", 23, 930, 2060);
            WriteStr("결과보고일", 23, 930, 2130);

            WriteStr("우(37661) 경북 포항시 남구 대잠동길 17", 23, 80, 2180);
            WriteStr("포항성모병원 병리과   ☎:직) (054)260-8265  대) (054)272-0151 ", 25, 70, 2220);
            WriteStr("* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.", 25, 70, 2260);

            #endregion
        }
        #endregion

        #region PTF 검사결과
        public static bool New_PFT_Select(PsmhDb pDbCon, string var, string strPatid, string strDate, string strClinCode, string strClass, string strOutDate, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            Ftpedt ftpedt = null;

            //126(각종기능검사결과지), 006(??)
            gstrFormcode = strClass.Trim().Equals("I") ? "126" : "006";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            PageNum = 0;
            #endregion

            try
            {
                CreateSaveFolder();

                #region 쿼리
                SQL = " SELECT PTNO,  IMAGE, GbFTP ,FILEPATH, TO_CHAR(RDATE,'YYYYMMDD') RDATE ";
                SQL += ComNum.VBLF + "   FROM  KOSMOS_OCS.ETC_JUPMST ";
                SQL += ComNum.VBLF + "  WHERE PTNO ='" + strPatid + "'";


                if(strClass.Equals("I"))
                {
                    SQL += ComNum.VBLF + "  AND GBIO ='I'";
                    SQL += ComNum.VBLF + "  AND BDate >=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  AND BDate <=TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                }
                else
                {

                    SQL += ComNum.VBLF + "  AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')  ";
                    SQL += ComNum.VBLF + "  AND GBIO ='O'";
                    if(strClinCode.Equals("RA") || strClinCode.Equals("MR"))
                    {
                        SQL += ComNum.VBLF + "  AND DEPTCODE IN ( 'MD','MR')  AND DRCODE in ('1107','1125','0901','0902','0903') ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "  AND DEPTCODE = '" + strClinCode + "' ";
                    }
                }

                SQL += ComNum.VBLF + "  AND GBIO = '" + strClass + "' ";
                SQL += ComNum.VBLF + "  AND GUBUN ='4'";// 'PFT
                SQL += ComNum.VBLF + "  AND ( IMAGE IS NOT NULL OR FILEPATH IS NOT NULL) ";
                SQL += ComNum.VBLF + "  AND DEPTCODE NOT IN ('HR' ,'TO') ";
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    return rtnVal;
                }

                SaveCvtLog(strPatid, strTREATNO, "PFT");

                rtnVal = UploadPTF(pDbCon, strPatid, strDate, strClinCode, reader, strTREATNO);
                reader.Dispose();

            }
            catch (Exception ex)
            {
                if (ftpedt != null)
                {
                    ftpedt.Dispose();
                }

                SaveCvtLog(strPatid, strTREATNO, "PFT 에러");
                //ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message + "\r\nPTF 검사결과 업로드중 에러", "", pDbCon);
            }

            return rtnVal;
        }

        public static bool UploadPTF(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, OracleDataReader reader, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            Ftpedt ftpedt = null;
            PageNum = 0;
            #endregion

            try
            {
                ftpedt = new Ftpedt();
                if (ftpedt.FtpConnetBatch("192.168.100.31", "oracle", ftpedt.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle")) == false)
                {
                    ftpedt.FtpDisConnetBatch();
                    ftpedt.Dispose();
                    ComFunc.MsgBox("서버 접속 실패!");
                    return rtnVal;
                }

                while(reader.Read())
                {
                    string strPtno = reader.GetValue(0).ToString().Trim();
                    string strPath = @"C:\PSMHEXE\IMGCVT\PostScan\" + strPtno + ".jpg";
                    string strPathR = "/data/ocs_etc/" + reader.GetValue(4).ToString().Trim();
                    string strGBFTP = reader.GetValue(2).ToString().Trim();
                    string strSPath = reader.GetValue(3).ToString().Trim();

                    PageNum += 1;

                    if (strGBFTP.Equals("Y"))
                    {
                        //if (ftpedt.FtpDownloadBatch(strPath, strPtno + ".jpg", strPathR) == false)
                        if (ftpedt.FtpDownloadBatch2(strPath, strSPath, strPathR) == false)
                        {
                            ComFunc.MsgBox("파일 다운로드 실패!");
                            SaveCvtLog(strPatid, strTREATNO, "PFT 에러");
                        }
                        else
                        {
                            if (File.Exists(strPath))
                            {
                                mBitmap = new Bitmap(strPath);
                            }
                        }
                    }
                    else
                    {
                        strPathR = @"C:\PSMHEXE\IMGCVT\PostScan\" + strPtno + "resize.jpg";
                        ResizeJpg(strPathR, 600, 800);
                        if (File.Exists(strPathR))
                        {
                            mBitmap = new Bitmap(strPathR);
                        }
                    }

                    ////TifSave(@"C:\PSMHEXE\IMGCVT\" + strPtno  + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif", true);
                    SaveJpeg(@"C:\PSMHEXE\IMGCVT\" + strPtno + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif", mBitmap, 50);

                    File.Delete(strPath);
                    if (strPathR.IndexOf("resize.jpg") != -1)
                    {
                        File.Delete(strPathR);
                    }
                }

                rtnVal = true;
            }
            catch(Exception ex)
            {
                if (ftpedt != null)
                {
                    ftpedt.Dispose();
                }
                SaveCvtLog(strPatid, strTREATNO, "PFT 에러");
                clsDB.SaveSqlErrLog(ex.Message + "\r\nPTF 업로드중 오류발생", "", pDbCon);
            }

            return rtnVal;
        }

        #endregion

        #region ResizeJpg
        public static void ResizeJpg(string path, int nWidth, int nHeight)
        {
            using (var result = new Bitmap(nWidth, nHeight))
            {
                using (var input = new Bitmap(path))
                {
                    using (Graphics g = Graphics.FromImage((System.Drawing.Image)result))
                    {
                        g.DrawImage(input, 0, 0, nWidth, nHeight);
                    }
                }

                var ici = ImageCodecInfo.GetImageEncoders().FirstOrDefault(ie => ie.MimeType == "image/jpeg");
                var eps = new EncoderParameters(1);
                eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 70L);
                result.Save(path, ici, eps);
            }
        }
        #endregion

        #region Counsult
        /// <summary>
        /// 
        /// </summary>
        /// <param name="var"></param>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="strOutDate"></param>
        /// <param name="strDocCode"></param>
        /// <param name="strTREATNO"></param>
        /// <returns></returns>
        public static bool New_Consult_Select(PsmhDb pDbCon, string var, string strPatid, string strDate, string strClinCode, string strClass, string strOutDate, string strDocCode, string strTREATNO)
        {
            //외래는 변환 않함.
            if (strClass.Equals("O"))
                return false;

            #region 변수
            bool rtnVal = false;

            //109(협의진단), 002(외래 진료기록지)
            gstrFormcode = strClass.Trim().Equals("I") ? "109" : "002";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            PageNum = 0;
            #endregion

            try
            {

                #region 쿼리
                SQL = " SELECT   A.PTNO, ";
                SQL += ComNum.VBLF + "                A.FRDEPTCODE, A.FRDRCODE , B.DRNAME FrDrName,  A.FRREMARK , TO_CHAR(A.SDATE,'YYYY-MM-DD HH24:MI') SDATE, ";
                SQL += ComNum.VBLF + "                A.TODEPTCODE, A.TODRCODE , C.DRNAME TODrName, A.TOREMARK,   TO_CHAR(A.EDATE,'YYYY-MM-DD HH24:MI') EDATE, ";
                SQL += ComNum.VBLF + "                A.SNAME, A.AGE, A.SEX , A.ROOMCODE , a.gbemsms ";
                SQL += ComNum.VBLF + " FROM      KOSMOS_OCS.OCS_ITRANSFER A,  KOSMOS_PMPA.BAS_DOCTOR B,   KOSMOS_PMPA.BAS_DOCTOR C";
                SQL += ComNum.VBLF + " WHERE A.PTNO ='" + strPatid + "' ";
                SQL += ComNum.VBLF + " AND A.SDATE  >=TO_DATE('2008-07-01','YYYY-MM-DD')";//     '2008년 7월 1일 부터 자동변환되도록처리(자동변환 정보을 2008년8월 1일 부터 저장됨)
                SQL += ComNum.VBLF + " AND A.BDATE  >=TO_DATE('" + strDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + " AND A.BDATE <= TO_DATE('" + strOutDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + " AND A.GBCONFIRM ='*'";
                SQL += ComNum.VBLF + " AND (A.GBDEL <>'*' OR A.GBDEL IS NULL ) ";// '2010-10-20 윤;
                SQL += ComNum.VBLF + " AND ( A.GBSEND IS NULL OR A.GBSEND =' ' )";
                SQL += ComNum.VBLF + " AND A.FRDRCODE = B.DRCODE(+)";
                SQL += ComNum.VBLF + " AND A.TODRCODE = C.DRCODE(+)";
                SQL += ComNum.VBLF + " ORDER BY TODEPTCODE, TODRCODE,  BDATE ASC ";
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                SaveCvtLog(strPatid, strTREATNO, "Consult");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PageNum += 1;

                    //consult
                    New_Consult(pDbCon, Trim(strPatid), Trim(strDate), Trim(strClinCode), Trim(strClass), dt.Rows[i]["PTNO"].ToString().Trim(),
                                             dt.Rows[i]["FrDeptCode"].ToString().Trim(), dt.Rows[i]["frDrCode"].ToString().Trim(), dt.Rows[i]["frDrName"].ToString().Trim(), dt.Rows[i]["FrRemark"].ToString().Trim(), dt.Rows[i]["SDate"].ToString().Trim(),
                                             dt.Rows[i]["ToDeptCode"].ToString().Trim(), dt.Rows[i]["ToDrCode"].ToString().Trim(), dt.Rows[i]["todrname"].ToString().Trim(), dt.Rows[i]["toremark"].ToString().Trim(), dt.Rows[i]["EDate"].ToString().Trim(),
                                             dt.Rows[i]["sName"].ToString().Trim(), dt.Rows[i]["Age"].ToString().Trim(), dt.Rows[i]["Sex"].ToString().Trim(), dt.Rows[i]["roomcode"].ToString().Trim(), dt.Rows[i]["gbemsms"].ToString().Trim(),
                                             strTREATNO);//
                }

                dt.Dispose();

                rtnVal = true;
            }
            catch (Exception ex)
            {
                SaveCvtLog(strPatid, strTREATNO, "Consult에러");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }



        /// <summary>
        /// Consult
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="strPano"></param>
        /// <param name="strFrDeptCode"></param>
        /// <param name="strFrDrCode"></param>
        /// <param name="strFrDrName"></param>
        /// <param name="strFrRemark"></param>
        /// <param name="strSDate"></param>
        /// <param name="strTrDeptCode"></param>
        /// <param name="strTrDrCode"></param>
        /// <param name="strTrDrName"></param>
        /// <param name="strTrRemark"></param>
        /// <param name="strEdate"></param>
        /// <param name="strSName"></param>
        /// <param name="strAge"></param>
        /// <param name="strSex"></param>
        /// <param name="strGbEMsms"></param>
        /// <returns></returns>
        public static bool New_Consult(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string strPano,
                                      string strFrDeptCode, string strFrDrCode, string strFrDrName, string strFrRemark, string strSDate,
                                      string strTrDeptCode, string strTrDrCode, string strToDrName, string strTrRemark, string strEdate,
                                      string strSName, string strAge, string strSex, string strRoomCode, string strGbEMsms, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            //long LNGY = 40;
            int lngLine = 0;
            string[] strResult;
            string strTResult = string.Empty;

            string strDrName = string.Empty;

            FontName = "굴림체";

            #endregion

            try
            {
                #region 제목줄
                New_initFormConsult("CONSULTATION");
                #endregion

                WriteStr(strToDrName, 25, 300, 240);

                WriteStr(strSName + " (" + strSex + "/" + strAge + ")", 25, 300, 290);

                WriteStr(strPatid, 25, 900, 290);

                WriteStr(strRoomCode + VB.Space(10) + (strGbEMsms.Equals("Y") ? "■응급  □비응급" : "□응급  ■비응급"), 25, 1100, 290);

                strTResult = strFrRemark;
                strResult = strTResult.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                lngLine = 0;

                #region 의뢰내용
                for (int K = 0; K < strResult.Length; K++)
                {
                    if (string.IsNullOrWhiteSpace(strResult[K]))
                    {
                        //lngLine += 1;
                    }
                    else
                    {
                        #region 기존

                        #region new page
                        //if (lngLine >= 38)
                        //{
                        //    WriteStr("(계속)", 25, 750, 1780);

                        //    TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                        //    //LNGY = 40;
                        //    lngLine = 1;
                        //    PageNum += 1;

                        //    New_initFormConsult("CONSULTATION");

                        //    WriteStr(strToDrName, 25, 300, 240);
                        //    WriteStr(strSName + " (" + strSex + "/" + strAge + ")", 25, 300, 290);
                        //    WriteStr(strPatid, 25, 900, 290);
                        //    WriteStr(strRoomCode, 25, 1100, 290);
                        //}
                        #endregion
                        //if (strResult[K].Length >= 60)
                        //{
                        //    WriteStr(VB.Left(strResult[K], 60), 20, 70, 360 + (lngLine * 35));
                        //    lngLine += 1;

                        //    if (strResult[K].Length - 60 > 60)
                        //    {
                        //        WriteStr(VB.Mid(strResult[K], 60 + 1, 60), 20, 70, 360 + (lngLine * 35));
                        //        lngLine += 1;
                        //        WriteStr(VB.Mid(strResult[K], 120 + 1, 60), 20, 70, 360 + (lngLine * 35));
                        //    }
                        //    else
                        //    {
                        //        WriteStr(VB.Mid(strResult[K], 60 + 1, 60), 20, 70, 360 + (lngLine * 35));
                        //    }
                        //}
                        //else
                        //{
                        //    WriteStr(strResult[K], 20, 70, 360 + (lngLine * 35));
                        //}

                        #endregion

                        #region 신규
                        int lastPos = 0;
                        using (Font font = new Font(FontName, 20))
                        {
                            Size PageSize = new Size(1648 - 70, 20);
                            Size strWidth = TextRenderer.MeasureText(strResult[K], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                            if (strWidth.Width < 1100)
                            {
                                WriteStr(20, 70, 360 + (lngLine * 35), strResult[K]);
                                lngLine += 1;
                            }
                            else
                            {
                                for (int l = 0; l < strResult[K].Length + 1; l++)
                                {
                                    bool DataOutPut = false;
                                    string strText = strResult[K].Substring(lastPos, l);
                                    strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                    if (strWidth.Width >= 1100)
                                    {
                                        lastPos += l;
                                        l = 0;
                                        WriteStr(20, 70, 360 + (lngLine * 35), strText);
                                        lngLine += 1;
                                        DataOutPut = true;
                                    }

                                    if (DataOutPut && lastPos + l == strResult[K].Length)
                                    {
                                        break;
                                    }

                                    if (DataOutPut == false && (l == strResult[K].Length || lastPos + l == strResult[K].Length))
                                    {
                                        WriteStr(20, 70, 360 + (lngLine * 35), strText);
                                        lngLine += 1;
                                        break;
                                    }
                                }
                            }

                            #region new page
                            if (lngLine >= 38)
                            {
                                string strNext = "N";
                                for (int kk = K; kk < strResult.Length; kk++)
                                {
                                    if (string.IsNullOrWhiteSpace(strResult[kk]) == false)
                                    {
                                        strNext = "Y";
                                    }
                                }

                                if (strNext.Equals("Y"))
                                {
                                    WriteStr("(계속)", 25, 750, 1780);

                                    TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                                    //LNGY = 40;
                                    lngLine = 1;
                                    PageNum += 1;

                                    New_initFormConsult("CONSULTATION");

                                    WriteStr(strToDrName, 25, 300, 240);
                                    WriteStr(strSName + " (" + strSex + "/" + strAge + ")", 25, 300, 290);
                                    WriteStr(strPatid, 25, 900, 290);
                                    WriteStr(strRoomCode, 25, 1100, 290);
                                }
                            }
                            #endregion
                        }
                        #endregion

                        lngLine += 1;

                    }
                }
                #endregion

                

                if (lngLine < 10) lngLine = 10;

                WriteStr(strSDate + " / " + strFrDrName + "[" + strFrDeptCode + "]", 25, 950, 360 + (lngLine * 35));

                #region 가로줄
                lngLine += 1;
                hline2(850, 370 + (lngLine * 35), 1550);

                lngLine += 1;
                hline(50, 360 + (lngLine * 35));
                #endregion

                #region new page
                if (lngLine >= 38)
                {
                    WriteStr("(계속)", 25, 750, 1780);

                    TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                    //LNGY = 40;
                    lngLine = 1;
                    PageNum += 1;

                    New_initFormConsult("CONSULTATION");

                    WriteStr(strToDrName, 25, 300, 240);
                    WriteStr(strSName + " (" + strSex + "/" + strAge + ")", 25, 300, 290);
                    WriteStr(strPatid, 25, 900, 290);
                    WriteStr(strRoomCode, 25, 1100, 290);
                }
                #endregion

                lngLine += 1;

                strTResult = strTrRemark.Replace(ComNum.VBLF, "\n");
                strResult = strTResult.Split('\n');

                int k = 0;
                #region 의뢰내용
                if (strResult.Length > 0)
                {
                    string strNext = string.Empty;

                    for (k = 0; k < strResult.Length; k++)
                    {
                        if (string.IsNullOrWhiteSpace(strResult[k]))
                        {
                            lngLine += 1;
                        }
                        else
                        {
                            #region 신규
                            int lastPos = 0;
                            using (Font font = new Font(FontName, 20))
                            {
                                Size PageSize = new Size(1648 - 70, 20);
                                Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                                if (strWidth.Width < 1100)
                                {
                                    WriteStr(20, 70, 360 + (lngLine * 35), strResult[k]);
                                    lngLine += 1;
                                }
                                else
                                {
                                    for (int l = 0; l < strResult[k].Length + 1;l++)
                                    {
                                        bool DataOutPut = false;
                                        string strText = strResult[k].Substring(lastPos, l);
                                        strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                        if (strWidth.Width >= 1100)
                                        {
                                            lastPos += l;
                                            l = 0;
                                            WriteStr(20, 70, 360 + (lngLine * 35), strText);
                                            lngLine += 1;
                                            DataOutPut = true;
                                        }

                                        if (DataOutPut && lastPos + l == strResult[k].Length)
                                        {
                                            break;
                                        }

                                        if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                        {
                                            WriteStr(20, 70, 360 + (lngLine * 35), strText);
                                            lngLine += 1;
                                            break;
                                        }
                                    }
                                }

                                #region new page
                                if (lngLine >= 38)
                                {
                                    strNext = "N";
                                    for (int kk = k; kk < strResult.Length; kk++)
                                    {
                                        if (string.IsNullOrWhiteSpace(strResult[kk]) == false)
                                        {
                                            strNext = "Y";
                                        }
                                    }

                                    if (strNext.Equals("Y"))
                                    {
                                        WriteStr("(계속)", 25, 750, 1780);

                                        TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                                        //LNGY = 40;
                                        lngLine = 1;
                                        PageNum += 1;

                                        New_initFormConsult("CONSULTATION");

                                        WriteStr(strToDrName, 25, 300, 240);
                                        WriteStr(strSName + " (" + strSex + "/" + strAge + ")", 25, 300, 290);
                                        WriteStr(strPatid, 25, 900, 290);
                                        WriteStr(strRoomCode, 25, 1100, 290);
                                    }
                                }
                                #endregion
                            }
                            #endregion
                            #region 한글 쪼개짐

                            //string str = string.Empty;
                            //StringBuilder strTmp = new StringBuilder();

                            #region 이전
                            //for (int L = 1; L < strByte + 1; L++)
                            //{
                            //str = ComFunc.MidH(str2, L, 1);

                            //byte[] AscBytes = Encoding.ASCII.GetBytes(str);

                            //if (AscBytes.Length > 0 && AscBytes[0] == 63) //한글 쪼개짐
                            //{
                            //    strTmp.Append(ComFunc.MidH(str2, L, 2));
                            //    L += 1;

                            //    nCNT += 2;
                            //}
                            //else
                            //{
                            //    strTmp.Append(ComFunc.MidH(str2, L, 1));
                            //    nCNT += 1;
                            //}

                            //if (nCNT >= 98 || L >= strByte)
                            //{
                            //    WriteStr(strTmp.ToString().Trim(), 20, 70, 360 + (lngLine * 35));
                            //    strTmp.Clear();
                            //    nCNT = 0;
                            //    lngLine += 1;

                            //    #region new page
                            //    if (lngLine >= 38)
                            //    {
                            //        strNext = strResult.ToList().Where(d => string.IsNullOrWhiteSpace(d) == false).Count() > 0 ? "Y" : "N";

                            //        if (strNext.Equals("Y"))
                            //        {
                            //            WriteStr("(계속)", 25, 750, 1780);

                            //            TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                            //            //LNGY = 40;
                            //            lngLine = 1;
                            //            PageNum += 1;

                            //            New_initFormConsult("CONSULTATION");

                            //            WriteStr(strToDrName, 25, 300, 240);
                            //            WriteStr(strSName + " (" + strSex + "/" + strAge + ")", 25, 300, 290);
                            //            WriteStr(strPatid, 25, 900, 290);
                            //            WriteStr(strRoomCode, 25, 1100, 290);
                            //        }
                            //    }
                            //    #endregion
                            //}
                            //}
                            #endregion

                            #region 신규

                            #endregion

                            #endregion
                        }
                        //lngLine += 1;
                    }
                    #region new page
                    if (lngLine >= 38)
                    {
                        strNext = "N";
                        for (int kk = k; kk < strResult.Length; kk++)
                        {
                            if (string.IsNullOrWhiteSpace(strResult[kk]) == false)
                            {
                                strNext = "Y";
                            }
                        }

                        if (strNext.Equals("Y"))
                        {
                            WriteStr("(계속)", 25, 750, 1780);

                            TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                            //LNGY = 40;
                            lngLine = 1;
                            PageNum += 1;

                            New_initFormConsult("CONSULTATION");

                            WriteStr(strToDrName, 25, 300, 240);
                            WriteStr(strSName + " (" + strSex + "/" + strAge + ")", 25, 300, 290);
                            WriteStr(strPatid, 25, 900, 290);
                            WriteStr(strRoomCode, 25, 1100, 290);
                        }

                        #endregion
                    }
                }

                #endregion

                //WriteStr(strEdate + " / " + strToDrName + "[" + strTrDeptCode + "]", 25, 950, 1750);
                //hline2(850, 1795, 1550);
                lngLine += 1;
                WriteStr(strEdate + " / " + strToDrName + "[" + strTrDeptCode + "]", 25, 950, 360 + (lngLine * 35));

                //lngLine += 1;
                //hline2(850, 370 + (lngLine * 35), 1550);

                lngLine += 1;
                hline2(850, 370 + (lngLine * 35), 1550);
                //hline(50, 360 + (lngLine * 35)); //마지막줄

                //using (Pen pen = new Pen(Brushes.Black, 3))
                //{
                //    mGraphics.DrawLine(pen, 800, 360 + (lngLine * 35), 800, 360 + (lngLine * 35) + 497);
                //}

                //lngLine += 3;
                //WriteStr("포항성모병원", 25, 1100, 360 + (lngLine * 35));

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");
            }
            catch (Exception ex)
            {
                SaveCvtLog(strPatid, strTREATNO, "Consult에러");
                clsDB.SaveSqlErrLog(ex.Message + "\r\nConsult에러", "", pDbCon);
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        public static void New_initFormConsult(string str)
        {
            #region 이미지 그리기
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(str, 35, 650, 80);

            WriteStr("Attending Physician : ", 25, 70, 190);
            hline(50, 230);

            WriteStr("Consultant :", 25, 70, 240);
            hline(50, 285);


            WriteStr("Patient    :", 25, 70, 290);

            WriteStr("병록번호 :", 25, 650, 290);
            WriteStr("호실", 25, 1200, 290);

            hline(50, 335);
            hline(50, 1830); //마지막줄
            vline(800, 1830, 2200); //가운데ㅐ줄
            WriteStr("포항성모병원", 25, 1100, 2000);
            #endregion
        }
        #endregion

        #region 처방의 판독지
        /// <summary>
        /// 
        /// </summary>
        /// <param name="var"></param>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="strOutDate"></param>
        /// <param name="strDocCode"></param>
        /// <param name="strTREATNO"></param>
        /// <returns></returns>
        public static bool New_Xray_SelectNo_Dr(PsmhDb pDbCon, string var, string strPatid, string strDate, string strClinCode, string strClass, string strOutDate, string strDocCode, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            
            //137(입원), 138(외래)
            gstrFormcode = strClass.Trim().Equals("I") ? "137" : "138";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            PageNum = 0;

            string ErTreatNo = string.Empty;
            string ErOutDate = string.Empty;
            GetPatIpdInfo(pDbCon, strPatid, "O", DateTime.ParseExact(strDate, "yyyyMMdd", null).AddDays(1).ToString("yyyyMMdd"), "ER", ref ErTreatNo, ref ErOutDate);
            #endregion

            try
            {

                #region 쿼리
                SQL = "  SELECT DRWRTNO, PANO, SEEKDATE , ORDERNAME ,XJONG, ENTERDATE FROM kosmos_PMPA.XRAY_DETAIL";
                SQL += ComNum.VBLF + " WHERE pano =  '" + strPatid + "' ";
                SQL += ComNum.VBLF + "   AND ENTERDATE >= TO_DATE('2007-08-01','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "   AND SEEKDATE  >= TO_DATE('2007-08-01','YYYY-MM-DD') ";

                //SQL += ComNum.VBLF + "   AND ENTERDATE >= TO_DATE('2011-01-01','YYYY-MM-DD') ";
                //SQL += ComNum.VBLF + "   AND SEEKDATE  >= TO_DATE('2011-01-01','YYYY-MM-DD') ";

                if (strClass.Equals("O") && strDate.Equals("20041231") && strClinCode.Equals("RA") == false)
                {
                    SQL += ComNum.VBLF + "  AND DEPTCODE = '" + strClinCode + "' ";
                    SQL += ComNum.VBLF + "  AND IPDOPD   = '" + strClass + "' AND DRWRTNO > 0 ";
                    SQL += ComNum.VBLF + "  AND to_char(ENTERDATE,'YYYYMMDD') <= TO_CHAR(SYSDATE,'YYYYMMDD') AND DRCODE not in ('1107','1125','0901','0902','0903') ";
                }
                else if (strClass.Equals("O"))
                {
                    if (strClinCode.Equals("ER"))
                    {
                        //다음날 내원 안했을 경우 +1일 까지
                        if (string.IsNullOrWhiteSpace(ErTreatNo))
                        {
                            SQL += ComNum.VBLF + "       AND BDATE >= TO_DATE('" + DateTime.ParseExact(strDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "       AND BDATE <= TO_DATE('" + DateTime.ParseExact(strDate, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        }
                        else//다음날도 또 왔으면 당일 것만..
                        {
                            SQL += ComNum.VBLF + "       AND BDATE = TO_DATE('" + DateTime.ParseExact(strDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        }
                    }
                    else
                    {

                        SQL += ComNum.VBLF + "  AND to_char(BDATE,'YYYYMMDD') ='" + strDate + "' ";
                    }

                    SQL += ComNum.VBLF + "  AND DEPTCODE = '" + strClinCode + "' ";
                    SQL += ComNum.VBLF + "  AND IPDOPD   = '" + strClass + "'";
                }
                else if (strClass.Equals("I"))
                {
                    SQL += ComNum.VBLF + "  AND IPDOPD  = '" + strClass + "' ";
                    SQL += ComNum.VBLF + "  AND to_char(BDATE,'YYYYMMDD') >='" + strDate + "' ";
                    SQL += ComNum.VBLF + "  AND to_char(BDATE,'YYYYMMDD') <='" + strOutDate + "'  ";
                    SQL += ComNum.VBLF + "  AND DRCODE not in ('1107','1125','0901','0902','0903') ";
                }

                #region 김민원 오동호류마티스내과....
                if (strClinCode.Equals("RA") || strClinCode.Equals("MR"))
                {
                    SQL = "  SELECT DRWRTNO, EXINFO,PANO, SEEKDATE , ORDERNAME ,XJONG, ENTERDATE FROM kosmos_PMPA.XRAY_DETAIL";
                    SQL += ComNum.VBLF + "     WHERE pano =  '" + strPatid + "' ";

                    if (strClass.Equals("O") && strDate.Equals("20041231"))
                    {
                        SQL += ComNum.VBLF + "  AND DEPTCODE IN ('MD' ,'MR') ";
                        SQL += ComNum.VBLF + "  AND IPDOPD ='" + strClass + "' AND DRWRTNO > 0 ";
                        SQL += ComNum.VBLF + "  AND to_char(ENTERDATE,'YYYYMMDD') <=TO_CHAR(SYSDATE,'YYYYMMDD') and DRCODE IN ('1107','1125','0901','0902','0903') "; 
                    }
                    else if (strClass.Equals("O"))
                    {
                        SQL += ComNum.VBLF + "  AND DEPTCODE IN ('MD','MR')  ";
                        SQL += ComNum.VBLF + "  AND IPDOPD ='" + strClass + "' AND DRWRTNO > 0 ";
                        SQL += ComNum.VBLF + "  AND to_char(BDATE,'YYYYMMDD') ='" + strDate + "'  and DRCODE IN ( '1107','1125','0901','0902','0903') ";
                    }
                    else if (strClass.Equals("I"))
                    {
                        SQL += ComNum.VBLF + "  AND to_char(BDATE,'YYYYMMDD') >='" + strDate + "' ";
                        SQL += ComNum.VBLF + "  AND to_char(BDATE,'YYYYMMDD') <='" + strOutDate + "'   ";
                    }
                }
                #endregion


                SQL += ComNum.VBLF + " AND DRWRTNO > 0 ";//  ' NULL이나 0은 판독결과 없음.
                SQL += ComNum.VBLF + " ORDER BY DRWRTNO ";

                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                SaveCvtLog(strPatid, strTREATNO, "처방의");

                for(int i = 0; i < dt.Rows.Count; i++)
                { 

                    PageNum += 1;

                    //DRWRTNO, PANO, SEEKDATE , ORDERNAME ,XJONG, ENTERDATE
                    New_XRAY_DR(
                     pDbCon, strPatid.Trim(), strDate.Trim(), strClinCode.Trim(), strClass.Trim(),
                     dt.Rows[i]["DRWRTNO"].ToString().Trim(),
                     dt.Rows[i]["ENTERDATE"].ToString().Trim(), 
                     strTREATNO.Trim(), 
                     dt.Rows[i]["ORDERNAME"].ToString().Trim(),
                     dt.Rows[i]["XJONG"].ToString().Trim(),
                     dt.Rows[i]["SEEKDATE"].ToString().Trim());
                }

                dt.Dispose();

                rtnVal = true;
            }
            catch (Exception ex)
            {
                SaveCvtLog(strPatid, strTREATNO, "처방의에러");
                clsDB.SaveSqlErrLog(ex.Message + "\r\n처방의에러", "", pDbCon);
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        /// <summary>
        /// 처방의 검사지
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="strWrtno"></param>
        /// <param name="strEnterdate"></param>
        /// <param name="strTREATNO"></param>
        /// <param name="strOrderName"></param>
        /// <param name="strXjong"></param>
        /// <param name="strSeekDate"></param>
        /// <param name="argOrderno"></param>
        /// <returns></returns>
        public static bool New_XRAY_DR(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string strWrtno, string strEnterdate,
                                    string strTREATNO, string strOrderName, string strXjong, string strSeekDate)
        {
            #region 변수
            bool rtnVal = false;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            //long LNGY = 40;
            int lngLine = 0;
            string FileName = string.Empty;
            string[] strResult;
            //string[] strResult1;
            string strTResult = string.Empty;
            //long PAGENO = 0;

            string strDrName = string.Empty;

            FontName = "굴림체";

            #endregion

            if (strWrtno.Trim().Equals("1"))
                return rtnVal;


            try
            {
                #region 쿼리
                SQL = " SELECT PANO,SNAME,AGE ||'/'|| SEX AgeSex, (CASE WHEN IPDOPD ='I' THEN '입원' ELSE '외래' END) gbio ,";
                SQL += ComNum.VBLF + " WARDCODE ||'/'||ROOMCODE  ward, D.DEPTNAMEK,DRCODE ,XDRCODE1,READDATE , RESULT , RESULT1  ,XNAME , SYSDATE";
                SQL += ComNum.VBLF + "  FROM KOSMOS_PMPA.XRAY_RESULTNEW_DR X, kosmos_pmpa.BAS_CLINICDEPT  D ";
                SQL += ComNum.VBLF + " WHERE D.DEPTCODE = X.DEPTCODE ";
                SQL += ComNum.VBLF + "   AND DRWRTNO =  " + strWrtno;
                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                #region 제목줄
                New_initFormXray_DR("처방의 판독 결과지", strXjong);
                #endregion

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strDrName = dt.Rows[i]["DrCode"].ToString().Trim();

                    if (string.IsNullOrWhiteSpace(strDrName)) strDrName = New_select_Doccode(pDbCon, strWrtno);

                    #region 내용 뿌리기
                    WriteStr(dt.Rows[i]["Pano"].ToString().Trim(), 25, 250, 245);
                    WriteStr(dt.Rows[i]["DeptNamek"].ToString().Trim(), 25, 250, 295);
                    WriteStr(dt.Rows[i]["sName"].ToString().Trim() + "", 25, 740, 245);
                    WriteStr(New_Spec_SelectDocName(pDbCon, dt.Rows[i]["DrCode"].ToString().Trim()), 25, 740, 295);
                    WriteStr(dt.Rows[i]["agesex"].ToString().Trim(), 25, 1350, 245);
                    WriteStr(dt.Rows[i]["GBio"].ToString().Trim(), 25, 1440, 245);
                    WriteStr(Convert.ToDateTime(strEnterdate).ToString("yyyy-MM-dd"), 25, 1350, 295); //검사요청일
                    WriteStr(Convert.ToDateTime(strSeekDate).ToString("yyyy-MM-dd"), 25, 240, 2060); //촬영일자

                    WriteStr(Convert.ToDateTime(dt.Rows[i]["ReadDate"].ToString().Trim()).ToString("yyyy-MM-dd"), 25, 940, 2060);
                    WriteStr(READ_PassName(pDbCon, dt.Rows[i]["xdrcode1"].ToString().Trim()), 25, 1400, 2060);

                    WriteStr("검사명 : " + dt.Rows[i]["xname"].ToString().Trim(), 25, 90, 450);
                    #endregion

                    strTResult = dt.Rows[i]["result"].ToString().Trim() + dt.Rows[i]["RESULT1"].ToString().Trim();
                    strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                    strResult = strTResult.Split('\n');

                    if (strResult.Length > 0)
                    {
                        for (int k = 0; k < strResult.Length; k++)
                        {
                            if (string.IsNullOrWhiteSpace(strResult[k]))
                            {
                                lngLine += 1;
                            }
                            else
                            {
                                int strByte = Encoding.Default.GetBytes(strResult[k]).Length;

                                #region 한글분해
                                if (strByte >= 98)
                                {
                                    string strNext = string.Empty;

                                    #region 신규
                                    int lastPos = 0;
                                    
                                    using (Font font = new Font(FontName, 20))
                                    {
                                        Size PageSize = new Size(1648 - 70, 20);
                                        Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                                        if (strWidth.Width < 1100)
                                        {
                                            WriteStr(20, 70, 550 + (lngLine * 35), strResult[k]);
                                            lngLine += 1;
                                        }
                                        else
                                        {
                                            for (int l = 0; l < strResult[k].Length + 1;l++)
                                            {
                                                bool DataOutPut = false;
                                                string strText = strResult[k].Substring(lastPos, l);
                                                strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                                if (strWidth.Width >= 1100)
                                                {
                                                    lastPos += l;
                                                    l = 0;
                                                    WriteStr(20, 70, 550 + (lngLine * 35), strText);
                                                    lngLine += 1;
                                                    DataOutPut = true;
                                                }

                                                if (DataOutPut && lastPos + l == strResult[k].Length)
                                                {
                                                    break;
                                                }

                                                if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                                {
                                                    WriteStr(20, 70, 550 + (lngLine * 35), strText);
                                                    lngLine += 1;
                                                    break;
                                                }
                                            }
                                        }

                                        #region new page
                                        if (lngLine > 37)
                                        {
                                            strNext = "N";
                                            for (int kk = k; kk < strResult.Length; kk++)
                                            {
                                                if (string.IsNullOrWhiteSpace(strResult[kk]) == false)
                                                {
                                                    strNext = "Y";
                                                }
                                            }

                                            if (strNext.Equals("Y"))
                                            {
                                                WriteStr("(계속)", 25, 750, 1910);

                                                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                                                //LNGY = 40;
                                                lngLine = 1;
                                                PageNum += 1;

                                                New_initFormXray_DR("처방의 판독 결과지", strXjong);

                                                WriteStr(dt.Rows[i]["Pano"].ToString().Trim(), 25, 250, 245);
                                                WriteStr(dt.Rows[i]["DeptNamek"].ToString().Trim(), 25, 250, 295);
                                                WriteStr(dt.Rows[i]["sName"].ToString().Trim() + "", 25, 740, 245);
                                                WriteStr(New_Spec_SelectDocName(pDbCon, dt.Rows[i]["DrCode"].ToString().Trim()), 25, 740, 295);
                                                WriteStr(dt.Rows[i]["agesex"].ToString().Trim(), 25, 1350, 245);
                                                WriteStr(strClass.Equals("O") ? dt.Rows[i]["GBio"].ToString().Trim() : dt.Rows[i]["Ward"].ToString().Trim(), 25, 1440, 245);
                                                WriteStr(Convert.ToDateTime(strEnterdate).ToString("yyyy-MM-dd"), 25, 1350, 295); //검사요청일
                                                WriteStr(Convert.ToDateTime(strSeekDate).ToString("yyyy-MM-dd"), 25, 240, 2060); //촬영일자

                                                WriteStr(Convert.ToDateTime(dt.Rows[i]["ReadDate"].ToString().Trim()).ToString("yyyy-MM-dd"), 25, 940, 2060);
                                                WriteStr(READ_PassName(pDbCon, dt.Rows[i]["xdrcode1"].ToString().Trim()), 25, 1400, 2060);

                                                WriteStr("검사명 : " + dt.Rows[i]["xname"].ToString().Trim(), 25, 90, 450);
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region 한글 쪼개짐 (이전)

                                    //string str = string.Empty;
                                    //string str1 = string.Empty;
                                    //string str2 = string.Empty;
                                    //StringBuilder strTmp = new StringBuilder();
                                    //string strNext = string.Empty;
                                    //int nCNT = 0;

                                    //for (int L = 1; L < strByte + 1; L++)
                                    //{
                                    //    str = ComFunc.MidH(str2, L, 1);

                                    //    if (string.IsNullOrWhiteSpace(str) == false)
                                    //    {
                                    //        if (Encoding.ASCII.GetBytes(str)[0] == 63) //한글 쪼개짐
                                    //        {
                                    //            strTmp.Append(ComFunc.MidH(str2, L, 2));
                                    //            L += 1;

                                    //            nCNT += 2;
                                    //        }
                                    //        else
                                    //        {
                                    //            strTmp.Append(ComFunc.MidH(str2, L, 1));
                                    //            nCNT += 1;
                                    //        }
                                    //    }


                                    //    if (nCNT >= 98 || L >= strByte)
                                    //    {
                                    //        WriteStr(strTmp.ToString().Trim(), 20, 70, 550 + (lngLine * 35));
                                    //        strTmp.Clear();
                                    //        nCNT = 0;
                                    //        lngLine += 1;

                                    //        #region new page
                                    //        if (lngLine > 37)
                                    //        {
                                    //            strNext = strResult.ToList().Where(d => string.IsNullOrWhiteSpace(d) == false).Count() > 0 ? "Y" : "N";

                                    //            if (strNext.Equals("Y"))
                                    //            {
                                    //                WriteStr("(계속)", 25, 750, 1910);

                                    //                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                                    //                //LNGY = 40;
                                    //                lngLine = 1;
                                    //                PageNum += 1;

                                    //                New_initFormXray_DR("처방의 판독 결과지", strXjong);

                                    //                WriteStr(dt.Rows[i]["Pano"].ToString().Trim(), 25, 250, 245);
                                    //                WriteStr(dt.Rows[i]["DeptNamek"].ToString().Trim(), 25, 250, 295);
                                    //                WriteStr(dt.Rows[i]["sName"].ToString().Trim() + "", 25, 740, 245);
                                    //                WriteStr(New_Spec_SelectDocName(pDbCon, dt.Rows[i]["DrCode"].ToString().Trim()), 25, 740, 295);
                                    //                WriteStr(dt.Rows[i]["agesex"].ToString().Trim(), 25, 1350, 245);
                                    //                WriteStr(dt.Rows[i]["GBio"].ToString().Trim(), 25, 1440, 245);
                                    //                WriteStr(Convert.ToDateTime(strEnterdate).ToString("yyyy-MM-dd"), 25, 1350, 295); //검사요청일
                                    //                WriteStr(Convert.ToDateTime(strSeekDate).ToString("yyyy-MM-dd"), 25, 240, 2060); //촬영일자

                                    //                WriteStr(Convert.ToDateTime(dt.Rows[i]["ReadDate"].ToString().Trim()).ToString("yyyy-MM-dd"), 25, 940, 2060);
                                    //                WriteStr(READ_PassName(pDbCon, dt.Rows[i]["xdrcode1"].ToString().Trim()), 25, 1400, 2060);

                                    //                WriteStr("검사명 : " + dt.Rows[i]["xname"].ToString().Trim(), 25, 90, 450);
                                    //            }
                                    //        }
                                    //        #endregion
                                    //    }
                                    //}
                                    #endregion
                                }
                                else
                                {
                                    WriteStr(strResult[k], 22, 70, 550 + (lngLine * 35));
                                }
                                #endregion 한글분해

                                lngLine += 1;
                            }

                            #region new page
                            if (lngLine > 37)
                            {
                                WriteStr("(계속)", 25, 750, 1910);

                                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                                //LNGY = 40;
                                lngLine = 1;
                                PageNum += 1;

                                New_initFormXray_DR("처방의 판독 결과지", strXjong);

                                WriteStr(dt.Rows[i]["Pano"].ToString().Trim(), 25, 250, 245);
                                WriteStr(dt.Rows[i]["DeptNamek"].ToString().Trim(), 25, 250, 295);
                                WriteStr(dt.Rows[i]["sName"].ToString().Trim() + "", 25, 740, 245);
                                WriteStr(New_Spec_SelectDocName(pDbCon, dt.Rows[i]["DrCode"].ToString().Trim()), 25, 740, 295);
                                WriteStr(dt.Rows[i]["agesex"].ToString().Trim(), 25, 1350, 245);
                                WriteStr(strClass.Equals("O") ? dt.Rows[i]["GBio"].ToString().Trim() : dt.Rows[i]["Ward"].ToString().Trim(), 25, 1440, 245);
                                WriteStr(Convert.ToDateTime(strEnterdate).ToString("yyyy-MM-dd"), 25, 1350, 295); //검사요청일
                                WriteStr(Convert.ToDateTime(strSeekDate).ToString("yyyy-MM-dd"), 25, 240, 2060); //촬영일자

                                WriteStr(Convert.ToDateTime(dt.Rows[i]["ReadDate"].ToString().Trim()).ToString("yyyy-MM-dd"), 25, 940, 2060);
                                WriteStr(READ_PassName(pDbCon, dt.Rows[i]["xdrcode1"].ToString().Trim()), 25, 1400, 2060);

                                WriteStr("검사명 : " + dt.Rows[i]["xname"].ToString().Trim(), 25, 90, 450);
                            }

                            #endregion

                        }
                    }

                }

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n처방의에러", "", pDbCon);
                SaveCvtLog(strPatid, strTREATNO, "처방의에러");
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        public static void New_initFormXray_DR(string str, string strXjong)
        {
            #region 이미지 그리기
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(str, 50, 550, 80);

            WriteStr("=========================================================================================", 25, 50, 200);
            WriteStr("=========================================================================================", 25, 50, 340);

            WriteStr("등록번호:", 25, 80, 245);
            WriteStr("의 뢰 과:", 25, 80, 295);
            WriteStr("성 명 :  ", 25, 570, 245);
            WriteStr("의 사 : ", 25, 570, 295);
            WriteStr("성    별: ", 25, 1140, 245);
            WriteStr("검사요청일: ", 25, 1140, 295);

            WriteStr("촬영일자: ", 25, 60, 2060);

            WriteStr("판독일자: ", 25, 750, 2060);
            WriteStr("판독의사: ", 25, 1230, 2060);

            WriteStr("----------------------------------------------------------------------------------------", 25, 50, 2100);

            if (strXjong.Equals("E"))
            {
                WriteStr("Pohang St.Mary's Hospital.  Physical medicine & Rehabilitation   Tel : 054 -289-4591", 25, 70, 2140);
            }
            else if (strXjong.Equals("6"))
            {
                WriteStr("Pohang St.Mary's Hospital.  Department of Nuclear Medicine.      Tel : 054 -289-4520", 25, 70, 2140);
            }
            else if (strXjong.Equals("8"))
            {
                WriteStr("Pohang St.Mary's Hospital.  Department of Nuclear Medicine.      Tel : 054 -289-4520", 25, 70, 2140);
            }
            else
            {
                WriteStr("* 포항성모병원 ", 25, 70, 2140);
            }
              

            #endregion
        }
        #endregion

        #region 방사선 판독지
        /// <summary>
        /// 
        /// </summary>
        /// <param name="var"></param>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="strOutDate"></param>
        /// <param name="strDocCode"></param>
        /// <param name="strTREATNO"></param>
        /// <returns></returns>
        public static bool New_Xray_SelectNo(PsmhDb pDbCon, string var, string strPatid, string strDate, string strClinCode, string strClass, string strOutDate, string strDocCode, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;

            gstrFormcode = strClass.Trim().Equals("I") ? "111" : "004";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;
            PageNum = 0;

            string ErTreatNo = string.Empty;
            string ErOutDate = string.Empty;
            GetPatIpdInfo(pDbCon, strPatid, "O", DateTime.ParseExact(strDate, "yyyyMMdd", null).AddDays(1).ToString("yyyyMMdd"), "ER", ref ErTreatNo, ref ErOutDate);
            #endregion

            try
            {

                #region 쿼리
                SQL = "  SELECT EXINFO,PANO, SEEKDATE , ORDERNAME ,XJONG, ENTERDATE, ORDERNO FROM kosmos_PMPA.XRAY_DETAIL";
                SQL += ComNum.VBLF + "     WHERE pano =  '" + strPatid + "' ";
                //SQL += ComNum.VBLF + "       AND BDATE >= TO_DATE('2011-01-01','YYYY-MM-DD') ";


                //if (strClass.Equals("O") && strDate.Equals("20041231") && strClinCode.Equals("RA") == false)
                //{
                //    SQL += ComNum.VBLF + "  AND DEPTCODE ='" + strClinCode + "' ";
                //    SQL += ComNum.VBLF + "  AND IPDOPD ='" + strClass + "' AND EXINFO > 1000 ";
                //    SQL += ComNum.VBLF + "  AND to_char(ENTERDATE,'YYYYMMDD') <=TO_CHAR(SYSDATE,'YYYYMMDD')";
                //}
                if (strClass.Equals("O"))
                {
                    if (strClinCode.Equals("R6"))
                    {
                        SQL += ComNum.VBLF + "       AND DEPTCODE IN ('R6','RD') ";
                        SQL += ComNum.VBLF + "       AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    }
                    else if (strClinCode.Equals("ER"))
                    {
                        SQL += ComNum.VBLF + "       AND DEPTCODE = '" + strClinCode + "' ";
                        //다음날 내원 안했을 경우 +1일 까지
                        if (string.IsNullOrWhiteSpace(ErTreatNo))
                        {
                            SQL += ComNum.VBLF + "       AND BDATE >= TO_DATE('" + DateTime.ParseExact(strDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "       AND BDATE <= TO_DATE('" + DateTime.ParseExact(strDate, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        }
                        else//다음날도 또 왔으면 당일 것만..
                        {
                            SQL += ComNum.VBLF + "       AND BDATE = TO_DATE('" + DateTime.ParseExact(strDate, "yyyyMMdd", null).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        }
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "       AND DEPTCODE = '" + strClinCode + "' ";
                        SQL += ComNum.VBLF + "       AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    }

                    SQL += ComNum.VBLF + "       AND IPDOPD = '" + strClass + "'";
                }
                else if (strClass.Equals("I"))
                {
                    SQL += ComNum.VBLF + "       AND ( IPDOPD ='" + strClass + "' or DeptCode ='ER') ";// '당일입원 등록요청
                    SQL += ComNum.VBLF + "       AND BDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "       AND BDATE <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                }

                #region 김민원 오동호류마티스내과....
                if (strClinCode.Equals("RA") || strClinCode.Equals("MR"))
                {
                    SQL = "  SELECT EXINFO,PANO, SEEKDATE , ORDERNAME ,XJONG, ENTERDATE, ORDERNO FROM kosmos_PMPA.XRAY_DETAIL";
                    SQL += ComNum.VBLF + "     WHERE pano =  '" + strPatid + "' ";
                    //SQL += ComNum.VBLF + "       AND BDATE >= TO_DATE('2011-01-01','YYYY-MM-DD') ";

                    //if (strClass.Equals("O") && strDate.Equals("20041231"))
                    //{
                    //    SQL += ComNum.VBLF + "  AND DEPTCODE IN ( 'MD' ,'MR')  ";
                    //    SQL += ComNum.VBLF + "  AND IPDOPD ='" + strClass + "' AND EXINFO > 10000 ";
                    //    SQL += ComNum.VBLF + "  AND to_char(ENTERDATE,'YYYYMMDD') <=TO_CHAR(SYSDATE,'YYYYMMDD') and DRCODE IN ('1107','1125','0901','0902','0903') ";
                    //}
                    if (strClass.Equals("O"))
                    {
                        SQL += ComNum.VBLF + "       AND DEPTCODE IN ( 'MD','MR')  ";
                        SQL += ComNum.VBLF + "       AND IPDOPD ='" + strClass + "' AND EXINFO > 10000 ";
                        SQL += ComNum.VBLF + "       AND BDATE = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "       AND DRCODE IN ('1107','1125','0901','0902','0903') ";
                        //SQL += ComNum.VBLF + "  AND to_char(BDATE,'YYYYMMDD') ='" + strDate + "'  and DRCODE IN ('1107','1125','0901','0902','0903') ";
                    }
                    else if (strClass.Equals("I"))
                    {
                        SQL += ComNum.VBLF + "       AND BDATE >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "       AND BDATE <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                    }
                }
                #endregion


                SQL += ComNum.VBLF + " AND EXINFO > 1000";
                SQL += ComNum.VBLF + " AND (EXINFO IS NOT NULL OR EXINFO >= 1000) ";//  ' NULL이나 0은 판독결과 없음.
                SQL += ComNum.VBLF + " ORDER BY EXINFO ";

                #endregion

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                SaveCvtLog(strPatid, strTREATNO, "방사선");

                for(int i = 0; i < dt.Rows.Count; i++)
                { 
                    if (strClass.Equals("I"))
                    {
                        gstrFormcode = "110";
                    }
                    else
                    {
                        gstrFormcode = "005";
                    }

                    string strJong = dt.Rows[i]["XJONG"].ToString().Trim();

                    if (strClass.Trim().Equals("I") && strJong.Equals("E"))
                    {
                        gstrFormcode = "129";//  '입원 근전도 검사지
                    }
                    else if (strClass.Trim().Equals("O") && strJong.Equals("E"))
                    {
                        gstrFormcode = "016"; //외래 근전도 검사지
                    }

                    PageNum += 1;

                    //전기진단 검사 결과지
                    if (strJong.Equals("E"))
                    {
                        //SELECT EXINFO,PANO, SEEKDATE , ORDERNAME ,XJONG, ENTERDATE, ORDERNO FROM kosmos_PMPA.XRAY_DETAIL
                        //검사지 근전도
                        New_XRAY(pDbCon, strPatid.Trim(), strDate.Trim(), strClinCode.Trim(), strClass.Trim(), dt.Rows[i]["exinfo"].ToString().Trim(),
                            dt.Rows[i]["entERdate"].ToString().Trim(), strTREATNO.Trim(), dt.Rows[i]["ordername"].ToString().Trim(), "E", dt.Rows[i]["seekdate"].ToString().Trim());
                    }
                    else if(strJong.Equals("4"))
                    {
                        New_XRAY(pDbCon, strPatid.Trim(), strDate.Trim(), strClinCode.Trim(), strClass.Trim(), dt.Rows[i]["exinfo"].ToString().Trim(),
                                dt.Rows[i]["entERdate"].ToString().Trim(), strTREATNO.Trim(), dt.Rows[i]["ordername"].ToString().Trim(), "4", dt.Rows[i]["seekdate"].ToString().Trim(),
                                dt.Rows[i]["orderno"].ToString().Trim());
                    }
                    else if(strJong.Equals("C"))
                    {
                    }
                    else
                    {
                        New_XRAY(pDbCon, strPatid.Trim(), strDate.Trim(), strClinCode.Trim(), strClass.Trim(), dt.Rows[i]["exinfo"].ToString().Trim(),
        dt.Rows[i]["entERdate"].ToString().Trim(), strTREATNO.Trim(), dt.Rows[i]["ordername"].ToString().Trim(), dt.Rows[i]["xjong"].ToString().Trim(), dt.Rows[i]["seekdate"].ToString().Trim(),
        dt.Rows[i]["orderno"].ToString().Trim());
                    }
                }

                dt.Dispose();

                rtnVal = true;
            }
            catch (Exception ex)
            {
                SaveCvtLog(strPatid, strTREATNO, "방사선에러");
                clsDB.SaveSqlErrLog(ex.Message + "\r\n방사선에러", "", pDbCon);
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        public static bool New_XRAY(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string strWrtno, string strEnterdate,
                                    string strTREATNO, string strOrderName, string strXjong, string strSeekDate, string argOrderno = "")
        {
            #region 변수
            bool rtnVal = false;

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            //long LNGY = 40;
            int lngLine = 0;
            string FileName = string.Empty;
            string[] strResult;
            //string[] strResult1;
            string strTResult = string.Empty;
            //long PAGENO = 0;

            string strDrName  = string.Empty;

            FontName = "굴림체";

            #endregion

            if (strWrtno.Trim().Equals("1"))
                return rtnVal;


            try
            {
   

                #region 쿼리
                SQL = " SELECT B.PANO, B.SNAME SNAME, X.AGE ||'/'|| X.SEX AGESEX , (CASE WHEN X.IPDOPD ='I' THEN '입원' ELSE '외래' END) GBIO  ,";
                SQL += ComNum.VBLF + " X.WARDCODE ||'/'||X.ROOMCODE WARD, D.DEPTNAMEK, X.DRCODE , X.XDRCODE1,";
                SQL += ComNum.VBLF + " NVL(TO_CHAR(READTIME, 'YYYY-MM-DD HH24:MI'), TO_CHAR(READDATE,'YYYY-MM-DD')) READDATE , RESULT , RESULT1  , ";
                SQL += ComNum.VBLF + " ADDENDUM1, ADDENDUM2, XNAME , SYSDATE, B.JUMIN1, B.JUMIN2 ";
                SQL += ComNum.VBLF + "    FROM KOSMOS_PMPA.XRAY_RESULTNEW X, kosmos_pmpa.BAS_CLINICDEPT  D, KOSMOS_PMPA.BAS_PATIENT B";
                SQL += ComNum.VBLF + "   WHERE D.DEPTCODE =X.DEPTCODE ";
                SQL += ComNum.VBLF + "     AND X.PANO = B.PANO";
                SQL += ComNum.VBLF + "     AND WRTNO =  " + strWrtno;

                //'bmd 는 검정여부 상관없이 결과가 있으면 무조건 변관
                if (strXjong.Equals("7"))
                {

                }
                else
                {
                    SQL += ComNum.VBLF + "     AND APPROVE = 'Y' ";
                }
                #endregion


                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                #region 제목줄
                if (strXjong.Equals("E"))
                {
                    New_initFormXray("전기진단 검사 결과지", strXjong);
                }
                else if (strXjong.Equals("6"))
                {
                    New_initFormXray("R I Study Report", strXjong);
                }
                else if (strXjong.Equals("8"))
                {
                }
                else
                {
                    New_initFormXray("방사선 촬영 결과지", strXjong);
                }
                #endregion

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strDrName = dt.Rows[i]["DrCode"].ToString().Trim();

                    if (string.IsNullOrWhiteSpace(strDrName)) strDrName = New_select_Doccode(pDbCon, strWrtno);

                    WriteStr(dt.Rows[i]["Pano"].ToString().Trim(), 25, 250, 245);
                    WriteStr(dt.Rows[i]["DeptNamek"].ToString().Trim(), 25, 250, 295);

                    if (strXjong.Equals("3"))
                    {
                        WriteStr(dt.Rows[i]["sName"].ToString().Trim() + "(" + ComFunc.GetBirthDate(dt.Rows[i]["Jumin1"].ToString().Trim(), dt.Rows[i]["Jumin2"].ToString().Trim(), "-")  + ")" , 25, 740, 245);
                    }
                    else
                    {
                        WriteStr(dt.Rows[i]["sName"].ToString().Trim(), 25, 740, 245);
                    }

                    switch (dt.Rows[i]["DrCode"].ToString().Trim())
                    {
                        case "1199":
                        case "2299":
                            if (string.IsNullOrWhiteSpace(argOrderno) == false)
                            {
                                WriteStr(New_select_xray_drname(pDbCon, strPatid, argOrderno), 25, 740, 295);
                            }
                            else
                            {
                                WriteStr(New_Spec_SelectDocName(pDbCon, dt.Rows[i]["DrCode"].ToString().Trim()), 25, 740, 295);
                            }
                            break;

                        default:
                            WriteStr(New_Spec_SelectDocName(pDbCon, dt.Rows[i]["DrCode"].ToString().Trim()), 25, 740, 295);
                            break;
                    }

                    WriteStr(dt.Rows[i]["agesex"].ToString().Trim(), 25, 1350, 245);
                    WriteStr(dt.Rows[i][strClass.Equals("O") ? "sName" : "Ward"].ToString().Trim(), 25, 1440, 245);
                    WriteStr(Convert.ToDateTime(strEnterdate).ToString("yyyy-MM-dd"), 25, 1350, 295); //검사요청일
                    WriteStr(Convert.ToDateTime(strSeekDate).ToString("yyyy-MM-dd"), 25, 240, 2060); //촬영일자

                    if (strXjong.Equals("7") == false)
                    {
                        //판독일자
                        WriteStr(dt.Rows[i]["ReadDate"].ToString().Trim(), 25, 940, 2060);
                    }

                    if (strXjong.Equals("3"))
                    {
                        WriteStr(READ_PassName(pDbCon, dt.Rows[i]["xdrcode1"].ToString().Trim()) +
                                 "(" + clsVbfunc.GetOCSDoctorDRBUNHO(pDbCon, dt.Rows[i]["xdrcode1"].ToString().Trim()) + ")"
                                 , 25, 1350, 2060);
                    }
                    else
                    {
                        WriteStr(READ_PassName(pDbCon, dt.Rows[i]["xdrcode1"].ToString().Trim()), 25, 1400, 2060);
                    }

                    WriteStr("검사명 : " + dt.Rows[i]["xname"].ToString().Trim(), 25, 90, 450);

                    strTResult = dt.Rows[i]["result"].ToString().Trim() + dt.Rows[i]["RESULT1"].ToString().Trim();


                    if (string.IsNullOrWhiteSpace(string.Concat(dt.Rows[i]["ADDENDUM1"].ToString().Trim(), dt.Rows[i]["ADDENDUM2"].ToString().Trim())) ==false)
                    {
                        strTResult += ComNum.VBLF + ComNum.VBLF + "Addendum Report: " +  ComNum.VBLF + ComNum.VBLF + string.Concat(dt.Rows[i]["ADDENDUM1"].ToString().Trim(), dt.Rows[i]["ADDENDUM2"].ToString().Trim());                        
                    }


                    strTResult = strTResult.Replace(ComNum.VBLF, "\n");
                    strResult = strTResult.Split('\n');

                    if (strResult.Length > 0)
                    {
                        for(int k = 0; k < strResult.Length; k++)
                        {
                            if (string.IsNullOrWhiteSpace(strResult[k]))
                            {
                                lngLine += 1;
                            }
                            else
                            {
                                #region 신규
                                int lastPos = 0;
                                using (Font font = new Font(FontName, 20))
                                {
                                    Size PageSize = new Size(1648 - 190, 20);
                                    Size strWidth = TextRenderer.MeasureText(strResult[k], font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);

                                    if (strWidth.Width < 1100)
                                    {
                                        WriteStr(20, 190, 550 + (lngLine * 35), strResult[k]);
                                        lngLine += 1;
                                    }
                                    else
                                    {
                                        for (int l = 0; l < strResult[k].Length + 1; l++)
                                        {
                                            bool DataOutPut = false;
                                            string strText = strResult[k].Substring(lastPos, l);
                                            strWidth = TextRenderer.MeasureText(strText, font, PageSize, TextFormatFlags.NoPadding | TextFormatFlags.TextBoxControl | TextFormatFlags.WordBreak);
                                            if (strWidth.Width >= 1100)
                                            {
                                                lastPos += l;
                                                l = 0;
                                                WriteStr(20, 190, 550 + (lngLine * 35), strText);
                                                lngLine += 1;
                                                DataOutPut = true;
                                            }


                                            if (DataOutPut && lastPos + l == strResult[k].Length)
                                            {
                                                break;
                                            }

                                            if (DataOutPut == false && (l == strResult[k].Length || lastPos + l == strResult[k].Length))
                                            {
                                                WriteStr(20, 190, 550 + (lngLine * 35), strText);
                                                lngLine += 1;
                                                break;
                                            }
                                        }
                                    }

                                    #region new page
                                    if (lngLine > 37)
                                    {
                                        TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                                        //LNGY = 40;
                                        lngLine = 1;
                                        PageNum += 1;

                                        if (strXjong.Equals("E"))
                                        {
                                            New_initFormXray("전기진단 검사 결과지", strXjong);
                                        }
                                        else if (strXjong.Equals("6"))
                                        {
                                            New_initFormXray("R I Study Report", strXjong);
                                        }
                                        else
                                        {
                                            New_initFormXray("방사선 촬영 결과지", strXjong);
                                        }


                                        WriteStr(dt.Rows[i]["Pano"].ToString().Trim(), 25, 250, 245);
                                        WriteStr(dt.Rows[i]["DeptNamek"].ToString().Trim(), 25, 250, 295);
                                        WriteStr(dt.Rows[i]["sName"].ToString().Trim(), 25, 740, 245);
                                        WriteStr(New_Spec_SelectDocName(pDbCon, dt.Rows[i]["DrCode"].ToString().Trim()), 25, 740, 295);
                                        WriteStr(dt.Rows[i]["agesex"].ToString().Trim(), 25, 1350, 245);
                                        WriteStr(dt.Rows[i][strClass.Equals("O") ? "sName" : "Ward"].ToString().Trim(), 25, 1440, 245);
                                        WriteStr(Convert.ToDateTime(strEnterdate).ToString("yyyy-MM-dd"), 25, 1350, 295); //검사요청일
                                        WriteStr(Convert.ToDateTime(strSeekDate).ToString("yyyy-MM-dd"), 25, 240, 2060); //촬영일자

                                        if (strXjong.Equals("7") == false)
                                        {
                                            //판독일자
                                            WriteStr(dt.Rows[i]["ReadDate"].ToString().Trim(), 25, 940, 2060);
                                        }

                                        if (strXjong.Equals("3"))
                                        {
                                            WriteStr(READ_PassName(pDbCon, dt.Rows[i]["xdrcode1"].ToString().Trim()) +
                                                     "(" + clsVbfunc.GetOCSDoctorDRBUNHO(pDbCon, dt.Rows[i]["xdrcode1"].ToString().Trim()) + ")"
                                                     , 25, 1350, 2060);
                                        }
                                        else
                                        {
                                            WriteStr(READ_PassName(pDbCon, dt.Rows[i]["xdrcode1"].ToString().Trim()), 25, 1400, 2060);
                                        }

                                        WriteStr("검사명 : " + dt.Rows[i]["xname"].ToString().Trim(), 25, 90, 450);
                                    }
                                    #endregion
                                }
                                #endregion


                                #region 이전
                                //for(double j = 1; j <= Math.Truncate(((double) (Encoding.Default.GetBytes(strResult[k]).Length) / 68)  + 1); j++)
                                //{
                                //    //Console.WriteLine(VB.Mid(strResult[k], 1 + 68 * (int)(j - 1), 68));
                                //    WriteStr(VB.Mid(strResult[k],  1 + 68 * (int) (j - 1), 68), 20, 190, 550 + (lngLine * 35));
                                //    lngLine += 1;

                                //    #region new page
                                //    if (lngLine > 37)
                                //    {
                                //        TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                                //        //LNGY = 40;
                                //        lngLine = 1;
                                //        PageNum += 1;

                                //        if (strXjong.Equals("E"))
                                //        {
                                //            New_initFormXray("전기진단 검사 결과지", strXjong);
                                //        }
                                //        else if(strXjong.Equals("6"))
                                //        {
                                //            New_initFormXray("R I Study Report", strXjong);
                                //        }
                                //        else
                                //        {
                                //            New_initFormXray("방사선 촬영 결과지", strXjong);
                                //        }


                                //        WriteStr(dt.Rows[i]["Pano"].ToString().Trim(), 25, 250, 245);
                                //        WriteStr(dt.Rows[i]["DeptNamek"].ToString().Trim(), 25, 250, 295);
                                //        WriteStr(dt.Rows[i]["sName"].ToString().Trim(), 25, 740, 245);
                                //        WriteStr(New_Spec_SelectDocName(pDbCon, dt.Rows[i]["DrCode"].ToString().Trim()), 25, 740, 295);
                                //        WriteStr(dt.Rows[i]["agesex"].ToString().Trim(), 25, 1350, 245);
                                //        WriteStr(dt.Rows[i][strClass.Equals("O") ? "sName" : "Ward"].ToString().Trim(), 25, 1440, 245);
                                //        WriteStr(Convert.ToDateTime(strEnterdate).ToString("yyyy-MM-dd"), 25, 1350, 295); //검사요청일
                                //        WriteStr(Convert.ToDateTime(strSeekDate).ToString("yyyy-MM-dd"), 25, 240, 2060); //촬영일자

                                //        if (strXjong.Equals("7") == false)
                                //        {
                                //            //판독일자
                                //            WriteStr(dt.Rows[i]["ReadDate"].ToString().Trim(), 25, 940, 2060);
                                //        }

                                //        if (strXjong.Equals("3"))
                                //        {
                                //            WriteStr(READ_PassName(pDbCon, dt.Rows[i]["xdrcode1"].ToString().Trim()) +
                                //                     clsVbfunc.GetOCSDoctorDRBUNHO(pDbCon, dt.Rows[i]["xdrcode1"].ToString().Trim())
                                //                     , 25, 1350, 2060);
                                //        }
                                //        else
                                //        {
                                //            WriteStr(READ_PassName(pDbCon, dt.Rows[i]["xdrcode1"].ToString().Trim()), 25, 1400, 2060);
                                //        }

                                //        WriteStr("검사명 : " + dt.Rows[i]["xname"].ToString().Trim(), 25, 90, 450);
                                //    }
                                //    #endregion


                                //}
                                #endregion
                            }
                        }
                    }

                }

                dt.Dispose();

                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message + "\r\n방사선에러", "", pDbCon);
                SaveCvtLog(strPatid, strTREATNO, "방사선에러");
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        public static void New_initFormXray(string str, string strXjong)
        {
            #region 이미지 그리기
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(str, 50, 550, 80);

            WriteStr("=========================================================================================", 25, 50, 200);
            WriteStr("=========================================================================================", 25, 50, 340);

            WriteStr("등록번호:", 25, 80, 245);
            WriteStr("의 뢰 과:", 25, 80, 295);
            WriteStr("성 명 :  ", 25, 570, 245);
            WriteStr("의 사 : ", 25, 570, 295);
            WriteStr("성    별: ", 25, 1140, 245);
            WriteStr("검사요청일: ", 25, 1140, 295);

            WriteStr("촬영일자: ", 25, 60, 2060);

            if (strXjong.Equals("7"))
            {
                WriteStr("방사선사: ", 25, 1230, 2060);
            }
            else
            {
                WriteStr("보고일자: ", 25, 750, 2060);
                WriteStr("검사자: ", 25, 1230, 2060);
            }

            WriteStr("----------------------------------------------------------------------------------------", 25, 50, 2100);

            if (strXjong.Equals("E"))
            {
                //WriteStr("Pohang St.Mary's Hospital.  Physical medicine & Rehabilitation   Tel : 054 -289-4591", 25, 70, 2140);
                WriteStr("* 포항성모병원 재활의학과 *      전화:054-260-8161 ", 25, 70, 2140);
                WriteStr("* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.", 25, 70, 2180);
            }
            else if (strXjong.Equals("6"))
            {
                //WriteStr("Pohang St.Mary's Hospital.  Department of Nuclear Medicine.      Tel : 054 -289-4520", 25, 70, 2140);
                WriteStr("* 포항성모병원 *                전화:054-260-8205 ", 25, 70, 2140);
                WriteStr("* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.", 25, 70, 2180);
            }
            else if (strXjong.Equals("8"))
            {
                WriteStr("Pohang St.Mary's Hospital.  Department of Nuclear Medicine.      Tel : 054 -289-4520", 25, 70, 2140);
            }
            else if (strXjong.Equals("C"))
            {
                WriteStr("* 포항성모병원 심장초음파실 *      전화:054-260-8231 ", 25, 70, 2140);
                WriteStr("* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.", 25, 70, 2180);
            }
            else
            {
                WriteStr("* 포항성모병원 영상의학과 *      전화:054-260-8163 ", 25, 70, 2140);
                if (strXjong.Equals("7") == false)
                {
                    WriteStr("* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.", 25, 70, 2180);
                }
            }
              

            #endregion
        }



        /// <summary>
        /// 전공의 응급실 처방 시 이름 가져오기
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public static string New_select_xray_drname(PsmhDb pDbCon, string Pano, string orderno)
        {
            string rtnVal = string.Empty;
            string SQL = string.Empty;

            SQL = "SELECT B.DRNAME";
            SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_IORDER A, KOSMOS_OCS.OCS_DOCTOR B";
            SQL += ComNum.VBLF + "WHERE PTNO = '" + Pano + "'";
            SQL += ComNum.VBLF + "  AND ORDERNO = " + orderno;
            SQL += ComNum.VBLF + "  AND A.DRCODE = B.SABUN";

            OracleDataReader reader = null;

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();

            return rtnVal;
        }


        /// <summary>
        /// 방사선 테이블 read
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public static string New_select_Doccode(PsmhDb pDbCon, string strWrtno1)
        {
            string rtnVal = string.Empty;
            string SQL = " SELECT drcode  FROM kosmos_pmpa.xray_detail " +
           " where exinfo = '" + strWrtno1 + "' ";

            OracleDataReader reader = null;

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();

            return rtnVal;
        }

        #endregion

        #region 검사지 결과 관련 함수
        /// <summary>
        /// 
        /// </summary>
        /// <param name="var"></param>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="strOutDate"></param>
        /// <param name="strDocCode"></param>
        /// <param name="strTREATNO"></param>
        /// <returns></returns>
        public static bool NEW_spec_SelectSpecNo(PsmhDb pDbCon, string var, string strPatid, string strDate, string strClinCode, string strClass, string strOutDate, string strDocCode, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;

            gstrFormcode = strClass.Trim().Equals("I") ? "111" : "004";
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            PageNum = 0;
            #endregion

            try
            {
                #region 쿼리
                //if (strClass.Equals("O") && strDate.Equals("20041231"))
                //{
                //    SQL = "  SELECT SPECNO, AGE, SEX, GB_GWAEXAM FROM kosmos_ocs.EXAM_SPECMST ";
                //    SQL += ComNum.VBLF + "WHERE pano =  '" + strPatid + "' ";
                //    SQL += ComNum.VBLF + "  AND bdate <= TRUNC(SYSDATE) ";
                //    SQL += ComNum.VBLF + "  AND DEPTCODE ='" + strClinCode + "' ";
                //    SQL += ComNum.VBLF + "  AND IPDOPD ='" + strClass + "' AND STATUS ='05' and DRCODE NOT IN (  '1107','1125' ,'0901','0902','0903') ";
                //}
                //else if (strClass.Equals("O"))
                if (strClass.Equals("O"))
                {
                    SQL = "  SELECT SPECNO, AGE,SEX, GB_GWAEXAM FROM kosmos_ocs.EXAM_SPECMST ";
                    SQL += ComNum.VBLF + "WHERE pano =  '" + strPatid + "' ";
                    if (strClinCode.Equals("ER"))
                    {
                        SQL += ComNum.VBLF + "  AND bdate >=  TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "  AND bdate <=  TO_DATE('" + DateTime.ParseExact(strDate, "yyyyMMdd", null).AddDays(+1).ToString("yyyyMMdd") + "','YYYY-MM-DD') ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "  AND bdate =  TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    }
                    SQL += ComNum.VBLF + "  AND DEPTCODE ='" + strClinCode + "' ";
                    SQL += ComNum.VBLF + "  AND IPDOPD ='" + strClass + "' AND STATUS ='05' ";

                

                }
                else if(strClass.Equals("I"))
                {
                    SQL = "  SELECT SPECNO, AGE,SEX, GB_GWAEXAM FROM kosmos_ocs.EXAM_SPECMST ";
                    SQL += ComNum.VBLF + "WHERE pano =  '" + strPatid + "' ";
                    SQL += ComNum.VBLF + "  AND bdate >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  AND bdate <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "  AND STATUS ='05' ";
                }

                #region 김민원 오동호류마티스내과....
                if (strClinCode.Equals("RA") || strClinCode.Equals("MR"))
                {
                    //if (strClass.Equals("O") && strDate.Equals("20041231"))
                    //{
                    //    SQL = "  SELECT SPECNO, AGE,SEX, GB_GWAEXAM FROM kosmos_ocs.EXAM_SPECMST ";
                    //    SQL += ComNum.VBLF + "WHERE pano =  '" + strPatid + "' ";
                    //    SQL += ComNum.VBLF + "  AND bdate <= TRUNC(SYSDATE) ";
                    //    SQL += ComNum.VBLF + "  AND DEPTCODE IN ( 'MD' ,'MR')  ";
                    //    SQL += ComNum.VBLF + "  AND IPDOPD ='" + strClass + "' AND STATUS ='05' and DRCODE IN ('1107','1125','0901','0902','0903')  ";
                    //}
                    if (strClass.Equals("O"))
                    {
                        SQL = "  SELECT SPECNO, AGE,SEX, GB_GWAEXAM FROM kosmos_ocs.EXAM_SPECMST ";
                        SQL += ComNum.VBLF + " WHERE pano =  '" + strPatid + "' ";
                        SQL += ComNum.VBLF + "   AND bdate = TO_DATE('" + strDate + "','YYYY-MM-DD')  ";
                        SQL += ComNum.VBLF + "   AND DEPTCODE IN ( 'MD','MR')  ";
                        SQL += ComNum.VBLF + "   AND IPDOPD ='" + strClass + "' AND STATUS ='05'  and DRCODE IN ( '1107','1125','0901','0902','0903')   ";
                    }
                    else if (strClass.Equals("I"))
                    {
                        SQL = "  SELECT SPECNO, AGE,SEX, GB_GWAEXAM FROM kosmos_ocs.EXAM_SPECMST ";
                        SQL += ComNum.VBLF + "WHERE pano =  '" + strPatid + "' ";
                        SQL += ComNum.VBLF + "  AND bdate >= TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "  AND bdate <= TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "  AND STATUS ='05'    ";
                    }
                }
                #endregion


                SQL += ComNum.VBLF + "  AND  ANATNO IS NULL ";// '允(2005-10-25)
                SQL += ComNum.VBLF + " ORDER BY SPECNO ";

                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    return rtnVal;
                }

                SaveCvtLog(strPatid, strTREATNO, "검사결과");

                while (reader.Read())
                {
                    PageNum += 1;
                    NEW_SPEC(pDbCon, strPatid.Trim(), strDate.Trim(), strClinCode.Trim(), strClass.Trim(), reader.GetValue(0).ToString().Trim(),
                        reader.GetValue(1).ToString().Trim(),
                        reader.GetValue(2).ToString().Trim(),
                        strTREATNO.Trim());
                }

                reader.Dispose();

                rtnVal = true;
            }
            catch(Exception ex)
            {
                SaveCvtLog(strPatid, strTREATNO, "검사결과에러");
                clsDB.SaveSqlErrLog(ex.Message + "\r\n검사결과에러", "", pDbCon);
            }

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPatid"></param>
        /// <param name="strDate"></param>
        /// <param name="strClinCode"></param>
        /// <param name="strClass"></param>
        /// <param name="strSpecNo"></param>
        /// <param name="strAge"></param>
        /// <param name="strSex"></param>
        /// <param name="strTREATNO"></param>
        /// <returns></returns>
        public static bool NEW_SPEC(PsmhDb pDbCon, string strPatid, string strDate, string strClinCode, string strClass, string strSpecNo, string strAge, string strSex, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            DataTable dt = null;
            OracleDataReader reader = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            //int nWsCNT = 0;
            string nResultSabun  = string.Empty;
            string strSpecCode   = string.Empty;
            string strWS = string.Empty;
            //int nPrintCNT = 0;


            string strPano        = string.Empty;
            string strSName = string.Empty;
            int nAge = (int) VB.Val(strAge);
            string strDeptCode = string.Empty;
            string strDeptName    = string.Empty;
            string strDrName      = string.Empty;
            string strWARD        = string.Empty;
            string strIpdOpd = string.Empty;


            string strSpecName = string.Empty;
            string strWsName    = string.Empty;
            //int nSpecPrtCNT = 0;// '종전까지 결과지 인쇄횟수


            string strMasterCode   = string.Empty;
            string strSubCode      = string.Empty;
            string strExamName = string.Empty;
            int nFootNoteCNT      = 0;
            string strFootNote      = string.Empty;
            string strResult        = string.Empty;
            string strStatus        = string.Empty;
            string strSeqNo         = string.Empty;
            string strRPD           = string.Empty;
            string strRef           = string.Empty;
            string strResultName    = string.Empty;
            string strNewTime       = string.Empty;//  '최종작업자 사번 READ용
            string strUnit = string.Empty;
            string sRefValFrom     = string.Empty;
            string sRefValTo = string.Empty;
            string strList = string.Empty;

            string[] strRefVal;

            int LNGY = 40;
            int lngLine = 1;

            List<string> lstFootNote = new List<string>();
            #endregion

            try
            {
                #region Result_Print_Specno_NEW_SUB1(검체번호별 검사결과,FootNote,결과사번을 READ)
                SQL = " SELECT M.ExamName, R.SeqNo,R.MasterCode, R.SubCode, R.Result, ";
                SQL += ComNum.VBLF + " R.Status, R.Refer, R.Unit, R.ResultSaBun, ";
                SQL += ComNum.VBLF + " TO_CHAR(R.ResultDate,'YYYY-MM-DD HH24:MI') ResultTime, ";
                SQL += ComNum.VBLF + " TO_CHAR(R.ResultDate,'YYYY-MM-DD') Rdate ";
                SQL += ComNum.VBLF + " FROM kosmos_ocs.Exam_ResultC R, kosmos_ocs.Exam_Master M ";
                SQL += ComNum.VBLF + " WHERE R.SpecNo= '" + strSpecNo + "' ";
                SQL += ComNum.VBLF + " AND R.SubCode = M.MasterCode ";
                SQL += ComNum.VBLF + " ORDER BY R.SeqNo ";


                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                New_initForm("", " 포 항 성 모 병 원");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strMasterCode = dt.Rows[i]["MASTERCODE"].ToString().Trim();
                    strSubCode = dt.Rows[i]["subcode"].ToString().Trim();
                    strExamName = dt.Rows[i]["examname"].ToString().Trim();
                    strResult = dt.Rows[i]["result"].ToString().Trim();
                    strStatus = dt.Rows[i]["Status"].ToString().Trim();
                    strSeqNo = dt.Rows[i]["SEQNO"].ToString().Trim();
                    strUnit = dt.Rows[i]["Unit"].ToString().Trim();


                    #region 서브 쿼리

                    lstFootNote.Clear();

                    SQL = "   SELECT FootNote FROM kosmos_ocs.Exam_ResultCf ";
                    SQL += ComNum.VBLF + " WHERE SpecNo= '" + strSpecNo + "'";
                    SQL += ComNum.VBLF + "   AND SeqNo =  '" + VB.Val(strSeqNo.Trim()) + "' order by sort";

                    SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        return rtnVal;
                    }

                    nFootNoteCNT = 0;

                    if (reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            nFootNoteCNT += 1;
                            lstFootNote.Add(reader.GetValue(0).ToString().Trim());
                        }
                    }

                    reader.Dispose();
                    #endregion

                    #region 결과가 있거나 검사항목이 Head성 이거나 Foot Note가 있다면
                    if (!string.IsNullOrWhiteSpace(strResult) || strStatus.Equals("H") || nFootNoteCNT > 0 || strMasterCode.Equals(strSubCode))
                    {
                        //Reference 결과(H:높음, L:낮음)
                        strRPD = dt.Rows[i]["refer"].ToString().Trim();
                        strRef = NEW_Reference(pDbCon, strSubCode, VB.Format(nAge, "#0"), strSex, dt.Rows[i]["Rdate"].ToString().Trim());

                        #region 참고치를 조회하여 출력시에 "H" 또는 "L"를 표시(2002,1,16추가)
                        if (!string.IsNullOrWhiteSpace(strRef) && string.IsNullOrWhiteSpace(strRPD.Trim()))
                        {
                            strRefVal = strRef.Split('~');
                            if (strRefVal.Length > 0)
                            {
                                sRefValFrom = strRefVal[0].Trim();
                                if (strRefVal.Length == 2)
                                {
                                    sRefValTo = strRefVal[1].Trim();
                                }
                                else
                                {
                                    sRefValTo = string.Empty;
                                }

                                if (VB.Val(strResult) < VB.Val(sRefValFrom))
                                {
                                    strRPD = "L";
                                }
                                else if (VB.Val(strResult) > VB.Val(sRefValTo))
                                {
                                    strRPD = "R";
                                }
                                else
                                {
                                    strRPD = string.Empty;
                                }
                                //참고치를 초과하거나 미달인 검사수치의 "H","L"표시를 EXAM_RESULTC에 저장
                            }
                        }
                        #endregion


                        #region 결과일시가 마지막인 사번을 찾음
                        if (VB.Val(dt.Rows[i]["ResultSabun"].ToString().Trim()) > 0)
                        {
                            if (dt.Rows[i]["ResultTime"].ToString().Trim().Length > 0)
                            {
                                if (strNewTime.Length > 0 && DateTime.Compare(Convert.ToDateTime(dt.Rows[i]["ResultTime"].ToString().Trim()), Convert.ToDateTime(strNewTime)) > 0 ||
                                string.IsNullOrWhiteSpace(strNewTime))
                                {
                                    nResultSabun = dt.Rows[i]["ResultSabun"].ToString().Trim(); //검사자 사번
                                    strNewTime = dt.Rows[i]["ResultTime"].ToString().Trim();
                                }
                            }
                        }
                        #endregion


                        if (VB.Left(strExamName, 1).Equals(" ") && string.IsNullOrWhiteSpace(strResult.Trim()))
                        {
                        }
                        else
                        {
                            #region NEW PAGE
                            if ((lngLine % 37) == 0 || LNGY >= 1700)
                            {
                                if ((lngLine - 1) % 3 == 0)
                                {
                                    //hline(52, 345 + LNGY);
                                    vline(53, 363, 345 + LNGY - 19);
                                    vline(1598, 363, 345 + LNGY - 19);
                                }
                                else
                                {
                                    hline(52, 345 + LNGY);
                                    vline(53, 363, 345 + LNGY);
                                    vline(1598, 363, 345 + LNGY);
                                }

                                LNGY = 40;
                                lngLine = 1;

                                if (Result_Print_Specno_NEW_SUB2(pDbCon, strDate, strClinCode, strSpecNo, nResultSabun, strPatid, strAge, strSex, i, ref LNGY, strTREATNO) == false)
                                {
                                    return rtnVal;
                                }

                                New_initForm(strWsName, " 포 항 성 모 병 원");

                                PageNum += 1;
                            }
                            #endregion

                            WriteStr(ComFunc.LeftH(strExamName, 25), 21, 80, 345 + LNGY);
                            WriteStr(strResult, 28, 460, 345 + LNGY);
                            WriteStr(strRPD, 29, 790, 345 + LNGY);
                            WriteStr(strUnit.ToUpper().Equals("NONE") ? string.Empty : strUnit, 30, 930, 345 + LNGY);
                            WriteStr(strRef, 31, 1300, 345 + LNGY);

                            if ((lngLine % 3) == 0) 
                            {
                                hline(52, 345 + LNGY + 40);
                                LNGY += 20;
                            }

                            LNGY += 40;
                            lngLine += 1;

                            if (lstFootNote.Count > 0)
                            {
                                for(int ss = 0; ss < lstFootNote.Count; ss++)
                                {
                                    WriteStr(lstFootNote[ss], 28, 200, 345 + LNGY);

                                    if (lngLine > 0 && (lngLine % 3) == 0)
                                    {
                                        hline(52, 345 + LNGY + 40);
                                        LNGY += 20;
                                    }

                                    LNGY += 40;
                                    lngLine += 1;
                                }
                            }
                        }
                    }
                    #endregion

                }

                dt.Dispose();
                #endregion

                if (((lngLine - 1) % 3) == 0)
                {
                    //hline(52, 345 + LNGY);
                    vline(53, 363, 345 + LNGY - 19);
                    vline(1598, 363, 345 + LNGY - 19);
                }
                else
                {
                    hline(52, 345 + LNGY);
                    vline(53, 363, 345 + LNGY);
                    vline(1598, 363, 345 + LNGY);
                }

                LNGY = 40;
                if (Result_Print_Specno_NEW_SUB2(pDbCon, strDate, strClinCode, strSpecNo, nResultSabun, strPatid, strAge, strSex, 0, ref LNGY, strTREATNO) == false)
                {
                    return rtnVal;
                }

                rtnVal = true;
            }
            catch(Exception ex)
            {
                SaveCvtLog(strPatid, strTREATNO, "검사결과에러");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        public static bool Result_Print_Specno_NEW_SUB2(PsmhDb pDbCon, string strDate, string strClinCode, string strSpecNo, string nResultSabun, string strPatid, string strAge, string strSex, int j, ref int LNGY, string strTREATNO)
        {
            #region 변수
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            #endregion

            try
            {
                SQL = " SELECT A.Pano, B.SNAME SNAME, A.Ward, A.Room, A.DeptCode,";
                SQL += ComNum.VBLF + " A.DrCode, A.Age, A.Sex, A.WorkSts, A.SpecCode, A.IpdOpd, ";
                SQL += ComNum.VBLF + " A.DeptCode, A.DrCode, TO_CHAR(A.BloodDate,'YYMMDD-HH24:MI') BloodDate, ";
                SQL += ComNum.VBLF + " TO_CHAR(A.ResultDate,'YYMMDD-HH24:MI') ResultDate, ";
                SQL += ComNum.VBLF + " TO_CHAR(A.ReceiveDate,'YYMMDD-HH24:MI') ReceiveDate, A.Print, ";
                SQL += ComNum.VBLF + " A.GB_GWAEXAM ";
                SQL += ComNum.VBLF + " From kosmos_ocs.EXAM_SPECMST A, KOSMOS_PMPA.BAS_PATIENT B";
                SQL += ComNum.VBLF + " WHERE SpecNo = '" + strSpecNo + "'";
                SQL += ComNum.VBLF + "   AND A.PANO = B.PANO";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return rtnVal;
                }

                #region Work Station을 READ
                string strWS = dt.Rows[0]["WorkSts"].ToString().Trim();
                string[] strWardval = strWS.Split(',');

                string strGb_GwaExam = dt.Rows[0]["Gb_GwaExam"].ToString().Trim();

                int nWsCNT = strWardval.Length;

                string strWsName = string.Empty;

                for(int nWardcount = 0; nWardcount < nWsCNT; nWardcount++)
                {
                    strWsName += New_Spec_SelectWardName(pDbCon, strWardval[nWardcount]) + ",";
                }

                if (!string.IsNullOrWhiteSpace(strWsName))
                {
                    //'마지막 컴마를 제거
                    strWsName = VB.Left(strWsName, strWsName.Length - 1) + " 검사";
                }

                //'등록번호,성명,나이,성별
                string strPano = dt.Rows[0]["Pano"].ToString().Trim();
                string strSName = dt.Rows[0]["sName"].ToString().Trim();


                //'병동,진료과,진료의사
                string strDeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();
                string strDrName = New_Spec_SelectDocName(pDbCon, dt.Rows[0]["DrCode"].ToString().Trim());
                if (strDeptCode.Equals("EM") || strDeptCode.Equals("ER")) strDrName = "응급실";
                string strWARD = dt.Rows[0]["Ward"].ToString().Trim() + " - " + VB.Val(dt.Rows[0]["Room"].ToString().Trim()).ToString("000");
                if (dt.Rows[0]["IpdOpd"].ToString().Trim().Equals("O")) strWARD = "외래";

                string strDeptName = New_Spec_SelectDeptName(pDbCon, dt.Rows[0]["DeptCode"].ToString().Trim());

                string strList = " 번호:" + strSpecNo;
                strList += "채취일시:" + dt.Rows[0]["BloodDate"].ToString().Trim();
                strList += "보고일시:" + dt.Rows[0]["RESULTDATE"].ToString().Trim();

                #endregion

                #region 검체명,접수일시,검사자
                string strSpecCode = dt.Rows[0]["SpecCode"].ToString().Trim();
                string strSpecName = New_READ_BasCode(pDbCon, strSpecCode);//     '검체명
                string strResultName = "검사실";//   '기본을 검사실로 함

                if (strGb_GwaExam.Equals("Y"))
                {
                    strResultName = New_Spec_SelectDocName(pDbCon, dt.Rows[0]["DrCode"].ToString().Trim());
                }
                else
                {
                    nResultSabun = string.IsNullOrWhiteSpace(nResultSabun) ? "0" : nResultSabun;
                    if (VB.Val(nResultSabun) > 0)
                    {
                        strResultName = READ_PassName(pDbCon,VB.Val(nResultSabun).ToString("00000"));
                    }

                    if (strResultName.Equals("양선문"))
                    {
                        strResultName += "  " + "Dr:YSM";
                    }
                    else if(strResultName.Equals("은상진"))
                    {
                        strResultName += "  " + "Dr:ESJ"; //2018-10-25
                    }
                }
                #endregion

                WriteStr(strPatid, 23, 250, 175);
                WriteStr(strWARD, 23, 250, 244);
                WriteStr(strSName, 23, 740, 175);
                WriteStr(strDeptName, 23, 740, 244);
                WriteStr(strAge + "/" + strSex, 23, 1310, 175);
                WriteStr(strDrName, 23, 1310, 244);


                WriteStr(strSpecNo, 23, 180, 2080);
                WriteStr(strSpecName, 23, 180, 2140);
                WriteStr(dt.Rows[0]["BloodDate"].ToString().Trim(), 23, 740, 2080);
                WriteStr(dt.Rows[0]["RECEIVEDATE"].ToString().Trim(), 23, 740, 2140);
                WriteStr(dt.Rows[0]["RESULTDATE"].ToString().Trim(), 23, 1310, 2080);
                WriteStr(strResultName, 23, 1280, 2140);


                WriteStr(strWsName, 50, 650, 80);

                if (dt.Rows.Count > j)
                {
                    LNGY = 40;
                }

                dt.Dispose();

                SQL = "UPDATE KOSMOS_OCS.EXAM_SPECMST    SET EMR = '1'  WHERE SPECNO= '" + strSpecNo + "'";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    return rtnVal;
                }

                //mBitmap.Save(@"C:\Users\2016-2360\Desktop\새 폴더\" + PageNum.ToString("0000") + ".tif", ImageFormat.Tiff);
                TifSave(strPath + strPatid + "_" + strDate + "_" + strClinCode + "_" + gstrFormcode + "_" + PageNum.ToString("0000") + ".tif");

                rtnVal = true;

            }
            catch (Exception ex)
            {
                SaveCvtLog(strPatid, strTREATNO, "검사결과에러");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                //ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

       

        public static string New_READ_BasCode(PsmhDb pDbCon, string strCode)
        {
            string rtnVal = string.Empty;
            string SQL = " SELECT NAME  FROM kosmos_ocs.EXAM_SPECODE" +
           " WHERE Gubun = '14' " +
           " AND DelDate IS NULL " +
           "  AND CODE ='" + strCode + "' ";

            OracleDataReader reader = null;

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();

            return rtnVal;
        }

        public static string New_Spec_SelectDocName(PsmhDb pDbCon, string strDRCode)
        {
            string rtnVal = string.Empty;
            string SQL = " SELECT DRNAME FROM kosmos_ocs.OCS_DOCTOR " +
                         "  WHERE DRCODE = '" + strDRCode + "' ";

            OracleDataReader reader = null;

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();

            return rtnVal;
        }

        public static string New_Spec_SelectBasDocName(PsmhDb pDbCon, string strDRCode)
        {
            string rtnVal = string.Empty;
            string SQL = " SELECT DRNAME FROM KOSMOS_PMPA.BAS_DOCTOR " +
                         "  WHERE DRCODE = '" + strDRCode + "' ";

            OracleDataReader reader = null;

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();

            return rtnVal;
        }

        public static string New_Spec_SelectDeptName(PsmhDb pDbCon, string strDeptCode)
        {
            string rtnVal = string.Empty;
            string SQL = " SELECT DeptNameK FROM Kosmos_Pmpa.Bas_ClinicDept "+
          " WHERE DeptCode = '" + strDeptCode + "' ";

            OracleDataReader reader = null;

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();

            return rtnVal;
        }

        public static string New_Spec_SelectWardName(PsmhDb pDbCon, string strWardname)
        {
            string rtnVal = string.Empty;
            string SQL = "   SELECT Name FROM kosmos_ocs.Exam_Specode " +
                         " WHERE GuBun = '12' " +
                         " AND YName = '" + (strWardname).Trim() + "' " +
                         " AND Code LIKE '%0' ";

            OracleDataReader reader = null;

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox(SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                rtnVal = reader.GetValue(0).ToString().Trim();
            }

            reader.Dispose();

            return rtnVal;
        }

        /// <summary>
        /// 참고치 가져오기
        /// </summary>
        /// <param name="code"></param>
        /// <param name="Age"></param>
        /// <param name="Sex"></param>
        /// <param name="argRdate"></param>
        /// <returns></returns>
        public static string NEW_Reference(PsmhDb pDbCon, string code, string Age, string Sex, string argRdate)
        {
            #region 변수
            string strClass = string.Empty;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            OracleDataReader reader = null;
            #endregion

            SQL = "     SELECT Normal, RefvalFrom, RefvalTo ";
            SQL += ComNum.VBLF + " FROM kosmos_ocs.Exam_Master_Sub ";
            SQL += ComNum.VBLF + "WHERE Gubun = '41' ";//   '41:Reference Value
            SQL += ComNum.VBLF + "  AND MasterCode = '" + (code).Trim() + "' ";
            SQL += ComNum.VBLF + "  AND (Sex IS NULL OR Sex = ' ' OR Sex='" + (Sex).Trim() + "') ";
            SQL += ComNum.VBLF + "  AND ((AgeFrom = 0 And AgeTo = 99) ";
            SQL += ComNum.VBLF + "   OR  (AgeFrom <= " + VB.Val(Age) + " AND AgeTo >= " + VB.Val(Age) + ")) ";
            //'2019-09-03 참고치 유효기간 생김. 인자값에 argRdate 추가함.
            SQL += ComNum.VBLF + "  AND  ((EXPIREDATE IS NOT NULL AND EXPIREDATE >= TO_DATE('" + argRdate + "','YYYY-MM-DD')) OR (EXPIREDATE IS NULL))";
            SQL += ComNum.VBLF + "     ORDER BY EXPIREDATE";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (string.IsNullOrWhiteSpace(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                ComFunc.MsgBox( SqlErr);
                return rtnVal;
            }

            if (reader.HasRows && reader.Read())
            {
                if (reader.GetValue(0).ToString().Trim().Length > 0)
                {
                    rtnVal = reader.GetValue(1).ToString().Trim();
                }
                else
                {
                    rtnVal = reader.GetValue(1).ToString().Trim() + " ~ " + reader.GetValue(2).ToString().Trim();
                }
            }

            reader.Dispose();

            return rtnVal;
        }

        public static void New_initForm(string str, string str1)
        {
            #region 이미지 그리기
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }

            mBitmap = new Bitmap(1648, 2327);
            mGraphics = Graphics.FromImage(mBitmap);
            mGraphics.Clear(Color.White);

            WriteStr(str, 50, 650, 80);

            #region Line
            RectLine(53, 165, 1545, 115);
            #endregion

            WriteStr("등록번호 : ", 23,  80, 175);
            WriteStr("병    동 :   ", 23, 80, 244);
            WriteStr("이   름 :  ", 23, 570, 175);
            WriteStr("진 료 과 :  ", 23, 570, 244);
            WriteStr("나  이 : ", 23, 1140, 175);
            WriteStr("의  사 : ", 23, 1140, 244);

            #region Line
            vline(350, 363, 308);
            vline(730, 363, 308);
            vline(880, 363, 308);
            vline(1190, 363, 308);
            RectLine(53, 308, 1545, 55);
            #endregion

            WriteStr("검사명", 30, 120, 315);
            WriteStr("검사결과", 28, 450, 315);
            WriteStr("R", 29, 790, 315);
            WriteStr("단  위 ", 30, 980, 315);
            WriteStr("참고치", 31, 1340, 315);

            #region Line
            RectLine(53, 2070, 1545, 105);
            #endregion


            WriteStr("번호: ", 23, 80, 2080);
            WriteStr("검체: ", 23, 80, 2140);
            WriteStr("채취일시 : ", 23, 570, 2080);
            WriteStr("접수일시 : ", 23, 570, 2140);
            WriteStr("보고일시 : ", 23, 1140, 2080);
            WriteStr("검사자 : ", 23, 1140, 2140);

            WriteStr(" ***   본 검사실은 대한진단검사의학과의 우수 검사실 신임 인증을 받은 검사실 입니다. ", 23, 80, 2180);
            WriteStr(" ***   우(37661)  경상북도  포항시  남구 대잠동길 17 Tel: (054) 260-8258        ", 23, 80, 2210);
            WriteStr(str1, 40, 500, 2240);
              

            #endregion
        }
        #endregion

        #region Line 

        public static void RectLine(int x, int y, int width, int Height, float pentWidth = 3)
        {
            using (Pen pen = new Pen(Brushes.Black, pentWidth))
            {
                Rectangle rect = new Rectangle(x, y, width, Height);
                mGraphics.DrawRectangle(pen, rect);
            }
        }

        public static Rectangle Set_Rect(int x, int y, int width, int Height, float pentWidth = 3, string BrushColor = "Black")
        {
            Rectangle rect = new Rectangle(x, y, width, Height);
            using (Pen pen = new Pen(BrushColor.Equals("Black") ? Color.Black : Color.White, pentWidth))
            {
                mGraphics.DrawRectangle(pen, rect);
            }

            return rect;
        }

        public static void vline(int x, int y, int y2)
        {
            using (Pen pen = new Pen(Brushes.Black, 3))
            {
                mGraphics.DrawLine(pen, x, y, x, y2);
            }
        }

        public static void vline2(int x, int y, long lastY)
        {
            long line = (lastY - 345) / 22;

            using (Pen pen = new Pen(Brushes.Black, 5))
            {
                for (int i = 0; i < line; i++)
                {
                    y += 22;
                }

                mGraphics.DrawLine(pen, x, lastY - 22, x, y);
            }
        }

        public static void hline(int x, int y)
        {
            using (Pen pen = new Pen(Brushes.Black, 2))
            {
                mGraphics.DrawLine(pen, x, y, 1648 - x, y);
            }
        }

        public static void hline2(int x, int y, int x2)
        {
            using (Pen pen = new Pen(Brushes.Black, 2))
            {
                mGraphics.DrawLine(pen, x, y, x2, y);
            }
        }
        #endregion

        public static string Trim(string str)
        {
            return str.Trim();
        }

        #region Print String

        public static void WriteImage(Image img, Rectangle rect)
        {
            mGraphics.DrawImage(img, rect);
        }

        public static void WriteStr(float fontSize, bool Bold, string str, Rectangle rect, StringAlignment stringAlignment = StringAlignment.Center)
        {
            using (Font font = new Font(FontName, fontSize, Bold ? FontStyle.Bold : FontStyle.Regular))
            {
                //extOut(FontFaceName, Size As Integer, x As Long, y As Long, TextColor As OLE_COLOR, BackColor As OLE_COLOR, Text)
                using (StringFormat stringFormat = new StringFormat())
                {
                    stringFormat.Alignment = stringAlignment;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    mGraphics.DrawString(str, font, Brushes.Black, rect, stringFormat);
                }
            }
        }

        public static void WriteStr(float fontSize, int x, int y, string str)
        {
            using (Font font = new Font(FontName, fontSize))
            {
                //extOut(FontFaceName, Size As Integer, x As Long, y As Long, TextColor As OLE_COLOR, BackColor As OLE_COLOR, Text)
                using (StringFormat stringFormat = new StringFormat())
                {
                    mGraphics.DrawString(str, font, Brushes.Black, x, y, stringFormat);
                }
            }
        }

        public static void WriteStr(string str, float fontSize, int x, int y)
        {
            using (Font font = new Font(FontName, fontSize))
            {
                //extOut(FontFaceName, Size As Integer, x As Long, y As Long, TextColor As OLE_COLOR, BackColor As OLE_COLOR, Text)
                using (StringFormat stringFormat = new StringFormat())
                {
                    mGraphics.DrawString(str, font, Brushes.Black, x, y, stringFormat);
                }
            }
        }
        #endregion

        #region NEW_PohangTreatInterface
        public static bool NEW_PohangTreatInterface(PsmhDb pDbCon, Form msgForm, string strPatid)
        {
            #region 변수
            bool rtnVal = false;
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int RowAffected = 0;
            OracleDataReader reader = null;
            OracleDataReader reader2 = null;
            #endregion

            #region 외래
            clsDB.setBeginTran(pDbCon);
            try
            {
                #region 쿼리
                SQL = " SELECT P.PANO, P.SNAME, P.SEX, P.JUMIN1||P.JUMIN2  JUMIN, E.PATID , E.ROWID " +
                      "   FROM KOSMOS_PMPA.BAS_PATIENT  P , KOSMOS_EMR.EMR_PATIENTT E" +
                      " WHERE E.PATID (+)=P.PANO AND " +
                      "  P.PANO ='" + strPatid.Trim() + "' ";
                #endregion

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBoxEx(msgForm, SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    clsDB.setRollbackTran(pDbCon);
                    return rtnVal;
                }


                if (reader.Read())
                {
                    #region EMR_PATIENTT 테이블에 환자가 없다.
                    if (reader.GetValue(4).ToString().Trim().Length == 0)
                    {
                        SQL = "INSERT INTO KOSMOS_EMR.EMR_PATIENTT(PATID, JUMINNO, NAME, SEX  ) " + " " +
                              " VALUES('" + reader.GetValue(0).ToString().Trim() + "' ," +
                              "'" + VB.Left(reader.GetValue(3).ToString().Trim(), 7) + "******" + "', " +
                              "'" + reader.GetValue(1).ToString().Trim() + "', " +
                              "'" + reader.GetValue(2).ToString().Trim() + "', " +
                              " ) ";
                    }
                    else
                    {
                        SQL = "UPDATE KOSMOS_EMR.EMR_PATIENTT" + " ";
                        SQL += ComNum.VBLF + "  SET NAME = '" + reader.GetValue(1).ToString().Trim() + "'";
                        SQL += ComNum.VBLF + "    , SEX  = '" + reader.GetValue(2).ToString().Trim() + "'";
                        SQL += ComNum.VBLF + "    , JUMINNO = '" + VB.Left(reader.GetValue(3).ToString().Trim(), 7) + "******" + "' ";
                        SQL += ComNum.VBLF + "  WHERE ROWID = '" + reader.GetValue(5).ToString().Trim() + "' ";
                    }

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        ComFunc.MsgBoxEx(msgForm, SqlErr);
                        return rtnVal;
                    }
                    #endregion
                }

                reader.Dispose();

                #region 진료정보 가져오기 외래 입원 따로 가져오기.
                SQL = "SELECT m.pano, TO_CHAR(M.BDATE, 'YYYYMMDD') Bdate ,m.deptcode, d.sabun, M.ROWID \r\n" +
                      " from kosmos_pmpa.opd_master m,  kosmos_ocs.VIEW_ocs_doctor_NEW d \r\n" +
                      "  where d.drcode = m.drcode AND M.BDATE >= TO_DATE('2005-01-01', 'YYYY-MM-DD') \r\n" +
                      "  and  PANO = '" + strPatid.Trim() + "'\r\n" +
                      "  AND EMR ='0'";

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBoxEx(msgForm, SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string strDept = reader.GetValue(2).ToString().Trim();

                        #region 서브쿼리
                        SQL = "SELECT TREATNO, ROWID  FROM KOSMOS_EMR.EMR_TREATT ";
                        SQL += ComNum.VBLF + "  WHERE PATID = '"  + reader.GetValue(0).ToString().Trim() + "' ";
                        SQL += ComNum.VBLF + "    AND INDATE  ='" + reader.GetValue(1).ToString().Trim() + "'";
                        SQL += ComNum.VBLF + "    AND CLINCODE = '" + strDept + "'";
                        SQL += ComNum.VBLF + "    AND CLASS = 'O'";

                        SqlErr = clsDB.GetAdoRs(ref reader2, SQL, pDbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.setRollbackTran(pDbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            ComFunc.MsgBoxEx(msgForm, SqlErr);
                            return rtnVal;
                        }

                        if (reader2.HasRows == false)
                        {
                            SQL = "INSERT INTO KOSMOS_EMR.EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                            SQL += ComNum.VBLF + " ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED";
                            SQL += ComNum.VBLF + " ) values(KOSMOS_EMR.SEQ_TREATNO.NEXTVAL, '" + (strPatid).Trim() + "' ,";
                            SQL += ComNum.VBLF + "'O' ,";//  'CLASS;
                            SQL += ComNum.VBLF + "'" + reader.GetValue(1).ToString().Trim() + "' ,";// 'INDATE
                            SQL += ComNum.VBLF + "'" + strDept + "' ,";// 'CLINCODE
                            SQL += ComNum.VBLF + "'' ,";//                          'OUTDATE
                            SQL += ComNum.VBLF + "'" + VB.Val(reader.GetValue(3).ToString().Trim()) + "',  ";// 'DOCCODE
                            SQL += ComNum.VBLF + "'0',  ";//                         'ERFLAG
                            SQL += ComNum.VBLF + "'000000',  ";//                       'INITTIME
                            SQL += ComNum.VBLF + "'" + (strPatid).Trim() + "',  ";//     'OLDPATID
                            SQL += ComNum.VBLF + "'2',  ";//      'FST
                            SQL += ComNum.VBLF + "'',  ";//                           'WARD
                            SQL += ComNum.VBLF + "'', ";//                            'ROOM
                            SQL += ComNum.VBLF + "'1' )";//                          'COMPLETE
                        }
                        else
                        {
                            if (reader2.Read())
                            {
                                SQL = " UPDATE KOSMOS_EMR.EMR_TREATT SET ";
                                SQL += ComNum.VBLF + "  DOCCODE = '" + VB.Val(reader.GetValue(3).ToString().Trim()) + "'";
                                SQL += ComNum.VBLF + "  WHERE ROWID = '" + reader2.GetValue(1).ToString().Trim() + "' ";
                            }
                            
                        }
                        reader2.Dispose();

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.setRollbackTran(pDbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            ComFunc.MsgBoxEx(msgForm, SqlErr);
                            return rtnVal;
                        }
                        #endregion

                        #region ocs서버 업데이트. 적용시점에 새로 시작
                        SQL = " UPDATE kosmos_pmpa.opd_master SET   EMR = '1' WHERE ROWID = '" + reader.GetValue(4).ToString().Trim() + "' ";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                        if (string.IsNullOrWhiteSpace(SqlErr) == false)
                        {
                            clsDB.setRollbackTran(pDbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            ComFunc.MsgBoxEx(msgForm, SqlErr);
                            return rtnVal;
                        }
                        #endregion
                    }
                }

                reader.Dispose();
                #endregion

                clsDB.setCommitTran(clsDB.DbCon);

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, "", pDbCon);
                ComFunc.MsgBoxEx(msgForm, ex.Message);
                return rtnVal;
            }
            #endregion


            #region 입원
            clsDB.setBeginTran(pDbCon);
            try
            {
                string strOK = string.Empty;

                SQL = " SELECT  S.PANO, TO_CHAR(S.INDATE, 'YYYYMMDD') INDATE,  TO_CHAR(S.OUTDATE, 'YYYYMMDD') OUTDATE, S.TDEPT, S.ROWID,  D.SABUN ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_PMPA.MID_SUMMARY S,  kosmos_ocs.VIEW_ocs_doctor_NEW d ";
                SQL += ComNum.VBLF + "  WHERE S.TDOCTOR = d.drcode ";
                SQL += ComNum.VBLF + "    AND S.PANO = '" + strPatid + "' ";
                SQL += ComNum.VBLF + "    AND (S.EMR = '0'  OR EMR IS NULL)";// '나중에 적용

                SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.setRollbackTran(pDbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    ComFunc.MsgBoxEx(msgForm, SqlErr);
                    return rtnVal;
                }

                if (reader.HasRows == false)
                {
                    reader.Dispose();
                    //clsDB.setRollbackTran(pDbCon);
                    clsDB.setCommitTran(pDbCon);
                    return rtnVal;
                }

                while (reader.Read())
                {
                    strOK = "OK";

                    if (reader.GetValue(5).ToString().Trim().Length == 0)
                    {
                        strOK = "NO";
                    }

                    string strDept = reader.GetValue(3).ToString().Trim();


                    #region 서브 쿼리
                    SQL = "SELECT TREATNO, ROWID  FROM KOSMOS_EMR.EMR_TREATT ";
                    SQL += ComNum.VBLF + " WHERE PATID  = '" + strPatid + "'";
                    SQL += ComNum.VBLF + "   AND INDATE = '" + reader.GetValue(1).ToString().Trim() + "'";
                    SQL += ComNum.VBLF + "   AND CLASS  = 'I'";

                    SqlErr = clsDB.GetAdoRs(ref reader2, SQL, pDbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        ComFunc.MsgBoxEx(msgForm, SqlErr);
                        return rtnVal;
                    }

                    if (reader2.HasRows == false)
                    {
                        SQL = "INSERT INTO KOSMOS_EMR.EMR_TREATT(TREATNO, PATID, CLASS, INDATE, CLINCODE, OUTDATE, DOCCODE, ";
                        SQL += ComNum.VBLF + " ERFLAG, INTIME, OLDPATID, FSTFLAG, WARD, ROOM, COMPLETED ) ";
                        SQL += ComNum.VBLF + " VALUES(KOSMOS_EMR.SEQ_TREATNO.NEXTVAL, '" + (strPatid).Trim() + "' ,";
                        SQL += ComNum.VBLF + "'I' ,";//                          'CLASS
                        SQL += ComNum.VBLF + "'" + reader.GetValue(1).ToString().Trim() + "' ,";// 'INDATE
                        SQL += ComNum.VBLF + "'" + strDept + "' ,";// 'CLINCODE
                        SQL += ComNum.VBLF + "'" + reader.GetValue(2).ToString().Trim() + "' ,";// 'OUTDATE
                        SQL += ComNum.VBLF + "'" + VB.Val(reader.GetValue(5).ToString().Trim()) + "',  ";// 'DOCCODE
                        SQL += ComNum.VBLF + "'0',  ";//                         'ERFLAG
                        SQL += ComNum.VBLF + "'000000',  ";//                      'INITTIME
                        SQL += ComNum.VBLF + "'" + (strPatid).Trim() + "',  ";//    'OLDPATID
                        SQL += ComNum.VBLF + "'2',  ";//     'FST
                        SQL += ComNum.VBLF + "'',  ";//                           'WARD
                        SQL += ComNum.VBLF + "'', ";//                            'ROOM
                        SQL += ComNum.VBLF + "'1') ";//                           'COMPLETE
                    }
                    else
                    {
                        if (reader2.Read())
                        {
                            SQL = " UPDATE KOSMOS_EMR.EMR_TREATT SET ";
                            SQL += ComNum.VBLF + "   CLINCODE = '" + strDept + "' ,";
                            SQL += ComNum.VBLF + "   DOCCODE = '" + VB.Val(reader.GetValue(5).ToString().Trim()) + "' ,";
                            SQL += ComNum.VBLF + "   OUTDATE = '" + reader.GetValue(2).ToString().Trim() + "' ";
                            SQL += ComNum.VBLF + "  WHERE ROWID = '" + reader2.GetValue(1).ToString().Trim() + "'";
                        }
                    }

                    reader2.Dispose();

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        ComFunc.MsgBoxEx(msgForm, SqlErr);
                        return rtnVal;
                    }
                    #endregion

                    #region ocs서버 업데이트. 사용시 풀기
                    SQL = " UPDATE kosmos_pmpa.MID_SUMMARY SET  EMR = '1'";
                    SQL += ComNum.VBLF + " WHERE ROWID = '" + reader.GetValue(4).ToString().Trim() + "' ";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref RowAffected, pDbCon);
                    if (string.IsNullOrWhiteSpace(SqlErr) == false)
                    {
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        ComFunc.MsgBoxEx(msgForm, SqlErr);
                        return rtnVal;
                    }
                    #endregion
                }

                reader.Dispose();

                rtnVal = true;
                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, "", pDbCon);
                ComFunc.MsgBoxEx(msgForm, ex.Message);
            }
            #endregion

            return rtnVal;
        }
        #endregion

        #region TIF 

        /// <summary>
        /// TIF 저장
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="ColorSave"></param>
        public static void TifSave(string strFileName)
        {
         
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            myImageCodecInfo = GetEncoderInfo("image/tiff");

            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(
                myEncoder,
                (long)EncoderValue.CompressionLZW);
            myEncoderParameters.Param[0] = myEncoderParameter;
            mBitmap.Save(strFileName, myImageCodecInfo, myEncoderParameters);
           
            if (mBitmap != null)
            {
                mBitmap.Dispose();
                mBitmap = null;
            }

            if (mGraphics != null)
            {
                mGraphics.Dispose();
                mGraphics = null;
            }
        }

        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        /// <summary>
        /// 컬러 이미지를 JPG압축하여 저장한다
        /// </summary>
        /// <param name="path"></param>
        /// <param name="img"></param>
        /// <param name="quality"></param>
        public static bool SaveJpeg(string path, Image myBitmap, long quality)
        {
            try
            {
                //Bitmap OutBitMap = AdjustBrightness((Bitmap)myBitmap, 20);

                // Encoder parameter for image quality 
                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                // JPEG image codec 
                ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;
                //((Image)OutBitMap).Save(path, jpegCodec, encoderParams);
                myBitmap.Save(path, jpegCodec, encoderParams);

                if (mBitmap != null)
                {
                    mBitmap.Dispose();
                    mBitmap = null;
                }

                if (mGraphics != null)
                {
                    mGraphics.Dispose();
                    mGraphics = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                if (mBitmap != null)
                {
                    mBitmap.Dispose();
                    mBitmap = null;
                }

                if (mGraphics != null)
                {
                    mGraphics.Dispose();
                    mGraphics = null;
                }
                
                clsDB.SaveSqlErrLog(ex.Message, "", clsDB.DbCon);
                //ComFunc.MsgBoxEx(this, ex.Message);
                return false;
            }
        }

        #endregion
    }
}
