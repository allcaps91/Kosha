using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using ComLibB;

namespace ComSupLibB
{
    public class clsSupPthlAtc
    {
        public int gintPrtOutDrug;      //원외처방전
        public int gintPrtBongTo;       //원외처방전
        public string gstrPrintData;    //약국ATC,원외처방 인쇄할 자료(일자+번호)
        public string gstrActiveForm;   //작업중인 화면(ATC,SLIP)

        public string gstrPath;         //C:\TR3000\TR3000.DAT
        public string gstrPath2;        //C:\TR3000\TR3000.DAT   '2010-03-09 김현욱 추가

        public string gstrTempValue;    //2011-01-05

        //========================================
        //2010-04-20
        //복약안내문 때문에 넣어둔겁니다.
        //혹시 nrinfo00.bas 를 추가할 일이 있으면 삭제하시면 됩니다.
        public string gstrWard;

        public string[] gstrBarCodeMain = new string[39];      //2D 바코드 생성용(약정보 제외)
        public string gstrBarCodeYak;
        public string[] gstrBarCode = new string[12];       //2D 바코드 생성용(약정보만)

        public struct OutDrugMst
        {
            public string strSlipDate;
            public int intSlipNo;
            public string strPano;
            public string strsName;
            public int intAge;
            public string strSex;
            public string strBDate;
            public string strDeptCode;
            public string strDrCode;
            public int intDrBunho;
            public string strBi;
            public string strJumin1;
            public string strJumin2;
            public string strflag;
            public string strPrtDept;
            public string strActDate;
            public string strPart;
            public int intSEQNO;
            public string strEntDate;
            public string strSendDate;
            public string strPrtDate;
            public string strDiease1;
            public string strDiease2;

            public string strDiease1_RO;   //2012-06-26
            public string strDiease2_RO;   //2012-06-26

            public string strRemark;
            public string strIpdOpd;
            public string strROWID;
            public string strPrtBun;
            public string strHapPrint;
            public string strGBV252;
            public string strRemarkDrug;
        }

        //약국 ATC용 용법 Table
        public struct TABLE_BAS_DOSAGE
        {
            public string strCode;
            public int intDiv;
            public string strGbn;
            public string strPattern;
            public string strName1;
            public string strName2;
            public string strGbPrtDiv;
            public string strGbPrtNal;
            public string strROWID;
        }

        //약국 ATC용 Table
        public struct TABLE_BAS_ATC
        {
            public string strPano;
            public string strsName;
            public string strBi;
            public string strDeptCode;
            public string strDrCode;
            public string strSex;
            public int intAge;
            public string strJumin;
            public string strSingu;
            public int intItemCnt;
            public string strROWID;

            public string strOcsROWID;     //ROWID
            public int intRP;              //RP
            public string strSuNext;       //수가(약품)코드
            public string strBun;          //약품구분
            public string strGbn;          //용법의 구분
            public double dblQty;          //1일투여량
            public double dblNal;          //투여일수
            public string strDev;          //Devide
            public int intDosage1;         //Atc용법
            public string strPattern;      //Atc Pattern
            public int intDosage2;         //봉투의 용법
            public string strSuNameK;      //약품의 명칭
            public string strDrRemark;     //의사Remark
        }

        //OCS ATC용 Table
        public struct TABLE_OCS_ATC
        {
            public string strBDate;
            public string strPano;
            public string strsName;
            public string strGBIO;
            public string strWardCode;
            public int intRoomCode;
            public string strBi;
            public string strDeptCode;
            public string strDrCode;
            public string strSex;
            public int intAge;
            public string strJumin;
            public string strJumin2;
            public string strSingu;
            public int intItemCnt;
            public string strPowder;

            public int intRP;                  //RP
            public string strSuNext;           //수가(약품)코드
            public string strBun;              //약품구분
            public string strGbn;              //용법의 구분
            public double dblQty;              //1일투여량
            public double dblNal;              //투여일수
            public string strDev;              //Devide
            public string strPatternFLAG;      //Pattern '0'=00000000
            public string strDosage1;          //Atc용법
            public string strPattern;          //Atc Pattern
            public string strDosage2;          //봉투의 용법
            public string strSuNameK;          //약품의 명칭
            public string strDrRemark;         //의사Remark
            public string strUnit;   	        //약품단위 => 1C 일 경우 약하나당 RP1개. 무조건 60이상부터
        }

        public TABLE_BAS_DOSAGE Clear_TABLE_BAS_DOSAGE(TABLE_BAS_DOSAGE TBD)
        {
            TBD.strCode = "";
            TBD.intDiv = 0;
            TBD.strGbn = "";
            TBD.strPattern = "";
            TBD.strName1 = "";
            TBD.strName2 = "";
            TBD.strGbPrtDiv = "";
            TBD.strGbPrtNal = "";
            TBD.strROWID = "";

            return TBD;
        }

        public OutDrugMst Clear_OutDrugMst(OutDrugMst TOM)
        {
            TOM.strSlipDate = "";
            TOM.intSlipNo = 0;
            TOM.strPano = "";
            TOM.strsName = "";
            TOM.intAge = 0;
            TOM.strSex = "";
            TOM.strBDate = "";
            TOM.strDeptCode = "";
            TOM.strDrCode = "";
            TOM.intDrBunho = 0;
            TOM.strBi = "";
            TOM.strJumin1 = "";
            TOM.strJumin2 = "";
            TOM.strflag = "";
            TOM.strPrtDept = "";
            TOM.strActDate = "";
            TOM.strPart = "";
            TOM.intSEQNO = 0;
            TOM.strEntDate = "";
            TOM.strSendDate = "";
            TOM.strPrtDate = "";
            TOM.strDiease1 = "";
            TOM.strDiease2 = "";

            TOM.strDiease1_RO = "";   //2012-06-26
            TOM.strDiease2_RO = "";   //2012-06-26

            TOM.strRemark = "";
            TOM.strIpdOpd = "";
            TOM.strROWID = "";
            TOM.strPrtBun = "";
            TOM.strHapPrint = "";
            TOM.strGBV252 = "";
            TOM.strRemarkDrug = "";

            return TOM;
        }

        public TABLE_BAS_DOSAGE OCS_Dosage_READ(string strCode)
        {
            string strSql = "";
            DataTable dt = null;

            TABLE_BAS_DOSAGE TBD = new TABLE_BAS_DOSAGE();

            TBD = Clear_TABLE_BAS_DOSAGE(TBD);

            try
            {
                strSql = "";
                strSql = "SELECT ROWID TBDRowid,Bun,GbDiv,AtcCode, ";
                strSql = strSql + ComNum.VBLF + "   LabelName1,LabelName2,GbPrtDiv,GbPrtNal  ";
                strSql = strSql + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_ODOSAGE ";
                strSql = strSql + ComNum.VBLF + "   WHERE DosCode = '" + strCode + "' ";

                dt = clsDB.GetDataTable(strSql);

                if(dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return TBD;
                }
                if(dt.Rows.Count == 0)
                {
                    TBD.strCode = "<ERR>";
                    TBD.intDiv = 0;
                    TBD.strGbn = "0";
                    TBD.strPattern = "00000000";

                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return TBD;
                }

                if(VB.Val(strCode) != 0)
                {
                    TBD.intDiv = Convert.ToInt32(VB.Val(dt.Rows[0]["GBDIV"].ToString().Trim()));
                }

                TBD.strCode = strCode;

                switch(dt.Rows[0]["BUN"].ToString().Trim())
                {
                    case "11":
                        TBD.strGbn = "1";
                        break;
                    default:
                        TBD.strGbn = "2";
                        break;
                }

                TBD.strPattern = dt.Rows[0]["ATCCODE"].ToString().Trim();
                TBD.strName1 = dt.Rows[0]["LABELNAME1"].ToString().Trim();
                TBD.strName2 = dt.Rows[0]["LABELNAME2"].ToString().Trim();
                TBD.strGbPrtDiv = dt.Rows[0]["GBPRTDIV"].ToString().Trim();
                TBD.strGbPrtNal = dt.Rows[0]["GBPRTNAL"].ToString().Trim();
                TBD.strROWID = dt.Rows[0]["TBDROWID"].ToString().Trim();

                return TBD;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return TBD;
            }
        }

        //원외처방전 마스타를 READ
        public OutDrugMst READ_OutDrugMst(string strDate, int intSlipNo, string strPano = "")
        {
            string strSql = "";
            DataTable dt = null;

            OutDrugMst TOM = new OutDrugMst();

            TOM = Clear_OutDrugMst(TOM);

            try
            {
                strSql = "";
                strSql = "SELECT a.Pano,b.Sname,a.DeptCode,a.DrCode,a.SlipNo,a.Bi,";
                strSql = strSql + ComNum.VBLF + "       TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,";
                strSql = strSql + ComNum.VBLF + "       TO_CHAR(a.EntDate,'YYYY-MM-DD HH24:MI') EntDate,";
                strSql = strSql + ComNum.VBLF + "       TO_CHAR(a.SendDate,'YYYY-MM-DD HH24:MI') SendDate,";
                strSql = strSql + ComNum.VBLF + "       TO_CHAR(a.PrtDate,'YYYY-MM-DD HH24:MI') PrtDate,";
                strSql = strSql + ComNum.VBLF + "       TO_CHAR(a.ActDate,'YYYY-MM-DD') ActDate,a.Part,a.SeqNo, A.GBV252, ";
                strSql = strSql + ComNum.VBLF + "       a.Flag,a.PrtDept,a.Remark,a.Diease1,a.Diease2,a.Diease1_RO,a.Diease2_RO,a.ROWID,";
                strSql = strSql + ComNum.VBLF + "       a.DrBunho,a.IpdOpd,b.Sex,b.Jumin1,b.Jumin2, b.jumin3, a.PrtBun, a.HapPrint, A.REMARKDRUG ";
                strSql = strSql + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_OUTDRUGMST a,KOSMOS_PMPA.BAS_PATIENT b ";
                strSql = strSql + ComNum.VBLF + "   WHERE a.SlipDate=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                strSql = strSql + ComNum.VBLF + "   AND a.SlipNo = " + intSlipNo;

                if(strPano != "")
                {
                    strSql = strSql + ComNum.VBLF + "  AND A.PANO = '" + strPano + "' ";
                }

                strSql = strSql + ComNum.VBLF + "   AND a.Pano=b.Pano(+) ";

                dt = clsDB.GetDataTable(strSql);

                if(dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return TOM;
                }
                if(dt.Rows.Count == 0)
                {
                    TOM.strSlipDate = strDate;
                    TOM.intSlipNo = intSlipNo;

                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    TOM.strSlipDate = strDate;
                    TOM.intSlipNo = intSlipNo;
                    TOM.strPano = dt.Rows[0]["PANO"].ToString().Trim();
                    TOM.strsName = dt.Rows[0]["SNAME"].ToString().Trim();
                    TOM.strBDate = dt.Rows[0]["BDATE"].ToString().Trim();
                    TOM.strJumin1 = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    TOM.strJumin2 = dt.Rows[0]["JUMIN3"].ToString().Trim();
                    TOM.intAge = ComFunc.AgeCalc(TOM.strJumin1 + TOM.strJumin2);
                    TOM.strSex = dt.Rows[0]["SEX"].ToString().Trim();
                    TOM.strDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    TOM.strDrCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                    TOM.strBi = dt.Rows[0]["BI"].ToString().Trim();
                    TOM.strflag = dt.Rows[0]["FLAG"].ToString().Trim();
                    TOM.strPrtDept = dt.Rows[0]["PRTDEPT"].ToString().Trim();
                    TOM.strActDate = dt.Rows[0]["ACTDATE"].ToString().Trim();
                    TOM.strPart = dt.Rows[0]["PART"].ToString().Trim();
                    TOM.intSEQNO = Convert.ToInt32(VB.Val(dt.Rows[0]["SEQNO"].ToString().Trim()));
                    TOM.strEntDate = dt.Rows[0]["ENTDATE"].ToString().Trim();
                    TOM.strSendDate = dt.Rows[0]["SENDDATE"].ToString().Trim();
                    TOM.strPrtDate = dt.Rows[0]["PRTDATE"].ToString().Trim();
                    TOM.strRemark = dt.Rows[0]["REMARK"].ToString().Trim();
                    TOM.strDiease1 = dt.Rows[0]["DIEASE1"].ToString().Trim();
                    TOM.strDiease2 = dt.Rows[0]["DIEASE2"].ToString().Trim();

                    TOM.strDiease1_RO = dt.Rows[0]["DIEASE1_RO"].ToString().Trim();
                    TOM.strDiease2_RO = dt.Rows[0]["DIEASE2_RO"].ToString().Trim();

                    TOM.intDrBunho = Convert.ToInt32(VB.Val(dt.Rows[0]["DRBUNHO"].ToString().Trim()));
                    TOM.strIpdOpd = dt.Rows[0]["IPDOPD"].ToString().Trim();
                    TOM.strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    TOM.strPrtBun = dt.Rows[0]["PRTBUN"].ToString().Trim();
                    TOM.strHapPrint = dt.Rows[0]["HAPPRINT"].ToString().Trim();
                    TOM.strGBV252 = dt.Rows[0]["GBV252"].ToString().Trim();
                    TOM.strRemarkDrug = dt.Rows[0]["REMARKDRUG"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if(TOM.strDiease1.Trim() == "")
                {
                    strSql = "";
                    strSql = "SELECT IllCode FROM KOSMOS_OCS.OCS_OILLS ";
                    strSql = strSql + ComNum.VBLF + "   WHERE Ptno = '" + strPano + "' ";
                    strSql = strSql + ComNum.VBLF + "   AND BDate = TO_DATE('" + TOM.strBDate + "','YYYY-MM-DD') ";
                    strSql = strSql + ComNum.VBLF + "   AND DeptCode = '" + TOM.strDeptCode + "' ";
                    strSql = strSql + ComNum.VBLF + "ORDER BY SeqNo ";

                    dt = clsDB.GetDataTable(strSql);

                    if(dt == null)
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        Cursor.Current = Cursors.Default;
                        return TOM;
                    }
                    if(dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        Cursor.Current = Cursors.Default;
                    }
                    else if(dt.Rows.Count == 1)
                    {
                        TOM.strDiease1 = dt.Rows[0]["IllCode"].ToString().Trim();

                        dt.Dispose();
                        dt = null;
                    }
                    else
                    {
                        TOM.strDiease2 = dt.Rows[1]["IllCode"].ToString().Trim();

                        dt.Dispose();
                        dt = null;
                    }

                    clsDB.setBeginTran();

                    if(TOM.strDiease1.Trim() != "" && TOM.strDiease2.Trim() == "")
                    {
                        strSql = "";
                        strSql = "UPDATE KOSMOS_OCS.OCS_OUTDRUGMST";
                        strSql = strSql + ComNum.VBLF + "   SET ";
                        strSql = strSql + ComNum.VBLF + "       Diease1 = '" + TOM.strDiease1.Trim() + "' ";
                        strSql = strSql + ComNum.VBLF + "WHERE SlipDate=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        strSql = strSql + ComNum.VBLF + "   AND SlipNo = " + intSlipNo;
                        strSql = strSql + ComNum.VBLF + "   AND PANO = '" + strPano + "'";
                    }
                    else if(TOM.strDiease1.Trim() != "" && TOM.strDiease2.Trim() != "")
                    {
                        strSql = "";
                        strSql = "UPDATE KOSMOS_OCS.OCS_OUTDRUGMST";
                        strSql = strSql + ComNum.VBLF + "   SET ";
                        strSql = strSql + ComNum.VBLF + "       Diease1 = '" + TOM.strDiease1.Trim() + "', ";
                        strSql = strSql + ComNum.VBLF + "       Diease2 = '" + TOM.strDiease2.Trim() + "' ";
                        strSql = strSql + ComNum.VBLF + "WHERE SlipDate=TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                        strSql = strSql + ComNum.VBLF + "   AND SlipNo = " + intSlipNo;
                        strSql = strSql + ComNum.VBLF + "   AND PANO = '" + strPano + "' ";
                    }

                    clsDB.ExecuteNonQuery(strSql);
                    clsDB.setCommitTran();
                }

                if(TOM.intDrBunho > 0)
                {
                    return TOM;
                }

                //전공의
                if(VB.Right(TOM.strDrCode, 2) == "99")
                {
                    return TOM;
                }

                //의사 면허번호
                strSql = "";
                strSql = "SELECT DrBunho FROM KOSMOS_OCS.OCS_DOCTOR";
                strSql = strSql + ComNum.VBLF + "   WHERE DRCODE = '" + TOM.strDrCode + "' ";

                dt = clsDB.GetDataTable(strSql);

                if(dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return TOM;
                }
                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return TOM;
                }
                else
                {
                    TOM.intDrBunho = Convert.ToInt32(VB.Val(dt.Rows[0]["DRBUNHO"].ToString().Trim()));

                    dt.Dispose();
                    dt = null;
                }

                return TOM;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return TOM;
            }
        }
    }
}
